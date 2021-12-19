namespace Composite.Task1
{
    public class InputText: Element
    {
        string name;
        string value;

        public InputText(string name, string value): base("inputText")
        {
            this.name = name;
            this.value = value;
        }

        public override string ConvertToString()
        {
            return $"<{elementName} name='{name}' value='{value}'/>";
        }
    }
}
