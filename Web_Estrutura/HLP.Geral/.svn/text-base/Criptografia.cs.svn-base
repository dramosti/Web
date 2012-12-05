using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HLP.Geral
{
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
}
