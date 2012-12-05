<%@ Page Title="Lista de Preço" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="ListaPreco.aspx.cs" Inherits="ListaPreco" %>

<%@ Register Assembly="HLP.Web.Controles" Namespace="HLP.Web.Controles" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
 <table style="width: 100%">
        <tr class="BordaInferior">
            <td style="text-align: left; color: Black" class="Titulo" colspan="3">
                Pesquisar Produto
            </td>
        </tr>
        <tr>
            <td>
                <table style="width: 100%; font-size: 13px; font-family: Segoe UI;">
                    <tr>
                        <td align="right" class="label">
                            Código
                        </td>
                        <td align="left">
                             <asp:TextBox ID="txtCodProd" runat="server" CssClass="textBox" Width="127px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right" class="label">
                            Descrição
                        </td>
                        <td align="left">
                               <asp:TextBox ID="txtProdDesc" runat="server" CssClass="textBox" Width="321px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right" class="label">
                            Lista de Preço
                        </td>
                        <td align="left">
                             <asp:DropDownList ID="cbxListaPreco" runat="server" CssClass="textBox" Width="235px"
                                    AutoPostBack="True" OnSelectedIndexChanged="cbxListaPreco_SelectedIndexChanged">
                                </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <br />
                            <asp:Button ID="Button1" runat="server" OnClick="btnLoc_Click" Text="Localizar" CssClass="button"
                                Width="131px" />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                                <asp:GridView ID="GridViewDb" CssClass="mGrid" PagerStyle-CssClass="pgr" Font-Names="Segoe UI"
                                    Font-Size="13px" AlternatingRowStyle-CssClass="alt" runat="server" AllowPaging="True"
                                    AutoGenerateColumns="False" Caption="&lt;b&gt;Produtos&lt;/b&gt;" CaptionAlign="Left"
                                    GridLines="None" OnPageIndexChanging="GridViewDb_PageIndexChanging" Width="100%"
                                    PageSize="100">
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:BoundField DataField="CD_ALTER" HeaderText="Código Alternativo" Visible="False">
                                            <ItemStyle HorizontalAlign="Left" Width="80px" Wrap="False" />
                                            <HeaderStyle HorizontalAlign="Left" Width="50px" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DS_PROD" HeaderText="Descrição">
                                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                            <ItemStyle HorizontalAlign="Left" Width="200px" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Vl. Unitário">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("VL_PRECOVE") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemStyle Font-Names="Segoe UI" Font-Size="13px" HorizontalAlign="Left" Width="60px"
                                                Wrap="False" />
                                            <HeaderStyle Font-Names="Segoe UI" Font-Size="13px" HorizontalAlign="Left" Wrap="False" />
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("VL_PRECOVE", "{0:N2}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CD_PROD" HeaderText="Código">
                                            <ItemStyle HorizontalAlign="Left" Width="60px" Font-Names="Segoe UI" Font-Size="13px" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="VL_PESOBRU" DataFormatString="{0:0.0000}" HeaderText="Peso">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="60px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="QT_ESTOQUE" DataFormatString="{0:0.00000}" HeaderText="Estoque">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="60px" />
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerStyle CssClass="pgr"></PagerStyle>
                                    <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                    <PagerSettings FirstPageText="Início" LastPageText="Fim" Mode="NextPreviousFirstLast"
                                        NextPageText="Próximo" PreviousPageText="Anterior" />
                                </asp:GridView>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
   
</asp:Content>
