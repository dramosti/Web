using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using System.IO;
using CrystalDecisions.Shared;

public partial class Visualizacao : System.Web.UI.Page
{
    private rpt_GCA_NF rpt = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            rpt = new rpt_GCA_NF();
            rpt.SetDataSource((Session["ListReport"] as List<boRelNfFrete>));
            CrystalReportViewer1.ReportSource = rpt;
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