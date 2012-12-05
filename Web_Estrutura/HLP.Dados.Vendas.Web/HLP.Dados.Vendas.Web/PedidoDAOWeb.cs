using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
//using System.Web.Configuration;
//using System.Web.SessionState;
using System.Data;
using HLP.Dados.Vendas;
using HLP.Geral;
using HLP.Web;
using System.Web.SessionState;
using System.Web.Configuration;

namespace HLP.Dados.Vendas.Web
{
    
    public class PedidoDAOWeb : PedidoDAO
    {

        private bool bTorcetex = false;//Renato - 21/07/2007 - OS 20611
        private string sCdPrazoAntigo = String.Empty;

        public PedidoDAOWeb(Tabela oTabelas)
            : base(oTabelas)
        {
            //Renato - 21/07/2007 - OS 20611
            string sCliente = oTabelas.hlpDbFuncoes.fMemoStr("0016");
            bTorcetex = ((sCliente != null) && (sCliente.Equals("TORCETEX")));
            ////////////////////////////////
        }

        protected override void GerarNovoRegistro()
        {
            string sCdPedido =
                oTabelas.FuncoesBanco.GeraNovoRegistroPedidoWeb(
                    oTabelas.CdVendedorAtual.ToString());
            RegistroAtual["CD_PEDIDO"] = sCdPedido;
            PreencherListaValoresPrimarios();
        }

        protected override bool PrevalidaCad()
        {
            bool bRetorno = base.PrevalidaCad();
            if (bRetorno)
            {
                RegistroAtual["CD_TIPODOC"] =
                    WebConfigurationManager.AppSettings["CdTipoDocPedidoDefault"];
                RegistroAtual["VL_COEF"] = 1;
                sCdPrazoAntigo = String.Empty;
            }
            return bRetorno;
        }

        protected override bool PrevalidaAlt()
        {
            bool bRetorno = base.PrevalidaAlt();
            if (bRetorno)
                sCdPrazoAntigo = RegistroAtual["CD_PRAZO"].ToString();
            return bRetorno;
        }

        protected override bool PosvalidaCad()            
        {
            bool bRetorno = base.PosvalidaCad();
            if (bRetorno)
            {
                DataTable qryClifor = oTabelas.hlpDbFuncoes.qrySeekRet(
                    "CLIFOR", "CD_CANAL, CD_TRANS, CD_REDESP", 
                    "(CD_CLIFOR = '" + RegistroAtual["CD_CLIENTE"].ToString() + "')");
                DataRow RegistroClifor = qryClifor.Rows[0];
                RegistroAtual["CD_CANAL"] = RegistroClifor["CD_CANAL"];
                //RegistroAtual["CD_TRANS"] = RegistroClifor["CD_TRANS"];
                RegistroAtual["CD_REDESP"] = RegistroClifor["CD_REDESP"];

                //Renato - 21/07/2007 - OS 20611                
                if (bTorcetex)
                {
                    string sCdPrazoNovo = RegistroAtual["CD_PRAZO"].ToString();
                    if (!sCdPrazoNovo.Equals(sCdPrazoAntigo))
                    {
                        string sVlCoef = oTabelas.hlpDbFuncoes.qrySeekValue(
                            "PRAZOS", "VL_COEF", "(CD_PRAZO = '" + sCdPrazoNovo + "')");
 
                        double rVlCoef;

           				try
				        {
					        rVlCoef = Convert.ToDouble(sVlCoef);
				        }
				        catch
				        {
					        rVlCoef = 1;
				        }
				        if (rVlCoef <= 0) 
					        rVlCoef = 1;

                        RegistroAtual["VL_COEF"] = rVlCoef;
                    }                    
                }
                ////////////////////////////////
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

        public static PedidoDAOWeb GetInstanciaPedidoDAOWeb(HttpSessionState Session,
            UsuarioWeb objUsuario)
        {
            PedidoDAOWeb objPedido = (PedidoDAOWeb)Session["ObjetoPedidoDetalhado"];
            if (objPedido == null)
            {
                objPedido = new PedidoDAOWeb(objUsuario.oTabelas);
                Session["ObjetoPedidoDetalhado"] = objPedido;
            }
            return objPedido;
        }
                                    
    }

}
