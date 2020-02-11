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
using Presidencia.Reporte_Requisiciones.Negocios;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;


/// <summary>
/// Summary description for Cls_Ope_Reporte_Requisiciones_Datos
/// </summary>

namespace Presidencia.Reporte_Requisiciones.Datos
{
    public class Cls_Rpt_Com_Reporte_Requisiciones_Datos
    {
        public Cls_Rpt_Com_Reporte_Requisiciones_Datos()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        ///*******************************************************************
        ///METODOS
        ///*******************************************************************

        #region Metodos

        public static DataTable Consultar_Requisiciones(Cls_Rpt_Com_Reporte_Requisiciones_Negocio Requisicion_Negocio)
        {
            
            String Mi_SQL = "SELECT " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                            "," + Ope_Com_Requisiciones.Campo_Folio +
                            "," + Ope_Com_Requisiciones.Campo_Fecha_Surtido +
                            "," + Ope_Com_Requisiciones.Campo_Tipo +
                            "," + Ope_Com_Requisiciones.Campo_Tipo_Articulo +
                            "," + Ope_Com_Requisiciones.Campo_Estatus +
                            "," + Ope_Com_Requisiciones.Campo_Total_Cotizado +
                            " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
            if (Requisicion_Negocio.P_Estatus == null)
            {
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_Estatus +
                        " IN ('SURTITO','COMPRA')";
            }
            else
            {
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_Estatus +
                        "='" + Requisicion_Negocio.P_Estatus + "'";
            }

            if (Requisicion_Negocio.P_Fecha_Inicial != null)
            {
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Campo_Fecha_Generacion + " BETWEEN '" + Requisicion_Negocio.P_Fecha_Inicial + "'" +
                    " AND '" + Requisicion_Negocio.P_Fecha_Final + "'";
            }

            if (Requisicion_Negocio.P_Dependencia_ID != null)
            {
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Campo_Dependencia_ID + " ='" + Requisicion_Negocio.P_Dependencia_ID + "'";
            }

            if (Requisicion_Negocio.P_Area_ID != null)
            {
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Campo_Area_ID + " ='" + Requisicion_Negocio.P_Area_ID + "'";
            }

            Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Com_Requisiciones.Campo_Fecha_Creo;


            if (Requisicion_Negocio.P_No_Requisicion != null)
            {
                Mi_SQL = "SELECT REQ."+Ope_Com_Requisiciones.Campo_Folio +
                    ",( SELECT " + Cat_Dependencias.Campo_Nombre + 
                    " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + 
                    " WHERE " + Cat_Dependencias.Campo_Dependencia_ID +
                    " =REQ." + Ope_Com_Requisiciones.Campo_Dependencia_ID +")" + " AS DEPENDENCIA" +
                    ",( SELECT " + Cat_Areas.Campo_Nombre + 
                    " FROM " + Cat_Areas.Tabla_Cat_Areas +
                    " WHERE " + Cat_Areas.Campo_Area_ID + 
                    " =REQ." + Ope_Com_Requisiciones.Campo_Area_ID + ")" + " AS AREA " +
                    ", REQ." + Ope_Com_Requisiciones.Campo_Tipo +
                    ", REQ." +  Ope_Com_Requisiciones.Campo_Estatus +
                    ", REQ." + Ope_Com_Requisiciones.Campo_Tipo_Articulo +
                    ", REQ." + Ope_Com_Requisiciones.Campo_Tipo_Compra + 
                    ", REQ." +  Ope_Com_Requisiciones.Campo_Verificaion_Entrega +
                    ", REQ." + Ope_Com_Requisiciones.Campo_Justificacion_Compra +
                    ", REQ." + Ope_Com_Requisiciones.Campo_Especificacion_Prod_Serv +
                    ", REQ." + Ope_Com_Requisiciones.Campo_No_Consolidacion +
                    ", REQ." + Ope_Com_Requisiciones.Campo_Consolidada +
                    ", TO_CHAR(REQ." + Ope_Com_Requisiciones.Campo_Fecha_Construccion + ",'DD/MON/YYYY') AS " + Ope_Com_Requisiciones.Campo_Fecha_Construccion +
                    ", (SELECT " + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno 
                    + "||' '||" + Cat_Empleados.Campo_Apellido_Materno + 
                    " FROM " +  Cat_Empleados.Tabla_Cat_Empleados + 
                    " WHERE "+ Cat_Empleados.Campo_Empleado_ID +
                    "=REQ." + Ope_Com_Requisiciones.Campo_Empleado_Construccion_ID + ") AS " + Ope_Com_Requisiciones.Campo_Empleado_Construccion_ID +
                    ", TO_CHAR(REQ." + Ope_Com_Requisiciones.Campo_Fecha_Generacion + ",'DD/MON/YYYY') AS " + Ope_Com_Requisiciones.Campo_Fecha_Generacion +
                    ", (SELECT " + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno
                    + "||' '||" + Cat_Empleados.Campo_Apellido_Materno +
                    " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                    " WHERE " + Cat_Empleados.Campo_Empleado_ID +
                    "=REQ." + Ope_Com_Requisiciones.Campo_Empleado_Generacion_ID + ") AS " + Ope_Com_Requisiciones.Campo_Empleado_Generacion_ID +
                    ", TO_CHAR(REQ." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion + ",'DD/MON/YYYY') AS " + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion +
                    ", (SELECT " + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno
                    + "||' '||" + Cat_Empleados.Campo_Apellido_Materno +
                    " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                    " WHERE " + Cat_Empleados.Campo_Empleado_ID +
                    "=REQ." + Ope_Com_Requisiciones.Campo_Empleado_Autorizacion_ID + ") AS " + Ope_Com_Requisiciones.Campo_Empleado_Autorizacion_ID +
                    ", TO_CHAR(REQ." + Ope_Com_Requisiciones.Campo_Fecha_Filtrado + ",'DD/MON/YYYY') AS " + Ope_Com_Requisiciones.Campo_Fecha_Filtrado +
                    ", (SELECT " + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno
                    + "||' '||" + Cat_Empleados.Campo_Apellido_Materno +
                    " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                    " WHERE " + Cat_Empleados.Campo_Empleado_ID +
                    "=REQ." + Ope_Com_Requisiciones.Campo_Empleado_Filtrado_ID + ") AS " + Ope_Com_Requisiciones.Campo_Empleado_Filtrado_ID +
                    ", TO_CHAR(REQ." + Ope_Com_Requisiciones.Campo_Fecha_Cotizacion + ",'DD/MON/YYYY') AS " + Ope_Com_Requisiciones.Campo_Fecha_Cotizacion +
                    ", (SELECT " + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno
                    + "||' '||" + Cat_Empleados.Campo_Apellido_Materno +
                    " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                    " WHERE " + Cat_Empleados.Campo_Empleado_ID +
                    "=REQ." + Ope_Com_Requisiciones.Campo_Empleado_Cotizacion_ID + ") AS " + Ope_Com_Requisiciones.Campo_Empleado_Cotizacion_ID +
                    ", TO_CHAR(REQ." + Ope_Com_Requisiciones.Campo_Fecha_Confirmacion + ",'DD/MON/YYYY') AS " + Ope_Com_Requisiciones.Campo_Fecha_Confirmacion +
                    ", (SELECT " + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno
                    + "||' '||" + Cat_Empleados.Campo_Apellido_Materno +
                    " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                    " WHERE " + Cat_Empleados.Campo_Empleado_ID +
                    "=REQ." + Ope_Com_Requisiciones.Campo_Empleado_Confirmacion_ID + ") AS " + Ope_Com_Requisiciones.Campo_Empleado_Confirmacion_ID +
                    ", TO_CHAR(REQ." + Ope_Com_Requisiciones.Campo_Fecha_Surtido + ",'DD/MON/YYYY') AS " + Ope_Com_Requisiciones.Campo_Fecha_Surtido +
                    ", (SELECT " + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno
                    + "||' '||" + Cat_Empleados.Campo_Apellido_Materno +
                    " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                    " WHERE " + Cat_Empleados.Campo_Empleado_ID +
                    "=REQ." + Ope_Com_Requisiciones.Campo_Empleado_Surtido_ID + ") AS " + Ope_Com_Requisiciones.Campo_Empleado_Surtido_ID +
                    ", TO_CHAR(REQ." + Ope_Com_Requisiciones.Campo_Fecha_Distribucion + ",'DD/MON/YYYY') AS " + Ope_Com_Requisiciones.Campo_Fecha_Distribucion +
                    ", (SELECT " + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno
                    + "||' '||" + Cat_Empleados.Campo_Apellido_Materno +
                    " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                    " WHERE " + Cat_Empleados.Campo_Empleado_ID +
                    "=REQ." + Ope_Com_Requisiciones.Campo_Empleado_Distribucion_ID + ") AS " + Ope_Com_Requisiciones.Campo_Empleado_Distribucion_ID +
                    ", REQ." + Ope_Com_Requisiciones.Campo_Total_Cotizado +
                    ", REQ." + Ope_Com_Requisiciones.Campo_Total +
                    " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQ WHERE " + 
                    Ope_Com_Requisiciones.Campo_Requisicion_ID + "='" +Requisicion_Negocio.P_No_Requisicion+ "'";
            }


            if (Requisicion_Negocio.P_Folio_Busqueda != null)
            {
                Mi_SQL = "SELECT " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                            "," + Ope_Com_Requisiciones.Campo_Folio +
                            "," + Ope_Com_Requisiciones.Campo_Fecha_Surtido +
                            "," + Ope_Com_Requisiciones.Campo_Tipo +
                            "," + Ope_Com_Requisiciones.Campo_Tipo_Articulo +
                            "," + Ope_Com_Requisiciones.Campo_Estatus +
                            "," + Ope_Com_Requisiciones.Campo_Total_Cotizado +
                            " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + 
                            " WHERE UPPER(" + Ope_Com_Requisiciones.Campo_Folio + ")" +
                            " LIKE UPPER('%" + Requisicion_Negocio.P_Folio_Busqueda+ "%')" +
                            " AND " + Ope_Com_Requisiciones.Campo_Estatus +
                            " IN ('SURTITO','COMPRA')" + 
                            " ORDER BY " + Ope_Com_Requisiciones.Campo_Fecha_Creo;
            }
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        public static DataTable Consultar_Productos(Cls_Rpt_Com_Reporte_Requisiciones_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = "";
            switch (Requisicion_Negocio.P_Tipo_Articulo)
            {
                case "PRODUCTO":
                    Mi_SQL = "SELECT PRO." + Cat_Com_Productos.Campo_Nombre + " AS NOMBRE_PROD_SERV" +
                            ", DET." + Ope_Com_Req_Producto.Campo_Cantidad +
                            ", DET." + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_Monto_Total +
                            ", DET." + Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
                            ", (SELECT " + Cat_Empleados.Campo_Nombre +
                            "||' '||" + Cat_Empleados.Campo_Apellido_Paterno +
                            "||' '||" + Cat_Empleados.Campo_Apellido_Materno + 
                            " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Campo_Empleado_ID +
                            "=DET." + Ope_Com_Req_Producto.Campo_Empleado_Cotizador_ID + ")" + " AS NOMBRE_COTIZADOR" +
                            " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " DET" +
                            " JOIN " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRO" +
                            " ON PRO." + Cat_Com_Productos.Campo_Producto_ID +
                            " = DET." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +
                            " WHERE DET." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                            " = '" + Requisicion_Negocio.P_No_Requisicion + "'";
                    break;
                case "SERVICIO":
                    Mi_SQL = "SELECT SER." + Cat_Com_Servicios.Campo_Nombre + " AS NOMBRE_PROD_SERV" +
                            ", DET." + Ope_Com_Req_Producto.Campo_Cantidad +
                            ", DET." + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_Monto_Total +
                            ", DET." + Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
                            ", (SELECT " + Cat_Empleados.Campo_Nombre +
                            "||' '||" + Cat_Empleados.Campo_Apellido_Paterno +
                            "||' '||" + Cat_Empleados.Campo_Apellido_Materno + 
                            " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Campo_Empleado_ID +
                            "=DET." + Ope_Com_Req_Producto.Campo_Empleado_Cotizador_ID + ")" + " AS NOMBRE_COTIZADOR" +
                            " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " DET" +
                            " JOIN " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + " SER" +
                            " ON SER." + Cat_Com_Servicios.Campo_Servicio_ID +
                            " = DET." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +
                            " WHERE DET." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                            " = '" + Requisicion_Negocio.P_No_Requisicion + "'";
                    break;

            }
            DataTable Dt_Productos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

            return Dt_Productos;
        }

        public static DataTable Consultar_Comentarios(Cls_Rpt_Com_Reporte_Requisiciones_Negocio Requisicion_Negocio)
        {

            String Mi_SQL = "SELECT " + Ope_Com_Req_Observaciones.Campo_Comentario +
                ", " + Ope_Com_Req_Observaciones.Campo_Estatus +
                ", TO_CHAR(" + Ope_Com_Req_Observaciones.Campo_Fecha_Creo + ",'DD/MON/YYYY') AS " + Ope_Com_Req_Observaciones.Campo_Fecha_Creo +
                ", " + Ope_Com_Req_Observaciones.Campo_Usuario_Creo +
                " FROM " + Ope_Com_Req_Observaciones.Tabla_Ope_Com_Req_Observaciones + 
                " WHERE " + Ope_Com_Req_Observaciones.Campo_Requisicion_ID + 
                "='" + Requisicion_Negocio.P_No_Requisicion+ "'" +
                " ORDER BY " + Ope_Com_Req_Observaciones.Campo_Fecha_Creo;

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        public static DataTable Consultar_Detalle_Compra(Cls_Rpt_Com_Reporte_Requisiciones_Negocio Requisicion_Negocio)
        {
            
            String Mi_SQL = "";
            DataTable Dt_Compra = new DataTable();
            switch (Requisicion_Negocio.P_Tipo_Compra)
            {
                case "COMITE COMPRAS":
                    Mi_SQL = "SELECT " + Ope_Com_Requisiciones.Campo_No_Comite_Compras +
                        " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                        " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                        "='" +Requisicion_Negocio.P_No_Requisicion +"'";
                    Dt_Compra = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    Mi_SQL = "SELECT " + Ope_Com_Comite_Compras.Campo_Folio +
                        " FROM " + Ope_Com_Comite_Compras.Tabla_Ope_Com_Comite_Compras +
                        " WHERE " + Ope_Com_Comite_Compras.Campo_No_Comite_Compras +
                        "='" + Dt_Compra.Rows[0][Ope_Com_Requisiciones.Campo_No_Comite_Compras].ToString() + "'";

                    break;
                case "LICITACION":
                    Mi_SQL = "SELECT " + Ope_Com_Requisiciones.Campo_No_Licitacion +
                        " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                        " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                        "='" + Requisicion_Negocio.P_No_Requisicion + "'";
                    Dt_Compra = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    Mi_SQL = "SELECT " + Ope_Com_Licitaciones.Campo_Folio +
                        " FROM " + Ope_Com_Licitaciones.Tabla_Ope_Com_Licitaciones +
                        " WHERE " + Ope_Com_Licitaciones.Campo_No_Licitacion +
                        "='" + Dt_Compra.Rows[0][Ope_Com_Requisiciones.Campo_No_Licitacion] + "'";

                    break;
                case "COTIZACION":
                    Mi_SQL = "SELECT " + Ope_Com_Requisiciones.Campo_No_Cotizacion +
                        " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                        " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                        "='" + Requisicion_Negocio.P_No_Requisicion + "'";
                    Dt_Compra = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    Mi_SQL = "SELECT " + Ope_Com_Cotizaciones.Campo_Folio +
                        " FROM " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones +
                        " WHERE " + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
                        "='" + Dt_Compra.Rows[0][Ope_Com_Requisiciones.Campo_No_Cotizacion] + "'";

                    break;

            }
            DataTable Dt_Folio_Compra = new DataTable();
            if (Mi_SQL.Trim() != String.Empty)
            {
                 Dt_Folio_Compra = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            return Dt_Folio_Compra;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Areas
        ///DESCRIPCIÓN: Metodo que ejecuta la sentencia SQL para consultar las Areas
        ///PARAMETROS:  1.- Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Areas(Cls_Rpt_Com_Reporte_Requisiciones_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = "SELECT " + Cat_Areas.Campo_Area_ID +
                            ", " + Cat_Areas.Campo_Nombre +
                            " FROM " + Cat_Areas.Tabla_Cat_Areas +
                            " WHERE " + Cat_Areas.Campo_Dependencia_ID + " ='" +
                            Cls_Sessiones.Dependencia_ID_Empleado + "'" +
                            " ORDER BY " + Cat_Areas.Campo_Nombre;
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Dependencias
        ///DESCRIPCIÓN: Metodo que ejecuta la sentencia SQL para consultar las Areas
        ///PARAMETROS:  1.- Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Dependencias(Cls_Rpt_Com_Reporte_Requisiciones_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = "SELECT " + Cat_Dependencias.Campo_Dependencia_ID +
                            ", " + Cat_Dependencias.Campo_Nombre +
                            " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                            " ORDER BY " + Cat_Dependencias.Campo_Nombre;

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        #endregion

    }
}