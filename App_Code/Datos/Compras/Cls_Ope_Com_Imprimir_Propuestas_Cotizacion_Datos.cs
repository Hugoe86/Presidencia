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
using Presidencia.Imprimir_Propuestas.Negocio;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Compras.Impresion_Requisiciones.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;


/// <summary>
/// Summary description for Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Datos
/// </summary>

namespace Presidencia.Imprimir_Propuestas.Datos
{
    public class Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Datos
    {

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
        public static DataTable Consultar_Propuesta_Cotizacion(Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio Clase_Negocio)
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



        }//fin de la consulta

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
        public static DataTable Consultar_Productos_Servicios(Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";
            switch (Clase_Negocio.P_Tipo_Articulo)
            {
                case "PRODUCTO":
                    Mi_SQL = "SELECT PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Prod_Serv_ID;
                    Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Ope_Com_Req_Producto_ID;
                    Mi_SQL = Mi_SQL + ", PRO." + Cat_Com_Productos.Campo_Clave;
                    Mi_SQL = Mi_SQL + "||' '|| PRO." + Cat_Com_Productos.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + ",(SELECT " + Cat_Com_Unidades.Campo_Nombre + " FROM ";
                    Mi_SQL = Mi_SQL + Cat_Com_Unidades.Tabla_Cat_Com_Unidades;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Unidades.Campo_Unidad_ID;
                    Mi_SQL = Mi_SQL + "=PRO." + Cat_Com_Productos.Campo_Unidad_ID +") AS UNIDAD";
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
                    Mi_SQL = Mi_SQL + ", NULL AS UNIDAD";
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
        public static DataTable Consultar_Requisiciones(Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio Clase_Negocio)
        {
         

            String Mi_SQL = "SELECT REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Folio;
            Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Tipo_Articulo;
            Mi_SQL = Mi_SQL + ", TO_CHAR(PROPUESTA." + Ope_Com_Requisiciones.Campo_Fecha_Creo;
            Mi_SQL = Mi_SQL + ",'DD/MM/YYYY') AS FECHA";
            Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Com_Proveedores.Campo_Nombre;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Proveedores.Campo_Proveedor_ID + "= PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Proveedor_ID + ") AS NOMBRE_PROVEEDOR";
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Nombre_Cotizador;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Proveedor_ID;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Estatus;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQ";
            Mi_SQL = Mi_SQL + " JOIN " + Ope_Com_Propuesta_Cotizacion.Tabla_Ope_Com_Propuesta_Cotizacion + " PROPUESTA";
            Mi_SQL = Mi_SQL + " ON PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_No_Requisicion + "=";
            Mi_SQL = Mi_SQL + " REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + " WHERE REQ." + Ope_Com_Requisiciones.Campo_Cotizador_ID + "='" + Cls_Sessiones.Empleado_ID.Trim()+ "'";
            

            if (Clase_Negocio.P_Requisicion_Busqueda != null)
            {
                Mi_SQL = Mi_SQL + " AND UPPER(PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_No_Requisicion + ") LIKE UPPER('%" + Clase_Negocio.P_Requisicion_Busqueda.Trim() + "%')";
            }

            if (Clase_Negocio.P_Busqueda_Fecha_Generacion_Ini != null)
            {
                Mi_SQL = Mi_SQL + " AND PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Fecha_Elaboracion;
                Mi_SQL = Mi_SQL + " BETWEEN TO_DATE ('" + Clase_Negocio.P_Busqueda_Fecha_Generacion_Ini + " 00:00:00', 'DD-MMM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Clase_Negocio.P_Busqueda_Fecha_Generacion_Fin + " 23:59:00', 'DD-MMM-YYYY HH24:MI:SS')";
            }

            if (Clase_Negocio.P_Busqueda_Fecha_Entrega_Ini != null)
            {
                Mi_SQL = Mi_SQL + " AND PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Tiempo_Entrega;
                Mi_SQL = Mi_SQL + " BETWEEN TO_DATE ('" + Clase_Negocio.P_Busqueda_Fecha_Entrega_Ini + " 00:00:00', 'DD-MMM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Clase_Negocio.P_Busqueda_Fecha_Entrega_Fin + " 23:59:00', 'DD-MMM-YYYY HH24:MI:SS')";
            }
            if (Clase_Negocio.P_Busqueda_Vigencia_Propuesta_Ini != null)
            {
                Mi_SQL = Mi_SQL + " AND PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Tiempo_Entrega;
                Mi_SQL = Mi_SQL + " BETWEEN TO_DATE ('" + Clase_Negocio.P_Busqueda_Vigencia_Propuesta_Ini + " 00:00:00', 'DD-MMM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Clase_Negocio.P_Busqueda_Vigencia_Propuesta_Fin + " 23:59:00', 'DD-MMM-YYYY HH24:MI:SS')";
            }

            if (Clase_Negocio.P_Busqueda_Proveedor != null)
            {
                Mi_SQL = Mi_SQL + " AND UPPER(PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Proveedor_ID + ")";
                Mi_SQL = Mi_SQL + " LIKE UPPER('%" + Clase_Negocio.P_Busqueda_Proveedor + "%')";
            }

           
            Mi_SQL = Mi_SQL + " GROUP BY ( REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Folio;
            Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Tipo_Articulo;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Requisiciones.Campo_Fecha_Creo;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Nombre_Cotizador;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Proveedor_ID;
            Mi_SQL = Mi_SQL + ", PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Estatus + ")";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        public static DataTable Consultar_Proveedores(Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio Clase_Negocio)
        {
            String Mi_SQL = "SELECT " + Cat_Com_Proveedores.Campo_Proveedor_ID;
            Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Nombre + " ||' '|| TO_NUMBER(" + Cat_Com_Proveedores.Campo_Proveedor_ID + ")";
            Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Proveedores.Campo_Estatus + "='ACTIVO'";
            Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Proveedores.Campo_Nombre + " ASC";

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
        public static DataTable Consultar_Detalle_Requisicion(Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio Clase_Negocio)
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
        public static DataTable Consultar_Impuesto_Producto(Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio Clase_Negocio)
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

        public static DataTable Consultar_Datos_Proveedor(Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";
            Mi_SQL = "SELECT " + Cat_Com_Proveedores.Campo_Proveedor_ID;
            Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Nombre;
            Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Compañia;
            Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_RFC;
            Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Contacto;
            Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Direccion;
            Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Colonia;
            Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Ciudad;
            Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Estado;
            Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_CP;
            Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Nextel;
            Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Fax;
            Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Correo_Electronico;
            Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Representante_Legal;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores ;
            Mi_SQL = Mi_SQL +  " WHERE " + Cat_Com_Proveedores.Campo_Proveedor_ID;
            Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Proveedor_ID.Trim() + "'";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        public static DataTable Consultar_Cotizacion(Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";
            Mi_SQL = "SELECT " + Ope_Com_Requisiciones.Campo_Tipo_Articulo + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + Clase_Negocio.P_No_Requisicion;
            Object Objeto = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            String Tipo_Articulo = Objeto.ToString().Trim();
            Mi_SQL = "";

            Mi_SQL = "" +
            "select " +
            "detalle.clave,detalle.nombre_producto_servicio,detalle.cantidad,detalle.tipo, " +
            "cotizacion.descripcion_producto,cotizacion.marca, " +
            "cotizacion.precio_u_sin_imp_cotizado,cotizacion.subtotal_cotizado, " +
            "cotizacion.subtotal_cotizado_requisicion, " +
            "cotizacion.iva_cotizado_req,cotizacion.ieps_cotizado_req,cotizacion.total_cotizado_requisicion, " +
            "cotizacion.fecha_elaboracion,cotizacion.garantia,cotizacion.vigencia_propuesta,cotizacion.nombre_cotizador, " +
            "cotizacion.estatus,cotizacion.tiempo_entrega,'RQ-'||detalle.no_requisicion no_requisicion,cotizacion.resultado,cotizacion.proveedor_id, ";
            if (Tipo_Articulo == "PRODUCTO")
            {
                Mi_SQL += "(select nombre from cat_com_unidades where unidad_id = (select unidad_id from cat_com_productos where producto_id = detalle.prod_serv_id)) unidad, ";
            }
            else if (Tipo_Articulo == "SERVICIO")
            {
                Mi_SQL += "'SERVICIO' AS unidad, ";
            }
            Mi_SQL += "prov.nombre ||' - '||prov.compania proveedor,prov.e_mail correo, prov.telefono1 ||', '||prov.telefono2 as telefono, cotizacion.elaboro_propuesta as firma " +
            "from ope_com_req_producto detalle " +
            "join ope_com_propuesta_cotizacion cotizacion " +
            "on detalle.ope_com_req_producto_id = cotizacion.ope_com_req_producto_id " +
            "join cat_com_proveedores prov " +
            "on cotizacion.proveedor_id = prov.proveedor_id " +
            "where cotizacion.proveedor_id = '" + Clase_Negocio.P_Proveedor_ID + 
            "' and cotizacion.no_requisicion = " + Clase_Negocio.P_No_Requisicion;
            Mi_SQL = Mi_SQL + " ORDER BY detalle." + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio;
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

        }

        public static String Imprimir_Cotizacion(Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio Clase_Negocio)
        {                       
            DataTable Datos_Cotizacion = Clase_Negocio.Consultar_Cotizacion();
            DataSet Ds_Reporte = new DataSet();
            String Str_Reporte = "";
            Datos_Cotizacion.TableName = "COTIZACION";
            Ds_Reporte.Tables.Add(Datos_Cotizacion.Copy());
            Str_Reporte = Generar_Reporte(ref Ds_Reporte, Clase_Negocio.P_Archivo_PDF, Clase_Negocio.P_Ruta_RPT,Clase_Negocio.P_Ruta_Exportacion);
            return Str_Reporte;
        }
        protected static String Generar_Reporte(ref DataSet Ds_Datos, String Nombre_Reporte_Generar, String Ruta_RPT, String Ruta_Exportacion)
        {
            ReportDocument Reporte = new ReportDocument();//Variable de tipo reporte.
            String Ruta = String.Empty;//Variable que almacenara la ruta del archivo del crystal report. 
            String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
            try
            {
                Ruta = Ruta_RPT;//@Server.MapPath("../Rpt/Compras/" + Nombre_Plantilla_Reporte);
                Reporte.Load(Ruta);

                if (Ds_Datos is DataSet)
                {
                    if (Ds_Datos.Tables.Count > 0)
                    {
                        Reporte.SetDataSource(Ds_Datos);
                        Exportar_Reporte_PDF(Reporte,Ruta_Exportacion);                       
                        Pagina += Nombre_Reporte_Generar;
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al generar el reporte. Error: [" + Ex.Message + "]");
            }
            return Pagina;
        }

        protected static void  Exportar_Reporte_PDF(ReportDocument Reporte, String Ruta_Exportacion)
        {
            ExportOptions Opciones_Exportacion = new ExportOptions();
            DiskFileDestinationOptions Direccion_Guardar_Disco = new DiskFileDestinationOptions();
            PdfRtfWordFormatOptions Opciones_Formato_PDF = new PdfRtfWordFormatOptions();

            try
            {
                if (Reporte is ReportDocument)
                {
                    Direccion_Guardar_Disco.DiskFileName = Ruta_Exportacion;//@Server.MapPath("../../Reporte/" + Nombre_Reporte);
                    Opciones_Exportacion.ExportDestinationOptions = Direccion_Guardar_Disco;
                    Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                    Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                    Reporte.Export(Opciones_Exportacion);
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al exportar el reporte. Error: [" + Ex.Message + "]");
            }
        }



    }
}//fin del names