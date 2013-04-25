using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HLP.Web;
using System.Text;
using HLP.Dados;

public partial class PesquisarPedidos : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DateTime dDataFinal = DateTime.Now;
            txtDataFinal.Text = dDataFinal.ToString("dd/MM/yyyy");
            txtDataInicial.Text = (dDataFinal.AddDays(-5).ToString("dd/MM/yyyy"));
            string sUser = UsuarioWeb.GetNomeUsuarioConectado(Session);
            if (sUser == "")
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        BaseDAO.CancelarOperacaoObjetoDAO((BaseDAO)Session["ObjetoPedidoDetalhado"]);
        ParametroPesquisaCapoli.InicializarParametroPesquisa(
            "FiltroPedidos", "PEDIDO", this.Session);
        Session["DadosConsultaPedidos"] = null;

    }

    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        ParametroPesquisa objParametros =
            (ParametroPesquisa)Session["FiltroPedidos"];

        StringBuilder strErros = new StringBuilder();

        bool bFiltroValido = (objParametros != null);
        bFiltroValido = VerificaDataPedidos(bFiltroValido, objParametros, strErros);
        if (bFiltroValido)
        {
            objParametros.dtINI = txtDataInicial.Text != "" ? Convert.ToDateTime(txtDataInicial.Text) : DateTime.Today;
            objParametros.dtFIM = txtDataFinal.Text != "" ? Convert.ToDateTime(txtDataFinal.Text) : DateTime.Today;

            //if (rdbEspecifico.Checked)
            //{
            //    bFiltroValido = txtNumeroPedido.ValorValido();
            //    if (bFiltroValido)
            //        objParametros.AddCriterio("(PEDIDO.CD_PEDIDO = '" + txtNumeroPedido.Text + "')");
            //    else
            //        strErros.Append("Número de pedido inválido!");
            //}

        }

        if (bFiltroValido)
        {
            string sNomeCliente = ""; // txtNomeCliente.Text.Trim().ToUpperInvariant().ToString();
            if (!sNomeCliente.Equals(String.Empty))
                objParametros.AddCriterio("(UPPER(PEDIDO.NM_CLIFOR) LIKE '%" +
                    sNomeCliente + "%')");
            Response.Redirect("~/ConsultaPedidos.aspx");
        }
        else
        {
            objParametros.Limpar();
            MessageHLP.ShowPopUpMsg(strErros.ToString(), this.Page);
        }
    }

    protected bool VerificaDataPedidos(bool bFiltroValido,
        ParametroPesquisa objParametros, StringBuilder strErros)
    {

        if (!bFiltroValido)
            return false;
        //bFiltroValido = ((txtDataInicial.ValorValido()) &&
        //                 (txtDataFinal.ValorValido()));
        //if (bFiltroValido)
        //{
        string dtInicial = txtDataInicial.Text != "" ? Convert.ToDateTime(txtDataInicial.Text).ToString("dd.MM.yyyy") : "";
        string dtFinal = txtDataFinal.Text != "" ? Convert.ToDateTime(txtDataFinal.Text).ToString("dd.MM.yyyy") : "";

        if (dtInicial != "" && dtFinal != "")
        {
            if (Convert.ToDateTime(dtInicial) <= Convert.ToDateTime(dtFinal))
            {
                bFiltroValido = true;
            }
            else
            {
                bFiltroValido = false;
            }
        }
        else
        {
            bFiltroValido = false;
        }


        if (bFiltroValido)
        {
            objParametros.AddCriterio("(DT_PEDIDO BETWEEN '" +
                dtInicial + "' AND '" +
                dtFinal + "')");
        }
        else
        {
            strErros.Append("A data inicial dos pedidos deve ser menor ou ");
            strErros.Append("igual à data final dos mesmos!");
        }
        //}
        //else
        //{
        //    strErros.Append("Foram definidos valores inválidos para as ");
        //    strErros.Append("datas dos pedidos!");
        //}
        return bFiltroValido;
    }

    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Home.aspx");
    }

    protected void rdbEspecifico_CheckedChanged(object sender, EventArgs e)
    {
        ConfigurarFiltroPedidoEspecifico();
    }

    private void ConfigurarFiltroPedidoEspecifico()
    {
        //bool bFiltroAtivo = rdbEspecifico.Checked;
        //txtNumeroPedido.Enabled = bFiltroAtivo;
        //if (bFiltroAtivo)
        //{
        //    txtNumeroPedido.Focus();
        //}
        //txtDataInicial.ReadOnly = !bFiltroAtivo;
        //txtDataFinal.ReadOnly = !bFiltroAtivo;
    }

    protected void rdbVariosPedidos_CheckedChanged(object sender, EventArgs e)
    {
        ConfigurarFiltroPedidoEspecifico();
    }

}
