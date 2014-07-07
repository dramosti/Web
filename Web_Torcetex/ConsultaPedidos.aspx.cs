using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HLP.Web;
using HLP.Dados.Vendas.Web;
using System.Drawing;
using HLP.Dados.Vendas;
using System.Text;
using HLP.Dados;
using System.Web.Configuration;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class ConsultaPedidos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sUser = UsuarioWeb.GetNomeUsuarioConectado(Session);
        if (sUser == "")
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!Page.IsPostBack)
        {
            string sTabela = WebConfigurationManager.AppSettings["TableItens"];
            if (sTabela.ToUpper() == "MOVIPEND")
            {
                gridConsultaPedidos.Columns[5].Visible = false;
            }
            BaseDAO.CancelarOperacaoObjetoDAO((BaseDAO)Session["ObjetoPedidoDetalhado"]);
            ParametroPesquisa objParametros = (ParametroPesquisa)Session["FiltroPedidos"];
            bool bParametrosValidos = (objParametros != null);
            if (bParametrosValidos)
                bParametrosValidos = (!objParametros.AindaNaoDefiniuFiltro());
            if (!bParametrosValidos)
            {
                Response.Redirect("~/PesquisarPedidos.aspx");
                return;
            }

            PesquisaDadosPedido(objParametros);
        }
    }

    protected void gridConsultaPedidos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridConsultaPedidos.PageIndex = e.NewPageIndex;
        ProcessaDataBind();
    }

    private void ProcessaDataBind()
    {
        gridConsultaPedidos.DataSource = (DataTable)Session["DadosConsultaPedidos"];
        gridConsultaPedidos.DataBind();
    }

    protected void lblVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/PesquisarPedidos.aspx");
    }

    private void PesquisaDadosPedido(ParametroPesquisa objParametros)
    {
        try
        {
            UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
            DataTable dtPedidos = (DataTable)Session["DadosConsultaPedidos"];
            bool bPesquisarDados = (dtPedidos == null);
            if (bPesquisarDados)
            {
                StringBuilder squery = new StringBuilder();
                squery.Append("SELECT distinct pedido.vl_totalped,  ");
                squery.Append("pedido.dt_pedido DT_DOC, ");
                squery.Append("pedido.cd_empresa, ");
                squery.Append("pedido.cd_vend1 CD_VEND, ");
                squery.Append("pedido.cd_cliente CD_CLIFOR, ");
                squery.Append("pedido.cd_pedido, ");
                squery.Append("pedido.nm_guerra ");
                squery.Append("FROM pedido left join pedseq ps on pedido.cd_pedido = ps.cd_pedido and pedido.cd_empresa = ps.cd_empresa ");
                squery.Append("where {0} ORDER BY pedido.CD_PEDIDO");
                //squery.Append("where p.dt_pedido between ('{0}') and ('{1}') ");
                //squery.Append("and p.cd_vend1 = '{2}' ");
                //squery.Append("and p.cd_empresa = '{3}' and coalesce(ps.st_canped,'N') <> 'S' ");

                dtPedidos = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(string.Format(squery.ToString(), objParametros.GetWhere()));
                DataColumn[] ChavePrimaria = new DataColumn[] { dtPedidos.Columns["CD_PEDIDO"] };
                dtPedidos.PrimaryKey = ChavePrimaria;
                Session["DadosConsultaPedidos"] = dtPedidos;
            }
            if (dtPedidos.Rows.Count == 0)
                MessageHLP.ShowPopUpMsg("Não existem registros no período selecionado", this.Page);
            if (!Page.IsPostBack)
                ProcessaDataBind();

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gridConsultaPedidos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string sTabela = WebConfigurationManager.AppSettings["TableItens"];
        if (e.CommandName == "Pedido")
        {
            int iCountFaturados = 0;
            int iCountItens = 0;
            string sNF = "";
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gridConsultaPedidos.Rows[index];
            string sCodigoPedido = row.Cells[0].Text;
            UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
            string sCodigoEmpresa = objUsuario.oTabelas.sEmpresa.Trim();
            string sCodigoVendedor = objUsuario.CodigoVendedor.Trim();
            if (sTabela == "MOVITEM")
            {
                string str = string.Format("select count(cd_pedido)TOTAL from {0} m where coalesce(m.cd_pedido,'0000000') = '{1}'and m.cd_empresa = '{2}'", sTabela, sCodigoPedido, sCodigoEmpresa);
                DataTable dtCountItens = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(str);
                iCountItens = Convert.ToInt32(dtCountItens.Rows[0]["TOTAL"].ToString());

                str = string.Format("select count(m.cd_docorigem)TOTAL, nr_lanc from {0} m where coalesce(m.cd_docorigem,'0000000') = '{1}' and m.cd_empresa = '{2}' group by nr_lanc", sTabela, sCodigoPedido, sCodigoEmpresa);
                dtCountItens = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(str);


                if (dtCountItens.Rows.Count > 0)
                {
                    iCountFaturados = Convert.ToInt32(dtCountItens.Rows[0]["TOTAL"].ToString());
                    sNF = dtCountItens.Rows[0]["nr_lanc"].ToString();
                }

                if (iCountItens > 0)
                {
                    if (iCountItens == iCountFaturados)
                    {
                        MessageHLP.ShowPopUpMsg("Pedido se encontra faturado! NF:" + sNF, this.Page);
                    }
                    else
                    {
                        MessageHLP.ShowPopUpMsg("Pedido ainda não foi faturado!", this.Page);
                    }
                }
            }
            else
            {
                string str = string.Format("select count(cd_pedido)TOTAL from MOVIPEND m where coalesce(m.cd_pedido,'0000000') = '{1}'and m.cd_empresa = '{2}'", sTabela, sCodigoPedido, sCodigoEmpresa);
                DataTable dtCountItens = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(str);
                iCountItens = Convert.ToInt32(dtCountItens.Rows[0]["TOTAL"].ToString());

                str = string.Format("select count(cd_pedido)TOTAL from MOVITEM m where coalesce(m.cd_pedido,'0000000') = '{1}'and m.cd_empresa = '{2}' ", sTabela, sCodigoPedido, sCodigoEmpresa);
                dtCountItens = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(str);


                if (dtCountItens.Rows.Count > 0)
                {
                    iCountFaturados = Convert.ToInt32(dtCountItens.Rows[0]["TOTAL"].ToString());
                }

                if (iCountItens == 0)
                {
                    //faturado
                    MessageHLP.ShowPopUpMsg("Pedido se encontra faturado!", this.Page);
                }
                else if (iCountFaturados == 0)
                {
                    // não faturado
                    MessageHLP.ShowPopUpMsg("Pedido ainda não foi faturado!", this.Page);
                }
                else if (iCountItens > 0 && iCountFaturados > 0)
                {
                    // parcial                    
                    MessageHLP.ShowPopUpMsg("Pedido parcialmente faturado! ", this.Page);
                }

            }
        }
        else if (e.CommandName == "Email")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gridConsultaPedidos.Rows[index];
            string sCodigoPedido = row.Cells[0].Text;
            Response.Redirect("~/Informativo.aspx?CD_PEDIDO_EMAIL=" + sCodigoPedido);

        }
    }


    private void PesquisarDados( string sNameFile)
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

        DataTable dtClientes = new DataTable();
        StringBuilder str = new StringBuilder();
        str.Append("SELECT PEDIDO.CD_EMPRESA, PEDIDO.CD_PEDIDO, ");
        str.Append("PEDIDO.CD_CLIENTE, ");
        str.Append("PEDIDO.CD_TIPODOC, PEDIDO.CD_TRANS, ");
        str.Append("PEDIDO.DS_ANOTA, ");
        str.Append("PEDIDO.DS_PEDCLI CD_PEDCLI, PEDIDO.DT_ABER, ");
        str.Append("PEDIDO.DT_PEDIDO, ");
        str.Append("PEDIDO.CD_VEND1, PEDIDO.CD_VEND2, ");
        str.Append("EMPRESA.IM_EMPRESA, EMPRESA.NM_EMPRESA, ");
        str.Append("EMPRESA.NM_BAIRRONOR NM_BAIRROEMP, EMPRESA.NM_CIDNOR NM_CIDEMP, ");
        str.Append("EMPRESA.DS_ENDNOR DS_ENDEMP, EMPRESA.CD_UFNOR CD_UFEMP, EMPRESA.CD_CEPNOR CD_CEPEMP, ");
        str.Append("EMPRESA.CD_CGC CD_CGCEMP, EMPRESA.CD_INSEST CD_INSESTEMP, EMPRESA.CD_FONENOR CD_FONEEMP, ");
        str.Append("EMPRESA.CD_FAXNOR CD_FAXEMP, EMPRESA.CD_EMAIL CD_EMAILEMP, ");
        str.Append("VENDEDOR.NM_GUERRA, VENDEDOR.NM_VEND NM_VENDEDOR, VENDEDOR.CD_FONE CD_FONEVEND, ");
        str.Append("PEDIDO.CD_PRAZO, PRAZOS.DS_PRAZO, ");
        str.Append("TRANSPOR.NM_TRANS, ");
        str.Append("TRANSPOR.CD_TRANS, PEDIDO.CD_FONETRANS_WEB, PEDIDO.NM_TRANS_WEB, ");
        str.Append("CLIFOR.NM_CLIFOR, CLIFOR.DS_ENDNOR DS_ENDCLI, ");
        str.Append("CLIFOR.NM_BAIRRONOR NM_BAIRROCLI, CLIFOR.NM_CIDNOR NM_CIDCLI, ");
        str.Append("CLIFOR.CD_UFNOR CD_UFCLI, CLIFOR.CD_CEPNOR CD_CEPCLI, ");
        str.Append("CLIFOR.CD_FONENOR CD_FONECLI, CLIFOR.CD_FAXNOR CD_FAXCLI, ");
        str.Append("CLIFOR.CD_CGC CD_CGCCLI, CLIFOR.CD_INSEST CD_INSESTCLI, CLIFOR.DS_CONTATO DS_CONTATOCLI, ");
        str.Append("ENDENTR.DS_ENDENT, ENDENTR.NM_BAIRROENT, ");
        str.Append("ENDENTR.NM_CIDENT, ENDENTR.CD_CEPENT, ");
        str.Append("PEDIDO.DS_OBS_WEB, "); //25989 - DIEGO
        str.Append("ENDENTR.CD_UFENT, ");
        str.Append("MOVIPEND.CD_PROD, ");
        str.Append("MOVIPEND.VL_UNIPROD, MOVIPEND.VL_TOTLIQ, ");
        str.Append("MOVIPEND.DT_PRAZOEN, MOVIPEND.CD_OPER, ");
        str.Append("MOVIPEND.CD_TPUNID, MOVIPEND.QT_PROD, MOVIPEND.DS_PROD, ");
        str.Append("MOVIPEND.CD_PEDCLI, MOVIPEND.CD_ALTER, ");
        str.Append("CAST (MOVIPEND.QT_PROD * MOVIPEND.VL_UNIPROD AS NUMERIC(18,2)) AS ");
        str.Append("VL_TOTBRUTO, ");
        str.Append("CAST((cast( MOVIPEND.VL_UNIPROD*MOVIPEND.VL_COEF as numeric(18,4))*(1-MOVIPEND.VL_PERDESC/100))* MOVIPEND.QT_PROD AS NUMERIC(18,2)) AS VL_TOTAL, ");//Diego - OS_25085 - 13/10/2010    
        str.Append(" CAST (CASE WHEN MOVIPEND.VL_COEF < 1 THEN cast(MOVIPEND.VL_COEF as numeric(18,6)) ELSE 1 END * cast(MOVIPEND.QT_PROD * MOVIPEND.VL_UNIPROD as numeric(18,6))  AS NUMERIC(18,2)) AS VL_TOTALCOMDESC "); // Diego - OS_25126 - 19/10/10
        str.Append("FROM PEDIDO ");
        str.Append("INNER JOIN EMPRESA ON (EMPRESA.CD_EMPRESA = PEDIDO.CD_EMPRESA) ");
        str.Append("LEFT OUTER JOIN VENDEDOR ON (VENDEDOR.CD_VEND = PEDIDO.CD_VEND1) ");
        str.Append("LEFT OUTER JOIN PRAZOS ON (PRAZOS.CD_PRAZO = PEDIDO.CD_PRAZO) ");
        str.Append("LEFT OUTER JOIN TRANSPOR ON (TRANSPOR.CD_TRANS = PEDIDO.CD_TRANS) ");
        str.Append("LEFT OUTER JOIN CLIFOR ON (CLIFOR.CD_CLIFOR = PEDIDO.CD_CLIENTE) ");
        str.Append("LEFT OUTER JOIN ENDENTR ON (ENDENTR.CD_CLIENTE = CLIFOR.CD_CLIFOR) ");
        str.Append("LEFT OUTER JOIN MOVIPEND ON ((MOVIPEND.CD_EMPRESA = PEDIDO.CD_EMPRESA) ");
        str.Append("AND (MOVIPEND.CD_PEDIDO = PEDIDO.CD_PEDIDO)) ");
        str.Append("WHERE {0} ORDER BY PEDIDO.CD_PEDIDO");

        ParametroPesquisa objParametros = (ParametroPesquisa)Session["FiltroPedidos"];
        dtClientes = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(string.Format(str.ToString(), objParametros.GetWhere()));
        Session["PedidoRes"] = dtClientes;       
        ExportPDF(sNameFile);
        Response.Redirect("~/ViewPedido.aspx?ANEXO=" + sNameFile);
    }
    private void ExportPDF(string sNameFile)
    {
        try
        {
            DirectoryInfo dinfo = new DirectoryInfo(Server.MapPath("Pedidos"));
            if (!dinfo.Exists)
            {
                dinfo.Create();
            }
            if (File.Exists(Server.MapPath("Pedidos\\" + sNameFile)))
            {
                File.Delete(Server.MapPath("Pedidos\\" + sNameFile + ".pdf"));
            }
            //            PesquisarDados();
            ReportDocument rpt = new ReportDocument();
            DataTable TabelaImpressao = (DataTable)Session["PedidoRes"];
            rpt.Load(Server.MapPath("RelatorioPedido.rpt"));
            rpt.SetDataSource(TabelaImpressao);

            CrystalDecisions.Web.CrystalReportViewer cryView = new CrystalDecisions.Web.CrystalReportViewer();
            ExportOptions CrExportOptions;
            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
            CrDiskFileDestinationOptions.DiskFileName = Server.MapPath("Pedidos\\" + sNameFile + ".pdf");
            CrExportOptions = rpt.ExportOptions;
            {
                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                CrExportOptions.FormatOptions = CrFormatTypeOptions;
            }
            rpt.Export();
        }
        catch (Exception ex)
        {
            throw ex;
            //lblInfo.Text = ex.Message + (ex.InnerException != null ? ex.InnerException.Message : "");
        }
    }

    protected void btnVisualizar_Click(object sender, EventArgs e)
    {
        try
        {
            string sNameFile = DateTime.Today.ToString("ddMMyy") + "_" + DateTime.Now.ToString("HHmmss") + ".pdf";
            PesquisarDados(sNameFile);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}