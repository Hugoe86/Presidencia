using System;
using System.IO;
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
using System.Text;
using System.Collections.Generic;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Reportes_Nomina_General.Negocio;

namespace Presidencia.Reportes_Nomina_General.Datos
{
    public class Cls_Rpt_Nom_Nomina_General_Datos
    {
        #region Consulta
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Nomina_General
        /// DESCRIPCION : Consulta las deducciones fijas que existan en la base de datos
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        :Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 09/Abril/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Nomina_General(Cls_Rpt_Nom_Nomina_General_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta para del Empleado
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Mi_SQL.Append("Select ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave + ",");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ",");
                Mi_SQL.Append("sum(" + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Monto + ") as Importe");

                Mi_SQL.Append(" From " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion);


                Mi_SQL.Append("  left outer join " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det);
                Mi_SQL.Append(" on " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID);
                Mi_SQL.Append("=" + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID);
                
                Mi_SQL.Append(" left outer join ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + " on ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo);
                Mi_SQL.Append("=" + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Recibo);

                Mi_SQL.Append(" where ");

                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Datos.P_Nomina_id + "'");
                Mi_SQL.Append(" and " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "=" + Datos.P_No_Nomina+ "");
                Mi_SQL.Append(" and " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Tipo + "='" + Datos.P_Tipo + "'");

                Mi_SQL.Append(" group by " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave + ",");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Nombre);

                Mi_SQL.Append(" order by " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave);

                Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                return Dt_Consulta;
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        #endregion
    }
}
