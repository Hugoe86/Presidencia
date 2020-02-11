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
using Presidencia.Almacen_Orden_Salida.Negocio;
using Presidencia.Constantes;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Sessiones;
using Presidencia.Bitacora_Eventos;

/// <summary>
/// Summary description for Cls_Alm_Com_Orden_Salida_Datos
/// </summary>
namespace Presidencia.Almacen_Orden_Salida.Datos
{
    public class Cls_Alm_Com_Orden_Salida_Datos
    {
        public Cls_Alm_Com_Orden_Salida_Datos()
        {
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Requisiciones
        /// DESCRIPCION:            Realizar la consulta de las requisciones de acuerdo a 
        ///                         un criterio de busqueda
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene 
        ///                         los criterios de busqueda
        /// CREO       :            Noe Mosqueda Valadez
        /// FECHA_CREO :            12/Noviembre/2010 19:00 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consulta_Requisiciones(Cls_Alm_Com_Orden_Salida_Negocio Datos)
        {
            // Declaracion de variables
            String Mi_SQL = String.Empty;

            try
            {
                // Asignar consulta
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
                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + Datos.P_No_Requisicion.ToString().Trim() + " ";

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
        public static DataTable Consulta_Requisicion_Detalles(Cls_Alm_Com_Orden_Salida_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty;

            try
            {
                //Asignar consulta para la requisicion
                Mi_SQL = "SELECT " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Clave + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre + " AS PRODUCTO, ";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Cantidad + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Importe + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Monto_Total + ", ";
                
                Mi_SQL = Mi_SQL + "( SELECT NOMBRE FROM "+ Cat_Com_Modelos.Tabla_Cat_Com_Modelos + " WHERE MODELO_ID = ( SELECT MODELO_ID FROM ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " WHERE PRODUCTO_ID = ";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " )) AS MODELO, ";

                Mi_SQL = Mi_SQL + "( SELECT NOMBRE FROM "+ Cat_Com_Marcas.Tabla_Cat_Com_Marcas+ " WHERE MARCA_ID = ( SELECT MARCA_ID FROM ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " WHERE PRODUCTO_ID = ";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " )) AS MARCA, ";

                Mi_SQL = Mi_SQL + "( SELECT ABREVIATURA FROM "+ Cat_Com_Unidades.Tabla_Cat_Com_Unidades+ " WHERE UNIDAD_ID = ( SELECT UNIDAD_ID FROM ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " WHERE PRODUCTO_ID = ";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " )) AS UNIDAD ";

                Mi_SQL = Mi_SQL + "FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Producto_ID + " ";
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
        /// NOMBRE DE LA CLASE:     Alta_orden_Salida
        /// DESCRIPCION:            Dar de alta la orden de salida de material
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene 
        ///                         los datos para al operacion
        /// CREO       :            Noe Mosqueda Valadez
        /// FECHA_CREO :            20/Noviembre/2010 9:28 
        /// MODIFICO          :     Salvador Hernández Ramírez
        /// FECHA_MODIFICO    :     10/Mayo/2011
        /// CAUSA_MODIFICACION:     Se asigno codigo para actualizar los Montos y las cantidades de productos
        ///*******************************************************************************/
        public static long Alta_Orden_Salida(Cls_Alm_Com_Orden_Salida_Negocio Datos)
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
            DataTable Dt_Requisiciones_Detalles = new DataTable(); //Tabla para los detalles de las requisiciones

            Double Monto_Comprometido = 0.0; // Variable para el monto comprometido
            Double Monto_Disponible = 0.0;   // Variable para el monto disponible
            Double Monto_Ejercido = 0.0;    // Variable para el monto ejercido

            String No_Asignacion = String.Empty; // Variable para el No de Asignacion            
            String Partida_ID = String.Empty; // Variable para el ID de la partida
            String Proyecto_Programa_ID = String.Empty; // Variable para el ID del programa o proyecto
            String Dependencia_ID = String.Empty; // Variable para el ID de la dependencia
            Double Monto_Total = 0.0; // Variable para el monto total de los detalles de la requisicion
            
            // Variables utilizadas para actualizar los productos
            int Cantidad_Comprometida = 0; // Variable para la cantidad Comprometida
            int Cantidad_Existente = 0; // Variable para la cantidad Existente
            int Cantidad_Disponible = 0; //Variable para la cantidad Disponible

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
                    Datos.P_No_Salida =  Convert.ToInt64(Aux) + 1;
                else
                    Datos.P_No_Salida = 1;

                // Consulta para los ID de la dependencia, area, etc
                Mi_SQL = "SELECT " + Ope_Com_Requisiciones.Campo_Dependencia_ID + ", " + Ope_Com_Requisiciones.Campo_Area_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_Empleado_Generacion_ID + " ";
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
                    // Colocar los valores en las variables
                    Datos.P_Dependencia_ID = Dt_Aux.Rows[0][0].ToString().Trim();
                    Datos.P_Area_ID = Dt_Aux.Rows[0][1].ToString().Trim();
                    Datos.P_Empleado_Solicito_ID = Dt_Aux.Rows[0][2].ToString().Trim();

                    //El tipo de salida es la 1
                    Datos.P_Tipo_Salida_ID = "00001";
                }
                else
                {
                    throw new Exception("Datos no encontrados requisicion no " + Datos.P_No_Requisicion.ToString().Trim());
                }
                
                // Consulta para dar de alta la salida
                Mi_SQL = "INSERT INTO " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + " (" + Alm_Com_Salidas.Campo_No_Salida + ", ";
                Mi_SQL = Mi_SQL + Alm_Com_Salidas.Campo_Dependencia_ID + ", " + Alm_Com_Salidas.Campo_Area_ID + ", ";
                Mi_SQL = Mi_SQL + Alm_Com_Salidas.Campo_Empleado_Solicito_ID + ", " + Alm_Com_Salidas.Campo_Requisicion_ID + ", ";
                Mi_SQL = Mi_SQL + Alm_Com_Salidas.Campo_Tipo_Salida_ID + ", " + Alm_Com_Salidas.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Alm_Com_Salidas.Campo_Fecha_Creo + ", " + Alm_Com_Salidas.Campo_Empleado_Almacen_ID + ") ";
                Mi_SQL = Mi_SQL + "VALUES(" + Datos.P_No_Salida + ", ";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Dependencia_ID + "', '" + Datos.P_Area_ID + "', '" + Datos.P_Empleado_Solicito_ID + "', ";
                Mi_SQL = Mi_SQL + Datos.P_No_Requisicion.ToString().Trim() + ", '" + Datos.P_Tipo_Salida_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Usuario + "', SYSDATE, '" + Datos.P_Empleado_Surtido_ID + "')";

                String No_Salida = Convert.ToString(Datos.P_No_Salida);

                // Se registra  el Insert en la bitacora
                //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Alm_Com_Orden_Salida.aspx", No_Salida, Mi_SQL);

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                //Consulta para la actualizacion de la requisicion
                Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " ";
                Mi_SQL = Mi_SQL + "SET " + Ope_Com_Requisiciones.Campo_Estatus + " = 'SURTIDA', ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_Fecha_Surtido + " = SYSDATE, ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_Empleado_Surtido_ID + " = '" + Datos.P_Empleado_Surtido_ID + "' ";
                Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + Datos.P_No_Requisicion.ToString().Trim() + " ";

                String No_Requisicion = Convert.ToString(Datos.P_No_Requisicion);

                // Se registra  el update en la bitacora
                //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Alm_Com_Orden_Salida.aspx", No_Requisicion, Mi_SQL);

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
                Mi_SQL = Mi_SQL + "VALUES(" + Datos.P_Observacion_ID + ", " + Datos.P_No_Requisicion.ToString().Trim() + ", '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + "'SURTIDA', '" + Datos.P_Usuario + "', SYSDATE) ";

                String Observacion_ID = Convert.ToString(Datos.P_Observacion_ID);

                // Se registra  el Insert en la bitacora
                //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Alm_Com_Orden_Salida.aspx", Observacion_ID, Mi_SQL);

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                //Consulta para los detalles de la requisicion
                Mi_SQL = "SELECT " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Partida_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Monto_Total + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Cantidad + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Costo + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Costo_Promedio + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID + " ";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Producto_ID + " ";
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Requisicion_ID;
                Mi_SQL = Mi_SQL + " = " + Datos.P_No_Requisicion.ToString().Trim() + " ";
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
                Mi_SQL = Mi_SQL + " = " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Requisicion_ID;

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Adaptador.SelectCommand = Obj_Comando;
                Obj_Adaptador.Fill(Dt_Requisiciones_Detalles);

                //Verificar si tiene datos
                if (Dt_Requisiciones_Detalles.Rows.Count > 0)
                {
                    //Ciclo para el desplazamiento de la tabla
                    for (int Cont_Elementos=0; Cont_Elementos < Dt_Requisiciones_Detalles.Rows.Count; Cont_Elementos++)
                    {
                        //Consulta para dar de alta los detalles de la salida
                        Mi_SQL = "INSERT INTO " + Alm_Com_Salidas_Detalles.Tabla_Alm_Com_Salidas_Detalles + " (" + Alm_Com_Salidas_Detalles.Campo_No_Salida + ", ";
                        Mi_SQL = Mi_SQL + Alm_Com_Salidas_Detalles.Campo_Producto_ID + ", " + Alm_Com_Salidas_Detalles.Campo_Cantidad + ", ";
                        Mi_SQL = Mi_SQL + Alm_Com_Salidas_Detalles.Campo_Costo + ", " + Alm_Com_Salidas_Detalles.Campo_Costo_Promedio + ", ";
                        Mi_SQL = Mi_SQL + Alm_Com_Salidas_Detalles.Campo_Importe + ") VALUES(" + Datos.P_No_Salida + ", ";
                        Mi_SQL = Mi_SQL + "'" + Dt_Requisiciones_Detalles.Rows[Cont_Elementos]["PROD_SERV_ID"].ToString().Trim() + "', ";
                        Mi_SQL = Mi_SQL + Dt_Requisiciones_Detalles.Rows[Cont_Elementos]["CANTIDAD"].ToString().Trim() + ", ";
                        Mi_SQL = Mi_SQL + Dt_Requisiciones_Detalles.Rows[Cont_Elementos]["COSTO"].ToString().Trim() + ", ";
                        Mi_SQL = Mi_SQL + Dt_Requisiciones_Detalles.Rows[Cont_Elementos]["COSTO_PROMEDIO"].ToString().Trim() + ", ";
                        //Mi_SQL = Mi_SQL + Convert.ToString(Convert.ToDouble(Dt_Requisiciones_Detalles.Rows[Cont_Elementos]["CANTIDAD"]) * Convert.ToDouble(Dt_Requisiciones_Detalles.Rows[Cont_Elementos]["COSTO_PROMEDIO"])) + ")";
                        Mi_SQL = Mi_SQL + Dt_Requisiciones_Detalles.Rows[Cont_Elementos]["MONTO_TOTAL"].ToString().Trim() + ")";

                        String N_Salida = Convert.ToString(Datos.P_No_Salida);
                        // Se registra  el Insert en la bitacora
                        //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Alm_Com_Orden_Salida.aspx", N_Salida, Mi_SQL);

                        //Ejecutar consulta
                        Obj_Comando.CommandText = Mi_SQL;
                        Obj_Comando.ExecuteNonQuery();


                // SE ACTUALIZAN LOS MONTOS 
                        // Asignar el ID de la partida y el ID del proyecto o programa
                        Partida_ID = Dt_Requisiciones_Detalles.Rows[Cont_Elementos]["PARTIDA_ID"].ToString().Trim();
                        Proyecto_Programa_ID = Dt_Requisiciones_Detalles.Rows[Cont_Elementos]["PROYECTO_PROGRAMA_ID"].ToString().Trim();
                        Dependencia_ID = Dt_Requisiciones_Detalles.Rows[Cont_Elementos]["DEPENDENCIA_ID"].ToString().Trim();
                        //Verificar si no es nulo
                        if (Convert.IsDBNull(Dt_Requisiciones_Detalles.Rows[Cont_Elementos]["MONTO_TOTAL"]) == false)
                            Monto_Total = Convert.ToDouble(Dt_Requisiciones_Detalles.Rows[Cont_Elementos]["MONTO_TOTAL"]);
                        else
                            Monto_Total = 0;

                        // Consulta para obtener el mayor numero de asignación
                        Mi_SQL = "SELECT  MAX (" + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ") ";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " = '" + Dependencia_ID+ "'";
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
                        Mi_SQL = Mi_SQL + " = '" +  Proyecto_Programa_ID + "'";
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

                            if (Convert.IsDBNull(Dt_Aux_Presupuestos.Rows[0]["MONTO_DISPONIBLE"]) == false)
                                Monto_Disponible = Convert.ToDouble(Dt_Aux_Presupuestos.Rows[0]["MONTO_DISPONIBLE"]);// Obtener el  MONTO DISPONIBLE y le resta el MONTO TOTAL del producto
                            else
                                Monto_Disponible = 0;

                            // Actualizar la tabla de los presupuestos
                            Mi_SQL = " UPDATE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + " ";
                            Mi_SQL = Mi_SQL + " SET " + Cat_Com_Dep_Presupuesto.Campo_Monto_Ejercido + " = " + Monto_Ejercido + ", ";
                            Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + " = " + Monto_Comprometido + ", ";
                            Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + " = " + Monto_Disponible + "";
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
                        Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = '" + Dt_Requisiciones_Detalles.Rows[Cont_Elementos][3].ToString().Trim() + "'";

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
                        else
                        {
                            // Asignar los valores de los montos
                            Cantidad_Comprometida = 0;
                            Cantidad_Existente = 0;
                        }

                        //Realizar los calculos de los montos
                        Cantidad_Comprometida = Cantidad_Comprometida - Convert.ToInt32(Dt_Requisiciones_Detalles.Rows[Cont_Elementos][4]);
                        Cantidad_Existente= Cantidad_Existente - Convert.ToInt32(Dt_Requisiciones_Detalles.Rows[Cont_Elementos][4]);
                        Cantidad_Disponible = Cantidad_Existente;

                        //Consulta para modificar las cantidades en la base de datos
                        Mi_SQL = "UPDATE " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " ";
                        Mi_SQL = Mi_SQL + "SET " + Cat_Com_Productos.Campo_Comprometido + " = " + Cantidad_Comprometida.ToString().Trim() + ", ";
                        Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Existencia + " = " + Cantidad_Existente.ToString().Trim() + ", ";
                        Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Disponible + " = " + Cantidad_Disponible.ToString().Trim() + " ";
                        Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = '" + Dt_Requisiciones_Detalles.Rows[Cont_Elementos][3].ToString().Trim() + "'";

                        String Producto_ID = "" + Dt_Requisiciones_Detalles.Rows[Cont_Elementos][3].ToString().Trim();

                        // Se registra  el Update en la bitacora
                        //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Alm_Com_Orden_Salida.aspx", Producto_ID, Mi_SQL);

                        //Ejecutar consulta
                        Obj_Comando.CommandText = Mi_SQL;
                        Obj_Comando.ExecuteNonQuery();
                    }
                }
                //Ejecutar transaccion
                Obj_Transaccion.Commit();

                //Entregar resultado
                return Datos.P_No_Salida;
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
        /// NOMBRE DE LA CLASE:     Imprime_orden_Salida
        /// DESCRIPCION:            Consultar los datos para la impresion de la orden de salida
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene 
        ///                         los datos para la operacion
        /// CREO       :            Noe Mosqueda Valadez
        /// FECHA_CREO :            24/Noviembre/2010 16:25 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataSet Imprime_Orden_Salida(Cls_Alm_Com_Orden_Salida_Negocio Datos)
        {
            //Declaracion de variables
            DataSet Ds_Orden_Salida = new DataSet(); //Dataset que contiene los datos de la orden de salida
            DataTable Dt_Cabecera = new DataTable("Cabecera"); //Tabla para la cabecera
            DataTable Dt_Detalles = new DataTable("Detalles"); //Tabla para los detalles
            DataTable Dt_Cabecera_tmp = new DataTable("Cabecera"); //Tabla para la cabecera
            DataTable Dt_Detalles_tmp = new DataTable("Detalles"); //Tabla para los detalles
            String Mi_SQL = String.Empty; //Variable para las consultas
            DataRow Renglon; //Renglon para el llenado de las tablas

            try
            {
                //Asignar consulta para la cabecera
                Mi_SQL = "SELECT " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + "." + Alm_Com_Salidas.Campo_No_Salida + ", ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DIRECCION, ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Codigo_Programatico + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Folio + " AS REQUISICION, ";
                Mi_SQL = Mi_SQL + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + "." + Alm_Com_Salidas.Campo_Fecha_Creo + " AS FECHA " + ", ";
                Mi_SQL = Mi_SQL + "(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " || ' ' || ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ";
                Mi_SQL = Mi_SQL + "NVL(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ", '')) AS ENTREGADO, ";
                Mi_SQL = Mi_SQL + "(CAT_EMPLEADOS_2." + Cat_Empleados.Campo_Nombre + " || ' ' || ";
                Mi_SQL = Mi_SQL + "CAT_EMPLEADOS_2." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ";
                Mi_SQL = Mi_SQL + "NVL(CAT_EMPLEADOS_2." + Cat_Empleados.Campo_Apellido_Materno + ", '')) AS RESPONSABLE, (ROWNUM - ROWNUM) AS DESCUENTO, ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Subtotal + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_IVA + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_IEPS + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Total + " ";               
                Mi_SQL = Mi_SQL + "FROM " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + ", " + Cat_Empleados.Tabla_Cat_Empleados + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + " CAT_EMPLEADOS_2, " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + "." + Alm_Com_Salidas.Campo_Requisicion_ID;
                Mi_SQL = Mi_SQL + " = " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " ";
                Mi_SQL = Mi_SQL + "AND " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + "." + Alm_Com_Salidas.Campo_Empleado_Solicito_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " ";
                Mi_SQL = Mi_SQL + "AND " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + "." + Alm_Com_Salidas.Campo_Empleado_Almacen_ID;
                Mi_SQL = Mi_SQL + " = CAT_EMPLEADOS_2." + Cat_Empleados.Campo_Empleado_ID + " ";
                Mi_SQL = Mi_SQL + "AND " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + "." + Alm_Com_Salidas.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " ";               
                Mi_SQL = Mi_SQL + "AND " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + "." + Alm_Com_Salidas.Campo_No_Salida + " = " + Datos.P_No_Salida.ToString().Trim() + " ";

                //Ejecutar consulta
                Dt_Cabecera_tmp = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                //Asignar consulta para los detalles
                Mi_SQL = "SELECT " + Alm_Com_Salidas_Detalles.Tabla_Alm_Com_Salidas_Detalles + "." + Alm_Com_Salidas_Detalles.Campo_No_Salida + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Clave + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre + " AS PRODUCTO, ";
                Mi_SQL = Mi_SQL + Alm_Com_Salidas_Detalles.Tabla_Alm_Com_Salidas_Detalles + "." + Alm_Com_Salidas_Detalles.Campo_Cantidad + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + "." + Cat_Com_Unidades.Campo_Abreviatura + " AS UNIDADES, ";
                Mi_SQL = Mi_SQL + Alm_Com_Salidas_Detalles.Tabla_Alm_Com_Salidas_Detalles + "." + Alm_Com_Salidas_Detalles.Campo_Costo + " AS PRECIO, ";
                Mi_SQL = Mi_SQL + Alm_Com_Salidas_Detalles.Tabla_Alm_Com_Salidas_Detalles + "." + Alm_Com_Salidas_Detalles.Campo_Importe + " AS TOTAL, ";

                Mi_SQL = Mi_SQL + "( SELECT NOMBRE FROM " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + " WHERE MODELO_ID = ( SELECT MODELO_ID FROM ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " WHERE PRODUCTO_ID = ";
                Mi_SQL = Mi_SQL + Alm_Com_Salidas_Detalles.Tabla_Alm_Com_Salidas_Detalles + "." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + " )) AS MODELO, ";

                Mi_SQL = Mi_SQL + "( SELECT NOMBRE FROM " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " WHERE MARCA_ID = ( SELECT MARCA_ID FROM ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " WHERE PRODUCTO_ID = ";
                Mi_SQL = Mi_SQL +  Alm_Com_Salidas_Detalles.Tabla_Alm_Com_Salidas_Detalles + "." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + " )) AS MARCA  ";

                Mi_SQL = Mi_SQL + " FROM " + Alm_Com_Salidas_Detalles.Tabla_Alm_Com_Salidas_Detalles + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " ";
                Mi_SQL = Mi_SQL + " WHERE " + Alm_Com_Salidas_Detalles.Tabla_Alm_Com_Salidas_Detalles + "." + Alm_Com_Salidas_Detalles.Campo_Producto_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Producto_ID + " ";
               
                Mi_SQL = Mi_SQL + "AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Unidad_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + "." + Cat_Com_Unidades.Campo_Unidad_ID + " ";
                Mi_SQL = Mi_SQL + "AND " + Alm_Com_Salidas_Detalles.Tabla_Alm_Com_Salidas_Detalles + "." + Alm_Com_Salidas_Detalles.Campo_No_Salida;
                Mi_SQL = Mi_SQL + " = " + Datos.P_No_Salida.ToString().Trim() + " ";

                //Ejecutar consulta
                Dt_Detalles_tmp = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                //Clonar tablas
                Dt_Cabecera = Dt_Cabecera_tmp.Clone();
                Dt_Cabecera.TableName = "Cabecera";
                Dt_Detalles = Dt_Detalles_tmp.Clone();
                Dt_Detalles.TableName = "Detalles";

                //Llenar las tablas
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
                    Dt_Detalles.ImportRow(Renglon);
                }

                //Colocar tablas en el dataset
                Ds_Orden_Salida.Tables.Add(Dt_Cabecera);
                Ds_Orden_Salida.Tables.Add(Dt_Detalles);

                //Entregar resultado
                return Ds_Orden_Salida;
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