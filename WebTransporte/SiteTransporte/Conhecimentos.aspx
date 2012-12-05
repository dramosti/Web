<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Conhecimentos.aspx.cs" Inherits="Conhecimentos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style5
        {
        }
        .style13
        {
            width: 130px;
        }
        .style14
        {
            width: 130px;
            height: 24px;
        }
        .style15
        {
            height: 24px;
        }
        .style16
        {
            width: 130px;
            height: 23px;
        }
        .style17
        {
            height: 23px;
        }
        .style18
        {
            width: 130px;
            height: 19px;
        }
        .style19
        {
            height: 19px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>
                    <div align="center" style="z-index: 105; left: 600px; position: fixed; top: 300px">
                        <center>
                            <img id="imgloader" runat="server" src="imagem/jloader.gif" /><br />
                            <b>Carregando</b>
                        </center>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <table class="style1">
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Filtrar por " Font-Size="11px" Width="60px"
                            Font-Bold="true " Font-Names="Segoe UI" ForeColor="Black"></asp:Label>
                        <asp:DropDownList ID="cboCampo" runat="server" Font-Names="Segoe UI" ForeColor="Black"
                            Font-Size="11px" AutoPostBack="True" OnSelectedIndexChanged="cboCampo_SelectedIndexChanged">
                            <asp:ListItem Value="CD_NF">Nota Fiscal</asp:ListItem>
                            <asp:ListItem Value="conhec.CD_CONHECI">Cód. Conhecimento</asp:ListItem>
                            <asp:ListItem Value="conhec.DT_EMISSAO">Data Emissão Conhecimento</asp:ListItem>
                            <asp:ListItem Value="dest.nm_social">Destinatário</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;
                        <asp:DropDownList ID="cboFiltro" runat="server" Font-Names="Segoe UI" Font-Size="11px" Width="83px"
                            ForeColor="Black" OnSelectedIndexChanged="cboFiltro_SelectedIndexChanged" AutoPostBack="True">
                            <asp:ListItem Value="=">Igual a</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;
                        <asp:TextBox ID="txtFiltro1" runat="server" Font-Names="Segoe UI" Font-Size="11px"
                            ForeColor="Black"></asp:TextBox>&nbsp;
                        <asp:TextBox ID="txtFiltro2" runat="server" Font-Names="Segoe UI" Font-Size="11px"
                            ForeColor="Black" Visible="False"></asp:TextBox>&nbsp;
                        <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Segoe UI" Font-Size="11px"
                            ForeColor="Black" Text="Status" Width="39px"></asp:Label>
                        <asp:DropDownList ID="cboBaixa" runat="server" Font-Names="Segoe UI" Font-Size="11px"
                            ForeColor="Black">
                            <asp:ListItem Value="A">Ambos</asp:ListItem>
                            <asp:ListItem Value="N">Aberto</asp:ListItem>
                            <asp:ListItem Value="S">Baixado</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" Font-Bold="true " Font-Names="Segoe UI"
                            ForeColor="Black" Font-Size="11px" ValidationGroup="Pesquisar" OnClick="btnPesquisar_Click" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:RequiredFieldValidator ID="CampoObrigatorio" runat="server" ControlToValidate="txtFiltro1"
                            Display="Dynamic" ErrorMessage="Insira um parâmetro para pesquisa." ValidationGroup="Pesquisar"
                            Font-Names="Segoe UI" Font-Size="10px" Font-Bold="True" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:Label ID="lblResul" runat="server" Text="Nenhum Registro Encontrado." Visible="False"
                            Font-Size="11px" Width="221px" Font-Bold="True" Font-Names="Segoe UI" ForeColor="Black"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblTotConhec" runat="server" Font-Bold="True" 
                            Font-Names="Segoe UI" Font-Size="11px" ForeColor="#3333FF" Height="16px" 
                            Style="text-align: left" Width="360px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gridConhec" runat="server" AutoGenerateColumns="False" CellPadding="1"
                            ForeColor="#333333" GridLines="None" Width="868px" Font-Names="Segoe UI" Font-Size="11px"
                            AllowPaging="True" OnPageIndexChanging="gridConhec_PageIndexChanging" OnSelectedIndexChanged="gridConhec_SelectedIndexChanged"
                            DataKeyNames="NR_LANC">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="NR_LANC" HeaderText="NR_LANC" ReadOnly="True" SortExpression="NR_LANC"
                                    Visible="False" />
                                <asp:BoundField DataField="CD_CONHECI" HeaderText="Conhecimento" SortExpression="CD_CONHECI">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="70px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DT_EMISSAO" HeaderText="Dt_Emissão" SortExpression="DT_EMISSAO"
                                    DataFormatString="{0:dd/MM/yyyy}">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DS_DESTINATARIO" HeaderText="Destinatário" SortExpression="DS_DESTINATARIO">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DS_REDESPACHO" HeaderText="Redespacho" SortExpression="DS_REDESPACHO">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CD_RESPONS" HeaderText="Responsável" SortExpression="CD_RESPONS">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ST_CONHECI" HeaderText="Cancelado?" SortExpression="ST_CONHECI">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DT_CANCCON" HeaderText="Dt_Cancel" SortExpression="DT_CANCCON"
                                    DataFormatString="{0:dd/MM/yyyy}" Visible="False">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ST_BAIXA" HeaderText="Status" SortExpression="ST_BAIXA">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DT_BAIXA" HeaderText="Dt_Baixa" SortExpression="DT_BAIXA"
                                    DataFormatString="{0:dd/MM/yyyy}" Visible="False">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:CommandField SelectText="Detalhes" ShowSelectButton="True">
                                    <ItemStyle Width="40px" />
                                </asp:CommandField>
                            </Columns>
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td class="style5">
                        &nbsp;<asp:Label ID="lblTotNF" runat="server" Font-Bold="True" 
                            Font-Names="Segoe UI" Font-Size="11px" ForeColor="#3333FF" Height="16px" 
                            Style="text-align: left" Width="360px"></asp:Label>
&nbsp;<table class="style1">
                            <tr>
                                <td>
                                    <table align="left">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gridNotas" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                    CellPadding="1" Font-Names="Segoe UI" Font-Size="11px" ForeColor="#333333" GridLines="None"
                                                    OnPageIndexChanging="gridNotas_PageIndexChanging">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="CD_NF" HeaderText="Nota Fiscal">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CD_CONHECI" HeaderText="Conhecimento">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="VL_NF" DataFormatString="{0:c}" HeaderText="Valor">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Width="80px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DT_EMI" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Data Emissão">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Width="80px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ST_BAIXA" HeaderText="Status">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <EditRowStyle BackColor="#999999" />
                                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <table align="right">
                                        <tr>
                                            <td class="style18">
                                                <asp:Label ID="lblEndereco" runat="server" Font-Bold="True" Font-Names="Segoe UI"
                                                    Font-Size="11px" ForeColor="Black" Height="29px" Style="text-align: right" Text="Endereço de Entrega:"
                                                    Width="116px"></asp:Label>
                                            </td>
                                            <td class="style19">
                                                <asp:Label ID="lbldaoEndereco" runat="server" Font-Bold="True" Font-Names="Segoe UI"
                                                    Font-Size="11px" ForeColor="Red" Height="32px" Style="margin-left: 0px" Text="Endereço de Entrega:"
                                                    Width="340px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style16">
                                                <asp:Label ID="lbldtCanc" runat="server" Font-Bold="True" Font-Names="Segoe UI" 
                                                    Font-Size="11px" ForeColor="Black" Style="text-align: right" 
                                                    Text="Data Cancelamento:" Width="117px"></asp:Label>
                                            </td>
                                            <td class="style17">
                                                <asp:Label ID="lbldaodtCanc" runat="server" Font-Bold="True" 
                                                    Font-Names="Segoe UI" Font-Size="11px" ForeColor="Red" Text="data cancelamento" 
                                                    Width="334px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style13">
                                                <asp:Label ID="lbldtbaixa" runat="server" Font-Bold="True" 
                                                    Font-Names="Segoe UI" Font-Size="11px" ForeColor="Black" 
                                                    Style="text-align: right" Text="Data Baixa:" Width="117px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbldaoDtBaixa" runat="server" Font-Bold="True" 
                                                    Font-Names="Segoe UI" Font-Size="11px" ForeColor="Red" Text="data coleta" 
                                                    Width="334px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style13">
                                                <asp:Label ID="lblColeta" runat="server" Font-Bold="True" Font-Names="Segoe UI" Font-Size="11px"
                                                    ForeColor="Black" Style="text-align: right" Text="Coleta:" Width="117px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbldaoColeta" runat="server" Font-Bold="True" Font-Names="Segoe UI"
                                                    Font-Size="11px" ForeColor="Red" Text="Coleta:" Width="334px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblOcorrencia" runat="server" Font-Bold="True" Font-Names="Segoe UI"
                                                    Font-Size="11px" ForeColor="Blue" Style="text-align: right" Text="Ocorrência:"
                                                    Width="117px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbldaoOcorrencia" runat="server" Font-Bold="True" Font-Names="Segoe UI"
                                                    Font-Size="11px" ForeColor="Red" Text="Ocorrência:" Width="336px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style13">
                                                <asp:Label ID="lblUfColeta" runat="server" Font-Bold="True" Font-Names="Segoe UI"
                                                    Font-Size="11px" ForeColor="Black" Style="text-align: right" Text="Uf Coleta:"
                                                    Width="118px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbldaoUfColeta" runat="server" Font-Bold="True" Font-Names="Segoe UI"
                                                    Font-Size="11px" ForeColor="Red" Text="Uf Coleta:" Width="55px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style13">
                                                <asp:Label ID="lblCfop" runat="server" Font-Bold="True" Font-Names="Segoe UI" Font-Size="11px"
                                                    ForeColor="Black" Style="text-align: right" Text="CFOP:" Width="118px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbldaoCfop" runat="server" Font-Bold="True" Font-Names="Segoe UI"
                                                    Font-Size="11px" ForeColor="Red" Text="CFOP:" Width="55px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style13">
                                                <asp:Label ID="lblMotorista" runat="server" Font-Bold="True" Font-Names="Segoe UI"
                                                    Font-Size="11px" ForeColor="Black" Style="text-align: right" Text="Motorista Entrega:"
                                                    Width="118px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbldaoMotorista" runat="server" Font-Bold="True" Font-Names="Segoe UI"
                                                    Font-Size="11px" ForeColor="Red" Height="16px" Text="Motorista Entrega:" Width="336px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style13">
                                                <asp:Label ID="lblPlaca" runat="server" Font-Bold="True" Font-Names="Segoe UI" Font-Size="11px"
                                                    ForeColor="Black" Style="text-align: right" Text="Placa Veículo:" Width="118px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbldaoPlaca" runat="server" Font-Bold="True" Font-Names="Segoe UI"
                                                    Font-Size="11px" ForeColor="Red" Height="16px" Text="Placa Veículo:" Width="218px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style13">
                                                <asp:Label ID="lblModelo" runat="server" Font-Bold="True" Font-Names="Segoe UI" Font-Size="11px"
                                                    ForeColor="Black" Style="text-align: right" Text="Modelo:" Width="118px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbldaoModelo" runat="server" Font-Bold="True" Font-Names="Segoe UI"
                                                    Font-Size="11px" ForeColor="Red" Text="Modelo:" Width="215px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTexto" runat="server" Font-Bold="True" Font-Names="Segoe UI" Font-Size="11px"
                                                    ForeColor="Black" Style="text-align: right" Text="Obs:" Width="117px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbldaoTexto" runat="server" Font-Bold="True" Font-Names="Segoe UI"
                                                    Font-Size="11px" ForeColor="Red" Text="Obs:" Width="329px"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="style5">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
