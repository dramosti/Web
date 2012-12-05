using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.CompilerServices;
using System.Configuration;
using System.Web.SessionState;
using System.Web.Security;


/// <summary>
/// Summary description for PaginasAplicacao
/// </summary>
public sealed class PaginasAplicacao
{
    private static string PaginaInicial = null;
    private static string strImagemExclusao = null;
    private static string strImagemAlteracao = null;

    private PaginasAplicacao()
    {
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static string GetPaginaInicial()
    {
        if (PaginaInicial == null)
        {
            try
            {
                PaginaInicial = ConfigurationManager.AppSettings["PaginaInicial"];
            }
            catch
            {
                PaginaInicial = "Login.aspx";
            }
        }
        return PaginaInicial;
    }

    public static void Logoff(HttpResponse Response, HttpSessionState Session)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Session.Clear();
        FormsAuthentication.SignOut();
    }

}