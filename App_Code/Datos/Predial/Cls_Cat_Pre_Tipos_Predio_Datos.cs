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
using Presidencia.Catalogo_Tipos_Predio.Negocio;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

/// <summary>
/// Summary description for Cls_Cat_Pre_Estados_Predio_Datos
/// </summary>

namespace Presidencia.Catalogo_Tipos_Predio.Datos{

    public class Cls_Cat_Pre_Tipos_Predio_Datos {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Tipo_Predio
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nuevo registro de Tipo de Predio
        ///PARAMETROS:     
        ///             1. Tipo_Predio.   Objeto con las propiedades necesarias para dar
        ///                                 de alta el Tipo de Predio.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 28/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Tipo_Predio(Cls_Cat_Pre_Tipos_Predio_Negocio Tipo_Predio) {
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
                String Tipo_Predio_ID = Obtener_ID_Consecutivo(Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio, Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio;
                Mi_SQL = Mi_SQL + "( " + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + ", " + Cat_Pre_Tipos_Predio.Campo_Descripcion;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tipos_Predio.Campo_Usuario_Creo + ", " + Cat_Pre_Tipos_Predio.Campo_Fecha_Creo;
                Mi_SQL = Mi_SQL + ") VALUES ( '" + Tipo_Predio_ID + "', '" + Tipo_Predio.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + ", '" + Tipo_Predio.P_Usuario + "', SYSDATE )";
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Tipo de Predio. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally{
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Tipo_Predio
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Registro de Tipo de Predio
        ///PARAMETROS:     
        ///             1. Tipo_Predio.   Objeto con las propiedades necesarias para dar
        ///                                 de actualizar el Tipo de Predio.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 28/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Tipo_Predio(Cls_Cat_Pre_Tipos_Predio_Negocio Tipo_Predio){
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio;
                Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Tipos_Predio.Campo_Descripcion + " = '" + Tipo_Predio.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tipos_Predio.Campo_Usuario_Modifico + " = '" + Tipo_Predio.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tipos_Predio.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " = '" + Tipo_Predio.P_Tipo_Predio_ID + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Tipo de Predio. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);             
            } finally{
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tipo_Predio
        ///DESCRIPCIÓN: Obtiene todos Registros de un tipo de consulta y las devueve en 
        ///             un DataTable.
        ///PARAMETROS:   
        ///             1. Tipo_Predio.   Objeto con las propiedades necesarias para 
        ///                                 hacer la consulta.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 28/Octubre/2010 
        ///MODIFICO             : Antonio Salvador Benavides Guardado
        ///FECHA_MODIFICO       : 17/Diciembre/2010
        ///CAUSA_MODIFICACIÓN   : Adecuar funcionalidad base para consultas más dinámicas
        ///*******************************************************************************
        public static DataTable Consultar_Tipo_Predio(Cls_Cat_Pre_Tipos_Predio_Negocio Tipo_Predio)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";

            try
            {
                if (Tipo_Predio.P_Campos_Dinamicos != null && Tipo_Predio.P_Campos_Dinamicos != "")
                {
                    Mi_SQL = "SELECT " + Tipo_Predio.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL = "SELECT ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " AS TIPO_PREDIO_ID, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Predio.Campo_Descripcion + " AS DESCRIPCION";
                }
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio;
                if (Tipo_Predio.P_Filtros_Dinamicos != null && Tipo_Predio.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Tipo_Predio.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Tipos_Predio.Campo_Descripcion + " LIKE '%" + Tipo_Predio.P_Descripcion + "%'";
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
                if (Tipo_Predio.P_Agrupar_Dinamico != null && Tipo_Predio.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL = Mi_SQL + " GROUP BY " + Tipo_Predio.P_Agrupar_Dinamico;
                }
                if (Tipo_Predio.P_Ordenar_Dinamico != null && Tipo_Predio.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL = Mi_SQL + " ORDER BY " + Tipo_Predio.P_Ordenar_Dinamico;
                }
                //if (Tipo_Predio.P_Tipo_DataTable.Equals("TIPOS_PREDIO"))
                //{
                //    Mi_SQL = "SELECT " + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " AS TIPO_PREDIO_ID, " + Cat_Pre_Tipos_Predio.Campo_Descripcion + " AS DESCRIPCION";
                //    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio;
                //    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Tipos_Predio.Campo_Descripcion + " LIKE '%" + Tipo_Predio.P_Descripcion + "%'";
                //}
                //if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
                //{
                //    dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                //}
                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null)
                {
                    Tabla = dataSet.Tables[0];
                }
            }catch(Exception Ex){
                String Mensaje = "Error al intentar consultar los registros de Tipo de Predio. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Tipo_Predio
        ///DESCRIPCIÓN: Elimina un Registro de Tipos Predio de la Base de Datos
        ///PARAMETROS:    
        ///             1. Tipo_Predio.   Objeto con las propiedades necesarias para
        ///                                eliminar el Tipo Predio.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 28/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Tipo_Predio(Cls_Cat_Pre_Tipos_Predio_Negocio Tipo_Predio) {
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
                String Mi_SQL = "DELETE FROM " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID;
                Mi_SQL = Mi_SQL + " = '" + Tipo_Predio.P_Tipo_Predio_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            } catch (OracleException Ex){
                if (Ex.Code == 547) {
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]";
                } else {
                    Mensaje = "Error al intentar eliminar el registro de Tipo de Predio. Error: [" + Ex.Message + "]"; 
                }
                throw new Exception(Mensaje);
            } catch (Exception Ex){
                Mensaje = "Error al intentar eliminar el registro de Tipo de Predio. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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