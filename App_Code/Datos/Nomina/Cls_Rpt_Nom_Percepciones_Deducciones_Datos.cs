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
using Presidencia.Reportes_Nomina.Percepciones_Deducciones.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Utilidades_Nomina;

/// <summary>
/// Summary description for Cls_Rpt_Nom_Percepciones_Deducciones_Datos
/// </summary>

namespace Presidencia.Reportes_Nomina.Percepciones_Deducciones.Datos {

    public class Cls_Rpt_Nom_Percepciones_Deducciones_Datos {

        #region "Metodos"

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Percepciones_Deducciones
            ///DESCRIPCIÓN: Ejecuta la Consulta de Percepciones y Deducciones
            ///PARAMETROS:  1.- Parametros.- Filtros para la consulta.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 10/Abril/2012
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public static DataTable Consultar_Datos_Percepciones_Deducciones(Cls_Rpt_Nom_Percepciones_Deducciones_Negocio Parametros) {
                DataTable Dt_Datos_P_D = new DataTable();
                DataSet Ds_Datos_P_D = null;
                String Mi_SQL = String.Empty;
                Boolean Entro_Consulta_Where = false;
                try {
                    Mi_SQL = "SELECT RECIBOS." + Ope_Nom_Recibos_Empleados.Campo_No_Recibo + " AS NO_RECIBO";
                    Mi_SQL = Mi_SQL + ", RECIBOS." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + " AS EMPLEADO_ID";
                    Mi_SQL = Mi_SQL + ", TRIM(DEPENDENCIAS." + Cat_Dependencias.Campo_Clave + " ||' - '|| DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + ") AS NOMBRE_DEPENDENCIA ";
                    Mi_SQL = Mi_SQL + ", EMPLEADOS." + Cat_Empleados.Campo_No_Empleado + " AS NO_EMPLEADO";
                    Mi_SQL = Mi_SQL + ", TRIM (EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + " ||' '|| EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + " ||' '|| EMPLEADOS." + Cat_Empleados.Campo_Nombre + ") NOMBRE_EMPLEADO";
                    Mi_SQL = Mi_SQL + ", CALENDARIOS_NOMINAS." + Cat_Nom_Calendario_Nominas.Campo_Anio + " AS ANIO";
                    Mi_SQL = Mi_SQL + ", NOMINA_DETALLES." + Cat_Nom_Nominas_Detalles.Campo_No_Nomina + " AS PERIODO";
                    Mi_SQL = Mi_SQL + ", TRIM(' ['|| TRIM(PERCEPCIONES_DEDUCCIONES." + Cat_Nom_Percepcion_Deduccion.Campo_Clave + ") ||'] '|| PERCEPCIONES_DEDUCCIONES." + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ") AS PERCEPCION_DEDUCCION";
                    Mi_SQL = Mi_SQL + ", RECIBOS_DETALLES." + Ope_Nom_Recibos_Empleados_Det.Campo_Monto + " AS IMPORTE";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + " RECIBOS";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS ON RECIBOS." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + " = EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS ON EMPLEADOS." + Cat_Empleados.Campo_Dependencia_ID + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + " CALENDARIOS_NOMINAS ON RECIBOS." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + " = CALENDARIOS_NOMINAS." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + " NOMINA_DETALLES ON RECIBOS." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + " = NOMINA_DETALLES." + Cat_Nom_Nominas_Detalles.Campo_No_Nomina + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + " RECIBOS_DETALLES ON RECIBOS." + Ope_Nom_Recibos_Empleados.Campo_No_Recibo + " = RECIBOS_DETALLES." + Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " PERCEPCIONES_DEDUCCIONES ON RECIBOS_DETALLES." + Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID + " = PERCEPCIONES_DEDUCCIONES." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + "";
                    if (!String.IsNullOrEmpty(Parametros.P_Empleado_ID)) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE"; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND"; }
                        Mi_SQL = Mi_SQL + " RECIBOS." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + " = '" + Parametros.P_Empleado_ID + "'";
                    }
                    if (!String.IsNullOrEmpty(Parametros.P_Nomina_ID)) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE"; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND"; }
                        Mi_SQL = Mi_SQL + " RECIBOS." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + " = '" + Parametros.P_Nomina_ID + "'";
                    }
                    if (!String.IsNullOrEmpty(Parametros.P_No_Nomina)) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE"; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND"; }
                        Mi_SQL = Mi_SQL + " RECIBOS." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + " = '" + Parametros.P_No_Nomina + "'";
                    }
                    if (!String.IsNullOrEmpty(Parametros.P_Tipo_Percepcion_Deduccion)) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE"; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND"; }
                        Mi_SQL = Mi_SQL + " PERCEPCIONES_DEDUCCIONES." + Cat_Nom_Percepcion_Deduccion.Campo_Tipo + " = '" + Parametros.P_Tipo_Percepcion_Deduccion + "'";
                    }
                    if (!String.IsNullOrEmpty(Parametros.P_Percepcion_Deduccion_ID)) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE"; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND"; }
                        Mi_SQL = Mi_SQL + " PERCEPCIONES_DEDUCCIONES." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID+ " = '" + Parametros.P_Percepcion_Deduccion_ID + "'";
                    }
                    if (!String.IsNullOrEmpty(Parametros.P_Dependencia_ID)) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE"; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND"; }
                        Mi_SQL = Mi_SQL + " DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                    }
                    if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE"; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND"; }
                    Mi_SQL = Mi_SQL + " CALENDARIOS_NOMINAS." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID + " = NOMINA_DETALLES." + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID + "";
                    Mi_SQL = Mi_SQL + " ORDER BY NOMBRE_DEPENDENCIA, NOMBRE_EMPLEADO, PERCEPCION_DEDUCCION ASC";
                    Ds_Datos_P_D = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Datos_P_D != null) {
                        Dt_Datos_P_D = Ds_Datos_P_D.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Datos_P_D;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_PD_Clave
            ///DESCRIPCIÓN: Busca una Deduccion por Clave
            ///PARAMETROS:  1.- Parametros.- Filtros para la consulta.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 10/Abril/2012
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public static Cls_Rpt_Nom_Percepciones_Deducciones_Negocio Consultar_PD_Clave(Cls_Rpt_Nom_Percepciones_Deducciones_Negocio Parametros) { 
                String Mi_SQL = "SELECT * FROM " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Percepcion_Deduccion.Campo_Clave + " = '" + Parametros.P_Percepcion_Deduccion_Clave + "'";
                Cls_Rpt_Nom_Percepciones_Deducciones_Negocio Regresar = new Cls_Rpt_Nom_Percepciones_Deducciones_Negocio();
                try {
                    OracleDataReader DataReader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    while (DataReader.Read()) {
                        Regresar.P_Percepcion_Deduccion_ID = (!String.IsNullOrEmpty(DataReader[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString())) ? DataReader[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString() : String.Empty;
                        Regresar.P_Percepcion_Deduccion_Clave = (!String.IsNullOrEmpty(DataReader[Cat_Nom_Percepcion_Deduccion.Campo_Clave].ToString())) ? DataReader[Cat_Nom_Percepcion_Deduccion.Campo_Clave].ToString() : String.Empty;
                        Regresar.P_Tipo_Percepcion_Deduccion = (!String.IsNullOrEmpty(DataReader[Cat_Nom_Percepcion_Deduccion.Campo_Tipo].ToString())) ? DataReader[Cat_Nom_Percepcion_Deduccion.Campo_Tipo].ToString() : String.Empty;
                    }
                } catch (OracleException Ex) {
                    throw new Exception(Ex.Message);
                }
                return Regresar;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Empleados_Resguardos
            ///DESCRIPCIÓN          : Obtiene empleados de la Base de Datos y los regresa en un 
            ///                       DataTable de acuerdo a los filtros pasados.
            ///PARAMETROS           : Parametros.  Contiene los parametros que se van a utilizar para
            ///                                         hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 24/Octubre/2011 
            ///MODIFICO             : 
            ///FECHA_MODIFICO       : 
            ///CAUSA_MODIFICACIÓN   : 
            ///*******************************************************************************
            public static DataTable Consultar_Empleados(Cls_Rpt_Nom_Percepciones_Deducciones_Negocio Parametros) {
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

        #endregion

	}
}
