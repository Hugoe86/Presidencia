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
using Presidencia.Catalogo_Turnos.Negocio;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

namespace Presidencia.Catalogo_Turnos.Datos {

    public class Cls_Cat_Pre_Turnos_Datos {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Turno
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nuevo registro de Turno
        ///PARAMETROS:     
        ///             1. Turno.   Objeto con las propiedades necesarias para dar
        ///                                 de alta el Turno.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 28/Junio/2011 
        ///MODIFICO:            José Alfredo García Pichardo
        ///FECHA_MODIFICO       08/Julio/2011
        ///CAUSA_MODIFICACIÓN   Hacia falta agregar la Fecha de creacion y el Usuario que
        ///                     lo creo en la base de datos.
        ///*******************************************************************************
        public static void Alta_Turno(Cls_Cat_Pre_Turnos_Negocio Turno){
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try
            {
                String Turnos = Obtener_ID_Consecutivo(Cat_Pre_Turnos.Tabla_Cat_Pre_Turnos, Cat_Pre_Turnos.Campo_Turno_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Turnos.Tabla_Cat_Pre_Turnos;
                Mi_SQL = Mi_SQL + "( " + Cat_Pre_Turnos.Campo_Turno_ID + ", " + Cat_Pre_Turnos.Campo_Descripcion;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Turnos.Campo_Hora_Inicio + ", " + Cat_Pre_Turnos.Campo_Hora_Fin;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Turnos.Campo_Comentarios;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Turnos.Campo_Usuario_Creo;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Turnos.Campo_Fecha_Creo;
                Mi_SQL = Mi_SQL + ") VALUES ( '" + Turnos + "', '" + Turno.P_Nombre + "'";
                Mi_SQL = Mi_SQL + ", '" + Turno.P_Hora_Inicio + "', '" + Turno.P_Hora_Fin + "'";
                Mi_SQL = Mi_SQL + ", '" + Turno.P_Comentarios + "'";
                Mi_SQL = Mi_SQL + ", '" + Turno.P_Usuario+ "'";
                Mi_SQL = Mi_SQL + ", SYSDATE)";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                } 
                else if (Ex.Code == 2627)
                {
                    if (Ex.Message.IndexOf("PRIMARY") != -1)
                    {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    } else if (Ex.Message.IndexOf("UNIQUE") != -1) 
                    {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                    } else 
                    {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
                    }
                } 
                else if (Ex.Code == 547) 
                {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
                } 
                else if (Ex.Code == 515) 
                {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                } 
                else 
                {
                    Mensaje = "Error al intentar dar de Alta un Turno. Error: [" + Ex.Message + "]";
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } 
            finally
            {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Turno
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Registro de Turno
        ///PARAMETROS:     
        ///             1. Turno.   Objeto con las propiedades necesarias para dar
        ///                                 de actualizar el Turno.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 29/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Turno(Cls_Cat_Pre_Turnos_Negocio Turno){
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Turnos.Tabla_Cat_Pre_Turnos;
                Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Turnos.Campo_Descripcion + " = '" + Turno.P_Nombre + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Turnos.Campo_Hora_Inicio + " = '" + Turno.P_Hora_Inicio + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Turnos.Campo_Hora_Fin + " = '" + Turno.P_Hora_Fin + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Turnos.Campo_Comentarios + " = '" + Turno.P_Comentarios + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Turnos.Campo_Usuario_Modifico + " = '" + Turno.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Turnos.Campo_Fecha_Modifico + " = SYSDATE" ;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Turnos.Campo_Turno_ID + " = '" + Turno.P_Turno_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            } catch (OracleException Ex) {
                Trans.Rollback();
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
                    Mensaje = "Error al intentar modificar un Registro de Turnos. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);             
            } finally{
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Turno
        ///DESCRIPCIÓN: Obtiene los registros de Turnos y las devuelve en 
        ///             un DataTable.
        ///PARAMETROS:   
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 28/Junio/2011 
        ///MODIFICO             
        ///FECHA_MODIFICO       
        ///CAUSA_MODIFICACIÓN   
        ///*******************************************************************************
        public static DataTable Consultar_Turno()
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "SELECT " + Cat_Pre_Turnos.Campo_Turno_ID + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Turnos.Campo_Descripcion + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Turnos.Campo_Hora_Inicio+ ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Turnos.Campo_Hora_Fin + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Turnos.Campo_Comentarios;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Turnos.Tabla_Cat_Pre_Turnos;
            Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Turnos.Campo_Turno_ID;
            try
            {
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los Cajeros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Busqueda
        ///DESCRIPCIÓN: Obtiene todos Registros de un tipo de consulta y las devueve en 
        ///             un DataTable.
        ///PARAMETROS:   
        ///             1. Turno.   Objeto con las propiedades necesarias para 
        ///                                 hacer la consulta.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 02/Junio/2011
        ///MODIFICO          
        ///FECHA_MODIFICO     
        ///CAUSA_MODIFICACIÓN   
        ///*******************************************************************************
        public static DataTable Consultar_Busqueda(Cls_Cat_Pre_Turnos_Negocio Turno)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "SELECT " + Cat_Pre_Turnos.Campo_Turno_ID + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Turnos.Campo_Descripcion + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Turnos.Campo_Hora_Inicio + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Turnos.Campo_Hora_Fin + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Turnos.Campo_Comentarios;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Turnos.Tabla_Cat_Pre_Turnos;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Turnos.Campo_Turno_ID + " = '" + Turno.P_Turno_ID + "'";
            Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Turnos.Campo_Turno_ID;
            try
            {
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los Turnos. Error: [" + Ex.Message + "]";
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Turno
        ///DESCRIPCIÓN: Elimina un Registro de Turno de la Base de Datos
        ///PARAMETROS:    
        ///             1. Turno.   Objeto con las propiedades necesarias para
        ///                                 eliminar el Turno.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 29/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Turno(Cls_Cat_Pre_Turnos_Negocio Turno){
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
                String Mi_SQL = "DELETE FROM " + Cat_Pre_Turnos.Tabla_Cat_Pre_Turnos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Turnos.Campo_Turno_ID;
                Mi_SQL = Mi_SQL + " = '" + Turno.P_Turno_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            } catch (OracleException Ex){
                if (Ex.Code == 547) {
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]";
                } else {
                    Mensaje = "Error al intentar eliminar el registro de Estado Predio. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            } catch (Exception Ex){
                Mensaje = "Error al intentar eliminar el registro de Estado Predio. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS: 
        ///             1. Tabla.   Nombre de la tabla a la que se hace referencia.
        ///             2. Campo.   El nombre del campo que se quiere obtener.
        ///             3. Longitud_ID.     Longitud del campo a obtener.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 29/Junio/2011 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
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
        ///PARÁMETROS:     
        ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
        ///             2. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: José Alfredo García Picgardo.
        ///FECHA_CREO: 29/Junio/2011 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
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