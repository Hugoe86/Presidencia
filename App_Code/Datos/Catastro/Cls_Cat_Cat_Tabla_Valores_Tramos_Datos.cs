using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Catalogo_Cat_Tabla_Valores_Tramos.Negocio;
using SharpContent.ApplicationBlocks.Data;
using System.Data;
using Presidencia.Sessiones;

/// <summary>
/// Summary description for Cls_Cat_Cat_Tabla_Valores_Tramos_Datos
/// </summary>

namespace Presidencia.Catalogo_Cat_Tabla_Valores_Tramos.Datos
{
    public class Cls_Cat_Cat_Tabla_Valores_Tramos_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Valor_Tramo
        ///DESCRIPCIÓN: Da de alta en la Base de Datos la tabla de valores par Tramos
        ///PARAMENTROS:     
        ///             1. Tabla_Val.       Instancia de la Clase de Negocio de Tabla de valores para tramos
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 10/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Alta_Valor_Tramo(Cls_Cat_Cat_Tabla_Valores_Tramos_Negocio Tabla_Val)
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
            String Valor_Tramo_Id = Obtener_ID_Consecutivo(Cat_Cat_Tabla_Valores_Tramos.Tabla_Cat_Cat_Tabla_Valores_Tramos, Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo_Id, "", 5);
            try
            {
                foreach (DataRow Dr_Renglon in Tabla_Val.P_Dt_Tabla_Valores_Tramos.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "BAJA" && Dr_Renglon["VALOR_TRAMO_ID"].ToString().Trim().Replace("&nbsp;", "") != "")
                    {
                        Mi_sql = "DELETE " + Cat_Cat_Tabla_Valores_Tramos.Tabla_Cat_Cat_Tabla_Valores_Tramos + " WHERE " + Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo_Id;
                        Mi_sql += "='" + Dr_Renglon["VALOR_TRAMO_ID"].ToString() + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Cat_Cat_Tabla_Valores_Tramos.Tabla_Cat_Cat_Tabla_Valores_Tramos + "(";
                        Mi_sql += Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo_Id + ", ";
                        Mi_sql += Cat_Cat_Tabla_Valores_Tramos.Campo_Tramo_Id + ", ";
                        Mi_sql += Cat_Cat_Tabla_Valores_Tramos.Campo_Anio + ", ";
                        Mi_sql += Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo + ", ";
                        Mi_sql += Cat_Cat_Tabla_Valores_Tramos.Campo_Usuario_Creo + ", ";
                        Mi_sql += Cat_Cat_Tabla_Valores_Tramos.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += Valor_Tramo_Id + "', '";
                        Mi_sql += Tabla_Val.P_Tramo_Id + "', ";
                        Mi_sql += Dr_Renglon["ANIO"].ToString() + ", ";
                        Mi_sql += Dr_Renglon["VALOR_TRAMO"].ToString() + ", '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        Valor_Tramo_Id = (Convert.ToInt16(Valor_Tramo_Id) + 1).ToString("00000");
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "ACTUALIZAR")
                    {
                        Mi_sql = "UPDATE " + Cat_Cat_Tabla_Valores_Tramos.Tabla_Cat_Cat_Tabla_Valores_Tramos;
                        Mi_sql += " SET " + Cat_Cat_Tabla_Valores_Tramos.Campo_Anio + " = " + Dr_Renglon["ANIO"].ToString() + ", ";
                        Mi_sql += Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo + " = " + Dr_Renglon["VALOR_TRAMO"].ToString() + ", ";
                        Mi_sql += Cat_Cat_Tabla_Valores_Tramos.Campo_Usuario_Creo + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += Cat_Cat_Tabla_Valores_Tramos.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_sql += " WHERE " + Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo_Id + "='" + Dr_Renglon["VALOR_TRAMO_ID"].ToString() + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Alta_Valor_Tramo: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Valor_Tramo
        ///DESCRIPCIÓN: Modifica en la Base de Datos la tabla de valores para tramos
        ///PARAMENTROS:     
        ///             1. Tabla_Val.       Instancia de la Clase de Negocio de Tabla de valores de Tramos
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta, eliminados y/o modificados.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 10/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Modificar_Valor_Tramo(Cls_Cat_Cat_Tabla_Valores_Tramos_Negocio Tabla_Val)
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
            String Valor_Tramo_Id = Obtener_ID_Consecutivo(Cat_Cat_Tabla_Valores_Tramos.Tabla_Cat_Cat_Tabla_Valores_Tramos, Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo_Id, "", 5);
            try
            {
                foreach (DataRow Dr_Renglon in Tabla_Val.P_Dt_Tabla_Valores_Tramos.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "BAJA" && Dr_Renglon["VALOR_TRAMO_ID"].ToString().Trim().Replace("&nbsp;", "") != "")
                    {
                        Mi_sql = "DELETE " + Cat_Cat_Tabla_Valores_Tramos.Tabla_Cat_Cat_Tabla_Valores_Tramos + " WHERE " + Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo_Id;
                        Mi_sql += "='" + Dr_Renglon["VALOR_TRAMO_ID"].ToString() + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Cat_Cat_Tabla_Valores_Tramos.Tabla_Cat_Cat_Tabla_Valores_Tramos + "(";
                        Mi_sql += Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo_Id + ", ";
                        Mi_sql += Cat_Cat_Tabla_Valores_Tramos.Campo_Tramo_Id + ", ";
                        Mi_sql += Cat_Cat_Tabla_Valores_Tramos.Campo_Anio + ", ";
                        Mi_sql += Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo + ", ";
                        Mi_sql += Cat_Cat_Tabla_Valores_Tramos.Campo_Usuario_Creo + ", ";
                        Mi_sql += Cat_Cat_Tabla_Valores_Tramos.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += Valor_Tramo_Id + "', '";
                        Mi_sql += Tabla_Val.P_Tramo_Id + "', ";
                        Mi_sql += Dr_Renglon["ANIO"].ToString() + ", ";
                        Mi_sql += Dr_Renglon["VALOR_TRAMO"].ToString() + ", '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        Valor_Tramo_Id = (Convert.ToInt16(Valor_Tramo_Id) + 1).ToString("00000");
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "ACTUALIZAR")
                    {
                        Mi_sql = "UPDATE " + Cat_Cat_Tabla_Valores_Tramos.Tabla_Cat_Cat_Tabla_Valores_Tramos;
                        Mi_sql += " SET " + Cat_Cat_Tabla_Valores_Tramos.Campo_Anio + " = " + Dr_Renglon["ANIO"].ToString() + ", ";
                        Mi_sql += Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo + " = " + Dr_Renglon["VALOR_TRAMO"].ToString() + ", ";
                        Mi_sql += Cat_Cat_Tabla_Valores_Tramos.Campo_Usuario_Creo + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += Cat_Cat_Tabla_Valores_Tramos.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_sql += " WHERE " + Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo_Id + "='" + Dr_Renglon["VALOR_TRAMO_ID"].ToString() + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Valor_Tramo: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tabla_Valores_Tramo
        ///DESCRIPCIÓN: Obtiene la tabla de valores que estan dados de alta en la Base de Datos de un Tramo de calle
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 03/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Tabla_Valores_Tramo(Cls_Cat_Cat_Tabla_Valores_Tramos_Negocio Tabla_Val)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT " + Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo_Id
                    + ", " + Cat_Cat_Tabla_Valores_Tramos.Campo_Tramo_Id
                    + ", " + Cat_Cat_Tabla_Valores_Tramos.Campo_Anio
                    + ", " + Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo
                    + ", 'NADA' AS ACCION"
                    + ", " + Cat_Cat_Tabla_Valores_Tramos.Campo_Usuario_Creo
                    + ", " + Cat_Cat_Tabla_Valores_Tramos.Campo_Fecha_Creo
                    + " FROM  " + Cat_Cat_Tabla_Valores_Tramos.Tabla_Cat_Cat_Tabla_Valores_Tramos
                    + " WHERE ";
                if (Tabla_Val.P_Valor_Tramo_Id != null && Tabla_Val.P_Valor_Tramo_Id.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo_Id + " = '" + Tabla_Val.P_Valor_Tramo_Id + "' AND ";
                }
                if (Tabla_Val.P_Tramo_Id != null && Tabla_Val.P_Tramo_Id.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Tabla_Valores_Tramos.Campo_Tramo_Id + " = '" + Tabla_Val.P_Tramo_Id + "' AND ";
                }
                if (Tabla_Val.P_Anio != null && Tabla_Val.P_Anio.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Tabla_Valores_Tramos.Campo_Anio + " = " + Tabla_Val.P_Anio + " AND ";
                }
                if (Tabla_Val.P_Valor_Tramo != null && Tabla_Val.P_Valor_Tramo.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo + " = " + Tabla_Val.P_Valor_Tramo + " AND ";
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
                String Mensaje = "Error al intentar consultar los registros de Tabla de valores para Tramos de calle. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tabla_Valores_Tramo
        ///DESCRIPCIÓN: Obtiene la tabla de valores que estan dados de alta en la Base de Datos de un Tramo de calle
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 03/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Tramos_Tabla_Valores(Cls_Cat_Cat_Tabla_Valores_Tramos_Negocio Tabla_Val)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT " + Cat_Cat_Tramos_Calles.Campo_Tramo_Id
                    + ", " + Cat_Cat_Tramos_Calles.Campo_Calle_Id
                    + ", " + Cat_Cat_Tramos_Calles.Campo_Tramo_Descripcion
                    + ", (SELECT CA."+Cat_Pre_Calles.Campo_Nombre+" FROM "+Cat_Pre_Calles.Tabla_Cat_Pre_Calles+" CA WHERE CA."+Cat_Pre_Calles.Campo_Calle_ID+"= "+Cat_Cat_Tramos_Calles.Tabla_Cat_Cat_Tramos_Calle+"."+Cat_Cat_Tramos_Calles.Campo_Calle_Id+") AS NOMBRE_CALLE"
                    + ", (SELECT CO."+Cat_Ate_Colonias.Campo_Nombre+" FROM "+Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias+" CO WHERE CO."+Cat_Ate_Colonias.Campo_Colonia_ID+"= (SELECT CA." + Cat_Pre_Calles.Campo_Colonia_ID + " FROM " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CA WHERE CA." + Cat_Pre_Calles.Campo_Calle_ID + "= " + Cat_Cat_Tramos_Calles.Tabla_Cat_Cat_Tramos_Calle + "." + Cat_Cat_Tramos_Calles.Campo_Calle_Id + ")) AS NOMBRE_COLONIA" 
                    + ", " + Cat_Cat_Tramos_Calles.Campo_Usuario_Creo
                    + ", " + Cat_Cat_Tramos_Calles.Campo_Fecha_Creo
                    + " FROM  " + Cat_Cat_Tramos_Calles.Tabla_Cat_Cat_Tramos_Calle
                    + " WHERE ";
                if (Tabla_Val.P_Tramo_Id != null && Tabla_Val.P_Tramo_Id.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Tramos_Calles.Campo_Tramo_Id + " = '" + Tabla_Val.P_Tramo_Id + "' AND ";
                }
                if (Tabla_Val.P_Descripcion_Tramo != null && Tabla_Val.P_Descripcion_Tramo.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Tramos_Calles.Campo_Tramo_Descripcion + " LIKE '%" + Tabla_Val.P_Descripcion_Tramo + "%' AND ";
                }
                if (Tabla_Val.P_Calle_Busqueda != null && Tabla_Val.P_Calle_Busqueda.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Tramos_Calles.Campo_Calle_Id + " IN (SELECT CAL." + Cat_Pre_Calles.Campo_Calle_ID + " FROM " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CAL WHERE CAL." + Cat_Pre_Calles.Campo_Nombre + " LIKE '%" + Tabla_Val.P_Calle_Busqueda + "%') AND ";
                }
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                Mi_SQL += " ORDER BY " + Cat_Cat_Tramos_Calles.Campo_Tramo_Descripcion + " DESC";
                // agregar filtro y orden a la consulta
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar Tramos de calle. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Tabla_Valores_Calculado
        ///DESCRIPCIÓN: Modifica en la Base de Datos la tabla de valores para tramos
        ///PARAMENTROS:     
        ///             1. Tabla_Val.       Instancia de la Clase de Negocio de Tabla de valores de Tramos
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta, eliminados y/o modificados.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 10/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Modificar_Tabla_Valores_Calculado(Cls_Cat_Cat_Tabla_Valores_Tramos_Negocio Tabla_Val)
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
            String Valor_Tramo_Id = Obtener_ID_Consecutivo(Cat_Cat_Tabla_Valores_Tramos.Tabla_Cat_Cat_Tabla_Valores_Tramos, Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo_Id, "", 5);
            try
            {
                foreach (DataRow Dr_Renglon in Tabla_Val.P_Dt_Tabla_Valores_Tramos.Rows)
                {                   
                    Mi_sql = "INSERT INTO " + Cat_Cat_Tabla_Valores_Tramos.Tabla_Cat_Cat_Tabla_Valores_Tramos + "(";
                    Mi_sql += Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo_Id + ", ";
                    Mi_sql += Cat_Cat_Tabla_Valores_Tramos.Campo_Tramo_Id + ", ";
                    Mi_sql += Cat_Cat_Tabla_Valores_Tramos.Campo_Anio + ", ";
                    Mi_sql += Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo + ", ";
                    Mi_sql += Cat_Cat_Tabla_Valores_Tramos.Campo_Usuario_Creo + ", ";
                    Mi_sql += Cat_Cat_Tabla_Valores_Tramos.Campo_Fecha_Creo;
                    Mi_sql += ") VALUES ('";
                    Mi_sql += Valor_Tramo_Id + "', '";
                    Mi_sql += Dr_Renglon["TRAMO_ID"] + "', ";
                    Mi_sql += Tabla_Val.P_Anio + ", ";
                    Mi_sql += Dr_Renglon["VALOR_TRAMO"].ToString() + ", '";
                    Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                    Mi_sql += "SYSDATE";
                    Mi_sql += ")";
                    Cmd.CommandText = Mi_sql;
                    Cmd.ExecuteNonQuery();
                    Valor_Tramo_Id = (Convert.ToInt16(Valor_Tramo_Id) + 1).ToString("00000");                   
                }
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Valor_Tramo: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

    }
}