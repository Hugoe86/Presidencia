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
using Presidencia.Ayudante_JQuery;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Dependencias.Negocios;
using Presidencia.Empleados.Negocios;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.Text.RegularExpressions;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class paginas_Nomina_Frm_Rpt_Nom_Controlador_Mov_Empl : System.Web.UI.Page
{
    static String URL_Pagina;

    #region (Init/Load)
    /// *************************************************************************************************************
    /// NOMBRE: Page_Load
    /// 
    /// DESCRIPCIÓN: Método que se ejecuta al cargar completamente la página.
    ///              
    /// PARÁMETROS: No Aplicá
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 16/Mayo/2011 17:45 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Opcion"]))
                Controlador_Peticiones(Request.QueryString["Opcion"]);
        }
    }
    #endregion

    #region (Controlador)
    /// *************************************************************************************************************
    /// NOMBRE: Controlador_Peticiones
    /// 
    /// DESCRIPCIÓN: Método que controla las peticiones de la página.
    ///              
    /// PARÁMETROS: Opcion.- Tipo de operación a realizar.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 16/Mayo/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************************************
    protected void Controlador_Peticiones(String Opcion)
    {
        String RESULTADO = String.Empty;                //Variable que almacena la cadena con formato JSON, que se devolvera a la página que realiza la petición.
        String Tabla = String.Empty;                    //Variable que almacena el nombre de la tabla.
        String Tipo_Movimiento = String.Empty;          //Variable que almacena el tipo de movimiento que se consultara de los empleados.
        String Tipo_Nomina = String.Empty;              // Variable que almacena el identificador del tipo de nómina.
        String Unidad_Responsable_ID = String.Empty;    //Variable que almacena el identificador de la unidad responsable.
        String Empleado_ID = String.Empty;              //Variable que almacena el identificador del empleado.
        String Fecha_Inicio = String.Empty;             //Variable que almacena la fecha de inicio de la búsqueda.
        String Fecha_Fin = String.Empty;                //Variable que almacena la fecha de fin de la búsqueda.

        try
        {
            //Obtenemos el nombre de la tabla.
            if (!String.IsNullOrEmpty(Request.QueryString["Tabla"]))
                Tabla = Request.QueryString["Tabla"];

            Response.Clear();

            switch (Opcion)
            {
                case "1":
                    Response.ContentType = "application/json";
                    RESULTADO = Ayudante_JQuery.Crear_Tabla_Formato_JSON(Consultar_Tipos_Nominas(Tabla));
                    break;
                case"2":
                    Response.ContentType = "application/json";
                    RESULTADO = Ayudante_JQuery.Crear_Tabla_Formato_JSON(Consultar_Unidades_Responsables(Tabla));
                    break;
                case "3":
                    Response.ContentType = "application/json";
                    Unidad_Responsable_ID = Request.QueryString["Unidad_Responsable"];
                    RESULTADO = Ayudante_JQuery.Crear_Tabla_Formato_JSON(Consultar_Empleados_Unidad_Responsable(Tabla, Unidad_Responsable_ID));
                    break;
                case "4":
                    //Generamos el reporte.
                    Tipo_Movimiento = Request.QueryString["Tipo_Movimiento"];
                    Tipo_Nomina = Request.QueryString["Tipo_Nomina"];
                    Unidad_Responsable_ID = Request.QueryString["Unidad_Responsable"];
                    Empleado_ID = Request.QueryString["Empleado"];
                    Fecha_Inicio = Request.QueryString["Fecha_Inicio"];
                    Fecha_Fin = Request.QueryString["Fecha_Fin"];

                    DataTable Dt_Movimientos = Consultar_Movimientos_Empleado(Tipo_Movimiento, Tipo_Nomina, Unidad_Responsable_ID, 
                        Empleado_ID, Fecha_Inicio, Fecha_Fin);
                    Ejecutar_Generacion_Reporte(Dt_Movimientos);

                    Response.Clear();
                    Response.ContentType = "application/text";
                    Response.Write(URL_Pagina);
                    Response.End();
                    return;
                    break;
                default:
                    break;
            }

            Response.Write(RESULTADO);
            Response.Flush();
            Response.Close();
        }
        catch (Exception Ex)
        {
            throw new Exception("Evento que controla las peticiones de la pagina. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Métodos)

    #region (Métodos Consulta)
    /// *************************************************************************************************************
    /// NOMBRE: Consultar_Tipos_Nominas
    /// 
    /// DESCRIPCIÓN: Consulta los tipos de nómina que existen actualmente registradas en sistema.
    ///              
    /// PARÁMETROS: Nombre_Tabla.- Nombre que se le dará a la tabla que almacenara los tipos de nómina.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 16/Mayo/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************************************
    protected DataTable Consultar_Tipos_Nominas(String Nombre_Tabla)
    {
        Cls_Cat_Tipos_Nominas_Negocio Obj_Tipos_Nomina = new Cls_Cat_Tipos_Nominas_Negocio();   //Variable de conexión con la capa de negocios.
        DataTable Dt_Tipos_Nomina = null;                                                       //Variable que almacena los tipos de nómina registradas en sistema.

        try
        {
            //Consultamos los tipos de nómina.
            Dt_Tipos_Nomina = Obj_Tipos_Nomina.Consulta_Tipos_Nominas();
            Dt_Tipos_Nomina.TableName = Nombre_Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los tipos de nomina en el sistema. Error: [" + Ex.Message + "]");
        }
        return Dt_Tipos_Nomina;
    }
    /// *************************************************************************************************************
    /// NOMBRE: Consultar_Unidades_Responsables
    /// 
    /// DESCRIPCIÓN: Consulta las Unidades Responsables que existen registradas actualmente en sistema.
    ///              
    /// PARÁMETROS: Nombre_Tabla.- Nombre que se le dará a la tabla que almacenara las unidades responsables.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 16/Mayo/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************************************
    protected DataTable Consultar_Unidades_Responsables(String Nombre_Tabla)
    {
        Cls_Cat_Dependencias_Negocio Obj_Unidades_Responsables = new Cls_Cat_Dependencias_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Unidades_Responsables = null;                                                  //Variable que almacenara la lista de unidades responsables.
        DataTable Dt_Resultado = new DataTable(Nombre_Tabla);                                       //Variable que almacenara las unidades responsables.
        String Dependencia_ID = String.Empty;                                                       //Variable que almacenara el identificador de la unidad responsable
        String Nombre = String.Empty;                                                               //Variable que almacenara el nombre de la unidad responsable.

        try
        {
            //Construimos la estructura de la tabla resultado.
            Dt_Resultado.Columns.Add( Cat_Dependencias.Campo_Dependencia_ID , typeof(String));
            Dt_Resultado.Columns.Add(Cat_Dependencias.Campo_Nombre, typeof(String));

            //Consultamos las unidades reesponsables en sistema.
            Dt_Unidades_Responsables = Obj_Unidades_Responsables.Consulta_Dependencias();

            if (Dt_Unidades_Responsables is DataTable)
            {
                foreach (DataRow UNIDAD_RESPONSABLE in Dt_Unidades_Responsables.Rows)
                {
                    if (UNIDAD_RESPONSABLE is DataRow)
                    {
                        if (!String.IsNullOrEmpty(UNIDAD_RESPONSABLE[Cat_Dependencias.Campo_Dependencia_ID].ToString()))
                            Dependencia_ID = UNIDAD_RESPONSABLE[Cat_Dependencias.Campo_Dependencia_ID].ToString();
                        if (!String.IsNullOrEmpty(UNIDAD_RESPONSABLE[Cat_Dependencias.Campo_Nombre].ToString()))
                            Nombre = UNIDAD_RESPONSABLE[Cat_Dependencias.Campo_Nombre].ToString();

                        DataRow Dr_Unidad_Responsable = Dt_Resultado.NewRow();
                        Dr_Unidad_Responsable[Cat_Dependencias.Campo_Dependencia_ID] = Dependencia_ID;
                        Dr_Unidad_Responsable[Cat_Dependencias.Campo_Nombre] = Nombre;
                        Dt_Resultado.Rows.Add(Dr_Unidad_Responsable);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las unidades responsables. Error: [" + Ex.Message + "]");
        }
        return Dt_Resultado;
    }
    /// *************************************************************************************************************
    /// NOMBRE: Consultar_Empleados_Unidad_Responsable
    /// 
    /// DESCRIPCIÓN: Método que consulta los empleados que pertecen a una determina unidad responsable.
    ///              
    /// PARÁMETROS: Nombre_Tabla.- Nombre de la tabla que almacenara los empleados que pertencen a una determinada
    ///                            Unidad Responsable.
    ///             Unidad_Responsable_ID.- Identificador de la unidad reponsable.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 16/Mayo/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************************************
    protected DataTable Consultar_Empleados_Unidad_Responsable(String Nombre_Tabla, String Unidad_Responsable_ID)
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
        DataTable Dt_Empleados = null;                                              //Variable que almacenara una lista de los empleados.
        DataTable Dt_Resultado = new DataTable(Nombre_Tabla);                       //Variable que almacenara la lista que se le pasara al método que crea la estructura JSON.
        String Empleado_ID = String.Empty;                                          //Identificador único del empleado.
        String Nombre = String.Empty;                                               //Nombre completo del empleado.

        try
        {
            //Creamos la estructura de la tabla de resultado.
            Dt_Resultado.Columns.Add(Cat_Empleados.Campo_Empleado_ID,typeof(String));
            Dt_Resultado.Columns.Add("EMPLEADO", typeof(String));

            //Consultamos los empleados por unidad responsable.
            Obj_Empleados.P_Dependencia_ID = Unidad_Responsable_ID;
            Dt_Empleados = Obj_Empleados.Consulta_Empleados_Dependencia();

            if (Dt_Empleados is DataTable) {
                foreach (DataRow EMPLEADO in Dt_Empleados.Rows) {
                    if (EMPLEADO is DataRow) {
                        if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString()))
                            Empleado_ID = EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString();

                        if (!String.IsNullOrEmpty(EMPLEADO["EMPLEADO"].ToString()))
                            Nombre = " " + EMPLEADO["EMPLEADO"].ToString();


                        DataRow Dr_EMPLEADO = Dt_Resultado.NewRow();
                        Dr_EMPLEADO[Cat_Empleados.Campo_Empleado_ID] = Empleado_ID;
                        Dr_EMPLEADO["EMPLEADO"] = Nombre;
                        Dt_Resultado.Rows.Add(Dr_EMPLEADO);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los empleados por unidad responsable. Error: [" + Ex.Message + "]");
        }
        return Dt_Resultado;
    }
    #endregion

    #region (Métodos Operación)
    /// *************************************************************************************************************
    /// NOMBRE: Consultar_Movimientos_Empleado
    /// 
    /// DESCRIPCIÓN: Método que consulta los movimientos que ha tenido el empleado. Ej. [ALTAS ó BAJAS]
    ///              
    /// PARÁMETROS: Tipo_Movimiento.- Se refiere si el movimiento a consultar serán las altas o bajas de los empleados. 
    ///             Tipo_Nomina.- El tipo de nómina al que pertence el empleado.
    ///             Unidad_Responsable.- Unidad Responsable a la que pertence el empleado.
    ///             Empleado_ID.- Identificador único del empleado.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 16/Mayo/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************************************
    protected DataTable Consultar_Movimientos_Empleado(String Tipo_Movimiento, String Tipo_Nomina,
        String Unidad_Responsable, String Empleado_ID, String Fecha_Inicio, String Fecha_Fin)
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexión a l capa de negocios.
        DataTable Dt_Movimientos_Empleado = null;//Variable que almacenara una lista delos movimientos de los empleados.

        try
        {
            /***
             * Obtenemos los filtros que aplicaran para la búsqueda.
             **/
            if (!String.IsNullOrEmpty(Tipo_Movimiento))
                Obj_Empleados.P_Tipo_Movimiento = Tipo_Movimiento;

            if (!String.IsNullOrEmpty(Tipo_Nomina))
                Obj_Empleados.P_Tipo_Nomina_ID = Tipo_Nomina;

            if (!String.IsNullOrEmpty(Unidad_Responsable))
                Obj_Empleados.P_Dependencia_ID = Unidad_Responsable;

            if (!String.IsNullOrEmpty(Empleado_ID))
                if (!Empleado_ID.Contains("undefined"))
                    Obj_Empleados.P_Empleado_ID = Empleado_ID;

            if (!String.IsNullOrEmpty(Fecha_Inicio))
                Obj_Empleados.P_Fecha_Inicio_Busqueda = Fecha_Inicio;

            if (!String.IsNullOrEmpty(Fecha_Fin))
                Obj_Empleados.P_Fecha_Fin_Busqueda = Fecha_Fin;

            //Consultamos los movimientos de los empleados.
            Dt_Movimientos_Empleado = Obj_Empleados.Consultar_Movimientos_Empleados();            
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los movimientos del empelado. Error: [" + Ex.Message + "]");
        }
        return Dt_Movimientos_Empleado;
    }
    /// *************************************************************************************
    /// NOMBRE: Ejecutar_Generacion_Reporte
    /// 
    /// DESCRIPCIÓN: Ejecuta la generacion del reporte de conceptos de los empleados.
    ///              
    /// PARÁMETROS: No Aplicá
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 9/Mayo/2011 17:45 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Ejecutar_Generacion_Reporte(DataTable Dt_Datos)
    {
        DataSet Ds_Datos = null;//Variable que almacenara los datos que serán pasados al reporte.

        try
        {
            if (Dt_Datos is DataTable)
            {
                if (Dt_Datos.Rows.Count > 0)
                {
                    Ds_Datos = new DataSet();                   //Instanciamos al DataSet que se le pasara ala reporte.
                    Dt_Datos.TableName = "Movimiento_Empleados";//Nombramos a la tabla que se le pasara al DataSet.
                    Ds_Datos.Tables.Add(Dt_Datos.Copy());       //Se agrega la tabla al DataSet.
                    //Se genera el reporte.
                    Generar_Reporte(ref Ds_Datos, "Cr_Rpt_Nom_Movimientos_Empleados.rpt", "Movimientos_Empleados" + Session.SessionID + ".pdf");
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al ejecutar la generación del reporte. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion

    #region (Reportes)
    /// *************************************************************************************
    /// NOMBRE: Generar_Reporte
    /// 
    /// DESCRIPCIÓN: Método que invoca la generación del reporte.
    ///              
    /// PARÁMETROS: Nombre_Plantilla_Reporte.- Nombre del archivo del Crystal Report.
    ///             Nombre_Reporte_Generar.- Nombre que tendrá el reporte generado.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 18:15 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Generar_Reporte(ref DataSet Ds_Datos, String Nombre_Plantilla_Reporte, String Nombre_Reporte_Generar)
    {
        ReportDocument Reporte = new ReportDocument();  //Variable de tipo reporte.
        String Ruta = String.Empty;                     //Variable que almacenara la ruta del archivo del crystal report. 

        try
        {
            Ruta = @Server.MapPath("../Rpt/Nomina/" + Nombre_Plantilla_Reporte);   //Obtenemos la ruta completa del reporte del crystal report.
            Reporte.Load(Ruta);                                                                     //Cargamos el archivo del crystal al objeto reporte.

            if (Ds_Datos is DataSet)
            {
                if (Ds_Datos.Tables.Count > 0)
                {
                    Reporte.SetDataSource(Ds_Datos);                        //Cargamos los datos al reporte.
                    Exportar_Reporte_PDF(Reporte, Nombre_Reporte_Generar);  //Exportamos el reporte a PDF.
                    Mostrar_Reporte(Nombre_Reporte_Generar);                //Obtenemos la URL que mostrara el reporte.
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Exportar_Reporte_PDF
    /// 
    /// DESCRIPCIÓN: Método que guarda el reporte generado en formato PDF en la ruta
    ///              especificada.
    ///              
    /// PARÁMETROS: Reporte.- Objeto de tipo documento que contiene el reporte a guardar.
    ///             Nombre_Reporte.- Nombre que se le dará al reporte.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 18:19 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Exportar_Reporte_PDF(ReportDocument Reporte, String Nombre_Reporte)
    {
        ExportOptions Opciones_Exportacion = new ExportOptions();
        DiskFileDestinationOptions Direccion_Guardar_Disco = new DiskFileDestinationOptions();
        PdfRtfWordFormatOptions Opciones_Formato_PDF = new PdfRtfWordFormatOptions();

        try
        {
            if (Reporte is ReportDocument)
            {
                Direccion_Guardar_Disco.DiskFileName = @Server.MapPath("../../Reporte/" + Nombre_Reporte);
                Opciones_Exportacion.ExportDestinationOptions = Direccion_Guardar_Disco;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                Reporte.Export(Opciones_Exportacion);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al exportar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Mostrar_Reporte
    /// 
    /// DESCRIPCIÓN: Muestra el reporte en pantalla.
    ///              
    /// PARÁMETROS: Nombre_Reporte.- Nombre que tiene el reporte que se mostrara en pantalla.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 18:20 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";//Variable que almacenara la URL de la página que muestra el reporte.

        try
        {
            Pagina = Pagina + Nombre_Reporte;//Completamos la URL que mostrara el reporte.
            URL_Pagina = Pagina;//Establecemos el valor de la variable estatica que almacenara la URL completa del reporte.
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    #endregion
}
