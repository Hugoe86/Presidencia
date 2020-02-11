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
using Presidencia.Nomina_Actualizar_Salario.Negocio;
using Presidencia.Sindicatos.Negocios;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

/// <summary>
/// Summary description for Cls_Ope_Nom_Actualizar_Salario_Datos
/// </summary>
/// 
namespace Presidencia.Nomina_Actualizar_Salario.Datos {

    public class Cls_Ope_Nom_Actualizar_Salario_Datos {

        #region Metodos 

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Registrar_Actualizacion
            ///DESCRIPCIÓN: Registra la Actualización de Salario.
            ///PARAMETROS:     
            ///             1.Parametros.   Contiene todos los datos para generar el registro.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 07/Abril/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************    
            public static void Registrar_Actualizacion(Cls_Ope_Nom_Actualizar_Salario_Negocio Parametros) {
                String Mensaje = "";
                OracleConnection Cn = new OracleConnection();
                OracleCommand Cmd = new OracleCommand();
                OracleTransaction Trans;
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                String No_Actualizar_Salario = Obtener_ID_Consecutivo(Ope_Nom_Actualizar_Salario.Tabla_Ope_Nom_Actualizar_Salario, Ope_Nom_Actualizar_Salario.Campo_No_Actualizar_Salario, 10);
                try {
                    String Mi_SQL = "INSERT INTO " + Ope_Nom_Actualizar_Salario.Tabla_Ope_Nom_Actualizar_Salario + " ( " + Ope_Nom_Actualizar_Salario.Campo_No_Actualizar_Salario + ", " + Ope_Nom_Actualizar_Salario.Campo_Tipo_Nomina_ID; 
                    Mi_SQL = Mi_SQL + ", " + Ope_Nom_Actualizar_Salario.Campo_Nomina_ID + ", " + Ope_Nom_Actualizar_Salario.Campo_No_Nomina + ", " + Ope_Nom_Actualizar_Salario.Campo_Fecha_Actualizacion;
                    Mi_SQL = Mi_SQL + ", " + Ope_Nom_Actualizar_Salario.Campo_Usuario_Creo + ", " + Ope_Nom_Actualizar_Salario.Campo_Fecha_Creo + " ) VALUES ( '" + No_Actualizar_Salario + "', '" + Parametros.P_Tipo_Nomina_ID + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Nomina_ID + "', " + Parametros.P_No_Nomina + ", '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Actualizacion) + "', '" + Parametros.P_Usuario + "', SYSDATE)";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    if (Parametros.P_Listado_Sindicatos != null && Parametros.P_Listado_Sindicatos.Count > 0) {
                        Int32 Act_Salario_Detalle_ID = Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Nom_Act_Sal_Det.Tabla_Ope_Nom_Act_Sal_Det, Ope_Nom_Act_Sal_Det.Campo_Act_Salario_Detalle_ID, 50));
                        foreach(Cls_Cat_Nom_Sindicatos_Negocio Sindicato in Parametros.P_Listado_Sindicatos){
                            if (!Validar_Actualizacion_Sindicato(Sindicato, Parametros)) {
                                Mi_SQL = "INSERT INTO " + Ope_Nom_Act_Sal_Det.Tabla_Ope_Nom_Act_Sal_Det + " (" + Ope_Nom_Act_Sal_Det.Campo_Act_Salario_Detalle_ID + ", " + Ope_Nom_Act_Sal_Det.Campo_No_Actualizar_Salario;
                                Mi_SQL = Mi_SQL + ", " + Ope_Nom_Act_Sal_Det.Campo_Sindicato_ID + ") VALUES ( " + Act_Salario_Detalle_ID + ", '" + No_Actualizar_Salario + "', '" + Sindicato.P_Sindicato_ID + "')";
                                Cmd.CommandText = Mi_SQL;
                                Cmd.ExecuteNonQuery();
                                Act_Salario_Detalle_ID = Convert.ToInt32(Convertir_A_Formato_ID(Convert.ToInt32(Act_Salario_Detalle_ID) + 1, 50));                            
                            }
                        }
                    }
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
                        Mensaje = "Error al intentar dar de Alta un Registro. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    }
                    //Indicamos el mensaje 
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Actualizacion_Sindicato
            ///DESCRIPCIÓN: Valida la Actualización de un Sindicato, si ya fue hecha o no para un Tipo de Nomina
            ///             y una Nomina especifica.
            ///PARAMETROS:     
            ///             1.Parametros.   Contiene todos los datos para generar el registro.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 07/Abril/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************    
            public static Boolean Validar_Actualizacion_Sindicato(Cls_Cat_Nom_Sindicatos_Negocio Sindicato, Cls_Ope_Nom_Actualizar_Salario_Negocio Actualizacion) { 
                String Mi_SQL = null;
                DataSet Ds_Sindicato = null;
                Boolean Existe_Actualizacion = false;
                try {
                    Mi_SQL = "SELECT * FROM " + Ope_Nom_Act_Sal_Det.Tabla_Ope_Nom_Act_Sal_Det + " WHERE " + Ope_Nom_Act_Sal_Det.Campo_Sindicato_ID + " = '" + Sindicato.P_Sindicato_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Nom_Act_Sal_Det.Campo_No_Actualizar_Salario + " IN ( SELECT " + Ope_Nom_Actualizar_Salario.Campo_No_Actualizar_Salario + " FROM ";
                    Mi_SQL = Mi_SQL + Ope_Nom_Actualizar_Salario.Tabla_Ope_Nom_Actualizar_Salario + " WHERE " + Ope_Nom_Actualizar_Salario.Campo_Nomina_ID + " = '" + Actualizacion.P_Nomina_ID + "' ";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Nom_Actualizar_Salario.Campo_Tipo_Nomina_ID + " = '" + Actualizacion.P_Tipo_Nomina_ID + "' )";
                    Ds_Sindicato = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Sindicato != null) {
                        if (Ds_Sindicato.Tables.Count > 0) {
                            if (Ds_Sindicato.Tables[0].Rows.Count > 0) {
                                Existe_Actualizacion = true;
                            }
                        }
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Existe_Actualizacion;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Obtener_Listado_Sindicatos
            ///DESCRIPCIÓN: Hace la consulta de los Sindicatos que faltan por ser actualizados
            ///             dependiendo de los parametros pasados.
            ///PARAMETROS:     
            ///             1.Parametros.   Contiene todos los datos para generar el registro.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 09/Abril/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************    
            public static DataTable Obtener_Listado_Sindicatos(Cls_Ope_Nom_Actualizar_Salario_Negocio Parametros) { 
                String Mi_SQL = null;
                DataSet Ds_Sindicatos = null;
                DataTable Dt_Sindicatos = new DataTable();
                try {
                    Mi_SQL = "SELECT";
                    Ds_Sindicatos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }               
               return Dt_Sindicatos;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
            ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
            ///PARAMETROS:     
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
            ///PARAMETROS:     
            ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
            ///             2. Longitud_ID. Longitud que tendra el ID. 
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 10/Marzo/2010 
            ///MODIFICO             : 
            ///FECHA_MODIFICO       : 
            ///CAUSA_MODIFICACIÓN   
            ///
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

        #endregion
    }

}