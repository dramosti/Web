using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HLP.Dados;

public partial class Componentes_HlpTextBoxData2 : System.Web.UI.UserControl
{

    public DateTime GetValor()
    {
        return Convert.ToDateTime(this.TextBox1.Text);
    }

    public void SetValor(DateTime dtValor)
    {
        this.TextBox1.Text = dtValor.ToString("dd/MM/yyyy");
    }

    public string RetornaStringDataSQL()
    {
        return HlpDbFuncoes.RetornaStringDataSQL(this.TextBox1.Text.Trim());
    }

    public string Tabela { get; set; }
    public string Campo { get; set; }

    public bool bApenasLeitura
    {
        set { this.TextBox1.ReadOnly = value; }
    }

 
}