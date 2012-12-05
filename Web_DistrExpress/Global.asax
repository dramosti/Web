<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Routing" %>
<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {

        // RegisterRoutes(RouteTable.Routes);
    }

    public static void RegisterRoutes(RouteCollection routes)
    {

        routes.MapPageRoute("Login", "Login/", "~/Login.aspx");

        routes.MapPageRoute("Home", "Home/", "~/Home.aspx");

        routes.MapPageRoute("CadastroCliente",
            "Clientes/{inclui}",
            "~/CadastroCliente.aspx");

    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started
        Session["IncluirClientePedido"] = false;
    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
