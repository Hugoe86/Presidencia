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
using System.Text;
using System.Collections.Generic;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Compras_Reporte_Proveedores_Excel.Negocio;

namespace Presidencia.Compras_Reporte_Proveedores_Excel.Datos
{
    public class Cls_Rpt_Com_Proveedores_Excel_Datos
    {
        #region Metodos
        /// ********************************************************************************************************************
        /// NOMBRE:         Consultar_Datos_Proveedor
        /// COMENTARIOS:    Consulta el movimiento presupuestal que se haya llevado en la tabla 
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ:     19/Enero/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Datos_Proveedor(Cls_Rpt_Com_Proveedores_Excel_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Movimiento = new DataTable();
            try
            {
                Mi_SQL.Append("Select " + Cat_Com_Det_Part_Prov.Campo_Proveedor_ID + "," +
                                Cat_Com_Det_Part_Prov.Campo_Partida_Generica_ID + ",");

                Mi_SQL.Append("(Select " + Cat_SAP_Partida_Generica.Campo_Descripcion + " from " +
                                Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + " where " +
                                Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID + "=" +
                                Cat_Com_Det_Part_Prov.Tabla_Cat_Com_Det_Part_Prov + "." +
                                Cat_Com_Det_Part_Prov.Campo_Partida_Generica_ID + ") Nombre_Partida,");
                
                Mi_SQL.Append("(Select " + Cat_SAP_Partida_Generica.Campo_Clave + " from " +
                                Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + " where " +
                                Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID + "=" +
                                Cat_Com_Det_Part_Prov.Tabla_Cat_Com_Det_Part_Prov + "." +
                                Cat_Com_Det_Part_Prov.Campo_Partida_Generica_ID + ") Clave_Partida,");

                Mi_SQL.Append("(Select " + Cat_Com_Proveedores.Campo_Fecha_Creo + " From " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores +
                                " Where " + Cat_Com_Proveedores.Campo_Proveedor_ID + "=" + Cat_Com_Det_Part_Prov.Tabla_Cat_Com_Det_Part_Prov +
                                "." + Cat_Com_Det_Part_Prov.Campo_Proveedor_ID + ") Fecha_Registro,");

                Mi_SQL.Append("(Select " + Cat_Com_Proveedores.Campo_Nombre+ " From " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores +
                                " Where " + Cat_Com_Proveedores.Campo_Proveedor_ID + "=" + Cat_Com_Det_Part_Prov.Tabla_Cat_Com_Det_Part_Prov +
                                "." + Cat_Com_Det_Part_Prov.Campo_Proveedor_ID + ") Nombre_Proveedor,");

                Mi_SQL.Append("(Select " + Cat_Com_Proveedores.Campo_Compañia + " From " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores +
                               " Where " + Cat_Com_Proveedores.Campo_Proveedor_ID + "=" + Cat_Com_Det_Part_Prov.Tabla_Cat_Com_Det_Part_Prov +
                               "." + Cat_Com_Det_Part_Prov.Campo_Proveedor_ID + ") Compañia,");

                Mi_SQL.Append("(Select " + Cat_Com_Proveedores.Campo_Representante_Legal + " From " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores +
                               " Where " + Cat_Com_Proveedores.Campo_Proveedor_ID + "=" + Cat_Com_Det_Part_Prov.Tabla_Cat_Com_Det_Part_Prov +
                               "." + Cat_Com_Det_Part_Prov.Campo_Proveedor_ID + ") Representante,");

                Mi_SQL.Append("(Select " + Cat_Com_Proveedores.Campo_Contacto + " From " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores +
                               " Where " + Cat_Com_Proveedores.Campo_Proveedor_ID + "=" + Cat_Com_Det_Part_Prov.Tabla_Cat_Com_Det_Part_Prov +
                               "." + Cat_Com_Det_Part_Prov.Campo_Proveedor_ID + ") Contacto,");

                Mi_SQL.Append("(Select " + Cat_Com_Proveedores.Campo_RFC + " From " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores +
                               " Where " + Cat_Com_Proveedores.Campo_Proveedor_ID + "=" + Cat_Com_Det_Part_Prov.Tabla_Cat_Com_Det_Part_Prov +
                               "." + Cat_Com_Det_Part_Prov.Campo_Proveedor_ID + ") Rfc,");

                Mi_SQL.Append("(Select " + Cat_Com_Proveedores.Campo_Estatus + " From " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores +
                               " Where " + Cat_Com_Proveedores.Campo_Proveedor_ID + "=" + Cat_Com_Det_Part_Prov.Tabla_Cat_Com_Det_Part_Prov +
                               "." + Cat_Com_Det_Part_Prov.Campo_Proveedor_ID + ") Estatus,");

                Mi_SQL.Append("(Select " + Cat_Com_Proveedores.Campo_Telefono_1 + " From " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores +
                               " Where " + Cat_Com_Proveedores.Campo_Proveedor_ID + "=" + Cat_Com_Det_Part_Prov.Tabla_Cat_Com_Det_Part_Prov +
                               "." + Cat_Com_Det_Part_Prov.Campo_Proveedor_ID + ") Telefono1,");

                Mi_SQL.Append("(Select " + Cat_Com_Proveedores.Campo_Telefono_2 + " From " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores +
                               " Where " + Cat_Com_Proveedores.Campo_Proveedor_ID + "=" + Cat_Com_Det_Part_Prov.Tabla_Cat_Com_Det_Part_Prov +
                               "." + Cat_Com_Det_Part_Prov.Campo_Proveedor_ID + ") Telefono2 ");

                Mi_SQL.Append(" From " + Cat_Com_Det_Part_Prov.Tabla_Cat_Com_Det_Part_Prov);
                Mi_SQL.Append(" Where " + Cat_Com_Det_Part_Prov.Tabla_Cat_Com_Det_Part_Prov + "." + 
                                Cat_Com_Det_Part_Prov.Campo_Partida_Generica_ID +
                                "='" + Datos.P_Partida_Generica_ID + "'");

                Mi_SQL.Append("order by NOMBRE_PROVEEDOR");
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Movimientos Presupuestales que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
        }

        
        /// ********************************************************************************************************************
        /// NOMBRE:         Consultar_Ventas_Proveedor
        /// COMENTARIOS:    Consulta las ventas realizadas en un lapso de tiempo
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ:     19/Enero/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Ventas_Proveedor(Cls_Rpt_Com_Proveedores_Excel_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Movimiento = new DataTable();
            try
            {
                Mi_SQL.Append("Select * from " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra);
                Mi_SQL.Append(" where " + Ope_Com_Ordenes_Compra.Campo_Estatus + "='" + Datos.P_Estatus + "'" +
                                " and " + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + ">='" + Datos.P_Fecha_Inicial + "'" +
                                " and " + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + "<='" + Datos.P_Fecha_Final + "'");
                Mi_SQL.Append(" Order by " + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID);
                
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Movimientos Presupuestales que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
        }
        /// ********************************************************************************************************************
        /// NOMBRE:         Consultar_Ventas_Realizadas
        /// COMENTARIOS:    Consulta cunatas ventas se realizaron en un lapso de tiempo
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ:     19/Enero/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Ventas_Realizadas(Cls_Rpt_Com_Proveedores_Excel_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Movimiento = new DataTable();
            try
            {
                Mi_SQL.Append("Select  COUNT(" + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID + ") ARTICULOS_VENDIDOS" +
                                " from " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra);
                Mi_SQL.Append(" where " + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID + "='" + Datos.P_Proveedor_ID + "'" +
                                " and " + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + ">='" + Datos.P_Fecha_Inicial + "'" +
                                " and " + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + "<='" + Datos.P_Fecha_Final + "'" +
                                " and " + Ope_Com_Ordenes_Compra.Campo_Estatus + "='" + Datos.P_Estatus + "'");
                Mi_SQL.Append(" Order by " + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID);
                
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Movimientos Presupuestales que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
        }
        /// ********************************************************************************************************************
        /// NOMBRE:         Consultar_Suma_Ventas_Realizadas
        /// COMENTARIOS:    Consulta las ventas realizadas en un lapso de tiempo
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ:     19/Enero/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Suma_Ventas_Realizadas(Cls_Rpt_Com_Proveedores_Excel_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Movimiento = new DataTable();
            try
            {
                Mi_SQL.Append("Select  Sum(" + Ope_Com_Ordenes_Compra.Campo_Total + ") SUMA_TOTAL," +
                                "Sum(" + Ope_Com_Ordenes_Compra.Campo_Total_IEPS + ") SUMA_TOTAL_IEPS," +
                                "Sum(" + Ope_Com_Ordenes_Compra.Campo_Subtotal + ") SUMA_SUBTOTAL," +
                                "Sum(" + Ope_Com_Ordenes_Compra.Campo_Total_IVA + ") SUMA_TOTAL_IVA" +
                                " from " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra);
                Mi_SQL.Append(" where " + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID + "='" + Datos.P_Proveedor_ID + "'" +
                                " and " + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + ">='" + Datos.P_Fecha_Inicial + "'" +
                                " and " + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + "<='" + Datos.P_Fecha_Final + "'" +
                                " and " + Ope_Com_Ordenes_Compra.Campo_Estatus + "='" + Datos.P_Estatus + "'");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Movimientos Presupuestales que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
        }
        /// ********************************************************************************************************************
        /// NOMBRE:         Consultar_Tipos_Articulos_Vendidos
        /// COMENTARIOS:    Consulta las ventas realizadas en un lapso de tiempo
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ:     19/Enero/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Tipos_Articulos_Vendidos(Cls_Rpt_Com_Proveedores_Excel_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Movimiento = new DataTable();
            try
            {
                Mi_SQL.Append("Select  distinct(" + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + ") TIPO_ARTICULO" +
                                " from " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra);
                Mi_SQL.Append(" where " + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID + "='" + Datos.P_Proveedor_ID + "'" +
                                " and " + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + ">='" + Datos.P_Fecha_Inicial + "'" +
                                " and " + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + "<='" + Datos.P_Fecha_Final + "'" +
                                " and " + Ope_Com_Ordenes_Compra.Campo_Estatus + "='" + Datos.P_Estatus + "'");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Movimientos Presupuestales que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
        }
        /// ********************************************************************************************************************
        /// NOMBRE:         Consultar_Nombre_Articulos_Vendidos
        /// COMENTARIOS:    Consulta los nombres de los articulos vendidos en un lapso de tiempo
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ:     17/Febrero/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Nombre_Articulos_Vendidos(Cls_Rpt_Com_Proveedores_Excel_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Movimiento = new DataTable();
            try
            {
                Mi_SQL.Append("Select  distinct(" + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio + ") TIPO_ARTICULO" +
                                " from " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto);
                Mi_SQL.Append(" where " + Ope_Com_Req_Producto.Campo_Requisicion_ID + "='" + Datos.P_No_Requisicion +"'");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Movimientos Presupuestales que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
        }


        
        

        /// ********************************************************************************************************************
        /// NOMBRE:         Consultar_Fecha_Registro
        /// COMENTARIOS:    Consulta los nombres de los articulos vendidos en un lapso de tiempo
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Susana Trigueros
        /// FECHA CREÓ:     17/Febrero/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Fecha_Registro(Cls_Rpt_Com_Proveedores_Excel_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Movimiento = new DataTable();
            try
            {
                Mi_SQL.Append("SELECT " + Cat_Com_Proveedores.Campo_Proveedor_ID + ","  + Cat_Com_Proveedores.Campo_RFC + " AS Rfc, " + Cat_Com_Proveedores.Campo_Nombre + " AS Nombre_Proveedor, ");
                
                //Mi_SQL.Append("(Select " + Cat_SAP_Partida_Generica.Campo_Clave + " from " +
                //                Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + " where " +
                //                Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID + "=" +
                //                Cat_Com_Det_Part_Prov.Tabla_Cat_Com_Det_Part_Prov + "." +
                //                Cat_Com_Det_Part_Prov.Campo_Partida_Generica_ID + ") Clave_Partida,");

                Mi_SQL.Append(Cat_Com_Proveedores.Campo_Compañia + " AS Compañia ," + Cat_Com_Proveedores.Campo_Representante_Legal + " AS Representante,");
                Mi_SQL.Append(Cat_Com_Proveedores.Campo_Contacto + " AS Contacto ," + Cat_Com_Proveedores.Campo_Estatus + " AS  Estatus, ");
                Mi_SQL.Append(Cat_Com_Proveedores.Campo_Fecha_Registro + " AS Fecha_Registro, " + Cat_Com_Proveedores.Campo_Telefono_1 + " AS Telefono1,");
                Mi_SQL.Append(Cat_Com_Proveedores.Campo_Telefono_2 + " AS Telefono2 FROM " +Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores);
                Mi_SQL.Append(" WHERE " + Cat_Com_Proveedores.Campo_Fecha_Registro + ">='" + Datos.P_Fecha_Inicial + "'");
                Mi_SQL.Append(" AND " + Cat_Com_Proveedores.Campo_Fecha_Registro + "<='" + Datos.P_Fecha_Final + "'");

                

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Movimientos Presupuestales que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
        }
        /// ********************************************************************************************************************
        /// NOMBRE:         Consultar_Fecha_Actualizacion
        /// COMENTARIOS:    Consulta los nombres de los articulos vendidos en un lapso de tiempo
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Susana Trigueros
        /// FECHA CREÓ:     17/Febrero/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Fecha_Actualizacion(Cls_Rpt_Com_Proveedores_Excel_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Movimiento = new DataTable();
            try
            {
                Mi_SQL.Append("SELECT " + Cat_Com_Proveedores.Campo_Proveedor_ID + "," + Cat_Com_Proveedores.Campo_RFC + " AS Rfc, " + Cat_Com_Proveedores.Campo_Nombre + " AS Nombre_Proveedor, ");

                //Mi_SQL.Append("(Select " + Cat_SAP_Partida_Generica.Campo_Clave + " from " +
                //                Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + " where " +
                //                Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID + "=" +
                //                Cat_Com_Det_Part_Prov.Tabla_Cat_Com_Det_Part_Prov + "." +
                //                Cat_Com_Det_Part_Prov.Campo_Partida_Generica_ID + ") Clave_Partida,");

                Mi_SQL.Append(Cat_Com_Proveedores.Campo_Compañia + " AS Compañia ," + Cat_Com_Proveedores.Campo_Representante_Legal + " AS Representante,");
                Mi_SQL.Append(Cat_Com_Proveedores.Campo_Contacto + " AS Contacto ," + Cat_Com_Proveedores.Campo_Estatus + " AS  Estatus, ");
                Mi_SQL.Append(Cat_Com_Proveedores.Campo_Fecha_Actualizacion + " AS Fecha_Actualizacion, " + Cat_Com_Proveedores.Campo_Telefono_1 + " AS Telefono1,");
                Mi_SQL.Append(Cat_Com_Proveedores.Campo_Telefono_2 + " AS Telefono2 FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores);
                Mi_SQL.Append(" WHERE " + Cat_Com_Proveedores.Campo_Fecha_Actualizacion + ">='" + Datos.P_Fecha_Inicial + "'");
                Mi_SQL.Append(" AND " + Cat_Com_Proveedores.Campo_Fecha_Actualizacion + "<='" + Datos.P_Fecha_Final + "'");



                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Movimientos Presupuestales que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
        }
        #endregion

    }
}