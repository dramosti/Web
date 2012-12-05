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
using FirebirdSql.Data.FirebirdClient;
using System.Text;

public partial class ConsultarRelatorioVisita : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sGrupo = (string)Session["Grupo"];

        if ((sGrupo != null) && (!sGrupo.Equals("ADMIN")))
        {
            TextBox3.Visible = false;
            Label4.Visible = false;
        }
        else
        {
            TextBox3.Visible = true;
            Label4.Visible = true;
        }

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (TextBox4.Text.Equals("__/__/____"))
        {
            TextBox4.Text = String.Empty;
        }
        if (TextBox5.Text.Equals("__/__/____"))
        {
            TextBox5.Text = String.Empty;
        }
        
        if ( (TextBox4.Text.Equals(String.Empty)) && (!TextBox5.Text.Equals(String.Empty)) )
        {
            TextBox4.Text = TextBox5.Text;
        }else
        if ( (!TextBox4.Text.Equals(String.Empty)) && (TextBox5.Text.Equals(String.Empty)) )
        {
            TextBox5.Text = TextBox4.Text;
        }

        if ((!TextBox1.Text.Equals(String.Empty)) || (!TextBox2.Text.Equals(String.Empty))
            || (!TextBox3.Text.Equals(String.Empty)) || (!TextBox4.Text.Equals(String.Empty)) 
            || (!TextBox5.Text.Equals(String.Empty)) || (!TextBox6.Text.Equals(String.Empty)))
        {
            Label7.Visible = false;
            Label7.Text = String.Empty;

            StringBuilder strVisita = new StringBuilder();

            strVisita.Append("SELECT LANCREL.NR_LANC, LANCREL.HR_ENTRADA, LANCREL.HR_SAIDA, ");
            strVisita.Append("LANCREL.NM_GUERRA, LANCREL.DT_REL, LANCREL.CD_OPERADO, ACESSO.NM_OPERADO ");
            strVisita.Append("FROM LANCREL ");
            strVisita.Append("INNER JOIN ACESSO ON (ACESSO.CD_OPERADO = LANCREL.CD_OPERADO) ");
            strVisita.Append("WHERE ");
            
            if ((!TextBox1.Text.Equals(String.Empty)))
                strVisita.Append("(LANCREL.NR_LANC = '" + HlpBancoDados.GetCodigo(TextBox1.Text.Trim()) + "') AND ");
            if ((!TextBox2.Text.Equals(String.Empty)))
                strVisita.Append("(LANCREL.NM_GUERRA LIKE '" + TextBox2.Text.ToUpper().Trim() + "%') AND ");
            if (TextBox3.Visible)
            {
                if ((!TextBox3.Text.Equals(String.Empty)))
                    strVisita.Append("(ACESSO.NM_OPERADO = '" + TextBox3.Text.ToUpper().Trim() + "') AND ");
            }
            else
            {
                string sUsu = (string)Session["CodigoUsuario"];
                strVisita.Append("(LANCREL.CD_OPERADO = '" + sUsu + "') AND ");
            }
            if ((!TextBox6.Text.Equals(String.Empty)))
                strVisita.Append("(LANCREL.CD_REL = '" + HlpBancoDados.GetCodigo(TextBox6.Text.Trim()) + "') AND ");
            if ((!TextBox4.Text.Equals(String.Empty)) || (!TextBox5.Text.Equals(String.Empty)))
                strVisita.Append("(LANCREL.DT_REL BETWEEN '" + HlpBancoDados.ConverterData(TextBox4.Text.Trim()) + "' AND '" + HlpBancoDados.ConverterData(TextBox5.Text.Trim()) + "') AND");

            strVisita.Append("(LANCREL.HR_ENTRADA IS NOT NULL) ORDER BY LANCREL.NR_LANC");

            FbCommand cmdVisita = (FbCommand)HlpBancoDados.CommandSelect(strVisita.ToString());

            cmdVisita.Connection.Open();

            FbDataReader drVisita = cmdVisita.ExecuteReader();

            DataTable dtVisita = new DataTable("ConsultaVisita");
            dtVisita.Columns.Add("NR_LANC");
            dtVisita.Columns.Add("HR_ENTRADA");
            dtVisita.Columns.Add("HR_SAIDA");
            dtVisita.Columns.Add("NM_GUERRA");
            dtVisita.Columns.Add("DT_REL");
            dtVisita.Columns.Add("NM_OPERADO");
            dtVisita.Columns.Add("CD_OPERADO");

            DataRow dr;

            while (drVisita.Read())
            {
                dr = dtVisita.NewRow();
                dr["NR_LANC"] = drVisita["NR_LANC"];
                dr["HR_ENTRADA"] = drVisita["HR_ENTRADA"];
                dr["HR_SAIDA"] = drVisita["HR_SAIDA"];
                dr["NM_GUERRA"] = drVisita["NM_GUERRA"];
                dr["DT_REL"] = GetFormataDataRetorno(drVisita["DT_REL"].ToString());
                dr["NM_OPERADO"] = drVisita["NM_OPERADO"];
                dr["CD_OPERADO"] = drVisita["CD_OPERADO"];

                dtVisita.Rows.Add(dr);
            }

            drVisita.Close();
            cmdVisita.Connection.Close();

            int iResultado = dtVisita.Rows.Count;
            if (iResultado > 0)
            {
                Session["TabelaVisita"] = dtVisita;
                Response.Redirect("~/ResultadoConsultaVisita.aspx");
            }
            else
                Response.Redirect("~/Informacao.aspx?RET=Não foi encontrado os dados solicitados!");
            
        }
        else
        {
            Label7.Visible = true;
            Label7.Text = "Ao menos um dos campo precisa estar preenchido!";
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
}
