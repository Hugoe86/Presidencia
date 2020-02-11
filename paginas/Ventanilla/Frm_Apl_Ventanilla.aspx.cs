using System;
using System.Web.UI;
using Presidencia.Sessiones;

public partial class paginas_Ventanilla_Frm_Apl_Ventanilla : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["Activa"] = true;//Variable para mantener la session activa.

        // si no hay un id de ciudadadano (no ha iniciado sesión), redireccionar a la página de login
        if (String.IsNullOrEmpty(Cls_Sessiones.Ciudadano_ID))
        {
            Response.Redirect("Frm_Apl_Login_Ventanilla.aspx");
        }

        //Cls_Sessiones.Mostrar_Menu = ;
       // Cls_Sessiones.Nombre_Empleado = "CIUDADANO";
    }

    protected void Btn_Portafolio_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Sessiones.Mostrar_Menu = true;
        Response.Redirect("../Ventanilla/Frm_Cat_Ven_Portafolio.aspx");
    }
    protected void Btn_Atencion_Ciudadana_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Sessiones.Mostrar_Menu = true;
        Response.Redirect("../Ventanilla/Frm_Ope_Ven_Registrar_Peticion.aspx");
    }
    protected void Btn_Tramites_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Sessiones.Mostrar_Menu = true;
        Response.Redirect("../Ventanilla/Frm_Ope_Ven_Lista_Tramites.aspx");
    }
    protected void Btn_Consultar_Tramites_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Sessiones.Mostrar_Menu = true;
        Response.Redirect("../Ventanilla/Frm_Rpt_Ven_Consultar_Tramites.aspx");
    }
    
}
