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
using Presidencia.Movimiento_Presupuestal.Negocio;



namespace Presidencia.Movimiento_Presupuestal.Datos
{
    public class Cls_Ope_Pre_Movimiento_Presupuestal_Datos
    {
        #region(Metodos)
        /// ********************************************************************************************************************
        /// NOMBRE: Alta_Movimiento
        /// 
        /// COMENTARIOS: Esta operación inserta un nuevo registro de un movimiento presupuestal en la tabla de 
        /// 
        /// PARÁMETROS: Datos.- Valor de los campos a insertar en la tabla de 
        /// 
        /// USUARIO CREÓ: Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ: 18/Octubre/2011 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static Boolean Alta_Movimiento(Cls_Ope_Pre_Movimiento_Presupuestal_Negocio Datos)
        {

            StringBuilder My_SQL = new StringBuilder();//Variable que almacenara la consulta.
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
                My_SQL.Append("SELECT NVL(MAX (" + Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud + "),0) ");
                My_SQL.Append("FROM " + Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf );
                No_Solicitud_Max = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, My_SQL.ToString());

                if (Convert.IsDBNull(No_Solicitud_Max) == false ) 
                {
                    
                    Datos.P_No_Solicitud = String.Format("{0:00000}", Convert.ToInt32(No_Solicitud_Max) + 1);
                }
                else 
                {

                    Datos.P_No_Solicitud = "00001";
                }
                My_SQL = new StringBuilder();
                //Da de Alta los datos del Nuevo Parametro con los datos proporcionados por el usuario.
                My_SQL.Append("INSERT INTO " + Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf  + " (");
                My_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud + ", ");
                My_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Codigo1 +", ");
                My_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Codigo2 + ", ");
                My_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Importe + ", ");
                My_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Justificacion+ ", ");
                My_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Estatus + ", ");
                My_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Usuario_Creo + ", ");
                My_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Fecha_Creo + ") ");
                My_SQL.Append("VALUES (" + Datos.P_No_Solicitud + ", ");
                My_SQL.Append("'" + Datos.P_Codigo_Programatico_De + "', ");
                My_SQL.Append("'" + Datos.P_Codigo_Programatico_Al + "', ");
                My_SQL.Append(Datos.P_Monto + ", ");
                My_SQL.Append("'" + Datos.P_Justificacion + "', ");
                My_SQL.Append("'" + Datos.P_Estatus + "',");
                My_SQL.Append("'" + Datos.P_Usuario_Creo + "',");
                My_SQL.Append("SYSDATE)");

                //Ejecutar consulta
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, My_SQL.ToString());

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
        /// NOMBRE: Modificar_Movimiento
        /// 
        /// COMENTARIOS: Esta operación actualiza un registro del movimiento en la tabla de 
        /// 
        /// PARÁMETROS: Datos.- Valor de los campos a insertar en la tabla de 
        /// 
        /// USUARIO CREÓ: Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ:  18/Octubre/2011 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static Boolean Modificar_Movimiento(Cls_Ope_Pre_Movimiento_Presupuestal_Negocio Datos)
        {
            StringBuilder My_SQL = new StringBuilder();//Variable que almacenara la consulta.
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
                My_SQL.Append("UPDATE " + Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf + " SET ");
                My_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Codigo1 + "='" + Datos.P_Codigo_Programatico_De +"',");
                My_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Codigo2 + "='" + Datos.P_Codigo_Programatico_Al + "', ");
                My_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Importe + "="  + Datos.P_Monto + ", ");
                My_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Justificacion  + "='" + Datos.P_Justificacion + "', ");
                My_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Estatus + "='" + Datos.P_Estatus + "', ");
                My_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Usuario_Creo   + "='" + Datos.P_Usuario_Creo + "', ");
                My_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Fecha_Creo + "=SYSDATE ");
                My_SQL.Append("WHERE "  + Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud +"=" + Datos.P_No_Solicitud);

                //Ejecutar consulta
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, My_SQL.ToString());

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
        /// NOMBRE: Eliminar_Movimiento
        /// 
        /// COMENTARIOS: Esta operación eliminara un registro del movimiento que se haya realizado en la tabla de 
        /// 
        /// PARÁMETROS: Datos.- Valor de los campos a insertar en la tabla de 
        /// 
        /// USUARIO CREÓ: Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ:  14/Octubre/2011 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static Boolean Eliminar_Movimiento(Cls_Ope_Pre_Movimiento_Presupuestal_Negocio Datos)
        {
            StringBuilder My_SQL = new StringBuilder();//Variable que almacenara la consulta.
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
                

                My_SQL.Append("Delete From " + Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf + " ");
                My_SQL.Append("where " + Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud + "=" + Datos.P_No_Solicitud);
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, My_SQL.ToString());
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
        /// NOMBRE: Consultar_Movimiento
        /// 
        /// COMENTARIOS: Consulta el movimiento presupuestal que se haya llevado  
        /// 
        /// PARÁMETROS: Datos.- Valor de los campos a insertar en la tabla de 
        /// 
        /// USUARIO CREÓ: Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ:  14/Octubre/2011 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Movimiento(Cls_Ope_Pre_Movimiento_Presupuestal_Negocio Datos)
        {
            StringBuilder My_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Movimiento = new DataTable();
            try
            {
                My_SQL.Append("Select " + Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf + ".* ");
                My_SQL.Append("From " + Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf);

                if (!string.IsNullOrEmpty(Datos.P_No_Solicitud))
                {
                    if (My_SQL.ToString().Contains("WHERE"))
                    {
                        //no llevan comilla simple es entero el numero de solicitud
                        My_SQL.Append(" OR " + Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud + "=" + Datos.P_No_Solicitud + "");
                    }
                     else
                    {
                        My_SQL.Append(" WHERE " + Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud + "=" + Datos.P_No_Solicitud + "");
                    }
                }

                


                if (!string.IsNullOrEmpty(Datos.P_Estatus))
                {
                    if (My_SQL.ToString().Contains("WHERE"))
                    {
                        My_SQL.Append(" OR " + Cat_Ope_Com_Solicitud_Transf.Campo_Estatus + "='" + Datos.P_Estatus + "'");
                    }
                    else
                    {
                        My_SQL.Append(" WHERE " + Cat_Ope_Com_Solicitud_Transf.Campo_Estatus + "='" + Datos.P_Estatus + "'");
                    }
                }



                Dt_Movimiento = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, My_SQL.ToString()).Tables[0];
                return Dt_Movimiento;
            }
        
            
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los bancos que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
            return Dt_Movimiento;
        }

        #endregion


    }
}