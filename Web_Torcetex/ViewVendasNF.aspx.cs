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
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.CrystalReports;
using System.IO;
using HLP.Web;

public partial class ViewVendasNF : System.Web.UI.Page
{


    private ReportDocument rpt = new ReportDocument();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DateTime dDataFinal = DateTime.Now;
            string sUser = UsuarioWeb.GetNomeUsuarioConectado(Session);
            if (sUser == "")
            {
                Response.Redirect("~/Login.aspx");
            }
        }
        DataTable TabelaImpNf = (DataTable)Session["NF"];
        DataTable TabelaImpDupli = (DataTable)Session["DUPLICATAS"];
        DataTable TabelaImpItens = (DataTable)Session["ITENS"];

        rpt.Load(Server.MapPath("RelatorioVendaNF.rpt"));
        rpt.SetDataSource(TabelaImpNf);
        //rpt.Subreports["SubRelatorioItensNF.rpt"].OpenSubreport("SubRelatorioItensNF.rpt");
        rpt.Subreports["SubRelatorioItensNF.rpt"].SetDataSource(TabelaImpItens);
        //rpt.Subreports[1].OpenSubreport("SubRelatorioDupli.rpt");
        rpt.Subreports["SubRelatorioDupli.rpt"].SetDataSource(TabelaImpDupli);
        CrystalReportViewer1.ReportSource = rpt;
        GerarPDF();

    }
    private void GerarPDF()
    {
        MemoryStream oStream; // using System.IO
        oStream = (MemoryStream)rpt.ExportToStream(ExportFormatType.PortableDocFormat);
        Response.Clear();
        Response.Buffer = true;
        Response.ContentType = "application/pdf";
        Response.BinaryWrite(oStream.ToArray());
        Response.End();
    }
}