using System;
using System.Data;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Pae_Cuentas_Ingresadas.Negocio;
using SharpContent.ApplicationBlocks.Data;

namespace Presidencia.Operacion_Predial_Pae_Cuentas_Ingresadas.Datos
{
    public class Cls_Ope_Pre_Pae_Cuentas_Ingresadas_Datos
    {

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consulta_Fechas_Etapas
        /// DESCRIPCIÓN: Forma y ejecuta una consulta de las fechas de ingreso de cuentas a etapas del PAE
        /// PARÁMETROS:
        /// 		1. Filtros: instancia de la clase de negocio con filtros para la consulta
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 10-may-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Fechas_Etapas(Cls_Ope_Pre_Pae_Cuentas_Ingresadas_Negocio Filtros)
        {
            DataTable Tabla = null;
            String Mi_SQL;

            try
            {
                Mi_SQL = "WITH FECHAS AS (SELECT TO_CHAR("
                    + Ope_Pre_Pae_Detalles_Cuentas.Campo_Fecha_Proceso_Cambio + ", 'dd-Mon-yyyy', 'NLS_LANGUAGE = \"MEXICAN SPANISH\"') FECHA"
                    + ",ROW_NUMBER() OVER (PARTITION BY " + Ope_Pre_Pae_Detalles_Cuentas.Campo_No_Detalle_Etapa     // consulta que enumera por detalle_etapa
                    + " ORDER BY CASE " + Ope_Pre_Pae_Detalles_Cuentas.Campo_Proceso_Actual
                    + " WHEN 'DETERMINACION' THEN 1 WHEN 'REQUERIMIENTO' THEN 2 WHEN 'EMBARGO' THEN 3 WHEN 'REMOCION' THEN 4 WHEN 'ALMONEDA' THEN 5 END) NUMERO_FILA"
                    + " FROM " + Ope_Pre_Pae_Detalles_Cuentas.Tabla_Ope_Pre_Pae_Detalles_Cuentas + " WHERE ";

                // filtro por FECHA
                if (Filtros.P_Fecha_Ingreso != DateTime.MinValue)
                {
                    Mi_SQL += "TO_CHAR(" + Ope_Pre_Pae_Detalles_Cuentas.Campo_Fecha_Proceso_Cambio + ", 'd-M-yyyy)' = '" + Filtros.P_Fecha_Ingreso.ToString("d-M-yyyy") + "' AND ";
                }

                // eliminar AND o WHERE al final de la consulta
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                else if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }

                // terminar consulta del WITH
                Mi_SQL += ") SELECT DISTINCT(FECHA) " + Ope_Pre_Pae_Detalles_Cuentas.Campo_Fecha_Proceso_Cambio + " FROM FECHAS WHERE NUMERO_FILA = 1";

                // ORDER BY
                if (!string.IsNullOrEmpty(Filtros.P_Ordenar_Dinamico))
                {
                    Mi_SQL += " ORDER BY " + Filtros.P_Ordenar_Dinamico;
                }

                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null)
                {
                    Tabla = dataSet.Tables[0];
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
        /// NOMBRE_FUNCIÓN: Consulta_Cuentas_Ingresadas
        /// DESCRIPCIÓN: Forma y ejecuta una consulta de las cuentas en etapas del PAE
        /// PARÁMETROS:
        /// 		1. Filtros: instancia de la clase de negocio con filtros para la consulta
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 10-may-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Cuentas_Ingresadas(Cls_Ope_Pre_Pae_Cuentas_Ingresadas_Negocio Filtros)
        {
            DataTable Tabla = null;
            String Mi_SQL;

            try
            {
                Mi_SQL = "WITH DETALLES_CUENTAS AS (SELECT "
                    + Ope_Pre_Pae_Detalles_Cuentas.Campo_No_Detalle_Cuenta
                    + "," + Ope_Pre_Pae_Detalles_Cuentas.Campo_No_Detalle_Etapa
                    + "," + Ope_Pre_Pae_Detalles_Cuentas.Campo_Fecha_Proceso_Cambio
                    + "," + Ope_Pre_Pae_Detalles_Cuentas.Campo_Proceso_Anterior
                    + "," + Ope_Pre_Pae_Detalles_Cuentas.Campo_Proceso_Actual
                    + ",ROW_NUMBER() OVER (PARTITION BY " + Ope_Pre_Pae_Detalles_Cuentas.Campo_No_Detalle_Etapa
                    + " ORDER BY CASE " + Ope_Pre_Pae_Detalles_Cuentas.Campo_Proceso_Actual
                    + " WHEN 'DETERMINACION' THEN 1 WHEN 'REQUERIMIENTO' THEN 2 WHEN 'EMBARGO' THEN 3 WHEN 'REMOCION' THEN 4 WHEN 'ALMONEDA' THEN 5 END) NUMERO_FILA"
                    + " FROM " + Ope_Pre_Pae_Detalles_Cuentas.Tabla_Ope_Pre_Pae_Detalles_Cuentas + ")";

                Mi_SQL += " SELECT DETALLES_CUENTAS." + Ope_Pre_Pae_Detalles_Cuentas.Campo_Fecha_Proceso_Cambio
                    + "," + Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id
                    + "," + Ope_Pre_Pae_Det_Etapas.Campo_Periodo_Corriente
                    + "," + Ope_Pre_Pae_Det_Etapas.Campo_Periodo_Rezago
                    + "," + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Corriente
                    + "," + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Rezago
                    + "," + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Ordinarios
                    + "," + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Moratorios
                    + "," + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Honorarios
                    + "," + Ope_Pre_Pae_Det_Etapas.Campo_Multas
                    + "," + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Total
                    + "," + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa
                    + "," + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Proceso_Actual
                    + "," + Ope_Pre_Pae_Det_Etapas.Campo_Estatus
                    + ",NVL(" + Ope_Pre_Pae_Det_Etapas.Campo_Omitida + ",'NO') " + Ope_Pre_Pae_Det_Etapas.Campo_Omitida
                    + "," + Ope_Pre_Pae_Det_Etapas.Campo_Motivo_Omision
                    + "," + Ope_Pre_Pae_Det_Etapas.Campo_Folio
                    + "," + Ope_Pre_Pae_Det_Etapas.Campo_Impresa
                    + ", (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM " // subconsulta de cuenta predial
                    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE "
                    + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "="
                    + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id
                    + ") " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial
                    + ", (SELECT " + Ope_Pre_Pae_Etapas.Campo_Numero_Entrega + " FROM " // subconsulta de numero de entrega
                    + Ope_Pre_Pae_Etapas.Tabla_Ope_Pre_Pae_Etapas + " WHERE "
                    + Ope_Pre_Pae_Etapas.Campo_No_Etapa + "="
                    + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Etapa
                    + ") " + Ope_Pre_Pae_Etapas.Campo_Numero_Entrega
                    + ", (SELECT " + Cat_Pre_Despachos_Externos.Campo_Despacho + " FROM " 
                    + Cat_Pre_Despachos_Externos.Tabla_Cat_Pre_Despachos_Externos + " WHERE " 
                    + Cat_Pre_Despachos_Externos.Campo_Despacho_Id + " = "
                    + "(SELECT " + Ope_Pre_Pae_Etapas.Campo_Despacho_Id + " FROM " // subconsulta de numero de entrega
                    + Ope_Pre_Pae_Etapas.Tabla_Ope_Pre_Pae_Etapas + " WHERE "
                    + Ope_Pre_Pae_Etapas.Campo_No_Etapa + "="
                    + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Etapa
                    + ")) " + Cat_Pre_Despachos_Externos.Campo_Despacho
                    + " FROM " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas
                    + " LEFT JOIN DETALLES_CUENTAS ON "
                    + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + "="
                    + "DETALLES_CUENTAS." + Ope_Pre_Pae_Detalles_Cuentas.Campo_No_Detalle_Etapa
                    + " AND DETALLES_CUENTAS.NUMERO_FILA=1";

                Mi_SQL += " WHERE ";

                // filtro por ETAPA
                if (!string.IsNullOrEmpty(Filtros.P_Proceso_Actual))
                {
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Proceso_Actual + "= '" + Filtros.P_Proceso_Actual + "' AND ";
                }

                // filtro por FECHA INGRESO
                if (Filtros.P_Fecha_Ingreso != DateTime.MinValue)
                {
                    Mi_SQL += " TO_CHAR(DETALLES_CUENTAS." + Ope_Pre_Pae_Detalles_Cuentas.Campo_Fecha_Proceso_Cambio + ", 'DD-MM-YYYY') = '" + Filtros.P_Fecha_Ingreso.ToString("dd-MM-yyyy") + "' AND ";
                }

                // filtro por FECHA INICIAL
                if (Filtros.P_Fecha_Inicial != DateTime.MinValue)
                {
                    Mi_SQL += " DETALLES_CUENTAS." + Ope_Pre_Pae_Detalles_Cuentas.Campo_Fecha_Proceso_Cambio + " >= '" + Filtros.P_Fecha_Inicial.ToString("dd-MM-yyyy") + "' AND ";
                }

                // filtro por FECHA FINAL
                if (Filtros.P_Fecha_Final != DateTime.MinValue)
                {
                    Mi_SQL += " DETALLES_CUENTAS." + Ope_Pre_Pae_Detalles_Cuentas.Campo_Fecha_Proceso_Cambio + " <= '" + Filtros.P_Fecha_Final.ToString("dd-MM-yyyy") + "' AND ";
                }

                // filtro por FOLIO
                if (!string.IsNullOrEmpty(Filtros.P_Folio))
                {
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Campo_Folio + "= '" + Filtros.P_Folio + "' ";
                }

                // eliminar AND o WHERE al final de la consulta
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                else if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }

                // ORDER BY
                if (!string.IsNullOrEmpty(Filtros.P_Ordenar_Dinamico))
                {
                    Mi_SQL += " ORDER BY " + Filtros.P_Ordenar_Dinamico;
                }
                else
                {
                    Mi_SQL += " ORDER BY " + Ope_Pre_Pae_Detalles_Cuentas.Campo_Fecha_Proceso_Cambio + " DESC";
                }

                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null)
                {
                    Tabla = dataSet.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de la Cuentas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

    }
}