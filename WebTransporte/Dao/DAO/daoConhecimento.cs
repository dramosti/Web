using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;

namespace DAO
{
    public class daoConhecimento
    {




        /// <summary>
        ///  sequencia - PK DA TABELA CONHECIM.
        /// </summary>
        private string _NR_LANC;

        public string NR_LANC
        {
            get { return _NR_LANC; }
            set { _NR_LANC = value.ToString().PadLeft(7, '0'); }
        }

        /// <summary>
        /// CÓD. CONHECIMENTO
        /// </summary>
        public string CD_CONHECI { get; set; }

        public DateTime DT_EMISSAO { get; set; }

        /// <summary>
        /// razão social do Destinatário
        /// </summary>
        public string DS_DESTINATARIO { get; set; }

        /// <summary>
        /// FK -> CD_REDES
        /// </summary>
        public string DS_REDESPACHO { get; set; }


        /// <summary>
        /// REMETENTE,R;DESTINATÁRIO,D;REDESPACHO,E;CONSIGNATÁRIO,C
        /// </summary>
        private string _CD_RESPONS;

        public string CD_RESPONS
        {
            get { return _CD_RESPONS; }
            set
            {
                switch (value.ToString())
                {
                    case "R": { _CD_RESPONS = "Remetente"; }
                        break;
                    case "D": { _CD_RESPONS = "Destinatário"; }
                        break;
                    case "E": { _CD_RESPONS = "Redespacho"; }
                        break;
                    case "C": { _CD_RESPONS = "Consignatário"; }
                        break;
                }
            }
        }


        /// <summary>
        /// SIM,1;NÃO, / cancelado ou não
        /// </summary>
        private string _ST_CONHECI;

        public string ST_CONHECI
        {
            get { return _ST_CONHECI; }
            set { _ST_CONHECI = (value.ToString().Equals("0") ? "Não" : "Sim"); }
        }

        /// <summary>
        /// data Cancelamento
        /// </summary>
        private string _DT_CANCCON;

        public string DT_CANCCON
        {
            get { return _DT_CANCCON; }
            set { _DT_CANCCON = (value.ToString() != "" ? (Convert.ToDateTime(value.ToString())).ToString("dd/MM/yyyy") : ""); ; }
        }

        /// <summary>
        /// SIM,S;NÃO,N  ->> principal  -> "Baixado" : "Aberto"
        /// </summary>
        private string _ST_BAIXA;

        public string ST_BAIXA
        {
            get { return _ST_BAIXA; }
            set { _ST_BAIXA = (value.ToString() == "S" ? "Baixado" : "Aberto"); }
        }

        private string _DT_BAIXA;

        public string DT_BAIXA
        {
            get { return _DT_BAIXA; }
            set { _DT_BAIXA = (value.ToString() != "" ? (Convert.ToDateTime(value.ToString())).ToString("dd/MM/yyyy") : ""); }
        }

        public daoDadosAdicionais DadosAdicionais = new daoDadosAdicionais();

        public daoComplemento Complemento = new daoComplemento();

        //public List<daoNF> NotasFiscais = new List<daoNF>();

        private class CamposSelect
        {
            public string sCampo = "";
            public string sAlias = "";
        }


        /// <summary>
        /// get all
        /// </summary>
        /// <param name="sBaixa">ABERTO, FECHADO, AMBOS</param>
        /// <param name="sCAMPO"> CD_CONHECI, DT_EMISSAO, DS_DESTINATARIO,CD_NF</param>
        /// <param name="sParametro">IGUAL, MAIOR, MENOR, ENTRE</param>
        /// <param name="sValor1"></param>
        /// <param name="sValor2"></param>
        /// <returns></returns>
        public List<daoConhecimento> GetAll(string sBaixa, string sCAMPO, string sParametro, string sValor1, string sValor2)
        {
            List<daoConhecimento> objListaConhecimento = new List<daoConhecimento>();

            if (sValor1 != null)
            {
                if (sValor1 != "")
                {
                    if (sCAMPO.Equals("CD_NF"))
                    {
                        objListaConhecimento = PesquisaPorNF(sBaixa, sParametro, sValor1, sValor2);
                    }
                    else
                    {
                        #region BUSCA CONHECIMENTOS
                        sValor1 = sValor1.ToUpper();
                        sValor2 = (sValor2 != null ? sValor2.ToUpper() : ""); ;

                        List<CamposSelect> lCampos = new List<CamposSelect>();
                        StringBuilder sCampos = new StringBuilder();
                        sCampos.Append("SELECT ");
                        lCampos.Add(new CamposSelect { sCampo = "conhec.NR_LANC", sAlias = "NR_LANC" });
                        lCampos.Add(new CamposSelect { sCampo = "conhec.CD_CONHECI", sAlias = "CD_CONHECI" });
                        lCampos.Add(new CamposSelect { sCampo = "conhec.DT_EMISSAO", sAlias = "DT_EMISSAO" });
                        lCampos.Add(new CamposSelect { sCampo = "dest.nm_social", sAlias = "DS_DESTINATARIO" });
                        lCampos.Add(new CamposSelect { sCampo = "redesp.nm_social", sAlias = "DS_REDESPACHO" });
                        lCampos.Add(new CamposSelect { sCampo = "conhec.CD_RESPONS", sAlias = "CD_RESPONS" });
                        lCampos.Add(new CamposSelect { sCampo = "coalesce(ST_CONHECI,0)", sAlias = "ST_CONHECI" });
                        lCampos.Add(new CamposSelect { sCampo = "coalesce(DT_CANCCON, '')", sAlias = "DT_CANCCON" });
                        lCampos.Add(new CamposSelect { sCampo = "conhec.ST_BAIXA", sAlias = "ST_BAIXA" });
                        lCampos.Add(new CamposSelect { sCampo = "coalesce(conhec.DT_BAIXA,'')", sAlias = "DT_BAIXA" });
                        lCampos.Add(new CamposSelect { sCampo = "conhec.CD_CFOP", sAlias = "CD_CFOP" });
                        lCampos.Add(new CamposSelect { sCampo = "conhec.DS_COLETA", sAlias = "DS_COLETA" });
                        lCampos.Add(new CamposSelect { sCampo = "conhec.CD_UFCOLE", sAlias = "CD_UFCOLE" });
                        lCampos.Add(new CamposSelect { sCampo = "(case when coalesce(conhec.cd_ocor,'0') <> 0 then (select tabocorr.ds_ocor from tabocorr where conhec.cd_ocor = tabocorr.cd_ocor) else '' end)", sAlias = "DS_OCOR" });
                        lCampos.Add(new CamposSelect { sCampo = "coalesce(conhec.ds_texto,'')", sAlias = "DS_TEXTO" });
                        lCampos.Add(new CamposSelect { sCampo = "conhec.DS_ENDENTREGA", sAlias = "DS_ENDENTREGA" });
                        lCampos.Add(new CamposSelect { sCampo = "(case when coalesce(conhec.cd_motoris,0) <> 0 then (select motorista.nm_motoris from motorista where motorista.cd_motoris = conhec.cd_motoris) else 'S/NOME' end)", sAlias = "NM_MOTORIS" });
                        lCampos.Add(new CamposSelect { sCampo = "coalesce(veiculo.cd_cidplac,'')", sAlias = "cd_cidplac" });
                        lCampos.Add(new CamposSelect { sCampo = "coalesce(veiculo.cd_placa,'')", sAlias = "cd_placa" });
                        lCampos.Add(new CamposSelect { sCampo = "coalesce(veiculo.cd_modelo,'') ", sAlias = "cd_modelo" });

                        ConcatCampos(lCampos, sCampos);

                        StringBuilder sFrom = new StringBuilder();
                        sFrom.Append("FROM conhecim conhec left join remetent dest     on conhec.cd_destinat = dest.cd_remetent " + Environment.NewLine);
                        sFrom.Append("                     left join remetent redesp   on conhec.cd_redes = redesp.cd_remetent " + Environment.NewLine);
                        sFrom.Append("                     left join veiculo         on conhec.cd_veiculo = veiculo.cd_veiculo " + Environment.NewLine);

                        StringBuilder sWhere = new StringBuilder();
                        sWhere.Append("Where " + sCAMPO + " ");

                        string sLike = " Like '%{0}%' ";
                        string soutros = " {0} '{1}' ";
                        string sBetween = "{0} '{1}' and '{2}' ";


                        if (sCAMPO == "conhec.DT_EMISSAO")
                        {
                            sValor1 = (Convert.ToDateTime(sValor1)).ToString("dd.MM.yyyy");
                            if (sParametro == "between")
                            {
                                sValor2 = (Convert.ToDateTime(sValor2)).ToString("dd.MM.yyyy");
                            }
                        }
                        else if (sCAMPO == "conhec.CD_CONHECI")
                        {
                            sValor1 = sValor1.PadLeft(6, '0');
                            sValor2 = sValor2.PadLeft(6, '0');
                        }

                        switch (sParametro)
                        {
                            case "=":
                                {
                                    if (sCAMPO != "conhec.DT_EMISSAO") { sWhere.Append(string.Format(sLike, sValor1)); }
                                    else
                                    { sWhere.Append(string.Format(soutros, sParametro, sValor1)); }
                                }
                                break;
                            case ">": { sWhere.Append(string.Format(soutros, sParametro, sValor1)); }
                                break;
                            case "<": { sWhere.Append(string.Format(soutros, sParametro, sValor1)); }
                                break;
                            case "between": { sWhere.Append(string.Format(sBetween, sParametro, sValor1, sValor2)); }
                                break;
                        }

                        if (sBaixa != "A")
                        {
                            sWhere.Append(" and conhec.ST_BAIXA = '" + sBaixa + "'");
                        }

                        sWhere.Append(" and conhec.cd_remetent  = " + daoStatic.CD_CLIFOR + " ");

                        sWhere.Append(Environment.NewLine + " ORDER by conhec.dt_emissao desc,conhec.CD_CONHECI, conhec.st_baixa " + Environment.NewLine);


                        string sQuery = sCampos.ToString() + sFrom.ToString() + sWhere.ToString();

                        using (FbConnection Conn = new FbConnection(daoStatic.sConn))
                        {
                            try
                            {
              
                                Conn.Open();
                                using (FbCommand cmd = new FbCommand(sQuery, Conn))
                                {
                                    FbDataReader dr = cmd.ExecuteReader();
                                    while (dr.Read())
                                    {
                                        daoConhecimento objConhec = new daoConhecimento();
                                        objConhec.NR_LANC = dr["NR_LANC"].ToString();
                                        objConhec.CD_CONHECI = dr["CD_CONHECI"].ToString();
                                        objConhec.DT_EMISSAO = Convert.ToDateTime(dr["DT_EMISSAO"].ToString());
                                        objConhec.DS_DESTINATARIO = dr["DS_DESTINATARIO"].ToString();
                                        objConhec.DS_REDESPACHO = dr["DS_REDESPACHO"].ToString();
                                        objConhec.CD_RESPONS = dr["CD_RESPONS"].ToString();
                                        objConhec.ST_CONHECI = dr["ST_CONHECI"].ToString();
                                        objConhec.DT_CANCCON = dr["DT_CANCCON"].ToString();
                                        objConhec.ST_BAIXA = dr["ST_BAIXA"].ToString();
                                        objConhec.DT_BAIXA = dr["DT_BAIXA"].ToString();
                                        objConhec.DadosAdicionais.DS_TEXTO = dr["DS_TEXTO"].ToString();
                                        objConhec.DadosAdicionais.DS_OCOR = dr["DS_OCOR"].ToString();
                                        objConhec.DadosAdicionais.CD_CFOP = dr["CD_CFOP"].ToString();
                                        objConhec.DadosAdicionais.DS_COLETA = dr["DS_COLETA"].ToString();
                                        objConhec.DadosAdicionais.CD_UFCOLE = dr["CD_UFCOLE"].ToString();
                                        objConhec.DadosAdicionais.DS_ENDENTREGA = dr["DS_ENDENTREGA"].ToString();
                                        objConhec.Complemento.NM_MOTORIS = dr["NM_MOTORIS"].ToString();
                                        objConhec.Complemento.CD_CIDPLAC = dr["CD_CIDPLAC"].ToString();
                                        objConhec.Complemento.CD_PLACA = dr["CD_PLACA"].ToString();
                                        objConhec.Complemento.CD_MODELO = dr["CD_MODELO"].ToString();
                                        objListaConhecimento.Add(objConhec);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            finally
                            { Conn.Close(); }

                        }
                        #endregion
                    }
                }
            }

            return objListaConhecimento;
        }

        public List<daoConhecimento> PesquisaPorNF(string sBaixa, string sParametro, string sValor1, string sValor2)
        {
            List<daoConhecimento> objListaConhecimento = new List<daoConhecimento>();
            StringBuilder sQuery = new StringBuilder();
            sValor1 = sValor1.PadLeft(6, '0');
            sValor2 = sValor2.PadLeft(6, '0');
            sQuery.Append("Select ");
            sQuery.Append("distinct(conhecim.cd_conheci) cd_conheci ");
            sQuery.Append("from nfconhec INNER JOIN conhecim on nfconhec.nr_lancconhecim = conhecim.nr_lanc ");
            if (sParametro != "between")
            {
                sQuery.Append(string.Format("Where nfconhec.cd_nf {0} '{1}' ", sParametro, sValor1));
            }
            else
            {
                sQuery.Append(string.Format("Where nfconhec.cd_nf {0} '{1}' and '{2}' ", sParametro, sValor1, sValor2));
            }

            sQuery.Append(string.Format("and conhecim.cd_remetent  = '{0}' ", daoStatic.CD_CLIFOR));


            using (FbConnection Conn = new FbConnection(daoStatic.sConn))
            {
                try
                {
                    using (FbCommand cmd = new FbCommand(sQuery.ToString(), Conn))
                    {
                        Conn.Open();
                        FbDataReader dr = cmd.ExecuteReader();

                        List<string> NFs = new List<string>();

                        while (dr.Read())
                        {
                            NFs.Add(dr["cd_conheci"].ToString());
                        }
                        string sNota1;
                        string sNota2;
                        if ((NFs.Count() > 0) && (NFs.Count() > 1))
                        {
                            sNota1 = NFs[0];
                            sNota2 = NFs[NFs.Count() - 1];
                            if (sNota2 == "")
                            {
                                sNota2 = NFs[NFs.Count() - 2];
                            }
                            objListaConhecimento = GetAll(sBaixa, "conhec.CD_CONHECI", "between", sNota1, sNota2);
                        }
                        else if (NFs.Count() == 1)
                        {
                            sNota1 = NFs[0];
                            sNota2 = NFs[NFs.Count() - 1];
                            objListaConhecimento = GetAll(sBaixa, "conhec.CD_CONHECI", "between", sNota1, sNota2);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally { Conn.Close(); }
            }
            return objListaConhecimento;

        }


        private void ConcatCampos(List<CamposSelect> lCampos, StringBuilder sCampos)
        {
            for (int i = 0; i < lCampos.Count; i++)
            {
                CamposSelect camp = lCampos[i];
                sCampos.Append(camp.sCampo + " " + camp.sAlias + ((i + 1) != lCampos.Count() ? "," : "") + Environment.NewLine);
            }
        }

    }
}
