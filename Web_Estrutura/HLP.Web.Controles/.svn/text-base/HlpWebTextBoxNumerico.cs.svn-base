using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using HLP.Geral;

namespace HLP.Web.Controles
{

    public class HlpWebTextBoxNumerico : TextBox, ITela
    {

        public object GetValor()
        {
            if (ValorValido())
            {
                Decimal valor = Convert.ToDecimal(this.Text);
                valor = Convert.ToDecimal(valor.ToString(Mascara));
                return valor;
            }
            else
                return null;
        }

        public void SetValor(object valor)
        {
            if (valor is Decimal)
                this.Text = ((Decimal)valor).ToString(this.Mascara);
            else
                this.Text = valor.ToString();
        }

        public bool ValorValido()
        {
            try
            {
                Convert.ToDouble(this.Text);
                return true;
            }
            catch
            {
                return false;
            }
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

        private string sMascara;

        public string Mascara
        {
            get
            {
                if ((sMascara != null) && (!sMascara.Equals(String.Empty)))
                    return sMascara;
                else
                    return "N2";
            }
            set
            {
                sMascara = value;
            }
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
