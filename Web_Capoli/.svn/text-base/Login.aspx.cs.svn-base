using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;
using HLP.Web;
using System.Configuration;
using FirebirdSql.Data.FirebirdClient;

public partial class Account_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["SAIR"] != null)
            {
                if (Request["SAIR"].ToString().Equals("TRUE"))
                {
                    Session["ObjetoUsuario"] = null;
                }
            }
        }

        Page.Form.DefaultButton = this.ControleLogin.FindControl("LoginButton").UniqueID;
        if (!IsPostBack)
        {
            GetEmpresa();
        }
    }


    protected void HlpWebLogin1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        UsuarioWeb objUsuario = new UsuarioWeb();
        string sTotal = "¯¯";

        int iCount = Convert.ToInt32(objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("ACESSO", "COUNT(*)", "TP_OPERADO = 'WEB'"));
        int iTotal = Convert.ToInt32(HLP.Web.Criptografia.Decripta(sTotal));
        if (iCount > iTotal)
        {
            MessageHLP.ShowPopUpMsg("O número de usuários cadastrados do tipo 'WEB' excede o limite de licenças." + Environment.NewLine +
            "Entre em contato com o administrador do sistema.", this.Page);
        }
        else
        {
            Session["Empresa"] = cboEmpresa.SelectedValue;
            this.ControleLogin.ExecutarAuthenticate(e);
        }
    }

    protected void LoginButton_Click(object sender, EventArgs e)
    {

    }

    private void GetEmpresa()
    {
        string strConn = ConfigurationManager.ConnectionStrings["ConnectionStringFB"].ConnectionString.ToString();

        FbConnection Conn = new FbConnection(strConn);
        try
        {
            FbCommand cmd = new FbCommand();
            cmd.CommandText = "SELECT  (CD_EMPRESA || ' - ' || NM_GUERRA  ) AS NM_GUERRA  , CD_EMPRESA FROM EMPRESA";
            cmd.Connection = Conn;
            Conn.Open();

            cboEmpresa.DataSource = cmd.ExecuteReader();
            cboEmpresa.DataBind();
        }
        catch (Exception EX)
        {
            throw EX;
        }
        finally
        {
            Conn.Close();
        }


    }
}
