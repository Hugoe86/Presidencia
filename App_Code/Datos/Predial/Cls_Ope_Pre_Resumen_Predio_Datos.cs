using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using Presidencia.Constantes;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Operacion_Resumen_Predio.Negocio;
/// <summary>
/// Summary description for Cls_Ope_Pre_Resumen_Predio_Datos
/// </summary>
/// 
namespace Presidencia.Operacion_Resumen_Predio.Datos
{

    public class Cls_Ope_Pre_Resumen_Predio_Datos
    {


        #region Metodos
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Descuentos_Recargos
        ///DESCRIPCIÓN: Consulta los descuentos en base al mes
        ///PARAMETROS:Consulta la cuenta predial ´para mandar mensaje si esta aun tiene un estatus de pendiente
        ///CREO: Christian Perez barra
        ///FECHA_CREO: 9/Octubre/1987
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Descuentos_Recargos(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = " SELECT ";
                Mi_SQL = Mi_SQL + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + ".*";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + " inner join ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " on ";
                Mi_SQL = Mi_SQL + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Cuenta_Predial_ID + "=";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "='" + Datos.P_Cuenta_Predial_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Estatus + "='VIGENTE'";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Descuentos_Pronto_Pago
        ///DESCRIPCIÓN: Consulta los descuentos en base al mes
        ///PARAMETROS:Consulta la cuenta predial ´para mandar mensaje si esta aun tiene un estatus de pendiente
        ///CREO: Christian Perez barra
        ///FECHA_CREO: 9/Octubre/1987
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Descuentos_Pronto_Pago(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = " SELECT ";
                Mi_SQL = Mi_SQL + Cat_Pre_Descuentos_Predial.Tabla_Cat_Pre_Descuentos_Predial + ".*";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Descuentos_Predial.Tabla_Cat_Pre_Descuentos_Predial;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Descuentos_Predial.Tabla_Cat_Pre_Descuentos_Predial + "." + Cat_Pre_Descuentos_Predial.Campo_Año + " = " + "'" + DateTime.Now.Year + "'";

                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Adeudos_Cuenta_Predial_Con_Totales
        /// DESCRIPCIÓN: Consulta los adeudos de una cuenta predial dada (regresa datatable con adeudos), 
        ///             incluye un campo con el total por anio
        /// PARÁMETROS:
        /// 	1. Cuenta_Predial: Cuenta predial de la que se obtendrán los adeudos
        /// 	2. Estatus: si se especifica, se agrega filtro a la contulta
        /// 	3. Desde_Anio: si se especifica, se agrega filtro a la contulta
        /// 	4. Hasta_Anio: si se especifica, se agrega filtro a la contulta
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 25-sep-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Adeudos_Cuenta_Predial_Con_Totales(String Cuenta_Predial, String Estatus, Int32 Desde_Anio, Int32 Hasta_Anio)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL
            String Filtros_SQL = "";

            try
            {
                Mi_SQL = "WITH ADEUDOS AS ( SELECT ";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Anio + ", Sum(NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + ", 0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ", 0))  ADEUDO_BIMESTRE_1, Sum(NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + ", 0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ", 0))  ADEUDO_BIMESTRE_2, Sum(NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + ", 0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ", 0))  ADEUDO_BIMESTRE_3, Sum(NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + ", 0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ", 0))  ADEUDO_BIMESTRE_4, Sum(NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + ", 0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ", 0))  ADEUDO_BIMESTRE_5, Sum(NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + ", 0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ", 0))  ADEUDO_BIMESTRE_6, Sum((NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + ", 0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ", 0)) + (NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + ", 0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ", 0)) + (NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + ", 0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ", 0))  + (NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + ", 0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ", 0)) + (NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + ", 0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ", 0)) + (NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + ", 0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ", 0))) ADEUDO_TOTAL_ANIO";
                Mi_SQL += " FROM " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial;
                if (!String.IsNullOrEmpty(Estatus))   // filtrar desde fecha si llego parametro
                {
                    Filtros_SQL += " WHERE " + Ope_Pre_Adeudos_Predial.Campo_Estatus + " LIKE '" + Estatus + "'";
                }
                if (!String.IsNullOrEmpty(Cuenta_Predial))   // filtrar por cuenta predial si llego como parametro
                {
                    if (Filtros_SQL.Length == 0)        // si no hay filtro, agregar nuevos filtros con WHERE
                    {
                        Filtros_SQL += " WHERE " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID;
                        Filtros_SQL += " = '" + Cuenta_Predial + "'";
                    }
                    else
                    {
                        Filtros_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID;
                        Filtros_SQL += " = '" + Cuenta_Predial + "'";
                    }
                }
                if (Desde_Anio > 0)   // filtrar desde anio si llego parametro
                {
                    if (Filtros_SQL.Length == 0)        // si no hay filtro, agregar nuevos filtros con WHERE
                    {
                        Filtros_SQL += " WHERE " + Ope_Pre_Adeudos_Predial.Campo_Anio;
                        Filtros_SQL += " >= " + Desde_Anio;
                    }
                    else
                    {
                        Filtros_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_Anio;
                        Filtros_SQL += " >= " + Desde_Anio;
                    }
                }
                if (Hasta_Anio > 0)   // filtrar desde anio si llego parametro
                {
                    if (Filtros_SQL.Length == 0)        // si no hay filtro, agregar nuevos filtros con WHERE
                    {
                        Filtros_SQL += " WHERE " + Ope_Pre_Adeudos_Predial.Campo_Anio;
                        Filtros_SQL += " <= " + Hasta_Anio;
                    }
                    else
                    {
                        Filtros_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_Anio;
                        Filtros_SQL += " <= " + Hasta_Anio;
                    }
                }
                Filtros_SQL += " Group By " + Ope_Pre_Adeudos_Predial.Campo_Anio + " ORDER BY " + Ope_Pre_Adeudos_Predial.Campo_Anio;
                Mi_SQL += Filtros_SQL
                    + " ) SELECT * FROM ADEUDOS "
                    + " WHERE ADEUDO_TOTAL_ANIO <> 0 ";
                //+ " WHERE (NVL(ADEUDO_BIMESTRE_1, 0) + NVL(ADEUDO_BIMESTRE_2, 0) + NVL(ADEUDO_BIMESTRE_3, 0) + NVL(ADEUDO_BIMESTRE_4, 0) + NVL(ADEUDO_BIMESTRE_5, 0) + NVL(ADEUDO_BIMESTRE_6, 0)) > 0 ";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }   //FUNCIÓN: Consultar_Adeudos_Cuenta_Predial_Con_Totales

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN      : Consultar_Adeudos_Cancelados_Cuenta_Predial_Con_Totales
        /// DESCRIPCIÓN         : Consulta los adeudos Cancelados de una cuenta predial dada (regresa datatable con adeudos), incluye un campo con el total por anio
        /// PARÁMETROS:
        /// 1. Cuenta_Predial: Cuenta predial de la que se obtendrán los adeudos
        /// 2. Estatus: si se especifica, se agrega filtro a la contulta
        /// 3. Desde_Anio: si se especifica, se agrega filtro a la contulta
        /// 4. Hasta_Anio: si se especifica, se agrega filtro a la contulta
        /// CREO                : Antonio Salvador Benavides Guardado
        /// FECHA_CREO          : 02/Marzo/2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Adeudos_Cancelados_Cuenta_Predial_Con_Totales(String Cuenta_Predial, String No_Orden_Variacion, String Estatus, Int32 Desde_Anio, Int32 Hasta_Anio)
        {
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "WITH ADEUDOS AS ( SELECT ";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Anio + ", Sum(NVL(";
                Mi_SQL += Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle + "." + Ope_Pre_Diferencias_Detalle.Campo_Bimestre_1 + ", 0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ", 0))  ADEUDO_BIMESTRE_1, Sum(NVL(";
                Mi_SQL += Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle + "." + Ope_Pre_Diferencias_Detalle.Campo_Bimestre_2 + ", 0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ", 0))  ADEUDO_BIMESTRE_2, Sum(NVL(";
                Mi_SQL += Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle + "." + Ope_Pre_Diferencias_Detalle.Campo_Bimestre_3 + ", 0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ", 0))  ADEUDO_BIMESTRE_3, Sum(NVL(";
                Mi_SQL += Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle + "." + Ope_Pre_Diferencias_Detalle.Campo_Bimestre_4 + ", 0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ", 0))  ADEUDO_BIMESTRE_4, Sum(NVL(";
                Mi_SQL += Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle + "." + Ope_Pre_Diferencias_Detalle.Campo_Bimestre_5 + ", 0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ", 0))  ADEUDO_BIMESTRE_5, Sum(NVL(";
                Mi_SQL += Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle + "." + Ope_Pre_Diferencias_Detalle.Campo_Bimestre_6 + ", 0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ", 0))  ADEUDO_BIMESTRE_6, Sum((NVL(";
                Mi_SQL += Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle + "." + Ope_Pre_Diferencias_Detalle.Campo_Bimestre_1 + ", 0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ", 0)) + (NVL(";
                Mi_SQL += Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle + "." + Ope_Pre_Diferencias_Detalle.Campo_Bimestre_2 + ", 0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ", 0)) + (NVL(";
                Mi_SQL += Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle + "." + Ope_Pre_Diferencias_Detalle.Campo_Bimestre_3 + ", 0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ", 0))  + (NVL(";
                Mi_SQL += Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle + "." + Ope_Pre_Diferencias_Detalle.Campo_Bimestre_4 + ", 0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ", 0)) + (NVL(";
                Mi_SQL += Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle + "." + Ope_Pre_Diferencias_Detalle.Campo_Bimestre_5 + ", 0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ", 0)) + (NVL(";
                Mi_SQL += Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle + "." + Ope_Pre_Diferencias_Detalle.Campo_Bimestre_6 + ", 0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ", 0))) ADEUDO_TOTAL_ANIO";
                Mi_SQL += " FROM " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + ", " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + ", " + Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle;
                Mi_SQL += " WHERE " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_Cuenta_Predial_ID + " AND ";
                Mi_SQL += Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_No_Diferencia + " = " + Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle + "." + Ope_Pre_Diferencias_Detalle.Campo_No_Diferencia + " AND ";
                Mi_SQL += "TO_CHAR(" + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Anio + ") = SUBSTR(" + Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle + "." + Ope_Pre_Diferencias_Detalle.Campo_Periodo + ", -4) AND ";
                if (!String.IsNullOrEmpty(Estatus))   // filtrar desde fecha si llego parametro
                {
                    Mi_SQL += Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Estatus + " LIKE '" + Estatus + "' AND ";
                }
                if (!String.IsNullOrEmpty(Cuenta_Predial))   // filtrar por cuenta predial si llego como parametro
                {
                    Mi_SQL += Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID;
                    Mi_SQL += " = '" + Cuenta_Predial + "' AND ";
                }
                if (!String.IsNullOrEmpty(No_Orden_Variacion))   // filtrar por cuenta predial si llego como parametro
                {
                    Mi_SQL += Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_No_Orden_Variacion;
                    Mi_SQL += " = '" + No_Orden_Variacion + "' AND ";
                }
                if (Desde_Anio > 0)   // filtrar desde anio si llego parametro
                {
                    Mi_SQL += Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Anio;
                    Mi_SQL += " >= " + Desde_Anio + " AND ";
                }
                if (Hasta_Anio > 0)   // filtrar desde anio si llego parametro
                {
                    Mi_SQL += Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Anio;
                    Mi_SQL += " <= " + Hasta_Anio + " AND ";
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                Mi_SQL += " GROUP BY " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Anio;
                Mi_SQL += " ORDER BY " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Anio;
                Mi_SQL += " ) SELECT * FROM ADEUDOS";
                Mi_SQL += " WHERE ADEUDO_TOTAL_ANIO <> 0 ";
                //Mi_SQL += " WHERE (NVL(ADEUDO_BIMESTRE_1, 0) + NVL(ADEUDO_BIMESTRE_2, 0) + NVL(ADEUDO_BIMESTRE_3, 0) + NVL(ADEUDO_BIMESTRE_4, 0) + NVL(ADEUDO_BIMESTRE_5, 0) + NVL(ADEUDO_BIMESTRE_6, 0)) > 0 ";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }   //FUNCIÓN: Adeudos_Cancelados_Cuenta_Predial_Con_Totales


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tipo_Predial
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene el tipo de Predio
        ///PARAMETROS:Consulta la cuenta predial ´para mandar mensaje si esta aun tiene un estatus de pendiente
        ///CREO: Jacqueline Ramírez Sierra
        ///FECHA_CREO: 29/Septiembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Estatus_Cuentas(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = " SELECT ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ",";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ",";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estatus;

                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estatus + " = " + "'PENDIENTE'";

                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tasa
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene los datos en un
        ///             DataTable.
        ///PARAMETROS:     
        ///             1.  No Calle ID
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 08/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Tasa(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            DataTable Dt_Traslado_Dominio = new DataTable();
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT  ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + "." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual + " as Descripcion";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Tasas_Predial.Tabla_Cat_Pre_Tasas_Predial + " INNER JOIN  ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + " on ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + "." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Predial_ID + "=";
                Mi_SQL = Mi_SQL + Cat_Pre_Tasas_Predial.Tabla_Cat_Pre_Tasas_Predial + "." + Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID + " where ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + "." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID;
                Mi_SQL = Mi_SQL + "='" + Datos.P_Tasa_Predial_ID + "' order by ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + "." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID + " desc";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Detalles_Cuenta
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene los datos en un
        ///             DataTable.
        ///PARAMETROS:     
        ///             1.  No Calle ID
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 08/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Detalles_Cuenta(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            DataTable Dt_Traslado_Dominio = new DataTable();
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT To_Number(";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ") AS No_Contrarecibo,";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + " AS Fecha,";
                Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Identificador + " as Movimiento,";
                Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Nombre + " ||' '|| ";
                Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " ||' '|| ";
                Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " AS Propietario";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " left outer join ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " on ";
                Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Movimiento_ID + "=";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID;
                Mi_SQL = Mi_SQL + " left outer JOIN  ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " ON ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "=";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID;
                Mi_SQL = Mi_SQL + " left outer JOIN ";
                Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + " on ";
                Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + "." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + "=";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo;
                Mi_SQL = Mi_SQL + " left outer join ";
                Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + " on ";
                Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + "." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + "=";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " left outer join ";
                Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " on ";
                Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + "." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID + "=";
                Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " ";
                Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "='" + Datos.P_Cuenta_Predial_ID + "' AND ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + "='ACEPTADA' and ";
                Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + "." + Ope_Pre_Contrarecibos.Campo_Estatus + "!='PAGADO' AND ";
                Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + "." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + "='PROPIETARIO' AND ";
                Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + "." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus + "='ALTA'";
                //Mi_SQL = Mi_SQL + " order by " + Ope_Pre_Orden_Detalles.Tabla_Ope_Pre_Orden_Detalles + "." + Ope_Pre_Orden_Detalles.Campo_No_Orden_Detalles  + " DESC";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Ultimo_Movimiento
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene los datos en un
        ///             DataTable.
        ///PARAMETROS:     
        ///             1.  No Calle ID
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 08/Agosto/2011
        ///MODIFICO: Roberto Gonzalez Oseguera
        ///FECHA_MODIFICO: 26-mar-2012
        ///CAUSA_MODIFICACIÓN: Se agrega subconsulta para omitir movimientos pendientes de 
        ///             aplicar ( contrarecibo con estatus diferente de PAGADO)
        ///*******************************************************************************
        public static DataTable Consultar_Ultimo_Movimiento(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT  ";
                Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." +
                         Cat_Pre_Movimientos.Campo_Identificador + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." +
                         Cat_Pre_Movimientos.Campo_Descripcion;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " INNER JOIN  ";
                Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " ON ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." +
                         Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + " = ";
                Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." +
                         Cat_Pre_Movimientos.Campo_Movimiento_ID;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." +
                         Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." +
                         Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " = 'ACEPTADA'";
                Mi_SQL = Mi_SQL + " AND NVL(" + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                    + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + ",-1) NOT IN (SELECT "
                    + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " FROM "
                    + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + " WHERE "
                    + Ope_Pre_Contrarecibos.Campo_Estatus + " != 'PAGADO')";
                Mi_SQL = Mi_SQL + " Order By " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." +
                             Ope_Pre_Ordenes_Variacion.Campo_Fecha_Modifico + " DESC,";
                Mi_SQL = Mi_SQL + " " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." +
                         Ope_Pre_Ordenes_Variacion.Campo_No_Nota + " DESC";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]";
                //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Imprimir_Resumen_Generales
        ///DESCRIPCIÓN: se consultan los datos generales de la cuenta predial
        ///PARAMETROS: 
        ///CREO: christian Perez Ibarra
        ///FECHA_CREO: 24/Agosto/2011 11:49:25 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        internal static DataTable Consultar_Imprimir_Resumen_Generales(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = ""; //Variable para la consulta SQL            
            DataSet Ds_Resultado = new DataSet();
            try
            {
                Mi_SQL = "";
                Mi_SQL += "SELECT";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ",";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ",";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen + ",";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + " as Tipo_Predio,";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID + " as Uso_Predio,";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID + " as Estado_Predio,";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Estatus + ",";
                Mi_SQL += " NVL(CUEN." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + ",0) " + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + ",";
                Mi_SQL += " NVL(CUEN." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + ",0) " + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + ",";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + " as Numero_Exterior,";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_No_Interior + " as Numero_Interior,";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral + ",";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Efectos + ",";
                Mi_SQL += " CALLES." + Cat_Pre_Calles.Campo_Calle_ID + " AS CALLE_ID, ";
                Mi_SQL += " CALLES." + Cat_Pre_Calles.Campo_Nombre + " AS Ubicacion, ";
                Mi_SQL += " CALLES." + Cat_Pre_Calles.Campo_Colonia_ID + ", ";
                Mi_SQL += " COL." + Cat_Ate_Colonias.Campo_Nombre + " AS COLONIA, ";
                Mi_SQL += " CUOTA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + " AS CUOTA_MINIMA_ID, ";
                Mi_SQL += " CUOTA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota + " AS CUOTA, ";
                Mi_SQL += " '' AS ESTATUS_RESUMEN ";

                Mi_SQL += " FROM ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUEN ";


                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES ON CALLES." + Cat_Pre_Calles.Campo_Calle_ID + " = CUEN.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_ID;
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COL ON COL." + Cat_Ate_Colonias.Campo_Colonia_ID;
                Mi_SQL += " = CALLES." + Cat_Pre_Calles.Campo_Colonia_ID + " LEFT OUTER JOIN " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas + " CUOTA ON ";
                Mi_SQL += " CUOTA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + " = ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID + " ";
                Mi_SQL += " WHERE CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Datos.P_Cuenta_Predial + "'";
                DataTable Dt_Folios_Guardados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                if (Dt_Folios_Guardados != null)
                {
                    Dt_Folios_Guardados.Columns.Add("Ultimo_Movimiento");
                    Tabla = Dt_Folios_Guardados.Clone();
                    DataRow No_Folio;

                    No_Folio = Tabla.NewRow();
                    No_Folio["Cuenta_Predial"] = Dt_Folios_Guardados.Rows[0]["Cuenta_Predial"].ToString();
                    No_Folio["Cuenta_Origen"] = Dt_Folios_Guardados.Rows[0]["Cuenta_Origen"].ToString();
                    if (!string.IsNullOrEmpty(Dt_Folios_Guardados.Rows[0]["Cuenta_Predial_ID"].ToString().Trim()))
                    {
                        Mi_SQL = "SELECT  ";
                        Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Identificador + " as Descripcion";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " INNER JOIN  ";
                        Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " on ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + "=";
                        Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Movimiento_ID;
                        Mi_SQL = Mi_SQL + " Where " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + "='" + Dt_Folios_Guardados.Rows[0]["Cuenta_Predial_ID"].ToString();
                        Mi_SQL = Mi_SQL + "' and " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + "='ACEPTADA'";
                        Mi_SQL = Mi_SQL + " order by " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Modifico + " DESC";
                        DataSet Ds_Ultimo_Movimiento = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        if (Ds_Ultimo_Movimiento.Tables[0].Rows.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(Ds_Ultimo_Movimiento.Tables[0].Rows[0]["Descripcion"].ToString().Trim()))
                            {
                                No_Folio["Ultimo_Movimiento"] = Ds_Ultimo_Movimiento.Tables[0].Rows[0]["Descripcion"].ToString();
                            }
                        }
                    }
                    //btener el tipo de predio
                    if (!string.IsNullOrEmpty(Dt_Folios_Guardados.Rows[0]["Tipo_Predio"].ToString().Trim()))
                    {
                        Mi_SQL = "SELECT " + Cat_Pre_Tipos_Predio.Campo_Descripcion;
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio;
                        Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + "='" + Dt_Folios_Guardados.Rows[0]["Tipo_Predio"].ToString() + "'";
                        DataSet Ds_Predio = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        No_Folio["Tipo_Predio"] = Ds_Predio.Tables[0].Rows[0]["Descripcion"].ToString();
                    }
                    //Obtener el uso del predio
                    if (!string.IsNullOrEmpty(Dt_Folios_Guardados.Rows[0]["Uso_Predio"].ToString().Trim()))
                    {
                        Mi_SQL = "SELECT * ";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Uso_Suelo.Tabla_Cat_Pre_Uso_Suelo;
                        Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Uso_Suelo.Campo_Uso_Suelo_ID + "='" + Dt_Folios_Guardados.Rows[0]["Uso_Predio"].ToString() + "'";
                        DataSet Ds_Uso_Predio = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        if (Ds_Uso_Predio.Tables[0].Rows.Count - 1 >= 0)
                        {
                            if (!string.IsNullOrEmpty(Ds_Uso_Predio.Tables[0].Rows[0]["Descripcion"].ToString().Trim()))
                            {
                                No_Folio["Uso_Predio"] = Ds_Uso_Predio.Tables[0].Rows[0]["Descripcion"].ToString();
                            }
                        }
                    }
                    No_Folio["Ubicacion"] = Dt_Folios_Guardados.Rows[0]["Ubicacion"].ToString();
                    No_Folio["Colonia"] = Dt_Folios_Guardados.Rows[0]["Colonia"].ToString();
                    if (!string.IsNullOrEmpty(Dt_Folios_Guardados.Rows[0]["Estado_Predio"].ToString().Trim()))
                    {
                        Mi_SQL = "SELECT " + Cat_Pre_Estados_Predio.Campo_Descripcion;
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Estados_Predio.Tabla_Cat_Pre_Estados_Predio;
                        Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Estados_Predio.Campo_Estado_Predio_ID + "='" + Dt_Folios_Guardados.Rows[0]["Estado_Predio"].ToString() + "'";
                        DataSet Ds_Estado_Predio = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        No_Folio["Estado_Predio"] = Ds_Estado_Predio.Tables[0].Rows[0]["Descripcion"].ToString();
                    }
                    No_Folio["Superficie_Construida"] = Dt_Folios_Guardados.Rows[0]["Superficie_Construida"];
                    No_Folio["Superficie_Total"] = Dt_Folios_Guardados.Rows[0]["Superficie_Total"];
                    No_Folio["Estatus"] = Dt_Folios_Guardados.Rows[0]["Estatus"].ToString();
                    //No_Folio["Colonia"] = Dt_Folios_Guardados.Rows[0]["Colonia"].ToString();
                    No_Folio["Numero_Exterior"] = Dt_Folios_Guardados.Rows[0]["Numero_Exterior"].ToString();
                    No_Folio["Numero_Interior"] = Dt_Folios_Guardados.Rows[0]["Numero_Interior"].ToString();
                    No_Folio["Clave_Catastral"] = Dt_Folios_Guardados.Rows[0]["Clave_Catastral"].ToString();
                    No_Folio["Efectos"] = Dt_Folios_Guardados.Rows[0]["Efectos"].ToString();
                    //No_Folio["Ultimo_Movimiento"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["MULTAS"].ToString();
                    //No_Folio["GASTOS_EJECUCION"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["GASTOS_EJECUCION"].ToString();
                    //if (Dt_Folios_Guardados.Rows[Cuenta_Folios]["DESCUENTO_RECARGOS"].ToString().Trim() != "")
                    //{
                    //    No_Folio["DESCUENTO_RECARGOS"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["DESCUENTO_RECARGOS"].ToString();
                    //}
                    //if (Dt_Folios_Guardados.Rows[Cuenta_Folios]["DESCUENTO_HONORARIOS"].ToString().Trim() != "")
                    //{
                    //    No_Folio["DESCUENTO_HONORARIOS"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["DESCUENTO_HONORARIOS"].ToString();
                    //}
                    //if (Dt_Folios_Guardados.Rows[Cuenta_Folios]["DESCUENTO_PRONTO_PAGO"].ToString().Trim() != "")
                    //{
                    //    No_Folio["DESCUENTO_PRONTO_PAGO"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["DESCUENTO_PRONTO_PAGO"].ToString();
                    //}
                    //    if (Dt_Folios_Guardados.Rows[Cuenta_Folios]["FECHA"].ToString().Trim() != "")
                    //{
                    //    No_Folio["FECHA"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["FECHA"].ToString();
                    //}
                    //No_Folio["NO_RECIBO"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["NO_RECIBO"].ToString();
                    //No_Folio["NO_OPERACION"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["NO_OPERACION"].ToString();
                    //No_Folio["NUMERO_CAJA"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["NUMERO_CAJA"].ToString();
                    //No_Folio["CUENTA_PREDIAL"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["CUENTA_PREDIAL"].ToString();
                    //No_Folio["CLAVE_BANCO"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["CLAVE_BANCO"].ToString();
                    if (Tabla.Rows.Count == 0)
                    {
                        Tabla.Rows.InsertAt(No_Folio, 0);

                    }
                    else
                    {
                        Tabla.Rows.InsertAt(No_Folio, Tabla.Rows.Count);
                    }
                }
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Mi_SQL + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
            return Tabla;
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Datos_Cuenta_Generales
        ///DESCRIPCIÓN: se consultan los datos generales de la cuenta predial
        ///PARAMETROS: 
        ///CREO: christian Perez Ibarra
        ///FECHA_CREO: 08/11/2011 11:49:25 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        internal static DataSet Consulta_Datos_Cuenta_Generales(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL            
            DataSet Ds_Resultado = new DataSet();
            try
            {
                Mi_SQL = "SELECT "
                    + "CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Estatus
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_No_Interior
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Efectos
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion
                    + ", TO_CHAR(CUEN." + Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo + ", 'dd-Mon-yyyy') "
                    + Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Usuario_Creo
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Fecha_Creo
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Usuario_Modifico
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Fecha_Modifico
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Costo_m2
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Tasa_ID
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_No_Diferencia
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion
                    + ", CUEN." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion;
                Mi_SQL += ", CALLES." + Cat_Pre_Calles.Campo_Calle_ID + " AS CALLE_ID";
                Mi_SQL += ", CALLES." + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE";
                Mi_SQL += ", CALLES." + Cat_Pre_Calles.Campo_Colonia_ID;
                Mi_SQL += ", COL." + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA";
                Mi_SQL += ", CUOTA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + " AS CUOTA_MINIMA_ID";
                Mi_SQL += ", CUOTA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota + " AS CUOTA";

                Mi_SQL += " FROM ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUEN";

                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES ON CALLES." + Cat_Pre_Calles.Campo_Calle_ID + " = CUEN.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_ID;
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COL ON COL." + Cat_Ate_Colonias.Campo_Colonia_ID;
                Mi_SQL += " = CALLES." + Cat_Pre_Calles.Campo_Colonia_ID + " LEFT OUTER JOIN " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas + " CUOTA ON ";
                Mi_SQL += " CUOTA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + " = ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID + " ";

                //Mi_SQL += " WHERE CUEN." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " = '" + Datos.P_Contrarecibo + "'";
                Mi_SQL += " WHERE CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Datos.P_Cuenta_Predial + "'";

                Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                Ds_Resultado.Tables[0].TableName = "Dt_Generales";

                return Ds_Resultado;
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Mi_SQL + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Estado_Cuenta
        ///DESCRIPCIÓN: Hace una consulta al estado de cuenta
        ///PARAMETROS:     
        ///             1.  No Cuenta_Predial
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 18/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        internal static DataTable Consultar_Estado_Cuenta(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL           

            try
            {
                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_No_Adeudo + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Anio + ",";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " as Predial,";
                Mi_SQL = Mi_SQL + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + " ||' - '|| ";
                Mi_SQL = Mi_SQL + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + " ||' - '|| ";
                Mi_SQL = Mi_SQL + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + " ||' - '|| ";
                Mi_SQL = Mi_SQL + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + " ||' - '|| ";
                Mi_SQL = Mi_SQL + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + " ||' - '|| ";
                Mi_SQL = Mi_SQL + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + " as Periodo,";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " as Predial,";
                Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Monto_Recargos + " as Recargos,";
                Mi_SQL = Mi_SQL + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Monto_Por_Pagar + " as Total_Monto_Pagar ";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + " LEFT OUTER JOIN ";
                Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + " on ";
                Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Cuenta_Predial_Id + "=";
                Mi_SQL = Mi_SQL + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + " LEFT OUTER JOIN ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " on ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "=";
                Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Cuenta_Predial_Id + " ";
                Mi_SQL = Mi_SQL + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Datos.P_Cuenta_Predial + "'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Anio + " ASC ";
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Mi_SQL + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Convenios_Detalles
        ///DESCRIPCIÓN: Hace una consulta a los detalles de la cuota fija
        ///PARAMETROS:     
        ///             1.  No Cuota Fija
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 16/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        internal static DataTable Consultar_Convenios_Detalles(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL           
            DataTable Tabla = new DataTable();
            try
            {
                Mi_SQL = "";
                Mi_SQL = "SELECT *";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision;
                Mi_SQL = Mi_SQL + " WHERE ";
                Mi_SQL = Mi_SQL + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + Datos.P_No_Convenio + "'";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset.Tables[0].Rows.Count > 0)
                {
                    Tabla = dataset.Tables[0];
                }
                else if (dataset.Tables[0].Rows.Count <= 0)
                {
                    Mi_SQL = "";
                    Mi_SQL = "SELECT *";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos;
                    Mi_SQL = Mi_SQL + " WHERE ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + Datos.P_No_Convenio + "'";
                    dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (dataset.Tables[0].Rows.Count > 0)
                    {
                        Tabla = dataset.Tables[0];
                    }
                    else if (dataset.Tables[0].Rows.Count <= 0)
                    {
                        Mi_SQL = "";
                        Mi_SQL = "SELECT *";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio;
                        Mi_SQL = Mi_SQL + " WHERE ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio + " = '" + Datos.P_No_Convenio + "'";
                        dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                }


            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Mi_SQL + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cuota_Fija_Detalles
        ///DESCRIPCIÓN: Hace una consulta a los detalles de la cuota fija
        ///PARAMETROS:     
        ///             1.  No Cuota Fija
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 16/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        internal static DataTable Consultar_Cuota_Fija_Detalles(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL           

            try
            {
                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_Plazo_Financiamiento + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_Exedente_Construccion + " AS EXCEDENTE_CONSTRUCCION,";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_Exedente_Valor + " AS EXCEDENTE_VALOR,";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_Total_Cuota_Fija + " AS TOTAL,";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_Cuota_Minima + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_Excedente_Construccion_Total + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_Excedente_Valor_Total + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_Tasa_Valor + ",";
                Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + "." + Cat_Pre_Casos_Especiales.Campo_Articulo + " ||'   '|| ";
                Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + "." + Cat_Pre_Casos_Especiales.Campo_Inciso + " ||'   '|| ";
                Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + "." + Cat_Pre_Casos_Especiales.Campo_Observaciones + " as Fundamento_Legal,";
                Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + "." + Cat_Pre_Casos_Especiales.Campo_Tipo + " ";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + " inner join ";
                Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + " on ";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_Caso_Especial_Id + "=";
                Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + "." + Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " ";
                Mi_SQL = Mi_SQL + " WHERE ";
                if (Datos.P_No_Cuota_Fija.Trim().Length == 5)
                {
                    Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + "." + Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " = '" + Datos.P_No_Cuota_Fija + "'";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_No_Cuota_Fija + " = '" + Datos.P_No_Cuota_Fija + "'";
                }
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Mi_SQL + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Historial_Pagos
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene el tipo de Predio
        ///PARAMETROS:     
        ///             1.  Tipo Predio ID 
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 05/Agosto/2011
        ///MODIFICO: Roberto González Oseguera
        ///FECHA_MODIFICO: 23-feb-20120
        ///CAUSA_MODIFICACIÓN: Se quitó la suma de montos para obtener el total y se agregan 
        ///     los campos total y ajuste_tarifario a la consulta. También se reestructuró el código
        ///*******************************************************************************
        public static DataTable Consultar_Historial_Pagos(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            DataTable Dt_Traslado_Dominio = new DataTable();
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "";
                Mi_SQL += "WITH PAGOS_CANCELADOS AS (";
                Mi_SQL += "SELECT PAGOS_1." + Ope_Caj_Pagos.Campo_No_Recibo;
                Mi_SQL += " ,PAGOS_1." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID;
                Mi_SQL += " FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " PASIVOS";
                Mi_SQL += " ," + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " PAGOS_1";
                Mi_SQL += " ," + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " PAGOS_2";
                Mi_SQL += " ," + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " CAJA";
                Mi_SQL += " WHERE PAGOS_1." + Ope_Caj_Pagos.Campo_No_Pago + " = PASIVOS." + Ope_Ing_Pasivo.Campo_No_Pago;
                Mi_SQL += " AND PAGOS_1." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " = PAGOS_2." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID;
                Mi_SQL += " AND PAGOS_1." + Ope_Caj_Pagos.Campo_No_Recibo + " = PAGOS_2." + Ope_Caj_Pagos.Campo_No_Recibo;
                Mi_SQL += " AND (PAGOS_1." + Ope_Caj_Pagos.Campo_Caja_ID + " = CAJA." + Cat_Pre_Cajas.Campo_Caja_Id;
                Mi_SQL += "   OR PAGOS_2." + Ope_Caj_Pagos.Campo_Caja_ID + " = CAJA." + Cat_Pre_Cajas.Campo_Caja_Id + ")";
                Mi_SQL += " AND PAGOS_1." + Ope_Caj_Pagos.Campo_Estatus + " = 'PAGADO'";
                Mi_SQL += " AND PAGOS_2." + Ope_Caj_Pagos.Campo_Estatus + " = 'CANCELADO'";
                //Mi_SQL += " AND PAGOS_1." + Ope_Caj_Pagos.Campo_No_Turno + " = '" + Cancelacion.P_No_Turno + "'";
                Mi_SQL += " AND PASIVOS." + Ope_Ing_Pasivo.Campo_Descripcion + " <> 'AJUSTE TARIFARIO'";
                Mi_SQL += ") ";

                Mi_SQL += "SELECT " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Tipo_Pago + " AS PREDIAL"
                    + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo
                    + ", REPLACE(TO_CHAR(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha
                    + ", 'dd-Mon-yyyy', 'NLS_LANGUAGE = \"MEXICAN SPANISH\"'),'Dec','Dic') as Fecha ";
                Mi_SQL += ", to_number(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "."
                    + Ope_Caj_Pagos.Campo_No_Operacion + ") as No_Operacion";
                Mi_SQL += ", " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " as Numero_Caja";
                //Mi_SQL = Mi_SQL + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Clave_Banco + " as Clave_Banco, ";
                Mi_SQL += "," + Cat_Pre_Instituciones_Que_Reciben_Pagos.Tabla_Cat_Pre_Instituciones_Que_Reciben_Pagos + "."
                    + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion + " as Clave_Banco ";
                Mi_SQL += "," + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial;

                Mi_SQL += "," + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Periodo_Rezago + " ||' '|| "
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Periodo_Corriente + " as Periodo  ";
                Mi_SQL += ",NVL(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Periodo_Rezago + ",'-')" + Ope_Caj_Pagos.Campo_Periodo_Rezago
                    + ",NVL(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Periodo_Corriente + ",'-')" + Ope_Caj_Pagos.Campo_Periodo_Corriente;

                Mi_SQL += ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Monto_Rezago;
                Mi_SQL += ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Monto_Corriente;
                Mi_SQL += ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Monto_Recargos + " as Recargos_Ordinarios";
                Mi_SQL += ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Monto_Recargos_Moratorios + " as Recargos_Moratorios";

                Mi_SQL += ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Honorarios
                    + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Gastos_Ejecucion
                    + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Descuento_Recargos
                    + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Descuento_Moratorios + " as Descuento_Moratorios"
                    + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Descuento_Honorarios
                    + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Descuento_Pronto_Pago
                    + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Ajuste_Tarifario
                    + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Total;
                // agregar propietario de la cuenta si se especifica
                if (Datos.P_Incluir_Propietario == true)
                {
                    Mi_SQL += ",NVL((SELECT "
                        + Ope_Ing_Pasivo.Campo_Contribuyente
                        + " FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo
                        + " WHERE " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Recibo + " = "
                        + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo
                        + " AND " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Pago + " = "
                        + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago
                        + " AND " + Ope_Ing_Pasivo.Campo_Contribuyente + " IS NOT NULL"
                        + " AND ROWNUM=1"
                        + "),(SELECT TRIM("
                       + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || "
                       + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || "
                       + Cat_Pre_Contribuyentes.Campo_Nombre + ")"
                       + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes
                       + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = "
                       + "(SELECT " + Cat_Pre_Propietarios.Campo_Contribuyente_ID
                       + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios
                       + " WHERE " + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + "="
                       + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " AND "
                       + Cat_Pre_Propietarios.Campo_Tipo + " IN ('POSEEDOR','PROPIETARIO') AND ROWNUM = 1)"
                        + ")) NOMBRE_PROPIETARIO";
                }

                Mi_SQL += " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas
                    + " inner join "
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " on "
                    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "="
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID
                    + " inner join "
                    + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " on "
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + "="
                    + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id
                    + " left outer join "
                    + Cat_Pre_Instituciones_Que_Reciben_Pagos.Tabla_Cat_Pre_Instituciones_Que_Reciben_Pagos + " on "
                    + Cat_Pre_Instituciones_Que_Reciben_Pagos.Tabla_Cat_Pre_Instituciones_Que_Reciben_Pagos + "."
                    + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Caja + "="
                    + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id;

                Mi_SQL += " Where (" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "."
                    + Ope_Caj_Pagos.Campo_Tipo_Pago + "='PREDIAL' OR "
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Tipo_Pago + "='PREDIAL CONVENIO') ";

                if (!string.IsNullOrEmpty(Datos.P_Cuenta_Predial_ID))
                {
                    Mi_SQL += " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "."
                        + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + "='" + Datos.P_Cuenta_Predial_ID + "'";
                }

                // filtrar por fecha
                if (Datos.P_Desde_Fecha != DateTime.MinValue)
                {
                    Mi_SQL += " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "."
                        + Ope_Caj_Pagos.Campo_Fecha + ">='" + Datos.P_Desde_Fecha.ToString("dd/MM/yy") + "'";
                }

                // filtrar por fecha
                if (Datos.P_Hasta_Fecha != DateTime.MinValue)
                {
                    Mi_SQL += " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "."
                        + Ope_Caj_Pagos.Campo_Fecha + "<='" + Datos.P_Hasta_Fecha.ToString("dd/MM/yy") + "'";
                }


                Mi_SQL += " AND (" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "."
                    + Ope_Caj_Pagos.Campo_Tipo_Pago + "='PREDIAL' OR "
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Tipo_Pago + "='PREDIAL CONVENIO') ";


                //Mi_SQL += " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "."
                //    + Ope_Caj_Pagos.Campo_Estatus + "!='CANCELADO'";
                Mi_SQL = Mi_SQL + " AND NOT " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo + " || " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " IN (SELECT PAGOS_CANCELADOS." + Ope_Caj_Pagos.Campo_No_Recibo + " | | PAGOS_CANCELADOS." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " FROM PAGOS_CANCELADOS)";

                Mi_SQL += " ORDER BY " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha + " DESC";
                Mi_SQL += ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo + " DESC";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Pagos_Constancias
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene el tipo de Predio
        ///PARAMETROS:     
        ///             1.  Tipo Predio ID 
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 05/Agosto/2011
        ///MODIFICO: Roberto González Oseguera
        ///FECHA_MODIFICO: 14-mar-2012
        ///CAUSA_MODIFICACIÓN: Se cambia consulta para tomar datos a partir de ope_caj_pagos en lugar de ope_pre_constancias
        ///*******************************************************************************
        public static DataTable Consultar_Pagos_Constancias(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            DataTable Dt_Traslado_Dominio = new DataTable();
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "";
                Mi_SQL += "WITH PAGOS_CANCELADOS AS (";
                Mi_SQL += "SELECT PAGOS_1." + Ope_Caj_Pagos.Campo_No_Recibo;
                Mi_SQL += " ,PAGOS_1." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID;
                Mi_SQL += " FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " PASIVOS";
                Mi_SQL += " ," + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " PAGOS_1";
                Mi_SQL += " ," + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " PAGOS_2";
                Mi_SQL += " ," + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " CAJA";
                Mi_SQL += " WHERE PAGOS_1." + Ope_Caj_Pagos.Campo_No_Pago + " = PASIVOS." + Ope_Ing_Pasivo.Campo_No_Pago;
                Mi_SQL += " AND PAGOS_1." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " = PAGOS_2." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID;
                Mi_SQL += " AND PAGOS_1." + Ope_Caj_Pagos.Campo_No_Recibo + " = PAGOS_2." + Ope_Caj_Pagos.Campo_No_Recibo;
                Mi_SQL += " AND (PAGOS_1." + Ope_Caj_Pagos.Campo_Caja_ID + " = CAJA." + Cat_Pre_Cajas.Campo_Caja_Id;
                Mi_SQL += "   OR PAGOS_2." + Ope_Caj_Pagos.Campo_Caja_ID + " = CAJA." + Cat_Pre_Cajas.Campo_Caja_Id + ")";
                Mi_SQL += " AND PAGOS_1." + Ope_Caj_Pagos.Campo_Estatus + " IN ('PAGADO', 'PARCIAL')";
                Mi_SQL += " AND PAGOS_2." + Ope_Caj_Pagos.Campo_Estatus + " = 'CANCELADO'";
                //Mi_SQL += " AND PAGOS_1." + Ope_Caj_Pagos.Campo_No_Turno + " = '" + Cancelacion.P_No_Turno + "'";
                Mi_SQL += " AND PASIVOS." + Ope_Ing_Pasivo.Campo_Descripcion + " <> 'AJUSTE TARIFARIO'";
                Mi_SQL += ") ";

                Mi_SQL += "SELECT ";
                Mi_SQL += "'CONSTANCIAS'"
                    + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo
                    + ", " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " as Numero_Caja"
                    + ", REPLACE(TO_CHAR(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha
                    + ", 'dd-Mon-yyyy', 'NLS_LANGUAGE = \"MEXICAN SPANISH\"'),'Dec','Dic') as Fecha"
                    + "," + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial;
                Mi_SQL += ",(SELECT " + Cat_Pre_Tipos_Constancias.Campo_Descripcion
                    + " FROM " + Cat_Pre_Tipos_Constancias.Tabla_Cat_Pre_Tipos_Constancias
                    + " WHERE " + Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID + "="
                    + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Tipo_Constancia_ID
                    + ") as Tipo_Constancia";
                Mi_SQL += "," + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Monto_Corriente + " as Importe"
                    + "," + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Folio + " AS NO_CONSTANCIA"
                    + ", (" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Total + ") as Total";
                // agregar propietario de la cuenta si se especifica
                if (Datos.P_Incluir_Propietario == true)
                {
                    Mi_SQL += ",NVL((SELECT "
                        + Ope_Ing_Pasivo.Campo_Contribuyente
                        + " FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo
                        + " WHERE " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Recibo + " = "
                        + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo
                        + " AND " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Pago + " = "
                        + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago
                        + " AND " + Ope_Ing_Pasivo.Campo_Contribuyente + " IS NOT NULL"
                        + " AND ROWNUM=1"
                        + "),(SELECT TRIM("
                       + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || "
                       + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || "
                      + Cat_Pre_Contribuyentes.Campo_Nombre + ")"
                       + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes
                       + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = "
                       + "(SELECT " + Cat_Pre_Propietarios.Campo_Contribuyente_ID
                       + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios
                       + " WHERE " + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + "="
                       + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " AND "
                       + Cat_Pre_Propietarios.Campo_Tipo + " IN ('POSEEDOR','PROPIETARIO') AND ROWNUM = 1)"
                        + ")) NOMBRE_PROPIETARIO";
                }

                Mi_SQL += " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos
                    + " LEFT JOIN "
                    + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + " on TRIM("
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Documento + ")= TRIM("
                    + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Folio + ")"
                    + " LEFT JOIN "
                    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " on "
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + "="
                    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID
                    + " LEFT JOIN "
                    + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " on "
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + "="
                    + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id;

                Mi_SQL += " Where (" + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Estatus + "='PAGADA'"
                    + " OR " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Estatus + "='IMPRESA')"
                    + " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Tipo_Pago + "='DOCUMENTOS'"
                    + " AND NOT " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo + " || " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " IN (SELECT PAGOS_CANCELADOS." + Ope_Caj_Pagos.Campo_No_Recibo + " | | PAGOS_CANCELADOS." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " FROM PAGOS_CANCELADOS)";
                //+ " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Estatus + "!='CANCELADO'";

                if (!string.IsNullOrEmpty(Datos.P_Cuenta_Predial_ID))
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + "='" + Datos.P_Cuenta_Predial_ID + "'";
                }

                // filtrar por fecha
                if (Datos.P_Desde_Fecha != DateTime.MinValue)
                {
                    Mi_SQL += " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "."
                        + Ope_Caj_Pagos.Campo_Fecha + ">='" + Datos.P_Desde_Fecha.ToString("dd/MM/yy") + "'";
                }

                // filtrar por fecha
                if (Datos.P_Hasta_Fecha != DateTime.MinValue)
                {
                    Mi_SQL += " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "."
                        + Ope_Caj_Pagos.Campo_Fecha + "<='" + Datos.P_Hasta_Fecha.ToString("dd/MM/yy") + "'";
                }

                // UNION para constancias por traslado
                Mi_SQL += " UNION ";

                Mi_SQL += "SELECT 'CONSTANCIAS'"
                    + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo
                    + ", " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " as Numero_Caja"
                    + ", REPLACE(TO_CHAR(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha
                    + ", 'dd-Mon-yyyy', 'NLS_LANGUAGE = \"MEXICAN SPANISH\"'),'Dec','Dic') as Fecha"
                    + "," + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial
                    + ",'CONSTANCIA DE NO ADEUDO' AS Tipo_Constancia";
                Mi_SQL += "," + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Monto + " as Importe"
                    + "," + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Documento + " AS NO_CONSTANCIA"
                    + ", (" + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Monto + ") as Total";
                // agregar propietario de la cuenta si se especifica
                if (Datos.P_Incluir_Propietario == true)
                {
                    Mi_SQL += ",NVL((SELECT "
                        + Ope_Ing_Pasivo.Campo_Contribuyente
                        + " FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo
                        + " WHERE " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Recibo + " = "
                       + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo
                        + " AND " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Pago + " = "
                        + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago
                        + " AND " + Ope_Ing_Pasivo.Campo_Contribuyente + " IS NOT NULL"
                        + " AND ROWNUM=1"
                        + "),(SELECT TRIM("
                       + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || "
                       + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || "
                       + Cat_Pre_Contribuyentes.Campo_Nombre + ")"
                       + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes
                       + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = "
                       + "(SELECT " + Cat_Pre_Propietarios.Campo_Contribuyente_ID
                       + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios
                       + " WHERE " + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + "="
                       + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " AND "
                       + Cat_Pre_Propietarios.Campo_Tipo + " IN ('POSEEDOR','PROPIETARIO') AND ROWNUM = 1)"
                        + ")) NOMBRE_PROPIETARIO";
                }

                Mi_SQL += " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos
                    + " LEFT JOIN "
                    + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + " on TRIM("
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Documento + ")= TRIM("
                    + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Folio + ")"
                    + " LEFT JOIN "
                    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " on "
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + "="
                    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID
                    + " LEFT JOIN "
                    + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " on "
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + "="
                    + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id
                    + " LEFT JOIN "
                    + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " on "
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo + "="
                    + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Recibo + " AND "
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago + "="
                    + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Pago + " AND "
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Documento + "="
                    + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Referencia;

                Mi_SQL += " Where " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Descripcion + "='CONSTANCIA'"
                    + " AND UPPER(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Documento + ") LIKE 'TD%'"
                    + " AND NOT " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo + " || " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " IN (SELECT PAGOS_CANCELADOS." + Ope_Caj_Pagos.Campo_No_Recibo + " | | PAGOS_CANCELADOS." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " FROM PAGOS_CANCELADOS)";
                //+" AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Estatus + " IN ('PAGADO','PARCIAL')";

                if (!string.IsNullOrEmpty(Datos.P_Cuenta_Predial_ID))
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + "='" + Datos.P_Cuenta_Predial_ID + "'";
                }

                // filtrar por fecha
                if (Datos.P_Desde_Fecha != DateTime.MinValue)
                {
                    Mi_SQL += " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "."
                        + Ope_Caj_Pagos.Campo_Fecha + ">='" + Datos.P_Desde_Fecha.ToString("dd/MM/yy") + "'";
                }

                // filtrar por fecha
                if (Datos.P_Hasta_Fecha != DateTime.MinValue)
                {
                    Mi_SQL += " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "."
                        + Ope_Caj_Pagos.Campo_Fecha + "<='" + Datos.P_Hasta_Fecha.ToString("dd/MM/yy") + "'";
                }


                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Historial_Pagos_Fraccionamientos
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene el tipo de Predio
        ///PARAMETROS:     
        ///             1.  Tipo Predio ID 
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 05/Agosto/2011
        ///MODIFICO: Roberto González Oseguera
        ///FECHA_MODIFICO: 28-feb-20120
        ///CAUSA_MODIFICACIÓN: Se agregan descuento a multas y descuento a recargos a la consulta
        ///*******************************************************************************
        public static DataTable Consultar_Pagos_Traslado(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            DataTable Dt_Traslado_Dominio = new DataTable();
            DataTable Tabla = new DataTable();
            try
            {
                // formar primera parte de la consulta que toma datos de calculo de impuesto de traslado para pagos de traslado
                String Mi_SQL = "";
                Mi_SQL += "WITH PAGOS_CANCELADOS AS (";
                Mi_SQL += "SELECT PAGOS_1." + Ope_Caj_Pagos.Campo_No_Recibo;
                Mi_SQL += " ,PAGOS_1." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID;
                Mi_SQL += " FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " PASIVOS";
                Mi_SQL += " ," + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " PAGOS_1";
                Mi_SQL += " ," + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " PAGOS_2";
                Mi_SQL += " ," + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " CAJA";
                Mi_SQL += " WHERE PAGOS_1." + Ope_Caj_Pagos.Campo_No_Pago + " = PASIVOS." + Ope_Ing_Pasivo.Campo_No_Pago;
                Mi_SQL += " AND PAGOS_1." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " = PAGOS_2." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID;
                Mi_SQL += " AND PAGOS_1." + Ope_Caj_Pagos.Campo_No_Recibo + " = PAGOS_2." + Ope_Caj_Pagos.Campo_No_Recibo;
                Mi_SQL += " AND (PAGOS_1." + Ope_Caj_Pagos.Campo_Caja_ID + " = CAJA." + Cat_Pre_Cajas.Campo_Caja_Id;
                Mi_SQL += "   OR PAGOS_2." + Ope_Caj_Pagos.Campo_Caja_ID + " = CAJA." + Cat_Pre_Cajas.Campo_Caja_Id + ")";
                Mi_SQL += " AND PAGOS_1." + Ope_Caj_Pagos.Campo_Estatus + " = 'PAGADO'";
                Mi_SQL += " AND PAGOS_2." + Ope_Caj_Pagos.Campo_Estatus + " = 'CANCELADO'";
                //Mi_SQL += " AND PAGOS_1." + Ope_Caj_Pagos.Campo_No_Turno + " = '" + Cancelacion.P_No_Turno + "'";
                Mi_SQL += " AND PASIVOS." + Ope_Ing_Pasivo.Campo_Descripcion + " <> 'AJUSTE TARIFARIO'";
                Mi_SQL += ") ";

                Mi_SQL += "SELECT "
                    + "'TRASLADO' AS " + Ope_Caj_Pagos.Campo_Tipo_Pago + ", "
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo + ","
                    + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " as Numero_Caja,"
                    + "REPLACE(TO_CHAR(" + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Fecha
                    + ", 'dd-Mon-yyyy', 'NLS_LANGUAGE = \"MEXICAN SPANISH\"'),'Dec','Dic') as Fecha, ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ",";
                Mi_SQL += " NVL(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Monto_Corriente
                    + ",0) as Impuesto_Traslado,";
                Mi_SQL += " NVL(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Monto_Impuesto_Division + ",0) as Impuesto_Division,";
                Mi_SQL += " NVL(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Monto_Constancia + ",0) as Constancia_No_Adeudo,";
                Mi_SQL += " NVL(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Multas + ",0) as Multas, ";
                Mi_SQL += " NVL(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Descuento_Multas + ",0) as Descuento_Multas, ";
                Mi_SQL += " NVL(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Monto_Recargos + ",0) as Recargos, ";
                Mi_SQL += " NVL(" + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Descuento_Recargos + ",0) as Descuento_Recargos, ";
                Mi_SQL += " NVL(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Total + ",0) as Total ";
                // agregar propietario de la cuenta si se especifica
                if (Datos.P_Incluir_Propietario == true)
                {
                    Mi_SQL += ",NVL((SELECT "
                        + Ope_Ing_Pasivo.Campo_Contribuyente
                        + " FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo
                        + " WHERE " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Recibo + " = "
                        + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo
                        + " AND " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Pago + " = "
                        + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago
                        + " AND " + Ope_Ing_Pasivo.Campo_Contribuyente + " IS NOT NULL"
                        + " AND ROWNUM=1"
                        + "),(SELECT TRIM("
                       + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || "
                       + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || "
                       + Cat_Pre_Contribuyentes.Campo_Nombre + ")"
                       + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes
                       + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = "
                       + "(SELECT " + Cat_Pre_Propietarios.Campo_Contribuyente_ID
                       + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios
                       + " WHERE " + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + "="
                       + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " AND "
                       + Cat_Pre_Propietarios.Campo_Tipo + " IN ('POSEEDOR','PROPIETARIO') AND ROWNUM = 1)"
                        + ")) NOMBRE_PROPIETARIO";
                }

                Mi_SQL += " From " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos
                    + " inner join "
                    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " on "
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "."
                    + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + "="
                    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "."
                    + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID
                    + " inner join "
                    + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " on "
                    + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Caja_Id + "="
                    + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id;

                Mi_SQL += " Where " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Tipo_Pago
                    + "='TRASLADO DOMINIO'";

                // filtrar por cuenta predial
                if (!string.IsNullOrEmpty(Datos.P_Cuenta_Predial_ID))
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "."
                        + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + "='" + Datos.P_Cuenta_Predial_ID + "'";
                }

                // filtrar por fecha
                if (Datos.P_Desde_Fecha != DateTime.MinValue)
                {
                    Mi_SQL += " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "."
                        + Ope_Caj_Pagos.Campo_Fecha + ">='" + Datos.P_Desde_Fecha.ToString("dd/MM/yy") + "'";
                }

                // filtrar por fecha
                if (Datos.P_Hasta_Fecha != DateTime.MinValue)
                {
                    Mi_SQL += " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "."
                        + Ope_Caj_Pagos.Campo_Fecha + "<='" + Datos.P_Hasta_Fecha.ToString("dd/MM/yy") + "'";
                }

                //Mi_SQL += " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Estatus + "!='CANCELADO'";
                Mi_SQL = Mi_SQL + " AND NOT " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo + " || " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " IN (SELECT PAGOS_CANCELADOS." + Ope_Caj_Pagos.Campo_No_Recibo + " | | PAGOS_CANCELADOS." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " FROM PAGOS_CANCELADOS)";

                //Mi_SQL += " AND To_Number(" + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado
                //    + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + ")=";
                //Mi_SQL += "SUBSTR(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Documento
                //    + ",3,LENGTH(TRIM(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Documento + "))-6)";

                //Mi_SQL += " GROUP BY "
                //    + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_No_Recibo + ","
                //    + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " ,"
                //    + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Fecha + ", "
                //    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", "
                //    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", "
                //    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago;

                // UNION pagos de calculos y de convenios
                Mi_SQL += " UNION";

                Mi_SQL += " SELECT "
                    + "'TRASLADO CONVENIO' AS " + Ope_Caj_Pagos.Campo_Tipo_Pago + ", "
                    + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_No_Recibo + ","
                    + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " as Numero_Caja,"
                    + "REPLACE(TO_CHAR(" + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Fecha + ", 'dd-Mon-yyyy', 'NLS_LANGUAGE = \"MEXICAN SPANISH\"'),'Dec','Dic') as Fecha, ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ",";
                Mi_SQL += " NVL(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "."
                    + Ope_Caj_Pagos.Campo_Monto_Corriente + ",0) as Impuesto_Traslado,";
                Mi_SQL += " NVL(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "."
                    + Ope_Caj_Pagos.Campo_Monto_Impuesto_Division + ",0) as Impuesto_Division,";
                Mi_SQL += " NVL(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "."
                    + Ope_Caj_Pagos.Campo_Monto_Constancia + ",0) as Constancia_No_Adeudo,";
                Mi_SQL += " NVL(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "."
                    + Ope_Caj_Pagos.Campo_Multas + ",0) as Multas, ";
                Mi_SQL += " NVL(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Descuento_Multas + ",0) as Descuento_Multas, ";
                Mi_SQL += " NVL(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "."
                    + Ope_Caj_Pagos.Campo_Monto_Recargos + ",0) as Recargos, ";
                Mi_SQL += " NVL(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Descuento_Recargos + ",0) as Descuento_Recargos, ";
                Mi_SQL += " NVL(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Total + ",0) as Total ";
                // agregar propietario de la cuenta si se especifica
                if (Datos.P_Incluir_Propietario == true)
                {
                    Mi_SQL += ",NVL((SELECT "
                        + Ope_Ing_Pasivo.Campo_Contribuyente
                        + " FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo
                        + " WHERE " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Recibo + " = "
                        + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo
                        + " AND " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Pago + " = "
                        + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago
                        + " AND " + Ope_Ing_Pasivo.Campo_Contribuyente + " IS NOT NULL"
                        + " AND ROWNUM=1"
                        + "),(SELECT TRIM("
                       + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || "
                       + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || "
                       + Cat_Pre_Contribuyentes.Campo_Nombre + ")"
                       + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes
                       + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = "
                       + "(SELECT " + Cat_Pre_Propietarios.Campo_Contribuyente_ID
                       + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios
                       + " WHERE " + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + "="
                       + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " AND "
                       + Cat_Pre_Propietarios.Campo_Tipo + " IN ('POSEEDOR','PROPIETARIO') AND ROWNUM = 1)"
                        + ")) NOMBRE_PROPIETARIO";
                }

                Mi_SQL += " From " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos
                    + " inner join "
                    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " on "
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "."
                    + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + "="
                    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "."
                    + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID
                    + " inner join "
                    + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " on "
                    + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Caja_Id + "="
                    + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id;

                Mi_SQL += " Where " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Tipo_Pago
                    + "='TRASLADO CONVENIO'";

                // filtrar por cuenta predial
                if (!string.IsNullOrEmpty(Datos.P_Cuenta_Predial_ID))
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "."
                        + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + "='" + Datos.P_Cuenta_Predial_ID + "'";
                }

                // filtrar por fecha
                if (Datos.P_Desde_Fecha != DateTime.MinValue)
                {
                    Mi_SQL += " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "."
                        + Ope_Caj_Pagos.Campo_Fecha + ">='" + Datos.P_Desde_Fecha.ToString("dd/MM/yy") + "'";
                }

                // filtrar por fecha
                if (Datos.P_Hasta_Fecha != DateTime.MinValue)
                {
                    Mi_SQL += " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "."
                        + Ope_Caj_Pagos.Campo_Fecha + "<='" + Datos.P_Hasta_Fecha.ToString("dd/MM/yy") + "'";
                }

                //Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "."
                //    + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Estatus + "='PAGADO'";

                //Mi_SQL += " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Estatus + "!='CANCELADO'";
                Mi_SQL = Mi_SQL + " AND NOT " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo + " || " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " IN (SELECT PAGOS_CANCELADOS." + Ope_Caj_Pagos.Campo_No_Recibo + " | | PAGOS_CANCELADOS." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " FROM PAGOS_CANCELADOS)";

                //Mi_SQL += " GROUP BY "
                //    + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_No_Recibo + ","
                //    + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " ,"
                //    + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Fecha + ", "
                //    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", "
                //    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", "
                //    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago;

                // ejecutar consulta
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Historial_Pagos_Fraccionamientos
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene el tipo de Predio
        ///PARAMETROS:     
        ///             1.  Tipo Predio ID 
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 05/Agosto/2011
        ///MODIFICO: Roberto González Oseguera
        ///FECHA_MODIFICO: 28-feb-20120
        ///CAUSA_MODIFICACIÓN: Se agregan descuento a multas y descuento a recargos a la consulta
        ///*******************************************************************************
        public static DataTable Consultar_Pagos_Derechos_Supervision(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            DataTable Dt_Traslado_Dominio = new DataTable();
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "";
                Mi_SQL += "WITH PAGOS_CANCELADOS AS (";
                Mi_SQL += "SELECT PAGOS_1." + Ope_Caj_Pagos.Campo_No_Recibo;
                Mi_SQL += " ,PAGOS_1." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID;
                Mi_SQL += " FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " PASIVOS";
                Mi_SQL += " ," + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " PAGOS_1";
                Mi_SQL += " ," + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " PAGOS_2";
                Mi_SQL += " ," + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " CAJA";
                Mi_SQL += " WHERE PAGOS_1." + Ope_Caj_Pagos.Campo_No_Pago + " = PASIVOS." + Ope_Ing_Pasivo.Campo_No_Pago;
                Mi_SQL += " AND PAGOS_1." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " = PAGOS_2." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID;
                Mi_SQL += " AND PAGOS_1." + Ope_Caj_Pagos.Campo_No_Recibo + " = PAGOS_2." + Ope_Caj_Pagos.Campo_No_Recibo;
                Mi_SQL += " AND (PAGOS_1." + Ope_Caj_Pagos.Campo_Caja_ID + " = CAJA." + Cat_Pre_Cajas.Campo_Caja_Id;
                Mi_SQL += "   OR PAGOS_2." + Ope_Caj_Pagos.Campo_Caja_ID + " = CAJA." + Cat_Pre_Cajas.Campo_Caja_Id + ")";
                Mi_SQL += " AND PAGOS_1." + Ope_Caj_Pagos.Campo_Estatus + " = 'PAGADO'";
                Mi_SQL += " AND PAGOS_2." + Ope_Caj_Pagos.Campo_Estatus + " = 'CANCELADO'";
                //Mi_SQL += " AND PAGOS_1." + Ope_Caj_Pagos.Campo_No_Turno + " = '" + Cancelacion.P_No_Turno + "'";
                Mi_SQL += " AND PASIVOS." + Ope_Ing_Pasivo.Campo_Descripcion + " <> 'AJUSTE TARIFARIO'";
                Mi_SQL += ") ";

                Mi_SQL += "SELECT ";
                Mi_SQL = Mi_SQL + "'DERECHOS'" + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo + ","
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Tipo_Pago + ",";
                Mi_SQL = Mi_SQL + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " as Numero_Caja,";
                Mi_SQL = Mi_SQL + "REPLACE(TO_CHAR(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha
                    + ", 'dd-Mon-yyyy', 'NLS_LANGUAGE = \"MEXICAN SPANISH\"'),'Dec','Dic') as Fecha, ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ",";
                Mi_SQL = Mi_SQL + " Sum(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Monto_Corriente + ") as Importe,";
                Mi_SQL = Mi_SQL + " NVL(Sum(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Monto_Recargos + "),0) AS Recargos, ";
                Mi_SQL = Mi_SQL + " NVL(Sum(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Multas + "),0) AS Multas, ";
                Mi_SQL = Mi_SQL + " NVL(SUM(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Descuento_Multas + "),0) as Descuento_Multas, ";
                Mi_SQL = Mi_SQL + " NVL(SUM(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Descuento_Recargos + "),0) as Descuento_Recargos, ";
                Mi_SQL = Mi_SQL + " Sum(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Total + ") AS Total ";
                // agregar propietario de la cuenta si se especifica
                if (Datos.P_Incluir_Propietario == true)
                {
                    Mi_SQL += ",NVL((SELECT "
                        + Ope_Ing_Pasivo.Campo_Contribuyente
                        + " FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo
                        + " WHERE " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Recibo + " = "
                        + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo
                        + " AND " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Pago + " = "
                        + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago
                        + " AND " + Ope_Ing_Pasivo.Campo_Contribuyente + " IS NOT NULL"
                        + " AND ROWNUM=1"
                        + "),(SELECT TRIM("
                       + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || "
                       + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || "
                       + Cat_Pre_Contribuyentes.Campo_Nombre + ")"
                       + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes
                       + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = "
                       + "(SELECT " + Cat_Pre_Propietarios.Campo_Contribuyente_ID
                       + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios
                       + " WHERE " + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + "="
                       + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " AND "
                       + Cat_Pre_Propietarios.Campo_Tipo + " IN ('POSEEDOR','PROPIETARIO') AND ROWNUM = 1)"
                        + ")) NOMBRE_PROPIETARIO";
                }

                Mi_SQL = Mi_SQL + " From " + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + " inner join ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " on ";
                Mi_SQL = Mi_SQL + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + "." + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Cuenta_Predial_ID + "=";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " inner join ";
                Mi_SQL = Mi_SQL + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " on ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "=";
                Mi_SQL = Mi_SQL + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " inner join ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " on ";
                Mi_SQL = Mi_SQL + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + "=";
                Mi_SQL = Mi_SQL + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id;

                Mi_SQL = Mi_SQL + " Where (" + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + "." + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus + "='PAGADO'";
                Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + "." + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus + "='PARCIAL')";

                if (!string.IsNullOrEmpty(Datos.P_Cuenta_Predial_ID))
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + "='" + Datos.P_Cuenta_Predial_ID + "'";
                }

                Mi_SQL = Mi_SQL + " AND (" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Tipo_Pago + "='DERECHOS SUPERVISION' OR " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Tipo_Pago + "='DER SUP CONVENIO')";

                // filtrar por fecha
                if (Datos.P_Desde_Fecha != DateTime.MinValue)
                {
                    Mi_SQL += " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "."
                        + Ope_Caj_Pagos.Campo_Fecha + ">='" + Datos.P_Desde_Fecha.ToString("dd/MM/yy") + "'";
                }

                // filtrar por fecha
                if (Datos.P_Hasta_Fecha != DateTime.MinValue)
                {
                    Mi_SQL += " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "."
                        + Ope_Caj_Pagos.Campo_Fecha + "<='" + Datos.P_Hasta_Fecha.ToString("dd/MM/yy") + "'";
                }


                //Mi_SQL = Mi_SQL + " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Estatus + "!='CANCELADO' GROUP by ";
                Mi_SQL = Mi_SQL + " AND NOT " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo + " || " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " IN (SELECT PAGOS_CANCELADOS." + Ope_Caj_Pagos.Campo_No_Recibo + " | | PAGOS_CANCELADOS." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " FROM PAGOS_CANCELADOS) GROUP by ";

                Mi_SQL = Mi_SQL + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo + ",";
                Mi_SQL = Mi_SQL + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " ,";
                Mi_SQL = Mi_SQL + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha
                    + ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial
                    + ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID
                    + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Tipo_Pago
                    + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago;

                // agregar ORDER BY, por defecto fecha de pago
                if (string.IsNullOrEmpty(Datos.p_Orden_Dinamico))
                {
                    Mi_SQL += " ORDER BY " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha + " DESC";
                }
                else
                {
                    Mi_SQL += " ORDER BY " + Datos.p_Orden_Dinamico;
                }

                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Historial_Pagos_Fraccionamientos
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene el tipo de Predio
        ///PARAMETROS:     
        ///             1.  Tipo Predio ID 
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 05/Agosto/2011
        ///MODIFICO: Roberto González Oseguera
        ///FECHA_MODIFICO: 28-feb-20120
        ///CAUSA_MODIFICACIÓN: Se agregan descuento a multas y descuento a recargos a la consulta
        ///*******************************************************************************
        public static DataTable Consultar_Pagos_Fraccionamientos(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            DataTable Dt_Traslado_Dominio = new DataTable();
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "";
                Mi_SQL += "WITH PAGOS_CANCELADOS AS (";
                Mi_SQL += "SELECT PAGOS_1." + Ope_Caj_Pagos.Campo_No_Recibo;
                Mi_SQL += " ,PAGOS_1." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID;
                Mi_SQL += " FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " PASIVOS";
                Mi_SQL += " ," + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " PAGOS_1";
                Mi_SQL += " ," + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " PAGOS_2";
                Mi_SQL += " ," + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " CAJA";
                Mi_SQL += " WHERE PAGOS_1." + Ope_Caj_Pagos.Campo_No_Pago + " = PASIVOS." + Ope_Ing_Pasivo.Campo_No_Pago;
                Mi_SQL += " AND PAGOS_1." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " = PAGOS_2." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID;
                Mi_SQL += " AND PAGOS_1." + Ope_Caj_Pagos.Campo_No_Recibo + " = PAGOS_2." + Ope_Caj_Pagos.Campo_No_Recibo;
                Mi_SQL += " AND (PAGOS_1." + Ope_Caj_Pagos.Campo_Caja_ID + " = CAJA." + Cat_Pre_Cajas.Campo_Caja_Id;
                Mi_SQL += "   OR PAGOS_2." + Ope_Caj_Pagos.Campo_Caja_ID + " = CAJA." + Cat_Pre_Cajas.Campo_Caja_Id + ")";
                Mi_SQL += " AND PAGOS_1." + Ope_Caj_Pagos.Campo_Estatus + " = 'PAGADO'";
                Mi_SQL += " AND PAGOS_2." + Ope_Caj_Pagos.Campo_Estatus + " = 'CANCELADO'";
                //Mi_SQL += " AND PAGOS_1." + Ope_Caj_Pagos.Campo_No_Turno + " = '" + Cancelacion.P_No_Turno + "'";
                Mi_SQL += " AND PASIVOS." + Ope_Ing_Pasivo.Campo_Descripcion + " <> 'AJUSTE TARIFARIO'";
                Mi_SQL += ") ";

                Mi_SQL += "SELECT 'FRACCIONAMIENTO', "
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo + ","
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Tipo_Pago + ","
                    + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " as Numero_Caja,"
                    + "REPLACE(TO_CHAR(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha
                    + ", 'dd-Mon-yyyy', 'NLS_LANGUAGE = \"MEXICAN SPANISH\"'),'Dec','Dic') as Fecha, "
                    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ","
                    + " Sum(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Monto_Corriente + ") as Importe,"
                    + " NVL(Sum(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Monto_Recargos + "),0) AS Recargos, "
                    + " NVL(Sum(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Multas + "),0) AS Multas, "
                    + " NVL(SUM(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Descuento_Multas + "),0) as Descuento_Multas, "
                    + " NVL(SUM(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Descuento_Recargos + "),0) as Descuento_Recargos, "
                    + " Sum(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Total + ") AS Total ";
                // agregar propietario de la cuenta si se especifica
                if (Datos.P_Incluir_Propietario == true)
                {
                    Mi_SQL += ",NVL((SELECT "
                        + Ope_Ing_Pasivo.Campo_Contribuyente
                        + " FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo
                        + " WHERE " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Recibo + " = "
                        + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo
                        + " AND " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Pago + " = "
                        + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago
                        + " AND " + Ope_Ing_Pasivo.Campo_Contribuyente + " IS NOT NULL"
                        + " AND ROWNUM=1"
                        + "),(SELECT TRIM("
                       + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || "
                       + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || "
                       + Cat_Pre_Contribuyentes.Campo_Nombre + ")"
                       + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes
                       + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = "
                       + "(SELECT " + Cat_Pre_Propietarios.Campo_Contribuyente_ID
                       + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios
                       + " WHERE " + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + "="
                       + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " AND "
                       + Cat_Pre_Propietarios.Campo_Tipo + " IN ('POSEEDOR','PROPIETARIO') AND ROWNUM = 1)"
                        + ")) NOMBRE_PROPIETARIO";
                }

                Mi_SQL += " FROM " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + " inner join "
                    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " on "
                    + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID + "="
                    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID
                    + " inner join "
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " on "
                    + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID + "="
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " AND "
                    + "'IMP' || TO_CHAR(" + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "."
                    + Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Creo + ", 'yy','NLS_LANGUAGE = \"MEXICAN SPANISH\"') || TO_NUMBER("
                    + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "."
                    + Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + ") = "
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Documento
                    + " inner join "
                    + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " on "
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + "="
                    + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id;
                Mi_SQL += " WHERE "
                    + " (" + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + "='PAGADO'"
                    + " OR " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + "='PARCIAL')";
                // que el pago no esté cancelado
                //Mi_SQL += " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Estatus + "!='CANCELADO' ";
                Mi_SQL += " AND NOT " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo + " || " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " IN (SELECT PAGOS_CANCELADOS." + Ope_Caj_Pagos.Campo_No_Recibo + " | | PAGOS_CANCELADOS." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " FROM PAGOS_CANCELADOS)";

                Mi_SQL += " AND (" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Tipo_Pago
                    + "='FRACCIONAMIENTOS' OR "
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Tipo_Pago + "='FRACC CONVENIO')";

                // si se especifica el id de la cuenta predial agregar filtro
                if (!string.IsNullOrEmpty(Datos.P_Cuenta_Predial_ID))
                {
                    Mi_SQL += " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "."
                        + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + "='" + Datos.P_Cuenta_Predial_ID + "'";
                }

                // filtrar por fecha
                if (Datos.P_Desde_Fecha != DateTime.MinValue)
                {
                    Mi_SQL += " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "."
                        + Ope_Caj_Pagos.Campo_Fecha + ">='" + Datos.P_Desde_Fecha.ToString("dd/MM/yy") + "'";
                }

                // filtrar por fecha
                if (Datos.P_Hasta_Fecha != DateTime.MinValue)
                {
                    Mi_SQL += " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "."
                        + Ope_Caj_Pagos.Campo_Fecha + "<='" + Datos.P_Hasta_Fecha.ToString("dd/MM/yy") + "'";
                }

                // agruopar 
                Mi_SQL += " GROUP BY "
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo + ","
                    + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " ,"
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha
                    + ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial
                    + ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID
                    + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Tipo_Pago
                    + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago;

                // agregar ORDER BY, por defecto fecha de pago
                if (string.IsNullOrEmpty(Datos.p_Orden_Dinamico))
                {
                    Mi_SQL += " ORDER BY " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha + " DESC";
                }
                else
                {
                    Mi_SQL += " ORDER BY " + Datos.p_Orden_Dinamico;
                }

                // ejecutar consulta
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Pagos_Predial_Por_Periodo
        ///DESCRIPCIÓN: Consulta los pagos de predial para una cuenta filtrados por periodo corriente
        ///PARAMETROS:     
        ///             1.  Tipo Predio ID 
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 05/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Pagos_Predial_Por_Periodo(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            DataTable Dt_pagos = new DataTable();
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT "
                    + Ope_Caj_Pagos.Campo_No_Pago
                    + ", " + Ope_Caj_Pagos.Campo_No_Operacion
                    + ", " + Ope_Caj_Pagos.Campo_No_Recibo
                    + ", " + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID
                    + ", " + Ope_Caj_Pagos.Campo_Fecha;
                Mi_SQL += " FROM "
                    + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos
                    + " WHERE ";

                // filtros: cuenta predial
                if (!string.IsNullOrEmpty(Datos.P_Cuenta_Predial_ID))
                {
                    Mi_SQL += Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "' AND ";
                }
                // estatus
                if (!string.IsNullOrEmpty(Datos.p_Estatus))
                {
                    Mi_SQL += Ope_Caj_Pagos.Campo_Estatus + " = '" + Datos.p_Estatus + "' AND ";
                }
                // periodo
                if (!string.IsNullOrEmpty(Datos.p_Periodo_Corriente))
                {
                    Mi_SQL += Ope_Caj_Pagos.Campo_Periodo_Corriente + Datos.p_Periodo_Corriente + " AND ";
                }


                // eliminar AND o WHERE al final de la consulta
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }

                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cuenta_Predial
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene los datos en un
        ///             DataTable.
        ///PARAMETROS:     
        ///             1.  No Cuenta Predial
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 05/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Cuenta_Predial(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {


            DataTable Dt_Traslado_Dominio = new DataTable();

            DataTable Tabla = new DataTable();
            try
            {


                String Mi_SQL = "SELECT * ";

                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;
                Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + "='" + Datos.P_Cuenta_Predial_ID + "'";

                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " DESC ";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }


            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cuenta_Predial
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene los datos en un
        ///             DataTable.
        ///PARAMETROS:     
        ///             1.  No Cuenta Predial
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 05/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Beneficio(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {


            DataTable Dt_Traslado_Dominio = new DataTable();

            DataTable Tabla = new DataTable();
            try
            {


                String Mi_SQL = "SELECT * ";

                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Quitar_Beneficio.Tabla_Ope_Pre_Quitar_Beneficio;
                Mi_SQL = Mi_SQL + " Where " + Ope_Pre_Quitar_Beneficio.Campo_Cuenta_Predial_ID + "='" + Datos.P_Cuenta_Predial_ID + "'";

                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " DESC ";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }


            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tipo_Predial
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene el tipo de Predio
        ///PARAMETROS:     
        ///             1.  Tipo Predio ID 
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 05/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Tipo_Predio(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            DataTable Dt_Traslado_Dominio = new DataTable();
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Tipos_Predio.Campo_Descripcion;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio;
                Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + "='" + Datos.P_Tipo_Predio + "'";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Estado_Predio
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene el Estado de Predio
        ///PARAMETROS:     
        ///             1.  Estado Predio ID 
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 05/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Estado_Predio(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            DataTable Dt_Traslado_Dominio = new DataTable();
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Estados_Predio.Campo_Descripcion;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Estados_Predio.Tabla_Cat_Pre_Estados_Predio;
                Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Estados_Predio.Campo_Estado_Predio_ID + "='" + Datos.P_Estado_Predio + "'";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Estado_Predio_Propietario
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene el Estado de Predio
        ///PARAMETROS:     
        ///             1.  Estado Predio ID 
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 05/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Estado_Predio_Propietario(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            DataTable Dt_Traslado_Dominio = new DataTable();
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Estados.Campo_Nombre + " As Descripcion";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Estados.Tabla_Cat_Pre_Estados;
                Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Estados.Campo_Estado_ID + "='" + Datos.P_Estado_Predio + "'";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Propietario
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene el Tipo Propietario
        ///PARAMETROS:     
        ///             1.  Propietario ID 
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 06/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Propietario(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            DataTable Dt_Traslado_Dominio = new DataTable();
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT *";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios;
                Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + "='" + Datos.P_Propietario_ID + "'";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Propietario
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene el Tipo Propietario
        ///PARAMETROS:     
        ///             1.  Propietario ID 
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 06/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Contribuyentes(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            DataTable Dt_Traslado_Dominio = new DataTable();
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT *";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes;
                Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + "='" + Datos.P_Contribuyente_ID + "'";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Calle_Generales
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene los datos en un
        ///             DataTable.
        ///PARAMETROS:     
        ///             1.  No Calle ID
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 08/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Ciudad(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            DataTable Dt_Traslado_Dominio = new DataTable();
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT * ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades;
                Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Ciudades.Campo_Ciudad_ID + "='" + Datos.P_Ciudad_ID + "'";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Calle_Generales
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene los datos en un
        ///             DataTable.
        ///PARAMETROS:     
        ///             1.  No Calle ID
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 08/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Calle_Generales(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            DataTable Dt_Traslado_Dominio = new DataTable();
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT * ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles;
                Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Calles.Campo_Calle_ID + "='" + Datos.P_Calle_ID + "'";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Calle_Generales
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene los datos en un
        ///             DataTable.
        ///PARAMETROS:     
        ///             1.  No Calle ID
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 08/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Colonia_Generales(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            DataTable Dt_Traslado_Dominio = new DataTable();
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT * ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
                Mi_SQL = Mi_SQL + " Where " + Cat_Ate_Colonias.Campo_Colonia_ID + "='" + Datos.P_Colonia_ID + "'";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Copropietarios
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene los datos en un
        ///             DataTable.
        ///PARAMETROS:     
        ///             1.  Cuenta Predial ID
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 08/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Copropietarios(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            DataTable Dt_Traslado_Dominio = new DataTable();
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT * ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios;
                Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + "='" + Datos.P_Propietario_ID + "'";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Uso_Predio
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene los datos en un
        ///             DataTable.
        ///PARAMETROS:     
        ///             1.  Cuenta Predial ID
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 11/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Uso_Predio(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            DataTable Dt_Traslado_Dominio = new DataTable();
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT * ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Uso_Suelo.Tabla_Cat_Pre_Uso_Suelo;
                Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Uso_Suelo.Campo_Uso_Suelo_ID + "='" + Datos.P_Uso_Suelo_ID + "'";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }


        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Datos_Cuenta_Impuestos
        ///DESCRIPCIÓN: se consultan los datos generales de la cuenta predial
        ///PARAMETROS: 
        ///CREO: Christian Perez
        ///FECHA_CREO: 08/15/2011 11:49:25 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        internal static DataSet Consulta_Datos_Cuenta_Impuestos(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL            
            DataSet Ds_Resultado = new DataSet();
            try
            {
                Mi_SQL = "";
                Mi_SQL += "SELECT ";
                Mi_SQL += " CONT." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " AS CONTRARECIBO, ";
                Mi_SQL += " CONT." + Ope_Pre_Contrarecibos.Campo_Cuenta_Predial_ID + " AS ID, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " AS CTA_PREDIAL_ID, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " AS NO_CUENTA_PREDIAL, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen + " AS CUENTA_ORIGEN, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + " AS CUOTA_FIJA, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + " AS TIPO_PREDIO_ID, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID + " AS USO_SUELO_ID, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID + " AS ESTADO_PREDIO_ID, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Estatus + " AS ESTATUS, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + " AS SUPERFICIE_CONSTRUIDA, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + " AS SUPERFICIE_TOTAL, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + " AS CALLE_ID, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + " AS NO_EXTERIOR, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + " AS NO_INTERIOR, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral + " AS CLAVE_CATASTRAL, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Efectos + " AS EFECTOS, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + " AS VALOR_FISCAL, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente + " AS PERIODO_CORRIENTE, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + " AS CUOTA_ANUAL, ";
                Mi_SQL += " TRUNC(CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + "/6,5) AS CUOTA_BIMESTRAL, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion + " AS DIFERENCIA_CONSTRUCCION, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + " AS PORCENTAJE_EXCENCION, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo + " AS FECHA_AVALUO, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + " AS TERMINO_EXCENCION, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + " AS PORCENTAJE_EXCENCION, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Costo_m2 + " AS COSTO_M2, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + " AS CUOTA_FIJA_ID, ";
                Mi_SQL += " CALLES." + Cat_Pre_Calles.Campo_Calle_ID + " AS CALLE_ID, ";
                Mi_SQL += " CALLES." + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE, ";
                Mi_SQL += " CALLES." + Cat_Pre_Calles.Campo_Colonia_ID + " AS COLONIA_ID, ";
                Mi_SQL += " COL." + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA, ";
                Mi_SQL += " CUOTA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + " AS CUOTA_MINIMA_ID, ";
                Mi_SQL += " CUOTA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota + " AS CUOTA ";

                Mi_SQL += " FROM " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + " CONT LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUEN ON CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                Mi_SQL += " = CONT." + Ope_Pre_Contrarecibos.Campo_Cuenta_Predial_ID;

                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES ON CALLES." + Cat_Pre_Calles.Campo_Calle_ID + " = CUEN.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_ID;
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COL ON COL." + Cat_Ate_Colonias.Campo_Colonia_ID;
                Mi_SQL += " = CALLES." + Cat_Pre_Calles.Campo_Colonia_ID + " LEFT OUTER JOIN " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas + " CUOTA ON ";
                Mi_SQL += " CUOTA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + " = ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID + " ";

                //if (Datos.P_Contrarecibo != "" && Datos.P_Contrarecibo != null)
                //{
                //    Mi_SQL += " WHERE CONT." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " = '" + Datos.P_Contrarecibo + "'";
                //}

                if (Datos.P_Cuenta_Predial_ID != "" && Datos.P_Cuenta_Predial_ID != null)
                {
                    Mi_SQL += " WHERE CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Datos.P_Cuenta_Predial_ID + "'";
                }
                if ((Datos.P_Cuenta_Predial_ID != "" && Datos.P_Cuenta_Predial_ID != null))
                {
                    DataTable Dt_Temporal = new DataTable();
                    DataSet Ds_Temporal = new DataSet();
                    Ds_Temporal = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    Dt_Temporal = Ds_Temporal.Tables[0];
                    Ds_Resultado.Tables.Add(Dt_Temporal.Copy());
                }
                else
                {
                    throw new Exception("No se obtuvieron campos para especificar la búsqueda");
                }
                Ds_Resultado.Tables[0].TableName = "Dt_Generales";



                return Ds_Resultado;

            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Mi_SQL + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Historial_Movimientos
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene los datos en un
        ///             DataTable.
        ///PARAMETROS:     
        ///             1.  Cuenta Predial ID
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 08/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Historial_Movimientos(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT  ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Nota + ",";
                Mi_SQL = Mi_SQL + Cat_Pre_Grupos_Movimiento.Tabla_Cat_Pre_Grupos_Movimiento + "." + Cat_Pre_Grupos_Movimiento.Campo_Nombre + " as Grupo_Movimiento,";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + " as Fecha,To_Number(";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ") as Movimiento_ID,";
                Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Identificador + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + ", NVL(";
                Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + "." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + ",'DIRECTA') AS NO_CONTRARECIBO, NVL(";
                Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + "." + Ope_Pre_Contrarecibos.Campo_Estatus + ",'DIRECTA') AS ESTATUS_CONTRARECIBO ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Grupos_Movimiento.Tabla_Cat_Pre_Grupos_Movimiento + " LEFT OUTER join ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " on ";
                Mi_SQL = Mi_SQL + Cat_Pre_Grupos_Movimiento.Tabla_Cat_Pre_Grupos_Movimiento + "." + Cat_Pre_Grupos_Movimiento.Campo_Grupo_Movimiento_ID + "=";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Grupo_Movimiento_ID + " LEFT OUTER join ";
                Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + " on ";
                Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + "." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + "=";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo;
                //Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + "." + Ope_Pre_Contrarecibos.Campo_Estatus + " = 'PAGADO' LEFT OUTER JOIN ";
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " on ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + "=";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " LEFT OUTER join ";
                Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " on ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + " = ";
                Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Movimiento_ID;
                Mi_SQL = Mi_SQL + " Where (NOT " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " IN ('CANCELADA')) ";
                if (Datos.P_Validar_Contrarecibos_Pagados)
                {
                    //Mi_SQL = Mi_SQL + " AND NOT " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + " IS NULL ";
                    //Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + "." + Ope_Pre_Contrarecibos.Campo_Estatus + " = 'PAGADO' ";
                }
                //Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + "." +
                //        Ope_Pre_Contrarecibos.Campo_Estatus + " IS NULL ";
                //Mi_SQL = Mi_SQL + "OR (NOT " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." +
                //         Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " IN ('CANCELADA') AND ";
                //Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + "." +
                //        Ope_Pre_Contrarecibos.Campo_Estatus + "='PAGADO')) ";
                if (!string.IsNullOrEmpty(Datos.P_Cuenta_Predial))
                {
                    if (Mi_SQL.Contains("Where"))
                    {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + "='" + Datos.P_Cuenta_Predial + "'";
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + "='" + Datos.P_Cuenta_Predial + "'";
                    }
                }
                Mi_SQL = Mi_SQL + " Order By " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Modifico + " DESC,";
                Mi_SQL = Mi_SQL + " " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " DESC";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]";
                //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Convenios
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos los convenios de cuenta preedial
        ///PARAMETROS:     
        ///             1.  Cuenta Predial
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 17/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Convenios(Cls_Ope_Pre_Resumen_Predio_Negocio Datos)
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + "'CDER'" + "||''|| TO_NUMBER(" +
                         Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "." +
                         Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio + ") AS No_Convenio,";
                Mi_SQL = Mi_SQL + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision +
                         "." + Ope_Pre_Convenios_Derechos_Supervision.Campo_Total_Convenio + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision +
                         "." + Ope_Pre_Convenios_Derechos_Supervision.Campo_Fecha + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision +
                         "." + Ope_Pre_Convenios_Derechos_Supervision.Campo_Numero_Parcialidades + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision +
                         "." + Ope_Pre_Convenios_Derechos_Supervision.Campo_Estatus + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision +
                         "." + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Impuesto_Dereho_Supervisio +
                         " as No_Impuesto ";
                Mi_SQL = Mi_SQL + " FROM " +
                         Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision +
                         " inner join ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " on ";
                Mi_SQL = Mi_SQL + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision +
                         "." + Ope_Pre_Convenios_Derechos_Supervision.Campo_Cuenta_Predial_ID + "=";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." +
                         Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " ";
                Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." +
                         Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + "='" + Datos.P_Cuenta_Predial + "'";
                Mi_SQL = Mi_SQL + " union ";
                Mi_SQL = Mi_SQL + "SELECT ";
                Mi_SQL = Mi_SQL + "'CFRA'" + "||''|| TO_NUMBER(" +
                         Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "." +
                         Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + ") AS No_Convenio,";
                Mi_SQL = Mi_SQL + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "." +
                         Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Convenio + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "." +
                         Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "." +
                         Ope_Pre_Convenios_Fraccionamientos.Campo_Numero_Parcialidades + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "." +
                         Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "." +
                         Ope_Pre_Convenios_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + " as No_Impuesto ";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos +
                         " inner join ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " on ";
                Mi_SQL = Mi_SQL + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "." +
                         Ope_Pre_Convenios_Fraccionamientos.Campo_Cuenta_Predial_ID + "=";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." +
                         Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " ";
                Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." +
                         Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + "='" + Datos.P_Cuenta_Predial + "'";
                Mi_SQL = Mi_SQL + " union ";
                Mi_SQL = Mi_SQL + "SELECT ";
                Mi_SQL = Mi_SQL + "'CTRA'" + "||''|| TO_NUMBER(" +
                         Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." +
                         Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio + ") AS No_Convenio,";
                Mi_SQL = Mi_SQL + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." +
                         Ope_Pre_Convenios_Traslados_Dominio.Campo_Total_Convenio + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." +
                         Ope_Pre_Convenios_Traslados_Dominio.Campo_Fecha + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." +
                         Ope_Pre_Convenios_Traslados_Dominio.Campo_Numero_Parcialidades + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." +
                         Ope_Pre_Convenios_Traslados_Dominio.Campo_Estatus + ", NULL as No_Impuesto";
                Mi_SQL = Mi_SQL + " FROM " +
                         Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + " inner join ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " on ";
                Mi_SQL = Mi_SQL + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." +
                         Ope_Pre_Convenios_Traslados_Dominio.Campo_Cuenta_Predial_ID + "=";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." +
                         Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " ";
                Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." +
                         Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + "='" + Datos.P_Cuenta_Predial + "'";
                Mi_SQL = Mi_SQL + " union ";
                Mi_SQL = Mi_SQL + "SELECT ";
                Mi_SQL = Mi_SQL + "'CPRE'" + "||''|| TO_NUMBER(" +
                         Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." +
                         Ope_Pre_Convenios_Predial.Campo_No_Convenio + ") AS No_Convenio,";
                Mi_SQL = Mi_SQL + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." +
                         Ope_Pre_Convenios_Predial.Campo_Total_Convenio + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." +
                         Ope_Pre_Convenios_Predial.Campo_Fecha + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." +
                         Ope_Pre_Convenios_Predial.Campo_Numero_Parcialidades + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." +
                         Ope_Pre_Convenios_Predial.Campo_Estatus + ", NULL as No_Impuesto";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + " inner join ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " on ";
                Mi_SQL = Mi_SQL + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." +
                         Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id + "=";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." +
                         Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " ";
                Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." +
                         Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + "='" + Datos.P_Cuenta_Predial + "'";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]";
                //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        #endregion


    }
}