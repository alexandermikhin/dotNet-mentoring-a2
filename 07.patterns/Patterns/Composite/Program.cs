using System;

namespace Composite
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = new InputText("myInput", "myInputValue");
            var label = new LabelText("myLabel");
            var form = new Form("myForm");
            form.AddComponent(input);
            form.AddComponent(label);
            var elements = new Element[]
            {
                input,
                label,
                form,
            };

            foreach (var element in elements)
            {
                Console.WriteLine(element.ConvertToString());
            }
        }
    }
}
