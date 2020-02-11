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
using Presidencia.Autorizar_Traspaso_Presupuestal.Negocio;


namespace Presidencia.Movimiento_Presupuestal.Datos
{
    public class Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Datos
    {
        #region(Metodods)
            /// ********************************************************************************************************************
            /// NOMBRE: Alta_Autorizacion_Traspaso
            /// 
            /// COMENTARIOS: Esta operación inserta un nuevo registro de un movimiento presupuestal en la tabla de 
            /// 
            /// PARÁMETROS: Datos.- Valor de los campos a insertar en la tabla de 
            /// 
            /// USUARIO CREÓ: Hugo Enrique Ramirez Aguilera
            /// FECHA CREÓ: 22/Octubre/2011 
            /// USUARIO MODIFICO:
            /// FECHA MODIFICO:
            /// CAUSA DE LA MODIFICACIÓN:
            /// ********************************************************************************************************************
            public static Boolean Alta_Autorizacion_Traspaso(Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Negocio Datos)
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
                    Mi_SQL.Append("FROM " + Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf);
                    //realizar consulta
                    No_Solicitud_Max = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                    
                    
                    if (Convert.IsDBNull(No_Solicitud_Max) == false)
                    {

                        Datos.P_Numero_Solicitud = String.Format("{0:00000}", Convert.ToInt32(No_Solicitud_Max) + 1);
                    }
                    else
                    {
                        //si no tiene registro se le asigna 1
                        Datos.P_Numero_Solicitud = "00001";
                    }
                    Mi_SQL = new StringBuilder();

                    //cadena para insertar el la base de datos
                    Mi_SQL.Append("INSERT INTO " + Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf + " (");
                    Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud + ", ");
                    Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Codigo1 + ", ");
                    Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Codigo2 + ", ");
                    Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Importe + ", ");
                    Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Justificacion + ", ");
                    Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Estatus + ", ");
                    Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Usuario_Creo + ", ");
                    Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Fecha_Creo + ") ");
                    Mi_SQL.Append("VALUES (" + Datos.P_Numero_Solicitud + ", ");
                    Mi_SQL.Append("'" + Datos.P_Codigo_Programatico_Origen + "', ");
                    Mi_SQL.Append("'" + Datos.P_Codigo_Programatico_Destino + "', ");
                    Mi_SQL.Append(Datos.P_Importe + ", ");
                    Mi_SQL.Append("'" + Datos.P_Justificacion + "', ");
                    Mi_SQL.Append("'" + Datos.P_Estatus + "',");
                    Mi_SQL.Append("'" + Datos.P_Usuario_Creo + "',");
                    Mi_SQL.Append("SYSDATE)");

                    //Ejecutar consulta
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                    Conexion.Close();
                    Operacion_Completa = true;

                    //Ejecutar consulta
                    //OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

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
            /// NOMBRE: Modificar_Comentario
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
            public static Boolean Modificar_Comentario(Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Negocio Datos)
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
                    Mi_SQL.Append("UPDATE " + Ope_Psp_Comentarios_Mov.Tabla_Ope_Psp_Comentarios_Mov  +" SET ");
                    Mi_SQL.Append(Ope_Psp_Comentarios_Mov.Campo_Comentario +"='" +Datos.P_Comentario + "', ");
                    Mi_SQL.Append(Ope_Psp_Comentarios_Mov.Campo_Fecha +"=SYSDATE" + ", ");
                    Mi_SQL.Append(Ope_Psp_Comentarios_Mov.Campo_Usuario_Modifico +"='" +Datos.P_Usuario_Creo  + "', ");
                    Mi_SQL.Append(Ope_Psp_Comentarios_Mov.Campo_Fecha_Modifico + "=SYSDATE" + " ");
                    Mi_SQL.Append("WHERE " + Ope_Psp_Comentarios_Mov.Campo_Numero_Solicitud + " = " + Datos.P_Numero_Solicitud);
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
            /// NOMBRE: Modificar_Autorizacion_Traspaso
            /// 
            /// COMENTARIOS: Esta operación actualiza un registro del movimiento en la tabla de 
            /// 
            /// PARÁMETROS: Datos.- Valor de los campos a Modificar en la tabla de  
            /// 
            /// USUARIO CREÓ: Hugo Enrique Ramirez Aguilera
            /// FECHA CREÓ:  22/Octubre/2011 
            /// USUARIO MODIFICO:
            /// FECHA MODIFICO:
            /// CAUSA DE LA MODIFICACIÓN:
            /// ********************************************************************************************************************
            public static Boolean Modificar_Autorizacion_Traspaso(Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Negocio Datos)
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
                    Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Codigo1 + "='" + Datos.P_Codigo_Programatico_Origen + "', ");
                    Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Codigo2 + "='" + Datos.P_Codigo_Programatico_Destino + "', ");
                    Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Importe + "=" + Datos.P_Importe+ ", ");
                    Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Justificacion + "='" + Datos.P_Justificacion + "', ");
                    Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Estatus + "='" + Datos.P_Estatus + "', ");
                    Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Creo + "', ");
                    Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Tipo_Operacion + "='" + Datos.P_Tipo_Operacion + "', ");
                    Mi_SQL.Append(Cat_Ope_Com_Solicitud_Transf.Campo_Fecha_Modifico + "=SYSDATE ");
                    Mi_SQL.Append("WHERE " + Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud+ " = " + Datos.P_Numero_Solicitud);

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
            /// NOMBRE: Eliminar_Autorizacion_Traspaso
            /// 
            /// COMENTARIOS: Esta operación eliminara un registro del movimiento que se haya realizado en la tabla de 
            /// 
            /// PARÁMETROS: Datos.- Valor de los campos a insertar en la tabla de 
            /// 
            /// USUARIO CREÓ: Hugo Enrique Ramirez Aguilera
            /// FECHA CREÓ:  22/Octubre/2011 
            /// USUARIO MODIFICO:
            /// FECHA MODIFICO:
            /// CAUSA DE LA MODIFICACIÓN:
            /// ********************************************************************************************************************
            public static Boolean Eliminar_Autorizacion_Traspaso(Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Negocio Datos)
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
                    Mi_SQL.Append("where " + Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud+ "=" + Datos.P_Numero_Solicitud);
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
            /// NOMBRE: Consulta_Autorizacion_Traspaso
            /// 
            /// COMENTARIOS: Consulta el movimiento presupuestal que se haya llevado en la tabla   
            /// 
            /// PARÁMETROS: Datos.- Valor de los campos a insertar en la tabla de 
            /// 
            /// USUARIO CREÓ: Hugo Enrique Ramirez Aguilera
            /// FECHA CREÓ:  24/Octubre/2011 
            /// USUARIO MODIFICO:
            /// FECHA MODIFICO:
            /// CAUSA DE LA MODIFICACIÓN:
            /// ********************************************************************************************************************
            public static DataTable Consulta_Autorizacion_Traspaso(Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
                DataTable Dt_Movimiento = new DataTable();
                try
                {
                    Mi_SQL.Append("Select " +Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf +".* ");
                    Mi_SQL.Append("From " + Cat_Ope_Com_Solicitud_Transf.Tabla_Cat_Ope_Com_Solicitud_Transf + " ");

                    if (!string.IsNullOrEmpty(Datos.P_Numero_Solicitud))
                    {
                        if (Mi_SQL.ToString().Contains("WHERE"))
                        {
                            //no llevan comilla simple es entero el numero de solicitud
                            Mi_SQL.Append(" OR " + Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud + "=" + Datos.P_Numero_Solicitud + "");
                        }
                        else
                        {
                            Mi_SQL.Append(" WHERE " + Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud + "=" + Datos.P_Numero_Solicitud + "");
                        }
                    }
                    if (!string.IsNullOrEmpty(Datos.P_Importe))
                    {
                        if (Mi_SQL.ToString().Contains("WHERE"))
                        {
                            //no llevan comilla simple es entero el numero de solicitud
                            Mi_SQL.Append(" OR " + Cat_Ope_Com_Solicitud_Transf.Campo_Importe + "=" + Datos.P_Importe + "");
                        }
                        else
                        {
                            Mi_SQL.Append(" WHERE " + Cat_Ope_Com_Solicitud_Transf.Campo_Importe + "=" + Datos.P_Importe + "");
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
                   Mi_SQL.Append( " Order by " +Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud  +" asc");
                Dt_Movimiento = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
               
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al consultar los Movimientos Presupuestales que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
                }
                return Dt_Movimiento;
            }
            /// ********************************************************************************************************************
            /// NOMBRE: Consulta_Datos_Partidas
            /// 
            /// COMENTARIOS: Consulta  la partida especifica
            /// 
            /// PARÁMETROS: Datos.- Valor de los campos a insertar en la tabla de 
            /// 
            /// USUARIO CREÓ: Hugo Enrique Ramirez Aguilera
            /// FECHA CREÓ:  17/noviembre/2011 
            /// USUARIO MODIFICO:
            /// FECHA MODIFICO:
            /// CAUSA DE LA MODIFICACIÓN:
            /// ********************************************************************************************************************
            public static DataTable Consulta_Datos_Partidas(Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
                DataTable Dt_Movimiento = new DataTable();
                try
                {
                    Mi_SQL.Append("SELECT * ");
                    Mi_SQL.Append("From " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas +" ");
                    Mi_SQL.Append("where " + Cat_Com_Partidas.Campo_Clave +"='"  +Datos.P_Partida +"'");
                    Dt_Movimiento = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
               
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al consultar los Movimientos Presupuestales que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
                }
                return Dt_Movimiento;
            }
        #endregion

       
    }
}