using System;
using System.Text;
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
using Presidencia.Constantes;
using Presidencia.Operaciones_Apertura_Turnos.Negocio;


/// <summary>
/// Summary description for Cls_Ope_Pre_Apertura_Turnos_Datos
/// </summary>
namespace Presidencia.Operaciones_Apertura_Turnos.Datos
{
    public class Cls_Ope_Pre_Apertura_Turnos_Datos
    {
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Apertura_Turno
        /// DESCRIPCION : 1.Consulta el último No dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta el Turno del Empleado en la BD con los datos
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 10-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Apertura_Turno(Cls_Ope_Pre_Apertura_Turno_Negocio Datos)
        {
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;
            OracleDataAdapter Da_Turno_Dia;
            DataSet Ds_Turno_Dia;
            StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la cadena de inserción hacía la base de datos
            Object No_Turno;                            //Obtiene el No con el cual se va a guardar el registro
            Object No_Turno_Dia;                            //Obtiene el No con el cual se va a guardar el registro

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
                //Consulta si la apertura del dia esta abierta
                Mi_SQL.Append("SELECT " + Ope_Caj_Turnos_Dia.Campo_No_Turno);
                Mi_SQL.Append(" FROM " + Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia);
                Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos_Dia.Campo_Estatus + " = 'ABIERTO'");
                Mi_SQL.Append(" AND " + Ope_Caj_Turnos_Dia.Campo_Fecha_Turno);
                Mi_SQL.Append(" BETWEEN TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Turno)) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                Mi_SQL.Append(" AND TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Turno)) + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')");
                Comando_SQL.CommandText = Mi_SQL.ToString();
                Da_Turno_Dia = new OracleDataAdapter(Comando_SQL);
                Ds_Turno_Dia = new DataSet();
                Da_Turno_Dia.Fill(Ds_Turno_Dia);
                //Valida si trae datos el dataset
                if (Ds_Turno_Dia.Tables[0].Rows.Count <= 0)
                {
                    Mi_SQL.Length = 0;
                    //Consulta el último No de Turno de Dia que fue agregado a la base de datos
                    Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Caj_Turnos_Dia.Campo_No_Turno + "),'0000000000')");
                    Mi_SQL.Append(" FROM " + Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia);
                    Comando_SQL.CommandText = Mi_SQL.ToString();
                    No_Turno_Dia = Comando_SQL.ExecuteOracleScalar().ToString();

                    if (Convert.IsDBNull(No_Turno_Dia))
                    {
                        No_Turno_Dia = "0000000001";
                    }
                    else
                    {
                        No_Turno_Dia = String.Format("{0:0000000000}", Convert.ToInt32(No_Turno_Dia) + 1);
                    }
                    Mi_SQL.Length = 0;
                    //inserta el registro del turno del dia
                    Mi_SQL.Append("INSERT INTO " + Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia);
                    Mi_SQL.Append(" (" + Ope_Caj_Turnos_Dia.Campo_No_Turno + ", ");
                    Mi_SQL.Append(Ope_Caj_Turnos_Dia.Campo_Fecha_Turno + ", ");
                    Mi_SQL.Append(Ope_Caj_Turnos_Dia.Campo_Hora_Apertura + ", ");
                    Mi_SQL.Append(Ope_Caj_Turnos_Dia.Campo_Estatus + ", ");
                    Mi_SQL.Append(Ope_Caj_Turnos_Dia.Campo_Usuario_Creo + ", " + Ope_Caj_Turnos_Dia.Campo_Fecha_Creo + ")");
                    Mi_SQL.Append(" VALUES ('" + Convert.ToString(No_Turno_Dia) + "', ");
                    Mi_SQL.Append(" '" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Turno)) + "', ");
                    Mi_SQL.Append(" SYSDATE, ");
                    Mi_SQL.Append(" 'ABIERTO', ");
                    Mi_SQL.Append(" '" + Datos.P_Nombre_Empleado + "', SYSDATE)");
                    Comando_SQL.CommandText = Mi_SQL.ToString();
                    Comando_SQL.ExecuteNonQuery();
                }
                else
                {
                    No_Turno_Dia = Ds_Turno_Dia.Tables[0].Rows[0]["No_Turno_Dia"].ToString();
                }
                Datos.P_No_Turno_Dia = Convert.ToString(No_Turno_Dia);
                Mi_SQL.Length = 0;

                //Consulta el último No de Turno que fue agregado a la base de datos
                Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Caj_Turnos.Campo_No_Turno + "),'0000000000')");
                Mi_SQL.Append(" FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos);
                Comando_SQL.CommandText = Mi_SQL.ToString();
                No_Turno = Comando_SQL.ExecuteOracleScalar().ToString();

                if (Convert.IsDBNull(No_Turno))
                {
                    No_Turno = "0000000001";
                }
                else
                {
                    No_Turno = String.Format("{0:0000000000}", Convert.ToInt32(No_Turno) + 1);
                }
                Datos.P_No_Turno = Convert.ToString(No_Turno);
                Mi_SQL.Length = 0;

                //Inserta los valores del turno en la base de datos
                Mi_SQL.Append("INSERT INTO " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos);
                Mi_SQL.Append(" (" + Ope_Caj_Turnos.Campo_No_Turno + ", " + Ope_Caj_Turnos.Campo_No_Turno_Dia + ", " + Ope_Caj_Turnos.Campo_Caja_ID + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Empleado_ID + ", " + Ope_Caj_Turnos.Campo_Recibo_Inicial + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Aplicacion_Pago + ", " + Ope_Caj_Turnos.Campo_Fecha_Turno + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Hora_Apertura + ", " + Ope_Caj_Turnos.Campo_Fondo_Inicial + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Estatus + ", " + Ope_Caj_Turnos.Campo_Total_Bancos + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Total_Cheques + ", " + Ope_Caj_Turnos.Campo_Total_Transferencias + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Total_Recolectado + ", " + Ope_Caj_Turnos.Campo_Total_Efectivo_Sistema + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Total_Efectivo_Caja + ", " + Ope_Caj_Turnos.Campo_Faltante + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Sobrante + ", " + Ope_Caj_Turnos.Campo_Contador_Recibo + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Usuario_Creo + ", " + Ope_Caj_Turnos.Campo_Fecha_Creo + ")");
                Mi_SQL.Append(" VALUES ('" + Datos.P_No_Turno + "', '" + Datos.P_No_Turno_Dia + "', '" + Datos.P_Caja_Id + "', ");
                Mi_SQL.Append(" '" + Datos.P_Empleado_ID + "', '" + String.Format("{0:0000000000}", Datos.P_Recibo_Inicial) + "', ");
                Mi_SQL.Append(" '" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Aplicacion_Pago)) + "', ");
                Mi_SQL.Append(" '" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Turno)) + "', ");
                Mi_SQL.Append("SYSDATE, " + Datos.P_Fondo_Inicial + ", 'ABIERTO', 0, 0, 0, 0, 0, 0, 0, 0, ");
                Mi_SQL.Append(" '" + String.Format("{0:0000000000}", Datos.P_Contador_Recibo) + "', '" + Datos.P_Nombre_Empleado + "', SYSDATE)");
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
        /// NOMBRE DE LA FUNCION: Modifica_Apertura_Turno
        /// DESCRIPCION : Realiza la actualizacion del Turno del Empleado en la BD con los datos
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Ismael Prieto Sánchez
        /// FECHA_CREO  : 19-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Modifica_Apertura_Turno(Cls_Ope_Pre_Apertura_Turno_Negocio Datos)
        {
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;
            StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la cadena de inserción hacía la base de datos
           
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
                //Actualiza el turno del empleado
                Mi_SQL.Append("UPDATE " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos);
                Mi_SQL.Append(" SET " + Ope_Caj_Turnos.Campo_Recibo_Inicial + " = '" + Datos.P_Recibo_Inicial + "'");
                Mi_SQL.Append(", " + Ope_Caj_Turnos.Campo_Contador_Recibo + " = '" + Datos.P_Contador_Recibo + "'");
                Mi_SQL.Append(", " + Ope_Caj_Turnos.Campo_Fondo_Inicial + " = " + Datos.P_Fondo_Inicial);
                Mi_SQL.Append(", " + Ope_Caj_Turnos.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Empleado  + "'");
                Mi_SQL.Append(", " + Ope_Caj_Turnos.Campo_Fecha_Modifico + " = SYSDATE");
                Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos.Campo_No_Turno + " = '" + Datos.P_No_Turno + "'");

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
        /// NOMBRE DE LA FUNCION: Modificar_Datos_Turno
        /// DESCRIPCION : Actualiza los valores del turno para poder contener los valores
        ///               del turno
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 23-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Modificar_Datos_Turno(Cls_Ope_Pre_Apertura_Turno_Negocio Datos)
        {
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;
            StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la cadena de inserción hacía la base de datos

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
                //Actualiza los datos del turno que fueron introducidos para su cierre
                Mi_SQL.Append("UPDATE " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos);
                Mi_SQL.Append(" SET " + Ope_Caj_Turnos.Campo_Fecha_Cierre + " = '" + String.Format("{0:dd/MM/yyyy}", DateTime.Today) + "', ");
                if (!String.IsNullOrEmpty(Datos.P_Recibo_Final)) Mi_SQL.Append(Ope_Caj_Turnos.Campo_Recibo_Final + " = " + Datos.P_Recibo_Final + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Hora_Cierre + "= SYSDATE, ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Total_Bancos + " = " + Datos.P_Total_Bancos + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Total_Cheques + " = " + Datos.P_Total_Cheques + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Total_Transferencias + " = " + Datos.P_Total_Trasnferencias + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Total_Recolectado + " = " + Datos.P_Total_Recolectado + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Total_Efectivo_Sistema + " = " + Datos.P_Total_Efectivo_Sistema + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Total_Efectivo_Caja + " = " + Datos.P_Total_Efectivo_Caja + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Conteo_Bancos + " = " + Datos.P_Conteo_Bancos + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Conteo_Cheques + " = " + Datos.P_Conteo_Cheques + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Conteo_Transferencias + " = " + Datos.P_Conteo_Transferencias + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Total_Recolectado_Bancos + " = " + Datos.P_Total_Recolectado_Bancos + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Total_Recolectado_Cheques + " = " + Datos.P_Total_Recolectado_Cheques + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Total_Recolectado_Transferencias + " = " + Datos.P_Total_Recolectado_Trasnferencias + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Conteo_Recolectado_Bancos + " = " + Datos.P_Conteo_Recolectado_Bancos + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Conteo_Recolectado_Cheques + " = " + Datos.P_Conteo_Recolectado_Cheques + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Conteo_Recolectado_Transferencias + " = " + Datos.P_Conteo_Recolectado_Transferencias + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Faltante + " = " + Datos.P_Faltante + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Sobrante + " = " + Datos.P_Sobrante + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Estatus + " = 'CERRADO', ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_ReApertura_Autorizo + " = 'NO', ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Nombre_Recibe + " = '" + Datos.P_Nombre_Recibe + "', ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Empleado + "', ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Fecha_Modifico + " = SYSDATE");
                Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos.Campo_No_Turno + " = '" + Datos.P_No_Turno + "'");

                Comando_SQL.CommandText = Mi_SQL.ToString();
                Comando_SQL.ExecuteNonQuery();

                Mi_SQL.Length = 0;
                //Elimina los detalles de las recolecciones que se pudieron generar con respecto a las denominaciones
                Mi_SQL.Append("DELETE FROM " + Ope_Caj_Turnos_Detalles.Tabla_Ope_Caj_Turnos_Detalles);
                Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos_Detalles.Campo_No_Turno + " = '" + Datos.P_No_Turno + "'");
                Comando_SQL.CommandText = Mi_SQL.ToString();
                Comando_SQL.ExecuteNonQuery();

                Mi_SQL.Length = 0;
                //Da de alta el detalle de las denominaciones que fueron proporcionados por el usuario
                Mi_SQL.Append("INSERT INTO " + Ope_Caj_Turnos_Detalles.Tabla_Ope_Caj_Turnos_Detalles);
                Mi_SQL.Append(" (" + Ope_Caj_Turnos_Detalles.Campo_No_Turno + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos_Detalles.Campo_Billete_1000 + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos_Detalles.Campo_Billete_500 + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos_Detalles.Campo_Billete_200 + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos_Detalles.Campo_Billete_100 + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos_Detalles.Campo_Billete_50 + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos_Detalles.Campo_Billete_20 + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos_Detalles.Campo_Moneda_20 + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos_Detalles.Campo_Moneda_10 + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos_Detalles.Campo_Moneda_5 + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos_Detalles.Campo_Moneda_2 + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos_Detalles.Campo_Moneda_1 + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos_Detalles.Campo_Moneda_050 + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos_Detalles.Campo_Moneda_020 + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos_Detalles.Campo_Moneda_010 + ")");
                Mi_SQL.Append(" VALUES ('" + Datos.P_No_Turno + "', " + Datos.P_Biillete_1000 + ", " + Datos.P_Biillete_500 + ", ");
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
        /// NOMBRE DE LA FUNCION: Modificar_Datos_Turno
        /// DESCRIPCION : Actualiza los valores del turno para poder contener los valores
        ///               del turno
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 23-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Modificar_Turno_Reapertura(Cls_Ope_Pre_Apertura_Turno_Negocio Datos)
        {
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;
            StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la cadena de inserción hacía la base de datos

            //inicializa la conexion
            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction();  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;              //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;

            try
            {
                //Actualiza los datos del turno que fueron introducidos para su reapertura
                Mi_SQL.Append("UPDATE " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos);
                Mi_SQL.Append(" SET " + Ope_Caj_Turnos.Campo_Fecha_Reapertura + " = '" + String.Format("{0:dd/MM/yyyy}", DateTime.Today) + "', ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Hora_Reapertura + "= SYSDATE, ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Estatus + " = 'ABIERTO', ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Empleado + "', ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Fecha_Modifico + " = SYSDATE");
                Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos.Campo_No_Turno + " = '" + Datos.P_No_Turno + "'");
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
        /// NOMBRE DE LA FUNCION: Autoriza_ReAPertura_Turno
        /// DESCRIPCION : Actualiza los datos de turno para la reapertura
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Ismael Prieto Sánchez
        /// FECHA_CREO  : 12-Diciembre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Autoriza_ReAPertura_Turno(Cls_Ope_Pre_Apertura_Turno_Negocio Datos)
        {
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;
            StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la cadena de inserción hacía la base de datos

            //inicializa la conexion
            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction();  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;              //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;

            try
            {
                //Actualiza los datos del turno que fueron introducidos para su reapertura
                Mi_SQL.Append("UPDATE " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos);
                Mi_SQL.Append(" SET " + Ope_Caj_Turnos.Campo_ReApertura_Nombre_Autorizo + " = '" + Datos.P_ReApertua_Nombre_Autorizo + "', ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_ReApertura_Observaciones_Autorizo + " = '" + Datos.P_ReApertua_Observaciones_Autorizo + "', ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_ReApertura_Fecha_Autorizo + "= SYSDATE, ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_ReApertura_Autorizo + " = 'SI', ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_ReApertura_Empleado_ID_Autorizo + " = '" + Datos.P_Empleado_ID + "', ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Empleado + "', ");
                Mi_SQL.Append(Ope_Caj_Turnos.Campo_Fecha_Modifico + " = SYSDATE");
                Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos.Campo_No_Turno + " = '" + Datos.P_No_Turno + "'");
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
        #region (Métodos Consulta)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Datos_Generales_Turno
            /// DESCRIPCION : Consulta todos los datos generales del turno que se encuentra
            ///               abierto por parte del empleado
            /// PARAMETROS  : Datos: Obtiene el ID del empleado a consultar
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 10-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Datos_Generales_Turno(Cls_Ope_Pre_Apertura_Turno_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Consulta todos los datos del turno que se encuentra abierto por el empleado
                try
                {
                    Mi_SQL.Append("SELECT " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno + ", ");
                    Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Aplicacion_Pago + ", ");
                    Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_ReApertura_Autorizo + ", ");
                    Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Fecha_Turno + ", ");
                    Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Fondo_Inicial + ", ");
                    Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Recibo_Inicial + ", ");
                    Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Hora_Apertura + ", ");
                    Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Estatus + ", ");
                    Mi_SQL.Append(Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS Caja, ");
                    Mi_SQL.Append("(" + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Clave);
                    Mi_SQL.Append(" ||' '|| " + Cat_Pre_Modulos.Campo_Ubicacion + ") AS Modulo");
                    Mi_SQL.Append(" FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + ", ");
                    Mi_SQL.Append(Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + ", " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas);
                    Mi_SQL.Append(" WHERE " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Caja_ID);
                    Mi_SQL.Append(" = " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_ID);
                    Mi_SQL.Append(" AND " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Modulo_Id);
                    Mi_SQL.Append(" = " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Modulo_Id);
                    Mi_SQL.Append(" AND " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Estatus + " = 'ABIERTO'");
                    Mi_SQL.Append(" AND " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'");
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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
            /// NOMBRE DE LA FUNCION: Consulta_Datos_Turno_Empleado
            /// DESCRIPCION : 1. Consulta los datos generales del último turno que fue abierto
            ///                  por el empleado
            ///               2. Si el empledo tuvo algun turno entonces consulta el último
            ///                  No. de Recibo que fue realizado en el sistema
            ///               3. De acuerdo a los valores obtenidos de las consultas anteriores
            ///                  retorna estos valores para ser visualizados por el usuario
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 10-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Datos_Turno_Empleado(Cls_Ope_Pre_Apertura_Turno_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Consulta los datos generales del último turno que fue abierto por el empleado
                DataTable Dt_Turno = new DataTable();       //Obtiene el último turno abierto por el usuaruo y su estatus
                DataTable Dt_Datos_Turno = new DataTable(); //Obtiene los datos generales de último turno que fue abierto por el empleado
                try
                {
                    Mi_SQL.Append("SELECT " + Ope_Caj_Turnos.Campo_No_Turno + ", " + Ope_Caj_Turnos.Campo_Estatus + ", ");
                    Mi_SQL.Append(Ope_Caj_Turnos.Campo_Empleado_ID + ", " + Ope_Caj_Turnos.Campo_Caja_ID + ", " + Ope_Caj_Turnos.Campo_Fecha_Turno);
                    Mi_SQL.Append(" FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos);
                    Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'");
                    if (Datos.P_No_Turno != "")
                    {
                        Mi_SQL.Append(" AND " + Ope_Caj_Turnos.Campo_No_Turno + " = '" + Datos.P_No_Turno + "'");
                    }
                    Mi_SQL.Append(" AND ROWNUM = 1 ORDER BY " + Ope_Caj_Turnos.Campo_No_Turno + " DESC");
                    Dt_Turno = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                    if (Dt_Turno.Rows.Count > 0)
                    {
                        //Se realiza la estructura a contener de los datos
                        Dt_Datos_Turno.Columns.Add(Ope_Caj_Turnos.Campo_No_Turno, typeof(System.String));
                        Dt_Datos_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Empleado_ID, typeof(System.String));
                        Dt_Datos_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Estatus, typeof(System.String));
                        Dt_Datos_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Fecha_Turno, typeof(System.DateTime));
                        Dt_Datos_Turno.Columns.Add("ULTIMO_FOLIO_TURNO", typeof(System.String));
                        DataRow Registro;
                        foreach (DataRow Turno in Dt_Turno.Rows)
                        {
                            Mi_SQL.Length = 0; //Limpia la variable para poder asignar la siguiente consulta a la base de datos
                            //Asigna los valores obtenidos de la consulta
                            Registro = Dt_Datos_Turno.NewRow();
                            Registro[Ope_Caj_Turnos.Campo_No_Turno] = Turno[Ope_Caj_Turnos.Campo_No_Turno].ToString();
                            Registro[Ope_Caj_Turnos.Campo_Empleado_ID] = Turno[Ope_Caj_Turnos.Campo_Empleado_ID].ToString();
                            Registro[Ope_Caj_Turnos.Campo_Estatus] = Turno[Ope_Caj_Turnos.Campo_Estatus].ToString();
                            Registro[Ope_Caj_Turnos.Campo_Fecha_Turno] = Convert.ToDateTime(Turno[Ope_Caj_Turnos.Campo_Fecha_Turno].ToString()).ToString("dd-MMM-yyyy");
                            //Registro[Ope_Caj_Turnos.Campo_Fecha_Turno] = Convert.ToDateTime(String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Turno[Ope_Caj_Turnos.Campo_Fecha_Turno].ToString())));

                            //Consulta el último folio que fue registrado del último turno que fue abierto por el empleado
                            Mi_SQL.Append("SELECT MAX (" + Ope_Caj_Pagos.Campo_No_Recibo + ") AS " + Ope_Caj_Pagos.Campo_No_Recibo);
                            Mi_SQL.Append(" FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
                            Mi_SQL.Append(" WHERE " + Ope_Caj_Pagos.Campo_No_Turno + " = '" + Turno[Ope_Caj_Turnos.Campo_No_Turno].ToString() + "'");
                            Mi_SQL.Append(" AND " + Ope_Caj_Pagos.Campo_Caja_ID + " = '" + Turno[Ope_Caj_Turnos.Campo_Caja_ID].ToString() + "'");
                            DataTable Dt_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                            if (Dt_Datos.Rows.Count > 0)
                            {
                                foreach (DataRow Folio in Dt_Datos.Rows)
                                {
                                    Registro["ULTIMO_FOLIO_TURNO"] = Folio[Ope_Caj_Pagos.Campo_No_Recibo].ToString(); //Asigna al campo correspondiente el último no de recibo realizado
                                }
                            }
                            Dt_Datos_Turno.Rows.Add(Registro);
                        }
                    }
                    return Dt_Datos_Turno;
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
            /// NOMBRE DE LA FUNCION: Consulta_Turno_Abierto_Caja
            /// DESCRIPCION : Consulta si hay algun turno abierto en la caja asignada para el
            ///               empleado
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 11-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Turno_Abierto_Caja(Cls_Ope_Pre_Apertura_Turno_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la consulta a realizar a la base de datos
                try
                {
                    Mi_SQL.Append("SELECT " + Ope_Caj_Turnos.Campo_Estatus + ", " + Ope_Caj_Turnos.Campo_Usuario_Creo);
                    Mi_SQL.Append(" FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos);
                    Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos.Campo_Caja_ID + " = '" + Datos.P_Caja_Id + "'");
                    Mi_SQL.Append(" AND " + Ope_Caj_Turnos.Campo_Estatus + " = 'ABIERTO'");
                    Mi_SQL.Append(" AND " + Ope_Caj_Turnos.Campo_Fecha_Turno);
                    Mi_SQL.Append(" BETWEEN TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Turno)) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                    Mi_SQL.Append(" AND TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Turno)) + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')");
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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
            /// NOMBRE DE LA FUNCION: Consulta_Turno_Abierto_Dia
            /// DESCRIPCION : Consulta si hay algun turno abierto del día
            /// PARAMETROS  : 
            /// CREO        : Ismael Prieto Sánchez
            /// FECHA_CREO  : 20-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Turno_Abierto_Dia(Cls_Ope_Pre_Apertura_Turno_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la consulta a realizar a la base de datos
                try
                {
                    Mi_SQL.Append("SELECT " + Ope_Caj_Turnos_Dia.Campo_No_Turno);
                    Mi_SQL.Append(" FROM " + Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia);
                    Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos_Dia.Campo_Estatus + " = 'ABIERTO'");
                    Mi_SQL.Append(" AND " + Ope_Caj_Turnos_Dia.Campo_Fecha_Turno);
                    Mi_SQL.Append(" < TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Turno)) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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
            /// NOMBRE DE LA FUNCION: Consultar_Apertura
            /// DESCRIPCION : Consulta las cajas que se encuentran abietas
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 12-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consultar_Apertura(Cls_Ope_Pre_Apertura_Turno_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la consulta a realizar a la base de datos
                try
                {
                    Mi_SQL.Append("SELECT * FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos);
                    Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos.Campo_Estatus + " = '" +  Datos.P_Estatus + "'");
                    Mi_SQL.Append(" ORDER BY " + Ope_Caj_Turnos.Campo_No_Turno);
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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
            ///NOMBRE DE LA FUNCIÓN: Consultar_Turnos
            ///DESCRIPCIÓN: Obtiene todos las Aperturas de turnos que estan dadas de 
            ///             alta en la Base de Datos
            ///PARAMENTROS:   
            ///             1.  Turno.          Parametro de donde se sacara si habra o no un filtro de busqueda, en este
            ///                                 caso el filtro es la hora de apertura y el número de recibo.
            ///CREO: Miguel Angel Bedolla Moreno.
            ///FECHA_CREO: 01/Agosto/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            public static DataTable Consultar_Turnos(Cls_Ope_Pre_Apertura_Turno_Negocio Turno)
            {
                DataTable tabla = new DataTable();
                try
                {
                    String Mi_SQL = "SELECT c." + Ope_Caj_Turnos.Campo_No_Turno + " AS " + Ope_Caj_Turnos.Campo_No_Turno;
                    Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Turnos.Campo_Hora_Apertura + " AS " + Ope_Caj_Turnos.Campo_Hora_Apertura;
                    Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Turnos.Campo_Hora_Cierre + " AS " + Ope_Caj_Turnos.Campo_Hora_Cierre;
                    Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Turnos.Campo_Fecha_Turno + " AS " + Ope_Caj_Turnos.Campo_Fecha_Turno;
                    Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Turnos.Campo_Estatus + " AS " + Ope_Caj_Turnos.Campo_Estatus;
                    Mi_SQL = Mi_SQL + ", ca." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS " + Cat_Pre_Cajas.Campo_Numero_De_Caja;
                    Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Turnos.Campo_Usuario_Creo + " AS CAJERO";
                    Mi_SQL = Mi_SQL + ", m." + Cat_Pre_Modulos.Campo_Descripcion + " AS " + Cat_Pre_Modulos.Campo_Descripcion;
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + " c";
                    Mi_SQL = Mi_SQL + " JOIN " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " ca";
                    Mi_SQL = Mi_SQL + " ON c." + Ope_Caj_Turnos.Campo_Caja_Id + "=ca." + Cat_Pre_Cajas.Campo_Caja_Id;
                    Mi_SQL = Mi_SQL + " JOIN " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + " m";
                    Mi_SQL = Mi_SQL + " ON m." + Cat_Pre_Modulos.Campo_Modulo_Id + "=ca." + Cat_Pre_Cajas.Campo_Modulo_Id;
                    if (Turno.P_Filtro.Length != 0)
                    {
                        Mi_SQL = Mi_SQL + " WHERE c." + Ope_Caj_Turnos.Campo_Usuario_Creo + " LIKE '%" + Turno.P_Filtro + "%' OR c." + Ope_Caj_Turnos.Campo_No_Turno + " LIKE '%" + Turno.P_Filtro + "%'";
                    }
                    if (Turno.P_Estatus != null && Turno.P_Estatus != "" && Turno.P_Filtro.Length == 0)
                    {
                        Mi_SQL = Mi_SQL + " WHERE c." + Ope_Caj_Turnos.Campo_Estatus + " = '" + Turno.P_Estatus + "'";
                    }
                    else if (Turno.P_Estatus != null && Turno.P_Estatus != "" && Turno.P_Filtro.Length != 0)
                    {
                        Mi_SQL = Mi_SQL + " AND c." + Ope_Caj_Turnos.Campo_Estatus + " = '" + Turno.P_Estatus + "'";
                    }

                    Mi_SQL = Mi_SQL + " ORDER BY c." + Ope_Caj_Turnos.Campo_Fecha_Turno + " DESC";
                    DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (dataset != null)
                    {
                        tabla = dataset.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros de Apertura de Turnos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return tabla;
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consultar_Datos_Turno
            /// DESCRIPCION : Consulta los daros del último turno que fue abierto por el empleado
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 22-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consultar_Datos_Turno(Cls_Ope_Pre_Apertura_Turno_Negocio Datos)
            {
                DataTable Dt_Datos_Turno = new DataTable(); //Obtiene los valores de la consulta
                DataTable Dt_Turno = new DataTable(); //Obtiene los valores para la visualización del usuario
                StringBuilder Mi_SQL = new StringBuilder(); //Variable que va a contener la consulta de los datos
                try
                {
                    //Consulta todos los datos del último turno abierto por el empleado
                    Mi_SQL.Append("SELECT * FROM (SELECT * FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos);
                    Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'");
                    if (!String.IsNullOrEmpty(Datos.P_Estatus)) Mi_SQL.Append(" AND " + Ope_Caj_Turnos.Campo_Estatus + " = '" + Datos.P_Estatus + "'");
                    Mi_SQL.Append(" ORDER BY " + Ope_Caj_Turnos.Campo_No_Turno + " DESC)");
                    Mi_SQL.Append(" WHERE ROWNUM = 1");
                    Dt_Datos_Turno = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                    if (Dt_Datos_Turno.Rows.Count > 0)
                    {
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_No_Turno, typeof(System.String));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Caja_ID, typeof(System.String));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Recibo_Inicial, typeof(System.String));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Aplicacion_Pago, typeof(System.DateTime));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Fondo_Inicial, typeof(System.Double));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Hora_Apertura, typeof(System.DateTime));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Hora_Cierre, typeof(System.DateTime));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Fecha_Turno, typeof(System.DateTime));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Estatus, typeof(System.String));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Conteo_Bancos, typeof(System.Int32));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Total_Bancos, typeof(System.Double));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Conteo_Cheques, typeof(System.Int32));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Total_Cheques, typeof(System.Double));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Total_Efectivo_Caja, typeof(System.Double));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Total_Efectivo_Sistema, typeof(System.Double));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Total_Recolectado, typeof(System.Double));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Conteo_Transferencias, typeof(System.Int32));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Total_Transferencias, typeof(System.Double));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Faltante, typeof(System.Double));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Sobrante, typeof(System.Double));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Recibo_Final, typeof(System.String));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Fecha_Cierre, typeof(System.DateTime));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_No_Turno_Dia, typeof(System.String));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Nombre_Recibe, typeof(System.String));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Conteo_Recolectado_Bancos, typeof(System.Int32));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Total_Recolectado_Bancos, typeof(System.Double));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Conteo_Recolectado_Cheques, typeof(System.Int32));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Total_Recolectado_Cheques, typeof(System.Double));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Conteo_Recolectado_Transferencias, typeof(System.Int32));
                        Dt_Turno.Columns.Add(Ope_Caj_Turnos.Campo_Total_Recolectado_Transferencias, typeof(System.Double));
                        Dt_Turno.Columns.Add("Caja", typeof(System.String));
                        Dt_Turno.Columns.Add("Modulo", typeof(System.String));
                        DataRow Registro;
                        Registro = Dt_Turno.NewRow();
                        foreach(DataRow Renglon in Dt_Datos_Turno.Rows)
                        {
                            Mi_SQL.Length = 0;
                            //Consulta la caja y el modulo que pertenece al turno que fue obtenido de la consulta anterior
                            Mi_SQL.Append("SELECT " + Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS Caja,");
                            Mi_SQL.Append(" (" + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Clave);
                            Mi_SQL.Append(" ||' '|| " + Cat_Pre_Modulos.Campo_Ubicacion + ") AS Modulo");
                            Mi_SQL.Append(" FROM " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + ", " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas);
                            Mi_SQL.Append(" WHERE " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Caja_ID);
                            Mi_SQL.Append(" = '" + Renglon[Ope_Caj_Turnos.Campo_Caja_ID].ToString() + "'");
                            Mi_SQL.Append(" AND " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Modulo_Id);
                            Mi_SQL.Append(" = " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Modulo_Id);
                            DataTable Dt_Caja_Modulo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                            foreach (DataRow Caja_Modulo in Dt_Caja_Modulo.Rows)
                            {
                                Registro["Caja"] = Caja_Modulo["Caja"].ToString();
                                Registro["Modulo"] = Caja_Modulo["Modulo"].ToString();
                            }
                            //Agrega los valores obtenidos de la consulta anterior al registro para el paso de valores
                            Registro[Ope_Caj_Turnos.Campo_No_Turno] = Renglon[Ope_Caj_Turnos.Campo_No_Turno].ToString();
                            Registro[Ope_Caj_Turnos.Campo_Caja_ID] = Renglon[Ope_Caj_Turnos.Campo_Caja_ID].ToString();
                            Registro[Ope_Caj_Turnos.Campo_Recibo_Inicial] = Renglon[Ope_Caj_Turnos.Campo_Recibo_Inicial].ToString();
                            Registro[Ope_Caj_Turnos.Campo_Aplicacion_Pago] = Convert.ToDateTime(Renglon[Ope_Caj_Turnos.Campo_Aplicacion_Pago].ToString());
                            Registro[Ope_Caj_Turnos.Campo_Fondo_Inicial] = Convert.ToDouble(Renglon[Ope_Caj_Turnos.Campo_Fondo_Inicial].ToString());
                            Registro[Ope_Caj_Turnos.Campo_Hora_Apertura] = Convert.ToDateTime(Renglon[Ope_Caj_Turnos.Campo_Hora_Apertura].ToString());
                            if (!String.IsNullOrEmpty(Renglon[Ope_Caj_Turnos.Campo_Hora_Cierre].ToString()))
                            {
                                Registro[Ope_Caj_Turnos.Campo_Hora_Cierre] = Convert.ToDateTime(Renglon[Ope_Caj_Turnos.Campo_Hora_Cierre].ToString());
                            }
                            Registro[Ope_Caj_Turnos.Campo_Fecha_Turno] = Convert.ToDateTime(Renglon[Ope_Caj_Turnos.Campo_Fecha_Turno].ToString());
                            Registro[Ope_Caj_Turnos.Campo_Estatus] = Renglon[Ope_Caj_Turnos.Campo_Estatus].ToString();
                            if (!String.IsNullOrEmpty(Renglon[Ope_Caj_Turnos.Campo_Conteo_Bancos].ToString()))
                            {
                                Registro[Ope_Caj_Turnos.Campo_Conteo_Bancos] = Convert.ToInt32(Renglon[Ope_Caj_Turnos.Campo_Conteo_Bancos].ToString());
                            }
                            else
                            {
                                Registro[Ope_Caj_Turnos.Campo_Conteo_Bancos] = 0;
                            }
                            Registro[Ope_Caj_Turnos.Campo_Total_Bancos] = Convert.ToDouble(Renglon[Ope_Caj_Turnos.Campo_Total_Bancos].ToString());
                            if (!String.IsNullOrEmpty(Renglon[Ope_Caj_Turnos.Campo_Conteo_Cheques].ToString()))
                            {
                                Registro[Ope_Caj_Turnos.Campo_Conteo_Cheques] = Convert.ToInt32(Renglon[Ope_Caj_Turnos.Campo_Conteo_Cheques].ToString());
                            }
                            else
                            {
                                Registro[Ope_Caj_Turnos.Campo_Conteo_Cheques] = 0;
                            }
                            Registro[Ope_Caj_Turnos.Campo_Total_Cheques] = Convert.ToDouble(Renglon[Ope_Caj_Turnos.Campo_Total_Cheques].ToString());
                            Registro[Ope_Caj_Turnos.Campo_Total_Efectivo_Caja] = Convert.ToDouble(Renglon[Ope_Caj_Turnos.Campo_Total_Efectivo_Caja].ToString());
                            Registro[Ope_Caj_Turnos.Campo_Total_Efectivo_Sistema] = Convert.ToDouble(Renglon[Ope_Caj_Turnos.Campo_Total_Efectivo_Sistema].ToString());
                            Registro[Ope_Caj_Turnos.Campo_Total_Recolectado] = Convert.ToDouble(Renglon[Ope_Caj_Turnos.Campo_Total_Recolectado].ToString());
                            if (!String.IsNullOrEmpty(Renglon[Ope_Caj_Turnos.Campo_Conteo_Transferencias].ToString()))
                            {
                                Registro[Ope_Caj_Turnos.Campo_Conteo_Transferencias] = Convert.ToInt32(Renglon[Ope_Caj_Turnos.Campo_Conteo_Transferencias].ToString());
                            }
                            else
                            {
                                Registro[Ope_Caj_Turnos.Campo_Conteo_Transferencias] = 0;
                            }
                            Registro[Ope_Caj_Turnos.Campo_Total_Transferencias] = Convert.ToDouble(Renglon[Ope_Caj_Turnos.Campo_Total_Transferencias].ToString());
                            Registro[Ope_Caj_Turnos.Campo_Faltante] = Convert.ToDouble(Renglon[Ope_Caj_Turnos.Campo_Faltante].ToString());
                            Registro[Ope_Caj_Turnos.Campo_Sobrante] = Convert.ToDouble(Renglon[Ope_Caj_Turnos.Campo_Sobrante].ToString());
                            Registro[Ope_Caj_Turnos.Campo_Recibo_Final] = Renglon[Ope_Caj_Turnos.Campo_Recibo_Final].ToString();
                            if (!String.IsNullOrEmpty(Renglon[Ope_Caj_Turnos.Campo_Fecha_Cierre].ToString()))
                            {
                                Registro[Ope_Caj_Turnos.Campo_Fecha_Cierre] = Convert.ToDateTime(Renglon[Ope_Caj_Turnos.Campo_Fecha_Cierre].ToString());
                            }
                            Registro[Ope_Caj_Turnos.Campo_No_Turno_Dia] = Renglon[Ope_Caj_Turnos.Campo_No_Turno_Dia].ToString();
                            if (!String.IsNullOrEmpty(Renglon[Ope_Caj_Turnos.Campo_Nombre_Recibe].ToString()))
                            {
                                Registro[Ope_Caj_Turnos.Campo_Nombre_Recibe] = Renglon[Ope_Caj_Turnos.Campo_Nombre_Recibe].ToString();
                            }
                            else
                            {
                                Registro[Ope_Caj_Turnos.Campo_Nombre_Recibe] = "";
                            }
                            if (!String.IsNullOrEmpty(Renglon[Ope_Caj_Turnos.Campo_Conteo_Recolectado_Bancos].ToString()))
                            {
                                Registro[Ope_Caj_Turnos.Campo_Conteo_Recolectado_Bancos] = Convert.ToInt32(Renglon[Ope_Caj_Turnos.Campo_Conteo_Recolectado_Bancos].ToString());
                            }
                            else
                            {
                                Registro[Ope_Caj_Turnos.Campo_Conteo_Recolectado_Bancos] = 0;
                            }
                            if (!String.IsNullOrEmpty(Renglon[Ope_Caj_Turnos.Campo_Total_Recolectado_Bancos].ToString()))
                            {
                                Registro[Ope_Caj_Turnos.Campo_Total_Recolectado_Bancos] = Convert.ToDouble(Renglon[Ope_Caj_Turnos.Campo_Total_Recolectado_Bancos].ToString());
                            }
                            else
                            {
                                Registro[Ope_Caj_Turnos.Campo_Total_Recolectado_Bancos] = 0;
                            }
                            if (!String.IsNullOrEmpty(Renglon[Ope_Caj_Turnos.Campo_Conteo_Recolectado_Cheques].ToString()))
                            {
                                Registro[Ope_Caj_Turnos.Campo_Conteo_Recolectado_Cheques] = Convert.ToInt32(Renglon[Ope_Caj_Turnos.Campo_Conteo_Recolectado_Cheques].ToString());
                            }
                            else
                            {
                                Registro[Ope_Caj_Turnos.Campo_Conteo_Recolectado_Cheques] = 0;
                            }
                            if (!String.IsNullOrEmpty(Renglon[Ope_Caj_Turnos.Campo_Total_Recolectado_Cheques].ToString()))
                            {
                                Registro[Ope_Caj_Turnos.Campo_Total_Recolectado_Cheques] = Convert.ToDouble(Renglon[Ope_Caj_Turnos.Campo_Total_Recolectado_Cheques].ToString());
                            }
                            else
                            {
                                Registro[Ope_Caj_Turnos.Campo_Total_Recolectado_Cheques] = 0;
                            }
                            if (!String.IsNullOrEmpty(Renglon[Ope_Caj_Turnos.Campo_Conteo_Recolectado_Transferencias].ToString()))
                            {
                                Registro[Ope_Caj_Turnos.Campo_Conteo_Recolectado_Transferencias] = Convert.ToInt32(Renglon[Ope_Caj_Turnos.Campo_Conteo_Recolectado_Transferencias].ToString());
                            }
                            else
                            {
                                Registro[Ope_Caj_Turnos.Campo_Conteo_Recolectado_Transferencias] = 0;
                            }
                            if (!String.IsNullOrEmpty(Renglon[Ope_Caj_Turnos.Campo_Total_Recolectado_Transferencias].ToString()))
                            {
                                Registro[Ope_Caj_Turnos.Campo_Total_Recolectado_Transferencias] = Convert.ToDouble(Renglon[Ope_Caj_Turnos.Campo_Total_Recolectado_Transferencias].ToString());
                            }
                            else
                            {
                                Registro[Ope_Caj_Turnos.Campo_Total_Recolectado_Transferencias] = 0;
                            }
                            Dt_Turno.Rows.Add(Registro);
                        }
                    }   
                    //Mi_SQL.Append("SELECT " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + ".*,");
                    //Mi_SQL.Append(" (" + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Clave);
                    //Mi_SQL.Append(" ||' '|| " + Cat_Pre_Cajas.Campo_Numero_De_Caja + ") AS Caja,");
                    //Mi_SQL.Append(" (" + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Clave);
                    //Mi_SQL.Append(" ||' '|| " + Cat_Pre_Modulos.Campo_Ubicacion + ") AS Modulo");
                    //Mi_SQL.Append(" FROM (SELECT " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + ".*,");
                    //Mi_SQL.Append(" (" + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Clave);
                    //Mi_SQL.Append(" ||' '|| " + Cat_Pre_Cajas.Campo_Numero_De_Caja + ") AS Caja,");
                    //Mi_SQL.Append(" (" + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Clave);
                    //Mi_SQL.Append(" ||' '|| " + Cat_Pre_Modulos.Campo_Ubicacion + ") AS Modulo");
                    //Mi_SQL.Append(" FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + ", ");
                    //Mi_SQL.Append(Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + ", " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas);
                    //Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'");
                    //Mi_SQL.Append(" AND " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Caja_ID);
                    //Mi_SQL.Append(" = " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_ID);
                    //Mi_SQL.Append(" AND " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Modulo_Id);
                    //Mi_SQL.Append(" = " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Modulo_Id);
                    //if (!String.IsNullOrEmpty(Datos.P_Estatus)) Mi_SQL.Append(" AND " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Estatus + " = '" + Datos.P_Estatus + "'");
                    //Mi_SQL.Append(" ORDER BY " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno + " DESC)");
                    //Mi_SQL.Append(" WHERE ROWNUM = 1");
                    return Dt_Turno;
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
            /// NOMBRE DE LA FUNCION: Consultar_Formas_Pago_Turno
            /// DESCRIPCION : Consulta el monto total que se tuvo durante el turno de las diferentes
            ///               formas de pago
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 23-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consultar_Formas_Pago_Turno(Cls_Ope_Pre_Apertura_Turno_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Variable que va a contener la consulta de los datos
                try
                {
                    Mi_SQL.Append("WITH PAGOS_CANCELADOS AS (");
                    Mi_SQL.Append("SELECT PAGOS_1." + Ope_Caj_Pagos.Campo_No_Recibo);
                    Mi_SQL.Append(" ,PAGOS_1." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID);
                    Mi_SQL.Append(" FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " PASIVOS");
                    Mi_SQL.Append(" ," + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " PAGOS_1");
                    Mi_SQL.Append(" ," + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " PAGOS_2");
                    Mi_SQL.Append(" ," + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " CAJA");
                    Mi_SQL.Append(" WHERE PAGOS_1." + Ope_Caj_Pagos.Campo_No_Pago + " = PASIVOS." + Ope_Ing_Pasivo.Campo_No_Pago);
                    Mi_SQL.Append(" AND PAGOS_1." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " = PAGOS_2." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID);
                    Mi_SQL.Append(" AND PAGOS_1." + Ope_Caj_Pagos.Campo_No_Recibo + " = PAGOS_2." + Ope_Caj_Pagos.Campo_No_Recibo);
                    Mi_SQL.Append(" AND (PAGOS_1." + Ope_Caj_Pagos.Campo_Caja_ID + " = CAJA." + Cat_Pre_Cajas.Campo_Caja_Id);
                    Mi_SQL.Append("   OR PAGOS_2." + Ope_Caj_Pagos.Campo_Caja_ID + " = CAJA." + Cat_Pre_Cajas.Campo_Caja_Id + ")");
                    Mi_SQL.Append(" AND PAGOS_1." + Ope_Caj_Pagos.Campo_Estatus + " = 'PAGADO'");
                    Mi_SQL.Append(" AND PAGOS_2." + Ope_Caj_Pagos.Campo_Estatus + " = 'CANCELADO'");
                    Mi_SQL.Append(" AND PAGOS_1." + Ope_Caj_Pagos.Campo_No_Turno + " = '" + Datos.P_No_Turno + "'");
                    Mi_SQL.Append(" AND PASIVOS." + Ope_Ing_Pasivo.Campo_Descripcion + " <> 'AJUSTE TARIFARIO'");
                    Mi_SQL.Append(") ");

                    Mi_SQL.Append("SELECT NVL(SUM(" + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Monto + "), 0) AS Total_Pagado, ");
                    Mi_SQL.Append("NVL(COUNT(" + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Consecutivo + "), 0) AS Conteo, ");
                    Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago);
                    Mi_SQL.Append(" FROM " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " PAGOS");
                    Mi_SQL.Append(" WHERE PAGOS." + Ope_Caj_Pagos.Campo_No_Pago);
                    Mi_SQL.Append(" = " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_No_Pago);
                    //Mi_SQL.Append(" AND PAGOS." + Ope_Caj_Pagos.Campo_Estatus + " = 'PAGADO'");
                    Mi_SQL.Append(" AND PAGOS." + Ope_Caj_Pagos.Campo_No_Turno + " = '" + Datos.P_No_Turno + "'");
                    Mi_SQL.Append(" AND NOT PAGOS." + Ope_Caj_Pagos.Campo_No_Recibo + " || PAGOS." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " IN (SELECT  PAGOS_CANCELADOS." + Ope_Caj_Pagos.Campo_No_Recibo + " || PAGOS_CANCELADOS." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " FROM PAGOS_CANCELADOS)");
                    Mi_SQL.Append(" GROUP BY " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago);
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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
            /// NOMBRE DE LA FUNCION: Consultar_Monto_Recolectado
            /// DESCRIPCION : Consulta lo que fue recolectado durante el turno por el usuario
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 23-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consultar_Monto_Recolectado(Cls_Ope_Pre_Apertura_Turno_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Variable que va a contener la consulta de los datos
                try
                {
                    Mi_SQL.Append("SELECT NVL(SUM(" + Ope_Caj_Recolecciones.Campo_Monto_Recolectado + "), 0) AS Total_Recoleccion,");
                    Mi_SQL.Append(" NVL(SUM(" + Ope_Caj_Recolecciones.Campo_Monto_Tarjeta + "), 0) AS Total_Tarjeta,");
                    Mi_SQL.Append(" NVL(SUM(" + Ope_Caj_Recolecciones.Campo_Monto_Cheque + "), 0) AS Total_Cheques,");
                    Mi_SQL.Append(" NVL(SUM(" + Ope_Caj_Recolecciones.Campo_Monto_Transferencia + "), 0) AS Total_Transferencias,");
                    Mi_SQL.Append(" NVL(SUM(" + Ope_Caj_Recolecciones.Campo_Conteo_Tarjeta + "), 0) AS Conteo_Tarjeta,");
                    Mi_SQL.Append(" NVL(SUM(" + Ope_Caj_Recolecciones.Campo_Conteo_Cheque + "), 0) AS Conteo_Cheques,");
                    Mi_SQL.Append(" NVL(SUM(" + Ope_Caj_Recolecciones.Campo_Conteo_Transferencia + "), 0) AS Conteo_Transferncias,");
                    Mi_SQL.Append(" COUNT(*) AS No_Recolecciones");
                    Mi_SQL.Append(" FROM " + Ope_Caj_Recolecciones.Tabla_Ope_Caj_Recolecciones);
                    Mi_SQL.Append(" WHERE " + Ope_Caj_Recolecciones.Campo_No_Turno + " = '" + Datos.P_No_Turno + "'");
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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
            /// NOMBRE DE LA FUNCION: Consultar_Monto_Recolectado_Detalles
            /// DESCRIPCION : Consulta el detalle de los cortes parciales recolectados
            /// PARAMETROS  : 
            /// CREO        : Ismael Prieto Sánchez
            /// FECHA_CREO  : 17-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consultar_Monto_Recolectado_Detalles(Cls_Ope_Pre_Apertura_Turno_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Variable que va a contener la consulta de los datos
                try
                {
                    Mi_SQL.Append("SELECT " + Ope_Caj_Recolecciones.Campo_Num_Recoleccion + ", " + Ope_Caj_Recolecciones.Campo_Recibe_Efectivo + ", " + Ope_Caj_Recolecciones.Campo_Monto_Recolectado + ", ");
                    Mi_SQL.Append(Ope_Caj_Recolecciones.Campo_Monto_Tarjeta + ", " + Ope_Caj_Recolecciones.Campo_Monto_Cheque + ", " + Ope_Caj_Recolecciones.Campo_Monto_Transferencia);
                    Mi_SQL.Append(" FROM " + Ope_Caj_Recolecciones.Tabla_Ope_Caj_Recolecciones);
                    Mi_SQL.Append(" WHERE " + Ope_Caj_Recolecciones.Campo_No_Turno + " = '" + Datos.P_No_Turno + "'");
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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
            /// NOMBRE DE LA FUNCION: Consulta_Detalles_Turno
            /// DESCRIPCION : Consulta las denominaciones que se dieron de alta para el cierre
            ///               del turno
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 23-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Detalles_Turno(Cls_Ope_Pre_Apertura_Turno_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Variable que va a contener la consulta de los datos
                try
                {
                    Mi_SQL.Append("SELECT * FROM " + Ope_Caj_Turnos_Detalles.Tabla_Ope_Caj_Turnos_Detalles);
                    Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos_Detalles.Campo_No_Turno + " = '" + Datos.P_No_Turno + "'");
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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
            /// NOMBRE DE LA FUNCION: Consulta_Datos_Generales_Turno_Fecha
            /// DESCRIPCION : Consulta todos los datos generales del turno que fue abierto por
            ///               el empleado en una fecha en especifico
            /// PARAMETROS  : Datos: Obtiene el ID del empleado a consultar
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 27-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Datos_Generales_Turno_Fecha(Cls_Ope_Pre_Apertura_Turno_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Consulta todos los datos del turno que se encuentra abierto por el empleado
                try
                {
                    Mi_SQL.Append("SELECT " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno + ", ");
                    Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno_Dia + ", ");
                    Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_ReApertura_Autorizo + ", ");
                    Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Aplicacion_Pago + ", ");
                    Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Fecha_Turno + ", ");
                    Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Fondo_Inicial + ", ");
                    Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Recibo_Inicial + ", ");
                    Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Hora_Apertura + ", ");
                    Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Hora_Cierre + ", ");
                    Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Estatus + ", ");
                    Mi_SQL.Append(Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS Caja, ");
                    Mi_SQL.Append("(" + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Clave);
                    Mi_SQL.Append(" ||' '|| " + Cat_Pre_Modulos.Campo_Ubicacion + ") AS Modulo");
                    Mi_SQL.Append(" FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + ", ");
                    Mi_SQL.Append(Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + ", " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas);
                    Mi_SQL.Append(" WHERE " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Caja_ID);
                    Mi_SQL.Append(" = " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_ID);
                    Mi_SQL.Append(" AND " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Modulo_Id);
                    Mi_SQL.Append(" = " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Modulo_Id);
                    Mi_SQL.Append(" AND " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'");
                    Mi_SQL.Append(" AND " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Fecha_Turno);
                    Mi_SQL.Append(" BETWEEN TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Turno)) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                    Mi_SQL.Append(" AND TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Turno)) + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')");
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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
            /// NOMBRE DE LA FUNCION: Consulta_Datos_Cierre_Dia
            /// DESCRIPCION : Consulta el Estatus del Cierre del Día al cual pertenece el
            ///               Turno que se esta consultando
            /// PARAMETROS  : Datos: Son los parámetros de consulta para el filtro de valores
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 27-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static String Consulta_Datos_Cierre_Dia(Cls_Ope_Pre_Apertura_Turno_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Variable que va a contener la consulta de los datos
                try
                {
                    Mi_SQL.Append("SELECT " + Ope_Caj_Turnos_Dia.Campo_Estatus);
                    Mi_SQL.Append(" FROM " + Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia);
                    Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos_Dia.Campo_No_Turno + " = '" + Datos.P_No_Turno_Dia + "'");
                    return Convert.ToString(OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()));
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
            /// NOMBRE DE LA FUNCION: Valida_Recibo_Inicial
            /// DESCRIPCION : Consulta el Estatus del Cierre del Día al cual pertenece el
            ///               Turno que se esta consultando
            /// PARAMETROS  : Datos: Son los parámetros de consulta para el filtro de valores
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 27-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static Boolean Valida_Recibo_Inicial(Cls_Ope_Pre_Apertura_Turno_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Variable que va a contener la consulta de los datos
                Object Conteo; //Almacena el conteo para ver si existe el contrarecibo

                try
                {
                    Mi_SQL.Append("SELECT NVL(COUNT(" + Ope_Caj_Pagos.Campo_No_Pago + "),'')");
                    Mi_SQL.Append(" FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
                    Mi_SQL.Append(" WHERE " + Ope_Caj_Pagos.Campo_No_Recibo + " = '" + Datos.P_Recibo_Inicial + "'");
                    Mi_SQL.Append(" OR " + Ope_Caj_Pagos.Campo_No_Recibo + " = '" + Convert.ToInt32(Datos.P_Recibo_Inicial) + "'");
                    Conteo = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).ToString();
                    if (String.IsNullOrEmpty(Conteo.ToString()))
                    {
                        return false;
                    }
                    else
                    {
                        if (Conteo.ToString() == "0")
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    
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
            /// NOMBRE DE LA FUNCION: Consulta_Ultimo_Folio
            /// DESCRIPCION : Consulta el ultimo folio utilizado por el empleado en una caja
            /// PARAMETROS  : Datos: Son los parámetros de consulta para el filtro de valores
            /// CREO        : Ismael Prieto Sánchez
            /// FECHA_CREO  : 30-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static String Consulta_Ultimo_Folio(Cls_Ope_Pre_Apertura_Turno_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Variable que va a contener la consulta de los datos
                try
                {
                    //Consulta el último folio que fue registrado del último turno que fue abierto por el empleado
                    Mi_SQL.Append("SELECT * FROM (SELECT NVL(" + Ope_Caj_Turnos.Campo_Contador_Recibo + ",'')");
                    Mi_SQL.Append(" FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos);
                    Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos.Campo_Caja_Id + " = '" + Datos.P_Caja_Id + "'");
                    Mi_SQL.Append(" ORDER BY " + Ope_Caj_Turnos.Campo_No_Turno + " DESC)");
                    Mi_SQL.Append(" WHERE ROWNUM = 1");
                    return Convert.ToString(OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()));
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
            /// NOMBRE DE LA FUNCION: Consulta_Datos_Generales_Turno
            /// DESCRIPCION : Consulta todos los datos generales del turno que se encuentra
            ///               abierto por parte del empleado
            /// PARAMETROS  : Datos: Obtiene el ID del empleado a consultar
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 10-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Turnos_Cerrados(Cls_Ope_Pre_Apertura_Turno_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Consulta todos los datos del turno que se encuentra abierto por el empleado
                try
                {
                    Mi_SQL.Append("SELECT " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno + ", ");
                    Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Fecha_Turno + ", ");
                    Mi_SQL.Append(Cat_Pre_Cajas.Campo_Numero_De_Caja + ", ");
                    Mi_SQL.Append("(" + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Clave);
                    Mi_SQL.Append(" ||' '|| " + Cat_Pre_Modulos.Campo_Ubicacion + ") AS Modulo, ");
                    Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Usuario_Creo + " AS Cajero ");
                    Mi_SQL.Append(" FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + ", ");
                    Mi_SQL.Append(Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + ", " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas);
                    Mi_SQL.Append(" WHERE " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Caja_ID);
                    Mi_SQL.Append(" = " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_ID);
                    Mi_SQL.Append(" AND " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Modulo_Id);
                    Mi_SQL.Append(" = " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Modulo_Id);
                    Mi_SQL.Append(" AND " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Estatus + " = 'CERRADO'");
                    Mi_SQL.Append(" AND (" + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_ReApertura_Autorizo + " = 'NO'");
                    Mi_SQL.Append(" OR " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_ReApertura_Autorizo + " IS NULL)");
                    if (Datos.P_Modulo != "")
                    {
                        Mi_SQL.Append(" AND " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Modulo_Id + " = '" + Datos.P_Modulo + "'");
                    }
                    if (Datos.P_Caja_Id != "")
                    {
                        Mi_SQL.Append(" AND " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Caja_Id + " = '" + Datos.P_Caja_Id + "'");
                    }
                    if (!string.IsNullOrEmpty(Datos.P_Fecha_Cierre) && !string.IsNullOrEmpty(Datos.P_Fecha_Turno))
                    {
                        Mi_SQL.Append(" AND " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Fecha_Turno);
                        Mi_SQL.Append(" BETWEEN TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Cierre)) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                        Mi_SQL.Append(" AND TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Turno)) + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')");
                    }
                    Mi_SQL.Append(" ORDER BY " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Fecha_Turno + " DESC, ");
                    Mi_SQL.Append("(" + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Clave);
                    Mi_SQL.Append(" ||' '|| " + Cat_Pre_Modulos.Campo_Ubicacion + "), ");
                    Mi_SQL.Append(Cat_Pre_Cajas.Campo_Numero_De_Caja);
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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