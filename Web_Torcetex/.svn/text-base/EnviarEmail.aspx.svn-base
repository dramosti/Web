<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="EnviarEmail.aspx.cs" Inherits="EnviarEmail" %>

<%@ Register Assembly="HLP.Web.Controles" Namespace="HLP.Web.Controles" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 90px;
        }
        .style3
        {
            width: 92px;
        }
        .style6
        {
            font-size: 14px;
            font-family: Segoe UI;
            color: Black;
            width: 61px;
        }
        .style7
        {
            width: 61px;
        }
        .style8
        {
            width: 394px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div align="center">
        <asp:MultiView ID="MultViewEmail" runat="server" ActiveViewIndex="0">
            <asp:View ID="viewEmail" runat="server">
                <table style="width: 100%">
                    <tr class="BordaInferior">
                        <td style="text-align: left; color: Black" class="Titulo" colspan="3">
                            Email
                        </td>
                    </tr>
                    <tr>
                        <td class="style6" align="right">
                            Remetente
                        </td>
                        <td align="left" colspan="2">
                            <asp:Label ID="lblRemetente" runat="server" CssClass="label" Width="353px"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #000000">
                        <td class="style6" align="right">
                            Destinatário
                        </td>
                        <td align="left" colspan="2">
                            <asp:TextBox ID="txtDestino" runat="server" CssClass="textBox" Width="346px"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtDestino"
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style6" align="right">
                            Anexo
                        </td>
                        <td align="left" colspan="2">
                            <asp:Label ID="lblNmArquivo" Font-Names="Segoe UI" Font-Size="8pt" Font-Bold="true"
                                ForeColor="Blue" runat="server" Text="Label"></asp:Label>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style6" align="right">
                            Assunto
                        </td>
                        <td align="left" align="left" colspan="2">
                            <asp:TextBox ID="txtTitulo" runat="server" CssClass="textBox" MaxLength="20" Width="347px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style6" align="right">
                            Observação
                        </td>
                        <td style="width: 248px">
                            <table>
                                <tr>
                                    <td align="left" colspan="2">
                                        <asp:Panel ID="panelBody" runat="server">
                                            <asp:TextBox ID="txtCorpo" runat="server" CssClass="textBox" Height="143px" TextMode="MultiLine"
                                                Width="550px"></asp:TextBox>
                                        </asp:Panel>
                                        <asp:HtmlEditorExtender ID="txtCorpo_HtmlEditorExtender" runat="server" Enabled="True"
                                            TargetControlID="txtCorpo">
                                            <Toolbar>
                                                <asp:Undo />
                                                <asp:Redo />
                                                <asp:Bold />
                                                <asp:Italic />
                                                <asp:Underline />
                                                <asp:StrikeThrough />
                                                <asp:Subscript />
                                                <asp:Superscript />
                                                <asp:JustifyLeft />
                                                <asp:JustifyCenter />
                                                <asp:JustifyRight />
                                                <asp:JustifyFull />
                                                <asp:InsertOrderedList />
                                                <asp:InsertUnorderedList />
                                                <asp:CreateLink />
                                                <asp:UnLink />
                                                <asp:RemoveFormat />
                                                <asp:SelectAll />
                                                <asp:UnSelect />
                                                <asp:Delete />
                                                <asp:Cut />
                                                <asp:Copy />
                                                <asp:Paste />
                                                <asp:BackgroundColorSelector />
                                                <asp:ForeColorSelector />
                                                <asp:FontNameSelector />
                                                <asp:FontSizeSelector />
                                                <asp:Indent />
                                                <asp:Outdent />
                                                <asp:InsertHorizontalRule />
                                                <asp:HorizontalSeparator />
                                            </Toolbar>
                                        </asp:HtmlEditorExtender>
                                    </td>
                                    <td align="left" colspan="2">
                                        <asp:Button ID="btnEnviar" runat="server" CssClass="button" OnClick="btnEnviar_Click"
                                            Text="Enviar" Width="82px" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <tr>
                            <td class="style7">
                                &nbsp;
                            </td>
                            <td style="width: 77px">
                                &nbsp;
                            </td>
                        </tr>
                    </tr>
                </table>
            </asp:View>
             <asp:View ID="viewInformacao" runat="server">
                <table style="width: 100%">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblInfo" runat="server" CssClass="label"></asp:Label>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnVoltar" runat="server" CssClass="button" Text="Voltar" Width="82px"
                                OnClick="btnVoltar_Click" />
                        </td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
