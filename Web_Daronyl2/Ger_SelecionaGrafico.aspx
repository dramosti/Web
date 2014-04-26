<%@ Page Title="Selecionar Gráfico" Language="C#" MasterPageFile="~/Ger_Site.master"
    AutoEventWireup="true" CodeFile="Ger_SelecionaGrafico.aspx.cs" Inherits="Ger_SelecionaGrafico" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .style1
        {
            height: 220px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <link href="Styles/menu.css" rel="stylesheet" type="text/css" />
    <table width="100%">
        <tr class="BordaInferior">
            <td style="text-align: left; color: Black" class="Titulo" colspan="2">
                Selecione o Tipo de Gráfico
            </td>

        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td align="center">
                <div id="menuH" align="center">
                    <ul>
                        <li><a href="Ger_VendasPorRepresentante.aspx">Vendas por Representante (Anual)</a></li>
                        <li><a href="Ger_ProdutosMaisVendidos.aspx">Produtos mais Vendidos por Período</a></li>
                        <li><a href="Ger_VendasProduto.aspx">Vendas por Produto</a></li>
                    </ul>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
