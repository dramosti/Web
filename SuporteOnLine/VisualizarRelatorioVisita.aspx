<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VisualizarRelatorioVisita.aspx.cs"
    Inherits="VisualizarRelatorioVisita" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Relatório de Visita</title>
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 715px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table align="center" class="style1" style="background-image: url('Imagem/Layout/Topo_r1_c1.jpg');
            background-repeat: no-repeat; height: 180px">
            <tr>
                <td style="width: 45%; height: 170px;">
                    &nbsp;
                </td>
                <td style="width: 55%; height: 170px;">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" style="background-image: url('Imagem/Layout/Topo_r4_c1.jpg');
                    background-repeat: no-repeat; height: 41px;" valign="middle">
                    <asp:Button ID="btnImprimir" runat="server" Font-Bold="False" Font-Names="Verdana"
                        Font-Size="X-Small" Text="Imprimir" OnClick="btnImprimir_Click" />&nbsp;
                    <asp:Button ID="BtnVoltar" runat="server" Height="20px" PostBackUrl="~/Default.aspx"
                        Text="Voltar" Width="70px" Font-Names="Verdana" Font-Size="X-Small" />
                </td>
            </tr>
        </table>
        <table align="center" class="style1">
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="padding: 5px; border: thin dotted #000000;">
                    <CR:CrystalReportViewer ID="RptVisita" runat="server" AutoDataBind="true" EnableDatabaseLogonPrompt="False"
                        DisplayToolbar="False" ToolPanelView="None" />
                </td>
            </tr>
        </table>
        <br />
    </div>
    </form>
</body>
</html>
