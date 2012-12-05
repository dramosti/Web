using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using HLP.Geral;
using System.Web;
using System.Web.Configuration;

namespace HLP.Dados
{
    public abstract class Tabela
    {
        private IDbConnection objConexao = null;
        private FuncoesEspecificasBanco objFuncoesBanco = null;
        private string strCodEmpresa;
        private string strDriver;
        private string strUserName;
        private string strSenha;
        private string strDataBaseName;
        private string strServidor;
        private string sNmModulo;
        private string sCodigoCliente = String.Empty;
        private string sRamoAtividade = String.Empty;
        private string sUsuarioAtual = String.Empty;
        private bool bMantemConexaoAberta = false;
        private HlpDbFuncoes objHlpDbFuncoes = null;
        private string sCdVendedorAtual;
        // Inicio OS 0020753.
        private string sTransp;
        // Fim OS 0020753.
        public Tabela()
        {
            //O CONSTRUTOR FICARÁ REALMENTE VAZIO, FORÇANDO A INVOCAÇÃO DO 
            //MÉTODO DE INICIALIZAÇÃO DESTA CLASSE.
        }

        // Inicio OS 0020753.

        public string TRANSP
        {
            get
            {
                return sTransp;
            }
            set
            {
                sTransp = value;
            }
        }

        // Fim OS 0020753.

        public IDbConnection dbsGeral
        {
            get
            {
                return GetObjetoConexao();
            }
        }

        public string sEmpresa
        {
            get
            {
                return strCodEmpresa;
            }
            set
            {
                SetEmpresa(value);
            }
        }

        private void SetEmpresa(string scodigoemp)
        {
            strCodEmpresa = scodigoemp;
        }

        public string MS_DRIVER
        {
            get
            {
                return strDriver;
            }
            set
            {
                strDriver = value;
            }
        }

        public string MS_USERNAME
        {
            get
            {
                return strUserName;
            }
            set
            {
                strUserName = value;
            }
        }

        public string MS_SENHA
        {
            get
            {
                return strSenha;
            }
            set
            {
                strSenha = value;
            }
        }

        public string MS_DATABASENAME
        {
            get
            {
                return strDataBaseName;
            }
            set
            {
                strDataBaseName = value;
            }
        }

        public string MS_SERVERNAME
        {
            get
            {
                return strServidor;
            }
            set
            {
                strServidor = value;
            }
        }

        public bool MantemConexaoAberta
        {
            get
            {
                return bMantemConexaoAberta;
            }
            set
            {
                bMantemConexaoAberta = value;
            }
        }

        public string NmModulo
        {
            get
            {
                return sNmModulo;
            }
            set
            {
                sNmModulo = value;
            }
        }

        public string CodigoCliente
        {
            get
            {
                return sCodigoCliente;
            }
            set
            {
                sCodigoCliente = value;
            }
        }

        public string RamoAtividade
        {
            get
            {
                return sRamoAtividade;
            }
            set
            {
                sRamoAtividade = value;
            }
        }
        public string CdUsuarioGestor { get; set; }

        public string CdUsuarioAtual
        {
            get
            {
                return sUsuarioAtual;
            }
            set
            {
                sUsuarioAtual = value;
            }
        }

        public string CdVendedorAtual
        {
            get
            {
                return sCdVendedorAtual;
            }
            set
            {
                sCdVendedorAtual = value;
            }
        }

        public HlpDbFuncoes hlpDbFuncoes
        {
            get
            {
                return objHlpDbFuncoes;
            }
        }

        public FuncoesEspecificasBanco FuncoesBanco
        {
            get
            {
                return objFuncoesBanco;
            }
        }

        private IDbConnection GetObjetoConexao()
        {
            return objConexao;
        }

        public abstract void CarregarPropriedadesAcesso();

        public void Inicializar(bool FecharAutomaticamenteConexao)
        {
            CarregarPropriedadesAcesso();
            this.objFuncoesBanco = ObjetoFuncoesBanco.CriaInstancia(MS_DRIVER);
            this.objFuncoesBanco.SetObjetoTabelas(this);
            this.MantemConexaoAberta = (!FecharAutomaticamenteConexao);
            ConfigurarConectarBase(MS_SERVERNAME, MS_DATABASENAME,
                this.MantemConexaoAberta);

            if (objHlpDbFuncoes == null)
                objHlpDbFuncoes = new HlpDbFuncoes(this);

            this.RamoAtividade = objHlpDbFuncoes.qrySeekValue(
                "ARQSISTE", "CD_RA", "(NM_ARQUIVO = 'ACESSO')");
            this.CodigoCliente = objHlpDbFuncoes.fMemoStr("0016");
        }

        public void FecharConexao()
        {
            EfetuarFechamentoConexao(this.dbsGeral);
        }

        private static void EfetuarFechamentoConexao(IDbConnection oconexao)
        {
            if (oconexao != null)
            {
                if (oconexao.State != ConnectionState.Closed)
                    oconexao.Close();
            }
        }

        public void ConfigurarConectarBase(string sservidor, string sbanco,
            bool MantemConexaoAberta)
        {
            //Rotina implementada por Renato - 22/03/2006 - OS 14461
            EfetuarFechamentoConexao(objConexao);
            if (objConexao == null)
            {
                objConexao = (IDbConnection)HlpFuncoes.getObjeto(
                    objFuncoesBanco.GetDriverBaseDados(),
                    objFuncoesBanco.GetClasseConexaoBaseDados());
            }
            string sPorta = WebConfigurationManager.AppSettings["PORT"];
            objConexao.ConnectionString = objFuncoesBanco.GetStringConexao(sservidor,
               sbanco, MS_USERNAME, MS_SENHA, sPorta);
            objConexao.Open();
            if (!MantemConexaoAberta)
                objConexao.Close();
        }

        public string GetDriverBaseDados()
        {
            return objFuncoesBanco.GetDriverBaseDados();
        }

        public string GetClasseDbDataAdapterBaseDados()
        {
            return objFuncoesBanco.GetClasseDbDataAdapterBaseDados();
        }

        public string GetClasseDbCommandBaseDados()
        {
            return objFuncoesBanco.GetClasseDbCommandBaseDados();
        }

        public bool AnalisaExistenciaTabela(string sTabela)
        {
            if (objConexao.State != ConnectionState.Open)
                objConexao.Open();
            bool bExiste = objFuncoesBanco.ExisteTabela(sTabela, this.dbsGeral);
            if (!this.bMantemConexaoAberta)
                objConexao.Close();
            return bExiste;
        }

        public bool AnalisaExistenciaCampoTabela(string sTabela, string sCampo)
        {
            if (objConexao.State != ConnectionState.Open)
                objConexao.Open();
            bool bExiste = objFuncoesBanco.ExisteCampo(sTabela, sCampo,
                this.dbsGeral);
            if (!this.bMantemConexaoAberta)
                objConexao.Close();
            return bExiste;
        }

        public void LimpaInfoCamposTabelasExistentes()
        {
            objFuncoesBanco.LimpaInfoCamposTabelasExistentes();
        }

        public abstract string GetOperacaoDefault(string sCdTipoDoc);

        public abstract PlataformaUtilizada GetPlataformaUtilizada();
    }
}
