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

public partial class ResultadoConsultaVisita : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dtResultVisita = (DataTable)Session["TabelaVisita"];
        //Session["TabelaVisita"] = null;
        GridView1.DataSource = dtResultVisita.DefaultView;
        GridView1.DataBind();

    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["IdRelatorio"] = GridView1.SelectedRow.Cells[1].Text;
    }
    
    protected void Button1_Click(object sender, EventArgs e)
    {
        Session["IdRelatorio"] = null;
        Response.Redirect("~/CadastroRelatorioVisita.aspx?IdRel=incluir");
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        string strIdRel = null;
        strIdRel = (string)Session["IdRelatorio"];
        if ((strIdRel != null))
        {
            Session["IdRelatorio"] = null;
            Response.Redirect("~/Confirmacao.aspx?ID=" + strIdRel + ",E");
        }
        else
        {
            Response.Redirect("~/Informacao.aspx?RET=Não foi encontrado os dados solicitados!");
        }
    }
    protected void btnImp_Click(object sender, EventArgs e)
    {
        string NR_LANC = null;
        NR_LANC = (string)Session["IdRelatorio"];
        if ((NR_LANC != null))
        {
            Session["IdRelatorio"] = null;
            Response.Redirect("~/VisualizarRelatorioVisita.aspx?ID=" + NR_LANC);
        }
        else
        {
            Response.Redirect("~/Informacao.aspx?RET=Não foi encontrado os dados solicitados!");
        }
    }
}
