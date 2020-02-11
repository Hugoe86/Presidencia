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
using Presidencia.Catalogo_Predial_Tipos_Documento.Negocio;

namespace Presidencia.Catalogo_Predial_Tipos_Documento.Datos
{

    public class Cls_Cat_Pre_Tipos_Documento_Datos
    {
	    public Cls_Cat_Pre_Tipos_Documento_Datos()
	    {
	    }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Alta_Tipo_Documento
        /// DESCRIPCIÓN: 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///                  2. Da de Alta el tipo de documento en la BD con los datos proporcionados por el usuario
        ///                  Regresa el número de filas insertadas
        /// PARÁMETROS:
        /// 	1. Datos: Instancia de la clase de negocio con los datos que serán insertados en la base de datos
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 22-mar-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Alta_Tipo_Documento(Cls_Cat_Pre_Tipos_Documento_Negocio Datos)
        {
            String Mi_SQL;  //Obtiene la cadena de inserción hacía la base de datos
            Object Documento_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos

            try
            {
                Mi_SQL = "SELECT NVL(MAX(" + Cat_Pre_Tipos_Documento.Campo_Documento_ID + "),'00000') ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Pre_Tipos_Documento.Tabla_Cat_Pre_Tipos_Documento;
                Documento_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Documento_ID))
                {
                    Datos.P_Documento_ID = "00001";
                }
                else
                {
                    Datos.P_Documento_ID = String.Format("{0:00000}", Convert.ToInt32(Documento_ID) + 1);
                }
                //Consulta para la inserción del Tipo de documento con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Cat_Pre_Tipos_Documento.Tabla_Cat_Pre_Tipos_Documento + " (";
                Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Documento.Campo_Documento_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Documento.Campo_Nombre_Documento + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Documento.Campo_Descripcion + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Documento.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Documento.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Documento.Campo_Fecha_Creo + ") VALUES ('";
                Mi_SQL = Mi_SQL + Datos.P_Documento_ID + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Nombre_Documento + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Descripcion + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Estatus + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Nombre_Usuario + "', SYSDATE)";
                
                //regresar el número de inserciones realizadas
                return OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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


        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Modificar_Tipo_Documento
        /// DESCRIPCIÓN: Modifica los datos del tipo de documento con lo que introdujo el usuario
        ///             Regresa el número de filas modificadas
        /// PARÁMETROS:
        /// 	1. Datos: Instancia de la clase de negocio con los datos que serán modificados en la base de datos
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 22-mar-2011 
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Modificar_Tipo_Documento(Cls_Cat_Pre_Tipos_Documento_Negocio Datos)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos

            try
            {
                //Consulta para actualizar los datos del tipo de documento con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Cat_Pre_Tipos_Documento.Tabla_Cat_Pre_Tipos_Documento + " SET ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Documento.Campo_Documento_ID + " = '" + Datos.P_Documento_ID + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Documento.Campo_Nombre_Documento + " = '" + Datos.P_Nombre_Documento + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Documento.Campo_Descripcion + " = '" + Datos.P_Descripcion + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Documento.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Documento.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Documento.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Documento.Campo_Documento_ID + " = '" + Datos.P_Documento_ID + "'";

                //regresar el número de filas modificadas
                return OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consulta_Datos_Tipos_Documento
        /// DESCRIPCIÓN: Consulta todos los campos de los tipos de documento en la BD
        /// PARÁMETROS:
        /// 	1. Datos: Indica qué registro se desea consultar a la base de datos
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 22-mar-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Datos_Tipos_Documento(Cls_Cat_Pre_Tipos_Documento_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL
            String Filtro_SQL = ""; //Variable para el filtro de la consulta SQL

            try
            {
                Mi_SQL = "SELECT * FROM " + Cat_Pre_Tipos_Documento.Tabla_Cat_Pre_Tipos_Documento;
                if (Datos.P_Documento_ID != null)      // Si se recibió un ID de documento filtrar por ese ID
                {
                    Filtro_SQL = " WHERE " + Cat_Pre_Tipos_Documento.Campo_Documento_ID + " = '" + Datos.P_Documento_ID + "'";
                }
                if (Datos.P_Estatus != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar OR y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND UPPER(" + Cat_Pre_Tipos_Documento.Campo_Estatus + ") = UPPER('" + Datos.P_Estatus + "')";
                    else
                        Filtro_SQL = " WHERE UPPER(" + Cat_Pre_Tipos_Documento.Campo_Estatus + ") = UPPER('" + Datos.P_Estatus + "')";
                }
                if (Datos.P_Nombre_Documento != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND UPPER(" + Cat_Pre_Tipos_Documento.Campo_Nombre_Documento + ") LIKE UPPER('" + Datos.P_Nombre_Documento + "')";
                    else
                        Filtro_SQL = " WHERE UPPER(" + Cat_Pre_Tipos_Documento.Campo_Nombre_Documento + ") LIKE UPPER('" + Datos.P_Nombre_Documento + "')";
                }
                if (Datos.P_Descripcion != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar OR y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " OR UPPER(" + Cat_Pre_Tipos_Documento.Campo_Descripcion + ") LIKE UPPER('%" + Datos.P_Descripcion + "%')";
                    else
                        Filtro_SQL = " WHERE UPPER(" + Cat_Pre_Tipos_Documento.Campo_Descripcion + ") LIKE UPPER('%" + Datos.P_Descripcion + "%')";
                }
                Mi_SQL = Mi_SQL + Filtro_SQL + " ORDER BY " + Cat_Pre_Tipos_Documento.Campo_Nombre_Documento;

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
        }   //FUNCIÓN: Consulta_Datos_Tipos_Documento

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consulta_Tipos_Documento
        /// DESCRIPCIÓN: Consulta los campos Documento_ID y nombre_Documento de los tipos de documento en la BD
        /// PARÁMETROS:
        /// 	1. Datos: Indica qué registro se desea consultar a la base de datos
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 22-mar-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Tipos_Documento(Cls_Cat_Pre_Tipos_Documento_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL
            String Filtro_SQL = ""; //Variable para el filtro de la consulta SQL

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Documento.Campo_Documento_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Documento.Campo_Nombre_Documento + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Documento.Campo_Descripcion;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Tipos_Documento.Tabla_Cat_Pre_Tipos_Documento;

                if (Datos.P_Documento_ID != null)   // Si se recibió un ID de documento filtrar por ese ID
                {
                    Filtro_SQL = " WHERE " + Cat_Pre_Tipos_Documento.Campo_Documento_ID + " = '" + Datos.P_Documento_ID + "'";
                }
                if (Datos.P_Estatus != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar OR y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND UPPER(" + Cat_Pre_Tipos_Documento.Campo_Estatus + ") = UPPER('" + Datos.P_Estatus + "')";
                    else
                        Filtro_SQL = " WHERE UPPER(" + Cat_Pre_Tipos_Documento.Campo_Estatus + ") = UPPER('" + Datos.P_Estatus + "')";
                }
                if (Datos.P_Nombre_Documento != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND UPPER(" + Cat_Pre_Tipos_Documento.Campo_Nombre_Documento + ") LIKE UPPER('" + Datos.P_Nombre_Documento + "')";
                    else
                        Filtro_SQL = " WHERE UPPER(" + Cat_Pre_Tipos_Documento.Campo_Nombre_Documento + ") LIKE UPPER('" + Datos.P_Nombre_Documento + "')";
                }
                if (Datos.P_Descripcion != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar OR y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " OR " + Cat_Pre_Tipos_Documento.Campo_Descripcion + " LIKE '%" + Datos.P_Descripcion + "%'";
                    else
                        Filtro_SQL = " WHERE " + Cat_Pre_Tipos_Documento.Campo_Descripcion + " LIKE '%" + Datos.P_Descripcion + "%'";
                }
                Mi_SQL = Mi_SQL + Filtro_SQL + " ORDER BY " + Cat_Pre_Tipos_Documento.Campo_Nombre_Documento;

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
        }   //FUNCIÓN: Consulta_Tipos_Documento


    }//termina clase Cls_Cat_Pre_Tipos_Documento_Datos

}//termina namespace