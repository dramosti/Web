using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using HLP.Dados;
using HLP.Geral;
using HLP.Dados.Faturamento;

namespace HLP.Dados.Vendas
{

    public class MovipendPedidoDAO : BaseDAO
    {

        private bool bTorcetex = false;

        public MovipendPedidoDAO(Tabela oTabelas) : base(oTabelas)
        {
            //Renato - 21/07/2007 - OS 20611
            string sCliente = oTabelas.hlpDbFuncoes.fMemoStr("0016");
            bTorcetex = ((sCliente != null) && (sCliente.Equals("TORCETEX")));
            ////////////////////////////////

            this.Tabela = "MOVIPEND";
            this.ChavePrimaria.Add("CD_EMPRESA");
            this.ChavePrimaria.Add("NR_LANC");
            this.Inicializar();            
        }

        protected override bool Exclusao()
        {
            return true;
        }

        protected override bool PrevalidaAlt()
        {
            RegistroAtual["DT_ATUCAD"] = DateTime.Now.Date;
            RegistroAtual["CD_USUALT"] = oTabelas.CdUsuarioAtual;
            return true;
        }

        private void PreencheCdLista()
        {
            string ValorCdLista = oTabelas.hlpDbFuncoes.qrySeekValue("PRAZOS",
                "CD_LISTA", "(CD_PRAZO = '" +    
                GetRegistroPai()["CD_PRAZO"].ToString() + "')");
            if ((ValorCdLista != null) && (!ValorCdLista.Equals(String.Empty)))
                RegistroAtual["CD_LISTA"] = ValorCdLista;
        }

        protected override bool PrevalidaCad()
        {
            DataColumnCollection CamposMovipend = EstruturaDataTable.Columns;
            DataRow RegistroPedido = GetRegistroPai();
            DateTime dDataAtual = DateTime.Now.Date;
            RegistroAtual["CD_OPER"] = oTabelas.GetOperacaoDefault(
                RegistroPedido["CD_TIPODOC"].ToString());
            RegistroAtual["DT_PRAZOEN"] = dDataAtual;
            RegistroAtual["DT_LANC"] = dDataAtual;
            RegistroAtual["CD_USUINC"] = oTabelas.CdUsuarioAtual;
            if (CamposMovipend.Contains("CD_CLIENTE"))
                RegistroAtual["CD_CLIENTE"] = RegistroPedido["CD_CLIENTE"];
            if (CamposMovipend.Contains("CD_VEND1"))
                RegistroAtual["CD_VEND1"] = RegistroPedido["CD_VEND1"];
            if (CamposMovipend.Contains("CD_VEND2"))
                RegistroAtual["CD_VEND2"] = RegistroPedido["CD_VEND2"];
            if (CamposMovipend.Contains("VL_PERENTR"))
                RegistroAtual["VL_PERENTR"] = 100;

            RegistroAtual["QT_PROD"] = 0; 
            RegistroAtual["VL_TOTLIQ"] = 0;

            double rVlCoef = Convert.ToDouble(RegistroPedido["VL_COEF"]);
            if (bTorcetex)
            {
                RegistroAtual["VL_COEFDESC"] = rVlCoef;
                if (rVlCoef < 1)
                    rVlCoef = 1;
            }
            RegistroAtual["VL_COEF"] = rVlCoef;            
            
            RegistroAtual["CD_PEDCLI"] = RegistroPedido["DS_PEDCLI"];
            PreencheCdLista();

            RegistroAtual["VL_PERCOMI1"] = HlpFuncoesVendas.GetPercentualComissao(
                RegistroPedido["CD_VEND1"].ToString(),
                RegistroPedido["CD_PRAZO"].ToString(),
                oTabelas);
            
            // Inicio OS - 0020693.
            // Acrescentado comissao do 2º Representante ao pedido.

            RegistroAtual["VL_PERCOMI2"] = HlpFuncoesVendas.GetPercentualComissao(
                RegistroPedido["CD_VEND2"].ToString(),
                RegistroPedido["CD_PRAZO"].ToString(),
                oTabelas);

            // Fim OS - 0020693.

            RegistroAtual["DT_LANC"] = DateTime.Now.Date;
            RegistroAtual["CD_USUINC"] = oTabelas.CdUsuarioAtual;
            return true;
        }      

        protected override bool PosvalidaCad()
        {
            bool bRetorno = true;
            if (bRetorno)
            { 
                DataRow RegistroPedido = GetRegistroPai();
                DateTime dDtAtucad = DateTime.Now.Date;
                RegistroPedido["DT_ATUCAD"] = dDtAtucad;
                oTabelas.hlpDbFuncoes.SqlCommand(
                    "UPDATE PEDIDO SET " + 
                    "CD_USUALT = '" + oTabelas.CdUsuarioAtual + "', " + 
                    "DT_ATUCAD = " +                     
                    HlpDbFuncoesGeral.RetornaStrValor(dDtAtucad,
                    RegistroPedido.Table.Columns["DT_ATUCAD"]) + " " +
                    "WHERE (CD_EMPRESA = '" + oTabelas.sEmpresa + "') AND " +
                    "(CD_PEDIDO = '" + RegistroAtual["CD_PEDIDO"].ToString() + "')");
            }
            return bRetorno;
        }

        protected override bool Valid(ITela oCampo)
        {
            return true;
        }

        protected override bool When(ITela oCampo)
        {
            return true;
        }

        protected override void GerarNovoRegistro()
        {
            string sNrLanc = "-1";
            if (oTabelas.GetPlataformaUtilizada() == PlataformaUtilizada.Web)
            {
                sNrLanc =
                    oTabelas.FuncoesBanco.GeraNovoRegistroMovipendWeb(
                    RegistroAtual["CD_PEDIDO"].ToString());
            }
            RegistroAtual["NR_LANC"] = sNrLanc;
            RegistroAtual["NR_ORDEM"] = 
                oTabelas.hlpDbFuncoes.qrySeekValue("MOVIPEND", "NR_ORDEM",
                "(NR_LANC = '" + sNrLanc + "')");
            PreencherListaValoresPrimarios();
        }

        public void CarregarInformacoesProduto(string sCdProd)
        {
            if ((sCdProd == null) || (sCdProd.Equals(String.Empty)))
                return;
            RegistroAtual["CD_PROD"] = sCdProd;
            DataTable qryProduto = oTabelas.hlpDbFuncoes.qrySeekRet(
                "PRODUTO", "DS_DETALHE, DS_PROD, CD_SITTRIB, CD_TPUNID, " + 
                "CD_CF, CD_ALIICMS, CD_ALTER", 
                "(CD_PROD = '" + sCdProd + "')");
            if (qryProduto.Rows.Count == 1)
            {
                DataRow RegProduto = qryProduto.Rows[0];
                RegistroAtual["DS_PROD"] = RegProduto["DS_DETALHE"];
                if ((RegistroAtual["DS_PROD"] == null) ||
                    (RegistroAtual["DS_PROD"].ToString().Trim().Equals(String.Empty)))
                    RegistroAtual["DS_PROD"] = RegProduto["DS_PROD"];
                RegistroAtual["CD_SITTRIB"] = RegProduto["CD_SITTRIB"];
                RegistroAtual["CD_TPUNID"] = RegProduto["CD_TPUNID"];
                RegistroAtual["CD_CF"] = RegProduto["CD_CF"];
                RegistroAtual["CD_ALIICMS"] = RegProduto["CD_ALIICMS"];
                RegistroAtual["CD_ALTER"] = RegProduto["CD_ALTER"];
            }

            PreencheCdLista();            
            DataTable qryPrecos = oTabelas.hlpDbFuncoes.qrySeekRet(
                "PRECOS", "VL_PRECOVE",
                "(CD_PROD = '" + sCdProd + "') AND " +
                "(CD_LISTA = '" + RegistroAtual["CD_LISTA"].ToString() + "')");
            if (qryPrecos.Rows.Count == 1)
                RegistroAtual["VL_UNIPROD"] = qryPrecos.Rows[0]["VL_PRECOVE"];
            else
                RegistroAtual["VL_UNIPROD"] = 0;    

            RegistroAtual["VL_ALIIPI"] = HlpFuncoesFaturamento.GetAliquotaIPI(
                RegistroAtual["CD_OPER"].ToString(),
                GetRegistroPai()["CD_CLIENTE"].ToString(),
                RegistroAtual["CD_CF"].ToString(),
                oTabelas);

            CalcularTotalItem();
        }

        public void CalcularTotalItem() 
        {
            decimal rVlTotliq = Decimal.Round(
                Convert.ToDecimal(RegistroAtual["QT_PROD"]) *
                Convert.ToDecimal(RegistroAtual["VL_UNIPROD"]) *
                Convert.ToDecimal(RegistroAtual["VL_COEF"]), 2);
            RegistroAtual["VL_TOTLIQ"] = rVlTotliq;                
        }

    }

}