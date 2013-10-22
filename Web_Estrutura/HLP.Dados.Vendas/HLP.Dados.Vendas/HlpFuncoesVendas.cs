using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using HLP.Dados;
using System.Web;

namespace HLP.Dados.Vendas
{

    public class HlpFuncoesVendas
    {

        private HlpFuncoesVendas()
        {
        }

        public static decimal GetPercentualComissao(string sCdVend, string sCdPrazo,
            Tabela oTabelas)
        {
            bool bVendaAPrazo = true;
            decimal rPercentual = 0;
            if ((sCdPrazo != null) && (!sCdPrazo.Equals(String.Empty)))
            {
                string sDsFormula = oTabelas.hlpDbFuncoes.qrySeekValue(
                    "PRAZOS", "DS_FORMULA", "(CD_PRAZO = '" + sCdPrazo + "')");
                if ((sDsFormula != null) && (sDsFormula.Equals("0")))
                    bVendaAPrazo = false;
            }
            if ((sCdVend != null) && (!sCdVend.Equals(String.Empty)))
            {
                string sCampoComissao;
                if (bVendaAPrazo)
                    sCampoComissao = "VL_PERCOAP";
                else
                    sCampoComissao = "VL_PERCOAV";
                DataTable qryVendedor = oTabelas.hlpDbFuncoes.qrySeekRet(
                    "VENDEDOR", sCampoComissao, "(CD_VEND = '" + sCdVend + "')");
                if (qryVendedor.Rows.Count == 1)
                    rPercentual = Convert.ToDecimal(qryVendedor.Rows[0][sCampoComissao]);
            }
            return rPercentual;
        }

        public static DataTable GetVendasPorRepresentanteAnual(Tabela oTabelas, string sAno, string sCodRepresentante = "")
        {
            System.Text.StringBuilder sQuery = new System.Text.StringBuilder();
            sQuery.Append("select coalesce(sum(p.vl_total_liberado_com_desc),0) Total,v.nm_vend, case ");
            sQuery.Append("when extract( month from p.dt_pedido) = 1 then 'Jan' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 2 then 'Fev' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 3 then 'Mar' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 4 then 'Abr' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 5 then 'Mai' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 6 then 'Jun' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 7 then 'Jul' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 8 then 'Ago' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 9 then 'Set' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 10 then 'Out' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 11 then 'Nov' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 12 then 'Dez' ");
            sQuery.Append("end as Mes from pedido p ");
            sQuery.Append("left join pedseq ps on pedido.cd_pedido = ps.cd_pedido and pedido.cd_empresa = ps.cd_empresa ");            
            sQuery.Append("inner join clifor c on p.cd_cliente = c.cd_clifor inner join tpdoc t on p.cd_tipodoc = t.cd_tipodoc ");
            sQuery.Append(" inner join vendedor v on v.cd_vend = c.cd_vend1 ");
            DataTable dtReport;

            if (sCodRepresentante != "")
            {
                sQuery.Append("where coalesce(ps.st_canped,'N') <> 'S' and coalesce(t.st_fatur,'1') = '0' and ");
                sQuery.Append("c.cd_vend1 = '{0}' and ");
                sQuery.Append("p.dt_pedido between '01.01.{1}' and '31.12.{1}' and ");
                sQuery.Append("t.st_utiliza_web = 'S' ");
                sQuery.Append("group by extract( month from p.dt_pedido), v.nm_vend order by extract( month from p.dt_pedido) ");
                string sQueryFinal = string.Format(sQuery.ToString(), sCodRepresentante, sAno);
                dtReport = oTabelas.hlpDbFuncoes.qrySeekRet(sQueryFinal);

            }
            else
            {
                sQuery.Append("where coalesce(ps.st_canped,'N') <> 'S' and coalesce(t.st_fatur,'') = 'S'and coalesce(t.st_fatur,'1') = '0' and ");
                sQuery.Append("p.st_web = 'S' and p.dt_pedido between '01.01.{0}' and '31.12.{0}' and ");
                sQuery.Append("t.st_utiliza_web = 'S' ");
                sQuery.Append("group by extract( month from p.dt_pedido) , v.nm_vend order by v.nm_vend, extract( month from p.dt_pedido) ");
                string sQueryFinal = string.Format(sQuery.ToString(), sAno);
                dtReport = oTabelas.hlpDbFuncoes.qrySeekRet(sQueryFinal);
            }
            return dtReport;
        }

        public static DataTable GetVendasProduto(Tabela oTabelas, string sTabela, string sCdProd, string sAno)
        {
            System.Text.StringBuilder sQuery = new System.Text.StringBuilder();
            sQuery.Append("select coalesce(sum(m.qt_prod),0) QTDE, case ");
            sQuery.Append("when extract( month from p.dt_pedido) = 1 then 'Jan' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 2 then 'Fev' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 3 then 'Mar' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 4 then 'Abr' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 5 then 'Mai' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 6 then 'Jun' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 7 then 'Jul' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 8 then 'Ago' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 9 then 'Set' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 10 then 'Out' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 11 then 'Nov' ");
            sQuery.Append("when extract( month from p.dt_pedido) = 12 then 'Dez' ");
            sQuery.Append("end as Mes ");
            sQuery.Append("from Movitem m ");
            sQuery.Append("inner join pedido p on p.cd_pedido = m.cd_pedido inner join tpdoc t on p.cd_tipodoc = t.cd_tipodoc  ");
            sQuery.Append("where m.cd_prod= '{1}' ");
            sQuery.Append("and  extract(year from p.dt_pedido) = '{2}'");
            sQuery.Append("and  t.st_utiliza_web = 'S' ");
            sQuery.Append("group by extract( month from p.dt_pedido)");

            DataTable dtResult;


            string sQueryFinal = string.Format(sQuery.ToString(), sTabela, sCdProd, sAno);
            dtResult = oTabelas.hlpDbFuncoes.qrySeekRet(sQueryFinal);
            return dtResult;
        }


        public static DataTable GetProdutosMaisVendidos(Tabela oTabelas, string sTabela, int iQtdeRegistros, DateTime dtInicio, DateTime dtFinal)
        {
            System.Text.StringBuilder sQuery = new System.Text.StringBuilder();
            sQuery.Append("SELECT  first({0}) sum(m.qt_prod) QTDE , m.cd_prod, prod.ds_prod DESCRICAO ");
            sQuery.Append("FROM MOVITEM m inner join pedido p on ");
            sQuery.Append("p.cd_pedido = m.cd_pedido and ");
            sQuery.Append("p.cd_empresa = m.cd_empresa ");
            sQuery.Append("inner join tpdoc t on p.cd_tipodoc = t.cd_tipodoc ");
            sQuery.Append("inner join produto prod on ");
            sQuery.Append("m.cd_prod = prod.cd_prod and ");
            sQuery.Append("m.cd_empresa = prod.cd_empresa ");
            sQuery.Append("where ");
            sQuery.Append("p.dt_pedido between '{2}' and '{3}' and p.cd_vend1 = '{4}' and ");
            sQuery.Append("t.st_utiliza_web = 'S' ");
            sQuery.Append("group by m.cd_prod, prod.ds_prod order by sum(m.qt_prod) desc ");

            

            DataTable dtResult;

            string sQueryFinal = string.Format(sQuery.ToString(), iQtdeRegistros.ToString(),
                                                                  sTabela,
                                                                  dtInicio.ToString("dd.MM.yyyy"),
                                                                  dtFinal.ToString("dd.MM.yyyy"),
                                                                  oTabelas.CdVendedorAtual
                                                                  );

            dtResult = oTabelas.hlpDbFuncoes.qrySeekRet(sQueryFinal);
            return dtResult;
        }

    }

}
