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
using Presidencia.Subsidio.Negocios;

/// <summary>
/// Summary description for Cls_Tab_Nom_Subsidio_Datos
/// </summary>
namespace Presidencia.Subsidio.Datos
{
    public class Cls_Tab_Nom_Subsidio_Datos
    {
        public Cls_Tab_Nom_Subsidio_Datos()
        {
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Subsidio
        /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta el Subsidio en la BD con los datos proporcionados
        ///                  por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 30-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Subsidio(Cls_Tab_Nom_Subsidio_Negocio Datos)
        {
            String Mi_SQL;      //Obtiene la cadena de inserción hacía la base de datos
            Object Subsidio_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos

            try
            {
                Mi_SQL = "SELECT NVL(MAX(" + Tab_Nom_Subsidio.Campo_Subsidio_ID + "),'00000') ";
                Mi_SQL = Mi_SQL + "FROM " + Tab_Nom_Subsidio.Tabla_Tab_Nom_Subsidio;
                Subsidio_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Subsidio_ID))
                {
                    Datos.P_Subsidio_ID = "00001";
                }
                else
                {
                    Datos.P_Subsidio_ID = String.Format("{0:00000}", Convert.ToInt32(Subsidio_ID) + 1);
                }
                //Consulta para la inserción del Subsidio con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Tab_Nom_Subsidio.Tabla_Tab_Nom_Subsidio + " (";
                Mi_SQL = Mi_SQL + Tab_Nom_Subsidio.Campo_Subsidio_ID + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_Subsidio.Campo_Limite_Inferior + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_Subsidio.Campo_Subsidio + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_Subsidio.Campo_Tipo_Nomina + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_Subsidio.Campo_Comentarios + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_Subsidio.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_Subsidio.Campo_Fecha_Creo + ") VALUES ('";
                Mi_SQL = Mi_SQL + Datos.P_Subsidio_ID + "', " + Datos.P_Limite_Inferior + ", ";
                Mi_SQL = Mi_SQL + Datos.P_Subsidio + ", '" + Datos.P_Tipo_Nomina + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Comentarios + "', '" + Datos.P_Nombre_Usuario + "', SYSDATE)";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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
        /// NOMBRE DE LA FUNCION: Modificar_Subsidio
        /// DESCRIPCION : Modifica los datos del Subsidio con lo que fueron introducidos por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán modificados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 30-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Modificar_Subsidio(Cls_Tab_Nom_Subsidio_Negocio Datos)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos

            try
            {
                //Consulta para la modificación del Subsidio con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Tab_Nom_Subsidio.Tabla_Tab_Nom_Subsidio + " SET ";
                Mi_SQL = Mi_SQL + Tab_Nom_Subsidio.Campo_Limite_Inferior + " = " + Datos.P_Limite_Inferior + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_Subsidio.Campo_Subsidio + " = " + Datos.P_Subsidio + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_Subsidio.Campo_Tipo_Nomina + " = '" + Datos.P_Tipo_Nomina + "', ";
                Mi_SQL = Mi_SQL + Tab_Nom_Subsidio.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + Tab_Nom_Subsidio.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL = Mi_SQL + Tab_Nom_Subsidio.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                Mi_SQL = Mi_SQL + Tab_Nom_Subsidio.Campo_Subsidio_ID + " = '" + Datos.P_Subsidio_ID + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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
        /// NOMBRE DE LA FUNCION: Eliminar_Subsidio
        /// DESCRIPCION : Elimina el Subsidio que fue seleccionado por el usuario de la BD
        /// PARAMETROS  : Datos: Obtiene que Subsidio desea eliminar de la BD
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 30-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Eliminar_Subsidio(Cls_Tab_Nom_Subsidio_Negocio Datos)
        {
            String Mi_SQL; //Variable de Consulta para la eliminación del Subsidio
            try
            {
                Mi_SQL = "DELETE FROM " + Tab_Nom_Subsidio.Tabla_Tab_Nom_Subsidio + " WHERE ";
                Mi_SQL = Mi_SQL + Tab_Nom_Subsidio.Campo_Subsidio_ID + " = '" + Datos.P_Subsidio_ID + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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
        /// NOMBRE DE LA FUNCION: Consulta_Datos_Subsidio
        /// DESCRIPCION : Consulta todos los datos del Subsidio que estan dados de alta en
        ///                   la BD con todos sus datos
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 30-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Datos_Subsidio(Cls_Tab_Nom_Subsidio_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta para los Subsidio

            try
            {
                //Consulta todos los datos del Subsidio que se fue seleccionado por el usuario
                Mi_SQL = "SELECT * FROM " + Tab_Nom_Subsidio.Tabla_Tab_Nom_Subsidio;
                if (Datos.P_Subsidio_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Tab_Nom_Subsidio.Campo_Subsidio_ID + " = '" + String.Format("{0:00000}", Convert.ToInt32(Datos.P_Subsidio_ID)) + "'";
                }
                if (Datos.P_Tipo_Nomina != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Tab_Nom_Subsidio.Campo_Tipo_Nomina + " LIKE '%" + Datos.P_Tipo_Nomina + "%'";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Tab_Nom_Subsidio.Campo_Tipo_Nomina + ", " + Tab_Nom_Subsidio.Campo_Limite_Inferior;
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
    }
}