<%@ Page Title="Pesquisar Pedidos" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeFile="PesquisarPedidos.aspx.cs" Inherits="PesquisarPedidos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="HLP.Web.Controles" Namespace="HLP.Web.Controles" TagPrefix="cc1" %>
<%@ Register Src="Componentes/HlpTextBoxData2.ascx" TagName="HlpTextBoxData2" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 200px;
        }
        .style2
        {
            width: 358px;
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
    <div style="text-align: center">
        <table style="width: 100%">
            <tr class="BordaInferior">
                <td style="text-align: left; color: Black" class="Titulo" colspan="3">
                    Selecione abaixo o critério para a Pesquisa de Pedidos
                </td>
            </tr>
            <tr>
                <td style="text-align: right" class="style2">
                    <cc1:HlpWebRadioButton ID="rdbTodos" runat="server" Text="Todos" CssClass="label"
                        GroupName="Opcoes" ForeColor="Black" Width="100px" AutoPostBack="True" OnCheckedChanged="rdbVariosPedidos_CheckedChanged"
                        Checked="True" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="text-align: right" class="style2">
                    <cc1:HlpWebRadioButton ID="rdbEspecifico" runat="server" CssClass="label" GroupName="Opcoes"
                        OnCheckedChanged="rdbEspecifico_CheckedChanged" AutoPostBack="True" Width="141px"
                        Text="Pedido Específico" />
                </td>
                <td style="text-align: left; height: 26px;">
                    &nbsp;<cc1:HlpWebTextBoxInteiro ID="txtNumeroPedido" runat="server" Enabled="False"
                        Width="105px" CssClass="textBox"></cc1:HlpWebTextBoxInteiro>
                </td>
            </tr>
            <tr class="BordaInferior">
                <td style="text-align: left; color: Black" class="Titulo" colspan="3">
                    Período Desejado
                </td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
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
                   
                      <asp:TextBox runat="server" CssClass="textBox" ID="txtDataFinal" Width="100px"
                        MaxLength="10"></asp:TextBox><asp:CalendarExtender ID="CalendarExtender2" runat="server"
                            TargetControlID="txtDataFinal" Format="dd/MM/yyyy" TodaysDateFormat="dd/MM/yyyy">
                        </asp:CalendarExtender>
                </td>
                <td style="width: 76px; height: 33px;">
                </td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
                <td style="width: 100px">
                </td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr class="BordaInferior">
                <td style="text-align: left; color: Black" class="Titulo" colspan="3">
                    Cliente
                </td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
                <td style="width: 100px">
                </td>
                <td style="width: 100px; text-align: right">
                    <cc1:HlpWebLabel ID="HlpWebLabel5" runat="server" CssClass="label">Razão Social</cc1:HlpWebLabel>
                </td>
                <td style="width: 100px; text-align: left">
                    <cc1:HlpWebTextBox ID="txtNomeCliente" runat="server" MaxLength="20" Width="271px"
                        CssClass="textBox"></cc1:HlpWebTextBox>
                </td>
                <td style="width: 100px">
                </td>
            </tr>
        </table>
        <br />
        <table style="width: 100%">
            <tr>
                <td style="width: 100px">
                </td>
                <td style="width: 100px; text-align: center">
                    <cc1:HlpWebButton ID="btnPesquisar" runat="server" OnClick="btnPesquisar_Click" Text="Pesquisar"
                        Width="124px" CssClass="button" />
                </td>
                <td style="width: 100px">
                </td>
                <td style="width: 100px; text-align: center">
                    <cc1:HlpWebButton ID="btnVoltar" runat="server" OnClick="btnVoltar_Click" Text="Voltar"
                        Width="116px" CssClass="button" />
                </td>
                <td style="width: 100px">
                </td>
            </tr>
        </table>
    </div>
    <table style="width: 100%">
        <tr>
            <td style="width: 103px">
            </td>
            <td style="width: 108px">
                &nbsp;
            </td>
            <td style="width: 100px">
            </td>
        </tr>
    </table>
</asp:Content>
