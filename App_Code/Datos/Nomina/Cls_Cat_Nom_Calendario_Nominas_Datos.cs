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
using Presidencia.Calendario_Nominas.Negocios;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Globalization;
using System.Text;

namespace Presidencia.Calendario_Nominas.Datos
{
    public class Cls_Cat_Nom_Calendario_Nominas_Datos
    {
        #region (Metodos)

        #region (Metodos Operacion)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Calendario_Nomina
        /// DESCRIPCION : Alta de un Calendario de Nomina
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 19/Octubre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Alta_Calendario_Nomina(Cls_Cat_Nom_Calendario_Nominas_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();         //Variable que almacenara la consulta.
            OracleTransaction Transaccion = null;               //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            OracleConnection Conexion;                          //Variable para la conexión para la base de datos   
            OracleCommand Comando;                              //Sirve para la ejecución de las operaciones a la base de datos
            Object Nomina_ID;                                   //Identificador unico de la tabla de bancos.
            String Mensaje = String.Empty;                      //Variable que almacena el mensaje de estado de la operación
            Boolean Operacion_Completa = false;                 //Estatus de la operacion de alta.

            Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
            Comando = new OracleCommand();
            Conexion.Open();
            Transaccion = Conexion.BeginTransaction();
            Comando.Transaction = Transaccion;
            Comando.Connection = Conexion;

            try
            {
                //Consultas para el ID
                Mi_SQL.Append("SELECT NVL(MAX(" + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID + "), '00000') FROM " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas);

                //Ejecutar consulta
                Comando.CommandText = Mi_SQL.ToString();
                Nomina_ID = Comando.ExecuteScalar();

                //Verificar si no es nulo
                if (Convert.IsDBNull(Nomina_ID) == false)
                    Datos.P_Nomina_ID = String.Format("{0:00000}", Convert.ToInt32(Nomina_ID) + 1);
                else
                    Datos.P_Nomina_ID = "00001";

                Mi_SQL = new StringBuilder();

                Mi_SQL.Append("INSERT INTO " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + " (");
                Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Campo_Nomina_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Campo_Fecha_Inicio + ", ");
                Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin + ", ");
                Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Campo_Usuario_Creo + ", ");
                Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Campo_Fecha_Creo + ", ");
                Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Campo_Anio + ") VALUES(");
                Mi_SQL.Append("'" + Datos.P_Nomina_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_Fecha_Inicio + "', ");
                Mi_SQL.Append("'" + Datos.P_Fecha_Fin + "', ");
                Mi_SQL.Append("'" + Datos.P_Usuario_Creo + "', ");
                Mi_SQL.Append(" SYSDATE, ");
                Mi_SQL.Append(Datos.P_Anio + ")");

                //Ejecutar consulta
                Comando.CommandText = Mi_SQL.ToString();
                Comando.ExecuteNonQuery();

                foreach (DataRow row in Datos.P_Dt_Periodos_Pago.Rows)
                {

                    Mi_SQL = new StringBuilder();

                    Mi_SQL.Append("SELECT NVL(MAX(" + Cat_Nom_Nominas_Detalles.Campo_Detalle_Nomina_ID + "),'00000') ");
                    Mi_SQL.Append("FROM " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles);

                    Comando.CommandText = Mi_SQL.ToString();
                    object Detalle_Nomina_ID = Comando.ExecuteScalar();

                    Mi_SQL = new StringBuilder();
                    String Detalle_Nomina_ID_Corrida = "";

                    if (Convert.IsDBNull(Detalle_Nomina_ID))
                    {
                        Detalle_Nomina_ID_Corrida = "00001";
                    }
                    else
                    {
                        Detalle_Nomina_ID_Corrida = String.Format("{0:00000}", Convert.ToInt32(Detalle_Nomina_ID) + 1);
                    }

                    Mi_SQL.Append("INSERT INTO " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + " (");
                    Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Campo_Detalle_Nomina_ID + ", ");
                    Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Campo_Nomina_ID + ", ");
                    Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Campo_No_Nomina + ", ");
                    Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio + ", ");
                    Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin + ", ");
                    Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Campo_Estatus + ", ");
                    Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Campo_Usuario_Creo + ", ");
                    Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Campo_Fecha_Creo);
                    Mi_SQL.Append(") VALUES ('");
                    Mi_SQL.Append(Detalle_Nomina_ID_Corrida + "', '");
                    Mi_SQL.Append(Datos.P_Nomina_ID + "', ");
                    Mi_SQL.Append(Convert.ToInt32(row[Cat_Nom_Nominas_Detalles.Campo_No_Nomina].ToString()) + ", '");
                    Mi_SQL.Append(String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(row[Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString())) + "', '");
                    Mi_SQL.Append(String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(row[Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString())) + "', '");
                    Mi_SQL.Append(row[Cat_Nom_Nominas_Detalles.Campo_Estatus].ToString() + "', '");
                    Mi_SQL.Append(Datos.P_Usuario_Creo + "', ");
                    Mi_SQL.Append("SYSDATE)");

                    //Ejecutar consulta
                    Comando.CommandText = Mi_SQL.ToString();
                    Comando.ExecuteNonQuery();
                }

                //Ejecutar transaccion
                Transaccion.Commit();
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
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Calendario_Nomina
        /// DESCRIPCION : Actualizar de un Calendario de Nomina
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 20/Octubre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Actualizar_Calendario_Nomina(Cls_Cat_Nom_Calendario_Nominas_Negocio Datos)
        {
            String Mi_SQL;  //Obtiene la cadena de inserción hacía la base de datos
            Boolean Operacion_Completa = false;

            try
            {
                //Actualizar los datos del Calendario de Nomina
                Mi_SQL = "UPDATE " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + " SET " +
                    Cat_Nom_Calendario_Nominas.Campo_Anio + "=" + Datos.P_Anio + ", " +
                    Cat_Nom_Calendario_Nominas.Campo_Fecha_Inicio + "='" + Datos.P_Fecha_Inicio + "', " +
                    Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin + "='" + Datos.P_Fecha_Fin + "', " +
                    Cat_Nom_Calendario_Nominas.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Modico + "', " +
                    Cat_Nom_Calendario_Nominas.Campo_Fecha_Modifico + "= SYSDATE " +
                    " WHERE " + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                //Eliminar los detalles de Nomina generados.
                Mi_SQL = "DELETE FROM " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles +
                        " WHERE " + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Operacion_Completa = true;

                //Dar de Alta los nuevos Detalles de Nomina
                foreach (DataRow row in Datos.P_Dt_Periodos_Pago.Rows)
                {
                    Mi_SQL = "SELECT NVL(MAX(" + Cat_Nom_Nominas_Detalles.Campo_Detalle_Nomina_ID + "),'00000') ";
                    Mi_SQL = Mi_SQL + "FROM " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles;
                    object Detalle_Nomina_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    String Detalle_Nomina_ID_Corrida = "";

                    if (Convert.IsDBNull(Detalle_Nomina_ID))
                    {
                        Detalle_Nomina_ID_Corrida = "00001";
                    }
                    else
                    {
                        Detalle_Nomina_ID_Corrida = String.Format("{0:00000}", Convert.ToInt32(Detalle_Nomina_ID) + 1);
                    }

                    Mi_SQL = "INSERT INTO " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + " (" +
                    Cat_Nom_Nominas_Detalles.Campo_Detalle_Nomina_ID + ", " +
                    Cat_Nom_Nominas_Detalles.Campo_Nomina_ID + ", " +
                    Cat_Nom_Nominas_Detalles.Campo_No_Nomina + ", " +
                    Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio + ", " +
                    Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin + ", " +
                    Cat_Nom_Nominas_Detalles.Campo_Estatus + ", " +
                    Cat_Nom_Nominas_Detalles.Campo_Usuario_Creo + ", " +
                    Cat_Nom_Nominas_Detalles.Campo_Fecha_Creo +
                    ") VALUES ('" +
                    Detalle_Nomina_ID_Corrida + "', '" +
                    Datos.P_Nomina_ID + "', " +
                    Convert.ToInt32(row[Cat_Nom_Nominas_Detalles.Campo_No_Nomina].ToString()) + ", '" +
                    String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(row[Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString())) + "', '" +
                    String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(row[Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString())) + "', '" +
                    row[Cat_Nom_Nominas_Detalles.Campo_Estatus].ToString() + "', '" +
                    Datos.P_Usuario_Creo + "', " +
                    "SYSDATE)";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                //Avisar que la operacion se realizo con exito
                Operacion_Completa = true;
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
            return Operacion_Completa;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Eliminar_Calendario_Nomina
        /// DESCRIPCION : Ejecuta la baja de un Calendario de Nomina
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 20/Octubre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Eliminar_Calendario_Nomina(Cls_Cat_Nom_Calendario_Nominas_Negocio Datos)
        {
            String Mi_SQL;  //Obtiene la cadena de inserción hacía la base de datos
            Boolean Operacion_Completa = false;

            try
            {
                Mi_SQL = "DELETE FROM " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles +
                " WHERE " + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Operacion_Completa = true;

                //Consulta para la inserción del área con los datos proporcionados por el usuario
                Mi_SQL = "DELETE FROM " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas +
                    " WHERE " + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Operacion_Completa = true;
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
            return Operacion_Completa;
        }


        public static Boolean Alta_Cierre_Periodo_Nomina(Cls_Cat_Nom_Calendario_Nominas_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable almacena la consulta.
            Boolean Operacion_Completa = false;//Variable que guarda el estatus si la operacion se completo.

            try
            {
                Mi_SQL.Append("INSERT INTO  " + Cat_Nom_Periodo_Tipo_Nom.Tabla_Cat_Nom_Periodo_Tipo_Nom);
                Mi_SQL.Append(" (" + Cat_Nom_Periodo_Tipo_Nom.Campo_Detalle_Nomina_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Periodo_Tipo_Nom.Campo_Tipo_Nomina_ID + ") VALUES(");
                Mi_SQL.Append("'" + Datos.P_Detalle_Nomina_ID + "', '" + Datos.P_Tipo_Nomina_ID + "')");

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al cambiar el estatus del peiodo de pago. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }

        public static DataTable Consulta_Periodo_Cerrado(Cls_Cat_Nom_Calendario_Nominas_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable almacena la consulta.
            DataTable Dt_Periodo_Tipo_Nomina = null;//Variable que guarda el estatus si la operacion se completo.

            try
            {
                Mi_SQL.Append("SELECT " + Cat_Nom_Periodo_Tipo_Nom.Tabla_Cat_Nom_Periodo_Tipo_Nom + ".*");
                Mi_SQL.Append(" FROM " + Cat_Nom_Periodo_Tipo_Nom.Tabla_Cat_Nom_Periodo_Tipo_Nom);
                Mi_SQL.Append(" WHERE " + Cat_Nom_Periodo_Tipo_Nom.Campo_Detalle_Nomina_ID + "='" + Datos.P_Detalle_Nomina_ID + "'");
                Mi_SQL.Append(" AND " + Cat_Nom_Periodo_Tipo_Nom.Campo_Tipo_Nomina_ID + "='" + Datos.P_Tipo_Nomina_ID + "'");

                Dt_Periodo_Tipo_Nomina = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al cambiar el estatus del peiodo de pago. Error: [" + Ex.Message + "]");
            }
            return Dt_Periodo_Tipo_Nomina;
        }
        #endregion

        #region (Metodos Consulta)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Calendario_Nominas
        /// DESCRIPCION : Consulta de la base de Datos todos los Calendarios de las Nominas
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 20/Octubre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Calendario_Nominas()
        {
            String Mi_SQL = "SELECT " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + ".* " +
                "FROM " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + " ORDER BY " + Cat_Nom_Calendario_Nominas.Campo_Fecha_Inicio;

            DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            DataTable tabla;
            if (dataset == null)
            {
                tabla = new DataTable();
            }
            else
            {
                tabla = dataset.Tables[0];
            }
            return tabla;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Detalles_Nomina
        /// DESCRIPCION : Consulta los detalles de la nomina seleccionada
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 20/Octubre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Detalles_Nomina(Cls_Cat_Nom_Calendario_Nominas_Negocio Datos)
        {
            String Mi_SQL = "SELECT " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + ".* " +
                "FROM " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + " WHERE " + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID + "='" +
                Datos.P_Nomina_ID + "'" +
                " ORDER BY " + Cat_Nom_Calendario_Nominas.Campo_Fecha_Inicio;

            DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            DataTable tabla;
            if (dataset == null)
            {
                tabla = new DataTable();
            }
            else
            {
                tabla = dataset.Tables[0];
            }
            return tabla;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Periodos_Nomina
        /// DESCRIPCION : Consulta los Periodos de la nomina.
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 14/Diciembre/2010 5:04 pm.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Periodos_Nomina(Cls_Cat_Nom_Calendario_Nominas_Negocio Datos){
            String Mi_Oracle = "";//Variable que almacenara la consulta.
            DataTable Dt_Periodos_Nomina = null;//Variable que almacenra los periodos de la nomina.

            try
            {
                Mi_Oracle = "SELECT " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + ".* " +
                    " FROM " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + " WHERE " +
                    Cat_Nom_Nominas_Detalles.Campo_No_Nomina + "=" + Datos.P_No_Nomina +
                    " AND " + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'";

                Dt_Periodos_Nomina = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
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
            return Dt_Periodos_Nomina;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Calendario_Nomina_Fecha_Actual
        /// DESCRIPCION : Consulta el calendario de nomina por fecha.
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 3/Marzo/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Calendario_Nomina_Fecha_Actual()
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable que almacenara la consulta.
            DataTable Dt_Calendario_Nomina = null;      //Variable que almacenara el registro del calendario de nomina consultado.

            try
            {
                Mi_SQL.Append(" SELECT " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + ".*");
                Mi_SQL.Append(" FROM " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas);
                Mi_SQL.Append(" WHERE (SYSDATE >= " + Cat_Nom_Calendario_Nominas.Campo_Fecha_Inicio);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(" SYSDATE <= " + Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin + ")");

                Dt_Calendario_Nomina = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar el calendario de nomina por fecha. Error: [" + Ex.Message + "]");
            }
            return Dt_Calendario_Nomina;
        }

        internal static DataTable Consultar_Periodos_Por_Nomina_Periodo(Cls_Cat_Nom_Calendario_Nominas_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable que almacenara la consulta.
            DataTable Dt_Detalle_Nomina = null;      //Variable que almacenara el registro del calendario de nomina consultado.

            try
            {
                Mi_SQL.Append(" SELECT " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + ".*");
                Mi_SQL.Append(" FROM " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Campo_No_Nomina + "=" + Datos.P_No_Nomina);

                Dt_Detalle_Nomina = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar el calendario de nomina por fecha. Error: [" + Ex.Message + "]");
            }
            return Dt_Detalle_Nomina;
        }

        internal static DataTable Consulta_Detalle_Periodo_Actual(Cls_Cat_Nom_Calendario_Nominas_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Detalle_Periodo_Nomina = null;

            try
            {
                Mi_SQL.Append("SELECT " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + ".*");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append("'" + Datos.P_Fecha_Busqueda_Periodo + "' >= " + Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append("'" + Datos.P_Fecha_Busqueda_Periodo + "' <= " + Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin);

                Dt_Detalle_Periodo_Nomina = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los detalles del periodo actual. Error: [" + Ex.Message + "]");
            }
            return Dt_Detalle_Periodo_Nomina;
        }
        #endregion

        #endregion
    }
}