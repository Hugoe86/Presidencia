using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Requisiciones_Parciales.Negocio;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Bitacora_Eventos;
using Presidencia.Generar_Requisicion.Negocio;
using Presidencia.Generar_Requisicion.Datos;

namespace Presidencia.Requisiciones_Parciales.Datos
{
    public class Cls_Ope_Com_Alm_Requisiciones_Parciales_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Requisiciones_Parciales
        ///DESCRIPCIÓN:          Método utilizado para consultar las requisiciones parciales de stock de almacén
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           27/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Requisiciones_Parciales(Cls_Ope_Com_Alm_Requisiciones_Parciales_Negocio Datos)
        {
            // Declaración de variables
            String Mi_SQL = null;
            DataTable Dt_Requisiciones = new DataTable();

            Mi_SQL = "SELECT " + "REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + ""; // NO_REQUISICION
            Mi_SQL = Mi_SQL + ", DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + " as UNIDAD_RESPONSABLE";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio + "";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion + " as FECHA";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Total + " as MONTO";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Estatus + " as ESTATUS";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
            Mi_SQL = Mi_SQL + " ON REQUISICIONES." + Ope_Com_Requisiciones.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + " WHERE  REQUISICIONES." + Ope_Com_Requisiciones.Campo_Tipo + "= '" + "STOCK'";
            Mi_SQL = Mi_SQL + " and  REQUISICIONES." + Ope_Com_Requisiciones.Campo_Estatus + " IN ('PARCIAL','ALMACEN')"; //" IN ('PARCIAL','ALMACEN','EN CONSTRUCCION','GENERADA','AUTORIZADA','RECHAZADA')";

            if (Datos.P_No_Requisicion != null)
            {
                Mi_SQL = Mi_SQL + "AND REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " like '%" + Datos.P_No_Requisicion + "%'";
            }

            if ((Datos.P_Fecha_Inicial != null) && (Datos.P_Fecha_Final != null))
            {
                Mi_SQL = Mi_SQL + " AND TO_DATE(TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion+ ",'DD/MM/YY')) BETWEEN '" + Datos.P_Fecha_Inicial + "'" +
                " AND '" + Datos.P_Fecha_Final + "'";
            }

            Mi_SQL = Mi_SQL + " order by REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID;

            Dt_Requisiciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Requisiciones;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Detalles_Requisiciones
        ///DESCRIPCIÓN:          Método utilizado para consultar los detalles de la requisicion
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           27/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Detalles_Requisicion(Cls_Ope_Com_Alm_Requisiciones_Parciales_Negocio Datos)
        {
            // Declaración de variables
            String Mi_SQL = null;
            DataTable Dt_Requisicion = new DataTable();

            Mi_SQL = "SELECT " + "REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + ""; //NO_REQUISICION
            Mi_SQL = Mi_SQL + ", DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + " as UNIDAD_RESPONSABLE";
            Mi_SQL = Mi_SQL + ", DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID + " as UNIDAD_RESPONSABLE_ID";

            Mi_SQL = Mi_SQL + ",( SELECT AREAS." + Cat_Areas.Campo_Nombre + " FROM " + Cat_Areas.Tabla_Cat_Areas + " AREAS ";
            Mi_SQL = Mi_SQL + " WHERE AREAS."+  Cat_Areas.Campo_Area_ID + " = REQUISICIONES." + Ope_Com_Requisiciones.Campo_Area_ID;
            Mi_SQL = Mi_SQL  + " AND REQUISICIONES."+ Ope_Com_Requisiciones.Campo_Requisicion_ID + " = '" + Datos.P_No_Requisicion + "' ) as AREA ";
            Mi_SQL = Mi_SQL + ",( SELECT PARTIDA." + Cat_Sap_Partidas_Especificas.Campo_Nombre + " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " PARTIDA ";
            Mi_SQL = Mi_SQL + " WHERE PARTIDA." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " = REQUISICIONES." + Ope_Com_Requisiciones.Campo_Partida_ID;
            Mi_SQL = Mi_SQL + " AND REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = '" + Datos.P_No_Requisicion + "' ) as PARTIDA ";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio + "";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion + " as FECHA";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Justificacion_Compra + " as COMENTARIOS";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_IVA + "";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Subtotal + "";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Total + "";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
            Mi_SQL = Mi_SQL + " ON REQUISICIONES." + Ope_Com_Requisiciones.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + " WHERE  REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "= '" + Datos.P_No_Requisicion + "'";

            Dt_Requisicion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Requisicion;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Productos_Requisicion_Parcial
        ///DESCRIPCIÓN:          Método utilizado para consultar los productos de las requisiciones parciales de stock de almacén
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           27/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Productos_Requisicion_Parcial(Cls_Ope_Com_Alm_Requisiciones_Parciales_Negocio Datos) // ESTE ES EL METODO
        {
            // Declaración de variables
            String Mi_SQL = null;
            DataTable Dt_Detalles = new DataTable();

            Mi_SQL = Mi_SQL + " SELECT REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " as PRODUCTO_ID ";
            Mi_SQL = Mi_SQL + ",(select PRODUCTOS." + Cat_Com_Productos.Campo_Clave + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = PRODUCTOS.";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as CLAVE ";

            Mi_SQL = Mi_SQL + ",(select UNIDADES." + Cat_Com_Unidades.Campo_Abreviatura + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " UNIDADES ";
            Mi_SQL = Mi_SQL + " where  UNIDADES." + Cat_Com_Unidades.Campo_Unidad_ID + " = ";
            Mi_SQL = Mi_SQL + " (select PRODUCTOS." + Cat_Com_Productos.Campo_Unidad_ID + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where  REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = ";
            Mi_SQL = Mi_SQL + " PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID + ")) as UNIDADES ";

            Mi_SQL = Mi_SQL + ",(select PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = PRODUCTOS.";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as PRODUCTO ";

            Mi_SQL = Mi_SQL + ",(select PRODUCTOS." + Cat_Com_Productos.Campo_Descripcion + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = PRODUCTOS.";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as DESCRIPCION ";

            Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Cantidad + " as CANTIDAD_SOLICITADA  "; // Cantidad Solicitada
            Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Cantidad_Entregada + " ";               // Cantidad Entregada
            Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Precio_Unitario + " as PRECIO ";           
            Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Partida_ID+ " ";
            Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Porcentaje_IVA + " ";
            
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO";
            Mi_SQL = Mi_SQL + " WHERE REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = " + Datos.P_No_Requisicion.Trim();
           
            Dt_Detalles = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Detalles;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Productos_Requisicion_Parcial
        ///DESCRIPCIÓN:          Método utilizado para consultar los productos de las requisiciones parciales de stock de almacén
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           27/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Pragrama_Financiamiento(Cls_Ope_Com_Alm_Requisiciones_Parciales_Negocio Datos)
        {
            DataTable Dt_Consulta = new DataTable();
            String Mi_SQL = null;

            Mi_SQL = " SELECT DISTINCT" + " PROYECTOS_PROGRAMAS." + Cat_Com_Proyectos_Programas.Campo_Descripcion + " as PROYECTO_PROGRAMA ";
            Mi_SQL = Mi_SQL + ", PROYECTOS_PROGRAMAS." + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + " ";
            Mi_SQL = Mi_SQL + ", FINANCIAMIENTO." + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + " as FINANCIAMIENTO ";
            Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + " PROYECTOS_PROGRAMAS ";
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO ";
            Mi_SQL = Mi_SQL + ", " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + " FINANCIAMIENTO ";
            Mi_SQL = Mi_SQL + " WHERE REQ_PRODUCTO. " + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID + " = ";
            Mi_SQL = Mi_SQL + " PROYECTOS_PROGRAMAS. " + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + " ";
            Mi_SQL = Mi_SQL + " AND REQ_PRODUCTO. " + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID + " = ";
            Mi_SQL = Mi_SQL + " FINANCIAMIENTO. " + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + " ";
            Mi_SQL = Mi_SQL + " AND REQ_PRODUCTO. " + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + " '" + Datos.P_No_Requisicion + "' ";

            Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Consulta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_DataTable
        ///DESCRIPCIÓN:          Método utilizado para consultar loas dependencias y las áreas
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           18/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_DataTable(Cls_Ope_Com_Alm_Requisiciones_Parciales_Negocio Datos)
        {
            // Declaración de Variables
            String Mi_SQL = null;
            DataSet Ds_Consulta = null;
            DataTable Dt_consulta = new DataTable();

            try
            {
                if (Datos.P_Tipo_Data_Table.Equals("DEPENDENCIAS"))
                {
                    Mi_SQL = "SELECT " + Cat_Dependencias.Campo_Dependencia_ID + " AS DEPENDENCIA_ID, " + Cat_Dependencias.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias;
                }
                else if (Datos.P_Tipo_Data_Table.Equals("AREAS"))
                {
                    Mi_SQL = "SELECT " + Cat_Areas.Campo_Area_ID + " AS AREA_ID, " + Cat_Areas.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Areas.Tabla_Cat_Areas;
                }
                
                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
                {
                    Ds_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Consulta != null)
                {
                    Dt_consulta= Ds_Consulta.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_consulta;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Liberar_Requisiciones
        ///DESCRIPCIÓN:          Método utilizado para liberar las requisiciones seleccionadas por el usuario
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           27/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static String Cancelar_Requisicion(Cls_Ope_Com_Alm_Requisiciones_Parciales_Negocio Datos)
        {
            return Cls_Ope_Com_Requisiciones_Datos.Cancelar_Requisicion_Stock_Parcial(Datos.P_No_Requisicion, Cls_Sessiones.Nombre_Empleado, Datos.P_Observaciones);
            //// Declaración de Variables
            //String Mi_SQL = null;           
            //String Dependencia_ID = "";
            //String Proyecto_Programa_ID = "";
            //String Partida_Producto_ID = "";
            //String No_Asignacion= "";
            //Double Monto_Disponible = 0;
            //Double Monto_Comprometido = 0;

            //Int32 Cantidad_Comprometida=0;
            //Int32 Cantidad_Disponible = 0;
            //Int32 Cantidad_Productos_Cancelar = 0;
            //// Declaracion de variables
            //OracleTransaction Obj_Transaccion = null;
            //OracleConnection Obj_Conexion;
            //OracleCommand Obj_Comando;
            //String Mensaje = String.Empty; //Variable para el mensaje de error
            //Object Aux; // Variable auxiliar para las consultas
            //OracleDataAdapter Obj_Adaptador; //Adapatador para el llenado de las tablas

            //DataTable Dt_Productos_A_Cancelar= new DataTable();
            //DataTable Dt_Aux;
           
            //// Validacion
            //if (Datos.P_Estatus == "PARCIAL")
            //{
            //    Datos.P_Estatus = "LIBERADA";
            //}
            //else 
            //{
            //    Datos.P_Estatus = "CANCELADA";
            //}

            //try
            //{
            //        if (Datos.P_Dt_Productos_Cancelar.Rows.Count > 0) // Si la tabla tiene valores
            //        {
            //             Dt_Productos_A_Cancelar = Datos.P_Dt_Productos_Cancelar; 

            //             Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
            //             Obj_Comando = new OracleCommand();
            //             Obj_Adaptador = new OracleDataAdapter();
            //             Obj_Conexion.Open();
            //             Obj_Transaccion = Obj_Conexion.BeginTransaction();
            //             Obj_Comando.Transaction = Obj_Transaccion;
            //             Obj_Comando.Connection = Obj_Conexion;

            //             //** ACTUALIZAR REQUISICIÓN A LIBERADA
            //             // Consulta para la actualizacion del estatus de la requisicion 
            //             Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " ";
            //             Mi_SQL = Mi_SQL + "SET " + Ope_Com_Requisiciones.Campo_Estatus + " ='" + Datos.P_Estatus + "'";
            //             Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + Datos.P_No_Requisicion + " ";

            //             //Ejecutar consulta
            //             Obj_Comando.CommandText = Mi_SQL;
            //             Obj_Comando.ExecuteNonQuery();


            //             // Se Guarda el Historial de la requisición
            //             Cls_Ope_Com_Requisiciones_Negocio Requisiciones = new Cls_Ope_Com_Requisiciones_Negocio();
            //             Requisiciones.Registrar_Historial(Datos.P_Estatus, Datos.P_No_Requisicion);

            //            // Se guarda el cambio en la tabla utilizada para darle seguimiento a las requisiciones que se cancelan

            //             if (Dt_Productos_A_Cancelar.Rows.Count > 0) // Si la tabla contiene productos
            //             {
            //                 for (int i = 0; i < Dt_Productos_A_Cancelar.Rows.Count; i++) // For para el recorrido de los productos por cada requisición
            //                 {
            //                     //** LIBERAR MONTOS
            //                     Double Monto_Total = Convert.ToDouble(Dt_Productos_A_Cancelar.Rows[i]["TOTAL"].ToString().Trim());

            //                     Partida_Producto_ID = Dt_Productos_A_Cancelar.Rows[i]["PARTIDA_ID"].ToString(); // Se consulta la partida del producto
            //                     Proyecto_Programa_ID = Datos.P_Proyecto_Programa_ID.ToString().Trim();
            //                     Dependencia_ID = Datos.P_Dependencia_ID.ToString().Trim();

            //                     // Consulta para obtener el mayor numero de asignación
            //                     //Mi_SQL = "SELECT  MAX (" + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ") ";
            //                     //Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
            //                     //Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID;
            //                     //Mi_SQL = Mi_SQL + " = '" + Dependencia_ID + "'";
            //                     //Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Partida_ID;
            //                     //Mi_SQL = Mi_SQL + " = '" + Partida_Producto_ID + "'";
            //                     //Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID;
            //                     //Mi_SQL = Mi_SQL + " = '" + Proyecto_Programa_ID + "'";
            //                     //Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto;
            //                     //Mi_SQL = Mi_SQL + " = extract(year from sysdate)";
            //                     //@
            //                     //Mi_SQL = Mi_SQL + " = 2011";

            //                     // Ejecutar consulta
            //                     //Obj_Comando.CommandText = Mi_SQL;
            //                     //Aux = Obj_Comando.ExecuteScalar();

            //                     //// Verificar si es nulo
            //                     //if (Convert.IsDBNull(Aux) == false)
            //                     //    No_Asignacion = Aux.ToString().Trim();

            //                     No_Asignacion = "1"; 
            //                     // Consulta para obtener los  montos 
            //                     Mi_SQL = "SELECT  " + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + ", ";
            //                     Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + "";
            //                     Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
            //                     Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID;
            //                     Mi_SQL = Mi_SQL + " = '" + Dependencia_ID + "'";
            //                     Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Partida_ID;
            //                     Mi_SQL = Mi_SQL + " = '" + Partida_Producto_ID + "'";
            //                     Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID;
            //                     Mi_SQL = Mi_SQL + " = '" + Proyecto_Programa_ID + "'";
            //                     Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto;
            //                     Mi_SQL = Mi_SQL + " = extract(year from sysdate)";
            //                     Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio;
            //                     Mi_SQL = Mi_SQL + " = " + No_Asignacion;

            //                     DataTable Dt_Aux_Presupuestos = new DataTable(); // Se crea la tabla para guardar los presupuestos
            //                     // Ejecutar consulta;
            //                     Obj_Comando.CommandText = Mi_SQL;

            //                     Obj_Adaptador.SelectCommand = Obj_Comando;
            //                     Obj_Adaptador.Fill(Dt_Aux_Presupuestos);

            //                     // Verificar si la consulta tiene elementos
            //                     if (Dt_Aux_Presupuestos.Rows.Count > 0)
            //                     {
            //                         if (Convert.IsDBNull(Dt_Aux_Presupuestos.Rows[0]["MONTO_DISPONIBLE"]) != false) // Si no tiene un monto ejercido entra
            //                             Monto_Disponible = Monto_Total; // Obtener el nuevo monto disponible 
            //                         else
            //                             Monto_Disponible = (Convert.ToDouble(Dt_Aux_Presupuestos.Rows[0]["MONTO_DISPONIBLE"]) + Monto_Total);// Obtener el  monto ejercido y lo suma al monto Total del producto

            //                         if (Convert.IsDBNull(Dt_Aux_Presupuestos.Rows[0]["MONTO_COMPROMETIDO"]) == false)
            //                             Monto_Comprometido = (Convert.ToDouble(Dt_Aux_Presupuestos.Rows[0]["MONTO_COMPROMETIDO"]) - Monto_Total); // Obtener el  MONTO COMPROMETIDO y le resta el MONTO TOTAL del producto
            //                         else
            //                             Monto_Comprometido = 0;

            //                         // Actualizar la tabla de los presupuestos
            //                         Mi_SQL = " UPDATE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + " ";
            //                         Mi_SQL = Mi_SQL + " SET " + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + " = " + Monto_Disponible + ", ";
            //                         Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + " = " + Monto_Comprometido + " ";
            //                         Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + " = '" + Dependencia_ID + "'";
            //                         Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID + " = '" + Partida_Producto_ID + "'";
            //                         Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + " = '" + Proyecto_Programa_ID + "'";
            //                         Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + " = " + "extract(year from sysdate)";
            //                         //" 2011 ";//" = extract(year from sysdate)";//@
            //                         Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + " = " + No_Asignacion;

            //                         //Ejecutar consulta
            //                         Obj_Comando.CommandText = Mi_SQL;
            //                         Obj_Comando.ExecuteNonQuery(); // Se ejecuta la operación 
            //                     }

            //                     //** LIBERAR PRODUCTOS
            //                     //Consulta para el obtener el comprometido y el disponible
            //                     Mi_SQL = "SELECT " + Cat_Com_Productos.Campo_Comprometido + ", " + Cat_Com_Productos.Campo_Disponible + " ";
            //                     Mi_SQL = Mi_SQL + "FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " ";
            //                     Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = '";
            //                     Mi_SQL = Mi_SQL + Dt_Productos_A_Cancelar.Rows[i]["PRODUCTO_ID"].ToString().Trim() + "'";

            //                     //Ejecutar consulta
            //                     Dt_Aux = new DataTable();
            //                     Obj_Comando.CommandText = Mi_SQL;
            //                     Obj_Adaptador.SelectCommand = Obj_Comando;
            //                     Obj_Adaptador.Fill(Dt_Aux);

            //                     // Verificar si la consulta arrojo resultado
            //                     if (Dt_Aux.Rows.Count > 0)
            //                     {
            //                         //Asignar los valores de los montos
            //                         if (Convert.IsDBNull(Dt_Aux.Rows[0][0]) == false)
            //                             Cantidad_Comprometida = Convert.ToInt32(Dt_Aux.Rows[0][0]);
            //                         else
            //                             Cantidad_Comprometida = 0;

            //                         if (Convert.IsDBNull(Dt_Aux.Rows[0][1]) == false)
            //                             Cantidad_Disponible = Convert.ToInt32(Dt_Aux.Rows[0][1]);
            //                         else
            //                             Cantidad_Disponible = 0;
            //                     }

            //                     if (Dt_Productos_A_Cancelar.Rows[i]["CANTIDAD_CANCELAR"].ToString().Trim() != "") // Si 
            //                         Cantidad_Productos_Cancelar = Convert.ToInt32(Dt_Productos_A_Cancelar.Rows[i]["CANTIDAD_CANCELAR"].ToString().Trim());

            //                     if (Cantidad_Comprometida != 0)
            //                     {
            //                         // Se asigna el valor a las cantidades
            //                         Cantidad_Comprometida = Cantidad_Comprometida - Cantidad_Productos_Cancelar;
            //                         Cantidad_Disponible = Cantidad_Disponible + Cantidad_Productos_Cancelar;

            //                         //Consulta para modificar las cantidades en la base de datos
            //                         Mi_SQL = "UPDATE " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " ";
            //                         Mi_SQL = Mi_SQL + "SET " + Cat_Com_Productos.Campo_Comprometido + " = " + Cantidad_Comprometida.ToString().Trim() + ", ";
            //                         Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Disponible + " = " + Cantidad_Disponible.ToString().Trim() + " ";
            //                         Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = '" + Dt_Productos_A_Cancelar.Rows[i]["PRODUCTO_ID"].ToString().Trim() + "'";

            //                         //Ejecutar consulta
            //                         Obj_Comando.CommandText = Mi_SQL;
            //                         Obj_Comando.ExecuteNonQuery();
            //                     }
            //                 }
            //             }
            //        }

            //        //Ejecutar transaccion
            //        Obj_Transaccion.Commit();
            //}
            //catch (OracleException Ex)
            //{
            //    if (Obj_Transaccion != null)
            //    {
            //        Obj_Transaccion.Rollback();
            //    }
            //    switch (Ex.Code.ToString())
            //    {
            //        case "2291":
            //            Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
            //            break;
            //        case "923":
            //            Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
            //            break;
            //        case "12170":
            //            Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
            //            break;
            //        default:
            //            Mensaje = "Error:  [" + Ex.Message + "]";
            //            break;
            //    }
            //    throw new Exception(Mensaje, Ex);
            //}
            //finally
            //{
            //    Obj_Comando = null;
            //    Obj_Conexion = null;
            //    Obj_Transaccion = null;
            //}
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Productos_Requisicion
        ///DESCRIPCIÓN:          Se consultan los productos de una requisición determinada
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           27/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Productos_Requisicion( String No_Requisicion)
        {
            // Declaración de Variables
            String Mi_SQL = null;
            DataTable Dt_Productos_Requisicion = new DataTable();

            Mi_SQL = Mi_SQL + " SELECT distinct SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + " ";
            Mi_SQL = Mi_SQL + ", SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Costo + "";
            Mi_SQL = Mi_SQL + ", SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Cantidad + " as CANTIDAD_ENTREGADA";

            Mi_SQL = Mi_SQL + ",(select REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Cantidad + " from ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where  REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + " (select SALIDAS." + Alm_Com_Salidas.Campo_Requisicion_ID + " from ";
            Mi_SQL = Mi_SQL + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + " SALIDAS ";
            Mi_SQL = Mi_SQL + " where  SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_No_Salida + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS." + Alm_Com_Salidas.Campo_No_Salida + ")";
            Mi_SQL = Mi_SQL + " and REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + ") as CANTIDAD_SOLICITADA ";

            Mi_SQL = Mi_SQL + ",(select REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Partida_ID + " from ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where  REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + " (select SALIDAS." + Alm_Com_Salidas.Campo_Requisicion_ID + " from ";
            Mi_SQL = Mi_SQL + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + " SALIDAS ";
            Mi_SQL = Mi_SQL + " where  SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_No_Salida + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS." + Alm_Com_Salidas.Campo_No_Salida + ")";
            Mi_SQL = Mi_SQL + " and REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + ") as PARTIDA_ID ";

            Mi_SQL = Mi_SQL + ",(select REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Porcentaje_IVA + " from ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where  REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + " (select SALIDAS." + Alm_Com_Salidas.Campo_Requisicion_ID + " from ";
            Mi_SQL = Mi_SQL + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + " SALIDAS ";
            Mi_SQL = Mi_SQL + " where  SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_No_Salida + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS." + Alm_Com_Salidas.Campo_No_Salida + ")";
            Mi_SQL = Mi_SQL + " and REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + ") as PORCENTAJE_IVA ";

            Mi_SQL = Mi_SQL + ",(select REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID + " from ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where  REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + " (select SALIDAS." + Alm_Com_Salidas.Campo_Requisicion_ID + " from ";
            Mi_SQL = Mi_SQL + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + " SALIDAS ";
            Mi_SQL = Mi_SQL + " where  SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_No_Salida + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS." + Alm_Com_Salidas.Campo_No_Salida + ")";
            Mi_SQL = Mi_SQL + " and REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + ") as PROYECTO_PROGRAMA_ID ";

            Mi_SQL = Mi_SQL + " FROM " + Alm_Com_Salidas_Detalles.Tabla_Alm_Com_Salidas_Detalles + " SALIDAS_DETALLES ";
            Mi_SQL = Mi_SQL + " JOIN " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + " SALIDAS ";
            Mi_SQL = Mi_SQL + " ON SALIDAS." + Alm_Com_Salidas.Campo_No_Salida + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_No_Salida + "";
            Mi_SQL = Mi_SQL + " WHERE SALIDAS." + Alm_Com_Salidas.Campo_Requisicion_ID + " = " + No_Requisicion.Trim();

            Dt_Productos_Requisicion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Productos_Requisicion;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Dependencia_ID
        ///DESCRIPCIÓN:          Método utilizado para consultar la Dependencia_ID
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           27/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static String Consultar_Dependencia_ID( String No_Requisicion)
        {
             // Declaración de Variables
            String Mi_SQL = null;
            OracleCommand Obj_Comando = new OracleCommand();
            DataTable Dt_Dependencia = new DataTable();
            String Dependencia_ID = "";

            Mi_SQL = " SELECT REQUISICIONES." + Ope_Com_Requisiciones.Campo_Dependencia_ID + "";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "  REQUISICIONES";
            Mi_SQL = Mi_SQL + " WHERE REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + No_Requisicion.Trim();

            Dt_Dependencia = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            Dependencia_ID = "" + Dt_Dependencia.Rows[0][0].ToString();

            return Dependencia_ID.Trim();
        }

    }
}