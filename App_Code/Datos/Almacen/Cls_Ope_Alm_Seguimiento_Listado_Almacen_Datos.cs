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
using Presidencia.Seguimiento_Listado.Negocios;
using SharpContent.ApplicationBlocks.Data;

/// <summary>
/// Summary description for Cls_Ope_Alm_Seguimiento_Listado_Almacen_Datos
/// </summary>

namespace Presidencia.Seguimiento_Listado.Datos
{
    public class Cls_Ope_Alm_Seguimiento_Listado_Almacen_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Listado
        ///DESCRIPCIÓN: Metodo que consulta la tabla de listado
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO:
        ///FECHA_CREO: 22/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Listado(Cls_Ope_Alm_Seguimiento_Listado_Almacen_Negocio Datos_Listado)
        {
            String Mi_SQL = "";
            Mi_SQL = "SELECT LISTADO." + Ope_Com_Listado.Campo_Folio +
                     ",  TO_CHAR(LISTADO." + Ope_Com_Listado.Campo_Fecha_Creo + ",'DD/MON/YYYY') AS FECHA_CREO" +
                     ", LISTADO." + Ope_Com_Listado.Campo_Tipo +
                     ", LISTADO." + Ope_Com_Listado.Campo_Estatus +
                     ", LISTADO." + Ope_Com_Listado.Campo_Total +
                     ", LISTADO." + Ope_Com_Listado.Campo_Listado_ID +
                     " FROM " + Ope_Com_Listado.Tabla_Ope_Com_Listado + " LISTADO" +
                     " WHERE " + Ope_Com_Listado.Campo_Folio + " IS NOT NULL";


            if (Datos_Listado.P_Fecha_Inicial != null)
            {
                Mi_SQL = Mi_SQL + " AND LISTADO." + Ope_Com_Listado.Campo_Fecha_Creo + " BETWEEN '" + Datos_Listado.P_Fecha_Inicial + "'" +
                    " AND '" + Datos_Listado.P_Fecha_Final + "'";
            }
            if (Datos_Listado.P_Folio_Busqueda != null)
            {
                Mi_SQL = Mi_SQL + " AND UPPER(LISTADO." + Ope_Com_Listado.Campo_Folio +
                    ") LIKE UPPER('%" + Datos_Listado.P_Folio_Busqueda + "%')";
            }
            if (Datos_Listado.P_Tipo != null)
            {
                Mi_SQL = Mi_SQL + " AND LISTADO." + Ope_Com_Listado.Campo_Tipo;
                Mi_SQL = Mi_SQL + "='" +  Datos_Listado.P_Tipo.Trim() + "'";
  
            }

            if (Datos_Listado.P_Listado_ID != null)
            {

                Mi_SQL = "SELECT " + Ope_Com_Listado.Campo_Folio +

                         ", LIS." + Ope_Com_Listado.Campo_Estatus +
                         ", LIS." + Ope_Com_Listado.Campo_Tipo +
                         ", LIS." + Ope_Com_Listado.Campo_Listado_ID +
                         ", (SELECT " + Cat_Sap_Partidas_Especificas.Campo_Clave +
                         "||' '||" + Cat_Sap_Partidas_Especificas.Campo_Nombre +
                         " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas +
                         " WHERE " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID +
                         " = LIS." + Ope_Com_Listado.Campo_No_Partida_ID + ") AS " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID +
                         ", TO_CHAR(" + Ope_Com_Listado.Campo_Fecha_Construccion + ",'DD/MON/YYYY') AS " + Ope_Com_Listado.Campo_Fecha_Construccion +
                         ", TO_CHAR(" + Ope_Com_Listado.Campo_Fecha_Generacion + ",'DD/MON/YYYY') AS " + Ope_Com_Listado.Campo_Fecha_Generacion +
                         ", TO_CHAR(" + Ope_Com_Listado.Campo_Fecha_Autorizacion + ",'DD/MON/YYYY') AS " + Ope_Com_Listado.Campo_Fecha_Autorizacion +
                         ", TO_CHAR(" + Ope_Com_Listado.Campo_Fecha_Cancelacion + ",'DD/MON/YYYY') AS " + Ope_Com_Listado.Campo_Fecha_Cancelacion +
                         ", (SELECT " + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno +
                         "||' '||" + Cat_Empleados.Campo_Apellido_Materno + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                         " WHERE " + Cat_Empleados.Campo_Empleado_ID + "= LIS." + Ope_Com_Listado.Campo_Empleado_Construccion_ID + ") AS "+ Ope_Com_Listado.Campo_Empleado_Construccion_ID +
                         ", (SELECT " + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno +
                         "||' '||" + Cat_Empleados.Campo_Apellido_Materno + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                         " WHERE " + Cat_Empleados.Campo_Empleado_ID + "= LIS." + Ope_Com_Listado.Campo_Empleado_Generacion_ID + ") AS " + Ope_Com_Listado.Campo_Empleado_Generacion_ID +
                         ", (SELECT " + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno +
                         "||' '||" + Cat_Empleados.Campo_Apellido_Materno + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                         " WHERE " + Cat_Empleados.Campo_Empleado_ID + "= LIS." + Ope_Com_Listado.Campo_Empleado_Autorizacion_ID + ") AS " +Ope_Com_Listado.Campo_Empleado_Autorizacion_ID +
                         ", (SELECT " + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno +
                         "||' '||" + Cat_Empleados.Campo_Apellido_Materno + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                         " WHERE " + Cat_Empleados.Campo_Empleado_ID + "= LIS." + Ope_Com_Listado.Campo_Empleado_Cancelacion_ID + ") AS " + Ope_Com_Listado.Campo_Empleado_Cancelacion_ID +

                         " FROM " + Ope_Com_Listado.Tabla_Ope_Com_Listado + " LIS" +
                         " WHERE " + Ope_Com_Listado.Campo_Listado_ID + "='" + Datos_Listado.P_Listado_ID+ "'";

            }
            Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Com_Listado.Campo_Listado_ID + " DESC";

            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }

         ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Listado
        ///DESCRIPCIÓN: Metodo que consulta la tabla de listado
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO:
        ///FECHA_CREO: 22/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Requisiciones_Listado(Cls_Ope_Alm_Seguimiento_Listado_Almacen_Negocio Datos_Listado)
        {
            String Mi_SQL = "";
            Mi_SQL = Mi_SQL + "SELECT";
            Mi_SQL = Mi_SQL + " REQ." + Ope_Com_Requisiciones.Campo_Folio;
            Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Estatus;
            Mi_SQL = Mi_SQL + ",(SELECT " + Cat_Empleados.Campo_Nombre;
            Mi_SQL = Mi_SQL + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno;
            Mi_SQL = Mi_SQL + "||' '||" + Cat_Empleados.Campo_Apellido_Materno;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Empleado_ID;
            Mi_SQL = Mi_SQL + " = REQ." + Ope_Com_Requisiciones.Campo_Cotizador_ID + ") AS COTIZADOR";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQ";
            Mi_SQL = Mi_SQL + " WHERE REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Com_Listado_Detalle.Campo_No_Requisicion;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Listado_Detalle.Tabla_Ope_Com_Listado_Detalle;
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Listado_Detalle.Campo_No_Listado_ID;
            Mi_SQL = Mi_SQL + "='" + Datos_Listado.P_Listado_ID.Trim() + "'";
            Mi_SQL = Mi_SQL + " GROUP BY " + Ope_Com_Listado_Detalle.Campo_No_Requisicion + ")";
            Mi_SQL = Mi_SQL + " OR REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Com_Requisiciones.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
            Mi_SQL = Mi_SQL + " WHERE REQ_ORIGEN_ID IN (SELECT " + Ope_Com_Listado_Detalle.Campo_No_Requisicion;  
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Listado_Detalle.Tabla_Ope_Com_Listado_Detalle;
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Listado_Detalle.Campo_No_Listado_ID;
            Mi_SQL = Mi_SQL + "='" + Datos_Listado.P_Listado_ID.Trim() + "'";
            Mi_SQL = Mi_SQL + " GROUP BY " + Ope_Com_Listado_Detalle.Campo_No_Requisicion + "))";

            Mi_SQL = Mi_SQL + " GROUP BY ";
            Mi_SQL = Mi_SQL + " REQ." + Ope_Com_Requisiciones.Campo_Folio;
            Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Cotizador_ID;



            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

        }

          ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Listado
        ///DESCRIPCIÓN: Metodo que consulta la tabla de listado
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO:
        ///FECHA_CREO: 22/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Detalles_Listado(Cls_Ope_Alm_Seguimiento_Listado_Almacen_Negocio Datos_Listado)
        {
            String Mi_SQL = "";

            Mi_SQL = "SELECT DET." + Ope_Com_Listado_Detalle.Campo_No_Producto_ID;
            Mi_SQL = Mi_SQL + ", PRO." + Cat_Com_Productos.Campo_Clave;
            Mi_SQL = Mi_SQL + ", PRO." + Cat_Com_Productos.Campo_Nombre;
            Mi_SQL = Mi_SQL + "||' '|| PRO." + Cat_Com_Productos.Campo_Descripcion + " AS NOMBRE_PRODUCTO";
            Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Com_Unidades.Campo_Nombre;
            Mi_SQL = Mi_SQL + "||' '||" + Cat_Com_Unidades.Campo_Abreviatura;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Unidades.Campo_Unidad_ID;
            Mi_SQL = Mi_SQL + "=PRO." + Cat_Com_Productos.Campo_Unidad_ID +") AS UNIDAD";
            Mi_SQL = Mi_SQL + ", DET." + Ope_Com_Listado_Detalle.Campo_Cantidad;
            Mi_SQL = Mi_SQL + ", DET." + Ope_Com_Listado_Detalle.Campo_No_Requisicion;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Listado_Detalle.Tabla_Ope_Com_Listado_Detalle + " DET";
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRO";
            Mi_SQL = Mi_SQL + " ON PRO." + Cat_Com_Productos.Campo_Producto_ID;
            Mi_SQL = Mi_SQL + "= DET." + Ope_Com_Listado_Detalle.Campo_No_Producto_ID;
            Mi_SQL = Mi_SQL + " WHERE DET." + Ope_Com_Listado_Detalle.Campo_No_Listado_ID;
            Mi_SQL = Mi_SQL + "='" + Datos_Listado.P_Listado_ID.Trim() + "'";
            Mi_SQL = Mi_SQL + " ORDER BY PRO." + Ope_Com_Req_Producto.Campo_Clave;



            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

         ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Listado_Reporte
        ///DESCRIPCIÓN: Metodo que consulta la tabla de listado
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO:Susana Trigueros Armenta
        ///FECHA_CREO:1/Noviembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Listado_Reporte(Cls_Ope_Alm_Seguimiento_Listado_Almacen_Negocio Datos_Listado)
        {
            String Mi_SQL = "";
            Mi_SQL = "SELECT LISTADO." + Ope_Com_Listado.Campo_Folio +
                    ",  TO_CHAR(LISTADO." + Ope_Com_Listado.Campo_Fecha_Generacion + ",'DD/MON/YYYY') AS " + Ope_Com_Listado.Campo_Fecha_Generacion +
                    ", LISTADO." + Ope_Com_Listado.Campo_Tipo +
                    ", LISTADO." + Ope_Com_Listado.Campo_Estatus +
                    ", LISTADO." + Ope_Com_Listado.Campo_Total +
                    ", LISTADO." + Ope_Com_Listado.Campo_Listado_ID +
                    ", (SELECT " + Cat_Sap_Partidas_Especificas.Campo_Clave + "||' '||" +
                    Cat_Sap_Partidas_Especificas.Campo_Nombre + " FROM " +
                    Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " WHERE " +
                    Cat_Sap_Partidas_Especificas.Campo_Partida_ID + "=LISTADO." + Ope_Com_Listado.Campo_No_Partida_ID + ") AS PARTIDA" +
                    " FROM " + Ope_Com_Listado.Tabla_Ope_Com_Listado + " LISTADO" +
                    " WHERE " + Ope_Com_Listado.Campo_Listado_ID + "='" + Datos_Listado.P_Listado_ID.Trim() + "'";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Listado_Detalles_Reporte
        ///DESCRIPCIÓN: Metodo que consulta la tabla de listado
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO:Susana Trigueros Armenta
        ///FECHA_CREO:1/Noviembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Listado_Detalles_Reporte(Cls_Ope_Alm_Seguimiento_Listado_Almacen_Negocio Datos_Listado)
        {
            String Mi_SQL = "SELECT LISTADO." + Ope_Com_Listado_Detalle.Campo_No_Producto_ID + " AS PRODUCTO_ID" +
                        ", PRODUCTO." + Cat_Com_Productos.Campo_Clave +
                        ", PRODUCTO." + Cat_Com_Productos.Campo_Nombre +
                        "||' '|| PRODUCTO." + Cat_Com_Productos.Campo_Descripcion + " AS NOMBRE_PRODUCTO" +
                        ", (SELECT " + Cat_Com_Unidades.Campo_Abreviatura + 
                        " FROM " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + 
                        " WHERE " + Cat_Com_Unidades.Campo_Unidad_ID + 
                        " = PRODUCTO." + Cat_Com_Unidades.Campo_Unidad_ID + ") AS UNIDAD" +  
                         ", LISTADO." + Ope_Com_Listado_Detalle.Campo_Cantidad +
                        ", LISTADO." + Ope_Com_Listado_Detalle.Campo_Costo_Compra +
                        ", LISTADO." + Ope_Com_Listado_Detalle.Campo_Importe +
                        ", LISTADO." + Ope_Com_Listado_Detalle.Campo_Monto_IVA +
                        ", LISTADO." + Ope_Com_Listado_Detalle.Campo_Monto_IEPS +
                        ", LISTADO." + Ope_Com_Listado_Detalle.Campo_Porcentaje_IVA +
                        ", LISTADO." + Ope_Com_Listado_Detalle.Campo_Porcentaje_IEPS +
                        " FROM " + Ope_Com_Listado_Detalle.Tabla_Ope_Com_Listado_Detalle + " LISTADO" +
                        " JOIN " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTO" +
                        " ON PRODUCTO." + Cat_Com_Productos.Campo_Producto_ID + " = LISTADO." + Ope_Com_Listado_Detalle.Campo_No_Producto_ID +
                        " WHERE LISTADO." + Ope_Com_Listado_Detalle.Campo_No_Listado_ID + " = '" + Datos_Listado.P_Listado_ID + "'" +
                        " ORDER BY PRODUCTO." + Cat_Com_Productos.Campo_Nombre;



            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

        }


    }
}//Fin del Namespace