using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HLP.Dados;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Configuration;
using System.Data;
using HLP.Geral;
using System.Web.UI;

namespace HLP.Web
{
    public class UsuarioWeb
    {
        private Tabela objTabela;
        private StatusUsuarioWeb vStatus = StatusUsuarioWeb.NaoAutenticado;
        private string sCodigoUsuario;
        private string strGrupos = "geral";
        private string sNomeUsuario;
        private string sCodigoVendedor;

        public Tabela oTabelas
        {
            get
            {
                return objTabela;
            }
        }

        public StatusUsuarioWeb Status
        {
            get
            {
                return vStatus;
            }
        }

        public string CodigoUsuario
        {
            get
            {
                return sCodigoUsuario;
            }
        }

        public string NomeUsuario
        {
            get
            {
                return sNomeUsuario;
            }
        }

        public string CodigoVendedor
        {
            get
            {
                return sCodigoVendedor;
            }
        }

        public UsuarioWeb()
        {
            objTabela = new TabelaWeb();
            objTabela.Inicializar(true);
        }

        public static void AutenticarRequisicao()
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                        FormsAuthenticationTicket ticket = id.Ticket;
                        string userData = ticket.UserData;
                        string[] userRoles = userData.Split(new char[] { ',' });
                        HttpContext.Current.User =
                            new System.Security.Principal.GenericPrincipal(id, userRoles);
                    }
                }
            }
        }

        public static bool AutenticacaoJaEfetuada(HttpSessionState Sessao)
        {
            UsuarioWeb usuario = (UsuarioWeb)Sessao["ObjetoUsuario"];
            bool bAutenticado = (usuario != null);
            if (bAutenticado)
                bAutenticado = (usuario.Status == StatusUsuarioWeb.Autenticado);
            return bAutenticado;
        }

        public bool VerificarUsuarioSenha(HttpSessionState Sessao,
            HttpResponse Response, HlpControleLogin controle)
        {
            string sUsuario = controle.GetUsuario();
            string sSenha = controle.GetSenha();
            if (SenhaValida(sUsuario, Criptografia.Encripta(sSenha.ToUpper())))
            {
                this.sCodigoUsuario = sUsuario;
                this.vStatus = StatusUsuarioWeb.Autenticado;
                objTabela.CdUsuarioAtual =
                    WebConfigurationManager.AppSettings["UsuarioDefault"];
                objTabela.CdUsuarioGestor = "";
                objTabela.CdVendedorAtual = sCodigoVendedor;
                CriaCookieSessao(Response);
                return true;
            }
            else
            {
                this.vStatus = StatusUsuarioWeb.NaoAutenticado;
            }
            return false;
        }

        public bool VerificarUsuarioSenhaAdministrativo(HttpSessionState Sessao,
           HttpResponse Response, HlpControleLogin controle)
        {
            string sUsuario = controle.GetUsuario();
            string sSenha = controle.GetSenha();
            if (SenhaValidaAdministrativo(sUsuario, Criptografia.Encripta(sSenha)))
            {
                this.sCodigoUsuario = sUsuario;
                this.vStatus = StatusUsuarioWeb.Autenticado;
                objTabela.CdUsuarioGestor =
                    WebConfigurationManager.AppSettings["UsuarioDefault"];
                objTabela.CdUsuarioAtual = "";
                objTabela.CdVendedorAtual = sCodigoVendedor;
                CriaCookieSessao(Response);
                return true;
            }
            else
            {
                this.vStatus = StatusUsuarioWeb.NaoAutenticado;
            }
            return false;
        }

        public bool PermissaoAcessoWeb(HlpControleLogin controle)
        {
            bool bPermitido = false;
            string Count =
                oTabelas.hlpDbFuncoes.qrySeekValue("ACESSOEM",
                "COUNT(*) ",
                "CD_OPERADO = '" + controle.GetUsuario().PadLeft(10, '0').ToString().ToUpper() + "' AND CD_EMPRESA = '" + HttpContext.Current.Session["Empresa"] + "'");

            if (Convert.ToInt32(Count) > 0)
            {
                bPermitido = true;
            }
            else
            {
                bPermitido = false;
            }
            return bPermitido;
        }

        private void CriaCookieSessao(HttpResponse Response)
        {
            FormsAuthentication.Initialize();
            int iTimeOut =
                Convert.ToInt32(WebConfigurationManager.AppSettings["TimeOut"]);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1, this.sCodigoUsuario,
                DateTime.Now,
                DateTime.Now.AddMinutes(iTimeOut),
                true,
                strGrupos,
                FormsAuthentication.FormsCookiePath);
            string hash = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
            Response.Cookies.Add(cookie);
        }

        private bool SenhaValida(string sUsuario, string sSenha)
        {
            bool bSenhaValida = false;
            DataTable qryAcessoWeb =
                oTabelas.hlpDbFuncoes.qrySeekRet("ACESSO A JOIN VENDEDOR V ON A.CD_VEND= V.CD_VEND ",
                "COALESCE(A.CD_SENHA,'')CD_SENHA, A.NM_OPERADO, COALESCE(V.NM_VEND,'')NM_VEND, V.CD_VEND",
                "A.CD_OPERADO = '" + sUsuario.PadLeft(10, '0').ToString().ToUpper() + "' AND COALESCE(A.CD_SENHA,'') = '" + sSenha.ToString() + "' AND COALESCE(A.TP_OPERADO,'') ='WEB' ");

            if (qryAcessoWeb.Rows.Count > 0)
            {
                DataRow registro = qryAcessoWeb.Rows[0];
                bSenhaValida = true;
                sNomeUsuario = (string)registro["NM_VEND"];
                sCodigoVendedor = registro["CD_VEND"].ToString();
            }
            return bSenhaValida;
        }

        private bool SenhaValidaAdministrativo(string sUsuario, string sSenha)
        {
            bool bSenhaValida = false;
            DataTable qryAcessoWeb =
                oTabelas.hlpDbFuncoes.qrySeekRet("ACESSO",
                "COALESCE(CD_SENHA,'')CD_SENHA, CD_OPERADO, NM_OPERADO",
                "CD_OPERADO = '" + sUsuario.PadLeft(10, '0').ToString().ToUpper() + "' AND COALESCE(CD_SENHA,'') = '" + sSenha.ToString().ToUpper() + "' AND TP_OPERADO !='WEB' ");

            if (qryAcessoWeb.Rows.Count > 0)
            {
                DataRow registro = qryAcessoWeb.Rows[0];
                bSenhaValida = true;
                sNomeUsuario = (string)registro["NM_OPERADO"];
                sCodigoVendedor = registro["CD_OPERADO"].ToString();
            }
            return bSenhaValida;
        }

        public static string GetNomeUsuarioConectado(HttpSessionState Session)
        {

            UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
            string sNome = String.Empty;
            if (objUsuario != null)
            {
                if (objUsuario.oTabelas.CdUsuarioAtual != "")
                {
                    sNome = objUsuario.NomeUsuario;
                }
            }
            return sNome;
        }

        public static string GetNomeUsuarioGestorConectado(HttpSessionState Session)
        {

            UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
            string sNome = String.Empty;
            if (objUsuario != null)
            {
                if (objUsuario.oTabelas.CdUsuarioGestor != "")
                {
                    sNome = objUsuario.NomeUsuario;
                }
            }
            return sNome;
        }

        public static string Encripta(string strTexto)
        {
            char[] arraysenha = strTexto.ToCharArray();
            for (int i = 0; i < arraysenha.Length; i++)
            {
                int digito = (arraysenha[i] + 125 + i + 1);
                arraysenha[i] = (char)digito;
            }
            string strResultado = new string(arraysenha);
            return strResultado;
        }
    }
}
