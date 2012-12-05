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

public partial class ViewPendencias : System.Web.UI.Page
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
            DataTable TabelaImpressao = (DataTable)Session["DadosConsultaPendencias"];
            rpt.Load(Server.MapPath("rptPendenciasCliente.rpt"));
            rpt.SetDataSource(TabelaImpressao);
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