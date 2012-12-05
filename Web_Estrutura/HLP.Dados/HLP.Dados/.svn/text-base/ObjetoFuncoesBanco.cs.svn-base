using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HLP.Dados
{
    public class ObjetoFuncoesBanco
    {
        private ObjetoFuncoesBanco()
        {
        }

        public static FuncoesEspecificasBanco CriaInstancia(string sTipoBanco)
        {
            FuncoesEspecificasBanco objFuncoes = null;
            switch (sTipoBanco)
            {
                case "INTRBASE":
                    objFuncoes = new FuncoesFirebird();
                    break;

                case "MSSQL":
                    //objFuncoes = new FuncoesSql();
                    break;

                case "ORACLE":
                    // objFuncoes = new FuncoesOracle();
                    break;
            }
            return objFuncoes;
        }

    }
}
