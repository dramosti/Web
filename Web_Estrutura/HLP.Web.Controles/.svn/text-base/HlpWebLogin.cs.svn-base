using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using HLP.Dados;

namespace HLP.Web.Controles
{
    public class HlpWebLogin : Login, HlpControleLogin
    {

        #region HlpControleLogin Members

        public string GetUsuario()
        {
            return this.UserName.Trim();
        }

        public string GetSenha()
        {
            return this.Password;
        }

        #endregion

        public void ExecutarPageLoad()
        {
            string sLogoff = (string)Page.Request["logoff"];
            if ((sLogoff != null) && (sLogoff.Trim().ToLower().Equals("true")))
            {
                EfetuarLogoff();
                this.Page.Response.Redirect(this.DestinationPageUrl);
                return;
            }
            if (!UsuarioWeb.AutenticacaoJaEfetuada(this.Page.Session))
            {
                this.TextBoxStyle.Font.Size = FontUnit.Small;
                this.TextBoxStyle.Width = 180;
            }
            else
            {
                this.Page.Response.Redirect(this.DestinationPageUrl);
            }
        }

        public void EfetuarLogoff()
        {
            Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Page.Session.Clear();
            FormsAuthentication.SignOut();
        }

        public void ExecutarAuthenticate(AuthenticateEventArgs e)
        {
            HttpSessionState Session = this.Page.Session;
            UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
            if (objUsuario == null)
            {
                objUsuario = new UsuarioWeb();
                Session["ObjetoUsuario"] = objUsuario;
            }
            objUsuario.VerificarUsuarioSenha(Page.Session, Page.Response, this);

            bool bAutenticado = (objUsuario.Status == StatusUsuarioWeb.Autenticado);

            if (!bAutenticado)
            {
                objUsuario.VerificarUsuarioSenhaAdministrativo(Page.Session, Page.Response, this);
                bAutenticado = (objUsuario.Status == StatusUsuarioWeb.Autenticado);
                e.Authenticated = bAutenticado;
                Session["ObjetoUsuario"] = objUsuario;
                if (!objUsuario.PermissaoAcessoWeb(this))
                {
                    bAutenticado = false;
                    Session["ObjetoUsuario"] = null;
                }
                if (bAutenticado)
                {
                    objUsuario.oTabelas.sEmpresa = Session["Empresa"].ToString();
                    Page.Response.Redirect("Ger_SelecionaGrafico.aspx");
                }
            }
            if (!objUsuario.PermissaoAcessoWeb(this))
            {
                bAutenticado = false;
                Session["ObjetoUsuario"] = null;
            }
            e.Authenticated = bAutenticado;
            if (bAutenticado)
            {
                objUsuario.oTabelas.sEmpresa = Session["Empresa"].ToString();
                Page.Response.Redirect(this.DestinationPageUrl);
            }
        }
    }
}
