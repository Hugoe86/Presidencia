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
using Presidencia.Catalogo_Diferencias.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Diferencias_Datos
/// </summary>
/// 

namespace Presidencia.Catalogo_Diferencias.Datos{
    public class Cls_Cat_Pre_Diferencias_Datos {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Diferencia
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nueva Diferencia
        ///PARAMETROS:     
        ///             1. Tasa_Predial.    Instancia de la Clase de Negocio de Cls_Cat_Pre_Tasas_Prediales_Negocio
        ///                                 con los datos de la Tasa predial que va a ser dado de Alta.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 31/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Diferencia(Cls_Cat_Pre_Diferencias_Negocio Tasa_Predial){
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
                String Tasa_Predial_ID = Obtener_ID_Consecutivo(Cat_Pre_Tasas_Predial.Tabla_Cat_Pre_Tasas_Predial, Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Tasas_Predial.Tabla_Cat_Pre_Tasas_Predial;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID + ", " + Cat_Pre_Tasas_Predial.Campo_Identificador;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tasas_Predial.Campo_Descripcion + ", " + Cat_Pre_Tasas_Predial.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tasas_Predial.Campo_Usuario_Creo + ", " + Cat_Pre_Tasas_Predial.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Tasa_Predial_ID + "', '" + Tasa_Predial.P_Identificador + "'";
                Mi_SQL = Mi_SQL + ",'" + Tasa_Predial.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + ",'" + Tasa_Predial.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Tasa_Predial.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", SYSDATE";
                Mi_SQL = Mi_SQL + ")";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                if (Tasa_Predial.P_Diferencias_Tasas != null && Tasa_Predial.P_Diferencias_Tasas.Rows.Count > 0)
                {
                    String Tasa_ID = Obtener_ID_Consecutivo(Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual, Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID, 5);
                    for (int cnt = 0; cnt < Tasa_Predial.P_Diferencias_Tasas.Rows.Count; cnt++)
                    {
                        Mi_SQL = "INSERT INTO " + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual;
                        Mi_SQL = Mi_SQL + " (" + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID + ", " + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Predial_ID;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tasas_Predial_Anual.Campo_Año;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tasas_Predial_Anual.Campo_Usuario_Creo + ", " + Cat_Pre_Tasas_Predial_Anual.Campo_Fecha_Creo + ")";
                        Mi_SQL = Mi_SQL + " VALUES ('" + Tasa_ID + "', '" + Tasa_Predial_ID + "', '" + Tasa_Predial.P_Diferencias_Tasas.Rows[cnt][1].ToString() + "', " + Convert.ToDouble(Tasa_Predial.P_Diferencias_Tasas.Rows[cnt][2].ToString())+" ";
                        Mi_SQL = Mi_SQL + ",'" + Tasa_Predial.P_Usuario + "', SYSDATE";
                        Mi_SQL = Mi_SQL + ")";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        Tasa_ID = Convertir_A_Formato_ID(Convert.ToInt32(Tasa_ID) + 1, 5);
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Tasas Prediales. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }finally {
                 Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Diferencia
        ///DESCRIPCIÓN: Actualiza en la Base de Datos una Diferencia
        ///PARAMETROS:     
        ///             1. Tasa_Predial.    Instancia de la Clase de Negocio de Tasas prediales 
        ///                                 con los datos de la tasa predial que va a ser Actualizada.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 31/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Diferencia(Cls_Cat_Pre_Diferencias_Negocio Tasa_Predial){
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
                Cls_Cat_Pre_Diferencias_Negocio Tasa_Predial_Tmp = Consultar_Datos_Diferencia(Tasa_Predial);
                String Mi_SQL = "UPDATE " + Cat_Pre_Tasas_Predial.Tabla_Cat_Pre_Tasas_Predial + " SET " + Cat_Pre_Tasas_Predial.Campo_Identificador + " = '" + Tasa_Predial.P_Identificador + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Tasas_Predial.Campo_Estatus + " = '" + Tasa_Predial.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Tasas_Predial.Campo_Descripcion + " = '" + Tasa_Predial.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Tasas_Predial.Campo_Usuario_Modifico + " = '" + Tasa_Predial.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Tasas_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID + " = '" + Tasa_Predial.P_Tasa_Predial_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Tasa_Predial_Tmp = Obtener_Diferencias_Tasas_Eliminados(Tasa_Predial_Tmp, Tasa_Predial);
                for (int cnt = 0; cnt < Tasa_Predial_Tmp.P_Diferencias_Tasas.Rows.Count; cnt++)
                {
                    Mi_SQL = "DELETE FROM " + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID + " = '" + Tasa_Predial_Tmp.P_Diferencias_Tasas.Rows[cnt][0].ToString() + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }
                if (Tasa_Predial.P_Diferencias_Tasas != null && Tasa_Predial.P_Diferencias_Tasas.Rows.Count > 0)
                {
                    String Tasa_ID = Obtener_ID_Consecutivo(Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual, Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID, 5);
                    for (int cnt = 0; cnt < Tasa_Predial.P_Diferencias_Tasas.Rows.Count; cnt++)
                    {
                        if (Tasa_Predial.P_Diferencias_Tasas.Rows[cnt][0].ToString().Trim().Equals(""))
                        {
                            Mi_SQL = "INSERT INTO " + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual;
                            Mi_SQL = Mi_SQL + " (" + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID + ", " + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Predial_ID;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tasas_Predial_Anual.Campo_Año;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tasas_Predial_Anual.Campo_Usuario_Creo + ", " + Cat_Pre_Tasas_Predial_Anual.Campo_Fecha_Creo + ")";
                            Mi_SQL = Mi_SQL + " VALUES ('" + Tasa_ID + "', '" + Tasa_Predial.P_Tasa_Predial_ID + "', " + Tasa_Predial.P_Diferencias_Tasas.Rows[cnt][1].ToString() + ", " + Tasa_Predial.P_Diferencias_Tasas.Rows[cnt][2].ToString();
                            Mi_SQL = Mi_SQL + ",'" + Tasa_Predial.P_Usuario + "', SYSDATE";
                            Mi_SQL = Mi_SQL + ")";
                            Tasa_ID = Convertir_A_Formato_ID(Convert.ToInt32(Tasa_ID) + 1, 5);
                        } else {
                            Mi_SQL = "UPDATE " + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + " SET " + Cat_Pre_Tasas_Predial_Anual.Campo_Año + " = " + Tasa_Predial.P_Diferencias_Tasas.Rows[cnt][1].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual + " = " + Tasa_Predial.P_Diferencias_Tasas.Rows[cnt][2].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tasas_Predial_Anual.Campo_Usuario_Modifico + " = '" + Tasa_Predial.P_Usuario + "'";
                            Mi_SQL = Mi_SQL + "," + Cat_Pre_Tasas_Predial_Anual.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID + " = '" + Tasa_Predial.P_Diferencias_Tasas.Rows[cnt][0].ToString().Trim() + "'";
                            //Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Tasas_Predial_Anual.Campo_ + " = '" + Tasa_Predial.P_Diferencias_Tasas.Rows[cnt][0].ToString().Trim() + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Tasas Prediales. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Diferencias_Tasas_Eliminados
        ///DESCRIPCIÓN: Obtiene la lista de las Tasas que fueron dados de alta en la Actualizacion de la
        ///             Tasa predial
        ///PARAMETROS:     
        ///             1. Actuales.        Tasa predial que se usa para saber las Tasas que estan en 
        ///                                 la Base de Datos antes de la Modificacion.
        ///             2. Actualizados.    Tasa predial que se usa para saber las Tasas que estaran en 
        ///                                 la Base de Datos despues de la Modificacion.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 31/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static Cls_Cat_Pre_Diferencias_Negocio Obtener_Diferencias_Tasas_Eliminados(Cls_Cat_Pre_Diferencias_Negocio Actuales, Cls_Cat_Pre_Diferencias_Negocio Actualizados){
            Cls_Cat_Pre_Diferencias_Negocio Tasa_Predial = new Cls_Cat_Pre_Diferencias_Negocio();
            DataTable tabla = new DataTable();
            tabla.Columns.Add("TASA_ID", Type.GetType("System.String"));
            tabla.Columns.Add("ANIO", Type.GetType("System.String"));
            tabla.Columns.Add("TASA_ANUAL", Type.GetType("System.String"));
            for (int cnt1 = 0; cnt1 < Actuales.P_Diferencias_Tasas.Rows.Count; cnt1++) {
                bool eliminar = true;
                for (int cnt2 = 0; cnt2 < Actualizados.P_Diferencias_Tasas.Rows.Count; cnt2++) {
                    if (!Actualizados.P_Diferencias_Tasas.Rows[cnt2][0].ToString().Equals("")) {
                        if (Actuales.P_Diferencias_Tasas.Rows[cnt1][0].ToString().Equals(Actualizados.P_Diferencias_Tasas.Rows[cnt2][0].ToString())) {
                            eliminar = false;
                            break;
                        }
                    }
                }
                if (eliminar) {
                    DataRow fila = tabla.NewRow();
                    fila["TASA_ID"] = Actuales.P_Diferencias_Tasas.Rows[cnt1][0].ToString();
                    fila["ANIO"] = Actuales.P_Diferencias_Tasas.Rows[cnt1][1].ToString();
                    fila["TASA_ANUAL"] = Actuales.P_Diferencias_Tasas.Rows[cnt1][2].ToString();
                    tabla.Rows.Add(fila);
                }
            }
            Tasa_Predial.P_Diferencias_Tasas = tabla;
            return Tasa_Predial;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Diferencias
        ///DESCRIPCIÓN: Obtiene todas las Tasas Prediales que estan dadas de alta en la Base de Datos
        ///PARAMETROS:   
        ///             1.  Tasa_Predial.       Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                                     caso el filtro es el Identificador.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 31/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Diferencias(Cls_Cat_Pre_Diferencias_Negocio Tasa_Predial){
            DataTable Tabla = new DataTable();
            try{
                String Mi_SQL = "SELECT " + Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID + " AS TASA_PREDIAL_ID, " + Cat_Pre_Tasas_Predial.Campo_Identificador + " AS IDENTIFICADOR";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Tasas_Predial.Campo_Estatus + " AS ESTATUS";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Tasas_Predial.Campo_Descripcion + " AS DESCRIPCION";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Tasas_Predial.Tabla_Cat_Pre_Tasas_Predial;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Tasas_Predial.Campo_Descripcion + " LIKE '%" + Tasa_Predial.P_Identificador + "%' ";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null) {
                    Tabla = dataset.Tables[0];
                }
            }catch(Exception Ex){
                String Mensaje = "Error al intentar consultar los registros de Tasas Prediales. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Diferencia
        ///DESCRIPCIÓN: Obtiene a detalle una Diferencia.
        ///PARAMETROS:   
        ///             1. P_Tasa_Predial.   Diferencia que se va ver a Detalle.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 31/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Diferencias_Negocio Consultar_Datos_Diferencia(Cls_Cat_Pre_Diferencias_Negocio P_Tasa_Predial){
            String Mi_SQL = "SELECT " + Cat_Pre_Tasas_Predial.Campo_Identificador + ", " + Cat_Pre_Tasas_Predial.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tasas_Predial.Campo_Descripcion + " FROM " + Cat_Pre_Tasas_Predial.Tabla_Cat_Pre_Tasas_Predial;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID + " = '" + P_Tasa_Predial.P_Tasa_Predial_ID + "'";
            Cls_Cat_Pre_Diferencias_Negocio R_Tasa_Predial = new Cls_Cat_Pre_Diferencias_Negocio();
            OracleDataReader Data_Reader;
            try{
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Tasa_Predial.P_Tasa_Predial_ID = P_Tasa_Predial.P_Tasa_Predial_ID;
                while (Data_Reader.Read()){
                    R_Tasa_Predial.P_Identificador = Data_Reader[Cat_Pre_Tasas_Predial.Campo_Identificador].ToString();
                    R_Tasa_Predial.P_Estatus = Data_Reader[Cat_Pre_Tasas_Predial.Campo_Estatus].ToString();
                    R_Tasa_Predial.P_Descripcion = Data_Reader[Cat_Pre_Tasas_Predial.Campo_Descripcion].ToString();
                }
                Data_Reader.Close();
                Mi_SQL = "SELECT " + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID + " AS TASA_ID, " + Cat_Pre_Tasas_Predial_Anual.Campo_Año + " AS ANIO";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual + " AS TASA_ANUAL";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Predial_ID + " = '" + P_Tasa_Predial.P_Tasa_Predial_ID + "'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Tasas_Predial_Anual.Campo_Año + " DESC";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset == null){
                    R_Tasa_Predial.P_Diferencias_Tasas = new DataTable();
                }else{
                    R_Tasa_Predial.P_Diferencias_Tasas = dataset.Tables[0];
                }
            }catch (Exception Ex){
                String Mensaje = "Error al intentar consultar el registro de Tasas Prediales. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Tasa_Predial;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Diferencia
        ///DESCRIPCIÓN: Elimina una Tasa predial
        ///PARAMETROS:   
        ///             1. Tasa_Predial.   Tasa predial que se va eliminar.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Diferencia(Cls_Cat_Pre_Diferencias_Negocio Tasa_Predial){
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
                String Mi_SQL = "DELETE FROM " + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Predial_ID + " = '" + Tasa_Predial.P_Tasa_Predial_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Mi_SQL = "DELETE FROM " + Cat_Pre_Tasas_Predial.Tabla_Cat_Pre_Tasas_Predial;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID + " = '" + Tasa_Predial.P_Tasa_Predial_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            } catch (OracleException Ex) {
                if (Ex.Code == 547){
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]";
                } else {
                    Mensaje = "Error al intentar eliminar el registro de Tasas prediales. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex) {
                Mensaje = "Error al intentar eliminar el registro de Tasas prediales. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                {
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