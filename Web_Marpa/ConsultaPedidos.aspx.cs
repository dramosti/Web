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
                gridConsultaPedidos.Columns[5].Visible = false;
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

            Boolean bComissao = false;
            if (Request["comissao"] != null)
            {
                bComissao = Convert.ToBoolean(Request["comissao"]);
            }

            if (!bComissao)
            {
                PesquisaDadosPedido(objParametros);
            }
            else
            {
                PesquisarDadosComissao(objParametros.GetWhere(), objParametros.GetHaving());
            }
        }
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

    private void PesquisaDadosPedido(ParametroPesquisa objParametros)
    {
        try
        {
            btnComissao.Visible = false;
            UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
            DataTable dtPedidos = (DataTable)Session["DadosConsultaPedidos"];
            bool bPesquisarDados = (dtPedidos == null);
            if (bPesquisarDados)
            {
                StringBuilder squery = new StringBuilder();
                squery.Append("SELECT P.vl_total_reservado_com_desc VL_TOTAL_RES, P.vl_total_liberado_com_desc VL_TOTAL_LIB, ");
                squery.Append("P.dt_pedido DT_DOC, ");
                squery.Append("P.cd_empresa, ");
                squery.Append("P.cd_vend1 CD_VEND, ");
                squery.Append("P.cd_cliente CD_CLIFOR, ");
                squery.Append("p.cd_pedido, ");
                squery.Append("p.nm_clifor ");
                squery.Append("FROM pedido P left join pedseq ps on p.cd_pedido = ps.cd_pedido and p.cd_empresa = ps.cd_empresa ");
                squery.Append("where p.dt_pedido between ('{0}') and ('{1}') ");
                squery.Append("and p.cd_vend1 = '{2}' ");
                squery.Append("and p.cd_empresa = '{3}' and coalesce(ps.st_canped,'N') <> 'S' ");

                dtPedidos = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(string.Format(squery.ToString(),objParametros.dtINI.ToString("dd.MM.yyyy"), objParametros.dtFIM.ToString("dd.MM.yyyy"),objUsuario.CodigoVendedor,objUsuario.oTabelas.sEmpresa));
                DataColumn[] ChavePrimaria = new DataColumn[] { dtPedidos.Columns["CD_PEDIDO"] };
                dtPedidos.PrimaryKey = ChavePrimaria;
                Session["DadosConsultaPedidos"] = dtPedidos;
            }
            if (dtPedidos.Rows.Count == 0)
                MessageHLP.ShowPopUpMsg("Não existem registros no período selecionado", this.Page);
            if (!Page.IsPostBack)
                ProcessaDataBind();

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void PesquisarDadosComissao(string sWhere, string sHaving)
    {
        btnComissao.Visible = true;
        string sTabela = WebConfigurationManager.AppSettings["TableItens"];
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        DataTable dtPedidos = (DataTable)Session["DadosConsultaPedidos"];
        ParametroPesquisa objParametros = (ParametroPesquisa)Session["FiltroPedidos"];
        bool bPesquisarDados = (dtPedidos == null);
        if (bPesquisarDados)
        {
            StringBuilder squery = new StringBuilder();
            squery.Append("SELECT '0,00'VL_TOTAL_RES, SUM(S.VL_TOTAL) VL_TOTAL_LIB, SUM(S.VL_TOTLIQ) VL_TOTPROD, ");
            squery.Append("SUM(VL_COMISSAO) VL_COMISSAO, ");
            squery.Append("CASE S.CD_TIPODOC ");
            squery.Append("WHEN '006' THEN  10 ");
            squery.Append("ELSE ");
            squery.Append("CAST((CAST(SUM(S.VL_COMISSAO) AS NUMERIC(18, 4)) / ");
            squery.Append("CAST(SUM(S.VL_TOTLIQ) AS NUMERIC(18, 4))) AS NUMERIC(18, 4)) * 100 ");
            squery.Append("END AS VL_PERCOMISSAO , S.DT_DOC, ");
            squery.Append("S.CD_EMPRESA, S.CD_VEND, S.CD_CLIFOR, S.DS_TIPODOC, S.CD_PEDIDO, ");
            squery.Append("C.NM_CLIFOR,  V.NM_VEND ");
            squery.Append("FROM SP_COMISSAO_MARPA ('{0}', '{1}', ");
            squery.Append("'{2}', '{2}', ");
            squery.Append("'{3}','{3}') S ");
            squery.Append("LEFT JOIN CLIFOR C ON (C.CD_CLIFOR = S.CD_CLIFOR) ");
            squery.Append("LEFT JOIN VENDEDOR V ON (V.CD_VEND = S.CD_VEND) ");
            squery.Append("LEFT JOIN EMPRESA E ON (E.CD_EMPRESA=S.CD_EMPRESA) ");
            //squery.Append("WHERE (P.CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa + "') AND ");
            //squery.Append(sWhere + " ");
            squery.Append("GROUP BY S.CD_EMPRESA,S.CD_VEND, S.DT_DOC, S.CD_CLIFOR, S.CD_PEDIDO, C.NM_CLIFOR, V.NM_VEND, S.CD_TIPODOC, S.DS_TIPODOC ");
            squery.Append("ORDER BY S.CD_EMPRESA,S.CD_VEND, S.DT_DOC, S.CD_CLIFOR, S.CD_PEDIDO ");

            if (!sHaving.Equals(String.Empty))
                squery.Append("HAVING " + sHaving);
            dtPedidos = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(
              string.Format(squery.ToString(), objParametros.dtINI.ToString("dd.MM.yyyy"), objParametros.dtFIM.ToString("dd.MM.yyyy"), objUsuario.CodigoVendedor, objUsuario.oTabelas.sEmpresa));

            DataColumn[] ChavePrimaria = new DataColumn[] { dtPedidos.Columns["CD_PEDIDO"] };
            dtPedidos.PrimaryKey = ChavePrimaria;
            Session["DadosConsultaPedidos"] = dtPedidos;
        }
        if (dtPedidos.Rows.Count == 0)
            MessageHLP.ShowPopUpMsg("Não existem registros no período selecionado", this.Page);
        if (!Page.IsPostBack)
            ProcessaDataBind();
    }

    protected void btnComissao_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["DadosConsultaPedidos"] != null)
            {
                DataTable dt = Session["DadosConsultaPedidos"] as DataTable;

                List<belComissao> objListComissao = dt.AsEnumerable().Select(item => new belComissao()
                {
                    CD_CLIFOR = item["CD_CLIFOR"].ToString(),
                    CD_EMPRESA = item["CD_EMPRESA"].ToString(),
                    CD_PEDIDO = item["CD_PEDIDO"].ToString(),
                    DS_TIPODOC = item["DS_TIPODOC"].ToString(),
                    DT_DOC = Convert.ToDateTime(item["DT_DOC"].ToString()),
                    NM_CLIFOR = item["NM_CLIFOR"].ToString(),
                    NM_VEND = item["NM_VEND"].ToString(),
                    VL_COMISSAO = Convert.ToDecimal(item["VL_COMISSAO"].ToString()),
                    VL_PERCOMISSAO = Convert.ToDecimal(item["VL_PERCOMISSAO"].ToString()),
                    VL_TOTAL = Convert.ToDecimal(item["VL_TOTAL"].ToString()),
                    VL_TOTPROD = Convert.ToDecimal(item["VL_TOTPROD"].ToString())
                }).ToList();


                Session["DataSetPedidoComissao"] = objListComissao;
                Response.Redirect("~/ViewPedidoComissao.aspx");
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "_new", "window.open('ViewPedidoComissao.aspx');", true);
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
            int iCountFaturados = 0;
            int iCountItens = 0;
            string sNF = "";
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gridConsultaPedidos.Rows[index];
            string sCodigoPedido = row.Cells[0].Text;
            UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
            string sCodigoEmpresa = objUsuario.oTabelas.sEmpresa.Trim();
            string sCodigoVendedor = objUsuario.CodigoVendedor.Trim();
            if (sTabela == "MOVITEM")
            {
                string str = string.Format("select count(cd_pedido)TOTAL from {0} m where coalesce(m.cd_pedido,'0000000') = '{1}'and m.cd_empresa = '{2}'", sTabela, sCodigoPedido, sCodigoEmpresa);
                DataTable dtCountItens = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(str);
                iCountItens = Convert.ToInt32(dtCountItens.Rows[0]["TOTAL"].ToString());

                str = string.Format("select count(m.cd_docorigem)TOTAL, nr_lanc from {0} m where coalesce(m.cd_docorigem,'0000000') = '{1}' and m.cd_empresa = '{2}' group by nr_lanc", sTabela, sCodigoPedido, sCodigoEmpresa);
                dtCountItens = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(str);


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
            else
            {
                string str = string.Format("select count(cd_pedido)TOTAL from MOVIPEND m where coalesce(m.cd_pedido,'0000000') = '{1}'and m.cd_empresa = '{2}'", sTabela, sCodigoPedido, sCodigoEmpresa);
                DataTable dtCountItens = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(str);
                iCountItens = Convert.ToInt32(dtCountItens.Rows[0]["TOTAL"].ToString());

                str = string.Format("select count(cd_pedido)TOTAL from MOVITEM m where coalesce(m.cd_pedido,'0000000') = '{1}'and m.cd_empresa = '{2}' ", sTabela, sCodigoPedido, sCodigoEmpresa);
                dtCountItens = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(str);


                if (dtCountItens.Rows.Count > 0)
                {
                    iCountFaturados = Convert.ToInt32(dtCountItens.Rows[0]["TOTAL"].ToString());
                }

                if (iCountItens == 0)
                {
                    //faturado
                    MessageHLP.ShowPopUpMsg("Pedido se encontra faturado!", this.Page);
                }
                else if (iCountFaturados == 0)
                {
                    // não faturado
                    MessageHLP.ShowPopUpMsg("Pedido ainda não foi faturado!", this.Page);
                }
                else if (iCountItens > 0 && iCountFaturados > 0)
                {
                    // parcial                    
                    MessageHLP.ShowPopUpMsg("Pedido parcialmente faturado! ", this.Page);
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