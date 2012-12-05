using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;

namespace DAO
{
    public class daoNF
    {
        public string CD_NF { get; set; }

        public decimal VL_NF { get; set; }

        public DateTime DT_EMI { get; set; }

        public string NR_LANC { get; set; }

        public string CD_CONHECI { get; set; }

        public string ST_BAIXA { get; set; }


        public List<daoNF> GetAllNF(List<daoConhecimento> objListaDaoConhec)
        {
            List<daoNF> objListaNF = new List<daoNF>();

            using (FbConnection Conn = new FbConnection(daoStatic.sConn))
            {
                try
                {
                    Conn.Open();
                    foreach (daoConhecimento conhec in objListaDaoConhec)
                    {
                        StringBuilder sQuery = new StringBuilder();
                        sQuery.Append("Select ");
                        sQuery.Append("CD_NF, ");
                        sQuery.Append("VL_NF, ");
                        sQuery.Append("DT_EMI ");
                        sQuery.Append("from nfconhec Where nfconhec.nr_lancconhecim = @param1");


                        using (FbCommand cmd = new FbCommand(sQuery.ToString(), Conn))
                        {
                            cmd.Parameters.AddWithValue("@param1", conhec.NR_LANC);
                            FbDataReader dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                daoNF objNF = new daoNF();
                                objNF.NR_LANC = conhec.NR_LANC;
                                objNF.CD_CONHECI = conhec.CD_CONHECI;
                                objNF.CD_NF = dr["CD_NF"].ToString();
                                objNF.DT_EMI = Convert.ToDateTime(dr["DT_EMI"].ToString());
                                objNF.VL_NF = Convert.ToDecimal(dr["VL_NF"].ToString());
                                objNF.ST_BAIXA = conhec.ST_BAIXA;
                                objListaNF.Add(objNF);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally { Conn.Close(); }
            }

            return objListaNF;
        }
    }
}
