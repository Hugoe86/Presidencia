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
using Presidencia.Sessiones;
using Presidencia.Operacion_Caj_Cierre_Dia.Negocio;
using System.Text;

/// <summary>
/// Summary description for Cls_Ope_Caj_Cierre_Dia_Datos
/// </summary>

namespace Presidencia.Operacion_Caj_Cierre_Dia.Datos
{
    public class Cls_Ope_Caj_Cierre_Dia_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Cierre
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nueva caja
        ///PARAMENTROS:     
        ///             1. Cierre.            Instancia de la Clase de Negocio de Cierre de día 
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 07/Septiembre/2011 
        ///MODIFICO: 
        ///FECHA_MODIFICO 
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
        public static void Alta_Cierre(Cls_Ope_Caj_Cierre_Dia_Negocio Cierre)
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
            StringBuilder Query = new StringBuilder();
            object No_Cierre_Dia = null;

            try
            {
                //Consultas para el ID
                Query.Append("SELECT NVL(MAX(" + Ope_Caj_Cierre_Dia.Campo_No_Cierre_Dia + "), '0000000000') FROM " + Ope_Caj_Cierre_Dia.Tabla_Ope_Caj_Cierre_Dia);

                //Ejecutar consulta
                Cmd.CommandText = Query.ToString();
                No_Cierre_Dia = Cmd.ExecuteScalar();

                //Verificar si no es nulo
                if (Convert.IsDBNull(No_Cierre_Dia) == false)
                    Cierre.P_No_Cierre_Dia = String.Format("{0:0000000000}", Convert.ToInt32(No_Cierre_Dia) + 1);
                else
                    Cierre.P_No_Cierre_Dia = "0000000001";

                Query = new StringBuilder();

                Query.Append("INSERT INTO ");
                Query.Append(Ope_Caj_Cierre_Dia.Tabla_Ope_Caj_Cierre_Dia);
                Query.Append(" (");
                Query.Append(Ope_Caj_Cierre_Dia.Campo_No_Cierre_Dia + ", ");
                Query.Append(Ope_Caj_Cierre_Dia.Campo_Fecha_Cierre_Dia + ", ");
                Query.Append(Ope_Caj_Cierre_Dia.Campo_Estatus + ", ");
                Query.Append(Ope_Caj_Cierre_Dia.Campo_Modulo_Id + ", ");
                Query.Append(Ope_Caj_Cierre_Dia.Campo_Usuario_Creo + ", ");
                Query.Append(Ope_Caj_Cierre_Dia.Campo_Fecha_Creo);
                Query.Append(") VALUES(");
                Query.Append("'" + Cierre.P_No_Cierre_Dia + "', ");
                Query.Append("'" + String.Format("{0:dd/MM/yyyy}", Cierre.P_Fecha_Cierre_Dia) + "', ");
                Query.Append("'" + Cierre.P_Estatus + "', ");
                Query.Append("'" + Cierre.P_Modulo_Id + "', ");
                Query.Append("'" + Cierre.P_Usuario + "', SYSDATE)");

                Cmd.CommandText = Query.ToString();
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Cierre de día. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Cn.State == ConnectionState.Open)
                {
                    Cn.Close();
                }
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Cierre
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Cierre
        ///PARAMENTROS:     
        ///             1. Cierre.            Instancia de la Clase de Cierres de día 
        ///                                 con los datos del Registro 
        ///                                 que va a ser Actualizado.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 07/Septiembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Cierre(Cls_Ope_Caj_Cierre_Dia_Negocio Cierre)
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
                String Mi_SQL = "UPDATE " + Ope_Caj_Cierre_Dia.Tabla_Ope_Caj_Cierre_Dia;
                Mi_SQL = Mi_SQL + " SET " + Ope_Caj_Cierre_Dia.Campo_Estatus + " = '" + Cierre.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Caj_Cierre_Dia.Campo_Fecha_Cierre_Dia + " = '" + String.Format("{0:dd/MM/yyyy}", Cierre.P_Fecha_Cierre_Dia) + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Caj_Cierre_Dia.Campo_Modulo_Id + " = '" + Cierre.P_Modulo_Id + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Caj_Cierre_Dia.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Caj_Cierre_Dia.Campo_Fecha_Modifico + " = sysdate";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Cierre_Dia.Campo_No_Cierre_Dia + " = '" + Cierre.P_No_Cierre_Dia + "'";
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
                        Mensaje = "Error general en la base de datos";
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
                    Mensaje = "Error al intentar modificar un Registro de Cierre de día. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Cierre_Dia
        ///DESCRIPCIÓN: Ciere el turno general del día que fue abierto por el primero cajero
        ///             que fue quien abrio el turno en su caja
        ///PARAMETROS : Datos: Obtiene los valores a registrar en la base de datos
        ///CREO       : Yazmin Delgado Gómez
        ///FECHA_CREO : 25-Octubre-2011
        ///MODIFICO          :
        ///FECHA_MODIFICO    : 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************
        public static void Modificar_Cierre_Dia(Cls_Ope_Caj_Cierre_Dia_Negocio Datos)
        {
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;
            StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la cadena de inserción hacía la base de datos

            //inicializa la conexion
            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(); //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;             //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;

            try
            {
                //Actualiza los datos del turno que fueron introducidos para su cierre
                Mi_SQL.Append("UPDATE " + Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia);
                Mi_SQL.Append(" SET " + Ope_Caj_Turnos_Dia.Campo_Fecha_Cierre + " = '" + String.Format("{0:dd/MM/yyyy}", DateTime.Today) + "', ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Campo_Hora_Cierre + " = SYSDATE, ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Campo_Estatus + " = 'CERRADO', ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario + "', ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Campo_Fecha_Modifico + " = SYSDATE");
                Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos_Dia.Campo_No_Turno + " = '" + Datos.P_No_Cierre_Dia + "'");

                Comando_SQL.CommandText = Mi_SQL.ToString();
                Comando_SQL.ExecuteNonQuery();

                Transaccion_SQL.Commit();
            }
            catch (OracleException Ex)
            {
                Transaccion_SQL.Rollback();
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                Transaccion_SQL.Rollback();
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                Transaccion_SQL.Rollback();
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Conexion_Base.Close();
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cierres
        ///DESCRIPCIÓN: Obtiene todos los Cierres que estan dadas de 
        ///             alta en la Base de Datos
        ///PARAMENTROS:   
        ///             1.  Cierre.           Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                                 caso son las fechas y la descripción del módulo.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 07/Septiembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO 
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
        public static DataTable Consultar_Cierres(Cls_Ope_Caj_Cierre_Dia_Negocio Cierre)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT c." + Ope_Caj_Cierre_Dia.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Cierre_Dia.Campo_Fecha_Cierre_Dia + "";
                Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Cierre_Dia.Campo_Fecha_Reapertura_Dia + "";
                Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Cierre_Dia.Campo_Modulo_Id + "";
                Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Cierre_Dia.Campo_No_Cierre_Dia + "";
                //Mi_SQL = Mi_SQL + ", (select m." + Cat_Pre_Modulos.Campo_Descripcion + " from " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + " m where c." + Ope_Caj_Cierre_Dia.Campo_Modulo_Id + "=m." + Cat_Pre_Modulos.Campo_Modulo_Id + ") as MODULO";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Cierre_Dia.Tabla_Ope_Caj_Cierre_Dia + " c LEFT JOIN ";
                Mi_SQL = Mi_SQL + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + " m ON c." + Ope_Caj_Cierre_Dia.Campo_Modulo_Id + "=m." + Cat_Pre_Modulos.Campo_Modulo_Id;
                if (Cierre.P_Filtro.Trim().Length == 0)
                {
                    Mi_SQL = Mi_SQL + " WHERE (c." + Ope_Caj_Cierre_Dia.Campo_Fecha_Cierre_Dia + " LIKE '%" + Cierre.P_Filtro + "%' OR c." + Ope_Caj_Cierre_Dia.Campo_Fecha_Reapertura_Dia + " LIKE '%" + Cierre.P_Filtro + "%') OR ";
                    Mi_SQL = Mi_SQL + " m." + Cat_Pre_Modulos.Campo_Descripcion + " LIKE '%" + Cierre.P_Filtro + "%'";
                }
                Mi_SQL = Mi_SQL + " ORDER BY c." + Ope_Caj_Cierre_Dia.Campo_Fecha_Cierre_Dia;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Cierres de día. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Cierres
        ///DESCRIPCIÓN: Obtiene a detalle un cierre de dia.
        ///PARAMENTROS:   
        ///             1. P_Cierre.   Cierre de dia que se va ver a Detalle.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 07/Septiembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Ope_Caj_Cierre_Dia_Negocio Consultar_Datos_Cierres(Cls_Ope_Caj_Cierre_Dia_Negocio P_Cierre)
        {
            Cls_Ope_Caj_Cierre_Dia_Negocio R_Cierre = new Cls_Ope_Caj_Cierre_Dia_Negocio();
            String Mi_SQL = "SELECT c." + Ope_Caj_Cierre_Dia.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Cierre_Dia.Campo_Fecha_Cierre_Dia;
            Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Cierre_Dia.Campo_Modulo_Id;
            Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Cierre_Dia.Campo_No_Cierre_Dia;
            Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Cierre_Dia.Campo_Fecha_Reapertura_Dia;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Cierre_Dia.Tabla_Ope_Caj_Cierre_Dia + " c ";
            Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + " m ON ";
            Mi_SQL = Mi_SQL + "c." + Ope_Caj_Cierre_Dia.Campo_Modulo_Id + "=m." + Cat_Pre_Modulos.Campo_Modulo_Id;
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Cierre_Dia.Campo_No_Cierre_Dia + " = '" + P_Cierre.P_No_Cierre_Dia + "'";
            OracleDataReader Data_Reader;
            try
            {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Cierre.P_No_Cierre_Dia = P_Cierre.P_No_Cierre_Dia;
                while (Data_Reader.Read())
                {
                    R_Cierre.P_Estatus = Data_Reader[Ope_Caj_Cierre_Dia.Campo_Estatus].ToString();
                    R_Cierre.P_Fecha_Cierre_Dia = Convert.ToDateTime(Data_Reader[Ope_Caj_Cierre_Dia.Campo_Fecha_Cierre_Dia].ToString());
                    R_Cierre.P_Modulo_Id = Data_Reader[Ope_Caj_Cierre_Dia.Campo_Modulo_Id].ToString();
                    R_Cierre.P_No_Cierre_Dia = Data_Reader[Ope_Caj_Cierre_Dia.Campo_No_Cierre_Dia].ToString();
                    R_Cierre.P_Fecha_Reapertura_Dia = Convert.ToDateTime(Data_Reader[Ope_Caj_Cierre_Dia.Campo_Fecha_Reapertura_Dia].ToString());
                }
                Data_Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el registro de Cierres de Turnos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Cierre;
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
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cajas_Abiertas
        ///DESCRIPCIÓN: Obtiene todos los Cierres que estan dadas de 
        ///             alta en la Base de Datos
        ///PARAMENTROS:   
        ///             1.  Cierre.           Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                                 caso son las fechas y la descripción del módulo.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 07/Septiembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO 
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
        public static DataTable Consultar_Cajas_Abiertas(Cls_Ope_Caj_Cierre_Dia_Negocio Cierre)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT c." + Ope_Caj_Turnos.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Cierre_Dia.Campo_Fecha_Cierre_Dia + "";
                Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Cierre_Dia.Campo_Fecha_Reapertura_Dia + "";
                Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Cierre_Dia.Campo_Modulo_Id + "";
                Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Cierre_Dia.Campo_No_Cierre_Dia + "";
                //Mi_SQL = Mi_SQL + ", (select m." + Cat_Pre_Modulos.Campo_Descripcion + " from " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + " m where c." + Ope_Caj_Cierre_Dia.Campo_Modulo_Id + "=m." + Cat_Pre_Modulos.Campo_Modulo_Id + ") as MODULO";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Cierre_Dia.Tabla_Ope_Caj_Cierre_Dia + " c LEFT JOIN ";
                Mi_SQL = Mi_SQL + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + " m ON c." + Ope_Caj_Cierre_Dia.Campo_Modulo_Id + "=m." + Cat_Pre_Modulos.Campo_Modulo_Id;
                if (Cierre.P_Filtro.Trim().Length == 0)
                {
                    Mi_SQL = Mi_SQL + " WHERE (c." + Ope_Caj_Cierre_Dia.Campo_Fecha_Cierre_Dia + " LIKE '%" + Cierre.P_Filtro + "%' OR c." + Ope_Caj_Cierre_Dia.Campo_Fecha_Reapertura_Dia + " LIKE '%" + Cierre.P_Filtro + "%') OR ";
                    Mi_SQL = Mi_SQL + " m." + Cat_Pre_Modulos.Campo_Descripcion + " LIKE '%" + Cierre.P_Filtro + "%'";
                }
                Mi_SQL = Mi_SQL + " ORDER BY c." + Ope_Caj_Cierre_Dia.Campo_Fecha_Cierre_Dia;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Cierres de día. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Movimientos_Cajas
        ///DESCRIPCIÓN: Obtiene los movimientos de una caja seleccionada.
        ///PARAMENTROS:   
        ///             1.  Cierre.           Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                                 caso son la fecha del día consultado y la el id de la caja.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 07/Septiembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO 
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
        public static DataTable Consultar_Movimientos_Cajas(Cls_Ope_Caj_Cierre_Dia_Negocio Cierre)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT c." + Ope_Caj_Pagos.Campo_No_Recibo + " AS RECIBO";
                Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Pagos.Campo_Folio + " AS FOLIO";
                Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Pagos.Campo_Fecha + " AS FECHA";
                Mi_SQL = Mi_SQL + ", TO_NUMBER (c." + Ope_Caj_Pagos.Campo_No_Operacion + ") AS NO_OPERACION";
                Mi_SQL = Mi_SQL + ", cu." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " AS CUENTA_PREDIAL";
                Mi_SQL = Mi_SQL + ", mov." + Cat_Pre_Movimientos.Campo_Descripcion + " AS MOVIMIENTO";
                Mi_SQL = Mi_SQL + ", mov." + Cat_Pre_Movimientos.Campo_Identificador + " AS CLAVE";
                //Mi_SQL = Mi_SQL + ", de." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + " AS TIPO_OPERACION";
                Mi_SQL = Mi_SQL + ", pa." + Ope_Ing_Pasivo.Campo_Monto + " AS MONTO";
                Mi_SQL = Mi_SQL + ", ca." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS CAJA";
                Mi_SQL = Mi_SQL + ", mo." + Cat_Pre_Modulos.Campo_Descripcion + " AS MODULO";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " c LEFT JOIN ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " cu ON c." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + "=cu." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;

                Mi_SQL = Mi_SQL + " RIGHT JOIN " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " pa ON c." + Ope_Caj_Pagos.Campo_No_Pago + "=pa." + Ope_Caj_Pagos.Campo_No_Pago;

                //Mi_SQL = Mi_SQL + " LEFT JOIN " +Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + " de ON c." + Ope_Caj_Pagos.Campo_No_Pago + "=de." + Ope_Caj_Pagos_Detalles.Campo_No_Pago;
                Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + " ca ON c." + Ope_Caj_Pagos.Campo_Caja_ID + "=ca." + Cat_Pre_Cajas.Campo_Numero_De_Caja;
                Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + " mo ON ca." + Cat_Pre_Cajas.Campo_Modulo_Id + "=mo." + Cat_Pre_Modulos.Campo_Modulo_Id;
                Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + " cla ON c." + Ope_Caj_Pagos.Campo_Clave_Ingreso_ID + "=cla." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID;
                Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det + " cla_det ON cla." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + "=cla_det." + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID;
                Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " mov ON cla_det." + Cat_Pre_Claves_Ingreso_Det.Campo_Movimiento_ID + "=mov." + Cat_Pre_Movimientos.Campo_Movimiento_ID;
                Mi_SQL = Mi_SQL + " WHERE ";

                //Mi_SQL = Mi_SQL + "de." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + " <> 'CAMBIO'";
                //Mi_SQL = Mi_SQL + " AND ";
                //Mi_SQL = Mi_SQL + "de." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + " <> 'AJUSTE TARIFARIO'";
                //Mi_SQL = Mi_SQL + " AND ";

                if (!String.IsNullOrEmpty(Cierre.P_Caja_Id))
                {
                    Mi_SQL = Mi_SQL + " c." + Ope_Caj_Pagos.Campo_Caja_ID + "='" + Cierre.P_Caja_Id + "' AND ";
                }

                //if (!String.IsNullOrEmpty(Cierre.P_Modulo_Id))
                //{
                //    Mi_SQL = Mi_SQL + " mo." + Cat_Pre_Modulos.Campo_Modulo_Id + "='" + Cierre.P_Modulo_Id + "' AND ";
                //}

                Mi_SQL = Mi_SQL + " c." + Ope_Caj_Pagos.Campo_Fecha + " BETWEEN TO_DATE ('" + String.Format("{0:d-M-yyyy}", DateTime.Now) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS') AND TO_DATE ('" + String.Format("{0:d-M-yyyy}", DateTime.Now) + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";

                Mi_SQL = Mi_SQL + " ORDER BY c." + Ope_Caj_Pagos.Campo_Fecha + " DESC";

                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Cierres de día. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Concentrado_Cajas
        ///DESCRIPCIÓN: Obtiene los movimientos de todas las cajas
        ///PARAMENTROS:   
        ///             1.  Cierre.           Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                                 caso son las fechas y la descripción del módulo.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 07/Septiembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO 
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
        public static DataTable Consultar_Concentrado_Cajas(Cls_Ope_Caj_Cierre_Dia_Negocio Cierre)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT c." + Ope_Caj_Pagos.Campo_No_Recibo + " AS RECIBO";
                Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Pagos.Campo_Folio + " AS FOLIO";
                Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Pagos.Campo_Fecha + " AS FECHA";
                Mi_SQL = Mi_SQL + ", cu." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " AS CUENTA_PREDIAL";
                Mi_SQL = Mi_SQL + ", mov." + Cat_Pre_Movimientos.Campo_Descripcion + " AS MOVIMIENTO";
                Mi_SQL = Mi_SQL + ", mov." + Cat_Pre_Movimientos.Campo_Identificador + " AS CLAVE";
                Mi_SQL = Mi_SQL + ", de." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + " AS TIPO_OPERACION";
                Mi_SQL = Mi_SQL + ", de." + Ope_Caj_Pagos_Detalles.Campo_Monto + " AS MONTO";
                Mi_SQL = Mi_SQL + ", ca." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS CAJA";
                Mi_SQL = Mi_SQL + ", mo." + Cat_Pre_Modulos.Campo_Descripcion + " AS MODULO";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " c LEFT JOIN ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " cu ON c." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + "=cu." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                Mi_SQL = Mi_SQL + " LEFT JOIN " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + " de ON c." + Ope_Caj_Pagos.Campo_No_Pago + "=de." + Ope_Caj_Pagos_Detalles.Campo_No_Pago;
                Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + " ca ON c." + Ope_Caj_Pagos.Campo_Caja_ID + "=ca." + Cat_Pre_Cajas.Campo_Numero_De_Caja;
                Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + " mo ON ca." + Cat_Pre_Cajas.Campo_Modulo_Id + "=mo." + Cat_Pre_Modulos.Campo_Modulo_Id;
                Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + " cla ON c." + Ope_Caj_Pagos.Campo_Clave_Ingreso_ID + "=cla." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID;
                Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det + " cla_det ON cla." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + "=cla_det." + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID;
                Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " mov ON cla_det." + Cat_Pre_Claves_Ingreso_Det.Campo_Movimiento_ID + "=mov." + Cat_Pre_Movimientos.Campo_Movimiento_ID;
                Mi_SQL = Mi_SQL + " WHERE ";
                if (Cierre.P_Caja_Id != null && Cierre.P_Caja_Id != "")
                {
                    Mi_SQL = Mi_SQL + "c." + Ope_Caj_Pagos.Campo_Caja_ID + "='" + Cierre.P_Caja_Id + "' AND ";
                }

                Mi_SQL = Mi_SQL + "c." + Ope_Caj_Pagos.Campo_Fecha + "='" + String.Format("{0:d-M-yyyy}", DateTime.Now) + "'";
                Mi_SQL = Mi_SQL + " ORDER BY c." + Ope_Caj_Pagos.Campo_Fecha + " DESC";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Cierres de día. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Datos_Cierre_Dia
        /// DESCRIPCION : Consulta los Datos generales del Cierre del Día que fue 
        ///               proporcionado por el usuario
        /// PARAMETROS  : Datos: Son los parámetros de consulta para el filtro de valores
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 25-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Datos_Cierre_Dia(Cls_Ope_Caj_Cierre_Dia_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable que va a contener la consulta de los datos
            try
            {
                Mi_SQL.Append("SELECT * FROM " + Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia);
                Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos_Dia.Campo_Fecha_Turno);
                Mi_SQL.Append(" BETWEEN TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Filtro)) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                Mi_SQL.Append(" AND TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Filtro)) + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')");
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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
        /// NOMBRE DE LA FUNCION: Consulta_Turnos_Abiertos
        /// DESCRIPCION : Consulta los turnos que aun se puedan encontrar abiertos y que
        ///               pertenecescan al No. Turno de Dia que se esta consultando
        /// PARAMETROS  : Datos: Son los parámetros de consulta para el filtro de valores
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 25-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Turnos_Abiertos(Cls_Ope_Caj_Cierre_Dia_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable que va a contener la consulta de los datos
            try
            {
                Mi_SQL.Append("SELECT " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Fecha_Turno + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Usuario_Creo + " AS Cajero, ");
                Mi_SQL.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS Caja, ");
                Mi_SQL.Append("(" + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Clave);
                Mi_SQL.Append(" ||' '|| " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Ubicacion + ") AS Modulo");
                Mi_SQL.Append(" FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + ", " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + ", " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas);
                Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Estatus + " = 'ABIERTO'");
                Mi_SQL.Append(" AND " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno_Dia + " = '" + Datos.P_No_Cierre_Dia + "'");
                Mi_SQL.Append(" AND " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_ID);
                Mi_SQL.Append(" = " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Caja_ID);
                Mi_SQL.Append(" AND " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Modulo_Id);
                Mi_SQL.Append(" = " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Modulo_Id);
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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
        /// NOMBRE DE LA FUNCION: Consultar_Formas_Pago_Turno_Dia
        /// DESCRIPCION : Consulta el monto total que se tuvo durante el turno del día de 
        ///               las diferentes formas de pago de todos los turnos abiertos
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 26-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Formas_Pago_Turno_Dia(Cls_Ope_Caj_Cierre_Dia_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable que va a contener la consulta de los datos
            try
            {
                Mi_SQL.Append("WITH PAGOS_CANCELADOS AS (");
                Mi_SQL.Append("SELECT PAGOS_1." + Ope_Caj_Pagos.Campo_No_Recibo);
                Mi_SQL.Append(" ,PAGOS_1." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID);
                Mi_SQL.Append(" FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " PASIVOS");
                Mi_SQL.Append(" ," + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " PAGOS_1");
                Mi_SQL.Append(" ," + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " PAGOS_2");
                Mi_SQL.Append(" ," + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " CAJA");
                Mi_SQL.Append(" WHERE PAGOS_1." + Ope_Caj_Pagos.Campo_No_Pago + " = PASIVOS." + Ope_Ing_Pasivo.Campo_No_Pago);
                Mi_SQL.Append(" AND PAGOS_1." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " = PAGOS_2." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID);
                Mi_SQL.Append(" AND PAGOS_1." + Ope_Caj_Pagos.Campo_No_Recibo + " = PAGOS_2." + Ope_Caj_Pagos.Campo_No_Recibo);
                Mi_SQL.Append(" AND (PAGOS_1." + Ope_Caj_Pagos.Campo_Caja_ID + " = CAJA." + Cat_Pre_Cajas.Campo_Caja_Id);
                Mi_SQL.Append("   OR PAGOS_2." + Ope_Caj_Pagos.Campo_Caja_ID + " = CAJA." + Cat_Pre_Cajas.Campo_Caja_Id + ")");
                Mi_SQL.Append(" AND PAGOS_1." + Ope_Caj_Pagos.Campo_Estatus + " = 'PAGADO'");
                Mi_SQL.Append(" AND PAGOS_2." + Ope_Caj_Pagos.Campo_Estatus + " = 'CANCELADO'");
                Mi_SQL.Append(" AND PAGOS_1." + Ope_Caj_Pagos.Campo_No_Turno + " = '" + Datos.P_No_Cierre_Dia + "'");
                Mi_SQL.Append(" AND PASIVOS." + Ope_Ing_Pasivo.Campo_Descripcion + " <> 'AJUSTE TARIFARIO'");
                Mi_SQL.Append(") ");

                Mi_SQL.Append("SELECT NVL(SUM(" + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Monto + "), 0) AS Total_Pagado, ");
                Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago);
                Mi_SQL.Append(" FROM " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " PAGOS, ");
                Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + ", " + Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia);
                Mi_SQL.Append(" WHERE PAGOS." + Ope_Caj_Pagos.Campo_No_Pago + " = " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_No_Pago);
                //Mi_SQL.Append(" AND PAGOS." + Ope_Caj_Pagos.Campo_Estatus + " = 'PAGADO'");
                Mi_SQL.Append(" AND PAGOS." + Ope_Caj_Pagos.Campo_No_Turno + " = " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno);
                Mi_SQL.Append(" AND " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno_Dia + " = " + Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia + "." + Ope_Caj_Turnos_Dia.Campo_No_Turno);
                Mi_SQL.Append(" AND " + Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia + "." + Ope_Caj_Turnos_Dia.Campo_No_Turno + " = '" + Datos.P_No_Cierre_Dia + "'");
                Mi_SQL.Append(" AND NOT PAGOS." + Ope_Caj_Pagos.Campo_No_Recibo + " || PAGOS." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " IN (SELECT  PAGOS_CANCELADOS." + Ope_Caj_Pagos.Campo_No_Recibo + " || PAGOS_CANCELADOS." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " FROM PAGOS_CANCELADOS)");
                Mi_SQL.Append(" GROUP BY " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago);
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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
    }
}