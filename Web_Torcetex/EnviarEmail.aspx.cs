using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using System.Text;
using HLP.Web;
using System.IO;

public partial class EnviarEmail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string sUser = UsuarioWeb.GetNomeUsuarioConectado(Session);
            if (sUser == "")
            {
                Response.Redirect("~/Login.aspx");
            }

            UsuarioWeb objUsuario = (UsuarioWeb)Session["ObjetoUsuario"];
            lblRemetente.Text = objUsuario.NomeUsuario.Trim();
            string sPathAnexo = "";
            if (Request.QueryString["ANEXO"].ToString() != String.Empty)
            {
                sPathAnexo = Request.QueryString["ANEXO"].ToString();
                FileInfo finfo = new FileInfo(Server.MapPath("Pedidos//" + sPathAnexo) + ".pdf");
                if (finfo.Exists)
                {
                    lblNmArquivo.Text = finfo.Name;
                    Session["ANEXO"] = finfo;
                }
                txtDestino.Text = objUsuario.oTabelas.hlpDbFuncoes.qrySeekValue("pedido inner join clifor on pedido.cd_cliente = clifor.cd_clifor",
                        "coalesce(clifor.cd_emailweb,'')cd_emailweb",
                        string.Format("pedido.cd_pedido = '{0}' and pedido.cd_empresa='{1}'", sPathAnexo, objUsuario.oTabelas.sEmpresa));
                txtTitulo.Text = "Confirmação do seu Pedido Web " + sPathAnexo + ".";
            }

            
        }
    }

    protected string MontarCorpoEmail()
    {
        StringBuilder corpo = new StringBuilder();
        try
        {
            corpo.Append("<htm><body>");

            corpo.Append("<font face='Segoe UI' size='2'>Sr. Cliente, <br /><br />");
            corpo.Append("Este e-mail foi enviado por: " + lblRemetente.Text + "<br />");
            corpo.Append("Esta mensagem se refere ao Pedido de N°" + Request.QueryString["ANEXO"].ToString());
            corpo.Append(" que já se encontra em nosso servidor de dados. ");
            corpo.Append("<br />Segue anexo o pdf do pedido. " + Environment.NewLine + "</font><br /><br />");
            if (txtCorpo.Text != "")
            {
                corpo.Append("<font face='Segoe UI' size='2'>Observação: " + txtCorpo.Text + Environment.NewLine + "</font><br/><br/>");
            }
            corpo.Append("<I><font color = '#3B5998' size = 4>A " + ((UsuarioWeb)Session["ObjetoUsuario"]).oTabelas.CodigoCliente + " agradece pela Preferência!</font></I>");
            corpo.Append("<br /><br /><font color = " + "\"" + "Red" + "\"" + " size = 2>HLP - Estratégia em Software");
            corpo.Append("<br /><a href=" + "\"" + "http://www.hlp.com.br" + "\"" + ">www.hlp.com.br</a></font>");
            corpo.Append("</body></htm>");

        }
        catch (Exception x)
        {
            throw new Exception(x.Message);
        }
        return corpo.ToString();
    }

   
    protected void btnEnviar_Click(object sender, EventArgs e)
    {
        if (txtDestino.Text != "")
        {
            SmtpClient Cliente = new SmtpClient();
            Cliente.Host = "smtp.hlp.com.br";
            Cliente.Port = 587;

            MailAddress Destino = new MailAddress(txtDestino.Text);
            MailAddress Remeter = new MailAddress("pedidoweb@hlp.com.br");

            MailMessage email = new MailMessage(Remeter, Destino);
            email.Subject = txtTitulo.Text.Trim();
            email.SubjectEncoding = System.Text.Encoding.UTF8;
            email.IsBodyHtml = true;

            email.Body = MontarCorpoEmail();

            if ((Session["ANEXO"] as FileInfo).Exists)
            {
                Attachment Anexo = new Attachment((Session["ANEXO"] as FileInfo).FullName);
                email.Attachments.Add(Anexo);
            }

            NetworkCredential Credendical = new NetworkCredential("pedidoweb@hlp.com.br", "hlpmudar");
            Cliente.Credentials = Credendical;

            try
            {
                Cliente.Send(email);
                MultViewEmail.ActiveViewIndex = 1;
                lblInfo.Text = "E-mail enviado com sucesso";
            }
            catch (SmtpException ex)
            {
                MessageHLP.ShowPopUpMsg(ex.Message, this.Page);
            }
        }
    }
    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/ConsultaPedidos.aspx");
    }
}
