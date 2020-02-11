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
using Presidencia.Tipos_Contratos.Negocios;

/// <summary>
/// Summary description for Cls_Cat_Nom_Tipos_Contratos_Datos
/// </summary>
namespace Presidencia.Tipos_Contratos.Datos
{
    public class Cls_Cat_Nom_Tipos_Contratos_Datos
    {
        public Cls_Cat_Nom_Tipos_Contratos_Datos()
        {
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Tipo_Contrato
        /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta el tipo de Contrato en la BD con los datos proporcionados
        ///                  por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 27-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Tipo_Contrato(Cls_Cat_Nom_Tipos_Contratos_Negocio Datos)
        {
            String Mi_SQL;        //Obtiene la cadena de inserción hacía la base de datos
            Object Tipo_Contrato_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos

            try
            {
                Mi_SQL = "SELECT NVL(MAX(" + Cat_Nom_Tipos_Contratos.Campo_Tipo_Contrato_ID + "),'00000') ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Nom_Tipos_Contratos.Tabla_Cat_Nom_Tipos_Contratos;
                Tipo_Contrato_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Tipo_Contrato_ID))
                {
                    Datos.P_Tipo_Contrato_ID = "00001";
                }
                else
                {
                    Datos.P_Tipo_Contrato_ID = String.Format("{0:00000}", Convert.ToInt32(Tipo_Contrato_ID) + 1);
                }
                //Consulta para la inserción del tipo de contrato con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Cat_Nom_Tipos_Contratos.Tabla_Cat_Nom_Tipos_Contratos + " (";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Contratos.Campo_Tipo_Contrato_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Contratos.Campo_Descripcion + ", " + Cat_Nom_Tipos_Contratos.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Contratos.Campo_Comentarios + ", " + Cat_Nom_Tipos_Contratos.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Contratos.Campo_Fecha_Creo + ") VALUES ('";
                Mi_SQL = Mi_SQL + Datos.P_Tipo_Contrato_ID + "', '" + Datos.P_Descripcion + "', '";
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
        /// NOMBRE DE LA FUNCION: Modificar_Tipo_Contrato
        /// DESCRIPCION : Modifica los datos del Tipo de Contrato con lo que fueron introducidos por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán modificados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 27-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Modificar_Tipo_Contrato(Cls_Cat_Nom_Tipos_Contratos_Negocio Datos)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos

            try
            {
                //Consulta para la modificación del Tipo de Contrato con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Cat_Nom_Tipos_Contratos.Tabla_Cat_Nom_Tipos_Contratos + " SET ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Contratos.Campo_Descripcion + " = '" + Datos.P_Descripcion + "', ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Contratos.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Contratos.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Contratos.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Contratos.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Contratos.Campo_Tipo_Contrato_ID + " = '" + Datos.P_Tipo_Contrato_ID + "'";
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
        /// NOMBRE DE LA FUNCION: Eliminar_Tipo_Contrato
        /// DESCRIPCION : Elimina el tipo de Contrato que fue seleccionada por el usuario de la BD
        /// PARAMETROS  : Datos: Obtiene que puesto desea eliminar de la BD
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 27-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Eliminar_Tipo_Contrato(Cls_Cat_Nom_Tipos_Contratos_Negocio Datos)
        {
            String Mi_SQL; //Variable de Consulta para la eliminación del tipo de contrato
            try
            {
                Mi_SQL = "DELETE FROM " + Cat_Nom_Tipos_Contratos.Tabla_Cat_Nom_Tipos_Contratos + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Contratos.Campo_Tipo_Contrato_ID + " = '" + Datos.P_Tipo_Contrato_ID + "'";
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
        /// NOMBRE DE LA FUNCION: Consulta_Datos_Tipo_Contrato
        /// DESCRIPCION : Consulta todos los datos de los tipos de contrato que estan dadas de alta en
        ///               la BD con todos sus datos
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 27-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Datos_Tipo_Contrato(Cls_Cat_Nom_Tipos_Contratos_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de los tipos de contratos

            try
            {
                //Consulta todos los datos del tipo de contrato que se fue seleccionado por el usuario
                Mi_SQL = "SELECT * FROM " + Cat_Nom_Tipos_Contratos.Tabla_Cat_Nom_Tipos_Contratos;
                if (Datos.P_Tipo_Contrato_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Tipos_Contratos.Campo_Tipo_Contrato_ID + " = '" + Datos.P_Tipo_Contrato_ID + "'";
                }
                if (Datos.P_Descripcion != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_Nom_Tipos_Contratos.Campo_Descripcion + ") LIKE UPPER('%" + Datos.P_Descripcion + "%')";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Nom_Tipos_Contratos.Campo_Descripcion;
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
        /// NOMBRE DE LA FUNCION: Consulta_Tipos_Contratos
        /// DESCRIPCION : Consulta los tipos de contratos que estan dados de alta en la BD 
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 27-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Tipos_Contratos(Cls_Cat_Nom_Tipos_Contratos_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de los tipos de contrato

            try
            {
                //Consulta los contratos que estan dados de alta en la base de datos
                Mi_SQL = "SELECT " + Cat_Nom_Tipos_Contratos.Campo_Tipo_Contrato_ID + ", " + Cat_Nom_Tipos_Contratos.Campo_Descripcion;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Nom_Tipos_Contratos.Tabla_Cat_Nom_Tipos_Contratos;
                if (Datos.P_Tipo_Contrato_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Tipos_Contratos.Campo_Tipo_Contrato_ID + " = '" + Datos.P_Tipo_Contrato_ID + "'";
                }
                if (Datos.P_Descripcion != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_Nom_Tipos_Contratos.Campo_Descripcion + ") LIKE UPPER('%" + Datos.P_Descripcion + "%')";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Nom_Tipos_Contratos.Campo_Descripcion;
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