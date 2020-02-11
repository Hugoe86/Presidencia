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
using Presidencia.Orden_Compra.Negocio;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Generar_Requisicion.Negocio;
using Presidencia.Administrar_Requisiciones.Negocios;

namespace Presidencia.Orden_Compra.Datos
{
    public class Cls_Ope_Com_Orden_Compra_Datos
    {
        public Cls_Ope_Com_Orden_Compra_Datos()
        {

        }
        #region METODOS
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

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Guardar_Orden_Compra()
        ///DESCRIPCIÓN: Inserta un registro de una Orden de Compra
        ///PARAMETROS: 1.-Objeto de negocio con datos de Orden de Compra
        ///Se debe setear previo la propiedad LISTA_REQUISICIONES
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 19/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Guardar_Orden_Compra(Cls_Ope_Com_Orden_Compra_Negocio Negocio) 
        {
            int No_Proveedores = 0;
            String Mi_Sql = "";
            //datatable de respuesta con las ordenes de compra que se generaron
            DataTable Dt_Orden_Compra = new DataTable("ORDENES_COMPRA");
            DataColumn Dc_Temporal = null;
            Dc_Temporal = new DataColumn("NO_ORDEN_COMPRA", System.Type.GetType("System.String"));
            Dt_Orden_Compra.Columns.Add(Dc_Temporal);
            Dc_Temporal = new DataColumn("FOLIO", System.Type.GetType("System.String"));
            Dt_Orden_Compra.Columns.Add(Dc_Temporal);
            Dc_Temporal = new DataColumn("PROVEEDOR_ID", System.Type.GetType("System.String"));
            Dt_Orden_Compra.Columns.Add(Dc_Temporal);
            Dc_Temporal = new DataColumn("NOMBRE_PROVEEDOR", System.Type.GetType("System.String"));
            Dt_Orden_Compra.Columns.Add(Dc_Temporal);
            Dc_Temporal = new DataColumn("SUBTOTAL", System.Type.GetType("System.String"));
            Dt_Orden_Compra.Columns.Add(Dc_Temporal);
            Dc_Temporal = new DataColumn("IEPS", System.Type.GetType("System.String"));
            Dt_Orden_Compra.Columns.Add(Dc_Temporal);
            Dc_Temporal = new DataColumn("IVA", System.Type.GetType("System.String"));
            Dt_Orden_Compra.Columns.Add(Dc_Temporal);
            Dc_Temporal = new DataColumn("TOTAL", System.Type.GetType("System.String"));
            Dt_Orden_Compra.Columns.Add(Dc_Temporal);
            Dc_Temporal = new DataColumn("LISTA_REQUISICIONES", System.Type.GetType("System.String"));
            Dt_Orden_Compra.Columns.Add(Dc_Temporal);
            DataRow Dr_Temporal = null;
            char[] Ch = {','};            
            //Buscar proveedores
            String[] Requisiciones = Negocio.P_Lista_Requisiciones.Split(Ch);
            //SELECT DISTINCT(PROVEEDOR_ID), NOMBRE_PROVEEDOR FROM OPE_COM_REQ_PRODUCTO 
            //WHERE NO_REQUISICION IN (1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25)
            Mi_Sql = "SELECT DISTINCT(" + Ope_Com_Req_Producto.Campo_Proveedor_ID + "), " +
            Ope_Com_Req_Producto.Campo_Nombre_Proveedor + 
            " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
            " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID +
            " IN (" + Negocio.P_Lista_Requisiciones + ")";
            DataTable Dt_Proveedores = 
                OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql).Tables[0];
            No_Proveedores = Dt_Proveedores.Rows.Count;
            if (No_Proveedores > 1)
            {
                Negocio.P_Mensaje = "2";//hay mas de un proveedor
                return null;
            }
            //Buscar si es de ramo 33 o especial
            Mi_Sql = "SELECT " + Ope_Com_Requisiciones.Campo_Especial_Ramo_33 +             
            " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
            " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
            " IN (" + Negocio.P_Lista_Requisiciones + ")";
            String Especial_Ramo_33 =
                OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql).ToString();



            //Buscar Articulos
            //SELECT * FROM FROM OPE_COM_REQ_PRODUCTO 
            //WHERE WHERE NO_REQUISICION IN (1,2,3,4,5,6)
            Mi_Sql = "SELECT *FROM " +
            Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
            " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID +
            " IN (" + Negocio.P_Lista_Requisiciones + ")";
            DataTable Dt_Articulos =
                OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql).Tables[0];
            int No_Orden_Compra = Obtener_Consecutivo(Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra, Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra);
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

                foreach (DataRow Renglon in Dt_Proveedores.Rows)
                {
                    Dr_Temporal = Dt_Orden_Compra.NewRow();
                    //Calcular Datos 
                    Negocio.P_Subtotal = 0;
                    Negocio.P_Total_IEPS = 0;
                    Negocio.P_Total_IVA = 0;
                    Negocio.P_Total = 0;
                    String Lista_Requisiciones = "";
                    foreach (DataRow Articulo in Dt_Articulos.Rows) 
                    {
                        if (Articulo["PROVEEDOR_ID"].ToString().Trim() == Renglon["PROVEEDOR_ID"].ToString().Trim()) 
                        {
                            String Str_Aux = "0";
                            Str_Aux = Articulo["SUBTOTAL_COTIZADO"].ToString() != null && 
                                        Articulo["SUBTOTAL_COTIZADO"].ToString().Trim().Length > 0 ? 
                                            Articulo["SUBTOTAL_COTIZADO"].ToString().Trim() : "0";                            
                            Negocio.P_Subtotal += Convert.ToDouble(Str_Aux);
                            Str_Aux = Articulo["IEPS_COTIZADO"].ToString() != null && 
                                        Articulo["IEPS_COTIZADO"].ToString().Trim().Length > 0 ? 
                                            Articulo["IEPS_COTIZADO"].ToString().Trim() : "0"; 
                            Negocio.P_Total_IEPS += Convert.ToDouble(Str_Aux);
                            Str_Aux = Articulo["IVA_COTIZADO"].ToString() != null &&
                                        Articulo["IVA_COTIZADO"].ToString().Trim().Length > 0 ?
                                            Articulo["IVA_COTIZADO"].ToString().Trim() : "0";
                            Negocio.P_Total_IVA += Convert.ToDouble(Str_Aux);
                            Str_Aux = Articulo["TOTAL_COTIZADO"].ToString() != null &&
                                        Articulo["TOTAL_COTIZADO"].ToString().Trim().Length > 0 ?
                                            Articulo["TOTAL_COTIZADO"].ToString().Trim() : "0";
                            Negocio.P_Total += Convert.ToDouble(Str_Aux);
                            Lista_Requisiciones += Articulo["NO_REQUISICION"].ToString().Trim() + ",";
                        }
                    }
                    //Quitar el último caracter
                    if (Lista_Requisiciones.Length > 0)
                    {
                        Lista_Requisiciones = Lista_Requisiciones.Substring(0, Lista_Requisiciones.Length - 1);
                    }
                    //Eliminar Requisas repetidas                    
                    String[] Tmp_Requisas = Lista_Requisiciones.Split(Ch);
                    String Respuesta = "";
                    for (int i = 0; i < Tmp_Requisas.Length; i++)
                    {
                        if (!Respuesta.Contains(Tmp_Requisas[i]))
                        {
                            Respuesta += Tmp_Requisas[i] + ",";
                        }
                    }
                    if (Respuesta.Length > 0)
                    {
                        Respuesta = Respuesta.Substring(0, Respuesta.Length - 1);
                    }
                    //formatear fecha
                    DateTime _DateTime = Convert.ToDateTime(Negocio.P_Fecha_Entrega);
                    Negocio.P_Fecha_Entrega = _DateTime.ToString("dd/MM/yy");
                    Lista_Requisiciones = Respuesta;
                    //LLENAR DATOS DE RESPUESTA
                    Dr_Temporal["NO_ORDEN_COMPRA"] = No_Orden_Compra + "";
                    Dr_Temporal["FOLIO"] = "OC-" + No_Orden_Compra;
                    Dr_Temporal["PROVEEDOR_ID"] = Dt_Articulos.Rows[0]["PROVEEDOR_ID"].ToString().Trim();
                    Dr_Temporal["NOMBRE_PROVEEDOR"] = Dt_Articulos.Rows[0]["NOMBRE_PROVEEDOR"].ToString().Trim();
                    Dr_Temporal["SUBTOTAL"] = Negocio.P_Subtotal.ToString();
                    Dr_Temporal["IEPS"] = Negocio.P_Total_IEPS.ToString();
                    Dr_Temporal["IVA"] = Negocio.P_Total_IVA.ToString();
                    Dr_Temporal["TOTAL"] = Negocio.P_Total.ToString();
                    Dr_Temporal["LISTA_REQUISICIONES"] = "";//Lista_Requisiciones;
                    //Guardar una orden de compra por proveedor
                    Mi_Sql = "INSERT INTO " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra +
                    " (" +
                    Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + "," +
                    Ope_Com_Ordenes_Compra.Campo_Folio + "," +
                        //Ope_Com_Ordenes_Compra.Campo_No_Cotizacion + "," +
                    Ope_Com_Ordenes_Compra.Campo_Tipo_Proceso + "," +
                    Ope_Com_Ordenes_Compra.Campo_Estatus + "," +
                    Ope_Com_Ordenes_Compra.Campo_Proveedor_ID + "," +
                    Ope_Com_Ordenes_Compra.Campo_Nombre_Proveedor + "," +
                    Ope_Com_Ordenes_Compra.Campo_Subtotal + "," +
                    Ope_Com_Ordenes_Compra.Campo_Total_IEPS + "," +
                    Ope_Com_Ordenes_Compra.Campo_Total_IVA + "," +
                    Ope_Com_Ordenes_Compra.Campo_Total + "," +
                    Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + "," +
                    Ope_Com_Ordenes_Compra.Campo_Tipo_Articulo + "," +
                        //Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + "," +
                    Ope_Com_Ordenes_Compra.Campo_Fecha_Entrega + "," +
                    Ope_Com_Ordenes_Compra.Campo_Usuario_Creo + "," +
                    Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + "," +
                    Ope_Com_Ordenes_Compra.Campo_Especiales_Ramo33 + "," +

                    Ope_Com_Ordenes_Compra.Campo_Condicion1 + "," +
                    Ope_Com_Ordenes_Compra.Campo_Condicion2 + "," +
                    Ope_Com_Ordenes_Compra.Campo_Condicion3 + "," +
                    Ope_Com_Ordenes_Compra.Campo_Condicion4 + "," +
                    Ope_Com_Ordenes_Compra.Campo_Condicion5 + "," +
                    Ope_Com_Ordenes_Compra.Campo_Condicion6;
                    if (!String.IsNullOrEmpty(Negocio.P_No_Reserva))
                    {
                        Mi_Sql += "," + Ope_Com_Ordenes_Compra.Campo_No_Reserva;
                    }
                    Mi_Sql += ") VALUES (" +
                    No_Orden_Compra + ",'" +
                    "OC-" + No_Orden_Compra + "','" +
                        //Negocio.P_No_Cotizacion + ",'" +
                    Negocio.P_Tipo_Compra + "','" +
                    Negocio.P_Estatus + "','" +
                    Renglon["PROVEEDOR_ID"].ToString().Trim() + "','" +
                    Renglon["NOMBRE_PROVEEDOR"].ToString().Trim() + "'," +
                    Negocio.P_Subtotal + "," +
                    Negocio.P_Total_IEPS + "," +
                    Negocio.P_Total_IVA + "," +
                    Negocio.P_Total + ",'" +
                    Lista_Requisiciones + "','" +
                    Negocio.P_Tipo_Articulo + "','" +
                    Negocio.P_Fecha_Entrega + "','" +
                    Cls_Sessiones.Nombre_Empleado + "'," +
                    "SYSDATE" +
                    ",'" + Especial_Ramo_33 + "','" +
                    Negocio.P_Condicion1 + "','" +
                    Negocio.P_Condicion2 + "','" +
                    Negocio.P_Condicion3 + "','" +
                    Negocio.P_Condicion4 + "','" +
                    Negocio.P_Condicion5 + "','" +
                    Negocio.P_Condicion6 + "'";
                    if (!String.IsNullOrEmpty(Negocio.P_No_Reserva))
                    {
                        Mi_Sql += ",'" + Negocio.P_No_Reserva + "'";
                    }                    
                    Mi_Sql += ")";
                    Comando_SQL.CommandText = Mi_Sql;
                    Comando_SQL.ExecuteNonQuery();                    

                    Mi_Sql = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " SET " +
                    Ope_Com_Requisiciones.Campo_No_Orden_Compra + " = " + No_Orden_Compra +
                    " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + Negocio.P_No_Requisicion;
                    Comando_SQL.CommandText = Mi_Sql;
                    Comando_SQL.ExecuteNonQuery(); 

                    //Guardar detalles de la orden de compra, los articulos
                    Mi_Sql = "UPDATE " +
                    Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                    " SET " + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " = " + No_Orden_Compra +
                    " WHERE " + Ope_Com_Req_Producto.Campo_Proveedor_ID + " = '" +
                    Renglon["PROVEEDOR_ID"].ToString().Trim() + "' AND " +
                    Ope_Com_Req_Producto.Campo_Requisicion_ID + " IN (" + Negocio.P_Lista_Requisiciones + ")";
                    Comando_SQL.CommandText = Mi_Sql;
                    Comando_SQL.ExecuteNonQuery();
                    Dt_Orden_Compra.Rows.Add(Dr_Temporal);
                    No_Orden_Compra++; 
                }
                //Actualizar requisiciones
                Mi_Sql = "UPDATE " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " SET " + Ope_Com_Requisiciones.Campo_Estatus + " = 'COMPRA', " +
                Ope_Com_Requisiciones.Campo_Tipo_Compra + " = '" + Negocio.P_Tipo_Compra + "', " +
                Ope_Com_Requisiciones.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', " +
                Ope_Com_Requisiciones.Campo_Fecha_Modifico + " = SYSDATE" +
                " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " IN (" + Negocio.P_Lista_Requisiciones + ")";
                Comando_SQL.CommandText = Mi_Sql;
                Comando_SQL.ExecuteNonQuery();
                Transaccion_SQL.Commit();
                //Registar Historial de requisición
                Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio = new Cls_Ope_Com_Requisiciones_Negocio();
                Requisicion_Negocio.Registrar_Historial("COMPRA / ORDEN COMPRA ELABORADA",Negocio.P_Lista_Requisiciones);                              
                Cls_Ope_Com_Administrar_Requisiciones_Negocio Administrar_Requisicion =
                    new Cls_Ope_Com_Administrar_Requisiciones_Negocio();
                Administrar_Requisicion.P_Requisicion_ID = Negocio.P_Lista_Requisiciones;
                Administrar_Requisicion.P_Comentario = "Nota: Se generó la Orden de Compra y pasa a validación de Contabilidad";
                Administrar_Requisicion.P_Estatus = "COMPRA";
                Administrar_Requisicion.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToString();
                Administrar_Requisicion.Alta_Observaciones();
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                Dt_Orden_Compra = null;
                throw new Exception("Información: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                Dt_Orden_Compra = null;
                throw new Exception("Los datos fueron actualizados por otro Usuario. Información: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                Dt_Orden_Compra = null;
                throw new Exception("Información: " + Ex.Message);
            }
            finally
            {
                Conexion_Base.Close();
            } 
            return Dt_Orden_Compra;
        }


        public static int Consultar_Numero_Proveedores_De_Requisicion(Cls_Ope_Com_Orden_Compra_Negocio Negocio)
        {
            String Mi_SQL = "";
            Object Objeto = null;
            int No_Proveedores = 0;
            try
            {
                Mi_SQL = "SELECT COUNT (DISTINCT(Proveedor_ID)) NO_PROVEEDORES " +
                " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                " IN (" + Negocio.P_Lista_Requisiciones + ")";
                Objeto = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                No_Proveedores = Convert.ToInt32(Objeto);
            }
            catch(Exception Ex)
            {
                No_Proveedores = 0;
                throw new Exception(Ex.ToString());
            }
            return No_Proveedores;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Ordenes_Compra
        ///DESCRIPCIÓN: Consulta loas ordenes de compra en base a parametros y devuelve un 
        ///DataTable con la informacion solicitada, se le debe setear FechaInicial, FechaFinal 
        ///Estatus
        ///PARAMETROS: 1.-Objeto de Negocio de Orden de Compra
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Ordenes_Compra(Cls_Ope_Com_Orden_Compra_Negocio Negocio)
        {
            Negocio.P_Fecha_Inicial = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Negocio.P_Fecha_Inicial));
            Negocio.P_Fecha_Final = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Negocio.P_Fecha_Final));
            String Mi_Sql = "";
            if (Negocio.P_Folio != null && Negocio.P_Folio != "")
            {
                Negocio.P_Folio = Negocio.P_Folio.Replace("OC-","");
                //Mi_Sql = "SELECT *FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra +
                //" WHERE " + Ope_Com_Ordenes_Compra.Campo_Folio + " = '"k + Negocio.P_Folio + "'";
                Mi_Sql = "SELECT " +
                Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ".*," +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Justificacion_Compra + "," +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Codigo_Programatico + " AS CODIGO" +
                ",(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE " +
                Cat_Dependencias.Campo_Dependencia_ID + " = " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." +
                Ope_Com_Requisiciones.Campo_Dependencia_ID +
                ") AS NOMBRE_DEPENDENCIA," +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                " FROM " +
                Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " JOIN " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " ON " +
                Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_No_Orden_Compra +
                " WHERE " + 
                Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = '" + 
                Negocio.P_Folio + "'";
                Mi_Sql = Mi_Sql + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ".ESPECIALES_RAMO_33" +
                " = '" + "NO" + "'";
            }
            else
            {
                Mi_Sql = "SELECT " +
                Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ".*," +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Justificacion_Compra + "," +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Codigo_Programatico + " AS CODIGO" + 
                ",(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE " +
                Cat_Dependencias.Campo_Dependencia_ID + " = " + 
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + 
                Ope_Com_Requisiciones.Campo_Dependencia_ID +
                ") AS NOMBRE_DEPENDENCIA," +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                " FROM " +
                Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " JOIN " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " ON " +
                Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_No_Orden_Compra +
                " WHERE " +
                "TO_DATE(TO_CHAR(" + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + ",'DD/MM/YY'))" +
                        " >= '" + Negocio.P_Fecha_Inicial + "' AND " +
                "TO_DATE(TO_CHAR(" + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + ",'DD/MM/YY'))" +
                        " <= '" + Negocio.P_Fecha_Final + "'";
                Mi_Sql = Mi_Sql + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ".ESPECIALES_RAMO_33" +
                " = '" + "NO" + "'";

                if (!String.IsNullOrEmpty(Negocio.P_Estatus))
                {
                    Mi_Sql = Mi_Sql + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." +
                    Ope_Com_Ordenes_Compra.Campo_Estatus + " IN ('" + Negocio.P_Estatus + "')";
                }
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Ordenes_Compra
        ///DESCRIPCIÓN: Consulta loas ordenes de compra en base a parametros y devuelve un 
        ///DataTable con la informacion solicitada, se le debe setear FechaInicial, FechaFinal 
        ///Estatus
        ///PARAMETROS: 1.-Objeto de Negocio de Orden de Compra
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Ordenes_Compra_Especiales(Cls_Ope_Com_Orden_Compra_Negocio Negocio)
        {
            Negocio.P_Fecha_Inicial = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Negocio.P_Fecha_Inicial));
            Negocio.P_Fecha_Final = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Negocio.P_Fecha_Final));
            String Mi_Sql = "";
            if (Negocio.P_Folio != null && Negocio.P_Folio != "")
            {
                Negocio.P_Folio = Negocio.P_Folio.Replace("OC-", "");
                //Mi_Sql = "SELECT *FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra +
                //" WHERE " + Ope_Com_Ordenes_Compra.Campo_Folio + " = '"k + Negocio.P_Folio + "'";
                Mi_Sql = "SELECT " +
                Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ".*," +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Justificacion_Compra + "," +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Codigo_Programatico + " AS CODIGO" +
                ",(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE " +
                Cat_Dependencias.Campo_Dependencia_ID + " = " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." +
                Ope_Com_Requisiciones.Campo_Dependencia_ID +
                ") AS NOMBRE_DEPENDENCIA," +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                " FROM " +
                Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " JOIN " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " ON " +
                Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_No_Orden_Compra +
                " WHERE " +
                Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = '" +
                Negocio.P_Folio + "'";
                Mi_Sql = Mi_Sql + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ".ESPECIALES_RAMO_33" +
                " = '" + "SI" + "'";
            }
            else
            {
                Mi_Sql = "SELECT " +
                Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ".*," +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Justificacion_Compra + "," +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Codigo_Programatico + " AS CODIGO" +
                ",(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE " +
                Cat_Dependencias.Campo_Dependencia_ID + " = " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." +
                Ope_Com_Requisiciones.Campo_Dependencia_ID +
                ") AS NOMBRE_DEPENDENCIA," +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                " FROM " +
                Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " JOIN " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " ON " +
                Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_No_Orden_Compra +
                " WHERE " +
                "TO_DATE(TO_CHAR(" + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + ",'DD/MM/YY'))" +
                        " >= '" + Negocio.P_Fecha_Inicial + "' AND " +
                "TO_DATE(TO_CHAR(" + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + ",'DD/MM/YY'))" +
                        " <= '" + Negocio.P_Fecha_Final + "'";
                Mi_Sql = Mi_Sql + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ".ESPECIALES_RAMO_33" +
                " = '" + "SI" + "'";

                if (!String.IsNullOrEmpty(Negocio.P_Estatus))
                {
                    Mi_Sql = Mi_Sql + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." +
                    Ope_Com_Ordenes_Compra.Campo_Estatus + " IN ('" + Negocio.P_Estatus + "')";
                }
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Requisiciones_Directas
        ///DESCRIPCIÓN: Consulta las Requisiciones confirmadas y que son para compra directa
        ///y devuelve DataTable con la informacion solicitada
        ///PARAMETROS: 1.-Objeto de Negocio de Orden de Compra
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Requisiciones_Directas(Cls_Ope_Com_Orden_Compra_Negocio Negocio)
        {
            String Mi_Sql = "";

            if (Negocio.P_Folio != null && Negocio.P_Folio != "")
            {
                Mi_Sql = "SELECT " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ".*," +
                "(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + " = " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID + ") AS UNIDAD_RESPONSABLE" +
                " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " WHERE " + Ope_Com_Requisiciones.Campo_Folio + " = '" + Negocio.P_Folio + "'";
            }
            else
            {
                Mi_Sql = "SELECT " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ".*," +
                "(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + " = " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID + ") AS UNIDAD_RESPONSABLE" +
                " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " WHERE " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Estatus + " = 'CONFIRMADA'";
                if (Negocio.P_Cotizador_ID != null && Negocio.P_Cotizador_ID != "0")
                {
                    Mi_Sql = Mi_Sql + " AND " +
                    Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." +
                        Ope_Com_Requisiciones.Campo_Cotizador_ID + " ='" + Negocio.P_Cotizador_ID + "'";
                }
                //" AND " +
                //Ope_Com_Requisiciones.Campo_Tipo_Compra + " = 'COMPRA DIRECTA'" +
                Mi_Sql = Mi_Sql + " ORDER BY " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + 
                    Ope_Com_Requisiciones.Campo_Requisicion_ID + " ASC";
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Requisiciones_Para_Dividir
        ///DESCRIPCIÓN: Consulta las Requisiciones confirmadas y que son para compra directa
        ///y devuelve DataTable con la informacion solicitada
        ///PARAMETROS: 1.-Objeto de Negocio de Orden de Compra
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 15 marzo 2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Requisiciones_Para_Dividir(Cls_Ope_Com_Orden_Compra_Negocio Negocio)
        {

            
            //DataTable Dt_Proveedores =
              //  OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];



            String Mi_Sql = "";

            if (Negocio.P_Folio != null && Negocio.P_Folio != "")
            {
                Mi_Sql = "SELECT " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ".*," +
                "(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + " = " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID + ") AS UNIDAD_RESPONSABLE" +
                " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " WHERE " + Ope_Com_Requisiciones.Campo_Folio + " = '" + Negocio.P_Folio + "'";
                Mi_Sql += " AND " +
                "(SELECT COUNT (DISTINCT(" + Ope_Com_Req_Producto.Campo_Proveedor_ID + ")) " +
                " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                " = " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + ") > 1 ";
            }
            else
            {
                Mi_Sql = "SELECT " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ".*," +
                "(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + " = " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID + ") AS UNIDAD_RESPONSABLE" +
                " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " WHERE " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Estatus + " = 'CONFIRMADA'";
                
                Mi_Sql+= " AND " +
                "(SELECT COUNT (DISTINCT(" + Ope_Com_Req_Producto.Campo_Proveedor_ID + ")) " +               
                " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                " = " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + ") > 1 ";
                
                
                if (Negocio.P_Cotizador_ID != null && Negocio.P_Cotizador_ID != "0")
                {
                    Mi_Sql = Mi_Sql + " AND " +
                    Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." +
                        Ope_Com_Requisiciones.Campo_Cotizador_ID + " ='" + Negocio.P_Cotizador_ID + "'";
                }
                //" AND " +
                //Ope_Com_Requisiciones.Campo_Tipo_Compra + " = 'COMPRA DIRECTA'" +
                Mi_Sql = Mi_Sql + " ORDER BY " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." +
                    Ope_Com_Requisiciones.Campo_Requisicion_ID + " ASC";
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Licitaciones
        ///DESCRIPCIÓN: Consulta las Licitaciones que tienen estatus de TERMINADA
        ///y devuelve DataTable con la informacion solicitada
        ///PARAMETROS: 1.-Objeto de Negocio de Orden de Compra
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Licitaciones(Cls_Ope_Com_Orden_Compra_Negocio Negocio)
        {
            String Mi_Sql = "";
            if (Negocio.P_Folio != null && Negocio.P_Folio != "")
            {
                Mi_Sql = "SELECT *FROM " + Ope_Com_Licitaciones.Tabla_Ope_Com_Licitaciones +
                " WHERE " + Ope_Com_Licitaciones.Campo_Folio + " = '" + Negocio.P_Folio + "'";
            }
            else
            {
                Mi_Sql = "SELECT *FROM " + Ope_Com_Licitaciones.Tabla_Ope_Com_Licitaciones +
                " WHERE " +
                Ope_Com_Licitaciones.Campo_Estatus + " IN ('" + Negocio.P_Estatus + "')" +
                " ORDER BY " + Ope_Com_Licitaciones.Campo_No_Licitacion + " DESC";
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Comite_Compras
        ///DESCRIPCIÓN: Consulta Comite de Compra que tienen estatus de TERMINADA
        ///y devuelve DataTable con la informacion solicitada
        ///PARAMETROS: 1.-Objeto de Negocio de Orden de Compra
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Comite_Compras(Cls_Ope_Com_Orden_Compra_Negocio Negocio)
        {
            String Mi_Sql = "";
            if (Negocio.P_Folio != null && Negocio.P_Folio != "")
            {
                Mi_Sql = "SELECT *FROM " + Ope_Com_Comite_Compras.Tabla_Ope_Com_Comite_Compras +
                " WHERE " + Ope_Com_Comite_Compras.Campo_Folio + " = '" + Negocio.P_Folio + "'";
            }
            else
            {
                Mi_Sql = "SELECT *FROM " + Ope_Com_Comite_Compras.Tabla_Ope_Com_Comite_Compras +
                " WHERE " +
                Ope_Com_Comite_Compras.Campo_Estatus + " IN ('" + Negocio.P_Estatus + "')" +
                " ORDER BY " + Ope_Com_Comite_Compras.Campo_No_Comite_Compras + " DESC";
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cotizaciones
        ///DESCRIPCIÓN: Consulta Cotizaciones que tienen estatus de TERMINADA
        ///y devuelve DataTable con la informacion solicitada
        ///PARAMETROS: 1.-Objeto de Negocio de Orden de Compra
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Cotizaciones(Cls_Ope_Com_Orden_Compra_Negocio Negocio)
        {
            String Mi_Sql = "";
            if (Negocio.P_Folio != null && Negocio.P_Folio != "")
            {
                Mi_Sql = "SELECT *FROM " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones +
                " WHERE " + Ope_Com_Cotizaciones.Campo_Folio + " = '" + Negocio.P_Folio + "'";
            }
            else
            {
                Mi_Sql = "SELECT *FROM " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones +
                " WHERE " +
                Ope_Com_Cotizaciones.Campo_Estatus + " IN ('" + Negocio.P_Estatus + "')" +
                " ORDER BY " + Ope_Com_Cotizaciones.Campo_No_Cotizacion + " DESC";
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
        ///NOMBRE DE LA FUNCIÓN: Actualizar_Orden_Compra
        ///DESCRIPCIÓN: Actualiza las ordenes de compra
        ///y devuelve DataTable con la informacion solicitada
        ///PARAMETROS: 1.-Objeto de Negocio de Orden de Compra
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Actualizar_Orden_Compra(Cls_Ope_Com_Orden_Compra_Negocio Negocio)
        {
            String Mi_Sql = "";
            Mi_Sql = "UPDATE " +
            Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra +
            " SET " +
            Ope_Com_Ordenes_Compra.Campo_Estatus + " = '" + Negocio.P_Estatus + "', " +
            Ope_Com_Ordenes_Compra.Campo_Comentarios + " = '" + Negocio.P_Comentarios + "', " +
            Ope_Com_Ordenes_Compra.Campo_No_Reserva + " = '" + Negocio.P_No_Reserva + "'" +
            " WHERE " + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = " + Negocio.P_No_Orden_Compra;
            int Registros_Afectados = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            return Registros_Afectados;
        }
        public static int Actualizar_Requisicion(Cls_Ope_Com_Orden_Compra_Negocio Negocio)
        {
            String Mi_Sql = "";
            Mi_Sql = "UPDATE " +
            Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
            " SET " +
            Ope_Com_Requisiciones.Campo_Estatus + " = '" + Negocio.P_Estatus + "' " +
            
            " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + Negocio.P_Lista_Requisiciones;
            int Registros_Afectados = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            return Registros_Afectados;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Actualizar_Impresion
        ///DESCRIPCIÓN: Actualiza las ordenes de compra
        ///y devuelve DataTable con la informacion solicitada
        ///PARAMETROS: 1.-Objeto de Negocio de Orden de Compra
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Actualizar_Impresion(Cls_Ope_Com_Orden_Compra_Negocio Negocio)
        {
            int Registros_Afectados = 0;
            try
            {
                String Mi_Sql = "";
                Mi_Sql = "UPDATE " +
                Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra +
                " SET " +
                Ope_Com_Ordenes_Compra.Campo_Impresa + " = 'SI'" +
                " WHERE " + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = " + Negocio.P_No_Orden_Compra;
                Registros_Afectados = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            }
            catch (Exception Ex)
            {
                Registros_Afectados = 0;
                throw new Exception(Ex.ToString());
            }
            return Registros_Afectados;
        }
          
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Comentarios_De_Orden_Compra
        ///DESCRIPCIÓN: Actualiza las ordenes de compra
        ///y devuelve DataTable con la informacion solicitada
        ///PARAMETROS: 1.-Objeto de Negocio de Orden de Compra
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 22 marzo 2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static String Consultar_Comentarios_De_Orden_Compra(Cls_Ope_Com_Orden_Compra_Negocio Negocio)
        {
            String Comentarios = "";
            try
            {
                String Mi_Sql = "SELECT NVL(COMENTARIOS,'SIN COMENTARIOS') FROM OPE_COM_ORDENES_COMPRA WHERE NO_ORDEN_COMPRA  = " + Negocio.P_No_Orden_Compra;
                Object Objeto = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                Comentarios = Objeto.ToString();
            }
            catch (Exception Ex)
            {            
                throw new Exception(Ex.ToString());
            }
            return Comentarios;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Dias_Plazo
        ///DESCRIPCIÓN: Consulta las Requisiciones confirmadas y que son para compra directa
        ///y devuelve DataTable con la informacion solicitada
        ///PARAMETROS: 1.-Objeto de Negocio de Orden de Compra
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Consultar_Dias_Plazo()
        {
            int Respuesta = 0;
            String Mi_Sql = "SELECT " +
            Cat_Com_Parametros.Campo_Plazo_Surtir_Orden_Compra +
            " FROM " + Cat_Com_Parametros.Tabla_Cat_Com_Parametros;
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            DataTable _DataTable = null;
            if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
            {
                try {
                    String Dato = 
                    _DataSet.Tables[0].Rows[0][Cat_Com_Parametros.Campo_Plazo_Surtir_Orden_Compra].ToString().Trim();
                    Respuesta = int.Parse(Dato);
                } 
                catch (Exception e)
                {
                    e.ToString();
                    Respuesta = 0;
                }
            }
            return Respuesta;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Dias_Entrega_Proveedor
        ///DESCRIPCIÓN:
        ///PARAMETROS: 1.-Objeto de Negocio de Orden de Compra
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 12 Oct 2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Consultar_Dias_Entrega_Proveedor(Cls_Ope_Com_Orden_Compra_Negocio Negocio)
        {
            //SELECT TIEMPO_ENTREGA FROM OPE_COM_PROPUESTA_COTIZACION WHERE RESULTADO='ACEPTADO' AND NO_REQUISICION=16
            int Respuesta = 0;
            String Mi_Sql = "SELECT " +
            Ope_Com_Propuesta_Cotizacion.Campo_Tiempo_Entrega +
            " FROM " +
            Ope_Com_Propuesta_Cotizacion.Tabla_Ope_Com_Propuesta_Cotizacion +
            " WHERE " +
            Ope_Com_Propuesta_Cotizacion.Campo_Resultado + "='ACEPTADA'" +
            " AND " +
                Ope_Com_Propuesta_Cotizacion.Campo_No_Requisicion + " = " + Negocio.P_No_Requisicion;
            Object Objeto = null;
            int Dias_Entrega = 0;
            try
            {
                Objeto = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                Dias_Entrega = int.Parse(Objeto.ToString());            
            }
            catch (Exception e)
            {
                e.ToString();
                Dias_Entrega = 0;
            }
            return Dias_Entrega;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cotizadores
        ///DESCRIPCIÓN:
        ///PARAMETROS: 1.-Objeto de Negocio de Orden de Compra
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 14 Oct 2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Cotizadores(Cls_Ope_Com_Orden_Compra_Negocio Negocio)
        {
            //SELECT TIEMPO_ENTREGA FROM OPE_COM_PROPUESTA_COTIZACION WHERE RESULTADO='ACEPTADO' AND NO_REQUISICION=16
            DataTable Dt_Cotizadores = null;
            String Mi_Sql = "SELECT " +
            Cat_Com_Cotizadores.Campo_Empleado_ID + "," +
            Cat_Com_Cotizadores.Campo_Nombre_Completo +
            " FROM " +
            Cat_Com_Cotizadores.Tabla_Cat_Com_Cotizadores;
            try
            {
                Dt_Cotizadores = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql).Tables[0];                
            }
            catch (Exception e)
            {
                Dt_Cotizadores = null;
                throw new Exception(e.ToString());
            }
            return Dt_Cotizadores;
        }

        public static int Actualizar_Descripcion_Productos_OC(Cls_Ope_Com_Orden_Compra_Negocio Negocio)
        {
            String No_Orden_Compra = Negocio.P_No_Orden_Compra.ToString();
            DataTable Dt_Productos = Negocio.P_Dt_Detalles_Orden_Compra;
            String Condiciones = Negocio.P_Condicion1;
            int Rows_Actualizados = 0;
            String Mi_SQL = "";
            //actualizar condiciones
            Mi_SQL = "UPDATE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra +
            " SET " + Ope_Com_Ordenes_Compra.Campo_Condicion1 + " = '" + Condiciones + "' WHERE " +
            Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = " + No_Orden_Compra;
            Rows_Actualizados += OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            //actualizar productos
            if (Dt_Productos != null && Dt_Productos.Rows.Count > 0)
            {
                foreach(DataRow Producto in Dt_Productos.Rows)
                {
                    Mi_SQL = "UPDATE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " SET " +
                    Ope_Com_Req_Producto.Campo_Marca_OC + " = '" + Producto["MARCA"].ToString() + "', " +
                    Ope_Com_Req_Producto.Campo_Nombre_Prod_Serv_Orden_Compra + " = '" + Producto["PRODUCTO"].ToString() + "' " +
                    " WHERE " + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID + " = " +
                    Producto[Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID].ToString();
                    Rows_Actualizados += OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
            }
            return Rows_Actualizados;
        }
        #endregion
    }
}