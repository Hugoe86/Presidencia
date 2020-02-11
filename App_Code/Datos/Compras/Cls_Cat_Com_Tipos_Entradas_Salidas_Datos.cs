using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Catalogo_Compras_Tipos_Entradas_Salidas.Negocio;
using System.Data.OracleClient;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data;

namespace Presidencia.Catalogo_Compras_Tipos_Entradas_Salidas.Datos
{
    public class Cls_Cat_Com_Tipos_Entradas_Salidas_Datos
    {
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Alta_Tipo_Movimiento
        ///DESCRIPCIÓN: Dar de Alta un nuevo tipo de movimiento en la base de datos
        ///PARAMETROS: Datos: Objeto de la Clase negocio
        ///CREO: jtoledo
        ///FECHA_CREO: 02/08/2011 05:09:22 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static void Alta_Tipo_Movimiento(Cls_Cat_Com_Tipos_Entradas_Salidas_Negocio Datos)
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
                Mi_SQL = "SELECT NVL(MAX(" + Cat_Com_Tipos_Ent_Sal.Campo_Tipo_Movimiento_ID + "), '00000') FROM " + Cat_Com_Tipos_Ent_Sal.Tabla_Cat_Com_Tipos_Ent_Sal;

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Aux = Obj_Comando.ExecuteScalar();

                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false)
                    Datos.P_Tipo_Movimiento_ID = String.Format("{0:00000}", Convert.ToInt32(Aux) + 1);
                else
                    Datos.P_Tipo_Movimiento_ID = "00001";

                //Consulta para la insercion
                Mi_SQL = "INSERT INTO " + Cat_Com_Tipos_Ent_Sal.Tabla_Cat_Com_Tipos_Ent_Sal + " ( " + Cat_Com_Tipos_Ent_Sal.Campo_Tipo_Movimiento_ID + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Tipos_Ent_Sal.Campo_Nombre + "," + Cat_Com_Tipos_Ent_Sal.Campo_Abreviatura + "," + Cat_Com_Tipos_Ent_Sal.Campo_Tipo + "," + Cat_Com_Tipos_Ent_Sal.Campo_Comentarios + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Tipos_Ent_Sal.Campo_Usuario_Creo + "," + Cat_Com_Tipos_Ent_Sal.Campo_Fecha_Creo + " ) VALUES ('" + Datos.P_Tipo_Movimiento_ID + "',";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Nombre + "','" + Datos.P_Abreviatura + "','" + Datos.P_Tipo + "','" + Datos.P_Comentarios + "',";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Usuario_Creo + "',SYSDATE)";

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
        ///NOMBRE DE LA FUNCIÓN: Baja_Tipo_Movimiento
        ///DESCRIPCIÓN: Dar de baja un tipo de movimiento en la base de datos
        ///PARAMETROS: Datos: Objeto de la Clase negocio
        ///CREO: jtoledo
        ///FECHA_CREO: 02/08/2011 05:30:43 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static void Baja_Tipo_Movimiento(Cls_Cat_Com_Tipos_Entradas_Salidas_Negocio Datos)
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
                Mi_SQL = Mi_SQL + "DELETE FROM " + Cat_Com_Tipos_Ent_Sal.Tabla_Cat_Com_Tipos_Ent_Sal + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Tipos_Ent_Sal.Campo_Tipo_Movimiento_ID + " = '" + Datos.P_Tipo_Movimiento_ID + "'";

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
        ///NOMBRE DE LA FUNCIÓN: Cambio_Tipo_Movimiento
        ///DESCRIPCIÓN: modificar los datos de un tipo de movimiento existente en la base de datos
        ///PARAMETROS: Datos: Objeto de la Clase negocio
        ///CREO: jtoledo
        ///FECHA_CREO: 02/08/2011 05:31:18 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static void Cambio_Tipo_Movimiento(Cls_Cat_Com_Tipos_Entradas_Salidas_Negocio Datos)
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
                Mi_SQL = "UPDATE " + Cat_Com_Tipos_Ent_Sal.Tabla_Cat_Com_Tipos_Ent_Sal + " SET " + Cat_Com_Tipos_Ent_Sal.Campo_Nombre + " = '" + Datos.P_Nombre + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Tipos_Ent_Sal.Campo_Abreviatura + " = '" + Datos.P_Abreviatura + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Tipos_Ent_Sal.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Tipos_Ent_Sal.Campo_Tipo + " = '" + Datos.P_Tipo + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Tipos_Ent_Sal.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Creo + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Tipos_Ent_Sal.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Tipos_Ent_Sal.Campo_Tipo_Movimiento_ID + " = '" + Datos.P_Tipo_Movimiento_ID + "'";

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
        ///NOMBRE DE LA FUNCIÓN: Consulta_Tipo_Movimiento
        ///DESCRIPCIÓN: consulta los datos de un tipo de movimiento existente en la base de datos
        ///PARAMETROS: Datos: Objeto de la Clase negocio
        ///CREO: jtoledo
        ///FECHA_CREO: 02/08/2011 05:33:34 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static System.Data.DataTable Consulta_Tipo_Movimiento(Cls_Cat_Com_Tipos_Entradas_Salidas_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty;

            try
            {
                //Asignar consulta para los datos
                Mi_SQL = "SELECT " + Cat_Com_Tipos_Ent_Sal.Campo_Tipo_Movimiento_ID + "," + Cat_Com_Tipos_Ent_Sal.Campo_Nombre + "," + Cat_Com_Tipos_Ent_Sal.Campo_Abreviatura + ",";
                Mi_SQL = Mi_SQL + Cat_Com_Tipos_Ent_Sal.Campo_Comentarios + "," + Cat_Com_Tipos_Ent_Sal.Campo_Tipo + " FROM " + Cat_Com_Tipos_Ent_Sal.Tabla_Cat_Com_Tipos_Ent_Sal + " ";

                //Verificar si hay un ID
                if (Datos.P_Tipo_Movimiento_ID != "" && Datos.P_Tipo_Movimiento_ID != String.Empty && Datos.P_Tipo_Movimiento_ID != null)
                    Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Tipos_Ent_Sal.Campo_Tipo_Movimiento_ID + " = '" + Datos.P_Tipo_Movimiento_ID + "' ";
                else
                    if (Datos.P_Nombre != "" && Datos.P_Nombre != String.Empty && Datos.P_Nombre != null)
                    {
                        Mi_SQL = Mi_SQL + " WHERE ( upper(" + Cat_Com_Tipos_Ent_Sal.Campo_Nombre + ") LIKE upper('%" + Datos.P_Nombre + "%')";
                        Mi_SQL = Mi_SQL + " OR upper(" + Cat_Com_Tipos_Ent_Sal.Campo_Abreviatura + ") LIKE upper('%" + Datos.P_Nombre + "%' ))";
                    }

                //Ordenar
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Tipos_Ent_Sal.Campo_Nombre;

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