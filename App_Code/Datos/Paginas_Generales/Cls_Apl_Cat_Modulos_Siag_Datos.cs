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
using System.Text;
using System.Collections.Generic;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Modulos_SIAG.Negocio;

namespace Presidencia.Modulos_SIAG.Datos
{
    public class Cls_Apl_Cat_Modulos_Siag_Datos
    {
        #region(Metodos)
        /// ********************************************************************************************************************
        /// NOMBRE:         Alta_Modulo
        /// COMENTARIOS:    Esta operación inserta un nuevo registro de un movimiento presupuestal en la tabla 
        /// PARÁMETROS:     Datos.- Valor de los campos a insertar en la tabla
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     10/Enero/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static Boolean Alta_Modulo_Siag(Cls_Apl_Cat_Modulos_Siag_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            OracleTransaction Transaccion = null;////Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            OracleConnection Conexion;//Variable para la conexión para la base de datos   
            OracleCommand Comando;//Sirve para la ejecución de las operaciones a la base de datos
            Object No_Modulo_Max;//Identificador el elemento de la busque (cual es el mayor id de solicitud)
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
                //se sacara el valor para el modulo id
                Mi_SQL.Append("SELECT NVL(MAX (" + Apl_Cat_Modulos_Siag.Campo_Modulo_ID + "),0) ");
                Mi_SQL.Append("FROM " + Apl_Cat_Modulos_Siag.Tabla_Apl_Cat_Modulos_Siag);
                No_Modulo_Max = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                if (Convert.IsDBNull(No_Modulo_Max) == false)
                {

                    Datos.P_Modulo_ID = String.Format("{0:00000}", Convert.ToInt32(No_Modulo_Max) + 1);
                }
                else
                {

                    Datos.P_Modulo_ID = "00001";
                }
                Mi_SQL = new StringBuilder();

                //Se dara de alta el registro
                Mi_SQL.Append("Insert into " + Apl_Cat_Modulos_Siag.Tabla_Apl_Cat_Modulos_Siag + "(");
                Mi_SQL.Append(Apl_Cat_Modulos_Siag.Campo_Modulo_ID + "," +
                                Apl_Cat_Modulos_Siag.Campo_Nombre + "," +
                                Apl_Cat_Modulos_Siag.Campo_Usuario_Creo + "," +
                                Apl_Cat_Modulos_Siag.Campo_Fecha_Creo);

                Mi_SQL.Append(") Values (");
                
                Mi_SQL.Append("'" + Datos.P_Modulo_ID + "','" + Datos.P_Nombre + "'," +
                                "'" + Datos.P_Usuario_Creo + "',SYSDATE)");

                //Ejecutar consulta
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

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

        /// ********************************************************************************************************************
        /// NOMBRE:         Modificar_Modulo
        /// COMENTARIOS:    Esta operación actualiza un registro del movimiento en la tabla
        /// PARÁMETROS:     Datos.- Valor de los campos a Modificar en la tabla
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     10/Enero/2012
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static Boolean Modificar_Modulo_Siag(Cls_Apl_Cat_Modulos_Siag_Negocio Datos)
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
                Mi_SQL.Append("UPDATE " + Apl_Cat_Modulos_Siag.Tabla_Apl_Cat_Modulos_Siag + " SET ");
                Mi_SQL.Append(Apl_Cat_Modulos_Siag.Campo_Nombre + "='" + Datos.P_Nombre + "',");
                Mi_SQL.Append(Apl_Cat_Modulos_Siag.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Modifico + "',");
                Mi_SQL.Append(Apl_Cat_Modulos_Siag.Campo_Fecha_Modifico + "=SYSDATE");
                Mi_SQL.Append(" Where " + Apl_Cat_Modulos_Siag.Campo_Modulo_ID + "='" + Datos.P_Modulo_ID + "'");
                

                //Ejecutar consulta
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

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
        /// ********************************************************************************************************************
        /// NOMBRE:         Eliminar_Modulo
        /// COMENTARIOS:    Esta operación eliminara un registro del movimiento que se haya realizado en la tabla
        /// PARÁMETROS:     Datos.- Valor de los campos a eliminar en la tabla
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     10/Enero/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static Boolean Eliminar_Modulo_Siag(Cls_Apl_Cat_Modulos_Siag_Negocio Datos)
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
                Mi_SQL.Append("Delete From " + Apl_Cat_Modulos_Siag.Tabla_Apl_Cat_Modulos_Siag + " ");
                Mi_SQL.Append("Where " + Apl_Cat_Modulos_Siag.Campo_Modulo_ID + "='" + Datos.P_Modulo_ID + "'");
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
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
        /// ********************************************************************************************************************
        /// NOMBRE:         Consultar_Modulo_Siag
        /// COMENTARIOS:    Consulta el movimiento presupuestal que se haya llevado en la tabla
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar en la tabla
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     10/Enero/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Modulo_Siag(Cls_Apl_Cat_Modulos_Siag_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Mi_SQL.Append("Select " + Apl_Cat_Modulos_Siag.Tabla_Apl_Cat_Modulos_Siag + ".* ");
                Mi_SQL.Append("From " + Apl_Cat_Modulos_Siag.Tabla_Apl_Cat_Modulos_Siag);

                if (!string.IsNullOrEmpty(Datos.P_Modulo_ID))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" OR " + Apl_Cat_Modulos_Siag.Campo_Modulo_ID + "='" + Datos.P_Modulo_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Apl_Cat_Modulos_Siag.Campo_Modulo_ID + "='" + Datos.P_Modulo_ID + "'");
                    }
                }
                if (!string.IsNullOrEmpty(Datos.P_Nombre))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" OR " + Apl_Cat_Modulos_Siag.Campo_Nombre + "='" + Datos.P_Nombre + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Apl_Cat_Modulos_Siag.Campo_Nombre + "='" + Datos.P_Nombre + "'");
                    }
                }

                Mi_SQL.Append(" Order by " + Apl_Cat_Modulos_Siag.Campo_Nombre + " asc");
                Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Movimientos Presupuestales que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
            return Dt_Consulta;
        }

        #endregion
    }
}
