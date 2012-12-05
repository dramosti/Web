<%@ Page Title="Pedidos" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeFile="ConsultaPedidos.aspx.cs" Inherits="ConsultaPedidos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="HLP.Web.Controles" Namespace="HLP.Web.Controles" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div align="center">
        <asp:GridView ID="gridConsultaPedidos" runat="server" CssClass="mGrid" PagerStyle-CssClass="pgr"
            Font-Names="Segoe UI" Font-Size="13px" AlternatingRowStyle-CssClass="alt" GridLines="None"
            AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="gridConsultaPedidos_PageIndexChanging"
            PageSize="7" Width="100%" DataKeyNames="CD_PEDIDO" OnSelectedIndexChanged="gridConsultaPedidos_SelectedIndexChanged"
            OnRowCommand="gridConsultaPedidos_RowCommand">
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:BoundField DataField="CD_PEDIDO" HeaderText="Pedido" >
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Cliente">
                    <EditItemTemplate>
                        <asp:TextBox runat="server" Text='<%# Bind("NM_GUERRA") %>' ID="txtNmGuerra"></asp:TextBox>
                    </EditItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Bind("NM_GUERRA") %>' ID="lblNmGuerra"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Data de Emiss&#227;o">
                    <EditItemTemplate>
                        <asp:TextBox runat="server" Text='<%# Bind("DT_PEDIDO") %>' ID="TextBox1"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Bind("DT_PEDIDO", "{0:dd/MM/yyyy}") %>' ID="lblDtPedido"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:ButtonField Text="Status" CommandName="Pedido" ItemStyle-ForeColor="Blue">
                    <ItemStyle ForeColor="Blue" />
                </asp:ButtonField>
                <asp:ButtonField Text="Email/Visualizar" CommandName="Email" ItemStyle-ForeColor="Blue">
                    <ItemStyle ForeColor="Blue" />
                </asp:ButtonField>
            </Columns>
            <PagerSettings FirstPageText="In&#237;cio" LastPageText="Fim" NextPageText="Pr&#243;ximo"
                PreviousPageText="Anterior" Mode="NextPreviousFirstLast" />
            <PagerStyle CssClass="pgr" />
        </asp:GridView>
        <br />
        <br />
        <asp:Button ID="lblVoltar" runat="server" Text="Efetuar nova pesquisa" Width="175px"
            OnClick="lblVoltar_Click" CssClass="button" />
        <br />
        <br />
        <asp:Button ID="btnComissao" runat="server" Text="Imprimir Comissão" Width="175px"
            CssClass="button" OnClick="btnComissao_Click" />
        <br />
        <br />
    </div>
</asp:Content>
