using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using HLP.Geral;

namespace HLP.Web.Controles
{
    
    public class HlpWebTextBox : TextBox, ITela
    {

        private bool bRetornaEmMaiusculo = true;
        private string sMascaraValidacao = String.Empty;

        public bool RetornaEmMaiusculo
        {
            get
            {
                return bRetornaEmMaiusculo;
            }
            set
            {
                bRetornaEmMaiusculo = value;
            }
        }

        public string MascaraValidacao
        {
            get
            {
                return sMascaraValidacao;
            }
            set
            {
                sMascaraValidacao = value.Trim();
            }
        }

        public object GetValor()
        {
            string sTexto = this.Text.Trim().Replace("'", " ");
            if (bRetornaEmMaiusculo)
                return sTexto.ToUpper();
            else
                return sTexto;
        }

        public void SetValor(object valor)
        {
            if (valor != null)
                this.Text = valor.ToString();
            else
                this.Text = String.Empty;
        }

        public bool ValorValido()
        {
            bool bValorValido = this.EmBranco();
            if (!bValorValido)
            {
                bValorValido = ((sMascaraValidacao == null) || 
                    (sMascaraValidacao.Equals(String.Empty)));
                if (!bValorValido)
                {
                    bValorValido = Regex.IsMatch(this.GetValor().ToString(),
                        sMascaraValidacao);
                }
            }
            return bValorValido;
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

        public bool EmBranco()
        {
            return (this.Text.Trim().Equals(String.Empty));
        }

    }

}