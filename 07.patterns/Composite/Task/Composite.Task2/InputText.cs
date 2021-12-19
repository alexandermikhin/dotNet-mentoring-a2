using System;

namespace Composite.Task2
{
    public class InputText : IComponent
    {
        string name;
        string value;
        const string elementName = "inputText";

        public InputText(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public string ConvertToString(int depth = 0)
        {
            return $"{new string(' ', depth)}<{elementName} name='{name}' value='{value}'/>";
        }
    }
}
