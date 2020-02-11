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
using Presidencia.Reportes_Nomina.Reporteador_Empleados.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Utilidades_Nomina;
/// <summary>
/// Summary description for Cls_Rpt_Nom_Reporteador_Empleados_Datos
/// </summary>
namespace Presidencia.Reportes_Nomina.Reporteador_Empleados.Datos{

    public class Cls_Rpt_Nom_Reporteador_Empleados_Datos{

        #region "Metodos"

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Empleado
            ///DESCRIPCIÓN: Ejecuta la Consulta de Empleados
            ///PARAMETROS:  1.- Parametros.- Filtros para la consulta.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 04/Abril/2012
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public static DataTable Consultar_Datos_Empleado(Cls_Rpt_Nom_Reporteador_Empleados_Negocio Parametros) {
                DataTable Dt_Datos_Empleado = new DataTable();
                DataSet Ds_Datos_Empleados = null;
                String Mi_SQL = String.Empty;
                Boolean Entro_Consulta_Where = false;
                try {
                    Mi_SQL = "SELECT EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID + " AS EMPLEADO_ID";
                    Mi_SQL = Mi_SQL + ", (DEPENDENCIAS." + Cat_Dependencias.Campo_Clave + " ||' - '|| DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + ") AS NOMBRE_DEPENDENCIA";
                    Mi_SQL = Mi_SQL + ", EMPLEADOS." + Cat_Empleados.Campo_No_Empleado + " AS NO_EMPLEADO";
                    Mi_SQL = Mi_SQL + ", EMPLEADOS." + Cat_Empleados.Campo_RFC + " AS RFC";
                    Mi_SQL = Mi_SQL + ", TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + " ||' '|| EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + " ||' '|| EMPLEADOS." + Cat_Empleados.Campo_Nombre + ") AS NOMBRE_EMPLEADO";
                    Mi_SQL = Mi_SQL + ", EMPLEADOS." + Cat_Empleados.Campo_SAP_Codigo_Programatico + " AS CODIGO_PROGRAMATICO";
                    Mi_SQL = Mi_SQL + ", (' ['|| NVL(TO_CHAR(TO_NUMBER(SINDICATOS." + Cat_Nom_Sindicatos.Campo_Sindicato_ID + ")),'-') ||'] '|| SINDICATOS." + Cat_Nom_Sindicatos.Campo_Nombre + ") AS CLAVE_SINDICATO";
                    Mi_SQL = Mi_SQL + ", (EMPLEADOS." + Cat_Empleados.Campo_Salario_Diario + " * " + Cls_Utlidades_Nomina.Dias_Mes_Fijo + ") AS SUELDO_MENSUAL";
                    Mi_SQL = Mi_SQL + ", EMPLEADOS." + Cat_Empleados.Campo_SAP_Codigo_Programatico + " AS CODIGO_PROGRAMATICO";
                    Mi_SQL = Mi_SQL + ", EMPLEADOS." + Cat_Empleados.Campo_Fecha_Inicio + " AS FECHA_INGRESO";
                    Mi_SQL = Mi_SQL + ", PUESTOS." + Cat_Puestos.Campo_Nombre + " AS NIVEL_CARGO";
                    Mi_SQL = Mi_SQL + ", (EMPLEADOS." + Cat_Empleados.Campo_Salario_Diario + " * " + Cls_Utlidades_Nomina.Dias_Mes_Fijo + ") AS SUELDO_ACTUAL";
                    Mi_SQL = Mi_SQL + ", PUESTOS." + Cat_Puestos.Campo_Nombre + " AS NIVEL_ACTUAL";
                    Mi_SQL = Mi_SQL + ", EMPLEADOS." + Cat_Empleados.Campo_Fecha_Nacimiento + " AS FECHA_NACIMIENTO";
                    Mi_SQL = Mi_SQL + ", EMPLEADOS." + Cat_Empleados.Campo_Estado + " AS ESTADO_CIVIL";
                    Mi_SQL = Mi_SQL + ", EMPLEADOS." + Cat_Empleados.Campo_Sexo + " AS SEXO";
                    Mi_SQL = Mi_SQL + ", EMPLEADOS." + Cat_Empleados.Campo_Salario_Diario + " AS SALARIO_DIARIO";
                    Mi_SQL = Mi_SQL + ", TO_NUMBER(TIPOS_NOMINA." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + ") AS TIPO_NOMINA";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Puestos.Tabla_Cat_Puestos + " PUESTOS";
                    Mi_SQL = Mi_SQL + " ON EMPLEADOS." + Cat_Empleados.Campo_Puesto_ID + " = PUESTOS." + Cat_Puestos.Campo_Puesto_ID; 
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + " SINDICATOS";
                    Mi_SQL = Mi_SQL + " ON EMPLEADOS." + Cat_Empleados.Campo_Sindicato_ID + " = SINDICATOS." + Cat_Nom_Sindicatos.Campo_Sindicato_ID;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " TIPOS_NOMINA";
                    Mi_SQL = Mi_SQL + " ON EMPLEADOS." + Cat_Empleados.Campo_Tipo_Nomina_ID + " = TIPOS_NOMINA." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
                    Mi_SQL = Mi_SQL + " ON EMPLEADOS." + Cat_Empleados.Campo_Dependencia_ID + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID;
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
                    if (!String.IsNullOrEmpty(Parametros.P_Nombre_Empleado)) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE ("; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND ("; }
                        Mi_SQL = Mi_SQL + " UPPER(TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + " ||' '|| EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + " ||' '|| EMPLEADOS." + Cat_Empleados.Campo_Nombre + ")) LIKE '%" + Parametros.P_Nombre_Empleado.ToUpper() + "%'";
                        Mi_SQL = Mi_SQL + " OR UPPER(TRIM(EMPLEADOS." + Cat_Empleados.Campo_Nombre + " ||' '|| EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + " ||' '|| EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + ")) LIKE '%" + Parametros.P_Nombre_Empleado.ToUpper() + "%'";
                        Mi_SQL = Mi_SQL + ")";
                    }
                    if (!String.IsNullOrEmpty(Parametros.P_Tipo_Reporte)) {
                        if (Parametros.P_Tipo_Reporte.Equals("SINDICATO")) {
                            if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE"; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND"; }
                            Mi_SQL = Mi_SQL + " EMPLEADOS." + Cat_Empleados.Campo_Sindicato_ID + " IS NOT NULL";
                        } else if (Parametros.P_Tipo_Reporte.Equals("BAJAS")) {
                            if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE"; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND"; }
                            Mi_SQL = Mi_SQL + " EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID + " IN (";
                            Mi_SQL = Mi_SQL + "SELECT " + Cat_Emp_Movimientos_Det.Campo_Empleado_ID + " FROM " + Cat_Emp_Movimientos_Det.Tabla_Cat_Emp_Movimientos_Det;
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Emp_Movimientos_Det.Campo_Tipo_Movimiento + "= 'BAJA' ";
                            if (String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Inicial).Equals(String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Inicial))) {
                                Mi_SQL = Mi_SQL + " AND " + Cat_Emp_Movimientos_Det.Campo_Fecha_Creo + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Inicial) + "'";    
                            }
                            if (String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Final).Equals(String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Inicial))) {
                                Mi_SQL = Mi_SQL + " AND " + Cat_Emp_Movimientos_Det.Campo_Fecha_Creo + " < '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Final.AddDays(1)) + "'";    
                            }
                            Mi_SQL = Mi_SQL + ")";
                        } else if (Parametros.P_Tipo_Reporte.Equals("PROMOCIONES")) {
                            if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE"; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND"; }
                            Mi_SQL = Mi_SQL + " EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID + " IN (";
                            Mi_SQL = Mi_SQL + "SELECT " + Cat_Emp_Movimientos_Det.Campo_Empleado_ID + " FROM " + Cat_Emp_Movimientos_Det.Tabla_Cat_Emp_Movimientos_Det;
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Emp_Movimientos_Det.Campo_Tipo_Movimiento + "= 'PROMOCION' ";
                            if (String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Inicial).Equals(String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Inicial))) {
                                Mi_SQL = Mi_SQL + " AND " + Cat_Emp_Movimientos_Det.Campo_Fecha_Creo + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Inicial) + "'";    
                            }
                            if (String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Final).Equals(String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Inicial))) {
                                Mi_SQL = Mi_SQL + " AND " + Cat_Emp_Movimientos_Det.Campo_Fecha_Creo + " < '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Final.AddDays(1)) + "'";    
                            }
                            Mi_SQL = Mi_SQL + ")";
                        } else if (Parametros.P_Tipo_Reporte.Equals("REINGRESOS")) {
                            if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE"; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND"; }
                            Mi_SQL = Mi_SQL + " EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID + " IN (";
                            Mi_SQL = Mi_SQL + "SELECT " + Cat_Emp_Movimientos_Det.Campo_Empleado_ID + " FROM " + Cat_Emp_Movimientos_Det.Tabla_Cat_Emp_Movimientos_Det;
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Emp_Movimientos_Det.Campo_Tipo_Movimiento + "= 'REINGRESO' ";
                            if (String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Inicial).Equals(String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Inicial))) {
                                Mi_SQL = Mi_SQL + " AND " + Cat_Emp_Movimientos_Det.Campo_Fecha_Creo + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Inicial) + "'";    
                            }
                            if (String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Final).Equals(String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Inicial))) {
                                Mi_SQL = Mi_SQL + " AND " + Cat_Emp_Movimientos_Det.Campo_Fecha_Creo + " < '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Final.AddDays(1)) + "'";    
                            }
                            Mi_SQL = Mi_SQL + ")";
                        } else if (Parametros.P_Tipo_Reporte.Equals("ALTAS")) {
                            if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE"; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND"; }
                            if (String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Inicial).Equals(String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Inicial))) {
                                Mi_SQL = Mi_SQL + " EMPLEADOS." + Cat_Empleados.Campo_Fecha_Inicio + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Inicial) + "'";    
                            }
                            if (String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Final).Equals(String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Inicial))) {
                                Mi_SQL = Mi_SQL + " EMPLEADOS." + Cat_Empleados.Campo_Fecha_Inicio + " < '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Final.AddDays(1)) + "'";    
                            }
                        }
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY NOMBRE_DEPENDENCIA, NOMBRE_EMPLEADO";
                    Ds_Datos_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Datos_Empleados != null) {
                        Dt_Datos_Empleado = Ds_Datos_Empleados.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Datos_Empleado;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Ultimo_Movimiento_Empleado
            ///DESCRIPCIÓN: Consulta el Ultimo movimiento de un tipo
            ///PARAMETROS:  1.- Parametros.- Filtros para la consulta.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 04/Abril/2012
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public static DataTable Consultar_Ultimo_Movimiento_Empleado(Cls_Rpt_Nom_Reporteador_Empleados_Negocio Parametros) { 
                DataTable Dt_Datos_Empleado = new DataTable();
                DataSet Ds_Datos_Empleados = null;
                String Mi_SQL = String.Empty;
                Boolean Entro_Consulta_Where = false;
                try {
                    Mi_SQL = "SELECT MOVIMIENTOS." + Cat_Emp_Movimientos_Det.Campo_No_Movimiento+ " AS NO_MOVIMIENTO";
                    Mi_SQL = Mi_SQL + ", MOVIMIENTOS." + Cat_Emp_Movimientos_Det.Campo_Tipo_Movimiento + " AS TIPO_MOVIMIENTO";
                    Mi_SQL = Mi_SQL + ", MOVIMIENTOS." + Cat_Emp_Movimientos_Det.Campo_Motivo_Movimiento + " AS MOTIVO_MOVIMIENTO";
                    Mi_SQL = Mi_SQL + ", MOVIMIENTOS." + Cat_Emp_Movimientos_Det.Campo_Sueldo_Actual + " AS SUELDO_ACTUAL";
                    Mi_SQL = Mi_SQL + ", MOVIMIENTOS." + Cat_Emp_Movimientos_Det.Campo_Fecha_Creo + " AS FECHA_MOVIMIENTO";
                    Mi_SQL = Mi_SQL + ", PUESTOS." + Cat_Puestos.Campo_Nombre + " AS PUESTO";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Emp_Movimientos_Det.Tabla_Cat_Emp_Movimientos_Det + " MOVIMIENTOS";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Puestos.Tabla_Cat_Puestos + " PUESTOS";
                    Mi_SQL = Mi_SQL + " ON MOVIMIENTOS." + Cat_Emp_Movimientos_Det.Campo_Puesto_ID + " = PUESTOS." + Cat_Puestos.Campo_Puesto_ID; 
                    if (!String.IsNullOrEmpty(Parametros.P_Tipo_Movimiento)) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE"; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND"; }
                        Mi_SQL = Mi_SQL + " MOVIMIENTOS." + Cat_Emp_Movimientos_Det.Campo_Tipo_Movimiento + " = '" + Parametros.P_Tipo_Movimiento + "'";
                    }
                    if (!String.IsNullOrEmpty(Parametros.P_Empleado_ID)) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE"; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND"; }
                        Mi_SQL = Mi_SQL + " MOVIMIENTOS." + Cat_Emp_Movimientos_Det.Campo_Empleado_ID + " = '" + Parametros.P_Empleado_ID + "'";
                    }
                    if (!String.IsNullOrEmpty(Parametros.P_Motivo_Movimiento)) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE"; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND"; }
                        Mi_SQL = Mi_SQL + " MOVIMIENTOS." + Cat_Emp_Movimientos_Det.Campo_Motivo_Movimiento + " = '" + Parametros.P_Motivo_Movimiento + "'";
                    }
                    if (String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Inicial).Equals(String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Inicial))) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE"; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND"; }
                        Mi_SQL = Mi_SQL + " MOVIMIENTOS." + Cat_Emp_Movimientos_Det.Campo_Fecha_Creo + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Inicial) + "'";    
                    }
                    if (String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Final).Equals(String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Inicial))) {
                        if (!Entro_Consulta_Where) { Mi_SQL = Mi_SQL + " WHERE"; Entro_Consulta_Where = true; } else { Mi_SQL = Mi_SQL + " AND"; }
                        Mi_SQL = Mi_SQL + " MOVIMIENTOS." + Cat_Emp_Movimientos_Det.Campo_Fecha_Creo + " < '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Final.AddDays(1)) + "'";    
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY NO_MOVIMIENTO DESC";
                    Ds_Datos_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Datos_Empleados != null) {
                        Dt_Datos_Empleado = Ds_Datos_Empleados.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Datos_Empleado;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Dependencias_Activas
            ///DESCRIPCIÓN: Consulta las Dependendicas Activas
            ///PARAMETROS: 
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 05/Abril/2012
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
            ///FECHA_CREO: 07/Abril/2012
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