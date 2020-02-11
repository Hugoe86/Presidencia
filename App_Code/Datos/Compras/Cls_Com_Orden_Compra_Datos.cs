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
using Presidencia.Rpt_Orden_Compra.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;


/// <summary>
/// Summary description for Cls_Com_Orden_Compra_Datos
/// </summary>
/// 
namespace Presidencia.Rpt_Orden_Compra.Datos
{

    public class Cls_Com_Orden_Compra_Datos
    {

        public static DataSet Consultar_Ordenes_Compra(Cls_Com_Orden_Compra_Negocio Datos_Negocio)
        {
            String Mi_SQL = "";

            switch (Datos_Negocio.P_Tipo.Trim())
            {
                case "COMPRA DIRECTA":
                    Mi_SQL = "SELECT OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Folio +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Estatus +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Tipo_Proceso +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Tipo_Articulo +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Nombre_Proveedor +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Resguardada +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Recibo_Transitorio +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Reserva +
                        ", TO_CHAR(OPE_COM_ORDENES_COMPRA." +  Ope_Com_Ordenes_Compra.Campo_Fecha_Creo +
                        ",'DD/MON/YYYY') AS " + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo +
                        ", TO_CHAR(OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Fecha_Entrega +
                        ",'DD/MON/YYYY') AS " + Ope_Com_Ordenes_Compra.Campo_Fecha_Entrega +
                        ", NULL AS FOLIO_TIPO_COMPRA" +
                        ", OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio + " AS FOLIO_REQUISICION" +
                        ", OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio +
                        ", OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Cantidad +
                        ", OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
                        ", OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                        ", OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                        " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " OPE_COM_ORDENES_COMPRA " +
                        ", " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " OPE_COM_REQUISICIONES" +
                        ", " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " OPE_COM_REQ_PRODUCTO" +
                        " WHERE OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra +
                        "= OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_No_Orden_Compra +
                        " AND OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                        "= OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID;
                    
                    break;
                case "COTIZACION":
                    Mi_SQL = "SELECT OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Folio +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Estatus +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Tipo_Proceso +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Tipo_Articulo +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Nombre_Proveedor +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Resguardada +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Recibo_Transitorio +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Reserva +
                        ", TO_CHAR(OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo +
                        ",'DD/MON/YYYY') AS " + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo +
                        ", TO_CHAR(OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Fecha_Entrega +
                        ",'DD/MON/YYYY') AS " + Ope_Com_Ordenes_Compra.Campo_Fecha_Entrega +
                        ", OPE_COM_COTIZACION." + Ope_Com_Cotizaciones.Campo_Folio + " AS FOLIO_TIPO_COMPRA" + 
                        ", OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio + " AS FOLIO_REQUISICION" +
                        ", OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio +
                        ", OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Cantidad +
                        ", OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
                        ", OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                        ", OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                        " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " OPE_COM_ORDENES_COMPRA " +
                        ", " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " OPE_COM_REQUISICIONES" +
                        ", " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones + " OPE_COM_COTIZACION" +
                        ", " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " OPE_COM_REQ_PRODUCTO" +
                        " WHERE OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra +
                        "= OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_No_Orden_Compra +
                        " AND OPE_COM_COTIZACION." + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
                        "= OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Cotizacion + 
                        " AND OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                        "= OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID;
                    break;
                case "COMITE COMPRAS":
                    Mi_SQL = "SELECT OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Folio +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Estatus +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Tipo_Proceso +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Tipo_Articulo +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Nombre_Proveedor +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Resguardada +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Recibo_Transitorio +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Reserva +
                        ", TO_CHAR(OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo +
                        ",'DD/MON/YYYY') AS " + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo +
                        ", TO_CHAR(OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Fecha_Entrega +
                        ",'DD/MON/YYYY') AS " + Ope_Com_Ordenes_Compra.Campo_Fecha_Entrega +
                        ", OPE_COM_COMITE_COMPRA." + Ope_Com_Comite_Compras.Campo_Folio + " AS FOLIO_TIPO_COMPRA" +
                        ", OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio + " AS FOLIO_REQUISICION" +
                        ", OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio +
                        ", OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Cantidad +
                        ", OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
                        ", OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                        ", OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                        " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " OPE_COM_ORDENES_COMPRA " +
                        ", " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " OPE_COM_REQUISICIONES" +
                        ", " + Ope_Com_Comite_Compras.Tabla_Ope_Com_Comite_Compras + " OPE_COM_COMITE_COMPRA" +
                        ", " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " OPE_COM_REQ_PRODUCTO" +
                        " WHERE OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra +
                        "= OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_No_Orden_Compra +
                        " AND OPE_COM_COMITE_COMPRA." + Ope_Com_Comite_Compras.Campo_No_Comite_Compras +
                        "= OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Comite_Compras +
                        " AND OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                        "= OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID;

                    break;
                case "LICITACION":
                    Mi_SQL = "SELECT OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Folio +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Estatus +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Tipo_Proceso +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Tipo_Articulo +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Nombre_Proveedor +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Resguardada +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Recibo_Transitorio +
                        ", OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Reserva +
                        ", TO_CHAR(OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo +
                        ",'DD/MON/YYYY') AS " + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo +
                        ", TO_CHAR(OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Fecha_Entrega +
                        ",'DD/MON/YYYY') AS " + Ope_Com_Ordenes_Compra.Campo_Fecha_Entrega +
                        ", OPE_COM_LICITACION." + Ope_Com_Licitaciones.Campo_Folio + " AS FOLIO_TIPO_COMPRA" +
                        ", OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio + " AS FOLIO_REQUISICION" +
                        ", OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio +
                        ", OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Cantidad +
                        ", OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
                        ", OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                        ", OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                        " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " OPE_COM_ORDENES_COMPRA " +
                        ", " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " OPE_COM_REQUISICIONES" +
                        ", " + Ope_Com_Licitaciones.Tabla_Ope_Com_Licitaciones + " OPE_COM_LICITACION" +
                        ", " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " OPE_COM_REQ_PRODUCTO" +
                        " WHERE OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra +
                        "= OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_No_Orden_Compra +
                        " AND OPE_COM_LICITACION." + Ope_Com_Licitaciones.Campo_No_Licitacion +
                        "= OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Licitacion +
                        " AND OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                        "= OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID;
                    break;
            }

            if (Datos_Negocio.P_Tipo != null)
            {
                Mi_SQL = Mi_SQL + " AND OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Tipo_Proceso +
                    "='" + Datos_Negocio.P_Tipo + "'";
            }
            if (Datos_Negocio.P_Estatus != null)
            {
                Mi_SQL = Mi_SQL + " AND OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Estatus +
                    "='" +Datos_Negocio.P_Estatus +"'";
            }

            Mi_SQL = Mi_SQL + " AND OPE_COM_ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo +
                     " BETWEEN TO_DATE ('" + Datos_Negocio.P_Fecha_Inicial + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                     " AND TO_DATE ('" + Datos_Negocio.P_Fecha_Final + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }

    }
}