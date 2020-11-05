using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class StepConnectionConfig
    {
        protected StepConnectionConfig()
        {
        }

        /// <summary>
        /// Initializes a new instance of the StepConnectionConfig class.
        /// </summary>
        public StepConnectionConfig(string stepName, int fromX, int fromY, int toX, int toY)
        {
            FromX = fromX;
            FromY = fromY;
            ToX = toX;
            ToY = toY;
            StepName = stepName;
        }

        [SimpleAttribute]
        public int FromX { get; set; }

        [SimpleAttribute]
        public int FromY { get; set; }

        [SimpleAttribute]
        public int ToX { get; set; }

        [SimpleAttribute]
        public int ToY { get; set; }

        [SimpleAttribute]
        public string StepName { get; set; }
    }
}