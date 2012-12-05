using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.SessionState;
using HLP.Geral;
using HLP.Dados;

namespace HLP.Web.Controles
{

    public class HlpWebDropDownList : DropDownList, ITela
    {

        private string sExpressaoSqlDadosConsulta = String.Empty;

        public string ExpressaoDados
        {
            get
            {
                return sExpressaoSqlDadosConsulta;
            }
            set
            {
                sExpressaoSqlDadosConsulta = value;
            }
        }

        public void CarregarValores(HttpSessionState Session, string sWhere = "")
        {
            if (this.DataSource == null)
            {
                string sDataTable = "DataTable" + this.ID;
                DataTable dtValores = (DataTable)Session[sDataTable];
                //if (dtValores == null)
                {
                    Tabela oTabelas = ((UsuarioWeb)Session["ObjetoUsuario"]).oTabelas;
                    StringBuilder strExpressao = new StringBuilder();
                    strExpressao.Append(sExpressaoSqlDadosConsulta);
                    strExpressao.Replace("<CD_EMPRESA>", "'" + oTabelas.sEmpresa + "'");
                    strExpressao.Replace("<CD_VEND>", "'" + oTabelas.CdVendedorAtual + "'");
                    if (sWhere != "")
                    {
                        strExpressao.Append(" Where " + sWhere);
                    }

                    dtValores = oTabelas.hlpDbFuncoes.qrySeekRet(strExpressao.ToString());
                    Session[sDataTable] = dtValores;
                }
                this.DataSource = dtValores;
                this.DataBind();
            }
        }

        public void LimparValores(HttpSessionState Session)
        {
            Session["DataTable" + this.ID] = null;
        }

        public object GetValor()
        {
            return this.SelectedValue;
        }

        public void SetValor(object valor)
        {
            if ((valor != null) && (!valor.ToString().Equals(String.Empty)))
                this.SelectedValue = valor.ToString();
        }

        public bool ValorValido()
        {
            string valor = this.SelectedValue;
            return ((valor != null) && (!valor.Equals(String.Empty)));
        }

        private string sCampo;

        public string Campo
        {
            get
            {
                return GetCampo();
            }
            set
            {
                SetCampo(value);
            }
        }

        public string GetCampo()
        {
            return sCampo;
        }

        public void SetCampo(string sCampo)
        {
            this.sCampo = sCampo;
        }

        private string sTabela;

        public string Tabela
        {
            get
            {
                return GetTabela();
            }
            set
            {
                SetTabela(value);
            }
        }

        public string GetTabela()
        {
            return sTabela;
        }

        public void SetTabela(string sTabela)
        {
            this.sTabela = sTabela;
        }

        private bool bApenasLeitura;

        public bool ApenasLeitura
        {
            get
            {
                return GetApenasLeitura();
            }
            set
            {
                SetApenasLeitura(value);
            }
        }

        public bool GetApenasLeitura()
        {
            return bApenasLeitura;
        }

        public void SetApenasLeitura(bool bApenasLeitura)
        {
            this.bApenasLeitura = bApenasLeitura;
        }

    }

}
