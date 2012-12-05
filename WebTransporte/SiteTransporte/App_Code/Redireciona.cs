using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

/// <summary>
/// Summary description for Redireciona
/// </summary>
public static class Redireciona
{
    public static void RedirecionaPaginaInicial(int MinutosDuracaoSessao,
       HttpResponse Response, string ID)
    {
        FormsAuthentication.Initialize();
        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
            1, ID,
            DateTime.Now,
            DateTime.Now.AddMinutes(MinutosDuracaoSessao),
            true,
            FormsAuthentication.FormsCookiePath);
        string hash = FormsAuthentication.Encrypt(ticket);
        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
        Response.Cookies.Add(cookie);
        Response.Redirect(PaginasAplicacao.GetPaginaInicial());
    }
}