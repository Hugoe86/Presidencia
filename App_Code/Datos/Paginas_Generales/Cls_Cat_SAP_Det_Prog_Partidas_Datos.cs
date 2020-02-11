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
using Presidencia.Constantes;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Catalogo_SAP_Det_Prog_Partidas.Negocio;

namespace Presidencia.Catalogo_SAP_Det_Prog_Partidas.Datos
{

    public class Cls_Cat_SAP_Det_Prog_Partidas_Datos
    {
        public Cls_Cat_SAP_Det_Prog_Partidas_Datos()
        {            
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Alta_Partida
        ///DESCRIPCIÓN: Asignar una partida a un proyecto
        ///PARAMETROS: Datos: Entidad de la clase negocio que contiene los datos a consultar
        ///CREO: jtoledo
        ///FECHA_CREO: 02/25/2011 06:59:32 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        internal static void Alta_Partida(Cls_Cat_SAP_Det_Prog_Partidas_Negocio Datos)
        {
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = String.Empty;
            Object Aux; //Variable auxiliar para las consultas
            Object Aux2; //Variable auxiliar para las consultas
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
                Mi_SQL = "SELECT NVL(MAX(" + Cat_Sap_Det_Prog_Partidas.Campo_Det_Prog_Partidas_ID + "), '00000') ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas;

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Aux = Obj_Comando.ExecuteScalar();

                //Verificar si es nulo
                if (Convert.IsDBNull(Aux) == true)
                    Datos.P_Det_Prog_Partidas_ID = "00001";
                else
                    Datos.P_Det_Prog_Partidas_ID = String.Format("{0:00000}", Convert.ToInt32(Aux) + 1);

                Mi_SQL = "SELECT Count(*) FROM " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Sap_Det_Prog_Partidas.Campo_Det_Partida_ID + "= '" + Datos.P_Det_Partida_ID;
                Mi_SQL = Mi_SQL + "' AND " + Cat_Sap_Det_Prog_Partidas.Campo_Det_Proyecto_Programa_ID + " = '" + Datos.P_Det_Proyecto_Programa_ID +"'";
                Obj_Comando.CommandText = Mi_SQL;
                Aux2 = Obj_Comando.ExecuteScalar();

                if (Convert.ToInt32(Aux2) <= 0)
                {

                    //Asignar consulta para la insercion
                    Mi_SQL = "INSERT INTO " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + " (";
                    Mi_SQL = Mi_SQL + Cat_Sap_Det_Prog_Partidas.Campo_Det_Prog_Partidas_ID + ",";
                    Mi_SQL = Mi_SQL + Cat_Sap_Det_Prog_Partidas.Campo_Det_Proyecto_Programa_ID + "," + Cat_Sap_Det_Prog_Partidas.Campo_Det_Partida_ID;
                    Mi_SQL = Mi_SQL + ") VALUES('" + Datos.P_Det_Prog_Partidas_ID + "',";
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Det_Proyecto_Programa_ID + "','" + Datos.P_Det_Partida_ID + "')";

                    //Ejecutar consulta
                    Obj_Comando.CommandText = Mi_SQL;
                    Obj_Comando.ExecuteNonQuery();

                }
                else 
                {
                    throw new Exception("Esta partida ya fue asignada");
                }

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
        ///NOMBRE DE LA FUNCIÓN: Baja_Partida
        ///DESCRIPCIÓN: Eliminar una partida asignada a un proyecto
        ///PARAMETROS: Datos: Entidad de la clase negocio que contiene los datos a consultar
        ///CREO: jtoledo
        ///FECHA_CREO: 02/25/2011 06:59:32 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        internal static void Baja_Partida(Cls_Cat_SAP_Det_Prog_Partidas_Negocio Datos)
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
                Mi_SQL = "DELETE FROM " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas+ " ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Sap_Det_Prog_Partidas.Campo_Det_Prog_Partidas_ID + " = '" + Datos.P_Det_Prog_Partidas_ID + "'";

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
        ///NOMBRE DE LA FUNCIÓN: Consulta_Partidas
        ///DESCRIPCIÓN: Realizar una consulta de uno o mas registros de la tabla de partidas especidicas
        ///PARAMETROS: Datos: Entidad de la clase negocio que contiene los datos a consultar
        ///CREO: jtoledo
        ///FECHA_CREO: 02/25/2011 06:59:32 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Partidas(Cls_Cat_SAP_Det_Prog_Partidas_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty;

            try
            {
                //Asignar consulta para los Subfamilias
                Mi_SQL = "SELECT " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + "." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Partida_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + "." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Prog_Partidas_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + "." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Proyecto_Programa_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Nombre + " AS PARTIDA_NOMBRE ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + ", " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas;

                Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID;
                Mi_SQL = Mi_SQL + " = '" + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + "." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Partida_ID + "' ";
                Mi_SQL = Mi_SQL + " AND " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + "." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Proyecto_Programa_ID + " = '" + Datos.P_Det_Proyecto_Programa_ID + "' ";


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
    }
}