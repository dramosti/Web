using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HLP.Web;
using HLP.Dados.Vendas.Web;
using System.Drawing;
using HLP.Dados.Vendas;
using System.Text;
using HLP.Dados;
using System.Web.Configuration;

public partial class ConsultaPedidos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sUser = UsuarioWeb.GetNomeUsuarioConectado(Session);
        if (sUser == "")
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!Page.IsPostBack)
        {
            string sTabela = WebConfigurationManager.AppSettings["TableItens"];
            if (sTabela.ToUpper() == "MOVIPEND")
            {
                gridConsultaPedidos.Columns[3].Visible = false;
            }

            BaseDAO.CancelarOperacaoObjetoDAO((BaseDAO)Session["ObjetoPedidoDetalhado"]);
            ParametroPesquisa objParametros = (ParametroPesquisa)Session["FiltroPedidos"];
            bool bParametrosValidos = (objParametros != null);
            if (bParametrosValidos)
                bParametrosValidos = (!objParametros.AindaNaoDefiniuFiltro());
            if (!bParametrosValidos)
            {
                Response.Redirect("~/PesquisarPedidos.aspx");
                return;
            }
            PesquisarDados(objParametros.GetWhere(), objParametros.GetHaving());
        }
    }

    private void PesquisarDados(string sWhere, string sHaving)
    {
        string sTabela = WebConfigurationManager.AppSettings["TableItens"];
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        DataTable dtPedidos = (DataTable)Session["DadosConsultaPedidos"];
        bool bPesquisarDados = (dtPedidos == null);
        if (bPesquisarDados)
        {
            StringBuilder str = new StringBuilder();
            str.Append("SELECT PEDIDO.CD_EMPRESA, ");
            str.Append("PEDIDO.DT_PEDIDO, ");
            str.Append("PEDIDO.CD_PEDIDO, ");
            str.Append("PEDIDO.NM_GUERRA "); //NM_CLIFOR
            str.Append("FROM PEDIDO ");
            str.Append("LEFT OUTER JOIN {0} MOVI ON (MOVI.CD_EMPRESA = PEDIDO.CD_EMPRESA) AND (MOVI.CD_PEDIDO = PEDIDO.CD_PEDIDO) ");
            str.Append("WHERE (PEDIDO.CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa + "') AND ");
            str.Append(sWhere + " ");
            str.Append("GROUP BY PEDIDO.CD_EMPRESA, PEDIDO.DT_PEDIDO, PEDIDO.CD_PEDIDO, PEDIDO.NM_GUERRA "); //NM_CLIFOR

            if (!sHaving.Equals(String.Empty))
                str.Append("HAVING " + sHaving);
            dtPedidos = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(string.Format(str.ToString(), sTabela));

            DataColumn[] ChavePrimaria = new DataColumn[] { dtPedidos.Columns["CD_PEDIDO"] };
            dtPedidos.PrimaryKey = ChavePrimaria;
            Session["DadosConsultaPedidos"] = dtPedidos;
        }
        if (dtPedidos.Rows.Count == 0)
            MessageHLP.ShowPopUpMsg("Não existem registros no período selecionado", this.Page);
        if (!Page.IsPostBack)
            ProcessaDataBind();
    }

    protected void gridConsultaPedidos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridConsultaPedidos.PageIndex = e.NewPageIndex;
        ProcessaDataBind();
    }

    private void ProcessaDataBind()
    {
        gridConsultaPedidos.DataSource = (DataTable)Session["DadosConsultaPedidos"];
        gridConsultaPedidos.DataBind();
    }

    protected void lblVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/PesquisarPedidos.aspx");
    }

    protected void btnComissao_Click(object sender, EventArgs e)
    {
        try
        {
            string sTabela = WebConfigurationManager.AppSettings["TableItens"];
            dsPedidoComissao ds = new dsPedidoComissao();
            dsPedidoComissao.PedidoRow drPedido;
            dsPedidoComissao.MovitemRow drow;
            DataTable dtItensPedido;
            UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

            StringBuilder sQuery = new StringBuilder();
            sQuery.Append("SELECT ");
            sQuery.Append("NR_LANC, ");
            sQuery.Append("CD_PEDIDO, ");
            sQuery.Append("DS_PROD ,");
            //  sQuery.Append("COALESCE(QT_PACOTES,0)QT_PACOTES, ");
            sQuery.Append("QT_PROD, ");
            sQuery.Append("VL_TOTLIQ, ");
            sQuery.Append("VL_PERCOMI1 ");
            sQuery.Append("FROM {0} WHERE CD_EMPRESA = '{1}' AND CD_PEDIDO= '{2}' ");


            if (gridConsultaPedidos.Rows.Count > 0)
            {
                foreach (GridViewRow row in gridConsultaPedidos.Rows)
                {
                    drPedido = ds.Pedido.NewPedidoRow();
                    drPedido.CD_PEDIDO = row.Cells[0].Text;// (row.Cells[0].FindControl("hlPedido") as HyperLink).Text;
                    drPedido.NM_GUERRA = (row.Cells[1].FindControl("lblNmGuerra") as Label).Text;
                    drPedido.DT_PEDIDO = Convert.ToDateTime((row.Cells[2].FindControl("lblDtPedido") as Label).Text);
                    ds.Pedido.AddPedidoRow(drPedido);

                    dtItensPedido = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(string.Format(sQuery.ToString(), sTabela, objUsuario.oTabelas.sEmpresa, row.Cells[0].Text)); //(row.Cells[0].FindControl("hlPedido") as HyperLink).Text));

                    foreach (DataRow rowItem in dtItensPedido.Rows)
                    {
                        drow = ds.Movitem.NewMovitemRow();
                        drow.CD_PEDIDO = rowItem["CD_PEDIDO"].ToString(); // (row.Cells[0].FindControl("hlPedido") as HyperLink).Text;
                        drow.NR_LANC = rowItem["NR_LANC"].ToString();
                        drow.DS_PROD = rowItem["DS_PROD"].ToString();
                        // drMovitem.QT_PACOTES = rowItem["QT_PACOTES"].ToString();
                        drow.QT_PROD = Convert.ToDecimal(rowItem["QT_PROD"].ToString());
                        drow.VL_TOTLIQ = Convert.ToDecimal(rowItem["VL_TOTLIQ"].ToString());
                        drow.VL_PERCOMI1 = Convert.ToDecimal(rowItem["VL_PERCOMI1"].ToString());
                        ds.Movitem.AddMovitemRow(drow);
                    }
                }
                Session["DataSetPedidoComissao"] = ds;
                Response.Redirect("~/ViewPedidoComissao.aspx");
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "_new", "window.open('ViewPedidoComissao.aspx');", true);


            }
            else
            {
                Session["DataSetPedidoComissao"] = null;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gridConsultaPedidos_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void gridConsultaPedidos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string sTabela = WebConfigurationManager.AppSettings["TableItens"];
        if (e.CommandName == "Pedido")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gridConsultaPedidos.Rows[index];
            string sCodigoPedido = row.Cells[0].Text;
            UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
            string sCodigoEmpresa = objUsuario.oTabelas.sEmpresa.Trim();
            string sCodigoVendedor = objUsuario.CodigoVendedor.Trim();

            string str = string.Format("select count(cd_pedido)TOTAL from {0} m where coalesce(m.cd_pedido,'0000000') = '{1}'and m.cd_empresa = '{2}'", sTabela, sCodigoPedido, sCodigoEmpresa);
            DataTable dtCountItens = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(str);
            int iCountItens = Convert.ToInt32(dtCountItens.Rows[0]["TOTAL"].ToString());

            str = string.Format("select count(m.cd_docorigem)TOTAL, nr_lanc from {0} m where coalesce(m.cd_docorigem,'0000000') = '{1}' and m.cd_empresa = '{2}' group by nr_lanc", sTabela, sCodigoPedido, sCodigoEmpresa);
            dtCountItens = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(str);
            int iCountFaturados = 0;
            string sNF = "";
            if (dtCountItens.Rows.Count > 0)
            {
                iCountFaturados = Convert.ToInt32(dtCountItens.Rows[0]["TOTAL"].ToString());
                sNF = dtCountItens.Rows[0]["nr_lanc"].ToString();
            }

            if (iCountItens > 0)
            {
                if (iCountItens == iCountFaturados)
                {
                    MessageHLP.ShowPopUpMsg("Pedido se encontra faturado! NF:" + sNF, this.Page);
                }
                else
                {
                    MessageHLP.ShowPopUpMsg("Pedido ainda não foi faturado!", this.Page);
                }
            }
        }
        else if (e.CommandName == "Email")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gridConsultaPedidos.Rows[index];
            string sCodigoPedido = row.Cells[0].Text;
            Response.Redirect("~/Informativo.aspx?CD_PEDIDO_EMAIL=" + sCodigoPedido);

        }
    }
}