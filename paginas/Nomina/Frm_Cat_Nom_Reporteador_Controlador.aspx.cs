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
using Presidencia.Rpt_Cat_Nomina.Negocio;
using System.Text;
using Presidencia.Constantes;
using Presidencia.Ayudante_Excel;
using CarlosAg.ExcelXmlWriter;
using Presidencia.Ayudante_CarlosAG;
using Presidencia.Ayudante_JQuery;
using System.Collections.Specialized;

public partial class paginas_Nomina_Frm_Cat_Nom_Reporteador_Controlador : System.Web.UI.Page
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
    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Controlador_Peticiones_Cliente
    ///DESCRIPCIÓN          : Metodo para tener el control de las peticiones del cliente
    ///PROPIEDADES          :
    ///CREO                 : Juan Alberto Hernandez Negrete
    ///FECHA_CREO           : Noviembre/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    private void Controlador_Peticiones_Cliente()
    {
        String Str_Respuesta_JSON = String.Empty;
        String Str_Respuesta_Texto_Plano = String.Empty;
        String Opcion = String.Empty;
        String Tabla = String.Empty;
        String Pagina = String.Empty;
        String Filas = String.Empty;
        String Tabla_Seleccionada = String.Empty;

        try
        {
            //Obtenemos los valores de la petición.
            Obtener_Parametros(ref Opcion, ref Tabla, ref Filas, ref Pagina, ref Tabla_Seleccionada);

            //Limpiamos el objeto response.
            Response.Clear();

            switch (Opcion)
            {
                case "consultar_tablas_nomina":
                    Str_Respuesta_JSON = Consultar_Tablas_Nomina(Tabla);
                    break;
                case "consultar_campos_por_tabla":
                    Str_Respuesta_JSON = Consultar_Campos_Por_Tabla(Tabla, Tabla_Seleccionada);
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

    #region (Métodos Consulta)
    /// *************************************************************************************************************************
    /// Nombre Método: Consultar_Tablas_Nomina
    /// 
    /// Descripción: Método que consulta todas las tablas de nomina de tipo catalogo.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Diciembre/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    private String Consultar_Tablas_Nomina(String Tabla)
    {
        Cls_Rpt_Nom_Catalogos_Nomina_Negocio Obj_Catalogos_Nomina = new Cls_Rpt_Nom_Catalogos_Nomina_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Tablas_Catalogos_Nomina = null;//Variable que listara las tablas de tipo catalogo de nomina.

        try
        {
            Dt_Tablas_Catalogos_Nomina = Obj_Catalogos_Nomina.Consultar_Tablas_Nomina();
            Dt_Tablas_Catalogos_Nomina.TableName = Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las tablas de nómina. Error: [" + Ex.Message + "]");
        }
        return Ayudante_JQuery.Crear_Tabla_Formato_JSON(Dt_Tablas_Catalogos_Nomina);
    }
    /// *************************************************************************************************************************
    /// Nombre Método: Consultar_Campos_Por_Tabla
    /// 
    /// Descripción: Método que consulta todos los campos de la tabla seleccionada.
    /// 
    /// Parámetros: Tabla.- Tabla de la cuál se consultaran los campos.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Diciembre/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    private String Consultar_Campos_Por_Tabla(String Tabla, String Tabla_Seleccionada)
    {
        Cls_Rpt_Nom_Catalogos_Nomina_Negocio Obj_Catalogos_Nomina = new Cls_Rpt_Nom_Catalogos_Nomina_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Campos_Por_Tabla = null;//Variable que lista los campos de la tabla seleccionada.

        try
        {
            Obj_Catalogos_Nomina.P_Tabla = Tabla_Seleccionada;
            Dt_Campos_Por_Tabla = Obj_Catalogos_Nomina.Consultar_Campos_Por_Tabla();
            Dt_Campos_Por_Tabla.TableName = Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los campos de la tabla seleccionada. Error: [" + Ex.Message + "]");
        }
        return Ayudante_JQuery.Crear_Tabla_Formato_JSON(Dt_Campos_Por_Tabla);
    }
    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Obtener_Registros_Mostrar_Tabla
    ///DESCRIPCIÓN          : 
    ///PROPIEDADES          :
    ///CREO                 : Juan Alberto Hernandez Negrete
    ///FECHA_CREO           : Noviembre/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    private DataTable Obtener_Registros_Mostrar_Tabla(DataTable Dt_Tabla, Int32 Filas, Int32 Pagina)
    {
        Int32 Contador_Registros_Mostrar = 0;
        DataTable Dt_Segmento_Recibos_Registros_Mostrar = null;
        int Tope_Segmento_Registros = Filas * Pagina;

        try
        {
            Dt_Segmento_Recibos_Registros_Mostrar = Dt_Tabla.Clone();

            if (Dt_Tabla is DataTable)
            {
                if (Dt_Tabla.Rows.Count > 0)
                {
                    foreach (DataRow Registro in Dt_Tabla.Rows)
                    {
                        if (Registro is DataRow)
                        {
                            ++Contador_Registros_Mostrar;
                            if (Contador_Registros_Mostrar > (Tope_Segmento_Registros - Filas) && Contador_Registros_Mostrar <= Tope_Segmento_Registros)
                            {
                                Dt_Segmento_Recibos_Registros_Mostrar.ImportRow(Registro);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al paginar. Error: [" + Ex.Message + "]");
        }
        return Dt_Segmento_Recibos_Registros_Mostrar;
    }
    #endregion

    #region (Métodos Generales)
    //********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Obtener_Parametros
    ///DESCRIPCIÓN          : Metodo para obtener los parametros del las peticiones del cliente
    ///PROPIEDADES          :
    ///CREO                 : Juan Alberto Hernandez Negrete
    ///FECHA_CREO           : Noviembre/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    private void Obtener_Parametros(
        ref String Opcion,
        ref String Tabla,
        ref String Filas,
        ref String Pagina,
        ref String Tabla_Seleccionada
    )
    {
        NameValueCollection nvc = Request.Form;

        if (!String.IsNullOrEmpty(Request.QueryString["opcion"]))
            Opcion = Request.QueryString["opcion"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["tabla"]))
            Tabla = Request.QueryString["tabla"].ToString().Trim();

        if (!String.IsNullOrEmpty(nvc["page"]))
            Pagina = nvc["page"].ToString().Trim();

        if (!String.IsNullOrEmpty(nvc["rows"]))
            Filas = nvc["rows"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["tabla_seleccionada"]))
            Tabla_Seleccionada = Request.QueryString["tabla_seleccionada"].ToString().Trim();

    }
    #endregion
}
