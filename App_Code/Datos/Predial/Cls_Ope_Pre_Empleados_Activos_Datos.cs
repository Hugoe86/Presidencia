using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Operacion_Predial_Empleados_Activos.Negocio;
using System.Data;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Data.OracleClient;

namespace Presidencia.Operacion_Predial_Empleados_Activos.Datos
{
    public class Cls_Ope_Pre_Empleados_Activos_Datos
    {
        #region Metodos
        public Cls_Ope_Pre_Empleados_Activos_Datos()
        {
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Asignar_Pendiente
        ///DESCRIPCIÓN: metodo para asignar un responsable para revisar los documentos necesarios para realizar el tramite
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 03/23/2011 12:39:17 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public static String Asignar_Pendiente()
        {
            //Declaracion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = String.Empty;  //Variable para la sentencia SQL
            String Mensaje = String.Empty; //Variable para el mensaje de error
            Object Aux; //Variable auxiliar para las consultas            
            String Carga_Trabajo;//Variable para almacenar el campo de concecutivo que almacena el orden de las asignaciones
            String Asignado;
            int Empleados_Activos; //Variable para almacenar el numero de empleados que se escuentran activos
            int Consecutivo; //Variable para actualizar en uno el campo de concecutivo que almacena el orden de las asignaciones
            DataTable Dt_Empleados_Asignacion;//Datatable de los empleados a asignar


            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;
                                                   
                //Formar Sentencia 
                Mi_SQL = "";
                Mi_SQL = "SELECT NVL(MAX(";
                Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Consecutivo + "),0)";
                Mi_SQL = Mi_SQL + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Tabla_Ope_Pre_Empleados_Activos;
            
                //Ejecutar comando
                Obj_Comando.CommandText = Mi_SQL;
                Aux = Obj_Comando.ExecuteScalar();

                if (Convert.IsDBNull(Aux) == false)
                    Carga_Trabajo =  Convert.ToString(Convert.ToInt32(Aux));
                else
                    Carga_Trabajo = "0";

                Mi_SQL = "";
                Mi_SQL = "SELECT COUNT(*) FROM ";
                Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Tabla_Ope_Pre_Empleados_Activos;

                //Ejecutar comando
                Obj_Comando.CommandText = Mi_SQL;
                Aux = Obj_Comando.ExecuteScalar();

                if (Convert.IsDBNull(Aux) == false)
                    Empleados_Activos = Convert.ToInt32(Aux);
                else
                    return null;
                    //throw new Exception("No hay Empleados disponibles para reasignar el pendiente");

                Mi_SQL = "";

                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Empleado_ID +" AS EMPLEADO_ID,";
                Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Consecutivo +" AS CONSECUTIVO,";
                Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Estatus +" AS ESTATUS";                    
                Mi_SQL = Mi_SQL + " FROM ";

                Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Tabla_Ope_Pre_Empleados_Activos;
                Mi_SQL = Mi_SQL + " WHERE ";
                Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Consecutivo + " < " + Carga_Trabajo;
                Mi_SQL = Mi_SQL + " AND ";
                Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Estatus + " = 'ACTIVO'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pre_Empleados_Activos.Campo_Empleado_ID ;

                Dt_Empleados_Asignacion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                                
                if (Dt_Empleados_Asignacion.Rows.Count == Empleados_Activos || Dt_Empleados_Asignacion.Rows.Count == 0)
                {
                    Mi_SQL = "";
                    Mi_SQL = "SELECT ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Empleado_ID + " AS EMPLEADO_ID,";
                    Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Consecutivo + " AS CONSECUTIVO,";
                    Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Estatus + " AS ESTATUS";
                    Mi_SQL = Mi_SQL + " FROM ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Tabla_Ope_Pre_Empleados_Activos;
                    Mi_SQL = Mi_SQL + " WHERE ";                    
                    Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Estatus + " = 'ACTIVO'";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pre_Empleados_Activos.Campo_Empleado_ID;
                    Dt_Empleados_Asignacion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                }


                if (Dt_Empleados_Asignacion.Rows.Count > 0)
                {
                    Asignado = Dt_Empleados_Asignacion.Rows[0]["EMPLEADO_ID"].ToString();
                    Consecutivo = Convert.ToInt32(Dt_Empleados_Asignacion.Rows[0][Ope_Pre_Empleados_Activos.Campo_Consecutivo].ToString()) + 1;
                    Mi_SQL = "";
                    Mi_SQL = "UPDATE ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Tabla_Ope_Pre_Empleados_Activos;
                    Mi_SQL = Mi_SQL + " SET ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Consecutivo + " = " + Consecutivo;
                    Mi_SQL = Mi_SQL + " WHERE ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Empleado_ID + " = '" + Asignado + "'";


                    //Ejecutar comando
                    Obj_Comando.CommandText = Mi_SQL;
                    Obj_Comando.ExecuteNonQuery();

                    //Ejecutar transaccion
                    Obj_Transaccion.Commit();
                    Obj_Conexion.Close();
                }
                else
                {
                    return null;
                    //throw new Exception("No hay Empleados activos para asignarle la recepcion");
                    //Obj_Transaccion.Rollback();
                }
                   
                    

                    return Asignado;

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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Empleado_Activo
        ///DESCRIPCIÓN: metodo para cambiar el estatus de un empleado
        ///PARAMETROS: Datos: Objeto de la clase negocio con los datos a actualizar
        ///CREO: jtoledo
        ///FECHA_CREO: 03/23/2011 1:39:17 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public static String Modificar_Empleado_Activo(Cls_Ope_Pre_Empleado_Activos_Negocio Datos)
        {
            //Declaracion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = String.Empty;  //Variable para la sentencia SQL
            String Mensaje = String.Empty; //Variable para el mensaje de error
            String Resultado = ""; //Variable para el resultado
            Object Aux; //Variable auxiliar para las consultas
            int Movimientos;

            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;

                if (Datos.P_Dt_Empleados.Rows.Count > 0)
                {

                    foreach (DataRow Renglon_Temporal in Datos.P_Dt_Empleados.Rows)
                    {
                        Mi_SQL = "";
                        Mi_SQL = "SELECT COUNT(*) FROM ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs;
                        Mi_SQL = Mi_SQL + " WHERE ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Empleado_ID;
                        Mi_SQL = Mi_SQL + " = '";
                        Mi_SQL = Mi_SQL + Renglon_Temporal["EMPLEADO_ID"] + "'";
                        
                        //Ejecutar comando
                        Obj_Comando.CommandText = Mi_SQL;
                        Aux = Obj_Comando.ExecuteScalar();                        
                        Movimientos = Convert.ToInt32(Aux);

                        //if (Movimientos <= 0)
                        //{

                            //Formar Sentencia 
                            Mi_SQL = "";
                            Mi_SQL = "UPDATE " + Ope_Pre_Empleados_Activos.Tabla_Ope_Pre_Empleados_Activos + " SET ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Estatus + " = '";
                            Mi_SQL = Mi_SQL + Renglon_Temporal[Ope_Pre_Empleados_Activos.Campo_Estatus] + "', ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Usuario_Modifico + " = '";
                            Mi_SQL = Mi_SQL + Datos.P_Usuario + "',";
                            Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Fecha_Modifico + " = SYSDATE ";
                            Mi_SQL = Mi_SQL + "WHERE ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Empleado_ID + " = ";
                            Mi_SQL = Mi_SQL + Renglon_Temporal["EMPLEADO_ID"] + " ";

                            //Ejecutar comando
                            Obj_Comando.CommandText = Mi_SQL;
                            Obj_Comando.ExecuteNonQuery();
                        //}
                        //else
                        //{
                        //    if (Renglon_Temporal[Ope_Pre_Empleados_Activos.Campo_Estatus].ToString() == "INACTIVO")
                        //    {
                        //        Mi_SQL = "";
                        //        Mi_SQL = "SELECT ";
                        //        Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " || ' ' || ";
                        //        Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ";
                        //        Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " AS NOMBRE_EMPLEADO ";
                        //        //FROM;
                        //        Mi_SQL = Mi_SQL + " FROM ";
                        //        Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados;
                        //        //WHERE
                        //        Mi_SQL = Mi_SQL + " WHERE ";
                        //        Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Empleado_ID;
                        //        Mi_SQL = Mi_SQL + " = '";
                        //        Mi_SQL = Mi_SQL + Renglon_Temporal["EMPLEADO_ID"] + "'";
                        //        Resultado = Resultado + "- ";
                        //        Resultado = Resultado + OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0]["NOMBRE_EMPLEADO"].ToString();
                        //        Resultado = Resultado + "</br>";
                        //    }
                        //}
                    }
                    //Ejecutar transaccion
                    Obj_Transaccion.Commit();
                    Obj_Conexion.Close();
                }

                return Resultado;
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
        ///NOMBRE DE LA FUNCIÓN: Reasignar_Movimiento
        ///DESCRIPCIÓN: metodo para cambiar el estatus de un empleado
        ///PARAMETROS: Datos: Objeto de la clase negocio con los datos a actualizar
        ///CREO: jtoledo
        ///FECHA_CREO: 03/23/2011 1:39:17 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public static void Reasignar_Movimiento(Cls_Ope_Pre_Empleado_Activos_Negocio Datos)
        {
            //Declaracion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            OracleTransaction Obj_Transaccion_1 = null;
            OracleConnection Obj_Conexion_1;
            OracleCommand Obj_Comando_1;
            String Mi_SQL = String.Empty;  //Variable para la sentencia SQL
            String Mensaje = String.Empty; //Variable para el mensaje de error
            String Empleado_Nuevo;
            int Consecutivo;
            Object Aux; //Variable auxiliar para las consultas            

            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;

                Obj_Conexion_1 = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando_1 = new OracleCommand();
                Obj_Conexion_1.Open();
                Obj_Transaccion_1 = Obj_Conexion_1.BeginTransaction();
                Obj_Comando_1.Transaction = Obj_Transaccion_1;
                Obj_Comando_1.Connection = Obj_Conexion_1;
                                
                        //Formar Sentencia 
                        Mi_SQL = "";
                        Mi_SQL = "UPDATE " + Ope_Pre_Empleados_Activos.Tabla_Ope_Pre_Empleados_Activos + " SET ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Estatus + " = 'INACTIVO' ";                        
                        Mi_SQL = Mi_SQL + " WHERE ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Empleado_ID + " = '";
                        Mi_SQL = Mi_SQL + Datos.P_Empleado_ID + "' ";

                        //Ejecutar comando
                        Obj_Comando_1.CommandText = Mi_SQL;
                        Obj_Comando_1.ExecuteNonQuery();
                        //Ejecutar transaccion
                        Obj_Transaccion_1.Commit();
                        Obj_Conexion_1.Close();
                        

                        Empleado_Nuevo = Asignar_Pendiente();                

                        //Formar Sentencia 
                        Mi_SQL = "";
                        Mi_SQL = "UPDATE " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + " SET ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Empleado_ID + " = '" + Empleado_Nuevo + "' ";
                        Mi_SQL = Mi_SQL + " WHERE ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + " = '";
                        Mi_SQL = Mi_SQL + Datos.P_No_Movimiento + "' ";

                        //Ejecutar comando
                        Obj_Comando.CommandText = Mi_SQL;
                        Obj_Comando.ExecuteNonQuery();

                        //Formar Sentencia 
                        Mi_SQL = "";
                        Mi_SQL = "SELECT ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Consecutivo;
                        Mi_SQL = Mi_SQL + " FROM ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Tabla_Ope_Pre_Empleados_Activos;
                        Mi_SQL = Mi_SQL + " WHERE ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Empleado_ID + " = '";
                        Mi_SQL = Mi_SQL + Datos.P_Empleado_ID + "' ";

                        //Ejecutar comando
                        Obj_Comando.CommandText = Mi_SQL;
                        Aux = Obj_Comando.ExecuteScalar();

                        Consecutivo = Convert.ToInt32(Aux) - 1 ;

                        //Formar Sentencia 
                        Mi_SQL = "";
                        Mi_SQL = "UPDATE " + Ope_Pre_Empleados_Activos.Tabla_Ope_Pre_Empleados_Activos + " SET ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Estatus + " = 'ACTIVO', ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Consecutivo + " = " + Consecutivo;
                        Mi_SQL = Mi_SQL + " WHERE ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Empleado_ID + " = '";
                        Mi_SQL = Mi_SQL + Datos.P_Empleado_ID + "' ";

                        //Ejecutar comando
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
        ///NOMBRE DE LA FUNCIÓN: Consulta_Empleados_Rol
        ///DESCRIPCIÓN: consulta una lista de los empleados activos con el rol de translado de dominio
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 03/23/2011 01:55:36 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public static DataTable Consulta_Empleados_Rol()
        {
            //Declaracion de Variables
            String Mi_SQL = String.Empty; //Variable para las consultas

            try
            {
                //Asignar consulta para el listado
                //Campos de la tabla de empleados
                Mi_SQL = "SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Area_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ", ";
                //Nombre de la Dependencia
                Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS NOMBRE_DEPENDENCIA, ";
                //Nombre del Area
                Mi_SQL = Mi_SQL + Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Nombre + " AS NOMBRE_AREA ";
                //From
                Mi_SQL = Mi_SQL + "FROM " + Cat_Empleados.Tabla_Cat_Empleados + ", ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + ", ";
                Mi_SQL = Mi_SQL + Cat_Areas.Tabla_Cat_Areas + " ";
                //Clausula WHERE
                Mi_SQL = Mi_SQL + " WHERE";                
                Mi_SQL = Mi_SQL + " " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " = ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + " ";
                Mi_SQL = Mi_SQL + " AND " + Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Area_ID + " = ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Area_ID + " ";
                Mi_SQL = Mi_SQL + " AND upper(" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ") LIKE upper('%IMPUESTOS INMOB%')";
                Mi_SQL = Mi_SQL + " AND upper(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Estatus + ") = upper('ACTIVO')";
                //Mi_SQL = Mi_SQL + "AND upper(" + Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Nombre + ") LIKE upper('%TRASLADO DE DOMINIO%')";

                //Ordenacion
                Mi_SQL = Mi_SQL + "ORDER BY " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " ";

                //Entregar resultado
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Oracle.DataAccess.Client.OracleException ex)
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
        ///NOMBRE DE LA FUNCIÓN: Consulta_Empleados_Activos
        ///DESCRIPCIÓN: consulta los empleados guardados en la tabla de empleados activos
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 03/23/2011 01:55:36 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        internal static DataTable Consulta_Empleados_Activos()
        {
            //Declaracion de Variables
            String Mi_SQL = String.Empty; //Variable para las consultas

            try
            {
                //Asignar consulta para el listado
                //Campos de la tabla de empleados activos
                Mi_SQL = "SELECT " + Ope_Pre_Empleados_Activos.Tabla_Ope_Pre_Empleados_Activos + "." + Ope_Pre_Empleados_Activos.Campo_Empleado_ID + " AS EMPLEADO_ID, ";
                Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Tabla_Ope_Pre_Empleados_Activos + "." + Ope_Pre_Empleados_Activos.Campo_Consecutivo + " AS CONSECUTIVO, ";
                Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Tabla_Ope_Pre_Empleados_Activos + "." + Ope_Pre_Empleados_Activos.Campo_Estatus + " AS ESTATUS, ";
                //Campos de la tabla de empleados                
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " || ' ' || ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " AS NOMBRE_EMPLEADO, ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " ";

                //From
                Mi_SQL = Mi_SQL + "FROM " + Ope_Pre_Empleados_Activos.Tabla_Ope_Pre_Empleados_Activos + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + " ";              
                //Clausula WHERE
                Mi_SQL = Mi_SQL + "WHERE ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " = ";
                Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Tabla_Ope_Pre_Empleados_Activos + "." + Ope_Pre_Empleados_Activos.Campo_Empleado_ID + " ";
                //Ordenacion
                Mi_SQL = Mi_SQL + "ORDER BY " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " ";

                //Entregar resultado
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Oracle.DataAccess.Client.OracleException ex)
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
        ///NOMBRE DE LA FUNCIÓN: Generar_Tabla_Empleados_Activos
        ///DESCRIPCIÓN: inserta los empleados guardados en la tabla de empleados activos
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 03/23/2011 01:55:36 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        internal static void Generar_Tabla_Empleados_Activos()
        {
            //Declaracion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = String.Empty;  //Variable para la sentencia SQL
            String Mensaje = String.Empty; //Variable para el mensaje de error
            DataTable Dt_Empleados;
            DataTable Dt_Movimientos;
            DataTable Dt_Empleados_Activos;
            Object Aux; //Variable auxiliar para las consultas            
            String No_Consecutivo;


            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;
                
                Dt_Empleados_Activos = Consulta_Empleados_Activos();
                Dt_Empleados = Consulta_Empleados_Rol();

                if (Dt_Empleados_Activos.Rows.Count <= 0)
                {
                    if (Dt_Empleados.Rows.Count > 0)
                    {
                        foreach (DataRow Renglon_Temporal in Dt_Empleados.Rows)
                        {
                            
                            No_Consecutivo = "0";
                            //Formar Sentencia
                            Mi_SQL = "";
                            Mi_SQL = "INSERT INTO " + Ope_Pre_Empleados_Activos.Tabla_Ope_Pre_Empleados_Activos + " ( ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Empleado_ID + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Consecutivo + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Estatus + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Usuario_Creo + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Fecha_Creo + ")";

                            Mi_SQL = Mi_SQL + "VALUES('" + Renglon_Temporal["EMPLEADO_ID"] + "','";
                            Mi_SQL = Mi_SQL + No_Consecutivo + "','";
                            Mi_SQL = Mi_SQL + "ACTIVO','" + Cls_Sessiones.Nombre_Empleado + "',SYSDATE)";

                            //Ejecutar comando
                            Obj_Comando.CommandText = Mi_SQL;
                            Obj_Comando.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    foreach (DataRow Renglon_Temporal in Dt_Empleados.Rows)
                    {
                        DataRow[] Dr_Encontrados = Dt_Empleados_Activos.Select("Empleado_ID" + " = " + Renglon_Temporal["Empleado_ID"]);
                        if (Dr_Encontrados.Length <=0)
                        {
                            Mi_SQL = "";
                            Mi_SQL = "INSERT INTO " + Ope_Pre_Empleados_Activos.Tabla_Ope_Pre_Empleados_Activos + " ( ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Empleado_ID + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Consecutivo + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Estatus + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Usuario_Creo + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Fecha_Creo + ")";

                            Mi_SQL = Mi_SQL + "VALUES('" + Renglon_Temporal["EMPLEADO_ID"] + "','0','";
                            Mi_SQL = Mi_SQL + "ACTIVO','" + Cls_Sessiones.Nombre_Empleado + "',SYSDATE)";

                            //Ejecutar comando
                            Obj_Comando.CommandText = Mi_SQL;
                            Obj_Comando.ExecuteNonQuery();
                        }
                    }

                    foreach (DataRow Renglon_Temporal in Dt_Empleados_Activos.Rows)
                    {
                        DataRow[] Dr_Encontrados = Dt_Empleados.Select("EMPLEADO_ID = " + Renglon_Temporal["EMPLEADO_ID"]);
                        if (Dr_Encontrados.Length <= 0)
                        {                            
                                Mi_SQL = "";
                                Mi_SQL = "UPDATE " + Ope_Pre_Empleados_Activos.Tabla_Ope_Pre_Empleados_Activos;
                                Mi_SQL = " SET " + Ope_Pre_Empleados_Activos.Campo_Estatus + "= 'INACTIVO'";
                                Mi_SQL = Mi_SQL + " WHERE ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Empleados_Activos.Campo_Empleado_ID + "= '" + Renglon_Temporal["EMPLEADO_ID"] + "'";

                                //Ejecutar comando
                                Obj_Comando.CommandText = Mi_SQL;
                                Obj_Comando.ExecuteNonQuery();                            
                        }
                    }

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
        ///NOMBRE DE LA FUNCIÓN: Consulta_Movimientos
        ///DESCRIPCIÓN: consulta los empleados guardados en la tabla de empleados activos
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 03/23/2011 01:55:36 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        internal static DataTable Consulta_Movimientos(Cls_Ope_Pre_Empleado_Activos_Negocio Datos)
        {
            //Declaracion de Variables
            String Mi_SQL = String.Empty; //Variable para las consultas

            try
            {
                //Asignar consulta para el listado                
                Mi_SQL = " SELECT ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + ".*, ";
                
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + ".";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ";

                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Tabla_Ope_Pre_Recepcion_Documentos +".";
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Tabla_Ope_Pre_Recepcion_Documentos + ".";
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Notario_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Tabla_Ope_Pre_Recepcion_Documentos + ".";
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Observaciones + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Tabla_Ope_Pre_Recepcion_Documentos + ".";
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Fecha + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Tabla_Ope_Pre_Recepcion_Documentos + ".";
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Clave_Tramite + ", ";

                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Tabla_Cat_Pre_Notarios + ".";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Numero_Notaria + ", ";

                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Tabla_Cat_Pre_Notarios + ".";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Nombre + " || ' ' ||";

                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Tabla_Cat_Pre_Notarios + ".";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Apellido_Paterno + " || ' ' || ";

                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Tabla_Cat_Pre_Notarios + ".";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Apellido_Materno + " AS NOMBRE_NOTARIO ";

                //From
                Mi_SQL = Mi_SQL + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Tabla_Ope_Pre_Recepcion_Documentos + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Tabla_Cat_Pre_Notarios + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " ";
                
                //Clausula WHERE
                Mi_SQL = Mi_SQL + " WHERE ";

                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + ".";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + ".";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Cuenta_Predial_ID + " ";

                Mi_SQL = Mi_SQL + " AND ";

                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Tabla_Ope_Pre_Recepcion_Documentos +".";
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + " = ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + ".";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " ";

                Mi_SQL = Mi_SQL + " AND ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + ".";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Empleado_ID + " = ";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Empleado_ID + "' ";

                Mi_SQL = Mi_SQL + " AND ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Tabla_Cat_Pre_Notarios + ".";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Notario_ID + " = ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Tabla_Ope_Pre_Recepcion_Documentos + ".";
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Notario_ID + " ";                

                //Entregar resultado
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Oracle.DataAccess.Client.OracleException ex)
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
    
        #endregion
    }
}