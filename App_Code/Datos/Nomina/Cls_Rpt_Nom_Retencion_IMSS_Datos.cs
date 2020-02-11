using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Presidencia.Rpt_Retencion_IMSS.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Text;

namespace Presidencia.Rpt_Retencion_IMSS.Datos
{
    public class Cls_Rpt_Nom_Retencion_IMSS_Datos
    {
        #region(Métodos Consulta)

        /// ********************************************************************************************************************
        /// Nombre: Consultar_Rpt_Incapacidades
        /// 
        /// Descripción: Consulta la información que sera mostrada en el reporte de incapacidades.
        /// 
        /// Parámetros: Datos.- Objeto que contiene los valores que se usaran dentro de la consulta de incapacidades.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creo: 05/Diciembre/2011 10:28 a.m.
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Rpt_Retencion_Imss(Cls_Rpt_Nom_Retencion_IMSS_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            StringBuilder Mi_SQL_Aux = new StringBuilder();
            DataTable Dt_Retencion = null;//Variable que almacenara el listado de incapacidades.

            try
            {
                Mi_SQL.Append("SELECT ");

                Mi_SQL.Append("(SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Codigo_Programatico +
                   " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                   " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID +
                   "=" + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + ") AS CODIGO_PROGRAMATICO, ");

                Mi_SQL.Append("(SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC +
                   " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                   " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID +
                   "=" + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + ") AS RFC, ");

                Mi_SQL.Append("(SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado +
                    " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                    " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID +
                    "=" + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + ") AS NO_EMPLEADO, ");

                Mi_SQL.Append("(SELECT (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") ");
                Mi_SQL.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "=");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + ") AS EMPLEADO, ");

                Mi_SQL.Append("(SELECT " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre +
                    " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                    " WHERE " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID +
                    "=" + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Dependencia_ID + ") AS DEPENDENCIA,");

                Mi_SQL.Append("('AÑO-PER   ' || (SELECT " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Anio +
                    " FROM " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas +
                    " WHERE " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID +
                    "=" + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + ") || '-' || ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Campo_No_Nomina + ") AS NOMINA, ");

                Mi_SQL.Append("(SELECT ( 'Del  ' || TO_CHAR(" + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio + ", 'DD-Mon-YYYY')");
                Mi_SQL.Append(" || '  Al  ' || ");
                Mi_SQL.Append("TO_CHAR(" + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin + ", 'DD-Mon-YYYY')");
                Mi_SQL.Append(")");
                Mi_SQL.Append(" FROM " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles);
                Mi_SQL.Append(" WHERE " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_No_Nomina);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + ") AS FECHAS, ");

                Mi_SQL.Append("(SELECT " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina);
                Mi_SQL.Append(" FROM " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas);
                Mi_SQL.Append(" WHERE " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append("=(SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append("=" + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append(")) AS TIPO_NOMINA, ");

                Mi_SQL.Append("(SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Diario +
                   " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                   " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID +
                   "=" + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + ") AS SALARIO_DIARIO, ");

                Mi_SQL.Append("(SELECT " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Monto +
                  " FROM " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det +
                  " WHERE " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID  +
                  "= (SELECT " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID +
                  " FROM " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + 
                  " WHERE " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID  + 
                  " = (SELECT " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Deduccion_IMSS +
                  " FROM " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "))" + 
                  " AND " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Recibo +
                  " = " +Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo +
                  " ) AS RETENCION, ");

                Mi_SQL.Append("'' AS SALARIO_MENSUAL, '' AS RETENCION_IMSS, ");

                Mi_SQL.Append("(SELECT " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave +
                  " || ' ' || " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Nombre +
                  " FROM " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + ", " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det +
                  " WHERE " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID +
                  " = " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID  +
                  " AND " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID +
                  " = (SELECT " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Deduccion_IMSS +
                  " FROM " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + ")" +
                  " AND " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Recibo +
                  " = " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo +
                  " ) AS CLAVE_NOMBRE_DEDUCCION ");

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados);

                if (!String.IsNullOrEmpty(Datos.P_Tipo_Nomina_ID))
                {
                    Mi_SQL_Aux.Append(" WHERE ");
                    Mi_SQL_Aux.Append(Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + " IN ");
                    Mi_SQL_Aux.Append("(SELECT " + Cat_Empleados.Campo_Empleado_ID);
                    Mi_SQL_Aux.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
                    Mi_SQL_Aux.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID + "='" + Datos.P_Tipo_Nomina_ID + "')");
                }

                if (!String.IsNullOrEmpty(Datos.P_Nombre_Empleado))
                {
                    if (Mi_SQL_Aux.ToString().Contains("WHERE"))
                    {
                        Mi_SQL_Aux.Append(" AND ");
                        Mi_SQL_Aux.Append(Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + " IN ");
                        Mi_SQL_Aux.Append("(SELECT " + Cat_Empleados.Campo_Empleado_ID);
                        Mi_SQL_Aux.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
                        Mi_SQL_Aux.Append(" WHERE ");
                        Mi_SQL_Aux.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                        Mi_SQL_Aux.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                        Mi_SQL_Aux.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") ");
                        Mi_SQL_Aux.Append(" LIKE '%" + Datos.P_Nombre_Empleado + "%'");
                        Mi_SQL_Aux.Append(" OR ");
                        Mi_SQL_Aux.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " || ' ' || ");
                        Mi_SQL_Aux.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                        Mi_SQL_Aux.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ") ");
                        Mi_SQL_Aux.Append(" LIKE '%" + Datos.P_Nombre_Empleado + "%')");
                    }
                    else
                    {
                        Mi_SQL_Aux.Append(" WHERE ");
                        Mi_SQL_Aux.Append(Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + " IN ");
                        Mi_SQL_Aux.Append("(SELECT " + Cat_Empleados.Campo_Empleado_ID);
                        Mi_SQL_Aux.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
                        Mi_SQL_Aux.Append(" WHERE ");
                        Mi_SQL_Aux.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                        Mi_SQL_Aux.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                        Mi_SQL_Aux.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") ");
                        Mi_SQL_Aux.Append(" LIKE '%" + Datos.P_Nombre_Empleado + "%'");
                        Mi_SQL_Aux.Append(" OR ");
                        Mi_SQL_Aux.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " || ' ' || ");
                        Mi_SQL_Aux.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                        Mi_SQL_Aux.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ") ");
                        Mi_SQL_Aux.Append(" LIKE '%" + Datos.P_Nombre_Empleado + "%')");
                    }

                }

                if (!String.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    if (Mi_SQL_Aux.ToString().Contains("WHERE"))
                    {
                        Mi_SQL_Aux.Append(" AND ");
                        Mi_SQL_Aux.Append(Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + " IN ");
                        Mi_SQL_Aux.Append("(SELECT " + Cat_Empleados.Campo_Empleado_ID);
                        Mi_SQL_Aux.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
                        Mi_SQL_Aux.Append(" WHERE " + Cat_Empleados.Campo_No_Empleado + " = '" + String.Format("{0:000000}", Convert.ToInt32(Datos.P_No_Empleado)) + "')");

                    }
                    else
                    {
                        Mi_SQL_Aux.Append(" WHERE ");
                        Mi_SQL_Aux.Append(Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + " IN ");
                        Mi_SQL_Aux.Append("(SELECT " + Cat_Empleados.Campo_Empleado_ID);
                        Mi_SQL_Aux.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
                        Mi_SQL_Aux.Append(" WHERE " + Cat_Empleados.Campo_No_Empleado + " = '" + String.Format("{0:000000}", Convert.ToInt32(Datos.P_No_Empleado)) + "')");
                    }

                }

                if (!String.IsNullOrEmpty(Datos.P_Nomina_ID))
                {
                    if (Mi_SQL_Aux.ToString().Contains("WHERE"))
                        Mi_SQL_Aux.Append(" AND " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'");
                    else
                        Mi_SQL_Aux.Append(" WHERE " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'");
                }

                if (Datos.P_No_Nomina > 0)
                {
                    if (Mi_SQL_Aux.ToString().Contains("WHERE"))
                        Mi_SQL_Aux.Append(" AND " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "=" + Datos.P_No_Nomina);
                    else
                        Mi_SQL_Aux.Append(" WHERE " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "=" + Datos.P_No_Nomina);
                }

                if (!String.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    if (Mi_SQL_Aux.ToString().Contains("WHERE"))
                        Mi_SQL_Aux.Append(" AND " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'");
                    else
                        Mi_SQL_Aux.Append(" WHERE " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'");
                }

                Mi_SQL_Aux.Append(" ORDER BY ");

                Mi_SQL_Aux.Append("(SELECT " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina);
                Mi_SQL_Aux.Append(" FROM " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas);
                Mi_SQL_Aux.Append(" WHERE " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);
                Mi_SQL_Aux.Append("=(SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL_Aux.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL_Aux.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                Mi_SQL_Aux.Append("=" + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID);
                Mi_SQL_Aux.Append(")), ");

                Mi_SQL_Aux.Append("(SELECT " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre +
                   " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                   " WHERE " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID +
                   "=" + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Dependencia_ID + "), ");

                Mi_SQL_Aux.Append("(SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno +
                   " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                   " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID +
                   "=" + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + ") ");

                Mi_SQL_Aux.Append(" ASC ");


                Dt_Retencion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, (Mi_SQL.ToString() + Mi_SQL_Aux.ToString())).Tables[0];

                Dt_Retencion.Columns.Add("USUARIO", typeof(String));

                if (Dt_Retencion is DataTable)
                {
                    if (Dt_Retencion.Rows.Count > 0)
                    {
                        foreach (DataRow INCAPACIDAD in Dt_Retencion.Rows)
                        {
                            if (INCAPACIDAD is DataRow)
                            {
                                INCAPACIDAD["USUARIO"] = "Elaboro: " + Cls_Sessiones.Nombre_Empleado;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar la información para la generacion del reporte de incapacidades. Error: [" + Ex.Message + "]");
            }
            return Dt_Retencion;
        }
        
        #endregion
    }
}
