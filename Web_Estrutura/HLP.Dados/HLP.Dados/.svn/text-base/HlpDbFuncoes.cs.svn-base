using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.Configuration;

namespace HLP.Dados
{
    public class HlpDbFuncoes
    {
        private Tabela oTabelas = null;

        public HlpDbFuncoes(Tabela objTabela)
        {
            oTabelas = objTabela;
        }

        public DataTable qrySeekRet(string sExpressao)
        {
            DataTable dt = HlpDbFuncoesGeral.QrySeekRet(oTabelas.dbsGeral,
                oTabelas.GetDriverBaseDados(),
                oTabelas.GetClasseDbDataAdapterBaseDados(),
                oTabelas.GetClasseDbCommandBaseDados(),
                sExpressao, !oTabelas.MantemConexaoAberta);
            return dt;
        }

        public DataTable qrySeekRet(string sTabela, string sCampos)
        {
            return (qrySeekRet(sTabela, sCampos, String.Empty, String.Empty));
        }

        public DataTable qrySeekRet(string sTabela, string sCampos,
             string sWhere)
        {
            return (qrySeekRet(sTabela, sCampos, sWhere, String.Empty));
        }

        public DataTable qrySeekRet(string sTabela, string sCampos,
            string sWhere, string sOrdem)
        {
            StringBuilder strExpressao = new StringBuilder();
            strExpressao.Append("SELECT ");
            if (sCampos.Trim() == String.Empty)
                strExpressao.Append("*");
            else
                strExpressao.Append(sCampos);
            strExpressao.Append(" FROM " + sTabela);

            StringBuilder strWhere = new StringBuilder();
            if ((sWhere != null) && (sWhere.Trim() != String.Empty))
                strWhere.Append(sWhere);
            if ((fExisteCampo(sTabela, "CD_EMPRESA")) &&
                (sWhere.ToUpper().IndexOf("CD_EMPRESA") < 0))
            {
                if (strWhere.Length > 0)
                    strWhere.Append(" AND ");
                strWhere.Append("(CD_EMPRESA = '" + oTabelas.sEmpresa + "')");
            }
            if (strWhere.Length > 0)
            {
                strWhere.Insert(0, " WHERE ");
                strExpressao.Append(strWhere.ToString());
            }

            if ((sOrdem != null) && (sOrdem.Trim() != String.Empty))
                strExpressao.Append(" ORDER BY " + sOrdem);

            DataTable dt = HlpDbFuncoesGeral.QrySeekRet(oTabelas.dbsGeral,
                oTabelas.GetDriverBaseDados(),
                oTabelas.GetClasseDbDataAdapterBaseDados(),
                oTabelas.GetClasseDbCommandBaseDados(),
                strExpressao.ToString(), !oTabelas.MantemConexaoAberta);
            return dt;
        }

        public string qrySeekValue(string sTabela, string sCampo,
            string sWhere)
        {
            return qrySeekValue(sTabela, sCampo, sWhere, false);
        }

        private string qrySeekValue(string sTabela, string sCampo,
            string sWhere, bool bGeraErroSemRegistros)
        {
            DataTable dt = qrySeekRet(sTabela, sCampo, sWhere);
            String svalor = null;
            if (dt.Rows.Count > 0)
            {
                DataRow registro = dt.Rows[0];
                //svalor = registro[sCampo].ToString();
                svalor = registro[0].ToString();
            }
            else
            {
                if (bGeraErroSemRegistros)
                    throw new Exception("Não foram encontrados registros válidos!");
            }
            if (svalor == null)
                svalor = String.Empty;
            return svalor;
        }

        public string fMemoStr(string sNivelControl)
        {
            string svalor = null;
            try
            {
                svalor = qrySeekValue("CONTROL", "CD_CONTEUD",
                    "(CD_NIVEL = '" + sNivelControl + "')", true);
            }
            catch
            {
                throw new Exception("O nível do Control " + sNivelControl +
                    " não foi encontrado!");
            }
            return svalor;
        }

        public string fNmCampos(string sTabela)
        {
            return fNmCampos(sTabela, false);
        }

        public string fNmCampos(string sTabela, bool bRetornaComNomeTabela)
        {
            sTabela = sTabela.ToUpper();
            string snmestrutura = qrySeekValue("ARQSISTE", "NM_ESTRUTURA",
                "(NM_ARQUIVO = '" + sTabela + "')");
            StringBuilder strCampos = new StringBuilder();

            DataTable qry = qrySeekRet("HLPESTRU",
                "DISTINCT ST_TPCAMPO, CD_CAMPO, CD_ALTSIST",
                "(NM_ESTRUTURA = '" + snmestrutura + "') AND (ST_ATIVO = 'S')",
                "ST_TPCAMPO, CD_CAMPO");
            int iaux;
            string scampo;
            foreach (DataRow registro in qry.Rows)
            {
                scampo = registro["CD_CAMPO"].ToString().Trim();
                iaux = scampo.IndexOf("->");
                if (iaux == 0)
                    scampo = scampo.Substring(iaux + 2);
                else
                    continue;

                if (!registro["CD_ALTSIST"].ToString().Trim().Equals("HLP"))
                {
                    if (!fExisteCampo(sTabela, scampo))
                        continue;
                }

                if (snmestrutura.Equals("GERADOR2"))
                    snmestrutura += oTabelas.NmModulo;

                if (strCampos.Length > 0)
                    strCampos.Append(", ");
                if (bRetornaComNomeTabela)
                    strCampos.Append(snmestrutura + ".");
                strCampos.Append(scampo);
            }

            return strCampos.ToString().ToUpper();
        }

        public bool fExisteCampo(string sTabela, string sCampo)
        {
            return oTabelas.AnalisaExistenciaCampoTabela(sTabela, sCampo);
        }

        public bool fExisteTabela(string sTabela)
        {
            return oTabelas.AnalisaExistenciaTabela(sTabela);
        }

        public bool qrySeek(string sTabela, string[] sCampos, string[] sValores)
        {
            StringBuilder strCampos = new StringBuilder();
            StringBuilder strWhere = new StringBuilder();
            string sCampo;

            for (int i = 0; i < sCampos.Length; i++)
            {
                if (strCampos.Length > 0)
                {
                    strCampos.Append(", ");
                    strWhere.Append(" AND ");
                }
                sCampo = sCampos[i];
                strCampos.Append(sCampo);
                strWhere.Append("(" + sCampo + " = '" + sValores[i].Trim() + "')");
            }

            DataTable dt = qrySeekRet(sTabela, strCampos.ToString(),
                strWhere.ToString());

            return (dt.Rows.Count > 0);
        }

        public void SqlCommand(string sExpressao)
        {
            HlpDbFuncoesGeral.SqlCommand(oTabelas.dbsGeral,
                oTabelas.GetDriverBaseDados(),
                oTabelas.GetClasseDbCommandBaseDados(),
                sExpressao, !oTabelas.MantemConexaoAberta);
        }

        public static string RetornaStringDataSQL(string sData)
        {
            DateTime dData = Convert.ToDateTime(sData);
            sData = "'" + dData.ToString("MM/dd/yyyy") + "'";
            return sData;
        }

        public bool VerificaExistenciaGenerator(string sNomeGen)
        {
            try
            {
                return HlpDbFuncoesGeral.VerificaExistenciaGenerator(sNomeGen);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string RetornaProximoValorGenerator(string sNomeGen)
        {
            return HlpDbFuncoesGeral.RetornaProximoValorGenerator(sNomeGen);
        }

        public void CreateGenerator(string sNomeGen, int iValorIni)
        {
            try
            {
                HlpDbFuncoesGeral.CreateGenerator(sNomeGen, iValorIni);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
