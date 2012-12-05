using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using FirebirdSql.Data.FirebirdClient;

public partial class CadastroRelatorioVisita : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strGrupo = (string)Session["Grupo"];

        if (!strGrupo.Equals("ADMIN"))
        {
            lblFaturar.Visible = false;
            lblTecnico.Visible = false;
            DDLTecnico.Visible = false;
            DDLFaturar.Visible = false;
        }
        if (!IsPostBack)
        {
            DataTable Cliente = GetCliente();
            DDLCliente.DataSource = Cliente;
            DDLCliente.DataMember = "Cliente";
            DDLCliente.DataTextField = "Nome";
            DDLCliente.DataValueField = "Id";
            DDLCliente.DataBind();

            DataTable Tecnico = GetTecnico();
            DDLTecnico.DataSource = Tecnico;
            DDLTecnico.DataMember = "Tecnico";
            DDLTecnico.DataTextField = "Nome";
            DDLTecnico.DataValueField = "Id";
            DDLTecnico.DataBind();

            //DDLSistema.Items.Clear();

            //DataTable Sistema = GetSistema();
            //DDLSistema.DataSource = Sistema;
            //DDLSistema.DataMember = "Sistema";
            //DDLSistema.DataTextField = "Nome";
            //DDLSistema.DataValueField = "Id";
            //DDLSistema.DataBind();

           

            if (Request.QueryString["IdRel"].ToString() != String.Empty)
            {
                if (Request.QueryString["IdRel"].ToString() == "incluir")
                {
                    Label15.Text = "Inclusão de novo Cadastro de Visita";
                    txtDataCadastro.Text = GetDataCorreta();
                    txtHoraInicio.Text = "00:00";
                    txtHoraFinal.Text = GetHoraCorreta(); 
                    txtHoraDesc.Text = "00:00";
                    lblHoraTotal.Text = "00:00";
                }
                else
                {
                    Label15.Text = "Alterar Cadastro de Visita: " + Request.QueryString["IdRel"].ToString();
                    string strCodRelatorio = Request.QueryString["IdRel"].ToString();

                    StringBuilder strSelect = new StringBuilder();

                    strSelect.Append("SELECT LANCREL.DT_REL, LANCREL.NM_CONTATO, ");
                    strSelect.Append("LANCREL.CD_CLIENTE, LANCREL.CD_OPERADO, ");
                    strSelect.Append("LANCREL.CD_SISTEMA, LANCREL.CD_TIPOREL, ");
                    strSelect.Append("LANCREL.ST_FATURA, LANCREL.HR_ENTRADA, ");
                    strSelect.Append("LANCREL.HR_DESCONTO, LANCREL.HR_TOTAL, LANCREL.HR_SAIDA, ");
                    strSelect.Append("LANCREL.NM_GUERRA, LANCREL.DS_OBS, ACESSO.NM_OPERADO, SISTEMA.DS_SISTEMA ");
                    strSelect.Append("FROM LANCREL ");
                    strSelect.Append("INNER JOIN ACESSO ON (ACESSO.CD_OPERADO = LANCREL.CD_OPERADO) ");
                    strSelect.Append("INNER JOIN SISTEMA ON (SISTEMA.CD_SISTEMA = LANCREL.CD_SISTEMA) ");
                    strSelect.Append("WHERE (NR_LANC = '" + strCodRelatorio + "')");

                    FbCommand cmdAutRel = (FbCommand)HlpBancoDados.CommandSelect(strSelect.ToString());
                    cmdAutRel.Connection.Open();

                    FbDataReader drRel = cmdAutRel.ExecuteReader();

                    if (drRel.Read())
                    {
                        txtDataCadastro.Text = GetFormataDataRetorno(drRel["DT_REL"].ToString().Replace(".", "/"));
                        txtContato.Text = drRel["NM_CONTATO"].ToString();
                        DDLCliente.SelectedValue = drRel["NM_GUERRA"].ToString();
                        DDLTecnico.SelectedValue = drRel["NM_OPERADO"].ToString();
                        DDLSistema.SelectedValue = drRel["DS_SISTEMA"].ToString();
                        if (drRel["CD_TIPOREL"].ToString().Equals("4"))
                            DDLTipoRelatoio.Text = "SOFTWARE";
                        else
                            DDLTipoRelatoio.Text = "HARDWARE";
                        if (drRel["ST_FATURA"].ToString().Equals("S"))
                            DDLFaturar.Text = "SIM";
                        else
                            DDLFaturar.Text = "NÃO";
                        txtHoraInicio.Text = drRel["HR_ENTRADA"].ToString();
                        txtHoraFinal.Text = drRel["HR_SAIDA"].ToString();
                        txtHoraDesc.Text = drRel["HR_DESCONTO"].ToString();
                        lblHoraTotal.Text = drRel["HR_TOTAL"].ToString();

                        if (!drRel["DS_OBS"].ToString().Equals(String.Empty))
                        {
                            byte[] bt = (byte[])drRel["DS_OBS"];
                            txtMemo.Text = ASCIIEncoding.Default.GetString(bt);
                        }

                    }
                    drRel.Close();
                    cmdAutRel.Connection.Close();
                }
            }

            DDLTituloRel.Items.Clear();
            DDLTituloRel.Items.Add("MANUTENCAO");
            DDLTituloRel.Items.Add("ORCAMENTO");
            DDLTituloRel.Items.Add("GARANTIA");
            DDLTituloRel.Items.Add("OUTROS");

            DDLTituloRel.DataBind();
        }
    }

    protected DataTable GetCliente()
    {
        StringBuilder strCliente = new StringBuilder();

        strCliente.Append("SELECT DISTINCT CLIFOR.NM_GUERRA, CLIFOR.CD_CLIFOR ");
        strCliente.Append("FROM CLIFOR INNER JOIN SITCONTR ON (SITCONTR.CD_CLIFOR = CLIFOR.CD_CLIFOR) ");
        strCliente.Append("WHERE (CLIFOR.CD_CLIFOR IS NOT NULL) AND (CLIFOR.ST_INATIVO = 'N') ");
        strCliente.Append("GROUP BY CLIFOR.NM_GUERRA, CLIFOR.CD_CLIFOR, ");
        strCliente.Append("CLIFOR.ST_ATIVOSUPORTE, SITCONTR.ST_CONT_HORAS, SITCONTR.ST_MANUT ");
        strCliente.Append("HAVING (CASE WHEN CLIFOR.ST_ATIVOSUPORTE IS NULL THEN ");
        strCliente.Append("(CASE WHEN SITCONTR.ST_CONT_HORAS = 'N' THEN ");
        strCliente.Append("(CASE WHEN SITCONTR.ST_MANUT = 'S' THEN 'S' ELSE SITCONTR.ST_MANUT END) ");
        strCliente.Append("ELSE (CASE WHEN SITCONTR.ST_CONT_HORAS IS NULL THEN 'N' ELSE SITCONTR.ST_CONT_HORAS END) END) ");
        strCliente.Append("ELSE CLIFOR.ST_ATIVOSUPORTE END) = 'S' ");
        strCliente.Append("ORDER BY CLIFOR.NM_GUERRA");

        FbCommand fbSelect = (FbCommand)HlpBancoDados.CommandSelect(strCliente.ToString());

        fbSelect.Connection.Open();

        FbDataReader drCliente = fbSelect.ExecuteReader();

        DataTable dtCliente = new DataTable("Cliente");
        dtCliente.Columns.Add("Nome").AllowDBNull = true;
        dtCliente.Columns.Add("Id").AllowDBNull = true;

        DataRow dr;

        while (drCliente.Read())
        {
            dr = dtCliente.NewRow();
            dr["Nome"] = drCliente["NM_GUERRA"];
            dr["Id"] = drCliente["CD_CLIFOR"];
            dtCliente.Rows.Add(dr);
        }

        drCliente.Close();
        fbSelect.Connection.Close();

        return dtCliente;

    }

    protected DataTable GetTecnico()
    {
        StringBuilder strTecnico = new StringBuilder();

        strTecnico.Append("SELECT ACESSO.NM_OPERADO, ACESSO.CD_OPERADO ");
        strTecnico.Append("FROM ACESSO ");
        strTecnico.Append("ORDER BY ACESSO.NM_OPERADO");

        FbCommand fbSelect = (FbCommand)HlpBancoDados.CommandSelect(strTecnico.ToString());

        fbSelect.Connection.Open();

        FbDataReader drTecnico = fbSelect.ExecuteReader();

        DataTable dtTecnico = new DataTable("Tecnico");
        dtTecnico.Columns.Add("Nome").AllowDBNull = true;
        dtTecnico.Columns.Add("Id").AllowDBNull = true;

        DataRow dr;

        while (drTecnico.Read())
        {
            dr = dtTecnico.NewRow();
            dr["Nome"] = drTecnico["NM_OPERADO"];
            dr["Id"] = drTecnico["CD_OPERADO"];
            dtTecnico.Rows.Add(dr);
        }

        drTecnico.Close();
        fbSelect.Connection.Close();

        return dtTecnico;

    }
    protected DataTable GetSistema()
    {
        StringBuilder strSistema = new StringBuilder();
        if (!RadioButtonList1.SelectedItem.Text.Equals("SIM"))
        {
            strSistema.Append("SELECT SISTEMA.DS_SISTEMA, SISTEMA.CD_SISTEMA ");
            strSistema.Append("FROM SISTEMA ");
            strSistema.Append("ORDER BY SISTEMA.DS_SISTEMA");
        }
        else
        {
            strSistema.Append("SELECT SITCONTR.CD_SISTEMA, SISTEMA.DS_SISTEMA ");
            strSistema.Append("FROM SITCONTR ");
            strSistema.Append("INNER JOIN SISTEMA ON (SISTEMA.CD_SISTEMA = SITCONTR.CD_SISTEMA) ");
            strSistema.Append("WHERE (SITCONTR.CD_CLIFOR = '" + DDLCliente.SelectedItem.Value + "') ");
            strSistema.Append("ORDER BY SISTEMA.DS_SISTEMA");
        }

        FbCommand fbSelect = (FbCommand)HlpBancoDados.CommandSelect(strSistema.ToString());

        fbSelect.Connection.Open();

        FbDataReader drSistema = fbSelect.ExecuteReader();

        DataTable dtSistema = new DataTable("Sistema");
        dtSistema.Columns.Add("Nome").AllowDBNull = true;
        dtSistema.Columns.Add("Id").AllowDBNull = true;

        DataRow dr;

        while (drSistema.Read())
        {
            dr = dtSistema.NewRow();
            dr["Nome"] = drSistema["DS_SISTEMA"];
            dr["Id"] = drSistema["CD_SISTEMA"];
            dtSistema.Rows.Add(dr);
        }

        drSistema.Close();
        fbSelect.Connection.Close();

        return dtSistema;

    }
    
    protected void DDLTipoRelatoio_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DDLTituloRel.Items.Clear();

        //if (DDLTipoRelatoio.SelectedValue.Equals("SOFTWARE"))
        //{
        //    DDLTituloRel.Items.Add("TREINAMENTO");
        //    DDLTituloRel.Items.Add("ASSESSORIA");
        //    DDLTituloRel.Items.Add("INSTALACAO");
        //    DDLTituloRel.Items.Add("ATUALIZACAO");
        //    DDLTituloRel.Items.Add("OUTROS");
        //}
        //else
        //{
        //    DDLTituloRel.Items.Add("MANUTENCAO");
        //    DDLTituloRel.Items.Add("ORCAMENTO");
        //    DDLTituloRel.Items.Add("GARANTIA");
        //    DDLTituloRel.Items.Add("OUTROS");
        //}

        //DDLTituloRel.DataBind();
	}

    protected string GetDataCorreta()
    {
        string sDia = DateTime.Today.Day.ToString();
        string sMes = DateTime.Today.Month.ToString();
        string sAno = DateTime.Today.Year.ToString();

        if (sDia.Length < 2)
            sDia = "00".Substring(0, 2 - sDia.Length) + sDia;
        if (sMes.Length < 2)
            sMes = "00".Substring(0, 2 - sMes.Length) + sMes;
        string sRetorno = sDia + "/" + sMes + "/" + sAno;

        return sRetorno;

    }

    protected string GetHoraCorreta()
    {
        string sHora = DateTime.Now.Hour.ToString();
        string sMinutos = DateTime.Now.Minute.ToString();

        if (sHora.Length < 2)
            sHora = "00".Substring(0, 2 - sHora.Length) + sHora;
        if (sMinutos.Length < 2)
            sMinutos = "00".Substring(0, 2 - sMinutos.Length) + sMinutos;

        string strTime = sHora + ":" + sMinutos;

        return strTime;
    }
    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }

    protected string GetHoraCalculada()
    {
        DateTime dInicio = Convert.ToDateTime(txtHoraInicio.Text);
        DateTime dFinal = Convert.ToDateTime(txtHoraFinal.Text);
        TimeSpan dTotal = dFinal.Subtract(dInicio);
        
        return dTotal.ToString().Substring(0,5);
    }

    protected string GetExisteDesconto()
    {
        
        DateTime dTotalAtua = Convert.ToDateTime(GetHoraCalculada());
        DateTime dDesconto = Convert.ToDateTime(txtHoraDesc.Text);
        TimeSpan tTotal = dTotalAtua.Subtract(dDesconto);
       
        return tTotal.ToString().Substring(0, 5);
    }

    protected string GetFormataDataRetorno(string strData)
    {
        string sRetorno = String.Empty;

        if (!strData.Equals(String.Empty))
        {
            DateTime tDataAtua = Convert.ToDateTime(strData);
            string sDia = tDataAtua.Day.ToString();
            string sMes = tDataAtua.Month.ToString();
            string sAno = tDataAtua.Year.ToString();
           
            if (sDia.Length < 2)
                sDia = "00".Substring(0, 2 - sDia.Length) + sDia;
            if (sMes.Length < 2)
                sMes = "00".Substring(0, 2 - sMes.Length) + sMes;
            sRetorno = sDia + "/" + sMes + "/" + sAno;

            
        }
        return sRetorno;
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
    
        if (ValidaCalculoHora())
        {
            lblHoraTotal.Text = GetExisteDesconto();
            lblErroHora.Text = String.Empty;
            lblErroHora.Visible = false;
            txtHoraDesc.Focus();
        }
        else
        {
            lblErroHora.Visible = true;
            lblErroHora.Text = " * Hora término menor que Hora inicial";
            txtHoraFinal.Focus();
        }
    
    }


    protected void btnGravar_Click(object sender, EventArgs e)
    {
        string strCodUsuario = (string)Session["CodigoUsuario"];
        string strLancRel = String.Empty;
        string strItLancRel = String.Empty;

        if (bCadastroValido())
        {
            if (Request.QueryString["IdRel"].ToString() == "incluir")
            {
                FbCommand cmdProcedure = (FbCommand)HlpBancoDados.CommandProcedure("SP_INCLUI_LANCREL");

                cmdProcedure.Connection.Open();

                strLancRel = cmdProcedure.ExecuteScalar().ToString();

                cmdProcedure.Connection.Close();
            }
            else
                strLancRel = Request.QueryString["IdRel"].ToString();

            StringBuilder strInserir = new StringBuilder();
            strInserir.Append("UPDATE LANCREL SET ");
            strInserir.Append("DT_REL = '" + txtDataCadastro.Text.Replace("/", ".").Trim() + "', NM_CONTATO = '" + txtContato.Text.ToUpper().Trim() + "', ");
            if (DDLCliente.Visible)
                strInserir.Append("CD_CLIENTE = '" + DDLCliente.SelectedValue + "', ");
            strInserir.Append("CD_OPERADO = '");
            if (DDLTecnico.Visible)
                strInserir.Append(DDLTecnico.SelectedValue + "', ");
            else
                strInserir.Append(strCodUsuario + "', ");

            strInserir.Append("CD_SISTEMA = '" + DDLSistema.SelectedValue + "', CD_TIPOREL = '");
            if (DDLTipoRelatoio.SelectedValue.Equals("SOFTWARE"))
            {
                if (DDLVncLocal.SelectedValue.Equals("VNC"))
                    strInserir.Append("5" + "', ");
                else
                    strInserir.Append("4" + "', ");
            }
            else
                strInserir.Append("3" + "', ");
            strInserir.Append("ST_FATURA = '");
            if (DDLFaturar.Visible)
            {
                if (DDLFaturar.SelectedValue.Equals("SIM"))
                    strInserir.Append("S" + "', ");
                else
                    strInserir.Append("N" + "', ");
            }
            else
            {
                if (DDLTituloRel.SelectedValue.Equals("GARANTIA"))
                    strInserir.Append("N" + "', ");
                else
                    strInserir.Append("S" + "', ");
            }
            strInserir.Append("ST_MAQ = '" + rdblEstMaqui.SelectedItem.Value + "', DS_MAQUINA = '" + txtDesMaqui.Text.ToUpper().Trim() + "', ");
            strInserir.Append("HR_ENTRADA = '" + txtHoraInicio.Text + "', HR_SAIDA = '" + txtHoraFinal.Text + "', ");
            strInserir.Append("HR_DESCONTO = '" + txtHoraDesc.Text + "', HR_TOTAL = '" + lblHoraTotal.Text + "', ");
            strInserir.Append("NM_GUERRA = '" + (DDLCliente.Visible ? DDLCliente.SelectedItem.Text : txtCliente.Text.ToUpper()) + "', ");
            strInserir.Append("DS_OBS = '" + DDLTituloRel.SelectedItem.Text + "(" + DDLVncLocal.SelectedItem.Text + ") - " + txtMemo.Text.ToUpper().Trim() + "' ");
            strInserir.Append("WHERE (NR_LANC = '" + strLancRel + "')");

            FbCommand cmdIncluir = (FbCommand)HlpBancoDados.CommandSelect(strInserir.ToString());
            cmdIncluir.Connection.Open();

            int iIncluiSucesso = cmdIncluir.ExecuteNonQuery();

            cmdIncluir.Connection.Close();

            if (iIncluiSucesso > 0)
            {
                foreach (ListItem Item in LtbOs.Items)
                {

                    FbCommand cmdSPItLancRel = (FbCommand)HlpBancoDados.CommandProcedure("SP_INCLUI_ITLANCREL");

                    cmdSPItLancRel.Connection.Open();

                    strItLancRel = cmdSPItLancRel.ExecuteScalar().ToString();

                    cmdSPItLancRel.Connection.Close();


                    if (!strItLancRel.Equals(String.Empty))
                    {
                        StringBuilder strItLanc = new StringBuilder();
                        strItLanc.Append("UPDATE ITLANCREL SET CD_OS = '" + Item.Text + "', ");
                        strItLanc.Append("NR_LANC = '" + strLancRel + "' ");
                        strItLanc.Append("WHERE (NR_ITLANCREL = '" + strItLancRel + "')");

                        FbCommand cmdInserirOs = (FbCommand)HlpBancoDados.CommandSelect(strItLanc.ToString());

                        cmdInserirOs.Connection.Open();

                        iIncluiSucesso = cmdInserirOs.ExecuteNonQuery();

                        cmdInserirOs.Connection.Close();
                    }
                }
                
                if (iIncluiSucesso > 0)
                    Response.Redirect("~/Confirmacao.aspx?ID=" + strLancRel + ",I");
                else
                    Response.Redirect("~/Confirmacao.aspx?ID=0000000,X");
            }
            else
                Response.Redirect("~/Confirmacao.aspx?ID=0000000,X");

        }
        else
        {
            lblErro.Text = "Todos os campos devem estar preenchidos!";
           
        }
    }

    protected bool bCadastroValido()
    {
        if (txtMemo.Text.Equals(String.Empty) || txtContato.Text.Equals(String.Empty)
            || txtDataCadastro.Text.Equals(String.Empty) || txtHoraFinal.Text.Equals("00:00") || txtDesMaqui.Text.Equals(String.Empty))
            return false;
        else
        {
            return true;
        }

            
    }
    protected bool ValidaCalculoHora()
    {
        DateTime dtInicio = Convert.ToDateTime(txtHoraInicio.Text);
        DateTime dtFim = Convert.ToDateTime(txtHoraFinal.Text);
        if (dtFim <= dtInicio)
            return false;
        else
            return true;
    }

    protected void DDLTipoRelatoio_TextChanged(object sender, EventArgs e)
    {
        DDLTituloRel.Items.Clear();
        if (DDLTipoRelatoio.SelectedValue.Equals("SOFTWARE"))
        {
            DDLTituloRel.Items.Add("TREINAMENTO");
            DDLTituloRel.Items.Add("ASSESSORIA");
            DDLTituloRel.Items.Add("INSTALACAO");
            DDLTituloRel.Items.Add("ATUALIZACAO");
            DDLTituloRel.Items.Add("OUTROS");
        }
        else
        {
            DDLTituloRel.Items.Add("MANUTENCAO");
            DDLTituloRel.Items.Add("ORCAMENTO");
            DDLTituloRel.Items.Add("GARANTIA");
            DDLTituloRel.Items.Add("OUTROS");
        }

        DDLTituloRel.DataBind();
    }
    protected void btnIncluiLista_Click(object sender, EventArgs e)
    {
        if (!txtOs.Text.Equals(String.Empty))
        {
            if (GetOrdemServico(HlpBancoDados.GetCodigo(txtOs.Text)))
            {
                ListItem ItemSelect = new ListItem(HlpBancoDados.GetCodigo(txtOs.Text));

                if (LtbOs.Items.IndexOf(ItemSelect) < 0)
                {
                    lblErro.Text = String.Empty;
                    lblErro.Visible = false;
                    LtbOs.Items.Add(HlpBancoDados.GetCodigo(txtOs.Text));
                    txtOs.Text = String.Empty;
                }
                else
                {
                    lblErro.Visible = true;
                    lblErro.Text = "Verifique se a Ordem de Serviço já existe na lista!";
                }
            }
            else
            {
                lblErro.Visible = true;
                lblErro.Text = "Ordem de Serviço não foi encontrada!";
            }
        }
        txtOs.Focus();

    }
    
    protected void Button1_Click1(object sender, EventArgs e)
    {
        if (!LtbOs.Items.Equals(String.Empty))
        {
            if (LtbOs.SelectedIndex >= 0)
            {
                ListItem ItemExcluir = new ListItem(LtbOs.SelectedItem.Text);
                LtbOs.Items.Remove(ItemExcluir);
            }
        }
        txtOs.Focus();
    }

    protected bool GetOrdemServico(string sCD_OS)
    {
        StringBuilder strOs = new StringBuilder();
        
        bool bRet = false;

        strOs.Append("SELECT ORDEMSER.CD_OS, ORDEMSER.NM_GUERRA ");
        strOs.Append("FROM ORDEMSER ");
        strOs.Append("WHERE CD_OS = '" + sCD_OS + "' ");
        strOs.Append("ORDER BY ORDEMSER.CD_OS");

        FbCommand fbSelect = (FbCommand)HlpBancoDados.CommandSelect(strOs.ToString());

        fbSelect.Connection.Open();

        FbDataReader drResultado = fbSelect.ExecuteReader();
        
        if (drResultado.Read())
            bRet = true;
        else
            bRet = false;

        
        drResultado.Close();

        fbSelect.Connection.Close();

        return bRet;
     }
    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RadioButtonList1.SelectedItem.Text.Equals("SIM"))
        {
            txtCliente.Visible = false;
            DDLCliente.Visible = true;
            DDLSistema.Items.Clear();
        }
        else
        {
            txtCliente.Visible = true;
            DDLCliente.Visible = false;
            
            DDLSistema.Items.Clear();

            DataTable Sistema = GetSistema();
            DDLSistema.DataSource = Sistema;
            DDLSistema.DataMember = "Sistema";
            DDLSistema.DataTextField = "Nome";
            DDLSistema.DataValueField = "Id";
            DDLSistema.DataBind();
        }
    }
    protected void txtOs_TextChanged(object sender, EventArgs e)
    {
        btnIncluiLista_Click(sender, e);
    }
    protected void txtHoraFinal_TextChanged(object sender, EventArgs e)
    {
        txtHoraFinal.Text = GetFormatoHora(txtHoraFinal.Text);
        Button1_Click(sender, e);
    }
    protected void txtHoraDesc_TextChanged(object sender, EventArgs e)
    {
        txtHoraDesc.Text = GetFormatoHora(txtHoraDesc.Text);
        Button1_Click(sender, e);
    }

    protected string GetFormatoHora(string sCampo)
    {
        string sRetTempo = String.Empty;
        sCampo = sCampo.Replace(":", "");
        if (sCampo.Length < 5)
            sCampo = "0000".Substring(0, 4 - sCampo.Length) + sCampo;
            
        sRetTempo = sCampo.Substring(0,2) + ":" + sCampo.Substring(2,2);

        return sRetTempo;
    }
    protected void txtHoraInicio_TextChanged(object sender, EventArgs e)
    {
        txtHoraInicio.Text = GetFormatoHora(txtHoraInicio.Text);
    }
    
    protected void DDLCliente_TextChanged(object sender, EventArgs e)
    {
        DDLSistema.Items.Clear();

        DataTable Sistema = GetSistema();
        DDLSistema.DataSource = Sistema;
        DDLSistema.DataMember = "Sistema";
        DDLSistema.DataTextField = "Nome";
        DDLSistema.DataValueField = "Id";
        DDLSistema.DataBind();
    }
}
