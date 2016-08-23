using Imposto.Core.Domain;
using Imposto.Core.Util;

namespace Imposto.Core.Data
{
    public interface INotaFiscalRepository
    {
        RetNotaFiscal Inserir(NotaFiscal notaFiscal);
    } 
}
