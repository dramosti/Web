using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    public string backgroundImage;
    public string nmEmpresa;
    public string rua;
    public string nro;
    public string cep;
    public string cidade;
    public string uf;
    public string tel;

    protected void Page_Load(object sender, EventArgs e)
    {
        belUsuario objbelUser = (belUsuario)Session["Usuario"];
        if (objbelUser == null)
        { 
            Response.RedirectPermanent("~/Account/Login.aspx");
        }
        Page.MaintainScrollPositionOnPostBack = true;

        if (!IsPostBack)
        {
            backgroundImage = objbelUser.IMG_EMP;

            nmEmpresa = objbelUser.NM_EMPRESA;
            rua = objbelUser.LOGRADOURO;
            nro = objbelUser.NUMERO;
            cep = objbelUser.CEP;
            cidade = objbelUser.MUNICIPIO;
            uf = objbelUser.UF;
            tel = objbelUser.FONE;
            Page.Title = "Consulta de Conhecimento";
        }
    }



}
