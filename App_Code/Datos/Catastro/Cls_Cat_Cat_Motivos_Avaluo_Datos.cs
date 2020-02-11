using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Catalogo_Cat_Motivos_Avaluo.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data;
using Presidencia.Sessiones;
using System.Data.OracleClient;
using Presidencia.Catalogo_Cat_Parametros.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Cat_Motivos_Avaluo_Datos
/// </summary>

namespace Presidencia.Catalogo_Cat_Motivos_Avaluo.Datos
{
    public class Cls_Cat_Cat_Motivos_Avaluo_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Motivo_Avaluo
        ///DESCRIPCIÓN: Da de alta en la Base de Datos Un nuevo motivo de avalúo
        ///PARAMENTROS:     
        ///             1. Motivo_Avaluo.   Instancia de la Clase de Negocio de Motivos de avalúo 
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Alta_Motivo_Avaluo(Cls_Cat_Cat_Motivos_Avaluo_Negocio Motivo_Avaluo)
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
            String Motivo_Avaluo_Id = "";
            Motivo_Avaluo_Id = Obtener_ID_Consecutivo(Cat_Cat_Motivos_Avaluo.Tabla_Cat_Cat_Motivos_Avaluo, Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Id, "", 5);
            try
            {
                Mi_sql = "INSERT INTO " + Cat_Cat_Motivos_Avaluo.Tabla_Cat_Cat_Motivos_Avaluo + "(";
                Mi_sql += Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Id + ", ";
                Mi_sql += Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Descripcion + ", ";
                Mi_sql += Cat_Cat_Motivos_Avaluo.Campo_Estatus + ", ";
                Mi_sql += Cat_Cat_Motivos_Avaluo.Campo_Usuario_Creo + ", ";
                Mi_sql += Cat_Cat_Motivos_Avaluo.Campo_Fecha_Creo;
                Mi_sql += ") VALUES ('";
                Mi_sql += Motivo_Avaluo_Id + "', '";
                Mi_sql += Motivo_Avaluo.P_Motivo_Avaluo_Descripcion + "', '";
                Mi_sql += Motivo_Avaluo.P_Estatus + "', '";
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
                throw new Exception("Alta_Motivo_Avaluo: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Motivo_Avaluo
        ///DESCRIPCIÓN: Modifica un motivo de Avaluo
        ///PARAMENTROS:     
        ///             1. Motivo_Avaluo.   Instancia de la Clase de Negocio de Motivos de Avaluo 
        ///                                 con los datos del que van a ser
        ///                                 modificado.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Modificar_Motivo_Avaluo(Cls_Cat_Cat_Motivos_Avaluo_Negocio Motivo_Avaluo)
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
                Mi_sql = "UPDATE " + Cat_Cat_Motivos_Avaluo.Tabla_Cat_Cat_Motivos_Avaluo;
                Mi_sql += " SET " + Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Descripcion + " = '" + Motivo_Avaluo.P_Motivo_Avaluo_Descripcion + "', ";
                Mi_sql += Cat_Cat_Motivos_Avaluo.Campo_Estatus + " = '" + Motivo_Avaluo.P_Estatus + "', ";
                Mi_sql += Cat_Cat_Motivos_Avaluo.Campo_Usuario_Creo + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += Cat_Cat_Motivos_Avaluo.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_sql += " WHERE " + Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Id + " = '" + Motivo_Avaluo.P_Motivo_Avaluo_ID + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Error al intentar modificar los motivos de avalúo [" + E.Message + "].");
            }
            Trans.Commit();
            return Alta;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Firmante
        ///DESCRIPCIÓN: Consulta los motivos de Avaluo
        ///PARAMENTROS: selecciona de la tabla firmantes con su puesto respectivamente     
        ///            
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Firmante()
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            try 
            {
                Mi_SQL = "SELECT " + Cat_Cat_Parametros.Campo_Firmante + " ||' - '||";
                Mi_SQL += " " + Cat_Cat_Parametros.Campo_Puesto + " AS FIRMANTE_1";
                Mi_SQL += ", " + Cat_Cat_Parametros.Campo_Firmante_2 + " ||' - '||";
                Mi_SQL += " " + Cat_Cat_Parametros.Campo_Puesto_2 + " AS FIRMANTE_2";
                Mi_SQL += " FROM " + Cat_Cat_Parametros.Tabla_Cat_Cat_Parametros;
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                // AGREGAR FILTRO Y ORDEN A LA CONSULTA.
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                } 
            }
            catch (Exception e) 
            {
                String Mensaje = "Error al intentar consultar el firmante de Avaluo. Error: [" + e.Message + "]."; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
        return Tabla;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Motivos_Avaluo
        ///DESCRIPCIÓN: Consulta los motivos de Avaluo
        ///PARAMENTROS:     
        ///             1. Motivo_Avaluo.   Instancia de la Clase de Negocio de Motivos de Avaluo 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Motivos_Avaluo(Cls_Cat_Cat_Motivos_Avaluo_Negocio Motivo_Avaluo)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT " + Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Id
                    + ", " + Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Descripcion
                    + ", " + Cat_Cat_Motivos_Avaluo.Campo_Estatus
                    + ", " + Cat_Cat_Motivos_Avaluo.Campo_Fecha_Creo
                    + ", " + Cat_Cat_Motivos_Avaluo.Campo_Usuario_Creo
                    + ", " + Cat_Cat_Motivos_Avaluo.Campo_Fecha_Modifico
                    + ", " + Cat_Cat_Motivos_Avaluo.Campo_Usuario_Modifico
                    + " FROM  " + Cat_Cat_Motivos_Avaluo.Tabla_Cat_Cat_Motivos_Avaluo
                    + " WHERE ";
                if (Motivo_Avaluo.P_Motivo_Avaluo_Descripcion != null && Motivo_Avaluo.P_Motivo_Avaluo_Descripcion.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Descripcion + " LIKE '%" + Motivo_Avaluo.P_Motivo_Avaluo_Descripcion + "%' AND ";
                }
                if (Motivo_Avaluo.P_Motivo_Avaluo_ID != null && Motivo_Avaluo.P_Motivo_Avaluo_ID.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Id + " = '" + Motivo_Avaluo.P_Motivo_Avaluo_ID + "' AND ";
                }
                if (Motivo_Avaluo.P_Estatus != null && Motivo_Avaluo.P_Estatus.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Motivos_Avaluo.Campo_Estatus + Motivo_Avaluo.P_Estatus + " AND ";
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
                String Mensaje = "Error al intentar consultar los registros de Motivos de Avaluo. Error: [" + Ex.Message + "]."; //"Error general en la base de datos"
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