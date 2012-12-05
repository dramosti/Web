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

public partial class ConsultaPedidos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sUser = UsuarioWeb.GetNomeUsuarioConectado(Session);
        if (sUser == "")
        {
            Response.Redirect("~/Home.aspx");
        }
        if (!Page.IsPostBack)
        {
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
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        DataTable dtPedidos = (DataTable)Session["DadosConsultaPedidos"];
        bool bPesquisarDados = (dtPedidos == null);

        string sDoc = "";
        string stpdocs = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("EMPRESA", "DS_TPDOCWEB", "CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa + "'");
        foreach (string item in stpdocs.Split(';'))
        {
            string[] tpdoc = item.Split(',');
            if (tpdoc.Length > 1)
            {
                sDoc = tpdoc[1].ToString().Trim();
            }
        }


        if (bPesquisarDados)
        {
            string sUserGestor = UsuarioWeb.GetNomeUsuarioGestorConectado(Session);
            string sUser = UsuarioWeb.GetNomeUsuarioConectado(Session);

            StringBuilder str = new StringBuilder();
            str.Append("SELECT P.CD_EMPRESA, ");
            str.Append("P.DT_PEDIDO, ");
            str.Append("P.CD_PEDIDO, ");
            str.Append("P.NM_GUERRA ");
            str.Append("FROM PEDIDO P ");
            str.Append("LEFT OUTER JOIN MOVITEM MOVI ON (MOVI.CD_EMPRESA = P.CD_EMPRESA) AND (MOVI.CD_PEDIDO = P.CD_PEDIDO) ");
            str.Append("WHERE (P.CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa + "') AND ");
            str.Append("(P.CD_TIPODOC = '" + sDoc + "') AND ");
            str.Append(sWhere + " ");
            str.Append("GROUP BY P.CD_EMPRESA, P.DT_PEDIDO, P.CD_PEDIDO, P.NM_GUERRA ");

            if (!sHaving.Equals(String.Empty))
                str.Append("HAVING " + sHaving);
            dtPedidos = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(
                str.ToString());

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
            dsPedidoComissao ds = new dsPedidoComissao();
            dsPedidoComissao.PedidoRow drPedido;
            dsPedidoComissao.MovitemRow drMovitem;
            DataTable dtItensPedido;
            UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

            StringBuilder sQuery = new StringBuilder();
            sQuery.Append("SELECT ");
            sQuery.Append("NR_LANC, ");
            sQuery.Append("CD_PEDIDO, ");
            sQuery.Append("DS_PROD ,");
            sQuery.Append("COALESCE(QT_PACOTES,0)QT_PACOTES, ");
            sQuery.Append("VL_TOTLIQ, ");
            sQuery.Append("VL_PERCOMI1 ");
            sQuery.Append("FROM MOVITEM WHERE CD_EMPRESA = '{0}' AND CD_PEDIDO= '{1}' ");


            if (gridConsultaPedidos.Rows.Count > 0)
            {
                foreach (GridViewRow row in gridConsultaPedidos.Rows)
                {
                    drPedido = ds.Pedido.NewPedidoRow();
                    drPedido.CD_PEDIDO = row.Cells[0].Text;// (row.Cells[0].FindControl("hlPedido") as HyperLink).Text;
                    drPedido.NM_GUERRA = (row.Cells[1].FindControl("lblNmGuerra") as Label).Text;
                    drPedido.DT_PEDIDO = Convert.ToDateTime((row.Cells[2].FindControl("lblDtPedido") as Label).Text);
                    ds.Pedido.AddPedidoRow(drPedido);

                    dtItensPedido = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(string.Format(sQuery.ToString(), objUsuario.oTabelas.sEmpresa, row.Cells[0].Text)); //(row.Cells[0].FindControl("hlPedido") as HyperLink).Text));

                    foreach (DataRow rowItem in dtItensPedido.Rows)
                    {
                        drMovitem = ds.Movitem.NewMovitemRow();
                        drMovitem.CD_PEDIDO = rowItem["CD_PEDIDO"].ToString(); // (row.Cells[0].FindControl("hlPedido") as HyperLink).Text;
                        drMovitem.NR_LANC = rowItem["NR_LANC"].ToString();
                        drMovitem.DS_PROD = rowItem["DS_PROD"].ToString();
                        drMovitem.QT_PACOTES = rowItem["QT_PACOTES"].ToString();
                        drMovitem.VL_TOTLIQ = Convert.ToDecimal(rowItem["VL_TOTLIQ"].ToString());
                        drMovitem.VL_PERCOMI1 = Convert.ToDecimal(rowItem["VL_PERCOMI1"].ToString());
                        ds.Movitem.AddMovitemRow(drMovitem);
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
        if (e.CommandName == "Pedido")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gridConsultaPedidos.Rows[index];
            string sCodigoPedido = row.Cells[0].Text;
            UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
            string sCodigoEmpresa = objUsuario.oTabelas.sEmpresa.Trim();
            string sCodigoVendedor = objUsuario.CodigoVendedor.Trim();

            string sWhere = string.Format("CD_PEDIDO ='{0}' AND CD_EMPRESA ='{1}' AND CD_VEND1 ='{2}'", sCodigoPedido, sCodigoEmpresa, sCodigoVendedor);
            string sValue = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("PEDIDO", "COALESCE(ST_PED,'N')ST_PED", sWhere);
            if (sValue == "F")
            {
                MessageHLP.ShowPopUpMsg("Pedido se encontra faturado!", this.Page);
            }
            else
            {
                MessageHLP.ShowPopUpMsg("Pedido ainda não foi faturado!", this.Page);
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