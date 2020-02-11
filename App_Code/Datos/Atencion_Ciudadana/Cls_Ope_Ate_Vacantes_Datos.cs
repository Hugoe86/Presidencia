using System;
using System.Data;
using Presidencia.Operacion_Atencion_Ciudadana_Vacantes.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Collections.Generic;
using Presidencia.Caja_Pagos.Negocio;

namespace Presidencia.Operacion_Atencion_Ciudadana_Vacantes.Datos
{

    public class Cls_Ope_Ate_Vacantes_Datos
    {

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Alta_Vacantes_Tabla
        /// DESCRIPCIÓN: dar de alta vacantes en la base de datos después de limpiar la tabla
        /// PARÁMETROS:
        /// 	1. Datos: Instancia de la clase de negocio con los datos que serán insertados en la base de datos
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 31-may-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Int32 Alta_Vacantes_Tabla(Cls_Ope_Ate_Vacantes_Negocio Datos)
        {
            String Mi_SQL;
            int No_Vacante = 0;
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;
            Int32 Filas_Afectadas = 0;

            // si llego un Comando como parametro, utilizarlo
            if (Datos.P_Comando_Oracle != null)    // si la conexion llego como parametro, establecer como comando para utilizar
            {
                Comando = Datos.P_Comando_Oracle;
            }
            else    // si no, crear nueva conexion y transaccion
            {
                Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Conexion.Open();
                Transaccion = Conexion.BeginTransaction();
                Comando.Connection = Conexion;
                Comando.Transaction = Transaccion;
            }

            try
            {
                // limpiar la tabla Ope_Ate_Vacantes
                Mi_SQL = "DELETE FROM " + Ope_Ate_Vacantes.Tabla_Ope_Ate_Vacantes;
                Comando.CommandText = Mi_SQL;
                Comando.ExecuteNonQuery();

                // validar que la tabla no sea null
                if (Datos.P_Dt_Vacantes != null)
                {
                    // recorrer la tabla de vacantes para insertar cada dato
                    foreach (DataRow Detalle in Datos.P_Dt_Vacantes.Rows)
                    {
                        // saltar el registro si no se obtiene un número de vacante
                        if (!int.TryParse(Detalle[Ope_Ate_Vacantes.Campo_No_Vacante].ToString(), out No_Vacante) || No_Vacante <= 0)
                        {
                            continue;
                        }

                        Mi_SQL = "INSERT INTO " + Ope_Ate_Vacantes.Tabla_Ope_Ate_Vacantes + " ("
                            + Ope_Ate_Vacantes.Campo_No_Vacante
                            + ", " + Ope_Ate_Vacantes.Campo_Nombre_Vacante
                            + ", " + Ope_Ate_Vacantes.Campo_Edad
                            + ", " + Ope_Ate_Vacantes.Campo_Sexo
                            + ", " + Ope_Ate_Vacantes.Campo_Escolaridad
                            + ", " + Ope_Ate_Vacantes.Campo_Experiencia
                            + ", " + Ope_Ate_Vacantes.Campo_Sueldo
                            + ", " + Ope_Ate_Vacantes.Campo_Contacto
                            + ", " + Ope_Ate_Vacantes.Campo_Usuario_Creo
                            + ", " + Ope_Ate_Vacantes.Campo_Fecha_Creo;
                        Mi_SQL += ") VALUES ("
                            + "'" + (No_Vacante).ToString().PadLeft(10, '0') + "'"
                            + ", '" + Detalle[Ope_Ate_Vacantes.Campo_Nombre_Vacante] + "'"
                            + ", '" + Detalle[Ope_Ate_Vacantes.Campo_Edad] + "'"
                            + ", '" + Detalle[Ope_Ate_Vacantes.Campo_Sexo] + "'"
                            + ", '" + Detalle[Ope_Ate_Vacantes.Campo_Escolaridad] + "'"
                            + ", '" + Detalle[Ope_Ate_Vacantes.Campo_Experiencia] + "'"
                            + ", '" + Detalle[Ope_Ate_Vacantes.Campo_Sueldo] + "'"
                            + ", '" + Detalle[Ope_Ate_Vacantes.Campo_Contacto] + "'"
                            + ", '" + Datos.P_Usuario + "', SYSDATE)";

                        Comando.CommandText = Mi_SQL;
                        Filas_Afectadas = Comando.ExecuteNonQuery();
                    }
                }

                if (Datos.P_Comando_Oracle == null)    // si la conexion no llego como parametro, aplicar consultas
                {
                    Transaccion.Commit();
                }

            }
            catch (OracleException Ex)
            {
                if (Datos.P_Comando_Oracle == null && Transaccion != null)
                {
                    Transaccion.Rollback();
                }
                throw new Exception("Error de oracle: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Datos.P_Comando_Oracle == null && Transaccion != null)
                {
                    Transaccion.Rollback();
                }
                throw new Exception("Los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Datos.P_Comando_Oracle == null && Transaccion != null)
                {
                    Transaccion.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                if (Datos.P_Comando_Oracle == null)
                {
                    Conexion.Close();
                }
            }

            return Filas_Afectadas;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Vacantes
        /// DESCRIPCIÓN: Genera y ejecuta una consulta de vacantes y regresa un datatable con la información encontrada
        ///             con filtros opcionales por nombre, sexo, número, escolaridad y experiencia
        /// PARÁMETROS:
        /// 		1. Datos: instancia de la clase de negocio con los datos para la consulta
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 31-mar-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Vacantes(Cls_Ope_Ate_Vacantes_Negocio Datos)
        {
            String Mi_SQL;
            DataSet Ds_Resultado;

            try
            {
                Mi_SQL = "SELECT TO_NUMBER(" + Ope_Ate_Vacantes.Campo_No_Vacante + ") " + Ope_Ate_Vacantes.Campo_No_Vacante
                    + ", " + Ope_Ate_Vacantes.Campo_Nombre_Vacante
                    + ", " + Ope_Ate_Vacantes.Campo_Edad
                    + ", " + Ope_Ate_Vacantes.Campo_Sexo
                    + ", " + Ope_Ate_Vacantes.Campo_Escolaridad
                    + ", " + Ope_Ate_Vacantes.Campo_Experiencia
                    + ", " + Ope_Ate_Vacantes.Campo_Sueldo
                    + ", " + Ope_Ate_Vacantes.Campo_Contacto
                    + " FROM " + Ope_Ate_Vacantes.Tabla_Ope_Ate_Vacantes + " WHERE ";

                // filtros opcionales
                if (!string.IsNullOrEmpty(Datos.P_No_Vacante))
                {
                    Mi_SQL += "TO_NUMBER(" + Ope_Ate_Vacantes.Campo_No_Vacante + ") = TO_NUMBER('" + Datos.P_No_Vacante + "') AND ";
                }
                if (!String.IsNullOrEmpty(Datos.P_Nombre_Vacante))
                {
                    Mi_SQL += "UPPER(" + Ope_Ate_Vacantes.Campo_Nombre_Vacante + ") LIKE UPPER('%" + Datos.P_Nombre_Vacante + "%') AND ";
                }
                if (!String.IsNullOrEmpty(Datos.P_Sexo))
                {
                    Mi_SQL += "UPPER(" + Ope_Ate_Vacantes.Campo_Sexo + ") LIKE UPPER('%" + Datos.P_Sexo + "%') AND ";
                }
                if (!String.IsNullOrEmpty(Datos.P_Escolaridad))
                {
                    Mi_SQL += "UPPER(" + Ope_Ate_Vacantes.Campo_Escolaridad + ") LIKE UPPER('%" + Datos.P_Escolaridad + "%') AND ";
                }
                if (!String.IsNullOrEmpty(Datos.P_Experiencia))
                {
                    Mi_SQL += "UPPER(" + Ope_Ate_Vacantes.Campo_Experiencia + ") LIKE UPPER('%" + Datos.P_Experiencia + "%') AND ";
                }

                // filtros dinámicos
                if (!String.IsNullOrEmpty(Datos.P_Filtros_Dinamicos))
                {
                    Mi_SQL += Datos.P_Filtros_Dinamicos;
                }

                // eliminar AND o WHERE del final de la consulta
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                else if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                Ds_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Resultado != null && Ds_Resultado.Tables.Count > 0)
                {
                    return Ds_Resultado.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de vacantes. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return null;
        }

    }

}