using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Catalogo_Cat_Parametros.Negocio;
using System.Data.OracleClient;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data;
using Presidencia.Sessiones;

/// <summary>
/// Summary description for Cls_Cat_Cat_Parametros_Datos
/// </summary>

namespace Presidencia.Catalogo_Cat_Parametros.Datos
{
    public class Cls_Cat_Cat_Parametros_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Parametros
        ///DESCRIPCIÓN: Modifica los parametros.
        ///PARAMENTROS:     
        ///             1. Parametro.       Instancia de la Clase de Negocio de Parametros
        ///                                 con los datos del que van a ser
        ///                                 modificados.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 10/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Modificar_Parametros(Cls_Cat_Cat_Parametros_Negocio Parametro)
        {
            Boolean Alta = false;
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String Mi_sql = "";            
            try
            {

                Mi_sql = "UPDATE " + Cat_Cat_Parametros.Tabla_Cat_Cat_Parametros;
                Mi_sql += " SET " + Cat_Cat_Parametros.Campo_Decimales_Redondeo + " = " + Parametro.P_Decimales_Redondeo + ", ";
                Mi_sql += Cat_Cat_Parametros.Campo_Incremento_Valor + " = " + Parametro.P_Incremento_Valor + ", ";
                Mi_sql += Cat_Cat_Parametros.Campo_Factor_Ef + " = '" + Parametro.P_Factor_Ef + "', ";
                Mi_sql += Cat_Cat_Parametros.Campo_Columnas_Calc_Construccion + " = '" + Parametro.P_Column_Calc_Const + "', ";
                Mi_sql += Cat_Cat_Parametros.Campo_Renglones_Calc_Construccion + " = '" + Parametro.P_Renglones_Calc_Const + "', ";
                Mi_sql += Cat_Cat_Parametros.Campo_Dias_Vigencia + " = '" + Parametro.P_Dias_Vigencia + "', ";
                Mi_sql += Cat_Cat_Parametros.Campo_Correo_Autorizacion + " = '" + Parametro.P_Correo_Autorizacion + "', ";
                Mi_sql += Cat_Cat_Parametros.Campo_Correo_General + " = '" + Parametro.P_Correo_General + "', ";
                Mi_sql += Cat_Cat_Parametros.Campo_Firmante + " = '" + Parametro.P_Firmante + "', ";
                Mi_sql += Cat_Cat_Parametros.Campo_Puesto + " = '" + Parametro.P_Puesto + "', "; 
                Mi_sql += Cat_Cat_Parametros.Campo_Firmante_2 + " = '" + Parametro.P_Firmante_2 + "', ";
                Mi_sql += Cat_Cat_Parametros.Campo_Fundamentacion_Legal + " = '" + Parametro.P_Fundamentacion_Legal + "', ";
                Mi_sql += Cat_Cat_Parametros.Campo_Puesto_2 + " = '" + Parametro.P_Puesto_2 + "', "; 
                Mi_sql += Cat_Cat_Parametros.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += Cat_Cat_Parametros.Campo_Fecha_Modifico + " = SYSDATE";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Parametros: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Parametros
        ///DESCRIPCIÓN: Obtiene la tabla de los parámetros del sistema
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 10/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Parametros(Cls_Cat_Cat_Parametros_Negocio Parametro)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT " + Cat_Cat_Parametros.Campo_Decimales_Redondeo
                    + ", " + Cat_Cat_Parametros.Campo_Incremento_Valor
                    + ", " + Cat_Cat_Parametros.Campo_Factor_Ef
                    + ", " + Cat_Cat_Parametros.Campo_Columnas_Calc_Construccion
                    + ", " + Cat_Cat_Parametros.Campo_Renglones_Calc_Construccion
                    + ", " + Cat_Cat_Parametros.Campo_Dias_Vigencia
                    + ", " + Cat_Cat_Parametros.Campo_Correo_General
                    + ", " + Cat_Cat_Parametros.Campo_Correo_Autorizacion
                    + ", " + Cat_Cat_Parametros.Campo_Firmante
                    + ", " + Cat_Cat_Parametros.Campo_Puesto
                    + ", " + Cat_Cat_Parametros.Campo_Firmante_2
                    + ", " + Cat_Cat_Parametros.Campo_Puesto_2
                    + ", " + Cat_Cat_Parametros.Campo_Fundamentacion_Legal
                    + ", " + Cat_Cat_Parametros.Campo_Usuario_Creo
                    + ", " + Cat_Cat_Parametros.Campo_Fecha_Creo
                    + " FROM  " + Cat_Cat_Parametros.Tabla_Cat_Cat_Parametros;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los Parámetros del sistema. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
    }
}