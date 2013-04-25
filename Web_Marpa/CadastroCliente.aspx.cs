using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HLP.Web;
using HLP.Dados.Cadastro;
using HLP.Dados.Cadastro.Web;
using HLP.Dados;
using System.Data;
using HLP.Geral;
using HLP.Web.Controles;
using System.Text;
using System.Configuration;
using FirebirdSql.Data.FirebirdClient;
using System.Web.Configuration;

public partial class CadastroCliente : System.Web.UI.Page, ITelaCadastroWeb
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sUser = UsuarioWeb.GetNomeUsuarioConectado(Session);
        if (sUser == "")
        {
            Response.Redirect("~/Login.aspx");
        }
        object IncluiCliente = Request["inclui"];
        if (Convert.ToBoolean(IncluiCliente))
        {
            Session["IncluirClientePedido"] = false;
        }

        bool IncluiClientePedido = (bool)Session["IncluirClientePedido"];
        bool PesquisaPedido = false;
        if (Session["PesquisaPendencia"] != null)
        {
            PesquisaPedido = (bool)Session["PesquisaPendencia"];
        }


        if (IncluiClientePedido)
        {
            btnConfirmar.Text = "Incluir Pedido";
            SomenteLeitura(false);
        }
        else if (PesquisaPedido)
        {
            btnConfirmar.Text = "Verificar Pendencia";
            SomenteLeitura(false);
        }
        else
        {
            btnConfirmar.Text = "Confirmar";
            SomenteLeitura(true);
        }
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        CliforDAO objCliente = ClienteDAOWeb.GetInstanciaClienteDAOWeb(Session,
            objUsuario);


        bool bSolicitouInclusao = ((IncluiCliente != null) &&
            (Convert.ToBoolean(IncluiCliente)));
        if (bSolicitouInclusao)
        {
            //A lógica está assim, a fim de que ao se clicar novamente em
            //inclusão o sistema limpe todos os campos e inicie um novo
            //cadastro.
            if (objCliente.Status != StatusRegistro.Nenhum)
                objCliente.CancelarOperacao();
            Response.Redirect("~/CadastroCliente.aspx?incluindo=true");
            return;
        }

        IncluiCliente = Request["incluindo"];
        bool bIncluindo = ((IncluiCliente != null) && (Convert.ToBoolean(IncluiCliente)));
        if ((bIncluindo) && (objCliente.Status != StatusRegistro.IncluindoRegistro))
        {
            if (objCliente.Status == StatusRegistro.AlterandoRegistro)
                objCliente.CancelarOperacao();
            objCliente.ProcessarPrevalidaCad();
            //  lblAdvertencia.Text = String.Empty;
            btnConfirmar.Text = "Cadastrar";
            CarregarValoresCamposLista();
            //  objCliente.PreencherComponentesComValoresBanco(this);
            cbxStPessoaJ_SelectedIndexChanged(null, null);
            txtCdBanco.Text = WebConfigurationManager.AppSettings["CdContaBancariaDefault"];
        }
        else if (!bIncluindo)
        {
            bool bClienteValido;
            try
            {
                if (objCliente.Status == StatusRegistro.IncluindoRegistro)
                    objCliente.CancelarOperacao();
                string sCdClifor = Request["CD_ALTER"].ToString();
                DataRow UltimoCliente = objCliente.RegistroAtual;
                bool bPesquisarCliente = (UltimoCliente == null);
                if (!bPesquisarCliente)
                {
                    string sUltimoClientePesquisado = UltimoCliente["CD_ALTER"].ToString();
                    bPesquisarCliente = (!sUltimoClientePesquisado.Equals(sCdClifor));
                }
                if (bPesquisarCliente)
                {
                    DataTable dtClifor = objCliente.Select(
                        "(CD_ALTER = '" + sCdClifor.ToString() + "') AND " +
                        "(CD_VEND1 = '" + objUsuario.CodigoVendedor + "') AND " +
                        "((ST_INATIVO <> 'S') OR (ST_INATIVO IS NULL)) ");
                    objCliente.RegistroAtual = dtClifor.Rows[0];
                }
                bClienteValido = (objCliente.RegistroAtual != null);
                if ((bClienteValido) && (objCliente.Status != StatusRegistro.AlterandoRegistro))
                    objCliente.ProcessarPrevalidaAlt();
            }
            catch
            {
                bClienteValido = false;
            }
            if (!bClienteValido)
            {
                Response.Redirect("~/PesquisarClientes.aspx");
                return;
            }
            ProcessarDataBind(objCliente);
            if (objCliente.Status == StatusRegistro.IncluindoRegistro)
            {
                btnConfirmar.OnClientClick = "return confirm('Deseja salvar as informações ?');";
            }
        }
    }

    protected void cbxStPessoaJ_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sStPessoaJ = cbxStPessoaJ.SelectedValue;
        if (sStPessoaJ.Equals("S"))
        {
            MultiViewClientes.ActiveViewIndex = 0;
            txtCdCpf.Text = String.Empty;
            txtCdRg.Text = String.Empty;
        }
        else
        {
            MultiViewClientes.ActiveViewIndex = 1;
            txtCNPJ.Text = String.Empty;
            txtCdInsest.Text = String.Empty;
        }
    }

    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        if (btnConfirmar.Text == "Confirmar")
        {
            if (!ValoresValidosCliente())
                return;
            Session["CD_ALTER"] = lblCdClifor.Text.Trim();
            CliforDAO objCliente =
                (CliforDAO)Session["ObjetoClienteDetalhado"];
            if (objCliente != null)
            {
                if (objCliente.Status == StatusRegistro.IncluindoRegistro)
                    GravarClienteIncluido(objCliente);
                else
                {
                    objCliente.SetValoresRegistroAtual(this);
                    ExecutaPosvalidaCad(objCliente, false);
                }
            }
        }
        if (btnConfirmar.Text == "Incluir Pedido")
        {
            Session["CD_ALTER"] = lblCdClifor.Text.Trim();
            Session["NM_CLIFOR"] = txtNmGuerra.Text.Trim();
            Session["IncluirClientePedido"] = false;
            Response.Redirect("~/Pedido.aspx");
        }
        else if (btnConfirmar.Text == "Verificar Pendencia")
        {
            Session["CD_ALTER"] = lblCdClifor.Text.Trim();
            Session["NM_CLIFOR"] = txtNmGuerra.Text.Trim();
            Session["IncluirClientePedido"] = false;
            Response.Redirect("~/PesquisaPendenciaCliente.aspx");
        }
    }

    //Métodos
    private void ClearComponentes()
    {

    }
    private bool ExecutaPosvalidaCad(CliforDAO objCliente, bool bIncluindo)
    {
        bool bRetorno = objCliente.ProcessarPosvalidaCad();
        string sCdClifor = objCliente.GetValorPrimario("CD_CLIFOR");
        if (bRetorno)
        {
            if (!bIncluindo)
            {
                MessageHLP.ShowPopUpMsg(string.Format("Dados do Cliente {0}, foram alterados com sucesso! {1}Código: {2}", txtNmGuerra.GetValor(), Environment.NewLine, Session["CD_ALTER"].ToString()), this.Page);
            }
            else
            {
                MessageHLP.ShowPopUpMsg(string.Format("Cliente {0} salvo com sucesso! {1}Código: {2}", txtNmGuerra.GetValor(), Environment.NewLine, Session["CD_ALTER"].ToString()), this.Page);
            }
            objCliente.ClearComponentes(this);
            Response.Redirect("~/PesquisarClientes.aspx?CD_ALTER=" + Session["CD_ALTER"].ToString());
        }
        else
        {
            MessageHLP.ShowPopUpMsg(objCliente.GetErros(), this.Page);
        }
        return bRetorno;
    }
    private void GravarClienteIncluido(CliforDAO objCliente)
    {
        objCliente.SetValoresRegistroAtual(this);
        bool bContinua = ExecutaPosvalidaCad(objCliente, true);
        if (bContinua)
        {
            objCliente.RegistroAtual = null;
            btnConfirmar.Text = "Confirmar";
        }
    }
    private bool ValoresValidosCliente()
    {
        Dictionary<HlpWebTextBox, HlpWebLabel> CamposObrigatorios =
            new Dictionary<HlpWebTextBox, HlpWebLabel>();
        CamposObrigatorios.Add(txtNmGuerra, lblNmGuerra);
        CamposObrigatorios.Add(txtNmClifor, lblNmClifor);
        bool bPessoaJuridica = cbxStPessoaJ.SelectedValue.Equals("S");
        if (bPessoaJuridica)
        {
            CamposObrigatorios.Add(txtCNPJ, lblCdCNPJ);
        }
        else
        {
            CamposObrigatorios.Add(txtCdCpf, lblCdCpf);
            CamposObrigatorios.Add(txtCdRg, lblCdRg);
        }
        CamposObrigatorios.Add(txtDsEndnor, lblDsEndnor);
        CamposObrigatorios.Add(txtNumero, lblNumero);
        CamposObrigatorios.Add(txtNM_BAIRRONOR, lblNM_BAIRRONOR);
        CamposObrigatorios.Add(txtCdCepnor, lblCdCepnor);
        CamposObrigatorios.Add(txtCdFonenor, lblCdFonenor);

        bool bCamposNaoPreenchidos = false;
        foreach (HlpWebTextBox campo in CamposObrigatorios.Keys)
        {
            HlpWebLabel label = CamposObrigatorios[campo];
            if (campo.EmBranco())
            {
                label.Visible = true;
                bCamposNaoPreenchidos = true;
            }
            else
                label.Visible = false;
        }
        if (bCamposNaoPreenchidos)
        {
            MessageHLP.ShowPopUpMsg("Os campos com '*' devem ser obrigatoriamente preenchidos", this.Page);
            return false;
        }

        if (bPessoaJuridica)
        {
            if (!FormatoValidoCampo("CNPJ", "XX.XXX.XXX/XXXX-XX", txtCNPJ))
                return false;
            if (!HlpFuncoes.CNPJValido(txtCNPJ.GetValor().ToString()))
            {
                MessageHLP.ShowPopUpMsg("O número de CNPJ informado é inválido!", this.Page);
                return false;
            }
            //Inicio OS - 21175 Colocar verificação na IE.
            if (!txtCdInsest.Text.ToUpper().Equals("ISENTO"))
            {
                if (!ValidarInscEstadual(txtCdInsest.Text.Replace(".", "")))
                {
                    MessageHLP.ShowPopUpMsg("Inscrição Estadual só pode conter números." + Environment.NewLine +
                        "Caso o cliente seja ISENTO preencha o campo com o valor ISENTO.", this.Page);
                    return false;
                }
            }
            //FIM OS - 21175 Colocar verificação na IE.
        }
        else
        {
            if (!FormatoValidoCampo("CPF", "XXX.XXX.XXX-XX", txtCdCpf))
                return false;
            if (!HlpFuncoes.CPFValido(txtCdCpf.GetValor().ToString()))
            {
                MessageHLP.ShowPopUpMsg("O número de CPF informado é inválido!", this.Page);
                return false;
            }
        }

        if (!FormatoValidoCampo("CEP", "XXXXX-XXX", txtCdCepnor))
            return false;
        if ((!txtCdEmail.EmBranco()) && (!txtCdEmail.ValorValido()))
        {
            MessageHLP.ShowPopUpMsg("O endereço de e-mail informato é inválido!", this.Page);
            return false;
        }

        return true;
    }
    private void SomenteLeitura(bool bLeitura)
    {
        txtCdCepnor.Enabled = bLeitura;
        txtNM_BAIRRONOR.Enabled = bLeitura;
        txtNumero.Enabled = bLeitura;
        txtCdCpf.Enabled = bLeitura;
        txtCdCxpnor.Enabled = bLeitura;
        txtCdEmail.Enabled = bLeitura;
        txtCdFaxnor.Enabled = bLeitura;
        txtCdFonenor.Enabled = bLeitura;
        txtCdInsest.Enabled = bLeitura;
        txtCdRg.Enabled = bLeitura;
        txtCNPJ.Enabled = bLeitura;
        txtDsContato.Enabled = bLeitura;
        txtDsEndnor.Enabled = bLeitura;
        cbxCidades.Enabled = bLeitura;
        txtNmClifor.Enabled = bLeitura;
        txtNmGuerra.Enabled = bLeitura;
        cbxCdUfnor.Enabled = bLeitura;
        cbxStPessoaJ.Enabled = bLeitura;
    }
    public void CarregarValoresCamposLista()
    {
        if (!Page.IsPostBack)
        {
            cbxStPessoaJ.LimparValores(Session);
            cbxStPessoaJ.CarregarValores(Session);
            cbxCdUfnor.LimparValores(Session);
            cbxCdUfnor.CarregarValores(Session);
            CarregaCidades();
        }
    }
    public List<ITela> GetComponentesTelaCadastro()
    {
        List<ITela> componentes = new List<ITela>();
        componentes.Add(txtNmClifor);
        componentes.Add(txtNmGuerra);
        componentes.Add(cbxStPessoaJ);
        componentes.Add(txtCNPJ);
        componentes.Add(txtCdInsest);
        componentes.Add(txtCdCpf);
        componentes.Add(txtCdRg);
        componentes.Add(txtNM_BAIRRONOR);
        componentes.Add(txtNumero);
        componentes.Add(txtDsContato);
        componentes.Add(txtDsEndnor);
        componentes.Add(cbxCidades);
        componentes.Add(cbxCdUfnor);
        componentes.Add(txtCdCepnor);
        componentes.Add(txtCdCxpnor);
        componentes.Add(txtCdFonenor);
        componentes.Add(txtCdFaxnor);
        componentes.Add(txtCdEmail);
        componentes.Add(txtCdBanco);
        return componentes;
    }
    protected bool ValidarInscEstadual(string sTexto)
    {
        bool bValido = true;

        if (sTexto.IndexOf("A") > 0)
            bValido = false;
        if (sTexto.IndexOf("a") > 0)
            bValido = false;
        if (sTexto.IndexOf("B") > 0)
            bValido = false;
        if (sTexto.IndexOf("b") > 0)
            bValido = false;
        if (sTexto.IndexOf("C") > 0)
            bValido = false;
        if (sTexto.IndexOf("c") > 0)
            bValido = false;
        if (sTexto.IndexOf("D") > 0)
            bValido = false;
        if (sTexto.IndexOf("d") > 0)
            bValido = false;
        if (sTexto.IndexOf("E") > 0)
            bValido = false;
        if (sTexto.IndexOf("e") > 0)
            bValido = false;
        if (sTexto.IndexOf("F") > 0)
            bValido = false;
        if (sTexto.IndexOf("f") > 0)
            bValido = false;
        if (sTexto.IndexOf("G") > 0)
            bValido = false;
        if (sTexto.IndexOf("g") > 0)
            bValido = false;
        if (sTexto.IndexOf("H") > 0)
            bValido = false;
        if (sTexto.IndexOf("h") > 0)
            bValido = false;
        if (sTexto.IndexOf("I") > 0)
            bValido = false;
        if (sTexto.IndexOf("i") > 0)
            bValido = false;
        if (sTexto.IndexOf("J") > 0)
            bValido = false;
        if (sTexto.IndexOf("j") > 0)
            bValido = false;
        if (sTexto.IndexOf("K") > 0)
            bValido = false;
        if (sTexto.IndexOf("k") > 0)
            bValido = false;
        if (sTexto.IndexOf("L") > 0)
            bValido = false;
        if (sTexto.IndexOf("l") > 0)
            bValido = false;
        if (sTexto.IndexOf("M") > 0)
            bValido = false;
        if (sTexto.IndexOf("m") > 0)
            bValido = false;
        if (sTexto.IndexOf("N") > 0)
            bValido = false;
        if (sTexto.IndexOf("n") > 0)
            bValido = false;
        if (sTexto.IndexOf("O") > 0)
            bValido = false;
        if (sTexto.IndexOf("o") > 0)
            bValido = false;
        if (sTexto.IndexOf("P") > 0)
            bValido = false;
        if (sTexto.IndexOf("p") > 0)
            bValido = false;
        if (sTexto.IndexOf("Q") > 0)
            bValido = false;
        if (sTexto.IndexOf("q") > 0)
            bValido = false;
        if (sTexto.IndexOf("R") > 0)
            bValido = false;
        if (sTexto.IndexOf("r") > 0)
            bValido = false;
        if (sTexto.IndexOf("S") > 0)
            bValido = false;
        if (sTexto.IndexOf("s") > 0)
            bValido = false;
        if (sTexto.IndexOf("T") > 0)
            bValido = false;
        if (sTexto.IndexOf("t") > 0)
            bValido = false;
        if (sTexto.IndexOf("U") > 0)
            bValido = false;
        if (sTexto.IndexOf("u") > 0)
            bValido = false;
        if (sTexto.IndexOf("V") > 0)
            bValido = false;
        if (sTexto.IndexOf("v") > 0)
            bValido = false;
        if (sTexto.IndexOf("X") > 0)
            bValido = false;
        if (sTexto.IndexOf("x") > 0)
            bValido = false;
        if (sTexto.IndexOf("W") > 0)
            bValido = false;
        if (sTexto.IndexOf("w") > 0)
            bValido = false;
        if (sTexto.IndexOf("Y") > 0)
            bValido = false;
        if (sTexto.IndexOf("y") > 0)
            bValido = false;
        if (sTexto.IndexOf("Z") > 0)
            bValido = false;
        if (sTexto.IndexOf("z") > 0)
            bValido = false;
        if (sTexto.IndexOf("/") > 0)
            bValido = false;
        if (sTexto.IndexOf(".") > 0)
            bValido = false;
        if (sTexto.IndexOf("_") > 0)
            bValido = false;
        if (sTexto.IndexOf("-") > 0)
            bValido = false;

        return bValido;
    }
    private bool FormatoValidoCampo(string sDescricaoCampo, string sFormatoCampo,
      HlpWebTextBox campo)
    {
        bool bRetorno = campo.ValorValido();
        if (!bRetorno)
        {
            MessageHLP.ShowPopUpMsg("O campo '" + sDescricaoCampo +
                "' deve ser preenchido no formato '" + sFormatoCampo + "'", this.Page);
        }
        return bRetorno;
    }
    private void ProcessarDataBind(CliforDAO objCliente)
    {
        if (!Page.IsPostBack)
        {
            if (objCliente.RegistroAtual != null)
            {
                DataRow registroClifor = objCliente.RegistroAtual;
                lblCdClifor.Text = registroClifor["CD_ALTER"].ToString();

                CarregarValoresCamposLista();
                cbxCidades.LimparValores(Session);
                cbxCidades.SelectedValue = null;
                cbxCidades.DataSource = null;
                cbxCidades.DataBind();
                objCliente.PreencherComponentesComValoresBanco(this);

                CarregaCidades();
                cbxCidades.SetValor(registroClifor["NM_CIDNOR"].ToString());
                cbxCidades.DataBind();

                cbxStPessoaJ_SelectedIndexChanged(null, null);

                if (txtNumero.GetValor().ToString().Equals(""))
                {
                    txtNumero.SetValor("S/N");
                }
                //  lblAdvertencia.Visible = false;

            }
            else
            {
                //  painelDetalhe.Visible = false;
                //lblAdvertencia.Visible = true;
            }
        }
    }

    protected void cbxCdUfnor_SelectedIndexChanged(object sender, EventArgs e)
    {
        CarregaCidades();
    }

    private void CarregaCidades()
    {
        if (cbxCdUfnor.SelectedValue != null)
        {
            string sUF = cbxCdUfnor.SelectedValue;
            if (sUF != "")
            {
                cbxCidades.CarregarValores(Session, string.Format("CD_UFNOR = '{0}' ORDER BY NM_CIDNOR", sUF));
                cbxCidades.DataBind();
            }
        }
    }
    protected void btnBuscaCep_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtCdCepnor.Text != "")
            {
                HLP.WebServices.Cep ws = new HLP.WebServices.Cep();
                HLP.WebServices.Endereco end = ws.BuscaEndereco(txtCdCepnor.Text);

                if (end != null)
                {
                    txtDsEndnor.Text = end.Logradouro;
                    txtNM_BAIRRONOR.Text = end.Bairro;
                    cbxCdUfnor.SelectedValue = end.Uf.ToUpper();
                    CarregaCidades();
                    cbxCidades.SelectedValue = end.Cidade.ToUpper();
                    txtDsEndnor.Focus();
                }
            }
        }
        catch (Exception)
        {
        }
    }
}