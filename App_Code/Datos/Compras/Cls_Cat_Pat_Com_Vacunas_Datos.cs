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
using Presidencia.Control_Patrimonial_Catalogo_Vacunas.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
/// <summary>
/// Summary description for Cls_Cat_Pat_Com_Vacunas_Datos
/// </summary>

namespace Presidencia.Control_Patrimonial_Catalogo_Vacunas.Datos {

    public class Cls_Cat_Pat_Com_Vacunas_Datos {

        #region Metodos

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Alta_Vacuna
            ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo registro Vacuna.
            ///PARAMETROS           : 
            ///                    1.  Vacuna.   Contiene los parametros que se van a dar de
            ///                                 Alta en la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 10/Diciembre/2010 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Alta_Vacuna(Cls_Cat_Pat_Com_Vacunas_Negocio Vacuna) {
                String Mensaje = "";
                OracleConnection Cn = new OracleConnection();
                OracleCommand Cmd = new OracleCommand();
                OracleTransaction Trans;
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                String Vacuna_ID = Obtener_ID_Consecutivo(Cat_Pat_Vacunas.Tabla_Cat_Pat_Vacunas, Cat_Pat_Vacunas.Campo_Vacuna_ID, 5);
                try {
                    String Mi_SQL = "INSERT INTO " + Cat_Pat_Vacunas.Tabla_Cat_Pat_Vacunas;
                    Mi_SQL = Mi_SQL + " (" + Cat_Pat_Vacunas.Campo_Vacuna_ID + ", " + Cat_Pat_Vacunas.Campo_Nombre + ", " + Cat_Pat_Vacunas.Campo_Comentarios;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Vacunas.Campo_Estatus + ", " + Cat_Pat_Vacunas.Campo_Usuario_Creo + ", " + Cat_Pat_Vacunas.Campo_Fecha_Creo;
                    Mi_SQL = Mi_SQL + ") VALUES ('" + Vacuna_ID + "', '" + Vacuna.P_Nombre + "','" + Vacuna.P_Comentarios + "'";
                    Mi_SQL = Mi_SQL + ",'" + Vacuna.P_Estatus + "', '" + Vacuna.P_Usuario + "', SYSDATE)"; 
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
                        Mensaje = "Error al intentar dar de Alta una Vacuna. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    }
                    //Indicamos el mensaje 
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Modificar_Vacuna
            ///DESCRIPCIÓN          : Modifica en la Base de Datos la Vacuna
            ///PARAMETROS           : 
            ///                     1.  Vacuna. Contiene los parametros que se van hacer la
            ///                                 Modificación en la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 10/Diciembre/2010 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Modificar_Vacuna(Cls_Cat_Pat_Com_Vacunas_Negocio Vacuna) {
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
                    String Mi_SQL = "UPDATE " + Cat_Pat_Vacunas.Tabla_Cat_Pat_Vacunas;
                    Mi_SQL = Mi_SQL + " SET " + Cat_Pat_Vacunas.Campo_Nombre + " = '" + Vacuna.P_Nombre + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Vacunas.Campo_Comentarios + " = '" + Vacuna.P_Comentarios + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Vacunas.Campo_Estatus + " = '" + Vacuna.P_Estatus + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Vacunas.Campo_Usuario_Modifico + " = '" + Vacuna.P_Usuario + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Vacunas.Campo_Fecha_Modifico + " = SYSDATE";
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Vacunas.Campo_Vacuna_ID + " = '" + Vacuna.P_Vacuna_ID + "'";
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
                        Mensaje = "Error al intentar Modificar una Vacuna. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
            ///                     1.  Vacuna. Contiene los parametros que se van a utilizar para
            ///                                 hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 10/Diciembre/2010 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_DataTable(Cls_Cat_Pat_Com_Vacunas_Negocio Vacuna) {
                String Mi_SQL = null;
                DataSet Ds_Vacunas = null;
                DataTable Dt_Vacunas = new DataTable();
                try {
                    if (Vacuna.P_Tipo_DataTable.Equals("VACUNAS")) {
                        Mi_SQL = "SELECT " + Cat_Pat_Vacunas.Campo_Vacuna_ID + " AS VACUNA_ID";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Vacunas.Campo_Nombre + " AS NOMBRE";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Vacunas.Campo_Estatus + " AS ESTATUS";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Vacunas.Tabla_Cat_Pat_Vacunas;
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Vacunas.Campo_Nombre + " LIKE '%" + Vacuna.P_Nombre + "%'";
                        Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pat_Vacunas.Campo_Vacuna_ID;
                    } 
                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                        Ds_Vacunas = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Vacunas != null) {
                        Dt_Vacunas = Ds_Vacunas.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Vacunas;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Eliminar_Vacuna
            ///DESCRIPCIÓN          : Elimina un Registro de un Vacuna
            ///PARAMETROS           : 
            ///                     1.  Vacuna. Contiene los parametros que se van a utilizar para
            ///                                 hacer la eliminacion de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 10/Diciembre/2010 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Eliminar_Vacuna(Cls_Cat_Pat_Com_Vacunas_Negocio Vacuna) {
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
                    String Mi_SQL = "DELETE FROM " + Cat_Pat_Vacunas.Tabla_Cat_Pat_Vacunas;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Vacunas.Campo_Vacuna_ID + " = '" + Vacuna.P_Vacuna_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Trans.Commit();
                } catch (OracleException Ex) {
                    if (Ex.Code == 547) {
                        Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]";
                    } else {
                        Mensaje = "Error al intentar eliminar la Vacuna. Error: [" + Ex.Message + "]";
                    }
                    throw new Exception(Mensaje);
                } catch (Exception Ex) {
                    Mensaje = "Error al intentar eliminar la Vacuna. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
            }


            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Vacuna
            ///DESCRIPCIÓN          :  Consultar Datos de una Vacuna
            ///PARAMETROS           : 
            ///                     1.  Vacuna.  Contiene los parametros que se van a utilizar para
            ///                                 hacer la eliminacion de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 10/Diciembre/2010 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static Cls_Cat_Pat_Com_Vacunas_Negocio Consultar_Datos_Vacuna(Cls_Cat_Pat_Com_Vacunas_Negocio Vacuna) {
                String Mi_SQL = "SELECT " + Cat_Pat_Vacunas.Campo_Nombre + ", " + Cat_Pat_Vacunas.Campo_Comentarios;
                Mi_SQL = Mi_SQL + ", " + Cat_Pat_Vacunas.Campo_Estatus + " FROM " + Cat_Pat_Vacunas.Tabla_Cat_Pat_Vacunas;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Vacunas.Campo_Vacuna_ID + " = '" + Vacuna.P_Vacuna_ID + "'";
                Cls_Cat_Pat_Com_Vacunas_Negocio Resultado = new Cls_Cat_Pat_Com_Vacunas_Negocio();
                OracleDataReader Data_Reader;
                try {
                    Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    Resultado.P_Vacuna_ID = Vacuna.P_Vacuna_ID;
                    while (Data_Reader.Read()) {
                        Resultado.P_Nombre = (Data_Reader[Cat_Pat_Vacunas.Campo_Nombre] != null) ? Data_Reader[Cat_Pat_Vacunas.Campo_Nombre].ToString() : "";
                        Resultado.P_Comentarios = (Data_Reader[Cat_Pat_Vacunas.Campo_Comentarios] != null) ? Data_Reader[Cat_Pat_Vacunas.Campo_Comentarios].ToString() : "";
                        Resultado.P_Estatus = (Data_Reader[Cat_Pat_Vacunas.Campo_Estatus] != null) ? Data_Reader[Cat_Pat_Vacunas.Campo_Estatus].ToString() : "";
                    }
                    Data_Reader.Close();
                } catch (OracleException Ex) {
                    String Mensaje = "Error al intentar consultar la Vacuna. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Resultado;             
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