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
using Presidencia.Catalogo_Compras_Parametros.Negocio;
using Presidencia.Constantes;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;

/// <summary>
/// Summary description for Cls_Cat_Com_Parametros_Datos
/// </summary>
namespace Presidencia.Catalogo_Compras_Parametros.Datos
{
    public class Cls_Cat_Com_Parametros_Datos
    {
        public Cls_Cat_Com_Parametros_Datos()
        {
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Alta_Parametros
        /// DESCRIPCION:            Dar de Alta un nuevo Parametro a la base de datos
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a ingresar
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            07/Enero/2011 12:27 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static void Alta_Parametros(Cls_Cat_Com_Parametros_Negocio Datos)
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
                Mi_SQL = "SELECT NVL(MAX(" + Cat_Com_Parametros.Campo_Parametro_ID + "), '00000') FROM " + Cat_Com_Parametros.Tabla_Cat_Com_Parametros;

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Aux = Obj_Comando.ExecuteScalar();

                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false)
                    Datos.P_Parametro_ID = String.Format("{0:00000}", Convert.ToInt32(Aux) + 1);
                else
                    Datos.P_Parametro_ID = "00001";

                //Asignar consulta para la insercion
                Mi_SQL = "INSERT INTO " + Cat_Com_Parametros.Tabla_Cat_Com_Parametros + " (" + Cat_Com_Parametros.Campo_Parametro_ID + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Parametros.Campo_Cantidad_Sal_Min_Resguardo + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Parametros.Campo_Plazo_Surtir_Orden_Compra + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Parametros.Campo_Usuario_Creo + "," + Cat_Com_Parametros.Campo_Fecha_Creo + ") ";
                Mi_SQL = Mi_SQL + "VALUES('" + Datos.P_Parametro_ID + "','" + Datos.P_Salario_Minimo_Resguardado + "','";
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
        /// NOMBRE DE LA CLASE:     Cambio_Parametros
        /// DESCRIPCION:            Modificar un Parametro existente de la base de datos
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a modificar
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            07/Enero/2011 12:32 
        /// MODIFICO          :     Roberto González
        /// FECHA_MODIFICO    :     07/Febrero/2011
        /// CAUSA_MODIFICACION:     Cambio a parámetro estático para el campo ID en la consulta para que sólo 
        ///                         modifique el primer registro
        ///*******************************************************************************/
        public static void Cambio_Parametros(Cls_Cat_Com_Parametros_Negocio Datos)
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
                Mi_SQL = "UPDATE " + Cat_Com_Parametros.Tabla_Cat_Com_Parametros + " SET ";
                Mi_SQL = Mi_SQL + Cat_Com_Parametros.Campo_Cantidad_Sal_Min_Resguardo + " = '" + Datos.P_Salario_Minimo_Resguardado + "',";
                Mi_SQL = Mi_SQL + Cat_Com_Parametros.Campo_Plazo_Surtir_Orden_Compra + " = '" + Datos.P_Plazo_Surtir_Orden_Compra + "',";
                Mi_SQL = Mi_SQL + Cat_Com_Parametros.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Parametros.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Parametros.Campo_Parametro_ID + " = '00001'";

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
        /// NOMBRE DE LA CLASE:     Consultar_Generico
        /// DESCRIPCION:            Realizar la consulta del listado de Partida Generica
        /// PARAMETROS :            
        /// CREO       :            Jacqueline Ramírez Sierra
        /// FECHA_CREO :            12/Marzo/2011 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consultar_Generico(Cls_Cat_Com_Parametros_Negocio Parametros)
        {
            String Mi_SQL = "SELECT " + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID +
                            ", " + Cat_SAP_Partida_Generica.Campo_Descripcion +
                            " FROM " + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica;
            //if (Parametros. != null)
            //{
            //    Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Giros.Campo_Giro_ID +
            //             " ='" + Cotizadores.P_Giro_ID + "'";
            //}


            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consultar_Especifico
        /// DESCRIPCION:            Realizar la consulta del listado de Partida Especifica
        /// PARAMETROS :            
        /// CREO       :            Jacqueline Ramírez Sierra
        /// FECHA_CREO :            12/Marzo/2011 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consultar_Especificas(Cls_Cat_Com_Parametros_Negocio Parametros)
        {
            String Mi_SQL = "SELECT " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID +
                            ", " + Cat_Sap_Partidas_Especificas.Campo_Descripcion +
                            " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas +
                            " WHERE " + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID + "='" + Parametros.P_Partida_Generica_ID + "'";
            //if (Parametros. != null)
            //{
            //    Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Giros.Campo_Giro_ID +
            //             " ='" + Cotizadores.P_Giro_ID + "'";
            //}


            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Parametros
        /// DESCRIPCION:            Realizar la consulta de los Parametros por criterio de busqueda o por un ID
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a modificar
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            07/Enero/2011 12:34
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consultar_Combo(Cls_Cat_Com_Parametros_Negocio Parametros)
        {
            String Mi_SQL = "SELECT " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID +
                            //", " + Cat_Sap_Partidas_Especificas.Campo_Descripcion +
                            " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + 
                            " WHERE " + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID + "='" + Parametros.P_Partida_Generica_ID + "'" ;
 
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }
        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Parametros
        /// DESCRIPCION:            Realizar la consulta de los Parametros por criterio de busqueda o por un ID
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a modificar
        /// CREO       :            José Antonio López Hernández
        /// FECHA_CREO :            07/Enero/2011 12:34
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consulta_Parametros(Cls_Cat_Com_Parametros_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty;

            try
            {
                //Asignar consulta para los Parametros
                Mi_SQL = "SELECT " + Cat_Com_Parametros.Campo_Parametro_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Parametros.Campo_Cantidad_Sal_Min_Resguardo + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Parametros.Campo_Plazo_Surtir_Orden_Compra;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Parametros.Tabla_Cat_Com_Parametros;
                //Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Parametros.Campo_Parametro_ID + " = '00001'";

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