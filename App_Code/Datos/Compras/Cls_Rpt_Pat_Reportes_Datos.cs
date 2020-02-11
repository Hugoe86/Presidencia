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
using Presidencia.Control_Patrimonial_Reportes.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

/// <summary>
/// Summary description for Cls_Rpt_Pat_Reportes_Datos
/// </summary>

namespace Presidencia.Control_Patrimonial_Reportes.Datos {

    public class Cls_Rpt_Pat_Reportes_Datos {

        #region Metodos 

            public static DataSet Consultar_Datos_Reporte_Bienes_Muebles(Cls_Rpt_Pat_Reportes_Negocio Parametros) {
                DataSet Ds_Reporte_Bienes_Muebles = null;
                try {
                    String Mi_SQL = "SELECT " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS ";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre;
                    Mi_SQL = Mi_SQL + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre;
                    Mi_SQL = Mi_SQL + ", " + Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Nombre;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Descripcion;
                }catch(Exception Ex){
                    String Mensaje = "Error al intentar consultar los datos. Error [" + Ex.Message + "]";
                    throw new Exception(Mensaje);
                }
                return Ds_Reporte_Bienes_Muebles;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Reporte_Licencias_Vencidas
            ///DESCRIPCIÓN: Realiza una consulta a la base de datos para buscar informacion 
            ///sobre el los resguardantes vigentes con licencias vencidas.
            ///PARAMETROS: 
            ///             1.-Parametros.  Contiene los datos de filtro del reporte.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 19/Enero/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public static DataSet Consultar_Datos_Reporte_Licencias_Vencidas(Cls_Rpt_Pat_Reportes_Negocio Parametros) {
                DataSet Ds_Reporte_Licencias_Vencidas = null;
                try {
                    String Mi_SQL = "SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " AS EMPLEADO_ID, " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " AS APELLIDO_PATERNO, " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " AS APELLIDO_MATERNO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE, " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + " AS RFC, " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Licencia_Manejo + " AS NO_LICENCIA_MANEJO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Vencimiento_Licencia + " AS FECHA_VENCIMIENTO_LICENCIA";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + "";
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " IN ( ";
                    Mi_SQL = Mi_SQL + " SELECT DISTINCT " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + "";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'VEHICULO' ";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'";
                    if (Parametros.P_Responsable != null && Parametros.P_Responsable.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = '" + Parametros.P_Responsable + "' ";
                    }
                    Mi_SQL = Mi_SQL + ")"; 
                    if (Parametros.P_Dependencia != null && Parametros.P_Dependencia.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia + "' ";
                    }
                    if (Parametros.P_Tomar_Fecha_Inicio) {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Vencimiento_Licencia + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Inicio) + "' ";
                    }
                    if (Parametros.P_Tomar_Fecha_Fin) {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Vencimiento_Licencia + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Fin).AddDays(1).Date) + "' ";
                    }
                    Ds_Reporte_Licencias_Vencidas = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los datos. Error [" + Ex.Message + "]";
                    throw new Exception(Mensaje);
                }
                return Ds_Reporte_Licencias_Vencidas;                
            }



            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_DataTable
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
            ///PARAMETROS           : 
            ///                     1.  Parametros.   Contiene los parametros que se van a utilizar para
            ///                                     hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 19/Enero/2010 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_DataTable(Cls_Rpt_Pat_Reportes_Negocio Parametros)
            {
                String Mi_SQL = null;
                DataSet Ds_Reporte = null;
                DataTable Dt_Reporte = new DataTable();
                try
                {
                    if (Parametros.P_Tipo_DataTable.Equals("DEPENDENCIAS")) {
                        Mi_SQL = "SELECT " + Cat_Dependencias.Campo_Dependencia_ID + " AS DEPENDENCIA_ID, " + Cat_Dependencias.Campo_Nombre + " AS NOMBRE";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + " ORDER BY " + Cat_Dependencias.Campo_Nombre;
                    } else if (Parametros.P_Tipo_DataTable.Equals("EMPLEADOS_DEPENDENCIA")) {
                        Mi_SQL = "SELECT " + Cat_Empleados.Campo_Empleado_ID + " AS EMPLEADO_ID, " + Cat_Empleados.Campo_Apellido_Paterno + " ||' '|| " + Cat_Empleados.Campo_Apellido_Materno;
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Campo_Nombre + " AS NOMBRE FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia + "'" + " ORDER BY NOMBRE";
                    }
                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                        Ds_Reporte = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Reporte != null) {
                        Dt_Reporte = Ds_Reporte.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Reporte;
            }

        #endregion

    }

}