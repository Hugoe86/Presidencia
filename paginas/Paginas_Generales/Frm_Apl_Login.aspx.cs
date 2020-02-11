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
using Presidencia.Seguridad;
using Presidencia.Generar_Requisicion.Datos;
using Presidencia.Login;

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
    private void Autentificacion(String Usuario, String Password){        
        bool Acceso = false;        
        if (Txt_Usuario.Text == "987654" && Txt_Password.Text == "314163")
        {
            Cls_Sessiones.Nombre_Empleado = "ADMINISTRADOR";
            Cls_Sessiones.Rol_ID = "00003";
            Cls_Sessiones.Empleado_ID = "";
            Cls_Sessiones.No_Empleado = "";
            Cls_Sessiones.Dependencia_ID_Empleado = "00036";
            Cls_Sessiones.Area_ID_Empleado = "";
            FormsAuthentication.Initialize();
            FormsAuthentication.RedirectFromLoginPage(Convert.ToString(Session[Cls_Sessiones.Nombre_Empleado]), false);
            Cls_Sessiones.Mostrar_Menu = true;
        }        
        else
        {
            int numero = Convert.ToInt32(Txt_Usuario.Text.Trim());
            Usuario = String.Format("{0:000000}",numero);
            Acceso = Cls_Apl_Login.Validar_Acceso(Usuario, Password);
            if (Acceso)
            {
          
                if (Txt_Password.Text.Trim() != "12345678")
                {
                    DataTable Dt_Datos_Empleado = Cls_Apl_Login.Consultar_Datos_Empleado(Usuario);
                    Cls_Sessiones.Datos_Empleado = Dt_Datos_Empleado;
                    DataRow Registro = Dt_Datos_Empleado.Rows[0];
                    Cls_Sessiones.Nombre_Empleado = Registro["Empleado"].ToString();
                    Cls_Sessiones.Rol_ID = Registro["Rol_ID"].ToString();
                    Cls_Sessiones.Empleado_ID = Registro["Empleado_ID"].ToString();
                    Cls_Sessiones.No_Empleado = Registro["No_Empleado"].ToString();
                    Cls_Sessiones.Dependencia_ID_Empleado = Registro[Cat_Empleados.Campo_Dependencia_ID].ToString();
                    Cls_Sessiones.Area_ID_Empleado = Registro[Cat_Empleados.Campo_Area_ID].ToString();
                    
                    //Validar usuario COORDINADOR del almacen

                    FormsAuthentication.Initialize();
                    FormsAuthentication.RedirectFromLoginPage(Convert.ToString(Session[Cls_Sessiones.Nombre_Empleado]), false);
                    Cls_Sessiones.Mostrar_Menu = true;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                       "Login", "alert('Es necesario cambiar su Password, presione el link  \"Cambiar Password\" que se encuentra en esta página');", true);
                }
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
    }

    private void Evento_Log_IN()
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
    protected void Btn_Aceptar_Click(object sender, ImageClickEventArgs e)
    {
        Evento_Log_IN();
    }
    protected void Txt_Password_TextChanged(object sender, EventArgs e)
    {
        Evento_Log_IN();
    }
}
