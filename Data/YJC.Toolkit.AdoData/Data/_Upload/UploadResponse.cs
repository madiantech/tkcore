using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class UploadResponse
    {
        [ObjectElement(IsMultiple = true)]
        public List<UploadFileResponse> files { get; private set; }

        public UploadResponse(IEnumerable<UploadFileResponse> fileResponses)
        {
            files = new List<UploadFileResponse>(fileResponses);
        }
    }
}
