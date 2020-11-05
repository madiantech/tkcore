using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using YJC.Toolkit.Data;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

// 有关程序集的常规信息通过以下
// 特性集控制。更改这些特性值可修改
// 与程序集关联的信息。
[assembly: AssemblyTitle("YJC.Toolkit.Web.Razor2")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("YJC.Toolkit.Web.Razor2")]
[assembly: AssemblyCopyright("Copyright ©  2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: CLSCompliant(true)]

// 将 ComVisible 设置为 false 使此程序集中的类型
// 对 COM 组件不可见。  如果需要从 COM 访问此程序集中的类型，
// 则将该类型上的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("b8c65ee8-937b-4ccf-ab15-61b09b4fc047")]

// 程序集的版本信息由下面四个值组成:
//
//      主版本
//      次版本
//      生成号
//      修订号
//
// 可以指定所有这些值，也可以使用“生成号”和“修订号”的默认值，
// 方法是按如下所示使用“*”:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("5.1.0.0")]
[assembly: AssemblyFileVersion("5.1.0.0")]
[assembly: AssemblyPlugInFactory(typeof(ModuleTemplatePlugInFactory))]
[assembly: AssemblyPlugInFactory(typeof(PageTemplatePlugInFactory))]
[assembly: AssemblyPlugInFactory(typeof(ModelCreatorPlugInFactory))]
[assembly: AssemblyPlugInFactory(typeof(RazorBaseTemplatePlugInFactory))]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]