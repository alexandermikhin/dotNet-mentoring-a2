using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapter
{
    public interface IElements<T>
    {
        IEnumerable<T> GetElements();
    }

    public class Elements<T>: IElements<T>
    {
        List<T> _els = new List<T>();

        public List<T> Els => _els;

        public IEnumerable<T> GetElements()
        {
            return Els.AsEnumerable();
        }
    }
}
