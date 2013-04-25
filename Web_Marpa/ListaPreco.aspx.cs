using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HLP.Web;
using System.Data;
using System.Text;

public partial class ListaPreco : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string sUser = UsuarioWeb.GetNomeUsuarioConectado(Session);
            if (sUser == "")
            {
                Response.Redirect("~/Login.aspx");
            }
            UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

            cbxListaPreco.DataTextField = "DS_LISTA";
            cbxListaPreco.DataValueField = "CD_LISTA";
            cbxListaPreco.DataSource = GetListaPrecos();
            cbxListaPreco.DataBind();
            cbxListaPreco.SelectedIndex = 0;

            cbxLinhaProduto.DataSource = GetLinhaProduto();
            cbxLinhaProduto.DataBind();

            GridViewDb.DataSource = GetProdutoGrid();
            GridViewDb.DataBind();
        }
    }


    protected void cbxListaPreco_SelectedIndexChanged(object sender, EventArgs e)
    {
        dvAviso.DataSource = null;
        dvAviso.DataBind();

        btnLoc_Click(sender, e);
    }

    protected void btnLoc_Click(object sender, EventArgs e)
    {
        dvAviso.DataSource = null;
        dvAviso.DataBind();

        GridViewDb.DataSource = GetProdutoGrid();
        GridViewDb.DataBind();
    }

    private DataTable GetProdutoGrid()
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        string sCodProduto = txtCodProd.Text.ToUpper().Trim().PadLeft(7, '0');
        //Busca a Lista de Preço Padrão do cadastro de Cliente
        string ListaPreco = cbxListaPreco.SelectedValue.ToString();
        ListaPreco = ListaPreco == "" ? "00001" : ListaPreco;

        /// M-manual / A-automatico
        string st_atualizacao = string.Empty;
        decimal vl_perc = 0;

        DataTable dtListaPreco = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("LISTAPRE", "ST_ATUALIZACAO, VL_PERC", "CD_LISTA = '" + ListaPreco + "'");

        foreach (DataRow dr in dtListaPreco.Rows)
        {
            st_atualizacao = dr["ST_ATUALIZACAO"].ToString().Trim();
            vl_perc = (Convert.ToDecimal(dr["VL_PERC"]) / 100) + 1;
            break;
        }


        StringBuilder strWhere = new StringBuilder();
        strWhere.Append("(PRECOS.CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa.ToString().Trim() + "') AND ((PRODUTO.ST_INATIVO IS NULL) OR ");
        strWhere.Append("(PRODUTO.ST_INATIVO <> 'S')) AND ");
        strWhere.Append("((PRECOS.CD_LISTA = '" + (st_atualizacao.Equals("M") ? ListaPreco.Trim() : "00001") + "') AND (PRECOS.VL_PRECOVE > 0)) ");
        if (!txtCodProd.Text.Equals(String.Empty))
            strWhere.Append("AND (PRODUTO.CD_PROD STARTING'" + sCodProduto + "') ");
        if (!txtProdDesc.Text.Equals(String.Empty))
            strWhere.Append("AND (PRODUTO.DS_PROD LIKE ('%" + txtProdDesc.Text.ToUpper().Trim() + "%')) ");
        strWhere.Append(" AND PRODUTO.CD_LINHA = '" + cbxLinhaProduto.SelectedValue.ToString() + "' "); // OS_27292

        DataTable dtProduto = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("PRECOS INNER JOIN PRODUTO ON (PRODUTO.CD_PROD = PRECOS.CD_PROD)" +
            " INNER JOIN CLAS_FIS ON PRODUTO.CD_CF = CLAS_FIS.CD_CF ",
            (st_atualizacao.Equals("M") ? "PRECOS.VL_PRECOVE" : "(PRECOS.VL_PRECOVE * " + vl_perc.ToString().Replace(',', '.') + ")VL_PRECOVE") +
       ", PRODUTO.CD_BARRAS, PRODUTO.CD_PROD, PRODUTO.DS_DETALHE, PRODUTO.VL_PESOBRU, PRODUTO.QT_ESTOQUE, PRODUTO.CD_SITTRIB, coalesce(precos.vl_precove_subst,0)vl_precove_subst, coalesce(precos.vl_subst,0)vl_subst, " +
       "CLAS_FIS.DS_CLASFIS, CLAS_FIS.VL_ALIIPI, CLAS_FIS.VL_REDBASE", strWhere.ToString(), "PRODUTO.DS_DETALHE");


        DataTable dtRetorno = new DataTable("TabelaItens");
        dtRetorno.Columns.Add("CD_PROD").DataType = System.Type.GetType("System.String");
        dtRetorno.Columns.Add("CD_BARRAS").DataType = System.Type.GetType("System.String");
        dtRetorno.Columns.Add("DS_PROD").DataType = System.Type.GetType("System.String");
        dtRetorno.Columns.Add("VL_PRECOVE").DataType = System.Type.GetType("System.Double");
        dtRetorno.Columns.Add("VL_PESOBRU").DataType = System.Type.GetType("System.Double");
        dtRetorno.Columns.Add("QT_ESTOQUE").DataType = System.Type.GetType("System.Double");
        dtRetorno.Columns.Add("DS_CLASFIS").DataType = System.Type.GetType("System.String");
        dtRetorno.Columns.Add("VL_ALIIPI").DataType = System.Type.GetType("System.Double");
        dtRetorno.Columns.Add("VL_REDBASE").DataType = System.Type.GetType("System.Double");
        dtRetorno.Columns.Add("VL_ALIQUOT").DataType = System.Type.GetType("System.Double");
        dtRetorno.Columns.Add("VL_ALISUBS").DataType = System.Type.GetType("System.Double");
        dtRetorno.Columns.Add("CD_SITTRIB").DataType = System.Type.GetType("System.String");
        dtRetorno.Columns.Add("vl_precove_subst").DataType = System.Type.GetType("System.Double");
        dtRetorno.Columns.Add("vl_subst").DataType = System.Type.GetType("System.Double");


        DataRow drRet;

        foreach (DataRow dr in dtProduto.Rows)
        {
            drRet = dtRetorno.NewRow();
            drRet["CD_PROD"] = dr["CD_PROD"];
            drRet["CD_BARRAS"] = dr["CD_BARRAS"];
            drRet["DS_PROD"] = dr["DS_DETALHE"];
            drRet["VL_PRECOVE"] = Convert.ToDecimal(dr["VL_PRECOVE"]).ToString("#0,00");
            drRet["VL_PESOBRU"] = dr["VL_PESOBRU"];
            drRet["QT_ESTOQUE"] = dr["QT_ESTOQUE"];
            drRet["DS_CLASFIS"] = dr["DS_CLASFIS"];
            drRet["VL_ALIIPI"] = dr["VL_ALIIPI"];
            drRet["VL_REDBASE"] = (dr["VL_REDBASE"].ToString().Equals("0") ? 0 : 100 - Convert.ToDouble(dr["VL_REDBASE"]));
            drRet["CD_SITTRIB"] = dr["CD_SITTRIB"];
            drRet["vl_precove_subst"] = dr["vl_precove_subst"];
            drRet["vl_subst"] = dr["vl_subst"];

            DataTable dtAliquotas = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("ICM", "VL_ALIQUOT, VL_ALISUBS ", "cd_ufnor = '" + Session["CD_UFNOR"] + "' and icm.cd_aliicms =  (SELECT  PRODUTO.cd_aliicms FROM PRODUTO  WHERE CD_PROD = '" + sCodProduto + "')");
            if (dtAliquotas.Rows.Count > 0)
            {
                drRet["VL_ALIQUOT"] = Convert.ToDouble(dtAliquotas.Rows[0]["VL_ALIQUOT"].ToString());
                drRet["VL_ALISUBS"] = Convert.ToDouble(dtAliquotas.Rows[0]["VL_ALISUBS"].ToString());
            }
            else
            {
                drRet["VL_ALIQUOT"] = 0;
                drRet["VL_ALISUBS"] = 0;
            }
            dtRetorno.Rows.Add(drRet);
        }
        Session["DadosConsultaProduto"] = dtRetorno;
        return dtRetorno;
    }
    private DataTable GetListaPrecos()
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        DataTable dtListas = null;

        string sListaPermit = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("ACESSO", "cd_listapermitida", "cd_vend = '" + objUsuario.oTabelas.CdVendedorAtual + "'");

        if (sListaPermit != "")
        {
            dtListas = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("listapre", "CD_LISTA, DS_LISTA", "CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa + "' AND cd_lista in (" + sListaPermit + ")");
        }
        else
        {
            dtListas = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("listapre", "CD_LISTA, DS_LISTA", "CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa + "'");
        }

        return dtListas;
    }
    private DataTable GetLinhaProduto()
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

        DataTable dtLinhaProd = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("linhapro", "cd_linha, ds_linha", "CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa + "' and st_linha = 'A' and coalesce(st_web,'S') = 'S' order by ds_linha ");
        return dtLinhaProd;
    }

    protected void GridViewDb_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dvAviso.DataSource = null;
        dvAviso.DataBind();

        ((DataTable)Session["DadosConsultaProduto"]).DefaultView.RowFilter = null;

        GridViewDb.PageIndex = e.NewPageIndex;
        GridViewDb.DataSource = (DataTable)Session["DadosConsultaProduto"];
        GridViewDb.DataBind();
    }
    protected void GridViewDb_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["sCodProd"] != null)
        {
            if (Session["sCodProd"].ToString().Equals(GridViewDb.SelectedDataKey[0].ToString()) && dvAviso.Rows.Count > 0)
            {
                dvAviso.DataSource = null;
                dvAviso.DataBind();
            }
            else
            {
                MostraDetalhesProduto();
            }
        }
        else
        {
            MostraDetalhesProduto();
        }
    }

    private void MostraDetalhesProduto()
    {
        Session["sCodProd"] = GridViewDb.SelectedDataKey[0].ToString();

        ((DataTable)Session["DadosConsultaProduto"]).DefaultView.RowFilter = "CD_PROD ='" + Session["sCodProd"] + "'";
        dvAviso.DataSource = ((DataTable)Session["DadosConsultaProduto"]).DefaultView;
        dvAviso.DataBind();
    }
}