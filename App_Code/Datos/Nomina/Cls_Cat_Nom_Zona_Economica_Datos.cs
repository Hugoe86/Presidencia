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
using Presidencia.Zona_Economica.Negocios;

/// <summary>
/// Summary description for Cls_Cat_Zona_Economica_Datos
/// </summary>
namespace Presidencia.Zona_Economica.Datos
{
    public class Cls_Cat_Nom_Zona_Economica_Datos
    {
        public Cls_Cat_Nom_Zona_Economica_Datos()
        {
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Zona_Economica
        /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta la Zona Económica en la BD con los datos proporcionados
        ///                  por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 27-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Zona_Economica(Cls_Cat_Nom_Zona_Economica_Negocio Datos)
        {
            String Mi_SQL; //Obtiene la cadena de inserción hacía la base de datos
            Object Zona_ID;   //Obtiene el ID con la cual se guardo los datos en la base de datos

            try
            {
                Mi_SQL = "SELECT NVL(MAX(" + Cat_Nom_Zona_Economica.Campo_Zona_ID + "),'00000') ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Nom_Zona_Economica.Tabla_Cat_Nom_Zona_Economica;
                Zona_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Zona_ID))
                {
                    Datos.P_Zona_ID = "00001";
                }
                else
                {
                    Datos.P_Zona_ID = String.Format("{0:00000}", Convert.ToInt32(Zona_ID) + 1);
                }
                //Consulta para la inserción de la escolaridad con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Cat_Nom_Zona_Economica.Tabla_Cat_Nom_Zona_Economica + " (";
                Mi_SQL = Mi_SQL + Cat_Nom_Zona_Economica.Campo_Zona_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Zona_Economica.Campo_Zona_Economica + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Zona_Economica.Campo_Comentarios + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Zona_Economica.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Zona_Economica.Campo_Fecha_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Zona_Economica.Campo_Salario_Diario + ") VALUES ('";
                Mi_SQL = Mi_SQL + Datos.P_Zona_ID + "', '" + Datos.P_Zona_Economica + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Comentarios + "', '" + Datos.P_Nombre_Usuario + "', SYSDATE, "+ Datos.P_Salario_Diario +")";
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
        /// NOMBRE DE LA FUNCION: Modificar_Zona_Economica
        /// DESCRIPCION : Modifica los datos de la Zona Económica con lo que fueron introducidos por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán modificados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 27-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Modificar_Zona_Economica(Cls_Cat_Nom_Zona_Economica_Negocio Datos)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos

            try
            {
                //Consulta para la modificación de la Escolaridad con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Cat_Nom_Zona_Economica.Tabla_Cat_Nom_Zona_Economica + " SET ";
                Mi_SQL = Mi_SQL + Cat_Nom_Zona_Economica.Campo_Zona_Economica + " = '" + Datos.P_Zona_Economica + "', ";
                Mi_SQL = Mi_SQL + Cat_Nom_Zona_Economica.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + Cat_Nom_Zona_Economica.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL = Mi_SQL + Cat_Nom_Zona_Economica.Campo_Salario_Diario + "=" + Datos.P_Salario_Diario + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Zona_Economica.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                Mi_SQL = Mi_SQL + Cat_Nom_Zona_Economica.Campo_Zona_ID + " = '" + Datos.P_Zona_ID + "'";
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
        /// NOMBRE DE LA FUNCION: Eliminar_Zona_Economica
        /// DESCRIPCION : Elimina la Zona Económica que fue seleccionada por el usuario de la BD
        /// PARAMETROS  : Datos: Obtiene que Zona Económica desea eliminar de la BD
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 27-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Eliminar_Zona_Economica(Cls_Cat_Nom_Zona_Economica_Negocio Datos)
        {
            String Mi_SQL; //Variable de Consulta para la eliminación de la Zona Económica
            try
            {
                Mi_SQL = "DELETE FROM " + Cat_Nom_Zona_Economica.Tabla_Cat_Nom_Zona_Economica + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Nom_Zona_Economica.Campo_Zona_ID + " = '" + Datos.P_Zona_ID + "'";
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
        /// NOMBRE DE LA FUNCION: Consulta_Datos_Zona_Economica
        /// DESCRIPCION : Consulta todos los datos de la Zona Económica que estan dadas de alta en
        ///               la BD con todos sus datos
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 27-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Datos_Zona_Economica(Cls_Cat_Nom_Zona_Economica_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de las Zonas Económicas

            try
            {
                //Consulta todos los datos de la Zona Económica que se fue seleccionado por el usuario
                Mi_SQL = "SELECT * FROM " + Cat_Nom_Zona_Economica.Tabla_Cat_Nom_Zona_Economica;
                if (Datos.P_Zona_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Zona_Economica.Campo_Zona_ID + " = '" + Datos.P_Zona_ID + "'";
                }
                if (Datos.P_Zona_Economica != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_Nom_Zona_Economica.Campo_Zona_Economica + ") LIKE UPPER('%" + Datos.P_Zona_Economica + "%')";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Nom_Zona_Economica.Campo_Zona_Economica;
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
        /// NOMBRE DE LA FUNCION: Consulta_Zona_Economica
        /// DESCRIPCION : Consulta las Zonas Económicas que estan dados de alta en la BD 
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 27-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Zona_Economica(Cls_Cat_Nom_Zona_Economica_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de las Escolaridades

            try
            {
                //Consulta las Zona Económica que estan dados de alta en la base de datos
                Mi_SQL = "SELECT " + Cat_Nom_Zona_Economica.Campo_Zona_ID + ", " + Cat_Nom_Zona_Economica.Campo_Zona_Economica;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Nom_Zona_Economica.Tabla_Cat_Nom_Zona_Economica;
                if (Datos.P_Zona_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Zona_Economica.Campo_Zona_ID + " = '" + Datos.P_Zona_ID + "'";
                }
                if (Datos.P_Zona_Economica != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_Nom_Zona_Economica.Campo_Zona_Economica + ") LIKE UPPER('%" + Datos.P_Zona_Economica + "%')";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Nom_Zona_Economica.Campo_Zona_Economica;
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