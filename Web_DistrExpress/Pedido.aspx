<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Pedido.aspx.cs" Inherits="Pedido" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="HLP.Web.Controles" Namespace="HLP.Web.Controles" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .style1
        {
            font-size: 14px;
            font-family: Segoe UI;
            color: Black;
            width: 923px;
        }
        .style3
        {
            font-size: 14px;
            font-family: Segoe UI;
            color: Black;
            width: 183px;
        }
        .style4
        {
            width: 183px;
        }
        .style5
        {
            font-size: 14px;
            font-family: Segoe UI;
            color: Black;
            width: 215px;
        }
        .style6
        {
            width: 215px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        $('#<%=txtDesconto.ClientID%>').live("blur", function (e) {

            var Valor = $('#<%=txtDesconto.ClientID%>').val();
            var valido = /^\d{1,5}(\,\d{1,2})?$/.test(Valor);

            if (valido) {
                Valor = Valor.replace(",", ".")
                var vlr = parseFloat(Valor);
                $('#<%=txtDesconto.ClientID%>').val(vlr.toFixed(2).replace(".", ","));
                return true;
            }
            else {
                $('#<%=txtDesconto.ClientID%>').val("0,00");
                $("#errmsg").html("Valor Inválido").show().fadeOut("slow");
                return false;
            }
        });
    </script>
    <div align="center">
        <asp:MultiView ID="MultiViewItensPed" runat="server" ActiveViewIndex="0">
            <asp:View ID="ViewDadosPedido" runat="server">
                <table style="width: 100%;">
                    <tr class="BordaInferior">
                        <td style="text-align: left; color: Black" class="Titulo" colspan="2">
                            Incluir Pedido
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="label">
                            Data do Pedido
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtDataPedido" runat="server" CssClass="textBox" Enabled="False"
                                Width="103px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="label">
                            Cliente
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCodCli" runat="server" CssClass="textBox" Enabled="True" Width="101px"></asp:TextBox>
                            <asp:TextBox ID="txtCliente" runat="server" CssClass="textBox" Enabled="False" Width="286px"></asp:TextBox>
                            <asp:Button ID="btnCliente" runat="server" CssClass="button" Height="26px" Text="Buscar Cliente"
                                OnClick="btnCliente_Click" />
                            <asp:Button ID="btnPesqCliente" runat="server" CssClass="button" Height="26px" Text="Filtrar Pesquisar"
                                OnClick="btnPesqCliente_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="label">
                            Tipo de Documento
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cbxDS_TPDOCWEB" runat="server" AutoPostBack="True" CssClass="textBox"
                                DataTextField="DS_TPDOC" DataValueField="CD_TPDOC" Enabled="true" Width="185px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <br />
                            <asp:Button ID="btnIncluiItens" runat="server" CssClass="button" OnClick="btnIncluiItens_Click"
                                Height="26px" Text="Incluir Itens" Width="180px" />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="btnVerificarPendencias" runat="server" OnClick="btnVerificarPendencias_Click"
                                Height="26px" Text="Pendência Financeira" CssClass="button" Width="180px" />
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
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="ViewItens" runat="server">
                <table width="100%">
                    <tr class="BordaInferior">
                        <td style="text-align: left; color: Black" class="Titulo" colspan="2">
                            Pesquisar Produto
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style5">
                            Prazo de Pagamento
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cbxPrazoPgto" runat="server" AutoPostBack="True" CssClass="textBox"
                                Width="185px" OnSelectedIndexChanged="cbxPrazoPgto_SelectedIndexChanged">
                                <asp:ListItem Value="0">A VISTA</asp:ListItem>
                                <asp:ListItem Value="1">A PRAZO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style5">
                            Código
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCodProd" runat="server" CssClass="textBox" Width="127px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtCodProd"
                                ValidChars="0123456789">
                            </asp:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style5">
                            Descrição
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtProdDesc" runat="server" CssClass="textBox" Width="321px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style5">
                            Lista de Preço
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cbxListaPreco" runat="server" CssClass="textBox" Width="235px"
                                AutoPostBack="True" OnSelectedIndexChanged="cbxListaPreco_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style5">
                        </td>
                        <td align="left">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="label" colspan="2">
                            <asp:Button ID="btnLoc" runat="server" OnClick="btnLoc_Click" Text="Localizar" CssClass="button"
                                Width="131px" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="label" colspan="2">
                            <asp:Button ID="btnVoltar" runat="server" OnClick="btnVoltar_Click" Text="Voltar"
                                Visible="False" CssClass="button" Width="131px" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style5">
                        </td>
                        <td align="left">
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style5">
                        </td>
                        <td align="left">
                        </td>
                    </tr>
                    <tr class="BordaInferior">
                        <td style="text-align: left; color: Black" class="Titulo" colspan="2">
                            Itens do Pedido
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:GridView ID="GridViewDb" CssClass="mGrid" PagerStyle-CssClass="pgr" Font-Names="Segoe UI"
                                Font-Size="13px" AlternatingRowStyle-CssClass="alt" runat="server" AllowPaging="True"
                                AutoGenerateColumns="False" Caption="&lt;b&gt;Produtos a serem inseridos&lt;/b&gt;"
                                CaptionAlign="Left" GridLines="None" OnPageIndexChanging="GridViewDb_PageIndexChanging"
                                OnRowCommand="GridViewDb_RowCommand" Width="100%" OnRowDataBound="GridViewDb_RowDataBound">
                                <Columns>
                                    <asp:ButtonField CommandName="Incluir" Text="Incluir">
                                    <ControlStyle BackColor="Transparent" BorderStyle="None" Font-Names="Segoe UI" Font-Size="13px" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="30px" ForeColor="#3333CC" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="CD_ALTER" HeaderText="Código Alternativo" Visible="False">
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
                                        <ItemStyle Font-Names="Segoe UI" Font-Size="13px" HorizontalAlign="Left" Width="60px"
                                            Wrap="False" />
                                        <HeaderStyle Font-Names="Segoe UI" Font-Size="13px" HorizontalAlign="Left" Wrap="False" />
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("VL_PRECOVE", "{0:N2}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CD_PROD" HeaderText="Código">
                                    <ItemStyle HorizontalAlign="Left" Width="60px" Font-Names="Segoe UI" Font-Size="13px" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VL_PESOBRU" DataFormatString="{0:0.0000}" HeaderText="Peso">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="QT_ESTOQUE" DataFormatString="{0:0.00000}" HeaderText="Estoque">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="60px" />
                                    </asp:BoundField>
                                    <asp:ButtonField CommandName="Alertar" Text="Alertar">
                                    <ControlStyle BackColor="Transparent" BorderStyle="None" Font-Names="Segoe UI" Font-Size="13px" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="30px" />
                                    </asp:ButtonField>
                                </Columns>
                                <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                <PagerSettings FirstPageText="Início" LastPageText="Fim" Mode="NextPreviousFirstLast"
                                    NextPageText="Próximo" PreviousPageText="Anterior" />
                            </asp:GridView>
                            <br />
                        </td>
                    </tr>
                    <tr style="visibility: hidden">
                        <td align="right" class="style5">
                            Desconto no Total(R$)
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtDesconto" runat="server" CssClass="textBox" MaxLength="8">0,00</asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style6">
                        </td>
                        <td align="left">
                            <div id="errmsg">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:GridView ID="GridViewNovo" runat="server" AllowSorting="True" AlternatingRowStyle-CssClass="alt"
                                AutoGenerateColumns="False" Caption="&lt;b&gt;Produtos inseridos&lt;/b&gt;" CaptionAlign="Left"
                                CssClass="mGrid" Font-Names="Segoe UI" Font-Size="13px" GridLines="None" OnRowCommand="GridViewNovo_RowCommand"
                                OnRowDataBound="GridViewNovo_RowDataBound" OnRowEditing="GridViewNovo_RowEditing"
                                PagerStyle-CssClass="pgr" PageSize="20" ShowFooter="True" Width="100%">
                                <AlternatingRowStyle CssClass="alt" />
                                <Columns>
                                    <asp:ButtonField CommandName="Excluir" Text="Excluir">
                                    <ControlStyle BackColor="Transparent" BorderStyle="None" Font-Names="Segoe UI" Font-Size="13px" />
                                    <ItemStyle HorizontalAlign="Left" Width="30px" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="CD_ALTER" HeaderText="Código/Lista">
                                    <ItemStyle HorizontalAlign="Left" Width="80px" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DESC" HeaderText="Descrição">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="200px" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CD_LISTA" HeaderText="Lista" Visible="False">
                                    <ItemStyle HorizontalAlign="Left" Width="80px" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VL_PROD" DataFormatString="{0:N2}" HeaderText="Vl. Unitário">
                                    <ItemStyle Width="60px" Wrap="False" />
                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Pacote" Visible="False">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="60px" Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="SUBTOTAL" HeaderText="Vl. Total" HtmlEncode="False">
                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VL_PESOBRU" DataFormatString="{0:0.0000}" HeaderText="Peso">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="80px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Qtde">
                                        <ItemTemplate>
                                            <cc1:HlpWebTextBoxInteiro ID="txtQtde" runat="server" BackColor="Info" BorderColor="#E0E0E0"
                                                Font-Names="Segoe UI" Font-Size="13px" MaxLength="4" Text='<%# Bind("QT_PROD") %>'
                                                Width="50px"></cc1:HlpWebTextBoxInteiro>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <cc1:HlpWebTextBoxInteiro ID="txtQtde" runat="server" BackColor="White" BorderColor="#E0E0E0"
                                                Font-Names="Segoe UI" Font-Size="13px" MaxLength="3" Text='<%# Bind("QT_PROD") %>'
                                                Width="50px"></cc1:HlpWebTextBoxInteiro>
                                        </EditItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="60px" Wrap="False" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="pgr" />
                                <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="label">
                            Condição Pagto
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cbxCD_PRAZO" runat="server" AutoPostBack="True" CssClass="textBox"
                                DataTextField="DS_PRAZO" DataValueField="CD_PRAZO"  Width="185px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="btnAtualiza" runat="server" CssClass="button" OnClick="btnAtualiza_Click"
                                Text="Atualizar Total" Visible="False" Width="135px" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="btnAvancar" runat="server" CssClass="button" OnClick="btnAvancar_Click"
                                Text="Finalizar Pedido" Width="135px" />
                        </td>
                    </tr>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="ViewObs" runat="server">
                <table style="width: 100%">
                    <tr class="BordaInferior">
                        <td style="text-align: left; color: Black" class="Titulo" colspan="3">
                            Observação
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="width: 100px">
                        </td>
                        <td style="width: 100px" align="center">
                            <asp:TextBox ID="txtObs" runat="server" CssClass="textBox" Height="182px" TextMode="MultiLine"
                                Width="501px"></asp:TextBox>
                        </td>
                        <td align="center" style="width: 100px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px; height: 21px">
                        </td>
                        <td style="width: 100px; height: 21px;">
                        </td>
                        <td style="width: 100px; height: 21px">
                        </td>
                    </tr>
                </table>
                <table style="width: 623px">
                    <tr>
                        <td style="width: 100px; height: 21px">
                            <table style="width: 614px">
                                <tr>
                                    <td align="center" style="width: 100px; height: 21px;">
                                    </td>
                                    <td align="center" style="width: 100px; height: 21px;">
                                        <table style="width: 504px">
                                            <tr>
                                                <td style="width: 100px">
                                                </td>
                                                <td style="width: 100px">
                                                </td>
                                                <td style="width: 100px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100px">
                                                </td>
                                                <td style="width: 100px">
                                                    <asp:Button ID="btnVolteItem" runat="server" OnClick="btnVolteItem_Click" Text="Voltar "
                                                        CssClass="button" Width="130px" />
                                                </td>
                                                <td style="width: 100px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100px">
                                                </td>
                                                <td style="width: 100px">
                                                </td>
                                                <td style="width: 100px">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="center" style="width: 100px; height: 21px;">
                                    </td>
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
                                                <td style="width: 100px">
                                                </td>
                                                <td style="width: 100px">
                                                </td>
                                                <td style="width: 100px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100px">
                                                </td>
                                                <td style="width: 100px">
                                                </td>
                                                <td style="width: 100px">
                                                    <asp:Button ID="btnGravar" runat="server" Text="Gravar Pedido" Width="130px" OnClick="btnGravar_Click"
                                                        CssClass="button" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100px; height: 21px">
                                                </td>
                                                <td style="width: 100px; height: 21px">
                                                </td>
                                                <td style="width: 100px; height: 21px">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 94px">
                                    </td>
                                    <td style="width: 100px" align="center" valign="top">
                                        <table style="width: 250px">
                                            <tr>
                                                <td style="width: 100px">
                                                </td>
                                                <td style="width: 100px">
                                                </td>
                                                <td style="width: 100px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100px">
                                                    <asp:Button ID="btnCancelar" runat="server" OnClick="btnCancelar_Click" Text="Cancelar"
                                                        Width="130px" CssClass="button" />
                                                </td>
                                                <td style="width: 100px">
                                                </td>
                                                <td style="width: 100px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100px">
                                                </td>
                                                <td style="width: 100px">
                                                </td>
                                                <td style="width: 100px">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 198px">
                                    </td>
                                    <td style="width: 94px">
                                    </td>
                                    <td style="width: 100px">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px" align="center">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="ViewDuplicatas" runat="server">
                <table style="width: 100%;">
                    <tr class="BordaInferior">
                        <td style="text-align: left; color: Black" class="Titulo" colspan="3">
                            Duplicatas em Aberto
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
                                <Columns>
                                    <asp:BoundField DataField="DT_VENCI" HeaderText="Vencimento" DataFormatString="{0:dd/MM/yyyy}">
                                    <ItemStyle HorizontalAlign="Left" Width="100px" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VL_DOC" HeaderText="Valor" DataFormatString="{0:c}">
                                    <ItemStyle HorizontalAlign="Left" Width="150px" Wrap="False" />
                                    <FooterStyle Font-Names="Segoe UI" Font-Size="13px" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <br />
                            <br />
                            <asp:Button ID="btnVoltarDuplicata" runat="server" Text="Voltar para o Pedido" Width="160px"
                                OnClick="btnVolteClie_Click" CssClass="button" />
                        </td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
