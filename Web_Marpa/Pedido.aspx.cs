using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using HLP.Dados;
using HLP.Web;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Web.Configuration;
using HLP.Dados.Faturamento;
using HLP.Dados.Cadastro;
using System.Text.RegularExpressions;
using HLP.Dados.Cadastro.Web;
using System.Linq;
using System.Net;


public partial class Pedido : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtCodCli.Text = "";
            txtCliente.Text = "";
            string sUser = UsuarioWeb.GetNomeUsuarioConectado(Session);
            if (sUser == "")
            {
                Response.Redirect("~/Login.aspx");
            }
            string CodigoCliente;
            string sCdClifor;

            CarregaDadosCliente(out CodigoCliente, out sCdClifor);
            CarregaDocs(CodigoCliente, sCdClifor);

            Session["lsCodigoInseridos"] = new Dictionary<string, double>();//OS 28143
        }
    }

    private void CarregaDocs(string CodigoCliente, string sCdClifor)
    {
        GetPrazo();
        GetTPDOC();
        cbxLinhaProduto.DataSource = GetLinhaProduto();
        cbxLinhaProduto.DataBind();
    }

    private void CarregaDadosCliente(out string CodigoCliente, out string sCdClifor)
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

        CodigoCliente = (string)Session["CD_ALTER"];
        sCdClifor = "";
        string NomeCliente = (string)Session["NM_CLIFOR"];
        txtDataPedido.Text = GetDataFormatada(DateTime.Today.ToShortDateString().ToString());

        string sPrazoPadrao = "";

        if (CodigoCliente != null)
        {
            sCdClifor = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR", "CD_CLIFOR", "CD_ALTER = '" + CodigoCliente + "'");

            txtCodCli.Text = CodigoCliente;
            DataTable dtRet = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(string.Format("select count(NR_DOC)TOTAL from doc_ctr where coalesce(st_baixa,'') <> 'B' and cd_empresa = '{0}' and cd_cliente = '{1}' and dt_venci < current_date", objUsuario.oTabelas.sEmpresa, sCdClifor));
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

            txtCliente.Text = NomeCliente.ToUpper();
            Session["CD_UFNOR"] = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR", "cd_ufnor", "cd_clifor = '" + sCdClifor + "'");
            GetPrazo();
            sPrazoPadrao = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR", "CD_PRAZO", "cd_clifor = '" + sCdClifor + "'");
            if (sPrazoPadrao != "")
            {
                cbxCD_PRAZO.SelectedValue = sPrazoPadrao;
            }
            else
            {
                cbxCD_PRAZO.SelectedIndex = 0;
            }
            decimal dVL_PERDESC = Convert.ToDecimal(objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR", "COALESCE(VL_PERDESC,0) VL_PERDESC", "CD_CLIFOR = '" + sCdClifor + "'"));
            txtDesconto.Text = dVL_PERDESC.ToString();
            Session["ST_DESC"] = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("EMPRESA", "COALESCE(ST_DESC,'U')", "CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa + "'");
            //decimal dCOEFDESC = (100 - dVL_PERDESC) / 100;
        }
        else
        {
            Session["QtdePendencias"] = 0;
        }
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
        LimparSession();
        Session["IncluirClientePedido"] = true;
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
            string sListaPadrao = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR", "coalesce(CD_LISTA,'')CD_LISTA", "CD_ALTER = '" + txtCodCli.Text + "'");
            cbxListaPreco.Enabled = false;
            cbxListaPreco.BackColor = System.Drawing.ColorTranslator.FromHtml("#E5E5E5");//cinza;

            if (sListaPadrao == "")
            {
                sListaPadrao = "00001";
            }
            cbxListaPreco.DataTextField = "DS_LISTA";
            cbxListaPreco.DataValueField = "CD_LISTA";
            cbxListaPreco.DataSource = GetListaPrecos(sListaPadrao);
            cbxListaPreco.DataBind();

            cbxListaPreco.SelectedValue = sListaPadrao;
            GridViewDb.DataSource = GetProdutoGrid();
            GridViewDb.DataBind();
            txtTipoDoc.Text = cbxDS_TPDOCWEB.SelectedItem.ToString();

            string[] docs = new string[] {"005","006","007"};

            if (docs.Contains(cbxDS_TPDOCWEB.SelectedValue))
            {
                cbxCD_PRAZO.SelectedValue = "0000000";
            }
            
        }
        else
        {
            return;
        }
    }
    protected void btnAtualiza_Click(object sender, EventArgs e)
    {
        RecalculaGridNovo(false);
    }

    private double ValidaValorTextBoxQtde(TextBox txt)
    {
        Regex reg = new Regex(@"^\d{1,5}(\,\d{1,2})?$");

        if (txt.Text == "")
        {
            return 1;
        }
        else
        {
            if (reg.IsMatch(String.Format("{0:N2}", txt.Text)))
            {
                return Convert.ToDouble(String.Format("{0:N2}", txt.Text));
            }
            else
            {
                txt.Text = "1";
                return 1;
            }
        }
    }

    private void RecalculaGridNovo(bool bBuscaQtdSession)
    {
        double TotalAtual = 0;
        double TotalDesconto = 0;
        double TotalPedidoComDesc = 0;
        int iLoop = 0;
        Regex reg = new Regex(@"^\d{1,5}(\,\d{1,2})?$");
        foreach (GridViewRow Linha in GridViewNovo.Rows)
        {
            double dqtde = 1;
            TextBox txtQtde = (TextBox)Linha.Cells[5].FindControl("txtQtde");
            txtQtde.Text = txtQtde.Text.Replace('.', ',');
            if (!bBuscaQtdSession)
            {
                dqtde = ValidaValorTextBoxQtde(txtQtde);
                txtQtde.Text = dqtde.ToString();
            }
            else
            {
                string sCD_PROD = Linha.Cells[1].Text;
                txtQtde.Text = (Session["lsCodigoInseridos"] as Dictionary<string, double>).FirstOrDefault(c => c.Key == sCD_PROD).Value.ToString();//OS 28143
                dqtde = Convert.ToDouble(String.Format("{0:N2}", txtQtde.Text));
            }
            double dVL_UNIPROD = 0;
            double dVL_UNIPROD_SEM_DESC = Convert.ToDouble(Linha.Cells[7].Text);
            double dVL_DESC = (dVL_UNIPROD_SEM_DESC * Convert.ToDouble(txtDesconto.Text.Replace('.', ',')) / 100);

            if (Session["ST_DESC"].ToString().Equals("U"))
            {
                dVL_UNIPROD = dVL_UNIPROD_SEM_DESC - dVL_DESC;
            }
            else
            {
                dVL_UNIPROD = dVL_UNIPROD_SEM_DESC;
            }
            Linha.Cells[4].Text = String.Format("{0:N4}", dVL_DESC);
            Linha.Cells[6].Text = String.Format("{0:N4}", dVL_UNIPROD);
            Linha.Cells[7].Text = String.Format("{0:N4}", dVL_UNIPROD_SEM_DESC);
            double Total = dqtde * dVL_UNIPROD;
            Linha.Cells[8].Text = String.Format("{0:N4}", Total);
            TotalAtual += Total;
            TotalDesconto += dVL_DESC * dqtde;
            TotalPedidoComDesc += dVL_UNIPROD_SEM_DESC * dqtde;
            iLoop++;
        }
        GridViewNovo.FooterRow.Cells[8].Text = String.Format("{0:N2}", TotalAtual);
        txtTotalDesconto.Text = String.Format("{0:N2}", TotalDesconto);
        double desconto = Convert.ToDouble(txtDesconto.Text);
        TotalPedidoComDesc = TotalPedidoComDesc - (TotalPedidoComDesc * (desconto / 100));
        txtTotalPedidoSemDesc.Text = String.Format("{0:N2}", TotalPedidoComDesc);
    }
    protected void btnPesqCliente_Click(object sender, EventArgs e)
    {
        Session["IncluirClientePedido"] = true;
        Response.Redirect("~/ConsultaClientes.aspx");
    }
    protected void GridViewNovo_RowEditing(object sender, GridViewEditEventArgs e)
    {
        TextBox txtQtde = (TextBox)GridViewNovo.Rows[e.NewEditIndex].Cells[2].FindControl("txtQtde");
        txtQtde.Text = txtQtde.Text.Replace('.', ',');
        double dqtde = Convert.ToDouble(String.Format("{0:N2}", txtQtde.Text));
        double Valor = Convert.ToDouble(GridViewNovo.Rows[e.NewEditIndex].Cells[6].Text);
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
            CarregaSessionDadosInseridos();
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GridViewNovo.Rows[index];
            ExcluirItem(Server.HtmlDecode(row.Cells[1].Text), Server.HtmlDecode(row.Cells[3].Text));
            (Session["lsCodigoInseridos"] as Dictionary<string, double>).Remove(Server.HtmlDecode(row.Cells[1].Text));//OS 28143
            ColoriItensJaSelecionadosNaPesquisa();
            RecalculaGridNovo(true);
        }
    }
    protected void GridViewDb_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "Incluir")
        {
            CarregaSessionDadosInseridos();
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GridViewDb.Rows[index];
            Label lblValor = (Label)row.Cells[4].FindControl("Label1");
            IncluirNaLista(Server.HtmlDecode(row.Cells[1].Text), Server.HtmlDecode(row.Cells[3].Text), Server.HtmlDecode(row.Cells[1].Text), Server.HtmlDecode(lblValor.Text));


            if (!(((Session["lsCodigoInseridos"] as Dictionary<string, double>).Where(c => c.Key == Server.HtmlDecode(row.Cells[1].Text)).Count()) > 0))
            {
                (Session["lsCodigoInseridos"] as Dictionary<string, double>).Add(Server.HtmlDecode(row.Cells[1].Text), 1);
            }

            ColoriItensJaSelecionadosNaPesquisa();
            RecalculaGridNovo(true);

        }
        else if (e.CommandName == "Alertar")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GridViewDb.Rows[index];


            UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

            string strConn = ConfigurationManager.ConnectionStrings["ConnectionStringFB"].ConnectionString.ToString();
            FbConnection Conn = new FbConnection(strConn);
            FbCommand cmdInsertAviso = new FbCommand();
            cmdInsertAviso.CommandText = "SP_ALERTA_ESTWEB";
            cmdInsertAviso.CommandType = CommandType.StoredProcedure;
            cmdInsertAviso.Connection = Conn;
            Conn.Open();
            //SCD_CLIFOR VARCHAR(7),
            //SCD_VEND VARCHAR(7),
            //SCD_EMPRESA VARCHAR(3),
            //SCD_PRODUTO VARCHAR(7),
            //SNM_CLIFOR VARCHAR(60),
            //SNM_PROD VARCHAR(60),
            //SNM_VEND VARCHAR(60))

            string sCD_PROD = Server.HtmlDecode(row.Cells[1].Text);
            string sNM_PROD = Server.HtmlDecode(row.Cells[3].Text);
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

    private void CarregaSessionDadosInseridos()
    {
        //OS 28143 - INICIO
        Dictionary<string, double> dicDadosInseridos = new Dictionary<string, double>();
        foreach (GridViewRow r in GridViewNovo.Rows)
        {
            TextBox txtQtde = (TextBox)r.Cells[5].FindControl("txtQtde");
            txtQtde.Text = txtQtde.Text.Replace('.', ',');
            string svalor = txtQtde.Text;
            dicDadosInseridos.Add(r.Cells[1].Text, Convert.ToDouble(txtQtde.Text));
        }
        Session["lsCodigoInseridos"] = dicDadosInseridos;
        //OS 28143 - FIM
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
            TextBox txtQtde = (TextBox)e.Row.Cells[2].FindControl("txtQtde");
            txtQtde.Text = txtQtde.Text.Replace('.', ',');
            double dqtde = Convert.ToDouble(String.Format("{0:N2}", txtQtde.Text));
            double ValorUnitario = Convert.ToDouble(e.Row.Cells[6].Text);
            double ValorTotal = dqtde * ValorUnitario;
            e.Row.Cells[6].Text = String.Format("{0:N2}", ValorUnitario);
            e.Row.Cells[8].Text = String.Format("{0:N2}", ValorTotal);
        }

    }
    protected void Pesquisa_Click(object sender, EventArgs e)
    {
        dvAviso.DataSource = null;
        dvAviso.DataBind();

        GridViewDb.DataSource = GetProdutoGrid();
        GridViewDb.DataBind();

        ColoriItensJaSelecionadosNaPesquisa();
    }

    private void ColoriItensJaSelecionadosNaPesquisa()
    {
        foreach (GridViewRow row in GridViewDb.Rows)
        {
            if (((Session["lsCodigoInseridos"] as Dictionary<string, double>).Where(c => c.Key == row.Cells[1].Text.Trim())).Count() > 0)//OS 28143
            {
                row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFB3B3");//Color.Red;
            }
            else
            {
                row.BackColor = Color.White;
            }
        }
        GridViewNovo.Caption = "<b>" + (Session["lsCodigoInseridos"] as Dictionary<string, double>).Count.ToString() + " Produto(s) inserido(s)</b>";//OS 28143
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
    private void LimparSession()
    {
        Session["Lista"] = null;
        Session["CD_ALTER"] = null;
        Session["NM_CLIFOR"] = null;
    }
    protected void Cancel_Click(object sender, EventArgs e)
    {
        MessageHLP.ShowPopUpMsg("Cancelado!", this.Page);
    }
    protected struct update
    {
        public string sCampo { get; set; }
        public string sValor { get; set; }

    }

    public string GetComputerName(string clientIP)
    {
        try
        {
            var hostEntry = Dns.GetHostEntry(clientIP);
            return hostEntry.HostName;
        }
        catch (Exception ex)
        {
            return string.Empty;
        }
    }


    protected void btnGravar_Click(object sender, EventArgs e)
    {
        try
        {

            string clientMachineName = "";
            try
            {
                clientMachineName = GetComputerName(Request.UserHostAddress);
                //var WinNetwork = new ActiveXObject("WScript.Network");
                //var ComputerName = WinNetwork.ComputerName;
            }
            catch (Exception a)
            {
                clientMachineName = "No Machine";
            }


            //if (Session["CD_ALTER"] != null)
            if (txtCodCli.Text != "")
            {
                UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
                string sCdClifor = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR", "CD_CLIFOR", "CD_ALTER = '" + txtCodCli.Text + "'");

                string strConn = ConfigurationManager.ConnectionStrings["ConnectionStringFB"].ConnectionString.ToString();

                FbConnection Conn = new FbConnection(strConn);

                FbCommand cmdSelectPed = new FbCommand();
                cmdSelectPed.CommandText = "SP_INCLUI_PEDIDO_WEB";
                cmdSelectPed.CommandType = CommandType.StoredProcedure;
                cmdSelectPed.Connection = Conn;

                Conn.Open();
                if ((!objUsuario.oTabelas.hlpDbFuncoes.VerificaExistenciaGenerator("GEN_PEDIDO_WEB")))
                {
                    objUsuario.oTabelas.hlpDbFuncoes.CreateGenerator("GEN_PEDIDO_WEB", 3000000);

                }

                string sTipoDocumento = cbxDS_TPDOCWEB.SelectedValue.ToString(); // WebConfigurationManager.AppSettings["CdTipoDocPedidoDefault"];
                string Doc = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("TPDOC", "TP_DOC", "CD_TIPODOC='" + sTipoDocumento + "'");
                string sGenerator = "PEDIDO" + objUsuario.oTabelas.sEmpresa + Doc;
                string sCD_RELAPED = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("TPDOC", "coalesce(CD_RELAPED,'1')", "CD_TIPODOC='" + sTipoDocumento + "'");
                if (sCD_RELAPED == "1")
                {
                    sGenerator = "PEDIDO001NF";
                }

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









                string sUFclifor = Session["CD_UFNOR"].ToString();
                string sUFempresa = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("EMPRESA", "cd_ufnor", string.Format("cd_empresa = '{0}'", objUsuario.oTabelas.sEmpresa.Trim()));
                string sCD_CFOP = "";
                string sDS_CFOP = "";

                if (sUFclifor.Equals("EX"))
                {
                    sCD_CFOP = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("TPDOC", "cd_cfopext", string.Format("cd_tipodoc = '{0}'", sTipoDocumento));
                }
                else if (sUFclifor == sUFempresa)
                {
                    sCD_CFOP = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("TPDOC", "cd_cfopest", string.Format("cd_tipodoc = '{0}'", sTipoDocumento));
                }
                else
                {
                    sCD_CFOP = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("TPDOC", "cd_cfopfes", string.Format("cd_tipodoc = '{0}'", sTipoDocumento));
                }

                if (sCD_CFOP != "")
                {
                    sDS_CFOP = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("cfops", "ds_cfopabv", string.Format("cd_cfops = '{0}'", sCD_CFOP));
                }

                cmdSelectPed.Parameters.Add("@SCD_EMPRESA", FbDbType.VarChar, 3).Value = objUsuario.oTabelas.sEmpresa.ToString();
                cmdSelectPed.Parameters.Add("@SCD_VEND1", FbDbType.VarChar, 7).Value = objUsuario.CodigoVendedor.ToString();
                cmdSelectPed.Parameters.Add("@SCD_PEDIDO", FbDbType.VarChar, 7).Value = sCD_PEDIDO;

                string strRetPedido = cmdSelectPed.ExecuteScalar().ToString();

                Conn.Close();

                if (strRetPedido != null)
                {

                    StringBuilder strUpDatePed = new StringBuilder();

                    string sVL_TOTALPED = GridViewNovo.FooterRow.Cells[8].Text.Replace(".", "").Replace(",", ".");
                    string sST_FRETE = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("EMPRESA", "ST_RESPONSAVEL_FRETE", "CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa.Trim() + "'");

                    strUpDatePed.Append(" UPDATE PEDIDO SET CD_PEDIDO = '" + strRetPedido.Trim());
                    //strUpDatePed.Append("', HR_PEDIDO = '" + Session["HoraPedido"]);
                    strUpDatePed.Append("', HR_PEDIDO = '" + DateTime.Now.ToShortTimeString());
                    strUpDatePed.Append("', ST_FRETE = '" + sST_FRETE);
                    strUpDatePed.Append("', VL_TOTALPED = '" + sVL_TOTALPED);
                    strUpDatePed.Append("', st_desccond = '" + "N");
                    strUpDatePed.Append("', ST_WEB = '" + "S");

                    strUpDatePed.Append("', VL_DESCONTO = '" + "-" + txtTotalDesconto.Text.Replace(".", "").Replace(",", "."));
                    strUpDatePed.Append("', VL_PERDESC = '" + "-" + txtDesconto.Text.Replace(".", "").Replace(",", "."));

                    strUpDatePed.Append("', CD_CLIENTE = '" + sCdClifor);
                    strUpDatePed.Append("', NM_GUERRA = '" + ((CliforDAO)(Session["ObjetoClienteDetalhado"])).RegistroAtual["NM_GUERRA"].ToString());
                    strUpDatePed.Append("', ST_DESC = '" + Session["ST_DESC"].ToString());
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
                    if (sCD_CFOP != "")
                    {
                        strUpDatePed.Append("', cd_cfops = '" + sCD_CFOP);
                        strUpDatePed.Append("', ds_cfop = '" + sDS_CFOP);
                    }
                    strUpDatePed.Append("', CD_TIPODOC = '" + sTipoDocumento);
                    strUpDatePed.Append("', CD_PRAZO = '" + cbxCD_PRAZO.SelectedItem.Value.Trim());
                    strUpDatePed.Append("', CD_TRANS = '" + objUsuario.oTabelas.TRANSP.ToString().Trim());
                    strUpDatePed.Append("', DS_OBS_WEB = '" + txtObs.Text.ToUpper().Trim() + Environment.NewLine + "PEDIDO FEITO POR: " + clientMachineName.ToUpper());
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
                            string sTableItens = (WebConfigurationManager.AppSettings["TableItens"]).ToUpper();

                            FbCommand cmdSelectMoviPend = new FbCommand();
                            cmdSelectMoviPend.CommandText = sTableItens.Equals("MOVITEM") ? "SP_INCLUI_MOVITEM_WEB" : "sp_inclui_movipend_web";
                            cmdSelectMoviPend.CommandType = CommandType.StoredProcedure;
                            cmdSelectMoviPend.Connection = Conn;
                            Conn.Open();
                            cmdSelectMoviPend.Parameters.Add("@SCD_EMPRESA", FbDbType.VarChar, 3).Value = objUsuario.oTabelas.sEmpresa.ToString();
                            cmdSelectMoviPend.Parameters.Add("@SCD_PEDIDO", FbDbType.VarChar, 7).Value = strRetPedido.Trim();
                            string strRetMoviPend = cmdSelectMoviPend.ExecuteScalar().ToString();
                            Conn.Close();


                            //ITENS DA GRID INSERIDOS.
                            string sCD_PROD = row.Cells[1].Text.Trim();
                            string sCD_ALTER = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("PRODUTO", "CD_ALTER", "CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa.Trim() + "' AND CD_PROD ='" + sCD_PROD + "'");
                            TextBox txtQtde = (TextBox)row.Cells[2].FindControl("txtQtde");
                            string sQTDE = txtQtde.Text.Replace(".", "").Replace(",", ".");
                            string sDESC = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("PRODUTO", "DS_PROD", "CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa.Trim() + "' AND CD_PROD ='" + sCD_PROD + "'");
                            string dVL_DESCONTO = row.Cells[4].Text.Trim().Replace(".", "").Replace(",", ".");
                            string sCD_LISTA = row.Cells[5].Text.Trim();
                            string dVL_PROD = row.Cells[6].Text.Trim().Replace(".", "").Replace(",", ".");
                            string dVL_UNIPROD_SEM_DESC = row.Cells[7].Text.Trim().Replace(".", "").Replace(",", ".");
                            string dSUBTOTAL = row.Cells[8].Text.Trim().Replace(".", "").Replace(",", ".");

                            if (strRetMoviPend != null)
                            {
                                strUpDatePed.Length = 0;
                                string CF = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("PRODUTO", "CD_CF", "CD_PROD = '" + sCD_PROD + "'");
                                string UNID = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("PRODUTO", "CD_TPUNID", "CD_PROD = '" + sCD_PROD + "'");
                                string CODICMS = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("PRODUTO", "CD_ALIICMS", "CD_PROD = '" + sCD_PROD + "'");
                                string SITTRIB = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("PRODUTO", "CD_SITTRIB", "CD_PROD = '" + sCD_PROD + "'");
                                string scdOper = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("tpdoc", "substring(tpdoc.cd_operval from 1 for 3) cd_operval", "cd_tipodoc = '" + sTipoDocumento + "'");
                                if (objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("TPDOC", "coalesce(tp_doc,'')", "cd_tipodoc = '" + sTipoDocumento + "'").Equals("PE"))
                                {
                                    string sOPERVAL = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("TPDOC", "coalesce(cd_operval,'')", "cd_tipodoc = '" + sTipoDocumento + "'");


                                    string sOperProd = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("produto", "coalesce(cd_oper,'')", "cd_prod = '" + sCD_PROD + "'");
                                    if (sOperProd != "")
                                    {
                                        if (sOPERVAL.Contains(sOperProd))
                                        {
                                            scdOper = sOperProd;
                                        }
                                    }
                                }



                                string sSittribipi = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("PRODUTO", "cd_sittribipi", "CD_PROD = '" + sCD_PROD + "' and CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa + "'");
                                string sSittribpis = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("clas_fis", "cd_sittribpis", "cd_cf = '" + CF + "' and CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa + "'");
                                string sSittribcofins = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("clas_fis", "cd_sittribcof", "cd_cf = '" + CF + "' and CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa + "'");
                                string sTipoPrazo = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("PRAZOS", "DS_FORMULA", "CD_PRAZO = '" + cbxCD_PRAZO.SelectedItem.Value.Trim() + "'");


                                if (sSittribipi == "")
                                {
                                    sSittribipi = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("OPEREVE", "cd_sittribipi", "CD_OPER = '" + scdOper + "'");
                                }
                                if (sSittribpis == "")
                                {
                                    sSittribpis = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("OPEREVE", "cd_sittribpis", "CD_OPER = '" + scdOper + "'");
                                }
                                if (sSittribcofins == "")
                                {
                                    sSittribcofins = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("OPEREVE", "cd_sittribcof", "CD_OPER = '" + scdOper + "'");
                                }

                                #region Busca Valor % de comissão

                                string dVL_PERCOMI1 = "0";
                                string dVL_PERCOMI2 = "0";
                                //Verifica se o TPDOC se encontra no cadastro de empresa, caso sim, zera comissão.
                                string sCD_TPDOCBONI = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("EMPRESA", "cd_tpdocboni", "CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa + "'");
                                if (!sCD_TPDOCBONI.Contains(cbxDS_TPDOCWEB.SelectedValue.ToString()))
                                {
                                    string sST_COMREP = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR", "COALESCE(ST_COMREP,'C')", "CD_CLIFOR = '" + sCdClifor + "'");
                                    switch (sST_COMREP)
                                    {
                                        case "C": //busca comissão do cliente
                                            {
                                                dVL_PERCOMI1 = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR", "COALESCE(vl_percomi1,'0')", "CD_CLIFOR = '" + sCdClifor + "'");
                                                dVL_PERCOMI2 = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR", "COALESCE(vl_percomi2,'0')", "CD_CLIFOR = '" + sCdClifor + "'");
                                            } break;
                                        case "P":
                                            {
                                                dVL_PERCOMI1 = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("PRODUTO", "COALESCE(vl_percomi1,'0')", "CD_PROD = '" + sCD_PROD + "'");
                                                dVL_PERCOMI2 = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("PRODUTO", "COALESCE(vl_percomi2,'0')", "CD_PROD = '" + sCD_PROD + "'");
                                            } break;
                                        case "L":
                                            {
                                                FbCommand cmdSP_PRECOS = new FbCommand();
                                                cmdSP_PRECOS.CommandText = "SP_PRECOS";
                                                cmdSP_PRECOS.CommandType = CommandType.StoredProcedure;
                                                cmdSP_PRECOS.Connection = Conn;
                                                Conn.Open();

                                                //CD_EMPRESAINP VARCHAR(3),
                                                //CD_LISTAINI VARCHAR(5),
                                                //CD_LISTAFIM VARCHAR(5),
                                                //CD_PRODINI VARCHAR(7),
                                                //CD_PRODFIM VARCHAR(7))                                            
                                                cmdSP_PRECOS.Parameters.Add("@CD_EMPRESAINP", FbDbType.VarChar, 3).Value = objUsuario.oTabelas.sEmpresa;
                                                cmdSP_PRECOS.Parameters.Add("@CD_LISTAINI", FbDbType.VarChar, 5).Value = sCD_LISTA;
                                                cmdSP_PRECOS.Parameters.Add("@CD_LISTAFIM", FbDbType.VarChar, 5).Value = sCD_LISTA;
                                                cmdSP_PRECOS.Parameters.Add("@CD_PRODINI", FbDbType.VarChar, 7).Value = sCD_PROD;
                                                cmdSP_PRECOS.Parameters.Add("@CD_PRODFIM", FbDbType.VarChar, 7).Value = sCD_PROD;

                                                FbDataReader dr = cmdSP_PRECOS.ExecuteReader();
                                                dr.Read();
                                                dVL_PERCOMI1 = dr["VL_COMIVEN"].ToString();
                                                dVL_PERCOMI2 = dVL_PERCOMI1.Replace(".", "").Replace(",", ".");
                                                Conn.Close();

                                            } break;
                                        case "Q":
                                            {
                                                string sFORMULA = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("PRAZOS", "COALESCE(DS_FORMULA,'')", "CD_PRAZO = '" + cbxCD_PRAZO.SelectedValue.ToString() + "'");
                                                bool bAVISTA = false;
                                                if (sFORMULA.Length > 0)
                                                {
                                                    bAVISTA = sFORMULA[0].Equals("0") ? true : false;
                                                }
                                                if (bAVISTA)
                                                {
                                                    if (Convert.ToInt32(objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("vendcomi", "count(*)", "CD_PROD = '" + sCD_PROD + "' and CD_VEND = '" + objUsuario.oTabelas.CdVendedorAtual + "'")) > 0)
                                                    {
                                                        dVL_PERCOMI1 = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("vendcomi", "coalesce(VL_PERCOAV,0)", "CD_PROD = '" + sCD_PROD + "' and CD_VEND = '" + objUsuario.oTabelas.CdVendedorAtual + "'");
                                                        dVL_PERCOMI2 = dVL_PERCOMI1;
                                                    }
                                                }
                                                else
                                                {
                                                    if (Convert.ToInt32(objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("vendcomi", "count(*)", "CD_PROD = '" + sCD_PROD + "' and CD_VEND = '" + objUsuario.oTabelas.CdVendedorAtual + "'")) > 0)
                                                    {
                                                        dVL_PERCOMI1 = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("vendcomi", "coalesce(VL_PERCOAP,0)", "CD_PROD = '" + sCD_PROD + "' and CD_VEND = '" + objUsuario.oTabelas.CdVendedorAtual + "'");
                                                        dVL_PERCOMI2 = dVL_PERCOMI1;
                                                    }
                                                }

                                            } break;
                                    }
                                }
                                #endregion


                                #region Calcula Desconto Item

                                //decimal dVL_PERDESC = Convert.ToDecimal(objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR", "COALESCE(VL_PERDESC,0) VL_PERDESC", "CD_CLIFOR = '" + Session["CD_CLIFOR"].ToString() + "'"));
                                decimal dCOEFDESC = 100 - Convert.ToDecimal(txtDesconto.Text.Replace('.', ','));
                                dCOEFDESC = dCOEFDESC / 100;

                                // Geral ->    UNITARIO,U; TOTAL,T

                                #endregion



                                string sUpdateItens = "Update {0} set {1} where {2} ;";

                                List<update> obljup = new List<update>();
                                obljup.Add(new update { sCampo = "CD_VEND1", sValor = objUsuario.oTabelas.CdVendedorAtual });
                                obljup.Add(new update { sCampo = "VL_PERENTR", sValor = "100.00" });
                                obljup.Add(new update { sCampo = "NR_LANC", sValor = strRetMoviPend.Trim() });
                                obljup.Add(new update { sCampo = "CD_PEDIDO", sValor = strRetPedido.Trim() });
                                obljup.Add(new update { sCampo = "DS_PROD", sValor = sDESC });
                                obljup.Add(new update { sCampo = "CD_LISTA", sValor = sCD_LISTA });
                                obljup.Add(new update { sCampo = "CD_PROD", sValor = sCD_PROD });
                                obljup.Add(new update { sCampo = "CD_ALTER", sValor = sCD_ALTER });
                                obljup.Add(new update { sCampo = "QT_PROD", sValor = sQTDE });
                                obljup.Add(new update { sCampo = "VL_UNIPROD_SEM_DESC", sValor = dVL_UNIPROD_SEM_DESC });
                                obljup.Add(new update { sCampo = "VL_TOTLIQ", sValor = dSUBTOTAL });
                                obljup.Add(new update { sCampo = "VL_DESCONTO_VALOR", sValor = dVL_DESCONTO });
                                obljup.Add(new update { sCampo = "VL_UNIPROD", sValor = dVL_PROD });
                                obljup.Add(new update { sCampo = "CD_USUINC", sValor = objUsuario.oTabelas.CdUsuarioAtual.ToString() });
                                obljup.Add(new update { sCampo = "DT_LANC", sValor = txtDataPedido.Text.Replace("/", ".") });
                                obljup.Add(new update { sCampo = "CD_USUALT", sValor = objUsuario.oTabelas.CdUsuarioAtual.ToString() });
                                obljup.Add(new update { sCampo = "CD_CF", sValor = (CF.Equals(String.Empty) ? "0000000" : CF.Trim()) });
                                obljup.Add(new update { sCampo = "CD_TPUNID", sValor = UNID.Trim() });
                                obljup.Add(new update { sCampo = "CD_ALIICMS", sValor = CODICMS.Trim() });
                                obljup.Add(new update { sCampo = "CD_SITTRIB", sValor = SITTRIB.Trim() });
                                obljup.Add(new update { sCampo = "VL_COEF", sValor = dCOEFDESC.ToString().Replace(",", ".") });
                                obljup.Add(new update { sCampo = "VL_PERCOMI1", sValor = dVL_PERCOMI1.ToString() });
                                obljup.Add(new update { sCampo = "DT_PRAZOEN", sValor = txtDataPedido.Text.Replace("/", ".") });
                                obljup.Add(new update { sCampo = "VL_ALIIPI", sValor = HlpFuncoesFaturamento.GetAliquotaIPI(objUsuario.oTabelas.GetOperacaoDefault("CD_OPER"), txtCodCli.Text.Trim(), CF, objUsuario.oTabelas).ToString() });
                                obljup.Add(new update { sCampo = "CD_OPER", sValor = scdOper });
                                obljup.Add(new update { sCampo = "cd_sittribipi", sValor = sSittribipi });
                                obljup.Add(new update { sCampo = "cd_sittribpis", sValor = sSittribpis });
                                obljup.Add(new update { sCampo = "cd_sittribcof", sValor = sSittribcofins });


                                if (sTableItens.ToUpper().Equals("MOVITEM"))
                                {
                                    obljup.Add(new update { sCampo = "CD_SEQPED", sValor = iCountSeq.ToString().PadLeft(2, '0') });
                                    obljup.Add(new update { sCampo = "DT_DOC", sValor = txtDataPedido.Text.Replace("/", ".") });
                                }

                                string sCampos = string.Empty;
                                foreach (update up in obljup)
                                {
                                    sCampos += up.sCampo + " = '" + up.sValor + "'" + Environment.NewLine + " ,";
                                }
                                sCampos = sCampos.Remove(sCampos.Length - 1, 1);

                                string sWhere = " (NR_LANC = '" + strRetMoviPend.Trim() + "') AND (CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa.Trim() + "')";

                                sUpdateItens = string.Format(sUpdateItens, sTableItens, sCampos, sWhere);

                                FbCommand cmdUpDateMoviPend = new FbCommand();
                                cmdUpDateMoviPend.CommandText = sUpdateItens;
                                cmdUpDateMoviPend.CommandType = CommandType.Text;
                                cmdUpDateMoviPend.Connection = Conn;
                                Conn.Open();
                                Ret = cmdUpDateMoviPend.ExecuteNonQuery();
                                Conn.Close();
                                iCountSeq++;
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
        }
        catch (Exception ex)
        {
            MessageHLP.ShowPopUpMsg("Cadastro não foi concluído com sucesso!" + Environment.NewLine +
                    ex.Message, this.Page);
        }
    }
    protected void btnAvancar_Click(object sender, EventArgs e)
    {
        if (cbxCD_PRAZO.Text.Equals(String.Empty) || cbxCD_PRAZO.SelectedIndex == -1)
        {
            lblInfo.Visible = true;
            MessageHLP.ShowPopUpMsg("Condição de Pagamento não foi informado!", this.Page);
        }
        if (GridViewNovo.Rows.Count > 0)
        {
            btnAtualiza_Click(sender, e);
            txtObs.Text = "";
            if (Convert.ToInt32(Session["QtdePendencias"].ToString()) > 0)
            {
                txtObs.Text += "**EXISTE PENDÊNCIA FINANCEIRA PARA ESSE CLIENTE**";
            }
            MultiViewItensPed.ActiveViewIndex = 2;
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
        dvAviso.DataSource = null;
        dvAviso.DataBind();

        ((DataTable)Session["DadosConsultaProduto"]).DefaultView.RowFilter = null;

        GridViewDb.PageIndex = e.NewPageIndex;
        GridViewDb.DataSource = (DataTable)Session["DadosConsultaProduto"];
        GridViewDb.DataBind();
        ColoriItensJaSelecionadosNaPesquisa();
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

        GridViewNovo.DataBind();
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
        string sCdClifor = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR", "CD_CLIFOR", "CD_ALTER = '" + (string)txtCodCli.Text + "'");
        string sWhere = "coalesce(st_baixa,'') <> 'B' and cd_empresa = '" + objUsuario.oTabelas.sEmpresa.ToString().Trim() + "' and cd_cliente = '" + sCdClifor + "'";

        DataTable dtDuplicatas = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("DOC_CTR", "CD_DUPLI,dt_venci, vl_doc ", sWhere);

        DataTable dtRetono = new DataTable("TabelaDuplicatas");
        dtRetono.Columns.Add("CD_DUPLI").DataType = System.Type.GetType("System.String");
        dtRetono.Columns.Add("DT_VENCI").DataType = System.Type.GetType("System.DateTime");
        dtRetono.Columns.Add("VL_DOC").DataType = System.Type.GetType("System.Double");
        dtRetono.Columns.Add("TOT_VL_DOC", System.Type.GetType("System.Double"), "SUM(VL_DOC)");
        //dt.Columns.Add("TOTAL", System.Type.GetType("System.Double"), "SUM(SUBTOTAL)");

        DataRow dtRet;

        foreach (DataRow dr in dtDuplicatas.Rows)
        {
            dtRet = dtRetono.NewRow();
            dtRet["CD_DUPLI"] = dr["CD_DUPLI"];
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
        string sCodProduto = txtCodProd.Text.ToUpper().Trim().PadLeft(7, '0');
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
            strWhere.Append("AND (PRODUTO.CD_PROD STARTING'" + sCodProduto + "') ");
        if (!txtProdDesc.Text.Equals(String.Empty))
            strWhere.Append("AND (PRODUTO.DS_PROD LIKE ('%" + txtProdDesc.Text.ToUpper().Trim() + "%')) ");
        strWhere.Append(" AND PRODUTO.CD_LINHA = '" + cbxLinhaProduto.SelectedValue.ToString() + "' "); // OS_27292

        DataTable dtProduto = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("PRECOS INNER JOIN PRODUTO ON (PRODUTO.CD_PROD = PRECOS.CD_PROD)" +
            " INNER JOIN CLAS_FIS ON PRODUTO.CD_CF = CLAS_FIS.CD_CF ",
            (st_atualizacao.Equals("M") ? "PRECOS.VL_PRECOVE" : "(PRECOS.VL_PRECOVE * " + vl_perc.ToString().Replace(',', '.') + ")VL_PRECOVE") +
       ", PRODUTO.CD_BARRAS, PRODUTO.CD_PROD, PRODUTO.DS_DETALHE, PRODUTO.VL_PESOBRU, PRODUTO.QT_ESTOQUE, PRODUTO.CD_SITTRIB, coalesce(precos.vl_precove_subst,0)vl_precove_subst, coalesce(precos.vl_subst,0)vl_subst, " +
       "CLAS_FIS.DS_CLASFIS, CLAS_FIS.VL_ALIIPI, CLAS_FIS.VL_REDBASE", strWhere.ToString(), "PRODUTO.DS_DETALHE");


        DataTable dtRetorno = new DataTable("TabelaItens");

        DataColumn column = new DataColumn();
        column.DataType = System.Type.GetType("System.String");
        column.ColumnName = "CD_PROD";
        dtRetorno.Columns.Add(column);

        dtRetorno.Columns.Add("CD_BARRAS").DataType = System.Type.GetType("System.String");
        dtRetorno.Columns.Add("DS_PROD").DataType = System.Type.GetType("System.String");
        dtRetorno.Columns.Add("VL_PRECOVE").DataType = System.Type.GetType("System.Double");
        dtRetorno.Columns.Add("VL_PESOBRU").DataType = System.Type.GetType("System.Double");
        dtRetorno.Columns.Add("QT_ESTOQUE").DataType = System.Type.GetType("System.Double");
        dtRetorno.Columns.Add("DS_CLASFIS").DataType = System.Type.GetType("System.String");
        dtRetorno.Columns.Add("VL_ALIIPI").DataType = System.Type.GetType("System.Double");
        dtRetorno.Columns.Add("VL_REDBASE").DataType = System.Type.GetType("System.Double");
        dtRetorno.Columns.Add("VL_ALIQUOT").DataType = System.Type.GetType("System.Double");
        dtRetorno.Columns.Add("VL_ALISUBS").DataType = System.Type.GetType("System.Double");
        dtRetorno.Columns.Add("CD_SITTRIB").DataType = System.Type.GetType("System.String");
        dtRetorno.Columns.Add("vl_precove_subst").DataType = System.Type.GetType("System.Double");
        dtRetorno.Columns.Add("vl_subst").DataType = System.Type.GetType("System.Double");


        DataColumn[] keys = new DataColumn[1];
        keys[0] = column;
        dtRetorno.PrimaryKey = keys;

        DataRow drRet;

        foreach (DataRow dr in dtProduto.Rows)
        {
            drRet = dtRetorno.NewRow();
            drRet["CD_PROD"] = dr["CD_PROD"];
            drRet["CD_BARRAS"] = dr["CD_BARRAS"];
            drRet["DS_PROD"] = dr["DS_DETALHE"];
            drRet["CD_SITTRIB"] = dr["CD_SITTRIB"];
            drRet["VL_PRECOVE"] = Convert.ToDecimal(dr["VL_PRECOVE"]);
            drRet["VL_PESOBRU"] = Convert.ToDecimal(dr["VL_PESOBRU"]);
            drRet["QT_ESTOQUE"] = dr["QT_ESTOQUE"];
            drRet["DS_CLASFIS"] = dr["DS_CLASFIS"];
            drRet["VL_ALIIPI"] = Convert.ToDecimal(dr["VL_ALIIPI"]);
            drRet["VL_REDBASE"] = (dr["CD_SITTRIB"].ToString().Equals("040") ? 0 : 100 - Convert.ToDouble(dr["VL_REDBASE"]));
            drRet["vl_precove_subst"] = Convert.ToDecimal(dr["vl_precove_subst"]);
            drRet["vl_subst"] = Convert.ToDecimal(dr["vl_subst"]);

            DataTable dtAliquotas = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("ICM", "VL_ALIQUOT, VL_ALISUBS ", "cd_ufnor = '" + Session["CD_UFNOR"] + "' and icm.cd_aliicms =  (SELECT  PRODUTO.cd_aliicms FROM PRODUTO  WHERE CD_PROD = '" + sCodProduto + "')");

            if (dtAliquotas.Rows.Count > 0)
            {
                drRet["VL_ALIQUOT"] = Convert.ToDouble(dtAliquotas.Rows[0]["VL_ALIQUOT"].ToString());
                drRet["VL_ALISUBS"] = Convert.ToDouble(dtAliquotas.Rows[0]["VL_ALISUBS"].ToString());
            }
            else
            {
                drRet["VL_ALIQUOT"] = 0;
                drRet["VL_ALISUBS"] = 0;
            }
            dtRetorno.Rows.Add(drRet);
        }
        Session["DadosConsultaProduto"] = dtRetorno;
        return dtRetorno;
    }
    private void GetPrazo()
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        DataTable dtPrazo = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("PRAZOS", "DS_PRAZO, CD_PRAZO", "COALESCE(ST_WEB,'N') = 'S' AND (ST_PAGAR_OU_RECEBER IN ('A', 'V'))", "DS_PRAZO");
        if (cbxCD_PRAZO.DataSource == null)
        {
            cbxCD_PRAZO.Items.Clear();
            cbxCD_PRAZO.DataSource = dtPrazo;
            cbxCD_PRAZO.DataBind();
        }

    }
    private void GetTPDOC()
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        DataTable dtTPDOC = new DataTable();
        dtTPDOC.Columns.Add("CD_TPDOC", System.Type.GetType("System.String"));
        dtTPDOC.Columns.Add("DS_TPDOC", System.Type.GetType("System.String"));
        DataRow rowItem;
        foreach (DataRow row in objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("select t.ds_tipodoc DS_TPDOC, t.cd_tipodoc CD_TPDOC from tpdoc t where t.st_utiliza_web = 'S'").Rows)
        {
            rowItem = dtTPDOC.NewRow();
            rowItem["CD_TPDOC"] = row["CD_TPDOC"].ToString().Trim();
            rowItem["DS_TPDOC"] = row["DS_TPDOC"].ToString().Trim();
            dtTPDOC.Rows.Add(rowItem);
        }

        string sCodClifor = (string)txtCodCli.Text;
        string stpDocCliFor = "000";
        if (sCodClifor != null)
        {
            stpDocCliFor = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR", "coalesce(cd_tipodoc,'000')", "CD_ALTER = '" + sCodClifor + "'");
            if (stpDocCliFor != "000" && stpDocCliFor != "")
            {
                DataRow row = dtTPDOC.NewRow();
                row["CD_TPDOC"] = stpDocCliFor;
                row["DS_TPDOC"] = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("TPDOC", "coalesce(ds_tipodoc,'')", "cd_tipodoc = '" + stpDocCliFor + "'");
                dtTPDOC.Rows.Add(row);
            }
        }
        cbxDS_TPDOCWEB.DataSource = dtTPDOC;
        cbxDS_TPDOCWEB.DataBind();
        if (stpDocCliFor != "000")
        {
            cbxDS_TPDOCWEB.SelectedValue = stpDocCliFor;
        }
    }
    private DataTable GetLinhaProduto()
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];

        DataTable dtLinhaProd = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("linhapro", "cd_linha, ds_linha", "CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa + "' and st_linha = 'A' and coalesce(st_web,'S') = 'S' order by ds_linha");

        return dtLinhaProd;
    }
    private DataTable GetListaPrecos(string _sListaPadrao)
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        DataTable dtListas = null;

        string sListaPermit = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("ACESSO", "cd_listapermitida", "cd_vend = '" + objUsuario.oTabelas.CdVendedorAtual + "'");

        if (sListaPermit != "")
        {
            if (_sListaPadrao != "")
            {
                sListaPermit += "," + _sListaPadrao;
            }
            dtListas = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("listapre", "CD_LISTA, DS_LISTA", "CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa + "' AND cd_lista in (" + sListaPermit + ")");
        }
        else
        {
            dtListas = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("listapre", "CD_LISTA, DS_LISTA", "CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa + "'");
        }

        return dtListas;
    }
    private void IncluirNaLista(string strProduto, string strDescProd, string strCodigo, string _sVL_PROD)
    {
        decimal dVL_PROD = Convert.ToDecimal(_sVL_PROD);
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        DataTable ListaProduto = CriaDataSet().Tables[0];
        DataRow row = ListaProduto.Rows.Find(strCodigo);

        if (row == null)
        {
            string strValorUnitario = String.Empty;

            row = ListaProduto.NewRow();
            row["DESC"] = strDescProd;
            row["CD_LISTA"] = cbxListaPreco.SelectedValue.ToString();
            decimal dVL_DESC = dVL_PROD * Convert.ToDecimal(txtDesconto.Text.Replace('.', ',')) / 100;
            row["VL_DESCONTO"] = String.Format("{0:N2}", dVL_DESC);
            row["VL_PROD"] = String.Format("{0:N4}", dVL_PROD);
            row["VL_UNIPROD_SEM_DESC"] = String.Format("{0:N4}", dVL_PROD);
            row["SUBTOTAL"] = string.Format("{0:N2}", dVL_PROD - dVL_DESC);
            row["QT_PROD"] = "1";
            row["CD_PROD"] = strCodigo;

            ListaProduto.Rows.Add(row);
            GridViewNovo.DataSource = ListaProduto;
            GridViewNovo.DataBind();
            //RecalculaGridNovo(true);            
        }
        else
        {
            string strAbrir = "if(window.alert('Já existe esse Produto: " + strProduto.Trim() + " - " + strDescProd.Trim() + " na Lista!'))";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Confirmar", strAbrir, true);
        }

        btnAtualiza.Visible = true;
        btnAvancar.Visible = true;

    }
    private DataSet CriaDataSet()
    {
        if (Session["Lista"] == null)
        {

            DataSet ds = new DataSet();
            DataTable dt = new DataTable("Lista");
            DataColumn CD_PROD = new DataColumn("CD_PROD", System.Type.GetType("System.String"), "");
            DataColumn[] PkCollumn = new DataColumn[1];
            dt.Columns.Add(CD_PROD);
            dt.Columns.Add("DESC", System.Type.GetType("System.String"), "");
            dt.Columns.Add("SUBTOTAL", System.Type.GetType("System.Decimal"), "");//VL_PROD - (VL_PROD * VL_DESCONTO)
            dt.Columns.Add("CD_LISTA", System.Type.GetType("System.String"), "");
            dt.Columns.Add("VL_PROD", System.Type.GetType("System.Decimal"), "");
            dt.Columns.Add("VL_UNIPROD_SEM_DESC", System.Type.GetType("System.Decimal"), "");
            dt.Columns.Add("VL_DESCONTO", System.Type.GetType("System.Decimal"), "");
            dt.Columns.Add("QT_PROD", System.Type.GetType("System.String"), "");
            dt.Columns.Add("VL_PESOBRU", System.Type.GetType("System.Decimal"), "");
            dt.Columns.Add("TOTAL", System.Type.GetType("System.Decimal"), "SUM(SUBTOTAL)");

            PkCollumn[0] = CD_PROD;
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
                e.Row.Cells[7].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFB3B3");//Color.Red;
                // e.Row.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
    protected void cbxListaPreco_SelectedIndexChanged(object sender, EventArgs e)
    {
        dvAviso.DataSource = null;
        dvAviso.DataBind();
        Pesquisa_Click(sender, e);
    }
    protected void GridViewDb_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["sCodProd"] != null)
        {
            if (Session["sCodProd"].ToString().Equals(GridViewDb.SelectedDataKey[0].ToString()) && dvAviso.Rows.Count > 0)
            {
                dvAviso.DataSource = null;
                dvAviso.DataBind();
            }
            else
            {
                MostraDetalhesProduto();
            }
        }
        else
        {
            MostraDetalhesProduto();
        }
    }
    private void MostraDetalhesProduto()
    {
        Session["sCodProd"] = GridViewDb.SelectedDataKey[0].ToString();

        ((DataTable)Session["DadosConsultaProduto"]).DefaultView.RowFilter = "CD_PROD ='" + Session["sCodProd"] + "'";
        dvAviso.DataSource = ((DataTable)Session["DadosConsultaProduto"]).DefaultView;
        dvAviso.DataBind();
    }
    protected void dvAviso_PageIndexChanging(object sender, DetailsViewPageEventArgs e)
    {

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


        StringBuilder str = new StringBuilder();
        str.Append("SELECT CD_CLIFOR, CD_ALTER, NM_GUERRA, NM_CLIFOR, DS_ENDNOR, NR_ENDNOr, NM_BAIRRONOR, NM_CIDNOR, CD_UFNOR, ST_PESSOAJ, CD_CGC, cd_insest, CD_CPF, CD_RG, CD_CEPNOR, CD_FONENOR, CD_FAXNOR   ");
        str.Append("FROM CLIFOR ");
        str.Append("WHERE ");
        str.Append("((CLIFOR.ST_INATIVO <> 'S') OR (CLIFOR.ST_INATIVO IS NULL)) AND (CLIFOR.CD_VEND1 = '" + objUsuario.CodigoVendedor + "') AND (NM_GUERRA IS NOT NULL) ");
        str.Append("AND (CD_ALTER ='" + txtCodCli.Text + "')");
        dtClientes = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(
            str.ToString());
        DataColumn[] ChavePrimaria = new DataColumn[] { dtClientes.Columns["CD_ALTER"] };
        dtClientes.PrimaryKey = ChavePrimaria;
        Session["DadosConsultaClientes"] = dtClientes;


        if (dtClientes.Rows.Count == 0)
        {
            MessageHLP.ShowPopUpMsg("Não existem registros para o filtro selecionado", this.Page);
            if (Session["CD_ALTER"] != null)
            {
                txtCodCli.Text = Session["CD_ALTER"].ToString();
            }
            else
            {
                txtCodCli.Text = "";
            }
        }
        else
        {
            BaseDAO.CancelarOperacaoObjetoDAO((BaseDAO)Session["ObjetoClienteDetalhado"]);

            Session["CD_ALTER"] = txtCodCli.Text;
            Session["NM_CLIFOR"] = dtClientes.Rows[0]["NM_CLIFOR"].ToString();
            Session["IncluirClientePedido"] = false;
            CliforDAO objCliente = ClienteDAOWeb.GetInstanciaClienteDAOWeb(Session,
           objUsuario);
            objCliente.RegistroAtual = dtClientes.Rows[0];
            string CodigoCliente;
            string sCdClifor;
            CarregaDadosCliente(out CodigoCliente, out sCdClifor);

            CarregaDocs(CodigoCliente, sCdClifor);

            Session["lsCodigoInseridos"] = new Dictionary<string, double>();//OS 28143
        }

    }
}
