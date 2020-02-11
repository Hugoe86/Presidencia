using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Data;
using Presidencia.Seguimiento_Ordenes.Negocio;

/// <summary>
/// Summary description for Cls_Ope_Com_Seguimiento_Ordenes_Compra_Datos
/// </summary>
namespace Presidencia.Seguimiento_Ordenes.Datos
{
    public class Cls_Ope_Com_Seguimiento_Ordenes_Compra_Datos
    {


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Ordenes_Compra
        ///DESCRIPCIÓN: Consulta loas ordenes de compra en base a parametros y devuelve un 
        ///DataTable con la informacion solicitada, se le debe setear FechaInicial, FechaFinal 
        ///Estatus
        ///PARAMETROS: 1.-Objeto de Negocio de Orden de Compra
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Ordenes_Compra(Cls_Ope_Com_Seguimiento_Ordenes_Compra_Negocio Negocio)
        {
            Negocio.P_Fecha_Inicial = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Negocio.P_Fecha_Inicial));
            Negocio.P_Fecha_Final = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Negocio.P_Fecha_Final));
            String Mi_Sql = "";
            if (Negocio.P_Folio != null && Negocio.P_Folio != "")
            {
                Negocio.P_Folio = Negocio.P_Folio.Replace("OC-", "");
                //Mi_Sql = "SELECT *FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra +
                //" WHERE " + Ope_Com_Ordenes_Compra.Campo_Folio + " = '"k + Negocio.P_Folio + "'";
                Mi_Sql = "SELECT " +
                Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ".*," +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Justificacion_Compra + "," +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Codigo_Programatico + " AS CODIGO" +
                ",(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE " +
                Cat_Dependencias.Campo_Dependencia_ID + " = " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." +
                Ope_Com_Requisiciones.Campo_Dependencia_ID +
                ") AS NOMBRE_DEPENDENCIA," +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                " FROM " +
                Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " JOIN " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " ON " +
                Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + " = " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                " WHERE " +
                Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = '" +
                Negocio.P_Folio + "'";
                //Mi_Sql = Mi_Sql + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ".ESPECIALES_RAMO_33" +
                //" = '" + "NO" + "'";

            }
            else
            {
                Mi_Sql = "SELECT " +
                Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ".*," +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Justificacion_Compra + "," +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Codigo_Programatico + " AS CODIGO" +
                ",(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE " +
                Cat_Dependencias.Campo_Dependencia_ID + " = " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." +
                Ope_Com_Requisiciones.Campo_Dependencia_ID +
                ") AS NOMBRE_DEPENDENCIA," +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                " FROM " +
                Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " JOIN " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " ON " +
                Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones+ " = " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                " WHERE " +
                "TO_DATE(TO_CHAR(" + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + ",'DD/MM/YY'))" +
                        " >= '" + Negocio.P_Fecha_Inicial + "' AND " +
                "TO_DATE(TO_CHAR(" + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + ",'DD/MM/YY'))" +
                        " <= '" + Negocio.P_Fecha_Final + "'";
                //Mi_Sql = Mi_Sql + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ".ESPECIALES_RAMO_33" +
                //" = '" + "NO" + "'";

                if (!String.IsNullOrEmpty(Negocio.P_Estatus))
                {
                    Mi_Sql = Mi_Sql + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." +
                    Ope_Com_Ordenes_Compra.Campo_Estatus + " IN ('" + Negocio.P_Estatus + "')";
                }
            }
            Mi_Sql = Mi_Sql + " ORDER BY " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            DataTable _DataTable = null;
            if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
            {
                _DataTable = _DataSet.Tables[0];
            }
            return _DataTable;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consulta_Cabecera_Orden_Compra
        ///DESCRIPCIÓN          : Método utilizado para consultar las ordenes de compra
        ///PARAMETROS           : Datos: Contiene los parametros que se van a utilizar para
        ///                       realizar la consulta a la Base de Datos.
        ///CREO                 : Susana Trigueros Armenta
        ///FECHA_CREO           : 24/Sep/12
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : Se agrego la sub consuslta para la optención de la Marca
        ///*******************************************************************************
        public static DataTable Consulta_Cabecera_Orden_Compra(Cls_Ope_Com_Seguimiento_Ordenes_Compra_Negocio Datos)
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
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Total_IVA + " as IVA,";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Total_IEPS + " as IEPS,";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Total + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Reserva + ", ";
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
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = " + Datos.P_No_Orden_Compra;
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones;
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
        ///CREO                 : Susana Trigueros Armenta
        ///FECHA_CREO           : 24/Sep/12
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consulta_Detalles_Orden_Compra(Cls_Ope_Com_Seguimiento_Ordenes_Compra_Negocio Datos)
        {
            DataTable Dt_Productos_OC = new DataTable();
            String Mi_SQL = String.Empty; // Variable para las consultas

            try
            {
                Mi_SQL = "SELECT " + Ope_Com_Requisiciones.Campo_Tipo_Articulo + ", " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " FROM " +
                    Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " WHERE " +
                    Ope_Com_Requisiciones.Campo_Requisicion_ID + " = (SELECT " + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones +
                    " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " WHERE "  + 
                    Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = " + Convert.ToString(Datos.P_No_Orden_Compra) + ")";

                DataTable Dt_Requisicion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                String P_Tipo_Producto_Servicio = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Tipo_Articulo].ToString().Trim();
                if (P_Tipo_Producto_Servicio == "PRODUCTO")
                {
                    Mi_SQL = "SELECT " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Clave + ", ";
                    Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID + ", ";
                    //Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre + " ||', '|| ";

                    Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Nombre_Prod_Serv_Orden_Compra;
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
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones;
                    Mi_SQL = Mi_SQL + " = " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " ";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Producto_ID + " ";
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Unidad_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + "." + Cat_Com_Unidades.Campo_Unidad_ID + " ";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;
                    Mi_SQL = Mi_SQL + " = " + Datos.P_No_Orden_Compra+ " ";
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
                    Mi_SQL = Mi_SQL + " = " + Datos.P_No_Orden_Compra + " ";
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

    }
}