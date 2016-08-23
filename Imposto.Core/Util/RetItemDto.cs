using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core.Util
{
    public class RetItemDto<T>
    {
        public T Item { get; set; }
        public Info Info { get; set; }
    }
}
