using System;
using System.Text;

using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Vacaciones_Acumuladas_Por_Empleado.Negocio;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Vacaciones_Acumuladas_Por_Empleado.Negocio;

/// <summary>
/// Summary description for Cls_Rpt_Nom_Vacaciones_Acumuladas_Por_Empleado_Datos
/// </summary>
///
namespace Presidencia.Vacaciones_Acumuladas_Por_Empleado.Datos
{
    public class Cls_Rpt_Nom_Vacaciones_Acumuladas_Por_Empleado_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:  Consultar_Unidades_Responsables
        ///DESCRIPCIÓN: Metodo que Consulta las Unidades Responsables
        ///PARAMETROS: 1.- Cls_Rpt_Nom_Relacion_Recibos_Nomina_Negocio Clase_Negocios, objeto de la clase de negocios
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 09/ABRIL/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Unidades_Responsables()
        {
            StringBuilder Mi_SQL = new StringBuilder();
            Mi_SQL.Append("SELECT " + Cat_Dependencias.Campo_Dependencia_ID + "," + Cat_Dependencias.Campo_Clave);
            Mi_SQL.Append(" ||' '|| " + Cat_Dependencias.Campo_Nombre + " AS " + Cat_Dependencias.Campo_Nombre);
            Mi_SQL.Append(" FROM " + Cat_Dependencias.Tabla_Cat_Dependencias);
            Mi_SQL.Append(" WHERE " + Cat_Dependencias.Campo_Estatus + "='ACTIVO'");
            Mi_SQL.Append(" ORDER BY  " + Cat_Dependencias.Campo_Nombre + " ASC ");


            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];


        }


        public static DataTable Consultar_Tipo_Nomina()
        {
            StringBuilder Mi_SQL = new StringBuilder();
            Mi_SQL.Append("SELECT " + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);
            Mi_SQL.Append(", " + Cat_Nom_Tipos_Nominas.Campo_Nomina);
            Mi_SQL.Append(" FROM " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas);
            Mi_SQL.Append(" ORDER BY " + Cat_Nom_Tipos_Nominas.Campo_Nomina + " ASC");

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];


        }

        public static DataTable Consultar_Empleado(Cls_Rpt_Nom_Vacaciones_Acumuladas_Por_Empleado_Negocio Clase_Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            Mi_SQL.Append("SELECT " + Cat_Empleados.Campo_Empleado_ID);
            Mi_SQL.Append(", " + Cat_Empleados.Campo_Apellido_Paterno);
            Mi_SQL.Append("||' '|| " + Cat_Empleados.Campo_Apellido_Materno);
            Mi_SQL.Append("||' '|| " + Cat_Empleados.Campo_Nombre);
            Mi_SQL.Append(" AS " + Cat_Empleados.Campo_Nombre);
            Mi_SQL.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
            Mi_SQL.Append(" WHERE " + Cat_Empleados.Campo_No_Empleado);
            Mi_SQL.Append("='" + String.Format("{0:000000}", Convert.ToInt32(Clase_Negocio.P_No_Empleado))  +"'");

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

        }

        public static DataTable Consultar_Vacaciones(Cls_Rpt_Nom_Vacaciones_Acumuladas_Por_Empleado_Negocio Clase_Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            Mi_SQL.Append("SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado);
            Mi_SQL.Append(", " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave);
            Mi_SQL.Append("||' '|| " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS UNIDAD_RESPONSABLE");
            Mi_SQL.Append(", " + Ope_Nom_Vacaciones_Empleado_Detalles.Tabla_Ope_Nom_Vacaciones_Empl_Det + "." + Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Anio);
            Mi_SQL.Append(", " + Ope_Nom_Vacaciones_Empleado_Detalles.Tabla_Ope_Nom_Vacaciones_Empl_Det + "." + Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Periodo_Vacacional);
            Mi_SQL.Append(", " + Ope_Nom_Vacaciones_Empleado_Detalles.Tabla_Ope_Nom_Vacaciones_Empl_Det + "." + Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Dias_Disponibles);
            Mi_SQL.Append(", " + Ope_Nom_Vacaciones_Empleado_Detalles.Tabla_Ope_Nom_Vacaciones_Empl_Det + "." + Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Dias_Tomados);
            Mi_SQL.Append(" FROM " + Ope_Nom_Vacaciones_Empleado_Detalles.Tabla_Ope_Nom_Vacaciones_Empl_Det);
            Mi_SQL.Append(", " + Cat_Empleados.Tabla_Cat_Empleados);
            Mi_SQL.Append(", " + Cat_Dependencias.Tabla_Cat_Dependencias);
            Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
            Mi_SQL.Append(" = " + Ope_Nom_Vacaciones_Empleado_Detalles.Tabla_Ope_Nom_Vacaciones_Empl_Det + "." + Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Empleado_ID);
            Mi_SQL.Append(" AND " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
            Mi_SQL.Append("=" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID);
            if(Clase_Negocio.P_Empleado_ID != null)
            {
                Mi_SQL.Append(" AND " + Ope_Nom_Vacaciones_Empleado_Detalles.Tabla_Ope_Nom_Vacaciones_Empl_Det + "." + Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Empleado_ID);
                Mi_SQL.Append("='" + Clase_Negocio.P_Empleado_ID.Trim() + "'");

            }

            if (Clase_Negocio.P_Tipo_Nomina_ID != null)
            {
                Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append("='" + Clase_Negocio.P_Tipo_Nomina_ID.Trim() + "'");

            }

            if (Clase_Negocio.P_Unidad_Responsable_ID != null)
            {
                Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID);
                Mi_SQL.Append("='" + Clase_Negocio.P_Unidad_Responsable_ID.Trim() + "'");

            }

            if (Clase_Negocio.P_Anio != null)
            {
                Mi_SQL.Append(" AND " + Ope_Nom_Vacaciones_Empleado_Detalles.Tabla_Ope_Nom_Vacaciones_Empl_Det + "." + Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Anio);
                Mi_SQL.Append("=" + Clase_Negocio.P_Anio.Trim());
            }
            if (Clase_Negocio.P_Periodo_Vacacional != null)
            {
                Mi_SQL.Append(" AND " + Ope_Nom_Vacaciones_Empleado_Detalles.Tabla_Ope_Nom_Vacaciones_Empl_Det + "." + Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Periodo_Vacacional);
                Mi_SQL.Append("=" + Clase_Negocio.P_Periodo_Vacacional.Trim());
            }

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

        }
    }//fin del class
}