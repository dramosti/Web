using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HLP.Dados;
using HLP.Web.Controles;
using System.Text;
using HLP.Web;

public partial class PesquisarClientes : System.Web.UI.Page
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

            BaseDAO.CancelarOperacaoObjetoDAO((BaseDAO)Session["ObjetoClienteDetalhado"]);
            ParametroPesquisaCapoli.InicializarParametroPesquisa(
                "FiltroClientes", "CLIFOR", this.Session);
            Session["DadosConsultaClientes"] = null;

            if (Request["CD_CLIFOR"] != null)
            {
                MultViewPesquisaCliente.ActiveViewIndex = 1;
                UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
                lblMsg.Text = "Cliente Salvo com sucesso!";
                lblCodigo.Text = Request["CD_CLIFOR"].ToString();
                lblNmCliente.Text = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR", "nm_clifor", "CD_CLIFOR='" + Request["CD_CLIFOR"].ToString() + "'");
                Session["NM_CLIFOR"] = "";
                Session["CD_CLIFOR"] = null;
            }
        }

    }

    protected void btnPesquisar_Click(object sender, EventArgs e)
    {

        BaseDAO.CancelarOperacaoObjetoDAO((BaseDAO)Session["ObjetoClienteDetalhado"]);
        ParametroPesquisaCapoli.InicializarParametroPesquisa(
            "FiltroClientes", "CLIFOR", this.Session);
        Session["DadosConsultaClientes"] = null;

        ParametroPesquisa objParametros =
            (ParametroPesquisa)Session["FiltroClientes"];

        StringBuilder strErros = new StringBuilder();

        bool bFiltroValido = (objParametros != null);
        if (bFiltroValido)
        {
            if (rdbCodigo.Checked)
            {
                bFiltroValido = (!txtCodigo.EmBranco());
                if (bFiltroValido)
                    objParametros.AddCriterio("(CD_CLIFOR ='" +
                        txtCodigo.GetValor().ToString().PadLeft(7, '0') + "')");
                else
                    strErros.Append("Código inválido!!!");
            }
            else if (rdbRazao.Checked)
            {
                bFiltroValido = (!txtRazaoSocial.EmBranco());
                if (bFiltroValido)
                    objParametros.AddCriterio("(UPPER(NM_CLIFOR) LIKE '%" +
                        txtRazaoSocial.GetValor() + "%')");
                else
                    strErros.Append("Razão Social inválida!!!");
            }
            else if (rdbGuerra.Checked)
            {
                bFiltroValido = (!txtNomeCliente.EmBranco());
                if (bFiltroValido)
                    objParametros.AddCriterio("(UPPER(NM_GUERRA) LIKE '%" +
                        txtNomeCliente.GetValor() + "%')");
                else
                    strErros.Append("Nome de Guerra inválido!!!");
            }
            else if (rdbCidade.Checked)
            {
                bFiltroValido = (!txtCidade.EmBranco());
                if (bFiltroValido)
                    objParametros.AddCriterio("(UPPER(NM_CIDNOR) LIKE '%" +
                        txtCidade.GetValor() + "%')");
                else
                    strErros.Append("Cidade inválida!!!");
            }
            else if (rdbTodos.Checked)
            {
                objParametros.AddCriterio("(NM_GUERRA IS NOT NULL)");
            }
        }
        if (bFiltroValido)

            Response.Redirect("~/ConsultaClientes.aspx");
        else
        {
            objParametros.Limpar();
            MessageHLP.ShowPopUpMsg(strErros.ToString(), this.Page);
        }
    }

    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Home.aspx");
    }

    protected void rdbCodigo_CheckedChanged(object sender, EventArgs e)
    {
        HabilitaFiltro(txtCodigo);
        DesabilitaFiltro(txtRazaoSocial);
        DesabilitaFiltro(txtNomeCliente);
        DesabilitaFiltro(txtCidade);
    }

    protected void rdbRazao_CheckedChanged(object sender, EventArgs e)
    {
        HabilitaFiltro(txtRazaoSocial);
        DesabilitaFiltro(txtCodigo);
        DesabilitaFiltro(txtNomeCliente);
        DesabilitaFiltro(txtCidade);
    }

    protected void rdbGuerra_CheckedChanged(object sender, EventArgs e)
    {
        HabilitaFiltro(txtNomeCliente);
        DesabilitaFiltro(txtCodigo);
        DesabilitaFiltro(txtRazaoSocial);
        DesabilitaFiltro(txtCidade);
    }

    protected void rdbCidade_CheckedChanged(object sender, EventArgs e)
    {
        HabilitaFiltro(txtCidade);
        DesabilitaFiltro(txtCodigo);
        DesabilitaFiltro(txtRazaoSocial);
        DesabilitaFiltro(txtNomeCliente);
    }

    private void HabilitaFiltro(HlpWebTextBox txtFiltro)
    {
        txtFiltro.Enabled = true;
        txtFiltro.Focus();
    }

    private void DesabilitaFiltro(HlpWebTextBox txtFiltro)
    {
        txtFiltro.Text = String.Empty;
        txtFiltro.Enabled = false;
    }

    protected void rdbTodos_CheckedChanged(object sender, EventArgs e)
    {
        DesabilitaFiltro(txtCodigo);
        DesabilitaFiltro(txtRazaoSocial);
        DesabilitaFiltro(txtNomeCliente);
    }
    protected void btnPesquisarClientes_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/PesquisarClientes.aspx");
    }
}