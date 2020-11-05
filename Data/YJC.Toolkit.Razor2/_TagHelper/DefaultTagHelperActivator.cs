using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace YJC.Toolkit.Razor
{
    /// <summary>
    /// Default implementation of <see cref="ITagHelperActivator"/>.
    /// </summary>
    public class DefaultTagHelperActivator : ITagHelperActivator
    {

        /// <summary>
        /// Instantiates a new <see cref="DefaultTagHelperActivator"/> instance.
        /// </summary>
        /// <param name="typeActivatorCache">The <see cref="ITypeActivatorCache"/>.</param>
        public DefaultTagHelperActivator()
        {
        }

        /// <inheritdoc />
        public TTagHelper Create<TTagHelper>(PageContext context)
            where TTagHelper : ITagHelper
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            //TODO: replace serviceCollection
            //return _typeActivatorCache.CreateInstance<TTagHelper>(
            //    context.HttpContext.RequestServices,
            //    typeof(TTagHelper));

            throw new NotImplementedException();
        }
    }
}