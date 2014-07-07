using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HLP.Web;
using System.Data;
using System.IO;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Net;

public partial class ViewPedido : System.Web.UI.Page
{
    private ReportDocument rpt = new ReportDocument();


    protected void Page_Load(object sender, EventArgs e)
    {       
        if (!IsPostBack)
        {

            string sUser = UsuarioWeb.GetNomeUsuarioConectado(Session);
            if (sUser == "")
            {
                Response.Redirect("~/Home.aspx");
            }          
            string sPathAnexo = "";

            if (Request.QueryString["ANEXO"].ToString() != String.Empty)
            {
                sPathAnexo = Request.QueryString["ANEXO"].ToString();
                FileInfo finfo = new FileInfo(Server.MapPath("Pedidos//" + sPathAnexo) + ".pdf");
                if (finfo.Exists)
                {
                    sPathAnexo = finfo.FullName;
                }               
                ShowFile2(sPathAnexo);
            }

        }
    }

    private void ShowFile2(string spath)
    {
        string path = spath;
        FileInfo myDoc = new FileInfo(spath.ToString());
        WebClient client = new WebClient();
        Byte[] buffer = client.DownloadData(path);

        if (buffer != null)
        {
            Response.ContentType = ReturnExtension(myDoc.Extension);
            Response.AddHeader("content-length", buffer.Length.ToString());
            Response.BinaryWrite(buffer);

        }
    }

    private string ReturnExtension(string fileExtension)
    {
        switch (fileExtension.ToLower())
        {
            case ".pdf":
                return "application/pdf";
            case ".txt":
                return "text/plain";
            case ".gif":
                return "image/gif";
            case ".jpg":
            case "jpeg":
                return "image/jpeg";
            case ".bmp":
                return "image/bmp";
            case ".wav":
                return "audio/wav";
            case ".dwg":
                return "image/vnd.dwg";
            case ".doc":
                return "application/msword";
            case ".dot":
                return "application/msword";
            case ".docx":
                return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            case ".dotx":
                return "application/vnd.openxmlformats-officedocument.wordprocessingml.template";
            case ".docm":
                return "application/vnd.ms-word.document.macroEnabled.12";
            case ".dotm":
                return "application/vnd.ms-word.template.macroEnabled.12";
            case ".xls":
            case ".xlt":
            case ".xla":
                return "application/vnd.ms-excel";
            case ".xlsx":
                return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            case ".xltx":
                return "application/vnd.openxmlformats-officedocument.spreadsheetml.template";
            case ".xlsm":
                return "application/vnd.ms-excel.sheet.macroEnabled.12";
            case ".xltm":
                return "application/vnd.ms-excel.template.macroEnabled.12";
            case ".xlam":
                return "application/vnd.ms-excel.addin.macroEnabled.12";
            case ".xlsb":
                return "application/vnd.ms-excel.sheet.binary.macroEnabled.12";
            case ".ppt":
            case ".pot":
            case ".pps":
            case ".ppa":
                return "application/vnd.ms-powerpoint";
            case ".pptx":
                return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
            case ".potx":
                return "application/vnd.openxmlformats-officedocument.presentationml.template";
            case ".ppsx":
                return "application/vnd.openxmlformats-officedocument.presentationml.slideshow";
            case ".ppam":
                return "application/vnd.ms-powerpoint.addin.macroEnabled.12";
            case ".pptm":
                return "application/vnd.ms-powerpoint.presentation.macroEnabled.12";
            case ".potm":
                return "application/vnd.ms-powerpoint.template.macroEnabled.12";
            case ".ppsm":
                return "application/vnd.ms-powerpoint.slideshow.macroEnabled.12";
            default:
                return "application/octet-stream";
        }
    }

}