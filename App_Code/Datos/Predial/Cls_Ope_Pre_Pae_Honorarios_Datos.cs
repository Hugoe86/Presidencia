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
using Presidencia.Predial_Pae_Honorarios.Negocio;

namespace Presidencia.Predial_Pae_Honorarios.Datos
{
    public class Cls_Ope_Pre_Pae_Honorarios_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Honorario
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nuevo Honorario.
        ///PARAMENTROS:     
        ///CREO: Armando Zavala Moreno
        ///FECHA_CREO: 07/Marzo/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO 
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
        public static void Alta_Honorario(Cls_Ope_Pre_Pae_Honorarios_Negocio Honorarios)
        {
            try
            {
                String No_Honorario = Obtener_ID_Consecutivo(Ope_Pre_Pae_Honorarios.Tabla_Ope_Pre_Pae_Honorarios, Ope_Pre_Pae_Honorarios.Campo_No_Honorario, 10);
                String Mi_SQL = "INSERT INTO " + Ope_Pre_Pae_Honorarios.Tabla_Ope_Pre_Pae_Honorarios;
                Mi_SQL += " (" + Ope_Pre_Pae_Honorarios.Campo_No_Honorario + ", " + Ope_Pre_Pae_Honorarios.Campo_No_Detalle_Etapa;
                Mi_SQL += ", " + Ope_Pre_Pae_Honorarios.Campo_Gasto_Ejecucion_Id + ", " + Ope_Pre_Pae_Honorarios.Campo_Fecha_Honorario;
                Mi_SQL += ", " + Ope_Pre_Pae_Honorarios.Campo_Proceso + ", " + Ope_Pre_Pae_Honorarios.Campo_Importe;
                Mi_SQL += ", " + Ope_Pre_Pae_Honorarios.Campo_Estatus;
                Mi_SQL += ", " + Ope_Pre_Pae_Honorarios.Campo_Usuario_Creo + ", " + Ope_Pre_Pae_Honorarios.Campo_Fecha_Creo + ")";
                Mi_SQL += " VALUES ('" + No_Honorario + "'";
                Mi_SQL += ", '" + Honorarios.P_No_Detalle_Etapa + "'";
                Mi_SQL += ", '" + Honorarios.P_Gasto_Ejecucion_Id + "'";
                Mi_SQL += ", sysdate";
                Mi_SQL += ", '" + Honorarios.P_Proceso + "'";
                Mi_SQL += ", " + Honorarios.P_Importe;
                Mi_SQL += ", '" + Honorarios.P_Estatus + "'";
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Honorario
        ///DESCRIPCIÓN: Devuelve un registro de la tabla de Honorarios
        ///PARAMETROS:      
        ///CREO: Armando Zavala Moreno.
        ///FECHA_CREO: 07/Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Honorario(Cls_Ope_Pre_Pae_Honorarios_Negocio Honorarios)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;
            try
            {
                if (Honorarios.P_Campos_Dinamicos != null && Honorarios.P_Campos_Dinamicos != "")
                    Mi_SQL = "SELECT " + Honorarios.P_Campos_Dinamicos;
                else
                {
                    Mi_SQL = "SELECT " + Ope_Pre_Pae_Honorarios.Tabla_Ope_Pre_Pae_Honorarios + ".*";
                }
                Mi_SQL += ", ( SELECT " + Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id + " FROM " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas;
                Mi_SQL += " WHERE " + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + " = " + Ope_Pre_Pae_Honorarios.Tabla_Ope_Pre_Pae_Honorarios + "." + Ope_Pre_Pae_Honorarios.Campo_No_Detalle_Etapa;
                Mi_SQL += ") CUENTA_PREDIAL_ID";
                Mi_SQL += " FROM " + Ope_Pre_Pae_Honorarios.Tabla_Ope_Pre_Pae_Honorarios;
                if (Honorarios.P_Filtro != null && Honorarios.P_Filtro != "")
                {
                    Mi_SQL += " WHERE " + Honorarios.P_Filtro;
                }
                if (Honorarios.P_No_Detalle_Etapa != null && Honorarios.P_No_Detalle_Etapa != "")
                {
                    Mi_SQL += " WHERE " + Ope_Pre_Pae_Honorarios.Campo_No_Detalle_Etapa + " = " + Honorarios.P_No_Detalle_Etapa;
                }
                if (Honorarios.P_Agrupar_Dinamico != null && Honorarios.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Honorarios.P_Agrupar_Dinamico;
                }
                else
                {
                    Mi_SQL += " ORDER BY " + Ope_Pre_Pae_Honorarios.Campo_No_Honorario;
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

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Total_Honorario
        /// DESCRIPCIÓN: Breve descripción de lo que hace la función.
        /// PARÁMETROS:
        /// 	1. Honorarios: Instancia de la clase de negocio con parámetros para la consulta
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 23-abr-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Total_Honorarios(Cls_Ope_Pre_Pae_Honorarios_Negocio Honorarios_Negocio)
        {
            DataTable Tabla = null;
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT SUM(" + Ope_Pre_Pae_Honorarios.Tabla_Ope_Pre_Pae_Honorarios + "." + Ope_Pre_Pae_Honorarios.Campo_Importe + "-"
                    + Ope_Pre_Pae_Honorarios.Tabla_Ope_Pre_Pae_Honorarios + "." + Ope_Pre_Pae_Honorarios.Campo_Monto_Pagado + ") TOTAL_HONORARIOS";
                Mi_SQL += "," + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id;

                Mi_SQL += " FROM " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas;
                Mi_SQL += " JOIN " + Ope_Pre_Pae_Honorarios.Tabla_Ope_Pre_Pae_Honorarios + " ON "
                    + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + "="
                    + Ope_Pre_Pae_Honorarios.Tabla_Ope_Pre_Pae_Honorarios + "." + Ope_Pre_Pae_Honorarios.Campo_No_Detalle_Etapa;

                // WHERE
                Mi_SQL += " WHERE " + Ope_Pre_Pae_Honorarios.Tabla_Ope_Pre_Pae_Honorarios + "." + Ope_Pre_Pae_Honorarios.Campo_Importe + "-"
                    + Ope_Pre_Pae_Honorarios.Tabla_Ope_Pre_Pae_Honorarios + "." + Ope_Pre_Pae_Honorarios.Campo_Monto_Pagado + " > 0";

                // filtrar por CUENTA PREDIAL
                if (!string.IsNullOrEmpty(Honorarios_Negocio.P_Cuenta_Predial_ID))
                {
                    Mi_SQL += " AND " + Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id + "= '" + Honorarios_Negocio.P_Cuenta_Predial_ID + "'";
                }

                // filtrar por ETAPA
                if (!string.IsNullOrEmpty(Honorarios_Negocio.P_No_Detalle_Etapa))
                {
                    Mi_SQL += " AND " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa
                        + "= '" + Honorarios_Negocio.P_No_Detalle_Etapa + "'";
                }

                // GROUP BY
                if (!string.IsNullOrEmpty(Honorarios_Negocio.P_Agrupar_Dinamico))
                {
                    Mi_SQL += " GROUP BY " + Honorarios_Negocio.P_Agrupar_Dinamico;
                }
                else
                {
                    Mi_SQL += " GROUP BY " + Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id;
                }

                // ejecutar consulta
                DataSet Ds_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Consulta != null && Ds_Consulta.Tables.Count > 0)
                {
                    Tabla = Ds_Consulta.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar Honorarios. Error: [" + Ex.Message + "]");
            }
            return Tabla;
        }
        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Detalles_Honorarios
        /// DESCRIPCIÓN: Obtiene los campos de rezagos para mostrarlos en la ventana emergente detalles de honorarios.
        /// PARÁMETROS:
        /// 	1. Honorarios: Instancia de la clase de negocio con parámetros para la consulta
        /// CREO: Armando Zavala Moreno.
        /// FECHA_CREO: 02-May-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Detalles_Honorarios(Cls_Ope_Pre_Pae_Honorarios_Negocio Honorarios_Negocio)
        {
            DataTable Tabla = null;
            StringBuilder Mi_SQL = new StringBuilder();
            try
            {
                Mi_SQL.Append("SELECT NVL(" + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Corriente + ",0)CORRIENTE, ");
                Mi_SQL.Append("NVL(" + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Rezago + ",0)REZAGO, ");
                Mi_SQL.Append("NVL(" + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Ordinarios + ",0)ORDINARIOS, ");
                Mi_SQL.Append("NVL(" + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Moratorios + ",0)MORATORIOS, ");
                Mi_SQL.Append("NVL(" + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Honorarios + ",0)HONORARIOS_TOTAL ");
                Mi_SQL.Append(" FROM " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas);
                Mi_SQL.Append(" INNER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas);
                Mi_SQL.Append(" ON " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id + "=" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID);
                if (!string.IsNullOrEmpty(Honorarios_Negocio.P_Cuenta_Predial))
                {
                    Mi_SQL.Append(" WHERE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + "='" + Honorarios_Negocio.P_Cuenta_Predial + "'");
                }
                // ejecutar consulta
                DataSet Ds_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                if (Ds_Consulta != null && Ds_Consulta.Tables.Count > 0)
                {
                    Tabla = Ds_Consulta.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar Honorarios. Error: [" + Ex.Message + "]");
            }
            return Tabla;
        }
    }
}
