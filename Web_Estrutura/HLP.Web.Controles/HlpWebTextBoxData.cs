using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using HLP.Geral;
using HLP.Dados;

namespace HLP.Web.Controles
{

    public class HlpWebTextBoxData : TextBox, ITela
    {

        public object GetValor()
        {
            if (this.ValorValido())
                return Convert.ToDateTime(this.Text);
            else
                return null;
        }

        public void SetValor(object valor)
        {
            if (valor is DateTime)
                this.Text = ((DateTime)valor).ToString("dd/MM/yyyy");
            else
                this.Text = valor.ToString();
        }

        public bool ValorValido()
        {
            return HlpFuncoes.DataValida(this.Text.Trim());
        }


        public string RetornaStringDataSQL()
        {
            return HlpDbFuncoes.RetornaStringDataSQL(this.Text.Trim());
        }

        public DateTime ToDateTime()
        {
            return Convert.ToDateTime(this.Text);
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
