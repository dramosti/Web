using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.IO;
using System.Text;

public class Criptografia
{

	private Criptografia()
	{
	}

    public static string Encripta(string texto)
    {
        char[] arraysenha = texto.ToCharArray();
        for (int i = 0; i < arraysenha.Length; i++)
        {
            int digito = (arraysenha[i] + 125 + i + 1);
            arraysenha[i] = (char)digito;
        }
        string resultado = new string(arraysenha);
        return resultado;
    }

    public static string Decripta(string texto)
    {
        char[] arraysenha = texto.ToCharArray();
        for (int i = 0; i < arraysenha.Length; i++)
        {
            int digito = (arraysenha[i] - 125 - i - 1);
            arraysenha[i] = (char)digito;
        }
        string resultado = new string(arraysenha);
        return resultado;
    }

}
