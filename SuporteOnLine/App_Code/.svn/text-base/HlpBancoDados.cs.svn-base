using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FirebirdSql.Data.FirebirdClient;
using System.Data.SqlClient;

/// <summary>
/// Summary description for HlpBancoDados
/// </summary>
/// 

public class HlpBancoDados
{
    

	public HlpBancoDados()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static IDbConnection GetConexao()
    {
        string strConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        string strProvider = ConfigurationManager.ConnectionStrings["ConnectionString"].ProviderName.Substring(0,4);
    
        if ((strProvider.Equals("Fire")))
            return new FbConnection(strConn);
        else if ((strProvider.Equals("Syst")))
            return new SqlConnection(strConn);
        else
            throw new Exception("O banco de dados não foi definido ou é inválido!");
      }

    public static IDbCommand CommandSelect(string strExpressao)
    {
        IDbConnection Conn = HlpBancoDados.GetConexao();

        FbCommand cmdSelect = new FbCommand();
        
        cmdSelect.CommandText = strExpressao;
        cmdSelect.CommandType = CommandType.Text;
        cmdSelect.Connection = (FbConnection)Conn;

        return cmdSelect;
 
    }

    public static IDbCommand CommandProcedure(string strNomeProc)
    {
        IDbConnection Conn = HlpBancoDados.GetConexao();

        FbCommand cmdProc = new FbCommand();

        cmdProc.CommandText = strNomeProc;
        cmdProc.CommandType = CommandType.StoredProcedure;
        cmdProc.Connection = (FbConnection)Conn;

        return cmdProc;

    }

    public static string GetUsuario(string strCodigoUsu)
    {
        int iaux = strCodigoUsu.Length;
        string strRetornoUsu = String.Empty;
        if (iaux < 10)
            strRetornoUsu = "0000000000".Substring(0, 10 - iaux) + strCodigoUsu;
        else
            strRetornoUsu = strCodigoUsu;

        return strRetornoUsu;
    }

    public static string ConverterData(string strCampo)
    {
        if ((!strCampo.Equals(String.Empty) || (!strCampo.Equals(null))))
            strCampo = strCampo.Replace("/", ".");

        return strCampo;
    }
    public static string GetCodigo(string strCodigo)
    {
        int iaux = strCodigo.Length;
        string strRetorno = String.Empty;
        if (iaux < 7)
            strRetorno = "0000000".Substring(0, 7 - iaux) + strCodigo;
        else
            strRetorno = strCodigo;

        return strRetorno;
    }
    
}
