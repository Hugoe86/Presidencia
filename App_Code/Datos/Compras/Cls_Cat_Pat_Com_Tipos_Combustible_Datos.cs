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
using Presidencia.Control_Patrimonial_Catalogo_Tipos_Combustible.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

/// <summary>
/// Summary description for Cls_Cat_Pat_Com_Tipos_Combustible_Datos
/// </summary>

namespace Presidencia.Control_Patrimonial_Catalogo_Tipos_Combustible.Datos {

    public class Cls_Cat_Pat_Com_Tipos_Combustible_Datos {

        #region Metodos

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Alta_Tipo_Combustible
            ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo Tipo de Combustible
            ///PARAMETROS           : 
            ///                     1.  Tipo_Combustible.   Contiene los parametros que se van a dar de
            ///                                             Alta en la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 23/Noviembre/2010 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Alta_Tipo_Combustible(Cls_Cat_Pat_Com_Tipos_Combustible_Negocio Tipo_Combustible) {
                String Mensaje = "";
                OracleConnection Cn = new OracleConnection();
                OracleCommand Cmd = new OracleCommand();
                OracleTransaction Trans;
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                String Tipo_Combustible_ID = Obtener_ID_Consecutivo(Cat_Pat_Tipos_Combustible.Tabla_Cat_Pat_Tipos_Combustible, Cat_Pat_Tipos_Combustible.Campo_Tipo_Combustible_ID, 5);
                try {
                    String Mi_SQL = "INSERT INTO " + Cat_Pat_Tipos_Combustible.Tabla_Cat_Pat_Tipos_Combustible + " (" + Cat_Pat_Tipos_Combustible.Campo_Tipo_Combustible_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Combustible.Campo_Descripcion + ", " + Cat_Pat_Tipos_Combustible.Campo_Estatus;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Combustible.Campo_Usuario_Creo + ", " + Cat_Pat_Tipos_Combustible.Campo_Fecha_Creo;
                    Mi_SQL = Mi_SQL + ") VALUES ('" + Tipo_Combustible_ID + "', '" + Tipo_Combustible.P_Descripcion + "','" + Tipo_Combustible.P_Estatus + "'";
                    Mi_SQL = Mi_SQL + ",'" + Tipo_Combustible.P_Usuario + "', SYSDATE)"; 
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
                        Mensaje = "Error al intentar dar de Alta un Tipo de Vehiculo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    }
                    //Indicamos el mensaje 
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Modificar_Tipo_Combustible
            ///DESCRIPCIÓN          :Modifica en la Base de Datos el registro indicado del
            ///                      Tipo de Vehiculo.
            ///PARAMETROS           : 
            ///                     1.  Tipo_Combustible.   Contiene los parametros que se van hacer la
            ///                                             Modificación en la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 23/Noviembre/2010 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Modificar_Tipo_Combustible(Cls_Cat_Pat_Com_Tipos_Combustible_Negocio Tipo_Combustible)
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
                    String Mi_SQL = "UPDATE " + Cat_Pat_Tipos_Combustible.Tabla_Cat_Pat_Tipos_Combustible;
                    Mi_SQL = Mi_SQL + " SET " + Cat_Pat_Tipos_Combustible.Campo_Descripcion + " = '" + Tipo_Combustible.P_Descripcion + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Combustible.Campo_Estatus + " = '" + Tipo_Combustible.P_Estatus + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Combustible.Campo_Usuario_Modifico + " = '" + Tipo_Combustible.P_Usuario + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Combustible.Campo_Fecha_Modifico + " = SYSDATE";
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Tipos_Combustible.Campo_Tipo_Combustible_ID + " = '" + Tipo_Combustible.P_Tipo_Combustible_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Trans.Commit();
                } catch (OracleException Ex) {
                    Trans.Rollback();
                    //variable para el mensaje 
                    //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                    if (Ex.Code == 8152) {
                        Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar";
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
                        Mensaje = "Error al intentar Modificar un Tipo de Combustible. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    }
                    //Indicamos el mensaje 
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_DataTable
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
            ///PARAMETROS           : 
            ///                     1.  Tipo_Combustible.   Contiene los parametros que se van a utilizar para
            ///                                             hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 23/Noviembre/2010 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_DataTable(Cls_Cat_Pat_Com_Tipos_Combustible_Negocio Tipo_Combustible)  {
                String Mi_SQL = null;
                DataSet Ds_Tipos_Combustible = null;
                DataTable Dt_Tipos_Combustible = new DataTable();
                try {
                    if (Tipo_Combustible.P_Tipo_DataTable.Equals("TIPOS_COMBUSTIBLE")) {
                        Mi_SQL = "SELECT " + Cat_Pat_Tipos_Combustible.Campo_Tipo_Combustible_ID + " AS TIPO_COMBUSTIBLE_ID";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Combustible.Campo_Descripcion + " AS DESCRIPCION";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Combustible.Campo_Estatus + " AS ESTATUS";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Tipos_Combustible.Tabla_Cat_Pat_Tipos_Combustible;
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Tipos_Combustible.Campo_Descripcion + " LIKE '%" + Tipo_Combustible.P_Descripcion + "%'";
                        Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pat_Tipos_Combustible.Campo_Tipo_Combustible_ID;
                    } 
                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                        Ds_Tipos_Combustible = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Tipos_Combustible != null) {
                        Dt_Tipos_Combustible = Ds_Tipos_Combustible.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Tipos_Combustible;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Eliminar_Tipo_Combustible
            ///DESCRIPCIÓN          : Elimina un Registro de un Tipo de Combustible
            ///PARAMETROS           : 
            ///                     1.  Tipo_Combustible.   Contiene los parametros que se van a utilizar para
            ///                                             hacer la eliminacion de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 23/Noviembre/2010 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Eliminar_Tipo_Combustible(Cls_Cat_Pat_Com_Tipos_Combustible_Negocio Tipo_Combustible)
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
                    String Mi_SQL = "DELETE FROM " + Cat_Pat_Tipos_Combustible.Tabla_Cat_Pat_Tipos_Combustible;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Tipos_Combustible.Campo_Tipo_Combustible_ID + " = '" + Tipo_Combustible.P_Tipo_Combustible_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Trans.Commit();
                } catch (OracleException Ex) {
                    if (Ex.Code == 547) {
                        Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]";
                    } else {
                        Mensaje = "Error al intentar eliminar el Tipo de Combustible. Error: [" + Ex.Message + "]";
                    }
                    throw new Exception(Mensaje);
                } catch (Exception Ex) {
                    Mensaje = "Error al intentar eliminar el Tipo de Combustible. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
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

        #endregion

    }

}