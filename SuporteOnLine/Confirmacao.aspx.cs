using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using FirebirdSql.Data.FirebirdClient;

public partial class Confirmacao : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["ID"].ToString() != String.Empty)
            {
                
                if (Request.QueryString["ID"].ToString().Substring(8, 1).Equals("I"))
                {
                    btnInicial.Text = "Tela Inicial";
                    btnIncluir.Text = "Incluir Novo Cadastro";
                    lblTitulo.Text = "Inclusão Realizada com Sucesso!";
                    lblLancamento.Text = "Novo Cadastro de Visita Incluso: " + Request.QueryString["ID"].ToString().Substring(0, 7) + ".";
                }
                else if (Request.QueryString["ID"].ToString().Substring(8, 1).Equals("X"))
                {
                    lblTitulo.Text = "Inclusão Não foi Realizada com Sucesso!";
                    lblLancamento.Text = "Clique no botão Incluir Novo Cadastro.";
                }
                else if (Request.QueryString["ID"].ToString().Substring(8, 1).Equals("E"))
                {
                    btnIncluir.Text = "Excluir";
                    btnInicial.Text = "Cancelar";
                    lblTitulo.Text = "Tem certeza que gostaria de excluir este Cadastro?";
                    lblLancamento.Text = "Cadastro de Visitas a ser excluído: " + Request.QueryString["ID"].ToString().Substring(0, 7) + ".";
                }
                else if (Request.QueryString["ID"].ToString().Substring(8, 1).Equals("N"))
                {
                    btnIncluir.Enabled = false;
                    btnInicial.Text = "Tela Inicial";
                    lblTitulo.Text = "Tem certeza que gostaria de excluir este Cadastro?";
                    lblLancamento.Text = "Cadastro de Visitas a ser excluído: " + Request.QueryString["ID"].ToString().Substring(0, 7) + ".";
                }
            }
        }
    }
    protected void btnIncluir_Click(object sender, EventArgs e)
    {
        if (!btnIncluir.Text.Equals("Excluir"))
            Response.Redirect("~/CadastroRelatorioVisita.aspx?IdRel=incluir");
        else
        {
            StringBuilder str = new StringBuilder();

            str.Append("DELETE FROM LANCREL ");
            str.Append("WHERE (NR_LANC = '" + Request.QueryString["ID"].ToString().Substring(0,7) + "')");

            FbCommand cmdDelete = (FbCommand)HlpBancoDados.CommandSelect(str.ToString());
            cmdDelete.Connection.Open();

            int iSucesso = cmdDelete.ExecuteNonQuery();

            cmdDelete.Connection.Close();

            if (iSucesso > 0)
                Response.Redirect("~/Default.aspx");
            else
                Response.Redirect("~/Confirmacao.aspx?ID=" + Request.QueryString["ID"].ToString().Substring(0,7) + ",N");

        }
    }
    protected void btnInicial_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }
    protected void btnImp_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/VisualizarRelatorioVisita.aspx?ID=" + Request.QueryString["ID"].ToString().Substring(0, 7));
    }
}
