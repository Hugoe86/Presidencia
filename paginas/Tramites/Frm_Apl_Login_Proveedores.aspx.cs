using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Roles.Negocio;
using System.Windows.Forms;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Empleados.Negocios;
using Presidencia.Bitacora_Eventos;
using Presidencia.Catalogo_Compras_Proveedores.Negocio;
public partial class paginas_Frm_Apl_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
        if (!Page.IsPostBack)
        {
            //Colocamos el foco en el primer control
            Txt_Usuario.Focus();
            Session.RemoveAll();            
        }
    }
    /****************************************************************************************
     NOMBRE DE LA FUNCION: Autentificacion
     DESCRIPCION : Verificar que el usuario y password sean validos en el sistema para poder
                   acceder al mismo
     PARAMETROS  : Login: Nombre de usuario
    '              Password: Contraseña
     CREO        : Yazmin A Delgado Gómez
     FECHA_CREO  : 14-Septiembre-2010
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACION:
    ****************************************************************************************/
    private void Autentificacion(String Login, String Password)
    {
        Cls_Cat_Com_Proveedores_Negocio Negocio_Proveedores =
            new Cls_Cat_Com_Proveedores_Negocio();
        try
        {
            Negocio_Proveedores.P_Usuario = Txt_Usuario.Text.Trim();
            Negocio_Proveedores.P_Password = Txt_Password.Text.Trim();

            DataTable Dt_Datos_Proveedor = Negocio_Proveedores.Validar_Proveedor();
            if (Dt_Datos_Proveedor != null && Dt_Datos_Proveedor.Rows.Count > 0)
            {
                Cls_Sessiones.Datos_Proveedor = Dt_Datos_Proveedor;
                Cls_Sessiones.Mostrar_Menu = true;
                Cls_Sessiones.Rol_ID = Dt_Datos_Proveedor.Rows[0]["ROL_ID"].ToString();
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal_Proveedores.aspx");
            }
            else
            {
                Txt_Usuario.Text = "";
                Txt_Password.Text = "";
                Txt_Usuario.Focus();
                Lbl_Mensaje.Text = "El Usuario y Password no son correctos, favor de verificar";
                Btn_Img_Mensaje.Style[HtmlTextWriterStyle.Visibility] = "Show";
                Txt_Usuario.Focus();
                return;
            }
        }
        catch (Exception Ex)
        {
            String Mensaje = "Existe un problema con la conexión a la fuente de datos [" + Ex.ToString() + "]";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('"+Mensaje+"');", true);
        }
    }
    protected void Btn_Aceptar_Click(object sender, ImageClickEventArgs e)
    {
        //Se limpia el label de de Error
        Lbl_Mensaje.Text = null;
        Btn_Img_Mensaje.Style[HtmlTextWriterStyle.Visibility] = "Hidden";
        //Verifica que el Login y Password no sean Nulos
        if (Txt_Usuario.Text != "" & Txt_Password.Text != "")
        {
            Autentificacion(Txt_Usuario.Text, Txt_Password.Text);
        }
        else
        {
            if (Txt_Usuario.Text.Trim() == String.Empty)
            {
                Lbl_Mensaje.Text = "Proporcione el Usuario para poder acceder al sistema";
                Btn_Img_Mensaje.Style[HtmlTextWriterStyle.Visibility] = "Show";
                Txt_Usuario.Focus();
                return;
            }
            if (Txt_Password.Text.Trim() == String.Empty)
            {
                Lbl_Mensaje.Text = "Proporcione la Contraseña para poder acceder al sistema";
                Btn_Img_Mensaje.Style[HtmlTextWriterStyle.Visibility] = "Show";
                Txt_Usuario.Focus();
                return;
            }
        }
    }
}
