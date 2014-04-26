<%@ Page Title="Log In" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeFile="Login.aspx.cs" Inherits="Account_Login" %>

<%@ Register Assembly="HLP.Web.Controles" Namespace="HLP.Web.Controles" TagPrefix="cc1" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style3
        {
            width: 399px;
        }
        .style5
        {
        }
        .style6
        {
            width: 224px;
        }
        .style7
        {
            width: 77px;
        }
        .BordaInferior td
        {
            border-bottom: 2px solid black;
        }
        .style8
        {
            height: 30px;
        }
        .style9
        {
            width: 70%;
            height: 30px;
        }
        .style10
        {
            height: 30px;
            width: 82px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <table width="100%">
        <tr width="100%">
            <td colspan="2">
            </td>
        </tr>
        <tr class="BordaInferior">
            <td class="style5" colspan="4">
                <asp:Label ID="lblAcesso" runat="server" CssClass="Titulo">Login</asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center" class="style10">
                <asp:Label ID="Label1" runat="server" CssClass="label" Text="Empresa"  ></asp:Label>
         
            </td>
            <td align="left" class="style8">
                      <asp:DropDownList ID="cboEmpresa" runat="server" CssClass="textBox" DataTextField="NM_GUERRA"
                    DataValueField="CD_EMPRESA" Width="200px" AutoPostBack="True">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="cboEmpresa"
                    ErrorMessage="Nome Usuário é Obrigatório" ToolTip="Nome Usuário é Obrigatório"
                    ValidationGroup="ControleLogin">*</asp:RequiredFieldValidator>
            </td>
            <td align="left" class="style9">
                &nbsp;
            </td>
        </tr>
        <td class="style5" colspan="2">
            &nbsp;
            <cc1:HlpWebLogin ID="ControleLogin" runat="server" DestinationPageUrl="Home.aspx"
                Width="100%" Font-Names="Segoe UI" Font-Size="10px" OnAuthenticate="HlpWebLogin1_Authenticate"
                FailureText="Sua tentativa de acesso não foi bem sucedida. Por Favor, tente novamente."
                PasswordLabelText="Senha" PasswordRequiredErrorMessage="Senha é Obrigatório"
                RememberMeText="Lembrar-me da próxima vez" UserNameLabelText="Usuário" UserNameRequiredErrorMessage="Nome Usuário é Obrigatório">
                <LayoutTemplate>
                    <table width="100%">
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td align="center" class="style8">
                                            <asp:Label ID="lblLogin" runat="server" CssClass="label" Text="Usuário"></asp:Label>
                                        </td>
                                        <td class="style9" style="text-align: left;">
                                            <asp:TextBox ID="UserName" runat="server" CssClass="textBox">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                ErrorMessage="Nome Usuário é Obrigatório" ToolTip="Nome Usuário é Obrigatório"
                                                ValidationGroup="ControleLogin">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="style7">
                                            <asp:Label ID="lblSenha" runat="server" CssClass="label" Text="Senha"></asp:Label>
                                        </td>
                                        <td class="style6">
                                            <asp:TextBox ID="Password" CssClass="textBox" runat="server" TextMode="Password"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                ErrorMessage="Senha é Obrigatório" ToolTip="Senha é Obrigatório" ValidationGroup="ControleLogin">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2" style="color: Red;">
                                            <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <br />
                                            <asp:Button ID="LoginButton" runat="server" CssClass="button" CommandName="Login"
                                                Text="Entrar" ValidationGroup="ControleLogin" Width="108px" OnClick="LoginButton_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </LayoutTemplate>
            </cc1:HlpWebLogin>
        </td>
        <td class="style3" align="center">
            &nbsp;
        </td>
        <td>
            &nbsp;
        </td>
        </tr>
    </table>
    <p>
        &nbsp;<div style="color: Black">
        </div>
    </p>
</asp:Content>
