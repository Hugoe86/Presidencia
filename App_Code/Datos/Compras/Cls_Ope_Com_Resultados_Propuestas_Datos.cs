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
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Resultados_Propuesta.Negocios;


/// <summary>
/// Summary description for Cls_Ope_Com_Resultados_Propuestas_Datos
/// </summary>

namespace Presidencia.Resultados_Propuesta.Datos
{
    public class Cls_Ope_Com_Resultados_Propuestas_Datos
    {
        public static bool Guardar_Cotizacion(Cls_Ope_Com_Resultados_Propuestas_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";
            //OBTENEMOS EL ID DEL PROVEEDOR QUE SE LOGUEO
            DataTable Dt_Proveedor_sesion = (DataTable)Cls_Sessiones.Datos_Proveedor;

            bool Operacion_Realizada = false;
            try
            {
                for (int i = 0; i < Clase_Negocio.P_Dt_Productos.Rows.Count; i++)
                {
                    Mi_SQL = "UPDATE " + Ope_Com_Propuesta_Cotizacion.Tabla_Ope_Com_Propuesta_Cotizacion;
                    Mi_SQL = Mi_SQL + " SET " + Ope_Com_Propuesta_Cotizacion.Campo_IVA_Cotizado + "='";
                    Mi_SQL = Mi_SQL + Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Propuesta_Cotizacion.Campo_IVA_Cotizado].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_IEPS_Cotizado + "='";
                    Mi_SQL = Mi_SQL + Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Propuesta_Cotizacion.Campo_IEPS_Cotizado].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Subtota_Cotizado + "='";
                    Mi_SQL = Mi_SQL + Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Propuesta_Cotizacion.Campo_Subtota_Cotizado].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Total_Cotizado + "='";
                    Mi_SQL = Mi_SQL + Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Propuesta_Cotizacion.Campo_Total_Cotizado].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Total_Cotizado_Requisicion + "='";
                    Mi_SQL = Mi_SQL + Clase_Negocio.P_Total_Cotizado + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Subtotal_Cotizado_Requisicion + "='";
                    Mi_SQL = Mi_SQL + Clase_Negocio.P_Subtotal_Cotizado + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Precio_U_Sin_Imp_Cotizado + "='";
                    Mi_SQL = Mi_SQL + Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Propuesta_Cotizacion.Campo_Precio_U_Sin_Imp_Cotizado].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Precio_U_Con_Imp_Cotizado + "='";
                    Mi_SQL = Mi_SQL + Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Propuesta_Cotizacion.Campo_Precio_U_Con_Imp_Cotizado].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Marca + "='";
                    Mi_SQL = Mi_SQL + Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Propuesta_Cotizacion.Campo_Marca].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Descripcion_Producto + "='";
                    Mi_SQL = Mi_SQL + Clase_Negocio.P_Dt_Productos.Rows[i]["DESCRIPCION_PRODUCTO_COT"].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_IEPS_Cotizado_Req + "='";
                    Mi_SQL = Mi_SQL + Clase_Negocio.P_IEPS_Cotizado + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_IVA_Cotizado_Req + "='";
                    Mi_SQL = Mi_SQL + Clase_Negocio.P_IVA_Cotizado + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Registro_Padron_Prov + "='";
                    Mi_SQL = Mi_SQL + Clase_Negocio.P_Reg_Padron_Proveedor + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Vigencia_Propuesta + "='";
                    Mi_SQL = Mi_SQL + Clase_Negocio.P_Vigencia_Propuesta + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Fecha_Elaboracion + "='";
                    Mi_SQL = Mi_SQL + Clase_Negocio.P_Fecha_Elaboracion + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Garantia + "='";
                    Mi_SQL = Mi_SQL + Clase_Negocio.P_Garantia + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Tiempo_Entrega + "='";
                    Mi_SQL = Mi_SQL + Clase_Negocio.P_Tiempo_Entrega + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Estatus + "='";
                    Mi_SQL = Mi_SQL + Clase_Negocio.P_Estatus_Propuesta + "'";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Propuesta_Cotizacion.Campo_Proveedor_ID;
                    Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Proveedor_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Propuesta_Cotizacion.Campo_No_Requisicion;
                    Mi_SQL = Mi_SQL + " ='" + Clase_Negocio.P_No_Requisicion + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Propuesta_Cotizacion.Campo_Ope_Com_Req_Producto_ID;
                    Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Propuesta_Cotizacion.Campo_Ope_Com_Req_Producto_ID].ToString().Trim() + "'";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    Operacion_Realizada = true;


                }//fin del For 
            }
            catch
            {
                Operacion_Realizada = false;
            }
            return Operacion_Realizada;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:  Consultar_Productos_Servicios
        ///DESCRIPCIÓN: Metodo que Consulta los detalles de la Requisicion seleccionada, ya sea Producto o servicio.
        ///PARAMETROS: 1.- Cls_Ope_Com_Definir_Cotizadores_Negocio Clase_Negocios, objeto de la clase de negocios
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 01/JULIO/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Propuesta_Cotizacion(Cls_Ope_Com_Resultados_Propuestas_Negocio Clase_Negocio)
        {
            String Mi_SQL = "SELECT ";
            Mi_SQL = Mi_SQL + " PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_IEPS_Cotizado_Req;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_IVA_Cotizado_Req;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Total_Cotizado_Requisicion;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Subtotal_Cotizado_Requisicion;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Garantia;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", TO_CHAR(PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Fecha_Elaboracion + ",'DD/MON/YYYY') AS " + Ope_Com_Propuesta_Cotizacion.Campo_Fecha_Elaboracion;
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Tiempo_Entrega;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Registro_Padron_Prov;
            Mi_SQL = Mi_SQL + ",TO_CHAR(PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Vigencia_Propuesta + ",'DD/MON/YYYY') AS " + Ope_Com_Propuesta_Cotizacion.Campo_Vigencia_Propuesta;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Propuesta_Cotizacion.Tabla_Ope_Com_Propuesta_Cotizacion;
            Mi_SQL = Mi_SQL + " PROPUESTA WHERE PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_No_Requisicion;
            Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_No_Requisicion + "'";
            Mi_SQL = Mi_SQL + " AND PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Proveedor_ID;
            Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Proveedor_ID + "'";
            Mi_SQL = Mi_SQL + " AND PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Estatus;
            Mi_SQL = Mi_SQL + "='COTIZADA'";
            Mi_SQL = Mi_SQL + " GROUP BY (";
            Mi_SQL = Mi_SQL + " PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_IEPS_Cotizado_Req;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_IVA_Cotizado_Req;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Total_Cotizado_Requisicion;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Subtotal_Cotizado_Requisicion;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Garantia;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Fecha_Elaboracion;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Tiempo_Entrega;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Registro_Padron_Prov;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Vigencia_Propuesta + ")";



            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];



        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:  Consultar_Productos_Servicios
        ///DESCRIPCIÓN: Metodo que Consulta los detalles de la Requisicion seleccionada, ya sea Producto o servicio.
        ///PARAMETROS: 1.- Cls_Ope_Com_Definir_Cotizadores_Negocio Clase_Negocios, objeto de la clase de negocios
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 01/JULIO/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Productos_Servicios(Cls_Ope_Com_Resultados_Propuestas_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";
            switch (Clase_Negocio.P_Tipo_Articulo)
            {
                case "PRODUCTO":
                    Mi_SQL = "SELECT PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Prod_Serv_ID;
                    Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Ope_Com_Req_Producto_ID;
                    Mi_SQL = Mi_SQL + ", PRO." + Cat_Com_Productos.Campo_Nombre;
                    Mi_SQL = Mi_SQL + "||' '|| PRO." + Cat_Com_Productos.Campo_Clave + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + ", PRO." + Cat_Com_Productos.Campo_Descripcion;
                    Mi_SQL = Mi_SQL + ",(SELECT " + Cat_Com_Impuestos.Campo_Porcentaje_Impuesto + " FROM ";
                    Mi_SQL = Mi_SQL + Cat_Com_Impuestos.Tabla_Cat_Impuestos + " WHERE ";
                    Mi_SQL = Mi_SQL + Cat_Com_Impuestos.Campo_Impuesto_ID + "= PRO." + Cat_Com_Productos.Campo_Impuesto_ID;
                    Mi_SQL = Mi_SQL + ") AS PORCENTAJE_IMPUESTO";
                    Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Cantidad;
                    Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Total_Cotizado;
                    Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Subtota_Cotizado;
                    Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_IVA_Cotizado;
                    Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_IEPS_Cotizado;
                    Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Precio_U_Con_Imp_Cotizado;
                    Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Precio_U_Sin_Imp_Cotizado;
                    Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Marca;
                    Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Descripcion_Producto + " AS DESCRIPCION_PRODUCTO_COT";
                    Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Resultado;
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Propuesta_Cotizacion.Tabla_Ope_Com_Propuesta_Cotizacion + " PROPUESTA";
                    Mi_SQL = Mi_SQL + " INNER JOIN " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRO";
                    Mi_SQL = Mi_SQL + " ON PRO." + Cat_Com_Productos.Campo_Producto_ID + "=";
                    Mi_SQL = Mi_SQL + " PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Prod_Serv_ID;
                    Mi_SQL = Mi_SQL + " WHERE PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_No_Requisicion + "='" + Clase_Negocio.P_No_Requisicion.Trim() + "'";
                    Mi_SQL = Mi_SQL + " AND PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Proveedor_ID;
                    Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Proveedor_ID + "'";

                    break;
                case "SERVICIO":
                    Mi_SQL = "SELECT PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Prod_Serv_ID;
                    Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Ope_Com_Req_Producto_ID;
                    Mi_SQL = Mi_SQL + ", SER." + Cat_Com_Servicios.Campo_Clave;
                    Mi_SQL = Mi_SQL + " ||' '|| SER." + Cat_Com_Servicios.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + ", SER." + Cat_Com_Servicios.Campo_Comentarios + " AS DESCRIPCION";
                    Mi_SQL = Mi_SQL + ",(SELECT " + Cat_Com_Impuestos.Campo_Porcentaje_Impuesto + " FROM ";
                    Mi_SQL = Mi_SQL + Cat_Com_Impuestos.Tabla_Cat_Impuestos + " WHERE ";
                    Mi_SQL = Mi_SQL + Cat_Com_Impuestos.Campo_Impuesto_ID + "= SER." + Cat_Com_Productos.Campo_Impuesto_ID;
                    Mi_SQL = Mi_SQL + ") AS PORCENTAJE_IMPUESTO";
                    Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Cantidad;
                    Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Total_Cotizado;
                    Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Subtota_Cotizado;
                    Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_IVA_Cotizado;
                    Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_IEPS_Cotizado;
                    Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Precio_U_Con_Imp_Cotizado;
                    Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Precio_U_Sin_Imp_Cotizado;
                    Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Marca;
                    Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Descripcion_Producto + " AS DESCRIPCION_PRODUCTO_COT";
                    Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Resultado;
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Propuesta_Cotizacion.Tabla_Ope_Com_Propuesta_Cotizacion + " PROPUESTA";
                    Mi_SQL = Mi_SQL + " INNER JOIN " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + " SER";
                    Mi_SQL = Mi_SQL + " ON SER." + Cat_Com_Servicios.Campo_Servicio_ID + "=";
                    Mi_SQL = Mi_SQL + " PROPUESTA." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID;
                    Mi_SQL = Mi_SQL + " WHERE PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_No_Requisicion + "='" + Clase_Negocio.P_No_Requisicion.Trim() + "'";
                    Mi_SQL = Mi_SQL + " AND PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Proveedor_ID;
                    Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Proveedor_ID.Trim() + "'";

                    break;

            }
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:  Consultar_Requisiciones
        ///DESCRIPCIÓN: Metodo que consulta las requisiciones listas para ser distribuidas a los cotizadores
        ///PARAMETROS: 1.- Cls_Ope_Com_Definir_Cotizadores_Negocio Clase_Negocios, objeto de la clase de negocios
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 01/JULIO/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Requisiciones(Cls_Ope_Com_Resultados_Propuestas_Negocio Clase_Negocio)
        {
            //OBTENEMOS EL ID DEL PROVEEDOR QUE SE LOGUEO
            DataTable Dt_Proveedor_sesion = (DataTable)Cls_Sessiones.Datos_Proveedor;

            String Mi_SQL = "SELECT REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Folio;
            Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Tipo_Articulo;
            Mi_SQL = Mi_SQL + ", TO_CHAR(PROPUESTA." + Ope_Com_Requisiciones.Campo_Fecha_Creo;
            Mi_SQL = Mi_SQL + ",'DD/MON/YYYY') AS FECHA";
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Nombre_Cotizador;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Proveedor_ID;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Estatus;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQ";
            Mi_SQL = Mi_SQL + " JOIN " + Ope_Com_Propuesta_Cotizacion.Tabla_Ope_Com_Propuesta_Cotizacion + " PROPUESTA";
            Mi_SQL = Mi_SQL + " ON PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_No_Requisicion + "=";
            Mi_SQL = Mi_SQL + " REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + " WHERE REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " IN";
            Mi_SQL = Mi_SQL + "(SELECT " + Ope_Com_Propuesta_Cotizacion.Campo_No_Requisicion;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Propuesta_Cotizacion.Tabla_Ope_Com_Propuesta_Cotizacion;
            Mi_SQL = Mi_SQL + " GROUP BY (" + Ope_Com_Propuesta_Cotizacion.Campo_No_Requisicion + "))";
            if (Clase_Negocio.P_Requisicion_Busqueda != null)
            {
                Mi_SQL = Mi_SQL + " AND UPPER(PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_No_Requisicion + ") LIKE UPPER('%" + Clase_Negocio.P_Requisicion_Busqueda.Trim() + "%')";
            }

            if (Clase_Negocio.P_Busqueda_Fecha_Generacion_Ini != null)
            {
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Propuesta_Cotizacion.Campo_Fecha_Elaboracion;
                Mi_SQL = Mi_SQL + " BETWEEN TO_DATE ('" + Clase_Negocio.P_Busqueda_Fecha_Generacion_Ini + " 00:00:00', 'DD-MMM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Clase_Negocio.P_Busqueda_Fecha_Generacion_Fin + " 23:59:00', 'DD-MMM-YYYY HH24:MI:SS')";
            }

            if (Clase_Negocio.P_Busqueda_Fecha_Entrega_Ini != null)
            {
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Propuesta_Cotizacion.Campo_Tiempo_Entrega;
                Mi_SQL = Mi_SQL + " BETWEEN TO_DATE ('" + Clase_Negocio.P_Busqueda_Fecha_Entrega_Ini + " 00:00:00', 'DD-MMM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Clase_Negocio.P_Busqueda_Fecha_Entrega_Fin + " 23:59:00', 'DD-MMM-YYYY HH24:MI:SS')";
            }
            if (Clase_Negocio.P_Busqueda_Vigencia_Propuesta_Ini != null)
            {
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Propuesta_Cotizacion.Campo_Tiempo_Entrega;
                Mi_SQL = Mi_SQL + " BETWEEN TO_DATE ('" + Clase_Negocio.P_Busqueda_Vigencia_Propuesta_Ini + " 00:00:00', 'DD-MMM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Clase_Negocio.P_Busqueda_Vigencia_Propuesta_Fin + " 23:59:00', 'DD-MMM-YYYY HH24:MI:SS')";
            }

            if (Clase_Negocio.P_Busqueda_Cotizador != null)
            {
                Mi_SQL = Mi_SQL + " AND UPPER(" + Ope_Com_Propuesta_Cotizacion.Campo_Nombre_Cotizador + ")";
                Mi_SQL = Mi_SQL + " LIKE UPPER('%" + Clase_Negocio.P_Busqueda_Cotizador + "%')";
            }

            Mi_SQL = Mi_SQL + " AND PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Proveedor_ID + "='" + Dt_Proveedor_sesion.Rows[0]["Proveedor_ID"].ToString().Trim() + "'";
            Mi_SQL = Mi_SQL + " AND PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Estatus + "='COTIZADA'";
            Mi_SQL = Mi_SQL + " GROUP BY ( REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Folio;
            Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Tipo_Articulo;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Requisiciones.Campo_Fecha_Creo;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Nombre_Cotizador;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Proveedor_ID;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Estatus + ")";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:  Consultar_Detalle_Requisicion
        ///DESCRIPCIÓN: Metodo que consulta los detalles de la requisicion seleccionada en el Grid_Requisiciones
        ///PARAMETROS: 1.- Cls_Ope_Com_Definir_Cotizadores_Negocio Clase_Negocios, objeto de la clase de negocios
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 01/JULIO/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Detalle_Requisicion(Cls_Ope_Com_Resultados_Propuestas_Negocio Clase_Negocio)
        {
            String Mi_SQL = "SELECT ";
            Mi_SQL = Mi_SQL + " DEPENDENCIA." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA";


            Mi_SQL = Mi_SQL + ",(SELECT " + Cat_Sap_Concepto.Campo_Clave + " ||' '|| " + Cat_Sap_Concepto.Campo_Descripcion + " FROM ";
            Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + " WHERE " + Cat_Sap_Concepto.Campo_Concepto_ID + "=(SELECT ";
            Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Genericas.Campo_Concepto_ID + " FROM ";
            Mi_SQL = Mi_SQL + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + " WHERE ";
            Mi_SQL = Mi_SQL + "CAT_SAP_PARTIDA_GENERICA." + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID;
            Mi_SQL = Mi_SQL + "=(SELECT " + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID + " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " = REQUISICION." + Ope_Com_Requisiciones.Campo_Partida_ID + "))) AS CONCEPTO";
            Mi_SQL = Mi_SQL + ",(SELECT " + Cat_Sap_Concepto.Campo_Concepto_ID + " FROM ";
            Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + " WHERE " + Cat_Sap_Concepto.Campo_Concepto_ID + "=(SELECT ";
            Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Genericas.Campo_Concepto_ID + " FROM ";
            Mi_SQL = Mi_SQL + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + " WHERE ";
            Mi_SQL = Mi_SQL + "CAT_SAP_PARTIDA_GENERICA." + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID;
            Mi_SQL = Mi_SQL + "=(SELECT " + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID + " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " = REQUISICION." + Ope_Com_Requisiciones.Campo_Partida_ID + "))) AS CONCEPTO_ID";
            Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_Tipo;
            Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_Folio;
            Mi_SQL = Mi_SQL + ", TO_CHAR( REQUISICION." + Ope_Com_Requisiciones.Campo_Fecha_Generacion + ",'DD/MON/YYYY') AS FECHA_GENERACION";
            Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_Subtotal;
            Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_IEPS;
            Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_IVA;
            Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_Total;
            Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_Justificacion_Compra;
            Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_Especificacion_Prod_Serv;
            Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_Verificaion_Entrega;
            Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_Tipo_Articulo;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICION ";
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIA ";
            Mi_SQL = Mi_SQL + " ON REQUISICION." + Ope_Com_Requisiciones.Campo_Dependencia_ID + "= DEPENDENCIA.";
            Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + " JOIN " + Ope_Com_Propuesta_Cotizacion.Tabla_Ope_Com_Propuesta_Cotizacion + " PROPUESTA ";
            Mi_SQL = Mi_SQL + " ON PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_No_Requisicion;
            Mi_SQL = Mi_SQL + "= REQUISICION." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + " WHERE REQUISICION." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_No_Requisicion.Trim() + "'";
            Mi_SQL = Mi_SQL + " AND PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Proveedor_ID;
            Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Proveedor_ID + "'";
            Mi_SQL = Mi_SQL + " ";




            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Impuesto_Producto
        ///DESCRIPCIÓN: Metodo que consulta los impuestos de los productos
        ///PARAMETROS:1.- Cls_Ope_Com_Licitacion_Proveedores_Negocio Datos_Lic_Pro
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 15/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Impuesto_Producto(Cls_Ope_Com_Resultados_Propuestas_Negocio Clase_Negocio)
        {
            String Mi_SQL = "SELECT PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID +
                         ", PRODUCTOS." + Cat_Com_Productos.Campo_Clave +
                         ", PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + " AS PRODUCTO_NOMBRE" +
                         ", PRODUCTOS." + Cat_Com_Productos.Campo_Costo + " AS PRECIO_UNITARIO" +
                         ", PRODUCTOS." + Cat_Com_Productos.Campo_Impuesto_ID +
                         ", PRODUCTOS." + Cat_Com_Productos.Campo_Impuesto_2_ID +
                         ",(SELECT " + Cat_Com_Impuestos.Campo_Nombre +
                         " FROM " + Cat_Com_Impuestos.Tabla_Cat_Impuestos +
                         " WHERE PRODUCTOS." + Cat_Com_Productos.Campo_Impuesto_ID + "=" +
                         Cat_Com_Impuestos.Campo_Impuesto_ID + ") AS TIPO_IMPUESTO_1 " +
                         ",(SELECT " + Cat_Com_Impuestos.Campo_Nombre +
                         " FROM " + Cat_Com_Impuestos.Tabla_Cat_Impuestos +
                         " WHERE PRODUCTOS." + Cat_Com_Productos.Campo_Impuesto_2_ID + "=" +
                         Cat_Com_Impuestos.Campo_Impuesto_ID + ") AS TIPO_IMPUESTO_2 " +
                         ",(SELECT " + Cat_Com_Impuestos.Campo_Porcentaje_Impuesto +
                         " FROM " + Cat_Com_Impuestos.Tabla_Cat_Impuestos +
                         " WHERE PRODUCTOS." + Cat_Com_Productos.Campo_Impuesto_ID + "=" +
                         Cat_Com_Impuestos.Campo_Impuesto_ID + ") AS IMPUESTO_PORCENTAJE_1 " +
                         ",(SELECT " + Cat_Com_Impuestos.Campo_Porcentaje_Impuesto +
                         " FROM " + Cat_Com_Impuestos.Tabla_Cat_Impuestos +
                         " WHERE PRODUCTOS." + Cat_Com_Productos.Campo_Impuesto_2_ID + "=" +
                         Cat_Com_Impuestos.Campo_Impuesto_ID + ") AS IMPUESTO_PORCENTAJE_2 " +
                         " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS" +
                         " JOIN " + Cat_Com_Impuestos.Tabla_Cat_Impuestos + " IMPUESTOS" +
                         " ON IMPUESTOS." + Cat_Com_Impuestos.Campo_Impuesto_ID + "= PRODUCTOS." + Cat_Com_Productos.Campo_Impuesto_ID +
                         " WHERE PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID +
                         " ='" + Clase_Negocio.P_Producto_ID + "'";
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }

    }
}