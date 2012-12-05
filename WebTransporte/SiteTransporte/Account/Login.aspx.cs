using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;

public partial class Account_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Login ::: GCA Transportes :::";
        this.LoginUser.Focus();
    }
    private int intMinutosDuracaoSessao;
    public int MinutosDuracaoSessao
    {
        get
        {
            if (intMinutosDuracaoSessao != 0)
                return intMinutosDuracaoSessao;
            else
                return 15;
        }
        set
        {
            intMinutosDuracaoSessao = value;
        }
    }

    protected void Login_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            belUsuario objbelUser = new belUsuario(this.LoginUser.UserName, this.LoginUser.Password);
            if (objbelUser.ValidaUser_Senha())
            {
                (Session["Usuario"]) = objbelUser;
                DAO.daoStatic.CD_CLIFOR = objbelUser.CD_CLIFOR;
                Response.Clear();
                Redireciona.RedirecionaPaginaInicial(15, Response, DAO.daoStatic.EMP_NOME_CGC);
            }
            else
            {
                lblErro.Text = "Não foi possível se logar com esse Usuário e Senha!";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
