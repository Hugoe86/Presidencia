using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Catalogo_SAP_Conceptos.Negocio;
using System.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;



namespace Presidencia.Catalogo_SAP_Conceptos.Datos
{
    public class Cls_Cat_SAP_Conceptos_Datos
    {
	    public Cls_Cat_SAP_Conceptos_Datos()
	    {		    
	    }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Alta_Concepto_Sap
        ///DESCRIPCIÓN: Dar de alta un registro en la tabla de CAT_SAP_CONCEPTO
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 02/25/2011 06:57:03 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public static void Alta_Concepto_SAP(Cls_Cat_SAP_Conceptos_Negocio Datos)
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
                Mi_SQL = "SELECT NVL(MAX(" + Cat_Sap_Concepto.Campo_Concepto_ID + "), '00000') FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto;

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Aux = Obj_Comando.ExecuteScalar();

                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false)
                    Datos.P_Concepto_ID= String.Format("{0:00000}", Convert.ToInt32(Aux) + 1);
                else
                    Datos.P_Concepto_ID = "00001";

                //Asignar consulta para la insercion
                Mi_SQL = "INSERT INTO " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + " (" + Cat_Sap_Concepto.Campo_Concepto_ID + ",";
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Campo_Capitulo_ID + "," + Cat_Sap_Concepto.Campo_Clave + ",";
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Campo_Estatus + "," + Cat_Sap_Concepto.Campo_Descripcion + ",";
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Campo_Usuario_Creo + "," + Cat_Sap_Concepto.Campo_Fecha_Creo + ") ";
                Mi_SQL = Mi_SQL + "VALUES('" + Datos.P_Concepto_ID + "','" + Datos.P_Capitulo_ID + "','" + Datos.P_Clave + "',";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Estatus + "','" + Datos.P_Descripcion + "',";
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
        ///NOMBRE DE LA FUNCIÓN: Baja_Concepto_Sap
        ///DESCRIPCIÓN: Dar de baja un registro en la tabla de CAT_SAP_CONCEPTO
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 02/25/2011 06:57:37 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static void Baja_Concepto_Sap(Cls_Cat_SAP_Conceptos_Negocio Datos)
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
                Mi_SQL = "DELETE FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Sap_Concepto.Campo_Concepto_ID + " = '" + Datos.P_Concepto_ID + "'";

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
        ///NOMBRE DE LA FUNCIÓN: Cambio_Concepto_Sap
        ///DESCRIPCIÓN: Modificar un registro en la tabla de CAT_SAP_CONCEPTO
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 02/25/2011 06:58:25 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static void Cambio_Concepto_Sap(Cls_Cat_SAP_Conceptos_Negocio Datos)
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
                Mi_SQL = "UPDATE " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + " SET " + Cat_Sap_Concepto.Campo_Clave + " = '" + Datos.P_Clave + "',";
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Campo_Capitulo_ID + " = '" + Datos.P_Capitulo_ID + "', ";                
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Campo_Descripcion + " = '" + Datos.P_Descripcion + "', ";
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario + "', ";
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Sap_Concepto.Campo_Concepto_ID + " = '" + Datos.P_Concepto_ID + "'";

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
        ///NOMBRE DE LA FUNCIÓN: Consulta_Concepto_Sap
        ///DESCRIPCIÓN: Realizar una consulta de uno o mas registros de la tabla de CAT_SAP_CONCEPTO
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 02/25/2011 06:59:32 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Concepto_Sap(Cls_Cat_SAP_Conceptos_Negocio Datos)
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
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Descripcion + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Clave + " AS CAPITULO_CLAVE";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + ", " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Capitulo_ID + " = " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Capitulo_ID + " ";

                
                if (Datos.P_Concepto_ID != "" && Datos.P_Concepto_ID != String.Empty && Datos.P_Concepto_ID != null)
                {
                    Mi_SQL = Mi_SQL + "AND " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Concepto_ID + " = '" + Datos.P_Concepto_ID + "' ";
                }
                else if(Datos.P_Clave != "" && Datos.P_Clave != String.Empty && Datos.P_Clave != null)
                {
                    Mi_SQL = Mi_SQL + "AND (" + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Clave + " LIKE '%" + Datos.P_Clave + "%' ";
                    Mi_SQL = Mi_SQL + "OR " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Clave + " LIKE '%" + Datos.P_Clave + "%' ";
                    Mi_SQL = Mi_SQL + "OR upper( " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_SAP_Capitulos.Campo_Descripcion + ") LIKE upper('%" + Datos.P_Clave + "%') )";
                }                    

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
        ///NOMBRE DE LA FUNCIÓN: Consulta_Capitulos
        ///DESCRIPCIÓN: Realizar una consulta de uno o mas registros de la tabla de capitulos para llenar el combo de capitulos
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 02/25/2011 07:01:32 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Capitulos()
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty;

            try
            {
                //Asignar consulta para los Subfamilias
                Mi_SQL = "SELECT " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Capitulo_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Clave;
                Mi_SQL = Mi_SQL + " FROM " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos;
                
                //Ordenar
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Clave;

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