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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Catalogo_Cat_Reg_Condominio.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Cat_Reg_Condominio_Datos
/// </summary>
/// 
namespace Presidencia.Catalogo_Cat_Reg_Condominio.Datos
{
    public class Cls_Cat_Cat_Reg_Condominio_Datos
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
        ///
        public static Boolean Alta_Regimen_Condominio(Cls_Cat_Cat_Reg_Condominio_Negocio Reg_Condominio)
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
            String Regimen_Condominio_Id = "";
            Regimen_Condominio_Id = Obtener_ID_Consecutivo(Cat_Cat_Reg_Condominio.Tabla_Cat_Cat_Reg_Condominio, 
                Cat_Cat_Reg_Condominio.Campo_Regimen_Condominio_ID, "", 5);
            try
            {
                Mi_Sql = "INSERT INTO " + Cat_Cat_Reg_Condominio.Tabla_Cat_Cat_Reg_Condominio + "(";
                Mi_Sql += Cat_Cat_Reg_Condominio.Campo_Regimen_Condominio_ID + ",";
                Mi_Sql += Cat_Cat_Reg_Condominio.Campo_Estatus + ",";
                Mi_Sql += Cat_Cat_Reg_Condominio.Campo_Nombre_Documento + ",";
                Mi_Sql += Cat_Cat_Reg_Condominio.Campo_Tipo + ",";
                Mi_Sql += Cat_Cat_Reg_Condominio.Campo_Usuario_Creo + ",";
                Mi_Sql += Cat_Cat_Reg_Condominio.Campo_Fecha_Creo;
                Mi_Sql += ") VALUES ('";
                Mi_Sql += Regimen_Condominio_Id + "', '";
                Mi_Sql += Reg_Condominio.P_Estatus + "', '";
                Mi_Sql += Reg_Condominio.P_Nombre_Documento + "','";
                Mi_Sql += Reg_Condominio.P_Tipo + "', '";
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
                throw new Exception("Alta_Regimen_de_Condominio:" + ex.Message);
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
        ///
        public static Boolean Modificar_Regimen_Condiminio(Cls_Cat_Cat_Reg_Condominio_Negocio Reg_Condominio)
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
                Mi_Sql = "UPDATE " + Cat_Cat_Reg_Condominio.Tabla_Cat_Cat_Reg_Condominio;
                Mi_Sql += " SET " + Cat_Cat_Reg_Condominio.Campo_Estatus + " = '"  + Reg_Condominio.P_Estatus + "', ";
                Mi_Sql += Cat_Cat_Reg_Condominio.Campo_Tipo + "= '" + Reg_Condominio.P_Tipo + "', ";
                Mi_Sql += Cat_Cat_Reg_Condominio.Campo_Nombre_Documento + "= '" + Reg_Condominio.P_Nombre_Documento + "', ";
                Mi_Sql += Cat_Cat_Reg_Condominio.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_Sql += Cat_Cat_Reg_Condominio.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_Sql += " WHERE " + Cat_Cat_Reg_Condominio.Campo_Regimen_Condominio_ID + " = '" + Reg_Condominio.P_Regimen_Condominio_ID + "'";
                Cmd.CommandText = Mi_Sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                throw new Exception ("Modifica_Regimen_Condominios: [" + ex.Message + "].");
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
        ///

        public static DataTable Consultar_Regimen_Condominio(Cls_Cat_Cat_Reg_Condominio_Negocio Reg_Condominio)
        {
            DataTable Tabla = new DataTable();
            String Mi_Sql="";
            try
            {
                Mi_Sql = "SELECT " + Cat_Cat_Reg_Condominio.Campo_Regimen_Condominio_ID
                    + ", " + Cat_Cat_Reg_Condominio.Campo_Nombre_Documento
                    + ", " + Cat_Cat_Reg_Condominio.Campo_Estatus
                    + ", " + Cat_Cat_Reg_Condominio.Campo_Tipo
                    + " FROM " + Cat_Cat_Reg_Condominio.Tabla_Cat_Cat_Reg_Condominio
                    + " WHERE ";
                if (Reg_Condominio.P_Estatus != null && Reg_Condominio.P_Estatus.Trim() != "")
                {
                    Mi_Sql += Cat_Cat_Reg_Condominio.Campo_Estatus + " = '" + Reg_Condominio.P_Estatus + "' AND ";
                }
                if (Reg_Condominio.P_Nombre_Documento != null && Reg_Condominio.P_Nombre_Documento.Trim() != "")
                {
                    Mi_Sql += Cat_Cat_Reg_Condominio.Campo_Nombre_Documento + " = '" + Reg_Condominio.P_Nombre_Documento + "' AND ";
                }
                if(Reg_Condominio.P_Tipo != null && Reg_Condominio.P_Tipo.Trim() != "")
                {
                    Mi_Sql += Cat_Cat_Reg_Condominio.Campo_Tipo + " = '" + Reg_Condominio.P_Tipo + "' AND ";
                }
                if (Reg_Condominio.P_Regimen_Condominio_ID != null && Reg_Condominio.P_Regimen_Condominio_ID.Trim() != "")
                {
                    Mi_Sql += Cat_Cat_Reg_Condominio.Campo_Regimen_Condominio_ID + " = '" + Reg_Condominio.P_Regimen_Condominio_ID+ "' AND ";
                }
                if (Mi_Sql.EndsWith(" AND "))
                {
                    Mi_Sql=Mi_Sql.Substring(0 , Mi_Sql.Length - 5);
                }
                if (Mi_Sql.EndsWith(" WHERE "))
                {
                    Mi_Sql=Mi_Sql.Substring(0, Mi_Sql.Length - 5);
                }
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                if(dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {

                String Message = "Consultar_Regimen_Condominio: [" + Ex.Message + "].";
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

