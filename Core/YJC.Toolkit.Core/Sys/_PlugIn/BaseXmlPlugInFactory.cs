using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace YJC.Toolkit.Sys
{
    public abstract class BaseXmlPlugInFactory : BaseInstancePlugInFactory
    {
        private readonly List<XmlPlugInAttribute> fXmlPlugInAttributes;
        private readonly Dictionary<string, Type> fBaseClassTypes;
        private readonly Dictionary<string, XmlRegItem> fXmlPlugIns;
        private Type fDefaultBaseClassType;

        protected BaseXmlPlugInFactory(string name, string description)
            : base(name, description)
        {
            fBaseClassTypes = new Dictionary<string, Type>();
            fXmlPlugIns = new Dictionary<string, XmlRegItem>();
            fXmlPlugInAttributes = new List<XmlPlugInAttribute>();
            SearchAttribute();
        }

        public override int Count
        {
            get
            {
                return fXmlPlugIns.Count + base.Count;
            }
        }

        private void SearchAttribute()
        {
            Type type = GetType();
            Attribute[] temp = Attribute.GetCustomAttributes(type, typeof(XmlPlugInAttribute), false);
            if (temp.Length > 0)
            {
                foreach (XmlPlugInAttribute item in temp)
                    fXmlPlugInAttributes.Add(item);

                temp = Attribute.GetCustomAttributes(type, typeof(XmlBaseClassAttribute), false);
                foreach (XmlBaseClassAttribute item in temp)
                    AddBaseClassAttribute(item);
            }
        }

        private Type GetBaseClassType(string baseClass)
        {
            Type result;
            fBaseClassTypes.TryGetValue(baseClass, out result);
            return result ?? fDefaultBaseClassType;
        }

        private void AddPlugInXmlItem(IXmlPlugInItem item, string fileName)
        {
            Type baseClassType = GetBaseClassType(item.BaseClass);
            if (Contains(item.RegName))
            {
                TkDebug.ThrowIfNoGlobalVariable();
                BaseGlobalVariable.Current.AddXmlError(item.RegName, fileName, PlugInErrorType.Duplicate);
            }
            else
            {
                TkTrace.LogInfo($"工厂[{Description}]添加注册名为{item.RegName}的插件");
                fXmlPlugIns.Add(item.RegName, new XmlRegItem(item.RegName, item, baseClassType, fileName));
                Add(item.RegName, item, baseClassType);
            }
        }

        protected virtual void Add(string regName, IXmlPlugInItem plugItem, Type baseType)
        {
        }

        private void AddPlugInXmlItems(IEnumerable items, string fileName)
        {
            foreach (IXmlPlugInItem item in items)
                AddPlugInXmlItem(item, fileName);
        }

        private void AddPlugInXmlItems(IEnumerator items, string fileName)
        {
            while (items.MoveNext())
            {
                IXmlPlugInItem item = items.Current as IXmlPlugInItem;
                AddPlugInXmlItem(item, fileName);
            }
        }

        private void AddXmlConfigXml(ToolkitConfig config, XmlConfigInfo configInfo)
        {
            string path = Path.GetFileName(config.FullPath);
            foreach (XmlPlugInItemInfo configItem in configInfo.ConfigItems)
            {
                object value = ObjectUtil.GetValue(configItem.PropertyInfo, config);
                if (value != null)
                    switch (configItem.Type)
                    {
                        case XmlPlugInItemType.Single:
                            AddPlugInXmlItem(value as IXmlPlugInItem, path);
                            break;

                        case XmlPlugInItemType.Enumerable:
                            AddPlugInXmlItems(value as IEnumerable, path);
                            break;

                        case XmlPlugInItemType.Enumerator:
                            AddPlugInXmlItems(value as IEnumerator, path);
                            break;
                    }
            }
        }

        private void SearchXmlFile(XmlPlugInAttribute attribute, string file)
        {
            TkTrace.LogInfo($"工厂[{Description}]搜索Xml文件{file}");
            ToolkitConfig configXml = ObjectUtil.CreateObject(attribute.XmlConfigType).Convert<ToolkitConfig>();
            configXml.ReadXmlFromFile(file);
            AddXmlConfigXml(configXml, attribute.ConfigInfo);
        }

        private void SearchXmlPlugInFile(XmlPlugInAttribute attribute, string fileName)
        {
            //if (attribute.IsVersionSwitcher)
            //{
            //    XmlVersionSwitcher switcher = ObjectUtil.CreateObject(attribute.XmlConfigType)
            //        as XmlVersionSwitcher;
            //    switcher.LoadFromFile(fileName, AddXmlConfigXml);
            //}
            //else
            try
            {
                SearchXmlFile(attribute, fileName);
            }
            catch (Exception ex)
            {
            }
        }

        private void SearchXml(XmlPlugInAttribute attribute, string path)
        {
            string[] files = Directory.GetFiles(path, FileUtil.GetXmlFilePattern(
                attribute.SearchPattern), SearchOption.AllDirectories);

            foreach (string file in files)
                SearchXmlPlugInFile(attribute, file);
        }

        private void SearchXmlPlugIn(XmlPlugInAttribute attribute, string basePath)
        {
            string path = Path.Combine(basePath, attribute.XmlPath);
            if (!Directory.Exists(path))
                return;
            //if (attribute.IsVersionSwitcher)
            //{
            //    XmlVersionSwitcher switcher = ObjectUtil.CreateObject(attribute.XmlConfigType)
            //        as XmlVersionSwitcher;
            //    switcher.LoadFromDirectory(path, attribute.SearchPattern, AddXmlConfigXml);
            //}
            //else
            SearchXml(attribute, path);
        }

        private void AssertXmlPlugIn()
        {
            //fXmlPlugInAttributes.Sort(XmlPlugInAttributeComparer.Comparer);
            TkDebug.Assert(fBaseClassTypes.Count > 0, string.Format(ObjectUtil.SysCulture,
                "类型{0}既然附着了XmlPlugInAttribute，就必须至少附着一个PlugInBaseClassAttribute以指明通过配置生成的类型",
                GetType()), this);
            if (fDefaultBaseClassType == null)
                fDefaultBaseClassType = fBaseClassTypes.First().Value;
        }

        private void SearchXmlPlugInPath(string basePath)
        {
            AssertXmlPlugIn();
            fXmlPlugInAttributes.ForEach(
                attribute => SearchXmlPlugIn(attribute, basePath));
        }

        public override bool Contains(string regName)
        {
            if (string.IsNullOrEmpty(regName))
                return false;

            if (fXmlPlugIns.ContainsKey(regName))
                return true;
            else
                return base.Contains(regName);
        }

        internal override BaseRegItem GetRegItem(string regName)
        {
            XmlRegItem result = ObjectUtil.TryGetValue(fXmlPlugIns, regName);
            if (result != null)
                return result;

            return base.GetRegItem(regName);
        }

        protected override void SearchXmlPath(string basePath)
        {
            SearchXmlPlugInPath(basePath);
        }

        internal void EnumableXmlPlugIn(Action<IXmlPlugInItem, string, Type, BasePlugInAttribute> action)
        {
            if (action == null)
                return;

            foreach (var item in fXmlPlugIns)
            {
                var value = item.Value;
                action(value.XmlConfig, value.FileName, value.BaseClassType, value.Attribute);
            }
        }

        public void EnumableXmlPlugIn(Action<IXmlPlugInItem, Type, BasePlugInAttribute> action)
        {
            if (action == null)
                return;

            foreach (var item in fXmlPlugIns)
            {
                var value = item.Value;
                action(value.XmlConfig, value.BaseClassType, value.Attribute);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddBaseClassAttribute(XmlBaseClassAttribute attribute)
        {
            TkDebug.AssertArgumentNull(attribute, "attribute", this);

            if (!fBaseClassTypes.ContainsKey(attribute.BaseClass))
            {
                fBaseClassTypes.Add(attribute.BaseClass, attribute.RegType);
                if (attribute.Default)
                {
                    TkDebug.Assert(fDefaultBaseClassType == null, string.Format(ObjectUtil.SysCulture,
                       "XmlBaseClassAttribute只能有一个属性声明为Default，在声明{0}之前已经有属性设置了Default=true了，请确认",
                       attribute.BaseClass), this);
                    fDefaultBaseClassType = attribute.RegType;
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddXmlPlugInAttribute(XmlPlugInAttribute attribute)
        {
            TkDebug.AssertArgumentNull(attribute, "attribute", this);

            fXmlPlugInAttributes.Add(attribute);
        }
    }
}