using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using HLP.Geral;
using System.Web.SessionState;

namespace HLP.Dados
{
    public abstract class BaseDAO
    {

        private List<string> objListaAdvertencias = new List<string>();
        private List<string> objListaErros = new List<string>();
        private List<string> objCampos = new List<string>();
        private List<string> objChavePrimaria = new List<string>();
        protected Dictionary<string, string> ValoresPrimarios =
            new Dictionary<string, string>();
        private DataTable objEstrutura = null;
        private DataRow objRegistroAtual;
        protected Tabela oTabelas;
        private StatusRegistro status = StatusRegistro.Nenhum;
        private string sTabela;
        private string sListaCampos = "*";
        private string sCamposOrderBy = null;
        private bool bCarregaFilhosAutomaticamente = true;
        private Dictionary<string, EstruturaFilha> objDadosFilhos =
            new Dictionary<string, EstruturaFilha>();
        private EstruturaFilha objEstruturaFilha = null;
        private bool bIniciandoInclusao = false;

        public EstruturaFilha ObjetoEstruturaFilha
        {
            get
            {
                return objEstruturaFilha;
            }
            set
            {
                objEstruturaFilha = value;
            }
        }

        public BaseDAO(Tabela oTabelas)
        {
            this.oTabelas = oTabelas;
        }

        protected void Inicializar()
        {
            StringBuilder strCampos = new StringBuilder();
            foreach (string sCampo in objCampos)
            {
                if (strCampos.Length > 0)
                    strCampos.Append(",");
                strCampos.Append(sCampo);
            }
            if (strCampos.Length > 0)
                sListaCampos = strCampos.ToString();
            objEstrutura = oTabelas.hlpDbFuncoes.qrySeekRet(sTabela, sListaCampos,
                GeraWhereChavePrimaria(true));
            foreach (string sCampo in objChavePrimaria)
                ValoresPrimarios.Add(sCampo, String.Empty);
        }

        public DataRow RegistroAtual
        {
            get
            {
                return objRegistroAtual;
            }
            set
            {
                objRegistroAtual = value;
                PreencherListaValoresPrimarios();
                if (bCarregaFilhosAutomaticamente)
                    SincronizarRegistrosFilhos();
            }
        }

        public List<string> Campos
        {
            get
            {
                return objCampos;
            }
        }

        public List<string> ChavePrimaria
        {
            get
            {
                return objChavePrimaria;
            }
        }

        public string Tabela
        {
            get
            {
                return sTabela;
            }
            set
            {
                if (value != null)
                    sTabela = value.ToUpper();
                else
                    sTabela = null;
            }
        }

        public DataTable EstruturaDataTable
        {
            get
            {
                return objEstrutura;
            }
        }

        public StatusRegistro Status
        {
            get
            {
                return status;
            }
        }

        public bool CarregaFilhosAutomaticamente
        {
            get
            {
                return bCarregaFilhosAutomaticamente;
            }
            set
            {
                bCarregaFilhosAutomaticamente = value;
            }
        }

        public string CamposOrderBy
        {
            get
            {
                return sCamposOrderBy;
            }
            set
            {
                sCamposOrderBy = value;
            }
        }


        public Dictionary<string, EstruturaFilha> DadosFilhos
        {
            get
            {
                return objDadosFilhos;
            }
        }

        public bool IniciandoInclusao
        {
            get
            {
                return bIniciandoInclusao;
            }
        }

        public void Insert()
        {
            string sExpresao = MontaComando.MontaExpressaoSql(
                TipoComando.Insert, sTabela, String.Empty,
                null, false, objRegistroAtual);
            oTabelas.hlpDbFuncoes.SqlCommand(sExpresao);
        }

        private void Update()
        {
            string sExpresao = MontaComando.MontaExpressaoSql(
                TipoComando.Update, sTabela, GeraWhereChavePrimaria(),
                null, false, objRegistroAtual);
            oTabelas.hlpDbFuncoes.SqlCommand(sExpresao);
        }

        private void LimparValoresPrimarios()
        {
            foreach (string sCampo in objChavePrimaria)
                ValoresPrimarios[sCampo] = String.Empty;
        }

        private void Delete()
        {
            oTabelas.hlpDbFuncoes.SqlCommand(
                "DELETE FROM " + sTabela + " WHERE " +
                GeraWhereChavePrimaria());
            RegistroAtual = null;
            LimparValoresPrimarios();
            CancelarOperacao();
        }

        public DataTable Select(string sWhere)
        {
            return oTabelas.hlpDbFuncoes.qrySeekRet(this.Tabela,
                sListaCampos, sWhere, sCamposOrderBy);
        }

        protected abstract bool Exclusao();
        protected abstract bool PrevalidaAlt();
        protected abstract bool PrevalidaCad();
        protected abstract bool PosvalidaCad();
        protected abstract bool Valid(ITela oCampo);
        protected abstract bool When(ITela oCampo);

        protected void PreencherListaValoresPrimarios()
        {
            LimparValoresPrimarios();
            if (objRegistroAtual == null)
                return;
            object valor;
            foreach (string sCampo in objChavePrimaria)
            {
                valor = objRegistroAtual[sCampo];
                if ((valor != null) && (!valor.ToString().Equals(String.Empty)))
                    ValoresPrimarios[sCampo] = valor.ToString();
            }
        }

        private void LimparListas()
        {
            objListaAdvertencias.Clear();
            objListaErros.Clear();

        }

        private string GeraWhereChavePrimaria()
        {
            return GeraWhereChavePrimaria(false);
        }

        private string GeraWhereChavePrimaria(bool bChaveNula)
        {
            StringBuilder str = new StringBuilder();
            foreach (string sCampo in objChavePrimaria)
            {
                if (str.Length > 0)
                    str.Append(" AND ");
                str.Append("(" + sCampo);
                if (bChaveNula)
                    str.Append(" IS NULL)");
                else
                {
                    str.Append(" = " + HlpDbFuncoesGeral.RetornaStrValor(
                            ValoresPrimarios[sCampo],
                            objEstrutura.Columns[sCampo]) + ")");
                }
            }
            return str.ToString();
        }

        public bool ProcessarExclusao()
        {
            LimparListas();
            bool bExcluir = Exclusao();
            if (bExcluir)
                Delete();
            return bExcluir;
        }

        public void CancelarOperacao()
        {
            status = StatusRegistro.Nenhum;
            objEstrutura.Clear();
        }

        protected abstract void GerarNovoRegistro();

        public bool ProcessarPrevalidaCad()
        {
            bIniciandoInclusao = true;
            DataRow oldRegistroAtual = objRegistroAtual;
            LimparValoresPrimarios();
            CarregarNovoRegistro();
            LimparListas();
            bool bRetorno = PrevalidaCad();
            if (bRetorno)
                status = StatusRegistro.IncluindoRegistro;
            else
            {
                CancelarOperacao();
                RegistroAtual = oldRegistroAtual;
            }
            bIniciandoInclusao = false;
            return bRetorno;
        }

        public bool ChavePrimariaPreenchida()
        {
            bool bRetorno = false;
            object valor;
            foreach (string sCampo in objChavePrimaria)
            {
                valor = ValoresPrimarios[sCampo];
                bRetorno = ((valor != null) && (!valor.ToString().Equals(String.Empty)));
                if (!bRetorno)
                    return false;
            }
            return bRetorno;
        }

        protected void CarregarNovoRegistro()
        {
            if (!ChavePrimariaPreenchida())
            {
                RegistroAtual = objEstrutura.NewRow();

                if (objEstruturaFilha != null)
                    objEstruturaFilha.PreencherRegistroFilhoComChavePai();

                if ((objEstrutura.Columns.Contains("CD_EMPRESA")) &&
                    ((RegistroAtual["CD_EMPRESA"] == null) ||
                     (RegistroAtual["CD_EMPRESA"].ToString().Equals(String.Empty))))
                    RegistroAtual["CD_EMPRESA"] = oTabelas.sEmpresa;
            }
            else
            {
                PreencherListaValoresPrimarios();
                objEstrutura.Clear();
                DataTable dtRegistroAtual = oTabelas.hlpDbFuncoes.qrySeekRet(
                    this.sTabela, this.sListaCampos, this.GeraWhereChavePrimaria());
                RegistroAtual = dtRegistroAtual.Rows[0];
            }
        }

        public bool ProcessarPrevalidaAlt()
        {
            LimparListas();
            bool bRetorno = PrevalidaAlt();
            if (bRetorno)
                status = StatusRegistro.AlterandoRegistro;
            return bRetorno;
        }

        public bool ProcessarPosvalidaCad()
        {
            LimparListas();
            bool bRetorno = PosvalidaCad();
            if (bRetorno)
            {
                if (this.status == StatusRegistro.IncluindoRegistro)
                    GerarNovoRegistro();
                Update();
                PreencherListaValoresPrimarios();
                CancelarOperacao();
            }
            return bRetorno;
        }

        public bool ProcessarValid(ITela oCampo)
        {
            //FALTA FINALIZAR IMPLEMENTAÇÃO
            LimparListas();
            return Valid(oCampo);
        }

        public bool ProcessarWhen(ITela oCampo)
        {
            //FALTA FINALIZAR IMPLEMENTAÇÃO
            LimparListas();
            return When(oCampo);
        }

        public void SetErro(string sMensagem)
        {
            if (!objListaErros.Contains(sMensagem))
            {
                if (objListaErros.Count > 0)
                    objListaErros.Add("\n");
                objListaErros.Add(sMensagem);
            }
        }

        public void SetAdvertencia(string sMensagem)
        {
            if (!objListaAdvertencias.Contains(sMensagem))
            {
                if (objListaAdvertencias.Count > 0)
                    objListaAdvertencias.Add("\n");
                objListaAdvertencias.Add(sMensagem);
            }
        }

        private string GetErrosOuAdvertencias(List<string> lista)
        {
            StringBuilder str = new StringBuilder();
            foreach (string sMensagem in lista)
            {
                if (str.Length > 0)
                    str.Append("\n");
                str.Append(sMensagem);
            }
            if (str.Length > 0)
                return str.ToString();
            else
                return null;
        }

        public string GetErros()
        {
            return GetErrosOuAdvertencias(objListaErros);
        }

        public string GetAdvertencias()
        {
            return GetErrosOuAdvertencias(objListaAdvertencias);
        }

        public void SincronizarRegistrosFilhos()
        {
            foreach (EstruturaFilha objPersistencia in objDadosFilhos.Values)
                objPersistencia.Sincronizar();
        }

        public void SetValoresRegistroAtual(ITelaCadastro TelaCadastroAtual)
        {
            List<ITela> componentes =
                TelaCadastroAtual.GetComponentesTelaCadastro();
            string sTabela;
            foreach (ITela componente in componentes)
            {
                if (componente.GetApenasLeitura())
                    continue;
                sTabela = componente.GetTabela();
                if ((sTabela != null) && (sTabela.Equals(this.sTabela)))
                    objRegistroAtual[componente.GetCampo()] =
                        componente.GetValor();
            }
        }

        public void ClearComponentes(ITelaCadastro TelaCadastroAtual)
        {
            List<ITela> componentes =
                TelaCadastroAtual.GetComponentesTelaCadastro();
            foreach (ITela componente in componentes)
            {
                componente.SetValor("");
            }

        }

        public void PreencherComponentesComValoresBanco(ITelaCadastro TelaCadastroAtual)
        {
            List<ITela> componentes =
                TelaCadastroAtual.GetComponentesTelaCadastro();
            string sTabela;
            foreach (ITela componente in componentes)
            {
                sTabela = componente.GetTabela();
                if ((sTabela != null) && (sTabela.Equals(this.sTabela)))
                    componente.SetValor(objRegistroAtual[componente.GetCampo()]);
            }
        }

        public string GetValorPrimario(string sCampo)
        {
            return ValoresPrimarios[sCampo];
        }

        public static void CancelarOperacaoObjetoDAO(BaseDAO objDAO)
        {
            if (objDAO != null)
            {
                objDAO.CancelarOperacao();
                objDAO.RegistroAtual = null;
            }
        }

        public DataRow GetRegistroPai()
        {
            if (objEstruturaFilha != null)
                return objEstruturaFilha.PaiDAO.RegistroAtual;
            else
                return null;
        }

    }
}
