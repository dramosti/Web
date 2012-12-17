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
            CarregaDataTableParaImpressao();
            Response.Redirect("~/ViewPedido.aspx");

            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "_new", "window.open('ViewPedido.aspx');", true);
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
        dr.DT_PEDIDO = Convert.ToDateTime(row["DT_PEDIDO"]);
        dr.CD_CLIENTE = row["CD_CLIENTE"].ToString();//NAO TEM
        dr.NM_CLIFOR = row["NM_CLIFOR"].ToString();
        dr.DS_ENDCLI = row["DS_ENDCLI"].ToString();
        dr.NM_BAIRROCLI = row["NM_BAIRROCLI"].ToString();
        dr.NM_CIDCLI = row["NM_CIDCLI"].ToString();
        dr.CD_UFCLI = row["CD_UFCLI"].ToString();
        dr.CD_CEPCLI = row["CD_CEPCLI"].ToString();
        dr.CD_FONECLI = row["CD_FONECLI"].ToString();
        dr.CD_FAXCLI = row["CD_FAXCLI"].ToString();
        dr.CD_CGCCLI = row["CD_CGCCLI"].ToString();
        dr.CD_INSESTCLI = row["CD_INSESTCLI"].ToString();

        dr.DS_ENDENT = row["DS_ENDENT"].ToString();
        dr.NM_BAIRROENT = row["NM_BAIRROENT"].ToString();
        dr.NM_CIDENT = row["NM_CIDENT"].ToString();
        dr.CD_CEPENT = row["CD_CEPENT"].ToString();
        dr.DS_OBS_WEB = row["DS_OBS_WEB"].ToString();
        dr.CD_UFENT = row["CD_UFENT"].ToString();

        dr.DS_PRAZO = row["DS_PRAZO"].ToString();
        dr.DS_PROD = row["DS_PROD"].ToString();
        
        dr.VL_UNIPROD = row["VL_UNIPROD"].ToString();
        dr.QT_PROD = row["QT_PROD"].ToString();
        dr.DT_PRAZOEN = row["DT_PRAZOEN"].ToString();
        dr.CD_OPER = row["CD_OPER"].ToString();
        dr.CD_ALTER = row["CD_ALTER"].ToString();
        dr.CD_TPUNID = row["CD_TPUNID"].ToString();
        dr.NM_VENDEDOR = row["NM_VENDEDOR"].ToString();
        dr.CD_PEDCLI = row["CD_PEDCLI"].ToString();
        dr.CD_PEDIDO = row["CD_PEDIDO"].ToString();
        dr.CD_FONEVEND = row["CD_FONEVEND"].ToString();
        dr.DS_TIPODOC = row["DS_TIPODOC"].ToString();
        dr.ST_PEDIDO = row["ST_PEDIDO"].ToString();
        dr.DS_CONTATOCLI = row["DS_CONTATOCLI"].ToString();

        dr.VL_TOTALBRUTO = row["VL_TOTBRUTO"].ToString(); //total bruto do pedido qtd * preco unitario
        dr.VL_TOTALCOMDESC = row["VL_TOTALCOMDESC"].ToString();
        dr.VL_TOTAL = row["VL_TOTAL"].ToString(); //total 

        dr.CD_INSESTEMP = row["CD_INSESTEMP"].ToString();
        dr.NM_EMPRESA = row["NM_EMPRESA"].ToString();
        dr.NM_BAIRROEMP = row["NM_BAIRROEMP"].ToString();
        dr.NM_CIDEMP = row["NM_CIDEMP"].ToString();
        dr.DS_ENDEMP = row["DS_ENDEMP"].ToString();
        dr.CD_UFEMP = row["CD_UFEMP"].ToString();
        dr.CD_CEPEMP = row["CD_CEPEMP"].ToString();
        dr.CD_CGCEMP = row["CD_CGCEMP"].ToString();
        dr.CD_FONEEMP = row["CD_FONEEMP"].ToString();
        dr.CD_FAXEMP = row["CD_FAXEMP"].ToString();
        dr.CD_EMAILEMP = row["CD_EMAILEMP"].ToString();
        dr.NM_GUERRA = row["NM_GUERRA"].ToString();
        dr.NM_TRANS = row["NM_TRANS"].ToString();
        dr.CD_FONETRANS_WEB = row["CD_FONETRANS_WEB"].ToString();
        dr.NM_TRANS_WEB = row["NM_TRANS_WEB"].ToString();

    }

    private string MontaQueryPedido(UsuarioWeb objUsuario, string sTabelaFilho)
    {
        StringBuilder str = new StringBuilder();
        str.Append("SELECT PEDIDO.CD_EMPRESA, PEDIDO.CD_PEDIDO, ");
        str.Append("PEDIDO.CD_CLIENTE, ");
        str.Append("PEDIDO.CD_TIPODOC, PEDIDO.CD_TRANS, ");
        str.Append("PEDIDO.DS_ANOTA, ");
        str.Append("PEDIDO.DS_PEDCLI CD_PEDCLI, PEDIDO.DT_ABER, ");
        str.Append("PEDIDO.DT_PEDIDO, ");
        str.Append("PEDIDO.CD_VEND1, PEDIDO.CD_VEND2, ");
        str.Append("TPDOC.DS_TIPODOC,"); //NOVO
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
        str.Append("PEDIDO.DS_OBS_WEB, "); 
        str.Append(sTabelaFilho.Equals("MOVITEM") ? "'F' ST_PEDIDO," : "'P' ST_PEDIDO,");
        str.Append("ENDENTR.CD_UFENT, ");
        str.Append("M.CD_PROD, ");
        str.Append("M.VL_UNIPROD, M.VL_TOTLIQ, ");
        str.Append("M.DT_PRAZOEN, M.CD_OPER, ");
        str.Append("M.CD_TPUNID, M.QT_PROD, M.DS_PROD, ");
        str.Append("M.CD_PEDCLI, M.CD_ALTER, ");
        str.Append("CAST (M.QT_PROD * M.VL_UNIPROD AS NUMERIC(18,2)) AS ");
        str.Append("VL_TOTBRUTO, ");
        str.Append("CAST((cast( M.VL_UNIPROD*M.VL_COEF as numeric(18,4))*(1-M.VL_PERDESC/100))* M.QT_PROD AS NUMERIC(18,2)) AS VL_TOTAL, ");
        str.Append(" CAST (CASE WHEN M.VL_COEF < 1 THEN cast(M.VL_COEF as numeric(18,6)) ELSE 1 END * cast(M.QT_PROD * M.VL_UNIPROD as numeric(18,6))  AS NUMERIC(18,2)) AS VL_TOTALCOMDESC ");
        str.Append("FROM PEDIDO ");
        str.Append("INNER JOIN EMPRESA ON (EMPRESA.CD_EMPRESA = PEDIDO.CD_EMPRESA) ");
        str.Append("LEFT OUTER JOIN {0} M ON ((M.CD_EMPRESA = PEDIDO.CD_EMPRESA) AND (M.CD_PEDIDO = PEDIDO.CD_PEDIDO)) ");
        str.Append("LEFT OUTER JOIN VENDEDOR ON (VENDEDOR.CD_VEND = PEDIDO.CD_VEND1) ");
        str.Append("LEFT OUTER JOIN PRAZOS ON (PRAZOS.CD_PRAZO = PEDIDO.CD_PRAZO) ");
        str.Append("LEFT OUTER JOIN TRANSPOR ON (TRANSPOR.CD_TRANS = PEDIDO.CD_TRANS) ");
        str.Append("LEFT OUTER JOIN CLIFOR ON (CLIFOR.CD_CLIFOR = PEDIDO.CD_CLIENTE) ");
        str.Append("LEFT OUTER JOIN ENDENTR ON (ENDENTR.CD_CLIENTE = CLIFOR.CD_CLIFOR) ");
        str.Append("LEFT OUTER JOIN TPDOC ON (TPDOC.CD_TIPODOC = PEDIDO.CD_TIPODOC) ");
        str.Append("WHERE PEDIDO.CD_VEND1 = " + "'" + objUsuario.CodigoVendedor.ToString() + "'");
        str.Append(" AND PEDIDO.CD_PEDIDO = '" + sCodigoPedido.Trim() + "'");
        str.Append(" ORDER BY PEDIDO.CD_PEDIDO");


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