using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirebirdSql.Data.FirebirdClient;
using System.Configuration;
using HLP.Web;
using System.Data;

public partial class Ger_Aviso : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string sUser = UsuarioWeb.GetNomeUsuarioGestorConectado(Session);
            if (sUser == "")
            {
                Response.Redirect("~/Login.aspx");
            }
            CarregaRepresentantes();
        }

    }

    protected void CarregaRepresentantes()
    {
        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        DataTable dtRepres = objUsuario.oTabelas.hlpDbFuncoes.qrySeekRet("select v.cd_vend, v.nm_vend from vendedor v where v.st_acessa_web = 'S'");
        cboRepresentante.DataSource = dtRepres;
        cboRepresentante.DataBind();
    }


    protected void cboTipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cboTipo.SelectedIndex == 0)
        {
            cboRepresentante.Enabled = false;
        }
        else
        {
            cboRepresentante.Enabled = true;
        }
    }


    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        string strConn = ConfigurationManager.ConnectionStrings["ConnectionStringFB"].ConnectionString.ToString();
        FbConnection Conn = new FbConnection(strConn);
        FbCommand cmdInsertAviso = new FbCommand();

        UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
        if (!objUsuario.oTabelas.hlpDbFuncoes.VerificaExistenciaGenerator("AVISO_WEB"))
        {
            objUsuario.oTabelas.hlpDbFuncoes.CreateGenerator("AVISO_WEB", 0);
        }

        cmdInsertAviso.CommandText = "SELECT GEN_ID(AVISO_WEB, 1) FROM RDB$DATABASE";
        cmdInsertAviso.Connection = Conn;
        Conn.Open();

        string sCD_AVISO = "";
        FbDataReader dr = cmdInsertAviso.ExecuteReader();
        while (dr.Read())
        {
            sCD_AVISO = dr["GEN_ID"].ToString();
        }

        cmdInsertAviso = new FbCommand();
        cmdInsertAviso.Connection = Conn;
        cmdInsertAviso.CommandText = "INSERT INTO AVISO_WEB VALUES (@CD_AVISO, @CD_REPRESENTANTE, @DS_AVISO, @DS_TITULO, @DT_FINALAVISO, @ST_TIPOAVISO)";
        cmdInsertAviso.Parameters.Add("@CD_AVISO", sCD_AVISO.PadLeft(7, '0'));
        cmdInsertAviso.Parameters.Add("@CD_REPRESENTANTE", cboTipo.SelectedIndex == 1 ? cboRepresentante.SelectedValue : null);
        cmdInsertAviso.Parameters.Add("@DS_AVISO", txtAviso.Text);
        cmdInsertAviso.Parameters.Add("@DS_TITULO", txtTitulo.Text);
        cmdInsertAviso.Parameters.Add("@DT_FINALAVISO", txtDataFinal.Text.Replace("/", "."));
        cmdInsertAviso.Parameters.Add("@ST_TIPOAVISO", cboTipo.SelectedValue);


        int sRetorno = cmdInsertAviso.ExecuteNonQuery();


        if (sRetorno > 0)
        {
            MessageHLP.ShowPopUpMsg("Aviso cadastrado com sucesso!", this.Page);
            txtTitulo.Text = "";
            txtAviso.Text = "";
            txtDataFinal.Text = "";
        }
    }
}