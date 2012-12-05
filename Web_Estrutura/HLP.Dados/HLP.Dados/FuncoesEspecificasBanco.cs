using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using HLP.Geral;

namespace HLP.Dados
{
    public abstract class FuncoesEspecificasBanco
    {
        private Dictionary<string, bool> ListaCamposTabelas = null;

        public abstract string GetStringConexao(string Servidor, string BancoDados,
            string Usuario, string Senha, string sPort);
        private string sDriverBaseDados;
        private string sClasseConexaoBaseDados;
        private string sClasseDbCommandBaseDados;
        private string sClasseDbDataAdapterBaseDados;
        protected Tabela oTabelas;

        public FuncoesEspecificasBanco()
        {
            InicializaListasTabelasCampos();
            InicializaInformacoesClassesConexao();
        }

        private void InicializaListasTabelasCampos()
        {
            if (ListaCamposTabelas == null)
                ListaCamposTabelas = new Dictionary<string, bool>();
            else
                ListaCamposTabelas.Clear();
        }

        public void SetObjetoTabelas(Tabela oTabelas)
        {
            this.oTabelas = oTabelas;
        }

        public abstract string GetDriverBaseDados();
        public abstract string GetClasseConexaoBaseDados();
        public abstract string GetClasseDbCommandBaseDados();
        public abstract string GetClasseDbDataAdapterBaseDados();
        public abstract string GeraNovoRegistroPedidoWeb(string sCdVend1);
        public abstract string GeraNovoRegistroCliforWeb(string sCdVend1);

        private void InicializaInformacoesClassesConexao()
        {
            sDriverBaseDados = this.GetDriverBaseDados();
            sClasseConexaoBaseDados = this.GetClasseConexaoBaseDados();
            sClasseDbCommandBaseDados = this.GetClasseDbCommandBaseDados();
            sClasseDbDataAdapterBaseDados = this.GetClasseDbDataAdapterBaseDados();

            /*
            DataTable tabConfig = HlpDbFuncoesGeral.GetTabelaXml(sArquivoXmlConfiguracao,
                this.GetNomeItensConfiguracao());
            string schave, svalor;
            foreach (DataRow conf in tabConfig.Rows)
            {
                schave = conf["parametro"].ToString().Trim();
                if (schave.Equals(String.Empty))
                    continue;

                svalor = conf["valor"].ToString().Trim();
                switch (schave)
                {
                    case "DriverBaseDados":
                        sDriverBaseDados = svalor;
                        break;
                    case "ClasseConexaoBaseDados":
                        sClasseConexaoBaseDados = svalor;
                        break;
                    case "ClasseDbCommandBaseDados":
                        sClasseDbCommandBaseDados = svalor;
                        break;
                    case "ClasseDbDataAdapterBaseDados":
                        sClasseDbDataAdapterBaseDados = svalor;
                        break;
                }
            }*/
        }

        public void LimpaInfoCamposTabelasExistentes()
        {
            ListaCamposTabelas.Clear();
        }

        protected void PreencheInfoCamposTabelasExistentes(string schave, bool bExiste)
        {
            if (!ListaCamposTabelas.ContainsKey(schave))
                ListaCamposTabelas.Add(schave, bExiste);
        }

        public bool ExisteTabela(string sTabela, IDbConnection conexao)
        {
            Nullable<bool> bExiste = null;
            if (ListaCamposTabelas.ContainsKey(sTabela))
                bExiste = ListaCamposTabelas[sTabela];
            if (bExiste == null)
            {
                bExiste = ExisteTabelaBancoEspecifico(sTabela, conexao);
                PreencheInfoCamposTabelasExistentes(sTabela, bExiste.Value);
            }

            return bExiste.Value;
        }

        public bool ExisteCampo(string sTabela, string sCampo, IDbConnection conexao)
        {
            string sChave = sTabela + "->" + sCampo;
            Nullable<bool> bExiste = null;
            if (ListaCamposTabelas.ContainsKey(sChave))
                bExiste = ListaCamposTabelas[sChave];
            if (bExiste == null)
            {
                bExiste = ExisteCampoBancoEspecifico(sTabela, sCampo, conexao);
                PreencheInfoCamposTabelasExistentes(sChave, bExiste.Value);
            }

            return bExiste.Value;
        }

        protected abstract bool ExisteTabelaBancoEspecifico(string sTabela,
            IDbConnection conexao);
        protected abstract bool ExisteCampoBancoEspecifico(string sTabela,
            string sCampo, IDbConnection conexao);

        public abstract string GetExpressaoExclusaoProcedure(string sProcedure);
        public abstract string GetExpressaoCriacaoProcedure(string sProcedure, string sCorpoProcedure);
        public abstract string GetExpressaoAlteracaoProcedure(string sProcedure, string sCorpoProcedure);

        public abstract string GeraNovoRegistroMovipendWeb(string sCdPedido);

        public IDbDataAdapter GetDataAdapter()
        {
            IDbDataAdapter da = (IDbDataAdapter)HlpFuncoes.getObjeto(sDriverBaseDados,
                sClasseDbDataAdapterBaseDados);
            return da;
        }

        public IDbCommand GetDbCommand()
        {
            IDbCommand comando = (IDbCommand)HlpFuncoes.getObjeto(sDriverBaseDados,
                sClasseDbCommandBaseDados);
            return comando;
        }

    }
}
