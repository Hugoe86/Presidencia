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
using Presidencia.Cuadro_Comparativo.Negocio;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
/// <summary>
/// Summary description for Cls_Ope_Com_Cuadro_Comparativo_Datos
/// </summary>
/// 
namespace Presidencia.Cuadro_Comparativo.Datos
{
    public class Cls_Ope_Com_Cuadro_Comparativo_Datos
    {
        #region METODOS 
        public Cls_Ope_Com_Cuadro_Comparativo_Datos()
        {

        }
        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Proveedores_Que_Cotizaron
        /// DESCRIPCION:            
        /// PARAMETROS :           
        /// CREO       :            Gustavo Angeles Cruz
        /// FECHA_CREO :            6/Julio/2011  
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consultar_Proveedores_Que_Cotizaron(Cls_Ope_Com_Cuadro_Comparativo_Negocio Negocio)
        {
            DataTable Dt_Tabla = null;
            String Mi_SQL = "SELECT CAT_COM_PROVEEDORES.* FROM CAT_COM_PROVEEDORES JOIN OPE_COM_PROPUESTA_COTIZACION " +
                            "ON OPE_COM_PROPUESTA_COTIZACION.PROVEEDOR_ID = CAT_COM_PROVEEDORES.PROVEEDOR_ID " +
                            "WHERE OPE_COM_PROPUESTA_COTIZACION.NO_REQUISICION = " + Negocio.P_No_Requisicion + " AND OPE_COM_PROPUESTA_COTIZACION.PROD_SERV_ID = " +
                            "(SELECT PROD_SERV_ID FROM  OPE_COM_REQ_PRODUCTO WHERE NO_REQUISICION=" + Negocio.P_No_Requisicion + " AND ROWNUM=1) " +
                            " ORDER BY OPE_COM_PROPUESTA_COTIZACION.NO_PROPUESTA_COTIZACION ASC";
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            if (_DataSet != null && _DataSet.Tables.Count > 0)
            {
                Dt_Tabla = _DataSet.Tables[0];
            }
            return Dt_Tabla;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consultar_Precios_Cotizados
        /// DESCRIPCION:            
        /// PARAMETROS :           
        /// CREO       :            Gustavo Angeles Cruz
        /// FECHA_CREO :            6/Julio/2011  
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consultar_Precios_Cotizados(Cls_Ope_Com_Cuadro_Comparativo_Negocio Negocio)
        {
            DataTable Dt_Tabla = null;
            String Mi_SQL = "SELECT * FROM OPE_COM_PROPUESTA_COTIZACION WHERE NO_REQUISICION = " + 
                Negocio.P_No_Requisicion + " AND PROVEEDOR_ID = '" + Negocio.P_Proveedor_ID + "' ORDER BY PROD_SERV_ID ASC";
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            if (_DataSet != null && _DataSet.Tables.Count > 0)
            {
                Dt_Tabla = _DataSet.Tables[0];
            }
            return Dt_Tabla;
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consultar_Precios_Cotizados
        /// DESCRIPCION:            
        /// PARAMETROS :           
        /// CREO       :            Gustavo Angeles Cruz
        /// FECHA_CREO :            6/Julio/2011  
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static String Consultar_Dias_Credito_De_Proveedor(String Proveedor_ID)
        {
            DataTable Dt_Tabla = null;
            String Dato = "";
            String Mi_SQL = 
                "select DIAS_CREDITO from " + 
                Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " where " + 
                Cat_Com_Proveedores.Campo_Proveedor_ID + " = '" + Proveedor_ID + "'";
                
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
            {
                Dato = _DataSet.Tables[0].Rows[0]["DIAS_CREDITO"].ToString();
            }
            return Dato;
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consultar_Productos_Requisicion
        /// DESCRIPCION:            
        /// PARAMETROS :           
        /// CREO       :            Gustavo Angeles Cruz
        /// FECHA_CREO :            6/Julio/2011  
        /// MODIFICO          :     5 abril 2012
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consultar_Productos_Requisicion(Cls_Ope_Com_Cuadro_Comparativo_Negocio Negocio)
        {
            DataTable Dt_Tabla = null;
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT " + Ope_Com_Requisiciones.Campo_Tipo_Articulo + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                    " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + Negocio.P_No_Requisicion;
                Object Objeto = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Negocio.P_Tipo_Articulo = Objeto.ToString().Trim();
                Mi_SQL = "";
                if (Negocio.P_Tipo_Articulo == "PRODUCTO")
                {
                    Mi_SQL = "SELECT OPE_COM_REQ_PRODUCTO.*," +
                    "(SELECT ABREVIATURA FROM CAT_COM_UNIDADES WHERE UNIDAD_ID IN " +
                    "(SELECT UNIDAD_ID FROM CAT_COM_PRODUCTOS WHERE PRODUCTO_ID = OPE_COM_REQ_PRODUCTO.PROD_SERV_ID)) UNIDAD," +
                    "(SELECT CLAVE ||' - '|| DESCRIPCION FROM CAT_SAP_FTE_FINANCIAMIENTO WHERE FUENTE_FINANCIAMIENTO_ID = OPE_COM_REQ_PRODUCTO.FUENTE_FINANCIAMIENTO_ID) NOMBRE_FUENTE," +
                    "(SELECT NOMBRE FROM CAT_SAP_PARTIDAS_ESPECIFICAS WHERE PARTIDA_ID = OPE_COM_REQ_PRODUCTO.PARTIDA_ID) NOMBRE_PARTIDA" +
                    " FROM OPE_COM_REQ_PRODUCTO WHERE OPE_COM_REQ_PRODUCTO.NO_REQUISICION = " +
                    Negocio.P_No_Requisicion + " ORDER BY OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio + " ASC";
                }
                else if (Negocio.P_Tipo_Articulo == "SERVICIO")
                {
                    Mi_SQL = "SELECT OPE_COM_REQ_PRODUCTO.*," +
                    "'SERVICIO' AS UNIDAD," +
                    "(SELECT CLAVE ||' - '|| DESCRIPCION FROM CAT_SAP_FTE_FINANCIAMIENTO WHERE FUENTE_FINANCIAMIENTO_ID = OPE_COM_REQ_PRODUCTO.FUENTE_FINANCIAMIENTO_ID) NOMBRE_FUENTE," +
                    "(SELECT NOMBRE FROM CAT_SAP_PARTIDAS_ESPECIFICAS WHERE PARTIDA_ID = OPE_COM_REQ_PRODUCTO.PARTIDA_ID) NOMBRE_PARTIDA" +
                    " FROM OPE_COM_REQ_PRODUCTO WHERE OPE_COM_REQ_PRODUCTO.NO_REQUISICION = " +
                    Negocio.P_No_Requisicion + " ORDER BY OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio +" ASC";
                }

                DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (_DataSet != null && _DataSet.Tables.Count > 0)
                {
                    Dt_Tabla = _DataSet.Tables[0];
                }
            }
            catch(Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Dt_Tabla;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consultar_Requisicion
        /// DESCRIPCION:            
        /// PARAMETROS :           
        /// CREO       :            Gustavo Angeles Cruz
        /// FECHA_CREO :            6/Julio/2011  
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consultar_Requisicion(Cls_Ope_Com_Cuadro_Comparativo_Negocio Negocio)
        {
            DataTable Dt_Tabla = null;
            String Mi_SQL = "SELECT OPE_COM_REQ_PRODUCTO.*," +
                "(SELECT ABREVIATURA FROM CAT_COM_UNIDADES WHERE UNIDAD_ID IN " +
                "(SELECT UNIDAD_ID FROM CAT_COM_PRODUCTOS WHERE PRODUCTO_ID = OPE_COM_REQ_PRODUCTO.PROD_SERV_ID)) UNIDAD" +
            " FROM OPE_COM_REQ_PRODUCTO WHERE OPE_COM_REQ_PRODUCTO.NO_REQUISICION = " +
                Negocio.P_No_Requisicion + " ORDER BY PROD_SERV_ID ASC";
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            if (_DataSet != null && _DataSet.Tables.Count > 0)
            {
                Dt_Tabla = _DataSet.Tables[0];
            }
            return Dt_Tabla;
        }
        #endregion
    }        
}