using System;

namespace Composite.Task2
{
    public class LabelText : IComponent
    {
        string value;
        const string elementName = "label";

        public LabelText(string value)
        {
            this.value = value;
        }

        public string ConvertToString(int depth = 0)
        {
            return $"{new string(' ', depth)}<{elementName} value='{value}'/>";
        }
    }
}
