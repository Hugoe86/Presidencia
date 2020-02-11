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
using Presidencia.Catalogo_Fraccionamientos.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Fraccionamientos
/// </summary>

namespace Presidencia.Catalogo_Fraccionamientos.Datos{

    public class Cls_Cat_Pre_Fraccionamientos_Datos {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Fraccionamiento
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo Fraccionamiento
        ///PARAMETROS:     
        ///             1. Fraccionamientos.    Instancia de la Clase de Negocio de Fraccionamientos 
        ///                                     con los datos del Fraccionamiento que va a ser dado de Alta.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 20/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Fraccionamiento(Cls_Cat_Pre_Fraccionamientos_Negocio Fraccionamiento) {
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
                String Fraccionamiento_ID = Obtener_ID_Consecutivo(Cat_Pre_Fraccionamientos.Tabla_Cat_Pre_Fraccionamientos, Cat_Pre_Fraccionamientos.Campo_Fraccionamiento_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Fraccionamientos.Tabla_Cat_Pre_Fraccionamientos;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Fraccionamientos.Campo_Fraccionamiento_ID + ", " + Cat_Pre_Fraccionamientos.Campo_Identificador;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Fraccionamientos.Campo_Descripcion + ", " + Cat_Pre_Fraccionamientos.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Fraccionamientos.Campo_Usuario_Creo + ", " + Cat_Pre_Fraccionamientos.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Fraccionamiento_ID + "', '" + Fraccionamiento.P_Identificador + "'";
                Mi_SQL = Mi_SQL + ",'" + Fraccionamiento.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + ",'" + Fraccionamiento.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Fraccionamiento.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", SYSDATE";
                Mi_SQL = Mi_SQL + ")";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                if (Fraccionamiento.P_Fraccionamientos_Impuestos != null && Fraccionamiento.P_Fraccionamientos_Impuestos.Rows.Count > 0){
                    String Fraccionamiento_Impuesto_ID = Obtener_ID_Consecutivo(Cat_Pre_Fracc_Impuestos.Tabla_Cat_Pre_Fracc_Impuestos, Cat_Pre_Fracc_Impuestos.Campo_Impuesto_Fraccionamiento_ID, 5);
                    for (int cnt = 0; cnt < Fraccionamiento.P_Fraccionamientos_Impuestos.Rows.Count; cnt++){
                        Mi_SQL = "INSERT INTO " + Cat_Pre_Fracc_Impuestos.Tabla_Cat_Pre_Fracc_Impuestos;
                        Mi_SQL = Mi_SQL + " (" + Cat_Pre_Fracc_Impuestos.Campo_Impuesto_Fraccionamiento_ID + ", " + Cat_Pre_Fracc_Impuestos.Campo_Fraccionamiento_ID;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Fracc_Impuestos.Campo_Año + ", " + Cat_Pre_Fracc_Impuestos.Campo_Monto;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Fracc_Impuestos.Campo_Usuario_Creo + ", " + Cat_Pre_Fracc_Impuestos.Campo_Fecha_Creo + ")";
                        Mi_SQL = Mi_SQL + " VALUES ('" + Fraccionamiento_Impuesto_ID + "', '" + Fraccionamiento_ID + "', " + Fraccionamiento.P_Fraccionamientos_Impuestos.Rows[cnt][1].ToString() + ", " + Fraccionamiento.P_Fraccionamientos_Impuestos.Rows[cnt][2].ToString();
                        Mi_SQL = Mi_SQL + ",'" + Fraccionamiento.P_Usuario + "', SYSDATE";
                        Mi_SQL = Mi_SQL + ")";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        Fraccionamiento_Impuesto_ID = Convertir_A_Formato_ID(Convert.ToInt32(Fraccionamiento_Impuesto_ID) + 1, 5);
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Fraccionamientos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }finally {
                 Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Fraccionamiento
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Fraccionamiento
        ///PARAMETROS:     
        ///             1. Fraccionamientos.    Instancia de la Clase de Negocio de Fraccionamientos 
        ///                                     con los datos del Fraccionamiento que va a ser Actualizado.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Fraccionamiento(Cls_Cat_Pre_Fraccionamientos_Negocio Fraccionamiento){
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
                Cls_Cat_Pre_Fraccionamientos_Negocio Fraccionamiento_Tmp = Consultar_Datos_Fraccionamiento(Fraccionamiento);
                String Mi_SQL = "UPDATE " + Cat_Pre_Fraccionamientos.Tabla_Cat_Pre_Fraccionamientos + " SET " + Cat_Pre_Fraccionamientos.Campo_Identificador + " = '" + Fraccionamiento.P_Identificador + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Fraccionamientos.Campo_Estatus + " = '" + Fraccionamiento.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Fraccionamientos.Campo_Descripcion + " = '" + Fraccionamiento.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Fraccionamiento.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Fraccionamientos.Campo_Fraccionamiento_ID + " = '" + Fraccionamiento.P_Fraccionamiento_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Fraccionamiento_Tmp = Obtener_Fraccionamientos_Impuestos_Eliminados(Fraccionamiento_Tmp, Fraccionamiento);
                for (int cnt = 0; cnt < Fraccionamiento_Tmp.P_Fraccionamientos_Impuestos.Rows.Count; cnt++) {
                    Mi_SQL = "DELETE FROM " + Cat_Pre_Fracc_Impuestos.Tabla_Cat_Pre_Fracc_Impuestos;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Fracc_Impuestos.Campo_Impuesto_Fraccionamiento_ID + " = '" + Fraccionamiento_Tmp.P_Fraccionamientos_Impuestos.Rows[cnt][0].ToString() + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }
                if (Fraccionamiento.P_Fraccionamientos_Impuestos != null && Fraccionamiento.P_Fraccionamientos_Impuestos.Rows.Count > 0){
                    String Fraccionamiento_Impuesto_ID = Obtener_ID_Consecutivo(Cat_Pre_Fracc_Impuestos.Tabla_Cat_Pre_Fracc_Impuestos, Cat_Pre_Fracc_Impuestos.Campo_Impuesto_Fraccionamiento_ID, 5);
                    for (int cnt = 0; cnt < Fraccionamiento.P_Fraccionamientos_Impuestos.Rows.Count; cnt++){
                        if (Fraccionamiento.P_Fraccionamientos_Impuestos.Rows[cnt][0].ToString().Trim().Equals("")){
                            Mi_SQL = "INSERT INTO " + Cat_Pre_Fracc_Impuestos.Tabla_Cat_Pre_Fracc_Impuestos;
                            Mi_SQL = Mi_SQL + " (" + Cat_Pre_Fracc_Impuestos.Campo_Impuesto_Fraccionamiento_ID + ", " + Cat_Pre_Fracc_Impuestos.Campo_Fraccionamiento_ID;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Fracc_Impuestos.Campo_Año + ", " + Cat_Pre_Fracc_Impuestos.Campo_Monto;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Fracc_Impuestos.Campo_Usuario_Creo + ", " + Cat_Pre_Fracc_Impuestos.Campo_Fecha_Creo + ")";
                            Mi_SQL = Mi_SQL + " VALUES ('" + Fraccionamiento_Impuesto_ID + "', '" + Fraccionamiento.P_Fraccionamiento_ID + "', " + Fraccionamiento.P_Fraccionamientos_Impuestos.Rows[cnt][2].ToString() + ", " + Fraccionamiento.P_Fraccionamientos_Impuestos.Rows[cnt][3].ToString();
                            Mi_SQL = Mi_SQL + ",'" + Fraccionamiento.P_Usuario + "', SYSDATE";
                            Mi_SQL = Mi_SQL + ")";
                            Fraccionamiento_Impuesto_ID = Convertir_A_Formato_ID(Convert.ToInt32(Fraccionamiento_Impuesto_ID) + 1, 5);
                        } else {
                            Mi_SQL = "UPDATE " + Cat_Pre_Fracc_Impuestos.Tabla_Cat_Pre_Fracc_Impuestos + " SET " + Cat_Pre_Fracc_Impuestos.Campo_Año + " = " + Fraccionamiento.P_Fraccionamientos_Impuestos.Rows[cnt][2].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Fracc_Impuestos.Campo_Monto + " = " + Fraccionamiento.P_Fraccionamientos_Impuestos.Rows[cnt][3].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Fracc_Impuestos.Campo_Usuario_Modifico + " = '" + Fraccionamiento.P_Usuario + "'";
                            Mi_SQL = Mi_SQL + "," + Cat_Pre_Fracc_Impuestos.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Fracc_Impuestos.Campo_Impuesto_Fraccionamiento_ID + " = '" + Fraccionamiento.P_Fraccionamientos_Impuestos.Rows[cnt][0].ToString().Trim() +"'";
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
                    Mensaje = "Error al intentar modificar un Registro de Fraccionamientos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Fraccionamientos_Impuestos_Eliminados
        ///DESCRIPCIÓN: Obtiene la lista de los Fraccionamientos Impuestos que fueron dados de alta en la Actualizacion de los
        ///             Fraccionamientos
        ///PARAMETROS:     
        ///             1. Actuales.        Fraccionamientos que se usa para saber los Fraccionamientos_Impuestos que estan en 
        ///                                 la Base de Datos antes de la Modificacion.
        ///             2. Actualizados.    Fraccionamientos que se usa para saber los Fraccionamientos_Impuestos que estaran en 
        ///                                 la Base de Datos despues de la Modificacion.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static Cls_Cat_Pre_Fraccionamientos_Negocio Obtener_Fraccionamientos_Impuestos_Eliminados( Cls_Cat_Pre_Fraccionamientos_Negocio Actuales, Cls_Cat_Pre_Fraccionamientos_Negocio Actualizados ) {
            Cls_Cat_Pre_Fraccionamientos_Negocio Fraccionamiento = new Cls_Cat_Pre_Fraccionamientos_Negocio();
            DataTable tabla = new DataTable();
            tabla.Columns.Add("IMPUESTO_FRACCIONAMIENTO_ID", Type.GetType("System.String"));
            tabla.Columns.Add("ANIO", Type.GetType("System.String"));
            tabla.Columns.Add("MONTO", Type.GetType("System.String"));
            for (int cnt1 = 0; cnt1 < Actuales.P_Fraccionamientos_Impuestos.Rows.Count; cnt1++) {
                bool eliminar = true;
                for (int cnt2 = 0; cnt2 < Actualizados.P_Fraccionamientos_Impuestos.Rows.Count; cnt2++) {
                    if (!Actualizados.P_Fraccionamientos_Impuestos.Rows[cnt2][0].ToString().Equals("")) {
                        if (Actuales.P_Fraccionamientos_Impuestos.Rows[cnt1][0].ToString().Equals(Actualizados.P_Fraccionamientos_Impuestos.Rows[cnt2][0].ToString())) {
                            eliminar = false;
                            break;
                        }
                    }
                }
                if (eliminar) {
                    DataRow fila = tabla.NewRow();
                    fila["IMPUESTO_FRACCIONAMIENTO_ID"] = Actuales.P_Fraccionamientos_Impuestos.Rows[cnt1][0].ToString();
                    fila["ANIO"] = Actuales.P_Fraccionamientos_Impuestos.Rows[cnt1][1].ToString();
                    fila["MONTO"] = Actuales.P_Fraccionamientos_Impuestos.Rows[cnt1][2].ToString();
                    tabla.Rows.Add(fila);
                }
            }
            Fraccionamiento.P_Fraccionamientos_Impuestos = tabla;
            return Fraccionamiento;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Fraccionamientos
        ///DESCRIPCIÓN: Obtiene todos los fraccionamientos que estan dados de alta en la Base de Datos
        ///PARAMETROS:      
        ///             1.  Fraccionamiento.    Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                                     caso el filtro es el Identificador.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 20/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Fraccionamientos(Cls_Cat_Pre_Fraccionamientos_Negocio Fraccionamiento) {
            DataTable Tabla = new DataTable();
            try {
                String Mi_SQL = "SELECT " + Cat_Pre_Fraccionamientos.Campo_Fraccionamiento_ID + " AS FRACCIONAMIENTO_ID, " + Cat_Pre_Fraccionamientos.Campo_Identificador + " AS IDENTIFICADOR";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Fraccionamientos.Campo_Descripcion + " AS DESCRIPCION";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Fraccionamientos.Campo_Estatus + " AS ESTATUS";
                Mi_SQL = Mi_SQL +  " FROM " + Cat_Pre_Fraccionamientos.Tabla_Cat_Pre_Fraccionamientos;
                Mi_SQL = Mi_SQL + " WHERE ";
                if (Fraccionamiento.P_Identificador != null && Fraccionamiento.P_Identificador != "")
                {
                    Mi_SQL = Mi_SQL + Cat_Pre_Fraccionamientos.Campo_Identificador + " LIKE '%" + Fraccionamiento.P_Identificador + "%' AND ";
                }
                if (Fraccionamiento.P_Fraccionamiento_ID != null && Fraccionamiento.P_Fraccionamiento_ID != "")
                {
                    Mi_SQL = Mi_SQL + Cat_Pre_Fraccionamientos.Campo_Fraccionamiento_ID + Validar_Operador_Comparacion(Fraccionamiento.P_Fraccionamiento_ID) + " AND ";
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
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Fraccionamientos.Campo_Fraccionamiento_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null) {
                    Tabla = dataset.Tables[0];
                }
            }catch(Exception Ex){
                String Mensaje = "Error al intentar consultar los registros de Fraccionamientos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Fraccionamiento
        ///DESCRIPCIÓN: Obtiene a detalle un Fraccionamiento.
        ///PARAMETROS:   
        ///             1. P_Fraccionamiento.   Fraccionamiento que se va ver a Detalle.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Fraccionamientos_Negocio Consultar_Datos_Fraccionamiento(Cls_Cat_Pre_Fraccionamientos_Negocio P_Fraccionamiento){
            String Mi_SQL = "SELECT " + Cat_Pre_Fraccionamientos.Campo_Identificador + ", " + Cat_Pre_Fraccionamientos.Campo_Estatus + ", " + Cat_Pre_Fraccionamientos.Campo_Descripcion;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Fraccionamientos.Tabla_Cat_Pre_Fraccionamientos;
            if (P_Fraccionamiento.P_Fraccionamiento_ID != null && P_Fraccionamiento.P_Fraccionamiento_ID != "")
            {
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Fraccionamientos.Campo_Fraccionamiento_ID + Validar_Operador_Comparacion(P_Fraccionamiento.P_Fraccionamiento_ID);
            }
            Cls_Cat_Pre_Fraccionamientos_Negocio R_Fraccionamiento = new Cls_Cat_Pre_Fraccionamientos_Negocio();
            OracleDataReader Data_Reader;
            try{
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Fraccionamiento.P_Fraccionamiento_ID = P_Fraccionamiento.P_Fraccionamiento_ID;
                while (Data_Reader.Read()){
                    R_Fraccionamiento.P_Identificador = Data_Reader[Cat_Pre_Fraccionamientos.Campo_Identificador].ToString();
                    R_Fraccionamiento.P_Estatus = Data_Reader[Cat_Pre_Fraccionamientos.Campo_Estatus].ToString();
                    R_Fraccionamiento.P_Descripcion = Data_Reader[Cat_Pre_Fraccionamientos.Campo_Descripcion].ToString();
                }
                Data_Reader.Close();
                Mi_SQL = "SELECT " + Cat_Pre_Fracc_Impuestos.Campo_Impuesto_Fraccionamiento_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Fracc_Impuestos.Campo_Fraccionamiento_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Fracc_Impuestos.Campo_Año + "";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Fracc_Impuestos.Campo_Monto + "";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Fracc_Impuestos.Tabla_Cat_Pre_Fracc_Impuestos;
                Mi_SQL = Mi_SQL + " WHERE ";
                if (P_Fraccionamiento.P_Fraccionamiento_ID != null && P_Fraccionamiento.P_Fraccionamiento_ID != "")
                {
                    Mi_SQL = Mi_SQL + Cat_Pre_Fracc_Impuestos.Campo_Fraccionamiento_ID + Validar_Operador_Comparacion(P_Fraccionamiento.P_Fraccionamiento_ID) + " AND ";
                }
                if (P_Fraccionamiento.P_Fraccionamiento_Impuesto_ID != null && P_Fraccionamiento.P_Fraccionamiento_Impuesto_ID != "")
                {
                    Mi_SQL = Mi_SQL + Cat_Pre_Fracc_Impuestos.Campo_Impuesto_Fraccionamiento_ID + Validar_Operador_Comparacion(P_Fraccionamiento.P_Fraccionamiento_Impuesto_ID) + " AND ";
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
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Fracc_Impuestos.Campo_Año + " DESC";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset == null){
                    R_Fraccionamiento.P_Fraccionamientos_Impuestos = new DataTable();
                }else{
                    R_Fraccionamiento.P_Fraccionamientos_Impuestos = dataset.Tables[0];
                }
            }catch (Exception Ex){
                String Mensaje = "Error al intentar consultar el registro de Fraccionamientos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Fraccionamiento;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Fraccionamiento
        ///DESCRIPCIÓN: Elimina un Fraccionamiento
        ///PARAMETROS:   
        ///             1. P_Fraccionamiento.   Fraccionamiento que se va eliminar.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Agosto/2010 
        ///MODIFICO: Roberto González Oseguera
        ///FECHA_MODIFICO: 21-feb-2012
        ///CAUSA_MODIFICACIÓN: Cambio de eliminación física de la base a cambio de estatus a BAJA
        ///*******************************************************************************
        public static void Eliminar_Fraccionamiento(Cls_Cat_Pre_Fraccionamientos_Negocio Fraccionamiento)
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
            try
            {
                String Mi_SQL = "UPDATE " + Cat_Pre_Fraccionamientos.Tabla_Cat_Pre_Fraccionamientos
                    + " SET " + Cat_Pre_Fraccionamientos.Campo_Estatus + " = '" + Fraccionamiento.P_Estatus + "'"
                    + ", " + Cat_Pre_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Fraccionamiento.P_Usuario + "'"
                    + ", " + Cat_Pre_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE"
                    + " WHERE " + Cat_Pre_Fraccionamientos.Campo_Fraccionamiento_ID + " = '" + Fraccionamiento.P_Fraccionamiento_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al intentar modificar el registro de Fraccionamiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Fraccionamientos_Impuestos
        ///DESCRIPCIÓN          : Obtiene todos las Convenio_Traslado_Dominio que estan dadas de alta en la base de datos
        ///PARAMETROS           : 1. Convenio_Traslado_Dominio.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Derecho_Supervisions_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 03/Agosto/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Fraccionamientos_Impuestos(Cls_Cat_Pre_Fraccionamientos_Negocio Fraccionamiento)
        {
            DataTable Dt_Fraccionamientos = new DataTable();
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT ";
                if (Fraccionamiento.P_Campos_Dinamicos != null && Fraccionamiento.P_Campos_Dinamicos != "")
                {
                    Mi_SQL += Fraccionamiento.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL += Cat_Pre_Fraccionamientos.Tabla_Cat_Pre_Fraccionamientos + "." + Cat_Pre_Fraccionamientos.Campo_Fraccionamiento_ID + ", ";
                    Mi_SQL += Cat_Pre_Fraccionamientos.Tabla_Cat_Pre_Fraccionamientos + "." + Cat_Pre_Fraccionamientos.Campo_Identificador + ", ";
                    Mi_SQL += Cat_Pre_Fraccionamientos.Tabla_Cat_Pre_Fraccionamientos + "." + Cat_Pre_Fraccionamientos.Campo_Descripcion + ", ";
                    Mi_SQL += Cat_Pre_Fraccionamientos.Tabla_Cat_Pre_Fraccionamientos + "." + Cat_Pre_Fraccionamientos.Campo_Estatus + ", ";

                    Mi_SQL += Cat_Pre_Fracc_Impuestos.Tabla_Cat_Pre_Fracc_Impuestos + "." + Cat_Pre_Fracc_Impuestos.Campo_Impuesto_Fraccionamiento_ID + ", ";
                    Mi_SQL += Cat_Pre_Fracc_Impuestos.Tabla_Cat_Pre_Fracc_Impuestos + "." + Cat_Pre_Fracc_Impuestos.Campo_Año + ", ";
                    Mi_SQL += Cat_Pre_Fracc_Impuestos.Tabla_Cat_Pre_Fracc_Impuestos + "." + Cat_Pre_Fracc_Impuestos.Campo_Monto;
                }
                Mi_SQL += " FROM " + Cat_Pre_Fraccionamientos.Tabla_Cat_Pre_Fraccionamientos + ", " + Cat_Pre_Fracc_Impuestos.Tabla_Cat_Pre_Fracc_Impuestos;
                Mi_SQL += " WHERE " + Cat_Pre_Fraccionamientos.Tabla_Cat_Pre_Fraccionamientos + "." + Cat_Pre_Fraccionamientos.Campo_Fraccionamiento_ID + " = " + Cat_Pre_Fracc_Impuestos.Tabla_Cat_Pre_Fracc_Impuestos + "." + Cat_Pre_Fracc_Impuestos.Campo_Fraccionamiento_ID + " AND ";
                if (Fraccionamiento.P_Filtros_Dinamicos != null && Fraccionamiento.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL += Fraccionamiento.P_Filtros_Dinamicos;
                }
                else
                {
                    if (Fraccionamiento.P_Fraccionamiento_ID != "" && Fraccionamiento.P_Fraccionamiento_ID != null)
                    {
                        Mi_SQL += Cat_Pre_Fraccionamientos.Tabla_Cat_Pre_Fraccionamientos + "." + Cat_Pre_Fraccionamientos.Campo_Fraccionamiento_ID + " = '" + Fraccionamiento.P_Fraccionamiento_ID + "'";
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
                if (Fraccionamiento.P_Agrupar_Dinamico != null && Fraccionamiento.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " GROUP BY " + Fraccionamiento.P_Agrupar_Dinamico;
                }
                if (Fraccionamiento.P_Ordenar_Dinamico != null && Fraccionamiento.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Fraccionamiento.P_Ordenar_Dinamico;
                }
                else
                {
                    Mi_SQL += " ORDER BY " + Cat_Pre_Fraccionamientos.Tabla_Cat_Pre_Fraccionamientos + "." + Cat_Pre_Fraccionamientos.Campo_Identificador + ", " + Cat_Pre_Fraccionamientos.Tabla_Cat_Pre_Fraccionamientos + "." + Cat_Pre_Fraccionamientos.Campo_Descripcion;
                }

                DataSet Ds_Fraccionamientos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Fraccionamientos != null)
                {
                    Dt_Fraccionamientos = Ds_Fraccionamientos.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Fraccionamientos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Fraccionamientos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Fraccionamientos_Impuestos
        ///DESCRIPCIÓN          : Obtiene todos las Convenio_Traslado_Dominio que estan dadas de alta en la base de datos
        ///PARAMETROS           : 1. Convenio_Traslado_Dominio.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Derecho_Supervisions_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 03/Agosto/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Fraccionamientos_Detalles_Impuestos(Cls_Cat_Pre_Fraccionamientos_Negocio Fraccionamiento)
        {
            DataTable Dt_Fraccionamientos = new DataTable();
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT ";
                if (Fraccionamiento.P_Campos_Dinamicos != null && Fraccionamiento.P_Campos_Dinamicos != "")
                {
                    Mi_SQL += Fraccionamiento.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL += Cat_Pre_Fraccionamientos.Campo_Fraccionamiento_ID + ", ";
                    Mi_SQL += Cat_Pre_Fraccionamientos.Campo_Identificador + ", ";
                    Mi_SQL += Cat_Pre_Fraccionamientos.Campo_Descripcion + ", ";
                    Mi_SQL += Cat_Pre_Fraccionamientos.Campo_Estatus;
                }
                Mi_SQL += " FROM " + Cat_Pre_Fraccionamientos.Tabla_Cat_Pre_Fraccionamientos;
                Mi_SQL += " WHERE ";
                if (Fraccionamiento.P_Filtros_Dinamicos != null && Fraccionamiento.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL += Fraccionamiento.P_Filtros_Dinamicos;
                }
                else
                {
                    if (Fraccionamiento.P_Fraccionamiento_ID != "" && Fraccionamiento.P_Fraccionamiento_ID != null)
                    {
                        Mi_SQL += Cat_Pre_Fraccionamientos.Campo_Fraccionamiento_ID + " = '" + Fraccionamiento.P_Fraccionamiento_ID + "'";
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
                if (Fraccionamiento.P_Agrupar_Dinamico != null && Fraccionamiento.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " GROUP BY " + Fraccionamiento.P_Agrupar_Dinamico;
                }
                if (Fraccionamiento.P_Ordenar_Dinamico != null && Fraccionamiento.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Fraccionamiento.P_Ordenar_Dinamico;
                }
                else
                {
                    Mi_SQL += " ORDER BY " + Cat_Pre_Fraccionamientos.Campo_Identificador + ", " + Cat_Pre_Fraccionamientos.Campo_Descripcion;
                }

                DataSet Ds_Fraccionamientos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Fraccionamientos != null)
                {
                    Dt_Fraccionamientos = Ds_Fraccionamientos.Tables[0];

                    Mi_SQL = "SELECT ";
                    Mi_SQL += Cat_Pre_Fracc_Impuestos.Campo_Impuesto_Fraccionamiento_ID + ", ";
                    Mi_SQL += Cat_Pre_Fracc_Impuestos.Campo_Año + ", ";
                    Mi_SQL += Cat_Pre_Fracc_Impuestos.Campo_Monto;
                    Mi_SQL += " FROM ";
                    Mi_SQL += Cat_Pre_Fracc_Impuestos.Tabla_Cat_Pre_Fracc_Impuestos;
                    if (Fraccionamiento.P_Filtros_Dinamicos != null && Fraccionamiento.P_Filtros_Dinamicos != "")
                    {
                        Mi_SQL += " WHERE ";
                        Mi_SQL += Cat_Pre_Fracc_Impuestos.Campo_Fraccionamiento_ID + " IN (SELECT " + Cat_Pre_Fraccionamientos.Campo_Fraccionamiento_ID + " FROM " + Cat_Pre_Fraccionamientos.Tabla_Cat_Pre_Fraccionamientos + " WHERE " + Fraccionamiento.P_Filtros_Dinamicos + ")";
                    }
                    else
                    {
                        if (Fraccionamiento.P_Fraccionamiento_ID != null && Fraccionamiento.P_Fraccionamiento_ID != "")
                        {
                            Mi_SQL += " WHERE ";
                            Mi_SQL += Cat_Pre_Fracc_Impuestos.Campo_Fraccionamiento_ID + " = '" + Fraccionamiento.P_Fraccionamiento_ID + "'";
                        }
                    }
                    Fraccionamiento.P_Dt_Fraccionamientos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Fraccionamientos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Fraccionamientos;
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