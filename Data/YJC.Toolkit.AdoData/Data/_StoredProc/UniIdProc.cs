using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal sealed class UniIdProc : StoredProc
    {
        internal class IdPair
        {
            public int Current { get; set; }
            public int Max { get; set; }
        }

        private readonly static Dictionary<string, IdPair> fIdTable = new Dictionary<string, IdPair>();

        private UniIdProc()
            : base("sp_get_uni_id")
        {
            AddParameter("NAME", ParameterDirection.Input, TkDataType.String, 255);
            AddParameter("STEP", ParameterDirection.Input, TkDataType.Int, 0, 1);
            AddParameter("VALUE", ParameterDirection.Output, TkDataType.String, 255);
        }

        public UniIdProc(TkDbContext context)
            : this()
        {
            Context = context;
        }

        public string Name
        {
            get
            {
                TkDebug.ThrowToolkitException(
                    "属性Name是Input参数不能读取，提供Get是为了屏蔽微软的规则检查", this);
                return null;
            }
            set
            {
                this["NAME"] = value;
            }
        }

        public int Step
        {
            get
            {
                TkDebug.ThrowToolkitException(
                    "属性Step是Input参数不能读取，提供Get是为了屏蔽微软的规则检查", this);
                return 0;
            }
            set
            {
                this["STEP"] = value;
            }
        }

        public string Value
        {
            get
            {
                return GetParamValue<string>("VALUE");
            }
        }

        public static string ExecuteProc(string name, TkDbContext context)
        {
            using (UniIdProc proc = new UniIdProc(context) { Name = name })
            {
                proc.Execute();
                return proc.Value;
            }
        }

        public static string ExecuteProcWithStep(string name, TkDbContext context)
        {
            using (UniIdProc proc = new UniIdProc(context)
            {
                Name = name,
                Step = context.ContextConfig.IdStep
            })
            {
                proc.Execute();
                return proc.Value;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static string ExecuteCache(string name, TkDbContext context)
        {
            int step = context.ContextConfig.IdStep;
            IdPair pair;
            if (fIdTable.TryGetValue(name, out pair))
            {
                if (++pair.Current >= pair.Max)
                {
                    pair.Current = int.Parse(ExecuteProcWithStep(name, context),
                        ObjectUtil.SysCulture);
                    pair.Max = pair.Current + step - 1;
                }
                return pair.Current.ToString(ObjectUtil.SysCulture);
            }
            else
            {
                int value = int.Parse(ExecuteProcWithStep(name, context),
                    ObjectUtil.SysCulture);
                fIdTable.Add(name, new IdPair { Current = value, Max = value + step - 1 });
                return value.ToString(ObjectUtil.SysCulture);
            }
        }

        public static string Execute(string name, TkDbContext context)
        {
            if (context.ContextConfig.IdStep == 1)
                return ExecuteProc(name, context);
            else
                return ExecuteCache(name, context);
        }
    }
}
