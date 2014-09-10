using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HLP.Geral;
using HLP.Web;

public partial class PesquisaVendas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DateTime dDataFinal = DateTime.Now;
            txtDataFinal.Text = dDataFinal.ToString("dd/MM/yyyy");
            txtDataInicial.Text = (dDataFinal.AddDays(-5).ToString("dd/MM/yyyy"));
            string sUser = UsuarioWeb.GetNomeUsuarioConectado(Session);
            if (sUser == "")
            {
                Response.Redirect("~/Login.aspx");
            }
        }

    }

    protected void btnPesquisarPedido_Click(object sender, EventArgs e)
    {
        PesquisarDados();
        IncluiItens();
        Response.Redirect("~/ViewVendasNF.aspx");

    }


    private void PesquisarDados()
    {
        try
        {
            UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
            DataTable dtVendas = new DataTable();
            StringBuilder str = new StringBuilder();
            str.Append("SELECT DISTINCT VEND.CD_VEND,VEND.NM_GUERRA AS NM_GUERRAVE, VEND.NM_VEND, ");
            str.Append("DOC_CTR.DT_REFCOMI, NF.DT_EMI, NF.CD_CLIFOR, ");
            str.Append("CAST(NF.CD_NOTAFIS AS VARCHAR(6)) CD_NOTAFIS, ");
            str.Append("NF.CD_NFSEQ, ");
            str.Append("CAST(NF.CD_EMPRESA||'-'||NF.CD_VEND1||'-'|| ");
            str.Append("NF.CD_NOTAFIS AS VARCHAR(25)) AS CD_RELACAO, ");
            str.Append("CAST(NF.VL_TOTNF AS NUMERIC(15,2)) VL_TOTPROD, ");
            str.Append("NF.CD_EMPRESA, NF.NM_GUERRA AS NM_GUERRA, ");
            str.Append("CAST(NF.NM_CLIFORNOR AS VARCHAR(80)) AS NM_CLIFOR, ");
            str.Append("CAST(NULL AS VARCHAR(7)) AS CD_PEDIDO, ");
            str.Append("CAST(NULL AS VARCHAR(2)) AS CD_SEQPED ");
            str.Append("FROM VENDEDOR VEND ");
            str.Append("INNER JOIN NF ON (NF.CD_VEND1 = VEND.CD_VEND) ");
            str.Append("INNER JOIN TPDOC ON (NF.CD_TIPODOC = TPDOC.CD_TIPODOC) ");
            str.Append("LEFT JOIN DOC_CTR ON (DOC_CTR.CD_NFSEQ = NF.CD_NFSEQ) ");
            str.Append("WHERE ((NF.CD_NOTAFIS IS NOT NULL) OR (NF.CD_NOTAFIS <> '')) AND ");
            str.Append("((NF.ST_CANNF IS NULL) OR (NF.ST_CANNF = '')) AND (TPDOC.ST_FATUR = '0') ");
            str.Append("AND (CD_VEND = " + "'" + objUsuario.CodigoVendedor.ToString() + "')");

            if (txtDataFinal.Text != "" && txtDataInicial.Text != "")
            {
                str.Append(" AND NF.DT_EMI Between " + "'" + txtDataInicial.Text.Replace("/", ".") + "'" + " AND " + "'" + txtDataFinal.Text.Replace("/", ".") + "'");
            }
            else
            {
                throw new Exception("Data inválida.");
            }

            str.Append(" UNION ");
            str.Append("SELECT VEND.CD_VEND, VEND.NM_GUERRA AS NM_GUERRAVE, VEND.NM_VEND, ");
            str.Append("DOC_CTR.DT_REFCOMI, ");
            str.Append("PEDSEQ.DT_ABERSEQ as DT_EMI, ");
            str.Append("PEDIDO.CD_CLIENTE as CD_CLIFOR, ");
            str.Append("CAST(NULL AS VARCHAR(6)) AS CD_NOTAFIS, ");
            str.Append("CAST(NULL AS VARCHAR(6)) AS CD_NFSEQ, ");
            str.Append("CAST(PEDIDO.CD_EMPRESA||'-'||PEDIDO.CD_VEND1||'-'|| ");
            str.Append("PEDIDO.CD_PEDIDO||'-'||PEDSEQ.CD_SEQPED AS ");
            str.Append("VARCHAR(25)) AS CD_RELACAO, ");
            str.Append("(SELECT VL_TOTPED  FROM SP_TOTALPED(PEDIDO.CD_EMPRESA,PEDIDO.CD_PEDIDO,PEDSEQ.CD_SEQPED)), ");
            str.Append("PEDIDO.CD_EMPRESA, PEDIDO.NM_GUERRA AS NM_GUERRA, ");
            str.Append("CAST(CLIFOR.NM_CLIFOR AS VARCHAR(80)) AS NM_CLIFOR, ");
            str.Append("CAST(PEDIDO.CD_PEDIDO AS VARCHAR(7)) AS CD_PEDIDO, ");
            str.Append("CAST(PEDSEQ.cd_seqped AS VARCHAR(2)) AS CD_SEQPED ");
            str.Append("FROM VENDEDOR VEND ");
            str.Append("INNER JOIN PEDIDO ON (PEDIDO.CD_VEND1 = VEND.CD_VEND) ");
            str.Append("INNER JOIN PEDSEQ ON (PEDSEQ.CD_PEDIDO = PEDIDO.CD_PEDIDO AND  PEDSEQ.CD_EMPRESA=PEDIDO.CD_EMPRESA AND (PEDSEQ.CD_NFSEQ IS NULL OR PEDSEQ.CD_NFSEQ='') AND PEDSEQ.ST_CANPED='N') ");
            str.Append("INNER JOIN CLIFOR ON (PEDIDO.CD_CLIENTE = CLIFOR.CD_CLIFOR) ");
            str.Append("INNER JOIN TPDOC ON (PEDIDO.CD_TIPODOC = TPDOC.CD_TIPODOC) ");
            str.Append("LEFT JOIN DOC_CTR ON (DOC_CTR.CD_PEDIDO = PEDIDO.CD_PEDIDO) ");
            str.Append("WHERE (TPDOC.ST_FATUR = '0') ");
            str.Append("AND (CD_VEND = " + "'" + objUsuario.CodigoVendedor.ToString() + "')");

            if (txtDataFinal.Text != "" && txtDataInicial.Text != "")
            {
                str.Append(" AND PEDSEQ.DT_ABERSEQ Between " + "'" + txtDataInicial.Text.Replace("/", ".") + "'" + " AND " + "'" + txtDataFinal.Text.Replace("/", ".") + "'");
            }
            
            dtVendas = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(str.ToString());
            Session["Vendas"] = dtVendas;
        }
        catch (Exception ex)
        {
            MessageHLP.ShowPopUpMsg(ex.Message, this);
        }

    }

    private void IncluiItens()
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        DataTable dtVendas = (DataTable)Session["Vendas"];

        // Criando a tabela de itens

        DataTable dtItens = new DataTable("ITENS");
        DataColumn dcColunasItem;

        dcColunasItem = new DataColumn();
        dcColunasItem.DataType = System.Type.GetType("System.String");
        dcColunasItem.ColumnName = "NR_LANC";
        dcColunasItem.ReadOnly = true;
        dtItens.Columns.Add(dcColunasItem);

        dcColunasItem = new DataColumn();
        dcColunasItem.DataType = System.Type.GetType("System.String");
        dcColunasItem.ColumnName = "CD_EMPRESA";
        dcColunasItem.ReadOnly = true;
        dtItens.Columns.Add(dcColunasItem);

        dcColunasItem = new DataColumn();
        dcColunasItem.DataType = System.Type.GetType("System.String");
        dcColunasItem.ColumnName = "CD_VEND";
        dcColunasItem.ReadOnly = true;
        dtItens.Columns.Add(dcColunasItem);

        dcColunasItem = new DataColumn();
        dcColunasItem.DataType = System.Type.GetType("System.String");
        dcColunasItem.ColumnName = "CD_NOTAFIS";
        dcColunasItem.ReadOnly = true;
        dtItens.Columns.Add(dcColunasItem);

        dcColunasItem = new DataColumn();
        dcColunasItem.DataType = System.Type.GetType("System.Decimal");
        dcColunasItem.ColumnName = "CD_PEDIDO";
        dcColunasItem.ReadOnly = true;
        dtItens.Columns.Add(dcColunasItem);

        dcColunasItem = new DataColumn();
        dcColunasItem.DataType = System.Type.GetType("System.Decimal");
        dcColunasItem.ColumnName = "CD_SEQPED";
        dcColunasItem.ReadOnly = true;
        dtItens.Columns.Add(dcColunasItem);

        dcColunasItem = new DataColumn();
        dcColunasItem.DataType = System.Type.GetType("System.String");
        dcColunasItem.ColumnName = "CD_RELACAO";
        dcColunasItem.ReadOnly = true;
        dtItens.Columns.Add(dcColunasItem);

        dcColunasItem = new DataColumn();
        dcColunasItem.DataType = System.Type.GetType("System.String");
        dcColunasItem.ColumnName = "CD_PROD";
        dcColunasItem.ReadOnly = true;
        dtItens.Columns.Add(dcColunasItem);

        dcColunasItem = new DataColumn();
        dcColunasItem.DataType = System.Type.GetType("System.String");
        dcColunasItem.ColumnName = "CD_ALTER";
        dcColunasItem.ReadOnly = true;
        dtItens.Columns.Add(dcColunasItem);

        dcColunasItem = new DataColumn();
        dcColunasItem.DataType = System.Type.GetType("System.String");
        dcColunasItem.ColumnName = "DS_PROD";
        dcColunasItem.ReadOnly = true;
        dtItens.Columns.Add(dcColunasItem);

        dcColunasItem = new DataColumn();
        dcColunasItem.DataType = System.Type.GetType("System.String");
        dcColunasItem.ColumnName = "CD_PEDCLI";
        dcColunasItem.ReadOnly = true;
        dtItens.Columns.Add(dcColunasItem);

        dcColunasItem = new DataColumn();
        dcColunasItem.DataType = System.Type.GetType("System.String");
        dcColunasItem.ColumnName = "CD_TPUNID";
        dcColunasItem.ReadOnly = true;
        dtItens.Columns.Add(dcColunasItem);

        dcColunasItem = new DataColumn();
        dcColunasItem.DataType = System.Type.GetType("System.Decimal");
        dcColunasItem.ColumnName = "VL_PERCOMI1";
        dcColunasItem.ReadOnly = true;
        dtItens.Columns.Add(dcColunasItem);

        dcColunasItem = new DataColumn();
        dcColunasItem.DataType = System.Type.GetType("System.Double");
        dcColunasItem.ColumnName = "QT_PROD";
        dcColunasItem.ReadOnly = true;
        dtItens.Columns.Add(dcColunasItem);

        dcColunasItem = new DataColumn();
        dcColunasItem.DataType = System.Type.GetType("System.Decimal");
        dcColunasItem.ColumnName = "VL_COMISSAO";
        dcColunasItem.ReadOnly = true;
        dtItens.Columns.Add(dcColunasItem);

        dcColunasItem = new DataColumn();
        dcColunasItem.DataType = System.Type.GetType("System.Decimal");
        dcColunasItem.ColumnName = "VL_TOTLIQ";
        dcColunasItem.ReadOnly = true;
        dtItens.Columns.Add(dcColunasItem);
        dtItens.Columns.IndexOf("CD_RELACAO" + "DS_PROD");

        //Fim tabela itens

        DataTable dtMovitem = new DataTable("MOVITEM");

        //Criando Tabela NF

        DataTable dtNF = new DataTable("NF");
        DataColumn dcColNF;

        dcColNF = new DataColumn();
        dcColNF.DataType = System.Type.GetType("System.String");
        dcColNF.ColumnName = "NR_LANC";
        dcColNF.ReadOnly = true;
        dtNF.Columns.Add(dcColNF);

        dcColNF = new DataColumn();
        dcColNF.DataType = System.Type.GetType("System.String");
        dcColNF.ColumnName = "CD_EMPRESA";
        dcColNF.ReadOnly = true;
        dtNF.Columns.Add(dcColNF);

        dcColNF = new DataColumn();
        dcColNF.DataType = System.Type.GetType("System.String");
        dcColNF.ColumnName = "CD_NFSEQ";
        dcColNF.ReadOnly = true;
        dtNF.Columns.Add(dcColNF);

        dcColNF = new DataColumn();
        dcColNF.DataType = System.Type.GetType("System.String");
        dcColNF.ColumnName = "CD_CLIFOR";
        dcColNF.ReadOnly = true;
        dtNF.Columns.Add(dcColNF);

        dcColNF = new DataColumn();
        dcColNF.DataType = System.Type.GetType("System.String");
        dcColNF.ColumnName = "CD_VEND";
        dcColNF.ReadOnly = true;
        dtNF.Columns.Add(dcColNF);

        dcColNF = new DataColumn();
        dcColNF.DataType = System.Type.GetType("System.String");
        dcColNF.ColumnName = "CD_NOTAFIS";
        dcColNF.ReadOnly = true;
        dtNF.Columns.Add(dcColNF);

        dcColNF = new DataColumn();
        dcColNF.DataType = System.Type.GetType("System.String");
        dcColNF.ColumnName = "CD_PEDIDO";
        dcColNF.ReadOnly = true;
        dtNF.Columns.Add(dcColNF);

        dcColNF = new DataColumn();
        dcColNF.DataType = System.Type.GetType("System.String");
        dcColNF.ColumnName = "CD_SEQPED";
        dcColNF.ReadOnly = true;
        dtNF.Columns.Add(dcColNF);

        dcColNF = new DataColumn();
        dcColNF.DataType = System.Type.GetType("System.String");
        dcColNF.ColumnName = "CD_RELACAO";
        dcColNF.ReadOnly = true;
        dtNF.Columns.Add(dcColNF);

        dcColNF = new DataColumn();
        dcColNF.DataType = System.Type.GetType("System.String");
        dcColNF.ColumnName = "NM_GUERRA";
        dcColNF.ReadOnly = true;
        dtNF.Columns.Add(dcColNF);

        dcColNF = new DataColumn();
        dcColNF.DataType = System.Type.GetType("System.String");
        dcColNF.ColumnName = "NM_VEND";
        dcColNF.ReadOnly = true;
        dtNF.Columns.Add(dcColNF);

        dcColNF = new DataColumn();
        dcColNF.DataType = System.Type.GetType("System.String");
        dcColNF.ColumnName = "NM_CLIFOR";
        dcColNF.ReadOnly = true;
        dtNF.Columns.Add(dcColNF);

        dcColNF = new DataColumn();
        dcColNF.DataType = System.Type.GetType("System.DateTime");
        dcColNF.ColumnName = "DT_EMI";
        dcColNF.ReadOnly = true;
        dtNF.Columns.Add(dcColNF);

        dcColNF = new DataColumn();
        dcColNF.DataType = System.Type.GetType("System.Decimal");
        dcColNF.ColumnName = "VL_TOTPROD";
        dcColNF.ReadOnly = true;
        dtNF.Columns.Add(dcColNF);

        dcColNF = new DataColumn();
        dcColNF.DataType = System.Type.GetType("System.Decimal");
        dcColNF.ColumnName = "VL_TOTCOMI";
        dcColNF.ReadOnly = true;
        dtNF.Columns.Add(dcColNF);

        dcColNF = new DataColumn();
        dcColNF.DataType = System.Type.GetType("System.DateTime");
        dcColNF.ColumnName = "DT_REFCOMI";
        dcColNF.ReadOnly = true;
        dtNF.Columns.Add(dcColNF);
        dtNF.Columns.IndexOf("CD_EMPRESA" + "CD_VEND" + "NM_GUERRA" + "DT_EMI" +
                             "CD_REFCOMI" + "CD_NOTAFIS" + "CD_PEDIDO" + "CD_SEQPED");

        //Fim tabela NF

        DataTable dtConsulta = new DataTable("Consulta");

        //Criando tabela Duplicata

        DataTable dtDuplicata = new DataTable("DUPLICATA");
        DataColumn dcColDupli;

        dcColDupli = new DataColumn();
        dcColDupli.DataType = System.Type.GetType("System.String");
        dcColDupli.ColumnName = "CD_DUPLI";
        dcColDupli.ReadOnly = true;
        dtDuplicata.Columns.Add(dcColDupli);

        dcColDupli = new DataColumn();
        dcColDupli.DataType = System.Type.GetType("System.String");
        dcColDupli.ColumnName = "CD_RELACAO";
        dcColDupli.ReadOnly = true;
        dtDuplicata.Columns.Add(dcColDupli);

        dcColDupli = new DataColumn();
        dcColDupli.DataType = System.Type.GetType("System.Decimal");
        dcColDupli.ColumnName = "VL_DOC";
        dcColDupli.ReadOnly = true;
        dtDuplicata.Columns.Add(dcColDupli);

        dcColDupli = new DataColumn();
        dcColDupli.DataType = System.Type.GetType("System.DateTime");
        dcColDupli.ColumnName = "DT_VENC";
        dcColDupli.ReadOnly = true;
        dtDuplicata.Columns.Add(dcColDupli);
        dtDuplicata.Columns.IndexOf("CD_RELACAO" + "CD_DUPLI");
        //Fim tabela Duplicata

        decimal TotComi = 0;
        DataRow drDupli;
        DataRow drItenNF;
        DataRow drNF;

        ListBox lbCodigo = new ListBox();

        foreach (DataRow dr in dtVendas.Rows)
        {

            if (lbCodigo.Items.IndexOf(lbCodigo.Items.FindByText(dr["CD_RELACAO"].ToString())) >= 0)
                continue;
            else
                lbCodigo.Items.Add(dr["CD_RELACAO"].ToString());

            StringBuilder strSelect = new StringBuilder();
          //  strSelect.Append("SELECT CD_EMPRESA, CD_EMPRESA, ((CASE WHEN VL_COEF < 1 THEN VL_COEF ELSE 1 END * VL_TOTLIQ)-coalesce(VL_DESCSUFRAMA,0)) AS VL_TOTLIQ, VL_DESCPISSUFRAMA, VL_DESCCOFINSSUFRAMA, ");
            strSelect.Append("SELECT CD_EMPRESA, VL_TOTLIQ, VL_DESCPISSUFRAMA, VL_DESCCOFINSSUFRAMA, ");
            strSelect.Append("CD_PROD, DS_PROD,QT_PROD,CD_TPUNID,CD_ALTER, ");
            strSelect.Append("CD_PEDCLI, CD_VEND1, VL_PERCOMI1 ");
            strSelect.Append("FROM MOVITEM INNER JOIN OPEREVE ON (MOVITEM.CD_OPER = OPEREVE.CD_OPER) ");
            strSelect.Append("WHERE (CD_EMPRESA = '001') AND (CD_NFSEQ = '" + dr["CD_NFSEQ"] + "') AND (OPEREVE.ST_TPOPER = '0')");

            dtMovitem = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(strSelect.ToString());

            // Preencher Duplicatas.

            //PreencheDuplicata(sWhere.Substring(0, sWhere.Length - 4), dtVendas);


            StringBuilder strSelectDuplic = new StringBuilder();
            strSelectDuplic.Append("SELECT CD_DUPLI, DT_VENCI, VL_DOC ");
            strSelectDuplic.Append("FROM DOC_CTR ");
            strSelectDuplic.Append("WHERE ");
            if (dr["CD_NFSEQ"].ToString() != "")
                strSelectDuplic.Append("(CD_NFSEQ = '" + dr["CD_NFSEQ"] + "') AND ");
            else
                strSelectDuplic.Append("(CD_PEDIDO = '" + dr["CD_PEDIDO"] + "') AND (CD_SEQPED = '" + dr["CD_SEQPED"] + "') AND ");
            strSelectDuplic.Append("(CD_EMPRESA = '001') ");
            strSelectDuplic.Append("ORDER BY CD_EMPRESA, CD_DUPLI");

            dtConsulta = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet(strSelectDuplic.ToString());

            foreach (DataRow drItensDupli in dtConsulta.Rows)
            {

                drDupli = dtDuplicata.NewRow();

                //if (strComparacao != drItensDupli["CD_DUPLI"].ToString())
                //{
                if (dr["CD_NOTAFIS"].ToString() == "")
                    drDupli["CD_RELACAO"] = dr["CD_EMPRESA"] + "-" + dr["CD_VEND1"] + "-" +
                                            dr["CD_PEDIDO"] + "-" + dr["CD_SEQPED"];
                else
                {
                    drDupli["CD_RELACAO"] = dr["CD_EMPRESA"] + "-" + dr["CD_VEND"] + "-" +
                                            dr["CD_NOTAFIS"];
                }
                drDupli["CD_DUPLI"] = drItensDupli["CD_DUPLI"];
                drDupli["DT_VENC"] = drItensDupli["DT_VENCI"];
                drDupli["VL_DOC"] = drItensDupli["VL_DOC"];
                dtDuplicata.Rows.Add(drDupli);
                //}
            }

            // Fim Preencher Duplicatas.

            foreach (DataRow drItem in dtMovitem.Rows)
            {

                drItenNF = dtItens.NewRow();
                drItenNF["NR_LANC"] = HlpFuncoes.AlinharZerosEsquerda(Convert.ToString(dtItens.Rows.Count + 1), 7);
                drItenNF["CD_EMPRESA"] = "001";
                drItenNF["CD_NOTAFIS"] = dr["CD_NOTAFIS"];
                drItenNF["CD_VEND"] = objUsuario.CodigoVendedor.ToString();
                if (descontopiscofinssuframa(dr["CD_NFSEQ"].ToString(), "001"))
                {
                    drItenNF["VL_TOTLIQ"] = Convert.ToDecimal((drItem["VL_TOTLIQ"].ToString() == "" ? "0" : drItem["VL_TOTLIQ"].ToString())) 
                                            - Convert.ToDecimal((drItem["VL_DESCCOFINSSUFRAMA"].ToString() == "" ? "0" : drItem["VL_DESCCOFINSSUFRAMA"].ToString())) 
                                            - Convert.ToDecimal((drItem["VL_DESCPISSUFRAMA"].ToString() == "" ? "0" : drItem["VL_DESCPISSUFRAMA"].ToString()));
                }
                else
                {
                    drItenNF["VL_TOTLIQ"] = drItem["VL_TOTLIQ"];
                }
                if (drItem["CD_PEDCLI"] != null)
                    drItenNF["CD_PEDCLI"] = drItem["CD_PEDCLI"];

                if (dr["CD_NOTAFIS"].ToString().Trim() == "")
                    drItenNF["CD_RELACAO"] = drItem["CD_EMPRESA"] + "-" +
                                                 drItem["CD_VEND"] + "-" +
                                                 drItem["CD_PEDIDO"] + "-" +
                                                 drItem["CD_SEQPED"];
                else
                {
                    drItenNF["CD_RELACAO"] = drItem["CD_EMPRESA"] + "-" +
                                                 drItem["CD_VEND1"] + "-" +
                                                 dr["CD_NOTAFIS"];
                }

                drItenNF["QT_PROD"] = drItem["QT_PROD"];
                drItenNF["CD_TPUNID"] = drItem["CD_TPUNID"];
                drItenNF["VL_PERCOMI1"] = drItem["VL_PERCOMI1"];
                drItenNF["VL_COMISSAO"] = (Convert.ToDecimal(drItem["VL_PERCOMI1"].ToString()) * Convert.ToDecimal(drItem["VL_TOTLIQ"].ToString())) / 100;
                drItenNF["CD_PROD"] = drItem["CD_PROD"];
                drItenNF["DS_PROD"] = drItem["DS_PROD"];
                drItenNF["CD_ALTER"] = drItem["CD_ALTER"];

                // drItenNF["CD_INDEX"] = drItem["CD_ALTER"];

                //dtNF.Columns.IndexOf("CD_EMPRESA" + "NM_CLIFOR" + "DT_REFCOMI" + "CD_NOTAFIS" + "CD_PEDIDO" + "CD_SEQPES");
                TotComi += Convert.ToDecimal(drItenNF["VL_COMISSAO"].ToString());
                dtItens.Rows.Add(drItenNF);

            }

            drNF = dtNF.NewRow();
            drNF["NR_LANC"] = HlpFuncoes.AlinharZerosEsquerda(Convert.ToString(dtNF.Rows.Count + 1), 7);
            drNF["CD_EMPRESA"] = dr["CD_EMPRESA"];
            drNF["CD_NFSEQ"] = dr["CD_NFSEQ"];
            drNF["CD_VEND"] = dr["CD_VEND"];
            drNF["NM_GUERRA"] = dr["NM_GUERRA"];
            drNF["NM_VEND"] = dr["NM_VEND"];
            drNF["CD_CLIFOR"] = dr["CD_CLIFOR"];
            drNF["CD_NOTAFIS"] = dr["CD_NOTAFIS"];
            drNF["NM_CLIFOR"] = dr["NM_CLIFOR"];
            drNF["DT_EMI"] = dr["DT_EMI"];
            drNF["VL_TOTPROD"] = dr["VL_TOTPROD"];
            drNF["VL_TOTCOMI"] = TotComi;

            drNF["CD_RELACAO"] = dr["CD_RELACAO"];
            drNF["DT_REFCOMI"] = dr["DT_REFCOMI"];
            dtNF.Rows.Add(drNF);
            TotComi = 0;
        }

        Session["NF"] = dtNF;
        Session["ITENS"] = dtItens;
        Session["DUPLICATAS"] = dtDuplicata;
    }

    private bool descontopiscofinssuframa(string scdnfseq, string scdempresa)
    {

        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        DataTable dtNF = new DataTable();
        decimal TotaliqMovItem, TotalValoresNF = 0;
        bool Retorno = false;
        dtNF = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("NF", "VL_SEG, VL_FRETE, VL_OUTRAS, VL_ICMSSUB, VL_IMPIMPORT, VL_SERVICO, VL_TOTIPI, VL_TOTNF",
                                                          "CD_NFSEQ = " + "'" + scdnfseq + "'" + " AND CD_EMPRESA = " + "'" + objUsuario.oTabelas.sEmpresa + "'");

        foreach (DataRow dr in dtNF.Rows)
        {
            //StringBuilder strValor = new StringBuilder();
            //strValor.Append("SELECT ");
            //strValor.Append("FROM  ");
            //strValor.Append("WHERE );
            TotaliqMovItem = Convert.ToDecimal(objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("MOVITEM", "SUM(VL_TOTLIQ - COALESCE(VL_DESCSUFRAMA,0) - COALESCE(VL_DESCPISSUFRAMA,0) - COALESCE(VL_DESCCOFINSSUFRAMA,0)) AS VL_TOTLIQ", "CD_NFSEQ = '" + scdnfseq + "' AND CD_EMPRESA = '" + scdempresa + "'"));

            TotalValoresNF = Convert.ToDecimal(dr["VL_SEG"].ToString()) + Convert.ToDecimal(dr["VL_FRETE"].ToString()) + Convert.ToDecimal(dr["VL_OUTRAS"].ToString()) + Convert.ToDecimal(dr["VL_ICMSSUB"].ToString()) + Convert.ToDecimal(dr["VL_IMPIMPORT"].ToString()) + Convert.ToDecimal(dr["VL_SERVICO"].ToString());

            if (TotaliqMovItem == (Convert.ToDecimal(dr["VL_TOTNF"].ToString()) - TotalValoresNF))
                Retorno = true;

        }

        return Retorno;
    }
}