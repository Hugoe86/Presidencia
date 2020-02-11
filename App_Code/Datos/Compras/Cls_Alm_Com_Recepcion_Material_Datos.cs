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
using Presidencia.Almacen_Recepcion_Material.Negocio;
using Presidencia.Constantes;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Sessiones;
using Presidencia.Bitacora_Eventos;
using Presidencia.Generar_Requisicion.Negocio;

/// <summary>
/// Summary description for Cls_Alm_Com_Recepcion_Material_Datos
/// </summary>
namespace Presidencia.Almacen_Recepcion_Material.Datos
{
    public class Cls_Alm_Com_Recepcion_Material_Datos
    {
        public Cls_Alm_Com_Recepcion_Material_Datos()
        {
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Proveedores
        /// DESCRIPCION:            Consultar los datos de los proveedores
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene 
        ///                         los datos para la busqueda
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            26/Febrero/2011 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consulta_Proveedores(Cls_Alm_Com_Recepcion_Material_Negocio Datos)
        {
            // Declaracion de variables
            String Mi_SQL = String.Empty; 

            try
            {
                //Asignar consulta
                Mi_SQL = "SELECT DISTINCT " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." +Cat_Com_Proveedores.Campo_Proveedor_ID + ", " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "."+ Cat_Com_Proveedores.Campo_Compañia + " ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + ", " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra ;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + " ";
                Mi_SQL = Mi_SQL + " and " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Estatus + "= 'AUTORIZADA' ";
                Mi_SQL = Mi_SQL + "ORDER BY " +  Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." +Cat_Com_Proveedores.Campo_Compañia; //Ordenamiento
              
                //Entregar resultado
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
        /// NOMBRE DE LA CLASE:     Consulta_Ordenes_Compra
        /// DESCRIPCION:            Método utilizado para consultar las ordenes de compra que se encuentren en estatus "COMPRA"
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene 
        ///                         los datos para la busqueda
        /// CREO       :            Salvador Hernandez Ramirez
        /// FECHA_CREO :            28/Febrero/2010  
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consulta_Ordenes_Compra(Cls_Alm_Com_Recepcion_Material_Negocio Datos)
        {
            //Declaracion de Variables
            String Mi_SQL = String.Empty; //Variable para las consultas

            try
            {
                //Asignar consulta
                Mi_SQL = "SELECT DISTINCT " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + " AS PROVEEDOR, ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + " AS FECHA, ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Folio + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Total+ ", ";
                
                Mi_SQL = Mi_SQL + "(select distinct " + Ope_Com_Req_Producto.Campo_Requisicion_ID + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra+ "." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + " = ";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Campo_Requisicion_ID + ") as REQUISICION, ";

                Mi_SQL = Mi_SQL + " (select REQUISICIONES." + Ope_Com_Requisiciones.Campo_Tipo_Articulo + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";
                Mi_SQL = Mi_SQL + " WHERE REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + ") AS TIPO_ARTICULO,";
                
                //Mi_SQL = Mi_SQL + "(select " + Ope_Com_Requisiciones.Campo_Tipo_Articulo + " FROM ";
                //Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "";
                //Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = ";
                //Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_No_Orden_Compra + ") as TIPO_ARTICULO, ";

                //Mi_SQL = Mi_SQL + "(select " + Ope_Com_Requisiciones.Campo_Folio + " FROM ";
                //Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "";
                //Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = ";
                //Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_No_Orden_Compra + ") as FOLIO_REQ, ";

                Mi_SQL = Mi_SQL + " (select REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";
                Mi_SQL = Mi_SQL + " WHERE REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + ") AS FOLIO_REQ,";

                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + ", ";
                Mi_SQL = Mi_SQL + "REPLACE(" + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + ".ROWID, " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + ".ROWID, 'NO') AS SELECCIONADA ";
                
                Mi_SQL = Mi_SQL + "FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ", " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + ", ";
                Mi_SQL = Mi_SQL + "" + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + " ";
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Estatus + " = 'AUTORIZADA'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;
                Mi_SQL = Mi_SQL + " = " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " ";

                if (Datos.P_No_Orden_Compra != null)
                {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " LIKE '%" + Datos.P_No_Orden_Compra + "%' ";  
                }

                if (Datos.P_No_Requisicion != null)
                {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Requisicion_ID+ " LIKE '%" + Datos.P_No_Requisicion + "%' ";
                }

                if (Datos.P_Proveedor_ID != null)
                {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID + " = '" + Datos.P_Proveedor_ID+ "'";  
                }

                if ((Datos.P_Fecha_Inicio_B != null) && (Datos.P_Fecha_Fin_B != null))
                {
                    Mi_SQL = Mi_SQL + " AND TO_DATE(TO_CHAR(" + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + ",'DD/MM/YY')) BETWEEN '" + Datos.P_Fecha_Inicio_B + "'" +
                    " AND '" + Datos.P_Fecha_Fin_B + "'";
                }

                Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " DESC"; 

                //Entregar resultado
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
        /// NOMBRE DE LA CLASE:     Consulta_Servicios_OC
        /// DESCRIPCION:            Consultar los servicios de una orden de compra
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene 
        ///                         el numero de orden de compra
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            19/Julio/2011 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consulta_Servicios_OC(Cls_Alm_Com_Recepcion_Material_Negocio Datos)
        {
            String Mi_SQL = String.Empty; //Variable para las consultas
            DataTable Dt_Servicios = new DataTable(); //Tabla para los servicios de la orden de compra

          try
            {
            //Asignar consulta
            Mi_SQL = "SELECT " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Clave + ", ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre + " as PRODUCTO, ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Descripcion + ", ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Importe + " as COSTO_REAL, ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Monto_Total + " as MONTO ";
            Mi_SQL = Mi_SQL + " FROM ";
            Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ", " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + ", ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos;
            Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;
            Mi_SQL = Mi_SQL + " = " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " ";
            Mi_SQL = Mi_SQL + "AND " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID;
            Mi_SQL = Mi_SQL + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Producto_ID + " ";
            Mi_SQL = Mi_SQL + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;
            Mi_SQL = Mi_SQL + " = " + Datos.P_No_Orden_Compra + " ";

            //Ejecutar consulta
            Dt_Servicios = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Servicios;

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
        /// NOMBRE DE LA CLASE:     Consulta_Orden_Compra_Detalles
        /// DESCRIPCION:            Consultar los detalles de una orden de compra
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene 
        ///                         el numero de orden de compra
        /// CREO       :            Noe Mosqueda Valadez
        /// FECHA_CREO :            30/Diciembre/2010 13:52 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consulta_Orden_Compra_Detalles(Cls_Alm_Com_Recepcion_Material_Negocio Datos)
        {
            // Declarar variables
            String Mi_SQL = String.Empty; //Variable para las consultas
            DataTable Dt_Detalles_Orden_Compra = new DataTable(); //Tabla para el resultado
            DataTable Dt_Productos = new DataTable(); //Tabla para los productos de la orden de compra
            DataTable Dt_Servicios = new DataTable(); //Tabla para los servicios de la orden de compra
            DataRow Renglon; //Renglon para el llenado de al tabla

            try
            {
                //Asignar consulta
                Mi_SQL = "SELECT " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Clave + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre + " as PRODUCTO, ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Descripcion + ", ";
                Mi_SQL = Mi_SQL + " ( SELECT " + Cat_Com_Modelos.Campo_Nombre + " FROM " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Com_Modelos.Campo_Modelo_ID + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Modelo_ID + ")  as MODELO ,";
                Mi_SQL = Mi_SQL + " ( SELECT " + Cat_Com_Marcas.Campo_Nombre + " FROM " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Com_Marcas.Campo_Marca_ID + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Marca_ID + ")  as MARCA ,";
                Mi_SQL = Mi_SQL + "( SELECT " + Cat_Com_Unidades.Campo_Nombre + " FROM " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Com_Unidades.Campo_Unidad_ID + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Unidad_ID + ")  as UNIDAD,";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Cantidad + ", ";
//                Mi_SQL = Mi_SQL + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + "." + Cat_Com_Unidades.Campo_Nombre + " as UNIDAD, ";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado + " as COSTO_REAL, ";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Subtota_Cotizado + " as MONTO, ";
                Mi_SQL = Mi_SQL + "REPLACE(" + Cat_Com_Productos.Tabla_Cat_Com_Productos + ".ROWID, ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + ".ROWID, 'PRODUCTO') as TIPO, ";
                Mi_SQL = Mi_SQL + "REPLACE(" + Cat_Com_Productos.Tabla_Cat_Com_Productos+ ".ROWID, ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos+ ".ROWID, 'SI') as SELECCIONADA FROM ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ", " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;
                Mi_SQL = Mi_SQL + " = " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " ";
                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Producto_ID + " ";
                //Mi_SQL = Mi_SQL + "AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Unidad_ID;
                //Mi_SQL = Mi_SQL + " = " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + "." + Cat_Com_Unidades.Campo_Unidad_ID + " ";
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;
                Mi_SQL = Mi_SQL + " = " + Datos.P_No_Orden_Compra + " ";

                //Ejecutar consulta
                 Dt_Productos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                //Clonar tabla
                Dt_Detalles_Orden_Compra = Dt_Productos.Clone();
                Dt_Detalles_Orden_Compra.TableName = "Detalles_Orden_Compra";

                //Ciclo para el llenado de la tabla del resultado con los productos y servicios de la orden de compra
                for (int Cont_Elementos = 0; Cont_Elementos < Dt_Productos.Rows.Count; Cont_Elementos++)
                {
                    //Instanciar renglon e ingresarlo a al tabla
                    Renglon = Dt_Productos.Rows[Cont_Elementos];
                    Dt_Detalles_Orden_Compra.ImportRow(Renglon);
                }

                //Entregar el resultado
                return Dt_Detalles_Orden_Compra;
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


        /////*******************************************************************************
        ///// NOMBRE DE LA CLASE:     Consultar_Productos_Resguardados
        ///// DESCRIPCION:            Metodo utilizado para consultar los productos de las ordenes de compra resuardadas
        ///// PARAMETROS :            Datos: Variable de la capa de negocios 
        /////                         
        ///// CREO       :            Salvador Hernández Ramírez
        ///// FECHA_CREO :            10/Febrero/2011
        ///// MODIFICO          :
        ///// FECHA_MODIFICO    :
        ///// CAUSA_MODIFICACION:
        /////*******************************************************************************/
        //public static DataTable Consultar_Productos_Resguardados(Cls_Alm_Com_Recepcion_Material_Negocio Datos)
        //{
        //    DataTable Dt_Productos_Resguardados = new DataTable();

        //    //Declaracion de variables
        //    String Mensaje = "";
        //    String Mi_SQL = String.Empty;
        //    OracleConnection Cn = new OracleConnection();
        //    OracleCommand Cmd = new OracleCommand();
        //    OracleTransaction Trans;
        //    Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
        //    Cn.Open();
        //    Trans = Cn.BeginTransaction();
        //    Cmd.Connection = Cn;
        //    Cmd.Transaction = Trans;
        //    OracleDataAdapter Da = new OracleDataAdapter(); //Adaptador para el llenado de los datatable

        //    try
        //    {
        //        //Ciclo para el barrido de la tabla de las ordenes de compra
        //        for (int Cont_Elementos = 0; Cont_Elementos < Datos.P_Dt_Ordenes_Compra.Rows.Count; Cont_Elementos++)
        //        {
        //            //Asignar consulta para los detalles de la orden de compra
        //            Mi_SQL = "SELECT  Req_Producto." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " ";
        //            Mi_SQL = Mi_SQL + ", Req_Producto." + Ope_Com_Req_Producto.Campo_Cantidad + " ";
        //            Mi_SQL = Mi_SQL + ", Req_Producto." + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio + " as PRODUCTO";
        //            Mi_SQL = Mi_SQL + ", Req_Producto." + Ope_Com_Req_Producto.Campo_Nombre_Proveedor + " as PROVEEDOR";
        //            Mi_SQL = Mi_SQL + ", Req_Producto." + Ope_Com_Req_Producto.Campo_Importe + " as PRECIO";
        //            Mi_SQL = Mi_SQL + ", Req_Producto." + Ope_Com_Req_Producto.Campo_Monto_Total + " as TOTAL";
        //            Mi_SQL = Mi_SQL + ", Req_Producto." + Ope_Com_Req_Producto.Campo_Clave + " ";
        //            Mi_SQL = Mi_SQL + ", Req_Producto." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " as ORDEN_COMPRA";
        //            Mi_SQL = Mi_SQL + ", Facturas_Proveedor." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Proveedor + " as NO_FACTURA";
        //            Mi_SQL = Mi_SQL + ", Facturas_Proveedor." + Ope_Com_Facturas_Proveedores.Campo_Fecha_Recepcion + " ";
        //            Mi_SQL = Mi_SQL + ", Facturas_Proveedor." + Ope_Com_Facturas_Proveedores.Campo_SubTotal_Sin_Impuesto + " as SUBTOTAL ";
        //            Mi_SQL = Mi_SQL + ", Facturas_Proveedor." + Ope_Com_Facturas_Proveedores.Campo_IVA + " ";
        //            Mi_SQL = Mi_SQL + ", Facturas_Proveedor." + Ope_Com_Facturas_Proveedores.Campo_IEPS + " ";
        //            Mi_SQL = Mi_SQL + ", Facturas_Proveedor." + Ope_Com_Facturas_Proveedores.Campo_Total + " as TOTAL_FACTURA";
        //            Mi_SQL = Mi_SQL + ", Marcas." + Cat_Com_Marcas.Campo_Nombre + " as MARCA";
        //            Mi_SQL = Mi_SQL + ", Modelos." + Cat_Com_Modelos.Campo_Nombre + " as MODELO";
        //            Mi_SQL = Mi_SQL + ", Unidades." + Cat_Com_Unidades.Campo_Nombre + " as UNIDAD";
        //            Mi_SQL = Mi_SQL + ", Empleado." + Cat_Empleados.Campo_Nombre + "  ||' '||";
        //            Mi_SQL = Mi_SQL + " Empleado." + Cat_Empleados.Campo_Apellido_Paterno + "  ||' '||";
        //            Mi_SQL = Mi_SQL + " Empleado." + Cat_Empleados.Campo_Apellido_Materno + "  as EMPLEADO_RESGUARDO";

        //            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " Req_Producto ";
        //            Mi_SQL = Mi_SQL + " join " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " Ordenes_Compra ";
        //            Mi_SQL = Mi_SQL + " on Req_Producto." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " ";
        //            Mi_SQL = Mi_SQL + " = Ordenes_Compra." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " ";
        //            Mi_SQL = Mi_SQL + " join " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + " Facturas_Proveedor ";
        //            Mi_SQL = Mi_SQL + " on Facturas_Proveedor." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " ";
        //            Mi_SQL = Mi_SQL + " = Ordenes_Compra." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " ";
        //            Mi_SQL = Mi_SQL + " join " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " Productos";
        //            Mi_SQL = Mi_SQL + " on Req_Producto." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " ";
        //            Mi_SQL = Mi_SQL + " = Productos." + Cat_Com_Productos.Campo_Producto_ID + " ";
        //            Mi_SQL = Mi_SQL + " join " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + " Modelos";
        //            Mi_SQL = Mi_SQL + " on Modelos." + Cat_Com_Modelos.Campo_Modelo_ID + " ";
        //            Mi_SQL = Mi_SQL + " = Productos." + Cat_Com_Productos.Campo_Modelo_ID + " ";
        //            Mi_SQL = Mi_SQL + " join " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " Marcas";
        //            Mi_SQL = Mi_SQL + " on Marcas." + Cat_Com_Marcas.Campo_Marca_ID + " ";
        //            Mi_SQL = Mi_SQL + " = Productos." + Cat_Com_Productos.Campo_Marca_ID + " ";
        //            Mi_SQL = Mi_SQL + " join " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " Unidades";
        //            Mi_SQL = Mi_SQL + " on Unidades." + Cat_Com_Unidades.Campo_Unidad_ID + " ";
        //            Mi_SQL = Mi_SQL + " = Productos." + Cat_Com_Productos.Campo_Unidad_ID + " ";
        //            Mi_SQL = Mi_SQL + " join " + Cat_Empleados.Tabla_Cat_Empleados + " Empleado";
        //            Mi_SQL = Mi_SQL + " on Ordenes_Compra." + Ope_Com_Ordenes_Compra.Campo_Usuario_Id_Resguardo + " ";
        //            Mi_SQL = Mi_SQL + " = Empleado." + Cat_Empleados.Campo_Empleado_ID;
        //            Mi_SQL = Mi_SQL + " WHERE Req_Producto." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " = " + Datos.P_Dt_Ordenes_Compra.Rows[Cont_Elementos][Ope_Com_Req_Producto.Campo_No_Orden_Compra].ToString().Trim() + " ";
                    
        //            //Ejecutar consulta
        //            Cmd.CommandText = Mi_SQL;
        //            Da.SelectCommand = Cmd;
        //            Da.Fill(Dt_Productos_Resguardados); // Se llena la tabla con la consulta
        //        }
        //    }         
        //    catch (OracleException Ex)
        //    {
        //        Trans.Rollback();
        //        //variable para el mensaje 
        //        //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
        //        if (Ex.Code == 8152)
        //        {
        //            Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
        //        }
        //        else if (Ex.Code == 2627)
        //        {
        //            if (Ex.Message.IndexOf("PRIMARY") != -1)
        //            {
        //                Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
        //            }
        //            else if (Ex.Message.IndexOf("UNIQUE") != -1)
        //            {
        //                Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
        //            }
        //            else
        //            {
        //                Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
        //            }
        //        }
        //        else if (Ex.Code == 547)
        //        {
        //            Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
        //        }
        //        else if (Ex.Code == 515)
        //        {
        //            Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
        //        }
        //        else
        //        {
        //            Mensaje = "Error al intentar consultar productos resguardados. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
        //        }
        //        //Indicamos el mensaje 
        //        throw new Exception(Mensaje);
        //    }
        //    finally
        //    {
        //        Cn.Close();
        //    }
                   
        //    return Dt_Productos_Resguardados; // Retorna la tabla
        //}


        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Montos_Orden_Compra
        /// DESCRIPCION:            Se obtienen los montos de la orden de compra seleccionada por el usuario
        /// PARAMETROS :                                 
        /// CREO       :            Noe Mosqueda Valadez  
        /// FECHA_CREO :            24/Febrero/2010 
        /// MODIFICO          :     Se implemtento el método "Alta_Bitacora"
        /// FECHA_MODIFICO    :     03/Marzo/2011
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Montos_Orden_Compra(Cls_Alm_Com_Recepcion_Material_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty; //Variable para las consultas

            try
            {
                //Asignar consulta
                Mi_SQL = "SELECT " + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + ", " + Ope_Com_Ordenes_Compra.Campo_Subtotal + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Campo_Total_IEPS + ", " + Ope_Com_Ordenes_Compra.Campo_Total_IVA + ", " + Ope_Com_Ordenes_Compra.Campo_Total + " ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = " + Datos.P_No_Orden_Compra;

                //Entregar resultado
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
        /// NOMBRE DE LA CLASE:     Actualizar_Orden_Compra
        /// DESCRIPCION:            Se actualizan las ordenes de compra al estatus "SURTIDA"
        ///                         y se guardan los comentarios
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene 
        ///                         los numero de orden de compra
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            25/Febrero/2010 
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static void Actualizar_Orden_Compra(Cls_Alm_Com_Recepcion_Material_Negocio Datos)
        {
            DataTable DataTable_Temporal = null;
            String Mi_SQL;
            DataTable_Temporal = Datos.P_Dt_Ordenes_Compra;

            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;

            try
            {
                    Mi_SQL = "UPDATE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra;
                    Mi_SQL = Mi_SQL + " SET " + Ope_Com_Ordenes_Compra.Campo_Estatus + "=" + "'SURTIDA'";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " =" + Datos.P_No_Orden_Compra;

                    // Se da de alta la operación en el método "Alta_Bitacora"
                    // Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Alm_Com_Recepcion_Material.aspx", No_Orden_Compra, Mi_SQL);

                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery(); // Se ejecuta la operación 


                    String Fecha_Sustio = DateTime.Now.ToString("dd/MM/yyyy").ToUpper();

                    Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
                    Mi_SQL = Mi_SQL + " SET " + Ope_Com_Requisiciones.Campo_Estatus + "=" + "'SURTIDA'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Campo_Fecha_Surtido + "= '" + Fecha_Sustio + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Campo_Empleado_Surtido_ID + "= '" + Cls_Sessiones.Empleado_ID + "'";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_No_Orden_Compra + " =" + Datos.P_No_Orden_Compra;

                    // Se da de alta la operación en el método "Alta_Bitacora"
                    // Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Alm_Com_Recepcion_Material.aspx", No_Orden_Compra, Mi_SQL);

                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery(); // Se ejecuta la operación
 

                // METODO UTILIZADO PARA consultar EL NUMERO DE REQUISICIÓN
                    String No_Requisicion = "";
                    Object No_Req = null;

                    Mi_SQL = " SELECT " + Ope_Com_Requisiciones.Campo_Requisicion_ID; // Este es el NO_CONTRA_RECIBO
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_No_Orden_Compra + " = " + Datos.P_No_Orden_Compra.Trim();

                    //Ejecutar consulta
                    No_Req = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    //Verificar si no es nulo
                    if (No_Req != null && Convert.IsDBNull(No_Req) == false)
                        No_Requisicion = Convert.ToString(No_Req);
                    else
                        No_Requisicion = "";

                // Se Guarda el Historial de la requisición
                Cls_Ope_Com_Requisiciones_Negocio Requisiciones = new Cls_Ope_Com_Requisiciones_Negocio();
                Requisiciones.Registrar_Historial("SURTIDA / EN ALMACÉN", No_Requisicion);

                    
                // Ene sta parte se insertan los comentarios en la tabla OPE_COM_COMENT_ORDEN_COMP
                    if (Datos.P_Observaciones.ToString().Trim() != "")
                    {
                        String No_Comentario = Obtener_Id_Consecutivo(Ope_Com_Coment_orden_Comp.Campo_No_Comentario, Ope_Com_Coment_orden_Comp.Tabla_Ope_Com_Ordenes_Compra);
                        String Fecha_Creo = DateTime.Now.ToString("dd/MM/yyyy").ToUpper();

                        Mi_SQL = "INSERT INTO " + Ope_Com_Coment_orden_Comp.Tabla_Ope_Com_Ordenes_Compra +
                        " (" + Ope_Com_Coment_orden_Comp.Campo_No_Comentario +
                        ", " + Ope_Com_Coment_orden_Comp.Campo_No_Orden_Compra +
                        ", " + Ope_Com_Coment_orden_Comp.Campo_Comentario +
                        ", " + Ope_Com_Coment_orden_Comp.Campo_Estatus +
                        ", " + Ope_Com_Coment_orden_Comp.Campo_Usuario_Creo +
                        ", " + Ope_Com_Coment_orden_Comp.Campo_Fecha_Creo +
                        ") VALUES ('" +
                        No_Comentario + "'," +
                        Datos.P_No_Orden_Compra + ",'" +
                        Datos.P_Observaciones + "','" +
                        "SURTIDA" + "','" +
                        Cls_Sessiones.Nombre_Empleado + "','" + Fecha_Creo + "')";

                        // Ejecutar consulta
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery(); // Se ejecutan las operación 2 
                        
                        // Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Com_Alm_Generar_Inventario_Stock.aspx", Datos.P_No_Inventario, Mi_SQL);
                    }
                    Trans.Commit(); // Se ejecuta la transacciones
            }
            catch (Exception Ex)
            {
                Trans.Rollback();

                String Mensaje = "Error al intentar realizar las  transacción. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
       }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Id_Consecutivo
        ///DESCRIPCIÓN: crea una sentencia sql para insertar un inventario en la base de datos
        ///PARAMETROS: 1.-Campo_ID, nombre del campo de la tabla al cual se quiere sacar el ultimo valor
        ///            2.-Tabla, nombre de la tabla que se va a consultar
        ///CREO: Salvador Hernández Ramírez
        ///FECHA_CREO: 07/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static String Obtener_Id_Consecutivo(String Campo_ID, String Tabla)
        {
            String Consecutivo = "";
            String Mi_SQL;         
            Object Obj;            

            try
            {
                Mi_SQL = "SELECT NVL(MAX (" + Campo_ID + "),'0000000000') ";
                Mi_SQL = Mi_SQL + "FROM " + Tabla;
                Obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Obj))
                {
                    Consecutivo = "0000000001";
                }
                else
                {
                    Consecutivo = string.Format("{0:0000000000}", Convert.ToInt32(Obj) + 1);
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Consecutivo;
        }



        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consultar_Datos_Orden_Compra
        ///DESCRIPCIÓN:             Método utilizado para consultar los comentarios y el numero de reserva
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              19/Abril/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Datos_Orden_Compra(Cls_Alm_Com_Recepcion_Material_Negocio Datos)
        {
            // Declaracion de Variables
            String Mi_SQL = String.Empty; //Variable para las consultas
            DataTable Dt_Datos_Ordenes_Compra = new DataTable();

            try
            {
                Mi_SQL = "SELECT " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Comentarios + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Reserva + ", ";
                Mi_SQL = Mi_SQL + "(select " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_No_Orden_Compra + ") as REQUISICION ";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;
                Mi_SQL = Mi_SQL + " = " + Datos.P_No_Orden_Compra.Trim();

                Dt_Datos_Ordenes_Compra = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                return Dt_Datos_Ordenes_Compra;
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consultar_Datos_Orden_Compra
        ///DESCRIPCIÓN:             Método utilizado para consultar los comentarios y el numero de reserva
        ///PARAMETROS:              Clase de negocio con no de orden de compra y no requisición
        ///CREO:                    Gustavo Angeles C.
        ///FECHA_CREO:              25/jul/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Productos_Servicios_Orden_Compra(Cls_Alm_Com_Recepcion_Material_Negocio Datos)
        {
            // Declaracion de Variables
            String Mi_SQL = String.Empty; //Variable para las consultas
            DataTable Dt_Productos_Servicos = new DataTable();
            try
            {
                Mi_SQL = "SELECT * FROM " +                     
                Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = '" + Datos.P_Busqueda.Trim() + "' AND " +
                Ope_Com_Req_Producto.Campo_No_Orden_Compra + " = " + Datos.P_No_Orden_Compra.Trim();
                Dt_Productos_Servicos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                return Dt_Productos_Servicos;
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
        }
    }
}