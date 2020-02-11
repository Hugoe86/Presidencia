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
using Presidencia.Programas.Negocios;

/// <summary>
/// Summary description for Cls_Cat_Nom_Programas_Datos
/// </summary>
namespace Presidencia.Programas.Datos
{
    public class Cls_Cat_Nom_Programas_Datos
    {
        public Cls_Cat_Nom_Programas_Datos()
        {
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Programa
        /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta el Programa en la BD con los datos proporcionados por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 26-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Programa(Cls_Cat_Nom_Programas_Negocio Datos)
        {
            String Mi_SQL;   //Obtiene la cadena de inserción hacía la base de datos
            Object Programa_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos

            try
            {
                Mi_SQL = "SELECT NVL(MAX(" + Cat_Nom_Programas.Campo_Programa_ID + "),'00000') ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Nom_Programas.Tabla_Cat_Nom_Programas;
                Programa_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Programa_ID))
                {
                    Datos.P_Programa_ID = "00001";
                }
                else
                {
                    Datos.P_Programa_ID = String.Format("{0:00000}", Convert.ToInt32(Programa_ID) + 1);
                }
                //Consulta para la inserción del programa con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Cat_Nom_Programas.Tabla_Cat_Nom_Programas + " (" + Cat_Nom_Programas.Campo_Programa_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Programas.Campo_Dependencia_ID + ", " + Cat_Nom_Programas.Campo_Nombre + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Programas.Campo_Estatus + ", " + Cat_Nom_Programas.Campo_Comentarios + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Programas.Campo_Usuario_Creo + ", " + Cat_Nom_Programas.Campo_Fecha_Creo + ") VALUES ('";
                Mi_SQL = Mi_SQL + Datos.P_Programa_ID + "', '" + Datos.P_Dependencia_ID + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Nombre + "', '" + Datos.P_Estatus + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Comentarios + "', '" + Datos.P_Nombre_Usuario + "', SYSDATE)";
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
        /// NOMBRE DE LA FUNCION: Modificar_Programa
        /// DESCRIPCION : Modifica los datos del programa con lo que fueron introducidos por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán modificados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 26-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Modificar_Programa(Cls_Cat_Nom_Programas_Negocio Datos)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos

            try
            {
                //Consulta para la modificación del programa con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Cat_Nom_Programas.Tabla_Cat_Nom_Programas + " SET ";
                Mi_SQL = Mi_SQL + Cat_Nom_Programas.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "', ";
                Mi_SQL = Mi_SQL + Cat_Nom_Programas.Campo_Nombre + " = '" + Datos.P_Nombre + "', ";
                Mi_SQL = Mi_SQL + Cat_Nom_Programas.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                Mi_SQL = Mi_SQL + Cat_Nom_Programas.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + Cat_Nom_Programas.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL = Mi_SQL + Cat_Nom_Programas.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                Mi_SQL = Mi_SQL + Cat_Nom_Programas.Campo_Programa_ID + " = '" + Datos.P_Programa_ID + "'";
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
        /// NOMBRE DE LA FUNCION: Eliminar_Programa
        /// DESCRIPCION : Elimina el programa que fue seleccionada por el usuario de la BD
        /// PARAMETROS  : Datos: Obtiene que puesto desea eliminar de la BD
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 26-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Eliminar_Programa(Cls_Cat_Nom_Programas_Negocio Datos)
        {
            String Mi_SQL; //Variable de Consulta para la eliminación del programa
            try
            {
                Mi_SQL = "DELETE FROM " + Cat_Nom_Programas.Tabla_Cat_Nom_Programas + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Nom_Programas.Campo_Programa_ID + " = '" + Datos.P_Programa_ID + "'";
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
        /// NOMBRE DE LA FUNCION: Consulta_Datos_Programas
        /// DESCRIPCION : Consulta todos los datos de los programas que estan dadas de alta en
        ///               la BD con todos sus datos
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 26-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Datos_Programas(Cls_Cat_Nom_Programas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de los programas

            try
            {
                //Consulta todos los datos del programa que se fue seleccionado por el usuario
                Mi_SQL = "SELECT " + Cat_Nom_Programas.Tabla_Cat_Nom_Programas + ".*, ";
                Mi_SQL = Mi_SQL + "(" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ") AS Dependencia";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Nom_Programas.Tabla_Cat_Nom_Programas + ", " + Cat_Dependencias.Tabla_Cat_Dependencias;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Programas.Tabla_Cat_Nom_Programas + "." + Cat_Nom_Programas.Campo_Dependencia_ID + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID;
                if (Datos.P_Programa_ID != null)
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Nom_Programas.Tabla_Cat_Nom_Programas + "." + Cat_Nom_Programas.Campo_Programa_ID + " = '" + Datos.P_Programa_ID + "'";
                }
                if (Datos.P_Nombre != null)
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Nom_Programas.Tabla_Cat_Nom_Programas + "." + Cat_Nom_Programas.Campo_Nombre + " LIKE '%" + Datos.P_Nombre + "%'";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Nom_Programas.Tabla_Cat_Nom_Programas + "." + Cat_Nom_Programas.Campo_Nombre;
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
        /// NOMBRE DE LA FUNCION: Consulta_Programas
        /// DESCRIPCION : Consulta los programas que estan dados de alta en la BD 
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 26-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Programas(Cls_Cat_Nom_Programas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de los programas

            try
            {
                //Consulta los programas que estan dados de alta en la base de datos
                Mi_SQL = "SELECT " + Cat_Nom_Programas.Campo_Programa_ID + ", " + Cat_Nom_Programas.Campo_Nombre;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Nom_Programas.Tabla_Cat_Nom_Programas;
                if (Datos.P_Programa_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Programas.Campo_Programa_ID + " = '" + Datos.P_Programa_ID + "'";
                }
                if (Datos.P_Nombre != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Programas.Campo_Nombre + " LIKE '%" + Datos.P_Nombre + "%'";
                }
                if (Datos.P_Dependencia_ID != null || !Datos.P_Dependencia_ID.Equals(""))
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Programas.Campo_Nombre + " LIKE '%" + Datos.P_Nombre + "%'";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Nom_Programas.Campo_Nombre;
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