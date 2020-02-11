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
using Presidencia.Consolidar_Requisicion.Negocio;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Sessiones;
namespace Presidencia.Consolidar_Requisicion.Datos
{
    public class Cls_Ope_Com_Consolidar_Requisicion_Datos
    {
        #region METODOS
        public Cls_Ope_Com_Consolidar_Requisicion_Datos()
        {

        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Requisicion
        ///DESCRIPCIÓN: Obtiene un DataTable de una consulta de la tabla de Ope_Com_Requisiciones
        ///PARAMETROS: 1.-Objeto de negocio con datos
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 11/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Requisicion(Cls_Ope_Com_Consolidar_Requisicion_Negocio Negocio )
        {
            String Mi_Sql = "SELECT " +
            Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ".*," +
                "(SELECT " + Cat_Dependencias.Campo_Nombre + 
                " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + 
                "=" + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID + ") AS NOMBRE_DEPENDENCIA " +            
            " FROM " + 
            Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
            " WHERE " +
            Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Estatus +
            " = '" + Negocio.P_Estatus + "' AND " +
            Ope_Com_Requisiciones.Campo_Consolidada + " = 'NO' AND " + 
            Ope_Com_Requisiciones.Campo_Tipo + " = '" + Negocio.P_Tipo + "' AND " +
            Ope_Com_Requisiciones.Campo_Tipo_Articulo + " ='" + Negocio.P_Tipo_Articulo + "' AND " +
            Ope_Com_Requisiciones.Campo_Tipo_Compra + " IS NULL";
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            DataTable _DataTable = null;
            if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
            {
                _DataTable = _DataSet.Tables[0];
            }
            return _DataTable;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Requisiciones_De_Consolidacion
        ///DESCRIPCIÓN: Obtiene un DataTable de una consulta de la tabla de Ope_Com_Requisiciones
        ///PARAMETROS: 1.-Objeto de negocio con datos
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 11/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Requisiciones_De_Consolidacion(Cls_Ope_Com_Consolidar_Requisicion_Negocio Negocio)
        {
            String Mi_Sql = "SELECT " +
            Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ".*, 'A' AS GRUPO," +
                "(SELECT " + Cat_Dependencias.Campo_Nombre +
                " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                " WHERE " + Cat_Dependencias.Campo_Dependencia_ID +
                "=" + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID + ") AS NOMBRE_DEPENDENCIA " +
            " FROM " +
            Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
            " WHERE " +
            Ope_Com_Requisiciones.Campo_Requisicion_ID + " IN (" + Negocio.P_Requisas_Seleccionadas + ")";
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            DataTable _DataTable = null;
            if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
            {
                _DataTable = _DataSet.Tables[0];
            }
            return _DataTable;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consolidar_Requisiciones_Productos
        ///DESCRIPCIÓN: Obtiene un DataTable de una consulta con la consolidacin
        ///de lso productos de las requisas
        ///PARAMETROS: 1.-Objeto de negocio con datos
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consolidar_Requisiciones_Productos(Cls_Ope_Com_Consolidar_Requisicion_Negocio Negocio)
        {
           
            String Mi_Sql = "";
            Mi_Sql += "SELECT " +
            "PROD_SERV." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " AS ID, " +
            "PRODUCTOS." + Cat_Com_Productos.Campo_Clave + ", " +
            "PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + ", " +
            "SUM(PROD_SERV." + Ope_Com_Req_Producto.Campo_Cantidad + ") AS CANTIDAD, " +
            //"PRODUCTOS." + Cat_Com_Productos.Campo_Giro_ID + ", " +
            //"GIROS." + Cat_Com_Giros.Campo_Nombre + " AS NOMBRE_GIRO, " +
            "'PRODUCTO' AS TIPO, " +

            "PROD_SERV." + Ope_Com_Req_Producto.Campo_Nombre_Proveedor + ", " +

            //CALCULAMOS EL IMPORTE TOTAL DE LOS PRODUCTOS YA CON IMPUESTOS
            //((PRODUCTOS.COSTO * 
            //(SELECT PORCENTAJE_IMPUESTO FROM CAT_COM_IMPUESTOS 
            //WHERE IMPUESTO_ID = PRODUCTOS.IMPUESTO_ID)/100)+(PRODUCTOS.COSTO * 
            //(SELECT PORCENTAJE_IMPUESTO FROM CAT_COM_IMPUESTOS WHERE IMPUESTO_ID = PRODUCTOS.IMPUESTO_2_ID)/100)+
            //PRODUCTOS.COSTO)*SUM(PROD_SERV.CANTIDAD) AS IMPORTE_TOTAL_CON_IMP

            //"PRODUCTOS." + Cat_Com_Productos.Campo_Costo + " AS IMPORTE_TOTAL_CON_IMP, " +
                "((PRODUCTOS." + Cat_Com_Productos.Campo_Costo + " * " + 
                    "(SELECT " + Cat_Com_Impuestos.Campo_Porcentaje_Impuesto + 
                    " FROM " + Cat_Com_Impuestos.Tabla_Cat_Impuestos +
                    " WHERE " + Cat_Com_Impuestos.Campo_Impuesto_ID +
                    " = PRODUCTOS." + Cat_Com_Productos.Campo_Impuesto_ID + ")/100)+" +
                "(PRODUCTOS." + Cat_Com_Productos.Campo_Costo + " * " + 
                    "(SELECT " + Cat_Com_Impuestos.Campo_Porcentaje_Impuesto + 
                    " FROM " + Cat_Com_Impuestos.Tabla_Cat_Impuestos +
                    " WHERE " + Cat_Com_Impuestos.Campo_Impuesto_ID +
                    " = PRODUCTOS." + Cat_Com_Productos.Campo_Impuesto_2_ID + ")/100)+" +
                "PRODUCTOS." + Cat_Com_Productos.Campo_Costo + ") * SUM(PROD_SERV." + 
                    Ope_Com_Req_Producto.Campo_Cantidad + ") AS IMPORTE_TOTAL_CON_IMP, " +

                "((PROD_SERV." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado + " * " +
                    "(SELECT " + Cat_Com_Impuestos.Campo_Porcentaje_Impuesto +
                    " FROM " + Cat_Com_Impuestos.Tabla_Cat_Impuestos +
                    " WHERE " + Cat_Com_Impuestos.Campo_Impuesto_ID +
                    " = PRODUCTOS." + Cat_Com_Productos.Campo_Impuesto_ID + ")/100)+" +
                "(PROD_SERV." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado + " * " +
                    "(SELECT " + Cat_Com_Impuestos.Campo_Porcentaje_Impuesto +
                    " FROM " + Cat_Com_Impuestos.Tabla_Cat_Impuestos +
                    " WHERE " + Cat_Com_Impuestos.Campo_Impuesto_ID +
                    " = PRODUCTOS." + Cat_Com_Productos.Campo_Impuesto_2_ID + ")/100)+" +
                "PROD_SERV." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado + ") * SUM(PROD_SERV." +
                    Ope_Com_Req_Producto.Campo_Cantidad + ") AS IMPORTE_TOTAL_CON_IMP_COT, " +

            "PRODUCTOS." + Cat_Com_Productos.Campo_Costo_Promedio + " AS COSTO_PROM_UNIDAD_SIN_IMP, " +
            "PRODUCTOS." + Cat_Com_Productos.Campo_Estatus + 
            " FROM " +
            Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISA JOIN " +
            Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " PROD_SERV ON (REQUISA." +
            Ope_Com_Requisiciones.Campo_Requisicion_ID +
            "=PROD_SERV." + Ope_Com_Req_Producto.Campo_Requisicion_ID + ") JOIN " +
            Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ON (PROD_SERV." +
            Ope_Com_Req_Producto.Campo_Prod_Serv_ID + "=PRODUCTOS." +
            Cat_Com_Productos.Campo_Producto_ID + ")" + //" JOIN " +
            //Cat_Com_Giros.Tabla_Cat_Com_Giros + " GIROS ON (PRODUCTOS." +
            //Cat_Com_Productos.Campo_Giro_ID + " = GIROS." +
            //Cat_Com_Giros.Campo_Giro_ID + ")" +
            " WHERE REQUISA." +
            Ope_Com_Requisiciones.Campo_Requisicion_ID + " IN (" + Negocio.P_Requisas_Seleccionadas + ")" +
            " GROUP BY (" +
            " PROD_SERV." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + "," +
            " PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + "," +
            //" PRODUCTOS." + Cat_Com_Productos.Campo_Giro_ID + "," +
            " PRODUCTOS." + Cat_Com_Productos.Campo_Clave + "," +
            " PRODUCTOS." + Cat_Com_Productos.Campo_Costo + "," +
            " PRODUCTOS." + Cat_Com_Productos.Campo_Costo_Promedio + "," +
            " PRODUCTOS." + Cat_Com_Productos.Campo_Estatus + "," +
            " PRODUCTOS." + Cat_Com_Productos.Campo_Impuesto_ID + "," +
            " PRODUCTOS." + Cat_Com_Productos.Campo_Impuesto_2_ID + "," +
            //" GIROS." + Cat_Com_Giros.Campo_Nombre + "," +
            " PROD_SERV." + Ope_Com_Req_Producto.Campo_Nombre_Proveedor + "," +
            " PROD_SERV." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
            ") ORDER BY " +
            " PROD_SERV." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " ASC";
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            DataTable _DataTable = null;
            if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
            {
                _DataTable = _DataSet.Tables[0];
            }
            return _DataTable;            
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consolidar_Requisiciones_Servicios
        ///DESCRIPCIÓN: Obtiene un DataTable de una consulta con la consolidacin
        ///de lso servicios de las requisas
        ///PARAMETROS: 1.-Objeto de negocio con datos
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consolidar_Requisiciones_Servicios(Cls_Ope_Com_Consolidar_Requisicion_Negocio Negocio)
        {
            String Mi_Sql = "";
            Mi_Sql += "SELECT " +
            "PROD_SERV." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " AS ID, " +
            "SERVICIOS." + Cat_Com_Servicios.Campo_Clave + ", " +
            "SERVICIOS." + Cat_Com_Servicios.Campo_Nombre + ", " +
            "SUM(PROD_SERV." + Ope_Com_Req_Producto.Campo_Cantidad + ") AS CANTIDAD, " +
            //"SERVICIOS." + Cat_Com_Servicios.Campo_Giro_ID + ", " +
            //"GIROS." + Cat_Com_Giros.Campo_Nombre + " AS NOMBRE_GIRO, " +
            "'SERVICIO' AS TIPO, " +

            "PROD_SERV." + Ope_Com_Req_Producto.Campo_Nombre_Proveedor + ", " +

            //CALCULAMOS EL IMPORTE TOTAL DE LOS SERVICIOS YA CON IMPUESTOS
            //(SERVICIOS.COSTO+(SERVICIOS.COSTO * IMPUESTOS.PORCENTAJE_IMPUESTO / 100))*SUM(PROD_SERV.CANTIDAD)
                "(SERVICIOS." + Cat_Com_Servicios.Campo_Costo + "+(SERVICIOS." + Cat_Com_Servicios.Campo_Costo +
                " * IMPUESTOS." + Cat_Com_Impuestos.Campo_Porcentaje_Impuesto + " / 100)) * SUM(PROD_SERV." + Ope_Com_Req_Producto.Campo_Cantidad + ")" +
                " AS IMPORTE_TOTAL_CON_IMP, " +

                "(PROD_SERV." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado + "+(PROD_SERV." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                " * IMPUESTOS." + Cat_Com_Impuestos.Campo_Porcentaje_Impuesto + " / 100)) * SUM(PROD_SERV." + Ope_Com_Req_Producto.Campo_Cantidad + ")" +
                " AS IMPORTE_TOTAL_CON_IMP_COT, " +

            "SERVICIOS." + Cat_Com_Servicios.Campo_Costo + " AS COSTO_PROM_UNIDAD_SIN_IMP, " +
            "'ACTIVO' AS TIPO" +
            " FROM " +
            Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISA JOIN " +
            Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " PROD_SERV ON (REQUISA." +
            Ope_Com_Requisiciones.Campo_Requisicion_ID +
            "=PROD_SERV." + Ope_Com_Req_Producto.Campo_Requisicion_ID + ") JOIN " +
            Cat_Com_Servicios.Tabla_Cat_Com_Servicios + " SERVICIOS ON (PROD_SERV." +
            Ope_Com_Req_Producto.Campo_Prod_Serv_ID + "=SERVICIOS." +
            Cat_Com_Servicios.Campo_Servicio_ID + ") " + "JOIN " +
            //Cat_Com_Giros.Tabla_Cat_Com_Giros + " GIROS ON (SERVICIOS." +
            //Cat_Com_Servicios.Campo_Giro_ID + " = GIROS." +
            //Cat_Com_Giros.Campo_Giro_ID + ") JOIN " +
                //JOIN CAT_COM_IMPUESTOS IMPUESTOS ON (SERVICIOS.IMPUESTO_ID = IMPUESTOS.IMPUESTO_ID)
            Cat_Com_Impuestos.Tabla_Cat_Impuestos + " IMPUESTOS ON (SERVICIOS." +
            Cat_Com_Servicios.Campo_Impuesto_ID + " = IMPUESTOS." +
            Cat_Com_Impuestos.Campo_Impuesto_ID + ")" +
            " WHERE REQUISA." +
            Ope_Com_Requisiciones.Campo_Requisicion_ID + " IN (" + Negocio.P_Requisas_Seleccionadas + ")" +
            " GROUP BY (" +
            " PROD_SERV." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + "," +
            " SERVICIOS." + Cat_Com_Servicios.Campo_Nombre + "," +
            //" SERVICIOS." + Cat_Com_Servicios.Campo_Giro_ID + "," +
            " SERVICIOS." + Cat_Com_Servicios.Campo_Clave + "," +
            " SERVICIOS." + Cat_Com_Servicios.Campo_Costo + "," +
            " IMPUESTOS." + Cat_Com_Impuestos.Campo_Porcentaje_Impuesto + "," +
            " CANTIDAD," +
            //" GIROS." + Cat_Com_Giros.Campo_Nombre + "," +
            " PROD_SERV." + Ope_Com_Req_Producto.Campo_Nombre_Proveedor + "," +
            " PROD_SERV." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
            ") ORDER BY " +
            " PROD_SERV." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " ASC";
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            DataTable _DataTable = null;
            if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
            {
                _DataTable = _DataSet.Tables[0];
            }
            return _DataTable;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Productos_Requisas_Filtradas
        ///DESCRIPCIÓN: Obtiene un DataTable de una consulta con
        ///los productos de las requisas
        ///PARAMETROS: 1.-Objeto de negocio con datos
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Productos_Requisas_Filtradas(Cls_Ope_Com_Consolidar_Requisicion_Negocio Negocio)
        {
            //SELECT DISTINCT(PROD_SERV.PROD_SERV_ID) 
            //FROM OPE_COM_REQ_PRODUCTO PROD_SERV 
            //JOIN OPE_COM_REQUISICIONES REQUISA ON 
            //PROD_SERV.NO_REQUISICION = REQUISA.NO_REQUISICION 
            //WHERE REQUISA.ESTATUS IN('GENERADA','EN CONSTRUCCION','AUTORIZADA','FILTRADA','CONFIRMADA','SURTIDA') 
            //AND REQUISA.CONSOLIDADA='NO' 
            //AND REQUISA.TIPO_COMPRA IS NULL 
            //ORDER BY PROD_SERV.PROD_SERV_ID
            String Mi_Sql = "";
            Mi_Sql = "SELECT DISTINCT(PROD_SERV." + 
            Ope_Com_Req_Producto.Campo_Prod_Serv_ID +")" +
            " FROM " + 
            Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " PROD_SERV JOIN " + 
            Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISA ON PROD_SERV." + 
            Ope_Com_Req_Producto.Campo_Requisicion_ID + " = REQUISA." +
            Ope_Com_Requisiciones.Campo_Requisicion_ID + 
            " WHERE REQUISA." +
            Ope_Com_Requisiciones.Campo_Estatus + " IN('" + Negocio.P_Estatus + "') " + 
            "AND REQUISA." + Ope_Com_Requisiciones.Campo_Consolidada + " = 'NO' " +
            "AND REQUISA." + Ope_Com_Requisiciones.Campo_Tipo_Compra + " IS NULL" +
            " ORDER BY PROD_SERV." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID;
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            DataTable _DataTable = null;
            if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
            {
                _DataTable = _DataSet.Tables[0];
            }
            return _DataTable;
        }
        /////*******************************************************************************
        /////NOMBRE DE LA FUNCIÓN: Consultar_Servicios_Requisas_Filtradas
        /////DESCRIPCIÓN: Obtiene un DataTable de una consulta con
        /////los servicios de las requisas
        /////PARAMETROS: 1.-Objeto de negocio con datos
        /////CREO: Gustavo Angeles Cruz
        /////FECHA_CREO: 10/Enero/2011
        /////MODIFICO:
        /////FECHA_MODIFICO
        /////CAUSA_MODIFICACIÓN
        /////*******************************************************************************
        //public static DataTable Consultar_Servicios_Requisas_Filtradas(Cls_Ope_Com_Consolidar_Requisicion_Negocio Negocio)
        //{
        //    String Mi_Sql = "";
        //    Mi_Sql = "SELECT DISTINCT(PROD_SERV." +
        //    Ope_Com_Req_Producto.Campo_Servicio_ID + ")" +
        //    " FROM " +
        //    Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " PROD_SERV JOIN " +
        //    Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISA ON PROD_SERV." +
        //    Ope_Com_Req_Producto.Campo_Requisicion_ID + " = REQUISA." +
        //    Ope_Com_Requisiciones.Campo_Requisicion_ID +
        //    " WHERE REQUISA." +
        //    Ope_Com_Requisiciones.Campo_Estatus + " IN('" + Negocio.P_Estatus + "') AND PROD_SERV." +
        //    Ope_Com_Req_Producto.Campo_Servicio_ID + " <> '-'" +
        //    " AND REQUISA." + Ope_Com_Requisiciones.Campo_Consolidada + "='NO'" +
        //    " ORDER BY PROD_SERV." +
        //    Ope_Com_Req_Producto.Campo_Servicio_ID;
        //    DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
        //    DataTable _DataTable = null;
        //    if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
        //    {
        //        _DataTable = _DataSet.Tables[0];
        //    }
        //    return _DataTable;
        //}

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Requisas_Con_Productos
        ///DESCRIPCIÓN: Obtiene un DataTable de una consulta con
        ///cada producto de las requisas filtradas y su correspondiente numero de requisa
        ///PARAMETROS: 1.-Objeto de negocio con datos
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Obtener_Requisas_Con_Productos(Cls_Ope_Com_Consolidar_Requisicion_Negocio Negocio)
        {
            String Mi_Sql = "";
            //Mi_Sql = "SELECT PROD_SERV.NO_REQUISICION, PROD_SERV.PRODUCTO_ID FROM OPE_COM_REQ_PRODUCTO 
            //PROD_SERV JOIN OPE_COM_REQUISICIONES REQUISA ON PROD_SERV.NO_REQUISICION = REQUISA.NO_REQUISICION 
            //WHERE REQUISA.ESTATUS IN('GENERADA') AND 
            //PROD_SERV.PRODUCTO_ID <> '-' ORDER BY PROD_SERV.PRODUCTO_ID";
            Mi_Sql = "SELECT " +
            "PROD_SERV." + Ope_Com_Req_Producto.Campo_Requisicion_ID + "," +
            "PROD_SERV." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +
            " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " PROD_SERV JOIN " +
            Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISA ON PROD_SERV." +
            Ope_Com_Req_Producto.Campo_Requisicion_ID + " = REQUISA." +
            Ope_Com_Requisiciones.Campo_Requisicion_ID + " WHERE REQUISA." +
            Ope_Com_Requisiciones.Campo_Estatus + " IN ('" + Negocio.P_Estatus + "') ORDER BY PROD_SERV." +
            Ope_Com_Req_Producto.Campo_Prod_Serv_ID;            
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            DataTable _DataTable = null;
            if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
            {
                _DataTable = _DataSet.Tables[0];
            }
            return _DataTable;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Requisas_Con_Servicios
        ///DESCRIPCIÓN: Obtiene un DataTable de una consulta con
        ///cada servicio de las requisas filtradas y su correspondiente numero de requisa
        ///PARAMETROS: 1.-Objeto de negocio con datos
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Obtener_Requisas_Con_Servicios(Cls_Ope_Com_Consolidar_Requisicion_Negocio Negocio)
        {
            String Mi_Sql = "";
            //Mi_Sql = "SELECT PROD_SERV.NO_REQUISICION, PROD_SERV.SERVICIO_ID 
            //FROM OPE_COM_REQ_PRODUCTO PROD_SERV JOIN OPE_COM_REQUISICIONES REQUISA ON 
            //PROD_SERV.NO_REQUISICION = REQUISA.NO_REQUISICION WHERE REQUISA.ESTATUS IN('GENERADA') 
            //AND PROD_SERV.SERVICIO_ID <> '-' ORDER BY PROD_SERV.SERVICIO_ID";
            Mi_Sql = "SELECT " +
            "PROD_SERV." + Ope_Com_Req_Producto.Campo_Requisicion_ID + "," +
            "PROD_SERV." + Ope_Com_Req_Producto.Campo_Servicio_ID +
            " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " PROD_SERV JOIN " +
            Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISA ON PROD_SERV." +
            Ope_Com_Req_Producto.Campo_Requisicion_ID + " = REQUISA." +
            Ope_Com_Requisiciones.Campo_Requisicion_ID + " WHERE REQUISA." +
            Ope_Com_Requisiciones.Campo_Estatus + " IN ('" + Negocio.P_Estatus + "') AND PROD_SERV." +
            Ope_Com_Req_Producto.Campo_Requisicion_ID + " <> '-' ORDER BY PROD_SERV." +
            Ope_Com_Req_Producto.Campo_Servicio_ID;            
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            DataTable _DataTable = null;
            if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
            {
                _DataTable = _DataSet.Tables[0];
            }
            return _DataTable;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Consecutivo
        ///DESCRIPCIÓN: Obtiene el numero consecutivo para las tablas ocupadas en esta clase
        ///PARAMETROS: 1.-Campo del cual se obtendra el consecutivo
        ///            2.-Nombre de la tabla
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
            Mi_Sql = "SELECT NVL(MAX (" + Campo_ID + "),0) FROM " + Tabla;
            Obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            Consecutivo = (Convert.ToInt32(Obj) + 1);
            return Consecutivo;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Guardar_Consolidacion
        ///DESCRIPCIÓN: Hace la insercion de una consolidacion con sus detalles y 
        ///la actualizacion a las requisiciones marcandolas como consolidadas
        ///PARAMETROS: 1.-Objeto de Negocio
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Guardar_Consolidacion(Cls_Ope_Com_Consolidar_Requisicion_Negocio Negocio)
        {
            String Mi_Sql = "";            
            int No_Consolidacion = Obtener_Consecutivo(Ope_Com_Consolidaciones.Campo_No_Consolidacion, Ope_Com_Consolidaciones.Tabla_Ope_Com_Consolidaciones);
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos
            try
            {
                //Cadena de insercion para las consolidaciones
                Mi_Sql = "INSERT INTO " +
                Ope_Com_Consolidaciones.Tabla_Ope_Com_Consolidaciones + " (" +
                Ope_Com_Consolidaciones.Campo_No_Consolidacion + "," +
                Ope_Com_Consolidaciones.Campo_Folio + "," +                
                Ope_Com_Consolidaciones.Campo_Monto + "," +
                Ope_Com_Consolidaciones.Campo_Lista_Requisiciones + "," +
                Ope_Com_Consolidaciones.Campo_Estatus + "," +
                Ope_Com_Consolidaciones.Campo_Tipo + "," +
                Ope_Com_Consolidaciones.Campo_Usuario_Creo + "," +
                Ope_Com_Consolidaciones.Campo_Fecha_Creo + ") VALUES (" +
                No_Consolidacion + "," +
                "'CN-" + No_Consolidacion + "'," +
                Negocio.P_Monto + ",'" +
                Negocio.P_Requisas_Seleccionadas.Replace("'","") + "','" +
                "GENERADA','" +
                Negocio.P_Tipo_Articulo + "','" +
                Negocio.P_Usuario + "'," +
                "SYSDATE)";
                Comando_SQL.CommandText = Mi_Sql;
                Comando_SQL.ExecuteNonQuery();
                //Update de las requsiciones
                Mi_Sql = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " SET " +
                Ope_Com_Requisiciones.Campo_No_Consolidacion + " = " + No_Consolidacion + 
                " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " IN (" + Negocio.P_Requisas_Seleccionadas + ")";
                Comando_SQL.CommandText = Mi_Sql;
                Comando_SQL.ExecuteNonQuery();
                //Update de las requsiciones
                Mi_Sql = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " SET " +
                Ope_Com_Requisiciones.Campo_Consolidada + " = 'SI'" +
                " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " IN (" + Negocio.P_Requisas_Seleccionadas + ")";
                Comando_SQL.CommandText = Mi_Sql;
                Comando_SQL.ExecuteNonQuery();

                Mi_Sql = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " SET " +
                Ope_Com_Requisiciones.Campo_No_Consolidacion + " = " + No_Consolidacion +
                " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " IN (" + Negocio.P_Requisas_Seleccionadas + ")";
                Comando_SQL.CommandText = Mi_Sql;
                Comando_SQL.ExecuteNonQuery();
                Transaccion_SQL.Commit();
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Información: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Los datos fueron actualizados por otro Usuario. Información: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Información: " + Ex.Message);
            }
            finally
            {
                Conexion_Base.Close();
            } return 0;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Consolidaciones
        ///DESCRIPCIÓN: Hace la insercion de una consolidacion con sus detalles y 
        ///la actualizacion a las requisiciones marcandolas como consolidadas
        ///PARAMETROS: 1.-Objeto de Negocio
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Consolidaciones(Cls_Ope_Com_Consolidar_Requisicion_Negocio Negocio)
        {
            String Mi_Sql = "";

            if (Negocio.P_Folio != null && Negocio.P_Folio != "")
            {
                Mi_Sql = "SELECT *FROM " + Ope_Com_Consolidaciones.Tabla_Ope_Com_Consolidaciones +
                " WHERE " + Ope_Com_Consolidaciones.Campo_Folio + " = '" + Negocio.P_Folio + "'";
            }
            else
            {
                Mi_Sql = "SELECT *FROM " + Ope_Com_Consolidaciones.Tabla_Ope_Com_Consolidaciones +
                " WHERE " +
                "TO_DATE(TO_CHAR(" + Ope_Com_Consolidaciones.Campo_Fecha_Creo + ",'DD/MM/YY'))" +
                        " >= '" + Negocio.P_Fecha_Inicial + "' AND " +
                "TO_DATE(TO_CHAR(" + Ope_Com_Consolidaciones.Campo_Fecha_Creo + ",'DD/MM/YY'))" +
                        " <= '" + Negocio.P_Fecha_Final + "'" +
                " AND " + Ope_Com_Consolidaciones.Campo_Estatus + " = '" + Negocio.P_Estatus_Consolidacion + "'";
            }
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            DataTable _DataTable = null;
            if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
            {
                _DataTable = _DataSet.Tables[0];
            }
            return _DataTable;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Requisiciones_Consolidacion
        ///DESCRIPCIÓN: Consulta los detalles de la consolidacion
        ///usando el no_consolidacion 
        ///PARAMETROS: 1.-Objeto de Negocio
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Requisiciones_Consolidacion(Cls_Ope_Com_Consolidar_Requisicion_Negocio Negocio)
        {
            String Mi_Sql = "";
            Mi_Sql = "SELECT " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ".*" +
                ",(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + " = " + 
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID + ")" +
                " AS NOMBRE_DEPENDENCIA, 'CN' AS GRUPO" + 
            " FROM " + 
            Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
            " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " IN (" + Negocio.P_Requisas_Seleccionadas + ")";

            //Mi_Sql = "SELECT OPE_COM_REQUISICIONES.*," +
            //    "(SELECT NOMBRE FROM CAT_DEPENDENCIAS WHERE DEPENDENCIA_ID = OPE_COM_REQUISICIONES.DEPENDENCIA_ID) " +
            //    "AS NOMBRE_DEPENDENCIA, 'CN' AS GRUPO FROM OPE_COM_REQUISICIONES WHERE NO_REQUISICION IN (" + Negocio.P_Requisas_Seleccionadas + ")";

            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            DataTable _DataTable = null;
            if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
            {
                _DataTable = _DataSet.Tables[0];
            }
            return _DataTable;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Actualizar_Consolidacion
        ///DESCRIPCIÓN: Consulta los detalles de la consolidacion
        ///usando el no_consolidacion 
        ///PARAMETROS: 1.-Objeto de Negocio
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 5/Febrero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Actualizar_Consolidacion(Cls_Ope_Com_Consolidar_Requisicion_Negocio Negocio)
        {
            String Mi_Sql = "";
            int No_Consolidacion = Obtener_Consecutivo(Ope_Com_Consolidaciones.Campo_No_Consolidacion, Ope_Com_Consolidaciones.Tabla_Ope_Com_Consolidaciones);
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos
            try
            {
                //Update de las requsiciones
                Mi_Sql = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " SET " +
                Ope_Com_Requisiciones.Campo_No_Consolidacion + " = " + "null, "  +
                Ope_Com_Requisiciones.Campo_Consolidada + " = 'NO'" +
                " WHERE " + Ope_Com_Requisiciones.Campo_No_Consolidacion + " IN (" + Negocio.P_No_Consolidacion + ")";
                Comando_SQL.CommandText = Mi_Sql;
                Comando_SQL.ExecuteNonQuery();

                //Cadena de actualizacion para las consolidaciones
                Mi_Sql = "UPDATE " +
                Ope_Com_Consolidaciones.Tabla_Ope_Com_Consolidaciones +
                " SET " +
                Ope_Com_Consolidaciones.Campo_Monto + " = '" + Negocio.P_Monto + "'," +
                Ope_Com_Consolidaciones.Campo_Lista_Requisiciones + " = '" + Negocio.P_Requisas_Seleccionadas.Replace("'", "") + "'," +
                Ope_Com_Consolidaciones.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "'," +
                Ope_Com_Consolidaciones.Campo_Fecha_Modifico + " = SYSDATE" +
                " WHERE " + Ope_Com_Consolidaciones.Campo_No_Consolidacion + " = " + Negocio.P_No_Consolidacion;                
                Comando_SQL.CommandText = Mi_Sql;
                Comando_SQL.ExecuteNonQuery();

                Transaccion_SQL.Commit();
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Información: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Los datos fueron actualizados por otro Usuario. Información: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Información: " + Ex.Message);
            }
            finally
            {
                Conexion_Base.Close();
            } return 0;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Requisiciones_Por_Partida_Presupuestal
        ///DESCRIPCIÓN: Consulta los detalles de la consolidacion
        ///usando el no_consolidacion 
        ///PARAMETROS: 1.-Objeto de Negocio
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 22 Marzo 2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Requisiciones_Por_Partida_Presupuestal(Cls_Ope_Com_Consolidar_Requisicion_Negocio Negocio)
        {
            DataTable Dt_tabla = null;
            String Mi_SQL = "";
            Mi_SQL = "" +
            "SELECT DISTINCT (REQ.NO_REQUISICION),REQ.FOLIO,DET.PARTIDA_ID,PARTIDA.CLAVE ||' '|| PARTIDA.DESCRIPCION CLAVE_NOMBRE_PARTIDA, NVL(REQ.TOTAL,0) IMPORTE,REQ.ESTATUS,REQ.FECHA_CREO, " +
            "(SELECT CAT_DEPENDENCIAS.COMENTARIOS FROM CAT_DEPENDENCIAS WHERE DEPENDENCIA_ID = REQ.DEPENDENCIA_ID) UR, 0 AS GRUPO " +
            "FROM OPE_COM_REQUISICIONES REQ " +
            "JOIN OPE_COM_REQ_PRODUCTO DET " +
            "ON REQ.NO_REQUISICION = DET.NO_REQUISICION " +
            "JOIN CAT_DEPENDENCIAS UR ON " +
            "REQ.DEPENDENCIA_ID = UR.DEPENDENCIA_ID " +
            "JOIN cat_sap_partidas_especificas PARTIDA " +
            "ON det.partida_id = partida.partida_id " +
            "WHERE REQ.TIPO = 'TRANSITORIA' " +
            "AND REQ.ESTATUS = 'FILTRADA' " +
            "ORDER BY DET.PARTIDA_ID,REQ.NO_REQUISICION  ASC";
            try
            {
                Dt_tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch(Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Dt_tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Partidas_De_Requisiciones_Posibles_De_Consolidar
        ///DESCRIPCIÓN: Consulta los detalles de la consolidacion
        ///usando el no_consolidacion 
        ///PARAMETROS: 1.-Objeto de Negocio
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 22 Marzo 2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Partidas_De_Requisiciones_Posibles_De_Consolidar(Cls_Ope_Com_Consolidar_Requisicion_Negocio Negocio)
        {
            DataTable Dt_tabla = null;
            String Mi_SQL = "";
            Mi_SQL = "" +
            "SELECT DISTINCT (DET.PARTIDA_ID),PARTIDA.CLAVE ||' '|| PARTIDA.DESCRIPCION CLAVE_NOMBRE_PARTIDA, sum(req.total) IMPORTE " +
            "FROM OPE_COM_REQUISICIONES REQ " +
            "JOIN OPE_COM_REQ_PRODUCTO DET " +
            "ON REQ.NO_REQUISICION = DET.NO_REQUISICION " +
            "JOIN CAT_DEPENDENCIAS UR ON " +
            "REQ.DEPENDENCIA_ID = UR.DEPENDENCIA_ID " +
            "JOIN cat_sap_partidas_especificas PARTIDA " +
            "ON det.partida_id = partida.partida_id " +
            "WHERE REQ.TIPO = 'TRANSITORIA' " +
            "AND REQ.ESTATUS = 'FILTRADA' " +
            "GROUP BY(DET.PARTIDA_ID,PARTIDA.CLAVE,PARTIDA.DESCRIPCION) " +
            "ORDER BY det.partida_id  ASC ";
            try
            {
                Dt_tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch(Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Dt_tabla;
        }
        
        #endregion
    }
}

