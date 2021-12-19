using System.Xml;

namespace Composite
{
    public abstract class Element
    {
        protected XmlElement xmlElement;
        protected readonly string elementName;
        private readonly XmlDocument xmlDocument = new XmlDocument();

        protected Element(string name)
        {
            xmlElement = xmlDocument.CreateElement(name);
        }

        public abstract string ConvertToString();
    }

    public class InputText: Element
    {
        public InputText(string name, string value): base("inputText")
        {
            xmlElement.SetAttribute("name", name);
            xmlElement.SetAttribute("value", value);
        }

        public override string ConvertToString()
        {
            return xmlElement.OuterXml;
        }
    }
    public class LabelText: Element
    {
        public LabelText(string value): base("label")
        {
            xmlElement.SetAttribute("value", value);
        }

        public override string ConvertToString()
        {
            return xmlElement.OuterXml;
        }
    }

}
