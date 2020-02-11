using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Programas_AC.Negocio;
using System.Data;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Sessiones;

namespace Presidencia.Programas_AC.Datos
{
    public class Cls_Cat_Ate_Programas_Datos
    {
        #region METODOS
	    
        public Cls_Cat_Ate_Programas_Datos()
	    {
	    }
        private static int Consecutivo()
        {
            int Numero_Consecutivo = 0;
            String Mi_SQL;
            Object Objeto;
            Mi_SQL = "SELECT NVL(MAX (" + Cat_Ate_Programas.Campo_Programa_ID + "),0) ";
            Mi_SQL = Mi_SQL + "FROM " + Cat_Ate_Programas.Tabla_Cat_Ate_Programas;
            Objeto = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            Numero_Consecutivo = Convert.ToInt32(Objeto) + 1;
            return Numero_Consecutivo;
        }

        public static int Guardar_Registro(Cls_Cat_Ate_Programas_Negocio Negocio) 
        {
            int Registros_Guardados = 0;
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "INSERT INTO " + Cat_Ate_Programas.Tabla_Cat_Ate_Programas +
                " (" +
                Cat_Ate_Programas.Campo_Programa_ID + ", " +
                Cat_Ate_Programas.Campo_Clave + ", " +
                Cat_Ate_Programas.Campo_Nombre + ", " +
                Cat_Ate_Programas.Campo_Descripcion + ", " +
                Cat_Ate_Programas.Campo_Estatus + ", " +
                Cat_Ate_Programas.Campo_Prefijo_Folio + ", " +
                Cat_Ate_Programas.Campo_Folio_Anual + ", " +
                Cat_Ate_Programas.Campo_Usuario_Creo + ", " +
                Cat_Ate_Programas.Campo_Fecha_Creo + 
                ") VALUES (" +
                Consecutivo() + ", " +
                "'" + Negocio.P_Clave + "', " +
                "'" + Negocio.P_Nombre + "', " +
                "'" + Negocio.P_Descripcion + "', " +
                "'" + Negocio.P_Estatus + "', " +
                "'" + Negocio.P_Prefijo_Folio + "', " +
                "'" + Negocio.P_Folio_Anual + "', " +
                "'" + Cls_Sessiones.Nombre_Empleado + "', SYSDATE)";                
                Registros_Guardados = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                Registros_Guardados = 0;
                throw new Exception(Ex.ToString());
            }
            return Registros_Guardados;
        }
        public static int Actualizar_Registro(Cls_Cat_Ate_Programas_Negocio Negocio)
        {
            int Registros_Guardados = 0;
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "UPDATE " + Cat_Ate_Programas.Tabla_Cat_Ate_Programas +
                " SET " +
                Cat_Ate_Programas.Campo_Clave + " = '" + Negocio.P_Clave + "', " +
                Cat_Ate_Programas.Campo_Nombre + " = '" + Negocio.P_Nombre + "', " +
                Cat_Ate_Programas.Campo_Descripcion + " = '" + Negocio.P_Descripcion + "', " +
                Cat_Ate_Programas.Campo_Estatus + " = '" + Negocio.P_Estatus + "', " +
                Cat_Ate_Programas.Campo_Prefijo_Folio + " = '" + Negocio.P_Prefijo_Folio + "', " +
                Cat_Ate_Programas.Campo_Folio_Anual + " = '" + Negocio.P_Folio_Anual + "', " +
                Cat_Ate_Programas.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', " +
                Cat_Ate_Programas.Campo_Fecha_Modifico + " = SYSDATE" +
                " WHERE " + Cat_Ate_Programas.Campo_Programa_ID + " = " + Negocio.P_ID;
                Registros_Guardados = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                Registros_Guardados = 0;
                throw new Exception(Ex.ToString());
            }

            return Registros_Guardados;
        }

        public static int Eliminar_Registro(Cls_Cat_Ate_Programas_Negocio Negocio)
        {
            int Registros_Eliminados = 0;
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "DELETE " + Cat_Ate_Programas.Tabla_Cat_Ate_Programas +
                " WHERE " + Cat_Ate_Programas.Campo_Programa_ID + " = '" + Negocio.P_ID + "'";                
                Registros_Eliminados = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                Registros_Eliminados = 0;
                throw new Exception(Ex.ToString());
            }

            return Registros_Eliminados;
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Consultar_Registros
        ///DESCRIPCIÓN: forma y ejecuta una consulta de programas con filtros opcionales
        ///PARÁMETROS:
        /// 		1. Negocio: instancia de la clase de negocio de donde se toman los filtros opcionales
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 25-ago-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Registros(Cls_Cat_Ate_Programas_Negocio Negocio)
        {
            DataTable Dt_datos = null;
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT * FROM " + Cat_Ate_Programas.Tabla_Cat_Ate_Programas + " WHERE ";

                // agregar filtros opcionales por clave y estatus
                if ( !String.IsNullOrEmpty(Negocio.P_Clave))
                {
                    Mi_SQL += Cat_Ate_Programas.Campo_Clave + " LIKE ('%" + Negocio.P_Clave + "%') AND ";   
                }
                if ( !String.IsNullOrEmpty(Negocio.P_Estatus))
                {
                    Mi_SQL += Cat_Ate_Programas.Campo_Estatus + " ='" + Negocio.P_Estatus + "'";   
                }

                // quitar AND o WHERE del final de la consulta
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                else if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }

                Dt_datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }

            return Dt_datos;
        }

        public static DataTable Consultar_Programas(Cls_Cat_Ate_Programas_Negocio Negocio)
        {
            DataTable Dt_datos = null;
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT "
                    + " TO_CHAR(" + Cat_Ate_Programas.Campo_Programa_ID + ") " + Cat_Ate_Programas.Campo_Programa_ID
                    + "," + Cat_Ate_Programas.Campo_Nombre
                    + "," + Cat_Ate_Programas.Campo_Clave
                    + "," + Cat_Ate_Programas.Campo_Descripcion
                    + "," + Cat_Ate_Programas.Campo_Estatus
                    + "," + Cat_Ate_Programas.Campo_Prefijo_Folio
                    + ", NVL(" + Cat_Ate_Programas.Campo_Folio_Anual + ",'SI') " + Cat_Ate_Programas.Campo_Folio_Anual
                    + " FROM " + Cat_Ate_Programas.Tabla_Cat_Ate_Programas;
                if (!String.IsNullOrEmpty(Negocio.P_ID))
                {
                    Mi_SQL += " WHERE " + Cat_Ate_Programas.Campo_Programa_ID + " = '" + Negocio.P_ID + "'";
                } 
                else if (!String.IsNullOrEmpty(Negocio.P_Estatus))
                {
                    Mi_SQL += " WHERE " + Cat_Ate_Programas.Campo_Estatus + " = '" + Negocio.P_Estatus + "'";
                } 
                else if (!String.IsNullOrEmpty(Negocio.P_Clave))
                {
                    Mi_SQL += " WHERE UPPER(" + Cat_Ate_Programas.Campo_Clave + ") LIKE UPPER('%" + Negocio.P_Clave + "%')";
                }
                Dt_datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }

            return Dt_datos;
        }

        public static bool Clave_Duplicada(Cls_Cat_Ate_Programas_Negocio Negocio)
        {
            bool Dato_Duplicado = false;
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT " + Cat_Ate_Programas.Campo_Clave + " FROM " + Cat_Ate_Programas.Tabla_Cat_Ate_Programas +
                " WHERE " +
                Cat_Ate_Programas.Campo_Clave + " = '" + Negocio.P_Clave + "'";
                if (!String.IsNullOrEmpty(Negocio.P_ID))
                {
                    Mi_SQL += " AND " + Cat_Ate_Programas.Campo_Programa_ID + " NOT IN (" + Negocio.P_ID + ")";
                }                                                
                Object Objeto = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Objeto != null && (Objeto.ToString().Length > 0))
                {
                    Dato_Duplicado = true;
                }
            }
            catch(Exception Ex)
            {
                Dato_Duplicado = false;
                throw new Exception(Ex.ToString());
            }
            return Dato_Duplicado;
        }


        #endregion

    }
}