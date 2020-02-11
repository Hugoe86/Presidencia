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
using System.Text;
using System.Collections.Generic;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Almacen_Reporte_Requisiciones_Canceladas.Negocio;

namespace Presidencia.Almacen_Reporte_Requisiciones_Canceladas.Datos
{
    public class Cls_Rpt_Alm_Requisiciones_Canceladas_Datos
    {
        #region(Metodos)
        /// ********************************************************************************************************************
        /// NOMBRE:         Consultar_Requisiciones_Canceladas
        /// COMENTARIOS:    Se realiza una consulta de las dependencias que tienen el cierto grupo de dependnecia id
        /// PARÁMETROS:     
        /// USUARIO CREÓ:   Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ:     05/Diciembre/2011 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Requisiciones_Canceladas(Cls_Rpt_Alm_Requisiciones_Canceladas_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Consulta = new DataTable();
            String SQL = "";
            try
            {         
                SQL = "" +
                "UPDATE OPE_COM_REQUISICIONES SET fecha_cancelada = " +
                "(select max(fecha_creo) from ope_com_historial_req where estatus = 'LIBERADA' " +
                "and no_requisicion = OPE_COM_REQUISICIONES.NO_REQUISICION " +
                ")" +
                "WHERE ESTATUS = 'LIBERADA'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, SQL);

                SQL = "" +
                "UPDATE OPE_COM_REQUISICIONES SET fecha_cancelada = " +
                "(select max(fecha_creo) from ope_com_historial_req where estatus = 'CANCELADA' " +
                "and no_requisicion = OPE_COM_REQUISICIONES.NO_REQUISICION " +
                ")" +
                "WHERE ESTATUS = 'CANCELADA'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, SQL);

                SQL = "" +
                " SELECT " +
                " NO_RESERVA, " +
                " FOLIO REQUISICION, ESTATUS, CODIGO_PROGRAMATICO, TOTAL TOTAL_REQUISICION, " +
                " NVL((SELECT SUM(NVL(TOTAL,0)) FROM ALM_COM_SALIDAS WHERE NO_REQUISICION = OPE_COM_REQUISICIONES.NO_REQUISICION AND ESTATUS = 'GENERADA'),0)  TOTAL_SALIDA, " +
                " NVL((TOTAL - NVL((SELECT SUM(NVL(TOTAL,0)) FROM ALM_COM_SALIDAS WHERE NO_REQUISICION = OPE_COM_REQUISICIONES.NO_REQUISICION AND ESTATUS = 'GENERADA'),0)),0) TOTAL_CANCELADO, " +
                " (select max(fecha_creo) from ope_com_historial_req where estatus = 'LIBERADA' " +
                " and no_requisicion = OPE_COM_REQUISICIONES.NO_REQUISICION " +
                " ) as FECHA_MOVIMIENTO " +
                " FROM OPE_COM_REQUISICIONES WHERE ESTATUS IN ('LIBERADA') " +
                " AND TO_DATE(TO_CHAR(FECHA_CANCELADA,'DD-MM-YYYY')) >= '" + Datos.P_Fecha_Inicial + "' " +
                " AND TO_DATE(TO_CHAR(FECHA_CANCELADA,'DD-MM-YYYY')) <= '" + Datos.P_Fecha_Final + "' " +
                " AND NO_REQUISICION IN " +
                " (SELECT NO_REQUISICION FROM OPE_COM_HISTORIAL_REQ WHERE ESTATUS = 'PARCIAL') " +
                " UNION ALL " +
                " SELECT " +
                " NO_RESERVA, " +
                " FOLIO REQUISICION, ESTATUS, CODIGO_PROGRAMATICO, TOTAL TOTAL_REQUISICION, " +
                " NVL((SELECT SUM(NVL(TOTAL,0)) FROM ALM_COM_SALIDAS WHERE NO_REQUISICION = OPE_COM_REQUISICIONES.NO_REQUISICION AND ESTATUS = 'GENERADA'),0)  TOTAL_SALIDA, " +
                " NVL((TOTAL - NVL((SELECT SUM(NVL(TOTAL,0)) FROM ALM_COM_SALIDAS WHERE NO_REQUISICION = OPE_COM_REQUISICIONES.NO_REQUISICION AND ESTATUS = 'GENERADA'),0)),0) TOTAL_CANCELADO, " +
                " (select max(fecha_creo) from ope_com_historial_req where estatus = 'CANCELADA' " +
                " and no_requisicion = OPE_COM_REQUISICIONES.NO_REQUISICION " +
                " ) as FECHA_MOVIMIENTO " +
                " FROM OPE_COM_REQUISICIONES WHERE ESTATUS IN ('CANCELADA') " +
                " AND TO_DATE(TO_CHAR(FECHA_CANCELADA,'DD-MM-YYYY')) >= '" + Datos.P_Fecha_Inicial + "' " +
                " AND TO_DATE(TO_CHAR(FECHA_CANCELADA,'DD-MM-YYYY')) <= '" + Datos.P_Fecha_Final + "' " +
                " AND NO_REQUISICION IN " +
                " (SELECT NO_REQUISICION FROM OPE_COM_HISTORIAL_REQ WHERE ESTATUS = 'ALMACEN') ";
                Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, SQL).Tables[0];


            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Movimientos Presupuestales que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
            return Dt_Consulta;
        }
        /// ********************************************************************************************************************
        /// NOMBRE:         Consultar_Motivo_Canceladas
        /// COMENTARIOS:    Se realiza una consulta de los motivos por el que se cancelo la requisicion
        /// PARÁMETROS:     
        /// USUARIO CREÓ:   Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ:     05/Diciembre/2011 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Motivo_Canceladas(Cls_Rpt_Alm_Requisiciones_Canceladas_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Mi_SQL.Append("Select * from " + Ope_Com_Req_Observaciones.Tabla_Ope_Com_Req_Observaciones);
                Mi_SQL.Append(" where " + Ope_Com_Req_Observaciones.Campo_Requisicion_ID + "='" + Datos.P_No_Requisicion + "'");
                Mi_SQL.Append(" order by " + Ope_Com_Req_Observaciones.Campo_Requisicion_ID);
                Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Movimientos Presupuestales que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
            return Dt_Consulta;
        }
        #endregion

    }
}
