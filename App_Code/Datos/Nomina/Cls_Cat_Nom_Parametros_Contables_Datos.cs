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
using Presidencia.Parametros_Contables.Negocio;
using System.Text;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

namespace Presidencia.Parametros_Contables.Datos
{
    public class Cls_Cat_Nom_Parametros_Contables_Datos
    {
        #region (Métodos)

        #region (Operación)
        /// *************************************************************************************************************************
        /// Nombre: Alta_Parametro_Contable
        /// 
        /// Descripción: Alta del Parámetro Contable.
        /// 
        /// Parámetros: No Aplica.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: Marzo/2012
        /// Usuario modifico:
        /// Fecha Modifico:
        /// Causa Modificación:
        /// *************************************************************************************************************************
        public static Boolean Alta(Cls_Cat_Nom_Parametros_Contables_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            Boolean Estatus_Operacion = false;//Estatus de la operación.
            Object PrimaryKey_Parametro_ID = null;//Objeto que almacena la llave primaria del parámetro contable.

            OracleConnection Obj_Conexion = new OracleConnection();
            OracleCommand Obj_Comando = new OracleCommand();
            OracleTransaction Obj_Transaccion = null;

            Obj_Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            Obj_Conexion.Open();

            Obj_Transaccion = Obj_Conexion.BeginTransaction();
            Obj_Comando.Connection = Obj_Conexion;
            Obj_Comando.Transaction = Obj_Transaccion;

            try
            {
                Mi_SQL.Append("SELECT NVL(MAX(" + Cat_Nom_Parametros_Contables.Campo_Parametro_ID + "),'00000') ");
                Mi_SQL.Append("FROM " + Cat_Nom_Parametros_Contables.Tabla_Cat_Nom_Parametros_Contables);

                Obj_Comando.CommandText = Mi_SQL.ToString();
                PrimaryKey_Parametro_ID = Obj_Comando.ExecuteScalar();

                if (Convert.IsDBNull(PrimaryKey_Parametro_ID))
                {
                    Datos.P_PrimaryKey_Parametro_ID = "00001";
                }
                else
                {
                    Datos.P_PrimaryKey_Parametro_ID = String.Format("{0:00000}", Convert.ToInt32(PrimaryKey_Parametro_ID) + 1);
                }

                Mi_SQL.Remove(0, Mi_SQL.Length);

                Mi_SQL.Append("insert into ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Tabla_Cat_Nom_Parametros_Contables);
                Mi_SQL.Append(" (");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Parametro_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Aportaciones_IMSS + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Aportaciones_ISSEG + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Cuotas_Fondo_Retiro + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Dietas + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Estimulos_Productividad_Eficiencia + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Gratificaciones_Fin_Anio + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Honorarios_Asimilados + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Horas_Extra + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Impuestos_Sobre_Nominas + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Participacipaciones_Vigilancia + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Pensiones + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Prestaciones + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Prestaciones_Establecidas_Condiciones_Trabajo + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Prevision_Social_Multiple + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Prima_Dominical + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Prima_Vacacional + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Remuneraciones_Eventuales + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Sueldos_Base + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Honorarios + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Seguros + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Liquidacion_Indemnizacion + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Prestaciones_Retiro + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Usuario_Creo + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Fecha_Creo);
                Mi_SQL.Append(") values(");
                Mi_SQL.Append("'" + Datos.P_PrimaryKey_Parametro_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_Aportaciones_IMSS + "', ");
                Mi_SQL.Append("'" + Datos.P_Aportaciones_ISSEG + "', ");
                Mi_SQL.Append("'" + Datos.P_Cuotas_Fondo_Retiro + "', ");
                Mi_SQL.Append("'" + Datos.P_Dietas + "', ");
                Mi_SQL.Append("'" + Datos.P_Estimulos_Productividad_Eficiencia + "', ");
                Mi_SQL.Append("'" + Datos.P_Gratificaciones_Fin_Anio + "', ");
                Mi_SQL.Append("'" + Datos.P_Honorarios_Asimilados + "', ");
                Mi_SQL.Append("'" + Datos.P_Horas_Extra + "', ");
                Mi_SQL.Append("'" + Datos.P_Impuestos_Sobre_Nominas + "', ");
                Mi_SQL.Append("'" + Datos.P_Participacipaciones_Vigilancia + "', ");
                Mi_SQL.Append("'" + Datos.P_Pensiones + "', ");
                Mi_SQL.Append("'" + Datos.P_Prestaciones + "', ");
                Mi_SQL.Append("'" + Datos.P_Prestaciones_Establecidas_Condiciones_Trabajo + "', ");
                Mi_SQL.Append("'" + Datos.P_Prevision_Social_Multiple + "', ");
                Mi_SQL.Append("'" + Datos.P_Prima_Dominical + "', ");
                Mi_SQL.Append("'" + Datos.P_Prima_Vacacional + "', ");
                Mi_SQL.Append("'" + Datos.P_Remuneraciones_Eventuales + "', ");
                Mi_SQL.Append("'" + Datos.P_Sueldos_Base + "', ");
                Mi_SQL.Append("'" + Datos.P_Honorarios + "', ");
                Mi_SQL.Append("'" + Datos.P_Seguros + "', ");
                Mi_SQL.Append("'" + Datos.P_Liquidaciones_Indemnizacion + "', ");
                Mi_SQL.Append("'" + Datos.P_Prestaciones_Retiro + "', ");
                Mi_SQL.Append("'" + Datos.P_Usuario_Creo + "', ");
                Mi_SQL.Append("SYSDATE)");

                Obj_Comando.CommandText = Mi_SQL.ToString();
                Obj_Comando.ExecuteNonQuery();
                Obj_Transaccion.Commit();

                Estatus_Operacion = true;
            }
            catch (Exception Ex)
            {
                Obj_Transaccion.Rollback();
                throw new Exception(Ex.Message);
            }
            return Estatus_Operacion;
        }
        /// *************************************************************************************************************************
        /// Nombre: Actualizar
        /// 
        /// Descripción: Actualización del Parámetro Contable.
        /// 
        /// Parámetros: No Aplica.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: Marzo/2012
        /// Usuario modifico:
        /// Fecha Modifico:
        /// Causa Modificación:
        /// *************************************************************************************************************************
        public static Boolean Actualizar(Cls_Cat_Nom_Parametros_Contables_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            Boolean Estatus_Operacion = false;//Estatus de la actualización.

            OracleConnection Obj_Conexion = new OracleConnection();
            OracleCommand Obj_Comando = new OracleCommand();
            OracleTransaction Obj_Transaccion = null;


            Obj_Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            Obj_Conexion.Open();

            Obj_Transaccion = Obj_Conexion.BeginTransaction();
            Obj_Comando.Connection = Obj_Conexion;
            Obj_Comando.Transaction = Obj_Transaccion;

            try
            {
                Mi_SQL.Append("update ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Tabla_Cat_Nom_Parametros_Contables);
                Mi_SQL.Append(" set ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Aportaciones_IMSS + "='" + Datos.P_Aportaciones_IMSS+ "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Aportaciones_ISSEG + "='" + Datos.P_Aportaciones_ISSEG + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Cuotas_Fondo_Retiro + "='" + Datos.P_Cuotas_Fondo_Retiro+ "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Dietas + "='" + Datos.P_Dietas + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Estimulos_Productividad_Eficiencia + "='" + Datos.P_Estimulos_Productividad_Eficiencia + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Gratificaciones_Fin_Anio + "='" + Datos.P_Gratificaciones_Fin_Anio + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Honorarios_Asimilados + "='" + Datos.P_Honorarios_Asimilados + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Horas_Extra + "='" + Datos.P_Horas_Extra + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Impuestos_Sobre_Nominas + "='" + Datos.P_Impuestos_Sobre_Nominas + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Participacipaciones_Vigilancia + "='" + Datos.P_Participacipaciones_Vigilancia + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Pensiones + "='" + Datos.P_Pensiones + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Prestaciones + "='" + Datos.P_Prestaciones + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Prestaciones_Establecidas_Condiciones_Trabajo + "='" + Datos.P_Prestaciones_Establecidas_Condiciones_Trabajo + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Prevision_Social_Multiple + "='" + Datos.P_Prevision_Social_Multiple + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Prima_Dominical + "='" + Datos.P_Prima_Dominical + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Prima_Vacacional + "='" + Datos.P_Prima_Vacacional + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Remuneraciones_Eventuales + "='" + Datos.P_Remuneraciones_Eventuales + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Sueldos_Base + "='" + Datos.P_Sueldos_Base + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Honorarios + "='" + Datos.P_Honorarios + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Seguros + "='" + Datos.P_Seguros + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Liquidacion_Indemnizacion + "='" + Datos.P_Liquidaciones_Indemnizacion + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Prestaciones_Retiro + "='" + Datos.P_Prestaciones_Retiro + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Usuario_Creo + "='" + Datos.P_Usuario_Creo + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Fecha_Creo + "=SYSDATE");
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Campo_Parametro_ID + "='" + Datos.P_PrimaryKey_Parametro_ID + "'");

                Obj_Comando.CommandText = Mi_SQL.ToString();
                Obj_Comando.ExecuteNonQuery();
                Obj_Transaccion.Commit();
                Estatus_Operacion = true;
            }
            catch (Exception Ex)
            {
                Obj_Transaccion.Rollback();
                throw new Exception(Ex.Message);
            }
            return Estatus_Operacion;
        }
        /// *************************************************************************************************************************
        /// Nombre: Eliminar
        /// 
        /// Descripción: Eliminar el Parámetro Contable.
        /// 
        /// Parámetros: No Aplica.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: Marzo/2012
        /// Usuario modifico:
        /// Fecha Modifico:
        /// Causa Modificación:
        /// *************************************************************************************************************************
        public static Boolean Eliminar(Cls_Cat_Nom_Parametros_Contables_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            Boolean Estatus_Operacion = false;//Estatus de la actualización.

            OracleConnection Obj_Conexion = new OracleConnection();
            OracleCommand Obj_Comando = new OracleCommand();
            OracleTransaction Obj_Transaccion = null;

            Obj_Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            Obj_Conexion.Open();

            Obj_Transaccion = Obj_Conexion.BeginTransaction();
            Obj_Comando.Connection = Obj_Conexion;
            Obj_Comando.Transaction = Obj_Transaccion;

            try
            {
                Mi_SQL.Append("delete from " + Cat_Nom_Parametros_Contables.Tabla_Cat_Nom_Parametros_Contables);
                Mi_SQL.Append(" where " + Cat_Nom_Parametros_Contables.Campo_Parametro_ID + "='" + Datos.P_PrimaryKey_Parametro_ID + "'");

                Obj_Comando.CommandText = Mi_SQL.ToString();
                Obj_Comando.ExecuteNonQuery();
                Obj_Transaccion.Commit();
                Estatus_Operacion = true;
            }
            catch (Exception Ex)
            {
                Obj_Transaccion.Rollback();
                throw new Exception(Ex.Message);
            }
            return Estatus_Operacion;
        }
        #endregion

        #region (Consulta)
        /// *************************************************************************************************************************
        /// Nombre: Consultar_Parametros_Contables
        /// 
        /// Descripción: Consultamos los Parámetros Contables.
        /// 
        /// Parámetros: No Aplica.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: Marzo/2012
        /// Usuario modifico:
        /// Fecha Modifico:
        /// Causa Modificación:
        /// *************************************************************************************************************************
        public static DataTable Consultar_Parametros_Contables()
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Parametros_Contables = null;

            try
            {
                Mi_SQL.Append("select ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Tabla_Cat_Nom_Parametros_Contables + ".* ");
                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Cat_Nom_Parametros_Contables.Tabla_Cat_Nom_Parametros_Contables);

                Dt_Parametros_Contables = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los parámetros contables. Error: [" + Ex.Message + "]");
            }
            return Dt_Parametros_Contables;
        }
        #endregion

        #endregion
    }
}
