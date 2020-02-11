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
using Presidencia.Reporte_Despensa.Negocio;
using System.Text;
using Presidencia.Constantes;
using Presidencia.Ayudante_Informacion;
using Presidencia.Cat_Parametros_Nomina.Negocio;
using Presidencia.Empleados.Negocios;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;

namespace Presidencia.Reporte_Despensa.Datos
{
    public class Cls_Rpt_Nom_Despensa_Datos
    {
        #region (Métodos)
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Despensa_Empleados
        /// 
        /// DESCRIPCION : Consulta la cantidad que se le pago a los empleados por concepto de espensa.
        ///               
        /// PARAMETROS  : No Aplica.
        /// 
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : Enero/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///************************************************************************************************************************
        public static DataTable Consultar_Despensas(Cls_Rpt_Nom_Despensa_Negocio Datos)
        {
            Cls_Cat_Nom_Parametros_Negocio INF_PARAMETRO = null;//Variable que almacenara la información de los parametros de nomina.
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            StringBuilder Mi_SQL_Aux = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Despensas = null;//Variable que almacena las despensas.

            try
            {
                //Consultalos parámetros de nomina.
                INF_PARAMETRO = Cls_Ayudante_Nom_Informacion._Informacion_Parametros_Nomina();

                Mi_SQL.Append("select ");

                Mi_SQL.Append(" (select ");
                Mi_SQL.Append(" ( '[' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " || '] -- ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") ");
                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "=");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + ") as EMPLEADO, ");

                Mi_SQL.Append(" (select (");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave + " || ' ' ||");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ") ");
                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion);
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + "=");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID + ") as DESPENSA, ");

                Mi_SQL.Append(" NVL(" + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." +
                    Ope_Nom_Recibos_Empleados_Det.Campo_Monto + ", 0) as " + Ope_Nom_Recibos_Empleados_Det.Campo_Monto + ", ");

                Mi_SQL.Append(" (select ");
                Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Anio);
                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas);
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID + "=");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + ") as CALENDARIO, ");

                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina);

                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + " right outer join ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + " on ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +Ope_Nom_Recibos_Empleados.Campo_No_Recibo + "=");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo);


                if (!String.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    INF_EMPLEADO = Cls_Ayudante_Nom_Informacion._Informacion_Empleado(Datos.P_No_Empleado);

                    if (Mi_SQL_Aux.ToString().Trim().ToUpper().Contains("WHERE"))
                        Mi_SQL_Aux.Append(" and " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + "='" + INF_EMPLEADO.P_Empleado_ID + "'");
                    else
                        Mi_SQL_Aux.Append(" where " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + "='" + INF_EMPLEADO.P_Empleado_ID + "'");
                }

                if (!String.IsNullOrEmpty(Datos.P_Nombre_Empleado))
                {
                    INF_EMPLEADO = Cls_Ayudante_Nom_Informacion._Informacion_Empleado_Nombre(Datos.P_Nombre_Empleado.ToUpper());

                    if (Mi_SQL_Aux.ToString().Trim().ToUpper().Contains("WHERE"))
                        Mi_SQL_Aux.Append(" and " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + "='" + INF_EMPLEADO.P_Empleado_ID + "'");
                    else
                        Mi_SQL_Aux.Append(" where " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + "='" + INF_EMPLEADO.P_Empleado_ID + "'");
                }

                if (!String.IsNullOrEmpty(Datos.P_Unidad_Responsable))
                {
                    if (Mi_SQL_Aux.ToString().Trim().ToUpper().Contains("WHERE"))
                        Mi_SQL_Aux.Append(" and " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Unidad_Responsable + "'");
                    else
                        Mi_SQL_Aux.Append(" where " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Unidad_Responsable + "'");
                }

                if (!String.IsNullOrEmpty(Datos.P_Nomina_ID))
                {
                    if (Mi_SQL_Aux.ToString().Trim().ToUpper().Contains("WHERE"))
                        Mi_SQL_Aux.Append(" and " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'");
                    else
                        Mi_SQL_Aux.Append(" where " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'");
                }

                if (!String.IsNullOrEmpty(Datos.P_No_Nomina))
                {
                    if (Mi_SQL_Aux.ToString().Trim().ToUpper().Contains("WHERE"))
                        Mi_SQL_Aux.Append(" and " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "=" + Datos.P_No_Nomina);
                    else
                        Mi_SQL_Aux.Append(" where " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "=" + Datos.P_No_Nomina);
                }

                if (!String.IsNullOrEmpty(INF_PARAMETRO.P_Percepcion_Despensa))
                {
                    if (Mi_SQL_Aux.ToString().Trim().ToUpper().Contains("WHERE"))
                        Mi_SQL_Aux.Append(" and " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID + "='" + INF_PARAMETRO.P_Percepcion_Despensa + "'");
                    else
                        Mi_SQL_Aux.Append(" where " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID + "='" + INF_PARAMETRO.P_Percepcion_Despensa + "'");
                }

                Dt_Despensas = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, (Mi_SQL.ToString() + Mi_SQL_Aux.ToString())).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las despensas. Error: [" + Ex.Message + "]");
            }
            return Dt_Despensas;
        }
        #endregion
    }
}
