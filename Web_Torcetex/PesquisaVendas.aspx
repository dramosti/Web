<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="PesquisaVendas.aspx.cs" Inherits="PesquisaVendas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="HLP.Web.Controles" Namespace="HLP.Web.Controles" TagPrefix="cc1" %>
<%@ Register Src="Componentes/HlpTextBoxData2.ascx" TagName="HlpTextBoxData2" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .auto-style2 {
            width: 400px;
        }
        .auto-style4 {
            width: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <table id="Table1" width="100%" runat="server">
        <tr align="center">
            <td>&nbsp;
            </td>
            <td>
                <div style="text-align: center">
                    <table style="width: 100%">
                        <tr class="BordaInferior">
                            <td style="text-align: left; color: Black" class="Titulo" colspan="3">Período Desejado
                            </td>
                        </tr>
                    </table>
                    <table style="width: 106%">
                        <tr>
                            <td style="text-align: right;" class="auto-style4">
                                <cc1:HlpWebLabel ID="lblPeriodo" runat="server" CssClass="label">De </cc1:HlpWebLabel>
                            </td>
                            <td align="left" class="auto-style2">
                                <asp:TextBox runat="server" CssClass="textBox" ID="txtDataInicial" Width="100px"
                                    MaxLength="10"></asp:TextBox><asp:CalendarExtender ID="CalendarExtender1" runat="server"
                                        TargetControlID="txtDataInicial" Format="dd/MM/yyyy" TodaysDateFormat="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                <asp:TextBox ID="TextBox1" Text="a" runat="server" CssClass="textBox" BackColor="#E8E8E8" Enabled="false" Width="10" />

                                <asp:TextBox runat="server" CssClass="textBox" ID="txtDataFinal" MaxLength="10">
                                </asp:TextBox><asp:CalendarExtender
                                    ID="CalendarExtender2" runat="server" TargetControlID="txtDataFinal" Format="dd/MM/yyyy"
                                    TodaysDateFormat="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>                         
                        </tr>
                    </table>
                    <br />
                    <table width="100%">
                        <tr align="left">
                           
                            <td>
                                <cc1:HlpWebButton ID="btnPesquisarPedido" runat="server" 
                                    Text="Pesquisar" Width="124px" CssClass="button" OnClick="btnPesquisarPedido_Click" /> 
                            </td>

                        </tr>
                    </table>                   
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

