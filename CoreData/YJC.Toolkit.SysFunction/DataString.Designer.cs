﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace YJC.Toolkit.SysFunction {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class DataString {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal DataString() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("YJC.Toolkit.SysFunction.DataString", typeof(DataString).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   重写当前线程的 CurrentUICulture 属性
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 &lt;tk:Toolkit version=&quot;5.0&quot; xmlns:tk=&quot;http://www.qdocuments.net&quot; xmlns:xsi=&quot;http://www.w3.org/2001/XMLSchema-instance&quot;&gt;
        ///  &lt;MetaData&gt;
        ///    &lt;tk:StdCodeTableMetaData TableName=&quot;{0}&quot; ShowCodeValue=&quot;{1}&quot; ShowPy=&quot;{2}&quot; ShowSort=&quot;{3}&quot; PyCaption=&quot;{4}&quot;/&gt;
        ///  &lt;/MetaData&gt;
        ///  &lt;Source&gt;
        ///    &lt;tk:StdCodeTableSource TableName=&quot;{0}&quot;&gt;
        ///      {5}
        ///    &lt;/tk:StdCodeTableSource&gt;
        ///  &lt;/Source&gt;
        ///  &lt;PageMaker&gt;
        ///      &lt;tk:RazorModuleTemplatePageMaker ModuleTemplate=&quot;SingleDialog&quot;&gt;
        ///        &lt;tk:PageTemplate Style=&quot;List&quot;&gt;
        ///          &lt;tk:R [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string Config {
            get {
                return ResourceManager.GetString("Config", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 &lt;tk:AutoKeyCreator Length=&quot;{0}&quot; /&gt; 的本地化字符串。
        /// </summary>
        internal static string KeyCreator {
            get {
                return ResourceManager.GetString("KeyCreator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 &lt;tk:StdCodeTableMetaData TableName=&quot;{0}&quot; ShowCodeValue=&quot;{1}&quot; ShowPy=&quot;{2}&quot; ShowSort=&quot;{3}&quot; PyCaption=&quot;{4}&quot;/&gt; 的本地化字符串。
        /// </summary>
        internal static string MetaData {
            get {
                return ResourceManager.GetString("MetaData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 &lt;tk:CompositePageMaker&gt;
        ///  &lt;tk:Item&gt;
        ///    &lt;tk:Condition SearchType=&quot;Style&quot; Style=&quot;CFakeDelete CPinyin CRestore&quot;/&gt;
        ///    &lt;tk:PostPageMaker DestUrl=&quot;Custom&quot;&gt;
        ///      &lt;tk:CustomUrl&gt;ListRefresh&lt;/tk:CustomUrl&gt;
        ///    &lt;/tk:PostPageMaker&gt;
        ///  &lt;/tk:Item&gt;
        ///  &lt;tk:Item&gt;
        ///    &lt;tk:Condition SearchType=&quot;True&quot;/&gt;
        ///      &lt;tk:RazorModuleTemplatePageMaker ModuleTemplate=&quot;SingleDialog&quot;&gt;
        ///        &lt;tk:PageTemplate Style=&quot;List&quot;&gt;
        ///          &lt;tk:RazorData&gt;
        ///            &lt;tk:NormalListData Display=&quot;Striped&quot; ShowPage=&quot;false&quot; ShowTitle=&quot;fal [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string PageMaker {
            get {
                return ResourceManager.GetString("PageMaker", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 &lt;tk:StdCodeTableSource TableName=&quot;{0}&quot; AutoPinyin=&quot;{2}&quot;&gt;
        ///  {1}
        ///&lt;/tk:StdCodeTableSource&gt; 的本地化字符串。
        /// </summary>
        internal static string Source {
            get {
                return ResourceManager.GetString("Source", resourceCulture);
            }
        }
    }
}