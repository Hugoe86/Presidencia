using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Predial_Pae_Postores.Negocio;

namespace Presidencia.Predial_Pae_Postores.Datos
{
    public class Cls_Ope_Pre_Pae_Postores_Datos
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
        ///PARÁMETROS:     
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
                    Mensaje = "Error al intentar dar de Alta un Registro. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Alta_Pae_Postores
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nueva Almoneda de una determinada cuenta
        ///PARAMENTROS:     
        ///CREO: Armando Zavala Moreno
        ///FECHA_CREO: 26/Abril/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO 
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
        public static void Alta_Pae_Postores(Cls_Ope_Pre_Pae_Postores_Negocio Postores)
        {
            StringBuilder MI_SQL = new StringBuilder();
            try
            {
                String No_Postor = Obtener_ID_Consecutivo(Ope_Pre_Pae_Postores.Tabla_Ope_Pre_Pae_Postores, Ope_Pre_Pae_Postores.Campo_No_Postor, 10);
                MI_SQL.Append("INSERT INTO " + Ope_Pre_Pae_Postores.Tabla_Ope_Pre_Pae_Postores);
                MI_SQL.Append(" (" + Ope_Pre_Pae_Postores.Campo_No_Postor + ", " + Ope_Pre_Pae_Postores.Campo_No_Detalle_Etapa + ", ");
                MI_SQL.Append(Ope_Pre_Pae_Postores.Campo_Nombre_Postor + ", " + Ope_Pre_Pae_Postores.Campo_Deposito + ", ");
                MI_SQL.Append(Ope_Pre_Pae_Postores.Campo_Porcentaje + ", " + Ope_Pre_Pae_Postores.Campo_Domicilio + ", ");
                MI_SQL.Append(Ope_Pre_Pae_Postores.Campo_Telefono + ", " + Ope_Pre_Pae_Postores.Campo_Rfc + ", ");
                MI_SQL.Append(Ope_Pre_Pae_Postores.Campo_No_Ife + ", " + Ope_Pre_Pae_Postores.Campo_Sexo + ", ");
                MI_SQL.Append(Ope_Pre_Pae_Postores.Campo_Estado_Civil + ", " + Ope_Pre_Pae_Postores.Campo_Estatus + ", ");
                MI_SQL.Append(Ope_Pre_Pae_Postores.Campo_Usuario_Creo + ", " + Ope_Pre_Pae_Postores.Campo_Fecha_Creo + ")");
                MI_SQL.Append(" VALUES ('" + No_Postor + "' ");
                MI_SQL.Append(",'" + Postores.P_No_Detalle_Etapa + "' ");
                MI_SQL.Append(",'" + Postores.P_Nombre_Postor + "' ");
                MI_SQL.Append(",'" + Postores.P_Deposito + "' ");
                MI_SQL.Append(",'" + Postores.P_Porcentaje + "' ");
                MI_SQL.Append(",'" + Postores.P_Domicilio + "' ");
                MI_SQL.Append(",'" + Postores.P_Telefono + "' ");
                MI_SQL.Append(",'" + Postores.P_Rfc + "' ");
                MI_SQL.Append(",'" + Postores.P_No_Ife + "' ");
                MI_SQL.Append(",'" + Postores.P_Sexo + "' ");
                MI_SQL.Append(",'" + Postores.P_Estado_Civil + "' ");
                MI_SQL.Append(",'" + Postores.P_Estatus + "' ");
                MI_SQL.Append(",'" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'");
                MI_SQL.Append(", sysdate");
                MI_SQL.Append(")");
                Ejecuta_Consulta(MI_SQL.ToString());
            }
            catch (Exception Ex)
            {
                new Exception(Ex.Message);
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Pae_Postores
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nueva Almoneda de una determinada cuenta
        ///PARAMENTROS:     
        ///CREO: Armando Zavala Moreno
        ///FECHA_CREO: 26/Abril/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO 
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
        public static DataTable Busqueda_Pae_Postores(Cls_Ope_Pre_Pae_Postores_Negocio Postores)
        {
            DataTable Tabla = null;
            StringBuilder MI_SQL = new StringBuilder();
            try
            {                
                MI_SQL.Append("SELECT * FROM" + Ope_Pre_Pae_Postores.Tabla_Ope_Pre_Pae_Postores);
                if (!String.IsNullOrEmpty(Postores.P_Estatus))
                {
                    MI_SQL.Append(" WHERE " + Ope_Pre_Pae_Postores.Campo_Estatus + "='" + Postores.P_Estatus + "'");
                }
                if (!String.IsNullOrEmpty(Postores.P_No_Detalle_Etapa))
                {
                    MI_SQL.Append(" AND " + Ope_Pre_Pae_Postores.Campo_No_Detalle_Etapa + "=" + Postores.P_No_Detalle_Etapa);
                }
                // ejecutar consulta
                DataSet Ds_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, MI_SQL.ToString());
                if (Ds_Consulta != null && Ds_Consulta.Tables.Count > 0)
                {
                    Tabla = Ds_Consulta.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar Postores. Error: [" + Ex.Message + "]");
            }
            return Tabla;
        }
    }
}
