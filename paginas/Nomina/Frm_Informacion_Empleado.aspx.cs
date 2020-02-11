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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Empleados.Negocios;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Dependencias.Negocios;
using System.Text;
using Presidencia.Ayudante_JQuery;
using Presidencia.Prestamos.Negocio;
using Presidencia.Recibo_Pago.Negocio;
using System.Collections.Specialized;

public partial class paginas_Nomina_Frm_Informacion_Empleado : System.Web.UI.Page
{
    #region (Load/Init)
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Controlador_Peticiones_Cliente();
        }
    }
    #endregion

    #region (Controlador Peticiones)
    private void Controlador_Peticiones_Cliente()
    {
        String Str_Respuesta_JSON = String.Empty;
        String Str_Respuesta_Texto_Plano = String.Empty;
        String Opcion = String.Empty;
        String Tabla = String.Empty;
        String No_Empleado = String.Empty;

        try
        {
            //Obtenemos los valores de la petición.
            Obtener_Parametros(ref Opcion, ref Tabla, ref No_Empleado);

            //Limpiamos el objeto response.
            Response.Clear();

            switch (Opcion)
            {
                case "consultar_empleados":
                    Str_Respuesta_JSON = JSON_Consultar_Informacion_Empleado(Tabla, No_Empleado);
                    break;
                default:
                    break;
            }

            Response.ContentType = "application/json";
            Response.Write(Str_Respuesta_JSON);
            Response.Flush();
            Response.Close();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error lanzado en el controlador de peticiones del cliente. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Métodos Consulta)JSON_Consultar_Informacion_Empleado
    private String JSON_Consultar_Informacion_Empleado(String Tabla, String No_Empleado)
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();
        DataTable Dt_Empleados = null;

        try
        {
            if (IsNumeric(No_Empleado))
            {
                if (No_Empleado.Length > 6)
                    Obj_Empleados.P_No_Empleado = String.Format("{0:000000}", Convert.ToInt32(No_Empleado.Substring(0, 5)));
                else
                    Obj_Empleados.P_No_Empleado = String.Format("{0:000000}", Convert.ToInt32(No_Empleado));
            }
            else
                Obj_Empleados.P_Nombre = No_Empleado;

            Dt_Empleados = Obj_Empleados.JSON_Consulta_Empleados_General();
            Dt_Empleados.TableName = Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar la información del empleado. Error: [" + Ex.Message + "]");
        }
        return Presidencia.Ayudante_JQuery.Ayudante_JQuery.Crear_Tabla_Formato_JSON(Dt_Empleados);
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 18/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean IsNumeric(String Cadena)
    {
        Boolean Resultado = true;
        Char[] Array = Cadena.ToCharArray();
        try
        {
            for (int index = 0; index < Array.Length; index++)
            {
                if (!Char.IsDigit(Array[index])) return false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Validar si es un dato numerico. Error [" + Ex.Message + "]");
        }
        return Resultado;
    }
    #endregion

    #region (Métodos Generales)
    private void Obtener_Parametros(
        ref String Opcion,
        ref String Tabla,
        ref String No_Empleado
    )
    {
        if (!String.IsNullOrEmpty(Request.QueryString["opcion"]))
            Opcion = Request.QueryString["opcion"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["tabla"]))
            Tabla = Request.QueryString["tabla"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["no_empleado"]))
            No_Empleado = Request.QueryString["no_empleado"].ToString().Trim();
    }
    #endregion
}
