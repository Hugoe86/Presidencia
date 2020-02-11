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
using Presidencia.IMSS.Negocios;

/// <summary>
/// Summary description for Cls_Tab_Nom_IMSS_Datos
/// </summary>
namespace Presidencia.IMSS.Datos
{
    public class Cls_Tab_Nom_IMSS_Datos
    {
        public Cls_Tab_Nom_IMSS_Datos()
        {
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_IMSS
        /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta el IMSS en la BD con los datos proporcionados por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 02-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_IMSS(Cls_Tab_Nom_IMSS_Negocio Datos)
        {
            String Mi_SQL; //Obtiene la cadena de inserción hacía la base de datos
            Object IMSS_ID;   //Obtiene el ID con la cual se guardo los datos en la base de datos

            try
            {
                Mi_SQL = "SELECT NVL(MAX(" + Tab_Nom_IMSS.Campo_IMSS_ID + "),'00000') ";
                Mi_SQL = Mi_SQL + "FROM " + Tab_Nom_IMSS.Tabla_Tab_Nom_IMSS;
                IMSS_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(IMSS_ID))
                {
                    Datos.P_IMSS_ID = "00001";
                }
                else
                {
                    Datos.P_IMSS_ID = String.Format("{0:00000}", Convert.ToInt32(IMSS_ID) + 1);
                }
                //Consulta para la inserción del IMSS con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Tab_Nom_IMSS.Tabla_Tab_Nom_IMSS + " (";
                Mi_SQL = Mi_SQL + Tab_Nom_IMSS.Campo_IMSS_ID + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_IMSS.Campo_Porcentaje_Enf_Mat_Esp + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_IMSS.Campo_Porcentaje_Enf_Mat_Pes + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_IMSS.Campo_Porcentaje_Invalidez_Vida + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_IMSS.Campo_Porcentaje_Cesantia_Vejez + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_IMSS.Campo_Comentarios + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_IMSS.Campo_Excendete_3_SMG_DF + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_IMSS.Campo_Prestaciones_Dinero + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_IMSS.Campo_Gastos_Medicos + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_IMSS.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_IMSS.Campo_Fecha_Creo + ") VALUES ('";
                Mi_SQL = Mi_SQL + Datos.P_IMSS_ID + "', " + Datos.P_Porcentaje_Enfermedad_Maternidad_Especie + ", ";
                Mi_SQL = Mi_SQL + Datos.P_Porcentaje_Enfermedad_Maternidad_Pesos + ", ";
                Mi_SQL = Mi_SQL + Datos.P_Porcentaje_Invalidez_Vida + ", " + Datos.P_Porcentaje_Cesantia_Vejez + ", '";
                Mi_SQL = Mi_SQL + Datos.P_Comentarios + "', '" + Datos.P_Excendente_SMG_DF + "', '" + Datos.P_Prestaciones_Dinero + "', '" + Datos.P_Gastos_Medicos + "', '";
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
        /// NOMBRE DE LA FUNCION: Modificar_IMSS
        /// DESCRIPCION : Modifica los datos del IMSS con lo que fueron introducidos por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán modificados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 02-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Modificar_IMSS(Cls_Tab_Nom_IMSS_Negocio Datos)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos

            try
            {
                //Consulta para la modificación del IMSS con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Tab_Nom_IMSS.Tabla_Tab_Nom_IMSS + " SET ";
                Mi_SQL = Mi_SQL + Tab_Nom_IMSS.Campo_Porcentaje_Enf_Mat_Esp + " = " + Datos.P_Porcentaje_Enfermedad_Maternidad_Especie + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_IMSS.Campo_Porcentaje_Enf_Mat_Pes + " = " + Datos.P_Porcentaje_Enfermedad_Maternidad_Pesos + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_IMSS.Campo_Porcentaje_Invalidez_Vida + " = " + Datos.P_Porcentaje_Invalidez_Vida + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_IMSS.Campo_Porcentaje_Cesantia_Vejez + " = " + Datos.P_Porcentaje_Cesantia_Vejez + ", ";
                Mi_SQL = Mi_SQL + Tab_Nom_IMSS.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + Tab_Nom_IMSS.Campo_Excendete_3_SMG_DF + " = '" + Datos.P_Excendente_SMG_DF + "', ";
                Mi_SQL = Mi_SQL + Tab_Nom_IMSS.Campo_Prestaciones_Dinero + " = '" + Datos.P_Prestaciones_Dinero + "', ";
                Mi_SQL = Mi_SQL + Tab_Nom_IMSS.Campo_Gastos_Medicos + " = '" + Datos.P_Gastos_Medicos + "', ";
                Mi_SQL = Mi_SQL + Tab_Nom_IMSS.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL = Mi_SQL + Tab_Nom_IMSS.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                Mi_SQL = Mi_SQL + Tab_Nom_IMSS.Campo_IMSS_ID + " = '" + Datos.P_IMSS_ID + "'";
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
        /// NOMBRE DE LA FUNCION: Eliminar_IMSS
        /// DESCRIPCION : Elimina el IMSS que fue seleccionado por el usuario de la BD
        /// PARAMETROS  : Datos: Obtiene que IMSS desea eliminar de la BD
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 02-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Eliminar_IMSS(Cls_Tab_Nom_IMSS_Negocio Datos)
        {
            String Mi_SQL; //Variable de Consulta para la eliminación del IMSS
            try
            {
                Mi_SQL = "DELETE FROM " + Tab_Nom_IMSS.Tabla_Tab_Nom_IMSS + " WHERE ";
                Mi_SQL = Mi_SQL + Tab_Nom_IMSS.Campo_IMSS_ID + " = '" + Datos.P_IMSS_ID + "'";
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
        /// NOMBRE DE LA FUNCION: Consulta_Datos_IMSS
        /// DESCRIPCION : Consulta todos los datos del IMSS que estan dados de alta en
        ///               la BD con todos sus datos
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 02-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Datos_IMSS(Cls_Tab_Nom_IMSS_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta para el IMSS

            try
            {
                //Consulta todos los datos del IMSS que se fue seleccionado por el usuario
                Mi_SQL = "SELECT * FROM " + Tab_Nom_IMSS.Tabla_Tab_Nom_IMSS;
                if (Datos.P_IMSS_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Tab_Nom_IMSS.Campo_IMSS_ID + " = '" + String.Format("{0:00000}", Convert.ToInt32(Datos.P_IMSS_ID)) + "'";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Tab_Nom_IMSS.Campo_IMSS_ID;
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