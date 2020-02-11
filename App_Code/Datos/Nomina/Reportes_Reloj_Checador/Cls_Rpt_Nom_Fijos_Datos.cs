using System;
using System.IO;
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
using System.Text;
using System.Collections.Generic;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Reportes_nomina_Fijos.Negocio;

namespace Presidencia.Reportes_nomina_Fijos.Datos
{
    public class Cls_Rpt_Nom_Fijos_Datos
    {
        #region Consulta
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Deducciones_Fijas
        /// DESCRIPCION : Consulta las deducciones fijas que existan en la base de datos
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        :Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 26/Marzo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Deducciones_Fijas(Cls_Rpt_Nom_Fijos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta para del Empleado
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append("( Trim(" + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave +
                        ") || '  -  ' || " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Nombre +
                        ") as Concepto, ");
                
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") as Nombre_Empleado,");

                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Codigo_Programatico + ", ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " as Clave_Dependencia, ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " as Nombre_Dependencia, ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad + ", ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe + ", ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo + ", ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida + ", ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina + ", ");
                Mi_SQL.Append(" (cast(" + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID +
                        " as decimal(10,5) )) as Tipo_Nomina_ID, ");

                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio + ", ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin + ", ");
                Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Anio + ", ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_No_Nomina + " ");

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det);

                Mi_SQL.Append(" left outer join " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " on ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID);
                Mi_SQL.Append("=" + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID);

                Mi_SQL.Append(" left outer join " + Cat_Empleados.Tabla_Cat_Empleados + " on ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID);
                Mi_SQL.Append("=" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);

                Mi_SQL.Append(" left outer join " + Cat_Dependencias.Tabla_Cat_Dependencias + " on ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID);
                Mi_SQL.Append("=" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);

                Mi_SQL.Append(" left outer join " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " on ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append("=" + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);

                Mi_SQL.Append(" left outer join " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + " on ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Nomina_ID);
                Mi_SQL.Append("=" + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID);

                Mi_SQL.Append(" left outer join " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + " on ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Nomina_ID);
                Mi_SQL.Append("=" + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_No_Nomina);
                Mi_SQL.Append("=" + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_No_Nomina);

                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." +
                        Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion + "='FIJA' ");

                Mi_SQL.Append(" AND " + Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." +
                        Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Nomina_ID + "='" + Datos.P_Nomina_id + "' ");

                Mi_SQL.Append(" AND " + Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." +
                                      Cat_Nom_Emp_Perc_Dedu_Deta.Campo_No_Nomina + " = " + Datos.P_No_Nomina + " ");

                Mi_SQL.Append(" AND " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." +
                    Cat_Nom_Percepcion_Deduccion.Campo_Tipo + "='" + Datos.P_Tipo + "' ");

                Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." +
                    Cat_Empleados.Campo_Estatus + "='ACTIVO' ");

                //  filtro por clave de deduccion
                if (!String.IsNullOrEmpty(Datos.P_Clave_Deduccion))
                {
                    Mi_SQL.Append(" AND " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." +
                            Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + " in " + "(select " + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID +
                            " From " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " where " +
                            Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + "='" + Datos.P_Clave_Deduccion + "') ");
                }
                //  filtro por dependencia
                if (!String.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    Mi_SQL.Append(" AND " + Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." +
                            Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + " in ( SELECT " + Cat_Empleados.Campo_Empleado_ID +
                            " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_Dependencia_ID +
                            "='" + Datos.P_Dependencia_ID + "') ");
                }

                //  filtro por numero de empleado
                if (!String.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    Mi_SQL.Append(" AND " + Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." +
                            Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + " in ( SELECT " + Cat_Empleados.Campo_Empleado_ID +
                            " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_No_Empleado +
                            "='" + Datos.P_No_Empleado + "') ");
                }
                //  filtrar por tipo de nomina
                if (!String.IsNullOrEmpty(Datos.P_Tipo_Nomina_ID))
                {
                    Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." +
                            Cat_Empleados.Campo_Tipo_Nomina_ID + " in ( SELECT " + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID +
                            " FROM " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " WHERE " + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID +
                            "='" + Datos.P_Tipo_Nomina_ID + "') ");
                }

                // ordenar las columnas
                Mi_SQL.Append(" order by " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." +
                        Cat_Nom_Tipos_Nominas.Campo_Nomina + ", Nombre_Dependencia, Concepto, Nombre_Empleado");

                Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                return Dt_Consulta;
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Deducciones_Variables
        /// DESCRIPCION : Consulta las deducciones variables que existan en la base de datos
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        :Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 26/Marzo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Deducciones_Variables(Cls_Rpt_Nom_Fijos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta para del Empleado
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append("( Trim(" + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave +
                        ") || '  -  ' || " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Nombre +
                        ") as Concepto, ");

                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") as Nombre_Empleado,");

                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Codigo_Programatico + ", ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " as Clave_Dependencia, ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " as Nombre_Dependencia, ");
                Mi_SQL.Append(Ope_Nom_Deduc_Var_Emp_Det.Tabla_Ope_Nom_Deduc_Var_Emp_Det + "." + Ope_Nom_Deduc_Var_Emp_Det.Campo_Cantidad + ", ");

                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina + ", ");
                Mi_SQL.Append(" (cast(" + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID +
                        " as decimal(10,5) )) as Tipo_Nomina_ID, ");

                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio + ", ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin + ", ");
                Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Anio + ", ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_No_Nomina + " ");

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Nom_Deduc_Var_Emp_Det.Tabla_Ope_Nom_Deduc_Var_Emp_Det);
             
                Mi_SQL.Append(" left outer join " + Ope_Nom_Deducciones_Var.Tabla_Ope_Nom_Deducciones_Var + " on ");
                Mi_SQL.Append(Ope_Nom_Deduc_Var_Emp_Det.Tabla_Ope_Nom_Deduc_Var_Emp_Det + "." + Ope_Nom_Deduc_Var_Emp_Det.Campo_No_Deduccion);
                Mi_SQL.Append("=" + Ope_Nom_Deducciones_Var.Tabla_Ope_Nom_Deducciones_Var + "." + Ope_Nom_Deducciones_Var.Campo_No_Deduccion);

                Mi_SQL.Append(" left outer join " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " on ");
                Mi_SQL.Append(Ope_Nom_Deducciones_Var.Tabla_Ope_Nom_Deducciones_Var + "." + Ope_Nom_Deducciones_Var.Campo_Percepcion_Deduccion_ID);
                Mi_SQL.Append("=" + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID);

                Mi_SQL.Append(" left outer join " + Cat_Empleados.Tabla_Cat_Empleados + " on ");
                Mi_SQL.Append(Ope_Nom_Deduc_Var_Emp_Det.Tabla_Ope_Nom_Deduc_Var_Emp_Det + "." + Ope_Nom_Deduc_Var_Emp_Det.Campo_Empleado_ID);
                Mi_SQL.Append("=" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);

                Mi_SQL.Append(" left outer join " + Cat_Dependencias.Tabla_Cat_Dependencias + " on ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID);
                Mi_SQL.Append("=" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);

                Mi_SQL.Append(" left outer join " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " on ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append("=" + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);

                Mi_SQL.Append(" left outer join " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + " on ");
                Mi_SQL.Append(Ope_Nom_Deducciones_Var.Tabla_Ope_Nom_Deducciones_Var + "." + Ope_Nom_Deducciones_Var.Campo_Nomina_ID);
                Mi_SQL.Append("=" + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID);

                Mi_SQL.Append(" left outer join " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + " on ");
                Mi_SQL.Append(Ope_Nom_Deducciones_Var.Tabla_Ope_Nom_Deducciones_Var + "." + Ope_Nom_Deducciones_Var.Campo_Nomina_ID);
                Mi_SQL.Append("=" + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Deducciones_Var.Tabla_Ope_Nom_Deducciones_Var + "." + Ope_Nom_Deducciones_Var.Campo_No_Nomina);
                Mi_SQL.Append("=" + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_No_Nomina);

                Mi_SQL.Append(" WHERE ");

                Mi_SQL.Append(Ope_Nom_Deducciones_Var.Tabla_Ope_Nom_Deducciones_Var + "." +
                        Ope_Nom_Deducciones_Var.Campo_Nomina_ID + "='" + Datos.P_Nomina_id + "' ");

                Mi_SQL.Append(" AND " + Ope_Nom_Deducciones_Var.Tabla_Ope_Nom_Deducciones_Var+ "." +
                        Ope_Nom_Deducciones_Var.Campo_No_Nomina + "=" + Datos.P_No_Nomina + " ");

                Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." +
                        Cat_Empleados.Campo_Estatus + "='ACTIVO' ");

                //  filtro por clave de deduccion
                if (!String.IsNullOrEmpty(Datos.P_Clave_Deduccion))
                {
                    Mi_SQL.Append(" AND " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." +
                            Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + " in " + "(select " + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID +
                            " From " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " where " +
                            Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + "='" + Datos.P_Clave_Deduccion + "') ");
                }
                //  filtro por dependencia
                if (!String.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    Mi_SQL.Append(" AND " + Ope_Nom_Deduc_Var_Emp_Det.Tabla_Ope_Nom_Deduc_Var_Emp_Det + "." +
                            Ope_Nom_Deduc_Var_Emp_Det.Campo_Empleado_ID + " in ( SELECT " + Cat_Empleados.Campo_Empleado_ID +
                            " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_Dependencia_ID +
                            "='" + Datos.P_Dependencia_ID + "') ");
                }

                //  filtro por numero de empleado
                if (!String.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    Mi_SQL.Append(" AND " + Ope_Nom_Deduc_Var_Emp_Det.Tabla_Ope_Nom_Deduc_Var_Emp_Det + "." +
                            Ope_Nom_Deduc_Var_Emp_Det.Campo_Empleado_ID + " in ( SELECT " + Cat_Empleados.Campo_Empleado_ID +
                            " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_No_Empleado +
                            "='" + Datos.P_No_Empleado + "') ");
                }
                //  filtrar por tipo de nomina
                if (!String.IsNullOrEmpty(Datos.P_Tipo_Nomina_ID))
                {
                    Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." +
                            Cat_Empleados.Campo_Tipo_Nomina_ID + " in ( SELECT " + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID +
                            " FROM " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " WHERE " + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID +
                            "='" + Datos.P_Tipo_Nomina_ID + "') ");
                }

                // ordenar las columnas
                Mi_SQL.Append(" order by " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." +
                        Cat_Nom_Tipos_Nominas.Campo_Nomina + ", Nombre_Dependencia, Concepto, Nombre_Empleado");

                Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                return Dt_Consulta;
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Percepciones
        /// DESCRIPCION : Consulta las percepciones que existan en la base de datos
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        :Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 27/Marzo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Percepciones(Cls_Rpt_Nom_Fijos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta para del Empleado
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append("( Trim(" + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave +
                        ") || '  -  ' || " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Nombre +
                        ") as Concepto, ");

                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") as Nombre_Empleado,");

                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Codigo_Programatico + ", ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " as Clave_Dependencia, ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " as Nombre_Dependencia, ");
                Mi_SQL.Append(Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + "." + Ope_Nom_Perc_Var_Emp_Det.Campo_Cantidad + ", ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina + ", ");
                
                Mi_SQL.Append(" (cast(" + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID +
                        " as decimal(10,5) )) as Tipo_Nomina_ID, ");

                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio + ", ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin + ", ");
                Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Anio + ", ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_No_Nomina + " ");


                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det);

                Mi_SQL.Append(" left outer join " + Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + " on ");
                Mi_SQL.Append(Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + "." + Ope_Nom_Perc_Var_Emp_Det.Campo_No_Percepcion);
                Mi_SQL.Append("=" + Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + "." + Ope_Nom_Percepciones_Var.Campo_No_Percepcion);

                Mi_SQL.Append(" left outer join " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " on ");
                Mi_SQL.Append(Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + "." + Ope_Nom_Percepciones_Var.Campo_Percepcion_Deduccion_ID);
                Mi_SQL.Append("=" + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID);

                Mi_SQL.Append(" left outer join " + Cat_Empleados.Tabla_Cat_Empleados + " on ");
                Mi_SQL.Append(Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + "." + Ope_Nom_Perc_Var_Emp_Det.Campo_Empleado_ID);
                Mi_SQL.Append("=" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);

                

                Mi_SQL.Append(" left outer join " + Cat_Dependencias.Tabla_Cat_Dependencias + " on ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID);
                Mi_SQL.Append("=" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);

                Mi_SQL.Append(" left outer join " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " on ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append("=" + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);

                Mi_SQL.Append(" left outer join " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + " on ");
                Mi_SQL.Append(Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + "." + Ope_Nom_Percepciones_Var.Campo_Nomina_ID);
                Mi_SQL.Append("=" + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID);

                Mi_SQL.Append(" left outer join " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + " on ");
                Mi_SQL.Append(Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + "." + Ope_Nom_Percepciones_Var.Campo_Nomina_ID);
                Mi_SQL.Append("=" + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + "." + Ope_Nom_Percepciones_Var.Campo_No_Nomina);
                Mi_SQL.Append("=" + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_No_Nomina);

                Mi_SQL.Append(" WHERE ");

                Mi_SQL.Append(Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + "." +
                        Ope_Nom_Percepciones_Var.Campo_Nomina_ID + "='" + Datos.P_Nomina_id + "' ");

                Mi_SQL.Append(" AND " + Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + "." +
                        Ope_Nom_Percepciones_Var.Campo_No_Nomina + "=" + Datos.P_No_Nomina + " ");

                Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." +
                        Cat_Empleados.Campo_Estatus + "='ACTIVO' ");

                Mi_SQL.Append(" AND " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." +
                        Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + " in (select " +
                        Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + " From " +
                        Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " where " +
                        Cat_Nom_Percepcion_Deduccion.Campo_Tipo + "='" + Datos.P_Tipo + "' and " +
                        Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion + "='" + Datos.P_Tipo_Asignacion + "' and " +
                        Cat_Nom_Percepcion_Deduccion.Campo_Estatus + "='ACTIVO')");


                //  filtro por clave de deduccion
                if (!String.IsNullOrEmpty(Datos.P_Clave_Deduccion))
                {
                    Mi_SQL.Append(" AND " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." +
                            Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + " in " + "(select " + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID +
                            " From " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " where " +
                            Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + "='" + Datos.P_Clave_Deduccion + "') ");
                }
                //  filtro por dependencia
                if (!String.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    Mi_SQL.Append(" AND " + Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + "." +
                            Ope_Nom_Perc_Var_Emp_Det.Campo_Empleado_ID + " in ( SELECT " + Cat_Empleados.Campo_Empleado_ID +
                            " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_Dependencia_ID +
                            "='" + Datos.P_Dependencia_ID + "') ");
                }

                //  filtro por numero de empleado
                if (!String.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    Mi_SQL.Append(" AND " + Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + "." +
                            Ope_Nom_Perc_Var_Emp_Det.Campo_Empleado_ID + " in ( SELECT " + Cat_Empleados.Campo_Empleado_ID +
                            " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_No_Empleado +
                            "='" + Datos.P_No_Empleado + "') ");
                }
                //  filtrar por tipo de nomina
                if (!String.IsNullOrEmpty(Datos.P_Tipo_Nomina_ID))
                {
                    Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." +
                            Cat_Empleados.Campo_Tipo_Nomina_ID + " in ( SELECT " + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID +
                            " FROM " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " WHERE " + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID +
                            "='" + Datos.P_Tipo_Nomina_ID + "') ");
                }

                // ordenar las columnas
                Mi_SQL.Append(" order by " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." +
                        Cat_Nom_Tipos_Nominas.Campo_Nomina + ", Nombre_Dependencia, Concepto, Nombre_Empleado");

                Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                return Dt_Consulta;
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        #endregion
    }
}