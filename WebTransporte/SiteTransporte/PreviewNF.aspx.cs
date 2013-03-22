using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;

public partial class PreviewNF : System.Web.UI.Page
{
    private rpt_GCA_NF rpt = null;

    protected void Page_Load(object sender, EventArgs e)
    {
      
        if (!IsPostBack)
        {

        }
    }
    protected void btnImprimi_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    Convert.ToDateTime(txtDtEmissao.Text);
        //}
        //catch (Exception)
        //{
        //    txtDtEmissao.Text = "";
        //}

        //if (!txtDtEmissao.Text.Equals(""))
        //{
        //    boRelNfFrete obj = new boRelNfFrete();
        //    Session["ListReport"] = obj.GetDadosToReport(cbxStatus.SelectedValue.ToString(), Convert.ToDateTime(txtDtEmissao.Text).ToString("dd.MM.yyyy"));

        //    rpt = new rpt_GCA_NF();
        //    rpt.SetDataSource((Session["ListReport"] as List<boRelNfFrete>));
        //    CrystalReportViewer1.ReportSource = rpt;
        //}
    }
    protected void btnImprimi0_Click(object sender, EventArgs e)
    {
        try
        {
            if (!txtDtEmissao.Text.Equals(""))
            {
                boRelNfFrete obj = new boRelNfFrete();
                Session["ListReport"] = obj.GetDadosToReport(cbxStatus.SelectedValue.ToString(), Convert.ToDateTime(txtDtEmissao.Text).ToString("dd.MM.yyyy"), Convert.ToDateTime(txtDtFinal.Text).ToString("dd.MM.yyyy"));
                Response.Redirect("~/Visualizacao.aspx");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}