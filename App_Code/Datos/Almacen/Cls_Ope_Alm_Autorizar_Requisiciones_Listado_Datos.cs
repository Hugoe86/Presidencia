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
using Presidencia.Autorizar_Req_Listado.Negocio;


/// <summary>
/// Summary description for Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Datos
/// </summary>
/// 
namespace Presidencia.Autorizar_Req_Listado.Datos
{
    public class Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Datos
    {
        #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Requisicion
        ///DESCRIPCIÓN: Metodo que ejecuta la sentencia SQL para modificar una requisicion
        ///PARAMETROS:   1.- Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static void Modificar_Requisicion(Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = "";

            //DE ACUERDO AL ESTATUS MODIFICAMOS LA REQUISICION
            switch (Requisicion_Negocio.P_Estatus.Trim())
            {
                case "AUTORIZADA":
                    if (Requisicion_Negocio.P_Tipo == "STOCK")
                    {
                        Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " SET " +
                                Ope_Com_Requisiciones.Campo_Estatus + " = 'ALMACEN', " +
                                Ope_Com_Requisiciones.Campo_Empleado_Autorizacion_ID + " = '" + Requisicion_Negocio.P_Empleado_ID + "', " +
                                Ope_Com_Requisiciones.Campo_Fecha_Autorizacion + " = SYSDATE" +
                                " WHERE " + Ope_Com_Requisiciones.Campo_Folio + "='" + Requisicion_Negocio.P_Folio + "'";
                    }
                    else
                    {
                        Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " SET " +
                                Ope_Com_Requisiciones.Campo_Estatus + " = '" + Requisicion_Negocio.P_Estatus + "', " +
                                Ope_Com_Requisiciones.Campo_Empleado_Autorizacion_ID + " = '" + Requisicion_Negocio.P_Empleado_ID + "', " +
                                Ope_Com_Requisiciones.Campo_Fecha_Autorizacion + " = SYSDATE" +
                                " WHERE " + Ope_Com_Requisiciones.Campo_Folio + "='" + Requisicion_Negocio.P_Folio + "'";
                    }
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    break;

                case "RECHAZADA":
                    Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " SET " +
                            Ope_Com_Requisiciones.Campo_Estatus + "= 'EN CONSTRUCCION', " +
                            Ope_Com_Requisiciones.Campo_Fecha_Rechazo + " =SYSDATE, " +
                            " ALERTA='AMARILLO', " +
                            Ope_Com_Requisiciones.Campo_Empleado_Rechazo_ID + "='" + Requisicion_Negocio.P_Empleado_ID + "'" +
                            " WHERE " + Ope_Com_Requisiciones.Campo_Folio + "='" + Requisicion_Negocio.P_Folio + "'";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    break;

                case "CANCELADA":
                    Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " SET " +
                            Ope_Com_Requisiciones.Campo_Estatus + "= '" + Requisicion_Negocio.P_Estatus + "', " +
                            Ope_Com_Requisiciones.Campo_Fecha_Cancelada + " =SYSDATE, " +
                            Ope_Com_Requisiciones.Campo_Empleado_Cancelada_ID + "='" + Requisicion_Negocio.P_Empleado_ID + "'" +
                            " WHERE " + Ope_Com_Requisiciones.Campo_Folio + "='" + Requisicion_Negocio.P_Folio + "'";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    //Primero se consultan los productos de esta requisicion
                    //rEALIZAMOS LA CONSULTA PARA OBTENER TODOS LOS PRODUCTOS DE LA REQUISICION CON SU PARTIDA CORRESPONDIENTE
                    Mi_SQL = "SELECT " + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
                             ", " + Ope_Com_Req_Producto.Campo_Monto_Total +
                             ", " + Ope_Com_Req_Producto.Campo_Partida_ID +
                             ", " + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID +
                             ", " + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID +
                             " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                             " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID + "='" + Requisicion_Negocio.P_Requisicion_ID + "'";
                    DataTable Dt_Partidas_Productos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                   
                    break;
                case "CONFIRMADA":
                    Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " SET " +
                         Ope_Com_Requisiciones.Campo_Estatus + "= '" + Requisicion_Negocio.P_Estatus + "', " +
                         Ope_Com_Requisiciones.Campo_Fecha_Confirmacion + " =SYSDATE, " +
                         Ope_Com_Requisiciones.Campo_Empleado_Confirmacion_ID + "='" + Requisicion_Negocio.P_Empleado_ID + "'" +
                         " WHERE " + Ope_Com_Requisiciones.Campo_Folio + "='" + Requisicion_Negocio.P_Folio + "'";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    break;
                case "COTIZADA-RECHAZADA":
                    Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " SET " +
                            Ope_Com_Requisiciones.Campo_Estatus + "= 'PROVEEDOR', " +
                            Ope_Com_Requisiciones.Campo_Alerta + "='AMARILLO2',"+
                            Ope_Com_Requisiciones.Campo_Fecha_Cotizada_Rechazada + " =SYSDATE, " +
                            Ope_Com_Requisiciones.Campo_Empleado_Cotizada_Rechazada_ID + "='" + Requisicion_Negocio.P_Empleado_ID + "'" +
                            " WHERE " + Ope_Com_Requisiciones.Campo_Folio + "='" + Requisicion_Negocio.P_Folio + "'";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                   
                    //Regresamos los valores de los productos pertenecientes a esta requisicion a nulos solo los cotizados y el proveedor para que sean cotizados nuevamente

                    Mi_SQL = "UPDATE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                         " SET " + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                         "=NULL" +
                         ", " + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
                         "=NULL" +
                         ", " + Ope_Com_Req_Producto.Campo_IVA_Cotizado +
                         "=NULL" +
                         ", " + Ope_Com_Req_Producto.Campo_IEPS_Cotizado +
                         "=NULL" +
                         ", " + Ope_Com_Req_Producto.Campo_Subtota_Cotizado +
                         "=NULL" +
                         ", " + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                         "=NULL" +
                         ", " + Ope_Com_Req_Producto.Campo_Proveedor_ID +
                         "=NULL" +
                         ", " + Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
                         "=NULL" +
                         " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                         "='" + Requisicion_Negocio.P_Requisicion_ID + "'";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    
                    //AHORA MODIFICAMOS LOS DETALLES DE LA REQUISICION, COMO LO ES EL MONTO COTIZADO PARA ESTA REQUISICION
                    Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                        " SET " + Ope_Com_Requisiciones.Campo_IVA_Cotizado +
                        "=NULL" +
                        ", " + Ope_Com_Requisiciones.Campo_IEPS_Cotizado +
                        "=NULL" +
                        ", " + Ope_Com_Requisiciones.Campo_Subtotal_Cotizado +
                        "=NULL" +
                        ", " + Ope_Com_Requisiciones.Campo_Total_Cotizado +
                        "=NULL" +
                        " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                        "='" + Requisicion_Negocio.P_Requisicion_ID + "'";

                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    //Modificamos todas las propuestas de Cotizacion a EN CONSTRUCCION para poder modificar los montos en caso de ser necesario
                    Mi_SQL = "UPDATE " + Ope_Com_Propuesta_Cotizacion.Tabla_Ope_Com_Propuesta_Cotizacion;
                    Mi_SQL = Mi_SQL + " SET " + Ope_Com_Propuesta_Cotizacion.Campo_Estatus;
                    Mi_SQL = Mi_SQL + "='EN CONSTRUCCION'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Resultado + "=NULL";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Propuesta_Cotizacion.Campo_No_Requisicion +"='" +Requisicion_Negocio.P_Requisicion_ID.ToString() + "'";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    break;

            }
            //Sentencia que ejecuta el query

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Requisiciones
        ///DESCRIPCIÓN: Metodo que ejecuta la sentencia SQL para consultar requisisciones
        ///PARAMETROS:  1.- Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************

        public static DataSet Consulta_Requisiciones(Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio Requisicion_Negocio)
        {
            //Consultar dependencias de usuario
            String Dependencias = "";
            DataTable Dt_URs = Cls_Util.Consultar_URs_De_Empleado(Cls_Sessiones.Empleado_ID);
            if (Dt_URs != null && Dt_URs.Rows.Count > 0)
            {
                foreach (DataRow Renglon in Dt_URs.Rows)
                {
                    Dependencias += Renglon["DEPENDENCIA_ID"].ToString() + ",";
                }
                Dependencias += "0";
            }

            String Mi_SQL = "SELECT " + Ope_Com_Requisiciones.Campo_Folio +
                            ", " + Ope_Com_Requisiciones.Campo_Tipo +
                            ", " + Ope_Com_Requisiciones.Campo_Estatus +
                            ", " + Ope_Com_Requisiciones.Campo_Tipo_Articulo +
                            ", TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Generacion + ",'DD/MON/YYYY') AS FECHA_GENERACION" +
                            ", " + Ope_Com_Requisiciones.Campo_Total +
                            " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                            " WHERE " + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                            " IN (" + Dependencias + ")";

            if (Requisicion_Negocio.P_Estatus_Busqueda != null)
            {

                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Campo_Estatus + "=" + "'" + Requisicion_Negocio.P_Estatus_Busqueda + "'";
            }
            else
            {
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Campo_Estatus + " IN ('COTIZADA')";
            }

            if (Requisicion_Negocio.P_Campo_Busqueda != null)
            {
                Mi_SQL = Mi_SQL + " AND UPPER(" + Ope_Com_Requisiciones.Campo_Folio + ") LIKE ('%" + Requisicion_Negocio.P_Campo_Busqueda + "%')";
            }

            if (Requisicion_Negocio.P_Fecha_Inicial != null)
            {
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Campo_Fecha_Generacion + " BETWEEN '" + Requisicion_Negocio.P_Fecha_Inicial + "'" +
                    " AND '" + Requisicion_Negocio.P_Fecha_Final + "'";
            }

            if (Requisicion_Negocio.P_Area_ID != null)
            {
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Campo_Area_ID + " ='" + Requisicion_Negocio.P_Area_ID + "'";
            }
            Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Campo_Listado_Almacen + "='SI'";
            Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " ASC";

            if (Requisicion_Negocio.P_Folio != null)
            {
                Mi_SQL = "SELECT " +
                         " DEPENDENCIA." + Cat_Dependencias.Campo_Nombre +
                         ", (SELECT NOMBRE FROM CAT_AREAS WHERE AREA_ID=REQUISICION.AREA_ID)AS AREA" +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_Tipo +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_Folio +
                         ", TO_CHAR( REQUISICION." + Ope_Com_Requisiciones.Campo_Fecha_Generacion + ",'DD/MON/YYYY') AS FECHA_GENERACION" +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_Estatus +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_Subtotal +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_IEPS +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_IVA +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_Total +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_Justificacion_Compra +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_Especificacion_Prod_Serv +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_Verificaion_Entrega +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_Tipo_Articulo +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_Total_Cotizado +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_Subtotal_Cotizado +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_IVA_Cotizado +
                         " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICION " +
                         " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIA " +
                         " ON REQUISICION." + Ope_Com_Requisiciones.Campo_Dependencia_ID + "= DEPENDENCIA." +
                         Cat_Dependencias.Campo_Dependencia_ID +
                         " WHERE " + Ope_Com_Requisiciones.Campo_Folio + "='" + Requisicion_Negocio.P_Folio + "'";
            }
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;

        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Productos
        ///DESCRIPCIÓN: Metodo que ejecuta la sentencia SQL para consultar los productos 
        ///PARAMETROS:  1.- Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************

        public static DataSet Consulta_Productos_Requisicion(Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = "SELECT " +
                   " PRODUCTOS." + Cat_Com_Productos.Campo_Clave +
                   ", PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + " AS PRODUCTO" +
                   ", PRODUCTOS." + Cat_Com_Productos.Campo_Descripcion +
                   ", REQUISICION_DET." + Ope_Com_Req_Producto.Campo_Cantidad + " AS CANTIDAD" +
                   ", PRODUCTOS." + Cat_Com_Productos.Campo_Costo + " AS PRECIO_UNITARIO" +
                   ", REQUISICION_DET." + Ope_Com_Req_Producto.Campo_Monto_Total + " AS IMPORTE_S_I" +
                   " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                   " REQUISICION_DET" +
                   " JOIN " + Cat_Com_Productos.Tabla_Cat_Com_Productos +
                   " PRODUCTOS ON PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID +
                   " = REQUISICION_DET." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +
                   " JOIN " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICION ON " +
                   "REQUISICION." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "= REQUISICION_DET." +
                   Ope_Com_Req_Producto.Campo_Requisicion_ID +
                   " WHERE REQUISICION." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "='" + Requisicion_Negocio.P_Requisicion_ID + "'" +
                   " UNION ALL " +
                   "SELECT " +
                   " SERVICIOS." + Cat_Com_Servicios.Campo_Nombre + " AS PRODUCTO" +
                   ", SERVICIOS." + Cat_Com_Servicios.Campo_Clave +
                   ", NULL AS DESCRIPCION" +
                   ", REQUISICION_DET." + Ope_Com_Req_Producto.Campo_Cantidad + " AS CANTIDAD" +
                   ", SERVICIOS." + Cat_Com_Servicios.Campo_Costo + " AS PRECIO_UNITARIO" +
                   ", REQUISICION_DET." + Ope_Com_Req_Producto.Campo_Monto_Total + " AS IMPORTE_S_I" +
                   " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                   " REQUISICION_DET" +
                   " JOIN " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios +
                   " SERVICIOS ON SERVICIOS." + Cat_Com_Servicios.Campo_Servicio_ID +
                   " = REQUISICION_DET." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +
                   " JOIN " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICION ON " +
                   "REQUISICION." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "= REQUISICION_DET." +
                   Ope_Com_Req_Producto.Campo_Requisicion_ID +
                   " WHERE REQUISICION." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "='" + Requisicion_Negocio.P_Requisicion_ID + "'";

            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Productos_Cotizados
        ///DESCRIPCIÓN: Metodo que ejecuta la sentencia SQL para consultar los productos que ya fueron consolidadas
        ///PARAMETROS:  1.- Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 25/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Productos_Cotizados(Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = "";

            switch (Requisicion_Negocio.P_Tipo_Articulo)
            {
                case "PRODUCTO":
                    Mi_SQL = "SELECT PRODUCTO." + Cat_Com_Productos.Campo_Nombre +
                            ", PRODUCTO." + Cat_Com_Productos.Campo_Clave +
                            ", PRODUCTO." + Cat_Com_Productos.Campo_Descripcion +
                            ", DET_REQ." + Ope_Com_Req_Producto.Campo_Cantidad +
                            ", DET_REQ." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                            ", DET_REQ." + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
                            ", DET_REQ." + Ope_Com_Req_Producto.Campo_Subtota_Cotizado +
                            ", DET_REQ." + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                            ", DET_REQ." + Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
                            " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " DET_REQ" +
                            " JOIN " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTO" +
                            " ON PRODUCTO." + Cat_Com_Productos.Campo_Producto_ID + " =" +
                            " DET_REQ." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +
                            " WHERE DET_REQ." + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                            "='" + Requisicion_Negocio.P_Requisicion_ID + "'";
                    break;
                case "SERVICIO":
                    Mi_SQL = "SELECT SERVICIO." + Cat_Com_Servicios.Campo_Nombre +
                            ", SERVICIO." + Cat_Com_Servicios.Campo_Clave +
                            ", NULL AS DESCRIPCION " +
                            ", DET_REQ." + Ope_Com_Req_Producto.Campo_Cantidad +
                            ", DET_REQ." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                            ", DET_REQ." + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
                            ", DET_REQ." + Ope_Com_Req_Producto.Campo_Subtota_Cotizado +
                            ", DET_REQ." + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                            ", DET_REQ." + Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
                            " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " DET_REQ" +
                            " JOIN " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + " SERVICIO" +
                            " ON SERVICIO." + Cat_Com_Servicios.Campo_Servicio_ID + " =" +
                            " DET_REQ." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +
                            " WHERE DET_REQ." + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                            "='" + Requisicion_Negocio.P_Requisicion_ID + "'";

                    break;

            }
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }






        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Requisicion_Consolidada
        ///DESCRIPCIÓN: Metodo que permite consultar si la requisicion esta consolidad y regresa un booleano
        ///PARAMETROS:  1.- Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 25/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static bool Consultar_Requisicion_Consolidada(Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = " SELECT " + Ope_Com_Requisiciones.Campo_No_Consolidacion +
                            " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                            " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                            " ='" + Requisicion_Negocio.P_Requisicion_ID + "'";
            DataTable Dt_Consolidacion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            bool Consolidada = false;
            if (Dt_Consolidacion.Rows.Count != 0)
            {
                if (Dt_Consolidacion.Rows[0][0] != null)
                    Consolidada = true;
                else
                    Consolidada = false;
            }
            return Consolidada;
        }

        #region Observaciones

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consecutivo
        ///DESCRIPCIÓN: Metodo que verfifica el consecutivo en la tabla y ayuda a generar el nuevo Id. 
        ///PARAMETROS: 
        ///CREO:
        ///FECHA_CREO: 24/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static String Consecutivo()
        {
            String Consecutivo = "";
            String Mi_SQL;         //Obtiene la cadena de inserción hacía la base de datos
            Object Asunto_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos

            Mi_SQL = "SELECT NVL(MAX (" + Ope_Com_Req_Observaciones.Campo_Observacion_ID + "),'00000') ";
            Mi_SQL = Mi_SQL + "FROM " + Ope_Com_Req_Observaciones.Tabla_Ope_Com_Req_Observaciones;
            Asunto_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Convert.IsDBNull(Asunto_ID))
            {
                Consecutivo = "00001";
            }
            else
            {
                Consecutivo = string.Format("{0:00000}", Convert.ToInt32(Asunto_ID) + 1);
            }
            return Consecutivo;
        }//fin de consecutivo

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Productos
        ///DESCRIPCIÓN: Metodo que ejecuta la sentencia SQL para consultar Observaciones 
        ///PARAMETROS:  1.- Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataSet Consulta_Observaciones(Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = "SELECT " + Ope_Com_Req_Observaciones.Campo_Observacion_ID +
                            ", " + Ope_Com_Req_Observaciones.Campo_Comentario +
                            ", " + Ope_Com_Req_Observaciones.Campo_Estatus +
                            ", TO_CHAR( " + Ope_Com_Req_Observaciones.Campo_Fecha_Creo + ",'DD/MON/YYYY') AS FECHA_CREO" +
                            ", " + Ope_Com_Req_Observaciones.Campo_Usuario_Creo +
                            " FROM " + Ope_Com_Req_Observaciones.Tabla_Ope_Com_Req_Observaciones +
                            " WHERE " + Ope_Com_Req_Observaciones.Campo_Requisicion_ID + " = '" + Requisicion_Negocio.P_Requisicion_ID + "'" +
                            " ORDER BY " + Ope_Com_Req_Observaciones.Campo_Observacion_ID + " DESC";
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Observaciones
        ///DESCRIPCIÓN: Metodo que ejecuta la sentencia SQL para dar de alta observaciones
        ///PARAMETROS:  1.- Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static void Alta_Observaciones(Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = "INSERT INTO " + Ope_Com_Req_Observaciones.Tabla_Ope_Com_Req_Observaciones +
            " (" + Ope_Com_Req_Observaciones.Campo_Observacion_ID +
            ", " + Ope_Com_Req_Observaciones.Campo_Requisicion_ID +
            ", " + Ope_Com_Req_Observaciones.Campo_Comentario +
            ", " + Ope_Com_Req_Observaciones.Campo_Estatus +
            ", " + Ope_Com_Req_Observaciones.Campo_Usuario_Creo +
            ", " + Ope_Com_Req_Observaciones.Campo_Fecha_Creo +
            ") VALUES (" +
            "SECUENCIA_OBSERVACION_REQ_ID.NEXTVAL,'" +
            Requisicion_Negocio.P_Requisicion_ID + "','" +
            Requisicion_Negocio.P_Comentario + "','" +
            Requisicion_Negocio.P_Estatus + "','" +
            Requisicion_Negocio.P_Usuario + "',SYSDATE)";
            //Sentencia que ejecuta el query
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }

        #endregion
        #endregion

    }
}