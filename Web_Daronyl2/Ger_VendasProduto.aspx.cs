using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using HLP.Web;
using HLP.Dados.Vendas;
using System.Web.Configuration;

public partial class Ger_VendasProduto : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string sUser = UsuarioWeb.GetNomeUsuarioGestorConectado(Session);
            if (sUser == "")
            {
                Response.Redirect("~/Login.aspx");
            }
            CarregaAnos();
        }
    }

    protected void CarregaAnos()
    {
        int iAnoAtual = DateTime.Now.Year;
        cbxAno.Items.Add(iAnoAtual.ToString());

        for (int i = 0; i < 20; i++)
        {
            iAnoAtual--;
            cbxAno.Items.Add(iAnoAtual.ToString());
        }
        cbxAno.DataBind();
    }



    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        GridViewDb.DataSource = GetProdutoGrid();
        GridViewDb.DataBind();
    }

    protected void GridViewDb_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridViewDb.PageIndex = e.NewPageIndex;
        GridViewDb.DataSource = (DataTable)Session["DadosConsultaProduto"];
        GridViewDb.DataBind();
    }
    protected void GridViewDb_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "Gerar")
        {


            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GridViewDb.Rows[index];
            string sCD_PROD = Server.HtmlDecode(row.Cells[2].Text);
            string sDesc = Server.HtmlDecode(row.Cells[1].Text);

            UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

            string sTabela = WebConfigurationManager.AppSettings["TableItens"];

            DataTable dtResult = HlpFuncoesVendas.GetVendasProduto(objUsuario.oTabelas, sTabela, sCD_PROD, cbxAno.SelectedItem.Text);

            Chart1.Titles[0].Text = "Qtde Vendida do Produto " + sDesc;
            Chart1.DataSource = dtResult;
            Chart1.DataBind();

            MultViewVendasPorRepres.ActiveViewIndex = 1;
        }

    }

    private DataTable GetProdutoGrid()
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

        StringBuilder strWhere = new StringBuilder();

        if (!txtCodProd.Text.Equals(String.Empty))
        {
            strWhere.Append("(PRODUTO.CD_ALTER STARTING'" + txtCodProd.Text.ToUpper().Trim() + "') ");
        }
        if (!txtProdDesc.Text.Equals(String.Empty))
        {
            if (!txtCodProd.Text.Equals(String.Empty))
            {
                strWhere.Append("AND (PRODUTO.DS_PROD STARTING '" + txtProdDesc.Text.ToUpper().Trim() + "') ");
            }
            else
            {
                strWhere.Append("(PRODUTO.DS_PROD STARTING '" + txtProdDesc.Text.ToUpper().Trim() + "') ");
            }
        }

        DataTable dtProduto = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("PRODUTO", "PRODUTO.CD_ALTER, PRODUTO.DS_PROD, PRODUTO.CD_PROD", strWhere.ToString(), "PRODUTO.CD_PROD");


        DataTable dtRetorno = new DataTable("TabelaItens");
        dtRetorno.Columns.Add("CD_ALTER").DataType = System.Type.GetType("System.String");
        dtRetorno.Columns.Add("DS_PROD").DataType = System.Type.GetType("System.String");
        dtRetorno.Columns.Add("CD_PROD").DataType = System.Type.GetType("System.String");

        DataRow drRet;

        foreach (DataRow dr in dtProduto.Rows)
        {
            drRet = dtRetorno.NewRow();
            drRet["CD_ALTER"] = dr["CD_ALTER"];
            drRet["DS_PROD"] = dr["DS_PROD"];
            drRet["CD_PROD"] = dr["CD_PROD"];

            dtRetorno.Rows.Add(drRet);
        }

        Session["DadosConsultaProduto"] = dtRetorno;
        return dtRetorno;
    }
    protected void btnNovaPesquisa_Click(object sender, EventArgs e)
    {
        MultViewVendasPorRepres.ActiveViewIndex = 0;
    }
}