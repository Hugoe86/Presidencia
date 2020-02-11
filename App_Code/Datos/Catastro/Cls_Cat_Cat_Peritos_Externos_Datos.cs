using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Catalogo_Cat_Peritos_Externos.Negocio;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Data;
using SharpContent.ApplicationBlocks.Data;

/// <summary>
/// Summary description for Cls_Cat_Cat_Peritos_Externos_Datos
/// </summary>

namespace Presidencia.Catalogo_Cat_Peritos_Externos.Datos
{
    public class Cls_Cat_Cat_Peritos_Externos_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Perito_Externo
        ///DESCRIPCIÓN: Modifica un Perito Externo.
        ///PARAMENTROS:     
        ///             1. Perito_Ext.      Instancia de la Clase de Negocio de Peritos Externos
        ///                                 con los datos del que van a ser
        ///                                 dados de alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 14/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Alta_Perito_Externo(Cls_Cat_Cat_Peritos_Externos_Negocio Perito_Ext)
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
            String Perito_Externo_Id = Obtener_ID_Consecutivo(Cat_Cat_Peritos_Externos.Tabla_Cat_Cat_Peritos_Externos, Cat_Cat_Peritos_Externos.Campo_Perito_Externo_Id, "", 5);
            try
            {
                Mi_sql = "INSERT INTO " + Cat_Cat_Peritos_Externos.Tabla_Cat_Cat_Peritos_Externos;
                Mi_sql += "(" + Cat_Cat_Peritos_Externos.Campo_Perito_Externo_Id + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Nombre + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Apellido_Paterno + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Apellido_Materno + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Calle + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Colonia + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Estado + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Ciudad + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Telefono + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Celular + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Usuario + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Password + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Estatus + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Observaciones + ", ";
                if (Perito_Ext.P_Fecha != null && Perito_Ext.P_Fecha.Trim() != "")
                {
                    Mi_sql += Cat_Cat_Peritos_Externos.Campo_Fecha + ", ";
                }
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Usuario_Creo + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Fecha_Creo + ") VALUES ('";
                Mi_sql += Perito_Externo_Id + "', '";
                Mi_sql += Perito_Ext.P_Nombre + "', '";
                Mi_sql += Perito_Ext.P_Apellido_Paterno + "', '";
                Mi_sql += Perito_Ext.P_Apellido_Materno + "', '";
                Mi_sql += Perito_Ext.P_Calle + "', '";
                Mi_sql += Perito_Ext.P_Colonia + "', '";
                Mi_sql += Perito_Ext.P_Estado + "', '";
                Mi_sql += Perito_Ext.P_Ciudad + "', '";
                Mi_sql += Perito_Ext.P_Telefono + "', '";
                Mi_sql += Perito_Ext.P_Celular + "', '";
                Mi_sql += Perito_Ext.P_Usuario + "', '";
                Mi_sql += Perito_Ext.P_Password + "', '";
                Mi_sql += Perito_Ext.P_Estatus + "', '";
                Mi_sql += Perito_Ext.P_Observaciones + "', '";
                if (Perito_Ext.P_Fecha != null && Perito_Ext.P_Fecha.Trim() != "")
                {
                    Mi_sql += Convert.ToDateTime(Perito_Ext.P_Fecha).ToString("d-M-yyyy") + "', '";
                }
                Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += "SYSDATE";
                Mi_sql += ")";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
                Perito_Ext.P_Perito_Externo_Id = Perito_Externo_Id;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Perito_Externo: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Perito_Externo
        ///DESCRIPCIÓN: Modifica un Perito Externo.
        ///PARAMENTROS:     
        ///             1. Perito_Ext.      Instancia de la Clase de Negocio de Peritos Externos
        ///                                 con los datos del que van a ser
        ///                                 modificados.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 14/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Modificar_Perito_Externo(Cls_Cat_Cat_Peritos_Externos_Negocio Perito_Ext)
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
                Mi_sql = "UPDATE " + Cat_Cat_Peritos_Externos.Tabla_Cat_Cat_Peritos_Externos;
                Mi_sql += " SET " + Cat_Cat_Peritos_Externos.Campo_Nombre + " = '" + Perito_Ext.P_Nombre + "', ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Apellido_Paterno + " = '" + Perito_Ext.P_Apellido_Paterno + "', ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Apellido_Materno + " = '" + Perito_Ext.P_Apellido_Materno + "', ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Calle + " = '" + Perito_Ext.P_Calle + "', ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Colonia + " = '" + Perito_Ext.P_Colonia + "', ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Estado + " = '" + Perito_Ext.P_Estado + "', ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Ciudad + " = '" + Perito_Ext.P_Ciudad+ "', ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Telefono + " = '" + Perito_Ext.P_Telefono + "', ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Celular + " = '" + Perito_Ext.P_Celular + "', ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Usuario + " = '" + Perito_Ext.P_Usuario+ "', ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Password + " = '" + Perito_Ext.P_Password + "', ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Estatus + " = '" + Perito_Ext.P_Estatus + "', ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Observaciones + " = '" + Perito_Ext.P_Observaciones+ "', ";
                if (Perito_Ext.P_Fecha != null && Perito_Ext.P_Fecha.Trim() != "")
                {
                    Mi_sql += Cat_Cat_Peritos_Externos.Campo_Fecha + " = '" + Convert.ToDateTime(Perito_Ext.P_Fecha).ToString("d-M-yyyy") + "', ";
                }
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_sql += " WHERE " + Cat_Cat_Peritos_Externos.Campo_Perito_Externo_Id + " = '" + Perito_Ext.P_Perito_Externo_Id + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Perito_Externo: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Perito_Externo
        ///DESCRIPCIÓN: Modifica un Perito Externo.
        ///PARAMENTROS:     
        ///             1. Perito_Ext.      Instancia de la Clase de Negocio de Peritos Externos
        ///                                 con los datos del que van a ser
        ///                                 modificados.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 14/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Modificar_Perito_Externo_Est(Cls_Cat_Cat_Peritos_Externos_Negocio Perito_Ext)
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

                Mi_sql = "UPDATE " + Cat_Cat_Peritos_Externos.Tabla_Cat_Cat_Peritos_Externos;
                Mi_sql += " SET " + Cat_Cat_Peritos_Externos.Campo_Estatus + " = '" + Perito_Ext.P_Estatus + "', ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_sql += " WHERE " + Cat_Cat_Peritos_Externos.Campo_Perito_Externo_Id + " = '" + Perito_Ext.P_Perito_Externo_Id + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Perito_Externo: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Baja_Perito_Externo
        ///DESCRIPCIÓN: Da de Baja un Perito Externo.
        ///PARAMENTROS:     
        ///             1. Perito_Ext.      Instancia de la Clase de Negocio de Peritos Externos
        ///                                 con los datos del que van a ser
        ///                                 modificados.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 14/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Baja_Perito_Externo(Cls_Cat_Cat_Peritos_Externos_Negocio Perito_Ext)
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

                Mi_sql = "UPDATE " + Cat_Cat_Peritos_Externos.Tabla_Cat_Cat_Peritos_Externos;
                Mi_sql += " SET " + Cat_Cat_Peritos_Externos.Campo_Estatus + " = 'BAJA', ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_sql += " WHERE " + Cat_Cat_Peritos_Externos.Campo_Perito_Externo_Id + " = '" + Perito_Ext.P_Perito_Externo_Id + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Baja_Perito_Externo: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Peritos_ExternosTemporales
        ///DESCRIPCIÓN: Obtiene la tabla de Peritos Externos temporales
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 10/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Peritos_Externos(Cls_Cat_Cat_Peritos_Externos_Negocio Perito_Externo)
        {
            DataTable Tabla = new DataTable();
            String Mi_sql = "";

            try
            {
                Mi_sql = "SELECT " + Cat_Cat_Peritos_Externos.Campo_Perito_Externo_Id + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Nombre + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Apellido_Paterno + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Apellido_Materno + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Apellido_Paterno + "||' '||" + Cat_Cat_Peritos_Externos.Campo_Apellido_Materno + "||' '||" + Cat_Cat_Peritos_Externos.Campo_Nombre + " AS PERITO_EXTERNO, ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Calle + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Colonia + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Estado + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Ciudad + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Telefono + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Celular + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Usuario + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Password + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Estatus + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Observaciones + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Fecha + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Usuario_Creo + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Fecha_Creo;
                Mi_sql += " FROM  " + Cat_Cat_Peritos_Externos.Tabla_Cat_Cat_Peritos_Externos;
                Mi_sql += " WHERE ";
                if (Perito_Externo.P_Perito_Externo_Id != null && Perito_Externo.P_Perito_Externo_Id.Trim() != "")
                {
                    Mi_sql += Cat_Cat_Peritos_Externos.Campo_Perito_Externo_Id + "='" + Perito_Externo.P_Perito_Externo_Id + "' AND ";
                }
                if (Perito_Externo.P_Usuario != null && Perito_Externo.P_Usuario.Trim() != "")
                {
                    Mi_sql += Cat_Cat_Peritos_Externos.Campo_Usuario + "='" + Perito_Externo.P_Usuario + "' AND ";
                }
                if (Perito_Externo.P_Password != null && Perito_Externo.P_Password.Trim() != "")
                {
                    Mi_sql += Cat_Cat_Peritos_Externos.Campo_Password + "='" + Perito_Externo.P_Password + "' AND ";
                }
                if (Perito_Externo.P_Estatus != null && Perito_Externo.P_Estatus.Trim() != "")
                {
                    Mi_sql += Cat_Cat_Peritos_Externos.Campo_Estatus + " " + Perito_Externo.P_Estatus + "' AND ";
                }
                if (Perito_Externo.P_Nombre != null && Perito_Externo.P_Nombre.Trim() != "")
                {
                    //Validar con el nombre completo que vendrá cargado en la variable P_Nombre.
                    Mi_sql += Cat_Cat_Peritos_Externos.Campo_Apellido_Paterno + "||' '||"+Cat_Cat_Peritos_Externos.Campo_Apellido_Materno+"||' '||"+Cat_Cat_Peritos_Externos.Campo_Nombre+" LIKE '%" + Perito_Externo.P_Nombre + "%' AND ";
                }
                if(Mi_sql.EndsWith(" AND "))
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
                String Mensaje = "Error al intentar consultar los Peritos externos del sistema. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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