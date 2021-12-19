using System;

namespace Composite
{
    class Program
    {
        static void Main(string[] args)
        {
            var elements = new Element[]
            {
                new InputText("myInput", "myInputValue"),
                new LabelText("myLabel"),
            };

            foreach (var element in elements)
            {
                Console.WriteLine(element.ConvertToString());
            }
        }
    }
}
