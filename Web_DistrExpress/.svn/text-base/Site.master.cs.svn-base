using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HLP.Web;
using System.Data;
using System.Text;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string sUser = UsuarioWeb.GetNomeUsuarioConectado(Session);
            UsuarioWeb objUsuario = new UsuarioWeb();
            DataTable dtDadosEmpresa = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("EMPRESA", "nm_empresa,nm_guerra", "CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa.Trim() + "'");


            foreach (DataRow row in dtDadosEmpresa.Rows)
            {

                lblNmFantasia.Text = "Módulo de Vendas - " + row["nm_guerra"].ToString();
                lblNmEmpresa.Text = row["nm_empresa"].ToString();
            }


            if (sUser != "")
            {
                string sMessage = string.Empty;
                int iHour = DateTime.Now.Hour;
                if (iHour >= 0 && iHour <= 12)
                {
                    sMessage = "Bom dia {0}";
                }
                else if (iHour > 12 && iHour <= 18)
                {
                    sMessage = "Boa tarde {0}";
                }
                else
                {
                    sMessage = "Boa noite {0}";
                }
                lblRepres.Text = string.Format(sMessage, UsuarioWeb.GetNomeUsuarioConectado(Session));
                btnSair.OnClientClick = "return confirm('Deseja realmente sair do Sistema ?');";
            }
            else
            {
                lblRepres.Text = "";
            }
        }

    }
    protected void btnRelatorioVendas_Click(object sender, EventArgs e)
    {
        string sUser = UsuarioWeb.GetNomeUsuarioConectado(Session);
        if (sUser != "")
        {

            Session["DadosConsultaRepresentante"] = null;
            UsuarioWeb objUsuario = Session["ObjetoUsuario"] as UsuarioWeb;
            string sCodRepresentante = objUsuario.oTabelas.CdVendedorAtual;

            DateTime data = DateTime.Now;
            int iTotal = DateTime.DaysInMonth(data.Year, data.Month);

            string firstDayMonth = new DateTime(data.Year, data.Month, 1).ToShortDateString().Replace("/", ".");
            string lastDayMonth = new DateTime(data.Year, data.Month, iTotal).ToShortDateString().Replace("/", ".");
            string firstDayWeek = data.AddDays(1 - Convert.ToDouble(data.DayOfWeek)).ToShortDateString().Replace("/", ".");
            string lastDayWeek = data.AddDays(7 - Convert.ToDouble(data.DayOfWeek)).ToShortDateString().Replace("/", ".");
            string today = data.ToShortDateString().Replace("/", ".");

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
            sQuery.Append("select coalesce(sum(p.vl_totalped),0) TOTAL_MES,v.nm_vend, case ");
            sQuery.Append("when extract( month from p.dt_pedido) = 1 then 'Janeiro' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 2 then 'Fevereiro' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 3 then 'Março' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 4 then 'Abril' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 5 then 'Maio' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 6 then 'Junho' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 7 then 'Julho' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 8 then 'Agosto' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 9 then 'Setembro' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 10 then 'Outubro' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 11 then 'Novembro' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 12 then 'Dezembro' ");
            sQuery.Append("end as Mes ,");
            sQuery.Append("(select coalesce(sum(p.vl_totalped),0)  ");
            sQuery.Append("from pedido p ");
            sQuery.Append("inner join clifor c on p.cd_cliente = c.cd_clifor ");
            sQuery.Append("inner join vendedor v on v.cd_vend = c.cd_vend1 ");
            sQuery.Append("where c.cd_vend1 = '{0}' ");
            sQuery.Append("and p.dt_pedido between '{4}' and '{5}' and ");
            sQuery.Append("p.cd_tipodoc in ({3})) TOTAL_SEMANA, ");
            sQuery.Append("(select coalesce(sum(p.vl_totalped),0)  ");
            sQuery.Append("from pedido p ");
            sQuery.Append("inner join clifor c on p.cd_cliente = c.cd_clifor ");
            sQuery.Append("inner join vendedor v on v.cd_vend = c.cd_vend1 ");
            sQuery.Append("where c.cd_vend1 = '{0}' ");
            sQuery.Append("and p.dt_pedido = '{6}' and ");
            sQuery.Append("p.cd_tipodoc in ({3})) TOTAL_DIA ");
            sQuery.Append("from pedido p inner join clifor c on p.cd_cliente = c.cd_clifor ");
            sQuery.Append("inner join vendedor v on v.cd_vend = c.cd_vend1 ");
            sQuery.Append("where ");
            sQuery.Append("c.cd_vend1 = '{0}' and ");
            sQuery.Append("p.dt_pedido between '{1}' and '{2}' and ");
            sQuery.Append("p.cd_tipodoc in ({3}) ");
            sQuery.Append("group by extract( month from p.dt_pedido), v.nm_vend order by extract( month from p.dt_pedido) ");

            string sQueryFinal = string.Format(sQuery.ToString(), sCodRepresentante, firstDayMonth, lastDayMonth, sTpdocFinal, firstDayWeek, lastDayWeek, today);
            Session["DadosConsultaRepresentante"] = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(sQueryFinal);


            Page.Response.Redirect("~/ViewVendasRepresentante.aspx");
        }
    }


    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Session["ObjetoUsuario"] = null;
        Page.Response.Redirect("~/Login.aspx");
    }

}
