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
using Presidencia.Catalogo_Rangos_Identificadores_Colonias.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Rangos_Identificadores_Colonias_Datos
/// </summary>
/// 

namespace Presidencia.Catalogo_Rangos_Identificadores_Colonias.Datos{

    public class Cls_Cat_Pre_Rangos_Identificadores_Colonias_Datos {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Rango_Identificador_Colonia
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo registro de Rango_Identificador_Colonia
        ///PARAMETROS          : 1. Rango_Identificador_Colonia.   Instancia de la Clase de Negocio de Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio
        ///                                 con los datos del Rango_Identificador_Colonia que va a ser dado de Alta.
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 28/Octubre/2010 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Alta_Rango_Identificador_Colonia(Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio Rango_Identificador_Colonia) {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Boolean Alta = false;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String Rango_Identificador_Colonia_ID = Obtener_ID_Consecutivo(Cat_Pre_Rangos_Identificadores_Colonias.Tabla_Cat_Pre_Rangos_Identificadores_Colonias, Cat_Pre_Rangos_Identificadores_Colonias.Campo_Rango_Identificador_Colonia_ID, 5);
            try {
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Rangos_Identificadores_Colonias.Tabla_Cat_Pre_Rangos_Identificadores_Colonias + " (";
                Mi_SQL = Mi_SQL + Cat_Pre_Rangos_Identificadores_Colonias.Campo_Rango_Identificador_Colonia_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Rangos_Identificadores_Colonias.Campo_Tipo_Colonia_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Rangos_Identificadores_Colonias.Campo_Rango_Inicial + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Rangos_Identificadores_Colonias.Campo_Rango_Final + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Rangos_Identificadores_Colonias.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Rangos_Identificadores_Colonias.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Rangos_Identificadores_Colonias.Campo_Fecha_Creo + ") ";
                Mi_SQL = Mi_SQL + "VALUES ('";
                Mi_SQL = Mi_SQL + Rango_Identificador_Colonia_ID + "', '";
                Mi_SQL = Mi_SQL + Rango_Identificador_Colonia.P_Tipo_Colonia_ID + "', ";
                Mi_SQL = Mi_SQL + Rango_Identificador_Colonia.P_Rango_Inicial + ", ";
                Mi_SQL = Mi_SQL + Rango_Identificador_Colonia.P_Rango_Final + ",'";
                Mi_SQL = Mi_SQL + Rango_Identificador_Colonia.P_Estatus + "', '";
                Mi_SQL = Mi_SQL + Rango_Identificador_Colonia.P_Usuario + "', SYSDATE)";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
                Alta = true;
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Rango Identificador de Colonia. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Rango_Identificador_Colonia
        ///DESCRIPCIÓN          : Modifica en la Base de Datos el registro indicado del Rango_Identificador_Colonia
        ///PARAMETROS          : 1. Rango_Identificador_Colonia.   Instancia de la Clase de Negocio de Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio
        ///                                 con los datos del Rango_Identificador_Colonia que va a ser Modificado.
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 28/Octubre/2010 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Modificar_Rango_Identificador_Colonia(Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio Rango_Identificador_Colonia)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Boolean Actualizar = false;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try
            {
                String Mi_SQL = "UPDATE " + Cat_Pre_Rangos_Identificadores_Colonias.Tabla_Cat_Pre_Rangos_Identificadores_Colonias + " SET ";
                Mi_SQL = Mi_SQL + Cat_Pre_Rangos_Identificadores_Colonias.Campo_Tipo_Colonia_ID + " = '" + Rango_Identificador_Colonia.P_Tipo_Colonia_ID + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Rangos_Identificadores_Colonias.Campo_Rango_Inicial + " = " + Rango_Identificador_Colonia.P_Rango_Inicial + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Rangos_Identificadores_Colonias.Campo_Rango_Final + " = " + Rango_Identificador_Colonia.P_Rango_Final + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Rangos_Identificadores_Colonias.Campo_Estatus + " = '" + Rango_Identificador_Colonia.P_Estatus + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Rangos_Identificadores_Colonias.Campo_Usuario_Modifico + " = '" + Rango_Identificador_Colonia.P_Usuario + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Rangos_Identificadores_Colonias.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Pre_Rangos_Identificadores_Colonias.Campo_Rango_Identificador_Colonia_ID + " = '" + Rango_Identificador_Colonia.P_Rango_Identificador_Colonia_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
                Actualizar = true;
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar";
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
                    Mensaje = "Error al intentar modificar un Registro de Rango Identificador de Colonia. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
            return Actualizar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_DataTable
        ///DESCRIPCIÓN          : Obtiene todos los Rango_Identificador_Colonia que estan dados de alta en la base de datos
        ///PARAMETROS          : 1. Rango_Identificador_Colonia.   Instancia de la Clase de Negocio de Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 28/Octubre/2010 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_DataTable(Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio Rango_Identificador_Colonia) {
            String Mi_SQL = null;
            DataSet Ds_Rangos_Identificadores_Colonias = null;
            DataTable Dt_Rangos_Identificadores_Colonias = new DataTable();
            try{
                if (Rango_Identificador_Colonia.P_Tipo_DataTable.Equals("RANGOS_IDENTIFICADORES")) { 
                    Mi_SQL = "SELECT " + Cat_Pre_Rangos_Identificadores_Colonias.Campo_Rango_Identificador_Colonia_ID + " AS RANGO_IDENTIFICADOR_COLONIA_ID, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Colonias.Campo_Descripcion + " AS DESCRIPCION";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Rangos_Identificadores_Colonias.Tabla_Cat_Pre_Rangos_Identificadores_Colonias + ", ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Colonias.Tabla_Cat_Pre_Tipos_Colonias;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Rangos_Identificadores_Colonias.Tabla_Cat_Pre_Rangos_Identificadores_Colonias + "." + Cat_Pre_Rangos_Identificadores_Colonias.Campo_Tipo_Colonia_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Pre_Tipos_Colonias.Tabla_Cat_Pre_Tipos_Colonias + "." + Cat_Pre_Tipos_Colonias.Campo_Tipo_Colonia_ID;
                    Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Tipos_Colonias.Tabla_Cat_Pre_Tipos_Colonias + "." + Cat_Pre_Tipos_Colonias.Campo_Descripcion;
                    Mi_SQL = Mi_SQL + " LIKE '%" + Rango_Identificador_Colonia.P_Tipo_Colonia_ID + "%'";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Rangos_Identificadores_Colonias.Campo_Rango_Identificador_Colonia_ID;
                } else if (Rango_Identificador_Colonia.P_Tipo_DataTable.Equals("TIPOS_COLONIAS")) {
                    Mi_SQL = "SELECT " + Cat_Pre_Tipos_Colonias.Campo_Tipo_Colonia_ID + " AS TIPO_COLONIA_ID, " + Cat_Pre_Tipos_Colonias.Campo_Descripcion + " AS DESCRIPCION";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Tipos_Colonias.Tabla_Cat_Pre_Tipos_Colonias;
                }
                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0){
                    Ds_Rangos_Identificadores_Colonias = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }            
                if (Ds_Rangos_Identificadores_Colonias != null) {
                    Dt_Rangos_Identificadores_Colonias = Ds_Rangos_Identificadores_Colonias.Tables[0];
                }
            }catch(Exception Ex){
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Rangos_Identificadores_Colonias;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Rango_Identificador_Colonia
        ///DESCRIPCIÓN          : Obtiene a detalle un Registro de un Rango_Identificador_Colonia
        ///PARAMETROS          : 1. Rango_Identificador_Colonia.   Instancia de la Clase de Negocio de Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 28/Octubre/2010 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio Consultar_Datos_Rango_Identificador_Colonia(Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio P_Rango_Identificador_Colonia) {
            String Mi_SQL = "SELECT " + Cat_Pre_Rangos_Identificadores_Colonias.Campo_Tipo_Colonia_ID + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Rangos_Identificadores_Colonias.Campo_Rango_Inicial + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Rangos_Identificadores_Colonias.Campo_Rango_Final + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Rangos_Identificadores_Colonias.Campo_Estatus;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Rangos_Identificadores_Colonias.Tabla_Cat_Pre_Rangos_Identificadores_Colonias;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Rangos_Identificadores_Colonias.Campo_Rango_Identificador_Colonia_ID + " = '" + P_Rango_Identificador_Colonia.P_Rango_Identificador_Colonia_ID + "'";
            Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio R_Rango_Identificador_Colonia = new Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio();
            OracleDataReader Dr_Rangos_Identificadores_Colonias;
            try {
                Dr_Rangos_Identificadores_Colonias = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Rango_Identificador_Colonia.P_Rango_Identificador_Colonia_ID = P_Rango_Identificador_Colonia.P_Rango_Identificador_Colonia_ID;
                while (Dr_Rangos_Identificadores_Colonias.Read()){
                    R_Rango_Identificador_Colonia.P_Tipo_Colonia_ID = Dr_Rangos_Identificadores_Colonias[Cat_Pre_Rangos_Identificadores_Colonias.Campo_Tipo_Colonia_ID].ToString();
                    R_Rango_Identificador_Colonia.P_Rango_Inicial = Convert.ToInt32(Dr_Rangos_Identificadores_Colonias[Cat_Pre_Rangos_Identificadores_Colonias.Campo_Rango_Inicial].ToString());
                    R_Rango_Identificador_Colonia.P_Rango_Final =  Convert.ToInt32(Dr_Rangos_Identificadores_Colonias[Cat_Pre_Rangos_Identificadores_Colonias.Campo_Rango_Final].ToString());
                    R_Rango_Identificador_Colonia.P_Estatus = Dr_Rangos_Identificadores_Colonias[Cat_Pre_Rangos_Identificadores_Colonias.Campo_Estatus].ToString();;
                }//
                Dr_Rangos_Identificadores_Colonias.Close();
            } catch (OracleException Ex) {
                String Mensaje = "Error al intentar consultar el registro de Rango Identificador de Colonia. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Rango_Identificador_Colonia;
        }
 
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Eliminar_Rango_Identificador_Colonia
        ///DESCRIPCIÓN          : Elimina un Registro de un Rango_Identificador_Colonia
        ///PARAMETROS          : 1. Rango_Identificador_Colonia.   Instancia de la Clase de Negocio de Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 28/Octubre/2010 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Eliminar_Rango_Identificador_Colonia(Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio Rango_Identificador_Colonia) {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Boolean Eliminar = false;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try {
                String Mi_SQL = "DELETE FROM " + Cat_Pre_Rangos_Identificadores_Colonias.Tabla_Cat_Pre_Rangos_Identificadores_Colonias;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Rangos_Identificadores_Colonias.Campo_Rango_Identificador_Colonia_ID + " = '" + Rango_Identificador_Colonia.P_Rango_Identificador_Colonia_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
                Eliminar = true;
            } catch (OracleException Ex) {
                if (Ex.Code == 547) {
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]";
                } else {
                    Mensaje = "Error al intentar eliminar el registro de Rango Identificador de Colonia. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            } catch (Exception Ex) {
                Mensaje = "Error al intentar eliminar el registro de Rango Identificador de Colonia. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
            return Eliminar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : Antonio Salvador Benavides Guardado
        ///FECHA_MODIFICO       : 26/Octubre/2010
        ///CAUSA_MODIFICACIÓN   : Estandarizar variables usadas
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
        ///PARAMETROS:     
        ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
        ///             2. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : Antonio Salvador Benavides Guardado
        ///FECHA_MODIFICO       : 26/Octubre/2010
        ///CAUSA_MODIFICACIÓN   : Estandarizar variables usadas
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