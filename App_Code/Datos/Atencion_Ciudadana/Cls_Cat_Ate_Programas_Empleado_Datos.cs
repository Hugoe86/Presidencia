using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Text;
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
using Presidencia.Sessiones;
using Presidencia.Catalogo_Atencion_Ciudadana_Programas_Empleado.Negocio;

namespace Presidencia.Catalogo_Atencion_Ciudadana_Programas_Empleado.Datos
{
    public class Cls_Cat_Ate_Programas_Empleado_Datos
    {
        #region Consultas
        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Consultar_Empleado
        ///DESCRIPCIÓN: Formar y ejecutar una consulta de  empleados (con subconsulta) con filtros opcionales
        ///PARÁMETROS:
        /// 		1. Datos: instancia de la clase de negocio con filtros para búsqueda
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 02-jul-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Empleado(Cls_Cat_Ate_Programas_Empleado_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append(", (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") as Nombre_Empleado ");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Empleados.Campo_Estatus + "='ACTIVO'");

                //  FILTRO UNIDAD RESPONSABLE
                if (!String.IsNullOrEmpty(Datos.P_Unidad_Responsable_ID))
                {
                    Mi_SQL.Append(" and " + Cat_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Unidad_Responsable_ID + "'");
                }
                //  FILTRO NUMERO DE EMPLEADO
                if (!String.IsNullOrEmpty(Datos.P_Numero_Empleado))
                {
                    Mi_SQL.Append(" and TO_NUMBER(" + Cat_Empleados.Campo_No_Empleado + ")=TO_NUMBER('" + Datos.P_Numero_Empleado + "')");
                }
                //  FILTRO NOMBRE EMPLEADO
                if (!String.IsNullOrEmpty(Datos.P_Nombre_Empleado))
                {
                    Mi_SQL.Append(" AND (upper(" + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||");
                    Mi_SQL.Append(Cat_Empleados.Campo_Apellido_Materno + ") LIKE upper('%" + Datos.P_Nombre_Empleado + "%') )");
                }

                Mi_SQL.Append(" order by ");
                Mi_SQL.Append(Cat_Empleados.Campo_Apellido_Paterno + "," + Cat_Empleados.Campo_Apellido_Materno + "," + Cat_Empleados.Campo_Nombre);
                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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
        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Consultar_Programas_Empleados
        ///DESCRIPCIÓN: Forma y ejecuta una consulta de programas_empleados con filtros opcionales
        ///PARÁMETROS:
        ///         1. Datos: instancia de la clase de negocio con filtros para la consulta
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 30-jun-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Programas_Empleados(Cls_Cat_Ate_Programas_Empleado_Negocio Datos)
        {
            DataTable Dt_datos = null;
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT "
                    + Cat_Ate_Programas_Empleados.Campo_Programa_Empleado_ID
                    + "," + Cat_Ate_Programas_Empleados.Campo_Empleado_ID
                    + "," + Cat_Ate_Programas_Empleados.Campo_Programa_ID
                    + ", (SELECT " + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || "
                    + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || " + Cat_Empleados.Campo_Nombre
                    + " FROM " + Cat_Empleados.Tabla_Cat_Empleados
                    + " WHERE " + Cat_Empleados.Campo_Empleado_ID + " = "
                    + Cat_Ate_Programas_Empleados.Tabla_Cat_Ate_Programas_Empleados + "." + Cat_Ate_Programas_Empleados.Campo_Empleado_ID
                    + ") NOMBRE_EMPLEADO"
                    + ", (SELECT " + Cat_Ate_Programas.Campo_Nombre
                    + " FROM " + Cat_Ate_Programas.Tabla_Cat_Ate_Programas
                    + " WHERE " + Cat_Ate_Programas.Campo_Programa_ID + " = "
                    + Cat_Ate_Programas_Empleados.Tabla_Cat_Ate_Programas_Empleados + "." + Cat_Ate_Programas_Empleados.Campo_Programa_ID
                    + ") PROGRAMA"
                    + "," + Cat_Ate_Programas_Empleados.Campo_Estatus
                    + " FROM " + Cat_Ate_Programas_Empleados.Tabla_Cat_Ate_Programas_Empleados + " WHERE ";

                // agregar filtros
                if (!String.IsNullOrEmpty(Datos.P_Estatus)) // buscar en nombre, clave o descripción
                {
                    Mi_SQL += Cat_Ate_Programas_Empleados.Campo_Estatus + " = '" + Datos.P_Estatus + "' AND ";
                }
                if (!String.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    Mi_SQL += Cat_Ate_Programas_Empleados.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "' AND ";
                }
                if (!String.IsNullOrEmpty(Datos.P_Programa_Empleado_ID))
                {
                    Mi_SQL += Cat_Ate_Programas_Empleados.Campo_Programa_Empleado_ID + " = '" + Datos.P_Programa_Empleado_ID + "' AND ";
                }

                if (Datos.P_Programa_ID > 0)
                {
                    Mi_SQL += Cat_Ate_Programas_Empleados.Campo_Programa_ID + " = " + Datos.P_Programa_ID.ToString();
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
                throw new Exception("Error, Consulta_Programa_Empleado" + Ex.Message);
            }
            return Dt_datos;
        }

        #endregion

        #region Alta-Modifciacion-eliminar

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Alta_Programa_Empleado
        ///DESCRIPCIÓN: Forma y ejecuta una consulta para insertar un registro con los datos recibidos como parámetro
        ///PARÁMETROS:
        /// 		1. Negocio: instancia de la clase de negocio con los parámetros para la consulta
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 30-jun-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Alta_Programa_Empleado(Cls_Cat_Ate_Programas_Empleado_Negocio Negocio)
        {
            string Mi_SQL;
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            int Registros_Insertados = 0;
            int Programa_Empleado_Id = 0;

            Cn.ConnectionString = Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;

            try
            {
                // obtener el máximo programa_empleado_id
                Mi_SQL = "SELECT NVL(MAX("
                    + Cat_Ate_Programas_Empleados.Campo_Programa_Empleado_ID + "),0) FROM "
                    + Cat_Ate_Programas_Empleados.Tabla_Cat_Ate_Programas_Empleados;
                Cmd.CommandText = Mi_SQL;
                int.TryParse(Cmd.ExecuteScalar().ToString(), out Programa_Empleado_Id);

                // formar consulta INSERT de Cat_Ate_Programas_Empleados
                Mi_SQL = "INSERT INTO " + Cat_Ate_Programas_Empleados.Tabla_Cat_Ate_Programas_Empleados
                    + " (" + Cat_Ate_Programas_Empleados.Campo_Programa_Empleado_ID
                    + ", " + Cat_Ate_Programas_Empleados.Campo_Empleado_ID
                    + ", " + Cat_Ate_Programas_Empleados.Campo_Programa_ID
                    + ", " + Cat_Ate_Programas_Empleados.Campo_Estatus
                    + ", " + Cat_Ate_Programas_Empleados.Campo_Usuario_Creo
                    + ", " + Cat_Ate_Programas_Empleados.Campo_Fecha_Creo
                    + ") VALUES ("
                    + "'" + (++Programa_Empleado_Id).ToString().PadLeft(5, '0') + "'"
                    + ", '" + Negocio.P_Empleado_ID + "'"
                    + ", " + Negocio.P_Programa_ID
                    + ", '" + Negocio.P_Estatus + "'"
                    + ", '" + Negocio.P_Nombre_Usuario + "'"
                    + ", SYSDATE)";
                Cmd.CommandText = Mi_SQL;
                Registros_Insertados = Cmd.ExecuteNonQuery();

                Trans.Commit();
            }
            catch (Exception Ex)
            {
                Registros_Insertados = 0;
                if (Trans != null)
                {
                    Trans.Rollback();
                }
                throw new Exception("Alta Programa_Empleado. Error: [" + Ex.Message + "]");
            }
            finally
            {
                if (Cn.State == ConnectionState.Open)
                {
                    Cn.Close();
                }
            }
            return Registros_Insertados;
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Modificar_Programa_Empleado
        ///DESCRIPCIÓN: Forma y ejecuta una consulta para actualizar un registro con los datos recibidos como parámetro
        ///PARÁMETROS:
        /// 		1. Negocio: instancia de la clase de negocio con los parámetros para la consulta
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 30-jun-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Modificar_Programa_Empleado(Cls_Cat_Ate_Programas_Empleado_Negocio Negocio)
        {
            string Mi_SQL;
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            int Registros_Actualizados = 0;

            Cn.ConnectionString = Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;

            try
            {
                Mi_SQL = "UPDATE " + Cat_Ate_Programas_Empleados.Tabla_Cat_Ate_Programas_Empleados + " SET "
                    + Cat_Ate_Programas_Empleados.Campo_Empleado_ID + "= '" + Negocio.P_Empleado_ID + "', "
                    + Cat_Ate_Programas_Empleados.Campo_Programa_ID + "= " + Negocio.P_Programa_ID + ", "
                    + Cat_Ate_Programas_Empleados.Campo_Estatus + "= '" + Negocio.P_Estatus + "', "
                    + Cat_Ate_Programas_Empleados.Campo_Usuario_Modifico + "= '" + Negocio.P_Nombre_Usuario + "', "
                    + Cat_Ate_Programas_Empleados.Campo_Fecha_Modifico + "= SYSDATE WHERE "
                    + Cat_Ate_Programas_Empleados.Campo_Programa_Empleado_ID + "= '" + Negocio.P_Programa_Empleado_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Registros_Actualizados = Cmd.ExecuteNonQuery();
                        
                Trans.Commit();
            }
            catch (Exception Ex)
            {
                if (Trans != null)
                {
                    Trans.Rollback();
                    Registros_Actualizados = 0;
                }
                throw new Exception("Error al actualizar registro: [" + Ex.Message + "]");
            }
            finally
            {
                if (Cn.State == ConnectionState.Open)
                {
                    Cn.Close();
                }
            }
            return Registros_Actualizados;
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Eliminar_Programa_Empleado
        ///DESCRIPCIÓN: Forma y ejecuta una consulta para eliminar un registro con el ID recibido como parámetro
        ///PARÁMETROS:
        /// 		1. Negocio: instancia de la clase de negocio con el parámetro para la consulta
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 30-jun-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Eliminar_Programa_Empleado(Cls_Cat_Ate_Programas_Empleado_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            int Registros_Eliminados = 0;

            Cn.ConnectionString = Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;

            try
            {
                Mi_SQL.Append("DELETE FROM " + Cat_Ate_Programas_Empleados.Tabla_Cat_Ate_Programas_Empleados);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Ate_Programas_Empleados.Campo_Programa_Empleado_ID + "='" + Negocio.P_Programa_Empleado_ID + "'");
                Cmd.CommandText = Mi_SQL.ToString();
                Registros_Eliminados = Cmd.ExecuteNonQuery();

                Trans.Commit();
            }
            catch (Exception Ex)
            {
                if (Trans != null)
                {
                    Registros_Eliminados = 0;
                }
                throw new Exception("Error al eliminar programa_empleado: [" + Ex.Message + "]");
            }
            finally
            {
                if (Cn.State == ConnectionState.Open)
                {
                    Cn.Close();
                }
            }
            return Registros_Eliminados;
        }
        #endregion
    }
}