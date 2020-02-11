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
using Presidencia.Catalogo_Divisiones.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Divisiones_Datos
/// </summary>
/// 

namespace Presidencia.Catalogo_Divisiones.Datos{
    public class Cls_Cat_Pre_Divisiones_Datos {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Division
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nueva Division
        ///PARAMETROS:     
        ///             1. Division.    Instancia de la Clase de Negocio de Cls_Cat_Pre_Divisiones_Negocio
        ///                             con los datos de la Division que va a ser dado de Alta.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 26/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Division(Cls_Cat_Pre_Divisiones_Negocio Division) {
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
                String Division_ID = Obtener_ID_Consecutivo(Cat_Pre_Divisiones.Tabla_Cat_Pre_Divisiones, Cat_Pre_Divisiones.Campo_Division_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Divisiones.Tabla_Cat_Pre_Divisiones;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Divisiones.Campo_Division_ID + ", " + Cat_Pre_Divisiones.Campo_Identificador;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Divisiones.Campo_Descripcion + ", " + Cat_Pre_Divisiones.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Divisiones.Campo_Usuario_Creo + ", " + Cat_Pre_Divisiones.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Division_ID + "', '" + Division.P_Identificador + "'";
                Mi_SQL = Mi_SQL + ",'" + Division.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + ",'" + Division.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Division.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", SYSDATE";
                Mi_SQL = Mi_SQL + ")";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                if (Division.P_Divisiones_Impuestos != null && Division.P_Divisiones_Impuestos.Rows.Count > 0){
                    String Division_Impuesto_ID = Obtener_ID_Consecutivo(Cat_Pre_Divisiones_Impuestos.Tabla_Cat_Pre_Divisiones_Impuestos, Cat_Pre_Divisiones_Impuestos.Campo_Impuesto_Division_Lot_ID, 5);
                    for (int cnt = 0; cnt < Division.P_Divisiones_Impuestos.Rows.Count; cnt++){
                        Mi_SQL = "INSERT INTO " + Cat_Pre_Divisiones_Impuestos.Tabla_Cat_Pre_Divisiones_Impuestos;
                        Mi_SQL = Mi_SQL + " (" + Cat_Pre_Divisiones_Impuestos.Campo_Impuesto_Division_Lot_ID + ", " + Cat_Pre_Divisiones_Impuestos.Campo_Division_ID;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Divisiones_Impuestos.Campo_Año + ", " + Cat_Pre_Divisiones_Impuestos.Campo_Tasa;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Divisiones_Impuestos.Campo_Usuario_Creo + ", " + Cat_Pre_Divisiones_Impuestos.Campo_Fecha_Creo + ")";
                        Mi_SQL = Mi_SQL + " VALUES ('" + Division_Impuesto_ID + "', '" + Division_ID + "', " + Division.P_Divisiones_Impuestos.Rows[cnt][1].ToString() + ", " + Division.P_Divisiones_Impuestos.Rows[cnt][2].ToString();
                        Mi_SQL = Mi_SQL + ",'" + Division.P_Usuario + "', SYSDATE";
                        Mi_SQL = Mi_SQL + ")";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        Division_Impuesto_ID = Convertir_A_Formato_ID(Convert.ToInt32(Division_Impuesto_ID) + 1, 5);
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
                    Mensaje = "Error al intentar dar de Alta un Registro de División. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }finally {
                 Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Division
        ///DESCRIPCIÓN: Actualiza en la Base de Datos una Division
        ///PARAMETROS:     
        ///             1. Divisiones.  Instancia de la Clase de Negocio de Divisiones 
        ///                             con los datos de la Division que va a ser Actualizada.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 26/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Division(Cls_Cat_Pre_Divisiones_Negocio Division){
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
                Cls_Cat_Pre_Divisiones_Negocio Division_Tmp = Consultar_Datos_Division(Division);
                String Mi_SQL = "UPDATE " + Cat_Pre_Divisiones.Tabla_Cat_Pre_Divisiones + " SET " + Cat_Pre_Divisiones.Campo_Identificador + " = '" + Division.P_Identificador + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Divisiones.Campo_Estatus + " = '" + Division.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Divisiones.Campo_Descripcion + " = '" + Division.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Divisiones.Campo_Usuario_Modifico + " = '" + Division.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Divisiones.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Divisiones.Campo_Division_ID + " = '" + Division.P_Division_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Division_Tmp = Obtener_Divisiones_Impuestos_Eliminados(Division_Tmp, Division);
                for (int cnt = 0; cnt < Division_Tmp.P_Divisiones_Impuestos.Rows.Count; cnt++) {
                    Mi_SQL = "DELETE FROM " + Cat_Pre_Divisiones_Impuestos.Tabla_Cat_Pre_Divisiones_Impuestos;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Divisiones_Impuestos.Campo_Impuesto_Division_Lot_ID + " = '" + Division_Tmp.P_Divisiones_Impuestos.Rows[cnt][0].ToString() + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }
                if (Division.P_Divisiones_Impuestos!= null && Division.P_Divisiones_Impuestos.Rows.Count > 0){
                    String Division_Impuesto_ID = Obtener_ID_Consecutivo(Cat_Pre_Divisiones_Impuestos.Tabla_Cat_Pre_Divisiones_Impuestos, Cat_Pre_Divisiones_Impuestos.Campo_Impuesto_Division_Lot_ID, 5);
                    for (int cnt = 0; cnt < Division.P_Divisiones_Impuestos.Rows.Count; cnt++){
                        if (Division.P_Divisiones_Impuestos.Rows[cnt][0].ToString().Trim().Equals("")){
                            Mi_SQL = "INSERT INTO " + Cat_Pre_Divisiones_Impuestos.Tabla_Cat_Pre_Divisiones_Impuestos;
                            Mi_SQL = Mi_SQL + " (" + Cat_Pre_Divisiones_Impuestos.Campo_Impuesto_Division_Lot_ID + ", " + Cat_Pre_Divisiones_Impuestos.Campo_Division_ID;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Divisiones_Impuestos.Campo_Año + ", " + Cat_Pre_Divisiones_Impuestos.Campo_Tasa;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Divisiones_Impuestos.Campo_Usuario_Creo + ", " + Cat_Pre_Divisiones_Impuestos.Campo_Fecha_Creo + ")";
                            Mi_SQL = Mi_SQL + " VALUES ('" + Division_Impuesto_ID + "', '" + Division.P_Division_ID + "', " + Division.P_Divisiones_Impuestos.Rows[cnt][1].ToString() + ", " + Division.P_Divisiones_Impuestos.Rows[cnt][2].ToString();
                            Mi_SQL = Mi_SQL + ",'" + Division.P_Usuario + "', SYSDATE";
                            Mi_SQL = Mi_SQL + ")";
                            Division_Impuesto_ID = Convertir_A_Formato_ID(Convert.ToInt32(Division_Impuesto_ID) + 1, 5);
                        } else {
                            Mi_SQL = "UPDATE " + Cat_Pre_Divisiones_Impuestos.Tabla_Cat_Pre_Divisiones_Impuestos + " SET " + Cat_Pre_Divisiones_Impuestos.Campo_Año + " = " + Division.P_Divisiones_Impuestos.Rows[cnt][1].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Divisiones_Impuestos.Campo_Tasa + " = " + Division.P_Divisiones_Impuestos.Rows[cnt][2].ToString().Trim();
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Divisiones_Impuestos.Campo_Usuario_Modifico + " = '" + Division.P_Usuario + "'";
                            Mi_SQL = Mi_SQL + "," + Cat_Pre_Divisiones_Impuestos.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Divisiones_Impuestos.Campo_Impuesto_Division_Lot_ID + " = '" + Division.P_Divisiones_Impuestos.Rows[cnt][0].ToString().Trim() + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de División. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Divisiones_Impuestos_Eliminados
        ///DESCRIPCIÓN: Obtiene la lista de los Divisiones Impuestos que fueron dados de alta en la Actualizacion de las
        ///             Divisiones
        ///PARAMETROS:     
        ///             1. Actuales.        Divisiones que se usa para saber los Diviviones_Impuestos que estan en 
        ///                                 la Base de Datos antes de la Modificacion.
        ///             2. Actualizados.    Divisiones que se usa para saber los Divisiones_Impuestos que estaran en 
        ///                                 la Base de Datos despues de la Modificacion.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 26/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static Cls_Cat_Pre_Divisiones_Negocio Obtener_Divisiones_Impuestos_Eliminados(Cls_Cat_Pre_Divisiones_Negocio Actuales, Cls_Cat_Pre_Divisiones_Negocio Actualizados){
            Cls_Cat_Pre_Divisiones_Negocio Division = new Cls_Cat_Pre_Divisiones_Negocio();
            DataTable tabla = new DataTable();
            tabla.Columns.Add("IMPUESTO_DIVISION_LOT_ID", Type.GetType("System.String"));
            tabla.Columns.Add("ANIO", Type.GetType("System.String"));
            tabla.Columns.Add("TASA", Type.GetType("System.String"));
            for (int cnt1 = 0; cnt1 < Actuales.P_Divisiones_Impuestos.Rows.Count; cnt1++) {
                bool eliminar = true;
                for (int cnt2 = 0; cnt2 < Actualizados.P_Divisiones_Impuestos.Rows.Count; cnt2++) {
                    if (!Actualizados.P_Divisiones_Impuestos.Rows[cnt2][0].ToString().Equals("")) {
                        if (Actuales.P_Divisiones_Impuestos.Rows[cnt1][0].ToString().Equals(Actualizados.P_Divisiones_Impuestos.Rows[cnt2][0].ToString())) {
                            eliminar = false;
                            break;
                        }
                    }
                }
                if (eliminar) {
                    DataRow fila = tabla.NewRow();
                    fila["IMPUESTO_DIVISION_LOT_ID"] = Actuales.P_Divisiones_Impuestos.Rows[cnt1][0].ToString();
                    fila["ANIO"] = Actuales.P_Divisiones_Impuestos.Rows[cnt1][1].ToString();
                    fila["TASA"] = Actuales.P_Divisiones_Impuestos.Rows[cnt1][2].ToString();
                    tabla.Rows.Add(fila);
                }
            }
            Division.P_Divisiones_Impuestos = tabla;
            return Division;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Divisiones
        ///DESCRIPCIÓN: Obtiene todas las divisiones que estan dadas de alta en la Base de Datos
        ///PARAMETROS:   
        ///             1.  Division.   Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                             caso el filtro es el Identificador.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 26/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Divisiones(Cls_Cat_Pre_Divisiones_Negocio Division){
            DataTable Tabla = new DataTable();
            try{
                String Mi_SQL = "SELECT D." + Cat_Pre_Divisiones.Campo_Division_ID + " AS DIVISION_ID, D." + Cat_Pre_Divisiones.Campo_Descripcion + " AS DESCRIPCION";
                Mi_SQL = Mi_SQL + ", D."+ Cat_Pre_Divisiones.Campo_Estatus +" AS ESTATUS";
                Mi_SQL = Mi_SQL +  " FROM " + Cat_Pre_Divisiones.Tabla_Cat_Pre_Divisiones + " D";
                //Mi_SQL = Mi_SQL + " D LEFT JOIN " + Cat_Pre_Divisiones_Impuestos.Tabla_Cat_Pre_Divisiones_Impuestos;
                //Mi_SQL = Mi_SQL + " DI ON DI." + Cat_Pre_Divisiones_Impuestos.Campo_Division_ID + " = D." + Cat_Pre_Divisiones.Campo_Division_ID;
                Mi_SQL = Mi_SQL + " WHERE D." + Cat_Pre_Divisiones.Campo_Identificador + " LIKE '%" + Division.P_Identificador + "%' ";
                Mi_SQL = Mi_SQL + " OR D." + Cat_Pre_Divisiones.Campo_Descripcion + " LIKE '%" + Division.P_Identificador + "%' ";
                //Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Divisiones_Impuestos.Campo_Año + " DESC";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Divisiones.Campo_Descripcion;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null) {
                    Tabla = dataset.Tables[0];
                }
            }catch(Exception Ex){
                String Mensaje = "Error al intentar consultar los registros de Divisiones. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Divisiones_ID
        ///DESCRIPCIÓN: Obtiene datos de la division con el ID proporcionado
        ///PARAMETROS:   
        ///             1.  Division_ID.   ID de tasa de division a localizar
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 15/ago/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Divisiones_ID(String Division_Lot_ID)
        {
            String Mi_SQL; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL += "D." + Cat_Pre_Divisiones.Campo_Division_ID + ", ";
                Mi_SQL += "D." + Cat_Pre_Divisiones.Campo_Descripcion + ", ";
                Mi_SQL += "D." + Cat_Pre_Divisiones.Campo_Identificador + ", ";
                Mi_SQL += "T." + Cat_Pre_Divisiones_Impuestos.Campo_Año + ", ";
                Mi_SQL += "T." + Cat_Pre_Divisiones_Impuestos.Campo_Tasa;
                Mi_SQL += " FROM ";
                Mi_SQL += Cat_Pre_Divisiones.Tabla_Cat_Pre_Divisiones + " D, ";
                Mi_SQL += Cat_Pre_Divisiones_Impuestos.Tabla_Cat_Pre_Divisiones_Impuestos + " T ";
                Mi_SQL += " WHERE ";
                Mi_SQL += "T." + Cat_Pre_Divisiones_Impuestos.Campo_Division_ID + " = ";
                Mi_SQL += "D." + Cat_Pre_Divisiones.Campo_Division_ID;
                if (!String.IsNullOrEmpty(Division_Lot_ID))      // Si se recibió un ID de documento filtrar por ese ID
                {
                    Mi_SQL += " AND " + Cat_Pre_Divisiones_Impuestos.Campo_Impuesto_Division_Lot_ID + " = '" + Division_Lot_ID + "'";
                }
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Division
        ///DESCRIPCIÓN: Obtiene a detalle una Disivion.
        ///PARAMETROS:   
        ///             1. P_Division.   Division que se va ver a Detalle.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 26/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Divisiones_Negocio Consultar_Datos_Division(Cls_Cat_Pre_Divisiones_Negocio P_Division){
            String Mi_SQL = "SELECT " + Cat_Pre_Divisiones.Campo_Identificador + ", " + Cat_Pre_Divisiones.Campo_Estatus + ", " + Cat_Pre_Divisiones.Campo_Descripcion + " FROM " + Cat_Pre_Divisiones.Tabla_Cat_Pre_Divisiones + " WHERE " + Cat_Pre_Divisiones.Campo_Division_ID + " = '" + P_Division.P_Division_ID + "'";
            Cls_Cat_Pre_Divisiones_Negocio R_Division = new Cls_Cat_Pre_Divisiones_Negocio();
            OracleDataReader Data_Reader;
            try{
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Division.P_Division_ID = P_Division.P_Division_ID;
                while (Data_Reader.Read()){
                    R_Division.P_Identificador = Data_Reader[Cat_Pre_Divisiones.Campo_Identificador].ToString();
                    R_Division.P_Estatus = Data_Reader[Cat_Pre_Divisiones.Campo_Estatus].ToString();
                    R_Division.P_Descripcion = Data_Reader[Cat_Pre_Divisiones.Campo_Descripcion].ToString();
                }
                Data_Reader.Close();
                Mi_SQL = "SELECT " + Cat_Pre_Divisiones_Impuestos.Campo_Impuesto_Division_Lot_ID + " AS IMPUESTO_DIVISION_LOT_ID, " + Cat_Pre_Divisiones_Impuestos.Campo_Año + " AS ANIO";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Divisiones_Impuestos.Campo_Tasa + " AS TASA";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Divisiones_Impuestos.Tabla_Cat_Pre_Divisiones_Impuestos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Divisiones_Impuestos.Campo_Division_ID + " = '" + P_Division.P_Division_ID + "'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Divisiones_Impuestos.Campo_Año + " DESC";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset == null){
                    R_Division.P_Divisiones_Impuestos = new DataTable();
                }else{
                    R_Division.P_Divisiones_Impuestos = dataset.Tables[0];
                }
            }catch (Exception Ex){
                String Mensaje = "Error al intentar consultar el registro de División. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Division;
        }

        /////*******************************************************************************
        /////NOMBRE DE LA FUNCIÓN   : Consultar_Tasas_Divisines_Lotificaciones
        /////DESCRIPCIÓN            : Obtiene las tasas de las Divisiones y Lotificaciones
        /////PARAMETROS             : Division_Lotificacion, instancia de Cls_Cat_Pre_Divisiones_Negocio
        /////CREO                   : Antonio Salvador Benavides Guardado
        /////FECHA_CREO             : 10/Diciembre/2010 
        /////MODIFICO               : Roberto González Oseguera
        /////FECHA_MODIFICO         : 08-mar-2012
        /////CAUSA_MODIFICACIÓN     : Se agrega filtro opcional por año
        /////*******************************************************************************
        public static DataTable Consultar_Tasas_Divisines_Lotificaciones(Cls_Cat_Pre_Divisiones_Negocio Division_Lotificacion)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "SELECT (SELECT " + Cat_Pre_Divisiones.Campo_Division_ID + " FROM " + Cat_Pre_Divisiones.Tabla_Cat_Pre_Divisiones + " WHERE " + Cat_Pre_Divisiones.Campo_Division_ID + " = " + Cat_Pre_Divisiones_Impuestos.Tabla_Cat_Pre_Divisiones_Impuestos + "." + Cat_Pre_Divisiones_Impuestos.Campo_Division_ID + ") AS DIVISION_ID";
            Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Pre_Divisiones.Campo_Identificador + " FROM " + Cat_Pre_Divisiones.Tabla_Cat_Pre_Divisiones + " WHERE " + Cat_Pre_Divisiones.Campo_Division_ID + " = " + Cat_Pre_Divisiones_Impuestos.Tabla_Cat_Pre_Divisiones_Impuestos + "." + Cat_Pre_Divisiones_Impuestos.Campo_Division_ID + ") AS IDENTIFICADOR";
            Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Pre_Divisiones.Campo_Descripcion + " FROM " + Cat_Pre_Divisiones.Tabla_Cat_Pre_Divisiones + " WHERE " + Cat_Pre_Divisiones.Campo_Division_ID + " = " + Cat_Pre_Divisiones_Impuestos.Tabla_Cat_Pre_Divisiones_Impuestos + "." + Cat_Pre_Divisiones_Impuestos.Campo_Division_ID + ") AS DESCRIPCION";
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Divisiones_Impuestos.Campo_Impuesto_Division_Lot_ID;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Divisiones_Impuestos.Campo_Año;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Divisiones_Impuestos.Campo_Tasa;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Divisiones_Impuestos.Tabla_Cat_Pre_Divisiones_Impuestos;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Divisiones_Impuestos.Campo_Division_ID + " IN (SELECT " + Cat_Pre_Divisiones.Campo_Division_ID + " FROM " + Cat_Pre_Divisiones.Tabla_Cat_Pre_Divisiones + " WHERE " + Cat_Pre_Divisiones.Campo_Estatus + " = 'VIGENTE')";

            // si se especifico un año, agregar como filtro a la consulta
            if (Division_Lotificacion.P_Anio > 0)
            {
                Mi_SQL+= " AND " + Cat_Pre_Divisiones_Impuestos.Campo_Año + "=" + Division_Lotificacion.P_Anio;
            }

            Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Divisiones_Impuestos.Campo_Año + " DESC ";
            try
            {
                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null)
                {
                    Tabla = dataSet.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar las Tasas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Division
        ///DESCRIPCIÓN: Elimina una Division
        ///PARAMETROS:   
        ///             1. Division.   Division que se va eliminar.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 26/Agosto/2010 
        ///MODIFICO: Roberto González Oseguera
        ///FECHA_MODIFICO: 22-feb-2012
        ///CAUSA_MODIFICACIÓN: Agregar baja lógica si se especifica un estatus
        ///*******************************************************************************
        public static void Eliminar_Division(Cls_Cat_Pre_Divisiones_Negocio Division)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            String Mi_SQL;

            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try
            {
                // si no se especifica estatus intentar la baja física en BD
                if (string.IsNullOrEmpty(Division.P_Estatus))
                {
                    Mi_SQL = "DELETE FROM " + Cat_Pre_Divisiones_Impuestos.Tabla_Cat_Pre_Divisiones_Impuestos;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Divisiones_Impuestos.Campo_Division_ID + " = '" + Division.P_Division_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Mi_SQL = "DELETE FROM " + Cat_Pre_Divisiones.Tabla_Cat_Pre_Divisiones;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Divisiones.Campo_Division_ID + " = '" + Division.P_Division_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Trans.Commit();
                }
                else    // si se especifica estatus cambiar por estatus especificado
                {
                    Mi_SQL = "UPDATE " + Cat_Pre_Divisiones.Tabla_Cat_Pre_Divisiones
                        + " SET " + Cat_Pre_Divisiones.Campo_Estatus + " = '" + Division.P_Estatus + "'"
                        + "," + Cat_Pre_Divisiones.Campo_Usuario_Modifico + " = '" + Division.P_Usuario + "'"
                        + "," + Cat_Pre_Divisiones.Campo_Fecha_Modifico + " = SYSDATE"
                        + " WHERE " + Cat_Pre_Divisiones.Campo_Division_ID + " = '" + Division.P_Division_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Trans.Commit();
                }
            }
            catch (OracleException Ex)
            {
                if (Ex.Code == 547)
                {
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos.";
                }
                else
                {
                    Mensaje = "Error al intentar eliminar el registro de División. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al intentar eliminar el registro de División. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
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