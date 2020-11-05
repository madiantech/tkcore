using System.Collections.Generic;
using System.Threading.Tasks;

namespace YJC.Toolkit.Razor
{
    public abstract class TkRazorProject
    {
        protected TkRazorProject()
        {
        }

        public abstract Task<TkRazorProjectItem> GetItemAsync(string templateKey);

        public abstract Task<IEnumerable<TkRazorProjectItem>> GetImportsAsync(string templateKey);
    }
}