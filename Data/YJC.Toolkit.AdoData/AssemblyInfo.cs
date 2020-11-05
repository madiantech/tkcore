using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using YJC.Toolkit.Data;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.Log;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

// 有关程序集的常规信息通过以下 特性集控制。更改这些特性值可修改 与程序集关联的信息。
[assembly: CLSCompliant(true)]

// 程序集的版本信息由下面四个值组成:
//
// 主版本 次版本 生成号 修订号
//
// 可以指定所有这些值，也可以使用“生成号”和“修订号”的默认值， 方法是按如下所示使用“*”: [assembly: AssemblyVersion("1.0.*")]
[assembly: InternalsVisibleTo("YJC.Toolkit.WebApp, PublicKey=00240000048000009400000006020000"
    + "00240000525341310004000001000100CFD581E82257611BA519160619FD26B7"
    + "FAB3EE8B69D29E3662F2D0FA16D0CCFF5483F375BE6BA603B06479B3AB4D6FDB"
    + "1D1CF6C3D6195AE980783ACD2267064A4366B9A3CD10DFC29BFF259058D07085"
    + "4807DFE5A9DD79F49F58245DDAB1573D8BC42F18479F224B3525EB104AEE31E9"
    + "973CA69E2285F9F991D65BE62AC07AE8")]
[assembly: InternalsVisibleTo("YJC.Toolkit.Web.Common, PublicKey=00240000048000009400000006020000"
    + "00240000525341310004000001000100CFD581E82257611BA519160619FD26B7"
    + "FAB3EE8B69D29E3662F2D0FA16D0CCFF5483F375BE6BA603B06479B3AB4D6FDB"
    + "1D1CF6C3D6195AE980783ACD2267064A4366B9A3CD10DFC29BFF259058D07085"
    + "4807DFE5A9DD79F49F58245DDAB1573D8BC42F18479F224B3525EB104AEE31E9"
    + "973CA69E2285F9F991D65BE62AC07AE8")]
[assembly: InternalsVisibleTo("YJC.Toolkit.Tenant, PublicKey=00240000048000009400000006020000"
    + "00240000525341310004000001000100CFD581E82257611BA519160619FD26B7"
    + "FAB3EE8B69D29E3662F2D0FA16D0CCFF5483F375BE6BA603B06479B3AB4D6FDB"
    + "1D1CF6C3D6195AE980783ACD2267064A4366B9A3CD10DFC29BFF259058D07085"
    + "4807DFE5A9DD79F49F58245DDAB1573D8BC42F18479F224B3525EB104AEE31E9"
    + "973CA69E2285F9F991D65BE62AC07AE8")]
[assembly: Initialization(typeof(AdoInitializtion))]
[assembly: AssemblyPlugInFactory(typeof(ResolverCreatorConfigFactory))]
[assembly: AssemblyPlugInFactory(typeof(ResolverPlugInFactory))]
[assembly: AssemblyPlugInFactory(typeof(OperateRightConfigFactory))]
[assembly: AssemblyPlugInFactory(typeof(OperatorsConfigFactory))]
[assembly: AssemblyPlugInFactory(typeof(CodeTableConfigFactory))]
[assembly: AssemblyPlugInFactory(typeof(EasySearchConfigFactory))]
[assembly: AssemblyPlugInFactory(typeof(SearchPlugInFactory))]
[assembly: AssemblyPlugInFactory(typeof(UploadProcessorConfigFactory))]
[assembly: AssemblyPlugInFactory(typeof(TreeConfigFactory))]
[assembly: AssemblyPlugInFactory(typeof(OperateRightPlugInFactory))]
[assembly: AssemblyPlugInFactory(typeof(UpdatedActionConfigFactory))]
[assembly: AssemblyPlugInFactory(typeof(UpdatedActionPlugInFactory))]
[assembly: AssemblyPlugInFactory(typeof(RecordDataPickerConfigFactory))]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]