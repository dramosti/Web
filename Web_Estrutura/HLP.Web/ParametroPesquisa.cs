using System;
using System.Text;
using System.Web.SessionState;

namespace HLP.Web
{
    public class ParametroPesquisa
    {


        protected StringBuilder strWhere = new StringBuilder();
        private StringBuilder strHaving = new StringBuilder();
        private bool bAindaNaoDefiniuFiltro = true;

        protected ParametroPesquisa()
        {
        }

        public virtual void Limpar()
        {
            bAindaNaoDefiniuFiltro = true;
            if (strWhere.Length > 0)
                strWhere.Length = 0;
            if (strHaving.Length > 0)
                strHaving.Length = 0;
        }

        public void AddCriterio(string sCriterio)
        {
            bAindaNaoDefiniuFiltro = false;
            if (strWhere.Length > 0)
                strWhere.Append(" AND ");
            strWhere.Append(sCriterio);
        }

        public void AddCriterioHaving(string sCriterio)
        {
            if (strHaving.Length > 0)
                strHaving.Append(" AND ");
            strHaving.Append(sCriterio);
        }

        public string GetWhere()
        {
            return strWhere.ToString();
        }

        public string GetHaving()
        {
            return strHaving.ToString();
        }

        public bool AindaNaoDefiniuFiltro()
        {
            return bAindaNaoDefiniuFiltro;
        }

        public static void InicializarParametro(string sNomeParametro,
            HttpSessionState Session)
        {
            if (Session[sNomeParametro] != null)
            {
                ParametroPesquisa objParametro =
                    (ParametroPesquisa)Session[sNomeParametro];
                objParametro.Limpar();
            }
            else
                Session[sNomeParametro] = new ParametroPesquisa();
        }

    }

}
