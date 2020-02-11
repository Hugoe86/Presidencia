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
using Presidencia.Catalogo_Conceptos.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Conceptos_Datos
/// </summary>
namespace Presidencia.Catalogo_Conceptos.Datos
{
    public class Cls_Cat_Pre_Conceptos_Datos{

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Concepto
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo concepto
        ///PARAMETROS:     
        ///             1. Concepto.    Instancia de la Clase de Negocio de Cls_Cat_Pre_Conceptos_Negocio
        ///                             con los datos del Concepto que va a ser dado de Alta.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 27/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Concepto(Cls_Cat_Pre_Conceptos_Negocio Concepto) {
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
                String Concepto_ID = Obtener_ID_Consecutivo(Cat_Pre_Conceptos.Tabla_Cat_Pre_Conceptos, Cat_Pre_Conceptos.Campo_Concepto_Predial_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Conceptos.Tabla_Cat_Pre_Conceptos + " (";
                Mi_SQL = Mi_SQL + Cat_Pre_Conceptos.Campo_Concepto_Predial_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Conceptos.Campo_Identificador + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Conceptos.Campo_Tipo_Concepto + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Conceptos.Campo_Descripcion + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Conceptos.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Conceptos.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Conceptos.Campo_Fecha_Creo + ") VALUES ('";
                Mi_SQL = Mi_SQL + Concepto_ID + "', '";
                Mi_SQL = Mi_SQL + Concepto.P_Identificador + "', '";
                Mi_SQL = Mi_SQL + Concepto.P_Tipo_Concepto + "', '";
                Mi_SQL = Mi_SQL + Concepto.P_Descripcion + "', '";
                Mi_SQL = Mi_SQL + Concepto.P_Estatus + "', '";
                Mi_SQL = Mi_SQL + Concepto.P_Usuario + "', SYSDATE)";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                if (Concepto.P_Conceptos_Impuestos_Predial != null && Concepto.P_Conceptos_Impuestos_Predial.Rows.Count > 0) {
                    String Impuesto_Predial_ID = Obtener_ID_Consecutivo(Cat_Pre_Conceptos_Imp_Predia.Tabla_Cat_Pre_Conceptos_Imp_Predia, Cat_Pre_Conceptos_Imp_Predia.Campo_Impuesto_ID_Predial, 5); 
                    for (int cnt = 0; cnt < Concepto.P_Conceptos_Impuestos_Predial.Rows.Count; cnt++){
                        Mi_SQL = "INSERT INTO " + Cat_Pre_Conceptos_Imp_Predia.Tabla_Cat_Pre_Conceptos_Imp_Predia + " (";
                        Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Predia.Campo_Impuesto_ID_Predial + ", ";
                        Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Predia.Campo_Concepto_Predial_ID + ", ";
                        Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Predia.Campo_Año + ", ";
                        Mi_SQL = Mi_SQL + Convert.ToDouble(Cat_Pre_Conceptos_Imp_Predia.Campo_Tasa) + ", ";
                        Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Predia.Campo_Usuario_Creo + ", ";
                        Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Predia.Campo_Fecha_Creo + ") VALUES ('";
                        Mi_SQL = Mi_SQL + Impuesto_Predial_ID + "', '";
                        Mi_SQL = Mi_SQL + Concepto_ID + "', ";
                        Mi_SQL = Mi_SQL + Concepto.P_Conceptos_Impuestos_Predial.Rows[cnt][1] + ", ";
                        Mi_SQL = Mi_SQL + Concepto.P_Conceptos_Impuestos_Predial.Rows[cnt][2] + ", '";
                        Mi_SQL = Mi_SQL + Concepto.P_Usuario + "', SYSDATE )";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        Impuesto_Predial_ID = Convertir_A_Formato_ID(Convert.ToInt32(Impuesto_Predial_ID) + 1, 5);
                    }        
                }
                if (Concepto.P_Conceptos_Impuestos_Traslacion != null && Concepto.P_Conceptos_Impuestos_Traslacion.Rows.Count > 0) {
                    String Impuesto_Traslacion_ID = Obtener_ID_Consecutivo(Cat_Pre_Conceptos_Imp_Trasl.Tabla_Cat_Pre_Conceptos_Imp_Trasl, Cat_Pre_Conceptos_Imp_Trasl.Campo_Impuesto_ID_Traslacion, 5);
                    for (int cnt = 0; cnt < Concepto.P_Conceptos_Impuestos_Traslacion.Rows.Count; cnt++){
                        Mi_SQL = "INSERT INTO " + Cat_Pre_Conceptos_Imp_Trasl.Tabla_Cat_Pre_Conceptos_Imp_Trasl + " (";
                        Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Trasl.Campo_Impuesto_ID_Traslacion + ", ";
                        Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Trasl.Campo_Concepto_Predial_ID + ", ";
                        Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Trasl.Campo_Año + ", ";
                        Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Trasl.Campo_Tasa + ", ";
                        Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Trasl.Campo_Deducible_Normal + ", ";
                        Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Trasl.Campo_Deducible_Interes_Social + ", ";
                        Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Trasl.Campo_Deducible_20_Salarios + ", ";
                        Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Predia.Campo_Usuario_Creo + ", ";
                        Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Predia.Campo_Fecha_Creo + ") VALUES ('";
                        Mi_SQL = Mi_SQL + Impuesto_Traslacion_ID + "', '";
                        Mi_SQL = Mi_SQL + Concepto_ID + "', ";
                        Mi_SQL = Mi_SQL + Concepto.P_Conceptos_Impuestos_Traslacion.Rows[cnt][1] + ", ";
                        Mi_SQL = Mi_SQL + Concepto.P_Conceptos_Impuestos_Traslacion.Rows[cnt][2] + ", ";
                        Mi_SQL = Mi_SQL + Convert.ToDouble(Concepto.P_Conceptos_Impuestos_Traslacion.Rows[cnt][3]) + ", ";
                        Mi_SQL = Mi_SQL + Convert.ToDouble(Concepto.P_Conceptos_Impuestos_Traslacion.Rows[cnt][4]) + ", ";
                        Mi_SQL = Mi_SQL + Convert.ToDouble(Concepto.P_Conceptos_Impuestos_Traslacion.Rows[cnt][5]) + ", '";
                        Mi_SQL = Mi_SQL + Concepto.P_Usuario + "', SYSDATE )";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        Impuesto_Traslacion_ID = Convertir_A_Formato_ID(Convert.ToInt32(Impuesto_Traslacion_ID) + 1, 5);
                    }
                }
                Trans.Commit();
            }catch(OracleException Ex){
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
                    Mensaje = "Error al intentar dar de Alta un Concepto. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                if (Cn.State == ConnectionState.Open) {
                    Cn.Close();
                }
            }
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Concepto
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Concepto
        ///PARAMETROS:     
        ///             1. Concepto.  Instancia de la Clase de Negocio de Conceptos 
        ///                             con los datos del Concepto que va a ser Actualizado.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 30/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Concepto(Cls_Cat_Pre_Conceptos_Negocio Concepto){
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
                Cls_Cat_Pre_Conceptos_Negocio Concepto_Tmp = Consultar_Datos_Concepto(Concepto);
                String Mi_SQL = "UPDATE " + Cat_Pre_Conceptos.Tabla_Cat_Pre_Conceptos + " SET " + Cat_Pre_Conceptos.Campo_Identificador + " = '" + Concepto.P_Identificador + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Conceptos.Campo_Tipo_Concepto + " = '" + Concepto.P_Tipo_Concepto + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Conceptos.Campo_Estatus + " = '" + Concepto.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Conceptos.Campo_Descripcion + " = '" + Concepto.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Conceptos.Campo_Usuario_Modifico + " = '" + Concepto.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Conceptos.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Conceptos.Campo_Concepto_Predial_ID + " = '" + Concepto.P_Concepto_Predial_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Concepto_Tmp = Obtener_Conceptos_Impuestos_Eliminados(Concepto_Tmp, Concepto);
                for (int cnt = 0; cnt < Concepto_Tmp.P_Conceptos_Impuestos_Predial.Rows.Count; cnt++){
                    Mi_SQL = "DELETE FROM " + Cat_Pre_Conceptos_Imp_Predia.Tabla_Cat_Pre_Conceptos_Imp_Predia;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Conceptos_Imp_Predia.Campo_Impuesto_ID_Predial + " = '" + Concepto_Tmp.P_Conceptos_Impuestos_Predial.Rows[cnt][0].ToString() + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }
                for (int cnt = 0; cnt < Concepto_Tmp.P_Conceptos_Impuestos_Traslacion.Rows.Count; cnt++){
                    Mi_SQL = "DELETE FROM " + Cat_Pre_Conceptos_Imp_Trasl.Tabla_Cat_Pre_Conceptos_Imp_Trasl;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Conceptos_Imp_Trasl.Campo_Impuesto_ID_Traslacion + " = '" + Concepto_Tmp.P_Conceptos_Impuestos_Traslacion.Rows[cnt][0].ToString() + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }
                if (Concepto.P_Conceptos_Impuestos_Predial != null && Concepto.P_Conceptos_Impuestos_Predial.Rows.Count > 0){
                    String Impuesto_Predial_ID = Obtener_ID_Consecutivo(Cat_Pre_Conceptos_Imp_Predia.Tabla_Cat_Pre_Conceptos_Imp_Predia, Cat_Pre_Conceptos_Imp_Predia.Campo_Impuesto_ID_Predial, 5); 
                    for (int cnt = 0; cnt < Concepto.P_Conceptos_Impuestos_Predial.Rows.Count; cnt++){
                        if (Concepto.P_Conceptos_Impuestos_Predial.Rows[cnt][0].ToString().Trim().Equals("")){
                            
                            Mi_SQL = "INSERT INTO " + Cat_Pre_Conceptos_Imp_Predia.Tabla_Cat_Pre_Conceptos_Imp_Predia;
                            Mi_SQL = Mi_SQL + " (" + Cat_Pre_Conceptos_Imp_Predia.Campo_Impuesto_ID_Predial + ", " + Cat_Pre_Conceptos_Imp_Predia.Campo_Concepto_Predial_ID;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Conceptos_Imp_Predia.Campo_Año + ", " + Cat_Pre_Conceptos_Imp_Predia.Campo_Tasa;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Conceptos_Imp_Predia.Campo_Usuario_Creo + ", " + Cat_Pre_Conceptos_Imp_Predia.Campo_Fecha_Creo + ")";
                            Mi_SQL = Mi_SQL + " VALUES ('" + Impuesto_Predial_ID + "', '" + Concepto.P_Concepto_Predial_ID + "', " + Concepto.P_Conceptos_Impuestos_Predial.Rows[cnt][1].ToString() + ", " + Concepto.P_Conceptos_Impuestos_Predial.Rows[cnt][2].ToString();
                            Mi_SQL = Mi_SQL + ",'" + Concepto.P_Usuario + "', SYSDATE";
                            Mi_SQL = Mi_SQL + ")";
                            Impuesto_Predial_ID = Convertir_A_Formato_ID(Convert.ToInt32(Impuesto_Predial_ID) + 1, 5);
                        }else{
                            Mi_SQL = "UPDATE " + Cat_Pre_Conceptos_Imp_Predia.Tabla_Cat_Pre_Conceptos_Imp_Predia + " SET " + Cat_Pre_Conceptos_Imp_Predia.Campo_Año + " = " + Concepto.P_Conceptos_Impuestos_Predial.Rows[cnt][1].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Conceptos_Imp_Predia.Campo_Tasa + " = " + Concepto.P_Conceptos_Impuestos_Predial.Rows[cnt][2].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Conceptos_Imp_Predia.Campo_Usuario_Modifico + " = '" + Concepto.P_Usuario + "'";
                            Mi_SQL = Mi_SQL + "," + Cat_Pre_Conceptos_Imp_Predia.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Conceptos_Imp_Predia.Campo_Impuesto_ID_Predial + " = '" + Concepto.P_Conceptos_Impuestos_Predial.Rows[cnt][0].ToString().Trim() + "'";
                        }
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                }
                if (Concepto.P_Conceptos_Impuestos_Traslacion != null && Concepto.P_Conceptos_Impuestos_Traslacion.Rows.Count > 0){
                    String Impuesto_Traslacion_ID = Obtener_ID_Consecutivo(Cat_Pre_Conceptos_Imp_Trasl.Tabla_Cat_Pre_Conceptos_Imp_Trasl, Cat_Pre_Conceptos_Imp_Trasl.Campo_Impuesto_ID_Traslacion, 5);
                    for (int cnt = 0; cnt < Concepto.P_Conceptos_Impuestos_Traslacion.Rows.Count; cnt++){
                        if (Concepto.P_Conceptos_Impuestos_Traslacion.Rows[cnt][0].ToString().Trim().Equals("")){
                            Mi_SQL = "INSERT INTO " + Cat_Pre_Conceptos_Imp_Trasl.Tabla_Cat_Pre_Conceptos_Imp_Trasl;
                            Mi_SQL = Mi_SQL + " (" + Cat_Pre_Conceptos_Imp_Trasl.Campo_Impuesto_ID_Traslacion + ", " + Cat_Pre_Conceptos_Imp_Trasl.Campo_Concepto_Predial_ID;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Conceptos_Imp_Trasl.Campo_Año + ", " + Cat_Pre_Conceptos_Imp_Trasl.Campo_Tasa;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Conceptos_Imp_Trasl.Campo_Deducible_Normal+ ", " + Cat_Pre_Conceptos_Imp_Trasl.Campo_Deducible_Interes_Social;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Conceptos_Imp_Trasl.Campo_Usuario_Creo + ", " + Cat_Pre_Conceptos_Imp_Trasl.Campo_Fecha_Creo + ")";
                            Mi_SQL = Mi_SQL + " VALUES ('" + Impuesto_Traslacion_ID + "', '" + Concepto.P_Concepto_Predial_ID + "', " + Concepto.P_Conceptos_Impuestos_Traslacion.Rows[cnt][1].ToString() + ", " + Concepto.P_Conceptos_Impuestos_Traslacion.Rows[cnt][2].ToString();
                            Mi_SQL = Mi_SQL + ", " + Concepto.P_Conceptos_Impuestos_Traslacion.Rows[cnt][3].ToString() + ", " + Concepto.P_Conceptos_Impuestos_Traslacion.Rows[cnt][4].ToString();
                            Mi_SQL = Mi_SQL + ",'" + Concepto.P_Usuario + "', SYSDATE";
                            Mi_SQL = Mi_SQL + ")";
                            Impuesto_Traslacion_ID = Convertir_A_Formato_ID(Convert.ToInt32(Impuesto_Traslacion_ID) + 1, 5);
                        }else{
                            Mi_SQL = "UPDATE " + Cat_Pre_Conceptos_Imp_Trasl.Tabla_Cat_Pre_Conceptos_Imp_Trasl + " SET " + Cat_Pre_Conceptos_Imp_Trasl.Campo_Año + " = " + Concepto.P_Conceptos_Impuestos_Traslacion.Rows[cnt][1].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Conceptos_Imp_Trasl.Campo_Tasa + " = " + Concepto.P_Conceptos_Impuestos_Traslacion.Rows[cnt][2].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Conceptos_Imp_Trasl.Campo_Deducible_Normal + " = " + Concepto.P_Conceptos_Impuestos_Traslacion.Rows[cnt][3].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Conceptos_Imp_Trasl.Campo_Deducible_Interes_Social + " = " + Concepto.P_Conceptos_Impuestos_Traslacion.Rows[cnt][4].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Conceptos_Imp_Trasl.Campo_Usuario_Modifico + " = '" + Concepto.P_Usuario +"'";
                            Mi_SQL = Mi_SQL + "," + Cat_Pre_Conceptos_Imp_Trasl.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Conceptos_Imp_Trasl.Campo_Impuesto_ID_Traslacion + " = '" + Concepto.P_Conceptos_Impuestos_Traslacion.Rows[cnt][0].ToString().Trim() + "'";
                        }
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
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
                    Mensaje = "Error al intentar modificar un Concepto. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Conceptos_Impuestos_Eliminados
        ///DESCRIPCIÓN: Obtiene la lista de los Conceptos Impuestos que fueron dados de alta en la Actualizacion de los
        ///             Conceptos
        ///PARAMETROS:     
        ///             1. Actuales.        Conceptos que se usa para saber los Conceptos_Impuestos que estan en 
        ///                                 la Base de Datos antes de la Modificacion.
        ///             2. Actualizados.    Conceptos que se usa para saber los Conceptos_Impuestos que estaran en 
        ///                                 la Base de Datos despues de la Modificacion.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 30/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static Cls_Cat_Pre_Conceptos_Negocio Obtener_Conceptos_Impuestos_Eliminados(Cls_Cat_Pre_Conceptos_Negocio Actuales, Cls_Cat_Pre_Conceptos_Negocio Actualizados){
            Cls_Cat_Pre_Conceptos_Negocio Concepto = new Cls_Cat_Pre_Conceptos_Negocio();
            DataTable Tabla_Impuestos_Predial = new DataTable();
            Tabla_Impuestos_Predial.Columns.Add("IMPUESTO_ID_PREDIAL", Type.GetType("System.String"));
            Tabla_Impuestos_Predial.Columns.Add("ANIO", Type.GetType("System.String"));
            Tabla_Impuestos_Predial.Columns.Add("TASA", Type.GetType("System.String"));
            for (int cnt1 = 0; cnt1 < Actuales.P_Conceptos_Impuestos_Predial.Rows.Count; cnt1++){
                bool eliminar = true;
                for (int cnt2 = 0; cnt2 < Actualizados.P_Conceptos_Impuestos_Predial.Rows.Count; cnt2++){
                    if (!Actualizados.P_Conceptos_Impuestos_Predial.Rows[cnt2][0].ToString().Equals("")){
                        if (Actuales.P_Conceptos_Impuestos_Predial.Rows[cnt1][0].ToString().Equals(Actualizados.P_Conceptos_Impuestos_Predial.Rows[cnt2][0].ToString())){
                            eliminar = false;
                            break;
                        }
                    }
                }
                if (eliminar){
                    DataRow fila = Tabla_Impuestos_Predial.NewRow();
                    fila["IMPUESTO_ID_PREDIAL"] = Actuales.P_Conceptos_Impuestos_Predial.Rows[cnt1][0].ToString();
                    fila["ANIO"] = Actuales.P_Conceptos_Impuestos_Predial.Rows[cnt1][1].ToString();
                    fila["TASA"] = Actuales.P_Conceptos_Impuestos_Predial.Rows[cnt1][2].ToString();
                    Tabla_Impuestos_Predial.Rows.Add(fila);
                }
            }
            DataTable Tabla_Impuestos_Traslacion = new DataTable();
            Tabla_Impuestos_Traslacion.Columns.Add("IMPUESTO_ID_TRASLACION", Type.GetType("System.String"));
            Tabla_Impuestos_Traslacion.Columns.Add("ANIO", Type.GetType("System.String"));
            Tabla_Impuestos_Traslacion.Columns.Add("TASA", Type.GetType("System.String"));
            Tabla_Impuestos_Traslacion.Columns.Add("DEDUCIBLE_NORMAL", Type.GetType("System.String"));
            Tabla_Impuestos_Traslacion.Columns.Add("DEDUCIBLE_INTERES_SOCIAL", Type.GetType("System.String"));
            for (int cnt1 = 0; cnt1 < Actuales.P_Conceptos_Impuestos_Traslacion.Rows.Count; cnt1++) {
                bool eliminar = true;
                for (int cnt2 = 0; cnt2 < Actualizados.P_Conceptos_Impuestos_Traslacion.Rows.Count; cnt2++) {
                    if (!Actualizados.P_Conceptos_Impuestos_Traslacion.Rows[cnt2][0].ToString().Equals("")) {
                        if (Actuales.P_Conceptos_Impuestos_Traslacion.Rows[cnt1][0].ToString().Equals(Actualizados.P_Conceptos_Impuestos_Traslacion.Rows[cnt2][0].ToString())) {
                            eliminar = false;
                            break;
                        }
                    }
                }
                if (eliminar) {
                    DataRow fila = Tabla_Impuestos_Traslacion.NewRow();
                    fila["IMPUESTO_ID_TRASLACION"] = Actuales.P_Conceptos_Impuestos_Traslacion.Rows[cnt1][0].ToString();
                    fila["ANIO"] = Actuales.P_Conceptos_Impuestos_Traslacion.Rows[cnt1][1].ToString();
                    fila["TASA"] = Actuales.P_Conceptos_Impuestos_Traslacion.Rows[cnt1][2].ToString();
                    fila["DEDUCIBLE_NORMAL"] = Actuales.P_Conceptos_Impuestos_Traslacion.Rows[cnt1][3].ToString();
                    fila["DEDUCIBLE_INTERES_SOCIAL"] = Actuales.P_Conceptos_Impuestos_Traslacion.Rows[cnt1][4].ToString();
                    Tabla_Impuestos_Traslacion.Rows.Add(fila);
                }
            }
            Concepto.P_Conceptos_Impuestos_Predial = Tabla_Impuestos_Predial;
            Concepto.P_Conceptos_Impuestos_Traslacion = Tabla_Impuestos_Traslacion;
            return Concepto;
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Conceptos
        ///DESCRIPCIÓN: Obtiene todos los conceptos que estan dadas de alta en la Base de Datos
        ///PARAMETROS:   
        ///             1.  Concepto.   Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                             caso el filtro es el Identificador.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 27/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Conceptos(Cls_Cat_Pre_Conceptos_Negocio Concepto){
            DataTable Tabla = new DataTable();
            Boolean Primer_Filtro = true;
            try{
                String Mi_SQL = "SELECT " + Cat_Pre_Conceptos.Campo_Concepto_Predial_ID + " AS CONCEPTO_PREDIAL_ID, " + Cat_Pre_Conceptos.Campo_Identificador + " AS IDENTIFICADOR";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Conceptos.Campo_Estatus + " AS ESTATUS FROM " + Cat_Pre_Conceptos.Tabla_Cat_Pre_Conceptos;
                if (Concepto.P_Identificador != null && Concepto.P_Identificador != ""){
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Conceptos.Campo_Identificador + " LIKE '%" + Concepto.P_Identificador + "%'";
                    Primer_Filtro = false;
                }
                if (Concepto.P_Tipo_Concepto != null && Concepto.P_Tipo_Concepto != "")
                {
                    if (Primer_Filtro)
                    {
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Conceptos.Campo_Tipo_Concepto + " = '" + Concepto.P_Tipo_Concepto + "'";
                        Primer_Filtro = false;
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Conceptos.Campo_Tipo_Concepto + " = '" + Concepto.P_Tipo_Concepto + "'";
                    }
                }
                if (Concepto.P_Estatus != null && Concepto.P_Estatus != "")
                {
                    if (Primer_Filtro)
                    {
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Conceptos.Campo_Estatus + " = '" + Concepto.P_Estatus + "'";
                        Primer_Filtro = false;
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Conceptos.Campo_Estatus + " = '" + Concepto.P_Estatus + "'";
                    }
                }
                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null){
                    Tabla = dataSet.Tables[0];
                }
            }catch(Exception Ex){
                String Mensaje = "Error al intentar consultar los registros de Conceptos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;    
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Concepto
        ///DESCRIPCIÓN: Obtiene a detalle un  Concepto.
        ///PARAMETROS:   
        ///             1. Concepto.   Concepto que se va ver a Detalle.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 27/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Conceptos_Negocio Consultar_Datos_Concepto(Cls_Cat_Pre_Conceptos_Negocio Concepto){
            Cls_Cat_Pre_Conceptos_Negocio R_Concepto = new Cls_Cat_Pre_Conceptos_Negocio();
            OracleDataReader Data_Reader;
            String Mi_SQL = "SELECT " +  Cat_Pre_Conceptos.Campo_Identificador;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Conceptos.Campo_Tipo_Concepto + ", "+ Cat_Pre_Conceptos.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Conceptos.Campo_Descripcion + " FROM " + Cat_Pre_Conceptos.Tabla_Cat_Pre_Conceptos;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Conceptos.Campo_Concepto_Predial_ID + " = '" + Concepto.P_Concepto_Predial_ID + "'";
            try {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Concepto.P_Concepto_Predial_ID = Concepto.P_Concepto_Predial_ID;
                while (Data_Reader.Read()){
                    R_Concepto.P_Identificador = Data_Reader[Cat_Pre_Conceptos.Campo_Identificador].ToString();
                    R_Concepto.P_Tipo_Concepto = Data_Reader[Cat_Pre_Conceptos.Campo_Tipo_Concepto].ToString();
                    R_Concepto.P_Estatus = Data_Reader[Cat_Pre_Conceptos.Campo_Estatus].ToString();
                    R_Concepto.P_Descripcion = Data_Reader[Cat_Pre_Conceptos.Campo_Descripcion].ToString();
                }
                Data_Reader.Close();
                Mi_SQL = "SELECT " + Cat_Pre_Conceptos_Imp_Predia.Campo_Impuesto_ID_Predial + " AS IMPUESTO_ID_PREDIAL, " + Cat_Pre_Conceptos_Imp_Predia.Campo_Año + " AS ANIO";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Conceptos_Imp_Predia.Campo_Tasa + " AS TASA FROM " + Cat_Pre_Conceptos_Imp_Predia.Tabla_Cat_Pre_Conceptos_Imp_Predia;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Conceptos_Imp_Predia.Campo_Concepto_Predial_ID + " = '" + Concepto.P_Concepto_Predial_ID + "'";
                if (Concepto.P_Año != null && Concepto.P_Año != "") {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Conceptos_Imp_Predia.Campo_Año + " = '" + Concepto.P_Año + "'";
                }
                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet == null) { 
                    R_Concepto.P_Conceptos_Impuestos_Predial = new DataTable();
                } else {
                    R_Concepto.P_Conceptos_Impuestos_Predial = dataSet.Tables[0];
                }
                dataSet = null;
                Mi_SQL = "SELECT " + Cat_Pre_Conceptos_Imp_Trasl.Campo_Impuesto_ID_Traslacion + " AS IMPUESTO_ID_TRASLACION, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Trasl.Campo_Año + " AS ANIO, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Trasl.Campo_Tasa + " AS TASA, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Trasl.Campo_Deducible_Normal + " AS DEDUCIBLE_NORMAL, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Trasl.Campo_Deducible_Interes_Social + " AS DEDUCIBLE_INTERES_SOCIAL, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Trasl.Campo_Deducible_Interes_Social + " AS DEDUCIBLE_20_SALARIOS";
                Mi_SQL = Mi_SQL +" FROM " + Cat_Pre_Conceptos_Imp_Trasl.Tabla_Cat_Pre_Conceptos_Imp_Trasl;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Conceptos_Imp_Trasl.Campo_Concepto_Predial_ID + " = '" + Concepto.P_Concepto_Predial_ID + "'";
                if (Concepto.P_Año != null && Concepto.P_Año != "") {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Conceptos_Imp_Predia.Campo_Año + " = '" + Concepto.P_Año + "'";
                }
                dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet == null){
                    R_Concepto.P_Conceptos_Impuestos_Traslacion = new DataTable();
                }else{
                    R_Concepto.P_Conceptos_Impuestos_Traslacion = dataSet.Tables[0];
                }
            } catch (Exception Ex) {
                String Mensaje = "Error al intentar consultar el Concepto. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Concepto;
        }

        /////*******************************************************************************
        /////NOMBRE DE LA FUNCIÓN   : Consultar_Tasas_Traslado
        /////DESCRIPCIÓN            : Obtiene las tasas de Traslado
        /////PARAMETROS             : Concepto, instancia de Cls_Cat_Pre_Conceptos_Negocio
        /////CREO                   : Antonio Salvador Benavides Guardado
        /////FECHA_CREO             : 01/Diciembre/2010 
        /////MODIFICO:
        /////FECHA_MODIFICO
        /////CAUSA_MODIFICACIÓN
        /////*******************************************************************************
        public static DataTable Consultar_Tasas_Traslado(Cls_Cat_Pre_Conceptos_Negocio Concepto)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;
            String Mi_SQL_Campos_Foraneos = "";

            try
            {
                if (Concepto.P_Incluir_Campos_Foraneos)
                {
                    Mi_SQL_Campos_Foraneos = Mi_SQL_Campos_Foraneos + "(SELECT " + Cat_Pre_Conceptos.Campo_Identificador + " FROM " + Cat_Pre_Conceptos.Tabla_Cat_Pre_Conceptos + " WHERE " + Cat_Pre_Conceptos.Campo_Concepto_Predial_ID + " = " + Cat_Pre_Conceptos_Imp_Trasl.Tabla_Cat_Pre_Conceptos_Imp_Trasl + "." + Cat_Pre_Conceptos_Imp_Trasl.Campo_Concepto_Predial_ID + ") AS IDENTIFICADOR, ";
                }
                if (Concepto.P_Campos_Dinamicos != null && Concepto.P_Campos_Dinamicos != "")
                {
                    Mi_SQL = "SELECT " + Mi_SQL_Campos_Foraneos + Concepto.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL = "SELECT " + Mi_SQL_Campos_Foraneos;
                    Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Trasl.Campo_Impuesto_ID_Traslacion + " AS IMPUESTO_ID_TRASLACION, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Trasl.Campo_Concepto_Predial_ID + " AS CONCEPTO_PREDIAL_ID, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Trasl.Campo_Deducible_Normal + " AS DEDUCIBLE_NORMAL, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Trasl.Campo_Deducible_Interes_Social + " AS DEDUCIBLE_INTERES_SOCIAL, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Trasl.Campo_Deducible_20_Salarios + " AS DEDUCIBLE_20_SALARIOS, ";
                    //Mi_SQL = Mi_SQL + " LPAD(CAST(" + Cat_Pre_Conceptos_Imp_Trasl.Campo_Deducible_Normal + " AS VARCHAR(20)),9,'*') AS DEDUCIBLE_NORMAL, ";
                    //Mi_SQL = Mi_SQL + " LPAD(CAST(" + Cat_Pre_Conceptos_Imp_Trasl.Campo_Deducible_Interes_Social + " AS VARCHAR(20)),9,'*') AS DEDUCIBLE_INTERES_SOCIAL, ";
                    //Mi_SQL = Mi_SQL + " LPAD(CAST(" + Cat_Pre_Conceptos_Imp_Trasl.Campo_Deducible_20_Salarios + " AS VARCHAR(20)),9,'*') AS DEDUCIBLE_20_SALARIOS, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Trasl.Campo_Año + " AS ANIO, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Conceptos_Imp_Trasl.Campo_Tasa + " AS TASA";
                }
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Conceptos_Imp_Trasl.Tabla_Cat_Pre_Conceptos_Imp_Trasl;
                if (Concepto.P_Filtros_Dinamicos != null && Concepto.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Concepto.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Conceptos_Imp_Trasl.Campo_Concepto_Predial_ID + " IN (SELECT " + Cat_Pre_Conceptos.Campo_Concepto_Predial_ID + " FROM " + Cat_Pre_Conceptos.Tabla_Cat_Pre_Conceptos + " WHERE " + Cat_Pre_Conceptos.Campo_Tipo_Concepto + " IN ('TRASLADO DOMINIO', 'AMBOS'))";
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
                if (Concepto.P_Agrupar_Dinamico != null && Concepto.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL = Mi_SQL + " GROUP BY " + Concepto.P_Agrupar_Dinamico;
                }
                if (Concepto.P_Ordenar_Dinamico != null && Concepto.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL = Mi_SQL + " ORDER BY " + Concepto.P_Ordenar_Dinamico;
                }
                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null)
                {
                    Tabla = dataSet.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar las Tasas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Concepto
        ///DESCRIPCIÓN: Elimina un Concepto
        ///PARAMETROS:   
        ///             1. Concepto.   Concepto que se va eliminar.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 30/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Concepto(Cls_Cat_Pre_Conceptos_Negocio Concepto){
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
                String Mi_SQL = "DELETE FROM " + Cat_Pre_Conceptos_Imp_Predia.Tabla_Cat_Pre_Conceptos_Imp_Predia;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Conceptos_Imp_Predia.Campo_Concepto_Predial_ID + " = '" + Concepto.P_Concepto_Predial_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Mi_SQL = "DELETE FROM " + Cat_Pre_Conceptos_Imp_Trasl.Tabla_Cat_Pre_Conceptos_Imp_Trasl;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Conceptos_Imp_Trasl.Campo_Concepto_Predial_ID + " = '" + Concepto.P_Concepto_Predial_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Mi_SQL = "DELETE FROM " + Cat_Pre_Conceptos.Tabla_Cat_Pre_Conceptos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Conceptos.Campo_Concepto_Predial_ID + " = '" + Concepto.P_Concepto_Predial_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            } catch (OracleException Ex){
                Trans.Rollback();
                if (Ex.Code == 547) {
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos";
                } else {
                    Mensaje = "Error al intentar eliminar el Concepto. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                throw new Exception(Mensaje);
            } catch (Exception Ex) {
                Mensaje = "Error al intentar eliminar el Concepto. Error: [" + Ex.Message + "]"; //"Error general en la base de datos" //"Error general en la base de datos"
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