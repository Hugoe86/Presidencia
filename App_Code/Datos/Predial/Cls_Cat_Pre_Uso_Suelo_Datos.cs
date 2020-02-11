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
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Catalogo_Uso_Suelo.Negocio;
/// <summary>
/// Summary description for Cls_Cat_Pre_Uso_Suelo_Datos
/// </summary>
/// 

namespace Presidencia.Catalogo_Uso_Suelo.Datos{
    public class Cls_Cat_Pre_Uso_Suelo_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Uso_Suelo
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nuevo registro de Uso de Suelo
        ///PARAMETROS:     
        ///             1. Uso_Suelo.    Instancia de la Clase de Negocio de Cls_Cat_Pre_Uso_Suelo_Negocio
        ///                             con los datos del registro de Uso de Suelo que va a ser dado de Alta.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 30/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Uso_Suelo(Cls_Cat_Pre_Uso_Suelo_Negocio Uso_Suelo){
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try{
                String Uso_Suelo_ID = Obtener_ID_Consecutivo(Cat_Pre_Uso_Suelo.Tabla_Cat_Pre_Uso_Suelo, Cat_Pre_Uso_Suelo.Campo_Uso_Suelo_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Uso_Suelo.Tabla_Cat_Pre_Uso_Suelo;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Uso_Suelo.Campo_Uso_Suelo_ID + ", " + Cat_Pre_Uso_Suelo.Campo_Identificador;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Uso_Suelo.Campo_Descripcion + ", " + Cat_Pre_Uso_Suelo.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Uso_Suelo.Campo_Usuario_Creo + ", " + Cat_Pre_Uso_Suelo.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Uso_Suelo_ID + "', '" + Uso_Suelo.P_Identificador + "'";
                Mi_SQL = Mi_SQL + ",'" + Uso_Suelo.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + ",'" + Uso_Suelo.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Uso_Suelo.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", SYSDATE";
                Mi_SQL = Mi_SQL + ")";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            }catch (OracleException Ex){
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152){
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]"; 
                } else if (Ex.Code == 2627){
                    if (Ex.Message.IndexOf("PRIMARY") != -1){
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]"; 
                    } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]"; 
                    } else {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]"; 
                    }
                } else if (Ex.Code == 547) {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]"; 
                } else if (Ex.Code == 515) {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]"; 
                } else {
                    Mensaje = "Error al intentar dar de Alta un Registro de Registro de Uso de Predio. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally{
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Uso_Suelo
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Registro de Uso de Suelo
        ///PARAMETROS:     
        ///             1. Uso_Suelo.  Instancia de la Clase de Negocio de Uso de Suelo con los datos de la Registro que va a ser Actualizado.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 30/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Uso_Suelo(Cls_Cat_Pre_Uso_Suelo_Negocio Uso_Suelo){
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try {
                String Mi_SQL = "UPDATE " + Cat_Pre_Uso_Suelo.Tabla_Cat_Pre_Uso_Suelo + " SET " + Cat_Pre_Uso_Suelo.Campo_Identificador + " = '" + Uso_Suelo.P_Identificador + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Uso_Suelo.Campo_Estatus + " = '" + Uso_Suelo.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Uso_Suelo.Campo_Descripcion + " = '" + Uso_Suelo.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Uso_Suelo.Campo_Usuario_Modifico + " = '" + Uso_Suelo.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Uso_Suelo.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Uso_Suelo.Campo_Uso_Suelo_ID + " = '" + Uso_Suelo.P_Uso_Suelo_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            } catch (OracleException Ex) {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152){
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]"; 
                } else if (Ex.Code == 2627) {
                    if (Ex.Message.IndexOf("PRIMARY") != -1) {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]"; 
                    } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]"; 
                    } else {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]"; 
                    }
                } else if (Ex.Code == 547) {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]"; 
                } else if (Ex.Code == 515) {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]"; 
                } else {
                    Mensaje = "Error al intentar modificar un Registro de Uso de Predio. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);             
            } finally{
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Registros_Uso_Suelo
        ///DESCRIPCIÓN: Obtiene todos Registros de Uso de Suelo que estan dadas de alta en la Base de Datos
        ///PARAMETROS:   
        ///             1.  Uso_Suelo.   Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                             caso el filtro es el Identificador.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 30/Agosto/2010 
        ///MODIFICO             : Antonio salvador Benavides Guardado
        ///FECHA_MODIFICO       : 17/Diciembre/2010
        ///CAUSA_MODIFICACIÓN   : Adecuar funcionaliad base para consultas dinámicas
        ///*******************************************************************************
        public static DataTable Consultar_Uso_Suelo(Cls_Cat_Pre_Uso_Suelo_Negocio Uso_Suelo){
            DataTable Tabla = new DataTable();
            String Mi_SQL;

            try
            {
                if (Uso_Suelo.P_Campos_Dinamicos != null && Uso_Suelo.P_Campos_Dinamicos != "")
                {
                    Mi_SQL = "SELECT " + Uso_Suelo.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL = "SELECT ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Uso_Suelo.Campo_Uso_Suelo_ID + " AS USO_SUELO_ID, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Uso_Suelo.Campo_Identificador + " AS IDENTIFICADOR, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Uso_Suelo.Campo_Estatus + " AS ESTATUS, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Uso_Suelo.Campo_Descripcion + " AS DESCRIPCION";
                }
                Mi_SQL = Mi_SQL +  " FROM " + Cat_Pre_Uso_Suelo.Tabla_Cat_Pre_Uso_Suelo;
                if (Uso_Suelo.P_Filtros_Dinamicos != null && Uso_Suelo.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Uso_Suelo.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Uso_Suelo.Campo_Identificador + " LIKE '%" + Uso_Suelo.P_Identificador + "%' ";
                    Mi_SQL = Mi_SQL + " OR " + Cat_Pre_Uso_Suelo.Campo_Descripcion + " LIKE '%" + Uso_Suelo.P_Identificador + "%' "; 
                    //DESCOMENTAR EL SIGUIENTE BLOQUE IF SI SE AGREGAN FILTROS EN ESTA SECCIÓN
                    //if (Mi_SQL.EndsWith(" AND "))
                    //{
                    //    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                    //}
                    //DESCOMENTAR EL SIGUIENTE BLOQUE WHERE SI SE QUITA EL CAMPO CONCEPTO_PREDIAL_ID DE LA LÍNEA DEL WHERE
                    //if (Mi_SQL.EndsWith(" WHERE "))
                    //{
                    //    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                    //}
                }
                if (Uso_Suelo.P_Agrupar_Dinamico != null && Uso_Suelo.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL = Mi_SQL + " GROUP BY " + Uso_Suelo.P_Agrupar_Dinamico;
                }
                if (Uso_Suelo.P_Ordenar_Dinamico != null && Uso_Suelo.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL = Mi_SQL + " ORDER BY " + Uso_Suelo.P_Ordenar_Dinamico;
                }
                else
                {
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Uso_Suelo.Campo_Uso_Suelo_ID;
                }
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null) {
                    Tabla = dataset.Tables[0];
                }
            }catch(Exception Ex){
                String Mensaje = "Error al intentar consultar los registros de Uso de Predio. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Uso_Suelo
        ///DESCRIPCIÓN: Obtiene a detalle un Registro de Uso de Suelo.
        ///PARAMETROS:   
        ///             1. P_Uso_Suelo.   Uso de Suelo que se va ver a Detalle.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 30/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Uso_Suelo_Negocio Consultar_Datos_Uso_Suelo(Cls_Cat_Pre_Uso_Suelo_Negocio P_Uso_Suelo){
            String Mi_SQL = "SELECT " + Cat_Pre_Uso_Suelo.Campo_Identificador + ", " + Cat_Pre_Uso_Suelo.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Uso_Suelo.Campo_Descripcion + " FROM " + Cat_Pre_Uso_Suelo.Tabla_Cat_Pre_Uso_Suelo;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Uso_Suelo.Campo_Uso_Suelo_ID + " = '" + P_Uso_Suelo.P_Uso_Suelo_ID + "'";
            Cls_Cat_Pre_Uso_Suelo_Negocio R_Uso_Suelo = new Cls_Cat_Pre_Uso_Suelo_Negocio();
            OracleDataReader Data_Reader;
            try{
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Uso_Suelo.P_Uso_Suelo_ID = P_Uso_Suelo.P_Uso_Suelo_ID;
                while (Data_Reader.Read()){
                    R_Uso_Suelo.P_Identificador = Data_Reader[Cat_Pre_Uso_Suelo.Campo_Identificador].ToString();
                    R_Uso_Suelo.P_Estatus = Data_Reader[Cat_Pre_Uso_Suelo.Campo_Estatus].ToString();
                    R_Uso_Suelo.P_Descripcion = Data_Reader[Cat_Pre_Uso_Suelo.Campo_Descripcion].ToString();
                }
                Data_Reader.Close();
            }catch (OracleException Ex){
                String Mensaje = "Error al intentar consultar el registro de Uso de Predio. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Uso_Suelo;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Uso_Suelo
        ///DESCRIPCIÓN: Elimina un Registro de Uso de Suelo
        ///PARAMETROS:   
        ///             1. Uso_Suelo.   Registro que se va a eliminar.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 30/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Uso_Suelo(Cls_Cat_Pre_Uso_Suelo_Negocio Uso_Suelo){
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try {
                String Mi_SQL = "DELETE FROM " + Cat_Pre_Uso_Suelo.Tabla_Cat_Pre_Uso_Suelo;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Uso_Suelo.Campo_Uso_Suelo_ID + " = '" + Uso_Suelo.P_Uso_Suelo_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            } catch (OracleException Ex){
                if (Ex.Code == 547) {
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]";
                } else {
                    Mensaje = "Error al intentar eliminar el registro de Uso de Predio. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            } catch (Exception Ex){
                Mensaje = "Error al intentar eliminar el registro de Uso de Predio. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static String Obtener_ID_Consecutivo(String Tabla, String Campo, Int32 Longitud_ID)
        {
            String Id = Convertir_A_Formato_ID(1, Longitud_ID); ;
            try
            {
                String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla;
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                {
                    Id = Convertir_A_Formato_ID((Convert.ToInt32(Obj_Temp) + 1), Longitud_ID);
                }
            }
            catch (OracleException Ex)
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
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
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