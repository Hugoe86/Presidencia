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
using Presidencia.Cotizaciones_Proveedores.Negocio;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Sessiones;
using Presidencia.Constantes;

/// <summary>
/// Summary description for Cls_Ope_Com_Cotizaciones_Proveedores_Datos
/// </summary>
namespace Presidencia.Cotizaciones_Proveedores.Datos
{

    public class Cls_Ope_Com_Cotizaciones_Proveedores_Datos
    {
        
        public Cls_Ope_Com_Cotizaciones_Proveedores_Datos()
        {
            
        }

        #region Consultas
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cotizaciones
        ///DESCRIPCIÓN: Metodo que realiza la busqueda de las cotizaciones ecistentes 
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 09/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Cotizaciones(Cls_Ope_Com_Cotizaciones_Proveedores_Negocio Datos_Cotizacion)
        {
            String Mi_SQL = "SELECT COT." + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
                ", COT." + Ope_Com_Cotizaciones.Campo_Folio +
                ", COT." + Ope_Com_Cotizaciones.Campo_Tipo +
                ", COT." + Ope_Com_Cotizaciones.Campo_Estatus +
                ", TO_CHAR(COT." + Ope_Com_Cotizaciones.Campo_Fecha_Creo + ",'DD/MON/YYYY') AS FECHA" +
                ", COT." + Ope_Com_Cotizaciones.Campo_Total +
                " FROM " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones + " COT " +
                " JOIN " + Ope_Com_Det_Cotizaciones.Tabla_Ope_Com_Det_Cotizaciones + " DET " +
                " ON DET." + Ope_Com_Det_Cotizaciones.Campo_No_Cotizacion + 
                "=COT." + Ope_Com_Cotizaciones.Campo_No_Cotizacion +  
                " WHERE COT." + Ope_Com_Cotizaciones.Campo_Estatus +
                " IN ('COTIZADOR DEFINIDO','ASIGNADA')" +
                " AND DET." + Ope_Com_Det_Cotizaciones.Campo_Empleado_ID +
                " ='" + Cls_Sessiones.Empleado_ID + "'" +
                " ORDER BY COT." + Ope_Com_Cotizaciones.Campo_Fecha_Creo;
            if (Datos_Cotizacion.P_No_Cotizacion != null)
            {
                Mi_SQL = "SELECT " + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
                    ", " + Ope_Com_Cotizaciones.Campo_Folio +
                    ", " + Ope_Com_Cotizaciones.Campo_Tipo +
                    ", " + Ope_Com_Cotizaciones.Campo_Estatus +
                    ", " + Ope_Com_Cotizaciones.Campo_Condiciones +
                    ", TO_CHAR(" + Ope_Com_Cotizaciones.Campo_Fecha_Creo + ",'DD/MON/YYYY') AS FECHA" +
                    ", " + Ope_Com_Cotizaciones.Campo_Total +
                    ", " + Ope_Com_Cotizaciones.Campo_Total_Cotizado + 
                    ", " + Ope_Com_Cotizaciones.Campo_Lista_Requisiciones +
                    ", " + Ope_Com_Cotizaciones.Campo_Listado_Almacen + 
                    " FROM " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones +
                    " WHERE " + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
                    "='" + Datos_Cotizacion.P_No_Cotizacion + "'";
            }//fin del IF
            if (Datos_Cotizacion.P_Folio != null)
            {
                Mi_SQL = "SELECT COT." + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
                ", COT." + Ope_Com_Cotizaciones.Campo_Folio +
                ", COT." + Ope_Com_Cotizaciones.Campo_Tipo +
                ", COT." + Ope_Com_Cotizaciones.Campo_Estatus +
                ", TO_CHAR(COT." + Ope_Com_Cotizaciones.Campo_Fecha_Creo + ",'DD/MON/YYYY') AS FECHA" +
                ", COT." + Ope_Com_Cotizaciones.Campo_Total +
                " FROM " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones + " COT " +
                " JOIN " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones + " DET " +
                " ON DET." + Ope_Com_Det_Cotizaciones.Campo_No_Cotizacion +
                "= COT." + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
                " WHERE COT." + Ope_Com_Cotizaciones.Campo_Estatus +
                " IN ('COTIZADOR DEFINIDO','ASIGNADA')" +
                " AND DET." + Ope_Com_Det_Cotizaciones.Campo_Empleado_ID +
                "=" + Cls_Sessiones.Empleado_ID +
                " AND DET." + Ope_Com_Cotizaciones.Campo_Folio +
                " LIKE '%" + Datos_Cotizacion.P_Folio + "%'" +
                " ORDER BY DET." + Ope_Com_Cotizaciones.Campo_Folio;
                
            }

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cotizadores
        ///DESCRIPCIÓN: Metodo que realiza la busqueda de los cotizadores pertenecientes a la cotizacion 
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 09/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public bool Consultar_Cotizadores()
        {
            bool Empleado_Cotizador = false;
            String Mi_SQL = "SELECT " + Cat_Com_Cotizadores.Campo_Empleado_ID +
                " FROM " + Cat_Com_Cotizadores.Tabla_Cat_Com_Cotizadores +
                " WHERE " + Cat_Com_Cotizadores.Campo_Empleado_ID +
                "='" + Cls_Sessiones.Empleado_ID+"'";
            DataTable Dt_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            if (Dt_Datos.Rows.Count > 0)
                Empleado_Cotizador = true;

            return Empleado_Cotizador;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Productos
        ///DESCRIPCIÓN: Metodo que realiza la busqueda de los productos o servicios pertenecientes a la cotizacion 
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 09/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Concepto_Cotizador(Cls_Ope_Com_Cotizaciones_Proveedores_Negocio Datos_Cotizacion)
        {
            String Mi_SQL = "";
            Mi_SQL = "SELECT COT." + Ope_Com_Det_Cotizaciones.Campo_Giro_ID +
                     ",(SELECT " + Cat_Sap_Concepto.Campo_Clave +
                     " FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto +
                     " WHERE " + Cat_Sap_Concepto.Campo_Concepto_ID +
                     "= COT." + Ope_Com_Det_Cotizaciones.Campo_Giro_ID + ")" +
                     " ||''|| (SELECT " + Cat_Sap_Concepto.Campo_Descripcion +
                     " FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto +
                     " WHERE " + Cat_Sap_Concepto.Campo_Concepto_ID +
                     "= COT." + Ope_Com_Det_Cotizaciones.Campo_Giro_ID + ")" +
                     " FROM " + Ope_Com_Det_Cotizaciones.Tabla_Ope_Com_Det_Cotizaciones + " COT" +
                     " WHERE COT." + Ope_Com_Det_Cotizaciones.Campo_Empleado_ID +
                     "='" + Cls_Sessiones.Empleado_ID + "'" +
                     " AND COT." + Ope_Com_Det_Cotizaciones.Campo_No_Cotizacion +
                     "='" + Datos_Cotizacion.P_No_Cotizacion+"'";
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Comite_Detalle_Requisicion
        ///DESCRIPCIÓN: Consulta las requisiciones que le pertenecen al comite de compras seleccionado
        ///PARAMETROS: 
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************

        public DataTable Consultar_Detalle_Requisicion(Cls_Ope_Com_Cotizaciones_Proveedores_Negocio Datos_Cotizacion)
        {
            String Mi_SQL = "SELECT REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                         ", REQ." + Ope_Com_Requisiciones.Campo_Folio +
                         ", DEP." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA" +
                         ", AREA." + Cat_Areas.Campo_Nombre + " AS AREA" +
                         ", TO_CHAR(REQ." + Ope_Com_Requisiciones.Campo_Fecha_Filtrado + ",'DD/MON/YYYY') AS FECHA" +
                         ", REQ." + Ope_Com_Requisiciones.Campo_Total +
                         " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQ" +
                         " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEP" +
                         " ON DEP." + Cat_Dependencias.Campo_Dependencia_ID + "=" +
                         " REQ." + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                         " JOIN " + Cat_Areas.Tabla_Cat_Areas + " AREA" +
                         " ON AREA." + Cat_Areas.Campo_Area_ID + "=" +
                         " REQ." + Ope_Com_Requisiciones.Campo_Area_ID +
                         " WHERE REQ." + Ope_Com_Requisiciones.Campo_No_Cotizacion + "='" + 
                         Datos_Cotizacion.P_No_Cotizacion + "'" +
                         " AND REQ." + Ope_Com_Requisiciones.Campo_No_Consolidacion + " IS NULL " +
                         " GROUP BY (REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                         ", REQ." + Ope_Com_Requisiciones.Campo_Folio +
                         ", DEP." + Cat_Dependencias.Campo_Nombre +
                         ", AREA." + Cat_Areas.Campo_Nombre +
                         ", REQ." + Ope_Com_Requisiciones.Campo_Fecha_Filtrado +
                         ", REQ." + Ope_Com_Requisiciones.Campo_Total + ")";
            
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Detalle_Consolidacion
        ///DESCRIPCIÓN: Consulta las requisiciones que le pertenecen al comite de compras seleccionado
        ///PARAMETROS: 
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataTable Consultar_Detalle_Consolidacion(Cls_Ope_Com_Cotizaciones_Proveedores_Negocio Datos_Cotizacion)
        {
            String Mi_SQL = "SELECT CON." + Ope_Com_Consolidaciones.Campo_No_Consolidacion +
                        ", CON." + Ope_Com_Consolidaciones.Campo_Folio +
                        ", CON." + Ope_Com_Consolidaciones.Campo_Estatus +
                        ", CON." + Ope_Com_Consolidaciones.Campo_Fecha_Creo + " AS FECHA" +
                        ", CON." + Ope_Com_Consolidaciones.Campo_Monto +
                        ", CON." + Ope_Com_Consolidaciones.Campo_Lista_Requisiciones +
                        " FROM " + Ope_Com_Consolidaciones.Tabla_Ope_Com_Consolidaciones + " CON" +
                        " JOIN " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQ" +
                        " ON REQ." + Ope_Com_Requisiciones.Campo_No_Consolidacion +
                        " =CON." + Ope_Com_Consolidaciones.Campo_No_Consolidacion +
                        " WHERE REQ." + Ope_Com_Requisiciones.Campo_No_Cotizacion +
                        " ='" + Datos_Cotizacion.P_No_Cotizacion + "'" +
                        " GROUP BY (CON." + Ope_Com_Consolidaciones.Campo_No_Consolidacion +
                        ", CON." + Ope_Com_Consolidaciones.Campo_Folio +
                        ", CON." + Ope_Com_Consolidaciones.Campo_Estatus +
                        ", CON." + Ope_Com_Consolidaciones.Campo_Fecha_Creo +
                        ", CON." + Ope_Com_Consolidaciones.Campo_Monto +
                        ", CON." + Ope_Com_Consolidaciones.Campo_Lista_Requisiciones + ")";
            
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;

        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Productos
        ///DESCRIPCIÓN: Metodo que realiza la busqueda de los productos o servicios pertenecientes a la cotizacion 
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 09/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Productos(Cls_Ope_Com_Cotizaciones_Proveedores_Negocio Datos_Cotizacion)
        {

            String Mi_SQL = "";

            switch (Datos_Cotizacion.P_Tipo)
            {
                case "PRODUCTO":
                    Mi_SQL = "SELECT DET." + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
                            ", PRO." + Cat_Com_Productos.Campo_Producto_ID + " AS PROD_SERV_ID" +
                            ", PRO." + Cat_Com_Productos.Campo_Nombre + " AS NOMBRE_PROD_SERV" +
                            ", DET." + Ope_Com_Req_Producto.Campo_Cantidad +
                            ", DET." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_IVA_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_IEPS_Cotizado +
                            ", CONCEPTO." + Cat_Sap_Concepto.Campo_Concepto_ID +
                            ", DET." + Ope_Com_Req_Producto.Campo_Subtota_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_Proveedor_ID +
                            ", DET." + Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
                            " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " DET" +
                            " JOIN " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRO" +
                            " ON PRO." + Cat_Com_Productos.Campo_Producto_ID +
                            " = DET." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +
                            " JOIN " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQ" +
                            " ON REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                            " = DET." + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                            " JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " PARTIDA_ESP" +
                            " ON PARTIDA_ESP." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + "=" +
                            "DET." + Ope_Com_Req_Producto.Campo_Partida_ID +
                            " JOIN " + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + " PARTIDA_GEN" +
                            " ON PARTIDA_GEN." + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID + 
                            " = PARTIDA_ESP." + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID +
                            " JOIN " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + " CONCEPTO" +
                            " ON CONCEPTO." + Cat_Sap_Concepto.Campo_Concepto_ID + 
                            " = PARTIDA_GEN." + Cat_SAP_Partida_Generica.Campo_Concepto_ID +
                            " WHERE REQ." + Ope_Com_Requisiciones.Campo_No_Cotizacion +
                            " = '" + Datos_Cotizacion.P_No_Cotizacion + "'" +
                            " AND DET." + Ope_Com_Req_Producto.Campo_Empleado_Cotizador_ID + 
                            " = '" +Cls_Sessiones.Empleado_ID +"'";
                    break;
                case "SERVICIO":
                    Mi_SQL = "SELECT DET." + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
                            ", SER." + Cat_Com_Servicios.Campo_Servicio_ID + " AS PROD_SER_ID" +
                            ", SER." + Cat_Com_Servicios.Campo_Nombre + " AS NOMBRE_PROD_SERV" +
                            ", DET." + Ope_Com_Req_Producto.Campo_Cantidad +
                            ", DET." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_IVA_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_IEPS_Cotizado +
                            ", CONCEPTO." + Cat_Sap_Concepto.Campo_Concepto_ID +
                            ", DET." + Ope_Com_Req_Producto.Campo_Subtota_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_Proveedor_ID +
                            ", DET." + Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
                            " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " DET" +
                            " JOIN " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + " SER" +
                            " ON SER." + Cat_Com_Servicios.Campo_Servicio_ID +
                            " = DET." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +
                            " JOIN " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQ" +
                            " ON REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                            " = DET." + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                            " JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " PARTIDA_ESP" +
                            " ON PARTIDA_ESP." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + "=" +
                            "DET." + Ope_Com_Req_Producto.Campo_Partida_ID +
                            " JOIN " + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + " PARTIDA_GEN" +
                            " ON PARTIDA_GEN." + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID + 
                            " = PARTIDA_ESP." + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID +
                            " JOIN " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + " CONCEPTO" +
                            " ON CONCEPTO." + Cat_Sap_Concepto.Campo_Concepto_ID + 
                            " = PARTIDA_GEN." + Cat_SAP_Partida_Generica.Campo_Concepto_ID +
                            " WHERE REQ." + Ope_Com_Requisiciones.Campo_No_Cotizacion +
                            " = '" + Datos_Cotizacion.P_No_Cotizacion + "'" +
                            " AND DET." + Ope_Com_Req_Producto.Campo_Empleado_Cotizador_ID +
                            " = '" + Cls_Sessiones.Empleado_ID + "'";

                    break;

            }
        
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Proveedores
        ///DESCRIPCIÓN: Metodo que realiza la busqueda de los proveedores que cumplen con el giro. 
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 09/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Proveedores(Cls_Ope_Com_Cotizaciones_Proveedores_Negocio Datos_Cotizacion)
        {
            String Mi_SQL = "SELECT PRO." + Cat_Com_Proveedores.Campo_Proveedor_ID +
                            ", PRO." + Cat_Com_Proveedores.Campo_Nombre +
                            " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PRO" +
                            " JOIN " + Cat_Com_Giro_Proveedor.Tabla_Cat_Com_Giro_Proveedor + " DET" +
                            " ON DET." + Cat_Com_Giro_Proveedor.Campo_Proveedor_ID +
                            " = PRO." + Cat_Com_Proveedores.Campo_Proveedor_ID +
                            " WHERE DET." + Cat_Com_Giro_Proveedor.Campo_Giro_ID +
                            " ='" + Datos_Cotizacion.P_Giro_ID + "'";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Proveedores_Asignados
        ///DESCRIPCIÓN: Consulta las requisiciones que le pertenecen al comite de compras seleccionado
        ///PARAMETROS: 
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataTable Consultar_Proveedores_Asignados(Cls_Ope_Com_Cotizaciones_Proveedores_Negocio Datos_Cotizacion)
        {
            String Mi_SQL = "SELECT CONCEPTO." + Cat_Sap_Concepto.Campo_Concepto_ID +
                            ",CONCEPTO." + Cat_Sap_Concepto.Campo_Clave + " AS CLAVE_CONCEPTO " +
                            ", CONCEPTO." + Cat_Sap_Concepto.Campo_Descripcion + " AS DESCRIPCION_CONCEPTO" +
                            ", DET." + Ope_Com_Req_Producto.Campo_Proveedor_ID +
                            ", DET." + Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
                            " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " DET" +
                            " JOIN " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQ" +
                            " ON REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                            "= DET." + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                            " JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " PARTIDA_ESP" +
                            " ON PARTIDA_ESP." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + "=" +
                            "DET." + Ope_Com_Req_Producto.Campo_Partida_ID +
                            " JOIN " + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + " PARTIDA_GEN" +
                            " ON PARTIDA_GEN." + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID +
                            " = PARTIDA_ESP." + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID +
                            " JOIN " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + " CONCEPTO" +
                            " ON CONCEPTO." + Cat_Sap_Concepto.Campo_Concepto_ID +   
                            "=PARTIDA_GEN." + Cat_SAP_Partida_Generica.Campo_Concepto_ID +
                            " WHERE REQ." + Ope_Com_Requisiciones.Campo_No_Cotizacion +
                            "='" + Datos_Cotizacion.P_No_Cotizacion + "'" +
                            " AND DET." + Ope_Com_Req_Producto.Campo_Proveedor_ID +
                            " IS NOT NULL" +
                            " AND DET." + Ope_Com_Req_Producto.Campo_Empleado_Cotizador_ID + 
                            "='" + Cls_Sessiones.Empleado_ID + "'" +
                            " GROUP BY ( CONCEPTO." + Cat_Sap_Concepto.Campo_Concepto_ID +
                            ",CONCEPTO." + Cat_Sap_Concepto.Campo_Clave + 
                            ", CONCEPTO." + Cat_Sap_Concepto.Campo_Descripcion +
                            ", DET." + Ope_Com_Req_Producto.Campo_Proveedor_ID +
                            ", DET." + Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
                            ", DET." + Ope_Com_Req_Producto.Campo_Proveedor_ID +
                            ", DET." + Ope_Com_Req_Producto.Campo_Nombre_Proveedor + ")";

            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
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
        public DataTable Consultar_Impuesto_Producto(Cls_Ope_Com_Cotizaciones_Proveedores_Negocio Datos_Cotizacion)
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
                         " ='" + Datos_Cotizacion.P_Producto_ID + "'";
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }

        #endregion Fin_Consultas

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Cotizacion
        ///DESCRIPCIÓN: Metodo que realiza la modificacion de la cotizacion de acuerdo al estatus. 
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 09/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Modificar_Cotizacion(Cls_Ope_Com_Cotizaciones_Proveedores_Negocio Datos_Cotizacion)
        {
            String Mi_SQL = "";
            
            //Modifica estatus de Cotizacion

            Mi_SQL = "UPDATE " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones;
            Mi_SQL = Mi_SQL + " SET " + Ope_Com_Cotizaciones.Campo_Estatus;
            Mi_SQL = Mi_SQL + " ='" + Datos_Cotizacion.P_Estatus + "'";
            Mi_SQL = Mi_SQL + "," + Ope_Com_Cotizaciones.Campo_Usuario_Modifico;
            Mi_SQL = Mi_SQL + "='" + Cls_Sessiones.Nombre_Empleado + "'";
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Cotizaciones.Campo_Fecha_Modifico;
            Mi_SQL = Mi_SQL + "=SYSDATE";
            if (Datos_Cotizacion.P_Estatus.Trim() != "CANCELADA")
            {
                Mi_SQL = Mi_SQL + ", " + Ope_Com_Cotizaciones.Campo_Total_Cotizado;
                Mi_SQL = Mi_SQL + "='" +Datos_Cotizacion.P_Total_Cotizado + "'";
            }
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Cotizaciones.Campo_No_Cotizacion;
            Mi_SQL = Mi_SQL + "='" + Datos_Cotizacion.P_No_Cotizacion + "'";

            if (Datos_Cotizacion.P_Estatus.Trim() == "CANCELADA")
            {
                //CAmbiamos el estatus de las requisiciones a FILTRADA Y EL 
                String Mi_Sql = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                        " SET " + Ope_Com_Requisiciones.Campo_Estatus +
                        "='FILTRADA'" +
                        ", " + Ope_Com_Requisiciones.Campo_Tipo_Compra +
                        "=NULL " +
                        ", " + Ope_Com_Requisiciones.Campo_No_Cotizacion +
                        "= NULL" +
                        " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                        " IN (" + Datos_Cotizacion.P_Lista_Requisiciones + ")";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                //MODIFICAMOS LOS DETALLES DE LA REQUISICION
                Mi_Sql = "UPDATE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                    " SET " + Ope_Com_Req_Producto.Campo_IVA_Cotizado + "=NULL," +
                    Ope_Com_Req_Producto.Campo_IEPS_Cotizado + "=NULL," +
                    Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado + "=NULL," +
                    Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado + "=NULL," +
                    Ope_Com_Req_Producto.Campo_Subtota_Cotizado + "=NULL," +
                    Ope_Com_Req_Producto.Campo_Total_Cotizado + "=NULL," +
                    Ope_Com_Req_Producto.Campo_Proveedor_ID + "=NULL," +
                    Ope_Com_Req_Producto.Campo_Nombre_Proveedor + "=NULL," +
                    Ope_Com_Req_Producto.Campo_Empleado_Cotizador_ID + "=NULL" +
                    " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                    " IN(" + Datos_Cotizacion.P_Lista_Requisiciones + ")";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                Mi_SQL = Mi_SQL + ", " + Ope_Com_Cotizaciones.Campo_Lista_Requisiciones +
                "=" + Datos_Cotizacion.P_Lista_Requisiciones;
                //Modificamos los detalles de los productos 
                Modificar_Detalle_Productos(Datos_Cotizacion);
            }
                         
                if (Datos_Cotizacion.P_Estatus == "TERMINADA")
                {
                    Modificar_Detalle_Productos(Datos_Cotizacion);
                    //DEspues de modificar los detalles de Producto se modifican los presupuestos 
                    Modificar_Presupuesto(Datos_Cotizacion);
                    //CAmbiamos el estatus de las requisiciones a Cotizadas
                    String Mi_Sql = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                            " SET " + Ope_Com_Requisiciones.Campo_Estatus +
                            "='COTIZADA'" +
                            " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                            " IN (" + Datos_Cotizacion.P_Lista_Requisiciones + ")";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                    //Modificamos el total cotizado de cada requisicion perteneciente a la cotizacion
                    //primero se obtiene el listado de requisiciones pertenecientes a esta cotizacion 
                    Mi_Sql = "SELECT " + Ope_Com_Cotizaciones.Campo_Lista_Requisiciones +
                            " FROM " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones +
                            " WHERE " + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
                            "= '" + Datos_Cotizacion.P_No_Cotizacion+"'";
                    DataTable Dt_Req = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql).Tables[0];
                    String[] Arr_Requisiciones = Dt_Req.Rows[0][Ope_Com_Cotizaciones.Campo_Lista_Requisiciones].ToString().Split(',');
                    for (int i = 0; i < Arr_Requisiciones.Length; i++)
                    {
                        Modificar_Montos_Cotizados_Requisiciones(Arr_Requisiciones[i]);
                    }
                    // Calculamos el total cotizado de la cotizacion
                    Calcular_Total_Cotizado(Datos_Cotizacion);
                }
            
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                


        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Calcular_Total_Cotizado
        ///DESCRIPCIÓN: Metodo que calcula el total cotizado de la cotizacion creada
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 09/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Calcular_Total_Cotizado(Cls_Ope_Com_Cotizaciones_Proveedores_Negocio Datos_Cotizacion)
        {
            String Mi_SQL = "SELECT " + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                    " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                    " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                    " IN (" + Datos_Cotizacion.P_Lista_Requisiciones + ")";
            DataTable Dt_Productos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            double Total_Cotizado = 0;
            //Recorremos todos los productos pertenecientes a las requisiciones de la cotizacion
            for (int i = 0; i<Dt_Productos.Rows.Count; i++)
            {
                if(Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Total_Cotizado].ToString().Trim() != String.Empty)
                    Total_Cotizado += double.Parse(Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Total_Cotizado].ToString());
            }
            //Modificamos el total cotizado
            Mi_SQL = "UPDATE " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones +
                " SET " + Ope_Com_Cotizaciones.Campo_Total_Cotizado +
                "='" + Total_Cotizado.ToString() + "'" +
                " WHERE " + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
                " ='" + Datos_Cotizacion.P_No_Cotizacion + "'";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Detalle_Productos
        ///DESCRIPCIÓN: Metodo que modifica los detalles cotizados de los productos pertenecientes a la cotizacion 
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 09/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Modificar_Detalle_Productos(Cls_Ope_Com_Cotizaciones_Proveedores_Negocio Datos_Cotizacion)
        {
            String Mi_SQL = "";
            //Recorremos el datatable de Productos para actualizar los valores de los detalles de las requisiciones pertenecientes 
            for (int i = 0; i < Datos_Cotizacion.P_Dt_Productos.Rows.Count; i++)
            {
                Mi_SQL = "UPDATE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                         " SET " + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
                         " ='" + Datos_Cotizacion.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado].ToString().Trim() + "'" +
                         ", " + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                         " ='" + Datos_Cotizacion.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado].ToString().Trim() + "'" +
                         ", " + Ope_Com_Req_Producto.Campo_IVA_Cotizado +
                         "='" + Datos_Cotizacion.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_IVA_Cotizado].ToString().Trim() + "'" +
                         ", " + Ope_Com_Req_Producto.Campo_IEPS_Cotizado +
                         "='" + Datos_Cotizacion.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_IEPS_Cotizado].ToString().Trim() + "'" +
                         ", " + Ope_Com_Req_Producto.Campo_Subtota_Cotizado +
                         "='" + Datos_Cotizacion.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Subtota_Cotizado].ToString().Trim() + "'" +
                         ", " + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                         "='" + Datos_Cotizacion.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Total_Cotizado].ToString().Trim() + "'" +
                         ", " + Ope_Com_Req_Producto.Campo_Proveedor_ID +
                         "='" + Datos_Cotizacion.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Proveedor_ID].ToString().Trim() + "'" +
                         ", " + Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
                         "='" + Datos_Cotizacion.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Nombre_Proveedor].ToString().Trim() + "'" +
                         " WHERE " + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
                         "='" + Datos_Cotizacion.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID].ToString().Trim() + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            } //fin del for

        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Montos_Cotizados_Requisiciones
        ///DESCRIPCIÓN: Metodo que modifica el monto total cotizado de una requisicion
        ///PARAMETROS: 1.- String No_Requisicion: No_Requisicion de la que se calculara su total cotizado
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 09/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Modificar_Montos_Cotizados_Requisiciones(String No_Requisicion)
        {
            String Mi_SQL = "SELECT * FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                " WHERE "+ Ope_Com_Req_Producto.Campo_Requisicion_ID +
                "='" + No_Requisicion+ "'";
            DataTable Dt_Productos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            double IVA_COTIZADO = 0;
            double IEPS_COTIZADO = 0;
            double SUBTOTAL_COTIZADO = 0;
            double TOTAL_COTIZADO = 0;
            for (int i=0; i < Dt_Productos.Rows.Count; i++)
            {
                IVA_COTIZADO += double.Parse(Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_IVA_Cotizado].ToString());
                IEPS_COTIZADO += double.Parse(Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_IEPS_Cotizado].ToString());
                SUBTOTAL_COTIZADO += double.Parse(Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Subtota_Cotizado].ToString());
                TOTAL_COTIZADO += double.Parse(Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Total_Cotizado].ToString());
            }


        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Presupuesto
        ///DESCRIPCIÓN: Metodo que modifica el monto total cotizado de una requisicion
        ///PARAMETROS: 1.- String No_Requisicion: No_Requisicion de la que se calculara su total cotizado
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 09/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Modificar_Presupuesto(Cls_Ope_Com_Cotizaciones_Proveedores_Negocio Datos_Cotizacion)
        {
            String Mi_SQL = "";
            double Diferencia = 0;
            String Partida_ID = "";
            String Dependencia_ID = "";
            String Proyecto_ID = "";
            String FF = "";//Fuente de financiamiento 
            double Presupuesto_Disponible = 0;
            double Monto_Anterior = 0;
            bool Suma_Diferencia = false;
            //Recorremos el Dt_Productos perteneciente al registro de Comite de compras
            for (int i = 0; i < Datos_Cotizacion.P_Dt_Productos.Rows.Count; i++)
            {
                double Monto_Cotizado = double.Parse(Datos_Cotizacion.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Total_Cotizado].ToString().Trim());
                //REalizamos la consulta que nos traera la dependencia, la partida y el proyecto al que pertenece el producto actual
                Mi_SQL = "SELECT " + Ope_Com_Req_Producto.Campo_Partida_ID +
                        ", " + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID +
                        ", (SELECT " + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                        " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                        " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                        " =(SELECT " + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                        " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                        " WHERE " + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
                        "='" + Datos_Cotizacion.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID].ToString().Trim() + "'))" +
                        ", " + Ope_Com_Req_Producto.Campo_Monto_Total +
                        ", " + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID +
                        " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                        " WHERE " + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID + 
                        "='" + Datos_Cotizacion.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID].ToString().Trim()+ "'";
                DataTable Dt_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                Partida_ID = Dt_Datos.Rows[0][Ope_Com_Req_Producto.Campo_Partida_ID].ToString();
                Proyecto_ID = Dt_Datos.Rows[0][Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID].ToString();
                Dependencia_ID = Dt_Datos.Rows[0][2].ToString();
                FF = Dt_Datos.Rows[0][Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID].ToString();
                Monto_Anterior = double.Parse(Dt_Datos.Rows[0][Ope_Com_Req_Producto.Campo_Monto_Total].ToString());
                // PASO 1 VERIFICAMOS CUAL DE LOS 2 MONTOS ES MAYOR SI EL COTIZADO O  EL ANTERIOR
                if (Monto_Cotizado > Monto_Anterior)
                {
                    //Obtenemos la resta
                    Diferencia = Monto_Cotizado - Monto_Anterior;
                    Suma_Diferencia = true;
                }
                if (Monto_Cotizado < Monto_Anterior)
                {
                    //obtener resta
                    Diferencia = Monto_Anterior - Monto_Cotizado;
                    Suma_Diferencia = false;
                }

                DataTable Dt_Presupuestos = Consultar_Presupuesto_Partidas(Partida_ID, Dependencia_ID, Proyecto_ID, FF);
                Presupuesto_Disponible = double.Parse(Dt_Presupuestos.Rows[0][Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible].ToString());
                
                //MODIFICAMOS LOS PRESUPUESTOS DE ACUERDO AL CASO EN EL QUE ENTRE EL MONTO RESTANTE DE LO COTIZADO 
                //Es true cuando necesitamos pedir mas presupuesto
                //Es false si sobra dinero, osea que se necesita liberar presupuesto ps este presupuesto sobro
                if (Suma_Diferencia == true)
                {
                    if (Diferencia < Presupuesto_Disponible)
                    {
                        //se ACTUALIZAN LOS PRESUPUESTOS DE LA DEPENDENCIA, CORRESPONDIENTE AL PROYECTO Y PARTIDA

                        Mi_SQL = "UPDATE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                            " SET " + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible +
                            " =" + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + " - " + Diferencia.ToString() +
                            "," + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido +
                            "=" + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + " + " + Diferencia.ToString() +
                            " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
                            "='" + Partida_ID + "'" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID +
                            "='" + Proyecto_ID + "'" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio +
                            " = (SELECT MAX(" + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ")" +
                            " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                            " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
                            "='" + Partida_ID + "'" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID +
                            "='" + Proyecto_ID + "'" +
                             " AND " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID +
                            "='" + FF + "'" + 
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
                            "= TO_CHAR(SYSDATE,'YYYY'))" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
                            "= TO_CHAR(SYSDATE,'YYYY')" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID +
                            "='" + Dependencia_ID + "'";
                        //Sentencia que ejecuta el query
                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                        //ACTUALIZAMOS LOS PRESUPUESTOS DE LA PARTIDA
                        //Mi_SQL = "UPDATE " + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida +
                        //    " SET " + Ope_Com_Pres_Partida.Campo_Monto_Disponible +
                        //    " =" + Ope_Com_Pres_Partida.Campo_Monto_Disponible + " - " + Diferencia.ToString() +
                        //    "," + Ope_Com_Pres_Partida.Campo_Monto_Comprometido +
                        //    "=" + Ope_Com_Pres_Partida.Campo_Monto_Comprometido + " + " + Diferencia.ToString() +
                        //    " WHERE " + Ope_Com_Pres_Partida.Campo_Partida_ID +
                        //    "='" + Partida_ID + "'" +
                        //    " AND " + Ope_Com_Pres_Partida.Campo_Anio_Presupuesto +
                        //    "= TO_CHAR(SYSDATE,'YYYY')";
                        //OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        ////ACUTUALIZAMOS LOS PRESUPUESTOS DEL PROYECTO
                        //Mi_SQL = "UPDATE " + Ope_Com_Pres_Prog_Proy.Tabla_Ope_Com_Pres_Prog_Proy +
                        //    " SET " + Ope_Com_Pres_Prog_Proy.Campo_Monto_Disponible +
                        //    " =" + Ope_Com_Pres_Prog_Proy.Campo_Monto_Disponible + " - " + Diferencia.ToString() +
                        //    "," + Ope_Com_Pres_Prog_Proy.Campo_Monto_Comprometido +
                        //    "=" + Ope_Com_Pres_Prog_Proy.Campo_Monto_Comprometido + " + " + Diferencia.ToString() +
                        //    " WHERE " + Ope_Com_Pres_Prog_Proy.Campo_Pres_Prog_Proy_ID +
                        //    "='" + Proyecto_ID + "'" +
                        //    " AND " + Ope_Com_Pres_Prog_Proy.Campo_Anio_Presupuesto +
                        //    "= TO_CHAR(SYSDATE,'YYYY')";
                        //OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    }
                    else
                    {
                        //Si no existe presupuesto modificamos a cero todos los valores cotizados 

                        Mi_SQL = "UPDATE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                         " SET " + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
                         " ='0'" +
                         ", " + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                         " ='0'" +
                         ", " + Ope_Com_Req_Producto.Campo_IVA_Cotizado +
                         "='0'" +
                         ", " + Ope_Com_Req_Producto.Campo_IEPS_Cotizado +
                         "='0'" +
                         ", " + Ope_Com_Req_Producto.Campo_Subtota_Cotizado +
                         "='0'" +
                         ", " + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                         "='0'" +
                         ", " + Ope_Com_Req_Producto.Campo_Proveedor_ID +
                         "='0'" +
                         ", " + Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
                         "='0'" +
                         " WHERE " + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
                         "='" + Datos_Cotizacion.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID].ToString().Trim() + "'";
                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }

                }//fin if SumaDiferencia
                else
                {
                    //Modificamos el presupuesto, ya que se resta el monto que sobro pues el valor cotizado es menor k el anterior
                    //se ACTUALIZAN LOS PRESUPUESTOS DE LA DEPENDENCIA, CORRESPONDIENTE AL PROYECTO Y PARTIDA

                    Mi_SQL = "UPDATE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                        " SET " + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible +
                        " =" + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + " + " + Diferencia.ToString() +
                        "," + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido +
                        "=" + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + " - " + Diferencia.ToString() +
                        " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
                        "='" + Partida_ID + "'" +
                        " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID +
                        "='" + Proyecto_ID + "'" +
                        " AND " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio +
                        " = (SELECT MAX(" + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ")" +
                        " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                        " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
                        "='" + Partida_ID+ "'" +
                        " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID +
                        "='" + Proyecto_ID + "'" +
                        " AND " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID +
                        "='" + FF + "'" + 
                        " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
                        "= TO_CHAR(SYSDATE,'YYYY'))" +
                        " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
                        "= TO_CHAR(SYSDATE,'YYYY')" +
                        " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID +
                        "='" + Dependencia_ID + "'";
                    //Sentencia que ejecuta el query
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    //ACTUALIZAMOS LOS PRESUPUESTOS DE LA PARTIDA
                    //Mi_SQL = "UPDATE " + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida +
                    //    " SET " + Ope_Com_Pres_Partida.Campo_Monto_Disponible +
                    //    " =" + Ope_Com_Pres_Partida.Campo_Monto_Disponible + " + " + Diferencia.ToString() +
                    //    "," + Ope_Com_Pres_Partida.Campo_Monto_Comprometido +
                    //    "=" + Ope_Com_Pres_Partida.Campo_Monto_Comprometido + " - " + Diferencia.ToString() +
                    //    " WHERE " + Ope_Com_Pres_Partida.Campo_Partida_ID +
                    //    "='" + Partida_ID + "'" +
                    //    " AND " + Ope_Com_Pres_Partida.Campo_Anio_Presupuesto +
                    //    "= TO_CHAR(SYSDATE,'YYYY')";
                    //OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    //ACUTUALIZAMOS LOS PRESUPUESTOS DEL PROYECTO
                    //Mi_SQL = "UPDATE " + Ope_Com_Pres_Prog_Proy.Tabla_Ope_Com_Pres_Prog_Proy +
                    //    " SET " + Ope_Com_Pres_Prog_Proy.Campo_Monto_Disponible +
                    //    " =" + Ope_Com_Pres_Prog_Proy.Campo_Monto_Disponible + " + " + Diferencia.ToString() +
                    //    "," + Ope_Com_Pres_Prog_Proy.Campo_Monto_Comprometido +
                    //    "=" + Ope_Com_Pres_Prog_Proy.Campo_Monto_Comprometido + " - " + Diferencia.ToString() +
                    //    " WHERE " + Ope_Com_Pres_Prog_Proy.Campo_Pres_Prog_Proy_ID +
                    //    "='" + Proyecto_ID + "'" +
                    //    " AND " + Ope_Com_Pres_Prog_Proy.Campo_Anio_Presupuesto +
                    //    "= TO_CHAR(SYSDATE,'YYYY')";
                    //OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                }
                //Como se puede observar no se agrego caso para cuando el monto cotizado y el monto anterior sean iguales ya que no se afecta nada en este caso

            }//Fin del FOR

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Presupuesto_Partidas
        ///DESCRIPCIÓN: Metodo que consulta la tabla de Presupuestos partidas
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO:
        ///FECHA_CREO: 22/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Presupuesto_Partidas(String Partida, String Dependencia, String Proyecto,String FF)
        {
            String Mi_SQL = "SELECT " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
                            ", " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
                            ", " + Cat_Com_Dep_Presupuesto.Campo_Monto_Presupuestal +
                            ", " + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible +
                            ", " + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido +
                            " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                            " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID + "= '" + Partida+ "'" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + "='" + Proyecto+ "'" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + "='" + Dependencia + "'" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID + "='" + FF + "'" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + "= TO_CHAR(SYSDATE,'YYYY')" +
                            " ORDER BY " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + " DESC";
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }
    }//fin del class

}//fin del namespace