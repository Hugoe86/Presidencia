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
using Presidencia.Constantes;
using Presidencia.Operacion_Cat_Digitalizacion_Avaluos.Negocio;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Sessiones;

/// <summary>
/// Summary description for Cls_Ope_Cat_Digitalizacion_Avaluos_Datos
/// </summary>
/// 
namespace Presidencia.Operacion_Cat_Digitalizacion_Avaluos.Datos
{
    public class Cls_Ope_Cat_Digitalizacion_Avaluos_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Documentos_Cuenta_Predial
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
        public static Boolean Alta_Documentos_Cuenta_Predial(Cls_Ope_Cat_Digitalizacion_Avaluos_Negocio Documentos)
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
            String Digitalizacion_Avaluo_Id = "";
            Digitalizacion_Avaluo_Id = Obtener_ID_Consecutivo(Ope_Cat_Digitalizacion_Avaluos.Tabla_Ope_Cat_Digitalizacion_Avaluos, Ope_Cat_Digitalizacion_Avaluos.Campo_Digitalizacion_Avaluo_Id, "", 10);

            try
            {
                foreach (DataRow Dr_Renglon in Documentos.P_Dt_Archivos.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Digitalizacion_Avaluos.Tabla_Ope_Cat_Digitalizacion_Avaluos + "(";
                        Mi_sql += Ope_Cat_Digitalizacion_Avaluos.Campo_Digitalizacion_Avaluo_Id + ", ";
                        Mi_sql += Ope_Cat_Digitalizacion_Avaluos.Campo_Cuenta_Predial_Id + ", ";
                        Mi_sql += Ope_Cat_Digitalizacion_Avaluos.Campo_Ruta_Documento + ", ";
                        Mi_sql += Ope_Cat_Digitalizacion_Avaluos.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Digitalizacion_Avaluos.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += Digitalizacion_Avaluo_Id + "', '";
                        Mi_sql += Documentos.P_Cuenta_Predial_Id + "', '";
                        Mi_sql += "../Catastro/Avaluos_Digitales/" + Documentos.P_Cuenta_Predial + "/" + Dr_Renglon[Ope_Cat_Digitalizacion_Avaluos.Campo_Ruta_Documento].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        Digitalizacion_Avaluo_Id = (Convert.ToInt32(Digitalizacion_Avaluo_Id) + 1).ToString("0000000000");
                    }                    
                }
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Alta_Documentos: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Documentos_Cuenta_Predial
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
        public static DataTable Consultar_Documentos_Cuenta_Predial(Cls_Ope_Cat_Digitalizacion_Avaluos_Negocio Cuenta_Predial)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT " + Ope_Cat_Digitalizacion_Avaluos.Campo_Digitalizacion_Avaluo_Id
                    + ", " + Ope_Cat_Digitalizacion_Avaluos.Campo_Cuenta_Predial_Id                    
                    + ", " + Ope_Cat_Digitalizacion_Avaluos.Campo_Ruta_Documento
                    + ", " + Ope_Cat_Digitalizacion_Avaluos.Campo_Fecha_Creo
                    + ", " + Ope_Cat_Digitalizacion_Avaluos.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Digitalizacion_Avaluos.Campo_Fecha_Modifico
                    + ", " + Ope_Cat_Digitalizacion_Avaluos.Campo_Usuario_Modifico
                    + ", 'NADA' AS ACCION"
                    + " FROM  " + Ope_Cat_Digitalizacion_Avaluos.Tabla_Ope_Cat_Digitalizacion_Avaluos
                    + " WHERE ";
                if (Cuenta_Predial.P_Cuenta_Predial_Id != null && Cuenta_Predial.P_Cuenta_Predial_Id.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Digitalizacion_Avaluos.Campo_Cuenta_Predial_Id + " = '" + Cuenta_Predial.P_Cuenta_Predial_Id + "' AND ";
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
                String Mensaje = "Error al intentar consultar los documentos. Error: [" + Ex.Message + "]."; //"Error general en la base de datos"
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