using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imposto.Core.Data;

namespace Imposto.Core.Service
{
    public class NotaFiscalServiceConstrutor
    {
        public static NotaFiscalService notaFiscalService()
        {
            return new NotaFiscalService(new NotaFiscalRepository(), new NotaFiscalItemRepository());
        }
    }
}
