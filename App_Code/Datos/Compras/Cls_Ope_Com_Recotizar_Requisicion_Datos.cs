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
using Presidencia.Recotizar.Negocio;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Sessiones;
/// <summary>
/// Summary description for Cls_Ope_Com_Recotizar_Requisicion_Datos
/// </summary>
/// 
namespace Presidencia.Recotizar.Datos
{
    public class Cls_Ope_Com_Recotizar_Requisicion_Datos
    {
        public static DataTable Consultar_Requisiciones(Cls_Ope_Com_Recotizar_Requisicion_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";
            Mi_SQL = "SELECT " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones+"."+Ope_Com_Requisiciones.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + ", " +  Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones+"."+ Ope_Com_Requisiciones.Campo_Folio;
            Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Dependencias.Campo_Clave;
            Mi_SQL = Mi_SQL + "||' '||" + Cat_Dependencias.Campo_Nombre;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Dependencias.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + "=" + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + ") AS DEPENDENCIA";

            Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones+"."+Ope_Com_Requisiciones.Campo_Tipo_Articulo;
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones+"."+Ope_Com_Requisiciones.Campo_Estatus;
            Mi_SQL = Mi_SQL + ",TO_CHAR( " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Creo + ",'DD/MON/YYYY') AS " + Ope_Com_Requisiciones.Campo_Fecha_Creo;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Estatus;
            Mi_SQL = Mi_SQL + " IN ('PROVEEDOR','COTIZADA')";

            if (Clase_Negocio.P_Folio != null)
            {
                Mi_SQL = Mi_SQL + " AND UPPER(" + Ope_Com_Requisiciones.Campo_Folio + ") LIKE UPPER('%" +Clase_Negocio.P_Folio.Trim() +"%')";
            }

            Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " DESC";

            if (Clase_Negocio.P_No_Requisicion != null)
            {
                Mi_SQL = "SELECT ";
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
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICION ";
                Mi_SQL = Mi_SQL + " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIA ";
                Mi_SQL = Mi_SQL + " ON REQUISICION." + Ope_Com_Requisiciones.Campo_Dependencia_ID + "= DEPENDENCIA.";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + " WHERE REQUISICION." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
                Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_No_Requisicion.Trim() + "'";
            }


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
        public static DataTable Consultar_Productos_Servicios(Cls_Ope_Com_Recotizar_Requisicion_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";
            DataTable Dt_Productos = new DataTable();
            //Con sultamos primero el tipo de producto de la compra 
            Mi_SQL = Mi_SQL + "SELECT " + Ope_Com_Req_Producto.Campo_Tipo;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto;
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_No_Requisicion.Trim() + "'";
            Mi_SQL = Mi_SQL + " GROUP BY " + Ope_Com_Req_Producto.Campo_Tipo;
            DataTable Dt_Tipo_Producto = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];


            if (Dt_Tipo_Producto.Rows[0][Ope_Com_Req_Producto.Campo_Tipo].ToString().Trim() != String.Empty)
            {

                switch (Dt_Tipo_Producto.Rows[0][Ope_Com_Req_Producto.Campo_Tipo].ToString().Trim())
                {

                    case "PRODUCTO":
                        Mi_SQL =" SELECT DET_REQ." + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
                                ", PRODUCTO." + Cat_Com_Productos.Campo_Clave +
                                "||' '|| PRODUCTO." + Cat_Com_Productos.Campo_Nombre + " AS Nombre_Prod_Serv" +
                                ", PRODUCTO." + Cat_Com_Productos.Campo_Descripcion + 
                                ", (SELECT " + Cat_Com_Unidades.Campo_Abreviatura + 
                                " FROM " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + 
                                " WHERE " + Cat_Com_Unidades.Campo_Unidad_ID +"=PRODUCTO." + Cat_Com_Productos.Campo_Unidad_ID + ") AS UNIDAD" + 
                                ", DET_REQ." + Ope_Com_Req_Producto.Campo_Cantidad +
                                ", DET_REQ." + Ope_Com_Req_Producto.Campo_Precio_Unitario +
                                ", DET_REQ." + Ope_Com_Req_Producto.Campo_Monto_Total +
                                ", DET_REQ." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                                ", DET_REQ." + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                                " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " DET_REQ" +
                                " JOIN " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTO" +
                                " ON PRODUCTO." + Cat_Com_Productos.Campo_Producto_ID + " =" +
                                " DET_REQ." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +
                                " WHERE DET_REQ." + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                                "='" + Clase_Negocio.P_No_Requisicion.Trim() +"'";
                        break;
                    case "SERVICIO":
                        Mi_SQL = "SELECT DET_REQ." +  Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
                                ", SERVICIO." + Cat_Com_Servicios.Campo_Clave +
                                " ||' '|| SERVICIO." + Cat_Com_Servicios.Campo_Nombre + " AS Nombre_Prod_Serv" +
                                ", NULL AS DESCRIPCION " +
                                ", NULL AS UNIDAD " +
                                 ", DET_REQ." + Ope_Com_Req_Producto.Campo_Cantidad +
                                ", DET_REQ." + Ope_Com_Req_Producto.Campo_Precio_Unitario +
                                ", DET_REQ." + Ope_Com_Req_Producto.Campo_Monto_Total +
                                ", DET_REQ." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                                ", DET_REQ." + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                                " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " DET_REQ" +
                                " JOIN " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + " SERVICIO" +
                                " ON SERVICIO." + Cat_Com_Servicios.Campo_Servicio_ID + " =" +
                                " DET_REQ." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +
                                " WHERE DET_REQ." + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                                "='" + Clase_Negocio.P_No_Requisicion.Trim()+"'";

                        break;

                }
                Dt_Productos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }//Fin del IF
            return Dt_Productos;

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:  Modificar_Requisicion
        ///DESCRIPCIÓN: Metodo que Modifica el Estatus de la requisicion
        ///PARAMETROS: 1.- Cls_Ope_Com_Definir_Cotizadores_Negocio Clase_Negocios, objeto de la clase de negocios
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 31/Oct/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static String Modificar_Requisicion(Cls_Ope_Com_Recotizar_Requisicion_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";
            String Mensaje = "";

            try
            {
                Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
                Mi_SQL = Mi_SQL + " SET " + Ope_Com_Requisiciones.Campo_Estatus;
                Mi_SQL = Mi_SQL + "='FILTRADA'";
                Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Campo_Alerta;
                Mi_SQL = Mi_SQL + "='AMARILLO2'";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID;
                Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_No_Requisicion.Trim() +"'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                Mi_SQL = "UPDATE " + Ope_Com_Propuesta_Cotizacion.Tabla_Ope_Com_Propuesta_Cotizacion;
                Mi_SQL = Mi_SQL + " SET " + Ope_Com_Propuesta_Cotizacion.Campo_Estatus;
                Mi_SQL = Mi_SQL + "='EN CONSTRUCCION'";
                Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Resultado;
                Mi_SQL = Mi_SQL + "=NULL";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Propuesta_Cotizacion.Campo_No_Requisicion;
                Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_No_Requisicion.Trim() + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                
                //SI LA REQUISICION VIENE D EUNA DIVISION DE OTRA SE QUITA SU RELACION
                Mi_SQL = "UPDATE OPE_COM_REQUISICIONES SET REQ_ORIGEN_ID = NULL WHERE NO_REQUISICION = " + Clase_Negocio.P_No_Requisicion.Trim();
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                Mensaje = "La requisicion RQ-" + Clase_Negocio.P_No_Requisicion.Trim() + " fue modificada satisfactoriamente.";
            }
            catch (Exception EX)
            {
                Mensaje = EX.Message;
            }


            return Mensaje;

        }
       
    }
}