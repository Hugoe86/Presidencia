using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Sessiones;
using Presidencia.Bitacora_Eventos;
using Presidencia.Constantes;
using Presidencia.Capitulos.Negocio;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Text;


namespace Presidencia.Capitulos.Datos
{
    public class Cls_Cat_SAP_Capitulos_Datos
    {
        #region Metodos
        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Alta_Capitulos
        /// 	DESCRIPCIÓN: 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///                  2. Da de Alta el capitulo en la BD con los datos proporcionados por el usuario
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio con los datos que serán insertados en la base de datos
        /// 	CREO: Jacqueline Ramírez Sierra
        /// 	FECHA_CREO: 26-feb-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static void Alta_Capitulos(Cls_Cat_SAP_Capitulos_Negocio Datos)
        {
            String Mi_SQL;  //Obtiene la cadena de inserción hacía la base de datos
            Object Capitulo_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos

            try
            {
                Mi_SQL = "SELECT NVL(MAX(" + Cat_SAP_Capitulos.Campo_Capitulo_ID + "),'00000') ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos;
                Capitulo_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Capitulo_ID))
                {
                    Datos.P_Capitulo_ID = "00001";
                }
                else
                {
                    Datos.P_Capitulo_ID = String.Format("{0:00000}", Convert.ToInt32(Capitulo_ID) + 1);
                }
                //Consulta para la inserción del capitulo con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + " (";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Capitulo_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Clave + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Descripcion + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Fecha_Creo + ") VALUES ('";
                Mi_SQL = Mi_SQL + Datos.P_Capitulo_ID + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Clave + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Descripcion + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Estatus + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Nombre_Usuario + "', SYSDATE)";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Modificar_Capitulos
        /// 	DESCRIPCIÓN: Modifica los datos del capitulo con lo que introdujo el usuario
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio con los datos que serán modificados en la base de datos
        /// 	CREO: Jacqueline Ramírez Sierra
        /// 	FECHA_CREO: 26-feb-2011 
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static void Modificar_Capitulos(Cls_Cat_SAP_Capitulos_Negocio Datos)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos

            try
            {
                //Consulta para actualizar el capitulo con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + " SET ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Capitulo_ID + " = '" + Datos.P_Capitulo_ID + "', ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Clave + " = '" + Datos.P_Clave + "', ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Descripcion + " = '" + Datos.P_Descripcion + "', ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Capitulo_ID + " = '" + Datos.P_Capitulo_ID + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Modificar_Eliminar_Combo
        /// 	DESCRIPCIÓN: Modifica los datos del capitulo con lo que introdujo el usuario
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio con los datos que serán modificados en la base de datos
        /// 	CREO: Jacqueline Ramírez Sierra
        /// 	FECHA_CREO: 26-feb-2011 
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Boolean Eliminar_Capitulos(Cls_Cat_SAP_Capitulos_Negocio Datos)
        {
            StringBuilder MI_SQL = new StringBuilder();//variable que almacenará la consulta
            Boolean Operacion_Completa = false;//Estado de la operacion.
            try
            {
                MI_SQL.Append(" UPDATE " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos);
                MI_SQL.Append(" SET " + Cat_SAP_Capitulos.Campo_Estatus + " = '" + Datos.P_Estatus + "', ");
                MI_SQL.Append(Cat_SAP_Capitulos.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ");
                MI_SQL.Append(Cat_SAP_Capitulos.Campo_Fecha_Modifico + " = SYSDATE");
                MI_SQL.Append(" WHERE " + Cat_SAP_Capitulos.Campo_Clave + " = '" + Datos.P_Clave + "'");

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, MI_SQL.ToString());
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al tratar de modificar Eliminar_Capitulos. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Datos_Capitulos
        /// 	DESCRIPCIÓN: Consulta todos los datos de los capitulos dados de alta en la BD y sus relaciones
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio con que indica qué registro se desea consultar a la base de datos
        /// 	CREO: Jacqueline Ramírez Sierra
        /// 	FECHA_CREO: 26-feb-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Datos_Capitulos(Cls_Cat_SAP_Capitulos_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de los capitulos.
            String Filtro_SQL = ""; // Almacenar cadena para filtrar datos

            try
            {
                //Consulta todos los datos de los capitulos seleccionado por el usuario
                Mi_SQL = "SELECT * FROM " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos;

                //Si la propiedad P_Capitulo_ID no es null, agregar filtro por ID
                if (Datos.P_Capitulo_ID != null)
                {
                    Filtro_SQL = Filtro_SQL + " WHERE " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + ".";
                    Filtro_SQL = Filtro_SQL + Cat_SAP_Capitulos.Campo_Capitulo_ID + " = '" + Datos.P_Capitulo_ID + "'";
                }
                //Si la propiedad P_Clave no es null, agregar filtro por Clave
                if (Datos.P_Clave != null)
                {
                    if (Filtro_SQL.Length < 1)  // Verificar si ya hay una condición para seguir con AND o comenzar con WHERE
                        Filtro_SQL = Filtro_SQL + " WHERE ";
                    else
                        Filtro_SQL = Filtro_SQL + " AND ";
                    Filtro_SQL = Filtro_SQL + " UPPER(" + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + ".";
                    Filtro_SQL = Filtro_SQL + Cat_SAP_Capitulos.Campo_Clave + ") LIKE UPPER('%" + Datos.P_Clave + "%')";
                }
                //Si la propiedad P_Descripcion no es null, agregar filtro por Descripción
                if (Datos.P_Descripcion != null)
                {
                    if (Filtro_SQL.Length < 1)  // Verificar si ya hay una condición para seguir con AND o comenzar con WHERE
                        Filtro_SQL = Filtro_SQL + " WHERE ";
                    else
                        Filtro_SQL = Filtro_SQL + " OR ";
                    Filtro_SQL = Filtro_SQL + " UPPER(" + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + ".";
                    Filtro_SQL = Filtro_SQL + Cat_SAP_Capitulos.Campo_Descripcion + ") LIKE UPPER('%" + Datos.P_Descripcion + "%')";
                }
                //Ordenar por Descripción
                Filtro_SQL = Filtro_SQL + " ORDER BY " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + ".";
                Filtro_SQL = Filtro_SQL + Cat_SAP_Capitulos.Campo_Clave + " ASC";

                //Regresar la Consulta de Mi_SQL + Filtro_SQL
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL + Filtro_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
      
        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Capitulos
        /// 	DESCRIPCIÓN: Consulta los capitulos (Capitulo_ID, Clave y Descripcion) 
        /// 	            en la BD filtrados por campo
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio que indica el registro a consultar en la base de datos
        /// 	CREO: Jacqueline Ramírez sierra
        /// 	FECHA_CREO: 26-feb-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Capitulos(Cls_Cat_SAP_Capitulos_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de los capitulos

            try
            {
                //Formar consulta (campos a traer de la tabla)
                Mi_SQL = "SELECT " + Cat_SAP_Capitulos.Campo_Capitulo_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Clave + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Descripcion;
                Mi_SQL = Mi_SQL + " FROM " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos;
                //Filtrar por ID si se proporcionó
                if (Datos.P_Capitulo_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_SAP_Capitulos.Campo_Capitulo_ID + " = '" + Datos.P_Capitulo_ID + "'";
                }
                else if (Datos.P_Clave != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE UPPER (" + Cat_SAP_Capitulos.Campo_Clave + ") = UPPER('" + Datos.P_Clave + "')";
                }
                else if (Datos.P_Descripcion != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_SAP_Capitulos.Campo_Descripcion;
                    Mi_SQL = Mi_SQL + ") LIKE UPPER('%" + Datos.P_Descripcion + "%')";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_SAP_Capitulos.Campo_Clave;
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            finally
            {
            }
        }
        #endregion
    }
        
}
