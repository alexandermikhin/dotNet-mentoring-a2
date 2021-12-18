using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapter
{
    class Program
    {
        static void Main(string[] args)
        {
            var elements = new Elements<string>();
            elements.Els.Add("item");
            var containerAdapter = new ContainerAdapter<string>(elements);
            var printer = new Printer();
            printer.Print(containerAdapter);
        }
    }
}
