using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HLP.Web;
using System.Data;
using System.IO;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

public partial class ViewPedido : System.Web.UI.Page
{
    private ReportDocument rpt = new ReportDocument();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string sUser = UsuarioWeb.GetNomeUsuarioConectado(Session);
            if (sUser == "")
            {
                Response.Redirect("~/Home.aspx");
            }
            dsPedido TabelaImpressao = (dsPedido)Session["PedidoRes"];
            rpt.Load(Server.MapPath("rptPedido.rpt"));
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