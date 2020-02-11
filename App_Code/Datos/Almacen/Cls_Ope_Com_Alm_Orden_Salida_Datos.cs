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
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Orden_Salida.Negocio;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Bitacora_Eventos;

/// <summary>
/// Summary description for Cls_Ope_Com_Alm_Orden_Salida_Datos
/// </summary>
/// 

namespace Presidencia.Orden_Salida.Datos
{
    public class Cls_Ope_Com_Alm_Orden_Salida_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Requisiciones
        ///DESCRIPCIÓN:          Método utilizado para consultar las requisiciones de stock de almacén
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           17/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Requisiciones(Cls_Ope_Com_Alm_Orden_Salida_Negocio Datos)
        {
            // Declaración de variables
            String Mi_SQL = null;
            DataSet Ds_Requisiciones = null;
            DataTable Dt_Requisiciones = new DataTable();

            Mi_SQL = "SELECT DISTINCT O_SALIDA." + Alm_Com_Salidas.Campo_No_Salida + " as NO_ORDEN_SALIDA ";
            Mi_SQL = Mi_SQL + ",REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Estatus+ "";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Tipo + " as TIPO_REQUISICION";
            Mi_SQL = Mi_SQL + ", DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + " as DEPENDENCIA";
            //Mi_SQL = Mi_SQL + ", AREAS." + Cat_Areas.Campo_Nombre + " as AREA";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Codigo_Programatico + "";
            Mi_SQL = Mi_SQL + ", O_SALIDA." + Alm_Com_Salidas.Campo_Usuario_Creo + " as EMPLEADO_SURTIO";
            Mi_SQL = Mi_SQL + ", O_SALIDA." + Alm_Com_Salidas.Campo_Fecha_Creo + " as FECHA_SURTIDO";
            Mi_SQL = Mi_SQL + ", O_SALIDA." + Alm_Com_Salidas.Campo_Subtotal + "";
            Mi_SQL = Mi_SQL + ", O_SALIDA." + Alm_Com_Salidas.Campo_IVA + "";
            Mi_SQL = Mi_SQL + ", O_SALIDA." + Alm_Com_Salidas.Campo_Total + "";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES";
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
            Mi_SQL = Mi_SQL + " ON REQUISICIONES." + Ope_Com_Requisiciones.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID;
            //Mi_SQL = Mi_SQL + " JOIN " + Cat_Areas.Tabla_Cat_Areas + " AREAS";
            //Mi_SQL = Mi_SQL + " ON REQUISICIONES." + Ope_Com_Requisiciones.Campo_Area_ID;
            //Mi_SQL = Mi_SQL + " = AREAS." + Cat_Areas.Campo_Area_ID;
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS";
            Mi_SQL = Mi_SQL + " ON REQUISICIONES." + Ope_Com_Requisiciones.Campo_Empleado_Surtido_ID;
            Mi_SQL = Mi_SQL + " = EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID;

            Mi_SQL = Mi_SQL + " JOIN " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + " O_SALIDA";
            Mi_SQL = Mi_SQL + " ON O_SALIDA." + Alm_Com_Salidas.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + " = REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID;

            Mi_SQL = Mi_SQL + " JOIN " + Alm_Com_Salidas_Detalles.Tabla_Alm_Com_Salidas_Detalles+ " SALIDA_DETALLES";
            Mi_SQL = Mi_SQL + " ON SALIDA_DETALLES." + Alm_Com_Salidas_Detalles.Campo_No_Salida;
            Mi_SQL = Mi_SQL + " = O_SALIDA." + Alm_Com_Salidas.Campo_No_Salida;

            Mi_SQL = Mi_SQL + " WHERE  REQUISICIONES." + Ope_Com_Requisiciones.Campo_Tipo + " IS NOT NULL";
            
            if (Datos.P_No_Orden_Salida != null)
            {
                Mi_SQL = Mi_SQL + " and  O_SALIDA." + Alm_Com_Salidas.Campo_No_Salida +  " like '%" + Datos.P_No_Orden_Salida + "%'";
            }
            if (Datos.P_No_Orden_Compra != null)
            {
                Mi_SQL = Mi_SQL + " and  REQUISICIONES." + Ope_Com_Requisiciones.Campo_No_Orden_Compra + " like '%" + Datos.P_No_Orden_Compra + "%'";
            }
            if (Datos.P_No_Requisicion != null)
            {
                Mi_SQL = Mi_SQL + " and  REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " like '%" + Datos.P_No_Requisicion + "%'";    
            }
            if (Datos.P_Dependencia != null)
            {
                Mi_SQL = Mi_SQL + " and  REQUISICIONES." + Ope_Com_Requisiciones.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia + "'";
            }
            //if (Datos.P_Area != null)
            //{
            //    Mi_SQL = Mi_SQL + " and  REQUISICIONES." + Ope_Com_Requisiciones.Campo_Area_ID + "= '" + Datos.P_Area + "'";
            //}
            if (Datos.P_Tipo_Salida!= null)
            {
                Mi_SQL = Mi_SQL + " and REQUISICIONES." + Ope_Com_Requisiciones.Campo_Tipo + " = '" + Datos.P_Tipo_Salida + "'";
            }
            if ((Datos.P_Fecha_Inicial != null) && (Datos.P_Fecha_Final != null))
            {
                Mi_SQL = Mi_SQL + " AND TO_DATE(TO_CHAR(O_SALIDA." + Alm_Com_Salidas.Campo_Fecha_Creo + ",'DD/MM/YY')) BETWEEN '" +
                string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Inicial)) + "'" +
                " AND '" + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Final)) + "'";
            }

            Mi_SQL = Mi_SQL + " order by O_SALIDA." + Alm_Com_Salidas.Campo_No_Salida + " DESC";

            Ds_Requisiciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            Dt_Requisiciones = Ds_Requisiciones.Tables[0];
            return Dt_Requisiciones;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Productos_Requisicion
        ///DESCRIPCIÓN:          Método utilizado para consultar los productos de 
        ///                      las requisiciones de stock de almacén
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           17/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Productos_Orden_Salida(Cls_Ope_Com_Alm_Orden_Salida_Negocio Datos)
        {
            // Declaración de variables
            String Mi_SQL = null;
            DataSet Ds_Productos_Requisicion = null;
            DataTable Dt_Productos_Requisicion = new DataTable();

            Mi_SQL = " SELECT DISTINCT REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + ", SALIDAS." + Alm_Com_Salidas.Campo_No_Salida + " ";
            Mi_SQL = Mi_SQL + ", SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + "";
            Mi_SQL = Mi_SQL + ", SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Cantidad + " as CANTIDAD_ENTREGADA";
            Mi_SQL = Mi_SQL + ", SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Costo + "";
            Mi_SQL = Mi_SQL + ", SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Importe + "";
            Mi_SQL = Mi_SQL + ", SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Subtotal + "";
            Mi_SQL = Mi_SQL + ", SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_IVA + "";
            Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Cantidad + " as CANTIDAD_SOLICITADA ";

            Mi_SQL = Mi_SQL + ", ( SELECT " + Cat_Com_Productos.Campo_Nombre + " FROM ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " WHERE ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + " = SALIDAS_DETALLES.";
            Mi_SQL = Mi_SQL + Alm_Com_Salidas_Detalles.Campo_Producto_ID + " AND SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_No_Salida;
            Mi_SQL = Mi_SQL + " = " + Datos.P_No_Orden_Salida.Trim() + " ) AS PRODUCTO ";

            Mi_SQL = Mi_SQL + ", ( SELECT " + Cat_Com_Productos.Campo_Descripcion + " FROM ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " WHERE ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + " = SALIDAS_DETALLES.";
            Mi_SQL = Mi_SQL + Alm_Com_Salidas_Detalles.Campo_Producto_ID + " AND SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_No_Salida;
            Mi_SQL = Mi_SQL + " = " + Datos.P_No_Orden_Salida.Trim() + " ) AS DESCRIPCION ";

            Mi_SQL = Mi_SQL + ",(SELECT UNIDADES." + Cat_Com_Unidades.Campo_Abreviatura + " FROM ";
            Mi_SQL = Mi_SQL + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " UNIDADES ";
            Mi_SQL = Mi_SQL + " WHERE UNIDADES." + Cat_Com_Unidades.Campo_Unidad_ID + " = ";
            Mi_SQL = Mi_SQL + " (SELECT PRODUCTOS." + Cat_Com_Productos.Campo_Unidad_ID + " FROM ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
            Mi_SQL = Mi_SQL + " WHERE SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + " = ";
            Mi_SQL = Mi_SQL + " PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID + ")) as UNIDAD ";

            Mi_SQL = Mi_SQL + " FROM " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + " SALIDAS, ";
            Mi_SQL = Mi_SQL + " " + Alm_Com_Salidas_Detalles.Tabla_Alm_Com_Salidas_Detalles + " SALIDAS_DETALLES, ";
            Mi_SQL = Mi_SQL + " " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO ";

            Mi_SQL = Mi_SQL + " WHERE  SALIDAS." + Alm_Com_Salidas.Campo_No_Salida + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_No_Salida + "  ";
            Mi_SQL = Mi_SQL + " and SALIDAS." + Alm_Com_Salidas.Campo_No_Salida + " = ";
            Mi_SQL = Mi_SQL + Datos.P_No_Orden_Salida;
            Mi_SQL = Mi_SQL + " and REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS." + Alm_Com_Salidas.Campo_Requisicion_ID + "  ";
            Mi_SQL = Mi_SQL + " and REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Producto_ID+ "  ";
            Ds_Productos_Requisicion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            Dt_Productos_Requisicion = Ds_Productos_Requisicion.Tables[0];           
            return Dt_Productos_Requisicion;
        }
     
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_DataTable
        ///DESCRIPCIÓN:          Método utilizado para consultar loas dependencias y las áreas
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           18/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_DataTable(Cls_Ope_Com_Alm_Orden_Salida_Negocio Datos)
        {
            // Declaración de Variables
            String Mi_SQL = null;
            DataSet Ds_Consulta = null;
            DataTable Dt_consulta = new DataTable();

            try
            {
                if (Datos.P_Tipo_Data_Table.Equals("DEPENDENCIAS"))
                {
                    Mi_SQL = "SELECT " + Cat_Dependencias.Campo_Dependencia_ID + " AS DEPENDENCIA_ID, ";
                    Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Dependencia_ID + " || ' ' || ";
                    Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Comentarios + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias;

                    Mi_SQL = Mi_SQL + " order by  " + Cat_Dependencias.Campo_Comentarios;
                }
                else if (Datos.P_Tipo_Data_Table.Equals("AREAS"))
                {
                    Mi_SQL = "SELECT AREAS." + Cat_Areas.Campo_Area_ID + " AS AREA_ID";
                    Mi_SQL = Mi_SQL + " , AREAS." + Cat_Areas.Campo_Nombre + " AS NOMBRE ";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Areas.Tabla_Cat_Areas + " AREAS ";

                    if (Datos.P_Dependencia != "")
                    {
                        Mi_SQL = Mi_SQL + " WHERE AREAS." + Cat_Areas.Campo_Dependencia_ID + " = ";
                        Mi_SQL = Mi_SQL + "'" + Datos.P_Dependencia + "'";
                    }
                    Mi_SQL = Mi_SQL + " order by " + Cat_Areas.Campo_Nombre + " ";
                }

                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
                {
                    Ds_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Consulta != null)
                {
                    Dt_consulta= Ds_Consulta.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_consulta;
        }



        // PARA LA REIMPRESION



        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Informacion_General_OS
        ///DESCRIPCIÓN:          Método donde se consulta la información general de la orden de salida que se genero
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           12/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Informacion_General_OS(Cls_Ope_Com_Alm_Orden_Salida_Negocio Datos)
        {
            // Declaración de variables
            String Mi_SQL = null;
            DataTable Dt_Cabecera = new DataTable();

            Mi_SQL = "SELECT " + "SALIDAS." + Alm_Com_Salidas.Campo_No_Salida + " as NO_ORDEN_SALIDA";
            Mi_SQL = Mi_SQL + ",(select DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + " from ";
            Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS ";
            Mi_SQL = Mi_SQL + " where SALIDAS." + Alm_Com_Salidas.Campo_Dependencia_ID + " = DEPENDENCIAS.";
            Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Dependencia_ID + ")as UNIDAD_RESPONSABLE";

            Mi_SQL = Mi_SQL + ",(select distinct (FINANCIAMIENTO." + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + ")";
            Mi_SQL = Mi_SQL + " from " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + " FINANCIAMIENTO ";
            Mi_SQL = Mi_SQL + "  where FINANCIAMIENTO." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID;
            Mi_SQL = Mi_SQL + " = (select distinct(REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID + ") from ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO ";
            Mi_SQL = Mi_SQL + " where  REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS." + Alm_Com_Salidas.Campo_Requisicion_ID + "))as F_FINANCIAMIENTO";

            Mi_SQL = Mi_SQL + ",(select distinct (PROY_PROGRAMAS." + Cat_Com_Proyectos_Programas.Campo_Descripcion + ")";
            Mi_SQL = Mi_SQL + " from " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + " PROY_PROGRAMAS ";
            Mi_SQL = Mi_SQL + "  where PROY_PROGRAMAS." + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID;
            Mi_SQL = Mi_SQL + " =(select distinct (REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID + ") from ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO ";
            Mi_SQL = Mi_SQL + " where  REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS." + Alm_Com_Salidas.Campo_Requisicion_ID + "))as PROGRAMA";

            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio;
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion;
            Mi_SQL = Mi_SQL + ", SALIDAS." + Alm_Com_Salidas.Campo_Usuario_Creo + " as ENTREGO ";

            Mi_SQL = Mi_SQL + ", (select EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + " ||' '||";
            Mi_SQL = Mi_SQL + " EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + " ||' '||";
            Mi_SQL = Mi_SQL + " EMPLEADOS." + Cat_Empleados.Campo_Nombre;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS ";
            Mi_SQL = Mi_SQL + " where EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS." + Alm_Com_Salidas.Campo_Empleado_Solicito_ID + ") as RECIBIO";

            Mi_SQL = Mi_SQL + " FROM " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + " SALIDAS ";
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";
            Mi_SQL = Mi_SQL + " where REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS." + Alm_Com_Salidas.Campo_Requisicion_ID + "";
            Mi_SQL = Mi_SQL + " AND SALIDAS." + Alm_Com_Salidas.Campo_No_Salida + " = ";
            Mi_SQL = Mi_SQL + Datos.P_No_Orden_Salida;

            Dt_Cabecera = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Cabecera;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Detalles_Orden_Salida
        ///DESCRIPCIÓN:          Método donde se consultan los detalles de la orden de salida que se genero
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           24/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Detalles_Orden_Salida(Cls_Ope_Com_Alm_Orden_Salida_Negocio Datos)
        {
            // Declaración de variables
            String Mi_SQL = null;
            DataTable Dt_Detalles = new DataTable();

            Mi_SQL = "SELECT SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_No_Salida + " as NO_ORDEN_SALIDA";
            Mi_SQL = Mi_SQL + ",(select PRODUCTOS." + Cat_Com_Productos.Campo_Clave + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + " = PRODUCTOS.";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as CLAVE";

            Mi_SQL = Mi_SQL + ",(select PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + " ||' '|| ";
            Mi_SQL = Mi_SQL + " PRODUCTOS." + Cat_Com_Productos.Campo_Descripcion + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + " = PRODUCTOS.";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as PRODUCTO";

            Mi_SQL = Mi_SQL + ",(select REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Cantidad + " from ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where  REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + " (select SALIDAS." + Alm_Com_Salidas.Campo_Requisicion_ID + " from ";
            Mi_SQL = Mi_SQL + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + " SALIDAS ";
            Mi_SQL = Mi_SQL + " where  SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_No_Salida + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS." + Alm_Com_Salidas.Campo_No_Salida + ")";
            Mi_SQL = Mi_SQL + " and REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + ") as CANTIDAD_SOLICITADA ";

            Mi_SQL = Mi_SQL + ",SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Cantidad + " as CANTIDAD_ENTREGADA";

            Mi_SQL = Mi_SQL + ",(select UNIDADES." + Cat_Com_Unidades.Campo_Abreviatura + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " UNIDADES ";
            Mi_SQL = Mi_SQL + " where  UNIDADES." + Cat_Com_Unidades.Campo_Unidad_ID + " = ";
            Mi_SQL = Mi_SQL + " (select PRODUCTOS." + Cat_Com_Productos.Campo_Unidad_ID + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where  SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + " = ";
            Mi_SQL = Mi_SQL + " PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID + ")) as UNIDADES";

            Mi_SQL = Mi_SQL + ",SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Costo + " as PRECIO";
            Mi_SQL = Mi_SQL + ",SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Subtotal + "";
            Mi_SQL = Mi_SQL + ",SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_IVA + "";
            Mi_SQL = Mi_SQL + ",SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Importe + " as TOTAL";

            Mi_SQL = Mi_SQL + " FROM " + Alm_Com_Salidas_Detalles.Tabla_Alm_Com_Salidas_Detalles + " SALIDAS_DETALLES";
            Mi_SQL = Mi_SQL + " WHERE SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_No_Salida + " = ";
            Mi_SQL = Mi_SQL + Datos.P_No_Orden_Salida;

            Dt_Detalles = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Detalles;
        }

    }
}