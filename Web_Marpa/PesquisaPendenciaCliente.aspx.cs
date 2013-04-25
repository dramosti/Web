using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HLP.Web;
using HLP.Web.Controles;
using System.Text;
using System.Data;
using HLP.Dados;
using HLP.Dados.Cadastro;
using HLP.Dados.Cadastro.Web;

public partial class PesquisaPendenciaCliente : System.Web.UI.Page
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
            if (Session["CD_ALTER"] != null)
            {
                txtCodigo.Text = Session["CD_ALTER"].ToString();
                EventoPesquisar();
            }
            if (Session["NM_CLIFOR"] != null)
            {
                txtCliente.Text = Session["NM_CLIFOR"].ToString();
            }
        }
    }


    protected void rdbTodos_CheckedChanged(object sender, EventArgs e)
    {
        DesabilitaFiltro(txtCodigo);
        DesabilitaFiltro(txtNomeCliente);
        txtCliente.Text = "";
    }
    protected void rdbGuerra_CheckedChanged(object sender, EventArgs e)
    {
        HabilitaFiltro(txtNomeCliente);
        DesabilitaFiltro(txtCodigo);
        txtCliente.Text = "";
    }
    protected void rdbCodigo_CheckedChanged(object sender, EventArgs e)
    {
        HabilitaFiltro(txtCodigo);
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


    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        EventoPesquisar();
    }

    private void EventoPesquisar()
    {
        Session["CD_ALTER"] = null;
        Session["NM_CLIFOR"] = null;
        bool bFiltroValido = false;
        string sWhere = "";
        string strErros = "";
        string select = "";
        if (rdbTodos.Checked)
        {
            bFiltroValido = true;
            sWhere = ("(d.nm_clifor IS NOT NULL)");
        }
        else if (rdbCodigo.Checked)
        {
            bFiltroValido = (!txtCodigo.EmBranco());
            if (bFiltroValido)
            {
                UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
                string sCdClifor = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR", "CD_CLIFOR", "CD_ALTER = '" + txtCodigo.GetValor().ToString() + "'");
                sWhere = ("(CD_CLIENTE ='" + sCdClifor + "')");
            }
            else
                strErros = "Código do Cliente inválido!!!";
        }
        else if (rdbGuerra.Checked)
        {
            bFiltroValido = (!txtNomeCliente.EmBranco());
            if (bFiltroValido)
                sWhere = ("(UPPER(d.nm_clifor) LIKE '%" +
                    txtNomeCliente.GetValor() + "%')");
            else
                strErros = "Nome de cliente inválido!!!";
        }

        if (bFiltroValido)
        {
            UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

            StringBuilder sql = new StringBuilder();

            sql.Append("select count(d.NR_DOC)TOTAL from doc_ctr d ");
            sql.Append("where coalesce(d.st_baixa,'') <> 'B' ");
            sql.Append("and d.cd_empresa = '{0}' ");
            sql.Append("and d.cd_vend1 = '" + objUsuario.CodigoVendedor + "' ");
            sql.Append("and d.dt_venci < current_date and {1}");

            select = string.Format(sql.ToString(), objUsuario.oTabelas.sEmpresa, sWhere);
            DataTable dtRet = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(select);

            int QtdePendencias = Convert.ToInt32(dtRet.Rows[0]["TOTAL"]);
            if (QtdePendencias > 0)
            {
                sql = new StringBuilder();
                sql.Append("select cast(d.DT_VENCI as date) DT_VENCI, ");
                sql.Append("v.NM_VEND, ");
                sql.Append("d.CD_CLIENTE, ");
                sql.Append("d.CD_VEND1, ");
                sql.Append("d.nm_clifor as nm_guerra , ");
                sql.Append("d.VL_DOC, ");
                sql.Append("cast(d.DT_EMI as date) DT_EMI  ");
                sql.Append("from DOC_CTR d  ");
                sql.Append("join vendedor v on v.cd_vend = d.cd_vend1 ");
                sql.Append("where coalesce(st_baixa,'') <> 'B' ");
                sql.Append("and cd_empresa = '{0}' ");
                sql.Append("and d.cd_vend1 = '" + objUsuario.CodigoVendedor + "' ");
                sql.Append("and dt_venci < current_date and {1} ");
                sql.Append("order by  d.nm_clifor");

                select = string.Format(sql.ToString(), objUsuario.oTabelas.sEmpresa, sWhere);
                Session["DadosConsultaPendencias"] = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(select);

                Page.Response.Redirect("~/ViewPendencias.aspx");
            }
            else
            {

                MessageHLP.ShowPopUpMsg("Nenhuma pendência foi encontrada!", this.Page);
            }
        }
        else
        {

            MessageHLP.ShowPopUpMsg(strErros.ToString(), this.Page);
        }
    }



    private void PesquisarCliente()
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        DataTable dtClientes = null;


        StringBuilder str = new StringBuilder();
        str.Append("SELECT CD_CLIFOR, CD_ALTER, NM_GUERRA, NM_CLIFOR, DS_ENDNOR, NR_ENDNOr, NM_BAIRRONOR, NM_CIDNOR, CD_UFNOR, ST_PESSOAJ, CD_CGC, cd_insest, CD_CPF, CD_RG, CD_CEPNOR, CD_FONENOR, CD_FAXNOR   ");
        str.Append("FROM CLIFOR ");
        str.Append("WHERE ");
        str.Append("((CLIFOR.ST_INATIVO <> 'S') OR (CLIFOR.ST_INATIVO IS NULL)) AND (CLIFOR.CD_VEND1 = '" + objUsuario.CodigoVendedor + "') AND (NM_GUERRA IS NOT NULL) ");
        str.Append("AND (CD_ALTER ='" + txtCodigo.Text + "')");
        dtClientes = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(
            str.ToString());
        DataColumn[] ChavePrimaria = new DataColumn[] { dtClientes.Columns["CD_ALTER"] };
        dtClientes.PrimaryKey = ChavePrimaria;

        if (dtClientes.Rows.Count == 0)
        {
            MessageHLP.ShowPopUpMsg("Não existem registros para o filtro selecionado", this.Page);
            if (Session["CD_ALTER"] != null)
            {
                txtCodigo.Text = Session["CD_ALTER"].ToString();
            }
            else
            {
                txtCodigo.Text = "";
            }
        }
        else
        {
            BaseDAO.CancelarOperacaoObjetoDAO((BaseDAO)Session["ObjetoClienteDetalhado"]);
            txtCodigo.Text = txtCodigo.Text;
            txtCliente.Text = dtClientes.Rows[0]["NM_CLIFOR"].ToString();
        }



    }


    protected void btnPesqCliente_Click(object sender, EventArgs e)
    {
        Session["PesquisaPendencia"] = true;
        Response.Redirect("~/ConsultaClientes.aspx");
    }
    protected void btnCliente_Click(object sender, EventArgs e)
    {
        PesquisarCliente();
    }
}