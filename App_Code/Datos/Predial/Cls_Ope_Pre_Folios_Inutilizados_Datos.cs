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
using Presidencia.Operacion_Folios_Inutilizados.Negocio;
using System.Text;

namespace Presidencia.Operacion_Folios_Inutilizados.Datos{
    
    public class Cls_Ope_Pre_Folios_Inutilizados_Datos {

       
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Folio_Inutilizado
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo Folio Inutilizado
        ///PARAMETROS:     
        ///             1. Folio.  Instancia de la Clase de Negocio de Folios Inutilzados con los datos 
        ///                         del Folio que va a ser dado de Alta.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 25/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Folio_Inutilizado(Cls_Ope_Pre_Folios_Inutilizados_Negocio Folio)
        {
            String Mensaje = "";
            String Pago = string.Empty;
            String Mi_SQL = "";
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
                if (String.IsNullOrEmpty(Folio.P_No_Folio_Fin))
                {
                    String Pago_ID = Obtener_ID_Consecutivo(ref Cmd, Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos, Ope_Caj_Pagos.Campo_No_Pago, 10);
                    Mi_SQL = "INSERT INTO " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                    Mi_SQL = Mi_SQL + " (" + Ope_Caj_Pagos.Campo_No_Pago + ", " + Ope_Caj_Pagos.Campo_No_Recibo;
                    Mi_SQL = Mi_SQL + ", " + Ope_Caj_Pagos.Campo_Estatus + ", " + Ope_Caj_Pagos.Campo_Caja_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Caj_Pagos.Campo_Fecha + ", " + Ope_Caj_Pagos.Campo_Motivo_Cancelacion_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Caj_Pagos.Campo_Observaciones + ", " + Ope_Pre_Pagos.Campo_Empleado_Id + ", " + Ope_Caj_Pagos.Campo_No_Turno;
                    Mi_SQL = Mi_SQL + ", " + Ope_Caj_Pagos.Campo_Usuario_Creo + ", " + Ope_Caj_Pagos.Campo_Fecha_Creo + ")";
                    Mi_SQL = Mi_SQL + " VALUES ('" + Pago_ID + "', '" + Folio.P_No_Recibo + "'";
                    Mi_SQL = Mi_SQL + ",'INUTILIZADO'";
                    Mi_SQL = Mi_SQL + ",'" + Folio.P_Caja_ID + "'";
                    Mi_SQL = Mi_SQL + ",'" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Folio.P_Fecha)) + "'";
                    Mi_SQL = Mi_SQL + ",'" + Folio.P_Motivo + "'";
                    Mi_SQL = Mi_SQL + ",'" + Folio.P_Observaciones + "'";
                    Mi_SQL = Mi_SQL + ",'" + Folio.P_Empleado_ID + "'";
                    Mi_SQL = Mi_SQL + ",'" + Folio.P_No_Turno + "'";
                    Mi_SQL = Mi_SQL + ",'" + Folio.P_Usuario + "'";
                    Mi_SQL = Mi_SQL + ", SYSDATE";
                    Mi_SQL = Mi_SQL + ")";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();

                    Mi_SQL = "UPDATE " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + " SET " + Ope_Caj_Turnos.Campo_Contador_Recibo + " = " + (Convert.ToInt32(Folio.P_No_Recibo) + 1);
                    Mi_SQL += " WHERE " + Ope_Caj_Turnos.Campo_No_Turno + " = '" + Folio.P_No_Turno + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }
                else { 
                    if (Folio.P_Dt_Folios.Columns.Count > 0){
                        if (Folio.P_Dt_Folios.Rows.Count > 0) {
                            foreach (DataRow Dr in Folio.P_Dt_Folios.Rows) {
                                String Pago_ID = Obtener_ID_Consecutivo(ref Cmd, Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos, Ope_Caj_Pagos.Campo_No_Pago, 10);
                                Folio.P_No_Recibo = Dr["No_Folio"].ToString();
                                Mi_SQL = "INSERT INTO " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                                Mi_SQL = Mi_SQL + " (" + Ope_Caj_Pagos.Campo_No_Pago + ", " + Ope_Caj_Pagos.Campo_No_Recibo;
                                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Pagos.Campo_Estatus + ", " + Ope_Caj_Pagos.Campo_Caja_ID;
                                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Pagos.Campo_Fecha + ", " + Ope_Caj_Pagos.Campo_Motivo_Cancelacion_ID;
                                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Pagos.Campo_Observaciones + ", " + Ope_Pre_Pagos.Campo_Empleado_Id + ", " + Ope_Caj_Pagos.Campo_No_Turno;
                                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Pagos.Campo_Usuario_Creo + ", " + Ope_Caj_Pagos.Campo_Fecha_Creo + ")";
                                Mi_SQL = Mi_SQL + " VALUES ('" + Pago_ID + "', '" + Dr["No_Folio"].ToString() + "'";
                                Mi_SQL = Mi_SQL + ",'INUTILIZADO'";
                                Mi_SQL = Mi_SQL + ",'" + Folio.P_Caja_ID + "'";
                                Mi_SQL = Mi_SQL + ",'" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Folio.P_Fecha)) + "'";
                                Mi_SQL = Mi_SQL + ",'" + Folio.P_Motivo + "'";
                                Mi_SQL = Mi_SQL + ",'" + Folio.P_Observaciones + "'";
                                Mi_SQL = Mi_SQL + ",'" + Folio.P_Empleado_ID + "'";
                                Mi_SQL = Mi_SQL + ",'" + Folio.P_No_Turno + "'";
                                Mi_SQL = Mi_SQL + ",'" + Folio.P_Usuario + "'";
                                Mi_SQL = Mi_SQL + ", SYSDATE";
                                Mi_SQL = Mi_SQL + ")";
                                Cmd.CommandText = Mi_SQL;
                                Cmd.ExecuteNonQuery();
                            }
                            Mi_SQL = "UPDATE " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + " SET " + Ope_Caj_Turnos.Campo_Contador_Recibo + " = " + (Convert.ToInt32(Folio.P_No_Recibo) + 1);
                            Mi_SQL += " WHERE " + Ope_Caj_Turnos.Campo_No_Turno + " = '" + Folio.P_No_Turno + "'";
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                        }
                    }
                }

                
                Trans.Commit();
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
          
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
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
                    Mensaje = "Error al intentar dar de Alta una Inutilizacion de Folio. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Recibos
        ///DESCRIPCIÓN: Obtiene los datos de los Pagos.
        ///PARAMETROS:   
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 25/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Recibos(Cls_Ope_Pre_Folios_Inutilizados_Negocio Folios_Negocios)
        {

            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT DISTINCT P." + Ope_Pre_Pagos.Campo_No_Pago + " AS NO_PAGO";
                Mi_SQL = Mi_SQL + ", P." + Ope_Pre_Pagos.Campo_No_Recibo + " AS NUM_RECIBO";
                Mi_SQL = Mi_SQL + ", P." + Ope_Pre_Cajas_Detalles.Campo_Caja_ID + " AS CAJA_ID";
                Mi_SQL = Mi_SQL + ", NVL(T." + Ope_Caj_Turnos.Campo_Usuario_Creo + ", ' ' ) AS CAJERO";
                Mi_SQL = Mi_SQL + ", NVL(P." + Ope_Pre_Pagos.Campo_Fecha + ", SYSDATE) AS FECHA";
                Mi_SQL = Mi_SQL + ", P." + Ope_Pre_Pagos.Campo_Motivo_Cancelacion_Id + " AS MOTIVO_ID";
                Mi_SQL = Mi_SQL + ", CA." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS NO_CAJA";
                Mi_SQL = Mi_SQL + ", MOD." + Cat_Pre_Modulos.Campo_Descripcion  + " AS MODULO";
                Mi_SQL = Mi_SQL + ", M." + Cat_Pre_Motivos.Campo_Nombre + " AS MOTIVO";
                Mi_SQL = Mi_SQL + ", NVL(P." + Ope_Pre_Pagos.Campo_Observaciones + ", ' ') AS OBSERVACIONES";
                Mi_SQL = Mi_SQL + " FROM  " + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos;
                Mi_SQL = Mi_SQL + " P INNER JOIN " + Cat_Pre_Motivos.Tabla_Cat_Pre_Motivos;
                Mi_SQL = Mi_SQL + " M ON " + "M." + Cat_Pre_Motivos.Campo_Motivo_ID + " = P." + Ope_Pre_Pagos.Campo_Motivo_Cancelacion_Id;
                Mi_SQL = Mi_SQL + " INNER JOIN " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos;
                Mi_SQL = Mi_SQL + " T ON " + "P." + Ope_Pre_Pagos.Campo_Caja_Id + " = T." + Ope_Caj_Turnos.Campo_Caja_Id;
                Mi_SQL = Mi_SQL + " AND P.NO_TURNO" + " = T." + Ope_Caj_Turnos.Campo_No_Turno;
                Mi_SQL = Mi_SQL + " INNER JOIN " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja;
                Mi_SQL = Mi_SQL + " CA ON " + "P." + Ope_Pre_Pagos.Campo_Caja_Id + " = CA." + Cat_Pre_Cajas.Campo_Caja_ID;
                Mi_SQL = Mi_SQL + " INNER JOIN " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo;
                Mi_SQL = Mi_SQL + " MOD ON " + "MOD." + Cat_Pre_Modulos.Campo_Modulo_Id  + " = CA." + Cat_Pre_Cajas.Campo_Modulo_Id ;
                Mi_SQL = Mi_SQL + " WHERE  P." + Ope_Pre_Pagos.Campo_Estatus  + " = 'INUTILIZADO'";
                if (!string.IsNullOrEmpty(Folios_Negocios.P_Caja_ID))
                {
                    Mi_SQL = Mi_SQL + " AND  P." + Ope_Pre_Pagos.Campo_Caja_Id + " = '" + Folios_Negocios.P_Caja_ID + "'";
                }
                if (!string.IsNullOrEmpty(Folios_Negocios.P_Empleado_ID))
                {
                    Mi_SQL = Mi_SQL + " AND  P." + Ope_Pre_Pagos.Campo_Empleado_Id + " = '" + Folios_Negocios.P_Empleado_ID + "'";
                }
                if (!string.IsNullOrEmpty(Folios_Negocios.P_No_Turno))
                {
                    Mi_SQL = Mi_SQL + " AND  P." + Ope_Caj_Pagos.Campo_No_Turno + " = '" + Folios_Negocios.P_No_Turno + "'";
                }
                if (!string.IsNullOrEmpty(Folios_Negocios.P_Fecha)) {
                    Mi_SQL = Mi_SQL + " AND P." + Ope_Pre_Pagos.Campo_Fecha + " BETWEEN TO_DATE ('" + String.Format("{0:dd-MM-yyy}", Folios_Negocios.P_Fecha) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')";
                    Mi_SQL = Mi_SQL + " AND TO_DATE ('" + String.Format("{0:dd-MM-yyy}", Folios_Negocios.P_Fecha) + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')";
                    Mi_SQL = Mi_SQL + " ORDER BY P." + Ope_Pre_Pagos.Campo_No_Recibo;
                }else{
                     Mi_SQL = Mi_SQL + " ORDER BY P." + Ope_Pre_Pagos.Campo_No_Pago;
                }
               

                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Folios Inutilizados. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Recibos_Busqueda
        ///DESCRIPCIÓN: Obtiene los Folios Inutilizados de la base de datos .
        ///PARAMETROS:   
        ///             1. Folio.   Folios que se van ver a Detalle.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Recibos_Busqueda(Cls_Ope_Pre_Folios_Inutilizados_Negocio Folio)
        {

            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT DISTINCT P." + Ope_Pre_Pagos.Campo_No_Pago + " AS NO_PAGO";
                Mi_SQL = Mi_SQL + ", P." + Ope_Pre_Pagos.Campo_No_Recibo + " AS NUM_RECIBO";
                Mi_SQL = Mi_SQL + ", P." + Ope_Pre_Cajas_Detalles.Campo_Caja_ID + " AS CAJA_ID";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Turnos.Campo_Usuario_Creo + " AS CAJERO";
                Mi_SQL = Mi_SQL + ", P." + Ope_Pre_Pagos.Campo_Fecha + " AS FECHA";
                Mi_SQL = Mi_SQL + ", P." + Ope_Pre_Pagos.Campo_Motivo_Cancelacion_Id + " AS MOTIVO_ID";
                Mi_SQL = Mi_SQL + ", M." + Cat_Pre_Motivos.Campo_Nombre + " AS MOTIVO";
                Mi_SQL = Mi_SQL + ", P." + Ope_Pre_Pagos.Campo_Observaciones + " AS OBSERVACIONES";
                Mi_SQL = Mi_SQL + " FROM  " + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos;
                Mi_SQL = Mi_SQL + " P JOIN " + Cat_Pre_Motivos.Tabla_Cat_Pre_Motivos;
                Mi_SQL = Mi_SQL + " M ON " + "M." + Cat_Pre_Motivos.Campo_Motivo_ID + " = P." + Ope_Pre_Pagos.Campo_Motivo_Cancelacion_Id;
                Mi_SQL = Mi_SQL + " JOIN " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos;
                Mi_SQL = Mi_SQL + " T ON " + "P." + Ope_Pre_Pagos.Campo_Caja_Id + " = T." + Ope_Caj_Turnos.Campo_Caja_Id;
                Mi_SQL = Mi_SQL + " WHERE " + "P." + Ope_Pre_Pagos.Campo_No_Recibo + " LIKE '%" + Folio.P_No_Recibo + "%'";
                Mi_SQL = Mi_SQL + " AND " + "P." + Ope_Pre_Pagos.Campo_Estatus + " = 'INUTILIZADO'";
                Mi_SQL = Mi_SQL + " ORDER BY P." + Ope_Pre_Pagos.Campo_No_Pago;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Folios. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Caja
        ///DESCRIPCIÓN: Obtiene el nombre de las Cajas de acuerdo al Cajero al que esta Asignada
        ///PARAMETROS:   
        ///             1. Folio.   Nombre del Cajero al que pertenece la Caja.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 25/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static String Consultar_Caja(Cls_Ope_Pre_Folios_Inutilizados_Negocio Folio)
        {
            String Mi_SQL = string.Empty;
            String Id = "";
            try
            {
                Mi_SQL = "SELECT " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_ID;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Empleado_ID + " = '" + Folio.P_Empleado_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Estatus + " <> 'CERRADO'";

                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                {
                    Id = Obj_Temp.ToString();
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Cajas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Id;
        }

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Caja_Empleado
        /// DESCRIPCION : Consulta la caja que tiene abierta el empleado para poder realizar
        ///               la recolección de la misma
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 19-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Caja_Empleado(Cls_Ope_Pre_Folios_Inutilizados_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.

            try
            {
                //Consulta los datos generales de la caja que tiene abierta el empleado que requiere realizar la recolección del dinero
                Mi_SQL.Append("SELECT " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS Caja, ");
                Mi_SQL.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_Id + ", ");
                Mi_SQL.Append("(" + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Clave);
                Mi_SQL.Append("||' '||" + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Descripcion + ") AS Modulo, ");
                Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno);
                Mi_SQL.Append(" FROM " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + ", " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + ", " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo);
                Mi_SQL.Append(" WHERE " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id + " = " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_Id);
                Mi_SQL.Append(" AND " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Estatus + " = 'ABIERTO'");
                Mi_SQL.Append(" AND " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Modulo_Id + "=");
                Mi_SQL.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Modulo_Id);
                Mi_SQL.Append(" AND " + Ope_Caj_Turnos.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'");
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Motivos
        ///DESCRIPCIÓN: Obtiene los Motivos por los cuales se puede volver Inutilizable un Folio
        ///PARAMETROS:   
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 25/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Motivos()
        {

            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Motivos.Campo_Motivo_ID ;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Motivos.Campo_Nombre ;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Motivos.Tabla_Cat_Pre_Motivos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Motivos.Campo_Estatus + " = 'VIGENTE'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Motivos.Campo_Motivo_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Mottivos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Folios_Utilizados
        ///DESCRIPCIÓN: los folios utilizados
        ///PARAMETROS:   
        ///CREO: leslie Gonzalez Vazquez.
        ///FECHA_CREO: 11/Octubre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Folios_Utilizados(Cls_Ope_Pre_Folios_Inutilizados_Negocio Datos)
        {
            String Mi_SQL = string.Empty;
            DataTable Tabla = new DataTable();
            try
            {
                Mi_SQL = "SELECT " + Ope_Caj_Pagos.Campo_No_Recibo + " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                Mi_SQL += " WHERE " + Ope_Caj_Pagos.Campo_No_Recibo + " >= " + Datos.P_No_Recibo;
                Mi_SQL += " AND " + Ope_Caj_Pagos.Campo_No_Recibo + " <= " + Datos.P_No_Folio_Fin;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de los folios. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS:     
        ///             1. Tabla: Tabla de referencia a la que se sacara el ultimo ID
        ///             2. Campo: Compo al que se le asignara la ultima ID
        ///             3. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 15/Julio/2011 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static String Obtener_ID_Consecutivo(ref OracleCommand Cmmd, String Tabla, String Campo, Int32 Longitud_ID)
        {
            String Id = string.Empty; 
            try
            {
                String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla;
                Cmmd.CommandText = Mi_SQL;
                Object Obj_Temp = Cmmd.ExecuteOracleScalar().ToString();
                Id = String.Format("{0:0000000000}", Convert.ToInt32(Obj_Temp) + 1);
                ////Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                //if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                //{
                //    Id = Convertir_A_Formato_ID((Convert.ToInt32(Obj_Temp) + 1), Longitud_ID);
                //}
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
        ///PARÁMETROS:     
        ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
        ///             2. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 15/Julio/2011 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
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