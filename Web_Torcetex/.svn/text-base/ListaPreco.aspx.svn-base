<%@ Page Title="Lista de Preço" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="ListaPreco.aspx.cs" Inherits="ListaPreco" %>

<%@ Register Assembly="HLP.Web.Controles" Namespace="HLP.Web.Controles" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div>
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
                                <asp:TextBox ID="txtCodProd" runat="server" CssClass="textBox" Width="127px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="label">
                                Descrição
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtProdDesc" runat="server" CssClass="textBox" Width="321px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="label">
                                Linha de Produto
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cbxLinhaProduto" runat="server" CssClass="textBox" DataTextField="DS_LINHA"
                                    DataValueField="CD_LINHA" OnSelectedIndexChanged="cbxListaPreco_SelectedIndexChanged"
                                    Width="328px" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="label">
                                Lista de Preço
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cbxListaPreco" runat="server" CssClass="textBox" Width="235px"
                                    AutoPostBack="True" OnSelectedIndexChanged="cbxListaPreco_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <br />
                                <asp:Button ID="btnLoc" runat="server" OnClick="btnLoc_Click" Text="Localizar" CssClass="button"
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
                                <asp:GridView ID="GridViewDb" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    CssClass="mGrid" PagerStyle-CssClass="pgr" Font-Names="Segoe UI" Font-Size="13px"
                                    AlternatingRowStyle-CssClass="alt" Caption="&lt;b&gt;Produtos&lt;/b&gt;"
                                    CaptionAlign="Left" GridLines="None" OnPageIndexChanging="GridViewDb_PageIndexChanging"
                                    Width="100%" OnSelectedIndexChanged="GridViewDb_SelectedIndexChanged" DataKeyNames="CD_PROD"
                                    PageSize="20">
                                    <AlternatingRowStyle CssClass="alt" />
                                    <Columns>
                                        <asp:BoundField DataField="CD_PROD" HeaderText="Código">
                                        <ItemStyle Font-Names="Segoe UI" Font-Size="13px" HorizontalAlign="Left" Width="60px" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CD_BARRAS" HeaderText="Código Barras">
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
                                        <asp:BoundField HeaderText="Vl. IVA" Visible="False">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="60px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="VL_PESOBRU" DataFormatString="{0:0.0000}" HeaderText="Peso"
                                            Visible="False">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="60px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="QT_ESTOQUE" DataFormatString="{0:0.00000}" HeaderText="Estoque"
                                            Visible="False">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="60px" />
                                        </asp:BoundField>
                                        <asp:ButtonField CommandName="Alertar" Text="Alertar" Visible="False">
                                        <ControlStyle BackColor="Transparent" BorderStyle="None" Font-Names="Segoe UI" Font-Size="13px" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="30px" />
                                        </asp:ButtonField>
                                        <asp:CommandField ButtonType="Link" CancelText="" DeleteText="" EditText="" NewText=""
                                            SelectText="Detalhes" ShowSelectButton="True">
                                        <HeaderStyle Width="20px" />
                                        <ItemStyle BorderStyle="None" Font-Underline="True" ForeColor="Blue" />
                                        </asp:CommandField>
                                    </Columns>
                                    <PagerStyle CssClass="pgr" />
                                    <SelectedRowStyle BackColor="#EFEFEF" Font-Bold="True" ForeColor="White" />
                                    <PagerSettings FirstPageText="Início" LastPageText="Fim" Mode="NextPreviousFirstLast"
                                        NextPageText="Próximo" PreviousPageText="Anterior" />
                                </asp:GridView>
                                <br />
                                <asp:DetailsView ID="dvAviso" runat="server" GridLines="None" Height="30px" Width="100%"
                                    AutoGenerateRows="False" CssClass="mGrid" PagerStyle-CssClass="pgr" Font-Names="Segoe UI"
                                    Font-Size="13px" AlternatingRowStyle-CssClass="alt">
                                    <AlternatingRowStyle BorderStyle="None" />
                                    <Fields>
                                        <asp:BoundField DataField="DS_PROD" HeaderText="Descrição">
                                        <HeaderStyle Width="150px" />
                                        <ItemStyle Font-Bold="True" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="VL_PESOBRU" HeaderText="Peso">
                                        <ItemStyle Font-Bold="True" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="QT_ESTOQUE" HeaderText="Estoque">
                                        <ItemStyle Font-Bold="True" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DS_CLASFIS" HeaderText="Classificação Fiscal">
                                        <ItemStyle Font-Bold="True" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="VL_ALIIPI" HeaderText="% IPI">
                                        <ItemStyle Font-Bold="True" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="VL_REDBASE" HeaderText="% Redução">
                                        <ItemStyle Font-Bold="True" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="VL_ALIQUOT" HeaderText="% ICMS">
                                        <ItemStyle Font-Bold="True" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="VL_ALISUBS" HeaderText="% Subst. Tributária">
                                        <ItemStyle Font-Bold="True" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CD_SITTRIB" HeaderText="Situação Tributária">
                                        <ItemStyle Font-Bold="True" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                    </Fields>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <PagerStyle CssClass="pgr" />
                                </asp:DetailsView>
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
    </div>
</asp:Content>
