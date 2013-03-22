using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace DAO
{
    public class daoRelNfFrete
    {

        public DataTable GetDadosToReport(string sStatus, string sDtInicio, string sDtFinal)
        {
            if (sStatus == "A")
            {
                sStatus = "";
            }
            string squery = "SELECT * FROM SP_RELNF_FRETE_POR_STATUS ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')";

            squery = string.Format(squery,
                                daoStatic.CD_EMPRESA,
                                daoStatic.CD_EMPRESA,
                                daoStatic.CD_CLIFOR,
                                daoStatic.CD_CLIFOR,
                                "       ",
                                "|||||||",
                                "       ",
                                "|||||||",
                                sDtInicio,
                                sDtFinal,
                                sStatus);

            FbCommand command = new FbCommand();
            command.Connection = new FbConnection(daoStatic.sConn);
            command.CommandText = squery.ToString();
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Clear();

            DataTable dtReturn = new DataTable();
            try
            {
                command.Connection.Open();
                FbDataAdapter dp = new FbDataAdapter(command);
                dp.Fill(dtReturn);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                command.Connection.Close();
            }
            return dtReturn;
        }
    }
}
