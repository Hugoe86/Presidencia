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
using Presidencia.Turnos.Negocios;

/// <summary>
/// Summary description for Cls_Cat_Turnos_Datos
/// </summary>
namespace Presidencia.Turnos.Datos
{
    public class Cls_Cat_Turnos_Datos
    {
        public Cls_Cat_Turnos_Datos()
        {
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Turnos
        /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta el Turno en la BD con los datos proporcionados por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 25-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Turnos(Cls_Cat_Turnos_Negocio Datos)
        {
            String Mi_SQL;   //Obtiene la cadena de inserción hacía la base de datos
            Object Turno_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos

            try
            {
                Mi_SQL = "SELECT NVL(MAX(" + Cat_Turnos.Campo_Turno_ID + "),'00000') ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Turnos.Tabla_Cat_Turnos;
                Turno_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Turno_ID))
                {
                    Datos.P_Turno_ID = "00001";
                }
                else
                {
                    Datos.P_Turno_ID = String.Format("{0:00000}", Convert.ToInt32(Turno_ID) + 1);
                }
                //Consulta para la inserción del turno con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Cat_Turnos.Tabla_Cat_Turnos + " (";
                Mi_SQL = Mi_SQL + Cat_Turnos.Campo_Turno_ID + ", " + Cat_Turnos.Campo_Descripcion + ", ";
                Mi_SQL = Mi_SQL + Cat_Turnos.Campo_Hora_Entrada + ", " + Cat_Turnos.Campo_Hora_Salida + ", ";
                Mi_SQL = Mi_SQL + Cat_Turnos.Campo_Estatus + ", " + Cat_Turnos.Campo_Comentarios + ", ";
                Mi_SQL = Mi_SQL + Cat_Turnos.Campo_Usuario_Creo + ", " + Cat_Turnos.Campo_Fecha_Creo + ") VALUES ('";
                Mi_SQL = Mi_SQL + Datos.P_Turno_ID + "', '" + Datos.P_Descripcion + "', ";
                Mi_SQL = Mi_SQL + "TO_TIMESTAMP ('" + String.Format("{0:HH:mm:ss}", Datos.P_Hora_Entrada) + "', 'HH24: MI: SS'), ";
                Mi_SQL = Mi_SQL + "TO_TIMESTAMP('" + String.Format("{0:HH:mm:ss}", Datos.P_Hora_Salida) + "', 'HH24: MI: SS'), '";
                Mi_SQL = Mi_SQL + Datos.P_Estatus + "', '" + Datos.P_Comentarios + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Nombre_Usuario + "', SYSDATE)";
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
            finally
            {
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Turnos
        /// DESCRIPCION : Modifica los datos del turno con lo que fueron introducidos por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán modificados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 25-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Modificar_Turnos(Cls_Cat_Turnos_Negocio Datos)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos

            try
            {
                //Consulta para la modificación del turno con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Cat_Turnos.Tabla_Cat_Turnos + " SET ";
                Mi_SQL = Mi_SQL + Cat_Turnos.Campo_Descripcion + " = '" + Datos.P_Descripcion + "', ";
                Mi_SQL = Mi_SQL + Cat_Turnos.Campo_Hora_Entrada + " = TO_TIMESTAMP ('" + String.Format("{0:HH:mm:ss}", Datos.P_Hora_Entrada) + "', 'HH24: MI: SS'), ";
                Mi_SQL = Mi_SQL + Cat_Turnos.Campo_Hora_Salida + " = TO_TIMESTAMP('" + String.Format("{0:HH:mm:ss}", Datos.P_Hora_Salida) + "', 'HH24: MI: SS'), ";
                Mi_SQL = Mi_SQL + Cat_Turnos.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                Mi_SQL = Mi_SQL + Cat_Turnos.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + Cat_Turnos.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL = Mi_SQL + Cat_Turnos.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                Mi_SQL = Mi_SQL + Cat_Turnos.Campo_Turno_ID + " = '" + Datos.P_Turno_ID + "'";
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
            finally
            {
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Elimina_Turno
        /// DESCRIPCION : Elimina el turno que fue seleccionada por el usuario de la BD
        /// PARAMETROS  : Datos: Obtiene que área desea eliminar de la BD
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 25-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Elimina_Turno(Cls_Cat_Turnos_Negocio Datos)
        {
            String Mi_SQL; //Variable de Consulta para la eliminación del turno
            try
            {
                Mi_SQL = "DELETE FROM " + Cat_Turnos.Tabla_Cat_Turnos + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Turnos.Campo_Turno_ID + " = '" + Datos.P_Turno_ID + "'";
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
            finally
            {
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Datos_Turnos
        /// DESCRIPCION : Consulta todos los datos de los turnos que estan dadas de alta en la BD
        ///               con todos sus datos
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 25-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Datos_Turnos(Cls_Cat_Turnos_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta del turno
            try
            {
                Mi_SQL = "SELECT * FROM " + Cat_Turnos.Tabla_Cat_Turnos;
                if (Datos.P_Turno_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Turnos.Campo_Turno_ID + " = '" + Datos.P_Turno_ID + "'";
                }
                if (Datos.P_Descripcion != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_Turnos.Campo_Descripcion + ") LIKE UPPER('%" + Datos.P_Descripcion + "%')";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Turnos.Campo_Descripcion;
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
        /// NOMBRE DE LA FUNCION: Consulta_Turnos
        /// DESCRIPCION : Consulta todos los turnos que estan dadas de alta en la BD
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 25-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Turnos(Cls_Cat_Turnos_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de las turno

            try
            {
                Mi_SQL = "SELECT " + Cat_Turnos.Campo_Turno_ID + ", " + Cat_Turnos.Campo_Descripcion + ", " +
                    "(" + Cat_Turnos.Campo_Descripcion + " || ' DE ' || " +
                    "TO_CHAR(" + Cat_Turnos.Campo_Hora_Entrada + ", 'HH24:MI') || ' A ' ||" +
                    "TO_CHAR(" + Cat_Turnos.Campo_Hora_Salida + ", 'HH24:MI')) AS INF_TURNO" + " FROM ";
                Mi_SQL = Mi_SQL + Cat_Turnos.Tabla_Cat_Turnos;
                if (Datos.P_Turno_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Turnos.Campo_Turno_ID + " = '" + Datos.P_Turno_ID + "'";
                }
                if (Datos.P_Descripcion != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_Turnos.Campo_Descripcion + ") LIKE UPPER('%" + Datos.P_Descripcion + "%')";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Turnos.Campo_Descripcion;
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
    }
}