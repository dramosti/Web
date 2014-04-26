using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HLP.Web;

public partial class Ger_SelecionaGrafico : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string sUser = UsuarioWeb.GetNomeUsuarioGestorConectado(Session);
            if (sUser == "")
            {
                Response.Redirect("~/Login.aspx");
            }
        }
    }
    protected void btnSair_Click(object sender, EventArgs e)
    {
        Session["ObjetoUsuario"] = null;
        Page.Response.Redirect("~/Login.aspx");
    }
}