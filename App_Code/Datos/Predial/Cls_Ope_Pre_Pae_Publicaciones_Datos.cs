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
using Presidencia.Predial_Pae_Publicaciones.Negocio;

namespace Presidencia.Predial_Pae_Publicaciones.Datos
{
    public class Cls_Ope_Pre_Pae_Publicaciones_Datos
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
        ///NOMBRE DE LA FUNCIÓN: Alta_Publicaciones
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nueva publicacion.
        ///PARAMENTROS:     
        ///CREO: Armando Zavala Moreno
        ///FECHA_CREO: 28/Marzo/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO 
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
        public static void Alta_Publicaciones(Cls_Ope_Pre_Pae_Publicaciones_Negocio Publicacion)
        {
            try
            {
                String No_Publicacion = Obtener_ID_Consecutivo(Ope_Pre_Pae_Publicaciones.Tabla_Ope_Pre_Pae_Publicaciones, Ope_Pre_Pae_Publicaciones.Campo_No_Publicacion, 10);
                String Mi_SQL = "INSERT INTO " + Ope_Pre_Pae_Publicaciones.Tabla_Ope_Pre_Pae_Publicaciones;
                Mi_SQL += " (" + Ope_Pre_Pae_Publicaciones.Campo_No_Publicacion + ", " + Ope_Pre_Pae_Publicaciones.Campo_No_Detalle_Etapa;
                Mi_SQL += ", " + Ope_Pre_Pae_Publicaciones.Campo_Fecha_Publicacion + ", " + Ope_Pre_Pae_Publicaciones.Campo_Medio_Publicacion;
                Mi_SQL += ", " + Ope_Pre_Pae_Publicaciones.Campo_Pagina + ", " + Ope_Pre_Pae_Publicaciones.Campo_Tomo;
                Mi_SQL += ", " + Ope_Pre_Pae_Publicaciones.Campo_Parte + ", " + Ope_Pre_Pae_Publicaciones.Campo_Foja;
                Mi_SQL += ", " + Ope_Pre_Pae_Publicaciones.Campo_Proceso + ", " + Ope_Pre_Pae_Publicaciones.Campo_Estatus;
                Mi_SQL += ", " + Ope_Pre_Pae_Publicaciones.Campo_Usuario_Creo + ", " + Ope_Pre_Pae_Publicaciones.Campo_Fecha_Creo + ")";
                Mi_SQL += " VALUES ('" + No_Publicacion + "'";
                Mi_SQL += ", '" + Publicacion.P_No_Detalle_Etapa + "'";
                Mi_SQL += ", '" + Publicacion.P_Fecha_Publicacion + "'";
                Mi_SQL += ", '" + Publicacion.P_Medio_Publicacion + "'";
                Mi_SQL += ", '" + Publicacion.P_Pagina + "'";
                Mi_SQL += ", '" + Publicacion.P_Tomo + "'";
                Mi_SQL += ", '" + Publicacion.P_Parte + "'";
                Mi_SQL += ", '" + Publicacion.P_Foja + "'";
                Mi_SQL += ", '" + Publicacion.P_Proceso + "'";
                Mi_SQL += ", '" + Publicacion.P_Estatus + "'";
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Publicacion
        ///DESCRIPCIÓN: Devuelve un registro de la tabla de Publicacion
        ///PARAMETROS:      
        ///CREO: Armando Zavala Moreno.
        ///FECHA_CREO: 28/Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Publicacion(Cls_Ope_Pre_Pae_Publicaciones_Negocio Publicacion)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;
            try
            {
                if (Publicacion.P_Campos_Dinamicos != null && Publicacion.P_Campos_Dinamicos != "")
                    Mi_SQL = "SELECT " + Publicacion.P_Campos_Dinamicos;
                else
                {
                    Mi_SQL = "SELECT " + Ope_Pre_Pae_Publicaciones.Tabla_Ope_Pre_Pae_Publicaciones + ".*";
                }
                Mi_SQL += ", ( SELECT " + Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id + " FROM " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas;
                Mi_SQL += " WHERE " + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + " = " + Ope_Pre_Pae_Publicaciones.Tabla_Ope_Pre_Pae_Publicaciones + "." + Ope_Pre_Pae_Publicaciones.Campo_No_Detalle_Etapa;
                Mi_SQL += ") CUENTA_PREDIAL_ID";
                Mi_SQL += " FROM " + Ope_Pre_Pae_Publicaciones.Tabla_Ope_Pre_Pae_Publicaciones;
                if (Publicacion.P_Filtro != null && Publicacion.P_Filtro != "")
                {
                    Mi_SQL += " WHERE " + Publicacion.P_Filtro;
                }
                if (Publicacion.P_No_Detalle_Etapa != null && Publicacion.P_No_Detalle_Etapa != "")
                {
                    Mi_SQL += " WHERE " + Ope_Pre_Pae_Publicaciones.Campo_No_Detalle_Etapa + " = " + Publicacion.P_No_Detalle_Etapa;
                }
                if (Publicacion.P_Agrupar_Dinamico != null && Publicacion.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Publicacion.P_Agrupar_Dinamico;
                }
                else
                {
                    Mi_SQL += " ORDER BY " + Ope_Pre_Pae_Publicaciones.Campo_No_Publicacion;
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
    }
}
