using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapter
{
    public interface IContainer<T>
    {
        IEnumerable<T> Items { get; }
        int Count { get; }
    }
}
