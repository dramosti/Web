<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="PreviewNF.aspx.cs" Inherits="PreviewNF" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%">
                    <tr class="BordaInferior">
                        <td style="text-align: left;" class="Titulo" colspan="3">
                            Filtros
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <table align="center">
                    <tr align="right">
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Data Início"></asp:Label>&nbsp;&nbsp;
                            <asp:TextBox runat="server" ID="txtDtEmissao" Width="100px" MaxLength="10"></asp:TextBox>
                            <asp:CalendarExtender ID="calExt" runat="server" TargetControlID="txtDtEmissao" Format="dd/MM/yyyy"
                                TodaysDateFormat="dd/MM/yyyy">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr align="right">
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Data Final"></asp:Label>&nbsp;&nbsp;
                            <asp:TextBox runat="server" ID="txtDtFinal" Width="100px" MaxLength="10"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDtFinal" Format="dd/MM/yyyy"
                                TodaysDateFormat="dd/MM/yyyy">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr align="right">
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Status"></asp:Label>&nbsp;&nbsp;
                            <asp:DropDownList ID="cbxStatus" runat="server" Width="100px">
                                <asp:ListItem Value="A">AMBOS</asp:ListItem>
                                <asp:ListItem Value="S">BAIXADO</asp:ListItem>
                                <asp:ListItem Value="N">ABERTO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr align="right">
                        <td>
                            <asp:Button ID="btnImprimi0" runat="server" Text="VISUALIZAR" OnClick="btnImprimi0_Click"
                                Style="height: 26px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            </td>
                    </tr>
                </table>
            </div>
            <br />
            <br />
            <%--  <div>
                <table width="100%">
                    <tr class="BordaInferior">
                        <td style="text-align: left;" class="Titulo" colspan="3">
                            Relatório
                        </td>
                    </tr>
                </table>
                <div>
                    <asp:Panel ID="Panel1" runat="server" Width="100%" ScrollBars="Vertical">
                        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" Height="50px" Width="100%"
                            EnableDatabaseLogonPrompt="False" BorderStyle="None" EnableTheming="False" GroupTreeStyle-ShowLines="False"
                            ToolPanelView="None" PageZoomFactor="95" DisplayToolbar="False" EnableParameterPrompt="False" />
                    </asp:Panel>
                </div>
            </div>--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
