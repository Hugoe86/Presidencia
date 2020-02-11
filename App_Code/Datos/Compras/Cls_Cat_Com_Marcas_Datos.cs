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
using Presidencia.Catalogo_Compras_Marcas.Negocio;
using Presidencia.Constantes;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;

/// <summary>
/// Summary description for Cls_Cat_Com_Marcas_Datos
/// </summary>
namespace Presidencia.Catalogo_Compras_Marcas.Datos
{
    public class Cls_Cat_Com_Marcas_Datos
    {
        public Cls_Cat_Com_Marcas_Datos()
        {
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Alta_Marcas
        /// DESCRIPCION:            Dar de Alta un nuevo Marca a la base de datos
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a ingresar
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            07/Enero/2011 17:21 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static void Alta_Marcas(Cls_Cat_Com_Marcas_Negocio Datos)
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
                Mi_SQL = "SELECT NVL(MAX(" + Cat_Com_Marcas.Campo_Marca_ID + "), '00000') FROM " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Aux = Obj_Comando.ExecuteScalar();

                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false)
                    Datos.P_Marca_ID = String.Format("{0:00000}", Convert.ToInt32(Aux) + 1);
                else
                    Datos.P_Marca_ID = "00001";

                //Asignar consulta para la insercion
                Mi_SQL = "INSERT INTO " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " (" + Cat_Com_Marcas.Campo_Marca_ID + "," + Cat_Com_Marcas.Campo_Nombre + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Marcas.Campo_Estatus + "," + Cat_Com_Marcas.Campo_Comentarios + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Marcas.Campo_Usuario_Creo + "," + Cat_Com_Marcas.Campo_Fecha_Creo + ") ";
                Mi_SQL = Mi_SQL + "VALUES('" + Datos.P_Marca_ID + "','" + Datos.P_Nombre + "','" + Datos.P_Estatus + "','" + Datos.P_Comentarios + "',";
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
        /// NOMBRE DE LA CLASE:     Baja_Marcas
        /// DESCRIPCION:            Eliminar un Marca existente de la base de datos
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a eliminar
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            07/Enero/2011 17:22 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static void Baja_Marcas(Cls_Cat_Com_Marcas_Negocio Datos)
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
                Mi_SQL = "DELETE FROM " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Marcas.Campo_Marca_ID + " = '" + Datos.P_Marca_ID + "'";

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
        /// NOMBRE DE LA CLASE:     Cambio_Marcas
        /// DESCRIPCION:            Modificar un Marca existente de la base de datos
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a modificar
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            07/Enero/2011 17:23 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static void Cambio_Marcas(Cls_Cat_Com_Marcas_Negocio Datos)
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
                Mi_SQL = "UPDATE " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " SET " + Cat_Com_Marcas.Campo_Nombre + " = '" + Datos.P_Nombre + "',";
                Mi_SQL = Mi_SQL + Cat_Com_Marcas.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Marcas.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Marcas.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Marcas.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Marcas.Campo_Marca_ID + " = '" + Datos.P_Marca_ID + "'";

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
        /// NOMBRE DE LA CLASE:     Consulta_Marcas
        /// DESCRIPCION:            Realizar la consulta de los Marcas por criterio de busqueda o por un ID
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a modificar
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            07/Enero/2011 17:23 
        /// MODIFICO          :     Jesus Toledo
        /// FECHA_MODIFICO    :     09/Febrero/2011
        /// CAUSA_MODIFICACION:     agregue la clausula upper en la consulta
        ///*******************************************************************************/
        public static DataTable Consulta_Marcas(Cls_Cat_Com_Marcas_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty;

            try
            {
                //Asignar consulta para los Marcas
                Mi_SQL = "SELECT " + Cat_Com_Marcas.Campo_Marca_ID + "," + Cat_Com_Marcas.Campo_Nombre + "," + Cat_Com_Marcas.Campo_Estatus + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Marcas.Campo_Comentarios + " FROM " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " ";

                //Verificar si hay un ID
                if (Datos.P_Marca_ID != "" && Datos.P_Marca_ID != String.Empty && Datos.P_Marca_ID != null)
                    Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Marcas.Campo_Marca_ID + " = '" + Datos.P_Marca_ID + "' ";
                else
                    Mi_SQL = Mi_SQL + "WHERE upper(" + Cat_Com_Marcas.Campo_Nombre + ") LIKE upper('%" + Datos.P_Nombre + "%') ";

                //Ordenar
                Mi_SQL = Mi_SQL + "ORDER BY " + Cat_Com_Marcas.Campo_Nombre;

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