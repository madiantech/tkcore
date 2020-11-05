using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class FastPropertySetter
    {
        private delegate TValue ByRefFunc<TDeclaringType, TValue>(ref TDeclaringType arg);

        private static readonly TypeInfo fTypeInfo = typeof(FastPropertySetter).GetTypeInfo();

        private static readonly MethodInfo fCallPropertyGetterOpenGenericMethod =
            fTypeInfo.GetDeclaredMethod(nameof(CallPropertyGetter));

        private static readonly MethodInfo fCallPropertyGetterByReferenceOpenGenericMethod =
            fTypeInfo.GetDeclaredMethod(nameof(CallPropertyGetterByReference));

        private static readonly MethodInfo fCallNullSafePropertyGetterOpenGenericMethod =
            fTypeInfo.GetDeclaredMethod(nameof(CallNullSafePropertyGetter));

        private static readonly MethodInfo fCallNullSafePropertyGetterByReferenceOpenGenericMethod =
            fTypeInfo.GetDeclaredMethod(nameof(CallNullSafePropertyGetterByReference));

        private static readonly MethodInfo fCallPropertySetterOpenGenericMethod =
            fTypeInfo.GetDeclaredMethod(nameof(CallPropertySetter));

        private static readonly ConcurrentDictionary<Type, FastPropertySetter[]> fPropertiesCache =
            new ConcurrentDictionary<Type, FastPropertySetter[]>();

        private static readonly ConcurrentDictionary<Type, FastPropertySetter[]> fVisiblePropertiesCache =
            new ConcurrentDictionary<Type, FastPropertySetter[]>();

        private Action<object, object> fValueSetter;
        private Func<object, object> fValueGetter;

        public FastPropertySetter(PropertyInfo property)
        {
            TkDebug.AssertArgumentNull(property, nameof(property), null);

            Property = property;
            Name = property.Name;
        }

        public PropertyInfo Property { get; }

        public virtual string Name { get; protected set; }

        public Func<object, object> ValueGetter
        {
            get
            {
                if (fValueGetter == null)
                    fValueGetter = MakeFastPropertyGetter(Property);

                return fValueGetter;
            }
        }

        public Action<object, object> ValueSetter
        {
            get
            {
                if (fValueSetter == null)
                    fValueSetter = MakeFastPropertySetter(Property);

                return fValueSetter;
            }
        }

        public object GetValue(object instance)
        {
            return ValueGetter(instance);
        }

        public void SetValue(object instance, object value)
        {
            ValueSetter(instance, value);
        }

        public static FastPropertySetter[] GetProperties(TypeInfo typeInfo)
        {
            return GetProperties(typeInfo.AsType());
        }

        public static FastPropertySetter[] GetProperties(Type type)
        {
            return GetProperties(type, CreateInstance, fPropertiesCache);
        }

        public static FastPropertySetter[] GetVisibleProperties(TypeInfo typeInfo)
        {
            return GetVisibleProperties(typeInfo.AsType(), CreateInstance, fPropertiesCache, fVisiblePropertiesCache);
        }

        public static FastPropertySetter[] GetVisibleProperties(Type type)
        {
            return GetVisibleProperties(type, CreateInstance, fPropertiesCache, fVisiblePropertiesCache);
        }

        private static FastPropertySetter CreateInstance(PropertyInfo property)
        {
            return new FastPropertySetter(property);
        }

        private static object CallPropertyGetter<TDeclaringType, TValue>(
            Func<TDeclaringType, TValue> getter, object target)
        {
            return getter((TDeclaringType)target);
        }

        // Called via reflection
        private static object CallPropertyGetterByReference<TDeclaringType, TValue>(
            ByRefFunc<TDeclaringType, TValue> getter, object target)
        {
            var unboxed = (TDeclaringType)target;
            return getter(ref unboxed);
        }

        private static object CallNullSafePropertyGetter<TDeclaringType, TValue>(
            Func<TDeclaringType, TValue> getter, object target)
        {
            if (target == null)
                return null;

            return getter((TDeclaringType)target);
        }

        private static object CallNullSafePropertyGetterByReference<TDeclaringType, TValue>(
            ByRefFunc<TDeclaringType, TValue> getter, object target)
        {
            if (target == null)
                return null;

            var unboxed = (TDeclaringType)target;
            return getter(ref unboxed);
        }

        private static void CallPropertySetter<TDeclaringType, TValue>(Action<TDeclaringType, TValue> setter,
            object target, object value)
        {
            setter((TDeclaringType)target, (TValue)value);
        }

        private static bool IsInterestingProperty(PropertyInfo property)
        {
            // For improving application startup time, do not use GetIndexParameters() api early in this check as it
            // creates a copy of parameter array and also we would like to check for the presence of a get method
            // and short circuit asap.
            return property.GetMethod != null && property.GetMethod.IsPublic &&
                !property.GetMethod.IsStatic && property.GetMethod.GetParameters().Length == 0;
        }

        private static Func<object, object> MakeFastPropertyGetter(PropertyInfo propertyInfo,
            MethodInfo propertyGetterWrapperMethod, MethodInfo propertyGetterByRefWrapperMethod)
        {
            TkDebug.AssertArgumentNull(propertyInfo, nameof(propertyInfo), null);
            TkDebug.AssertArgumentNull(propertyGetterWrapperMethod, nameof(propertyGetterWrapperMethod), null);
            TkDebug.AssertArgumentNull(propertyGetterByRefWrapperMethod, nameof(propertyGetterByRefWrapperMethod), null);

            // Must be a generic method with a Func<,> parameter
            TkDebug.Assert(propertyGetterWrapperMethod.IsGenericMethodDefinition, "", null);
            TkDebug.Assert(propertyGetterWrapperMethod.GetParameters().Length == 2, "", null);

            // Must be a generic method with a ByRefFunc<,> parameter
            TkDebug.Assert(propertyGetterByRefWrapperMethod.IsGenericMethodDefinition, "", null);
            TkDebug.Assert(propertyGetterByRefWrapperMethod.GetParameters().Length == 2, "", null);

            var getMethod = propertyInfo.GetMethod;
            TkDebug.AssertNotNull(getMethod, "", null);
            TkDebug.Assert(!getMethod.IsStatic, "", null);
            TkDebug.Assert(getMethod.GetParameters().Length == 0, "", null);

            // Instance methods in the CLR can be turned into static methods where the first parameter
            // is open over "target". This parameter is always passed by reference, so we have a code
            // path for value types and a code path for reference types.
            if (getMethod.DeclaringType.GetTypeInfo().IsValueType)
            {
                // Create a delegate (ref TDeclaringType) -> TValue
                return MakeFastPropertyGetter(typeof(ByRefFunc<,>), getMethod,
                    propertyGetterByRefWrapperMethod);
            }
            else
            {
                // Create a delegate TDeclaringType -> TValue
                return MakeFastPropertyGetter(typeof(Func<,>), getMethod,
                    propertyGetterWrapperMethod);
            }
        }

        private static Func<object, object> MakeFastPropertyGetter(Type openGenericDelegateType,
            MethodInfo propertyGetMethod, MethodInfo openGenericWrapperMethod)
        {
            var typeInput = propertyGetMethod.DeclaringType;
            var typeOutput = propertyGetMethod.ReturnType;

            var delegateType = openGenericDelegateType.MakeGenericType(typeInput, typeOutput);
            var propertyGetterDelegate = propertyGetMethod.CreateDelegate(delegateType);

            var wrapperDelegateMethod = openGenericWrapperMethod.MakeGenericMethod(typeInput, typeOutput);
            var accessorDelegate = wrapperDelegateMethod.CreateDelegate(
                typeof(Func<object, object>), propertyGetterDelegate);

            return (Func<object, object>)accessorDelegate;
        }

        protected static FastPropertySetter[] GetVisibleProperties(Type type,
            Func<PropertyInfo, FastPropertySetter> createPropertyHelper,
            ConcurrentDictionary<Type, FastPropertySetter[]> allPropertiesCache,
            ConcurrentDictionary<Type, FastPropertySetter[]> visiblePropertiesCache)
        {
            if (visiblePropertiesCache.TryGetValue(type, out FastPropertySetter[] result))
            {
                return result;
            }

            // The simple and common case, this is normal POCO object - no need to allocate.
            var allPropertiesDefinedOnType = true;
            var allProperties = GetProperties(type, createPropertyHelper, allPropertiesCache);
            foreach (var propertyHelper in allProperties)
            {
                if (propertyHelper.Property.DeclaringType != type)
                {
                    allPropertiesDefinedOnType = false;
                    break;
                }
            }

            if (allPropertiesDefinedOnType)
            {
                result = allProperties;
                visiblePropertiesCache.TryAdd(type, result);
                return result;
            }

            // There's some inherited properties here, so we need to check for hiding via 'new'.
            var filteredProperties = new List<FastPropertySetter>(allProperties.Length);
            foreach (var propertyHelper in allProperties)
            {
                var declaringType = propertyHelper.Property.DeclaringType;
                if (declaringType == type)
                {
                    filteredProperties.Add(propertyHelper);
                    continue;
                }

                // If this property was declared on a base type then look for the definition closest to the
                // the type to see if we should include it.
                var ignoreProperty = false;

                // Walk up the hierarchy until we find the type that actually declares this
                // PropertyInfo.
                var currentTypeInfo = type.GetTypeInfo();
                var declaringTypeInfo = declaringType.GetTypeInfo();
                while (currentTypeInfo != null && currentTypeInfo != declaringTypeInfo)
                {
                    // We've found a 'more proximal' public definition
                    var declaredProperty = currentTypeInfo.GetDeclaredProperty(propertyHelper.Name);
                    if (declaredProperty != null)
                    {
                        ignoreProperty = true;
                        break;
                    }

                    currentTypeInfo = currentTypeInfo.BaseType?.GetTypeInfo();
                }

                if (!ignoreProperty)
                {
                    filteredProperties.Add(propertyHelper);
                }
            }

            result = filteredProperties.ToArray();
            visiblePropertiesCache.TryAdd(type, result);
            return result;
        }

        protected static FastPropertySetter[] GetProperties(Type type,
            Func<PropertyInfo, FastPropertySetter> createPropertyHelper,
            ConcurrentDictionary<Type, FastPropertySetter[]> cache)
        {
            // Unwrap nullable types. This means Nullable<T>.Value and Nullable<T>.HasValue will not be
            // part of the sequence of properties returned by this method.
            type = Nullable.GetUnderlyingType(type) ?? type;

            if (!cache.TryGetValue(type, out FastPropertySetter[] helpers))
            {
                // We avoid loading indexed properties using the Where statement.
                var properties = type.GetRuntimeProperties().Where(IsInterestingProperty);

                var typeInfo = type.GetTypeInfo();
                if (typeInfo.IsInterface)
                {
                    // Reflection does not return information about inherited properties on the interface itself.
                    properties = properties.Concat(typeInfo.ImplementedInterfaces.SelectMany(
                        interfaceType => interfaceType.GetRuntimeProperties().Where(IsInterestingProperty)));
                }

                helpers = properties.Select(p => createPropertyHelper(p)).ToArray();
                cache.TryAdd(type, helpers);
            }

            return helpers;
        }

        public static Func<object, object> MakeFastPropertyGetter(PropertyInfo propertyInfo)
        {
            TkDebug.AssertArgumentNull(propertyInfo, nameof(propertyInfo), null);

            return MakeFastPropertyGetter(propertyInfo, fCallPropertyGetterOpenGenericMethod,
                fCallPropertyGetterByReferenceOpenGenericMethod);
        }

        public static Func<object, object> MakeNullSafeFastPropertyGetter(PropertyInfo propertyInfo)
        {
            TkDebug.AssertArgumentNull(propertyInfo, nameof(propertyInfo), null);

            return MakeFastPropertyGetter(propertyInfo, fCallNullSafePropertyGetterOpenGenericMethod,
                fCallNullSafePropertyGetterByReferenceOpenGenericMethod);
        }

        public static Action<object, object> MakeFastPropertySetter(PropertyInfo propertyInfo)
        {
            TkDebug.AssertArgumentNull(propertyInfo, nameof(propertyInfo), null);
            TkDebug.Assert(!propertyInfo.DeclaringType.GetTypeInfo().IsValueType, "", null);

            var setMethod = propertyInfo.SetMethod;
            TkDebug.AssertNotNull(setMethod, "", null);
            TkDebug.Assert(!setMethod.IsStatic, "", null);
            TkDebug.Assert(setMethod.ReturnType == typeof(void), "", null);
            var parameters = setMethod.GetParameters();
            TkDebug.Assert(parameters.Length == 1, "", null);

            // Instance methods in the CLR can be turned into static methods where the first parameter
            // is open over "target". This parameter is always passed by reference, so we have a code
            // path for value types and a code path for reference types.
            var typeInput = setMethod.DeclaringType;
            var parameterType = parameters[0].ParameterType;

            // Create a delegate TDeclaringType -> { TDeclaringType.Property = TValue; }
            var propertySetterAsAction =
                setMethod.CreateDelegate(typeof(Action<,>).MakeGenericType(typeInput, parameterType));
            var callPropertySetterClosedGenericMethod =
                fCallPropertySetterOpenGenericMethod.MakeGenericMethod(typeInput, parameterType);
            var callPropertySetterDelegate =
                callPropertySetterClosedGenericMethod.CreateDelegate(
                    typeof(Action<object, object>), propertySetterAsAction);

            return (Action<object, object>)callPropertySetterDelegate;
        }
    }
}