using Imposto.Core.Domain;
using Imposto.Core.Util;

namespace Imposto.Core.Data
{
    public interface INotaFiscalItemRepository
    {
        RetNotaFiscalItem Inserir(NotaFiscalItem notaFiscalItem);
    } 
}
