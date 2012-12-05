<%@ Page Title="Clientes Cadastrados" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeFile="ConsultaClientes.aspx.cs"
    Inherits="ConsultaClientes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="HLP.Web.Controles" Namespace="HLP.Web.Controles" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div align="center">
        <asp:Panel ID="Panel1" runat="server"  Width="100%" Style="text-align: center">
            <cc1:HlpWebMultiView ID="MultViewConsultaClientes" runat="server" ActiveViewIndex="0">
                <cc1:HlpWebView ID="ViewClientes" runat="server">
                <div align="center">
                    <cc1:HlpWebGridView ID="gridConsultaClientes" runat="server" AutoGenerateColumns="False"
                        CssClass="mGrid" PagerStyle-CssClass="pgr" Font-Names="Segoe UI" Font-Size="13px"
                        AlternatingRowStyle-CssClass="alt" AllowPaging="True" 
                        OnPageIndexChanging="gridConsultaClientes_PageIndexChanging" Width="100%" 
                        RowHeaderColumn="CD_ALTER" DataKeyNames="CD_ALTER"
                        OnRowDeleting="gridConsultaClientes_RowDeleting" 
                        OnRowCommand="gridConsultaClientes_RowCommand">
                        <AlternatingRowStyle CssClass="alt" />
                        <Columns>
                            <asp:TemplateField HeaderText="Código Alter.">
                                <ItemTemplate>
                                    <asp:HyperLink ID="HyperLink1" runat="server" Text='<%# Eval("CD_ALTER", "{0:D7}") %>'
                                        NavigateUrl='<%# Eval("CD_ALTER", "CadastroCliente.aspx?CD_ALTER={0}") %>'
                                        designer:wfdid="w6"></asp:HyperLink></ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Raz&#227;o Social">
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" Text='<%# Bind("NM_CLIFOR") %>' ID="TextBox1"></asp:TextBox></EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Bind("NM_CLIFOR") %>' ID="Label1"></asp:Label></ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Guerra">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("NM_GUERRA") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("NM_GUERRA") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cidade">
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" Text='<%# Bind("NM_CIDNOR") %>' ID="TextBox4"></asp:TextBox></EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Bind("NM_CIDNOR") %>' ID="Label4"></asp:Label></ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estado">
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" Text='<%# Bind("CD_UFNOR") %>' ID="TextBox2"></asp:TextBox></EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Bind("CD_UFNOR") %>' ID="Label2"></asp:Label></ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="CD_FONECOM" HeaderText="Fone">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:ButtonField CommandName="Pendencias" Text="Pendências" />
                        </Columns>
                        <PagerSettings FirstPageText="In&#237;cio" LastPageText="Fim" NextPageText="Pr&#243;ximo"
                            PreviousPageText="Anterior" Mode="NextPreviousFirstLast" />
                        <PagerStyle CssClass="pgr" />
                    </cc1:HlpWebGridView>
                    </div>
                    <br />
                    <div align="center">
                    <asp:Label runat="server" ID="lblCliente" CssClass="label" />
                        <asp:GridView ID="GridDuplicatas" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                CssClass="mGrid" PagerStyle-CssClass="pgr" Font-Names="Segoe UI" Font-Size="13px"
                                AlternatingRowStyle-CssClass="alt" CaptionAlign="Left" GridLines="None" ShowFooter="True"
                                Width="350px" PageSize="15" OnRowDataBound="GridDuplicatas_RowDataBound" >
                                <AlternatingRowStyle CssClass="alt" />
                                <Columns>
                                    <asp:BoundField DataField="DT_VENCI" DataFormatString="{0:dd/MM/yyyy}" 
                                        HeaderText="Vencimento">
                                    <ItemStyle HorizontalAlign="Left" Width="150px" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VL_DOC" DataFormatString="{0:c}" HeaderText="Valor">
                                    <ItemStyle HorizontalAlign="Left" Width="200px" Wrap="False" />
                                    <FooterStyle Font-Names="Segoe UI" Font-Size="8pt" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="pgr" />
                            </asp:GridView>
                            </div>
                              <br />
                    <asp:Button ID="btnEfeturaPesquisa" runat="server" Text="Efetuar nova pesquisa" Width="170px"
                        OnClick="lblVoltar_Click" CssClass="button" />
                    <br />
                    <br />
                    <br />
                    <asp:Button ID="btnImprimir" runat="server" Text="Imprimir Dados" Width="170px" OnClick="lblImprimir_Click"
                        CssClass="button" />
                    <br />
                </cc1:HlpWebView>
                &nbsp;
            </cc1:HlpWebMultiView>
        </asp:Panel>
    </div>
</asp:Content>
