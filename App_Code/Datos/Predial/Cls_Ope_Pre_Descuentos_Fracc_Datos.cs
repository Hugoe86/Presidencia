using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Ope_Pre_Descuentos_Fracc.Negocio;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using System.Data;

/// <summary>
/// Summary description for Cls_Ope_Pre_Descuentos_Fracc_Datos
/// </summary>

namespace Presidencia.Ope_Pre_Descuentos_Fracc.Datos
{
    public class Cls_Ope_Pre_Descuentos_Fracc_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Descuentos
        ///DESCRIPCIÓN: Obtiene los descuentos deseados
        ///PARAMENTROS:   
        ///             1.  Descuento.      Contiene los datos de los descuentos.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 20/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Descuentos(Cls_Ope_Pre_Descuentos_Fracc_Negocio Descuento)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT CP." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " AS CUENTA_PREDIAL ";
                Mi_SQL += ", CP." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " AS CUENTA_PREDIAL_ID ";
                if (Descuento.P_Campos_Dinamicos == null || Descuento.P_Campos_Dinamicos == "")
                {
                    Mi_SQL += ", DESCU." + Ope_Pre_Descuento_Fracc.Campo_No_Descuento;
                    Mi_SQL += ", DESCU." + Ope_Pre_Descuento_Fracc.Campo_No_Impuesto_fraccionamiento;
                    Mi_SQL += ", DESCU." + Ope_Pre_Descuento_Fracc.Campo_Desc_Multa;
                    Mi_SQL += ", DESCU." + Ope_Pre_Descuento_Fracc.Campo_Desc_Recargo;
                    Mi_SQL += ", DESCU." + Ope_Pre_Descuento_Fracc.Campo_Estatus;
                    Mi_SQL += ", DESCU." + Ope_Pre_Descuento_Fracc.Campo_Fecha_Inicial;
                    Mi_SQL += ", DESCU." + Ope_Pre_Descuento_Fracc.Campo_Fecha_Vencimiento;
                    Mi_SQL += ", DESCU." + Ope_Pre_Descuento_Fracc.Campo_Monto_Multas;
                    Mi_SQL += ", DESCU." + Ope_Pre_Descuento_Fracc.Campo_Monto_Recargos;
                    Mi_SQL += ", DESCU." + Ope_Pre_Descuento_Fracc.Campo_Realizo;
                    Mi_SQL += ", DESCU." + Ope_Pre_Descuento_Fracc.Campo_Total_Por_Pagar;
                    Mi_SQL += ", DESCU." + Ope_Pre_Descuento_Fracc.Campo_Observaciones;
                    Mi_SQL += ", DESCU." + Ope_Pre_Descuento_Fracc.Campo_Referencia;
                }
                else
                {
                    Mi_SQL += Descuento.P_Campos_Dinamicos;
                }
                Mi_SQL += " FROM " + Ope_Pre_Descuento_Fracc.Tabla_Ope_Pre_Descuento_Fracc + " DESCU";
                Mi_SQL += " LEFT OUTER JOIN " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + " IMP ";
                Mi_SQL += " ON DESCU." + Ope_Pre_Descuento_Fracc.Campo_No_Impuesto_fraccionamiento + "=IMP." + Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento;
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CP ";
                ; Mi_SQL += " ON IMP." + Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID + "=CP." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                Mi_SQL += " WHERE ";
                if (Descuento.P_Filtros_Dinamicos == null || Descuento.P_Filtros_Dinamicos == "")
                {
                    if (Descuento.P_No_Descuento != null && Descuento.P_No_Descuento != "")
                    {
                        Mi_SQL += " DESCU." + Ope_Pre_Descuento_Fracc.Campo_No_Descuento + "='" + Descuento.P_No_Descuento + "' AND ";
                    }
                    if (Descuento.P_Cuenta_Predial != null && Descuento.P_Cuenta_Predial != "")
                    {
                        Mi_SQL += " CP." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + "='" + Descuento.P_Cuenta_Predial + "' AND ";
                    }
                    if (Descuento.P_No_Impuesto_Fracc != null && Descuento.P_No_Impuesto_Fracc != "")
                    {
                        Mi_SQL += " DESCU." + Ope_Pre_Descuento_Fracc.Campo_No_Impuesto_fraccionamiento + "='" + Descuento.P_No_Impuesto_Fracc + "' AND ";
                    }
                    if (Descuento.P_Estatus != null && Descuento.P_Estatus != "")
                    {
                        //Realiza tu propia consulta para el estatus...
                        Mi_SQL += " DESCU." + Ope_Pre_Descuento_Fracc.Campo_Estatus + Descuento.P_Estatus + " AND ";
                    }
                    if (Mi_SQL.EndsWith(" AND "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                    }
                    if (Mi_SQL.EndsWith(" WHERE "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                    }
                }
                else
                {
                    Mi_SQL += Descuento.P_Filtros_Dinamicos;
                }
                if (Descuento.P_Agrupadores_Dinamicos != null && Descuento.P_Agrupadores_Dinamicos != "")
                {
                    Mi_SQL += " GROUP BY " + Descuento.P_Agrupadores_Dinamicos;
                }
                if (Descuento.P_Orden_Dinamico != null && Descuento.P_Orden_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Descuento.P_Orden_Dinamico;
                }
                else
                {
                    Mi_SQL += " ORDER BY DESCU." + Ope_Pre_Descuento_Fracc.Campo_Fecha_Inicial + " ASC, CP." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " ASC";
                }
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los descuentos de Fraccionamiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Descuentos
        ///DESCRIPCIÓN: Obtiene los descuentos deseados
        ///PARAMENTROS:   
        ///             1.  Descuento.      Contiene los datos de los descuentos.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 20/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Descuento_Activo(Cls_Ope_Pre_Descuentos_Fracc_Negocio Descuento)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Ope_Pre_Descuento_Fracc.Campo_No_Descuento;
                Mi_SQL += " FROM " + Ope_Pre_Descuento_Fracc.Tabla_Ope_Pre_Descuento_Fracc;
                Mi_SQL += " WHERE " + Ope_Pre_Descuento_Fracc.Campo_No_Impuesto_fraccionamiento + "='" + Descuento.P_No_Impuesto_Fracc + "'";
                Mi_SQL += " AND " + Ope_Pre_Descuento_Fracc.Campo_Estatus + " IN ('ACTIVO','BAJA')";

                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los descuentos de Fraccionamiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Descuento
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un Descuento
        ///PARAMENTROS:     
        ///             1. Descuento.       Instancia de la Clase de Negocio de Descuentos 
        ///                                 con los datos que van a ser
        ///                                 dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 19/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Descuento(Cls_Ope_Pre_Descuentos_Fracc_Negocio Descuento)
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

                String Descuento_Id = Obtener_ID_Consecutivo(Ope_Pre_Descuento_Fracc.Tabla_Ope_Pre_Descuento_Fracc, Ope_Pre_Descuento_Fracc.Campo_No_Descuento, 10);
                String Mi_SQL = "INSERT INTO " + Ope_Pre_Descuento_Fracc.Tabla_Ope_Pre_Descuento_Fracc;
                Mi_SQL += " (" + Ope_Pre_Descuento_Fracc.Campo_No_Descuento;
                Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Estatus;
                Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Fecha_Inicial;
                Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Desc_Multa;
                Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Desc_Recargo;
                Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Total_Por_Pagar;
                Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Realizo;
                Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Fecha_Vencimiento;
                Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Observaciones;
                Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Fundamento_Legal;
                Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_No_Impuesto_fraccionamiento;
                Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Referencia;
                Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Monto_Recargos;
                Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Monto_Multas;
                Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Usuario_Creo;
                Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Fecha_Creo + ")";
                Mi_SQL += " VALUES";
                Mi_SQL += " ('" + Descuento_Id + "'";
                Mi_SQL += ", '" + Descuento.P_Estatus + "'";
                Mi_SQL += ", '" + Descuento.P_Fecha.ToString("d-M-yyyy") + "'";
                Mi_SQL += ", " + Descuento.P_Desc_Multa;
                Mi_SQL += ", " + Descuento.P_Desc_Recargo;
                Mi_SQL += ", " + Descuento.P_Total_Por_Pagar;
                Mi_SQL += ", '" + Descuento.P_Realizo + "'";
                Mi_SQL += ", '" + Descuento.P_Fecha_Vencimiento.ToString("d-M-yyyy") + "'";
                Mi_SQL += ", '" + Descuento.P_Observaciones + "'";
                if (Descuento.P_Fundamento_Legal != null)
                {
                    Mi_SQL += ", '" + Descuento.P_Fundamento_Legal + "'";
                }
                else
                {
                    Mi_SQL += ", NULL";
                }
                Mi_SQL += ", '" + Descuento.P_No_Impuesto_Fracc + "'";
                if (Descuento.P_Referencia != null)
                {
                    Mi_SQL += ", '" + Descuento.P_Referencia + "'";
                }
                else
                {
                    Mi_SQL += ", NULL";
                }
                Mi_SQL += ", " + Descuento.P_Monto_Recargos;
                Mi_SQL += ", " + Descuento.P_Monto_Multas;
                Mi_SQL += ", '" + Descuento.P_Usuario_Creo + "'";
                Mi_SQL += ", SYSDATE)";
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Descuento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Descuento
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un descuento
        ///PARAMENTROS:     
        ///             1. Descuento.       Instancia de la Clase de descuentos
        ///                                 con los datos del Registro 
        ///                                 que va a ser Actualizado.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 19/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Descuento(Cls_Ope_Pre_Descuentos_Fracc_Negocio Descuento)
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
                String Mi_SQL = "UPDATE " + Ope_Pre_Descuento_Fracc.Tabla_Ope_Pre_Descuento_Fracc;
                Mi_SQL += " SET " + Ope_Pre_Descuento_Fracc.Campo_Estatus + "='" + Descuento.P_Estatus + "'";
                Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Desc_Multa + "=" + Descuento.P_Desc_Multa;
                Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Desc_Recargo + "=" + Descuento.P_Desc_Recargo;
                Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Fecha_Inicial + "='" + Descuento.P_Fecha.ToString("d-M-yyyy") + "'";
                Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Fecha_Vencimiento + "='" + Descuento.P_Fecha_Vencimiento.ToString("d-M-yyyy") + "'";
                if (Descuento.P_Fundamento_Legal != null)
                {
                    Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Fundamento_Legal + "='" + Descuento.P_Fundamento_Legal + "'";
                }
                else
                {
                    Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Fundamento_Legal + "= NULL";
                }
                Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Monto_Multas + "=" + Descuento.P_Monto_Multas;
                Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Monto_Recargos + "=" + Descuento.P_Monto_Recargos;
                Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Observaciones + "='" + Descuento.P_Observaciones + "'";
                if (Descuento.P_Referencia != null)
                {
                    Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Referencia + "='" + Descuento.P_Referencia + "'";
                }
                else
                {
                    Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Referencia + "= NULL";
                }
                Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Total_Por_Pagar + "=" + Descuento.P_Total_Por_Pagar;
                Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Usuario_Modifico + "='" + Descuento.P_Usuario_Modifico + "'";
                Mi_SQL += ", " + Ope_Pre_Descuento_Fracc.Campo_Fecha_Modifico + "=SYSDATE";
                Mi_SQL += " WHERE " + Ope_Pre_Descuento_Fracc.Campo_No_Descuento + "='" + Descuento.P_No_Descuento + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Descuentos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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

    }
}