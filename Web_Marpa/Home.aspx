<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeFile="Home.aspx.cs" Inherits="Home" %>

<%@ Register Assembly="HLP.Web.Controles" Namespace="HLP.Web.Controles" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .style2
        {
            width: 70%;
            height: 13px;
        }
        .style5
        {
            height: 16px;
        }
        .BordaInferior td
        {
            border-bottom: 2px solid black;
        }
        .style7
        {
            height: 16px;
            width: 7%;
        }
        .style8
        {
            height: 14px;
            width: 7%;
        }
        .style9
        {
            height: 16px;
            width: 66%;
        }
        .style10
        {
            width: 660px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div id="graficos" align="left">
        <table width="100%">
            <tr class="BordaInferior">
                <td style="text-align: left;" class="Titulo" colspan="3">
                    Gráficos
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td class="label" align="left">
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="btnCarregaGrafico_Click">Vendas no Ano</asp:LinkButton>
                </td>
               <td class="label" align="left">
                    <asp:LinkButton ID="LinkButton2" runat="server" onclick="LinkButton2_Click">Top 5 Produtos</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td align="left" class="style10">
                    <asp:Chart ID="graficoVendasAnuais" runat="server" BackColor="WhiteSmoke" BackGradientStyle="TopBottom"
                        BackSecondaryColor="White" BorderColor="#1A3B69" BorderlineDashStyle="Solid"
                        BorderWidth="2px" Height="230px" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)"
                        Width="420px" EnableViewState="True" Visible="False">
                        <Titles>
                            <asp:Title Font="Trebuchet MS, 12px, style=Bold" ForeColor="26, 59, 105" ShadowColor="32, 0, 0, 0"
                                ShadowOffset="3" Text="Vendas no Ano">
                            </asp:Title>
                        </Titles>
                        <Legends>
                            <asp:Legend BackColor="Transparent" Enabled="False" Font="Trebuchet MS, 8.25pt, style=Bold"
                                IsTextAutoFit="False" Name="Default">
                            </asp:Legend>
                        </Legends>
                        <BorderSkin SkinStyle="Emboss" />
                        <Series>
                            <asp:Series BorderColor="180, 26, 59, 105" ChartArea="ChartArea1" Color="220, 65, 140, 240"
                                Name="Series1" XValueMember="Mes" YValueMembers="Total" IsValueShownAsLabel="True"
                                ChartType="Line">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea BackColor="WhiteSmoke" BackSecondaryColor="White" BorderColor="64, 64, 64, 64"
                                Name="ChartArea1" ShadowColor="Transparent">
                                <Area3DStyle Enable3D="True" Inclination="15" IsClustered="False" IsRightAngleAxes="False"
                                    PointGapDepth="0" Rotation="10" WallWidth="0" />
                                <AxisY LineColor="64, 64, 64, 64">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                </AxisY>
                                <AxisX LineColor="64, 64, 64, 64">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                </AxisX>
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </td>
                <td align="right">
                    <asp:Chart ID="graficoTopCincoProd" runat="server" Width="420px" Height="230px" BackColor="WhiteSmoke"
                        BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2px" BorderColor="#1A3B69"
                        ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)" BackSecondaryColor="White"
                        EnableViewState="True" Visible="False">
                        <Legends>
                            <asp:Legend BackColor="Transparent" Name="Legend1">
                            </asp:Legend>
                        </Legends>
                        <Titles>
                            <asp:Title Name="Title1" Text="Top 5 produtos mais vendidos nos últimos 30 dias"
                                Font="Trebuchet MS, 12px, style=Bold" ForeColor="26, 59, 105" ShadowColor="32, 0, 0, 0"
                                ShadowOffset="3">
                            </asp:Title>
                        </Titles>
                        <BorderSkin SkinStyle="Emboss" />
                        <Series>
                            <asp:Series BorderColor="180, 26, 59, 105" ChartType="Pie" Color="220, 65, 140, 240"
                                Name="Default" XValueMember="DESCRICAO" YValueMembers="QTDE" IsValueShownAsLabel="True"
                                Legend="Legend1">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea BackColor="Transparent" BackSecondaryColor="Transparent" BorderColor="64, 64, 64, 64"
                                BorderWidth="0" Name="ChartArea1" ShadowColor="Transparent">
                                <Area3DStyle Rotation="0" Enable3D="True" />
                                <AxisY LineColor="64, 64, 64, 64">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                </AxisY>
                                <AxisX LineColor="64, 64, 64, 64">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                </AxisX>
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </td>
            </tr>
        </table>
    </div>
    <div align="center" style="color: Black">
        <br />
        <table width="100%">
            <tr class="BordaInferior">
                <td style="text-align: left;" class="Titulo" colspan="3">
                    Avisos
                </td>
            </tr>
            <tr>
                <td style="text-align: left;" class="style5" colspan="3">
                    <asp:Label ID="lblTotalAvisos" runat="server" CssClass="label" Visible="True" Width="100px"></asp:Label>
                </td>
            </tr>
            <tr align="center">
                <td style="text-align: center;" class="style5" colspan="3">
                    <asp:GridView ID="gridAvisos" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                        GridLines="None" Font-Names="Segoe UI" Font-Size="13px" Width="500px" DataKeyNames="CD_AVISO"
                        Font-Bold="False" PageSize="50" OnSelectedIndexChanged="gridAvisos_SelectedIndexChanged"
                        BorderStyle="None">
                        <FooterStyle BackColor="#3a4f63" Font-Names="Segoe UI" Font-Size="10px" ForeColor="Black"
                            BorderStyle="Solid" />
                        <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="DS_TITULO" HeaderText="Título">
                                <ItemStyle HorizontalAlign="Left" Width="450px" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DT_FINALAVISO" HeaderText="Data Final" />
                            <asp:CommandField SelectText="Ler" ShowSelectButton="True" />
                        </Columns>
                        <PagerStyle CssClass="pgr"></PagerStyle>
                        <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                        <PagerSettings FirstPageText="Início" LastPageText="Fim" Mode="NextPreviousFirstLast"
                            NextPageText="Próximo" PreviousPageText="Anterior" />
                    </asp:GridView>
                    <asp:DetailsView ID="dvAviso" runat="server" BackColor="White" BorderColor="#CCCCCC"
                        CssClass="mGrid" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black"
                        GridLines="Horizontal" Height="50px" Width="500px" AutoGenerateRows="False" Font-Bold="False"
                        Font-Names="Segoe UI" Font-Size="13px">
                        <EditRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                        <Fields>
                            <asp:BoundField DataField="DS_AVISO" ShowHeader="False">
                                <ItemStyle HorizontalAlign="Left" Font-Names="Segoe UI" Font-Size="13px" Font-Bold="False" />
                            </asp:BoundField>
                        </Fields>
                        <FooterStyle BackColor="#CCCC99" Font-Names="Segoe UI" Font-Size="13px" ForeColor="Black" />
                        <HeaderStyle BackColor="#3a4f63" Font-Bold="false" Font-Names="Segoe UI" Font-Size="13px"
                            ForeColor="White" />
                        <PagerStyle BackColor="White" Font-Names="Segoe UI" Font-Size="13px" ForeColor="Black"
                            HorizontalAlign="Left" />
                    </asp:DetailsView>
                </td>
            </tr>
            <tr class="BordaInferior">
                <td style="text-align: left; color: Black" class="Titulo" colspan="3">
                    Contato
                </td>
            </tr>
            <tr>
                <td style="text-align: center;" class="style8">
                </td>
                <td style="text-align: left;" class="label">
                    Fone 1:
                    <cc1:HlpWebLabel ID="lblFone1" runat="server"></cc1:HlpWebLabel>
                </td>
                <td style="text-align: center;" class="style2">
                </td>
            </tr>
            <tr>
                <td style="text-align: center;" class="style7">
                </td>
                <td style="text-align: left;" class="label">
                    Fone 2:
                    <cc1:HlpWebLabel ID="lblFone2" runat="server"></cc1:HlpWebLabel>
                </td>
                <td style="text-align: center;" class="style2">
                </td>
            </tr>
            <tr>
                <td style="text-align: center;" class="style7">
                </td>
                <td style="text-align: left;" class="label">
                    E-mail:
                    <cc1:HlpWebLabel ID="lblEmail" runat="server"></cc1:HlpWebLabel>
                </td>
                <td style="text-align: center;" class="style2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="text-align: center;" class="style7">
                    &nbsp;
                </td>
                <td style="text-align: center;" class="style9">
                    &nbsp;
                </td>
                <td style="text-align: center;" class="style2">
                    &nbsp;
                </td>
            </tr>
            <tr class="BordaInferior">
                <td style="text-align: left;" class="Titulo" colspan="3">
                    <strong><b>Endereço</b></strong>
                </td>
            </tr>
            <tr>
                <td style="text-align: center;" class="style7">
                </td>
                <td style="text-align: left;" class="label">
                    <cc1:HlpWebLabel ID="lblEnderecoCabec" runat="server"></cc1:HlpWebLabel>
                </td>
                <td style="text-align: center;" class="style2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="text-align: center;" class="style7">
                </td>
                <td style="text-align: left;" class="label">
                    <cc1:HlpWebLabel ID="lblEnderecoRodape" runat="server"></cc1:HlpWebLabel>
                </td>
                <td style="text-align: center;" class="style2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="text-align: center;" class="style7">
                    &nbsp;
                </td>
                <td style="text-align: center;" class="style9">
                    &nbsp;
                </td>
                <td style="text-align: center;" class="style2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="text-align: center;" class="style5" colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="text-align: center;" class="style5" colspan="3">
                    <asp:Button ID="btnAcessar" runat="server" CssClass="button" Text="Acessar Sistema"
                        Width="147px" OnClick="btnAcessar_Click" />
                </td>
            </tr>
            <tr>
                <td style="text-align: center;" class="style5" colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="text-align: center;" class="style5" colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="text-align: center;" class="style5" colspan="3">
                    &nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
