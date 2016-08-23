using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core.Util
{
    public class Erro
    {
        public TipoErro TipoErro { get; set; }
        public ErroNegocio ErroNegocio { get; set; }
        public Exception Exception { get; set; }
    }
}
