using Imposto.Core.Domain;
using Imposto.Core.Util;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Imposto.Core.Data
{
    public class NotaFiscalRepository : INotaFiscalRepository
    {
        private Context context;
        
        public RetNotaFiscal Inserir(NotaFiscal notaFiscal)
        {
            var retorno = new RetNotaFiscal { Info = new Info() };

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName = "@pId", Value= notaFiscal.Id, SqlDbType = SqlDbType.Int}, //, Direction = ParameterDirection.Output} 
                    new SqlParameter() {ParameterName = "@pNumeroNotaFiscal", SqlDbType = SqlDbType.Int, Value= notaFiscal.NumeroNotaFiscal},
                    new SqlParameter() {ParameterName = "@pSerie", SqlDbType = SqlDbType.Int, Value= notaFiscal.Serie},
                    new SqlParameter() {ParameterName = "@pNomeCliente", SqlDbType = SqlDbType.VarChar, Value= notaFiscal.NomeCliente},
                    new SqlParameter() {ParameterName = "@pEstadoDestino", SqlDbType = SqlDbType.VarChar, Value= notaFiscal.EstadoDestino},
                    new SqlParameter() {ParameterName = "@pEstadoOrigem", SqlDbType = SqlDbType.VarChar, Value= notaFiscal.EstadoOrigem},
                };
                using (context = new Context())
                {
                    retorno.Info = context.ExecuteCommand("dbo.P_NOTA_FISCAL", parameters);
                }
            }
            catch (System.Exception ex)
            {
                retorno.Info.Erros.Add(new Resultado().Retorno(TipoErroEnum.ErroNegocio, 999, "Erro na inclusão da NotaFiscal: " + ex.Message));
            }

            return retorno;
        }
    } 
}
