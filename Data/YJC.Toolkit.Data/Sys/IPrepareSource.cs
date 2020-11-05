namespace YJC.Toolkit.Sys
{
    public interface IPrepareSource : ISource
    {
        void Prepare(IInputData input);
    }
}