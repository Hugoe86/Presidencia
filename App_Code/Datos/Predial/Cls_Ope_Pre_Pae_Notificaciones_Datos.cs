using System;
using System.Text;
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
using Presidencia.Predial_Pae_Notificaciones.Negocio;

namespace Presidencia.Predial_Pae_Notificaciones.Datos
{
    public class Cls_Ope_Pre_Pae_Notificaciones_Datos
    {
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
        ///NOMBRE DE LA FUNCIÓN: Ejecuta_Consulta
        ///DESCRIPCIÓN: Ejecuta la consulta que se acaba de crear
        ///PARÁMETROS:     1. Mi_SQL. Es un String que contiene la consulta que se va a ejecutar
        ///CREO: Armando Zavala Moreno.
        ///FECHA_CREO: 16/Febrero/2012 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        private static void Ejecuta_Consulta(String Mi_SQL)
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
                    Mensaje = "Error al intentar dar de Alta un Registro en Honorarios. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Alta_Notificaciones
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nueva notificacion.
        ///PARAMENTROS:     
        ///CREO: Armando Zavala Moreno
        ///FECHA_CREO: 28/Marzo/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO 
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
        public static void Alta_Notificaciones(Cls_Ope_Pre_Pae_Notificaciones_Negocio Notificacion)
        {
            try
            {
                String No_Notificacion = Obtener_ID_Consecutivo(Ope_Pre_Pae_Notificaciones.Tabla_Ope_Pre_Pae_Notificaciones, Ope_Pre_Pae_Notificaciones.Campo_No_Notificacion, 10);
                String Mi_SQL = "INSERT INTO " + Ope_Pre_Pae_Notificaciones.Tabla_Ope_Pre_Pae_Notificaciones;
                Mi_SQL += " (" + Ope_Pre_Pae_Notificaciones.Campo_No_Notificacion + ", " + Ope_Pre_Pae_Notificaciones.Campo_No_Detalle_Etapa;
                Mi_SQL += ", " + Ope_Pre_Pae_Notificaciones.Campo_Fecha_Hora + ", " + Ope_Pre_Pae_Notificaciones.Campo_Estatus;
                Mi_SQL += ", " + Ope_Pre_Pae_Notificaciones.Campo_Notificador + ", " + Ope_Pre_Pae_Notificaciones.Campo_Recibio;
                Mi_SQL += ", " + Ope_Pre_Pae_Notificaciones.Campo_Acuse_Recibo + ", " + Ope_Pre_Pae_Notificaciones.Campo_Folio;
                Mi_SQL += ", " + Ope_Pre_Pae_Notificaciones.Campo_Medio_Notificacion + ", " + Ope_Pre_Pae_Notificaciones.Campo_Proceso;
                Mi_SQL += ", " + Ope_Pre_Pae_Notificaciones.Campo_Usuario_Creo + ", " + Ope_Pre_Pae_Notificaciones.Campo_Fecha_Creo + ")";
                Mi_SQL += " VALUES ('" + No_Notificacion + "'";
                Mi_SQL += ", '" + Notificacion.P_No_Detalle_Etapa + "'";
                Mi_SQL += ", '" + Notificacion.P_Fecha_Hora + "'";
                Mi_SQL += ", '" + Notificacion.P_Estatus + "'";
                Mi_SQL += ", '" + Notificacion.P_Notificador + "'";
                Mi_SQL += ", '" + Notificacion.P_Recibio + "'";
                Mi_SQL += ", '" + Notificacion.P_Acuse_Recibo + "'";
                Mi_SQL += ", '" + Notificacion.P_Folio + "'";
                Mi_SQL += ", '" + Notificacion.P_Medio_Notificacion + "'";
                Mi_SQL += ", '" + Notificacion.P_Proceso + "'";
                Mi_SQL += ",'" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL += ", sysdate";
                Mi_SQL += ")";
                Ejecuta_Consulta(Mi_SQL);
            }
            catch (OracleException Ex)
            {
                new Exception(Ex.Message);
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Notificacion
        ///DESCRIPCIÓN: Devuelve un registro de la tabla de Notificaciones
        ///PARAMETROS:      
        ///CREO: Armando Zavala Moreno.
        ///FECHA_CREO: 28/Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Notificacion(Cls_Ope_Pre_Pae_Notificaciones_Negocio Notificacion)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;
            try
            {
                if (Notificacion.P_Campos_Dinamicos != null && Notificacion.P_Campos_Dinamicos != "")
                    Mi_SQL = "SELECT " + Notificacion.P_Campos_Dinamicos;
                else
                {
                    Mi_SQL = "SELECT " + Ope_Pre_Pae_Notificaciones.Tabla_Ope_Pre_Pae_Notificaciones + ".*";
                }
                Mi_SQL += ", ( SELECT " + Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id + " FROM " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas;
                Mi_SQL += " WHERE " + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + " = " + Ope_Pre_Pae_Notificaciones.Tabla_Ope_Pre_Pae_Notificaciones + "." + Ope_Pre_Pae_Notificaciones.Campo_No_Detalle_Etapa;
                Mi_SQL += ") CUENTA_PREDIAL_ID";
                Mi_SQL += " FROM " + Ope_Pre_Pae_Notificaciones.Tabla_Ope_Pre_Pae_Notificaciones;
                if (Notificacion.P_Filtro != null && Notificacion.P_Filtro != "")
                {
                    Mi_SQL += " WHERE " + Notificacion.P_Filtro;
                }
                if (Notificacion.P_No_Detalle_Etapa != null && Notificacion.P_No_Detalle_Etapa != "")
                {
                    Mi_SQL += " WHERE " + Ope_Pre_Pae_Notificaciones.Campo_No_Detalle_Etapa + " = " + Notificacion.P_No_Detalle_Etapa;
                }
                if (Notificacion.P_Agrupar_Dinamico != null && Notificacion.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Notificacion.P_Agrupar_Dinamico;
                }
                else
                {
                    Mi_SQL += " ORDER BY " + Ope_Pre_Pae_Notificaciones.Campo_No_Notificacion;
                }
                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null)
                {
                    Tabla = dataSet.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de la Cuentas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Notificacion
        ///DESCRIPCIÓN: Devuelve un registro de la tabla de Notificaciones
        ///PARAMETROS:      
        ///CREO: Armando Zavala Moreno.
        ///FECHA_CREO: 28/Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consulta_Notificacion_Cuenta_Predial(Cls_Ope_Pre_Pae_Notificaciones_Negocio Notificacion)
        {
            DataTable Tabla = null;
            StringBuilder Mi_SQL = new StringBuilder();
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ");
                Mi_SQL.Append(Ope_Pre_Pae_Notificaciones.Tabla_Ope_Pre_Pae_Notificaciones + "." + Ope_Pre_Pae_Notificaciones.Campo_Fecha_Hora + ", ");
                Mi_SQL.Append(Ope_Pre_Pae_Notificaciones.Tabla_Ope_Pre_Pae_Notificaciones + "." + Ope_Pre_Pae_Notificaciones.Campo_Estatus + ", ");
                Mi_SQL.Append(Ope_Pre_Pae_Notificaciones.Tabla_Ope_Pre_Pae_Notificaciones + "." + Ope_Pre_Pae_Notificaciones.Campo_Notificador + ", ");
                Mi_SQL.Append(Ope_Pre_Pae_Notificaciones.Tabla_Ope_Pre_Pae_Notificaciones + "." + Ope_Pre_Pae_Notificaciones.Campo_Recibio + ", ");
                Mi_SQL.Append(Ope_Pre_Pae_Notificaciones.Tabla_Ope_Pre_Pae_Notificaciones + "." + Ope_Pre_Pae_Notificaciones.Campo_Acuse_Recibo + ", ");
                Mi_SQL.Append(Ope_Pre_Pae_Notificaciones.Tabla_Ope_Pre_Pae_Notificaciones + "." + Ope_Pre_Pae_Notificaciones.Campo_Folio + ", ");
                Mi_SQL.Append(Ope_Pre_Pae_Notificaciones.Tabla_Ope_Pre_Pae_Notificaciones + "." + Ope_Pre_Pae_Notificaciones.Campo_Proceso);
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas);
                Mi_SQL.Append(" LEFT OUTER JOIN " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas);
                Mi_SQL.Append(" ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "=" + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id);
                Mi_SQL.Append(" LEFT OUTER JOIN " + Ope_Pre_Pae_Notificaciones.Tabla_Ope_Pre_Pae_Notificaciones);
                Mi_SQL.Append(" ON " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + "=" + Ope_Pre_Pae_Notificaciones.Tabla_Ope_Pre_Pae_Notificaciones + "." + Ope_Pre_Pae_Notificaciones.Campo_No_Detalle_Etapa);

                if (!String.IsNullOrEmpty(Notificacion.P_Cuenta_predial))
                {
                    Mi_SQL.Append(" WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + "='" + Notificacion.P_Cuenta_predial + "'");
                }
                if (!String.IsNullOrEmpty(Notificacion.P_Proceso))
                {
                    Mi_SQL.Append(" AND " + Ope_Pre_Pae_Notificaciones.Campo_Proceso + "='" + Notificacion.P_Proceso + "'");
                }
                Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las Notificaciones. Error: [" + Ex.Message + "]");
            }
            return Tabla;
        }
    }
}
