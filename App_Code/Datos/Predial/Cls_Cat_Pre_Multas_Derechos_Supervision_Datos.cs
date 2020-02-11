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
using Presidencia.Catalogo_Multas_Derechos_Supervision.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Multas__Derechos_Supervision_Datos
/// </summary>

namespace Presidencia.Catalogo_Multas_Derechos_Supervision.Datos{

    public class Cls_Cat_Pre_Multas_Derechos_Supervision_Datos {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Multa
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nueva Multa
        ///PARAMETROS:     
        ///             1. Multa.    Instancia de la Clase de Negocio de Multas con los datos 
        ///                          de la Multa que va a ser dada de Alta.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Multa(Cls_Cat_Pre_Multas_Derechos_Supervision_Negocio Multa){
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
                String Multa_ID = Obtener_ID_Consecutivo(Cat_Pre_Multas_Derechos_Supervision.Tabla_Cat_Pre_Multas_Derechos_Supervision, Cat_Pre_Multas_Derechos_Supervision.Campo_Multa_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Multas_Derechos_Supervision.Tabla_Cat_Pre_Multas_Derechos_Supervision;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Multas_Derechos_Supervision.Campo_Multa_ID + ", " + Cat_Pre_Multas_Derechos_Supervision.Campo_Identificador;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Multas_Derechos_Supervision.Campo_Desde_Anios + ", " + Cat_Pre_Multas_Derechos_Supervision.Campo_Hasta_Anios;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Multas_Derechos_Supervision.Campo_Descripcion + ", " + Cat_Pre_Multas_Derechos_Supervision.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Multas_Derechos_Supervision.Campo_Usuario_Creo + ", " + Cat_Pre_Multas_Derechos_Supervision.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Multa_ID + "', '" + Multa.P_Identificador + "'";
                Mi_SQL = Mi_SQL + ",'" + Multa.P_Desde + "'";
                Mi_SQL = Mi_SQL + ",'" + Multa.P_Hasta + "'";
                Mi_SQL = Mi_SQL + ",'" + Multa.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + ",'" + Multa.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Multa.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", SYSDATE";
                Mi_SQL = Mi_SQL + ")";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                if (Multa.P_Multas_Cuotas != null && Multa.P_Multas_Cuotas.Rows.Count > 0){
                    String Multa_Cuota_ID = Obtener_ID_Consecutivo(Cat_Pre_Multas_Derechos_Supervision_Detalles.Tabla_Cat_Pre_Multas_Derechos_Supervision_Detalles, Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Multa_Cuota_ID, 5);
                    for (int cnt = 0; cnt < Multa.P_Multas_Cuotas.Rows.Count; cnt++){
                        Mi_SQL = "INSERT INTO " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Tabla_Cat_Pre_Multas_Derechos_Supervision_Detalles;
                        Mi_SQL = Mi_SQL + " (" + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Multa_Cuota_ID + ", " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Multa_ID;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Año + ", " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Monto;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Usuario_Creo + ", " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Fecha_Creo + ")";
                        Mi_SQL = Mi_SQL + " VALUES ('" + Multa_Cuota_ID + "', '" + Multa_ID + "', " + Multa.P_Multas_Cuotas.Rows[cnt][1].ToString() + ", " + Multa.P_Multas_Cuotas.Rows[cnt][2].ToString().Replace(",","").Trim();
                        Mi_SQL = Mi_SQL + ",'" + Multa.P_Usuario + "', SYSDATE";
                        Mi_SQL = Mi_SQL + ")";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        Multa_Cuota_ID = Convertir_A_Formato_ID(Convert.ToInt32(Multa_Cuota_ID) + 1, 5);
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Multa de Derechos de Supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }finally {
                 Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Multa
        ///DESCRIPCIÓN: Actualiza en la Base de Datos una Multa
        ///PARAMETROS:     
        ///             1. Multa.   Instancia de la Clase de Negocio de Multas con 
        ///                         los datos de la Multa que va a ser Actualizada.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Multa(Cls_Cat_Pre_Multas_Derechos_Supervision_Negocio Multa){
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
                Cls_Cat_Pre_Multas_Derechos_Supervision_Negocio Multa_Tmp = Consultar_Datos_Multa(Multa);
                String Mi_SQL = "UPDATE " + Cat_Pre_Multas_Derechos_Supervision.Tabla_Cat_Pre_Multas_Derechos_Supervision + " SET " + Cat_Pre_Multas_Derechos_Supervision.Campo_Identificador + " = '" + Multa.P_Identificador + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Multas_Derechos_Supervision.Campo_Estatus + " = '" + Multa.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Multas_Derechos_Supervision.Campo_Descripcion + " = '" + Multa.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Multas_Derechos_Supervision.Campo_Desde_Anios + " = '" + Multa.P_Desde + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Multas_Derechos_Supervision.Campo_Hasta_Anios + " = '" + Multa.P_Hasta + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Multas_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Multa.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Multas_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Multas_Derechos_Supervision.Campo_Multa_ID + " = '" + Multa.P_Multa_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Multa_Tmp = Obtener_Multas_Cuotas_Eliminadas(Multa_Tmp, Multa);
                for (int cnt = 0; cnt < Multa_Tmp.P_Multas_Cuotas.Rows.Count; cnt++) {
                    Mi_SQL = "DELETE FROM " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Tabla_Cat_Pre_Multas_Derechos_Supervision_Detalles;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Multa_Cuota_ID + " = '" + Multa_Tmp.P_Multas_Cuotas.Rows[cnt][0].ToString() + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }
                if (Multa.P_Multas_Cuotas != null && Multa.P_Multas_Cuotas.Rows.Count > 0){
                    String Multa_Cuota_ID = Obtener_ID_Consecutivo(Cat_Pre_Multas_Derechos_Supervision_Detalles.Tabla_Cat_Pre_Multas_Derechos_Supervision_Detalles, Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Multa_Cuota_ID, 5);
                    for (int cnt = 0; cnt < Multa.P_Multas_Cuotas.Rows.Count; cnt++){
                        if (Multa.P_Multas_Cuotas.Rows[cnt][0].ToString().Trim().Equals("")){
                            Mi_SQL = "INSERT INTO " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Tabla_Cat_Pre_Multas_Derechos_Supervision_Detalles;
                            Mi_SQL = Mi_SQL + " (" + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Multa_Cuota_ID + ", " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Multa_ID;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Año + ", " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Monto;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Usuario_Creo + ", " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Fecha_Creo + ")";
                            Mi_SQL = Mi_SQL + " VALUES ('" + Multa_Cuota_ID + "', '" + Multa.P_Multa_ID + "', " + Multa.P_Multas_Cuotas.Rows[cnt][1].ToString() + ", " + Multa.P_Multas_Cuotas.Rows[cnt][4].ToString();
                            Mi_SQL = Mi_SQL + ",'" + Multa.P_Usuario + "', SYSDATE";
                            Mi_SQL = Mi_SQL + ")";
                            Multa_Cuota_ID = Convertir_A_Formato_ID(Convert.ToInt32(Multa_Cuota_ID) + 1, 5);
                        } else {
                            Mi_SQL = "UPDATE " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Tabla_Cat_Pre_Multas_Derechos_Supervision_Detalles + " SET " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Año + " = " + Multa.P_Multas_Cuotas.Rows[cnt][1].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Monto + " = " + Multa.P_Multas_Cuotas.Rows[cnt][2].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Usuario_Modifico + " = '" + Multa.P_Usuario + "'";
                            Mi_SQL = Mi_SQL + "," + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Multa_Cuota_ID + " = '" + Multa.P_Multas_Cuotas.Rows[cnt][0].ToString().Trim() + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Multa de Derechos de Supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Multas_Cuotas_Eliminadas
        ///DESCRIPCIÓN: Obtiene la lista de las Multas_Cuotas que fueron dados de baja en la Actualizacion de los
        ///             Multas
        ///PARAMETROS:     
        ///             1. Actuales.        Multas que se usa para saber los Multas_Cuotas que estan en 
        ///                                 la Base de Datos antes de la Modificacion.
        ///             2. Actualizados.    Multas que se usa para saber los Multas_Cuotas que estaran en 
        ///                                 la Base de Datos despues de la Modificacion.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static Cls_Cat_Pre_Multas_Derechos_Supervision_Negocio Obtener_Multas_Cuotas_Eliminadas(Cls_Cat_Pre_Multas_Derechos_Supervision_Negocio Actuales, Cls_Cat_Pre_Multas_Derechos_Supervision_Negocio Actualizados){
            Cls_Cat_Pre_Multas_Derechos_Supervision_Negocio Multa = new Cls_Cat_Pre_Multas_Derechos_Supervision_Negocio();
            DataTable tabla = new DataTable();
            tabla.Columns.Add("MULTA_CUOTA_ID", Type.GetType("System.String"));
            tabla.Columns.Add("ANIO", Type.GetType("System.String"));
            tabla.Columns.Add("MONTO", Type.GetType("System.String"));
            for (int cnt1 = 0; cnt1 < Actuales.P_Multas_Cuotas.Rows.Count; cnt1++) {
                bool eliminar = true;
                for (int cnt2 = 0; cnt2 < Actualizados.P_Multas_Cuotas.Rows.Count; cnt2++) {
                    if (!Actualizados.P_Multas_Cuotas.Rows[cnt2][0].ToString().Equals("")) {
                        if (Actuales.P_Multas_Cuotas.Rows[cnt1][0].ToString().Equals(Actualizados.P_Multas_Cuotas.Rows[cnt2][0].ToString())) {
                            eliminar = false;
                            break;
                        }
                    }
                }
                if (eliminar) {
                    DataRow fila = tabla.NewRow();
                    fila["MULTA_CUOTA_ID"] = Actuales.P_Multas_Cuotas.Rows[cnt1][0].ToString();
                    fila["ANIO"] = Actuales.P_Multas_Cuotas.Rows[cnt1][1].ToString();
                    fila["MONTO"] = Actuales.P_Multas_Cuotas.Rows[cnt1][2].ToString();  
                    tabla.Rows.Add(fila);
                }
            }
            Multa.P_Multas_Cuotas = tabla;
            return Multa;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Multas
        ///DESCRIPCIÓN: Obtiene todos las Multas que estan dados de alta en la Base de Datos
        ///PARAMETROS:      
        ///             1.  Multa.    Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                           caso el filtro es el Identificador.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Multas(Cls_Cat_Pre_Multas_Derechos_Supervision_Negocio Multa){
            DataTable Tabla = new DataTable();
            try{
                String Mi_SQL = "SELECT " + Cat_Pre_Multas_Derechos_Supervision.Campo_Multa_ID + " AS MULTA_ID, " + Cat_Pre_Multas_Derechos_Supervision.Campo_Identificador + " AS IDENTIFICADOR";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Multas_Derechos_Supervision.Campo_Estatus + " AS ESTATUS, ";
                Mi_SQL += Cat_Pre_Multas_Derechos_Supervision.Campo_Descripcion + ", " + Cat_Pre_Multas_Derechos_Supervision.Campo_Desde_Anios;
                Mi_SQL += ", " + Cat_Pre_Multas_Derechos_Supervision.Campo_Hasta_Anios;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Multas_Derechos_Supervision.Tabla_Cat_Pre_Multas_Derechos_Supervision;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Multas_Derechos_Supervision.Campo_Identificador + " LIKE '%" + Multa.P_Identificador + "%' ";
                Mi_SQL = Mi_SQL + " OR " + Cat_Pre_Multas_Derechos_Supervision.Campo_Descripcion + " LIKE '%" + Multa.P_Identificador + "%' ";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Multas_Derechos_Supervision.Campo_Multa_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null) {
                    Tabla = dataset.Tables[0];
                }
            }catch(Exception Ex){
                String Mensaje = "Error al intentar consultar los registros de Multas de Derechos de Supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Multa
        ///DESCRIPCIÓN: Obtiene a detalle una Multa.
        ///PARAMETROS:   
        ///             1. P_Multa.   Multa que se va ver a Detalle.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Multas_Derechos_Supervision_Negocio Consultar_Datos_Multa(Cls_Cat_Pre_Multas_Derechos_Supervision_Negocio P_Multa)
        {
            String Mi_SQL = "SELECT " + Cat_Pre_Multas_Derechos_Supervision.Campo_Identificador + ", " + Cat_Pre_Multas_Derechos_Supervision.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Multas_Derechos_Supervision.Campo_Desde_Anios;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Multas_Derechos_Supervision.Campo_Hasta_Anios;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Multas_Derechos_Supervision.Campo_Descripcion;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Multas_Derechos_Supervision.Tabla_Cat_Pre_Multas_Derechos_Supervision;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Multas_Derechos_Supervision.Campo_Multa_ID + " = '" + P_Multa.P_Multa_ID + "'";
            Cls_Cat_Pre_Multas_Derechos_Supervision_Negocio R_Multa = new Cls_Cat_Pre_Multas_Derechos_Supervision_Negocio();
            OracleDataReader Data_Reader;
            try{
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Multa.P_Multa_ID = P_Multa.P_Multa_ID;
                while (Data_Reader.Read()){
                    R_Multa.P_Identificador = Data_Reader[Cat_Pre_Multas_Derechos_Supervision.Campo_Identificador].ToString();
                    R_Multa.P_Estatus = Data_Reader[Cat_Pre_Multas_Derechos_Supervision.Campo_Estatus].ToString();
                    R_Multa.P_Desde = Data_Reader[Cat_Pre_Multas_Derechos_Supervision.Campo_Desde_Anios].ToString();
                    R_Multa.P_Hasta = Data_Reader[Cat_Pre_Multas_Derechos_Supervision.Campo_Hasta_Anios].ToString();
                    R_Multa.P_Descripcion = Data_Reader[Cat_Pre_Multas_Derechos_Supervision.Campo_Descripcion].ToString();
                }
                Data_Reader.Close();
                Mi_SQL = "SELECT " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Multa_Cuota_ID + " AS MULTA_CUOTA_ID, " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Año + " AS ANIO";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Monto + " AS DESDE";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Monto + " AS HASTA";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Monto + " AS MONTO";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Tabla_Cat_Pre_Multas_Derechos_Supervision_Detalles;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Multa_ID + " = '" + P_Multa.P_Multa_ID + "' ";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Multa_Cuota_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset == null){
                    R_Multa.P_Multas_Cuotas = new DataTable();
                }else{
                    R_Multa.P_Multas_Cuotas = dataset.Tables[0];
                }
            }catch (Exception Ex){
                String Mensaje = "Error al intentar consultar el registro de Multa de Derechos de Supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Multa;
        }

        /////*******************************************************************************
        /////NOMBRE DE LA FUNCIÓN   : Consultar_Cuotas_Multas
        /////DESCRIPCIÓN            : Obtiene los montos de las Multas
        /////PARAMETROS             : Multa, instancia de Cls_Cat_Pre_Multas_Negocio
        /////CREO                   : Antonio Salvador Benavides Guardado
        /////FECHA_CREO             : 02/Diciembre/2010 
        /////MODIFICO:
        /////FECHA_MODIFICO
        /////CAUSA_MODIFICACIÓN
        /////*******************************************************************************
        public static DataTable Consultar_Cuotas_Multas(Cls_Cat_Pre_Multas_Derechos_Supervision_Negocio Multa)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;
            String Mi_SQL_Campos_Foraneos = "";

            try
            {
                if (Multa.P_Incluir_Campos_Foraneos)
                {
                    Mi_SQL_Campos_Foraneos = Mi_SQL_Campos_Foraneos + "(SELECT " + Cat_Pre_Multas_Derechos_Supervision.Campo_Identificador + " FROM " + Cat_Pre_Multas_Derechos_Supervision.Tabla_Cat_Pre_Multas_Derechos_Supervision + " WHERE " + Cat_Pre_Multas_Derechos_Supervision.Campo_Multa_ID + " = " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Tabla_Cat_Pre_Multas_Derechos_Supervision_Detalles + "." + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Multa_ID;
                    if (Multa.P_Identificador != null && Multa.P_Identificador != "")
                    {
                        Mi_SQL_Campos_Foraneos = Mi_SQL_Campos_Foraneos + " AND " + Cat_Pre_Multas_Derechos_Supervision.Campo_Identificador + " LIKE '%" + Multa.P_Identificador + "%'";
                    }
                    Mi_SQL_Campos_Foraneos = Mi_SQL_Campos_Foraneos + ") AS IDENTIFICADOR, ";
                    Mi_SQL_Campos_Foraneos = Mi_SQL_Campos_Foraneos + "(SELECT " + Cat_Pre_Multas_Derechos_Supervision.Campo_Desde_Anios + " FROM " + Cat_Pre_Multas_Derechos_Supervision.Tabla_Cat_Pre_Multas_Derechos_Supervision + " WHERE " + Cat_Pre_Multas_Derechos_Supervision.Campo_Multa_ID + " = " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Tabla_Cat_Pre_Multas_Derechos_Supervision_Detalles + "." + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Multa_ID + ") AS DESDE_ANIOS, ";
                    Mi_SQL_Campos_Foraneos = Mi_SQL_Campos_Foraneos + "(SELECT " + Cat_Pre_Multas_Derechos_Supervision.Campo_Hasta_Anios + " FROM " + Cat_Pre_Multas_Derechos_Supervision.Tabla_Cat_Pre_Multas_Derechos_Supervision + " WHERE " + Cat_Pre_Multas_Derechos_Supervision.Campo_Multa_ID + " = " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Tabla_Cat_Pre_Multas_Derechos_Supervision_Detalles + "." + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Multa_ID + ") AS HASTA_ANIOS, ";
                }
                if (Multa.P_Campos_Dinamicos != null && Multa.P_Campos_Dinamicos != "")
                {
                    Mi_SQL = "SELECT " + Mi_SQL_Campos_Foraneos + Multa.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL = "SELECT " + Mi_SQL_Campos_Foraneos;
                    Mi_SQL = Mi_SQL + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Multa_Cuota_ID + " AS MULTA_CUOTA_ID, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Multa_ID + " AS MULTA_ID, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Año + " AS ANIO, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Monto + " AS MONTO";
                }
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Tabla_Cat_Pre_Multas_Derechos_Supervision_Detalles;
                if (Multa.P_Filtros_Dinamicos != null && Multa.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Multa.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Multa_ID + " IN (SELECT " + Cat_Pre_Multas_Derechos_Supervision.Campo_Multa_ID + " FROM " + Cat_Pre_Multas_Derechos_Supervision.Tabla_Cat_Pre_Multas_Derechos_Supervision + " WHERE " + Cat_Pre_Multas_Derechos_Supervision.Campo_Estatus + " = 'VIGENTE')";
                    //DESCOMENTAR EL SIGUIENTE BLOQUE IF SI SE AGREGAN FILTROS EN ESTA SECCIÓN
                    //if (Mi_SQL.EndsWith(" AND "))
                    //{
                    //    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                    //}
                    //DESCOMENTAR EL SIGUIENTE BLOQUE WHERE SI SE QUITA EL CAMPO CONCEPTO_PREDIAL_ID DE LA LÍNEA DEL WHERE
                    //if (Mi_SQL.EndsWith(" WHERE "))
                    //{
                    //    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                    //}
                }
                if (Multa.P_Agrupar_Dinamico != null && Multa.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL = Mi_SQL + " GROUP BY " + Multa.P_Agrupar_Dinamico;
                }
                if (Multa.P_Ordenar_Dinamico != null && Multa.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL = Mi_SQL + " ORDER BY " + Multa.P_Ordenar_Dinamico;
                }
                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null)
                {
                    Tabla = dataSet.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los Montos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Multa
        ///DESCRIPCIÓN: Elimina una Multa
        ///PARAMETROS:   
        ///             1. Multa.   Multa que se va eliminar.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Multa(Cls_Cat_Pre_Multas_Derechos_Supervision_Negocio Multa)
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
            try{
                String Mi_SQL = "DELETE FROM " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Tabla_Cat_Pre_Multas_Derechos_Supervision_Detalles;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Multa_ID + " = '" + Multa.P_Multa_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Mi_SQL = "DELETE FROM " + Cat_Pre_Multas_Derechos_Supervision.Tabla_Cat_Pre_Multas_Derechos_Supervision;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Multas_Derechos_Supervision.Campo_Multa_ID + " = '" + Multa.P_Multa_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            } catch (OracleException Ex) {
                if (Ex.Code == 547){
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]"; 
                } else {
                    Mensaje = "Error al intentar eliminar el registro de Multa. Error: [" + Ex.Message + "]"; 
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex) {
                Mensaje = "Error al intentar eliminar el registro de Multa de Derechos de Supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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

    }
}