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
using Presidencia.Predial_Pae_Etapas.Negocio;
using Presidencia.Catalogo_Despachos_Externos.Negocio;
using Presidencia.Predial_Pae_Notificaciones.Negocio;

namespace Presidencia.Predial_Pae_Etapas.Datos
{
    public class Cls_Ope_Pre_Pae_Etapas_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Pae_Etapas
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nueva Institucion.
        ///PARAMENTROS:     
        ///CREO: Armando Zavala Moreno
        ///FECHA_CREO: 16/Febrero/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO 
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
        public static void Alta_Pae_Etapas(Cls_Ope_Pre_Pae_Etapas_Negocio Pae_Etapas)
        {
            try
            {
                String No_Etapa = Obtener_ID_Consecutivo(Ope_Pre_Pae_Etapas.Tabla_Ope_Pre_Pae_Etapas, Ope_Pre_Pae_Etapas.Campo_No_Etapa, 10);
                String Mi_SQL = "INSERT INTO " + Ope_Pre_Pae_Etapas.Tabla_Ope_Pre_Pae_Etapas;
                Mi_SQL += " (" + Ope_Pre_Pae_Etapas.Campo_No_Etapa + ", " + Ope_Pre_Pae_Etapas.Campo_Despacho_Id;
                Mi_SQL += ", " + Ope_Pre_Pae_Etapas.Campo_Numero_Entrega + " ," + Ope_Pre_Pae_Etapas.Campo_Total_Etapa;
                Mi_SQL += ", " + Ope_Pre_Pae_Etapas.Campo_Modo_Generacion + " ," + Ope_Pre_Pae_Etapas.Campo_Nombre_Archivo;
                Mi_SQL  +=", "+ Ope_Pre_Pae_Etapas.Campo_Fecha_Generacion +", " + Ope_Pre_Pae_Etapas.Campo_Comentario;
                Mi_SQL += ", " + Ope_Pre_Pae_Etapas.Campo_Usuario_Creo + ", " + Ope_Pre_Pae_Etapas.Campo_Fecha_Creo + ")";
                Mi_SQL += " VALUES ('" + No_Etapa + "'";
                Mi_SQL += ", '" + Pae_Etapas.P_Despacho_Id + "'";
                Mi_SQL += ", " + Pae_Etapas.P_Numero_Entrega;
                Mi_SQL += ", " + Pae_Etapas.P_Total_Etapa;
                Mi_SQL += ", '" + Pae_Etapas.P_Modo_Generacion + "'";
                Mi_SQL += ", '" + Pae_Etapas.P_Nombre_Archivo + "'";
                Mi_SQL += ", sysdate";
                Mi_SQL += ", '" + Pae_Etapas.P_Comentario + "'";
                Mi_SQL += ",'" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL +=", sysdate";
                Mi_SQL +=")";
                Ejecuta_Consulta(Mi_SQL);
                //Insertar Cuentas A Determinar
                if (Pae_Etapas.P_Dt_Generadas != null)
                {
                    Alta_Pae_Det_Etapas(Pae_Etapas, Mi_SQL, No_Etapa, Pae_Etapas.P_Dt_Generadas, "NO", false);
                }
                //Insertar Cuentas Omitidas
                if (Pae_Etapas.P_Dt_Omitidas != null)
                {
                    Alta_Pae_Det_Etapas(Pae_Etapas, Mi_SQL, No_Etapa, Pae_Etapas.P_Dt_Omitidas, "SI", true);
                }
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_No_Entrega
        ///DESCRIPCIÓN: Obtiene el numero de entrega que tiene cierto despacho
        ///PARÁMETROS:     
        ///CREO: Armando Zavala Moreno.
        ///FECHA_CREO: 16/Febrero/2012 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static String Consultar_No_Entrega(Cls_Ope_Pre_Pae_Etapas_Negocio Datos, String Año)
        {
            String No_Entrega="";
            try
            {                
                String Mi_SQL = "SELECT nvl(MAX(" + Ope_Pre_Pae_Etapas.Campo_Numero_Entrega + "),0) FROM " + Ope_Pre_Pae_Etapas.Tabla_Ope_Pre_Pae_Etapas;
                Mi_SQL += " WHERE " + Ope_Pre_Pae_Etapas.Campo_Despacho_Id + "='" + Datos.P_Despacho_Id + "' AND TO_CHAR(" + Ope_Pre_Pae_Etapas.Campo_Fecha_Creo+", 'yyyy')="+Año;
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                {
                    No_Entrega = (Convert.ToInt32(Obj_Temp) + 1).ToString();
                }
            }
            catch (OracleException Ex)
            {
                new Exception(Ex.Message);
            }
            return No_Entrega;
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
                    Mensaje = "Error al intentar dar de Alta un Registro del PAE Etapas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Alta_Pae_Detalles_Cuentas
        ///DESCRIPCIÓN: Inserta un registro en la tabla Detalles Cuentas
        ///PARÁMETROS:     
        ///CREO: Armando Zavala Moreno.
        ///FECHA_CREO: 17/Febrero/2012 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static void Alta_Pae_Detalles_Cuentas(String No_Detalle_Etapa, Cls_Ope_Pre_Pae_Etapas_Negocio Pae_Etapas)
        {
            string Mi_SQL;
            String No_Detalle_Cuenta = Obtener_ID_Consecutivo(Ope_Pre_Pae_Detalles_Cuentas.Tabla_Ope_Pre_Pae_Detalles_Cuentas, Ope_Pre_Pae_Detalles_Cuentas.Campo_No_Detalle_Cuenta, 10);
            Mi_SQL = "INSERT INTO " + Ope_Pre_Pae_Detalles_Cuentas.Tabla_Ope_Pre_Pae_Detalles_Cuentas;
            Mi_SQL += " (" + Ope_Pre_Pae_Detalles_Cuentas.Campo_No_Detalle_Cuenta + ", " + Ope_Pre_Pae_Detalles_Cuentas.Campo_No_Detalle_Etapa;
            Mi_SQL += ", " + Ope_Pre_Pae_Detalles_Cuentas.Campo_Fecha_Proceso_Cambio + ", " + Ope_Pre_Pae_Detalles_Cuentas.Campo_Proceso_Anterior;
            Mi_SQL += ", " + Ope_Pre_Pae_Detalles_Cuentas.Campo_Proceso_Actual + ", " + Ope_Pre_Pae_Detalles_Cuentas.Campo_Usuario_Creo;
            Mi_SQL += ", " + Ope_Pre_Pae_Detalles_Cuentas.Campo_Fecha_Creo + ")";
            Mi_SQL += " VALUES ('" + No_Detalle_Cuenta + "'";
            Mi_SQL += ", '" + No_Detalle_Etapa + "'";
            Mi_SQL += ", sysdate";
            Mi_SQL += ", '" + Pae_Etapas.P_Proceso_Anterior + "'";
            Mi_SQL += ", '" + Pae_Etapas.P_Proceso_Actual + "'";
            Mi_SQL += ",'" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
            Mi_SQL += ", sysdate";
            Mi_SQL += ")";
            Ejecuta_Consulta(Mi_SQL);
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Pae_Det_Etapas
        ///DESCRIPCIÓN: Inserta registro en la tabla determinacion Etapas
        ///PARÁMETROS:     
        ///CREO: Armando Zavala Moreno.
        ///FECHA_CREO: 17/Febrero/2012 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        private static void Alta_Pae_Det_Etapas(Cls_Ope_Pre_Pae_Etapas_Negocio Pae_Etapas, String Mi_SQL, String No_Etapa,DataTable Dt_Consulta,String Omitida,bool Tiene_Motivo)
        {
            Mi_SQL = "";
            String Tabla_Con_Motivo;
            String Folio;
            Int32 Folio_Aux = 0;
            foreach (DataRow Dr_Recorrido in Dt_Consulta.Rows)
            {
                if (Tiene_Motivo == true)
                {
                    Tabla_Con_Motivo = ", '" + Dr_Recorrido["MOTIVO_OMISIÓN"].ToString() + "'";
                    Folio = "''";
                }
                else
                {
                    Folio_Aux++;
                    Tabla_Con_Motivo = ", ''";
                    Folio = Folio_Aux.ToString();
                }
                String No_Detalle_Etapa = Obtener_ID_Consecutivo(Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas, Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa, 10);
                Mi_SQL = "INSERT INTO " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas;
                Mi_SQL += " (" + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + ", " + Ope_Pre_Pae_Det_Etapas.Campo_No_Etapa;
                Mi_SQL += ", " + Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id + ", " + Ope_Pre_Pae_Det_Etapas.Campo_Periodo_Corriente;
                Mi_SQL += ", " + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Corriente + ", " + Ope_Pre_Pae_Det_Etapas.Campo_Periodo_Rezago;
                Mi_SQL += ", " + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Rezago + ", " + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Ordinarios;
                Mi_SQL += ", " + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Moratorios + ", " + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Honorarios;
                Mi_SQL += ", " + Ope_Pre_Pae_Det_Etapas.Campo_Multas + ", " + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Total;
                Mi_SQL += ", " + Ope_Pre_Pae_Det_Etapas.Campo_Proceso_Actual + ", " + Ope_Pre_Pae_Det_Etapas.Campo_Estatus;
                Mi_SQL += ", " + Ope_Pre_Pae_Det_Etapas.Campo_Omitida + ", " + Ope_Pre_Pae_Det_Etapas.Campo_Motivo_Omision;
                Mi_SQL += ", " + Ope_Pre_Pae_Det_Etapas.Campo_Folio + ", " + Ope_Pre_Pae_Det_Etapas.Campo_Impresa;
                Mi_SQL += ", " + Ope_Pre_Pae_Det_Etapas.Campo_Usuario_Creo + ", " + Ope_Pre_Pae_Det_Etapas.Campo_Fecha_Creo + ")";
                Mi_SQL += " VALUES ('" + No_Detalle_Etapa + "'";
                Mi_SQL += ", '" + No_Etapa + "'";
                Mi_SQL += ", '" + Dr_Recorrido["CUENTA_PREDIAL_ID"].ToString() + "'";
                Mi_SQL += ", '" + Dr_Recorrido["PERIODO_CORRIENTE"].ToString() + "'";
                Mi_SQL += ", " + Dr_Recorrido["CORRIENTE"].ToString();
                Mi_SQL += ", '" + Dr_Recorrido["PERIODO_REZAGO"].ToString() + "'";
                Mi_SQL += ", " + Dr_Recorrido["REZAGO"].ToString();
                Mi_SQL += ", " + Dr_Recorrido["RECARGOS_ORDINARIOS"].ToString();
                Mi_SQL += ", " + Dr_Recorrido["RECARGOS_MORATORIOS"].ToString();
                Mi_SQL += ", " + Dr_Recorrido["HONORARIOS"].ToString();
                Mi_SQL += ", " + Dr_Recorrido["MULTAS"].ToString();
                Mi_SQL += ", " + Dr_Recorrido["ADEUDO"].ToString();
                Mi_SQL += ", '" + Pae_Etapas.P_Proceso_Actual + "'";
                Mi_SQL += ", '" + Dr_Recorrido["ESTATUS"].ToString() + "'";
                Mi_SQL += ", '" + Omitida + "'";
                Mi_SQL += Tabla_Con_Motivo;
                Mi_SQL += ", " + Folio;
                Mi_SQL += ", '" + Pae_Etapas.P_Impresa + "'";
                Mi_SQL += ",'" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL += ", sysdate";
                Mi_SQL += ")";
                Ejecuta_Consulta(Mi_SQL);
                Alta_Pae_Detalles_Cuentas(No_Detalle_Etapa, Pae_Etapas);
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Pae_Det_Etapas
        ///DESCRIPCIÓN: Obtiene una cueta predial ID en la tabla Pae_Det_Etapas
        ///PARAMETROS:      
        ///CREO: Armando Zavala Moreno.
        ///FECHA_CREO: 18/Febrero/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Pae_Det_Etapas(Cls_Ope_Pre_Pae_Etapas_Negocio Pae_Etapas)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;
            Boolean Segundo_Filtro = false;
            try
            {
                if (Pae_Etapas.P_Campos_Dinamicos != null && Pae_Etapas.P_Campos_Dinamicos != "")
                {
                    Mi_SQL = "SELECT " + Pae_Etapas.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL = "SELECT " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + ".* ," + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial;
                    Mi_SQL += ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID;
                    Mi_SQL += ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion;
                    Mi_SQL += ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion;
                    Mi_SQL += ", (SELECT " + Ope_Pre_Pae_Notificaciones.Campo_Fecha_Hora + " FROM " + Ope_Pre_Pae_Notificaciones.Tabla_Ope_Pre_Pae_Notificaciones + " WHERE " + Ope_Pre_Pae_Notificaciones.Campo_No_Detalle_Etapa + " = " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + " AND " + Ope_Pre_Pae_Notificaciones.Campo_Proceso + " = '" + Pae_Etapas.P_Proceso_Actual + "')";
                    Mi_SQL += " FECHA_NOTIFICACION";
                    Mi_SQL += ", " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Honorarios + " AS SUMA_HONORARIOS";
                    Mi_SQL += ", (" + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Corriente + " + " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Rezago;
                    Mi_SQL += " + " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Ordinarios + " + " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Moratorios;
                    Mi_SQL += " + " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Honorarios;
                    Mi_SQL += " + " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Multas;
                    Mi_SQL += ") TOTAL";
                    Mi_SQL += ", " + Cat_Pre_Despachos_Externos.Tabla_Cat_Pre_Despachos_Externos + "." + Cat_Pre_Despachos_Externos.Campo_Despacho;
                    Mi_SQL += ", " + Ope_Pre_Pae_Etapas.Tabla_Ope_Pre_Pae_Etapas + "." + Ope_Pre_Pae_Etapas.Campo_Numero_Entrega;
                }
                Mi_SQL += " FROM " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas;
                Mi_SQL += " INNER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;
                Mi_SQL += " ON " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id + "=" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                Mi_SQL += " INNER JOIN " + Ope_Pre_Pae_Etapas.Tabla_Ope_Pre_Pae_Etapas;
                Mi_SQL += " ON " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Etapa + "=" + Ope_Pre_Pae_Etapas.Tabla_Ope_Pre_Pae_Etapas + "." + Ope_Pre_Pae_Etapas.Campo_No_Etapa;
                Mi_SQL += " INNER JOIN " + Cat_Pre_Despachos_Externos.Tabla_Cat_Pre_Despachos_Externos;
                Mi_SQL += " ON " + Ope_Pre_Pae_Etapas.Tabla_Ope_Pre_Pae_Etapas + "." + Ope_Pre_Pae_Etapas.Campo_Despacho_Id + "=" + Cat_Pre_Despachos_Externos.Tabla_Cat_Pre_Despachos_Externos + "." + Cat_Pre_Despachos_Externos.Campo_Despacho_Id;

                if (Pae_Etapas.P_Filtro != null && Pae_Etapas.P_Filtro != "")
                {
                    Mi_SQL += " WHERE " + Pae_Etapas.P_Filtro;
                    Segundo_Filtro = true;
                }
                if (Pae_Etapas.P_Cuenta_Predial_Id != null && Pae_Etapas.P_Cuenta_Predial_Id != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id + " = " + Pae_Etapas.P_Cuenta_Predial_Id;
                    Segundo_Filtro = true;

                }
                if (Pae_Etapas.P_No_Detalle_Etapa != null && Pae_Etapas.P_No_Detalle_Etapa != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + " = " + Pae_Etapas.P_No_Detalle_Etapa;
                    Segundo_Filtro = true;
                }
                if (Pae_Etapas.P_No_Etapa != null && Pae_Etapas.P_No_Etapa != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Etapa + " = " + Pae_Etapas.P_No_Etapa;
                    Segundo_Filtro = true;
                }
                if (Pae_Etapas.P_Cuenta_Predial != null && Pae_Etapas.P_Cuenta_Predial != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Pae_Etapas.P_Cuenta_Predial + "'";
                    Segundo_Filtro = true;
                }
                if (Pae_Etapas.P_Proceso_Actual != null && Pae_Etapas.P_Proceso_Actual != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Proceso_Actual + " = '" + Pae_Etapas.P_Proceso_Actual + "'";
                    Segundo_Filtro = true;
                }
                if (Pae_Etapas.P_Estatus != null && Pae_Etapas.P_Estatus != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Estatus + " = '" + Pae_Etapas.P_Estatus + "'";
                    Segundo_Filtro = true;
                }
                if (Pae_Etapas.P_Omitida != null && Pae_Etapas.P_Omitida != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Omitida + " = '" + Pae_Etapas.P_Omitida + "'";
                    Segundo_Filtro = true;
                }
                if (Pae_Etapas.P_Impresa != null && Pae_Etapas.P_Impresa != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Impresa + " = '" + Pae_Etapas.P_Impresa + "'";
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Folio))//Se utiliza para un unico folio
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Folio + "='" + Pae_Etapas.P_Folio+"'";
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Folio_Inicial))//Se utiliza para el rango de los folios
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Folio + ">=" + Pae_Etapas.P_Folio_Inicial;
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Folio_Final))//Se utiliza para el rango de los folios
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Folio + "<=" + Pae_Etapas.P_Folio_Final;
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Despacho_Id))
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Cat_Pre_Despachos_Externos.Tabla_Cat_Pre_Despachos_Externos + "." + Cat_Pre_Despachos_Externos.Campo_Despacho_Id + "=" + Pae_Etapas.P_Despacho_Id;
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Tipo_Predio))
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + "='" + Pae_Etapas.P_Tipo_Predio + "'";
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Colonia_ID))
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + "='" + Pae_Etapas.P_Colonia_ID + "'";
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Calle_ID))
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + "='" + Pae_Etapas.P_Calle_ID + "'";
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Colonia_ID_Notificacion))
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + "='" + Pae_Etapas.P_Colonia_ID_Notificacion + "'";
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Calle_ID_Notificacion))
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + "='" + Pae_Etapas.P_Calle_ID_Notificacion + "'";
                    Segundo_Filtro = true;
                }
                if (Pae_Etapas.P_Agrupar_Dinamico != null && Pae_Etapas.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Pae_Etapas.P_Agrupar_Dinamico;
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
        ///NOMBRE DE LA FUNCIÓN : Consultar_Pae_Etapas
        ///DESCRIPCIÓN          : Obtiene las cuentas de acuerdo a los filtros establecidos en la interfaz
        ///PARAMETROS           : Cuenta, instancia de Cls_Ope_Pre_Pae_Etapas_Negocio
        ///CREO                 : Armando Zavala Moreno
        ///FECHA_CREO           : 24/Febrero/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Pae_Etapas(Cls_Ope_Pre_Pae_Etapas_Negocio Pae_Etapas)
        {
            //Cls_Cat_Pre_Despachos_Externos_Negocio Rs_Despachos = new Cls_Cat_Pre_Despachos_Externos_Negocio();
            DataTable Tabla = new DataTable();
            String Mi_SQL;
            try
            {
                if (Pae_Etapas.P_Campos_Dinamicos != null && Pae_Etapas.P_Campos_Dinamicos != "")
                    Mi_SQL = "SELECT " + Pae_Etapas.P_Campos_Dinamicos;
                else
                {
                    Mi_SQL = "SELECT " + Ope_Pre_Pae_Etapas.Tabla_Ope_Pre_Pae_Etapas + ".*";
                }
                Mi_SQL += ", ( SELECT " + Cat_Pre_Despachos_Externos.Campo_Despacho + " FROM " + Cat_Pre_Despachos_Externos.Tabla_Cat_Pre_Despachos_Externos;
                Mi_SQL += " WHERE " + Cat_Pre_Despachos_Externos.Campo_Despacho_Id + "=" + Ope_Pre_Pae_Etapas.Tabla_Ope_Pre_Pae_Etapas+"." + Ope_Pre_Pae_Etapas.Campo_Despacho_Id;
                Mi_SQL += ") DESPACHO";
                Mi_SQL += " FROM " + Ope_Pre_Pae_Etapas.Tabla_Ope_Pre_Pae_Etapas;
                if (Pae_Etapas.P_Filtro != null && Pae_Etapas.P_Filtro != "")
                {
                    Mi_SQL += " WHERE " + Pae_Etapas.P_Filtro;
                }
                if (Pae_Etapas.P_No_Etapa != null && Pae_Etapas.P_No_Etapa != "")
                {
                    Mi_SQL += " WHERE " + Ope_Pre_Pae_Etapas.Campo_No_Etapa + " = " + Pae_Etapas.P_No_Etapa;
                }
                if (Pae_Etapas.P_Despacho_Id != null && Pae_Etapas.P_Despacho_Id != "")
                {
                    Mi_SQL += " WHERE " + Ope_Pre_Pae_Etapas.Campo_Despacho_Id + " = " + Pae_Etapas.P_Despacho_Id;
                }
                if (Pae_Etapas.P_Agrupar_Dinamico != null && Pae_Etapas.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Pae_Etapas.P_Agrupar_Dinamico;
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
        ///NOMBRE DE LA FUNCIÓN: Actualiza_Pae_Det_Etapas
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Registro de PAE_DET_ETAPAS
        ///PARAMETROS: Pae_Etapas, instancia de la capa de negocios
        ///CREO: Armando Zavala Moreno.
        ///FECHA_CREO: 09/Marzo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Actualiza_Pae_Det_Etapas(Cls_Ope_Pre_Pae_Etapas_Negocio Pae_Etapas)
        {
            Boolean Actualizar = false;
            try
            {
                String Mi_SQL = "UPDATE " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas;
                Mi_SQL += " SET ";
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Periodo_Corriente))
                {
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Campo_Periodo_Corriente + " = '" + Pae_Etapas.P_Periodo_Corriente + "', ";
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Adeudo_Corriente))
                {
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Corriente + " = " + Pae_Etapas.P_Adeudo_Corriente + ", ";
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Periodo_Rezago))
                {
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Campo_Periodo_Rezago + " = '" + Pae_Etapas.P_Periodo_Rezago + "', ";
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Adeudo_Rezago))
                {
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Rezago + " = " + Pae_Etapas.P_Adeudo_Rezago + ", ";
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Adeudo_Recargos_Ordinarios))
                {
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Ordinarios + " = " + Pae_Etapas.P_Adeudo_Recargos_Ordinarios + ", ";
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Adeudo_Recargos_Moratorios))
                {
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Moratorios + " = " + Pae_Etapas.P_Adeudo_Recargos_Moratorios + ", ";
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Adeudo_Honorarios))
                {
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Honorarios + " = " + Pae_Etapas.P_Adeudo_Honorarios + ", ";
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Multas))
                {
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Campo_Multas + " = " + Pae_Etapas.P_Multas + ", ";
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Adeudo_Total))
                {
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Total + " = " + Pae_Etapas.P_Adeudo_Total + ", ";
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Proceso_Actual))
                {
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Campo_Proceso_Actual + " = '" + Pae_Etapas.P_Proceso_Actual + "', ";
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Estatus))
                {
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Campo_Estatus + " = '" + Pae_Etapas.P_Estatus + "', ";
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Omitida))
                {
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Campo_Omitida + " = '" + Pae_Etapas.P_Omitida + "', ";
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Motivo_Omision))
                {
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Campo_Motivo_Omision + " = '" + Pae_Etapas.P_Motivo_Omision + "', ";
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Impresa))
                {
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Campo_Impresa + " = '" + Pae_Etapas.P_Impresa + "', ";
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Fecha_Hora_Notificacion))
                {
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Campo_Fecha_Hora_Notificacion + " = '" + Pae_Etapas.P_Fecha_Hora_Notificacion + "', ";
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Motivo_Cambio_Estatus))
                {
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Campo_Motivo_Cambio_Estatus + " = '" + Pae_Etapas.P_Motivo_Cambio_Estatus + "', ";
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Resolucion))
                {
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Campo_Resolucion + " = '" + Pae_Etapas.P_Resolucion + "', ";
                }

                Mi_SQL += Ope_Pre_Pae_Det_Etapas.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "', ";
                Mi_SQL += Ope_Pre_Pae_Det_Etapas.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL += "WHERE " + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + " = " + Pae_Etapas.P_No_Detalle_Etapa;
                Ejecuta_Consulta(Mi_SQL);
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar Actualizar el registro de la Cuenta. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Actualizar;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Actualiza_Pae_Detalle_Cuentas
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Registro de PAE_DETALLE_CUENTAS
        ///PARAMETROS: Pae_Etapas, instancia de la capa de negocios
        ///CREO: An.
        ///FECHA_CREO: 09/Marzo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Actualiza_Pae_Detalle_Cuentas(Cls_Ope_Pre_Pae_Etapas_Negocio Pae_Etapas)
        {
            Boolean Actualizar = false;
            try
            {
                String Mi_SQL = " UPDATE " + Ope_Pre_Pae_Detalles_Cuentas.Tabla_Ope_Pre_Pae_Detalles_Cuentas;
                Mi_SQL += " SET ";
                if (Pae_Etapas.P_Fecha_Proceso_Cambio != "" && Pae_Etapas.P_Fecha_Proceso_Cambio != null)
                {
                    Mi_SQL += Ope_Pre_Pae_Detalles_Cuentas.Campo_Fecha_Proceso_Cambio + " = '" + Pae_Etapas.P_Fecha_Proceso_Cambio + "' , ";
                }
                Mi_SQL += Ope_Pre_Pae_Detalles_Cuentas.Campo_Proceso_Anterior + " = '" + Pae_Etapas.P_Proceso_Anterior + "' , "; //+ Pae_Etapas.P_Proceso_Anterior + "' , ";
                Mi_SQL += Ope_Pre_Pae_Detalles_Cuentas.Campo_Proceso_Actual + " = '" + Pae_Etapas.P_Proceso_Actual + "' , ";
                Mi_SQL += Ope_Pre_Pae_Detalles_Cuentas.Campo_Fecha_Proceso_Cambio + " = SYSDATE ,";
                Mi_SQL += Ope_Pre_Pae_Detalles_Cuentas.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "' , ";
                Mi_SQL += Ope_Pre_Pae_Detalles_Cuentas.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL += Ope_Pre_Pae_Detalles_Cuentas.Campo_No_Almoneda + "= ''";
                Mi_SQL += " WHERE " + Ope_Pre_Pae_Detalles_Cuentas.Campo_No_Detalle_Etapa + " = " + Pae_Etapas.P_No_Detalle_Etapa;
                Ejecuta_Consulta(Mi_SQL);
                
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar Actualizar el registro de la Cuenta. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Actualizar;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Contribuyente_Etapas_Pae
        ///DESCRIPCIÓN          : Obtiene las cuentas Predial que tiene un determinado contribuyente
        ///PARAMETROS           : Cuenta, instancia de Cls_Ope_Pre_Pae_Etapas_Negocio
        ///CREO                 : Armando Zavala Moreno
        ///FECHA_CREO           : 23/Marzo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Contribuyente_Etapas_Pae(Cls_Ope_Pre_Pae_Etapas_Negocio Pae_Etapas)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;
            try
            {
                if (Pae_Etapas.P_Campos_Dinamicos != null && Pae_Etapas.P_Campos_Dinamicos != "")
                    Mi_SQL = "SELECT " + Pae_Etapas.P_Campos_Dinamicos;
                else
                {
                    Mi_SQL = "SELECT " + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID;
                }
                Mi_SQL += " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios;
                Mi_SQL += " WHERE " + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " = ";
                Mi_SQL += Pae_Etapas.P_Contribuyente_Id;

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
        ///NOMBRE DE LA FUNCIÓN: Consulta_Cuentas_Honorarios
        ///DESCRIPCIÓN: Obtiene los detales de las cuentas que estan en el PAE
        ///PARAMETROS:      
        ///CREO: Armando Zavala Moreno.
        ///FECHA_CREO: 17/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consulta_Cuentas_Honorarios(Cls_Ope_Pre_Pae_Etapas_Negocio Pae_Etapas)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;
            Boolean Segundo_Filtro = false;
            try
            {
                if (Pae_Etapas.P_Campos_Dinamicos != null && Pae_Etapas.P_Campos_Dinamicos != "")
                {
                    Mi_SQL = "SELECT " + Pae_Etapas.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL = "SELECT CU." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial;
                    Mi_SQL += ", NVL((E." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Corriente + " + E." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Rezago;
                    Mi_SQL += " + E." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Ordinarios + " + E." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Moratorios;
                    Mi_SQL += " + E." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Honorarios + " + E." + Ope_Pre_Pae_Det_Etapas.Campo_Multas;
                    Mi_SQL += "),0) TOTAL";
                    Mi_SQL += ", E." + Ope_Pre_Pae_Det_Etapas.Campo_Folio;
                    Mi_SQL += ", DE." + Cat_Pre_Despachos_Externos.Campo_Despacho;
                    Mi_SQL += ", ETA." + Ope_Pre_Pae_Etapas.Campo_Numero_Entrega;
                    Mi_SQL += ", (SELECT " + Ope_Pre_Pae_Notificaciones.Campo_Fecha_Hora + " FROM " + Ope_Pre_Pae_Notificaciones.Tabla_Ope_Pre_Pae_Notificaciones + " WHERE " + Ope_Pre_Pae_Notificaciones.Campo_No_Detalle_Etapa + " = " + "E." + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + " AND " + Ope_Pre_Pae_Notificaciones.Campo_Proceso + " = " + "E." + Ope_Pre_Pae_Det_Etapas.Campo_Proceso_Actual + ")";
                    Mi_SQL += " FECHA_NOTIFICACION";
                    Mi_SQL += ", E." + Ope_Pre_Pae_Det_Etapas.Campo_Proceso_Actual;
                    Mi_SQL += ", E." + Ope_Pre_Pae_Det_Etapas.Campo_Estatus;
                    Mi_SQL += ", E." + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa;
                    Mi_SQL += ", (SELECT NVL(SUM(" + Ope_Pre_Pae_Honorarios.Campo_Importe + "-" + Ope_Pre_Pae_Honorarios.Campo_Monto_Pagado + "),0) FROM " + Ope_Pre_Pae_Honorarios.Tabla_Ope_Pre_Pae_Honorarios + " WHERE " + Ope_Pre_Pae_Honorarios.Campo_No_Detalle_Etapa + " = " + "E." + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + ") HONORARIOS_PAGAR";
                    Mi_SQL += ", E." + Ope_Pre_Pae_Det_Etapas.Campo_Omitida;
                    Mi_SQL += ", E." + Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id;
                }
                Mi_SQL += " FROM " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + " E";
                Mi_SQL += " INNER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CU";
                Mi_SQL += " ON E." + Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id + "=CU." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                Mi_SQL += " INNER JOIN " + Ope_Pre_Pae_Etapas.Tabla_Ope_Pre_Pae_Etapas + " ETA";
                Mi_SQL += " ON E." + Ope_Pre_Pae_Det_Etapas.Campo_No_Etapa + "=ETA." + Ope_Pre_Pae_Etapas.Campo_No_Etapa;
                Mi_SQL += " INNER JOIN " + Cat_Pre_Despachos_Externos.Tabla_Cat_Pre_Despachos_Externos + " DE";
                Mi_SQL += " ON ETA." + Ope_Pre_Pae_Etapas.Campo_Despacho_Id + "=DE." + Cat_Pre_Despachos_Externos.Campo_Despacho_Id;

                if (Pae_Etapas.P_Filtro != null && Pae_Etapas.P_Filtro != "")
                {
                    Mi_SQL += " WHERE " + Pae_Etapas.P_Filtro;
                    Segundo_Filtro = true;
                }
                if (Pae_Etapas.P_Cuenta_Predial_Id != null && Pae_Etapas.P_Cuenta_Predial_Id != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += "E." + Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id + " = " + Pae_Etapas.P_Cuenta_Predial_Id;
                    Segundo_Filtro = true;

                }
                if (Pae_Etapas.P_No_Detalle_Etapa != null && Pae_Etapas.P_No_Detalle_Etapa != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += "E." + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + " = " + Pae_Etapas.P_No_Detalle_Etapa;
                    Segundo_Filtro = true;
                }
                if (Pae_Etapas.P_No_Etapa != null && Pae_Etapas.P_No_Etapa != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += "E." + Ope_Pre_Pae_Det_Etapas.Campo_No_Etapa + " = " + Pae_Etapas.P_No_Etapa;
                    Segundo_Filtro = true;
                }
                if (Pae_Etapas.P_Cuenta_Predial != null && Pae_Etapas.P_Cuenta_Predial != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += "CU." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Pae_Etapas.P_Cuenta_Predial + "'";
                    Segundo_Filtro = true;
                }
                if (Pae_Etapas.P_Proceso_Actual != null && Pae_Etapas.P_Proceso_Actual != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += "E." + Ope_Pre_Pae_Det_Etapas.Campo_Proceso_Actual + " = '" + Pae_Etapas.P_Proceso_Actual + "'";
                    Segundo_Filtro = true;
                }
                if (Pae_Etapas.P_Estatus != null && Pae_Etapas.P_Estatus != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += "E." + Ope_Pre_Pae_Det_Etapas.Campo_Estatus + " = '" + Pae_Etapas.P_Estatus + "'";
                    Segundo_Filtro = true;
                }
                if (Pae_Etapas.P_Omitida != null && Pae_Etapas.P_Omitida != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += "E." + Ope_Pre_Pae_Det_Etapas.Campo_Omitida + " = '" + Pae_Etapas.P_Omitida + "'";
                    Segundo_Filtro = true;
                }
                if (Pae_Etapas.P_Impresa != null && Pae_Etapas.P_Impresa != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += "E." + Ope_Pre_Pae_Det_Etapas.Campo_Impresa + " = '" + Pae_Etapas.P_Impresa + "'";
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Folio))//Se utiliza para un unico folio
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += "E." + Ope_Pre_Pae_Det_Etapas.Campo_Folio + "='" + Pae_Etapas.P_Folio + "'";
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Folio_Inicial))//Se utiliza para el rango de los folios
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += "E." + Ope_Pre_Pae_Det_Etapas.Campo_Folio + ">=" + Pae_Etapas.P_Folio_Inicial;
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Folio_Final))//Se utiliza para el rango de los folios
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += "E." + Ope_Pre_Pae_Det_Etapas.Campo_Folio + "<=" + Pae_Etapas.P_Folio_Final;
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Despacho_Id))
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += "DE." + Cat_Pre_Despachos_Externos.Campo_Despacho_Id + "=" + Pae_Etapas.P_Despacho_Id;
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Tipo_Predio))
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += "CU." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + "='" + Pae_Etapas.P_Tipo_Predio + "'";
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Colonia_ID))
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += "CU." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + "='" + Pae_Etapas.P_Colonia_ID + "'";
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Calle_ID))
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += "CU." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + "='" + Pae_Etapas.P_Calle_ID + "'";
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Colonia_ID_Notificacion))
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += "CU." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + "='" + Pae_Etapas.P_Colonia_ID_Notificacion + "'";
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Calle_ID_Notificacion))
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += "CU." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + "='" + Pae_Etapas.P_Calle_ID_Notificacion + "'";
                    Segundo_Filtro = true;
                }
                if (Pae_Etapas.P_Agrupar_Dinamico != null && Pae_Etapas.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Pae_Etapas.P_Agrupar_Dinamico;
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
        ///NOMBRE DE LA FUNCIÓN: Consulta_Reporte_Impresas
        ///DESCRIPCIÓN: Devuelve las cuentas que estan en determinada etapa y no estan impresas
        ///PARAMETROS:      
        ///CREO: Armando Zavala Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consulta_Reporte_Impresas(Cls_Ope_Pre_Pae_Etapas_Negocio Pae_Etapas)
        {
            DataTable Tabla = new DataTable();
            StringBuilder Mi_SQL = new StringBuilder();
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(" PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id);
                Mi_SQL.Append(", CU." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial);
                Mi_SQL.Append(", PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Folio);
                Mi_SQL.Append(", (CON." + Cat_Pre_Contribuyentes.Campo_Nombre + "||' '||" + "CON." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + "||' '||" + "CON." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + ")AS CONTRIBUYENTE");
                Mi_SQL.Append(", CU." + Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal);
                Mi_SQL.Append(", PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Periodo_Rezago);
                Mi_SQL.Append(", PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Rezago);
                Mi_SQL.Append(", PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Periodo_Corriente);
                Mi_SQL.Append(", PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Corriente);
                Mi_SQL.Append(", PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Honorarios + " AS HONORARIOS");
                Mi_SQL.Append(", (PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Moratorios + "+PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Ordinarios+") AS RECARGOS");
                Mi_SQL.Append(", (PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Moratorios + "+PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Ordinarios + "+PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Rezago + ")+PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Corriente+ " AS TOTAL_PAGAR");
                Mi_SQL.Append(", (PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Moratorios + "+PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Ordinarios + "+PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Rezago + "+PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Honorarios + ")+PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Corriente + " AS TOTAL_REQ");
                Mi_SQL.Append(", CU." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID);
                Mi_SQL.Append(",(SELECT " + Cat_Pre_Calles.Campo_Nombre + " FROM " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " WHERE " + Cat_Pre_Calles.Campo_Calle_ID + "=CU." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + ") AS NOM_CALLE");
                Mi_SQL.Append(", CU." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID);
                Mi_SQL.Append(",(SELECT " + Cat_Pre_Calles.Campo_Nombre + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID+ "=CU." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + ") AS NOM_COLONIA");
                Mi_SQL.Append(", CU." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior);
                Mi_SQL.Append(", CU." + Cat_Pre_Cuentas_Predial.Campo_No_Interior);
                Mi_SQL.Append(", CU." + Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo);
                Mi_SQL.Append(", CU." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion);
                Mi_SQL.Append(",(SELECT " + Cat_Pre_Calles.Campo_Nombre + " FROM " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " WHERE " + Cat_Pre_Calles.Campo_Calle_ID + "=CU." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + ") AS NOM_CALLE_NOTI");
                Mi_SQL.Append(", CU." + Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion);
                Mi_SQL.Append(", CU." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion);
                Mi_SQL.Append(",(SELECT " + Cat_Pre_Calles.Campo_Nombre + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID + "=CU." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + ") AS NOM_COLONIA_NOTI");
                Mi_SQL.Append(", CU." + Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion);
                Mi_SQL.Append(", CU." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion);
                Mi_SQL.Append(", CU." + Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion);
                Mi_SQL.Append(", CU." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion);
                Mi_SQL.Append(", CU." + Cat_Pre_Cuentas_Predial.Campo_Efectos);
                Mi_SQL.Append(", CU." + Cat_Pre_Cuentas_Predial.Campo_Tasa_ID);
                Mi_SQL.Append(",(SELECT " + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual + " FROM " + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + " WHERE " + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID + "=CU." + Cat_Pre_Cuentas_Predial.Campo_Tasa_ID + ") AS TASA_ANUAL");
                Mi_SQL.Append(", NVL(ROUND((CU." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + "/6),2),0) AS CUOTA_BIMESTRAL");
                Mi_SQL.Append(",(SELECT " + Ope_Pre_Pae_Notificaciones.Campo_Fecha_Hora + " FROM " + Ope_Pre_Pae_Notificaciones.Tabla_Ope_Pre_Pae_Notificaciones + " WHERE " + Ope_Pre_Pae_Notificaciones.Campo_No_Detalle_Etapa + " = PAE." + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + " AND " + Ope_Pre_Pae_Notificaciones.Campo_Proceso + " = '" + Pae_Etapas.P_Proceso_Actual + "')FECHA_NOTI");
                Mi_SQL.Append(" FROM " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + " PAE");
                Mi_SQL.Append(" INNER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CU");
                Mi_SQL.Append(" ON PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id + "=CU." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID);
                Mi_SQL.Append(" INNER JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " PRO");
                Mi_SQL.Append(" ON CU." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "=PRO." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID);
                Mi_SQL.Append(" INNER JOIN " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " CON");
                Mi_SQL.Append(" ON PRO." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + "=CON." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID);
                Mi_SQL.Append(" INNER JOIN " + Ope_Pre_Pae_Etapas.Tabla_Ope_Pre_Pae_Etapas + " ETA");
                Mi_SQL.Append(" ON PAE." + Ope_Pre_Pae_Det_Etapas.Campo_No_Etapa + "=ETA." + Ope_Pre_Pae_Etapas.Campo_No_Etapa);
                Mi_SQL.Append(" WHERE (PRO." + Cat_Pre_Propietarios.Campo_Tipo + "='PROPIETARIO' OR PRO." + Cat_Pre_Propietarios.Campo_Tipo + "='POSEEDOR')");
                Mi_SQL.Append(" AND PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Omitida + "='NO'");

                if (!String.IsNullOrEmpty(Pae_Etapas.P_Proceso_Actual))
                {
                    Mi_SQL.Append(" AND PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Proceso_Actual + "='" + Pae_Etapas.P_Proceso_Actual + "'");
                }

                if (!String.IsNullOrEmpty(Pae_Etapas.P_Impresa))
                {
                    Mi_SQL.Append(" AND PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Impresa + "='" + Pae_Etapas.P_Impresa + "'");
                }

                if (!String.IsNullOrEmpty(Pae_Etapas.P_Cuenta_Predial))
                {
                    Mi_SQL.Append(" AND CU." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + "='" + Pae_Etapas.P_Cuenta_Predial + "'");
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Colonia_ID))
                {
                    Mi_SQL.Append(" AND CU." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + "='" + Pae_Etapas.P_Colonia_ID + "'");
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Colonia_ID_Notificacion))
                {
                    Mi_SQL.Append(" AND CU." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + "='" + Pae_Etapas.P_Colonia_ID_Notificacion + "'");
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Calle_ID))
                {
                    Mi_SQL.Append(" AND CU." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + "='" + Pae_Etapas.P_Calle_ID + "'");
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Calle_ID_Notificacion))
                {
                    Mi_SQL.Append(" AND CU." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + "='" + Pae_Etapas.P_Calle_ID_Notificacion + "'");
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Despacho_Id))
                {
                    Mi_SQL.Append(" AND ETA." + Ope_Pre_Pae_Etapas.Campo_Despacho_Id + "='" + Pae_Etapas.P_Despacho_Id + "'");
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Cuenta_Predial_Id))
                {
                    Mi_SQL.Append(" AND PAE."+ Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id + " = " + Pae_Etapas.P_Cuenta_Predial_Id);
                }                
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Estatus))
                {
                    Mi_SQL.Append(" AND PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Estatus + " = '" + Pae_Etapas.P_Estatus + "'");
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Folio))//Se utiliza para un unico folio
                { 
                    Mi_SQL.Append(" AND PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Folio + "='" + Pae_Etapas.P_Folio + "'");
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Folio_Inicial))//Se utiliza para el rango de los folios
                {
                    Mi_SQL.Append(" AND PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Folio + ">=" + Pae_Etapas.P_Folio_Inicial);
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Folio_Final))//Se utiliza para el rango de los folios
                {
                    Mi_SQL.Append(" AND PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Folio + "<=" + Pae_Etapas.P_Folio_Final);
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Tipo_Predio))
                {
                    Mi_SQL.Append(" AND CU." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + "='" + Pae_Etapas.P_Tipo_Predio + "'");
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Fecha_Creo_Fin))//Se utiliza para el rango de los folios
                {
                    Mi_SQL.Append(" AND PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Fecha_Creo + ">='" + Pae_Etapas.P_Fecha_Creo_Fin + "'");
                }
                if (!String.IsNullOrEmpty(Pae_Etapas.P_Fecha_Creo_Fin))
                {
                    Mi_SQL.Append(" AND PAE." + Ope_Pre_Pae_Det_Etapas.Campo_Fecha_Creo + "<='" + Pae_Etapas.P_Fecha_Creo_Fin + "'");
                }

                Mi_SQL.Append(" ORDER BY PAE." + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa);
                DataSet Ds_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                if (Ds_Consulta != null && Ds_Consulta.Tables.Count > 0)
                {
                    Tabla = Ds_Consulta.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de las cuentas que estan en el modulo del PAE. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
    }
}