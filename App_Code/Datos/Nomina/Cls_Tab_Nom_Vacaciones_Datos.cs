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
using Presidencia.Vacaciones.Negocios;

/// <summary>
/// Summary description for Cls_Tab_Nom_Vacaciones_Datos
/// </summary>
namespace Presidencia.Vacaciones.Datos
{
    public class Cls_Tab_Nom_Vacaciones_Datos
    {
        public Cls_Tab_Nom_Vacaciones_Datos()
        {
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Vacacion
        /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta el Vacacion en la BD con los datos proporcionados por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 02-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Vacacion(Cls_Tab_Nom_Vacaciones_Negocio Datos)
        {
            String Mi_SQL;      //Obtiene la cadena de inserción hacía la base de datos
            Object Vacacion_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos

            try
            {
                Mi_SQL = "SELECT NVL(MAX(" + Tab_Nom_Vacaciones.Campo_Vacacion_ID + "),'00000') ";
                Mi_SQL = Mi_SQL + "FROM " + Tab_Nom_Vacaciones.Tabla_Tab_Nom_Vacaciones;
                Vacacion_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Vacacion_ID))
                {
                    Datos.P_Vacacion_ID = "00001";
                }
                else
                {
                    Datos.P_Vacacion_ID = String.Format("{0:00000}", Convert.ToInt32(Vacacion_ID) + 1);
                }
                //Consulta para la inserción de la Vacacion con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Tab_Nom_Vacaciones.Tabla_Tab_Nom_Vacaciones + " (";
                Mi_SQL = Mi_SQL + Tab_Nom_Vacaciones.Campo_Vacacion_ID + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_Vacaciones.Campo_Antiguedad + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_Vacaciones.Campo_Dias + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_Vacaciones.Campo_Comentarios + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_Vacaciones.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_Vacaciones.Campo_Fecha_Creo + ") VALUES ('";
                Mi_SQL = Mi_SQL + Datos.P_Vacacion_ID + "', " + Datos.P_Antiguedad + ", ";
                Mi_SQL = Mi_SQL + Datos.P_Dias + ", '" + Datos.P_Comentarios + "', '";
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
        /// NOMBRE DE LA FUNCION: Modificar_Vacacion
        /// DESCRIPCION : Modifica los datos del Vacacion con lo que fueron introducidos por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán modificados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 02-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Modificar_Vacacion(Cls_Tab_Nom_Vacaciones_Negocio Datos)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos

            try
            {
                //Consulta para la modificación de la Vacacion con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Tab_Nom_Vacaciones.Tabla_Tab_Nom_Vacaciones + " SET ";
                Mi_SQL = Mi_SQL + Tab_Nom_Vacaciones.Campo_Antiguedad + " = " + Datos.P_Antiguedad + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_Vacaciones.Campo_Dias + " = " + Datos.P_Dias + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_Vacaciones.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + Tab_Nom_Vacaciones.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL = Mi_SQL + Tab_Nom_Vacaciones.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                Mi_SQL = Mi_SQL + Tab_Nom_Vacaciones.Campo_Vacacion_ID + " = '" + Datos.P_Vacacion_ID + "'";
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
        /// NOMBRE DE LA FUNCION: Eliminar_Vacacion
        /// DESCRIPCION : Elimina la Vacacion que fue seleccionado por el usuario de la BD
        /// PARAMETROS  : Datos: Obtiene que Vacación desea eliminar de la BD
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 02-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Eliminar_Vacacion(Cls_Tab_Nom_Vacaciones_Negocio Datos)
        {
            String Mi_SQL; //Variable de Consulta para la eliminación del Vacaciones 
            try
            {
                Mi_SQL = "DELETE FROM " + Tab_Nom_Vacaciones.Tabla_Tab_Nom_Vacaciones + " WHERE ";
                Mi_SQL = Mi_SQL + Tab_Nom_Vacaciones.Campo_Vacacion_ID + " = '" + Datos.P_Vacacion_ID + "'";
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
        /// NOMBRE DE LA FUNCION: Consulta_Datos_Vacaciones
        /// DESCRIPCION : Consulta todos los datos de las Vacaciones que estan dados de alta en
        ///               la BD con todos sus datos
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 02-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Datos_Vacaciones(Cls_Tab_Nom_Vacaciones_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta para el Vacaciones

            try
            {
                //Consulta todos los datos del Vacación que se fue seleccionado por el usuario
                Mi_SQL = "SELECT * FROM " + Tab_Nom_Vacaciones.Tabla_Tab_Nom_Vacaciones;
                if (Datos.P_Vacacion_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Tab_Nom_Vacaciones.Campo_Vacacion_ID + " = '" + String.Format("{0:00000}", Convert.ToInt32(Datos.P_Vacacion_ID)) + "'";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Tab_Nom_Vacaciones.Campo_Vacacion_ID;
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