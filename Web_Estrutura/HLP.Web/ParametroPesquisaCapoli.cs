using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;

namespace HLP.Web
{
    public class ParametroPesquisaCapoli : ParametroPesquisa
    {
        private string sVendedorDefault;
        private string sTabelaFiltrada;

        public override void Limpar()
        {
            base.Limpar();
            if (sTabelaFiltrada.Equals("CLIFOR"))
            {
                strWhere.Append("((CLIFOR.ST_INATIVO <> 'S') ");
                strWhere.Append("OR (CLIFOR.ST_INATIVO IS NULL)) AND ");
            }
            strWhere.Append("(" + sTabelaFiltrada + ".CD_VEND1 = '" + sVendedorDefault + "')");
        }

        public static void InicializarParametroPesquisa(string sNomeParametro,
            string sTabelaFiltrada, HttpSessionState Session)
        {
            ParametroPesquisaCapoli objParametro;
            if (Session[sNomeParametro] != null)
            {
                objParametro =
                    (ParametroPesquisaCapoli)Session[sNomeParametro];
                objParametro.Limpar();
            }
            else
            {
                objParametro = new ParametroPesquisaCapoli();
                Session[sNomeParametro] = objParametro;
            }
            objParametro.sVendedorDefault =
                ((UsuarioWeb)Session["ObjetoUsuario"]).CodigoVendedor;
            objParametro.sTabelaFiltrada = sTabelaFiltrada;
        }


    }

}
