using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Generacion_Polizas.Negocio;

namespace Presidencia.Generacion_Polizas.Datos
{
    public class Cls_Ope_SAP_Generacion_Polizas_Datos
    {
        public Cls_Ope_SAP_Generacion_Polizas_Datos()
        {
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consultar_Datos_Poliza
        /// 	DESCRIPCIÓN: 1.Consulta varias tablas para obtener la informacion que conforma la poliza de 
        /// 	                un orden de compra
        /// 	PARÁMETROS:
        /// 		1. Orden_Compra: Numero de la orden de compra de la que se obtendran los datos
        /// 	CREO: Roberto González
        /// 	FECHA_CREO: 18-abr-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Datos_Poliza(String Orden_Compra)
        {
            String Mi_SQL; //Variable para la consulta de los productos
            String Subconsulta_Lista_Requisiciones_SQL = ""; //Variable para el filtro de la consulta SQL

            // Subconsulta para obtener la lista de requisiciones de la orden de compra dada
            Subconsulta_Lista_Requisiciones_SQL = "SELECT " + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones +
                " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " WHERE " +
                Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = " + Orden_Compra;

            try
            {
                Mi_SQL = "SELECT " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas + ".";             //Campos Partida especifica
                Mi_SQL += Cat_Com_Partidas.Campo_Clave_SAP + " AS CLAVE_OPERACION_SAP, ";
                Mi_SQL += Cat_Com_Partidas.Tabla_Cat_Com_Partidas + ".";
                Mi_SQL += Cat_Com_Partidas.Campo_Cuenta_SAP + " AS CUENTA_SAP, ";
                Mi_SQL += Cat_Com_Partidas.Tabla_Cat_Com_Partidas + ".";
                Mi_SQL += Cat_Com_Partidas.Campo_Centro_Aplicacion + ", ";
                Mi_SQL += Cat_Com_Partidas.Tabla_Cat_Com_Partidas + ".";
                Mi_SQL += Cat_Com_Partidas.Campo_Afecta_Area_Funcional + ", ";
                Mi_SQL += Cat_Com_Partidas.Tabla_Cat_Com_Partidas + ".";
                Mi_SQL += Cat_Com_Partidas.Campo_Afecta_Partida + ", ";
                Mi_SQL += Cat_Com_Partidas.Tabla_Cat_Com_Partidas + ".";
                Mi_SQL += Cat_Com_Partidas.Campo_Afecta_Elemento_PEP + ", ";
                Mi_SQL += Cat_Com_Partidas.Tabla_Cat_Com_Partidas + ".";
                Mi_SQL += Cat_Com_Partidas.Campo_Afecta_Fondo + ", ";
                Mi_SQL += Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + ".";  //Campos Proyectos programas (clave)
                Mi_SQL += Cat_Com_Proyectos_Programas.Campo_Elemento_PEP + ", ";
                Mi_SQL += Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + ".";
                Mi_SQL += Cat_Com_Proyectos_Programas.Campo_Clave + " AS CLAVE_PROGRAMA, ";
                Mi_SQL += Cat_Dependencias.Tabla_Cat_Dependencias + ".";                        //Campos Dependencias (clave)
                Mi_SQL += Cat_Dependencias.Campo_Clave + " AS CLAVE_DEPENDENCIA, ";
                Mi_SQL += Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + ".";            //Campos Area funcional (clave)
                Mi_SQL += Cat_SAP_Area_Funcional.Campo_Clave + " AS CLAVE_AREA_FUNCIONAL, ";
                Mi_SQL += Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + ".";                  //Campos Proveedores (forma de pago y dias de credito)
                Mi_SQL += Cat_Com_Proveedores.Campo_Forma_Pago + ", ";
                Mi_SQL += Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + ".";
                Mi_SQL += Cat_Com_Proveedores.Campo_Dias_Credito + ", ";
                Mi_SQL += Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + ".";//Campos Facturas proveedores (factura y fecha)
                Mi_SQL += Ope_Com_Facturas_Proveedores.Campo_Fecha_Factura + ", ";
                Mi_SQL += Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + ".";
                Mi_SQL += Ope_Com_Facturas_Proveedores.Campo_No_Factura_Proveedor + ", ";
                Mi_SQL += Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ".";
                Mi_SQL += Ope_Com_Ordenes_Compra.Campo_No_Reserva + ", ";
                Mi_SQL += Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ".";
                Mi_SQL += Ope_Com_Ordenes_Compra.Campo_Folio + " AS FOLIO_ORDEN_COMPRA, ";
                Mi_SQL += Ope_SAP_Parametros.Tabla_Ope_SAP_Parametros + ".*, ";
                Mi_SQL += Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + ".";
                Mi_SQL += Ope_Com_Req_Producto.Campo_Total_Cotizado;
                Mi_SQL += " FROM ";
                Mi_SQL += Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ", ";
                Mi_SQL += Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ", ";
                Mi_SQL += Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + ", ";
                Mi_SQL += Cat_Com_Partidas.Tabla_Cat_Com_Partidas + ", ";
                Mi_SQL += Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + ", ";
                Mi_SQL += Cat_Dependencias.Tabla_Cat_Dependencias + ", ";
                Mi_SQL += Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + ", ";
                Mi_SQL += Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + ", ";
                Mi_SQL += Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + ", ";
                Mi_SQL += Ope_SAP_Parametros.Tabla_Ope_SAP_Parametros;
                Mi_SQL += " WHERE ";
                Mi_SQL += Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ".";
                Mi_SQL += Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = " + Orden_Compra;
                Mi_SQL += " AND " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + ".";
                Mi_SQL += Cat_Com_Proveedores.Campo_Proveedor_ID + " = ";
                Mi_SQL += Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ".";
                Mi_SQL += Ope_Com_Ordenes_Compra.Campo_Proveedor_ID;
                Mi_SQL += " AND " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + ".";
                Mi_SQL += Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " = ";
                Mi_SQL += Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ".";
                Mi_SQL += Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno;
                Mi_SQL += " AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ".";
                Mi_SQL += Ope_Com_Requisiciones.Campo_Requisicion_ID + " IN (" + Subconsulta_Lista_Requisiciones_SQL + ")";
                Mi_SQL += " AND " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + ".";
                Mi_SQL += Ope_Com_Req_Producto.Campo_Requisicion_ID + " = ";
                Mi_SQL += Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ".";
                Mi_SQL += Ope_Com_Requisiciones.Campo_Requisicion_ID;
                Mi_SQL += " AND " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas + ".";
                Mi_SQL += Cat_Com_Partidas.Campo_Partida_ID + " = ";
                Mi_SQL += Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + ".";
                Mi_SQL += Ope_Com_Req_Producto.Campo_Partida_ID;
                Mi_SQL += " AND " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + ".";
                Mi_SQL += Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + " = ";
                Mi_SQL += Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + ".";
                Mi_SQL += Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID;
                Mi_SQL += " AND " + Cat_Dependencias.Tabla_Cat_Dependencias + ".";
                Mi_SQL += Cat_Dependencias.Campo_Dependencia_ID + " = ";
                Mi_SQL += Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ".";
                Mi_SQL += Ope_Com_Requisiciones.Campo_Dependencia_ID;
                Mi_SQL += " AND " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + ".";
                Mi_SQL += Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID + " = ";
                Mi_SQL += Cat_Dependencias.Tabla_Cat_Dependencias + ".";
                Mi_SQL += Cat_Dependencias.Campo_Area_Funcional_ID;


                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
        }

    }//clase Cls_Ope_SAP_Generacion_Polizas_Datos
}//namespace