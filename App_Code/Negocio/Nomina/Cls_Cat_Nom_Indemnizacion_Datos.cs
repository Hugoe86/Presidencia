
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
using Presidencia.Indemnizacion.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Text;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;

namespace Presidencia.Indemnizacion.Datos
{
    public class Cls_Cat_Nom_Indemnizacion_Datos
    {
        #region (Métodos)

        #region (Métodos Operación)
        /// *****************************************************************************************************
        /// Nombre: Alta_Indemnizacion
        /// 
        /// Descripción: Ejecuta el alta de una indemnización.
        /// 
        /// Parámetros: Datos.- Información a guardar en al base de datos.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 20/Julio/2011
        /// Usuario modifico:
        /// Fecha Modifico:
        /// *****************************************************************************************************
        internal static Boolean Alta_Indemnizacion(Cls_Cat_Nom_Indemnizacion_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            OracleTransaction Transaccion = null;////Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            OracleConnection Conexion;//Variable para la conexión para la base de datos   
            OracleCommand Comando;//Sirve para la ejecución de las operaciones a la base de datos
            Object Indemnizacion_ID;//Identificador unico de la tabla de bancos.
            String Mensaje = String.Empty; //Variable que almacena el mensaje de estado de la operación
            Boolean Operacion_Completa = false;

            Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
            Comando = new OracleCommand();
            Conexion.Open();
            Transaccion = Conexion.BeginTransaction();
            Comando.Transaction = Transaccion;
            Comando.Connection = Conexion;

            try
            {
                //Consultas para el ID
                Mi_SQL.Append("SELECT NVL(MAX(" + Cat_Nom_Indemnizacion.Campo_Indemnizacion_ID + "), '00000') FROM " + Cat_Nom_Indemnizacion.Tabla_Cat_Nom_Indemnizacion);

                //Ejecutar consulta
                Comando.CommandText = Mi_SQL.ToString();
                Indemnizacion_ID = Comando.ExecuteScalar();

                //Verificar si no es nulo
                if (Convert.IsDBNull(Indemnizacion_ID) == false)
                    Datos.P_Indemnizacion_ID = String.Format("{0:00000}", Convert.ToInt32(Indemnizacion_ID) + 1);
                else
                    Datos.P_Indemnizacion_ID = "00001";

                Mi_SQL = new StringBuilder();

                Mi_SQL.Append("INSERT INTO " + Cat_Nom_Indemnizacion.Tabla_Cat_Nom_Indemnizacion + " (");
                Mi_SQL.Append(Cat_Nom_Indemnizacion.Campo_Indemnizacion_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Indemnizacion.Campo_Nombre + ", ");
                Mi_SQL.Append(Cat_Nom_Indemnizacion.Campo_Dias + ", ");
                Mi_SQL.Append(Cat_Nom_Indemnizacion.Campo_Comentarios + ", ");
                Mi_SQL.Append(Cat_Nom_Indemnizacion.Campo_Usuario_Creo + ", ");
                Mi_SQL.Append(Cat_Nom_Indemnizacion.Campo_Fecha_Creo + ") VALUES(");
                Mi_SQL.Append("'" + Datos.P_Indemnizacion_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_Nombre + "', ");
                Mi_SQL.Append("'" + Datos.P_Dias + "', ");
                Mi_SQL.Append("'" + Datos.P_Comentarios + "', ");
                Mi_SQL.Append("'" + Datos.P_Usuario_Creo + "', ");
                Mi_SQL.Append("SYSDATE)");

                //Ejecutar consulta
                Comando.CommandText = Mi_SQL.ToString();
                Comando.ExecuteNonQuery();

                //Ejecutar transaccion
                Transaccion.Commit();
                Conexion.Close();
                Operacion_Completa = true;
            }
            catch (OracleException Ex)
            {
                if (Transaccion != null)
                {
                    Transaccion.Rollback();
                }
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
                        break;
                    case "923":
                        Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
                        break;
                    case "12170":
                        Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
                        break;
                    default:
                        Mensaje = "Error:  [" + Ex.Message + "]";
                        break;
                }

                throw new Exception(Mensaje, Ex);
            }
            finally
            {
                Conexion.Close();

                Comando = null;
                Conexion = null;
                Transaccion = null;
            }
            return Operacion_Completa;
        }
        /// *****************************************************************************************************
        /// Nombre: Actualizar_Indemnizacion
        /// 
        /// Descripción: Ejecuta la actualización de una indemnización.
        /// 
        /// Parámetros: Datos.- Información a guardar en al base de datos.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 20/Julio/2011
        /// Usuario modifico:
        /// Fecha Modifico:
        /// *****************************************************************************************************
        internal static Boolean Actualizar_Indemnizacion(Cls_Cat_Nom_Indemnizacion_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            OracleTransaction Transaccion = null;////Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            OracleConnection Conexion;//Variable para la conexión para la base de datos   
            OracleCommand Comando;//Sirve para la ejecución de las operaciones a la base de datos
            String Mensaje = String.Empty; //Variable que almacena el mensaje de estado de la operación
            Boolean Operacion_Completa = false;

            Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
            Comando = new OracleCommand();
            Conexion.Open();
            Transaccion = Conexion.BeginTransaction();
            Comando.Transaction = Transaccion;
            Comando.Connection = Conexion;

            try
            {
                Mi_SQL.Append("UPDATE " + Cat_Nom_Indemnizacion.Tabla_Cat_Nom_Indemnizacion + " SET ");
                Mi_SQL.Append(Cat_Nom_Indemnizacion.Campo_Nombre + "='" + Datos.P_Nombre + "', ");
                Mi_SQL.Append(Cat_Nom_Indemnizacion.Campo_Dias + "='" + Datos.P_Dias + "', ");
                Mi_SQL.Append(Cat_Nom_Indemnizacion.Campo_Comentarios + "='" + Datos.P_Comentarios + "', ");
                Mi_SQL.Append(Cat_Nom_Indemnizacion.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Modifico + "', ");
                Mi_SQL.Append(Cat_Nom_Indemnizacion.Campo_Fecha_Modifico + "=SYSDATE");
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Nom_Indemnizacion.Campo_Indemnizacion_ID + "='" + Datos.P_Indemnizacion_ID + "'");

                //Ejecutar consulta
                Comando.CommandText = Mi_SQL.ToString();
                Comando.ExecuteNonQuery();

                //Ejecutar transaccion
                Transaccion.Commit();
                Conexion.Close();
                Operacion_Completa = true;
            }
            catch (OracleException Ex)
            {
                if (Transaccion != null)
                {
                    Transaccion.Rollback();
                }
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
                        break;
                    case "923":
                        Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
                        break;
                    case "12170":
                        Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
                        break;
                    default:
                        Mensaje = "Error:  [" + Ex.Message + "]";
                        break;
                }

                throw new Exception(Mensaje, Ex);
            }
            finally
            {
                Conexion.Close();

                Comando = null;
                Conexion = null;
                Transaccion = null;
            }
            return Operacion_Completa;
        }
        /// *****************************************************************************************************
        /// Nombre: Eliminar_Indemnizacion
        /// 
        /// Descripción: Ejecuta la baja de una indemnización.
        /// 
        /// Parámetros: Datos.- Información a guardar en al base de datos.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 20/Julio/2011
        /// Usuario modifico:
        /// Fecha Modifico:
        /// *****************************************************************************************************
        internal static Boolean Eliminar_Indemnizacion(Cls_Cat_Nom_Indemnizacion_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            OracleTransaction Transaccion = null;////Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            OracleConnection Conexion;//Variable para la conexión para la base de datos   
            OracleCommand Comando;//Sirve para la ejecución de las operaciones a la base de datos
            String Mensaje = String.Empty; //Variable que almacena el mensaje de estado de la operación
            Boolean Operacion_Completa = false;

            Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
            Comando = new OracleCommand();
            Conexion.Open();
            Transaccion = Conexion.BeginTransaction();
            Comando.Transaction = Transaccion;
            Comando.Connection = Conexion;

            try
            {
                Mi_SQL.Append("DELETE FROM " + Cat_Nom_Indemnizacion.Tabla_Cat_Nom_Indemnizacion);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Nom_Indemnizacion.Campo_Indemnizacion_ID + "='" + Datos.P_Indemnizacion_ID + "'");

                //Ejecutar consulta
                Comando.CommandText = Mi_SQL.ToString();
                Comando.ExecuteNonQuery();

                //Ejecutar transaccion
                Transaccion.Commit();
                Conexion.Close();
                Operacion_Completa = true;
            }
            catch (OracleException Ex)
            {
                if (Transaccion != null)
                {
                    Transaccion.Rollback();
                }
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
                        break;
                    case "923":
                        Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
                        break;
                    case "12170":
                        Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
                        break;
                    default:
                        Mensaje = "Error:  [" + Ex.Message + "]";
                        break;
                }

                throw new Exception(Mensaje, Ex);
            }
            finally
            {
                Conexion.Close();

                Comando = null;
                Conexion = null;
                Transaccion = null;
            }
            return Operacion_Completa;
        }
        #endregion

        #region (Métodos Consulta)
        /// *****************************************************************************************************
        /// Nombre: Consultar_Indemnizacion
        /// 
        /// Descripción: Consulta los registros de indemnización en la base de datos.
        /// 
        /// Parámetros: Datos.- Información a guardar en al base de datos.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 20/Julio/2011
        /// Usuario modifico:
        /// Fecha Modifico:
        /// *****************************************************************************************************
        internal static DataTable Consultar_Indemnizacion(Cls_Cat_Nom_Indemnizacion_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Indemnizaciones = null;//Variable que almacena los registros de indemnizacion encontrados en la bd.

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Nom_Indemnizacion.Tabla_Cat_Nom_Indemnizacion + ".*");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Nom_Indemnizacion.Tabla_Cat_Nom_Indemnizacion);

                if (!String.IsNullOrEmpty(Datos.P_Indemnizacion_ID))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                        Mi_SQL.Append(" AND " + Cat_Nom_Indemnizacion.Campo_Indemnizacion_ID + "='" + Datos.P_Indemnizacion_ID + "'");
                    else
                        Mi_SQL.Append(" WHERE " + Cat_Nom_Indemnizacion.Campo_Indemnizacion_ID + "='" + Datos.P_Indemnizacion_ID + "'");
                }

                if (!String.IsNullOrEmpty(Datos.P_Nombre))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                        Mi_SQL.Append(" AND UPPER(" + Cat_Nom_Indemnizacion.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Nombre + "%')");
                    else
                        Mi_SQL.Append(" WHERE UPPER(" + Cat_Nom_Indemnizacion.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Nombre + "%')");
                }

                if (!String.IsNullOrEmpty(Datos.P_Dias))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                        Mi_SQL.Append(" AND " + Cat_Nom_Indemnizacion.Campo_Dias + "='" + Datos.P_Dias + "'");
                    else
                        Mi_SQL.Append(" WHERE " + Cat_Nom_Indemnizacion.Campo_Dias + "='" + Datos.P_Dias + "'");
                }

                if (!String.IsNullOrEmpty(Datos.P_Comentarios))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                        Mi_SQL.Append(" AND UPPER(" + Cat_Nom_Indemnizacion.Campo_Comentarios + ") LIKE UPPER('%" + Datos.P_Comentarios + "%')");
                    else
                        Mi_SQL.Append(" WHERE UPPER(" + Cat_Nom_Indemnizacion.Campo_Comentarios + ") LIKE UPPER('%" + Datos.P_Comentarios + "%')");
                }

                Dt_Indemnizaciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las indemnizaciones. Error: [" + Ex.Message + "]");
            }
            return Dt_Indemnizaciones;
        }
        #endregion

        #endregion
    }
}
