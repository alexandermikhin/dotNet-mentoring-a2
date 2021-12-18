using System.Collections.Generic;
using System.Linq;

namespace Adapter
{
    class ContainerAdapter<T> : IContainer<T>
    {
        readonly IElements<T> adaptee;

        public ContainerAdapter(IElements<T> elements)
        {
            adaptee = elements;
        }

        public IEnumerable<T> Items => this.adaptee.GetElements();

        public int Count => this.Items.Count();
    }
}
