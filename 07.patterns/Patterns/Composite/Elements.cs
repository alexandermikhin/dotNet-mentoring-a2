using System.Collections.Generic;
using System.Text;

namespace Composite
{
    public abstract class Element
    {
        protected string elementName;

        protected Element(string name)
        {
            elementName = name;
        }

        public virtual void AddComponent(Element element)
        { }

        public abstract string ConvertToString();
    }

    public class InputText : Element
    {
        string name;
        string value;

        public InputText(string name, string value) : base("inputText")
        {
            this.name = name;
            this.value = value;
        }

        public override string ConvertToString()
        {
            return $"<{elementName} name=\"{name}\" value=\"{value}\" />";
        }
    }
    public class LabelText : Element
    {
        string value;

        public LabelText(string value) : base("label")
        {
            this.value = value;
        }

        public override string ConvertToString()
        {
            return $"<{elementName} value=\"{value}\" />";
        }
    }

    public class Form : Element
    {
        string name;
        protected List<Element> children;
        private StringBuilder builder;

        public Form(string name) : base("form")
        {
            this.name = name;
        }

        public override void AddComponent(Element element)
        {
            if (children == null)
            {
                children = new List<Element>();
            }

            children.Add(element);
        }

        public override string ConvertToString()
        {
            builder = new StringBuilder();
            var innerXml = GetInnerXml();

            return $"<{elementName} name=\"{name}\">{innerXml}</{elementName}>";
        }

        private string GetInnerXml()
        {
            children.ForEach(element => builder.Append(element.ConvertToString()));

            return builder.ToString();
        }
    }
}
