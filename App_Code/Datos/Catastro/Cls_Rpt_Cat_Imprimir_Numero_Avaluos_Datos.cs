using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;

/// <summary>
/// Summary description for Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Datos
/// </summary>
public class Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Datos
{
	 ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Avaluos_Asignados_Primera_Entrega
        ///DESCRIPCIÓN: Obtiene la tabla de los avaluos asignado a cada perito
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 12/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
    public static DataTable Consultar_Avaluos_Asignados_Primera_Entrega(Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Negocio Cuotas_Perito)
    {
        DataTable Tabla = new DataTable();
        String Mi_SQL = "";
        
        try
        {
            Mi_SQL = "SELECT PIN." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id
                + ", (SELECT COUNT(AC." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id 
                + ") FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas +" AC" 
                + " WHERE AC." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id +  " = PIN." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id +" AND"
                + " AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + " ='1_ENTREGA' AND" 
                + " (AC."+ Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN"
                + " (SELECT "+ Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion 
                + " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av +" AU"
                + " WHERE AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion
                + "= AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " AND AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Estatus + "= 'AUTORIZADO') OR" 
                + " AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN" + " (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion
                + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR"
                + " WHERE AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion
                + "= AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " AND AR." + Ope_Cat_Avaluo_Rustico_V.Campo_Estatus + "= 'AUTORIZADO'))" + ") AS TOTAL_ENTREGADOS" 
                + ", (SELECT CP." + Cat_Cat_Cuotas_Perito.Campo_1_Entrega 
                + " FROM " + Cat_Cat_Cuotas_Perito.Tabla_Cat_Cat_Cuotas_Perito + " CP"
                + " WHERE CP." + Cat_Cat_Cuotas_Perito.Campo_Perito_Interno_Id + " = PIN." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id + " AND "
                + Cat_Cat_Cuotas_Perito.Campo_Anio + "= TO_NUMBER(TO_CHAR(SYSDATE, 'YYYY'))) AS CUOTA"
                + " FROM " + Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos + " PIN"
                + " WHERE PIN." + Cat_Cat_Peritos_Internos.Campo_Estatus + "= 'VIGENTE'" + " AND ";
            
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
            String Mensaje = "Error al intentar consultar los Datos de peritos internos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
            throw new Exception(Mensaje);
        }
        return Tabla;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Avaluos_Asignados_Segunda_Entrega
    ///DESCRIPCIÓN: Obtiene la tabla de los avaluos asignado a cada perito
    ///PARAMETROS:
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 12/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public static DataTable Consultar_Avaluos_Asignados_Segunda_Entrega(Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Negocio Cuotas_Perito)
    {
        DataTable Tabla = new DataTable();
        String Mi_SQL = "";
        try
        {
            Mi_SQL = "SELECT PIN." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id
                + ", (SELECT COUNT(AC." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id
                + ") FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " AC"
                + " WHERE AC." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + " = PIN." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + " AND"
                + " AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + " ='2_ENTREGA' AND"
                + " (AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN"
                + " (SELECT " + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion
                + " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " AU"
                + " WHERE AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion
                + "= AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " AND AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Estatus + "= 'AUTORIZADO') OR"
                + " AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN" + " (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion
                + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR"
                + " WHERE AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion
                + "= AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " AND AR." + Ope_Cat_Avaluo_Rustico_V.Campo_Estatus + "= 'AUTORIZADO'))" + ") AS TOTAL_ENTREGADOS"
                + ", (SELECT CP." + Cat_Cat_Cuotas_Perito.Campo_2_Entrega
                + " FROM " + Cat_Cat_Cuotas_Perito.Tabla_Cat_Cat_Cuotas_Perito + " CP"
                + " WHERE CP." + Cat_Cat_Cuotas_Perito.Campo_Perito_Interno_Id + " = PIN." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id + " AND "
                + Cat_Cat_Cuotas_Perito.Campo_Anio + "= TO_NUMBER(TO_CHAR(SYSDATE, 'YYYY'))) AS CUOTA"
                + " FROM " + Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos + " PIN"
                + " WHERE PIN." + Cat_Cat_Peritos_Internos.Campo_Estatus + "= 'VIGENTE'" + " AND ";

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
            String Mensaje = "Error al intentar consultar los Datos de peritos internos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
            throw new Exception(Mensaje);
        }
        return Tabla;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Avaluos_Asignados_Tercera_Entrega
    ///DESCRIPCIÓN: Obtiene la tabla de los avaluos asignado a cada perito
    ///PARAMETROS:
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 12/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public static DataTable Consultar_Avaluos_Asignados_Tercera_Entrega(Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Negocio Cuotas_Perito)
    {
        DataTable Tabla = new DataTable();
        String Mi_SQL = "";
        try
        {
            Mi_SQL = "SELECT PIN." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id
                + ", (SELECT COUNT(AC." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id
                + ") FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " AC"
                + " WHERE AC." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + " = PIN." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + " AND"
                + " AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + " ='3_ENTREGA' AND"
                + " (AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN"
                + " (SELECT " + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion
                + " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " AU"
                + " WHERE AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion
                + "= AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " AND AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Estatus + "= 'AUTORIZADO') OR"
                + " AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN" + " (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion
                + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR"
                + " WHERE AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion
                + "= AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " AND AR." + Ope_Cat_Avaluo_Rustico_V.Campo_Estatus + "= 'AUTORIZADO'))" + ") AS TOTAL_ENTREGADOS"
                + ", (SELECT CP." + Cat_Cat_Cuotas_Perito.Campo_3_Entrega
                + " FROM " + Cat_Cat_Cuotas_Perito.Tabla_Cat_Cat_Cuotas_Perito + " CP"
                + " WHERE CP." + Cat_Cat_Cuotas_Perito.Campo_Perito_Interno_Id + " = PIN." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id + " AND "
                + Cat_Cat_Cuotas_Perito.Campo_Anio + "= TO_NUMBER(TO_CHAR(SYSDATE, 'YYYY'))) AS CUOTA"
                + " FROM " + Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos + " PIN"
                + " WHERE PIN." + Cat_Cat_Peritos_Internos.Campo_Estatus + "= 'VIGENTE'" + " AND ";

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
            String Mensaje = "Error al intentar consultar los Datos de peritos internos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
            throw new Exception(Mensaje);
        }
        return Tabla;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Avaluos_Asignados_Cuarta_Entrega
    ///DESCRIPCIÓN: Obtiene la tabla de los avaluos asignado a cada perito
    ///PARAMETROS:
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 12/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public static DataTable Consultar_Avaluos_Asignados_Cuarta_Entrega(Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Negocio Cuotas_Perito)
    {
        DataTable Tabla = new DataTable();
        String Mi_SQL = "";
        try
        {
            Mi_SQL = "SELECT PIN." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id
                + ", (SELECT COUNT(AC." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id
                + ") FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " AC"
                + " WHERE AC." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + " = PIN." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + " AND"
                + " AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + " ='4_ENTREGA' AND"
                + " (AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN"
                + " (SELECT " + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion
                + " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " AU"
                + " WHERE AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion
                + "= AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " AND AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Estatus + "= 'AUTORIZADO') OR"
                + " AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN" + " (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion
                + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR"
                + " WHERE AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion
                + "= AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " AND AR." + Ope_Cat_Avaluo_Rustico_V.Campo_Estatus + "= 'AUTORIZADO'))" + ") AS TOTAL_ENTREGADOS"
                + ", (SELECT CP." + Cat_Cat_Cuotas_Perito.Campo_4_Entrega
                + " FROM " + Cat_Cat_Cuotas_Perito.Tabla_Cat_Cat_Cuotas_Perito + " CP"
                + " WHERE CP." + Cat_Cat_Cuotas_Perito.Campo_Perito_Interno_Id + " = PIN." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id + " AND "
                + Cat_Cat_Cuotas_Perito.Campo_Anio + "= TO_NUMBER(TO_CHAR(SYSDATE, 'YYYY'))) AS CUOTA"
                + " FROM " + Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos + " PIN"
                + " WHERE PIN." + Cat_Cat_Peritos_Internos.Campo_Estatus + "= 'VIGENTE'" + " AND ";

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
            String Mensaje = "Error al intentar consultar los Datos de peritos internos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
            throw new Exception(Mensaje);
        }
        return Tabla;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Avaluos_Asignados_Quinta_Entrega
    ///DESCRIPCIÓN: Obtiene la tabla de los avaluos asignado a cada perito
    ///PARAMETROS:
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 12/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public static DataTable Consultar_Avaluos_Asignados_Quinta_Entrega(Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Negocio Cuotas_Perito)
    {
        DataTable Tabla = new DataTable();
        String Mi_SQL = "";
        try
        {
            Mi_SQL = "SELECT PIN." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id
                + ", (SELECT COUNT(AC." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id
                + ") FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " AC"
                + " WHERE AC." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + " = PIN." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + " AND"
                + " AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + " ='5_ENTREGA' AND"
                + " (AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN"
                + " (SELECT " + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion
                + " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " AU"
                + " WHERE AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion
                + "= AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " AND AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Estatus + "= 'AUTORIZADO') OR"
                + " AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN" + " (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion
                + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR"
                + " WHERE AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion
                + "= AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " AND AR." + Ope_Cat_Avaluo_Rustico_V.Campo_Estatus + "= 'AUTORIZADO'))" + ") AS TOTAL_ENTREGADOS"
                + ", (SELECT CP." + Cat_Cat_Cuotas_Perito.Campo_5_Entrega
                + " FROM " + Cat_Cat_Cuotas_Perito.Tabla_Cat_Cat_Cuotas_Perito + " CP"
                + " WHERE CP." + Cat_Cat_Cuotas_Perito.Campo_Perito_Interno_Id + " = PIN." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id + " AND "
                + Cat_Cat_Cuotas_Perito.Campo_Anio + "= TO_NUMBER(TO_CHAR(SYSDATE, 'YYYY'))) AS CUOTA"
                + " FROM " + Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos + " PIN"
                + " WHERE PIN." + Cat_Cat_Peritos_Internos.Campo_Estatus + "= 'VIGENTE'" + " AND ";

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
            String Mensaje = "Error al intentar consultar los Datos de peritos internos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
            throw new Exception(Mensaje);
        }
        return Tabla;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Avaluos_Asignados_Sexta_Entrega
    ///DESCRIPCIÓN: Obtiene la tabla de los avaluos asignado a cada perito
    ///PARAMETROS:
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 12/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public static DataTable Consultar_Avaluos_Asignados_Sexta_Entrega(Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Negocio Cuotas_Perito)
    {
        DataTable Tabla = new DataTable();
        String Mi_SQL = "";
        try
        {
            Mi_SQL = "SELECT PIN." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id
                + ", (SELECT COUNT(AC." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id
                + ") FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " AC"
                + " WHERE AC." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + " = PIN." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + " AND"
                + " AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + " ='6_ENTREGA' AND"
                + " (AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN"
                + " (SELECT " + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion
                + " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " AU"
                + " WHERE AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion
                + "= AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " AND AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Estatus + "= 'AUTORIZADO') OR"
                + " AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN" + " (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion
                + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR"
                + " WHERE AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion
                + "= AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " AND AR." + Ope_Cat_Avaluo_Rustico_V.Campo_Estatus + "= 'AUTORIZADO'))" + ") AS TOTAL_ENTREGADOS"
                + ", (SELECT CP." + Cat_Cat_Cuotas_Perito.Campo_6_Entrega
                + " FROM " + Cat_Cat_Cuotas_Perito.Tabla_Cat_Cat_Cuotas_Perito + " CP"
                + " WHERE CP." + Cat_Cat_Cuotas_Perito.Campo_Perito_Interno_Id + " = PIN." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id + " AND "
                + Cat_Cat_Cuotas_Perito.Campo_Anio + "= TO_NUMBER(TO_CHAR(SYSDATE, 'YYYY'))) AS CUOTA"
                + " FROM " + Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos + " PIN"
                + " WHERE PIN." + Cat_Cat_Peritos_Internos.Campo_Estatus + "= 'VIGENTE'" + " AND ";

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
            String Mensaje = "Error al intentar consultar los Datos de peritos internos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
            throw new Exception(Mensaje);
        }
        return Tabla;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Avaluos_Asignados_Septima_Entrega
    ///DESCRIPCIÓN: Obtiene la tabla de los avaluos asignado a cada perito
    ///PARAMETROS:
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 12/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public static DataTable Consultar_Avaluos_Asignados_Septima_Entrega(Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Negocio Cuotas_Perito)
    {
        DataTable Tabla = new DataTable();
        String Mi_SQL = "";
        try
        {
            Mi_SQL = "SELECT PIN." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id
                + ", (SELECT COUNT(AC." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id
                + ") FROM " + Ope_Cat_Asignacion_Cuentas.Tabla_Ope_Cat_Asignacion_Cuentas + " AC"
                + " WHERE AC." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + " = PIN." + Ope_Cat_Asignacion_Cuentas.Campo_Perito_Interno_Id + " AND"
                + " AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Entrega + " ='7_ENTREGA' AND"
                + " (AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN"
                + " (SELECT " + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion
                + " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " AU"
                + " WHERE AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion
                + "= AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " AND AU." + Ope_Cat_Avaluo_Urbano_Av.Campo_Estatus + "= 'AUTORIZADO') OR"
                + " AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " IN" + " (SELECT AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion
                + " FROM " + Ope_Cat_Avaluo_Rustico_V.Tabla_Ope_Cat_Avaluo_Rustico_V + " AR"
                + " WHERE AR." + Ope_Cat_Avaluo_Rustico_V.Campo_No_Asignacion
                + "= AC." + Ope_Cat_Asignacion_Cuentas.Campo_No_Asignacion + " AND AR." + Ope_Cat_Avaluo_Rustico_V.Campo_Estatus + "= 'AUTORIZADO'))" + ") AS TOTAL_ENTREGADOS"
                + ", (SELECT CP." + Cat_Cat_Cuotas_Perito.Campo_7_Entrega
                + " FROM " + Cat_Cat_Cuotas_Perito.Tabla_Cat_Cat_Cuotas_Perito + " CP"
                + " WHERE CP." + Cat_Cat_Cuotas_Perito.Campo_Perito_Interno_Id + " = PIN." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id + " AND "
                + Cat_Cat_Cuotas_Perito.Campo_Anio + "= TO_NUMBER(TO_CHAR(SYSDATE, 'YYYY'))) AS CUOTA"
                + " FROM " + Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos + " PIN"
                + " WHERE PIN." + Cat_Cat_Peritos_Internos.Campo_Estatus + "= 'VIGENTE'" + " AND ";

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
            String Mensaje = "Error al intentar consultar los Datos de peritos internos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
            throw new Exception(Mensaje);
        }
        return Tabla;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Avaluos_Asignados_Septima_Entrega
    ///DESCRIPCIÓN: Obtiene la tabla de los avaluos asignado a cada perito
    ///PARAMETROS:
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 12/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public static DataTable Consultar_Avaluos_Fiscales(Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Negocio Avaluos)
    {
        DataTable Tabla = new DataTable();
        String Mi_SQL = "";
        try
        {
            Mi_SQL = "SELECT PAS." + Ope_Ing_Pasivo.Campo_Monto + " AS HONORARIOS"
                + ", SOL." + Ope_Tra_Solicitud.Campo_Cuenta_Predial + " AS CUENTA"
                + ", SOL." + Ope_Tra_Solicitud.Campo_Inspector_ID + " AS PERITO"
                + ", AU." + Ope_Cat_Avaluo_Urbano.Campo_Colonia + " || ' ' ||"
                + " AU." + Ope_Cat_Avaluo_Urbano.Campo_Ubicacion + " AS UBICACION"
                + ", AU." + Ope_Cat_Avaluo_Urbano.Campo_Valor_Total_Predio + " AS VAL_PREDIO"
                + ", AU." + Ope_Cat_Avaluo_Urbano.Campo_Region
                + ", AU." + Ope_Cat_Avaluo_Urbano.Campo_Manzana
                 + ", 'U' AS TIPO"
                + " FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " PAS"
                + " INNER JOIN " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " SOL ON"
                + " UPPER(TRIM(PAS." + Ope_Ing_Pasivo.Campo_Referencia + "))="
                + "UPPER(TRIM(SOL." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + "))"
                + " INNER JOIN  " + Ope_Cat_Avaluo_Urbano.Tabla_Ope_Cat_Avaluo_Urbano + " AU ON"
                + " UPPER(TRIM(SOL." + Ope_Tra_Solicitud.Campo_Solicitud_ID + "))="
                + "UPPER(TRIM(AU." + Ope_Cat_Avaluo_Urbano.Campo_Solicitud_Id + "))"
                + " WHERE UPPER(PAS." + Ope_Ing_Pasivo.Campo_Descripcion + ")='AVALUO' AND"
                + " PAS." + Ope_Ing_Pasivo.Campo_Estatus + "='PAGADO' UNION"


                + " SELECT PAS." + Ope_Ing_Pasivo.Campo_Monto + " AS HONORARIOS"
                + ", SOL." + Ope_Tra_Solicitud.Campo_Cuenta_Predial + " AS CUENTA"
                + ", SOL." + Ope_Tra_Solicitud.Campo_Inspector_ID + " AS PERITO"
                + ", AR." + Ope_Cat_Avaluo_Rustico.Campo_Ubicacion
                + ", AR." + Ope_Cat_Avaluo_Rustico.Campo_Valor_Total_Predio + " AS VAL_PREDIO"
                + ", '' AS REGION"
                 + ", '' AS MANZANA"
                + ", 'R' AS TIPO"
                + " FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " PAS"
                + " INNER JOIN " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " SOL ON"
                + " UPPER(TRIM(PAS." + Ope_Ing_Pasivo.Campo_Referencia + "))="
                + "UPPER(TRIM(SOL." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + "))"
                + " INNER JOIN  " + Ope_Cat_Avaluo_Rustico.Tabla_Ope_Cat_Avaluo_Rustico + " AR ON"
                + " UPPER(TRIM(SOL." + Ope_Tra_Solicitud.Campo_Solicitud_ID + "))="
                + "UPPER(TRIM(AR." + Ope_Cat_Avaluo_Rustico.Campo_Solicitud_Id + "))"
                + " WHERE UPPER(PAS." + Ope_Ing_Pasivo.Campo_Descripcion + ")='AVALUO' AND"
                + " PAS." + Ope_Ing_Pasivo.Campo_Estatus + "='PAGADO'" + " AND ";

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
            String Mensaje = "Error al intentar consultar los Datos de peritos internos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
            throw new Exception(Mensaje);
        }
        return Tabla;
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Avaluos_Asignados_Septima_Entrega
    ///DESCRIPCIÓN: Obtiene la tabla de los avaluos asignado a cada perito
    ///PARAMETROS:
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 12/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public static DataTable Consultar_Metas_Autorizacion(Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Negocio Avaluos)
    {
        DataTable Tabla = new DataTable();
        String Mi_SQL = "";
        try
        {
            Mi_SQL = " SELECT distinct(UPPER(To_Char(AU.FECHA_AUTORIZO,'Month')))  AS MES , UPPER(To_Char(AU.FECHA_AUTORIZO,'mm')) as orden , au.solicitud_id,(SELECT COUNT(AUS.fecha_autorizo)  FROM OPE_CAT_AVALUO_URBANO AUS WHERE UPPER(To_Char(AU.FECHA_AUTORIZO,'Month'))=UPPER(To_Char(AUS.FECHA_AUTORIZO,'Month')) AND aus.estatus='AUTORIZADO') AS AUTORIZADOS,"
                    + " (SELECT SUM(AUSR.VECES_RECHAZO)  FROM OPE_CAT_AVALUO_URBANO AUSR WHERE UPPER(To_Char(AU.FECHA_AUTORIZO,'Month'))=UPPER(To_Char(AUSR.FECHA_AUTORIZO,'Month')) AND ausR.estatus='AUTORIZADO') AS CORRECCIONES,"
                    +" (SELECT SUM(PAS.MONTO) FROM OPE_ING_PASIVO PAS "
                    +" INNER JOIN OPE_TRA_SOLICITUD SOL ON UPPER(trim(PAS.REFERENCIA))=UPPER(trim(SOL.CLAVE_SOLICITUD)) " 
                    +" INNER JOIN OPE_CAT_AVALUO_URBANO AUP ON UPPER(trim(sol.solicitud_id))=UPPER(trim(auP.solicitud_id))"
                    +" WHERE   pas.estatus='PAGADO' AND auP.estatus='AUTORIZADO' AND UPPER(To_Char(au.FECHA_AUTORIZO,'Month'))=UPPER(To_Char(AUP.FECHA_AUTORIZO,'Month'))) AS HONORARIOS"
                    +" FROM OPE_CAT_AVALUO_URBANO AU WHERE AU.estatus= 'AUTORIZADO' ORDER BY  UPPER(To_Char(AU.FECHA_AUTORIZO,'mm')) ";
                   
            DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            if (dataset != null)
            {
                Tabla = dataset.Tables[0];
            }
        }
        catch (Exception Ex)
        {
            String Mensaje = "Error al intentar consultar los Datos de peritos internos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
            throw new Exception(Mensaje);
        }
        return Tabla;
    }


}
