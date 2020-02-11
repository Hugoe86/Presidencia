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
using Presidencia.Catalogo_Otros_Pagos.Negocio;

namespace Presidencia.Catalogo_Otros_Pagos.Datos{
    
    public class Cls_Cat_Pre_Otros_Pagos_Datos {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Otro_Pago
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo conceto de pago
        ///PARAMETROS:     
        ///             1. Otro_Pago.    Instancia de la Clase de Negocio de Otros Pagos con los datos 
        ///                          del concepto de Pago que va a ser dada de Alta.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 15/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Otro_Pago(Cls_Cat_Pre_Otros_Pagos_Negocio Otro_Pago)
        {
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
                String Otro_Pago_ID = Obtener_ID_Consecutivo(Cat_Pre_Otros_Pagos.Tabla_Cat_Pre_Otros_Pagos, Cat_Pre_Otros_Pagos.Campo_Pago_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Otros_Pagos.Tabla_Cat_Pre_Otros_Pagos;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Otros_Pagos.Campo_Pago_ID + ", " + Cat_Pre_Otros_Pagos.Campo_Concepto;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Otros_Pagos.Campo_Estatus + ", " + Cat_Pre_Otros_Pagos.Campo_Descripcion;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Otros_Pagos.Campo_Usuario_Creo + ", " + Cat_Pre_Otros_Pagos.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Otro_Pago_ID + "', '" + Otro_Pago.P_Concepto + "'";
                Mi_SQL = Mi_SQL + ",'" + Otro_Pago.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Otro_Pago.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + ",'" + Otro_Pago.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", SYSDATE";
                Mi_SQL = Mi_SQL + ")";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 2627)
                {
                    if (Ex.Message.IndexOf("PRIMARY") != -1)
                    {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    }
                    else if (Ex.Message.IndexOf("UNIQUE") != -1)
                    {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                    }
                    else
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Multa. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Otro_Pago
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Concepto de Pago
        ///PARAMETROS:     
        ///             1. Otro_Pago.   Instancia de la Clase de Negocio de Otros Pagos con 
        ///                         los datos del conceptod de pago que va a ser Actualizada.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 15/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Otro_Pago(Cls_Cat_Pre_Otros_Pagos_Negocio Otro_Pago)
        {
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Otros_Pagos.Tabla_Cat_Pre_Otros_Pagos + " SET ";
                Mi_SQL = Mi_SQL + Cat_Pre_Otros_Pagos.Campo_Concepto + " = '" + Otro_Pago.P_Concepto + "', "; 
                Mi_SQL = Mi_SQL + Cat_Pre_Otros_Pagos.Campo_Estatus + " = '" + Otro_Pago.P_Estatus + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Otros_Pagos.Campo_Descripcion + " = '" + Otro_Pago.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Otros_Pagos.Campo_Usuario_Modifico + " = '" + Otro_Pago.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Otros_Pagos.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Otros_Pagos.Campo_Pago_ID + " = '" + Otro_Pago.P_Pago_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 2627)
                {
                    if (Ex.Message.IndexOf("PRIMARY") != -1)
                    {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    }
                    else if (Ex.Message.IndexOf("UNIQUE") != -1)
                    {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                    }
                    else
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
                    Mensaje = "Error al intentar modificar un Registro de Otros Pagos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Otros_Pagos
        ///DESCRIPCIÓN: Obtiene todos los Conptos de Pago que estan dados de alta en la Base de Datos
        ///PARAMETROS:
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 15/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Otros_Pagos()
        {
            DataTable Tabla = new DataTable();
            try{
                String Mi_SQL = "SELECT " + Cat_Pre_Otros_Pagos.Campo_Pago_ID + " AS PAGO_ID "; 
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Otros_Pagos.Campo_Concepto + " AS CONCEPTO";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Otros_Pagos.Campo_Estatus + " AS ESTATUS";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Otros_Pagos.Campo_Descripcion + " AS DESCRIPCION";
                Mi_SQL = Mi_SQL +  " FROM " + Cat_Pre_Otros_Pagos.Tabla_Cat_Pre_Otros_Pagos;
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Otros_Pagos.Campo_Pago_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null) {
                    Tabla = dataset.Tables[0];
                }
            }catch(Exception Ex){
                String Mensaje = "Error al intentar consultar los registros de Otros Pagos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Otro_Pago
        ///DESCRIPCIÓN: Obtiene a detalle un Concepto de Pago.
        ///PARAMETROS:   
        ///             1. Otro_Pago.   Concepto de Pago que se va ver a Detalle.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 15/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Otro_Pago(Cls_Cat_Pre_Otros_Pagos_Negocio Otro_Pago)
        {
            
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Otros_Pagos.Campo_Pago_ID + ", " + Cat_Pre_Otros_Pagos.Campo_Concepto;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Otros_Pagos.Campo_Estatus + ", " + Cat_Pre_Otros_Pagos.Campo_Descripcion;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Otros_Pagos.Tabla_Cat_Pre_Otros_Pagos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Otros_Pagos.Campo_Concepto + " LIKE '%" + Otro_Pago.P_Concepto + "%'";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Otros Pagos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Otro_Pago
        ///DESCRIPCIÓN: Elimina un concepto de multa
        ///PARAMETROS:   
        ///             1. Otro_Pago.   Concepto de Pago que se va eliminar.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 15/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Otro_Pago(Cls_Cat_Pre_Otros_Pagos_Negocio Otro_Pago)
        {
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Otros_Pagos.Tabla_Cat_Pre_Otros_Pagos;
                Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Otros_Pagos.Campo_Estatus + " = 'BAJA'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Otros_Pagos.Campo_Usuario_Modifico + " = '" + Otro_Pago.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Otros_Pagos.Campo_Fecha_Modifico + " = SYSDATE" ;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Otros_Pagos.Campo_Pago_ID + " = '" + Otro_Pago.P_Pago_ID + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Otro concepto de Pago. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);             
            } finally{
                Cn.Close();
            }
        
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 15/Julio/2011 
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
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 15/Julio/2011 
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