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
using Presidencia.Cancelar_Ordenes.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Generar_Requisicion.Datos;
using Presidencia.Administrar_Requisiciones.Negocios;

/// <summary>
/// Summary description for Cls_Ope_Com_Cancelar_Ordenes_Compra_Datos
/// </summary>

namespace Presidencia.Cancelar_Ordenes.Datos
{
    public class Cls_Ope_Com_Cancelar_Ordenes_Compra_Datos
    {

        public static DataTable Consultar_Ordenes_Compra(Cls_Ope_Com_Cancelar_Ordenes_Compra_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";

            Mi_SQL = "SELECT ORDEN." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;
            Mi_SQL = Mi_SQL + ", ORDEN." + Ope_Com_Ordenes_Compra.Campo_Folio;
            Mi_SQL = Mi_SQL + ", ORDEN." + Ope_Com_Ordenes_Compra.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", ORDEN." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo;
            Mi_SQL = Mi_SQL + ", ORDEN." + Ope_Com_Ordenes_Compra.Campo_Total;
            Mi_SQL = Mi_SQL + ", ORDEN." + Ope_Com_Ordenes_Compra.Campo_Tipo_Articulo;
            Mi_SQL = Mi_SQL + ", ORDEN." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones;

            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDEN ";
            Mi_SQL = Mi_SQL + " WHERE ORDEN." + Ope_Com_Ordenes_Compra.Campo_Folio+ "  IS NOT NULL ";
           
                
            if (Clase_Negocio.P_Estatus != null)
            {
                Mi_SQL = Mi_SQL + " AND ORDEN." + Ope_Com_Ordenes_Compra.Campo_Estatus;
                Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Estatus.Trim() + "'";
            }
            else
            {
                Mi_SQL = Mi_SQL + " AND ORDEN." + Ope_Com_Ordenes_Compra.Campo_Estatus;
                Mi_SQL = Mi_SQL + " IN ('RECHAZADA','GENERADA','AUTORIZADA','CANCELACION PARCIAL','CANCELACION TOTAL')";
                 
            }

            if (Clase_Negocio.P_Folio_Busqueda != null)
            {
                Mi_SQL = Mi_SQL + " AND UPPER(ORDEN." + Ope_Com_Ordenes_Compra.Campo_Folio;
                Mi_SQL = Mi_SQL + ") LIKE ('%" +  Clase_Negocio.P_Folio_Busqueda.Trim() +"%')";

            }

            if (Clase_Negocio.P_No_Requisicio != null)
            {
                Mi_SQL = Mi_SQL + " AND ORDEN." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones;
                Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_No_Requisicio +"' ";

            }

            if (Clase_Negocio.P_Fecha_Inicio != null)
            {
                Mi_SQL = Mi_SQL + " AND ORDEN." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo;
                Mi_SQL = Mi_SQL + " BETWEEN '" + Clase_Negocio.P_Fecha_Inicio + "'";
                Mi_SQL = Mi_SQL + " AND '" + Clase_Negocio.P_Fecha_Fin + "'";

            }


            Mi_SQL = Mi_SQL + " ORDER BY ORDEN." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;

            if(Clase_Negocio.P_No_Orden_Compra != null)
            {
                Mi_SQL = "SELECT ORDEN." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;
                Mi_SQL = Mi_SQL + ", ORDEN." + Ope_Com_Ordenes_Compra.Campo_No_Reserva;
                Mi_SQL = Mi_SQL + ", TO_CHAR(" + Ope_Com_Ordenes_Compra.Campo_Fecha_Cancelacion + ",'DD/MON/YYYY') AS " + Ope_Com_Ordenes_Compra.Campo_Fecha_Cancelacion;
                Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Com_Proveedores.Campo_Nombre;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Proveedores.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + "= ORDEN." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID + ") AS PROVEEDOR";
                Mi_SQL = Mi_SQL + ", ORDEN." + Ope_Com_Ordenes_Compra.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", ORDEN." + Ope_Com_Ordenes_Compra.Campo_Fecha_Cancelacion;
                Mi_SQL = Mi_SQL + ", ORDEN." +Ope_Com_Ordenes_Compra.Campo_Motivo_Cancelacion;
                Mi_SQL = Mi_SQL + ", ORDEN." + Ope_Com_Ordenes_Compra.Campo_Subtotal;
                Mi_SQL = Mi_SQL + ", ORDEN." + Ope_Com_Ordenes_Compra.Campo_Total_IVA;
                Mi_SQL = Mi_SQL + ", ORDEN." + Ope_Com_Ordenes_Compra.Campo_Total;
                Mi_SQL = Mi_SQL + ", (SELECT " + Ope_Com_Requisiciones.Campo_Requisicion_ID;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_No_Orden_Compra;
                Mi_SQL = Mi_SQL + "= ORDEN." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + ") AS " + Ope_Com_Requisiciones.Campo_Requisicion_ID;
                Mi_SQL = Mi_SQL + ", (SELECT " + Ope_Com_Requisiciones.Campo_Listado_Almacen;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_No_Orden_Compra;
                Mi_SQL = Mi_SQL + "= ORDEN." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + ") AS " + Ope_Com_Requisiciones.Campo_Listado_Almacen;
                Mi_SQL = Mi_SQL + ", (SELECT " + Ope_Com_Requisiciones.Campo_Folio;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_No_Orden_Compra;
                Mi_SQL = Mi_SQL + "= ORDEN." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + ") AS FOLIO_REQUISICION ";
                Mi_SQL = Mi_SQL + ", (SELECT " + Ope_Com_Requisiciones.Campo_Codigo_Programatico;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_No_Orden_Compra;
                Mi_SQL = Mi_SQL + "= ORDEN." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + ") AS " + Ope_Com_Requisiciones.Campo_Codigo_Programatico;
                Mi_SQL = Mi_SQL + ", (SELECT " + Ope_Com_Requisiciones.Campo_Justificacion_Compra;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_No_Orden_Compra;
                Mi_SQL = Mi_SQL + "= ORDEN." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + ") AS " +Ope_Com_Requisiciones.Campo_Justificacion_Compra;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDEN ";
                Mi_SQL = Mi_SQL + " WHERE ORDEN." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;
                Mi_SQL = Mi_SQL + " ='" + Clase_Negocio.P_No_Orden_Compra.Trim() + "'";


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
        public static DataTable Consultar_Productos_Servicios(Cls_Ope_Com_Cancelar_Ordenes_Compra_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";
            DataTable Dt_Productos = new DataTable();

            if (Clase_Negocio.P_Estatus != "CANCELACION PARCIAL" && Clase_Negocio.P_Estatus != "CANCELACION TOTAL")
            {
                //Con sultamos primero el tipo de producto de la compra 
                Mi_SQL = Mi_SQL + "SELECT " + Ope_Com_Req_Producto.Campo_Tipo;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Req_Producto.Campo_No_Orden_Compra;
                Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_No_Orden_Compra.Trim() + "'";
                Mi_SQL = Mi_SQL + " GROUP BY " + Ope_Com_Req_Producto.Campo_Tipo;
                DataTable Dt_Tipo_Producto = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];


                if (Dt_Tipo_Producto.Rows[0][Ope_Com_Req_Producto.Campo_Tipo].ToString().Trim() != String.Empty)
                {

                    switch (Dt_Tipo_Producto.Rows[0][Ope_Com_Req_Producto.Campo_Tipo].ToString().Trim())
                    {

                        case "PRODUCTO":
                            Mi_SQL = "SELECT PRODUCTO." + Cat_Com_Productos.Campo_Clave +
                                    ", PRODUCTO." + Cat_Com_Productos.Campo_Nombre +
                                    "||' '|| PRODUCTO." + Cat_Com_Productos.Campo_Descripcion + " AS " + Cat_Com_Productos.Campo_Nombre +
                                    ", DET_REQ." + Ope_Com_Req_Producto.Campo_Cantidad +
                                    ", DET_REQ." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                                    ", DET_REQ." + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
                                    ", DET_REQ." + Ope_Com_Req_Producto.Campo_IVA_Cotizado +
                                    ", DET_REQ." + Ope_Com_Req_Producto.Campo_Subtota_Cotizado +
                                    ", DET_REQ." + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                                    " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " DET_REQ" +
                                    " JOIN " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTO" +
                                    " ON PRODUCTO." + Cat_Com_Productos.Campo_Producto_ID + " =" +
                                    " DET_REQ." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +
                                    " WHERE DET_REQ." + Ope_Com_Req_Producto.Campo_No_Orden_Compra +
                                    "='" + Clase_Negocio.P_No_Orden_Compra + "'";
                            break;
                        case "SERVICIO":
                            Mi_SQL = "SELECT SERVICIO." + Cat_Com_Servicios.Campo_Clave +
                                    ", SERVICIO." + Cat_Com_Servicios.Campo_Nombre +
                                    ", DET_REQ." + Ope_Com_Req_Producto.Campo_Cantidad +
                                    ", DET_REQ." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                                    ", DET_REQ." + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
                                    ", DET_REQ." + Ope_Com_Req_Producto.Campo_IVA_Cotizado +
                                    ", DET_REQ." + Ope_Com_Req_Producto.Campo_Subtota_Cotizado +
                                    ", DET_REQ." + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                                    " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " DET_REQ" +
                                    " JOIN " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + " SERVICIO" +
                                    " ON SERVICIO." + Cat_Com_Servicios.Campo_Servicio_ID + " =" +
                                    " DET_REQ." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +
                                    " WHERE DET_REQ." + Ope_Com_Req_Producto.Campo_No_Orden_Compra +
                                    "='" + Clase_Negocio.P_No_Orden_Compra + "'";

                            break;

                    }
                    Dt_Productos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                }//Fin del IF
            }//FIN DEL IF 
            return Dt_Productos;

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Liberar_Presupuesto_Cancelacion_Total
        ///DESCRIPCIÓN: Metodo que libera el presupúesto de una orden de compra 
        ///PARAMETROS: 1.- Cls_Ope_Com_Cancelar_Ordenes_Compra_Negocio Clase_Negocio
        ///CREO:
        ///FECHA_CREO: 10/OCT/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static String Liberar_Presupuesto_Cancelacion_Total(Cls_Ope_Com_Cancelar_Ordenes_Compra_Negocio Clase_Negocio)
        {
            String Partida_ID = "";
            String Proyecto_ID = "";
            String FF = "";
            double Monto_Total_Cotizado = 0;
            String Mi_SQL = "";
            String Mensaje="";

            try{

            //Consultamos los campos para afectar el presupuesto 
            Mi_SQL = "SELECT " + Ope_Com_Req_Producto.Campo_Partida_ID;
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID;
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto;
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Req_Producto.Campo_No_Orden_Compra;
            Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_No_Orden_Compra.Trim() + "'";
            Mi_SQL = Mi_SQL + " GROUP BY " + Ope_Com_Req_Producto.Campo_Partida_ID;
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID;
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID;
            

            DataTable Dt_Partidas_Productos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                //Obtenemos la Partida, Programa y Fuente de dinanciamiento correspondiente al Producto o servicio
                Partida_ID = Dt_Partidas_Productos.Rows[0][Ope_Com_Req_Producto.Campo_Partida_ID].ToString();
                Proyecto_ID = Dt_Partidas_Productos.Rows[0][Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID].ToString();
                FF = Dt_Partidas_Productos.Rows[0][Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID].ToString();
                Monto_Total_Cotizado = double.Parse(Clase_Negocio.P_Monto_Total);

                //liberamos el presupuesto que era para este listado
                Mi_SQL = "UPDATE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                                " SET " + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible +
                                " =" + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + " + " + Monto_Total_Cotizado +
                                "," + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido +
                                "=" + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + " - " + Monto_Total_Cotizado +
                                " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
                                "='" + Partida_ID + "'" +
                                " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID +
                                "='" + Proyecto_ID + "'" +
                                " AND " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID +
                                "='" + FF + "'" +
                                " AND " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio +
                                " = (SELECT MAX(" + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ")" +
                                " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                                " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
                                "='" + Partida_ID + "'" +
                                " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID +
                                "='" + Proyecto_ID + "'" +
                                " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
                                "= TO_CHAR(SYSDATE,'YYYY'))" +
                                " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
                                "= TO_CHAR(SYSDATE,'YYYY')" +
                                " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID +
                                "=(SELECT " + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                                " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                                " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + "=" +
                                "(SELECT " + Ope_Com_Ordenes_Compra.Campo_No_Requisicion +
                                " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra +
                                " WHERE " + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra +
                                "='" + Clase_Negocio.P_No_Orden_Compra + "' GROUP BY " +
                                Ope_Com_Ordenes_Compra.Campo_No_Requisicion + ") GROUP BY " + Ope_Com_Requisiciones.Campo_Dependencia_ID + ")";

                //Sentencia que ejecuta el query
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Mensaje = "Se cancelo exitosamente la Orden de Compra con su requisicion";
            }catch(Exception Ex)
            {
                Mensaje = Ex.Message;
            }

            return Mensaje;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Liberar_Presupuesto_Cancelacion_Total
        ///DESCRIPCIÓN: Metodo que libera el presupúesto de una orden de compra 
        ///PARAMETROS: 1.- Cls_Ope_Com_Cancelar_Ordenes_Compra_Negocio Clase_Negocio
        ///CREO:
        ///FECHA_CREO: 10/OCT/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static String Liberar_Presupuesto_Cancelacion_Parcial(Cls_Ope_Com_Cancelar_Ordenes_Compra_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";
            String Mensaje_Error = "";
            try
            {
                //consultamos los montos Cotizados y el inicial para hacer un apartado del inicial 
                Mi_SQL = "SELECT REQ_DET." + Ope_Com_Req_Producto.Campo_Partida_ID +
                        ",REQ_DET." + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID +
                        ", (SELECT " + Ope_Com_Requisiciones.Campo_Total +
                        " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                        " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                        "= REQ_DET." + Ope_Com_Req_Producto.Campo_Requisicion_ID + ") AS TOTAL" +
                         ", (SELECT " + Ope_Com_Requisiciones.Campo_Total_Cotizado +
                         " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                         " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                         "= REQ_DET." + Ope_Com_Req_Producto.Campo_Requisicion_ID + ") AS TOTAL_COTIZADO" +
                         ", (SELECT " + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                         " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                         " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                         "= REQ_DET." + Ope_Com_Req_Producto.Campo_Requisicion_ID + ") AS DEPENDENCIA_ID" +
                         ", REQ_DET." + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID +
                         " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_DET" +
                         " WHERE REQ_DET." + Ope_Com_Req_Producto.Campo_Requisicion_ID + "='" + Clase_Negocio.P_No_Requisicio + "'";
                DataTable Dt_Requisicion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                //Guardamos en la variable Monto_Restante la diferencia de Total_Cotizado y Monto_Total inicial del producto
                //Primero checamos si se va a restar o a aumentar el presupuesto
                double Monto_Cotizado = double.Parse(Dt_Requisicion.Rows[0]["TOTAL_COTIZADO"].ToString());
                double Monto_Anterior = double.Parse(Dt_Requisicion.Rows[0]["TOTAL"].ToString());
                double Diferencia = 0;
                String Partida_ID = Dt_Requisicion.Rows[0][Ope_Com_Req_Producto.Campo_Partida_ID].ToString().Trim();
                String Proyecto_ID = Dt_Requisicion.Rows[0][Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID].ToString().Trim();
                String Dependencia_ID = Dt_Requisicion.Rows[0]["DEPENDENCIA_ID"].ToString().Trim();
                String FF = Dt_Requisicion.Rows[0][Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID].ToString().Trim();
                bool Suma_Diferencia = false;

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
                //modificamos el monto dependiendo de si se resta la dir¿ferencia o se suma
                if (Suma_Diferencia == true)
                {
                    //Primero se Verifica que aun exista Presupuesto
                    Mi_SQL = "";
                    Mi_SQL = "SELECT " + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible;
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID;
                    Mi_SQL = Mi_SQL + "='" + Partida_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID;
                    Mi_SQL = Mi_SQL + "='" + Proyecto_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID;
                    Mi_SQL = Mi_SQL + "='" + FF + "'";
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto;
                    Mi_SQL = Mi_SQL + "= TO_CHAR(SYSDATE,'YYYY')";
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID;
                    Mi_SQL = Mi_SQL + "='" + Dependencia_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio;
                    Mi_SQL = Mi_SQL + " = (SELECT MAX(" + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ")";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID;
                    Mi_SQL = Mi_SQL + "='" + Partida_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID;
                    Mi_SQL = Mi_SQL + "='" + Proyecto_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID;
                    Mi_SQL = Mi_SQL + "='" + Dependencia_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto;
                    Mi_SQL = Mi_SQL + "= TO_CHAR(SYSDATE,'YYYY'))";
                    DataTable Dt_Monto_Disponible = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    double Monto_Disponible = double.Parse(Dt_Monto_Disponible.Rows[0][Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible].ToString().Trim());
                    if (Monto_Disponible > Diferencia)
                    {
                        //se ACTUALIZAN LOS PRESUPUESTOS DE LA DEPENDENCIA, CORRESPONDIENTE AL PROYECTO Y PARTIDA
                        Mi_SQL = "";
                        Mi_SQL = "UPDATE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                        Mi_SQL = Mi_SQL + " SET " + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible;
                        Mi_SQL = Mi_SQL + " =" + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + " - " + Diferencia.ToString();
                        Mi_SQL = Mi_SQL + "," + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido;
                        Mi_SQL = Mi_SQL + "=" + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + " + " + Diferencia.ToString();
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID;
                        Mi_SQL = Mi_SQL + "='" + Partida_ID + "'";
                        Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID;
                        Mi_SQL = Mi_SQL + "='" + Proyecto_ID + "'";
                        Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID;
                        Mi_SQL = Mi_SQL + "='" + FF + "'";
                        Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto;
                        Mi_SQL = Mi_SQL + "= TO_CHAR(SYSDATE,'YYYY')";
                        Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + "='" + Dependencia_ID + "'";
                        Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio;
                        Mi_SQL = Mi_SQL + " = (SELECT MAX(" + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ")";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID;
                        Mi_SQL = Mi_SQL + "='" + Partida_ID + "'";
                        Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID;
                        Mi_SQL = Mi_SQL + "='" + Proyecto_ID + "'";
                        Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + "='" + Dependencia_ID + "'";
                        Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto;
                        Mi_SQL = Mi_SQL + "= TO_CHAR(SYSDATE,'YYYY'))";

                        //Sentencia que ejecuta el query
                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        //Modificamos los montos cotizados de la requisicion a 0
                        Modificar_Montos_Cotizados_Requisicion(Clase_Negocio);
                        Mensaje_Error = "Se realizo la Cancelacion Parcial Satisfactoriamente";
                    }
                    else
                    {
                        Mensaje_Error = "No existe presupuesto Suficiente para realizar la Cancelacion Parcial";
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
                        " AND " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID +
                        "='" + FF + "'" +
                        " AND " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio +
                        " = (SELECT MAX(" + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ")" +
                        " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                        " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
                        "='" + Partida_ID + "'" +
                        " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID +
                        "='" + Proyecto_ID + "'" +
                        " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
                        "= TO_CHAR(SYSDATE,'YYYY'))" +
                        " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
                        "= TO_CHAR(SYSDATE,'YYYY')" +
                        " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID +
                        "='" + Dependencia_ID + "'";
                    //Sentencia que ejecuta el query
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    //Modificamos los montos cotizados de la requisicion a 0
                    Modificar_Montos_Cotizados_Requisicion(Clase_Negocio);
                    Mensaje_Error = "Se realizo la Cancelacion Parcial Satisfactoriamente";
                }

            }
            catch (Exception Ex)
            {
                Mensaje_Error = Ex.Message;
            }
            return Mensaje_Error;


        }

        public static void Modificar_Montos_Cotizados_Requisicion(Cls_Ope_Com_Cancelar_Ordenes_Compra_Negocio Clase_Negocio)
        {
            //mODIFICAMOS LOS TOTALES COTIZADOS EN LA TABLA OPE_COM_REQUISICIONES
            String Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
            Mi_SQL = Mi_SQL + " SET " + Ope_Com_Requisiciones.Campo_Subtotal_Cotizado + "=0";
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Campo_Total_Cotizado + "=0";
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Campo_IVA_Cotizado + "=0";
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Campo_IEPS_Cotizado + "=0";
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + "='" + Clase_Negocio.P_No_Requisicio.Trim() + "'";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            //MODIFICAMOS LOS MONTOS COTIZADOS EN EL DETALLE DE PRODUCTOS DE LA REQUISICION
            Mi_SQL = "";
            Mi_SQL = "UPDATE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto;
            Mi_SQL = Mi_SQL + " SET " + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado + "=0";
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado + "=0";
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Subtota_Cotizado + "=0";
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Total_Cotizado + "=0";
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Proveedor_ID + "=NULL";
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Nombre_Proveedor + "=NULL";
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_No_Orden_Compra + "=0";
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + "='" + Clase_Negocio.P_No_Requisicio.Trim() + "'";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

        }

        public static String Modificar_Orden_Compra(Cls_Ope_Com_Cancelar_Ordenes_Compra_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";
            String Mensaje = "";
            //VAriable que ayudara a mandar llamar el nombre del metodo para dar de alta el estatus en el segumiento de la requisicion
            Cls_Ope_Com_Requisiciones_Datos Requisicion = new Cls_Ope_Com_Requisiciones_Datos();
            Cls_Ope_Com_Administrar_Requisiciones_Negocio Administrar_Requisicion =
                new Cls_Ope_Com_Administrar_Requisiciones_Negocio();   
            try
            {

                
                Mi_SQL = "";
                

                //En caso de ser cancelacion total
                if (Clase_Negocio.P_Estatus.Trim() == "CANCELACION TOTAL")
                {
                   


                    Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
                    Mi_SQL = Mi_SQL + " SET " + Ope_Com_Requisiciones.Campo_Estatus;
                    Mi_SQL = Mi_SQL + "='CANCELADA'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Campo_Fecha_Cancelada;
                    Mi_SQL = Mi_SQL + "=SYSDATE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Campo_Empleado_Cancelada_ID;
                    Mi_SQL = Mi_SQL + "='" + Cls_Sessiones.Empleado_ID + "'";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID;
                    Mi_SQL = Mi_SQL + "='" +Clase_Negocio.P_No_Requisicio + "'";


                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    //Registramos la afectacion al Estatus  de las propuestas de cotizacion ya que se cambia a aceptada, se pone como RECHAZADA
                    Mi_SQL = "UPDATE " + Ope_Com_Propuesta_Cotizacion.Tabla_Ope_Com_Propuesta_Cotizacion;
                    Mi_SQL = Mi_SQL + " SET ESTATUS='RECHAZADA' WHERE " + Ope_Com_Propuesta_Cotizacion.Campo_No_Requisicion + "='" + Clase_Negocio.P_No_Requisicio.Trim() + "'";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    Cls_Ope_Com_Requisiciones_Datos.Registrar_Historial("CANCELADA", Clase_Negocio.P_No_Requisicio);
                    Administrar_Requisicion.P_Requisicion_ID = Clase_Negocio.P_No_Requisicio;
                    Administrar_Requisicion.P_Comentario = Clase_Negocio.P_Motivo_Cancelacion;
                    Administrar_Requisicion.P_Estatus = "CANCELADA";
                    Administrar_Requisicion.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToString();
                    Administrar_Requisicion.Alta_Observaciones();

                    //Se modifica la propuesta de cotizacion se cambia el estatus a 
                    if (Clase_Negocio.P_Listado_Almacen == null)
                    {
                        //liberamos por completo el presupuesto en caso de no ser unlistado almace

                        Mensaje = Liberar_Presupuesto_Cancelacion_Total(Clase_Negocio);
                    }
                    else
                    {
                        Mensaje = "Se realizo la Cancelacion Total Satisfactoriamente";
                    }
                    //Modificamos el estatus de la Requisicion
                    Mi_SQL = "UPDATE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra;
                    Mi_SQL = Mi_SQL + " SET " + Ope_Com_Ordenes_Compra.Campo_Estatus;
                    Mi_SQL = Mi_SQL + "='CANCELADA'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Ordenes_Compra.Campo_Fecha_Cancelacion;
                    Mi_SQL = Mi_SQL + "=SYSDATE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Ordenes_Compra.Campo_Motivo_Cancelacion;
                    Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Motivo_Cancelacion + "'";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;
                    Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_No_Orden_Compra.Trim() + "'";
                    //Sentencia que ejecuta el query

                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }//Fin del if Cancelacion total

                //En caso de Ser cancelacion Parcial 
                if (Clase_Negocio.P_Estatus.Trim() == "CANCELACION PARCIAL")
                {                    
                    //Cuando es un listado almacen se realiza lo siguiente
                    if (Clase_Negocio.P_Listado_Almacen != null)
                    {
                        Mi_SQL = "";
                        Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
                        Mi_SQL = Mi_SQL + " SET " + Ope_Com_Requisiciones.Campo_Estatus;
                        Mi_SQL = Mi_SQL + "='FILTRADA'";
                        Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Campo_Alerta + "='ROJA2'";
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID;
                        Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_No_Requisicio.Trim() + "'";

                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        //REgistramos el cambio de estatus a Proveedor pero antes una de Reasignar_Proveedor

                        Cls_Ope_Com_Requisiciones_Datos.Registrar_Historial("R_PROVEEDOR", Clase_Negocio.P_No_Requisicio);
                        Cls_Ope_Com_Requisiciones_Datos.Registrar_Historial("FILTRADA", Clase_Negocio.P_No_Requisicio);
                        Administrar_Requisicion.P_Requisicion_ID = Clase_Negocio.P_No_Requisicio;
                        Administrar_Requisicion.P_Comentario = Clase_Negocio.P_Motivo_Cancelacion + " / Se canceló la orden de compra";
                        Administrar_Requisicion.P_Estatus = "FILTRADA";
                        Administrar_Requisicion.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToString();
                        Administrar_Requisicion.Alta_Observaciones();

                        //Modificamos todas las propuestas de Cotizacion al estatus de EN CONSTRUCCION con la finalidad de poder reecotizar los productos

                        //Modificamos todas las propuestas de Cotizacion a EN CONSTRUCCION para poder modificar los montos en caso de ser necesario
                        Mi_SQL = "UPDATE " + Ope_Com_Propuesta_Cotizacion.Tabla_Ope_Com_Propuesta_Cotizacion;
                        Mi_SQL = Mi_SQL + " SET " + Ope_Com_Propuesta_Cotizacion.Campo_Estatus;
                        Mi_SQL = Mi_SQL + "='EN CONSTRUCCION'";
                        Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Resultado + "=NULL";
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Propuesta_Cotizacion.Campo_No_Requisicion + "='" + Clase_Negocio.P_No_Requisicio.Trim() + "'";
                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                        //En caso de ser listado de almacen afectar solo los montos de la requisicion
                            Modificar_Montos_Cotizados_Requisicion(Clase_Negocio);
                            
                        
                        //Modificamos el estatus de la Requisicion
                        Mi_SQL = "UPDATE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra;
                        Mi_SQL = Mi_SQL + " SET " + Ope_Com_Ordenes_Compra.Campo_Estatus;
                        Mi_SQL = Mi_SQL + "='CANCELADA'";
                        Mi_SQL = Mi_SQL + ", " + Ope_Com_Ordenes_Compra.Campo_Fecha_Cancelacion;
                        Mi_SQL = Mi_SQL + "=SYSDATE";
                        Mi_SQL = Mi_SQL + ", " + Ope_Com_Ordenes_Compra.Campo_Motivo_Cancelacion;
                        Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Motivo_Cancelacion + "'";
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;
                        Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_No_Orden_Compra.Trim() + "'";
                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        Mensaje = "Se realizo la Cancelacion Parcial Satisfactoriamente";
                    }
                    //VERIFICAMOS PRIMERO SI EXISTE PRESUPUESTO ESTO ES PARA CUANDO NO ES UN LISTADO DE ALMACEN 
                    if (Clase_Negocio.P_Listado_Almacen == null)
                    {
                        if (Consultamos_Presupuesto_Existente(Clase_Negocio) == true)
                        {
                            Mi_SQL = "";
                            Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
                            Mi_SQL = Mi_SQL + " SET " + Ope_Com_Requisiciones.Campo_Estatus;
                            Mi_SQL = Mi_SQL + "='FILTRADA'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Campo_Alerta + "='ROJA2'";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID;
                            Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_No_Requisicio.Trim() + "'";

                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                            //REgistramos el cambio de estatus a Proveedor pero antes una de Reasignar_Proveedor

                            Cls_Ope_Com_Requisiciones_Datos.Registrar_Historial("R_PROVEEDOR", Clase_Negocio.P_No_Requisicio);
                            Cls_Ope_Com_Requisiciones_Datos.Registrar_Historial("FILTRADA", Clase_Negocio.P_No_Requisicio);
                            Administrar_Requisicion.P_Requisicion_ID = Clase_Negocio.P_No_Requisicio;
                            Administrar_Requisicion.P_Comentario = Clase_Negocio.P_Motivo_Cancelacion + " / Se canceló la orden de compra";
                            Administrar_Requisicion.P_Estatus = "FILTRADA";
                            Administrar_Requisicion.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToString();
                            Administrar_Requisicion.Alta_Observaciones();

                            //Modificamos todas las propuestas de Cotizacion al estatus de EN CONSTRUCCION con la finalidad de poder reecotizar los productos

                            //Modificamos todas las propuestas de Cotizacion a EN CONSTRUCCION para poder modificar los montos en caso de ser necesario
                            Mi_SQL = "UPDATE " + Ope_Com_Propuesta_Cotizacion.Tabla_Ope_Com_Propuesta_Cotizacion;
                            Mi_SQL = Mi_SQL + " SET " + Ope_Com_Propuesta_Cotizacion.Campo_Estatus;
                            Mi_SQL = Mi_SQL + "='EN CONSTRUCCION'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Resultado + "=NULL";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Propuesta_Cotizacion.Campo_No_Requisicion + "='" + Clase_Negocio.P_No_Requisicio.Trim() + "'";
                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);


                            if (Clase_Negocio.P_Listado_Almacen == null)
                            {
                                //liberamos por completo el presupuesto en caso de no ser unlistado almace

                                //Liberamos el presupuesto y pasamos a cero todos los valores cotizados de la requisicion para su nueva cotizacion
                                Mensaje = Liberar_Presupuesto_Cancelacion_Parcial(Clase_Negocio);
                            }
                            else
                            {
                                //En caso de ser listado de almacen afectar solo los montos de la requisicion
                                Modificar_Montos_Cotizados_Requisicion(Clase_Negocio);
                                Mensaje = "Se realizo la Cancelacion Parcial Satisfactoriamente";
                            }
                            //Modificamos el estatus de la Requisicion
                            Mi_SQL = "UPDATE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra;
                            Mi_SQL = Mi_SQL + " SET " + Ope_Com_Ordenes_Compra.Campo_Estatus;
                            Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Estatus.Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Com_Ordenes_Compra.Campo_Fecha_Cancelacion;
                            Mi_SQL = Mi_SQL + "=SYSDATE";
                            Mi_SQL = Mi_SQL + ", " + Ope_Com_Ordenes_Compra.Campo_Motivo_Cancelacion;
                            Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Motivo_Cancelacion + "'";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;
                            Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_No_Orden_Compra.Trim() + "'";
                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        }
                        else
                        {
                            Mensaje = "El presupuesto no permite realizar la cancelacion, es insuficiente";
                        }
                    }//fin Clase_Negocio.P_Listado_Almacen == null
                    
                }//fin del if Cancelacion Parcial 

                
                

            }
            catch(Exception EX)
            {
                Mensaje = EX.Message;
            }


            return Mensaje;

        }



    ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultamos_Presupuesto_Existente
        ///DESCRIPCIÓN: Metodo que consulta si existe presupuesto para los productos seleccionados
        ///PARAMETROS:  1.- Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 25/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static bool Consultamos_Presupuesto_Existente(Cls_Ope_Com_Cancelar_Ordenes_Compra_Negocio Requisicion_Negocio)
        {
            bool Existe_Presupuesto = false;
            String Mi_SQL = "";
            //Primero Obtenemos todos lo productos pertenecientes a la requisicion:
                    Mi_SQL = "SELECT REQ_DET." + Ope_Com_Req_Producto.Campo_Partida_ID +
                    ",REQ_DET." + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID +
                    ", (SELECT " + Ope_Com_Requisiciones.Campo_Total +
                    " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                    " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                    "= REQ_DET." + Ope_Com_Req_Producto.Campo_Requisicion_ID + ") AS TOTAL" +
                     ", (SELECT " + Ope_Com_Requisiciones.Campo_Total_Cotizado +
                     " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                     " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                     "= REQ_DET." + Ope_Com_Req_Producto.Campo_Requisicion_ID + ") AS TOTAL_COTIZADO" +
                     ", (SELECT " + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                     " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                     " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                     "= REQ_DET." + Ope_Com_Req_Producto.Campo_Requisicion_ID + ") AS DEPENDENCIA_ID" +
                     ", REQ_DET." + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID +
                     " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_DET" +
                     " WHERE REQ_DET." + Ope_Com_Req_Producto.Campo_Requisicion_ID + "='" + Requisicion_Negocio.P_No_Requisicio + "'";
            DataTable Dt_Requisicion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            //Guardamos en la variable Monto_Restante la diferencia de Total_Cotizado y Monto_Total inicial del producto
            //Primero checamos si se va a restar o a aumentar el presupuesto
            double Monto_Cotizado = double.Parse(Dt_Requisicion.Rows[0]["TOTAL_COTIZADO"].ToString().Trim());
            double Monto_Anterior = double.Parse(Dt_Requisicion.Rows[0]["TOTAL"].ToString().Trim());
            double Diferencia = 0;
            double Disponible = 0;
            String Partida_ID = Dt_Requisicion.Rows[0][Ope_Com_Req_Producto.Campo_Partida_ID].ToString().Trim();
            String Proyecto_ID = Dt_Requisicion.Rows[0][Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID].ToString().Trim();
            String Dependencia_ID = Dt_Requisicion.Rows[0]["DEPENDENCIA_ID"].ToString().Trim();
            String FF = Dt_Requisicion.Rows[0][Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID].ToString().Trim();
           
            // PASO 1 VERIFICAMOS CUAL DE LOS 2 MONTOS ES MAYOR SI EL COTIZADO O  EL ANTERIOR
            if (Monto_Anterior > Monto_Cotizado)
            {
                //Obtenemos la resta
                Diferencia = Monto_Anterior - Monto_Cotizado;
                Mi_SQL = "";
                //Consultamos el Monto presupuestal para ver si es suficiente 
                Mi_SQL = "SELECT " + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID;
                Mi_SQL = Mi_SQL + "='" + Partida_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID;
                Mi_SQL = Mi_SQL + "='" + Proyecto_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID;
                Mi_SQL = Mi_SQL + "='" + FF + "'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio;
                Mi_SQL = Mi_SQL + " = (SELECT MAX(" + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ")";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID;
                Mi_SQL = Mi_SQL + "='" + Partida_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID;
                Mi_SQL = Mi_SQL + "='" + Proyecto_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + "='" + Dependencia_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto;
                Mi_SQL = Mi_SQL + "= TO_CHAR(SYSDATE,'YYYY'))";
                Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto;
                Mi_SQL = Mi_SQL + "= TO_CHAR(SYSDATE,'YYYY')";
                Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + "='" + Dependencia_ID + "'";

                //Sentencia que ejecuta el query
                DataTable Dt_Presupuestos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                Disponible = double.Parse(Dt_Presupuestos.Rows[0][Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible].ToString().Trim());
                if (Disponible >= Diferencia)
                    Existe_Presupuesto = true;
                else
                    Existe_Presupuesto = false;
            }
            else
                Existe_Presupuesto = true; //Existe presupuesto ya que el monto cotizado es menor que el k se aparto 

           
            return Existe_Presupuesto;
        }

    }//fin del class
}//fin del namespace