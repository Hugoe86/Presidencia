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
using Presidencia.Ajuste_ISR.Negocio;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using System.Text;

namespace Presidencia.Ajuste_ISR.Datos
{
    public class Cls_Ope_Nom_Ajuste_ISR_Datos
    {
        #region (Metodos)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Ajuste_ISR
        /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta el Ajuste ISR en la BD con los datos proporcionados
        ///                  por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 22/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Alta_Ajuste_ISR(Cls_Ope_Nom_Ajuste_ISR_Negocio Datos) {
            String Mi_Oracle;       //Obtiene la cadena de inserción hacía la base de datos
            Object No_Ajuste_ISR;   //Obtiene el ID con la cual se guardo los datos en la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso
            Boolean Operacion_Completa = false;//Variable que almacenara si la operacion se realizo con exito o no.

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                Mi_Oracle = "SELECT NVL(MAX(" + Ope_Nom_Ajuste_ISR.Campo_No_Ajuste_ISR + "),'0000000000') ";
                Mi_Oracle = Mi_Oracle + "FROM " + Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR;
                No_Ajuste_ISR = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);

                if (Convert.IsDBNull(No_Ajuste_ISR))
                {
                    Datos.P_No_Ajuste_ISR = "0000000001";
                }
                else
                {
                    Datos.P_No_Ajuste_ISR = String.Format("{0:0000000000}", Convert.ToInt64(No_Ajuste_ISR) + 1);
                }

                Mi_Oracle = "INSERT INTO " + Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR + " ( " +
                    Ope_Nom_Ajuste_ISR.Campo_No_Ajuste_ISR + ", " +
                    Ope_Nom_Ajuste_ISR.Campo_Empleado_ID + ", " +
                    Ope_Nom_Ajuste_ISR.Campo_Dependencia_ID + ", " +
                    Ope_Nom_Ajuste_ISR.Campo_Percepcion_Deduccion_ID + ", " +
                    Ope_Nom_Ajuste_ISR.Campo_Nomina_ID + ", " +
                    Ope_Nom_Ajuste_ISR.Campo_No_Nomina + ", " +
                    Ope_Nom_Ajuste_ISR.Campo_Estatus_Ajuste_ISR + ", " +
                    Ope_Nom_Ajuste_ISR.Campo_Fecha_Inicio_Pago + ", " +
                    Ope_Nom_Ajuste_ISR.Campo_Fecha_Termino_Pago + ", " +
                    Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustar + ", " +
                    Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustado + ", " +
                    Ope_Nom_Ajuste_ISR.Campo_No_Catorcenas + ", " +
                    Ope_Nom_Ajuste_ISR.Campo_Pago_Catorcenal_ISR + ", " +
                    Ope_Nom_Ajuste_ISR.Campo_No_Pago + ", " +
                    Ope_Nom_Ajuste_ISR.Campo_Comentarios_Ajuste + ", " +
                    Ope_Nom_Ajuste_ISR.Campo_Usuario_Creo + ", " +
                    Ope_Nom_Ajuste_ISR.Campo_Fecha_Creo + ") VALUES(" +
                    "'" + Datos.P_No_Ajuste_ISR + "', " +
                    "'" + Datos.P_Empleado_ID + "', " +
                    "'" + Datos.P_Dependencia_ID + "', " +
                    "'" + Datos.P_Percepcion_Deduccion_ID + "', " +
                    "'" + Datos.P_Nomina_ID + "', " +
                    "" + Datos.P_No_Nomina + ", " +
                    "'" + Datos.P_Estatus_Ajuste_ISR + "', " +
                    "'" + Datos.P_Fecha_Inicio_Pago + "', " +
                    "'" + Datos.P_Fecha_Termino_Pago + "', " +
                    "" + Datos.P_Total_ISR_Ajustar + ", " +
                    "" + Datos.P_Total_ISR_Ajustado + ", " +
                    "" + Datos.P_No_Catorcenas + ", " +
                    "" + Datos.P_Pago_Catorcenal_ISR + ", " +
                    "" + Datos.P_No_Pago + ", " +
                    "'" + Datos.P_Comentarios_Ajuste + "', " +
                    "'" + Datos.P_Usuario_Creo + "', SYSDATE)";

                Comando_SQL.CommandText = Mi_Oracle; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                Transaccion_SQL.Commit(); //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos
                Operacion_Completa = true;
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Conexion_Base.Close();
            }
            return Operacion_Completa;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Ajuste_ISR
        /// DESCRIPCION : 1.Actualiza los datos del Ajuste de ISR seleccionado.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 22/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Modificar_Ajuste_ISR(Cls_Ope_Nom_Ajuste_ISR_Negocio Datos)
        {
            String Mi_Oracle;       //Obtiene la cadena de inserción hacía la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso
            Boolean Operacion_Completa = false;                                                 //almacenara si la operacion se completo con exito. 

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                Mi_Oracle = "UPDATE " + Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR + " SET " +
                    Ope_Nom_Ajuste_ISR.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "', " +
                    Ope_Nom_Ajuste_ISR.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "', " +
                    Ope_Nom_Ajuste_ISR.Campo_Percepcion_Deduccion_ID + "='" + Datos.P_Percepcion_Deduccion_ID + "', " +
                    Ope_Nom_Ajuste_ISR.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "', " +
                    Ope_Nom_Ajuste_ISR.Campo_No_Nomina + "=" + Datos.P_No_Nomina + ", " +
                    Ope_Nom_Ajuste_ISR.Campo_Estatus_Ajuste_ISR + "='" + Datos.P_Estatus_Ajuste_ISR + "', " +
                    Ope_Nom_Ajuste_ISR.Campo_Fecha_Inicio_Pago + "='" + Datos.P_Fecha_Inicio_Pago + "', " +
                    Ope_Nom_Ajuste_ISR.Campo_Fecha_Termino_Pago + "='" + Datos.P_Fecha_Termino_Pago + "', " +
                    Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustar + "=" + Datos.P_Total_ISR_Ajustar + ", " +
                    Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustado + "=" + Datos.P_Total_ISR_Ajustado + ", " +
                    Ope_Nom_Ajuste_ISR.Campo_No_Catorcenas + "=" + Datos.P_No_Catorcenas + ", " +
                    Ope_Nom_Ajuste_ISR.Campo_Pago_Catorcenal_ISR + "=" + Datos.P_Pago_Catorcenal_ISR + ", " +
                    Ope_Nom_Ajuste_ISR.Campo_No_Pago + "=" + Datos.P_No_Pago + ", " +
                    Ope_Nom_Ajuste_ISR.Campo_Comentarios_Ajuste + "='" + Datos.P_Comentarios_Ajuste + "', " +
                    Ope_Nom_Ajuste_ISR.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Modifico + "', " +
                    Ope_Nom_Ajuste_ISR.Campo_Fecha_Modifico + "= SYSDATE WHERE " +
                    Ope_Nom_Ajuste_ISR.Campo_No_Ajuste_ISR + "='" + Datos.P_No_Ajuste_ISR + "'";
                    
                Comando_SQL.CommandText = Mi_Oracle; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                Transaccion_SQL.Commit(); //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos
                Operacion_Completa = true;
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Conexion_Base.Close();
            }
            return Operacion_Completa;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Eliminar_Ajuste_ISR
        /// DESCRIPCION : 1.- Elimina el Ajuste de ISR seleccionado.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 22/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Eliminar_Ajuste_ISR(Cls_Ope_Nom_Ajuste_ISR_Negocio Datos) {
            String Mi_Oracle;       //Obtiene la cadena de inserción hacía la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso
            Boolean Operacion_Completa = false;                                                 //Variable que almacenara si la operacion se completo con exito.

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos
            Operacion_Completa = true;

            try
            {
                Mi_Oracle = "DELETE FROM " + Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR +
                            " WHERE " + Ope_Nom_Ajuste_ISR.Campo_No_Ajuste_ISR + "='" + Datos.P_No_Ajuste_ISR + "'";

                Comando_SQL.CommandText = Mi_Oracle; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                Transaccion_SQL.Commit(); //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Conexion_Base.Close();
            }
            return Operacion_Completa;
        }
        ///*******************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Fecha_Inicio_Periodo_Pago
        /// DESCRIPCION : Consulta la fecha de inicio del periodo de pago seleccionado.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 01/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************************************************
        public static DataTable Consultar_Fecha_Inicio_Periodo_Pago(Cls_Ope_Nom_Ajuste_ISR_Negocio Datos)
        {
            DataTable Dt_Detalles_Nomina = null;//Variable que almacenara los datos de la nomina seleccionada.
            String Mi_Oracle = "";//Variable que almacenara la consulta.

            try
            {
                Mi_Oracle = "SELECT " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio + ", " +
                             Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin +
                            " FROM " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + " INNER JOIN " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles +
                            " ON " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID +
                            " = " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID +                             
                            " WHERE " +
                             Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_No_Nomina + "=" + Datos.P_No_Nomina + 
                             " AND " +
                             Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'";

                Dt_Detalles_Nomina = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Dt_Detalles_Nomina;
        }
        ///*******************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Fechas_Periodo
        /// DESCRIPCION : Consulta las fechas del periodo seleccionado.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 06/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************************************************
        public static DataTable Consultar_Fechas_Periodo(Cls_Ope_Nom_Ajuste_ISR_Negocio Datos)
        {
            String Mi_Oracle = "";//Variable que almacenara la consulta.
            DataTable Dt_Datos_No_Nomina = null;//Variable que almacenara los detalles del numero de periodo seleccionado.
            try
            {
                Mi_Oracle = "SELECT " + Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio + ", " + Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin + " FROM " +
                    Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + " WHERE " +
                    Cat_Nom_Nominas_Detalles.Campo_No_Nomina + "='" + Datos.P_No_Nomina + "' AND " +
                    Cat_Nom_Nominas_Detalles.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'";

                Dt_Datos_No_Nomina = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Dt_Datos_No_Nomina;
        }
        ///*******************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Solicitudes_Prestamos
        /// DESCRIPCION : Consulta las solicitudes de prestamos vigentes en el sistema.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 30/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************************************************
        public static Cls_Ope_Nom_Ajuste_ISR_Negocio Consulta_Ajuste_ISR(Cls_Ope_Nom_Ajuste_ISR_Negocio Datos)
        {
            String Mi_Oracle = "";
            Cls_Ope_Nom_Ajuste_ISR_Negocio Cls_Transporta_Datos = new Cls_Ope_Nom_Ajuste_ISR_Negocio();
            DataTable Dt_Ajuste_ISR = null;
            String Fecha_Inicio = "";
            String Fecha_Fin = "";

            try
            {
                Fecha_Inicio = Datos.P_Fecha_Inicio_Pago;
                Fecha_Fin = Datos.P_Fecha_Termino_Pago;

                Mi_Oracle = "SELECT " + Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR + ".*, (" +
                    Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + "|| ' ' ||" +
                    Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "|| ' ' ||" +
                    Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ") As NOMBRE" +
                    " FROM " +
                    Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR + " INNER JOIN " + Cat_Empleados.Tabla_Cat_Empleados +
                    " ON " + Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR + "." + Ope_Nom_Ajuste_ISR.Campo_Empleado_ID + "=" +
                    Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;

                if (!string.IsNullOrEmpty(Datos.P_No_Ajuste_ISR))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR + "." +  Ope_Nom_Ajuste_ISR.Campo_No_Ajuste_ISR + "='" + Datos.P_No_Ajuste_ISR + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR + "." + Ope_Nom_Ajuste_ISR.Campo_No_Ajuste_ISR + "='" + Datos.P_No_Ajuste_ISR + "'";
                    }
                }
                if (!string.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR + "." + Ope_Nom_Ajuste_ISR.Campo_Empleado_ID + " IN " +
                            "(SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "')";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR + "." + Ope_Nom_Ajuste_ISR.Campo_Empleado_ID + " IN " +
                            "(SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "')";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_RFC_Empleado))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR + "." + Ope_Nom_Ajuste_ISR.Campo_Empleado_ID + " IN " +
                            "(SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC_Empleado + "')";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR + "." + Ope_Nom_Ajuste_ISR.Campo_Empleado_ID + " IN " +
                            "(SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC_Empleado + "')";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Estatus_Ajuste_ISR))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR + "." + Ope_Nom_Ajuste_ISR.Campo_Estatus_Ajuste_ISR + "='" + Datos.P_Estatus_Ajuste_ISR + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR + "." + Ope_Nom_Ajuste_ISR.Campo_Estatus_Ajuste_ISR + "='" + Datos.P_Estatus_Ajuste_ISR + "'";
                    }
                }
                if (!string.IsNullOrEmpty(Fecha_Inicio) && !string.IsNullOrEmpty(Fecha_Fin))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR + "." + Ope_Nom_Ajuste_ISR.Campo_Fecha_Creo + " BETWEEN TO_DATE ('" + Fecha_Inicio + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                         " AND TO_DATE ('" + Fecha_Fin + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR + "." + Ope_Nom_Ajuste_ISR.Campo_Fecha_Creo + " BETWEEN TO_DATE ('" + Fecha_Inicio + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                         " AND TO_DATE ('" + Fecha_Fin + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                }


                Dt_Ajuste_ISR = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];


                if (Dt_Ajuste_ISR != null)
                {
                    Cls_Transporta_Datos.P_Dt_Ajustes_ISR = Dt_Ajuste_ISR;
                    if (Dt_Ajuste_ISR.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_No_Ajuste_ISR].ToString())) Cls_Transporta_Datos.P_No_Ajuste_ISR = Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_No_Ajuste_ISR].ToString();
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Empleado_ID].ToString())) Cls_Transporta_Datos.P_Empleado_ID = Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Empleado_ID].ToString();                       
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Percepcion_Deduccion_ID].ToString())) Cls_Transporta_Datos.P_Percepcion_Deduccion_ID = Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Percepcion_Deduccion_ID].ToString();
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Nomina_ID].ToString())) Cls_Transporta_Datos.P_Nomina_ID = Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Nomina_ID].ToString();
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_No_Nomina].ToString())) Cls_Transporta_Datos.P_No_Nomina = Convert.ToInt32(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_No_Nomina].ToString());
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Estatus_Ajuste_ISR].ToString())) Cls_Transporta_Datos.P_Estatus_Ajuste_ISR= Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Estatus_Ajuste_ISR].ToString();                        
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Fecha_Creo].ToString())) Cls_Transporta_Datos.P_Fecha_Creo = string.Format("{0:dd/MM/yyyy}", HttpUtility.HtmlDecode(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Fecha_Creo].ToString()));
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Fecha_Inicio_Pago].ToString())) Cls_Transporta_Datos.P_Fecha_Inicio_Pago = string.Format("{0:dd/MM/yyyy}", HttpUtility.HtmlDecode(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Fecha_Inicio_Pago].ToString()));
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Fecha_Termino_Pago].ToString())) Cls_Transporta_Datos.P_Fecha_Termino_Pago = string.Format("{0:dd/MM/yyyy}", HttpUtility.HtmlDecode(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Fecha_Termino_Pago].ToString()));
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Comentarios_Ajuste].ToString())) Cls_Transporta_Datos.P_Comentarios_Ajuste = Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Comentarios_Ajuste].ToString();
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_No_Catorcenas].ToString())) Cls_Transporta_Datos.P_No_Catorcenas = Convert.ToInt32(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_No_Catorcenas].ToString());
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustar].ToString())) Cls_Transporta_Datos.P_Total_ISR_Ajustar = Convert.ToDouble(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustar].ToString());
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustado].ToString())) Cls_Transporta_Datos.P_Total_ISR_Ajustado = Convert.ToDouble(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustado].ToString());                                               
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Pago_Catorcenal_ISR].ToString())) Cls_Transporta_Datos.P_Pago_Catorcenal_ISR = Convert.ToDouble(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Pago_Catorcenal_ISR].ToString());
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_No_Pago].ToString())) Cls_Transporta_Datos.P_No_Pago = Convert.ToInt32(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_No_Pago].ToString());
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Cls_Transporta_Datos;
        }
        ///*******************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Percepcion_Ajuste_ISR
        /// DESCRIPCION : Consulta la percepción que le corresponde al AJUSTE DE ISR en la tabla de Parámetros de la Nómina.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 31/Enero/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************************************************
        public static DataTable Consultar_Percepcion_Ajuste_ISR(Cls_Ope_Nom_Ajuste_ISR_Negocio Datos)
        {
            String Mi_SQL = "";//Variable que almacenará la consulta de Percepción que le corresponde al ajuste de ISR.
            DataTable Dt_Percepcion_Ajuste_ISR = null;//Variable que almacena el registro de la percepión que corresponde al ajuste de ISR.

            try
            {
                Mi_SQL = "SELECT " + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID +
                        ", " +
                        Cat_Nom_Percepcion_Deduccion.Campo_Nombre +
                        " FROM " +
                        Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion +
                        " WHERE " +
                        Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + " IN " +
                        " (SELECT " + Cat_Nom_Parametros.Campo_Percepcion_Ajuste_ISR + " FROM " +
                        Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + ")";

                Dt_Percepcion_Ajuste_ISR = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
            return Dt_Percepcion_Ajuste_ISR;
        }
        ///*******************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Finiquitar_Ajuste_ISR
        /// DESCRIPCION : Finiquita el Ajuste de ISR del empleado.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 15/Junio/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************************************************
        public static void Finiquitar_Ajuste_ISR(Cls_Ope_Nom_Ajuste_ISR_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta
            DataTable Dt_Ajustes_ISR = null;
            Double Total_ISR_Ajustar = 0;
            Int32 No_Catorcenas = 0;

            try
            {
                Mi_SQL.Append("SELECT " + Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR + ".* ");
                Mi_SQL.Append(" FROM " + Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR);
                Mi_SQL.Append(" WHERE " + Ope_Nom_Ajuste_ISR.Campo_No_Ajuste_ISR + "='" + Datos.P_No_Ajuste_ISR + "'");

                Dt_Ajustes_ISR = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                Mi_SQL = new StringBuilder();

                if (Dt_Ajustes_ISR is DataTable) {
                    if (Dt_Ajustes_ISR.Rows.Count > 0) {
                        foreach (DataRow AJUSTE_ISR in Dt_Ajustes_ISR.Rows) {
                            if (AJUSTE_ISR is DataRow) {

                                if (!String.IsNullOrEmpty(AJUSTE_ISR[Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustar].ToString().Trim()))
                                    Total_ISR_Ajustar = Convert.ToDouble(AJUSTE_ISR[Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustar].ToString().Trim());

                                if (!String.IsNullOrEmpty(AJUSTE_ISR[Ope_Nom_Ajuste_ISR.Campo_No_Pago].ToString().Trim()))
                                    No_Catorcenas = Convert.ToInt32(AJUSTE_ISR[Ope_Nom_Ajuste_ISR.Campo_No_Catorcenas].ToString().Trim());

                                Mi_SQL.Append("UPDATE " + Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR);
                                Mi_SQL.Append(" SET " + Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustado + "=" + Total_ISR_Ajustar + ", ");
                                Mi_SQL.Append(Ope_Nom_Ajuste_ISR.Campo_No_Pago + "=" + No_Catorcenas + ", ");
                                Mi_SQL.Append(Ope_Nom_Ajuste_ISR.Campo_Estatus_Ajuste_ISR + "='Pagado'");
                                Mi_SQL.Append(" WHERE " + Ope_Nom_Ajuste_ISR.Campo_No_Ajuste_ISR + "='" + Datos.P_No_Ajuste_ISR + "'");

                                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error producido al ejecutar el finiquito del ajuste de ISR. Error: [" + Ex.Message + "]");
            }
        }
        #endregion
    }
}
