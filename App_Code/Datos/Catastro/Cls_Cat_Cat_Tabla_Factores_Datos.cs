using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using Presidencia.Catalogo_Cat_Tabla_Factores.Negocio;
using System.Web.Security;
using Presidencia.Constantes;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Sessiones;
using System.Data.OracleClient;
using System.Collections.Generic;
using SharpContent.ApplicationBlocks.Data;

/// <summary>
/// Summary description for Cls_Cat_Cat_Tabla_Factores_Datos
/// </summary>
/// 
namespace Presidencia.Catalogo_Cat_Tabla_Factores.Datos
{
    public class Cls_Cat_Cat_Tabla_Factores_Datos
    {
        
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Valor_Construccion_Rustico
        ///DESCRIPCIÓN: Da de alta en la Base de Datos el tipo de construccion Rústico con su tabla de valores
        ///PARAMENTROS:     
        ///             1. Tabla_Val.       Instancia de la Clase de Negocio de Tabla de valores de construcción Rústico
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Alta_Tabla_Factores_Cobro_Avaluos(Cls_Cat_Cat_Tabla_Factores_Negocio Tabla_Fac)
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
            String Factor_Cobro_Id = "";
            Factor_Cobro_Id = Obtener_ID_Consecutivo(Cat_Cat_Factores_Cobro_Avaluos.Tabla_Cat_Cat_Factores_Cobro_Avaluos, 
                Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Cobro_Id, "", 5);

                
            try
            {                            

                foreach (DataRow Dr_Renglon in Tabla_Fac.P_Dt_Tabla_Factores.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "BAJA" && Dr_Renglon["FACTOR_COBRO_ID"].ToString().Trim().Replace("&nbsp;", "") != "")
                    {
                        Mi_sql = "DELETE " + Cat_Cat_Factores_Cobro_Avaluos.Tabla_Cat_Cat_Factores_Cobro_Avaluos 
                            + " WHERE " + Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Cobro_Id;
                        Mi_sql += "='" + Dr_Renglon["FACTOR_COBRO_ID"].ToString() + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                      
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {

                        Mi_sql = "INSERT INTO " + Cat_Cat_Factores_Cobro_Avaluos.Tabla_Cat_Cat_Factores_Cobro_Avaluos + "(";
                        Mi_sql += Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Cobro_Id + ", ";
                        Mi_sql += Cat_Cat_Factores_Cobro_Avaluos.Campo_Anio + ", ";
                        Mi_sql += Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Cobro_2 + ", ";
                        Mi_sql += Cat_Cat_Factores_Cobro_Avaluos.Campo_Base_Cobro + ", ";
                        Mi_sql += Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Menor_1_Ha + ", ";
                        Mi_sql += Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Mayor_1_Ha + ", ";
                        Mi_sql += Cat_Cat_Factores_Cobro_Avaluos.Campo_Porcentaje_PE + ", ";
                        Mi_sql += Cat_Cat_Factores_Cobro_Avaluos.Campo_Usuario_Creo + ", ";
                        Mi_sql += Cat_Cat_Factores_Cobro_Avaluos.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += Factor_Cobro_Id + "', '";
                        Mi_sql += Dr_Renglon["ANIO"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["FACTOR_COBRO_2"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["BASE_COBRO"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["FACTOR_MENOR_1_HA"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["FACTOR_MAYOR_1_HA"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["PORCENTAJE_PE"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        Factor_Cobro_Id = (Convert.ToInt16(Factor_Cobro_Id) + 1).ToString("00000");
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "ACTUALIZAR")
                    {
                        Mi_sql = "UPDATE " + Cat_Cat_Factores_Cobro_Avaluos.Tabla_Cat_Cat_Factores_Cobro_Avaluos;
                        Mi_sql += " SET " + Cat_Cat_Factores_Cobro_Avaluos.Campo_Anio + " = " + Dr_Renglon["ANIO"].ToString() + ", ";
                        Mi_sql += Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Cobro_2 + " = " + Dr_Renglon["FACTOR_COBRO_2"].ToString() + ", ";
                        Mi_sql += Cat_Cat_Factores_Cobro_Avaluos.Campo_Base_Cobro + " = " + Dr_Renglon["BASE_COBRO"].ToString() + ", ";
                        Mi_sql += Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Menor_1_Ha + " = " + Dr_Renglon["FACTOR_MENOR_1_HA"].ToString() + ", ";
                        Mi_sql += Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Mayor_1_Ha + " = " + Dr_Renglon["FACTOR_MAYOR_1_HA"].ToString() + ", ";
                        Mi_sql += Cat_Cat_Factores_Cobro_Avaluos.Campo_Porcentaje_PE + " = " + Dr_Renglon["PORCENTAJE_PE"].ToString() + ", ";
                        Mi_sql += Cat_Cat_Factores_Cobro_Avaluos.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += Cat_Cat_Factores_Cobro_Avaluos.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_sql += " WHERE " + Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Cobro_Id + "='" + Dr_Renglon["FACTOR_COBRO_ID"].ToString() + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Alta_Factores_Cobro_Avaluos: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tabla_Valores_Construccion
        ///DESCRIPCIÓN: Obtiene la tabla de valores que estan dados de alta en la Base de Datos de un tipo de construcción
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 03/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Tabla_Factores_Cobro_Avaluos(Cls_Cat_Cat_Tabla_Factores_Negocio Tabla_Fac)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT " + Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Cobro_Id
                    + ", " + Cat_Cat_Factores_Cobro_Avaluos.Campo_Anio
                    + ", 'NADA' AS ACCION"
                    + ", " + Cat_Cat_Factores_Cobro_Avaluos.Campo_Base_Cobro
                    + ", " + Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Cobro_2
                    + ", " + Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Mayor_1_Ha
                    + ", " + Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Menor_1_Ha
                    + ", " + Cat_Cat_Factores_Cobro_Avaluos.Campo_Porcentaje_PE
                    + ", " + Cat_Cat_Factores_Cobro_Avaluos.Campo_Usuario_Creo
                    + ", " + Cat_Cat_Factores_Cobro_Avaluos.Campo_Fecha_Creo
                    + " FROM  " + Cat_Cat_Factores_Cobro_Avaluos.Tabla_Cat_Cat_Factores_Cobro_Avaluos
                    + " WHERE ";
                if (Tabla_Fac.P_Factor_Cobro_Id != null && Tabla_Fac.P_Factor_Cobro_Id.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Cobro_Id+ " = '" + Tabla_Fac.P_Factor_Cobro_Id + "' AND ";
                }
                if (Tabla_Fac.P_Anio != null && Tabla_Fac.P_Anio.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Factores_Cobro_Avaluos.Campo_Anio + " = '" + Tabla_Fac.P_Anio + "' AND ";
                }
                if (Tabla_Fac.P_Base_Cobro != null && Tabla_Fac.P_Base_Cobro.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Factores_Cobro_Avaluos.Campo_Base_Cobro + " = " + Tabla_Fac.P_Base_Cobro + "' AND ";
                }
                if (Tabla_Fac.P_Factor_Cobro_2 != null && Tabla_Fac.P_Factor_Cobro_2.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Cobro_2 + " = " + Tabla_Fac.P_Factor_Cobro_2 + "' AND ";
                }
                if (Tabla_Fac.P_Factor_Mayor_1_Ha != null && Tabla_Fac.P_Factor_Mayor_1_Ha.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Mayor_1_Ha + " = " + Tabla_Fac.P_Factor_Mayor_1_Ha + "' AND ";
                }
                if (Tabla_Fac.P_Factor_Menor_1_Ha != null && Tabla_Fac.P_Factor_Menor_1_Ha.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Menor_1_Ha + " = " + Tabla_Fac.P_Factor_Menor_1_Ha + "' AND ";
                }
                
                if (Tabla_Fac.P_Porcentaje_Perito_Externo != null && Tabla_Fac.P_Porcentaje_Perito_Externo.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Factores_Cobro_Avaluos.Campo_Porcentaje_PE + " = " + Tabla_Fac.P_Porcentaje_Perito_Externo + "' AND ";
                }
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                // agregar filtro y orden a la consulta
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Tabla  Factores Cobro Avaluos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : Antonio Salvador Benavides Guardado
        ///FECHA_MODIFICO       : 26/Octubre/2010
        ///CAUSA_MODIFICACIÓN   : Estandarizar variables usadas
        ///*******************************************************************************
        public static String Obtener_ID_Consecutivo(String Tabla, String Campo, String Filtro, Int32 Longitud_ID)
        {
            String Id = Convertir_A_Formato_ID(1, Longitud_ID); ;
            try
            {
                String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla;
                if (Filtro != "" && Filtro != null)
                {
                    Mi_SQL += " WHERE " + Filtro;
                }
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                {
                    Id = Convertir_A_Formato_ID((Convert.ToInt32(Obj_Temp) + 1), Longitud_ID);
                }
            }
            catch (Exception Ex)
            {
                new Exception(Ex.Message);
            }
            return Id;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Convertir_A_Formato_ID
        ///DESCRIPCIÓN: Pasa un numero entero a Formato de ID.
        ///PARAMETROS:     
        ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
        ///             2. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : Antonio Salvador Benavides Guardado
        ///FECHA_MODIFICO       : 26/Octubre/2010
        ///CAUSA_MODIFICACIÓN   : Estandarizar variables usadas
        ///*******************************************************************************
        private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID)
        {
            String Retornar = "";
            String Dato = "" + Dato_ID;
            for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++)
            {
                Retornar = Retornar + "0";
            }
            Retornar = Retornar + Dato;
            return Retornar;
        }

        }
    }
