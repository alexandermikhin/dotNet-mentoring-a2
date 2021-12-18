using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapter
{
    public class Printer
    {
        public void Print<T>(IContainer<T> container)
        {
            foreach (var item in container.Items)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }

}
