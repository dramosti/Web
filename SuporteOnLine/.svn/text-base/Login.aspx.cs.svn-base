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
using System.Data.SqlClient;
using System.Text;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((Request.QueryString["Acao"] != null) &&
            (Request.QueryString["Acao"].ToString() == "Sair"))
        {
            if (Session["Grupo"] != null)
            {
                Session["Grupo"] = null;
            }
            if (Session["CodigoUsuario"] != null)
            {
                Session["CodigoUsuario"] = null;
            }
            if (Session["Usuario"] != null)
            {
                Session["Usuario"] = null;
            }
            Response.Redirect("~/Login.aspx");
        }

        if (TextBox1.Text.Trim() == String.Empty)
        {
            TextBox1.Focus();
        }
        else
        {
            TextBox2.Focus();
        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        bool bExisteUsuario = false;

        StringBuilder strSelect = new StringBuilder();

        strSelect.Append("SELECT CD_OPERADO, CD_SENHA, DS_GRUPO, NM_OPERADO ");
        strSelect.Append("FROM ACESSO ");
        strSelect.Append("WHERE (CD_OPERADO = '" + HlpBancoDados.GetUsuario(TextBox1.Text.ToUpper()) + "')");

        FbCommand fbSelect = (FbCommand)HlpBancoDados.CommandSelect(strSelect.ToString());

        fbSelect.Connection.Open();

        FbDataReader drUsuario = fbSelect.ExecuteReader();

        bExisteUsuario = drUsuario.Read();

        if (bExisteUsuario)
        {
            string sSenhaDB = Criptografia.Decripta(drUsuario["CD_SENHA"].ToString()).ToLower();
            if (!TextBox2.Text.ToLower().Equals(sSenhaDB))
                bExisteUsuario = false;
            string sGrupo = String.Empty;
            if (drUsuario["DS_GRUPO"] != null)
                sGrupo = drUsuario["DS_GRUPO"].ToString().ToUpper();
            if (sGrupo.Equals(String.Empty))
                sGrupo = "USUARIO";
            Session["Grupo"] = sGrupo;
            Session["CodigoUsuario"] = drUsuario["CD_OPERADO"].ToString();
            Session["Usuario"] = drUsuario["NM_OPERADO"].ToString();
        }

        fbSelect.Connection.Close();

        if (!bExisteUsuario)
        {
            lblMsg.Visible = true;
            lblMsg.Text = "Usuário ou senha incorreta!";
        }
        else
        {
            lblMsg.Visible = true;
            lblMsg.Text = String.Empty;
            Response.Redirect("~/Default.aspx");
        }
    }
}
