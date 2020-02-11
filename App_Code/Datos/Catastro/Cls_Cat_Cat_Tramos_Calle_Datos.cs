using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Cat_Cat_Tramos_Calle.Negocio;
using System.Data;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Data.OracleClient;

/// <summary>
/// Summary description for Cls_Cat_Cat_Tramos_Calle_Datos
/// </summary>

namespace Presidencia.Cat_Cat_Tramos_Calle.Datos
{
    public class Cls_Cat_Cat_Tramos_Calle_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Tramos
        ///DESCRIPCIÓN: Da de alta en la Base de Datos los nuevos Tramos de calle
        ///PARAMENTROS:     
        ///             1. Tramo.           Instancia de la Clase de Negocio de Tramos 
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 03/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Alta_Tramos(Cls_Cat_Cat_Tramos_Calle_Negocio Tramo)
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
            String Tramo_Id = "";
            Tramo_Id = Obtener_ID_Consecutivo(Cat_Cat_Tramos_Calles.Tabla_Cat_Cat_Tramos_Calle, Cat_Cat_Tramos_Calles.Campo_Tramo_Id, "", 5);
            try
            {
                foreach (DataRow Dr_Renglon in Tramo.P_Dt_Tramos.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "BAJA" && Dr_Renglon["TRAMO_ID"].ToString().Trim().Replace("&nbsp;","") != "")
                    {
                        Mi_sql = "DELETE " + Cat_Cat_Tramos_Calles.Tabla_Cat_Cat_Tramos_Calle + " WHERE " + Cat_Cat_Tramos_Calles.Campo_Tramo_Id;
                        Mi_sql += "='" + Dr_Renglon["TRAMO_ID"].ToString() + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO "+ Cat_Cat_Tramos_Calles.Tabla_Cat_Cat_Tramos_Calle + "(";
                        Mi_sql += Cat_Cat_Tramos_Calles.Campo_Tramo_Id + ", ";
                        Mi_sql += Cat_Cat_Tramos_Calles.Campo_Tramo_Descripcion + ", ";
                        Mi_sql += Cat_Cat_Tramos_Calles.Campo_Calle_Id + ", ";
                        Mi_sql += Cat_Cat_Tramos_Calles.Campo_Usuario_Creo + ", ";
                        Mi_sql += Cat_Cat_Tramos_Calles.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql +=  Tramo_Id + "', '";
                        Mi_sql += Dr_Renglon["TRAMO_DESCRIPCION"].ToString() + "', '";
                        Mi_sql += Tramo.P_Calle_ID + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        Tramo_Id = (Convert.ToInt16(Tramo_Id) + 1).ToString("00000");
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "ACTUALIZAR")
                    {
                        Mi_sql = "UPDATE " + Cat_Cat_Tramos_Calles.Tabla_Cat_Cat_Tramos_Calle;
                        Mi_sql += " SET " + Cat_Cat_Tramos_Calles.Campo_Tramo_Descripcion + " = '" + Dr_Renglon["TRAMO_DESCRIPCION"].ToString() + "', ";
                        Mi_sql += Cat_Cat_Tramos_Calles.Campo_Usuario_Creo + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += Cat_Cat_Tramos_Calles.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_sql += " WHERE " + Cat_Cat_Tramos_Calles.Campo_Tramo_Id + "='" + Dr_Renglon["TRAMO_ID"].ToString() + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }
                Alta = true;
            }
            catch(Exception E)
            {
                Trans.Rollback();
                throw new Exception("Alta_Tramos: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Tramos
        ///DESCRIPCIÓN: Modifica en la Base de Datos los Tramos de calle ya sea eliminando, agregando o modificando
        ///PARAMENTROS:     
        ///             1. Tramo.           Instancia de la Clase de Negocio de Tramos 
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta, eliminados o modificados.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 03/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Modificar_Tramos(Cls_Cat_Cat_Tramos_Calle_Negocio Tramo)
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
            String Tramo_Id = "";
            Tramo_Id = Obtener_ID_Consecutivo(Cat_Cat_Tramos_Calles.Tabla_Cat_Cat_Tramos_Calle, Cat_Cat_Tramos_Calles.Campo_Tramo_Id, "", 5);
            try
            {
                foreach (DataRow Dr_Renglon in Tramo.P_Dt_Tramos.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "BAJA" && Dr_Renglon["TRAMO_ID"].ToString().Trim().Replace("&nbsp;", "") != "")
                    {
                        Mi_sql = "DELETE " + Cat_Cat_Tramos_Calles.Tabla_Cat_Cat_Tramos_Calle + " WHERE " + Cat_Cat_Tramos_Calles.Campo_Tramo_Id;
                        Mi_sql += "='" + Dr_Renglon["TRAMO_ID"].ToString() + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Cat_Cat_Tramos_Calles.Tabla_Cat_Cat_Tramos_Calle + "(";
                        Mi_sql += Cat_Cat_Tramos_Calles.Campo_Tramo_Id + ", ";
                        Mi_sql += Cat_Cat_Tramos_Calles.Campo_Tramo_Descripcion + ", ";
                        Mi_sql += Cat_Cat_Tramos_Calles.Campo_Calle_Id + ", ";
                        Mi_sql += Cat_Cat_Tramos_Calles.Campo_Usuario_Creo + ", ";
                        Mi_sql += Cat_Cat_Tramos_Calles.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += Tramo_Id + "', '";
                        Mi_sql += Dr_Renglon["TRAMO_DESCRIPCION"].ToString() + "', '";
                        Mi_sql += Tramo.P_Calle_ID + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        Tramo_Id = (Convert.ToInt16(Tramo_Id) + 1).ToString("00000");
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "ACTUALIZAR")
                    {
                        Mi_sql = "UPDATE " + Cat_Cat_Tramos_Calles.Tabla_Cat_Cat_Tramos_Calle;
                        Mi_sql += " SET " + Cat_Cat_Tramos_Calles.Campo_Tramo_Descripcion + " = '" + Dr_Renglon["TRAMO_DESCRIPCION"].ToString() + "', ";
                        Mi_sql += Cat_Cat_Tramos_Calles.Campo_Usuario_Creo + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += Cat_Cat_Tramos_Calles.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_sql += " WHERE " + Cat_Cat_Tramos_Calles.Campo_Tramo_Id + " = '" + Dr_Renglon["TRAMO_ID"].ToString() + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception(E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Calles
        ///DESCRIPCIÓN: Obtiene todas las Calles que estan dados de alta en la Base de Datos
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 03/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Calles(Cls_Cat_Cat_Tramos_Calle_Negocio Tramo)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT C." + Cat_Pre_Calles.Campo_Calle_ID
                    + " AS CALLE_ID, CC." + Cat_Ate_Colonias.Campo_Nombre + " AS COLONIA"
                    + ", CC." + Cat_Ate_Colonias.Campo_Colonia_ID
                    + ", C." + Cat_Pre_Calles.Campo_Clave
                    + ", C." + Cat_Pre_Calles.Campo_Estatus
                    + ", C." + Cat_Pre_Calles.Campo_Nombre
                    + ", NVL(C." + Cat_Pre_Calles.Campo_Comentarios + ", ' ') AS COMENTARIOS"
                    + " FROM  " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles
                    + " C JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias
                    + " CC ON " + "C." + Cat_Pre_Calles.Campo_Colonia_ID + " = CC." + Cat_Ate_Colonias.Campo_Colonia_ID
                    + " WHERE ";
                if (Tramo.P_Calle_Busqueda != null && Tramo.P_Calle_Busqueda.Trim() != "")
                {
                    Mi_SQL += "C." + Cat_Pre_Calles.Campo_Nombre + " LIKE '%" + Tramo.P_Calle_Busqueda + "%' AND ";
                }
                if (Tramo.P_Colonia_Busqueda != null && Tramo.P_Colonia_Busqueda.Trim() != "")
                {
                    Mi_SQL += "CC." + Cat_Ate_Colonias.Campo_Nombre + " LIKE '%" + Tramo.P_Colonia_Busqueda + "%' AND ";
                }
                if (Tramo.P_Calle_ID != null && Tramo.P_Calle_ID.Trim() != "")
                {
                    Mi_SQL += "C." + Cat_Pre_Calles.Campo_Calle_ID + " = '" + Tramo.P_Calle_ID + "'";
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
                String Mensaje = "Error al intentar consultar los registros de Calles. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tramos
        ///DESCRIPCIÓN: Obtiene todas las Calles que estan dados de alta en la Base de Datos
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 03/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Tramos(Cls_Cat_Cat_Tramos_Calle_Negocio Tramo)
        {
            DataTable Dt_Tramos = new DataTable();
            String My_Sql = "";
            try
            {
                My_Sql = "SELECT " + Cat_Cat_Tramos_Calles.Campo_Tramo_Descripcion + ", " + Cat_Cat_Tramos_Calles.Campo_Tramo_Id + ", 'NADA' AS ACCION FROM " + Cat_Cat_Tramos_Calles.Tabla_Cat_Cat_Tramos_Calle;
                if (Tramo.P_Calle_ID != null && Tramo.P_Calle_ID != "")
                {
                    My_Sql += " WHERE " + Cat_Cat_Tramos_Calles.Campo_Calle_Id + "='" + Tramo.P_Calle_ID + "'";
                }
                DataSet Ds_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, My_Sql);
                if (Ds_Tabla != null)
                {
                    Dt_Tramos = Ds_Tabla.Tables[0];
                }
            }
            catch(Exception E)
            {
                throw new Exception("Consultar_Tramos: Error al consultar los Tramos.");
            }
            return Dt_Tramos;
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