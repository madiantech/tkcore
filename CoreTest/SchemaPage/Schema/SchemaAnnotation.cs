using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using YJC.Toolkit.Sys;

namespace Toolkit.SchemaSuite.Schema
{
    internal class SchemaAnnotation
    {
        public SchemaAnnotation(XmlSchemaAnnotation annotation)
        {
            IsEmpty = true;
            if (annotation != null && annotation.Items.Count > 0)
            {
                if (annotation.Items[0] is XmlSchemaDocumentation doc)
                {
                    if (doc.Markup.Length > 0)
                    {
                        Value = doc.Markup[0].Value;
                        if (Value.Contains('`'))
                            Value = Value.Replace("`", "\\`");
                        if (Value.Contains('*'))
                            Value = Value.Replace("*", "\\*");
                        if (Value.Contains('^'))
                            Value = Value.Replace("^", "\\^");
                        IsEmpty = false;
                    }
                }
                if (annotation.Items.Count > 1)
                {
                    if (annotation.Items[1] is XmlSchemaDocumentation doc2)
                    {
                        if (doc2.Markup.Length > 0)
                        {
                            string hint = doc2.Markup[0].Value;
                            IsHide = hint.Contains('!');
                        }
                    }
                }
            }
        }

        public bool IsEmpty { get; }

        public string Value { get; }

        public bool IsHide { get; }

        public override string ToString() => Value;
    }
}