using System;
using System.Data;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Recep_Docs_Anexos.Negocio;


namespace Presidencia.Operacion_Predial_Recep_Docs_Anexos.Datos
{
    public class Cls_Ope_Pre_Recep_Docs_Anexos_Datos
    {
        public Cls_Ope_Pre_Recep_Docs_Anexos_Datos()
        {
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Alta_Anexo
        /// 	DESCRIPCIÓN: 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///                  2. Da de Alta el anexo (por movimiento de recepción de documentos) en la BD con los datos 
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
        public static int Alta_Anexo(Cls_Ope_Pre_Recep_Docs_Anexos_Negocio Datos)
        {
            String Mi_SQL;  //Obtiene la cadena de inserción hacía la base de datos
            Object Anexo_ID; //Obtiene el ID con el cual se guardarán la información en la base de datos

            try
            {
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Recep_Docs_Anexos.Campo_No_Anexo + "),'0000000000') ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Pre_Recep_Docs_Anexos.Tabla_Ope_Pre_Recep_Docs_Anexos;
                Anexo_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Anexo_ID))
                {
                    Datos.P_No_Anexo = "0000000001";
                }
                else
                {
                    Datos.P_No_Anexo = String.Format("{0:0000000000}", Convert.ToInt32(Anexo_ID) + 1);
                }
                //Consulta para la inserción del Tipo de documento con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Ope_Pre_Recep_Docs_Anexos.Tabla_Ope_Pre_Recep_Docs_Anexos + " (";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Anexos.Campo_No_Anexo + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Anexos.Campo_No_Movimiento + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Anexos.Campo_Ruta + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Anexos.Campo_Comentarios + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Anexos.Campo_Documento_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Anexos.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Anexos.Campo_Fecha_Creo + ") VALUES ('";
                Mi_SQL = Mi_SQL + Datos.P_No_Anexo + "', '";
                Mi_SQL = Mi_SQL + Datos.P_No_Movimiento + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Ruta + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Comentarios + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Documento_ID + "', '";
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
        /// 	NOMBRE_FUNCIÓN: Modificar_Anexo
        /// 	DESCRIPCIÓN: Modifica los datos del Anexo con lo que introdujo el usuario
        /// 	            Regresa el número de filas modificadas
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio con los datos que serán insertados en la base de datos
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 25-mar-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Modificar_Anexo(Cls_Ope_Pre_Recep_Docs_Anexos_Negocio Datos)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos

            try
            {
                //Consulta para actualizar los datos del Anexo con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Ope_Pre_Recep_Docs_Anexos.Tabla_Ope_Pre_Recep_Docs_Anexos + " SET ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Anexos.Campo_No_Anexo + " = '" + Datos.P_No_Anexo + "', ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Anexos.Campo_No_Movimiento + " = '" + Datos.P_No_Movimiento + "', ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Anexos.Campo_Ruta + " = '" + Datos.P_Ruta + "', ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Anexos.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Anexos.Campo_Documento_ID + " = '" + Datos.P_Documento_ID + "', ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Anexos.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Anexos.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Anexos.Campo_No_Anexo + " = '" + Datos.P_No_Anexo + "'";

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
        /// 	NOMBRE_FUNCIÓN: Consulta_Datos_Anexo
        /// 	DESCRIPCIÓN: Consulta todos los campos de los Anexos (por recepciones de documentos) en la BD (sin los de auditoría)
        /// 	PARÁMETROS:
        /// 		1. Datos: Indica qué registro se desea consultar a la base de datos
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 25-mar-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Datos_Anexo(Cls_Ope_Pre_Recep_Docs_Anexos_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL
            String Filtro_SQL = ""; //Variable para el filtro de la consulta SQL

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Anexos.Campo_No_Anexo + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Anexos.Campo_No_Movimiento + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Anexos.Campo_Ruta + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Anexos.Campo_Comentarios + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Anexos.Campo_Documento_ID;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Recep_Docs_Anexos.Tabla_Ope_Pre_Recep_Docs_Anexos;
                if (Datos.P_No_Anexo != null)      // Si se recibió un ID de anexo filtrar por ese ID
                {
                    Filtro_SQL = " WHERE " + Ope_Pre_Recep_Docs_Anexos.Campo_No_Anexo + " = '" + Datos.P_No_Anexo + "'";
                }
                if (Datos.P_No_Movimiento != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar OR y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND " + Ope_Pre_Recep_Docs_Anexos.Campo_No_Movimiento + " = '" + Datos.P_No_Movimiento + "'";
                    else
                        Filtro_SQL = " WHERE " + Ope_Pre_Recep_Docs_Anexos.Campo_No_Movimiento + " = '" + Datos.P_No_Movimiento + "'";
                }
                if (Datos.P_Documento_ID != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND " + Ope_Pre_Recep_Docs_Anexos.Campo_Documento_ID + " LIKE '" + Datos.P_Documento_ID + "'";
                    else
                        Filtro_SQL = " WHERE " + Ope_Pre_Recep_Docs_Anexos.Campo_Documento_ID + " LIKE '" + Datos.P_Documento_ID + "'";
                }
                if (Datos.P_Comentarios != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar OR y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " OR UPPER(" + Ope_Pre_Recep_Docs_Anexos.Campo_Comentarios + ") LIKE UPPER('%" + Datos.P_Comentarios + "%')";
                    else
                        Filtro_SQL = " WHERE UPPER(" + Ope_Pre_Recep_Docs_Anexos.Campo_Comentarios + ") LIKE UPPER('%" + Datos.P_Comentarios + "%')";
                }
                Mi_SQL = Mi_SQL + Filtro_SQL + " ORDER BY " + Ope_Pre_Recep_Docs_Anexos.Campo_No_Movimiento;

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