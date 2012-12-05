using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HLP.Web;
using System.Web.SessionState;
using HLP.Geral;
using System.Data;
using System.Web.Configuration;

namespace HLP.Dados.Cadastro.Web
{
    public class ClienteDAOWeb : CliforDAO
    {

        public ClienteDAOWeb(Tabela oTabelas)
            : base(oTabelas)
        {
        }

        public static ClienteDAOWeb GetInstanciaClienteDAOWeb(HttpSessionState Session,
            UsuarioWeb objUsuario)
        {
            ClienteDAOWeb objCliente = (ClienteDAOWeb)Session["ObjetoClienteDetalhado"];
            if (objCliente == null)
            {
                objCliente = new ClienteDAOWeb(objUsuario.oTabelas);
                Session["ObjetoClienteDetalhado"] = objCliente;
            }
            return objCliente;
        }

        protected override bool Valid(ITela oCampo)
        {
            return true;
        }

        protected override bool When(ITela oCampo)
        {
            return true;
        }

        protected override void GerarNovoRegistro()
        {
            string sCdClifor =
                oTabelas.FuncoesBanco.GeraNovoRegistroCliforWeb(
                    oTabelas.CdVendedorAtual.ToString());
            RegistroAtual["CD_CLIFOR"] = sCdClifor;

            DataTable qryClifor = oTabelas.hlpDbFuncoes.qrySeekRet(
                "CLIFOR", "CD_ALTER, CD_VEND1",
                "(CD_CLIFOR = '" + sCdClifor + "')");
            RegistroAtual["CD_ALTER"] = qryClifor.Rows[0]["CD_ALTER"].ToString();
            RegistroAtual["CD_VEND1"] = qryClifor.Rows[0]["CD_VEND1"].ToString();

            PreencherListaValoresPrimarios();
        }


        //protected override bool PrevalidaCad()
        //{
        //    bool bRetorno = base.PrevalidaCad();
        //    if (bRetorno)
        //    {
        //        RegistroAtual["CD_CATEGO"] =
        //            WebConfigurationManager.AppSettings["CdCategoriaDefaultCliente"];
        //        if (EstruturaDataTable.Columns.Contains("ST_LIBERADO_TOTALMENTE"))
        //            RegistroAtual["ST_LIBERADO_TOTALMENTE"] = "A";
        //    }
        //    return bRetorno;
        //}

    }
}
