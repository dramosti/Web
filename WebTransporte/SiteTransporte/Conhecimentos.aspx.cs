using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using DAO;
using Business;

public partial class Conhecimentos : System.Web.UI.Page
{
    Business.belConhecimento objbelConhec = new Business.belConhecimento();
    Business.belNF objbelNF = new Business.belNF();

    protected void Page_Load(object sender, EventArgs e)
    {
        belUsuario objbelUser = (belUsuario)Session["Usuario"];
        if (objbelUser == null)
        {
            Response.RedirectPermanent("~/Account/Login.aspx");
        }
        Page.MaintainScrollPositionOnPostBack = true;
        if (!IsPostBack)
        {
            VisualizaLabel(false);
        }
    }


    protected void cboFiltro_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtFiltro1.Text = "";
        txtFiltro2.Text = "";
        if (cboFiltro.SelectedIndex == 3)
        {
            txtFiltro2.Visible = true;
        }
        else
        {
            txtFiltro2.Visible = false;
        }
    }

    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        try
        {
            lblResul.Visible = false;
            if (ValidaPesquisa())
            {
                
                Session["FiltroPrincipal"] = objbelConhec.GetAll(cboBaixa.SelectedValue.ToString(), cboCampo.SelectedValue.ToString(), cboFiltro.SelectedValue.ToString(), txtFiltro1.Text, txtFiltro2.Text);
                gridConhec.DataSource = Session["FiltroPrincipal"];
                gridConhec.DataBind();

                if ((Session["FiltroPrincipal"] as List<daoConhecimento>).Count > 0 )
                {
                    lblTotConhec.Text = string.Format("Total de Conhecimentos = {0}", (Session["FiltroPrincipal"] as List<daoConhecimento>).Count.ToString());
                }
                else
                {
                    lblTotConhec.Text = "";
                }

                Session["Notas"] = objbelNF.GetAllNF((Session["FiltroPrincipal"] as List<daoConhecimento>));
                gridNotas.DataSource = Session["Notas"];
                gridNotas.DataBind();



                if ((Session["Notas"] as List<daoNF>).Count > 0)
                {
                    lblTotNF.Text = string.Format("Total de Notas Fiscais = {0}", (Session["Notas"] as List<daoNF>).Count.ToString());
                }
                else
                {
                    lblTotNF.Text = "";
                }



                if (gridConhec.Rows.Count == 0)
                {
                    lblResul.Visible = true;
                }
                else
                {
                    lblResul.Visible = false;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Alert", "alert('Padrão de pesquisa inválido! Verifique se o filtro inserido é compatível com o tipo de campo selecionado')", true);
            }
            VisualizaLabel(false);
        }
        catch (TimeoutException t) 
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Alert", "alert('A Pesquisa Realizada excede o tempo limite do banco de dados, tente realizar uma pesquisa para não trazer tantos Registros.')", true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private bool ValidaPesquisa()
    {
        bool ret = true;

        try
        {
            Regex reg;
            switch (cboCampo.SelectedIndex)
            {
                case 0:
                    reg = new Regex("^[0-9]+$");
                    if (!reg.IsMatch(txtFiltro1.Text))
                    {
                        ret = false;
                    }
                    break;

                case 1:
                    reg = new Regex("^[0-9]+$");
                    if (!reg.IsMatch(txtFiltro1.Text))
                    {
                        ret = false;
                    }
                    break;

                case 2:
                    reg = new Regex(@"^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$");
                    if (cboFiltro.SelectedIndex < 3)
                    {
                        if (!reg.IsMatch(txtFiltro1.Text))
                        {
                            ret = false;
                        }
                    }
                    else
                    {
                        if (!reg.IsMatch(txtFiltro1.Text) || !reg.IsMatch(txtFiltro2.Text))
                        {
                            ret = false;
                        }
                    }
                    break;

                default:
                    break;
            }
            return ret;
        }
        catch (Exception ex)
        {
            ret = false;
            throw ex;
        }
    }
    private void VisualizaLabel(bool bVisualiza)
    {
        lbldaoEndereco.Visible = bVisualiza;
        lblEndereco.Visible = bVisualiza;
        lbldaoTexto.Visible = bVisualiza;
        lblTexto.Visible = bVisualiza;
        lbldaoOcorrencia.Visible = bVisualiza;
        lblOcorrencia.Visible = bVisualiza;
        lbldaoColeta.Visible = bVisualiza;
        lbldaoUfColeta.Visible = bVisualiza;
        lblUfColeta.Visible = bVisualiza;
        lblColeta.Visible = bVisualiza;
        lbldaoCfop.Visible = bVisualiza;
        lblCfop.Visible = bVisualiza;
        lbldaoMotorista.Visible = bVisualiza;
        lblMotorista.Visible = bVisualiza;
        lbldaoPlaca.Visible = bVisualiza;
        lblPlaca.Visible = bVisualiza;
        lbldaoModelo.Visible = bVisualiza;
        lblModelo.Visible = bVisualiza;
        lbldaodtCanc.Visible = bVisualiza;
        lbldaoDtBaixa.Visible = bVisualiza;
        lbldtbaixa.Visible = bVisualiza;
        lbldtCanc.Visible = bVisualiza;
    }

    protected void gridNotas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gridNotas.PageIndex = e.NewPageIndex;
            gridNotas.DataSource = Session["Notas"];
            gridNotas.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void gridConhec_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gridConhec.PageIndex = e.NewPageIndex;
            gridConhec.DataSource = Session["FiltroPrincipal"];
            gridConhec.DataBind();
            VisualizaLabel(false);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gridConhec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            daoConhecimento objDaoConhecimento = ((List<daoConhecimento>)Session["FiltroPrincipal"]).FirstOrDefault(c => c.NR_LANC == gridConhec.SelectedValue.ToString());

            lbldaoEndereco.Text = objDaoConhecimento.DadosAdicionais.DS_ENDENTREGA;
            lbldaoOcorrencia.Text = objDaoConhecimento.DadosAdicionais.DS_OCOR;
            lbldaoTexto.Text = objDaoConhecimento.DadosAdicionais.DS_TEXTO;
            lbldaoColeta.Text = objDaoConhecimento.DadosAdicionais.DS_COLETA;
            lbldaoUfColeta.Text = objDaoConhecimento.DadosAdicionais.CD_UFCOLE;
            lbldaoCfop.Text = objDaoConhecimento.DadosAdicionais.CD_CFOP;
            lbldaoMotorista.Text = objDaoConhecimento.Complemento.NM_MOTORIS;
            lbldaoPlaca.Text = objDaoConhecimento.Complemento.CD_PLACA;
            lbldaoModelo.Text = objDaoConhecimento.Complemento.CD_MODELO;
            lbldaodtCanc.Text = objDaoConhecimento.DT_CANCCON;
            lbldaoDtBaixa.Text = objDaoConhecimento.DT_BAIXA;
            VisualizaLabel(true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cboCampo.SelectedIndex == 0)
        {
            cboFiltro.Items.Clear();
            cboFiltro.Items.Add(new ListItem { Text = "Igual a", Value = "=" });
        }
        else
        {
            if (cboFiltro.Items.Count == 1)
            {
                cboFiltro.Items.Clear();
                cboFiltro.Items.Add(new ListItem { Text = "Igual a", Value = "=" });
                cboFiltro.Items.Add(new ListItem { Text = "Maior que", Value = ">" });
                cboFiltro.Items.Add(new ListItem { Text = "Menor que", Value = "<" });
                cboFiltro.Items.Add(new ListItem { Text = "Entre", Value = "between" });
            }
        }
        txtFiltro1.Text = "";
        txtFiltro2.Text = "";
    }

}