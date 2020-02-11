using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Catalogo_Cat_Valores_Inpa.Negocio;
using System.Data.OracleClient;
using Presidencia.Constantes;
using System.Data;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Sessiones;

/// <summary>
/// Summary description for Cls_Cat_Cat_Valores_Inpa_Datos
/// </summary>

namespace Presidencia.Catalogo_Cat_Valores_Inpa.Datos
{
    public class Cls_Cat_Cat_Valores_Inpa_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Valor_Inpa
        ///DESCRIPCIÓN: Da de alta en la Base de Datos los valores I.N.P.A.
        ///PARAMENTROS:     
        ///             1. Tabla_Val.       Instancia de la Clase de Negocio de los valores I.n.p.a.
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Alta_Valor_Inpa(Cls_Cat_Cat_Valores_Inpa_Negocio Tabla_Val)
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
            String Valor_Inpa_Id = "";
            Valor_Inpa_Id = Obtener_ID_Consecutivo(Cat_Cat_Tab_Val_Inpa.Tabla_Cat_Cat_Tab_Val_Inpa, Cat_Cat_Tab_Val_Inpa.Campo_Valor_Inpa_Id, "", 5);
            try
            {
                foreach (DataRow Dr_Renglon in Tabla_Val.P_Dt_Tabla_Valores_Inpa.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "BAJA" && Dr_Renglon["VALOR_INPA_ID"].ToString().Trim().Replace("&nbsp;", "") != "")
                    {
                        Mi_sql = "DELETE " + Cat_Cat_Tab_Val_Inpa.Tabla_Cat_Cat_Tab_Val_Inpa + " WHERE " + Cat_Cat_Tab_Val_Inpa.Campo_Valor_Inpa_Id;
                        Mi_sql += "='" + Dr_Renglon["VALOR_INPA_ID"].ToString() + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Cat_Cat_Tab_Val_Inpa.Tabla_Cat_Cat_Tab_Val_Inpa + "(";
                        Mi_sql += Cat_Cat_Tab_Val_Inpa.Campo_Valor_Inpa_Id + ", ";
                        Mi_sql += Cat_Cat_Tab_Val_Inpa.Campo_Valor_Inpa + ", ";
                        Mi_sql += Cat_Cat_Tab_Val_Inpa.Campo_Anio + ", ";
                        Mi_sql += Cat_Cat_Tab_Val_Inpa.Campo_Usuario_Creo + ", ";
                        Mi_sql += Cat_Cat_Tab_Val_Inpa.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += Valor_Inpa_Id + "', ";
                        Mi_sql += Dr_Renglon["VALOR_INPA"].ToString() + ", ";
                        Mi_sql += Dr_Renglon["ANIO"].ToString() + ", '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        Valor_Inpa_Id = (Convert.ToInt16(Valor_Inpa_Id) + 1).ToString("00000");
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "ACTUALIZAR")
                    {
                        Mi_sql = "UPDATE " + Cat_Cat_Tab_Val_Inpa.Tabla_Cat_Cat_Tab_Val_Inpa;
                        Mi_sql += " SET " + Cat_Cat_Tab_Val_Inpa.Campo_Anio + " = " + Dr_Renglon["ANIO"].ToString() + ", ";
                        Mi_sql += Cat_Cat_Tab_Val_Inpa.Campo_Valor_Inpa + " = " + Dr_Renglon["VALOR_INPA"].ToString() + ", ";
                        Mi_sql += Cat_Cat_Tab_Val_Inpa.Campo_Usuario_Creo + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += Cat_Cat_Tab_Val_Inpa.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_sql += " WHERE " + Cat_Cat_Tab_Val_Inpa.Campo_Valor_Inpa_Id + "='" + Dr_Renglon["VALOR_INPA_ID"].ToString() + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Alta_Valor_Inpa: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Valor_Inpa
        ///DESCRIPCIÓN: Modifica en la Base de Datos el tipo de construcción y elimina, agrega y/o modifica los valores I.N.P.A.
        ///PARAMENTROS:     
        ///             1. Tabla_Val.       Instancia de la Clase de Negocio de Tabla de valores I.N.P.A.
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta, eliminados y/o modificados.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Modificar_Valor_Inpa(Cls_Cat_Cat_Valores_Inpa_Negocio Tabla_Val)
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
            String Valor_Inpa_Id = "";
            Valor_Inpa_Id = Obtener_ID_Consecutivo(Cat_Cat_Tab_Val_Inpa.Tabla_Cat_Cat_Tab_Val_Inpa, Cat_Cat_Tab_Val_Inpa.Campo_Valor_Inpa_Id, "", 5);
            try
            {
                foreach (DataRow Dr_Renglon in Tabla_Val.P_Dt_Tabla_Valores_Inpa.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "BAJA" && Dr_Renglon["VALOR_INPA_ID"].ToString().Trim().Replace("&nbsp;", "") != "")
                    {
                        Mi_sql = "DELETE " + Cat_Cat_Tab_Val_Inpa.Tabla_Cat_Cat_Tab_Val_Inpa + " WHERE " + Cat_Cat_Tab_Val_Inpa.Campo_Valor_Inpa_Id;
                        Mi_sql += "='" + Dr_Renglon["VALOR_INPA_ID"].ToString() + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Cat_Cat_Tab_Val_Inpa.Tabla_Cat_Cat_Tab_Val_Inpa + "(";
                        Mi_sql += Cat_Cat_Tab_Val_Inpa.Campo_Valor_Inpa_Id + ", ";
                        Mi_sql += Cat_Cat_Tab_Val_Inpa.Campo_Valor_Inpa + ", ";
                        Mi_sql += Cat_Cat_Tab_Val_Inpa.Campo_Anio + ", ";
                        Mi_sql += Cat_Cat_Tab_Val_Inpa.Campo_Usuario_Creo + ", ";
                        Mi_sql += Cat_Cat_Tab_Val_Inpa.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += Valor_Inpa_Id + "', ";
                        Mi_sql += Dr_Renglon["VALOR_INPA"].ToString() + ", ";
                        Mi_sql += Dr_Renglon["ANIO"].ToString() + ", '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        Valor_Inpa_Id = (Convert.ToInt16(Valor_Inpa_Id) + 1).ToString("00000");
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "ACTUALIZAR")
                    {
                        Mi_sql = "UPDATE " + Cat_Cat_Tab_Val_Inpa.Tabla_Cat_Cat_Tab_Val_Inpa;
                        Mi_sql += " SET " + Cat_Cat_Tab_Val_Inpa.Campo_Anio + " = " + Dr_Renglon["ANIO"].ToString() + ", ";
                        Mi_sql += Cat_Cat_Tab_Val_Inpa.Campo_Valor_Inpa + " = " + Dr_Renglon["VALOR_INPA"].ToString() + ", ";
                        Mi_sql += Cat_Cat_Tab_Val_Inpa.Campo_Usuario_Creo + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += Cat_Cat_Tab_Val_Inpa.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_sql += " WHERE " + Cat_Cat_Tab_Val_Inpa.Campo_Valor_Inpa_Id + "='" + Dr_Renglon["VALOR_INPA_ID"].ToString() + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Alta_Valor_Inpa: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Valores_Inpa
        ///DESCRIPCIÓN: Obtiene los datos de la Base de Datos de los valores I.n.p.a.
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 03/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Valores_Inpa(Cls_Cat_Cat_Valores_Inpa_Negocio Tabla_Val)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT " + Cat_Cat_Tab_Val_Inpa.Campo_Valor_Inpa_Id
                    + ", " + Cat_Cat_Tab_Val_Inpa.Campo_Anio
                    + ", 'NADA' AS ACCION"
                    + ", " + Cat_Cat_Tab_Val_Inpa.Campo_Valor_Inpa
                    + ", " + Cat_Cat_Tab_Val_Inpa.Campo_Usuario_Creo
                    + ", " + Cat_Cat_Tab_Val_Inpa.Campo_Fecha_Creo
                    + " FROM  " + Cat_Cat_Tab_Val_Inpa.Tabla_Cat_Cat_Tab_Val_Inpa
                    + " WHERE ";
                if (Tabla_Val.P_Valor_Inpa_Id != null && Tabla_Val.P_Valor_Inpa_Id.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Tab_Val_Inpa.Campo_Valor_Inpa_Id + " = '" + Tabla_Val.P_Valor_Inpa_Id + "' AND ";
                }
                if (Tabla_Val.P_Valor_Inpa != null && Tabla_Val.P_Valor_Inpa.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Tab_Val_Inpa.Campo_Valor_Inpa + " = " + Tabla_Val.P_Valor_Inpa + " AND ";
                }
                if (Tabla_Val.P_Anio != null && Tabla_Val.P_Anio.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Tab_Val_Inpa.Campo_Anio + " = " + Tabla_Val.P_Anio + " AND ";
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
                String Mensaje = "Consultar_Valores_Inpa: [" + Ex.Message + "]"; //"Error general en la base de datos"
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