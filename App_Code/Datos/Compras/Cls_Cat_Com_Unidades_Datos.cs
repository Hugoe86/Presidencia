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
using Presidencia.Catalogo_Compras_Unidades.Negocio;
using Presidencia.Constantes;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;

/// <summary>
/// Summary description for Cls_Cat_Com_Unidades_Datos
/// </summary>
namespace Presidencia.Catalogo_Compras_Unidades.Datos
{
    public class Cls_Cat_Com_Unidades_Datos
    {
        public Cls_Cat_Com_Unidades_Datos()
        {
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Alta_Unidades
        /// DESCRIPCION:            Dar de Alta una nueva unidad a la base de datos
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a ingresar
        /// CREO       :            Noe Mosqueda Valadez
        /// FECHA_CREO :            08/Noviembre/2010 10:11 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static void Alta_Unidades(Cls_Cat_Com_Unidades_Negocio Datos)
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

                //Asignar consulta para el ID
                Mi_SQL = "SELECT NVL(MAX(" + Cat_Com_Unidades.Campo_Unidad_ID + "), '00000') FROM " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades;

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Aux = Obj_Comando.ExecuteScalar();

                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false)
                    Datos.P_Unidad_ID = String.Format("{0:00000}", Convert.ToInt32(Aux) + 1);
                else
                    Datos.P_Unidad_ID = "00001";

                //Consulta para la insercion
                Mi_SQL = "INSERT INTO " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " (" + Cat_Com_Unidades.Campo_Unidad_ID + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Unidades.Campo_Nombre + "," + Cat_Com_Unidades.Campo_Abreviatura + "," + Cat_Com_Unidades.Campo_Comentarios + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Unidades.Campo_Usuario_Creo + "," + Cat_Com_Unidades.Campo_Fecha_Creo + ") VALUES('" + Datos.P_Unidad_ID + "',";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Nombre + "','" + Datos.P_Abreviatura + "','" + Datos.P_Comentarios + "',";
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
        /// NOMBRE DE LA CLASE:     Baja_Unidades
        /// DESCRIPCION:            Eliminar una unidad existente de la base de datos
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a eliminar
        /// CREO       :            Noe Mosqueda Valadez
        /// FECHA_CREO :            08/Noviembre/2010 10:22 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static void Baja_Unidades(Cls_Cat_Com_Unidades_Negocio Datos)
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

                //Asignar consulta para la baja
                Mi_SQL = Mi_SQL + "DELETE FROM " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Unidades.Campo_Unidad_ID + " = '" + Datos.P_Unidad_ID + "'";

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
        /// NOMBRE DE LA CLASE:     Cambio_Unidades
        /// DESCRIPCION:            Modificar una unidad existente de la base de datos
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a modificar
        /// CREO       :            Noe Mosqueda Valadez
        /// FECHA_CREO :            08/Noviembre/2010 10:50 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static void Cambio_Unidades(Cls_Cat_Com_Unidades_Negocio Datos)
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

                //Consulta para la modificacion de las unidades
                Mi_SQL = "UPDATE " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " SET " + Cat_Com_Unidades.Campo_Nombre + " = '" + Datos.P_Nombre + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Unidades.Campo_Abreviatura + " = '" + Datos.P_Abreviatura + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Unidades.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Unidades.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Unidades.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Unidades.Campo_Unidad_ID + " = '" + Datos.P_Unidad_ID + "'";

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
        /// NOMBRE DE LA CLASE:     Consulta_Unidades
        /// DESCRIPCION:            Realizar la consulta de las unidades por criterio de busqueda o por un ID
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a modificar
        /// CREO       :            Noe Mosqueda Valadez
        /// FECHA_CREO :            08/Noviembre/2010 11:00 
        /// MODIFICO          :     Jesus Toledo
        /// FECHA_MODIFICO    :     08/Febrero/2011 1:08
        /// CAUSA_MODIFICACION:     Se agrego la clausula UPPER para ignorar mayusculas y minusculas
        ///*******************************************************************************/
        public static DataTable Consulta_Unidades(Cls_Cat_Com_Unidades_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty;

            try
            {
                //Asignar consulta para los datos
                Mi_SQL = "SELECT " + Cat_Com_Unidades.Campo_Unidad_ID + "," + Cat_Com_Unidades.Campo_Nombre + "," + Cat_Com_Unidades.Campo_Abreviatura + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Unidades.Campo_Comentarios + " FROM " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " ";

                //Verificar si hay un ID
                if (Datos.P_Unidad_ID != "" && Datos.P_Unidad_ID != String.Empty && Datos.P_Unidad_ID != null)
                    Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Unidades.Campo_Unidad_ID + " = '" + Datos.P_Unidad_ID + "' ";
                else
                    if (Datos.P_Nombre != "" && Datos.P_Nombre != String.Empty && Datos.P_Nombre != null)
                    {
                        Mi_SQL = Mi_SQL + " WHERE ( upper(" + Cat_Com_Unidades.Campo_Nombre + ") LIKE upper('%" + Datos.P_Nombre + "%')";
                        Mi_SQL = Mi_SQL + " OR upper(" + Cat_Com_Unidades.Campo_Abreviatura + ") LIKE upper('%" + Datos.P_Nombre + "%' ))";
                    } 

                //Ordenar
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Unidades.Campo_Nombre;

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