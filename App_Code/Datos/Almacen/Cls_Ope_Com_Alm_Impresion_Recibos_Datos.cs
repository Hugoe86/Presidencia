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
using System.Data.OracleClient;
using System.Xml.Linq;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Bitacora_Eventos;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Almacen_Impresion_Recibos.Negocio;


/// <summary>
/// Summary description for Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Datos
/// </summary>
/// 
namespace Presidencia.Almacen_Impresion_Recibos.Datos
{
    public class Cls_Ope_Com_Alm_Impresion_Recibos_Datos
    {
        #region (Variables Locales)

        #endregion

        #region (Variables Publicas)

        #endregion

        #region (Métodos)

            
        #region (Métodos Recibos Transitorios)

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consultar_Recibos_Transitorios
        ///DESCRIPCIÓN:             Método el cual es utilizado para consultar los recibos transitorios                  
        ///PARAMETROS:              Datos: Parametro de la capa de negocios que contiene la información
        ///                         utilizada para realizar la consulta.
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              29/Marzo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Recibos_Transitorios(Cls_Ope_Com_Alm_Impresion_Recibos_Negocio Datos)
        {
            //Declaracion de Variables
            String Mi_SQL = String.Empty;
            DataTable Dt_Recibos_Transitorios = new DataTable();

            try
            {
                // Consulta de los recibos transitorios
                Mi_SQL = "SELECT RECIBOS_TRANSITORIOS." + Ope_Alm_Recibos_Transitorios.Campo_No_Recibo + ", ";
                Mi_SQL = Mi_SQL + " RECIBOS_TRANSITORIOS." + Ope_Alm_Recibos_Transitorios.Campo_Tipo + ", ";
                Mi_SQL = Mi_SQL + " RECIBOS_TRANSITORIOS." + Ope_Alm_Recibos_Transitorios.Campo_Fecha_Creo + ", ";
                Mi_SQL = Mi_SQL + " RECIBOS_TRANSITORIOS." + Ope_Alm_Recibos_Transitorios.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + " ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + ", ";
                Mi_SQL = Mi_SQL + " ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Folio + ", ";
                Mi_SQL = Mi_SQL + " ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Total+ ", ";
                Mi_SQL = Mi_SQL + " RECIBOS_TRANSITORIOS." + Ope_Alm_Recibos_Transitorios.Campo_No_Contra_Recibo + ", ";

                Mi_SQL = Mi_SQL + "(select " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "";
                Mi_SQL = Mi_SQL +  " WHERE ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra+ " = ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_No_Orden_Compra + ") as NO_REQUISICION, ";

                Mi_SQL = Mi_SQL + "(select " + Cat_Com_Proveedores.Campo_Nombre + " FROM ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores+ "";
                Mi_SQL = Mi_SQL +  " WHERE ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID + " = ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Proveedor_ID + ") as PROVEEDOR ";

                Mi_SQL = Mi_SQL + " FROM " + Ope_Alm_Recibos_Transitorios.Tabla_Ope_Alm_Recibos_Transitorios + " RECIBOS_TRANSITORIOS, ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDENES_COMPRA ";

                Mi_SQL = Mi_SQL + " WHERE ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno;
                Mi_SQL = Mi_SQL + " = RECIBOS_TRANSITORIOS." + Ope_Alm_Recibos_Transitorios.Campo_No_Contra_Recibo;

                if (Datos.P_No_Recibo != null)
                {
                    Mi_SQL = Mi_SQL + " AND RECIBOS_TRANSITORIOS." + Ope_Alm_Recibos_Transitorios.Campo_No_Recibo + " like '%" + Datos.P_No_Recibo + "%'";
                }

                if (Datos.P_No_Orden_Compra != null)
                {
                    Mi_SQL = Mi_SQL + " AND  ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " like '%" + Datos.P_No_Orden_Compra + "%'";
                }

                if (Datos.P_No_Requisicion != null)
                {
                    Mi_SQL = Mi_SQL + " AND ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + " like '%" + Datos.P_No_Requisicion + "%'";
                }

                if (Datos.P_Proveedor != null)
                {
                    Mi_SQL = Mi_SQL + " AND ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID + " = '" + Datos.P_Proveedor + "'";
                }

                if ((Datos.P_Fecha_Inicial != null) && (Datos.P_Fecha_Final != null))
                {
                    Mi_SQL = Mi_SQL + " AND TO_DATE(TO_CHAR( RECIBOS_TRANSITORIOS." + Ope_Alm_Recibos_Transitorios.Campo_Fecha_Creo + ",'DD/MM/YY')) BETWEEN '" + 
                    string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Inicial))  + "'" +
                   " AND '" +
                   string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Final)) + "'";
                }

                Mi_SQL = Mi_SQL + " order by RECIBOS_TRANSITORIOS." + Ope_Alm_Recibos_Transitorios.Campo_No_Recibo;
                
                Dt_Recibos_Transitorios = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
  
            return Dt_Recibos_Transitorios;
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consulta_Recibos_Transitorios_Totalidad
        ///DESCRIPCIÓN:             Método el cual es utilizado para consultar los recibos transitorios por totalidad                 
        ///PARAMETROS:              Datos: Parametro de la capa de negocios que contiene la información
        ///                         utilizada para realizar la consulta.
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              26/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Recibos_Transitorios_Totalidad(Cls_Ope_Com_Alm_Impresion_Recibos_Negocio Datos)
        {
            //Declaracion de Variables
            String Mi_SQL = String.Empty;
            DataTable Dt_Recibos_Transitorios = new DataTable();

            try
            {
                // Consulta de los recibos transitorios
                Mi_SQL = "SELECT RECIBOS_TRANSITORIOS." + Ope_Alm_Recibos_Transitorios_Totalidad.Campo_No_Recibo + ", ";
                Mi_SQL = Mi_SQL + " RECIBOS_TRANSITORIOS." + Ope_Alm_Recibos_Transitorios_Totalidad.Campo_Tipo + ", ";
                Mi_SQL = Mi_SQL + " RECIBOS_TRANSITORIOS." + Ope_Alm_Recibos_Transitorios_Totalidad.Campo_Fecha_Creo + ", ";
                Mi_SQL = Mi_SQL + " RECIBOS_TRANSITORIOS." + Ope_Alm_Recibos_Transitorios_Totalidad.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + " ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + ", ";
                Mi_SQL = Mi_SQL + " ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Folio + ", ";
                Mi_SQL = Mi_SQL + " ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Total+ ", ";
                Mi_SQL = Mi_SQL + " RECIBOS_TRANSITORIOS." + Ope_Alm_Recibos_Transitorios_Totalidad.Campo_No_Contra_Recibo + ", ";

                Mi_SQL = Mi_SQL + "(select " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "";
                Mi_SQL = Mi_SQL +  " WHERE ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra+ " = ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_No_Orden_Compra + ") as NO_REQUISICION, ";

                Mi_SQL = Mi_SQL + "(select " + Cat_Com_Proveedores.Campo_Nombre + " FROM ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores+ "";
                Mi_SQL = Mi_SQL +  " WHERE ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID + " = ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Proveedor_ID + ") as PROVEEDOR ";

                Mi_SQL = Mi_SQL + " FROM " + Ope_Alm_Recibos_Transitorios_Totalidad.Tabla_Ope_Alm_Recibos_Transitorios_Totalidad + " RECIBOS_TRANSITORIOS, ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDENES_COMPRA ";

                Mi_SQL = Mi_SQL + " WHERE ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno;
                Mi_SQL = Mi_SQL + " = RECIBOS_TRANSITORIOS." + Ope_Alm_Recibos_Transitorios_Totalidad.Campo_No_Contra_Recibo;

                if (Datos.P_No_Recibo != null)
                {
                    Mi_SQL = Mi_SQL + " AND RECIBOS_TRANSITORIOS." + Ope_Alm_Recibos_Transitorios_Totalidad.Campo_No_Recibo + " like '%" + Datos.P_No_Recibo + "%'";
                }
                if (Datos.P_No_Orden_Compra != null)
                {
                    Mi_SQL = Mi_SQL + " AND  ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " like '%" + Datos.P_No_Orden_Compra + "%'";
                }
                if (Datos.P_No_Requisicion != null)
                {
                    Mi_SQL = Mi_SQL + " AND ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + " like '%" + Datos.P_No_Requisicion + "%'";
                }
                if (Datos.P_Proveedor != null)
                {
                    Mi_SQL = Mi_SQL + " AND ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID + " = '" + Datos.P_Proveedor + "'";
                }
                if ((Datos.P_Fecha_Inicial != null) && (Datos.P_Fecha_Final != null))
                {
                    Mi_SQL = Mi_SQL + " AND TO_DATE(TO_CHAR( RECIBOS_TRANSITORIOS." + Ope_Alm_Recibos_Transitorios_Totalidad.Campo_Fecha_Creo + ",'DD/MM/YY')) BETWEEN '" + Datos.P_Fecha_Inicial + "'" +
                   " AND '" + Datos.P_Fecha_Final + "'";
                }
                Mi_SQL = Mi_SQL + " order by RECIBOS_TRANSITORIOS." + Ope_Alm_Recibos_Transitorios_Totalidad.Campo_No_Recibo;
                
                Dt_Recibos_Transitorios = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
  
            return Dt_Recibos_Transitorios;
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }


        

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consulta_Productos_Recibo_Transitorios
        ///DESCRIPCIÓN:             Método utilizado para consultar los prodctos 
        ///                         que forman parte del recibo transitorio.
        ///PARAMETROS:              Datos: Parametro de la capa de negocios que contiene la 
        ///                         información utilizada para realizar la consulta.
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              22/Marzo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Productos_Recibo_Transitorios(Cls_Ope_Com_Alm_Impresion_Recibos_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty; //Variable para las consultas
            DataTable Dt_Productos_Transitorios = new DataTable();
            
            Mi_SQL = "SELECT " + Ope_Com_Recibos.Tabla_Ope_Com_Recibos + "." + Ope_Com_Recibos.Campo_No_Recibo+ ", ";
            Mi_SQL = Mi_SQL + Ope_Com_Recibos.Tabla_Ope_Com_Recibos + "." + Ope_Com_Recibos.Campo_Usuario_Creo + ", ";
            Mi_SQL = Mi_SQL + Ope_Com_Recibos.Tabla_Ope_Com_Recibos + "." + Ope_Com_Recibos.Campo_Fecha_Creo + ", ";
            Mi_SQL = Mi_SQL + Ope_Com_Series_Productos.Tabla_Ope_Com_Series_Productos + "." + Ope_Com_Series_Productos.Campo_No_Orden_Compra + ", ";
            Mi_SQL = Mi_SQL + Ope_Com_Series_Productos.Tabla_Ope_Com_Series_Productos + "." + Ope_Com_Series_Productos.Campo_Serie + ", ";
            Mi_SQL = Mi_SQL + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Nombre + " as Modelo, ";
            Mi_SQL = Mi_SQL + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " as Marca, ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre + " as Producto, ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Clave + ", ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Existencia + " as Cantidad_Existente, ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Cantidad + " as Cantidad_Solicitada, ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado + " as Costo_Unitario, ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " as Producto_ID, ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_IVA_Cotizado + ", ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_IEPS_Cotizado + ", ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Total_Cotizado + " as Importe, ";
            Mi_SQL = Mi_SQL + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + "." + Cat_Com_Unidades.Campo_Nombre + " as Unidad FROM ";
            Mi_SQL = Mi_SQL + Ope_Com_Recibos.Tabla_Ope_Com_Recibos + ", " + Ope_Com_Series_Productos.Tabla_Ope_Com_Series_Productos+ ", ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + ", " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + ", ";
            Mi_SQL = Mi_SQL + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + ", " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + ", ";
            Mi_SQL = Mi_SQL + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " ";
            Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Recibos.Tabla_Ope_Com_Recibos+ "." + Ope_Com_Recibos.Campo_No_Recibo;
            Mi_SQL = Mi_SQL + " = "+ Datos.P_No_Recibo + " ";
            Mi_SQL = Mi_SQL + "AND " + Ope_Com_Recibos.Tabla_Ope_Com_Recibos + "." + Ope_Com_Recibos.Campo_No_Recibo;
            Mi_SQL = Mi_SQL + " = " + Ope_Com_Series_Productos.Tabla_Ope_Com_Series_Productos+ "." + Ope_Com_Series_Productos.Campo_No_Recibo + " ";
            Mi_SQL = Mi_SQL + "AND " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID;
            Mi_SQL = Mi_SQL + " = " + Ope_Com_Series_Productos.Tabla_Ope_Com_Series_Productos+ "." + Ope_Com_Series_Productos.Campo_Marca_Id + " ";
            Mi_SQL = Mi_SQL + "AND " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Modelo_ID;
            Mi_SQL = Mi_SQL + " = " + Ope_Com_Series_Productos.Tabla_Ope_Com_Series_Productos+ "." + Ope_Com_Series_Productos.Campo_Modelo_Id+ " ";
            Mi_SQL = Mi_SQL + "AND " + Ope_Com_Series_Productos.Tabla_Ope_Com_Series_Productos + "." + Ope_Com_Series_Productos.Campo_Producto_Id;
            Mi_SQL = Mi_SQL + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos+ "." + Cat_Com_Productos.Campo_Producto_ID+ " ";
            Mi_SQL = Mi_SQL + "AND " + Ope_Com_Series_Productos.Tabla_Ope_Com_Series_Productos + "." + Ope_Com_Series_Productos.Campo_No_Orden_Compra;
            Mi_SQL = Mi_SQL + " = " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto+ "." + Ope_Com_Req_Producto.Campo_No_Orden_Compra+ " ";
            Mi_SQL = Mi_SQL + "AND " + Ope_Com_Series_Productos.Tabla_Ope_Com_Series_Productos + "." + Ope_Com_Series_Productos.Campo_Producto_Id;
            Mi_SQL = Mi_SQL + " = " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto+ "." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID+ " ";
            Mi_SQL = Mi_SQL + "AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos+ "." + Cat_Com_Productos.Campo_Unidad_ID;
            Mi_SQL = Mi_SQL + " = " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades+ "." + Cat_Com_Unidades.Campo_Unidad_ID+ " ";

            // Ejecutar consulta
            Dt_Productos_Transitorios = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Productos_Transitorios;
        }

            #endregion

        #region (Métodos Contra Recibos)

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consulta_Contra_Recibos
        ///DESCRIPCIÓN:             Metodo utilizado para consultar los contra recibos
        ///PARAMETROS:              Datos: Parametro de la capa de negocios que contiene la 
        ///                         información utilizada para realizar la consulta.
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              01/Abril/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Contra_Recibos(Cls_Ope_Com_Alm_Impresion_Recibos_Negocio Datos)
        {
            // Declaracion de Variables
            String Mi_SQL = String.Empty;
            DataTable Dt_Contra_Recibos = new DataTable();

            try
            {
                Mi_SQL = "SELECT FACTURAS_PROVEEDORES." +  Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno+ ", ";
                Mi_SQL = Mi_SQL + " FACTURAS_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_Fecha_Creo + ", ";
                Mi_SQL = Mi_SQL + " FACTURAS_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + " ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + ", ";
                Mi_SQL = Mi_SQL + " ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Folio + ", ";
                Mi_SQL = Mi_SQL + " ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Total + ", ";
                Mi_SQL = Mi_SQL + " ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Tipo_Articulo+ ", ";

                Mi_SQL = Mi_SQL + "(select " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "";
                Mi_SQL = Mi_SQL + " WHERE ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_No_Orden_Compra + ") as NO_REQUISICION, ";

                Mi_SQL = Mi_SQL + "(select " + Ope_Com_Requisiciones.Campo_Listado_Almacen + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "";
                Mi_SQL = Mi_SQL + " WHERE ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_No_Orden_Compra + ") as LISTADO_ALMACEN, ";

                Mi_SQL = Mi_SQL + "(select " + Cat_Com_Proveedores.Campo_Nombre + " FROM ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES ";
                Mi_SQL = Mi_SQL + " WHERE ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID + " = PROVEEDORES.";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Proveedor_ID + ") as PROVEEDOR ";

                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + " FACTURAS_PROVEEDORES, ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDENES_COMPRA";
                Mi_SQL = Mi_SQL + " WHERE ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " = ";
                Mi_SQL = Mi_SQL + " FACTURAS_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + "";

                if (Datos.P_No_Recibo != null)
                {
                    Mi_SQL = Mi_SQL + " AND FACTURAS_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " like '%";
                    Mi_SQL = Mi_SQL + Datos.P_No_Recibo + "%'";
                }
                if (Datos.P_No_Orden_Compra != null)
                {
                    Mi_SQL = Mi_SQL + " AND ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " like '%" + Datos.P_No_Orden_Compra + "%'";
                }
                if (Datos.P_No_Requisicion != null)
                {
                    Mi_SQL = Mi_SQL + " AND ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + " like '%" + Datos.P_No_Requisicion + "%'";
                }
                if (Datos.P_Proveedor != null)
                {
                    Mi_SQL = Mi_SQL + " AND FACTURAS_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_Proveedor_ID + " = '" + Datos.P_Proveedor + "'";
                }

                if ((Datos.P_Fecha_Inicial != null) && (Datos.P_Fecha_Final != null))
                {
                    Mi_SQL = Mi_SQL + " AND TO_DATE(TO_CHAR( FACTURAS_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_Fecha_Recepcion + ",'DD/MM/YY')) BETWEEN '" +
                    string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Inicial)) + "'" +
                    " AND '" +
                    string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Final)) + "'";
                }
                Mi_SQL = Mi_SQL + " ORDER BY FACTURAS_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + "";

                Dt_Contra_Recibos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                return Dt_Contra_Recibos;
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }


        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Montos_Orden_Compra
        /// DESCRIPCION:            Consulta los montos de la orden de compra seleccionada por el usuario
        /// PARAMETROS :            
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            02/Julio/2011 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consulta_Montos_Orden_Compra(Cls_Ope_Com_Alm_Impresion_Recibos_Negocio Datos)
        {
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Subtotal + " ";
                Mi_SQL = Mi_SQL + ", ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Total_IVA + "";
                Mi_SQL = Mi_SQL + ", ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Total + " ";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDEN_COMPRA";
                Mi_SQL = Mi_SQL + " WHERE  ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " = ";
                Mi_SQL = Mi_SQL + Datos.P_No_Contra_Recibo + "";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0]; //  Entregar resultado
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consultar_Datos_Generales_ContraRecibo
        /// DESCRIPCION:            Método utilizado para consultar 
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos para realizar la consulta
        ///                         
        /// CREO       :            Salvador Hernandez Ramirez
        /// FECHA_CREO :            14/Marzo/2011  
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consultar_Datos_Generales_ContraRecibo(Cls_Ope_Com_Alm_Impresion_Recibos_Negocio Datos)
        {
            DataTable Dt_Datos_Generales = new DataTable();
            String Mi_SQL = null;

            // Consulta
            Mi_SQL = "SELECT Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " as No_Contrarecibo";
            Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_Proveedor_ID + " as No_Proveedor";
            Mi_SQL = Mi_SQL + ", Proveedores." + Cat_Com_Proveedores.Campo_Nombre + " as Proveedor";
            Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_Fecha_Recepcion + " as Fecha_Recepcion";
            Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_Fecha_Pago + " as Fecha_Pago";
            Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_Usuario_Creo + " as Empleado_Almacen";
            Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_Comentarios + " as Observaciones";
            Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_SubTotal_Sin_Impuesto + " as SubTotal";
            Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_IVA + " as IVA";
            Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_Total + " as Importe";
            Mi_SQL = Mi_SQL + ", Ordenes_Compra." + Ope_Com_Ordenes_Compra.Campo_Folio + " as Folio";
            Mi_SQL = Mi_SQL + ",(select REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " FROM ";
            Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";
            Mi_SQL = Mi_SQL + " WHERE REQUISICIONES." + Ope_Com_Requisiciones.Campo_No_Orden_Compra + " = ";
            Mi_SQL = Mi_SQL + " Ordenes_Compra." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " and ";
            Mi_SQL = Mi_SQL + " Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " = " + Datos.P_No_Contra_Recibo + ") as Requisicion";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + " Facturas_P";
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " Proveedores";
            Mi_SQL = Mi_SQL + " ON Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_Proveedor_ID;
            Mi_SQL = Mi_SQL + " = Proveedores." + Cat_Com_Proveedores.Campo_Proveedor_ID;
            Mi_SQL = Mi_SQL + " JOIN " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " Ordenes_Compra";
            Mi_SQL = Mi_SQL + " ON Ordenes_Compra." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno; // Este es el No_Contra_Recibo
            Mi_SQL = Mi_SQL + " = Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno;// Este es el No_Contra_Recibo
            Mi_SQL = Mi_SQL + " WHERE  Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " = " + Datos.P_No_Contra_Recibo + "";

            Dt_Datos_Generales = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Datos_Generales;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consulta_Productos_Contra_Recibo
        ///DESCRIPCIÓN:             Metodo utilizado para consultar los detalles del contra recibo
        ///PARAMETROS:              Datos: Parametro de la capa de negocios que contiene la 
        ///                         información utilizada para realizar la consulta.
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              01/Abril/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Productos_Contra_Recibo(Cls_Ope_Com_Alm_Impresion_Recibos_Negocio Datos)
        {
            DataTable Dt_Detalles = new DataTable();
            String Mi_SQL = "";

            if (Datos.P_Tipo_Articulo == "SERVICIO")
            {
                // Consulta
                Mi_SQL = "SELECT  REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " as PRODUCTO_ID";
                Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Cantidad + "";
                Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado + " as PRECIO ";
                Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Subtota_Cotizado + " as PRECIO_AC ";
                
                Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio + " AS  PRODUCTO ";
                Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Nombre_Giro + " AS  DESCRIPCION";

                Mi_SQL = Mi_SQL + ", ( SELECT " + Cat_Com_Unidades.Campo_Abreviatura + " FROM " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Com_Unidades.Campo_Unidad_ID + " = ( SELECT " + Cat_Com_Unidades.Campo_Unidad_ID + "  FROM ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = ";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " )) AS UNIDAD ";

                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO, ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";

                Mi_SQL = Mi_SQL + " WHERE  REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = ";
                Mi_SQL = Mi_SQL + " REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "";
                Mi_SQL = Mi_SQL + " AND  REQ_PRODUCTO." + Ope_Com_Requisiciones.Campo_No_Orden_Compra + " = ";
                Mi_SQL = Mi_SQL + " ( SELECT " + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra;
                Mi_SQL = Mi_SQL + "  where " + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " = ";
                Mi_SQL = Mi_SQL + Datos.P_No_Contra_Recibo + " )";

            }else if(Datos.P_Tipo_Articulo == "LISTADO_ALMACEN")
            {
                Mi_SQL = "SELECT  REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " as PRODUCTO_ID";

                Mi_SQL = Mi_SQL + ", ( SELECT " + Cat_Com_Unidades.Campo_Abreviatura + " FROM " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Com_Unidades.Campo_Unidad_ID + " = ( SELECT " + Cat_Com_Unidades.Campo_Unidad_ID + "  FROM ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = ";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " )) AS UNIDAD ";
                Mi_SQL = Mi_SQL + ",(select PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + " from ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
                Mi_SQL = Mi_SQL + " where REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = PRODUCTOS.";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as PRODUCTO";

                Mi_SQL = Mi_SQL + ",(select PRODUCTOS." + Cat_Com_Productos.Campo_Descripcion + " from ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
                Mi_SQL = Mi_SQL + " where REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = PRODUCTOS.";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as DESCRIPCION";

                Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Cantidad + "";
                Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado + " as PRECIO ";
                Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Subtota_Cotizado + " as PRECIO_AC ";

                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO, ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";

                Mi_SQL = Mi_SQL + " WHERE  REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = ";
                Mi_SQL = Mi_SQL + " REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "";
                Mi_SQL = Mi_SQL + " AND  REQ_PRODUCTO." + Ope_Com_Requisiciones.Campo_No_Orden_Compra + " = ";
                Mi_SQL = Mi_SQL + " ( SELECT " + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra;
                Mi_SQL = Mi_SQL + "  where " + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " = ";
                Mi_SQL = Mi_SQL + Datos.P_No_Contra_Recibo + " )";
            }
            else
            {
            Mi_SQL = " SELECT PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID + "";
            Mi_SQL = Mi_SQL + ", ( Select " + Cat_Com_Unidades.Campo_Abreviatura + " FROM " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " WHERE ";
            Mi_SQL = Mi_SQL + Cat_Com_Unidades.Campo_Unidad_ID + " = ( select " + Cat_Com_Unidades.Campo_Unidad_ID + "  from ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " where " + Cat_Com_Productos.Campo_Producto_ID + " = PROD_CONTRARECIBO.";
            Mi_SQL = Mi_SQL + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID + ")) AS UNIDAD ";

            Mi_SQL = Mi_SQL + ",(select PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID+ " = PRODUCTOS.";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as PRODUCTO";

            Mi_SQL = Mi_SQL + ",(select PRODUCTOS." + Cat_Com_Productos.Campo_Descripcion + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID + " = PRODUCTOS.";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as DESCRIPCION";

            Mi_SQL = Mi_SQL + ",(select REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Cantidad+ " ";
            Mi_SQL = Mi_SQL + " from  " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTOS";
            Mi_SQL = Mi_SQL + " where REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = ";
            Mi_SQL = Mi_SQL + " PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID;
            Mi_SQL = Mi_SQL + " AND REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " = ";
            Mi_SQL = Mi_SQL + "(select ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " from ";
            Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDEN_COMPRA ";
            Mi_SQL = Mi_SQL + " where  ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " = ";
            Mi_SQL = Mi_SQL + " PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_No_Contra_Recibo + ")) as CANTIDAD ";

            Mi_SQL = Mi_SQL + ",(select REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado + " ";
            Mi_SQL = Mi_SQL + " from  " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTOS";
            Mi_SQL = Mi_SQL + " where REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = ";
            Mi_SQL = Mi_SQL + " PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID;
            Mi_SQL = Mi_SQL + " AND REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " = ";
            Mi_SQL = Mi_SQL + "(select ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " from ";
            Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDEN_COMPRA ";
            Mi_SQL = Mi_SQL + " where  ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " = ";
            Mi_SQL = Mi_SQL + " PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_No_Contra_Recibo + ")) as PRECIO ";

            Mi_SQL = Mi_SQL + ",(select REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Subtota_Cotizado + " ";
            Mi_SQL = Mi_SQL + " from  " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTOS";
            Mi_SQL = Mi_SQL + " where REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = ";
            Mi_SQL = Mi_SQL + " PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID;
            Mi_SQL = Mi_SQL + " AND REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " = ";
            Mi_SQL = Mi_SQL + "(select ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " from ";
            Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDEN_COMPRA ";
            Mi_SQL = Mi_SQL + " where  ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " = ";
            Mi_SQL = Mi_SQL + " PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_No_Contra_Recibo + ")) as PRECIO_AC ";

            Mi_SQL = Mi_SQL + " FROM " + Ope_Alm_Productos_Contrarecibo.Tabla_Ope_Alm_Productos_Contrarecibo + " PROD_CONTRARECIBO ";
            Mi_SQL = Mi_SQL + " WHERE PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_No_Contra_Recibo + " = " + Datos.P_No_Contra_Recibo;
            
            }
            

            return  OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }


                ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consulta_Detalles_Contra_Recibo
        ///DESCRIPCIÓN:             Metodo utilizado para consultar los detalles del contra recibo
        ///PARAMETROS:              Datos: Parametro de la capa de negocios que contiene la 
        ///                         información utilizada para realizar la consulta.
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              01/Abril/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Detalles_Contra_Recibo(Cls_Ope_Com_Alm_Impresion_Recibos_Negocio Datos)
        {
            DataTable Dt_Detalles = new DataTable();
            String Mi_SQL = "";

            Mi_SQL = " SELECT FACTURA_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + ""; // El No. Contra Recibo
            Mi_SQL = Mi_SQL + ",  FACTURA_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_Comentarios + "";
            Mi_SQL = Mi_SQL + ",  FACTURA_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_Usuario_Creo + "";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + " FACTURA_PROVEEDORES ";
            Mi_SQL = Mi_SQL + " WHERE FACTURA_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " = " + Datos.P_No_Contra_Recibo.Trim();
            return  OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }


        

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consulta_Detalles_Contra_Recibo
        ///DESCRIPCIÓN:             Metodo utilizado para consultar los detalles del contra recibo
        ///PARAMETROS:              Datos: Parametro de la capa de negocios que contiene la 
        ///                         información utilizada para realizar la consulta.
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              01/Abril/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Documentos_Soporte(Cls_Ope_Com_Alm_Impresion_Recibos_Negocio Datos)
        {
            DataTable Dt_Documentos = new DataTable();
            String Mi_SQL = "";
                Mi_SQL = "SELECT " + Cat_Com_Documentos.Tabla_Cat_Com_Documentos + "." + Cat_Com_Documentos.Campo_Nombre + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Documentos.Tabla_Cat_Com_Documentos + "." + Cat_Com_Documentos.Campo_Comentarios+ " ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Documentos.Tabla_Cat_Com_Documentos + ", " + Ope_Com_Det_Doc_Soporte.Tabla_Ope_Com_Det_Doc_Soporte + ","+ Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + " ";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " = " + Datos.P_No_Contra_Recibo;
                Mi_SQL = Mi_SQL + " AND "+ Ope_Com_Det_Doc_Soporte.Tabla_Ope_Com_Det_Doc_Soporte + "." + Ope_Com_Det_Doc_Soporte.Campo_No_Factura_Interno + " ";
                Mi_SQL = Mi_SQL + " = "+ Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + "";
                Mi_SQL = Mi_SQL + " AND "+ Cat_Com_Documentos.Tabla_Cat_Com_Documentos + "." + Cat_Com_Documentos.Campo_Documento_ID + " ";
                Mi_SQL = Mi_SQL + " = "+ Ope_Com_Det_Doc_Soporte.Tabla_Ope_Com_Det_Doc_Soporte + "." + Ope_Com_Det_Doc_Soporte.Campo_Documento_ID + "";
                
            Dt_Documentos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Documentos;
        }

        public static DataTable Consultar_Facturas_ContraRecibo(Cls_Ope_Com_Alm_Impresion_Recibos_Negocio Datos)
        {
            String Mi_SQL = null;

            // Consulta
            Mi_SQL = "SELECT  REG_FACTURAS." + Ope_Alm_Registro_Facturas.Campo_No_Contra_Recibo + " No_Contrarecibo ";
            Mi_SQL = Mi_SQL + ", REG_FACTURAS." + Ope_Alm_Registro_Facturas.Campo_Factura_Proveedor + " as NO_FACTURA_PROVEEDOR ";
            Mi_SQL = Mi_SQL + ", REG_FACTURAS." + Ope_Alm_Registro_Facturas.Campo_Fecha_Factura + " as FECHA_FACTURA";
            Mi_SQL = Mi_SQL + ", REG_FACTURAS." + Ope_Alm_Registro_Facturas.Campo_Importe_Factura + " as Importe ";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Alm_Registro_Facturas.Tabla_Ope_Alm_Registro_Facturas + " REG_FACTURAS";
            Mi_SQL = Mi_SQL + " JOIN " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + " FACTURAS_P";
            Mi_SQL = Mi_SQL + " ON FACTURAS_P." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " = ";
            Mi_SQL = Mi_SQL + " REG_FACTURAS." + Ope_Alm_Registro_Facturas.Campo_No_Contra_Recibo + "";
            Mi_SQL = Mi_SQL + " WHERE  FACTURAS_P." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " = " + Datos.P_No_Contra_Recibo.ToString().Trim() + ""; // Este es el No_Contra_Recibo

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0]; //  Entregar resultado
        }

        #endregion

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consulta_Detalles_Contra_Recibo
        ///DESCRIPCIÓN:             Metodo utilizado para consultar los detalles del contra recibo
        ///PARAMETROS:              Datos: Parametro de la capa de negocios que contiene la 
        ///                         información utilizada para realizar la consulta.
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              01/Abril/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Tablas(Cls_Ope_Com_Alm_Impresion_Recibos_Negocio Datos)
        {
            DataTable Dt_Consulta = new DataTable();
            String Mi_SQL = "";

            if (Datos.P_Tipo_Tabla == "PROVEEDORES_RECIBO_TRANSITORIO") // Por el momento los proveedores 
            {
                Mi_SQL = "SELECT DISTINCT PROVEEDORES." + Cat_Com_Proveedores.Campo_Proveedor_ID + ", PROVEEDORES." + Cat_Com_Proveedores.Campo_Nombre + " ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES, ";
                Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + " FACTURA_PROVEEDORES, ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra+ " ORDEN_COMPRA ";
                Mi_SQL = Mi_SQL + " WHERE FACTURA_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + " = PROVEEDORES." + Cat_Com_Proveedores.Campo_Proveedor_ID + " ";
                Mi_SQL = Mi_SQL + " and FACTURA_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " = ";
                Mi_SQL = Mi_SQL + " ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno;
                Mi_SQL = Mi_SQL + " ORDER BY PROVEEDORES." + Cat_Com_Proveedores.Campo_Nombre; // Se ordena
            }

            if (Datos.P_Tipo_Tabla == "PROVEEDORES_CONTRA_RECIBO")
            {
                Mi_SQL = "SELECT DISTINCT PROVEEDORES." + Cat_Com_Proveedores.Campo_Proveedor_ID + ", PROVEEDORES." + Cat_Com_Proveedores.Campo_Nombre + " ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES, ";
                Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + " FACTURA_PROVEEDORES, ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDEN_COMPRA ";
                Mi_SQL = Mi_SQL + " WHERE FACTURA_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + " = PROVEEDORES." + Cat_Com_Proveedores.Campo_Proveedor_ID + " ";
                Mi_SQL = Mi_SQL + " and FACTURA_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " = ";
                Mi_SQL = Mi_SQL + " ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno;
                Mi_SQL = Mi_SQL + " ORDER BY PROVEEDORES." + Cat_Com_Proveedores.Campo_Nombre; // Se ordena
            }

            if (Datos.P_Tipo_Tabla == "RECIBOS_TRANSITORIOS")
            {
                Mi_SQL = "SELECT DISTINCT " + Ope_Alm_Recibos_Transitorios.Tabla_Ope_Alm_Recibos_Transitorios + "." + Ope_Alm_Recibos_Transitorios.Campo_No_Recibo + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Recibos_Transitorios.Tabla_Ope_Alm_Recibos_Transitorios + "." + Ope_Alm_Recibos_Transitorios.Campo_No_Contra_Recibo + " ";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Alm_Recibos_Transitorios.Tabla_Ope_Alm_Recibos_Transitorios;
            }

            if (Datos.P_Tipo_Tabla == "ORDEN_COMPRA")
            {
                Mi_SQL = "SELECT DISTINCT " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " ";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno; //Ordenamiento
                Mi_SQL = Mi_SQL + " = " + Datos.P_No_Contra_Recibo.Trim();
            }

            if (Datos.P_Tipo_Tabla == "UNIDAD_RESPONSABLE")
            {
                Mi_SQL = Mi_SQL + "SELECT DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + " as UNIDAD_RESPONSABLE FROM ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS, ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";
                Mi_SQL = Mi_SQL + " WHERE REQUISICIONES." + Ope_Com_Requisiciones.Campo_Dependencia_ID + " = DEPENDENCIAS.";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + " AND REQUISICIONES." + Ope_Com_Requisiciones.Campo_No_Orden_Compra + " = ";
                Mi_SQL = Mi_SQL + "(select ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " from ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDEN_COMPRA ";
                Mi_SQL = Mi_SQL + " where  ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " = ";
                Mi_SQL = Mi_SQL + Datos.P_No_Contra_Recibo.Trim() + " )";
            }

            Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Consulta;
        }

        #endregion
    }
}