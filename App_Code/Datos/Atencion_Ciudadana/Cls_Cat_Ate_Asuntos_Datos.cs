using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Asuntos_AC.Negocio;
using System.Data;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Sessiones;

namespace Presidencia.Asuntos_AC.Datos
{
    public class Cls_Cat_Ate_Asuntos_Datos
    {
        #region VARIABLES INTERNAS
        private String ID;
        private String Clave;
        private String Nombre;
        private String Descripcion;
        private String Estatus;
        private String Prefijo_Folio;
        
        #endregion

        #region VARIABLES PUBLICAS


        #endregion

        #region METODOS
	    
        public Cls_Cat_Ate_Asuntos_Datos()
	    {
	    }
        private static int Consecutivo()
        {
            int Numero_Consecutivo = 0;
            String Mi_SQL;
            Object Objeto;
            Mi_SQL = "SELECT NVL(MAX (" + Cat_Ate_Asuntos.Campo_AsuntoID + "),0) ";
            Mi_SQL = Mi_SQL + "FROM " + Cat_Ate_Asuntos.Tabla_Cat_Ate_Asuntos;
            Objeto = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            Numero_Consecutivo = Convert.ToInt32(Objeto) + 1;
            return Numero_Consecutivo;
        }

        public static int Guardar_Registro(Cls_Cat_Ate_Asuntos_Negocio Negocio) 
        {
            int Registros_Guardados = 0;
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "INSERT INTO " + Cat_Ate_Asuntos.Tabla_Cat_Ate_Asuntos +
                " (" +
                Cat_Ate_Asuntos.Campo_AsuntoID + ", " +
                Cat_Ate_Asuntos.Campo_Clave + ", " +
                Cat_Ate_Asuntos.Campo_Nombre + ", " +
                Cat_Ate_Asuntos.Campo_Descripcion + ", " +
                Cat_Ate_Asuntos.Campo_Estatus + ", " +
                Cat_Ate_Asuntos.Campo_DependenciaID + ", " +
                Cat_Ate_Asuntos.Campo_AreaID + ", " +
                Cat_Ate_Asuntos.Campo_UsuarioCreo + ", " +
                Cat_Ate_Asuntos.Campo_FechaCreo + 
                ") VALUES (" +
                Consecutivo() + ", " +
                "'" + Negocio.P_Clave + "', " +
                "'" + Negocio.P_Nombre + "', " +
                "'" + Negocio.P_Descripcion + "', " +
                "'" + Negocio.P_Estatus + "', " +
                "'" + Negocio.P_Dependencia_ID + "', " +
                "'" + Negocio.P_UR_ID + "', " +
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
        public static int Actualizar_Registro(Cls_Cat_Ate_Asuntos_Negocio Negocio)
        {
            int Registros_Guardados = 0;
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "UPDATE " + Cat_Ate_Asuntos.Tabla_Cat_Ate_Asuntos +
                " SET " +
                Cat_Ate_Asuntos.Campo_Clave + " = '" + Negocio.P_Clave + "', " +
                Cat_Ate_Asuntos.Campo_Nombre + " = '" + Negocio.P_Nombre + "', " +
                Cat_Ate_Asuntos.Campo_Descripcion + " = '" + Negocio.P_Descripcion + "', " +
                Cat_Ate_Asuntos.Campo_Estatus + " = '" + Negocio.P_Estatus + "', " +
                Cat_Ate_Asuntos.Campo_DependenciaID + " = '" + Negocio.P_Dependencia_ID + "', " +
                Cat_Ate_Asuntos.Campo_AreaID + " = '" + Negocio.P_UR_ID + "', " +
                Cat_Ate_Asuntos.Campo_UsuarioModifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', " +
                Cat_Ate_Asuntos.Campo_FechaModifico + " = SYSDATE" +
                " WHERE " + Cat_Ate_Asuntos.Campo_AsuntoID + " = " + Negocio.P_ID;
                Registros_Guardados = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                Registros_Guardados = 0;
                throw new Exception(Ex.ToString());
            }

            return Registros_Guardados;
        }

        public static int Eliminar_Registro(Cls_Cat_Ate_Asuntos_Negocio Negocio)
        {
            int Registros_Eliminados = 0;
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "DELETE " + Cat_Ate_Asuntos.Tabla_Cat_Ate_Asuntos +
                " WHERE " + Cat_Ate_Asuntos.Campo_AsuntoID + " = '" + Negocio.P_ID + "'";                
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
        ///DESCRIPCIÓN: Genera y ejecuta una consulta a Cat_Ate_Asuntos en la base de datos
        ///PARÁMETROS:
        /// 		1. Negocio: instancia de la clase de negocio con los filtros para la consulta
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 17-may-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Registros(Cls_Cat_Ate_Asuntos_Negocio Negocio)
        {
            DataTable Dt_datos = null;
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT * FROM " + Cat_Ate_Asuntos.Tabla_Cat_Ate_Asuntos + " WHERE ";

                // agregar filtros
                if (!String.IsNullOrEmpty(Negocio.P_Descripcion) && !String.IsNullOrEmpty(Negocio.P_Nombre) && !String.IsNullOrEmpty(Negocio.P_Clave)) // buscar en nombre, clave o descripción
                {
                    Mi_SQL += "(UPPER(" + Cat_Ate_Asuntos.Campo_Descripcion + ") LIKE UPPER('%" + Negocio.P_Descripcion + "%')"
                        + " OR UPPER(" + Cat_Ate_Asuntos.Campo_Nombre + ") LIKE UPPER('%" + Negocio.P_Nombre + "%')"
                        + " OR UPPER(" + Cat_Ate_Asuntos.Campo_Clave + ") LIKE UPPER('%" + Negocio.P_Clave + "%')) AND ";
                }
                else
                {
                    if (!String.IsNullOrEmpty(Negocio.P_Descripcion)) // buscar en nombre, clave o descripción
                    {
                        Mi_SQL += "UPPER(" + Cat_Ate_Asuntos.Campo_Descripcion + ") LIKE UPPER('%" + Negocio.P_Descripcion + "%') AND ";
                    }
                    if (!String.IsNullOrEmpty(Negocio.P_Clave))
                    {
                        Mi_SQL += " UPPER(" + Cat_Ate_Asuntos.Campo_Clave + ") LIKE UPPER('%" + Negocio.P_Clave + "%') AND ";
                    }
                    if (!String.IsNullOrEmpty(Negocio.P_Nombre))
                    {
                        Mi_SQL += "UPPER(" + Cat_Ate_Asuntos.Campo_Nombre + ") LIKE UPPER('%" + Negocio.P_Nombre + "%') AND ";
                    }
                }

                if ( !String.IsNullOrEmpty(Negocio.P_ID))
                {
                    Mi_SQL += Cat_Ate_Asuntos.Campo_AsuntoID + " = '" + Negocio.P_ID + "' AND ";
                }
                if ( !String.IsNullOrEmpty(Negocio.P_Dependencia_ID))
                {
                    Mi_SQL += Cat_Ate_Asuntos.Campo_DependenciaID + " = '" + Negocio.P_Dependencia_ID + "' AND ";
                }
                if ( !String.IsNullOrEmpty(Negocio.P_Estatus))
                {
                    Mi_SQL += Cat_Ate_Asuntos.Campo_Estatus + " = '" + Negocio.P_Estatus + "' ";
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

                // ejecutar consulta
                Dt_datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Dt_datos;
        }

        public static bool Clave_Duplicada(Cls_Cat_Ate_Asuntos_Negocio Negocio)
        {
            bool Dato_Duplicado = false;
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT " + Cat_Ate_Asuntos.Campo_Clave + " FROM " + Cat_Ate_Asuntos.Tabla_Cat_Ate_Asuntos +
                " WHERE " +
                Cat_Ate_Asuntos.Campo_Clave + " = '" + Negocio.P_Clave + "'";
                if (!String.IsNullOrEmpty(Negocio.P_ID))
                {
                    Mi_SQL += " AND " + Cat_Ate_Asuntos.Campo_AsuntoID + " NOT IN (" + Negocio.P_ID + ")";
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