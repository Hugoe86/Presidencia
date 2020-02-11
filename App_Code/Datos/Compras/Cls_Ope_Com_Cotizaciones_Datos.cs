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
using Presidencia.Cotizaciones_Compras.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;

/// <summary>
/// Summary description for Cls_Ope_Com_Cotizaciones_Datos
/// </summary>
/// 
namespace Presidencia.Cotizaciones_Compras.Datos
{
    public class Cls_Ope_Com_Cotizaciones_Datos
    {
        public Cls_Ope_Com_Cotizaciones_Datos()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #region Consultas
        public DataTable Consultar_Cotizaciones(Cls_Ope_Com_Cotizaciones_Negocio Datos_Cotizacion)
        {
            String Mi_SQL = "SELECT " + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
                ", " + Ope_Com_Cotizaciones.Campo_Folio +
                ", " + Ope_Com_Cotizaciones.Campo_Tipo +
                ", " + Ope_Com_Cotizaciones.Campo_Estatus +
                ", TO_CHAR(" + Ope_Com_Cotizaciones.Campo_Fecha_Creo + ",'DD/MON/YYYY') AS FECHA" +
                ", " + Ope_Com_Cotizaciones.Campo_Total +
                " FROM " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones +
                " WHERE " + Ope_Com_Cotizaciones.Campo_Estatus +
                " ='EN CONSTRUCCION'" +
                " ORDER BY " + Ope_Com_Cotizaciones.Campo_Fecha_Creo;
            if (Datos_Cotizacion.P_No_Cotizacion != null)
            {
                Mi_SQL = "SELECT " + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
                    ", " + Ope_Com_Cotizaciones.Campo_Folio +
                    ", " + Ope_Com_Cotizaciones.Campo_Tipo +
                    ", " + Ope_Com_Cotizaciones.Campo_Estatus +
                    ", " + Ope_Com_Cotizaciones.Campo_Condiciones +
                    ", TO_CHAR(" + Ope_Com_Cotizaciones.Campo_Fecha_Creo + ",'DD/MON/YYYY') AS FECHA" +
                    ", " + Ope_Com_Cotizaciones.Campo_Total +
                    ", " + Ope_Com_Cotizaciones.Campo_Listado_Almacen + 
                    " FROM " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones +
                    " WHERE " + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
                    "='"+Datos_Cotizacion.P_No_Cotizacion +"'";
            }//fin del IF
            if (Datos_Cotizacion.P_Folio != null)
            {
                Mi_SQL = "SELECT " + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
                ", " + Ope_Com_Cotizaciones.Campo_Folio +
                ", " + Ope_Com_Cotizaciones.Campo_Tipo +
                ", " + Ope_Com_Cotizaciones.Campo_Estatus +
                ", TO_CHAR(" + Ope_Com_Cotizaciones.Campo_Fecha_Creo + ",'DD/MON/YYYY') AS FECHA" +
                ", " + Ope_Com_Cotizaciones.Campo_Total +
                " FROM " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones +
                " WHERE " + Ope_Com_Cotizaciones.Campo_Estatus +
                " ='EN CONSTRUCCION'" +
                " AND " + Ope_Com_Cotizaciones.Campo_Folio +
                " LIKE '%"+Datos_Cotizacion.P_Folio +"%'" +
                " ORDER BY " + Ope_Com_Cotizaciones.Campo_Folio;
            }

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
        public DataTable Consultar_Requisiciones(Cls_Ope_Com_Cotizaciones_Negocio Datos_Cotizacion)
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
                            " ='FILTRADA'" +
                            " AND " + Ope_Com_Requisiciones.Campo_Tipo + " ='TRANSITORIA'" +
                            " AND REQ." + Ope_Com_Requisiciones.Campo_Total +
                             ">(SELECT " + Cat_Com_Monto_Proceso_Compra.Campo_Monto_Cotizacion_Ini +
                            " FROM " + Cat_Com_Monto_Proceso_Compra.Tabla_Cat_Com_Monto_Proceso_Compra +
                            " WHERE " + Cat_Com_Monto_Proceso_Compra.Campo_Tipo + "='" + Datos_Cotizacion.P_Tipo + "')" +
                            " AND REQ." + Ope_Com_Requisiciones.Campo_Total +
                            "<=(SELECT " + Cat_Com_Monto_Proceso_Compra.Campo_Monto_Cotizacion_Fin +
                            " FROM " + Cat_Com_Monto_Proceso_Compra.Tabla_Cat_Com_Monto_Proceso_Compra +
                            " WHERE " + Cat_Com_Monto_Proceso_Compra.Campo_Tipo + "='" + Datos_Cotizacion.P_Tipo + "')" +
                            " AND REQ." + Ope_Com_Requisiciones.Campo_Tipo_Compra + " IS NULL" +
                            " AND REQ." + Ope_Com_Requisiciones.Campo_No_Consolidacion + " IS NULL" +
                            " AND REQ." + Ope_Com_Requisiciones.Campo_Tipo_Articulo +
                            " ='" + Datos_Cotizacion.P_Tipo + "'";
            if (Datos_Cotizacion.P_Listado_Almacen == "SI")
            {
                Mi_SQL = Mi_SQL + " AND REQ." + Ope_Com_Requisiciones.Campo_Listado_Almacen +
                "='" + Datos_Cotizacion.P_Listado_Almacen + "'";
            }
            if (Datos_Cotizacion.P_No_Requisicion != null)
            {
                Mi_SQL = "SELECT REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                            ", REQ." + Ope_Com_Requisiciones.Campo_Folio +
                            ", DEP." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA" +
                            ", AREA." + Cat_Areas.Campo_Nombre + " AS AREA" +
                            ",TO_CHAR(REQ." + Ope_Com_Requisiciones.Campo_Fecha_Filtrado + ",'DD/MON/YYYY') AS FECHA" +
                            ", REQ." + Ope_Com_Requisiciones.Campo_Total +
                            " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQ" +
                            " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEP" +
                            " ON DEP." + Cat_Dependencias.Campo_Dependencia_ID + "=" +
                            " REQ." + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                            " JOIN " + Cat_Areas.Tabla_Cat_Areas + " AREA" +
                            " ON AREA." + Cat_Areas.Campo_Area_ID + "=" +
                            " REQ." + Ope_Com_Requisiciones.Campo_Area_ID +
                            " WHERE REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                            " IN (" + Datos_Cotizacion.P_No_Requisicion + ")";
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

        public DataTable Consulta_Consolidaciones(Cls_Ope_Com_Cotizaciones_Negocio Datos_Cotizacion)
        {
            String Mi_SQL = "SELECT " + Ope_Com_Consolidaciones.Campo_No_Consolidacion +
                            ", " + Ope_Com_Consolidaciones.Campo_Folio +
                            ", " + Ope_Com_Consolidaciones.Campo_Estatus +
                            ", TO_CHAR(" + Ope_Com_Consolidaciones.Campo_Fecha_Creo + ",'DD/MON/YYYY') AS FECHA" +
                            ", " + Ope_Com_Consolidaciones.Campo_Monto + " AS TOTAL" +
                            " FROM " + Ope_Com_Consolidaciones.Tabla_Ope_Com_Consolidaciones +
                            " WHERE " + Ope_Com_Consolidaciones.Campo_Estatus +
                            "='GENERADA'" +
                            " AND " + Ope_Com_Consolidaciones.Campo_Monto +
                            ">(SELECT " + Cat_Com_Monto_Proceso_Compra.Campo_Monto_Cotizacion_Ini +
                            " FROM " + Cat_Com_Monto_Proceso_Compra.Tabla_Cat_Com_Monto_Proceso_Compra +
                            " WHERE " + Cat_Com_Monto_Proceso_Compra.Campo_Tipo + "='" + Datos_Cotizacion.P_Tipo + "')" +
                            " AND " + Ope_Com_Consolidaciones.Campo_Monto +
                            "<=(SELECT " + Cat_Com_Monto_Proceso_Compra.Campo_Monto_Licitacion_P_Fin +
                            " FROM " + Cat_Com_Monto_Proceso_Compra.Tabla_Cat_Com_Monto_Proceso_Compra +
                            " WHERE " + Cat_Com_Monto_Proceso_Compra.Campo_Tipo + "='" + Datos_Cotizacion.P_Tipo + "')" +
                            " AND " + Ope_Com_Consolidaciones.Campo_Tipo +
                            "='" + Datos_Cotizacion.P_Tipo + "'";
                            

            if (Datos_Cotizacion.P_No_Consolidacion != null)
            {
                Mi_SQL = "SELECT " + Ope_Com_Consolidaciones.Campo_No_Consolidacion +
                            ", " + Ope_Com_Consolidaciones.Campo_Folio +
                            ", " + Ope_Com_Consolidaciones.Campo_Estatus +
                            ", TO_CHAR(" + Ope_Com_Consolidaciones.Campo_Fecha_Creo + ",'DD/MON/YYYY') AS FECHA" +
                            ", " + Ope_Com_Consolidaciones.Campo_Monto + " AS TOTAL" +
                            ", " + Ope_Com_Consolidaciones.Campo_Lista_Requisiciones +
                            " FROM " + Ope_Com_Consolidaciones.Tabla_Ope_Com_Consolidaciones +
                            " WHERE " + Ope_Com_Consolidaciones.Campo_No_Consolidacion +
                            " ='" + Datos_Cotizacion.P_No_Consolidacion + "'";
            }

            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;

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

        public DataTable Consultar_Comite_Detalle_Requisicion(Cls_Ope_Com_Cotizaciones_Negocio Datos_Cotizacion)
        {
            String Mi_SQL = "";
            if (Datos_Cotizacion.P_No_Cotizacion != null)
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
                         " WHERE REQ." + Ope_Com_Requisiciones.Campo_No_Cotizacion + "='" + Datos_Cotizacion.P_No_Cotizacion.Trim() + "'" +
                         " AND REQ." + Ope_Com_Requisiciones.Campo_No_Consolidacion + " IS NULL " +
                         " GROUP BY (REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                         ", REQ." + Ope_Com_Requisiciones.Campo_Folio +
                         ", DEP." + Cat_Dependencias.Campo_Nombre +
                         ", AREA." + Cat_Areas.Campo_Nombre +
                         ", REQ." + Ope_Com_Requisiciones.Campo_Fecha_Filtrado +
                         ", REQ." + Ope_Com_Requisiciones.Campo_Total + ")";
            }
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
        public DataTable Consultar_Detalle_Consolidacion(Cls_Ope_Com_Cotizaciones_Negocio Datos_Cotizacion)
        {
            String Mi_SQL = "";
            if (Datos_Cotizacion.P_No_Cotizacion != null)
            {
                Mi_SQL = "SELECT CON." + Ope_Com_Consolidaciones.Campo_No_Consolidacion +
                        ", CON." + Ope_Com_Consolidaciones.Campo_Folio +
                        ", CON." + Ope_Com_Consolidaciones.Campo_Estatus +
                        ", CON." + Ope_Com_Consolidaciones.Campo_Fecha_Creo + " AS FECHA" +
                        ", CON." + Ope_Com_Consolidaciones.Campo_Monto + " AS TOTAL" +
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
            }
           
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;

        }

        #endregion

        #region Altas

        public void Alta_Cotizacion(Cls_Ope_Com_Cotizaciones_Negocio Datos_Cotizacion)
        {
            Datos_Cotizacion.P_No_Cotizacion = Obtener_Consecutivo(Ope_Com_Cotizaciones.Campo_No_Cotizacion, Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones).ToString();

            String Mi_SQL = "INSERT INTO " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones +
                            " (" + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
                            ", " + Ope_Com_Cotizaciones.Campo_Folio +
                            ", " + Ope_Com_Cotizaciones.Campo_Estatus +
                            ", " + Ope_Com_Cotizaciones.Campo_Tipo +
                            ", " + Ope_Com_Cotizaciones.Campo_Condiciones +
                            ", " + Ope_Com_Cotizaciones.Campo_Total +
                            ", " + Ope_Com_Cotizaciones.Campo_Fecha_Creo +
                            ", " + Ope_Com_Cotizaciones.Campo_Usuario_Creo +
                            ", " + Ope_Com_Cotizaciones.Campo_Lista_Requisiciones +
                            ", " + Ope_Com_Cotizaciones.Campo_Listado_Almacen +
                            ") VALUES('" + Datos_Cotizacion.P_No_Cotizacion +
                            "','" + "COT-" + Datos_Cotizacion.P_No_Cotizacion +
                            "','" + Datos_Cotizacion.P_Estatus +
                            "','" + Datos_Cotizacion.P_Tipo +
                            "','" + Datos_Cotizacion.P_Condiciones +
                            "','" + Datos_Cotizacion.P_Monto_Total +
                            "', SYSDATE" +
                            ",'" + Datos_Cotizacion.P_Usuario +
                            "','" + Datos_Cotizacion.P_Lista_Requisiciones +
                            "','" + Datos_Cotizacion.P_Listado_Almacen + "')";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            //en caso de existir requisiciones para el 
            if (Datos_Cotizacion.P_Lista_Requisiciones != null)
            {
                Alta_Requisiciones_Cotizacion(Datos_Cotizacion);
            }
            //Si se seleccionaron consolidaciones modificamos el estatus de las consolidaciones a OCUPADA
            if (Datos_Cotizacion.P_Lista_Consolidaciones != null)
            {
                Mi_SQL = "UPDATE " + Ope_Com_Consolidaciones.Tabla_Ope_Com_Consolidaciones +
                         " SET " + Ope_Com_Consolidaciones.Campo_Estatus +
                         "='OCUPADA'" +
                         " WHERE " + Ope_Com_Consolidaciones.Campo_No_Consolidacion +
                         " IN (" + Datos_Cotizacion.P_Lista_Consolidaciones + ")";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }

        }
        public void Alta_Requisiciones_Cotizacion(Cls_Ope_Com_Cotizaciones_Negocio Datos_Cotizacion)
        {
            String Mi_SQL = "";
            //INSERTAMOS LA LICITACION A LA QUE PERTENCEN A LAS REQUISICIONES
            Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                     " SET " + Ope_Com_Requisiciones.Campo_No_Cotizacion +
                     "='" + Datos_Cotizacion.P_No_Cotizacion + "', " +
                     Ope_Com_Requisiciones.Campo_Tipo_Compra + " ='COTIZACION'" +
                     " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                     " IN (" + Datos_Cotizacion.P_Lista_Requisiciones + ")";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }

        public void Modificar_Cotizacion(Cls_Ope_Com_Cotizaciones_Negocio Datos_Cotizacion)
        {
            String Mi_SQL = "UPDATE " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones +
                           " SET " + Ope_Com_Cotizaciones.Campo_Estatus +
                           "='" + Datos_Cotizacion.P_Estatus +
                           "', " + Ope_Com_Cotizaciones.Campo_Condiciones +
                           "='" + Datos_Cotizacion.P_Condiciones +
                           "', " + Ope_Com_Cotizaciones.Campo_Total +
                           "='" + Datos_Cotizacion.P_Monto_Total +
                           "', " + Ope_Com_Cotizaciones.Campo_Fecha_Modifico +
                           "=SYSDATE" +
                           ", " + Ope_Com_Cotizaciones.Campo_Usuario_Modifico +
                           "='" + Datos_Cotizacion.P_Usuario +
                           "', " + Ope_Com_Cotizaciones.Campo_Lista_Requisiciones +
                           "='" + Datos_Cotizacion.P_Lista_Requisiciones + "'" +
                           " WHERE " + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
                           "='" + Datos_Cotizacion.P_No_Cotizacion + "'";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            //Obtenemos los datos de las consolidaciones pertenecientes al proceso de Cotizacion
            DataTable Dt_Consolidaciones = Consultar_Detalle_Consolidacion(Datos_Cotizacion);
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

            }//fin del if
            //Damos de Alta las consolidaciones en caso de existir y se pone su estatus como ocupada
            if (Datos_Cotizacion.P_Lista_Consolidaciones.Trim() != "")
            {
                Mi_SQL = "UPDATE " + Ope_Com_Consolidaciones.Tabla_Ope_Com_Consolidaciones +
                        " SET " + Ope_Com_Consolidaciones.Campo_Estatus +
                        "='OCUPADA'" +
                        " WHERE " + Ope_Com_Consolidaciones.Campo_No_Consolidacion +
                        " IN (" + Datos_Cotizacion.P_Lista_Consolidaciones + ")";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            //MODIFICAMOS LAS REQUISICIONES INSERTADAS A ESTA LICITACION
            //Primero se pasan a nulas el campo de tipo_compra y el de No_Licitacion
            Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                     " SET " + Ope_Com_Requisiciones.Campo_No_Cotizacion + "= NULL, " +
                     Ope_Com_Requisiciones.Campo_Tipo_Compra + " = NULL" +
                     " WHERE " + Ope_Com_Requisiciones.Campo_No_Cotizacion +
                     " ='" + Datos_Cotizacion.P_No_Cotizacion + "'";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            //Ahora damos de alta las requisiciones ya seleccionadas
            if (Datos_Cotizacion.P_Lista_Requisiciones.Trim() != "")
            {
                Alta_Requisiciones_Cotizacion(Datos_Cotizacion);
            }
        }


        #endregion 

        #region Metodos de Apoyo
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

    }
}//fin de namespace