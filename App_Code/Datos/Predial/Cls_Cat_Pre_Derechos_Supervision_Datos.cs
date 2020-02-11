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
using Presidencia.Catalogo_Derechos_Supervision.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Derechos_Supervision_Datos
/// </summary>

namespace Presidencia.Catalogo_Derechos_Supervision.Datos {

    public class Cls_Cat_Pre_Derechos_Supervision_Datos{

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Derecho_Supervision
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo Derecho Supervision
        ///PARAMETROS:     
        ///             1. Derecho_Supervision. Instancia de la Clase de Negocio de 
        ///                                     Cls_Cat_Pre_Derechos_Supervision_Negocio
        ///                                     con los datos del Derecho Supervision que 
        ///                                     va a ser dado de Alta.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 06/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Derecho_Supervision(Cls_Cat_Pre_Derechos_Supervision_Negocio Derecho_Supervision){
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
                String Derecho_Supervision_ID = Obtener_ID_Consecutivo(Cat_Pre_Derechos_Supervision.Tabla_Cat_Pre_Derechos_Supervision, Cat_Pre_Derechos_Supervision.Campo_Derecho_Supervision_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Derechos_Supervision.Tabla_Cat_Pre_Derechos_Supervision;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Derechos_Supervision.Campo_Derecho_Supervision_ID + ", " + Cat_Pre_Derechos_Supervision.Campo_Identificador;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Derechos_Supervision.Campo_Descripcion + ", " + Cat_Pre_Derechos_Supervision.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Derechos_Supervision.Campo_Usuario_Creo + ", " + Cat_Pre_Derechos_Supervision.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Derecho_Supervision_ID + "', '" + Derecho_Supervision.P_Identificador + "'";
                Mi_SQL = Mi_SQL + ",'" + Derecho_Supervision.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + ",'" + Derecho_Supervision.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Derecho_Supervision.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", SYSDATE";
                Mi_SQL = Mi_SQL + ")";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                if (Derecho_Supervision.P_Derechos_Tasas != null && Derecho_Supervision.P_Derechos_Tasas.Rows.Count > 0){
                    String Derecho_Tasa_ID = Obtener_ID_Consecutivo(Cat_Pre_Der_Super_Tasas.Tabla_Cat_Pre_Der_Super_Tasas, Cat_Pre_Der_Super_Tasas.Campo_Derecho_Supervision_Tasa_ID, 5);
                    for (int cnt = 0; cnt < Derecho_Supervision.P_Derechos_Tasas.Rows.Count; cnt++){                    
                        Mi_SQL = "INSERT INTO " + Cat_Pre_Der_Super_Tasas.Tabla_Cat_Pre_Der_Super_Tasas;
                        Mi_SQL = Mi_SQL + " (" + Cat_Pre_Der_Super_Tasas.Campo_Derecho_Supervision_Tasa_ID + ", " + Cat_Pre_Der_Super_Tasas.Campo_Derecho_Supervision_ID;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Der_Super_Tasas.Campo_Año + ", " + Cat_Pre_Der_Super_Tasas.Campo_Tasa;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Der_Super_Tasas.Campo_Usuario_Creo + ", " + Cat_Pre_Der_Super_Tasas.Campo_Fecha_Creo + ")";
                        Mi_SQL = Mi_SQL + " VALUES ('" + Derecho_Tasa_ID + "', '" + Derecho_Supervision_ID + "', " + Derecho_Supervision.P_Derechos_Tasas.Rows[cnt][1].ToString();
                        Mi_SQL = Mi_SQL +", " + Convert.ToDouble(Derecho_Supervision.P_Derechos_Tasas.Rows[cnt][2].ToString());
                        Mi_SQL = Mi_SQL + ",'" + Derecho_Supervision.P_Usuario + "', SYSDATE";
                        Mi_SQL = Mi_SQL + ")";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        Derecho_Tasa_ID = Convertir_A_Formato_ID(Convert.ToInt32(Derecho_Tasa_ID) + 1, 5);
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Derechos de Supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }finally {
                 Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Derecho_Supervision
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Derecho Supervision
        ///PARAMETROS:     
        ///             1. Derecho_Supervision. Instancia de la Clase de Negocio de Derecho 
        ///                                     Supervision con los datos del Derecho Supervision 
        ///                                     que va a ser Actualizada.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 06/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Derecho_Supervision(Cls_Cat_Pre_Derechos_Supervision_Negocio Derecho_Supervision){
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
                Cls_Cat_Pre_Derechos_Supervision_Negocio Derecho_Supervision_Tmp = Consultar_Datos_Derecho_Supervision(Derecho_Supervision);
                String Mi_SQL = "UPDATE " + Cat_Pre_Derechos_Supervision.Tabla_Cat_Pre_Derechos_Supervision + " SET " + Cat_Pre_Derechos_Supervision.Campo_Identificador + " = '" + Derecho_Supervision.P_Identificador + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Derechos_Supervision.Campo_Estatus + " = '" + Derecho_Supervision.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Derechos_Supervision.Campo_Descripcion + " = '" + Derecho_Supervision.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Derecho_Supervision.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Derechos_Supervision.Campo_Derecho_Supervision_ID + " = '" + Derecho_Supervision.P_Derecho_Supervision_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Derecho_Supervision_Tmp = Obtener_Derechos_Supervision_Tasas_Eliminados(Derecho_Supervision_Tmp, Derecho_Supervision);
                for (int cnt = 0; cnt < Derecho_Supervision_Tmp.P_Derechos_Tasas.Rows.Count; cnt++) {
                    Mi_SQL = "DELETE FROM " + Cat_Pre_Der_Super_Tasas.Tabla_Cat_Pre_Der_Super_Tasas;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Der_Super_Tasas.Campo_Derecho_Supervision_Tasa_ID + " = '" + Derecho_Supervision_Tmp.P_Derechos_Tasas.Rows[cnt][0].ToString() + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }
                if (Derecho_Supervision.P_Derechos_Tasas!= null && Derecho_Supervision.P_Derechos_Tasas.Rows.Count > 0){
                    String Derecho_Tasa_ID = Obtener_ID_Consecutivo(Cat_Pre_Der_Super_Tasas.Tabla_Cat_Pre_Der_Super_Tasas, Cat_Pre_Der_Super_Tasas.Campo_Derecho_Supervision_Tasa_ID, 5);
                    for (int cnt = 0; cnt < Derecho_Supervision.P_Derechos_Tasas.Rows.Count; cnt++){
                        if (Derecho_Supervision.P_Derechos_Tasas.Rows[cnt][0].ToString().Trim().Equals("")){
                            Mi_SQL = "INSERT INTO " + Cat_Pre_Der_Super_Tasas.Tabla_Cat_Pre_Der_Super_Tasas;
                            Mi_SQL = Mi_SQL + " (" + Cat_Pre_Der_Super_Tasas.Campo_Derecho_Supervision_Tasa_ID + ", " + Cat_Pre_Der_Super_Tasas.Campo_Derecho_Supervision_ID;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Der_Super_Tasas.Campo_Año + ", " + Cat_Pre_Der_Super_Tasas.Campo_Tasa;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Der_Super_Tasas.Campo_Usuario_Creo + ", " + Cat_Pre_Der_Super_Tasas.Campo_Fecha_Creo + ")";
                            Mi_SQL = Mi_SQL + " VALUES ('" + Derecho_Tasa_ID + "', '" + Derecho_Supervision.P_Derecho_Supervision_ID + "', " + Derecho_Supervision.P_Derechos_Tasas.Rows[cnt][2].ToString();
                            Mi_SQL = Mi_SQL + ", " + Convert.ToDouble(Derecho_Supervision.P_Derechos_Tasas.Rows[cnt][3].ToString());
                            Mi_SQL = Mi_SQL + ",'" + Derecho_Supervision.P_Usuario + "', SYSDATE";
                            Mi_SQL = Mi_SQL + ")";
                            Derecho_Tasa_ID = Convertir_A_Formato_ID(Convert.ToInt32(Derecho_Tasa_ID) + 1, 5);
                        } else {
                            Mi_SQL = "UPDATE " + Cat_Pre_Der_Super_Tasas.Tabla_Cat_Pre_Der_Super_Tasas + " SET " + Cat_Pre_Der_Super_Tasas.Campo_Año + " = " + Derecho_Supervision.P_Derechos_Tasas.Rows[cnt][2].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Der_Super_Tasas.Campo_Tasa + " = " + Derecho_Supervision.P_Derechos_Tasas.Rows[cnt][3].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Der_Super_Tasas.Campo_Usuario_Modifico + " = '" + Derecho_Supervision.P_Usuario + "'";
                            Mi_SQL = Mi_SQL + "," + Cat_Pre_Der_Super_Tasas.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Der_Super_Tasas.Campo_Derecho_Supervision_Tasa_ID + " = '" + Derecho_Supervision.P_Derechos_Tasas.Rows[cnt][0].ToString().Trim() + "'";
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
                    Mensaje = "Error al intentar dar Modificar un Registro de Derechos de Supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Derechos_Supervision_Tasas_Eliminados
        ///DESCRIPCIÓN: Obtiene la lista de los Derechos Supervision Tasas que fueron dados de alta en la Actualizacion de las
        ///             Derechos Supervision
        ///PARAMETROS:     
        ///             1. Actuales.        Derecho Supervision que se usa para saber los Derecho Supervision Tasas que estan en 
        ///                                 la Base de Datos antes de la Modificacion.
        ///             2. Actualizados.    Derecho Supervision que se usa para saber los Derecho Supervision Tasas que estaran en 
        ///                                 la Base de Datos despues de la Modificacion.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 06/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static Cls_Cat_Pre_Derechos_Supervision_Negocio Obtener_Derechos_Supervision_Tasas_Eliminados(Cls_Cat_Pre_Derechos_Supervision_Negocio Actuales, Cls_Cat_Pre_Derechos_Supervision_Negocio Actualizados){
            Cls_Cat_Pre_Derechos_Supervision_Negocio Derecho_Supervision = new Cls_Cat_Pre_Derechos_Supervision_Negocio();
            DataTable tabla = new DataTable();
            tabla.Columns.Add("DERECHO_SUPERVISION_TASA_ID", Type.GetType("System.String"));
            tabla.Columns.Add("ANIO", Type.GetType("System.String"));
            tabla.Columns.Add("TASA", Type.GetType("System.String"));
            for (int cnt1 = 0; cnt1 < Actuales.P_Derechos_Tasas.Rows.Count; cnt1++){
                bool eliminar = true;
                for (int cnt2 = 0; cnt2 < Actualizados.P_Derechos_Tasas.Rows.Count; cnt2++) {
                    if (!Actualizados.P_Derechos_Tasas.Rows[cnt2][0].ToString().Equals("")) {
                        if (Actuales.P_Derechos_Tasas.Rows[cnt1][0].ToString().Equals(Actualizados.P_Derechos_Tasas.Rows[cnt2][0].ToString())){
                            eliminar = false;
                            break;
                        }
                    }
                }
                if (eliminar) {
                    DataRow fila = tabla.NewRow();
                    fila["DERECHO_SUPERVISION_TASA_ID"] = Actuales.P_Derechos_Tasas.Rows[cnt1][0].ToString();
                    fila["ANIO"] = Actuales.P_Derechos_Tasas.Rows[cnt1][1].ToString();
                    fila["TASA"] = Actuales.P_Derechos_Tasas.Rows[cnt1][2].ToString();
                    tabla.Rows.Add(fila);
                }
            }
            Derecho_Supervision.P_Derechos_Tasas = tabla;
            return Derecho_Supervision;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Derechos_Supervision
        ///DESCRIPCIÓN: Obtiene todas los Derechos Supervision que estan dadas de alta en 
        ///             la Base de Datos
        ///PARAMETROS:   
        ///             1.  Derecho_Supervision.    Parametro de donde se sacara si habra o 
        ///                                         no un filtro de busqueda, en este caso 
        ///                                         el filtro es el Identificador.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 06/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Derechos_Supervision(Cls_Cat_Pre_Derechos_Supervision_Negocio Derecho_Supervision) {
            DataTable Tabla = new DataTable();
            try{
                String Mi_SQL = "SELECT " + Cat_Pre_Derechos_Supervision.Campo_Derecho_Supervision_ID + " AS DERECHO_SUPERVISION_ID, " + Cat_Pre_Derechos_Supervision.Campo_Identificador + " AS IDENTIFICADOR";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Derechos_Supervision.Campo_Descripcion + " AS DESCRIPCION";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Derechos_Supervision.Campo_Estatus + " AS ESTATUS";
                Mi_SQL = Mi_SQL +  " FROM " + Cat_Pre_Derechos_Supervision.Tabla_Cat_Pre_Derechos_Supervision;
                Mi_SQL = Mi_SQL + " WHERE ";
                if (Derecho_Supervision.P_Identificador != null && Derecho_Supervision.P_Identificador != "")
                {
                    Mi_SQL = Mi_SQL + Cat_Pre_Derechos_Supervision.Campo_Identificador + " LIKE '%" + Derecho_Supervision.P_Identificador + "%' AND ";
                }
                if (Derecho_Supervision.P_Derecho_Supervision_ID != null && Derecho_Supervision.P_Derecho_Supervision_ID != "")
                {
                    Mi_SQL = Mi_SQL + Cat_Pre_Derechos_Supervision.Campo_Derecho_Supervision_ID + Validar_Operador_Comparacion(Derecho_Supervision.P_Derecho_Supervision_ID) + " AND ";
                }
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" OR "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 4);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Derechos_Supervision.Campo_Derecho_Supervision_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null) {
                    Tabla = dataset.Tables[0];
                }
            }catch(Exception Ex){
                String Mensaje = "Error al intentar consultar los registros de Derechos de Supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Derecho_Supervision
        ///DESCRIPCIÓN: Obtiene a detalle una Derecho Supervision.
        ///PARAMETROS:   
        ///             1. P_Derecho_Supervision.   Derecho Supervision que se va ver a Detalle.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 06/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Derechos_Supervision_Negocio Consultar_Datos_Derecho_Supervision(Cls_Cat_Pre_Derechos_Supervision_Negocio P_Derecho_Supervision){
            String Mi_SQL = "SELECT " + Cat_Pre_Derechos_Supervision.Campo_Identificador + ", " + Cat_Pre_Derechos_Supervision.Campo_Estatus + ", " + Cat_Pre_Derechos_Supervision.Campo_Descripcion;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Derechos_Supervision.Tabla_Cat_Pre_Derechos_Supervision;
            if (P_Derecho_Supervision.P_Derecho_Supervision_ID != null && P_Derecho_Supervision.P_Derecho_Supervision_ID != "")
            {
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Derechos_Supervision.Campo_Derecho_Supervision_ID + Validar_Operador_Comparacion(P_Derecho_Supervision.P_Derecho_Supervision_ID);
            }
            Cls_Cat_Pre_Derechos_Supervision_Negocio R_Derecho_Supervision = new Cls_Cat_Pre_Derechos_Supervision_Negocio();
            OracleDataReader Data_Reader;
            try{
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Derecho_Supervision.P_Derecho_Supervision_ID = P_Derecho_Supervision.P_Derecho_Supervision_ID;
                while (Data_Reader.Read()){
                    R_Derecho_Supervision.P_Identificador = Data_Reader[Cat_Pre_Derechos_Supervision.Campo_Identificador].ToString();
                    R_Derecho_Supervision.P_Estatus = Data_Reader[Cat_Pre_Derechos_Supervision.Campo_Estatus].ToString();
                    R_Derecho_Supervision.P_Descripcion = Data_Reader[Cat_Pre_Derechos_Supervision.Campo_Descripcion].ToString();
                }
                Data_Reader.Close();
                Mi_SQL = "SELECT " + Cat_Pre_Der_Super_Tasas.Campo_Derecho_Supervision_Tasa_ID + " AS DERECHO_SUPERVISION_TASA_ID, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Der_Super_Tasas.Campo_Derecho_Supervision_ID + " AS DERECHO_SUPERVISION_ID, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Der_Super_Tasas.Campo_Año + " AS ANIO, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Der_Super_Tasas.Campo_Tasa + " AS TASA";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Der_Super_Tasas.Tabla_Cat_Pre_Der_Super_Tasas;
                Mi_SQL = Mi_SQL + " WHERE ";
                if (P_Derecho_Supervision.P_Derecho_Supervision_ID != null && P_Derecho_Supervision.P_Derecho_Supervision_ID != "")
                {
                    Mi_SQL = Mi_SQL + Cat_Pre_Der_Super_Tasas.Campo_Derecho_Supervision_ID + Validar_Operador_Comparacion(P_Derecho_Supervision.P_Derecho_Supervision_ID) + " AND ";
                }
                if (P_Derecho_Supervision.P_Derecho_Supervision_Tasa_ID != null && P_Derecho_Supervision.P_Derecho_Supervision_Tasa_ID != "")
                {
                    Mi_SQL = Mi_SQL + Cat_Pre_Der_Super_Tasas.Campo_Derecho_Supervision_Tasa_ID + Validar_Operador_Comparacion(P_Derecho_Supervision.P_Derecho_Supervision_Tasa_ID) + " AND ";
                }
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" OR "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 4);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Der_Super_Tasas.Campo_Año + " DESC";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset == null){
                    R_Derecho_Supervision.P_Derechos_Tasas = new DataTable();
                }else{
                    R_Derecho_Supervision.P_Derechos_Tasas = dataset.Tables[0];
                }
            }catch (Exception Ex){
                String Mensaje = "Error al intentar consultar los registros de Derechos de Supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Derecho_Supervision;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Derechos_Supervision_Tasas
        ///DESCRIPCIÓN          : Obtiene todos las Convenio_Traslado_Dominio que estan dadas de alta en la base de datos
        ///PARAMETROS           : 1. Convenio_Traslado_Dominio.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Derecho_Supervisions_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 03/Agosto/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Derechos_Supervision_Tasas(Cls_Cat_Pre_Derechos_Supervision_Negocio Derecho_Supervision)
        {
            DataTable Dt_Derechos_Supervision = new DataTable();
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT ";
                if (Derecho_Supervision.P_Campos_Dinamicos != null && Derecho_Supervision.P_Campos_Dinamicos != "")
                {
                    Mi_SQL += Derecho_Supervision.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL += Cat_Pre_Derechos_Supervision.Tabla_Cat_Pre_Derechos_Supervision + "." + Cat_Pre_Derechos_Supervision.Campo_Derecho_Supervision_ID + ", ";
                    Mi_SQL += Cat_Pre_Derechos_Supervision.Tabla_Cat_Pre_Derechos_Supervision + "." + Cat_Pre_Derechos_Supervision.Campo_Identificador + ", ";
                    Mi_SQL += Cat_Pre_Derechos_Supervision.Tabla_Cat_Pre_Derechos_Supervision + "." + Cat_Pre_Derechos_Supervision.Campo_Descripcion + ", ";
                    Mi_SQL += Cat_Pre_Derechos_Supervision.Tabla_Cat_Pre_Derechos_Supervision + "." + Cat_Pre_Derechos_Supervision.Campo_Estatus + ", ";

                    Mi_SQL += Cat_Pre_Der_Super_Tasas.Tabla_Cat_Pre_Der_Super_Tasas + "." + Cat_Pre_Der_Super_Tasas.Campo_Derecho_Supervision_Tasa_ID + ", ";
                    Mi_SQL += Cat_Pre_Der_Super_Tasas.Tabla_Cat_Pre_Der_Super_Tasas + "." + Cat_Pre_Der_Super_Tasas.Campo_Año + ", ";
                    Mi_SQL += Cat_Pre_Der_Super_Tasas.Tabla_Cat_Pre_Der_Super_Tasas + "." + Cat_Pre_Der_Super_Tasas.Campo_Tasa;
                }
                Mi_SQL += " FROM " + Cat_Pre_Derechos_Supervision.Tabla_Cat_Pre_Derechos_Supervision + ", " + Cat_Pre_Der_Super_Tasas.Tabla_Cat_Pre_Der_Super_Tasas;
                Mi_SQL += " WHERE " + Cat_Pre_Derechos_Supervision.Tabla_Cat_Pre_Derechos_Supervision + "." + Cat_Pre_Derechos_Supervision.Campo_Derecho_Supervision_ID + " = " + Cat_Pre_Der_Super_Tasas.Tabla_Cat_Pre_Der_Super_Tasas + "." + Cat_Pre_Der_Super_Tasas.Campo_Derecho_Supervision_ID + " AND ";
                if (Derecho_Supervision.P_Filtros_Dinamicos != null && Derecho_Supervision.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL += Derecho_Supervision.P_Filtros_Dinamicos;
                }
                else
                {
                    if (Derecho_Supervision.P_Derecho_Supervision_ID != "" && Derecho_Supervision.P_Derecho_Supervision_ID != null)
                    {
                        Mi_SQL += Cat_Pre_Derechos_Supervision.Tabla_Cat_Pre_Derechos_Supervision + "." + Cat_Pre_Derechos_Supervision.Campo_Derecho_Supervision_ID + " = '" + Derecho_Supervision.P_Derecho_Supervision_ID + "'";
                    }
                    if (Mi_SQL.EndsWith(" AND "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                    }
                    if (Mi_SQL.EndsWith(" WHERE "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                    }
                }
                if (Derecho_Supervision.P_Agrupar_Dinamico != null && Derecho_Supervision.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " GROUP BY " + Derecho_Supervision.P_Agrupar_Dinamico;
                }
                if (Derecho_Supervision.P_Ordenar_Dinamico != null && Derecho_Supervision.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Derecho_Supervision.P_Ordenar_Dinamico;
                }
                else
                {
                    Mi_SQL += " ORDER BY " + Cat_Pre_Derechos_Supervision.Tabla_Cat_Pre_Derechos_Supervision + "." + Cat_Pre_Derechos_Supervision.Campo_Identificador + ", " + Cat_Pre_Derechos_Supervision.Tabla_Cat_Pre_Derechos_Supervision + "." + Cat_Pre_Derechos_Supervision.Campo_Descripcion;
                }

                DataSet Ds_Derechos_Supervision = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Derechos_Supervision != null)
                {
                    Dt_Derechos_Supervision = Ds_Derechos_Supervision.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Derechos de Supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Derechos_Supervision;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Derechos_Supervision_Tasas
        ///DESCRIPCIÓN          : Obtiene todos las Convenio_Traslado_Dominio que estan dadas de alta en la base de datos
        ///PARAMETROS           : 1. Convenio_Traslado_Dominio.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Derecho_Supervisions_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 03/Agosto/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Derechos_Supervision_Detalles_Tasas(Cls_Cat_Pre_Derechos_Supervision_Negocio Derecho_Supervision)
        {
            DataTable Dt_Derechos_Supervision = new DataTable();
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT ";
                if (Derecho_Supervision.P_Campos_Dinamicos != null && Derecho_Supervision.P_Campos_Dinamicos != "")
                {
                    Mi_SQL += Derecho_Supervision.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL += Cat_Pre_Derechos_Supervision.Campo_Derecho_Supervision_ID + ", ";
                    Mi_SQL += Cat_Pre_Derechos_Supervision.Campo_Identificador + ", ";
                    Mi_SQL += Cat_Pre_Derechos_Supervision.Campo_Descripcion + ", ";
                    Mi_SQL += Cat_Pre_Derechos_Supervision.Campo_Estatus;
                }
                Mi_SQL += " FROM " + Cat_Pre_Derechos_Supervision.Tabla_Cat_Pre_Derechos_Supervision;
                Mi_SQL += " WHERE ";
                if (Derecho_Supervision.P_Filtros_Dinamicos != null && Derecho_Supervision.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL += Derecho_Supervision.P_Filtros_Dinamicos;
                }
                else
                {
                    if (Derecho_Supervision.P_Derecho_Supervision_ID != "" && Derecho_Supervision.P_Derecho_Supervision_ID != null)
                    {
                        Mi_SQL += Cat_Pre_Derechos_Supervision.Campo_Derecho_Supervision_ID + " = '" + Derecho_Supervision.P_Derecho_Supervision_ID + "'";
                    }
                    if (Mi_SQL.EndsWith(" AND "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                    }
                    if (Mi_SQL.EndsWith(" WHERE "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                    }
                }
                if (Derecho_Supervision.P_Agrupar_Dinamico != null && Derecho_Supervision.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " GROUP BY " + Derecho_Supervision.P_Agrupar_Dinamico;
                }
                if (Derecho_Supervision.P_Ordenar_Dinamico != null && Derecho_Supervision.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Derecho_Supervision.P_Ordenar_Dinamico;
                }
                else
                {
                    Mi_SQL += " ORDER BY " + Cat_Pre_Derechos_Supervision.Campo_Identificador + ", " + Cat_Pre_Derechos_Supervision.Campo_Descripcion;
                }

                DataSet Ds_Derechos_Supervision = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Derechos_Supervision != null)
                {
                    Dt_Derechos_Supervision = Ds_Derechos_Supervision.Tables[0];

                    Mi_SQL = "SELECT ";
                    Mi_SQL += Cat_Pre_Der_Super_Tasas.Campo_Derecho_Supervision_Tasa_ID + ", ";
                    Mi_SQL += Cat_Pre_Der_Super_Tasas.Campo_Año + ", ";
                    Mi_SQL += Cat_Pre_Der_Super_Tasas.Campo_Tasa;
                    Mi_SQL += " FROM ";
                    Mi_SQL += Cat_Pre_Der_Super_Tasas.Tabla_Cat_Pre_Der_Super_Tasas;
                    if (Derecho_Supervision.P_Filtros_Dinamicos != null && Derecho_Supervision.P_Filtros_Dinamicos != "")
                    {
                        Mi_SQL += " WHERE ";
                        Mi_SQL += Cat_Pre_Der_Super_Tasas.Campo_Derecho_Supervision_ID + " IN (SELECT " + Cat_Pre_Derechos_Supervision.Campo_Derecho_Supervision_ID + " FROM " + Cat_Pre_Derechos_Supervision.Tabla_Cat_Pre_Derechos_Supervision + " WHERE " + Derecho_Supervision.P_Filtros_Dinamicos + ")";
                    }
                    else
                    {
                        if (Derecho_Supervision.P_Derecho_Supervision_ID != null && Derecho_Supervision.P_Derecho_Supervision_ID != "")
                        {
                            Mi_SQL += " WHERE ";
                            Mi_SQL += Cat_Pre_Der_Super_Tasas.Campo_Derecho_Supervision_ID + " = '" + Derecho_Supervision.P_Derecho_Supervision_ID + "'";
                        }
                    }
                    Derecho_Supervision.P_Dt_Derechos_Supervision = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Derechos de Supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Derechos_Supervision;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Derecho_Supervision
        ///DESCRIPCIÓN: Elimina un Derecho Supervision
        ///PARAMETROS:   
        ///             1. Derecho_Supervision.   Derecho Supervision que se va eliminar.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 06/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Derecho_Supervision(Cls_Cat_Pre_Derechos_Supervision_Negocio Derecho_Supervision){
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
                String Mi_SQL = "DELETE FROM " + Cat_Pre_Der_Super_Tasas.Tabla_Cat_Pre_Der_Super_Tasas;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Der_Super_Tasas.Campo_Derecho_Supervision_ID + " = '" + Derecho_Supervision.P_Derecho_Supervision_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Mi_SQL = "DELETE FROM " + Cat_Pre_Derechos_Supervision.Tabla_Cat_Pre_Derechos_Supervision;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Derechos_Supervision.Campo_Derecho_Supervision_ID + " = '" + Derecho_Supervision.P_Derecho_Supervision_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            } catch (OracleException Ex) {
                if (Ex.Code == 547){
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]";
                } else {
                    Mensaje = "Error al intentar eliminar el registro de Derecho de Supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex) {
                Mensaje = "Error al intentar eliminar el registro de Derecho de Supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Validar_Operador_Comparacion
        ///DESCRIPCIÓN          : Devuelve una cadena adecuada al operador indicado en la capa de Negocios
        ///PARAMETROS           : 
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 11/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static String Validar_Operador_Comparacion(String Filtro)
        {
            String Cadena_Validada;
            if (Filtro.Trim().StartsWith("<")
               || Filtro.Trim().StartsWith(">")
               || Filtro.Trim().StartsWith("<>")
               || Filtro.Trim().StartsWith("<=")
               || Filtro.Trim().StartsWith(">=")
               || Filtro.Trim().StartsWith("=")
               || Filtro.Trim().ToUpper().StartsWith("BETWEEN")
               || Filtro.Trim().ToUpper().StartsWith("LIKE")
               || Filtro.Trim().ToUpper().StartsWith("IN"))
            {
                Cadena_Validada = " " + Filtro + " ";
            }
            else
            {
                Cadena_Validada = " = '" + Filtro + "' ";
            }
            return Cadena_Validada;
        }

    }
}