using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using HLP.Web;
using System.Data;
using System.IO;
using CrystalDecisions.Shared;

public partial class ViewVendasPorRepresentanteListagem : System.Web.UI.Page
{
    private ReportDocument rpt = new ReportDocument();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string sUser = UsuarioWeb.GetNomeUsuarioConectado(Session);
            if (Request["DT_INI"] == null || Request["DT_FIM"] == null)
            {
                Response.Redirect("~/Home.aspx");
            }
            string sDataInicial = Request["DT_INI"].ToString();
            string sDataFinal = Request["DT_FIM"].ToString();

            dsVendas dsImpressao = (dsVendas)Session["VendasRepresLista"];
            rpt.Load(Server.MapPath("rptVendasRepres.rpt"));
            rpt.SetDataSource(dsImpressao);
            CrystalReportViewer1.ReportSource = rpt;
            string sfiltro = string.Format("Período de {0} à {1}", sDataInicial, sDataFinal);
            if (rpt.DataDefinition.FormulaFields["Filtro"] != null)
            {
                rpt.DataDefinition.FormulaFields["Filtro"].Text = "\"" + sfiltro + "\"";
            }
            GerarPDF();
        }

    }
    private void GerarPDF()
    {

        MemoryStream oStream = (MemoryStream)rpt.ExportToStream(ExportFormatType.PortableDocFormat);
        Response.Clear();
        Response.Buffer = true;
        Response.ContentType = "application/pdf";
        Response.BinaryWrite(oStream.ToArray());
        Response.End();

    }
}