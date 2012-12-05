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
using System.Text;
using FirebirdSql.Data.FirebirdClient;

public partial class VisualizarRelatorioVisita : System.Web.UI.Page
{
    private ReportDocument rpt = new ReportDocument();

    protected void Page_Load(object sender, EventArgs e)
    {
        if ((Session["Grupo"] == null) ||
            (Session["CodigoUsuario"] == null))
        {
            Response.Redirect("~/Login.aspx");
        }
       
        if (Request.QueryString["ID"].ToString() != String.Empty)
        {
            string strID = Request.QueryString["ID"].ToString();

            StringBuilder str = new StringBuilder();
            str.Append("SELECT LANCREL.NR_LANC, LANCREL.DT_REL, LANCREL.NM_CONTATO, ");
            str.Append("LANCREL.CD_CLIENTE, LANCREL.CD_OPERADO, ");
            str.Append("LANCREL.CD_SISTEMA, LANCREL.CD_TIPOREL, ");
            str.Append("LANCREL.ST_FATURA, LANCREL.HR_ENTRADA, ");
            str.Append("LANCREL.HR_DESCONTO, LANCREL.HR_TOTAL, LANCREL.HR_SAIDA, ");
            str.Append("LANCREL.NM_GUERRA, LANCREL.DS_OBS, ACESSO.NM_OPERADO, SISTEMA.DS_SISTEMA, ST_MAQ, DS_MAQUINA ");
            str.Append("FROM LANCREL ");
            str.Append("INNER JOIN ACESSO ON (ACESSO.CD_OPERADO = LANCREL.CD_OPERADO) ");
            str.Append("LEFT JOIN SISTEMA ON (SISTEMA.CD_SISTEMA = LANCREL.CD_SISTEMA) ");
            str.Append("WHERE (NR_LANC = '" + strID + "')");

            FbCommand cmdSelect = (FbCommand)HlpBancoDados.CommandSelect(str.ToString());
            cmdSelect.Connection.Open();

            FbDataReader drVisita = cmdSelect.ExecuteReader();

            DataTable dtVisita = new DataTable("ConsultaVisita");
            
            dtVisita.Columns.Add("NR_LANC");
            dtVisita.Columns.Add("HR_ENTRADA");
            dtVisita.Columns.Add("HR_SAIDA");
            dtVisita.Columns.Add("HR_DESCONTO");
            dtVisita.Columns.Add("NM_GUERRA");
            dtVisita.Columns.Add("DT_REL");
            dtVisita.Columns.Add("NM_OPERADO");
            dtVisita.Columns.Add("TITULO");
            dtVisita.Columns.Add("NM_CONTATO");
            dtVisita.Columns.Add("DS_OBS");
            dtVisita.Columns.Add("ST_MAQ");
            dtVisita.Columns.Add("DS_MAQUINA");

            DataRow dr;

            if (drVisita.Read())
            {
                dr = dtVisita.NewRow();
                dr["NR_LANC"] = drVisita["NR_LANC"];
                dr["HR_ENTRADA"] = drVisita["HR_ENTRADA"];
                dr["HR_SAIDA"] = drVisita["HR_SAIDA"];
                dr["HR_DESCONTO"] = drVisita["HR_DESCONTO"];
                dr["NM_GUERRA"] = drVisita["NM_GUERRA"];
                dr["DT_REL"] = GetFormataDataRetorno(drVisita["DT_REL"].ToString());
                dr["NM_OPERADO"] = drVisita["NM_OPERADO"];
                dr["NM_CONTATO"] = drVisita["NM_CONTATO"];
                if (drVisita["CD_TIPOREL"].ToString() == "4")
                    dr["TITULO"] = "SOFTWARE";
                else if (drVisita["CD_TIPOREL"].ToString() == "3")
                    dr["TITULO"] = "HARDWARE";
                
                if (!drVisita["DS_OBS"].ToString().Equals(String.Empty))
                {
                    byte[] bt = (byte[])drVisita["DS_OBS"];
                    dr["DS_OBS"] = ASCIIEncoding.Default.GetString(bt);
                }

                if (!drVisita["DS_MAQUINA"].ToString().Equals(String.Empty))
                {
                    byte[] bt = (byte[])drVisita["DS_MAQUINA"];
                    dr["DS_MAQUINA"] = ASCIIEncoding.Default.GetString(bt);
                }

                if (drVisita["ST_MAQ"].ToString() == "1")
                {
                    dr["ST_MAQ"] = "Máquina em Perfeito Estado";
                }
                else
                {
                    dr["ST_MAQ"] = "Máquina com Problema";
                }

                dtVisita.Rows.Add(dr);
            }

            drVisita.Close();
            cmdSelect.Connection.Close();

            StringBuilder strOs = new StringBuilder();
            strOs.Append("SELECT CD_OS, NR_LANC FROM ITLANCREL ");
            strOs.Append("WHERE (NR_LANC = '" + strID + "') ORDER BY CD_OS");

            FbCommand cmdOs = (FbCommand)HlpBancoDados.CommandSelect(strOs.ToString());
            
            cmdOs.Connection.Open();

            FbDataReader drOs = cmdOs.ExecuteReader();

            DataTable dtOs = new DataTable("OS");
            dtOs.Columns.Add("CD_OS");
            dtOs.Columns.Add("NR_LANCREL");

            DataRow drItensRel;

            while (drOs.Read())
            {
                drItensRel = dtOs.NewRow();
                drItensRel[0] = drOs[0];
                drItensRel[1] = drOs[1];
                dtOs.Rows.Add(drItensRel);
            }

            drOs.Close();

            cmdOs.Connection.Close();

            rpt.Load(Server.MapPath("crRelVisita.rpt"));
            rpt.SetDataSource(dtVisita);
            rpt.Subreports["crOsRelVisita.rpt"].SetDataSource(dtOs);
            RptVisita.ReportSource = rpt;
            RptVisita.Visible = true;
        }
       

    }
    protected string GetFormataDataRetorno(string strData)
    {
        string sRetorno = String.Empty;

        if (!strData.Equals(String.Empty))
        {
            DateTime tDataAtua = Convert.ToDateTime(strData);
            string sDia = tDataAtua.Day.ToString();
            string sMes = tDataAtua.Month.ToString();
            string sAno = tDataAtua.Year.ToString();

            if (sDia.Length < 2)
                sDia = "00".Substring(0, 2 - sDia.Length) + sDia;
            if (sMes.Length < 2)
                sMes = "00".Substring(0, 2 - sMes.Length) + sMes;
            sRetorno = sDia + "/" + sMes + "/" + sAno;


        }
        return sRetorno;
    }
    protected void btnImprimir_Click(object sender, EventArgs e)
    {
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
        Response.End( );
    }
}
