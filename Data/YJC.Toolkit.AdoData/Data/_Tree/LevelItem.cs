
namespace YJC.Toolkit.Data
{
    internal class LevelItem
    {
        /// <summary>
        /// Initializes a new instance of the LevelItem class.
        /// </summary>
        public LevelItem(int start, int length)
        {
            Length = length;
            Start = start;
            End = start + Length;
        }

        public int Length { get; set; }

        public int Start { get; set; }

        public int End { get; private set; }
    }
}
