using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core.Util
{
    public class TipoErro
    {
        public int? Codigo { get; set; }
        public TipoErroEnum Descricao { get; set; }
    }
}
