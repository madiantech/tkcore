using System;
using System.Resources;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using YJC.Toolkit.Sys;
using YJC.Toolkit.MetaData;

// 有关程序集的常规信息通过以下特性集
// 控制。更改这些特性值可修改
// 与程序集关联的信息。
[assembly: CLSCompliant(true)]

// 程序集的版本信息由以下四个值组成:
//
//      主版本
//      次版本
//      生成号
//      修订号
//
// 可以指定所有这些值，也可以使用“生成号”和“修订号”的默认值，
// 方法是按如下所示使用“*”:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyPlugInFactory(typeof(MetaDataConfigFactory))]
[assembly: AssemblyPlugInFactory(typeof(TypeSchemeTypeFactory))]
[assembly: AssemblyPlugInFactory(typeof(TableSchemeConfigFactory))]
[assembly: AssemblyPlugInFactory(typeof(SingleMetaDataConfigFactory))]
[assembly: AssemblyPlugInFactory(typeof(TableSchemeExConfigFactory))]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]