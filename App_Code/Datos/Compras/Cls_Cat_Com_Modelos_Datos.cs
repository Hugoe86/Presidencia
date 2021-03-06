﻿using System;
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
using Presidencia.Catalogo_Compras_Modelos.Negocio;
using Presidencia.Catalogo_Compras_Subfamilias.Negocio;
using Presidencia.Constantes;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;

/// <summary>
/// Summary description for Cls_Cat_Com_Modelos_Datos
/// </summary>
namespace Presidencia.Catalogo_Compras_Modelos.Datos
{
    public class Cls_Cat_Com_Modelos_Datos
    {
        public Cls_Cat_Com_Modelos_Datos()
        {
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Alta_Modelos
        /// DESCRIPCION:            Dar de Alta un nuevo Modelo a la base de datos
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a ingresar
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            07/Enero/2011 13:42
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static void Alta_Modelos(Cls_Cat_Com_Modelos_Negocio Datos)
        {
            //Declaracion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = String.Empty;
            Object Aux; //Variable auxiliar para las consultas
            String Mensaje = String.Empty; //Variable para el mensaje de error

            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;

                //Consultas para el ID
                Mi_SQL = "SELECT NVL(MAX(" + Cat_Com_Modelos.Campo_Modelo_ID + "), '00000') FROM " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos;

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Aux = Obj_Comando.ExecuteScalar();

                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false)
                    Datos.P_Modelo_ID = String.Format("{0:00000}", Convert.ToInt32(Aux) + 1);
                else
                    Datos.P_Modelo_ID = "00001";

                //Asignar consulta para la insercion
                Mi_SQL = "INSERT INTO " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + " (" + Cat_Com_Modelos.Campo_Modelo_ID + ",";
                    
                Mi_SQL = Mi_SQL + Cat_Com_Modelos.Campo_Nombre + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Modelos.Campo_Estatus + "," + Cat_Com_Modelos.Campo_Comentarios + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Modelos.Campo_Usuario_Creo + "," + Cat_Com_Modelos.Campo_Fecha_Creo + ") ";
                Mi_SQL = Mi_SQL + "VALUES('" + Datos.P_Modelo_ID + "',";
                //Mi_SQL = Mi_SQL + "'" + Datos.P_Subfamilia_ID + "',";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Nombre + "','" + Datos.P_Estatus + "','" + Datos.P_Comentarios + "',";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Usuario + "',SYSDATE)";

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                //Ejecutar transaccion
                Obj_Transaccion.Commit();
                Obj_Conexion.Close();
            }
            catch (OracleException Ex)
            {
                if (Obj_Transaccion != null)
                {
                    Obj_Transaccion.Rollback();
                }
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
                        break;
                    case "923":
                        Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
                        break;
                    case "12170":
                        Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
                        break;
                    default:
                        Mensaje = "Error:  [" + Ex.Message + "]";
                        break;
                }

                throw new Exception(Mensaje, Ex);
            }
            finally
            {
                Obj_Comando = null;
                Obj_Conexion = null;
                Obj_Transaccion = null;
            }
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Baja_Modelos
        /// DESCRIPCION:            Eliminar un Modelo existente de la base de datos
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a eliminar
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            07/Enero/2011 13:46
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static void Baja_Modelos(Cls_Cat_Com_Modelos_Negocio Datos)
        {
            //Declaracion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = String.Empty;
            String Mensaje = String.Empty; //Variable para el mensaje de error

            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;

                //Asignar consulta para la eliminacion
                Mi_SQL = "DELETE FROM " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Modelos.Campo_Modelo_ID + " = '" + Datos.P_Modelo_ID + "'";

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                //Ejecutar transaccion
                Obj_Transaccion.Commit();
                Obj_Conexion.Close();
            }
            catch (OracleException Ex)
            {
                if (Obj_Transaccion != null)
                {
                    Obj_Transaccion.Rollback();
                }
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
                        break;
                    case "923":
                        Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
                        break;
                    case "12170":
                        Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
                        break;
                    default:
                        Mensaje = "Error:  [" + Ex.Message + "]";
                        break;
                }

                throw new Exception(Mensaje, Ex);
            }
            finally
            {
                Obj_Comando = null;
                Obj_Conexion = null;
                Obj_Transaccion = null;
            }
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Cambio_Modelos
        /// DESCRIPCION:            Modificar un Modelo existente de la base de datos
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a modificar
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            07/Enero/2011 13:47
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static void Cambio_Modelos(Cls_Cat_Com_Modelos_Negocio Datos)
        {
            //Declaracion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = String.Empty;
            String Mensaje = String.Empty; //Variable para el mensaje de error

            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;

                //Asignar consulta para la modificacion
                Mi_SQL = "UPDATE " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + " SET ";
                Mi_SQL = Mi_SQL + Cat_Com_Modelos.Campo_Subfamilia_ID + " = '" + Datos.P_Subfamilia_ID + "',";
                Mi_SQL = Mi_SQL + Cat_Com_Modelos.Campo_Nombre + " = '" + Datos.P_Nombre + "',";
                Mi_SQL = Mi_SQL + Cat_Com_Modelos.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Modelos.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Modelos.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Modelos.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Modelos.Campo_Modelo_ID + " = '" + Datos.P_Modelo_ID + "'";

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                //Ejecutar transaccion
                Obj_Transaccion.Commit();
                Obj_Conexion.Close();
            }
            catch (OracleException Ex)
            {
                if (Obj_Transaccion != null)
                {
                    Obj_Transaccion.Rollback();
                }
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
                        break;
                    case "923":
                        Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
                        break;
                    case "12170":
                        Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
                        break;
                    default:
                        Mensaje = "Error:  [" + Ex.Message + "]";
                        break;
                }

                throw new Exception(Mensaje, Ex);
            }
            finally
            {
                Obj_Comando = null;
                Obj_Conexion = null;
                Obj_Transaccion = null;
            }
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Modelos
        /// DESCRIPCION:            Realizar la consulta de los Modelos por criterio de busqueda o por un ID
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a modificar
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            07/Enero/2011 13:46
        /// MODIFICO          : Salvador Hernandez Ramírez
        /// FECHA_MODIFICO    : 17/01/2011
        /// CAUSA_MODIFICACION: Se le completo el campo "Campo_Subfamilia_ID"
        ///*******************************************************************************/
        public static DataTable Consulta_Modelos(Cls_Cat_Com_Modelos_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty;

            try
            {
                //Asignar consulta para los Modelos
                Mi_SQL = "SELECT " + Cat_Com_Modelos.Campo_Modelo_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Subfamilias.Tabla_Cat_Com_Subfamilias + "." + Cat_Com_Subfamilias.Campo_Nombre + " AS Subfamilia, ";
                Mi_SQL = Mi_SQL + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Nombre + " AS Modelo, ";
                Mi_SQL = Mi_SQL + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Comentarios;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + ", " + Cat_Com_Subfamilias.Tabla_Cat_Com_Subfamilias + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Subfamilia_ID + " = ";
                Mi_SQL = Mi_SQL + Cat_Com_Subfamilias.Tabla_Cat_Com_Subfamilias + "." + Cat_Com_Subfamilias.Campo_Subfamilia_ID;

                //Verificar si hay un ID
                if (Datos.P_Modelo_ID != "" && Datos.P_Modelo_ID != String.Empty && Datos.P_Modelo_ID != null)
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Modelos.Campo_Modelo_ID + " = '" + Datos.P_Modelo_ID + "' ";
                //Verificar si hay un nombre de Modelo para filtrar
                if (Datos.P_Nombre != "" && Datos.P_Nombre != String.Empty && Datos.P_Nombre != null)
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Nombre + " LIKE '%" + Datos.P_Nombre + "%' ";

                // Si hay un ID de subfamilia, filtrar por Subfamilia_ID
                if (Datos.P_Subfamilia_ID != "" && Datos.P_Subfamilia_ID != String.Empty && Datos.P_Subfamilia_ID != null)
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Subfamilia_ID + " = '" + Datos.P_Subfamilia_ID + "' ";
                }
                //Ordenar
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Nombre + " ASC";

                //Entregar resultado
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }
    }
}