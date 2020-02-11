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
using Presidencia.Sessiones;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Catalogo_Cat_Calendario_Entregas.Negocio;

using System.Collections.Generic;



/// <summary>
/// Summary description for Cat_Cat_Calendario_Entregas_Datos
/// </summary>
/// 
namespace Presidencia.Catalogo_Cat_Calendario_Entregas.Datos
{
    public class Cls_Cat_Cat_Calendario_Entregas_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tabla_Calendario_Entregas
        ///DESCRIPCIÓN: Obtiene la tabla de valores que estan dados de alta en la Base de Datos de una Fecha
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 03/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Calendario_Entregas(Cls_Cat_Cat_Calendario_Entregas_Negocio Calendario_Entregas)
        {
            DataTable Tabla = new DataTable();
            String Mi_Sql = "";
            try
            {
                Mi_Sql = "SELECT " + Cat_Cat_Calendario_Entregas.Campo_Fecha_Entrega_Id
                    + ", " + Cat_Cat_Calendario_Entregas.Campo_Anio
                    + ", " + Cat_Cat_Calendario_Entregas.Campo_Fecha_Primera_Entrega
                    + ", " + Cat_Cat_Calendario_Entregas.Campo_Fecha_Primera_Entrega_Real
                    + ", " + Cat_Cat_Calendario_Entregas.Campo_Fecha_Segunda_Entrega
                    + ", " + Cat_Cat_Calendario_Entregas.Campo_Fecha_Segunda_Entrega_Real
                    + ", " + Cat_Cat_Calendario_Entregas.Campo_Fecha_Tercera_Entrega
                    + ", " + Cat_Cat_Calendario_Entregas.Campo_Fecha_Tercera_Entrega_Real
                    + ", " + Cat_Cat_Calendario_Entregas.Campo_Fecha_Cuarta_Entrega
                    + ", " + Cat_Cat_Calendario_Entregas.Campo_Fecha_Cuarta_Entrega_Real
                    + ", " + Cat_Cat_Calendario_Entregas.Campo_Fecha_Quinta_Entrega
                    + ", " + Cat_Cat_Calendario_Entregas.Campo_Fecha_Quinta_Entrega_Real
                    + ", " + Cat_Cat_Calendario_Entregas.Campo_Fecha_Sexta_Entrega
                    + ", " + Cat_Cat_Calendario_Entregas.Campo_Fecha_Sexta_Entrega_Real
                    + ", " + Cat_Cat_Calendario_Entregas.Campo_Fecha_Septima_Entrega
                    + ", " + Cat_Cat_Calendario_Entregas.Campo_Fecha_Septima_Entrega_Real
                    + ", " + Cat_Cat_Calendario_Entregas.Campo_Usuario_Creo
                    + ", " + Cat_Cat_Calendario_Entregas.Campo_Fecha_Creo
                    + ", " + Cat_Cat_Calendario_Entregas.Campo_Usuario_Modifico
                    + ", " + Cat_Cat_Calendario_Entregas.Campo_Fecha_Modifico
                    + " FROM " + Cat_Cat_Calendario_Entregas.Tabla_Cat_Cat_Calendario_Entregas
                    + " WHERE ";

                if (Calendario_Entregas.P_Fecha_Entrega_Id != null && Calendario_Entregas.P_Fecha_Entrega_Id.Trim() != "")
                {
                    Mi_Sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Entrega_Id + " = '" + Calendario_Entregas.P_Fecha_Entrega_Id + "' AND ";
                }
                if (Calendario_Entregas.P_Anio != null && Calendario_Entregas.P_Anio.Trim() != "")
                {
                    Mi_Sql += Cat_Cat_Calendario_Entregas.Campo_Anio + " = " + Calendario_Entregas.P_Anio + " AND ";
                }

                if (Mi_Sql.EndsWith(" AND "))
                {
                    Mi_Sql = Mi_Sql.Substring(0, Mi_Sql.Length - 5);
                }
                if (Mi_Sql.EndsWith(" WHERE "))
                {
                    Mi_Sql = Mi_Sql.Substring(0, Mi_Sql.Length - 7);
                }
                Mi_Sql += " ORDER BY " + Cat_Cat_Calendario_Entregas.Campo_Anio + " DESC ";

                // agregar filtro y orden a la consulta
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar las Fechas de calendario. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Calendario_Entregas
        ///DESCRIPCIÓN: Da de alta en la Base de Datos los valores de las cuotas
        ///PARAMENTROS:     
        ///             
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Alta_Calendario_Entregas(Cls_Cat_Cat_Calendario_Entregas_Negocio Calendario_Entregas)
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
            String Fecha_Entrega_Id = "";
            Fecha_Entrega_Id = Obtener_ID_Consecutivo(Cat_Cat_Calendario_Entregas.Tabla_Cat_Cat_Calendario_Entregas, Cat_Cat_Calendario_Entregas.Campo_Fecha_Entrega_Id, "", 5);
            try
            {
                Mi_sql = " INSERT INTO " + Cat_Cat_Calendario_Entregas.Tabla_Cat_Cat_Calendario_Entregas + " (";
                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Entrega_Id + ", ";
                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Anio + ", ";
                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Primera_Entrega + ", ";
                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Primera_Entrega_Real + ", ";
                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Segunda_Entrega + ", ";
                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Segunda_Entrega_Real + ", ";
                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Tercera_Entrega + ", ";
                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Tercera_Entrega_Real + ", ";
                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Cuarta_Entrega + ", ";
                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Cuarta_Entrega_Real + ", ";
                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Quinta_Entrega + ", ";
                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Quinta_Entrega_Real + ", ";
                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Sexta_Entrega + ", ";
                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Sexta_Entrega_Real + ", ";
                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Septima_Entrega + ", ";
                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Septima_Entrega_Real + ", ";
                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Usuario_Creo + ", ";
                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Creo;
                Mi_sql += ") VALUES (";
                Mi_sql += "'" + Fecha_Entrega_Id + "', ";
                Mi_sql += Calendario_Entregas.P_Anio + ", ";
                Mi_sql += "'" + Calendario_Entregas.P_Fecha_Primera_Entrega + "', ";

                if (Calendario_Entregas.P_Fecha_Primera_Entrega_Real != null && Calendario_Entregas.P_Fecha_Primera_Entrega_Real.Trim() != "")
                {
                    Mi_sql += "'" + Calendario_Entregas.P_Fecha_Primera_Entrega_Real + "', ";
                }
                else {
                    Mi_sql += "null" + ", ";
                }
                Mi_sql += "'" + Calendario_Entregas.P_Fecha_Segunda_Entrega + "', ";
                if (Calendario_Entregas.P_Fecha_Segunda_Entrega_Real != null && Calendario_Entregas.P_Fecha_Segunda_Entrega_Real.Trim() != "")
                {
                    Mi_sql += "'" + Calendario_Entregas.P_Fecha_Segunda_Entrega_Real + "', ";
                }
                else {
                    Mi_sql += "null" + ", ";
                }
                Mi_sql += "'" + Calendario_Entregas.P_Fecha_Tercera_Entrega + "', ";
                if(Calendario_Entregas.P_Fecha_Tercera_Entrega_Real != null && Calendario_Entregas.P_Fecha_Tercera_Entrega_Real.Trim() != "")
                {
                    Mi_sql += "'" +Calendario_Entregas.P_Fecha_Tercera_Entrega_Real + "', ";   
                }
                else {
                    Mi_sql +=  "null" + ", ";
                }
                Mi_sql += "'" + Calendario_Entregas.P_Fecha_Cuarta_Entrega + "', ";                
                if(Calendario_Entregas.P_Fecha_Cuarta_Entrega_Real != null && Calendario_Entregas.P_Fecha_Cuarta_Entrega_Real.Trim() != "")
                {
                    Mi_sql += "'" + Calendario_Entregas.P_Fecha_Cuarta_Entrega_Real + "', ";   
                }
                else {
                    Mi_sql +=  "null" + ", ";
                }
                Mi_sql += "'" + Calendario_Entregas.P_Fecha_Quinta_Entrega + "', ";
                if(Calendario_Entregas.P_Fecha_Quinta_Entrega_Real != null && Calendario_Entregas.P_Fecha_Quinta_Entrega_Real.Trim() != "")
                {
                    Mi_sql += "'" +  Calendario_Entregas.P_Fecha_Quinta_Entrega_Real + "', ";   
                }else {
                    Mi_sql +=  "null" + ", ";
                }
                Mi_sql += "'" + Calendario_Entregas.P_Fecha_Sexta_Entrega + "', ";
                if(Calendario_Entregas.P_Fecha_Sexta_Entrega_Real != null && Calendario_Entregas.P_Fecha_Sexta_Entrega_Real.Trim() != "")
                {
                    Mi_sql += "'" + Calendario_Entregas.P_Fecha_Sexta_Entrega_Real + "', ";   
                }
                else {
                    Mi_sql +=  "null" + ", ";
                }
                Mi_sql += "'" +  Calendario_Entregas.P_Fecha_Septima_Entrega + "', "; 
                if(Calendario_Entregas.P_Fecha_Septima_Entrega_Real != null && Calendario_Entregas.P_Fecha_Septima_Entrega_Real.Trim() != "")
                {
                    Mi_sql += "'" + Calendario_Entregas.P_Fecha_Septima_Entrega_Real + "', ";   
                }
                else {
                    Mi_sql +=  "null" + ", ";
                }
                Mi_sql += "'" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += "SYSDATE";
                Mi_sql += ")";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Fecha_Entrega_Id = (Convert.ToInt16(Fecha_Entrega_Id) + 1).ToString("00000");
            }               
            catch (Exception Ex)
            {
                Trans.Rollback();
                throw new Exception("Alta_Calendario_Entregas:" + Ex.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Calendario_Entregas
        ///DESCRIPCIÓN: Da de alta en la Base de Datos los valores las cuotas 
        ///PARAMENTROS:     
        ///             
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Modificar_Calendario_Entregas(Cls_Cat_Cat_Calendario_Entregas_Negocio Calendario_Entregas)
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
                Mi_sql = "UPDATE " + Cat_Cat_Calendario_Entregas.Tabla_Cat_Cat_Calendario_Entregas;
                Mi_sql += " SET " + Cat_Cat_Calendario_Entregas.Campo_Anio + " = " + Calendario_Entregas.P_Anio + ", ";
                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Primera_Entrega + " = '" + Calendario_Entregas.P_Fecha_Primera_Entrega + "', ";
                if (Calendario_Entregas.P_Fecha_Primera_Entrega_Real == "")
                {
                    Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Primera_Entrega_Real + " = " + " null " + ", ";
                }
                else
                {
                    Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Primera_Entrega_Real + " = '" + Calendario_Entregas.P_Fecha_Primera_Entrega_Real + "', ";
                }                

                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Segunda_Entrega + " = '" + Calendario_Entregas.P_Fecha_Segunda_Entrega + "', ";
                if (Calendario_Entregas.P_Fecha_Segunda_Entrega_Real == "")
                {
                    Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Segunda_Entrega_Real + " = " + " null " + ", ";
                }
                else
                {
                    Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Segunda_Entrega_Real + " = '" + Calendario_Entregas.P_Fecha_Segunda_Entrega_Real + "', ";
                }
                
                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Tercera_Entrega + " = '" + Calendario_Entregas.P_Fecha_Tercera_Entrega + "', ";
                if (Calendario_Entregas.P_Fecha_Tercera_Entrega_Real == "")
                {
                    Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Tercera_Entrega_Real + " = " + " null " + ", ";
                }
                else
                {
                    Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Tercera_Entrega_Real + " = '" + Calendario_Entregas.P_Fecha_Tercera_Entrega_Real + "', ";
                }                
                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Cuarta_Entrega + " = '" + Calendario_Entregas.P_Fecha_Cuarta_Entrega + "', ";
                if (Calendario_Entregas.P_Fecha_Cuarta_Entrega_Real == "")
                {
                    Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Cuarta_Entrega_Real + " = " + " null " + ", ";
                }
                else
                {
                    Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Cuarta_Entrega_Real + " = '" + Calendario_Entregas.P_Fecha_Cuarta_Entrega_Real + "', ";
                }                
                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Quinta_Entrega + " = '" + Calendario_Entregas.P_Fecha_Quinta_Entrega + "', ";
                if (Calendario_Entregas.P_Fecha_Quinta_Entrega_Real == "")
                {
                    Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Quinta_Entrega_Real + " = " + " null " + ", ";
                }
                else
                {
                    Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Quinta_Entrega_Real + " = '" + Calendario_Entregas.P_Fecha_Quinta_Entrega_Real + "', ";
                }
                
                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Sexta_Entrega + " = '" + Calendario_Entregas.P_Fecha_Sexta_Entrega + "', ";
                if (Calendario_Entregas.P_Fecha_Sexta_Entrega_Real == "")
                {
                    Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Sexta_Entrega_Real + " = " + " null " + ", ";
                }
                else
                {
                    Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Sexta_Entrega_Real + " = '" + Calendario_Entregas.P_Fecha_Sexta_Entrega_Real + "', ";
                }
                
                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Septima_Entrega + " = '" + Calendario_Entregas.P_Fecha_Septima_Entrega + "', ";
                if (Calendario_Entregas.P_Fecha_Septima_Entrega_Real == "")
                {
                    Mi_sql +=Cat_Cat_Calendario_Entregas.Campo_Fecha_Septima_Entrega_Real + " = "  + " null " + ", ";
                }
                else
                {
                    Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Septima_Entrega_Real + " = '" + Calendario_Entregas.P_Fecha_Septima_Entrega_Real + "', ";
                }
                
                Mi_sql += Cat_Cat_Calendario_Entregas.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_sql += " WHERE " + Cat_Cat_Calendario_Entregas.Campo_Fecha_Entrega_Id + " = '" + Calendario_Entregas.P_Fecha_Entrega_Id + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
            }catch(Exception Ex)
            {
                Trans.Rollback();
                throw new Exception("Modificar Calendario Entregas: " + Ex.Message);
            }
            Trans.Commit();
            return Alta;
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
            String Retornar="";
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