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
using Presidencia.Control_Patrimonial_Reporte_Licencias.Negocio;

/// <summary>
/// Summary description for Cls_Rpt_Pat_Com_Licencias_Vencidas_Datos
/// </summary>

namespace Presidencia.Control_Patrimonial_Reporte_Licencias.Datos {

    public class Cls_Rpt_Pat_Com_Licencias_Vencidas_Datos {

         #region "Metodos"

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Empleados
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un 
            ///                       DataTable sobre la consulta de Empleados.
            ///PARAMETROS           : 1. Paramentros_Negocio.    Contiene los parametros que se 
            ///                         van a utilizar para hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           :16/Julio/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Empleados(Cls_Rpt_Pat_Com_Licencias_Vencidas_Negocio Parametros_Negocio) {
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
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Licencia + " AS TIPO_LICENCIA";
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Vencimiento_Licencia + " AS FECHA_VENCIMIENTO_LICENCIA";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Estatus + " = 'ACTIVO'";
                    if (Parametros_Negocio.P_Busqueda_RFC != null) {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + " LIKE '%" + Parametros_Negocio.P_Busqueda_RFC + "%'";
                    }
                    if (Parametros_Negocio.P_Busqueda_No_Empleado != null) {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " = '" + Convertir_A_Formato_ID(Convert.ToInt32(Parametros_Negocio.P_Busqueda_No_Empleado.Trim()), 6) + "'";
                    }
                    if (Parametros_Negocio.P_Busqueda_Dependencia_ID != null) {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + " = '" + Parametros_Negocio.P_Busqueda_Dependencia_ID + "'";
                    }
                    if (Parametros_Negocio.P_Busqueda_Empleado_ID != null) {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " = '" + Parametros_Negocio.P_Busqueda_Empleado_ID + "'";
                    }
                    if (Parametros_Negocio.P_Tomar_Fecha_Inicial) {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Vencimiento_Licencia + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros_Negocio.P_Busqueda_Fecha_Inicial) + "'";
                    }
                    if (Parametros_Negocio.P_Tomar_Fecha_Final) {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Vencimiento_Licencia + " <= '" + String.Format("{0:dd/MM/yyyy}", Parametros_Negocio.P_Busqueda_Fecha_Final) + "'";
                    }
                    if (Parametros_Negocio.P_Busqueda_Con_Sin_Licencia != null) {
                        if (Parametros_Negocio.P_Busqueda_Con_Sin_Licencia.Trim().Equals("SIN_LICENCIA")) {
                            Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Licencia_Manejo + " IS NULL";
                        } else if (Parametros_Negocio.P_Busqueda_Con_Sin_Licencia.Trim().Equals("CON_LICENCIA")) {
                            Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Licencia_Manejo + " IS NOT NULL";                        
                        }
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
            ///FECHA_CREO           :16/Julio/2011
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
            ///NOMBRE DE LA FUNCIÓN: Convertir_A_Formato_ID
            ///DESCRIPCIÓN: Pasa un numero entero a Formato de ID.
            ///PARÁMETROS:     
            ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
            ///             2. Longitud_ID. Longitud que tendra el ID. 
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 10/Marzo/2010 
            ///MODIFICO             : 
            ///FECHA_MODIFICO       : 
            ///CAUSA_MODIFICACIÓN   : 
            ///*******************************************************************************
            private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID)
            {
                String Retornar = "";
                String Dato = "" + Dato_ID;
                for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++)
                {
                    Retornar = Retornar + "0";
                }
                Retornar = Retornar + Dato;
                return Retornar;
            }

        #endregion "Metodos"

    }
}