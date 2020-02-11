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
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using System.Text;
using Presidencia.Reporte_Mensual_Ordenes_Compra.Negocio;

namespace Presidencia.Reporte_Mensual_Ordenes_Compra.Datos
{
    public class Cls_Rpt_Com_Mensual_Orden_Compra_Datos
    {
        #region METODOS
            ///*****************************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Ordenes_Compra
            ///DESCRIPCIÓN          : Consulta para obtener las ordenes de compreas
            ///PARAMETROS           1 Orden_Compra_Negocio: Conexion con la capa de negocios
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 28/Diciembre/2011
            ///MODIFICO             : 
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*****************************************************************************************************************
            internal static DataTable Consultar_Ordenes_Compra(Cls_Rpt_Com_Mensual_Orden_Compra_Negocio Orden_Compra_Negocio)
            {
                StringBuilder Mi_Sql = new StringBuilder();
                try
                {
                    //OBTENEMOS LAS DEPENDENCIAS DEL CATALOGO
                    Mi_Sql.Append("SELECT " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Folio + ",");
                    Mi_Sql.Append(Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + ", ");
                    Mi_Sql.Append(Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Estatus + ", ");
                    Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " || ' ' || ");
                    Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS UNIDAD_RESPONSABLE, ");
                    Mi_Sql.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " || ' ' || ");
                    Mi_Sql.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + " AS FUENTE_FINANCIAMIENTO, ");
                    Mi_Sql.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Clave + " || ' ' || ");
                    Mi_Sql.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Nombre + " AS PARTIDA_ESPECIFICA, ");
                    Mi_Sql.Append(Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio + " AS CONCEPTO, ");
                    Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " || ' ' || ");
                    Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                    Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " AS COTIZADOR, ");
                    Mi_Sql.Append(Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Nombre_Proveedor + ", ");
                    Mi_Sql.Append(Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "." + Ope_Com_Facturas_Proveedores.Campo_Fecha_Creo + " AS FECHA_CONTRARECIBO, ");
                    Mi_Sql.Append(Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "." + Ope_Com_Facturas_Proveedores.Campo_Fecha_Pago + " AS FECHA_PAGO, ");
                    Mi_Sql.Append(Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Total + " AS TOTALES, ");
                    Mi_Sql.Append(" '' AS TOTAL, '' AS OBSERVACIONES ");
                    Mi_Sql.Append(" FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra);
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores);
                    Mi_Sql.Append(" ON " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + ".NO_CONTRA_RECIBO");
                    Mi_Sql.Append("=" + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ".NO_CONTRA_RECIBO");
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones);
                    Mi_Sql.Append(" ON " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID);
                    Mi_Sql.Append(" = " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones);

                    Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias);
                    Mi_Sql.Append(" ON " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID);
                    Mi_Sql.Append(" = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto);
                    Mi_Sql.Append(" ON " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Requisicion_ID);
                    Mi_Sql.Append(" = " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID);
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento);
                    Mi_Sql.Append(" ON " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID);
                    Mi_Sql.Append(" = " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID);
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas);
                    Mi_Sql.Append(" ON " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Partida_ID);
                    Mi_Sql.Append(" = " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID);
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados);
                    Mi_Sql.Append(" ON " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Cotizador_ID);
                    Mi_Sql.Append(" = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                    Mi_Sql.Append(" WHERE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID);
                    Mi_Sql.Append(" = (SELECT MAX(" + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID + ")" );
                    Mi_Sql.Append(" FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto);
                    Mi_Sql.Append(" WHERE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Requisicion_ID);
                    Mi_Sql.Append(" = " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + ")");

                    if (!String.IsNullOrEmpty(Orden_Compra_Negocio.P_Fecha_Inicio) && !String.IsNullOrEmpty(Orden_Compra_Negocio.P_Fecha_Fin))
                    {
                        Mi_Sql.Append(" AND " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "." + Ope_Com_Facturas_Proveedores.Campo_Fecha_Pago);
                            Mi_Sql.Append(" BETWEEN TO_DATE ('" + Orden_Compra_Negocio.P_Fecha_Inicio + " 00:00:00', 'DD/MM/YYYY HH24:MI:SS')");
                            Mi_Sql.Append(" AND TO_DATE('" + Orden_Compra_Negocio.P_Fecha_Fin + "23:59:00', 'DD/MM/YYYY HH24:MI:SS')");
                    }

                    if (!String.IsNullOrEmpty(Orden_Compra_Negocio.P_Estatus))
                    {
                            Mi_Sql.Append(" AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Estatus);
                            Mi_Sql.Append(" = '" + Orden_Compra_Negocio.P_Estatus + "'");
                    }

                    Mi_Sql.Append(" ORDER BY " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " ASC");

                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros de las ordenes de compras. Error: [" + Ex.Message + "]");
                }
            }

            ///*****************************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Estatus
            ///DESCRIPCIÓN          : Consulta para obtener el estatus de las ordenes de compras
            ///PARAMETROS           :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 28/Diciembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*****************************************************************************************************************
            internal static DataTable Consultar_Estatus()
            {
                StringBuilder Mi_Sql = new StringBuilder();
                try
                {
                    //OBTENEMOS LAS DEPENDENCIAS DEL CATALOGO
                    Mi_Sql.Append("SELECT DISTINCT " + Ope_Com_Ordenes_Compra.Campo_Estatus);
                    Mi_Sql.Append(" FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra);
                    Mi_Sql.Append(" ORDER BY " + Ope_Com_Ordenes_Compra.Campo_Estatus + " ASC");

                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros de las ordenes de compras. Error: [" + Ex.Message + "]");
                }
            }
        #endregion
    }
}
