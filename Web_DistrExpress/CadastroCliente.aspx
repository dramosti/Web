<%@ Page Title="Cadastro de Cliente" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeFile="CadastroCliente.aspx.cs"
    Inherits="CadastroCliente" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="HLP.Web.Controles" Namespace="HLP.Web.Controles" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .style1
        {
            height: 26px;
            width: 269px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div align="center" style="color: Black">
        <cc1:HlpWebPanel ID="HlpWebPanel2" runat="server" Width="100%">
            <br />
            <table width="100%" >
                <tr class="BordaInferior">
                    <td style="text-align: left; color: Black" class="Titulo" colspan="3">
                    Cliente  &nbsp;<cc1:HlpWebLabel ID="lblCdClifor" runat="server" CssClass="label" ForeColor="Black">[Em inclusão]</cc1:HlpWebLabel>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="label">
                        Nome de Fantasia
                    </td>
                    <td style="text-align: left; width: 70%; height: 23px;">
                        <cc1:HlpWebTextBox ID="txtNmGuerra" runat="server" Campo="NM_GUERRA" MaxLength="15"
                            Tabela="CLIFOR" CssClass="textBox"></cc1:HlpWebTextBox>
                        <cc1:HlpWebLabel ID="lblNmGuerra" runat="server" Font-Bold="True" Font-Size="10pt"
                            ForeColor="Red" Visible="False" Width="1px">*</cc1:HlpWebLabel>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="label">
                        Razão Social
                    </td>
                    <td style="text-align: left; width: 70%; height: 14px;">
                        <cc1:HlpWebTextBox ID="txtNmClifor" runat="server" Campo="NM_CLIFOR" MaxLength="50"
                            Tabela="CLIFOR" Width="304px" CssClass="textBox"></cc1:HlpWebTextBox>
                        <cc1:HlpWebLabel ID="lblNmClifor" runat="server" Font-Bold="True" Font-Size="10pt"
                            ForeColor="Red" Visible="False" Width="1px">*</cc1:HlpWebLabel>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="label">
                        Pessoa Jurídica?
                    </td>
                    <td style="text-align: left; width: 70%; height: 16px;">
                        <cc1:HlpWebDropDownList ID="cbxStPessoaJ" runat="server" Campo="ST_PESSOAJ" Tabela="CLIFOR"
                            DataTextField="DS_DESCVALOR" DataValueField="DS_VALOR" ExpressaoDados="SELECT DS_DESCVALOR, DS_VALOR FROM HLPSTATUS WHERE DS_REFERENCIA = 'SIMNAO' ORDER BY DS_VALOR"
                            AutoPostBack="True" OnSelectedIndexChanged="cbxStPessoaJ_SelectedIndexChanged"
                            Width="98px" CssClass="textBox">
                        </cc1:HlpWebDropDownList>
                    </td>
                </tr>
            </table>
            <cc1:HlpWebMultiView ID="MultiViewClientes" runat="server" ActiveViewIndex="1">
                <cc1:HlpWebView ID="ViewPessoaJuridica" runat="server">
                    <table width="100%" style="font-size: 8pt; font-family: Segoe UI">
                        <tr>
                            <td align="right" class="label">
                                CNPJ
                            </td>
                            <td style="text-align: left; width: 70%">
                                <cc1:HlpWebTextBox ID="txtCNPJ" runat="server" Campo="CD_CGC" MaxLength="18" Tabela="CLIFOR"
                                    Width="157px" MascaraValidacao="\d{2}.?\d{3}.?\d{3}/?\d{4}-?\d{2}" CssClass="textBox"></cc1:HlpWebTextBox>
                                <asp:MaskedEditExtender ID="MaskedEditExtender4" TargetControlID="txtCNPJ" Mask="99,999,999/9999-99"
                                    ClearMaskOnLostFocus="false" runat="server">
                                </asp:MaskedEditExtender>
                                <cc1:HlpWebLabel ID="lblCdCNPJ" runat="server" Font-Bold="True" Font-Size="10pt"
                                    ForeColor="Red" Visible="False" Width="1px">*</cc1:HlpWebLabel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="label">
                                Inscrição Estadual
                            </td>
                            <td style="text-align: left; width: 70%; height: 26px;">
                                <cc1:HlpWebTextBox ID="txtCdInsest" runat="server" Campo="CD_INSEST" Tabela="CLIFOR"
                                    Width="156px" MaxLength="18" CssClass="textBox"></cc1:HlpWebTextBox>
                            </td>
                        </tr>
                    </table>
                </cc1:HlpWebView>
                <cc1:HlpWebView ID="ViewPessoaFisica" runat="server">
                    <table width="100%" style="font-size: 8pt; font-family: Segoe UI">
                        <tr>
                            <td align="right" class="label">
                                CPF
                            </td>
                            <td style="text-align: left; width: 70%">
                                <cc1:HlpWebTextBox ID="txtCdCpf" runat="server" Campo="CD_CPF" Tabela="CLIFOR" Width="156px"
                                    MascaraValidacao="^\d{3}\.\d{3}\.\d{3}\-\d{2}$" MaxLength="14" CssClass="textBox"></cc1:HlpWebTextBox>
                                <asp:MaskedEditExtender ID="MaskedEditExtender3" TargetControlID="txtCdCpf" Mask="999,999,999-99"
                                    ClearMaskOnLostFocus="false" runat="server">
                                </asp:MaskedEditExtender>
                                <cc1:HlpWebLabel ID="lblCdCpf" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Red"
                                    Visible="False" Width="1px">*</cc1:HlpWebLabel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="label">
                                RG
                            </td>
                            <td style="text-align: left; width: 70%">
                                <cc1:HlpWebTextBox ID="txtCdRg" runat="server" Campo="CD_RG" Tabela="CLIFOR" Width="155px"
                                    MaxLength="14" CssClass="textBox"></cc1:HlpWebTextBox>
                                <cc1:HlpWebLabel ID="lblCdRg" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Red"
                                    Visible="False" Width="1px">*</cc1:HlpWebLabel>
                            </td>
                        </tr>
                    </table>
                </cc1:HlpWebView>
            </cc1:HlpWebMultiView>
            <table width="100%" style="font-size: 8pt; font-family: Segoe UI">
                <tr>
                    <td align="right" class="label">
                        Contato
                    </td>
                    <td style="text-align: left; width: 70%; height: 16px;">
                        <cc1:HlpWebTextBox ID="txtDsContato" runat="server" Campo="DS_CONTATO" MaxLength="30"
                            Tabela="CLIFOR" Width="257px" CssClass="textBox"></cc1:HlpWebTextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="label">
                        CEP
                    </td>
                    <td style="text-align: left; width: 70%; height: 16px;">
                        <cc1:HlpWebTextBox ID="txtCdCepnor" runat="server" Campo="CD_CEPNOR" MascaraValidacao="\d{5}\-\d{3}"
                            MaxLength="9" Tabela="CLIFOR" Width="91px" CssClass="textBox"></cc1:HlpWebTextBox>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtCdCepnor" Mask="99999-999"
                            ClearMaskOnLostFocus="false" runat="server">
                        </asp:MaskedEditExtender>
                        <cc1:HlpWebLabel ID="lblCdCepnor" runat="server" Font-Bold="True" Font-Size="10pt"
                            ForeColor="Red" Visible="False" Width="1px">*</cc1:HlpWebLabel>
                        <asp:Button ID="btnBuscaCep" runat="server" Text="Consultar" CssClass="button" OnClick="btnBuscaCep_Click" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="label">
                        Endereço
                    </td>
                    <td style="text-align: left; width: 70%; height: 18px;">
                        <cc1:HlpWebTextBox ID="txtDsEndnor" runat="server" Campo="DS_ENDNOR" MaxLength="50"
                            Tabela="CLIFOR" Width="378px" CssClass="textBox"></cc1:HlpWebTextBox>
                        <cc1:HlpWebLabel ID="lblDsEndnor" runat="server" Font-Bold="True" Font-Size="10pt"
                            ForeColor="Red" Visible="False" Width="1px">*</cc1:HlpWebLabel>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="label">
                        Número
                    </td>
                    <td style="text-align: left; width: 70%; height: 18px;">
                        <cc1:HlpWebTextBox ID="txtNumero" runat="server" Campo="NR_ENDNOR" MaxLength="15"
                            Tabela="CLIFOR" Width="160px" CssClass="textBox"></cc1:HlpWebTextBox>
                        <cc1:HlpWebLabel ID="lblNumero" runat="server" Font-Bold="True" Font-Size="10pt"
                            ForeColor="Red" Visible="False" Width="1px">*</cc1:HlpWebLabel>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="label">
                        Bairro
                    </td>
                    <td style="text-align: left; width: 70%; height: 18px;">
                        <cc1:HlpWebTextBox ID="txtNM_BAIRRONOR" runat="server" Campo="NM_BAIRRONOR" MaxLength="20"
                            Tabela="CLIFOR" Width="250px" CssClass="textBox"></cc1:HlpWebTextBox>
                        <cc1:HlpWebLabel ID="lblNM_BAIRRONOR" runat="server" Font-Bold="True" Font-Size="10pt"
                            ForeColor="Red" Visible="False" Width="1px">*</cc1:HlpWebLabel>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="label">
                        Estado
                    </td>
                    <td style="text-align: left; width: 70%">
                        <cc1:HlpWebDropDownList ID="cbxCdUfnor" runat="server" Campo="CD_UFNOR" DataTextField="DS_UF"
                            DataValueField="CD_UF" ExpressaoDados="SELECT DS_UF, CD_UF FROM UF ORDER BY DS_UF"
                            Tabela="CLIFOR" Width="260px" CssClass="textBox" OnSelectedIndexChanged="cbxCdUfnor_SelectedIndexChanged"
                            AutoPostBack="True">
                        </cc1:HlpWebDropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="label">
                        Cidade
                    </td>
                    <td style="text-align: left; width: 70%">
                        <cc1:HlpWebDropDownList ID="cbxCidades" runat="server" Campo="NM_CIDNOR" DataTextField="NM_CIDNOR"
                            DataValueField="NM_CIDNOR" ExpressaoDados="select CD_MUNICIPIO, NM_CIDNOR from cidades"
                            CssClass="textBox" Tabela="CLIFOR" Width="260px">
                        </cc1:HlpWebDropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="label">
                        Caixa Postal
                    </td>
                    <td style="text-align: left; width: 70%">
                        <cc1:HlpWebTextBox ID="txtCdCxpnor" runat="server" Campo="CD_CXPNOR" MaxLength="7"
                            Tabela="CLIFOR" Width="70px" CssClass="textBox"></cc1:HlpWebTextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="label">
                        Fone
                    </td>
                    <td style="text-align: left; width: 70%">
                        <cc1:HlpWebTextBox ID="txtCdFonenor" runat="server" Campo="CD_FONENOR" MaxLength="25"
                            Tabela="CLIFOR" CssClass="textBox"></cc1:HlpWebTextBox>
                        <cc1:HlpWebLabel ID="lblCdFonenor" runat="server" Font-Bold="True" Font-Size="10pt"
                            ForeColor="Red" Visible="False" Width="1px">*</cc1:HlpWebLabel>
                        <asp:MaskedEditExtender ID="maskFone1" TargetControlID="txtCdFonenor" Mask="(999)9999-9999"
                            ClearMaskOnLostFocus="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="label">
                        Fax
                    </td>
                    <td style="text-align: left; width: 70%">
                        <cc1:HlpWebTextBox ID="txtCdFaxnor" runat="server" Campo="CD_FAXNOR" MaxLength="25"
                            Tabela="CLIFOR" CssClass="textBox"></cc1:HlpWebTextBox>
                        <asp:MaskedEditExtender ID="mskFone2" TargetControlID="txtCdFaxnor" Mask="(999)9999-9999"
                            ClearMaskOnLostFocus="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="label">
                        E-mail
                    </td>
                    <td style="text-align: left; width: 70%">
                        <cc1:HlpWebTextBox ID="txtCdEmail" runat="server" Campo="CD_EMAIL" MascaraValidacao="\S+@\S+.\S{2,3}"
                            MaxLength="35" Tabela="CLIFOR" Width="244px" RetornaEmMaiusculo="False" CssClass="textBox"></cc1:HlpWebTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 30%">
                        &nbsp;
                    </td>
                    <td style="text-align: left; width: 70%">
                        <cc1:HlpWebTextBox ID="txtCdBanco" Visible="false" runat="server" Campo="CD_CTABANC"
                            CssClass="textBox" MascaraValidacao="\S+@\S+.\S{2,3}" MaxLength="35" RetornaEmMaiusculo="False"
                            Tabela="CLIFOR" Width="100px"></cc1:HlpWebTextBox>
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td class="style1">
                    </td>
                    <td style="width: 176px; height: 26px;">
                        <cc1:HlpWebButton ID="btnConfirmar" runat="server" OnClick="btnConfirmar_Click" Text="Confirmar"
                            CssClass="button" />
                    </td>
                    <td style="width: 263px; height: 26px;">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
                <tr>
                    <td style="width: 33px">
                    </td>
                    <td style="text-align: center">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </cc1:HlpWebPanel>
    </div>
</asp:Content>
