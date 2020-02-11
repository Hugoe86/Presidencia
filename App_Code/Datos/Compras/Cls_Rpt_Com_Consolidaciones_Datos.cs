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
using Presidencia.Rpt_Com_Consolidaciones.Negocio;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Sessiones;


/// <summary>
/// Summary description for Cls_Rpt_Com_Consolidaciones_Datos
/// </summary>

namespace Presidencia.Rpt_Com_Consolidaciones.Datos
{
    public class Cls_Rpt_Com_Consolidaciones_Datos
    {

        public static DataSet Consultar_Consolidaciones(Cls_Rpt_Com_Consolidaciones_Negocio Datos_Consolidaciones)
        {
            String Mi_SQL = "SELECT OPE_COM_CONSOLIDACIONES." + Ope_Com_Consolidaciones.Campo_Folio +
                ", OPE_COM_CONSOLIDACIONES." + Ope_Com_Consolidaciones.Campo_Estatus +
                ", OPE_COM_CONSOLIDACIONES." + Ope_Com_Consolidaciones.Campo_Tipo +
                ", OPE_COM_CONSOLIDACIONES." + Ope_Com_Consolidaciones.Campo_Fecha_Creo +
                ", OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio + " AS FOLIO_REQUISICION" +
                ", OPE_COM_REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio +
                ", OPE_COM_REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Tipo + " AS TIPO_PRODUCTO" +
                ", OPE_COM_REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Cantidad +
                ", OPE_COM_REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Importe + " AS PRECIO_U_SIN_I" +
                ", OPE_COM_REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Monto_Total + " AS TOTAL_CON_I" +
                ", OPE_COM_REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado + " AS PRECIO_U_CI_COTIZADO" +
                ", OPE_COM_REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                " FROM " + Ope_Com_Consolidaciones.Tabla_Ope_Com_Consolidaciones + " OPE_COM_CONSOLIDACIONES" +
                ", " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " OPE_COM_REQUISICIONES" +
                ", " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " OPE_COM_REQ_PRODUCTOS" +
                " WHERE OPE_COM_CONSOLIDACIONES." + Ope_Com_Consolidaciones.Campo_No_Consolidacion +
                "= OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_No_Consolidacion +
                " AND OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                "= OPE_COM_REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Requisicion_ID;

            if (Datos_Consolidaciones.P_Tipo != null)
            {
                Mi_SQL = Mi_SQL + " AND OPE_COM_CONSOLIDACIONES." + Ope_Com_Consolidaciones.Campo_Tipo + "='" + Datos_Consolidaciones.P_Tipo + "'";

            }
            if (Datos_Consolidaciones.P_Estatus != null)
            {
                Mi_SQL = Mi_SQL + " AND OPE_COM_CONSOLIDACIONES." + Ope_Com_Consolidaciones.Campo_Estatus + "='" + Datos_Consolidaciones.P_Estatus+"'";
            }

            Mi_SQL = Mi_SQL + " AND OPE_COM_CONSOLIDACIONES." + Ope_Com_Consolidaciones.Campo_Fecha_Creo + 
                    " BETWEEN TO_DATE ('" + Datos_Consolidaciones.P_Fecha_Inicial + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                    " AND TO_DATE ('" + Datos_Consolidaciones.P_Fecha_Final + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }



    }
}