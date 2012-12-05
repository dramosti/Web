using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using HLP.Web;
using System.Data;
using System.IO;

public partial class _Default : System.Web.UI.Page
{
    private ReportDocument rpt = new ReportDocument();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string sUserGestor = UsuarioWeb.GetNomeUsuarioGestorConectado(Session);
            string sUser = UsuarioWeb.GetNomeUsuarioConectado(Session);
            if (sUserGestor == "")
            {
                if (sUser == "")
                {
                    Response.Redirect("~/Home.aspx");
                }
            }
            rpt.Load(Server.MapPath("rptPedidoComissao.rpt"));
            rpt.SetDataSource((DataSet)Session["DataSetPedidoComissao"]);
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