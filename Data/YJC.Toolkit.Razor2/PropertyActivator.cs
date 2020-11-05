using System;
using System.Linq;
using System.Reflection;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class PropertyActivator<TContext>
    {
        private readonly Func<TContext, object> fValueAccessor;
        private readonly Action<object, object> fFastPropertySetter;

        public PropertyActivator(PropertyInfo propertyInfo, Func<TContext, object> valueAccessor)
        {
            PropertyInfo = propertyInfo;
            fValueAccessor = valueAccessor;
            fFastPropertySetter = FastPropertySetter.MakeFastPropertySetter(propertyInfo);
        }

        public PropertyInfo PropertyInfo { get; }

        public object Activate(object instance, TContext context)
        {
            TkDebug.AssertArgumentNull(instance, nameof(instance), this);

            var value = fValueAccessor(context);
            fFastPropertySetter(instance, value);
            return value;
        }

        public static PropertyActivator<TContext>[] GetPropertiesToActivate(Type type,
             Type activateAttributeType, Func<PropertyInfo, PropertyActivator<TContext>> createActivateInfo)
        {
            TkDebug.AssertArgumentNull(type, nameof(type), null);
            TkDebug.AssertArgumentNull(activateAttributeType, nameof(activateAttributeType), null);
            TkDebug.AssertArgumentNull(createActivateInfo, nameof(createActivateInfo), null);

            return GetPropertiesToActivate(type, activateAttributeType, createActivateInfo, false);
        }

        public static PropertyActivator<TContext>[] GetPropertiesToActivate(Type type, Type activateAttributeType,
            Func<PropertyInfo, PropertyActivator<TContext>> createActivateInfo, bool includeNonPublic)
        {
            TkDebug.AssertArgumentNull(type, nameof(type), null);
            TkDebug.AssertArgumentNull(activateAttributeType, nameof(activateAttributeType), null);
            TkDebug.AssertArgumentNull(createActivateInfo, nameof(createActivateInfo), null);

            var properties = type.GetRuntimeProperties().Where((property) =>
                {
                    return
                        property.IsDefined(activateAttributeType) &&
                        property.GetIndexParameters().Length == 0 &&
                        property.SetMethod != null &&
                        !property.SetMethod.IsStatic;
                });

            if (!includeNonPublic)
            {
                properties = properties.Where(property => property.SetMethod.IsPublic);
            }

            return properties.Select(createActivateInfo).ToArray();
        }
    }
}