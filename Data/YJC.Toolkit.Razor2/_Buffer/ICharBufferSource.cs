namespace YJC.Toolkit.Razor
{
    internal interface ICharBufferSource
    {
        char[] Rent(int bufferSize);

        void Return(char[] buffer);
    }
}