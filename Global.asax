<%@ Application Language="C#" %>

<script runat="server">
    void Application_Start(object sender, EventArgs e) 
    {
        // se activa cuando la primera instancia de la clase HttpApplication se crea
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Se activa cuando la última instancia de la clase HttpApplication se destruye
    }
    void Session_Start(object sender, EventArgs e) 
    {
        // Se activa cuando un nuevo usuario visita el sitio web de la aplicación.
    }
    void Session_End(object sender, EventArgs e) 
    {
        // Se activa cuando los tiempos de sesión de usuario a cabo
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
    }
    public void Application_Init(object sender, EventArgs e)
    {
        // Se activa cuando una aplicación se inicializa. Es invocado en todas las instancias de objeto de HttpApplication 
    }
    public void Application_Disposed(object sender, EventArgs e)
    {
        // Despedido justo antes de una aplicación se destruya. Este es el lugar ideal para la limpieza de los recursos.
    }

    void Application_OnStart(object sender, EventArgs e)
    {
        // se activa cuando la primera instancia de la clase HttpApplication se crea
    }

    void Application_OnEnd(object sender, EventArgs e)
    {
        //  Se activa cuando la última instancia de la clase HttpApplication se destruye
    }
    void Application_Error(object sender, EventArgs e)
    {
        //if (Server.GetLastError().InnerException != null) 
        //{
        //    //Se activa cuando una excepción no controlada se encountre en la aplicación
        //    String Pagina = Context.Request.Path.ToString();
        //    Server.Transfer("~/paginas/Paginas_Generales/Log_Errores.aspx?error=" + Server.GetLastError().InnerException.Message + "&Pagina=" + Pagina);
        //}
    }
    public void Application_BeginRequest(object sender, EventArgs e)
    {
        // Es el primer evento de despedida para una solicitud, que es la solicitud de otra página (URL) que introduce un usuario.
    }
    public void Application_AuthenticateRequest(object sender, EventArgs e)
    {
        // Se activa cuando el módulo de seguridad hasestablished la identidad del usuario actual como válido. En este punto, las credenciales del usuario han sido validados.
    }
    public void Application_AuthorizeReques(object sender, EventArgs e)
    {
        // Se activa cuando el módulo de seguridad a verificado que un usuario puede acceder a los recursos
    }
    public void Application_ResolveRequestCache(object sender, EventArgs e)
    {
        // Se activa cuando la framework ASP.NET completa una solicitud de autorización.
    }
    public void Application_AcquireRequestState(object sender, EventArgs e)
    {
        // Se activa cuando la framework ASP.NET recibe el estado actual (estado de sesión) en relación a la solicitud recurrente
    }
    public void Application_PreRequestHandlerExecute(object sender, EventArgs e)
    {
        // Despedido antes de la framework ASP.NET comienza a ejecutar un controlador de eventos como una página o servicio web.
    }
    public void Applcation_PreSendRequestHeaders(object sender, EventArgs e)
    {
        // Despedido antes de la framework ASP.NET envía cabeceras HTTP a un cliente solicitante (navegador).
    }
    public void Application_PreSendContent(object sender, EventArgs e)
    {
        // Despedido antes de la página ASP.NET framework envie el contenido a un cliente solicitante (navegador).
    }

    public void Application_PostRequestHandlerExecute(object sender, EventArgs e)
    {
        // Se activa cuando la framework ASP.NET ha terminado la ejecución de un controlador de eventos.
        //HttpApplication app = sender as HttpApplication;
        //app.Context.Response.AddHeader("post", "yes");
    }

    public void Application_ReleaseRequestState(object sender, EventArgs e)
    {
        // Se activa cuando la framework ASP.NET completa la ejecución de todos los controladores de eventos. Este resultsin todos los módulos del Estado para salvar sus datos de estado actual
    }
    public void Application_UpdateRequestCache(object sender, EventArgs e)
    {
        //Se activa cuando la framework ASP.NET completa la ejecución de controlador para permitir el almacenamiento en caché las respuestas de los módulos tostore que se utilizarán para tramitar las solicitudes posteriores..
    }
    public void Application_EndRequest(object sender, EventArgs e)
    {
        //El último evento que disparó una peticion request
    }
    
   
    
</script>
