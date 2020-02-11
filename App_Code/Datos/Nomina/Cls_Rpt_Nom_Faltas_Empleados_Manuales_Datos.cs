using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Data.OracleClient;
using System.Text;
using Presidencia.Faltas_Empleados_Manuales.Negocio;
using SharpContent.ApplicationBlocks.Data;

namespace Presidencia.Faltas_Empleados_Manuales.Datos
{
    public class Cls_Rpt_Nom_Faltas_Empleados_Manuales_Datos
    {
        /// ********************************************************************************************************************
        /// NOMBRE: Consultar_Faltas
        /// DESCRIPCIÓN: Consulta las faltas que existen registradas actualmente en el sistema de manera manual.
        /// PARÁMETROS: Datos:
        /// USUARIO CREÓ: Armando Zavala Moreno.
        /// FECHA CREÓ: 12/Abril/2012 06:30 pm.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACION:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Faltas(Cls_Rpt_Nom_Faltas_Empleados_Manuales_Negocio Datos)
        {
            DataTable Dt_Faltas_Empleados = null;//Variable que almacenara una lista de las antiguedades que evaluaran los sindicatos.
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append("NVL(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ",' ') AS NO_EMPLEADO, ");
                Mi_SQL.Append("NVL(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + ",' ') AS RFC, ");
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS EMPLEADO, ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Nombre + " AS PUESTO, ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA, ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina + " AS NOMINA, ");
                Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Fecha);

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " ON ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Puestos.Tabla_Cat_Puestos + " ON ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Puesto_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " ON ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Empleado_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Empleado_ID);

                Mi_SQL.Append(" WHERE " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Comentarios);
                Mi_SQL.Append(" IS NULL AND " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Retardo + "='NO'");

                if (!String.IsNullOrEmpty(Datos.P_Tipo_Nomina))
                {
                    Mi_SQL.Append(" AND " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + "=" + Datos.P_Tipo_Nomina);
                }
                if (!String.IsNullOrEmpty(Datos.P_Dependencia))
                {
                    Mi_SQL.Append(" AND " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "=" + Datos.P_Dependencia);
                }
                if (!String.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "=" + Datos.P_No_Empleado);
                }
                if (!String.IsNullOrEmpty(Datos.P_RFC))
                {
                    Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC+"'");
                }
                if (!String.IsNullOrEmpty(Datos.P_Empleado))
                {
                    Mi_SQL.Append(" AND (UPPER(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                             Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ") LIKE UPPER('%" + Datos.P_Empleado + "%')");
                    Mi_SQL.Append(" OR UPPER(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                             Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "||' '||" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Empleado + "%'))");
                }
                if (!String.IsNullOrEmpty(Datos.P_Año))
                {
                    Mi_SQL.Append(" AND " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Nomina_ID + "=" + Datos.P_Año);
                }
                if (!String.IsNullOrEmpty(Datos.P_Periodo))
                {
                    Mi_SQL.Append(" AND " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_No_Nomina + "=" + Datos.P_Periodo);
                }
                Mi_SQL.Append(" ORDER BY DEPENDENCIA, " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Fecha+", PUESTO");
                Dt_Faltas_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los puestos que le pertencen a la dependencia. Error: [" + Ex.Message + "]");
            }
            return Dt_Faltas_Empleados;
        }
    }
}
