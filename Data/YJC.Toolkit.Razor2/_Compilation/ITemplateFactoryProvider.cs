using System;

namespace YJC.Toolkit.Razor
{
    public interface ITemplateFactoryProvider
    {
        Func<ITemplatePage> CreateFactory(CompiledTemplateDescriptor templateDescriptor);
    }
}