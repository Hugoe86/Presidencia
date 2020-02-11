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
using Presidencia.Reportes_Nomina.Prestamos.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Utilidades_Nomina;
using Presidencia.Reportes_Nomina.Formato_Pago_Proveedores.Negocio;

/// <summary>
/// Summary description for Cls_Rpt_Nom_Formato_Pago_Proveedores_Datos
/// </summary>

namespace Presidencia.Reportes_Nomina.Formato_Pago_Proveedores.Datos {

    public class Cls_Rpt_Nom_Formato_Pago_Proveedores_Datos {

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Dependencias_Activas
            ///DESCRIPCIÓN: Consulta las Dependendicas Activas
            ///PARAMETROS: 
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 09/Abril/2012
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public static DataTable Consultar_Dependencias_Activas()
            {
                DataTable Dt_Datos = new DataTable();
                DataSet Ds_Datos = null;
                String Mi_SQL = String.Empty;
                try
                {
                    Mi_SQL = "SELECT DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID + " AS DEPENDENCIA_ID";
                    Mi_SQL = Mi_SQL + ", (DEPENDENCIAS." + Cat_Dependencias.Campo_Clave + " ||' - '|| DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + ") AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
                    Mi_SQL = Mi_SQL + " WHERE DEPENDENCIAS." + Cat_Dependencias.Campo_Estatus + " = 'ACTIVO'";
                    Mi_SQL = Mi_SQL + " ORDER BY NOMBRE ASC";
                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Datos != null)
                    {
                        Dt_Datos = Ds_Datos.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Datos;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Proveedores_Activos
            ///DESCRIPCIÓN: Consulta los Proveedores Activos
            ///PARAMETROS: 
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 12/Abril/2012
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public static DataTable Consultar_Proveedores_Activos() { 
                DataTable Dt_Datos = new DataTable();
                DataSet Ds_Datos = null;
                String Mi_SQL = String.Empty;
                try {
                    Mi_SQL = "SELECT PROVEEDORES." + Cat_Nom_Proveedores.Campo_Proveedor_ID + " AS PROVEEDOR_ID";
                    Mi_SQL = Mi_SQL + ", (PROVEEDORES." + Cat_Nom_Proveedores.Campo_Nombre + ") AS NOMBRE_PROVEEDOR";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Nom_Proveedores.Tabla_Cat_Nom_Proveedores + " PROVEEDORES";
                    Mi_SQL = Mi_SQL + " WHERE PROVEEDORES." + Cat_Nom_Proveedores.Campo_Estatus + " = 'ACTIVO'";
                    Mi_SQL = Mi_SQL + " ORDER BY NOMBRE_PROVEEDOR ASC";
                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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
            ///NOMBRE DE LA FUNCIÓN : Consultar_Empleados_Resguardos
            ///DESCRIPCIÓN          : Obtiene empleados de la Base de Datos y los regresa en un 
            ///                       DataTable de acuerdo a los filtros pasados.
            ///PARAMETROS           : Parametros.  Contiene los parametros que se van a utilizar para
            ///                                     hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 12/Abril/2012
            ///MODIFICO             : 
            ///FECHA_MODIFICO       : 
            ///CAUSA_MODIFICACIÓN   : 
            ///*******************************************************************************
            public static DataTable Consultar_Empleados(Cls_Rpt_Nom_Formato_Pago_Proveedores_Negocio Parametros) {
                String Mi_SQL = null;
                DataSet Ds_Datos = null;
                DataTable Dt_Datos = new DataTable();
                Boolean Entro_Consulta_Where = false;
                try {
                    Mi_SQL = "SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " AS EMPLEADO_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " AS NO_EMPLEADO";
                    Mi_SQL = Mi_SQL + ", TRIM(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS NOMBRE";
                    Mi_SQL = Mi_SQL + ", TRIM(" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + "";
                    Mi_SQL = Mi_SQL + " ||' - '|| " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ") AS DEPENDENCIA";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias;
                    Mi_SQL = Mi_SQL + " ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID;
                    if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE "; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID.Trim() + "'";
                    }
                    if (Parametros.P_RFC_Empleado != null && Parametros.P_RFC_Empleado.Trim().Length > 0) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE "; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + " = '" + Parametros.P_RFC_Empleado.Trim() + "'";
                    }
                    if (Parametros.P_No_Empleado != null && Parametros.P_No_Empleado.Trim().Length > 0) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE "; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " = '" + Parametros.P_No_Empleado.Trim() + "'";
                    }
                    if (Parametros.P_Nombre_Empleado != null && Parametros.P_Nombre_Empleado.Trim().Length > 0) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE"; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND"; }
                        Mi_SQL = Mi_SQL + " (TRIM(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") LIKE '%" + Parametros.P_Nombre_Empleado.Trim() + "%'";
                        Mi_SQL = Mi_SQL + " OR TRIM(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ") LIKE '%" + Parametros.P_Nombre_Empleado.Trim() + "%')";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY NOMBRE";
                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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
            ///NOMBRE DE LA FUNCIÓN : Consultar_Monto_Pagar_Proveedor
            ///DESCRIPCIÓN          : Consulta el monto a pagar en al Proveedor
            ///PARAMETROS           : Parametros.  Contiene los parametros que se van a utilizar para
            ///                                     hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 12/Abril/2012
            ///MODIFICO             : 
            ///FECHA_MODIFICO       : 
            ///CAUSA_MODIFICACIÓN   : 
            ///*******************************************************************************
            public static DataTable Consultar_Monto_Pagar_Proveedor(Cls_Rpt_Nom_Formato_Pago_Proveedores_Negocio Parametros) { 
                String Mi_SQL = null;
                DataSet Ds_Datos = null;
                DataTable Dt_Datos = new DataTable();
                Boolean Entro_Consulta_Where = false;
                try {
                    Mi_SQL = "SELECT NVL(SUM(RECIBOS_DETALLES." + Ope_Nom_Recibos_Empleados_Det.Campo_Monto + "),0) AS MONTO";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + " RECIBOS_DETALLES";
                    if (!String.IsNullOrEmpty(Parametros.P_Nomina_ID)) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE "; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " RECIBOS_DETALLES." + Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo + " IN (";
                        Mi_SQL = Mi_SQL + " SELECT " + Ope_Nom_Recibos_Empleados.Campo_No_Recibo + " FROM " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + " WHERE " + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + " = '" + Parametros.P_Nomina_ID + "')";
                    }
                    if (!String.IsNullOrEmpty(Parametros.P_No_Nomina)) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE "; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " RECIBOS_DETALLES." + Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo + " IN (";
                        Mi_SQL = Mi_SQL + " SELECT " + Ope_Nom_Recibos_Empleados.Campo_No_Recibo + " FROM " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + " WHERE " + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + " = '" + Parametros.P_No_Nomina + "')";
                    }
                    if (!String.IsNullOrEmpty(Parametros.P_Proveedor_ID)) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE "; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + "  RECIBOS_DETALLES." + Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID + " IN (";
                        Mi_SQL = Mi_SQL + " SELECT PROVEEDORES_DETALLES." + Cat_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID + " FROM " + Cat_Nom_Proveedores_Detalles.Tabla_Cat_Nom_Proveedores_Detalles + " PROVEEDORES_DETALLES WHERE PROVEEDORES_DETALLES." + Cat_Nom_Proveedores_Detalles.Campo_Proveedor_ID + " IN (";
                        Mi_SQL = Mi_SQL + " SELECT PROVEEDORES." + Cat_Nom_Proveedores.Campo_Proveedor_ID + " FROM " + Cat_Nom_Proveedores.Tabla_Cat_Nom_Proveedores + " PROVEEDORES WHERE " + Cat_Nom_Proveedores.Campo_Proveedor_ID + " = '" + Parametros.P_Proveedor_ID + "'";
                        Mi_SQL = Mi_SQL + " )";
                        Mi_SQL = Mi_SQL + ")";
                    }
                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Datos != null) {
                        Dt_Datos = Ds_Datos.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Datos;
            }

	}

}