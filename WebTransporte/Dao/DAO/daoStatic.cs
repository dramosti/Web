using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace DAO
{
    public static class daoStatic
    {
        public static string sConn { get { return ConfigurationManager.ConnectionStrings["ConnectionStringFB"].ConnectionString.ToString(); } }

        public static string CD_CLIFOR { get; set; }

        public static string EMP_NOME_CGC { get; set; }

        public static string CD_EMPRESA { get; set; }
    }
}
