using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HLP.Web.Controles;
using HLP.Web;
using System.Data;
using System.Text;

public partial class Ger_PesquisaPendenciaCliente : System.Web.UI.Page
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
        }
    }

    protected void rdbGuerra_CheckedChanged(object sender, EventArgs e)
    {
        HabilitaFiltro(txtNomeCliente);
        DesabilitaFiltro(txtCodigo);
        DesabilitaFiltro(txtVendedor);
    }
    protected void rdbCodigo_CheckedChanged(object sender, EventArgs e)
    {
        HabilitaFiltro(txtCodigo);
        DesabilitaFiltro(txtNomeCliente);
        DesabilitaFiltro(txtVendedor);
    }
    protected void rdbVendedor_CheckedChanged(object sender, EventArgs e)
    {
        HabilitaFiltro(txtVendedor);
        DesabilitaFiltro(txtCodigo);
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
        bool bFiltroValido = false;
        string sWhere = "";
        string strErros = "";
        string select = "";
        if (rdbCodigo.Checked)
        {
            bFiltroValido = (!txtCodigo.EmBranco());
            if (bFiltroValido)
                sWhere = ("(CD_CLIENTE ='" +
                    txtCodigo.GetValor().ToString().PadLeft(7, '0') + "')");
            else
                strErros = "Código do Cliente inválido!!!";
        }
        else if (rdbGuerra.Checked)
        {
            bFiltroValido = (!txtNomeCliente.EmBranco());
            if (bFiltroValido)
                sWhere = ("(UPPER(d.NM_GUERRA) LIKE '%" +
                    txtNomeCliente.GetValor() + "%')");
            else
                strErros = "Nome de Guerra inválido!!!";
        }
        else if (rdbVendedor.Checked)
        {
            bFiltroValido = (!txtVendedor.EmBranco());
            if (bFiltroValido)
                sWhere = ("(CD_VEND1 = '" +
                   txtVendedor.GetValor().ToString().PadLeft(7, '0') + "')");
            else
                strErros = "Código do Vendedor!!!";
        }
        if (bFiltroValido)
        {
            UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

            StringBuilder sql = new StringBuilder();

            sql.Append("select count(d.NR_DOC)TOTAL from doc_ctr d ");
            sql.Append("where coalesce(d.st_baixa,'') <> 'B' ");
            sql.Append("and d.cd_empresa = '{0}' ");
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
                sql.Append("d.NM_GUERRA, ");
                sql.Append("d.VL_DOC, ");
                sql.Append("cast(d.DT_EMI as date) DT_EMI  ");
                sql.Append("from DOC_CTR d  ");
                sql.Append("join vendedor v on v.cd_vend = d.cd_vend1 ");
                sql.Append("where coalesce(st_baixa,'') <> 'B' ");
                sql.Append("and cd_empresa = '{0}' ");
                sql.Append("and dt_venci < current_date and {1} ");
                sql.Append("order by  d.NM_GUERRA");

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
}