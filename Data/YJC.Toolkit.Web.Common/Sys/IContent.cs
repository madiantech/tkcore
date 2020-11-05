using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace YJC.Toolkit.Sys
{
    public interface IContent
    {
        string ContentType { get; }

        string Content { get; }

        Encoding ContentEncoding { get; }

        Dictionary<string, string> Headers { get; }

        Task WritePage(object response);
    }
}