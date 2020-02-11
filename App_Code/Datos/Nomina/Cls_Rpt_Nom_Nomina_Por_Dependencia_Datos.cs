using System;
using System.Data;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using System.Text;
using Presidencia.Reportes_Nomina_Nomina_Por_Dependencia.Negocio;

namespace Presidencia.Reportes_Nomina_Nomina_Por_Dependencia.Datos
{
    public class Cls_Rpt_Nom_Nomina_Por_Dependencia_Datos
    {
        #region METODOS

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Nomina
        /// DESCRIPCIÓN: Genera la consulta de percepciones y deducciones con filtros por nómina, tipo de nómina y unidad responsable
        /// PARÁMETROS:
        /// 		1. Datos: instancia de la capa de negocio con fitros para la consulta
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 09-abr-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        internal static DataTable Consultar_Nomina(Cls_Rpt_Nom_Nomina_Por_Dependencia_Negocio Datos)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave + " CLAVE_PERCEPCION_DEDUCCION");
                Mi_Sql.Append("," + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + " NOMBRE_PERCEPCION_DEDUCCION");
                Mi_Sql.Append("," + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " CLAVE_DEPENDENCIA");
                Mi_Sql.Append("," + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " NOMBRE_DEPENDENCIA");
                Mi_Sql.Append(",TO_NUMBER(" + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + ") " + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);
                Mi_Sql.Append("," + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina);
                Mi_Sql.Append(",SUM(" + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Monto + ") " + Ope_Nom_Recibos_Empleados_Det.Campo_Monto);
                Mi_Sql.Append("," + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Tipo);

                Mi_Sql.Append(" FROM " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + " ON ");
                Mi_Sql.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo + "=");
                Mi_Sql.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Recibo);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " ON ");
                Mi_Sql.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID + "=");
                Mi_Sql.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " ON ");
                Mi_Sql.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Dependencia_ID + "=");
                Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " ON ");
                Mi_Sql.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Tipo_Nomina_ID + "=");
                Mi_Sql.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + " WHERE ");

                // agregar filtros
                //filtro por NOMINA_ID y NO_NOMINA
                if (!String.IsNullOrEmpty(Datos.P_No_Nomina) && !String.IsNullOrEmpty(Datos.P_Nomina_ID))
                {
                    Mi_Sql.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + " = '" + Datos.P_Nomina_ID + "' AND ");
                    Mi_Sql.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + " = " + Datos.P_No_Nomina + " AND ");
                }

                //filtro por DEPENDENCIA
                if (!String.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "' AND ");
                }

                //filtro por TIPO_NOMINA
                if (!String.IsNullOrEmpty(Datos.P_Tipo_Nomina_ID))
                {
                    Mi_Sql.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + " = '" + Datos.P_Tipo_Nomina_ID + "' AND ");
                }

                //filtro por TIPO_PERCEPCION_DEDUCCION
                if (!String.IsNullOrEmpty(Datos.P_Tipo_Percepcion_Deduccion))
                {
                    Mi_Sql.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Tipo + " = '" + Datos.P_Tipo_Percepcion_Deduccion + "' AND ");
                }

                //filtro dinamico
                if (!String.IsNullOrEmpty(Datos.P_Filtro_Dinamico))
                {
                    Mi_Sql.Append(Datos.P_Filtro_Dinamico);
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

                // agregar agrupamiento
                Mi_Sql.Append(" GROUP BY " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Dependencia_ID);
                Mi_Sql.Append("," + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave);
                Mi_Sql.Append("," + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Nombre);
                Mi_Sql.Append("," + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave);
                Mi_Sql.Append("," + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre);
                Mi_Sql.Append("," + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);
                Mi_Sql.Append("," + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina);
                Mi_Sql.Append("," + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Tipo);

                Mi_Sql.Append(" ORDER BY " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave + " ASC");

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
