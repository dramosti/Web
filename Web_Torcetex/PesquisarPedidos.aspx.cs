using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HLP.Web;
using System.Text;
using HLP.Dados;
using System.Data;

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
            this.GetClientes();
        }

        BaseDAO.CancelarOperacaoObjetoDAO((BaseDAO)Session["ObjetoPedidoDetalhado"]);
        ParametroPesquisaCapoli.InicializarParametroPesquisa(
            "FiltroPedidos", "PEDIDO", this.Session);
        Session["DadosConsultaPedidos"] = null;

    }

    private void GetClientes()
    {
        if (cbxCliente.DataSource == null)
        {
            UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
            ParametroPesquisaCapoli.InicializarParametroPesquisa(
               "FiltroClientes", "CLIFOR", this.Session);
            ParametroPesquisa objParametros =
                (ParametroPesquisa)Session["FiltroClientes"];
            objParametros.AddCriterio("(NM_GUERRA IS NOT NULL)");
            objParametros.AddCriterio("COALESCE(ST_LIBERADO_TOTALMENTE,'A') = 'A'");

            StringBuilder str = new StringBuilder();
            str.Append("SELECT CD_CLIFOR, NM_CLIFOR FROM CLIFOR ");
            str.Append("WHERE ");
            str.Append(objParametros.GetWhere() + " ");
            str.Append("ORDER BY NM_CLIFOR ");


            DataTable dtClientes = new DataTable();
            dtClientes.Columns.Add("CD_CLIFOR", System.Type.GetType("System.String"));
            dtClientes.Columns.Add("NM_CLIFOR", System.Type.GetType("System.String"));
            DataRow rowItem;
            rowItem = dtClientes.NewRow();
            rowItem["CD_CLIFOR"] = "0";
            rowItem["NM_CLIFOR"] = "Selecione . . .";
            dtClientes.Rows.Add(rowItem);
            foreach (DataRow row in objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(
                    str.ToString()).Rows)
            {
                rowItem = dtClientes.NewRow();
                rowItem["CD_CLIFOR"] = row["CD_CLIFOR"].ToString().Trim();
                rowItem["NM_CLIFOR"] = row["NM_CLIFOR"].ToString().Trim();
                dtClientes.Rows.Add(rowItem);
            }


            cbxCliente.Items.Clear();
            cbxCliente.DataSource = dtClientes;
            cbxCliente.DataBind();
            if (dtClientes.Rows.Count > 0)
                cbxCliente.SelectedIndex = -1;
        }

        //DataTable dtPrazo = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("PRAZOS", "DS_PRAZO, CD_PRAZO", "COALESCE(ST_WEB,'N') = 'S' AND (ST_PAGAR_OU_RECEBER IN ('A', 'V'))", "DS_PRAZO");
        //if (cbxCD_PRAZO.DataSource == null)
        //{
        //    cbxCD_PRAZO.Items.Clear();
        //    cbxCD_PRAZO.DataSource = dtPrazo;
        //    cbxCD_PRAZO.DataBind();
        //}
    }

    protected void btnPesquisar_Click(object sender, EventArgs e)
    {

        //PesquisarDados(cbxCliente.SelectedValue, HlpWebtxtPedCli.Text,
        //            txtDataInicial.Text, txtDataFinal.Text, HlpWebtxtPedido.Text);
        //Response.Redirect("~/ConsultaPedidos.aspx");

        ParametroPesquisa objParametros =
           (ParametroPesquisa)Session["FiltroPedidos"];
        StringBuilder strErros = new StringBuilder();
        bool bFiltroValido = (objParametros != null);
        bFiltroValido = VerificaDataPedidos(bFiltroValido, objParametros, strErros);


        if (bFiltroValido)
        {

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
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        objParametros.AddCriterio("PEDIDO.CD_VEND1 = " + "'" + objUsuario.CodigoVendedor.ToString() + "'");

        if (cbxCliente.SelectedValue.ToString() != "0")
        {
            objParametros.AddCriterio("PEDIDO.CD_CLIENTE = " + cbxCliente.SelectedValue.ToString());
        }

        if (txtDataFinal.Text != "" && txtDataFinal.Text != "")
        {
            string DtIni = txtDataFinal.Text.Replace("/", ".");
            string dtFim = txtDataFinal.Text.Replace("/", ".");

            objParametros.AddCriterio("PEDIDO.DT_PEDIDO Between " + "'" + DtIni + "'" + " AND " + "'" + dtFim + "'");
        }
        if (HlpWebtxtPedido.Text != "")
        {
            objParametros.AddCriterio("PEDIDO.CD_PEDIDO = '" + HlpWebtxtPedido.Text + "'");
        }
        if (HlpWebtxtPedCli.Text != "")
        {
            objParametros.AddCriterio(" AND PEDIDO.DS_PEDCLI = '" + HlpWebtxtPedCli.Text + "'");
        }
        return bFiltroValido;
    }

    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Home.aspx");
    }






}
