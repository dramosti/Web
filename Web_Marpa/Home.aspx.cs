using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HLP.Web;
using System.Web.Configuration;
using System.Text;

public partial class Home : System.Web.UI.Page
{



    protected void Page_Load(object sender, EventArgs e)
    {
        string sUser = UsuarioWeb.GetNomeUsuarioConectado(this.Session);
        if (sUser == "")
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            UsuarioWeb objUsuario = Session["ObjetoUsuario"] as UsuarioWeb;
            DataTable dtDadosEmpresa = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("EMPRESA left join cidades on (cidades.nm_cidnor = empresa.nm_cidnor) and (cidades.cd_ufnor = empresa.cd_ufnor)",
                                                                             "empresa.cd_fonenor,empresa.cd_faxnor,empresa.cd_email,empresa.ds_endnor,empresa.ds_endcomp,empresa.nm_bairronor,cidades.nm_cidnor,empresa.cd_ufnor,empresa.cd_cepnor", "empresa.CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa.Trim() + "'");

            foreach (DataRow row in dtDadosEmpresa.Rows)
            {
                lblEmail.Text = " " + row["cd_email"].ToString();
                lblFone1.Text = row["cd_fonenor"].ToString();
                lblFone2.Text = row["cd_faxnor"].ToString();
                lblEnderecoCabec.Text = row["ds_endnor"].ToString() + " , " + row["ds_endcomp"].ToString() + " - " + row["nm_bairronor"].ToString();
                lblEnderecoRodape.Text = row["nm_cidnor"].ToString() + " / " + row["cd_ufnor"].ToString() + " -  CEP:" + row["cd_cepnor"].ToString();

            }
            if (sUser == "")
            {
                btnAcessar.Visible = true;
                VerificaAvisosGeral();
            }
            else
            {
                btnAcessar.Visible = false;
                VerificaAvisosGeraleVendedor();
            }
        }
    }

   
    protected void btnAcessar_Click(object sender, EventArgs e)
    {     
        Page.Response.Redirect("~/Login.aspx");
    }




    public void VerificaAvisosGeral()
    {
        UsuarioWeb objUsuario = new UsuarioWeb();
        string sWhere = " DT_FINALAVISO >= current_date and ST_TIPOAVISO ='G' ";

        DataTable dtAvisos = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("AVISO_WEB", "CD_AVISO, DS_TITULO, DT_FINALAVISO, DS_AVISO ", sWhere);


        DataTable dtRetono = new DataTable("AVISOS_GERAL");
        dtRetono.Columns.Add("CD_AVISO").DataType = System.Type.GetType("System.String");
        dtRetono.Columns.Add("DS_TITULO").DataType = System.Type.GetType("System.String");
        dtRetono.Columns.Add("DT_FINALAVISO").DataType = System.Type.GetType("System.String");
        dtRetono.Columns.Add("DS_AVISO").DataType = System.Type.GetType("System.String");

        DataRow dtRet;

        foreach (DataRow dr in dtAvisos.Rows)
        {
            dtRet = dtRetono.NewRow();
            dtRet["CD_AVISO"] = dr["CD_AVISO"];
            dtRet["DS_TITULO"] = dr["DS_TITULO"];
            dtRet["DT_FINALAVISO"] = dr["DT_FINALAVISO"].ToString().Replace("00:00:00", "");
            dtRet["DS_AVISO"] = dr["DS_AVISO"];

            dtRetono.Rows.Add(dtRet);
        }
        Session["Avisos"] = dtRetono;
        gridAvisos.DataSource = dtRetono;
        gridAvisos.DataBind();

        lblTotalAvisos.Text = dtRetono.Rows.Count == 0 ? "Nenhum Aviso" : dtRetono.Rows.Count.ToString() + " Aviso(s)";

    }

    public void VerificaAvisosGeraleVendedor()
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

        StringBuilder strWhere = new StringBuilder();
        strWhere.Append(" DT_FINALAVISO >= current_date and ");
        strWhere.Append("(ST_TIPOAVISO ='G' OR    CD_REPRESENTANTE ='" + objUsuario.CodigoVendedor + "')");

        DataTable dtAvisos = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("AVISO_WEB", "CD_AVISO, DS_TITULO, DT_FINALAVISO, DS_AVISO ", strWhere.ToString());


        DataTable dtRetono = new DataTable("AVISOS_GERAL");
        dtRetono.Columns.Add("CD_AVISO").DataType = System.Type.GetType("System.String");
        dtRetono.Columns.Add("DS_TITULO").DataType = System.Type.GetType("System.String");
        dtRetono.Columns.Add("DT_FINALAVISO").DataType = System.Type.GetType("System.String");
        dtRetono.Columns.Add("DS_AVISO").DataType = System.Type.GetType("System.String");

        DataRow dtRet;

        foreach (DataRow dr in dtAvisos.Rows)
        {
            dtRet = dtRetono.NewRow();
            dtRet["CD_AVISO"] = dr["CD_AVISO"];
            dtRet["DS_TITULO"] = dr["DS_TITULO"];
            dtRet["DT_FINALAVISO"] = dr["DT_FINALAVISO"].ToString().Replace("00:00:00", "");
            dtRet["DS_AVISO"] = dr["DS_AVISO"];

            dtRetono.Rows.Add(dtRet);
        }
        Session["Avisos"] = dtRetono;
        gridAvisos.DataSource = dtRetono;
        gridAvisos.DataBind();

        lblTotalAvisos.Text = dtRetono.Rows.Count == 0 ? "Nenhum Aviso" : dtRetono.Rows.Count.ToString() + " Aviso(s)";

    }

    protected void gridAvisos_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sCodAviso = gridAvisos.SelectedDataKey[0].ToString();

        ((DataTable)Session["Avisos"]).DefaultView.RowFilter = "CD_AVISO ='" + sCodAviso + "'";
        dvAviso.DataSource = ((DataTable)Session["Avisos"]).DefaultView;
        dvAviso.DataBind();

    }


    protected void btnCarregaGrafico_Click(object sender, EventArgs e)
    {
        CarregaGraficoVendas();
    }
    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        CarregaGraficoTop5Produto();
    }

    private void CarregaGraficoVendas()
    {
        try
        {
            graficoVendasAnuais.Visible = true;
            UsuarioWeb objUsuario = Session["ObjetoUsuario"] as UsuarioWeb;
            graficoVendasAnuais.DataSource = HLP.Dados.Vendas.HlpFuncoesVendas.GetVendasPorRepresentanteAnual(objUsuario.oTabelas, DateTime.Now.Year.ToString(), objUsuario.oTabelas.CdVendedorAtual);
            graficoVendasAnuais.DataBind();
        }
        catch (Exception ex)
        {

            MessageHLP.ShowPopUpMsg(ex.Message, this);
        }
       
    }
    private void CarregaGraficoTop5Produto()
    {
        graficoTopCincoProd.Visible = true;
        UsuarioWeb objUsuario = Session["ObjetoUsuario"] as UsuarioWeb;
        string sTableItens = (WebConfigurationManager.AppSettings["TableItens"]).ToUpper();
        graficoTopCincoProd.DataSource = HLP.Dados.Vendas.HlpFuncoesVendas.GetProdutosMaisVendidos(objUsuario.oTabelas, sTableItens, 5, (DateTime.Now.AddDays(-30)), DateTime.Today);
        graficoTopCincoProd.DataBind();
    }
}