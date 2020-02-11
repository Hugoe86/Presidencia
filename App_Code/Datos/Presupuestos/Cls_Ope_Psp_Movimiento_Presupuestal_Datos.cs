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
    public class Cls_Ope_Psp_Movimiento_Presupuestal_Datos
    {
        #region(Metodos)
        /// ********************************************************************************************************************
        /// NOMBRE: Alta_Movimiento
        /// 
        /// COMENTARIOS: Esta operación inserta un nuevo registro de un movimiento presupuestal en la tabla de OPE_COM_SOLICITUD_TRANSF
        /// 
        /// PARÁMETROS: Datos.- Valor de los campos a insertar en la tabla de OPE_COM_SOLICITUD_TRANSF 
        /// 
        /// USUARIO CREÓ: Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ: 18/Octubre/2011 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static Boolean Alta_Movimiento(Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Datos)
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
                Mi_SQL.Append("SELECT NVL(MAX (" + Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud + "),0) ");
                Mi_SQL.Append("FROM " + Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf );
                No_Solicitud_Max = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                if (Convert.IsDBNull(No_Solicitud_Max) == false ) 
                {
                    
                    Datos.P_No_Solicitud = String.Format("{0:00000}", Convert.ToInt32(No_Solicitud_Max) + 1);
                }
                else 
                {

                    Datos.P_No_Solicitud = "00001";
                }
                Mi_SQL = new StringBuilder();
                //Da de Alta los datos del Nuevo Parametro con los datos proporcionados por el usuario.
                Mi_SQL.Append("INSERT INTO " + Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf  + " (");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud + ", ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Codigo1 +", ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Codigo2 + ", ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Importe + ", ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Justificacion+ ", ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Estatus + ", ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Usuario_Creo + ", ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Fecha_Creo + ", ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Tipo_Operacion +", ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Origen_Area_Funcional_Id + ", ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Origen_Programa_Id + ", ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Origen_Partida_Id + ", ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Origen_Dependencia_Id + ", ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Destino_Area_Funcional_Id + ", ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Destino_Programa_Id + ", ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Destino_Partida_Id + ", ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Destino_Dependencia_Id + ", ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Origen_Fuente_Financiamiento_Id + ", ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Destino_Fuente_Financiamiento_Id + ") ");
                Mi_SQL.Append(" VALUES (" + Datos.P_No_Solicitud + ", ");
                Mi_SQL.Append("'" + Datos.P_Codigo_Programatico_De + "', ");
                Mi_SQL.Append("'" + Datos.P_Codigo_Programatico_Al + "', ");
                Mi_SQL.Append(Datos.P_Monto + ", ");
                Mi_SQL.Append("'" + Datos.P_Justificacion + "', ");
                Mi_SQL.Append("'" + Datos.P_Estatus + "',");
                Mi_SQL.Append("'" + Datos.P_Usuario_Creo + "',");
                Mi_SQL.Append("SYSDATE, ");
                Mi_SQL.Append("'" + Datos.P_Tipo_Operacion +"',");
                Mi_SQL.Append("'" + Datos.P_Origen_Area_Funcional_Id + "',");
                Mi_SQL.Append("'" + Datos.P_Origen_Programa_Id + "',");
                Mi_SQL.Append("'" + Datos.P_Origen_Partida_Id + "',");
                Mi_SQL.Append("'" + Datos.P_Origen_Dependencia_Id + "',");
                Mi_SQL.Append("'" + Datos.P_Destino_Area_Funcional_Id + "',");
                Mi_SQL.Append("'" + Datos.P_Destino_Programa_Id + "',");
                Mi_SQL.Append("'" + Datos.P_Destino_Partida_Id + "',");
                Mi_SQL.Append("'" + Datos.P_Destino_Dependencia_Id + "', ");
                Mi_SQL.Append("'" + Datos.P_Origen_Fuente_Financiamiento_Id + "',"); 
                Mi_SQL.Append("'" + Datos.P_Destino_Fuente_Financiamiento_Id + "')");
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
        /// NOMBRE: Alta_Comentario
        /// 
        /// COMENTARIOS: Esta operación inserta un nuevo registro de un comentario presupuestal en la tabla de Ope_Psp_Cierre_Presup
        /// 
        /// PARÁMETROS: Datos.- Valor de los campos a insertar en la tabla de 
        /// 
        /// USUARIO CREÓ: Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ: 17/Noviembre/2011 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static Boolean Alta_Comentario(Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Datos)
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
                Mi_SQL.Append("INSERT INTO " + Ope_Psp_Comentarios_Mov.Tabla_Ope_Psp_Comentarios_Mov + " (");
                Mi_SQL.Append(Ope_Psp_Comentarios_Mov.Campo_Numero_Solicitud + ", ");
                Mi_SQL.Append(Ope_Psp_Comentarios_Mov.Campo_Comentario + ", ");
                Mi_SQL.Append(Ope_Psp_Comentarios_Mov.Campo_Fecha + ", ");
                Mi_SQL.Append(Ope_Psp_Comentarios_Mov.Campo_Usuario_Creo + ", ");
                Mi_SQL.Append(Ope_Psp_Comentarios_Mov.Campo_Fecha_Creo + ") ");
                Mi_SQL.Append("VALUES (" + Datos.P_No_Solicitud + ", ");
                Mi_SQL.Append("'" + Datos.P_Comentario + "', ");
                Mi_SQL.Append("SYSDATE , ");
                Mi_SQL.Append("'" + Datos.P_Usuario_Creo + "', ");
                Mi_SQL.Append("SYSDATE )");
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
        /// NOMBRE: Modificar_Movimiento
        /// 
        /// COMENTARIOS: Esta operación actualiza un registro del movimiento en la tabla de OPE_COM_SOLICITUD_TRANSF
        /// 
        /// PARÁMETROS: Datos.- Valor de los campos a Modificar en la tabla de OPE_COM_SOLICITUD_TRANSF 
        /// 
        /// USUARIO CREÓ: Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ:  18/Octubre/2011 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static Boolean Modificar_Movimiento(Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Datos)
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
                Mi_SQL.Append("UPDATE " + Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf + " SET ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Codigo1 + "='" + Datos.P_Codigo_Programatico_De + "',");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Codigo2 + "='" + Datos.P_Codigo_Programatico_Al + "', ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Importe + "=" + Datos.P_Monto + ", ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Justificacion + "='" + Datos.P_Justificacion + "', ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Estatus + "='" + Datos.P_Estatus + "', ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Tipo_Operacion + "='" + Datos.P_Tipo_Operacion + "', ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Creo + "', ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Origen_Area_Funcional_Id + "='" + Datos.P_Origen_Area_Funcional_Id + "', ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Origen_Dependencia_Id + "='" + Datos.P_Origen_Dependencia_Id + "', ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Origen_Fuente_Financiamiento_Id + "='" + Datos.P_Origen_Fuente_Financiamiento_Id + "', ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Origen_Partida_Id + "='" + Datos.P_Origen_Partida_Id  + "', ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Origen_Programa_Id + "='" + Datos.P_Origen_Programa_Id + "', ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Destino_Area_Funcional_Id + "='" + Datos.P_Destino_Area_Funcional_Id + "', ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Destino_Dependencia_Id  + "='" + Datos.P_Destino_Dependencia_Id + "', ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Destino_Fuente_Financiamiento_Id + "='" + Datos.P_Destino_Fuente_Financiamiento_Id + "', ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Destino_Partida_Id + "='" + Datos.P_Destino_Partida_Id + "', ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Destino_Programa_Id + "='" + Datos.P_Destino_Programa_Id + "', ");
                Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Fecha_Modifico + "=SYSDATE ");
                Mi_SQL.Append("WHERE " + Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud + "=" + Datos.P_No_Solicitud);

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
        /// NOMBRE: Eliminar_Movimiento
        /// 
        /// COMENTARIOS: Esta operación eliminara un registro del movimiento que se haya realizado en la tabla de OPE_COM_SOLICITUD_TRANSF
        /// 
        /// PARÁMETROS: Datos.- Valor de los campos a eliminar en la tabla de OPE_COM_SOLICITUD_TRANSF
        /// 
        /// USUARIO CREÓ: Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ:  14/Octubre/2011 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static Boolean Eliminar_Movimiento(Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Datos)
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
                

                Mi_SQL.Append("Delete From " + Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf + " ");
                Mi_SQL.Append("where " + Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud + "=" + Datos.P_No_Solicitud);
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
        /// NOMBRE: Consultar_Movimiento
        /// 
        /// COMENTARIOS: Consulta el movimiento presupuestal que se haya llevado en la tabla OPE_COM_SOLICITUD_TRANSF  
        /// 
        /// PARÁMETROS: Datos.- Valor de los campos a consultar en la tabla de OPE_COM_SOLICITUD_TRANSF
        /// 
        /// USUARIO CREÓ: Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ:  14/Octubre/2011 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Movimiento(Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Movimiento = new DataTable();
            try
            {
                Mi_SQL.Append("Select " + Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf + ".* ");
                Mi_SQL.Append("From " + Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf);

                if (!string.IsNullOrEmpty(Datos.P_No_Solicitud))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        //no llevan comilla simple es entero el numero de solicitud
                        Mi_SQL.Append(" OR " + Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud + "=" + Datos.P_No_Solicitud + "");
                    }
                     else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud + "=" + Datos.P_No_Solicitud + "");
                    }
                }
                if (!string.IsNullOrEmpty(Datos.P_Monto))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" OR " + Cat_Ope_Com_Solicitud_Transf.Campo_Importe + "=" + Datos.P_Monto + "");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Ope_Com_Solicitud_Transf.Campo_Importe + "=" + Datos.P_Monto + "");
                    }
                }
                if (!string.IsNullOrEmpty(Datos.P_Estatus))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" OR " + Cat_Ope_Com_Solicitud_Transf.Campo_Estatus + "='" + Datos.P_Estatus + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Ope_Com_Solicitud_Transf.Campo_Estatus + "='" + Datos.P_Estatus + "'");
                    }
                }
                Mi_SQL.Append(" Order by " + Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud + " asc");
                Dt_Movimiento = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
               
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Movimientos Presupuestales que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
            return Dt_Movimiento;
        }

        /// ********************************************************************************************************************
        /// NOMBRE: Consulta_Movimiento_Fecha
        /// 
        /// COMENTARIOS: Consulta el movimiento presupuestal que se haya llevado en la tabla OPE_COM_SOLICITUD_TRANSF por fecha  
        /// 
        /// PARÁMETROS: Datos.- Valor de los campos a consultar en la tabla de OPE_COM_SOLICITUD_TRANSF
        /// 
        /// USUARIO CREÓ: Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ:  18/Noviembre/2011 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consulta_Movimiento_Fecha(Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Movimiento = new DataTable();
            try
            {
                Mi_SQL.Append("Select " + Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf + ".* ");
                Mi_SQL.Append("From " + Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf);


                if ((!string.IsNullOrEmpty(Datos.P_Fecha_Inicio)) && (string.IsNullOrEmpty(Datos.P_Fecha_Final)))
                {
                    Mi_SQL.Append(" WHERE " + Cat_Ope_Com_Solicitud_Transf.Campo_Fecha_Creo + "='" + Datos.P_Fecha_Inicio + "' ");
                }
                else
                {
                    if ((!string.IsNullOrEmpty(Datos.P_Fecha_Inicio)))
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Ope_Com_Solicitud_Transf.Campo_Fecha_Creo + ">='" + Datos.P_Fecha_Inicio + "' ");
                    }

                    if (!string.IsNullOrEmpty(Datos.P_Fecha_Final))
                    {
                        Mi_SQL.Append("AND " + Cat_Ope_Com_Solicitud_Transf.Campo_Fecha_Creo + "<='" + Datos.P_Fecha_Final + "' ");
                    }
                }
                Mi_SQL.Append("AND " + Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf + "." + Cat_Ope_Com_Solicitud_Transf.Campo_Codigo1);
                Mi_SQL.Append(" like '%" + Datos.P_Responsable + "%'");
                Mi_SQL.Append(" AND " + Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf + "." + Cat_Ope_Com_Solicitud_Transf.Campo_Codigo2);
                Mi_SQL.Append(" like '%" + Datos.P_Responsable + "%'");
                
                Mi_SQL.Append(" Order by " + Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud + " asc");
                Dt_Movimiento = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
               
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Movimientos Presupuestales que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
            return Dt_Movimiento;
        }
        /// ********************************************************************************************************************
        /// NOMBRE: Consulta_Movimiento_Btn_Busqueda
        /// 
        /// COMENTARIOS: Consulta el movimiento presupuestal que se haya llevado en la tabla OPE_COM_SOLICITUD_TRANSF por fecha  
        /// 
        /// PARÁMETROS: Datos.- Valor de los campos a consultar en la tabla de OPE_COM_SOLICITUD_TRANSF
        /// 
        /// USUARIO CREÓ: Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ:  18/Noviembre/2011 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consulta_Movimiento_Btn_Busqueda(Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Movimiento = new DataTable();
            try
            {
                Mi_SQL.Append("Select " + Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf + ".* ");
                Mi_SQL.Append("From " + Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf);


                if (!string.IsNullOrEmpty(Datos.P_No_Solicitud))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        //no llevan comilla simple es entero el numero de solicitud
                        Mi_SQL.Append(" OR " + Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud + "=" + Datos.P_No_Solicitud + " ");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud + "=" + Datos.P_No_Solicitud + " ");
                    }
                }
                
                Mi_SQL.Append(" AND " + Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf + "." + Cat_Ope_Com_Solicitud_Transf.Campo_Codigo1);
                Mi_SQL.Append(" like '%" + Datos.P_Responsable + "%'");
                Mi_SQL.Append(" AND " + Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf + "." + Cat_Ope_Com_Solicitud_Transf.Campo_Codigo2);
                Mi_SQL.Append(" like '%" + Datos.P_Responsable + "%'");

                Mi_SQL.Append(" Order by " + Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud + " asc");
                Dt_Movimiento = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Movimientos Presupuestales que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
            return Dt_Movimiento;
        }
        /// ********************************************************************************************************************
        /// NOMBRE: Consultar_programa
        /// 
        /// COMENTARIOS: Consulta el programa al que pertenece de la tabla CAT_SAP_PROYECTOS_PROGRAMAS  
        /// 
        /// PARÁMETROS: Datos.- Valor de los campos a consultar en la tabla de CAT_SAP_PROYECTOS_PROGRAMAS 
        /// 
        /// USUARIO CREÓ: Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ:  14/Octubre/2011 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Programa(Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Datos)
        {
            DataTable Dt_Consulta_Programa = new DataTable();
            StringBuilder Mi_SQL;
            try
            {
                Mi_SQL = new StringBuilder();
                Mi_SQL.Append("Select *");
                Mi_SQL.Append(" from " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas);


                if (!string.IsNullOrEmpty(Datos.P_Programa))
                {
                    Mi_SQL.Append(" where " + Cat_Sap_Proyectos_Programas.Campo_Clave + "='" + Datos.P_Programa + "'");
                }
                Dt_Consulta_Programa = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error: [" + ex.Message + "]");
            }
            return Dt_Consulta_Programa;
        }

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Area_Funcional
        /// DESCRIPCION : Consulta la clave en la  CAT_SAP_AREA_FUNCIONAL  
        /// PARAMETROS  :  Datos.- Valor de los campos a insertar en la tabla de CAT_SAP_AREA_FUNCIONAL
        /// CREO        : Hugo Enrique Ramirez Aguilera 
        /// FECHA_CREO  : 14-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Area_Funcional(Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Datos)
        {
            StringBuilder Mi_SQL;
            try
            {
                Mi_SQL = new StringBuilder();
                Mi_SQL.Append("Select *");
                Mi_SQL.Append(" From " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional);
                if (!string.IsNullOrEmpty(Datos.P_Area_Funcional))
                {
                    Mi_SQL.Append(" where " + Cat_SAP_Area_Funcional.Campo_Clave + "='" + Datos.P_Area_Funcional + "'");
                }
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Like_Movimiento
        /// DESCRIPCION : Consulta la clave que contenga algun valor paresido en el registro en la  CAT_SAP_AREA_FUNCIONAL  
        /// PARAMETROS  :  Datos.- Valor de los campos a insertar en la tabla de OPE_COM_SOLICITUD_TRANSF
        /// CREO        : Hugo Enrique Ramirez Aguilera 
        /// FECHA_CREO  : 29-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Like_Movimiento(Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            try
            {
               
                Mi_SQL.Append("Select * ");
                Mi_SQL.Append("From " + Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf +" ");
                Mi_SQL.Append("Where " + Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf + "." + Cat_Ope_Com_Solicitud_Transf.Campo_Codigo1);
                Mi_SQL.Append(" like '%" + Datos.P_Responsable + "%'");
                Mi_SQL.Append(" Or " +Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf +"." +Cat_Ope_Com_Solicitud_Transf.Campo_Codigo2);
                Mi_SQL.Append(" like '%" +Datos.P_Responsable +"%'");
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

            }
            catch (Exception Ex)
            {
                throw new Exception("Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Dependencia_Ordenada
        /// DESCRIPCION : Consulta la clave que contenga algun valor paresido en el registro en la  CAT_SAP_AREA_FUNCIONAL  
        /// PARAMETROS  :  Datos.- Valor de los campos a insertar en la tabla de CAT_DEPENDENCIA    
        /// CREO        : Hugo Enrique Ramirez Aguilera 
        /// FECHA_CREO  : 29-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Dependencia_Ordenada(Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Datos)
        {
            StringBuilder Mi_SQL= new StringBuilder();
            try
            {
                Mi_SQL.Append("Select distinct " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " ");
                Mi_SQL.Append("from " + Cat_Dependencias.Tabla_Cat_Dependencias + " ");
                Mi_SQL.Append("Order by " + Cat_Dependencias.Campo_Clave + " ASC");
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

            }
            catch (Exception Ex)
            {
                throw new Exception("Error: [" + Ex.Message + "]");
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Datos_Partidas
        /// DESCRIPCION : Consulta la clave que contenga algun valor paresido en el registro en la  CAT_SAP_AREA_FUNCIONAL  
        /// PARAMETROS  :  Datos.- Valor de los campos a insertar en la tabla de CAT_DEPENDENCIA    
        /// CREO        : Hugo Enrique Ramirez Aguilera 
        /// FECHA_CREO  : 29-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
         public static DataTable Consulta_Datos_Partidas(Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta SQL
            String Filtro_SQL = ""; //Variable para el filtro de la consulta SQL

            try
            {
                Mi_SQL.Append("SELECT * ");
                Mi_SQL.Append(" FROM " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas);

                if (!string.IsNullOrEmpty(Datos.P_Partida))
                {
                    Mi_SQL.Append(" where " + Cat_Com_Partidas.Campo_Clave + " like '" + Datos.P_Partida + "%'");
                }

                Mi_SQL.Append("Order by " + Cat_Com_Partidas.Campo_Clave + " ASC");
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

            }
            catch (Exception Ex)
            {
                throw new Exception("Error: [" + Ex.Message + "]");
            }
        }
         ///*******************************************************************************
         /// NOMBRE DE LA FUNCION: Consulta_Datos_Comentarios
         /// DESCRIPCION : Consulta la clave que contenga algun valor paresido en el registro en la 
         ///                tabla Ope_Psp_Comentarios_Mov
         /// PARAMETROS  :  Datos.- Valor de los campos a insertar en la tabla de CAT_DEPENDENCIA    
         /// CREO        : Hugo Enrique Ramirez Aguilera 
         /// FECHA_CREO  : 17-noviembre-2011
         /// MODIFICO          :
         /// FECHA_MODIFICO    :
         /// CAUSA_MODIFICACION:
         ///*******************************************************************************
         public static DataTable Consulta_Datos_Comentarios(Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Datos)
         {
             StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta SQL

             try
             {
                 Mi_SQL.Append("SELECT * ");
                 Mi_SQL.Append(" FROM " + Ope_Psp_Comentarios_Mov.Tabla_Ope_Psp_Comentarios_Mov);

                 if (!string.IsNullOrEmpty(Datos.P_No_Solicitud))
                 {
                     Mi_SQL.Append(" where " + Ope_Psp_Comentarios_Mov.Campo_Numero_Solicitud + "=" + Datos.P_No_Solicitud);
                 }
                  Mi_SQL.Append(" Order by " + Ope_Psp_Comentarios_Mov.Campo_Fecha +" DESC");
                 
                 return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

             }
             catch (Exception Ex)
             {
                 throw new Exception("Error: [" + Ex.Message + "]");
             }
         }
        #endregion


    }
}