using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HLP.Dados;
using System.Web.Configuration;

namespace HLP.Web
{
    public class TabelaWeb : Tabela
    {
        public override void CarregarPropriedadesAcesso()
        {
            this.sEmpresa = WebConfigurationManager.AppSettings["EmpresaDefault"];
            this.MS_DRIVER = WebConfigurationManager.AppSettings["MS_DRIVER"];
            this.MS_DATABASENAME = WebConfigurationManager.AppSettings["MS_DATABASENAME"];
            this.MS_SERVERNAME = WebConfigurationManager.AppSettings["MS_SERVERNAME"];
            this.MS_USERNAME = WebConfigurationManager.AppSettings["MS_USERNAME"];
            this.MS_SENHA = WebConfigurationManager.AppSettings["MS_SENHA"];
            // Inicio OS - 0020753.
            this.TRANSP = WebConfigurationManager.AppSettings["TransportadoraDefault"];
            // Fim OS - 0020753.
        }

        public override string GetOperacaoDefault(string sCdTipoDoc)
        {
            return WebConfigurationManager.AppSettings["CdOperPedidoDefault"];
        }

        public override PlataformaUtilizada GetPlataformaUtilizada()
        {
            return PlataformaUtilizada.Web;
        }

    }
}
