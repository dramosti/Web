<%@ Page Title="Pesquisar Pedidos" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeFile="PesquisarPedidos.aspx.cs" Inherits="PesquisarPedidos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="HLP.Web.Controles" Namespace="HLP.Web.Controles" TagPrefix="cc1" %>
<%@ Register Src="Componentes/HlpTextBoxData2.ascx" TagName="HlpTextBoxData2" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .style1 {
            width: 105px;
        }

        .auto-style1 {
            font-size: 14px;
            font-family: Segoe UI;
            color: Black;
            width: auto;
        }

        .auto-styleDireita {
            font-size: 14px;
            font-family: Segoe UI;
            color: Black;
            width: auto;
        }

        .auto-style2 {
            width: 21%;
        }
    </style>
    <script type="text/javascript">
        $('#<%=txtDataInicial.ClientID%>').live("blur", function (e) {

            var DataIni = $('#<%=txtDataInicial.ClientID%>').val();
            if (DataIni != "") {
                var valido = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/.test(DataIni);

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
    <script type="text/javascript">
        $('#<%=txtDataFinal.ClientID%>').live("blur", function (e) {

            var DataFim = $('#<%=txtDataFinal.ClientID%>').val();
            if (DataFim != "") {
                var valido = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/.test(DataFim);

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <table width="100%" runat="server">
        <tr align="center">
            <td >&nbsp;
            </td>
            <td>
                <div style="text-align: center">
                    <table style="width: 100%">
                        <tr class="BordaInferior">
                            <td style="text-align: left; color: Black" class="Titulo" colspan="3">Período Desejado
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td style="text-align: right;">
                                <cc1:HlpWebLabel ID="lblPeriodo" runat="server" CssClass="label">De </cc1:HlpWebLabel>
                            </td>
                            <td align="left" class="auto-styleDireita">
                                <asp:TextBox runat="server" CssClass="textBox" ID="txtDataInicial" Width="100px"
                                    MaxLength="10"></asp:TextBox><asp:CalendarExtender ID="CalendarExtender1" runat="server"
                                        TargetControlID="txtDataInicial" Format="dd/MM/yyyy" TodaysDateFormat="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                <asp:TextBox Text="a" runat="server" CssClass="textBox" BackColor="#E8E8E8" Enabled="false" Width="10" />

                                <asp:TextBox runat="server" CssClass="textBox" ID="txtDataFinal" MaxLength="10">
                                </asp:TextBox><asp:CalendarExtender
                                    ID="CalendarExtender2" runat="server" TargetControlID="txtDataFinal" Format="dd/MM/yyyy"
                                    TodaysDateFormat="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>

                        </tr>
                        <tr>
                            <td align="right">Cliente:
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cbxCliente" runat="server" AutoPostBack="True" CssClass="textBox"
                                    DataTextField="nm_clifor" DataValueField="cd_clifor" Enabled="true"
                                    Width="451px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">Pedido Cliente:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="HlpWebtxtPedCli" MaxLength="15" runat="server" CssClass="textBox" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">Pedido Torcetex:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="HlpWebtxtPedido" MaxLength="15" runat="server" CssClass="textBox" Width="100px"></asp:TextBox>
                            </td>
                            <td />
                        </tr>
                    </table>
                    <br />
                    <table style="width: 65%">
                        <tr>
                            <%--<td style="text-align: left">
                                <cc1:HlpWebButton ID="btnPesquisarComissao" runat="server" OnClick="btnPesquisar_Click"
                                    Text="Comissões" Width="124px" CssClass="button" Visible="False" />
                            </td>--%>
                            <td style="text-align: left">
                                <cc1:HlpWebButton ID="btnPesquisarPedido" runat="server" OnClick="btnPesquisar_Click"
                                    Text="Pesquisar" Width="124px" CssClass="button" />
                                <td style="text-align: left">
                                    <cc1:HlpWebButton ID="btnVoltar" runat="server" OnClick="btnVoltar_Click" Text="Voltar"
                                        Width="124px" CssClass="button" />
                                </td>
                        </tr>
            </td>
        </tr>
    </table> </div> </td>
        <td width="30%"></td>
        </tr>
    </table>
    <table style="width: 100%">
        <tr>
            <td style="width: 103px"></td>
            <td style="width: 108px">&nbsp;
            </td>
            <td style="width: 100px"></td>
        </tr>
    </table>
</asp:Content>
