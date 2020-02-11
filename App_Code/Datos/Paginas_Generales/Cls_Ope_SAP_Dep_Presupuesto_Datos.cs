using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
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
using Presidencia.SAP_Operacion_Departamento_Presupuesto.Negocio;

namespace Presidencia.SAP_Operacion_Departamento_Presupuesto.Datos
{
    public class Cls_Ope_SAP_Dep_Presupuesto_Datos
    {
        public Cls_Ope_SAP_Dep_Presupuesto_Datos()
        {
        }

        //Transaccion
        public static void Metodo() 
        {
            String Mi_Sql = "";
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion);
            OracleCommand Comando_SQL = new OracleCommand();
            OracleTransaction Transaccion_SQL;
            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open();
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);
            Comando_SQL.Connection = Conexion_Base;
            Comando_SQL.Transaction = Transaccion_SQL;
            try
            {
                Mi_Sql = "INSERT ";

                Comando_SQL.CommandText = Mi_Sql;
                Comando_SQL.ExecuteNonQuery();

                Mi_Sql = "UPDATE ";
                Comando_SQL.CommandText = Mi_Sql;
                Comando_SQL.ExecuteNonQuery();




                Transaccion_SQL.Commit();
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Información: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Información: " + Ex.Message);
            }
            finally
            {
                Conexion_Base.Close();
            }

        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Actualizar_Montos_Presupuesto()
        /// 	DESCRIPCIÓN: Actualiza el monto disponible al igual que el monto comprometido.
        /// 	PARÁMETROS:
        /// 		   Datos: Instancia de la clase de negocio con los datos para actualizar la base de datos
        /// 	CREO: Salvador L. Rea Ayala
        /// 	FECHA_CREO: 19/Octubre/2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static void Actualizar_Montos_Presupuesto(Cls_Ope_SAP_Dep_Presupuesto_Negocio Datos)
        {
            String Mi_SQL; //Contiene la consulta de modificación hacía la base de datos
            try
            {
                //Da de Alta los datos del Nuevo Parametro con los datos proporcionados por el usuario.
                Mi_SQL = "UPDATE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                Mi_SQL += " SET " + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + " = " + Datos.P_Disponible + ", ";
                Mi_SQL += Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + " = " + Datos.P_Comprometido;
                Mi_SQL += " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "' ";
                Mi_SQL += " AND " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID + " = '" + Datos.P_Fuente_Financiamiento_ID + "' ";
                Mi_SQL += " AND " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID + " = '" + Datos.P_Partida_ID + "' ";
                Mi_SQL += " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + " = '" + Datos.P_Programa_ID + "'";

                //Manda Mi_SQL para ser procesada por ORACLE.
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Dependencia_Partida_ID
        /// 	DESCRIPCIÓN: Consulta las dependencias ligadas a una partida especifica
        /// 	PARÁMETROS:  Datos: Indica qué registro se desea consultar a la base de datos
        /// 	CREO: Salvador L. Rea Ayala
        /// 	FECHA_CREO: 04-mar-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Dependencia_Partida_ID(Cls_Ope_SAP_Dep_Presupuesto_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "SELECT DISTINCT " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID;
                Mi_SQL += " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                Mi_SQL += " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID + " = '" + Datos.P_Partida_ID + "'";
                Mi_SQL += " ORDER BY " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + " ASC";

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
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Dependencia_Programa_ID
        /// 	DESCRIPCIÓN: Consulta los Programas ligados a las Dependencias
        /// 	PARÁMETROS:  Datos: Indica qué registro se desea consultar a la base de datos
        /// 	CREO: Salvador L. Rea Ayala
        /// 	FECHA_CREO: 6/Octubre/2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Dependencia_Programa_ID(Cls_Ope_SAP_Dep_Presupuesto_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "SELECT DISTINCT " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID;
                Mi_SQL += " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                Mi_SQL += " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "'";
                Mi_SQL += " ORDER BY " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + " ASC";

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
        }
        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Alta_Presupuestos
        /// 	DESCRIPCIÓN: 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///                  2. Modifica los montos Disponible y comprometido de la tabla Ope_SAP_Pres_Partidas
        ///                  3. Da de Alta el Presupuesto en la BD con los datos proporcionados por el usuario
        ///             Uso de transacciones para asegurar consistencia, pues se modifican dos tablas
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio con los datos que serán insertados en la base de datos
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 04-mar-2011
        /// 	MODIFICÓ: Roberto González Oseguera
        /// 	FECHA_MODIFICÓ: 04-mar-2011
        /// 	CAUSA_MODIFICACIÓN: Se comento el codigo que valida y afecta el presupuesto en Ope_Com_Pres_Partida
        ///*******************************************************************************************************
        public static void Alta_Presupuestos(Cls_Ope_SAP_Dep_Presupuesto_Negocio Datos)
        {
            String Mi_SQL_Alta = "";        //Contiene la cadena de inserción hacía la base de datos
            Object Presupuesto_ID;          //Obtiene el ID con la cual se guardaran los datos en la base de datos
            //String Mi_SQL_Modifica = "";    //Contiene la cadena de actualización de montos presupuesto
            //DataTable Dt_Pres_Partida;      //Para almacenar los datos del Presupuesto de la partida
            //Double Monto_Disponible_Presupuesto_Partida = 0.0;
            //Double Monto_Comprometido_Presupuesto_Partida = 0.0;
            //Double Monto_Presupuesto_Dep = Convert.ToDouble(Datos.P_Monto_Presupuestal);
                    //Variable para transacción 
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion);
            OracleCommand Comando_SQL = new OracleCommand();
            OracleTransaction Transaccion_SQL;
            
                // Si no hay una conexión hacia la bd, abrirla
            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open();
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);
            Comando_SQL.Connection = Conexion_Base;
            Comando_SQL.Transaction = Transaccion_SQL;

            try
            {
                            // Obtener el ID del último registro insertado y sumar 1 (ID del Dep_Presupuesto a insertar)
                Mi_SQL_Alta = "SELECT MAX(" + Cat_Com_Dep_Presupuesto.Campo_Presupuesto_ID + ") ";
                Mi_SQL_Alta = Mi_SQL_Alta + "FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                Presupuesto_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL_Alta);

                if (Convert.IsDBNull(Presupuesto_ID))
                {
                    Datos.P_Presupuesto_ID = 1;
                }
                else
                {
                    Datos.P_Presupuesto_ID = Convert.ToInt32(Presupuesto_ID) + 1;
                }

                //    // Consultar el presupuesto de la Partida
                //Dt_Pres_Partida = Consulta_Ope_Pres_Partidas(Datos);
                //// si se obtuvieron datos, formar la consulta para modificar presupuesto
                //if (Dt_Pres_Partida.Rows.Count > 0)
                //{
                //            //Asignar valor a las variables con Montos Disponible y Comprometido
                //    Monto_Disponible_Presupuesto_Partida = Convert.ToDouble(Dt_Pres_Partida.Rows[0][Ope_Com_Pres_Partida.Campo_Monto_Disponible].ToString());
                //    Monto_Comprometido_Presupuesto_Partida = Convert.ToDouble(Dt_Pres_Partida.Rows[0][Ope_Com_Pres_Partida.Campo_Monto_Comprometido].ToString());

                //    //Verificar que el monto asignado es menor que el monto disponible
                //    if ((Monto_Disponible_Presupuesto_Partida - Monto_Presupuesto_Dep) >= 0)
                //    {
                //        //Obtener el nuevo monto Disponible del Presupuesto_Partida restando el monto presupuestal. Sumar el monto presupuestal al comprometido
                //        Monto_Disponible_Presupuesto_Partida = Monto_Disponible_Presupuesto_Partida - Monto_Presupuesto_Dep;
                //        Monto_Comprometido_Presupuesto_Partida = Monto_Comprometido_Presupuesto_Partida + Monto_Presupuesto_Dep;
                //        //Formar la consulta que actualiza los montos disponible y comprometido de Presupuesto_Partida
                //        Mi_SQL_Modifica = "UPDATE " + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + " SET ";
                //        Mi_SQL_Modifica = Mi_SQL_Modifica + Ope_Com_Pres_Partida.Campo_Monto_Disponible + " = '" + Monto_Disponible_Presupuesto_Partida + "', ";
                //        Mi_SQL_Modifica = Mi_SQL_Modifica + Ope_Com_Pres_Partida.Campo_Monto_Comprometido + " = '" + Monto_Comprometido_Presupuesto_Partida + "', ";
                //        Mi_SQL_Modifica = Mi_SQL_Modifica + Ope_Com_Pres_Partida.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                //        Mi_SQL_Modifica = Mi_SQL_Modifica + Ope_Com_Pres_Partida.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                //        Mi_SQL_Modifica = Mi_SQL_Modifica + Ope_Com_Pres_Partida.Campo_Presupuesto_Partida_ID + " = '" + Dt_Pres_Partida.Rows[0][Ope_Com_Pres_Partida.Campo_Presupuesto_Partida_ID] + "'";
                //    }
                //}

                //Consulta para la inserción de la Partida con los datos proporcionados por el usuario en Ope_SAP_Dep_Presupuesto
                Mi_SQL_Alta = "INSERT INTO " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + " (";
                Mi_SQL_Alta = Mi_SQL_Alta + Cat_Com_Dep_Presupuesto.Campo_Presupuesto_ID + ", ";
                Mi_SQL_Alta = Mi_SQL_Alta + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + ", ";
                Mi_SQL_Alta = Mi_SQL_Alta + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + ", ";
                Mi_SQL_Alta = Mi_SQL_Alta + Cat_Com_Dep_Presupuesto.Campo_Partida_ID + ", ";
                Mi_SQL_Alta = Mi_SQL_Alta + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID + ", ";
                Mi_SQL_Alta = Mi_SQL_Alta + Cat_Com_Dep_Presupuesto.Campo_Comentarios + ", ";
                Mi_SQL_Alta = Mi_SQL_Alta + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + ", ";
                Mi_SQL_Alta = Mi_SQL_Alta + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ", ";
                Mi_SQL_Alta = Mi_SQL_Alta + Cat_Com_Dep_Presupuesto.Campo_Monto_Presupuestal + ", ";
                Mi_SQL_Alta = Mi_SQL_Alta + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + ", ";
                Mi_SQL_Alta = Mi_SQL_Alta + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + ", ";
                Mi_SQL_Alta = Mi_SQL_Alta + Cat_Com_Dep_Presupuesto.Campo_Monto_Ejercido + ", ";
                Mi_SQL_Alta = Mi_SQL_Alta + Cat_Com_Dep_Presupuesto.Campo_Fecha_Asignacion + ", ";
                Mi_SQL_Alta = Mi_SQL_Alta + Cat_Com_Dep_Presupuesto.Campo_Usuario_Creo + ", ";
                Mi_SQL_Alta = Mi_SQL_Alta + Cat_Com_Dep_Presupuesto.Campo_Fecha_Creo + ") VALUES (";
                Mi_SQL_Alta = Mi_SQL_Alta + Datos.P_Presupuesto_ID + ", '";
                Mi_SQL_Alta = Mi_SQL_Alta + Datos.P_Dependencia_ID + "', '";
                Mi_SQL_Alta = Mi_SQL_Alta + Datos.P_Programa_ID + "', '";
                Mi_SQL_Alta = Mi_SQL_Alta + Datos.P_Partida_ID + "', '";
                Mi_SQL_Alta = Mi_SQL_Alta + Datos.P_Fuente_Financiamiento_ID + "', '";
                Mi_SQL_Alta = Mi_SQL_Alta + Datos.P_Comentarios + "', ";
                Mi_SQL_Alta = Mi_SQL_Alta + Datos.P_Anio + ", ";
                Mi_SQL_Alta = Mi_SQL_Alta + Datos.P_Numero_Asignacion + ", ";
                Mi_SQL_Alta = Mi_SQL_Alta + Datos.P_Monto_Presupuestal + ", ";
                Mi_SQL_Alta = Mi_SQL_Alta + Datos.P_Comprometido + ", ";
                Mi_SQL_Alta = Mi_SQL_Alta + Datos.P_Disponible + ", ";
                Mi_SQL_Alta = Mi_SQL_Alta + Datos.P_Ejercido + ", SYSDATE, '";
                Mi_SQL_Alta = Mi_SQL_Alta + Datos.P_Nombre_Usuario + "', SYSDATE)";

                try
                {
                    //Si Mi_SQL_Modifica no está vacío, ejecutar las dos consultas
                    //if (Mi_SQL_Modifica != "")
                    //{
                    //    Comando_SQL.CommandText = Mi_SQL_Modifica;
                    //    Comando_SQL.ExecuteNonQuery();              //Ejecutar consulta para actualizar Montos 

                        Comando_SQL.CommandText = Mi_SQL_Alta;
                        Comando_SQL.ExecuteNonQuery();              //Ejecutar consulta de alta en Dep_Presupuesto

                        Transaccion_SQL.Commit();                   //Enviar los cambios a la BD
                    //}
                }
                catch (OracleException Ex)
                {
                    if (Transaccion_SQL != null)        // Si el envío de la consulta SQL es nulo, hacer rollback de los datos
                    {
                        Transaccion_SQL.Rollback();
                    }
                    throw new Exception("Error Alta_Dep_Presupuesto: " + Ex.Message);
                }
                catch (DBConcurrencyException Ex)
                {
                    if (Transaccion_SQL != null)        // Si el envío de la consulta SQL es nulo, hacer rollback de los datos
                    {
                        Transaccion_SQL.Rollback();
                    }
                    throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
                }
                catch (Exception Ex)
                {
                    if (Transaccion_SQL != null)        // Si el envío de la consulta SQL es nulo, hacer rollback de los datos
                    {
                        Transaccion_SQL.Rollback();
                    }
                    throw new Exception("Error Alta_Dep_Presupuesto: " + Ex.Message);
                }

            }            
            catch (OracleException Ex)
            {
                throw new Exception("Error Alta_Dep_Presupuesto: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error Alta_Dep_Presupuesto: " + Ex.Message);
            }
            finally
            {
                Conexion_Base.Close();
            }
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Modificar_Dep_Presupuesto
        /// 	DESCRIPCIÓN: Modifica los datos de un presupuesto con lo que introdujo el usuario, regresar el 
        /// 	        monto presupuestal a su respectiva partida y actualizar con el nuevo valor en una transacción
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio con los datos que serán modificados en la base de datos
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 01-mar-2011
        /// 	MODIFICÓ: Roberto González Oseguera
        /// 	FECHA_MODIFICÓ: 28-abr-2011
        /// 	CAUSA_MODIFICACIÓN: Se comento el codigo que regresa presupuesto a la partida y valida presupuesto
        ///*******************************************************************************************************
        public static void Modificar_Dep_Presupuesto(Cls_Ope_SAP_Dep_Presupuesto_Negocio Datos)
        {
            String Mi_SQL;      //Obtiene la cadena de modificación hacía la base de datos
            //String Mi_SQL_Montos = ""; //Contiene la consulta de modificación hacía la base de datos
            //DataTable Dt_Pres_Partida;
            //DataTable Dt_Presupuesto;
            //Double Monto_Disponible_Presupuesto_Partida = 0.0;
            //Double Monto_Comprometido_Presupuesto_Partida = 0.0;
            //Double Monto_Presupuesto_Dep = Convert.ToDouble(Datos.P_Monto_Presupuestal);
            //Double Monto_Presupuesto_Anterior;
            //Variable para transacción 
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion);
            OracleCommand Comando_SQL = new OracleCommand();
            OracleTransaction Transaccion_SQL;

            // Si no hay una conexión hacia la bd, abrirla
            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open();
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);
            Comando_SQL.Connection = Conexion_Base;
            Comando_SQL.Transaction = Transaccion_SQL;

            //    // Consultar el presupuesto de la Partida
            //Dt_Pres_Partida = Consulta_Ope_Pres_Partidas(Datos);
            //// verificar que se obtuvieron datos
            //if (Dt_Pres_Partida.Rows.Count > 0)
            //{
            //        //Asignar valor a las variables con Montos Disponible y Comprometido
            //    Monto_Disponible_Presupuesto_Partida = Convert.ToDouble(Dt_Pres_Partida.Rows[0][Ope_Com_Pres_Partida.Campo_Monto_Disponible].ToString());
            //    Monto_Comprometido_Presupuesto_Partida = Convert.ToDouble(Dt_Pres_Partida.Rows[0][Ope_Com_Pres_Partida.Campo_Monto_Comprometido].ToString());
                
            //    //sumar el monto anterior del presupuesto al monto Disponible  y restar del monto Comprometido
            //    Dt_Presupuesto = Consulta_Datos_Presupuestos(Datos);    //Consultar el Presupuesto para obtener el monto Presupuestal anterior
            //    if (Dt_Presupuesto.Rows.Count > 0)
            //    {
            //        // Sumar el monto anterior al Disponible y restarlo del Comprometido
            //        Monto_Presupuesto_Anterior = Convert.ToDouble(Dt_Presupuesto.Rows[0][Cat_Com_Dep_Presupuesto.Campo_Monto_Presupuestal].ToString());
            //        Monto_Disponible_Presupuesto_Partida = Monto_Disponible_Presupuesto_Partida + Monto_Presupuesto_Anterior;
            //        Monto_Comprometido_Presupuesto_Partida = Monto_Comprometido_Presupuesto_Partida - Monto_Presupuesto_Anterior;
            //    }

            //    //Verificar que el monto asignado es menor que el monto disponible
            //    if ((Monto_Disponible_Presupuesto_Partida - Monto_Presupuesto_Dep) >= 0)
            //    {
            //        //Obtener el nuevo monto Disponible del Presupuesto_Partida restando el monto presupuestal. Sumar el monto presupuestal al comprometido
            //        Monto_Disponible_Presupuesto_Partida = Monto_Disponible_Presupuesto_Partida - Monto_Presupuesto_Dep;
            //        Monto_Comprometido_Presupuesto_Partida = Monto_Comprometido_Presupuesto_Partida + Monto_Presupuesto_Dep;
            //        //Formar la consulta que actualiza los montos disponible y comprometido de Presupuesto_Partida
            //        Mi_SQL_Montos = "UPDATE " + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + " SET ";
            //        Mi_SQL_Montos = Mi_SQL_Montos + Ope_Com_Pres_Partida.Campo_Monto_Disponible + " = '" + Monto_Disponible_Presupuesto_Partida + "', ";
            //        Mi_SQL_Montos = Mi_SQL_Montos + Ope_Com_Pres_Partida.Campo_Monto_Comprometido + " = '" + Monto_Comprometido_Presupuesto_Partida + "', ";
            //        Mi_SQL_Montos = Mi_SQL_Montos + Ope_Com_Pres_Partida.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
            //        Mi_SQL_Montos = Mi_SQL_Montos + Ope_Com_Pres_Partida.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
            //        Mi_SQL_Montos = Mi_SQL_Montos + Ope_Com_Pres_Partida.Campo_Presupuesto_Partida_ID + " = '" + Dt_Pres_Partida.Rows[0][Ope_Com_Pres_Partida.Campo_Presupuesto_Partida_ID] + "'";
            //    }
            //}

            //Consulta para actualizar la fuente de fin. con los datos proporcionados por el usuario
            Mi_SQL = "UPDATE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + " SET ";
            Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "', ";
            Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID + " = '" + Datos.P_Fuente_Financiamiento_ID + "', ";
            Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + " = '" + Datos.P_Programa_ID + "', ";
            Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Partida_ID + " = '" + Datos.P_Partida_ID + "', ";
            Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + " = '" + Datos.P_Anio + "', ";
            Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + " = '" + Datos.P_Numero_Asignacion + "', ";
            Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Presupuestal + " = '" + Datos.P_Monto_Presupuestal + "', ";
            Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Ejercido + " = '" + Datos.P_Ejercido + "', ";
            Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + " = '" + Datos.P_Comprometido + "', ";
            Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + " = '" + Datos.P_Disponible + "', ";
            Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
            Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
            Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
            Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Presupuesto_ID + " = '" + Datos.P_Presupuesto_ID + "'";

            try
            {
                //Si Mi_SQL_Modifica no está vacío, ejecutar las dos consultas
                //if (Mi_SQL_Montos != "")
                //{
                    //Comando_SQL.CommandText = Mi_SQL_Montos;
                    //Comando_SQL.ExecuteNonQuery();              //Ejecutar consulta para actualizar Montos 

                    Comando_SQL.CommandText = Mi_SQL;
                    Comando_SQL.ExecuteNonQuery();              //Ejecutar consulta de alta en Dep_Presupuesto

                    Transaccion_SQL.Commit();                   //Enviar los cambios a la BD
                //}
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)        // Si el envío de la consulta SQL es nulo, hacer rollback de los datos
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error Actualizacion_Dep_Presupuesto: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null)        // Si el envío de la consulta SQL es nulo, hacer rollback de los datos
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)        // Si el envío de la consulta SQL es nulo, hacer rollback de los datos
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error Actualizacion_Dep_Presupuesto: " + Ex.Message);
            }
            finally
            {
                Conexion_Base.Close();
            }
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Modificar_Presupuestos_Partidas
        /// 	DESCRIPCIÓN: 1. Actualiza el monto disponible y comprometido de Ope_SAP_Pres_Partidas
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio con los datos para actualizar la base de datos
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 05-mar-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Modificar_Presupuestos_Partidas(Cls_Ope_SAP_Dep_Presupuesto_Negocio Datos)
        {
            String Mi_SQL; //Contiene la consulta de modificación hacía la base de datos
            DataTable Dt_Pres_Partida;
            DataTable Dt_Presupuesto;
            Double Monto_Disponible_Presupuesto_Partida = 0.0;
            Double Monto_Comprometido_Presupuesto_Partida = 0.0;
            Double Monto_Presupuesto_Dep = Convert.ToDouble(Datos.P_Monto_Presupuestal);
            Double Monto_Presupuesto_Anterior;

            Dt_Pres_Partida = Consulta_Ope_Pres_Partidas(Datos);
                    // verificar que se obtuvieron datos
            if (Dt_Pres_Partida.Rows.Count > 0)
            {
                Monto_Disponible_Presupuesto_Partida = Convert.ToDouble(Dt_Pres_Partida.Rows[0][Ope_Com_Pres_Partida.Campo_Monto_Disponible].ToString());
                Monto_Comprometido_Presupuesto_Partida = Convert.ToDouble(Dt_Pres_Partida.Rows[0][Ope_Com_Pres_Partida.Campo_Monto_Comprometido].ToString());

                if (Datos.P_Presupuesto_ID > 0)     // si hay un dato en P_Presupuesto_ID, es una modificación
                {       //sumar el monto anterior del presupuesto al monto Disponible  y restar del monto Comprometido
                    Dt_Presupuesto = Consulta_Datos_Presupuestos(Datos);    //Consultar el Presupuesto para obtener el monto Presupuestal anterior
                    if (Dt_Presupuesto.Rows.Count > 0)
                    {
                        Monto_Presupuesto_Anterior = Convert.ToDouble(Dt_Presupuesto.Rows[0][Cat_Com_Dep_Presupuesto.Campo_Monto_Presupuestal].ToString());
                        Monto_Disponible_Presupuesto_Partida = Monto_Disponible_Presupuesto_Partida + Monto_Presupuesto_Anterior;
                        Monto_Comprometido_Presupuesto_Partida = Monto_Comprometido_Presupuesto_Partida - Monto_Presupuesto_Anterior;
                    }
                }
                //Verificar que el monto asignado es menor que el monto disponible
                if ((Monto_Disponible_Presupuesto_Partida - Monto_Presupuesto_Dep) >= 0)
                {
                    //Afectar el presupuesto
                    Monto_Disponible_Presupuesto_Partida = Monto_Disponible_Presupuesto_Partida - Monto_Presupuesto_Dep;
                    Monto_Comprometido_Presupuesto_Partida = Monto_Comprometido_Presupuesto_Partida + Monto_Presupuesto_Dep;
                    //Formar y ejecutar la consulta, regresando el resultado de la misma
                    Mi_SQL = "UPDATE " + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida + " SET ";
                    Mi_SQL = Mi_SQL + Ope_Com_Pres_Partida.Campo_Monto_Disponible + " = '" + Monto_Disponible_Presupuesto_Partida + "', ";
                    Mi_SQL = Mi_SQL + Ope_Com_Pres_Partida.Campo_Monto_Comprometido + " = '" + Monto_Comprometido_Presupuesto_Partida + "', ";
                    Mi_SQL = Mi_SQL + Ope_Com_Pres_Partida.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                    Mi_SQL = Mi_SQL + Ope_Com_Pres_Partida.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                    Mi_SQL = Mi_SQL + Ope_Com_Pres_Partida.Campo_Presupuesto_Partida_ID + " = '" + Dt_Pres_Partida.Rows[0][Ope_Com_Pres_Partida.Campo_Presupuesto_Partida_ID] + "'";
                    return OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
            }

            return 0;
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Numero_Asignacion_Presupuesto_Anio
        /// 	DESCRIPCIÓN: Consulta el último número de asignación para un Año de una Partida de un Programa de una Dependencia
        /// 	            Si alguno de los datos no se encuantra, regresa -1, si no encuentra datos que cumplan
        /// 	            el criterio, regresa 1, y si encuentra un valor, regresa ese valor incrementado en uno
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio con los datos que serán insertados en la base de datos
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 04-mar-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Int32 Consulta_Numero_Asignacion_Presupuesto_Anio(Cls_Ope_SAP_Dep_Presupuesto_Negocio Datos)
        {
            String Mi_SQL;  //Obtiene la cadena de inserción hacía la base de datos
            Object Indice_Presupuesto;

            if (Datos.P_Anio != null && Datos.P_Partida_ID != null && Datos.P_Programa_ID != null && Datos.P_Dependencia_ID != null)
            {
                try
                {
                    Mi_SQL = "SELECT MAX(" + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ") ";
                    Mi_SQL = Mi_SQL + "FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + " = " + Datos.P_Anio;
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID + " = " + Datos.P_Partida_ID;
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + " = " + Datos.P_Programa_ID;
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + " = " + Datos.P_Dependencia_ID;
                    Indice_Presupuesto = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    if (Convert.IsDBNull(Indice_Presupuesto))
                    {
                        return 1;
                    }
                    else
                    {
                        return Convert.ToInt32(Indice_Presupuesto) + 1;
                    }
                }
                catch (OracleException Ex)
                {
                    throw new Exception("Error: " + Ex.Message);
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error: " + Ex.Message);
                }
            }
            return -1;
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Datos_Presupuestos
        /// 	DESCRIPCIÓN: Consulta todos los campos de las Partidas en la tabla de la clase Cat_Com_Dep_Presupuesto
        /// 	    de la BD
        /// 	PARÁMETROS:
        /// 		1. Datos: Indica qué registro se desea consultar a la base de datos
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 04-mar-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Datos_Presupuestos(Cls_Ope_SAP_Dep_Presupuesto_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL
            String Filtro_SQL = ""; //Variable para el filtro de la consulta SQL

            try
            {
                Mi_SQL = "SELECT " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + ".* , ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Tabla_Cat_Com_Partidas + "." + Cat_Com_Partidas.Campo_Nombre + " As Nombre_Partida, ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Tabla_Cat_Com_Partidas + "." + Cat_Com_Partidas.Campo_Clave + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Tabla_Cat_Com_Partidas + "." + Cat_Com_Partidas.Campo_Clave + " || ' ' || ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Tabla_Cat_Com_Partidas + "." + Cat_Com_Partidas.Campo_Nombre + " As Clave_Y_Nombre";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + ", " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas;
                Filtro_SQL =  " WHERE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Partida_ID;
                Filtro_SQL = Filtro_SQL + " = " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas + "." + Cat_Com_Partidas.Campo_Partida_ID;

                if (Datos.P_Busqueda == null)  //Si P_Busqueda no fue asignada, filtrar por IDs
                {
                    if (Datos.P_Presupuesto_ID > 0)
                    {
                        Filtro_SQL = Filtro_SQL + " AND " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + ".";
                        Filtro_SQL = Filtro_SQL + Cat_Com_Dep_Presupuesto.Campo_Presupuesto_ID + " = " + Datos.P_Presupuesto_ID;
                    }
                    if (Datos.P_Dependencia_ID != null)
                    {
                        Filtro_SQL = Filtro_SQL + " AND " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + ".";
                        Filtro_SQL = Filtro_SQL + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "'";
                    }
                    if (Datos.P_Partida_ID != null)
                    {
                        Filtro_SQL = Filtro_SQL + " AND " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + ".";
                        Filtro_SQL = Filtro_SQL + Cat_Com_Dep_Presupuesto.Campo_Partida_ID + " = '" + Datos.P_Partida_ID + "'";
                    }
                    if (Datos.P_Anio != null)
                    {
                        Filtro_SQL = Filtro_SQL + " AND " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + ".";
                        Filtro_SQL = Filtro_SQL + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + " = '" + Datos.P_Anio + "'";
                    }
                    if (Datos.P_Programa_ID != null)
                    {
                        Filtro_SQL = Filtro_SQL + " AND " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + ".";
                        Filtro_SQL = Filtro_SQL + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + " = '" + Datos.P_Programa_ID + "'";
                    }
                    if (Datos.P_Comentarios != null)
                    {
                        Filtro_SQL = Filtro_SQL + " OR " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + ".";
                        Filtro_SQL = Filtro_SQL + Cat_Com_Dep_Presupuesto.Campo_Comentarios + " LIKE '%" + Datos.P_Comentarios + "%'";
                    }
                }
                else   //Si P_Busqueda fue asignada, buscar en clave, nombre de partida y en Comentarios de presupuestos
                {
                    if (Datos.P_Dependencia_ID != null)
                    {
                        Filtro_SQL = Filtro_SQL + " AND " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + ".";
                        Filtro_SQL = Filtro_SQL + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "'";
                    }
                    Filtro_SQL = Filtro_SQL + " AND (UPPER(" + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + ".";
                    Filtro_SQL = Filtro_SQL + Cat_Sap_Partidas_Especificas.Campo_Nombre + ") LIKE UPPER('%";
                    Filtro_SQL = Filtro_SQL + Datos.P_Busqueda + "%') OR " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + ".";
                    Filtro_SQL = Filtro_SQL + Cat_Sap_Partidas_Especificas.Campo_Clave + " LIKE '%" + Datos.P_Busqueda + "%' OR UPPER(";
                    Filtro_SQL = Filtro_SQL + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + ".";
                    Filtro_SQL = Filtro_SQL + Cat_Com_Dep_Presupuesto.Campo_Comentarios + ") LIKE UPPER('%" + Datos.P_Busqueda + "%'))";
                }
                Mi_SQL = Mi_SQL + Filtro_SQL + " ORDER BY " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto;
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
        }   //FUNCIÓN: Consulta_Datos_Partidas

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Programa_Fuente_ID
        /// 	DESCRIPCIÓN: Consulta las Fuentes de Financiamiento ligadas al Programa
        /// 	PARÁMETROS:  Datos: Indica qué registro se desea consultar a la base de datos
        /// 	CREO: Salvador L. Rea Ayala
        /// 	FECHA_CREO: 6/Octubre/2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Programa_Fuente_ID(Cls_Ope_SAP_Dep_Presupuesto_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "SELECT DISTINCT " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID;
                Mi_SQL += " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                Mi_SQL += " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + " = '" + Datos.P_Programa_ID + "'";
                Mi_SQL += " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "'";
                Mi_SQL += " ORDER BY " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID + " ASC";

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
        }
        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Fte_Area_Funcional_ID
        /// 	DESCRIPCIÓN: Consulta las Areas Funcionales ligadas a la Fuente de Financiamiento
        /// 	PARÁMETROS:  Datos: Indica qué registro se desea consultar a la base de datos
        /// 	CREO: Salvador L. Rea Ayala
        /// 	FECHA_CREO: 6/Octubre/2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Fte_Area_Funcional_ID(Cls_Ope_SAP_Dep_Presupuesto_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "SELECT DISTINCT " + Cat_Com_Dep_Presupuesto.Campo_Area_Funcional_ID;
                Mi_SQL += " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                Mi_SQL += " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID + " = '" + Datos.P_Fuente_Financiamiento_ID + "'";
                Mi_SQL += " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "'";
                Mi_SQL += " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + " = '" + Datos.P_Programa_ID + "'";
                Mi_SQL += " ORDER BY " + Cat_Com_Dep_Presupuesto.Campo_Area_Funcional_ID + " ASC";

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
        }
        ///**********************************************************************************************************************************
        ///                                                                TABLAS_EXTERNAS
        ///**********************************************************************************************************************************
#region (TABLAS_EXTERNAS)
        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Dependencias
        /// 	DESCRIPCIÓN: Consulta todas las Dependencias en la BD (CAT_DEPENDENCIAS)
        /// 	PARÁMETROS:
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 04-mar-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Dependencias()
        {
            String Mi_SQL; //Variable para la consulta de las Partidas Genericas

            try
            {
                //Formar consulta (campos a traer de la tabla)
                Mi_SQL = "SELECT " + Cat_Dependencias.Campo_Dependencia_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Nombre + ", ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Comentarios;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias;
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Dependencias.Campo_Nombre;
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

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Fuente_Financiamiento
        /// 	DESCRIPCIÓN: Consulta las fuentes de financiamieno filtradas por departamento
        /// 	PARÁMETROS:
        /// 	        1. Instancia de la clase de negocio con datos para filtrar campos
        /// 	FECHA_CREO: 04-mar-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Fuente_Financiamiento(Cls_Ope_SAP_Dep_Presupuesto_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de las fuentes de financiamiento

            try
            {
                //Formar consulta (campos a traer de la tabla)
                Mi_SQL = "SELECT " + Cat_SAP_Det_Fte_Dependencia.Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia + "." + Cat_SAP_Det_Fte_Dependencia.Campo_Dependencia_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Det_Fte_Dependencia.Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia + "." + Cat_SAP_Det_Fte_Dependencia.Campo_Fuente_Financiamiento_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Clave + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " || ' ' || " + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + " As Clave_Y_Descripcion ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Det_Fte_Dependencia.Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_SAP_Det_Fte_Dependencia.Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia + "." + Cat_SAP_Det_Fte_Dependencia.Campo_Fuente_Financiamiento_ID + " = ";
                Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID;
                if (Datos.P_Dependencia_ID != null)
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_SAP_Det_Fte_Dependencia.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "'";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_SAP_Fuente_Financiamiento.Campo_Clave;
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

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Programas
        /// 	DESCRIPCIÓN: Consulta los Programas filtradas por departamento
        /// 	PARÁMETROS:
        /// 	        1. Instancia de la clase de negocio con datos para filtrar campos
        /// 	FECHA_CREO: 04-mar-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Programas(Cls_Ope_SAP_Dep_Presupuesto_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de las fuentes de financiamiento

            try
            {
                //Formar consulta (campos a traer de la tabla)
                Mi_SQL = "SELECT " + Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia + "." + Cat_SAP_Det_Prog_Dependencia.Campo_Dependencia_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia + "." + Cat_SAP_Det_Prog_Dependencia.Campo_Proyecto_Programa_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + "." + Cat_Com_Proyectos_Programas.Campo_Clave + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + "." + Cat_Com_Proyectos_Programas.Campo_Nombre + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Clave + " || ' ' || " + Cat_Com_Proyectos_Programas.Campo_Nombre + " As Clave_Y_Nombre ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia + "." + Cat_SAP_Det_Prog_Dependencia.Campo_Proyecto_Programa_ID + " = ";
                Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + "." + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID;
                if (Datos.P_Dependencia_ID != null)
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_SAP_Det_Prog_Dependencia.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "'";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Proyectos_Programas.Campo_Clave;
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

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Partidas
        /// 	DESCRIPCIÓN: Consulta las Partidas filtradas por Programa
        /// 	PARÁMETROS:
        /// 	        1. Instancia de la clase de negocio con datos para filtrar campos
        /// 	FECHA_CREO: 04-mar-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Partidas(Cls_Ope_SAP_Dep_Presupuesto_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de las fuentes de financiamiento

            try
            {
                //Formar consulta (campos a traer de la tabla)
                Mi_SQL = "SELECT " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + "." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Partida_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + "." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Proyecto_Programa_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Tabla_Cat_Com_Partidas + "." + Cat_Com_Partidas.Campo_Nombre + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Tabla_Cat_Com_Partidas + "." + Cat_Com_Partidas.Campo_Clave + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Clave + " || ' ' || " + Cat_Com_Partidas.Campo_Nombre + " As Clave_Y_Nombre ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Tabla_Cat_Com_Partidas;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + "." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Partida_ID + " = ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Tabla_Cat_Com_Partidas + "." + Cat_Com_Partidas.Campo_Partida_ID;
                if (Datos.P_Programa_ID != null)
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + ".";
                    Mi_SQL = Mi_SQL + Cat_Sap_Det_Prog_Partidas.Campo_Det_Proyecto_Programa_ID + " = '" + Datos.P_Programa_ID + "'";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Partidas.Campo_Clave;
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

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Ope_Pres_Partidas
        /// 	DESCRIPCIÓN: Consulta los datos (MONTO_PRESUPUESTAL y MONTO_DISPONIBLE) de un Año, Partida y Programa 
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio con los datos que serán insertados en la base de datos
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 05-mar-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Ope_Pres_Partidas(Cls_Ope_SAP_Dep_Presupuesto_Negocio Datos)
        {
            String Mi_SQL;  //Contiene la consulta para la base de datos
            DataTable Tabla_Vacia = new DataTable();

            if (Datos.P_Anio != null && Datos.P_Partida_ID != null && Datos.P_Programa_ID != null)
            {
                try
                {
                    Mi_SQL = "SELECT * FROM " + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Pres_Partida.Campo_Anio_Presupuesto + " = " + Datos.P_Anio;
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Pres_Partida.Campo_Partida_ID + " = '" + Datos.P_Partida_ID;
                    Mi_SQL = Mi_SQL + "' AND " + Ope_Com_Pres_Partida.Campo_Proyecto_Programa_ID + " = '" + Datos.P_Programa_ID + "'";
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                }
                catch (OracleException Ex)
                {
                    throw new Exception("Error: " + Ex.Message);
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error: " + Ex.Message);
                }
            }
            // Si la cosulta no 
            return Tabla_Vacia;
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_IDs_De_Claves
        /// 	DESCRIPCIÓN: Consulta los IDs de las tablas Fuente de Financiamiento, Programa y Partida, de 
        /// 	            las claves dadas.
        /// 	PARÁMETROS:
        /// 		1. Datos: DataTable con la lista de claves a obtener
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 08-abr-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_IDs_De_Claves(DataTable Datos, out String Mensaje)
        {
            String Mi_SQL = "";  //Contiene la consulta para la base de datos
            DataTable Tabla_Resultados = new DataTable();
            DataRow Nueva_Fila = Datos.NewRow();
            DataTable Nueva_Tabla = new DataTable();
            Object ID_De_Clave = "";
            Mensaje = "";
            
            try
            {
                foreach (DataRow Fila in Datos.Rows)
                {
                    Mi_SQL = "SELECT " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + ".";
                    Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + ", ";
                    Mi_SQL = Mi_SQL + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + ".";
                    Mi_SQL = Mi_SQL + Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + ".";
                    Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + ", ";
                    Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + ".";
                    Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Dependencia_ID + ", ";
                    Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + ".";
                    Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Campo_Partida_ID;
                    Mi_SQL = Mi_SQL + " FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + ", ";
                    Mi_SQL = Mi_SQL + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + ", ";
                    Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + ", ";
                    Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + ".";
                    Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " = '" + Fila[1] + "'";
                    Mi_SQL = Mi_SQL + " AND " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + ".";
                    Mi_SQL = Mi_SQL + Cat_SAP_Area_Funcional.Campo_Clave + " = '" + Fila[2] + "'";
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + ".";
                    Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Clave + " = '" + Fila[3] + "'";
                    Mi_SQL = Mi_SQL + " AND " + Cat_Dependencias.Tabla_Cat_Dependencias + ".";
                    Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Clave + " = '" + Fila[4] + "'";
                    Mi_SQL = Mi_SQL + " AND " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + ".";
                    Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Campo_Clave + " = '" + Fila[5] + "'";

                    Nueva_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    if (Nueva_Tabla.Rows.Count > 0)   //Si hubo resultados
                    {
                        Fila[12] = Nueva_Tabla.Rows[0][0];   // Asignar ID de fuente de financiamiento
                        Fila[13] = Nueva_Tabla.Rows[0][1];   // Asignar ID de Area funcional
                        Fila[14] = Nueva_Tabla.Rows[0][2];   // Asignar ID de Programa ID
                        Fila[15] = Nueva_Tabla.Rows[0][3];   // Asignar ID de la Dependencia
                        Fila[16] = Nueva_Tabla.Rows[0][4];   // Asignar ID de Partida
                    }
                    else
                    {
                        // Consultar Fuente de financiamiento por separado
                        Mi_SQL = "SELECT " + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID;
                        Mi_SQL = Mi_SQL + " FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento;
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " = '" + Fila[1] + "'";
                        ID_De_Clave = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        if (Convert.IsDBNull(ID_De_Clave) || ID_De_Clave == null)   //Si no se encontro el ID, 
                        {
                            Mensaje += "La clave " + Fila[1] + " (fila " + Fila[0] + ") no se encontró en el catálogo de Fuentes de financiamiento. <br />";
                        }
                        else
                        {
                            Fila[12] = ID_De_Clave;
                        }

                        // Consultar Area funcional por separado
                        Mi_SQL = "SELECT " + Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID;
                        Mi_SQL = Mi_SQL + " FROM " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional;
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_SAP_Area_Funcional.Campo_Clave + " = '" + Fila[2] + "'";
                        ID_De_Clave = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        if (Convert.IsDBNull(ID_De_Clave) || ID_De_Clave == null)   //Si no se encontro el ID, agregar mensaje de error
                        {
                            Mensaje += "La clave " + Fila[2] + " (fila " + Fila[0] + ") no se encontró en el catálogo de Áreas funcionales.<br />";
                        }
                        else
                        {
                            Fila[13] = ID_De_Clave;
                        }

                        // Consultar Programas
                        Mi_SQL = "SELECT " + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID;
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas;
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Proyectos_Programas.Campo_Clave + " = '" + Fila[3] + "'";
                        ID_De_Clave = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        if (Convert.IsDBNull(ID_De_Clave) || ID_De_Clave == null)   //Si no se encontro el ID, agregar mensaje de error
                        {
                            Mensaje += "La clave " + Fila[3] + " (fila " + Fila[0] + ") no se encontró en el catálogo de Programas.<br />";
                        }
                        else
                        {
                            Fila[14] = ID_De_Clave;
                        }

                        // Consultar Dependencia
                        Mi_SQL = "SELECT " + Cat_Dependencias.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias;
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Dependencias.Campo_Clave + " = '" + Fila[4] + "'";
                        ID_De_Clave = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        if (Convert.IsDBNull(ID_De_Clave) || ID_De_Clave == null)   //Si no se encontro el ID, agregar mensaje de error
                        {
                            Mensaje += "La clave " + Fila[4] + " (fila " + Fila[0] + ") no se encontró en el catálogo de Unidades responsables.<br />";
                        }
                        else
                        {
                            Fila[15] = ID_De_Clave;
                        }

                        // Consultar Partidas especificas
                        Mi_SQL = "SELECT " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID;
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas;
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Partidas_Especificas.Campo_Clave + " = '" + Fila[5] + "'";
                        ID_De_Clave = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        if (Convert.IsDBNull(ID_De_Clave) || ID_De_Clave == null)   //Si no se encontro el ID, agregar mensaje de error
                        {
                            Mensaje += "La clave " + Fila[5] + " (fila " + Fila[0] + ") no se encontró en el catálogo de Partidas específicas.<br />";
                        }
                        else
                        {
                            Fila[16] = ID_De_Clave;
                        }
                    }
                }
                return Datos;
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Sincronizar_Datos
        /// 	DESCRIPCIÓN: Agrega los datos proporcionados en el datatable a la base de datos 
        /// 	            Regresa un arreglo de enteros con los registros insertados y los registros actualizados
        /// 	PARÁMETROS:
        /// 		1. Datos: DataTable con los presupuestos a dar de alta
        /// 		2. Datos_Actualizar: Datatable con los datos de presupuestos a actualizar
        /// 		3. Ruta_Archivo: Especifica la ruta en el servidor del archivo de sincronizacion utilizado
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 12-abr-2011
        /// 	MODIFICÓ: Roberto González Oseguera
        /// 	FECHA_MODIFICÓ: 29-abr-2011
        /// 	CAUSA_MODIFICACIÓN: Se comentaron las lineas de codigo que actualizan los montos de la 
        /// 	        partida al agregar presupuestos y que determinan un nuevo numero de asignacion por anio.
        /// 	        Se agrego un datatable como parametro para los presupuestos que se actualizaran en la 
        /// 	        sincronizacion
        ///*******************************************************************************************************
        public static int[] Sincronizar_Datos(DataTable Datos, DataTable Datos_Actualizar, Cls_Ope_SAP_Dep_Presupuesto_Negocio Datos_Presupuesto)
        {
            String Mi_SQL;  //Contiene la consulta para la base de datos
            String Insertar_SQL;
            int Contador_IDs;
            int[] Contador_Afectaciones = { 0, 0 };
            Object Presupuesto_ID;          //Obtiene el ID con el que se guardara el nuevo registro en la base de datos
            //Object Indice_Presupuesto;
            Object No_Sincronizacion;
            DataTable Tabla_Vacia = new DataTable();
            DataTable Tabla_Raultados = new DataTable();
            List<String> Consultas_SQL = new List<String>();
            //Variable para transacción 
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion);
            OracleCommand Comando_SQL = new OracleCommand();
            OracleTransaction Transaccion_SQL;

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open();
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);
            Comando_SQL.Connection = Conexion_Base;
            Comando_SQL.Transaction = Transaccion_SQL;

            try
            {
                // Obtener el ID del último registro insertado y sumar 1 (ID del Dep_Presupuesto a insertar)
                Mi_SQL = "SELECT MAX(" + Cat_Com_Dep_Presupuesto.Campo_Presupuesto_ID + ") ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                Presupuesto_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Convert.IsDBNull(Presupuesto_ID))
                {
                    Contador_IDs = 1;
                }
                else
                {
                    Contador_IDs = Convert.ToInt32(Presupuesto_ID) + 1;
                }

                // Obtener el ID del último registro insertado en Ope_Com_Sincronizaciones_Presupuesto
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Com_Sincronizaciones_Presupuesto.Campo_No_Sincronizacion + "),'0000000000') ";
                Mi_SQL += "FROM " + Ope_Com_Sincronizaciones_Presupuesto.Tabla_Ope_Com_Sincronizaciones_Presupuesto;
                No_Sincronizacion = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(No_Sincronizacion))
                {
                    No_Sincronizacion = "0000000001";
                }
                else
                {
                    No_Sincronizacion = String.Format("{0:0000000000}", Convert.ToInt32(No_Sincronizacion) + 1);
                }

                try
                {
                    foreach (DataRow Fila in Datos.Rows)
                    {
                        ////Obtener el numero de asignacion del presupuesto por anio
                        //Mi_SQL = "SELECT MAX(" + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ") ";
                        //Mi_SQL = Mi_SQL + "FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                        //Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + " = " + Fila["ANIO"];
                        //Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID + " = " + Fila["PARTIDA"];
                        //Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + " = " + Fila["PROGRAMA"];
                        //Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + " = " + Fila["UNIDAD_RESPONSABLE"];
                        //Indice_Presupuesto = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                        //if (Convert.IsDBNull(Indice_Presupuesto))
                        //{
                        //    Indice_Presupuesto = 1;
                        //}
                        //else
                        //{
                        //    Indice_Presupuesto = Convert.ToInt32(Indice_Presupuesto) + 1;
                        //}
                        //Consulta para la inserción de la Partida con los datos proporcionados por el usuario en Ope_SAP_Dep_Presupuesto
                        Insertar_SQL = "INSERT INTO " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + " (";
                        Insertar_SQL = Insertar_SQL + Cat_Com_Dep_Presupuesto.Campo_Presupuesto_ID + ", ";
                        Insertar_SQL = Insertar_SQL + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + ", ";
                        Insertar_SQL = Insertar_SQL + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + ", ";
                        Insertar_SQL = Insertar_SQL + Cat_Com_Dep_Presupuesto.Campo_Partida_ID + ", ";
                        Insertar_SQL = Insertar_SQL + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID + ", ";
                        Insertar_SQL = Insertar_SQL + Cat_Com_Dep_Presupuesto.Campo_Comentarios + ", ";
                        Insertar_SQL = Insertar_SQL + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + ", ";
                        Insertar_SQL = Insertar_SQL + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ", ";
                        Insertar_SQL = Insertar_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Presupuestal + ", ";
                        Insertar_SQL = Insertar_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + ", ";
                        Insertar_SQL = Insertar_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + ", ";
                        Insertar_SQL = Insertar_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Ejercido + ", ";
                        Insertar_SQL = Insertar_SQL + Cat_Com_Dep_Presupuesto.Campo_Fecha_Asignacion + ", ";
                        Insertar_SQL = Insertar_SQL + Cat_Com_Dep_Presupuesto.Campo_Usuario_Creo + ", ";
                        Insertar_SQL = Insertar_SQL + Cat_Com_Dep_Presupuesto.Campo_Fecha_Creo + ") VALUES (";
                        Insertar_SQL = Insertar_SQL + Contador_IDs++ + ", '";
                        Insertar_SQL = Insertar_SQL + Fila["UNIDAD_RESPONSABLE_ID"] + "', '";
                        Insertar_SQL = Insertar_SQL + Fila["PROGRAMA_ID"] + "', '";
                        Insertar_SQL = Insertar_SQL + Fila["PARTIDA_ID"] + "', '";
                        Insertar_SQL = Insertar_SQL + Fila["FUENTE_FINACIAMIENTO_ID"] + "', '";
                        Insertar_SQL = Insertar_SQL + Datos_Presupuesto.P_Comentarios + "', ";   //Comentario
                        Insertar_SQL = Insertar_SQL + Fila["ANIO"] + ", ";
                        //Insertar_SQL = Insertar_SQL + Indice_Presupuesto + ", "; //Se cambio por la linea que sigue debido a que ya no se hacen asignaciones por anio
                        Insertar_SQL = Insertar_SQL + 1 + ", ";
                        Insertar_SQL = Insertar_SQL + Fila["PRESUPUESTO"].ToString().Replace(",","") + ", ";
                        Insertar_SQL = Insertar_SQL + Fila["COMPROMETIDO"].ToString().Replace(",", "") + ", ";
                        Insertar_SQL = Insertar_SQL + Fila["DISPONIBLE"].ToString().Replace(",", "") + ", ";
                        Insertar_SQL = Insertar_SQL + Fila["EJERCIDO"].ToString().Replace(",", "") + ", SYSDATE, '";
                        Insertar_SQL = Insertar_SQL + Datos_Presupuesto.P_Nombre_Usuario + "', SYSDATE)";

                        Comando_SQL.CommandText = Insertar_SQL;
                        Contador_Afectaciones[0] += Comando_SQL.ExecuteNonQuery();              //Ejecutar consulta de alta en Dep_Presupuesto

                    }

                    foreach (DataRow Fila in Datos_Actualizar.Rows) //para cada fila de datos Actualizar, ejecutar consulta de actualizacion
                    {
                        Mi_SQL = "UPDATE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + " SET ";
                        Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + " = '" + Fila["ANIO"] + "', ";
                        Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Presupuestal + " = '" + Fila["PRESUPUESTO"].ToString().Replace(",", "") + "', ";
                        Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Ejercido + " = '" + Fila["EJERCIDO"].ToString().Replace(",", "") + "', ";
                        Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + " = '" + Fila["COMPROMETIDO"].ToString().Replace(",", "") + "', ";
                        Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + " = '" + Fila["DISPONIBLE"].ToString().Replace(",", "") + "', ";
                        Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Comentarios + " = '" + Datos_Presupuesto.P_Comentarios +
                            "' || '     ' || (SELECT " + Cat_Com_Dep_Presupuesto.Campo_Comentarios + " FROM " +     //Concatenar comentario
                            Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + " WHERE " +
                            Cat_Com_Dep_Presupuesto.Campo_Presupuesto_ID + " = " + Fila["DEP_PRESUPUESTO_ID"] + "), ";
                        Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Usuario_Modifico + " = '" + Datos_Presupuesto.P_Nombre_Usuario + "', ";
                        Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                        Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Presupuesto_ID + " = '" + Fila["DEP_PRESUPUESTO_ID"] + "'";

                        Comando_SQL.CommandText = Mi_SQL;
                        Contador_Afectaciones[1] += Comando_SQL.ExecuteNonQuery();              //Ejecutar consulta de alta en Dep_Presupuesto

                    }

                    //COnsulta para insertar datos de sincronizacion en Ope_Com_Sincronizaciones_Presupuesto
                    Insertar_SQL = "INSERT INTO " + Ope_Com_Sincronizaciones_Presupuesto.Tabla_Ope_Com_Sincronizaciones_Presupuesto + " (";
                    Insertar_SQL = Insertar_SQL + Ope_Com_Sincronizaciones_Presupuesto.Campo_No_Sincronizacion + ", ";
                    Insertar_SQL = Insertar_SQL + Ope_Com_Sincronizaciones_Presupuesto.Campo_Nombre_Archivo + ", ";
                    Insertar_SQL = Insertar_SQL + Ope_Com_Sincronizaciones_Presupuesto.Campo_Fecha + ", ";
                    Insertar_SQL = Insertar_SQL + Ope_Com_Sincronizaciones_Presupuesto.Campo_Usuario_Creo + ", ";
                    Insertar_SQL = Insertar_SQL + Ope_Com_Sincronizaciones_Presupuesto.Campo_Fecha_Creo + ") VALUES ('";
                    Insertar_SQL = Insertar_SQL + No_Sincronizacion + "', '";
                    Insertar_SQL = Insertar_SQL + Datos_Presupuesto.P_Ruta_Archivo + "', SYSDATE, '";
                    Insertar_SQL = Insertar_SQL + Datos_Presupuesto.P_Nombre_Usuario + "', SYSDATE)";

                    Comando_SQL.CommandText = Insertar_SQL;
                    Comando_SQL.ExecuteNonQuery();

                    Transaccion_SQL.Commit();                   //Enviar los cambios a la BD
                    return Contador_Afectaciones;
                }
                catch (OracleException Ex)
                {
                    if (Transaccion_SQL != null)        // Si el envío de la consulta SQL es nulo, hacer rollback de los datos
                    {
                        Transaccion_SQL.Rollback();
                    }
                    throw new Exception("Error Alta_Dep_Presupuesto: " + Ex.Message);
                }
                catch (DBConcurrencyException Ex)
                {
                    if (Transaccion_SQL != null)        // Si el envío de la consulta SQL es nulo, hacer rollback de los datos
                    {
                        Transaccion_SQL.Rollback();
                    }
                    throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
                }
                catch (Exception Ex)
                {
                    if (Transaccion_SQL != null)        // Si el envío de la consulta SQL es nulo, hacer rollback de los datos
                    {
                        Transaccion_SQL.Rollback();
                    }
                    throw new Exception("Error Alta_Dep_Presupuesto: " + Ex.Message);
                }
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error Alta_Dep_Presupuesto: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error Alta_Dep_Presupuesto: " + Ex.Message);
            }
            finally
            {
                Conexion_Base.Close();
            }
        }
#endregion

    }
}