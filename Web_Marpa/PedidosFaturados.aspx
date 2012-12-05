<%@ Page Title="Pedido Faturado" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeFile="PedidosFaturados.aspx.cs" Inherits="PedidosFaturados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div align="center" style="color: Black">
        <table style="width: 577px">
            <tr>
                <td align="left" rowspan="3" style="width: 76px" valign="baseline">
                    <table style="width: 245px">
                        <tr>
                            <td align="left" colspan="3">
                                <asp:Label ID="lblPedido" runat="server" CssClass="label" Text="Pedidos:" Width="462px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="3" style="height: 21px">
                                <asp:Label ID="lblCliente" runat="server" CssClass="label" Text="Cliente:" Width="463px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
            <tr>
                <td align="center" valign="top">
                    <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" BackColor="White"
                        Font-Names="Segoe UI" Font-Size="8pt" Font-Bold="true" BorderColor="#CCCCCC"
                        BorderStyle="None" BorderWidth="1px" CellPadding="4" Width="576px" ForeColor="Black"
                        GridLines="Horizontal">
                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                        <SelectedItemStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                        <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                        <Columns>
                            <asp:BoundColumn DataField="DT_EMI" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Data Faturamento">
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False"
                                    Font-Size="10px" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                    Wrap="False" />
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False"
                                    Font-Size="12px" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                    Width="6%" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="NOTAFISCAL" HeaderText="N&#186; Nota Fiscal">
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False"
                                    Font-Size="10px" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"
                                    Wrap="False" />
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False"
                                    BackColor="#3a4f63" Font-Size="12px" Font-Strikeout="False" Font-Underline="False"
                                    HorizontalAlign="Left" Width="5%" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="CD_ALTER" HeaderText="C&#243;d. Alter.">
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False"
                                    Font-Size="10px" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"
                                    Wrap="False" />
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False"
                                    Font-Size="12px" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"
                                    Width="5%" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="DS_PROD" HeaderText="Descri&#231;&#227;o">
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False"
                                    Font-Size="10px" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"
                                    Wrap="False" />
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False"
                                    Font-Size="12px" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"
                                    Width="15%" Wrap="False" />
                            </asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                    <table style="width: 577px">
                        <tr>
                            <td style="width: 10%; height: 64px">
                            </td>
                            <td align="center" style="width: 100px; height: 64px" valign="top">
                                <table style="width: 453px; height: 1%">
                                    <tr>
                                        <td style="width: 100px; height: 30px;">
                                        </td>
                                        <td style="width: 100px; height: 30px;" align="center">
                                            <asp:Button ID="btnVoltar" runat="server" Text="Voltar ao Pedido" Width="142px" CssClass="button"
                                                OnClick="btnVoltar_Click" />
                                        </td>
                                        <td style="width: 100px; height: 30px;">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 10%; height: 64px">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center" valign="top">
                                <asp:Label ID="lblInfo" runat="server" CssClass="label" Text='Itens com "******" no campo Nº Nota Fiscal referem-se a Notas Fiscais ainda não impressas.'
                                    Width="579px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
