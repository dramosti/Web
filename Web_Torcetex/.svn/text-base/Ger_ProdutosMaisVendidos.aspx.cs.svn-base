using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HLP.Web;
using System.Web.Configuration;
using System.Data;
using HLP.Dados.Vendas;

public partial class Ger_ProdutosMaisVendidos : System.Web.UI.Page
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

        for (int i = 2; i < 11; i++)
        {
            cbxQtde.Items.Add(i.ToString());
        }
        cbxQtde.DataBind();
    }



    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

        if (txtDataInicial.Text.ToString() != "" && txtDataFinal.Text.ToString() != "")
        {
            string sTableItens = (WebConfigurationManager.AppSettings["TableItens"]).ToUpper();

            DataTable dtResult = HlpFuncoesVendas.GetProdutosMaisVendidos(objUsuario.oTabelas, sTableItens, Convert.ToUInt16(cbxQtde.SelectedItem.Text), Convert.ToDateTime(txtDataInicial.Text), Convert.ToDateTime(txtDataFinal.Text));
            grafico.DataSource = dtResult;
            grafico.DataBind();
            MultViewVendasPorRepres.ActiveViewIndex = 1;

            grafico.Titles[0].Text = string.Format("Top {0} produtos mais vendidos no perido de {1} a {2}",
                                                            cbxQtde.SelectedItem.Text,
                                                            txtDataInicial.Text,
                                                            txtDataFinal.Text);
        }


    }
    protected void btnNovaPesquisa_Click(object sender, EventArgs e)
    {
        MultViewVendasPorRepres.ActiveViewIndex = 0;
    }
}