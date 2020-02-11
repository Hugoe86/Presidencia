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
using Presidencia.Bitacora_Eventos;
using Presidencia.Seguridad;
using Presidencia.Catalogo_Cat_Peritos_Externos.Negocio;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;

public partial class paginas_Paginas_Generales_Frm_Apl_Login_Peritos_Externos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
        if (!Page.IsPostBack)
        {
            //Colocamos el foco en el primer control
            Txt_Usuario.Focus();
            Session.RemoveAll();
            Hlk_Registro.NavigateUrl = "../../paginas/Catastro/Frm_Ope_Cat_Recepcion_Documentos_Perito_externo.aspx";
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
        DataTable Dt_Datos;                                                                      //Obtiene los datos de la consulta
        Cls_Cat_Cat_Peritos_Externos_Negocio Rs_Consulta_Cat_Empleados = new Cls_Cat_Cat_Peritos_Externos_Negocio(); //Variable para la conexión a la capa de Negocios

        try
        {
            Rs_Consulta_Cat_Empleados.P_Usuario = Txt_Usuario.Text.Trim();
            //Rs_Consulta_Cat_Empleados.P_Password = Cls_Seguridad.Encriptar(Txt_Password.Text.Trim());
            Rs_Consulta_Cat_Empleados.P_Password = Txt_Password.Text.Trim();
            Dt_Datos = Rs_Consulta_Cat_Empleados.Consultar_Peritos_Externos();

            //DataTable Dt_Datos_Empleado = Rs_Consulta_Cat_Empleados.Consulta_Datos_Empleado();
            if (Dt_Datos != null && Dt_Datos.Rows.Count > 0)
            {
                Cls_Sessiones.Datos_Empleado = Dt_Datos;
            }
            if (Dt_Datos.Rows.Count > 0)
            {
                foreach (DataRow Registro in Dt_Datos.Rows)
                {
                    Cls_Sessiones.Nombre_Empleado = Registro["PERITO_EXTERNO"].ToString();
                    Cls_Sessiones.Rol_ID = Obtener_Dato_Consulta();
                    Cls_Sessiones.Empleado_ID = Registro["PERITO_EXTERNO_ID"].ToString();
                    //Cls_Sessiones.No_Empleado = Registro["No_Empleado"].ToString();
                    //Cls_Sessiones.Dependencia_ID_Empleado = Registro[Cat_Empleados.Campo_Dependencia_ID].ToString();
                    //Cls_Sessiones.Area_ID_Empleado = Registro[Cat_Empleados.Campo_Area_ID].ToString();
                }
                FormsAuthentication.Initialize();
                FormsAuthentication.RedirectFromLoginPage(Convert.ToString(Session[Cls_Sessiones.Nombre_Empleado]), false);
                Cls_Sessiones.Mostrar_Menu = true;
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
            //ORA-12560:No hay conexion a internet
            //throw new Exception(ex.Message.ToString());
            String Mensaje = "Existe un problema con la conexión a la fuente de datos [" + Ex.ToString() + "]";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('" + Mensaje + "');", true);
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Componentes
    ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 24/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private String Obtener_Dato_Consulta()
    {
        String Mi_SQL;
        String Dato_Consulta = "";

        try
        {
            Mi_SQL = "SELECT " + Apl_Cat_Roles.Campo_Rol_ID + " FROM " + Apl_Cat_Roles.Tabla_Apl_Cat_Roles + " WHERE " + Apl_Cat_Roles.Campo_Nombre + " = 'Perito Externo'";

            OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Dr_Dato.Read())
            {
                if (Dr_Dato[0] != null)
                {
                    Dato_Consulta = Dr_Dato[0].ToString();
                }
                else
                {
                    Dato_Consulta = "";
                }
                Dr_Dato.Close();
            }
            else
            {
                Dato_Consulta = "";
            }
            Dr_Dato = null;
        }
        catch
        {
        }
        finally
        {
        }

        return Dato_Consulta;
    }

    protected void Hlk_Registro_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Frm_Ope_Cat_Recepcion_Documentos_Perito_externo.aspx");
        //NavigateUrl="..//paginas/Catastro/Frm_Ope_Cat_Recepcion_Documentos_Perito_externo.aspx" 
    }
}