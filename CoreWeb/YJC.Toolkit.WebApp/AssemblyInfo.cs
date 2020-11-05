using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

// 有关程序集的常规信息通过以下
// 特性集控制。更改这些特性值可修改
// 与程序集关联的信息。
[assembly: AssemblyCopyright("Copyright ©  2009-2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// 将 ComVisible 设置为 false 使此程序集中的类型
// 对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型，
// 则将该类型上的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("ae3dabd4-cf50-4eff-9b64-1593041d162c")]

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
[assembly: AssemblyPlugInFactory(typeof(ExceptionHanlderConfigFactory))]
[assembly: AssemblyPlugInFactory(typeof(WebPagePlugInFactory))]
[assembly: AssemblyPlugInFactory(typeof(DefaultHandlerConfigFactory))]
[assembly: AssemblyPlugInFactory(typeof(TimingJobPlugInFactory))]
[assembly: AssemblyPlugInFactory(typeof(HttpHandlerPlugInFactory))]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
[assembly: InternalsVisibleTo("YJC.Toolkit.WebExtension, PublicKey=00240000048000009400000006020000"
    + "00240000525341310004000001000100CFD581E82257611BA519160619FD26B7"
    + "FAB3EE8B69D29E3662F2D0FA16D0CCFF5483F375BE6BA603B06479B3AB4D6FDB"
    + "1D1CF6C3D6195AE980783ACD2267064A4366B9A3CD10DFC29BFF259058D07085"
    + "4807DFE5A9DD79F49F58245DDAB1573D8BC42F18479F224B3525EB104AEE31E9"
    + "973CA69E2285F9F991D65BE62AC07AE8")]
[assembly: InternalsVisibleTo("YJC.Toolkit.Razor.Web, PublicKey=00240000048000009400000006020000"
    + "00240000525341310004000001000100CFD581E82257611BA519160619FD26B7"
    + "FAB3EE8B69D29E3662F2D0FA16D0CCFF5483F375BE6BA603B06479B3AB4D6FDB"
    + "1D1CF6C3D6195AE980783ACD2267064A4366B9A3CD10DFC29BFF259058D07085"
    + "4807DFE5A9DD79F49F58245DDAB1573D8BC42F18479F224B3525EB104AEE31E9"
    + "973CA69E2285F9F991D65BE62AC07AE8")]
[assembly: InternalsVisibleTo("YJC.Toolkit.NodeJs, PublicKey=00240000048000009400000006020000"
    + "00240000525341310004000001000100CFD581E82257611BA519160619FD26B7"
    + "FAB3EE8B69D29E3662F2D0FA16D0CCFF5483F375BE6BA603B06479B3AB4D6FDB"
    + "1D1CF6C3D6195AE980783ACD2267064A4366B9A3CD10DFC29BFF259058D07085"
    + "4807DFE5A9DD79F49F58245DDAB1573D8BC42F18479F224B3525EB104AEE31E9"
    + "973CA69E2285F9F991D65BE62AC07AE8")]
//[assembly: InternalsVisibleTo("YJC.Toolkit.Web.Razor2, PublicKey=00240000048000009400000006020000"
//    + "00240000525341310004000001000100CFD581E82257611BA519160619FD26B7"
//    + "FAB3EE8B69D29E3662F2D0FA16D0CCFF5483F375BE6BA603B06479B3AB4D6FDB"
//    + "1D1CF6C3D6195AE980783ACD2267064A4366B9A3CD10DFC29BFF259058D07085"
//    + "4807DFE5A9DD79F49F58245DDAB1573D8BC42F18479F224B3525EB104AEE31E9"
//    + "973CA69E2285F9F991D65BE62AC07AE8")]
//[assembly: InternalsVisibleTo("YJC.Toolkit.Web.Excel, PublicKey=00240000048000009400000006020000"
//    + "00240000525341310004000001000100CFD581E82257611BA519160619FD26B7"
//    + "FAB3EE8B69D29E3662F2D0FA16D0CCFF5483F375BE6BA603B06479B3AB4D6FDB"
//    + "1D1CF6C3D6195AE980783ACD2267064A4366B9A3CD10DFC29BFF259058D07085"
//    + "4807DFE5A9DD79F49F58245DDAB1573D8BC42F18479F224B3525EB104AEE31E9"
//    + "973CA69E2285F9F991D65BE62AC07AE8")]