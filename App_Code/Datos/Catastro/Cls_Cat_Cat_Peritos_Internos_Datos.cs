using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Catalogo_Cat_Peritos_Internos.Negocio;
using System.Data.OracleClient;
using Presidencia.Sessiones;

/// <summary>
/// Summary description for Cls_Cat_Cat_Peritos_Internos_Datos
/// </summary>

namespace Presidencia.Catalogo_Cat_Peritos_Internos.Datos
{
    public class Cls_Cat_Cat_Peritos_Internos_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Perito_Interno
        ///DESCRIPCIÓN: Da de alta un perito interno
        ///PARAMENTROS:     
        ///             1. Perito_Int.      Instancia de la Clase de Negocio de peritos internos
        ///                                 con los datos del que van a ser
        ///                                 dados de alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 12/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Alta_Perito_Interno(Cls_Cat_Cat_Peritos_Internos_Negocio Perito_Int)
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
            String Perito_Interno_Id = "";
            Perito_Interno_Id = Obtener_ID_Consecutivo(Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos, Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id, "", 5);
            try
            {

                Mi_sql = "INSERT INTO " + Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos + "(";
                Mi_sql += Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id + ", ";
                Mi_sql += Cat_Cat_Peritos_Internos.Campo_Empleado_Id + ", ";
                Mi_sql += Cat_Cat_Peritos_Internos.Campo_Estatus + ", ";
                Mi_sql += Cat_Cat_Peritos_Internos.Campo_Usuario_Creo + ", ";
                Mi_sql += Cat_Cat_Peritos_Internos.Campo_Fecha_Creo;
                Mi_sql += ") VALUES ('";
                Mi_sql += Perito_Interno_Id + "', '";
                Mi_sql += Perito_Int.P_Empleado_Id + "', '";
                Mi_sql += Perito_Int.P_Estatus + "', '";
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
                throw new Exception("Alta_Perito_Interno: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Perito_Interno
        ///DESCRIPCIÓN: Modifica el estatus de un perito interno
        ///PARAMENTROS:     
        ///             1. Perito_Int.      Instancia de la Clase de Negocio de peritos internos
        ///                                 con los datos del que van a ser
        ///                                 dados de alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 12/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Modificar_Perito_Interno(Cls_Cat_Cat_Peritos_Internos_Negocio Perito_Int)
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

                Mi_sql = "UPDATE " + Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos;
                Mi_sql += " SET " + Cat_Cat_Peritos_Internos.Campo_Estatus + " = '" + Perito_Int.P_Estatus +"', ";
                Mi_sql += Cat_Cat_Peritos_Internos.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += Cat_Cat_Peritos_Internos.Campo_Fecha_Modifico + "= SYSDATE ";
                Mi_sql += "WHERE " + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id + "='" + Perito_Int.P_Perito_Interno_Id + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Perito_Interno: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Peritos_Internos
        ///DESCRIPCIÓN: Obtiene la tabla de los Peritos Internos del sistema
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 12/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Peritos_Internos(Cls_Cat_Cat_Peritos_Internos_Negocio Perito_Int)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT PI." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id
                    + ", PI." + Cat_Cat_Peritos_Internos.Campo_Empleado_Id
                    + ", PI." + Cat_Cat_Peritos_Internos.Campo_Usuario_Creo
                    + ", PI." + Cat_Cat_Peritos_Internos.Campo_Fecha_Creo
                    + ", PI." + Cat_Cat_Peritos_Internos.Campo_Estatus
                    + ", EMP." + Cat_Empleados.Campo_Nombre
                    + ", EMP." + Cat_Empleados.Campo_Apellido_Paterno
                    + ", EMP." + Cat_Empleados.Campo_Apellido_Materno
                    + ", UPPER(EMP." + Cat_Empleados.Campo_Apellido_Paterno + ")||' '||UPPER(EMP." + Cat_Empleados.Campo_Apellido_Materno + ")||' '||UPPER(EMP." + Cat_Empleados.Campo_Nombre + ") AS EMPLEADO"
                    + ", EMP." + Cat_Empleados.Campo_Calle
                    + ", EMP." + Cat_Empleados.Campo_Colonia
                    + ", EMP." + Cat_Empleados.Campo_Ciudad
                    + ", EMP." + Cat_Empleados.Campo_RFC
                    + ", EMP." + Cat_Empleados.Campo_Estado
                    + ", EMP." + Cat_Empleados.Campo_No_Empleado
                    + ", EMP." + Cat_Empleados.Campo_Telefono_Casa
                    + ", EMP." + Cat_Empleados.Campo_Celular
                    + " FROM " + Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos + " PI"
                    + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMP"
                    + " ON PI." + Cat_Cat_Peritos_Internos.Campo_Empleado_Id + " = EMP." + Cat_Empleados.Campo_Empleado_ID
                    + " WHERE ";
                //Añadir filtros LOL...
                if (Perito_Int.P_Empleado_Id != null && Perito_Int.P_Empleado_Id.Trim() != "")
                {
                    Mi_SQL += "EMP." + Cat_Empleados.Campo_Empleado_ID + "='"+Perito_Int.P_Empleado_Id+"' AND ";
                }
                if (Perito_Int.P_Empleado_Nombre != null && Perito_Int.P_Empleado_Nombre.Trim() != "")
                {
                    Mi_SQL += "UPPER(EMP." + Cat_Empleados.Campo_Apellido_Paterno + ")||' '||UPPER(EMP." + Cat_Empleados.Campo_Apellido_Materno + ")||' '||UPPER(EMP." + Cat_Empleados.Campo_Nombre + ") LIKE '%" + Perito_Int.P_Empleado_Nombre + "%' AND ";
                }
                if (Perito_Int.P_Perito_Interno_Id != null && Perito_Int.P_Perito_Interno_Id.Trim() != "")
                {
                    Mi_SQL += "PI." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id + "='" + Perito_Int.P_Perito_Interno_Id + "' AND ";
                }
                if (Perito_Int.P_Estatus != null && Perito_Int.P_Estatus.Trim() != "")
                {
                    Mi_SQL += "PI." + Cat_Empleados.Campo_Estatus + " " + Perito_Int.P_Estatus + " AND ";
                }
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Empleados
        ///DESCRIPCIÓN: Obtiene la tabla de los parámetros del sistema
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 10/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Empleados(Cls_Cat_Cat_Peritos_Internos_Negocio Empleado)
        {
            DataTable Tabla = new DataTable();
            String Mi_sql = "";

            try
            {
                Mi_sql = "SELECT ";
                Mi_sql += Cat_Empleados.Campo_Nombre + ", ";
                Mi_sql += Cat_Empleados.Campo_Empleado_ID + ", ";
                Mi_sql += Cat_Empleados.Campo_Apellido_Paterno + ", ";
                Mi_sql += Cat_Empleados.Campo_Apellido_Materno + ", ";
                Mi_sql += Cat_Empleados.Campo_Apellido_Paterno + "||' '||" + Cat_Empleados.Campo_Apellido_Materno + "||' '||" + Cat_Empleados.Campo_Nombre + " AS EMPLEADO, ";
                Mi_sql += Cat_Empleados.Campo_Calle + ", ";
                Mi_sql += Cat_Empleados.Campo_Colonia + ", ";
                Mi_sql += Cat_Empleados.Campo_Estado + ", ";
                Mi_sql += Cat_Empleados.Campo_Ciudad + ", ";
                Mi_sql += Cat_Empleados.Campo_Telefono_Casa + ", ";
                Mi_sql += Cat_Empleados.Campo_Celular + ", ";
                Mi_sql += Cat_Empleados.Campo_RFC + ", ";
                Mi_sql += Cat_Empleados.Campo_No_Empleado + ", ";
                Mi_sql += Cat_Empleados.Campo_Password + ", ";
                Mi_sql += Cat_Empleados.Campo_Estatus + ", ";
                Mi_sql += Cat_Empleados.Campo_Usuario_Creo + ", ";
                Mi_sql += Cat_Empleados.Campo_Fecha_Creo;
                Mi_sql += " FROM  " + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_sql += " WHERE ";
                if (Empleado.P_Empleado_Id != null && Empleado.P_Empleado_Id.Trim() != "")
                {
                    Mi_sql += Cat_Empleados.Campo_Empleado_ID + "='" + Empleado.P_Empleado_Id + "' AND ";
                }
                if (Mi_sql.EndsWith(" AND "))
                {
                    Mi_sql = Mi_sql.Substring(0, Mi_sql.Length - 5);
                }
                if (Mi_sql.EndsWith(" WHERE "))
                {
                    Mi_sql = Mi_sql.Substring(0, Mi_sql.Length - 7);
                }
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_sql);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los datos del empleado. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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