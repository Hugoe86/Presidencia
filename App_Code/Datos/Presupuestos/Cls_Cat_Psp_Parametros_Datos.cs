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
using Presidencia.Paramentros_Presupuestos.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Text;

namespace Presidencia.Paramentros_Presupuestos.Datos {
    public class Cls_Cat_Psp_Parametros_Datos {

        #region Metodos

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Alta_Parametro
            ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo registro de parametros
            ///PARAMETROS           : Datos.   Contiene los parametros que se van a dar de
            ///                                     Alta en la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 18/Octubre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Alta_Parametro(Cls_Cat_Psp_Parametros_Negocio Datos) {
                String Mensaje = "";
                OracleConnection Cn = new OracleConnection();
                OracleCommand Cmd = new OracleCommand();
                OracleTransaction Trans;
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                Datos.P_Parametro_ID = Obtener_ID_Consecutivo(Cat_Psp_Parametros.Tabla_Cat_Psp_Parametros, Cat_Psp_Parametros.Campo_Parametro_ID, 5);
                try {
                    String Mi_SQL = "INSERT INTO " + Cat_Psp_Parametros.Tabla_Cat_Psp_Parametros + " ("
                                    + Cat_Psp_Parametros.Campo_Parametro_ID +
                                    ", " + Cat_Psp_Parametros.Campo_Fecha_Apertura +
                                    ", " + Cat_Psp_Parametros.Campo_Fecha_Cierre +
                                    ", " + Cat_Psp_Parametros.Campo_Anio_Presupuestar +
                                    ", " + Cat_Psp_Parametros.Campo_Estatus +
                                    ", " + Cat_Psp_Parametros.Campo_Fte_Financiamiento_ID +
                                    ", " + Cat_Psp_Parametros.Campo_Usuario_Creo +
                                    ", " + Cat_Psp_Parametros.Campo_Fecha_Creo +
                                    ") VALUES ('" + Datos.P_Parametro_ID + "'" +
                                    ", '" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Apertura) + "'" +
                                    ", '" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Cierre) + "'" +
                                    ", '" + Datos.P_Anio_Presupuestar.ToString() + "'" +
                                    ", '" + Datos.P_Estatus + "'" +
                                    ", '" + Datos.P_Fuente_Financiamiento_ID + "'" +
                                    ", '" + Datos.P_Usuario + "'" +
                                    ", SYSDATE)";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    if (Datos.P_Dt_Partidas_Stock != null && Datos.P_Dt_Partidas_Stock.Rows.Count > 0) {
                        for (Int32 Contador = 0; Contador < Datos.P_Dt_Partidas_Stock.Rows.Count; Contador++) {
                            Mi_SQL = "INSERT INTO " + Cat_Psp_Parametros_Detalles.Tabla_Cat_Psp_Parametros_Detalles + " (" +
                                     Cat_Psp_Parametros_Detalles.Campo_Parametro_ID + 
                                     ", " + Cat_Psp_Parametros_Detalles.Campo_Partida_ID + 
                                     ") VALUES ('" + Datos.P_Parametro_ID + "'" + 
                                     ", '" + Datos.P_Dt_Partidas_Stock.Rows[Contador]["PARTIDA_ID"] + "')";
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
                        Mensaje = "Error al intentar dar de Alta. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    }
                    //Indicamos el mensaje 
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Modificar_Parametro
            ///DESCRIPCIÓN          : Modifica en la Base de Datos el registro indicado del Parametro
            ///PARAMETROS           : 1.  Datos.   Contiene los parametros que se van hacer la
            ///                                     Modificación en la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 18/Octubre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Modificar_Parametro(Cls_Cat_Psp_Parametros_Negocio Datos)
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
                    String Mi_SQL = "UPDATE " + Cat_Psp_Parametros.Tabla_Cat_Psp_Parametros;
                    Mi_SQL = Mi_SQL + " SET " + Cat_Psp_Parametros.Campo_Fecha_Apertura + " = '" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Apertura) + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Psp_Parametros.Campo_Fecha_Cierre + " = '" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Cierre) + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Psp_Parametros.Campo_Anio_Presupuestar + " = '" + Datos.P_Anio_Presupuestar.ToString() + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Psp_Parametros.Campo_Fte_Financiamiento_ID + " = '" + Datos.P_Fuente_Financiamiento_ID  + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Psp_Parametros.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Psp_Parametros.Campo_Fecha_Modifico + " = SYSDATE";
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Psp_Parametros.Campo_Parametro_ID + " = '" + Datos.P_Parametro_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();

                    Mi_SQL = "DELETE FROM " + Cat_Psp_Parametros_Detalles.Tabla_Cat_Psp_Parametros_Detalles +
                             " WHERE " + Cat_Psp_Parametros_Detalles.Campo_Parametro_ID +
                             " = '" + Datos.P_Parametro_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    
                    if (Datos.P_Dt_Partidas_Stock != null && Datos.P_Dt_Partidas_Stock.Rows.Count > 0) {
                        for (Int32 Contador = 0; Contador < Datos.P_Dt_Partidas_Stock.Rows.Count; Contador++) {
                            Mi_SQL = "INSERT INTO " + Cat_Psp_Parametros_Detalles.Tabla_Cat_Psp_Parametros_Detalles + " (" +
                                     Cat_Psp_Parametros_Detalles.Campo_Parametro_ID +
                                     ", " + Cat_Psp_Parametros_Detalles.Campo_Partida_ID +
                                     ") VALUES ('" + Datos.P_Parametro_ID + "'" +
                                     ", '" + Datos.P_Dt_Partidas_Stock.Rows[Contador]["PARTIDA_ID"] + "')";
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
                        Mensaje = "Error al intentar Modificar. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    }
                    //Indicamos el mensaje 
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Parametros
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
            ///PARAMETROS           : Datos.   Contiene los parametros que se van a utilizar para
            ///                                     hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 18/Octubre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Parametros(Cls_Cat_Psp_Parametros_Negocio Datos)  {
                String Mi_SQL = null;
                DataSet Ds_Datos = null;
                DataTable Dt_Datos = new DataTable();
                try {
                    Mi_SQL = "SELECT * FROM " + Cat_Psp_Parametros.Tabla_Cat_Psp_Parametros;
                    if (Datos.P_Anio_Presupuestar > (-1)) {
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Psp_Parametros.Campo_Anio_Presupuestar + " = " + Datos.P_Anio_Presupuestar.ToString();
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Psp_Parametros.Campo_Anio_Presupuestar + " DESC";
                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Datos != null) {
                        Dt_Datos = Ds_Datos.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Datos;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Detalles_Parametro
            ///DESCRIPCIÓN          :  Consultar Datos de un Parametros
            ///PARAMETROS           : Datos.  Contiene los parametros que se van a utilizar para
            ///                                 hacer la eliminacion de la Base de Datos.
            ///CREO                 :
            ///
            /// .03333333330000000
            ///FECHA_CREO           : 18/Octubre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static Cls_Cat_Psp_Parametros_Negocio Consultar_Detalles_Parametro(Cls_Cat_Psp_Parametros_Negocio Datos) {
                Boolean Entro_Where = false;
                String Mi_SQL = "SELECT * FROM " + Cat_Psp_Parametros.Tabla_Cat_Psp_Parametros;
                if (Datos.P_Anio_Presupuestar > (-1)) {
                    if (Entro_Where) {
                        Mi_SQL = Mi_SQL + " AND ";
                    } else {
                        Mi_SQL = Mi_SQL + " WHERE ";
                        Entro_Where = true;
                    }
                    Mi_SQL = Mi_SQL + Cat_Psp_Parametros.Campo_Anio_Presupuestar + " = '" + Datos.P_Anio_Presupuestar + "'";
                }
                if (Datos.P_Parametro_ID.Trim().Length > 0)  {
                    if (Entro_Where) {
                        Mi_SQL = Mi_SQL + " AND ";
                    } else {
                        Mi_SQL = Mi_SQL + " WHERE ";
                        Entro_Where = true;
                    }
                    Mi_SQL = Mi_SQL + Cat_Psp_Parametros.Campo_Parametro_ID + " = '" + Datos.P_Parametro_ID + "'";
                }
                Cls_Cat_Psp_Parametros_Negocio Parametros = new Cls_Cat_Psp_Parametros_Negocio();
                OracleDataReader Data_Reader;
                DataSet Ds_Datos = null;
                try {
                    Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    while (Data_Reader.Read()) {
                        Parametros.P_Parametro_ID = (!String.IsNullOrEmpty(Data_Reader[Cat_Psp_Parametros.Campo_Parametro_ID].ToString())) ? Data_Reader[Cat_Psp_Parametros.Campo_Parametro_ID].ToString() : "";
                        Parametros.P_Fecha_Apertura = (!String.IsNullOrEmpty(Data_Reader[Cat_Psp_Parametros.Campo_Fecha_Apertura].ToString())) ? Convert.ToDateTime(Data_Reader[Cat_Psp_Parametros.Campo_Fecha_Apertura]) : new DateTime();
                        Parametros.P_Fecha_Cierre = (!String.IsNullOrEmpty(Data_Reader[Cat_Psp_Parametros.Campo_Fecha_Cierre].ToString())) ? Convert.ToDateTime(Data_Reader[Cat_Psp_Parametros.Campo_Fecha_Cierre]) : new DateTime();
                        Parametros.P_Anio_Presupuestar = (!String.IsNullOrEmpty(Data_Reader[Cat_Psp_Parametros.Campo_Anio_Presupuestar].ToString())) ? Convert.ToInt32(Data_Reader[Cat_Psp_Parametros.Campo_Anio_Presupuestar]) : (-1);
                        Parametros.P_Estatus = (!String.IsNullOrEmpty(Data_Reader[Cat_Psp_Parametros.Campo_Estatus].ToString())) ? Data_Reader[Cat_Psp_Parametros.Campo_Estatus].ToString() : "";
                        Parametros.P_Fuente_Financiamiento_ID = (!String.IsNullOrEmpty(Data_Reader[Cat_Psp_Parametros.Campo_Fte_Financiamiento_ID].ToString())) ? Data_Reader[Cat_Psp_Parametros.Campo_Fte_Financiamiento_ID].ToString() : "";
                    }
                    Data_Reader.Close();
                    Mi_SQL = "SELECT " + Cat_Psp_Parametros_Detalles.Tabla_Cat_Psp_Parametros_Detalles + "." + Cat_Psp_Parametros_Detalles.Campo_Partida_ID + " AS PARTIDA_ID" +
                             ", " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Clave + " AS CLAVE" +
                             ", " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Nombre + " AS NOMBRE" +
                             " FROM " + Cat_Psp_Parametros_Detalles.Tabla_Cat_Psp_Parametros_Detalles +
                             " LEFT OUTER JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas +
                             " ON " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID +
                             " = " + Cat_Psp_Parametros_Detalles.Tabla_Cat_Psp_Parametros_Detalles + "." + Cat_Psp_Parametros_Detalles.Campo_Partida_ID +
                            " WHERE " + Cat_Psp_Parametros_Detalles.Tabla_Cat_Psp_Parametros_Detalles + "." + Cat_Psp_Parametros_Detalles.Campo_Parametro_ID +
                             " = '" + Datos.P_Parametro_ID + "'";
                                
                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Datos != null && Ds_Datos.Tables.Count > 0) {
                        Parametros.P_Dt_Partidas_Stock = Ds_Datos.Tables[0];
                    }
                } catch (OracleException Ex) {
                    String Mensaje = "Error al intentar consultar. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Parametros;             
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Eliminar_Parametro
            ///DESCRIPCIÓN          : Elimina un Registro de un Parametro.
            ///PARAMETROS           : Datos.   Contiene los parametros que se van a utilizar para
            ///                                     hacer la eliminacion de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 18/Octubre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Eliminar_Parametro(Cls_Cat_Psp_Parametros_Negocio Datos) {
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
                    
                    String Mi_SQL = "DELETE FROM " + Cat_Psp_Parametros_Detalles.Tabla_Cat_Psp_Parametros_Detalles +
                             " WHERE " + Cat_Psp_Parametros_Detalles.Campo_Parametro_ID +
                             " = '" + Datos.P_Parametro_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();

                    Mi_SQL = "DELETE FROM " + Cat_Psp_Parametros.Tabla_Cat_Psp_Parametros;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Psp_Parametros.Campo_Parametro_ID + " = '" + Datos.P_Parametro_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();

                    Trans.Commit();
                } catch (OracleException Ex) {
                    if (Ex.Code == 547) {
                        Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]";
                    } else {
                        Mensaje = "Error al intentar eliminar. Error: [" + Ex.Message + "]";
                    }
                    throw new Exception(Mensaje);
                } catch (Exception Ex) {
                    Mensaje = "Error al intentar eliminar. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Capitulos
            ///DESCRIPCIÓN          : Obtiene datos de los capitulos de la Base de Datos y los regresa en un DataTable.
            ///PARAMETROS           : 
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 07/Noviembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            internal static DataTable Consultar_Capitulos()
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Para fomar el query que contendra la consulta
                DataSet Ds_Capitulos = new DataSet(); //Dataset donde obtendremos los datos de la consulta
                DataTable Dt_Capitulos = new DataTable();
                try
                {
                    Mi_SQL.Append("SELECT " + Cat_SAP_Capitulos.Campo_Capitulo_ID + ", ");
                    Mi_SQL.Append(Cat_SAP_Capitulos.Campo_Clave + "||' " + " " + "'|| "+ Cat_SAP_Capitulos.Campo_Descripcion + " AS NOMBRE ");
                    Mi_SQL.Append(" FROM " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos);
                    Mi_SQL.Append(" WHERE " + Cat_SAP_Capitulos.Campo_Estatus + " = 'ACTIVO'");
                    Mi_SQL.Append(" AND " + Cat_SAP_Capitulos.Campo_Clave + " IN('2000', '3000','5000')");
                    Mi_SQL.Append(" ORDER BY " + Cat_SAP_Capitulos.Campo_Clave + " ASC");

                    Ds_Capitulos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                    if (Ds_Capitulos != null)
                    {
                        Dt_Capitulos = Ds_Capitulos.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros de los capitulos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Capitulos;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Partida_Especifica
            ///DESCRIPCIÓN          : Obtiene datos de las partidas especificas de la Base de Datos y los regresa en un DataTable.
            ///PARAMETROS           1 Parametros_Negocio conexion con la capa de negocios 
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 07/Noviembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            internal static DataTable Consultar_Partida_Especifica(Cls_Cat_Psp_Parametros_Negocio Parametros_Negocio)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Para fomar el query que contendra la consulta
                DataSet Ds_Capitulos = new DataSet(); //Dataset donde obtendremos los datos de la consulta
                DataTable Dt_Capitulos = new DataTable();
                try
                {
                    Mi_SQL.Append("SELECT " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas+ "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + ", ");
                    Mi_SQL.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Clave + "||' " + " " + "'|| " );
                    Mi_SQL.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Nombre + " AS NOMBRE ");
                    Mi_SQL.Append(" FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + ", ");
                    Mi_SQL.Append(Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + ", ");
                    Mi_SQL.Append(Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + ", ");
                    Mi_SQL.Append(Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos);
                    Mi_SQL.Append(" WHERE " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID);
                    Mi_SQL.Append( " = " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." +Cat_Sap_Partidas_Genericas.Campo_Partida_Generica_ID);
                    Mi_SQL.Append(" AND " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Concepto_ID);
                    Mi_SQL.Append(" = " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." + Cat_Sap_Partidas_Genericas.Campo_Concepto_ID);
                    Mi_SQL.Append(" AND " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Capitulo_ID);
                    Mi_SQL.Append(" = " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Capitulo_ID);
                    Mi_SQL.Append(" AND " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Estatus + " = 'ACTIVO'");
                    Mi_SQL.Append(" AND " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Capitulo_ID + " = '" + Parametros_Negocio.P_Capitulo_Id + "'");
                    Mi_SQL.Append(" ORDER BY " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Clave + " ASC");

                    Ds_Capitulos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                    if (Ds_Capitulos != null)
                    {
                        Dt_Capitulos = Ds_Capitulos.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros de las partidas especificas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Capitulos;
            }
            
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Programas
            ///DESCRIPCIÓN          : Obtiene datos de los programas de la Base de Datos y los regresa en un DataTable.
            ///PARAMETROS           : 
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 11/Noviembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            internal static DataTable Consultar_Programas()
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Para fomar el query que contendra la consulta
                DataSet Ds_Programas = new DataSet(); //Dataset donde obtendremos los datos de la consulta
                DataTable Dt_Programas = new DataTable();
                try
                {
                    Mi_SQL.Append("SELECT " + Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id + ", ");
                    Mi_SQL.Append(Cat_Sap_Proyectos_Programas.Campo_Clave + "||' " + " " + "'|| " + Cat_Sap_Proyectos_Programas.Campo_Nombre + " AS NOMBRE ");
                    Mi_SQL.Append(" FROM " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas);
                    Mi_SQL.Append(" WHERE " + Cat_Sap_Proyectos_Programas.Campo_Estatus + " = 'ACTIVO'");
                    Mi_SQL.Append(" ORDER BY " + Cat_Sap_Proyectos_Programas.Campo_Clave + " ASC");

                    Ds_Programas = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                    if (Ds_Programas != null)
                    {
                        Dt_Programas = Ds_Programas.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros de los programas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Programas;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Fuente_Financiamiento
            ///DESCRIPCIÓN          : Obtiene datos de las fuentes de financiamiento de la Base de Datos y los regresa en un DataTable.
            ///PARAMETROS           : 
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 11/Noviembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            internal static DataTable Consultar_Fuente_Financiamiento()
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Para fomar el query que contendra la consulta
                DataSet Ds_Fuente = new DataSet(); //Dataset donde obtendremos los datos de la consulta
                DataTable Dt_Fuente = new DataTable();
                try
                {
                    Mi_SQL.Append("SELECT " + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + ", ");
                    Mi_SQL.Append(Cat_SAP_Fuente_Financiamiento.Campo_Clave + "||' " + " " + "'|| " + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + " AS NOMBRE ");
                    Mi_SQL.Append(" FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento);
                    Mi_SQL.Append(" WHERE " + Cat_SAP_Fuente_Financiamiento.Campo_Estatus + " = 'ACTIVO'");
                    Mi_SQL.Append(" ORDER BY " + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " ASC");

                    Ds_Fuente = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                    if (Ds_Fuente != null)
                    {
                        Dt_Fuente = Ds_Fuente.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros de las fuentes de financiamiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Fuente;
            }
        #endregion

    }
}