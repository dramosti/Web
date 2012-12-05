<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Account_Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            height: 14px;
            width: 217px;
        }
        .style2
        {
            height: 29px;
        }
        .highlight
        {
            background-color: lemonchiffon;
        }
    </style>
</head>
<body background="../imagem/background.jpg">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <div align="center">
        <asp:Label ID="lblErro" runat="server" Font-Names="Verdana" Font-Size="10px" ForeColor="White"
            Text=""></asp:Label>
        <asp:Login ID="LoginUser" runat="server" BackColor="#ffffff" BorderColor="#E6E2D8"
            BorderPadding="4" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana"
            Font-Size="0.8em" ForeColor="#333333" Height="132px" Width="392px" FailureText="">
            <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
            <LayoutTemplate>
                <table class="style1" background="../imagem/fundoLogin.png">
                    <tr>
                        <td>
                            <table class="style1" style="width: 392px">
                                <tr>
                                    <td align="center" colspan="2" style="color: White; background-color: #5D7B9D; font-size: 0.9em;
                                        font-weight: bold;" class="style1">
                                        Log In
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="style2">
                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" Font-Bold="True"
                                            Font-Names="Segoe UI" Font-Size="11px" ForeColor="White">CNPJ:</asp:Label>
                                    </td>
                                    <td class="style2">
                                        <asp:TextBox ID="UserName" runat="server" Font-Names="Segoe UI" Font-Size="11px"
                                            TabIndex="1" ValidationGroup="LoginUser" Style="margin-left: 0px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" Font-Bold="true"
                                            Font-Names="Segoe UI" Font-Size="11px" ForeColor="White">Senha:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Password" runat="server" Font-Names="Segoe UI" Font-Size="11px"
                                            TextMode="Password" TabIndex="2"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                            Display="Dynamic" ErrorMessage="Usuário é Obrigatório!" ValidationGroup="LoginUser"
                                            Font-Names="Segoe UI" Font-Size="10px" Font-Bold="True" ForeColor="Red"></asp:RequiredFieldValidator>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2" style="color: Red;">
                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                            ErrorMessage="Senha é Obrigatória!" Display="Dynamic" ValidationGroup="LoginUser"
                                            Font-Names="Segoe UI" Font-Size="10px" Font-Bold="True" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2" style="color: Red;">
                                        <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ControlToValidate="UserName"
                                            Display="Dynamic" ErrorMessage="Cnpj Inválido! Utilize o formato xx.xxx.xxx/xxxx-xx" ValidationGroup="LoginUser"
                                            Font-Names="Segoe UI" Font-Size="10px" Font-Bold="True" ForeColor="Red" 
                                            ValidationExpression="(^\d{2}\.\d{3}\.\d{3}\/\d{4}\-\d{2}$)"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        <asp:ImageButton ID="Login" runat="server" ImageUrl="~/imagem/login.png" OnClick="Login_Click"
                                            Style="height: 48px" ToolTip="Entrar" ValidationGroup="LoginUser" />
                                    </td>
                                </tr>
                    </tr>
                </table>
                </td> </tr> </table>
            </LayoutTemplate>
            <LoginButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px"
                Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284775" />
            <TextBoxStyle Font-Size="0.8em" />
            <TitleTextStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="0.9em" ForeColor="White" />
        </asp:Login>
    </div>
    </form>
</body>
</html>
