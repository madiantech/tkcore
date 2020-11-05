namespace YJC.Toolkit.Web
{
    public class UserAgent
    {
        private readonly string fUserAgent;
        private ClientBrowser fBrowser;
        private ClientOS fOS;

        public UserAgent(string userAgent)
        {
            fUserAgent = userAgent;
        }

        public ClientBrowser Browser
        {
            get
            {
                if (fBrowser == null)
                    fBrowser = new ClientBrowser(fUserAgent);
                return fBrowser;
            }
        }

        public ClientOS OS
        {
            get
            {
                if (fOS == null)
                    fOS = new ClientOS(fUserAgent);
                return fOS;
            }
        }
    }
}