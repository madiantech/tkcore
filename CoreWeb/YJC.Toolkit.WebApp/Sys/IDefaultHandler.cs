using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace YJC.Toolkit.Sys
{
    public interface IDefaultHandler
    {
        Task Process(HttpContext context);
    }
}