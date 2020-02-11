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
using Presidencia.Almacen_Generar_Kardex_Productos.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

/// <summary>
/// Summary description for Cls_Ope_Alm_Generar_Kardex_Productos_Datos
/// </summary>
namespace Presidencia.Almacen_Generar_Kardex_Productos.Datos { 

    public class Cls_Ope_Alm_Generar_Kardex_Productos_Datos {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cls_Ope_Alm_Generar_Kardex_Productos_Negocio
        ///DESCRIPCIÓN: Obtener los detalles del Producto.
        ///PROPIEDADES: 
        ///             1.Parametros_Negocio. Parametros.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 26/Septiembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Obtener_Detalles_Producto(Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Parametros_Negocio) {
            Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Kardex_Negocio = new Cls_Ope_Alm_Generar_Kardex_Productos_Negocio();
            String Mi_SQL = null;
            OracleDataReader Data_Reader = null;
            try {
                Mi_SQL = "SELECT * FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos;
                if (Parametros_Negocio.P_Producto_ID != null && Parametros_Negocio.P_Producto_ID.Trim().Length > 0) { 
                    Mi_SQL =Mi_SQL + " WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = '" + Parametros_Negocio.P_Producto_ID.Trim() + "'";
                } else if (Parametros_Negocio.P_Clave != null && Parametros_Negocio.P_Clave.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Productos.Campo_Clave + " = '" + Parametros_Negocio.P_Clave.Trim() + "'";
                }
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                while (Data_Reader.Read()) {
                    Kardex_Negocio.P_Producto_ID = (!String.IsNullOrEmpty(Data_Reader[Cat_Com_Productos.Campo_Producto_ID].ToString())) ? Data_Reader[Cat_Com_Productos.Campo_Producto_ID].ToString().Trim() : null;
                    Kardex_Negocio.P_Clave = (!String.IsNullOrEmpty(Data_Reader[Cat_Com_Productos.Campo_Clave].ToString())) ? Data_Reader[Cat_Com_Productos.Campo_Clave].ToString().Trim() : null;
                    Kardex_Negocio.P_Estatus = (!String.IsNullOrEmpty(Data_Reader[Cat_Com_Productos.Campo_Estatus].ToString())) ? Data_Reader[Cat_Com_Productos.Campo_Estatus].ToString().Trim() : null;
                    Kardex_Negocio.P_Descripcion = (!String.IsNullOrEmpty(Data_Reader[Cat_Com_Productos.Campo_Nombre].ToString())) ? Data_Reader[Cat_Com_Productos.Campo_Nombre].ToString().Trim() : null;
                    if (!String.IsNullOrEmpty(Data_Reader[Cat_Com_Productos.Campo_Descripcion].ToString())) {
                        Kardex_Negocio.P_Descripcion = Kardex_Negocio.P_Descripcion + " [ " + Data_Reader[Cat_Com_Productos.Campo_Descripcion].ToString().Trim() + " ].";
                    }
                    Kardex_Negocio.P_Modelo = (!String.IsNullOrEmpty(Data_Reader[Cat_Com_Productos.Campo_Modelo].ToString())) ? Data_Reader[Cat_Com_Productos.Campo_Modelo].ToString().Trim() : null;
                    Kardex_Negocio.P_Marca = (!String.IsNullOrEmpty(Data_Reader[Cat_Com_Productos.Campo_Marca_ID].ToString())) ? Data_Reader[Cat_Com_Productos.Campo_Marca_ID].ToString().Trim() : null;
                    Kardex_Negocio.P_Unidad = (!String.IsNullOrEmpty(Data_Reader[Cat_Com_Productos.Campo_Unidad_ID].ToString())) ? Data_Reader[Cat_Com_Productos.Campo_Unidad_ID].ToString().Trim() : null;
                    Kardex_Negocio.P_Existencias = (!String.IsNullOrEmpty(Data_Reader[Cat_Com_Productos.Campo_Existencia].ToString())) ? Convert.ToInt32(Data_Reader[Cat_Com_Productos.Campo_Existencia].ToString().Trim()) : (-1);
                    Kardex_Negocio.P_Total_Comprometido = (!String.IsNullOrEmpty(Data_Reader[Cat_Com_Productos.Campo_Comprometido].ToString())) ? Convert.ToInt32(Data_Reader[Cat_Com_Productos.Campo_Comprometido].ToString().Trim()) : (-1);
                    Kardex_Negocio.P_Disponible = (!String.IsNullOrEmpty(Data_Reader[Cat_Com_Productos.Campo_Disponible].ToString())) ? Convert.ToInt32(Data_Reader[Cat_Com_Productos.Campo_Disponible].ToString().Trim()) : (-1);
                    Kardex_Negocio.P_Inicial = (!String.IsNullOrEmpty(Data_Reader[Cat_Com_Productos.Campo_Inicial].ToString())) ? Convert.ToInt32(Data_Reader[Cat_Com_Productos.Campo_Inicial].ToString().Trim()) : (-1);
                }
                Data_Reader.Close();
            } catch (Exception Ex) {
                throw new Exception("Excepción al Consultar los detalles del Producto: '" + Ex.Message + "'");
            }
            return Kardex_Negocio;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Detalles_Kardex
        ///DESCRIPCIÓN: Obtener los detalles del Kardex.
        ///PROPIEDADES: 
        ///             1.Parametros_Negocio. Parametros.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 26/Septiembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Obtener_Detalles_Kardex(Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Parametros_Negocio) {
            Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Kardex_Negocio = new Cls_Ope_Alm_Generar_Kardex_Productos_Negocio();
            DataTable Dt_Datos = new DataTable();
            String Mi_SQL = null;
            try {
                Dt_Datos = Consultar_Entradas_Productos(Parametros_Negocio);
                if (Dt_Datos == null) {
                    Dt_Datos = new DataTable();
                }
                Kardex_Negocio.P_Dt_Entradas = Dt_Datos;
                Dt_Datos = new DataTable();
                Dt_Datos = Consultar_Salidas_Productos(Parametros_Negocio);
                if (Dt_Datos == null) {
                    Dt_Datos = new DataTable();
                }
                Kardex_Negocio.P_Dt_Salidas = Dt_Datos;
                //Dt_Datos = Consultar_Comprometidos_Productos(Parametros_Negocio);
                Dt_Datos = Consultar_Compromisos(Parametros_Negocio);
                if (Dt_Datos == null) {
                    Dt_Datos = new DataTable();
                }
                Kardex_Negocio.P_Dt_Comprometidos = Dt_Datos;
                Dt_Datos = Consultar_Entradas_Ajuste_Productos(Parametros_Negocio);
                if (Dt_Datos == null) {
                    Dt_Datos = new DataTable();
                }
                Kardex_Negocio.P_Dt_Entradas_Ajuste = Dt_Datos;
                Dt_Datos = Consultar_Salidas_Ajuste_Productos(Parametros_Negocio);
                if (Dt_Datos == null) {
                    Dt_Datos = new DataTable();
                }
                Kardex_Negocio.P_Dt_Salidas_Ajuste = Dt_Datos;
            } catch (Exception Ex) {
                 throw new Exception("Excepción al Consultar los detalles del Producto: '" + Ex.Message + "'");
            }
            return Kardex_Negocio;
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consultar_Entradas_Productos
        /// DESCRIPCION:            Método utilizado para consultar las entradas del producto seleccionado por el usuario
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene la información para realizar la consulta
        ///                         
        /// CREO       :            Salvador Hernandez Ramirez
        /// FECHA_CREO :            13/Agosto/2011  
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consultar_Entradas_Productos(Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Datos)
        {
            String Mi_SQL = String.Empty; //Variable para las consultas

            try
            {
                // Asignar consulta
                Mi_SQL = "SELECT DISTINCT PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID + ", ";
                Mi_SQL = Mi_SQL + " PRODUCTOS." + Cat_Com_Productos.Campo_Clave + ", ";
                Mi_SQL = Mi_SQL + " REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Cantidad + ", ";
                Mi_SQL = Mi_SQL + " REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio + " AS REQUISICION, ";
                Mi_SQL = Mi_SQL + " REQUISICIONES." + Ope_Com_Requisiciones.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + " REQUISICIONES." + Ope_Com_Requisiciones.Campo_Fecha_Surtido + " as FECHA, ";
                Mi_SQL = Mi_SQL + " REQUISICIONES." + Ope_Com_Requisiciones.Campo_Estatus + " as REGISTRO_ID, ";

                Mi_SQL = Mi_SQL + " (select ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Folio + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDENES_COMPRA ";
                Mi_SQL = Mi_SQL + " WHERE REQUISICIONES." + Ope_Com_Requisiciones.Campo_No_Orden_Compra + " = ";
                Mi_SQL = Mi_SQL + " ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + ") AS NO_ENTRADA ";

                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO, ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES, ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";

                Mi_SQL = Mi_SQL + " WHERE REQ_PRODUCTO." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = ";
                Mi_SQL = Mi_SQL + " REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " ";
                Mi_SQL = Mi_SQL + " and PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID + " = ";
                Mi_SQL = Mi_SQL + " REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " ";
                Mi_SQL = Mi_SQL + " AND REQUISICIONES." + Ope_Com_Requisiciones.Campo_Estatus + " = 'SURTIDA'";
                Mi_SQL = Mi_SQL + " AND REQUISICIONES." + Ope_Com_Requisiciones.Campo_Listado_Almacen + "='SI'";
                Mi_SQL = Mi_SQL + " AND PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID + " = '" + Datos.P_Producto_ID + "'";
                if (Datos.P_Tomar_Fecha_Inicio) {
                    Mi_SQL = Mi_SQL + " AND REQUISICIONES." + Ope_Com_Requisiciones.Campo_Fecha_Surtido + " >= '" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Inicio) + "'";
                }
                if (Datos.P_Tomar_Fecha_Fin) {
                    Mi_SQL = Mi_SQL + " AND REQUISICIONES." + Ope_Com_Requisiciones.Campo_Fecha_Surtido + " < '" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Fin.AddDays(1)) + "'";
                }
                Mi_SQL = Mi_SQL + " order by REQUISICIONES." + Ope_Com_Requisiciones.Campo_Fecha_Surtido;

                // Entregar resultado
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
        /// NOMBRE DE LA CLASE:     Consultar_Entradas_Productos
        /// DESCRIPCION:            Método utilizado para consultar las entradas del producto seleccionado por el usuario
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene la información para realizar la consulta
        ///                         
        /// CREO       :            Salvador Hernandez Ramirez
        /// FECHA_CREO :            13/Agosto/2011  
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consultar_Salidas_Productos(Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Datos)
        {
            String Mi_SQL = String.Empty; //Variable para las consultas

            try
            {
                // Asignar consulta
                Mi_SQL = "SELECT DISTINCT SALIDA_DETALLES." + Alm_Com_Salidas_Detalles.Campo_No_Salida + ", ";
                Mi_SQL = Mi_SQL + " REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + ", ";
                Mi_SQL = Mi_SQL + " SALIDA_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Cantidad + ", ";
                Mi_SQL = Mi_SQL + " SALIDAS." + Alm_Com_Salidas.Campo_Fecha_Creo + " as FECHA, ";
                Mi_SQL = Mi_SQL + " (select REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";
                Mi_SQL = Mi_SQL + " WHERE REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = ";
                Mi_SQL = Mi_SQL + " SALIDAS." + Alm_Com_Salidas.Campo_Requisicion_ID + ") as REQUISICION, ";
                Mi_SQL = Mi_SQL + " SALIDAS." + Alm_Com_Salidas.Campo_Fecha_Creo + " as REGISTRO_ID ";

                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO, ";
                Mi_SQL = Mi_SQL + Alm_Com_Salidas_Detalles.Tabla_Alm_Com_Salidas_Detalles + " SALIDA_DETALLES, ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS, ";
                Mi_SQL = Mi_SQL + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + " SALIDAS ";

                Mi_SQL = Mi_SQL + " WHERE REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = ";
                Mi_SQL = Mi_SQL + " SALIDA_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + " ";
                Mi_SQL = Mi_SQL + " AND REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " ";
                Mi_SQL = Mi_SQL + " = PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID + " ";
                Mi_SQL = Mi_SQL + " AND PRODUCTOS." + Cat_Com_Productos.Campo_Stock + " ='SI' ";
                Mi_SQL = Mi_SQL + " AND SALIDAS." + Alm_Com_Salidas.Campo_No_Salida + " = ";
                Mi_SQL = Mi_SQL + " SALIDA_DETALLES." + Alm_Com_Salidas_Detalles.Campo_No_Salida + " ";
                Mi_SQL = Mi_SQL + " AND PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID + " = '" + Datos.P_Producto_ID + "'";
                if (!String.IsNullOrEmpty(Datos.P_Estatus_Salida))
                {
                    Mi_SQL = Mi_SQL + " AND SALIDAS." + Alm_Com_Salidas.Campo_Estatus + " = '" + Datos.P_Estatus_Salida + "'";
                }
               // Mi_SQL = Mi_SQL + " AND SALIDAS." + Alm_Com_Salidas.Campo_Producto_ID + " = '" +  + "'";
                if (Datos.P_Tomar_Fecha_Inicio) {
                    Mi_SQL = Mi_SQL + " AND SALIDAS." + Alm_Com_Salidas.Campo_Fecha_Creo + " >= '" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Inicio) + "'";
                }
                if (Datos.P_Tomar_Fecha_Fin)
                {
                    Mi_SQL = Mi_SQL + " AND SALIDAS." + Alm_Com_Salidas.Campo_Fecha_Creo + " <= '" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Fin.AddDays(1)) + "'";
                }
                Mi_SQL = Mi_SQL + " order by SALIDAS." + Alm_Com_Salidas.Campo_Fecha_Creo;

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
        /// NOMBRE DE LA CLASE:     Consultar_Comprometidos_Productos
        /// DESCRIPCION:            Consulta los comprometidos de Un producto.
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene la información para realizar la consulta
        ///                         
        /// CREO       :            Francisco Antonio Gallardo Castañeda
        /// FECHA_CREO :            01/Octubre/2011  
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consultar_Comprometidos_Productos(Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Datos) {
            String Mi_SQL = String.Empty; //Variable para las consultas

            try {
                // Asignar consulta
                Mi_SQL = "SELECT " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Folio + " AS FOLIO";
                Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Creo + " AS FECHA";
                Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Cantidad + " AS CANTIDAD";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ", " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Requisicion_ID;
                Mi_SQL = Mi_SQL + " = " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Estatus;
                Mi_SQL = Mi_SQL + " IN ('AUTORIZADA', 'EN CONSTRUCCION', 'GENERADA', 'ALMACEN')";
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID;
                Mi_SQL = Mi_SQL + " = '" + Datos.P_Producto_ID.Trim() + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Tipo + " = 'PRODUCTO' ";
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Tipo + " = 'STOCK'";
                if (Datos.P_Tomar_Fecha_Inicio) {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Creo + " >= '" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Inicio) + "'";
                }
                if (Datos.P_Tomar_Fecha_Fin) {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Creo + " < '" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Fin.AddDays(1)) + "'";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Folio;

                //Entregar resultado
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            } catch (OracleException ex) {
                throw new Exception("Error: " + ex.Message);
            } catch (DBConcurrencyException ex) {
                throw new Exception("Error: " + ex.Message);
            } catch (Exception ex) {
                throw new Exception("Error: " + ex.Message);
            } finally {
            }
        }
 
        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consultar_Entradas_Ajuste_Productos
        /// DESCRIPCION:            Consulta los ajustes de entrada de Un producto.
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene la información para realizar la consulta
        ///                         
        /// CREO       :            Francisco Antonio Gallardo Castañeda
        /// FECHA_CREO :            01/Octubre/2011  
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consultar_Entradas_Ajuste_Productos(Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Datos) {
            String Mi_SQL = String.Empty; //Variable para las consultas

            try {
                // Asignar consulta
                Mi_SQL = "SELECT " + Ope_Alm_Ajustes_Inv_Stock.Tabla_Ope_Alm_Ajustes_Inv_Stock + "." + Ope_Alm_Ajustes_Inv_Stock.Campo_No_Ajuste + " AS NO_AJUSTE";
                Mi_SQL = Mi_SQL + ", " + Ope_Alm_Ajustes_Inv_Stock.Tabla_Ope_Alm_Ajustes_Inv_Stock + "." + Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Autorizo + " AS FECHA";
                Mi_SQL = Mi_SQL + ", " + Ope_Alm_Ajustes_Detalles.Tabla_Ope_Alm_Ajustes_Almacen + "." + Ope_Alm_Ajustes_Detalles.Campo_Diferencia + " AS CANTIDAD";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Alm_Ajustes_Inv_Stock.Tabla_Ope_Alm_Ajustes_Inv_Stock + ", " + Ope_Alm_Ajustes_Detalles.Tabla_Ope_Alm_Ajustes_Almacen + "";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Alm_Ajustes_Inv_Stock.Tabla_Ope_Alm_Ajustes_Inv_Stock + "." + Ope_Alm_Ajustes_Inv_Stock.Campo_No_Ajuste + "";
                Mi_SQL = Mi_SQL + " = " + Ope_Alm_Ajustes_Detalles.Tabla_Ope_Alm_Ajustes_Almacen + "." + Ope_Alm_Ajustes_Detalles.Campo_No_Ajuste + "";
                Mi_SQL = Mi_SQL + " AND " + Ope_Alm_Ajustes_Inv_Stock.Tabla_Ope_Alm_Ajustes_Inv_Stock + "." + Ope_Alm_Ajustes_Inv_Stock.Campo_Estatus + " = 'AUTORIZADO'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Alm_Ajustes_Detalles.Tabla_Ope_Alm_Ajustes_Almacen + "." + Ope_Alm_Ajustes_Detalles.Campo_Tipo_Movimiento + " = 'ENTRADA'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Alm_Ajustes_Detalles.Tabla_Ope_Alm_Ajustes_Almacen + "." + Ope_Alm_Ajustes_Detalles.Campo_Producto_ID + " = '" + Datos.P_Producto_ID + "'";
                 if (Datos.P_Tomar_Fecha_Inicio) {
                     Mi_SQL = Mi_SQL + " AND " + Ope_Alm_Ajustes_Inv_Stock.Tabla_Ope_Alm_Ajustes_Inv_Stock + "." + Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Autorizo + " >= '" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Inicio) + "'";
                }
                if (Datos.P_Tomar_Fecha_Fin) {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Alm_Ajustes_Inv_Stock.Tabla_Ope_Alm_Ajustes_Inv_Stock + "." + Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Autorizo + " < '" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Fin.AddDays(1)) + "'";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Alm_Ajustes_Inv_Stock.Tabla_Ope_Alm_Ajustes_Inv_Stock + "." + Ope_Alm_Ajustes_Inv_Stock.Campo_No_Ajuste;
                //Entregar resultado
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            } catch (OracleException ex) {
                throw new Exception("Error: " + ex.Message);
            } catch (DBConcurrencyException ex) {
                throw new Exception("Error: " + ex.Message);
            } catch (Exception ex) {
                throw new Exception("Error: " + ex.Message);
            } finally {
            }
        }
                
        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consultar_Salidas_Ajuste_Productos
        /// DESCRIPCION:            Consulta los ajustes de salida de Un producto.
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene la información para realizar la consulta
        ///                         
        /// CREO       :            Francisco Antonio Gallardo Castañeda
        /// FECHA_CREO :            01/Octubre/2011  
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consultar_Salidas_Ajuste_Productos(Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Datos) {
            String Mi_SQL = String.Empty; //Variable para las consultas

            try {
                // Asignar consulta
                Mi_SQL = "SELECT " + Ope_Alm_Ajustes_Inv_Stock.Tabla_Ope_Alm_Ajustes_Inv_Stock + "." + Ope_Alm_Ajustes_Inv_Stock.Campo_No_Ajuste + " AS NO_AJUSTE";
                Mi_SQL = Mi_SQL + ", " + Ope_Alm_Ajustes_Inv_Stock.Tabla_Ope_Alm_Ajustes_Inv_Stock + "." + Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Autorizo + " AS FECHA";
                Mi_SQL = Mi_SQL + ", " + Ope_Alm_Ajustes_Detalles.Tabla_Ope_Alm_Ajustes_Almacen + "." + Ope_Alm_Ajustes_Detalles.Campo_Diferencia + " AS CANTIDAD";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Alm_Ajustes_Inv_Stock.Tabla_Ope_Alm_Ajustes_Inv_Stock + ", " + Ope_Alm_Ajustes_Detalles.Tabla_Ope_Alm_Ajustes_Almacen + "";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Alm_Ajustes_Inv_Stock.Tabla_Ope_Alm_Ajustes_Inv_Stock + "." + Ope_Alm_Ajustes_Inv_Stock.Campo_No_Ajuste + "";
                Mi_SQL = Mi_SQL + " = " + Ope_Alm_Ajustes_Detalles.Tabla_Ope_Alm_Ajustes_Almacen + "." + Ope_Alm_Ajustes_Detalles.Campo_No_Ajuste + "";
                Mi_SQL = Mi_SQL + " AND " + Ope_Alm_Ajustes_Inv_Stock.Tabla_Ope_Alm_Ajustes_Inv_Stock + "." + Ope_Alm_Ajustes_Inv_Stock.Campo_Estatus + " = 'AUTORIZADO'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Alm_Ajustes_Detalles.Tabla_Ope_Alm_Ajustes_Almacen + "." + Ope_Alm_Ajustes_Detalles.Campo_Tipo_Movimiento + " = 'SALIDA'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Alm_Ajustes_Detalles.Tabla_Ope_Alm_Ajustes_Almacen + "." + Ope_Alm_Ajustes_Detalles.Campo_Producto_ID + " = '" + Datos.P_Producto_ID + "'";
                 if (Datos.P_Tomar_Fecha_Inicio) {
                     Mi_SQL = Mi_SQL + " AND " + Ope_Alm_Ajustes_Inv_Stock.Tabla_Ope_Alm_Ajustes_Inv_Stock + "." + Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Autorizo + " >= '" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Inicio) + "'";
                }
                if (Datos.P_Tomar_Fecha_Fin) {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Alm_Ajustes_Inv_Stock.Tabla_Ope_Alm_Ajustes_Inv_Stock + "." + Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Autorizo + " < '" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Fin.AddDays(1)) + "'";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Alm_Ajustes_Inv_Stock.Tabla_Ope_Alm_Ajustes_Inv_Stock + "." + Ope_Alm_Ajustes_Inv_Stock.Campo_No_Ajuste;
                //Entregar resultado
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            } catch (OracleException ex) {
                throw new Exception("Error: " + ex.Message);
            } catch (DBConcurrencyException ex) {
                throw new Exception("Error: " + ex.Message);
            } catch (Exception ex) {
                throw new Exception("Error: " + ex.Message);
            } finally {
            }
        }

        public static DataTable Consultar_Kardex_Actualizado(Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Datos)
        {
            DataTable Dt_Kardex = null;
            try
            {
                String Mi_SQL = "";
                Mi_SQL = "SELECT CAT_COM_PRODUCTOS.PRODUCTO_ID, CAT_COM_PRODUCTOS.CLAVE,CAT_COM_PRODUCTOS.NOMBRE,CAT_COM_PRODUCTOS.DESCRIPCION," +
                " INICIAL, " +
                " 0 AS AJUSTE_ENTRADA, " +
                " 0 AS ENTRADA ," +
                " 0 AS AJUSTE_SALIDA, " +
                " 0 AS SALIDA, " +
                " 0 AS COMPROMETIDO, " +
                " 0 AS EXISTENCIA, " +
                " 0 AS DISPONIBLE " +

                "FROM CAT_COM_PRODUCTOS " +
                "WHERE  CAT_COM_PRODUCTOS.STOCK='SI' " +
                "AND CAT_COM_PRODUCTOS.STOCK = 'SI' " +
                "AND CAT_COM_PRODUCTOS.ESTATUS = 'ACTIVO' ";
                if (!String.IsNullOrEmpty(Datos.P_Partida_ID))
                {
                    Mi_SQL += " AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." +
                        Cat_Com_Productos.Campo_Partida_Especifica_ID +
                        " = '" + Datos.P_Partida_ID + "' ";
                }

                if (!String.IsNullOrEmpty(Datos.P_Clave))
                {
                    Mi_SQL += "AND CAT_COM_PRODUCTOS.CLAVE = '" + Datos.P_Clave + "' ";
                }
                Mi_SQL += " ORDER BY CAT_COM_PRODUCTOS.NOMBRE";
                Dt_Kardex = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Dt_Kardex;

        }

        public static DataTable Consultar_Kardex(Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Datos)
        {
            DataTable Dt_Kardex = null;
            try
            {
                String Mi_SQL = "";
                Mi_SQL = "SELECT CAT_COM_PRODUCTOS.PRODUCTO_ID, CAT_COM_PRODUCTOS.CLAVE,CAT_COM_PRODUCTOS.NOMBRE,CAT_COM_PRODUCTOS.DESCRIPCION," +
                "(" +
                "NVL(CAT_COM_PRODUCTOS.INICIAL,0)  + " +
                "NVL((SELECT SUM(DIFERENCIA) FROM OPE_ALM_AJUSTES_DETALLES WHERE TIPO_MOVIMIENTO='ENTRADA' " +
                "AND PRODUCTO_ID=CAT_COM_PRODUCTOS.PRODUCTO_ID " +
                "AND NO_AJUSTE IN (SELECT NO_AJUSTE FROM OPE_ALM_AJUSTES_INV_STOCK " +
                "WHERE NO_AJUSTE=OPE_ALM_AJUSTES_DETALLES.NO_AJUSTE AND ESTATUS='AUTORIZADO' " +
                "AND FECHA_CREO < " +
                "'" + Datos.P_Fecha_I + "' " +
                ")) ,0) + " +

                "NVL((SELECT SUM(CANTIDAD)  FROM OPE_COM_REQ_PRODUCTO WHERE PROD_SERV_ID = CAT_COM_PRODUCTOS.PRODUCTO_ID " +
                "AND NO_REQUISICION IN" +
                "(SELECT NO_REQUISICION FROM OPE_COM_REQUISICIONES WHERE NO_REQUISICION=OPE_COM_REQ_PRODUCTO.NO_REQUISICION " +
                "AND LISTADO_ALMACEN='SI' AND ESTATUS='SURTIDA' " +
                "AND FECHA_SURTIDO < " +
                "'" + Datos.P_Fecha_I + "' " +
                ")),0) - " +

                "NVL((SELECT SUM(DIFERENCIA) FROM OPE_ALM_AJUSTES_DETALLES WHERE " +
                "TIPO_MOVIMIENTO='SALIDA' AND PRODUCTO_ID=CAT_COM_PRODUCTOS.PRODUCTO_ID " +
                "AND NO_AJUSTE IN (SELECT NO_AJUSTE FROM OPE_ALM_AJUSTES_INV_STOCK WHERE NO_AJUSTE=OPE_ALM_AJUSTES_DETALLES.NO_AJUSTE " +
                "AND ESTATUS='AUTORIZADO' " +
                "AND FECHA_CREO < " +
                "'" + Datos.P_Fecha_I + "' " +
                ") ),0) - " +

                "NVL((SELECT SUM(CANTIDAD) FROM ALM_COM_SALIDAS_DETALLES WHERE PRODUCTO_ID = CAT_COM_PRODUCTOS.PRODUCTO_ID AND NO_SALIDA IN " +
                "(SELECT NO_SALIDA FROM ALM_COM_SALIDAS WHERE NO_SALIDA = ALM_COM_SALIDAS_DETALLES.NO_SALIDA AND ESTATUS='GENERADA' " +
                "AND FECHA_CREO < " +
                "'" + Datos.P_Fecha_I + "') " +
                " ),0)" +
                ") AS INICIAL, " +

                "NVL((SELECT SUM(DIFERENCIA) FROM OPE_ALM_AJUSTES_DETALLES WHERE TIPO_MOVIMIENTO='ENTRADA' " +
                "AND PRODUCTO_ID=CAT_COM_PRODUCTOS.PRODUCTO_ID " +
                "AND NO_AJUSTE IN (SELECT NO_AJUSTE FROM OPE_ALM_AJUSTES_INV_STOCK " +
                "WHERE NO_AJUSTE=OPE_ALM_AJUSTES_DETALLES.NO_AJUSTE AND ESTATUS='AUTORIZADO' " +
                "AND TO_DATE(TO_CHAR(FECHA_CREO,'DD/MM/YY')) BETWEEN " +
                "'" + Datos.P_Fecha_I + "' " +
                " AND " +
                "'" + Datos.P_Fecha_F + "' " +
                ")),0) AS AJUSTE_ENTRADA, " +


                "NVL((SELECT SUM(CANTIDAD)  FROM OPE_COM_REQ_PRODUCTO WHERE PROD_SERV_ID = CAT_COM_PRODUCTOS.PRODUCTO_ID " +
                "AND NO_REQUISICION IN " +
                "(SELECT NO_REQUISICION FROM OPE_COM_REQUISICIONES WHERE NO_REQUISICION=OPE_COM_REQ_PRODUCTO.NO_REQUISICION " +
                "AND LISTADO_ALMACEN='SI' AND ESTATUS='SURTIDA' " +
                "AND TO_DATE(TO_CHAR(FECHA_SURTIDO,'DD/MM/YY')) BETWEEN " +
                "'" + Datos.P_Fecha_I + "' " +
                " AND " +
                "'" + Datos.P_Fecha_F + "' " +
                ")),0)AS ENTRADA ," +

                "NVL((SELECT SUM(DIFERENCIA) FROM OPE_ALM_AJUSTES_DETALLES WHERE " +
                "TIPO_MOVIMIENTO='SALIDA' AND PRODUCTO_ID=CAT_COM_PRODUCTOS.PRODUCTO_ID " +
                "AND NO_AJUSTE IN (SELECT NO_AJUSTE FROM OPE_ALM_AJUSTES_INV_STOCK WHERE NO_AJUSTE=OPE_ALM_AJUSTES_DETALLES.NO_AJUSTE " +
                "AND ESTATUS='AUTORIZADO' " +
                "AND TO_DATE(TO_CHAR(FECHA_CREO,'DD/MM/YY')) BETWEEN " +
                "'" + Datos.P_Fecha_I + "' " +
                " AND " +
                "'" + Datos.P_Fecha_F + "' " +
                ") ),0) AS AJUSTE_SALIDA, " +

                "NVL((SELECT SUM(CANTIDAD) FROM ALM_COM_SALIDAS_DETALLES WHERE PRODUCTO_ID = CAT_COM_PRODUCTOS.PRODUCTO_ID " +
                "AND NO_SALIDA IN " +
                "(SELECT NO_SALIDA FROM ALM_COM_SALIDAS WHERE NO_SALIDA = ALM_COM_SALIDAS_DETALLES.NO_SALIDA AND ESTATUS='GENERADA' " +
                "AND TO_DATE(TO_CHAR(FECHA_CREO,'DD/MM/YY')) BETWEEN " +
                "'" + Datos.P_Fecha_I + "' " +
                " AND " +
                "'" + Datos.P_Fecha_F + "' " +
                ")),0) as SALIDA, " +

                //******************************************************************** 
                    //"(NVL((SELECT SUM( OPE_COM_REQ_PRODUCTO.CANTIDAD) " +
                    //"FROM OPE_COM_REQUISICIONES JOIN OPE_COM_REQ_PRODUCTO " +
                    //"ON OPE_COM_REQUISICIONES.NO_REQUISICION = OPE_COM_REQ_PRODUCTO.NO_REQUISICION " +
                    //"WHERE OPE_COM_REQUISICIONES.ESTATUS NOT IN ('CANCELADA','LIBERADA','CERRADA','PARCIAL','COMPLETA') " +
                    //"AND OPE_COM_REQUISICIONES.TIPO_ARTICULO = 'PRODUCTO' AND OPE_COM_REQUISICIONES.TIPO = 'STOCK' " +
                    //"AND TO_DATE(TO_CHAR(OPE_COM_REQUISICIONES.FECHA_CREO,'DD/MM/YY')) BETWEEN " +
                    //"'" + Datos.P_Fecha_I + "' " +
                    //" AND " +
                    //"'" + Datos.P_Fecha_F + "' " +
                    //"AND OPE_COM_REQ_PRODUCTO.PROD_SERV_ID = CAT_COM_PRODUCTOS.PRODUCTO_ID),0)) AS COMPROMETIDO " +                          
                    //********************************************************************


                "(" +
                "NVL(( " +
                "SELECT SUM( NVL(OPE_COM_REQ_PRODUCTO.CANTIDAD,0)  ) AS AX " +
                "FROM OPE_COM_REQUISICIONES JOIN OPE_COM_REQ_PRODUCTO " +
                "ON OPE_COM_REQUISICIONES.NO_REQUISICION = OPE_COM_REQ_PRODUCTO.NO_REQUISICION " +
                "WHERE OPE_COM_REQUISICIONES.ESTATUS NOT IN ('CANCELADA','LIBERADA','CERRADA','COMPLETA','PARCIAL') " +
                "AND OPE_COM_REQUISICIONES.TIPO_ARTICULO = 'PRODUCTO' AND OPE_COM_REQUISICIONES.TIPO = 'STOCK' " +
                "AND TO_DATE(TO_CHAR(OPE_COM_REQUISICIONES.FECHA_CREO,'DD/MM/YY')) BETWEEN " +
                "'" + Datos.P_Fecha_I + "' " +
                " AND " +
                "'" + Datos.P_Fecha_F + "' " +
                "AND OPE_COM_REQ_PRODUCTO.PROD_SERV_ID = CAT_COM_PRODUCTOS.PRODUCTO_ID " +

                "),0) " +
                " + " +
                "(NVL(( SELECT SUM( NVL(OPE_COM_REQ_PRODUCTO.CANTIDAD,0) - NVL(OPE_COM_REQ_PRODUCTO.CANTIDAD_ENTREGADA,0) ) AS AX " +
                "FROM OPE_COM_REQUISICIONES JOIN OPE_COM_REQ_PRODUCTO " +
                "ON OPE_COM_REQUISICIONES.NO_REQUISICION = OPE_COM_REQ_PRODUCTO.NO_REQUISICION " +
                "WHERE OPE_COM_REQUISICIONES.ESTATUS IN ('PARCIAL') " +
                "AND OPE_COM_REQUISICIONES.TIPO_ARTICULO = 'PRODUCTO' AND OPE_COM_REQUISICIONES.TIPO = 'STOCK' " +
                "AND TO_DATE(TO_CHAR(OPE_COM_REQUISICIONES.FECHA_CREO,'DD/MM/YY')) BETWEEN " +
                "'" + Datos.P_Fecha_I + "' " +
                " AND " +
                "'" + Datos.P_Fecha_F + "' " +
                "AND OPE_COM_REQ_PRODUCTO.PROD_SERV_ID = CAT_COM_PRODUCTOS.PRODUCTO_ID " +
                "AND OPE_COM_REQ_PRODUCTO.CANTIDAD - NVL(OPE_COM_REQ_PRODUCTO.CANTIDAD_ENTREGADA,0) <> 0),0)) " +
                ") AS COMPROMETIDO " +



                "FROM CAT_COM_PRODUCTOS " +
                "WHERE  CAT_COM_PRODUCTOS.STOCK='SI' " +
                "AND CAT_COM_PRODUCTOS.STOCK = 'SI' " +
                "AND CAT_COM_PRODUCTOS.ESTATUS = 'ACTIVO' ";
                if (!String.IsNullOrEmpty(Datos.P_Partida_ID))
                {
                    Mi_SQL += " AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." +
                        Cat_Com_Productos.Campo_Partida_Especifica_ID +
                        " = '" + Datos.P_Partida_ID + "' ";
                }
                
                if (!String.IsNullOrEmpty(Datos.P_Clave))
                {
                    Mi_SQL += "AND CAT_COM_PRODUCTOS.CLAVE = '" + Datos.P_Clave + "' ";
                }
                Mi_SQL += " ORDER BY NOMBRE";
                Dt_Kardex = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());                
            }
            return Dt_Kardex;
        }

        public static DataTable Consultar_Entradas(Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Datos)
        {
            DataTable Dt_Tabla = null;
            try
            {
                String Mi_SQL = "";
                Mi_SQL = "SELECT CAT_COM_PRODUCTOS.PRODUCTO_ID, OPE_COM_REQUISICIONES.FECHA_SURTIDO AS FECHA, OPE_COM_REQ_PRODUCTO.CANTIDAD, OPE_COM_REQUISICIONES.NO_ORDEN_COMPRA AS NO_OPERACION " +
                "FROM CAT_COM_PRODUCTOS JOIN OPE_COM_REQ_PRODUCTO ON CAT_COM_PRODUCTOS.PRODUCTO_ID = OPE_COM_REQ_PRODUCTO.PROD_SERV_ID JOIN " +
                "OPE_COM_REQUISICIONES ON OPE_COM_REQ_PRODUCTO.NO_REQUISICION = OPE_COM_REQUISICIONES.NO_REQUISICION " +
                "WHERE PROD_SERV_ID = CAT_COM_PRODUCTOS.PRODUCTO_ID " +
                "AND CAT_COM_PRODUCTOS.STOCK = 'SI' " +
                "AND CAT_COM_PRODUCTOS.ESTATUS = 'ACTIVO' ";
                if (!String.IsNullOrEmpty(Datos.P_Partida_ID))
                {
                    Mi_SQL += " AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." +
                        Cat_Com_Productos.Campo_Partida_Especifica_ID +
                        " = '" + Datos.P_Partida_ID + "' ";
                }
                if (!String.IsNullOrEmpty(Datos.P_Clave)) 
                {
                    Mi_SQL += " AND CAT_COM_PRODUCTOS.CLAVE = '" + Datos.P_Clave + "' ";
                }
                Mi_SQL += " AND OPE_COM_REQ_PRODUCTO.NO_REQUISICION IN " +
                "(SELECT NO_REQUISICION FROM OPE_COM_REQUISICIONES WHERE NO_REQUISICION=OPE_COM_REQ_PRODUCTO.NO_REQUISICION " +
                "AND LISTADO_ALMACEN='SI' AND ESTATUS='SURTIDA' " +
                "AND TO_DATE(TO_CHAR(FECHA_SURTIDO,'DD/MM/YY')) BETWEEN " +
                "'" + Datos.P_Fecha_I + "' " +
                " AND " +
                "'" + Datos.P_Fecha_F + "')";
                Mi_SQL += " ORDER BY OPE_COM_REQUISICIONES.FECHA_SURTIDO ASC";          
                Dt_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());                
            }
            return Dt_Tabla;        
        }
        //frm_ope_alm_alta_bienes_muebles.aspx
        public static DataTable Consultar_Entradas_Ajuste(Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Datos)
        {
            DataTable Dt_Tabla = null;
            try
            {
                String Mi_SQL = "";
                Mi_SQL = "SELECT CAT_COM_PRODUCTOS.PRODUCTO_ID, OPE_ALM_AJUSTES_DETALLES.DIFERENCIA AS CANTIDAD, OPE_ALM_AJUSTES_DETALLES.FECHA_CREO AS FECHA, OPE_ALM_AJUSTES_DETALLES.NO_AJUSTE AS NO_OPERACION" +
                " FROM CAT_COM_PRODUCTOS JOIN OPE_ALM_AJUSTES_DETALLES ON CAT_COM_PRODUCTOS.PRODUCTO_ID = OPE_ALM_AJUSTES_DETALLES.PRODUCTO_ID " +
                "WHERE OPE_ALM_AJUSTES_DETALLES.TIPO_MOVIMIENTO = 'ENTRADA' " +
                "AND CAT_COM_PRODUCTOS.stock = 'SI' " +
                "AND CAT_COM_PRODUCTOS.ESTATUS = 'ACTIVO' ";
                if (!String.IsNullOrEmpty(Datos.P_Partida_ID))
                {
                    Mi_SQL += " AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." +
                        Cat_Com_Productos.Campo_Partida_Especifica_ID +
                        " = '" + Datos.P_Partida_ID + "' ";
                }
                if (!String.IsNullOrEmpty(Datos.P_Clave))
                {
                    Mi_SQL += " AND CAT_COM_PRODUCTOS.CLAVE = '" + Datos.P_Clave + "' ";
                }
                Mi_SQL += " AND OPE_ALM_AJUSTES_DETALLES.NO_AJUSTE IN (SELECT NO_AJUSTE FROM OPE_ALM_AJUSTES_INV_STOCK " +
                "WHERE NO_AJUSTE = OPE_ALM_AJUSTES_DETALLES.NO_AJUSTE AND ESTATUS = 'AUTORIZADO' " +
                "AND TO_DATE(TO_CHAR(FECHA_CREO,'DD/MM/YY')) BETWEEN " +
                "'" + Datos.P_Fecha_I + "' " +
                " AND " +
                "'" + Datos.P_Fecha_F + "' " +
                ")";
                Mi_SQL += " ORDER BY OPE_ALM_AJUSTES_DETALLES.NO_AJUSTE ASC";
                Dt_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Dt_Tabla;  
        }

        public static DataTable Consultar_Salidas(Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Datos)
        {
            DataSet Ds_DataSet = null;
            DataTable Dt_Tabla = null;
            try
            {
                String Mi_SQL = "";
                Mi_SQL = "SELECT CAT_COM_PRODUCTOS.PRODUCTO_ID, ALM_COM_SALIDAS.FECHA_CREO AS FECHA, ALM_COM_SALIDAS_DETALLES.CANTIDAD, " +
                " ALM_COM_SALIDAS_DETALLES.NO_SALIDA AS NO_OPERACION FROM " +
                "CAT_COM_PRODUCTOS JOIN ALM_COM_SALIDAS_DETALLES ON " +
                "CAT_COM_PRODUCTOS.PRODUCTO_ID = ALM_COM_SALIDAS_DETALLES.PRODUCTO_ID " +
                "JOIN ALM_COM_SALIDAS ON ALM_COM_SALIDAS_DETALLES.NO_SALIDA = ALM_COM_SALIDAS.NO_SALIDA " +
                " WHERE CAT_COM_PRODUCTOS.STOCK = 'SI' " +
                "AND CAT_COM_PRODUCTOS.ESTATUS = 'ACTIVO' ";
                if (!String.IsNullOrEmpty(Datos.P_Partida_ID))
                {
                    Mi_SQL += " AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." +
                        Cat_Com_Productos.Campo_Partida_Especifica_ID +
                        " = '" + Datos.P_Partida_ID + "' ";
                }
                if (!String.IsNullOrEmpty(Datos.P_Clave))
                {
                    Mi_SQL += " AND CAT_COM_PRODUCTOS.CLAVE = '" + Datos.P_Clave + "' ";
                }
                Mi_SQL +=
                " AND ALM_COM_SALIDAS.ESTATUS='GENERADA' " +
                "AND TO_DATE(TO_CHAR(ALM_COM_SALIDAS.FECHA_CREO,'DD/MM/YY')) BETWEEN " +
                "'" + Datos.P_Fecha_I + "' " +
                " AND " +
                "'" + Datos.P_Fecha_F + "' ";
                Mi_SQL += " ORDER BY ALM_COM_SALIDAS.NO_SALIDA ASC";
                Ds_DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_DataSet.Tables.Count > 0)
                {
                    Dt_Tabla = Ds_DataSet.Tables[0];
                }
                else
                {
                    Dt_Tabla = null;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Dt_Tabla;
        }

        public static DataTable Consultar_Salidas_Ajuste(Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Datos)
        {
            DataTable Dt_Tabla = null;
            try
            {
                String Mi_SQL = "";
                Mi_SQL = "SELECT CAT_COM_PRODUCTOS.PRODUCTO_ID, OPE_ALM_AJUSTES_DETALLES.FECHA_CREO AS FECHA, OPE_ALM_AJUSTES_DETALLES.DIFERENCIA AS CANTIDAD , OPE_ALM_AJUSTES_DETALLES.NO_AJUSTE AS NO_OPERACION" +
                " FROM CAT_COM_PRODUCTOS JOIN OPE_ALM_AJUSTES_DETALLES ON CAT_COM_PRODUCTOS.PRODUCTO_ID = OPE_ALM_AJUSTES_DETALLES.PRODUCTO_ID " +
                "WHERE OPE_ALM_AJUSTES_DETALLES.TIPO_MOVIMIENTO = 'SALIDA' " +
                "AND CAT_COM_PRODUCTOS.stock = 'SI' " +
                "AND CAT_COM_PRODUCTOS.ESTATUS = 'ACTIVO' ";
                if (!String.IsNullOrEmpty(Datos.P_Partida_ID))
                {
                    Mi_SQL += " AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." +
                        Cat_Com_Productos.Campo_Partida_Especifica_ID +
                        " = '" + Datos.P_Partida_ID + "' ";
                }
                if (!String.IsNullOrEmpty(Datos.P_Clave))
                {
                    Mi_SQL += " AND CAT_COM_PRODUCTOS.CLAVE = '" + Datos.P_Clave + "' ";
                }
                Mi_SQL += " AND OPE_ALM_AJUSTES_DETALLES.NO_AJUSTE IN (SELECT NO_AJUSTE FROM OPE_ALM_AJUSTES_INV_STOCK " +
                "WHERE NO_AJUSTE = OPE_ALM_AJUSTES_DETALLES.NO_AJUSTE AND ESTATUS = 'AUTORIZADO' " +
                "AND TO_DATE(TO_CHAR(FECHA_CREO,'DD/MM/YY')) BETWEEN " +
                "'" + Datos.P_Fecha_I + "' " +
                " AND " +
                "'" + Datos.P_Fecha_F + "' " +
                ")";
                Mi_SQL += " ORDER BY OPE_ALM_AJUSTES_DETALLES.NO_AJUSTE ASC";
                Dt_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Dt_Tabla;
        }

        public static DataTable Consultar_Compromisos(Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Datos)
        {
            DataTable Dt_Tabla = null;
            try
            {
                String Mi_SQL = "";
                Mi_SQL = "SELECT CAT_COM_PRODUCTOS.PRODUCTO_ID, OPE_COM_REQUISICIONES.FECHA_CREO AS FECHA, OPE_COM_REQ_PRODUCTO.CANTIDAD, OPE_COM_REQ_PRODUCTO.NO_REQUISICION AS NO_OPERACION " +
                "FROM CAT_COM_PRODUCTOS JOIN OPE_COM_REQ_PRODUCTO ON CAT_COM_PRODUCTOS.PRODUCTO_ID = OPE_COM_REQ_PRODUCTO.PROD_SERV_ID JOIN " +
                "OPE_COM_REQUISICIONES " +
                "ON OPE_COM_REQ_PRODUCTO.NO_REQUISICION = OPE_COM_REQUISICIONES.NO_REQUISICION  " +
                "WHERE OPE_COM_REQUISICIONES.ESTATUS NOT IN ('CANCELADA','LIBERADA','CERRADA','PARCIAL','COMPLETA') " +
                "AND OPE_COM_REQUISICIONES.TIPO_ARTICULO = 'PRODUCTO' AND OPE_COM_REQUISICIONES.TIPO = 'STOCK' " +
                "AND CAT_COM_PRODUCTOS.ESTATUS = 'ACTIVO' ";
                if (!String.IsNullOrEmpty(Datos.P_Partida_ID))
                {
                    Mi_SQL += " AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." +
                        Cat_Com_Productos.Campo_Partida_Especifica_ID +
                        " = '" + Datos.P_Partida_ID + "' ";
                }
                if (!String.IsNullOrEmpty(Datos.P_Clave))
                {
                    Mi_SQL += " AND CAT_COM_PRODUCTOS.CLAVE = '" + Datos.P_Clave + "' ";
                }
                Mi_SQL = Mi_SQL + " AND TO_DATE(TO_CHAR(OPE_COM_REQUISICIONES.FECHA_CREO,'DD/MM/YY')) BETWEEN " +
                "'" + Datos.P_Fecha_I + "' " +
                " AND " +
                "'" + Datos.P_Fecha_F + "' " +
                "AND CAT_COM_PRODUCTOS.STOCK = 'SI' " +
                "AND OPE_COM_REQ_PRODUCTO.PROD_SERV_ID = CAT_COM_PRODUCTOS.PRODUCTO_ID";

                Mi_SQL = Mi_SQL + " UNION ALL ";
                Mi_SQL = Mi_SQL + "SELECT CAT_COM_PRODUCTOS.PRODUCTO_ID, OPE_COM_REQUISICIONES.FECHA_CREO AS FECHA, " +
                "(OPE_COM_REQ_PRODUCTO.CANTIDAD - OPE_COM_REQ_PRODUCTO.CANTIDAD_ENTREGADA) AS CANTIDAD, " +
                "OPE_COM_REQ_PRODUCTO.NO_REQUISICION AS NO_OPERACION " +
                "FROM CAT_COM_PRODUCTOS JOIN OPE_COM_REQ_PRODUCTO " +
                "ON CAT_COM_PRODUCTOS.PRODUCTO_ID = OPE_COM_REQ_PRODUCTO.PROD_SERV_ID JOIN " +
                "OPE_COM_REQUISICIONES " +
                "ON OPE_COM_REQ_PRODUCTO.NO_REQUISICION = OPE_COM_REQUISICIONES.NO_REQUISICION  " +
                "WHERE OPE_COM_REQUISICIONES.ESTATUS IN ('PARCIAL') AND (OPE_COM_REQ_PRODUCTO.CANTIDAD - NVL(OPE_COM_REQ_PRODUCTO.CANTIDAD_ENTREGADA,0)) <> 0 " +
                "AND OPE_COM_REQUISICIONES.TIPO_ARTICULO = 'PRODUCTO' AND OPE_COM_REQUISICIONES.TIPO = 'STOCK' " +
                "AND CAT_COM_PRODUCTOS.ESTATUS = 'ACTIVO' ";
                if (!String.IsNullOrEmpty(Datos.P_Partida_ID))
                {
                    Mi_SQL += " AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." +
                        Cat_Com_Productos.Campo_Partida_Especifica_ID +
                        " = '" + Datos.P_Partida_ID + "' ";
                }
                if (!String.IsNullOrEmpty(Datos.P_Clave))
                {
                    Mi_SQL += " AND CAT_COM_PRODUCTOS.CLAVE = '" + Datos.P_Clave + "' ";
                }
                Mi_SQL = Mi_SQL + "AND TO_DATE(TO_CHAR(OPE_COM_REQUISICIONES.FECHA_CREO,'DD/MM/YY')) BETWEEN " +
                "'" + Datos.P_Fecha_I + "' " +
                " AND " +
                "'" + Datos.P_Fecha_F + "' " +
                "AND CAT_COM_PRODUCTOS.STOCK = 'SI' " +
                "AND OPE_COM_REQ_PRODUCTO.PROD_SERV_ID = CAT_COM_PRODUCTOS.PRODUCTO_ID";
                //Mi_SQL += " ORDER BY OPE_COM_REQ_PRODUCTO.NO_REQUISICION ASC";
                Dt_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Dt_Tabla;
        }

        public static DataTable Consultar_Salidas_Unidad_Responsable(Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Datos)
        {
            DataSet Ds_DataSet = null;
            DataTable Dt_Tabla = null;
            try
            {
                String Mi_SQL = "";
                Mi_SQL = "SELECT CAT_COM_PRODUCTOS.PRODUCTO_ID, ALM_COM_SALIDAS.FECHA_CREO AS FECHA, ALM_COM_SALIDAS_DETALLES.CANTIDAD, " +
                " ALM_COM_SALIDAS_DETALLES.NO_SALIDA AS NO_OPERACION,ALM_COM_SALIDAS.DEPENDENCIA_ID, " +

                "(SELECT NOMBRE FROM CAT_DEPENDENCIAS WHERE DEPENDENCIA_ID = ALM_COM_SALIDAS.DEPENDENCIA_ID) AS UR " +

                " FROM " +
                "CAT_COM_PRODUCTOS JOIN ALM_COM_SALIDAS_DETALLES ON " +
                "CAT_COM_PRODUCTOS.PRODUCTO_ID = ALM_COM_SALIDAS_DETALLES.PRODUCTO_ID " +
                "JOIN ALM_COM_SALIDAS ON ALM_COM_SALIDAS_DETALLES.NO_SALIDA = ALM_COM_SALIDAS.NO_SALIDA " +
                " WHERE CAT_COM_PRODUCTOS.STOCK = 'SI' " +
                "AND CAT_COM_PRODUCTOS.ESTATUS = 'ACTIVO' ";
                if (!String.IsNullOrEmpty(Datos.P_Partida_ID))
                {
                    Mi_SQL += " AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." +
                        Cat_Com_Productos.Campo_Partida_Especifica_ID +
                        " = '" + Datos.P_Partida_ID + "' ";
                }
                if (!String.IsNullOrEmpty(Datos.P_Clave))
                {
                    Mi_SQL += " AND CAT_COM_PRODUCTOS.CLAVE = '" + Datos.P_Clave + "' ";
                }
                if (!String.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    Mi_SQL += " AND ALM_COM_SALIDAS.DEPENDENCIA_ID = '" + Datos.P_Dependencia_ID + "' ";
                }
                Mi_SQL +=
                " AND ALM_COM_SALIDAS.ESTATUS='GENERADA' " +
                "AND TO_DATE(TO_CHAR(ALM_COM_SALIDAS.FECHA_CREO,'DD/MM/YY')) BETWEEN " +
                "'" + Datos.P_Fecha_I + "' " +
                " AND " +
                "'" + Datos.P_Fecha_F + "' ";
                Mi_SQL += " ORDER BY ALM_COM_SALIDAS.NO_SALIDA ASC";
                Ds_DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_DataSet.Tables.Count > 0)
                {
                    Dt_Tabla = Ds_DataSet.Tables[0];
                }
                else
                {
                    Dt_Tabla = null;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Dt_Tabla;
        }


    }

}