using System;
using System.Data;
using System.Data.OracleClient;
using System.Text;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Catalogo_Atencion_Ciudadana_Organigrama.Negocio;

namespace Presidencia.Catalogo_Atencion_Ciudadana_Organigrama.Datos
{
    public class Cls_Cat_Ate_Organigrama_Datos
    {
        #region METODOS
        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Alta_Empleado_Unidad
        ///DESCRIPCIÓN: Forma y ejecuta una consulta para dar de alta un registro en la base de datos
        ///PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio con parámetros para la consulta
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 30-ago-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Alta_Empleado_Unidad(Cls_Cat_Ate_Organigrama_Negocios Datos)
        {
            OracleConnection Conexion = new OracleConnection();//Variable de conexión.
            OracleCommand Comando = new OracleCommand();//Variable que ejecuta las sentencias SQL.
            OracleTransaction Transaccion = null;//Variable que ejecutara las transacciones contra la base de datos.

            Object Parametro_ID = null;
            StringBuilder Mi_SQL = new StringBuilder();
            int Registros_Insertados = 0;

            Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            Conexion.Open();

            Transaccion = Conexion.BeginTransaction();
            Comando.Connection = Conexion;
            Comando.Transaction = Transaccion;

            try
            {
                Mi_SQL.Append("select nvl(max(" + Cat_Organigrama.Campo_Parametro_ID + "), '00000') from " + Cat_Organigrama.Tabla_Cat_Organigrama);

                Comando.CommandText = Mi_SQL.ToString();
                Parametro_ID = Comando.ExecuteOracleScalar();
                Mi_SQL.Remove(0, Mi_SQL.Length);

                if (Convert.IsDBNull(Parametro_ID))
                {
                    Datos.P_Parametro_ID = "00001";
                }
                else
                {
                    Datos.P_Parametro_ID = String.Format("{0:00000}", (Convert.ToInt32(Parametro_ID.ToString()) + 1));
                }

                Mi_SQL.Append("insert into " + Cat_Organigrama.Tabla_Cat_Organigrama + " ( ");
                Mi_SQL.Append(Cat_Organigrama.Campo_Parametro_ID + ", ");
                Mi_SQL.Append(Cat_Organigrama.Campo_Dependencia_ID + ", ");
                Mi_SQL.Append(Cat_Organigrama.Campo_Empleado_ID + ", ");
                Mi_SQL.Append(Cat_Organigrama.Campo_Nombre_Empleado + ", ");
                Mi_SQL.Append(Cat_Organigrama.Campo_Tipo + ", ");
                Mi_SQL.Append(Cat_Organigrama.Campo_Modulo + ", ");
                Mi_SQL.Append(Cat_Organigrama.Campo_Usuario_Creo + ", ");
                Mi_SQL.Append(Cat_Organigrama.Campo_Fecha_Creo + ") values(");
                Mi_SQL.Append("'" + Datos.P_Parametro_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_Dependencia_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_Empleado_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_Nombre_Empleado + "', ");
                Mi_SQL.Append("'" + Datos.P_Tipo + "', ");
                Mi_SQL.Append("'" + Datos.P_Modulo + "', ");
                Mi_SQL.Append("'" + Datos.P_Empleado_ID + "', SYSDATE)");

                Comando.CommandText = Mi_SQL.ToString();
                Registros_Insertados = Comando.ExecuteNonQuery();

                Transaccion.Commit();
            }
            catch (Exception Ex)
            {
                Transaccion.Rollback();
                throw new Exception("Error al realizar la operacion de insercion. Error: [" + Ex.Message + "]");
            }
            return Registros_Insertados;
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Modificar_Empleado_Unidad
        ///DESCRIPCIÓN: Forma y ejecuta una consulta para dar de actualizar un registro en la base de datos
        ///PARÁMETROS:
        /// 		1. Datos: instancia de la clase de negocio con los datos a actualizar
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 30-ago-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Modificar_Empleado_Unidad(Cls_Cat_Ate_Organigrama_Negocios Datos)
        {
            OracleConnection Conexion = new OracleConnection();//Variable de conexión.
            OracleCommand Comando = new OracleCommand();//Variable que ejecuta las sentencias SQL.
            OracleTransaction Transaccion = null;//Variable que ejecutara las transacciones contra la base de datos.

            Object Parametro_ID = null;
            StringBuilder Mi_SQL = new StringBuilder();
            int Registros_Modificados = 0;

            Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            Conexion.Open();

            Transaccion = Conexion.BeginTransaction();
            Comando.Connection = Conexion;
            Comando.Transaction = Transaccion;

            try
            {
                Mi_SQL.Append("update " + Cat_Organigrama.Tabla_Cat_Organigrama + " set ");
                Mi_SQL.Append(Cat_Organigrama.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "', ");
                Mi_SQL.Append(Cat_Organigrama.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "', ");
                Mi_SQL.Append(Cat_Organigrama.Campo_Nombre_Empleado + " = '" + Datos.P_Nombre_Empleado + "', ");
                Mi_SQL.Append(Cat_Organigrama.Campo_Tipo + " = '" + Datos.P_Tipo + "', ");
                Mi_SQL.Append(Cat_Organigrama.Campo_Usuario_Modifico + " = '" + Datos.P_Empleado_ID + "', ");
                Mi_SQL.Append(Cat_Organigrama.Campo_Fecha_Modifico + " = SYSDATE");
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Cat_Organigrama.Campo_Parametro_ID + " = '" + Datos.P_Parametro_ID + "'");

                Comando.CommandText = Mi_SQL.ToString();
                Registros_Modificados = Comando.ExecuteNonQuery();

                Transaccion.Commit();
            }
            catch (Exception Ex)
            {
                Transaccion.Rollback();
                throw new Exception("Error al realizar la operacion de actualización. Error: [" + Ex.Message + "]");
            }
            return Registros_Modificados;
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Eliminar_Empleado_Unidad
        ///DESCRIPCIÓN: Forma y ejecuta una consulta para eliminar un registro en la base de datos
        ///PARÁMETROS:
        /// 		1. Datos: instancia de la clase de negocio con los datos del registro a eliminar
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 30-ago-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Eliminar_Empleado_Unidad(Cls_Cat_Ate_Organigrama_Negocios Datos)
        {
            OracleConnection Conexion = new OracleConnection();//Variable de conexión.
            OracleCommand Comando = new OracleCommand();//Variable que ejecuta las sentencias SQL.
            OracleTransaction Transaccion = null;//Variable que ejecutara las transacciones contra la base de datos.

            Object Parametro_ID = null;
            StringBuilder Mi_SQL = new StringBuilder();
            int Registros_Eliminados = 0;

            Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            Conexion.Open();

            Transaccion = Conexion.BeginTransaction();
            Comando.Connection = Conexion;
            Comando.Transaction = Transaccion;

            try
            {
                Mi_SQL.Append("delete from " + Cat_Organigrama.Tabla_Cat_Organigrama + " where ");
                Mi_SQL.Append(Cat_Organigrama.Tabla_Cat_Organigrama + "." + Cat_Organigrama.Campo_Parametro_ID + " = '");
                Mi_SQL.Append(Datos.P_Parametro_ID + "' ");

                Comando.CommandText = Mi_SQL.ToString();
                Registros_Eliminados = Comando.ExecuteNonQuery();

                Transaccion.Commit();
            }
            catch (Exception Ex)
            {
                Transaccion.Rollback();
                throw new Exception("Error al realizar la operacion de baja. Error: [" + Ex.Message + "]");
            }
            return Registros_Eliminados;
        }
        #endregion METODOS
        #region CONSULTA

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Consultar_Empleado_Unidad
        ///DESCRIPCIÓN: Forma y ejecuta una consulta con filtros opcionales por empleado_id, puesto y id del registro
        ///PARÁMETROS:
        /// 		1. Datos: instancia de la clase de negocio con los datos del registro a eliminar
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 30-ago-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Empleado_Unidad(Cls_Cat_Ate_Organigrama_Negocios Datos)
        {
            DataTable Dt_Parametro = null; //tabla que contendra los datos de la consulta
            StringBuilder Mi_Sql = new StringBuilder();//CADENA QUE RECIBE LOS DATOS PARA LA SENTENCIA SQL
            try
            {
                Mi_Sql.Append("SELECT ");
                Mi_Sql.Append(Cat_Organigrama.Tabla_Cat_Organigrama + "." + Cat_Organigrama.Campo_Parametro_ID + ", ");
                Mi_Sql.Append(Cat_Organigrama.Tabla_Cat_Organigrama + "." + Cat_Organigrama.Campo_Empleado_ID + ", ");
                Mi_Sql.Append(Cat_Organigrama.Tabla_Cat_Organigrama + "." + Cat_Organigrama.Campo_Nombre_Empleado + ", ");
                Mi_Sql.Append(Cat_Organigrama.Tabla_Cat_Organigrama + "." + Cat_Organigrama.Campo_Dependencia_ID + ", ");
                Mi_Sql.Append(Cat_Organigrama.Tabla_Cat_Organigrama + "." + Cat_Organigrama.Campo_Modulo + ", ");
                Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS UR, ");
                Mi_Sql.Append(Cat_Organigrama.Tabla_Cat_Organigrama + "." + Cat_Organigrama.Campo_Tipo + ", NVL(");
                Mi_Sql.Append(Cat_Organigrama.Tabla_Cat_Organigrama + "." + Cat_Organigrama.Campo_Nombre_Empleado + ",(");
                Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " || ' ' || ");
                Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ")) AS EMPLEADO, ");
                Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado);
                Mi_Sql.Append(" from " + Cat_Organigrama.Tabla_Cat_Organigrama);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " on ");
                Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " = ");
                Mi_Sql.Append(Cat_Organigrama.Tabla_Cat_Organigrama + "." + Cat_Organigrama.Campo_Dependencia_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " on ");
                Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " = ");
                Mi_Sql.Append(Cat_Organigrama.Tabla_Cat_Organigrama + "." + Cat_Organigrama.Campo_Empleado_ID + " WHERE ");

                // agregar filtros opcionales
                if (!string.IsNullOrEmpty(Datos.P_Parametro_ID))
                {
                    Mi_Sql.Append(Cat_Organigrama.Tabla_Cat_Organigrama + "." + Cat_Organigrama.Campo_Parametro_ID + "='" + Datos.P_Parametro_ID + "' AND ");
                }
                if (!string.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    Mi_Sql.Append(Cat_Organigrama.Tabla_Cat_Organigrama + "." + Cat_Organigrama.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "' AND ");
                }
                if (!string.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    Mi_Sql.Append(Cat_Organigrama.Tabla_Cat_Organigrama + "." + Cat_Organigrama.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "' AND ");
                }
                if (!string.IsNullOrEmpty(Datos.P_Modulo))
                {
                    Mi_Sql.Append(Cat_Organigrama.Tabla_Cat_Organigrama + "." + Cat_Organigrama.Campo_Modulo + "='" + Datos.P_Modulo + "' AND ");
                }
                if (!string.IsNullOrEmpty(Datos.P_Nombre_Empleado))
                {
                    Mi_Sql.Append("( UPPER (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " || ' ' || "
                        + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || "
                        + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno
                        + ") LIKE UPPER('%" + Datos.P_Nombre_Empleado + "%') OR "
                        + "UPPER (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || "
                        + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || "
                         + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre
                        + ") LIKE UPPER('%" + Datos.P_Nombre_Empleado + "%') OR "
                        + "UPPER (" + Cat_Organigrama.Tabla_Cat_Organigrama + "." + Cat_Organigrama.Campo_Nombre_Empleado
                        + ") LIKE UPPER('%" + Datos.P_Nombre_Empleado + "%')) AND ");
                }

                // eliminar AND o WHERE al final del archivo
                if (Mi_Sql.ToString().EndsWith(" AND "))
                {
                    Mi_Sql.Remove(Mi_Sql.Length - 5, 5);
                }
                else if (Mi_Sql.ToString().EndsWith(" WHERE "))
                {
                    Mi_Sql.Remove(Mi_Sql.Length - 7, 7);
                }

                //ejecuta la sentencia SQL y regresa una tabla de datos
                Dt_Parametro = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar el parametros de descuentos. Error: [" + Ex.Message + "]");
            }
            return Dt_Parametro;
        }
        #endregion CONSULTA
    }
}