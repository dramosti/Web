using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using HLP.Geral;

namespace HLP.Web.Controles
{

    public class HlpWebTextBoxInteiro : TextBox, ITela
    {

        public object GetValor()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetValor(object valor)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool ValorValido()
        {
            return HlpFuncoes.InteiroValido(this.Text.Trim());
        }

        public int ToInteger()
        {
            return Convert.ToInt32(this.Text.Trim());
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
