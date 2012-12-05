using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace HLP.Geral
{
    public class HlpFuncoes
    {
        private static Dictionary<string, Type> ListaClassesTipos =
             new Dictionary<string, Type>();

        private HlpFuncoes()
        {
        }

        public static Object getObjeto(String strBiblioteca, String strClasse)
        {
            Type tipo = null;
            if (ListaClassesTipos.ContainsKey(strClasse))
                tipo = ListaClassesTipos[strClasse];

            if (tipo == null)
            {
                Assembly assembly = Assembly.Load(strBiblioteca);
                tipo = assembly.GetType(strClasse);
                assembly = null;
                ListaClassesTipos.Add(strClasse, tipo);
            }

            Object objeto = Activator.CreateInstance(tipo);
            tipo = null;
            return objeto;
        }

        public static string MemoToString(Byte[] valor)
        {
            ASCIIEncoding codificacao = new ASCIIEncoding();
            return codificacao.GetString(valor);
        }

        public static bool DataValida(string sData)
        {
            bool bRetorno = (sData != null);
            if (bRetorno)
            {
                try
                {
                    System.Convert.ToDateTime(sData);
                }
                catch
                {
                    bRetorno = false;
                }
            }
            return bRetorno;
        }

        public static bool InteiroValido(string sInteiro)
        {
            bool bRetorno = (sInteiro != null);
            if (bRetorno)
            {
                try
                {
                    System.Convert.ToInt32(sInteiro);
                }
                catch
                {
                    bRetorno = false;
                }
            }
            return bRetorno;
        }

        public static bool CPFValido(string cpf)
        {
            int d1, d2;
            int soma = 0;
            string digitado = "";
            string calculado = "";

            // Pesos para calcular o primeiro digito
            int[] peso1 = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            // Pesos para calcular o segundo digito
            int[] peso2 = new int[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            int[] n = new int[11];

            // Limpa a string
            cpf = cpf.Replace(".", "").Replace("-", "").Replace("/", "").Replace("\\", "");

            // Se o tamanho for < 11 entao retorna como inválido
            if (cpf.Length != 11)
                return false;

            // Caso coloque todos os numeros iguais
            String[] CPFInvalidos = {"00000000000", "11111111111", "22222222222",
            "33333333333", "44444444444", "55555555555", "66666666666", 
            "77777777777", "88888888888", "99999999999"};
            try
            {
                foreach (String CPFInvalido in CPFInvalidos)
                {
                    if (CPFInvalido.Equals(cpf))
                        return false;
                }
            }
            finally
            {
                CPFInvalidos = null;
            }

            // Quebra cada digito do CPF
            char[] matrizCPF = cpf.ToCharArray();
            try
            {
                int i = 0;
                foreach (char digito in matrizCPF)
                {
                    if ((digito < '0') || (digito > '9'))
                        return false;
                    else
                        n[i++] = Convert.ToInt32(digito.ToString());
                }
            }
            finally
            {
                matrizCPF = null;
            }


            // Calcula cada digito com seu respectivo peso
            for (int i = 0; i <= peso1.GetUpperBound(0); i++)
                soma += (peso1[i] * Convert.ToInt32(n[i]));

            // Pega o resto da divisao
            int resto = soma % 11;
            if (resto == 1 || resto == 0)
                d1 = 0;
            else
                d1 = 11 - resto;
            soma = 0;

            // Calcula cada digito com seu respectivo peso
            for (int i = 0; i <= peso2.GetUpperBound(0); i++)
                soma += (peso2[i] * Convert.ToInt32(n[i]));

            // Pega o resto da divisao
            resto = soma % 11;

            if (resto == 1 || resto == 0)
                d2 = 0;
            else
                d2 = 11 - resto;

            calculado = d1.ToString() + d2.ToString();
            digitado = n[9].ToString() + n[10].ToString();

            // Se os ultimos dois digitos calculados bater com
            // os dois ultimos digitos do cpf entao é válido
            if (calculado == digitado)
                return true;
            else
                return false;
        }

        public static bool CNPJValido(string cnpj)
        {
            cnpj = cnpj.Replace("-", "").Replace("/", "").Replace(".", "");
            if (cnpj.Length != 14)
                return false;
            string l, inx, dig;
            int s1, s2, i, d1, d2, v, m1, m2;
            inx = cnpj.Substring(12, 2);
            cnpj = cnpj.Substring(0, 12);
            s1 = 0;
            s2 = 0;
            m2 = 2;
            for (i = 11; i >= 0; i--)
            {
                l = cnpj.Substring(i, 1);
                v = Convert.ToInt16(l);
                m1 = m2;
                m2 = m2 < 9 ? m2 + 1 : 2;
                s1 += v * m1;
                s2 += v * m2;
            }
            s1 %= 11;
            d1 = s1 < 2 ? 0 : 11 - s1;
            s2 = (s2 + 2 * d1) % 11;
            d2 = s2 < 2 ? 0 : 11 - s2;
            dig = d1.ToString() + d2.ToString();

            if (inx == dig)
                return true;
            else
                return false;
        }

        // Função criada para Converter o sinal de condição da consulta SQL

        public static string ConvertSinal(string Sinal)
        {
            string RetSinal = "";

            switch (Sinal.ToString())
            {
                case "Igual": RetSinal = "=";
                    break;
                case "Diferente": RetSinal = "<>";
                    break;
                case "Menor": RetSinal = "<";
                    break;
                case "Menor Igual": RetSinal = "<=";
                    break;
                case "Maior": RetSinal = ">";
                    break;
                case "Maior Igual": RetSinal = ">=";
                    break;
                case "Entre": RetSinal = "Between";
                    break;
                case "Começando com": RetSinal = "starting with";
                    break;
                case "Na lista": RetSinal = "like";
                    break;
                case "Não Entre": RetSinal = "not in";
                    break;
            }
            return RetSinal;
        }
        // Função criada para concatenar zeros a esquerda
        public static string AlinharZerosEsquerda(string sCampo, int iCasas)
        {
            string Retorno = "";
            if (sCampo != string.Empty)
            {
                if (iCasas == 0)
                    iCasas = 1;
                for (int i = 0; i <= iCasas; i++)
                {
                    Retorno += "0";
                }
                Retorno = Retorno.Trim().Substring(0, iCasas - sCampo.Length) + sCampo;
            }
            return Retorno;
        }

    }
}
