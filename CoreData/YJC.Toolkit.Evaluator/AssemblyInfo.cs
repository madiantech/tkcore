using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

[assembly: CLSCompliant(true)]

// 配置插件工厂
[assembly: Initialization(typeof(EvaluatorInitialization))]