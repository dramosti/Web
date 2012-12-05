<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Confirmacao.aspx.cs" Inherits="Confirmacao" Title="Informativo"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="width: 100%" cellpadding="3">
        <tr>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td style="width: 100px">
                &nbsp;</td>
            <td align="center">
                <asp:Label ID="lblTitulo" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="Small"
                    Font-Underline="True" ForeColor="#0066CC"></asp:Label>
            </td>
            <td style="width: 100px">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width: 100px">
            </td>
            <td>
                &nbsp;</td>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td style="width: 100px">
            </td>
            <td align="center">
                <asp:Label ID="lblLancamento" runat="server" Font-Bold="True" 
                    Font-Names="Verdana" Font-Size="10px"
                    Text="Label"></asp:Label></td>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td style="width: 100px; height: 45px">
            </td>
            <td align="center" style="width: 100px; height: 45px">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 522px">
                    <tr>
                        <td style="width: 100px; height: 24px" align="left">
                            <asp:Button ID="btnInicial" runat="server" Font-Names="Verdana" Font-Size="10px" Text="Tela Inicial"
                                Width="147px" OnClick="btnInicial_Click" /></td>
                        <td style="width: 100px; height: 24px" align="center">
                            <asp:Button ID="btnIncluir" runat="server" Font-Names="Verdana" Font-Size="10px" Text="Incluir Novo Cadastro"
                                Width="147px" OnClick="btnIncluir_Click" /></td>
                        <td align="right" style="width: 100px; height: 24px">
                            <asp:Button ID="btnImp" runat="server" Font-Names="Verdana" Font-Size="10px" Text="Imprimir"
                                Width="147px" OnClick="btnImp_Click" /></td>
                    </tr>
                </table>
            </td>
            <td style="width: 100px; height: 45px">
            </td>
        </tr>
    </table>
</asp:Content>

