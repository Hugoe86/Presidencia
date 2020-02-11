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
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Generar_Req_Listado.Negocio;
using Presidencia.Sessiones;
using Presidencia.Generar_Requisicion.Negocio;



/// <summary>
/// Summary description for Cls_Ope_Alm_Requisicion_Listado_Stock_Datos
/// </summary>
/// 

namespace Presidencia.Generar_Req_Listado.Datos
{
    public class Cls_Ope_Alm_Requisicion_Listado_Stock_Datos
    {
        
        public static DataTable Consulta_Listado_Almacen(Cls_Ope_Alm_Requisicion_Listado_Stock_Negocio Clase_Negocio)
        {
            String Mi_SQL = "SELECT LISTADO." + Ope_Com_Listado.Campo_Folio;
            Mi_SQL = Mi_SQL + ", LISTADO." + Ope_Com_Listado.Campo_Listado_ID;
            Mi_SQL = Mi_SQL + ",  TO_CHAR(LISTADO." + Ope_Com_Listado.Campo_Fecha_Creo + ",'DD/MON/YYYY') AS FECHA_CREO";
            Mi_SQL = Mi_SQL + ", LISTADO." + Ope_Com_Listado.Campo_Tipo;
            Mi_SQL = Mi_SQL + ", LISTADO." + Ope_Com_Listado.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", LISTADO." + Ope_Com_Listado.Campo_Total;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Listado.Tabla_Ope_Com_Listado + " LISTADO";
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Listado.Campo_Estatus + "='AUTORIZADA'";

            if (Clase_Negocio.P_Listado_ID != null)
            {
                Mi_SQL = "SELECT * FROM " + Ope_Com_Listado.Tabla_Ope_Com_Listado + " WHERE " + Ope_Com_Listado.Campo_Listado_ID + "='" +Clase_Negocio.P_Listado_ID +"'";
            }
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        public static DataTable Consulta_Listado_Detalle(Cls_Ope_Alm_Requisicion_Listado_Stock_Negocio Clase_Negocio)
        {

            String Mi_SQL = "SELECT LISTADO." + Ope_Com_Listado_Detalle.Campo_No_Producto_ID + " AS PRODUCTO_ID";
            Mi_SQL = Mi_SQL + ", PRODUCTO." + Cat_Com_Productos.Campo_Clave;
            Mi_SQL = Mi_SQL + ", PRODUCTO." + Cat_Com_Productos.Campo_Nombre + " AS PRODUCTO_NOMBRE";
            Mi_SQL = Mi_SQL + ", PRODUCTO." + Cat_Com_Productos.Campo_Descripcion;
            Mi_SQL = Mi_SQL + ", PRODUCTO." + Cat_Com_Productos.Campo_Disponible;
            Mi_SQL = Mi_SQL + ", PRODUCTO." + Cat_Com_Productos.Campo_Reorden;
            Mi_SQL = Mi_SQL + ", PRODUCTO." + Cat_Com_Productos.Campo_Partida_ID;
            Mi_SQL = Mi_SQL + ", PRODUCTO." + Cat_Com_Productos.Campo_Costo + " AS PRECIO_UNITARIO ";
            Mi_SQL = Mi_SQL + ",(SELECT " + Cat_Sap_Concepto.Campo_Clave + " ||' '|| " + Cat_Sap_Concepto.Campo_Descripcion + " FROM ";
            Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + " WHERE " + Cat_Sap_Concepto.Campo_Concepto_ID + "=(SELECT ";
            Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Genericas.Campo_Concepto_ID + " FROM ";
            Mi_SQL = Mi_SQL + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + " WHERE ";
            Mi_SQL = Mi_SQL + "CAT_SAP_PARTIDA_GENERICA." + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID;
            Mi_SQL = Mi_SQL + "=(SELECT " + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID + " FROM ";
            Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " = PRODUCTO.";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Partida_ID + "))) AS CONCEPTO";
            Mi_SQL = Mi_SQL + ",(SELECT " + Cat_Sap_Concepto.Campo_Concepto_ID + " FROM ";
            Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + " WHERE " + Cat_Sap_Concepto.Campo_Concepto_ID + "=(SELECT ";
            Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Genericas.Campo_Concepto_ID + " FROM ";
            Mi_SQL = Mi_SQL + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + " WHERE ";
            Mi_SQL = Mi_SQL + "CAT_SAP_PARTIDA_GENERICA." + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID;
            Mi_SQL = Mi_SQL + "=(SELECT " + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID + " FROM ";
            Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " = PRODUCTO.";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Partida_ID + "))) AS CONCEPTO_ID";
            Mi_SQL = Mi_SQL + ", LISTADO." + Ope_Com_Listado_Detalle.Campo_Cantidad;
            Mi_SQL = Mi_SQL + ", LISTADO." + Ope_Com_Listado_Detalle.Campo_Costo_Compra;
            Mi_SQL = Mi_SQL + ", LISTADO." + Ope_Com_Listado_Detalle.Campo_Importe;
            Mi_SQL = Mi_SQL + ", LISTADO." + Ope_Com_Listado_Detalle.Campo_Monto_IVA;
            Mi_SQL = Mi_SQL + ", LISTADO." + Ope_Com_Listado_Detalle.Campo_Monto_IEPS;
            Mi_SQL = Mi_SQL + ", LISTADO." + Ope_Com_Listado_Detalle.Campo_Porcentaje_IVA;
            Mi_SQL = Mi_SQL + ", LISTADO." + Ope_Com_Listado_Detalle.Campo_Porcentaje_IEPS;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Listado_Detalle.Tabla_Ope_Com_Listado_Detalle + " LISTADO";
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTO";
            Mi_SQL = Mi_SQL + " ON PRODUCTO." + Cat_Com_Productos.Campo_Producto_ID + " = LISTADO." + Ope_Com_Listado_Detalle.Campo_No_Producto_ID;
            Mi_SQL = Mi_SQL + " WHERE LISTADO." + Ope_Com_Listado_Detalle.Campo_No_Listado_ID + " = '" +Clase_Negocio.P_Listado_ID + "'";
            Mi_SQL = Mi_SQL + " AND LISTADO." + Ope_Com_Listado_Detalle.Campo_Borrado + " IS NULL ";
            Mi_SQL = Mi_SQL + " AND LISTADO." + Ope_Com_Listado_Detalle.Campo_No_Requisicion + " IS NULL ";
            Mi_SQL = Mi_SQL + " ORDER BY PRODUCTO." + Cat_Com_Productos.Campo_Nombre;
            Mi_SQL = Mi_SQL + " , PRODUCTO." + Cat_Com_Productos.Campo_Descripcion;

            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }

        public static bool Borrar_Productos_Listado(Cls_Ope_Alm_Requisicion_Listado_Stock_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";
            bool Operacion_Realizada= false;
            try
            {
                //Recorremos el listado para eliminar 
                for (int i = 0; i < Clase_Negocio.P_Dt_Productos.Rows.Count; i++)
                {
                    Mi_SQL = "UPDATE " + Ope_Com_Listado_Detalle.Tabla_Ope_Com_Listado_Detalle;
                    Mi_SQL = Mi_SQL + " SET " + Ope_Com_Listado_Detalle.Campo_Borrado + "='SI'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Listado_Detalle.Campo_Motivo_Borrado + "='" + Clase_Negocio.P_Motivo_Borrado + "'";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Listado_Detalle.Campo_No_Listado_ID;
                    Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Listado_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Listado_Detalle.Campo_No_Producto_ID;
                    Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Dt_Productos.Rows[i]["Producto_ID"].ToString().Trim() +"'";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                Operacion_Realizada = true;
            }
            catch
            {
                Operacion_Realizada = false;
            }

            return Operacion_Realizada;
        }

        public static String Convertir_Requisicion_Transitoria(Cls_Ope_Alm_Requisicion_Listado_Stock_Negocio Clase_Negocio)
        {
            String Mensaje_Error = "";
            String Id_Requisicion = "";
            try
            {
                //Consultamos el id de la dependencia de Almacen que se encuentra en los parametros 
                String Mi_SQL = "SELECT " + Cat_Com_Parametros.Campo_Dependencia_ID_Almacen +
                    ", " + Cat_Com_Parametros.Campo_Partida_Esp_Almacen_Global +
                    ", " + Cat_Com_Parametros.Campo_Programa_Almacen +
                    " FROM " + Cat_Com_Parametros.Tabla_Cat_Com_Parametros;
                DataTable Dt_Parametros = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                String Partida_Esp_Almacen_Global = Dt_Parametros.Rows[0][Cat_Com_Parametros.Campo_Partida_Esp_Almacen_Global].ToString();
                String Programa_ID_Almacen = Dt_Parametros.Rows[0][Cat_Com_Parametros.Campo_Programa_Almacen].ToString();
                String Dependencia_ID_Almacen = Dt_Parametros.Rows[0][Cat_Com_Parametros.Campo_Dependencia_ID_Almacen].ToString();
                //Consultamos la Persona que autorizo el listado para asignarlo en la requisicion que se va crear
                Mi_SQL = "";
                Mi_SQL = "SELECT LIS." + Ope_Com_Listado.Campo_Empleado_Autorizacion_ID;
                Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Empleados.Campo_Nombre;
                Mi_SQL = Mi_SQL + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno;
                Mi_SQL = Mi_SQL + "||' '||" + Cat_Empleados.Campo_Apellido_Materno;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL = Mi_SQL + " = LIS." + Ope_Com_Listado.Campo_Empleado_Autorizacion_ID;
                Mi_SQL = Mi_SQL + ") AS EMPLEADO_AUTORIZO";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Listado.Tabla_Ope_Com_Listado + " LIS";
                Mi_SQL = Mi_SQL + " WHERE LIS." + Ope_Com_Listado.Campo_Listado_ID;
                Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Listado_ID + "'";
                DataTable Dt_Autorizo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                String Empleado_Autorizo = "";

                if (Dt_Autorizo.Rows.Count != 0)
                    Empleado_Autorizo = Dt_Autorizo.Rows[0][Ope_Com_Listado.Campo_Empleado_Autorizacion_ID].ToString().Trim();
                //Generamos el id de la requisiciion 
                Id_Requisicion = Obtener_Consecutivo(Ope_Com_Requisiciones.Campo_Requisicion_ID, Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones).ToString();
                Mi_SQL = "INSERT INTO " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                    " (" + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                    ", " + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                    ", " + Ope_Com_Requisiciones.Campo_Folio +
                    ", " + Ope_Com_Requisiciones.Campo_Estatus +
                    ", " + Ope_Com_Requisiciones.Campo_Tipo +
                    ", " + Ope_Com_Requisiciones.Campo_Fase +
                    ", " + Ope_Com_Requisiciones.Campo_Usuario_Creo +
                    ", " + Ope_Com_Requisiciones.Campo_Fecha_Creo +
                    ", " + Ope_Com_Requisiciones.Campo_Empleado_Filtrado_ID +
                    ", " + Ope_Com_Requisiciones.Campo_Fecha_Filtrado +
                    ", " + Ope_Com_Requisiciones.Campo_Tipo_Articulo +
                    ", " + Ope_Com_Requisiciones.Campo_Empleado_Construccion_ID +
                    ", " + Ope_Com_Requisiciones.Campo_Fecha_Construccion +
                    ", " + Ope_Com_Requisiciones.Campo_Empleado_Generacion_ID +
                    ", " + Ope_Com_Requisiciones.Campo_Fecha_Generacion +
                    ", " + Ope_Com_Requisiciones.Campo_Empleado_Autorizacion_ID +
                    ", " + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion +
                    ", " + Ope_Com_Requisiciones.Campo_Listado_Almacen +
                    ", " + Ope_Com_Requisiciones.Campo_Partida_ID +
                    ", " + Ope_Com_Requisiciones.Campo_Justificacion_Compra +
                    ") VALUES ('" + Id_Requisicion + "','" +
                    Dependencia_ID_Almacen + "','" +
                    "RQ-" + Id_Requisicion + "','" +
                    "PROCESAR','" +
                    "TRANSITORIA','" +
                    "REQUISICION','" +
                    Cls_Sessiones.Nombre_Empleado + "',SYSDATE," +
                    "'" + Cls_Sessiones.Empleado_ID + "',SYSDATE,'PRODUCTO'," +
                    "'" + Cls_Sessiones.Empleado_ID + "',SYSDATE," +
                    "'" + Cls_Sessiones.Empleado_ID + "',SYSDATE," +
                    "'" + Empleado_Autorizo + "',SYSDATE,'SI','" +
                    Clase_Negocio.P_Dt_Productos.Rows[0]["Partida_ID"].ToString().Trim() +
                    "','MATERIAL STOCK. ALMACEN DE MATERIALES Y SUMINISTROS DE CONSUMO')";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                //Ahora asignamos el id de la requisicion al detalle de listado de almacen, esto para realizar la relacion en caso de ser necesario
                for (int i = 0; i < Clase_Negocio.P_Dt_Productos.Rows.Count; i++)
                {
                    Mi_SQL = "UPDATE " + Ope_Com_Listado_Detalle.Tabla_Ope_Com_Listado_Detalle;
                    Mi_SQL = Mi_SQL + " SET " + Ope_Com_Listado_Detalle.Campo_No_Requisicion;
                    Mi_SQL = Mi_SQL + " = '" + Id_Requisicion + "'";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Listado_Detalle.Campo_No_Producto_ID + "='";
                    Mi_SQL = Mi_SQL + Clase_Negocio.P_Dt_Productos.Rows[i]["Producto_ID"].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Listado_Detalle.Campo_No_Listado_ID + "='";
                    Mi_SQL = Mi_SQL + Clase_Negocio.P_Listado_ID.ToString().Trim() + "'";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                //Ahora recorremos el data de los productos del listado y los pasamos a la requisicion

                //Calculamos el IEPS, IVA y Subtotal de la requisicion de acuerdo a los productos que le pertenecen a esta
                //Variable que almacena la suma de todos los valores del IVA que tiene cada producto del detalle
                double IVA_Acumulado = 0;
                //Variable que almacena la suma de todos los valores del IEPS que tiene cada producto del detalle
                double IEPS_Acumulado = 0;
                //Variable que almacena la suma del costo compra sin tomar en cuenta el aumento por impuestos
                double Subtotal = 0;
                double Total = 0;
                double Subtotal_Producto = 0;
                double IVA_Producto = 0;
                double IEPS_Producto = 0;
                if (Clase_Negocio.P_Dt_Productos.Rows.Count != 0)
                {
                    for (int i = 0; i < Clase_Negocio.P_Dt_Productos.Rows.Count; i++)
                    {
                        //obtenemos el Subtotal ya que el costo compra solo es el precio unitario sin la multiplicacion de la cantidad
                        Subtotal_Producto = double.Parse(Clase_Negocio.P_Dt_Productos.Rows[i]["Precio_Unitario"].ToString()) * int.Parse(Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Listado_Detalle.Campo_Cantidad].ToString());
                        IVA_Producto = double.Parse(Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Listado_Detalle.Campo_Monto_IVA].ToString());
                        IEPS_Producto = double.Parse(Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Listado_Detalle.Campo_Monto_IEPS].ToString());

                        Mi_SQL = "INSERT INTO ";
                        Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto;
                        Mi_SQL = Mi_SQL + " (" + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Requisicion_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Prod_Serv_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Partida_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Cantidad;
                        Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Usuario_Creo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Fecha_Creo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Precio_Unitario;
                        Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Monto_IVA;
                        Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Monto_IEPS;
                        Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Porcentaje_IVA;
                        Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Porcentaje_IEPS;
                        Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Importe;
                        Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Monto_Total;
                        Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Tipo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Clave;
                        Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Nombre_Giro;
                        Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Giro_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio;
                        Mi_SQL = Mi_SQL + " ) VALUES ";
                        Mi_SQL = Mi_SQL + "('" + Obtener_Consecutivo(Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID, Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto);
                        Mi_SQL = Mi_SQL + "','" + Id_Requisicion;
                        Mi_SQL = Mi_SQL + "','" + Clase_Negocio.P_Dt_Productos.Rows[i]["Producto_ID"].ToString().Trim();
                        Mi_SQL = Mi_SQL + "','" + Clase_Negocio.P_Dt_Productos.Rows[i]["Partida_ID"].ToString().Trim();
                        Mi_SQL = Mi_SQL + "', '" + Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Listado_Detalle.Campo_Cantidad].ToString();
                        Mi_SQL = Mi_SQL + "','" + Cls_Sessiones.Nombre_Empleado;
                        Mi_SQL = Mi_SQL + "',SYSDATE";
                        Mi_SQL = Mi_SQL + ",'" + Clase_Negocio.P_Dt_Productos.Rows[i]["Precio_Unitario"].ToString();
                        Mi_SQL = Mi_SQL + "','" + IVA_Producto;
                        Mi_SQL = Mi_SQL + "','" + IEPS_Producto;
                        Mi_SQL = Mi_SQL + "','" + Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Listado_Detalle.Campo_Porcentaje_IVA].ToString();
                        Mi_SQL = Mi_SQL + "','" + Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Listado_Detalle.Campo_Porcentaje_IEPS].ToString();
                        Mi_SQL = Mi_SQL + "','" + Subtotal_Producto;
                        Mi_SQL = Mi_SQL + "','" + Clase_Negocio.P_Dt_Productos.Rows[i][Ope_Com_Listado_Detalle.Campo_Importe].ToString();
                        Mi_SQL = Mi_SQL + "','PRODUCTO";
                        Mi_SQL = Mi_SQL + "','" + Clase_Negocio.P_Dt_Productos.Rows[i]["Clave"].ToString().Trim();
                        Mi_SQL = Mi_SQL + "','" + Clase_Negocio.P_Dt_Productos.Rows[i]["Concepto"].ToString().Trim();
                        Mi_SQL = Mi_SQL + "','" + Clase_Negocio.P_Dt_Productos.Rows[i]["Concepto_ID"].ToString().Trim();
                        Mi_SQL = Mi_SQL + "','" + Clase_Negocio.P_Dt_Productos.Rows[i]["Producto_Nombre"].ToString().Trim();
                        Mi_SQL = Mi_SQL + "'||';'||'" + Clase_Negocio.P_Dt_Productos.Rows[i]["Descripcion"].ToString().Trim() + "')";
                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                        IVA_Acumulado = IVA_Acumulado + IVA_Producto;
                        IEPS_Acumulado = IEPS_Acumulado + IEPS_Producto;
                        //Como el Costo Compra es el precio unitario sinla cantidad se multiplica el presio unitario sin impuesto por la cantidad para obtener el Subtotal
                        Subtotal = Subtotal + Subtotal_Producto;
                        Total = Total + double.Parse(Clase_Negocio.P_Dt_Productos.Rows[i]["Importe"].ToString());
                        //REgresamos a valores ceros los acumulados
                        IVA_Producto = 0;
                        IEPS_Producto = 0;
                        Subtotal_Producto = 0;
                    }

                    IVA_Acumulado = Total - Subtotal;
                    //Actualizamos la requisicion con los nuevos valores de IVA, IEPs y subtotal 
                    Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                            " SET " + Ope_Com_Requisiciones.Campo_IVA + " ='" + IVA_Acumulado.ToString() +
                            "', " + Ope_Com_Requisiciones.Campo_IEPS + "='" + IEPS_Acumulado.ToString() +
                            "', " + Ope_Com_Requisiciones.Campo_Subtotal + "='" + Subtotal.ToString() +
                            "', " + Ope_Com_Requisiciones.Campo_Total + "='" + Total.ToString() +
                            "' WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + "='" + Id_Requisicion + "'";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    //REALIZAMOS EL INSERT EN LA TABLA DE HISTORIAL DE ESTATUS DE REQUISIONES 
                    Cls_Ope_Com_Requisiciones_Negocio Requisicion = new Cls_Ope_Com_Requisiciones_Negocio();
                    Requisicion.Registrar_Historial("EN CONSTRUCCION", Id_Requisicion);
                    Requisicion.Registrar_Historial("GENERADA", Id_Requisicion);
                    Requisicion.Registrar_Historial("AUTORIZADA", Id_Requisicion);

                }//fin del if
            }
            catch(Exception EX)
            {
                Mensaje_Error = EX.Message;
            }

            return Id_Requisicion;
        }//fin de Convertir_Requisicion_Transitoria


        public static DataTable Consultar_Requisiciones_Listado(Cls_Ope_Alm_Requisicion_Listado_Stock_Negocio Clase_Negocio)
        {
            //Consultamos las requisiciones listado 
            String Mi_SQL = "SELECT " + Ope_Com_Requisiciones.Campo_Folio;
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Campo_Total;
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Campo_Estatus;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Com_Listado_Detalle.Campo_No_Requisicion + " FROM ";
            Mi_SQL = Mi_SQL + Ope_Com_Listado_Detalle.Tabla_Ope_Com_Listado_Detalle + " WHERE ";
            Mi_SQL = Mi_SQL + Ope_Com_Listado_Detalle.Campo_No_Listado_ID + "='" + Clase_Negocio.P_Listado_ID.Trim()+ "'";
            Mi_SQL = Mi_SQL + " GROUP BY(" + Ope_Com_Listado_Detalle.Campo_No_Requisicion + "))";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

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
            Mi_Sql = "SELECT NVL(MAX (" + Campo_ID + "),'00000') FROM " + Tabla;
            Obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            Consecutivo = (Convert.ToInt32(Obj) + 1);
            return Consecutivo;
        }

        public static bool Modificar_Listado(Cls_Ope_Alm_Requisicion_Listado_Stock_Negocio Clase_Negocio)
        {
            bool Operacion_Realizada = false;
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "UPDATE " + Ope_Com_Listado.Tabla_Ope_Com_Listado;
                Mi_SQL = Mi_SQL + " SET " + Ope_Com_Listado.Campo_Estatus;
                Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Estatus +"'";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Listado.Campo_Listado_ID;
                Mi_SQL = Mi_SQL + " ='" + Clase_Negocio.P_Listado_ID + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                Operacion_Realizada = true;
            }
            catch
            {
                Operacion_Realizada = false;
            }


            return Operacion_Realizada;
        }

    }//fin del class
}//fin del namespace