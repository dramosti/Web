using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using HLP.Dados;
using HLP.Geral;


namespace HLP.Dados.Vendas
{
    
    public abstract class PedidoDAO : BaseDAO
    {

        public PedidoDAO(Tabela oTabelas)
            : base(oTabelas)
        {
            this.Tabela = "PEDIDO";
            this.ChavePrimaria.Add("CD_EMPRESA");
            this.ChavePrimaria.Add("CD_PEDIDO");
            this.Inicializar();
            
            //Cria objeto para o MOVIPEND
            MovipendPedidoDAO objMovipendDAO = new MovipendPedidoDAO(oTabelas);
            EstruturaFilha MovipendPedido = new EstruturaFilha();
            MovipendPedido.PaiDAO = this;
            MovipendPedido.ObjetoDAO = objMovipendDAO;
            MovipendPedido.CamposRelacionamento.Add(new Relacionamento("CD_EMPRESA", "CD_EMPRESA"));
            MovipendPedido.CamposRelacionamento.Add(new Relacionamento("CD_PEDIDO", "CD_PEDIDO"));
            this.DadosFilhos.Add("MOVIPEND", MovipendPedido);
            
            //Cria objeto para o MOVITEM
            MovitemPedidoDAO objMovitemDAO = new MovitemPedidoDAO(oTabelas);
            EstruturaFilha MovitemPedido = new EstruturaFilha();
            MovitemPedido.PaiDAO = this;
            MovitemPedido.ObjetoDAO = objMovitemDAO;
            MovitemPedido.CamposRelacionamento.Add(new Relacionamento("CD_EMPRESA", "CD_EMPRESA"));
            MovitemPedido.CamposRelacionamento.Add(new Relacionamento("CD_PEDIDO", "CD_PEDIDO"));
            this.DadosFilhos.Add("MOVITEM", MovitemPedido);
        }

        protected override bool Exclusao()
        {
            string sCdPedido = RegistroAtual["CD_PEDIDO"].ToString();
            DataTable qrymovitem = oTabelas.hlpDbFuncoes.qrySeekRet(
                "MOVITEM", "DISTINCT CD_PEDIDO",
                "(CD_PEDIDO = '" + sCdPedido + "')");
            if (qrymovitem.Rows.Count > 0)
            {
                SetErro("Impossível a exclusão!!! Já existem\n" +
                    "liberações para o pedido " + sCdPedido + ")");
                return false;
            }
            else
                return true;
        }

        protected override bool PrevalidaAlt()
        {
            RegistroAtual["DT_ATUCAD"] = DateTime.Now.Date;
            RegistroAtual["CD_USUALT"] = oTabelas.CdUsuarioAtual;
            return true;
        }

        protected override bool PrevalidaCad()
        {
            DateTime dDataAtual = DateTime.Now.Date;
            RegistroAtual["DT_PEDIDO"] = dDataAtual;
            RegistroAtual["DT_ABER"] = dDataAtual;
            RegistroAtual["CD_USUINC"] = oTabelas.CdUsuarioAtual;
            RegistroAtual["CD_VEND1"] = oTabelas.CdVendedorAtual;
            
            // Inicio OS - 0020693.
            // Acrescentado 2º Representante ao pedido.
            
            RegistroAtual["CD_VEND2"] = oTabelas.hlpDbFuncoes.qrySeekValue("VENDEDOR", "CD_SEGVEND", "CD_VEND = '" + oTabelas.CdVendedorAtual + "'");

            // Fim OS - 0020693.

            // Inicio OS - 0020753.
            // Deixar RodoMago como transportadora padrão.
            RegistroAtual["CD_TRANS"] = oTabelas.TRANSP.ToString();
           
            // Fim OS - 0020753.
            
            return true;
        }

        protected override bool PosvalidaCad()
        {
            return true;
        }      

        private double GetValorReservado()
        {            
            return GetValorReservado(false);
        }

        private double GetValorReservado(bool bComDescontoTorcetex)
        {
            string sCampo;
            if (!bComDescontoTorcetex)
                sCampo = "SUM(VL_TOTLIQ) AS VL_TOTLIQ";
            else
            {
                sCampo = "SUM(" +
                    "CAST (((CASE WHEN VL_COEFDESC < 1 " +
                               "THEN VL_COEFDESC " +
                               "ELSE 1 " +
                           "END) * " +
                          "(QT_PROD * VL_UNIPROD)) " +
                          "AS NUMERIC(18,2) " +
                         ")) AS " +
                    "VL_TOTLIQ";
            }
            DataTable dtMovipend =
                oTabelas.hlpDbFuncoes.qrySeekRet("MOVIPEND",
                sCampo,
                "(CD_PEDIDO = '" + this.RegistroAtual["CD_PEDIDO"] + "')");
            double valor = 0;
            if (dtMovipend.Rows.Count == 1)
            {
                try
                {
                    valor = Convert.ToDouble(dtMovipend.Rows[0]["VL_TOTLIQ"]);
                }
                catch
                {
                }
            }
            return valor;
        }

        private double GetValorLiberado()
        {
            return GetValorLiberado(false);
        }

        private double GetValorLiberado(bool bComDescontoTorcetex)
        {
            string sCampo;
            if (!bComDescontoTorcetex)
                sCampo = "SUM(VL_TOTLIQ) AS VL_TOTLIQ";
            else
            {
                sCampo = "SUM(" +
                    "CAST (((CASE WHEN VL_COEFDESC < 1 " +
                               "THEN VL_COEFDESC " +
                               "ELSE 1 " +
                           "END) * " +
                          "(QT_PROD * VL_UNIPROD)) " +
                          "AS NUMERIC(18,2) " +
                         ")) AS " +
                    "VL_TOTLIQ";
            }
            DataTable dtMovitem =
                oTabelas.hlpDbFuncoes.qrySeekRet("MOVITEM",
                sCampo,
                "(CD_PEDIDO = '" + this.RegistroAtual["CD_PEDIDO"] + "')");
            double valor = 0;
            if (dtMovitem.Rows.Count == 1)
            {
                try
                {
                    valor = Convert.ToDouble(dtMovitem.Rows[0]["VL_TOTLIQ"]);
                }
                catch
                {
                }
            }
            return valor;
        }

        public string GetTotalReservado()
        {
            double rTotalReservado;
            try
            {
                rTotalReservado = this.GetValorReservado();
            }
            catch
            {
                rTotalReservado = 0;
            }
            return rTotalReservado.ToString("N2");
        }

        public string GetTotalLiberado()
        {
            double rTotalLiberado;
            try
            {
                rTotalLiberado = this.GetValorLiberado();
            }
            catch
            {
                rTotalLiberado = 0;
            }
            return rTotalLiberado.ToString("N2");
        }

        public string GetTotalPedido()
        {
            double rTotal;
            try
            {
                rTotal = this.GetValorLiberado() + 
                    this.GetValorReservado();
            }
            catch
            {
                rTotal = 0;
            }
            return rTotal.ToString("N2");
        }

        public double GetValorDescontoTorcetex()
        {
            return (GetValorReservado(false) - GetValorReservado(true)) +
                   (GetValorLiberado(false) - GetValorLiberado(true));

        }

        public double GetValorTotalPedidoTorcetex()
        {
            return (this.GetValorLiberado() +
                    this.GetValorReservado() -
                    this.GetValorDescontoTorcetex());
        }

    }

}
