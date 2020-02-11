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
using Presidencia.Sessiones;
using System.Text;
using Presidencia.Captura_Masiva_Perc_Deduc_Fijas.Negocio;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

namespace Presidencia.Captura_Masiva_Perc_Deduc_Fijas.Datos
{
    public class Cls_Ope_Nom_Cap_Masiva_Perc_Dedu_Fijas_Datos
    {
        #region (Metodos)

        #region (Operacion)
        /// ***********************************************************************************************************
        /// Nombre: Alta_Percepciones_Deducciones_Fijas
        /// 
        /// Descripción: Ejecuta el alta de conceptos fijos de los empleados de forma masiva.
        /// 
        /// Parámetros: Datos.- Parámetro de tipo objeto que encapsula los datos a usar en la consulta.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 13/Julio/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Modificación:
        /// ***********************************************************************************************************
        internal static Boolean Alta_Percepciones_Deducciones_Fijas(Cls_Ope_Nom_Cap_Masiva_Perc_Dedu_Fijas_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            Double Cantidad = 0.0;//Cantidad a retener a los empleados.
            Boolean Operacion_Completa = false;//Variable que almacena el estatus de la operación.

            try
            {
                if (Datos.P_Dt_Empleados is DataTable)
                {
                    if (Datos.P_Dt_Empleados.Rows.Count > 0)
                    {
                        foreach (DataRow EMPLEADO in Datos.P_Dt_Empleados.Rows)
                        {
                            if (EMPLEADO is DataRow)
                            {
                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString().Trim()))
                                    Cantidad = Convert.ToDouble(EMPLEADO[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString().Trim());

                                Mi_SQL.Append("UPDATE ");
                                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det);
                                Mi_SQL.Append(" SET ");
                                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad + "=" + Cantidad + ", ");
                                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Referencia + "='" + Datos.P_Referencia + "', ");
                                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "', ");
                                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_No_Nomina + "=" + Datos.P_No_Nomina);
                                Mi_SQL.Append(" WHERE ");
                                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID + "='" + Datos.P_Percepcion_Deduccion_ID.ToString().Trim() + "'");
                                Mi_SQL.Append(" AND ");
                                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + "='" + 
                                    EMPLEADO[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID].ToString().Trim() + "'");

                                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                                Mi_SQL = new StringBuilder();//Limpiamos la variable.
                            }
                        }
                    }
                }

                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error guardar los cambios al aplicar los conceptos fijos al empleado. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }
        #endregion

        #endregion
    }
}
