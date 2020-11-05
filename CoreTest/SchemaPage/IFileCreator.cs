using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolkit.SchemaSuite.Schema;

namespace Toolkit.SchemaSuite
{
    public interface IFileCreator
    {
        void CreateMdFile(string path);
    }
}