using System;
using System.Text;

using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Relacion_Recibos_Nomina.Negocio;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;

/// <summary>
/// Summary description for Cls_Rpt_Nom_Relacion_Recibos_Nomina_Datos
/// </summary>
/// 
namespace Presidencia.Relacion_Recibos_Nomina.Datos
{
    public class Cls_Rpt_Nom_Relacion_Recibos_Nomina_Datos
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
        public static DataTable Consultar_Unidades_Responsables(Cls_Rpt_Nom_Relacion_Recibos_Nomina_Negocio Clase_Negocios)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            Mi_SQL.Append("SELECT " +Cat_Dependencias.Campo_Dependencia_ID  + "," + Cat_Dependencias.Campo_Clave);
            Mi_SQL.Append(" ||' '|| " + Cat_Dependencias.Campo_Nombre + " AS " + Cat_Dependencias.Campo_Nombre);
            Mi_SQL.Append(" FROM " + Cat_Dependencias.Tabla_Cat_Dependencias);
            Mi_SQL.Append(" WHERE " + Cat_Dependencias.Campo_Estatus + "='ACTIVO'");
            Mi_SQL.Append(" ORDER BY  " + Cat_Dependencias.Campo_Nombre + " ASC ");


            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];


        }

        public static DataTable Consultar_Recibos_Empleados(Cls_Rpt_Nom_Relacion_Recibos_Nomina_Negocio Clase_Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            Mi_SQL.Append("SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado);
            Mi_SQL.Append(", " + Cat_Empleados.Tabla_Cat_Empleados + ".CODIGO_PROGRAMATICO");
            Mi_SQL.Append("," + Cat_Empleados.Tabla_Cat_Empleados + "." +  Cat_Empleados.Campo_RFC);
            Mi_SQL.Append("," + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno);
            Mi_SQL.Append(" ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno);
            Mi_SQL.Append(" ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre);
            Mi_SQL.Append(" AS NOMBRE FROM " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + ",");
            Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + " WHERE ");
            Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "=");
            Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID);
            Mi_SQL.Append(" AND " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Dependencia_ID);
            Mi_SQL.Append("='" + Clase_Negocio.P_Dependencia_ID + "'");
            Mi_SQL.Append(" AND " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina);
            Mi_SQL.Append("='" + Clase_Negocio.P_No_Nomina + "'");
            Mi_SQL.Append(" AND " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID);
            Mi_SQL.Append("='" + Clase_Negocio.P_Nomina_ID + "'");
            Mi_SQL.Append(" ORDER BY " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " ASC");



            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
        }


    }
}