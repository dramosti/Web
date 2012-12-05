using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace HLP.Dados.Cadastro
{
    public abstract class CliforDAO : BaseDAO
    {

        public CliforDAO(Tabela oTabelas)
            : base(oTabelas)
        {
            this.Tabela = "CLIFOR";
            this.ChavePrimaria.Add("CD_CLIFOR");
            this.Inicializar();
        }

        protected override bool Exclusao()
        {
            if (PossuiMovimentacao("PEDIDO", "CD_CLIENTE",
                "O Cliente tem Pedidos de Venda lançados."))
                return false;

            if (PossuiMovimentacao("NF", "CD_CLIFOR",
                "O Cliente ou Fornecedor tem Notas Fiscais lançadas."))
                return false;

            if (PossuiMovimentacao("DOC_CTR", "CD_CLIENTE",
                "O Cliente tem Documentos a Receber lançados."))
                return false;

            if (PossuiMovimentacao("DOC_CTP", "CD_FORNEC",
                "O Fornecedor tem Documentos a Pagar lançados."))
                return false;

            if (PossuiMovimentacao("MOVITEM", "CD_CLIFOR",
                "O Cliente ou Fornecedor tem Movimentações lançadas."))
                return false;

            return true;
        }

        private bool PossuiMovimentacao(string sTabela, string sCampoPesquisa,
            string sMensagem)
        {
            DataTable qry = oTabelas.hlpDbFuncoes.qrySeekRet(
                sTabela, sCampoPesquisa, "(" + sCampoPesquisa + " = '" +
                RegistroAtual["CD_CLIFOR"].ToString() + "') AND " +
                "(CD_EMPRESA IS NOT NULL)");
            bool bPossuiMovimentacao = (qry.Rows.Count > 0);
            if (bPossuiMovimentacao)
                this.SetErro(sMensagem + " A exclusão não é permitida!!!");
            return bPossuiMovimentacao;
        }

        protected override bool PrevalidaAlt()
        {
            RegistroAtual["DT_ATUCAD"] = DateTime.Now.Date;
            RegistroAtual["CD_USUALT"] = oTabelas.CdUsuarioAtual;
            return true;
        }

        protected override bool PrevalidaCad()
        {

            RegistroAtual["DT_CAD"] = DateTime.Now.Date;
            RegistroAtual["CD_USUINC"] = oTabelas.CdUsuarioAtual;
            DataColumnCollection Colunas = EstruturaDataTable.Columns;
            RegistroAtual["ST_PESSOAJ"] = "S";
            RegistroAtual["ST_INATIVO"] = "N";
            if (Colunas.Contains("ST_ZFMALC"))
                RegistroAtual["ST_ZFMALC"] = "N";
            if (Colunas.Contains("CD_UFNOR"))
                RegistroAtual["CD_UFNOR"] = "SP";
            if (Colunas.Contains("CD_UFCOM"))
                RegistroAtual["CD_UFCOM"] = "SP";
            if (Colunas.Contains("CD_UFCOB"))
                RegistroAtual["CD_UFCOB"] = "SP";
            if (Colunas.Contains("ST_SPC"))
                RegistroAtual["ST_SPC"] = "N";
            if (Colunas.Contains("ST_SCI"))
                RegistroAtual["ST_SCI"] = "S";
            if (Colunas.Contains("ST_ACOMER"))
                RegistroAtual["ST_ACOMER"] = "N";
            if (Colunas.Contains("ST_CONSFINAL"))
                RegistroAtual["ST_CONSFINAL"] = "S";
            if (Colunas.Contains("ST_CONSUMOSUBST"))
                RegistroAtual["ST_CONSUMOSUBST"] = "S";
            if (Colunas.Contains("ST_CONTRIB"))
                RegistroAtual["ST_CONTRIB"] = "S";
            if (Colunas.Contains("ST_DESCSUFRAMA"))
                RegistroAtual["ST_DESCSUFRAMA"] = "N";
            if (Colunas.Contains("ST_ZERAICMS"))
                RegistroAtual["ST_ZERAICMS"] = "N";
            if (Colunas.Contains("ST_ZERAIPI"))
                RegistroAtual["ST_ZERAIPI"] = "N";
            if (Colunas.Contains("CD_PAIS"))
                RegistroAtual["CD_PAIS"] = "1058";
            if (Colunas.Contains("CD_CATEGO"))
                RegistroAtual["CD_CATEGO"] = BuscaCategoria();
            return true;
        }

        private bool ExisteOutroClienteMesmoCNPJCPF()
        {
            string sValor;
            string sCampo;
            if (RegistroAtual["ST_PESSOAJ"].ToString().Equals("S"))
            {
                sCampo = "CD_CGC";
                sValor = RegistroAtual["CD_CGC"].ToString();
            }
            else
            {
                sCampo = "CD_CPF";
                sValor = RegistroAtual["CD_CPF"].ToString();
            }
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append("(" + sCampo + " = '" + sValor + "')");
            if (this.Status != StatusRegistro.IncluindoRegistro)
            {
                strWhere.Append(" AND (CD_CLIFOR <> '" +
                    RegistroAtual["CD_CLIFOR"].ToString() + "')");
            }
            DataTable qryClifor =
                oTabelas.hlpDbFuncoes.qrySeekRet("CLIFOR", sCampo,
                strWhere.ToString());

            bool bExisteOutroRegistro = false;

            if (this.Status == StatusRegistro.AlterandoRegistro && qryClifor.Rows.Count > 1)
            {
                bExisteOutroRegistro = true;
            }
            else if (this.Status == StatusRegistro.IncluindoRegistro && qryClifor.Rows.Count > 0)
            {
                bExisteOutroRegistro = true;
            }

            if (bExisteOutroRegistro)
            {
                this.SetErro("Já existe outro Cliente / Fornecedor " +
                    "com a mesma numeração de CNPJ / CPF!!!");
            }
            return bExisteOutroRegistro;
        }

        protected override bool PosvalidaCad()
        {
            bool bRetorno = true;
            bool bUsandoWeb = (oTabelas.GetPlataformaUtilizada() ==
                PlataformaUtilizada.Web);
            //Verificar depois como ficará em Windows Forms como ficará a
            //consistência de CNPJ / CPF, uma vez que nos sistemas Desktop
            //é efetuada uma pergunta antes de checar se existe outro cliente
            //com o mesmo CNPJ / CPF.
            if (bUsandoWeb)
            {
                if (ExisteOutroClienteMesmoCNPJCPF())
                {
                    return false;
                }
            }
            if (!bRetorno)
                return false;
            DataColumnCollection Colunas = EstruturaDataTable.Columns;
            if (RegistroAtual["ST_PESSOAJ"].ToString().Equals("N"))
            {
                if (Colunas.Contains("ST_SIMPLES"))
                    RegistroAtual["ST_SIMPLES"] = "N";
                if (Colunas.Contains("ST_SIMPAUL"))
                    RegistroAtual["ST_SIMPAUL"] = "N";
                if (Colunas.Contains("ST_ATACADISTA"))
                    RegistroAtual["ST_ATACADISTA"] = "N";
            }
            if (bUsandoWeb)
            {
                if (Colunas.Contains("DS_ENDCOB"))
                    RegistroAtual["DS_ENDCOB"] = RegistroAtual["DS_ENDNOR"];
                if (Colunas.Contains("NM_BAIBROCOB"))
                    RegistroAtual["NM_BAIBROCOB"] = RegistroAtual["NM_BAIRRONOR"];
                if (Colunas.Contains("NM_CIDCOB"))
                    RegistroAtual["NM_CIDCOB"] = RegistroAtual["NM_CIDNOR"];
                if (Colunas.Contains("CD_UFCOB"))
                    RegistroAtual["CD_UFCOB"] = RegistroAtual["CD_UFNOR"];
                if (Colunas.Contains("CD_CEPCOB"))
                    RegistroAtual["CD_CEPCOB"] = RegistroAtual["CD_CEPNOR"];
                if (Colunas.Contains("CD_FONECOB"))
                    RegistroAtual["CD_FONECOB"] = RegistroAtual["CD_FONENOR"];
                if (Colunas.Contains("CD_FAXCOB"))
                    RegistroAtual["CD_FAXCOB"] = RegistroAtual["CD_FAXNOR"];
                if (Colunas.Contains("CD_CXPCOB"))
                    RegistroAtual["CD_CXPCOB"] = RegistroAtual["CD_CXPNOR"];
                if (Colunas.Contains("DS_CONCOBR"))
                    RegistroAtual["DS_CONCOBR"] = RegistroAtual["DS_CONTATO"];
            }
            if (Colunas.Contains("ST_CONTRIB"))
            {
                bool bIsento = true;

                if (RegistroAtual["CD_INSEST"] != null)
                {
                    string sCdInsest = RegistroAtual["CD_INSEST"].ToString();
                    bIsento = ((sCdInsest.Equals(String.Empty)) &&
                        (sCdInsest.Equals("ISENTO")));

                    //Espeficio para Torcetex.

                    //for (int i = 0; i < sCdInsest.Length; i++)
                    //{
                    //    if ((sCdInsest.Substring(i, 1) == "/") ||
                    //        (sCdInsest.Substring(i, 1) == "."))
                    //    {
                    //        this.SetErro("Campo foi preenchido incorretamente: " +
                    //                     "campo Inscrição Estadual permite somente números");

                    //        return false;
                    //    }
                    //}

                }
                if (bIsento)
                    RegistroAtual["ST_CONTRIB"] = "S";
                else
                {
                    RegistroAtual["ST_CONTRIB"] = "N";
                }

            }
            return true;
        }

        private string BuscaCategoria()
        {
            string CATE = oTabelas.hlpDbFuncoes.qrySeekRet("SELECT FIRST 1 CD_CATEGO FROM CATEGCLF").Rows[0][0].ToString();
            return CATE;
        }

    }
}
