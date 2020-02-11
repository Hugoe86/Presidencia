using System;
using System.Data;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using System.Text;
using Presidencia.Reportes_Nomina_Control_Archivos_Dispersion.Negocio;

namespace Presidencia.Reportes_Nomina_Control_Archivos_Dispersion.Datos
{
    public class Cls_Rpt_Nom_Control_Archivos_Dispersion_Datos
    {
        #region METODOS

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Depositos_Tipo_Nomina
        /// DESCRIPCIÓN: Genera la consulta del catálogo de empleados de nómina con filtros por empleado, nómina y unidad responsable
        /// PARÁMETROS:
        /// 		1. Empleados_Negocio: isntancia de la capa de negocio con fitros para la consulta
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 05-abr-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        internal static DataTable Consultar_Depositos_Tipo_Nomina(Cls_Rpt_Nom_Control_Archivos_Dispersion_Negocio Empleados_Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT TO_NUMBER(" + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + ") " + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);
                Mi_Sql.Append("," + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina);
                Mi_Sql.Append(",NVL(COUNT(" + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Percepciones + "),0) CANTIDAD_DEPOSITOS");
                Mi_Sql.Append(",NVL(SUM(" + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Percepciones + "),0) " + Ope_Nom_Recibos_Empleados.Campo_Total_Percepciones);
                Mi_Sql.Append("," + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Nombre);
                Mi_Sql.Append("," + Cat_Nom_Tipos_Pagos.Tabla_Cat_Nom_Tipos_Pagos + "." + Cat_Nom_Tipos_Pagos.Campo_Nombre + " TIPO_PAGO");

                Mi_Sql.Append(" FROM " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " ON "
                    + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Tipo_Nomina_ID + "="
                    + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " ON "
                    + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + "="
                        + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                Mi_Sql.Append(" LEFT JOIN " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + " ON "
                    + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Banco_ID + "="
                        + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Banco_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Nom_Tipos_Pagos.Tabla_Cat_Nom_Tipos_Pagos + " ON "
                    + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Forma_Pago + "="
                        + Cat_Nom_Tipos_Pagos.Tabla_Cat_Nom_Tipos_Pagos + "." + Cat_Nom_Tipos_Pagos.Campo_Tipo_Pago_ID + " WHERE ");
               
                //filtro por tipo de nomina
                if (!String.IsNullOrEmpty(Empleados_Negocio.P_Nomina_ID))
                {
                    Mi_Sql.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + " = '" + Empleados_Negocio.P_Nomina_ID + "' AND ");
                }

                //filtro por tipo de nomina
                if (!String.IsNullOrEmpty(Empleados_Negocio.P_Numero_Nomina))
                {
                    Mi_Sql.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + " = '" + Empleados_Negocio.P_Numero_Nomina + "' AND ");
                }

                // eliminar AND o WHERE del final del string
                if (Mi_Sql.ToString().EndsWith(" AND "))
                {
                    Mi_Sql.Remove(Mi_Sql.Length - 5, 5);
                }
                else if (Mi_Sql.ToString().EndsWith(" WHERE "))
                {
                    Mi_Sql.Remove(Mi_Sql.Length - 7, 7);
                }

                // agregar GROUP BY a la consulta
                Mi_Sql.Append(" GROUP BY " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);
                Mi_Sql.Append("," + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Nombre);
                Mi_Sql.Append("," + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina);
                Mi_Sql.Append("," + Cat_Nom_Tipos_Pagos.Tabla_Cat_Nom_Tipos_Pagos + "." + Cat_Nom_Tipos_Pagos.Campo_Nombre);
                
                Mi_Sql.Append(" ORDER BY " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Nombre
                    + ", " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros del reporte del catálogo de empleados. Error: [" + Ex.Message + "]");
            }
        }

        #endregion
    }
}
