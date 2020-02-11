using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Control_Patrimonial_Reporte_Movimientos.Negocio;

/// <summary>
/// Summary description for Cls_Rpt_Pat_Listado_Movimientos_Datos
/// </summary>

namespace Presidencia.Control_Patrimonial_Reporte_Movimientos.Datos {
    public class Cls_Rpt_Pat_Listado_Movimientos_Datos {

        #region "Metodos"

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Empleados
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un 
            ///                       DataTable sobre la consulta de Empleados.
            ///PARAMETROS           : 1. Paramentros_Negocio.    Contiene los parametros que se 
            ///                         van a utilizar para hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           :14/Julio/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Empleados(Cls_Rpt_Pat_Listado_Movimientos_Negocio Parametros_Negocio) {
                String Mi_SQL = null;
                DataSet Ds_Datos = null;
                DataTable Dt_Datos = new DataTable();
                Boolean Consulta_Entro_Where = false;
                try {
                    Mi_SQL = "SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " AS EMPLEADO_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " AS NO_EMPLEADO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + " AS RFC";
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                    Mi_SQL = Mi_SQL + " ||' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                    Mi_SQL = Mi_SQL + " ||' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE_COMPLETO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Licencia_Manejo + " AS NO_LICENCIA_MANEJO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Vencimiento_Licencia + " AS FECHA_VENCIMIENTO_LICENCIA";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Estatus + " = 'ACTIVO'";
                    if (Parametros_Negocio.P_Busqueda_Dependencia_ID != null) {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + " = '" + Parametros_Negocio.P_Busqueda_Dependencia_ID + "'";
                    }
                    if (Parametros_Negocio.P_Busqueda_Empleado_ID != null) {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " = '" + Parametros_Negocio.P_Busqueda_Empleado_ID + "'";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno;
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno;
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre;
                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                        Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Datos != null) {
                        Dt_Datos = Ds_Datos.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Datos;                
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Dependencias
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un 
            ///                       DataTable sobre la consulta de Dependencias.
            ///PARAMETROS           : .
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           :14/Julio/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Dependencias() {
                String Mi_SQL = null;
                DataSet Ds_Datos = null;
                DataTable Dt_Datos = new DataTable();
                Boolean Consulta_Entro_Where = false;
                try {
                    Mi_SQL = "SELECT " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " AS DEPENDENCIA_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + "";
                    Mi_SQL = Mi_SQL + " ||' - '|| " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Estatus + " = 'ACTIVO'";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + "";
                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                        Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Datos != null) {
                        Dt_Datos = Ds_Datos.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Datos;                
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Registros_Bienes_Muebles
            ///DESCRIPCIÓN          : 
            ///PARAMETROS           : .
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 18/Julio/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Registros_Altas(Cls_Rpt_Pat_Listado_Movimientos_Negocio Parametros_Negocio) { 
                String Mi_SQL = null;
                DataSet Ds_Datos = null;
                DataTable Dt_Datos = new DataTable();
                //Dt_Datos.Columns.Add("MOVIMIENTO", Type.GetType("System.String"));
                //Dt_Datos.Columns.Add("FECHA", Type.GetType("System.String"));
                //Dt_Datos.Columns.Add("CANTIDAD", Type.GetType("System.String"));
                //Dt_Datos.Columns.Add("TIPO_BIEN", Type.GetType("System.String"));
                //Dt_Datos.Columns.Add("NO_INVENTARIO", Type.GetType("System.Int64"));
                //Dt_Datos.Columns.Add("CARACTERISTICAS", Type.GetType("System.String"));
                //Dt_Datos.Columns.Add("CONDICIONES", Type.GetType("System.String"));
                //Dt_Datos.Columns.Add("DEPENDENCIA", Type.GetType("System.String"));
                //Dt_Datos.Columns.Add("RESPONSABLE", Type.GetType("System.String"));
                //Dt_Datos.Columns.Add("COSTO", Type.GetType("System.Double"));
                //Dt_Datos.Columns.Add("PROVEEDOR", Type.GetType("System.String"));
                //Dt_Datos.Columns.Add("OBSERVACIONES", Type.GetType("System.String"));
                //Dt_Datos.Columns.Add("NO_FACTURA", Type.GetType("System.String"));

                DataTable Dt_Temporal = new DataTable();
                try {
                    Mi_SQL = "SELECT 'ALTA' AS MOVIMIENTO";
                    Mi_SQL = Mi_SQL + ", TO_CHAR(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + ",'DD/MON/YYYY') AS FECHA";
                    Mi_SQL = Mi_SQL + ", '1' AS CANTIDAD";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " AS TIPO_BIEN";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + " AS NO_INVENTARIO";
                    Mi_SQL = Mi_SQL + ", (" + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre;
                    Mi_SQL = Mi_SQL + "  ||', '|| " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Nombre;
                    Mi_SQL = Mi_SQL + "  ||', '|| " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion;
                    Mi_SQL = Mi_SQL + "  ||', '|| " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie;
                    Mi_SQL = Mi_SQL + "  ||', '|| " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Descripcion + ") AS CARACTERISTICAS";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estado + " AS CONDICIONES";
                    Mi_SQL = Mi_SQL + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + " AS IMPORTE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Factura + " AS FACTURA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Observadores + " AS OBSERVACIONES";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + ", " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + ", " + Cat_Dependencias.Tabla_Cat_Dependencias;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID;
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Modelo_ID;
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Color_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID;
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Material_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Material_ID;
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID;
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estatus;
                    Mi_SQL = Mi_SQL + " = 'VIGENTE'";
                    if (Parametros_Negocio.P_Busqueda_Dependencia_ID != null && Parametros_Negocio.P_Busqueda_Dependencia_ID.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " = '" + Parametros_Negocio.P_Busqueda_Dependencia_ID + "'";
                    }
                    if (Parametros_Negocio.P_Busqueda_Empleado_ID != null && Parametros_Negocio.P_Busqueda_Empleado_ID.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " = '" + Parametros_Negocio.P_Busqueda_Dependencia_ID + "'";
                    }
                     if (Parametros_Negocio.P_Tomar_Fecha_Inicial) {
                         Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros_Negocio.P_Busqueda_Fecha_Inicial) + "'";
                    }
                    if (Parametros_Negocio.P_Tomar_Fecha_Final) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " <= '" + String.Format("{0:dd/MM/yyyy}", Parametros_Negocio.P_Busqueda_Fecha_Final) + "'";
                    }
                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                        Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Datos != null) {
                        Dt_Datos = Ds_Datos.Tables[0];
                    }
                    //if (Dt_Temporal != null && Dt_Temporal.Rows.Count > 0) {
                    //    for (Int32 Contador = 0; Contador < Dt_Temporal.Rows.Count; Contador++) { 
                        
                    //    }
                    //}
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Datos;                  
            }

        #endregion "Metodos"

    }
}