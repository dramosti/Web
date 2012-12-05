﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HLP.Web;
using System.Data;

public partial class Ger_Site : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string sUser = UsuarioWeb.GetNomeUsuarioGestorConectado(Session);
            UsuarioWeb objUsuario = new UsuarioWeb();
            DataTable dtDadosEmpresa = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("EMPRESA", "nm_empresa,nm_guerra", "CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa.Trim() + "'");


            foreach (DataRow row in dtDadosEmpresa.Rows)
            {

                lblNmFantasia.Text = "Módulo de Vendas - " + row["nm_guerra"].ToString();
                lblNmEmpresa.Text = row["nm_empresa"].ToString();
            }


            if (sUser != "")
            {
                string sMessage = string.Empty;
                int iHour = DateTime.Now.Hour;
                if (iHour >= 0 && iHour <= 12)
                {
                    sMessage = "Bom dia {0}";
                }
                else if (iHour > 12 && iHour <= 18)
                {
                    sMessage = "Boa tarde {0}";
                }
                else
                {
                    sMessage = "Boa noite {0}";
                }

                lblRepres.Text = string.Format(sMessage, sUser);
            }
            else
            {
                lblRepres.Text = "";
            }
        }

    }
    
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        try
        {
            Session["ObjetoUsuario"] = null;
            Page.Response.Redirect("~/Login.aspx");
        }
        catch (Exception ex)
        {
            MessageHLP.ShowPopUpMsg(ex.Message, this.Page);
        }
    }
}
