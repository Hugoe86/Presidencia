using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Catalogo_Cat_Descripcion_Construccion_Rustico.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Data;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;

/// <summary>
/// Summary description for Cls_Cat_Cat_Descripcion_Construccion_Rustico_Datos
/// </summary>
namespace Presidencia.Catalogo_Cat_Descripcion_Construccion_Rustico.Datos
{
    public class Cls_Cat_Cat_Descripcion_Construccion_Rustico_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Descripcion_Construccion_Rustico
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
        public static Boolean Alta_Descripcion_Construccion_Rustico
            (Cls_Cat_Cat_Descripcion_Construccion_Rustico_Negocio Descripcion_Construccion_Rustico)
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
            String Descrip_Construc_Rustico_id = "";
            Descrip_Construc_Rustico_id = Obtener_ID_Consecutivo(Cat_Cat_Descrip_Const_Rustico.Tabla_Cat_Cat_Descrip_Const_Rustico,
                Cat_Cat_Descrip_Const_Rustico.Campo_Desc_Constru_Rustico_Id, String.Empty, 5);
            try
            {
                Mi_sql = "INSERT INTO " + Cat_Cat_Descrip_Const_Rustico.Tabla_Cat_Cat_Descrip_Const_Rustico + " (";
                Mi_sql += Cat_Cat_Descrip_Const_Rustico.Campo_Desc_Constru_Rustico_Id + ", ";
                Mi_sql += Cat_Cat_Descrip_Const_Rustico.Campo_Estatus + ", ";
                Mi_sql += Cat_Cat_Descrip_Const_Rustico.Campo_Identificador + ", ";
                Mi_sql += Cat_Cat_Descrip_Const_Rustico.Campo_Usuario_Creo + ", ";
                Mi_sql += Cat_Cat_Descrip_Const_Rustico.Campo_Fecha_Creo;
                Mi_sql += ") VALUES ('";
                Mi_sql += Descrip_Construc_Rustico_id + "', '";
                Mi_sql += Descripcion_Construccion_Rustico.P_Estatus + "', '";
                Mi_sql += Descripcion_Construccion_Rustico.P_Identificador + "', '";
                Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += "SYSDATE";
                Mi_sql += ")";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Alta = true;                
            }
            catch (Exception Ex)
            {
                Trans.Rollback();
                throw new Exception("Alta_Descripcion_Construccion_Rustico: " + Ex.Message);
            }
            Trans.Commit();
            return Alta;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Descripcion_Construccion_Rustico
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
        public static Boolean Modificar_Descripcion_Construccion_Rustico(Cls_Cat_Cat_Descripcion_Construccion_Rustico_Negocio Descripcion_Construccion_Rustico)
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
            String Mi_Sql = "";
            try
            {
                Mi_Sql = "UPDATE " + Cat_Cat_Descrip_Const_Rustico.Tabla_Cat_Cat_Descrip_Const_Rustico;
                Mi_Sql += " SET " + Cat_Cat_Descrip_Const_Rustico.Campo_Identificador + " = '" + Descripcion_Construccion_Rustico.P_Identificador + "', ";
                Mi_Sql += Cat_Cat_Descrip_Const_Rustico.Campo_Estatus + " = '" + Descripcion_Construccion_Rustico.P_Estatus + "', ";
                Mi_Sql += Cat_Cat_Descrip_Const_Rustico.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_Sql += Cat_Cat_Descrip_Const_Rustico.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_Sql += " WHERE " + Cat_Cat_Descrip_Const_Rustico.Campo_Desc_Constru_Rustico_Id + " = '" +
                    Descripcion_Construccion_Rustico.P_Descripcion_Construccion_Rustico_ID + "'";
                Cmd.CommandText = Mi_Sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Tipo_Construccion_Rustico: [" + E.Message + "].");
            }
            Trans.Commit();
            return Alta;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Descripcion_Construccion_Rustico
        ///DESCRIPCIÓN: Consulta los descripciones de construccion
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
        public static DataTable Consultar_Descripcion_Construccion_Rustico (Cls_Cat_Cat_Descripcion_Construccion_Rustico_Negocio Descripcion_Construccion_Rustico)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT " + Cat_Cat_Descrip_Const_Rustico.Campo_Desc_Constru_Rustico_Id
                + ", " + Cat_Cat_Descrip_Const_Rustico.Campo_Identificador
                + ", " + Cat_Cat_Descrip_Const_Rustico.Campo_Estatus
                + " FROM " + Cat_Cat_Descrip_Const_Rustico.Tabla_Cat_Cat_Descrip_Const_Rustico
                + " WHERE ";
                if (Descripcion_Construccion_Rustico.P_Identificador != null && Descripcion_Construccion_Rustico.P_Identificador.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Descrip_Const_Rustico.Campo_Identificador + " LIKE '%" + Descripcion_Construccion_Rustico.P_Identificador + "%' AND ";
                }
                if (Descripcion_Construccion_Rustico.P_Estatus != null && Descripcion_Construccion_Rustico.P_Estatus.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Descrip_Const_Rustico.Campo_Estatus + " = '" + Descripcion_Construccion_Rustico.P_Estatus + " AND ";
                }
                if (Descripcion_Construccion_Rustico.P_Descripcion_Construccion_Rustico_ID != null && Descripcion_Construccion_Rustico.P_Descripcion_Construccion_Rustico_ID.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Descrip_Const_Rustico.Campo_Desc_Constru_Rustico_Id + " = '" + Descripcion_Construccion_Rustico.P_Descripcion_Construccion_Rustico_ID + "' AND ";
                }
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                //agrega filtro y ordena la consulta
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Consultar_Descripcion_Construccion_Rustico: [" + Ex.Message + "]."; //"Error en la Base de Datos";
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
            Object Obj_Temp = null;
            String id = Convertir_A_Formato_ID(1, Longitud_ID);
            try
            {
                String Mi_SQL = "SELECT MAX (" + Campo + ") FROM " + Tabla;
                if (!String.IsNullOrEmpty(Filtro))
                    Mi_SQL += " WHERE " + Filtro;
                Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!Convert.IsDBNull(Obj_Temp))
                    id = Convertir_A_Formato_ID((Convert.ToInt32(Obj_Temp) + 1), Longitud_ID);
            }
            catch (Exception Ex)
            {
                new Exception(Ex.Message);
            }
            return id;
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
