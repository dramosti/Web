<%@ Page Title="Pesquisar Cliente" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeFile="PesquisarClientes.aspx.cs"
    Inherits="PesquisarClientes" %>

<%@ Register Assembly="HLP.Web.Controles" Namespace="HLP.Web.Controles" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .style3
        {
        }
        .style4
        {
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">    
    <div align="center">
        <asp:MultiView ID="MultViewPesquisaCliente" runat="server" ActiveViewIndex="0">
            <asp:View ID="Pesquisa" runat="server">
                <table style="width: 100%">
                    <tr class="BordaInferior">
                        <td style="text-align: left; color: Black" class="Titulo" colspan="3">
                            Cliente
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <cc1:HlpWebRadioButton ID="rdbTodos" runat="server" AutoPostBack="True" CssClass="label"
                                Text="Todos" GroupName="Opcoes" OnCheckedChanged="rdbTodos_CheckedChanged" />
                        </td>
                        <td>
                        </td>
                    </tr>
                     <tr>
                        <td align="right">
                            <cc1:HlpWebRadioButton ID="rdbCodigo" runat="server" AutoPostBack="True" GroupName="Opcoes"
                                Text="Código" CssClass="label" OnCheckedChanged="rdbCodigo_CheckedChanged" />
                        </td>
                        <td align="left">
                            <cc1:HlpWebTextBox ID="txtCodigo" runat="server" MaxLength="20" Width="293px" Enabled="False"
                                CssClass="textBox"></cc1:HlpWebTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <cc1:HlpWebRadioButton ID="rdbRazao" runat="server" AutoPostBack="True" GroupName="Opcoes"
                                Text="Razão Social" CssClass="label" OnCheckedChanged="rdbRazao_CheckedChanged"
                                Checked="True" />
                        </td>
                        <td align="left">
                            <cc1:HlpWebTextBox ID="txtRazaoSocial" runat="server" MaxLength="20" Width="293px"
                                CssClass="textBox"></cc1:HlpWebTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <cc1:HlpWebRadioButton ID="rdbGuerra" runat="server" AutoPostBack="True" GroupName="Opcoes"
                                Text=" Nome de Guerra" CssClass="label" OnCheckedChanged="rdbGuerra_CheckedChanged" />
                        </td>
                        <td align="left">
                            <cc1:HlpWebTextBox ID="txtNomeCliente" runat="server" MaxLength="20" Width="293px"
                                Enabled="False" CssClass="textBox"></cc1:HlpWebTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <cc1:HlpWebRadioButton ID="rdbCidade" runat="server" AutoPostBack="True" Text="Cidade"
                                CssClass="label" GroupName="Opcoes" OnCheckedChanged="rdbCidade_CheckedChanged" />
                        </td>
                        <td align="left">
                            <cc1:HlpWebTextBox ID="txtCidade" runat="server" MaxLength="20" Width="293px" Enabled="False"
                                CssClass="textBox"></cc1:HlpWebTextBox>
                        </td>
                    </tr>
                    <caption>
                        <tr>
                            <td colspan="2" style="height: 39px; text-align: center">
                                <cc1:HlpWebButton ID="btnPesquisar" runat="server" CssClass="button" OnClick="btnPesquisar_Click"
                                    Text="Pesquisar" Width="124px" />
                                &nbsp; &nbsp; &nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp;
                                <cc1:HlpWebButton ID="btnVoltar" runat="server" CssClass="button" OnClick="btnVoltar_Click"
                                    Text="Voltar" Width="124px" />
                            </td>
                        </tr>
                    </caption>
                </table>
            </asp:View>
            <asp:View ID="Informacao" runat="server">
                <table style="width: 100%">
                    <tr>
                        <td align="center" class="style3" colspan="2">
                            <asp:Label ID="lblMsg" runat="server" CssClass="label" Text="Label"></asp:Label>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td class="label" align="center" colspan="2">
                           Código:  
                            <asp:Label ID="lblCodigo" runat="server" CssClass="label" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="label" colspan="2">
                            Nome Cliente:
                            <asp:Label ID="lblNmCliente" runat="server" CssClass="label" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style4">
                        </td>
                        <td align="left" class="style6">
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td align="right" class="style10">
                            <cc1:HlpWebButton ID="HlpWebButton1" runat="server" CssClass="button" OnClick="btnPesquisarClientes_Click"
                                Text="Pesquisar Clientes" Width="150px" />
                        </td>
                        <td class="style9">
                        </td>
                        <td class="style8" align="left">
                            <cc1:HlpWebButton ID="HlpWebButton2" runat="server" CssClass="button" OnClick="btnVoltar_Click"
                                Text="Voltar" Width="150px" />
                        </td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
