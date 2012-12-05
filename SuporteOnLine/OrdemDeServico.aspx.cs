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
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Net;
using System.Net.Mail;
using System.IO;

public partial class OrdemDeServico : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        

        if (!IsPostBack)
        {
            MultiView1.ActiveViewIndex = 0;
            DataTable Cliente = GetCliente();
            DDLCliente.DataSource = Cliente;
            DDLCliente.DataMember = "Cliente";
            DDLCliente.DataTextField = "Nome";
            DDLCliente.DataValueField = "Id";
            DDLCliente.DataBind();

            ListItem item1 = new ListItem();
            ListItem item2 = new ListItem();
            ListItem item3 = new ListItem();
            ListItem item4 = new ListItem();
            ListItem item5 = new ListItem();
            ListItem item6 = new ListItem();

            item1.Text = "MELHORIAS GERAL";
            item1.Value = "ME";

            item2.Text = "ERROS GERAL";
            item2.Value = "ER";

            item3.Text = "OUTROS GERAL";
            item3.Value = "OU";

            item4.Text = "MELHORIAS ESPECIF.";
            item4.Value = "MC";

            item5.Text = "ERROS ESPECIF.";
            item5.Value = "EC";

            item6.Text = "OUTROS ESPECIF.";
            item6.Value = "OC";


            DDLTipoOs.Items.Clear();
            DDLTipoOs.Items.Add(item1);
            DDLTipoOs.Items.Add(item2);
            DDLTipoOs.Items.Add(item3);
            DDLTipoOs.Items.Add(item4);
            DDLTipoOs.Items.Add(item5);
            DDLTipoOs.Items.Add(item6);
            DDLTipoOs.DataBind();
        }

    }
    protected DataTable GetCliente()
    {
        StringBuilder strCliente = new StringBuilder();

        strCliente.Append("SELECT DISTINCT CLIFOR.NM_GUERRA, CLIFOR.CD_CLIFOR ");
        strCliente.Append("FROM CLIFOR INNER JOIN SITCONTR ON (SITCONTR.CD_CLIFOR = CLIFOR.CD_CLIFOR) ");
        strCliente.Append("WHERE (CLIFOR.CD_CLIFOR IS NOT NULL) AND (CLIFOR.ST_INATIVO = 'N') ");
        strCliente.Append("GROUP BY CLIFOR.NM_GUERRA, CLIFOR.CD_CLIFOR, ");
        strCliente.Append("CLIFOR.ST_ATIVOSUPORTE, SITCONTR.ST_CONT_HORAS, SITCONTR.ST_MANUT ");
        strCliente.Append("HAVING (CASE WHEN CLIFOR.ST_ATIVOSUPORTE IS NULL THEN ");
        strCliente.Append("(CASE WHEN SITCONTR.ST_CONT_HORAS = 'N' THEN ");
        strCliente.Append("(CASE WHEN SITCONTR.ST_MANUT = 'S' THEN 'S' ELSE SITCONTR.ST_MANUT END) ");
        strCliente.Append("ELSE (CASE WHEN SITCONTR.ST_CONT_HORAS IS NULL THEN 'N' ELSE SITCONTR.ST_CONT_HORAS END) END) ");
        strCliente.Append("ELSE CLIFOR.ST_ATIVOSUPORTE END) = 'S' ");
        strCliente.Append("ORDER BY CLIFOR.NM_GUERRA");

        FbCommand fbSelect = (FbCommand)HlpBancoDados.CommandSelect(strCliente.ToString());

        fbSelect.Connection.Open();

        FbDataReader drCliente = fbSelect.ExecuteReader();

        DataTable dtCliente = new DataTable("Cliente");
        dtCliente.Columns.Add("Nome").AllowDBNull = true;
        dtCliente.Columns.Add("Id").AllowDBNull = true;

        DataRow dr;

        while (drCliente.Read())
        {
            dr = dtCliente.NewRow();
            dr["Nome"] = drCliente["NM_GUERRA"];
            dr["Id"] = drCliente["CD_CLIFOR"];
            dtCliente.Rows.Add(dr);
        }

        drCliente.Close();
        fbSelect.Connection.Close();

        return dtCliente;

    }

    protected string GetDataCorreta()
    {
        string sDia = DateTime.Today.Day.ToString();
        string sMes = DateTime.Today.Month.ToString();
        string sAno = DateTime.Today.Year.ToString();

        if (sDia.Length < 2)
            sDia = "00".Substring(0, 2 - sDia.Length) + sDia;
        if (sMes.Length < 2)
            sMes = "00".Substring(0, 2 - sMes.Length) + sMes;
        string sRetorno = sDia + "/" + sMes + "/" + sAno;

        return sRetorno;

    }

    protected DataTable GetSistema()
    {
        StringBuilder strSistema = new StringBuilder();
        
        strSistema.Append("SELECT SITCONTR.CD_SISTEMA, SISTEMA.DS_SISTEMA ");
        strSistema.Append("FROM SITCONTR ");
        strSistema.Append("INNER JOIN SISTEMA ON (SISTEMA.CD_SISTEMA = SITCONTR.CD_SISTEMA) ");
        strSistema.Append("WHERE (SITCONTR.CD_CLIFOR = '" + DDLCliente.SelectedItem.Value + "') ");
        strSistema.Append("ORDER BY SISTEMA.DS_SISTEMA");
        

        FbCommand fbSelect = (FbCommand)HlpBancoDados.CommandSelect(strSistema.ToString());

        fbSelect.Connection.Open();

        FbDataReader drSistema = fbSelect.ExecuteReader();

        DataTable dtSistema = new DataTable("Sistema");
        dtSistema.Columns.Add("Nome").AllowDBNull = true;
        dtSistema.Columns.Add("Id").AllowDBNull = true;

        DataRow dr;

        while (drSistema.Read())
        {
            dr = dtSistema.NewRow();
            dr["Nome"] = drSistema["DS_SISTEMA"];
            dr["Id"] = drSistema["CD_SISTEMA"];
            dtSistema.Rows.Add(dr);
        }

        drSistema.Close();
        fbSelect.Connection.Close();

        return dtSistema;

    }
    protected void DDLCliente_TextChanged(object sender, EventArgs e)
    {
        DDLSistema.Items.Clear();

        DataTable Sistema = GetSistema();
        DDLSistema.DataSource = Sistema;
        DDLSistema.DataMember = "Sistema";
        DDLSistema.DataTextField = "Nome";
        DDLSistema.DataValueField = "Id";
        DDLSistema.DataBind();
    }
    protected void btnGravar_Click(object sender, EventArgs e)
    {
        string strCodUsuario = (string)Session["CodigoUsuario"];
        if (txtMemo.Text != String.Empty)
        {
            FbCommand SelectCodigoOS = (FbCommand)HlpBancoDados.CommandSelect("SELECT DS_RESULTADO FROM SP_FALINHA(Cast(GEN_ID(ORDEMSER, 1) as varchar(7)), 7)");
            SelectCodigoOS.Connection.Open();
            
            string sCodOS = SelectCodigoOS.ExecuteScalar().ToString();

            SelectCodigoOS.Connection.Close();
            
            ListBox lbCorrecao = new ListBox();

            StringBuilder sbCorrecoes = new StringBuilder();
            
            sbCorrecoes.AppendLine("1- LAYOUTS ALTERADOS");
            sbCorrecoes.AppendLine("");
            sbCorrecoes.AppendLine("NOME                          ALTERACAO");
            sbCorrecoes.AppendLine("");
            sbCorrecoes.AppendLine("");
            sbCorrecoes.AppendLine("__________________________________________________________________________________________________________________________");
            sbCorrecoes.AppendLine("2-TELAS ALTERADAS");
            sbCorrecoes.AppendLine("");
            sbCorrecoes.AppendLine("NOME                          ALTERACAO");
            sbCorrecoes.AppendLine("");
            sbCorrecoes.AppendLine("");
            sbCorrecoes.AppendLine("__________________________________________________________________________________________________________________________");
            sbCorrecoes.AppendLine("3-MUDANÇA DE ESTRUTURA");
            sbCorrecoes.AppendLine("");
            sbCorrecoes.AppendLine("TABELA                   CAMPO                 ALTERACOES");
            sbCorrecoes.AppendLine("");
            sbCorrecoes.AppendLine("");
            sbCorrecoes.AppendLine("__________________________________________________________________________________________________________________________");
            sbCorrecoes.AppendLine("4-MUDANÇA DE CONCEITOS");
            sbCorrecoes.AppendLine("");
            sbCorrecoes.AppendLine("");
            sbCorrecoes.AppendLine("__________________________________________________________________________________________________________________________");
            sbCorrecoes.AppendLine("5-CORREÇÕES");
            sbCorrecoes.AppendLine("");
            sbCorrecoes.AppendLine("");
            sbCorrecoes.AppendLine("__________________________________________________________________________________________________________________________");
            sbCorrecoes.AppendLine("6-ACOES A SEREM TOMADAS NO CLIENTE");
            sbCorrecoes.AppendLine("");
            sbCorrecoes.AppendLine("");
            sbCorrecoes.AppendLine("__________________________________________________________________________________________________________________________");
            sbCorrecoes.AppendLine("7-UPLOAD");
            sbCorrecoes.AppendLine("");

            byte[] bt = System.Text.Encoding.Default.GetBytes(sbCorrecoes.ToString());


            StringBuilder str = new StringBuilder();
            str.Append("INSERT INTO ORDEMSER (CD_OS, DT_OS, DT_ENTREGA, ST_OS, NR_ORDEM, ST_FATURA, ");
            str.Append("DT_CAD, CD_USUINC, DS_OS, CD_SISTEMA, TP_OS, CD_CLIENTE, NM_GUERRA, CD_PROGR, NM_PROGR, DS_CORRECOES) VALUES ('" + sCodOS.Trim() + "', '");
            str.Append(GetDataCorreta().Replace("/", ".") + "', '" + GetDataCorreta().Replace("/", ".") + "', 'AG', '99', 'N', '" + GetDataCorreta().Replace("/", ".") + "', '");
            str.Append(strCodUsuario + "', '" + txtMemo.Text.ToUpper().Trim() + "', '" + DDLSistema.SelectedItem.Value.Trim() + "', '");
            str.Append(DDLTipoOs.SelectedItem.Value.Trim() + "', '" + DDLCliente.SelectedItem.Value.Trim() + "', '" + DDLCliente.SelectedItem.Text.Trim() + "', '0000MASTER', 'MASTER', '" + ASCIIEncoding.Default.GetString(bt) + "')");

            FbCommand cmd = (FbCommand)HlpBancoDados.CommandSelect(str.ToString());
            cmd.Connection.Open();

            int iRetorno = cmd.ExecuteNonQuery();

            cmd.Connection.Close();

            if (iRetorno > 0)
            {
                MultiView1.ActiveViewIndex = 1;
                if (EnviarEmail(sCodOS))
                {
                    lblTitulo.Text = "Ordem de Serviço: " + sCodOS + " incluido com sucesso.";
                    lblLancamento.Text = "Foi enviado um e-mail para a HLP!";
                }
                else
                {
                    lblTitulo.Text = "Ordem de Serviço: " + sCodOS + " incluido com sucesso.";
                    lblLancamento.Text = "Não foi possível enviar um e-mail para a HLP!";
                }
            }
        }
    }

    protected bool EnviarEmail(string OS)
    {
        SmtpClient Cliente = new SmtpClient();
        Cliente.Host = "smtp.hlp.com.br";
        Cliente.Port = 25;
        string sEmailEnvio = ConfigurationManager.AppSettings["EmailPadrao"].ToString();
        MailAddress Destino = new MailAddress(sEmailEnvio);
        MailAddress Remeter = new MailAddress("pedidoweb@hlp.com.br");

        MailMessage email = new MailMessage(Remeter, Destino);
       
        StringBuilder str = new StringBuilder();
        str.Append("<html><body>");
        str.Append("<div><font face='Tahoma' size='2'> Olá, você acaba de receber uma Ordem de Serviço via web incluida por " + Session["Usuario"].ToString() + ". </font></div><br><br>");
        str.Append("<table border='1' width='30%' id='tablePedido'>");
        str.Append("<tr><td><div><b><font face='Tahoma' size='2'>Ordem de Serviço Nº</font></b></td>");
        str.Append("<td><div><font face='Tahoma' size='2'>" + OS.Trim() + "</font></div></td></tr>");
        str.Append("</table><br><br><br>");
        str.Append("<div><b><font face='Tahoma' size='2' color='#0000FF'>A empresa HLP sempre apresentando novas tecnologias. </font></b></div><br><br><br>");
        str.Append("<div><font face='Tahoma' size='2'> E-mail automático. Não responder essa mensagem.</font></div>");
        str.Append("<html><body>");

        email.Subject = "Ordem de Serviço - " + OS.Trim();
        email.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");
        email.IsBodyHtml = true;
        email.Body = str.ToString();

        NetworkCredential Credendical = new NetworkCredential("pedidoweb@hlp.com.br", "hlpmudar");
        Cliente.Credentials = Credendical;

        try
        {
            Cliente.Send(email);
        }
        catch
        {
            return false;
        }

        return true;
    }
    
}
