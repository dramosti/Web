using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using HLP.Geral;
using System.Web.Configuration;
using FirebirdSql.Data.FirebirdClient;
using System.Configuration;

namespace HLP.Dados
{
    public class HlpDbFuncoesGeral
    {
        private HlpDbFuncoesGeral()
        {
        }

        public static DataTable QrySeekRet(IDbConnection conexao,
           string sBibliotecaDll, string sClasseDataAdapter, string sClasseDbCommand,
           string sExpressaoSql, bool bFechaConexao)
        {
            DbDataAdapter da = (DbDataAdapter)HlpFuncoes.getObjeto(sBibliotecaDll,
                sClasseDataAdapter);
            da.SelectCommand = (DbCommand)GetObjetoDbCommand(conexao, sBibliotecaDll,
                sClasseDbCommand, sExpressaoSql);
            if (conexao.State != ConnectionState.Open)
                conexao.Open();
            DataSet ds = new DataSet("dadoshlp");
            da.Fill(ds, "registro");
            DataTable dt = ds.Tables[0];
            if (bFechaConexao)
                conexao.Close();
            return dt;
        }

        protected static IDataReader QrySeekRet(IDbConnection conexao,
            string sBibliotecaDll, string sClasseDbCommand,
            string sExpressaoSql, bool bFechaConexao)
        {
            IDbCommand comando = GetObjetoDbCommand(conexao, sBibliotecaDll,
                sClasseDbCommand, sExpressaoSql);
            IDataReader dr = comando.ExecuteReader();
            if (bFechaConexao)
                conexao.Close();
            return dr;
        }

        public static void SqlCommand(IDbConnection conexao,
            string sBibliotecaDll, string sClasseDbCommand,
            string sExpressaoSql, bool bFechaConexao)
        {
            IDbCommand comando = GetObjetoDbCommand(conexao, sBibliotecaDll,
                sClasseDbCommand, sExpressaoSql);
            comando.ExecuteNonQuery();
            if (bFechaConexao)
                conexao.Close();
        }

        private static IDbCommand GetObjetoDbCommand(IDbConnection conexao,
            string sBibliotecaDll, string sClasseDbCommand, string sExpressaoSql)
        {
            IDbCommand comando = (IDbCommand)HlpFuncoes.getObjeto(sBibliotecaDll,
                sClasseDbCommand);
            if (conexao.State != ConnectionState.Open)
                conexao.Open();
            comando.Connection = conexao;
            comando.CommandText = sExpressaoSql;
            return comando;
        }

        public static string RetornaStrValor(object Valor, DataColumn campo)
        {
            string sValor = null;
            if (Valor != null)
                sValor = Valor.ToString().Trim();
            bool bVazio = ((sValor == null) || (sValor.Equals(String.Empty)));
            bool bPontoFlutuante =
                ((campo.DataType == System.Type.GetType("System.Double")) ||
                 (campo.DataType == System.Type.GetType("System.Decimal")));
            if (bPontoFlutuante)
            {
                if (bVazio)
                    return "0";
                else
                    return Valor.ToString().Replace(',', '.');
            }
            if (bVazio)
                return "NULL";
            if ((campo.DataType == System.Type.GetType("System.Int32")) ||
                (bPontoFlutuante))
                return sValor.ToString();
            if (campo.DataType == System.Type.GetType("System.DateTime"))
                return "'" + ((DateTime)Valor).ToString("MM/dd/yyyy") + "'";
            return "'" + sValor.ToString() + "'";
        }

        public static bool VerificaExistenciaGenerator(string sNomeGen)
        {
            FbConnection Conn = null;
            try
            {
                string strConn = ConfigurationManager.ConnectionStrings["ConnectionStringFB"].ConnectionString.ToString();
                Conn = new FbConnection(strConn);

                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("SELECT RDB$GENERATORS.RDB$GENERATOR_NAME ");
                sQuery.Append("FROM RDB$GENERATORS ");
                sQuery.Append("WHERE (RDB$GENERATORS.RDB$GENERATOR_NAME = '" + sNomeGen + "')");
                FbCommand command = new FbCommand(sQuery.ToString(), Conn);
                Conn.Open();
                return (command.ExecuteScalar().ToString().Trim() != "" ? true : false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public static string RetornaProximoValorGenerator(string sNomeGen)
        {
            FbConnection con = new FbConnection(ConfigurationManager.ConnectionStrings["ConnectionStringFB"].ConnectionString.ToString());
            try
            {

                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("Select ");
                sQuery.Append("gen_id(" + sNomeGen + ",1) ");
                sQuery.Append("from rdb$database ");

                FbCommand command = new FbCommand(sQuery.ToString(), con);
                con.Open();
                return command.ExecuteScalar().ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }

        }

        public static void CreateGenerator(string sNomeGen, int iValorIni)
        {
            FbConnection con = new FbConnection(ConfigurationManager.ConnectionStrings["ConnectionStringFB"].ConnectionString.ToString());
            try
            {
                StringBuilder sQuery = new StringBuilder();
                sQuery.Append(" CREATE GENERATOR " + sNomeGen);
                FbCommand Command = new FbCommand(sQuery.ToString(), con);
                con.Open();
                Command.ExecuteNonQuery();
                sQuery = new StringBuilder();
                sQuery.Append(" SET GENERATOR " + sNomeGen + " TO " + iValorIni.ToString());
                Command = new FbCommand(sQuery.ToString(), con);
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }

        }
    }
}
