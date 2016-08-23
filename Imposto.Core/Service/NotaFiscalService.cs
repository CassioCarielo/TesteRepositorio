using Imposto.Core.Data;
using Imposto.Core.Domain;
using Imposto.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imposto.Core.Service
{
    public class NotaFiscalService
    {
        private readonly NotaFiscalRepository notaFiscalRepository;
        private readonly NotaFiscalItemRepository notaFiscalItemRepository;

        public NotaFiscalService(NotaFiscalRepository repo, NotaFiscalItemRepository repo2)
        {
            notaFiscalRepository = repo;
            notaFiscalItemRepository = repo2;
        }

        public RetNotaFiscal ValidarNotaFiscal(Pedido pedido, List<string> listEstados)
        {
            var retorno = new RetNotaFiscal { Info = new Info() };            

            if (pedido.NomeCliente == null || string.IsNullOrEmpty(pedido.NomeCliente))
                retorno.Info.Erros.Add(new Resultado().Retorno(TipoErroEnum.ErroNegocio, 999, "Cliente não informado."));

            if (pedido.EstadoOrigem == null || string.IsNullOrEmpty(pedido.EstadoOrigem))
                retorno.Info.Erros.Add(new Resultado().Retorno(TipoErroEnum.ErroNegocio, 999, "Estado Origem não informado."));

            if (pedido.EstadoDestino == null || string.IsNullOrEmpty(pedido.EstadoDestino))
                retorno.Info.Erros.Add(new Resultado().Retorno(TipoErroEnum.ErroNegocio, 999, "Estado Destino não informado."));

            if (pedido.ItensDoPedido == null || pedido.ItensDoPedido.Count == 0)
                retorno.Info.Erros.Add(new Resultado().Retorno(TipoErroEnum.ErroNegocio, 999, "Itens do Pedido não informado."));

            if (!retorno.Info.Sucesso) return retorno;

            if (!listEstados.Any(x => x.Contains(pedido.EstadoOrigem)))
                retorno.Info.Erros.Add(new Resultado().Retorno(TipoErroEnum.ErroNegocio, 999, "Estado Origem do Pedido inválido."));

            if (!listEstados.Any(x => x.Contains(pedido.EstadoDestino)))
                retorno.Info.Erros.Add(new Resultado().Retorno(TipoErroEnum.ErroNegocio, 999, "Estado Origem do Pedido inválido."));

            if (!retorno.Info.Sucesso) return retorno;

            int i = 1;
            foreach (PedidoItem itemPedido in pedido.ItensDoPedido)
            {
                if (itemPedido.NomeProduto == null || string.IsNullOrEmpty(itemPedido.NomeProduto))
                    retorno.Info.Erros.Add(new Resultado().Retorno(TipoErroEnum.ErroNegocio, 999, "Nome do Produto do Item " + i + " do Pedido não informado."));

                if (itemPedido.CodigoProduto == null || string.IsNullOrEmpty(itemPedido.CodigoProduto))
                    retorno.Info.Erros.Add(new Resultado().Retorno(TipoErroEnum.ErroNegocio, 999, "Código do Produto do Item " + i + " do Pedido não informado."));

                if (itemPedido.ValorItemPedido.Equals(0))
                    retorno.Info.Erros.Add(new Resultado().Retorno(TipoErroEnum.ErroNegocio, 999, "Valor do Produto do Item " + i + " do Pedido não informado."));

                i++;
            }

            return retorno;
        }

        public RetNotaFiscal GerarNotaFiscal(Domain.Pedido pedido)
        {
            var retorno = new RetNotaFiscal { Info = new Info() };
            RetNotaFiscal retNotaFiscal = new RetNotaFiscal();
            NotaFiscal _notafiscal = new NotaFiscal();
            List<string> listEstados = ListaEstados();

            retorno = ValidarNotaFiscal(pedido, listEstados);
            if (!retorno.Info.Sucesso) return retorno;

            _notafiscal.NumeroNotaFiscal = 99999;
            _notafiscal.Serie = new Random().Next(Int32.MaxValue);
            _notafiscal.NomeCliente = pedido.NomeCliente;
            _notafiscal.EstadoDestino = pedido.EstadoDestino;
            _notafiscal.EstadoOrigem = pedido.EstadoOrigem;
            List<NotaFiscalItem> listNotaFiscalItem = new List<NotaFiscalItem>();

            foreach (PedidoItem itemPedido in pedido.ItensDoPedido)
            {
                NotaFiscalItem notaFiscalItem = new NotaFiscalItem();

                int cfop = 6000;
                //Sem break porque SE se repete no estado destino, provavelmente deveria ser SP um deles
                foreach (var itemEstadosDestino in listEstados)
                {
                    if (_notafiscal.EstadoDestino == itemEstadosDestino)
                        foreach (var itemEstadosOrigem in listEstados)
                            if (_notafiscal.EstadoDestino == itemEstadosOrigem)
                                notaFiscalItem.Cfop = cfop.ToString("n0");
                    cfop++;
                }

                if (_notafiscal.EstadoDestino == _notafiscal.EstadoOrigem)
                {
                    notaFiscalItem.TipoIcms = "60";
                    notaFiscalItem.AliquotaIcms = 0.18;
                }
                else
                {
                    notaFiscalItem.TipoIcms = "10";
                    notaFiscalItem.AliquotaIcms = 0.17;
                }
                if (notaFiscalItem.Cfop == "6.009")
                {
                    notaFiscalItem.BaseIcms = itemPedido.ValorItemPedido * 0.90; //redução de base
                }
                else
                {
                    notaFiscalItem.BaseIcms = itemPedido.ValorItemPedido;
                }
                notaFiscalItem.ValorIcms = notaFiscalItem.BaseIcms * notaFiscalItem.AliquotaIcms;

                if (itemPedido.Brinde)
                {
                    notaFiscalItem.TipoIcms = "60";
                    notaFiscalItem.AliquotaIcms = 0.18;
                    notaFiscalItem.ValorIcms = notaFiscalItem.BaseIcms * notaFiscalItem.AliquotaIcms;
                }
                notaFiscalItem.NomeProduto = itemPedido.NomeProduto;
                notaFiscalItem.CodigoProduto = itemPedido.CodigoProduto;

                listNotaFiscalItem.Add(notaFiscalItem);
            }
            _notafiscal.ItensDaNotaFiscal = listNotaFiscalItem;

            //Insere a Nota Fiscal desacoplada
            retorno = notaFiscalRepository.Inserir(_notafiscal);
            _notafiscal.Id = retorno.Info.Codigo;
            if (!retorno.Info.Sucesso) return retorno;

            //Insere os itens da Nota Fiscal
            foreach (var item in _notafiscal.ItensDaNotaFiscal)
            {
                item.IdNotaFiscal = _notafiscal.Id;
                var retornoItem = notaFiscalItemRepository.Inserir(item);
                if (!retornoItem.Info.Sucesso) retorno.Info.Erros = retornoItem.Info.Erros;
            }

            retorno.Item = _notafiscal;

            return retorno;
        }

        public List<string> ListaEstados()
        {
            List<string> estados = new List<string>();
            estados.Add("RJ");
            estados.Add("PE");
            estados.Add("MG");
            estados.Add("PB");
            estados.Add("PR");
            estados.Add("PI");
            estados.Add("RO");
            estados.Add("PA");
            estados.Add("SE");
            estados.Add("SP");

            return estados;
        }
    }
}
