<%@ Page Title="" Language="C#" MasterPageFile="~/MasterDownload.master" AutoEventWireup="true"
    CodeFile="DownloadGeraXml.aspx.cs" Inherits="DownloadGeraXml" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .mGrid
        {
            margin-right: 161px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="main">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
        <br />
        <table class="style1" style="width: 42%">
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Selecione o Programa: "></asp:Label>
                    <asp:DropDownList ID="cbxPrograma" runat="server" Height="26px" Width="98px">
                        <asp:ListItem>GeraXML</asp:ListItem>
                        <asp:ListItem>SPED</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Tipo de Executável: "></asp:Label>
                    <asp:DropDownList ID="cbxTpExe" runat="server" Height="26px" Width="98px">
                        <asp:ListItem Value="0">Oficial</asp:ListItem>
                        <asp:ListItem Value="1">Testes</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnCarregar" runat="server" Height="26px" Text="Carregar" Width="89px"
                        OnClick="btnCarregar_Click" />
                </td>
            </tr>
            <tr class="BordaInferior">
                <td style="text-align: left; color: Black" class="Titulo" colspan="2">
                    Versões - GeraXml 3.0&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblVersaoCli" runat="server" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" class="style3">
                    <asp:GridView ID="gvVersoes" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="alt"
                        AutoGenerateColumns="False" CaptionAlign="Left" CssClass="mGrid" Font-Names="Segoe UI"
                        Font-Size="13px" GridLines="None" PagerStyle-CssClass="pgr" Width="100%" DataKeyNames="id"
                        OnRowCommand="gvVersoes_RowCommand" OnSelectedIndexChanged="gvVersoes_SelectedIndexChanged"
                        OnPageIndexChanging="gvVersoes_PageIndexChanging" PageSize="6">
                        <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                        <Columns>
                            <asp:CommandField SelectText="Detalhes" ShowSelectButton="True">
                                <ItemStyle ForeColor="Red" Width="80px" Wrap="False" />
                            </asp:CommandField>
                            <asp:BoundField DataField="id" HeaderText="ID">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="20px" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Versao" HeaderText="Versão">
                                <ItemStyle HorizontalAlign="Left" Width="100px" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Left" Width="50px" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Data" HeaderText="Data">
                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                <ItemStyle HorizontalAlign="Left" Width="200px" Wrap="True" />
                            </asp:BoundField>
                            <asp:ButtonField CommandName="Baixar" Text="Baixar" HeaderText="Download">
                                <ControlStyle BackColor="Transparent" BorderStyle="None" Font-Names="Segoe UI" Font-Size="13px" />
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="30px" />
                            </asp:ButtonField>
                        </Columns>
                        <PagerStyle CssClass="pgr"></PagerStyle>
                        <SelectedRowStyle BackColor="#EFEFEF" Font-Bold="True" ForeColor="White" />
                        <PagerSettings FirstPageText="Início" LastPageText="Fim" Mode="NextPreviousFirstLast"
                            NextPageText="Próximo" PreviousPageText="Anterior" />
                    </asp:GridView>
                    <br />
                </td>
            </tr>
            <tr class="BordaInferior">
                <td style="text-align: left; color: Black" class="Titulo" colspan="2">
                    Pré Requisitos
                </td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="linkFrameWork" runat="server" OnClick="LinkButton1_Click">FrameWork 4.0</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="linkCrystal64" runat="server" OnClick="LinkButton1_Click">Crystal Report 13 (64 bit)</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="linkCrystal32" runat="server" OnClick="LinkButton1_Click">Crystal Report 13 (32 bit)</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" PostBackUrl="http://www.microsoft.com/pt-br/download/details.aspx?id=32">WIC - Windows Imaging Component</asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
