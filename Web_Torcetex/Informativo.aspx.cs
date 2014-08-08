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
                ParametroPesquisa objParametros = (ParametroPesquisa)Session["FiltroPedidos"];
                objParametros.Limpar();
                objParametros.AddCriterio(string.Format("PEDIDO.CD_PEDIDO = '{0}'", sCodigoPedido));
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
            PesquisarDados(sCodigoPedido);
            Response.Redirect("~/ViewPedido.aspx?ANEXO=" + sCodigoPedido);
        }
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
        dr.CD_PROD = row["CD_PROD"].ToString();
    }
    protected void btnNovoPedido_Click1(object sender, EventArgs e)
    {
        Response.Redirect("~/Pedido.aspx");
    }
    protected void btnEmail_Click(object sender, EventArgs e)
    {
        //        ExportPDF();
        Response.Redirect("~/EnviarEmail.aspx?ANEXO=" + sCodigoPedido);
    }
    protected void btnEmail0_Click(object sender, EventArgs e)
    {
        DirectoryInfo dinfo = new DirectoryInfo(Server.MapPath("Pedidos"));
        lblInfo.Text = dinfo.FullName;
        MessageHLP.ShowPopUpMsg(dinfo.Exists.ToString(), this.Page);
    }



    private void PesquisarDados(string sNameFile)
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

        DataTable dtClientes = new DataTable();
        StringBuilder str = new StringBuilder();
        str.Append("SELECT PEDIDO.CD_EMPRESA, PEDIDO.CD_PEDIDO, ");
        str.Append("PEDIDO.CD_CLIENTE, ");
        str.Append("PEDIDO.CD_TIPODOC, PEDIDO.CD_TRANS, ");
        str.Append("PEDIDO.DS_ANOTA, ");
        str.Append("PEDIDO.DS_PEDCLI CD_PEDCLI, PEDIDO.DT_ABER, ");
        str.Append("PEDIDO.DT_PEDIDO, ");
        str.Append("PEDIDO.CD_VEND1, PEDIDO.CD_VEND2, ");
        str.Append("EMPRESA.IM_EMPRESA, EMPRESA.NM_EMPRESA, ");
        str.Append("EMPRESA.NM_BAIRRONOR NM_BAIRROEMP, EMPRESA.NM_CIDNOR NM_CIDEMP, ");
        str.Append("EMPRESA.DS_ENDNOR DS_ENDEMP, EMPRESA.CD_UFNOR CD_UFEMP, EMPRESA.CD_CEPNOR CD_CEPEMP, ");
        str.Append("EMPRESA.CD_CGC CD_CGCEMP, EMPRESA.CD_INSEST CD_INSESTEMP, EMPRESA.CD_FONENOR CD_FONEEMP, ");
        str.Append("EMPRESA.CD_FAXNOR CD_FAXEMP, EMPRESA.CD_EMAIL CD_EMAILEMP, ");
        str.Append("VENDEDOR.NM_GUERRA, VENDEDOR.NM_VEND NM_VENDEDOR, VENDEDOR.CD_FONE CD_FONEVEND, ");
        str.Append("PEDIDO.CD_PRAZO, PRAZOS.DS_PRAZO, ");
        str.Append("TRANSPOR.NM_TRANS, ");
        str.Append("TRANSPOR.CD_TRANS, PEDIDO.CD_FONETRANS_WEB, PEDIDO.NM_TRANS_WEB, ");
        str.Append("CLIFOR.NM_CLIFOR, CLIFOR.DS_ENDNOR DS_ENDCLI, ");
        str.Append("CLIFOR.NM_BAIRRONOR NM_BAIRROCLI, CLIFOR.NM_CIDNOR NM_CIDCLI, ");
        str.Append("CLIFOR.CD_UFNOR CD_UFCLI, CLIFOR.CD_CEPNOR CD_CEPCLI, ");
        str.Append("CLIFOR.CD_FONENOR CD_FONECLI, CLIFOR.CD_FAXNOR CD_FAXCLI, ");
        str.Append("CLIFOR.CD_CGC CD_CGCCLI, CLIFOR.CD_INSEST CD_INSESTCLI, CLIFOR.DS_CONTATO DS_CONTATOCLI, ");
        str.Append("ENDENTR.DS_ENDENT, ENDENTR.NM_BAIRROENT, ");
        str.Append("ENDENTR.NM_CIDENT, ENDENTR.CD_CEPENT, ");
        str.Append("PEDIDO.DS_OBS_WEB, "); //25989 - DIEGO
        str.Append("ENDENTR.CD_UFENT, ");
        str.Append("MOVIPEND.CD_PROD, ");
        str.Append("MOVIPEND.VL_UNIPROD, MOVIPEND.VL_TOTLIQ, ");
        str.Append("MOVIPEND.VL_DESCONTO_VALOR VL_DESC, ");
        str.Append("MOVIPEND.DT_PRAZOEN, MOVIPEND.CD_OPER, ");
        str.Append("MOVIPEND.CD_TPUNID, MOVIPEND.QT_PROD, MOVIPEND.DS_PROD, ");
        str.Append("MOVIPEND.CD_PEDCLI, MOVIPEND.CD_ALTER, ");
        str.Append("CAST (MOVIPEND.QT_PROD * MOVIPEND.VL_UNIPROD AS NUMERIC(18,2)) AS ");
        str.Append("VL_TOTBRUTO, ");
        str.Append("CAST((cast( MOVIPEND.VL_UNIPROD*MOVIPEND.VL_COEF as numeric(18,4))*(1-MOVIPEND.VL_PERDESC/100))* MOVIPEND.QT_PROD AS NUMERIC(18,2)) AS VL_TOTAL, ");//Diego - OS_25085 - 13/10/2010    
        str.Append(" CAST (CASE WHEN MOVIPEND.VL_COEF < 1 THEN cast(MOVIPEND.VL_COEF as numeric(18,6)) ELSE 1 END * cast(MOVIPEND.QT_PROD * MOVIPEND.VL_UNIPROD as numeric(18,6))  AS NUMERIC(18,2)) AS VL_TOTALCOMDESC "); // Diego - OS_25126 - 19/10/10
        str.Append("FROM PEDIDO ");
        str.Append("INNER JOIN EMPRESA ON (EMPRESA.CD_EMPRESA = PEDIDO.CD_EMPRESA) ");
        str.Append("LEFT OUTER JOIN VENDEDOR ON (VENDEDOR.CD_VEND = PEDIDO.CD_VEND1) ");
        str.Append("LEFT OUTER JOIN PRAZOS ON (PRAZOS.CD_PRAZO = PEDIDO.CD_PRAZO) ");
        str.Append("LEFT OUTER JOIN TRANSPOR ON (TRANSPOR.CD_TRANS = PEDIDO.CD_TRANS) ");
        str.Append("LEFT OUTER JOIN CLIFOR ON (CLIFOR.CD_CLIFOR = PEDIDO.CD_CLIENTE) ");
        str.Append("LEFT OUTER JOIN ENDENTR ON (ENDENTR.CD_CLIENTE = CLIFOR.CD_CLIFOR) ");
        str.Append("LEFT OUTER JOIN MOVIPEND ON ((MOVIPEND.CD_EMPRESA = PEDIDO.CD_EMPRESA) ");
        str.Append("AND (MOVIPEND.CD_PEDIDO = PEDIDO.CD_PEDIDO)) ");
        str.Append("WHERE {0} ORDER BY PEDIDO.CD_PEDIDO");

        ParametroPesquisa objParametros = (ParametroPesquisa)Session["FiltroPedidos"];
        if (objParametros == null)
        {
            ParametroPesquisaCapoli.InicializarParametroPesquisa(
            "FiltroPedidos", "PEDIDO", this.Session);
            objParametros = (ParametroPesquisa)Session["FiltroPedidos"];
        }
        objParametros.Limpar();
        objParametros.AddCriterio(string.Format("PEDIDO.CD_PEDIDO = '{0}'", sCodigoPedido));
        dtClientes = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(string.Format(str.ToString(), objParametros.GetWhere()));
        Session["PedidoRes"] = dtClientes;
        ExportPDF(sNameFile);
        Response.Redirect("~/ViewPedido.aspx?ANEXO=" + sNameFile);
    }
    private void ExportPDF(string sNameFile)
    {
        try
        {
            DirectoryInfo dinfo = new DirectoryInfo(Server.MapPath("Pedidos"));
            if (!dinfo.Exists)
            {
                dinfo.Create();
            }
            if (File.Exists(Server.MapPath("Pedidos\\" + sNameFile)))
            {
                File.Delete(Server.MapPath("Pedidos\\" + sNameFile + ".pdf"));
            }
            //            PesquisarDados();
            ReportDocument rpt = new ReportDocument();
            DataTable TabelaImpressao = (DataTable)Session["PedidoRes"];
            rpt.Load(Server.MapPath("RelatorioPedido.rpt"));
            rpt.SetDataSource(TabelaImpressao);

            CrystalDecisions.Web.CrystalReportViewer cryView = new CrystalDecisions.Web.CrystalReportViewer();
            ExportOptions CrExportOptions;
            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
            CrDiskFileDestinationOptions.DiskFileName = Server.MapPath("Pedidos\\" + sNameFile + ".pdf");
            CrExportOptions = rpt.ExportOptions;
            {
                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                CrExportOptions.FormatOptions = CrFormatTypeOptions;
            }
            rpt.Export();
        }
        catch (Exception ex)
        {
            throw ex;
            //lblInfo.Text = ex.Message + (ex.InnerException != null ? ex.InnerException.Message : "");
        }
    }

}