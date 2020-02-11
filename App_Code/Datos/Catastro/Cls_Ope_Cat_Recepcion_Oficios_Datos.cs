using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Operacion_Cat_Recepcion_Oficios.Negocio;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Data;
using SharpContent.ApplicationBlocks.Data;

/// <summary>
/// Summary description for Cls_Ope_Cat_Recepcion_Oficios_Datos
/// </summary>

namespace Presidencia.Operacion_Cat_Recepcion_Oficios.Datos
{
    public class Cls_Ope_Cat_Recepcion_Oficios_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Oficios
        ///DESCRIPCIÓN: Da de alta en la Base de Datos Un nuevo Oficio
        ///PARAMENTROS:     
        ///             1. Oficio.          Instancia de la Clase de Negocio de Recepcion de oficios
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Alta_Oficios(Cls_Ope_Cat_Recepcion_Oficios_Negocio Oficio)
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
            String No_Oficio = "";
            No_Oficio = Obtener_ID_Consecutivo(Ope_Cat_Recepcion_Oficios.Tabla_Ope_Cat_Recepcion_Oficios, Ope_Cat_Recepcion_Oficios.Campo_No_Oficio, "", 10);
            try
            {
                Mi_sql = "INSERT INTO " + Ope_Cat_Recepcion_Oficios.Tabla_Ope_Cat_Recepcion_Oficios + "(";
                Mi_sql += Ope_Cat_Recepcion_Oficios.Campo_No_Oficio + ", ";
                Mi_sql += Ope_Cat_Recepcion_Oficios.Campo_Fecha_Recepcion + ", ";
                Mi_sql += Ope_Cat_Recepcion_Oficios.Campo_Hora_Recepcion + ", ";
                Mi_sql += Ope_Cat_Recepcion_Oficios.Campo_Dependencia + ", ";
                Mi_sql += Ope_Cat_Recepcion_Oficios.Campo_Descripcion + ", ";
                Mi_sql += Ope_Cat_Recepcion_Oficios.Campo_Dep_Catastro + ", ";
                Mi_sql += Ope_Cat_Recepcion_Oficios.Campo_No_Oficio_Recepcion + ",";              
                Mi_sql += Ope_Cat_Recepcion_Oficios.Campo_Usuario_Creo + ", ";
                Mi_sql += Ope_Cat_Recepcion_Oficios.Campo_Fecha_Creo;
                Mi_sql += ") VALUES ('";
                Mi_sql += No_Oficio + "', '";
                Mi_sql += Oficio.P_Fecha_Recepcion.ToString("d-M-yyyy") + "', '";
                Mi_sql += Oficio.P_Hora_Recepcion + "', '";
                Mi_sql += Oficio.P_Dependencia + "', '";
                Mi_sql += Oficio.P_Descripcion + "', '";
                Mi_sql += Oficio.P_Departamento_Catastro + "', '";
                Mi_sql += Oficio.P_No_Oficio_Recepcion + "', '";                
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
                throw new Exception("Alta_Oficios: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Oficio
        ///DESCRIPCIÓN: Modifica un Oficio
        ///PARAMENTROS:     
        ///             1. Oficio.         Instancia de la Clase de Negocio de recepcion de oficios
        ///                                 con los datos del que van a ser
        ///                                 modificado.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Modificar_Oficio(Cls_Ope_Cat_Recepcion_Oficios_Negocio Oficio)
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
                Mi_sql = "UPDATE " + Ope_Cat_Recepcion_Oficios.Tabla_Ope_Cat_Recepcion_Oficios;
                Mi_sql += " SET " + Ope_Cat_Recepcion_Oficios.Campo_Dep_Catastro + " = '" + Oficio.P_Departamento_Catastro + "', ";
                Mi_sql += Ope_Cat_Recepcion_Oficios.Campo_Dependencia + " = '" + Oficio.P_Dependencia + "', ";
                Mi_sql += Ope_Cat_Recepcion_Oficios.Campo_No_Oficio_Recepcion + " = '" + Oficio.P_No_Oficio_Recepcion + "', ";
                Mi_sql += Ope_Cat_Recepcion_Oficios.Campo_Descripcion + " = '" + Oficio.P_Descripcion + "', ";
                Mi_sql += Ope_Cat_Recepcion_Oficios.Campo_Fecha_Recepcion + " = '" + Oficio.P_Fecha_Recepcion.ToString("d-M-yyyy") + "', ";
                Mi_sql += Ope_Cat_Recepcion_Oficios.Campo_Hora_Recepcion + " = '" + Oficio.P_Hora_Recepcion + "', ";
                Mi_sql += Ope_Cat_Recepcion_Oficios.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += Ope_Cat_Recepcion_Oficios.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_sql += " WHERE " + Ope_Cat_Recepcion_Oficios.Campo_No_Oficio + " = '" + Oficio.P_No_Oficio + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Oficio: [" + E.Message + "].");
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Oficio_Respuesta
        ///DESCRIPCIÓN: Modifica un Oficio
        ///PARAMENTROS:     
        ///             1. Oficio.         Instancia de la Clase de Negocio de recepcion de oficios
        ///                                 con los datos del que van a ser
        ///                                 modificado.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Modificar_Oficio_Respuesta(Cls_Ope_Cat_Recepcion_Oficios_Negocio Oficio)
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
                Mi_sql = "UPDATE " + Ope_Cat_Recepcion_Oficios.Tabla_Ope_Cat_Recepcion_Oficios;
                Mi_sql += " SET " + Ope_Cat_Recepcion_Oficios.Campo_Fecha_Respuesta + " = '" + Oficio.P_Fecha_Respuesta.ToString("d-M-yyyy") + "', ";
                Mi_sql += Ope_Cat_Recepcion_Oficios.Campo_Hora_Respuesta + " = '" + Oficio.P_Hora_Respuesta + "', ";
                Mi_sql += Ope_Cat_Recepcion_Oficios.Campo_No_Oficio_Respuesta + " = '" + Oficio.P_No_Oficio_Respuesta + "', ";
                Mi_sql += Ope_Cat_Recepcion_Oficios.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += Ope_Cat_Recepcion_Oficios.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_sql += " WHERE " + Ope_Cat_Recepcion_Oficios.Campo_No_Oficio + " = '" + Oficio.P_No_Oficio + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Oficio_Respuesta: [" + E.Message + "].");
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Oficios
        ///DESCRIPCIÓN: Consulta los oficios
        ///PARAMENTROS:     
        ///             1. Oficio.         Instancia de la Clase de Negocio de Recepcion de oficios 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Oficios(Cls_Ope_Cat_Recepcion_Oficios_Negocio Oficio)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT " + Ope_Cat_Recepcion_Oficios.Campo_No_Oficio
                    + ", " + Ope_Cat_Recepcion_Oficios.Campo_Dep_Catastro
                    + ", " + Ope_Cat_Recepcion_Oficios.Campo_Dependencia
                    + ", " + Ope_Cat_Recepcion_Oficios.Campo_Descripcion
                    + ", " + Ope_Cat_Recepcion_Oficios.Campo_No_Oficio_Recepcion
                    + ", " + Ope_Cat_Recepcion_Oficios.Campo_No_Oficio_Respuesta
                    + ", " + Ope_Cat_Recepcion_Oficios.Campo_Fecha_Recepcion
                    + ", " + Ope_Cat_Recepcion_Oficios.Campo_Hora_Recepcion
                    + ", " + Ope_Cat_Recepcion_Oficios.Campo_Fecha_Respuesta
                    + ", " + Ope_Cat_Recepcion_Oficios.Campo_Hora_Respuesta
                    + ", " + Ope_Cat_Recepcion_Oficios.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Recepcion_Oficios.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Recepcion_Oficios.Tabla_Ope_Cat_Recepcion_Oficios
                    + " WHERE ";
                if (Oficio.P_No_Oficio != null && Oficio.P_No_Oficio.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Recepcion_Oficios.Campo_No_Oficio + " = '" + Oficio.P_No_Oficio + "' AND ";
                }
                if (Oficio.P_Departamento_Catastro != null && Oficio.P_Departamento_Catastro.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Recepcion_Oficios.Campo_Dep_Catastro + " LIKE '%" + Oficio.P_Departamento_Catastro + "%' AND ";
                }
                if (Oficio.P_Dependencia != null && Oficio.P_Dependencia.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Recepcion_Oficios.Campo_Dependencia + " LIKE '%" + Oficio.P_Dependencia + "%' AND ";
                }
                if (Oficio.P_Descripcion != null && Oficio.P_Descripcion.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Recepcion_Oficios.Campo_Descripcion + " LIKE '%" + Oficio.P_Descripcion + "%' AND ";
                }
                if (Oficio.P_No_Oficio_Recepcion != null && Oficio.P_No_Oficio_Recepcion.Trim() != "")
                {
                    Mi_SQL+= Ope_Cat_Recepcion_Oficios.Campo_No_Oficio_Recepcion + " LIKE '%" + Oficio.P_No_Oficio_Recepcion + "%' AND ";
                }
                if (Oficio.P_No_Oficio_Respuesta != null && Oficio.P_No_Oficio_Respuesta.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Recepcion_Oficios.Campo_No_Oficio_Respuesta + " LIKE '%" + Oficio.P_No_Oficio_Respuesta + "%' AND ";
                }
                if (Oficio.P_Hora_Recepcion != null && Oficio.P_Hora_Recepcion.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Recepcion_Oficios.Campo_Hora_Recepcion + " LIKE '%" + Oficio.P_Hora_Recepcion + "%' AND ";
                }
                
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                Mi_SQL += " ORDER BY " + Ope_Cat_Recepcion_Oficios.Campo_Fecha_Recepcion + " DESC " ;
                // agregar filtro y orden a la consulta
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Consultar_Oficios: [" + Ex.Message + "]."; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Oficios_Avaluos
        ///DESCRIPCIÓN: Consulta los oficios
        ///PARAMENTROS:     
        ///             1. Oficio.         Instancia de la Clase de Negocio de Recepcion de oficios 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Oficios_Avaluos(Cls_Ope_Cat_Recepcion_Oficios_Negocio Oficio)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT " + Ope_Cat_Recepcion_Oficios.Campo_No_Oficio
                    + ", " + Ope_Cat_Recepcion_Oficios.Campo_Dependencia
                    + ", " + Ope_Cat_Recepcion_Oficios.Campo_Descripcion
                    + ", " + Ope_Cat_Recepcion_Oficios.Campo_No_Oficio_Recepcion
                    + ", " + Ope_Cat_Recepcion_Oficios.Campo_Fecha_Recepcion
                    + ", " + Ope_Cat_Recepcion_Oficios.Campo_Hora_Recepcion
                    + " FROM  " + Ope_Cat_Recepcion_Oficios.Tabla_Ope_Cat_Recepcion_Oficios
                    + " WHERE ";
                if (Oficio.P_No_Oficio != null && Oficio.P_No_Oficio.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Recepcion_Oficios.Campo_No_Oficio + " = '" + Oficio.P_No_Oficio + "' AND ";
                }
                if (Oficio.P_Departamento_Catastro != null && Oficio.P_Departamento_Catastro.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Recepcion_Oficios.Campo_Dep_Catastro + " LIKE '%" + Oficio.P_Departamento_Catastro + "%' AND ";
                }
                //Mi_SQL += Ope_Cat_Recepcion_Oficios.Campo_Dep_Catastro + " LIKE '%REGULARIZACION%' AND ";
                Mi_SQL += Ope_Cat_Recepcion_Oficios.Campo_Fecha_Respuesta + " IS NULL ";
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                Mi_SQL += " ORDER BY " + Ope_Cat_Recepcion_Oficios.Campo_Fecha_Recepcion + " DESC ";
                // agregar filtro y orden a la consulta
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Consultar_Oficios: [" + Ex.Message + "]."; //"Error general en la base de datos"
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
