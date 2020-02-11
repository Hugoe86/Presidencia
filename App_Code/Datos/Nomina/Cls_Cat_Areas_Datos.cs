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
using Presidencia.Areas.Negocios;

/// <summary>
/// Summary description for Cls_Cat_Areas_Datos
/// </summary>
namespace Presidencia.Areas.Datos
{
    public class Cls_Cat_Areas_Datos
    {
        public Cls_Cat_Areas_Datos()
        {
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Dependencia
        /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta el Área en la BD con los datos proporcionados por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 24-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Area(Cls_Cat_Areas_Negocio Datos)
        {
            String Mi_SQL;  //Obtiene la cadena de inserción hacía la base de datos
            Object Area_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos

            try
            {
                Mi_SQL = "SELECT NVL(MAX(" + Cat_Areas.Campo_Area_ID + "),'00000') ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Areas.Tabla_Cat_Areas;
                Area_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Area_ID))
                {
                    Datos.P_Area_ID = "00001";
                }
                else
                {
                    Datos.P_Area_ID = String.Format("{0:00000}", Convert.ToInt32(Area_ID) + 1);
                }
                //Consulta para la inserción del área con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Cat_Areas.Tabla_Cat_Areas + " (";
                Mi_SQL = Mi_SQL + Cat_Areas.Campo_Area_ID + ", " + Cat_Areas.Campo_Dependencia_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Areas.Campo_Nombre + ", " + Cat_Areas.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Cat_Areas.Campo_Comentarios + ", ";
                Mi_SQL = Mi_SQL + Cat_Areas.Campo_Usuario_Creo + ", " + Cat_Areas.Campo_Fecha_Creo + ") VALUES ('";
                Mi_SQL = Mi_SQL + Datos.P_Area_ID + "', '" + Datos.P_Dependencia_ID + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Nombre + "', '" + Datos.P_Estatus + "', '" + Datos.P_Comentarios + "', '";
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
        /// NOMBRE DE LA FUNCION: Modificar_Area
        /// DESCRIPCION : Modifica los datos del área con lo que fueron introducidos por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán modificados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 24-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Modificar_Area(Cls_Cat_Areas_Negocio Datos)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos

            try
            {
                //Consulta para la modificación del área con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Cat_Areas.Tabla_Cat_Areas + " SET ";
                Mi_SQL = Mi_SQL + Cat_Areas.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "', ";
                Mi_SQL = Mi_SQL + Cat_Areas.Campo_Nombre + " = '" + Datos.P_Nombre + "', ";
                Mi_SQL = Mi_SQL + Cat_Areas.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                Mi_SQL = Mi_SQL + Cat_Areas.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + Cat_Areas.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL = Mi_SQL + Cat_Areas.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                Mi_SQL = Mi_SQL + Cat_Areas.Campo_Area_ID + " = '" + Datos.P_Area_ID + "'";
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
        /// NOMBRE DE LA FUNCION: Elimina_Area
        /// DESCRIPCION : Elimina el área que fue seleccionada por el usuario de la BD
        /// PARAMETROS  : Datos: Obtiene que área desea eliminar de la BD
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 24-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Elimina_Area(Cls_Cat_Areas_Negocio Datos)
        {
            String Mi_SQL; //Variable de Consulta para la eliminación del área
            try
            {
                Mi_SQL = "DELETE FROM " + Cat_Areas.Tabla_Cat_Areas + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Areas.Campo_Area_ID + " = '" + Datos.P_Area_ID + "'";
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
        /// NOMBRE DE LA FUNCION: Consulta_Datos_Areas
        /// DESCRIPCION : Consulta todos los datos de las áreas que estan dadas de alta en la BD
        ///               con todos sus datos
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 24-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Datos_Areas(Cls_Cat_Areas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de las areas

            try
            {
                //Consulta todos los datos del área que se fue seleccionada por el usuario
                Mi_SQL = "SELECT " + Cat_Areas.Tabla_Cat_Areas + ".*, ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS Dependencia FROM ";
                Mi_SQL = Mi_SQL + Cat_Areas.Tabla_Cat_Areas + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " = " + Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Dependencia_ID;
                if (Datos.P_Area_ID != null)
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Area_ID + " = '" + Datos.P_Area_ID + "'";
                }
                if (Datos.P_Nombre != null)
                {
                    Mi_SQL = Mi_SQL + " AND UPPER(" + Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Nombre + "%')";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Nombre;
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
        /// NOMBRE DE LA FUNCION: Consulta_Areas
        /// DESCRIPCION : Consulta las áreas que estan dadas de alta en la BD
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 24-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    : Silvia Morales
        /// CAUSA_MODIFICACION: Se agrego la condición de buscar el área por dependencia
        ///*******************************************************************************
        public static DataTable Consulta_Areas(Cls_Cat_Areas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de las areas

            try
            {
                Mi_SQL = "SELECT " + Cat_Areas.Campo_Area_ID + ", " + Cat_Areas.Campo_Nombre;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Areas.Tabla_Cat_Areas;
                if (Datos.P_Area_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Areas.Campo_Area_ID + " = '" + Datos.P_Area_ID + "'";
                }
                if (Datos.P_Nombre != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_Areas.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Nombre + "%')";
                }
                if (Datos.P_Dependencia_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Areas.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "'";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Areas.Campo_Nombre;
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
