using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAO;
using System.Data;

namespace Business
{
    public class boRelNfFrete : daoRelNfFrete
    {
        public decimal VL_PESO { get; set; }
        public string CD_EMPRESA { get; set; }
        public string DT_EMISSAO { get; set; }
        public string CD_NF { get; set; }
        public decimal VL_NF { get; set; }
        private string _NM_SOCIAL;

        public string NM_SOCIAL
        {
            get { return _NM_SOCIAL; }
            set
            {
                _NM_SOCIAL = value;
                if (_NM_SOCIAL.Length > 24)
                {
                    _NM_SOCIAL = _NM_SOCIAL.Substring(0, 24);
                }
            }
        }
        private string _NM_SOCIAL2;

        public string NM_SOCIAL2
        {
            get { return _NM_SOCIAL2; }
            set
            {
                _NM_SOCIAL2 = value;
                if (_NM_SOCIAL2.Length > 24)
                {
                    _NM_SOCIAL2 = _NM_SOCIAL2.Substring(0, 24);
                }
            }
        }
        public string CD_CLIFOR { get; set; }
        public string NM_CLIENTE { get; set; }
        public string CD_CONHECI { get; set; }
        public string NR_LANC { get; set; }
        public decimal VL_TOTAL { get; set; }
        public string SREPETIDO { get; set; }
        public string ST_BAIXA { get; set; }


        public List<boRelNfFrete> GetDadosToReport(string sStatus, string sDtInicio, string sDtFinal)
        {
            DataTable dt = base.GetDadosToReport(sStatus, sDtInicio, sDtFinal);

            List<boRelNfFrete> lretorno = dt.AsEnumerable()
                  .Select(dr => new boRelNfFrete()
                  {
                      VL_PESO = Convert.ToDecimal(dr["VL_PESO"].ToString()),
                      CD_EMPRESA = dr["CD_EMPRESA"].ToString(),
                      DT_EMISSAO = Convert.ToDateTime(dr["DT_EMISSAO"].ToString()).ToShortDateString(),
                      CD_NF = dr["CD_NF"].ToString(),
                      VL_NF = Convert.ToDecimal(dr["VL_NF"].ToString()),
                      NM_SOCIAL = dr["NM_SOCIAL"].ToString(),
                      NM_SOCIAL2 = dr["NM_SOCIAL2"].ToString(),
                      CD_CLIFOR = dr["CD_CLIFOR"].ToString(),
                      NM_CLIENTE = dr["NM_CLIENTE"].ToString(),
                      CD_CONHECI = dr["CD_CONHECI"].ToString(),
                      NR_LANC = dr["NR_LANC"].ToString(),
                      VL_TOTAL = Convert.ToDecimal(dr["VL_TOTAL"].ToString()),
                      SREPETIDO = dr["SREPETIDO"].ToString(),
                      ST_BAIXA = dr["ST_BAIXA"].ToString()
                  })
                  .ToList();

            return lretorno;
        }

    }
}
