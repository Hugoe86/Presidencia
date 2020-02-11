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
using Presidencia.Listado_Ordenes_Compra.Negocio;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Bitacora_Eventos;

/// <summary>
/// Summary description for Cls_Ope_Com_Listado_Ordenes_Compra_Datos
/// </summary>
/// 
namespace Presidencia.Listado_Ordenes_Compra.Datos
{
    public class Cls_Ope_Com_Listado_Ordenes_Compra_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Proveedores
        ///DESCRIPCIÓN          : Metodo utilizado para consultar los proveedores
        ///PARAMETROS           : Datos: Contiene los parametros que se van a utilizar para
        ///                       realizar la consulta a la Base de Datos.
        ///CREO                 : Salvador Hernández Ramírez
        ///FECHA_CREO           : 18/Abril/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Proveedores(Cls_Ope_Com_Listado_Ordenes_Compra_Negocio Datos)
        {
            DataTable Dt_Proveedores = new DataTable(); // Tabla para guardar los proveedores
            String Mi_SQL = String.Empty; // Variable para las consultas

            try
            {
                // Consulta los Proveedores
                Mi_SQL = " SELECT DISTINCT " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Compañia;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ", " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + " ";

                // Se guardan los proveedores en la tabla "Dt_Proveedores"
                Dt_Proveedores = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                return Dt_Proveedores;
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
        ///NOMBRE DE LA FUNCIÓN : Consulta_Ordenes_Compra
        ///DESCRIPCIÓN          : Método utilizado para consultar las ordenes de compra
        ///PARAMETROS           : Datos: Contiene los parametros que se van a utilizar para
        ///                       realizar la consulta a la Base de Datos.
        ///CREO                 : Salvador Hernández Ramírez
        ///FECHA_CREO           : 18/Abril/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consulta_Ordenes_Compra(Cls_Ope_Com_Listado_Ordenes_Compra_Negocio Datos)
        {
            DataTable Dt_Ordenes_Compra = new DataTable();
            String Mi_SQL = String.Empty; // Variable para las consultas
            try
            {  
                // Consulta ordenes de compra Surtidas y que no tengan Numero de recibo transitorio
                Mi_SQL = "SELECT " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + " AS PROVEEDOR, ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + " AS FECHA, ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Folio + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Total + " as TOTAL_COTIZADO,";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Fecha_Entrega + " AS VIGENCIA,";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Estatus + ",";    
                Mi_SQL = Mi_SQL + "(SELECT NO_REQUISICION FROM OPE_COM_REQUISICIONES WHERE NO_ORDEN_COMPRA = " +
                    Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra
                    +") AS NO_REQUISICION";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra;// +", " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "";
                Mi_SQL = Mi_SQL + " JOIN " +
                    Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " ON " +
                    Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_No_Orden_Compra +
                    " = " +
                    Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;
                Mi_SQL = Mi_SQL + " JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " ON " +
                    Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID +
                    " = " +
                    Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + " ";
                Mi_SQL = Mi_SQL + " WHERE ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Total + " IS NOT NULL ";
                //Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID;
                //Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + " ";

                if (!String.IsNullOrEmpty(Datos.P_No_Orden_Compra))
                {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = '" + Datos.P_No_Orden_Compra + "'";
                }
                if (!String.IsNullOrEmpty( Datos.P_Proveedor_Id))
                {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID + " = '" + Datos.P_Proveedor_Id + "'";
                }
                if (!String.IsNullOrEmpty(Datos.P_Fecha_Inicial) && !String.IsNullOrEmpty(Datos.P_Fecha_Final))
                {

                    DateTime _DateTime_Inicial = Convert.ToDateTime(Datos.P_Fecha_Inicial);
                    DateTime _DateTime_Final = Convert.ToDateTime(Datos.P_Fecha_Final);
                    Datos.P_Fecha_Inicial = _DateTime_Inicial.ToString("dd/MM/yyyy");
                    Datos.P_Fecha_Final = _DateTime_Final.ToString("dd/MM/yyyy");                    
                    Mi_SQL = Mi_SQL + " AND TO_DATE(TO_CHAR(" + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + ",'DD/MM/YY')) BETWEEN '" + Datos.P_Fecha_Inicial + "'" +
                  " AND '" + Datos.P_Fecha_Final + "'";
                }
                if (!String.IsNullOrEmpty(Datos.P_Estatus))
                {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Estatus + 
                    " IN ('" + Datos.P_Estatus + "') ";
                }
                if (Datos.P_Cotizador_ID != null && Datos.P_Cotizador_ID != "0")
                {
                    Mi_SQL = Mi_SQL + " AND " +
                        Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Cotizador_ID +
                        " = '" + Datos.P_Cotizador_ID + "'";
                }
                if (!String.IsNullOrEmpty(Datos.P_Impresa))
                {
                    Mi_SQL = Mi_SQL + " AND " +
                        Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Impresa +
                        " = '" + Datos.P_Impresa + "'";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;
           
                // Se guardan las ordenes de compra en la tabla "Dt_Ordenes_Compra"
                Dt_Ordenes_Compra = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                return Dt_Ordenes_Compra;
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
        ///NOMBRE DE LA FUNCIÓN : Consulta_Cabecera_Orden_Compra
        ///DESCRIPCIÓN          : Método utilizado para consultar las ordenes de compra
        ///PARAMETROS           : Datos: Contiene los parametros que se van a utilizar para
        ///                       realizar la consulta a la Base de Datos.
        ///CREO                 : Salvador Hernández Ramírez
        ///FECHA_CREO           : 18/Abril/2011 
        ///MODIFICO             : Salvador Hernández Ramírez
        ///FECHA_MODIFICO       : 05/Mayo/2011 
        ///CAUSA_MODIFICACIÓN   : Se agrego la sub consuslta para la optención de la Marca
        ///*******************************************************************************
        public static DataTable Consulta_Cabecera_Orden_Compra(Cls_Ope_Com_Listado_Ordenes_Compra_Negocio Datos)
        {
            DataTable Dt_Cabecera_OC = new DataTable();
            String Mi_SQL = String.Empty; // Variable para las consultas

            try
            {
                Mi_SQL = "SELECT " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Folio + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + " AS PROVEEDOR, ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Fecha_Entrega + " AS VIGENCIA, ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + " as FECHA_CONSTRUCCION, ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Usuario_Creo + " as RESPONSABLE, ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Comentarios + " AS COMENT, ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Subtotal + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Total_IVA+ " as IVA,";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Total_IEPS + " as IEPS,";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Total + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Reserva+ ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Codigo_Programatico + " AS CODIGO_PROGRAMATICO, ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DIRECCION, ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Folio + " AS REQUISICION, ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Justificacion_Compra + " AS COMENTARIOS, ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Condicion1 + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Condicion2 + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Condicion3 + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Condicion4 + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Condicion5 + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Condicion6;

                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ", " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + ", " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ", " + Cat_Dependencias.Tabla_Cat_Dependencias;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + " ";
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = "+ Datos.P_No_Orden_Compra.Trim();
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_No_Orden_Compra + " = "+ Datos.P_No_Orden_Compra.Trim();
                Mi_SQL = Mi_SQL + " AND " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " = " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID;
                    Dt_Cabecera_OC = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                return Dt_Cabecera_OC;
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
        ///NOMBRE DE LA FUNCIÓN : Consulta_Detalles_Orden_Compra
        ///DESCRIPCIÓN          : Método utilizado para consultar las ordenes de compra
        ///PARAMETROS           : Datos: Contiene los parametros que se van a utilizar para
        ///                       realizar la consulta a la Base de Datos.
        ///CREO                 : Salvador Hernández Ramírez
        ///FECHA_CREO           : 18/Abril/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consulta_Detalles_Orden_Compra(Cls_Ope_Com_Listado_Ordenes_Compra_Negocio Datos)
        {
            DataTable Dt_Productos_OC = new DataTable();
            String Mi_SQL = String.Empty; // Variable para las consultas

            try
            {
                Mi_SQL = "SELECT " + Ope_Com_Requisiciones.Campo_Tipo_Articulo + " FROM " +
                    Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " WHERE " +
                    Ope_Com_Requisiciones.Campo_No_Orden_Compra + " = " + Datos.P_No_Orden_Compra;
                DataTable Dt_Requisicion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                Datos.P_Tipo_Producto_Servicio = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Tipo_Articulo].ToString().Trim();
                if (Datos.P_Tipo_Producto_Servicio == "PRODUCTO")
                {
                    Mi_SQL = "SELECT " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Clave + ", ";
                    Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID + ", ";
                    //Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre + " ||', '|| ";

                    Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Nombre_Prod_Serv_Orden_Compra ;
                    Mi_SQL = Mi_SQL + " ||'. '||" + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Marca_OC;
                    //Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Descripcion;

                        Mi_SQL = Mi_SQL + " AS PRODUCTO, ";
                    Mi_SQL = Mi_SQL + " MARCA_OC  as MARCA ,";
                    Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Cantidad + ", ";
                    Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado + " as PRECIO, ";
                    Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Subtota_Cotizado + " as IMPORTE, ";
                    Mi_SQL = Mi_SQL + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + "." + Cat_Com_Unidades.Campo_Nombre + " AS UNIDAD ";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ", " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + ", " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " ";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;
                    Mi_SQL = Mi_SQL + " = " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " ";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Producto_ID + " ";
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Unidad_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + "." + Cat_Com_Unidades.Campo_Unidad_ID + " ";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;
                    Mi_SQL = Mi_SQL + " = " + Datos.P_No_Orden_Compra.Trim() + " ";
                    Mi_SQL += " ORDER BY " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre + " ASC";
                }
                else
                {
                    Mi_SQL = "SELECT " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + "." + Cat_Com_Servicios.Campo_Clave + ", ";
                    Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID + ", ";
                    Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Nombre_Prod_Serv_Orden_Compra;
                    //Mi_SQL = Mi_SQL + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + "." + Cat_Com_Servicios.Campo_Nombre;
                    Mi_SQL = Mi_SQL + " AS PRODUCTO, ";
                    Mi_SQL = Mi_SQL + "'S/MARCA' as MARCA ,";
                    Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Cantidad + ", ";
                    Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado + " AS PRECIO, ";
                    Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Subtota_Cotizado + " AS IMPORTE, ";
                    Mi_SQL = Mi_SQL + "'S/UNIDAD' AS UNIDAD ";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Servicios.Tabla_Cat_Com_Servicios;
                    Mi_SQL = Mi_SQL + " WHERE ";
                    Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = ";
                    Mi_SQL = Mi_SQL + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + "." + Cat_Com_Servicios.Campo_Servicio_ID + " AND ";
                    Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_No_Orden_Compra;
                    Mi_SQL = Mi_SQL + " = " + Datos.P_No_Orden_Compra.Trim() + " ";
                    Mi_SQL += " ORDER BY " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + "." + Cat_Com_Servicios.Campo_Nombre + " ASC";
                }
                Dt_Productos_OC = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                return Dt_Productos_OC;
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

        public static DataTable Consulta_Directores()
        {
            DataTable Dt_Directores = new DataTable();
            String Mi_SQL = String.Empty; // Variable para las consultas

            try
            {
                Mi_SQL = "SELECT " + Cat_Com_Directores.Tabla_Cat_Com_Directores + "." + Cat_Com_Directores.Campo_Director_Adquisiciones + " AS DIRECTOR_ADQUISICIONES , ";
                Mi_SQL = Mi_SQL + " " + Cat_Com_Directores.Tabla_Cat_Com_Directores + "." + Cat_Com_Directores.Campo_Oficial_Mayor + " AS OFICIALIA_MAYOR , ";
                Mi_SQL = Mi_SQL + " " + Cat_Com_Directores.Tabla_Cat_Com_Directores + "." + Cat_Com_Directores.Campo_Tesorero + " AS TESORERO " + " FROM " + Cat_Com_Directores.Tabla_Cat_Com_Directores + " ";

                Dt_Directores = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                return Dt_Directores;
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
    }
}
