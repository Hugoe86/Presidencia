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
using Presidencia.Actualizar_Datos_ISSEG.Negocio;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using System.Text;

namespace Presidencia.Actualizar_Datos_ISSEG.Datos
{
    public class Cls_Cat_Nom_Empl_Act_Datos_ISSEG_Datos
    {
        #region (Metodos)

        #region (Operacion)
        /// **************************************************************************************************
        /// Nombre: Actualizar_Datos_ISSEG
        /// 
        /// Descripción: Método que actualiza los datos de ISSEG del empleado.
        /// 
        /// Parámetros: Datos.- Objeto que es una instancia de la clase que transaporta la informacion de 
        ///                     de la capa de negocio a la capa de datos. 
        /// 
        /// Usuario Creó: Juan Alebrto Hernández Negrete.
        /// Fecha Creo: 13/Abril/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Modificacion:
        /// **************************************************************************************************
         internal static Boolean Actualizar_Datos_ISSEG(Cls_Cat_Nom_Empl_Act_Datos_ISSEG_Negocio Datos)
        {
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;
            StringBuilder Mi_SQL = new StringBuilder();
            Boolean Estatus = false;

            Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            Conexion.Open();

            Transaccion = Conexion.BeginTransaction();
            Comando.Connection = Conexion;
            Comando.Transaction = Transaccion;

            try
            {
                Mi_SQL.Append("Update ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" set ");

                if (!String.IsNullOrEmpty(Datos.P_Aplica_ISSEG))
                {
                    Mi_SQL.Append(Cat_Empleados.Campo_Aplica_ISSEG + "='" + Datos.P_Aplica_ISSEG + "', ");
                }
                else
                {
                    Mi_SQL.Append(Cat_Empleados.Campo_Aplica_ISSEG + "=NULL, ");
                }

                if (!String.IsNullOrEmpty(Datos.P_Fecha_Alta_ISSEG))
                {
                    Mi_SQL.Append(Cat_Empleados.Campo_Fecha_Alta_Isseg + "='" + Datos.P_Fecha_Alta_ISSEG + "'");
                }
                else
                {
                    Mi_SQL.Append(Cat_Empleados.Campo_Fecha_Alta_Isseg + "=NULL");
                }

                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Cat_Empleados.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'");

                Comando.CommandText = Mi_SQL.ToString();
                Comando.ExecuteNonQuery();
                Transaccion.Commit();
                Estatus = true;
            }
            catch (Exception Ex)
            {
                Transaccion.Rollback();
                throw new Exception(Ex.Message);
            }
            finally { Conexion.Close(); }
            return Estatus;
        }
        #endregion

        #endregion
    }
}
