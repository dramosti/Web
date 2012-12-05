<%@ Page Title="Gráfico de Produtos" Language="C#" MasterPageFile="Ger_Site.master" AutoEventWireup="true"
    CodeFile="Ger_VendasProduto.aspx.cs" Inherits="Ger_VendasProduto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="HLP.Web.Controles" Namespace="HLP.Web.Controles" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div align="center">
        <asp:MultiView ID="MultViewVendasPorRepres" runat="server" ActiveViewIndex="0">
            <asp:View ID="Pesquisa" runat="server">
                <table style="width: 100%">
                    <tr class="BordaInferior">
                        <td style="text-align: left; color: Black" class="Titulo" colspan="2">
                            Produto
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
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
                        <td class="label" align="right">
                            Ano
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cbxAno" runat="server" AutoPostBack="True" CssClass="textBox"
                                Enabled="true" Width="122px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="height: 39px; text-align: center">
                            <cc1:HlpWebButton ID="btnPesquisar" runat="server" CssClass="button" Text="Localizar"
                                Width="124px" OnClick="btnPesquisar_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="height: 39px; text-align: center">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="height: 39px; text-align: center">
                            <asp:GridView ID="GridViewDb" CssClass="mGrid" PagerStyle-CssClass="pgr" Font-Names="Segoe UI"
                                Font-Size="13px" AlternatingRowStyle-CssClass="alt" runat="server" AllowPaging="True"
                                AutoGenerateColumns="False" Caption="&lt;b&gt;Produtos&lt;/b&gt;" CaptionAlign="Left"
                                GridLines="None" OnPageIndexChanging="GridViewDb_PageIndexChanging" OnRowCommand="GridViewDb_RowCommand"
                                Width="100%">
                                <AlternatingRowStyle CssClass="alt" />
                                <Columns>
                                    <asp:BoundField DataField="CD_ALTER" HeaderText="Código Alternativo" Visible="False">
                                        <ItemStyle HorizontalAlign="Left" Width="80px" Wrap="False" />
                                        <HeaderStyle HorizontalAlign="Left" Width="50px" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DS_PROD" HeaderText="Descrição">
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                        <ItemStyle HorizontalAlign="Left" Width="200px" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CD_PROD" HeaderText="Código">
                                        <ItemStyle Font-Names="Segoe UI" Font-Size="13px" HorizontalAlign="Left" Width="60px" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:ButtonField CommandName="Gerar" Text="Gerar_Gráfico">
                                        <ControlStyle BackColor="Transparent" BorderStyle="None" Font-Names="Segoe UI" Font-Size="13px" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="30px" />
                                    </asp:ButtonField>
                                </Columns>
                                <PagerStyle CssClass="pgr" />
                                <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                <PagerSettings FirstPageText="Início" LastPageText="Fim" Mode="NextPreviousFirstLast"
                                    NextPageText="Próximo" PreviousPageText="Anterior" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="Informacao" runat="server">
                <table style="width: 100%">
                    <tr class="BordaInferior">
                        <td style="text-align: left; color: Black" class="Titulo" colspan="3">
                            Gráfico
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:CHART ID="Chart1" runat="server" BackColor="WhiteSmoke" 
                                BackGradientStyle="TopBottom" BackSecondaryColor="White" 
                                BorderColor="26, 59, 105" BorderlineDashStyle="Solid" BorderWidth="2" 
                                Height="400px" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)" 
                                ImageType="Png" Palette="BrightPastel" Width="800px" 
                                EnableViewState="True">
                                <titles>
                                    <asp:Title Font="Trebuchet MS, 12 px, style=Bold" ForeColor="26, 59, 105" 
                                        ShadowColor="32, 0, 0, 0" ShadowOffset="3" Text="Quantidade Vendida do produto">
                                    </asp:Title>
                                </titles>
                                <legends>
                                    <asp:Legend BackColor="Transparent" Enabled="False" 
                                        Font="Trebuchet MS, 8.25pt, style=Bold" IsTextAutoFit="False" Name="Default">
                                    </asp:Legend>
                                </legends>
                                <borderskin SkinStyle="Emboss" />
                                <series>
                                    <asp:Series BorderColor="180, 26, 59, 105" ChartArea="ChartArea1" 
                                        Color="220, 65, 140, 240" Name="Series1" XValueMember="MES" 
                                        YValueMembers="QTDE" IsValueShownAsLabel="True">
                                    </asp:Series>
                                </series>
                                <chartareas>
                                    <asp:ChartArea BackColor="WhiteSmoke" BackSecondaryColor="White" 
                                        BorderColor="64, 64, 64, 64" Name="ChartArea1" ShadowColor="Transparent">
                                        <area3dstyle Enable3D="True" Inclination="15" IsClustered="False" 
                                            IsRightAngleAxes="False" PointGapDepth="0" Rotation="10" WallWidth="0" />
                                        <axisy LineColor="64, 64, 64, 64">
                                            <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                            <MajorGrid LineColor="64, 64, 64, 64" />
                                        </axisy>
                                        <axisx LineColor="64, 64, 64, 64">
                                            <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                            <MajorGrid LineColor="64, 64, 64, 64" />
                                        </axisx>
                                    </asp:ChartArea>
                                </chartareas>
                            </asp:CHART>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td align="right" class="style10">
                            <cc1:HlpWebButton ID="btnNovaPesquisa" runat="server" CssClass="button" Text="Nova Pesquisa"
                                Width="150px" OnClick="btnNovaPesquisa_Click" />
                        </td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
