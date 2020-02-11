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
using Presidencia.Catalogo_Recargos.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Recargos_Datos
/// </summary>
/// 

namespace Presidencia.Catalogo_Recargos.Datos{
    public class Cls_Cat_Pre_Recargos_Datos{

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Recargo
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo Recargo
        ///PARAMETROS:     
        ///             1. Recargo.    Instancia de la Clase de Negocio de Cls_Cat_Pre_Recargos_Negocio
        ///                             con los datos del Recargo que va a ser dado de Alta.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 07/Septiembre/2010
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Recargo(Cls_Cat_Pre_Recargos_Negocio Recargo){
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
                String Recargo_ID = Obtener_ID_Consecutivo(Cat_Pre_Recargos.Tabla_Cat_Pre_Recargos, Cat_Pre_Recargos.Campo_Recargo_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Recargos.Tabla_Cat_Pre_Recargos;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Recargos.Campo_Recargo_ID + ", " + Cat_Pre_Recargos.Campo_Identificador;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Descripcion + ", " + Cat_Pre_Recargos.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Usuario_Creo + ", " + Cat_Pre_Recargos.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Recargo_ID + "', '" + Recargo.P_Identificador + "'";
                Mi_SQL = Mi_SQL + ",'" + Recargo.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + ",'" + Recargo.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Recargo.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", SYSDATE";
                Mi_SQL = Mi_SQL + ")";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                if (Recargo.P_Recargos_Tasas != null && Recargo.P_Recargos_Tasas.Rows.Count > 0){
                    String Recargo_Tasa_ID = Obtener_ID_Consecutivo(Cat_Pre_Recargos_Tasas.Tabla_Cat_Pre_Recargos_Tasas, Cat_Pre_Recargos_Tasas.Campo_Recargo_Tasa_ID, 5);
                    for (int cnt = 0; cnt < Recargo.P_Recargos_Tasas.Rows.Count; cnt++){
                        Mi_SQL = "INSERT INTO " + Cat_Pre_Recargos_Tasas.Tabla_Cat_Pre_Recargos_Tasas;
                        Mi_SQL = Mi_SQL + " (" + Cat_Pre_Recargos_Tasas.Campo_Recargo_Tasa_ID + ", " + Cat_Pre_Recargos_Tasas.Campo_Recargo_ID;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_No_Bimestro;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Enero + ", " + Cat_Pre_Recargos_Tasas.Campo_Febrero;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Marzo + ", " + Cat_Pre_Recargos_Tasas.Campo_Abril;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Mayo + ", " + Cat_Pre_Recargos_Tasas.Campo_Junio;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Julio + ", " + Cat_Pre_Recargos_Tasas.Campo_Agosto;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Septiembre + ", " + Cat_Pre_Recargos_Tasas.Campo_Octubre;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Noviembre + ", " + Cat_Pre_Recargos_Tasas.Campo_Diciembre;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Usuario_Creo + ", " + Cat_Pre_Recargos_Tasas.Campo_Fecha_Creo + ")";
                        Mi_SQL = Mi_SQL + " VALUES ('" + Recargo_Tasa_ID + "', '" + Recargo_ID + "', " + Recargo.P_Recargos_Tasas.Rows[cnt][1].ToString();
                        Mi_SQL = Mi_SQL + ", " + Recargo.P_Recargos_Tasas.Rows[cnt][2].ToString() + ", " + Recargo.P_Recargos_Tasas.Rows[cnt][3].ToString();
                        Mi_SQL = Mi_SQL + ", " + Recargo.P_Recargos_Tasas.Rows[cnt][4].ToString() + ", " + Recargo.P_Recargos_Tasas.Rows[cnt][5].ToString();
                        Mi_SQL = Mi_SQL + ", " + Recargo.P_Recargos_Tasas.Rows[cnt][6].ToString() + ", " + Recargo.P_Recargos_Tasas.Rows[cnt][7].ToString();
                        Mi_SQL = Mi_SQL + ", " + Recargo.P_Recargos_Tasas.Rows[cnt][8].ToString() + ", " + Recargo.P_Recargos_Tasas.Rows[cnt][9].ToString();
                        Mi_SQL = Mi_SQL + ", " + Recargo.P_Recargos_Tasas.Rows[cnt][10].ToString() + ", " + Recargo.P_Recargos_Tasas.Rows[cnt][11].ToString();
                        Mi_SQL = Mi_SQL + ", " + Recargo.P_Recargos_Tasas.Rows[cnt][12].ToString() + ", " + Recargo.P_Recargos_Tasas.Rows[cnt][13].ToString();
                        Mi_SQL = Mi_SQL + ",'" + Recargo.P_Usuario + "', SYSDATE";
                        Mi_SQL = Mi_SQL + ")";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        Recargo_Tasa_ID = Convertir_A_Formato_ID(Convert.ToInt32(Recargo_Tasa_ID) + 1, 5);
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Recargo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }finally {
                 Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Recargo
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Recargo
        ///PARAMETROS:     
        ///             1. REcargo.  Instancia de la Clase de Negocio de Recargos 
        ///                          con los datos del Recargo que va a ser Actualizada.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 07/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Recargo(Cls_Cat_Pre_Recargos_Negocio Recargo){
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
                Cls_Cat_Pre_Recargos_Negocio Recargo_Tmp = Consultar_Datos_Recargo(Recargo);
                String Mi_SQL = "UPDATE " + Cat_Pre_Recargos.Tabla_Cat_Pre_Recargos + " SET " + Cat_Pre_Recargos.Campo_Identificador + " = '" + Recargo.P_Identificador + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Recargos.Campo_Estatus + " = '" + Recargo.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Recargos.Campo_Descripcion + " = '" + Recargo.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Recargos.Campo_Usuario_Modifico + " = '" + Recargo.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Recargos.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Recargos.Campo_Recargo_ID + " = '" + Recargo.P_Recargo_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Recargo_Tmp = Obtener_Recargos_Tasas_Eliminados(Recargo_Tmp, Recargo);
                for (int cnt = 0; cnt < Recargo_Tmp.P_Recargos_Tasas.Rows.Count; cnt++) {
                    Mi_SQL = "DELETE FROM " + Cat_Pre_Recargos_Tasas.Tabla_Cat_Pre_Recargos_Tasas;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Recargos_Tasas.Campo_Recargo_Tasa_ID + " = '" + Recargo_Tmp.P_Recargos_Tasas.Rows[cnt][0].ToString() + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }
                if (Recargo.P_Recargos_Tasas != null && Recargo.P_Recargos_Tasas.Rows.Count > 0){
                    String Recargo_Tasa_ID = Obtener_ID_Consecutivo(Cat_Pre_Recargos_Tasas.Tabla_Cat_Pre_Recargos_Tasas, Cat_Pre_Recargos_Tasas.Campo_Recargo_Tasa_ID, 5);
                    for (int cnt = 0; cnt < Recargo.P_Recargos_Tasas.Rows.Count; cnt++){
                        if (Recargo.P_Recargos_Tasas.Rows[cnt][0].ToString().Trim().Equals("")){
                            Mi_SQL = "INSERT INTO " + Cat_Pre_Recargos_Tasas.Tabla_Cat_Pre_Recargos_Tasas;
                            Mi_SQL = Mi_SQL + " (" + Cat_Pre_Recargos_Tasas.Campo_Recargo_Tasa_ID + ", " + Cat_Pre_Recargos_Tasas.Campo_Recargo_ID;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_No_Bimestro;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Enero + ", " + Cat_Pre_Recargos_Tasas.Campo_Febrero;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Marzo + ", " + Cat_Pre_Recargos_Tasas.Campo_Abril;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Mayo + ", " + Cat_Pre_Recargos_Tasas.Campo_Junio;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Julio + ", " + Cat_Pre_Recargos_Tasas.Campo_Agosto;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Septiembre + ", " + Cat_Pre_Recargos_Tasas.Campo_Octubre;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Noviembre + ", " + Cat_Pre_Recargos_Tasas.Campo_Diciembre;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Usuario_Creo + ", " + Cat_Pre_Recargos_Tasas.Campo_Fecha_Creo + ")";
                            Mi_SQL = Mi_SQL + " VALUES ('" + Recargo_Tasa_ID + "', '" + Recargo.P_Recargo_ID + "', " + Recargo.P_Recargos_Tasas.Rows[cnt][1].ToString();
                            Mi_SQL = Mi_SQL + ", " + Recargo.P_Recargos_Tasas.Rows[cnt][2].ToString() + ", " + Recargo.P_Recargos_Tasas.Rows[cnt][3].ToString();
                            Mi_SQL = Mi_SQL + ", " + Recargo.P_Recargos_Tasas.Rows[cnt][4].ToString() + ", " + Recargo.P_Recargos_Tasas.Rows[cnt][5].ToString();
                            Mi_SQL = Mi_SQL + ", " + Recargo.P_Recargos_Tasas.Rows[cnt][6].ToString() + ", " + Recargo.P_Recargos_Tasas.Rows[cnt][7].ToString();
                            Mi_SQL = Mi_SQL + ", " + Recargo.P_Recargos_Tasas.Rows[cnt][8].ToString() + ", " + Recargo.P_Recargos_Tasas.Rows[cnt][9].ToString();
                            Mi_SQL = Mi_SQL + ", " + Recargo.P_Recargos_Tasas.Rows[cnt][10].ToString() + ", " + Recargo.P_Recargos_Tasas.Rows[cnt][11].ToString();
                            Mi_SQL = Mi_SQL + ", " + Recargo.P_Recargos_Tasas.Rows[cnt][12].ToString() + ", " + Recargo.P_Recargos_Tasas.Rows[cnt][13].ToString();
                            Mi_SQL = Mi_SQL + ",'" + Recargo.P_Usuario + "', SYSDATE";
                            Mi_SQL = Mi_SQL + ")";
                            Recargo_Tasa_ID = Convertir_A_Formato_ID(Convert.ToInt32(Recargo_Tasa_ID) + 1, 5);
                        } else {
                            Mi_SQL = "UPDATE " + Cat_Pre_Recargos_Tasas.Tabla_Cat_Pre_Recargos_Tasas + " SET " + Cat_Pre_Recargos_Tasas.Campo_No_Bimestro + " = " + Recargo.P_Recargos_Tasas.Rows[cnt][1].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Enero + " = " + Recargo.P_Recargos_Tasas.Rows[cnt][2].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Febrero + " = " + Recargo.P_Recargos_Tasas.Rows[cnt][3].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Marzo + " = " + Recargo.P_Recargos_Tasas.Rows[cnt][4].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Abril + " = " + Recargo.P_Recargos_Tasas.Rows[cnt][5].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Mayo + " = " + Recargo.P_Recargos_Tasas.Rows[cnt][6].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Junio + " = " + Recargo.P_Recargos_Tasas.Rows[cnt][7].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Julio + " = " + Recargo.P_Recargos_Tasas.Rows[cnt][8].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Agosto + " = " + Recargo.P_Recargos_Tasas.Rows[cnt][9].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Septiembre + " = " + Recargo.P_Recargos_Tasas.Rows[cnt][10].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Octubre + " = " + Recargo.P_Recargos_Tasas.Rows[cnt][11].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Noviembre + " = " + Recargo.P_Recargos_Tasas.Rows[cnt][12].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Diciembre + " = " + Recargo.P_Recargos_Tasas.Rows[cnt][13].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Usuario_Modifico + " = '" + Recargo.P_Usuario + "'";
                            Mi_SQL = Mi_SQL + "," + Cat_Pre_Recargos_Tasas.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Recargos_Tasas.Campo_Recargo_Tasa_ID + " = '" + Recargo.P_Recargos_Tasas.Rows[cnt][0].ToString().Trim() + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Recargo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Recargos_Tasas_Eliminados
        ///DESCRIPCIÓN: Obtiene la lista de los Recargos Tasas que fueron dados de alta en la Actualizacion de las
        ///             Recargos
        ///PARAMETROS:     
        ///             1. Actuales.        Recargos que se usa para saber los Recargos_Tasas que estan en 
        ///                                 la Base de Datos antes de la Modificacion.
        ///             2. Actualizados.    Recargos que se usa para saber los Recargos_Tasas que estaran en 
        ///                                 la Base de Datos despues de la Modificacion.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 07/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static Cls_Cat_Pre_Recargos_Negocio Obtener_Recargos_Tasas_Eliminados(Cls_Cat_Pre_Recargos_Negocio Actuales, Cls_Cat_Pre_Recargos_Negocio Actualizados){
            Cls_Cat_Pre_Recargos_Negocio Recargo = new Cls_Cat_Pre_Recargos_Negocio();
            DataTable tabla = new DataTable();
            tabla.Columns.Add("RECARGO_TASA_ID", Type.GetType("System.String"));
            tabla.Columns.Add("NO_BIMESTRO", Type.GetType("System.String"));
            for (int cnt1 = 0; cnt1 < Actuales.P_Recargos_Tasas.Rows.Count; cnt1++) {
                bool eliminar = true;
                for (int cnt2 = 0; cnt2 < Actualizados.P_Recargos_Tasas.Rows.Count; cnt2++){
                    if (!Actualizados.P_Recargos_Tasas.Rows[cnt2][0].ToString().Equals("")){
                        if (Actuales.P_Recargos_Tasas.Rows[cnt1][0].ToString().Equals(Actualizados.P_Recargos_Tasas.Rows[cnt2][0].ToString())){
                            eliminar = false;
                            break;
                        }
                    }
                }
                if (eliminar) {
                    DataRow fila = tabla.NewRow();
                    fila["RECARGO_TASA_ID"] = Actuales.P_Recargos_Tasas.Rows[cnt1][0].ToString();
                    fila["NO_BIMESTRO"] = Actuales.P_Recargos_Tasas.Rows[cnt1][1].ToString();
                    tabla.Rows.Add(fila);
                }
            }
            Recargo.P_Recargos_Tasas = tabla;
            return Recargo;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Recargos
        ///DESCRIPCIÓN: Obtiene todos los Recargos que estan dadas de alta en la Base de Datos
        ///PARAMETROS:   
        ///             1.  Recargo.   Parametro de donde se sacara si habra o no un filtro
        ///                             de busqueda, en este caso el filtro es el Identificador.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 07/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Recargos(Cls_Cat_Pre_Recargos_Negocio Recargo) {
            DataTable Tabla = new DataTable();
            try{
                String Mi_SQL = "SELECT " + Cat_Pre_Recargos.Campo_Recargo_ID + " AS RECARGO_ID, " + Cat_Pre_Recargos.Campo_Identificador + " AS IDENTIFICADOR";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Recargos.Campo_Estatus + " AS ESTATUS";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Recargos.Tabla_Cat_Pre_Recargos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Recargos.Campo_Identificador + " LIKE '%" + Recargo.P_Identificador + "%' ";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Recargos.Campo_Recargo_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null) {
                    Tabla = dataset.Tables[0];
                }
            }catch(Exception Ex){
                String Mensaje = "Error al intentar consultar los registros de Recargos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Recargo
        ///DESCRIPCIÓN: Obtiene a detalle un Recargo.
        ///PARAMETROS:   
        ///             1. P_Recargo.   Recargo que se va ver a Detalle.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 07/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Recargos_Negocio Consultar_Datos_Recargo(Cls_Cat_Pre_Recargos_Negocio P_Recargo){
            String Mi_SQL = "SELECT " + Cat_Pre_Recargos.Campo_Identificador + ", " + Cat_Pre_Recargos.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Descripcion;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Recargos.Tabla_Cat_Pre_Recargos;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Recargos.Campo_Recargo_ID + " = '" + P_Recargo.P_Recargo_ID + "'";
            Cls_Cat_Pre_Recargos_Negocio R_Recargo = new Cls_Cat_Pre_Recargos_Negocio();
            OracleDataReader Data_Reader;
            try{
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Recargo.P_Recargo_ID = P_Recargo.P_Recargo_ID;
                while (Data_Reader.Read()){
                    R_Recargo.P_Identificador = Data_Reader[Cat_Pre_Recargos.Campo_Identificador].ToString();
                    R_Recargo.P_Estatus = Data_Reader[Cat_Pre_Recargos.Campo_Estatus].ToString();
                    R_Recargo.P_Descripcion = Data_Reader[Cat_Pre_Recargos.Campo_Descripcion].ToString();
                }
                Data_Reader.Close();
                Mi_SQL = "SELECT " + Cat_Pre_Recargos_Tasas.Campo_Recargo_Tasa_ID + " AS RECARGO_TASA_ID, " + Cat_Pre_Recargos_Tasas.Campo_No_Bimestro + " AS NO_BIMESTRO";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Enero + ", " + Cat_Pre_Recargos_Tasas.Campo_Febrero;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Marzo + ", " + Cat_Pre_Recargos_Tasas.Campo_Abril;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Mayo + ", " + Cat_Pre_Recargos_Tasas.Campo_Junio;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Julio + ", " + Cat_Pre_Recargos_Tasas.Campo_Agosto;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Septiembre + ", " + Cat_Pre_Recargos_Tasas.Campo_Octubre;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Tasas.Campo_Noviembre + ", " + Cat_Pre_Recargos_Tasas.Campo_Diciembre;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Recargos_Tasas.Tabla_Cat_Pre_Recargos_Tasas;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Recargos_Tasas.Campo_Recargo_ID + " = '" + P_Recargo.P_Recargo_ID + "'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Recargos_Tasas.Campo_Recargo_Tasa_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset == null){
                    R_Recargo.P_Recargos_Tasas = new DataTable();
                }else{
                    R_Recargo.P_Recargos_Tasas = dataset.Tables[0];
                }
            }catch (Exception Ex){
                String Mensaje = "Error al intentar consultar el registro de Recargo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Recargo;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Recargo
        ///DESCRIPCIÓN: Elimina un Recargo
        ///PARAMETROS:   
        ///             1. Recargo.   Recargo que se va eliminar.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 07/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Recargo(Cls_Cat_Pre_Recargos_Negocio Recargo){
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
                String Mi_SQL = "DELETE FROM " + Cat_Pre_Recargos_Tasas.Tabla_Cat_Pre_Recargos_Tasas;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Recargos_Tasas.Campo_Recargo_ID + " = '" + Recargo.P_Recargo_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Mi_SQL = "DELETE FROM " + Cat_Pre_Recargos.Tabla_Cat_Pre_Recargos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Recargos.Campo_Recargo_ID + " = '" + Recargo.P_Recargo_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            } catch (OracleException Ex) {
                Trans.Rollback();
                if (Ex.Code == 547){
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]";
                } else {
                    Mensaje = "Error al intentar eliminar el registro de Recargo. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex) {
                Mensaje = "Error al intentar eliminar el registro de Recargo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
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
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
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