using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Catalogo_Cat_Construccion_Dominante.Negocio;
using System.Data.OracleClient;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.Collections.Generic;
using SharpContent.ApplicationBlocks.Data;


/// <summary>
/// Summary description for Cls_Cat_Cat_Construccion_Dominante_Datos
/// </summary>
/// 
namespace Presidencia.Catalogo_Cat_Construccion_Dominante.Datos
{
    public class Cls_Cat_Cat_Construccion_Dominante_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Servicios_Zona
        ///DESCRIPCIÓN: Da de alta en la Base de Datos Un nuevo Servicios de Zona
        ///PARAMENTROS:     
        ///             1. Servicios_Zona.  Instancia de la Clase de Negocio de Servicios de Zona 
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 07/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Alta_Construccion_Dominante(Cls_Cat_Cat_Construccion_Dominante_Negocio Construccion)
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
            String Construccion_Id = "";
            Construccion_Id = Obtener_ID_Consecutivo(Cat_Cat_Construccion_Dominante.Tabla_Cat_Cat_Construccion_Dominante, Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante_Id, "", 5);
            try
            {
                Mi_sql = "INSERT INTO " + Cat_Cat_Construccion_Dominante.Tabla_Cat_Cat_Construccion_Dominante + "(";
                Mi_sql += Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante_Id + ", ";
                Mi_sql += Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante + ", ";
                Mi_sql += Cat_Cat_Construccion_Dominante.Campo_Estatus + ", ";
                Mi_sql += Cat_Cat_Construccion_Dominante.Campo_Usuario_Creo + ", ";
                Mi_sql += Cat_Cat_Construccion_Dominante.Campo_Fecha_Creo;
                Mi_sql += ") VALUES ('";
                Mi_sql += Construccion_Id + "', '";
                Mi_sql += Construccion.P_Construccion_Dominante + "', '";
                Mi_sql += Construccion.P_Estatus + "', '";
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
                throw new Exception("Alta_Construccion Dominante: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Servicios_Zona
        ///DESCRIPCIÓN: Modifica un motivo de rechazo
        ///PARAMENTROS:     
        ///             1. Motivo_Rechazo.  Instancia de la Clase de Negocio de Servicios deZona 
        ///                                 con los datos del que van a ser
        ///                                 modificado.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 07/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Modificar_Construccion_Dominante(Cls_Cat_Cat_Construccion_Dominante_Negocio Construccion)
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
                Mi_sql = "UPDATE " + Cat_Cat_Construccion_Dominante.Tabla_Cat_Cat_Construccion_Dominante;
                Mi_sql += " SET " + Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante + " = '" + Construccion.P_Construccion_Dominante + "', ";
                Mi_sql += Cat_Cat_Construccion_Dominante.Campo_Estatus + " = '" + Construccion.P_Estatus + "', ";
                Mi_sql += Cat_Cat_Construccion_Dominante.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += Cat_Cat_Construccion_Dominante.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_sql += " WHERE " + Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante_Id + " = '" + Construccion.P_Construccion_Dominante_ID + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Error al intentar modificar la construccion Dominante [" + E.Message + "].");
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Servicios_Zona
        ///DESCRIPCIÓN: Consulta los motivos de rechazo
        ///PARAMENTROS:     
        ///             1. Motivo_Rechazo.  Instancia de la Clase de Negocio de Motivos de Rechazo 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 07/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Construccion_Dominante(Cls_Cat_Cat_Construccion_Dominante_Negocio Construccion)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT " + Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante_Id
                    + ", " + Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante
                    + ", " + Cat_Cat_Construccion_Dominante.Campo_Estatus
                    + ", " + Cat_Cat_Construccion_Dominante.Campo_Fecha_Creo
                    + ", " + Cat_Cat_Construccion_Dominante.Campo_Usuario_Creo
                    + ", " + Cat_Cat_Construccion_Dominante.Campo_Fecha_Modifico
                    + ", " + Cat_Cat_Construccion_Dominante.Campo_Usuario_Modifico
                    + " FROM  " + Cat_Cat_Construccion_Dominante.Tabla_Cat_Cat_Construccion_Dominante
                    + " WHERE ";
                if (Construccion.P_Construccion_Dominante != null && Construccion.P_Construccion_Dominante.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante + " LIKE '%" + Construccion.P_Construccion_Dominante + "%' AND ";
                }
                if (Construccion.P_Construccion_Dominante_ID != null && Construccion.P_Construccion_Dominante_ID.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante_Id + " = '" + Construccion.P_Construccion_Dominante_ID + "' AND ";
                }
                if (Construccion.P_Estatus != null && Construccion.P_Estatus.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Construccion_Dominante.Campo_Estatus + Construccion.P_Estatus + " AND ";
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
                String Mensaje = "Error al intentar consultar los registros de Construccion Dominante. Error: [" + Ex.Message + "]."; //"Error general en la base de datos"
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