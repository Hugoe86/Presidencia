using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Catalogo_Cat_Tipos_Construccion.Negocio;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Data;
using SharpContent.ApplicationBlocks.Data;

/// <summary>
/// Summary description for Cls_Cat_Cat_Tipos_Construccion_Datos
/// </summary>

namespace Presidencia.Catalogo_Cat_Tipos_Construccion.Datos
{
    public class Cls_Cat_Cat_Tipos_Construccion_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Tipo_Construccion
        ///DESCRIPCIÓN: Da de alta en la Base de Datos Un nuevo Tipo de Construccion
        ///PARAMENTROS:     
        ///             1. Tipo_Construccion.           Instancia de la Clase de Negocio de tipos de construcción
        ///                                             con los datos del que van a ser
        ///                                             dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Alta_Tipo_Construccion(Cls_Cat_Cat_Tipos_Construccion_Negocio Tipo_Construccion)
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
            String Tipo_Construccion_Id = "";
            Tipo_Construccion_Id = Obtener_ID_Consecutivo(Cat_Cat_Tipos_Construccion.Tabla_Cat_Cat_Tipos_Construccion, Cat_Cat_Tipos_Construccion.Campo_Tipo_Construccion_Id, "", 5);
            try
            {
                Mi_sql = "INSERT INTO " + Cat_Cat_Tipos_Construccion.Tabla_Cat_Cat_Tipos_Construccion + "(";
                Mi_sql += Cat_Cat_Tipos_Construccion.Campo_Tipo_Construccion_Id + ", ";
                Mi_sql += Cat_Cat_Tipos_Construccion.Campo_Identificador + ", ";
                Mi_sql += Cat_Cat_Tipos_Construccion.Campo_Estatus + ", ";
                Mi_sql += Cat_Cat_Tipos_Construccion.Campo_Usuario_Creo + ", ";
                Mi_sql += Cat_Cat_Tipos_Construccion.Campo_Fecha_Creo;
                Mi_sql += ") VALUES ('";
                Mi_sql += Tipo_Construccion_Id + "', '";
                Mi_sql += Tipo_Construccion.P_Identificador + "', '";
                Mi_sql += Tipo_Construccion.P_Estatus + "', '";
                Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += "SYSDATE";
                Mi_sql += ")";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Alta_Tipo_Construccion: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Tipo_Construccion
        ///DESCRIPCIÓN: Modifica un concepto de Tipo de construccion
        ///PARAMENTROS:     
        ///             1. Tipo_Construccion.         Instancia de la Clase de Negocio de calidad de construccion
        ///                                         con los datos del que van a ser
        ///                                         modificado.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Modificar_Tipo_Construccion(Cls_Cat_Cat_Tipos_Construccion_Negocio Tipo_Construccion)
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
                Mi_sql = "UPDATE " + Cat_Cat_Tipos_Construccion.Tabla_Cat_Cat_Tipos_Construccion;
                Mi_sql += " SET " + Cat_Cat_Tipos_Construccion.Campo_Identificador + " = '" + Tipo_Construccion.P_Identificador + "', ";
                Mi_sql += Cat_Cat_Tipos_Construccion.Campo_Estatus + " = '" + Tipo_Construccion.P_Estatus + "', ";
                Mi_sql += Cat_Cat_Tipos_Construccion.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += Cat_Cat_Tipos_Construccion.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_sql += " WHERE " + Cat_Cat_Tipos_Construccion.Campo_Tipo_Construccion_Id + " = '" + Tipo_Construccion.P_Tipo_Construccion_Id + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Tipo_Construccion: [" + E.Message + "].");
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Calidad_Construccion
        ///DESCRIPCIÓN: Consulta los motivos de Avaluo
        ///PARAMENTROS:     
        ///             1. Calidad.         Instancia de la Clase de Negocio de Calidad de construccion 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Tipos_Construccion(Cls_Cat_Cat_Tipos_Construccion_Negocio Tipo_Construccion)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT " + Cat_Cat_Tipos_Construccion.Campo_Tipo_Construccion_Id
                    + ", " + Cat_Cat_Tipos_Construccion.Campo_Identificador
                    + ", " + Cat_Cat_Tipos_Construccion.Campo_Estatus
                    + ", " + Cat_Cat_Tipos_Construccion.Campo_Usuario_Creo
                    + ", " + Cat_Cat_Tipos_Construccion.Campo_Fecha_Creo
                    + " FROM  " + Cat_Cat_Tipos_Construccion.Tabla_Cat_Cat_Tipos_Construccion
                    + " WHERE ";
                if (Tipo_Construccion.P_Identificador != null && Tipo_Construccion.P_Identificador.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Tipos_Construccion.Campo_Identificador + " LIKE '%" + Tipo_Construccion.P_Identificador + "%' AND ";
                }
                if (Tipo_Construccion.P_Estatus != null && Tipo_Construccion.P_Estatus.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Tipos_Construccion.Campo_Estatus + " " + Tipo_Construccion.P_Estatus + " AND ";
                }
                if (Tipo_Construccion.P_Tipo_Construccion_Id != null && Tipo_Construccion.P_Tipo_Construccion_Id.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Tipos_Construccion.Campo_Tipo_Construccion_Id + " = '" + Tipo_Construccion.P_Tipo_Construccion_Id + "' AND ";
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
                String Mensaje = "Consultar_Tipos_Construccion: [" + Ex.Message + "]."; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tipos_Construccion_Uso
        ///DESCRIPCIÓN: Consulta los Tipos de Construcciones
        ///PARAMENTROS:     
        ///             1. Tipo_Construccion.           Instancia de la Clase de Negocio de los tipos de construcción 
        ///                                             con los datos que servirán de
        ///                                             filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 18/Diciembre/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Tipos_Construccion_Uso(Cls_Cat_Cat_Tipos_Construccion_Negocio Tipo_Construccion)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT " + Cat_Cat_Tipos_Construccion.Campo_Tipo_Construccion_Id
                    + ", " + Cat_Cat_Tipos_Construccion.Campo_Identificador
                    + ", " + Cat_Cat_Tipos_Construccion.Campo_Estatus
                    + ", " + Cat_Cat_Tipos_Construccion.Campo_Usuario_Creo
                    + ", " + Cat_Cat_Tipos_Construccion.Campo_Fecha_Creo
                    + " FROM  " + Cat_Cat_Tipos_Construccion.Tabla_Cat_Cat_Tipos_Construccion
                    + " WHERE ";
                if (Tipo_Construccion.P_Identificador != null && Tipo_Construccion.P_Identificador.Trim() != "")
                {
                    Mi_SQL += "TRIM(" + Cat_Cat_Tipos_Construccion.Campo_Identificador + ") <> '" + Tipo_Construccion.P_Identificador + "' AND ";
                }
                if (Tipo_Construccion.P_Estatus != null && Tipo_Construccion.P_Estatus.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Tipos_Construccion.Campo_Estatus + " " + Tipo_Construccion.P_Estatus + " AND ";
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
                String Mensaje = "Consultar_Tipos_Construccion: [" + Ex.Message + "]."; //"Error general en la base de datos"
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