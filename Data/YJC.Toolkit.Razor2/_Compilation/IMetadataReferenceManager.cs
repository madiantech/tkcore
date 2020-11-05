using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Reflection;

namespace YJC.Toolkit.Razor
{
    public interface IMetadataReferenceManager
    {
        IReadOnlyList<MetadataReference> Resolve(Assembly assembly);

        HashSet<MetadataReference> AdditionalMetadataReferences { get; }
    }
}