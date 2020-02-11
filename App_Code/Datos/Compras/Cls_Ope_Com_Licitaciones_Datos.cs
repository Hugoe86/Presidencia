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
using Presidencia.Licitaciones_Compras.Negocio;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Bitacora_Eventos;
using Presidencia.Sessiones;

/// <summary>
/// Summary description for Cls_Ope_Com_Licitaciones_Datos
/// </summary>

namespace Presidencia.Licitaciones_Compras.Datos
{
    public class Cls_Ope_Com_Licitaciones_Datos
    {

        #region Metodos


        public Cls_Ope_Com_Licitaciones_Datos()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        ///*******************************************************************************
        ///CONSULTAS
        ///*******************************************************************************
        #region Consultas


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:  Consultar_Licitaciones
        ///DESCRIPCIÓN: Consulta las licitaciones que estan EN CONSTRUCCION
        ///PARAMETROS: 
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataTable Consultar_Licitaciones(Cls_Ope_Com_Licitaciones_Negocio Licitacion_Datos)
        {
            String Mi_SQL = "SELECT " + Ope_Com_Licitaciones.Campo_No_Licitacion +
                     ", " + Ope_Com_Licitaciones.Campo_Folio +
                     ", " + Ope_Com_Licitaciones.Campo_Estatus +
                     ", TO_CHAR(" + Ope_Com_Licitaciones.Campo_Fecha_Inicio + ",'DD/MON/YYYY') AS " + Ope_Com_Licitaciones.Campo_Fecha_Inicio +
                     ", TO_CHAR(" + Ope_Com_Licitaciones.Campo_Fecha_Fin + ",'DD/MON/YYYY') AS " + Ope_Com_Licitaciones.Campo_Fecha_Fin +
                     " FROM " + Ope_Com_Licitaciones.Tabla_Ope_Com_Licitaciones;
            if (Licitacion_Datos.P_Estatus == null)
            {
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Licitaciones.Campo_Estatus +
                         " ='EN CONSTRUCCION'";
            }
            else
            {
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Licitaciones.Campo_Estatus + "='" + Licitacion_Datos.P_Estatus + "'";
            }

            if (Licitacion_Datos.P_Folio != null)
            {
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Licitaciones.Campo_Folio +
                         " ='" + Licitacion_Datos.P_Folio + "'";
            }

            if (Licitacion_Datos.P_No_Licitacion != null)
            {
                Mi_SQL = "SELECT " + Ope_Com_Licitaciones.Campo_Folio +
                        ", " + Ope_Com_Licitaciones.Campo_Estatus +
                        ", TO_CHAR(" + Ope_Com_Licitaciones.Campo_Fecha_Inicio + ",'DD/MON/YYYY') AS " + Ope_Com_Licitaciones.Campo_Fecha_Inicio +
                        ", TO_CHAR(" + Ope_Com_Licitaciones.Campo_Fecha_Fin + ",'DD/MON/YYYY') AS " + Ope_Com_Licitaciones.Campo_Fecha_Fin +
                        ", " + Ope_Com_Licitaciones.Campo_Justificacion +
                        ", " + Ope_Com_Licitaciones.Campo_Comentarios +
                        ", " + Ope_Com_Licitaciones.Campo_Monto_Total +
                        ", " + Ope_Com_Licitaciones.Campo_Tipo + 
                        ", " + Ope_Com_Licitaciones.Campo_Clasificacion +
                        " FROM " + Ope_Com_Licitaciones.Tabla_Ope_Com_Licitaciones +
                        " WHERE " + Ope_Com_Licitaciones.Campo_No_Licitacion +
                        "='" + Licitacion_Datos.P_No_Licitacion +"'";
            }
            if (Licitacion_Datos.P_Fecha_Inicio != null)
            {
                Mi_SQL = "SELECT " + Ope_Com_Licitaciones.Campo_No_Licitacion +
                     ", " + Ope_Com_Licitaciones.Campo_Folio +
                     ", " + Ope_Com_Licitaciones.Campo_Estatus +
                     ", TO_CHAR(" + Ope_Com_Licitaciones.Campo_Fecha_Inicio + ",'DD/MON/YYYY') AS " + Ope_Com_Licitaciones.Campo_Fecha_Inicio +
                     ", TO_CHAR(" + Ope_Com_Licitaciones.Campo_Fecha_Fin + ",'DD/MON/YYYY') AS " + Ope_Com_Licitaciones.Campo_Fecha_Fin +
                     " FROM " + Ope_Com_Licitaciones.Tabla_Ope_Com_Licitaciones +
                     " WHERE " + Ope_Com_Licitaciones.Campo_Fecha_Inicio +
                     " BETWEEN '" + Licitacion_Datos.P_Fecha_Inicio + 
                     "' AND '" + Licitacion_Datos.P_Fecha_Fin + "'" +
                     " AND " + Ope_Com_Licitaciones.Campo_Fecha_Fin +
                     " BETWEEN '" + Licitacion_Datos.P_Fecha_Inicio +
                     "' AND '" + Licitacion_Datos.P_Fecha_Fin + "'";
            }
            if (Licitacion_Datos.P_Folio_Busqueda != null)
            {
                Mi_SQL = "SELECT " + Ope_Com_Licitaciones.Campo_No_Licitacion +
                     ", " + Ope_Com_Licitaciones.Campo_Folio +
                     ", " + Ope_Com_Licitaciones.Campo_Estatus +
                     ", TO_CHAR(" + Ope_Com_Licitaciones.Campo_Fecha_Inicio + ",'DD/MON/YYYY') AS " + Ope_Com_Licitaciones.Campo_Fecha_Inicio +
                     ", TO_CHAR(" + Ope_Com_Licitaciones.Campo_Fecha_Fin + ",'DD/MON/YYYY') AS " + Ope_Com_Licitaciones.Campo_Fecha_Fin +
                     " FROM " + Ope_Com_Licitaciones.Tabla_Ope_Com_Licitaciones +
                     " WHERE UPPER(" + Ope_Com_Licitaciones.Campo_Folio + ") LIKE UPPER('%" +
                     Licitacion_Datos.P_Folio_Busqueda + "%')" +
                     " AND " +  Ope_Com_Licitaciones.Campo_Estatus  +
                     "='" + Licitacion_Datos.P_Estatus + "'";
            }            

            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:  Consultar_Licitacion_Detalle_Requisicion
        ///DESCRIPCIÓN: Consulta el listado de requisiciones que se agregaron a la licitacion
        ///PARAMETROS: Cls_Ope_Com_Licitaciones_Negocio Licitacion_Datos, Objeto de la clase de negocios.
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataTable Consultar_Licitacion_Detalle_Requisicion(Cls_Ope_Com_Licitaciones_Negocio Licitacion_Datos)
        {
            String Mi_SQL = "";
            if (Licitacion_Datos.P_No_Licitacion != null)
            {
                                
                Mi_SQL = "SELECT REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                         ", REQ." + Ope_Com_Requisiciones.Campo_Folio +
                         ", DEP." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA" +
                         ", AREA." + Cat_Areas.Campo_Nombre + " AS AREA" +
                         ",TO_CHAR(REQ." + Ope_Com_Requisiciones.Campo_Fecha_Filtrado + ",'DD/MON/YYYY') AS FECHA" +
                         ", REQ." + Ope_Com_Requisiciones.Campo_Total +
                         ", REQ." + Ope_Com_Requisiciones.Campo_No_Consolidacion +
                         " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQ" +
                         " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEP" +
                         " ON DEP." + Cat_Dependencias.Campo_Dependencia_ID + "=" +
                         " REQ." + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                         " JOIN " + Cat_Areas.Tabla_Cat_Areas + " AREA" +
                         " ON AREA." + Cat_Areas.Campo_Area_ID + "=" +
                         " REQ." + Ope_Com_Requisiciones.Campo_Area_ID +
                         " WHERE REQ." + Ope_Com_Requisiciones.Campo_No_Licitacion + "='" + Licitacion_Datos.P_No_Licitacion + "'" +
                         " AND REQ." + Ope_Com_Requisiciones.Campo_No_Consolidacion + " IS NULL " +
                         " GROUP BY (REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID + 
                         ", REQ." + Ope_Com_Requisiciones.Campo_Folio +
                         ", DEP." + Cat_Dependencias.Campo_Nombre +
                         ", AREA." + Cat_Areas.Campo_Nombre +
                         ", REQ." + Ope_Com_Requisiciones.Campo_Fecha_Filtrado +
                         ", REQ." + Ope_Com_Requisiciones.Campo_Total +
                         ", REQ." + Ope_Com_Requisiciones.Campo_No_Consolidacion + ")";
            }
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:  Consultar_Licitacion_Detalle_Consolidacion
        ///DESCRIPCIÓN: Consulta el listado de las consolidaciones que se agregaron a la licitacion
        ///PARAMETROS: Cls_Ope_Com_Licitaciones_Negocio Licitacion_Datos, Objeto de la clase de negocios.
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataTable Consultar_Licitacion_Detalle_Consolidacion(Cls_Ope_Com_Licitaciones_Negocio Licitacion_Datos)
        {
            String Mi_SQL = "";
            if (Licitacion_Datos.P_No_Licitacion != null)
            {
                Mi_SQL = "SELECT CON." + Ope_Com_Consolidaciones.Campo_No_Consolidacion +
                        ", CON." + Ope_Com_Consolidaciones.Campo_Folio +
                        ", CON." + Ope_Com_Consolidaciones.Campo_Estatus +
                        ", CON." + Ope_Com_Consolidaciones.Campo_Fecha_Creo +
                        ", CON." + Ope_Com_Consolidaciones.Campo_Monto +
                        ", CON." + Ope_Com_Consolidaciones.Campo_Lista_Requisiciones +
                        " FROM " + Ope_Com_Consolidaciones.Tabla_Ope_Com_Consolidaciones + " CON" +
                        " JOIN " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQ" +
                        " ON REQ." + Ope_Com_Requisiciones.Campo_No_Consolidacion +
                        " =CON." + Ope_Com_Consolidaciones.Campo_No_Consolidacion +
                        " WHERE REQ." + Ope_Com_Requisiciones.Campo_No_Licitacion +
                        " ='" + Licitacion_Datos.P_No_Licitacion + "'" +
                        " GROUP BY (CON." + Ope_Com_Consolidaciones.Campo_No_Consolidacion +
                        ", CON." + Ope_Com_Consolidaciones.Campo_Folio +
                        ", CON." + Ope_Com_Consolidaciones.Campo_Estatus +
                        ", CON." + Ope_Com_Consolidaciones.Campo_Fecha_Creo +
                        ", CON." + Ope_Com_Consolidaciones.Campo_Monto +
                        ", CON." + Ope_Com_Consolidaciones.Campo_Lista_Requisiciones + ")";
            }
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Requisiciones
        ///DESCRIPCIÓN: Consulta las requisiciones creadas y que cumplen el monto de la licitacion
        ///PARAMETROS: 
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataTable Consultar_Requisiciones(Cls_Ope_Com_Licitaciones_Negocio Licitacion_Datos)
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
                            " WHERE REQ." + Ope_Com_Requisiciones.Campo_Estatus +
                            " ='FILTRADA'";
            switch (Licitacion_Datos.P_Clasificacion)
            {
                case "RESTRINGIDA":
                    //Esto es en caso de dar de alta una licitacion RESTRINGIDA ya que su monto cambia
                    Mi_SQL = Mi_SQL +
                    " AND REQ." + Ope_Com_Requisiciones.Campo_Total +
                     ">(SELECT " + Cat_Com_Monto_Proceso_Compra.Campo_Monto_Licitacion_R_Ini +
                    " FROM " + Cat_Com_Monto_Proceso_Compra.Tabla_Cat_Com_Monto_Proceso_Compra +
                    " WHERE " + Cat_Com_Monto_Proceso_Compra.Campo_Tipo + "='" + Licitacion_Datos.P_Tipo + "')" +
                    " AND REQ." + Ope_Com_Requisiciones.Campo_Total +
                    "<=(SELECT " + Cat_Com_Monto_Proceso_Compra.Campo_Monto_Licitacion_R_Fin +
                    " FROM " + Cat_Com_Monto_Proceso_Compra.Tabla_Cat_Com_Monto_Proceso_Compra +
                    " WHERE " + Cat_Com_Monto_Proceso_Compra.Campo_Tipo + "='" + Licitacion_Datos.P_Tipo + "')";
                    break;
                case "PUBLICA":
                    //Esto es en caso de dar de alta una licitacion PUBLICA ya que su monto cambia
                    Mi_SQL = Mi_SQL +
                    " AND REQ." + Ope_Com_Requisiciones.Campo_Total +
                     ">(SELECT " + Cat_Com_Monto_Proceso_Compra.Campo_Monto_Licitacion_P_Ini +
                    " FROM " + Cat_Com_Monto_Proceso_Compra.Tabla_Cat_Com_Monto_Proceso_Compra +
                    " WHERE " + Cat_Com_Monto_Proceso_Compra.Campo_Tipo + "='" + Licitacion_Datos.P_Tipo + "')" +
                    " AND REQ." + Ope_Com_Requisiciones.Campo_Total +
                    "<=(SELECT " + Cat_Com_Monto_Proceso_Compra.Campo_Monto_Licitacion_P_Fin +
                    " FROM " + Cat_Com_Monto_Proceso_Compra.Tabla_Cat_Com_Monto_Proceso_Compra +
                    " WHERE " + Cat_Com_Monto_Proceso_Compra.Campo_Tipo + "='" + Licitacion_Datos.P_Tipo + "')";

                    break;
            }//Fin del switch
            Mi_SQL = Mi_SQL + " AND REQ." + Ope_Com_Requisiciones.Campo_Tipo_Compra + " IS NULL" +
            " AND REQ." + Ope_Com_Requisiciones.Campo_No_Consolidacion + " IS NULL" +
            " AND REQ." + Ope_Com_Requisiciones.Campo_Tipo_Articulo +
            " ='" + Licitacion_Datos.P_Tipo + "'" +
            " AND REQ." + Ope_Com_Requisiciones.Campo_Tipo + "='TRANSITORIA'" +
            " AND REQ." + Ope_Com_Requisiciones.Campo_Listado_Almacen +
            " IS NULL ";
            if (Licitacion_Datos.P_Requisicion_ID != null)
            {
                Mi_SQL = "SELECT REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
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
                            " WHERE REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                            " IN (" + Licitacion_Datos.P_Requisicion_ID + ")";
            }


            Mi_SQL = Mi_SQL + " GROUP BY (REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                           ", REQ." + Ope_Com_Requisiciones.Campo_Folio +
                           ", DEP." + Cat_Dependencias.Campo_Nombre +
                           ", AREA." + Cat_Areas.Campo_Nombre +
                           ", REQ." + Ope_Com_Requisiciones.Campo_Fecha_Filtrado +
                           ", REQ." + Ope_Com_Requisiciones.Campo_Total + ")" +
                           " ORDER BY REQ." + Ope_Com_Requisiciones.Campo_Fecha_Filtrado + " ASC";

            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Consolidaciones
        ///DESCRIPCIÓN: Consulta las Consolidaciones creadas y que cumplen el monto de la licitacion
        ///PARAMETROS: 
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************

        public DataTable Consulta_Consolidaciones(Cls_Ope_Com_Licitaciones_Negocio Licitacion_Datos)
        {
            String Mi_SQL = ""; 
            switch (Licitacion_Datos.P_Clasificacion)
            {
                case "RESTRINGIDA":
                    Mi_SQL = "SELECT " + Ope_Com_Consolidaciones.Campo_No_Consolidacion +
                                    ", " + Ope_Com_Consolidaciones.Campo_Folio +
                                    ", " + Ope_Com_Consolidaciones.Campo_Estatus +
                                    ", " + Ope_Com_Consolidaciones.Campo_Fecha_Creo +
                                    ", " + Ope_Com_Consolidaciones.Campo_Monto +
                                    " FROM " + Ope_Com_Consolidaciones.Tabla_Ope_Com_Consolidaciones +
                                    " WHERE " + Ope_Com_Consolidaciones.Campo_Estatus +
                                    "='GENERADA'" +
                                    " AND " + Ope_Com_Consolidaciones.Campo_Monto +
                                    ">(SELECT " + Cat_Com_Monto_Proceso_Compra.Campo_Monto_Licitacion_R_Ini +
                                    " FROM " + Cat_Com_Monto_Proceso_Compra.Tabla_Cat_Com_Monto_Proceso_Compra +
                                    " WHERE " + Cat_Com_Monto_Proceso_Compra.Campo_Tipo + "='" + Licitacion_Datos.P_Tipo + "')" +
                                    " AND " + Ope_Com_Consolidaciones.Campo_Monto +
                                    "<=(SELECT " + Cat_Com_Monto_Proceso_Compra.Campo_Monto_Licitacion_R_Fin +
                                    " FROM " + Cat_Com_Monto_Proceso_Compra.Tabla_Cat_Com_Monto_Proceso_Compra +
                                    " WHERE " + Cat_Com_Monto_Proceso_Compra.Campo_Tipo + "='" + Licitacion_Datos.P_Tipo + "')" +
                                    " AND " + Ope_Com_Consolidaciones.Campo_Tipo +
                                    "='" + Licitacion_Datos.P_Tipo + "'";
                   
                break;
                case "PUBLICA":
                Mi_SQL = "SELECT " + Ope_Com_Consolidaciones.Campo_No_Consolidacion +
                                ", " + Ope_Com_Consolidaciones.Campo_Folio +
                                ", " + Ope_Com_Consolidaciones.Campo_Estatus +
                                ", " + Ope_Com_Consolidaciones.Campo_Fecha_Creo +
                                ", " + Ope_Com_Consolidaciones.Campo_Monto +
                                " FROM " + Ope_Com_Consolidaciones.Tabla_Ope_Com_Consolidaciones +
                                " WHERE " + Ope_Com_Consolidaciones.Campo_Estatus +
                                "='GENERADA'" +
                                " AND " + Ope_Com_Consolidaciones.Campo_Monto +
                                ">(SELECT " + Cat_Com_Monto_Proceso_Compra.Campo_Monto_Licitacion_P_Ini +
                                " FROM " + Cat_Com_Monto_Proceso_Compra.Tabla_Cat_Com_Monto_Proceso_Compra +
                                " WHERE " + Cat_Com_Monto_Proceso_Compra.Campo_Tipo + "='" + Licitacion_Datos.P_Tipo + "')" +
                                " AND " + Ope_Com_Consolidaciones.Campo_Monto +
                                "<=(SELECT " + Cat_Com_Monto_Proceso_Compra.Campo_Monto_Licitacion_P_Fin +
                                " FROM " + Cat_Com_Monto_Proceso_Compra.Tabla_Cat_Com_Monto_Proceso_Compra +
                                " WHERE " + Cat_Com_Monto_Proceso_Compra.Campo_Tipo + "='" + Licitacion_Datos.P_Tipo + "')" +
                                " AND " + Ope_Com_Consolidaciones.Campo_Tipo +
                                "='" + Licitacion_Datos.P_Tipo + "'";
                break;
            }

            if (Licitacion_Datos.P_No_Consolidacion != null)
            {
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Consolidaciones.Campo_No_Consolidacion +
                        " ='" + Licitacion_Datos.P_No_Consolidacion + "'";
            }
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Producto_Requisicion
        ///DESCRIPCIÓN: Consulta los productos de una requisicion
        ///PARAMETROS:
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataTable Consultar_Producto_Requisicion(String No_Requisicion)
        {
            String Mi_SQL = "SELECT " + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                            ", " + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
                            ", " + Ope_Com_Req_Producto.Campo_Monto_Total +
                            " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                            " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                            "='" + No_Requisicion + "'";
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }

        #endregion Consultas

        ///*******************************************************************************
        ///ALTAS
        ///*******************************************************************************
        #region Altas
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Licitacion
        ///DESCRIPCIÓN: Metodo que da de alta las licitaciones en la tabla Ope_Com_Licitaciones
        ///
        ///PARAMETROS:  Cls_Ope_Com_Licitaciones_Negocio Licitacion_Datos= Objeto de la clase de Negocio 
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 21/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Alta_Licitacion(Cls_Ope_Com_Licitaciones_Negocio Licitacion_Datos)
        {
            //obtenemos el Id de la licitacion nueva 
            Licitacion_Datos.P_No_Licitacion = Obtener_Consecutivo(Ope_Com_Licitaciones.Campo_No_Licitacion, Ope_Com_Licitaciones.Tabla_Ope_Com_Licitaciones).ToString();

            String Mi_SQL = "INSERT INTO " + Ope_Com_Licitaciones.Tabla_Ope_Com_Licitaciones +
                            " (" + Ope_Com_Licitaciones.Campo_No_Licitacion +
                            ", " + Ope_Com_Licitaciones.Campo_Folio +
                            ", " + Ope_Com_Licitaciones.Campo_Estatus +
                            ", " + Ope_Com_Licitaciones.Campo_Fecha_Inicio +
                            ", " + Ope_Com_Licitaciones.Campo_Fecha_Fin +
                            ", " + Ope_Com_Licitaciones.Campo_Justificacion +
                            ", " + Ope_Com_Licitaciones.Campo_Comentarios +
                            ", " + Ope_Com_Licitaciones.Campo_Lista_Requisiciones +
                            ", " + Ope_Com_Licitaciones.Campo_Monto_Total +
                            ", " + Ope_Com_Licitaciones.Campo_Usuario_Creo +
                            ", " + Ope_Com_Licitaciones.Campo_Clasificacion + 
                            ", " + Ope_Com_Licitaciones.Campo_Tipo + 
                            ", " + Ope_Com_Licitaciones.Campo_Fecha_Creo +                            
                            ") VALUES ('" +
                            Licitacion_Datos.P_No_Licitacion + "','" +
                            "LC-"+Licitacion_Datos.P_No_Licitacion + "','" +
                            Licitacion_Datos.P_Estatus + "','" +
                            Licitacion_Datos.P_Fecha_Inicio+ "','" +
                            Licitacion_Datos.P_Fecha_Fin + "','" +
                            Licitacion_Datos.P_Justificacion + "','" +
                            Licitacion_Datos.P_Comentarios + "','" +
                            Licitacion_Datos.P_Lista_Requisiciones + "','" + 
                            Licitacion_Datos.P_Monto_Total + "','" +
                            Licitacion_Datos.P_Usuario + "','" +
                            Licitacion_Datos.P_Clasificacion + "','" +
                            Licitacion_Datos.P_Tipo + "'" +
                            ",SYSDATE)";
            
            //Sentencia que ejecuta el query
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
           // Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Com_Licitaciones.aspx", Licitacion_Datos.P_Folio, Mi_SQL);

            //Damos de Alta la relacion de la licitacion con las requisiciones seleccionadas
            //en caso de tener requisiciones agregadas al listado se dan de alta los detalles de licitacion
            if (Licitacion_Datos.P_Dt_Requisiciones != null)
            {
                //Damos de alta los detalles de cada una de la requisicion 
                Alta_Requisicion_Licitacion(Licitacion_Datos);
            }
            //Si se seleccionaron consolidaciones modificamos el estatus de las consolidaciones a OCUPADA
            if (Licitacion_Datos.P_Lista_Consolidaciones != null)
            {
                Mi_SQL = "UPDATE " + Ope_Com_Consolidaciones.Tabla_Ope_Com_Consolidaciones +
                         " SET " + Ope_Com_Consolidaciones.Campo_Estatus +
                         "='OCUPADA'" +
                         " WHERE " + Ope_Com_Consolidaciones.Campo_No_Consolidacion +
                         " IN (" + Licitacion_Datos.P_Lista_Consolidaciones + ")";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            
        }//fin de alta licitacion

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Requisicion_Licitacion
        ///DESCRIPCIÓN: Metodo que asigna a las requisiciones de la licitacion la referencia con esta  
        ///en el campo No_Licitacion de la tabla de Ope_Com_Requisiciones
        ///PARAMETROS:  Cls_Ope_Com_Licitaciones_Negocio Licitacion_Datos= Objeto de la clase de Negocio 
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 21/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Alta_Requisicion_Licitacion(Cls_Ope_Com_Licitaciones_Negocio Licitacion_Datos)
        {
            String Mi_SQL = "";
            //INSERTAMOS LA LICITACION A LA QUE PERTENCEN A LAS REQUISICIONES
            Mi_SQL = " UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                     " SET " + Ope_Com_Requisiciones.Campo_No_Licitacion +
                     "='" + Licitacion_Datos.P_No_Licitacion + "', " +
                     Ope_Com_Requisiciones.Campo_Tipo_Compra + " ='LICITACION'" +
                     " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                     " IN (" + Licitacion_Datos.P_Lista_Requisiciones +")";                       
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);            
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Licitacion
        ///DESCRIPCIÓN: Metodo que modifica los campos de una licitacion
        ///PARAMETROS:  Cls_Ope_Com_Licitaciones_Negocio Licitacion_Datos= Objeto de la clase de Negocio 
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 21/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Modificar_Licitacion(Cls_Ope_Com_Licitaciones_Negocio Licitacion_Datos)
        {
            String Mi_SQL = "UPDATE " + Ope_Com_Licitaciones.Tabla_Ope_Com_Licitaciones +
                            " SET " + Ope_Com_Licitaciones.Campo_Estatus + "='" +
                            Licitacion_Datos.P_Estatus +
                            "', " + Ope_Com_Licitaciones.Campo_Justificacion + "='" +
                            Licitacion_Datos.P_Justificacion +
                            "', " + Ope_Com_Licitaciones.Campo_Comentarios + "='" +
                            Licitacion_Datos.P_Comentarios +
                            "', " + Ope_Com_Licitaciones.Campo_Fecha_Inicio + "='" +
                            Licitacion_Datos.P_Fecha_Inicio +
                            "', " + Ope_Com_Licitaciones.Campo_Fecha_Fin + "='" +
                            Licitacion_Datos.P_Fecha_Fin +
                            "', " + Ope_Com_Licitaciones.Campo_Monto_Total + "='" +
                            Licitacion_Datos.P_Monto_Total +
                            "', " + Ope_Com_Licitaciones.Campo_Usuario_Modifico + "='" +
                            Licitacion_Datos.P_Usuario +
                            "', " + Ope_Com_Licitaciones.Campo_Fecha_Modifico + "= SYSDATE" +
                            " WHERE " + Ope_Com_Licitaciones.Campo_No_Licitacion + "='" +
                            Licitacion_Datos.P_No_Licitacion + "'";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);  
            //CONSULTAMOS LAS CONSOLIDACIONES QUE ESTABAN OCUPADAS ANTERIORMENTE CON ESTA LICITACION, PARA PASARLA A ESTATUS DE GENERADA
            DataTable Dt_Consolidaciones = Consultar_Licitacion_Detalle_Consolidacion(Licitacion_Datos);
            //hacemos un for para modificar el estatus de estas consolidaciones a estatus GENERADA, para posteriormente dar de alta las nuevas
            //consolidaciones con estatus de OCUPADA
            if (Dt_Consolidaciones.Rows.Count != 0)
            {
                for (int i = 0; i < Dt_Consolidaciones.Rows.Count; i++)
                {
                    Mi_SQL = "UPDATE " + Ope_Com_Consolidaciones.Tabla_Ope_Com_Consolidaciones +
                            " SET " + Ope_Com_Consolidaciones.Campo_Estatus +
                            "='GENERADA'" +
                            " WHERE " + Ope_Com_Consolidaciones.Campo_No_Consolidacion +
                            " ='" + Dt_Consolidaciones.Rows[i]["No_Consolidacion"] + "'";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                //Damos de Alta las consolidaciones en caso de e 
                if (Licitacion_Datos.P_Lista_Consolidaciones != null)
                {
                    Mi_SQL = "UPDATE " + Ope_Com_Consolidaciones.Tabla_Ope_Com_Consolidaciones +
                            " SET " + Ope_Com_Consolidaciones.Campo_Estatus +
                            "='OCUPADA'" +
                            " WHERE " + Ope_Com_Consolidaciones.Campo_No_Consolidacion +
                            " IN (" + Licitacion_Datos.P_Lista_Consolidaciones + ")";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
            }
            
            //MODIFICAMOS LAS REQUISICIONES INSERTADAS A ESTA LICITACION
            //Primero se pasan a nulas el campo de tipo_compra y el de No_Licitacion
                Mi_SQL = " UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                         " SET " + Ope_Com_Requisiciones.Campo_No_Licitacion + "= NULL, " +
                         Ope_Com_Requisiciones.Campo_Tipo_Compra + " = NULL" +
                         " WHERE " + Ope_Com_Requisiciones.Campo_No_Licitacion +
                         " ='" + Licitacion_Datos.P_No_Licitacion + "'";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL); 
  
            //Ahora damos de alta las requisiciones ya seleccionadas
            Alta_Requisicion_Licitacion(Licitacion_Datos);


        }


        #endregion Altas


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
            Mi_Sql = "SELECT NVL(MAX (" + Campo_ID + "),'00000') FROM " + Tabla;
            Obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            Consecutivo = (Convert.ToInt32(Obj) + 1);
            return Consecutivo;
        }

        #endregion
    }//fin del class
}