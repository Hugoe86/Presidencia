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
using Presidencia.Operacion_Predial_Convenios_Fraccionamientos.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Sessiones;
using System.Data.OracleClient;

/// <summary>
/// Summary description for Cls_Ope_Pre_Convenios_Fraccionamientos_Datos
/// </summary>

namespace Presidencia.Operacion_Predial_Convenios_Fraccionamientos.Datos
{

    public class Cls_Ope_Pre_Convenios_Fraccionamientos_Datos
    {
        #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Convenio_Fraccionamiento
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo registro de Convenio_Fraccionamiento
        ///PARAMETROS           : 1. Convenio_Fraccionamiento.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Derecho_Supervisions_Negocio
        ///                                 con los datos de Derecho_Supervisions que va a ser dado de Alta.
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 22/Julio/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Alta_Convenio_Fraccionamiento(Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio Convenio_Fraccionamiento)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Boolean Alta = false;

            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String No_Convenio = Obtener_ID_Consecutivo(Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos, Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio, "", 10);
            try
            {
                String Mi_SQL = "INSERT INTO " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + " (";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Propietario_ID + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Realizo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_No_Reestructura + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Solicitante + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_RFC + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Numero_Parcialidades + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Periodicidad_Pago + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Observaciones + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Descuento_Recargos_Ordinarios + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Descuento_Recargos_Moratorios + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Descuento_Multas + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Adeudo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Descuento + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Sub_Total + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Porcentaje_Anticipo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Anticipo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Convenio + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Anticipo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_No_Descuento + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Anio + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Usuario_Creo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha_Creo + ") ";
                Mi_SQL += "VALUES ('";
                Mi_SQL += No_Convenio + "', ";
                if (Convenio_Fraccionamiento.P_Cuenta_Predial_ID != "" && Convenio_Fraccionamiento.P_Cuenta_Predial_ID != null)
                {
                    Mi_SQL += "'" + Convenio_Fraccionamiento.P_Cuenta_Predial_ID + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Convenio_Fraccionamiento.P_Propietario_ID != "" && Convenio_Fraccionamiento.P_Propietario_ID != null)
                {
                    Mi_SQL += "'" + Convenio_Fraccionamiento.P_Propietario_ID + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Convenio_Fraccionamiento.P_Realizo != "" && Convenio_Fraccionamiento.P_Realizo != null)
                {
                    Mi_SQL += "'" + Convenio_Fraccionamiento.P_Realizo + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Convenio_Fraccionamiento.P_No_Reestructura != null)
                {
                    Mi_SQL += Convenio_Fraccionamiento.P_No_Reestructura + ", ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Convenio_Fraccionamiento.P_Estatus != "" && Convenio_Fraccionamiento.P_Estatus != null)
                {
                    Mi_SQL += "'" + Convenio_Fraccionamiento.P_Estatus + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Convenio_Fraccionamiento.P_Solicitante != "" && Convenio_Fraccionamiento.P_Solicitante != null)
                {
                    Mi_SQL += "'" + Convenio_Fraccionamiento.P_Solicitante + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Convenio_Fraccionamiento.P_RFC != "" && Convenio_Fraccionamiento.P_RFC != null)
                {
                    Mi_SQL += "'" + Convenio_Fraccionamiento.P_RFC + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                Mi_SQL += Convenio_Fraccionamiento.P_Numero_Parcialidades + ", ";
                if (Convenio_Fraccionamiento.P_Periodicidad_Pago != "" && Convenio_Fraccionamiento.P_Periodicidad_Pago != null)
                {
                    Mi_SQL += "'" + Convenio_Fraccionamiento.P_Periodicidad_Pago + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Convenio_Fraccionamiento.P_Fecha.ToString() != "" && Convenio_Fraccionamiento.P_Fecha != null)
                {
                    Mi_SQL += "'" + Convenio_Fraccionamiento.P_Fecha.ToString("d-M-yyyy") + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Convenio_Fraccionamiento.P_Fecha_Vencimiento.ToString() != "" && Convenio_Fraccionamiento.P_Fecha_Vencimiento != null)
                {
                    Mi_SQL += "'" + Convenio_Fraccionamiento.P_Fecha_Vencimiento.ToString("d-M-yyyy") + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Convenio_Fraccionamiento.P_Observaciones != "" && Convenio_Fraccionamiento.P_Observaciones != null)
                {
                    Mi_SQL += "'" + Convenio_Fraccionamiento.P_Observaciones + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                Mi_SQL += Convenio_Fraccionamiento.P_Descuento_Recargos_Ordinarios + ", ";
                Mi_SQL += Convenio_Fraccionamiento.P_Descuento_Recargos_Moratorios + ", ";
                Mi_SQL += Convenio_Fraccionamiento.P_Descuento_Multas + ", ";
                Mi_SQL += Convenio_Fraccionamiento.P_Total_Adeudo + ", ";
                Mi_SQL += Convenio_Fraccionamiento.P_Total_Descuento + ", ";
                Mi_SQL += Convenio_Fraccionamiento.P_Sub_Total + ", ";
                Mi_SQL += Convenio_Fraccionamiento.P_Porcentaje_Anticipo + ", ";
                Mi_SQL += Convenio_Fraccionamiento.P_Total_Anticipo + ", ";
                Mi_SQL += Convenio_Fraccionamiento.P_Total_Convenio + ", ";
                Mi_SQL += "'POR PAGAR', ";
                if (Convenio_Fraccionamiento.P_No_Impuesto_Fraccionamiento != "" && Convenio_Fraccionamiento.P_No_Impuesto_Fraccionamiento != null)
                {
                    Mi_SQL += "'" + Convenio_Fraccionamiento.P_No_Impuesto_Fraccionamiento + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Convenio_Fraccionamiento.P_No_Descuento != "" && Convenio_Fraccionamiento.P_No_Descuento != null)
                {
                    Mi_SQL += "'" + Convenio_Fraccionamiento.P_No_Descuento + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                Mi_SQL += Convenio_Fraccionamiento.P_Anio + ", ";
                Mi_SQL += "'" + Convenio_Fraccionamiento.P_Usuario + "', SYSDATE)";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //Inserta los Detalles de las Parcialidades de los Convenios
                if (Convenio_Fraccionamiento.P_Dt_Parcialidades != null
                    && Convenio_Fraccionamiento.P_Dt_Parcialidades.Rows.Count > 0)
                {
                    foreach (DataRow Dr_Detalle in Convenio_Fraccionamiento.P_Dt_Parcialidades.Rows)
                    {
                        Mi_SQL = "INSERT INTO " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + " (";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Usuario_Creo + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Creo + ") ";
                        Mi_SQL += "VALUES (";
                        Mi_SQL += "'" + No_Convenio + "', ";
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento] != null)
                        {
                            Mi_SQL += "'" + Convert.ToDateTime(Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento]).ToString("d-M-yyyy") + "', ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus] != null)
                        {
                            Mi_SQL += "'" + Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus].ToString() + "', ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        Mi_SQL += "'" + Convenio_Fraccionamiento.P_Usuario + "', SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        Convenio_Fraccionamiento.P_No_Convenio = No_Convenio;
                    }
                }

                Trans.Commit();
                Alta = true;
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Impuesto por Derecho de Supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Reestructura_Convenio_Fraccionamiento
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo registro de Convenio_Fraccionamiento
        ///PARAMETROS           : 1. Convenio_Fraccionamiento.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Derecho_Supervisions_Negocio
        ///                                 con los datos de Derecho_Supervisions que va a ser dado de Alta.
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 10/Agosto/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Alta_Reestructura_Convenio_Fraccionamiento(Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio Convenio_Fraccionamiento)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Boolean Alta = false;

            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String No_Reestructura = Obtener_ID_Consecutivo(Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos, Ope_Pre_Convenios_Fraccionamientos.Campo_No_Reestructura, Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + Convenio_Fraccionamiento.P_No_Convenio + "'", 10);
            try
            {
                String Mi_SQL = "UPDATE " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + " SET ";
                //Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Cuenta_Predial_ID + " = '" + Convenio_Fraccionamiento.P_Cuenta_Predial_ID + "', ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_No_Reestructura + " = '" + No_Reestructura + "', ";
                if (Convenio_Fraccionamiento.P_Propietario_ID != "" && Convenio_Fraccionamiento.P_Propietario_ID != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Propietario_ID + " = '" + Convenio_Fraccionamiento.P_Propietario_ID + "', ";
                }
                if (Convenio_Fraccionamiento.P_Realizo != "" && Convenio_Fraccionamiento.P_Realizo != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Realizo + " = '" + Convenio_Fraccionamiento.P_Realizo + "', ";
                }
                if (Convenio_Fraccionamiento.P_No_Descuento != null && Convenio_Fraccionamiento.P_No_Descuento != "")
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_No_Descuento + " = '" + Convenio_Fraccionamiento.P_No_Descuento + "', ";
                }
                if (Convenio_Fraccionamiento.P_Estatus != "" && Convenio_Fraccionamiento.P_Estatus != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus + " = '" + Convenio_Fraccionamiento.P_Estatus + "', ";
                }
                if (Convenio_Fraccionamiento.P_Solicitante != "" && Convenio_Fraccionamiento.P_Solicitante != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Solicitante + " = '" + Convenio_Fraccionamiento.P_Solicitante + "', ";
                }
                if (Convenio_Fraccionamiento.P_RFC != "" && Convenio_Fraccionamiento.P_RFC != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_RFC + " = '" + Convenio_Fraccionamiento.P_RFC + "', ";
                }
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Numero_Parcialidades + " = " + Convenio_Fraccionamiento.P_Numero_Parcialidades + ", ";
                if (Convenio_Fraccionamiento.P_Periodicidad_Pago != "" && Convenio_Fraccionamiento.P_Periodicidad_Pago != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Periodicidad_Pago + " = '" + Convenio_Fraccionamiento.P_Periodicidad_Pago + "', ";
                }
                if (Convenio_Fraccionamiento.P_Fecha.ToString() != "" && Convenio_Fraccionamiento.P_Fecha != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha + " = '" + Convenio_Fraccionamiento.P_Fecha.ToString("d-M-yyyy") + "', ";
                }
                if (Convenio_Fraccionamiento.P_Fecha_Vencimiento.ToString() != "" && Convenio_Fraccionamiento.P_Fecha_Vencimiento != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento + " = '" + Convenio_Fraccionamiento.P_Fecha_Vencimiento.ToString("d-M-yyyy") + "', ";
                }
                if (Convenio_Fraccionamiento.P_Observaciones != "" && Convenio_Fraccionamiento.P_Observaciones != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Observaciones + " = '" + Convenio_Fraccionamiento.P_Observaciones + "', ";
                }
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Descuento_Recargos_Ordinarios + " = " + Convenio_Fraccionamiento.P_Descuento_Recargos_Ordinarios + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Descuento_Recargos_Moratorios + " = " + Convenio_Fraccionamiento.P_Descuento_Recargos_Moratorios + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Descuento_Multas + " = " + Convenio_Fraccionamiento.P_Descuento_Multas + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Adeudo + " = " + Convenio_Fraccionamiento.P_Total_Adeudo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Descuento + " = " + Convenio_Fraccionamiento.P_Total_Descuento + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Sub_Total + " = " + Convenio_Fraccionamiento.P_Sub_Total + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Porcentaje_Anticipo + " = " + Convenio_Fraccionamiento.P_Porcentaje_Anticipo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Anticipo + " = " + Convenio_Fraccionamiento.P_Total_Anticipo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Convenio + " = " + Convenio_Fraccionamiento.P_Total_Convenio + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Convenio_Fraccionamiento.P_Usuario + "', ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Anticipo_Reestructura + " = 'POR PAGAR', ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL += "WHERE " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + Convenio_Fraccionamiento.P_No_Convenio + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //Cancela las parcialidades anteriores los Detalles de los Impuestos
                Mi_SQL = "UPDATE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos;
                Mi_SQL += " SET " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + "='CANCELADO'";
                Mi_SQL += " WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + Convenio_Fraccionamiento.P_No_Convenio + "'";
                Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + "!='CANCELADO'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //Inserta los Detalles de las Parcialidades de los Convenios
                if (Convenio_Fraccionamiento.P_Dt_Parcialidades != null
                    && Convenio_Fraccionamiento.P_Dt_Parcialidades.Rows.Count > 0)
                {
                    foreach (DataRow Dr_Detalle in Convenio_Fraccionamiento.P_Dt_Parcialidades.Rows)
                    {
                        Mi_SQL = "INSERT INTO " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + " (";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Usuario_Creo + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Creo + ") ";
                        Mi_SQL += "VALUES (";
                        Mi_SQL += "'" + Convenio_Fraccionamiento.P_No_Convenio + "', ";
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento] != null)
                        {
                            Mi_SQL += "'" + Convert.ToDateTime(Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento]).ToString("d-M-yyyy") + "', ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus] != null)
                        {
                            Mi_SQL += "'" + Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus].ToString() + "', ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        Mi_SQL += "'" + No_Reestructura + "', ";
                        Mi_SQL += "'" + Convenio_Fraccionamiento.P_Usuario + "', SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        Convenio_Fraccionamiento.P_No_Reestructura = "" + No_Reestructura;
                    }
                }

                Trans.Commit();
                Alta = true;
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Impuesto por Derecho de Supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cancelar_Pasivo
        ///DESCRIPCIÓN          : Modifica en la Base de Datos el registro indicado por la referencia
        ///PARAMETROS          : 1. Referencia.   Referencia con la cual se modificara el registro a cancelado
        ///CREO                 : Miguel Angel Bedolla Moreno
        ///FECHA_CREO           : 28/Septiembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Cancelar_Pasivo(String Referencia, String Monto)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Boolean Actualizar = false;

            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try
            {
                String Mi_SQL = "UPDATE " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " SET ";
                Mi_SQL += Ope_Ing_Pasivo.Campo_Estatus + " = 'POR PAGAR', ";
                Mi_SQL += Ope_Ing_Pasivo.Campo_Monto + " = " + Convert.ToDouble(Monto) + ", ";
                Mi_SQL += Ope_Ing_Pasivo.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "', ";
                Mi_SQL += Ope_Ing_Pasivo.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL += "WHERE " + Ope_Ing_Pasivo.Campo_Referencia + " = '" + Referencia + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                Trans.Commit();
                Actualizar = true;
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
                    Mensaje = "Error al intentar modificar un el pasivo de Impuesto de Fraccionamiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
            return Actualizar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Convenio_Fraccionamiento
        ///DESCRIPCIÓN          : Modifica en la Base de Datos el registro indicado del Convenio_Fraccionamiento
        ///PARAMETROS          : 1. Convenio_Fraccionamiento.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Derecho_Supervisions_Negocio
        ///                                 con los datos del Derecho_Supervisions que va a ser Modificado.
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 22/Julio/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Modificar_Convenio_Fraccionamiento(Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio Convenio_Fraccionamiento)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Boolean Actualizar = false;

            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try
            {
                String Mi_SQL = "UPDATE " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + " SET ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Cuenta_Predial_ID + " = '" + Convenio_Fraccionamiento.P_Cuenta_Predial_ID + "', ";
                if (Convenio_Fraccionamiento.P_Propietario_ID != "" && Convenio_Fraccionamiento.P_Propietario_ID != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Propietario_ID + " = '" + Convenio_Fraccionamiento.P_Propietario_ID + "', ";
                }
                if (Convenio_Fraccionamiento.P_Realizo != "" && Convenio_Fraccionamiento.P_Realizo != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Realizo + " = '" + Convenio_Fraccionamiento.P_Realizo + "', ";
                }
                if (Convenio_Fraccionamiento.P_No_Reestructura != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_No_Reestructura + " = " + Convenio_Fraccionamiento.P_No_Reestructura + ", ";
                }
                if (Convenio_Fraccionamiento.P_Estatus != "" && Convenio_Fraccionamiento.P_Estatus != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus + " = '" + Convenio_Fraccionamiento.P_Estatus + "', ";
                }
                if (Convenio_Fraccionamiento.P_Solicitante != "" && Convenio_Fraccionamiento.P_Solicitante != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Solicitante + " = '" + Convenio_Fraccionamiento.P_Solicitante + "', ";
                }
                if (Convenio_Fraccionamiento.P_RFC != "" && Convenio_Fraccionamiento.P_RFC != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_RFC + " = '" + Convenio_Fraccionamiento.P_RFC + "', ";
                }
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Numero_Parcialidades + " = " + Convenio_Fraccionamiento.P_Numero_Parcialidades + ", ";
                if (Convenio_Fraccionamiento.P_Periodicidad_Pago != "" && Convenio_Fraccionamiento.P_Periodicidad_Pago != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Periodicidad_Pago + " = '" + Convenio_Fraccionamiento.P_Periodicidad_Pago + "', ";
                }
                if (Convenio_Fraccionamiento.P_Fecha.ToString() != "" && Convenio_Fraccionamiento.P_Fecha != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha + " = '" + Convenio_Fraccionamiento.P_Fecha.ToString("d-M-yyyy") + "', ";
                }
                if (Convenio_Fraccionamiento.P_Fecha_Vencimiento.ToString() != "" && Convenio_Fraccionamiento.P_Fecha_Vencimiento != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Derechos_Supervision.Campo_Fecha_Vencimiento + " = '" + Convenio_Fraccionamiento.P_Fecha_Vencimiento.ToString("d-M-yyyy") + "', ";
                }
                if (Convenio_Fraccionamiento.P_Observaciones != "" && Convenio_Fraccionamiento.P_Observaciones != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Observaciones + " = '" + Convenio_Fraccionamiento.P_Observaciones + "', ";
                }
                if (Convenio_Fraccionamiento.P_No_Descuento != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_No_Descuento + " = '" + Convenio_Fraccionamiento.P_No_Descuento + "', ";
                }
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Descuento_Recargos_Ordinarios + " = " + Convenio_Fraccionamiento.P_Descuento_Recargos_Ordinarios + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Descuento_Recargos_Moratorios + " = " + Convenio_Fraccionamiento.P_Descuento_Recargos_Moratorios + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Descuento_Multas + " = " + Convenio_Fraccionamiento.P_Descuento_Multas + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Adeudo + " = " + Convenio_Fraccionamiento.P_Total_Adeudo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Descuento + " = " + Convenio_Fraccionamiento.P_Total_Descuento + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Sub_Total + " = " + Convenio_Fraccionamiento.P_Sub_Total + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Porcentaje_Anticipo + " = " + Convenio_Fraccionamiento.P_Porcentaje_Anticipo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Anticipo + " = " + Convenio_Fraccionamiento.P_Total_Anticipo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Convenio + " = " + Convenio_Fraccionamiento.P_Total_Convenio + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Convenio_Fraccionamiento.P_Usuario + "', ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL += "WHERE " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + Convenio_Fraccionamiento.P_No_Convenio + "'";
                Mi_SQL += " AND " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Reestructura + " IS NULL";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //Elimina los Detalles de los Impuestos
                Mi_SQL = "DELETE FROM " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos;
                Mi_SQL += " WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + Convenio_Fraccionamiento.P_No_Convenio + "'";
                Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + " IS NULL";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //Inserta los Detalles de las Parcialidades de los Convenios
                if (Convenio_Fraccionamiento.P_Dt_Parcialidades != null
                    && Convenio_Fraccionamiento.P_Dt_Parcialidades.Rows.Count > 0)
                {
                    foreach (DataRow Dr_Detalle in Convenio_Fraccionamiento.P_Dt_Parcialidades.Rows)
                    {
                        Mi_SQL = "INSERT INTO " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + " (";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Usuario_Creo + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Creo + ") ";
                        Mi_SQL += "VALUES (";
                        Mi_SQL += "'" + Convenio_Fraccionamiento.P_No_Convenio + "', ";
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento] != null)
                        {
                            Mi_SQL += "'" + Convert.ToDateTime(Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento]).ToString("d-M-yyyy") + "', ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Convenio_Fraccionamiento.P_Estatus != "CANCELADO")
                        {
                            if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus] != null)
                            {
                                Mi_SQL += "'" + Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus].ToString() + "', ";
                            }
                            else
                            {
                                Mi_SQL += "NULL, ";
                            }
                        }
                        else
                        {
                            Mi_SQL += "'CANCELADO', ";
                        }
                        Mi_SQL += "'" + Convenio_Fraccionamiento.P_Usuario + "', SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                }

                Trans.Commit();
                Actualizar = true;
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
                    Mensaje = "Error al intentar modificar un Registro de Impuesto por Derecho de Supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
            return Actualizar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Reestructura_Convenio_Fraccionamiento
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo registro de Convenio_Fraccionamiento
        ///PARAMETROS           : 1. Convenio_Fraccionamiento.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Derecho_Supervisions_Negocio
        ///                                 con los datos de Derecho_Supervisions que va a ser dado de Alta.
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 10/Agosto/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Modificar_Reestructura_Convenio_Fraccionamiento(Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio Convenio_Fraccionamiento)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Boolean Actualizar = false;

            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try
            {
                String Mi_SQL = "UPDATE " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + " SET ";
                //Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Cuenta_Predial_ID + " = '" + Convenio_Fraccionamiento.P_Cuenta_Predial_ID + "', ";
                if (Convenio_Fraccionamiento.P_Propietario_ID != "" && Convenio_Fraccionamiento.P_Propietario_ID != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Propietario_ID + " = '" + Convenio_Fraccionamiento.P_Propietario_ID + "', ";
                }
                if (Convenio_Fraccionamiento.P_Realizo != "" && Convenio_Fraccionamiento.P_Realizo != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Realizo + " = '" + Convenio_Fraccionamiento.P_Realizo + "', ";
                }
                if (Convenio_Fraccionamiento.P_Estatus != "" && Convenio_Fraccionamiento.P_Estatus != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus + " = '" + Convenio_Fraccionamiento.P_Estatus + "', ";
                }
                if (Convenio_Fraccionamiento.P_Solicitante != "" && Convenio_Fraccionamiento.P_Solicitante != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Solicitante + " = '" + Convenio_Fraccionamiento.P_Solicitante + "', ";
                }
                if (Convenio_Fraccionamiento.P_No_Descuento != null && Convenio_Fraccionamiento.P_No_Descuento != "")
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_No_Descuento + " = '" + Convenio_Fraccionamiento.P_No_Descuento + "', ";
                }
                if (Convenio_Fraccionamiento.P_RFC != "" && Convenio_Fraccionamiento.P_RFC != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_RFC + " = '" + Convenio_Fraccionamiento.P_RFC + "', ";
                }
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Numero_Parcialidades + " = " + Convenio_Fraccionamiento.P_Numero_Parcialidades + ", ";
                if (Convenio_Fraccionamiento.P_Periodicidad_Pago != "" && Convenio_Fraccionamiento.P_Periodicidad_Pago != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Periodicidad_Pago + " = '" + Convenio_Fraccionamiento.P_Periodicidad_Pago + "', ";
                }
                if (Convenio_Fraccionamiento.P_Fecha.ToString() != "" && Convenio_Fraccionamiento.P_Fecha != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha + " = '" + Convenio_Fraccionamiento.P_Fecha.ToString("d-M-yyyy") + "', ";
                }
                if (Convenio_Fraccionamiento.P_Fecha_Vencimiento.ToString() != "" && Convenio_Fraccionamiento.P_Fecha_Vencimiento != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento + " = '" + Convenio_Fraccionamiento.P_Fecha_Vencimiento.ToString("d-M-yyyy") + "', ";
                }
                if (Convenio_Fraccionamiento.P_Observaciones != "" && Convenio_Fraccionamiento.P_Observaciones != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Observaciones + " = '" + Convenio_Fraccionamiento.P_Observaciones + "', ";
                }
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Descuento_Recargos_Ordinarios + " = " + Convenio_Fraccionamiento.P_Descuento_Recargos_Ordinarios + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Descuento_Recargos_Moratorios + " = " + Convenio_Fraccionamiento.P_Descuento_Recargos_Moratorios + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Descuento_Multas + " = " + Convenio_Fraccionamiento.P_Descuento_Multas + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Adeudo + " = " + Convenio_Fraccionamiento.P_Total_Adeudo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Descuento + " = " + Convenio_Fraccionamiento.P_Total_Descuento + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Sub_Total + " = " + Convenio_Fraccionamiento.P_Sub_Total + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Porcentaje_Anticipo + " = " + Convenio_Fraccionamiento.P_Porcentaje_Anticipo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Anticipo + " = " + Convenio_Fraccionamiento.P_Total_Anticipo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Convenio + " = " + Convenio_Fraccionamiento.P_Total_Convenio + ", ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Convenio_Fraccionamiento.P_Usuario + "', ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL += "WHERE " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + Convenio_Fraccionamiento.P_No_Convenio + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //Elimina los Detalles de los Impuestos
                Mi_SQL = "DELETE FROM " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos;
                Mi_SQL += " WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + Convenio_Fraccionamiento.P_No_Convenio + "' AND ";
                Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + "=(SELECT " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Reestructura + " FROM " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + " WHERE " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + "='" + Convenio_Fraccionamiento.P_No_Convenio + "')";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //Inserta los Detalles de las Parcialidades de los Convenios
                if (Convenio_Fraccionamiento.P_Dt_Parcialidades != null
                    && Convenio_Fraccionamiento.P_Dt_Parcialidades.Rows.Count > 0)
                {
                    foreach (DataRow Dr_Detalle in Convenio_Fraccionamiento.P_Dt_Parcialidades.Rows)
                    {
                        Mi_SQL = "INSERT INTO " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + " (";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Usuario_Creo + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Creo + ") ";
                        Mi_SQL += "VALUES (";
                        Mi_SQL += "'" + Convenio_Fraccionamiento.P_No_Convenio + "', ";
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento] != null)
                        {
                            Mi_SQL += "'" + Convert.ToDateTime(Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento]).ToString("d-M-yyyy") + "', ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Convenio_Fraccionamiento.P_Estatus != "CANCELADO")
                        {
                            if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus] != null)
                            {
                                Mi_SQL += "'" + Dr_Detalle[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus].ToString() + "', ";
                            }
                            else
                            {
                                Mi_SQL += "NULL, ";
                            }
                        }
                        else
                        {
                            Mi_SQL += "'CANCELADO', ";
                        }
                        Mi_SQL += "(SELECT " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Reestructura + " FROM " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + " WHERE " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + "='" + Convenio_Fraccionamiento.P_No_Convenio + "'), ";
                        Mi_SQL += "'" + Convenio_Fraccionamiento.P_Usuario + "', SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                }

                Trans.Commit();
                Actualizar = true;
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Impuesto por Derecho de Supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
            return Actualizar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Estatus_Convenio_Reestructura
        ///DESCRIPCIÓN          : Modifica el Estatus según lo indicado en la Capa de Negocios y lo aplica a los convenios Activos o Vigentes según los filtros
        ///PARAMETROS           : 1. Convenio_Fraccionamiento.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio
        ///                                 con los datos de Convenio_Fraccionamiento que van a ser Modificados.
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 20/Marzo/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Modificar_Estatus_Convenio_Reestructura(Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio Convenio_Fraccionamiento)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Modificado = false;

            if (Convenio_Fraccionamiento.P_Cmmd != null)
            {
                Cmmd = Convenio_Fraccionamiento.P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmmd.Connection = Trans.Connection;
                Cmmd.Transaction = Trans;
            }

            try
            {
                String Mi_SQL = "UPDATE " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos;
                Mi_SQL += " SET ";
                if (Convenio_Fraccionamiento.P_Campos_Dinamicos != null && Convenio_Fraccionamiento.P_Campos_Dinamicos != "")
                {
                    Mi_SQL += Convenio_Fraccionamiento.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus + " = '" + Convenio_Fraccionamiento.P_Estatus + "'";
                }
                Mi_SQL += " WHERE ";
                if (Convenio_Fraccionamiento.P_Filtros_Dinamicos != null && Convenio_Fraccionamiento.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL += Convenio_Fraccionamiento.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + Convenio_Fraccionamiento.P_No_Convenio + "'";
                }
                Cmmd.CommandText = Mi_SQL;
                Cmmd.ExecuteNonQuery();

                if (Convenio_Fraccionamiento.P_Cmmd == null)
                {
                    Trans.Commit();
                }
                Modificado = true;
            }
            catch (OracleException Ex)
            {
                if (Convenio_Fraccionamiento.P_Cmmd == null)
                {
                    Trans.Rollback();
                }
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
                if (Convenio_Fraccionamiento.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }
            return Modificado;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Convenio_Fraccionamiento
        ///DESCRIPCIÓN          : Obtiene todos las Convenio_Fraccionamiento que estan dadas de alta en la base de datos
        ///PARAMETROS           : 1. Convenio_Fraccionamiento.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Derecho_Supervisions_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 22/Julio/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Convenio_Fraccionamiento(Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio Convenio_Fraccionamiento)
        {
            DataTable Dt_Fraccionamiento = new DataTable();
            String Mi_SQL = "";
            try
            {
                if (Convenio_Fraccionamiento.P_Mostrar_Ultimo_Convenio)
                {
                    Mi_SQL += "SELECT * FROM (";
                }
                Mi_SQL += "SELECT ";
                if (Convenio_Fraccionamiento.P_Campos_Foraneos)
                {
                    Mi_SQL += "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "." + Ope_Pre_Convenios_Fraccionamientos.Campo_Cuenta_Predial_ID + ") AS Cuenta_Predial, ";
                    Mi_SQL += "(SELECT " + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || " + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || " + Cat_Empleados.Campo_Nombre + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_Empleado_ID + " = " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "." + Ope_Pre_Convenios_Fraccionamientos.Campo_Realizo + ") AS Nombre_Realizo, ";
                    //Mi_SQL += "(SELECT " + Cat_Pre_Contribuyentes.Campo_Nombre + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " IN(SELECT " + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " WHERE " + Cat_Pre_Propietarios.Campo_Propietario_ID + " = " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "." + Ope_Pre_Convenios_Fraccionamientos.Campo_Propietario_ID + ")) AS Nombre_Propietario, ";
                    Mi_SQL += "(SELECT " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " ||' '||" + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + "||' '||" + Cat_Pre_Contribuyentes.Campo_Nombre + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " WHERE " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "." + Ope_Pre_Convenios_Fraccionamientos.Campo_Propietario_ID + "=" + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + ") AS Nombre_Propietario,";
                }
                if (Convenio_Fraccionamiento.P_Campos_Dinamicos != null && Convenio_Fraccionamiento.P_Campos_Dinamicos != "")
                {
                    Mi_SQL += Convenio_Fraccionamiento.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Cuenta_Predial_ID + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Propietario_ID + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Realizo + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_No_Reestructura + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Solicitante + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_RFC + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Numero_Parcialidades + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Periodicidad_Pago + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Observaciones + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Descuento_Recargos_Ordinarios + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Descuento_Recargos_Moratorios + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Descuento_Multas + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Adeudo + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Descuento + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Sub_Total + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Porcentaje_Anticipo + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha_Creo + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Anticipo + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_No_Descuento + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Anticipo + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Anticipo_Reestructura + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Convenio;
                }
                Mi_SQL += " FROM " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos;
                if (Convenio_Fraccionamiento.P_Filtros_Dinamicos != null && Convenio_Fraccionamiento.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL += " WHERE " + Convenio_Fraccionamiento.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL += " WHERE ";
                    if (Convenio_Fraccionamiento.P_No_Convenio != "" && Convenio_Fraccionamiento.P_No_Convenio != null)
                    {
                        Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + Convenio_Fraccionamiento.P_No_Convenio + "' AND ";
                    }
                    if (Convenio_Fraccionamiento.P_Cuenta_Predial_ID != "" && Convenio_Fraccionamiento.P_Cuenta_Predial_ID != null)
                    {
                        Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Cuenta_Predial_ID + " = '" + Convenio_Fraccionamiento.P_Cuenta_Predial_ID + "' AND ";
                    }
                    if (Convenio_Fraccionamiento.P_Propietario_ID != "" && Convenio_Fraccionamiento.P_Propietario_ID != null)
                    {
                        Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Propietario_ID + " = '" + Convenio_Fraccionamiento.P_Propietario_ID + "' AND ";
                    }
                    if (Convenio_Fraccionamiento.P_Realizo != "" && Convenio_Fraccionamiento.P_Realizo != null)
                    {
                        Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Realizo + " = '" + Convenio_Fraccionamiento.P_Realizo + "' AND ";
                    }
                    if (Convenio_Fraccionamiento.P_No_Reestructura != null)
                    {
                        Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_No_Reestructura + " = " + Convenio_Fraccionamiento.P_No_Reestructura + " AND ";
                    }
                    if (Convenio_Fraccionamiento.P_Estatus != "" && Convenio_Fraccionamiento.P_Estatus != null)
                    {
                        Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus + " = '" + Convenio_Fraccionamiento.P_Estatus + "' AND ";
                    }
                    if (Convenio_Fraccionamiento.P_Solicitante != "" && Convenio_Fraccionamiento.P_Solicitante != null)
                    {
                        Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Solicitante + " LIKE '%" + Convenio_Fraccionamiento.P_Solicitante + "%' AND ";
                    }
                    if (Convenio_Fraccionamiento.P_RFC != "" && Convenio_Fraccionamiento.P_RFC != null)
                    {
                        Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_RFC + " = '" + Convenio_Fraccionamiento.P_RFC + "' AND ";
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
                if (Convenio_Fraccionamiento.P_Agrupar_Dinamico != null && Convenio_Fraccionamiento.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " GROUP BY " + Convenio_Fraccionamiento.P_Agrupar_Dinamico;
                }
                if (Convenio_Fraccionamiento.P_Ordenar_Dinamico != null && Convenio_Fraccionamiento.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Convenio_Fraccionamiento.P_Ordenar_Dinamico;
                }
                else
                {
                    Mi_SQL += " ORDER BY " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " DESC";
                }
                if (Convenio_Fraccionamiento.P_Mostrar_Ultimo_Convenio)
                {
                    Mi_SQL += ") WHERE ROWNUM = 1";
                }

                DataSet Ds_Fraccionamiento = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Fraccionamiento != null)
                {
                    Dt_Fraccionamiento = Ds_Fraccionamiento.Tables[0];

                    if (Dt_Fraccionamiento.Rows.Count > 0)
                    {
                        //Consulta las Parcialidades del Convenio
                        Mi_SQL = "SELECT ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto + ", ";
                        Mi_SQL += "(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas + " + " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios + " + " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios + " + " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto + ") MONTO_IMPORTE, ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento + ", ";
                        Mi_SQL += "NULL PERIODO, ";
                        Mi_SQL += "0.00 HONORARIOS, ";
                        Mi_SQL += "(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios + "+" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto + ") AS TOTAL, ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus;
                        Mi_SQL += " FROM ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos;
                        Mi_SQL += " WHERE ";
                        if (Convenio_Fraccionamiento.P_No_Convenio != null && Convenio_Fraccionamiento.P_No_Convenio != "")
                        {
                            Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + Convenio_Fraccionamiento.P_No_Convenio + "' AND ";
                            if (Convenio_Fraccionamiento.P_Mostrar_Detalles_Con_Reestructura)
                            {
                                Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + "=(SELECT MAX(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + ") FROM " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + " WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + "='" + Convenio_Fraccionamiento.P_No_Convenio + "') AND ";
                            }
                            else
                            {
                                Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + " IS NULL AND ";
                            }
                        }
                        else
                        {
                            if (Convenio_Fraccionamiento.P_Mostrar_Detalles_Con_Reestructura)
                            {
                                Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + "=(SELECT MAX(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + ") FROM " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + " WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + "='" + Ds_Fraccionamiento.Tables[0].Rows[0]["NO_CONVENIO"].ToString() + "') AND ";
                            }
                            else
                            {
                                Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + " IS NULL AND ";
                            }
                        }
                        if (Convenio_Fraccionamiento.P_Es_Reestructura)
                        {
                            Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + Ds_Fraccionamiento.Tables[0].Rows[0][Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio] + "' AND " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + " IN ('POR PAGAR','CANCELADO','INCUMPLIDO') AND ";
                        }
                        else
                        {
                            Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + Ds_Fraccionamiento.Tables[0].Rows[0][Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio] + "' AND (" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + " IN ('POR PAGAR','CANCELADO','INCUMPLIDO','PAGADO')) AND ";
                        }
                        if (Convenio_Fraccionamiento.P_Cuenta_Predial_ID != null && Convenio_Fraccionamiento.P_Cuenta_Predial_ID != "")
                        {
                            Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " IN (SELECT " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " FROM " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + " WHERE " + Ope_Pre_Convenios_Fraccionamientos.Campo_Cuenta_Predial_ID + " = '" + Convenio_Fraccionamiento.P_Cuenta_Predial_ID + "') AND ";
                        }
                        if (Mi_SQL.EndsWith(" AND "))
                        {
                            Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                        }
                        if (Mi_SQL.EndsWith(" WHERE "))
                        {
                            Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                        }
                        Mi_SQL += " ORDER BY " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago;
                        Convenio_Fraccionamiento.P_Dt_Parcialidades = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    }
                    if (Dt_Fraccionamiento.Rows.Count > 0)
                    {
                        //Consulta las Parcialidades del Convenio
                        Mi_SQL = "SELECT ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto + ", ";
                        Mi_SQL += "(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas + " + " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios + " + " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios + " + " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto + ") MONTO_IMPORTE, ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento + ", ";
                        Mi_SQL += "NULL PERIODO, ";
                        Mi_SQL += "0.00 HONORARIOS, ";
                        Mi_SQL += "(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios + "+" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto + ") AS TOTAL, ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus;
                        Mi_SQL += " FROM ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos;
                        Mi_SQL += " WHERE ";
                        if (Convenio_Fraccionamiento.P_No_Convenio != null && Convenio_Fraccionamiento.P_No_Convenio != "")
                        {
                            Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + Convenio_Fraccionamiento.P_No_Convenio + "' AND ";
                            if (Convenio_Fraccionamiento.P_Mostrar_Detalles_Con_Reestructura)
                            {
                                Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + "=(SELECT MAX(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + ") FROM " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + " WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + "='" + Convenio_Fraccionamiento.P_No_Convenio + "') AND ";
                            }
                            else 
                            {
                                Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + " IS NULL AND ";
                            }
                        }
                        else
                        {
                            if (Convenio_Fraccionamiento.P_Mostrar_Detalles_Con_Reestructura)
                            {
                                Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + "=(SELECT MAX(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + ") FROM " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + " WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + "='" + Ds_Fraccionamiento.Tables[0].Rows[0]["NO_CONVENIO"].ToString() + "') AND ";
                            }
                            else
                            {
                                Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + " IS NULL AND ";
                            }
                        }
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + Ds_Fraccionamiento.Tables[0].Rows[0][Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio] + "' AND " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + "='PAGADO' AND ";
                        if (Convenio_Fraccionamiento.P_Cuenta_Predial_ID != null && Convenio_Fraccionamiento.P_Cuenta_Predial_ID != "")
                        {
                            Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " IN (SELECT " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " FROM " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + " WHERE " + Ope_Pre_Convenios_Fraccionamientos.Campo_Cuenta_Predial_ID + " = '" + Convenio_Fraccionamiento.P_Cuenta_Predial_ID + "') AND ";
                        }
                        if (Mi_SQL.EndsWith(" AND "))
                        {
                            Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                        }
                        if (Mi_SQL.EndsWith(" WHERE "))
                        {
                            Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                        }
                        Mi_SQL += " ORDER BY " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago;
                        Convenio_Fraccionamiento.P_Dt_Parcialidades_Pagadas = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    }
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Impuestos por Derechos de Supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Fraccionamiento;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : Antonio Salvador Benavides Guardado
        ///FECHA_MODIFICO       : 26/Octubre/2010
        ///CAUSA_MODIFICACIÓN   : Estandarizar variables usadas
        ///*******************************************************************************
        public static String Obtener_ID_Consecutivo(String Tabla, String Campo, String Filtro, Int32 Longitud_ID)
        {
            String Id = Convertir_A_Formato_ID(1, Longitud_ID); ;
            try
            {
                String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla;
                if (Filtro != "" && Filtro != null)
                {
                    Mi_SQL += " WHERE " + Filtro;
                }
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
        ///PARAMETROS:     
        ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
        ///             2. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : Antonio Salvador Benavides Guardado
        ///FECHA_MODIFICO       : 26/Octubre/2010
        ///CAUSA_MODIFICACIÓN   : Estandarizar variables usadas
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
        ///NOMBRE DE LA FUNCIÓN : Consultar_Adeudos_Fraccionamientos
        ///DESCRIPCIÓN          : Obtiene todos los Adeudos para realizar la reestructura de estos.
        ///PARAMETROS           : 1. Convenio_Derecho_Supervision.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Derecho_Supervisions_Negocio
        ///CREO                 : Miguel Angel Bedolla Moreno
        ///FECHA_CREO           : 13/Septiembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Adeudos_Fraccionamientos(Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio Convenio_Fraccionamiento, Boolean Total_O_A_Pagar)
        {
            DataTable Dt_Derecho_Supervision = new DataTable();
            String Mi_SQL = "";
            try
            {
                //Consulta las Parcialidades del Convenio
                Mi_SQL = "SELECT ";
                //Mi_SQL += Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + ", ";
                //Mi_SQL += Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago + ", ";
                Mi_SQL += "0.00 HONORARIOS, ";
                Mi_SQL += "SUM(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas + ") AS " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas + ", ";
                Mi_SQL += "SUM(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios + ") AS " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios + ", ";
                Mi_SQL += "SUM(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios + ") AS " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios + ", ";
                Mi_SQL += "SUM(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto + ") AS " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto + ", ";
                Mi_SQL += "SUM(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas + " + " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios + " + " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Recargos_Moratorios + " + " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Monto_Impuesto + ") MONTO_IMPORTE ";
                Mi_SQL += " FROM ";
                Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos;
                Mi_SQL += " WHERE ";
                Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + Convenio_Fraccionamiento.P_No_Convenio + "' AND ";
                if (Total_O_A_Pagar)
                {
                    Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + "='INCUMPLIDO' AND ";
                }
                if (Convenio_Fraccionamiento.P_Mostrar_Detalles_Con_Reestructura)
                {
                    Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + "=(SELECT MAX(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + ") FROM " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + " WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + "='" + Convenio_Fraccionamiento.P_No_Convenio + "') ";
                }
                else
                {
                    Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + " IS NULL";
                }
                Convenio_Fraccionamiento.P_Dt_Parcialidades = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los detalles de convenios por Derechos de Supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Derecho_Supervision;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Descuentos
        ///DESCRIPCIÓN          : Obtiene todos los Adeudos para realizar la reestructura de estos.
        ///PARAMETROS           : 1. Convenio_Derecho_Supervision.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Derecho_Supervisions_Negocio
        ///CREO                 : Miguel Angel Bedolla Moreno
        ///FECHA_CREO           : 13/Septiembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Descuentos(Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio Convenio_Fraccionamiento)
        {
            DataTable Dt_Derecho_Supervision = new DataTable();
            String Mi_SQL = "";
            try
            {
                //Consulta las Parcialidades del Convenio
                Mi_SQL = "SELECT NVL((" + Ope_Pre_Descuento_Fracc.Campo_Desc_Multa + "),0) AS " + Ope_Pre_Descuento_Fracc.Campo_Desc_Multa + ", NVL((" + Ope_Pre_Descuento_Fracc.Campo_Desc_Recargo + "),0) AS " + Ope_Pre_Descuento_Fracc.Campo_Desc_Recargo + ", " + Ope_Pre_Descuento_Fracc.Campo_No_Descuento + " FROM " + Ope_Pre_Descuento_Fracc.Tabla_Ope_Pre_Descuento_Fracc;
                Mi_SQL += " WHERE " + Ope_Pre_Descuento_Fracc.Campo_No_Impuesto_fraccionamiento + " = '" + Convenio_Fraccionamiento.P_No_Impuesto_Fraccionamiento + "' AND ";
                Mi_SQL += Ope_Pre_Descuento_Fracc.Campo_Estatus + "='VIGENTE'";

                Dt_Derecho_Supervision = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los detalles de convenios por Fraccionamiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Derecho_Supervision;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cancelar_Pasivo
        ///DESCRIPCIÓN          : Modifica en la Base de Datos el registro indicado por la referencia
        ///PARAMETROS          : 1. Referencia.   Referencia con la cual se modificara el registro a cancelado
        ///CREO                 : Miguel Angel Bedolla Moreno
        ///FECHA_CREO           : 28/Septiembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Eliminar_Pasivo(String Referencia)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Boolean Actualizar = false;

            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try
            {
                String Mi_SQL = "DELETE FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " ";
                Mi_SQL += "WHERE " + Ope_Ing_Pasivo.Campo_Referencia + " = '" + Referencia + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                Trans.Commit();
                Actualizar = true;
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
                    Mensaje = "Error al intentar ELIMINAR el pasivo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
            return Actualizar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Impuesto_Para_Eliminar_Pasivo
        ///DESCRIPCIÓN          : Consulta los datos del impuesto para después eliminar el pasivo
        ///PARAMETROS          : 1. Convenio.   Instancia que contiene los datos necesarios para consultar los datos del impuesto.
        ///CREO                 : Miguel Angel Bedolla Moreno
        ///FECHA_CREO           : 01/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Impuesto_Para_Eliminar_Pasivo(Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio Convenio)
        {
            DataTable Dt_fracc = new DataTable();
            String Mi_SQL = "";
            try
            {
                //Consulta las Parcialidades del Convenio
                Mi_SQL = "SELECT " + Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Creo + " FROM " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos;
                Mi_SQL += " WHERE " + Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + "='" + Convenio.P_No_Impuesto_Fraccionamiento + "'";

                Dt_fracc = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los datos del fraccionamiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_fracc;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Convenio_Incumplido
        ///DESCRIPCIÓN: Cambia el estatus a incumplido una vez pasada la fecha de vencimiento.
        ///PARAMENTROS:     
        ///             Convenio_Fracc: Contiene el no_reestructura y el no_convenio para hacer referencia a los datos de la bd que se modificaran.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 06/Diciembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Convenio_Incumplido(Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio Convenio_Fracc)
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
            Boolean Actualizado = false;
            try
            {
                String Mi_SQL = "";
                Mi_SQL += "UPDATE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + " SET ";
                Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + "='INCUMPLIDO' ";
                Mi_SQL += "WHERE ";
                Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + "='" + Convenio_Fracc.P_No_Convenio + "' AND ";
                Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + "='POR PAGAR' AND ";
                if (Convenio_Fracc.P_No_Reestructura != null && Convenio_Fracc.P_No_Reestructura != "")
                {
                    Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + "=" + Convenio_Fracc.P_No_Reestructura;
                }
                else
                {
                    Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + " IS NULL";
                }
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                Mi_SQL = "";
                Mi_SQL += "UPDATE " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + " SET ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus + "='INCUMPLIDO' ";
                Mi_SQL += "WHERE ";
                Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + "='" + Convenio_Fracc.P_No_Convenio + "'";

                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
                Actualizado = true;
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
                    Mensaje = "Error al intentar modificar un Registro de Convenios de Derechos de supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
            return Actualizado;
        }

        #endregion

    }

    
}