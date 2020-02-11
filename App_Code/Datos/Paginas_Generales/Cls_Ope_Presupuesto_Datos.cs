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
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Text;
using Presidencia.SCG_Presupuestos.Negocio;
/// <summary>
/// Summary description for Cls_Ope_Presupuesto_Datos
/// </summary>
/// 
namespace Presidencia.SCG_Presupuestos.Datos
{

    public class Cls_Ope_Presupuesto_Datos
    {
        public Cls_Ope_Presupuesto_Datos()
        {
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Insertar_Requisicion
        ///DESCRIPCIÓN: crea una sentencia sql para insertar una Requisa en la base de datos
        ///PARAMETROS: 1.-Clase de Negocio
        ///            2.-Usuario que crea la requisa
        ///CREO: 
        ///FECHA_CREO: Noviembre/2010 
        ///MODIFICO:Gustavo Angeles Cruz
        ///FECHA_MODIFICO: 25/Ene/2011
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        ///      
        public static DataTable Consultar_Presupuesto(Cls_Ope_Presupuesto_Negocio Datos)
        {
            String Mi_SQL = "" +
            "SELECT " +
            "CAT_DEPENDENCIAS.NOMBRE AS UR," +
            "CAT_SAP_FTE_FINANCIAMIENTO.DESCRIPCION FUENTE, " +
            "CAT_SAP_PROYECTOS_PROGRAMAS.DESCRIPCION PROGRAMA," +
            "CAT_SAP_PARTIDAS_ESPECIFICAS.CLAVE ||' ' || CAT_SAP_PARTIDAS_ESPECIFICAS.DESCRIPCION PARTIDA," +
            "OPE_SAP_DEP_PRESUPUESTO.MONTO_PRESUPUESTAL ASIGNADO," +
            
            "OPE_SAP_DEP_PRESUPUESTO.MONTO_AMPLIACION AMPLIACION," +
            "OPE_SAP_DEP_PRESUPUESTO.MONTO_REDUCCION REDUCCION," +
            "OPE_SAP_DEP_PRESUPUESTO.MONTO_MODIFICADO MODIFICADO," +
            "OPE_SAP_DEP_PRESUPUESTO.MONTO_DEVENGADO DEVENGADO," +
            "OPE_SAP_DEP_PRESUPUESTO.MONTO_PAGADO PAGADO," +

            "OPE_SAP_DEP_PRESUPUESTO.MONTO_COMPROMETIDO COMPROMETIDO," +
            "OPE_SAP_DEP_PRESUPUESTO.MONTO_DISPONIBLE DISPONIBLE," +
            "OPE_SAP_DEP_PRESUPUESTO.MONTO_EJERCIDO EJERCIDO " +
            "FROM OPE_SAP_DEP_PRESUPUESTO JOIN CAT_DEPENDENCIAS " +
            "ON OPE_SAP_DEP_PRESUPUESTO.DEPENDENCIA_ID = CAT_DEPENDENCIAS.DEPENDENCIA_ID " +
            "JOIN CAT_SAP_FTE_FINANCIAMIENTO " +
            "ON OPE_SAP_DEP_PRESUPUESTO.FUENTE_FINANCIAMIENTO_ID = CAT_SAP_FTE_FINANCIAMIENTO.FUENTE_FINANCIAMIENTO_ID " +
            "JOIN CAT_SAP_PROYECTOS_PROGRAMAS " +
            "ON OPE_SAP_DEP_PRESUPUESTO.PROYECTO_PROGRAMA_ID = CAT_SAP_PROYECTOS_PROGRAMAS.PROYECTO_PROGRAMA_ID " +
            "JOIN CAT_SAP_PARTIDAS_ESPECIFICAS " +
            "ON OPE_SAP_DEP_PRESUPUESTO.PARTIDA_ID = CAT_SAP_PARTIDAS_ESPECIFICAS.PARTIDA_ID " +
            "WHERE OPE_SAP_DEP_PRESUPUESTO.DEPENDENCIA_ID = '" + Datos.P_Dependencia_ID + "' " +
            "ORDER BY CAT_SAP_PARTIDAS_ESPECIFICAS.CLAVE";                
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            DataTable _DataTable = null; ;
            if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
            {
                _DataTable = _DataSet.Tables[0];
            }
            return _DataTable;
        }
    }
}