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
using System.Data.SqlClient;
using Presidencia.Constantes;
using System.Text;
using Presidencia.Recoleccion_Caja.Negocio;

namespace Presidencia.Recoleccion_Caja.Datos
{
    public class Cls_Ope_Caj_Recolecciones_Datos
    {
	    public Cls_Ope_Caj_Recolecciones_Datos()
	    {
        }
        #region (Consulta)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Caja_Empleado
            /// DESCRIPCION : Consulta la caja que tiene abierta el empleado para poder realizar
            ///               la recolección de la misma
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 19-Agosto-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Caja_Empleado(Cls_Ope_Caj_Recolecciones_Negocio Datos)
            {
                String Mi_SQL; //Obtiene la consulta a realizar a la base de datos
                try
                {
                    //Consulta los datos generales de la caja que tiene abierta el empleado que requiere realizar la recolección del dinero
                    Mi_SQL = "SELECT " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS Caja, " +
                             Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_Id +
                             " FROM " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + ", " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos +
                             " WHERE " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id + " = " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_Id +
                             " AND " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Estatus + " = 'ABIERTO'" +
                             " AND " + Ope_Caj_Turnos.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'";
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                }
                catch (OracleException Ex)
                {
                    throw new Exception("Error: " + Ex.Message);
                }

                catch (DBConcurrencyException Ex)
                {
                    throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
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
            /// NOMBRE DE LA FUNCION: Consulta_Detalles_Recolecciones
            /// DESCRIPCION : Consulta el detalle de la recoleccion que fue seleccionada por
            ///               el usuario
            /// PARAMETROS  : Datos: contiene el No de Recoleccion a consultar
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 13-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Detalles_Recolecciones(Cls_Ope_Caj_Recolecciones_Negocio Datos)
            {
                StringBuilder Mi_SQL= new StringBuilder(); //Obtiene la consulta a realizar a la base de datos
                try
                {
                    //Consulta los detalles de la recoleccion que fue seleccionada por el usuario
                    Mi_SQL.Append("SELECT * FROM " + Ope_Caj_Recolecciones_Detalles.Tabla_Ope_Caj_Recolecciones_Detalles);
                    Mi_SQL.Append(" WHERE " + Ope_Caj_Recolecciones_Detalles.Campo_No_Recoleccion + " = '" + Datos.P_No_Recoleccion + "'");
                    Mi_SQL.Append(" AND " + Ope_Caj_Recolecciones_Detalles.Campo_No_Turno + " = '" + Datos.P_No_Turno + "'");
                    Mi_SQL.Append(" AND " + Ope_Caj_Recolecciones_Detalles.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'");
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                }
                catch (OracleException Ex)
                {
                    throw new Exception("Error: " + Ex.Message);
                }

                catch (DBConcurrencyException Ex)
                {
                    throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
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
            /// NOMBRE DE LA FUNCION: Consulta_Recolecciones
            /// DESCRIPCION : Consulta las recolecciones que ha realizado el cajero de acuerdo
            ///               a la fecha proporcionada
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 19-Agosto-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Recolecciones(Cls_Ope_Caj_Recolecciones_Negocio Datos)
            {
                String Mi_SQL; //Obtiene la consulta a la base de datos
                try
                { 
                    //Consulta las recolecciones que fueron realizadas por el cajero en la fecha que proporciono
                    Mi_SQL = "SELECT " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS Caja," +
                             " (" + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Clave + "||' '||" + 
                             Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Descripcion + ") AS Modulo, (" +
                             Ope_Caj_Recolecciones.Tabla_Ope_Caj_Recolecciones + "." + Ope_Caj_Recolecciones.Campo_Fecha_Creo + ") AS Fecha, " +
                             Ope_Caj_Recolecciones.Tabla_Ope_Caj_Recolecciones + "." + Ope_Caj_Recolecciones.Campo_Monto_Recolectado + ", " +
                             Ope_Caj_Recolecciones.Tabla_Ope_Caj_Recolecciones + "." + Ope_Caj_Recolecciones.Campo_Monto_Tarjeta + ", " +
                             Ope_Caj_Recolecciones.Tabla_Ope_Caj_Recolecciones + "." + Ope_Caj_Recolecciones.Campo_Monto_Cheque + ", " +
                             Ope_Caj_Recolecciones.Tabla_Ope_Caj_Recolecciones + "." + Ope_Caj_Recolecciones.Campo_Monto_Transferencia + ", " +
                             Ope_Caj_Recolecciones.Tabla_Ope_Caj_Recolecciones + "." + Ope_Caj_Recolecciones.Campo_Conteo_Tarjeta + ", " +
                             Ope_Caj_Recolecciones.Tabla_Ope_Caj_Recolecciones + "." + Ope_Caj_Recolecciones.Campo_Conteo_Cheque + ", " +
                             Ope_Caj_Recolecciones.Tabla_Ope_Caj_Recolecciones + "." + Ope_Caj_Recolecciones.Campo_Conteo_Transferencia + ", " +
                             Ope_Caj_Recolecciones.Tabla_Ope_Caj_Recolecciones + "." + Ope_Caj_Recolecciones.Campo_Num_Recoleccion + ", " +
                             Ope_Caj_Recolecciones.Tabla_Ope_Caj_Recolecciones + "." + Ope_Caj_Recolecciones.Campo_No_Recoleccion + ", " +
                             Ope_Caj_Recolecciones.Tabla_Ope_Caj_Recolecciones + "." + Ope_Caj_Recolecciones.Campo_Recibe_Efectivo +
                             " FROM " + Ope_Caj_Recolecciones.Tabla_Ope_Caj_Recolecciones + ", " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + ", " + 
                             Cat_Pre_Cajas.Tabla_Cat_Pre_Caja +
                             " WHERE " + Ope_Caj_Recolecciones.Tabla_Ope_Caj_Recolecciones + "." + Ope_Caj_Recolecciones.Campo_Caja_ID + 
                             " = " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id +
                             " AND " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Modulo_Id + 
                             " = " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Modulo_Id +
                             " AND " + Ope_Caj_Recolecciones.Tabla_Ope_Caj_Recolecciones + "." + Ope_Caj_Recolecciones.Campo_No_Turno + " = '" + Datos.P_No_Turno + "'" +
                             " AND " + Ope_Caj_Recolecciones.Tabla_Ope_Caj_Recolecciones + "." + Ope_Caj_Recolecciones.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'";

                    if (String.IsNullOrEmpty(Datos.P_Fecha_Busqueda.ToString()))
                    {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Caj_Recolecciones.Campo_Fecha + " BETWEEN TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Busqueda) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                                          " AND TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Busqueda) + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Caj_Recolecciones.Campo_Fecha + " DESC";
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                }
                catch (OracleException Ex)
                {
                    throw new Exception("Error: " + Ex.Message);
                }

                catch (DBConcurrencyException Ex)
                {
                    throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
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
            ///NOMBRE DE LA FUNCIÓN: Consultar_Total_Recolectado
            ///DESCRIPCIÓN: Obiene la cantidad Total Recolectada segun la caja seleccionada
            ///PARAMETROS:1. Caja.  Instancia de la Clase de Negocio de Recolecciones con los datos 
            ///                       de la Caja que va a ser consultada.
            ///CREO: Jacqueline Ramírez Sierra
            ///FECHA_CREO: 21/Septiembre/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            public static DataSet Consultar_Total_Recolectado(Cls_Ope_Caj_Recolecciones_Negocio Turno)
            {
                DataSet dataset;
                DataTable Tabla = new DataTable();
                String Mi_SQL = "SELECT NVL(SUM(";
                Mi_SQL = Mi_SQL + Ope_Caj_Recolecciones.Campo_Monto_Recolectado + "),'0') AS TOTAL_RECOLECTADO ";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Recolecciones.Tabla_Ope_Caj_Recolecciones;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Recolecciones.Campo_No_Turno + " = '" + Turno.P_No_Turno + "'";
                try
                {
                    dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (dataset != null)
                    {
                        Tabla = dataset.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar el Total Recolectado. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return dataset;
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Cajeros
            ///DESCRIPCIÓN: Obtiene todos los tipos de colonias existentes en la base de datos
            ///PARAMETROS:
            ///             1. Caja.  Instancia de la Clase de Negocio de Recolecciones con los datos 
            ///                       de la Caja que va a ser consultada.
            ///CREO: José Alfredo García Pichardo.
            ///FECHA_CREO: 24/Julio/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            public static DataSet Consultar_Total_Cobrado(Cls_Ope_Caj_Recolecciones_Negocio Caja)
            {
                DataSet dataset;
                DataTable Tabla = new DataTable();
                String Mi_SQL = "SELECT NVL(SUM(";
                Mi_SQL = Mi_SQL + Ope_Caj_Pagos_Detalles.Campo_Monto + "),'0') AS TOTAL_COBRADO ";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                Mi_SQL = Mi_SQL + " P JOIN " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles;
                Mi_SQL = Mi_SQL + " PD ON PD." + Ope_Caj_Pagos_Detalles.Campo_No_Pago + " = P." + Ope_Caj_Pagos.Campo_No_Pago;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Pagos.Campo_Caja_ID + " = '" + Caja.P_Caja_ID + "'";
                try
                {
                    dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (dataset != null)
                    {
                        Tabla = dataset.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar el Total Recolectado. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return dataset;
            }
        #endregion
        #region Operacion
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Alta_Recoleccion
            /// DESCRIPCION : Da de alta la recolección realizada por el empleado de la caja
            ///               con los datos proporcionados, con sus denominaciones
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 18-Agosto-2011
            /// MODIFICO          : Yazmin A Delgado Gómez
            /// FECHA_MODIFICO    : 13-Octubre-2011
            /// CAUSA_MODIFICACION: Porque se agrego el desglose de billetes y monedas
            ///*******************************************************************************
            public static void Alta_Recoleccion(Cls_Ope_Caj_Recolecciones_Negocio Datos)
            {
                OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
                OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
                OracleTransaction Transaccion_SQL;
                StringBuilder Mi_SQL = new StringBuilder();  //Contiene el string de la consulta a la base de datos
                Object Consecutivo;    //Obtiene el número total de recolecciones realizadas
                Object No_Recoleccion; //Obtiene el número de recolecciones que tiene el usuario realizado

                //inicializa la conexion
                if (Conexion_Base.State != ConnectionState.Open)
                {
                    Conexion_Base.Open(); //Abre la conexión a la base de datos            
                }
                Transaccion_SQL = Conexion_Base.BeginTransaction();  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
                Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
                Comando_SQL.Transaction = Transaccion_SQL;      
                try
                {
                    //Consulta el último registro de recoleccion dado de alta en la base de datos
                    Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Caj_Recolecciones.Campo_No_Recoleccion + "),0)");
                    Mi_SQL.Append(" FROM " + Ope_Caj_Recolecciones.Tabla_Ope_Caj_Recolecciones);
                    Comando_SQL.CommandText = Mi_SQL.ToString();
                    No_Recoleccion = Comando_SQL.ExecuteOracleScalar().ToString();
                    if (Convert.IsDBNull(No_Recoleccion))
                    {
                        No_Recoleccion = "0000000001";
                    }
                    else
                    {
                        No_Recoleccion = String.Format("{0:0000000000}", Convert.ToInt32(No_Recoleccion) + 1);
                    }
                    Mi_SQL.Length = 0;
                    //Consulta el número de recolecciones que ha tenido el empleado durante el día
                    Mi_SQL.Append("SELECT NVL(COUNT(*),0) FROM " + Ope_Caj_Recolecciones.Tabla_Ope_Caj_Recolecciones);
                    Mi_SQL.Append(" WHERE " + Ope_Caj_Recolecciones.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'");
                    Mi_SQL.Append(" AND " + Ope_Caj_Recolecciones.Campo_Caja_ID + " = '" + Datos.P_Caja_ID + "'");
                    Mi_SQL.Append(" AND " + Ope_Caj_Recolecciones.Campo_Fecha + " BETWEEN TO_DATE ('" + String.Format("{0:dd-MM-yyy}", DateTime.Today) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                    Mi_SQL.Append(" AND TO_DATE ('" + String.Format("{0:dd-MM-yyy}", DateTime.Today) + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')");
                    Comando_SQL.CommandText = Mi_SQL.ToString();
                    Consecutivo = Comando_SQL.ExecuteOracleScalar().ToString();
                    if (Convert.IsDBNull(Consecutivo))
                    {
                        Consecutivo = 1;
                    }
                    else
                    {
                        Consecutivo = Convert.ToInt32(Consecutivo) + 1;
                    }
                    Datos.P_Numero_Recoleccion = Convert.ToInt32(Consecutivo);
                    Mi_SQL.Length = 0;
                    //Da de alta la recoleccion con los datos proporcionados por el usuario
                    Mi_SQL.Append("INSERT INTO " + Ope_Caj_Recolecciones.Tabla_Ope_Caj_Recolecciones + "(");
                    Mi_SQL.Append(Ope_Caj_Recolecciones.Campo_No_Recoleccion + ", " + Ope_Caj_Recolecciones.Campo_Caja_ID + ", ");
                    Mi_SQL.Append(Ope_Caj_Recolecciones.Campo_No_Turno + ", ");
                    Mi_SQL.Append(Ope_Caj_Recolecciones.Campo_Empleado_ID + ", " + Ope_Caj_Recolecciones.Campo_Num_Recoleccion + ", ");
                    Mi_SQL.Append(Ope_Caj_Recolecciones.Campo_Fecha + ", " + Ope_Caj_Recolecciones.Campo_Monto_Recolectado + ", ");
                    Mi_SQL.Append(Ope_Caj_Recolecciones.Campo_Monto_Tarjeta + ", " + Ope_Caj_Recolecciones.Campo_Monto_Cheque + ", " + Ope_Caj_Recolecciones.Campo_Monto_Transferencia + ", ");
                    Mi_SQL.Append(Ope_Caj_Recolecciones.Campo_Conteo_Tarjeta + ", " + Ope_Caj_Recolecciones.Campo_Conteo_Cheque + ", " + Ope_Caj_Recolecciones.Campo_Conteo_Transferencia + ", ");
                    Mi_SQL.Append(Ope_Caj_Recolecciones.Campo_Recibe_Efectivo + ", ");
                    Mi_SQL.Append(Ope_Caj_Recolecciones.Campo_Usuario_Creo + ", " + Ope_Caj_Recolecciones.Campo_Fecha_Creo + ")");
                    Mi_SQL.Append(" VALUES ('" + No_Recoleccion + "', '" + Datos.P_Caja_ID + "', '" + Datos.P_No_Turno + "', '");
                    Mi_SQL.Append(Datos.P_Empleado_ID + "', " + Datos.P_Numero_Recoleccion + ", ");
                    Mi_SQL.Append("'" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Busqueda) + "', ");
                    Mi_SQL.Append(Datos.P_Monto_Recolectado + ", " + Datos.P_Monto_Tarjeta + ", " + Datos.P_Monto_Cheque + ", " + Datos.P_Monto_Transferencia + ", ");
                    Mi_SQL.Append(Datos.P_Conteo_Tarjeta + ", " + Datos.P_Conteo_Cheque + ", " + Datos.P_Conteo_Transferencia + ", ");
                    Mi_SQL.Append("'" + Datos.P_Recibe_Efectivo + "', '" + Datos.P_Nombre_Usuario + "', SYSDATE)");
                    Comando_SQL.CommandText = Mi_SQL.ToString();
                    Comando_SQL.ExecuteNonQuery();

                    Mi_SQL.Length = 0;
                    //Da de alta el detalle de las denominaciones que fueron proporcionados por el usuario
                    Mi_SQL.Append("INSERT INTO " + Ope_Caj_Recolecciones_Detalles.Tabla_Ope_Caj_Recolecciones_Detalles);
                    Mi_SQL.Append(" (" + Ope_Caj_Recolecciones_Detalles.Campo_No_Recoleccion + ", ");
                    Mi_SQL.Append(Ope_Caj_Recolecciones_Detalles.Campo_No_Turno + ", ");
                    Mi_SQL.Append(Ope_Caj_Recolecciones_Detalles.Campo_Caja_ID + ", ");
                    Mi_SQL.Append(Ope_Caj_Recolecciones_Detalles.Campo_Empleado_ID + ", ");
                    Mi_SQL.Append(Ope_Caj_Recolecciones_Detalles.Campo_Billete_1000 + ", ");
                    Mi_SQL.Append(Ope_Caj_Recolecciones_Detalles.Campo_Billete_500 + ", ");
                    Mi_SQL.Append(Ope_Caj_Recolecciones_Detalles.Campo_Billete_200 + ", ");
                    Mi_SQL.Append(Ope_Caj_Recolecciones_Detalles.Campo_Billete_100 + ", ");
                    Mi_SQL.Append(Ope_Caj_Recolecciones_Detalles.Campo_Billete_50 + ", ");
                    Mi_SQL.Append(Ope_Caj_Recolecciones_Detalles.Campo_Billete_20 + ", ");
                    Mi_SQL.Append(Ope_Caj_Recolecciones_Detalles.Campo_Moneda_20 + ", ");
                    Mi_SQL.Append(Ope_Caj_Recolecciones_Detalles.Campo_Moneda_10 + ", ");
                    Mi_SQL.Append(Ope_Caj_Recolecciones_Detalles.Campo_Moneda_5 + ", ");
                    Mi_SQL.Append(Ope_Caj_Recolecciones_Detalles.Campo_Moneda_2 + ", ");
                    Mi_SQL.Append(Ope_Caj_Recolecciones_Detalles.Campo_Moneda_1 + ", ");
                    Mi_SQL.Append(Ope_Caj_Recolecciones_Detalles.Campo_Moneda_050 + ", ");
                    Mi_SQL.Append(Ope_Caj_Recolecciones_Detalles.Campo_Moneda_020 + ", ");
                    Mi_SQL.Append(Ope_Caj_Recolecciones_Detalles.Campo_Moneda_010 + ")");
                    Mi_SQL.Append(" VALUES ('" + No_Recoleccion +"', '" + Datos.P_No_Turno + "', '" + Datos.P_Caja_ID + "',");
                    Mi_SQL.Append(" '" + Datos.P_Empleado_ID + "', " + Datos.P_Biillete_1000 + ", " + Datos.P_Biillete_500 + ", ");
                    Mi_SQL.Append(Datos.P_Biillete_200 + ", " + Datos.P_Biillete_100 + ", " + Datos.P_Biillete_50 + ", ");
                    Mi_SQL.Append(Datos.P_Biillete_20 + ", " + Datos.P_Moneda_20 + ", " + Datos.P_Moneda_10 + ", ");
                    Mi_SQL.Append(Datos.P_Moneda_5 + ", " + Datos.P_Moneda_2 + ", " + Datos.P_Moneda_1 + ", " + Datos.P_Moneda_050 + ", ");
                    Mi_SQL.Append(Datos.P_Moneda_020 + ", " + Datos.P_Moneda_010 + ")");
                    Comando_SQL.CommandText = Mi_SQL.ToString();
                    Comando_SQL.ExecuteNonQuery();

                    Transaccion_SQL.Commit(); 
                }
                catch (OracleException Ex)
                {
                    Transaccion_SQL.Rollback();
                    throw new Exception("Error: " + Ex.Message);
                }
                catch (DBConcurrencyException Ex)
                {
                    Transaccion_SQL.Rollback();
                    throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
                }
                catch (Exception Ex)
                {
                    Transaccion_SQL.Rollback();
                    throw new Exception("Error: " + Ex.Message);
                }
                finally
                {
                    Conexion_Base.Close();
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Caja_Total_Cobrado
            ///DESCRIPCIÓN: Obtiene todos los tipos de colonias existentes en la base de datos
            ///PARAMETROS:
            ///             1. Caja.  Instancia de la Clase de Negocio de Recolecciones con los datos 
            ///                       de la Caja que va a ser consultada.
            ///CREO: José Alfredo García Pichardo.
            ///FECHA_CREO: 24/Julio/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            public static DataTable Consultar_Caja_Total_Cobrado(Cls_Ope_Caj_Recolecciones_Negocio Caja)
            {
                DataTable Dt_Consulta;
                DataTable Tabla = new DataTable();
                DataRow Registro;
                DataRow Registro_Consulta;
                Double Total_Recoleccion_Tarjeta = 0;
                Double Total_Recoleccion_Cheques = 0;
                Double Total_Recoleccion_Transferncias = 0;
                Int32 Total_Conteo_Tarjeta = 0;
                Int32 Total_Conteo_Cheques = 0;
                Int32 Total_Conteo_Transferencias = 0;

                //Crea la tabla
                Tabla.Columns.Add("CONTEO_TARJETA", typeof(System.Int32));
                Tabla.Columns.Add("TOTAL_TARJETA", typeof(System.Double));
                Tabla.Columns.Add("CONTEO_CHEQUE", typeof(System.Int32));
                Tabla.Columns.Add("TOTAL_CHEQUE", typeof(System.Double));
                Tabla.Columns.Add("CONTEO_TRANSFERENCIA", typeof(System.Int32));
                Tabla.Columns.Add("TOTAL_TRANSFERENCIA", typeof(System.Double));
                Registro = Tabla.NewRow();
                Registro["CONTEO_TARJETA"] = 0;
                Registro["TOTAL_TARJETA"] = 0;
                Registro["CONTEO_CHEQUE"] = 0;
                Registro["TOTAL_CHEQUE"] = 0;
                Registro["CONTEO_TRANSFERENCIA"] = 0;
                Registro["TOTAL_TRANSFERENCIA"] = 0;
                Tabla.Rows.Add(Registro);

                String Mi_SQL;
                try
                {
                    Mi_SQL = "SELECT NVL(SUM(" + Ope_Caj_Recolecciones.Campo_Monto_Tarjeta + "),'0') AS Monto_Tarjeta, NVL(SUM(" + Ope_Caj_Recolecciones.Campo_Monto_Cheque + "),'0') AS Monto_Cheques, NVL(SUM(" + Ope_Caj_Recolecciones.Campo_Monto_Transferencia + "),'0') AS Monto_Transferencias, ";
                    Mi_SQL += "NVL(SUM(" + Ope_Caj_Recolecciones.Campo_Conteo_Tarjeta + "),'0') AS Conteo_Tarjeta, NVL(SUM(" + Ope_Caj_Recolecciones.Campo_Conteo_Cheque + "),'0') AS Conteo_Cheques, NVL(SUM(" + Ope_Caj_Recolecciones.Campo_Conteo_Transferencia + "),'0') AS Conteo_Transferencias";
                    Mi_SQL += " FROM " + Ope_Caj_Recolecciones.Tabla_Ope_Caj_Recolecciones;
                    Mi_SQL += " WHERE " + Ope_Caj_Recolecciones.Campo_No_Turno + " = '" + Caja.P_No_Turno + "'";
                    Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    if (Dt_Consulta.Rows.Count > 0)
                    {
                        Registro_Consulta = Dt_Consulta.Rows[0];
                        Total_Conteo_Tarjeta = Convert.ToInt32(Registro_Consulta["Conteo_Tarjeta"].ToString());
                        Total_Conteo_Cheques = Convert.ToInt32(Registro_Consulta["Conteo_Cheques"].ToString());
                        Total_Conteo_Transferencias = Convert.ToInt32(Registro_Consulta["Conteo_Transferencias"].ToString());

                        Total_Recoleccion_Tarjeta = Convert.ToDouble(Registro_Consulta["Monto_Tarjeta"].ToString());
                        Total_Recoleccion_Cheques = Convert.ToDouble(Registro_Consulta["Monto_Cheques"].ToString());
                        Total_Recoleccion_Transferncias = Convert.ToDouble(Registro_Consulta["Monto_Transferencias"].ToString());
                    }

                    Mi_SQL = "SELECT NVL(SUM(";
                    Mi_SQL = Mi_SQL + Ope_Caj_Pagos_Detalles.Campo_Monto;
                    Mi_SQL = Mi_SQL + "),'0') AS MONTO, ";
                    Mi_SQL = Mi_SQL + "NVL(COUNT(";
                    Mi_SQL = Mi_SQL + Ope_Caj_Pagos_Detalles.Campo_Consecutivo;
                    Mi_SQL = Mi_SQL + "),'0') AS CONTEO ";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                    Mi_SQL = Mi_SQL + " P JOIN " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles;
                    Mi_SQL = Mi_SQL + " PD ON PD." + Ope_Caj_Pagos_Detalles.Campo_No_Pago + " = P." + Ope_Caj_Pagos.Campo_No_Pago;
                    Mi_SQL = Mi_SQL + " AND PD." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + " = 'BANCO'";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Pagos.Campo_No_Turno + " = '" + Caja.P_No_Turno + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Caj_Pagos.Campo_Estatus + " = 'PAGADO'";
                    Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    if (Dt_Consulta.Rows[0]["MONTO"] != null)
                    {
                        Registro = Tabla.Rows[0];
                        Registro.BeginEdit();
                        Registro["CONTEO_TARJETA"] = Convert.ToDouble(Registro["CONTEO_TARJETA"].ToString()) + Convert.ToDouble(Dt_Consulta.Rows[0]["CONTEO"].ToString()) - Total_Conteo_Tarjeta;
                        Registro["TOTAL_TARJETA"] = Convert.ToDouble(Registro["TOTAL_TARJETA"].ToString()) + Convert.ToDouble(Dt_Consulta.Rows[0]["MONTO"].ToString()) - Total_Recoleccion_Tarjeta;
                        Registro.EndEdit();
                        Tabla.AcceptChanges();
                    }
                    Mi_SQL = "SELECT NVL(SUM(";
                    Mi_SQL = Mi_SQL + Ope_Caj_Pagos_Detalles.Campo_Monto;
                    Mi_SQL = Mi_SQL + "),'0') AS MONTO, ";
                    Mi_SQL = Mi_SQL + "NVL(COUNT(";
                    Mi_SQL = Mi_SQL + Ope_Caj_Pagos_Detalles.Campo_Consecutivo;
                    Mi_SQL = Mi_SQL + "),'0') AS CONTEO ";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                    Mi_SQL = Mi_SQL + " P JOIN " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles;
                    Mi_SQL = Mi_SQL + " PD ON PD." + Ope_Caj_Pagos_Detalles.Campo_No_Pago + " = P." + Ope_Caj_Pagos.Campo_No_Pago;
                    Mi_SQL = Mi_SQL + " AND PD." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + " = 'CHEQUE'";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Pagos.Campo_No_Turno + " = '" + Caja.P_No_Turno + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Caj_Pagos.Campo_Estatus + " = 'PAGADO'";
                    Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    if (Dt_Consulta.Rows[0]["MONTO"] != null)
                    {
                        Registro = Tabla.Rows[0];
                        Registro.BeginEdit();
                        Registro["CONTEO_CHEQUE"] = Convert.ToDouble(Registro["CONTEO_CHEQUE"].ToString()) + Convert.ToDouble(Dt_Consulta.Rows[0]["CONTEO"].ToString()) - Total_Conteo_Cheques;
                        Registro["TOTAL_CHEQUE"] = Convert.ToDouble(Registro["TOTAL_CHEQUE"].ToString()) + Convert.ToDouble(Dt_Consulta.Rows[0]["MONTO"].ToString()) - Total_Recoleccion_Cheques;
                        Registro.EndEdit();
                        Tabla.AcceptChanges();
                    }
                    Mi_SQL = "SELECT NVL(SUM(";
                    Mi_SQL = Mi_SQL + Ope_Caj_Pagos_Detalles.Campo_Monto;
                    Mi_SQL = Mi_SQL + "),'0') AS MONTO, ";
                    Mi_SQL = Mi_SQL + "NVL(COUNT(";
                    Mi_SQL = Mi_SQL + Ope_Caj_Pagos_Detalles.Campo_Consecutivo;
                    Mi_SQL = Mi_SQL + "),'0') AS CONTEO ";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                    Mi_SQL = Mi_SQL + " P JOIN " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles;
                    Mi_SQL = Mi_SQL + " PD ON PD." + Ope_Caj_Pagos_Detalles.Campo_No_Pago + " = P." + Ope_Caj_Pagos.Campo_No_Pago;
                    Mi_SQL = Mi_SQL + " AND PD." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + " = 'TRANSFERENCIA'";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Pagos.Campo_No_Turno + " = '" + Caja.P_No_Turno + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Caj_Pagos.Campo_Estatus + " = 'PAGADO'";
                    Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    if (Dt_Consulta.Rows[0]["MONTO"] != null)
                    {
                        Registro = Tabla.Rows[0];
                        Registro.BeginEdit();
                        Registro["CONTEO_TRANSFERENCIA"] = Convert.ToDouble(Registro["CONTEO_TRANSFERENCIA"].ToString()) + Convert.ToDouble(Dt_Consulta.Rows[0]["CONTEO"].ToString()) - Total_Conteo_Transferencias;
                        Registro["TOTAL_TRANSFERENCIA"] = Convert.ToDouble(Registro["TOTAL_TRANSFERENCIA"].ToString()) + Convert.ToDouble(Dt_Consulta.Rows[0]["MONTO"].ToString()) - Total_Recoleccion_Transferncias;
                        Registro.EndEdit();
                        Tabla.AcceptChanges();
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar el Total Recolectado. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Tabla;
            }
        #endregion
    }
}
