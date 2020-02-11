using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Constantes;
using System.Data;
using Presidencia.Catalogo_Cat_Identificadores_Predio.Negocio;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Sessiones;

/// <summary>
/// Summary description for Cls_Cat_Cat_Identificadores_Predio_Datos
/// </summary>

namespace Presidencia.Catalogo_Cat_Identificadores_Predio.Datos
{
    public class Cls_Cat_Cat_Identificadores_Predio_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Identificadores_Predio
        ///DESCRIPCIÓN          : Obtiene las cuentas de acuerdo a los filtros establecidos en la interfaz
        ///PARAMETROS           : Cuenta, instancia de Cls_Cat_Pre_Cuentas_Predial_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 07/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Identificadores_Predio(Cls_Cat_Cat_Identificadores_Predio_Negocio Cuenta)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;
            try
            {
                    Mi_SQL = "SELECT ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Region + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Manzana + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Lote + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Horas_X + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Minutos_X + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Segundos_X + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Orientacion_X + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Horas_Y + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Minutos_Y + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Segundos_Y + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Orientacion_Y + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Coordenadas_UTM + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Coordenadas_UTM_Y + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo + " ";
                    Mi_SQL += " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;
                    Mi_SQL += " WHERE ";
                        Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta.P_Cuenta_Predial_Id + "'";
                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null)
                {
                    Tabla = dataSet.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar Obtener los identificadores del predio. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Identificadores_Predio
        ///DESCRIPCIÓN: Modifica la región, manzana y lote de un Predio
        ///PARAMENTROS:     
        ///             1. Cuenta.          Instancia de la Clase de Negocio de Identificadores de Predio
        ///                                 con los datos del que van a ser
        ///                                 modificado.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 22/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Modificar_Identificadores_Predio(Cls_Cat_Cat_Identificadores_Predio_Negocio Cuenta)
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
                Mi_sql = "UPDATE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;
                Mi_sql += " SET " + Cat_Pre_Cuentas_Predial.Campo_Region + " = '" + Cuenta.P_Region + "', ";
                Mi_sql += Cat_Pre_Cuentas_Predial.Campo_Manzana + " = '" + Cuenta.P_Manzana + "', ";
                Mi_sql += Cat_Pre_Cuentas_Predial.Campo_Lote + " = '" + Cuenta.P_Lote + "', ";
                if (Cuenta.P_Horas_X != null && Cuenta.P_Horas_X.Trim() != "")
                {
                    Mi_sql += Cat_Pre_Cuentas_Predial.Campo_Horas_X + " = " + Cuenta.P_Horas_X + ", ";
                }
                else
                {
                    Mi_sql += Cat_Pre_Cuentas_Predial.Campo_Horas_X + " = NULL, ";
                }
                if (Cuenta.P_Minutos_X != null && Cuenta.P_Minutos_X.Trim() != "")
                {
                    Mi_sql += Cat_Pre_Cuentas_Predial.Campo_Minutos_X + " = " + Cuenta.P_Minutos_X + ", ";
                }
                else
                {
                    Mi_sql += Cat_Pre_Cuentas_Predial.Campo_Minutos_X + " = NULL, ";
                }
                if (Cuenta.P_Segundos_X != null && Cuenta.P_Segundos_X.Trim() != "")
                {
                    Mi_sql += Cat_Pre_Cuentas_Predial.Campo_Segundos_X + " = " + Cuenta.P_Segundos_X + ", ";
                }
                else
                {
                    Mi_sql += Cat_Pre_Cuentas_Predial.Campo_Segundos_X + " = NULL, ";
                }
                Mi_sql += Cat_Pre_Cuentas_Predial.Campo_Orientacion_X + " = '" + Cuenta.P_Orientacion_X + "', ";
                if (Cuenta.P_Horas_Y != null && Cuenta.P_Horas_Y.Trim() != "")
                {
                    Mi_sql += Cat_Pre_Cuentas_Predial.Campo_Horas_Y + " = " + Cuenta.P_Horas_Y + ", ";
                }
                else
                {
                    Mi_sql += Cat_Pre_Cuentas_Predial.Campo_Horas_Y + " = NULL, ";
                }
                if (Cuenta.P_Minutos_Y != null && Cuenta.P_Minutos_Y.Trim() != "")
                {
                    Mi_sql += Cat_Pre_Cuentas_Predial.Campo_Minutos_Y + " = " + Cuenta.P_Minutos_Y + ",";
                }
                else
                {
                    Mi_sql += Cat_Pre_Cuentas_Predial.Campo_Minutos_Y + " = NULL, ";
                }
                if (Cuenta.P_Segundos_Y != null && Cuenta.P_Segundos_Y.Trim() != "")
                {
                    Mi_sql += Cat_Pre_Cuentas_Predial.Campo_Segundos_Y + " = " + Cuenta.P_Segundos_Y + ",";
                }
                else
                {
                    Mi_sql += Cat_Pre_Cuentas_Predial.Campo_Segundos_Y + " = NULL, ";
                }
                Mi_sql += Cat_Pre_Cuentas_Predial.Campo_Orientacion_Y + " = '" + Cuenta.P_Orientacion_Y + "',";
                Mi_sql += Cat_Pre_Cuentas_Predial.Campo_Coordenadas_UTM + " = '" + Cuenta.P_Coordenadas_UTM + "',";
                Mi_sql += Cat_Pre_Cuentas_Predial.Campo_Coordenadas_UTM_Y + " = '" + Cuenta.P_Coordenadas_UTM_Y + "',";
                Mi_sql += Cat_Pre_Cuentas_Predial.Campo_Tipo + " = '" + Cuenta.P_Tipo + "',";
                Mi_sql += Cat_Pre_Cuentas_Predial.Campo_Usuario_Creo + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += Cat_Pre_Cuentas_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_sql += " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta.P_Cuenta_Predial_Id + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Identificadores_Predio: [" + E.Message + "].");
            }
            Trans.Commit();
            return Alta;
        }
    }
}