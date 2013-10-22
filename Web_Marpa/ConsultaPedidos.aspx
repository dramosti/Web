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
                <asp:BoundField DataField="CD_PEDIDO" HeaderText="Pedido" HeaderStyle-HorizontalAlign="Left"
                    ItemStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Left" Width="80px"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="NM_CLIFOR" HeaderText="Razão Social"></asp:BoundField>
                <asp:BoundField DataField="DT_DOC" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Data Pedido">
                    <ItemStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="VL_TOTAL_RES" DataFormatString="{0:n}" HeaderText="Valor Reservado">
                    <ItemStyle HorizontalAlign="Right" Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="VL_TOTAL_LIB" DataFormatString="{0:n}" HeaderText="Valor Liberado">
                    <ItemStyle HorizontalAlign="Right" Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="VL_COMISSAO" DataFormatString="{0:n}" HeaderText="Valor Comissão"
                    Visible="False" />
                <asp:BoundField DataField="VL_PERCOMISSAO" DataFormatString="{0:n}" HeaderText="% Comissão"
                    Visible="False" />
                <asp:BoundField DataField="CD_PEDIDO" HeaderText="Nº da Nota / Ped." Visible="False" />
                <asp:BoundField DataField="DS_TIPODOC" Visible="False" />
                <asp:ButtonField Text="Status" CommandName="Pedido" ItemStyle-ForeColor="Blue">
                    <ItemStyle ForeColor="Blue" Width="70px" />
                </asp:ButtonField>
                <asp:ButtonField Text="Email/Visualizar" CommandName="Email" ItemStyle-ForeColor="Blue"
                    ItemStyle-Width="160px">
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
