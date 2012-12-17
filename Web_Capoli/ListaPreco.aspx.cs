using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using HLP.Web;

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
            GridViewDb.DataSource = GetProdutoGrid();
            GridViewDb.DataBind();
        }
    }


    protected void cbxListaPreco_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnLoc_Click(sender, e);
    }

    protected void btnLoc_Click(object sender, EventArgs e)
    {
        GridViewDb.DataSource = GetProdutoGrid();
        GridViewDb.DataBind();
    }

    private DataTable GetProdutoGrid()
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        // string ListaPreco = WebConfigurationManager.AppSettings["ListaDefault"];

        //Busca a Lista de Preço Padrão do cadastro de Cliente
        string ListaPreco = cbxListaPreco.SelectedValue.ToString();
        ListaPreco = ListaPreco == "" ? "00001" : ListaPreco;

        //if (ListaPreco == String.Empty)
        //    ListaPreco = "00001";
        //if (ListaPreco == null)
        //    ListaPreco = "00001";

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
            strWhere.Append("AND (PRODUTO.CD_ALTER STARTING'" + txtCodProd.Text.ToUpper().Trim().PadLeft(7, '0') + "') ");
        if (!txtProdDesc.Text.Equals(String.Empty))
            strWhere.Append("AND (PRODUTO.DS_PROD LIKE ('%" + txtProdDesc.Text.ToUpper().Trim() + "%'))");
        //DataTable dtProduto = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("PRODUTO", "CD_ALTER, CD_PROD, DS_PROD", strWhere.ToString(), "DS_PROD");
        strWhere.Append(" AND (PRODUTO.QT_ESTOQUE > 0)");

        DataTable dtProduto = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("PRECOS INNER JOIN PRODUTO ON (PRODUTO.CD_PROD = PRECOS.CD_PROD)", (st_atualizacao.Equals("M") ? "PRECOS.VL_PRECOVE" : "(PRECOS.VL_PRECOVE * " + vl_perc.ToString().Replace(',', '.') + ")VL_PRECOVE") + ", PRODUTO.CD_ALTER, PRODUTO.CD_PROD, PRODUTO.DS_DETALHE, PRODUTO.VL_PESOBRU, PRODUTO.QT_ESTOQUE", strWhere.ToString(), "PRODUTO.DS_PROD");


        DataTable dtRetorno = new DataTable("TabelaItens");
        dtRetorno.Columns.Add("CD_ALTER").DataType = System.Type.GetType("System.String");
        dtRetorno.Columns.Add("DS_PROD").DataType = System.Type.GetType("System.String");
        dtRetorno.Columns.Add("VL_PRECOVE").DataType = System.Type.GetType("System.Double");
        dtRetorno.Columns.Add("CD_PROD").DataType = System.Type.GetType("System.String");
        dtRetorno.Columns.Add("VL_PESOBRU").DataType = System.Type.GetType("System.Double");
        dtRetorno.Columns.Add("QT_ESTOQUE").DataType = System.Type.GetType("System.Double");

        DataRow drRet;

        foreach (DataRow dr in dtProduto.Rows)
        {
            drRet = dtRetorno.NewRow();
            drRet["CD_ALTER"] = dr["CD_ALTER"]; // dr["CD_ALTER"]; cliente não utiliza o CD_ALTER
            drRet["DS_PROD"] = dr["DS_DETALHE"];
            drRet["VL_PRECOVE"] = dr["VL_PRECOVE"];
            drRet["CD_PROD"] = dr["CD_PROD"];
            drRet["VL_PESOBRU"] = dr["VL_PESOBRU"];
            drRet["QT_ESTOQUE"] = dr["QT_ESTOQUE"];

            dtRetorno.Rows.Add(drRet);
        }

        Session["DadosConsultaProduto"] = dtRetorno;
        return dtRetorno;
    }
    private DataTable GetListaPrecos()
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

        DataTable dtListas = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("listapre", "CD_LISTA, DS_LISTA", "CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa + "'");

        return dtListas;
    }

    protected void GridViewDb_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridViewDb.PageIndex = e.NewPageIndex;
        GridViewDb.DataSource = (DataTable)Session["DadosConsultaProduto"];
        GridViewDb.DataBind();
    }
}