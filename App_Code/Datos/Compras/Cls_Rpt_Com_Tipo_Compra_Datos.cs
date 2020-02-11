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
using Presidencia.Rpt_Tipo_Compra.Negocio;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;

/// <summary>
/// Summary description for Cls_Rpt_Com_Tipo_Compra_Datos
/// </summary>

namespace Presidencia.Rpt_Tipo_Compra.Datos
{
    public class Cls_Rpt_Com_Tipo_Compra_Datos
    {
        ///*******************************************************************
        ///CONSULTAS COMBOS
        ///*******************************************************************
        #region Consultas para Combos
        public static DataTable Consultar_Requisiciones(Cls_Rpt_Com_Tipo_Compra_Negocio Datos)
        {
            String Mi_SQL = "SELECT " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                ", " + Ope_Com_Requisiciones.Campo_Folio +
                " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " WHERE " + Ope_Com_Requisiciones.Campo_Estatus + "='COMPRA'" +
                " AND " + Ope_Com_Requisiciones.Campo_Tipo_Articulo + "='" + Datos.P_Tipo_Articulo+"'"+
                " ORDER BY " + Ope_Com_Requisiciones.Campo_Folio;
            DataTable Dt_Requisiciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Requisiciones;
        }
        public static DataTable Consultar_Proveedores(Cls_Rpt_Com_Tipo_Compra_Negocio Datos)
        {
            String Mi_SQL = "SELECT " + Cat_Com_Proveedores.Campo_Proveedor_ID +
                ", " + Cat_Com_Proveedores.Campo_Nombre +
                " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores +
                " ORDER BY " + Cat_Com_Proveedores.Campo_Nombre; 

            DataTable Dt_Proveedores = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Proveedores;
        }
        public static DataTable Consultar_Cotizadores(Cls_Rpt_Com_Tipo_Compra_Negocio Datos)
        {
            String Mi_SQL = "SELECT " + Cat_Com_Cotizadores.Campo_Empleado_ID +
                ", " + Cat_Com_Cotizadores.Campo_Nombre_Completo +
                " FROM " + Cat_Com_Cotizadores.Tabla_Cat_Com_Cotizadores +
                " ORDER BY " + Cat_Com_Cotizadores.Campo_Nombre_Completo;
            DataTable Dt_Cotizadores = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Cotizadores;
        }

        public static DataTable Consultar_Productos(Cls_Rpt_Com_Tipo_Compra_Negocio Datos)
        {
            String Mi_SQL = "SELECT " + Cat_Com_Productos.Campo_Producto_ID +
                 "," + Cat_Com_Productos.Campo_Nombre +
                 " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos +
                 " WHERE " + Cat_Com_Productos.Campo_Stock + " IS NULL " +
                 " OR " + Cat_Com_Productos.Campo_Stock + "='NO'" +
                 " ORDER BY " + Cat_Com_Productos.Campo_Nombre;
            DataTable Dt_Productos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Productos;
        }

        public static DataTable Consultar_Servicios(Cls_Rpt_Com_Tipo_Compra_Negocio Datos)
        {
            String Mi_SQL= "SELECT " + Cat_Com_Servicios.Campo_Servicio_ID +
                ", " + Cat_Com_Servicios.Campo_Nombre + 
                " FROM " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios +
                " ORDER BY " + Cat_Com_Servicios.Campo_Nombre;
            DataTable Dt_Servicios = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Servicios;
        }


        #endregion
        ///*******************************************************************
        ///CONSULTAS COMPRAS
        ///*******************************************************************
        #region Consultas Compras

        public static DataSet Consultar_Compra_Directa(Cls_Rpt_Com_Tipo_Compra_Negocio Datos)
        {
            String Mi_SQL = "SELECT OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                ",OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio +
                ",OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Estatus +
                ",OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Total + " AS PRIMERA_COTIZACION"+
                ",OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Total_Cotizado + " AS COTIZACION_FINAL" +
                ",OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Tipo_Articulo + 
                ",OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
                ",OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio +
                ",OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
                ",(SELECT EMPLEADO." + Cat_Empleados.Campo_Nombre + "||' '||" +
                " EMPLEADO." + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                " EMPLEADO." + Cat_Empleados.Campo_Apellido_Materno +
                " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADO" +
                " WHERE EMPLEADO." + Cat_Empleados.Campo_Empleado_ID +
                "= OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Empleado_Cotizador_ID + ") AS EMPLEADO_COTIZADOR" +
                ",OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Cantidad +
                ",OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
                ",OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " OPE_COM_REQ_PRODUCTO" +
                ", " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " OPE_COM_REQUISICIONES " +
                " WHERE OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                "=OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                " AND OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Tipo_Compra + "='" + Datos.P_Tipo_Compra + "'" +
                " AND OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Tipo_Articulo + "='" + Datos.P_Tipo_Articulo + "'"; 
               
                //Aqui se maneja el filtrado de Productos,requisiciones, cotizadore y proveedores
                Mi_SQL = Mi_SQL + " AND OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID + 
                " IN(SELECT " +Ope_Com_Req_Producto.Campo_Requisicion_ID +
                " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                " WHERE " + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " IS NOT NULL";

                if(Datos.P_Requisicion_ID!= null)
                {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Req_Producto.Campo_Requisicion_ID + "='" + Datos.P_Requisicion_ID + "'";

                }
                
                if(Datos.P_Empleado_Proveedor_ID !=null)
                {
                    Mi_SQL = Mi_SQL + " OR " + Ope_Com_Req_Producto.Campo_Proveedor_ID + "='" + Datos.P_Empleado_Proveedor_ID + "'";
                }
                
                if(Datos.P_Empleado_Cotizador_ID != null)
                {
                    Mi_SQL = Mi_SQL + " OR " + Ope_Com_Req_Producto.Campo_Empleado_Cotizador_ID + "='" + Datos.P_Empleado_Cotizador_ID + "'";
                }
                if( Datos.P_Producto_ID !=null)
                {
                    Mi_SQL= Mi_SQL + " OR " + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + "='" + Datos.P_Producto_ID+"'"; 
                }

                Mi_SQL = Mi_SQL + " GROUP BY " + Ope_Com_Req_Producto.Campo_Requisicion_ID + ")";
                    //" AND OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Fecha_Cotizacion + 
                    //" BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicial + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                    //" AND TO_DATE ('" + Datos.P_Fecha_Final + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                Mi_SQL = Mi_SQL + " ORDER BY OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID;

            DataSet Ds_Compra_Directa = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Ds_Compra_Directa;
        }

        public static DataSet Consultar_Comite_Compra(Cls_Rpt_Com_Tipo_Compra_Negocio Datos)
        {

            String Mi_SQL = "SELECT OPE_COM_COMITE_COMPRAS." + Ope_Com_Comite_Compras.Campo_No_Comite_Compras +
                ", OPE_COM_COMITE_COMPRAS." + Ope_Com_Comite_Compras.Campo_Folio +
                ", OPE_COM_COMITE_COMPRAS." + Ope_Com_Comite_Compras.Campo_Estatus +
                ", OPE_COM_COMITE_COMPRAS." + Ope_Com_Comite_Compras.Campo_Justificacion +
                ", OPE_COM_COMITE_COMPRAS." + Ope_Com_Comite_Compras.Campo_Monto_Total + " AS PRIMERA_COTIZACION" +
                ", OPE_COM_COMITE_COMPRAS." + Ope_Com_Comite_Compras.Campo_Total_Cotizado + " AS COTIZACION_FINAL" +
                ", OPE_COM_COMITE_COMPRAS." + Ope_Com_Comite_Compras.Campo_Tipo +
                ", OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio + " AS FOLIO_REQUISICION" +
                ",OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Estatus + " AS ESTATUS_REQUISICION" +
                ",OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
                ",OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio +
                ",OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
                ",(SELECT EMPLEADO." + Cat_Empleados.Campo_Nombre + "||' '||" +
                " EMPLEADO." + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                " EMPLEADO." + Cat_Empleados.Campo_Apellido_Materno +
                " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADO" +
                " WHERE EMPLEADO." + Cat_Empleados.Campo_Empleado_ID +
                "= OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Empleado_Cotizador_ID + ") AS EMPLEADO_COTIZADOR" +
                ",OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Cantidad +
                ",OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
                ",OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " OPE_COM_REQ_PRODUCTO" +
                ", " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " OPE_COM_REQUISICIONES " +
                ", " + Ope_Com_Comite_Compras.Tabla_Ope_Com_Comite_Compras + " OPE_COM_COMITE_COMPRAS " +
                " WHERE OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                "=OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                " AND OPE_COM_COMITE_COMPRAS." + Ope_Com_Comite_Compras.Campo_No_Comite_Compras + 
                "=OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_No_Comite_Compras +
                " AND OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Tipo_Compra + "='" + Datos.P_Tipo_Compra + "'" +
                " AND OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Tipo_Articulo + "='" + Datos.P_Tipo_Articulo + "'";

            //Aqui se maneja el filtrado de Productos,requisiciones, cotizadore y proveedores
            Mi_SQL = Mi_SQL + " AND OPE_COM_COMITE_COMPRAS." + Ope_Com_Comite_Compras.Campo_No_Comite_Compras +
            " IN(SELECT OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_No_Comite_Compras +
            " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " OPE_COM_REQUISICIONES" +
            "," + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " OPE_COM_REQ_PRODUCTO" +
            " WHERE OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
            "= OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID;

            if (Datos.P_Requisicion_ID != null)
            {
                Mi_SQL = Mi_SQL + " AND OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID + "='" + Datos.P_Requisicion_ID + "'";
            }

            if (Datos.P_Empleado_Proveedor_ID != null)
            {
                Mi_SQL = Mi_SQL + " OR OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Proveedor_ID + "='" + Datos.P_Empleado_Proveedor_ID + "'";
            }

            if (Datos.P_Empleado_Cotizador_ID != null)
            {
                Mi_SQL = Mi_SQL + " OR OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Empleado_Cotizador_ID + "='" + Datos.P_Empleado_Cotizador_ID + "'";
            }
            if (Datos.P_Producto_ID != null)
            {
                Mi_SQL = Mi_SQL + " OR OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + "='" + Datos.P_Producto_ID + "'";
            }

            Mi_SQL = Mi_SQL + " GROUP BY OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_No_Comite_Compras + ")";
                //" AND OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Fecha_Cotizacion +
                //" BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicial + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                //" AND TO_DATE ('" + Datos.P_Fecha_Final + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
            Mi_SQL = Mi_SQL + " ORDER BY OPE_COM_COMITE_COMPRAS." + Ope_Com_Comite_Compras.Campo_No_Comite_Compras;

            DataSet Ds_Comite_Compra = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            return Ds_Comite_Compra;
        }
        public static DataSet Consultar_Cotizacion(Cls_Rpt_Com_Tipo_Compra_Negocio Datos)
        {
            String Mi_SQL = "SELECT OPE_COM_COTIZACIONES." + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
               ", OPE_COM_COTIZACIONES." + Ope_Com_Cotizaciones.Campo_Folio +
               ", OPE_COM_COTIZACIONES." + Ope_Com_Cotizaciones.Campo_Estatus +
               ", OPE_COM_COTIZACIONES." + Ope_Com_Cotizaciones.Campo_Condiciones +
               ", OPE_COM_COTIZACIONES." + Ope_Com_Cotizaciones.Campo_Listado_Almacen +
               ", OPE_COM_COTIZACIONES." + Ope_Com_Cotizaciones.Campo_Tipo +
               ", OPE_COM_COTIZACIONES." + Ope_Com_Cotizaciones.Campo_Total + " AS PRIMERA_COTIZACION" +
               ", OPE_COM_COTIZACIONES." + Ope_Com_Cotizaciones.Campo_Total_Cotizado + " AS COTIZACION_FINAL" +
               ", OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio + " AS FOLIO_REQUISICION" +
               ",OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Estatus + " AS ESTATUS_REQUISICION" +
               ",OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
               ",OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio +
               ",OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
               ",(SELECT EMPLEADO." + Cat_Empleados.Campo_Nombre + "||' '||" +
               " EMPLEADO." + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
               " EMPLEADO." + Cat_Empleados.Campo_Apellido_Materno +
               " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADO" +
               " WHERE EMPLEADO." + Cat_Empleados.Campo_Empleado_ID +
               "= OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Empleado_Cotizador_ID + ") AS EMPLEADO_COTIZADOR" +
               ",OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Cantidad +
               ",OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
               ",OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Total_Cotizado +
               " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " OPE_COM_REQ_PRODUCTO" +
               ", " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " OPE_COM_REQUISICIONES " +
               ", " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones + " OPE_COM_COTIZACIONES " +
               " WHERE OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
               "=OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID +
               " AND OPE_COM_COTIZACIONES." + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
               "=OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_No_Cotizacion +
               " AND OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Tipo_Compra + "='" + Datos.P_Tipo_Compra + "'" +
               " AND OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Tipo_Articulo + "='" + Datos.P_Tipo_Articulo + "'";

            //Aqui se maneja el filtrado de Productos,requisiciones, cotizadore y proveedores
            Mi_SQL = Mi_SQL + " AND OPE_COM_COTIZACIONES." + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
            " IN(SELECT OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_No_Cotizacion + 
            " FROM " +  Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones+ " OPE_COM_REQUISICIONES" +
            "," + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " OPE_COM_REQ_PRODUCTO" +
            " WHERE OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID+ 
            "= OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID;
            
            if (Datos.P_Requisicion_ID != null)
            {
                Mi_SQL = Mi_SQL + " AND OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID + "='" + Datos.P_Requisicion_ID + "'";
            }

            if (Datos.P_Empleado_Proveedor_ID != null)
            {
                Mi_SQL = Mi_SQL + " OR OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Proveedor_ID + "='" + Datos.P_Empleado_Proveedor_ID + "'";
            }

            if (Datos.P_Empleado_Cotizador_ID != null)
            {
                Mi_SQL = Mi_SQL + " OR OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Empleado_Cotizador_ID + "='" + Datos.P_Empleado_Cotizador_ID + "'";
            }
            if (Datos.P_Producto_ID != null)
            {
                Mi_SQL = Mi_SQL + " OR OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + "='" + Datos.P_Producto_ID + "'";
            }

            Mi_SQL = Mi_SQL + " GROUP BY OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_No_Cotizacion + ")";
                //" AND OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Fecha_Cotizacion +
                //" BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicial + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                //" AND TO_DATE ('" + Datos.P_Fecha_Final + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
            Mi_SQL = Mi_SQL + " ORDER BY OPE_COM_COTIZACIONES." + Ope_Com_Cotizaciones.Campo_No_Cotizacion;

            DataSet Ds_Cotizacion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            return Ds_Cotizacion;
        }
        public static DataSet Consultar_Licitacion(Cls_Rpt_Com_Tipo_Compra_Negocio Datos)
        {
            String Mi_SQL = "SELECT OPE_COM_LICITACIONES." + Ope_Com_Licitaciones.Campo_No_Licitacion +
               ", OPE_COM_LICITACIONES." + Ope_Com_Licitaciones.Campo_Folio +
               ", OPE_COM_LICITACIONES." + Ope_Com_Licitaciones.Campo_Estatus +
               ", TO_CHAR(OPE_COM_LICITACIONES." + Ope_Com_Licitaciones.Campo_Fecha_Inicio +
               ",'DD/MON/YYYY') AS " + Ope_Com_Licitaciones.Campo_Fecha_Inicio +
               ", TO_CHAR(OPE_COM_LICITACIONES." + Ope_Com_Licitaciones.Campo_Fecha_Fin +
               ",'DD/MON/YYYY') AS " + Ope_Com_Licitaciones.Campo_Fecha_Fin +
               ", OPE_COM_LICITACIONES." + Ope_Com_Licitaciones.Campo_Justificacion +
               ", OPE_COM_LICITACIONES." + Ope_Com_Licitaciones.Campo_Tipo +
               ", OPE_COM_LICITACIONES." + Ope_Com_Licitaciones.Campo_Monto_Total + " AS PRIMERA_COTIZACION" +
               ", OPE_COM_LICITACIONES." + Ope_Com_Licitaciones.Campo_Total_Cotizado + " AS COTIZACION_FINAL" +
               ", OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio + " AS FOLIO_REQUISICION" +
               ",OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Estatus + " AS ESTATUS_REQUISICION" +
               ",OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
               ",OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio +
               ",OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
               ",(SELECT EMPLEADO." + Cat_Empleados.Campo_Nombre + "||' '||" +
               " EMPLEADO." + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
               " EMPLEADO." + Cat_Empleados.Campo_Apellido_Materno +
               " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADO" +
               " WHERE EMPLEADO." + Cat_Empleados.Campo_Empleado_ID +
               "= OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Empleado_Cotizador_ID + ") AS EMPLEADO_COTIZADOR" +
               ",OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Cantidad +
               ",OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
               ",OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Total_Cotizado +
               " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " OPE_COM_REQ_PRODUCTO" +
               ", " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " OPE_COM_REQUISICIONES " +
               ", " + Ope_Com_Licitaciones.Tabla_Ope_Com_Licitaciones + " OPE_COM_LICITACIONES " +
               " WHERE OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
               "=OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID +
               " AND OPE_COM_LICITACIONES." + Ope_Com_Licitaciones.Campo_No_Licitacion +
               "=OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_No_Licitacion +
               " AND OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Tipo_Compra + "='" + Datos.P_Tipo_Compra + "'" +
               " AND OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Tipo_Articulo + "='" + Datos.P_Tipo_Articulo + "'";

            //Aqui se maneja el filtrado de Productos,requisiciones, cotizadore y proveedores
            Mi_SQL = Mi_SQL + " AND OPE_COM_LICITACIONES." + Ope_Com_Licitaciones.Campo_No_Licitacion +
            " IN(SELECT OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_No_Licitacion +
            " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " OPE_COM_REQUISICIONES" +
            "," + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " OPE_COM_REQ_PRODUCTO" +
            " WHERE OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
            "= OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID;

            if (Datos.P_Requisicion_ID != null)
            {
                Mi_SQL = Mi_SQL + " AND OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID + "='" + Datos.P_Requisicion_ID + "'";
            }

            if (Datos.P_Empleado_Proveedor_ID != null)
            {
                Mi_SQL = Mi_SQL + " OR OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Proveedor_ID + "='" + Datos.P_Empleado_Proveedor_ID + "'";
            }

            if (Datos.P_Empleado_Cotizador_ID != null)
            {
                Mi_SQL = Mi_SQL + " OR OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Empleado_Cotizador_ID + "='" + Datos.P_Empleado_Cotizador_ID + "'";
            }
            if (Datos.P_Producto_ID != null)
            {
                Mi_SQL = Mi_SQL + " OR OPE_COM_REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + "='" + Datos.P_Producto_ID + "'";
            }

            Mi_SQL = Mi_SQL + " GROUP BY OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_No_Licitacion + ")";
                //" AND OPE_COM_REQUISICIONES." + Ope_Com_Requisiciones.Campo_Fecha_Cotizacion +
                //" BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicial + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                //" AND TO_DATE ('" + Datos.P_Fecha_Final + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
            Mi_SQL = Mi_SQL + " ORDER BY OPE_COM_LICITACIONES." + Ope_Com_Licitaciones.Campo_No_Licitacion; 

            DataSet Ds_Licitacion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            return Ds_Licitacion;
        }        
        
        #endregion
    }
}//fin del namespace