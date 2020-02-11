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
using System.Text;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Tipos_Nominas.Negocios;
using System.Text;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Empleados.Negocios;
using Presidencia.Sindicatos.Negocios;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Dependencias.Negocios;
using Presidencia.Areas.Negocios;
using System.Text.RegularExpressions;
using System.Data;
using Presidencia.Prestamos.Negocio;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;
using System.Text.RegularExpressions;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class paginas_Nomina_Frm_Nom_Controlador : System.Web.UI.Page
{
    static String URL_Pagina;

    protected void Page_Load(object sender, EventArgs e)
    {         
        if (!IsPostBack) {
            String Opcion = Request.QueryString["Opcion"];

            if (Opcion.Equals("4"))
            {
                String Nomina_ID = Request.QueryString["NOMINA_ID"];
                String No_Nomina = Request.QueryString["NO_NOMINA"];
                String Tipo_Nomina_ID = Request.QueryString["TIPO_NOMINA_ID"];
                DataTable Dt_Totales = Consultar_Tabla_Totales_Nomina(Nomina_ID, No_Nomina, Tipo_Nomina_ID);
                Ejecutar_Generacion_Reporte(Dt_Totales);

                Response.Clear();
                Response.Write(URL_Pagina);
                Response.End();
            }
            else
            {
                Ejecutar_Load_Pagina();
            }

        }
    }

    protected void Ejecutar_Load_Pagina()
    {
        String Tabla = String.Empty;
        String Texto = String.Empty;
        String Valor = String.Empty;
        String JSON_Tipos_Nomina = String.Empty;
        String Opcion = Request.QueryString["Opcion"];

        try
        {

            if (!String.IsNullOrEmpty(Request.QueryString["Tabla"]))
            {
                Tabla = Request.QueryString["Tabla"];

                if (!String.IsNullOrEmpty(Request.QueryString["Texto"]))
                {
                    Texto = Request.QueryString["Texto"];

                    if (!String.IsNullOrEmpty(Request.QueryString["Valor"]))
                    {
                        Valor = Request.QueryString["Valor"];

                        switch (Opcion)
                        {
                            case "1":
                                DataTable Dt_Tipos_Nominas = Consultar_Tipos_Nominas();
                                JSON_Tipos_Nomina = Cargar_DDL(Dt_Tipos_Nominas, Dt_Tipos_Nominas.Rows.Count, Tabla, Texto, Valor);
                                break;
                            case "2":
                                DataTable Dt_Calendario_Nominas = Consultar_Calendarios_Nomina();
                                JSON_Tipos_Nomina = Cargar_DDL(Dt_Calendario_Nominas, Dt_Calendario_Nominas.Rows.Count, Tabla, Texto, Valor);
                                break;
                            case "3":
                                String Nomina_ID = Request.QueryString["Nomina_ID"];
                                DataTable Dt_Periodo = Consultar_Periodos(Nomina_ID);
                                JSON_Tipos_Nomina = Cargar_DDL(Dt_Periodo, Dt_Periodo.Rows.Count, Tabla, Texto, Valor);
                                break;
                            default:
                                break;
                        }

                        Response.Clear();
                        Response.ContentType = "application/json";
                        Response.Write(JSON_Tipos_Nomina);
                        Response.Flush();
                        Response.Close();
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }

    private String Cargar_DDL(DataTable Data_Table, Int32 Total_Registros, String Nombre_Tabla,
                              String Campo_Texto, String Campo_Valor)
    {
        StringBuilder JSON = new StringBuilder(); //Cadena Resultado

        try
        {
            JSON.Append(@"{""TOTAL"":""" + Total_Registros + @""",");

            if (Data_Table is DataTable)
            {
                if (Data_Table.Rows.Count > 0)
                {
                    if (Total_Registros > 0)
                    {
                        JSON.Append(@"""" + Nombre_Tabla + @""":[");
                        foreach (DataRow Registro in Data_Table.Rows)
                        {

                            //Comienzo de la lectura de registros de la tabla de empleados
                            JSON.Append(@"{""" + Campo_Valor + @""":""" + Registro[Campo_Valor].ToString() + @""",");
                            JSON.Append(@"""" + Campo_Texto + @""":""" + Registro[Campo_Texto].ToString() + @"""");
                            JSON.Append("},");
                        }
                    }
                }
            }
            JSON = new StringBuilder(JSON.ToString().Remove(JSON.ToString().Length - 1));
            JSON.Append("]}");
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener la cadena de datos del tipo de nomina. Error: [" + ex.Message + "]");
        }
        return JSON.ToString();
    }

    protected DataTable Consultar_Tipos_Nominas()
    {
        Cls_Cat_Tipos_Nominas_Negocio Obj_Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Tipos_Nominas = null;

        try
        {
            Dt_Tipos_Nominas = Obj_Tipos_Nominas.Consulta_Tipos_Nominas();      
        }
        catch (Exception Ex)
        {

            throw new Exception("Error al consultar los tipos de nominas en el sistema. Error: [" + Ex.Message + "]");
        }
        return Dt_Tipos_Nominas;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Calendarios_Nomina
    /// DESCRIPCION : 
    /// 
    /// PARAMETROS:
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Consultar_Calendarios_Nomina()
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nominales = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Calendarios_Nominales = null;//Variable que almacena los calendarios nominales que existén actualmente en el sistema.
        try
        {
            Dt_Calendarios_Nominales = Obj_Calendario_Nominales.Consultar_Calendario_Nominas();
            Dt_Calendarios_Nominales = Formato_Fecha_Calendario_Nomina(Dt_Calendarios_Nominales);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los calendarios de nómina que existen actualmente registrados en el sistema. Error: [" + Ex.Message + "]");
        }
        return Dt_Calendarios_Nominales;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Formato_Fecha_Calendario_Nomina
    /// DESCRIPCION : Crea el DataTable con la consulta de las nomina vigentes en el 
    /// sistema.
    /// PARAMETROS: Dt_Calendario_Nominas.- Lista de las nominas vigentes actualmente 
    ///             en el sistema.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Formato_Fecha_Calendario_Nomina(DataTable Dt_Calendario_Nominas)
    {
        DataTable Dt_Nominas = new DataTable();
        DataRow Renglon_Dt_Clon = null;
        Dt_Nominas.Columns.Add("ANIO", typeof(System.String));
        Dt_Nominas.Columns.Add(Cat_Nom_Calendario_Nominas.Campo_Nomina_ID, typeof(System.String));

        if (Dt_Calendario_Nominas is DataTable)
        {
            foreach (DataRow Renglon in Dt_Calendario_Nominas.Rows)
            {
                if (Renglon is DataRow)
                {
                    Renglon_Dt_Clon = Dt_Nominas.NewRow();
                    Renglon_Dt_Clon["ANIO"] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin].ToString().Split(new char[] { ' ' })[0].Split(new char[] { '/' })[2];
                    Renglon_Dt_Clon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID];
                    Dt_Nominas.Rows.Add(Renglon_Dt_Clon);
                }
            }
        }
        return Dt_Nominas;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Periodos_Catorcenales_Nomina
    ///DESCRIPCIÓN: Consulta los periodos catorcenales para el 
    ///calendario de nomina seleccionado.
    ///PARAMETROS: Nomina_ID.- Indica el calendario de nomina del cuál se desea consultar
    ///                        los periodos catorcenales.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private DataTable Consultar_Periodos(String Nomina_ID)
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nomina_Periodos = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Clase de conexion con la capa de negocios
        DataTable Dt_Periodos_Catorcenales = null;//Variable que almacenra unaa lista de los periodos catorcenales que le correspónden a la nomina seleccionada.

        try
        {
            Consulta_Calendario_Nomina_Periodos.P_Nomina_ID = Nomina_ID;
            Dt_Periodos_Catorcenales = Consulta_Calendario_Nomina_Periodos.Consulta_Detalles_Nomina();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los periodos catorcenales del  calendario de nomina seleccionado. Error: [" + Ex.Message + "]");
        }
        return Dt_Periodos_Catorcenales;
    }


    protected DataTable Consultar_Tabla_Totales_Nomina(String Nomina_ID, String No_Nomina, String Tipo_Nomina_ID)
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();
        Cls_Cat_Nom_Percepciones_Deducciones_Business Obj_Conceptos = new Cls_Cat_Nom_Percepciones_Deducciones_Business();
        DataTable Dt_Totales_Nomina = null;
        String Percepcion_Deduccion_ID = String.Empty;
        String Nombre_Percepcion_Deduccion = String.Empty;
        String Tipo = String.Empty;
        String Clave = String.Empty;
        DataTable Dt_Totales = new DataTable("Totales_Generales_Nomina");
        DataTable Dt_Conceptos = null;

        try
        {
            Dt_Totales.Columns.Add("NO_TOTAL_NOMINA", typeof(String));
            Dt_Totales.Columns.Add("ANIO", typeof(String));
            Dt_Totales.Columns.Add("NO_NOMINA", typeof(String));
            Dt_Totales.Columns.Add("TIPO_NOMINA_ID", typeof(String));
            Dt_Totales.Columns.Add("CONCEPTO", typeof(String));
            Dt_Totales.Columns.Add("TIPO", typeof(String));
            Dt_Totales.Columns.Add("CLAVE", typeof(String));
            Dt_Totales.Columns.Add("MONTO", typeof(Double));

            Obj_Empleados.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
            Obj_Empleados.P_Nomina_ID = Nomina_ID;
            Obj_Empleados.P_No_Nomina = No_Nomina;
            Dt_Totales_Nomina = Obj_Empleados.Consultar_Rpt_Totales_Nomina();

            if (Dt_Totales_Nomina is DataTable)
            {
                if (Dt_Totales_Nomina.Rows.Count > 0)
                {
                    if (Dt_Totales_Nomina.Columns.Count > 0)
                    {
                        foreach (DataColumn COLUMNA in Dt_Totales_Nomina.Columns)
                        {
                            if (COLUMNA is DataColumn)
                            {
                                if (COLUMNA.ColumnName.Trim().Contains("SUMA_"))
                                {
                                    Percepcion_Deduccion_ID = COLUMNA.ColumnName.Replace("SUMA_", "");
                                    Dt_Conceptos = Obj_Conceptos.Busqueda_Percepcion_Deduccion_Por_ID(Percepcion_Deduccion_ID);

                                    if (Dt_Conceptos is DataTable)
                                    {
                                        foreach (DataRow CONCEPTO in Dt_Conceptos.Rows)
                                        {
                                            if (CONCEPTO is DataRow)
                                            {
                                                if (!String.IsNullOrEmpty(CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString()))
                                                {
                                                    if (String.IsNullOrEmpty(Dt_Totales_Nomina.Rows[0][COLUMNA.ColumnName].ToString()))
                                                        continue;
                                                    else if (Dt_Totales_Nomina.Rows[0][COLUMNA.ColumnName].ToString().Trim().Equals("0"))
                                                        continue;

                                                    Nombre_Percepcion_Deduccion = CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString();
                                                    Tipo = CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Tipo].ToString();
                                                    Clave = CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Clave].ToString();

                                                    DataRow Dr_Total = Dt_Totales.NewRow();
                                                    Dr_Total["NO_TOTAL_NOMINA"] = Dt_Totales_Nomina.Rows[0][Ope_Nom_Totales_Nomina.Campo_No_Total_Nomina].ToString();
                                                    Dr_Total["ANIO"] = Dt_Totales_Nomina.Rows[0][Cat_Nom_Calendario_Nominas.Campo_Anio].ToString();
                                                    Dr_Total["NO_NOMINA"] = Dt_Totales_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_No_Nomina].ToString();
                                                    Dr_Total["TIPO_NOMINA_ID"] = Dt_Totales_Nomina.Rows[0][Cat_Nom_Tipos_Nominas.Campo_Nomina].ToString();
                                                    Dr_Total["CONCEPTO"] = Nombre_Percepcion_Deduccion;
                                                    Dr_Total["TIPO"] = Tipo;
                                                    Dr_Total["CLAVE"] = Clave;
                                                    Dr_Total["MONTO"] = Convert.ToDouble(String.IsNullOrEmpty(Dt_Totales_Nomina.Rows[0][COLUMNA.ColumnName].ToString()) ? "0" : Dt_Totales_Nomina.Rows[0][COLUMNA.ColumnName].ToString().Trim());
                                                    Dt_Totales.Rows.Add(Dr_Total);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error en diseño al consultar los totales de nómina. Error: [" + Ex.Message + "]");
        }
        return Dt_Totales;
    }

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
        ReportDocument Reporte = new ReportDocument();//Variable de tipo reporte.
        String Ruta = String.Empty;//Variable que almacenara la ruta del archivo del crystal report. 

        try
        {
            Ruta = @Server.MapPath("../Rpt/Nomina/" + Nombre_Plantilla_Reporte);
            Reporte.Load(Ruta);

            if (Ds_Datos is DataSet)
            {
                if (Ds_Datos.Tables.Count > 0)
                {
                    Reporte.SetDataSource(Ds_Datos);
                    Exportar_Reporte_PDF(Reporte, Nombre_Reporte_Generar);
                    Mostrar_Reporte(Nombre_Reporte_Generar);
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
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            Pagina = Pagina + Nombre_Reporte;
            URL_Pagina = Pagina;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

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
        DataSet Ds_Datos = null;
        try
        {
            if (Dt_Datos is DataTable)
            {
                if (Dt_Datos.Rows.Count > 0)
                {
                    Ds_Datos = new DataSet();
                    Dt_Datos.TableName = "Totales_Generales_Nomina";
                    Ds_Datos.Tables.Add(Dt_Datos.Copy());
                    Generar_Reporte(ref Ds_Datos, "Cr_Rpt_Nom_Totales_Generales_Nomina.rpt", "Totales_Nomina" + Session.SessionID + ".pdf");
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al ejecutar la generación del reporte. Error: [" + Ex.Message + "]");
        }
    }

}
