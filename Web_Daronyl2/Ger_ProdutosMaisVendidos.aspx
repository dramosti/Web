<%@ Page Title="" Language="C#" MasterPageFile="Ger_Site.master" AutoEventWireup="true"
    CodeFile="Ger_ProdutosMaisVendidos.aspx.cs" Inherits="Ger_ProdutosMaisVendidos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="HLP.Web.Controles" Namespace="HLP.Web.Controles" TagPrefix="cc1" %>
<%@ Register Src="Componentes/HlpTextBoxData2.ascx" TagName="HlpTextBoxData2" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .style2
        {
            width: 123px;
        }
        .style3
        {
            width: 25px;
        }
    </style>
    <script type="text/javascript">
        $('#<%=txtDataFinal.ClientID%>').live("blur", function (e) {
            var dtFim = $('#<%=txtDataFinal.ClientID%>').val();
            if (dtFim != "") {
                var valido = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/.test(dtFim);

                if (valido) {
                    return true;
                }
                else {
                    $('#<%=txtDataFinal.ClientID%>').val("");
                    return false;
                }
            }
        });
    </script>
    <script type="text/javascript">
        $('#<%=txtDataInicial.ClientID%>').live("blur", function (e) {
            var Data = $('#<%=txtDataInicial.ClientID%>').val();
            if (Data != "") {
                var valido = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/.test(Data);

                if (valido) {
                    return true;
                }
                else {
                    $('#<%=txtDataInicial.ClientID%>').val("");
                    return false;
                }
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div align="center">
        <asp:MultiView ID="MultViewVendasPorRepres" runat="server" ActiveViewIndex="0">
            <asp:View ID="Pesquisa" runat="server">
                <table style="width: 100%">
                    <tr class="BordaInferior">
                        <td style="text-align: left; color: Black" class="Titulo" colspan="3">
                            Filtro
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td align="right">
                                        Período 
                                    </td>
                                    <td align="left" class="style2">
                                        <asp:TextBox runat="server" CssClass="textBox" ID="txtDataInicial" Width="100px"
                                            MaxLength="10"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server"
                                                TargetControlID="txtDataInicial" Format="dd/MM/yyyy" TodaysDateFormat="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td align="center" class="style3">
                                        a
                                    </td>
                                    <td align="left">
                                        <asp:TextBox runat="server" CssClass="textBox" ID="txtDataFinal" Width="100px" MaxLength="10"></asp:TextBox>
                                        <asp:CalendarExtender
                                            ID="CalendarExtender2" runat="server" TargetControlID="txtDataFinal" Format="dd/MM/yyyy"
                                            TodaysDateFormat="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        Primeros 
                                    </td>
                                    <td align="left" class="style2">
                                        <asp:DropDownList ID="cbxQtde" runat="server" AutoPostBack="True" CssClass="textBox"
                                            Enabled="true" Width="70px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="style3">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
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
                            <asp:Chart ID="grafico" runat="server" Width="800px" Height="400px" BackColor="WhiteSmoke"
                                BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" 
                                BorderWidth="2px" BorderColor="#1A3B69"
                                ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)" 
                                BackSecondaryColor="White" EnableViewState="True">
                                <Legends>
                                    <asp:Legend BackColor="Transparent" Name="Legend1">
                                    </asp:Legend>
                                </Legends>
                                <Titles>
                                    <asp:Title Name="Title1" Text="Pie Chart" Font="Trebuchet MS, 12px, style=Bold" ForeColor="26, 59, 105"
                                        ShadowColor="32, 0, 0, 0" ShadowOffset="3">
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
