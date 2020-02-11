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
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Almacen_Recibos.Negocio;
using Presidencia.Sessiones;
using Presidencia.Bitacora_Eventos;

/// <summary>
/// Summary description for Cls_Alm_Com_Recibos_Datos
/// </summary>
namespace Presidencia.Almacen_Recibos.Datos
{
    public class Cls_Alm_Com_Recibos_Datos
    {
        public Cls_Alm_Com_Recibos_Datos()
        {
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Alta_Recibo
        /// DESCRIPCION:            Dar de alta un recibo de material
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene 
        ///                         los datos para el recibo
        /// CREO       :            Noe Mosqueda Valadez
        /// FECHA_CREO :            27/Noviembre/2010 14:00 
        /// MODIFICO          :     Salvador Hernández Ramírez
        /// FECHA_MODIFICO    :     31/Enero/2011 12:50 
        /// CAUSA_MODIFICACION:     Se instancio el metodo "Alta_Bitacora" para registrar los Upadate e Insert en la tabla "APL_BITACORA"
        ///*******************************************************************************/
        public static String Alta_Recibo(Cls_Alm_Com_Recibos_Negocio Datos)
        {
            //Declaracion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = String.Empty;
            Object Aux; //Variable auxiliar para las consultas
            String Mensaje = String.Empty; //Variable para el mensaje de error
            OracleDataAdapter Obj_Adaptador; //Adapatador para el llenado de las tablas auxiliares
            DataTable Dt_Aux = new DataTable(); //Tabla auxiliar para las consultas
            DataTable Dt_Requisiciones_Detalles = new DataTable(); //Tabla para los detalles de las requisiciones

            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Adaptador = new OracleDataAdapter();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;

                //Consulta para los ID de la dependencia, area, etc
                Mi_SQL = "SELECT " + Ope_Com_Requisiciones.Campo_Dependencia_ID + ", " + Ope_Com_Requisiciones.Campo_Area_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_Empleado_Generacion_ID + " ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = '" + Datos.P_No_Requisicion + "'";

                //Ejecutar consulta
                Dt_Aux.Clear();
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Adaptador.SelectCommand = Obj_Comando;
                Obj_Adaptador.Fill(Dt_Aux);

                //Verificar si la consulta arrojo resultado
                if (Dt_Aux.Rows.Count > 0)
                {
                    //Colocar los valores en las variables
                    Datos.P_Dependencia_ID = Dt_Aux.Rows[0][0].ToString().Trim();
                    Datos.P_Area_ID = Dt_Aux.Rows[0][1].ToString().Trim();
                    Datos.P_Empleado_Recibo_ID = Dt_Aux.Rows[0][2].ToString().Trim();
                }
                else
                {
                    throw new Exception("Datos no encontrados requisicion no " + Datos.P_No_Requisicion);
                }

                //Consulta para la actualización de la requisicion
                Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " ";
                Mi_SQL = Mi_SQL + "SET " + Ope_Com_Requisiciones.Campo_Estatus + " = 'SURTIDA', ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_Fecha_Surtido + " = SYSDATE, ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_Empleado_Surtido_ID + " = '" + Datos.P_Empleado_Recibo_ID + "' ";
                Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = '" + Datos.P_No_Requisicion + "' ";

                // Se registra  el update en la bitacora
                //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Alm_Com_Recibos.aspx", Datos.P_No_Requisicion.ToString(), Mi_SQL);

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                //Consulta para el maximo ID de las observaciones
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Com_Req_Observaciones.Campo_Observacion_ID + "), 0) ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Com_Req_Observaciones.Tabla_Ope_Com_Req_Observaciones + " ";

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Aux = Obj_Comando.ExecuteScalar();

                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false)
                    Datos.P_Observacion_ID = Convert.ToInt64(Aux) + 1;
                else
                    Datos.P_Observacion_ID = 1;


                //Consulta para agregar los comentarios
                Mi_SQL = "INSERT INTO " + Ope_Com_Req_Observaciones.Tabla_Ope_Com_Req_Observaciones;
                Mi_SQL = Mi_SQL + " (" + Ope_Com_Req_Observaciones.Campo_Observacion_ID + ", " + Ope_Com_Req_Observaciones.Campo_Requisicion_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Observaciones.Campo_Comentario + ", " + Ope_Com_Req_Observaciones.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Observaciones.Campo_Usuario_Creo + ", " + Ope_Com_Req_Observaciones.Campo_Fecha_Creo + ") ";
                Mi_SQL = Mi_SQL + "VALUES('" + Datos.P_Observacion_ID + "', '" + Datos.P_No_Requisicion + "', '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + "'SURTIDA', '" + Datos.P_Usuario + "', SYSDATE) ";

               

                // Se registra  el insert en la bitacora
                //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Alm_Com_Recibos.aspx", Convert.ToString(Datos.P_Observacion_ID), Mi_SQL);

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();


                //Consulta para el maximo ID del recibo
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Com_Recibos.Campo_No_Recibo + "), '0000000000') FROM " + Ope_Com_Recibos.Tabla_Ope_Com_Recibos + " ";

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Aux = Obj_Comando.ExecuteScalar();

                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false)
                    Datos.P_No_Recibo = String.Format("{0:0000000000}", Convert.ToInt32(Aux) + 1);
                else
                    Datos.P_No_Recibo = "0000000001";

                //Asignar consulta para el recibo
                Mi_SQL = "INSERT INTO " + Ope_Com_Recibos.Tabla_Ope_Com_Recibos + " (" + Ope_Com_Recibos.Campo_No_Recibo + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Recibos.Campo_Empleado_Recibo_ID + ", " + Ope_Com_Recibos.Campo_Empleado_Almacen_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Recibos.Campo_Fecha + ", " + Ope_Com_Recibos.Campo_Usuario_Creo + ", " + Ope_Com_Recibos.Campo_Fecha_Creo + ") ";
                Mi_SQL = Mi_SQL + "VALUES('" + Datos.P_No_Recibo + "', '" + Datos.P_Empleado_Recibo_ID + "', '" + Datos.P_Empleado_Almacen_ID + "', ";
                Mi_SQL = Mi_SQL + "SYSDATE, '" + Datos.P_Usuario + "', SYSDATE) ";

                // Se registra  el insert en la bitacora
                //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Alm_Com_Recibos.aspx", Datos.P_No_Recibo, Mi_SQL);

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                //Consulta para obtener el numero de entrada
                Mi_SQL = "SELECT " + Alm_Com_Entradas.Tabla_Alm_Com_Entradas + "." + Alm_Com_Entradas.Campo_No_Entrada + " ";
                Mi_SQL = Mi_SQL + "FROM " + Alm_Com_Entradas.Tabla_Alm_Com_Entradas + ", " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ", " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Alm_Com_Entradas.Tabla_Alm_Com_Entradas + "." + Alm_Com_Entradas.Campo_No_Factura_Interno;
                Mi_SQL = Mi_SQL + " = " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " ";
                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;
                Mi_SQL = Mi_SQL + " = " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_No_Orden_Compra + " ";
                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + Datos.P_No_Requisicion.ToString().Trim();

                //Mi_SQL = "SELECT " + Alm_Com_Entradas.Tabla_Alm_Com_Entradas + "." + Alm_Com_Entradas.Campo_No_Entrada + " ";
                //Mi_SQL = Mi_SQL + "FROM " + Alm_Com_Entradas.Tabla_Alm_Com_Entradas + ", " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + ", ";
                //Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ", " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones + ", ";
                //Mi_SQL = Mi_SQL + Ope_Com_Consolidacion_Req.Tabla_Ope_Com_Consolidacion_Req + " ";
                //Mi_SQL = Mi_SQL + "WHERE " + Alm_Com_Entradas.Tabla_Alm_Com_Entradas + "." + Alm_Com_Entradas.Campo_No_Factura_Interno;
                //Mi_SQL = Mi_SQL + " = " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " ";
                //Mi_SQL = Mi_SQL + "AND " + Alm_Com_Entradas.Tabla_Alm_Com_Entradas + "." + Alm_Com_Entradas.Campo_No_Factura_Interno;
                //Mi_SQL = Mi_SQL + " = " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " ";
                //Mi_SQL = Mi_SQL + "AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Cotizacion;
                //Mi_SQL = Mi_SQL + " = " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones + "." + Ope_Com_Cotizaciones.Campo_No_Cotizacion + " ";
                //Mi_SQL = Mi_SQL + "AND " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones + "." + Ope_Com_Cotizaciones.Campo_No_Cotizacion;
                //Mi_SQL = Mi_SQL + " = " + Ope_Com_Consolidacion_Req.Tabla_Ope_Com_Consolidacion_Req + "." + Ope_Com_Consolidacion_Req.Campo_No_Cotizacion + " ";
                //Mi_SQL = Mi_SQL + "AND " + Ope_Com_Consolidacion_Req.Tabla_Ope_Com_Consolidacion_Req + "." + Ope_Com_Consolidacion_Req.Campo_No_Requisicion + " = '" + Datos.P_No_Requisicion + "'";

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Aux = Obj_Comando.ExecuteOracleScalar();

                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false && Aux != null)
                    Datos.P_No_Entrada = Aux.ToString().Trim();
                else
                {
                    //throw new Exception("No existe entrada de producto");
                    Datos.P_No_Entrada = "0000000001";
                }

                //Consulta para los detalles de la requisicion
                Mi_SQL = "SELECT " + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + ", " + Ope_Com_Req_Producto.Campo_Cantidad + " ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = " + Datos.P_No_Requisicion + " ";

                //Ejecutar consulta
                Dt_Aux.Clear();
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Adaptador.SelectCommand = Obj_Comando;
                Obj_Adaptador.Fill(Dt_Requisiciones_Detalles);

                //Verificar si la tabla tiene datos
                if (Dt_Requisiciones_Detalles.Rows.Count > 0)
                {
                    //Ciclo para el barrido de la tabla
                    for (int Cont_Elementos = 0; Cont_Elementos < Dt_Requisiciones_Detalles.Rows.Count; Cont_Elementos++)
                    {
                        //Colocar el numero de recibo en los detalles de la entrada
                        Mi_SQL = "UPDATE " + Alm_Com_Entradas_Detalles.Tabla_Alm_Com_Entradas_Detalles + " ";
                        Mi_SQL = Mi_SQL + "SET " + Alm_Com_Entradas_Detalles.Campo_No_Recibo + " = '" + Datos.P_No_Recibo + "' ";
                        Mi_SQL = Mi_SQL + "WHERE " + Alm_Com_Entradas_Detalles.Campo_No_Entrada + " = '" + Datos.P_No_Entrada + "' ";
                        Mi_SQL = Mi_SQL + "AND " + Alm_Com_Entradas_Detalles.Campo_Producto_ID + " = '" + Dt_Requisiciones_Detalles.Rows[Cont_Elementos][0].ToString().Trim() + "'";

                        // Se registra  el update en la bitacora
                        //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Alm_Com_Recibos.aspx", Datos.P_No_Recibo, Mi_SQL);

                        // Ejecutar consulta
                        Obj_Comando.CommandText = Mi_SQL;
                        Obj_Comando.ExecuteNonQuery();
                    }
                }

                //Ejecutar transaccion
               Obj_Transaccion.Commit();

                //Entregar numero de recibo
                return Datos.P_No_Recibo;
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
                Obj_Comando = null;
                Obj_Conexion = null;
                Obj_Transaccion = null;
            }
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Requisicion_Detalles
        /// DESCRIPCION:            Realizar la consulta de los detalles de una requiscion
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene 
        ///                         los criterios de busqueda
        /// CREO       :            Noe Mosqueda Valadez
        /// FECHA_CREO :            12/Noviembre/2010 10:28 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consulta_Requisicion_Detalles(Cls_Alm_Com_Recibos_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty; //Variable para las consultas

            try
            {
                //Asignar consulta para la requisicion
                Mi_SQL = "SELECT " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Clave + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre + " AS PRODUCTO, ";
                Mi_SQL = Mi_SQL + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Nombre + " AS MODELO, ";
                Mi_SQL = Mi_SQL + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " AS MARCA, ";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Cantidad + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + "." + Cat_Com_Unidades.Campo_Abreviatura + " AS UNIDAD, ";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Importe + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Monto_Total + " ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + ", " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Producto_ID + " ";
                Mi_SQL = Mi_SQL + "AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Modelo_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Modelo_ID + " ";
                Mi_SQL = Mi_SQL + "AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Marca_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + " ";
                Mi_SQL = Mi_SQL + "AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Unidad_ID + " ";
                Mi_SQL = Mi_SQL + " = " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + "." + Cat_Com_Unidades.Campo_Unidad_ID + " ";
                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Requisicion_ID;
                Mi_SQL = Mi_SQL + " = " + Datos.P_No_Requisicion.ToString().Trim() + " ";

                //Entregar resultado
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Imprime_Recibo
        /// DESCRIPCION:            Consulta para los datos del recibo
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene 
        ///                         los datos del recibo
        /// CREO       :            Noe Mosqueda Valadez
        /// FECHA_CREO :            29/Noviembre/2010 17:13 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataSet Imprime_Recibo(Cls_Alm_Com_Recibos_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty; //Variable para las consultas
            DataSet Ds_Recibo = new DataSet();
            DataTable Dt_Cabecera = new DataTable(); //Tabla para la cabcera del recibo
            DataTable Dt_Detalles = new DataTable(); //Tabla para los detalles del recibos 
            DataTable Dt_Cabecera_tmp = new DataTable(); //Tabla para la cabcera del recibo
            DataTable Dt_Detalles_tmp = new DataTable(); //Tabla para los detalles del recibos 
            DataRow Renglon; //Renglon para el llenado de las tablas

            try
            {
                //Consulta para la cabecera
                Mi_SQL = "SELECT " + Ope_Com_Recibos.Tabla_Ope_Com_Recibos + "." + Ope_Com_Recibos.Campo_No_Recibo + ", ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA ";
                Mi_SQL = Mi_SQL + "(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ";
                Mi_SQL = Mi_SQL + "NVL(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ", '') || ' ' || ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS RESPONSABLE, ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + ", ";
                Mi_SQL = Mi_SQL + "(CAT_EMPLEADOS_2." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ";
                Mi_SQL = Mi_SQL + "NVL(CAT_EMPLEADOS_2." + Cat_Empleados.Campo_Apellido_Materno + ", '') || ' ' || ";
                Mi_SQL = Mi_SQL + "CAT_EMPLEADOS_2." + Cat_Empleados.Campo_Nombre + ") AS ALMACEN ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Com_Recibos.Tabla_Ope_Com_Recibos + ", " + Alm_Com_Entradas_Detalles.Tabla_Alm_Com_Entradas_Detalles + ", ";
                Mi_SQL = Mi_SQL + Alm_Com_Entradas.Tabla_Alm_Com_Entradas + ", " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + ", " + Cat_Empleados.Tabla_Cat_Empleados + " CAT_EMPLEADOS_2 ";
                Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Recibos.Tabla_Ope_Com_Recibos + "." + Ope_Com_Recibos.Campo_No_Recibo;
                Mi_SQL = Mi_SQL + " = " + Alm_Com_Entradas_Detalles.Tabla_Alm_Com_Entradas_Detalles + "." + Alm_Com_Entradas_Detalles.Campo_No_Recibo + " ";
                Mi_SQL = Mi_SQL + "AND " + Alm_Com_Entradas_Detalles.Tabla_Alm_Com_Entradas_Detalles + "." + Alm_Com_Entradas_Detalles.Campo_No_Entrada;
                Mi_SQL = Mi_SQL + " = " + Alm_Com_Entradas.Tabla_Alm_Com_Entradas + "." + Alm_Com_Entradas.Campo_No_Entrada + " ";
                Mi_SQL = Mi_SQL + "AND " + Alm_Com_Entradas.Tabla_Alm_Com_Entradas + "." + Alm_Com_Entradas.Campo_No_Factura_Interno;
                Mi_SQL = Mi_SQL + " = " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " ";
                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;
                Mi_SQL = Mi_SQL + " = " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_No_Orden_Compra + " ";
                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Recibos.Tabla_Ope_Com_Recibos + "." + Ope_Com_Recibos.Campo_Empleado_Recibo_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " ";
                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Recibos.Tabla_Ope_Com_Recibos + "." + Ope_Com_Recibos.Campo_Empleado_Almacen_ID;
                Mi_SQL = Mi_SQL + " = CAT_EMPLEADOS_2." + Cat_Empleados.Campo_Empleado_ID + " ";
                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " ";
                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Recibos.Tabla_Ope_Com_Recibos + "." + Ope_Com_Recibos.Campo_No_Recibo + " = " + Datos.P_No_Recibo.ToString().Trim() + " ";

                //Ejecutar consulta
                Dt_Cabecera_tmp = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                //Consulta para los detalles
                Mi_SQL = "SELECT " + Alm_Com_Entradas_Detalles.Tabla_Alm_Com_Entradas_Detalles + "." + Alm_Com_Entradas_Detalles.Campo_No_Recibo + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + " AS PROVEEDOR, ";
                Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Proveedor + "AS FACTURA, ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " AS ORDEN_COMPRA, ";
                Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "." + Ope_Com_Facturas_Proveedores.Campo_Fecha_Factura + " AS FECHA, ";
                Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "." + Ope_Com_Facturas_Proveedores.Campo_Total + " AS IMPORTE, ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " AS REQUISICION, ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre + " AS PRODUCTO, ";
                Mi_SQL = Mi_SQL + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Nombre + " AS MODELO, ";
                Mi_SQL = Mi_SQL + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " AS MARCA, ";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Cantidad + " AS CANTIDAD_SOLICITADA, ";
                Mi_SQL = Mi_SQL + Alm_Com_Entradas_Detalles.Tabla_Alm_Com_Entradas_Detalles + "." + Alm_Com_Entradas_Detalles.Campo_Cantidad + " AS CANTIDAD_EXISTENTE, ";
                Mi_SQL = Mi_SQL + Alm_Com_Entradas_Detalles.Tabla_Alm_Com_Entradas_Detalles + "." + Alm_Com_Entradas_Detalles.Campo_Costo_Compra + " AS COSTO ";
                Mi_SQL = Mi_SQL + "FROM " + Alm_Com_Entradas_Detalles.Tabla_Alm_Com_Entradas_Detalles + ", " + Alm_Com_Entradas.Tabla_Alm_Com_Entradas + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + ", " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ", " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + ", " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Alm_Com_Entradas_Detalles.Tabla_Alm_Com_Entradas_Detalles + "." + Alm_Com_Entradas_Detalles.Campo_No_Entrada;
                Mi_SQL = Mi_SQL + " = " + Alm_Com_Entradas.Tabla_Alm_Com_Entradas + "." + Alm_Com_Entradas.Campo_No_Entrada + " ";
                Mi_SQL = Mi_SQL + "AND " + Alm_Com_Entradas.Tabla_Alm_Com_Entradas + "." + Alm_Com_Entradas.Campo_No_Factura_Interno;
                Mi_SQL = Mi_SQL + " = " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " ";
                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno;
                Mi_SQL = Mi_SQL + " = " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " ";
                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;
                Mi_SQL = Mi_SQL + " = " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_No_Orden_Compra + " ";
                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
                Mi_SQL = Mi_SQL + " = " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " ";
                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Producto_ID + " ";
                Mi_SQL = Mi_SQL + "AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Modelo_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Modelo_ID + " ";
                Mi_SQL = Mi_SQL + "AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Marca_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + " ";
                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Recibos.Tabla_Ope_Com_Recibos + "." + Ope_Com_Recibos.Campo_No_Recibo + " = " + Datos.P_No_Recibo.ToString().Trim() + " ";

                //Ejecutar consulta
                Dt_Detalles_tmp = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                //Clonar tablas
                Dt_Cabecera = Dt_Cabecera_tmp.Clone();
                Dt_Cabecera.TableName = "Cabecera";
                Dt_Detalles = Dt_Detalles_tmp.Clone();
                Dt_Detalles.TableName = "Detalles";

                //Ciclos para los llenados de las tablas
                for (int Cont_Elementos = 0; Cont_Elementos < Dt_Cabecera_tmp.Rows.Count; Cont_Elementos++)
                {
                    //Instanciar e importar renglon
                    Renglon = Dt_Cabecera_tmp.Rows[Cont_Elementos];
                    Dt_Cabecera.ImportRow(Renglon);
                }

                for (int Cont_Elementos = 0; Cont_Elementos < Dt_Detalles_tmp.Rows.Count; Cont_Elementos++)
                {
                    //Instanciar e importar renglon
                    Renglon = Dt_Detalles_tmp.Rows[Cont_Elementos];
                    Dt_Detalles_tmp.ImportRow(Renglon);
                }

                //Colocar tablas en el dataset
                Ds_Recibo.Tables.Add(Dt_Cabecera);
                Ds_Recibo.Tables.Add(Dt_Detalles);

                //Entregar resultado
                return Ds_Recibo;
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Datos_Requisicion
        /// DESCRIPCION:            Consulta para los datos de la requisicion
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene 
        ///                         los datos de la requisicion
        /// CREO       :            Noe Mosqueda Valadez
        /// FECHA_CREO :            30/Noviembre/2010 9:55 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consulta_Datos_Requisicion(Cls_Alm_Com_Recibos_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty; //Variable para las consultas

            try
            {
                //Asignar consulta
                Mi_SQL = "SELECT " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA, ";
                Mi_SQL = Mi_SQL + Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Nombre + " AS AREA, ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Folio + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ";
                Mi_SQL = Mi_SQL + "(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' ";
                Mi_SQL = Mi_SQL + "|| NVL(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ", '') || ' ' ";
                Mi_SQL = Mi_SQL + "|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS EMPLEADO_GENERACION, ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Total + " ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + ", ";
                Mi_SQL = Mi_SQL + Cat_Areas.Tabla_Cat_Areas + ", " + Cat_Empleados.Tabla_Cat_Empleados + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " ";
                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Area_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Area_ID + " ";
                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Empleado_Generacion_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " ";
                //Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Estatus + " = 'AUTORIZADA' ";
                //Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Tipo + " = 'STOCK' ";
                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = '" + Datos.P_No_Requisicion + "' ";

                //Entregar resultado
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }
    }
}