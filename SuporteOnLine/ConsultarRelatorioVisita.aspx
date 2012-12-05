<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ConsultarRelatorioVisita.aspx.cs" Inherits="ConsultarRelatorioVisita" Title="Consulta de Visitas" %>
<asp:Content ID="Content1" runat="server" 
    contentplaceholderid="ContentPlaceHolder1">
    <p>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
            EnableScriptGlobalization="True" EnableScriptLocalization="True">
        </ajaxToolkit:ToolkitScriptManager>
    </p>
    <p>
    <table align="center" cellpadding="0" cellspacing="0" style="width: 95%">
        <tr>
            <td colspan="4">
                &nbsp;</td>
        </tr>
        <tr>
            <td align="center" colspan="4">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="Large"
                    Text="Consulta Relatório de Visita" ForeColor="#0066CC"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width: 193px">
                <asp:Label ID="Label2" runat="server" Font-Names="Verdana" Font-Size="10px" 
                    Text="Pesquisar por código do relatório"></asp:Label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="TextBox1" runat="server" Font-Names="Verdana" Font-Size="10px" Width="117px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 193px">
                <asp:Label ID="Label3" runat="server" Font-Names="Verdana" Font-Size="10px" 
                    Text="Pesquisar por nome do cliente"></asp:Label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="TextBox2" runat="server" Font-Names="Verdana" Font-Size="10px" 
                    Width="117px" TabIndex="1"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 193px">
                <asp:Label ID="Label8" runat="server" Font-Names="Verdana" Font-Size="10px" 
                    Text="Pesquisar por Código Digitado"></asp:Label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="TextBox6" runat="server" Font-Names="Verdana" Font-Size="10px" 
                    Width="117px" TabIndex="2"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 193px">
                <asp:Label ID="Label5" runat="server" Font-Names="Verdana" Font-Size="10px" 
                    Text="Pesquisar por data do relatório" 
                    style="margin-right: 0px; margin-bottom: 0px"></asp:Label>
            </td>
            <td style="width: 113px" valign="middle">
                <asp:TextBox ID="TextBox4" runat="server" Font-Names="Verdana" Font-Size="10px" 
                    Width="79px" TabIndex="3"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" 
                    ImageUrl="~/Imagem/BtnCalendario.jpg" />
            </td>
            <td align="center" style="width: 36px" valign="middle">
                <asp:Label ID="Label6" runat="server" Font-Names="Verdana" Font-Size="10px" 
                    Text="até"></asp:Label>
            </td>
            <td valign="middle">
                <asp:TextBox ID="TextBox5" runat="server" Font-Names="Verdana" Font-Size="10px" 
                    Width="80px" TabIndex="4"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" 
                    ImageUrl="~/Imagem/BtnCalendario.jpg" />
            </td>
        </tr>
        <tr>
            <td style="width: 193px">
                <asp:Label ID="Label4" runat="server" Font-Names="Verdana" Font-Size="10px" 
                    Text="Pesquisar por nome do técnico"></asp:Label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="TextBox3" runat="server" Font-Names="Verdana" Font-Size="10px" 
                    Width="117px" TabIndex="5"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
                <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small"
                    ForeColor="Red" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
                <asp:Button ID="Button1" runat="server" Font-Names="Verdana" Font-Size="10px" 
                    Text="Pesquisar..." OnClick="Button1_Click" TabIndex="6" />
            </td>
        </tr>
        <tr>
            <td style="width: 193px">
                &nbsp;</td>
            <td colspan="3">
                &nbsp;</td>
        </tr>
    </table>
    </p>
    <p>
        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" 
            ClearMaskOnLostFocus="False" Mask="99/99/9999" 
            TargetControlID="TextBox4" UserDateFormat="DayMonthYear">
        </ajaxToolkit:MaskedEditExtender>
        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" 
            PopupButtonID="ImageButton1" TargetControlID="TextBox4" 
            Format="dd/MM/yyyy">
        </ajaxToolkit:CalendarExtender>
        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" 
            ClearMaskOnLostFocus="False" Mask="99/99/9999" 
            TargetControlID="TextBox5" UserDateFormat="DayMonthYear">
        </ajaxToolkit:MaskedEditExtender>
        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" 
            PopupButtonID="ImageButton2" TargetControlID="TextBox5" 
            Format="dd/MM/yyyy">
        </ajaxToolkit:CalendarExtender>
    </p>

</asp:Content>


