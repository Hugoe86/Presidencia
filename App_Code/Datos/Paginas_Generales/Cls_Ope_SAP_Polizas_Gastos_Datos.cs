using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Polizas_Gastos.Negocio;

namespace Presidencia.Polizas_Gastos.Datos
{
    public class Cls_Ope_SAP_Polizas_Gastos_Datos
    {
        public Cls_Ope_SAP_Polizas_Gastos_Datos()
        {
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consultar_Gastos
        /// 	DESCRIPCIÓN: Consulta la tabla Ope_Com_Gastos filtrados por rango de fechas o por folio
        /// 	PARÁMETROS:
        /// 		1. Negocio: Datos para filtrar los Gastos
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 28-abr-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Gastos(Cls_Ope_SAP_Polizas_Gastos_Negocio Negocio)
        {
            String Mi_Sql = "";

            Mi_Sql = "SELECT " + Ope_Com_Gastos.Tabla_Ope_Com_Gastos + ".*, ";
            Mi_Sql += Cat_Dependencias.Tabla_Cat_Dependencias + "." ; //De la tabla CAt_Dependencias obtener clave y nombre de la dependencia
            Mi_Sql += Cat_Dependencias.Campo_Clave + " || '  ' || ";
            Mi_Sql += Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS CLAVE_NOMBRE_DEPENDENCIA ";
            Mi_Sql += " FROM " + Ope_Com_Gastos.Tabla_Ope_Com_Gastos;
            Mi_Sql += ", " + Cat_Dependencias.Tabla_Cat_Dependencias;
            Mi_Sql += " WHERE " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID;
            Mi_Sql += " = " + Ope_Com_Gastos.Tabla_Ope_Com_Gastos + "." + Ope_Com_Gastos.Campo_Dependencia_ID;

            if (Negocio.P_Folio != null && Negocio.P_Folio != "")   //Si se recibio un folio, filtrar por folio
            {
                Mi_Sql += " AND UPPER(" + Ope_Com_Gastos.Campo_Folio + ") LIKE UPPER('" + Negocio.P_Folio + "')";
            }
                //filtrar por fecha si se recibio fecha inicial y final
            else if (Negocio.P_Fecha_Final != null && Negocio.P_Fecha_Final != "" && Negocio.P_Fecha_Inicial != null && Negocio.P_Fecha_Inicial != "")
            {
                Mi_Sql += " AND TO_DATE(TO_CHAR(" + Ope_Com_Gastos.Tabla_Ope_Com_Gastos + "." + 
                            Ope_Com_Gastos.Campo_Fecha_Creo + ",'DD/MM/YY'))" +
                        " >= '" + Convert.ToDateTime(Negocio.P_Fecha_Inicial).ToString("dd/MM/yyyy") + "' AND " +
                        "TO_DATE(TO_CHAR(" + Ope_Com_Gastos.Tabla_Ope_Com_Gastos + "." + 
                            Ope_Com_Gastos.Campo_Fecha_Creo + ",'DD/MM/YY'))" +
                        " <= '" + Convert.ToDateTime(Negocio.P_Fecha_Final).ToString("dd/MM/yyyy") + "'" +
                        " AND " + Ope_Com_Gastos.Tabla_Ope_Com_Gastos + "." + Ope_Com_Consolidaciones.Campo_Estatus + 
                            " IN ('" + Negocio.P_Estatus + "')";
            }
            Mi_Sql += " ORDER BY " + Ope_Com_Gastos.Tabla_Ope_Com_Gastos + "." + Ope_Com_Gastos.Campo_Folio;
            DataTable _DataTable = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql).Tables[0];
            if (_DataTable.Rows.Count > 0)      // Si la consulta arrojo resultados, regresar el datatable con los resultados
            {
                return _DataTable;
            }
            return null;
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consultar_Datos_Poliza
        /// 	DESCRIPCIÓN: 1.Consulta varias tablas para obtener la informacion que conforma la poliza de 
        /// 	                un Gasto
        /// 	PARÁMETROS:
        /// 		1. Gasto: Gasto del que se obtendran los datos
        /// 	CREO: Roberto González
        /// 	FECHA_CREO: 28-abr-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Datos_Poliza(String Gasto)
        {
            String Mi_SQL; //Variable para la consulta de los productos
            
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
                Mi_SQL += Ope_Com_Gastos.Tabla_Ope_Com_Gastos + ".";
                Mi_SQL += Ope_Com_Gastos.Campo_Numero_Reserva + ", ";
                Mi_SQL += Ope_Com_Gastos.Tabla_Ope_Com_Gastos + ".";
                Mi_SQL += Ope_Com_Gastos.Campo_Costo_Total_Gasto + ", ";
                Mi_SQL += Ope_Com_Gastos.Tabla_Ope_Com_Gastos + ".";
                Mi_SQL += Ope_Com_Gastos.Campo_No_Factura + ", ";
                Mi_SQL += Ope_Com_Gastos.Tabla_Ope_Com_Gastos + ".";
                Mi_SQL += Ope_Com_Gastos.Campo_Fecha_Factura + ", ";
                Mi_SQL += Ope_Com_Gastos.Tabla_Ope_Com_Gastos + ".";
                Mi_SQL += Ope_Com_Gastos.Campo_Folio + " AS FOLIO_GASTO, ";
                Mi_SQL += Ope_SAP_Parametros.Tabla_Ope_SAP_Parametros + ".* ";
                Mi_SQL += " FROM ";
                Mi_SQL += Ope_Com_Gastos.Tabla_Ope_Com_Gastos + ", ";
                Mi_SQL += Cat_Com_Partidas.Tabla_Cat_Com_Partidas + ", ";
                Mi_SQL += Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + ", ";
                Mi_SQL += Cat_Dependencias.Tabla_Cat_Dependencias + ", ";
                Mi_SQL += Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + ", ";
                Mi_SQL += Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + ", ";
                Mi_SQL += Ope_SAP_Parametros.Tabla_Ope_SAP_Parametros;
                Mi_SQL += " WHERE ";
                Mi_SQL += Ope_Com_Gastos.Tabla_Ope_Com_Gastos + ".";
                Mi_SQL += Ope_Com_Gastos.Campo_Gasto_ID + " = " + Gasto;
                Mi_SQL += " AND " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + ".";
                Mi_SQL += Cat_Com_Proveedores.Campo_Proveedor_ID + " = ";
                Mi_SQL += Ope_Com_Gastos.Tabla_Ope_Com_Gastos + ".";
                Mi_SQL += Ope_Com_Gastos.Campo_Proveedor_ID;
                Mi_SQL += " AND " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas + ".";
                Mi_SQL += Cat_Com_Partidas.Campo_Partida_ID + " = ";
                Mi_SQL += Ope_Com_Gastos.Tabla_Ope_Com_Gastos + ".";
                Mi_SQL += Ope_Com_Gastos.Campo_Partida_ID;
                Mi_SQL += " AND " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + ".";
                Mi_SQL += Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + " = ";
                Mi_SQL += Ope_Com_Gastos.Tabla_Ope_Com_Gastos + ".";
                Mi_SQL += Ope_Com_Gastos.Campo_Proyecto_Programa_ID;
                Mi_SQL += " AND " + Cat_Dependencias.Tabla_Cat_Dependencias + ".";
                Mi_SQL += Cat_Dependencias.Campo_Dependencia_ID + " = ";
                Mi_SQL += Ope_Com_Gastos.Tabla_Ope_Com_Gastos + ".";
                Mi_SQL += Ope_Com_Gastos.Campo_Dependencia_ID;
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

    }//clase
}//namespace
