using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using HLP.Web;

public partial class PedidosFaturados : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

            string sUser = UsuarioWeb.GetNomeUsuarioConectado(Session);
            if (sUser == "")
            {
                Response.Redirect("~/Login.aspx");
            }

            string sCodigoPedido = Request.QueryString["CD_PEDIDO"].ToString().Trim();
            string sCodigoEmpresa = objUsuario.oTabelas.sEmpresa.Trim();
            string sCodigoVendedor = objUsuario.CodigoVendedor.Trim();



            //StringBuilder str = new StringBuilder();
            //str.Append("SELECT P.CD_EMPRESA, P.CD_PEDIDO, NF.CD_NFSEQ, NF.DT_EMI, MOVI.NR_LANC, ");
            //str.Append("CASE ");
            //str.Append("WHEN (MOVI.CD_DOC LIKE '______') THEN NF.CD_NOTAFIS ELSE '******' END AS NOTAFISCAL, ");
            //str.Append("MOVI.CD_ALTER, MOVI.DS_PROD ");
            //str.Append("FROM PEDIDO P ");
            //str.Append("LEFT OUTER JOIN MOVITEM MOVI ON (MOVI.CD_EMPRESA = P.CD_EMPRESA) AND (MOVI.CD_PEDIDO = P.CD_PEDIDO) ");
            //str.Append("LEFT OUTER JOIN NF ON (NF.CD_NFSEQ = MOVI.CD_NFSEQ) AND (NF.CD_EMPRESA = MOVI.CD_EMPRESA) ");
            //str.Append("WHERE (P.CD_EMPRESA = '" + sCodigoEmpresa + "') AND (PEDIDO.CD_VEND1 = '" + sCodigoVendedor + "') AND (MOVI.NR_LANC IS NOT NULL) AND ");
            //str.Append("(NF.CD_NFSEQ IS NOT NULL) AND (PEDIDO.CD_PEDIDO = '" + sCodigoPedido + "') ");
            //str.Append("ORDER BY NF.DT_EMI");

          


            //DataTable dtPedFaturado = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(str.ToString());
            //if (dtPedFaturado.Rows.Count > 0)
            //{
            //    lblPedido.Text = "Pedido: " + sCodigoPedido;
            //    lblCliente.Text = "Cliente: " + objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("PEDIDO", "NM_GUERRA", "(CD_PEDIDO = '" + sCodigoPedido + "')");
            //    DataGrid1.DataSource = dtPedFaturado;
            //    DataGrid1.DataBind();
            //}
            //else
            //{
            //    Response.Redirect("~/Msg.aspx");

            //}
        }
    }
    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/ConsultaPedidos.aspx");
    }
}
