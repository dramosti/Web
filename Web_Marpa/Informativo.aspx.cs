using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HLP.Web;
using System.Data;
using System.Text;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Web.Configuration;

public partial class Informativo : System.Web.UI.Page
{
    protected static string sCodigoPedido = String.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string sUser = UsuarioWeb.GetNomeUsuarioConectado(Session);
            if (sUser == "")
            {
                Response.Redirect("~/Login.aspx");
            }

            if (Request["PARAMETERCODIGO"] != null)
            {
                sCodigoPedido = Request["PARAMETERCODIGO"].ToString();
                lblInfo.Text = "Pedido nº " + sCodigoPedido.Trim() + " foi realizado com sucesso!";
                btnNovoPedido.Visible = true;
                btnImprimir.Visible = true;
            }
            else if (Request["CD_PEDIDO_EMAIL"] != null)
            {
                sCodigoPedido = Request["CD_PEDIDO_EMAIL"].ToString();
                lblInfo.Text = "Pedido nº " + sCodigoPedido.Trim() + "!";
                btnNovoPedido.Visible = false;
                btnImprimir.Visible = true;
            }
        }
    }

    protected void btnImprimir_Click1(object sender, EventArgs e)
    {
        if (this.btnImprimir.Text == "Visualizar Pedido")
        {
            ExportPDF();
            Response.Redirect("~/ViewPedido.aspx?ANEXO=" + sCodigoPedido);
        }
    }

    private void CarregaDataTableParaImpressao()
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        DataTable dtPedMovipend = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(MontaQueryPedido(objUsuario, "MOVIPEND"));
        DataTable dtPedMoviitem = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(MontaQueryPedido(objUsuario, "MOVITEM"));


        dsPedido ds = new dsPedido();
        dsPedido.PedidoRow drPedido;

        foreach (DataRow row in dtPedMovipend.Rows)
        {
            if (row["QT_PROD"].ToString() != "")
            {
                drPedido = ds.Pedido.NewPedidoRow();
                CopyRow(drPedido, row);
                ds.Pedido.Rows.Add(drPedido);
            }

        }
        foreach (DataRow row in dtPedMoviitem.Rows)
        {
            if (row["QT_PROD"].ToString() != "")
            {
                drPedido = ds.Pedido.NewPedidoRow();
                CopyRow(drPedido, row);
                ds.Pedido.Rows.Add(drPedido);
            }
        }
        Session["PedidoRes"] = ds;
    }

    private static void CopyRow(dsPedido.PedidoRow dr, DataRow row)
    {
        dr.DT_PEDIDO = row["DT_PEDIDO"].ToString();
        dr.CD_CLIENTE = row["CD_CLIENTE"].ToString();
        dr.NM_CLIFOR = row["NM_CLIFOR"].ToString();
        dr.DS_ENDCLI = row["DS_ENDCLI"].ToString();
        dr.NM_BAIRROCLI = row["NM_BAIRROCLI"].ToString();
        dr.NM_CIDCLI = row["NM_CIDCLI"].ToString();
        dr.CD_UFCLI = row["CD_UFCLI"].ToString();
        dr.CD_CEPCLI = row["CD_CEPCLI"].ToString();
        dr.CD_FONECLI = row["CD_FONECLI"].ToString();
        dr.DS_PRAZO = row["DS_PRAZO"].ToString();
        dr.DS_PROD = row["DS_PROD"].ToString();
        dr.VL_UNIPROD = row["VL_UNIPROD"].ToString();
        dr.QT_PROD = row["QT_PROD"].ToString();
        dr.NM_VENDEDOR = row["NM_VENDEDOR"].ToString();
        dr.CD_PEDCLI = row["CD_PEDCLI"].ToString();
        dr.CD_FONEVEND = row["CD_FONEVEND"].ToString();
        dr.DS_TIPODOC = row["DS_TIPODOC"].ToString();
        dr.ST_PEDIDO = row["ST_PEDIDO"].ToString();
        dr.VL_DESC = row["VL_DESCONTO_VALOR"].ToString();
    }

    private string MontaQueryPedido(UsuarioWeb objUsuario, string sTabelaFilho)
    {
        StringBuilder str = new StringBuilder();
        str.Append("SELECT PEDIDO.CD_EMPRESA, PEDIDO.CD_PEDIDO CD_PEDCLI, ");
        str.Append("(CLIFOR.CD_ALTER) CD_CLIENTE, ");
        str.Append("PEDIDO.DT_PEDIDO, ");
        str.Append("TPDOC.DS_TIPODOC,");
        str.Append("VENDEDOR.NM_VEND NM_VENDEDOR, ");
        str.Append("VENDEDOR.CD_FONE CD_FONEVEND, ");
        str.Append("PEDIDO.CD_PRAZO, ");
        str.Append("PRAZOS.DS_PRAZO, ");
        str.Append("CLIFOR.NM_CLIFOR, ");
        str.Append("CLIFOR.DS_ENDNOR DS_ENDCLI, ");
        str.Append("CLIFOR.NM_BAIRRONOR NM_BAIRROCLI, ");
        str.Append("CLIFOR.NM_CIDNOR NM_CIDCLI, ");
        str.Append("CLIFOR.CD_UFNOR CD_UFCLI, ");
        str.Append("CLIFOR.CD_CEPNOR CD_CEPCLI, ");
        str.Append("CLIFOR.CD_FONENOR CD_FONECLI, ");
        str.Append("CLIFOR.DS_CONTATO DS_CONTATOCLI, ");
        str.Append("PEDIDO.DS_OBS_WEB, ");
        str.Append(sTabelaFilho.Equals("MOVITEM") ? "'F' ST_PEDIDO," : "'P' ST_PEDIDO,");
        str.Append("M.VL_UNIPROD, ");
        str.Append("M.QT_PROD, ");
        str.Append("M.VL_DESCONTO_VALOR, ");
        str.Append("M.DS_PROD ");
        str.Append("FROM PEDIDO ");
        str.Append("INNER JOIN EMPRESA ON (EMPRESA.CD_EMPRESA = PEDIDO.CD_EMPRESA) ");
        str.Append("LEFT OUTER JOIN {0} M ON ((M.CD_EMPRESA = PEDIDO.CD_EMPRESA) AND (M.CD_PEDIDO = PEDIDO.CD_PEDIDO)) ");
        str.Append("LEFT OUTER JOIN VENDEDOR ON (VENDEDOR.CD_VEND = PEDIDO.CD_VEND1) ");
        str.Append("LEFT OUTER JOIN PRAZOS ON (PRAZOS.CD_PRAZO = PEDIDO.CD_PRAZO) ");
        str.Append("LEFT OUTER JOIN CLIFOR ON (CLIFOR.CD_CLIFOR = PEDIDO.CD_CLIENTE) ");
        str.Append("LEFT OUTER JOIN TPDOC ON (TPDOC.CD_TIPODOC = PEDIDO.CD_TIPODOC)");
        str.Append("WHERE PEDIDO.cd_vend1 = '" + objUsuario.CodigoVendedor.ToString() + "' ");
        str.Append("AND PEDIDO.CD_PEDIDO = '" + sCodigoPedido + "' ");
        str.Append("ORDER BY PEDIDO.CD_PEDIDO ");
        return string.Format(str.ToString(), sTabelaFilho);
    }
    protected void btnNovoPedido_Click1(object sender, EventArgs e)
    {
        Response.Redirect("~/Pedido.aspx");
    }
    protected void btnEmail_Click(object sender, EventArgs e)
    {
        ExportPDF();
        Response.Redirect("~/EnviarEmail.aspx?ANEXO=" + sCodigoPedido);
    }

    private void ExportPDF()
    {
        try
        {
            lblInfo.Text = "Gerando arquivo pdf para anexo";
            DirectoryInfo dinfo = new DirectoryInfo(Server.MapPath("Pedidos"));
            if (!dinfo.Exists)
            {
                dinfo.Create();
            }
            if (File.Exists(Server.MapPath("Pedidos\\" + sCodigoPedido + ".pdf")))
            {
                File.Delete(Server.MapPath("Pedidos\\" + sCodigoPedido + ".pdf"));
            }
            CarregaDataTableParaImpressao();
            ReportDocument rpt = new ReportDocument();
            dsPedido TabelaImpressao = (dsPedido)Session["PedidoRes"];
            rpt.Load(Server.MapPath("rptPedido.rpt"));
            rpt.SetDataSource(TabelaImpressao);

            CrystalDecisions.Web.CrystalReportViewer cryView = new CrystalDecisions.Web.CrystalReportViewer();
            ExportOptions CrExportOptions;
            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
            CrDiskFileDestinationOptions.DiskFileName = Server.MapPath("Pedidos\\" + sCodigoPedido + ".pdf");
            CrExportOptions = rpt.ExportOptions;
            {
                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                CrExportOptions.FormatOptions = CrFormatTypeOptions;
            }
            rpt.Export();
            lblInfo.Text = "Exportando arquivo para o servidor";
        }
        catch (Exception ex)
        {
            throw ex;
            //lblInfo.Text = ex.Message + (ex.InnerException != null ? ex.InnerException.Message : "");
        }
    }

    protected void btnEmail0_Click(object sender, EventArgs e)
    {
        DirectoryInfo dinfo = new DirectoryInfo(Server.MapPath("Pedidos"));
        lblInfo.Text = dinfo.FullName;
        MessageHLP.ShowPopUpMsg(dinfo.Exists.ToString(), this.Page);
    }
}