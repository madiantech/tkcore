﻿using System;
using System.Reflection;
using System.Runtime.InteropServices;
using YJC.Toolkit.Sys;
using YJC.Toolkit.WeChat;
using YJC.Toolkit.WeChat.Rule;

// 有关程序集的常规信息通过以下 特性集控制。更改这些特性值可修改 与程序集关联的信息。
[assembly: AssemblyCopyright("Copyright ©  2009-2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// 将 ComVisible 设置为 false 使此程序集中的类型 对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型， 则将该类型上的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("ae3dabd4-cf50-4eff-9b64-1593041d162c")]

// 程序集的版本信息由下面四个值组成:
//
// 主版本 次版本 生成号 修订号
//
// 可以指定所有这些值，也可以使用“生成号”和“修订号”的默认值， 方法是按如下所示使用“*”: [assembly: AssemblyVersion("1.0.*")]
[assembly: Initialization(typeof(WeChatToolkitInitialization))]
[assembly: AssemblyPlugInFactory(typeof(ReplyMessagePlugInFactory))]
[assembly: AssemblyPlugInFactory(typeof(ReplyMessageConfigFactory))]
[assembly: AssemblyPlugInFactory(typeof(MatchRuleConfigFactory))]
[assembly: AssemblyPlugInFactory(typeof(RulePlugInFactory))]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]