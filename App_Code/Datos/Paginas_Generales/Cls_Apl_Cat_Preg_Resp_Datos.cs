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
using Presidencia.Preguntas_Respuestas.Negocio;
using System.Text;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;

namespace Presidencia.Preguntas_Respuestas.Datos
{
    public class Cls_Apl_Cat_Preg_Resp_Datos
    {
        #region (Métodos)

        #region (Operación)
        internal static Boolean Alta_Preguntas(Cls_Apl_Cat_Preg_Resp_Negocios Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            Boolean Estatus = false;
            OracleTransaction Transaccion = null;////Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            OracleConnection Conexion;//Variable para la conexión para la base de datos   
            OracleCommand Comando;//Sirve para la ejecución de las operaciones a la base de datos
            Object Preg_Resp_ID;//Identificador unico de la tabla de bancos.

            Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
            Comando = new OracleCommand();
            Conexion.Open();
            Transaccion = Conexion.BeginTransaction();
            Comando.Transaction = Transaccion;
            Comando.Connection = Conexion;

            try
            {
                //Consultas para el ID
                Mi_SQL.Append("SELECT NVL(MAX(" + Apl_Cat_Preg_Resp.Campo_Preg_Resp_ID + "), '00000000000000000000') FROM " + Apl_Cat_Preg_Resp.Tabla_Apl_Cat_Preg_Resp);

                //Ejecutar consulta
                Comando.CommandText = Mi_SQL.ToString();
                Preg_Resp_ID = Comando.ExecuteScalar();

                //Verificar si no es nulo
                if (Convert.IsDBNull(Preg_Resp_ID) == false)
                    Datos.P_Preg_Resp_ID = String.Format("{0:00000000000000000000}", Convert.ToInt32(Preg_Resp_ID) + 1);
                else
                    Datos.P_Preg_Resp_ID = "00000000000000000001";

                Mi_SQL = new StringBuilder();

                Mi_SQL.Append("INSERT INTO " + Apl_Cat_Preg_Resp.Tabla_Apl_Cat_Preg_Resp + "(");
                Mi_SQL.Append(Apl_Cat_Preg_Resp.Campo_Preg_Resp_ID + ", ");
                Mi_SQL.Append(Apl_Cat_Preg_Resp.Campo_Pregunta + ", ");
                Mi_SQL.Append(Apl_Cat_Preg_Resp.Campo_Usuario_Creo + ", ");
                Mi_SQL.Append(Apl_Cat_Preg_Resp.Campo_Fecha_Creo + ") VALUES(");
                Mi_SQL.Append("'" + Datos.P_Preg_Resp_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_Pregunta + "', ");
                Mi_SQL.Append("'" + Datos.P_Usuario_Creo + "', SYSDATE)");

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                Estatus = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al insertar la pregunta. Error: [" + Ex.Message + "]");
            }
            return Estatus;
        }

        internal static Boolean Modificar_Preguntas(Cls_Apl_Cat_Preg_Resp_Negocios Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            Boolean Estatus = false;

            try
            {
                Mi_SQL.Append("UPDATE " + Apl_Cat_Preg_Resp.Tabla_Apl_Cat_Preg_Resp + " SET ");
                Mi_SQL.Append(Apl_Cat_Preg_Resp.Campo_Respuesta + "='" + Datos.P_Respuesta + "', ");
                Mi_SQL.Append(Apl_Cat_Preg_Resp.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Modifico + "', ");
                Mi_SQL.Append(Apl_Cat_Preg_Resp.Campo_Fecha_Modifico + "=SYSDATE");
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Apl_Cat_Preg_Resp.Campo_Preg_Resp_ID + "='" + Datos.P_Preg_Resp_ID + "'");

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                Estatus = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al modificar la pregunta. Error: [" + Ex.Message + "]");
            }
            return Estatus;
        }
        #endregion

        #region (Consultas)
        internal static DataTable Consultar_Preguntas(Cls_Apl_Cat_Preg_Resp_Negocios Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            Boolean Estatus = false;
            DataTable Dt_Listado_Preguntas = null;

            try
            {
                Mi_SQL.Append("SELECT " + Apl_Cat_Preg_Resp.Tabla_Apl_Cat_Preg_Resp + ".*");
                Mi_SQL.Append(" FROM " + Apl_Cat_Preg_Resp.Tabla_Apl_Cat_Preg_Resp);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append("UPPER(" + Apl_Cat_Preg_Resp.Campo_Pregunta + ") LIKE UPPER('%" + Datos.P_Pregunta + "%')");

                Dt_Listado_Preguntas = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                Estatus = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al modificar la pregunta. Error: [" + Ex.Message + "]");
            }
            return Dt_Listado_Preguntas;
        }
        #endregion

        #endregion
    }
}
