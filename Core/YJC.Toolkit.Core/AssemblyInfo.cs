﻿using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

[assembly: CLSCompliant(true)]
[assembly: InternalsVisibleTo("YJC.Toolkit.ConsoleApp, PublicKey=00240000048000009400000006020000"
    + "00240000525341310004000001000100CFD581E82257611BA519160619FD26B7"
    + "FAB3EE8B69D29E3662F2D0FA16D0CCFF5483F375BE6BA603B06479B3AB4D6FDB"
    + "1D1CF6C3D6195AE980783ACD2267064A4366B9A3CD10DFC29BFF259058D07085"
    + "4807DFE5A9DD79F49F58245DDAB1573D8BC42F18479F224B3525EB104AEE31E9"
    + "973CA69E2285F9F991D65BE62AC07AE8")]
[assembly: InternalsVisibleTo("YJC.Toolkit.ToolApp, PublicKey=00240000048000009400000006020000"
    + "00240000525341310004000001000100CFD581E82257611BA519160619FD26B7"
    + "FAB3EE8B69D29E3662F2D0FA16D0CCFF5483F375BE6BA603B06479B3AB4D6FDB"
    + "1D1CF6C3D6195AE980783ACD2267064A4366B9A3CD10DFC29BFF259058D07085"
    + "4807DFE5A9DD79F49F58245DDAB1573D8BC42F18479F224B3525EB104AEE31E9"
    + "973CA69E2285F9F991D65BE62AC07AE8")]
[assembly: InternalsVisibleTo("YJC.Toolkit.WebApp, PublicKey=00240000048000009400000006020000"
    + "00240000525341310004000001000100CFD581E82257611BA519160619FD26B7"
    + "FAB3EE8B69D29E3662F2D0FA16D0CCFF5483F375BE6BA603B06479B3AB4D6FDB"
    + "1D1CF6C3D6195AE980783ACD2267064A4366B9A3CD10DFC29BFF259058D07085"
    + "4807DFE5A9DD79F49F58245DDAB1573D8BC42F18479F224B3525EB104AEE31E9"
    + "973CA69E2285F9F991D65BE62AC07AE8")]
[assembly: InternalsVisibleTo("YJC.Toolkit.TestApp, PublicKey=00240000048000009400000006020000"
    + "00240000525341310004000001000100CFD581E82257611BA519160619FD26B7"
    + "FAB3EE8B69D29E3662F2D0FA16D0CCFF5483F375BE6BA603B06479B3AB4D6FDB"
    + "1D1CF6C3D6195AE980783ACD2267064A4366B9A3CD10DFC29BFF259058D07085"
    + "4807DFE5A9DD79F49F58245DDAB1573D8BC42F18479F224B3525EB104AEE31E9"
    + "973CA69E2285F9F991D65BE62AC07AE8")]
[assembly: InternalsVisibleTo("YJC.Toolkit.WpfApp, PublicKey=00240000048000009400000006020000"
    + "00240000525341310004000001000100CFD581E82257611BA519160619FD26B7"
    + "FAB3EE8B69D29E3662F2D0FA16D0CCFF5483F375BE6BA603B06479B3AB4D6FDB"
    + "1D1CF6C3D6195AE980783ACD2267064A4366B9A3CD10DFC29BFF259058D07085"
    + "4807DFE5A9DD79F49F58245DDAB1573D8BC42F18479F224B3525EB104AEE31E9"
    + "973CA69E2285F9F991D65BE62AC07AE8")]
[assembly: InternalsVisibleTo("YJC.Toolkit.Data, PublicKey=00240000048000009400000006020000"
    + "00240000525341310004000001000100CFD581E82257611BA519160619FD26B7"
    + "FAB3EE8B69D29E3662F2D0FA16D0CCFF5483F375BE6BA603B06479B3AB4D6FDB"
    + "1D1CF6C3D6195AE980783ACD2267064A4366B9A3CD10DFC29BFF259058D07085"
    + "4807DFE5A9DD79F49F58245DDAB1573D8BC42F18479F224B3525EB104AEE31E9"
    + "973CA69E2285F9F991D65BE62AC07AE8")]
[assembly: InternalsVisibleTo("YJC.Toolkit.AdoData, PublicKey=00240000048000009400000006020000"
    + "00240000525341310004000001000100CFD581E82257611BA519160619FD26B7"
    + "FAB3EE8B69D29E3662F2D0FA16D0CCFF5483F375BE6BA603B06479B3AB4D6FDB"
    + "1D1CF6C3D6195AE980783ACD2267064A4366B9A3CD10DFC29BFF259058D07085"
    + "4807DFE5A9DD79F49F58245DDAB1573D8BC42F18479F224B3525EB104AEE31E9"
    + "973CA69E2285F9F991D65BE62AC07AE8")]
[assembly: InternalsVisibleTo("YJC.Toolkit.Evaluator, PublicKey=00240000048000009400000006020000"
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

// 配置插件工厂
[assembly: AssemblyPlugInFactory(typeof(SerializerPlugInFactory))]
[assembly: AssemblyPlugInFactory(typeof(CacheItemCreatorPlugInFactory))]
[assembly: AssemblyPlugInFactory(typeof(CacheDependencyConfigFactory))]
[assembly: AssemblyPlugInFactory(typeof(ExpressionPlugInFactory))]
[assembly: AssemblyPlugInFactory(typeof(ParamExpressionPlugInFactory))]
[assembly: AssemblyPlugInFactory(typeof(RegClassPlugInFactory))]
[assembly: AssemblyPlugInFactory(typeof(RegTypeFactory))]
[assembly: AssemblyPlugInFactory(typeof(EvaluateAdditionPlugInFactory))]
[assembly: AssemblyPlugInFactory(typeof(CacheDependencyStoreConfigFactory))]
[assembly: AssemblyPlugInFactory(typeof(ConfigTypeFactory))]
[assembly: AssemblyPlugInFactory(typeof(CacheCreatorPlugInFactory))]
[assembly: AssemblyPlugInFactory(typeof(DefaultValueTypeFactory))]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
[assembly: Initialization(typeof(ToolkitInitialization))]