using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using DAO;


namespace Business
{
    public class belUsuario : daoUsuario
    {

        public belUsuario(string sUser, string sSenha)
        {
            CD_CGC  = sUser;
            NM_SENHA = sSenha;       
        }    
    }
}
