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
using System.Text;
using Presidencia.Constantes;
using Presidencia.Asistencias.Negocio;

namespace Presidencia.Asistencias.Datos
{
    public class Cls_Ope_Nom_Asistencias_Datos
    {
        public Cls_Ope_Nom_Asistencias_Datos()
        {
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Asistencia
        /// DESCRIPCION : 1.Consulta el último No dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta la Aistencia del Empleado en la BD con los datos
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 27-Julio-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Asistencia(Cls_Ope_Nom_Asistencias_Negocio Datos)
        {
            String Mi_SQL;        //Obtiene la cadena de inserción hacía la base de datos
            Object No_Asistencia; //Obtiene el No con la cual se guardo los datos en la base de datos

            try
            {
                //Consulta el último No de Asistencia que fue agregado a la base de datos
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Nom_Asistencias.Campo_No_Asistencia + "),'0000000000') ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Nom_Asistencias.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'";
                No_Asistencia = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(No_Asistencia))
                {
                    Datos.P_No_Asistencia = "0000000001";
                }
                else
                {
                    Datos.P_No_Asistencia = String.Format("{0:0000000000}", Convert.ToInt32(No_Asistencia) + 1);
                }
                //Consulta para la inserción de los datos
                Mi_SQL = "INSERT INTO " + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "(" +
                       Ope_Nom_Asistencias.Campo_No_Asistencia + ", " + Ope_Nom_Asistencias.Campo_Empleado_ID + ", " +
                       Ope_Nom_Asistencias.Campo_Reloj_Checador_ID + ", " + 
                       Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada + ", " +
                       Ope_Nom_Asistencias.Campo_Fecha_Hora_Salida + ", " + Ope_Nom_Asistencias.Campo_Usuario_Creo + ", " +
                       Ope_Nom_Asistencias.Campo_Fecha_Creo + ") VALUES ('" +
                       Datos.P_No_Asistencia + "', '" + Datos.P_Empleado_ID + "', '" + Datos.P_Reloj_Checador_ID + "', " +
                       "TO_TIMESTAMP ('" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", Datos.P_Fecha_Hora_Entrada) + "', 'DD/MM/YYYY HH24:MI:SS'), " +
                       "TO_TIMESTAMP ('" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", Datos.P_Fecha_Hora_Salida) + "', 'DD/MM/YYYY HH24:MI:SS'), '" +
                       Datos.P_Nombre_Usuario + "', SYSDATE)";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Asistencia
        /// DESCRIPCION : Modifica los datos de la Asistencia con lo que fueron introducidos por el
        ///               usuario
        /// PARAMETROS  :  Datos: Datos que son enviados de la capa de Negocios y que fueron 
        ///                       proporcionados por el usuario y van a sustituir a los datos que se
        ///                       encuentran en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 27-Julio-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Modificar_Asistencia(Cls_Ope_Nom_Asistencias_Negocio Datos)
        {
            String Mi_SQL;//Obtiene la cadena de modificación hacía la base de datos
            try
            { 
                //Consulta para la modificación de la asistencia con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + " SET " +
                         Ope_Nom_Asistencias.Campo_Reloj_Checador_ID + " = '" + Datos.P_Reloj_Checador_ID + "', " +
                         Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada + " = TO_TIMESTAMP ('" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", Datos.P_Fecha_Hora_Entrada) + "', 'DD/MM/YYYY HH24:MI:SS'), " +
                         Ope_Nom_Asistencias.Campo_Fecha_Hora_Salida + " = TO_TIMESTAMP ('" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", Datos.P_Fecha_Hora_Salida) + "', 'DD/MM/YYYY HH24:MI:SS'), " +
                         Ope_Nom_Asistencias.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', " +
                         Ope_Nom_Asistencias.Campo_Fecha_Modifico + " = SYSDATE WHERE " + 
                         Ope_Nom_Asistencias.Campo_No_Asistencia + " = '" + Datos.P_No_Asistencia + "' AND " +
                         Ope_Nom_Asistencias.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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
        /// NOMBRE DE LA FUNCION: Elimina_Asistencia
        /// DESCRIPCION : Elimina la asistencia que fue seleccionada por el usuario de la BD
        /// PARAMETROS  : Datos: Obtiene que dependencia desea eliminar de la BD
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 27-Julio-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Eliminar_Asistencia(Cls_Ope_Nom_Asistencias_Negocio Datos)
        {
            String Mi_SQL; //Variable de Consulta para la eliminación de la asistencia

            try
            { 
                Mi_SQL="DELETE FROM " + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias +
                        " WHERE " + Ope_Nom_Asistencias.Campo_No_Asistencia + " = '" + Datos.P_No_Asistencia + "'" +
                        " AND " + Ope_Nom_Asistencias.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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
        #region (Métodos Consulta)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Datos_Empleado
            /// DESCRIPCION : Consulta los datos generales del empleado que se desea consultar
            ///               las asistencias
            /// PARAMETROS  : Datos: Obtiene el ID del empleado a consultar
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 03-Agosto-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Datos_Empleado(Cls_Ope_Nom_Asistencias_Negocio Datos)
            {
                String Mi_SQL; //Variable para la consulta de los datos del empleado

                try 
                {
                    Mi_SQL = "SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + ", " +
                             Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", " +
                             Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + ", " +
                             Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Estatus + ", " +
                             "(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno +
                             "||' '||" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno +
                             "||' '||" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS Empleado, " +
                             Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS Unidad_Responsable" +
                             " FROM " + Cat_Empleados.Tabla_Cat_Empleados + ", " + Cat_Dependencias.Tabla_Cat_Dependencias +
                             " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID +
                             " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'";
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
            /// NOMBRE DE LA FUNCION: Consulta_Asistencia
            /// DESCRIPCION : Consulta las Asistencias del empleado que estan dadas de alta en
            ///               la BD con todos sus datos
            /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 04-Agosto-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Asistencia(Cls_Ope_Nom_Asistencias_Negocio Datos)
            {
                String Mi_SQL; //Variable para la consulta de las asistencia
                try
                {
                    Mi_SQL = "SELECT " + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + ".*, " + 
                             Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador+ "." + Cat_Nom_Reloj_Checador.Campo_Reloj_Checador_ID + "," +
                             "(" + Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador + "." + Cat_Nom_Reloj_Checador.Campo_Clave + "||' '||" +
                             Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador + "." + Cat_Nom_Reloj_Checador.Campo_Ubicacion + ") AS Reloj" +
                             " FROM " + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + ", " + Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador +
                             " WHERE " + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "." + Ope_Nom_Asistencias.Campo_Reloj_Checador_ID + " = " + 
                             Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador + "." + Cat_Nom_Reloj_Checador.Campo_Reloj_Checador_ID +
                             " AND " + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "." + Ope_Nom_Asistencias.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'";
                    if (!String.IsNullOrEmpty(Datos.P_Fecha_Inicio) && !String.IsNullOrEmpty(Datos.P_Fecha_Termino))
                    {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio + " 00:00:00', 'DD/MM/YYYY HH24:MI:SS')" +
                                          " AND TO_DATE ('" + Datos.P_Fecha_Termino + " 23:59:00', 'DD/MM/YYYY HH24:MI:SS')";
                    }

                    Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "." + Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada + " DESC";
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
        #endregion
    }
}