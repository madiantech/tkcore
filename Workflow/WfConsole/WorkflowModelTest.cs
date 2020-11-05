using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.SimpleWorkflow;
using YJC.Toolkit.Sys;

namespace WfConsole
{
    public static class WorkflowModelTest
    {
        public static void CreateModel()
        {
            BaseGlobalVariable.Current.UserInfo = new SimpleUserInfo("sa", "sa", 1, 1);
            using (EmptyDbDataSource source = new EmptyDbDataSource())
            using (WorkflowDefResolver resolver = new WorkflowDefResolver(source))
            {
                DataRow row = resolver.NewRow();
                row.BeginEdit();
                row["Id"] = resolver.CreateUniId();
                row["ShortName"] = "Hello";
                row["Name"] = "Hello, world";
                row["Content"] = DBNull.Value;
                row["IsUsed"] = 0;
                resolver.UpdateTrackField(UpdateKind.Insert, row);
                row.EndEdit();

                resolver.SetCommands(AdapterCommand.Insert);
                resolver.UpdateDatabase();
            }
        }

        private static void CreateModel(WorkflowConfig config)
        {
            BaseGlobalVariable.Current.UserInfo = new SimpleUserInfo("sa", "sa", 1, 1);
            using (EmptyDbDataSource source = new EmptyDbDataSource())
            using (WorkflowDefResolver resolver = new WorkflowDefResolver(source))
            {
                resolver.Insert(config);
            }
        }

        public static void CreateModel2()
        {
            CreateModel(CreateSampleConfig1());
        }

        public static void CreateModel3()
        {
            CreateModel(CreateSampleConfig2());
        }

        public static void CreateModel4()
        {
            CreateModel(CreateSampleConfig3());
        }

        public static void CreateModel5()
        {
            CreateModel(CreateSampleConfig4());
        }

        public static void CreateModel6()
        {
            CreateModel(CreateSampleConfig5());
        }

        public static void CreateModel7()
        {
            CreateModel(CreateSampleConfig6());
        }

        public static void CreateModel8()
        {
            CreateModel(CreateSampleConfig7());
        }

        public static void CreateModel9()
        {
            CreateModel(CreateSampleConfig8());
        }

        public static void UpdateModel()
        {
            UpdateModel(CreateSampleConfig4());
        }

        private static void UpdateModel(WorkflowConfig config)
        {
            BaseGlobalVariable.Current.UserInfo = new SimpleUserInfo("sa", "sa", 1, 1);
            using (EmptyDbDataSource source = new EmptyDbDataSource())
            using (WorkflowDefResolver resolver = new WorkflowDefResolver(source))
            {
                resolver.Update(config);
            }
        }

        public static void UpdateModel3()
        {
            UpdateModel(CreateSampleConfig3());
        }

        public static void GetConfig()
        {
            TkDbContext context = DbContextUtil.CreateDefault();
            WorkflowConfig wfConfig = CacheManager.GetItem("WorkflowConfig", "Hello",
                context).Convert<WorkflowConfig>();
            Console.Write(wfConfig.CreateXml());
        }

        public static void TestJson()
        {
            TkDbContext context = DbContextUtil.CreateDefault();
            WorkflowConfig wfConfig = CacheManager.GetItem("WorkflowConfig", "Hello_Manual",
                context).Convert<WorkflowConfig>();
            string json = wfConfig.CreateJson();
            wfConfig = WorkflowConfig.ReadJson(json);
            string json2 = wfConfig.CreateJson();
            string xml = wfConfig.CreateXml();
            Console.WriteLine(json == json2);
        }

        private static WorkflowConfig CreateSampleConfig1()
        {
            WorkflowConfig wf = new WorkflowConfig()
            {
                Name = "Hello",
                MainTableColumnPrefix = string.Empty,
                DisplayName = "测试1",
                Priority = WorkflowPriority.Normal
            };
            BeginStepConfig begin = new InternalBeginStepConfig(wf)
            {
                CreatorConfig = "{'RegNameCreator':{'Content':'TestBegin'}}" //"<RegNameCreator>TestBegin</RegNameCreator>"
            };
            EndStepConfig end = new InternalEndStepConfig(wf)
            {
                Name = "End1"
            };
            begin.AddNextConfig(end);
            wf.Steps.Add(begin);
            wf.Steps.Add(end);

            return wf;
        }

        private static WorkflowConfig CreateSampleConfig2()
        {
            WorkflowConfig wf = new WorkflowConfig()
            {
                Name = "Hello1",
                MainTableColumnPrefix = string.Empty,
                DisplayName = "测试2",
                Priority = WorkflowPriority.Normal
            };
            BeginStepConfig begin = new InternalBeginStepConfig(wf)
            {
                CreatorConfig = "{'RegNameCreator':{'Content':'TestBegin'}}"
            };
            AutoStepConfig auto = new InternalAutoStepConfig(wf)
            {
                Name = "Auto1",
                ProcessorConfig = "{'RegNameAutoProcessor':{'Content':'Test'}}"//"<tk:RegNameAutoProcessor>Test</tk:RegNameAutoProcessor>",
            };
            EndStepConfig end = new InternalEndStepConfig(wf)
            {
                Name = "End1"
            };
            begin.AddNextConfig(auto);
            auto.AddNextConfig(end);
            wf.Steps.Add(begin);
            wf.Steps.Add(end);
            wf.Steps.Add(auto);

            return wf;
        }

        private static WorkflowConfig CreateSampleConfig3()
        {
            WorkflowConfig wf = new WorkflowConfig()
            {
                Name = "Hello2",
                MainTableColumnPrefix = string.Empty,
                DisplayName = "测试3",
                Priority = WorkflowPriority.Normal
            };
            BeginStepConfig begin = new InternalBeginStepConfig(wf)
            {
                CreatorConfig = "{'RegNameCreator':{'Content':'TestBegin'}}"
            };
            RouteStepConfig route = new InternalRouteStepConfig(wf)
            {
                Name = "Route1",
                FillMode = FillContentMode.MainOnly
            };
            ConnectionConfig conn1 = new ConnectionConfig()
            {
                Name = "Route1_Conn1",
                DisplayName = "Yes",
                ExpressionType = CalculationType.Expression,
                Expression = "DataSetExtension.GetString(dataRow, 'Name') == 'Hello'",
                NextStepName = "Auto1"
            };
            ConnectionConfig conn2 = new ConnectionConfig()
            {
                Name = "Route1_Conn2",
                DisplayName = "No",
                ExpressionType = CalculationType.Expression,
                Expression = "DataSetExtension.GetString(dataRow, 'Name') != 'Hello'",
                NextStepName = "Auto2"
            };
            route.AddConnectionConfig(conn1);
            route.AddConnectionConfig(conn2);
            AutoStepConfig auto = new InternalAutoStepConfig(wf)
            {
                Name = "Auto1",
                ProcessorConfig = "{'RegNameAutoProcessor':{'Content':'Test'}}",
            };
            AutoStepConfig auto2 = new InternalAutoStepConfig(wf)
            {
                Name = "Auto2",
                ProcessorConfig = "{'RegNameAutoProcessor':{'Content':'Test2'}}",
            };
            EndStepConfig end = new InternalEndStepConfig(wf)
            {
                Name = "End1"
            };
            begin.AddNextConfig(route);
            route.AddNextConfig(auto);
            route.AddNextConfig(auto2);
            auto.AddNextConfig(end);
            auto2.AddNextConfig(end);
            wf.Steps.Add(begin);
            wf.Steps.Add(end);
            wf.Steps.Add(route);
            wf.Steps.Add(auto);
            wf.Steps.Add(auto2);

            return wf;
        }

        private static WorkflowConfig CreateSampleConfig4()
        {
            WorkflowConfig wf = new WorkflowConfig()
            {
                Name = "Hello_Manual",
                MainTableColumnPrefix = string.Empty,
                DisplayName = "测试人工",
                Priority = WorkflowPriority.Normal,
                ContentXmlConfig = @"{'XmlFile':{'FileName':'Test\\Test1Content.xml'}}"// @"<tk:XmlFile FileName='Test\Test1Content.xml'/>"
            };
            BeginStepConfig begin = new InternalBeginStepConfig(wf)
            {
                CreatorConfig = "{'RegNameCreator':{'Content':'TestBegin'}}"
            };
            ManualStepConfig manual = new InternalManualStepConfig(wf)
            {
                Name = "Manual1",
                DisplayName = "审批测试",
                ContainsSave = true,
                ContentXmlConfig = @"{'XmlFile':{'FileName':'Test\\Test1Content.xml'}}",//@"<tk:XmlFile FileName='Test\Test1Content.xml'/>",
                ProcessXmlConfig = @"{'XmlFile':{'FileName':'Test\\Test1Process.xml'}}",//@"<tk:XmlFile FileName='Test\Test1Process.xml'/>",
                ActorConfig = "{'Creator':{}}",//"<tk:Creator/>",
                Process = new ProcessConfig
                {
                    UIOperation = new UIOperationConfig
                    {
                        Name = "Approve",
                        DisplayName = "审批",
                        ButtonCaption = "审批",
                        PlugIn = "{'ProcessXml':{}}"//"<tk:ProcessXml/>"
                    }
                }
            };
            //manual.AddNotify("{'RegNameNotifyAction':{'Content':'Test'}}"); //"<tk:RegNameNotifyAction>Test</tk:RegNameNotifyAction>");
            EndStepConfig end = new InternalEndStepConfig(wf)
            {
                Name = "End1"
            };
            //end.AddNotify("{'RegNameNotifyAction':{'Content':'Test'}}");
            begin.AddNextConfig(manual);
            manual.AddNextConfig(end);
            wf.Steps.Add(begin);
            wf.Steps.Add(end);
            wf.Steps.Add(manual);

            return wf;
        }

        private static WorkflowConfig CreateSampleConfig5()
        {
            WorkflowConfig wf = new WorkflowConfig()
            {
                Name = "Hello_Approve",
                MainTableColumnPrefix = string.Empty,
                DisplayName = "测试审批",
                Priority = WorkflowPriority.Normal,
                ContentXmlConfig = @"<tk:Approve><tk:XmlFile FileName='Test\Test1Content.xml'/></tk:Approve>"
            };
            BeginStepConfig begin = new InternalBeginStepConfig(wf)
            {
                CreatorConfig = "<RegNameCreator>TestBegin</RegNameCreator>"
            };
            ManualStepConfig manual = new InternalManualStepConfig(wf)
            {
                Name = "Manual1",
                DisplayName = "组长审批",
                UseApprove = true,
                ContainsSave = true,
                ContentXmlConfig = @"<tk:XmlFile FileName='Test\Test1Content.xml'/>",
                ProcessXmlConfig = @"<tk:Approve/>",
                ActorConfig = "<tk:Creator/>",
                //Actor = new StepActorConfig
                //{
                //    ActorType = ActorType.Organization,
                //    ActorInfo1 = "test1"
                //},
                //Actor = new StepActorConfig
                //{
                //    ActorType = ActorType.Custom,
                //    Calculation = CalculationType.Expression,
                //    Expression = "DataSetExtension.GetString(dataRow, 'BillNo')"
                //},
                Process = new ProcessConfig
                {
                    UIOperation = new UIOperationConfig
                    {
                        Name = "Approve",
                        DisplayName = "审批",
                        ButtonCaption = "审批",
                        PlugIn = "<tk:Approve/>"
                    }
                }
            };

            RouteStepConfig route = new InternalRouteStepConfig(wf)
            {
                Name = "Route1",
                FillMode = FillContentMode.MainOnly
            };
            ConnectionConfig conn1 = new ConnectionConfig()
            {
                Name = "Route1_Conn1",
                DisplayName = "Yes",
                ExpressionType = CalculationType.Expression,
                Expression = "DataSetExtension.GetString(workflowRow, 'CustomData') == '1'",
                NextStepName = "Manual3"
            };
            ConnectionConfig conn2 = new ConnectionConfig()
            {
                Name = "Route1_Conn2",
                DisplayName = "No",
                ExpressionType = CalculationType.Expression,
                Expression = "DataSetExtension.GetString(workflowRow, 'CustomData') != '1'",
                NextStepName = "Manual2"
            };
            route.AddConnectionConfig(conn1);
            route.AddConnectionConfig(conn2);
            ManualStepConfig manual2 = new InternalManualStepConfig(wf)
            {
                Name = "Manual2",
                DisplayName = "自己查看",
                ContentXmlConfig = @"<tk:Approve><tk:XmlFile FileName='Test\Test1Content.xml'/></tk:Approve>",
                ProcessXmlConfig = @"<tk:Empty />",
                ActorConfig = "<tk:Creator/>",
                //Actor = new StepActorConfig
                //{
                //    ActorType = ActorType.SingleUser,
                //    ActorInfo1 = "1001"
                //},
                //Actor = new StepActorConfig
                //{
                //    ActorType = ActorType.RoleCustomOrg,
                //    ActorInfo1 = "1",
                //    Calculation = CalculationType.Expression,
                //    Expression = "DataSetExtension.GetString(dataRow, 'Content')"
                //},
                Process = new ProcessConfig
                {
                    UIOperation = new UIOperationConfig
                    {
                        Name = "Verify",
                        DisplayName = "确认",
                        ButtonCaption = "确认",
                        PlugIn = "<tk:NextProcessor/>"
                    }
                }
            };
            ManualStepConfig manual3 = new InternalManualStepConfig(wf)
            {
                Name = "Manual3",
                DisplayName = "经理审批",
                ContentXmlConfig = @"<tk:Approve><tk:XmlFile FileName='Test\Test1Content.xml'/></tk:Approve>",
                ProcessXmlConfig = @"<tk:Approve/>",
                ActorConfig = "<tk:Creator/>",
                //Actor = new StepActorConfig
                //{
                //    ActorType = ActorType.OrgRole,
                //    ActorInfo1 = "child1",
                //    ActorInfo2 = "1"
                //},
                //Actor = new StepActorConfig
                //{
                //    ActorType = ActorType.Relative,
                //    ActorInfo1 = "1",
                //    RelationType = YJC.Toolkit.SimpleWorkflow.RelationType.SamePerson,
                //    RelativeUserType = RelativeUserType.Processor,
                //    RelativeStepName = "Manual1"
                //},
                Process = new ProcessConfig
                {
                    UIOperation = new UIOperationConfig
                    {
                        Name = "Approve",
                        DisplayName = "审批",
                        ButtonCaption = "审批",
                        PlugIn = "<tk:Approve/>"
                    }
                }
            };
            NonUIOperationConfig nonUi = new NonUIOperationConfig
            {
                Name = "Back",
                DisplayName = "退回",
                ButtonCaption = "退回",
                PlugIn = "Back",
                NeedPrompt = true
            };
            manual3.Process.AddNonUIOperationConfig(nonUi);
            nonUi = new NonUIOperationConfig
            {
                Name = "Abort",
                DisplayName = "终止",
                ButtonCaption = "终止",
                PlugIn = "Abort",
                NeedPrompt = true
            };
            manual3.Process.AddNonUIOperationConfig(nonUi);
            nonUi = new NonUIOperationConfig
            {
                Name = "Unlock",
                DisplayName = "反签收",
                ButtonCaption = "反签收",
                PlugIn = "Unlock",
                NeedPrompt = true
            };
            manual3.Process.AddNonUIOperationConfig(nonUi);

            RouteStepConfig route2 = new InternalRouteStepConfig(wf)
            {
                Name = "Route2",
                FillMode = FillContentMode.MainOnly
            };
            ConnectionConfig conn3 = new ConnectionConfig()
            {
                Name = "Route2_Conn1",
                DisplayName = "Yes",
                ExpressionType = CalculationType.Expression,
                Expression = "DataSetExtension.GetString(workflowRow, 'CustomData') == '1'",
                NextStepName = "End1"
            };
            ConnectionConfig conn4 = new ConnectionConfig()
            {
                Name = "Route2_Conn2",
                DisplayName = "No",
                ExpressionType = CalculationType.Expression,
                Expression = "DataSetExtension.GetString(workflowRow, 'CustomData') != '1'",
                NextStepName = "Manual2"
            };
            route2.AddConnectionConfig(conn3);
            route2.AddConnectionConfig(conn4);
            EndStepConfig end = new InternalEndStepConfig(wf)
            {
                Name = "End1"
            };
            begin.AddNextConfig(manual);
            manual.AddNextConfig(route);
            route.AddNextConfig(manual3);
            route.AddNextConfig(manual2);
            manual3.AddNextConfig(route2);
            route2.AddNextConfig(end);
            route2.AddNextConfig(manual2);
            manual2.AddNextConfig(end);
            wf.Steps.Add(begin);
            wf.Steps.Add(end);
            wf.Steps.Add(manual);
            wf.Steps.Add(manual2);
            wf.Steps.Add(manual3);
            wf.Steps.Add(route);
            wf.Steps.Add(route2);

            return wf;
        }

        private static WorkflowConfig CreateSampleConfig6()
        {
            WorkflowConfig wf = new WorkflowConfig()
            {
                Name = "Hello3",
                MainTableColumnPrefix = string.Empty,
                DisplayName = "测试4",
                Priority = WorkflowPriority.Normal
            };
            BeginStepConfig begin = new InternalBeginStepConfig(wf)
            {
                CreatorConfig = "<RegNameCreator>TestBegin</RegNameCreator>"
            };
            AutoStepConfig auto = new InternalAutoStepConfig(wf)
            {
                Name = "Auto1",
                ProcessorConfig = "<tk:RegNameAutoProcessor>CreateSubWf</tk:RegNameAutoProcessor>",
            };
            EndStepConfig end = new InternalEndStepConfig(wf)
            {
                Name = "End1"
            };
            begin.AddNextConfig(auto);
            auto.AddNextConfig(end);
            wf.Steps.Add(begin);
            wf.Steps.Add(end);
            wf.Steps.Add(auto);

            return wf;
        }

        private static WorkflowConfig CreateSampleConfig7()
        {
            WorkflowConfig wf = new WorkflowConfig()
            {
                Name = "CounterSign",
                MainTableColumnPrefix = string.Empty,
                DisplayName = "测试会签",
                Priority = WorkflowPriority.Normal,
                ProcessDisplay = ProcessDisplay.Parent,
                ContentXmlConfig = @"<tk:XmlFile FileName='CounterSign\CounterSignContentTotal.xml'/>"
            };
            BeginStepConfig begin = new InternalBeginStepConfig(wf)
            {
                DisplayName = "开始",
                CreatorConfig = "<RegNameCreator>CounterSignBegin</RegNameCreator>"
            };
            ManualStepConfig manual = new InternalManualStepConfig(wf)
            {
                Name = "Manual1",
                DisplayName = "审批测试",
                ContainsSave = true,
                ContentXmlConfig = @"<tk:XmlFile FileName='CounterSign\CounterSignContent.xml'/>",
                ProcessXmlConfig = @"<tk:XmlFile FileName='CounterSign\CounterSignProcess.xml'/>",
                ActorConfig = "<tk:Custom Expression=\"DataSetExtension.GetString(dataRow, 'Operator')\"/>",
                Process = new ProcessConfig
                {
                    UIOperation = new UIOperationConfig
                    {
                        Name = "CounterSign",
                        DisplayName = "会签",
                        ButtonCaption = "会签",
                        PlugIn = "<tk:ProcessXml/>"
                    }
                }
            };
            EndStepConfig end = new InternalEndStepConfig(wf)
            {
                DisplayName = "结束",
                Name = "End1"
            };
            begin.AddNextConfig(manual);
            manual.AddNextConfig(end);
            wf.Steps.Add(begin);
            wf.Steps.Add(end);
            wf.Steps.Add(manual);

            return wf;
        }

        private static WorkflowConfig CreateSampleConfig8()
        {
            WorkflowConfig wf = new WorkflowConfig()
            {
                Name = "Hello_CounterSign",
                MainTableColumnPrefix = string.Empty,
                DisplayName = "审批和会签",
                Priority = WorkflowPriority.Normal,
                ProcessDisplay = ProcessDisplay.Child,
                ContentXmlConfig = @"<tk:Approve><tk:XmlFile FileName='CounterSign\NormalContentTotal.xml'/></tk:Approve>"
            };
            BeginStepConfig begin = new InternalBeginStepConfig(wf)
            {
                DisplayName = "开始",
                CreatorConfig = "<RegNameCreator>TestBegin2</RegNameCreator>"
            };
            ManualStepConfig manual = new InternalManualStepConfig(wf)
            {
                Name = "Manual1",
                DisplayName = "组长审批",
                UseApprove = true,
                ContainsSave = true,
                ContentXmlConfig = @"<tk:XmlFile FileName='CounterSign\NormalContent.xml'/>",
                ProcessXmlConfig = @"<tk:Approve/>",
                ActorConfig = "<tk:Creator/>",
                //Actor = new StepActorConfig
                //{
                //    ActorType = ActorType.Organization,
                //    ActorInfo1 = "test1"
                //},
                //Actor = new StepActorConfig
                //{
                //    ActorType = ActorType.Custom,
                //    Calculation = CalculationType.Expression,
                //    Expression = "DataSetExtension.GetString(dataRow, 'BillNo')"
                //},
                Process = new ProcessConfig
                {
                    UIOperation = new UIOperationConfig
                    {
                        Name = "Approve",
                        DisplayName = "审批",
                        ButtonCaption = "审批",
                        PlugIn = "<tk:Approve/>"
                    }
                }
            };

            RouteStepConfig route = new InternalRouteStepConfig(wf)
            {
                Name = "Route1",
                DisplayName = "确认是否同意？",
                FillMode = FillContentMode.MainOnly
            };
            ConnectionConfig conn1 = new ConnectionConfig()
            {
                Name = "Route1_Conn1",
                DisplayName = "Yes",
                ExpressionType = CalculationType.Expression,
                Expression = "DataSetExtension.GetString(workflowRow, 'CustomData') == '1'",
                NextStepName = "Auto1"
            };
            ConnectionConfig conn2 = new ConnectionConfig()
            {
                Name = "Route1_Conn2",
                DisplayName = "No",
                ExpressionType = CalculationType.Expression,
                Expression = "DataSetExtension.GetString(workflowRow, 'CustomData') != '1'",
                NextStepName = "Manual2"
            };
            route.AddConnectionConfig(conn1);
            route.AddConnectionConfig(conn2);
            ManualStepConfig manual2 = new InternalManualStepConfig(wf)
            {
                Name = "Manual2",
                DisplayName = "自己查看",
                ContentXmlConfig = @"<tk:Approve><tk:XmlFile FileName='CounterSign\NormalContent.xml'/></tk:Approve>",
                ProcessXmlConfig = @"<tk:Empty />",
                ActorConfig = "<tk:Creator/>",
                //Actor = new StepActorConfig
                //{
                //    ActorType = ActorType.SingleUser,
                //    ActorInfo1 = "1001"
                //},
                //Actor = new StepActorConfig
                //{
                //    ActorType = ActorType.RoleCustomOrg,
                //    ActorInfo1 = "1",
                //    Calculation = CalculationType.Expression,
                //    Expression = "DataSetExtension.GetString(dataRow, 'Content')"
                //},
                Process = new ProcessConfig
                {
                    UIOperation = new UIOperationConfig
                    {
                        Name = "Verify",
                        DisplayName = "确认",
                        ButtonCaption = "确认",
                        PlugIn = "<tk:NextProcessor/>"
                    }
                }
            };

            AutoStepConfig auto = new InternalAutoStepConfig(wf)
            {
                Name = "Auto1",
                DisplayName = "创建会签",
                ProcessorConfig = "<tk:CounterSignProcessor SubWorkflowName='CounterSign'><tk:TitleExpression>DataSetExtension.GetString(dataRow, 'BillNo')</tk:TitleExpression><tk:CounterSignUserConfig><tk:Composite><tk:Creator/><tk:SingleUser UserId='1001'/></tk:Composite></tk:CounterSignUserConfig></tk:CounterSignProcessor>",
            };

            MergeStepConfig merge = new InternalMergeStepConfig(wf)
            {
                Name = "Merge1",
                ContentXmlConfig = @"<tk:Approve><tk:XmlFile FileName='CounterSign\NormalContentTotal.xml'/></tk:Approve>",
                DisplayName = "等待会签",
                MergerConfig = "<tk:AllFinish/>"
            };
            ManualStepConfig manual3 = new InternalManualStepConfig(wf)
            {
                Name = "Manual3",
                DisplayName = "某人查看",
                ContentXmlConfig = @"<tk:Approve><tk:XmlFile FileName='CounterSign\NormalContentTotal.xml'/></tk:Approve>",
                ProcessXmlConfig = @"<tk:Empty />",
                ActorConfig = "<tk:Creator/>",
                //Actor = new StepActorConfig
                //{
                //    ActorType = ActorType.SingleUser,
                //    ActorInfo1 = "1001"
                //},
                //Actor = new StepActorConfig
                //{
                //    ActorType = ActorType.RoleCustomOrg,
                //    ActorInfo1 = "1",
                //    Calculation = CalculationType.Expression,
                //    Expression = "DataSetExtension.GetString(dataRow, 'Content')"
                //},
                Process = new ProcessConfig
                {
                    UIOperation = new UIOperationConfig
                    {
                        Name = "Verify",
                        DisplayName = "确认",
                        ButtonCaption = "确认",
                        PlugIn = "<tk:NextProcessor/>"
                    }
                }
            };
            EndStepConfig end = new InternalEndStepConfig(wf)
            {
                DisplayName = "结束",
                Name = "End1"
            };
            begin.AddNextConfig(manual);
            manual.AddNextConfig(route);
            route.AddNextConfig(manual2);
            manual2.AddNextConfig(end);
            route.AddNextConfig(auto);
            auto.AddNextConfig(merge);
            merge.AddNextConfig(manual3);
            manual3.AddNextConfig(end);

            wf.Steps.Add(begin);
            wf.Steps.Add(end);
            wf.Steps.Add(manual);
            wf.Steps.Add(manual2);
            wf.Steps.Add(manual3);
            wf.Steps.Add(route);
            wf.Steps.Add(auto);
            wf.Steps.Add(merge);
            return wf;
        }

        public static void CreateSampleData()
        {
            Tk5DataXml dataXml = Tk5DataXml.Create(@"TestWf/Requirement.xml");
            BaseGlobalVariable.Current.UserInfo = new SimpleUserInfo("sa", "sa", 1, 1);
            using (EmptyDbDataSource source = new EmptyDbDataSource())
            using (Tk5TableResolver resolver = new Tk5TableResolver(dataXml, source))
            {
                DataRow row = resolver.NewRow();
                row.BeginEdit();
                row["Id"] = resolver.CreateUniId();
                row["BillNo"] = "Hello-" + row["Id"].ToString().PadLeft(5, '0');
                row["Name"] = "Hello";
                row["FaqDate"] = DateTime.Today;
                row["Content"] = "Hello, world";
                resolver.UpdateTrackField(UpdateKind.Insert, row);
                row.EndEdit();
                resolver.SetCommands(AdapterCommand.Insert);

                resolver.UpdateDatabase();
            }
        }

        public static void TestSampleWf1()
        {
            bool value = WorkflowUtil.CreateWorkflow(DbContextUtil.CreateDefault(), "Hello", "1", null, "Id", "1");
            Console.Write(value);
        }

        public static void TestSampleWf2()
        {
            bool value = WorkflowUtil.CreateWorkflow(DbContextUtil.CreateDefault(), "Hello1", "1", null, "Id", "1");
            Console.Write(value);
        }

        public static void TestSampleWf3()
        {
            bool value = WorkflowUtil.CreateWorkflow(DbContextUtil.CreateDefault(), "Hello2", "1", null, "Id", "1");
            Console.Write(value);
        }

        public static void TestSampleWf4()
        {
            bool value = WorkflowUtil.CreateWorkflow(DbContextUtil.CreateDefault(), "Hello_Manual", "1", null, "Id", "1");
            Console.Write(value);
        }

        public static void TestSampleWf6()
        {
            bool value = WorkflowUtil.CreateWorkflow(DbContextUtil.CreateDefault(), "Hello3", "1", null, "Id", "3");
            Console.Write(value);
        }

        public static void RerunSampleWf1(int id)
        {
            Workflow wf = Workflow.CreateWorkflow(DbContextUtil.CreateDefault(), id.ToString());
            wf.Run();
        }

        public static void TestXml()
        {
            string xml = "<tk:CounterSignProcessor SubWorkflowName='CounterSign'><tk:TitleExpression>DataSetExtension.GetString(dataRow, 'BillNo')</tk:TitleExpression><tk:CounterSignUserConfig><tk:Composite><tk:Creator/><tk:SingleUser UserId='1001'/></tk:Composite></tk:CounterSignUserConfig></tk:CounterSignProcessor>";
            var obj = xml.ReadXmlFromFactory(AutoProcessorConfigFactory.REG_NAME);
            if (obj != null)
            {
                xml = obj.WriteXml();
                Trace.Write(xml);
                string json = obj.WriteJson();
                Trace.Write(json);
            }
        }
    }
}