using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data;
using System.Data.OracleClient;
using Presidencia.Catalogo_Compras_Servicios.Negocio;

namespace Presidencia.Catalogo_Compras_Servicios.Datos
{
    public class Cls_Cat_Com_Servicios_Datos
    {

          //SELECT CAT_COM_SERVICIOS.CLAVE, CAT_COM_SERVICIOS.NOMBRE, CAT_COM_SERVICIOS.COSTO, 
          //(SELECT NOMBRE FROM CAT_COM_IMPUESTOS WHERE  IMPUESTO_ID = CAT_COM_SERVICIOS.IMPUESTO_ID) AS IMPUESTO,
          //(SELECT CLAVE || '-' || NOMBRE  FROM CAT_SAP_PARTIDAS_ESPECIFICAS WHERE PARTIDA_ID = CAT_COM_SERVICIOS.PARTIDA_ID) AS PARTIDA
          //FROM CAT_COM_SERVICIOS;
        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Datos_Producto_Para_Excel
        /// 	DESCRIPCIÓN: Consulta los datos de los productos registrados en la base de datos
        /// 	PARÁMETROS:
        /// 	CREO: Susana Trigueros
        /// 	FECHA_CREO: 02-feb-2013
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Datos_Servicios_Para_Excel()
        {
            String Mi_SQL; //Variable para la consulta de los productos
            DataTable Dt_Productos;
            try
            {
                //Consulta todos los datos de los producto 

                Mi_SQL = "SELECT " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + "." + Cat_Com_Servicios.Campo_Clave;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + "." + Cat_Com_Servicios.Campo_Nombre;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + "." + Cat_Com_Servicios.Campo_Costo;
                Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Com_Impuestos.Campo_Nombre;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Impuestos.Tabla_Cat_Impuestos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Impuestos.Campo_Impuesto_ID;
                Mi_SQL = Mi_SQL + " =" + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + "." + Cat_Com_Servicios.Campo_Impuesto_ID + ") AS IMPUESTO,";
                Mi_SQL = Mi_SQL + "(SELECT " + Cat_Sap_Partidas_Especificas.Campo_Clave + " ||' '|| ";
                Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Campo_Nombre + " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + "=";
                Mi_SQL = Mi_SQL + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + "." + Cat_Com_Servicios.Campo_Partida_ID + ") AS PARTIDA";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios;                



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


        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Alta_Servicios
        ///DESCRIPCIÓN: Realiza una inscercion de un nuevo registro en la tabla de Servicios
        ///PARAMETROS: Datos: Objeto de la clase negocio
        ///CREO: jtoledo
        ///FECHA_CREO: 02/03/2011 11:13:04 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        internal static void Alta_Servicios(Cls_Cat_Com_Servicios_Negocio Datos)
        {
            //Declaraion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL;
            String Clave_SQL;
            Object ID_Consecutivo;
            Object Ultima_Clave;

            //Inicializacion de variables
            Mi_SQL = String.Empty;
            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;
                        
                Mi_SQL = "SELECT NVL(MAX (" + Cat_Com_Servicios.Campo_Servicio_ID + "),'00000') " +
                         " FROM " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios;

                ID_Consecutivo = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!Convert.IsDBNull(ID_Consecutivo))      //Si la consulta no regresa nulo, obtener el nuevo ID
                {
                    Datos.P_Servicio_ID = string.Format("{0:00000}", Convert.ToInt32(ID_Consecutivo) + 1);
                            //Obtener la última clave (PARTIDAID + S + Consecutivo)
                    Clave_SQL = "SELECT * FROM (SELECT " + Cat_Com_Servicios.Campo_Clave;
                    Clave_SQL = Clave_SQL + " FROM " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios;
                    Clave_SQL = Clave_SQL + " WHERE " + Cat_Com_Servicios.Campo_Clave + " LIKE '" + Datos.P_Clave_Partida + "S%'";
                    Clave_SQL = Clave_SQL + " ORDER BY " + Cat_Com_Servicios.Campo_Clave + " DESC";
                    Clave_SQL = Clave_SQL + ") WHERE ROWNUM = 1";
                    Ultima_Clave = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Clave_SQL);
                    if (Ultima_Clave != null)    // Si en la base de datos se encontró una clave con la partida proporcionada,
                    {                                       // sumar 1 al consecutivo para obtener la nueva clave
                        Ultima_Clave = Ultima_Clave.ToString().Substring(Ultima_Clave.ToString().Length - 4);  //Ultimos 4 caracteres del dato obtenido
                        Datos.P_Clave = Datos.P_Clave_Partida + "S" + String.Format("{0:0000}", Convert.ToInt32(Ultima_Clave) + 1);
                    }
                    else                    // Si no se encontraron datos, asignar el primer para la partida
                    {
                        Datos.P_Clave = Datos.P_Clave_Partida + "S0001";
                    }
                }
                else
                {
                    Datos.P_Servicio_ID = "00001";
                    Datos.P_Clave = Datos.P_Clave_Partida + "S0001";
                }
                try
                {
                    Datos.P_Clave = int.Parse(Datos.P_Servicio_ID).ToString();
                }
                catch(Exception Ex)
                {
                    Ex.ToString();
                    Datos.P_Clave = "";
                }

                        //Query para el alta del nuevo Servicio
                Mi_SQL = "INSERT INTO " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + " ( ";
                Mi_SQL += Cat_Com_Servicios.Campo_Servicio_ID + ",";
                Mi_SQL += Cat_Com_Servicios.Campo_Clave + ",";
                Mi_SQL += Cat_Com_Servicios.Campo_Nombre + ",";
                Mi_SQL += Cat_Com_Servicios.Campo_Costo + ",";
                Mi_SQL += Cat_Com_Servicios.Campo_Impuesto_ID + ",";
                Mi_SQL += Cat_Com_Servicios.Campo_Partida_ID + ",";
                Mi_SQL += Cat_Com_Servicios.Campo_Comentarios + ",";
                Mi_SQL += Cat_Com_Servicios.Campo_Usuario_Creo + ",";
                Mi_SQL += Cat_Com_Servicios.Campo_Fecha_Creo + " )";

                Mi_SQL += " VALUES( '";
                Mi_SQL += Datos.P_Servicio_ID + "','";
                Mi_SQL += Datos.P_Clave + "','";
                Mi_SQL += Datos.P_Nombre + "', ";
                Mi_SQL += Datos.P_Costo + ",'";
                Mi_SQL += Datos.P_Impuesto_ID + "','";
                Mi_SQL += Datos.P_Partida_ID + "','";
                Mi_SQL += Datos.P_Comentarios + "','";
                Mi_SQL += Datos.P_Usuario_Creo + "', SYSDATE )";

                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();
                Obj_Transaccion.Commit();
                Obj_Conexion.Close();
            }
            catch (OracleException Ex)
            {
                if (Obj_Transaccion != null)
                {
                    Obj_Transaccion.Rollback();
                }
                String Mensaje = "Error: ";
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "No existe un registro relacionado con esta operacion";
                        break;
                    case "923":
                        Mensaje = "Consulta SQL";
                        break;
                    case "12170":
                        Mensaje = "Conexion con el Servidor";
                        break;
                    default:
                        Mensaje = Ex.Message;
                        break;
                }
                throw new Exception(Mensaje + "[" + Ex.ToString() + "]");
            }
            finally
            {
                Obj_Comando = null;
                Obj_Conexion = null;
                Obj_Transaccion = null;

            }
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Servicios
        ///DESCRIPCIÓN: Realizar la consulta de la lista de servicios filtrada por el nombre del servicio
        ///PARAMETROS: Datos: Objeto de la clase negocio
        ///CREO: jtoledo
        ///FECHA_CREO: 02/04/2011 12:07:35 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public static DataTable Consulta_Servicios(Cls_Cat_Com_Servicios_Negocio Datos)
        {
            DataTable Dt_Temporal = null;
            String Mi_SQL;
            String Filtros_SQL = String.Empty;

            try
            {
                Mi_SQL = "SELECT " +
                         Cat_Com_Servicios.Campo_Servicio_ID + ", " +
                         Cat_Com_Servicios.Campo_Clave + ", " +
                         Cat_Com_Servicios.Campo_Nombre + ", " +
                         Cat_Com_Servicios.Campo_Costo + ", " +
                         Cat_Com_Servicios.Campo_Impuesto_ID + ", " +
                         Cat_Com_Servicios.Campo_Partida_ID + ", " +
                         Cat_Com_Servicios.Campo_Comentarios + ", " +

                         Cat_Com_Servicios.Campo_Usuario_Creo + ", " +
                         Cat_Com_Servicios.Campo_Fecha_Creo +

                    " FROM " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios;

                if (Datos.P_Nombre != null && Datos.P_Nombre != "")
                {
                    Filtros_SQL = " WHERE upper(" + Cat_Com_Servicios.Campo_Nombre + ") LIKE upper('%" +
                    Datos.P_Nombre + "%')";
                }
                if (Datos.P_Clave != null && Datos.P_Clave != "")
                {
                    if (Filtros_SQL.Length > 0)
                    Filtros_SQL += " OR upper(" + Cat_Com_Servicios.Campo_Clave + ") LIKE upper('" +
                    Datos.P_Clave + "%')";
                    else
                        Filtros_SQL += " WHERE upper(" + Cat_Com_Servicios.Campo_Clave + ") LIKE upper('" +
                        Datos.P_Clave + "%')";
                }
                Dt_Temporal = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL + Filtros_SQL).Tables[0];
                return Dt_Temporal;
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
        ///NOMBRE DE LA FUNCIÓN: Baja_Servicios
        ///DESCRIPCIÓN: Ejecuta sentencia para dar de baja un servicio de la base de datos
        ///PARAMETROS: Datos: Objeto de la clase negocio
        ///CREO: jtoledo
        ///FECHA_CREO: 02/04/2011 11:38:06 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        internal static void Baja_Servicios(Cls_Cat_Com_Servicios_Negocio Datos)
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
                Mi_SQL = "DELETE FROM " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Servicios.Campo_Servicio_ID + " = '" + Datos.P_Servicio_ID + "'";

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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Servicio
        ///DESCRIPCIÓN: Ejecuta la sentencia para modificar un registro de un Servicio y lo guarda en la base de datos
        ///PARAMETROS: Datos: Objeto de la clase negocio
        ///CREO: jtoledo
        ///FECHA_CREO: 02/04/2011 12:05:38 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public static void Modificar_Servicio(Cls_Cat_Com_Servicios_Negocio Datos)
        {
            //Declaracion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = String.Empty;
            String Mensaje = String.Empty; //Variable para el mensaje de error
            String Clave_SQL;
            Object Ultima_Clave;

            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;

                //Si P_Clave esta vacia, generar nueva clave
                if (Datos.P_Clave == "")
                {
                    //Obtener la última clave (PARTIDAID + S + Consecutivo)
                    Clave_SQL = "SELECT * FROM (SELECT " + Cat_Com_Servicios.Campo_Clave;
                    Clave_SQL = Clave_SQL + " FROM " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios;
                    Clave_SQL = Clave_SQL + " WHERE " + Cat_Com_Servicios.Campo_Clave + " LIKE '" + Datos.P_Clave_Partida + "S%'";
                    Clave_SQL = Clave_SQL + " ORDER BY " + Cat_Com_Servicios.Campo_Clave + " DESC";
                    Clave_SQL = Clave_SQL + ") WHERE ROWNUM = 1";
                    Ultima_Clave = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Clave_SQL);
                    if (Ultima_Clave != null)    // Si en la base de datos se encontró una clave con la partida proporcionada,
                    {                                       // sumar 1 al consecutivo para obtener la nueva clave
                        Ultima_Clave = Ultima_Clave.ToString().Substring(Ultima_Clave.ToString().Length - 4);  //Ultimos 4 caracteres del dato obtenido
                        Datos.P_Clave = Datos.P_Clave_Partida + "S" + String.Format("{0:0000}", Convert.ToInt32(Ultima_Clave) + 1);
                    }
                    else                    // Si no se encontraron datos, asignar el primer para la partida
                    {
                        Datos.P_Clave = Datos.P_Clave_Partida + "S0001";
                    }
                }

                //Asignar consulta para la modificacion
                Mi_SQL = "UPDATE " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios;
                Mi_SQL = Mi_SQL + " SET " + Cat_Com_Servicios.Campo_Nombre + " = '" + Datos.P_Nombre + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Servicios.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Servicios.Campo_Costo + " = '" + Datos.P_Costo + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Servicios.Campo_Clave + " = '" + Datos.P_Clave + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Servicios.Campo_Partida_ID + " = '" + Datos.P_Partida_ID + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Servicios.Campo_Impuesto_ID + " = '" + Datos.P_Impuesto_ID + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Servicios.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Creo + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Servicios.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Servicios.Campo_Servicio_ID + " = '" + Datos.P_Servicio_ID + "'";

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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Servicio_Proceso
        ///DESCRIPCIÓN: Consulta los servicios que estan en proceso de Requisicion
        ///PARAMETROS: Datos: Objeto de la clase negocio
        ///CREO: susana trigueros
        ///FECHA_CREO: 30-jul-12
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public static DataTable Consultar_Servicio_en_Proceso(Cls_Cat_Com_Servicios_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de llos productos

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " "; ;

                Mi_SQL = Mi_SQL + " FROM ";

                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
                Mi_SQL = Mi_SQL + " JOIN " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto;
                Mi_SQL = Mi_SQL + " ON " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID; ;
                Mi_SQL = Mi_SQL + "=" + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Requisicion_ID;
                Mi_SQL = Mi_SQL + " WHERE ";

                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = ";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Servicio_ID.Trim() + "'";

                Mi_SQL = Mi_SQL + " AND ";

                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Tipo_Articulo + " ='SERVICIO' ";

                Mi_SQL = Mi_SQL + " AND ";

                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Estatus + " IN ('AUTORIZADA', 'COMPRA','COMPRA-RECHAZADA','CONFIRMADA', 'COTIZADA','EN CONSTRUCCION','FILTRADA','GENERADA','PROCESAR','PROVEEDOR','RECHAZADA','RECOTIZAR','REVISAR','R_PROVEEDOR')";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }


        }

    }
}