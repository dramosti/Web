using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using HLP.Geral;

namespace HLP.Dados
{
    public class FuncoesFirebird : FuncoesEspecificasBanco
    {
        public override string GetStringConexao(string Servidor, string BancoDados,
           string Usuario, string Senha, string sPort)
        {
            StringBuilder str = new StringBuilder();

            if (sPort != "")
            {
                str.Append("Port=" + sPort + ";");
            }
            str.Append("Database=" + BancoDados + ";");
            str.Append("User=" + Usuario + ";");
            str.Append("Password=" + Senha + ";");

            string sDialeto = BancoDados.ToUpper();
            if ((sDialeto.IndexOf(".FDB") >= 0) ||
                (sDialeto.IndexOf("E0") < 0))
                sDialeto = "3";
            else
                sDialeto = "1";
            str.Append("Dialect=" + sDialeto + ";");
            str.Append("Connection lifetime=3600");

            if (!Servidor.Equals(String.Empty))
                str.Append(";Server=" + Servidor);

            return str.ToString();
        }

        private IDataReader GetDataReaderExistenciaObjeto(StringBuilder strExpressao,
            IDbConnection conexao)
        {
            IDbCommand comando = (IDbCommand)HlpFuncoes.getObjeto(
                GetDriverBaseDados(),
                GetClasseDbCommandBaseDados());
            comando.Connection = conexao;
            comando.CommandText = strExpressao.ToString();

            IDataReader dr = comando.ExecuteReader();
            return dr;
        }

        protected override bool ExisteTabelaBancoEspecifico(string sTabela,
            IDbConnection conexao)
        {
            StringBuilder str = new StringBuilder();
            str.Append("SELECT RDB$RELATION_NAME FROM RDB$RELATIONS ");
            str.Append("WHERE (RDB$RELATION_NAME = '");
            str.Append(sTabela);
            str.Append("')");

            IDataReader dr = GetDataReaderExistenciaObjeto(str, conexao);
            return dr.Read();
        }

        protected override bool ExisteCampoBancoEspecifico(string sTabela,
            string sCampo, IDbConnection conexao)
        {
            StringBuilder str = new StringBuilder();
            str.Append("SELECT RDB$FIELD_NAME FROM RDB$RELATION_FIELDS ");
            str.Append("WHERE (RDB$RELATION_NAME = '");
            str.Append(sTabela);
            str.Append("') AND ");
            str.Append("(RDB$FIELD_NAME = '");
            str.Append(sCampo);
            str.Append("')");

            IDataReader dr = GetDataReaderExistenciaObjeto(str, conexao);
            return dr.Read();
        }

        public override string GetExpressaoExclusaoProcedure(string sProcedure)
        {
            return "DROP PROCEDURE " + sProcedure;
        }

        public override string GetExpressaoCriacaoProcedure(string sProcedure, string sCorpoProcedure)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string GetExpressaoAlteracaoProcedure(string sProcedure, string sCorpoProcedure)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string GetDriverBaseDados()
        {
            return "FirebirdSql.Data.FirebirdClient, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3750abcc3150b00c";
        }

        public override string GetClasseConexaoBaseDados()
        {
            return "FirebirdSql.Data.FirebirdClient.FbConnection";
        }

        public override string GetClasseDbCommandBaseDados()
        {
            return "FirebirdSql.Data.FirebirdClient.FbCommand";
        }

        public override string GetClasseDbDataAdapterBaseDados()
        {
            return "FirebirdSql.Data.FirebirdClient.FbDataAdapter";
        }

        public override string GeraNovoRegistroPedidoWeb(string sCdVend1)
        {
            DataTable qryPedido =
                oTabelas.hlpDbFuncoes.qrySeekRet(
                "SELECT CD_PEDIDO FROM SP_INCLUI_PEDIDO_WEB('" +
                oTabelas.sEmpresa + "', '" + sCdVend1 + "')");
            return (qryPedido.Rows[0]["CD_PEDIDO"]).ToString();
        }


        public override string GeraNovoRegistroMovipendWeb(string sCdPedido)
        {
            DataTable qryMovitem =
                oTabelas.hlpDbFuncoes.qrySeekRet(
                "SELECT NR_LANC FROM SP_INCLUI_MOVIPEND_WEB('" +
                oTabelas.sEmpresa + "', '" + sCdPedido + "')");
            return (qryMovitem.Rows[0]["NR_LANC"]).ToString();
        }

        public override string GeraNovoRegistroCliforWeb(string sCdVend1)
        {
            DataTable qryClifor =
                oTabelas.hlpDbFuncoes.qrySeekRet(
                "SELECT CD_CLIFOR FROM SP_INCLUI_CLIENTE_WEB('" + sCdVend1 + "')");
            return (qryClifor.Rows[0]["CD_CLIFOR"]).ToString();
        }

    }
}
