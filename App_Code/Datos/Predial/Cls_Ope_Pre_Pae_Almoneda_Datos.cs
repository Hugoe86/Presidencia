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
using Presidencia.Predial_Pae_Almonedas.Negocio;

namespace Presidencia.Predial_Pae_Almonedas.Datos
{
    public class Cls_Ope_Pre_Pae_Almoneda_Datos
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
        ///NOMBRE DE LA FUNCIÓN: Alta_Pae_Almonedas
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nueva Almoneda de una determinada cuenta
        ///PARAMENTROS:     
        ///CREO: Armando Zavala Moreno
        ///FECHA_CREO: 26/Abril/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO 
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
        public static void Alta_Pae_Almonedas(Cls_Ope_Pre_Pae_Almoneda_Negocios Almoneda)
        {
            StringBuilder MI_SQL = new StringBuilder();
            try
            {
                String No_Almoneda = Obtener_ID_Consecutivo(Ope_Pre_Pae_Almonedas.Tabla_Ope_Pre_Pae_Almonedas, Ope_Pre_Pae_Almonedas.Campo_No_Almoneda, 10);
                MI_SQL.Append("INSERT INTO " + Ope_Pre_Pae_Almonedas.Tabla_Ope_Pre_Pae_Almonedas);
                MI_SQL.Append(" (" + Ope_Pre_Pae_Almonedas.Campo_No_Almoneda + ", " + Ope_Pre_Pae_Almonedas.Campo_No_Detalle_Etapa + ", ");
                MI_SQL.Append(Ope_Pre_Pae_Almonedas.Campo_Numero_Almoneda_Cuenta + ", " + Ope_Pre_Pae_Almonedas.Campo_Valor_Avaluo + ", ");
                MI_SQL.Append(Ope_Pre_Pae_Almonedas.Campo_Estatus + ", " + Ope_Pre_Pae_Almonedas.Campo_Fecha + ", ");
                MI_SQL.Append(Ope_Pre_Pae_Almonedas.Campo_Usuario_Creo + ", " + Ope_Pre_Pae_Almonedas.Campo_Fecha_Creo + ")");
                MI_SQL.Append(" VALUES ('" + No_Almoneda + "' ");
                MI_SQL.Append(",'" + Almoneda.P_No_Detalle_Etapa + "' ");
                MI_SQL.Append(",'" + Almoneda.P_Numero_Almoneda_Cuenta + "' ");
                MI_SQL.Append(",'" + Almoneda.P_Valor_Avaluo + "' ");
                MI_SQL.Append(",'" + Almoneda.P_Estatus + "' ");
                MI_SQL.Append(", sysdate");
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
        ///NOMBRE DE LA FUNCIÓN: Alta_Pae_Almonedas
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nueva Almoneda de una determinada cuenta
        ///PARAMENTROS:     
        ///CREO: Armando Zavala Moreno
        ///FECHA_CREO: 26/Abril/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO 
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
        public static DataTable Consulta_Det_Etapas_Almonedas_Remocion(Cls_Ope_Pre_Pae_Almoneda_Negocios Almoneda)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;
            Boolean Segundo_Filtro = false;
            try
            {
                Mi_SQL = "SELECT " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + ".* ," + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial;
                Mi_SQL += ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID;
                Mi_SQL += ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion;
                Mi_SQL += ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion;
                Mi_SQL += ", (SELECT " + Ope_Pre_Pae_Notificaciones.Campo_Fecha_Hora + " FROM " + Ope_Pre_Pae_Notificaciones.Tabla_Ope_Pre_Pae_Notificaciones + " WHERE " + Ope_Pre_Pae_Notificaciones.Campo_No_Detalle_Etapa + " = " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + " AND " + Ope_Pre_Pae_Notificaciones.Campo_Proceso + "='" + Almoneda.P_Proceso_Actual + "')";
                Mi_SQL += " FECHA_NOTIFICACION";
                Mi_SQL += ", (SELECT NVL(SUM(" + Ope_Pre_Pae_Honorarios.Campo_Importe + "-" + Ope_Pre_Pae_Honorarios.Campo_Monto_Pagado + "),0) FROM " + Ope_Pre_Pae_Honorarios.Tabla_Ope_Pre_Pae_Honorarios + " WHERE " + Ope_Pre_Pae_Honorarios.Campo_No_Detalle_Etapa + " = " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + ")";
                Mi_SQL += " SUMA_HONORARIOS";
                Mi_SQL += ", (" + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Corriente + " + " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Rezago;
                Mi_SQL += " + " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Ordinarios + " + " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Moratorios;
                Mi_SQL += " + (SELECT NVL(SUM(" + Ope_Pre_Pae_Honorarios.Campo_Importe + "-" + Ope_Pre_Pae_Honorarios.Campo_Monto_Pagado + "),0) FROM " + Ope_Pre_Pae_Honorarios.Tabla_Ope_Pre_Pae_Honorarios + " WHERE " + Ope_Pre_Pae_Honorarios.Campo_No_Detalle_Etapa + " = " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + ")";
                Mi_SQL += " + " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Multas;
                Mi_SQL += ") TOTAL";
                Mi_SQL += ", (SELECT " + Ope_Pre_Pae_Peritajes.Campo_Valor + " FROM " + Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes + " WHERE " + Ope_Pre_Pae_Peritajes.Campo_No_Detalle_Etapa + "=" + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + " AND " + Ope_Pre_Pae_Peritajes.Campo_No_Peritaje + "=(SELECT MAX(" + Ope_Pre_Pae_Peritajes.Campo_No_Peritaje + ") FROM " + Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes + " WHERE " + Ope_Pre_Pae_Peritajes.Campo_No_Detalle_Etapa + "=" + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa+")) AS AVALUO_PERITAJE";
                Mi_SQL += ", " + Ope_Pre_Pae_Almonedas.Tabla_Ope_Pre_Pae_Almonedas + "." + Ope_Pre_Pae_Almonedas.Campo_No_Almoneda;
                Mi_SQL += ", NVL(" + Ope_Pre_Pae_Almonedas.Tabla_Ope_Pre_Pae_Almonedas + "." + Ope_Pre_Pae_Almonedas.Campo_Numero_Almoneda_Cuenta + ",0)NUMERO_ALMONEDA, " + Ope_Pre_Pae_Almonedas.Tabla_Ope_Pre_Pae_Almonedas + "." + Ope_Pre_Pae_Almonedas.Campo_Valor_Avaluo;
                Mi_SQL += ", " + Ope_Pre_Pae_Almonedas.Tabla_Ope_Pre_Pae_Almonedas + "." + Ope_Pre_Pae_Almonedas.Campo_Estatus + " AS ESTATUS_ALMONEDA, " + Ope_Pre_Pae_Almonedas.Tabla_Ope_Pre_Pae_Almonedas + "." + Ope_Pre_Pae_Almonedas.Campo_Fecha;
                Mi_SQL += ", " + Cat_Pre_Despachos_Externos.Tabla_Cat_Pre_Despachos_Externos + "." + Cat_Pre_Despachos_Externos.Campo_Despacho;
                Mi_SQL += ", " + Ope_Pre_Pae_Etapas.Tabla_Ope_Pre_Pae_Etapas + "." + Ope_Pre_Pae_Etapas.Campo_Numero_Entrega;
               
                Mi_SQL += " FROM " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas;
                Mi_SQL += " INNER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;
                Mi_SQL += " ON " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id + "=" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
 
                Mi_SQL += " LEFT OUTER JOIN " + Ope_Pre_Pae_Almonedas.Tabla_Ope_Pre_Pae_Almonedas;
                Mi_SQL += " ON " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + "=" + Ope_Pre_Pae_Almonedas.Tabla_Ope_Pre_Pae_Almonedas + "." + Ope_Pre_Pae_Almonedas.Campo_No_Detalle_Etapa;
                Mi_SQL += " INNER JOIN " + Ope_Pre_Pae_Etapas.Tabla_Ope_Pre_Pae_Etapas;
                Mi_SQL += " ON " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Etapa + "=" + Ope_Pre_Pae_Etapas.Tabla_Ope_Pre_Pae_Etapas + "." + Ope_Pre_Pae_Etapas.Campo_No_Etapa;
                Mi_SQL += " INNER JOIN " + Cat_Pre_Despachos_Externos.Tabla_Cat_Pre_Despachos_Externos;
                Mi_SQL += " ON " + Ope_Pre_Pae_Etapas.Tabla_Ope_Pre_Pae_Etapas + "." + Ope_Pre_Pae_Etapas.Campo_Despacho_Id + "=" + Cat_Pre_Despachos_Externos.Tabla_Cat_Pre_Despachos_Externos + "." + Cat_Pre_Despachos_Externos.Campo_Despacho_Id;

                if (!String.IsNullOrEmpty(Almoneda.P_Cuenta_Predial))
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + "='" + Almoneda.P_Cuenta_Predial + "'";
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Almoneda.P_Despacho_Id))
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Cat_Pre_Despachos_Externos.Tabla_Cat_Pre_Despachos_Externos + "." + Cat_Pre_Despachos_Externos.Campo_Despacho_Id + "=" + Almoneda.P_Despacho_Id;
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Almoneda.P_Folio))
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Folio + "='" + Almoneda.P_Folio + "'";
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Almoneda.P_Folio_Inicial))
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Folio + ">=" + Almoneda.P_Folio_Inicial;
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Almoneda.P_Folio_Final))
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Folio + "<=" + Almoneda.P_Folio_Final;
                    Segundo_Filtro = true;
                }
                
                if (!String.IsNullOrEmpty(Almoneda.P_Colonia_ID))
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + "='" + Almoneda.P_Colonia_ID + "'";
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Almoneda.P_Calle_ID))
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + "='" + Almoneda.P_Calle_ID + "'";
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Almoneda.P_Colonia_ID_Notificacion))
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + "='" + Almoneda.P_Colonia_ID_Notificacion + "'";
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Almoneda.P_Calle_ID_Notificacion))
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + "='" + Almoneda.P_Calle_ID_Notificacion + "'";
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Almoneda.P_Tipo_Predio))
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + "='" + Almoneda.P_Tipo_Predio + "'";
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Almoneda.P_Estatus_Etapa))
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Estatus + "='" + Almoneda.P_Estatus_Etapa + "'";
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Almoneda.P_Proceso_Actual))
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Proceso_Actual + "='" + Almoneda.P_Proceso_Actual + "'";
                    Segundo_Filtro = true;
                }
                if (!String.IsNullOrEmpty(Almoneda.P_Omitida))
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Omitida + "='" + Almoneda.P_Omitida + "'";
                    Segundo_Filtro = true;
                }

                Mi_SQL += " ORDER BY " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa;
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
