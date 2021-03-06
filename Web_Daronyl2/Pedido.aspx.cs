﻿using System;
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

            if (Session["PesquisaPendencia"] != null)
            {
                if ((bool)Session["PesquisaPendencia"])
                {
                    Session["CD_ALTER"] = CodigoCliente;
                    Session["NM_CLIFOR"] = txtCliente.Text;
                    Session["PesquisaPendencia"] = false;
                    Response.Redirect("~/PesquisaPendenciaCliente.aspx");
                }
            }
            CarregaDocs(CodigoCliente, sCdClifor);
            Session["lsCodigoInseridos"] = new Dictionary<string, double>();//OS 28143
        }
    }

    private void CarregaDocs(string CodigoCliente, string sCdClifor)
    {
        GetPrazo();
        GetTPDOC();
        GetTipoConbrancas();
        GetStFrete();
        if (cbxFormaPgto.Items.Count == 1)
            GetFormaPgto();

        cbxLinhaProduto.DataSource = GetLinhaProduto();
        cbxLinhaProduto.DataBind();
    }

    private void CarregaDadosCliente(out string CodigoCliente, out string sCdClifor)
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        if (Request["CD_ALTER"] != null)
            Session["CD_ALTER"] = Request["CD_ALTER"];
        CodigoCliente = (string)Session["CD_ALTER"];
        sCdClifor = "";

        if (CodigoCliente != null)
        {
            Session["NM_CLIFOR"] = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR", "NM_CLIFOR", "CD_ALTER = '" + CodigoCliente + "'");  //(string)Session["NM_CLIFOR"];
            string NomeCliente = (string)Session["NM_CLIFOR"];
            txtDataPedido.Text = GetDataFormatada(DateTime.Today.ToShortDateString().ToString());
            string sPrazoPadrao = "";
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
            // decimal dVL_PERDESC = Convert.ToDecimal(objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR", "COALESCE(VL_PERDESC,0) VL_PERDESC", "CD_CLIFOR = '" + sCdClifor + "'"));
            //txtDesconto.Text = "0";// dVL_PERDESC.ToString();
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
                decimal sValor = Convert.ToDecimal(Linha.Cells[10].Text.ToString());
                string sINDEX = Convert.ToInt32(sValor).ToString();
                txtQtde.Text = (Session["lsCodigoInseridos"] as Dictionary<string, double>).FirstOrDefault(c => c.Key == sINDEX).Value.ToString();//OS 28143
                dqtde = Convert.ToDouble(String.Format("{0:N2}", txtQtde.Text));
            }
            double dVL_UNIPROD = 0;
            double dVL_UNIPROD_SEM_DESC;
            ((TextBox)Linha.Cells[6].FindControl("txtValorUnit")).Text = ((TextBox)Linha.Cells[6].FindControl("txtValorUnit")).Text.Replace('.', ',');
            double.TryParse(((TextBox)Linha.Cells[6].FindControl("txtValorUnit")).Text, out dVL_UNIPROD_SEM_DESC);
            Linha.Cells[7].Text = dVL_UNIPROD_SEM_DESC.ToString();

            double dVL_DESC = 0;// (dVL_UNIPROD_SEM_DESC * Convert.ToDouble(txtDesconto.Text.Replace('.', ',')) / 100);

            if (Session["ST_DESC"].ToString().Equals("U"))
            {
                dVL_UNIPROD = dVL_UNIPROD_SEM_DESC - dVL_DESC;
            }
            else
            {
                dVL_UNIPROD = dVL_UNIPROD_SEM_DESC;
            }
            Linha.Cells[4].Text = String.Format("{0:N4}", dVL_DESC);


            ((TextBox)Linha.Cells[6].FindControl("txtValorUnit")).Text = String.Format("{0:N4}", dVL_UNIPROD);
            Linha.Cells[7].Text = String.Format("{0:N4}", dVL_UNIPROD_SEM_DESC);
            double Total = dqtde * dVL_UNIPROD;
            Linha.Cells[8].Text = String.Format("{0:N4}", Total);
            TotalAtual += Total;
            TotalDesconto += dVL_DESC * dqtde;
            TotalPedidoComDesc += dVL_UNIPROD_SEM_DESC * dqtde;
            iLoop++;
        }
        GridViewNovo.FooterRow.Cells[8].Text = String.Format("{0:N2}", TotalAtual);
        //txtTotalDesconto.Text = String.Format("{0:N2}", TotalDesconto);
        //double desconto = Convert.ToDouble(txtDesconto.Text);
        //TotalPedidoComDesc = TotalPedidoComDesc - (TotalPedidoComDesc * (desconto / 100));
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
        double Valor = Convert.ToDouble
            (
            ((TextBox)GridViewNovo.Rows[e.NewEditIndex].Cells[6].FindControl("txtValorUnit")).Text
            );
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
            ExcluirItem(Server.HtmlDecode(row.Cells[10].Text), Server.HtmlDecode(row.Cells[3].Text));
            (Session["lsCodigoInseridos"] as Dictionary<string, double>).Remove(Server.HtmlDecode(row.Cells[10].Text));//OS 28143
            //ColoriItensJaSelecionadosNaPesquisa();
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
            int id = IncluirNaLista(Server.HtmlDecode(row.Cells[1].Text), Server.HtmlDecode(row.Cells[3].Text), Server.HtmlDecode(row.Cells[1].Text), Server.HtmlDecode(lblValor.Text));


            //if (!(((Session["lsCodigoInseridos"] as Dictionary<string, double>).Where(c => c.Key == Server.HtmlDecode(row.Cells[1].Text)).Count()) > 0))
            //{
            (Session["lsCodigoInseridos"] as Dictionary<string, double>).Add(id.ToString(), 1);
            //}

            //ColoriItensJaSelecionadosNaPesquisa();
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
            dicDadosInseridos.Add(r.Cells[10].Text, Convert.ToDouble(txtQtde.Text));
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
            double ValorUnitario = Convert.ToDouble(((TextBox)e.Row.Cells[6].FindControl("txtValorUnit")).Text);
            double ValorTotal = dqtde * ValorUnitario;
            ((TextBox)e.Row.Cells[6].FindControl("txtValorUnit")).Text = String.Format("{0:N2}", ValorUnitario);
            e.Row.Cells[8].Text = String.Format("{0:N2}", ValorTotal);

        }

    }
    protected void Pesquisa_Click(object sender, EventArgs e)
    {
        dvAviso.DataSource = null;
        dvAviso.DataBind();

        GridViewDb.DataSource = GetProdutoGrid();
        GridViewDb.DataBind();

        //ColoriItensJaSelecionadosNaPesquisa();
    }

    //private void ColoriItensJaSelecionadosNaPesquisa()
    //{
    //    foreach (GridViewRow row in GridViewDb.Rows)
    //    {
    //        if (((Session["lsCodigoInseridos"] as Dictionary<string, double>).Where(c => c.Key == row.Cells[1].Text.Trim())).Count() > 0)//OS 28143
    //        {
    //            row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFB3B3");//Color.Red;
    //        }
    //        else
    //        {
    //            row.BackColor = Color.White;
    //        }
    //    }
    //    GridViewNovo.Caption = "<b>" + (Session["lsCodigoInseridos"] as Dictionary<string, double>).Count.ToString() + " Produto(s) inserido(s)</b>";//OS 28143
    //}
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
                    //string sST_FRETE = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("EMPRESA", "ST_RESPONSAVEL_FRETE", "CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa.Trim() + "'");
                    string sST_FRETE = cbxStFrete.SelectedValue;

                    strUpDatePed.Append(" UPDATE PEDIDO SET CD_PEDIDO = '" + strRetPedido.Trim());
                    strUpDatePed.Append("', ST_FRETE = '" + sST_FRETE);
                    strUpDatePed.Append("', VL_TOTALPED = '" + sVL_TOTALPED);
                    strUpDatePed.Append("', st_desccond = '" + "N");
                    strUpDatePed.Append("', ST_WEB = '" + "S");
                    if (cbxClassificacao.Text != "")
                        strUpDatePed.Append("', ST_CLAS = '" + cbxClassificacao.Text);
                    //strUpDatePed.Append("', VL_DESCONTO = '" + "-" + txtTotalDesconto.Text.Replace(".", "").Replace(",", "."));
                    //strUpDatePed.Append("', VL_PERDESC = '" + "-" + txtDesconto.Text.Replace(".", "").Replace(",", "."));
                    strUpDatePed.Append("', CD_CLIENTE = '" + sCdClifor);

                    strUpDatePed.Append("', CD_TPCOBR = '" + cbxConranca1.SelectedValue.ToString());
                    strUpDatePed.Append("', CD_TPCOBR2 = '" + cbxConranca2.SelectedValue.ToString());
                    strUpDatePed.Append("', ST_FORMAPAG = '" + cbxFormaPgto.SelectedValue.ToString());


                    strUpDatePed.Append("', NM_GUERRA = '" + objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR", "NM_GUERRA", "CD_ALTER = '" + txtCodCli.Text + "'")); // ((CliforDAO)(Session["ObjetoClienteDetalhado"])).RegistroAtual["NM_GUERRA"].ToString());                   
                    strUpDatePed.Append("', ST_DESC = '" + Session["ST_DESC"].ToString());
                    strUpDatePed.Append("', DT_PEDIDO = '" + txtDataPedido.Text.Replace("/", "."));
                    if (sCD_CFOP != "")
                    {
                        strUpDatePed.Append("', cd_cfops = '" + sCD_CFOP);
                        strUpDatePed.Append("', ds_cfop = '" + sDS_CFOP);
                    }
                    strUpDatePed.Append("', CD_TIPODOC = '" + sTipoDocumento);
                    strUpDatePed.Append("', CD_PRAZO = '" + cbxCD_PRAZO.SelectedItem.Value.Trim());

                    string sTransp = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR", "COALESCE(CD_TRANS,'')", "CD_ALTER = '" + txtCodCli.Text + "'");
                    if (sTransp != "")

                        strUpDatePed.Append("', CD_TRANS = '" + sTransp.Trim());
                    else
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


                    string sVL_COMISSAO = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("vendedor", "vl_percoav", "cd_vend = '" + objUsuario.CodigoVendedor + "'");

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

                            


                            string dVL_PROD = ((TextBox)row.Cells[6].FindControl("txtValorUnit")).Text.Trim().Replace(".", "").Replace(",", ".");
                            string sCor = ((TextBox)row.Cells[9].FindControl("txtCor")).Text.Trim().Replace(".", "").Replace(",", ".");
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

                                #region Calcula Desconto Item

                                //decimal dVL_PERDESC = Convert.ToDecimal(objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR", "COALESCE(VL_PERDESC,0) VL_PERDESC", "CD_CLIFOR = '" + Session["CD_CLIFOR"].ToString() + "'"));
                                decimal dCOEFDESC = 1;// 100 - Convert.ToDecimal(txtDesconto.Text.Replace('.', ','));
                                //  dCOEFDESC = dCOEFDESC / 100;

                                // Geral ->    UNITARIO,U; TOTAL,T

                                #endregion

                                string sUpdateItens = "Update {0} set {1} where {2} ;";
                                List<update> obljup = new List<update>();
                                if (cbxClassificacao.SelectedValue == "A")
                                    obljup.Add(new update { sCampo = "VL_PERENTR", sValor = "100" });
                                if (cbxClassificacao.SelectedValue == "B")
                                    obljup.Add(new update { sCampo = "VL_PERENTR", sValor = "50" }); 
                                obljup.Add(new update { sCampo = "VL_PERCOMI1", sValor = sVL_COMISSAO }); // OS 30137
                                obljup.Add(new update { sCampo = "DS_COR", sValor = sCor }); // OS 30137                                
                                obljup.Add(new update { sCampo = "CD_VEND1", sValor = objUsuario.oTabelas.CdVendedorAtual });
                                //obljup.Add(new update { sCampo = "VL_PERENTR", sValor = "100.00" });
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
                                //obljup.Add(new update { sCampo = "VL_PERCOMI1", sValor = "0" });//dVL_PERCOMI1.ToString() });
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
        else if (cbxFormaPgto.Text.Equals(String.Empty) || cbxFormaPgto.SelectedIndex == 0)
        {
            lblInfo.Visible = true;
            MessageHLP.ShowPopUpMsg("Forma de pagamento não foi informado!", this.Page);
        }
        else if (GridViewNovo.Rows.Count > 0)
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
        //ColoriItensJaSelecionadosNaPesquisa();
    }
    protected void GridDuplicatas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridDuplicatas.PageIndex = e.NewPageIndex;
        GridDuplicatas.DataSource = (DataTable)Session["DadosConsultaDuplicatas"];
        GridDuplicatas.DataBind();
    }
    private void ExcluirItem(string strindex, string strDesc)
    {
        DataSet ExcluirProduto = CriaDataSet();

        DataRow row = ExcluirProduto.Tables[0].Rows.Find(strindex);

        if (row != null)
        {
            ExcluirProduto.Tables[0].Rows.Remove(row);
            ExcluirProduto.AcceptChanges();
            GridViewNovo.DataSource = CriaDataSet();
            GridViewNovo.DataBind();
        }
        else
        {
            string strAbrir = "if(window.alert('Não foi possível excluir o Produto: " + strindex.Trim() + " - " + strDesc.Trim() + " da Lista!'))";
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
        string sCD_CLIFOR = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR", "CD_CLIFOR", "CD_ALTER = '" + txtCodCli.Text + "'");
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
        //strWhere.Append("((PRECOS.CD_LISTA = '" + (st_atualizacao.Equals("M") ? ListaPreco.Trim() : "00001") + "') AND (PRECOS.VL_PRECOVE > 0)) ");
        strWhere.Append("((PRECOS.CD_LISTA = '" + (st_atualizacao.Equals("M") ? ListaPreco.Trim() : "00001") + "')) ");
        if (!txtCodProd.Text.Equals(String.Empty))
            strWhere.Append("AND (PRODUTO.CD_PROD STARTING'" + sCodProduto + "') ");
        if (!txtProdDesc.Text.Equals(String.Empty))
            strWhere.Append("AND (PRODUTO.DS_PROD LIKE ('%" + txtProdDesc.Text.ToUpper().Trim() + "%')) ");
        strWhere.Append(" AND PRODUTO.CD_LINHA = '" + cbxLinhaProduto.SelectedValue.ToString() + "' "); // OS_27292

        DataTable dtProduto = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("PRECOS INNER JOIN PRODUTO ON (PRODUTO.CD_PROD = PRECOS.CD_PROD)" +
            " INNER JOIN CLAS_FIS ON PRODUTO.CD_CF = CLAS_FIS.CD_CF ",
            (st_atualizacao.Equals("M") ? "COALESCE(PRECOS.VL_PRECOVE,0)VL_PRECOVE" : "(COALESCE(PRECOS.VL_PRECOVE,0) * " + vl_perc.ToString().Replace(',', '.') + ")VL_PRECOVE") +
       ", PRODUTO.cd_alter, PRODUTO.CD_PROD, PRODUTO.DS_DETALHE, PRODUTO.VL_PESOBRU, PRODUTO.QT_ESTOQUE ",
        strWhere.ToString(), "PRODUTO.DS_DETALHE");


        DataTable dtRetorno = new DataTable("TabelaItens");

        DataColumn column = new DataColumn();
        column.DataType = System.Type.GetType("System.String");
        column.ColumnName = "CD_PROD";
        dtRetorno.Columns.Add(column);

        dtRetorno.Columns.Add("cd_alter").DataType = System.Type.GetType("System.String");
        dtRetorno.Columns.Add("DS_PROD").DataType = System.Type.GetType("System.String");
        dtRetorno.Columns.Add("VL_PRECOVE").DataType = System.Type.GetType("System.Double");
        dtRetorno.Columns.Add("VL_PESOBRU").DataType = System.Type.GetType("System.Double");
        dtRetorno.Columns.Add("QT_ESTOQUE").DataType = System.Type.GetType("System.Double");
        DataColumn[] keys = new DataColumn[1];
        keys[0] = column;
        dtRetorno.PrimaryKey = keys;

        DataRow drRet;
        string sVl_ultVenda = "";
        foreach (DataRow dr in dtProduto.Rows)
        {
            drRet = dtRetorno.NewRow();
            drRet["CD_PROD"] = dr["CD_PROD"];
            drRet["cd_alter"] = dr["cd_alter"];
            drRet["DS_PROD"] = dr["DS_DETALHE"];
            sVl_ultVenda =
                objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue
                (
                "prodclif",
                "vl_ultvend",
                string.Format("cd_clifor = '{0}' and cd_prod = '{1}'", sCD_CLIFOR, dr["CD_PROD"].ToString())
                );
            if (sVl_ultVenda != "")
                drRet["VL_PRECOVE"] = Convert.ToDecimal(sVl_ultVenda);
            else
                drRet["VL_PRECOVE"] = Convert.ToDecimal(dr["VL_PRECOVE"]);
            drRet["VL_PESOBRU"] = Convert.ToDecimal(dr["VL_PESOBRU"]);
            drRet["QT_ESTOQUE"] = dr["QT_ESTOQUE"];

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
    private void GetStFrete()
    {

        List<ModelToComboBox> l = new List<ModelToComboBox>();
        l.Add(new ModelToComboBox { ID_ITEM = "0", DS_ITEM = "0-EMITENTE" });
        l.Add(new ModelToComboBox { ID_ITEM = "1", DS_ITEM = "1-REMETENTE" });
        l.Add(new ModelToComboBox { ID_ITEM = "2", DS_ITEM = "2-DESTINATÁRIO" });
        l.Add(new ModelToComboBox { ID_ITEM = "3", DS_ITEM = "3-TERCEIROS" });
        l.Add(new ModelToComboBox { ID_ITEM = "9", DS_ITEM = "9-OUTROS" });

        if (cbxStFrete.DataSource == null)
        {
            cbxStFrete.Items.Clear();
            cbxStFrete.DataSource = l;
            cbxStFrete.DataBind();
        }
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        string sST_FRETE = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("EMPRESA", "ST_RESPONSAVEL_FRETE", "CD_EMPRESA = '" + objUsuario.oTabelas.sEmpresa.Trim() + "'");
        if (sST_FRETE != "")
        {
            cbxStFrete.SelectedValue = sST_FRETE;
        }

    }
    private void GetTipoConbrancas()
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        DataTable dtCobrancas = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("tpcobr", "DS_TPCOBR, CD_TPCOBR");
        if (cbxConranca1.DataSource == null && cbxConranca2.DataSource == null)
        {
            cbxConranca1.Items.Clear();
            cbxConranca1.DataSource = dtCobrancas;
            cbxConranca1.DataBind();

            cbxConranca2.Items.Clear();
            cbxConranca2.DataSource = dtCobrancas;
            cbxConranca2.DataBind();
        }

    }

    private void GetFormaPgto()
    {

        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        DataTable dtFormaPgto = new DataTable();
        dtFormaPgto.Columns.Add("CD_FORMA", System.Type.GetType("System.String"));
        dtFormaPgto.Columns.Add("DS_FORMA", System.Type.GetType("System.String"));

        string sResult = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("gerador2magnific", "cd_edicao", "nm_tela = 'PEDIDO' and cd_campo = '->ST_FORMAPAG'");
        DataRow rowItem;
        foreach (string item in sResult.Split(';'))
        {
            rowItem = dtFormaPgto.NewRow();
            rowItem["CD_FORMA"] = item.Split(',')[1];
            rowItem["DS_FORMA"] = item.Split(',')[0];
            dtFormaPgto.Rows.Add(rowItem);
        }

        cbxFormaPgto.DataSource = dtFormaPgto;
        cbxFormaPgto.DataBind();
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
        //if (sCodClifor != "")
        //{
        //    stpDocCliFor = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("CLIFOR", "coalesce(cd_tipodoc,'000')", "CD_ALTER = '" + sCodClifor + "'");
        //    if (stpDocCliFor != "000" && stpDocCliFor != "")
        //    {
        //        DataRow row = dtTPDOC.NewRow();
        //        row["CD_TPDOC"] = stpDocCliFor;
        //        row["DS_TPDOC"] = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("TPDOC", "coalesce(ds_tipodoc,'')", "cd_tipodoc = '" + stpDocCliFor + "'");
        //        dtTPDOC.Rows.Add(row);
        //    }
        //}
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
    private int IncluirNaLista(string strProduto, string strDescProd, string strCodigo, string _sVL_PROD)
    {
        decimal dVL_PROD = Convert.ToDecimal(_sVL_PROD);
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        DataTable ListaProduto = CriaDataSet().Tables[0];
        //DataRow row = ListaProduto.Rows.Find(strCodigo);
        DataRow row = null;

        //if (row == null)
        //{
        string strValorUnitario = String.Empty;

        row = ListaProduto.NewRow();
        row["DESC"] = strDescProd;
        row["CD_LISTA"] = cbxListaPreco.SelectedValue.ToString();
        decimal dVL_DESC = 0; // dVL_PROD * Convert.ToDecimal(txtDesconto.Text.Replace('.', ',')) / 100;
        row["VL_DESCONTO"] = String.Format("{0:N2}", dVL_DESC);
        row["VL_PROD"] = String.Format("{0:N4}", dVL_PROD);
        row["VL_UNIPROD_SEM_DESC"] = String.Format("{0:N4}", dVL_PROD);
        row["SUBTOTAL"] = string.Format("{0:N2}", dVL_PROD - dVL_DESC);
        row["QT_PROD"] = "1";
        row["CD_PROD"] = strCodigo;
        int id = 0;
        if (ListaProduto.Rows.Count > 0)
            id = ListaProduto.AsEnumerable().Select(c => Convert.ToInt32(c["INDEX"].ToString())).Max() + 1;
        row["INDEX"] = id;

        ListaProduto.Rows.Add(row);
        GridViewNovo.DataSource = ListaProduto;
        GridViewNovo.DataBind();
        //RecalculaGridNovo(true);            
        //}
        //else
        //{
        //    string strAbrir = "if(window.alert('Já existe esse Produto: " + strProduto.Trim() + " - " + strDescProd.Trim() + " na Lista!'))";
        //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Confirmar", strAbrir, true);
        //}

        btnAtualiza.Visible = true;
        btnAvancar.Visible = true;
        return id;

    }
    private DataSet CriaDataSet()
    {
        if (Session["Lista"] == null)
        {

            DataSet ds = new DataSet();
            DataTable dt = new DataTable("Lista");
            DataColumn INDEX = new DataColumn("INDEX", System.Type.GetType("System.Int32"));
            DataColumn[] PkCollumn = new DataColumn[1];
            dt.Columns.Add(INDEX);
            dt.Columns.Add("CD_PROD", System.Type.GetType("System.String"), "");
            dt.Columns.Add("DESC", System.Type.GetType("System.String"), "");
            dt.Columns.Add("SUBTOTAL", System.Type.GetType("System.Decimal"), "");//VL_PROD - (VL_PROD * VL_DESCONTO)
            dt.Columns.Add("CD_LISTA", System.Type.GetType("System.String"), "");
            dt.Columns.Add("VL_PROD", System.Type.GetType("System.Decimal"), "");
            dt.Columns.Add("VL_UNIPROD_SEM_DESC", System.Type.GetType("System.Decimal"), "");
            dt.Columns.Add("VL_DESCONTO", System.Type.GetType("System.Decimal"), "");
            dt.Columns.Add("QT_PROD", System.Type.GetType("System.String"), "");
            dt.Columns.Add("VL_PESOBRU", System.Type.GetType("System.Decimal"), "");
            dt.Columns.Add("TOTAL", System.Type.GetType("System.Decimal"), "SUM(SUBTOTAL)");
            dt.Columns.Add("DS_COR", System.Type.GetType("System.String"), "");

            PkCollumn[0] = INDEX;
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
            // BaseDAO.CancelarOperacaoObjetoDAO((BaseDAO)Session["ObjetoClienteDetalhado"]);

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



public sealed class ModelToComboBox
{
    public string ID_ITEM { get; set; }
    public string DS_ITEM { get; set; }
}
