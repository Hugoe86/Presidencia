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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Faltas_Empleado.Negocio;
using System.Text;

namespace Presidencia.Faltas_Empleado.Datos
{
    public class Cls_Ope_Nom_Faltas_Empleado_Datos
    {
        #region (Metodos)
        
        #region (Metodos Consulta)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Dependencias
        /// DESCRIPCION : Consultalas dependecncias existentes en la base de datos.
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 12/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Dependencias() {
            String Mi_Oracle = "";
            try
            {
                Mi_Oracle = "SELECT " + Cat_Dependencias.Campo_Nombre + ", " + Cat_Dependencias.Campo_Dependencia_ID  + " FROM " +
                    Cat_Dependencias.Tabla_Cat_Dependencias;

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las Dependencias en la Base de Datos. Error [" + Ex.Message + "]");
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Empleados
        /// DESCRIPCION : Consulta los empleados por la dependencia a la que pertenecen.
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 12/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Empleados(Cls_Ope_Nom_Faltas_Empleado_Negocio Datos)
        {
            String Mi_Oracle = "";
            try
            {
                Mi_Oracle = "SELECT " + Cat_Empleados.Campo_Empleado_ID + ", " +
                            "(" + Cat_Empleados.Campo_Apellido_Paterno +
                            "||' '||" + Cat_Empleados.Campo_Apellido_Materno +
                            "||' '||" + Cat_Empleados.Campo_Nombre + ") AS NOMBRE" +
                            " FROM " + Cat_Empleados.Tabla_Cat_Empleados+
                            " WHERE " +
                            Cat_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID  + "'";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Empleados en la Base de Datos por Dependencia. Error [" + Ex.Message + "]");
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Faltas_Empleado
        /// DESCRIPCION : Consulta las Faltas que ha tenido el Empleado.
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 12/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Faltas_Empleado(Cls_Ope_Nom_Faltas_Empleado_Negocio Datos) {
            String Mi_Oracle = null;
            try
            {
                Mi_Oracle = "SELECT " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + ".*, " +
                                Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA " +
                            " FROM " +
                                Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + ", " + Cat_Dependencias.Tabla_Cat_Dependencias +
                            " WHERE (" +
                                Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Dependencia_ID +
                            "=" +
                                Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + ")";

                if (!string.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Estatus))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Falta))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Tipo_Falta + "='" + Datos.P_Tipo_Falta + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Tipo_Falta + "='" + Datos.P_Tipo_Falta + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Fecha_Inicio) && !string.IsNullOrEmpty(Datos.P_Fecha_Fin))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Fecha + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Datos.P_Fecha_Fin + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Fecha + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Datos.P_Fecha_Fin + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                }

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las Faltas del Empleado Seleccionado. Error: ["+ Ex.Message+"]");
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Empleados
        /// DESCRIPCION : Consulta todos los Empleados que estan dados de alta en la BD y que
        ///               tengan alguna similitud con lo proporcionado por el usuario
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 10-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Empleados_Avanzada(Cls_Ope_Nom_Faltas_Empleado_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta para el Empleado

            try
            {
                //Consulta todos los Empleados que coincidan con lo proporcionado por el usuario
                Mi_SQL = "SELECT " + Cat_Empleados.Campo_Empleado_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Dependencia_ID + ", ";
                Mi_SQL = Mi_SQL + "(" + Cat_Empleados.Campo_Apellido_Paterno;
                Mi_SQL = Mi_SQL + "||' '||" + Cat_Empleados.Campo_Apellido_Materno;
                Mi_SQL = Mi_SQL + "||' '||" + Cat_Empleados.Campo_Nombre + ") AS NOMBRE";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;

                if (Datos.P_Nombre_Empleado != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE (" + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||";
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Apellido_Materno + ") LIKE '%" + Datos.P_Nombre_Empleado + "%'";                                        
                }
                if (Datos.P_Empleado_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Empleados.Campo_Apellido_Paterno;
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
        /// NOMBRE DE LA FUNCION: Consulta_Empleados
        /// DESCRIPCION : Consulta todos los Empleados que estan dados de alta en la BD y que
        ///               tengan alguna similitud con lo proporcionado por el usuario
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 10-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Dependencia_Del_Empelado(Cls_Ope_Nom_Faltas_Empleado_Negocio Datos)
        {
            String Mi_Oracle = null;
            try
            {
                Mi_Oracle = "SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID +
                            " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Ope_Nom_Faltas_Empleado.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las Faltas del Empleado Seleccionado. Error: [" + Ex.Message + "]");
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Periodo_Por_Fecha
        /// DESCRIPCION : Consultar las nomina que seran validas.
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 08/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Periodo_Por_Fecha(Cls_Ope_Nom_Faltas_Empleado_Negocio Datos)
        {
            String Mi_Oracle = "";//Variable que almacenara la consulta.
            DataTable Dt_Periodos_Nomina = null;//Variable que lamcenara una lista con los periodos de la nomina seleccionada.

            try
            {
                Mi_Oracle = "select *  from" +
                "(SELECT " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + ".*  FROM " +
                Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles +
                " WHERE " + Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio +
                " <= TO_DATE ('" + Datos.P_Fecha + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS') order by " +
                Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin + " desc) where" +
                " rownum =1";

                Dt_Periodos_Nomina = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error generado al consultar el periodo actual de la nómina. Error: [" + Ex.Message + "]");
            }
            return Dt_Periodos_Nomina;
        }
        /// ********************************************************************************************************************
        /// NOMBRE: Consultar_Faltas_General
        /// 
        /// DESCRIPCIÓN: Consulta las faltas que existen registradas actualmente en el sistema.
        /// 
        /// PARÁMETROS: Datos: Información a actualizar en la base de datos.
        /// 
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 06/Abril/2011 17:11 pm.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACION:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Faltas_General(Cls_Ope_Nom_Faltas_Empleado_Negocio Datos)
        {
            DataTable Dt_Faltas_Empleados = null;//Variable que almacenara una lista de las antiguedades que evaluaran los sindicatos.
            String Mi_SQL = "";//Variable que almacenara la consulta.
            try
            {
                Mi_SQL = "SELECT " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + ".*, " +
                                Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA, " +
                                " (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + "|| ' ' ||" +
                                Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "|| ' ' || " +
                                Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ") As Nombre" +
                            " FROM " +
                                Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + ", " + Cat_Dependencias.Tabla_Cat_Dependencias +
                                ", " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE (" +
                                Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Dependencia_ID +
                            "=" +
                                Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + ") AND " +
                            "(" + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Empleado_ID + "=" +
                            Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + ")";

                if (!string.IsNullOrEmpty(Datos.P_No_Falta))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_No_Falta + "='" + Datos.P_No_Falta + "'";
                    }
                    else
                    {
                        Mi_SQL += " WHERE " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_No_Falta + "='" + Datos.P_No_Falta + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    }
                    else
                    {
                        Mi_SQL += " WHERE " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    }
                    else
                    {
                        Mi_SQL += " WHERE " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Estatus))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                    }
                    else
                    {
                        Mi_SQL += " WHERE " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Nomina_ID))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'";
                    }
                    else
                    {
                        Mi_SQL += " WHERE " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'";
                    }
                }

                if (Datos.P_No_Nomina is Int32 && Datos.P_No_Nomina > 0)
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_No_Nomina + "=" + Datos.P_No_Nomina;
                    }
                    else
                    {
                        Mi_SQL += " WHERE " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_No_Nomina + "=" + Datos.P_No_Nomina;
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Fecha_Inicio) && !string.IsNullOrEmpty(Datos.P_Fecha_Fin))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Fecha + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Datos.P_Fecha_Fin + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                    else
                    {
                        Mi_SQL += " WHERE " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Fecha + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Datos.P_Fecha_Fin + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                }

                Dt_Faltas_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las Faltas de los empleados. Error: [" + Ex.Message + "]");
            }
            return Dt_Faltas_Empleados;
        }
        #endregion

        #region (Metodos Operacion)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Falta_Empleado
        /// DESCRIPCION : Ejecutar alta de la Falta de un Empleado
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 12/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Alta_Falta_Empleado(Cls_Ope_Nom_Faltas_Empleado_Negocio Datos) {
            Boolean Operacion_Completa = false;
            String Mi_Oracle = "";
            object No_Falta = null;

            try
            {
                Mi_Oracle = "SELECT NVL(MAX(" + Ope_Nom_Faltas_Empleado.Campo_No_Falta+ "),'00000') ";
                Mi_Oracle = Mi_Oracle + "FROM " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado;
                No_Falta = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);

                if (Convert.IsDBNull(No_Falta))
                {
                    Datos.P_No_Falta = "00001";
                }
                else
                {
                    Datos.P_No_Falta= String.Format("{0:00000}", Convert.ToInt32(No_Falta) + 1);
                }

                Mi_Oracle = "INSERT INTO " +
                            Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado +
                            " (" +
                            Ope_Nom_Faltas_Empleado.Campo_No_Falta + ", " +
                            Ope_Nom_Faltas_Empleado.Campo_Empleado_ID + ", " +
                            Ope_Nom_Faltas_Empleado.Campo_Dependencia_ID + ", " +
                            Ope_Nom_Faltas_Empleado.Campo_Fecha + ", " +
                            Ope_Nom_Faltas_Empleado.Campo_Tipo_Falta + ", " +
                            Ope_Nom_Faltas_Empleado.Campo_Retardo + ", " +
                            Ope_Nom_Faltas_Empleado.Campo_Cantidad + ", " +
                            Ope_Nom_Faltas_Empleado.Campo_Comentarios + ", " +
                            Ope_Nom_Faltas_Empleado.Campo_Usuario_Creo + ", " +
                            Ope_Nom_Faltas_Empleado.Campo_Fecha_Creo + ", " +
                            Ope_Nom_Faltas_Empleado.Campo_Nomina_ID + ", " +
                            Ope_Nom_Faltas_Empleado.Campo_No_Nomina + ", " + 
                            Ope_Nom_Faltas_Empleado.Campo_Estatus +
                            ") VALUES(" +
                            "'" + Datos.P_No_Falta + "', " +
                            "'" + Datos.P_Empleado_ID + "', " +
                            "'" + Datos.P_Dependencia_ID + "', " +
                            "'" + Datos.P_Fecha + "', " +
                            "'" + Datos.P_Tipo_Falta + "', " +
                            "'" + Datos.P_Retardo + "', " +
                            "" + Datos.P_Cantidad + ", " +
                            "'" + Datos.P_Comentarios + "', " +
                            "'" + Datos.P_Usuario_Creo + "', SYSDATE, " +
                            "'" + Datos.P_Nomina_ID + "', " + Datos.P_No_Nomina + ", 'Autorizado')";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Ejecutar el alta de una Falta del Empleado. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Falta_Empleado
        /// DESCRIPCION : Ejecutar Actualizacion de la Falta de un Empleado
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 12/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Modificar_Falta_Empleado(Cls_Ope_Nom_Faltas_Empleado_Negocio Datos)
        {
            Boolean Operacion_Completa = false;
            String Mi_Oracle = "";
            try
            {
                Mi_Oracle = "UPDATE " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado +
                            " SET " +
                            Ope_Nom_Faltas_Empleado.Campo_Fecha + "='" + Datos.P_Fecha + "', " +
                            Ope_Nom_Faltas_Empleado.Campo_Tipo_Falta + "='" + Datos.P_Tipo_Falta + "', " +
                            Ope_Nom_Faltas_Empleado.Campo_Retardo + "='" + Datos.P_Retardo + "', " +
                            Ope_Nom_Faltas_Empleado.Campo_Cantidad + "=" + Datos.P_Cantidad + ", " +
                            Ope_Nom_Faltas_Empleado.Campo_Comentarios + "='" + Datos.P_Comentarios + "', " +
                            Ope_Nom_Faltas_Empleado.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Modifico + "', " +
                            Ope_Nom_Faltas_Empleado.Campo_Fecha_Modifico + "= SYSDATE" + ", " +
                            Ope_Nom_Faltas_Empleado.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "', " +
                            Ope_Nom_Faltas_Empleado.Campo_No_Nomina + "=" + Datos.P_No_Nomina + 
                            " WHERE " +
                            Ope_Nom_Faltas_Empleado.Campo_No_Falta + "='" + Datos.P_No_Falta +"'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Ejecutar la Modificacion de la Falta del Empleado. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Eliminar_Falta_Empleado
        /// DESCRIPCION : Ejecutar la Baja de la Falta de un Empleado
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 12/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Eliminar_Falta_Empleado(Cls_Ope_Nom_Faltas_Empleado_Negocio Datos)
        {
            Boolean Operacion_Completa = false;
            String Mi_Oracle = "";
            try
            {
                Mi_Oracle = "DELETE FROM " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + 
                            " WHERE " +
                            Ope_Nom_Faltas_Empleado.Campo_No_Falta + "='" + Datos.P_No_Falta + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Ejecutar la Baja de una Falta del Empleado. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }
        /// ********************************************************************************************************************
        /// NOMBRE: Modificar_Incapacidad
        /// 
        /// DESCRIPCIÓN: Ejecuta la modificación de una Incapacidad.
        /// 
        /// PARÁMETROS: Datos: Información a actualizar en la base de datos.
        /// 
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 06/Abril/2011 17:11 pm.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACION:
        /// ********************************************************************************************************************
        public static Boolean Cambiar_Estatus_Faltas_Empleados(Cls_Ope_Nom_Faltas_Empleado_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Obtiene la cadena de inserción hacía la base de datos
            String Mensaje = ""; //Obtiene la descripción del error ocurrido durante la ejecución de Mi_SQL
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Boolean Operacion_Completa = false;

            Cn.ConnectionString = Cls_Constantes.Str_Conexion;
            Cn.Open();
            //Esta inserción se realiza sin el Ayudante de SQL y con el BeginTrans y Commit para proteger la información
            //el ayudante de SQL solo debe usarse cuando solo se afecte una tabla o para movimientos que NO son críticos
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;

            try
            {
                Mi_SQL.Append("UPDATE " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + " SET ");
                Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Campo_Estatus + "='" + Datos.P_Estatus + "', ");
                Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Modifico + "', ");
                Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Campo_Fecha_Modifico + "=SYSDATE");
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Campo_No_Falta + "='" + Datos.P_No_Falta + "'");

                //Ejecutar la consulta
                Cmd.CommandText = Mi_SQL.ToString();
                Cmd.ExecuteNonQuery();

                //Ejecutar transaccion
                Trans.Commit();
                Operacion_Completa = true;
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code.ToString().Equals("8152"))
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar";
                }
                else if (Ex.Code.ToString().Equals("2627"))
                {
                    if (Ex.Message.IndexOf("PRIMARY") != -1)
                    {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar";
                    }
                    else if (Ex.Message.IndexOf("UNIQUE") != -1)
                    {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor";
                    }
                    else
                    {
                        Mensaje = "Error general en la base de datos";
                    }
                }
                else if (Ex.Code.ToString().Equals("547"))
                {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla";
                }
                else if (Ex.Code.ToString().Equals("515"))
                {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar";
                }
                else
                {
                    Mensaje = Ex.Message; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            catch (DBConcurrencyException Ex)
            {
                //Indicamos el mensaje 
                throw new Exception("Lo siento, los datos fueron actualizados por otro Rol. Error: [" + Ex.Message + "]");

            }
            catch (Exception Ex)
            {
                //Indicamos el mensaje 
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Cn.Close();
            }
            return Operacion_Completa;
        }
        #endregion

        #endregion
    }
}