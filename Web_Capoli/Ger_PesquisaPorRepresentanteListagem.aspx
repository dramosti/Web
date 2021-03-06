﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Ger_Site.master" AutoEventWireup="true" CodeFile="Ger_PesquisaPorRepresentanteListagem.aspx.cs" Inherits="Ger_PesquisaPorRepresentanteListagem" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="HLP.Web.Controles" Namespace="HLP.Web.Controles" TagPrefix="cc1" %>
<%@ Register Src="Componentes/HlpTextBoxData2.ascx" TagName="HlpTextBoxData2" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
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
    <script language="javascript" type="text/javascript">
        function onClickDesabilita() {
            document.getElementById("MainContent_cbxVendedor").disabled = !document.getElementById("MainContent_ckbVendedor").checked;
        }
    </script>
    <style type="text/css">
        .style1
        {
            width: 9px;
        }
        .style2
        {
            width: 227px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div align="center">
        <table style="width: 100%;">
            <tr class="BordaInferior">
                <td style="text-align: left; color: Black" class="Titulo" colspan="2">
                    Filtro
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr align="center">
                            <td style="text-align: right;" class="style1">
                                <cc1:HlpWebLabel ID="lblPeriodo" runat="server" CssClass="label">De </cc1:HlpWebLabel>
                            </td>
                            <td style="width: 54px; height: 33px;">
                                <asp:TextBox runat="server" CssClass="textBox" ID="txtDataInicial" Width="100px"
                                    MaxLength="10"></asp:TextBox><asp:CalendarExtender ID="CalendarExtender1" runat="server"
                                        TargetControlID="txtDataInicial" Format="dd/MM/yyyy" TodaysDateFormat="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                            </td>
                            <td style="width: 32px; height: 33px;">
                                <cc1:HlpWebLabel ID="HlpWebLabel1" runat="server" CssClass="label">a</cc1:HlpWebLabel>
                            </td>
                            <td style="width: 89px; height: 33px;">
                                <asp:TextBox runat="server" CssClass="textBox" ID="txtDataFinal" Width="100px" MaxLength="10"></asp:TextBox><asp:CalendarExtender
                                    ID="CalendarExtender2" runat="server" TargetControlID="txtDataFinal" Format="dd/MM/yyyy"
                                    TodaysDateFormat="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr align="center">
                            <td align="right">
                                <input type="checkbox" runat="server" id="ckbVendedor" name="ckbVendedor" class="label"
                                    onclick="onClickDesabilita()">
                                Vendedor
                            </td>
                            <td class="style2">
                                <asp:DropDownList ID="cbxVendedor" runat="server" CssClass="textBox" Width="235px"
                                    AutoPostBack="False" Enabled="False">
                                </asp:DropDownList>
                            </td>
                            <td align="center">
                                <asp:Button ID="btnVisualizar" CssClass="button" Text="Visualizar" runat="server"
                                    Width="115px" OnClick="btnVisualizar_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>    
</asp:Content>

