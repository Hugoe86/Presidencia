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
using Presidencia.Nomina_Catalogo_Plazas.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

/// <summary>
/// Summary description for Cls_Cat_Nom_Plazas_Datos
/// </summary>

namespace Presidencia.Nomina_Catalogo_Plazas.Datos {

    public class Cls_Cat_Nom_Plazas_Datos {

        #region Metodos

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Alta_Plaza
            ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo registro de Plaza
            ///PARAMETROS           : 
            ///                     1.  Parametros.   Contiene los parametros que se van a dar de
            ///                                     Alta en la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 04/Abril/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Alta_Plaza(Cls_Cat_Nom_Plazas_Negocio Parametros) {
                String Mensaje = "";
                OracleConnection Cn = new OracleConnection();
                OracleCommand Cmd = new OracleCommand();
                OracleTransaction Trans;
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                String Plaza_ID = Obtener_ID_Consecutivo(Cat_Nom_Plazas.Tabla_Cat_Nom_Plazas, Cat_Nom_Plazas.Campo_Plaza_ID, 5);
                try {
                    String Mi_SQL = "INSERT INTO " + Cat_Nom_Plazas.Tabla_Cat_Nom_Plazas + " (" + Cat_Nom_Plazas.Campo_Plaza_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Nom_Plazas.Campo_Nombre + ", " + Cat_Nom_Plazas.Campo_Comentarios + ", " + Cat_Nom_Plazas.Campo_Estatus;
                    Mi_SQL = Mi_SQL + ", " + Cat_Nom_Plazas.Campo_Usuario_Creo + ", " + Cat_Nom_Plazas.Campo_Fecha_Creo;
                    Mi_SQL = Mi_SQL + ") VALUES ('" + Plaza_ID + "', '" + Parametros.P_Nombre + "','" + Parametros.P_Comentarios + "','" + Parametros.P_Estatus + "'";
                    Mi_SQL = Mi_SQL + ",'" + Parametros.P_Usuario + "', SYSDATE)"; 
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
                        Mensaje = "Error al intentar dar de Alta una Plaza. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    }
                    //Indicamos el mensaje 
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Modificar_Plaza
            ///DESCRIPCIÓN          : Modifica en la Base de Datos el registro indicado de la Plaza
            ///PARAMETROS           : 
            ///                     1.  Parametros.   Contiene los parametros que se van hacer la
            ///                                     Modificación en la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 04/Abril/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Modificar_Plaza(Cls_Cat_Nom_Plazas_Negocio Parametros) {
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
                    String Mi_SQL = "UPDATE " + Cat_Nom_Plazas.Tabla_Cat_Nom_Plazas;
                    Mi_SQL = Mi_SQL + " SET " + Cat_Nom_Plazas.Campo_Nombre + " = '" + Parametros.P_Nombre + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Nom_Plazas.Campo_Estatus + " = '" + Parametros.P_Estatus + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Nom_Plazas.Campo_Comentarios + " = '" + Parametros.P_Comentarios + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Nom_Plazas.Campo_Usuario_Modifico + " = '" + Parametros.P_Usuario + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Nom_Plazas.Campo_Fecha_Modifico + " = SYSDATE";
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Plazas.Campo_Plaza_ID + " = '" + Parametros.P_Plaza_ID + "'";
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
                        Mensaje = "Error al intentar Modificar una Plaza. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    }
                    //Indicamos el mensaje 
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Plazas
            ///DESCRIPCIÓN          : Obtiene un listado de las plazas y los regresa en un DataTable.
            ///PARAMETROS           : 
            ///                     1.  Parametros. Contiene los parametros que se van a utilizar para
            ///                                     hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 04/Abril/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Plazas(Cls_Cat_Nom_Plazas_Negocio Parametros) {
                String Mi_SQL = null;
                DataSet Ds_Plazas = null;
                DataTable Dt_Plazas = new DataTable();
                try {
                        Mi_SQL = "SELECT " + Cat_Nom_Plazas.Campo_Plaza_ID + " AS PLAZA_ID";
                        Mi_SQL = Mi_SQL + ", " + Cat_Nom_Plazas.Campo_Nombre + " AS NOMBRE";
                        Mi_SQL = Mi_SQL + ", " + Cat_Nom_Plazas.Campo_Estatus + " AS ESTATUS";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Nom_Plazas.Tabla_Cat_Nom_Plazas;
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Plazas.Campo_Nombre + " LIKE '%" + Parametros.P_Nombre + "%'";
                        Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Nom_Plazas.Campo_Nombre; 
                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                        Ds_Plazas = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Plazas != null) {
                        Dt_Plazas = Ds_Plazas.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Plazas;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Eliminar_Plaza
            ///DESCRIPCIÓN          : Elimina un Registro de una Plaza
            ///PARAMETROS           : 
            ///                     1.  Parametros. Contiene los parametros que se van a utilizar para
            ///                                     hacer la eliminacion de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 04/Abril/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Eliminar_Plaza(Cls_Cat_Nom_Plazas_Negocio Parametros) {
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
                    String Mi_SQL = "DELETE FROM " + Cat_Nom_Plazas.Tabla_Cat_Nom_Plazas;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Plazas.Campo_Plaza_ID + " = '" + Parametros.P_Plaza_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Trans.Commit();
                } catch (OracleException Ex) {
                    if (Ex.Code == 547) {
                        Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]";
                    } else {
                        Mensaje = "Error al intentar eliminar la Zona. Error: [" + Ex.Message + "]";
                    }
                    throw new Exception(Mensaje);
                } catch (Exception Ex) {
                    Mensaje = "Error al intentar eliminar la Zona. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Plaza
            ///DESCRIPCIÓN          : Consulta los detalles de una plaza
            ///PARAMETROS           : 
            ///                     1.  Parametros. Contiene los parametros que se van a utilizar para
            ///                                     hacer la Consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 04/Abril/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static Cls_Cat_Nom_Plazas_Negocio Consultar_Datos_Plaza(Cls_Cat_Nom_Plazas_Negocio Parametros) {
                Cls_Cat_Nom_Plazas_Negocio Plaza = new Cls_Cat_Nom_Plazas_Negocio();
                try {
                    String Mi_SQL = "SELECT * FROM " + Cat_Nom_Plazas.Tabla_Cat_Nom_Plazas + " WHERE " + Cat_Nom_Plazas.Campo_Plaza_ID;
                    Mi_SQL = Mi_SQL + " = '" +  Parametros.P_Plaza_ID + "'";
                    OracleDataReader Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    while (Data_Reader.Read()) {
                        Plaza.P_Plaza_ID = (Data_Reader[Cat_Nom_Plazas.Campo_Plaza_ID] != null) ? Data_Reader[Cat_Nom_Plazas.Campo_Plaza_ID].ToString() : "";
                        Plaza.P_Nombre = (Data_Reader[Cat_Nom_Plazas.Campo_Nombre] != null) ? Data_Reader[Cat_Nom_Plazas.Campo_Nombre].ToString() : "";
                        Plaza.P_Estatus = (Data_Reader[Cat_Nom_Plazas.Campo_Estatus] != null) ? Data_Reader[Cat_Nom_Plazas.Campo_Estatus].ToString() : "";
                        Plaza.P_Comentarios = (Data_Reader[Cat_Nom_Plazas.Campo_Comentarios] != null) ? Data_Reader[Cat_Nom_Plazas.Campo_Comentarios].ToString() : "";
                    }
                    Data_Reader.Close();
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los datos del Plaza. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Plaza;
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