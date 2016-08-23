using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core.Util
{
    public class Resultado
    {
        public Erro Retorno(TipoErroEnum tipoErroEnum, int codigoErroNegocio, string mensagem)
        {
            int? codigoErro = null;

            switch (tipoErroEnum)
            {
                case TipoErroEnum.ErroNegocio:
                    codigoErro = 1;
                    break;
                case TipoErroEnum.Exception:
                    codigoErro = 2;
                    break;
            }

            var erro = new Erro
            {
                TipoErro = new TipoErro
                {
                    Codigo = codigoErro,
                    Descricao = tipoErroEnum
                },
                ErroNegocio = new ErroNegocio
                {
                    Codigo = codigoErroNegocio,
                    Mensagem = mensagem
                },
            };

            return erro;
        }
    }
}
