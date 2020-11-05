using System;
using System.Resources;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Right;
using YJC.Toolkit.SimpleWorkflow;

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
[assembly: InternalsVisibleTo("WfConsole")]
[assembly: InternalsVisibleTo("YJC.Toolkit.SimpleWorkflow.Web")]
[assembly: InternalsVisibleTo("YJC.Toolkit.SimpleWorkflow.WebDesigner")]
[assembly: Initialization(typeof(WorkflowInitialization))]
[assembly: AssemblyPlugInFactory(typeof(MergerConfigFactory))]
[assembly: AssemblyPlugInFactory(typeof(CreatorPlugInFactory))]
[assembly: AssemblyPlugInFactory(typeof(ConnectionPlugInFactory))]
[assembly: AssemblyPlugInFactory(typeof(AutoProcessorPlugInFactory))]
[assembly: AssemblyPlugInFactory(typeof(WorkflowStepConfigFactory))]
[assembly: AssemblyPlugInFactory(typeof(ProcessorPlugInFactory))]
[assembly: AssemblyPlugInFactory(typeof(ProcessorConfigFactory))]
[assembly: AssemblyPlugInFactory(typeof(OperationPlugInFactory))]
[assembly: AssemblyPlugInFactory(typeof(ManualStepUserConfigFactory))]
[assembly: AssemblyPlugInFactory(typeof(AutoProcessorConfigFactory))]
[assembly: AssemblyPlugInFactory(typeof(CreatorConfigFactory))]
[assembly: AssemblyPlugInFactory(typeof(NotifyActionPlugInFactory))]
[assembly: AssemblyPlugInFactory(typeof(NotifyActionConfigFactory))]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]
//[assembly: AssemblyPlugInFactory(typeof())]