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
using Presidencia.ISR.Negocios;

/// <summary>
/// Summary description for Cls_Tab_Nom_ISR_Datos
/// </summary>
namespace Presidencia.ISR.Datos
{
    public class Cls_Tab_Nom_ISR_Datos
    {
        public Cls_Tab_Nom_ISR_Datos()
        {
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_ISR
        /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta el ISR en la BD con los datos proporcionados
        ///                  por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 01-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_ISR(Cls_Tab_Nom_ISR_Negocio Datos)
        {
            String Mi_SQL; //Obtiene la cadena de inserción hacía la base de datos
            Object ISR_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos

            try
            {
                Mi_SQL = "SELECT NVL(MAX(" + Tab_Nom_ISR.Campo_ISR_ID + "),'00000') ";
                Mi_SQL = Mi_SQL + "FROM " + Tab_Nom_ISR.Tabla_Tab_Nom_ISR;
                ISR_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(ISR_ID))
                {
                    Datos.P_ISR_ID = "00001";
                }
                else
                {
                    Datos.P_ISR_ID = String.Format("{0:00000}", Convert.ToInt32(ISR_ID) + 1);
                }
                //Consulta para la inserción del ISR con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Tab_Nom_ISR.Tabla_Tab_Nom_ISR + " (";
                Mi_SQL = Mi_SQL + Tab_Nom_ISR.Campo_ISR_ID + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_ISR.Campo_Limite_Inferior + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_ISR.Campo_Cuota_Fija + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_ISR.Campo_Porcentaje + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_ISR.Campo_Tipo_Nomina + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_ISR.Campo_Comentarios + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_ISR.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_ISR.Campo_Fecha_Creo + ") VALUES ('";
                Mi_SQL = Mi_SQL + Datos.P_ISR_ID + "', " + Datos.P_Limite_Inferior + ", ";
                Mi_SQL = Mi_SQL + Datos.P_Couta_Fija + ", " + Datos.P_Porcentaje + ", '" + Datos.P_Tipo_Nomina + "', '";
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
        /// NOMBRE DE LA FUNCION: Modificar_ISR
        /// DESCRIPCION : Modifica los datos del ISR con lo que fueron introducidos por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán modificados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 01-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Modificar_ISR(Cls_Tab_Nom_ISR_Negocio Datos)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos

            try
            {
                //Consulta para la modificación del ISR con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Tab_Nom_ISR.Tabla_Tab_Nom_ISR + " SET ";
                Mi_SQL = Mi_SQL + Tab_Nom_ISR.Campo_Limite_Inferior + " = " + Datos.P_Limite_Inferior + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_ISR.Campo_Cuota_Fija + " = " + Datos.P_Couta_Fija + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_ISR.Campo_Porcentaje + " = " + Datos.P_Porcentaje + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_ISR.Campo_Tipo_Nomina + " = '" + Datos.P_Tipo_Nomina + "', ";
                Mi_SQL = Mi_SQL + Tab_Nom_ISR.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + Tab_Nom_ISR.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL = Mi_SQL + Tab_Nom_ISR.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                Mi_SQL = Mi_SQL + Tab_Nom_ISR.Campo_ISR_ID + " = '" + Datos.P_ISR_ID + "'";
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
        /// NOMBRE DE LA FUNCION: Eliminar_ISR
        /// DESCRIPCION : Elimina el ISR que fue seleccionado por el usuario de la BD
        /// PARAMETROS  : Datos: Obtiene que ISR desea eliminar de la BD
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 01-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Eliminar_ISR(Cls_Tab_Nom_ISR_Negocio Datos)
        {
            String Mi_SQL; //Variable de Consulta para la eliminación del ISR
            try
            {
                Mi_SQL = "DELETE FROM " + Tab_Nom_ISR.Tabla_Tab_Nom_ISR + " WHERE ";
                Mi_SQL = Mi_SQL + Tab_Nom_ISR.Campo_ISR_ID + " = '" + Datos.P_ISR_ID + "'";
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
        /// NOMBRE DE LA FUNCION: Consulta_Datos_ISR
        /// DESCRIPCION : Consulta todos los datos del ISR que estan dados de alta en
        ///               la BD con todos sus datos
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 01-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Datos_ISR(Cls_Tab_Nom_ISR_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de los Sindicatos

            try
            {
                //Consulta todos los datos del ISR que se fue seleccionado por el usuario
                Mi_SQL = "SELECT * FROM " + Tab_Nom_ISR.Tabla_Tab_Nom_ISR;
                if (Datos.P_ISR_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Tab_Nom_ISR.Campo_ISR_ID + " = '" + String.Format("{0:00000}", Convert.ToInt32(Datos.P_ISR_ID)) + "'";
                }
                if (Datos.P_Tipo_Nomina != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Tab_Nom_ISR.Campo_Tipo_Nomina + " LIKE '%" + Datos.P_Tipo_Nomina + "%'";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Tab_Nom_ISR.Campo_Tipo_Nomina + ", " + Tab_Nom_ISR.Campo_Limite_Inferior;
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