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
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Requisiciones_Stock.Negocio;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Bitacora_Eventos;
using Presidencia.Generar_Requisicion.Negocio;

/// <summary>
/// Summary description for 
/// </summary>
/// 

namespace Presidencia.Requisiciones_Stock.Datos
{
    public class Cls_Ope_Com_Alm_Requisiciones_Stock_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Requisiciones
        ///DESCRIPCIÓN:          Método utilizado para consultar las requisiciones de stock de almacén
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           22/junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Requisiciones(Cls_Ope_Com_Alm_Requisiciones_Stock_Negocio Datos)
        {
            // Declaración de variables
            String Mi_SQL = null;  
            DataTable Dt_Requisiciones = new DataTable();

            Mi_SQL = "SELECT " + "REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + ""; //NO_REQUISICION
            Mi_SQL = Mi_SQL + ", DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + " as UNIDAD_RESPONSABLE";
            Mi_SQL = Mi_SQL + ", DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID + " as UNIDAD_RESPONSABLE_ID";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio + "";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Estatus + "";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion + "";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Total + "";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
            Mi_SQL = Mi_SQL + " ON REQUISICIONES." + Ope_Com_Requisiciones.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + " WHERE  REQUISICIONES." + Ope_Com_Requisiciones.Campo_Tipo + "= '" + "STOCK'";
            Mi_SQL = Mi_SQL + " and  ( REQUISICIONES." + Ope_Com_Requisiciones.Campo_Estatus + "= '" + "ALMACEN'"; // Nota en esta parte debe ir Surtida
            Mi_SQL = Mi_SQL + " or  REQUISICIONES." + Ope_Com_Requisiciones.Campo_Estatus + "= '" + "PARCIAL')"; // Nota en esta parte debe ir Surtida

            if (Datos.P_No_Requisicion != null)
            {
                Mi_SQL = Mi_SQL + "AND REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " like '%" + Datos.P_No_Requisicion + "%'";
            }

            //if (Datos.P_Dependencia_ID != null)
            //{
            //    Mi_SQL = Mi_SQL + " and  REQUISICIONES." + Ope_Com_Requisiciones.Campo_Dependencia_ID + "= '" + Datos.P_Dependencia_ID + "'";
            //}

            if ((Datos.P_Fecha_Inicial != null) && (Datos.P_Fecha_Final != null))
            {
                Mi_SQL = Mi_SQL + " AND TO_DATE(TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion + ",'DD/MM/YY')) BETWEEN '" + Datos.P_Fecha_Inicial + "'" +
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
        ///FECHA_CREO:           22/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Detalles_Requisicion(Cls_Ope_Com_Alm_Requisiciones_Stock_Negocio Datos)
        {
            // Declaración de variables
            String Mi_SQL = null;
            DataTable Dt_Requisicion = new DataTable();

            //Mi_SQL = "SELECT " + "REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + ""; //NO_REQUISICION
            //Mi_SQL = Mi_SQL + ", DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + " as UNIDAD_RESPONSABLE";
            //Mi_SQL = Mi_SQL + ", DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID + " as UNIDAD_RESPONSABLE_ID";
            //Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio + "";
            //Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion + "";
            //Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Justificacion_Compra + " as COMENTARIOS";
            //Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_IVA + "";
            //Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Subtotal+ "";
            //Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Total + "";
            //Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";
            //Mi_SQL = Mi_SQL + " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
            //Mi_SQL = Mi_SQL + " ON REQUISICIONES." + Ope_Com_Requisiciones.Campo_Dependencia_ID;
            //Mi_SQL = Mi_SQL + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID;
            //Mi_SQL = Mi_SQL + " WHERE  REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "= '" +  Datos.P_No_Requisicion+ "'";

            Mi_SQL = "SELECT " + "REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + ""; //NO_REQUISICION
            Mi_SQL = Mi_SQL + ", DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + " as UNIDAD_RESPONSABLE";
            Mi_SQL = Mi_SQL + ", DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID + " as UNIDAD_RESPONSABLE_ID";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio + "";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion + "";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Justificacion_Compra + " as COMENTARIOS";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_IVA + " as MONTO_IVA";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Subtotal + " as SUBTOTAL";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Total + " AS MONTO_TOTAL";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
            Mi_SQL = Mi_SQL + " ON REQUISICIONES." + Ope_Com_Requisiciones.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + " WHERE  REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "= '" + Datos.P_No_Requisicion + "'";




            Dt_Requisicion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Requisicion;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Productos_Requisicion
        ///DESCRIPCIÓN:          Método utilizado el programa y la fuente de financiomiento
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           23/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Pragrama_Financiamiento(Cls_Ope_Com_Alm_Requisiciones_Stock_Negocio Datos)
        {
            DataTable Dt_Consulta = new DataTable();
            String Mi_SQL = null;

            Mi_SQL = " SELECT DISTINCT" + " PROYECTOS_PROGRAMAS." + Cat_Com_Proyectos_Programas.Campo_Descripcion + " as PROYECTO_PROGRAMA ";
            Mi_SQL = Mi_SQL + ", PROYECTOS_PROGRAMAS." + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + " ";
            Mi_SQL = Mi_SQL + ", FINANCIAMIENTO." + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion+ " as FINANCIAMIENTO ";
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
        ///NOMBRE DE LA FUNCIÓN: Consulta_Productos_Requisicion
        ///DESCRIPCIÓN:          Método utilizado para consultar los productos de 
        ///                      las requisiciones de stock de almacén
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           22/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Productos_Requisicion(Cls_Ope_Com_Alm_Requisiciones_Stock_Negocio Datos)
        {
            // Declaración de variables
            String Mi_SQL = null;
            DataSet Ds_Productos_Requisicion = null;
            DataTable Dt_Productos_Requisicion = new DataTable();

            Mi_SQL = "SELECT " + " REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " as PRODUCTO_ID";
            Mi_SQL = Mi_SQL + ", PRODUCTOS." + Cat_Com_Productos.Campo_Clave + "";
            Mi_SQL = Mi_SQL + ", PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + " as NOMBRE_PRODUCTO";
            Mi_SQL = Mi_SQL + ", PRODUCTOS." + Cat_Com_Productos.Campo_Descripcion + " as DESCRIPCION";
            Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Cantidad + " as CANTIDAD_SOLICITADA";
            Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Cantidad_Entregada + " ";
            Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Precio_Unitario + " as PRECIO";
            Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Porcentaje_IVA;
            Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Importe + " as SUBTOTAL";
            Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Monto_IVA + "";
            Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Monto_Total + " ";
            Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Partida_ID + "";
            Mi_SQL = Mi_SQL + ", ( SELECT " + Cat_Com_Unidades.Campo_Abreviatura + " FROM " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " WHERE ";
            Mi_SQL = Mi_SQL + Cat_Com_Unidades.Campo_Unidad_ID + " = ( SELECT " + Cat_Com_Unidades.Campo_Unidad_ID + "  FROM ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " )) AS UNIDAD ";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO";
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS";
            Mi_SQL = Mi_SQL + " ON REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID;
            Mi_SQL = Mi_SQL + " = PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID;

            Mi_SQL = Mi_SQL + " WHERE  REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID + "= '" + Datos.P_No_Requisicion + "'";


            Dt_Productos_Requisicion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Productos_Requisicion;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Empleado
        ///DESCRIPCIÓN:          Método utilizado para consultar los empleados
        ///PARAMETROS:   
        ///CREO:                 Gustavo Angeles C
        ///FECHA_CREO:           08/Dic/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Empleado(Cls_Ope_Com_Alm_Requisiciones_Stock_Negocio Datos)
        {
            String Mi_SQL = "";
            DataTable Dt_Empleados = null;
            try
            {
                Mi_SQL = "SELECT " +
                Cat_Empleados.Campo_Empleado_ID + "," +
                Cat_Empleados.Campo_Nombre + " ||' '|| " +
                Cat_Empleados.Campo_Apellido_Paterno + " ||' '|| " +
                Cat_Empleados.Campo_Apellido_Materno + " AS NOMBRE" +
                " FROM " +
                Cat_Empleados.Tabla_Cat_Empleados +
                " WHERE " +
                Cat_Empleados.Campo_No_Empleado + " = " +
                Datos.P_No_Empleado;
                Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch(Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; 
                throw new Exception(Mensaje);
            }
            return Dt_Empleados;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_DataTable
        ///DESCRIPCIÓN:          Método utilizado para consultar loas dependencias y las áreas
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           22/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_DataTable(Cls_Ope_Com_Alm_Requisiciones_Stock_Negocio Datos)
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
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Dependencias.Campo_Nombre;
                }
                else if (Datos.P_Tipo_Data_Table.Equals("EMPLEADOS_UR"))
                {
                    Mi_SQL = Mi_SQL + " SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + "  ||' '||";
                    Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "  ||' '||";
                    Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "  as EMPLEADO, ";
                    Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " as EMPLEADO_ID";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + ", " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + "=";
                    Mi_SQL = Mi_SQL + "" + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID;
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = ";
                    Mi_SQL = Mi_SQL + "'" + Datos.P_No_Requisicion + "'";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre;
                }
                else if (Datos.P_Tipo_Data_Table.Equals("EMPLEADOS"))
                {
                    Mi_SQL = Mi_SQL + " SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + "  ||' '||";
                    Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "  ||' '||";
                    Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "  as EMPLEADO, ";
                    Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " as EMPLEADO_ID";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado;
                    Mi_SQL = Mi_SQL + " = '" + Datos.P_No_Empleado + "'";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre;
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
        /// NOMBRE DE LA CLASE:     Alta_orden_Salida
        /// DESCRIPCION:            Dar de alta la orden de salida de material
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos para al operacion
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            23/Junio/2010 

        ///*******************************************************************************/
        public static long Alta_Orden_Salida(Cls_Ope_Com_Alm_Requisiciones_Stock_Negocio Datos)
        {
            // Declaracion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = String.Empty;
            Object Aux; // Variable auxiliar para las consultas
            String Mensaje = String.Empty; //Variable para el mensaje de error
            DataTable Dt_Aux = new DataTable(); //Tabla auxiliar para las consultas
            OracleDataAdapter Obj_Adaptador; //Adapatador para el llenado de las tablas

            Double Monto_Comprometido = 0.0; // Variable para el monto comprometido
            Double Monto_Ejercido = 0.0;    // Variable para el monto ejercido

            String No_Asignacion = String.Empty; // Variable para el No de Asignacion            
            String Partida_ID = String.Empty; // Variable para el ID de la partida
            String Proyecto_Programa_ID = String.Empty; // Variable para el ID del programa o proyecto
            String Dependencia_ID = String.Empty; // Variable para el ID de la dependencia
            Double Monto_Total = 0.0; // Variable para el monto total de los detalles de la requisicion

            // Variables utilizadas para actualizar los productos
            Int64 Cantidad_Comprometida = 0; // Variable para la cantidad Comprometida
            Int64 Cantidad_Existente = 0; // Variable para la cantidad Existente
            String Tipo_Salida_ID = "";

            Double SubTotal_Prod_Req = 0.0;
            Double IVA_Prod_Req = 0.0;
            Double Total_Prod_Req = 0.0;
            Int64 Cantidad_Entregada = 0;
            Int64 Cantidad_A_Entregar = 0;

            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Adaptador = new OracleDataAdapter();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;

                //Asignar consulta para el Maximo ID
                Mi_SQL = "SELECT NVL(MAX(" + Alm_Com_Salidas.Campo_No_Salida + "), 0) FROM " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas;

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Aux = Obj_Comando.ExecuteScalar();

                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false)
                    Datos.P_No_Orden_Salida = Convert.ToInt64(Aux) + 1;
                else
                    Datos.P_No_Orden_Salida = 1;

                // Consulta para los ID de la dependencia, area, etc
                Mi_SQL = "SELECT " + Ope_Com_Requisiciones.Campo_Dependencia_ID + ", " + Ope_Com_Requisiciones.Campo_Area_ID + " ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + Datos.P_No_Requisicion.ToString().Trim() + " ";

                //Ejecutar consulta
                Dt_Aux = new DataTable();
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Adaptador.SelectCommand = Obj_Comando;
                Obj_Adaptador.Fill(Dt_Aux);

                //Verificar si la consulta arrojo resultado
                if (Dt_Aux.Rows.Count > 0)
                {
                    Datos.P_Dependencia_ID = Dt_Aux.Rows[0][0].ToString().Trim(); // Colocar los valores en las variables
                    Datos.P_Area_ID = Dt_Aux.Rows[0][1].ToString().Trim();
                }
                else
                {
                    throw new Exception("Datos no encontrados requisicion No. " + Datos.P_No_Requisicion.ToString().Trim());
                }

                // For utilizado para calcular los montos de la requisición
                for (int j = 0; j < Datos.P_Dt_Productos_Requisicion.Rows.Count; j++)
                {
                    SubTotal_Prod_Req = SubTotal_Prod_Req + Convert.ToDouble(Datos.P_Dt_Productos_Requisicion.Rows[j]["SUBTOTAL"]);
                    IVA_Prod_Req = IVA_Prod_Req + Convert.ToDouble(Datos.P_Dt_Productos_Requisicion.Rows[j]["MONTO_IVA"]);
                    Total_Prod_Req = Total_Prod_Req + Convert.ToDouble(Datos.P_Dt_Productos_Requisicion.Rows[j]["TOTAL"]);
                }

                // Consulta para dar de alta la salida
                Mi_SQL = "INSERT INTO " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + " (" + Alm_Com_Salidas.Campo_No_Salida + ", ";
                Mi_SQL = Mi_SQL + Alm_Com_Salidas.Campo_Dependencia_ID + ", ";
                Mi_SQL = Mi_SQL + Alm_Com_Salidas.Campo_Empleado_Solicito_ID + ", " + Alm_Com_Salidas.Campo_Requisicion_ID + ", ";
                Mi_SQL = Mi_SQL + Alm_Com_Salidas.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Alm_Com_Salidas.Campo_Fecha_Creo + ", " + Alm_Com_Salidas.Campo_Empleado_Almacen_ID + ", ";
                Mi_SQL = Mi_SQL + Alm_Com_Salidas.Campo_Subtotal + " , " + Alm_Com_Salidas.Campo_IVA + ", " + Alm_Com_Salidas.Campo_Total + ") ";
                Mi_SQL = Mi_SQL + " VALUES(" + Datos.P_No_Orden_Salida + ", ";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Dependencia_ID + "', '" + Datos.P_Empleado_Recibio_ID + "', ";
                Mi_SQL = Mi_SQL + Datos.P_No_Requisicion.ToString().Trim() + ", ";
                //Mi_SQL = Mi_SQL + "'" + Datos.P_Nombre_Empleado_Almacen + "', SYSDATE, '" + Datos.P_Empleado_Almacen_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + Cls_Sessiones.Nombre_Empleado + "', SYSDATE, '" + Cls_Sessiones.Empleado_ID.Trim() + "', ";
                Mi_SQL = Mi_SQL + SubTotal_Prod_Req + ", " + IVA_Prod_Req + ", " + Total_Prod_Req + " )";

                //String No_Salida = Convert.ToString(Datos.P_No_Orden_Salida);
                // Se registra  el Insert en la bitacora
                //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Alm_Com_Orden_Salida.aspx", No_Salida, Mi_SQL);

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                // Consulta para la actualizacion del estatus de la requisicion 
                Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " ";
                Mi_SQL = Mi_SQL + "SET " + Ope_Com_Requisiciones.Campo_Estatus + " = '" + Datos.P_Estatus.ToString().Trim() + "', ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_Fecha_Surtido + " = SYSDATE, ";
                //Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_Empleado_Surtido_ID + " = '" + Datos.P_Empleado_Almacen_ID + "' ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_Empleado_Surtido_ID + " = '" + Cls_Sessiones.Empleado_ID.Trim() + "' ";
                Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + Datos.P_No_Requisicion.ToString().Trim() + " ";

                //String No_Requisicion = Convert.ToString(Datos.P_No_Requisicion);
                // Se registra  el update en la bitacora
                //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Alm_Com_Orden_Salida.aspx", No_Requisicion, Mi_SQL);

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();


                // Se Guarda el Historial de la requisición
                Cls_Ope_Com_Requisiciones_Negocio Requisiciones = new Cls_Ope_Com_Requisiciones_Negocio();
                Requisiciones.Registrar_Historial(Datos.P_Estatus.ToString().Trim(), Datos.P_No_Requisicion.ToString().Trim());


                // Verificar si tiene datos la tabla enviada con las cantidades entregadas
                if (Datos.P_Dt_Productos_Requisicion.Rows.Count > 0)
                {
                    // Ciclo para el desplazamiento de la tabla
                    for (int Cont_Elementos = 0; Cont_Elementos < Datos.P_Dt_Productos_Requisicion.Rows.Count; Cont_Elementos++)
                    {
                        if (Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["PRECIO"].ToString().Trim() == null || Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["PRECIO"].ToString().Trim() == "") // Se realiza esta validación por que luego el precio es 0 para que no marque error
                            Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["PRECIO"] = 0;

                        //Consulta para dar de alta los detalles de la salida
                        Mi_SQL = "INSERT INTO " + Alm_Com_Salidas_Detalles.Tabla_Alm_Com_Salidas_Detalles + " (" + Alm_Com_Salidas_Detalles.Campo_No_Salida + ", ";
                        Mi_SQL = Mi_SQL + Alm_Com_Salidas_Detalles.Campo_Producto_ID + ", " + Alm_Com_Salidas_Detalles.Campo_Cantidad + ", ";
                        Mi_SQL = Mi_SQL + Alm_Com_Salidas_Detalles.Campo_Costo + ", " + Alm_Com_Salidas_Detalles.Campo_Costo_Promedio + ", ";
                        Mi_SQL = Mi_SQL + Alm_Com_Salidas_Detalles.Campo_Subtotal + ", " + Alm_Com_Salidas_Detalles.Campo_IVA + ", ";
                        Mi_SQL = Mi_SQL + Alm_Com_Salidas_Detalles.Campo_Importe + ") VALUES(" + Datos.P_No_Orden_Salida + ", ";
                        Mi_SQL = Mi_SQL + "'" + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["PRODUCTO_ID"].ToString().Trim() + "', ";
                        Mi_SQL = Mi_SQL + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["CANTIDAD_A_ENTREGAR"].ToString().Trim() + ", ";
                        Mi_SQL = Mi_SQL + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["PRECIO"].ToString().Trim() + ", ";
                        Mi_SQL = Mi_SQL + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["PRECIO"].ToString().Trim() + ", ";
                        Mi_SQL = Mi_SQL + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["SUBTOTAL"].ToString().Trim() + ", ";
                        Mi_SQL = Mi_SQL + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["MONTO_IVA"].ToString().Trim() + ", ";
                        Mi_SQL = Mi_SQL + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["TOTAL"].ToString().Trim() + ")";

                        //String N_Salida = Convert.ToString(Datos.P_No_Salida);
                        // Se registra  el Insert en la bitacora
                        //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Alm_Com_Orden_Salida.aspx", N_Salida, Mi_SQL);

                        //Ejecutar consulta
                        Obj_Comando.CommandText = Mi_SQL;
                        Obj_Comando.ExecuteNonQuery();

                        // SE ACTUALIZA LA CANTIDAD ENTREGADA DE PRODUCTOS EN LA TABLA REQ_PRODUCTOS
                        Cantidad_Entregada = Convert.ToInt64(Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["CANTIDAD_ENTREGADA"].ToString().Trim());
                        Cantidad_A_Entregar = Convert.ToInt64(Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["CANTIDAD_A_ENTREGAR"].ToString().Trim());

                        Cantidad_Entregada = Cantidad_Entregada + Cantidad_A_Entregar; // Se suman las cantidades, lo que se va a entregar, con lo que se entrego

                        // Consulta para la actualizacion del estatus de la requisicion 
                        Mi_SQL = "UPDATE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " ";
                        Mi_SQL = Mi_SQL + "SET " + Ope_Com_Req_Producto.Campo_Cantidad_Entregada + " = " + Cantidad_Entregada + " ";
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = '" + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["PRODUCTO_ID"].ToString().Trim() + "' ";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = " + Datos.P_No_Requisicion.ToString().Trim() + " ";

                        //String No_Requisicion = Convert.ToString(Datos.P_No_Requisicion);
                        // Se registra  el update en la bitacora
                        //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Alm_Com_Orden_Salida.aspx", No_Requisicion, Mi_SQL);

                        //Ejecutar consulta
                        Obj_Comando.CommandText = Mi_SQL;
                        Obj_Comando.ExecuteNonQuery();



                        // SE ACTUALIZAN LOS MONTOS 
                        // Asignar el ID de la partida, a la dependencia y el ID del proyecto o programa
                        Partida_ID = Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["PARTIDA_ID"].ToString().Trim();
                        Dependencia_ID = Datos.P_Dependencia_ID.ToString().Trim();
                        Proyecto_Programa_ID = Datos.P_Proyecto_Programa_ID.ToString().Trim();

                        // Verificar si no es nulo
                        if (Convert.IsDBNull(Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["TOTAL"]) == false)
                            Monto_Total = Convert.ToDouble(Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["TOTAL"]); // Se asigna el monto total de cada producto de la requisición
                        else
                            Monto_Total = 0;

                        // Consulta para obtener el mayor numero de asignación
                        Mi_SQL = "SELECT  MAX (" + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ") ";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " = '" + Dependencia_ID + "'";
                        Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Partida_ID;
                        Mi_SQL = Mi_SQL + " = '" + Partida_ID + "'";
                        Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID;
                        Mi_SQL = Mi_SQL + " = '" + Proyecto_Programa_ID + "'";
                        Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto;
                        Mi_SQL = Mi_SQL + " = extract(year from sysdate)";

                        // Ejecutar consulta
                        Obj_Comando.CommandText = Mi_SQL;
                        Aux = Obj_Comando.ExecuteScalar();

                        // Verificar si es nulo
                        if (Convert.IsDBNull(Aux) == false)
                            No_Asignacion = Aux.ToString().Trim();

                        // Consulta para obtener los  montos 
                        Mi_SQL = "SELECT  " + Cat_Com_Dep_Presupuesto.Campo_Monto_Ejercido + ", ";
                        Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + ", ";
                        Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + "";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " = '" + Dependencia_ID + "'";
                        Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Partida_ID;
                        Mi_SQL = Mi_SQL + " = '" + Partida_ID + "'";
                        Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID;
                        Mi_SQL = Mi_SQL + " = '" + Proyecto_Programa_ID + "'";
                        Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto;
                        Mi_SQL = Mi_SQL + " = extract(year from sysdate)";
                        Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio;
                        Mi_SQL = Mi_SQL + " = " + No_Asignacion;

                        //Ejecutar consulta
                        DataTable Dt_Aux_Presupuestos = new DataTable();
                        Obj_Comando.CommandText = Mi_SQL;
                        Obj_Adaptador.SelectCommand = Obj_Comando;
                        Obj_Adaptador.Fill(Dt_Aux_Presupuestos);

                        // Verificar si la consulta tiene elementos
                        if (Dt_Aux_Presupuestos.Rows.Count > 0)
                        {
                            if (Convert.IsDBNull(Dt_Aux_Presupuestos.Rows[0]["MONTO_EJERCIDO"]) != false) // Si no tiene un monto ejercido entra
                                Monto_Ejercido = Monto_Total; // Obtener el nuevo monto ejercido 
                            else
                                Monto_Ejercido = Convert.ToDouble(Dt_Aux_Presupuestos.Rows[0]["MONTO_EJERCIDO"]) + Monto_Total;// Obtener el  monto ejercido y lo suma al monto Total del producto

                            if (Convert.IsDBNull(Dt_Aux_Presupuestos.Rows[0]["MONTO_COMPROMETIDO"]) == false)
                                Monto_Comprometido = Convert.ToDouble(Dt_Aux_Presupuestos.Rows[0]["MONTO_COMPROMETIDO"]) - Monto_Total; // Obtener el  MONTO COMPROMETIDO y le resta el MONTO TOTAL del producto
                            else
                                Monto_Comprometido = 0;

                            // Actualizar la tabla de los presupuestos
                            Mi_SQL = " UPDATE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + " ";
                            Mi_SQL = Mi_SQL + " SET " + Cat_Com_Dep_Presupuesto.Campo_Monto_Ejercido + " = " + Monto_Ejercido + ", ";
                            Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + " = " + Monto_Comprometido + " ";
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + " = '" + Dependencia_ID + "'";
                            Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID + " = '" + Partida_ID + "'";
                            Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + " = '" + Proyecto_Programa_ID + "'";
                            Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + " = extract(year from sysdate)";
                            Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + " = " + No_Asignacion;

                            // Se da de alta la operación en el método "Alta_Bitacora"
                            //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Alm_Com_Recepcion_Material.aspx", Proyecto_Programa_ID, Mi_SQL);

                            //Ejecutar consulta
                            Obj_Comando.CommandText = Mi_SQL;
                            Obj_Comando.ExecuteNonQuery(); // Se ejecuta la operación 
                        }
                        else
                        {
                            // Escribir un mensaje que indica que no se actualizó el presupuesto  
                        }

                        // SE DISMINUYEN LOS PRODUCTOS
                        //Consulta para el campo comprometido
                        Mi_SQL = "SELECT " + Cat_Com_Productos.Campo_Comprometido + ", " + Cat_Com_Productos.Campo_Existencia + " ";
                        Mi_SQL = Mi_SQL + "FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " ";
                        Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = '" + 
                            Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["PRODUCTO_ID"].ToString().Trim() + "'";

                        //Ejecutar consulta
                        Dt_Aux = new DataTable();
                        Obj_Comando.CommandText = Mi_SQL;
                        Obj_Adaptador.SelectCommand = Obj_Comando;
                        Obj_Adaptador.Fill(Dt_Aux);

                        //Verificar si la consulta arrojo resultado
                        if (Dt_Aux.Rows.Count > 0)
                        {
                            //Asignar los valores de los montos
                            if (Convert.IsDBNull(Dt_Aux.Rows[0][0]) == false)
                                Cantidad_Comprometida = Convert.ToInt32(Dt_Aux.Rows[0][0]);
                            else
                                Cantidad_Comprometida = 0;

                            if (Convert.IsDBNull(Dt_Aux.Rows[0][1]) == false)
                                Cantidad_Existente = Convert.ToInt32(Dt_Aux.Rows[0][1]);
                            else
                                Cantidad_Existente = 0;
                        }
                        //VALIDACION PARA NO ENTREGAR MAS DE LO DISPONIBLE Y EXISTENCIA Y NO PERMITIR NEGATIVOS
                        //if ((Cantidad_Comprometida != 0) & (Cantidad_Existente != 0))
                        //if ((Cantidad_Comprometida > 0) & (Cantidad_Existente > 0))


                        int Ctd_Entregar = int.Parse(Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["CANTIDAD_A_ENTREGAR"].ToString());
                        //if (Cantidad_Comprometida >= Ctd_Entregar && Cantidad_Existente >= Ctd_Entregar)
                        //{
                        //Realizar los calculos de los montos
                        Cantidad_Comprometida = Cantidad_Comprometida - Convert.ToInt64(Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["CANTIDAD_A_ENTREGAR"]);
                        Cantidad_Existente = Cantidad_Existente - Convert.ToInt64(Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["CANTIDAD_A_ENTREGAR"]);

                        //Consulta para modificar las cantidades en la base de datos
                        Mi_SQL = "UPDATE " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " ";
                        Mi_SQL = Mi_SQL + "SET " + Cat_Com_Productos.Campo_Comprometido + " = " + Cantidad_Comprometida.ToString().Trim() + ", ";
                        Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Existencia + " = " + Cantidad_Existente.ToString().Trim() + " ";
                        Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = '" + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["PRODUCTO_ID"].ToString().Trim() + "'";

                        //    //String Producto_ID = "" + Dt_Requisiciones_Detalles.Rows[Cont_Elementos][3].ToString().Trim();
                        //    // Se registra  el Update en la bitacora
                        //    //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Alm_Com_Orden_Salida.aspx", Producto_ID, Mi_SQL);

                        //    //Ejecutar consulta
                        Obj_Comando.CommandText = Mi_SQL;
                        Obj_Comando.ExecuteNonQuery();
                        //}
                        //else 
                        //{
                        //    Datos.P_Mensaje = "Verifique kardex de producto: [" +
                        //    Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["PRODUCTO_ID"].ToString().Trim() + "]";                          
                        //    Obj_Transaccion.Rollback();
                        //    Obj_Conexion.Close();
                        //    return (0);
                        //}
                    }
                }
                //Ejecutar transaccion
                Obj_Transaccion.Commit();

                //Entregar resultado
                return Datos.P_No_Orden_Salida;
            }
            catch (OracleException Ex)
            {
                if (Obj_Transaccion != null)
                {
                    Obj_Transaccion.Rollback();
                }
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
                        break;
                    case "923":
                        Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
                        break;
                    case "12170":
                        Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
                        break;
                    default:
                        Mensaje = "Error:  [" + Ex.Message + "]";
                        break;
                }
                throw new Exception(Mensaje, Ex);
            }
            finally
            {
                //Obj_Conexion.Close();
                Obj_Comando = null;
                Obj_Conexion = null;
                Obj_Transaccion = null;
            }
            //// Declaracion de variables
            //OracleTransaction Obj_Transaccion = null;
            //OracleConnection Obj_Conexion;
            //OracleCommand Obj_Comando;
            //String Mi_SQL = String.Empty;
            //Object Aux; // Variable auxiliar para las consultas
            //String Mensaje = String.Empty; //Variable para el mensaje de error
            //DataTable Dt_Aux = new DataTable(); //Tabla auxiliar para las consultas
            //OracleDataAdapter Obj_Adaptador; //Adapatador para el llenado de las tablas
            
            //Double Monto_Comprometido = 0.0; // Variable para el monto comprometido
            //Double Monto_Ejercido = 0.0;    // Variable para el monto ejercido

            //String No_Asignacion = String.Empty; // Variable para el No de Asignacion            
            //String Partida_ID = String.Empty; // Variable para el ID de la partida
            //String Proyecto_Programa_ID = String.Empty; // Variable para el ID del programa o proyecto
            //String Dependencia_ID = String.Empty; // Variable para el ID de la dependencia
            //Double Monto_Total = 0.0; // Variable para el monto total de los detalles de la requisicion

            //// Variables utilizadas para actualizar los productos
            //Int64 Cantidad_Comprometida = 0; // Variable para la cantidad Comprometida
            //Int64 Cantidad_Existente = 0; // Variable para la cantidad Existente
            //String Tipo_Salida_ID = "";

            //try
            //{
            //    Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
            //    Obj_Comando = new OracleCommand();
            //    Obj_Adaptador = new OracleDataAdapter();
            //    Obj_Conexion.Open();
            //    Obj_Transaccion = Obj_Conexion.BeginTransaction();
            //    Obj_Comando.Transaction = Obj_Transaccion;
            //    Obj_Comando.Connection = Obj_Conexion;

            //    //Asignar consulta para el Maximo ID
            //    Mi_SQL = "SELECT NVL(MAX(" + Alm_Com_Salidas.Campo_No_Salida + "), 0) FROM " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas;

            //    //Ejecutar consulta
            //    Obj_Comando.CommandText = Mi_SQL;
            //    Aux = Obj_Comando.ExecuteScalar();

            //    //Verificar si no es nulo
            //    if (Convert.IsDBNull(Aux) == false)
            //        Datos.P_No_Orden_Salida= Convert.ToInt64(Aux) + 1;
            //    else
            //        Datos.P_No_Orden_Salida = 1;

            //    // Consulta para los ID de la dependencia, area, etc
            //    Mi_SQL = "SELECT " + Ope_Com_Requisiciones.Campo_Dependencia_ID + ", " + Ope_Com_Requisiciones.Campo_Area_ID + " ";
            //    Mi_SQL = Mi_SQL + "FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " ";
            //    Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + Datos.P_No_Requisicion.ToString().Trim() + " ";

            //    //Ejecutar consulta
            //    Dt_Aux = new DataTable();
            //    Obj_Comando.CommandText = Mi_SQL;
            //    Obj_Adaptador.SelectCommand = Obj_Comando;
            //    Obj_Adaptador.Fill(Dt_Aux);

            //    //Verificar si la consulta arrojo resultado
            //    if (Dt_Aux.Rows.Count > 0)
            //    {
            //        Datos.P_Dependencia_ID = Dt_Aux.Rows[0][0].ToString().Trim(); // Colocar los valores en las variables
            //        Datos.P_Area_ID = Dt_Aux.Rows[0][1].ToString().Trim();
            //    }
            //    else
            //    {
            //        throw new Exception("Datos no encontrados requisicion No. " + Datos.P_No_Requisicion.ToString().Trim());
            //    }

            //    //El tipo de salida es la 1
            //    Tipo_Salida_ID = "00001";

            //    // Consulta para dar de alta la salida
            //    Mi_SQL = "INSERT INTO " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + " (" + Alm_Com_Salidas.Campo_No_Salida + ", ";
            //    Mi_SQL = Mi_SQL + Alm_Com_Salidas.Campo_Dependencia_ID + ", " + Alm_Com_Salidas.Campo_Area_ID + ", ";
            //    Mi_SQL = Mi_SQL + Alm_Com_Salidas.Campo_Empleado_Solicito_ID + ", " + Alm_Com_Salidas.Campo_Requisicion_ID + ", ";
            //    Mi_SQL = Mi_SQL + Alm_Com_Salidas.Campo_Tipo_Salida_ID + ", " + Alm_Com_Salidas.Campo_Usuario_Creo + ", ";
            //    Mi_SQL = Mi_SQL + Alm_Com_Salidas.Campo_Fecha_Creo + ", " + Alm_Com_Salidas.Campo_Empleado_Almacen_ID + ", ";
            //    Mi_SQL = Mi_SQL + Alm_Com_Salidas.Campo_Subtotal + " , " + Alm_Com_Salidas.Campo_IVA + ", "  + Alm_Com_Salidas.Campo_Total + ") ";
            //    Mi_SQL = Mi_SQL + " VALUES(" + Datos.P_No_Orden_Salida + ", ";
            //    Mi_SQL = Mi_SQL + "'" + Datos.P_Dependencia_ID + "', '" + Datos.P_Area_ID + "', '" + Datos.P_Empleado_Recibio_ID + "', ";
            //    Mi_SQL = Mi_SQL + Datos.P_No_Requisicion.ToString().Trim() + ", '" + Tipo_Salida_ID + "', ";
            //    Mi_SQL = Mi_SQL + "'" + Datos.P_Nombre_Empleado_Almacen + "', SYSDATE, '" + Datos.P_Empleado_Almacen_ID + "', ";
            //    Mi_SQL = Mi_SQL + Datos.P_Subtotal + ", " + Datos.P_Iva + ", "  + Datos.P_Total + " )";

            //    //String No_Salida = Convert.ToString(Datos.P_No_Orden_Salida);
            //    // Se registra  el Insert en la bitacora
            //    //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Alm_Com_Orden_Salida.aspx", No_Salida, Mi_SQL);

            //    //Ejecutar consulta
            //    Obj_Comando.CommandText = Mi_SQL;
            //    Obj_Comando.ExecuteNonQuery();

            //    // Consulta para la actualizacion del estatus de la requisicion 
            //    Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " ";
            //    Mi_SQL = Mi_SQL + "SET " + Ope_Com_Requisiciones.Campo_Estatus + " = '" + Datos.P_Estatus.ToString().Trim() + "', ";
            //    Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_Fecha_Surtido + " = SYSDATE, ";
            //    Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_Empleado_Surtido_ID + " = '" + Datos.P_Empleado_Almacen_ID + "' ";
            //    Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + Datos.P_No_Requisicion.ToString().Trim() + " ";

            //    //String No_Requisicion = Convert.ToString(Datos.P_No_Requisicion);
            //    // Se registra  el update en la bitacora
            //    //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Alm_Com_Orden_Salida.aspx", No_Requisicion, Mi_SQL);

            //    //Ejecutar consulta
            //    Obj_Comando.CommandText = Mi_SQL;
            //    Obj_Comando.ExecuteNonQuery();


            //    // Verificar si tiene datos la tabla enviada con las cantidades entregadas
            //    if (Datos.P_Dt_Productos_Requisicion.Rows.Count > 0)
            //    {
            //        // Ciclo para el desplazamiento de la tabla
            //        for (int Cont_Elementos = 0; Cont_Elementos < Datos.P_Dt_Productos_Requisicion.Rows.Count; Cont_Elementos++)
            //        {
            //            //Consulta para dar de alta los detalles de la salida
            //            Mi_SQL = "INSERT INTO " + Alm_Com_Salidas_Detalles.Tabla_Alm_Com_Salidas_Detalles + " (" + Alm_Com_Salidas_Detalles.Campo_No_Salida + ", ";
            //            Mi_SQL = Mi_SQL + Alm_Com_Salidas_Detalles.Campo_Producto_ID + ", " + Alm_Com_Salidas_Detalles.Campo_Cantidad + ", ";
            //            Mi_SQL = Mi_SQL + Alm_Com_Salidas_Detalles.Campo_Costo + ", " + Alm_Com_Salidas_Detalles.Campo_Costo_Promedio + ", ";
            //            Mi_SQL = Mi_SQL + Alm_Com_Salidas_Detalles.Campo_Subtotal + ", " + Alm_Com_Salidas_Detalles.Campo_IVA+ ", ";
            //            Mi_SQL = Mi_SQL + Alm_Com_Salidas_Detalles.Campo_Importe + ") VALUES(" + Datos.P_No_Orden_Salida + ", ";
            //            Mi_SQL = Mi_SQL + "'" + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["PRODUCTO_ID"].ToString().Trim() + "', ";
            //            Mi_SQL = Mi_SQL + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["CANTIDAD_ENTREGADA"].ToString().Trim() + ", ";
            //            Mi_SQL = Mi_SQL + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["PRECIO"].ToString().Trim() + ", ";
            //            Mi_SQL = Mi_SQL + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["PRECIO"].ToString().Trim() + ", ";
            //            Mi_SQL = Mi_SQL + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["SUBTOTAL"].ToString().Trim() + ", ";
            //            Mi_SQL = Mi_SQL + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["MONTO_IVA"].ToString().Trim() + ", ";
            //            Mi_SQL = Mi_SQL + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["TOTAL"].ToString().Trim() + ")";

            //            //String N_Salida = Convert.ToString(Datos.P_No_Salida);
            //            // Se registra  el Insert en la bitacora
            //            //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Alm_Com_Orden_Salida.aspx", N_Salida, Mi_SQL);

            //            //Ejecutar consulta
            //            Obj_Comando.CommandText = Mi_SQL;
            //            Obj_Comando.ExecuteNonQuery();


            //      // SE ACTUALIZAN LOS MONTOS 
            //            // Asignar el ID de la partida, a la dependencia y el ID del proyecto o programa
            //            Partida_ID = Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["PARTIDA_ID"].ToString().Trim();
            //            Dependencia_ID= Datos.P_Dependencia_ID.ToString().Trim();
            //            Proyecto_Programa_ID = Datos.P_Proyecto_Programa_ID.ToString().Trim();

            //                // Verificar si no es nulo
            //            if (Convert.IsDBNull(Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["TOTAL"]) == false)
            //                Monto_Total = Convert.ToDouble(Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["TOTAL"]); // Se asigna el monto total de cada producto de la requisición
            //            else
            //                Monto_Total = 0;

            //            // Consulta para obtener el mayor numero de asignación
            //            Mi_SQL = "SELECT  MAX (" + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ") ";
            //            Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
            //            Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID;
            //            Mi_SQL = Mi_SQL + " = '" + Dependencia_ID + "'";
            //            Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Partida_ID;
            //            Mi_SQL = Mi_SQL + " = '" + Partida_ID + "'";
            //            Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID;
            //            Mi_SQL = Mi_SQL + " = '" + Proyecto_Programa_ID + "'";
            //            Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto;
            //            Mi_SQL = Mi_SQL + " = extract(year from sysdate)";

            //            // Ejecutar consulta
            //            Obj_Comando.CommandText = Mi_SQL;
            //            Aux = Obj_Comando.ExecuteScalar();

            //            // Verificar si es nulo
            //            if (Convert.IsDBNull(Aux) == false)
            //                No_Asignacion = Aux.ToString().Trim();

            //            // Consulta para obtener los  montos 
            //            Mi_SQL = "SELECT  " + Cat_Com_Dep_Presupuesto.Campo_Monto_Ejercido + ", ";
            //            Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + ", ";
            //            Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + "";
            //            Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
            //            Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID;
            //            Mi_SQL = Mi_SQL + " = '" + Dependencia_ID + "'";
            //            Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Partida_ID;
            //            Mi_SQL = Mi_SQL + " = '" + Partida_ID + "'";
            //            Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID;
            //            Mi_SQL = Mi_SQL + " = '" + Proyecto_Programa_ID + "'";
            //            Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto;
            //            Mi_SQL = Mi_SQL + " = extract(year from sysdate)";
            //            Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio;
            //            Mi_SQL = Mi_SQL + " = " + No_Asignacion;

            //            //Ejecutar consulta
            //            DataTable Dt_Aux_Presupuestos = new DataTable();
            //            Obj_Comando.CommandText = Mi_SQL;
            //            Obj_Adaptador.SelectCommand = Obj_Comando;
            //            Obj_Adaptador.Fill(Dt_Aux_Presupuestos);

            //            // Verificar si la consulta tiene elementos
            //            if (Dt_Aux_Presupuestos.Rows.Count > 0)
            //            {
            //                if (Convert.IsDBNull(Dt_Aux_Presupuestos.Rows[0]["MONTO_EJERCIDO"]) != false) // Si no tiene un monto ejercido entra
            //                    Monto_Ejercido = Monto_Total; // Obtener el nuevo monto ejercido 
            //                else
            //                    Monto_Ejercido = Convert.ToDouble(Dt_Aux_Presupuestos.Rows[0]["MONTO_EJERCIDO"]) + Monto_Total;// Obtener el  monto ejercido y lo suma al monto Total del producto

            //                if (Convert.IsDBNull(Dt_Aux_Presupuestos.Rows[0]["MONTO_COMPROMETIDO"]) == false)
            //                    Monto_Comprometido = Convert.ToDouble(Dt_Aux_Presupuestos.Rows[0]["MONTO_COMPROMETIDO"]) - Monto_Total; // Obtener el  MONTO COMPROMETIDO y le resta el MONTO TOTAL del producto
            //                else
            //                    Monto_Comprometido = 0;

            //                // Actualizar la tabla de los presupuestos
            //                Mi_SQL = " UPDATE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + " ";
            //                Mi_SQL = Mi_SQL + " SET " + Cat_Com_Dep_Presupuesto.Campo_Monto_Ejercido + " = " + Monto_Ejercido + ", ";
            //                Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + " = " + Monto_Comprometido + " ";
            //                Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + " = '" + Dependencia_ID + "'";
            //                Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID + " = '" + Partida_ID + "'";
            //                Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + " = '" + Proyecto_Programa_ID + "'";
            //                Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + " = extract(year from sysdate)";
            //                Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + " = " + No_Asignacion;

            //                // Se da de alta la operación en el método "Alta_Bitacora"
            //                //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Alm_Com_Recepcion_Material.aspx", Proyecto_Programa_ID, Mi_SQL);

            //                //Ejecutar consulta
            //                Obj_Comando.CommandText = Mi_SQL;
            //                Obj_Comando.ExecuteNonQuery(); // Se ejecuta la operación 
            //            }
            //            else
            //            {
            //                // Escribir un mensaje que indica que no se actualizó el presupuesto  
            //            }

            //            // SE DISMINUYEN LOS PRODUCTOS
            //            //Consulta para el campo comprometido
            //            Mi_SQL = "SELECT " + Cat_Com_Productos.Campo_Comprometido + ", " + Cat_Com_Productos.Campo_Existencia + " ";
            //            Mi_SQL = Mi_SQL + "FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " ";
            //            Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = '" + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["PRODUCTO_ID"].ToString().Trim() + "'";

            //            //Ejecutar consulta
            //            Dt_Aux = new DataTable();
            //            Obj_Comando.CommandText = Mi_SQL;
            //            Obj_Adaptador.SelectCommand = Obj_Comando;
            //            Obj_Adaptador.Fill(Dt_Aux);

            //            //Verificar si la consulta arrojo resultado
            //            if (Dt_Aux.Rows.Count > 0)
            //            {
            //                //Asignar los valores de los montos
            //                if (Convert.IsDBNull(Dt_Aux.Rows[0][0]) == false)
            //                    Cantidad_Comprometida = Convert.ToInt32(Dt_Aux.Rows[0][0]);
            //                else
            //                    Cantidad_Comprometida = 0;

            //                if (Convert.IsDBNull(Dt_Aux.Rows[0][1]) == false)
            //                    Cantidad_Existente = Convert.ToInt32(Dt_Aux.Rows[0][1]);
            //                else
            //                    Cantidad_Existente = 0;
            //            }

            //            if ((Cantidad_Comprometida != 0) & (Cantidad_Existente != 0))
            //            {
            //                //Realizar los calculos de los montos
            //                Cantidad_Comprometida = Cantidad_Comprometida - Convert.ToInt64(Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["CANTIDAD_ENTREGADA"]);
            //                Cantidad_Existente = Cantidad_Existente - Convert.ToInt64(Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["CANTIDAD_ENTREGADA"]);

            //                //Consulta para modificar las cantidades en la base de datos
            //                Mi_SQL = "UPDATE " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " ";
            //                Mi_SQL = Mi_SQL + "SET " + Cat_Com_Productos.Campo_Comprometido + " = " + Cantidad_Comprometida.ToString().Trim() + ", ";
            //                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Existencia + " = " + Cantidad_Existente.ToString().Trim() + " ";
            //                Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = '" + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["PRODUCTO_ID"].ToString().Trim() + "'";

            //                //String Producto_ID = "" + Dt_Requisiciones_Detalles.Rows[Cont_Elementos][3].ToString().Trim();
            //                // Se registra  el Update en la bitacora
            //                //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Alm_Com_Orden_Salida.aspx", Producto_ID, Mi_SQL);

            //                //Ejecutar consulta
            //                Obj_Comando.CommandText = Mi_SQL;
            //                Obj_Comando.ExecuteNonQuery();
            //            }
            //        }
            //    }
            //    //Ejecutar transaccion
            //    Obj_Transaccion.Commit();

                //Entregar resultado
            //    return Datos.P_No_Orden_Salida;
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Informacion_General_OS
        ///DESCRIPCIÓN:          Método donde se consulta la información general de la orden de salida que se genero
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           24/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Informacion_General_OS(Cls_Ope_Com_Alm_Requisiciones_Stock_Negocio Datos)
        {
            // Declaración de variables
            String Mi_SQL = null;
            DataTable Dt_Cabecera = new DataTable();

            Mi_SQL = "SELECT " + "SALIDAS." + Alm_Com_Salidas.Campo_No_Salida + " as NO_ORDEN_SALIDA"; 
            Mi_SQL = Mi_SQL + ",(select DEPENDENCIAS."+ Cat_Dependencias.Campo_Nombre + " from ";
            Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS "; 
            Mi_SQL = Mi_SQL + " where SALIDAS."+ Alm_Com_Salidas.Campo_Dependencia_ID + " = DEPENDENCIAS." ;
            Mi_SQL = Mi_SQL+ Cat_Dependencias.Campo_Dependencia_ID + ")as UNIDAD_RESPONSABLE";

            Mi_SQL = Mi_SQL + ",(select distinct (FINANCIAMIENTO."+ Cat_SAP_Fuente_Financiamiento.Campo_Descripcion +")";
            Mi_SQL = Mi_SQL + " from " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + " FINANCIAMIENTO "; 
            Mi_SQL = Mi_SQL + "  where FINANCIAMIENTO."+ Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID;
            Mi_SQL = Mi_SQL + " = (select distinct(REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID + ") from ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO "; 
            Mi_SQL = Mi_SQL + " where  REQ_PRODUCTO." +  Ope_Com_Req_Producto.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS." + Alm_Com_Salidas.Campo_Requisicion_ID + "))as F_FINANCIAMIENTO" ;
            
            Mi_SQL = Mi_SQL + ",(select distinct (PROY_PROGRAMAS."+ Cat_Com_Proyectos_Programas.Campo_Descripcion + ")" ;
            Mi_SQL = Mi_SQL + " from " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + " PROY_PROGRAMAS "; 
            Mi_SQL = Mi_SQL + "  where PROY_PROGRAMAS."+ Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID;
            Mi_SQL = Mi_SQL + " =(select distinct (REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID + ") from ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO "; 
            Mi_SQL = Mi_SQL + " where  REQ_PRODUCTO." +  Ope_Com_Req_Producto.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS." +Alm_Com_Salidas.Campo_Requisicion_ID + "))as PROGRAMA" ;

            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio;
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion;
            Mi_SQL = Mi_SQL + ", SALIDAS." + Alm_Com_Salidas.Campo_Usuario_Creo + " as ENTREGO ";
            
            Mi_SQL = Mi_SQL + ", (select EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + " ||' '||";
            Mi_SQL = Mi_SQL + " EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + " ||' '||";
            Mi_SQL = Mi_SQL + " EMPLEADOS." + Cat_Empleados.Campo_Nombre;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS ";
            Mi_SQL = Mi_SQL + " where EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS." + Alm_Com_Salidas.Campo_Empleado_Solicito_ID + ") as RECIBIO";
            
            Mi_SQL = Mi_SQL + " FROM " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas+ " SALIDAS ";
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones+ " REQUISICIONES ";
            Mi_SQL = Mi_SQL + " where REQUISICIONES." +  Ope_Com_Requisiciones.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS." + Alm_Com_Salidas.Campo_Requisicion_ID+ "";
            Mi_SQL = Mi_SQL + " AND SALIDAS." + Alm_Com_Salidas.Campo_No_Salida+ " = ";
            Mi_SQL = Mi_SQL + Datos.P_No_Orden_Salida;

            Dt_Cabecera = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Cabecera;
        }



        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Detalles_Orden_Salida
        ///DESCRIPCIÓN:          Método donde se consultan los detalles de la orden de salida que se genero
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           24/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Detalles_Orden_Salida(Cls_Ope_Com_Alm_Requisiciones_Stock_Negocio Datos)
        {
            // Declaración de variables
            String Mi_SQL = null;
            DataTable Dt_Detalles = new DataTable();

            Mi_SQL = "SELECT SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_No_Salida + " as NO_ORDEN_SALIDA"; 
            Mi_SQL = Mi_SQL + ",(select PRODUCTOS."+ Cat_Com_Productos.Campo_Clave+ " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS "; 
            Mi_SQL = Mi_SQL + " where SALIDAS_DETALLES."+ Alm_Com_Salidas_Detalles.Campo_Producto_ID + " = PRODUCTOS." ;
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as CLAVE";

            Mi_SQL = Mi_SQL + ",(select PRODUCTOS."+ Cat_Com_Productos.Campo_Nombre+ " ||' '|| ";
            Mi_SQL = Mi_SQL + " PRODUCTOS." + Cat_Com_Productos.Campo_Descripcion+ " from " ;
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS "; 
            Mi_SQL = Mi_SQL + " where SALIDAS_DETALLES."+ Alm_Com_Salidas_Detalles.Campo_Producto_ID + " = PRODUCTOS." ;
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as PRODUCTO";

            Mi_SQL = Mi_SQL + ",(select REQ_PRODUCTOS."+ Ope_Com_Req_Producto.Campo_Cantidad+ " from ";
            Mi_SQL = Mi_SQL +  Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto+ " REQ_PRODUCTOS " ;
            Mi_SQL = Mi_SQL + " where  REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = "; 
            Mi_SQL = Mi_SQL + " (select SALIDAS."+ Alm_Com_Salidas.Campo_Requisicion_ID+ " from ";
            Mi_SQL = Mi_SQL +  Alm_Com_Salidas.Tabla_Alm_Com_Salidas + " SALIDAS " ;
            Mi_SQL = Mi_SQL + " where  SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_No_Salida + " = "; 
            Mi_SQL = Mi_SQL +  " SALIDAS." + Alm_Com_Salidas.Campo_No_Salida + ")";
            Mi_SQL = Mi_SQL + " and REQ_PRODUCTOS."+ Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + ") as CANTIDAD_SOLICITADA ";

            Mi_SQL = Mi_SQL + ",SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Cantidad + " as CANTIDAD_ENTREGADA"; 
            
            Mi_SQL = Mi_SQL + ",(select UNIDADES."+ Cat_Com_Unidades.Campo_Abreviatura+ " from ";
            Mi_SQL = Mi_SQL +  Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " UNIDADES " ;
            Mi_SQL = Mi_SQL + " where  UNIDADES." + Cat_Com_Unidades.Campo_Unidad_ID+ " = "; 
            Mi_SQL = Mi_SQL + " (select PRODUCTOS."+ Cat_Com_Productos.Campo_Unidad_ID + " from ";
            Mi_SQL = Mi_SQL +  Cat_Com_Productos.Tabla_Cat_Com_Productos+ " PRODUCTOS " ;
            Mi_SQL = Mi_SQL + " where  SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + " = ";
            Mi_SQL = Mi_SQL + " PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID + ")) as UNIDADES";

            Mi_SQL = Mi_SQL + ",SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Costo + " as PRECIO"; 
            Mi_SQL = Mi_SQL + ",SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Subtotal + "";          
            Mi_SQL = Mi_SQL + ",SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_IVA + "";
            Mi_SQL = Mi_SQL + ",SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Importe + " as TOTAL"; 
          
            Mi_SQL = Mi_SQL + " FROM " + Alm_Com_Salidas_Detalles.Tabla_Alm_Com_Salidas_Detalles + " SALIDAS_DETALLES"; 
            Mi_SQL = Mi_SQL + " WHERE SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_No_Salida + " = ";
            Mi_SQL = Mi_SQL + Datos.P_No_Orden_Salida;

            Dt_Detalles = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Detalles;
        }
    }
}