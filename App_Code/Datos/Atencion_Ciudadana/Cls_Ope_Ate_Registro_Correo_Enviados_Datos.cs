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
using Presidencia.Operacion_Atencion_Ciudadana_Registro_Correos_Enviados.Negocio;
using Presidencia.Constantes;
using System.Text;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;


namespace Presidencia.Operacion_Atencion_Ciudadana_Registro_Correos_Enviados.Datos
{
    public class Cls_Ope_Ate_Registro_Correo_Enviados_Datos
    {
        public Cls_Ope_Ate_Registro_Correo_Enviados_Datos()
        {
        }

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
        public static Int32 Alta_Registro_Correo_Enviado(Cls_Ope_Ate_Registro_Correo_Enviados_Negocio Neg_Parametros)
        {
            String Mi_SQL;
            int No_Envio = 0;
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;
            Int32 Filas_Afectadas = 0;
            object Obj_No_Envio;

            // si llego un Comando como parametro, utilizarlo
            if (Neg_Parametros.P_Comando_Oracle != null)    // si la conexion llego como parametro, establecer como comando para utilizar
            {
                Comando = Neg_Parametros.P_Comando_Oracle;
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
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Ate_Correos_Enviados.Campo_No_Envio + "), '0')"
                    + " FROM " + Ope_Ate_Correos_Enviados.Tabla_Ope_Ate_Correos_Enviados;

                Comando.CommandText = Mi_SQL;
                Obj_No_Envio = Comando.ExecuteOracleScalar();

                // convierte a entero el número de envío
                int.TryParse(Obj_No_Envio.ToString(), out No_Envio);
                Mi_SQL = "INSERT INTO " + Ope_Ate_Correos_Enviados.Tabla_Ope_Ate_Correos_Enviados + " ("
                    + Ope_Ate_Correos_Enviados.Campo_No_Envio
                    + ", " + Ope_Ate_Correos_Enviados.Campo_Destinatario
                    + ", " + Ope_Ate_Correos_Enviados.Campo_Motivo
                    + ", " + Ope_Ate_Correos_Enviados.Campo_Fecha_Notificacion
                    + ", " + Ope_Ate_Correos_Enviados.Campo_Contribuyente_ID
                    + ", " + Ope_Ate_Correos_Enviados.Campo_Nombre_Ciudadano
                    + ", " + Ope_Ate_Correos_Enviados.Campo_Usuario_Creo
                    + ", " + Ope_Ate_Correos_Enviados.Campo_Fecha_Creo;
                Mi_SQL += ") VALUES ("
                    + "'" + (++No_Envio).ToString().PadLeft(10, '0') + "'"
                    + ", '" + Neg_Parametros.P_Destinatario + "'"
                    + ", '" + Neg_Parametros.P_Motivo + "'"
                    + ", SYSDATE"
                    + ", '" + Neg_Parametros.P_Contribuyente_ID + "'"
                    + ", '" + Neg_Parametros.P_Nombre_Contribuyente + "'"
                    + ", '" + Neg_Parametros.P_Usuario_Creo + "', SYSDATE)";

                Comando.CommandText = Mi_SQL;
                Filas_Afectadas = Comando.ExecuteNonQuery();

                if (Neg_Parametros.P_Comando_Oracle == null)    // si la conexion no llego como parametro, aplicar consultas
                {
                    Transaccion.Commit();
                }

            }
            catch (Exception Ex)
            {
                if (Neg_Parametros.P_Comando_Oracle == null && Transaccion != null)
                {
                    Transaccion.Rollback();
                    Filas_Afectadas = 0;
                }
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                if (Neg_Parametros.P_Comando_Oracle == null)
                {
                    Conexion.Close();
                }
            }

            return Filas_Afectadas;
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Consultar_Registro_Correos_Enviados
        ///DESCRIPCIÓN: Genera y ejecuta consulta para obtener los registros de correos enviados filtrados por fecha y tipo
        ///PARÁMETROS:
        ///         1. Obj_Parametros: instancia de la clase de negocio con parámetros para filtros
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 23-oct-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        internal static DataTable Consultar_Registro_Correos_Enviados(Cls_Ope_Ate_Registro_Correo_Enviados_Negocio Neg_Parametros)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            DataTable Dt_Resultado;
            try
            {
                Mi_Sql.Append("SELECT *");
                Mi_Sql.Append(" FROM " + Ope_Ate_Correos_Enviados.Tabla_Ope_Ate_Correos_Enviados);
                Mi_Sql.Append(" WHERE ");

                // agregar filtro a la consulta si se especifica como propiedad en el objeto de negocio
                if (Neg_Parametros.P_Fecha_Notificacion != DateTime.MinValue)
                {
                    Mi_Sql.Append("(TRUNC(" + Ope_Ate_Correos_Enviados.Campo_Fecha_Notificacion + ") = '"
                        + Neg_Parametros.P_Fecha_Notificacion.ToString("dd/MM/yyyy") + "'"
                        + " OR TRUNC(" + Ope_Ate_Correos_Enviados.Campo_Fecha_Notificacion + ") = '"
                        + DateTime.Today.ToString("dd/MM/yyyy") + "') AND ");
                }
                // filtro opcional por email destinatario
                if (!string.IsNullOrEmpty(Neg_Parametros.P_Destinatario))
                {
                    Mi_Sql.Append(Ope_Ate_Correos_Enviados.Campo_Destinatario + " = '"
                        + Neg_Parametros.P_Destinatario + "' AND ");
                }
                // filtro opcional por tipo de correo (motivo: felicitación y notificacion)
                if (!string.IsNullOrEmpty(Neg_Parametros.P_Motivo))
                {
                    Mi_Sql.Append(Ope_Ate_Correos_Enviados.Campo_Motivo + " = '"
                        + Neg_Parametros.P_Motivo + "' AND ");
                }
                /// filtro por rango de fechas
                if (Neg_Parametros.P_Fecha_Inicial != DateTime.MinValue)
                {
                    Mi_Sql.Append(Ope_Ate_Correos_Enviados.Campo_Fecha_Notificacion + " >= TO_DATE('"
                        + Neg_Parametros.P_Fecha_Inicial.ToString("dd/MM/yyyy") + " 00:00:00','dd/mm/yyyy hh24:mi:ss') AND ");
                }
                if (Neg_Parametros.P_Fecha_Final != DateTime.MinValue)
                {
                    Mi_Sql.Append(Ope_Ate_Correos_Enviados.Campo_Fecha_Notificacion + " <= TO_DATE('"
                        + Neg_Parametros.P_Fecha_Final.ToString("dd/MM/yyyy") + " 23:59:59','dd/mm/yyyy hh24:mi:ss') AND ");
                }
                // filtro opcional por email contribuyente
                if (!string.IsNullOrEmpty(Neg_Parametros.P_Email))
                {
                    Mi_Sql.Append("UPPER(" + Ope_Ate_Correos_Enviados.Campo_Destinatario + ") LIKE UPPER('%"
                        + Neg_Parametros.P_Email + "%') AND ");
                }
                // filtro opcional por nombre contribuyente
                if (!string.IsNullOrEmpty(Neg_Parametros.P_Nombre_Contribuyente))
                {
                    Mi_Sql.Append("UPPER(" + Ope_Ate_Correos_Enviados.Campo_Nombre_Ciudadano + ") LIKE UPPER('%"
                        + Neg_Parametros.P_Nombre_Contribuyente + "%') AND ");
                }

                // quitar AND o WHERE al final de la consulta
                if (Mi_Sql.ToString().EndsWith(" AND "))
                {
                    Mi_Sql.Length = Mi_Sql.Length - 5;
                }
                else if (Mi_Sql.ToString().EndsWith(" WHERE "))
                {
                    Mi_Sql.Length = Mi_Sql.Length - 7;
                }

                Dt_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];

                return Dt_Resultado;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

    }
}
