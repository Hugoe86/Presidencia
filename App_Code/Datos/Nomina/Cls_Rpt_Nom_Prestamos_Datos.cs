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

/// <summary>
/// Summary description for Cls_Rpt_Nom_Prestamos_Datos
/// </summary>

namespace Presidencia.Reportes_Nomina.Prestamos.Datos {

    public class Cls_Rpt_Nom_Prestamos_Datos {

        #region "Metodos"

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Prestamos
            ///DESCRIPCIÓN: Ejecuta la Consulta de Prestamos
            ///PARAMETROS:  1.- Parametros.- Filtros para la consulta.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 09/Abril/2012
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public static DataTable Consultar_Datos_Prestamos(Cls_Rpt_Nom_Prestamos_Negocio Parametros) {
                DataTable Dt_Datos_Prestamos = new DataTable();
                DataSet Ds_Datos_Prestamos = null;
                String Mi_SQL = String.Empty;
                Boolean Entro_Consulta_Where = false;
                try {
                    Mi_SQL = "SELECT PRESTAMOS." + Ope_Nom_Solicitud_Prestamo.Campo_Solicita_Empleado_ID + " AS EMPLEADO_ID";
                    Mi_SQL = Mi_SQL + ", EMPLEADOS." + Cat_Empleados.Campo_No_Empleado + " AS NO_EMPLEADO";
                    Mi_SQL = Mi_SQL + ", TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + " ||' '|| EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + " ||' '|| EMPLEADOS." + Cat_Empleados.Campo_Nombre + ") AS NOMBRE_EMPLEADO";
                    Mi_SQL = Mi_SQL + ", (DEPENDENCIAS." + Cat_Dependencias.Campo_Clave + " ||' - '|| DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + ")  AS NOMBRE_DEPENDENCIA";
                    Mi_SQL = Mi_SQL + ", PRESTAMOS." + Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud + " AS NO_SOLICITUD";
                    Mi_SQL = Mi_SQL + ", PRESTAMOS." + Ope_Nom_Solicitud_Prestamo.Campo_Importe_Prestamo + " AS IMPORTE_PRESTAMO";
                    Mi_SQL = Mi_SQL + ", PRESTAMOS." + Ope_Nom_Solicitud_Prestamo.Campo_No_Pagos + " AS CATORCENAS_DESCONTAR";
                    Mi_SQL = Mi_SQL + ", PRESTAMOS." + Ope_Nom_Solicitud_Prestamo.Campo_Abono + " AS CANTIDAD_DESCONTAR";
                    Mi_SQL = Mi_SQL + ", PRESTAMOS." + Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Solicitud + " AS ESTATUS_SOLICITUD";
                    Mi_SQL = Mi_SQL + ", NVL(PRESTAMOS." + Ope_Nom_Solicitud_Prestamo.Campo_Monto_Abonado + ", 0) AS CANTIDAD_PAGADA";
                    Mi_SQL = Mi_SQL + ", PRESTAMOS." + Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual + " AS SALDO_PRESTAMO";
                    Mi_SQL = Mi_SQL + ", EMPLEADOS." + Cat_Empleados.Campo_No_Cuenta_Bancaria + "  AS NO_CUENTA";
                    Mi_SQL = Mi_SQL + ", BANCOS." + Cat_Nom_Bancos.Campo_Nombre + " AS BANCO";
                    Mi_SQL = Mi_SQL + ", PRESTAMOS." + Ope_Nom_Solicitud_Prestamo.Campo_No_Pagos + " AS CATORCENAS_PAGAR";
                    Mi_SQL = Mi_SQL + ", (TRIM(DEDUCCIONES." + Cat_Nom_Percepcion_Deduccion.Campo_Clave + ") ||' - '|| DEDUCCIONES." + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ")  AS CLAVE_DEDUCCION";
                    Mi_SQL = Mi_SQL + ", PRESTAMOS." + Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Inicio_Pago + " AS INICIO_PAGO";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo + " PRESTAMOS";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS  ON PRESTAMOS." + Ope_Nom_Solicitud_Prestamo.Campo_Solicita_Empleado_ID + " = EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS  ON EMPLEADOS." + Cat_Empleados.Campo_Dependencia_ID + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + " BANCOS  ON EMPLEADOS." + Cat_Empleados.Campo_Banco_ID + " = BANCOS." + Cat_Nom_Bancos.Campo_Banco_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " DEDUCCIONES  ON PRESTAMOS." + Ope_Nom_Solicitud_Prestamo.Campo_Percepcion_Deduccion_ID + " = DEDUCCIONES." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + "";
                    if (!String.IsNullOrEmpty(Parametros.P_Tipo_Nomina_ID)) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE"; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND"; }
                        Mi_SQL = Mi_SQL + " EMPLEADOS." + Cat_Empleados.Campo_Tipo_Nomina_ID + " = '" + Parametros.P_Tipo_Nomina_ID + "'";
                    }
                    if (!String.IsNullOrEmpty(Parametros.P_Dependencia_ID)) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE"; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND"; }
                        Mi_SQL = Mi_SQL + " EMPLEADOS." + Cat_Empleados.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                    }
                    if (!String.IsNullOrEmpty(Parametros.P_No_Empleado)) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE"; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND"; }
                        Mi_SQL = Mi_SQL + " EMPLEADOS." + Cat_Empleados.Campo_No_Empleado + " = '" + Parametros.P_No_Empleado + "'";
                    }
                    if (!String.IsNullOrEmpty(Parametros.P_RFC_Empleado)) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE"; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND"; }
                        Mi_SQL = Mi_SQL + " EMPLEADOS." + Cat_Empleados.Campo_RFC + " LIKE '%" + Parametros.P_RFC_Empleado + "%'";
                    }
                    if (!String.IsNullOrEmpty(Parametros.P_Nomina_ID)) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE"; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND"; }
                        Mi_SQL = Mi_SQL + " PRESTAMOS." + Ope_Nom_Solicitud_Prestamo.Campo_Nomina_ID + " = '" + Parametros.P_Nomina_ID + "'";
                    }
                    if (!String.IsNullOrEmpty(Parametros.P_No_Nomina)) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE"; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND"; }
                        Mi_SQL = Mi_SQL + " PRESTAMOS." + Ope_Nom_Solicitud_Prestamo.Campo_No_Nomina + " = '" + Parametros.P_No_Nomina + "'";
                    }
                    if (!String.IsNullOrEmpty(Parametros.P_Nombre_Empleado)) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE ("; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND ("; }
                        Mi_SQL = Mi_SQL + " UPPER(TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + " ||' '|| EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + " ||' '|| EMPLEADOS." + Cat_Empleados.Campo_Nombre + ")) LIKE '%" + Parametros.P_Nombre_Empleado.ToUpper() + "%'";
                        Mi_SQL = Mi_SQL + " OR UPPER(TRIM(EMPLEADOS." + Cat_Empleados.Campo_Nombre + " ||' '|| EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + " ||' '|| EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + ")) LIKE '%" + Parametros.P_Nombre_Empleado.ToUpper() + "%'";
                        Mi_SQL = Mi_SQL + ")";
                    }
                    if (!String.IsNullOrEmpty(Parametros.P_Tipo_Reporte) && !Parametros.P_Tipo_Reporte.Trim().Equals("PRESTAMOS_CAPTURADOS")) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE"; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND"; }
                        Mi_SQL = Mi_SQL + " PRESTAMOS." + Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Solicitud + " = 'Autorizado'";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY NOMBRE_DEPENDENCIA, NOMBRE_EMPLEADO ASC";
                    Ds_Datos_Prestamos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Datos_Prestamos != null) {
                        Dt_Datos_Prestamos = Ds_Datos_Prestamos.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Datos_Prestamos;
            }

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
            public static DataTable Consultar_Dependencias_Activas() { 
                DataTable Dt_Datos = new DataTable();
                DataSet Ds_Datos = null;
                String Mi_SQL = String.Empty;
                try {
                    Mi_SQL = "SELECT DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID + " AS DEPENDENCIA_ID";
                    Mi_SQL = Mi_SQL + ", (DEPENDENCIAS." + Cat_Dependencias.Campo_Clave + " ||' - '|| DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + ") AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
                    Mi_SQL = Mi_SQL + " WHERE DEPENDENCIAS." + Cat_Dependencias.Campo_Estatus + " = 'ACTIVO'";
                    Mi_SQL = Mi_SQL + " ORDER BY NOMBRE ASC";
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
            ///NOMBRE DE LA FUNCIÓN: Consultar_Tipos_Nomina_Activos
            ///DESCRIPCIÓN: Consulta los Tipos de Nomina Activas
            ///PARAMETROS: 
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 09/Abril/2012
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public static DataTable Consultar_Tipos_Nomina() { 
                DataTable Dt_Datos = new DataTable();
                DataSet Ds_Datos = null;
                String Mi_SQL = String.Empty;
                try {
                    Mi_SQL = "SELECT TIPOS_NOMINA." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + " AS TIPO_NOMINA_ID";
                    Mi_SQL = Mi_SQL + ", (TO_CHAR(TO_NUMBER(TIPOS_NOMINA." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + ")) ||' - '|| TIPOS_NOMINA." + Cat_Nom_Tipos_Nominas.Campo_Nomina + ") AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " TIPOS_NOMINA";
                    Mi_SQL = Mi_SQL + " ORDER BY NOMBRE ASC";
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