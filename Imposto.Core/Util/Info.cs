using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core.Util
{
    public class Info
    {
        public Info()
        {
            Erros = new List<Erro>();
        }

        public bool Sucesso
        {
            get { return !Erros.Any(); }
            set { }
        }
        public int Codigo { get; set; }
        public List<Erro> Erros { get; set; }
    }
}
