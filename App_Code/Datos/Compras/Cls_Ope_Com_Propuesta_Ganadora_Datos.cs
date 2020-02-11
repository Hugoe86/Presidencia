﻿using System;
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
using Presidencia.Propuesta_Ganadora.Negocios;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;

/// <summary>
/// Summary description for Cls_Ope_Com_Propuesta_Ganadora_Datos
/// </summary>

namespace Presidencia.Propuesta_Ganadora.Datos
{
    public class Cls_Ope_Com_Propuesta_Ganadora_Datos
    {
        #region Metodos
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
        public static DataTable Consultar_Productos_Servicios(Cls_Ope_Com_Propuesta_Ganadora_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";
            switch (Clase_Negocio.P_Tipo_Articulo)
            {
                case "PRODUCTO":
                    Mi_SQL = "SELECT DET." + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
                             ", PRO." + Cat_Com_Productos.Campo_Producto_ID + " AS PROD_SERV_ID" +
                            ", PRO." + Cat_Com_Productos.Campo_Clave +
                            "||' '|| PRO." + Cat_Com_Productos.Campo_Nombre + " AS NOMBRE_PROD_SERV" +
                            ", PRO." + Cat_Com_Productos.Campo_Descripcion +
                            ", DET." + Ope_Com_Req_Producto.Campo_Cantidad +
                            ", DET." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_IVA_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_IEPS_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_Subtota_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_Proveedor_ID +
                            ", DET." + Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
                            ", DET." + "NOMBRE_PROD_SERV_OC" +
                            ", DET." + "MARCA_OC" +
                            " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " DET" +
                            " JOIN " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRO" +
                            " ON PRO." + Cat_Com_Productos.Campo_Producto_ID +
                            " = DET." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +
                            " JOIN " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQ" +
                            " ON REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                            " = DET." + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                            " WHERE REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                            " = '" + Clase_Negocio.P_No_Requisicion + "'";
                    break;
                case "SERVICIO":
                    Mi_SQL = "SELECT DET." + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
                            ",SER." + Cat_Com_Servicios.Campo_Servicio_ID + " AS PROD_SERV_ID" +
                            ", SER." + Cat_Com_Servicios.Campo_Clave +
                            "||' '||  SER." + Cat_Com_Servicios.Campo_Nombre + " AS NOMBRE_PROD_SERV" +
                            ", NULL AS DESCRIPCION" +
                            ", DET." + Ope_Com_Req_Producto.Campo_Cantidad +
                            ", DET." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_IVA_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_IEPS_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_Subtota_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_Proveedor_ID +
                            ", DET." + Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
                            ", DET." + "NOMBRE_PROD_SERV_OC" +
                            ", DET." + "MARCA_OC" +
                            " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " DET" +
                            " JOIN " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + " SER" +
                            " ON SER." + Cat_Com_Servicios.Campo_Servicio_ID +
                            " = DET." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +
                            " JOIN " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQ" +
                            " ON REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                            " = DET." + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                            " WHERE REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                            " = '" + Clase_Negocio.P_No_Requisicion + "'";

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
        public static DataTable Consultar_Requisiciones()
        {
            String Mi_SQL = "SELECT REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Folio;
            Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Tipo_Articulo;
            Mi_SQL = Mi_SQL + ", TO_CHAR(REQ." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion;
            Mi_SQL = Mi_SQL + ",'DD/MON/YYYY') AS FECHA";
            Mi_SQL = Mi_SQL + ",(SELECT " + Cat_Sap_Concepto.Campo_Clave + " ||' '|| " + Cat_Sap_Concepto.Campo_Descripcion + " FROM ";
            Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + " WHERE " + Cat_Sap_Concepto.Campo_Concepto_ID + "=(SELECT ";
            Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Genericas.Campo_Concepto_ID + " FROM ";
            Mi_SQL = Mi_SQL + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + " WHERE ";
            Mi_SQL = Mi_SQL + "CAT_SAP_PARTIDA_GENERICA." + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID;
            Mi_SQL = Mi_SQL + "=(SELECT " + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID + " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " = REQ." + Ope_Com_Requisiciones.Campo_Partida_ID + "))) AS CONCEPTO";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQ";
            Mi_SQL = Mi_SQL + " WHERE REQ." + Ope_Com_Requisiciones.Campo_Estatus + "='PROVEEDOR'";
            Mi_SQL = Mi_SQL + " AND REQ." + Ope_Com_Requisiciones.Campo_Cotizador_ID + "='" + Cls_Sessiones.Empleado_ID + "'";



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
        public static DataTable Consultar_Detalle_Requisicion(Cls_Ope_Com_Propuesta_Ganadora_Negocio Clase_Negocio)
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
            Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_Total_Cotizado;
            Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_Subtotal_Cotizado;
            Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_IVA_Cotizado;
            Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_IEPS_Cotizado;
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
            Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_Especial_Ramo_33;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICION ";
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIA ";
            Mi_SQL = Mi_SQL + " ON REQUISICION." + Ope_Com_Requisiciones.Campo_Dependencia_ID + "= DEPENDENCIA.";
            Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + " WHERE REQUISICION." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_No_Requisicion.Trim() + "'";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }


        public static DataTable Consultar_Proveedores(Cls_Ope_Com_Propuesta_Ganadora_Negocio Clase_Negocio)
        {
            String Mi_SQL = "SELECT PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Proveedor_ID;
            Mi_SQL = Mi_SQL + ",(SELECT " + Cat_Com_Proveedores.Campo_Nombre;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Proveedores.Campo_Proveedor_ID+ "=";
            Mi_SQL = Mi_SQL + " PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Proveedor_ID + ")";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Propuesta_Cotizacion.Tabla_Ope_Com_Propuesta_Cotizacion + " PROPUESTA ";
            Mi_SQL = Mi_SQL + " WHERE PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_No_Requisicion + "='";
            Mi_SQL = Mi_SQL + Clase_Negocio.P_No_Requisicion + "'";
            Mi_SQL = Mi_SQL + " GROUP BY (PROPUESTA." + Ope_Com_Propuesta_Cotizacion.Campo_Proveedor_ID + ")";
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
        public static DataTable Consultar_Impuesto_Producto(Cls_Ope_Com_Propuesta_Ganadora_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";
            switch (Clase_Negocio.P_Tipo_Articulo)
            {
                case "PRODUCTO":
                    Mi_SQL = "SELECT PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID +
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
                    break;
                case "SERVICIO":
                    Mi_SQL = "SELECT SER." + Cat_Com_Servicios.Campo_Servicio_ID +
                         ", SER." + Cat_Com_Servicios.Campo_Clave +
                         ", SER." + Cat_Com_Servicios.Campo_Nombre + " AS PRODUCTO_NOMBRE" +
                         ", SER." + Cat_Com_Servicios.Campo_Costo + " AS PRECIO_UNITARIO" +
                         ", SER." + Cat_Com_Servicios.Campo_Impuesto_ID +
                         ",(SELECT " + Cat_Com_Impuestos.Campo_Nombre +
                         " FROM " + Cat_Com_Impuestos.Tabla_Cat_Impuestos +
                         " WHERE SER." + Cat_Com_Servicios.Campo_Impuesto_ID + "=" +
                         Cat_Com_Impuestos.Campo_Impuesto_ID + ") AS TIPO_IMPUESTO " +
                         ",(SELECT " + Cat_Com_Impuestos.Campo_Porcentaje_Impuesto +
                         " FROM " + Cat_Com_Impuestos.Tabla_Cat_Impuestos +
                         " WHERE SER." + Cat_Com_Servicios.Campo_Impuesto_ID + "=" +
                         Cat_Com_Impuestos.Campo_Impuesto_ID + ") AS IMPUESTO_PORCENTAJE " +
                         " FROM " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + " SER" +
                         " JOIN " + Cat_Com_Impuestos.Tabla_Cat_Impuestos + " IMPUESTOS" +
                         " ON IMPUESTOS." + Cat_Com_Impuestos.Campo_Impuesto_ID + "= SER." + Cat_Com_Servicios.Campo_Impuesto_ID +
                         " WHERE SER." + Cat_Com_Servicios.Campo_Servicio_ID +
                         " ='" + Clase_Negocio.P_Producto_ID + "'";


                    break;

            }
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
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
        public static DataTable Consultar_Productos_Propuesta(Cls_Ope_Com_Propuesta_Ganadora_Negocio Clase_Negocio)
        {
            String Mi_SQL = "SELECT * FROM " + Ope_Com_Propuesta_Cotizacion.Tabla_Ope_Com_Propuesta_Cotizacion;
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Propuesta_Cotizacion.Campo_No_Requisicion;
            Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_No_Requisicion.Trim() + "'";
            Mi_SQL = Mi_SQL + " AND " + Ope_Com_Propuesta_Cotizacion.Campo_Proveedor_ID;
            Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Proveedor_ID.Trim() + "'";
            Mi_SQL = Mi_SQL + " AND " + Ope_Com_Propuesta_Cotizacion.Campo_Prod_Serv_ID;
            Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Producto_ID.Trim()+"'";




            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];



        }

        public static bool Agregar_Cotizaciones(Cls_Ope_Com_Propuesta_Ganadora_Negocio Clase_Negocio)
        {
            bool Operacion_Realizada = false;
            String Mi_SQL = "";
            try
            {
                //RECORREMOS LOS DETALLES DE LA LICITACION,   
                for (int i = 0; i < Clase_Negocio.P_Dt_Productos.Rows.Count; i++)
                {

                    Mi_SQL = "UPDATE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                             " SET " + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                             "='" + Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado].ToString().Trim() + "'" +
                             ", " + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
                             "='" + Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado].ToString().Trim() + "'" +
                             ", " + Ope_Com_Req_Producto.Campo_IVA_Cotizado +
                             "='" + Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_IVA_Cotizado].ToString().Trim() + "'" +
                             ", " + Ope_Com_Req_Producto.Campo_IEPS_Cotizado +
                             "='" + Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_IEPS_Cotizado].ToString().Trim() + "'" +
                             ", " + Ope_Com_Req_Producto.Campo_Subtota_Cotizado +
                             "='" + Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Subtota_Cotizado].ToString().Trim() + "'" +
                             ", " + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                             "='" + Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Total_Cotizado].ToString().Trim() + "'" +
                             ", " + Ope_Com_Req_Producto.Campo_Proveedor_ID +
                             "='" + Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Proveedor_ID].ToString().Trim() + "'" +
                             ", " + Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
                             "='" + Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Nombre_Proveedor].ToString().Trim() + "'" +

                             ", " + "NOMBRE_PROD_SERV_OC " +
                             "='" + Clase_Negocio.P_Dt_Productos.Rows[i]["NOMBRE_PROD_SERV_OC"].ToString().Trim() + "'" +
                             ", " + "MARCA_OC " +
                             "='" + Clase_Negocio.P_Dt_Productos.Rows[i]["MARCA_OC"].ToString().Trim() + "'" +                             
                             
                             " WHERE " + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
                             "='" + Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID].ToString().Trim() + "'";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    //Modificamos el detalle de la propuesta seleccionada

                    Mi_SQL = "UPDATE " + Ope_Com_Propuesta_Cotizacion.Tabla_Ope_Com_Propuesta_Cotizacion;
                    Mi_SQL = Mi_SQL + " SET " + Ope_Com_Propuesta_Cotizacion.Campo_Resultado;
                    Mi_SQL = Mi_SQL + " ='ACEPTADA'";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Propuesta_Cotizacion.Campo_Proveedor_ID;
                    Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Proveedor_ID].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Propuesta_Cotizacion.Campo_No_Requisicion;
                    Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_No_Requisicion.Trim() + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Propuesta_Cotizacion.Campo_Ope_Com_Req_Producto_ID;
                    Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID].ToString().Trim() + "'";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);


 

                }//fIN FOR I
                Operacion_Realizada = true;
            }
            catch
            {
                Operacion_Realizada = false;
            }

            return Operacion_Realizada;
        }

        public static bool Modificar_Requisicion(Cls_Ope_Com_Propuesta_Ganadora_Negocio Clase_Negocio)
        {
            bool Operacion_Realizada = false;
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
                Mi_SQL = Mi_SQL + " SET " + Ope_Com_Requisiciones.Campo_Estatus + "='" + Clase_Negocio.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Campo_IVA_Cotizado + "='" + Clase_Negocio.P_IVA_Cotizado + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Campo_IEPS_Cotizado + "='" + Clase_Negocio.P_IEPS_Cotizado + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Campo_Subtotal_Cotizado + "='" + Clase_Negocio.P_Subtotal_Cotizado + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Campo_Total_Cotizado + "='" + Clase_Negocio.P_Total_Cotizado + "'";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + "='" + Clase_Negocio.P_No_Requisicion + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Operacion_Realizada = true;
                Operacion_Realizada = Cls_Util.Registrar_Historial(Clase_Negocio.P_Estatus, Clase_Negocio.P_No_Requisicion);
            }
            catch
            {
                Operacion_Realizada = false;
            }


            return Operacion_Realizada;
        }

        #endregion

    }
}
