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
using Presidencia.Distribuir_a_Proveedores.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Sessiones;
using System.Data.OracleClient;



/// <summary>
/// Summary description for Cls_Ope_Com_Distribuir_Requisiciones_Prov_Datos
/// </summary>

namespace Presidencia.Distribuir_a_Proveedores.Datos
{
    public class Cls_Ope_Com_Distribuir_Requisiciones_Prov_Datos
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
        public static DataTable Consultar_Productos_Servicios(Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";
            switch (Clase_Negocio.P_Tipo_Articulo)
            {
                case "PRODUCTO":
                    Mi_SQL = "SELECT PRO." + Cat_Com_Productos.Campo_Clave;
                    Mi_SQL = Mi_SQL + ", PRO." + Cat_Com_Productos.Campo_Nombre;
                    Mi_SQL = Mi_SQL + ", PRO." + Cat_Com_Productos.Campo_Descripcion;
                    Mi_SQL = Mi_SQL + ", REQ_PRO." + Ope_Com_Req_Producto.Campo_Cantidad;
                    Mi_SQL = Mi_SQL + ", REQ_PRO." + Ope_Com_Req_Producto.Campo_Monto_Total;
                    Mi_SQL = Mi_SQL + ", REQ_PRO." + Ope_Com_Req_Producto.Campo_Importe;
                    Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Com_Unidades.Campo_Nombre;
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Unidades.Campo_Unidad_ID;
                    Mi_SQL = Mi_SQL + "= PRO." + Cat_Com_Productos.Campo_Unidad_ID + ") AS UNIDAD";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQ";
                    Mi_SQL = Mi_SQL + " INNER JOIN " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRO ";
                    Mi_SQL = Mi_SQL + " ON REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "=";
                    Mi_SQL = Mi_SQL + " REQ_PRO." + Ope_Com_Req_Producto.Campo_Requisicion_ID;
                    Mi_SQL = Mi_SQL + " INNER JOIN " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRO";
                    Mi_SQL = Mi_SQL + " ON PRO." + Cat_Com_Productos.Campo_Producto_ID + "=";
                    Mi_SQL = Mi_SQL + " REQ_PRO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID;
                    Mi_SQL = Mi_SQL + " WHERE REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "='" + Clase_Negocio.P_No_Requisicion.Trim() + "'";
                    Mi_SQL = Mi_SQL + " ORDER BY REQ_PRO." + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio;
                    break;
                case "SERVICIO":
                    Mi_SQL = "SELECT SER." + Cat_Com_Servicios.Campo_Clave;
                    Mi_SQL = Mi_SQL + ", SER." + Cat_Com_Servicios.Campo_Nombre;
                    Mi_SQL = Mi_SQL + ", SER." + Cat_Com_Servicios.Campo_Comentarios + " AS DESCRIPCION";
                    Mi_SQL = Mi_SQL + ", REQ_PRO." + Ope_Com_Req_Producto.Campo_Cantidad;
                    Mi_SQL = Mi_SQL + ", REQ_PRO." + Ope_Com_Req_Producto.Campo_Monto_Total;
                    Mi_SQL = Mi_SQL + ", REQ_PRO." + Ope_Com_Req_Producto.Campo_Importe;
                    Mi_SQL = Mi_SQL + ", NULL AS UNIDAD";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQ";
                    Mi_SQL = Mi_SQL + " INNER JOIN " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRO ";
                    Mi_SQL = Mi_SQL + " ON REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "=";
                    Mi_SQL = Mi_SQL + " REQ_PRO." + Ope_Com_Req_Producto.Campo_Requisicion_ID;
                    Mi_SQL = Mi_SQL + " INNER JOIN " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + " SER";
                    Mi_SQL = Mi_SQL + " ON SER." + Cat_Com_Servicios.Campo_Servicio_ID + "=";
                    Mi_SQL = Mi_SQL + " REQ_PRO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID;
                    Mi_SQL = Mi_SQL + " WHERE REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "='" + Clase_Negocio.P_No_Requisicion.Trim() + "'";
                    Mi_SQL = Mi_SQL + " ORDER BY " + " REQ_PRO." + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio;
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
        public static DataTable Consultar_Requisiciones(Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio Clase_Negocio)
        {
            String Mi_SQL = "SELECT REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Folio;
            Mi_SQL = Mi_SQL + ", DEP." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA";
            Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Total;
            Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Tipo_Articulo;
            Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Estatus; 
            Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Leido_Por_Cotizador + " AS LEIDO";
            Mi_SQL = Mi_SQL + ",(SELECT " + Cat_Sap_Concepto.Campo_Clave + " ||' '|| " + Cat_Sap_Concepto.Campo_Descripcion + " FROM ";
            Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + " WHERE " + Cat_Sap_Concepto.Campo_Concepto_ID + "=(SELECT ";
            Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Genericas.Campo_Concepto_ID + " FROM ";
            Mi_SQL = Mi_SQL + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + " WHERE ";
            Mi_SQL = Mi_SQL + "CAT_SAP_PARTIDA_GENERICA." + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID;
            Mi_SQL = Mi_SQL + "=(SELECT " + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID + " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " = REQ." + Ope_Com_Requisiciones.Campo_Partida_ID + "))) AS CONCEPTO";
            Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Alerta;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQ";
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEP";
            Mi_SQL = Mi_SQL + " ON DEP." + Cat_Dependencias.Campo_Dependencia_ID + "=";
            Mi_SQL = Mi_SQL + " REQ." + Ope_Com_Requisiciones.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + " WHERE REQ." + Ope_Com_Requisiciones.Campo_Tipo + "='TRANSITORIA'";
            //Mi_SQL = Mi_SQL + " AND REQ." + Ope_Com_Requisiciones.Campo_Cotizador_ID + "='" + Cls_Sessiones.Empleado_ID +"'";
            if (Clase_Negocio.P_Cotizador_ID != null && Clase_Negocio.P_Cotizador_ID != "0")
            {
                Mi_SQL = Mi_SQL + " AND REQ." + Ope_Com_Requisiciones.Campo_Cotizador_ID + "='" + Clase_Negocio.P_Cotizador_ID + "'";
            }            
            Mi_SQL = Mi_SQL + " AND REQ." + Ope_Com_Requisiciones.Campo_Estatus + " IN ('FILTRADA','COTIZADA-RECHAZADA')";
            Mi_SQL = Mi_SQL + " ORDER BY REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
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
        public static DataTable Consultar_Detalle_Requisicion(Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio Clase_Negocio)
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
            Mi_SQL = Mi_SQL + ", TO_CHAR( REQUISICION." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion + ",'DD/MON/YYYY') AS FECHA_GENERACION";
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
            Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_Codigo_Programatico;

            Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_Especial_Ramo_33;

            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICION ";
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIA ";
            Mi_SQL = Mi_SQL + " ON REQUISICION." + Ope_Com_Requisiciones.Campo_Dependencia_ID + "= DEPENDENCIA.";
            Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + " WHERE REQUISICION." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_No_Requisicion.Trim() + "'";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        public static DataTable Consultar_Proveedores_Asignados(Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";

            if (Clase_Negocio.P_Proveedor_ID != null)
            {
                Mi_SQL = "SELECT " + Cat_Com_Proveedores.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Nombre;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Compañia;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Telefono_1 + "||' ,'||";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Telefono_2 + " AS TELEFONOS";
                Mi_SQL = Mi_SQL +", " + Cat_Com_Proveedores.Campo_Correo_Electronico;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Proveedores.Campo_Proveedor_ID + "='" + Clase_Negocio.P_Proveedor_ID.Trim() + "'"; 
            }
            else
            {
                Mi_SQL = "SELECT " + Cat_Com_Proveedores.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Nombre;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Compañia;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Telefono_1 + "||' ,'||";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Telefono_2 + " AS TELEFONOS";
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Correo_Electronico;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Proveedores.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Com_Propuesta_Cotizacion.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Propuesta_Cotizacion.Tabla_Ope_Com_Propuesta_Cotizacion;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Propuesta_Cotizacion.Campo_No_Requisicion;
                Mi_SQL = Mi_SQL + " ='" + Clase_Negocio.P_No_Requisicion.Trim();
                Mi_SQL = Mi_SQL + "' GROUP BY "+Ope_Com_Propuesta_Cotizacion.Campo_Proveedor_ID +")";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Proveedores.Campo_Nombre;
            }

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        public static DataTable Consultar_Proveedores(Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio Clase_Negocio)
        {
            String Mi_SQL = "SELECT " + Cat_Com_Proveedores.Campo_Proveedor_ID;
            Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Nombre + " ||' '|| TO_NUMBER(" + Cat_Com_Proveedores.Campo_Proveedor_ID +")";
            Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Proveedores.Campo_Estatus + "='ACTIVO'";
            Mi_SQL = Mi_SQL + " AND " + Cat_Com_Proveedores.Campo_Proveedor_ID + " IN (SELECT ";
            Mi_SQL = Mi_SQL + Cat_Com_Giro_Proveedor.Campo_Proveedor_ID + " FROM ";
            Mi_SQL = Mi_SQL + Cat_Com_Giro_Proveedor.Tabla_Cat_Com_Giro_Proveedor;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Giro_Proveedor.Campo_Giro_ID + "='" + Clase_Negocio.P_Concepto_ID.Trim() +"')";
            Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Proveedores.Campo_Nombre + " ASC";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

        }

        public static void Eliminar_Proveedores(Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";

            Mi_SQL = "DELETE " + Ope_Com_Propuesta_Cotizacion.Tabla_Ope_Com_Propuesta_Cotizacion;
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Propuesta_Cotizacion.Campo_No_Requisicion;
            Mi_SQL = Mi_SQL + " ='" + Clase_Negocio.P_No_Requisicion.Trim() + "'";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

        }

        public static bool Alta_Proveedores_Asignados(Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";
            bool Operacion_Realizada = false;
            //Consultamos los Productos de esta Requisicion pára darlos de alta en la Propuesta de Cotizacion
            Mi_SQL = "SELECT * FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " WHERE ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Campo_Requisicion_ID +"='" + Clase_Negocio.P_No_Requisicion+"'";
            DataTable Dt_Productos_Requisicion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            try
            {

                if (Clase_Negocio.P_Dt_Proveedores != null && Dt_Productos_Requisicion.Rows.Count != 0)
                    for (int i = 0; i < Clase_Negocio.P_Dt_Proveedores.Rows.Count; i++)//For k buscara a los proveedores seleccionados 
                    {
                        for (int y = 0; y < Dt_Productos_Requisicion.Rows.Count; y++)
                        {
                            Mi_SQL = "INSERT INTO " + Ope_Com_Propuesta_Cotizacion.Tabla_Ope_Com_Propuesta_Cotizacion;
                            Mi_SQL = Mi_SQL + "(" + Ope_Com_Propuesta_Cotizacion.Campo_No_Propuesta_Cotizacion;
                            Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Ope_Com_Req_Producto_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_No_Requisicion;
                            Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Cantidad;
                            Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Prod_Serv_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Tipo;
                            Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Nombre_Producto_Servicion;
                            Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Proveedor_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Nombre_Cotizador;
                            Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Fecha_Creo;
                            Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Usuario_Creo;
                            Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_IVA_Cotizado;
                            Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_IEPS_Cotizado;
                            Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Precio_U_Con_Imp_Cotizado;
                            Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Precio_U_Sin_Imp_Cotizado;
                            Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Subtota_Cotizado;
                            Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Total_Cotizado;
                            Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Estatus;
                            Mi_SQL = Mi_SQL + ") VALUES(" + Obtener_Consecutivo(Ope_Com_Propuesta_Cotizacion.Campo_No_Propuesta_Cotizacion, Ope_Com_Propuesta_Cotizacion.Tabla_Ope_Com_Propuesta_Cotizacion);
                            Mi_SQL = Mi_SQL + ",'" + Dt_Productos_Requisicion.Rows[y][Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID].ToString().Trim();
                            Mi_SQL = Mi_SQL + "','" + Clase_Negocio.P_No_Requisicion;
                            Mi_SQL = Mi_SQL + "','" + Dt_Productos_Requisicion.Rows[y][Ope_Com_Req_Producto.Campo_Cantidad].ToString().Trim();
                            Mi_SQL = Mi_SQL + "','" + Dt_Productos_Requisicion.Rows[y][Ope_Com_Req_Producto.Campo_Prod_Serv_ID].ToString().Trim();
                            Mi_SQL = Mi_SQL + "','" + Dt_Productos_Requisicion.Rows[y][Ope_Com_Req_Producto.Campo_Tipo].ToString().Trim();
                            Mi_SQL = Mi_SQL + "','" + Dt_Productos_Requisicion.Rows[y][Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio].ToString().Trim();
                            Mi_SQL = Mi_SQL + "','" + Clase_Negocio.P_Dt_Proveedores.Rows[i]["Proveedor_ID"];
                            Mi_SQL = Mi_SQL + "','" + Cls_Sessiones.Nombre_Empleado;
                            Mi_SQL = Mi_SQL + "',SYSDATE,'" + Cls_Sessiones.Nombre_Empleado + "',0,0,0,0,0,0,'EN CONSTRUCCION')";

                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        }//fin del for y
                    }//fin del for i
                //Actualizamos la requisicion 

                Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
                Mi_SQL = Mi_SQL + " SET " + Ope_Com_Requisiciones.Campo_Estatus+"='PROVEEDOR'";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + "='" + Clase_Negocio.P_No_Requisicion + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                //REalizamos el insert del historial
                Cls_Util.Registrar_Historial("PROVEEDOR", Clase_Negocio.P_No_Requisicion);

                Operacion_Realizada = true;
            }//Fin del try 
            catch
            {
                Operacion_Realizada = false;
            }
            return Operacion_Realizada;
        }

        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Consecutivo
        ///DESCRIPCIÓN: Obtiene el numero consecutivo para las tablas ocupadas en esta clase
        ///PARAMETROS: 1.-Campo del cual se obtendra el consecutivo
        ///            2.-P_Nombre de la tabla
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Obtener_Consecutivo(String Campo_ID, String Tabla)
        {
            int Consecutivo = 0;
            String Mi_Sql;
            Object Obj; //Obtiene el ID con la cual se guardo los datos en la base de datos
            Mi_Sql = "SELECT NVL(MAX (" + Campo_ID + "),'00000') FROM " + Tabla;
            Obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            Consecutivo = (Convert.ToInt32(Obj) + 1);
            return Consecutivo;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA METODO: Modificar_Peticion
        ///        DESCRIPCIÓN: Modifica la peticion
        ///         PARAMETROS: 1.-
        ///                     2.-
        ///               CREO: 
        ///         FECHA_CREO: 
        ///           MODIFICO:
        ///     FECHA_MODIFICO:
        /// CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Parametros()
        {
            DataTable Dt_Temporal = null;
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT " +
                         Apl_Parametros.Campo_Servidor_Correo + ", " +
                         Apl_Parametros.Campo_Correo_Saliente + ", " +
                         Apl_Parametros.Campo_Password_Correo +

                    " FROM " + Apl_Parametros.Tabla_Apl_Parametros;

                Dt_Temporal = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                return Dt_Temporal;
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            

        }

        public static DataTable Consultar_Email_Proveedores(Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio Clase_Negocio)
        {
            String Mi_SQL = "SELECT " + Cat_Com_Proveedores.Campo_Correo_Electronico;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Proveedores.Campo_Proveedor_ID;
            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Com_Propuesta_Cotizacion.Campo_Proveedor_ID;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Propuesta_Cotizacion.Tabla_Ope_Com_Propuesta_Cotizacion;
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Propuesta_Cotizacion.Campo_No_Requisicion;
            Mi_SQL = Mi_SQL + " ='" + Clase_Negocio.P_No_Requisicion + "')";

            DataTable Dt_Proveedores = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Proveedores;
        }


        public static DataTable Consultar_Datos_Cotizador(Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio Clase_Negocio)
        {
            String Mi_SQL = "SELECT " + Cat_Com_Cotizadores.Campo_Correo;
            Mi_SQL = Mi_SQL + ", " + Cat_Com_Cotizadores.Campo_Password_Correo;
            Mi_SQL = Mi_SQL + ", " + Cat_Com_Cotizadores.Campo_IP_Correo_Saliente;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Cotizadores.Tabla_Cat_Com_Cotizadores;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Cotizadores.Campo_Empleado_ID;
            Mi_SQL = Mi_SQL + "='" + Cls_Sessiones.Empleado_ID +"'";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        public static DataTable Consultar_Comentarios(Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio Clase_Negocio)
        {
            String Mi_SQL = "SELECT " + Ope_Com_Req_Observaciones.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Observaciones.Campo_Comentario;
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Observaciones.Campo_Usuario_Creo;
            Mi_SQL = Mi_SQL + ", TO_CHAR( " + Ope_Com_Req_Observaciones.Campo_Fecha_Creo + ",'DD/MON/YYYY') AS " + Ope_Com_Req_Observaciones.Campo_Fecha_Creo;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Req_Observaciones.Tabla_Ope_Com_Req_Observaciones;
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Req_Observaciones.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_No_Requisicion.Trim() + "'";
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Parametro_Invitacion
        ///DESCRIPCIÓN: Obtiene el numero consecutivo para las tablas ocupadas en esta clase
        ///PARAMETROS: 1.-Campo del cual se obtendra el consecutivo
        ///            2.-P_Nombre de la tabla
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 26 Mayo 2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        ///
        public static DataTable Consultar_Parametro_Invitacion(Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio Clase_Negocio)
        {
            String Mi_SQL = "SELECT " + Cat_Com_Parametros.Campo_Invitacion_Proveedores;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Parametros.Tabla_Cat_Com_Parametros;
           
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Consecutivo
        ///DESCRIPCIÓN: Obtiene el numero consecutivo para las tablas ocupadas en esta clase
        ///PARAMETROS: 1.-Campo del cual se obtendra el consecutivo
        ///            2.-P_Nombre de la tabla
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 26 Mayo 2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        ///
        public static int Marcar_Leida_Por_Cotizador(Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio Clase_Negocio)
        {
            String Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " SET " +
            Ope_Com_Requisiciones.Campo_Leido_Por_Cotizador + " = 'SI' " +
            " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + Clase_Negocio.P_No_Requisicion.Trim();
            return OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Consecutivo
        ///DESCRIPCIÓN: Obtiene el numero consecutivo para las tablas ocupadas en esta clase
        ///PARAMETROS: 1.-Campo del cual se obtendra el consecutivo
        ///            2.-P_Nombre de la tabla
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 26 Mayo 2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        ///
        public static DataTable Consultar_Req_Origen(Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio Clase_Datos)
        {
            String Mi_SQL = "SELECT  REQ_ORIGEN_ID ";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + "=" + Clase_Datos.P_No_Requisicion;
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

    }//fin del class
} //fin del namespace