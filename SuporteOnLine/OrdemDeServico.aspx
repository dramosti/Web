<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="OrdemDeServico.aspx.cs" Inherits="OrdemDeServico" Title="Ordem de Serviço"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    &nbsp;
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <table align="center" style="width: 100%">
                <tr>
                    <td align="center" colspan="3">
                        <asp:Label ID="Label16" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="Large"
                            ForeColor="#0066CC" Text="Ordem de Serviço"></asp:Label>
                    </td>
                </tr>
                <tr style="font-size: 12pt; font-family: Times New Roman">
                    <td align="center" colspan="3">
                        <asp:Label ID="Label15" runat="server" Font-Names="Verdana" Font-Size="10px"></asp:Label>
                    </td>
                </tr>
                <tr style="font-size: 12pt; font-family: Times New Roman">
                    <td align="center" colspan="3">
                        &nbsp;</td>
                </tr>
                <tr style="font-size: 12pt; font-family: Times New Roman">
                    <td align="right" style="width: 35%">
                        <asp:Label ID="Label4" runat="server" Font-Names="Verdana" Font-Size="X-Small" Text="Cliente"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="DDLCliente" runat="server" AutoPostBack="True" DataTextField="Nome"
                            DataValueField="Id" Font-Names="Verdana" Font-Size="10px" OnTextChanged="DDLCliente_TextChanged"
                            Width="280px">
                        </asp:DropDownList>&nbsp;</td>
                </tr>
                <tr style="font-size: 12pt; font-family: Times New Roman">
                    <td align="right" style="width: 35%">
                        <asp:Label ID="Label6" runat="server" Font-Names="Verdana" Font-Size="X-Small" Text="Sistema"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="DDLSistema" runat="server" Font-Names="Verdana" Font-Size="10px"
                            Width="280px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr style="font-size: 12pt; font-family: Times New Roman">
                    <td align="right" style="width: 35%">
                        <asp:Label ID="Label1" runat="server" Font-Names="Verdana" Font-Size="X-Small" Text="Tipo da OS"></asp:Label>&nbsp;</td>
                    <td colspan="2">
                        <asp:DropDownList ID="DDLTipoOs" runat="server" AutoPostBack="True" Font-Names="Verdana"
                            Font-Size="10px" Width="170px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr style="font-size: 12pt; font-family: Times New Roman">
                    <td align="right" style="width: 35%">
                    </td>
                    <td colspan="2">
                    </td>
                </tr>
                <tr style="font-size: 12pt; font-family: Times New Roman">
                    <td align="left" colspan="3">
                        &nbsp; &nbsp; &nbsp;
                        <asp:Label ID="Label13" runat="server" Font-Names="Verdana" Font-Size="X-Small" Text="Descrição da Ordem de Serviço"></asp:Label>
                    </td>
                </tr>
                <tr style="font-size: 12pt; font-family: Times New Roman">
                    <td align="left" colspan="3">
                        &nbsp; &nbsp; &nbsp;
                        <asp:TextBox ID="txtMemo" runat="server" Font-Names="Verdana" Font-Size="10px" Height="185px"
                            TextMode="MultiLine" Width="484px"></asp:TextBox>
                    </td>
                </tr>
                <tr style="font-size: 12pt; font-family: Times New Roman">
                    <td align="right" style="width: 35%">
                        &nbsp;</td>
                    <td colspan="2">
                        &nbsp;</td>
                </tr>
                <tr style="font-size: 12pt; font-family: Times New Roman">
                    <td align="right" style="width: 35%">
                        &nbsp;</td>
                    <td colspan="2">
                        &nbsp;</td>
                </tr>
                <tr style="font-size: 12pt; font-family: Times New Roman">
                    <td align="center" colspan="3">
                        <asp:Label ID="lblErro" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small"
                            ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr style="font-size: 12pt; font-family: Times New Roman">
                    <td align="right" style="width: 35%">
                        &nbsp;</td>
                    <td colspan="2">
                        &nbsp;</td>
                </tr>
                <tr style="font-size: 12pt; font-family: Times New Roman">
                    <td align="center" colspan="3">
                        <asp:Button ID="btnGravar" runat="server" Font-Names="Verdana" Font-Size="10px" OnClick="btnGravar_Click"
                            Text="Gravar" Width="90px" />
                        <asp:Button ID="btnVoltar" runat="server" Font-Names="Verdana" Font-Size="10px" Text="Voltar"
                            Width="90px" />
                    </td>
                </tr>
                <tr style="font-size: 12pt; font-family: Times New Roman">
                    <td align="center" colspan="3">
                        &nbsp;</td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <table cellpadding="3" style="width: 100%">
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
                        &nbsp;</td>
                    <td style="width: 100px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 100px">
                    </td>
                    <td align="center">
                        &nbsp;<asp:Label ID="lblTitulo" runat="server" Font-Bold="True" Font-Names="Verdana"
                            Font-Size="Small" Font-Underline="True" ForeColor="#0066CC"></asp:Label></td>
                    <td style="width: 100px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px">
                    </td>
                    <td align="center">
                        <asp:Label ID="lblLancamento" runat="server" Font-Bold="True" Font-Names="Verdana"
                            Font-Size="10px" Text="Label"></asp:Label></td>
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
                                <td align="left" style="width: 100px; height: 24px">
                                </td>
                                <td align="center" style="width: 100px; height: 24px">
                                </td>
                                <td align="right" style="width: 100px; height: 24px">
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 100px; height: 45px">
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>

