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
using Presidencia.Sessiones;
using Presidencia.Kardex_Productos.Negocio;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Bitacora_Eventos;





/// <summary>
/// Summary description for Cls_Ope_Alm_Kardex_Productos_Datos
/// </summary>

namespace Presidencia.Kardex_Productos.Datos
{
    public class Cls_Ope_Alm_Kardex_Productos_Datos
    {
        public Cls_Ope_Alm_Kardex_Productos_Datos()
        {
            //
            // TODO: Add constructor logic here
            //
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
        public static DataTable Consultar_Entradas_Productos(Cls_Ope_Alm_Kardex_Productos_Negocio Datos)
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
                Mi_SQL = Mi_SQL + " ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + ") AS ORDEN_COMPRA ";

                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO, ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES, ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";

                Mi_SQL = Mi_SQL + " WHERE REQ_PRODUCTO." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = ";
                Mi_SQL = Mi_SQL + " REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " ";
                Mi_SQL = Mi_SQL + " and PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID + " = ";
                Mi_SQL = Mi_SQL + " REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " ";
                Mi_SQL = Mi_SQL + " AND REQUISICIONES." + Ope_Com_Requisiciones.Campo_Estatus + " = 'SURTIDA'";
                Mi_SQL = Mi_SQL + " AND REQUISICIONES." + Ope_Com_Requisiciones.Campo_Listado_Almacen + "='SI'";

                if (Datos.P_Clave_B != null)
                {
                    Mi_SQL = Mi_SQL + " AND PRODUCTOS." + Cat_Com_Productos.Campo_Clave + " = '" + Datos.P_Clave_B + "'";
                }

                if ((Datos.P_Fecha_Inicio_B != null) && (Datos.P_Fecha_Fin_B != null))
                {
                    Mi_SQL = Mi_SQL + " AND TO_DATE(TO_CHAR( REQUISICIONES." + Ope_Com_Requisiciones.Campo_Fecha_Surtido + ",'DD/MM/YY')) BETWEEN '" + Datos.P_Fecha_Inicio_B + "'" +
                  " AND '" + Datos.P_Fecha_Fin_B + "'";
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
        public static DataTable Consultar_Salidas_Productos(Cls_Ope_Alm_Kardex_Productos_Negocio Datos)
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

                if (Datos.P_Clave_B != null)
                {
                    Mi_SQL = Mi_SQL + " AND PRODUCTOS." + Cat_Com_Productos.Campo_Clave + " = '" + Datos.P_Clave_B + "'";
                }

                if ((Datos.P_Fecha_Inicio_B != null) && (Datos.P_Fecha_Fin_B != null))
                {
                    Mi_SQL = Mi_SQL + " AND TO_DATE(TO_CHAR( SALIDAS." + Alm_Com_Salidas.Campo_Fecha_Creo + ",'DD/MM/YY')) BETWEEN '" + Datos.P_Fecha_Inicio_B + "'" +
                  " AND '" + Datos.P_Fecha_Fin_B + "'";
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
        /// NOMBRE DE LA CLASE:     Consultar_Datos_Producto
        /// DESCRIPCION:            Método utilizado para consultar los detalle del producto
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene la información para realizar la consulta
        ///                         
        /// CREO       :            Salvador Hernandez Ramirez
        /// FECHA_CREO :            13/Agosto/2011  
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consultar_Datos_Producto(Cls_Ope_Alm_Kardex_Productos_Negocio Datos)
        {
            String Mi_SQL = String.Empty; //Variable para las consultas

            try
            {
                // Asignar consulta
                Mi_SQL = "SELECT DISTINCT " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre + " as PRODUCTO, ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Descripcion + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Clave+ ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Comprometido + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Comprometido + " as REGISTRO_ID, ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Comprometido + " as USUARIO_ELABORO, ";

                Mi_SQL = Mi_SQL + "(select UNIDADES." + Cat_Com_Unidades.Campo_Nombre + " FROM ";
                Mi_SQL = Mi_SQL + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " UNIDADES ";
                Mi_SQL = Mi_SQL + " WHERE UNIDADES." + Cat_Com_Unidades.Campo_Unidad_ID+ " = ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Unidad_ID + ") AS UNIDAD, ";

                Mi_SQL = Mi_SQL + "(select MODELOS." + Cat_Com_Modelos.Campo_Nombre + " FROM ";
                Mi_SQL = Mi_SQL + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + " MODELOS ";
                Mi_SQL = Mi_SQL + " WHERE MODELOS." + Cat_Com_Modelos.Campo_Modelo_ID+ " = ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Modelo_ID + ") AS MODELO, ";

                Mi_SQL = Mi_SQL + "(select MARCAS." + Cat_Com_Marcas.Campo_Nombre + " FROM ";
                Mi_SQL = Mi_SQL + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " MARCAS ";
                Mi_SQL = Mi_SQL + " WHERE MARCAS." + Cat_Com_Marcas.Campo_Marca_ID + " = ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Marca_ID + ") AS MARCA ";

                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " ";

                Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Clave + " = ";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Clave_B +"'";

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
