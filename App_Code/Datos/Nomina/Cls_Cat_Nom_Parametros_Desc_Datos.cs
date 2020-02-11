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
using Presidencia.Parametros_Descuentos.Negocio;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using System.Text;

namespace Presidencia.Parametros_Descuentos.Datos
{
    public class Cls_Cat_Nom_Parametros_Desc_Datos
    {
        #region (Metodos)

        #region (Operacion)
        public static Boolean Alta(Cls_Cat_Nom_Parametros_Desc_Negocio Datos)
        {
            OracleConnection Conexion = new OracleConnection();//Variable de conexión.
            OracleCommand Comando = new OracleCommand();//Variable que ejecuta las sentencias SQL.
            OracleTransaction Transaccion = null;//Variable que ejecutara las transacciones contra la base de datos.

            Object Parametro_ID = null;
            StringBuilder Mi_SQL = new StringBuilder();
            Boolean Estatus = false;

            Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            Conexion.Open();

            Transaccion = Conexion.BeginTransaction();
            Comando.Connection = Conexion;
            Comando.Transaction = Transaccion;

            try
            {
                Mi_SQL.Append("select nvl(max(" + Cat_Nom_Parametros_Desc.Campo_Parametro_ID + "), '00000') from " + Cat_Nom_Parametros_Desc.Tabla_Cat_Nom_Parametros_Desc);

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

                Mi_SQL.Append("insert into ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Tabla_Cat_Nom_Parametros_Desc);
                Mi_SQL.Append(" (");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Campo_Parametro_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Campo_Desc_PMO_Mercados + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Campo_Desc_PMO_Tesoreria + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Campo_Desc_PMO_Corto_Plazo + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Campo_Desc_PMO_Pago_Aval + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Campo_Desc_PMO_IMUVI + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Campo_Desc_Llamadas_Tel + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Campo_Desc_Perdida_Equipo + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Campo_Desc_Otros_Fijos + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Campo_Desc_Otros_Variables + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Campo_Desc_Agua + ", ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Campo_Desc_Pago_Predial);
                Mi_SQL.Append(") values(");
                Mi_SQL.Append("'" + Datos.P_Parametro_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_Desc_PMO_Mercados + "', ");
                Mi_SQL.Append("'" + Datos.P_PMO_Tesoreria + "', ");
                Mi_SQL.Append("'" + Datos.P_PMO_Corto_Plazo + "', ");
                Mi_SQL.Append("'" + Datos.P_PMO_Pago_Aval + "', ");
                Mi_SQL.Append("'" + Datos.P_PMO_IMUVI + "', ");
                Mi_SQL.Append("'" + Datos.P_Desc_Llamadas_Tel + "', ");
                Mi_SQL.Append("'" + Datos.P_Desc_Perdida_Equipo + "', ");
                Mi_SQL.Append("'" + Datos.P_Desc_Otros_Fijos + "', ");
                Mi_SQL.Append("'" + Datos.P_Desc_Otros_Variables + "', ");
                Mi_SQL.Append("'" + Datos.P_Desc_Agua + "', ");
                Mi_SQL.Append("'" + Datos.P_Desc_Pago_Predial + "')");

                Comando.CommandText = Mi_SQL.ToString();
                Comando.ExecuteNonQuery();

                Transaccion.Commit();
                Estatus = true;
            }
            catch (Exception Ex)
            {
                Transaccion.Rollback();
                throw new Exception("Error al realizar la operacion de insercion. Error: [" + Ex.Message + "]");
            }
            return Estatus;
        }

        public static Boolean Modificar(Cls_Cat_Nom_Parametros_Desc_Negocio Datos)
        {
            OracleConnection Conexion = new OracleConnection();//Variable de conexión.
            OracleCommand Comando = new OracleCommand();//Variable que ejecuta las sentencias SQL.
            OracleTransaction Transaccion = null;//Variable que ejecutara las transacciones contra la base de datos.

            Object Parametro_ID = null;
            StringBuilder Mi_SQL = new StringBuilder();
            Boolean Estatus = false;

            Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            Conexion.Open();

            Transaccion = Conexion.BeginTransaction();
            Comando.Connection = Conexion;
            Comando.Transaction = Transaccion;

            try
            {
                Mi_SQL.Append("update ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Tabla_Cat_Nom_Parametros_Desc);
                Mi_SQL.Append(" set ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Campo_Desc_PMO_Mercados + "='" + Datos.P_Desc_PMO_Mercados + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Campo_Desc_PMO_Tesoreria + "='" + Datos.P_PMO_Tesoreria + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Campo_Desc_PMO_Corto_Plazo + "='" + Datos.P_PMO_Corto_Plazo + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Campo_Desc_PMO_Pago_Aval + "='" + Datos.P_PMO_Pago_Aval + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Campo_Desc_PMO_IMUVI + "='" + Datos.P_PMO_IMUVI + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Campo_Desc_Llamadas_Tel + "='" + Datos.P_Desc_Llamadas_Tel + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Campo_Desc_Perdida_Equipo + "='" + Datos.P_Desc_Perdida_Equipo + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Campo_Desc_Otros_Fijos + "='" + Datos.P_Desc_Otros_Fijos + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Campo_Desc_Otros_Variables + "='" + Datos.P_Desc_Otros_Variables + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Campo_Desc_Agua + "='" + Datos.P_Desc_Agua + "', ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Campo_Desc_Pago_Predial + "='" + Datos.P_Desc_Pago_Predial + "'");
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Campo_Parametro_ID + "='" + Datos.P_Parametro_ID + "'");

                Comando.CommandText = Mi_SQL.ToString();
                Comando.ExecuteNonQuery();

                Transaccion.Commit();
                Estatus = true;
            }
            catch (Exception Ex)
            {
                Transaccion.Rollback();
                throw new Exception("Error al realizar la operacion de actualización. Error: [" + Ex.Message + "]");
            }
            return Estatus;
        }

        public static Boolean Eliminar(Cls_Cat_Nom_Parametros_Desc_Negocio Datos)
        {
            OracleConnection Conexion = new OracleConnection();//Variable de conexión.
            OracleCommand Comando = new OracleCommand();//Variable que ejecuta las sentencias SQL.
            OracleTransaction Transaccion = null;//Variable que ejecutara las transacciones contra la base de datos.

            Object Parametro_ID = null;
            StringBuilder Mi_SQL = new StringBuilder();
            Boolean Estatus = false;

            Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            Conexion.Open();

            Transaccion = Conexion.BeginTransaction();
            Comando.Connection = Conexion;
            Comando.Transaction = Transaccion;

            try
            {
                Mi_SQL.Append("delete from " + Cat_Nom_Parametros_Desc.Tabla_Cat_Nom_Parametros_Desc + " where ");
                Mi_SQL.Append(Cat_Nom_Parametros_Desc.Campo_Parametro_ID + "='" + Datos.P_Parametro_ID + "'");

                Comando.CommandText = Mi_SQL.ToString();
                Comando.ExecuteNonQuery();

                Transaccion.Commit();
                Estatus = true;
            }
            catch (Exception Ex)
            {
                Transaccion.Rollback();
                throw new Exception("Error al realizar la operacion de baja. Error: [" + Ex.Message + "]");
            }
            return Estatus;
        }
        #endregion

        #region (Consulta)
        public static DataTable Consultar_Parametro()
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_ResulSet = null;

            try
            {
                Mi_SQL.Append("select * from " + Cat_Nom_Parametros_Desc.Tabla_Cat_Nom_Parametros_Desc);

                Dt_ResulSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar el parametros de descuentos. Error: [" + Ex.Message + "]");
            }
            return Dt_ResulSet;
        }
        #endregion

        #endregion
    }
}