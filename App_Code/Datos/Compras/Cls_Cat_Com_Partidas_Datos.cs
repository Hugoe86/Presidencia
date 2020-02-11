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
using Presidencia.Catalogo_Compras_Partidas.Negocio;

namespace Presidencia.Catalogo_Compras_Partidas.Datos
{
    public class Cls_Cat_Com_Partidas_Datos
    {
        public Cls_Cat_Com_Partidas_Datos()
        {
        }


        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Alta_Partida
        /// 	DESCRIPCIÓN: 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///                  2. Da de Alta la Partida en la BD con los datos proporcionados por el usuario
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio con los datos que serán insertados en la base de datos
        /// 	CREO: Roberto González
        /// 	FECHA_CREO: 01-mar-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static void Alta_Partida(Cls_Cat_Com_Partidas_Negocio Datos)
        {
            String Mi_SQL;  //Obtiene la cadena de inserción hacía la base de datos
            Object Partida_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos

            try
            {
                Mi_SQL = "SELECT NVL(MAX(" + Cat_Com_Partidas.Campo_Partida_ID + "),'0000000000') ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas;
                Partida_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Partida_ID))
                {
                    Datos.P_Partida_ID = "0000000001";
                }
                else
                {
                    Datos.P_Partida_ID = String.Format("{0:0000000000}", Convert.ToInt32(Partida_ID) + 1);
                }
                //Consulta para la inserción de la Partida con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas + " (";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Partida_ID + ", ";                
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Partida_Generica_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Clave + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Nombre + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Descripcion + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Cuenta_SAP + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Operacion + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Clave_SAP + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Centro_Aplicacion + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Afecta_Area_Funcional + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Afecta_Partida + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Afecta_Elemento_PEP + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Afecta_Fondo + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Fecha_Creo +", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Descripcion_Especifica;
                Mi_SQL = Mi_SQL + ") VALUES ('";
                Mi_SQL = Mi_SQL + Datos.P_Partida_ID + "', '";                
                Mi_SQL = Mi_SQL + Datos.P_Partida_Generica_ID + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Clave + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Nombre_Partida + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Descripcion + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Estatus + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Cuenta_SAP + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Operacion + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Clave_SAP + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Centro_Aplicacion_SAP + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Afecta_Area_Funcional + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Afecta_Partida + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Afecta_Elemento_PEP + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Afecta_Fondo + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Nombre_Usuario + "', SYSDATE, '";
                Mi_SQL = Mi_SQL + Datos.P_Descripcion_Especifica + "')";
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
        /// 	NOMBRE_FUNCIÓN: Modificar_Partida
        /// 	DESCRIPCIÓN: Modifica los datos de la partida con lo que introdujo el usuario
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio con los datos que serán modificados en la base de datos
        /// 	CREO: Roberto González
        /// 	FECHA_CREO: 01-mar-2011 
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static void Modificar_Partida(Cls_Cat_Com_Partidas_Negocio Datos)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos

            try
            {
                //Consulta para actualizar la fuente de fin. con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas + " SET ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Partida_ID + " = '" + Datos.P_Partida_ID + "', ";                
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Clave + " = '" + Datos.P_Clave + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Partida_Generica_ID + " = '" + Datos.P_Partida_Generica_ID + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Nombre + " = '" + Datos.P_Nombre_Partida + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Descripcion + " = '" + Datos.P_Descripcion + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Cuenta_SAP + " = '" + Datos.P_Cuenta_SAP + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Operacion + " = '" + Datos.P_Operacion + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Clave_SAP + " = '" + Datos.P_Clave_SAP + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Centro_Aplicacion + " = '" + Datos.P_Centro_Aplicacion_SAP + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Afecta_Area_Funcional + " = '" + Datos.P_Afecta_Area_Funcional + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Afecta_Partida + " = '" + Datos.P_Afecta_Partida + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Afecta_Elemento_PEP + " = '" + Datos.P_Afecta_Elemento_PEP + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Afecta_Fondo + " = '" + Datos.P_Afecta_Fondo + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Fecha_Modifico + " = SYSDATE,";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Descripcion_Especifica + "='" + Datos.P_Descripcion_Especifica + "'";   
                Mi_SQL = Mi_SQL + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Partida_ID + " = '" + Datos.P_Partida_ID + "'";
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
        /// 	NOMBRE_FUNCIÓN: Consulta_Datos_Partidas
        /// 	DESCRIPCIÓN: Consulta todos los campos de las Partidas en la BD
        /// 	PARÁMETROS:
        /// 		1. Datos: Indica qué registro se desea consultar a la base de datos
        /// 	CREO: Roberto González
        /// 	FECHA_CREO: 28-feb-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Datos_Partidas(Cls_Cat_Com_Partidas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL
            String Filtro_SQL = ""; //Variable para el filtro de la consulta SQL

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Partida_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Partida_Generica_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Clave + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Nombre + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Descripcion + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Cuenta_SAP + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Operacion + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Clave_SAP + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Centro_Aplicacion + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Afecta_Area_Funcional + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Afecta_Partida + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Afecta_Elemento_PEP + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Descripcion_Especifica + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Afecta_Fondo;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas;
                if (Datos.P_Partida_ID != null)
                {
                    Filtro_SQL = " WHERE " + Cat_Com_Partidas.Campo_Partida_ID + " = '" + Datos.P_Partida_ID + "'";
                }
                else if (Datos.P_Partida_Generica_ID != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND UPPER(" + Cat_Com_Partidas.Campo_Partida_Generica_ID + ") LIKE UPPER('" + Datos.P_Partida_Generica_ID + "')";
                    else
                        Filtro_SQL = " WHERE UPPER(" + Cat_Com_Partidas.Campo_Partida_Generica_ID + ") LIKE UPPER('" + Datos.P_Partida_Generica_ID + "')";
                }
                if (Datos.P_Clave != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND " + Cat_Com_Partidas.Campo_Clave + " LIKE '" + Datos.P_Clave + "'";
                    else
                        Filtro_SQL = " WHERE " + Cat_Com_Partidas.Campo_Clave + " LIKE '" + Datos.P_Clave + "'";
                }
                if (Datos.P_Descripcion != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar OR y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " OR " + Cat_Com_Partidas.Campo_Descripcion + " LIKE '%" + Datos.P_Descripcion + "%'";
                    else
                        Filtro_SQL = " WHERE " + Cat_Com_Partidas.Campo_Descripcion + " LIKE '%" + Datos.P_Descripcion + "%'";
                }
                if (Datos.P_Nombre_Partida != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar OR y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " OR UPPER (" + Cat_Com_Partidas.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Nombre_Partida + "%')";
                    else
                        Filtro_SQL = " WHERE UPPER (" + Cat_Com_Partidas.Campo_Nombre + ") LIKE UPPERE('%" + Datos.P_Nombre_Partida + "%')";
                }
                Mi_SQL = Mi_SQL + Filtro_SQL + " ORDER BY " + Cat_Com_Partidas.Campo_Nombre;
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
        }   //FUNCIÓN: Consulta_Datos_Partidas

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_IDs
        /// 	DESCRIPCIÓN: Consulta los IDs de la Partida genérica, Concepto y Capítulo de una partida específica dada
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio con e registro se desea consultar a la base de datos
        /// 	CREO: Roberto González
        /// 	FECHA_CREO: 02-mar-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_IDs(Cls_Cat_Com_Partidas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de los IDs que 

            try
            {
                //Formar consulta (campos con IDs a traer de las tablas)
                Mi_SQL = "SELECT " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas + "." + Cat_Com_Partidas.Campo_Partida_Generica_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + "." + Cat_SAP_Partida_Generica.Campo_Concepto_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto+ "." + Cat_Sap_Concepto.Campo_Capitulo_ID;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + ", ";
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos;
                ////Si Partida_ID es nulo, asignar caracter vacío para evitar error
                //if (Datos.P_Partida_ID == null)
                //{
                //    Datos.P_Partida_ID = "";
                //}

                Mi_SQL = Mi_SQL + " WHERE ";    // que la Partida especifica en la tabla Cat_Com_Partidas sea igual al ID en la tabla Cat_SAP_Partida_Generica
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Tabla_Cat_Com_Partidas + "." + Cat_Com_Partidas.Campo_Partida_Generica_ID + " = ";
                Mi_SQL = Mi_SQL + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + "." + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID;
                Mi_SQL = Mi_SQL + " AND ";      // que la Partida generica en la tabla Cat_SAP_Partida_Generica sea igual que el ID en la tabla Cat_Sap_Concepto
                Mi_SQL = Mi_SQL + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + "." + Cat_SAP_Partida_Generica.Campo_Concepto_ID + " = ";
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Concepto_ID;
                Mi_SQL = Mi_SQL + " AND ";      // que la Partida generica en la tabla Cat_SAP_Partida_Generica sea igual que el ID en la tabla Cat_Sap_Concepto
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Capitulo_ID + " = ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Capitulo_ID;
                Mi_SQL = Mi_SQL + " AND ";      // para la partida con el ID:
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Tabla_Cat_Com_Partidas + "." + Cat_Com_Partidas.Campo_Partida_ID + " = '" + Datos.P_Partida_ID + "'";
                
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
        }   //FUNCIÓN: Consulta_IDs

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Partidas
        /// 	DESCRIPCIÓN: Consulta las Partidas (Nombre, clave y Partida_ID) en la BD
        /// 	PARÁMETROS:
        /// 		1. Datos: Indica qué registro se desea consultar a la base de datos
        /// 	CREO: Roberto González
        /// 	FECHA_CREO: 28-feb-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Partidas(Cls_Cat_Com_Partidas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de llos productos
            String Filtro_SQL = ""; //Variable para el filtro de la consulta SQL

            try
            {
                Mi_SQL = "SELECT " + Cat_Com_Partidas.Campo_Partida_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Nombre + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Clave + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Clave + " || ' ' || ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Descripcion + " as Clave_Descripcion";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas;
                if (Datos.P_Partida_ID != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND " + Cat_Com_Partidas.Campo_Partida_ID + " = '" + Datos.P_Partida_ID + "'";
                    else
                        Filtro_SQL = " WHERE " + Cat_Com_Partidas.Campo_Partida_ID + " = '" + Datos.P_Partida_ID + "'";
                }
                if (Datos.P_Partida_Generica_ID != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND UPPER(" + Cat_Com_Partidas.Campo_Partida_Generica_ID + ") LIKE UPPER('" + Datos.P_Partida_Generica_ID + "')";
                    else
                        Filtro_SQL = " WHERE UPPER(" + Cat_Com_Partidas.Campo_Partida_Generica_ID + ") LIKE UPPER('" + Datos.P_Partida_Generica_ID + "')";
                }
                if (Datos.P_Clave != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND " + Cat_Com_Partidas.Campo_Clave + " = '" + Datos.P_Clave + "'";
                    else
                        Filtro_SQL = " WHERE " + Cat_Com_Partidas.Campo_Clave + " = '" + Datos.P_Clave + "'";
                }
                if (Datos.P_Descripcion != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " OR UPPER(" + Cat_Com_Partidas.Campo_Descripcion + ") LIKE UPPER('%" + Datos.P_Descripcion + "%')";
                    else
                        Filtro_SQL = " WHERE UPPER(" + Cat_Com_Partidas.Campo_Descripcion + ") LIKE UPPER('%" + Datos.P_Descripcion + "%')";
                }
                if (Datos.P_Nombre_Partida != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " OR UPPER(" + Cat_Com_Partidas.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Nombre_Partida+ "%')";
                    else
                        Filtro_SQL = " WHERE UPPER(" + Cat_Com_Partidas.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Nombre_Partida + "%')";
                }
                Mi_SQL = Mi_SQL + Filtro_SQL + " ORDER BY " + Cat_Com_Partidas.Campo_Clave + " ASC";
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
        }   //FUNCIÓN: Consulta_Partidas

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Capitulos
        /// 	DESCRIPCIÓN: Consulta los Capítulos (CAPITULO_ID, Clave y Descripcion) 
        /// 	            en la BD filtrados por campo
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio que indica el registro a consultar en la base de datos
        /// 	CREO: Roberto González
        /// 	FECHA_CREO: 28-feb-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Capitulos(Cls_Cat_Com_Partidas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de los capítulos

            try
            {
                //Formar consulta (campos a traer de la tabla)
                Mi_SQL = "SELECT " + Cat_SAP_Capitulos.Campo_Capitulo_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Clave + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Descripcion + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Clave + " || ' ' || ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Descripcion + " as Clave_Descripcion";
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
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_SAP_Capitulos.Campo_Clave + " ASC";
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
        /// 	NOMBRE_FUNCIÓN: Consulta_Conceptos
        /// 	DESCRIPCIÓN: Consulta los Conceptos (Conceptos_ID, Clave y Descripcion) 
        /// 	            en la BD filtrados por campo
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio que indica el registro a consultar en la base de datos
        /// 	CREO: Roberto González
        /// 	FECHA_CREO: 28-feb-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Conceptos(Cls_Cat_Com_Partidas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de los Conceptos

            try
            {
                //Formar consulta (campos a traer de la tabla)
                Mi_SQL = "SELECT " + Cat_Sap_Concepto.Campo_Concepto_ID+ ", ";
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Campo_Clave + ", ";
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Campo_Descripcion + ", ";
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Campo_Clave + " || ' ' || ";
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Campo_Descripcion + " as Clave_Descripcion";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto;
                //Filtrar por ID si se proporcionó
                if (Datos.P_Capitulo_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Concepto.Campo_Capitulo_ID + " = '" + Datos.P_Capitulo_ID + "'";
                }
                else if (Datos.P_Concepto_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Concepto.Campo_Concepto_ID + " = '" + Datos.P_Concepto_ID + "'";
                }
                else if (Datos.P_Clave != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE UPPER (" + Cat_Sap_Concepto.Campo_Clave + ") = UPPER('" + Datos.P_Clave + "')";
                }
                else if (Datos.P_Descripcion != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_Sap_Concepto.Campo_Descripcion;
                    Mi_SQL = Mi_SQL + ") LIKE UPPER('%" + Datos.P_Descripcion + "%')";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Sap_Concepto.Campo_Clave + " ASC";
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
        /// 	NOMBRE_FUNCIÓN: Consulta_Partidas_Genericas
        /// 	DESCRIPCIÓN: Consulta las Partidas Genericas (Partida_Generica_ID, Clave y Descripcion) 
        /// 	            en la BD filtrados por campo
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio que indica el registro a consultar en la base de datos
        /// 	CREO: Roberto González
        /// 	FECHA_CREO: 28-feb-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Partidas_Genericas(Cls_Cat_Com_Partidas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de las Partidas Genericas

            try
            {
                //Formar consulta (campos a traer de la tabla)
                Mi_SQL = "SELECT " + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Partida_Generica.Campo_Clave + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Partida_Generica.Campo_Descripcion + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Partida_Generica.Campo_Clave + " || ' ' || " + Cat_SAP_Partida_Generica.Campo_Descripcion + " as Clave_Descripcion";
                Mi_SQL = Mi_SQL + " FROM " + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica;
                //Filtrar por ID si se proporcionó
                if (Datos.P_Concepto_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_SAP_Partida_Generica.Campo_Concepto_ID + " = '" + Datos.P_Concepto_ID + "'";
                }
                else if (Datos.P_Partida_Generica_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID + " = '" + Datos.P_Partida_Generica_ID + "'";
                }
                else if (Datos.P_Clave != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE UPPER (" + Cat_SAP_Partida_Generica.Campo_Clave + ") = UPPER('" + Datos.P_Clave + "')";
                }
                else if (Datos.P_Descripcion != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_SAP_Partida_Generica.Campo_Descripcion;
                    Mi_SQL = Mi_SQL + ") LIKE UPPER('%" + Datos.P_Descripcion + "%')";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_SAP_Partida_Generica.Campo_Clave + " ASC";
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
        /// 	NOMBRE_FUNCIÓN: Consulta_Nombre_Partidas
        /// 	DESCRIPCIÓN: Consulta el nombre de todas las partidas.
        /// 	PARÁMETROS:  Datos: Instancia de la clase de negocio que indica el registro a consultar en la base de datos
        /// 	CREO: Salvador L. Rea Ayala
        /// 	FECHA_CREO: 26/Septiembre/2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Nombre_Partidas()
        {
            String Mi_SQL = ""; //Variable para la consulta de las Partidas Genericas

            try
            {
                Mi_SQL = "SELECT " + Cat_Com_Partidas.Campo_Nombre + ", " + Cat_Com_Partidas.Campo_Partida_ID + " FROM " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas;
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

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Nombre_Cuenta_Partidas
        /// 	DESCRIPCIÓN: Consulta el nombre o el numero de cuenta de la partida.
        /// 	PARÁMETROS:  Datos: Instancia de la clase de negocio que indica el registro a consultar en la base de datos
        /// 	CREO: Salvador L. Rea Ayala
        /// 	FECHA_CREO: 26/Septiembre/2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Nombre_Cuenta_Partidas(Cls_Cat_Com_Partidas_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta de las Partidas Genericas

            try
            {
                if (!string.IsNullOrEmpty(Datos.P_Cuenta_SAP))
                {
                    Mi_SQL = "SELECT " + Cat_Com_Partidas.Campo_Nombre;
                    Mi_SQL += " FROM " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas;
                    Mi_SQL += " WHERE " + Cat_Com_Partidas.Campo_Cuenta_SAP + " = '" + Datos.P_Cuenta_SAP + "'";
                }

                if (!string.IsNullOrEmpty(Datos.P_Nombre_Partida))
                {
                    Mi_SQL = "SELECT " + Cat_Com_Partidas.Campo_Cuenta_SAP;
                    Mi_SQL += " FROM " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas;
                    Mi_SQL += " WHERE " + Cat_Com_Partidas.Campo_Nombre + " = '" + Datos.P_Nombre_Partida + "'";
                }
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
