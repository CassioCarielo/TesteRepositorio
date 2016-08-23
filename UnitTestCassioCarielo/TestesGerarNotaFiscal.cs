using Imposto.Core.Domain;
using Imposto.Core.Service;
using Imposto.Core.Util;
using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestCassioCarielo
{
    [TestClass]
    public class TestesGerarNotaFiscal
    {
        private readonly NotaFiscalService notaFiscalService;

        public TestesGerarNotaFiscal()
        {
            notaFiscalService = NotaFiscalServiceConstrutor.notaFiscalService();
        }

        [TestMethod]
        public void TesteGerarNFValida()
        {
            var retorno = new RetNotaFiscal();
            Pedido pedido = new Pedido();
            
            pedido.EstadoOrigem = "SP";
            pedido.EstadoDestino = "SP";
            pedido.NomeCliente = "Cliente";

            pedido.ItensDoPedido.Add(
                new PedidoItem()
                {
                    Brinde = true,
                    CodigoProduto = "33",
                    NomeProduto = "Produto Teste",
                    ValorItemPedido = 11
                });

            retorno = notaFiscalService.GerarNotaFiscal(pedido);
            Assert.IsTrue(retorno.Info.Sucesso);
        }

        [TestMethod]
        public void TesteGerarNFInvalida()
        {
            var retorno = new RetNotaFiscal();
            NotaFiscalService notaFiscalService = NotaFiscalServiceConstrutor.notaFiscalService();
            Pedido pedido = new Pedido();

            pedido.EstadoOrigem = "SPS";
            pedido.EstadoDestino = "SP";
            pedido.NomeCliente = "Cliente";

            pedido.ItensDoPedido.Add(
                new PedidoItem()
                {
                    Brinde = true,
                    CodigoProduto = "33",
                    NomeProduto = "Produto Teste",
                    ValorItemPedido = 0
                });

            retorno = notaFiscalService.GerarNotaFiscal(pedido);
            Assert.IsFalse(retorno.Info.Sucesso);
        }
    }
}

