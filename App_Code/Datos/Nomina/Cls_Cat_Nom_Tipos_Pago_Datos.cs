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
using Presidencia.Nomina_Tipos_Pago.Negocio;

namespace Presidencia.Nomina_Tipos_Pago.Datos
{
    public class Cls_Cat_Nom_Tipos_Pago_Datos
    {
         #region(Metodos)
        /// ********************************************************************************************************************
        /// NOMBRE:         Alta_Tipo_Pago
        /// COMENTARIOS:    Esta operación inserta un nuevo registro 
        /// PARÁMETROS:     Datos.- Valor de los campos a insertar en la tabla 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     06/Enero/2012
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static Boolean Alta_Tipo_Pago(Cls_Cat_Nom_Tipos_Pago_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            OracleTransaction Transaccion = null;////Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            OracleConnection Conexion;//Variable para la conexión para la base de datos   
            OracleCommand Comando;//Sirve para la ejecución de las operaciones a la base de datos
            Object No_Solicitud_Max;//Identificador el elemento de la busque (cual es el mayor id de solicitud)
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
                Mi_SQL.Append("SELECT NVL(MAX (" + Cat_Nom_Tipos_Pagos.Campo_Tipo_Pago_ID + "),0) ");
                Mi_SQL.Append("FROM " + Cat_Nom_Tipos_Pagos.Tabla_Cat_Nom_Tipos_Pagos);
                No_Solicitud_Max = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                if (Convert.IsDBNull(No_Solicitud_Max) == false)
                {

                    Datos.P_Tipo_Pago_ID = String.Format("{0:00000}", Convert.ToInt32(No_Solicitud_Max) + 1);
                }
                else
                {

                    Datos.P_Tipo_Pago_ID = "00001";
                }
                Mi_SQL = new StringBuilder();

                Mi_SQL.Append("INSERT INTO " + Cat_Nom_Tipos_Pagos.Tabla_Cat_Nom_Tipos_Pagos + "(");
                Mi_SQL.Append(Cat_Nom_Tipos_Pagos.Campo_Tipo_Pago_ID + ",");
                Mi_SQL.Append(Cat_Nom_Tipos_Pagos.Campo_Nombre + ",");
                Mi_SQL.Append(Cat_Nom_Tipos_Pagos.Campo_Usuario_Creo + ",");
                Mi_SQL.Append(Cat_Nom_Tipos_Pagos.Campo_Fecha_Creo);
                Mi_SQL.Append(") VALUES (" );
                Mi_SQL.Append("'" + Datos.P_Tipo_Pago_ID + "',");
                Mi_SQL.Append("'" + Datos.P_Nombre + "',");
                Mi_SQL.Append("'" + Datos.P_Usuario_Creo + "',");
                Mi_SQL.Append("SYSDATE) ");
                
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
        /// NOMBRE:         Modificar_Tipo_Pago
        /// COMENTARIOS:    Esta operación modifica un registro 
        /// PARÁMETROS:     Datos.- Valor de los campos a insertar en la tabla 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     06/Enero/2012
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static Boolean Modificar_Tipo_Pago(Cls_Cat_Nom_Tipos_Pago_Negocio Datos)
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
                Mi_SQL.Append("UPDATE " + Cat_Nom_Tipos_Pagos.Tabla_Cat_Nom_Tipos_Pagos+ " SET ");
                Mi_SQL.Append(Cat_Nom_Tipos_Pagos.Campo_Nombre + "='" + Datos.P_Nombre + "',");
                Mi_SQL.Append(Cat_Nom_Tipos_Pagos.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Modifico + "',");
                Mi_SQL.Append(Cat_Nom_Tipos_Pagos.Campo_Fecha_Modifico + "=SYSDATE ");
                Mi_SQL.Append(" WHERE " + Cat_Nom_Tipos_Pagos.Campo_Tipo_Pago_ID + "='" + Datos.P_Tipo_Pago_ID + "'");

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
        /// NOMBRE:         Eliminar_Tipo_Pago
        /// COMENTARIOS:    Esta operación elimina un registro 
        /// PARÁMETROS:     Datos.- Valor de los campos a insertar en la tabla 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     06/Enero/2012
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static Boolean Eliminar_Tipo_Pago(Cls_Cat_Nom_Tipos_Pago_Negocio Datos)
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
                Mi_SQL.Append("DELETE FROM " + Cat_Nom_Tipos_Pagos.Tabla_Cat_Nom_Tipos_Pagos);
                Mi_SQL.Append(" WHERE " + Cat_Nom_Tipos_Pagos.Campo_Tipo_Pago_ID + "='" + Datos.P_Tipo_Pago_ID + "'");
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
        /// NOMBRE:         Consultar_Tipo_Pago
        /// COMENTARIOS:    Esta operación consulta los registros 
        /// PARÁMETROS:     Datos.- Valor de los campos a insertar en la tabla 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     06/Enero/2012
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Tipo_Pago(Cls_Cat_Nom_Tipos_Pago_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            try
            {
                Mi_SQL.Append("Select * ");
                Mi_SQL.Append("From " + Cat_Nom_Tipos_Pagos.Tabla_Cat_Nom_Tipos_Pagos);

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Pago_ID))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" OR " + Cat_Nom_Tipos_Pagos.Campo_Tipo_Pago_ID + "='" + Datos.P_Tipo_Pago_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Nom_Tipos_Pagos.Campo_Tipo_Pago_ID + "='" + Datos.P_Tipo_Pago_ID + "'");
                    }
                }
                if (!string.IsNullOrEmpty(Datos.P_Nombre))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" OR " + Cat_Nom_Tipos_Pagos.Campo_Nombre + "='" + Datos.P_Nombre + "'");
                        Mi_SQL.Append(" OR " + Cat_Nom_Tipos_Pagos.Campo_Nombre + " LIKE '" + Datos.P_Nombre + "%'");
                        Mi_SQL.Append(" OR " + Cat_Nom_Tipos_Pagos.Campo_Nombre + " LIKE '%" + Datos.P_Nombre + "%'");
                        Mi_SQL.Append(" OR " + Cat_Nom_Tipos_Pagos.Campo_Nombre + " LIKE '%" + Datos.P_Nombre + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Nom_Tipos_Pagos.Campo_Nombre + "='" + Datos.P_Nombre + "'");
                        Mi_SQL.Append(" OR " + Cat_Nom_Tipos_Pagos.Campo_Nombre + " LIKE '" + Datos.P_Nombre + "%'");
                        Mi_SQL.Append(" OR " + Cat_Nom_Tipos_Pagos.Campo_Nombre + " LIKE '%" + Datos.P_Nombre + "%'");
                        Mi_SQL.Append(" OR " + Cat_Nom_Tipos_Pagos.Campo_Nombre + " LIKE '%" + Datos.P_Nombre + "'");
                    }
                }
                Mi_SQL.Append(" Order by " + Cat_Nom_Tipos_Pagos.Campo_Nombre + " asc");
               
                //se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Tipos de pago que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
        }
        #endregion
       
    }
}
