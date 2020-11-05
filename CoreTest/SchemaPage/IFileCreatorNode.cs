using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Toolkit.SchemaSuite
{
    public interface IFileCreatorNode
    {
        void CreateContent(StringBuilder builder, int level, bool isRoot);

        void CreateRefContent(StringBuilder builder, string fileName, int level);
    }
}