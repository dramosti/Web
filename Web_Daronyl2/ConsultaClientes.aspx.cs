using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using HLP.Dados.Cadastro.Web;
using HLP.Dados.Cadastro;
using HLP.Web;
using System.Text;
using HLP.Dados;

public partial class ConsultaClientes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {            
            string sUser = UsuarioWeb.GetNomeUsuarioConectado(Session);
            if (sUser == "")
            {
                Response.Redirect("~/Login.aspx");
            }
            Session["Lista"] = null;
            Session["ListaPreco"] = null;

            BaseDAO.CancelarOperacaoObjetoDAO((BaseDAO)Session["ObjetoClienteDetalhado"]);
            ParametroPesquisa objParametros = (ParametroPesquisa)Session["FiltroClientes"];
            Session["FiltroClientes"] = null;

            bool bParametrosValidos = (objParametros != null);
            if (bParametrosValidos)
                bParametrosValidos = (!objParametros.AindaNaoDefiniuFiltro());
            if (!bParametrosValidos)
            {
                Response.Redirect("~/PesquisarClientes.aspx");
                return;
            }
            PesquisarDados(objParametros.GetWhere());
        }

    }
    private void PesquisarDados(string sWhere)
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        DataTable dtClientes = (DataTable)Session["DadosConsultaClientes"];
        bool bPesquisarDados = (dtClientes == null);
        if (bPesquisarDados)
        {
            StringBuilder str = new StringBuilder();
            str.Append("SELECT NM_GUERRA, NM_CLIFOR, CD_ALTER, CD_UFNOR, NM_CIDNOR,CD_FONENOR  ");
            str.Append("FROM CLIFOR ");
            str.Append("WHERE ");
            str.Append(sWhere + " ");
            str.Append("ORDER BY NM_CLIFOR ");
            dtClientes = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(
                str.ToString());
            DataColumn[] ChavePrimaria = new DataColumn[] { dtClientes.Columns["CD_ALTER"] };
            dtClientes.PrimaryKey = ChavePrimaria;
            Session["DadosConsultaClientes"] = dtClientes;
        }
        if (dtClientes.Rows.Count == 0)
            MessageHLP.ShowPopUpMsg("Não existem registros para o filtro selecionado", this.Page);
        if (!Page.IsPostBack)
            ProcessaDataBind();
    }

    protected void gridConsultaClientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        lblCliente.Text = "";
        GridDuplicatas.DataSource = null;
        GridDuplicatas.DataBind();

        gridConsultaClientes.PageIndex = e.NewPageIndex;
        ProcessaDataBind();

    }

    private void ProcessaDataBind()
    {
        gridConsultaClientes.DataSource = (DataTable)Session["DadosConsultaClientes"];
        gridConsultaClientes.DataBind();

    }

    protected void gridConsultaClientes_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        MultViewConsultaClientes.ActiveViewIndex = 2;
        GridViewRow RegistroAtual = gridConsultaClientes.Rows[e.RowIndex];
        DataTable dtClientes = (DataTable)Session["DadosConsultaClientes"];
        DataRow RegistroAExcluir = dtClientes.Rows[RegistroAtual.DataItemIndex];
        Session["RegistroAExcluirCliente"] = RegistroAExcluir;
        Session["ExcluiuRegistroCliente"] = null;
    }

    private bool EfetuarExclusao(DataRow RegistroAExcluir)
    {
        bool bExcluiu = false;
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        string sCdClifor = RegistroAExcluir["CD_ALTER"].ToString();
        CliforDAO objCliente = ClienteDAOWeb.GetInstanciaClienteDAOWeb(Session,
            objUsuario);
        try
        {
            DataTable dtPedidoExclusao =
                objCliente.Select("(CD_ALTER = '" + sCdClifor + "')");
            objCliente.RegistroAtual = dtPedidoExclusao.Rows[0];
            bExcluiu = objCliente.ProcessarExclusao();
            if (bExcluiu)
                SetMensagemExclusao("O cliente " + sCdClifor +
                    " foi excluído com sucesso!!!", false);
            else
                SetMensagemExclusao(objCliente.GetErros(), true);
        }
        catch
        {
            SetMensagemExclusao("Ocorreu um erro durante a exclusão " +
                "do cliente de código " + sCdClifor + " !!!", true);
        }
        finally
        {
            objCliente.RegistroAtual = null;
        }
        return bExcluiu;
    }

    private void SetMensagemExclusao(string sMensagem, bool bErro)
    {
        //lblMensagemExclusao.Text = sMensagem;
        //if (!bErro)
        //    lblMensagemExclusao.ForeColor = Color.Black;
        //else
        //    lblMensagemExclusao.ForeColor = Color.Red;
    }

    protected void btnVoltarViewClientes_Click(object sender, EventArgs e)
    {
        object ExcluiuRegistroPedido = Session["ExcluiuRegistroCliente"];
        if ((ExcluiuRegistroPedido != null) &&
            (Convert.ToBoolean(ExcluiuRegistroPedido)))
        {
            Session["ExcluiuRegistroCliente"] = null;
            Response.Redirect("~/ConsultaClientes.aspx");
        }
        else
            MultViewConsultaClientes.ActiveViewIndex = 0;
    }

    protected void btnCancelaExclusao_Click(object sender, EventArgs e)
    {
        MultViewConsultaClientes.ActiveViewIndex = 0;
        Session["RegistroAExcluirCliente"] = null;
    }

    protected void btnConfirmaExclusao_Click(object sender, EventArgs e)
    {
        bool bExcluiu = EfetuarExclusao((DataRow)Session["RegistroAExcluirCliente"]);
        if (bExcluiu)
        {
            Session["DadosConsultaClientes"] = null;
            Session["ExcluiuRegistroCliente"] = true;
        }
        Session["RegistroAExcluirCliente"] = null;
        MultViewConsultaClientes.ActiveViewIndex = 1;
    }

    protected void lblVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/PesquisarClientes.aspx");
    }
    protected void lblImprimir_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/ViewCliente.aspx");
        // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "_new", "window.open('ViewCliente.aspx');", true); 
    }

    protected void gridConsultaClientes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Pendencias")
        {
            UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

            CliforDAO objCliente = ClienteDAOWeb.GetInstanciaClienteDAOWeb(Session,
           objUsuario);
            int index = Convert.ToInt32(e.CommandArgument);

            DataTable dtClientes = (DataTable)Session["DadosConsultaClientes"];
            GridViewRow RegistroAtual = gridConsultaClientes.Rows[index];
            DataRow Registro = dtClientes.Rows[RegistroAtual.DataItemIndex];
            Session["CD_ALTER"] = Registro["CD_ALTER"];
            string sCdClifor = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR", "CD_CLIFOR", "CD_ALTER = '" + Registro["CD_ALTER"] + "'");

            DataTable dtRet = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(string.Format("select count(NR_DOC)TOTAL from doc_ctr where coalesce(st_baixa,'') <> 'B' and cd_empresa = '{0}' and cd_cliente = '{1}' and dt_venci < current_date", objUsuario.oTabelas.sEmpresa, sCdClifor));

            int QtdePendencias = Convert.ToInt32(dtRet.Rows[0]["TOTAL"]);
            if (QtdePendencias > 0)
            {
                GridDuplicatas.DataSource = GetDuplicatasAbertas();
                GridDuplicatas.DataBind();
            }
            else
            {
                lblCliente.Text = "";
                GridDuplicatas.DataSource = null;
                GridDuplicatas.DataBind();

                MessageHLP.ShowPopUpMsg("Não existem pendências para esse cliente!", this.Page);
            }
        }
    }

    protected void GridDuplicatas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Wrap = false;
            e.Row.Cells[1].Text = "R$ " + String.Format("{0:c}", (Session["DadosConsultaDuplicatas"] as DataTable).Rows[0]["TOT_VL_DOC"].ToString());
        }

    }
    public DataTable GetDuplicatasAbertas()
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        string sCdClifor = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR", "CD_CLIFOR", "CD_ALTER = '" + (string)Session["CD_ALTER"] + "'");

        lblCliente.Text = "Cliente " + (string)Session["CD_ALTER"];
        string sWhere = "coalesce(st_baixa,'') <> 'B' and cd_empresa = '" + objUsuario.oTabelas.sEmpresa.ToString().Trim() + "' and cd_cliente = '" + sCdClifor + "'";

        DataTable dtDuplicatas = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("DOC_CTR", "dt_venci, vl_doc ", sWhere);

        DataTable dtRetono = new DataTable("TabelaDuplicatas");
        dtRetono.Columns.Add("DT_VENCI").DataType = System.Type.GetType("System.DateTime");
        dtRetono.Columns.Add("VL_DOC").DataType = System.Type.GetType("System.Double");
        dtRetono.Columns.Add("TOT_VL_DOC", System.Type.GetType("System.Double"), "SUM(VL_DOC)");
        //dt.Columns.Add("TOTAL", System.Type.GetType("System.Double"), "SUM(SUBTOTAL)");

        DataRow dtRet;

        foreach (DataRow dr in dtDuplicatas.Rows)
        {
            dtRet = dtRetono.NewRow();
            dtRet["DT_VENCI"] = dr["DT_VENCI"];
            dtRet["VL_DOC"] = dr["VL_DOC"];

            dtRetono.Rows.Add(dtRet);
        }

        Session["DadosConsultaDuplicatas"] = dtRetono;
        return dtRetono; ;
    }
}