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
using Presidencia.Catalogo_Compras_Presupuesto_Dependencias.Negocio;
using Presidencia.Constantes;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;

namespace Presidencia.Catalogo_Compras_Presupuesto_Dependencias.Datos
{
    
    public class Cls_Cat_Com_Presupuesto_Dependencias_Datos
    {
        ///*******************************************************************************
        /// NOMBRE DEL METODO:      Alta_Presupuesto
        /// DESCRIPCION:            Dar de Alta un nuevo Familia a la base de datos
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a ingresar
        /// CREO       :            Jacqueline Ramírez Sierra
        /// FECHA_CREO :            04/Febrero/2011  
        /// MODIFICO          :     Jesus Toledo
        /// FECHA_MODIFICO    :     10/Febrero/2011
        /// CAUSA_MODIFICACION:     Se modifico por modificacion a la base de datos
        ///*******************************************************************************/
        public static void Alta_Presupuesto(Cls_Cat_Com_Presupuesto_Dependencias_Negocio Datos)
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

                ////Consultas para el ID
                Mi_SQL = "SELECT MAX(" + Cat_Com_Dep_Presupuesto.Campo_Presupuesto_ID + ") FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Aux = Obj_Comando.ExecuteScalar();

                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false)
                    Datos.P_Presupuesto_ID = Convert.ToInt32(Aux) + 1;
                else
                    Datos.P_Presupuesto_ID = 1;

                //Asignar consulta para la insercion
                Mi_SQL = "INSERT INTO " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + " (" + Cat_Com_Dep_Presupuesto.Campo_Presupuesto_ID +", "+ Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + "," + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Presupuestal + "," + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + "," + Cat_Com_Dep_Presupuesto.Campo_Comentarios + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Usuario_Creo + "," + Cat_Com_Dep_Presupuesto.Campo_Fecha_Creo + ") ";
                Mi_SQL = Mi_SQL + "VALUES( " + Datos.P_Presupuesto_ID + ",'" + Datos.P_Dependencia_ID + "','" + Datos.P_Anio_Presupuesto + "',";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Monto_Presupuestal + "','" + Datos.P_Monto_Comprometido + "','" + Datos.P_Monto_Disponible + "',";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Comentarios + "','" + Datos.P_Usuario_Creo + "',SYSDATE)";

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
        /// NOMBRE DEL METODO:     Baja_Presupuesto
        /// DESCRIPCION:            Eliminar un Presupuesto existente de la base de datos
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a eliminar
        /// CREO       :            Jacqueline Ramírez Sierra
        /// FECHA_CREO :            04/Febrero/2011 
        /// MODIFICO          :     Jesus Toledo
        /// FECHA_MODIFICO    :     10/Febrero/2011
        /// CAUSA_MODIFICACION:     Se modifico por modificacion a la base de datos
        ///*******************************************************************************/
        public static void Baja_Presupuesto(Cls_Cat_Com_Presupuesto_Dependencias_Negocio Datos)
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
                Mi_SQL = "DELETE FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Dep_Presupuesto.Campo_Presupuesto_ID + " = '" + Datos.P_Presupuesto_ID + "'";

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
        /// NOMBRE DEL METODO:     Cambio_Presupuesto
        /// DESCRIPCION:            Modificar un Presupuesto existente de la base de datos
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a modificar
        /// CREO       :            Jacqueline Ramírez Sierra
        /// FECHA_CREO :            04/Febrero/2011  
        /// MODIFICO          :     Jesus Toledo
        /// FECHA_MODIFICO    :     10/Febrero/2011
        /// CAUSA_MODIFICACION:     Se modifico por modificacion a la base de datos
        ///*******************************************************************************/
        public static void Cambio_Presupuesto(Cls_Cat_Com_Presupuesto_Dependencias_Negocio Datos)
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
                Mi_SQL = "UPDATE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + " SET " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + " = '" + Datos.P_Anio_Presupuesto + "',";
                Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Presupuestal + " = '" + Datos.P_Monto_Presupuestal + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + " = '" + Datos.P_Monto_Comprometido + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + " = '" + Datos.P_Monto_Disponible + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Creo + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Dep_Presupuesto.Campo_Presupuesto_ID + " = '" + Datos.P_Presupuesto_ID + "'";

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
        /// NOMBRE DEL METODO:     Consulta_Presupuestos
        /// DESCRIPCION:            Realizar la consulta del Presupuesto por criterio de busqueda o por un ID
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a modificar
        /// CREO       :            Jacqueline Ramírez Sierra
        /// FECHA_CREO :            04/Febrero/2011
        /// MODIFICO          :     Jesus Toledo Rdz
        /// FECHA_MODIFICO    :     10/Febrero/2011 12:24
        /// CAUSA_MODIFICACION:     Se modifico la consulta para traer el nombre de 
        ///                         la dependencia y buscar por este mismo o por año
        ///*******************************************************************************/
        public static DataTable Consulta_Presupuestos(Cls_Cat_Com_Presupuesto_Dependencias_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty;

            try
            {
                //Asignar consulta para los Familias
                Mi_SQL = "SELECT " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Presupuesto_ID + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Monto_Presupuestal + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Comentarios + ",";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA " ;    
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + " ";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + " = " +
                    Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " ";
                //Verificar si hay un ID
                if (Datos.P_Dependencia_ID != "" && Datos.P_Dependencia_ID != String.Empty && Datos.P_Dependencia_ID != null)
                {
                    Mi_SQL = Mi_SQL + "AND ( upper( " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ") LIKE upper('%" + Datos.P_Dependencia_ID + "%') ";
                    Mi_SQL = Mi_SQL + "OR " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + " = '" + Datos.P_Anio_Presupuesto+ "' )";
                }
                //Ordenar
                Mi_SQL = Mi_SQL + "ORDER BY " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto;

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