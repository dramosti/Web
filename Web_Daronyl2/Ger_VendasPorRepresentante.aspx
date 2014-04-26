<%@ Page Title="Gráfico de Vendas" Language="C#" MasterPageFile="Ger_Site.master" AutoEventWireup="true"
    CodeFile="Ger_VendasPorRepresentante.aspx.cs" Inherits="Ger_VendasPorRepresentante" %>

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
                        <td style="text-align: left; color: Black" class="Titulo" colspan="3">
                            Representante
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
                        <td class="label" align="right">
                            Todos
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="cbkTodos" runat="server"   AutoPostBack="True"
                                OnCheckedChanged="cbkTodos_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="label">
                            Representante
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cbxRepresentante" runat="server" AutoPostBack="True" CssClass="textBox"
                                DataTextField="nm_vend" DataValueField="cd_vend" Enabled="true" Width="387px">
                            </asp:DropDownList>
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
                    <caption>
                        <tr>
                            <td colspan="2" style="height: 39px; text-align: center">
                                <cc1:HlpWebButton ID="btnPesquisar" runat="server" CssClass="button" Text="Pesquisar"
                                    Width="124px" OnClick="btnPesquisar_Click" />
                            </td>
                        </tr>
                    </caption>
                </table>
            </asp:View>
            <asp:View ID="Informacao" runat="server">
                <table style="width: 100%">
                    <tr class="BordaInferior">
                        <td style="text-align: left; color: Black" class="Titulo" colspan="3">
                            Gráfico</td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                         <asp:Chart ID="grafico" runat="server" BackColor="WhiteSmoke" BackGradientStyle="TopBottom"
                        BackSecondaryColor="White" BorderColor="#1A3B69" BorderlineDashStyle="Solid"
                        BorderWidth="2px" Height="400px" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)"
                        Width="800px" EnableViewState="True">
                        <Titles>
                            <asp:Title Font="Trebuchet MS, 12px, style=Bold" ForeColor="26, 59, 105" ShadowColor="32, 0, 0, 0"
                                ShadowOffset="3" Text="Vendas no Ano">
                            </asp:Title>
                        </Titles>
                        <Legends>
                            <asp:Legend BackColor="Transparent" Font="Trebuchet MS, 8.25pt, style=Bold"
                                IsTextAutoFit="False" Name="Default">
                            </asp:Legend>
                        </Legends>
                        <BorderSkin SkinStyle="Emboss" />
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
