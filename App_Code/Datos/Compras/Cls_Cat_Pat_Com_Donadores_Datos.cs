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
using Presidencia.Control_Patrimonial_Catalogo_Donadores.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
/// <summary>
/// Summary description for Cls_Cat_Pat_Com_Donadores_Datos
/// </summary>

namespace Presidencia.Control_Patrimonial_Catalogo_Donadores.Datos {

    public class Cls_Cat_Pat_Com_Donadores_Datos
    {

        #region Metodos

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Alta_Donador
            ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo registro Donador.
            ///PARAMETROS           : 
            ///                    1.  Donador.   Contiene los parametros que se van a dar de
            ///                                     Alta en la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 22/Enero/2010 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static Cls_Cat_Pat_Com_Donadores_Negocio Alta_Donador(Cls_Cat_Pat_Com_Donadores_Negocio Donador)
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
                String Donador_ID = Obtener_ID_Consecutivo(Cat_Pat_Donadores.Tabla_Cat_Pat_Donadores, Cat_Pat_Donadores.Campo_Donador_ID, 5);
                try {
                    String Mi_SQL = "INSERT INTO " + Cat_Pat_Donadores.Tabla_Cat_Pat_Donadores;
                    Mi_SQL = Mi_SQL + " (" + Cat_Pat_Donadores.Campo_Donador_ID + ", " + Cat_Pat_Donadores.Campo_Nombre + ", " + Cat_Pat_Donadores.Campo_Apellido_Paterno;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_Apellido_Materno + ", " + Cat_Pat_Donadores.Campo_Direccion + ", " + Cat_Pat_Donadores.Campo_Cuidad;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_Estado + ", " + Cat_Pat_Donadores.Campo_Telefono + ", " + Cat_Pat_Donadores.Campo_Celular;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_CURP + ", " + Cat_Pat_Donadores.Campo_RFC;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_Usuario_Creo + ", " + Cat_Pat_Donadores.Campo_Fecha_Creo;
                    Mi_SQL = Mi_SQL + ") VALUES ('" + Donador_ID + "', '" + Donador.P_Nombre + "','" + Donador.P_Apellido_Paterno + "'";
                    Mi_SQL = Mi_SQL + ", '" + Donador.P_Apellido_Materno + "', '" + Donador.P_Direccion + "','" + Donador.P_Cuidad + "'";
                    Mi_SQL = Mi_SQL + ", '" + Donador.P_Estado + "', '" + Donador.P_Telefono + "','" + Donador.P_Celular + "'";
                    Mi_SQL = Mi_SQL + ", '" + Donador.P_CURP + "', '" + Donador.P_RFC + "'";
                    Mi_SQL = Mi_SQL + ", '" + Donador.P_Usuario + "', SYSDATE)"; 
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Trans.Commit();
                    Donador.P_Donador_ID = Donador_ID;
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
                        Mensaje = "Error al intentar dar de Alta un Donador. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    }
                    //Indicamos el mensaje 
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
                return Donador;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Modificar_Donador
            ///DESCRIPCIÓN          : Modifica en la Base de Datos un Donador
            ///PARAMETROS           : 
            ///                     1.  Donador.Contiene los parametros que se van hacer la
            ///                                     Modificación en la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 22/Enero/2010 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Modificar_Donador(Cls_Cat_Pat_Com_Donadores_Negocio Donador)
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
                    String Mi_SQL = "UPDATE " + Cat_Pat_Donadores.Tabla_Cat_Pat_Donadores;
                    Mi_SQL = Mi_SQL + " SET " + Cat_Pat_Donadores.Campo_Nombre + " = '" + Donador.P_Nombre + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_Apellido_Paterno + " = '" + Donador.P_Apellido_Paterno + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_Apellido_Materno + " = '" + Donador.P_Apellido_Materno + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_Direccion + " = '" + Donador.P_Direccion + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_Cuidad + " = '" + Donador.P_Cuidad + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_Estado + " = '" + Donador.P_Estado + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_Telefono + " = '" + Donador.P_Telefono + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_Celular + " = '" + Donador.P_Celular + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_CURP + " = '" + Donador.P_CURP + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_RFC + " = '" + Donador.P_RFC + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_Usuario_Modifico + " = '" + Donador.P_Usuario + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_Fecha_Modifico + " = SYSDATE";
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Donadores.Campo_Donador_ID + " = '" + Donador.P_Donador_ID + "'";
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
                        Mensaje = "Error al intentar Modificar una Donador. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
            ///                     1.  Donador.Contiene los parametros que se van a utilizar para
            ///                                     hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 22/Enero/2010 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_DataTable(Cls_Cat_Pat_Com_Donadores_Negocio Donador) {
                String Mi_SQL = null;
                DataSet Ds_Donadores = null;
                DataTable Dt_Donadores = new DataTable();
                try {
                    if (Donador.P_Tipo_DataTable.Equals("DONADORES")) {
                        Mi_SQL = "SELECT " + Cat_Pat_Donadores.Campo_Donador_ID + " AS DONADOR_ID";

                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_Apellido_Paterno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Pat_Donadores.Campo_Apellido_Materno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Pat_Donadores.Campo_Nombre + " AS NOMBRE_COMPLETO";

                        Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Donadores.Tabla_Cat_Pat_Donadores;
                        Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pat_Donadores.Campo_Apellido_Paterno;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_Apellido_Materno;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_Nombre;
                    } 
                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                        Ds_Donadores = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Donadores != null) {
                        Dt_Donadores = Ds_Donadores.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Donadores;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Eliminar_Donador
            ///DESCRIPCIÓN          : Elimina un Registro de un Donador
            ///PARAMETROS           : 
            ///                     1.  Donador.Contiene los parametros que se van a utilizar para
            ///                                     hacer la eliminacion de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 22/Enero/2010 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Eliminar_Donador(Cls_Cat_Pat_Com_Donadores_Negocio Donador) {
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
                    String Mi_SQL = "DELETE FROM " + Cat_Pat_Donadores.Tabla_Cat_Pat_Donadores;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Donadores.Campo_Donador_ID + " = '" + Donador.P_Donador_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Trans.Commit();
                } catch (OracleException Ex) {
                    if (Ex.Code == 547) {
                        Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]";
                    } else {
                        Mensaje = "Error al intentar eliminar el Donador. Error: [" + Ex.Message + "]";
                    }
                    throw new Exception(Mensaje);
                } catch (Exception Ex) {
                    Mensaje = "Error al intentar eliminar el Donador. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
            }


            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Donador
            ///DESCRIPCIÓN          :  Consultar Datos de una Donador
            ///PARAMETROS           : 
            ///                     1.  Donador.Contiene los parametros que se van a utilizar 
            ///                                     para hacer la eliminacion de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 22/Enero/2010
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static Cls_Cat_Pat_Com_Donadores_Negocio Consultar_Datos_Donador(Cls_Cat_Pat_Com_Donadores_Negocio Donador)
            {
                String Mi_SQL = "SELECT " + Cat_Pat_Donadores.Campo_Donador_ID + " AS DONADOR_ID, " + Cat_Pat_Donadores.Campo_Nombre + " AS NOMBRE";
                Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_Apellido_Paterno + " AS APELLIDO_PATERNO, " + Cat_Pat_Donadores.Campo_Apellido_Materno + " AS APELLIDO_MATERNO";
                Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_Direccion + " AS DIRECCION, " + Cat_Pat_Donadores.Campo_Cuidad + " AS CUIDAD";
                Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_Estado + " AS ESTADO, " + Cat_Pat_Donadores.Campo_Telefono + " AS TELEFONO";
                Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_Celular + " AS CELULAR, " + Cat_Pat_Donadores.Campo_CURP + " AS CURP";
                Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_RFC + " AS RFC ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Donadores.Tabla_Cat_Pat_Donadores;
                if (Donador.P_Donador_ID != null && Donador.P_Donador_ID.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Donadores.Campo_Donador_ID + " = '" + Donador.P_Donador_ID + "'";
                } else if (Donador.P_RFC != null && Donador.P_RFC.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Donadores.Campo_RFC + " = '" + Donador.P_RFC + "'";
                }
                Cls_Cat_Pat_Com_Donadores_Negocio Resultado = new Cls_Cat_Pat_Com_Donadores_Negocio();
                OracleDataReader Data_Reader;
                try {
                    Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    while (Data_Reader.Read()) {
                        Resultado.P_Donador_ID = (Data_Reader["DONADOR_ID"] != null) ? Data_Reader["DONADOR_ID"].ToString() : "";
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
                    }
                    Data_Reader.Close();
                } catch (OracleException Ex) {
                    String Mensaje = "Error al intentar consultar el Donador. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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



            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Donadores
            ///DESCRIPCIÓN          : Obtiene donadores de la Base de Datos y los regresa en un DataTable.
            ///PARAMETROS           : 
            ///CREO                 : Salvador Hernández Ramírez
            ///FECHA_CREO           : 14/Febrero/2011 
            ///MODIFICO             : Salvador Hernández Ramírez
            ///FECHA_MODIFICO       : 29/Abril/2011 
            ///CAUSA_MODIFICACIÓN   : Se agregó la búsqueda por nombre del donador
            ///*******************************************************************************
            public static DataTable Consultar_Donadores(Cls_Cat_Pat_Com_Donadores_Negocio Donador)
            {
                String Mi_SQL = null;
                DataSet Ds_Donadores = null;
                DataTable Dt_Donadores = new DataTable();
                try
                {
                        Mi_SQL = "SELECT " + Cat_Pat_Donadores.Campo_Donador_ID + " AS DONADOR_ID";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_Direccion + "";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_Cuidad + " AS CIUDAD";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_Estado+ "";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_CURP+ "";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_RFC + "";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_Apellido_Paterno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Pat_Donadores.Campo_Apellido_Materno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Pat_Donadores.Campo_Nombre + " AS NOMBRE_COMPLETO";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Donadores.Tabla_Cat_Pat_Donadores;
                        

                        if (Donador.P_Nombre != null)
                        {
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Donadores.Campo_Nombre + " LIKE '%" +  Donador.P_Nombre + "%'";
                        }

                        Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pat_Donadores.Campo_Apellido_Paterno;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_Apellido_Materno;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Donadores.Campo_Nombre;
                   
                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
                    {
                        Ds_Donadores = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Donadores != null)
                    {
                        Dt_Donadores = Ds_Donadores.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Donadores;
            }



            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Donadores
            ///DESCRIPCIÓN          : Obtiene completamente los datos de un donador y los regresa en un DataTable.
            ///PARAMETROS           : 
            ///                     1.  Donador.Contiene los parametros que se van a utilizar para
            ///                                     hacer la consulta de la Base de Datos.
            ///CREO                 : Salvador Hernández Ramirez
            ///FECHA_CREO           : 14/Febrero/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Productos_Donador(Cls_Cat_Pat_Com_Donadores_Negocio Donador)
            {
                String Mi_SQL = null;
                DataSet Ds_Donadores = null;
                DataTable Dt_Donadores = new DataTable();
                try
                {
                    if (Donador.P_Tipo_DataTable.Equals("BIENES_MUEBLES"))
                    {
                        Mi_SQL = "SELECT " + "(BIENES_M."+ Ope_Pat_Bienes_Muebles.Campo_Nombre + "";
                        Mi_SQL = Mi_SQL + " ||' '|| MARCAS." + Cat_Com_Marcas.Campo_Nombre + ") AS NOMBRE";
                        Mi_SQL = Mi_SQL + ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Cantidad+ "";
                        Mi_SQL = Mi_SQL + ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + "";
                        Mi_SQL = Mi_SQL + ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + "";
                        Mi_SQL = Mi_SQL + ", DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + " as DEPENDENCIA";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + " BIENES_M";
                        Mi_SQL = Mi_SQL + " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
                        Mi_SQL = Mi_SQL + " ON BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " MARCAS";
                        Mi_SQL = Mi_SQL + " ON BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID;
                        Mi_SQL = Mi_SQL + " = MARCAS." + Cat_Com_Marcas.Campo_Marca_ID;
                        Mi_SQL = Mi_SQL + " WHERE BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Donador_ID + " = '" + Donador.P_Donador_ID + "'";
                    }
                    else if (Donador.P_Tipo_DataTable.Equals("VEHICULOS"))
                    {
                        Mi_SQL = "SELECT (VEHICULOS." + Ope_Pat_Vehiculos.Campo_Nombre + "";
                        Mi_SQL = Mi_SQL + " ||' '|| MARCAS." + Cat_Com_Marcas.Campo_Nombre + ") AS NOMBRE";
                        Mi_SQL = Mi_SQL + ", VEHICULOS." + Ope_Pat_Vehiculos.Campo_Cantidad + "";
                        Mi_SQL = Mi_SQL + ", VEHICULOS." + Ope_Pat_Vehiculos.Campo_Numero_Inventario + "";
                        Mi_SQL = Mi_SQL + ", VEHICULOS." + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion + "";
                        Mi_SQL = Mi_SQL + ", DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + " VEHICULOS";
                        Mi_SQL = Mi_SQL + " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
                        Mi_SQL = Mi_SQL + " ON VEHICULOS." + Ope_Pat_Vehiculos.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " MARCAS";
                        Mi_SQL = Mi_SQL + " ON VEHICULOS." + Ope_Pat_Vehiculos.Campo_Marca_ID;
                        Mi_SQL = Mi_SQL + " = MARCAS." + Cat_Com_Marcas.Campo_Marca_ID;
                        Mi_SQL = Mi_SQL + " WHERE VEHICULOS." + Ope_Pat_Vehiculos.Campo_Donador_ID + " = '" + Donador.P_Donador_ID + "'";

                    }
                    else if (Donador.P_Tipo_DataTable.Equals("ANIMALES"))
                    {
                        Mi_SQL = "SELECT CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Nombre + "";
                        Mi_SQL = Mi_SQL + ", CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Cantidad + "";
                        Mi_SQL = Mi_SQL + ", CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Numero_Inventario + "";
                        Mi_SQL = Mi_SQL + ", CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Fecha_Adquisicion + "";
                        Mi_SQL = Mi_SQL + ", DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + " as DEPENDENCIA";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + " CEMOVIENTES";
                        Mi_SQL = Mi_SQL + " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
                        Mi_SQL = Mi_SQL + " ON CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " WHERE CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Donador_ID + " = '" + Donador.P_Donador_ID + "'";
                    }

                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
                    {
                        Ds_Donadores = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Donadores != null)
                    {
                        Dt_Donadores = Ds_Donadores.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Donadores;
            }

        #endregion
    }

}