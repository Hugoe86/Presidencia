using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Presidencia.Sessiones;

public partial class paginas_Paginas_Generales_MasterPage_Peritos_Externos : System.Web.UI.MasterPage
{
    public string m_strTitulo;

    /********************************************************************************************************
     * NOMBRE_FUNCIÓN: Do_Asignar_Estilo
     * DESCRIPCIÓN: Asigan el estilo del masterpage 
     * PARÁMETROS:
                1. p_intTipo, Estilo a aplicar
     * CREO: Alberto Pantoja Hernández
     * FECHA_CREO:27/07/10
     * MODIFICÓ:
     * FECHA_MODIFICÓ:
     * CAUSA_MODIFICACIÓN: 
     *********************************************************************************************************/
    public void Do_Asignar_Estilo(int p_intTipo)
    {

        switch (p_intTipo)
        {
            /*Ocultamos:
            * Div's de login
            * Imagen del login
            * Encabezado del login
            * Pie de pagina
            */
            case 1:
                principal.Style.Clear();
                principal.Style.Add("position", "absolute");
                principal.Style.Add("left", "50%");
                principal.Style.Add("height", "800px");
                principal.Style.Add("width", "1080px");//980
                principal.Style.Add("margin-left", "-540px");//-490
                principal.Style.Add("background-color", "white");

                encabezado.Visible = true;
                encabezado.Style.Clear();
                encabezado.Style.Add("position", "absolute");
                encabezado.Style.Add("width", "1080px");//980
                encabezado.Style.Add("height", "70px");
                encabezado.Style.Add("left", "0px");
                encabezado.Style.Add("top", "0px");
                encabezado.Style.Add("background-image", "url(../imagenes/master/encabezado.png)");
                encabezado.Style.Add("background-position", "center");
                encabezado.Style.Add("background-repeat", "no-repeat");

                barra.Visible = true;
                barra.Style.Clear();
                barra.Style.Add("position", "absolute");
                barra.Style.Add("width", "1080px");//980
                barra.Style.Add("height", "60px");
                barra.Style.Add("left", "0px");
                barra.Style.Add("top", "70px");
                barra.Style.Add("font-size", "small");
                barra.Style.Add("background-color", "gray");

                contenido_fondo.Visible = true;
                contenido_fondo.Style.Clear();
                contenido_fondo.Style.Add("position", "absolute");
                contenido_fondo.Style.Add("width", "860px");//760
                contenido_fondo.Style.Add("height", "auto");//contenido_fondo.Style.Add("height", "496
                contenido_fondo.Style.Add("left", "220px");
                contenido_fondo.Style.Add("top", "132px");
                contenido_fondo.Style.Add("font-size", "small");
                contenido_fondo.Style.Add("text-align", "justify");
                contenido_fondo.Style.Add("background-color", "white");
                contenido_fondo.Style.Add("overflow", "auto");
                contenido_fondo.Style.Add("float", "left");
                contenido_fondo.Style.Add("background-color", "white");

                pie.Visible = false;
                pie.Style.Clear();
                pie.Style.Add("position", "absolute");
                pie.Style.Add("width", "1080px");//980
                pie.Style.Add("height", "51px");
                pie.Style.Add("left", "0px");
                pie.Style.Add("top", "712px");
                pie.Style.Add("background-image", "url(../imagenes/master/pie_login2.jpg)");
                pie.Style.Add("background-position", "center");
                pie.Style.Add("background-repeat", "no-repeat");


                menu_flotante.Visible = true;
                menu_flotante.Style.Add("position", "absolute");
                menu_flotante.Style.Add("width", "217px");
                menu_flotante.Style.Add("height", "80%");
                menu_flotante.Style.Add("left", "0x");
                menu_flotante.Style.Add("top", "130px");
                menu_flotante.Style.Add("float", "left");
                menu_flotante.Style.Add("background-color", "ligth-gray");
                menu_flotante.Style.Add("overflow", "auto");
                menu_flotante.Style.Add("scrollbar-face-color", "#507CD1");
                menu_flotante.Style.Add("scrollbar-highlight-color", "#2F4E7D");
                menu_flotante.Style.Add("scrollbar-shadow-color", "#25406D");
                menu_flotante.Style.Add("scrollbar-3dlight-color", "#FFFFFF");
                menu_flotante.Style.Add("scrollbar-arrow-color", "#FFFFFF");
                menu_flotante.Style.Add("scrollbar-track-color", "#F0F8FF");
                menu_flotante.Style.Add("scrollbar-drakshadow-color", "#000000");

                break;
            case 2:
                principal.Style.Clear();
                principal.Style.Add("position", "absolute");
                principal.Style.Add("left", "50%");
                principal.Style.Add("top", "50%");
                principal.Style.Add("height", "542px");
                principal.Style.Add("margin-top", "-271px");
                principal.Style.Add("width", "980px");
                principal.Style.Add("margin-left", "-490px");
                principal.Style.Add("background-color", "white");

                //Ocultamos los div's de login
                encabezado_login.Visible = false;
                contenido_fondo_login.Style.Clear();
                contenido_fondo_login.Style.Add("position", "absolute");
                contenido_fondo_login.Style.Add("height", "491px");
                contenido_fondo_login.Style.Add("width", "980px");
                contenido_fondo_login.Style.Add("left", "0px");
                contenido_fondo_login.Style.Add("top", "18px");
                contenido_fondo_login.Style.Add("font-size", "small");
                contenido_fondo_login.Style.Add("background-image", "url(../imagenes/master/fondo_login3.bmp)");
                contenido_fondo_login.Style.Add("background-position", "center");
                contenido_fondo_login.Style.Add("background-repeat", "no-repeat");
                contenido_fondo_login.Style.Add("z-index", "4");

                pie_login.Style.Clear();
                pie_login.Style.Add("position", "absolute");
                pie_login.Style.Add("left", "0px");
                pie_login.Style.Add("top", "509px");
                pie_login.Style.Add("height", "51px");
                pie_login.Style.Add("width", "980px");
                pie_login.Style.Add("background-image", "url(../imagenes/master/pie_login_ciudadanos.png)");
                pie_login.Style.Add("background-position", "center");
                pie_login.Style.Add("background-repeat", "no-repeat");
                pie_login.Style.Add("z-index", "6");
                break;
        }
    }

    protected void Page_Load(object sendre, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Lbl_Fecha.Text = DateTime.Now.ToString("dd MMMM yyyy").ToUpper();

            btnSalir.CausesValidation = false;
            if (Convert.ToBoolean(Cls_Sessiones.Mostrar_Menu))
            {
                wuc_menu.Visible = true;
                Do_Asignar_Estilo(1);
                m_strTitulo = "SISTEMA INTEGRAL ADMINISTRATIVO GUBERNAMENTAL";
                // Lbl_Usuario.Text = ((DataTable)Cls_Sessiones.Datos_Proveedor).Rows[0]["COMPANIA"].ToString();
            }
            else
            {
                Do_Asignar_Estilo(2);
                //Asignamos el titulo correspondiente                
                m_strTitulo = "Sistema Integral Administrativo Gubernamental";
            }
        }
    }
    protected void btnSalir_Click(object sender, ImageClickEventArgs e)
    {
        Session.RemoveAll();
        FormsAuthentication.SignOut();
        //Redireccionamos a la pagina de login
        Response.Redirect("../Paginas_Generales/Frm_Apl_Login_Peritos_Externos.aspx");

    }
}
