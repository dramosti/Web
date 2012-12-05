using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using HLP.Dados;
using HLP.Dados.Vendas;
using HLP.Dados.Vendas.Web;
using HLP.Web;
using HLP.Geral;
using HLP.Web.Controles.Ajax;
using System.Text;
using Microsoft.Web.UI;
using HLP.Web.Controles;
using FirebirdSql.Data.FirebirdClient;
//using HLP.Dados.Especificos;
using System.Web.Configuration;
using System.Net;
using System.Web.Mail;
using HLP.Dados.Faturamento;
using HLP.Dados.Cadastro;
using System.Text.RegularExpressions;
using HLP.Dados.Cadastro.Web;


public partial class Pedido : System.Web.UI.Page
{


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CarregaDados();
        }
    }

    private void CarregaDados()
    {
        string sUser = UsuarioWeb.GetNomeUsuarioConectado(Session);
        if (sUser == "")
        {
            Response.Redirect("~/Login.aspx");
        }
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

        string CodigoCliente = (string)Session["CD_CLIFOR"];
        string NomeCliente = (string)Session["NM_CLIFOR"];
        txtDataPedido.Text = GetDataFormatada(DateTime.Today.ToShortDateString().ToString());

        string strCD_PRAZO = String.Empty;

        if (CodigoCliente != null)
        {
            txtCodCli.Text = CodigoCliente;
            strCD_PRAZO = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR INNER JOIN PRAZOS ON (PRAZOS.CD_PRAZO = CLIFOR.CD_PRAZO)", "PRAZOS.CD_PRAZO", "(CLIFOR.CD_CLIFOR = '" + txtCodCli.Text + "')");
            DataTable dtRet = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(string.Format("select count(NR_DOC)TOTAL from doc_ctr where coalesce(st_baixa,'') <> 'B' and cd_empresa = '{0}' and cd_cliente = '{1}' and dt_venci < current_date", objUsuario.oTabelas.sEmpresa, CodigoCliente));
            Session["QtdePendencias"] = Convert.ToUInt32(dtRet.Rows[0]["TOTAL"].ToString());
            if (Session["QtdePendencias"] != null)
            {
                if (Convert.ToInt32(Session["QtdePendencias"].ToString()) > 0)
                {
                    lblPendencias.Text = String.Format("{0} Pendência{1}.", Session["QtdePendencias"].ToString(), (Convert.ToInt32(Session["QtdePendencias"].ToString()) > 1 ? "s" : ""));
                }
                else
                {
                    lblPendencias.Text = "";
                }
            }
        }
        else
        {
            Session["QtdePendencias"] = 0;
        }
        CarregaFormaPgto();

        if (NomeCliente != null)
        {
            txtCliente.Text = NomeCliente.ToUpper();
            //string sPrazoPadrao = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR", "CD_PRAZO", "cd_clifor = '" + CodigoCliente + "'");
            //if (sPrazoPadrao != "")
            //{
            //    cbxCD_PRAZO.SelectedValue = sPrazoPadrao;
            //}
            //else
            //{
            //    cbxCD_PRAZO.SelectedValue = WebConfigurationManager.AppSettings["CondPagDefault"];
            //}
        }



        cbxDS_TPDOCWEB.DataSource = GetTPDOC();
        cbxDS_TPDOCWEB.DataBind();

        Session["lsCodigo"] = new List<string>();
    }

    private void CarregaFormaPgto()
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        string sFormas = string.Empty;
        string sWhere = "(ST_PAGAR_OU_RECEBER IN ('A', 'V')) ";
        if (Convert.ToInt32(cbxPrazoPgto.SelectedValue) == 0) // VISTA
        {
            sFormas = (WebConfigurationManager.AppSettings["FORMA_VISTA"]).ToUpper();
            sWhere += "AND CD_PRAZO IN (" + sFormas + ")";
        }

        DataTable dtPrazo = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("PRAZOS", "DS_PRAZO, CD_PRAZO", sWhere, "DS_PRAZO");
        cbxCD_PRAZO.DataSource = dtPrazo;
        cbxCD_PRAZO.DataBind();

    }


    protected void btnVerificarPendencias_Click(object sender, EventArgs e)
    {
        if (Session["QtdePendencias"] != null)
        {
            if (Convert.ToInt32(Session["QtdePendencias"].ToString()) > 0)
            {
                MultiViewItensPed.ActiveViewIndex = 3;
                GridDuplicatas.DataSource = GetDuplicatasAbertas();
                GridDuplicatas.DataBind();
            }
            else
            {
                MessageHLP.ShowPopUpMsg("Não existem pendências para esse cliente!", this.Page);
            }
        }
    }
    protected void btnIncluiItens_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(Session["QtdePendencias"].ToString()) > 0)
        {
            MessageHLP.ShowPopUpMsg("Não é possível efetuar um pedido para um Cliente com pendência financeira!", this.Page);
        }
        else
        {

            UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

            bool CamposObrigatorios = true;

            if (txtCliente.Text.Equals(String.Empty))
            {
                CamposObrigatorios = false;
                lblInfo.Visible = true;
                MessageHLP.ShowPopUpMsg("Cliente não foi informado!", this.Page);
            }
            if (CamposObrigatorios)
            {
                lblInfo.Visible = false;
                lblInfo.Text = String.Empty;
                MultiViewItensPed.ActiveViewIndex = 1;

                CarregaListaPreco();

                string sListaPadrao = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR", "coalesce(CD_LISTA,'00001')CD_LISTA", "CD_CLIFOR = '" + txtCodCli.Text + "'");
                //cbxListaPreco.SelectedValue = sListaPadrao;
                cbxListaPreco.SelectedIndex = cbxListaPreco.Items.Count - 1;
                GridViewDb.DataSource = GetProdutoGrid();
                GridViewDb.DataBind();
            }
            else
            {
                return;
            }
        }
    }

    private void CarregaListaPreco()
    {
        cbxListaPreco.DataTextField = "DS_LISTA";
        cbxListaPreco.DataValueField = "CD_LISTA";
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

        string sListas = string.Empty;
        if (Convert.ToInt32(cbxPrazoPgto.SelectedValue) == 0) // VISTA
        {
            sListas = (WebConfigurationManager.AppSettings["LISTA_VISTA"]).ToUpper();
        }
        else if (Convert.ToInt32(cbxPrazoPgto.SelectedValue) == 1) // PRAZO
        {
            sListas = (WebConfigurationManager.AppSettings["LISTA_PRAZO"]).ToUpper();
        }

        DataTable dtListas = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("listapre", "CD_LISTA, DS_LISTA", "CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa + "' AND CD_LISTA IN (" + sListas + ")");
        cbxListaPreco.DataSource = dtListas;
        cbxListaPreco.DataBind();
    }
    protected void btnAtualiza_Click(object sender, EventArgs e)
    {
        AtualizaTotal();
    }

    private void AtualizaTotal()
    {
        double TotalAtual = 0;
        int iLoop = 0;
        Regex reg = new Regex(@"^\d{1,5}(\.\d{1,2})?$");

        foreach (GridViewRow Linha in GridViewNovo.Rows)
        {
            double dqtde = 1;
            TextBox txtQtde = (TextBox)Linha.Cells[8].FindControl("txtQtde");
            if (reg.IsMatch(String.Format("{0:N2}", txtQtde.Text)))
            {
                dqtde = Convert.ToDouble(String.Format("{0:N2}", txtQtde.Text));
            }
            else
            {
                txtQtde.Text = "1";
            }
            double dVL_UNIPROD = Convert.ToDouble(Linha.Cells[4].Text);

            double Total = dqtde * dVL_UNIPROD;
            Linha.Cells[6].Text = String.Format("{0:N4}", Total);
            TotalAtual = TotalAtual + Total;
            iLoop++;
        }
        GridViewNovo.FooterRow.Cells[8].Text = String.Format("{0:N2}", TotalAtual);
    }


    protected void btnPesqCliente_Click(object sender, EventArgs e)
    {
        Session["IncluirClientePedido"] = true;
        Response.Redirect("~/ConsultaClientes.aspx");
    }
    protected void btnCliente_Click(object sender, EventArgs e)
    {
        if (txtCodCli.Text != "")
        {
            PesquisarDados();
        }
    }
    private void PesquisarDados()
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        DataTable dtClientes = null;

        txtCodCli.Text = txtCodCli.Text.PadLeft(7, '0');
        StringBuilder str = new StringBuilder();
        str.Append("SELECT CD_CLIFOR, CD_ALTER, NM_GUERRA, NM_CLIFOR, DS_ENDNOR, NR_ENDNOR, NM_BAIRRONOR, NM_CIDNOR, CD_UFNOR, ST_PESSOAJ, CD_CGC, cd_insest, CD_CPF, CD_RG, CD_CEPNOR, CD_FONENOR,CD_FONECOM, CD_FAXNOR   ");
        str.Append("FROM CLIFOR ");
        str.Append("WHERE ");
        str.Append("((CLIFOR.ST_INATIVO <> 'S') OR (CLIFOR.ST_INATIVO IS NULL)) AND (CLIFOR.CD_VEND1 = '" + objUsuario.CodigoVendedor.ToString() + "') AND (NM_GUERRA IS NOT NULL) ");
        str.Append("AND (CD_CLIFOR ='" + txtCodCli.Text + "')");
        dtClientes = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(
            str.ToString());
        DataColumn[] ChavePrimaria = new DataColumn[] { dtClientes.Columns["CD_CLIFOR"] };
        dtClientes.PrimaryKey = ChavePrimaria;
        Session["DadosConsultaClientes"] = dtClientes;


        if (dtClientes.Rows.Count == 0)
        {
            MessageHLP.ShowPopUpMsg("Não existem registros para o filtro selecionado", this.Page);
            if (Session["CD_CLIFOR"] != null)
            {
                txtCodCli.Text = Session["CD_CLIFOR"].ToString();
            }
            else
            {
                txtCodCli.Text = "";
            }
        }
        else
        {
            BaseDAO.CancelarOperacaoObjetoDAO((BaseDAO)Session["ObjetoClienteDetalhado"]);

            Session["CD_CLIFOR"] = txtCodCli.Text;
            Session["NM_CLIFOR"] = dtClientes.Rows[0]["NM_CLIFOR"].ToString();
            Session["IncluirClientePedido"] = false;
            CliforDAO objCliente = ClienteDAOWeb.GetInstanciaClienteDAOWeb(Session,
           objUsuario);
            objCliente.RegistroAtual = dtClientes.Rows[0];
            CarregaDados();

            Session["lsCodigoInseridos"] = new List<string>();
        }

    }

    protected void GridViewNovo_RowEditing(object sender, GridViewEditEventArgs e)
    {
        TextBox txtQtde = (TextBox)GridViewNovo.Rows[e.NewEditIndex].Cells[5].FindControl("txtQtde");
        double dqtde = Convert.ToDouble(String.Format("{0:N2}", txtQtde.Text));
        double Valor = Convert.ToDouble(GridViewNovo.Rows[e.NewEditIndex].Cells[4].Text);
        double Total = dqtde * Valor;
        GridViewRow row = GridViewNovo.Rows[e.NewEditIndex];
        row.Cells[8].Text = String.Format("{0:N2}", Total);

        double TotalAtual = 0;

        foreach (GridViewRow Linha in GridViewNovo.Rows)
        {
            TotalAtual = TotalAtual + Convert.ToDouble(Linha.Cells[8].Text);
        }

        GridViewNovo.FooterRow.Cells[8].Text = String.Format("{0:N2}", TotalAtual);

    }

    protected void GridViewNovo_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Excluir")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GridViewNovo.Rows[index];
            ExcluirItem(Server.HtmlDecode(row.Cells[1].Text), Server.HtmlDecode(row.Cells[2].Text));

            ((List<string>)Session["lsCodigo"]).Remove(row.Cells[1].Text.Split('-')[0].Trim());
            MudaCorLinha();

        }
    }
    protected void GridViewDb_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "Incluir")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GridViewDb.Rows[index];
            decimal dQtde = Convert.ToDecimal(Server.HtmlDecode(row.Cells[6].Text));

            if (dQtde > 0)
            {
                Label lblValor = (Label)row.Cells[3].FindControl("Label1");
                IncluirNaLista(Server.HtmlDecode(row.Cells[4].Text), Server.HtmlDecode(row.Cells[2].Text), Server.HtmlDecode(row.Cells[4].Text), Server.HtmlDecode(lblValor.Text), Server.HtmlDecode(row.Cells[5].Text));

                row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFB3B3");//Color.Red;
                if (!((List<string>)Session["lsCodigo"]).Contains(row.Cells[4].Text.Trim()))
                {
                    ((List<string>)Session["lsCodigo"]).Add(row.Cells[4].Text.Trim());
                }
                MudaCorLinha();
                AtualizaTotal();
            }
            else
            {
                MessageHLP.ShowPopUpMsg("Produto sem estoque, impossível ser utilizado!", this.Page);
            }
        }
        else if (e.CommandName == "Alertar")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GridViewDb.Rows[index];

            string strConn = ConfigurationManager.ConnectionStrings["ConnectionStringFB"].ConnectionString.ToString();

            UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

            FbConnection Conn = new FbConnection(strConn);

            FbCommand cmdInsertAviso = new FbCommand();
            cmdInsertAviso.CommandText = "SP_ALERTA_ESTWEB";
            cmdInsertAviso.CommandType = CommandType.StoredProcedure;
            cmdInsertAviso.Connection = Conn;

            Conn.Open();


            //        SCD_CLIFOR VARCHAR(7),
            //SCD_VEND VARCHAR(7),
            //SCD_EMPRESA VARCHAR(3),
            //SCD_PRODUTO VARCHAR(7),
            //SNM_CLIFOR VARCHAR(60),
            //SNM_PROD VARCHAR(60),
            //SNM_VEND VARCHAR(60))

            string sCD_PROD = Server.HtmlDecode(row.Cells[4].Text);
            string sNM_PROD = Server.HtmlDecode(row.Cells[2].Text);
            cmdInsertAviso.Parameters.Add("@SCD_CLIFOR", FbDbType.VarChar, 7).Value = txtCodCli.Text.Trim();
            cmdInsertAviso.Parameters.Add("@SCD_VEND", FbDbType.VarChar, 7).Value = objUsuario.CodigoVendedor.ToString();
            cmdInsertAviso.Parameters.Add("@SCD_EMPRESA", FbDbType.VarChar, 3).Value = objUsuario.oTabelas.sEmpresa.ToString();
            cmdInsertAviso.Parameters.Add("@SCD_PRODUTO", FbDbType.VarChar, 7).Value = sCD_PROD;
            cmdInsertAviso.Parameters.Add("@SNM_CLIFOR", FbDbType.VarChar, 60).Value = txtCliente.Text;
            cmdInsertAviso.Parameters.Add("@SNM_PROD", FbDbType.VarChar, 60).Value = sNM_PROD;
            cmdInsertAviso.Parameters.Add("@SNM_VEND", FbDbType.VarChar, 60).Value = objUsuario.NomeUsuario;
            //NM_CLIFOR,NM_PROD,NM_VEND 

            string sRetorno = cmdInsertAviso.ExecuteScalar().ToString();


            if (sRetorno != null)
            {
                MessageHLP.ShowPopUpMsg("Alerta de produto sem estoque enviado com sucesso!", this.Page);
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
    protected void GridViewNovo_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtQtde = (TextBox)e.Row.Cells[8].FindControl("txtQtde");
            string qtde = txtQtde.Text.Trim();

            double Qtdade = Convert.ToDouble(String.Format("{0:N2}", qtde));
            double ValorUnitario = Convert.ToDouble(e.Row.Cells[4].Text);
            double ValorTotal = Qtdade * ValorUnitario;
            e.Row.Cells[6].Text = String.Format("{0:N2}", ValorTotal);
        }

    }

    protected void btnLoc_Click(object sender, EventArgs e)
    {
        GridViewDb.DataSource = GetProdutoGrid();
        GridViewDb.DataBind();
        MudaCorLinha();

    }

    private void MudaCorLinha()
    {
        foreach (GridViewRow row in GridViewDb.Rows)
        {
            if (((List<string>)Session["lsCodigo"]).Contains(row.Cells[4].Text.Trim()))
            {
                row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFB3B3");//Color.Red;
            }
            else
            {
                row.BackColor = Color.White;
            }
        }
        GridViewNovo.Caption = "<b>" + (Session["lsCodigo"] as List<string>).Count.ToString() + " Produto(s) inserido(s)</b>";
    }
    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        MultiViewItensPed.ActiveViewIndex = 0;
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        LimparSession();

        Response.Redirect("~/Pedido.aspx");
    }
    public void txtValorUnitario_TextChanged(object sender, EventArgs e)
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        TextBox txtValorUnitario = (TextBox)sender;

        int index = ((GridViewRow)txtValorUnitario.Parent.Parent).DataItemIndex;
        string sCD_PROD = GridViewNovo.Rows[index].Cells[1].Text.Split('-')[0];


        double VL_UNIT = GetProdutoEspecifico(sCD_PROD);
        double VL_ATUAL = 0;

        try
        {
            VL_ATUAL = Convert.ToDouble(txtValorUnitario.Text);
        }
        catch (Exception)
        {
            txtValorUnitario.Text = VL_UNIT.ToString("N2");
            return;
        }

        if (VL_ATUAL < VL_UNIT)
        {
            MessageHLP.ShowPopUpMsg("Não Permitido: Valor abaixo da Lista " + cbxListaPreco.Items[cbxListaPreco.Items.Count - 1].Text + Environment.NewLine +
             "Escolha uma Lista menor ou venda com um valor maior.", this.Page);
            txtValorUnitario.Text = VL_UNIT.ToString("N2");
            txtValorUnitario.Focus();
        }
        else
        {
            txtValorUnitario.Text = VL_ATUAL.ToString("N2");
        }
    }


    private void LimparSession()
    {
        Session["Lista"] = null;
        Session["CD_CLIFOR"] = null;
        Session["NM_CLIFOR"] = null;
    }
    protected void Cancel_Click(object sender, EventArgs e)
    {
        MessageHLP.ShowPopUpMsg("Cancelado!", this.Page);
    }
    protected void btnGravar_Click(object sender, EventArgs e)
    {
        try
        {

            UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
            string sST_DESC = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("EMPRESA", "ST_DESC", "CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa + "'");

            string sTipoDocumentoPadrao = cbxDS_TPDOCWEB.SelectedValue.ToString();

            string Doc = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("TPDOC", "TP_DOC", "CD_TIPODOC='" + sTipoDocumentoPadrao + "'");
            string sGenerator = "PEDIDO" + objUsuario.oTabelas.sEmpresa + Doc;

            if (!objUsuario.oTabelas.hlpDbFuncoes.VerificaExistenciaGenerator(sGenerator))
            {
                int iIni = 0;
                switch (Doc)
                {
                    case "PE": iIni = 0;
                        break;
                    case "NE": iIni = 1000000;
                        break;
                    case "NS": iIni = 1000000;
                        break;
                    case "CF": iIni = 2000000;
                        break;
                    case "PC": iIni = 9000000;
                        break;
                    case "OR": iIni = 8000000;
                        break;

                    default:
                        throw new Exception("O generator " + sGenerator + " não existe");
                }
                objUsuario.oTabelas.hlpDbFuncoes.CreateGenerator(sGenerator, iIni);

            }
            string sCD_PEDIDO = objUsuario.oTabelas.hlpDbFuncoes.RetornaProximoValorGenerator(sGenerator).PadLeft(7, '0');


            string strConn = ConfigurationManager.ConnectionStrings["ConnectionStringFB"].ConnectionString.ToString();
            FbConnection Conn = new FbConnection(strConn);

            FbCommand cmdSelectPed = new FbCommand();
            cmdSelectPed.CommandText = "SP_INCLUI_PEDIDO_WEB";
            cmdSelectPed.CommandType = CommandType.StoredProcedure;
            cmdSelectPed.Connection = Conn;

            Conn.Open();

            cmdSelectPed.Parameters.Add("@SCD_EMPRESA", FbDbType.VarChar, 3).Value = objUsuario.oTabelas.sEmpresa.ToString();
            cmdSelectPed.Parameters.Add("@SCD_VEND1", FbDbType.VarChar, 7).Value = objUsuario.CodigoVendedor.ToString();
            cmdSelectPed.Parameters.Add("@SCD_PEDIDO", FbDbType.VarChar, 7).Value = sCD_PEDIDO;

            string strRetPedido = cmdSelectPed.ExecuteScalar().ToString();

            Conn.Close();

            if (strRetPedido != null)
            {
                StringBuilder strUpDatePed = new StringBuilder();

                string sVL_TOTALPED = GridViewNovo.FooterRow.Cells[8].Text.Replace(".", "");

                strUpDatePed.Append(" UPDATE PEDIDO SET CD_PEDIDO = '" + strRetPedido.Trim());
                strUpDatePed.Append("', VL_TOTALPED = '" + sVL_TOTALPED.Replace(",", "."));
                strUpDatePed.Append("', CD_CLIENTE = '" + txtCodCli.Text.Trim());
                strUpDatePed.Append("', NM_GUERRA = '" + ((CliforDAO)(Session["ObjetoClienteDetalhado"])).RegistroAtual["NM_GUERRA"].ToString());
                strUpDatePed.Append("', ST_DESC = '" + sST_DESC);

                strUpDatePed.Append("', NM_CLIFOR = '" + ((CliforDAO)(Session["ObjetoClienteDetalhado"])).RegistroAtual["NM_CLIFOR"].ToString());
                strUpDatePed.Append("', DS_ENDNOR = '" + ((CliforDAO)(Session["ObjetoClienteDetalhado"])).RegistroAtual["DS_ENDNOR"].ToString());
                strUpDatePed.Append("', NR_ENDNOR = '" + ((CliforDAO)(Session["ObjetoClienteDetalhado"])).RegistroAtual["NR_ENDNOR"].ToString());
                strUpDatePed.Append("', NM_BAIRRONOR = '" + ((CliforDAO)(Session["ObjetoClienteDetalhado"])).RegistroAtual["NM_BAIRRONOR"].ToString());
                strUpDatePed.Append("', NM_CIDNOR = '" + ((CliforDAO)(Session["ObjetoClienteDetalhado"])).RegistroAtual["NM_CIDNOR"].ToString());
                strUpDatePed.Append("', CD_UFNOR = '" + ((CliforDAO)(Session["ObjetoClienteDetalhado"])).RegistroAtual["CD_UFNOR"].ToString());
                strUpDatePed.Append("', ST_PESSOAJ = '" + ((CliforDAO)(Session["ObjetoClienteDetalhado"])).RegistroAtual["ST_PESSOAJ"].ToString());

                if (((CliforDAO)(Session["ObjetoClienteDetalhado"])).RegistroAtual["ST_PESSOAJ"].ToString() == "S")
                {
                    strUpDatePed.Append("', CD_CGCCPF = '" + ((CliforDAO)(Session["ObjetoClienteDetalhado"])).RegistroAtual["CD_CGC"].ToString());
                    strUpDatePed.Append("', CD_INSEST_RG = '" + ((CliforDAO)(Session["ObjetoClienteDetalhado"])).RegistroAtual["cd_insest"].ToString());
                }
                else
                {
                    strUpDatePed.Append("', CD_CGCCPF = '" + ((CliforDAO)(Session["ObjetoClienteDetalhado"])).RegistroAtual["CD_CPF"].ToString());
                    strUpDatePed.Append("', CD_INSEST_RG = '" + ((CliforDAO)(Session["ObjetoClienteDetalhado"])).RegistroAtual["CD_RG"].ToString());
                }
                strUpDatePed.Append("', CD_CEPNOR = '" + ((CliforDAO)(Session["ObjetoClienteDetalhado"])).RegistroAtual["CD_CEPNOR"].ToString());
                strUpDatePed.Append("', CD_FONENOR = '" + ((CliforDAO)(Session["ObjetoClienteDetalhado"])).RegistroAtual["CD_FONENOR"].ToString());
                strUpDatePed.Append("', CD_FAXNOR = '" + ((CliforDAO)(Session["ObjetoClienteDetalhado"])).RegistroAtual["CD_FAXNOR"].ToString());


                strUpDatePed.Append("', DT_PEDIDO = '" + txtDataPedido.Text.Replace("/", "."));
                //strUpDatePed.Append("', DT_PEDIDO = '" + DateTime.Now.ToString().Replace("/", "."));
                strUpDatePed.Append("', CD_TIPODOC = '" + sTipoDocumentoPadrao);
                strUpDatePed.Append("', CD_PRAZO = '" + cbxCD_PRAZO.SelectedItem.Value.Trim());
                strUpDatePed.Append("', CD_TRANS = '" + objUsuario.oTabelas.TRANSP.ToString().Trim());
                strUpDatePed.Append("', DS_OBS_WEB = '" + txtObs.Text.ToUpper().Trim());
                strUpDatePed.Append("', DT_ABER = '" + txtDataPedido.Text.Replace("/", "."));
                strUpDatePed.Append("', CD_USUINC ='" + objUsuario.oTabelas.CdUsuarioAtual);
                strUpDatePed.Append("', CD_VEND1 = '" + objUsuario.oTabelas.CdVendedorAtual + "'");
                strUpDatePed.Append(" WHERE (CD_PEDIDO = '" + strRetPedido.Trim() + "') AND (CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa.Trim() + "')");

                FbCommand cmdUpDatePedido = new FbCommand(strConn);
                cmdUpDatePedido.CommandText = strUpDatePed.ToString();
                cmdUpDatePedido.CommandType = CommandType.Text;
                cmdUpDatePedido.Connection = Conn;

                Conn.Open();

                int Ret = cmdUpDatePedido.ExecuteNonQuery();

                Conn.Close();

                if (Ret > 0)
                {
                    Ret = 0;
                    int iCountSeq = 1;
                    foreach (GridViewRow row in GridViewNovo.Rows)
                    {

                        FbCommand cmdSelectMoviPend = new FbCommand();
                        cmdSelectMoviPend.CommandText = "SP_INCLUI_MOVITEM_WEB";
                        cmdSelectMoviPend.CommandType = CommandType.StoredProcedure;
                        cmdSelectMoviPend.Connection = Conn;
                        Conn.Open();

                        cmdSelectMoviPend.Parameters.Add("@SCD_EMPRESA", FbDbType.VarChar, 3).Value = objUsuario.oTabelas.sEmpresa.ToString();
                        cmdSelectMoviPend.Parameters.Add("@SCD_PEDIDO", FbDbType.VarChar, 7).Value = strRetPedido.Trim();

                        string strRetMoviPend = cmdSelectMoviPend.ExecuteScalar().ToString();

                        Conn.Close();

                        if (strRetMoviPend != null)
                        {
                            strUpDatePed.Length = 0;
                            string sCD_LISTA = row.Cells[1].Text.Split('-')[1].ToString().Trim();
                            string sCD_PROD = row.Cells[1].Text.Split('-')[0].ToString().Trim();

                            TextBox txtQtde = (TextBox)row.Cells[8].FindControl("txtQtde");
                            string qtde = txtQtde.Text.Trim();
                            string CF = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("PRODUTO", "CD_CF", "CD_PROD = '" + sCD_PROD + "'");
                            string UNID = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("PRODUTO", "CD_TPUNID", "CD_PROD = '" + sCD_PROD + "'");
                            string CODICMS = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("PRODUTO", "CD_ALIICMS", "CD_PROD = '" + sCD_PROD + "'");
                            string SITTRIB = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("PRODUTO", "CD_SITTRIB", "CD_PROD = '" + sCD_PROD + "'");
                            string scdOper = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("tpdoc", "substring(tpdoc.cd_operval from 1 for 3) cd_operval", "cd_tipodoc = '" + sTipoDocumentoPadrao + "'");
                            string sSittribipi = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("OPEREVE", "cd_sittribipi", "CD_OPER = '" + scdOper + "'");
                            string sSittribpis = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("OPEREVE", "cd_sittribpis", "CD_OPER = '" + scdOper + "'");
                            string sSittribcofins = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("OPEREVE", "cd_sittribcof", "CD_OPER = '" + scdOper + "'");
                            string sTipoPrazo = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("PRAZOS", "DS_FORMULA", "CD_PRAZO = '" + cbxCD_PRAZO.SelectedItem.Value.Trim() + "'");

                            #region Busca Valor % de comissão

                            decimal dVL_PERCOMI1 = 0;
                            dVL_PERCOMI1 = Convert.ToDecimal(objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("LISTAPRE", "COALESCE(LISTAPRE.VL_PERCOMI,0)", "CD_LISTA = '" + sCD_LISTA + "'"));

                            #endregion


                            #region Calcula Desconto Item

                            string RecebeCoefDesc = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("PRAZOS", "VL_COEF", "(CD_PRAZO = '" + cbxCD_PRAZO.SelectedItem.Value.Trim() + "')");
                            decimal dCOEFDESC = Convert.ToDecimal((RecebeCoefDesc.Equals(String.Empty) ? "1.0" : RecebeCoefDesc));
                            decimal dVL_UNIPROD = 0;
                            decimal dVL_DESCONTO_VALOR = 0;
                            decimal VlTotLiq = 0;

                            dVL_UNIPROD = (Convert.ToDecimal(row.Cells[4].Text));

                            #endregion

                            VlTotLiq = Decimal.Round((Convert.ToDecimal(qtde) * dVL_UNIPROD), 2);
                            string sDS_PROD = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("PRODUTO", "DS_PROD", "CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa.Trim() + "' AND CD_PROD ='" + sCD_PROD + "'");
                            string sCD_ALTER = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("PRODUTO", "CD_ALTER", "CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa.Trim() + "' AND CD_PROD ='" + sCD_PROD + "'");

                            strUpDatePed.Append("UPDATE MOVITEM SET NR_LANC = '" + strRetMoviPend.Trim() + "', CD_PEDIDO = '" + strRetPedido.Trim() + "', DS_PROD = '" + sDS_PROD + "', ");
                            strUpDatePed.Append("CD_VEND1 = '" + objUsuario.oTabelas.CdVendedorAtual + "', ");
                            strUpDatePed.Append("CD_LISTA = '" + sCD_LISTA + "', CD_PROD = '" + sCD_PROD + "', CD_ALTER = '" + sCD_ALTER + "', ");
                            strUpDatePed.Append("QT_SALDOEN = '" + qtde.Replace(".", "").Replace(",", ".") + "', ");
                            strUpDatePed.Append("QT_PROD = '" + qtde.Replace(".", "").Replace(",", ".") + "', VL_UNIPROD_SEM_DESC = '" + dVL_UNIPROD.ToString().Replace(".", "").Replace(",", "."));
                            strUpDatePed.Append("', VL_TOTLIQ = '" + VlTotLiq.ToString().Replace(".", "").Replace(",", ".") + "'"); // adicionado o campo VL_UNIPROD_SEM_DESC
                            strUpDatePed.Append(", VL_TOTBRUTO = '" + VlTotLiq.ToString().Replace(".", "").Replace(",", ".") + "'");
                            strUpDatePed.Append(", CD_SEQPED = '" + iCountSeq.ToString().PadLeft(2, '0') + "'");
                            strUpDatePed.Append(", VL_DESCONTO_VALOR = '" + dVL_DESCONTO_VALOR.ToString().Replace(".", "").Replace(",", ".") + "'");
                            strUpDatePed.Append(", VL_UNIPROD = '" + dVL_UNIPROD.ToString().Replace(".", "").Replace(",", ".") + "'");
                            strUpDatePed.Append(", CD_USUINC = '" + objUsuario.oTabelas.CdUsuarioAtual.ToString() + "', dt_doc ='" + txtDataPedido.Text.Replace("/", ".") + "', DT_LANC = '" + txtDataPedido.Text.Replace("/", ".") + "', CD_USUALT = '" + objUsuario.oTabelas.CdUsuarioAtual.ToString() + "', ");
                            strUpDatePed.Append("CD_CF = '" + (CF.Equals(String.Empty) ? "0000000" : CF.Trim()) + "', CD_TPUNID = '" + UNID.Trim() + "', CD_ALIICMS = '" + CODICMS.Trim() + "', CD_SITTRIB = '" + SITTRIB.Trim() + "', VL_COEF = " + dCOEFDESC.ToString().Replace(",", ".") + ", "); //COEF.ToString().Replace(",", ".") + ", ");
                            strUpDatePed.Append("VL_PERCOMI1 = '" + dVL_PERCOMI1.ToString().Replace(".", "").Replace(",", ".") + "', DT_PRAZOEN = '" + txtDataPedido.Text.Replace("/", ".") + "', ");
                            strUpDatePed.Append("VL_ALIIPI = " + HlpFuncoesFaturamento.GetAliquotaIPI(objUsuario.oTabelas.GetOperacaoDefault("CD_OPER"), txtCodCli.Text.Trim(), CF, objUsuario.oTabelas) + ", ");
                            strUpDatePed.Append("CD_OPER = '" + scdOper);//+ "', VL_PERENTR = " + 100 + " ");
                            strUpDatePed.Append("' , cd_sittribipi = '" + sSittribipi + "', cd_sittribpis = '" + sSittribpis + "', cd_sittribcof = '" + sSittribcofins + "' ");
                            strUpDatePed.Append(" WHERE (NR_LANC = '" + strRetMoviPend.Trim() + "') AND (CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa.Trim() + "')");


                            FbCommand cmdUpDateMoviPend = new FbCommand();
                            cmdUpDateMoviPend.CommandText = strUpDatePed.ToString();
                            cmdUpDateMoviPend.CommandType = CommandType.Text;
                            cmdUpDateMoviPend.Connection = Conn;
                            Conn.Open();
                            Ret = cmdUpDateMoviPend.ExecuteNonQuery();
                            Conn.Close();
                        }
                    }
                    if (Ret > 0)
                    {
                        //EnviarEmailPedido(strRetPedido);
                        LimparSession();
                        Response.Redirect("~/Informativo.aspx?PARAMETERCODIGO=" + strRetPedido);
                    }
                    else
                        MessageHLP.ShowPopUpMsg("Cadastro não foi concluído!", this.Page);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void btnAvancar_Click(object sender, EventArgs e)
    {
        if (GridViewNovo.Rows.Count > 0)
        {
            if (cbxCD_PRAZO.Text.Equals(String.Empty))
            {
                MessageHLP.ShowPopUpMsg("Condição de Pagamento não foi informado!", this.Page);
            }
            else
            {
                AtualizaTotal();

                if (Convert.ToInt32(Session["QtdePendencias"].ToString()) > 0)
                {
                    txtObs.Text += "**EXISTE PENDÊNCIA FINANCEIRA PARA ESSE CLIENTE**";
                }
                else
                {
                    txtObs.Text = "";
                }
                MultiViewItensPed.ActiveViewIndex = 2;
            }

        }
    }
    protected void btnVolteClie_Click(object sender, EventArgs e)
    {
        MultiViewItensPed.ActiveViewIndex = 0;
    }
    protected void btnVolteItem_Click(object sender, EventArgs e)
    {
        MultiViewItensPed.ActiveViewIndex = 1;
    }

    protected void GridViewDb_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridViewDb.PageIndex = e.NewPageIndex;
        GridViewDb.DataSource = (DataTable)Session["DadosConsultaProduto"];
        GridViewDb.DataBind();
        MudaCorLinha();
    }
    protected void GridDuplicatas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridDuplicatas.PageIndex = e.NewPageIndex;
        GridDuplicatas.DataSource = (DataTable)Session["DadosConsultaDuplicatas"];
        GridDuplicatas.DataBind();
    }
    private void ExcluirItem(string strCodigo, string strDesc)
    {
        DataSet ExcluirProduto = CriaDataSet();

        DataRow row = ExcluirProduto.Tables[0].Rows.Find(strCodigo);

        if (row != null)
        {
            ExcluirProduto.Tables[0].Rows.Remove(row);
            ExcluirProduto.AcceptChanges();
            GridViewNovo.DataSource = CriaDataSet();
            GridViewNovo.DataBind();
        }
        else
        {
            string strAbrir = "if(window.alert('Não foi possível excluir o Produto: " + strCodigo.Trim() + " - " + strDesc.Trim() + " da Lista!'))";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Confirmar", strAbrir, true);
        }
        if (GridViewNovo.Rows.Count < 1)
        {
            btnAtualiza.Visible = false;
            btnAvancar.Visible = false;
        }
    }

    private string GetDataFormatada(string strCampo)
    {
        string Dia = Convert.ToDateTime(strCampo).Day.ToString();
        string Mes = Convert.ToDateTime(strCampo).Month.ToString();
        string Ano = Convert.ToDateTime(strCampo).Year.ToString();
        string Ret = null;

        if (Dia.Length < 2)
            Dia = "00".Substring(0, 2 - Dia.Length) + Dia;
        if (Mes.Length < 2)
            Mes = "00".Substring(0, 2 - Mes.Length) + Mes;

        Ret = Dia + "/" + Mes + "/" + Ano;

        return Ret;
    }
    public DataTable GetDuplicatasAbertas()
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        string sWhere = "coalesce(st_baixa,'') <> 'B' and cd_empresa = '" + objUsuario.oTabelas.sEmpresa.ToString().Trim() + "' and cd_cliente = '" + (string)Session["CD_CLIFOR"] + "' order by dt_venci ";

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
    private DataTable GetProdutoGrid()
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

        //Busca a Lista de Preço Padrão do cadastro de Cliente
        string ListaPreco = cbxListaPreco.SelectedValue.ToString();
        ListaPreco = ListaPreco == "" ? "00001" : ListaPreco;
        /// M-manual / A-automatico
        string st_atualizacao = string.Empty;
        decimal vl_perc = 0;


        DataTable dtListaPreco = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("LISTAPRE", "ST_ATUALIZACAO, VL_PERC", "CD_LISTA = '" + ListaPreco + "'");

        foreach (DataRow dr in dtListaPreco.Rows)
        {
            st_atualizacao = dr["ST_ATUALIZACAO"].ToString().Trim();
            vl_perc = (Convert.ToDecimal(dr["VL_PERC"]) / 100) + 1;
            break;
        }

        StringBuilder strWhere = new StringBuilder();
        strWhere.Append("(PRECOS.CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa.ToString().Trim() + "') AND ((PRODUTO.ST_INATIVO IS NULL) OR ");
        strWhere.Append("(PRODUTO.ST_INATIVO <> 'S')) AND ");
        strWhere.Append("((PRECOS.CD_LISTA = '" + (st_atualizacao.Equals("M") ? ListaPreco.Trim() : "00001") + "') AND (PRECOS.VL_PRECOVE > 0)) ");
        if (!txtCodProd.Text.Equals(String.Empty))
            strWhere.Append("AND (PRODUTO.CD_ALTER STARTING'" + txtCodProd.Text.ToUpper().Trim().PadLeft(7, '0') + "') ");
        if (!txtProdDesc.Text.Equals(String.Empty))
            strWhere.Append("AND (PRODUTO.DS_PROD LIKE ('%" + txtProdDesc.Text.ToUpper().Trim() + "%')) ");


        DataTable dtProduto = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("PRECOS INNER JOIN PRODUTO ON (PRODUTO.CD_PROD = PRECOS.CD_PROD)", (st_atualizacao.Equals("M") ? "PRECOS.VL_PRECOVE" : "(PRECOS.VL_PRECOVE * " + vl_perc.ToString().Replace(',', '.') + ")VL_PRECOVE") + ", PRODUTO.CD_ALTER, PRODUTO.CD_PROD, PRODUTO.DS_DETALHE, PRODUTO.VL_PESOBRU, PRODUTO.QT_ESTOQUE", strWhere.ToString(), "PRODUTO.DS_PROD");


        DataTable dtRetorno = new DataTable("TabelaItens");

        DataColumn column = new DataColumn();
        column.DataType = System.Type.GetType("System.String");
        column.ColumnName = "CD_PROD";
        dtRetorno.Columns.Add(column);

        dtRetorno.Columns.Add("CD_ALTER").DataType = System.Type.GetType("System.String");
        dtRetorno.Columns.Add("DS_PROD").DataType = System.Type.GetType("System.String");
        dtRetorno.Columns.Add("VL_PRECOVE").DataType = System.Type.GetType("System.Double");
        //dtRetorno.Columns.Add("CD_PROD").DataType = System.Type.GetType("System.String");
        dtRetorno.Columns.Add("VL_PESOBRU").DataType = System.Type.GetType("System.Double");
        dtRetorno.Columns.Add("QT_ESTOQUE").DataType = System.Type.GetType("System.Double");

        DataColumn[] keys = new DataColumn[1];
        keys[0] = column;
        dtRetorno.PrimaryKey = keys;

        DataRow drRet;

        foreach (DataRow dr in dtProduto.Rows)
        {
            drRet = dtRetorno.NewRow();
            drRet["CD_ALTER"] = dr["CD_ALTER"];
            drRet["DS_PROD"] = dr["DS_DETALHE"];
            drRet["VL_PRECOVE"] = dr["VL_PRECOVE"];
            drRet["CD_PROD"] = dr["CD_PROD"];
            drRet["VL_PESOBRU"] = dr["VL_PESOBRU"];
            drRet["QT_ESTOQUE"] = dr["QT_ESTOQUE"];

            dtRetorno.Rows.Add(drRet);
        }

        Session["DadosConsultaProduto"] = dtRetorno;
        return dtRetorno;
    }
    private double GetProdutoEspecifico(string CD_PROD)
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

        //Busca a Lista de Preço Padrão do cadastro de Cliente
        string ListaPreco = cbxListaPreco.Items[cbxListaPreco.Items.Count - 1].ToString();
        string st_atualizacao = string.Empty;
        decimal vl_perc = 0;


        DataTable dtListaPreco = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("LISTAPRE", "ST_ATUALIZACAO, VL_PERC", "CD_LISTA = '" + ListaPreco + "'");

        foreach (DataRow dr in dtListaPreco.Rows)
        {
            st_atualizacao = dr["ST_ATUALIZACAO"].ToString().Trim();
            vl_perc = (Convert.ToDecimal(dr["VL_PERC"]) / 100) + 1;
            break;
        }

        StringBuilder strWhere = new StringBuilder();
        strWhere.Append("(PRECOS.CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa.ToString().Trim() + "') AND ((PRODUTO.ST_INATIVO IS NULL) OR ");
        strWhere.Append("(PRODUTO.ST_INATIVO <> 'S')) AND ");
        strWhere.Append("((PRECOS.CD_LISTA = '" + (st_atualizacao.Equals("M") ? ListaPreco.Trim() : "00001") + "') AND (PRECOS.CD_PROD ='" + CD_PROD + "')) ");


        DataTable dtProduto = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("PRECOS INNER JOIN PRODUTO ON (PRODUTO.CD_PROD = PRECOS.CD_PROD)", (st_atualizacao.Equals("M") ? "PRECOS.VL_PRECOVE" : "(PRECOS.VL_PRECOVE * " + vl_perc.ToString().Replace(',', '.') + ")VL_PRECOVE"), strWhere.ToString());

        double VL_PRECOVE = 0;
        foreach (DataRow dr in dtProduto.Rows)
        {
            VL_PRECOVE = Convert.ToDouble(dr["VL_PRECOVE"]);

        }


        return VL_PRECOVE;
    }

    private DataTable GetTPDOC()
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

        string stpdocs = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("EMPRESA", "DS_TPDOCWEB", "CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa + "'");

        DataTable dtTPDOC = new DataTable();
        dtTPDOC.Columns.Add("CD_TPDOC", System.Type.GetType("System.String"));
        dtTPDOC.Columns.Add("DS_TPDOC", System.Type.GetType("System.String"));

        foreach (string item in stpdocs.Split(';'))
        {
            DataRow row = dtTPDOC.NewRow();
            string[] tpdoc = item.Split(',');
            if (tpdoc.Length > 1)
            {
                row["CD_TPDOC"] = tpdoc[1].ToString().Trim();
                row["DS_TPDOC"] = tpdoc[0].ToString().Trim();
                dtTPDOC.Rows.Add(row);
            }
        }
        return dtTPDOC;
    }




    private void IncluirNaLista(string strProduto, string strDescProd, string strCodAlter, string sVL_PROD, string sVL_PESOBRU)
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

        DataTable ListaProduto = CriaDataSet().Tables[0];

        DataRow row = ListaProduto.Rows.Find(strCodAlter + "-" + cbxListaPreco.SelectedValue.ToString());

        if (row == null)
        {
            string strValorUnitario = String.Empty;

            AdicionaProdutoLista(strDescProd, strCodAlter, sVL_PROD, sVL_PESOBRU, ListaProduto, row);
        }
        else if (!row["CD_LISTA"].Equals(cbxListaPreco.SelectedValue.ToString()))
        {
            AdicionaProdutoLista(strDescProd, strCodAlter, sVL_PROD, sVL_PESOBRU, ListaProduto, row);
        }
        else
        {
            string strAbrir = "if(window.alert('Já existe esse Produto: " + strProduto.Trim() + " - " + strDescProd.Trim() + " na Lista!'))";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Confirmar", strAbrir, true);
        }

        btnAtualiza.Visible = true;
        btnAvancar.Visible = true;
    }

    private void AdicionaProdutoLista(string strDescProd, string strCodAlter, string sVL_PROD, string sVL_PESOBRU, DataTable ListaProduto, DataRow row)
    {
        row = ListaProduto.NewRow();
        row["CD_ALTER"] = strCodAlter + "-" + cbxListaPreco.SelectedValue.ToString();
        row["DESC"] = strDescProd;
        //row["VL_PROD"] = String.Format("{0:N2}", strValorUnitario);
        row["CD_LISTA"] = cbxListaPreco.SelectedValue.ToString();
        row["VL_PROD"] = String.Format("{0:N4}", sVL_PROD);
        row["VL_PESOBRU"] = String.Format("{0:N4}", sVL_PESOBRU);
        row["QT_PROD"] = 1;
        ListaProduto.Rows.Add(row);
        GridViewNovo.DataSource = ListaProduto;
        GridViewNovo.DataBind();




    }

    private DataSet CriaDataSet()
    {
        if (Session["Lista"] == null)
        {

            DataSet ds = new DataSet();
            DataTable dt = new DataTable("Lista");
            DataColumn CD_ALTER = new DataColumn("CD_ALTER", System.Type.GetType("System.String"), "");
            DataColumn[] PkCollumn = new DataColumn[1];
            dt.Columns.Add(CD_ALTER);
            dt.Columns.Add("DESC", System.Type.GetType("System.String"), "");
            dt.Columns.Add("CD_LISTA", System.Type.GetType("System.String"), "");
            dt.Columns.Add("VL_PROD", System.Type.GetType("System.Double"), "");
            dt.Columns.Add("QT_PROD", System.Type.GetType("System.Double"), "");
            dt.Columns.Add("VL_PESOBRU", System.Type.GetType("System.Double"), "");
            dt.Columns.Add("SUBTOTAL", System.Type.GetType("System.Double"), "QT_PROD * VL_PROD");
            dt.Columns.Add("TOTAL", System.Type.GetType("System.Double"), "SUM(SUBTOTAL)");

            PkCollumn[0] = CD_ALTER;
            dt.PrimaryKey = PkCollumn;
            ds.Tables.Add(dt);




            Session["Lista"] = ds;

            return ds;

        }
        else
            return (DataSet)Session["Lista"];
    }

    protected void GridViewDb_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "QT_ESTOQUE")) <= 0)
            {
                e.Row.Cells[6].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFB3B3");//Color.Red;
                // e.Row.ForeColor = System.Drawing.Color.Red;
            }
        }
    }


    protected void cbxListaPreco_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnLoc_Click(sender, e);
    }

    protected void cbxPrazoPgto_SelectedIndexChanged(object sender, EventArgs e)
    {
        CarregaListaPreco();
        CarregaFormaPgto();

    }
}
