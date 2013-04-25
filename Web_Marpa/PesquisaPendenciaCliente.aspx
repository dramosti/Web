<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="PesquisaPendenciaCliente.aspx.cs" Inherits="PesquisaPendenciaCliente" %>

<%@ Register Assembly="HLP.Web.Controles" Namespace="HLP.Web.Controles" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 177px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <table style="width: 100%">
        <tr class="BordaInferior">
            <td style="text-align: left; color: Black" class="Titulo" colspan="3">
                Cliente
            </td>
        </tr>
        <tr>
            <td align="right" class="style1">
                <cc1:HlpWebRadioButton ID="rdbTodos" runat="server" AutoPostBack="True" CssClass="label"
                    Text="Todos" GroupName="Opcoes" OnCheckedChanged="rdbTodos_CheckedChanged" />
            </td>
            <td>
            </td>
        </tr>
        <%--  <tr>
            <td align="right" class="style1">
                <cc1:HlpWebRadioButton ID="rdbCodigo" runat="server" AutoPostBack="True" GroupName="Opcoes"
                    Checked="True" Text="Código Alternativo" CssClass="label" OnCheckedChanged="rdbCodigo_CheckedChanged" />
            </td>
            <td align="left">
                <cc1:HlpWebTextBox ID="txtCodigo" runat="server" MaxLength="20" Width="293px" CssClass="textBox"></cc1:HlpWebTextBox>
            </td>
        </tr>--%>
        <tr>
            <td align="right" class="style1">
                <cc1:HlpWebRadioButton ID="rdbCodigo" runat="server" AutoPostBack="True"
                    GroupName="Opcoes" Checked="True" Text="Código Alternativo" CssClass="label"
                    OnCheckedChanged="rdbCodigo_CheckedChanged" />
            </td>
            <td align="left">
                <%--<asp:TextBox ID="txtCodigo" runat="server" CssClass="textBox" Enabled="True" Width="101px"></asp:TextBox>--%>
                <cc1:HlpWebTextBox ID="txtCodigo" runat="server" MaxLength="20" Width="101px" CssClass="textBox"></cc1:HlpWebTextBox>
                <asp:TextBox ID="txtCliente" runat="server" CssClass="textBox" Enabled="False" Width="286px"></asp:TextBox>
                &nbsp;&nbsp;
                <asp:Button ID="btnCliente" runat="server" CssClass="button" Height="26px" Text="Buscar Cliente"
                    OnClick="btnCliente_Click" />
                &nbsp;&nbsp;
                <asp:Button ID="btnPesqCliente" runat="server" CssClass="button" Height="26px" OnClick="btnPesqCliente_Click"
                    Text="Filtrar Pesquisar" />
            </td>
        </tr>
        <tr>
            <td align="right" class="style1">
                <cc1:HlpWebRadioButton ID="rdbGuerra" runat="server" AutoPostBack="True" GroupName="Opcoes"
                    Text="Razão Social " CssClass="label" OnCheckedChanged="rdbGuerra_CheckedChanged" />
            </td>
            <td align="left">
                <cc1:HlpWebTextBox ID="txtNomeCliente" runat="server" MaxLength="20" Width="293px"
                    Enabled="False" CssClass="textBox"></cc1:HlpWebTextBox>
            </td>
        </tr>
        <caption>
            <tr>
                <td colspan="2" style="height: 39px; text-align: center">
                    <cc1:HlpWebButton ID="btnPesquisar" runat="server" CssClass="button" Text="Pesquisar"
                        Width="124px" OnClick="btnPesquisar_Click" />
                    &nbsp; &nbsp; &nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp;
                </td>
            </tr>
        </caption>
    </table>
</asp:Content>
