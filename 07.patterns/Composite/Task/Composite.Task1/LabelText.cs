namespace Composite.Task1
{
    public class LabelText: Element
    {
        string value;

        public LabelText(string value) : base("label")
        {
            this.value = value;
        }

        public override string ConvertToString()
        {
            return $"<{elementName} value='{value}'/>";
        }
    }
}
