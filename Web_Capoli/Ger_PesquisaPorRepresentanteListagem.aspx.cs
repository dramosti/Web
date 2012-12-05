using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using HLP.Web;
using System.IO;
using CrystalDecisions.Shared;
using System.Text;

public partial class Ger_PesquisaPorRepresentanteListagem : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string sUser = UsuarioWeb.GetNomeUsuarioGestorConectado(Session);
            if (sUser == "")
            {
                Response.Redirect("~/Home.aspx");
            }
            if (!Page.IsPostBack)
            {
                txtDataFinal.Text = DateTime.Today.ToShortDateString();
                txtDataInicial.Text = (DateTime.Today.AddDays(-10)).ToShortDateString();
                UsuarioWeb objUsuario = Session["ObjetoUsuario"] as UsuarioWeb;
                cbxVendedor.DataTextField = "nm_guerra";
                cbxVendedor.DataValueField = "cd_vend";
                cbxVendedor.DataSource = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("select cd_vend, nm_guerra from vendedor where coalesce(st_acessa_web,'N') = 'S'");
                cbxVendedor.DataBind();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void btnVisualizar_Click(object sender, EventArgs e)
    {
        try
        {

            string sUser = UsuarioWeb.GetNomeUsuarioConectado(Session);
            Session["DadosConsultaRepresentante"] = null;
            UsuarioWeb objUsuario = Session["ObjetoUsuario"] as UsuarioWeb;
            string sCodRepresentante = string.Empty;
            if (ckbVendedor.Checked)
            {
                sCodRepresentante = cbxVendedor.SelectedValue;
            }
            if (txtDataInicial.Text != "" && txtDataFinal.Text != "")
            {
                dsVendas ds = GetVendasByRepresentantes(Convert.ToDateTime(txtDataInicial.Text), Convert.ToDateTime(txtDataFinal.Text), objUsuario, sCodRepresentante);
                Session["VendasRepresLista"] = ds;
                string sDtInicial = txtDataInicial.Text.Replace("/", ".");
                string sDtFinal = txtDataFinal.Text.Replace("/", ".");
                Response.Redirect("~/ViewVendasPorRepresentanteListagem.aspx?DT_INI=" + sDtInicial + "&DT_FIM=" + sDtFinal);
            }
            else
            {
                MessageHLP.ShowPopUpMsg("As datas estão incorretas.", this);
            }

        }
        catch (Exception ex)
        {
            MessageHLP.ShowPopUpMsg(ex.Message, this.Page);
        }
    }

    protected dsVendas GetVendasByRepresentantes(DateTime dtIni, DateTime dtFim, UsuarioWeb objUsuario, string sCodRepresentante = "")
    {
        dsVendas ds = new dsVendas();
        string sdtIni = dtIni.ToShortDateString().Replace("/", ".");
        string sdtFim = dtFim.ToShortDateString().Replace("/", ".");
        string stpdocs = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("EMPRESA", "DS_TPDOCWEB", "CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa + "'");
        string sTpdocFinal = "";
        foreach (string item in stpdocs.Split(';'))
        {
            string[] tpdoc = item.Split(',');
            if (tpdoc.Length > 1)
            {
                sTpdocFinal += "'" + tpdoc[1].ToString().Trim() + "',";
            }
        }
        sTpdocFinal = sTpdocFinal.Remove(sTpdocFinal.Length - 1, 1);
        StringBuilder sQuery = new StringBuilder();
        sQuery.Append("select v.cd_vend, coalesce(sum(p.vl_totalped),0) TOTAL,v.nm_vend NM_VEND ");
        sQuery.Append("from pedido p inner join clifor c on p.cd_cliente = c.cd_clifor inner join vendedor v on v.cd_vend = c.cd_vend1 ");
        sQuery.Append("where coalesce(v.st_acessa_web,'N') = 'S'  and p.dt_pedido  between '{0}' and '{1}' and ");
        sQuery.Append("p.cd_tipodoc in ({2}) ");
        if (sCodRepresentante != "")
        {
            sQuery.Append("and v.cd_vend = '" + sCodRepresentante + "' ");
        }
        sQuery.Append("group by v.cd_vend,v.nm_vend order by v.cd_vend ");
        string sQueryFinal = string.Format(sQuery.ToString(), sdtIni, sdtFim, sTpdocFinal);

        StringBuilder sQueryTotDia = new StringBuilder();
        sQueryTotDia.Append("select coalesce(sum(p.vl_totalped),0)  from pedido p inner join clifor c on p.cd_cliente = c.cd_clifor inner join vendedor v on ");
        sQueryTotDia.Append("v.cd_vend = c.cd_vend1 where p.cd_vend1 = '{0}' and p.dt_pedido = '{1}' and p.cd_tipodoc in ('{2}')");


        dsVendas.VendasRow rowVendas = ds.Vendas.NewVendasRow();
        foreach (DataRow item in objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(sQueryFinal).Rows)
        {
            rowVendas.CD_VEND = item["CD_VEND"].ToString();
            rowVendas.TOTAL = item["TOTAL"].ToString();
            rowVendas.NM_VEND = item["NM_VEND"].ToString();
            rowVendas.TOTAL_DIA = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("PEDIDO", "coalesce(sum(vl_totalped),0)",
                                                                                string.Format("cd_vend1 = '{0}' and dt_pedido = '{1}' and cd_tipodoc in ({2})",
                                                                                rowVendas.CD_VEND,
                                                                                DateTime.Today.ToShortDateString().Replace("/", "."),
                                                                                sTpdocFinal));
            ds.Vendas.AddVendasRow(rowVendas);
            rowVendas = ds.Vendas.NewVendasRow();
        }
        return ds;

    }
}