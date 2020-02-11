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
using Presidencia.Control_Patrimonial_Catalogo_Veterinarios.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
/// <summary>
/// Summary description for Cls_Cat_Pat_Com_Veterinarios_Datos
/// </summary>

namespace Presidencia.Control_Patrimonial_Catalogo_Veterinarios.Datos {

    public class Cls_Cat_Pat_Com_Veterinarios_Datos {

        #region Metodos

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Alta_Veterinario
            ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo registro Veterinario.
            ///PARAMETROS           : 
            ///                    1.  Veterinario.   Contiene los parametros que se van a dar de
            ///                                 Alta en la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 14/Diciembre/2010 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Alta_Veterinario(Cls_Cat_Pat_Com_Veterinarios_Negocio Veterinario) {
                String Mensaje = "";
                OracleConnection Cn = new OracleConnection();
                OracleCommand Cmd = new OracleCommand();
                OracleTransaction Trans;
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                String Veterinario_ID = Obtener_ID_Consecutivo(Cat_Pat_Veterinarios.Tabla_Cat_Pat_Veterinarios, Cat_Pat_Veterinarios.Campo_Veterinario_ID, 5);
                try {
                    String Mi_SQL = "INSERT INTO " + Cat_Pat_Veterinarios.Tabla_Cat_Pat_Veterinarios;
                    Mi_SQL = Mi_SQL + " (" + Cat_Pat_Veterinarios.Campo_Veterinario_ID + ", " + Cat_Pat_Veterinarios.Campo_Nombre + ", " + Cat_Pat_Veterinarios.Campo_Apellido_Paterno;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_Apellido_Materno + ", " + Cat_Pat_Veterinarios.Campo_Direccion + ", " + Cat_Pat_Veterinarios.Campo_Cuidad;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_Estado + ", " + Cat_Pat_Veterinarios.Campo_Telefono + ", " + Cat_Pat_Veterinarios.Campo_Celular;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_CURP + ", " + Cat_Pat_Veterinarios.Campo_RFC + ", " + Cat_Pat_Veterinarios.Campo_Cedula_Profesional;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_Estatus + ", " + Cat_Pat_Veterinarios.Campo_Usuario_Creo + ", " + Cat_Pat_Veterinarios.Campo_Fecha_Creo;
                    Mi_SQL = Mi_SQL + ") VALUES ('" + Veterinario_ID + "', '" + Veterinario.P_Nombre + "','" + Veterinario.P_Apellido_Paterno + "'";
                    Mi_SQL = Mi_SQL + ", '" + Veterinario.P_Apellido_Materno + "', '" + Veterinario.P_Direccion + "','" + Veterinario.P_Cuidad + "'";
                    Mi_SQL = Mi_SQL + ", '" + Veterinario.P_Estado + "', '" + Veterinario.P_Telefono + "','" + Veterinario.P_Celular + "'";
                    Mi_SQL = Mi_SQL + ", '" + Veterinario.P_CURP + "', '" + Veterinario.P_RFC + "','" + Veterinario.P_Cedula_Profesional + "'";
                    Mi_SQL = Mi_SQL + ",'" + Veterinario.P_Estatus + "', '" + Veterinario.P_Usuario + "', SYSDATE)"; 
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
                        Mensaje = "Error al intentar dar de Alta un Veterinario. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    }
                    //Indicamos el mensaje 
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Modificar_Veterinario
            ///DESCRIPCIÓN          : Modifica en la Base de Datos un Veterinario
            ///PARAMETROS           : 
            ///                     1.  Veterinario.Contiene los parametros que se van hacer la
            ///                                     Modificación en la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 14/Diciembre/2010 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Modificar_Veterinario(Cls_Cat_Pat_Com_Veterinarios_Negocio Veterinario)
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
                    String Mi_SQL = "UPDATE " + Cat_Pat_Veterinarios.Tabla_Cat_Pat_Veterinarios;
                    Mi_SQL = Mi_SQL + " SET " + Cat_Pat_Veterinarios.Campo_Nombre + " = '" + Veterinario.P_Nombre + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_Apellido_Paterno + " = '" + Veterinario.P_Apellido_Paterno + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_Apellido_Materno + " = '" + Veterinario.P_Apellido_Materno + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_Direccion + " = '" + Veterinario.P_Direccion + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_Cuidad + " = '" + Veterinario.P_Cuidad + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_Estado + " = '" + Veterinario.P_Estado + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_Telefono + " = '" + Veterinario.P_Telefono + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_Celular + " = '" + Veterinario.P_Celular + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_CURP + " = '" + Veterinario.P_CURP + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_RFC + " = '" + Veterinario.P_RFC + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_Cedula_Profesional + " = '" + Veterinario.P_Cedula_Profesional + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_Estatus + " = '" + Veterinario.P_Estatus + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_Usuario_Modifico + " = '" + Veterinario.P_Usuario + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_Fecha_Modifico + " = SYSDATE";
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Veterinarios.Campo_Veterinario_ID + " = '" + Veterinario.P_Veterinario_ID + "'";
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
                        Mensaje = "Error al intentar Modificar una Veterinario. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
            ///                     1.  Veterinario.Contiene los parametros que se van a utilizar para
            ///                                     hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 14/Diciembre/2010 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_DataTable(Cls_Cat_Pat_Com_Veterinarios_Negocio Veterinario) {
                String Mi_SQL = null;
                DataSet Ds_Veterinarios = null;
                DataTable Dt_Veterinarios = new DataTable();
                try {
                    if (Veterinario.P_Tipo_DataTable.Equals("VETERINARIOS")) {
                        Mi_SQL = "SELECT " + Cat_Pat_Veterinarios.Campo_Veterinario_ID + " AS VETERINARIO_ID";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_Apellido_Paterno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Pat_Veterinarios.Campo_Apellido_Materno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Pat_Veterinarios.Campo_Nombre + " AS NOMBRE_COMPLETO";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_Estatus + " AS ESTATUS";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Veterinarios.Tabla_Cat_Pat_Veterinarios;
                        if (Veterinario.P_Nombre != null && Veterinario.P_Nombre.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " WHERE (" + Cat_Pat_Veterinarios.Campo_Apellido_Paterno + "";
                            Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Pat_Veterinarios.Campo_Apellido_Materno + "";
                            Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Pat_Veterinarios.Campo_Nombre + ") LIKE '%" + Veterinario.P_Nombre.Trim() + "%'";
                            Mi_SQL = Mi_SQL + " OR (" + Cat_Pat_Veterinarios.Campo_Nombre + "";
                            Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Pat_Veterinarios.Campo_Apellido_Paterno + "";
                            Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Pat_Veterinarios.Campo_Apellido_Materno + ") LIKE '%" + Veterinario.P_Nombre.Trim() + "%'";
                        }
                        Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pat_Veterinarios.Campo_Apellido_Paterno;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_Apellido_Materno;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_Nombre;
                    } 
                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                        Ds_Veterinarios = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Veterinarios != null) {
                        Dt_Veterinarios = Ds_Veterinarios.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Veterinarios;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Eliminar_Veterinario
            ///DESCRIPCIÓN          : Elimina un Registro de un Veterinario
            ///PARAMETROS           : 
            ///                     1.  Veterinario.Contiene los parametros que se van a utilizar para
            ///                                     hacer la eliminacion de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 14/Diciembre/2010 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Eliminar_Veterinario(Cls_Cat_Pat_Com_Veterinarios_Negocio Veterinario) {
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
                    String Mi_SQL = "DELETE FROM " + Cat_Pat_Veterinarios.Tabla_Cat_Pat_Veterinarios;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Veterinarios.Campo_Veterinario_ID + " = '" + Veterinario.P_Veterinario_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Trans.Commit();
                } catch (OracleException Ex) {
                    if (Ex.Code == 547) {
                        Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]";
                    } else {
                        Mensaje = "Error al intentar eliminar el Veterinario. Error: [" + Ex.Message + "]";
                    }
                    throw new Exception(Mensaje);
                } catch (Exception Ex) {
                    Mensaje = "Error al intentar eliminar el Veterinario. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
            }


            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Veterinario
            ///DESCRIPCIÓN          :  Consultar Datos de una Veterinario
            ///PARAMETROS           : 
            ///                     1.  Veterinario.Contiene los parametros que se van a utilizar 
            ///                                     para hacer la eliminacion de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 15/Diciembre/2010
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static Cls_Cat_Pat_Com_Veterinarios_Negocio Consultar_Datos_Veterinario(Cls_Cat_Pat_Com_Veterinarios_Negocio Veterinario) {
                String Mi_SQL = "SELECT " + Cat_Pat_Veterinarios.Campo_Veterinario_ID + " AS VETERINARIO_ID, " + Cat_Pat_Veterinarios.Campo_Nombre + " AS NOMBRE";
                Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_Apellido_Paterno + " AS APELLIDO_PATERNO, " + Cat_Pat_Veterinarios.Campo_Apellido_Materno + " AS APELLIDO_MATERNO";
                Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_Direccion + " AS DIRECCION, " + Cat_Pat_Veterinarios.Campo_Cuidad + " AS CUIDAD";
                Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_Estado + " AS ESTADO, " + Cat_Pat_Veterinarios.Campo_Telefono + " AS TELEFONO";
                Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_Celular + " AS CELULAR, " + Cat_Pat_Veterinarios.Campo_CURP + " AS CURP";
                Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_RFC + " AS RFC, " + Cat_Pat_Veterinarios.Campo_Cedula_Profesional + " AS CEDULA_PROFESIONAL";
                Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Campo_Estatus + " AS ESTATUS FROM " + Cat_Pat_Veterinarios.Tabla_Cat_Pat_Veterinarios;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Veterinarios.Campo_Veterinario_ID + " = '" + Veterinario.P_Veterinario_ID + "'";
                Cls_Cat_Pat_Com_Veterinarios_Negocio Resultado = new Cls_Cat_Pat_Com_Veterinarios_Negocio();
                OracleDataReader Data_Reader;
                try {
                    Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    while (Data_Reader.Read()) {
                        Resultado.P_Veterinario_ID = (Data_Reader["VETERINARIO_ID"] != null) ? Data_Reader["VETERINARIO_ID"].ToString() : "";
                        Resultado.P_Nombre = (Data_Reader["NOMBRE"] != null) ? Data_Reader["NOMBRE"].ToString() : "";
                        Resultado.P_Apellido_Paterno = (Data_Reader["APELLIDO_PATERNO"] != null) ? Data_Reader["APELLIDO_PATERNO"].ToString() : "";
                        Resultado.P_Apellido_Materno = (Data_Reader["APELLIDO_MATERNO"] != null) ? Data_Reader["APELLIDO_MATERNO"].ToString() : "";
                        Resultado.P_Direccion = (Data_Reader["DIRECCION"] != null) ? Data_Reader["DIRECCION"].ToString() : "";
                        Resultado.P_Cuidad = (Data_Reader["CUIDAD"] != null) ? Data_Reader["CUIDAD"].ToString() : "";
                        Resultado.P_Estado = (Data_Reader["ESTADO"] != null) ? Data_Reader["ESTADO"].ToString() : "";
                        Resultado.P_Telefono = (Data_Reader["TELEFONO"] != null) ? Data_Reader["TELEFONO"].ToString() : "";
                        Resultado.P_Celular = (Data_Reader["CELULAR"] != null) ? Data_Reader["CELULAR"].ToString() : "";
                        Resultado.P_CURP = (Data_Reader["CURP"] != null) ? Data_Reader["CURP"].ToString() : "";
                        Resultado.P_RFC = (Data_Reader["RFC"] != null) ? Data_Reader["RFC"].ToString() : "";
                        Resultado.P_Cedula_Profesional = (Data_Reader["CEDULA_PROFESIONAL"] != null) ? Data_Reader["CEDULA_PROFESIONAL"].ToString() : "";
                        Resultado.P_Estatus = (Data_Reader["ESTATUS"] != null) ? Data_Reader["ESTATUS"].ToString() : "";
                    }
                    Data_Reader.Close();
                } catch (OracleException Ex) {
                    String Mensaje = "Error al intentar consultar el Veterinario. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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