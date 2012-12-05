<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ResultadoConsultaVisita.aspx.cs" Inherits="ResultadoConsultaVisita" Title="Visitas Filtradas" %>

<%@ Register Assembly="Microsoft.Web.Atlas" Namespace="Microsoft.Web.UI" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <cc1:ScriptManager ID="ScriptManager2" runat="server" EnablePartialRendering="True">
    </cc1:ScriptManager>
    &nbsp;&nbsp;
    <cc1:UpdatePanel ID="UpdatePanel1" runat="server"><ContentTemplate>
<TABLE style="WIDTH: 100%"><TBODY><TR><TD style="WIDTH: 100px"></TD>
    <TD style="WIDTH: 379px"></TD><TD style="WIDTH: 113px"></TD></TR><TR><TD style="WIDTH: 100px"></TD>
    <TD style="WIDTH: 379px" align=center><asp:Label id="Label1" runat="server" 
            Text="Visita(s) Filtrada(s)" Font-Size="Large" Font-Names="Verdana" 
            Font-Bold="True" ForeColor="#0066CC"></asp:Label></TD><TD style="WIDTH: 113px"></TD></TR><TR><TD style="WIDTH: 100px"></TD>
    <TD style="WIDTH: 379px"></TD><TD style="WIDTH: 113px"></TD></TR>
    <tr>
        <td style="WIDTH: 100px">
            &nbsp;</td>
        <td style="WIDTH: 379px">
            &nbsp;</td>
        <td style="WIDTH: 113px">
            &nbsp;</td>
    </tr>
    <tr>
        <td align="center" colspan="3">
            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                AutoGenerateColumns="False" CellPadding="4" Font-Names="Verdana" 
                Font-Size="10px" ForeColor="#333333" GridLines="Horizontal" 
                OnPageIndexChanging="GridView1_PageIndexChanging" 
                OnSelectedIndexChanged="GridView1_SelectedIndexChanged" Width="495px" 
                BorderStyle="None" BorderWidth="0px" PageSize="20">
                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" 
                    BorderStyle="None" />
                <Columns>
                    <asp:CommandField SelectText="Selecionar" ShowSelectButton="True" />
                    <asp:BoundField DataField="NR_LANC" HeaderText="Relatório">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DT_REL" HeaderText="Data">
                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="HR_ENTRADA" HeaderText="Hr Entrada">
                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="HR_SAIDA" HeaderText="Hr Saída">
                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NM_GUERRA" HeaderText="Cliente">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NM_OPERADO" HeaderText="Técnico">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundField>
                </Columns>
                <RowStyle BackColor="#ECF5FF" ForeColor="#333333" Wrap="False" 
                    BorderStyle="None" />
                <SelectedRowStyle BackColor="#62B0FF" Font-Bold="True" ForeColor="White" 
                    BorderStyle="None" />
                <PagerStyle BackColor="White" ForeColor="#333333" HorizontalAlign="Center" 
                    BorderStyle="None" />
                <HeaderStyle BackColor="#0066CC" Font-Bold="True" Font-Names="Verdana" 
                    Font-Size="12px" ForeColor="White" Wrap="False" BorderStyle="None" />
                <AlternatingRowStyle BackColor="White" BorderStyle="None" />
            </asp:GridView>
        </td>
    </tr>
    <tr>
        <td align="right" colspan="3">
            <asp:Button ID="btnImp" runat="server" Font-Names="Verdana" Font-Size="10px" 
                onclick="btnImp_Click" Text="Visualizar" Width="90px" />
            <asp:Button ID="Button3" runat="server" Font-Names="Verdana" Font-Size="10px" 
                onclick="Button3_Click" Text="Excluir" Width="90px" />
            <asp:Button ID="Button1" runat="server" Font-Names="Verdana" Font-Size="10px" 
                onclick="Button1_Click" Text="Incluir" Width="90px" />
        </td>
    </tr>
    </TBODY></TABLE> <TABLE style="WIDTH: 100%"><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 8px" align=right>
            &nbsp;</TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 22px" align=left>&nbsp;</TD></TR><TR><TD style="WIDTH: 100px" align=right></TD></TR></TBODY></TABLE>
</ContentTemplate>
</cc1:UpdatePanel>
</asp:Content>

