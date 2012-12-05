<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CadastroRelatorioVisita.aspx.cs" Inherits="CadastroRelatorioVisita" Title="Cadastro Visita" %>

<%@ Register Assembly="Microsoft.Web.Atlas" Namespace="Microsoft.Web.UI" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" 
    runat="server" EnableScriptGlobalization="True" 
    EnableScriptLocalization="True" CombineScripts="True">
</ajaxToolkit:ToolkitScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline">
    <ContentTemplate>
<TABLE style="WIDTH: 100%" align=center><TBODY><TR><TD align=center colSpan=3><asp:Label id="Label16" runat="server" ForeColor="#0066CC" Text="Lançamento de Visita" Font-Size="Large" Font-Names="Verdana" Font-Bold="True" __designer:wfdid="w149"></asp:Label> </TD></TR><TR><TD align=center colSpan=3><asp:Label id="Label15" runat="server" Font-Size="10px" Font-Names="Verdana" __designer:wfdid="w150"></asp:Label> </TD></TR><TR><TD align=center colSpan=3>&nbsp;</TD></TR><TR><TD style="WIDTH: 35%" align=right><asp:Label id="Label2" runat="server" Text="Data do Relatório" Font-Size="X-Small" Font-Names="Verdana" __designer:wfdid="w151"></asp:Label> </TD><TD colSpan=2><asp:TextBox id="txtDataCadastro" runat="server" Width="103px" Font-Size="10px" Font-Names="Verdana" __designer:wfdid="w152"></asp:TextBox> <asp:ImageButton id="BtnData" runat="server" ImageUrl="~/Imagem/BtnCalendario.jpg" __designer:wfdid="w153"></asp:ImageButton> <ajaxToolkit:MaskedEditExtender id="MaskedEditExtender1" runat="server" UserDateFormat="DayMonthYear" TargetControlID="txtDataCadastro" Mask="99/99/9999" ClearMaskOnLostFocus="False" __designer:wfdid="w154">
                    </ajaxToolkit:MaskedEditExtender> <ajaxToolkit:CalendarExtender id="CalendarExtender1" runat="server" TargetControlID="txtDataCadastro" Format="dd/MM/yyyy" PopupButtonID="BtnData" __designer:wfdid="w155">
                    </ajaxToolkit:CalendarExtender> </TD></TR><TR><TD style="WIDTH: 35%" align=right><asp:Label id="Label3" runat="server" Text="Contato" Font-Size="X-Small" Font-Names="Verdana" __designer:wfdid="w156"></asp:Label> </TD><TD colSpan=2><asp:TextBox id="txtContato" runat="server" Width="274px" Font-Size="10px" Font-Names="Verdana" MaxLength="15" __designer:wfdid="w157"></asp:TextBox> </TD></TR><TR><TD style="WIDTH: 35%" align=right><asp:Label id="Label8" runat="server" Text="Cliente Cadastrado?" Font-Size="X-Small" Font-Names="Verdana" __designer:wfdid="w158"></asp:Label> </TD><TD colSpan=2><asp:RadioButtonList id="RadioButtonList1" runat="server" Width="88px" Font-Size="10px" Font-Names="Verdana" RepeatLayout="Flow" RepeatDirection="Horizontal" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" AutoPostBack="True" __designer:wfdid="w159">
                        <asp:ListItem Selected="True">SIM</asp:ListItem>
                        <asp:ListItem>NÃO</asp:ListItem>
                    </asp:RadioButtonList> </TD></TR><TR><TD style="WIDTH: 35%" align=right><asp:Label id="Label4" runat="server" Text="Cliente" Font-Size="X-Small" Font-Names="Verdana" __designer:wfdid="w160"></asp:Label> </TD><TD colSpan=2><asp:DropDownList id="DDLCliente" runat="server" Width="280px" Font-Size="10px" Font-Names="Verdana" AutoPostBack="True" OnTextChanged="DDLCliente_TextChanged" DataValueField="Id" DataTextField="Nome" __designer:wfdid="w161">
                    </asp:DropDownList> <BR /><asp:TextBox id="txtCliente" runat="server" Width="272px" Font-Size="10px" Font-Names="Verdana" MaxLength="30" Visible="False" __designer:wfdid="w162"></asp:TextBox> </TD></TR><TR><TD style="WIDTH: 35%" align=right><asp:Label id="lblTecnico" runat="server" Text="Técnico Responsável" Font-Size="X-Small" Font-Names="Verdana" __designer:wfdid="w163"></asp:Label> </TD><TD colSpan=2><asp:DropDownList id="DDLTecnico" runat="server" Width="280px" Font-Size="10px" Font-Names="Verdana" __designer:wfdid="w164">
                    </asp:DropDownList> </TD></TR><TR><TD style="WIDTH: 35%" align=right><asp:Label id="Label6" runat="server" Text="Sistema" Font-Size="X-Small" Font-Names="Verdana" __designer:wfdid="w165"></asp:Label> </TD><TD colSpan=2><asp:DropDownList id="DDLSistema" runat="server" Width="280px" Font-Size="10px" Font-Names="Verdana" __designer:wfdid="w166">
                    </asp:DropDownList> </TD></TR><TR><TD style="WIDTH: 35%" align=right><asp:Label id="Label7" runat="server" Text="Tipo do Relatório" Font-Size="X-Small" Font-Names="Verdana" __designer:wfdid="w167"></asp:Label> </TD><TD colSpan=2><asp:DropDownList id="DDLTipoRelatoio" runat="server" Width="280px" Font-Size="10px" Font-Names="Verdana" OnSelectedIndexChanged="DDLTipoRelatoio_SelectedIndexChanged" AutoPostBack="True" OnTextChanged="DDLTipoRelatoio_TextChanged" __designer:wfdid="w168">
                        <asp:ListItem>HARDWARE</asp:ListItem>
                        <asp:ListItem>SOFTWARE</asp:ListItem>
                    </asp:DropDownList> </TD></TR><TR><TD style="WIDTH: 35%" align=right>&nbsp;</TD><TD colSpan=2><asp:DropDownList id="DDLTituloRel" runat="server" Width="170px" Font-Size="10px" Font-Names="Verdana" OnSelectedIndexChanged="DDLTipoRelatoio_SelectedIndexChanged" __designer:wfdid="w169">
                    </asp:DropDownList> </TD></TR><TR><TD style="WIDTH: 35%" align=right>&nbsp;</TD><TD colSpan=2><asp:DropDownList id="DDLVncLocal" runat="server" Width="75px" Font-Size="10px" Font-Names="Verdana" __designer:wfdid="w170">
                        <asp:ListItem>LOCAL</asp:ListItem>
                        <asp:ListItem>VNC</asp:ListItem>
                    </asp:DropDownList> </TD></TR><TR><TD style="WIDTH: 35%" align=right><asp:Label id="lblFaturar" runat="server" Text="Faturar?" Font-Size="X-Small" Font-Names="Verdana" __designer:wfdid="w171"></asp:Label> </TD><TD colSpan=2><asp:DropDownList id="DDLFaturar" runat="server" Font-Size="10px" Font-Names="Verdana" __designer:wfdid="w172">
                        <asp:ListItem>SIM</asp:ListItem>
                        <asp:ListItem>NÃO</asp:ListItem>
                    </asp:DropDownList> </TD></TR><TR><TD style="WIDTH: 35%" align=right><asp:Label id="Label9" runat="server" Text="Horário de Inicio" Font-Size="X-Small" Font-Names="Verdana" __designer:wfdid="w173"></asp:Label> </TD><TD colSpan=2><asp:TextBox id="txtHoraInicio" runat="server" Width="50px" Font-Size="10px" Font-Names="Verdana" AutoPostBack="True" OnTextChanged="txtHoraInicio_TextChanged" __designer:wfdid="w174"></asp:TextBox> <ajaxToolkit:MaskedEditExtender id="MaskedEditExtender2" runat="server" TargetControlID="txtHoraInicio" Mask="99:99" ClearMaskOnLostFocus="False" MaskType="Time" __designer:wfdid="w175">
                    </ajaxToolkit:MaskedEditExtender> </TD></TR><TR><TD style="WIDTH: 35%" align=right><asp:Label id="Label10" runat="server" Text="Horário de Término" Font-Size="X-Small" Font-Names="Verdana" __designer:wfdid="w176"></asp:Label> </TD><TD colSpan=2><asp:TextBox id="txtHoraFinal" runat="server" Width="50px" Font-Size="10px" Font-Names="Verdana" MaxLength="5" AutoPostBack="True" OnTextChanged="txtHoraFinal_TextChanged" __designer:wfdid="w177"></asp:TextBox> <asp:Label id="lblErroHora" runat="server" ForeColor="Red" Font-Size="X-Small" Font-Names="Verdana" Visible="False" __designer:wfdid="w178"></asp:Label> <ajaxToolkit:MaskedEditExtender id="MaskedEditExtender3" runat="server" TargetControlID="txtHoraFinal" Mask="99:99" ClearMaskOnLostFocus="False" MaskType="Time" __designer:wfdid="w179">
                    </ajaxToolkit:MaskedEditExtender> </TD></TR><TR><TD style="WIDTH: 35%" align=right><asp:Label id="Label11" runat="server" Text="Horas Descontadas" Font-Size="X-Small" Font-Names="Verdana" __designer:wfdid="w180"></asp:Label> </TD><TD colSpan=2><asp:TextBox id="txtHoraDesc" runat="server" Width="50px" Font-Size="10px" Font-Names="Verdana" MaxLength="5" AutoPostBack="True" OnTextChanged="txtHoraDesc_TextChanged" __designer:wfdid="w181"></asp:TextBox> <ajaxToolkit:MaskedEditExtender id="MaskedEditExtender4" runat="server" TargetControlID="txtHoraDesc" Mask="99:99" ClearMaskOnLostFocus="False" MaskType="Time" __designer:wfdid="w182">
                    </ajaxToolkit:MaskedEditExtender> </TD></TR><TR><TD style="WIDTH: 35%" align=right><asp:Label id="Label12" runat="server" Text="Total de Horas" Font-Size="X-Small" Font-Names="Verdana" __designer:wfdid="w183"></asp:Label> </TD><TD style="WIDTH: 5%"><asp:Label style="MARGIN-BOTTOM: 0px" id="lblHoraTotal" runat="server" Width="85px" Font-Size="Small" Font-Names="Verdana" Font-Bold="True" __designer:wfdid="w184"></asp:Label> </TD><TD style="WIDTH: 60%" align=left><asp:Button style="MARGIN-BOTTOM: 0px" id="btnAtualizaHora" onclick="Button1_Click" runat="server" ForeColor="Black" Width="110px" Text="Atualizar Horas" Font-Size="X-Small" Font-Names="Verdana" Font-Underline="False" BorderWidth="1px" BorderColor="Black" BorderStyle="Dotted" BackColor="White" __designer:wfdid="w185"></asp:Button> </TD></TR><TR>
    <TD align=right style="WIDTH: 35%" valign="top">
        <asp:Label ID="lblEstMaqui" runat="server" __designer:wfdid="w183" 
            Font-Names="Verdana" Font-Size="X-Small" Text="Estado da Máquina"></asp:Label>
    </TD>
    <td style="WIDTH: 5%">
        <asp:RadioButtonList ID="rdblEstMaqui" runat="server" AutoPostBack="True" 
            Font-Names="Verdana" Font-Size="X-Small" RepeatLayout="Flow" Width="238px">
            <asp:ListItem Selected="True" Value="1">Máquina em Perfeito Estado</asp:ListItem>
            <asp:ListItem Value="2">Máquina com Problema</asp:ListItem>
        </asp:RadioButtonList>
    </td>
    <td align="left" style="WIDTH: 60%">
        &nbsp;</td>
    </TR>
    <tr>
        <td align="right" style="WIDTH: 35%" valign="top">
            <asp:Label ID="lblDescMaqui" runat="server" __designer:wfdid="w183" 
                Font-Names="Verdana" Font-Size="X-Small" Text="Descrição da Máquina"></asp:Label>
        </td>
        <td style="WIDTH: 5%">
            <asp:TextBox ID="txtDesMaqui" runat="server" Font-Names="Verdana" 
                Font-Size="X-Small" Height="108px" TextMode="MultiLine" Width="238px"></asp:TextBox>
        </td>
        <td align="left" style="WIDTH: 60%">
            &nbsp;</td>
    </tr>
    <TR><TD align=left colSpan=3>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
        <asp:Label ID="Label13" runat="server" __designer:wfdid="w186" 
            Font-Names="Verdana" Font-Size="X-Small" 
            Text="Descrição das Atividades Prestadas"></asp:Label>
        </TD></TR><TR><TD align=left colspan="3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="txtMemo" runat="server" __designer:wfdid="w187" 
            Font-Names="Verdana" Font-Size="10px" Height="185px" TextMode="MultiLine" 
            Width="484px"></asp:TextBox>
        </TD></TR><TR><TD style="WIDTH: 35%" align=right>&nbsp;</TD><TD colspan="2">&nbsp;</TD></TR><TR>
    <TD style="WIDTH: 35%" align="right" rowspan="2">
        <asp:Label ID="Label5" runat="server" __designer:wfdid="w188" 
            Font-Names="Verdana" Font-Size="X-Small" Text="Ordem de Serviço"></asp:Label>
    </TD><TD style="WIDTH: 30%">
        <asp:TextBox ID="txtOs" runat="server" __designer:wfdid="w189" 
            AutoPostBack="True" Font-Names="Verdana" Font-Size="10px" 
            OnTextChanged="txtOs_TextChanged" Width="139px"></asp:TextBox>
    </TD>
    <td>
        <asp:Button ID="btnIncluiLista" runat="server" __designer:wfdid="w190" 
            Font-Names="Verdana" Font-Size="10px" onclick="btnIncluiLista_Click" 
            Text="Incluir na Lista" Width="101px" />
    </td>
    </TR><TR><TD style="WIDTH: 30%" valign="top">
        <asp:ListBox ID="LtbOs" runat="server" __designer:wfdid="w191" 
            Font-Names="Verdana" Font-Size="10px" Height="169px" Width="145px">
        </asp:ListBox>
        </TD><TD valign="top">
            <asp:Button ID="Button1" runat="server" __designer:wfdid="w192" 
                Font-Names="Verdana" Font-Size="10px" Height="19px" onclick="Button1_Click1" 
                Text="Excluir da Lista" Width="101px" />
        </TD></TR><TR><TD align=right style="WIDTH: 35%">&nbsp;</TD>
        <td colspan="2">
            &nbsp;</td>
    </TR><TR><TD align=center colspan="3">
        <asp:Label ID="lblErro" runat="server" __designer:wfdid="w193" Font-Bold="True" 
            Font-Names="Verdana" Font-Size="X-Small" ForeColor="Red"></asp:Label>
        </TD></TR><TR><TD align=right style="WIDTH: 35%">&nbsp;</TD>
        <td colspan="2">
            &nbsp;</td>
    </TR><TR><TD align=center colSpan=3>
        <asp:Button ID="btnGravar" runat="server" __designer:wfdid="w194" 
            Font-Names="Verdana" Font-Size="10px" onclick="btnGravar_Click" Text="Gravar" 
            Width="90px" />
        <asp:Button ID="btnVoltar" runat="server" __designer:wfdid="w195" 
            Font-Names="Verdana" Font-Size="10px" onclick="btnVoltar_Click" Text="Voltar" 
            Width="90px" />
        </TD></TR>
    <tr>
        <td align="center" colspan="3">
            &nbsp;</td>
    </tr>
    </TBODY></TABLE>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

