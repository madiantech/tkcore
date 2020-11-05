using System;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class RouteStep : Step
    {
        public RouteStep(Workflow workflow, StepConfig config)
            : base(workflow, config)
        {
        }

        protected override bool Execute()
        {
            RouteStepConfig config = Config as RouteStepConfig;
            WorkflowContent content = WorkflowInstResolver.CreateContent(WorkflowRow);

            using (content)
            {
                //WorkflowRow.BeginEdit();
                content.FillWithMainData(config.FillMode, Source);
                //先检查一下 是否指定了下一个步骤
                if (string.IsNullOrEmpty(WorkflowRow["AdminData"].ToString()))
                {
                    SetRouteConnection(config, content);
                }
                else
                {
                    WorkflowRow["CustomData"] = WorkflowRow["AdminData"];
                    WorkflowRow["AdminData"] = DBNull.Value;
                }
                WorkflowRow["Status"] = (int)StepState.ProcessNotSend;
                // WorkflowRow.EndEdit();
                WorkflowResolver.SetCommands(AdapterCommand.Update);
                UpdateUtil.UpdateTableResolvers(Source.Context, null, WorkflowResolver);
            }
            return true;
        }

        private void SetRouteConnection(RouteStepConfig config, WorkflowContent content)
        {
            string nextStep = null;
            foreach (ConnectionConfig connection in config.Connections)
            {
                bool isMatch = false;
                switch (connection.ExpressionType)
                {
                    case CalculationType.Expression:
                        try
                        {
                            //string expression = Expression.Execute(connection, Source);

                            //isMatch = (bool)content.MainTableResolver.HostTable.Compute(expression, null);
                            isMatch = StepUtil.ExecuteExpression<bool>(connection.Expression,
                                content.MainRow, WorkflowRow);
                        }
                        catch (Exception ex)
                        {
                            throw new NoRouteException(config, config.Error, ex);
                            //TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                            //   "路由步骤{0}的宏表达式{1} 运算错误", config.Name, expression),
                            //   new NoRouteException(config, config.Error, ex), this);
                        }
                        break;

                    case CalculationType.PlugIn:
                        try
                        {
                            IConnection plugIn = PlugInFactoryManager.CreateInstance<IConnection>(
                                ConnectionPlugInFactory.REG_NAME, connection.Expression);

                            isMatch = plugIn.Match(content.MainRow, Source);
                        }
                        catch (Exception ex)
                        {
                            throw new NoRouteException(config, config.Error, ex);
                            //TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                            // "路由步骤{0}的连接线插件{1} 匹配错误", config.Name, connection.Expression),
                            // new NoRouteException(config, config.Error, ex), this);
                        }
                        break;
                }
                if (isMatch)
                {
                    nextStep = connection.NextStepName;
                    break;
                }
            }

            //TkDebug.AssertNotNull(nextStep, string.Format(ObjectUtil.SysCulture,
            //     "路由步骤{0} 的下一个步骤不能为空", config.Name), this);
            if (string.IsNullOrEmpty(nextStep))
            {
                throw new NoRouteException(config, config.Error);
            }
            //下一个步骤名
            WorkflowRow["CustomData"] = nextStep;
        }

        protected override void Send(StepConfig nextStep)
        {
            StepUtil.SendStep(Workflow, nextStep);
        }

        internal override State GetState(StepState state)
        {
            switch (state)
            {
                case StepState.NotReceive:
                    return RouteNRState.Instance;

                case StepState.ReceiveNotOpen:
                    return RouteRNOState.Instance;

                case StepState.OpenNotProcess:
                    return RouteONPState.Instance;

                case StepState.ProcessNotSend:
                    return RoutePNSState.Instance;

                case StepState.Mistake:
                    return RouteMState.Instance;

                default:
                    TkDebug.ThrowImpossibleCode(this);
                    return null;
            }
        }
    }
}