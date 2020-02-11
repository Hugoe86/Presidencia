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
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Prestamos.Negocio;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Empleados.Negocios;
using System.Text;

namespace Presidencia.Prestamos.Datos
{
    public class Cls_Ope_Nom_Prestamos_Datos
    {
        #region(Metodos)

        #region (Metodos de Operación)
        ///*******************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Solicitud_Prestamo
        /// DESCRIPCION : Consulta el ID consecutivo y ejecuta el alta de la solicitud del
        /// prestamo.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 30/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************************************************
        public static Boolean Alta_Solicitud_Prestamo(Cls_Ope_Nom_Pestamos_Negocio Datos)
        {
            String Mi_SQL = "";
            Boolean Operacion_Completa = false;
            Object No_Solicitud = null;                //Obtiene el ID con la cual se guardo los datos en la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }

            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud + "),'0000000000') ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo;
                No_Solicitud = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(No_Solicitud))
                {
                    Datos.P_No_Solicitud = "0000000001";
                }
                else
                {
                    Datos.P_No_Solicitud = String.Format("{0:0000000000}", Convert.ToInt32(No_Solicitud) + 1);
                }

                Mi_SQL = "INSERT INTO " + Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo + " (" +
                    Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud + ", " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Solicita_Empleado_ID + ", " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Aval_Empleado_ID + ", " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Proveedor_ID + ", " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Percepcion_Deduccion_ID + ", " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Nomina_ID + ", " +
                    Ope_Nom_Solicitud_Prestamo.Campo_No_Nomina + ", " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Solicitud + ", " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Pago + ", " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Solicitud + ", " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Inicio_Pago + ", " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Termino_Pago + ", " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Motivo_Prestamo + ", " +
                    Ope_Nom_Solicitud_Prestamo.Campo_No_Pagos + ", " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Importe_Prestamo + ", " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Importe_Interes + ", " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Total_Prestamo + ", " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual + ", " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Abono + ", " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Finiquito_Prestamo + ", " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Estado_Prestamo + ", " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Usuario_Creo + ", " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Creo + ", " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Aplica_Validaciones +
                    ") VALUES(";

                if (!string.IsNullOrEmpty(Datos.P_No_Solicitud))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_No_Solicitud + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (!string.IsNullOrEmpty(Datos.P_Solicita_Empleado_ID))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Solicita_Empleado_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (!string.IsNullOrEmpty(Datos.P_Aval_Empleado_ID))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Aval_Empleado_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (!string.IsNullOrEmpty(Datos.P_Proveedor_ID))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Proveedor_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (!string.IsNullOrEmpty(Datos.P_Percepcion_Deduccion_ID))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Percepcion_Deduccion_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (!string.IsNullOrEmpty(Datos.P_Nomina_ID))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Nomina_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (Datos.P_No_Nomina is Int32)
                {
                    Mi_SQL = Mi_SQL + "" + Datos.P_No_Nomina + ", ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (!string.IsNullOrEmpty(Datos.P_Estatus_Solicitud))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Estatus_Solicitud + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (!string.IsNullOrEmpty(Datos.P_Estatus_Pago))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Estatus_Pago + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (!string.IsNullOrEmpty(Datos.P_Fecha_Solicitud))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Fecha_Solicitud + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (!string.IsNullOrEmpty(Datos.P_Fecha_Inicio_Pago))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Fecha_Inicio_Pago + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (!string.IsNullOrEmpty(Datos.P_Fecha_Termino_Pago))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Fecha_Termino_Pago + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (!string.IsNullOrEmpty(Datos.P_Motivo_Prestamo))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Motivo_Prestamo + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (Datos.P_No_Pagos is Int32)
                {
                    Mi_SQL = Mi_SQL + "" + Datos.P_No_Pagos + ", ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (Datos.P_Importe_Prestamo is Double)
                {
                    Mi_SQL = Mi_SQL + "" + Datos.P_Importe_Prestamo + ", ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (Datos.P_Importe_Interes is Double)
                {
                    Mi_SQL = Mi_SQL + "" + Datos.P_Importe_Interes + ", ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (Datos.P_Total_Prestamo is Double)
                {
                    Mi_SQL = Mi_SQL + "" + Datos.P_Total_Prestamo + ", ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (Datos.P_Saldo_Actual is Double)
                {
                    Mi_SQL = Mi_SQL + "" + Datos.P_Saldo_Actual + ", ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (Datos.P_Abono is Double)
                {
                    Mi_SQL = Mi_SQL + "" + Datos.P_Abono + ", ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (!string.IsNullOrEmpty(Datos.P_Fecha_Finiquito_Prestamo))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Fecha_Finiquito_Prestamo + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (!string.IsNullOrEmpty(Datos.P_Estado_Prestamo))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Estado_Prestamo + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                Mi_SQL += "'" + Datos.P_Usuario_Creo + "', SYSDATE, ";

                if (string.IsNullOrEmpty(Datos.P_Aplica_Validaciones))
                {
                    Mi_SQL = Mi_SQL + "'SI')";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL)";
                }

                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
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
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
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
        /// NOMBRE DE LA FUNCION: Modificar_Solicitud_Prestamo
        /// DESCRIPCION : Ejecuta la actualizacion de los datos de la solicitud del prestamo
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 30/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************************************************
        public static Boolean Modificar_Solicitud_Prestamo(Cls_Ope_Nom_Pestamos_Negocio Datos)
        {
            String Mi_SQL = "";
            Boolean Operacion_Completa = false;
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }

            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                Mi_SQL = "UPDATE " + Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo + " SET ";

                if (Datos.P_Solicita_Empleado_ID != null)
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Solicita_Empleado_ID + " = '" + Datos.P_Solicita_Empleado_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Solicita_Empleado_ID + " = NULL, ";
                }

                if (Datos.P_Aval_Empleado_ID != null)
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Aval_Empleado_ID + " = '" + Datos.P_Aval_Empleado_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Aval_Empleado_ID + " = NULL, ";
                }

                if (Datos.P_Proveedor_ID != null)
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Proveedor_ID + " = '" + Datos.P_Proveedor_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Proveedor_ID + " = NULL, ";
                }

                if (Datos.P_Percepcion_Deduccion_ID != null)
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Percepcion_Deduccion_ID + " = '" + Datos.P_Percepcion_Deduccion_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Percepcion_Deduccion_ID + " = NULL, ";
                }

                if (Datos.P_Nomina_ID != null)
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Nomina_ID + " = '" + Datos.P_Nomina_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Nomina_ID + " = NULL, ";
                }

                if (Datos.P_No_Nomina != null)
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_No_Nomina + " = " + Datos.P_No_Nomina + ", ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_No_Nomina + " = NULL, ";
                }

                if (Datos.P_Estatus_Solicitud != null)
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Solicitud + " = '" + Datos.P_Estatus_Solicitud + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Solicitud + " = NULL, ";
                }

                if (Datos.P_Estatus_Pago != null)
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Pago + " = '" + Datos.P_Estatus_Pago + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Pago + " = NULL, ";
                }

                if (Datos.P_Fecha_Solicitud != null)
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Solicitud + " = '" + Datos.P_Fecha_Solicitud + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Solicitud + " = NULL, ";
                }

                if (Datos.P_Fecha_Inicio_Pago != null)
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Inicio_Pago + " = '" + Datos.P_Fecha_Inicio_Pago + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Inicio_Pago + " = NULL, ";
                }

                if (Datos.P_Fecha_Termino_Pago != null)
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Termino_Pago + " = '" + Datos.P_Fecha_Termino_Pago + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Termino_Pago + " = NULL, ";
                }

                if (Datos.P_Motivo_Prestamo != null)
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Motivo_Prestamo + " = '" + Datos.P_Motivo_Prestamo + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Motivo_Prestamo + " = NULL, ";
                }

                if (Datos.P_No_Pagos != null)
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_No_Pagos + " = " + Datos.P_No_Pagos + ", ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_No_Pagos + " = NULL, ";
                }

                if (Datos.P_Importe_Prestamo != null)
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Importe_Prestamo + " = " + Datos.P_Importe_Prestamo + ", ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Importe_Prestamo + " = NULL, ";
                }

                if (Datos.P_Importe_Interes != null)
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Importe_Interes + " = " + Datos.P_Importe_Interes + ", ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Importe_Interes + " = NULL, ";
                }

                if (Datos.P_Total_Prestamo != null)
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Total_Prestamo + " = " + Datos.P_Total_Prestamo + ", ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Total_Prestamo + " = NULL, ";
                }

                if (Datos.P_Saldo_Actual != null)
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual + " = " + Datos.P_Saldo_Actual + ", ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual + " = NULL, ";
                }

                if (Datos.P_Abono != null)
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Abono + " = " + Datos.P_Abono + ", ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Abono + " = NULL, ";
                }

                if (Datos.P_Aplica_Validaciones != null)
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Aplica_Validaciones + " = '" + Datos.P_Aplica_Validaciones + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Ope_Nom_Solicitud_Prestamo.Campo_Aplica_Validaciones + " = NULL, ";
                }

                Mi_SQL += Ope_Nom_Solicitud_Prestamo.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Modifico + "', " +
                   Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Modifico + "= SYSDATE" +
                   " WHERE " + Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud + "='" + Datos.P_No_Solicitud + "'";

                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
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
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
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
        /// NOMBRE DE LA FUNCION: Eliminar_Solicitud_Prestamo
        /// DESCRIPCION : Ejecuta la Baja de la solicitud del prestamo
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 30/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************************************************
        public static Boolean Eliminar_Solicitud_Prestamo(Cls_Ope_Nom_Pestamos_Negocio Datos) {
            String Mi_Oracle = "";
            Boolean Operacion_Completa = false;
            try
            {
                Mi_Oracle = "DELETE FROM " + Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo +
                  " WHERE " + Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud + "='" + Datos.P_No_Solicitud + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Operacion_Completa;
        }
        ///*******************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Autorizacion_Solicitudes_Prestamos
        /// DESCRIPCION : Ejecuta la Autorizacion de la solicitud del prestamo
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 30/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************************************************
        public static Boolean Autorizacion_Solicitudes_Prestamos(Cls_Ope_Nom_Pestamos_Negocio Datos) {
            String Mi_Oracle = "";
            Boolean Operacion_Completa = false;
            try
            {
                Mi_Oracle = "UPDATE " + Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo + " SET " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Solicitud + "='" + Datos.P_Estatus_Solicitud + "', " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Estado_Prestamo + "='PROCESO', " + 
                    Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Autorizacion + "= SYSDATE, " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Comentarios_Rechazo + "='" + Datos.P_Comentarios_Rechazo + "', " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Modifico + "', " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Modifico + "= SYSDATE" +
                    " WHERE " + Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud + "='" + Datos.P_No_Solicitud + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Operacion_Completa;
        }
        ///*******************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Cancelacion_Por_Liquidacion_Prestamos
        /// DESCRIPCION : Ejecuta la Cancelacion de del prestamo
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 30/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************************************************
        public static Boolean Cancelacion_Por_Liquidacion_Prestamos(Cls_Ope_Nom_Pestamos_Negocio Datos) {
            String Mi_Oracle = "";
            Boolean Operacion_Completa = false;
            try
            {
                Mi_Oracle = "UPDATE " + Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo + " SET " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Referencia_Recibo_Pago + "='" + Datos.P_Referencia_Recibo_Pago + "', " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Comentarios_Cancelacion+ "='" + Datos.P_Comentarios_Cancelacion + "', " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Estado_Prestamo + "='PAGADO', " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Finiquito_Prestamo + "= SYSDATE, " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Modifico + "', " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual + "=0, " +
                    Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Modifico + "= SYSDATE" +
                    " WHERE " + Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud + "='" + Datos.P_No_Solicitud + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Operacion_Completa;
        }
        ///*******************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Finiquitar_Prestamo
        /// DESCRIPCION : Finiquita el Ajuste de ISR del empleado.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 15/Junio/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************************************************
        public static void Finiquitar_Prestamo(Cls_Ope_Nom_Pestamos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta
            DataTable Dt_Prestamos = null;
            Double Total_Prestamo = 0;
            Int32 No_Pagos = 0;

            try
            {
                Mi_SQL.Append("SELECT " + Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo + ".* ");
                Mi_SQL.Append(" FROM " + Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo);
                Mi_SQL.Append(" WHERE " + Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud + "='" + Datos.P_No_Solicitud + "'");

                Dt_Prestamos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                Mi_SQL = new StringBuilder();

                if (Dt_Prestamos is DataTable)
                {
                    if (Dt_Prestamos.Rows.Count > 0)
                    {
                        foreach (DataRow PRESTAMO in Dt_Prestamos.Rows)
                        {
                            if (PRESTAMO is DataRow)
                            {

                                if (!String.IsNullOrEmpty(PRESTAMO[Ope_Nom_Solicitud_Prestamo.Campo_Total_Prestamo].ToString().Trim()))
                                    Total_Prestamo = Convert.ToDouble(PRESTAMO[Ope_Nom_Solicitud_Prestamo.Campo_Total_Prestamo].ToString().Trim());

                                if (!String.IsNullOrEmpty(PRESTAMO[Ope_Nom_Solicitud_Prestamo.Campo_No_Pagos].ToString().Trim()))
                                    No_Pagos = Convert.ToInt32(PRESTAMO[Ope_Nom_Solicitud_Prestamo.Campo_No_Pagos].ToString().Trim());

                                Mi_SQL.Append("UPDATE " + Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo);
                                Mi_SQL.Append(" SET " + Ope_Nom_Solicitud_Prestamo.Campo_Monto_Abonado + "=" + Total_Prestamo + ", ");
                                Mi_SQL.Append(Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual + "=" + 0 + ", ");
                                Mi_SQL.Append(Ope_Nom_Solicitud_Prestamo.Campo_No_Abono + "=" + No_Pagos + ", ");
                                Mi_SQL.Append(Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Finiquito_Prestamo + "=SYSDATE, ");
                                Mi_SQL.Append(Ope_Nom_Solicitud_Prestamo.Campo_Estado_Prestamo + "='PAGADO'");
                                Mi_SQL.Append(" WHERE " + Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud + "='" + Datos.P_No_Solicitud + "'");

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

        #region (Métodos Consulta)
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
        public static Cls_Ope_Nom_Pestamos_Negocio Consulta_Solicitudes_Prestamos(Cls_Ope_Nom_Pestamos_Negocio Datos) {
            String Mi_Oracle = "";
            Cls_Ope_Nom_Pestamos_Negocio Cls_Ope_Nom_Prestamos_Consultados = new Cls_Ope_Nom_Pestamos_Negocio();
            DataTable Dt_Solicitudes_Prestamo = null;
            String Fecha_Inicio = "";
            String Fecha_Fin = "";

            try
            {
                Fecha_Inicio = Datos.P_Fecha_Inicio_Pago;
                Fecha_Fin = Datos.P_Fecha_Termino_Pago;

                Mi_Oracle = "SELECT " + Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo + ".*, (" +
                    Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + "|| ' ' ||" +
                    Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "|| ' ' ||" +
                    Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ") As NOMBRE" +
                    " FROM " +
                    Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo + " INNER JOIN " + Cat_Empleados.Tabla_Cat_Empleados +
                    " ON " + Ope_Nom_Solicitud_Prestamo.Campo_Solicita_Empleado_ID + "=" + Cat_Empleados.Campo_Empleado_ID;

                if (!string.IsNullOrEmpty(Datos.P_No_Solicitud))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud + "='" + Datos.P_No_Solicitud + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud + "='" + Datos.P_No_Solicitud + "'";
                    }
                }
                if (!string.IsNullOrEmpty(Datos.P_Solicita_No_Empleado))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Solicitud_Prestamo.Campo_Solicita_Empleado_ID + " IN " +
                            "(SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_Solicita_No_Empleado + "')";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Solicitud_Prestamo.Campo_Solicita_Empleado_ID + " IN " +
                            "(SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_Solicita_No_Empleado + "')"; 
                    }
                }
                if (!string.IsNullOrEmpty(Datos.P_Aval_No_Empleado))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Solicitud_Prestamo.Campo_Aval_Empleado_ID + " IN " +
                            "(SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_Aval_No_Empleado + "')"; 
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Solicitud_Prestamo.Campo_Aval_Empleado_ID + " IN " +
                            "(SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_Aval_No_Empleado + "')"; 
                    }
                }
                if (!string.IsNullOrEmpty(Datos.P_RFC_Empleado_Solicita_Prestamo))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Solicitud_Prestamo.Campo_Solicita_Empleado_ID + " IN " +
                            "(SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC_Empleado_Solicita_Prestamo + "')"; 
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Solicitud_Prestamo.Campo_Solicita_Empleado_ID + " IN " +
                            "(SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC_Empleado_Solicita_Prestamo + "')";
                    }
                }
                if (!string.IsNullOrEmpty(Datos.P_RFC_Empleado_Aval_Prestamo))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Solicitud_Prestamo.Campo_Aval_Empleado_ID + " IN " +
                            "(SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC_Empleado_Aval_Prestamo + "')"; 
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Solicitud_Prestamo.Campo_Aval_Empleado_ID + " IN " +
                            "(SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC_Empleado_Aval_Prestamo + "')"; 
                    }
                }
                if (!string.IsNullOrEmpty(Datos.P_Estatus_Solicitud))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Solicitud + "='" + Datos.P_Estatus_Solicitud + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Solicitud + "='" + Datos.P_Estatus_Solicitud + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Estado_Prestamo))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Solicitud_Prestamo.Campo_Estado_Prestamo + "='" + Datos.P_Estado_Prestamo + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Solicitud_Prestamo.Campo_Estado_Prestamo + "='" + Datos.P_Estado_Prestamo + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Fecha_Inicio) && !string.IsNullOrEmpty(Fecha_Fin))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Solicitud + " BETWEEN TO_DATE ('" + Fecha_Inicio + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                         " AND TO_DATE ('" + Fecha_Fin + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Solicitud + " BETWEEN TO_DATE ('" + Fecha_Inicio + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                         " AND TO_DATE ('" + Fecha_Fin + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                }


                Dt_Solicitudes_Prestamo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
                if (Dt_Solicitudes_Prestamo != null)
                {
                    Cls_Ope_Nom_Prestamos_Consultados.P_Dt_Solicitudes_Prestamos = Dt_Solicitudes_Prestamo;
                    if (Dt_Solicitudes_Prestamo.Rows.Count > 0) {
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_No_Solicitud = Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud].ToString();
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Solicita_Empleado_ID].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Solicita_Empleado_ID = Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Solicita_Empleado_ID].ToString();
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Aval_Empleado_ID].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Aval_No_Empleado = Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Aval_Empleado_ID].ToString();
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Proveedor_ID].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Proveedor_ID= Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Proveedor_ID].ToString();
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Percepcion_Deduccion_ID].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Percepcion_Deduccion_ID= Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Percepcion_Deduccion_ID].ToString();
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Nomina_ID].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Nomina_ID = Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Nomina_ID].ToString();
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_No_Nomina].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_No_Nomina = Convert.ToInt32(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_No_Nomina].ToString());
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Solicitud].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Estatus_Solicitud = Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Solicitud].ToString();
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Pago].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Estatus_Pago = Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Pago].ToString();
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Solicitud].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Fecha_Solicitud = string.Format("{0:dd/MM/yyyy}", HttpUtility.HtmlDecode(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Solicitud].ToString()));                        
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Inicio_Pago].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Fecha_Inicio_Pago = string.Format("{0:dd/MM/yyyy}", HttpUtility.HtmlDecode(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Inicio_Pago].ToString()));
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Termino_Pago].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Fecha_Termino_Pago = string.Format("{0:dd/MM/yyyy}", HttpUtility.HtmlDecode(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Termino_Pago].ToString()));
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Motivo_Prestamo].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Motivo_Prestamo = Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Motivo_Prestamo].ToString();
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_No_Pagos].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_No_Pagos = Convert.ToInt32(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_No_Pagos].ToString());
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Importe_Prestamo].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Importe_Prestamo = Convert.ToDouble(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Importe_Prestamo].ToString());
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Importe_Interes].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Importe_Interes = Convert.ToDouble(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Importe_Interes].ToString());
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Total_Prestamo].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Total_Prestamo = Convert.ToDouble(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Total_Prestamo].ToString());
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Saldo_Actual = Convert.ToDouble(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual].ToString());
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Abono].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Abono = Convert.ToDouble(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Abono].ToString());
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_No_Abono].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_No_Abono = Convert.ToInt32(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_No_Abono].ToString());
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Comentarios_Cancelacion].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Comentarios_Cancelacion = Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Comentarios_Cancelacion].ToString();
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Comentarios_Rechazo].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Comentarios_Rechazo= Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Comentarios_Rechazo].ToString();
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Referencia_Recibo_Pago].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Referencia_Recibo_Pago = Dt_Solicitudes_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Referencia_Recibo_Pago].ToString();
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Cls_Ope_Nom_Prestamos_Consultados;
        }
        ///*******************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Prestamos
        /// DESCRIPCION : Consulta los prestamos vigentes en el sistema.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 30/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************************************************
        public static Cls_Ope_Nom_Pestamos_Negocio Consulta_Prestamos(Cls_Ope_Nom_Pestamos_Negocio Datos)
        {
            String Mi_Oracle = "";
            Cls_Ope_Nom_Pestamos_Negocio Cls_Ope_Nom_Prestamos_Consultados = new Cls_Ope_Nom_Pestamos_Negocio();
            DataTable Dt_Prestamo = null;
            try
            {
              Mi_Oracle= "SELECT " + Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo + ".* FROM " +
                       Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo + 
                       " WHERE " +
                       Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo + "." + Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud + "='" + Datos.P_No_Solicitud + "'" +
                       " OR " +
                       Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo + "." + Ope_Nom_Solicitud_Prestamo.Campo_Solicita_Empleado_ID + " IN " +
                       "(SELECT " +  Cat_Empleados.Campo_Empleado_ID + " FROM " +  Cat_Empleados.Tabla_Cat_Empleados + " WHERE " +
                       Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_Solicita_No_Empleado + "')"+
                       " OR " +
                       Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo + "." + Ope_Nom_Solicitud_Prestamo.Campo_Aval_Empleado_ID + " IN " +
                       "(SELECT " +  Cat_Empleados.Campo_Empleado_ID + " FROM " +  Cat_Empleados.Tabla_Cat_Empleados + " WHERE " +
                       Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_Aval_No_Empleado + "')";


                Dt_Prestamo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];

                if (Dt_Prestamo != null)
                {
                    Cls_Ope_Nom_Prestamos_Consultados.P_Dt_Prestamos = Dt_Prestamo;
                    if (Dt_Prestamo.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_No_Solicitud = Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud].ToString();
                        if (!string.IsNullOrEmpty(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Solicita_Empleado_ID].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Solicita_Empleado_ID = Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Solicita_Empleado_ID].ToString();
                        if (!string.IsNullOrEmpty(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Aval_Empleado_ID].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Aval_No_Empleado = Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Aval_Empleado_ID].ToString();
                        if (!string.IsNullOrEmpty(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Proveedor_ID].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Proveedor_ID = Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Proveedor_ID].ToString();
                        if (!string.IsNullOrEmpty(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Percepcion_Deduccion_ID].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Percepcion_Deduccion_ID = Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Percepcion_Deduccion_ID].ToString();
                        if (!string.IsNullOrEmpty(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Nomina_ID].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Nomina_ID = Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Nomina_ID].ToString();
                        if (!string.IsNullOrEmpty(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_No_Nomina].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_No_Nomina = Convert.ToInt32(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_No_Nomina].ToString());
                        if (!string.IsNullOrEmpty(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Solicitud].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Estatus_Solicitud = Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Solicitud].ToString();
                        if (!string.IsNullOrEmpty(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Pago].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Estatus_Pago = Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Pago].ToString();
                        if (!string.IsNullOrEmpty(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Solicitud].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Fecha_Solicitud = string.Format("{0:dd/MM/yyyy}", HttpUtility.HtmlDecode(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Solicitud].ToString()));
                        if (!string.IsNullOrEmpty(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Autorizacion].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Fecha_Autorizacion = string.Format("{0:dd/MM/yyyy}", HttpUtility.HtmlDecode(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Autorizacion].ToString()));
                        if (!string.IsNullOrEmpty(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Inicio_Pago].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Fecha_Inicio_Pago = string.Format("{0:dd/MM/yyyy}", HttpUtility.HtmlDecode(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Inicio_Pago].ToString()));
                        if (!string.IsNullOrEmpty(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Termino_Pago].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Fecha_Termino_Pago = string.Format("{0:dd/MM/yyyy}", HttpUtility.HtmlDecode(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Termino_Pago].ToString()));
                        if (!string.IsNullOrEmpty(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Motivo_Prestamo].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Motivo_Prestamo = Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Motivo_Prestamo].ToString();
                        if (!string.IsNullOrEmpty(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_No_Pagos].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_No_Pagos = Convert.ToInt32(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_No_Pagos].ToString());
                        if (!string.IsNullOrEmpty(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Importe_Prestamo].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Importe_Prestamo = Convert.ToDouble(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Importe_Prestamo].ToString());
                        if (!string.IsNullOrEmpty(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Importe_Interes].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Importe_Interes = Convert.ToDouble(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Importe_Interes].ToString());
                        if (!string.IsNullOrEmpty(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Total_Prestamo].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Total_Prestamo = Convert.ToDouble(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Total_Prestamo].ToString());                        
                        if (!string.IsNullOrEmpty(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Monto_Abonado].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Monto_Abonado = Convert.ToDouble(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Monto_Abonado].ToString());
                        if (!string.IsNullOrEmpty(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Saldo_Actual = Convert.ToDouble(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual].ToString());
                        if (!string.IsNullOrEmpty(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Abono].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Abono = Convert.ToDouble(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Abono].ToString());
                        if (!string.IsNullOrEmpty(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_No_Abono].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_No_Abono = Convert.ToInt32(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_No_Abono].ToString());
                        if (!string.IsNullOrEmpty(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Comentarios_Cancelacion].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Comentarios_Cancelacion = Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Comentarios_Cancelacion].ToString();
                        if (!string.IsNullOrEmpty(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Comentarios_Rechazo].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Comentarios_Rechazo = Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Comentarios_Rechazo].ToString();
                        if (!string.IsNullOrEmpty(Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Referencia_Recibo_Pago].ToString())) Cls_Ope_Nom_Prestamos_Consultados.P_Referencia_Recibo_Pago = Dt_Prestamo.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Referencia_Recibo_Pago].ToString();
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Cls_Ope_Nom_Prestamos_Consultados;
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
        public static DataTable Consultar_Fecha_Inicio_Periodo_Pago(Cls_Ope_Nom_Pestamos_Negocio Datos)
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
        public static DataTable Consultar_Fechas_Periodo(Cls_Ope_Nom_Pestamos_Negocio Datos)
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
        /// NOMBRE DE LA FUNCION: Consultar_Tipo_Nomina_Empleado_Solicitante
        /// DESCRIPCION : Consulta el tipo de nomina al que pertence el empleado.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 22/Marzo/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************************************************
        public static String Consultar_Tipo_Nomina_Empleado_Solicitante(Cls_Ope_Nom_Pestamos_Negocio Datos)
        {
            String Mi_SQL = "";         //Variable que almacenara la consulta.
            String Tipo_Nomina_ID = ""; //Variable el nombre del tipo de nomina.
            DataTable Dt_Empleado = null;//Variable que almacena la informacion del tipo de nomina.

            try
            {
                Mi_SQL = "SELECT " + Cat_Empleados.Campo_Tipo_Nomina_ID + 
                         " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " +
                         Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado  + "'";

                Dt_Empleado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                if (Dt_Empleado is DataTable) {
                    if (Dt_Empleado.Rows.Count > 0) {
                        foreach (DataRow Tipo_Nomina in Dt_Empleado.Rows) {
                            if (Tipo_Nomina is DataRow) {
                                if (!string.IsNullOrEmpty(Tipo_Nomina[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString())) {
                                    Tipo_Nomina_ID = Tipo_Nomina[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar el tipo de nomina a la que pertence el empleado. Error: [" + Ex.Message + "]");
            }
            return Tipo_Nomina_ID;
        }
        ///*******************************************************************************************************************
        /// Nombre: Consulta_Reporte_Prestamos
        /// 
        /// Descripción : Consulta los préstamos registrados en el sistema según los filtros seleccionados.
        /// 
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// 
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : Enero/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************************************************
        public static DataTable Consulta_Reporte_Prestamos(Cls_Ope_Nom_Pestamos_Negocio Datos)
        {
            Cls_Ope_Nom_Pestamos_Negocio Cls_Ope_Nom_Prestamos_Consultados = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexión con la capa de negocios.
            StringBuilder Mi_SQL = new StringBuilder();//variable que almacenara la consulta.
            String Mi_Oracle = "";//Variable que almacena las condiciones de la consulta.           
            DataTable Dt_Solicitudes_Prestamo = null;//Variable que listara los prestamos.
            String Fecha_Inicio = "";//Variable que almacenara la fecha de inicio de pagos de los préstamos.
            String Fecha_Fin = "";//Variable que almacenara la fecha de fin de los préstamos.

            try
            {
                Fecha_Inicio = Datos.P_Fecha_Inicio_Pago;//Obtenemos la fecha de inicio del los préstamos a consultar.
                Fecha_Fin = Datos.P_Fecha_Termino_Pago;//Consultamos la fecha de fin de los préstamos a consultar.

                Mi_SQL.Append(" SELECT ");

                Mi_SQL.Append("(SELECT (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno +  "  || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") from ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + " where ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "=");
                Mi_SQL.Append(Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo + "." + Ope_Nom_Solicitud_Prestamo.Campo_Solicita_Empleado_ID + ") as SOLICITANTE, ");

                Mi_SQL.Append("(SELECT (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "  || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") from ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + " where ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "=");
                Mi_SQL.Append(Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo + "." + Ope_Nom_Solicitud_Prestamo.Campo_Aval_Empleado_ID + ") as AVAL, ");

                Mi_SQL.Append("(select (" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " || ' ' || ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ") from ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + " where ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " in ");
                Mi_SQL.Append("(select " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + " from ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + " where ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "=");
                Mi_SQL.Append(Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo + "." + Ope_Nom_Solicitud_Prestamo.Campo_Solicita_Empleado_ID + ")) as UR_SOLICITANTE, ");

                Mi_SQL.Append("(select (" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " || ' ' || ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ") from ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + " where ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " in ");
                Mi_SQL.Append("(select " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + " from ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + " where ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "=");
                Mi_SQL.Append(Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo + "." + Ope_Nom_Solicitud_Prestamo.Campo_Aval_Empleado_ID + ")) as UR_AVAL, ");

                Mi_SQL.Append("(select " + Cat_Nom_Proveedores.Tabla_Cat_Nom_Proveedores + "." + Cat_Nom_Proveedores.Campo_Nombre);
                Mi_SQL.Append(" from " + Cat_Nom_Proveedores.Tabla_Cat_Nom_Proveedores + " where ");
                Mi_SQL.Append(Cat_Nom_Proveedores.Tabla_Cat_Nom_Proveedores + "." + Cat_Nom_Proveedores.Campo_Proveedor_ID + " = ");
                Mi_SQL.Append(Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo + "." + Ope_Nom_Solicitud_Prestamo.Campo_Proveedor_ID + ") as PROVEEDOR, ");

                Mi_SQL.Append("(select (" + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave + " || ' ' || ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ") from ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " where ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + " = ");
                Mi_SQL.Append(Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo + "." + Ope_Nom_Solicitud_Prestamo.Campo_Percepcion_Deduccion_ID+ ") as DEDUCCION, ");

                Mi_SQL.Append("(select " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Anio);
                Mi_SQL.Append(" from " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + " where ");
                Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID + " = ");
                Mi_SQL.Append(Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo + "." + Ope_Nom_Solicitud_Prestamo.Campo_Nomina_ID+ ") as NOMINA, ");

                Mi_SQL.Append(Ope_Nom_Solicitud_Prestamo.Campo_No_Nomina + ", ");
                Mi_SQL.Append(Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Solicitud + ", ");
                Mi_SQL.Append(Ope_Nom_Solicitud_Prestamo.Campo_Estado_Prestamo + ", ");
                Mi_SQL.Append(" to_char(" + Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Solicitud + ", 'yyyy/mm/dd') as FECHA_SOLICITUD, ");
                Mi_SQL.Append(" to_char(" + Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Autorizacion + ", 'yyyy/mm/dd') as FECHA_AUTORIZACION, ");
                Mi_SQL.Append(" to_char(" + Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Inicio_Pago + ", 'yyyy/mm/dd') as FECHA_INICIO_PAGOS, ");
                Mi_SQL.Append(" to_char(" + Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Termino_Pago + ", 'yyyy/mm/dd') as FECHA_TERMINO_PAGOS, ");
                Mi_SQL.Append(Ope_Nom_Solicitud_Prestamo.Campo_Motivo_Prestamo + ", ");
                Mi_SQL.Append(" nvl(" + Ope_Nom_Solicitud_Prestamo.Campo_No_Pagos + ", 0) as NO_PAGOS, ");
                Mi_SQL.Append(" nvl(" + Ope_Nom_Solicitud_Prestamo.Campo_Importe_Prestamo + ", 0) as IMPORTE_PRESTAMO, ");
                Mi_SQL.Append(" nvl(" + Ope_Nom_Solicitud_Prestamo.Campo_Importe_Interes + ", 0) as IMPORTE_INTERES,");
                Mi_SQL.Append(" nvl(" + Ope_Nom_Solicitud_Prestamo.Campo_Total_Prestamo + ", 0) as TOTAL_PRESTAMO, ");
                Mi_SQL.Append(" nvl(" + Ope_Nom_Solicitud_Prestamo.Campo_Monto_Abonado + ", 0) as MONTO_ABONADO, ");
                Mi_SQL.Append(" nvl(" + Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual + ", 0) as SALDO_ACTUAL, ");
                Mi_SQL.Append(" nvl(" + Ope_Nom_Solicitud_Prestamo.Campo_Abono + ", 0) as ABONO, ");
                Mi_SQL.Append(" nvl(" + Ope_Nom_Solicitud_Prestamo.Campo_No_Abono + ", 0) as NO_ABONO ");
                

                Mi_SQL.Append(" from " + Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo + " ");

                if (!string.IsNullOrEmpty(Datos.P_No_Solicitud))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud + "='" + Datos.P_No_Solicitud + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud + "='" + Datos.P_No_Solicitud + "'";
                    }
                }
                if (!string.IsNullOrEmpty(Datos.P_Solicita_No_Empleado))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Solicitud_Prestamo.Campo_Solicita_Empleado_ID + " IN " +
                            "(SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_Solicita_No_Empleado + "')";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Solicitud_Prestamo.Campo_Solicita_Empleado_ID + " IN " +
                            "(SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_Solicita_No_Empleado + "')";
                    }
                }
                if (!string.IsNullOrEmpty(Datos.P_Aval_No_Empleado))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Solicitud_Prestamo.Campo_Aval_Empleado_ID + " IN " +
                            "(SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_Aval_No_Empleado + "')";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Solicitud_Prestamo.Campo_Aval_Empleado_ID + " IN " +
                            "(SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_Aval_No_Empleado + "')";
                    }
                }
                if (!string.IsNullOrEmpty(Datos.P_RFC_Empleado_Solicita_Prestamo))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Solicitud_Prestamo.Campo_Solicita_Empleado_ID + " IN " +
                            "(SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC_Empleado_Solicita_Prestamo + "')";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Solicitud_Prestamo.Campo_Solicita_Empleado_ID + " IN " +
                            "(SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC_Empleado_Solicita_Prestamo + "')";
                    }
                }
                if (!string.IsNullOrEmpty(Datos.P_RFC_Empleado_Aval_Prestamo))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Solicitud_Prestamo.Campo_Aval_Empleado_ID + " IN " +
                            "(SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC_Empleado_Aval_Prestamo + "')";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Solicitud_Prestamo.Campo_Aval_Empleado_ID + " IN " +
                            "(SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC_Empleado_Aval_Prestamo + "')";
                    }
                }
                if (!string.IsNullOrEmpty(Datos.P_Estatus_Solicitud))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND upper(" + Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Solicitud + ")=upper('" + Datos.P_Estatus_Solicitud + "')";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE upper(" + Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Solicitud + ")=upper('" + Datos.P_Estatus_Solicitud + "')";
                    }
                }

                if (Datos.P_Total_Prestamo > 0)
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Solicitud_Prestamo.Campo_Total_Prestamo + "=" + Datos.P_Total_Prestamo;
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Solicitud_Prestamo.Campo_Total_Prestamo + "=" + Datos.P_Total_Prestamo;
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Estado_Prestamo))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND upper(" + Ope_Nom_Solicitud_Prestamo.Campo_Estado_Prestamo + ")=upper('" + Datos.P_Estado_Prestamo + "')";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE upper(" + Ope_Nom_Solicitud_Prestamo.Campo_Estado_Prestamo + ")=upper('" + Datos.P_Estado_Prestamo + "')";
                    }
                }

                if (!string.IsNullOrEmpty(Fecha_Inicio) && !string.IsNullOrEmpty(Fecha_Fin))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Solicitud + " BETWEEN TO_DATE ('" + Fecha_Inicio + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                         " AND TO_DATE ('" + Fecha_Fin + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Solicitud + " BETWEEN TO_DATE ('" + Fecha_Inicio + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                         " AND TO_DATE ('" + Fecha_Fin + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                }

                //Se crea la consulta la parte de los datos a mostrar y las condiciones que deben cumplir.
                Mi_Oracle = Mi_SQL.ToString() + Mi_Oracle;

                Dt_Solicitudes_Prestamo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Dt_Solicitudes_Prestamo;
        }
        #endregion

        #endregion
    }
}
