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
using Presidencia.Catalogo_Compras_Proyectos_Programas.Negocio;
using Presidencia.Constantes;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;

/// <summary>
/// Summary description for Cls_Cat_Com_Proyectos_Programas_Datos
/// </summary>
namespace Presidencia.Catalogo_Compras_Proyectos_Programas.Datos
{
    public class Cls_Cat_Com_Proyectos_Programas_Datos
    {
        public Cls_Cat_Com_Proyectos_Programas_Datos()
        {
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Alta_Proyectos_Programas
        /// DESCRIPCION:            Dar de Alta un nuevo proyecto o programa a la base de datos
        /// PARAMETROS :            Datos: Variable de la capa de negocios
        /// CREO       :            Jesus Toledo Rodriguez
        /// FECHA_CREO :            01/Marzo/2011 11:15 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static void Alta_Proyectos_Programas(Cls_Cat_Com_Proyectos_Programas_Negocio Datos)
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
                Mi_SQL = "SELECT NVL(MAX(" + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + "), '0000000000') ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas;

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Aux = Obj_Comando.ExecuteScalar();

                //Verificar si es nulo
                if (Convert.IsDBNull(Aux) == true)
                    Datos.P_Proyecto_Programa_ID = "0000000001";
                else
                    Datos.P_Proyecto_Programa_ID = String.Format("{0:0000000000}", Convert.ToInt32(Aux) + 1);

                //Asignar consulta para la insercion
                Mi_SQL = "INSERT INTO " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + " (";
                Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Nombre + "," + Cat_Com_Proyectos_Programas.Campo_Estatus + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Descripcion + "," + Cat_Com_Proyectos_Programas.Campo_Clave + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Elemento_PEP + "," + Cat_Com_Proyectos_Programas.Campo_Usuario_Creo + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Fecha_Creo + ") VALUES('" + Datos.P_Proyecto_Programa_ID + "',";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Nombre + "','" + Datos.P_Estatus + "','" + Datos.P_Comentarios + "','" + Datos.P_Clave + "', ";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Elemento_Pep + "','" + Datos.P_Usuario + "',SYSDATE)" ;
                
                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                //Ejecutar transaccion
                Obj_Transaccion.Commit();
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
        /// NOMBRE DE LA CLASE:     Baja_Programas_Proyectos
        /// DESCRIPCION:            Eliminar un proyecto o programa existente de la base de datos
        /// PARAMETROS :            Datos: Variable de la capa de negocios
        /// CREO       :            Jesus Toledo Rodriguez
        /// FECHA_CREO :            01/Marzo/2011 11:20
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static void Baja_Programas_Proyectos(Cls_Cat_Com_Proyectos_Programas_Negocio Datos)
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
                Mi_SQL = "DELETE FROM " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Sap_Det_Prog_Partidas.Campo_Det_Proyecto_Programa_ID + " = '" + Datos.P_Proyecto_Programa_ID + "'";                

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                Mi_SQL = "DELETE FROM " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + " = '" + Datos.P_Proyecto_Programa_ID + "'";

                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();


                //Ejecutar transaccion
                Obj_Transaccion.Commit();
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
        /// NOMBRE DE LA CLASE:     Cambio_Programas_Proyectos
        /// DESCRIPCION:            Modificar un programa o proyecto existente de la base de datos
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a modificar
        /// CREO       :            Jesus Toledo Rodriguez
        /// FECHA_CREO :            05/Noviembre/2010 13:42 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static void Cambio_Programas_Proyectos(Cls_Cat_Com_Proyectos_Programas_Negocio Datos)
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
                Mi_SQL = "UPDATE " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas;
                Mi_SQL = Mi_SQL + " SET " + Cat_Com_Proyectos_Programas.Campo_Nombre + " = '" + Datos.P_Nombre + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Clave + " = '" + Datos.P_Clave + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Elemento_PEP + " = '" + Datos.P_Elemento_Pep + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Descripcion + " = '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + " = '" + Datos.P_Proyecto_Programa_ID + "'";

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                //Ejecutar transaccion
                Obj_Transaccion.Commit();
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
        /// NOMBRE DE LA CLASE:     Consulta_Programas_Proyectos
        /// DESCRIPCION:            Realizar la consulta de los programas o proyectos por criterio de busqueda o por un ID
        /// PARAMETROS :            Datos: Variable de la capa de negocios
        /// CREO       :            Jesus Toledo Rodriguez
        /// FECHA_CREO :            01/Marzo/2011 11:18
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :     
        /// CAUSA_MODIFICACION:             
        ///*******************************************************************************/
        public static DataTable Consulta_Programas_Proyectos(Cls_Cat_Com_Proyectos_Programas_Negocio Datos)
        {
            //Declaracion de Variables
            String Mi_SQL = String.Empty; //Variable para las consultas

            try
            {
                //Asignar consulta para el listado
                Mi_SQL = "SELECT " + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + "," + Cat_Com_Proyectos_Programas.Campo_Nombre + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Estatus + "," + Cat_Com_Proyectos_Programas.Campo_Descripcion + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Elemento_PEP + "," + Cat_Com_Proyectos_Programas.Campo_Clave + " ";                
                Mi_SQL = Mi_SQL + "FROM " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + " ";

                //Verificar si se tiene asignado un ID
                if (Datos.P_Proyecto_Programa_ID != "" && Datos.P_Proyecto_Programa_ID != String.Empty && Datos.P_Proyecto_Programa_ID != null)
                    Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + " = '" + Datos.P_Proyecto_Programa_ID + "' ";
                else
                    if (Datos.P_Clave != "" && Datos.P_Clave != String.Empty && Datos.P_Clave != null)
                    {
                        Mi_SQL = Mi_SQL + "WHERE upper(" + Cat_Com_Proyectos_Programas.Campo_Clave + ") LIKE upper('%" + Datos.P_Clave + "%') ";
                        Mi_SQL = Mi_SQL + "OR upper(" + Cat_Com_Proyectos_Programas.Campo_Nombre + ") LIKE upper('%" + Datos.P_Clave + "%') ";
                        Mi_SQL = Mi_SQL + "OR upper(" + Cat_Com_Proyectos_Programas.Campo_Descripcion + ") LIKE upper('%" + Datos.P_Clave + "%') ";
                    }

                //Ordenacion
                Mi_SQL = Mi_SQL + "ORDER BY " + Cat_Com_Proyectos_Programas.Campo_Nombre + " ";

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

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Partidas_Genericas
        /// DESCRIPCION:            Realizar la consulta de las patidas por un el ID del concepto
        /// PARAMETROS :            Concepto ID : ID del concepto asociado a la partida
        /// CREO       :            Jesus Toledo Rodriguez
        /// FECHA_CREO :            01/Marzo/2011 01:30
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :     
        /// CAUSA_MODIFICACION:             
        ///*******************************************************************************/
        public static DataTable Consulta_Partidas_Genericas(String Concepto_ID)
        {
            //Declaracion de Variables
            String Mi_SQL = String.Empty; //Variable para las consultas

            try
            {
                //Asignar consulta para el listado
                Mi_SQL = "SELECT * ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + " ";

                //Verificar si se tiene asignado un ID

                Mi_SQL = Mi_SQL + "WHERE " + Cat_SAP_Partida_Generica.Campo_Concepto_ID + " = '" + Concepto_ID + "' ";                

                //Ordenacion
                Mi_SQL = Mi_SQL + "ORDER BY " + Cat_SAP_Partida_Generica.Campo_Clave + " ";

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
        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Partidas_Especificas
        /// DESCRIPCION:            Realizar la consulta de las patidas por un el ID del concepto
        /// PARAMETROS :            Concepto ID : ID del concepto asociado a la partida
        /// CREO       :            Jesus Toledo Rodriguez
        /// FECHA_CREO :            01/Marzo/2011 01:30
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :     
        /// CAUSA_MODIFICACION:             
        ///*******************************************************************************/
        public static DataTable Consulta_Partidas_Especificas(String Partida_Generica_ID)
        {
            //Declaracion de Variables
            String Mi_SQL = String.Empty; //Variable para las consultas

            try
            {
                //Asignar consulta para el listado
                Mi_SQL = "SELECT * ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " ";

                //Verificar si se tiene asignado un ID

                Mi_SQL = Mi_SQL + "WHERE " + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID + " = '" + Partida_Generica_ID + "' ";

                //Ordenacion
                Mi_SQL = Mi_SQL + "ORDER BY " + Cat_Sap_Partidas_Especificas.Campo_Nombre + " ";

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
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Conceptos
        ///DESCRIPCIÓN: Realizar una consulta de uno o mas registros de la tabla de CAT_SAP_CONCEPTO
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 02/25/2011 06:59:32 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        internal static DataTable Consulta_Conceptos(string ID)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty;

            try
            {
                //Asignar consulta para los Subfamilias
                Mi_SQL = "SELECT " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Concepto_ID + ",";
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Capitulo_ID + ",";
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Clave + ",";
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Estatus + ",";
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Descripcion + " ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Capitulo_ID + " = '" + ID + "' ";
                
                
                //Ordenar
                Mi_SQL = Mi_SQL + "ORDER BY " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Clave;

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
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Partidas
        ///DESCRIPCIÓN: Realizar una consulta de uno o mas registros de la tabla de partidas especidicas
        ///PARAMETROS: Datos: Entidad de la clase negocio que contiene los datos a consultar
        ///CREO: jtoledo
        ///FECHA_CREO: 02/25/2011 06:59:32 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Partidas(Cls_Cat_Com_Proyectos_Programas_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty;

            try
            {
                //Asignar consulta para los Subfamilias
                Mi_SQL = "SELECT " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + "." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Partida_ID + ", " ;
                Mi_SQL = Mi_SQL + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + "." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Prog_Partidas_ID + ", " ;
                Mi_SQL = Mi_SQL + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + "." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Proyecto_Programa_ID + ", " ;
                Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Nombre + " AS PARTIDA_NOMBRE, ";
                Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Clave + " AS PARTIDA_CLAVE ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + ", " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas;

                Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + "." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Partida_ID + " ";
                Mi_SQL = Mi_SQL + " AND " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + "." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Proyecto_Programa_ID + " = '" + Datos.P_Proyecto_Programa_ID + "' ";


                //Ordenar
                Mi_SQL = Mi_SQL + "ORDER BY " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + "." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Partida_ID;

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
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Capitulos
        ///DESCRIPCIÓN: Ejecuta la instruccion para consultar los capitulos existentes en la base de datos
        ///PARAMETROS: Datos: Variable de negocio que contiene los datos a consultar
        ///CREO: jtoledo
        ///FECHA_CREO: 03/03/2011 12:50:08 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************     
        public static DataTable Consulta_Capitulos(Cls_Cat_Com_Proyectos_Programas_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty;

            try
            {
                //Asignar consulta para los capitulos
                Mi_SQL = "SELECT " + Cat_SAP_Capitulos.Campo_Capitulo_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Clave + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Descripcion;
                Mi_SQL = Mi_SQL + " FROM " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos;
                //Filtrar por ID si se proporcionó
                
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_SAP_Capitulos.Campo_Clave;

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
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Programas_Especial
        ///DESCRIPCIÓN: Consulta los programas
        ///PARAMETROS: Datos: Variable de negocio que contiene los datos a consultar
        ///CREO: Salvador L. Rea Ayala
        ///FECHA_CREO: 4/Octubre/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************     
        public static DataTable Consulta_Programas_Especial(Cls_Cat_Com_Proyectos_Programas_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty;

            try
            {
                Mi_SQL = "SELECT " + Cat_Com_Proyectos_Programas.Campo_Clave + " ||' - '|| " + Cat_Com_Proyectos_Programas.Campo_Nombre + " AS CLAVE_NOMBRE, ";
                Mi_SQL += Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID;
                Mi_SQL += " FROM " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas;
                Mi_SQL += " ORDER BY " + Cat_Com_Proyectos_Programas.Campo_Clave + " ASC";

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