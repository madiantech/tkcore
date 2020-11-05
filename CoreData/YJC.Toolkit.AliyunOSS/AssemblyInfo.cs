using System;
using YJC.Toolkit.AliyunOSS;
using YJC.Toolkit.Sys;

// 配置插件工厂
[assembly: Initialization(typeof(AliyunOSSInitialization))]
[assembly: AssemblyPlugInFactory(typeof(AliyunOSSConfigFactory))]