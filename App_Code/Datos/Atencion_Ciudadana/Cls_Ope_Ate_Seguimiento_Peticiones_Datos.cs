using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Constantes;
using Presidencia.Seguimiento_Peticiones.Negocios;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
namespace Presidencia.Seguimiento_Peticiones.Datos
{
    public class Cls_Ope_Ate_Seguimiento_Peticiones_Datos
    {
        #region Metodos

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Alta_Seguimiento
        ///DESCRIPCIÓN: Forma y ejecuta una consulta para insertar un registro de seguimiento en Ope_Ate_Seguimiento_Peticiones
        ///         tomando en caso de haber, el Comando para transacción en la propiedad P_Comando_Oracle
        ///         Regresa un entero con el número de filas insertadas
        ///PARÁMETROS:
        /// 		1. Datos: instancia de la clase de negocio con los datos para ejecutar la consulta
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 18-may-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Alta_Seguimiento(Cls_Ope_Ate_Seguimiento_Peticiones_Negocio Datos)
        {
            int Registros_Afectados = 0;
            String Mi_SQL = "";
            Object Obj;
            OracleConnection Conexion_Consulta = new OracleConnection();
            OracleCommand Comando_Consulta = new OracleCommand();
            OracleTransaction Transaccion_Consulta = null;

            if (Datos.P_Comando_Oracle != null)
            {
                Comando_Consulta = Datos.P_Comando_Oracle;
            }
            else
            {
                Conexion_Consulta.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Conexion_Consulta.Open();
                Transaccion_Consulta = Conexion_Consulta.BeginTransaction();
                Comando_Consulta.Connection = Transaccion_Consulta.Connection;
                Comando_Consulta.Transaction = Transaccion_Consulta;
            }

            try
            {
                Mi_SQL = "SELECT NVL(MAX (" + Ope_Ate_Seguimiento_Peticiones.Campo_No_Seguimiento + "),0) " +
                         "FROM " + Ope_Ate_Seguimiento_Peticiones.Tabla_Ope_Ate_Seguimiento_Peticiones;
                Comando_Consulta.CommandText = Mi_SQL;
                Obj = Comando_Consulta.ExecuteScalar();

                if (Convert.IsDBNull(Obj) || Obj == null)
                {
                    Datos.P_Seguimiento_ID = "0000000001";
                }
                else
                {
                    Datos.P_Seguimiento_ID = string.Format("{0:0000000000}", Convert.ToInt32(Obj) + 1);
                }

                Mi_SQL = "INSERT INTO " +
                    Ope_Ate_Seguimiento_Peticiones.Tabla_Ope_Ate_Seguimiento_Peticiones + " (" +
                    Ope_Ate_Seguimiento_Peticiones.Campo_No_Seguimiento + ", " +
                    Ope_Ate_Seguimiento_Peticiones.Campo_No_Peticion + ", " +
                    Ope_Ate_Seguimiento_Peticiones.Campo_Anio_Peticion + ", " +
                    Ope_Ate_Seguimiento_Peticiones.Campo_Programa_ID + ", " +
                    Ope_Ate_Seguimiento_Peticiones.Campo_Asunto_ID + ", " +
                    Ope_Ate_Seguimiento_Peticiones.Campo_Dependencia_ID + ", " +
                    Ope_Ate_Seguimiento_Peticiones.Campo_Area_ID + ", " +
                    Ope_Ate_Seguimiento_Peticiones.Campo_Observaciones + ") VALUES ('" +
                    Datos.P_Seguimiento_ID + "', '" +
                    Datos.P_No_Peticion + "', '" +
                    Datos.P_Anio_Peticion + "', '" +
                    Datos.P_Programa_ID + "', '" +
                    Datos.P_Asunto_ID + "', '" +
                    Datos.P_Dependencia_ID + "', '" +
                    Datos.P_Area_ID + "', '" +
                    Datos.P_Observaciones + "')";

                Comando_Consulta.CommandText = Mi_SQL;
                Registros_Afectados = Comando_Consulta.ExecuteNonQuery();

                // si no se especificó un comando oracle, aplicar los cambios
                if (Datos.P_Comando_Oracle == null)
                {
                    Transaccion_Consulta.Commit();
                }
            }
            catch (Exception Ex)
            {
                // si no se especificó un comando oracle, hacer rollback de los cambios
                if (Datos.P_Comando_Oracle == null)
                {
                    Transaccion_Consulta.Rollback();
                    Registros_Afectados = 0;
                }
                // pasar la excepción al nivel anterior
                throw new Exception(Ex.Message);
            }
            finally
            {
                // si no se especificó un comando oracle, hacer rollback de los cambios
                if (Datos.P_Comando_Oracle == null)
                {
                    Conexion_Consulta.Close();
                }
            }
            return Registros_Afectados;
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Alta_Observacion
        ///DESCRIPCIÓN: Forma y ejecuta una consulta para insertar un registro de seguimiento en Ope_Ate_Observaciones_Petic
        ///         tomando en caso de haber, el Comando para transacción en la propiedad P_Comando_Oracle
        ///         Regresa un entero con el número de filas insertadas
        ///PARÁMETROS:
        /// 		1. Datos: instancia de la clase de negocio con los datos para ejecutar la consulta
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 23-may-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Alta_Observacion(Cls_Ope_Ate_Seguimiento_Peticiones_Negocio Datos)
        {
            int Registros_Afectados = 0;
            String Mi_SQL = "";
            string No_Observacion;
            Object Obj_Resultado_Consulta;
            OracleConnection Conexion_Consulta = new OracleConnection();
            OracleCommand Comando_Consulta = new OracleCommand();
            OracleTransaction Transaccion_Consulta = null;

            if (Datos.P_Comando_Oracle != null)
            {
                Comando_Consulta = Datos.P_Comando_Oracle;
            }
            else
            {
                Conexion_Consulta.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Conexion_Consulta.Open();
                Transaccion_Consulta = Conexion_Consulta.BeginTransaction();
                Comando_Consulta.Connection = Transaccion_Consulta.Connection;
                Comando_Consulta.Transaction = Transaccion_Consulta;
            }

            try
            {
                Mi_SQL = "SELECT NVL(MAX (" + Ope_Ate_Observaciones_Peticiones.Campo_No_Observacion + "),0) " +
                         "FROM " + Ope_Ate_Observaciones_Peticiones.Tabla_Ope_Ate_Observaciones_Peticiones;
                Comando_Consulta.CommandText = Mi_SQL;
                Obj_Resultado_Consulta = Comando_Consulta.ExecuteScalar();

                if (Convert.IsDBNull(Obj_Resultado_Consulta) || Obj_Resultado_Consulta == null)
                {
                    No_Observacion = "0000000001";
                }
                else
                {
                    No_Observacion = string.Format("{0:0000000000}", Convert.ToInt32(Obj_Resultado_Consulta) + 1);
                }

                Mi_SQL = "INSERT INTO " +
                    Ope_Ate_Observaciones_Peticiones.Tabla_Ope_Ate_Observaciones_Peticiones + " (" +
                    Ope_Ate_Observaciones_Peticiones.Campo_No_Observacion + ", " +
                    Ope_Ate_Observaciones_Peticiones.Campo_No_Peticion + ", " +
                    Ope_Ate_Observaciones_Peticiones.Campo_Anio_Peticion + ", " +
                    Ope_Ate_Observaciones_Peticiones.Campo_Programa_ID + ", " +
                    Ope_Ate_Observaciones_Peticiones.Campo_Observacion + ", " +
                    Ope_Ate_Observaciones_Peticiones.Campo_Estatus + ", " +
                    Ope_Ate_Observaciones_Peticiones.Campo_Fecha + ", " +
                    Ope_Ate_Observaciones_Peticiones.Campo_Usuario_Creo + ", " +
                    Ope_Ate_Observaciones_Peticiones.Campo_Fecha_Creo + ") VALUES ('" +
                    No_Observacion + "', '" +
                    Datos.P_No_Peticion + "', '" +
                    Datos.P_Anio_Peticion + "', '" +
                    Datos.P_Programa_ID + "', '" +
                    Datos.P_Observaciones + "', '" +
                    Datos.P_Estatus + "', " +
                    "SYSDATE, '" +
                    Datos.P_Usuario + "', SYSDATE)";

                Comando_Consulta.CommandText = Mi_SQL;
                Registros_Afectados = Comando_Consulta.ExecuteNonQuery();

                // si no se especificó un comando oracle, aplicar los cambios
                if (Datos.P_Comando_Oracle == null)
                {
                    Transaccion_Consulta.Commit();
                }
            }
            catch (Exception Ex)
            {
                // si no se especificó un comando oracle, hacer rollback de los cambios
                if (Datos.P_Comando_Oracle == null)
                {
                    Transaccion_Consulta.Rollback();
                    Registros_Afectados = 0;
                }
                // pasar la excepción al nivel anterior
                throw new Exception(Ex.Message);
            }
            finally
            {
                // si no se especificó un comando oracle, hacer rollback de los cambios
                if (Datos.P_Comando_Oracle == null)
                {
                    Conexion_Consulta.Close();
                }
            }
            return Registros_Afectados;
        }

        public static DataSet Consultar_Seguimiento(Cls_Ope_Ate_Seguimiento_Peticiones_Negocio Datos)
        {
            try
            {
                DataSet Registros = null;
                String Mi_SQL = "SELECT " +
                Ope_Ate_Seguimiento_Peticiones.Campo_Seguimiento_ID + ", " +
                Ope_Ate_Seguimiento_Peticiones.Campo_Peticion_ID + ", " +
                Ope_Ate_Seguimiento_Peticiones.Campo_Asunto_ID + ", " +
                Ope_Ate_Seguimiento_Peticiones.Campo_Area_ID + ", " +
                Ope_Ate_Seguimiento_Peticiones.Campo_Dependencia_ID + ", " +
                Ope_Ate_Seguimiento_Peticiones.Campo_Observaciones + ", " +
                Ope_Ate_Seguimiento_Peticiones.Campo_Fecha_Asignacion +
                " FROM " +
                Ope_Ate_Seguimiento_Peticiones.Tabla_Ope_Ate_Seguimiento_Peticiones;
                if (Datos.P_Seguimiento_ID != null)
                    Mi_SQL += " WHERE " + Ope_Ate_Seguimiento_Peticiones.Campo_Seguimiento_ID +
                    " ='" + Datos.P_Seguimiento_ID + "'";
                else
                    if (Datos.P_Peticion_ID != null)
                        Mi_SQL += " WHERE " + Ope_Ate_Seguimiento_Peticiones.Campo_Peticion_ID +
                        " ='" + Datos.P_Peticion_ID + "'";
                Registros = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                return Registros;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
        #endregion

    }
}
