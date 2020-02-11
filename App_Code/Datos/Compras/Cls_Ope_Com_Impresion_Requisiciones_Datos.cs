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
using Presidencia.Compras.Impresion_Requisiciones.Negocio;
using Oracle.DataAccess.Client;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;

namespace Presidencia.Compras.Impresion_Requisiciones.Datos
{

    public class Cls_Ope_Com_Impresion_Requisiciones_Datos
    {
        public Cls_Ope_Com_Impresion_Requisiciones_Datos()
        {            
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Requisiciones
        ///DESCRIPCIÓN: Consultar los detalles de la requisicion
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 04/28/2011 04:37:00 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private void Consultar_Requisiciones_Detalles()
        {

        }
        public static DataTable Consultar_Requisiciones(Cls_Ope_Com_Impresion_Requisiciones_Negocio Datos)
        {
            //Declaracion de Variables
            String Mi_SQL = String.Empty; //Variable para las consultas

            try
            {
                //Asignar consulta
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ".*, ";
                //Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + ".*, ";
                Mi_SQL = Mi_SQL + "(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE " + Cat_Dependencias.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + " = " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID + ")";
                Mi_SQL = Mi_SQL + " AS UNIDAD_RESPONSABLE, ";


                Mi_SQL = Mi_SQL + " ( SELECT ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + ".";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Nombre + " || ' ' || ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + ".";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + ".";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Apellido_Materno;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + ".";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Empleado_ID + " = ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ".";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_Empleado_Autorizacion_ID + " ) AS EMPLEADO ";

                Mi_SQL = Mi_SQL + "FROM ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
                Mi_SQL = Mi_SQL + " WHERE " +
                    Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + 
                    Ope_Com_Requisiciones.Campo_Estatus + " IS NOT NULL ";


                if (!String.IsNullOrEmpty(Datos.P_Requisicion_ID))
                {
                    Mi_SQL = Mi_SQL + " AND ";
                    Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = '" + Datos.P_Requisicion_ID + "' ";
                }
                if (!String.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    Mi_SQL = Mi_SQL + " AND ";
                    Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "' ";
                }
                if (!String.IsNullOrEmpty(Datos.P_Estatus))
                {
                    Mi_SQL = Mi_SQL + " AND ";
                    Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Estatus + " = '" + Datos.P_Estatus + "' ";
                }

                if (!String.IsNullOrEmpty(Datos.P_Fecha_Inicial) && !String.IsNullOrEmpty(Datos.P_Fecha_Final))
                {
                    Mi_SQL = Mi_SQL + " AND TO_DATE(TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Creo + ",'DD-MM-YYYY'))" +
                            " >= '" + Datos.P_Fecha_Inicial + "' AND " +
                    "TO_DATE(TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Creo + ",'DD-MM-YYYY'))" +
                            " <= '" + Datos.P_Fecha_Final + "'";
                }

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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Requisiciones_Detalles
        ///DESCRIPCIÓN: Consultar los detalles de la requisicion
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 04/28/2011 04:37:00 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        internal static DataTable Consultar_Requisiciones_Detalles(Cls_Ope_Com_Impresion_Requisiciones_Negocio Datos)
        {
            //Declaracion de Variables
            String Mi_SQL = String.Empty; //Variable para las consultas
            //Mi_SQL = "IF ((SELECT TIPO_ARTICULO FROM OPE_COM_REQUISICIONES WHERE NO_REQUISICION = '" + Datos.P_Requisicion_ID + "') = 'PRODUCTO') THEN ";
            Mi_SQL = "SELECT " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + "." + Cat_Com_Unidades.Campo_Abreviatura + " AS UNIDAD,";
            Mi_SQL += Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + ".*"; 
            Mi_SQL += " FROM ";
            Mi_SQL += Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto;
            Mi_SQL += " LEFT JOIN " + Cat_Com_Productos.Tabla_Cat_Com_Productos;
            Mi_SQL += " ON " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID;
            Mi_SQL += " = ";
            Mi_SQL += Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Producto_ID;
            Mi_SQL += " LEFT JOIN " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " ON ";
            Mi_SQL += Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Unidad_ID + " = ";
            Mi_SQL += Cat_Com_Unidades.Tabla_Cat_Com_Unidades + "." + Cat_Com_Unidades.Campo_Unidad_ID;
            Mi_SQL += " WHERE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + ".";
            Mi_SQL += Ope_Com_Req_Producto.Campo_Requisicion_ID + " = '" + Datos.P_Requisicion_ID + "' ";
            //Mi_SQL += " ELSE SELECT * FROM OPE_COM_REQ_PRODUCTO WHERE NO_REQUISICION =" + Datos.P_Requisicion_ID;
            Mi_SQL += " ORDER BY " + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio;
            try
            {
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
        ///NOMBRE DE LA FUNCIÓN:  Consultar_Detalle_Requisicion
        ///DESCRIPCIÓN: Metodo que consulta los detalles de la requisicion seleccionada en el Grid_Requisiciones
        ///PARAMETROS: 1.- Cls_Ope_Com_Definir_Cotizadores_Negocio Clase_Negocios, objeto de la clase de negocios
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 01/JULIO/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Detalle_Requisicion(Cls_Ope_Com_Impresion_Requisiciones_Negocio Clase_Negocio)
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
            Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Com_Cotizadores.Campo_Nombre_Completo + " FROM " + Cat_Com_Cotizadores.Tabla_Cat_Com_Cotizadores;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Cotizadores.Campo_Empleado_ID + "= REQUISICION." + Ope_Com_Requisiciones.Campo_Cotizador_ID + ") AS ELABORO";
           
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICION ";
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIA ";
            Mi_SQL = Mi_SQL + " ON REQUISICION." + Ope_Com_Requisiciones.Campo_Dependencia_ID + "= DEPENDENCIA.";
            Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + " WHERE REQUISICION." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_No_Requisicion.Trim() + "'";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

         ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Codigo_Programatico
        ///DESCRIPCIÓN: Consulta el codigo programatico
        ///PARAMETROS: 
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 3 Ene 13
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        ///
        public static DataTable Consultar_Codigo_Programatico(Cls_Ope_Com_Impresion_Requisiciones_Negocio Clase_Datos)
        {
            String Mi_SQL = "";
            Mi_SQL = "SELECT ";

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
        public static DataTable Consultar_Req_Origen(Cls_Ope_Com_Impresion_Requisiciones_Negocio Clase_Datos)
        {
            String Mi_SQL = "SELECT  REQ_ORIGEN_ID ";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + "=" + Clase_Datos.P_No_Requisicion;
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
        public static DataTable Consultar_Productos_Servicios(Cls_Ope_Com_Impresion_Requisiciones_Negocio Clase_Negocio)
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
    }
}