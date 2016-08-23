using Imposto.Core.Domain;
using Imposto.Core.Service;
using Imposto.Core.Util;
using System;
using System.Data;
using System.Windows.Forms;

namespace TesteImposto
{
    public partial class FormImposto : Form
    {
        private Pedido pedido = new Pedido();
        private readonly NotaFiscalService notaFiscalService;

        public FormImposto()
        {
            InitializeComponent();
            dataGridViewPedidos.AutoGenerateColumns = true;                       
            dataGridViewPedidos.DataSource = GetTablePedidos();
            ResizeColumns();
            notaFiscalService = NotaFiscalServiceConstrutor.notaFiscalService();
        }

        private void ResizeColumns()
        {
            double mediaWidth = dataGridViewPedidos.Width / dataGridViewPedidos.Columns.GetColumnCount(DataGridViewElementStates.Visible);

            for (int i = dataGridViewPedidos.Columns.Count - 1; i >= 0; i--)
            {
                var coluna = dataGridViewPedidos.Columns[i];
                coluna.Width = Convert.ToInt32(mediaWidth);
            }   
        }

        private object GetTablePedidos()
        {
            DataTable table = new DataTable("pedidos");
            table.Columns.Add(new DataColumn("Nome do produto", typeof(string)));
            table.Columns.Add(new DataColumn("Codigo do produto", typeof(string)));
            table.Columns.Add(new DataColumn("Valor", typeof(decimal)));
            table.Columns.Add(new DataColumn("Brinde", typeof(bool)));
                     
            return table;
        }

        private void buttonGerarNotaFiscal_Click(object sender, EventArgs e)
        {
            var retorno = new RetNotaFiscal();

            if (!ValidaDadosNotaFiscal()) return;

            try
            {
                pedido = new Pedido();
                pedido.EstadoOrigem = txtEstadoOrigem.Text;
                pedido.EstadoDestino = txtEstadoDestino.Text;
                pedido.NomeCliente = textBoxNomeCliente.Text;

                DataTable table = (DataTable)dataGridViewPedidos.DataSource;

                foreach (DataRow row in table.Rows)
                {
                    pedido.ItensDoPedido.Add(
                        new PedidoItem()
                        {
                            Brinde = Convert.ToBoolean(row["Brinde"]),
                            CodigoProduto = row["Codigo do produto"].ToString(),
                            NomeProduto = row["Nome do produto"].ToString(),
                            ValorItemPedido = Convert.ToDouble(row["Valor"].ToString())
                        });
                }
                
                retorno = notaFiscalService.GerarNotaFiscal(pedido);
                if (retorno.Info.Sucesso)
                    MessageBox.Show("Operação efetuada com sucesso");
                else
                {
                    var message = "Nota Fiscal não gerada:";
                    foreach (var itemErro in retorno.Info.Erros)
                    {
                        message = message + string.Join(Environment.NewLine, Environment.NewLine, itemErro.ErroNegocio.Mensagem); 
                    }
                    MessageBox.Show(message);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Nota Fiscal não gerada. Favor validar o formulário. Erro: " + ex.Message);
            }
        }

        public bool ValidaDadosNotaFiscal()
        {
            if (string.IsNullOrEmpty(textBoxNomeCliente.Text))
            {
                MessageBox.Show("Nome do Cliente não informado.");
                return false;
            }

            if (string.IsNullOrEmpty(txtEstadoOrigem.Text))
            {
                MessageBox.Show("Estado Origem não informado.");
                return false;
            }

            if (string.IsNullOrEmpty(txtEstadoDestino.Text))
            {
                MessageBox.Show("Estado Destino não informado.");
                return false;
            }

            DataTable table = (DataTable)dataGridViewPedidos.DataSource;
            foreach (DataRow row in table.Rows)
            {
                if (row["Brinde"] == null)
                {
                    MessageBox.Show("Brinde não informado.");
                    return false;
                }
                if (string.IsNullOrEmpty(row["Brinde"].ToString()))
                {
                    row["Brinde"] = false;
                }
                if (row["Codigo do produto"] == null || string.IsNullOrEmpty(row["Codigo do produto"].ToString()))
                {
                    MessageBox.Show("Código do produto não informado.");
                    return false;
                }
                if (row["Nome do produto"] == null || string.IsNullOrEmpty(row["Nome do produto"].ToString()))
                {
                    MessageBox.Show("Nome do produto não informado.");
                    return false;
                }
                if (row["Valor"] == null || string.IsNullOrEmpty(row["Valor"].ToString()))
                {
                    MessageBox.Show("Valor não informado.");
                    return false;
                }
                double sValor;
                if (!double.TryParse(row["Valor"].ToString(), out sValor))
                {
                    MessageBox.Show("Valor inválido.");
                    return false;
                }
            }
            
            return true;
        }
    }
}
