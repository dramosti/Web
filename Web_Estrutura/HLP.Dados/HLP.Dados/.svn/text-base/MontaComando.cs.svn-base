using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace HLP.Dados
{
    public enum TipoComando
    {
        Insert,
        Update,
        Delete
    }

    public class MontaComando
    {

        private MontaComando()
        {
        }

        public static string MontaExpressaoSql(TipoComando TipoExpressao,
            string sTabela, string sWhere, List<string> sCamposDesconsiderados,
            bool bDesconsideraNullUpdate, DataRow registro)
        {
            StringBuilder strExpressao = new StringBuilder();
            StringBuilder str1 = new StringBuilder();
            StringBuilder str2 = null;
            string sValor;
            bool bUsaWhere = (!sWhere.Trim().Equals(String.Empty));
            bool bContinua;

            if (TipoExpressao == TipoComando.Insert)
            {
                strExpressao.Append("INSERT INTO " + sTabela);
                str2 = new StringBuilder();
            }
            else
            {
                if (bUsaWhere)
                {
                    if (TipoExpressao == TipoComando.Update)
                        strExpressao.Append("UPDATE " + sTabela + " SET ");
                    else if (TipoExpressao == TipoComando.Delete)
                        strExpressao.Append("DELETE FROM " + sTabela);
                }
            }

            if (strExpressao.Length == 0)
                return String.Empty;

            if ((TipoExpressao == TipoComando.Insert) ||
                (TipoExpressao == TipoComando.Update))
            {
                string sCampo;
                foreach (DataColumn campo in registro.Table.Columns)
                {
                    sCampo = campo.ColumnName;
                    if (sCampo.StartsWith("CAMPO"))
                        continue;
                    if ((sCamposDesconsiderados != null) &&
                        (sCamposDesconsiderados.Contains(sCampo)))
                        continue;
                    if (campo.DataType.Equals(typeof(System.Byte[])))
                        continue;

                    sValor = HlpDbFuncoesGeral.RetornaStrValor(registro[campo], campo);
                    if (TipoExpressao == TipoComando.Insert)
                    {
                        //Renato - 25/09/2003 - OS 10603
                        if (!sValor.Equals("NULL"))
                        {
                            if (str1.Length > 0)
                                str1.Append(',');
                            str1.Append(sCampo);
                            if (str2.Length > 0)
                                str2.Append(',');
                            str2.Append(sValor);
                        }
                        ////////////////////////////////
                    }
                    else
                    {
                        bContinua = (!bDesconsideraNullUpdate);
                        if (!bContinua)
                            bContinua = (!sValor.Equals("NULL"));
                        if (bContinua)
                        {
                            if (str1.Length > 0)
                                str1.Append(',');
                            str1.Append(" " + sCampo + " = " + sValor);
                        }
                    }
                }
            }

            if (TipoExpressao == TipoComando.Insert)
            {
                if ((str1.Length > 0) && (str2.Length > 0))
                {
                    strExpressao.Append(" (" + str1.ToString() + ") VALUES (");
                    strExpressao.Append(str2.ToString());
                    strExpressao.Append(")");
                }
                else
                    strExpressao.Length = 0;
            }
            else if (TipoExpressao == TipoComando.Update)
            {
                if (str1.Length > 0)
                    strExpressao.Append(str1.ToString());
                else
                    strExpressao.Length = 0;
            }

            if ((strExpressao.Length > 0) && (bUsaWhere))
                strExpressao.Append(" WHERE " + sWhere);
            return strExpressao.ToString();
        }

    }

}
