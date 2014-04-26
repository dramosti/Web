<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Pedido.aspx.cs" Inherits="Pedido" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="HLP.Web.Controles" Namespace="HLP.Web.Controles" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 75px;
        }
        .style2
        {
            width: 139px;
        }
        .button
        {
        }
        .style3
        {
            font-size: 14px;
            font-family: Segoe UI;
            color: Black;
            width: 309px;
        }
        .style4
        {
            width: 309px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div align="center">
        <asp:MultiView ID="MultiViewItensPed" runat="server" ActiveViewIndex="0">
            <asp:View ID="ViewDadosPedido" runat="server">
                <table style="width: 100%;">
                    <tr class="BordaInferior">
                        <td style="text-align: left; color: Black" class="Titulo" colspan="3">Incluir Pedido
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="label">Data do Pedido
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtDataPedido" runat="server" CssClass="textBox" Enabled="False"
                                Width="103px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="label">Cliente:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCodCli" runat="server" CssClass="textBox" Enabled="True" Width="101px"></asp:TextBox>
                            <asp:TextBox ID="txtCliente" runat="server" CssClass="textBox" Enabled="False" Width="286px"></asp:TextBox>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnCliente" runat="server" CssClass="button" Height="26px" Text="Buscar Cliente"
                                OnClick="btnCliente_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnPesqCliente" runat="server" CssClass="button" Height="26px" Text="Filtrar Pesquisar"
                                OnClick="btnPesqCliente_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="label">Tipo de Documento:
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cbxDS_TPDOCWEB" runat="server" AutoPostBack="True" CssClass="textBox"
                                DataTextField="DS_TPDOC" DataValueField="CD_TPDOC" Enabled="true" 
                                Width="268px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="label">Pedido Cliente:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtPediCli" MaxLength="15" runat="server" CssClass="textBox" Width="253px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="label">Transportadora:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtTransp" MaxLength="50" runat="server" CssClass="textBox" Width="253px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="label">Fone Transportadora:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtFoneTransp" MaxLength="25" runat="server" CssClass="textBox" Width="126px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="btnIncluiItens" runat="server" CssClass="button" OnClick="btnIncluiItens_Click"
                                Text="Incluir Itens" Width="189px" />
                            <br />
                            <br />
                        </td>
                    </tr>                  
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="btnVerificarPendencias" runat="server" OnClick="btnVerificarPendencias_Click"
                                Text="Pendência Financeira" CssClass="button" Width="189px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblPendencias" runat="server" CssClass="label" Width="488px"></asp:Label>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div id="Div1">
                                <asp:Label ID="lblInfo" runat="server" CssClass="label" Height="16px" Visible="False"
                                    Width="488px"></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">&nbsp;
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="ViewItens" runat="server">
                <table width="100%">
                    <tr class="BordaInferior">
                        <td style="text-align: left; color: Black" class="Titulo" colspan="2">Pesquisar Produto
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style3">Código
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCodProd" runat="server" CssClass="textBox" Width="127px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtCodProd"
                                ValidChars="0123456789">
                            </asp:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style3">Descrição
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtProdDesc" runat="server" CssClass="textBox" Width="328px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style3">Linha de Produto
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cbxLinhaProduto" runat="server" AutoPostBack="True" CssClass="textBox"
                                DataTextField="DS_LINHA" DataValueField="CD_LINHA" OnSelectedIndexChanged="cbxListaPreco_SelectedIndexChanged"
                                Width="328px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style3">Lista de Preço
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cbxListaPreco" runat="server" AutoPostBack="True" CssClass="textBox"
                                OnSelectedIndexChanged="cbxListaPreco_SelectedIndexChanged" Width="235px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style3"></td>
                        <td align="left"></td>
                    </tr>
                    <tr>
                        <td align="center" class="label" colspan="2">
                            <asp:Button ID="btnLoc" runat="server" CssClass="button" OnClick="Pesquisa_Click"
                                Text="Localizar" Width="131px" />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="label" colspan="2">
                            <asp:Button ID="btnVoltar" runat="server" CssClass="button" OnClick="btnVoltar_Click"
                                Text="Voltar" Visible="False" Width="131px" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style3"></td>
                        <td align="left"></td>
                    </tr>
                    <tr>
                        <td align="right" class="style3"></td>
                        <td align="left"></td>
                    </tr>
                    <tr class="BordaInferior">
                        <td style="text-align: left; color: Black" class="Titulo" colspan="2">Itens do Pedido
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:GridView ID="GridViewDb" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="alt"
                                AutoGenerateColumns="False" Caption="&lt;b&gt;Produtos a serem inseridos&lt;/b&gt;"
                                CaptionAlign="Left" CssClass="mGrid" DataKeyNames="CD_PROD" Font-Names="Segoe UI"
                                Font-Size="13px" GridLines="None" OnPageIndexChanging="GridViewDb_PageIndexChanging"
                                OnRowCommand="GridViewDb_RowCommand" OnRowDataBound="GridViewDb_RowDataBound"
                                OnSelectedIndexChanged="GridViewDb_SelectedIndexChanged" PagerStyle-CssClass="pgr"
                                Width="100%">
                                <AlternatingRowStyle CssClass="alt" />
                                <Columns>
                                    <asp:ButtonField CommandName="Incluir" Text="Incluir">
                                    <ControlStyle BackColor="Transparent" BorderStyle="None" Font-Names="Segoe UI" Font-Size="13px" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle ForeColor="#3333CC" HorizontalAlign="Left" Width="30px" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="CD_PROD" HeaderText="Código">
                                    <ItemStyle Font-Names="Segoe UI" Font-Size="13px" HorizontalAlign="Left" Width="60px" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CD_BARRAS" HeaderText="Código Barras">
                                    <ItemStyle HorizontalAlign="Left" Width="80px" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Left" Width="50px" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DS_PROD" HeaderText="Descrição">
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    <ItemStyle HorizontalAlign="Left" Width="200px" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Vl. Unitário">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("VL_PRECOVE") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemStyle Font-Names="Segoe UI" Font-Size="13px" HorizontalAlign="Right" Width="60px"
                                            Wrap="False" />
                                        <HeaderStyle Font-Names="Segoe UI" Font-Size="13px" HorizontalAlign="Left" Wrap="False" />
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("VL_PRECOVE", "{0:N2}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Vl. IVA" Visible="False">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VL_PESOBRU" DataFormatString="{0:0.0000}" HeaderText="Peso"
                                        Visible="False">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="QT_ESTOQUE" DataFormatString="{0:0.00000}" HeaderText="Estoque"
                                        Visible="False">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="60px" />
                                    </asp:BoundField>
                                    <asp:ButtonField CommandName="Alertar" Text="Alertar" Visible="true">
                                    <ControlStyle BackColor="Transparent" BorderStyle="None" Font-Names="Segoe UI" Font-Size="13px" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="30px" />
                                    </asp:ButtonField>
                                    <asp:CommandField ButtonType="Link" CancelText="" DeleteText="" EditText="" NewText=""
                                        SelectText="Detalhes" ShowSelectButton="True">
                                    <HeaderStyle Width="20px" />
                                    <ItemStyle BorderStyle="None" Font-Underline="True" ForeColor="Blue" />
                                    </asp:CommandField>
                                </Columns>
                                <PagerStyle CssClass="pgr" />
                                <SelectedRowStyle BackColor="#EFEFEF" Font-Bold="True" ForeColor="White" />
                                <PagerSettings FirstPageText="Início" LastPageText="Fim" Mode="NextPreviousFirstLast"
                                    NextPageText="Próximo" PreviousPageText="Anterior" />
                            </asp:GridView>
                            <br />
                            <asp:DetailsView ID="dvAviso" runat="server" GridLines="None" Height="30px" Width="100%"
                                AutoGenerateRows="False" CssClass="mGrid" PagerStyle-CssClass="pgr" Font-Names="Segoe UI"
                                Font-Size="13px" AlternatingRowStyle-CssClass="alt">
                                <AlternatingRowStyle BorderStyle="None" />
                                <Fields>
                                    <asp:BoundField DataField="DS_PROD" HeaderText="Descrição">
                                    <HeaderStyle Width="300px" />
                                    <ItemStyle Font-Bold="True" HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VL_PESOBRU" HeaderText="Peso">
                                    <ItemStyle Font-Bold="True" HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="QT_ESTOQUE" HeaderText="Estoque">
                                    <ItemStyle Font-Bold="True" HorizontalAlign="Left" />
                                    </asp:BoundField>
                                </Fields>
                                <HeaderStyle HorizontalAlign="Left" />
                                <PagerStyle CssClass="pgr" />
                            </asp:DetailsView>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="label" colspan="2"></td>
                    </tr>                                      
                    <tr>
                        <td align="right" class="style3">Total do Pedido R$
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtTotalPedidoSemDesc" runat="server" CssClass="textBox" MaxLength="4"
                                BackColor="#CCCCCC" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style3">Condição de Pgto
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cbxCD_PRAZO" runat="server" AutoPostBack="True" CssClass="textBox"
                                AppendDataBoundItems="true" DataTextField="DS_PRAZO" DataValueField="CD_PRAZO"
                                Enabled="True" Width="185px">
                                <asp:ListItem>Selecione um item</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style3">Tipo de Documento
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtTipoDoc" runat="server" CssClass="textBox" MaxLength="4" BackColor="#CCCCCC"
                                ReadOnly="True" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:GridView ID="GridViewNovo" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                CssClass="mGrid" PagerStyle-CssClass="pgr" Font-Names="Segoe UI" Font-Size="13px"
                                AlternatingRowStyle-CssClass="alt" Caption="&lt;b&gt;Nenhum produto inserido&lt;/b&gt;"
                                CaptionAlign="Left" GridLines="None" OnRowCommand="GridViewNovo_RowCommand" OnRowDataBound="GridViewNovo_RowDataBound"
                                OnRowEditing="GridViewNovo_RowEditing" ShowFooter="True" Width="100%" PageSize="1000">
                                <AlternatingRowStyle CssClass="alt" />
                                <Columns>
                                    <asp:ButtonField CommandName="Excluir" Text="Excluir">
                                    <ControlStyle BackColor="Transparent" BorderStyle="None" Font-Names="Segoe UI" Font-Size="13px" />
                                    <ItemStyle HorizontalAlign="Left" Width="30px" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="CD_PROD" HeaderText="Código">
                                    <ItemStyle HorizontalAlign="Left" Width="80px" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Qtde">
                                        <ItemTemplate>
                                            <%--<cc1:HlpWebTextBoxInteiro ID="txtQtde" runat="server" BackColor="Info" BorderColor="#E0E0E0"
                                                Font-Names="Segoe UI" Font-Size="13px" MaxLength="9" Text='<%# Bind("QT_PROD") %>'
                                                Width="65px"></cc1:HlpWebTextBoxInteiro>--%>
                                            <asp:TextBox ID="txtQtde" runat="server" BackColor="Info" BorderColor="#E0E0E0" Font-Names="Segoe UI"
                                                Font-Size="13px" MaxLength="9" Text='<%# Bind("QT_PROD") %>' Width="65px"></asp:TextBox>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <%--  <cc1:HlpWebTextBoxInteiro ID="txtQtde" runat="server" BackColor="White" BorderColor="#E0E0E0"
                                                Font-Names="Segoe UI" Font-Size="13px" MaxLength="9" Text='<%# Bind("QT_PROD") %>'
                                                Width="65px" onfocus="this.select()"></cc1:HlpWebTextBoxInteiro>--%>
                                            <asp:TextBox ID="txtQtde" runat="server" BackColor="White" BorderColor="#E0E0E0"
                                                Font-Names="Segoe UI" Font-Size="13px" MaxLength="9" Text='<%# Bind("QT_PROD") %>'
                                                Width="65px"></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="60px" Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DESC" HeaderText="Descrição">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="200px" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VL_DESCONTO" HeaderText="Desconto" HeaderStyle-HorizontalAlign="Left"
                                        ItemStyle-HorizontalAlign="Left" ItemStyle-Width="60px">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Right" Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CD_LISTA" HeaderText="Lista" ItemStyle-HorizontalAlign="Left"
                                        ItemStyle-Width="60px">
                                    <ItemStyle HorizontalAlign="Right" Width="40px" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VL_PROD" HeaderText="Vl. Unitário">
                                    <ItemStyle Width="60px" Wrap="False" HorizontalAlign="Right" />
                                    <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VL_UNIPROD_SEM_DESC" Visible="true" HeaderText="Vl. s/desc">
                                    <ItemStyle Width="60px" Wrap="False" HorizontalAlign="Right" />
                                    <HeaderStyle Wrap="False" Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SUBTOTAL" HeaderText="Vl. Total">
                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" Width="50" />
                                    <ItemStyle HorizontalAlign="Right" Wrap="False" Width="50" />
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="pgr" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="btnAtualiza" runat="server" CssClass="button" OnClick="btnAtualiza_Click"
                                Text="Atualizar Total" Visible="False" Width="160px" />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="btnAvancar" runat="server" CssClass="button" OnClick="btnAvancar_Click"
                                Text="Finalizar Pedido" Width="160px" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            &nbsp;&nbsp;
            <asp:View ID="ViewObs" runat="server">
                <table style="width: 100%">
                    <tr class="BordaInferior">
                        <td style="text-align: left; color: Black" class="Titulo" colspan="3">Observação
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px"></td>
                        <td style="width: 100px"></td>
                        <td style="width: 100px"></td>
                    </tr>
                    <tr>
                        <td align="center" style="width: 100px"></td>
                        <td style="width: 100px" align="center">
                            <asp:TextBox ID="txtObs" runat="server" CssClass="textBox" Height="182px" TextMode="MultiLine"
                                Width="501px"></asp:TextBox>
                        </td>
                        <td align="center" style="width: 100px"></td>
                    </tr>
                    <tr>
                        <td style="width: 100px; height: 21px"></td>
                        <td style="width: 100px; height: 21px;"></td>
                        <td style="width: 100px; height: 21px"></td>
                    </tr>
                </table>
                <table style="width: 623px">
                    <tr>
                        <td style="width: 100px; height: 21px">
                            <table style="width: 614px">
                                <tr>
                                    <td align="center" style="width: 100px; height: 21px;"></td>
                                    <td align="center" style="width: 100px; height: 21px;">
                                        <table style="width: 504px">
                                            <tr>
                                                <td class="style2"></td>
                                                <td class="style1"></td>
                                                <td style="width: 100px"></td>
                                            </tr>
                                            <tr>
                                                <td class="style2"></td>
                                                <td align="center" class="style1">
                                                    <asp:Button ID="btnVolteItem" runat="server" OnClick="btnVolteItem_Click" Text="Voltar "
                                                        CssClass="button" Width="130px" />
                                                </td>
                                                <td style="width: 100px"></td>
                                            </tr>
                                            <tr>
                                                <td class="style2"></td>
                                                <td class="style1"></td>
                                                <td style="width: 100px"></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="center" style="width: 100px; height: 21px;"></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="width: 100px; height: 40px" valign="top">
                            <table style="width: 614px">
                                <tr>
                                    <td style="width: 198px" align="center" valign="top">
                                        <table style="width: 251px">
                                            <tr>
                                                <td style="width: 100px"></td>
                                                <td style="width: 100px"></td>
                                                <td style="width: 100px"></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100px"></td>
                                                <td style="width: 100px"></td>
                                                <td style="width: 100px">
                                                    <asp:Button ID="btnGravar" 
                                                        OnClientClick="this.disabled = true; this.value = 'Aguarde...'" 
                                                        runat="server" 
                                                        Text="Gravar Pedido" Width="130px" OnClick="btnGravar2_Click"
                                                        UseSubmitBehavior="false" 
                                                        CssClass="button" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100px; height: 21px"></td>
                                                <td style="width: 100px; height: 21px"></td>
                                                <td style="width: 100px; height: 21px"></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 94px"></td>
                                    <td style="width: 100px" align="center" valign="top">
                                        <table style="width: 250px">
                                            <tr>
                                                <td style="width: 100px"></td>
                                                <td style="width: 100px"></td>
                                                <td style="width: 100px"></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100px">
                                                    <asp:Button ID="btnCancelar" runat="server" OnClick="btnCancelar_Click" Text="Cancelar"
                                                        Width="130px" CssClass="button" />
                                                </td>
                                                <td style="width: 100px"></td>
                                                <td style="width: 100px"></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100px"></td>
                                                <td style="width: 100px"></td>
                                                <td style="width: 100px"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 198px"></td>
                                    <td style="width: 94px"></td>
                                    <td style="width: 100px"></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px" align="center">&nbsp;
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="ViewDuplicatas" runat="server">
                <table style="width: 100%;">
                    <tr class="BordaInferior">
                        <td style="text-align: left; color: Black" class="Titulo" colspan="3">Duplicatas em Aberto
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <br />
                            <br />
                            <asp:GridView ID="GridDuplicatas" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                CssClass="mGrid" PagerStyle-CssClass="pgr" Font-Names="Segoe UI" Font-Size="13px"
                                AlternatingRowStyle-CssClass="alt" CaptionAlign="Left" GridLines="None" ShowFooter="True"
                                Width="100px" PageSize="15" OnRowDataBound="GridDuplicatas_RowDataBound" OnPageIndexChanging="GridDuplicatas_PageIndexChanging">
                                <AlternatingRowStyle CssClass="alt" />
                                <Columns>
                                    <asp:BoundField DataField="DT_VENCI" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Vencimento">
                                    <ItemStyle HorizontalAlign="Left" Width="100px" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VL_DOC" DataFormatString="{0:c}" HeaderText="Valor">
                                    <ItemStyle HorizontalAlign="Right" Width="150px" Wrap="False" />
                                    <FooterStyle Font-Names="Segoe UI" Font-Size="13px" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CD_DUPLI" HeaderText="Duplicata">
                                    <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="pgr" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <br />
                            <br />
                            <asp:Button ID="btnVoltarDuplicata" runat="server" Text="Voltar para o Pedido" Width="171px"
                                OnClick="btnVolteClie_Click" CssClass="button" />
                        </td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
