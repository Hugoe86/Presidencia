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
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Cargar_Presupuesto_Calendarizado.Negocio;
/// <summary>
/// Summary description for Cls_Ope_Psp_Cargar_Presup_Calendarizado_Datos
/// </summary>
/// 
namespace Presidencia.Cargar_Presupuesto_Calendarizado.Datos
{
    public class Cls_Ope_Psp_Cargar_Presup_Calendarizado_Datos
    {
        #region VARIABLES

        public static String TIPO_CALENDARIZADO = "CALENDARIZADO";
        public static String TIPO_APROBADO = "APROBADO";

        public Cls_Ope_Psp_Cargar_Presup_Calendarizado_Datos()
        {
        }
        public int Consultar_Presupuesto_Calendarizado()
        {
            return 0;
        }
        public static DataTable Consultar_Anios_Presupuestados()
        {
            String Mi_SQL = "";
            DataTable Dt_Anios = null;
            try
            {
                    Mi_SQL = "SELECT DISTINCT (" + Ope_Psp_Calendarizacion_Presu.Campo_Anio + ") FROM " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu;
                    Mi_SQL += " ORDER BY " + Ope_Psp_Calendarizacion_Presu.Campo_Anio + " DESC";
                    Dt_Anios = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString() + "[NO SE PUDO CONSULTAR PRESUPUESTO]");
            }
            return Dt_Anios;
        }
        public static double Consultar_Importe_Presupuesto_Aprobado(Cls_Ope_Psp_Cargar_Presup_Calendarizado_Negocio Negocio)
        {
            String Mi_SQL = "";
            double Importe = 0;
            try
            {
                Mi_SQL = "SELECT SUM (" + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Total + ") FROM " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu;
                Mi_SQL += " WHERE " + Ope_Psp_Calendarizacion_Presu.Campo_Anio + " = " + Negocio.P_Anio;
                Object Objeto = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Importe = Convert.ToDouble(Objeto);
            }
            catch (Exception Ex)
            {
                Importe = 0;
                //throw new Exception(Ex.ToString() + "[NO SE PUDO CONSULTAR PRESUPUESTO]");
            }
            return Importe;
        }
        #endregion
    }
}