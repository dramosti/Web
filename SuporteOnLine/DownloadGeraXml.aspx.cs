using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;

public partial class DownloadGeraXml : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            object sVersaoCli = Request["Versao"];

            if (sVersaoCli != null)
            {
                lblVersaoCli.Text = string.Format("Sua versão é : {0}", Convert.ToString(sVersaoCli));
            }
            CarregaDados();
        }
    }

    void CarregaDados()
    {
        try
        {
            string sPath = "";
            if (cbxPrograma.SelectedIndex == 0)
            {
                sPath = ConfigurationManager.AppSettings["DownloadGera"].ToString();
            }
            else
            {
                sPath = ConfigurationManager.AppSettings["DownloadSPED"].ToString();
            }
            DirectoryInfo dinfo = new DirectoryInfo(sPath);
            DirectoryInfo[] diretorios;
            if (cbxTpExe.SelectedIndex == 0)
            {
                diretorios = dinfo.GetDirectories().Where(c => !c.Name.ToUpper().Contains("TESTE")).OrderByDescending(c => c.LastWriteTime).ToArray();
            }
            else
            {
                diretorios = dinfo.GetDirectories().Where(c => c.Name.ToUpper().Contains("TESTE")).OrderByDescending(c => c.LastWriteTime).ToArray();
            }
            List<Versoes> lVersoes = new List<Versoes>();
            Versoes versao;
            int iCountID = 1;
            foreach (DirectoryInfo pasta in diretorios)
            {
                versao = new Versoes();
                versao.id = iCountID;
                versao.Versao = pasta.Name;
                string sFileZip = pasta.Name.Replace("_TESTE", "");
                versao.Path = pasta.FullName + "\\" + sFileZip + ".zip";
                if (File.Exists(versao.Path))
                {
                    versao.Data = File.GetCreationTime(versao.Path);
                    string PathDetalhe = pasta.FullName + "\\" + "Detalhes.txt";
                    if (File.Exists(PathDetalhe))
                    {
                        StreamReader sr = File.OpenText(PathDetalhe);
                        versao.Detalhes = sr.ReadToEnd();
                    }
                    lVersoes.Add(versao);
                    iCountID++;
                }
            }
            Session["Versoes"] = lVersoes;
            gvVersoes.DataSource = lVersoes;
            gvVersoes.DataBind();


        }
        catch (Exception ex)
        {
            HlpMessageBox.ShowPopUpMsg(ex.Message, this);
        }



    }
    protected void gvVersoes_SelectedIndexChanged(object sender, EventArgs e)
    {
        int id = Convert.ToInt32(gvVersoes.SelectedDataKey[0].ToString());
        if (((List<Versoes>)Session["Versoes"]).Where(c => c.id == id).Count() > 0)
        {
            HlpMessageBox.ShowPopUpMsg(((List<Versoes>)Session["Versoes"]).FirstOrDefault(c => c.id == id).Detalhes, this);
        }
        else
        {
            //Mensagem
        }
    }
    protected void gvVersoes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Baixar")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvVersoes.Rows[index];
            int id = Convert.ToInt32(row.Cells[1].Text);

            Versoes versao = ((List<Versoes>)Session["Versoes"]).FirstOrDefault(c => c.id == id);

            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}.zip", versao.Versao));
            Response.TransmitFile(versao.Path);
            Response.End();
        }
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        LinkButton link = (LinkButton)sender;
        string sPath = "";
        string sArquivo = "";
        if (link.ID.Equals("linkAcesso"))
        {
            sPath = @"C:\GeraXml\Pre requisitos\TeamViewer\TeamViewer_Pot.rar";
            sArquivo = "TeamViewer_Pot.rar";
        }
        else if (link.ID.Equals("linkFrameWork"))
        {
            sPath = @"C:\GeraXml\Pre requisitos\Framework 4.0\dotNetFx40_Full_x86_x64.exe";
            sArquivo = "dotNetFx40_Full_x86_x64.exe";
        }
        else if (link.ID.Equals("linkCrystal32"))
        {
            sPath = @"C:\GeraXml\Pre requisitos\Crystal\Crystal_2010\CRRuntime_32bit_13_0_2.rar";
            sArquivo = "CRRuntime_32bit_13_0_2.rar";
        }
        else
        {
            sPath = @"C:\GeraXml\Pre requisitos\Crystal\Crystal_2010\CRRuntime_64bit_13_0_2.rar";
            sArquivo = "CRRuntime_64bit_13_0_2.msi";
        }
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", sArquivo));
        Response.TransmitFile(sPath);
        Response.End();

    }
    protected void gvVersoes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvVersoes.PageIndex = e.NewPageIndex;
        gvVersoes.DataSource = Session["Versoes"] as List<Versoes>;
        gvVersoes.DataBind();
    }

    protected void btnCarregar_Click(object sender, EventArgs e)
    {
        CarregaDados();

    }

}