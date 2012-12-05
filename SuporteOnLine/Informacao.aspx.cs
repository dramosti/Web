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

public partial class Informacao : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!Request.QueryString["RET"].ToString().Equals(String.Empty))
            {
                lblLancamento.Visible = false;
                lblTitulo.Visible = true;
                lblTitulo.Text = Request.QueryString["RET"].ToString();
            }
        }

    }
}
