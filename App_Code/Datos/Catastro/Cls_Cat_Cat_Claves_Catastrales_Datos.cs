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
using Oracle.DataAccess;
using System.Data.OracleClient;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Catalogo_Cat_Claves_Catastrales.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Cat_Claves_Catastrales_Datos
/// </summary>
/// 
namespace Presidencia.Catalogo_Cat_Claves_Catastrales.Datos
{
    public class Cls_Cat_Cat_Claves_Catastrales_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Claves_Catastrales
        ///DESCRIPCIÓN: Da de alta en la Base de Datos Una nueva clave catastral
        ///PARAMENTROS:     
        ///             1. Tipo_Construccion.           Instancia de la Clase de Negocio de claves catastrales
        ///                                             con los datos del que van a ser
        ///                                             dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        ///
        public static Boolean Alta_Claves_Catastrales(Cls_Cat_Cat_Claves_Catastrales_Negocio Claves_Catastrales)
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
            String Claves_Catastrales_ID = "";
            Claves_Catastrales_ID = Obtener_ID_Consecutivo(Cat_Cat_Claves_Catastrales.Tabla_Cat_Cat_Claves_Catastrales, Cat_Cat_Claves_Catastrales.Campo_Claves_Catastrales_ID, "", 5);
            try
            {
                Mi_Sql = "INSERT INTO " + Cat_Cat_Claves_Catastrales.Tabla_Cat_Cat_Claves_Catastrales + "( ";
                Mi_Sql += Cat_Cat_Claves_Catastrales.Campo_Claves_Catastrales_ID + ",";
                Mi_Sql += Cat_Cat_Claves_Catastrales.Campo_Identificador + ",";
                Mi_Sql += Cat_Cat_Claves_Catastrales.Campo_Estatus + ",";
                Mi_Sql += Cat_Cat_Claves_Catastrales.Campo_Tipo + ",";
                Mi_Sql += Cat_Cat_Claves_Catastrales.Campo_Usuario_Creo + ",";
                Mi_Sql += Cat_Cat_Claves_Catastrales.Campo_Fecha_Creo;
                Mi_Sql += ") VALUES ('";
                Mi_Sql += Claves_Catastrales_ID + "', '";
                Mi_Sql += Claves_Catastrales.P_Identificador + "','";
                Mi_Sql += Claves_Catastrales.P_Estatus + "', '";
                Mi_Sql += Claves_Catastrales.P_Tipo + "','";
                Mi_Sql += Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_Sql += "SYSDATE";
                Mi_Sql += ")";
                Cmd.CommandText = Mi_Sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                throw new Exception("Alta_Claves_Catastrales" + ex.Message);
            }
            Trans.Commit();
            return Alta;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Claves_Catastrales
        ///DESCRIPCIÓN: Modifica un concepto de clave catastral
        ///PARAMENTROS:     
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        ///
        public static Boolean Modificar_Claves_Catastrales(Cls_Cat_Cat_Claves_Catastrales_Negocio Claves_Catastraless)
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
                Mi_Sql = "UPDATE " + Cat_Cat_Claves_Catastrales.Tabla_Cat_Cat_Claves_Catastrales;
                Mi_Sql += " SET " + Cat_Cat_Claves_Catastrales.Campo_Estatus + " = '" + Claves_Catastraless.P_Estatus + "', ";
                Mi_Sql += Cat_Cat_Claves_Catastrales.Campo_Identificador + "= '" + Claves_Catastraless.P_Identificador + "', ";
                Mi_Sql += Cat_Cat_Claves_Catastrales.Campo_Tipo + "= '" + Claves_Catastraless.P_Tipo + "', ";
                Mi_Sql += Cat_Cat_Claves_Catastrales.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_Sql += Cat_Cat_Claves_Catastrales.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_Sql += " WHERE " + Cat_Cat_Claves_Catastrales.Campo_Claves_Catastrales_ID + " = '" + Claves_Catastraless.P_Claves_Catastrales_ID + "'";
                Cmd.CommandText = Mi_Sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Claves_Catastrales: [" + ex.Message + "].");
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
        

        public static DataTable Consultar_Claves_Catastrales(Cls_Cat_Cat_Claves_Catastrales_Negocio Claves_Catastrales)
        {
            DataTable Tabla = new DataTable();
            String Mi_Sql = "";
            try
            {
                Mi_Sql = "SELECT " + Cat_Cat_Claves_Catastrales.Campo_Claves_Catastrales_ID
                    + ", " + Cat_Cat_Claves_Catastrales.Campo_Estatus
                    + ", " + Cat_Cat_Claves_Catastrales.Campo_Identificador
                    + ", "+Cat_Cat_Claves_Catastrales.Campo_Tipo
                    + " FROM " + Cat_Cat_Claves_Catastrales.Tabla_Cat_Cat_Claves_Catastrales
                    + " WHERE ";
                if (Claves_Catastrales.P_Identificador != null && Claves_Catastrales.P_Identificador.Trim() != "")
                {
                    Mi_Sql += Cat_Cat_Claves_Catastrales.Campo_Identificador + " = '" + Claves_Catastrales.P_Identificador + "' AND ";
                }
                if (Claves_Catastrales.P_Estatus != null && Claves_Catastrales.P_Estatus.Trim() != "")
                {
                    Mi_Sql += Cat_Cat_Claves_Catastrales.Campo_Estatus + " = '" + Claves_Catastrales.P_Estatus + "' AND ";
                }
                if(Claves_Catastrales.P_Tipo != null && Claves_Catastrales.P_Tipo.Trim() != "")
                {
                    Mi_Sql += Cat_Cat_Claves_Catastrales.Campo_Tipo + " = '" + Claves_Catastrales.P_Tipo + "' AND ";
                }                
                if (Claves_Catastrales.P_Claves_Catastrales_ID != null && Claves_Catastrales.P_Claves_Catastrales_ID.Trim() != "")
                {
                    Mi_Sql += Cat_Cat_Claves_Catastrales.Campo_Claves_Catastrales_ID + " = '" + Claves_Catastrales.P_Claves_Catastrales_ID + "' AND ";
                }
                if (Mi_Sql.EndsWith(" AND "))
                {
                    Mi_Sql = Mi_Sql.Substring(0, Mi_Sql.Length - 5);
                }
                if (Mi_Sql.EndsWith(" WHERE "))
                {
                    Mi_Sql = Mi_Sql.Substring(0, Mi_Sql.Length - 5);
                }
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {

                String Message = "Consultar_Claves_Catastrales: [" + Ex.Message + "].";
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
        ///
        public static String Obtener_ID_Consecutivo(String Tabla, String Campo, String Filtro, Int32 Longitud_Id)
        {
            String Id = Convertir_A_Formato_ID(1, Longitud_Id);
            try
            {
                String Mi_Sql = " SELECT MAX (" + Campo + ") FROM " + Tabla;
                if (Filtro != "" && Filtro != null)
                {
                    Mi_Sql += " WHERE " + Filtro;
                }
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                {
                    Id = Convertir_A_Formato_ID((Convert.ToInt32(Obj_Temp) + 1), Longitud_Id);
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
        private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 longitud_ID)
        {
            String Retornar = "";
            String Dato = "" + Dato_ID;
            for (int Cont_Temp = Dato.Length; Cont_Temp < longitud_ID; Cont_Temp++)
            {
                Retornar = Retornar + "0";
            }
            Retornar = Retornar + Dato;
            return Retornar;
        }
    }

}

    