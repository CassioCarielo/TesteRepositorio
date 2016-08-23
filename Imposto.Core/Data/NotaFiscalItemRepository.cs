using Imposto.Core.Domain;
using Imposto.Core.Util;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Imposto.Core.Data
{
    public class NotaFiscalItemRepository : INotaFiscalItemRepository
    {
        private Context context;

        public RetNotaFiscalItem Inserir(NotaFiscalItem notaFiscalItem)
        {
            var retorno = new RetNotaFiscalItem { Info = new Info() };

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName = "@pId", Value= notaFiscalItem.Id, SqlDbType = SqlDbType.Int},
                    new SqlParameter() {ParameterName = "@pIdNotaFiscal", SqlDbType = SqlDbType.Int, Value= notaFiscalItem.IdNotaFiscal},
                    new SqlParameter() {ParameterName = "@pCfop", SqlDbType = SqlDbType.VarChar, Value= notaFiscalItem.Cfop},
                    new SqlParameter() {ParameterName = "@pTipoIcms", SqlDbType = SqlDbType.VarChar, Value= notaFiscalItem.TipoIcms},
                    new SqlParameter() {ParameterName = "@pBaseIcms", SqlDbType = SqlDbType.Float, Value= notaFiscalItem.BaseIcms},
                    new SqlParameter() {ParameterName = "@pAliquotaIcms", SqlDbType = SqlDbType.Float, Value= notaFiscalItem.AliquotaIcms},
                    new SqlParameter() {ParameterName = "@pValorIcms", SqlDbType = SqlDbType.Float, Value= notaFiscalItem.ValorIcms},
                    new SqlParameter() {ParameterName = "@pNomeProduto", SqlDbType = SqlDbType.VarChar, Value= notaFiscalItem.NomeProduto},
                    new SqlParameter() {ParameterName = "@pCodigoProduto", SqlDbType = SqlDbType.VarChar, Value= notaFiscalItem.CodigoProduto},
                };
                using (context = new Context())
                {
                    retorno.Info = context.ExecuteCommand("dbo.P_NOTA_FISCAL_ITEM", parameters);
                }
            }
            catch (System.Exception ex)
            {
                retorno.Info.Erros.Add(new Resultado().Retorno(TipoErroEnum.ErroNegocio, 999, "Erro na inclusão do item da NotaFiscal: " + ex.Message));
            }

            return retorno;
        }
    }
}
