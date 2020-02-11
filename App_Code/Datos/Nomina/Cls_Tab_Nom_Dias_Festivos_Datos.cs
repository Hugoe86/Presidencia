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
using Presidencia.Dias_Festivos.Negocios;

/// <summary>
/// Summary description for Cls_Tab_Nom_Dias_Festivos_Datos
/// </summary>
namespace Presidencia.Dias_Festivos.Datos
{
    public class Cls_Tab_Nom_Dias_Festivos_Datos
    {
        public Cls_Tab_Nom_Dias_Festivos_Datos()
        {
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Dia_Festivo
        /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta el Dia Festivo en la BD con los datos proporcionados por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 03-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Dia_Festivo(Cls_Tab_Nom_Dias_Festivos_Negocios Datos)
        {
            String Mi_SQL; //Obtiene la cadena de inserción hacía la base de datos
            Object Dia_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos

            try
            {
                Mi_SQL = "SELECT NVL(MAX(" + Tab_Nom_Dias_Festivos.Campo_Dia_ID + "),'00000') ";
                Mi_SQL = Mi_SQL + "FROM " + Tab_Nom_Dias_Festivos.Tabla_Tab_Nom_Dias_Festivos;
                Dia_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Dia_ID))
                {
                    Datos.P_Dia_ID = "00001";
                }
                else
                {
                    Datos.P_Dia_ID = String.Format("{0:00000}", Convert.ToInt32(Dia_ID) + 1);
                }
                //Consulta para la inserción del Día Festivo con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Tab_Nom_Dias_Festivos.Tabla_Tab_Nom_Dias_Festivos + " (";
                Mi_SQL = Mi_SQL + Tab_Nom_Dias_Festivos.Campo_Dia_ID + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_Dias_Festivos.Campo_Fecha + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_Dias_Festivos.Campo_Descripcion + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_Dias_Festivos.Campo_Comentarios + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_Dias_Festivos.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_Dias_Festivos.Campo_Fecha_Creo + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_Dias_Festivos.Campo_Nomina_ID + ") VALUES ('";
                Mi_SQL = Mi_SQL + Datos.P_Dia_ID + "', TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha) + "','DD/MM/YY'), '";
                Mi_SQL = Mi_SQL + Datos.P_Descripcion + "', '" + Datos.P_Comentarios + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Nombre_Usuario + "', SYSDATE, '" + Datos.P_Nomina_ID + "')";
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
        /// NOMBRE DE LA FUNCION: Modificar_Dia_Festivo
        /// DESCRIPCION : Modifica los datos del Día Festivo con lo que fueron introducidos por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán modificados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 03-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Modificar_Dia_Festivo(Cls_Tab_Nom_Dias_Festivos_Negocios Datos)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos

            try
            {
                //Consulta para la modificación del Día Festivo con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Tab_Nom_Dias_Festivos.Tabla_Tab_Nom_Dias_Festivos + " SET ";
                Mi_SQL = Mi_SQL + Tab_Nom_Dias_Festivos.Campo_Fecha + " = TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha) + "','DD/MM/YY'), ";
                Mi_SQL = Mi_SQL + Tab_Nom_Dias_Festivos.Campo_Descripcion + " = '" + Datos.P_Descripcion + "', ";
                Mi_SQL = Mi_SQL + Tab_Nom_Dias_Festivos.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + Tab_Nom_Dias_Festivos.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL = Mi_SQL + Tab_Nom_Dias_Festivos.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "', ";
                Mi_SQL = Mi_SQL + Tab_Nom_Dias_Festivos.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                Mi_SQL = Mi_SQL + Tab_Nom_Dias_Festivos.Campo_Dia_ID + " = '" + Datos.P_Dia_ID + "'";
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
        /// NOMBRE DE LA FUNCION: Eliminar_Dia_Festivo
        /// DESCRIPCION : Elimina el Dia Festivo que fue seleccionado por el usuario de la BD
        /// PARAMETROS  : Datos: Obtiene que Día Festivo desea eliminar de la BD
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 03-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Eliminar_Dia_Festivo(Cls_Tab_Nom_Dias_Festivos_Negocios Datos)
        {
            String Mi_SQL; //Variable de Consulta para la eliminación de la Día Festivo
            try
            {
                Mi_SQL = "DELETE FROM " + Tab_Nom_Dias_Festivos.Tabla_Tab_Nom_Dias_Festivos + " WHERE ";
                Mi_SQL = Mi_SQL + Tab_Nom_Dias_Festivos.Campo_Dia_ID + " = '" + Datos.P_Dia_ID + "'";
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
        /// NOMBRE DE LA FUNCION: Consulta_Datos_Dia_Festivo
        /// DESCRIPCION : Consulta todos los datos del Dia Festivo que estan dados de alta en
        ///               la BD con todos sus datos
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 03-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Datos_Dia_Festivo(Cls_Tab_Nom_Dias_Festivos_Negocios Datos)
        {
            String Mi_SQL; //Variable para la consulta para el Dia Festivo

            try
            {
                //Consulta todos los datos del Día Festivo que se fue seleccionado por el usuario
                Mi_SQL = "SELECT * FROM " + Tab_Nom_Dias_Festivos.Tabla_Tab_Nom_Dias_Festivos;


                if (!string.IsNullOrEmpty(Datos.P_Dia_ID))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL = Mi_SQL + " AND " + Tab_Nom_Dias_Festivos.Campo_Dia_ID + " = '" + String.Format("{0:00000}", Convert.ToInt32(Datos.P_Dia_ID)) + "'";
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + " WHERE " + Tab_Nom_Dias_Festivos.Campo_Dia_ID + " = '" + String.Format("{0:00000}", Convert.ToInt32(Datos.P_Dia_ID)) + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Nomina_ID))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL = Mi_SQL + " AND " + Tab_Nom_Dias_Festivos.Campo_Nomina_ID + " = '" + Datos.P_Nomina_ID + "'";
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + " WHERE " + Tab_Nom_Dias_Festivos.Campo_Nomina_ID + " = '" + Datos.P_Nomina_ID + "'";
                    }
                }

                Mi_SQL = Mi_SQL + " ORDER BY " + Tab_Nom_Dias_Festivos.Campo_Dia_ID;
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