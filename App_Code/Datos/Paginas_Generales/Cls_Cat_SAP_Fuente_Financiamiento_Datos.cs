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
using Presidencia.Catalogo_SAP_Fuente_Financiamiento.Negocio;
using Presidencia.Dependencias.Negocios;


namespace Presidencia.Catalogo_SAP_Fuente_Financiamiento.Datos
{
    public class Cls_Cat_SAP_Fuente_Financiamiento_Datos
    {
        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Alta_Fuente_Financiamiento
        /// 	DESCRIPCIÓN: 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///                  2. Da de Alta la Fuente de fin. en la BD con los datos proporcionados por el usuario
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio con los datos que serán insertados en la base de datos
        /// 	CREO: Roberto González
        /// 	FECHA_CREO: 25-feb-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static void Alta_Fuente_Financiamiento(Cls_Cat_SAP_Fuente_Financiamiento_Negocio Datos)
        {
            String Mi_SQL;  //Obtiene la cadena de inserción hacía la base de datos
            Object Fuente_Financiamiento_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos

            try
            {
                Mi_SQL = "SELECT NVL(MAX(" + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + "),'00000') ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento;
                Fuente_Financiamiento_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Fuente_Financiamiento_ID))
                {
                    Datos.P_Fuente_Financiamiento_ID = "00001";
                }
                else
                {
                    Datos.P_Fuente_Financiamiento_ID = String.Format("{0:00000}", Convert.ToInt32(Fuente_Financiamiento_ID) + 1);
                }
                //Consulta para la inserción de la fuente de fin con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + " (";
                Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Clave + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Especiales_Ramo_33 + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Fecha_Creo + ",";
                Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Anio + ") VALUES ('";
                Mi_SQL = Mi_SQL + Datos.P_Fuente_Financiamiento_ID + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Clave + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Descripcion + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Estatus + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Especiales_Ramo_33 + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Nombre_Usuario + "', SYSDATE,";
                Mi_SQL = Mi_SQL + Datos.P_Anio + ")";
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
        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Modificar_Fuente_Financiamiento
        /// 	DESCRIPCIÓN: Modifica los datos de la fuente de financiamiento con lo que introdujo el usuario
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio con los datos que serán modificados en la base de datos
        /// 	CREO: Roberto González
        /// 	FECHA_CREO: 25-feb-2011 
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static void Modificar_Fuente_Financiamiento(Cls_Cat_SAP_Fuente_Financiamiento_Negocio Datos)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos

            try
            {
                //Consulta para actualizar la fuente de fin. con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + " SET ";
                Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + " = '" + Datos.P_Fuente_Financiamiento_ID + "', ";
                Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " = '" + Datos.P_Clave + "', ";
                Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + " = '" + Datos.P_Descripcion + "', ";
                Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Fecha_Modifico + " = SYSDATE,";
                Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Anio + "= " + Datos.P_Anio + ",";
                Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Especiales_Ramo_33 + "= '" + Datos.P_Especiales_Ramo_33 + "'";
                Mi_SQL = Mi_SQL + "WHERE ";
                Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + " = '" + Datos.P_Fuente_Financiamiento_ID + "'";
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
        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Datos_Fuente_Financiamiento
        /// 	DESCRIPCIÓN: Consulta todos los datos de las Fuentes de financiamiento dados de alta en la BD y sus relaciones
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio con que indica qué registro se desea consultar a la base de datos
        /// 	CREO: Roberto González
        /// 	FECHA_CREO: 25-feb-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Datos_Fuente_Financiamiento(Cls_Cat_SAP_Fuente_Financiamiento_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de los Fuentes de fin.
            String Filtro_SQL = ""; // Almacenar cadena para filtrar datos

            try
            {
                //Consulta todos los datos de la fuente de fin. seleccionado por el usuario
                Mi_SQL = "SELECT * FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento;

                //Si la propiedad P_Fuente_Financiamiento_ID no es null, agregar filtro por ID
                if (Datos.P_Fuente_Financiamiento_ID != null)
                {
                    Filtro_SQL = Filtro_SQL + " WHERE " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + ".";
                    Filtro_SQL = Filtro_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + " = '" + Datos.P_Fuente_Financiamiento_ID + "'";
                }
                //Si la propiedad P_Clave no es null, agregar filtro por Clave
                if (Datos.P_Clave != null)
                {
                    if (Filtro_SQL.Length < 1)  // Verificar si ya hay una condición para seguir con AND o comenzar con WHERE
                        Filtro_SQL = Filtro_SQL + " WHERE ";
                    else
                        Filtro_SQL = Filtro_SQL + " AND ";
                    Filtro_SQL = Filtro_SQL + " UPPER(" + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + ".";
                    Filtro_SQL = Filtro_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Clave + ") LIKE UPPER('%" + Datos.P_Clave + "%')";
                }
                //Si la propiedad P_Descripcion no es null, agregar filtro por Descripción
                if (Datos.P_Descripcion != null)
                {
                    if (Filtro_SQL.Length < 1)  // Verificar si ya hay una condición para seguir con AND o comenzar con WHERE
                        Filtro_SQL = Filtro_SQL + " WHERE ";
                    else
                        Filtro_SQL = Filtro_SQL + " OR ";
                    Filtro_SQL = Filtro_SQL + " UPPER(" + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + ".";
                    Filtro_SQL = Filtro_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + ") LIKE UPPER('%" + Datos.P_Descripcion+ "%')";
                }
                //Ordenar por Descripción
                Filtro_SQL = Filtro_SQL + " ORDER BY " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + ".";
                Filtro_SQL = Filtro_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion;

                //Regresar la Consulta de Mi_SQL + Filtro_SQL
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL + Filtro_SQL).Tables[0];
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
        /// 	NOMBRE_FUNCIÓN: Consulta_Fuente_Financiamiento
        /// 	DESCRIPCIÓN: Consulta las fuentes de financiamieno (Fuente_Financiamiento_ID, Clave y Descripcion) 
        /// 	            en la BD filtrados por campo
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio que indica el registro a consultar en la base de datos
        /// 	CREO: Roberto González
        /// 	FECHA_CREO: 25-feb-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Fuente_Financiamiento(Cls_Cat_SAP_Fuente_Financiamiento_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de las fuentes de financiamiento

            try
            {
                //Formar consulta (campos a traer de la tabla)
                Mi_SQL = "SELECT " + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Clave + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion;
                Mi_SQL = Mi_SQL + " FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento;
                //Filtrar por ID si se proporcionó
                if (Datos.P_Fuente_Financiamiento_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID+ " = '" + Datos.P_Fuente_Financiamiento_ID + "'";
                }
                else if (Datos.P_Clave != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE UPPER (" + Cat_SAP_Fuente_Financiamiento.Campo_Clave+ ") = UPPER('" + Datos.P_Clave + "')";
                }
                else if (Datos.P_Descripcion != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion;
                    Mi_SQL = Mi_SQL + ") LIKE UPPER('%" + Datos.P_Descripcion+ "%')";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_SAP_Fuente_Financiamiento.Campo_Clave;
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
        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Fuente_Financiamiento_Especial
        /// 	DESCRIPCIÓN: Consulta las fuentes de financiamiento relacionadas con la dependencia
        /// 	PARÁMETROS: Datos: Instancia de la clase de negocio que indica el registro a consultar en la base de datos
        /// 	CREO: Salvador L. Rea Ayala
        /// 	FECHA_CREO: 4/Octubre/2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Fuente_Financiamiento_Especial(Cls_Cat_SAP_Fuente_Financiamiento_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de las fuentes de financiamiento

            try
            {
                //Formar consulta (campos a traer de la tabla)
                Mi_SQL = "SELECT " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + ", ";
                Mi_SQL += Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " ||' - '|| " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + " AS CLAVE_NOMBRE";
                Mi_SQL += " FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + " RIGHT OUTER JOIN " + Cat_SAP_Det_Fte_Dependencia.Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia;
                Mi_SQL += " ON " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + " = " + Cat_SAP_Det_Fte_Dependencia.Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia + "." + Cat_SAP_Det_Fte_Dependencia.Campo_Fuente_Financiamiento_ID;
                Mi_SQL += " WHERE " + Cat_SAP_Det_Fte_Dependencia.Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia + "." + Cat_SAP_Det_Fte_Dependencia.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "'";
                Mi_SQL += " ORDER BY " + Cat_SAP_Det_Fte_Dependencia.Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia + "." + Cat_SAP_Det_Fte_Dependencia.Campo_Fuente_Financiamiento_ID + " ASC";
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
        }
    }

}
