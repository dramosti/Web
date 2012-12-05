using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace HLP.Dados.Faturamento
{

    public class HlpFuncoesFaturamento
    {

        private HlpFuncoesFaturamento()
        { 
        }

        public static decimal GetAliquotaIPI(string sOperacao,
            string sCliente, string sCdCf, Tabela oTabelas)
        {
            //Esta função não foi completamente finalizada!!! Faltam
            //ser verificadas todas as formas possíveis de isenção
            //que hoje existem no sistema cliente-servidor. 
            //Renato - 28/12/2006
            bool bIPIZerado = false;
            //if ((sCliente != null) && (!sCliente.Equals(String.Empty)))
            //{
            //    string sStZeraIPI = oTabelas.hlpDbFuncoes.qrySeekValue(
            //        "CLIFOR", "ST_ZERAIPI", "(CD_CLIFOR = '" + sCliente + "')");
            //    bIPIZerado = ((sStZeraIPI != null) && (sStZeraIPI.Equals("S")));
            //}
            if (!bIPIZerado)
            {
                if ((sOperacao != null) && (!sOperacao.Equals(String.Empty)))
                {
                    string sStCalcIPIFa = oTabelas.hlpDbFuncoes.qrySeekValue(                    
                        "OPEREVE", "ST_CALCIPI_FA", "(CD_OPER = '" + sOperacao + "')");
                    bIPIZerado = ((sStCalcIPIFa == null) || (!sStCalcIPIFa.Equals("S")));
                }
            }
            if (!bIPIZerado)
            {
                decimal rVlAliIPI = 0;
                if ((sCdCf != null) && (!sCdCf.Equals(String.Empty)))
                {
                    DataTable qryClasFis = oTabelas.hlpDbFuncoes.qrySeekRet(
                        "CLAS_FIS", "VL_ALIIPI", "(CD_CF = '" + sCdCf + "')");
                    if (qryClasFis.Rows.Count == 1)
                        rVlAliIPI = Convert.ToDecimal(qryClasFis.Rows[0]["VL_ALIIPI"]);
                }
                return rVlAliIPI;
            }
            else
                return 0;
        }

    }

}
