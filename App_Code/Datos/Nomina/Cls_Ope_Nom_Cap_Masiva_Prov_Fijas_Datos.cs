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
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.Text;
using Presidencia.Cap_Masiva_Prov_Fijas.Negocio;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

namespace Presidencia.Cap_Masiva_Prov_Fijas.Datos
{
    public class Cls_Ope_Nom_Cap_Masiva_Prov_Fijas_Datos
    {
        #region (Métodos)

        #region (Operación)
        /// ***************************************************************************************************
        /// Nombre: Guardar_Deducciones_Fijas
        /// 
        /// Descripción: Guarda las deducciones que le aplicaran  al empleado por concepto de algún proveedor.
        /// 
        /// Parámetros: Datos.- Objeto que almacena los datos que se usaran en la consulta.
        /// 
        /// Usuario creo: Juan ALberto Hernández Negrete.
        /// Fecha Creó: 14/Julio/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***************************************************************************************************
        internal static Boolean Guardar_Deducciones_Fijas(Cls_Ope_Nom_Cap_Masiva_Prov_Fijas_Negocio Datos)
        {
            String Mi_SQL = String.Empty;//Variable que almacenara la consulta.
            Boolean Estatus = false;//Variable que guarda el estatus de la operación realizada.

            try
            {
                if (Datos.P_Dt_Perc_Dedu_Empl is DataTable) {
                    if (Datos.P_Dt_Perc_Dedu_Empl.Rows.Count > 0) {
                        foreach (DataRow PERCEPCION_DEDUCCION in Datos.P_Dt_Perc_Dedu_Empl.Rows)
                        {
                            if (PERCEPCION_DEDUCCION is DataRow)
                            {
                                if (String.IsNullOrEmpty(PERCEPCION_DEDUCCION["DEDUCCION_ID"].ToString().Trim()))
                                    continue;

                                if (String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Empleados.Campo_Empleado_ID].ToString().Trim()))
                                    continue;

                                Mi_SQL = "UPDATE " +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det +
                                    " SET " +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad + "=" + Convert.ToDouble((String.IsNullOrEmpty(PERCEPCION_DEDUCCION["CANTIDAD"].ToString().Trim())) ? "0" : PERCEPCION_DEDUCCION["CANTIDAD"].ToString().Trim()) + ", " +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe + "=" + Convert.ToDouble((String.IsNullOrEmpty(PERCEPCION_DEDUCCION["IMPORTE"].ToString().Trim())) ? "0" : PERCEPCION_DEDUCCION["IMPORTE"].ToString().Trim()) + ", " +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo + "=" + Convert.ToDouble((String.IsNullOrEmpty(PERCEPCION_DEDUCCION["SALDO"].ToString().Trim())) ? "0" : PERCEPCION_DEDUCCION["SALDO"].ToString().Trim()) + ", " +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida + "=" + Convert.ToDouble((String.IsNullOrEmpty(PERCEPCION_DEDUCCION["CANTIDAD_RETENIDA"].ToString().Trim())) ? "0" : PERCEPCION_DEDUCCION["CANTIDAD_RETENIDA"].ToString().Trim()) + ", " +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "', " +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_No_Nomina + "=" + Convert.ToDouble((String.IsNullOrEmpty(Datos.P_No_Nomina)) ? "0" : (Datos.P_No_Nomina)) + ", " +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Referencia + "='" + Datos.P_Referencia + "'" +
                                    " WHERE " +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + "='" + PERCEPCION_DEDUCCION[Cat_Empleados.Campo_Empleado_ID].ToString().Trim() + "'" +
                                    " AND " +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID + "='" + PERCEPCION_DEDUCCION["DEDUCCION_ID"].ToString().Trim() + "'";

                                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                                Estatus = true;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al guardar las deducciones fijas de los empelados de forma masiva. Error: [" + Ex.Message + "]");
            }
            return Estatus;
        }
        #endregion

        #region (Consulta)
        /// ***************************************************************************************************
        /// Nombre: Consultar_Perc_Dedu_Empleado
        /// 
        /// Descripción: Consulta las percepciones y deducciones que le que tiene asignadas el empleado
        ///              por tipo de nomina y por sindicato.
        /// 
        /// Parámetros: Datos.- Objeto que almacena los datos que se usaran en la consulta.
        /// 
        /// Usuario creo: Juan ALberto Hernández Negrete.
        /// Fecha Creó: 14/Julio/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***************************************************************************************************
        internal static DataTable Consultar_Perc_Dedu_Empleado(Cls_Ope_Nom_Cap_Masiva_Prov_Fijas_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacena la consulta.
            DataTable Dt_Perc_Dedu_Empl = null;//Variable que almacena un listado de la tabla de percepciones y deducciones del empleado.

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + ".*");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad + " <= 0");
                Mi_SQL.Append(" ORDER BY " + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID);

                Dt_Perc_Dedu_Empl = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las percepciones y deducciones que tiene asignadas el" +
                    " empleado con alguna cantidad. Error: [" + Ex.Message + "]");
            }
            return Dt_Perc_Dedu_Empl;
        }

        internal static DataTable Consultar_Claves_Disponibles(Cls_Ope_Nom_Cap_Masiva_Prov_Fijas_Negocio Datos)
        {
            DataTable Dt_Resultado = null;
            StringBuilder Mi_SQL = new StringBuilder();

            try
            {
                Mi_SQL.Append("select ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID);
                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det);
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID + " in ");

                //add subquery.
                Mi_SQL.Append("(select ");
                Mi_SQL.Append(Cat_Nom_Proveedores_Detalles.Tabla_Cat_Nom_Proveedores_Detalles + "." + Cat_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID);
                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Cat_Nom_Proveedores_Detalles.Tabla_Cat_Nom_Proveedores_Detalles);
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Cat_Nom_Proveedores_Detalles.Tabla_Cat_Nom_Proveedores_Detalles + "." + Cat_Nom_Proveedores_Detalles.Campo_Proveedor_ID + "='" + Datos.P_Proveedor_ID + "'");
                Mi_SQL.Append(")");

                Mi_SQL.Append(" and ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'");
                Mi_SQL.Append(" and ");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad + " <= 0");
                Mi_SQL.Append(" order by " + Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID);
                Mi_SQL.Append(" asc ");

                Dt_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Dt_Resultado;
        }
        #endregion

        #endregion
    }
}
