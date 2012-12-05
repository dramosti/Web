using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace HLP.Dados
{
    public class EstruturaFilha
    {
        private BaseDAO objDAO = null;
        private BaseDAO objPaiDAO = null;
        private DataTable objDadosAtuais = null;
        private List<Relacionamento> objCamposRelacionamento =
            new List<Relacionamento>();

        public BaseDAO ObjetoDAO
        {
            get
            {
                return objDAO;
            }
            set
            {
                objDAO = value;
                objDAO.ObjetoEstruturaFilha = this;
            }
        }

        public BaseDAO PaiDAO
        {
            get
            {
                return objPaiDAO;
            }
            set
            {
                objPaiDAO = value;
            }
        }

        public DataTable DadosAtuais
        {
            get
            {
                return objDadosAtuais;
            }
            set
            {
                objDadosAtuais = value;
            }
        }

        public List<Relacionamento> CamposRelacionamento
        {
            get
            {
                return objCamposRelacionamento;
            }
        }

        public EstruturaFilha()
        {
        }

        public void Sincronizar()
        {
            objDadosAtuais = null;
            objDAO.RegistroAtual = null;

            if (!objPaiDAO.ChavePrimariaPreenchida())
                return;

            objDadosAtuais = objDAO.Select(GeraWhereRelacionamento());
            if (objDadosAtuais.Rows.Count > 0)
                objDAO.RegistroAtual = objDadosAtuais.Rows[0];
        }

        private string GeraWhereRelacionamento()
        {
            StringBuilder str = new StringBuilder();
            string sCampoPai;
            string sValor;
            foreach (Relacionamento ligacao in objCamposRelacionamento)
            {
                sCampoPai = ligacao.CampoPai;
                sValor = HlpDbFuncoesGeral.RetornaStrValor(
                    objPaiDAO.RegistroAtual[sCampoPai],
                    objPaiDAO.EstruturaDataTable.Columns[sCampoPai]);
                if (str.Length > 0)
                    str.Append(" AND ");
                str.Append("(" + ligacao.CampoFilho + " = " + sValor + ")");
            }
            if (str.Length == 0)
                throw new Exception(
                    "Não foram definidos os campos de " +
                    "relacionamento entre as tabelas " +
                    this.objDAO.Tabela + " e " + this.objPaiDAO.Tabela + "!!!");

            return str.ToString();
        }

        public void PreencherRegistroFilhoComChavePai()
        {
            DataRow RegistroFilho = objDAO.RegistroAtual;
            DataRow RegistroPai = objPaiDAO.RegistroAtual;
            foreach (Relacionamento ligacao in objCamposRelacionamento)
            {
                RegistroFilho[ligacao.CampoFilho] =
                    RegistroPai[ligacao.CampoPai];
            }
        }

    }
}
