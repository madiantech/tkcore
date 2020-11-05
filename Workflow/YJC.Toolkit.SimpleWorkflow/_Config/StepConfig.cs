using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [CacheInstance]
    public abstract class StepConfig : IParentObject, IRegName
    {
        private readonly List<StepConnectionConfig> fNextStepNames;
        private readonly List<string> fPrevStepNames;
        private string fName;
        private ReadOnlyCollection<StepConfig> fNextSteps;
        private ReadOnlyCollection<StepConfig> fPrevSteps;
        private StepConfig[] fNextStepArray;
        private StepConfig[] fPrevStepArray;
        private bool fInitialized;

        protected internal StepConfig()
        {
            fNextStepNames = new List<StepConnectionConfig>();
            fPrevStepNames = new List<string>();
        }

        protected internal StepConfig(WorkflowConfig workflowConfig)
            : this()
        {
            Parent = workflowConfig;
        }

        #region IParentObject 成员

        void IParentObject.SetParent(object parent)
        {
            Parent = parent as WorkflowConfig;
            SetParentObj(parent);
        }

        #endregion IParentObject 成员

        #region IRegName 成员

        public string RegName
        {
            get
            {
                return Name;
            }
        }

        #endregion IRegName 成员

        [SimpleAttribute]
        internal int Height { get; set; }

        [SimpleAttribute]
        internal int Width { get; set; }

        [SimpleAttribute]
        internal int Left { get; set; }

        [SimpleAttribute]
        internal int Top { get; set; }

        [SimpleElement(LocalName = "PrevStep", IsMultiple = true, ObjectType = typeof(string))]
        protected internal IEnumerable<string> PrevStepName
        {
            get
            {
                return fPrevStepNames;
            }
        }

        [ObjectElement(LocalName = "NextStep", IsMultiple = true,
            ObjectType = typeof(StepConnectionConfig), UseConstructor = true)]
        internal IEnumerable<StepConnectionConfig> NextStepNames
        {
            get
            {
                return fNextStepNames;
            }
        }

        [SimpleElement]
        public virtual string Name
        {
            get
            {
                return fName;
            }
            set
            {
                if (fName == value)
                    return;
                fName = value;
                if (Parent != null)
                {
                }
            }
        }

        [SimpleElement]
        public virtual string DisplayName { get; internal set; }

        public WorkflowConfig Parent { get; internal set; }

        public IEnumerable<StepConfig> PrevSteps
        {
            get
            {
                Initialize();
                return fPrevSteps;
            }
        }

        public IEnumerable<StepConfig> NextSteps
        {
            get
            {
                Initialize();
                return fNextSteps;
            }
        }

        public int NextStepCount
        {
            get
            {
                return fNextStepNames.Count;
            }
        }

        public virtual StepType StepType
        {
            get
            {
                return StepType.None;
            }
        }

        public virtual bool HasInStep
        {
            get
            {
                return true;
            }
        }

        public virtual bool HasOutStep
        {
            get
            {
                return true;
            }
        }

        public virtual bool HasMultipleInStep
        {
            get
            {
                return true;
            }
        }

        public virtual bool HasMultipleOutStep
        {
            get
            {
                return false;
            }
        }

        private void Initialize()
        {
            if (!fInitialized)
            {
                fInitialized = true;
                fPrevStepArray = Array.ConvertAll(fPrevStepNames.ToArray(),
                    (input) => Parent.Steps[input]);
                fPrevSteps = new ReadOnlyCollection<StepConfig>(fPrevStepArray);
                fNextStepArray = Array.ConvertAll(fNextStepNames.ToArray(),
                    (input) => Parent.Steps[input.StepName]);
                fNextSteps = new ReadOnlyCollection<StepConfig>(fNextStepArray);
            }
        }

        protected internal void SetParentObj(object parent)
        {
            WorkflowConfig config = parent as WorkflowConfig;
            SetParent(config);
        }

        protected virtual void SetParent(WorkflowConfig parent)
        {
        }

        public virtual void Prepare(WorkflowContent content, DataRow workflowRow, IDbDataSource source)
        {
        }

        internal void AddNextConfig(StepConfig config)
        {
            AddNextConfig(config, 0, 0, 0, 0);
        }

        internal void AddNextConfig(StepConfig config, int fromX, int fromY, int toX, int toY)
        {
            config.fPrevStepNames.Add(Name);
            fNextStepNames.Add(new StepConnectionConfig(config.Name, fromX, fromY, toX, toY));
        }

        internal abstract Step CreateStep(Workflow workflow);
    }
}