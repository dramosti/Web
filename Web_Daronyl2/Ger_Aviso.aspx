<%@ Page Title="" Language="C#" MasterPageFile="~/Ger_Site.master" AutoEventWireup="true"
    CodeFile="Ger_Aviso.aspx.cs" Inherits="Ger_Aviso" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <script type="text/javascript">
        $('#<%=txtAviso.ClientID%>').live("blur", function (e) {
            var Text = $(this).val();
            if (Text.length > 1000) {
                $('#<%=txtAviso.ClientID%>').val(Text.substring(0, 1000));
            }

        });
    </script>
    <script type="text/javascript">
        $('#<%=txtAviso.ClientID%>').live("keypress", function (e) {
            var MaxLength = 1000;
            if ($(this).val().length >= MaxLength) {
                e.preventDefault();
            }
        });
    </script>
    <script type="text/javascript">
        $('#<%=txtDataFinal.ClientID%>').live("blur", function (e) {

            var Data = $('#<%=txtDataFinal.ClientID%>').val();
            if (Data != "") {
                var valido = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/.test(Data);

                if (valido) {
                    return true;
                }
                else {
                    $('#<%=txtDataFinal.ClientID%>').val("");
                    return false;
                }
            }
        });
    </script>
    <table width="100%">
        <tr>
            <td>
            </td>
        </tr>
        <tr class="BordaInferior">
            <td style="text-align: left; color: Black" class="Titulo" colspan="2">
                Cadastro de Aviso
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td align="right" class="label">
                Tipo de Aviso
            </td>
            <td align="left">
                <asp:DropDownList ID="cboTipo" runat="server" AutoPostBack="True" CssClass="textBox"
                    Enabled="true" Width="122px" OnSelectedIndexChanged="cboTipo_SelectedIndexChanged">
                    <asp:ListItem Value="G">Geral</asp:ListItem>
                    <asp:ListItem Value="R">Representante</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right" class="label">
                Representante
            </td>
            <td align="left">
                <asp:DropDownList ID="cboRepresentante" runat="server" AutoPostBack="True" CssClass="textBox"
                    DataTextField="nm_vend" DataValueField="cd_vend" Enabled="False" Width="387px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right" class="label">
                Título
            </td>
            <td align="left">
                <asp:TextBox runat="server" CssClass="textBox" ID="txtTitulo" Width="200px" MaxLength="20"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Font-Bold="True"
                    Font-Size="13px" ForeColor="Red" ErrorMessage="*" ControlToValidate="txtTitulo"
                    ValidationGroup="Confirmar"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="right">
                Aviso
            </td>
            <td align="left">
                <asp:TextBox runat="server" CssClass="textBox" ID="txtAviso" Width="400px" TextMode="MultiLine"
                    MaxLength="1000" Height="100px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Font-Bold="True"
                    Font-Size="13px" ForeColor="Red" ErrorMessage="*" ControlToValidate="txtAviso"
                    ValidationGroup="Confirmar"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="right" class="label">
                Data Final do Aviso
            </td>
            <td align="left">
                <asp:TextBox runat="server" CssClass="textBox" ID="txtDataFinal" Width="100px" MaxLength="10"></asp:TextBox><asp:CalendarExtender
                    ID="CalendarExtender1" runat="server" TargetControlID="txtDataFinal" Format="dd/MM/yyyy"
                    TodaysDateFormat="dd/MM/yyyy">
                </asp:CalendarExtender>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Font-Bold="True"
                    Font-Size="13px" ForeColor="Red" ErrorMessage="*" ControlToValidate="txtDataFinal"
                    ValidationGroup="Confirmar"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Button runat="server" ID="btnConfirmar" CssClass="button" Text="Confirmar" OnClick="btnConfirmar_Click"
                    ValidationGroup="Confirmar" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td align="left">
            </td>
        </tr>
    </table>
</asp:Content>
