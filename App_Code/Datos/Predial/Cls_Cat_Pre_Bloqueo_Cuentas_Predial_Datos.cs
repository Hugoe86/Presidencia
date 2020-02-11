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
using Presidencia.Catalogo_Bloqueo_Cuentas_Predial.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Datos
/// </summary>
/// 
namespace Presidencia.Catalogo_Bloqueo_Cuentas_Predial.Datos{
    public class Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Bloqueo_Cuentas_Predial
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nuevo Bloqueo de Cuentas Predial
        ///PARAMENTROS:     
        ///             1. Cuenta_Predial.  Instancia de la Clase de Negocio de Bloqueo 
        ///                                 Cuenta Predial con los datos del que van a ser
        ///                                 dados de Alta.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Bloqueo_Cuentas_Predial(Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Negocio Cuenta_Predial){
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
                String Cuenta_Predial_ID = Obtener_ID_Consecutivo(Cat_Pre_Bloq_Cuentas_Predial.Tabla_Cat_Pre_Bloq_Cuentas_Predial, Cat_Pre_Bloq_Cuentas_Predial.Campo_Bloque_Cuenta_Predial_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Bloq_Cuentas_Predial.Tabla_Cat_Pre_Bloq_Cuentas_Predial;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Bloq_Cuentas_Predial.Campo_Bloque_Cuenta_Predial_ID + ", " + Cat_Pre_Bloq_Cuentas_Predial.Campo_Cuenta_Predial;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Bloq_Cuentas_Predial.Campo_Estatus + ", " + Cat_Pre_Bloq_Cuentas_Predial.Campo_Tipo_Bloqueo;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Bloq_Cuentas_Predial.Campo_Usuario_Creo + ", " + Cat_Pre_Bloq_Cuentas_Predial.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Cuenta_Predial_ID + "', '" + Cuenta_Predial.P_Cuenta_Predial + "'";
                Mi_SQL = Mi_SQL + ",'" + Cuenta_Predial.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Cuenta_Predial.P_Tipo_Bloqueo + "'";
                Mi_SQL = Mi_SQL + ",'" + Cuenta_Predial.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", SYSDATE";
                Mi_SQL = Mi_SQL + ")";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            } catch (OracleException Ex) {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152) {
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
                    Mensaje = "Error al intentar dar de Alta un Registro al Bloqueo de Cuentas de Predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }finally {
                if (Cn.State == ConnectionState.Open) {
                    Cn.Close();
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Bloqueo_Cuentas_Predial
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Bloqueo de Cuenta Predial
        ///PARAMENTROS:     
        ///             1. Cuenta_Predial.  Instancia de la Clase de Negocio de Bloqueo 
        ///                                 de Cuenta Predial con los datos del Registro 
        ///                                 que va a ser Actualizado.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Bloqueo_Cuentas_Predial(Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Negocio Cuenta_Predial){
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Bloq_Cuentas_Predial.Tabla_Cat_Pre_Bloq_Cuentas_Predial;
                Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Bloq_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Cuenta_Predial.P_Cuenta_Predial + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Bloq_Cuentas_Predial.Campo_Estatus + " = '" + Cuenta_Predial.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Bloq_Cuentas_Predial.Campo_Tipo_Bloqueo + " = '" + Cuenta_Predial.P_Tipo_Bloqueo + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Bloq_Cuentas_Predial.Campo_Usuario_Modifico + " = '" + Cuenta_Predial.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Bloq_Cuentas_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Bloq_Cuentas_Predial.Campo_Bloque_Cuenta_Predial_ID + " = '" + Cuenta_Predial.P_Bloque_Cuenta_Predial_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            } catch (OracleException Ex) {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152) {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]"; 
                } else if (Ex.Code == 2627) {
                    if (Ex.Message.IndexOf("PRIMARY") != -1) {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]"; 
                    } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]"; 
                    } else {
                        Mensaje = "Error general en la base de datos";
                    }
                } else if (Ex.Code == 547) {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]"; 
                } else if (Ex.Code == 515) {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]"; 
                } else {
                    Mensaje = "Error al intentar modificar un Registro al Bloqueo de Cuentas de Predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Bloqueo_Cuentas_Predial
        ///DESCRIPCIÓN: Obtiene todos los Bloqueos de Cuentas Predial que estan dadas de 
        ///             alta en la Base de Datos
        ///PARAMENTROS:   
        ///             1.  Cuenta_Predial. Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                                 caso el filtro es el Identificador.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Bloqueo_Cuentas_Predial(Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Negocio Cuenta_Predial){
            DataTable tabla = new DataTable();;
            try { 
                String Mi_SQL = "SELECT " + Cat_Pre_Bloq_Cuentas_Predial.Campo_Bloque_Cuenta_Predial_ID;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Bloq_Cuentas_Predial.Campo_Cuenta_Predial + "";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Bloq_Cuentas_Predial.Campo_Estatus + "";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Bloq_Cuentas_Predial.Tabla_Cat_Pre_Bloq_Cuentas_Predial;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Bloq_Cuentas_Predial.Campo_Cuenta_Predial + " LIKE '%" + Cuenta_Predial.P_Cuenta_Predial + "%' ";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Bloq_Cuentas_Predial.Campo_Bloque_Cuenta_Predial_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null) {
                    tabla = dataset.Tables[0];
                }          
            }catch(Exception Ex){
                String Mensaje = "Error al intentar consultar los registros de Bloqueo Cuentas Predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;  
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Datos_Bloqueo_Cuentas_Predial
        ///DESCRIPCIÓN: Obtiene a detalle un Bloqueo de Cuenta Predial.
        ///PARAMENTROS:   
        ///             1. P_Cuenta_Predial.   Bloqueo de Cuenta Predial que se va ver a Detalle.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Negocio Consultar_Datos_Bloqueo_Cuentas_Predial(Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Negocio P_Cuenta_Predial){
            Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Negocio R_Cuenta_Predial = new Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Negocio();
            String Mi_SQL = "SELECT " + Cat_Pre_Bloq_Cuentas_Predial.Campo_Cuenta_Predial + ", " + Cat_Pre_Bloq_Cuentas_Predial.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Bloq_Cuentas_Predial.Campo_Tipo_Bloqueo;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Bloq_Cuentas_Predial.Tabla_Cat_Pre_Bloq_Cuentas_Predial;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Bloq_Cuentas_Predial.Campo_Bloque_Cuenta_Predial_ID + " = '" + P_Cuenta_Predial.P_Bloque_Cuenta_Predial_ID + "'";
            OracleDataReader Data_Reader;
            try{
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Cuenta_Predial.P_Bloque_Cuenta_Predial_ID = P_Cuenta_Predial.P_Bloque_Cuenta_Predial_ID;
                while (Data_Reader.Read()){
                    R_Cuenta_Predial.P_Cuenta_Predial = Data_Reader[Cat_Pre_Bloq_Cuentas_Predial.Campo_Cuenta_Predial].ToString();
                    R_Cuenta_Predial.P_Estatus = Data_Reader[Cat_Pre_Bloq_Cuentas_Predial.Campo_Estatus].ToString();
                    R_Cuenta_Predial.P_Tipo_Bloqueo = Data_Reader[Cat_Pre_Bloq_Cuentas_Predial.Campo_Tipo_Bloqueo].ToString();
                }
                Data_Reader.Close();
            }catch (Exception Ex){
                String Mensaje = "Error al intentar consultar el registro de Bloqueo Cuentas Predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Cuenta_Predial;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Bloqueo_Cuentas_Predial
        ///DESCRIPCIÓN: Elimina un Bloqueo Cuenta Predial de la Base de Datos.
        ///PARAMENTROS:   
        ///             1. Cuenta_Predial.   Registro que se va a eliminar.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Bloqueo_Cuentas_Predial(Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Negocio Cuenta_Predial){
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
                String Mi_SQL = "DELETE FROM " + Cat_Pre_Bloq_Cuentas_Predial.Tabla_Cat_Pre_Bloq_Cuentas_Predial;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Bloq_Cuentas_Predial.Campo_Bloque_Cuenta_Predial_ID + " = '" + Cuenta_Predial.P_Bloque_Cuenta_Predial_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            } catch (OracleException Ex) {
                if (Ex.Code == 547){
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]"; ;
                } else {
                    Mensaje = "Error al intentar eliminar el registro de Bloqueo Cuentas Predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                throw new Exception(Mensaje);
            } catch (Exception Ex) {
                Mensaje = "Error al intentar eliminar el registro de Bloqueo Cuentas Predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static String Obtener_ID_Consecutivo(String Tabla, String Campo, Int32 Longitud_ID) {
            String Id = Convertir_A_Formato_ID(1, Longitud_ID); ;
            try {
                String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla;
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals("")) {
                    Id = Convertir_A_Formato_ID((Convert.ToInt32(Obj_Temp) + 1), Longitud_ID);
                }
            } catch (OracleException Ex) {
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
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID) {
            String Retornar = "";
            String Dato = "" + Dato_ID;
            for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++) {
                Retornar = Retornar + "0";
            }
            Retornar = Retornar + Dato;
            return Retornar;
        }

    }
}