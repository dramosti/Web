using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HLP.Web;
using System.Data;
using HLP.Dados.Vendas;
using System.Web.UI.DataVisualization.Charting;

public partial class Ger_VendasPorRepresentante : System.Web.UI.Page
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
            CarregaRepresentantes();
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

    protected void CarregaRepresentantes()
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        DataTable dtRepres = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("select v.cd_vend, v.nm_vend from vendedor v where v.st_acessa_web = 'S'");
        cbxRepresentante.DataSource = dtRepres;
        cbxRepresentante.DataBind();
    }

    private struct DadosGrafico
    {
        public string sNmRepresentante { get; set; }
        public string sMes { get; set; }
        public double dTotal { get; set; }
        public string sColumnMes { get; set; }
        public string sColumnTotal { get; set; }
    }


    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        string sCodigoRepresentante = "";
        if (!cbkTodos.Checked)
        {
            sCodigoRepresentante = cbxRepresentante.SelectedValue.ToString();
        }

        DataTable dtResult = HlpFuncoesVendas.GetVendasPorRepresentanteAnual(objUsuario.oTabelas, cbxAno.SelectedItem.Text, sCodigoRepresentante);

        DataTable dtFinal = new DataTable();

        string sNomeRepresentante = "";
        int iCount = 0;
        int iCountRepresentantes = 0;
        List<DadosGrafico> lDadosGrafico = new List<DadosGrafico>();

        foreach (DataRow row in dtResult.Rows)
        {
            DadosGrafico stDadosGrafico = new DadosGrafico { sNmRepresentante = row["NM_VEND"].ToString(), sMes = row["MES"].ToString(), dTotal = Convert.ToDouble(row["TOTAL"].ToString()) };
            string sMes = "";
            string sTotal = "";
            if (sNomeRepresentante != row["NM_VEND"].ToString())
            {
                sNomeRepresentante = row["NM_VEND"].ToString();
                iCountRepresentantes = 0;
            }
        
            if (iCountRepresentantes == 0)
            {
                iCountRepresentantes++;
                sMes = "MES" + iCount.ToString();
                sTotal = "TOTAL" + iCount.ToString();
                stDadosGrafico.sColumnMes = sMes;
                stDadosGrafico.sColumnTotal = sTotal;

                dtFinal.Columns.Add(new DataColumn(sTotal, System.Type.GetType("System.Double")));
                dtFinal.Columns.Add(new DataColumn(sMes, System.Type.GetType("System.String")));
                grafico.Series.Add(new Series
                                            {
                                                XValueMember = sMes,
                                                XValueType = ChartValueType.String,
                                                YValueMembers = sTotal,
                                                YValueType = ChartValueType.Double,
                                                ChartType = SeriesChartType.Column,
                                                LegendText = sNomeRepresentante,
                                                IsVisibleInLegend = true,
                                                IsValueShownAsLabel = true
                                            });
                iCount++;
            }
            lDadosGrafico.Add(stDadosGrafico);
        }

        foreach (string m in lDadosGrafico.Select(c => c.sMes).Distinct())
        {
            DataRow dr = dtFinal.NewRow();
            foreach (DadosGrafico dg in lDadosGrafico.Where(c => c.sMes == m))
            {
                string sColMes = lDadosGrafico.Where(c => c.sNmRepresentante == dg.sNmRepresentante && c.sColumnMes != "").FirstOrDefault().sColumnMes;
                string sColTot = lDadosGrafico.Where(c => c.sNmRepresentante == dg.sNmRepresentante && c.sColumnTotal != "").FirstOrDefault().sColumnTotal;

                dr[sColMes] = dg.sMes;
                dr[sColTot] =  dg.dTotal.ToString("N2");
            }
            dtFinal.Rows.Add(dr);
        }

        grafico.DataSource = dtFinal;
        grafico.DataBind();
        MultViewVendasPorRepres.ActiveViewIndex = 1;
    }
    protected void btnNovaPesquisa_Click(object sender, EventArgs e)
    {
        grafico.Series.Clear();
        MultViewVendasPorRepres.ActiveViewIndex = 0;
    }
    protected void cbkTodos_CheckedChanged(object sender, EventArgs e)
    {
        if (cbkTodos.Checked)
        {
            cbxRepresentante.Enabled = false;
        }
        else
        {
            cbxRepresentante.Enabled = true;
        }

    }
}