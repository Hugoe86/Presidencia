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
using Presidencia.Constantes;
using Presidencia.Polizas.Negocios;
using Presidencia.Sessiones;
using System.Text;

namespace Presidencia.Polizas.Datos
{
    public class Cls_Ope_Con_Polizas_Datos
    {
        #region (Operaciones [Alta - Actualizar - Eliminar])
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Poliza
        /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta la Poliza en la BD con los datos proporcionados por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  :  10-Julio-2011
        /// MODIFICO          : Salvador L. Rea Ayala
        /// FECHA_MODIFICO    : 10/Octubre/2011
        /// CAUSA_MODIFICACION: Se agregaron los nuevos campos a la sentencia de insercion.
        ///*******************************************************************************
        public static string[] Alta_Poliza(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            String Mi_SQL;                          //Obtiene la cadena de inserción hacía la base de datos
            Object No_Poliza = null;                //Obtiene el No con la cual se guardo los datos en la base de datos
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmmd = new OracleCommand();
            OracleTransaction Trans = null;

            //OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            //OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            //OracleTransaction Transaccion_SQL;    //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            string[] Datos_Poliza = new string[4];
            if (Datos.P_Cmmd != null)
            {
                Cmmd = Datos.P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmmd.Connection = Trans.Connection;
                Cmmd.Transaction = Trans;
            }
            //if (Conexion_Base.State != ConnectionState.Open)
            //{
            //    Conexion_Base.Open(); //Abre la conexión a la base de datos            
            //}
            //Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            //Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            //Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                //Consulta para la obtención del último ID dado de alta 
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Con_Polizas.Campo_No_Poliza;
                Mi_SQL += "),'000000000') FROM " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas;
                Mi_SQL += " WHERE " + Ope_Con_Polizas.Campo_Mes_Ano + " = '" + Datos.P_Mes_Ano + "'";
                Mi_SQL += " AND " + Ope_Con_Polizas.Campo_Tipo_Poliza_ID + " = '" + Datos.P_Tipo_Poliza_ID+ "'";
                Mi_SQL += " ORDER BY " + Ope_Con_Polizas.Campo_No_Poliza + " DESC";

                Cmmd.CommandText = Mi_SQL; //Realiza la ejecuón de la obtención del ID del empleado
                No_Poliza = Cmmd.ExecuteScalar();

                //Valida si el ID es nulo para asignarle automaticamente el primer registro
                if (Convert.IsDBNull(No_Poliza))
                {
                    Datos.P_No_Poliza = "0000000001";
                }
                //Si no esta vacio el registro entonces al registro que se obtenga se le suma 1 para poder obtener el último registro
                else
                {
                    Datos.P_No_Poliza = String.Format("{0:0000000000}", Convert.ToInt32(No_Poliza) + 1);
                }
                //Consulta para la inserción del Empleado con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + " (";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_No_Poliza + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_Tipo_Poliza_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_Mes_Ano + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_Fecha_Poliza + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_Concepto + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_Total_Debe + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_Total_Haber + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_No_Partidas + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fecha_Creo + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_Empleado_ID_Creo + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_Empleado_ID_Autorizo + ") VALUES (";

                Mi_SQL = Mi_SQL + "'" + Datos.P_No_Poliza + "', ";

                if (Datos.P_Tipo_Poliza_ID != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Tipo_Poliza_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Mes_Ano != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Mes_Ano + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (String.Format("{0:MM/dd/yy}", Datos.P_Fecha_Poliza) != "01/01/0001")
                {
                    Mi_SQL = Mi_SQL + "TO_DATE('" + String.Format("{0:MM/dd/yy}", Datos.P_Fecha_Poliza) + "','MM/DD/YY'), ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Concepto != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Concepto + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Total_Debe > 0)
                {
                    Mi_SQL = Mi_SQL + Datos.P_Total_Debe + ", ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Total_Haber > 0)
                {
                    Mi_SQL = Mi_SQL + Datos.P_Total_Haber + ", ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_No_Partida > 0)
                {
                    Mi_SQL = Mi_SQL + Datos.P_No_Partida + ", ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Nombre_Usuario != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Nombre_Usuario + "', SYSDATE, ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Empleado_ID_Creo != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Empleado_ID_Creo + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Empleado_ID_Autorizo != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Empleado_ID_Autorizo + "')";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL)";
                }

                Cmmd.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Cmmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos  
                if (Datos.P_Cmmd == null)
                {
                    Trans.Commit();
                }
                //Transaccion_SQL.Commit(); //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos

                //Generamos los registros de alta de las percepciones y deducciones que tendrá el empleado.
                Registro_Detalles_Poliza(Datos.P_Dt_Detalles_Polizas, Datos.P_No_Poliza, Datos.P_Tipo_Poliza_ID, Datos.P_Mes_Ano, Datos.P_Fecha_Poliza,Cmmd);
                Datos_Poliza[0] = Datos.P_No_Poliza;
                Datos_Poliza[1] = Datos.P_Tipo_Poliza_ID;
                Datos_Poliza[2] = Datos.P_Mes_Ano;
                Datos_Poliza[3] = Convert.ToString(Datos.P_Fecha_Poliza);
                return Datos_Poliza;
            }
            catch (OracleException Ex)
            {
                if (Datos.P_Cmmd == null)
                {
                    Trans.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Datos.P_Cmmd == null)
                {
                    Trans.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Datos.P_Cmmd == null)
                {
                    Trans.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                if (Datos.P_Cmmd == null)
                {
                    Cn.Close();
                }

            }
        }

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Registro_Detalles_Poliza
        /// DESCRIPCION : 
        /// PARAMETROS  : Dt_Datos.- Detalles de la póliza a aplicar 
        ///               No_Poliza.- No de Póliza al que se le aplicaran los detalles
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  :  10-Julio-2011
        /// MODIFICO          : Salvador L. Rea Ayala
        /// FECHA_MODIFICO    : 10/Octubre/2011
        /// CAUSA_MODIFICACION: Se agregaron los nuevos campos a la sentencia de insercion.
        ///*******************************************************************************
        private static void Registro_Detalles_Poliza(DataTable Dt_Datos, String No_Poliza, String Tipo_Poliza_ID, String Mes_Ano, DateTime Fecha_Poliza, OracleCommand P_Cmmd)
        {
            String Mi_SQL;                                                                      //Obtiene la cadena de inserción hacía la base de datos
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmmd = new OracleCommand();
            OracleTransaction Trans = null;
            Object Consecutivo = null;                                                          //Obtiene el consecutivo con la cual se guardo los datos en la base de datos
            Object Saldo;
            if (P_Cmmd != null)
            {
                Cmmd = P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmmd.Connection = Trans.Connection;
                Cmmd.Transaction = Trans;
            }
            try
            {
                foreach (DataRow Renglon in Dt_Datos.Rows)
                {
                    Mi_SQL = "SELECT (NVL(SUM(" + Ope_Con_Polizas_Detalles.Campo_Debe + "),'0') - " +
                            " NVL(SUM(" + Ope_Con_Polizas_Detalles.Campo_Haber + "),'0')) AS Saldo" +
                            " FROM " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles +
                            " WHERE " + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + " = '" + Renglon[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID].ToString() + "'";
                    Cmmd.CommandText = Mi_SQL; //Realiza la ejecución para obtener el Saldo
                    Saldo = Cmmd.ExecuteScalar();

                    if (Convert.ToDouble(Renglon[Ope_Con_Polizas_Detalles.Campo_Debe].ToString()) > 0)
                    {
                        Saldo = Convert.ToDouble(Saldo) + Convert.ToDouble(Renglon[Ope_Con_Polizas_Detalles.Campo_Debe].ToString());
                    }
                    if (Convert.ToDouble(Renglon[Ope_Con_Polizas_Detalles.Campo_Haber].ToString()) > 0)
                    {
                        Saldo = Convert.ToDouble(Saldo) - Convert.ToDouble(Renglon[Ope_Con_Polizas_Detalles.Campo_Haber].ToString());
                    }

                    //Consulta para la obtención del último consecutivo dado de alta en la tabla de detalles de poliza
                    Mi_SQL = "SELECT NVL(MAX(" + Ope_Con_Polizas_Detalles.Campo_Consecutivo + "),'0000000000') ";
                    Mi_SQL = Mi_SQL + "FROM " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles;

                    Cmmd.CommandText = Mi_SQL; //Realiza la ejecuón de la obtención del ID del empleado
                    Consecutivo = Cmmd.ExecuteScalar();

                    //Valida si el ID es nulo para asignarle automaticamente el primer registro
                    if (Convert.IsDBNull(Consecutivo))
                    {
                        Consecutivo = "1";
                    }
                    //Si no esta vacio el registro entonces al registro que se obtenga se le suma 1 para poder obtener el último registro
                    else
                    {
                        Consecutivo = Convert.ToInt32(Consecutivo) + 1;
                    }

                    Mi_SQL = "INSERT INTO " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + " (" +
                             Ope_Con_Polizas_Detalles.Campo_No_Poliza + ", " + Ope_Con_Polizas_Detalles.Campo_Tipo_Poliza_ID + ", " +
                             Ope_Con_Polizas_Detalles.Campo_Mes_Ano + ", " + Ope_Con_Polizas_Detalles.Campo_Partida + ", " +
                             Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + ", " + Ope_Con_Polizas_Detalles.Campo_Concepto + ", " +
                             Ope_Con_Polizas_Detalles.Campo_Debe + ", " + Ope_Con_Polizas_Detalles.Campo_Haber + ", " +
                             Ope_Con_Polizas_Detalles.Campo_Saldo + ", " + Ope_Con_Polizas_Detalles.Campo_Fecha + ", " +
                             Ope_Con_Polizas_Detalles.Campo_Consecutivo + //", " + Ope_Con_Polizas_Detalles.Campo_Dependencia_ID + ", " +
                        //Ope_Con_Polizas_Detalles.Campo_Fuente_Financiamiento_ID + ", " + Ope_Con_Polizas_Detalles.Campo_Area_Funcional_ID + ", " +
                        //Ope_Con_Polizas_Detalles.Campo_Proyecto_Programa_ID + ", " + Ope_Con_Polizas_Detalles.Campo_Partida_ID +
                             ") VALUES(" + "'" + No_Poliza + "', '" + Tipo_Poliza_ID + "', '" + Mes_Ano + "', " +
                             Renglon[Ope_Con_Polizas_Detalles.Campo_Partida].ToString() + ", '" +
                             Renglon[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID].ToString() + "', '" +
                             Renglon[Ope_Con_Polizas_Detalles.Campo_Concepto].ToString() + "', " +
                             Convert.ToDouble(Renglon[Ope_Con_Polizas_Detalles.Campo_Debe].ToString()) + ", " +
                             Convert.ToDouble(Renglon[Ope_Con_Polizas_Detalles.Campo_Haber].ToString()) + ", " +
                             Convert.ToDouble(Saldo) + ", " + "TO_DATE('" + String.Format("{0:MM/dd/yy}", Fecha_Poliza) + "','MM/DD/YY'), " +
                             Consecutivo + ")";// ", " +
                    //Renglon[Ope_Con_Polizas_Detalles.Campo_Dependencia_ID].ToString() + "', '" +
                    //Renglon[Ope_Con_Polizas_Detalles.Campo_Fuente_Financiamiento_ID].ToString() + "', '" +
                    //Renglon[Ope_Con_Polizas_Detalles.Campo_Area_Funcional_ID].ToString() + "', '" +
                    //Renglon[Ope_Con_Polizas_Detalles.Campo_Proyecto_Programa_ID].ToString() + "', '" +
                    //Renglon[Ope_Con_Polizas_Detalles.Campo_Partida_ID].ToString() + "')";
                    Cmmd.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                    Cmmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos  
                }
                if (P_Cmmd == null)
                {
                    Trans.Commit();
                }
                Consulta_Saldo_y_Actualiza(Fecha_Poliza, Dt_Datos);

            }
            catch (OracleException Ex)
            {
                if (P_Cmmd == null)
                {
                    Trans.Rollback();
                }
                if (Trans != null)
                {
                    Trans.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Trans != null)
                {
                    Trans.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Trans != null)
                {
                    Trans.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                if (P_Cmmd == null)
                {
                    Cn.Close();
                }

            }
        }

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Polizas
        /// DESCRIPCION : Modifica los datos de la Póliza con los datos que fueron proporcionados
        ///               por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán modificados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 11-Julio-2011
        /// MODIFICO          : Salvador L. Rea Ayala
        /// FECHA_MODIFICO    : 10/Octubre/2011
        /// CAUSA_MODIFICACION: Se agregaron los nuevos campos a la sentencia de modificacion.
        ///*******************************************************************************
        public static void Modificar_Polizas(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos
            DataTable Ds_Consulta_Detalles; // se guardan los detalles que se eliminaran para realizar la actualizacion de los saldos
            
            try
            {
                Mi_SQL = "UPDATE " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + " SET " +
                         Ope_Con_Polizas.Campo_Tipo_Poliza_ID + " = '" + Datos.P_Tipo_Poliza_ID + "', " +
                         Ope_Con_Polizas.Campo_Mes_Ano + " = '" + Datos.P_Mes_Ano + "', " +
                         Ope_Con_Polizas.Campo_Fecha_Poliza + "= TO_DATE('" + String.Format("{0:MM/dd/yy}", Datos.P_Fecha_Poliza) + "','MM/DD/YY'), " +
                         Ope_Con_Polizas.Campo_Concepto + "='" + Datos.P_Concepto + "', " +
                         Ope_Con_Polizas.Campo_Total_Debe + " = " + Datos.P_Total_Debe + ", " +
                         Ope_Con_Polizas.Campo_Total_Haber + " = " + Datos.P_Total_Haber + ", " +
                         Ope_Con_Polizas.Campo_No_Partidas + " = " + Datos.P_No_Partida + ", " +
                         Ope_Con_Polizas.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', " +
                         Ope_Con_Polizas.Campo_Fecha_Modifico + " = SYSDATE WHERE " +
                         Ope_Con_Polizas.Campo_No_Poliza + " = '" + Datos.P_No_Poliza + "' AND " +
                         Ope_Con_Polizas.Campo_Mes_Ano + " = '" + Datos.P_Mes_Ano + "' AND " +
                         Ope_Con_Polizas.Campo_Tipo_Poliza_ID + " = '" + Datos.P_Tipo_Poliza_ID + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                //Consulta los detalles de la póliza que fueron dados de alta
                Mi_SQL = "SELECT * FROM " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles +
                         " WHERE " + Ope_Con_Polizas_Detalles.Campo_No_Poliza + " = '" + Datos.P_No_Poliza + "' AND " +
                         Ope_Con_Polizas_Detalles.Campo_Mes_Ano + " = '" + Datos.P_Mes_Ano + "' AND " +
                         Ope_Con_Polizas_Detalles.Campo_Tipo_Poliza_ID + " = '" + Datos.P_Tipo_Poliza_ID + "'";
                Ds_Consulta_Detalles=OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                foreach (DataRow Renglon in Ds_Consulta_Detalles.Rows)
                {
                    // se crea y se agregan columnas al Dt_Temporal
                    DataTable Dt_Temporal = new DataTable(); 
                    Dt_Temporal.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID, typeof(System.String));
                    Dt_Temporal.Columns.Add(Ope_Con_Polizas_Detalles.Campo_No_Poliza, typeof(System.String));
                    DataRow row;
                    // Se elimina la primera partida del detalle de la poliza
                    Mi_SQL = "DELETE FROM " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles +
                        " WHERE " + Ope_Con_Polizas_Detalles.Campo_No_Poliza + " = '" + Renglon[Ope_Con_Polizas_Detalles.Campo_No_Poliza].ToString() +
                        "' AND " + Ope_Con_Polizas_Detalles.Campo_Mes_Ano + " = '" + Renglon[Ope_Con_Polizas_Detalles.Campo_Mes_Ano].ToString() +
                        "' AND " + Ope_Con_Polizas_Detalles.Campo_Tipo_Poliza_ID + " = '" + Renglon[Ope_Con_Polizas_Detalles.Campo_Tipo_Poliza_ID ].ToString() +
                        "' AND " + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + " = '" + Renglon[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID].ToString() +
                        "' AND " + Ope_Con_Polizas_Detalles.Campo_Consecutivo + " = '" + Renglon[Ope_Con_Polizas_Detalles.Campo_Consecutivo].ToString()+"'";
                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    //se agrega una fila al Dt_Temporal
                    row = Dt_Temporal.NewRow();
                    row[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] =  Renglon[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID].ToString();
                    row[Ope_Con_Polizas_Detalles.Campo_No_Poliza] =  Renglon[Ope_Con_Polizas_Detalles.Campo_No_Poliza].ToString();
                    Dt_Temporal.Rows.Add(row);
                    Dt_Temporal.AcceptChanges();//Actualiza el Datatable
                    Consulta_Saldo_y_Actualiza(Convert.ToDateTime(Renglon[Ope_Con_Polizas_Detalles.Campo_Fecha].ToString()), Dt_Temporal);
                    Dt_Temporal=null;
                }
                Registro_Detalles_Poliza(Datos.P_Dt_Detalles_Polizas, Datos.P_No_Poliza, Datos.P_Tipo_Poliza_ID, Datos.P_Mes_Ano, Datos.P_Fecha_Poliza,null);

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
        /// NOMBRE DE LA FUNCION: Eliminar_Poliza
        /// DESCRIPCION : Elimina la Póliza y sus detalles que fue seleccionada por el usuario de la BD
        /// PARAMETROS  : Datos: Obtiene que Póliza desea eliminar de la BD
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 11-Julio-2011
        /// MODIFICO          : Salvador L. Rea Ayala
        /// FECHA_MODIFICO    : 10/Octubre/2011
        /// CAUSA_MODIFICACION: Se agregaron los nuevos parametros a la sentencia de eliminacion.
        ///*******************************************************************************
        public static void Eliminar_Poliza(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            String Mi_SQL; //Variable de Consulta para la eliminación del Empleado
            DataTable Ds_Consulta_Detalles;
            try
            {
                Mi_SQL = "SELECT * FROM " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles +
                         " WHERE " + Ope_Con_Polizas_Detalles.Campo_No_Poliza + " = '" + Datos.P_No_Poliza + "' AND " +
                         Ope_Con_Polizas_Detalles.Campo_Mes_Ano + " = '" + Datos.P_Mes_Ano + "' AND " +
                         Ope_Con_Polizas_Detalles.Campo_Tipo_Poliza_ID + " = '" + Datos.P_Tipo_Poliza_ID + "'";
                Ds_Consulta_Detalles = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                foreach (DataRow Renglon in Ds_Consulta_Detalles.Rows)
                {
                    // se crea y se agregan columnas al Dt_Temporal
                    DataTable Dt_Temporal = new DataTable();
                    Dt_Temporal.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID, typeof(System.String));
                    Dt_Temporal.Columns.Add(Ope_Con_Polizas_Detalles.Campo_No_Poliza, typeof(System.String));
                    DataRow row;
                    // Se elimina la primera partida del detalle de la poliza
                    Mi_SQL = "DELETE FROM " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles +
                        " WHERE " + Ope_Con_Polizas_Detalles.Campo_No_Poliza + " = '" + Renglon[Ope_Con_Polizas_Detalles.Campo_No_Poliza].ToString() +
                        "' AND " + Ope_Con_Polizas_Detalles.Campo_Mes_Ano + " = '" + Renglon[Ope_Con_Polizas_Detalles.Campo_Mes_Ano].ToString() +
                        "' AND " + Ope_Con_Polizas_Detalles.Campo_Tipo_Poliza_ID + " = '" + Renglon[Ope_Con_Polizas_Detalles.Campo_Tipo_Poliza_ID].ToString() +
                        "' AND " + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + " = '" + Renglon[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID].ToString() +
                        "' AND " + Ope_Con_Polizas_Detalles.Campo_Consecutivo + " = '" + Renglon[Ope_Con_Polizas_Detalles.Campo_Consecutivo].ToString() + "'";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    //se agrega una fila al Dt_Temporal
                    row = Dt_Temporal.NewRow();
                    row[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = Renglon[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID].ToString();
                    row[Ope_Con_Polizas_Detalles.Campo_No_Poliza] = Renglon[Ope_Con_Polizas_Detalles.Campo_No_Poliza].ToString();
                    Dt_Temporal.Rows.Add(row);
                    Dt_Temporal.AcceptChanges();//Actualiza el Datatable
                    Consulta_Saldo_y_Actualiza(Convert.ToDateTime(Renglon[Ope_Con_Polizas_Detalles.Campo_Fecha].ToString()), Dt_Temporal);
                    Dt_Temporal = null;
                }
                //Elimina los datos generales de la póliza
                Mi_SQL = "DELETE FROM " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas;
                Mi_SQL += " WHERE " + Ope_Con_Polizas.Campo_No_Poliza + " = '" + Datos.P_No_Poliza + "'";
                Mi_SQL += " AND " + Ope_Con_Polizas.Campo_Mes_Ano + " = '" + Datos.P_Mes_Ano + "'";
                Mi_SQL += " AND " + Ope_Con_Polizas_Detalles.Campo_Tipo_Poliza_ID + " = '" + Datos.P_Tipo_Poliza_ID + "'";
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
        /// NOMBRE DE LA FUNCION: Consulta_Saldo_y_Actualiza
        /// DESCRIPCION : Consulta el ultimo saldo y lo actualiza 
        /// PARAMETROS  : Fecha de tipo DATETIME y DataTable con las partidas de la poliza 
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 29/Noviembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    : 
        /// CAUSA_MODIFICACION: 
        ///*******************************************************************************
        public static void Consulta_Saldo_y_Actualiza(DateTime Fecha_Poliza, DataTable Dt_Datos)
        {
            String Mi_SQL;                          //Obtiene la cadena de inserción hacía la base de datos
            String Fecha;
            String Fecha_Saldo;
            String Cuenta_Contable_ID;
            DataTable Saldo;
            Double Saldo2;
            int contador;
            int Bandera;
            try
            {
                foreach (DataRow Renglon in Dt_Datos.Rows)
                {
                    Cuenta_Contable_ID = Renglon[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID].ToString();
                    Bandera = 0;
                    Saldo2 = 0;
                    contador = 0;
                    //se le resta a la fecha un dia 
                    Fecha = string.Format("{0:dd/MM/yyyy}", Fecha_Poliza.AddDays(-1));
                    //Obtiene el ultimo movimiento de acuerdo a la fecha
                    Mi_SQL = "SELECT MAX(" + Ope_Con_Polizas_Detalles.Campo_Fecha + ")";
                    Mi_SQL += "FROM " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles;
                    Mi_SQL += " WHERE " + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + "='" + Cuenta_Contable_ID + "' ";
                    Mi_SQL += " AND TO_DATE(TO_CHAR(" + Ope_Con_Polizas_Detalles.Campo_Fecha + ",'DD/MM/YY'))" + " <= '" + Fecha + "'";
                    Fecha_Saldo = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).ToString();
                    if (Fecha_Saldo != "" && Fecha_Saldo != null)
                    {
                        Fecha_Saldo = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Fecha_Saldo));
                        Mi_SQL = "SELECT " + Ope_Con_Polizas_Detalles.Campo_Saldo + ", " + Ope_Con_Polizas_Detalles.Campo_Partida;
                        Mi_SQL += " FROM " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + " WHERE TO_DATE(TO_CHAR(" + Ope_Con_Polizas_Detalles.Campo_Fecha +",'DD-MM-YYYY')) ='" + Fecha_Saldo + "'";
                        Mi_SQL += " AND " + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + "='" + Cuenta_Contable_ID + "' ORDER BY " + Ope_Con_Polizas_Detalles.Campo_Fecha + ", " + Ope_Con_Polizas_Detalles.Campo_Consecutivo + ", ";
                        Mi_SQL += Ope_Con_Polizas_Detalles.Campo_Partida;
                        Saldo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                        if (Saldo.Rows.Count > 0)
                        {
                            Saldo2 = Convert.ToDouble(Saldo.Rows[0][Ope_Con_Polizas_Detalles.Campo_Saldo].ToString());
                        }
                    }
                    else
                    {
                        Saldo2 = 0;
                        Bandera = 1;
                        Fecha_Saldo = Fecha;
                    }
                    //Actualiza saldos
                    Mi_SQL = "SELECT * FROM " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles;
                    Mi_SQL += " WHERE " + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + "='" + Cuenta_Contable_ID + "' ";
                    Mi_SQL += " AND TO_DATE(TO_CHAR(" + Ope_Con_Polizas_Detalles.Campo_Fecha + ",'DD-MM-YYYY'))" + " >= '" + Fecha_Saldo + "'";
                    Mi_SQL += " ORDER BY " + Ope_Con_Polizas_Detalles.Campo_Fecha + ", " + Ope_Con_Polizas_Detalles.Campo_Consecutivo + ", " + Ope_Con_Polizas_Detalles.Campo_Partida;
                    Saldo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    if (Saldo2 == 0 && Bandera == 1 && Saldo.Rows.Count > 0)
                    {
                        Saldo2 = Convert.ToDouble(Saldo.Rows[0][Ope_Con_Polizas_Detalles.Campo_Debe].ToString()) - Convert.ToDouble(Saldo.Rows[0][Ope_Con_Polizas_Detalles.Campo_Haber].ToString());
                    }
                    if (Saldo.Rows.Count > 0)
                    {
                        while (contador < Saldo.Rows.Count)
                        {
                            if (contador > 0)
                            {
                                Saldo2 = Saldo2 + Convert.ToDouble(Saldo.Rows[contador][Ope_Con_Polizas_Detalles.Campo_Debe].ToString()) - Convert.ToDouble(Saldo.Rows[contador][Ope_Con_Polizas_Detalles.Campo_Haber].ToString());
                            }
                            Mi_SQL = "UPDATE " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles;
                            Mi_SQL += " SET " + Ope_Con_Polizas_Detalles.Campo_Saldo + " = " + Saldo2;
                            Mi_SQL += " WHERE " + Ope_Con_Polizas_Detalles.Campo_Consecutivo + " = " + Saldo.Rows[contador][Ope_Con_Polizas_Detalles.Campo_Consecutivo].ToString();
                            Mi_SQL += " AND " + Ope_Con_Polizas_Detalles.Campo_Partida + " = " + Saldo.Rows[contador][Ope_Con_Polizas_Detalles.Campo_Partida].ToString();
                            Mi_SQL += " AND " + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + "='" + Saldo.Rows[contador][Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID].ToString() + "'";
                            //Manda Mi_SQL para ser procesada por ORACLE.
                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                            contador = contador + 1;
                        }
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
        }
        #endregion

        #region (Consulta)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Poliza
        /// DESCRIPCION : Consulta todas las pólizas que coincidan con lo proporcionado
        ///               por el usuario
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 11-Julio-2011
        /// MODIFICO          : Salvador L. Rea Ayala
        /// FECHA_MODIFICO    : 10/Octubre/2011
        /// CAUSA_MODIFICACION: Se agregaron nuevos filtros para la consulta.
        ///*******************************************************************************
        public static DataTable Consulta_Poliza(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta para la póliza
            Int32 No_Poliza;
            try
            {
                No_Poliza = Convert.ToInt32(Datos.P_No_Poliza);
                Datos.P_No_Poliza = String.Format("{0:0000000000}", No_Poliza);
                //Consulta todas las Polizas que coincidan con lo proporcionado por el usuario
                Mi_SQL = "SELECT * FROM " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas;
                Mi_SQL += " WHERE " + Ope_Con_Polizas.Campo_No_Poliza + " = '" + Datos.P_No_Poliza + "'";
                if (!string.IsNullOrEmpty(Datos.P_Tipo_Poliza_ID))
                {
                    Mi_SQL += " AND " + Ope_Con_Polizas.Campo_Tipo_Poliza_ID + " = '" + Datos.P_Tipo_Poliza_ID+ "'";
                }
                if(!string.IsNullOrEmpty(Datos.P_Concepto))
                {
                    Mi_SQL += " AND " + Ope_Con_Polizas.Campo_Concepto + " = '" + Datos.P_Concepto + "'";
                }
                Mi_SQL += " ORDER BY " + Ope_Con_Polizas.Campo_No_Poliza;

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
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Poliza_Popup
        /// DESCRIPCION : Consulta todas las pólizas que coincidan con lo proporcionado
        ///               por el usuario
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 22/Septiembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Poliza_Popup(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta para la póliza
            Int32 No_Poliza;
            Boolean Primer_Where = true;
            try
            {
                //Consulta todas las Polizas que coincidan con lo proporcionado por el usuario
                Mi_SQL = "SELECT " + Ope_Con_Polizas.Campo_No_Poliza + ", poliza.";
                Mi_SQL += Ope_Con_Polizas.Campo_Tipo_Poliza_ID + ", ";
                Mi_SQL += Cat_Con_Tipo_Polizas.Campo_Descripcion + ",";
                Mi_SQL += Ope_Con_Polizas.Campo_Fecha_Poliza + ", ";
                Mi_SQL += Ope_Con_Polizas.Campo_Concepto + " FROM ";
                Mi_SQL += Cat_Con_Tipo_Polizas.Tabla_Cat_Con_Tipo_Polizas +" tipo, ";
                Mi_SQL += Ope_Con_Polizas.Tabla_Ope_Con_Polizas + " poliza WHERE ";
                Mi_SQL += " tipo." + Cat_Con_Tipo_Polizas.Campo_Tipo_Poliza_ID + " = poliza." + Ope_Con_Polizas.Campo_Tipo_Poliza_ID + " AND ";

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Poliza_ID))
                {
                    Mi_SQL +=" tipo." +Ope_Con_Polizas.Campo_Tipo_Poliza_ID + " = " + Datos.P_Tipo_Poliza_ID;
                    Primer_Where = false;
                }
                if (!string.IsNullOrEmpty(Datos.P_Mes_Ano))
                {
                    if (Primer_Where == true)
                    {
                        Mi_SQL += Ope_Con_Polizas.Campo_Mes_Ano + " = " + Datos.P_Mes_Ano;
                    }
                    else
                        Mi_SQL += " AND " + Ope_Con_Polizas.Campo_Mes_Ano + " = " + Datos.P_Mes_Ano;
                }
                if (!string.IsNullOrEmpty(Datos.P_No_Poliza))
                {
                    No_Poliza = Convert.ToInt32(Datos.P_No_Poliza);
                    Datos.P_No_Poliza = String.Format("{0:0000000000}", No_Poliza);
                    if (Primer_Where == true)
                    {
                        Mi_SQL += Ope_Con_Polizas.Campo_No_Poliza + " = " + Datos.P_No_Poliza;
                    }
                    else
                        Mi_SQL += " AND " + Ope_Con_Polizas.Campo_No_Poliza + " = " + Datos.P_No_Poliza;
                }
                Mi_SQL += " ORDER BY " + Ope_Con_Polizas.Campo_No_Poliza + " ASC";

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
        /// NOMBRE DE LA FUNCION: Consulta_Detalles_Poliza
        /// DESCRIPCION : Consulta todos los detalles de la póliza
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 11-Julio-2011
        /// MODIFICO          : Salvador L. Rea Ayala
        /// FECHA_MODIFICO    : 10/Octubre/2011
        /// CAUSA_MODIFICACION: Se agregaron nuevos filtros para la consulta.
        ///*******************************************************************************
        public static DataTable Consulta_Detalles_Poliza(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consult del detalle de la póliza
            Int32 No_Poliza;
            try
            {
                No_Poliza = Convert.ToInt32(Datos.P_No_Poliza);
                Datos.P_No_Poliza = String.Format("{0:0000000000}", No_Poliza);
                //Consulta los detalles de la póliza
                Mi_SQL = "SELECT " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + ".*, " +
                       Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta +
                       " FROM " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + ", " +
                       Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables +
                       " WHERE " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + " = " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID +
                       " AND " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_No_Poliza + " = '" + Datos.P_No_Poliza + "'" +
                       " AND " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Tipo_Poliza_ID + " = '" + Datos.P_Tipo_Poliza_ID + "'" +
                       " ORDER BY " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Partida;
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
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Detalles_Poliza_Cuenta_Contable
        /// DESCRIPCION : Consulta todos los detalles de la póliza de acuerdo a un numero de cuenta
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 26/Octubre/2011
        /// MODIFICO          : 
        /// FECHA_MODIFICO    : 
        /// CAUSA_MODIFICACION: 
        ///*******************************************************************************
        public static DataTable Consulta_Detalles_Poliza_Cuenta_Contable(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consult del detalle de la póliza
            Int32 No_Poliza;
            try
            {
                No_Poliza = Convert.ToInt32(Datos.P_No_Poliza);
                Datos.P_No_Poliza = String.Format("{0:0000000000}", No_Poliza);
                //Consulta los detalles de la póliza
                Mi_SQL = "SELECT " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + ".*" +
                         " FROM " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles +
                         " WHERE " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + " = '" + Datos.P_Cuenta_Contable_ID + "'" +
                         " AND (" + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Mes_Ano + " >= '" + Datos.P_Mes_Inicio + "' AND " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Mes_Ano + " <= '" + Datos.P_Mes_Fin + "')" +
                         " ORDER BY " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Partida;
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
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Detalles_Poliza_Por_Referencia
        /// DESCRIPCION : Consulta todos los detalles de la póliza de acuerdo a un numero de cuenta
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 26/Octubre/2011
        /// MODIFICO          : 
        /// FECHA_MODIFICO    : 
        /// CAUSA_MODIFICACION: 
        ///*******************************************************************************
        public static DataTable Consulta_Detalles_Poliza_Por_Referencia(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consult del detalle de la póliza
            try
            {
                //Consulta los detalles de la póliza
                Mi_SQL = "SELECT " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + ".*," +Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables+"."+Cat_Con_Cuentas_Contables.Campo_Cuenta+
                         " FROM " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles;
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + " ON " +Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables +"."+Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID +"="+
                        Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles +"."+ Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID+
                         " WHERE " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + " = '" + Datos.P_Cuenta_Contable_ID + "'" ;
                         if(!String.IsNullOrEmpty(Datos.P_Referencia)){
                             Mi_SQL = Mi_SQL + " AND " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Referencia + " = '" + Datos.P_Referencia + "'";
                         }
                Mi_SQL = Mi_SQL + " AND " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Fecha + " " +
                        " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicial + " 00:00:00', 'MM-DD-YYYY HH24:MI:SS') AND TO_DATE ('" + Datos.P_Fecha_Final + " 23:59:59', 'MM-DD-YYYY HH24:MI:SS')"+
                         " ORDER BY " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Partida;
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
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Detalles_Poliza_Seleccionada
        /// DESCRIPCION : Consulta los datos sobre la poliza seleccionada
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 26/Septiembre/2011
        /// MODIFICO          : Salvador L. Rea Ayala
        /// FECHA_MODIFICO    : 10/Octubre/2011
        /// CAUSA_MODIFICACION: Se agregaron nuevos filtros para la consulta.
        ///*******************************************************************************
        public static DataTable Consulta_Detalles_Poliza_Seleccionada(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consult del detalle de la póliza
            Int32 No_Poliza;
            try
            {
                No_Poliza = Convert.ToInt32(Datos.P_No_Poliza);
                Datos.P_No_Poliza = String.Format("{0:0000000000}", No_Poliza);
                //Consulta los detalles de la póliza
                Mi_SQL = "SELECT ROWNUM AS PARTIDA, " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + ", ";
                Mi_SQL += Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta + " AS CUENTA, ";
                Mi_SQL += Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Concepto + ", ";
                Mi_SQL += Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Debe + ", ";
                Mi_SQL += Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Haber + ", ";
                Mi_SQL += Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Dependencia_ID + ", ";
                Mi_SQL += Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Proyecto_Programa_ID + ", ";
                Mi_SQL += Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Fuente_Financiamiento_ID + ", ";
                Mi_SQL += Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Partida_ID;

                //Mi_SQL += "((SELECT " + Cat_Dependencias.Campo_Clave + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "=" + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Dependencia_ID + ") ||'-'||";
                //Mi_SQL += " (SELECT " + Cat_Com_Proyectos_Programas.Campo_Clave + " FROM " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + " WHERE " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + "." + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + "=" + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Proyecto_Programa_ID + ") ||'-'||";
                //Mi_SQL += " (SELECT " + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + " WHERE " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + "=" + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Fuente_Financiamiento_ID + ") ||'-'||";
                //Mi_SQL += " (SELECT " + Cat_SAP_Area_Funcional.Campo_Clave + " FROM " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + " WHERE " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID + "=" + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Area_Funcional_ID + ") ||'-'||";
                //Mi_SQL += " (SELECT " + Cat_Sap_Partidas_Especificas.Campo_Clave + " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " WHERE " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + "=" + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Partida_ID + ")) AS CODIGO_PROGRAMATICO ";

                Mi_SQL += " FROM " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + " LEFT OUTER JOIN " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables;
                Mi_SQL += " ON " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + " = " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID;
                Mi_SQL += " WHERE " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_No_Poliza + " = '" + Datos.P_No_Poliza + "'";
                Mi_SQL += " AND " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Mes_Ano + " = '" + Datos.P_Mes_Ano + "'";
                Mi_SQL += " AND " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Tipo_Poliza_ID + " = '" + Datos.P_Tipo_Poliza_ID + "'";
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
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Detalles_Empleados
        /// DESCRIPCION : Consulta los datos sobre la poliza seleccionada
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 7/Octubre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Detalles_Empleado_Aprobo(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consult del detalle de la póliza
            try
            {
                Mi_SQL = "SELECT " + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" + Cat_Empleados.Campo_Apellido_Materno + "||' '||" + Cat_Empleados.Campo_Nombre + " AS EMPLEADO_AUTORIZO, ";
                Mi_SQL += Cat_Empleados.Campo_Empleado_ID + ", " + Cat_Empleados.Campo_No_Empleado;
                Mi_SQL += " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " RIGHT OUTER JOIN " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + " ON ";
                Mi_SQL += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " = " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Empleado_ID_Autorizo;
                Mi_SQL += " WHERE " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_No_Poliza + " = '" + Datos.P_No_Poliza + "'";
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Detalles_Empleado_Creo
        /// DESCRIPCION : Consulta los datos sobre la poliza seleccionada
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 7/Octubre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Detalles_Empleado_Creo(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consult del detalle de la póliza
            try
            {
                Mi_SQL = "SELECT " + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" + Cat_Empleados.Campo_Apellido_Materno + "||' '||" + Cat_Empleados.Campo_Nombre + " AS EMPLEADO_CREO, ";
                Mi_SQL += Cat_Empleados.Campo_Empleado_ID + ", " + Cat_Empleados.Campo_No_Empleado;
                Mi_SQL += " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " RIGHT OUTER JOIN " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + " ON ";
                Mi_SQL += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " = " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Empleado_ID_Creo;
                Mi_SQL += " WHERE " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_No_Poliza + " = '" + Datos.P_No_Poliza + "'";
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Fecha_Poliza
        /// DESCRIPCION : Consulta la fecha de la poliza a copiar
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 26/Septiembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Consulta_Fecha_Poliza(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta la fecha de la póliza
            DataTable Dt_Fecha_Poliza;
            string Fecha_Original;
            try
            {
                //Consulta los detalles de la póliza
                Mi_SQL = "SELECT " + Ope_Con_Polizas_Detalles.Campo_Fecha;
                Mi_SQL += " FROM " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles;
                Mi_SQL += " WHERE " + Ope_Con_Polizas_Detalles.Campo_No_Poliza + " = " + Datos.P_No_Poliza;
                Dt_Fecha_Poliza = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                Fecha_Original = Dt_Fecha_Poliza.Rows[0][0].ToString();
                Fecha_Original = String.Format("{0:MM/dd/yy}", Convert.ToDateTime(Fecha_Original));

                if (String.Format("{0:MM/dd/yy}", Convert.ToDateTime(Datos.P_Fecha_Poliza)) == Fecha_Original)
                    return true;
                else
                    return false;

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
        /// NOMBRE DE LA FUNCION: Consulta_Detalle_Poliza
        /// DESCRIPCION : Consulta todos los datos de la poliza a generar
        ///               el usuario
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 12-Enero-2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Detalle_Poliza(Cls_Ope_Con_Polizas_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la consulta a realizar a la base de datos             
                try
                {
                    Mi_SQL.Append("SELECT POLIZA." + Ope_Con_Polizas.Campo_No_Poliza + ", POLIZA.");
                    Mi_SQL.Append(Ope_Con_Polizas.Campo_Mes_Ano + ", POLIZA." + Ope_Con_Polizas.Campo_Tipo_Poliza_ID + ", POLIZA.");
                    Mi_SQL.Append(Ope_Con_Polizas.Campo_No_Partidas + ", POLIZA." + Ope_Con_Polizas.Campo_Concepto + ", POLIZA." + Ope_Con_Polizas.Campo_Fecha_Poliza + ", POLIZA.");
                    Mi_SQL.Append(Ope_Con_Polizas.Campo_Total_Haber + ", POLIZA." + Ope_Con_Polizas.Campo_Total_Debe + " , DETALLES.");
                    Mi_SQL.Append(Ope_Con_Polizas_Detalles.Campo_Partida + ", DETALLES." + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + ", DETALLES.");
                    Mi_SQL.Append(Ope_Con_Polizas_Detalles.Campo_Concepto + " AS CONCEPTO_PARTIDA , DETALLES."+Ope_Con_Polizas_Detalles.Campo_Referencia+", DETALLES.");
                    Mi_SQL.Append(Ope_Con_Polizas_Detalles.Campo_Debe + ", DETALLES." + Ope_Con_Polizas_Detalles.Campo_Haber + ", CUENTA.");
                    Mi_SQL.Append(Cat_Con_Cuentas_Contables.Campo_Cuenta + ", TIPO." + Cat_Con_Tipo_Polizas.Campo_Descripcion + " AS TIPO_POLIZA ,CUENTA." + Cat_Con_Cuentas_Contables.Campo_Descripcion);
                    Mi_SQL.Append(",(select " + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " from " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento +
                                    " where " + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + "=DETALLES." + Ope_Con_Polizas_Detalles.Campo_Fuente_Financiamiento_ID + ")");
                    Mi_SQL.Append(" ||'-'|| (select " + Cat_SAP_Area_Funcional.Campo_Clave + " from " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional +
                                    " where " + Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID + "=DETALLES." + Ope_Con_Polizas_Detalles.Campo_Area_Funcional_ID + ")");
                    Mi_SQL.Append(" ||'-'|| (select " + Cat_Sap_Proyectos_Programas.Campo_Clave + " from " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas +
                                    " where " + Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id + "=DETALLES." + Ope_Con_Polizas_Detalles.Campo_Proyecto_Programa_ID + ")");
                    Mi_SQL.Append(" ||'-'|| (select " + Cat_Dependencias.Campo_Clave + " from " + Cat_Dependencias.Tabla_Cat_Dependencias +
                                    " where " + Cat_Dependencias.Campo_Dependencia_ID + "=DETALLES." + Ope_Con_Polizas_Detalles.Campo_Dependencia_ID + ")");
                    Mi_SQL.Append(" ||'-'|| (select " + Cat_Sap_Partidas_Especificas.Campo_Clave + " from " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas +
                                    " where " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + "=DETALLES." + Ope_Con_Polizas_Detalles.Campo_Partida_ID + ") as CODIGO_PROGRAMATICO, ' ' as BENEFICIARIO");
                    Mi_SQL.Append(", PAGOS." + Ope_Con_Pagos.Campo_Beneficiario_Pago + " AS BENEFICIARIO_PAGO");
                    Mi_SQL.Append(" FROM " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + " POLIZA");
                    Mi_SQL.Append(" LEFT OUTER JOIN " + Ope_Con_Pagos.Tabla_Ope_Con_Pagos + " PAGOS ON PAGOS." + Ope_Con_Pagos.Campo_No_poliza + " =POLIZA." + Ope_Con_Polizas.Campo_No_Poliza + " AND ");
                    Mi_SQL.Append(" PAGOS." + Ope_Con_Pagos.Campo_Tipo_Poliza_ID + " =POLIZA." + Ope_Con_Polizas.Campo_Tipo_Poliza_ID + " AND PAGOS." + Ope_Con_Pagos.Campo_Mes_Ano + " = POLIZA." + Ope_Con_Polizas.Campo_Mes_Ano); ;
                    Mi_SQL.Append(", " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + " DETALLES, ");
                    Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + " CUENTA, " + Cat_Con_Tipo_Polizas.Tabla_Cat_Con_Tipo_Polizas + " TIPO ");
                    Mi_SQL.Append(" WHERE POLIZA." + Ope_Con_Polizas.Campo_No_Poliza + "= DETALLES." + Ope_Con_Polizas_Detalles.Campo_No_Poliza + " AND POLIZA.");
                    Mi_SQL.Append(Ope_Con_Polizas.Campo_Tipo_Poliza_ID + "= DETALLES." + Ope_Con_Polizas_Detalles.Campo_Tipo_Poliza_ID + " AND POLIZA." + Ope_Con_Polizas.Campo_Mes_Ano + "= DETALLES." + Ope_Con_Polizas_Detalles.Campo_Mes_Ano + " AND poliza.");
                    Mi_SQL.Append(Ope_Con_Polizas.Campo_Tipo_Poliza_ID + "= TIPO." + Cat_Con_Tipo_Polizas.Campo_Tipo_Poliza_ID + " AND DETALLES." + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + "= CUENTA.");
                    Mi_SQL.Append(Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + " AND POLIZA." + Ope_Con_Polizas.Campo_No_Poliza + "='" + Datos.P_No_Poliza + "' AND POLIZA.");
                    Mi_SQL.Append(Ope_Con_Polizas.Campo_Tipo_Poliza_ID + " ='" + Datos.P_Tipo_Poliza_ID + "' AND poliza." + Ope_Con_Polizas.Campo_Mes_Ano + "='" + Datos.P_Mes_Ano + "'");

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
        ///NOMBRE DE LA FUNCIÓN : Consultar_Fuente_Financiamiento
        ///DESCRIPCIÓN          : consulta para obtener los datos de las fuentes de financiamiento
        ///PARAMETROS           : 
        ///CREO                 : Leslie Gonzalez Vázquez
        ///FECHA_CREO           : 30/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
         public static DataTable Consulta_Fuente_Financiamiento(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                //OBTENEMOS LAS DEPENDENCIAS DEL CATALOGO
                Mi_Sql.Append("SELECT DISTINCT " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " || ' ' || ");
                Mi_Sql.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + " AS CLAVE_NOMBRE, ");
                Mi_Sql.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID);
                Mi_Sql.Append(" FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento);
                Mi_Sql.Append(" INNER JOIN " + Cat_Sap_Det_Fte_Concepto.Tabla_Cat_Sap_Det_Fte_Concepto);
                Mi_Sql.Append(" ON " + Cat_Sap_Det_Fte_Concepto.Tabla_Cat_Sap_Det_Fte_Concepto + "." + Cat_Sap_Det_Fte_Concepto.Campo_Fuente_Financiamiento_ID);
                Mi_Sql.Append(" = " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID);
                Mi_Sql.Append(" INNER JOIN " + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing);
                Mi_Sql.Append(" ON " + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + "." + Cat_Psp_Concepto_Ing.Campo_Concepto_Ing_ID);
                Mi_Sql.Append(" = " + Cat_Sap_Det_Fte_Concepto.Tabla_Cat_Sap_Det_Fte_Concepto + "." + Cat_Sap_Det_Fte_Concepto.Campo_Concepto_Ing_ID);
                Mi_Sql.Append(" INNER JOIN " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables);
                Mi_Sql.Append(" ON " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID);
                Mi_Sql.Append(" = " + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + "." + Cat_Psp_Concepto_Ing.Campo_Cuenta_Contable_ID);
                Mi_Sql.Append(" WHERE " + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + "." + Cat_Psp_Concepto_Ing.Campo_Anio);
                Mi_Sql.Append(" = " + String.Format("{0:yyyy}", DateTime.Now) + " AND " + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + "." + Cat_Psp_Concepto_Ing.Campo_Estatus +" ='ACTIVO' ");
                if( !String.IsNullOrEmpty(Datos.P_Partida_ID)){
                    Mi_Sql.Append(" AND " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Partida_ID + " = '" + Datos.P_Partida_ID + "'");
                }
                Mi_Sql.Append(" ORDER BY CLAVE_NOMBRE ASC");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros de las fuentes de financiamiento. Error: [" + Ex.Message + "]");
            }
        }
         ///*******************************************************************************
         ///NOMBRE DE LA FUNCIÓN : Consulta_Fuente_Financiamiento_Egr
         ///DESCRIPCIÓN          : consulta para obtener los datos de las fuentes de financiamiento DE EGRESOS
         ///PARAMETROS           : 
         ///CREO                 : SERGIO MANEUL GALLARDO ANDRADE
         ///FECHA_CREO           : 20/ABRIL/2012
         ///MODIFICO             :
         ///FECHA_MODIFICO       :
         ///CAUSA_MODIFICACIÓN   :
         ///*******************************************************************************
         public static DataTable Consulta_Fuente_Financiamiento_Egr(Cls_Ope_Con_Polizas_Negocio Datos)
         {
             StringBuilder Mi_Sql = new StringBuilder();
             try
             {
                 //OBTENEMOS LAS FUENTES DEL CATALOGO
                 Mi_Sql.Append("SELECT DISTINCT " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " || ' ' || ");
                 Mi_Sql.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + " AS CLAVE_NOMBRE, ");
                 Mi_Sql.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID);
                 Mi_Sql.Append(" FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento);
                 Mi_Sql.Append(" INNER JOIN " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado);
                 Mi_Sql.Append(" ON " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID);
                 Mi_Sql.Append(" = " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID);
                 Mi_Sql.Append(" INNER JOIN " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables);
                 Mi_Sql.Append(" ON " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Partida_ID);
                 Mi_Sql.Append(" = " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID);
                 Mi_Sql.Append(" WHERE " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Anio);
                 Mi_Sql.Append(" = " + String.Format("{0:yyyy}", DateTime.Now));
                 if (!String.IsNullOrEmpty(Datos.P_Partida_ID))
                 {
                     Mi_Sql.Append(" AND " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Partida_ID + " = '" + Datos.P_Partida_ID + "'");
                 }
                 Mi_Sql.Append(" ORDER BY CLAVE_NOMBRE ASC");

                 return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
             }
             catch (Exception Ex)
             {
                 throw new Exception("Error al intentar consultar los registros de las fuentes de financiamiento. Error: [" + Ex.Message + "]");
             }
         }
         ///*******************************************************************************
         ///NOMBRE DE LA FUNCIÓN : Consulta_Dependencia
         ///DESCRIPCIÓN          : consulta para obtener los datos de las dependencias de la fuente de financiamiento seleccionada
         ///PARAMETROS           : 
         ///CREO                 : SERGIO MANEUL GALLARDO ANDRADE
         ///FECHA_CREO           : 20/ABRIL/2012
         ///MODIFICO             :
         ///FECHA_MODIFICO       :
         ///CAUSA_MODIFICACIÓN   :
         ///*******************************************************************************
         public static DataTable Consulta_Dependencia(Cls_Ope_Con_Polizas_Negocio Datos)
         {
             StringBuilder Mi_Sql = new StringBuilder();
             try
             {
                 //OBTENEMOS LAS DEPENDENCIAS DEL CATALOGO
                 Mi_Sql.Append("SELECT  DISTINCT " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " ||' '|| ");
                 Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS CLAVE_NOMBRE, ");
                 Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " AS DEPENDENCIA ");
                 Mi_Sql.Append(" FROM " + Cat_Dependencias.Tabla_Cat_Dependencias);
                 Mi_Sql.Append(" INNER JOIN " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado);
                 Mi_Sql.Append(" ON " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID);
                 Mi_Sql.Append(" = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                 Mi_Sql.Append(" INNER JOIN " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables);
                 Mi_Sql.Append(" ON " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Partida_ID);
                 Mi_Sql.Append(" = " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID);
                 Mi_Sql.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Anio);
                 Mi_Sql.Append(" = '" +  String.Format("{0:yyyy}", DateTime.Now)+ "'");
                 
                 if (!String.IsNullOrEmpty(Datos.P_Fuente_Financiamiento_ID))
                 {
                     Mi_Sql.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID);
                     Mi_Sql.Append(" = '" + Datos.P_Fuente_Financiamiento_ID + "'");
                 }
                 if (!String.IsNullOrEmpty(Datos.P_Partida_ID))
                 {
                     Mi_Sql.Append(" AND " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Partida_ID + " = '" + Datos.P_Partida_ID + "'");
                 }
                 if (!String.IsNullOrEmpty(Datos.P_Clave_Dependencia))
                 {
                     Mi_Sql.Append(" AND " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " = '" + Datos.P_Clave_Dependencia + "'");
                 }
                 Mi_Sql.Append(" ORDER BY CLAVE_NOMBRE ASC");

                 return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
             }
             catch (Exception Ex)
             {
                 throw new Exception("Error al intentar consultar los registros de las fuentes de financiamiento. Error: [" + Ex.Message + "]");
             }
         }  
        ///*******************************************************************************
         ///NOMBRE DE LA FUNCIÓN : Consulta_Programas
         ///DESCRIPCIÓN          : consulta para obtener los programas de la dependencia seleccionada
         ///PARAMETROS           : 
         ///CREO                 : SERGIO MANEUL GALLARDO ANDRADE
         ///FECHA_CREO           : 20/ABRIL/2012
         ///MODIFICO             :
         ///FECHA_MODIFICO       :
         ///CAUSA_MODIFICACIÓN   :
         ///*******************************************************************************
         public static DataTable Consulta_Programas(Cls_Ope_Con_Polizas_Negocio Datos)
         {
             StringBuilder Mi_Sql = new StringBuilder();
             try
             {
                 //OBTENEMOS LAS DEPENDENCIAS DEL CATALOGO
                 Mi_Sql.Append("SELECT DISTINCT " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Clave + " ||' '|| ");
                 Mi_Sql.Append(Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Nombre + " AS CLAVE_NOMBRE, ");
                 Mi_Sql.Append(Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id + " AS PROGRAMA_ID ");
                 if (!String.IsNullOrEmpty(Datos.P_Momento))
                 {
                     Mi_Sql.Append(", "+Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "."+Datos.P_Momento+" AS MOMENTO");
                 }
                 if (!String.IsNullOrEmpty(Datos.P_Momento_Final))
                 {
                     Mi_Sql.Append(", " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Datos.P_Momento_Final + " AS MOMENTO_FINAL");
                 }
                 Mi_Sql.Append(" FROM " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas);
                 Mi_Sql.Append(" INNER JOIN " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado);
                 Mi_Sql.Append(" ON " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID);
                 Mi_Sql.Append(" = " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id);
                 Mi_Sql.Append(" INNER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias);
                 Mi_Sql.Append(" ON " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID);
                 Mi_Sql.Append(" = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                 Mi_Sql.Append(" INNER JOIN " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables);
                 Mi_Sql.Append(" ON " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Partida_ID);
                 Mi_Sql.Append(" = " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID);
                 Mi_Sql.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Anio);
                 Mi_Sql.Append(" = '" + String.Format("{0:yyyy}", DateTime.Now) + "'");
                 if (!String.IsNullOrEmpty(Datos.P_Dependencia_ID))
                 {
                     Mi_Sql.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID);
                     Mi_Sql.Append(" = '" + Datos.P_Dependencia_ID + "'");
                 }
                 if (!String.IsNullOrEmpty(Datos.P_Fuente_Financiamiento_ID))
                 {
                     Mi_Sql.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID);
                     Mi_Sql.Append(" = '" + Datos.P_Fuente_Financiamiento_ID + "'");
                 }
                 if (!String.IsNullOrEmpty(Datos.P_Partida_ID))
                 {
                     Mi_Sql.Append(" AND " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Partida_ID + " = '" + Datos.P_Partida_ID + "'");
                 }
                 if (!String.IsNullOrEmpty(Datos.P_Clave_Programa))
                 {
                     Mi_Sql.Append(" AND " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Clave + " = '" + Datos.P_Clave_Programa + "'");
                 }
                 Mi_Sql.Append(" ORDER BY CLAVE_NOMBRE ASC");

                 return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
             }
             catch (Exception Ex)
             {
                 throw new Exception("Error al intentar consultar los registros de las fuentes de financiamiento. Error: [" + Ex.Message + "]");
             }
         }
         ///*******************************************************************************
         ///NOMBRE DE LA FUNCIÓN : Consulta_Programas_Ing
         ///DESCRIPCIÓN          : consulta para obtener los datos de los programas que tienen esa fuente de financiamiento seleccionada
         ///PARAMETROS           : 
         ///CREO                 : Leslie Gonzalez Vázquez
         ///FECHA_CREO           : 30/Noviembre/2011
         ///MODIFICO             :
         ///FECHA_MODIFICO       :
         ///CAUSA_MODIFICACIÓN   :
         ///*******************************************************************************
         public static DataTable Consulta_Programas_Ing(Cls_Ope_Con_Polizas_Negocio Datos)
         {
             StringBuilder Mi_Sql = new StringBuilder();
             try
             {
                 //OBTENEMOS LAS DEPENDENCIAS DEL CATALOGO
                 Mi_Sql.Append("SELECT DISTINCT " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Clave + " || ' ' || ");
                 Mi_Sql.Append(Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Descripcion + " AS CLAVE_NOMBRE, ");
                 Mi_Sql.Append(Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id);
                 //Se utiliza en el combo de programa_poliza para obtener el monto
                 if (!String.IsNullOrEmpty(Datos.P_Programa_ID))
                 {
                     Mi_Sql.Append(", " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Importe);
                 }
                 Mi_Sql.Append(" FROM " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas);
                 Mi_Sql.Append(" INNER JOIN " + Cat_Sap_Det_Fte_Programa.Tabla_Cat_Sap_Det_Fte_Programa);
                 Mi_Sql.Append(" ON " + Cat_Sap_Det_Fte_Programa.Tabla_Cat_Sap_Det_Fte_Programa + "." + Cat_Sap_Det_Fte_Programa.Campo_Proyecto_Programa_ID);
                 Mi_Sql.Append(" = " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id);
                 Mi_Sql.Append(" WHERE " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Estatus + " ='ACTIVO' ");
                 if (!String.IsNullOrEmpty(Datos.P_Fuente_Financiamiento_ID))
                 {
                     Mi_Sql.Append(" AND " + Cat_Sap_Det_Fte_Programa.Tabla_Cat_Sap_Det_Fte_Programa + "." + Cat_Sap_Det_Fte_Programa.Campo_Fuente_Financiamiento_ID + " = '" + Datos.P_Fuente_Financiamiento_ID + "'");
                 }
                 //Se utiliza en el combo de programa_poliza para obtener el monto
                 if (!String.IsNullOrEmpty(Datos.P_Programa_ID))
                 {
                     Mi_Sql.Append(" AND " + Cat_Sap_Det_Fte_Programa.Tabla_Cat_Sap_Det_Fte_Programa + "." + Cat_Sap_Det_Fte_Programa.Campo_Proyecto_Programa_ID + " = '" + Datos.P_Programa_ID  + "'");
                 }
                 Mi_Sql.Append(" ORDER BY CLAVE_NOMBRE ASC");

                 return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
             }
             catch (Exception Ex)
             {
                 throw new Exception("Error al intentar consultar los registros de los programas. Error: [" + Ex.Message + "]");
             }
         }
        
                     ///*******************************************************************************
         ///NOMBRE DE LA FUNCIÓN : Consulta_Cuentas_Contables_De_Conceptos
         ///DESCRIPCIÓN          : consulta para obtener los datos de los programas que tienen esa fuente de financiamiento seleccionada
         ///PARAMETROS           : 
         ///CREO                 : Sergio Manuel Gallardo Andrade
         ///FECHA_CREO           : 30/Junio/2012
         ///MODIFICO             :
         ///FECHA_MODIFICO       :
         ///CAUSA_MODIFICACIÓN   :
         ///*******************************************************************************
         public static DataTable Consulta_Cuentas_Contables_De_Conceptos(Cls_Ope_Con_Polizas_Negocio Datos)
         {
             StringBuilder Mi_Sql = new StringBuilder();
             try
             {
                 //OBTENEMOS LAS DEPENDENCIAS DEL CATALOGO
                 Mi_Sql.Append("SELECT  " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID+", ");
                 Mi_Sql.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Descripcion + ", ");
                 Mi_Sql.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta +", ");
                 Mi_Sql.Append(Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + "." + Cat_Psp_Concepto_Ing.Campo_Concepto_Ing_ID + ", ");
                 Mi_Sql.Append(Cat_Sap_Det_Fte_Programa.Tabla_Cat_Sap_Det_Fte_Programa + "." + Cat_Sap_Det_Fte_Programa.Campo_Fuente_Financiamiento_ID+", ");
                 Mi_Sql.Append(Cat_Sap_Det_Fte_Programa.Tabla_Cat_Sap_Det_Fte_Programa + "." + Cat_Sap_Det_Fte_Programa.Campo_Importe);
                 Mi_Sql.Append(" FROM " + Cat_Sap_Det_Fte_Programa.Tabla_Cat_Sap_Det_Fte_Programa);
                 Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing);
                 Mi_Sql.Append(" ON " + Cat_Sap_Det_Fte_Programa.Tabla_Cat_Sap_Det_Fte_Programa + "." + Cat_Sap_Det_Fte_Programa.Campo_Concepto_Ing_ID);
                 Mi_Sql.Append(" = " + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + "." + Cat_Psp_Concepto_Ing.Campo_Concepto_Ing_ID);
                 Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables);
                 Mi_Sql.Append(" ON " + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + "." + Cat_Psp_Concepto_Ing.Campo_Cuenta_Contable_ID);
                 Mi_Sql.Append(" = " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID );
                 Mi_Sql.Append(" WHERE " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Afectable  + " ='SI' ");
                 if (!String.IsNullOrEmpty(Datos.P_Programa_ID))
                 {
                     Mi_Sql.Append(" AND " + Cat_Sap_Det_Fte_Programa.Tabla_Cat_Sap_Det_Fte_Programa + "." + Cat_Sap_Det_Fte_Programa.Campo_Proyecto_Programa_ID+"='"+Datos.P_Programa_ID+"'");
                 }
                 Mi_Sql.Append(" ORDER BY " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables+"."+Cat_Con_Cuentas_Contables.Campo_Cuenta + " DESC");

                 return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
             }
             catch (Exception Ex)
             {
                 throw new Exception("Error al intentar consultar los registros de los programas. Error: [" + Ex.Message + "]");
             }
         }
        #endregion

        #region (Metodos Clases Externas)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Empleados_Especial
        /// DESCRIPCION : Consulta los empleados que cumplen con los requerimientos.
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 7/Octubre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Empleados_Especial(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            String Mi_SQL;    //Variable para la consulta para el Empleado
            try
            {
                //Consulta todos los Empleados que coincidan con lo proporcionado por el usuario
                Mi_SQL = "SELECT " + Cat_Empleados.Campo_Empleado_ID + ", " + Cat_Empleados.Campo_No_Empleado + ", ";
                Mi_SQL += Cat_Empleados.Campo_RFC + ", " + Cat_Empleados.Campo_Estatus + ", ";
                Mi_SQL += "" + Cat_Empleados.Campo_Apellido_Paterno;
                Mi_SQL += "||' '||" + Cat_Empleados.Campo_Apellido_Materno;
                Mi_SQL += "||' '||" + Cat_Empleados.Campo_Nombre + " AS EMPLEADO";
                Mi_SQL += " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                if(!string.IsNullOrEmpty(Datos.P_Nombre))
                {
                    Mi_SQL += " WHERE (" + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||";
                    Mi_SQL += Cat_Empleados.Campo_Apellido_Materno + " LIKE UPPER ('%" + Datos.P_Nombre + "%'))";
                }
                else {
                    Mi_SQL += " Where (" + Cat_Empleados.Campo_No_Empleado + " = '" + String.Format("{0:000000}", Convert.ToInt16(Datos.P_Empleado_ID)) + "')";

                }
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
        }

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Partida_Especifica
        /// DESCRIPCION : Consulta la partida especifica de acuerdo a la cuenta contable
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 4/Octubre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Partida_Especifica(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta para la póliza            
            try
            {
                Mi_SQL = "SELECT " + Cat_Sap_Partidas_Especificas.Campo_Clave + ", ";
                Mi_SQL += Cat_Sap_Partidas_Especificas.Campo_Partida_ID + ", ";
                Mi_SQL += Cat_Sap_Partidas_Especificas.Campo_Nombre;
                Mi_SQL += " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas;

                if (!String.IsNullOrEmpty(Datos.P_Cuenta))
                    Mi_SQL += " WHERE " + Cat_Sap_Partidas_Especificas.Campo_Cuenta + " = '" + Datos.P_Cuenta + "'";

                if (!String.IsNullOrEmpty(Datos.P_Clave))
                    Mi_SQL += " WHERE " + Cat_Sap_Partidas_Especificas.Campo_Clave + " = '" + Datos.P_Clave + "'";

                if (!String.IsNullOrEmpty(Datos.P_Partida_ID))
                    Mi_SQL += " WHERE " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " = '" + Datos.P_Partida_ID + "'";

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
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Area_Funcional_Especial
        ///DESCRIPCIÓN: Consulta las Areas Funcionales asociadas a la Fte de Financiamiento
        ///PARAMETROS:  1.- Cls_Cat_SAP_Area_Funcional_Negocios
        ///CREO: Salvador L. Rea Ayala
        ///FECHA_CREO: 4/Octubre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Area_Funcional_Especial(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            string Mi_SQL;  //Variable que contendra la Query de consutla.

            try
            {
                Mi_SQL = "SELECT " + Cat_SAP_Area_Funcional.Campo_Clave + " ||' - '|| " + Cat_SAP_Area_Funcional.Campo_Descripcion + " AS CLAVE_NOMBRE, ";
                Mi_SQL += Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID;
                Mi_SQL += " FROM " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional;

                if (!String.IsNullOrEmpty(Datos.P_Area_Funcional_ID))
                    Mi_SQL += " WHERE " + Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID + " = '" + Datos.P_Area_Funcional_ID + "'";

                if (!String.IsNullOrEmpty(Datos.P_Clave))
                    Mi_SQL += " WHERE " + Cat_SAP_Area_Funcional.Campo_Clave + " = '" + Datos.P_Clave + "'";

                Mi_SQL += " ORDER BY " + Cat_SAP_Area_Funcional.Campo_Clave + " ASC";

                //Sentencia que ejecuta el query
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Alta_Area_Funcional. Error: [" + Ex.Message + "]");
            }
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Programas_Especial
        ///DESCRIPCIÓN: Consulta los programas
        ///PARAMETROS: Datos: Variable de negocio que contiene los datos a consultar
        ///CREO: Salvador L. Rea Ayala
        ///FECHA_CREO: 4/Octubre/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************     
        public static DataTable Consulta_Programas_Especial()
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty;

            try
            {
                Mi_SQL = "SELECT " + Cat_Com_Proyectos_Programas.Campo_Clave + " ||' - '|| " + Cat_Com_Proyectos_Programas.Campo_Nombre + " AS CLAVE_NOMBRE, ";
                Mi_SQL += Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID;
                Mi_SQL += " FROM " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas;
                Mi_SQL += " ORDER BY " + Cat_Com_Proyectos_Programas.Campo_Clave + " ASC";

                //Entregar resultado
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Actualizar_Montos_Presupuesto()
        /// 	DESCRIPCIÓN: Actualiza el monto disponible al igual que el monto comprometido.
        /// 	PARÁMETROS:
        /// 		   Datos: Instancia de la clase de negocio con los datos para actualizar la base de datos
        /// 	CREO: Salvador L. Rea Ayala
        /// 	FECHA_CREO: 19/Octubre/2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static void Actualizar_Montos_Presupuesto(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            String Mi_SQL; //Contiene la consulta de modificación hacía la base de datos
            try
            {
                //Da de Alta los datos del Nuevo Parametro con los datos proporcionados por el usuario.
                Mi_SQL = "UPDATE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                Mi_SQL += " SET " + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + " = " + Datos.P_Disponible + ", ";
                Mi_SQL += Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + " = " + Datos.P_Comprometido;
                Mi_SQL += " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "' ";
                Mi_SQL += " AND " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID + " = '" + Datos.P_Fuente_Financiamiento_ID + "' ";
                Mi_SQL += " AND " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID + " = '" + Datos.P_Partida_ID + "' ";
                Mi_SQL += " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + " = '" + Datos.P_Programa_ID + "'";

                //Manda Mi_SQL para ser procesada por ORACLE.
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

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Dependencia_Partida_ID
        /// 	DESCRIPCIÓN: Consulta las dependencias ligadas a una partida especifica
        /// 	PARÁMETROS:  Datos: Indica qué registro se desea consultar a la base de datos
        /// 	CREO: Salvador L. Rea Ayala
        /// 	FECHA_CREO: 04-mar-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Dependencia_Partida_ID(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "SELECT DISTINCT " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID;
                Mi_SQL += " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                Mi_SQL += " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID + " = '" + Datos.P_Partida_ID + "'";
                Mi_SQL += " ORDER BY " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + " ASC";

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
        }
        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_GrupoRol
        /// 	DESCRIPCIÓN: Consulta el rol del Empleado que autoriza la poliza si ya esta cerrado el mes
        /// 	PARÁMETROS:  Datos: Indica qué registro se desea consultar a la base de datos
        /// 	CREO:Sergio Manuel Gallardo Andrade 
        /// 	FECHA_CREO:  10/noviembre/2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_GrupoRol(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "SELECT Rol.Grupo_Roles_ID " + Apl_Cat_Roles.Campo_Grupo_Roles_ID ;
                Mi_SQL += " FROM " + Apl_Cat_Roles.Tabla_Apl_Cat_Roles + " Rol, "+ Cat_Empleados.Tabla_Cat_Empleados +" Emp ";
                Mi_SQL += " WHERE Emp." + Cat_Empleados.Campo_Rol_ID + " = Rol." + Apl_Cat_Roles.Campo_Rol_ID  + " AND Emp." + Cat_Empleados.Campo_No_Empleado +"='"+ Datos.P_Empleado_ID +"' ";

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
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Fte_Area_Funcional_ID
        /// 	DESCRIPCIÓN: Consulta las Areas Funcionales ligadas a la Fuente de Financiamiento
        /// 	PARÁMETROS:  Datos: Indica qué registro se desea consultar a la base de datos
        /// 	CREO: Salvador L. Rea Ayala
        /// 	FECHA_CREO: 6/Octubre/2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Fte_Area_Funcional_ID(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "SELECT DISTINCT " + Cat_Com_Dep_Presupuesto.Campo_Area_Funcional_ID;
                Mi_SQL += " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                Mi_SQL += " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID + " = '" + Datos.P_Fuente_Financiamiento_ID + "'";
                Mi_SQL += " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "'";
                Mi_SQL += " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + " = '" + Datos.P_Programa_ID + "'";
                Mi_SQL += " ORDER BY " + Cat_Com_Dep_Presupuesto.Campo_Area_Funcional_ID + " ASC";

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
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Dependencia_Programa_ID
        /// 	DESCRIPCIÓN: Consulta los Programas ligados a las Dependencias
        /// 	PARÁMETROS:  Datos: Indica qué registro se desea consultar a la base de datos
        /// 	CREO: Salvador L. Rea Ayala
        /// 	FECHA_CREO: 6/Octubre/2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Dependencia_Programa_ID(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "SELECT DISTINCT " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID;
                Mi_SQL += " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                Mi_SQL += " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "'";
                Mi_SQL += " ORDER BY " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + " ASC";

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
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Programa_Fuente_ID
        /// 	DESCRIPCIÓN: Consulta las Fuentes de Financiamiento ligadas al Programa
        /// 	PARÁMETROS:  Datos: Indica qué registro se desea consultar a la base de datos
        /// 	CREO: Salvador L. Rea Ayala
        /// 	FECHA_CREO: 6/Octubre/2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Programa_Fuente_ID(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "SELECT DISTINCT " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID;
                Mi_SQL += " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                Mi_SQL += " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + " = '" + Datos.P_Programa_ID + "'";
                Mi_SQL += " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "'";
                Mi_SQL += " ORDER BY " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID + " ASC";

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
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Empleado_Jefe_Dependencia
        /// 	DESCRIPCIÓN: Consulta si el empleado logeado es Jefe de Dependencia.
        /// 	PARÁMETROS:  Datos: Indica qué registro se desea consultar a la base de datos
        /// 	CREO: Salvador L. Rea Ayala
        /// 	FECHA_CREO: 24/Octubre/2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Empleado_Jefe_Dependencia(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "SELECT " + Cat_Empleados.Campo_No_Empleado;
                Mi_SQL += " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL += " WHERE " + Cat_Empleados.Campo_Rol_ID + " = '00003'";
                Mi_SQL += " AND " + Cat_Empleados.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'";
                Mi_SQL += " ORDER BY " + Cat_Empleados.Campo_Empleado_ID + " ASC";

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
        }
        #endregion

        #region

        /// ********************************************************************************************************************
        /// NOMBRE:         Consulta_Cuenta_Partida_ID
        /// COMENTARIOS:    Consulta la partida id de la cuenta contable que se busca
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     20/Abril/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consulta_Cuenta_Partida_ID(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            try
            {
                Mi_SQL.Append("Select ");
                Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + ".* ");
                Mi_SQL.Append(" from " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables);
                Mi_SQL.Append(" where " + Cat_Con_Cuentas_Contables.Campo_Cuenta + "='" + Datos.P_Clave_Cuenta_Contable + "'");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0]; ;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Movimientos Presupuestales que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
        }

        /// ********************************************************************************************************************
        /// NOMBRE:         Consulta_ID_Carga_Masiva
        /// COMENTARIOS:    Consulta las id del registro que se busca dentro de la carga masiva
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     20/Abril/2012  
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consulta_ID_Fte_Financiamiento(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            StringBuilder Mi_Sql = new StringBuilder();//Variable que almacenara la consulta.
            try
            {
                Mi_Sql.Append("SELECT DISTINCT " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " || ' ' || ");
                Mi_Sql.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + " AS CLAVE_NOMBRE, ");
                Mi_Sql.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID);
                Mi_Sql.Append(" FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento);
                Mi_Sql.Append(" INNER JOIN " + Ope_Psp_Presupuesto_Ingresos.Tabla_Ope_Psp_Presupuesto_Ingresos);
                Mi_Sql.Append(" ON " + Ope_Psp_Presupuesto_Ingresos.Tabla_Ope_Psp_Presupuesto_Ingresos + "." + Ope_Psp_Presupuesto_Ingresos.Campo_Fuente_Financiamiento_ID);
                Mi_Sql.Append(" = " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID);
                Mi_Sql.Append(" INNER JOIN " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables);
                Mi_Sql.Append(" ON " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Partida_ID);
                Mi_Sql.Append(" = " + Ope_Psp_Presupuesto_Ingresos.Tabla_Ope_Psp_Presupuesto_Ingresos + "." + Ope_Psp_Presupuesto_Ingresos.Campo_Concepto_Ing_ID);
                Mi_Sql.Append(" WHERE " + Ope_Psp_Presupuesto_Ingresos.Tabla_Ope_Psp_Presupuesto_Ingresos + "." + Ope_Psp_Presupuesto_Ingresos.Campo_Anio);
                Mi_Sql.Append(" = " + String.Format("{0:yyyy}", DateTime.Now));
                if (!String.IsNullOrEmpty(Datos.P_Partida_ID))
                {
                    Mi_Sql.Append(" AND " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Partida_ID + " = '" + Datos.P_Partida_ID + "'");
                }
                if (!String.IsNullOrEmpty(Datos.P_Clave_Fte_Financiamiento))
                {
                    Mi_Sql.Append(" AND " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " = '" + Datos.P_Clave_Fte_Financiamiento + "'");
                }
                Mi_Sql.Append(" ORDER BY CLAVE_NOMBRE ASC");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Movimientos Presupuestales que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
        }
        /// ********************************************************************************************************************
        /// NOMBRE:         Consulta_ID_Carga_Masiva
        /// COMENTARIOS:    Consulta las id del registro que se busca dentro de la carga masiva
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     20/Abril/2012  
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consulta_ID_Fte_Financiamiento_Egr(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            StringBuilder Mi_Sql = new StringBuilder();//Variable que almacenara la consulta.
            try
            {
                Mi_Sql.Append("SELECT DISTINCT " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " || ' ' || ");
                Mi_Sql.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + " AS CLAVE_NOMBRE, ");
                Mi_Sql.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID);
                Mi_Sql.Append(" FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento);
                Mi_Sql.Append(" INNER JOIN " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado);
                Mi_Sql.Append(" ON " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID);
                Mi_Sql.Append(" = " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID);
                Mi_Sql.Append(" INNER JOIN " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables);
                Mi_Sql.Append(" ON " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Partida_ID);
                Mi_Sql.Append(" = " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID);
                Mi_Sql.Append(" WHERE " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Anio);
                Mi_Sql.Append(" = " + String.Format("{0:yyyy}", DateTime.Now));
                if (!String.IsNullOrEmpty(Datos.P_Partida_ID))
                {
                    Mi_Sql.Append(" AND " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Partida_ID + " = '" + Datos.P_Partida_ID + "'");
                }
                if (!String.IsNullOrEmpty(Datos.P_Clave_Fte_Financiamiento))
                {
                    Mi_Sql.Append(" AND " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " = '" + Datos.P_Clave_Fte_Financiamiento + "'");
                }
                Mi_Sql.Append(" ORDER BY CLAVE_NOMBRE ASC");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Movimientos Presupuestales que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
        }
        #endregion
        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Solicitud_Pago
        /// 	DESCRIPCIÓN: Consulta la informacion de la solicitud de pago para conocer al proveedor
        /// 	PARÁMETROS:  Datos: Indica qué registro se desea consultar a la base de datos
        /// 	CREO:Sergio Manuel Gallardo Andrade 
        /// 	FECHA_CREO:  10/mayo/2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Solicitud_Pago(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "SELECT * FROM " + Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos;
                Mi_SQL += " WHERE " + Ope_Con_Solicitud_Pagos.Campo_No_Reserva + " = '" + Datos.P_No_Reserva + "' AND  ";
                Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_No_Poliza + " = '" + Datos.P_No_Poliza + "' AND  ";
                Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_Mes_Ano + " = '" + Datos.P_Mes_Ano + "' AND  ";
                Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_Tipo_Poliza_ID + " = '" + Datos.P_Tipo_Poliza_ID + "' AND ";
                Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_Estatus + " != 'CANCELADO'";

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
        }
        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Pago
        /// 	DESCRIPCIÓN: Consulta la informacion de la solicitud de pago para conocer al proveedor
        /// 	PARÁMETROS:  Datos: Indica qué registro se desea consultar a la base de datos
        /// 	CREO:Sergio Manuel Gallardo Andrade 
        /// 	FECHA_CREO:  10/mayo/2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Pago(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "SELECT Pago.*,Solicitud." + Ope_Con_Solicitud_Pagos .Campo_No_Solicitud_Pago +" As solicitud  FROM " + Ope_Con_Pagos.Tabla_Ope_Con_Pagos + " Pago," + Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + " Solicitud";
                Mi_SQL += " WHERE Pago." + Ope_Con_Pagos.Campo_No_Pago + " = Solicitud." + Ope_Con_Solicitud_Pagos.Campo_No_Pago + " AND  Pago.";
                Mi_SQL += Ope_Con_Pagos.Campo_No_poliza + " = '" + Datos.P_No_Poliza + "' AND  Pago.";
                Mi_SQL += Ope_Con_Pagos.Campo_Mes_Ano + " = '" + Datos.P_Mes_Ano + "' AND  Pago.";
                Mi_SQL += Ope_Con_Pagos.Campo_Tipo_Poliza_ID + " = '" + Datos.P_Tipo_Poliza_ID + "' AND Solicitud.";
                Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_No_Reserva + " = '" + Datos.P_No_Reserva + "' AND Solicitud.";
                Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_Estatus + " != 'CANCELADO'";
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
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Saldo_Disponible_Poliza
        ///DESCRIPCIÓN:     Consulta el saldo disponible de una cuenta
        ///PARAMENTROS:     
        ///CREO:            Armando Zavala Moreno
        ///FECHA_CREO:      30/Agosto/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO 
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
        public static Boolean Consulta_Saldo_Disponible_Poliza(Cls_Ope_Con_Polizas_Negocio Datos)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;
            Boolean Segundo_Filtro = false;
            Boolean Existe = false;
            try
            {
                Mi_SQL = "SELECT PRO." + Cat_SAP_Det_Prog_Dependencia.Campo_Proyecto_Programa_ID;

                Mi_SQL += " FROM " + Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia+" PRO";
                Mi_SQL += " INNER JOIN " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas+ " PAR";
                Mi_SQL += " ON PRO." + Cat_SAP_Det_Prog_Dependencia.Campo_Proyecto_Programa_ID + "= PAR." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Proyecto_Programa_ID;

                Mi_SQL += " INNER JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas+" P";
                Mi_SQL += " ON PAR." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Partida_ID + "= P." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID;

                Mi_SQL += " INNER JOIN " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + " G";
                Mi_SQL += " ON P." + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID + "= G." + Cat_Sap_Partidas_Genericas.Campo_Partida_Generica_ID;

                Mi_SQL += " INNER JOIN " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + " CON";
                Mi_SQL += " ON G." + Cat_Sap_Partidas_Genericas.Campo_Concepto_ID + "= CON." + Cat_Sap_Concepto.Campo_Concepto_ID;

                if (!String.IsNullOrEmpty(Datos.P_Partida_ID))
                {
                    if (Segundo_Filtro == true) { Mi_SQL += " AND "; }
                    else { Mi_SQL += " WHERE "; }
                    Mi_SQL += "P."+Cat_Sap_Partidas_Especificas.Campo_Partida_ID + "='" + Datos.P_Partida_ID + "'";
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Datos.P_Programa_ID))
                {
                    if (Segundo_Filtro == true) { Mi_SQL += " AND "; }
                    else { Mi_SQL += " WHERE "; }
                    Mi_SQL += "PAR." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Proyecto_Programa_ID + "='" + Datos.P_Programa_ID + "'";
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    if (Segundo_Filtro == true) { Mi_SQL += " AND "; }
                    else { Mi_SQL += " WHERE "; }
                    Mi_SQL += "PRO." + Cat_SAP_Det_Prog_Dependencia.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Datos.P_Validar_Saldo))
                {
                    if (Segundo_Filtro == true) { Mi_SQL += " AND "; }
                    else { Mi_SQL += " WHERE "; }
                    Mi_SQL += "((SELECT "+ Ope_Psp_Presupuesto_Aprobado.Campo_Disponible+" FROM "
                        + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado
                        + " WHERE " + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID + "='" + Datos.P_Fuente_Financiamiento_ID + "'"
                        + " AND " + Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'"
                        + " AND " + Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID + "= PAR." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Proyecto_Programa_ID
                        + " AND " + Ope_Psp_Presupuesto_Aprobado.Campo_Capitulo_ID + "= CON."+Cat_Sap_Concepto.Campo_Capitulo_ID
                        + " AND " + Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID + "= P." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + ")>=" + Datos.P_Validar_Saldo + ")";
                    Segundo_Filtro = true;
                }
                
                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null)
                {
                    Tabla = dataSet.Tables[0];
                    if (Tabla.Rows.Count > 0)
                    {
                        Existe = true;
                    }
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de la Cuentas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Existe;
        }
    }
}