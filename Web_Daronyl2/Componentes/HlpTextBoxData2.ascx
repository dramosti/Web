<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HlpTextBoxData2.ascx.cs"
    Inherits="Componentes_HlpTextBoxData2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:TextBox ID="TextBox1" runat="server" MaxLength="10"></asp:TextBox>
<asp:CalendarExtender ID="CalendarExtender1" TargetControlID="TextBox1" runat="server">
</asp:CalendarExtender>
<asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="TextBox1"
    Display="None" runat="server" ErrorMessage="&lt;b&gt;Data Inválida!&lt;b&gt;"
    ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"></asp:RegularExpressionValidator>
<asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" HighlightCssClass="highlight"
    TargetControlID="RegularExpressionValidator1">
</asp:ValidatorCalloutExtender>
