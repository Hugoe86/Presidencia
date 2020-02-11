using System;
using System.Data;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Recep_Docs_Movs.Negocio;


namespace Presidencia.Operacion_Predial_Recep_Docs_Movs.Datos
{
    public class Cls_Ope_Pre_Recep_Docs_Movs_Datos
    {
	    public Cls_Ope_Pre_Recep_Docs_Movs_Datos()
	    {
	    }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Alta_Movimiento_Documento
        /// 	DESCRIPCIÓN: 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///                  2. Da de Alta el movimiento (por recepción de documentos) en la BD con los datos 
        ///                  proporcionados por el usuario
        ///                  Regresa el número de filas insertadas
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio con los datos que serán insertados en la base de datos
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 25-mar-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Alta_Movimiento(Cls_Ope_Pre_Recep_Docs_Movs_Negocio Datos)
        {
            String Mi_SQL;  //Obtiene la cadena de inserción hacía la base de datos
            Object Movimiento_ID; //Obtiene el ID con el cual se guardarán la información en la base de datos

            try
            {
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + "),'0000000000') ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs;
                Movimiento_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Movimiento_ID))
                {
                    Datos.P_No_Movimiento = "0000000001";
                }
                else
                {
                    Datos.P_No_Movimiento = String.Format("{0:0000000000}", Convert.ToInt32(Movimiento_ID) + 1);
                }
                //Consulta para la inserción del Movimiento con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + " (";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Numero_Escritura + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Fecha_Escritura + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Observaciones + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Empleado_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_No_Contrarecibo + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Fecha_Creo + ") VALUES ('";
                Mi_SQL = Mi_SQL + Datos.P_No_Movimiento + "', '";
                Mi_SQL = Mi_SQL + Datos.P_No_Recepcion_Documento + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Numero_Escritura + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Fecha_Escritura + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Cuenta_Predial_ID + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Estatus + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Observaciones + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Empleado_ID + "', '";
                Mi_SQL = Mi_SQL + Datos.P_No_Contrarecibo + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Nombre_Usuario + "', SYSDATE)";
                
                //regresar el número de inserciones realizadas
                return OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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
        /// 	NOMBRE_FUNCIÓN: Modificar_Movimiento
        /// 	DESCRIPCIÓN: Modifica los datos del Movimiento con lo que introdujo el usuario
        /// 	            Regresa el número de filas modificadas
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio con los datos que serán insertados en la base de datos
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 25-mar-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Modificar_Movimiento(Cls_Ope_Pre_Recep_Docs_Movs_Negocio Datos)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos

            try
            {
                //Consulta para actualizar los datos del Movimiento con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + " SET ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + " = '" + Datos.P_No_Movimiento + "', ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " = '" + Datos.P_No_Recepcion_Documento + "', ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Numero_Escritura + " = '" + Datos.P_Numero_Escritura + "', ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Fecha_Escritura + " = '" + Datos.P_Fecha_Escritura + "', ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "', ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Observaciones + " = '" + Datos.P_Observaciones + "', ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "', ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_No_Contrarecibo + " = '" + Datos.P_No_Contrarecibo + "', ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + " = '" + Datos.P_No_Movimiento + "'";

                //regresar el número de filas modificadas
                return OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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
        /// 	NOMBRE_FUNCIÓN: Consulta_Datos_Movimiento
        /// 	DESCRIPCIÓN: Consulta todos los campos de los Movimientos (por recepciones de documentos) en la BD (sin los de auditoría)
        /// 	PARÁMETROS:
        /// 		1. Datos: Indica qué registro se desea consultar a la base de datos
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 25-mar-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Datos_Movimiento(Cls_Ope_Pre_Recep_Docs_Movs_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL
            String Filtro_SQL = ""; //Variable para el filtro de la consulta SQL

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Numero_Escritura + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Fecha_Escritura + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Observaciones + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_No_Contrarecibo + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Empleado_ID;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs;
                if (Datos.P_No_Movimiento != null)      // Si se recibió un No de movimiento filtrar por ese ID
                {
                    Filtro_SQL = " WHERE " + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + " = '" + Datos.P_No_Movimiento + "'";
                }
                if (Datos.P_No_Recepcion_Documento != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar OR y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND " + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " = '" + Datos.P_No_Recepcion_Documento + "'";
                    else
                        Filtro_SQL = " WHERE " + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " = '" + Datos.P_No_Recepcion_Documento + "'";
                }
                if (Datos.P_Numero_Escritura != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND " + Ope_Pre_Recep_Docs_Movs.Campo_Numero_Escritura + " LIKE '" + Datos.P_Numero_Escritura + "'";
                    else
                        Filtro_SQL = " WHERE " + Ope_Pre_Recep_Docs_Movs.Campo_Numero_Escritura + " LIKE '" + Datos.P_Numero_Escritura + "'";
                }
                if (Datos.P_Cuenta_Predial_ID != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND " + Ope_Pre_Recep_Docs_Movs.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "'";
                    else
                        Filtro_SQL = " WHERE " + Ope_Pre_Recep_Docs_Movs.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "'";
                }
                if (Datos.P_No_Contrarecibo != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND " + Ope_Pre_Recep_Docs_Movs.Campo_No_Contrarecibo + " = '" + Datos.P_No_Contrarecibo + "'";
                    else
                        Filtro_SQL = " WHERE " + Ope_Pre_Recep_Docs_Movs.Campo_No_Contrarecibo + " = '" + Datos.P_No_Contrarecibo + "'";
                }
                if (Datos.P_Estatus != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND " + Ope_Pre_Recep_Docs_Movs.Campo_Estatus + " = '" + Datos.P_Estatus + "'";
                    else
                        Filtro_SQL = " WHERE " + Ope_Pre_Recep_Docs_Movs.Campo_Estatus + " = '" + Datos.P_Estatus + "'";
                }
                if (Datos.P_Empleado_ID != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND " + Ope_Pre_Recep_Docs_Movs.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'";
                    else
                        Filtro_SQL = " WHERE " + Ope_Pre_Recep_Docs_Movs.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'";
                }
                if (Datos.P_Observaciones != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar OR y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " OR UPPER(" + Ope_Pre_Recep_Docs_Movs.Campo_Observaciones + ") LIKE UPPER('%" + Datos.P_Observaciones + "%')";
                    else
                        Filtro_SQL = " WHERE UPPER(" + Ope_Pre_Recep_Docs_Movs.Campo_Observaciones + ") LIKE UPPER('%" + Datos.P_Observaciones + "%')";
                }
                Mi_SQL = Mi_SQL + Filtro_SQL + " ORDER BY " + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento;

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
        /// 	NOMBRE_FUNCIÓN: Consulta_Movimiento
        /// 	DESCRIPCIÓN: Consulta los campos No_Movimiento, No_Recepcion_Documento, Numero_Escritura, Cuenta_Predial_ID y Empleado_ID
        /// 	            de las recepciones de documentos en la BD
        /// 	PARÁMETROS:
        /// 		1. Datos: Indica qué registro se desea consultar a la base de datos
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 25-mar-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Movimiento(Cls_Ope_Pre_Recep_Docs_Movs_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL
            String Filtro_SQL = ""; //Variable para el filtro de la consulta SQL

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Numero_Escritura + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_No_Contrarecibo + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Empleado_ID;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs;
                if (Datos.P_No_Movimiento != null)      // Si se recibió un no de movimiento filtrar por ese ID
                {
                    Filtro_SQL = " WHERE " + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + " = '" + Datos.P_No_Movimiento + "'";
                }
                if (Datos.P_No_Recepcion_Documento != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar OR y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND " + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " = '" + Datos.P_No_Recepcion_Documento + "'";
                    else
                        Filtro_SQL = " WHERE " + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " = '" + Datos.P_No_Recepcion_Documento + "'";
                }
                if (Datos.P_Numero_Escritura != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND " + Ope_Pre_Recep_Docs_Movs.Campo_Numero_Escritura + " LIKE '" + Datos.P_Numero_Escritura + "'";
                    else
                        Filtro_SQL = " WHERE " + Ope_Pre_Recep_Docs_Movs.Campo_Numero_Escritura + " LIKE '" + Datos.P_Numero_Escritura + "'";
                }
                if (Datos.P_Cuenta_Predial_ID != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND " + Ope_Pre_Recep_Docs_Movs.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "'";
                    else
                        Filtro_SQL = " WHERE " + Ope_Pre_Recep_Docs_Movs.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "'";
                }
                if (Datos.P_No_Contrarecibo != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND " + Ope_Pre_Recep_Docs_Movs.Campo_No_Contrarecibo + " = '" + Datos.P_No_Contrarecibo + "'";
                    else
                        Filtro_SQL = " WHERE " + Ope_Pre_Recep_Docs_Movs.Campo_No_Contrarecibo + " = '" + Datos.P_No_Contrarecibo + "'";
                }
                if (Datos.P_Estatus != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND " + Ope_Pre_Recep_Docs_Movs.Campo_Estatus + " = '" + Datos.P_Estatus + "'";
                    else
                        Filtro_SQL = " WHERE " + Ope_Pre_Recep_Docs_Movs.Campo_Estatus + " = '" + Datos.P_Estatus + "'";
                }
                if (Datos.P_Empleado_ID != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND " + Ope_Pre_Recep_Docs_Movs.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'";
                    else
                        Filtro_SQL = " WHERE " + Ope_Pre_Recep_Docs_Movs.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'";
                }
                if (Datos.P_Observaciones != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar OR y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " OR UPPER(" + Ope_Pre_Recep_Docs_Movs.Campo_Observaciones + ") LIKE UPPER('%" + Datos.P_Observaciones + "%')";
                    else
                        Filtro_SQL = " WHERE UPPER(" + Ope_Pre_Recep_Docs_Movs.Campo_Observaciones + ") LIKE UPPER('%" + Datos.P_Observaciones + "%')";
                }
                Mi_SQL = Mi_SQL + Filtro_SQL + " ORDER BY " + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento;

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

    }
}