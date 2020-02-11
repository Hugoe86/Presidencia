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
using Presidencia.Operacion_Modifica_Folio_Pago.Negocio;
using System.Text;

namespace Presidencia.Operacion_Modifica_Folio_Pago.Datos{
    
    public class Cls_Ope_Pre_Modifica_Folio_Pago_Datos {

       
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Colonia
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nueva Colonia
        ///PARAMETROS:     
        ///             1. Colonia.  Instancia de la Clase de Negocio de Colonias con los datos 
        ///                          de la Colonia que va a ser dada de Alta.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 18/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Modificacion(Cls_Ope_Pre_Modifica_Folio_Pago_Negocio Modificacion)
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
                String Modificacion_ID = Obtener_ID_Consecutivo(Ope_Pre_Modifica_Folio.Tabla_Ope_Pre_Modifica_Folio, Ope_Pre_Modifica_Folio.Campo_Modifica_ID, 10);
                String Mi_SQL = "INSERT INTO " + Ope_Pre_Modifica_Folio.Tabla_Ope_Pre_Modifica_Folio;
                Mi_SQL = Mi_SQL + " (" + Ope_Pre_Modifica_Folio.Campo_Modifica_ID + ", " + Ope_Pre_Modifica_Folio.Campo_No_Pago_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Modifica_Folio.Campo_Folio_Actual + ", " + Ope_Pre_Modifica_Folio.Campo_Folio_Nuevo;
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Modifica_Folio.Campo_Motivo;
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Modifica_Folio.Campo_Usuario_Creo + ", " + Ope_Pre_Modifica_Folio.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Modificacion_ID + "', '" + Modificacion.P_No_Pago_ID + "'";
                Mi_SQL = Mi_SQL + ",'" + Modificacion.P_Folio_Actual + "'";
                Mi_SQL = Mi_SQL + ",'" + Modificacion.P_Folio_Nuevo + "'";
                Mi_SQL = Mi_SQL + ",'" + Modificacion.P_Motivo + "'";
                Mi_SQL = Mi_SQL + ",'" + Modificacion.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", SYSDATE";
                Mi_SQL = Mi_SQL + ")";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
               

                Mi_SQL = "UPDATE " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                Mi_SQL = Mi_SQL + " SET " + Ope_Caj_Pagos.Campo_No_Recibo + " = '" + Modificacion.P_Folio_Nuevo + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Caj_Pagos.Campo_Usuario_Modifico + " = '" + Modificacion.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Caj_Pagos.Campo_Fecha_Modifico + " = sysdate";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Pagos.Campo_No_Pago + " = '" + Modificacion.P_No_Pago_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();


            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
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
                    Mensaje = "Error al intentar dar de Alta una Modificacion de Folio de Pago. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Nombre
        ///DESCRIPCIÓN: Obtiene el nombre de la Colonia solicitada.
        ///PARAMETROS:   
        ///             1. Colonia.   Nombre que se va ver a Detalle.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Folios(Cls_Ope_Pre_Modifica_Folio_Pago_Negocio Datos)
        {

            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT P." + Ope_Caj_Pagos.Campo_No_Pago + " AS NO_PAGO";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_No_Recibo + " AS FOLIO ";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_Documento + " AS DOCUMENTO";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_Fecha + " AS FECHA";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_No_Operacion+ " AS OPERACION";
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Cajas.Campo_Clave + " AS CAJA";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_Clave_Banco + " AS CLAVE_BANCO";  
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_Monto_Corriente + " AS CORRIENTE";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_Monto_Rezago + " AS REZAGOS";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_Monto_Recargos + " AS RECARGOS_ORDINARIOS";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_Monto_Recargos_Moratorios + " AS RECARGOS_MORATORIOS";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_Honorarios + " AS HONORARIOS";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_Multas + " AS MULTAS";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_Descuento_Recargos + " AS DESCUENTO";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_Total + " AS TOTAL";
                Mi_SQL = Mi_SQL + " FROM  " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                Mi_SQL = Mi_SQL + " P LEFT JOIN " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja;
                Mi_SQL = Mi_SQL + " C ON " + "C." + Cat_Pre_Cajas.Campo_Caja_ID + " = P." + Ope_Caj_Pagos.Campo_Caja_ID;
                Mi_SQL = Mi_SQL + " WHERE P." + Ope_Caj_Pagos.Campo_No_Turno + " = '" + Datos.P_No_Turno + "'";
                Mi_SQL = Mi_SQL + " ORDER BY P." + Ope_Caj_Pagos.Campo_No_Operacion;
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Nombre
        ///DESCRIPCIÓN: Obtiene el nombre de la Colonia solicitada.
        ///PARAMETROS:   
        ///             1. Colonia.   Nombre que se va ver a Detalle.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Folios_Busqueda(Cls_Ope_Pre_Modifica_Folio_Pago_Negocio Folio)
        {

            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT P." + Ope_Caj_Pagos.Campo_No_Pago + " AS NO_PAGO";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_No_Recibo + " AS FOLIO ";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_Documento + " AS DOCUMENTO";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_Fecha + " AS FECHA";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_No_Operacion + " AS OPERACION";
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Cajas.Campo_Clave + " AS CAJA";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_Clave_Banco + " AS CLAVE_BANCO";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_Total + " AS CORRIENTE";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_Monto_Rezago + " AS REZAGOS";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_Monto_Recargos + " AS RECARGOS_ORDINARIOS";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_Monto_Recargos_Moratorios + " AS RECARGOS_MORATORIOS";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_Honorarios + " AS HONORARIOS";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_Multas + " AS MULTAS";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_Descuento_Recargos + " AS DESCUENTO";
                //Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_Monto + " AS MONTO";
                Mi_SQL = Mi_SQL + " FROM  " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                Mi_SQL = Mi_SQL + " P LEFT JOIN " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja;
                Mi_SQL = Mi_SQL + " C ON " + "C." + Cat_Pre_Cajas.Campo_Caja_Id + " = P." + Ope_Caj_Pagos.Campo_Caja_ID;
                if (Folio.P_Recibo != "")
                {
                    Mi_SQL = Mi_SQL + " WHERE P." + Ope_Caj_Pagos.Campo_No_Recibo + " = " + Folio.P_Recibo;
                }
                Mi_SQL = Mi_SQL + " AND P." + Ope_Caj_Pagos.Campo_No_Turno + " = '" + Folio.P_No_Turno + "'";
                Mi_SQL = Mi_SQL + " ORDER BY P." + Ope_Caj_Pagos.Campo_No_Pago;
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Nombre
        ///DESCRIPCIÓN: Obtiene el nombre de la Colonia solicitada.
        ///PARAMETROS:   
        ///             1. Colonia.   Nombre que se va ver a Detalle.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 05/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Encontrar_Folios(Cls_Ope_Pre_Modifica_Folio_Pago_Negocio Folio)
        {

            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Ope_Caj_Pagos.Campo_No_Pago + " AS NO_PAGO";
                Mi_SQL = Mi_SQL + " FROM  " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Pagos.Campo_No_Recibo + " = '" + Folio.P_Recibo + "'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Caj_Pagos.Campo_No_Pago;
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
        public static DataTable Consulta_Caja_Empleado(Cls_Ope_Pre_Modifica_Folio_Pago_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.

            try
            {
                //Consulta los datos generales de la caja que tiene abierta el empleado que requiere realizar la recolección del dinero
                Mi_SQL.Append("SELECT (" + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Clave);
                Mi_SQL.Append("||' '||" + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + ") AS Caja, ");
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
    }
}