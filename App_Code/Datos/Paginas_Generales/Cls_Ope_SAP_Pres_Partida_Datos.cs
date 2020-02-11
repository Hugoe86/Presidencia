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
using Presidencia.Operacion_SAP_Pres_Partidas.Negocio;

namespace Presidencia.Operacion_SAP_Pres_Partidas.Datos
{
    public class Cls_Ope_SAP_Pres_Partidas_Datos
    {
        public Cls_Ope_SAP_Pres_Partidas_Datos()
        {
        }        
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Alta_Asignacion_Presupuesto
        ///DESCRIPCIÓN: Dar de alta un registro en la tabla de CAT_SAP_CONCEPTO
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 02/25/2011 06:57:03 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public static void Alta_Asignacion_Presupuesto(Cls_Ope_SAP_Pres_Partidas_Negocio Datos)
        {
            //Declaracion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = String.Empty;
            Object Aux; //Variable auxiliar para las consultas
            Object Aux2; //Variable auxiliar para las consultas
            Object Monto_Presupuestal_Programa; //Variable auxiliar para las consultas
            String Mensaje = String.Empty; //Variable para el mensaje de error

            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;

                //Asignar cadena para la consulta del registro en el año que se dara de alta
                Mi_SQL = "";
                Mi_SQL = "SELECT COUNT(*) FROM " + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + " "; 
                Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Pres_Partida.Campo_Proyecto_Programa_ID + " = '" + Datos.P_Proyecto_Programa_ID + "' ";
                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Pres_Partida.Campo_Partida_ID + " = '" + Datos.P_Partida_Especifica_ID +"' " ;
                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Pres_Partida.Campo_Anio_Presupuesto + " = '" + Datos.P_Anio_Presupuesto + "' ";

                Obj_Comando.CommandText = Mi_SQL;
                Aux2 = Obj_Comando.ExecuteScalar();

                if (Convert.ToInt32(Aux2) == 0 )
                {
                    //Consulta para obtener el presupuesto del proyecto
                    Mi_SQL = "";
                    Mi_SQL = "SELECT " + Ope_SAP_Pres_Prog_Proy.Campo_Monto_Disponible + " FROM " + Ope_SAP_Pres_Prog_Proy.Tabla_Ope_SAP_Pres_Prog_Proy + " ";
                    Mi_SQL = Mi_SQL + "WHERE " + Ope_SAP_Pres_Prog_Proy.Campo_Proyecto_Programa_ID + " = '" + Datos.P_Proyecto_Programa_ID + "' ";
                    
                    Obj_Comando.CommandText = Mi_SQL;
                    Monto_Presupuestal_Programa  = Obj_Comando.ExecuteScalar();
                    if ( Monto_Presupuestal_Programa != null )
                    {
                        if (Convert.ToDouble(Datos.P_Monto_Presupuestal) <= Convert.ToDouble(Monto_Presupuestal_Programa))
                        {
                            Mi_SQL = "";
                            //Consultas para el ID
                            Mi_SQL = "SELECT NVL(MAX(" + Ope_Com_Pres_Partida.Campo_Presupuesto_Partida_ID + "), '0000000000') FROM " + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida;

                            //Ejecutar consulta
                            Obj_Comando.CommandText = Mi_SQL;
                            Aux = Obj_Comando.ExecuteScalar();

                            //Verificar si no es nulo
                            if (Convert.IsDBNull(Aux) == false)
                                Datos.P_Pres_Partida_ID = String.Format("{0:0000000000}", Convert.ToInt32(Aux) + 1);
                            else
                                Datos.P_Pres_Partida_ID = "0000000001";

                            //Asignar consulta para la insercion
                            Mi_SQL = "";
                            Mi_SQL = "INSERT INTO " + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + " ( ";
                            Mi_SQL = Mi_SQL + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + "." + Ope_Com_Pres_Partida.Campo_Presupuesto_Partida_ID + ", ";
                            Mi_SQL = Mi_SQL + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + "." + Ope_Com_Pres_Partida.Campo_Partida_ID + ", ";
                            Mi_SQL = Mi_SQL + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + "." + Ope_Com_Pres_Partida.Campo_Anio_Presupuesto + ", ";
                            Mi_SQL = Mi_SQL + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + "." + Ope_Com_Pres_Partida.Campo_Monto_Presupuestal + ", ";
                            Mi_SQL = Mi_SQL + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + "." + Ope_Com_Pres_Partida.Campo_Monto_Disponible + ", ";
                            Mi_SQL = Mi_SQL + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + "." + Ope_Com_Pres_Partida.Campo_Monto_Comprometido + ", ";
                            Mi_SQL = Mi_SQL + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + "." + Ope_Com_Pres_Partida.Campo_Monto_Ejercido + ", ";
                            Mi_SQL = Mi_SQL + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + "." + Ope_Com_Pres_Partida.Campo_Proyecto_Programa_ID + ", ";
                            Mi_SQL = Mi_SQL + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + "." + Ope_Com_Pres_Partida.Campo_Usuario_Creo + ") ";

                            Mi_SQL = Mi_SQL + "VALUES( '" + Datos.P_Pres_Partida_ID + "', '";
                            Mi_SQL = Mi_SQL + Datos.P_Partida_Especifica_ID + "', '";
                            Mi_SQL = Mi_SQL + Datos.P_Anio_Presupuesto + "', '";
                            Mi_SQL = Mi_SQL + Datos.P_Monto_Presupuestal + "', '";
                            Mi_SQL = Mi_SQL + Datos.P_Monto_Disponible + "', '";
                            Mi_SQL = Mi_SQL + Datos.P_Monto_Comprometido + "', '";
                            Mi_SQL = Mi_SQL + Datos.P_Monto_Ejercido + "', '";
                            Mi_SQL = Mi_SQL + Datos.P_Proyecto_Programa_ID + "', '";
                            Mi_SQL = Mi_SQL + Datos.P_Usuario + "') ";

                            //Ejecutar consulta
                            Obj_Comando.CommandText = Mi_SQL;
                            Obj_Comando.ExecuteNonQuery();

                            //Consulta para restar el presupuesto asignado a la partida del presupuesto disponible en el proyecto
                            //y sumar a presupuesto comprometido
                            Mi_SQL = "";
                            Mi_SQL = "UPDATE " + Ope_SAP_Pres_Prog_Proy.Tabla_Ope_SAP_Pres_Prog_Proy + " SET ";
                            Mi_SQL = Mi_SQL + Ope_SAP_Pres_Prog_Proy.Campo_Monto_Disponible + " = " + Ope_SAP_Pres_Prog_Proy.Campo_Monto_Disponible + " - ";
                            Mi_SQL = Mi_SQL + Datos.P_Monto_Presupuestal + ", ";
                            Mi_SQL = Mi_SQL + Ope_SAP_Pres_Prog_Proy.Campo_Monto_Comprometido + " = " + Ope_SAP_Pres_Prog_Proy.Campo_Monto_Comprometido + " + ";
                            Mi_SQL = Mi_SQL + Datos.P_Monto_Presupuestal +" ";
                            Mi_SQL = Mi_SQL + Ope_SAP_Pres_Prog_Proy.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario + "' ,";
                            Mi_SQL = Mi_SQL + Ope_SAP_Pres_Prog_Proy.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Pres_Partida.Campo_Proyecto_Programa_ID + " = '" + Datos.P_Proyecto_Programa_ID + "' ";

                            //Ejecutar consulta
                            Obj_Comando.CommandText = Mi_SQL;
                            Obj_Comando.ExecuteNonQuery();

                        }
                        else
                        {
                            throw new Exception("El monto presupuestal Asignado a la partida es mayor que el Monto Presupuestal de Proyecto o Programa </br> Monto Disponible del Proyecto: $" + Monto_Presupuestal_Programa.ToString() + "</br> Monto Presupuestal de la Partida: $" + Datos.P_Monto_Presupuestal);
                        }
                    }
                    else
                    {
                        throw new Exception("El Proyecto no tiene Monto Presupuestal asignado");
                    }
                }
                else 
                {
                    throw new Exception("La Partida ya esta registrada en el año que se indicó");
                }

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
        ///NOMBRE DE LA FUNCIÓN: Cambio_Asignacion_Presupuesto
        ///DESCRIPCIÓN: Modificar un registro en la tabla de CAT_SAP_CONCEPTO
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 02/25/2011 06:58:25 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static void Cambio_Asignacion_Presupuesto(Cls_Ope_SAP_Pres_Partidas_Negocio Datos)
        {
            //Declaracion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = String.Empty;
            String Mensaje = String.Empty; //Variable para el mensaje de error
            Object Monto_Presupuestal_Programa; //Variable auxiliar para las consultas
            Double Presupuesto;

            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;

                //Consulta para obtener el presupuesto del proyecto
                    Mi_SQL = "";
                    Mi_SQL = "SELECT " + Ope_SAP_Pres_Prog_Proy.Campo_Monto_Disponible + " FROM " + Ope_SAP_Pres_Prog_Proy.Tabla_Ope_SAP_Pres_Prog_Proy + " ";
                    Mi_SQL = Mi_SQL + "WHERE " + Ope_SAP_Pres_Prog_Proy.Campo_Proyecto_Programa_ID + " = '" + Datos.P_Proyecto_Programa_ID + "' ";
                    
                    Obj_Comando.CommandText = Mi_SQL;
                    Monto_Presupuestal_Programa  = Obj_Comando.ExecuteScalar();
                    if (Monto_Presupuestal_Programa != null)
                    {
                        Presupuesto = Convert.ToDouble(Datos.P_Monto_Presupuestal);
                        if (Presupuesto < 0)                        
                            Presupuesto = Convert.ToDouble(Datos.P_Monto_Presupuestal) * -1;                        

                        if (Presupuesto <= Convert.ToDouble(Monto_Presupuestal_Programa))
                        {
                            Mi_SQL = "";
                            Mi_SQL = "UPDATE " + Ope_SAP_Pres_Prog_Proy.Tabla_Ope_SAP_Pres_Prog_Proy + " SET " + Ope_SAP_Pres_Prog_Proy.Campo_Monto_Disponible + " = " + Ope_SAP_Pres_Prog_Proy.Campo_Monto_Disponible + " + ";
                            Mi_SQL = Mi_SQL + Datos.P_Monto_Presupuestal + "," + Ope_SAP_Pres_Prog_Proy.Campo_Monto_Comprometido + " = " + Ope_SAP_Pres_Prog_Proy.Campo_Monto_Comprometido + " - ";
                            Mi_SQL = Mi_SQL + Datos.P_Monto_Presupuestal + ", "+ Ope_SAP_Pres_Prog_Proy.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario + "' ,";
                            Mi_SQL = Mi_SQL + Ope_SAP_Pres_Prog_Proy.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_SAP_Pres_Prog_Proy.Campo_Proyecto_Programa_ID + " = '" + Datos.P_Proyecto_Programa_ID + "'";

                            //Ejecutar consulta
                            Obj_Comando.CommandText = Mi_SQL;
                            Obj_Comando.ExecuteNonQuery();

                            //Asignar consulta para la modificacion
                            Mi_SQL = "";
                            Mi_SQL = "UPDATE " + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + " SET " + Ope_Com_Pres_Partida.Campo_Monto_Presupuestal + " = " + Ope_Com_Pres_Partida.Campo_Monto_Presupuestal + " - ";
                            Mi_SQL = Mi_SQL + Datos.P_Monto_Presupuestal + ", " + Ope_Com_Pres_Partida.Campo_Monto_Disponible + " = " + Ope_Com_Pres_Partida.Campo_Monto_Disponible + " - " + Datos.P_Monto_Presupuestal + ", ";
                            Mi_SQL = Mi_SQL + Ope_SAP_Pres_Prog_Proy.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario + "' ,";
                            Mi_SQL = Mi_SQL + Ope_SAP_Pres_Prog_Proy.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Pres_Partida.Campo_Presupuesto_Partida_ID + " = '" + Datos.P_Pres_Partida_ID + "'";

                            //Ejecutar consulta
                            Obj_Comando.CommandText = Mi_SQL;
                            Obj_Comando.ExecuteNonQuery();                            
                        }
                        else
                        {
                            throw new Exception("El monto presupuestal Asignado a la partida es mayor que el Monto Presupuestal de Proyecto o Programa </br> Monto Disponible del Proyecto: $" + Monto_Presupuestal_Programa.ToString());
                        }

                    }
                    else
                    {
                        throw new Exception("El Proyecto no tiene Monto Presupuestal asignado");
                    }                

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
        ///NOMBRE DE LA FUNCIÓN: Consulta_Asignacion_Presupuesto
        ///DESCRIPCIÓN: Realizar una consulta de uno o mas registros de la tabla de Ope_Sap_Pres_Prog_Proy
        ///             esta consulta muestra el presupuesto asignado a las partidas especificas 
        ///             pertencientes a cada uno de los programas
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 02/25/2011 06:59:32 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Asignacion_Presupuesto(Cls_Ope_SAP_Pres_Partidas_Negocio Datos)
        {
            String Mi_SQL = String.Empty; //Variable para las consultas
            double Valor_Numerico_Salida; //Variable para distinguir si el fitro es por año o por nombre de proyecto

            try
            {
                //Asignar consulta para el listado
                Mi_SQL = "SELECT " + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + "." + Ope_Com_Pres_Partida.Campo_Presupuesto_Partida_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + "." + Ope_Com_Pres_Partida.Campo_Partida_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + "." + Ope_Com_Pres_Partida.Campo_Proyecto_Programa_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + "." + Ope_Com_Pres_Partida.Campo_Monto_Presupuestal + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + "." + Ope_Com_Pres_Partida.Campo_Monto_Disponible + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + "." + Ope_Com_Pres_Partida.Campo_Monto_Comprometido + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + "." + Ope_Com_Pres_Partida.Campo_Anio_Presupuesto + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + "." + Ope_Com_Pres_Partida.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + "." + Ope_Com_Pres_Partida.Campo_Monto_Ejercido + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + "." + Cat_Com_Proyectos_Programas.Campo_Nombre +" AS PROYECTO_NOMBRE, ";
                Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Nombre + " AS PARTIDA_NOMBRE ";

                Mi_SQL = Mi_SQL + "FROM " + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + ", " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + ", ";
                Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " ";

                Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + "." + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID +" = ";
                Mi_SQL = Mi_SQL + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + "." + Ope_Com_Pres_Partida.Campo_Proyecto_Programa_ID + " ";
                Mi_SQL = Mi_SQL + "AND " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " = ";
                Mi_SQL = Mi_SQL + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + "." + Ope_Com_Pres_Partida.Campo_Partida_ID + " ";
                                
                //Filtro por Año
                if (Datos.P_Anio_Presupuesto != null && Datos.P_Anio_Presupuesto != "" && Datos.P_Anio_Presupuesto != String.Empty)
                {

                    if ( Double.TryParse(Convert.ToString(Datos.P_Anio_Presupuesto), System.Globalization.NumberStyles.Any,System.Globalization.NumberFormatInfo.InvariantInfo, out Valor_Numerico_Salida ) )
                    {
                        Mi_SQL = Mi_SQL + "AND " + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + "." + Ope_Com_Pres_Partida.Campo_Anio_Presupuesto + " = '";
                        Mi_SQL = Mi_SQL + Datos.P_Anio_Presupuesto + "' ";
                    }
                    else
                    {
                    Mi_SQL = Mi_SQL + "AND upper(" + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + "." + Cat_Com_Proyectos_Programas.Campo_Nombre + ") LIKE upper('%";
                    Mi_SQL = Mi_SQL + Datos.P_Anio_Presupuesto + "%') ";
                    }
                }
                //Ordenacion
                Mi_SQL = Mi_SQL + "ORDER BY " + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + "." + Ope_Com_Pres_Partida.Campo_Anio_Presupuesto;

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
        ///NOMBRE DE LA FUNCIÓN: Consulta_Partidas_Proyectos
        ///DESCRIPCIÓN: Ejecuta la consulta que nos arroja las partidas 
        ///             que corresponden a un proyecto en especifico 
        ///             para llenar el combo de partidas del formulario 
        ///             de asignacion de presupuesto
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/Marzo/2011 06:02:32 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        internal static DataTable Consulta_Partidas_Proyectos(Cls_Ope_SAP_Pres_Partidas_Negocio Datos)
        {
            //Declaracion de Variables
            String Mi_SQL = String.Empty; //Variable para las consultas

            try
            {
                //Asignar consulta para el listado
                Mi_SQL = "SELECT " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas +"."+ Cat_Sap_Det_Prog_Partidas.Campo_Det_Partida_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas +"."+ Cat_Sap_Det_Prog_Partidas.Campo_Det_Prog_Partidas_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + "." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Proyecto_Programa_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Nombre + " AS NOMBRE ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + " ," + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas +" ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " = ";
                Mi_SQL = Mi_SQL + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + "." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Partida_ID +" ";
                Mi_SQL = Mi_SQL + "AND " + Ope_SAP_Pres_Prog_Proy.Campo_Proyecto_Programa_ID + " = '" + Datos.P_Proyecto_Programa_ID +"' ";
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