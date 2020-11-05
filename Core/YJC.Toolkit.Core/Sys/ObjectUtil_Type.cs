using System;
using System.Linq;
using System.Reflection;
using System.Security;

namespace YJC.Toolkit.Sys
{
    public static partial class ObjectUtil
    {
        private const BindingFlags FLAGS = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
        private readonly static object[] EMPTY = new object[0];

        public static object CreateObjectWithCtor(Type type)
        {
            TkDebug.AssertArgumentNull(type, "type", null);
            try
            {
                ConstructorInfo[] ctors = type.GetConstructors(FLAGS);
                var ctor = (from item in ctors
                            where item.GetParameters().Length == 0
                            select item).FirstOrDefault();
                TkDebug.AssertNotNull(ctor, string.Format(ObjectUtil.SysCulture,
                    "类型{0}没有配置不带参数的构造函数", type), type);
                return ctor.Invoke(EMPTY);
            }
            catch (TargetInvocationException ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "调用类型{0}的不带参数的构造函数时，发生例外，请调试你的代码", type), ex, null);
            }
            catch (MemberAccessException ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "类型{0}可能是一个抽象类型，无法创建", type), ex, null);
            }
            catch (NotSupportedException ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "类型{0}不支持创建", type), ex, null);
            }
            catch (SecurityException ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "调用方不具有类型{0}访问权限", type), ex, null);
            }
            catch (Exception ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "调用类型{0}的不带参数的构造函数是发生例外", type), ex, null);
            }
            return null;
        }

        /// <summary>
        /// 根据类型创建对象
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="ToolkitException"></exception>
        /// <returns></returns>
        public static object CreateObject(Type type)
        {
            TkDebug.AssertArgumentNull(type, "type", null);

            try
            {
                return Activator.CreateInstance(type);
            }
            //catch (MissingMethodException ex)
            //{
            //    TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
            //        "没有找到类型{0}的默认构造函数，请确认", type), ex, null);
            //}
            catch (TargetInvocationException ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "调用类型{0}的默认构造函数时，发生例外，请调试你的代码", type), ex, null);
            }
            //catch (MethodAccessException ex)
            //{
            //    TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
            //        "类型{0}的默认构造函数访问权限不够", type), ex, null);
            //}
            catch (MemberAccessException ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "类型{0}可能是一个抽象类型，无法创建", type), ex, null);
            }
            catch (Exception ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "调用类型{0}的默认构造函数是发生例外", type), ex, null);
            }
            return null;
        }

        private static string GetMethodSign(object[] args)
        {
            var types = from arg in args
                        select arg == null ? "null" : arg.GetType().ToString();
            return string.Join(", ", types);
        }

        public static object CreateObject(Type type, params object[] args)
        {
            TkDebug.AssertArgumentNull(type, "type", null);
            //TkDebug.AssertEnumerableArgumentNull(args, "args", null);
            try
            {
                return Activator.CreateInstance(type, args);
            }
            //catch (MissingMethodException ex)
            //{
            //    TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
            //        "没有找到类型{0}参数为{1}的构造函数，请确认", type, GetMethodSign(args)), ex, null);
            //}
            catch (TargetInvocationException ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "调用类型{0}参数为{1}的构造函数时，发生例外，请调试你的代码",
                    type, GetMethodSign(args)), ex, null);
            }
            //catch (MethodAccessException ex)
            //{
            //    TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
            //        "类型{0}参数为{1}的构造函数访问权限不够",
            //        type, GetMethodSign(args)), ex, null);
            //}
            catch (MemberAccessException ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "类型{0}可能是一个抽象类型，无法创建", type), ex, null);
            }
            catch (Exception ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "调用类型{0}参数为{1}的构造函数是发生例外",
                    type, GetMethodSign(args)), ex, null);
            }
            return null;
        }

        public static object GetValue(PropertyInfo info, object receiver)
        {
            TkDebug.AssertArgumentNull(info, "info", null);
            TkDebug.AssertArgumentNull(receiver, "receiver", null);

            try
            {
                return info.GetValue(receiver, null);
            }
            catch (MemberAccessException ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "无法从对象{0}中获取属性{1}的值，请检查get的访问权限",
                    receiver.GetType(), info.Name), ex, null);
            }
            catch (Exception ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "对象{0}中可能不存在属性{1}，请检查",
                    receiver.GetType(), info.Name), ex, null);
            }
            TkDebug.ThrowImpossibleCode(null);
            return null;
        }

        public static object GetStaticValue(FieldInfo info)
        {
            TkDebug.AssertArgumentNull(info, "info", null);

            try
            {
                return info.GetValue(null);
            }
            catch (MemberAccessException ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "调用方没有访问类型{0}中字段{1}的权限",
                    info.DeclaringType, info.Name), ex, null);
            }
            catch (ArgumentException ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "类型{0}中既不声明字段{1}也不继承字段{1}",
                    info.DeclaringType, info.Name), ex, null);
            }
            catch (NotSupportedException ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "类型{0}中字段{1}被标记为文本，但是该字段没有一个可接受的文本类型",
                    info.DeclaringType, info.Name), ex, null);
            }
            catch (Exception ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "对象{0}中字段{1}可能不是静态的，请检查",
                    info.DeclaringType, info.Name), ex, null);
            }
            TkDebug.ThrowImpossibleCode(null);
            return null;
        }

        internal static object GetPropertyValue(object receiver, Type objectType, ObjectPropertyInfo info)
        {
            object value = info.GetValue(receiver); // ObjectUtil.GetValue(info, receiver);
            if (value == null)
            {
                Type type = objectType ?? info.DataType;
                value = ObjectUtil.CreateObject(type);
                info.SetValue(receiver, value);
            }
            return value;
        }

        public static void SetValue(PropertyInfo info, object receiver, object value)
        {
            TkDebug.AssertArgumentNull(info, "info", null);
            TkDebug.AssertArgumentNull(receiver, "receiver", null);

            try
            {
                info.SetValue(receiver, value, null);
            }
            catch (System.ArgumentException ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "对象{0}未找到属性{1}的 set 访问器或者数组不包含所需类型的参数",
                    receiver.GetType(), info.Name), ex, null);
            }
            catch (MemberAccessException ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "无法设置对象{0}中属性{1}的值，请检查set的访问权限",
                    receiver.GetType(), info.Name), ex, null);
            }
            catch (Exception ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "对象{0}中可能不存在属性{1}，请检查",
                    receiver.GetType(), info.Name), ex, null);
            }
        }

        public static bool IsSubType(Type baseType, Type type)
        {
            TkDebug.AssertArgumentNull(baseType, "baseType", null);
            TkDebug.AssertArgumentNull(type, "type", null);

            if (baseType.IsInterface)
            {
                if (baseType.IsGenericType)
                {
                    Type[] interfaces = type.GetInterfaces();
                    foreach (Type intf in interfaces)
                    {
                        if (baseType.IsGenericTypeDefinition)
                        {
                            if (intf.Namespace == baseType.Namespace && intf.Name == baseType.Name)
                                return true;
                        }
                        else
                            if (intf == baseType)
                                return true;
                    }
                    return false;
                }
                else
                {
                    var intf = (from item in type.GetInterfaces()
                                where item == baseType
                                select item).FirstOrDefault();
                    return intf != null;
                }
            }
            else
                return type.IsSubclassOf(baseType) || baseType == type;
        }

        public static Type GetRegType(string regName)
        {
            TkDebug.AssertArgumentNullOrEmpty(regName, "regName", null);

            TkDebug.ThrowIfNoGlobalVariable();
            RegTypeFactory factory = BaseGlobalVariable.Current.FactoryManager.
                GetCodeFactory(RegTypeFactory.REG_NAME).Convert<RegTypeFactory>();
            Type type = factory.GetType(regName);
            TkDebug.AssertNotNull(type, string.Format(ObjectUtil.SysCulture,
                "没有找到注册名为{0}的注册类型", regName), null);

            return type;
        }
    }
}