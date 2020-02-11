using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Empleados.Negocios;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;

namespace Presidencia.Aportacion.Datos
{
    public class Cls_Rpt_Nom_Aportacion_Datos
    {
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Aportación 
        /// 
        /// DESCRIPCION : Consulta empleado con, su RFC,No de Empleado, Unidad Responsable,
        ///               Nomina
        ///               
        /// CREO        : Armando Zavala Moreno
        /// FECHA_CREO  : 10/Abril/2012 03:55:00 a.m.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        internal static DataTable Consultar_Aportacion(Cls_Cat_Empleados_Negocios Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Puestos = null;
            Boolean Comprobacion_And = false;

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
                Mi_SQL.Append("NVL((((" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Diario + "*30.42)*.20)/2),0) AS IMPORTE_APORTACION");

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
                
                if (!String.IsNullOrEmpty(Datos.P_Estatus))
                {
                    if (Comprobacion_And == true)
                    {
                        Mi_SQL.Append(" AND ");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE ");
                    }
                    Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Estatus + "='" + Datos.P_Estatus + "'");
                    Comprobacion_And = true;
                }

                if (!String.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    if (Comprobacion_And == true)
                    {
                        Mi_SQL.Append(" AND ");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE ");
                    }
                    Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'");
                    Comprobacion_And = true;
                }

                if(!String.IsNullOrEmpty(Datos.P_Tipo_Nomina_ID))
                {
                    if (Comprobacion_And == true)
                    {
                        Mi_SQL.Append(" AND ");                        
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE ");
                    }
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID + "='" + Datos.P_Tipo_Nomina_ID + "'");
                    Comprobacion_And = true;
                }

                if (!String.IsNullOrEmpty(Datos.P_Nombre))
                {
                    if (Comprobacion_And == true)
                    {
                        Mi_SQL.Append(" AND ");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE ");
                    }
                    Mi_SQL.Append("(UPPER(" + Cat_Empleados.Tabla_Cat_Empleados + "." +Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Tabla_Cat_Empleados + "." +Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                             Cat_Empleados.Tabla_Cat_Empleados + "." +Cat_Empleados.Campo_Apellido_Materno + ") LIKE UPPER('%" + Datos.P_Nombre + "%')");
                    Mi_SQL.Append(" OR UPPER(" + Cat_Empleados.Tabla_Cat_Empleados + "." +Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                             Cat_Empleados.Tabla_Cat_Empleados + "." +Cat_Empleados.Campo_Apellido_Materno + "||' '||" + Cat_Empleados.Tabla_Cat_Empleados + "." +Cat_Empleados.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Nombre + "%'))");
                }
                if (!String.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    if (Comprobacion_And == true)
                    {
                        Mi_SQL.Append(" AND ");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE ");
                    }
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." +Cat_Empleados.Campo_No_Empleado+"='"+Datos.P_No_Empleado+"'");
                }

                Dt_Puestos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los puestos que le pertencen a la dependencia. Error: [" + Ex.Message + "]");
            }
            return Dt_Puestos;
        }
    }
}
