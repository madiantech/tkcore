using System;
using System.Diagnostics;
using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public class RazorBaseTemplate : MarshalByRefObject
    {
        private ExecuteContext fContext;

        /// <summary>
        /// You can pass in a generic context object
        /// to use in your template code
        /// </summary>
        public dynamic ViewBag
        {
            get
            {
                return fContext.ViewBag;
            }
        }

        public ExecuteContext Context
        {
            get
            {
                return fContext;
            }
        }

        public ITemplateService TemplateService { get; internal set; }

        public dynamic Model { get; internal set; }

        private void WritePositionTaggedLiteral(Tuple<string, int> value)
        {
            fContext.CurrentWriter.Write(value.Item1);
        }

        protected virtual void RenderLayout()
        {
        }

        /// <summary>
        /// This method is called upon instantiation
        /// and allows passing custom configuration
        /// data to the template from the Engine.
        /// 
        /// This method can then be overridden        
        /// </summary>
        /// <param name="configurationData"></param>
        public virtual void InitializeTemplate(object configurationData)
        {
        }

        public string Run(ExecuteContext context)
        {
            fContext = context;

            bool isCreated = fContext.CreateWriter();
            string result;
            try
            {
                Execute();
                RenderLayout();
            }
            catch (Exception ex)
            {
                if (fContext.Config.RaiseOnRunError)
                    throw new RazorRuntimeException(this, ex);
            }
            finally
            {
                if (isCreated)
                    result = fContext.DisposeWriter();
                else
                    result = string.Empty;
            }
            return result;
        }

        public virtual void Write(object value)
        {
            fContext.CurrentWriter.Write(value);
        }

        public virtual void WriteTo(TextWriter writer, object value)
        {
            TkDebug.AssertArgumentNull(writer, "writer", this);

            if (value == null) return;

            writer.Write(value);
        }

        public virtual void WriteAttribute(string name, Tuple<string, int> prefix,
            Tuple<string, int> suffix, params Tuple<Tuple<string, int>, Tuple<object, int>, bool>[] values)
        {
            bool first = true;
            bool wroteSomething = false;
            if (values.Length == 0)
            {
                // Explicitly empty attribute, so write the prefix and suffix
                WritePositionTaggedLiteral(prefix);
                WritePositionTaggedLiteral(suffix);
            }
            else
            {
                for (int i = 0; i < values.Length; i++)
                {
                    var attrVal = values[i];
                    Tuple<object, int> val = attrVal.Item2;

                    bool? boolVal;
                    if (val.Item1 is bool)
                        boolVal = (bool)val.Item1;
                    else
                        boolVal = null;

                    if (val.Item1 != null && (boolVal == null || boolVal.Value))
                    {
                        string valStr = val.Item1 as string;
                        if (valStr == null)
                        {
                            valStr = val.Item1.ToString();
                        }
                        if (boolVal != null)
                        {
                            Debug.Assert(boolVal.Value);
                            valStr = name;
                        }

                        if (first)
                        {
                            WritePositionTaggedLiteral(prefix);
                            first = false;
                        }
                        else
                            WritePositionTaggedLiteral(attrVal.Item1);

                        if (attrVal.Item3)
                            WriteLiteral(valStr);
                        else
                            Write(valStr); // Write value

                        wroteSomething = true;
                    }
                }
                if (wroteSomething)
                {
                    WritePositionTaggedLiteral(suffix);
                }
            }
        }

        public virtual void WriteString(object value)
        {
            fContext.CurrentWriter.Write(value);
            fContext.CurrentWriter.Write("\r\n");
        }

        public virtual void WriteLiteral(object value)
        {
            //if (value != null && value.ToString() == "\r\n")
            //    return;
            fContext.CurrentWriter.Write(value);
        }

        public virtual void WriteLiteralTo(TextWriter writer, string literal)
        {
            TkDebug.AssertArgumentNull(writer, "writer", this);

            if (literal == null)
                return;
            writer.Write(literal);
        }

        public virtual bool IsSectionDefined(string name)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, "name", this);


            return fContext.GetSectionDelegate(name) != null;
        }

        public void DefineSection(string name, Action action)
        {
            fContext.DefineSection(name, action);
        }

        public virtual string RenderSection(string name)
        {
            return RenderSection(name, null);
        }

        public virtual string RenderSectionIfDefined(string name)
        {
            if (IsSectionDefined(name))
                return RenderSection(name);
            else
                return string.Empty;
        }

        public virtual string RenderSection(string name, object sectionData)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, "name", this);

            var action = fContext.GetSectionDelegate(name);

            if (action != null)
                action();

            return string.Empty;
        }

        /// <summary>
        /// Razor Parser overrides this method
        /// </summary>
        public virtual void Execute()
        {
        }
    }
}

