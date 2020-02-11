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
using Presidencia.Operacion_Predial_Convenios_Predial.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;



namespace Presidencia.Operacion_Predial_Convenios_Predial.Datos
{

    public class Cls_Ope_Pre_Convenios_Predial_Datos
    {
        #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Convenio_Predial
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo registro de Convenio_Perdial
        ///PARAMETROS           : 1. Datos.   Instancia de la Clase de Negocio 
        ///                                 con los datos de Convenio predial que va a ser dado de Alta.
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 22/Julio/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Alta_Convenio_Predial(Cls_Ope_Pre_Convenios_Predial_Negocio Datos)
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
            String No_Convenio = Obtener_ID_Consecutivo(Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial, Ope_Pre_Convenios_Predial.Campo_No_Convenio, "", 10);
            Datos.P_No_Convenio = No_Convenio;
            try
            {
                String Mi_SQL = "INSERT INTO " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + " (";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_No_Convenio + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Contribuyente_Id + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Realizo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Hasta_Periodo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Estatus + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Solicitante + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_RFC + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Numero_Parcialidades + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Periodicidad_Pago + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Fecha + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Observaciones + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Ordinarios + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Moratorios + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Descuento_Multas + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_No_Descuento + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Adeudo_Corriente + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Adeudo_Rezago + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Predial + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Recargos + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Moratorios + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Honorarios + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Adeudo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Descuento + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Sub_Total + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Porcentaje_Anticipo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Anticipo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Convenio + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Anticipo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Parcialidades_Manual + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Usuario_Creo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Fecha_Creo + ") ";
                Mi_SQL += "VALUES ('";
                Mi_SQL += No_Convenio + "', ";
                if (Datos.P_Cuenta_Predial_ID != "" && Datos.P_Cuenta_Predial_ID != null)
                {
                    Mi_SQL += "'" + Datos.P_Cuenta_Predial_ID + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Datos.P_Propietario_ID != "" && Datos.P_Propietario_ID != null)
                {
                    Mi_SQL += "'" + Datos.P_Propietario_ID + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Datos.P_Realizo != "" && Datos.P_Realizo != null)
                {
                    Mi_SQL += "'" + Datos.P_Realizo + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Datos.P_No_Reestructura != null)
                {
                    Mi_SQL += "'" + Datos.P_No_Reestructura + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Datos.P_Hasta_Periodo != null)
                {
                    Mi_SQL += "'" + Datos.P_Hasta_Periodo + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Datos.P_Estatus != "" && Datos.P_Estatus != null)
                {
                    Mi_SQL += "'" + Datos.P_Estatus + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Datos.P_Solicitante != "" && Datos.P_Solicitante != null)
                {
                    Mi_SQL += "'" + Datos.P_Solicitante + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Datos.P_RFC != "" && Datos.P_RFC != null)
                {
                    Mi_SQL += "'" + Datos.P_RFC + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                Mi_SQL += Datos.P_Numero_Parcialidades + ", ";
                if (Datos.P_Periodicidad_Pago != "" && Datos.P_Periodicidad_Pago != null)
                {
                    Mi_SQL += "'" + Datos.P_Periodicidad_Pago + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Datos.P_Fecha.ToString() != "" && Datos.P_Fecha != null)
                {
                    Mi_SQL += "'" + Datos.P_Fecha.ToString("d-M-yyyy") + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Datos.P_Observaciones != "" && Datos.P_Observaciones != null)
                {
                    Mi_SQL += "'" + Datos.P_Observaciones + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                Mi_SQL += Datos.P_Descuento_Recargos_Ordinarios + ", "
                    + Datos.P_Descuento_Recargos_Moratorios + ", "
                    + Datos.P_Descuento_Multas + ", '"
                    + Datos.P_No_Descuento + "', "
                    + Datos.P_Adeudo_Corriente + ", "
                    + Datos.P_Adeudo_Rezago + ", "
                    + Datos.P_Total_Predial + ", "
                    + Datos.P_Total_Recargos + ", "
                    + Datos.P_Total_Moratorios + ", "
                    + Datos.P_Total_Honorarios + ", "
                    + Datos.P_Total_Adeudo + ", "
                    + Datos.P_Total_Descuento + ", "
                    + Datos.P_Sub_Total + ", "
                    + Datos.P_Porcentaje_Anticipo + ", "
                    + Datos.P_Total_Anticipo + ", "
                    + Datos.P_Total_Convenio + ", 'POR PAGAR', '"
                    + Datos.P_Parcialidades_Manual + "', '"
                    + Datos.P_Usuario + "', SYSDATE)";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //Inserta los Detalles de las Parcialidades de los Convenios
                if (Datos.P_Dt_Parcialidades != null)
                {
                    foreach (DataRow Dr_Detalle in Datos.P_Dt_Parcialidades.Rows)
                    {
                        Mi_SQL = "INSERT INTO " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + " ("
                            + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + ", "
                            + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + ", "
                            + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ", "
                            + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios + ", "
                            + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios + ", "
                            + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios + ", "
                            + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto + ", "
                            + Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento + ", "
                            + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + ", "
                            + Ope_Pre_Detalles_Convenios_Predial.Campo_Usuario_Creo + ", "
                            + Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Creo + ") "
                            + "VALUES ("
                            + "'" + No_Convenio + "', ";
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (!String.IsNullOrEmpty(Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo].ToString()))
                        {
                            Mi_SQL += "'" + Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo].ToString() + "', ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle["MONTO_HONORARIOS"] != null)
                        {
                            Mi_SQL += Dr_Detalle["MONTO_HONORARIOS"].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento] != null)
                        {
                            Mi_SQL += "'" + Convert.ToDateTime(Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento]).ToString("d-M-yyyy") + "', ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus] != null)
                        {
                            Mi_SQL += "'" + Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus].ToString() + "', ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        Mi_SQL += "'" + Datos.P_Usuario + "', SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                }

                //Inserta el desglose de las parcialidades del convenio
                if (Datos.P_Dt_Desglose_Parcialidades != null)
                {
                    foreach (DataRow Dr_Desglose in Datos.P_Dt_Desglose_Parcialidades.Rows)
                    {
                        Mi_SQL = "INSERT INTO " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + " ("
                            + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_No_Convenio + ", "
                            + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_No_Pago + ", "
                            + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Anio + ", "
                            + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_1 + ", "
                            + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_2 + ", "
                            + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_3 + ", "
                            + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_4 + ", "
                            + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_5 + ", "
                            + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_6 + ", "
                            + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Usuario_Creo + ", "
                            + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Fecha_Creo + ") "
                            + "VALUES ("
                            + "'" + Datos.P_No_Convenio + "', '"
                            + Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_No_Pago].ToString() + "', '"
                            + Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Anio].ToString() + "', ";
                        if (Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_1].ToString() != "")
                        {
                            Mi_SQL += Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_1].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "0, ";
                        }
                        if (Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_2].ToString() != "")
                        {
                            Mi_SQL += Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_2].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "0, ";
                        }
                        if (Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_3].ToString() != "")
                        {
                            Mi_SQL += Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_3].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "0, ";
                        }
                        if (Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_4].ToString() != "")
                        {
                            Mi_SQL += Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_4].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "0, ";
                        }
                        if (Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_5].ToString() != "")
                        {
                            Mi_SQL += Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_5].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "0, ";
                        }
                        if (Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_6].ToString() != "")
                        {
                            Mi_SQL += Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_6].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "0, ";
                        }
                        Mi_SQL += "'" + Datos.P_Usuario + "', SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                }

                // aplicar insercion
                Trans.Commit();
                // regresar numero de convenio insertado
                Datos.P_No_Convenio = No_Convenio;
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Impuesto Predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN : Alta_Reestructura_Convenio_Predial
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo registro de Convenio_Predial
        ///PARAMETROS           : 1. Datos.   Instancia de la Clase de Negocio de 
        ///                                 con los datos del convenio que va a ser dado de Alta.
        ///CREO                 : Roberto Gonzalez Oseguera
        ///FECHA_CREO           : 31-ago-2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Alta_Reestructura_Convenio_Predial(Cls_Ope_Pre_Convenios_Predial_Negocio Datos)
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
            String No_Reestructura = Obtener_ID_Consecutivo(
                Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial,
                Ope_Pre_Convenios_Predial.Campo_No_Reestructura,
                Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = '" +
                Datos.P_No_Convenio + "'", 10);

            No_Reestructura = Convert.ToInt32(No_Reestructura).ToString();
            try
            {
                String Mi_SQL = "UPDATE " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + " SET ";
                //Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "', ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_No_Reestructura + " = '" + No_Reestructura + "', ";
                if (Datos.P_Propietario_ID != "" && Datos.P_Propietario_ID != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Contribuyente_Id + " = '" + Datos.P_Propietario_ID + "', ";
                }
                if (Datos.P_Realizo != "" && Datos.P_Realizo != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Realizo + " = '" + Datos.P_Realizo + "', ";
                }
                if (Datos.P_Estatus != "" && Datos.P_Estatus != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                }
                if (Datos.P_Solicitante != "" && Datos.P_Solicitante != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Solicitante + " = '" + Datos.P_Solicitante + "', ";
                }
                if (Datos.P_RFC != "" && Datos.P_RFC != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_RFC + " = '" + Datos.P_RFC + "', ";
                }
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Numero_Parcialidades + " = " + Datos.P_Numero_Parcialidades + ", ";
                if (Datos.P_Periodicidad_Pago != "" && Datos.P_Periodicidad_Pago != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Periodicidad_Pago + " = '" + Datos.P_Periodicidad_Pago + "', ";
                }
                if (Datos.P_Fecha.ToString() != "" && Datos.P_Fecha != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Fecha + " = '" + Datos.P_Fecha.ToString("d-M-yyyy") + "', ";
                }
                if (Datos.P_Fecha_Vencimiento.ToString() != "" && Datos.P_Fecha_Vencimiento != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Fecha_Vencimiento + " = '" + Datos.P_Fecha_Vencimiento.ToString("d-M-yyyy") + "', ";
                }
                if (Datos.P_Observaciones != "" && Datos.P_Observaciones != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Observaciones + " = '" + Datos.P_Observaciones + "', ";
                }
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Ordinarios + " = " + Datos.P_Descuento_Recargos_Ordinarios + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Moratorios + " = " + Datos.P_Descuento_Recargos_Moratorios + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Descuento_Multas + " = " + Datos.P_Descuento_Multas + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_No_Descuento + " = '" + Datos.P_No_Descuento + "', ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Adeudo_Corriente + " = " + Datos.P_Adeudo_Corriente + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Adeudo_Rezago + " = " + Datos.P_Adeudo_Rezago + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Recargos + " = " + Datos.P_Total_Recargos + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Moratorios + " = " + Datos.P_Total_Moratorios + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Honorarios + " = " + Datos.P_Total_Honorarios + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Adeudo + " = " + Datos.P_Total_Adeudo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Descuento + " = " + Datos.P_Total_Descuento + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Sub_Total + " = " + Datos.P_Sub_Total + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Porcentaje_Anticipo + " = " + Datos.P_Porcentaje_Anticipo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Anticipo + " = " + Datos.P_Total_Anticipo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Convenio + " = " + Datos.P_Total_Convenio + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Anticipo_Reestructura + " = 'POR PAGAR', ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Parcialidades_Manual + " = '" + Datos.P_Parcialidades_Manual + "', ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario + "', ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL += "WHERE " + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = '" + Datos.P_No_Convenio + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                // poner estatus cancelado a las parcialidades de la reestructura anterior los Detalles de los Impuestos
                Mi_SQL = "UPDATE " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial
                    + " SET " + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'CANCELADO'"
                    + " WHERE " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " = '" + Datos.P_No_Convenio + "'"
                    + " AND TRIM(" + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + ") IN ('POR PAGAR', 'INCUMPLIDO')"
                    + " AND NVL(" + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ", 0) = "
                    + "( SELECT NVL(MAX(" + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + "), 0)"
                    + " FROM " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial
                    + " WHERE " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " = '" + Datos.P_No_Convenio + "'"
                    + ")";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //Inserta los Detalles de las Parcialidades de los Convenios
                if (Datos.P_Dt_Parcialidades != null)
                {
                    foreach (DataRow Dr_Detalle in Datos.P_Dt_Parcialidades.Rows)
                    {
                        Mi_SQL = "INSERT INTO " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + " (";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Usuario_Creo + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Creo + ") ";
                        Mi_SQL += "VALUES (";
                        Mi_SQL += "'" + Datos.P_No_Convenio + "', ";
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (!string.IsNullOrEmpty(Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo].ToString().Trim()))
                        {
                            Mi_SQL += "'" + Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo].ToString() + "', ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento] != null)
                        {
                            Mi_SQL += "'" + Convert.ToDateTime(Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento]).ToString("d-M-yyyy") + "', ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus] != null)
                        {
                            Mi_SQL += "'" + Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus].ToString() + "', ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        Mi_SQL += "'" + No_Reestructura + "',";
                        Mi_SQL += "'" + Datos.P_Usuario + "', SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                }

                // eliminar el desglose de las aplicaciones
                Mi_SQL = "DELETE FROM " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial
                    + " WHERE " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_No_Convenio + " = '" + Datos.P_No_Convenio + "'"
                    + " AND NVL(" + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_No_Reestructura + ", 0) = "
                    + "( SELECT NVL(MAX(" + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_No_Reestructura + "), 0)"
                    + " FROM " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial
                    + " WHERE " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_No_Convenio + " = '" + Datos.P_No_Convenio + "'"
                    + ")";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //Inserta el desglose de las parcialidades del convenio
                if (Datos.P_Dt_Desglose_Parcialidades != null)
                {
                    foreach (DataRow Dr_Desglose in Datos.P_Dt_Desglose_Parcialidades.Rows)
                    {
                        Mi_SQL = "INSERT INTO " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + " (";
                        Mi_SQL += Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_No_Convenio + ", ";
                        Mi_SQL += Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_No_Pago + ", ";
                        Mi_SQL += Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Anio + ", ";
                        Mi_SQL += Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_1 + ", ";
                        Mi_SQL += Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_2 + ", ";
                        Mi_SQL += Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_3 + ", ";
                        Mi_SQL += Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_4 + ", ";
                        Mi_SQL += Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_5 + ", ";
                        Mi_SQL += Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_6 + ", ";
                        Mi_SQL += Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_No_Reestructura + ", ";
                        Mi_SQL += Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Usuario_Creo + ", ";
                        Mi_SQL += Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Fecha_Creo + ") ";
                        Mi_SQL += "VALUES (";
                        Mi_SQL += "'" + Datos.P_No_Convenio + "', '";
                        Mi_SQL += Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_No_Pago].ToString() + "', '";
                        Mi_SQL += Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Anio].ToString() + "', ";
                        if (Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_1].ToString() != "")
                        {
                            Mi_SQL += Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_1].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "0, ";
                        }
                        if (Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_2].ToString() != "")
                        {
                            Mi_SQL += Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_2].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "0, ";
                        }
                        if (Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_3].ToString() != "")
                        {
                            Mi_SQL += Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_3].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "0, ";
                        }
                        if (Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_4].ToString() != "")
                        {
                            Mi_SQL += Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_4].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "0, ";
                        }
                        if (Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_5].ToString() != "")
                        {
                            Mi_SQL += Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_5].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "0, ";
                        }
                        if (Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_6].ToString() != "")
                        {
                            Mi_SQL += Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_6].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "0, ";
                        }
                        Mi_SQL += No_Reestructura + ", '" + Datos.P_Usuario + "', SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Convenio predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN : Modificar_Convenio_Predial
        ///DESCRIPCIÓN          : Modifica en la Base de Datos el registro indicado del Convenio predial
        ///PARAMETROS          : 1. Datos. Instancia de la Clase de Negocio de Cls_Ope_Pre_Convenio_Predial_Negocio
        ///                         con los datos del Convenio que va a ser Modificado.
        ///CREO                 : Roberto González Oseguera
        ///FECHA_CREO           : 02-sep-2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Modificar_Convenio_Predial(Cls_Ope_Pre_Convenios_Predial_Negocio Datos)
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
                String Mi_SQL = "UPDATE " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + " SET ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id + " = '" + Datos.P_Cuenta_Predial_ID + "', ";
                if (Datos.P_Propietario_ID != "" && Datos.P_Propietario_ID != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Contribuyente_Id + " = '" + Datos.P_Propietario_ID + "', ";
                }
                if (Datos.P_Realizo != "" && Datos.P_Realizo != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Realizo + " = '" + Datos.P_Realizo + "', ";
                }
                if (!String.IsNullOrEmpty(Datos.P_Hasta_Periodo))
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Hasta_Periodo + " = '" + Datos.P_Hasta_Periodo + "', ";
                }
                if (Datos.P_No_Reestructura != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_No_Reestructura + " = '" + Datos.P_No_Reestructura + "', ";
                }
                if (Datos.P_Estatus != "" && Datos.P_Estatus != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                }
                if (Datos.P_Solicitante != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Solicitante + " = '" + Datos.P_Solicitante + "', ";
                }
                if (Datos.P_RFC != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_RFC + " = '" + Datos.P_RFC + "', ";
                }
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Numero_Parcialidades + " = " + Datos.P_Numero_Parcialidades + ", ";
                if (Datos.P_Periodicidad_Pago != "" && Datos.P_Periodicidad_Pago != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Periodicidad_Pago + " = '" + Datos.P_Periodicidad_Pago + "', ";
                }
                if (Datos.P_Fecha.ToString() != "" && Datos.P_Fecha != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Fecha + " = '" + Datos.P_Fecha.ToString("d-M-yyyy") + "', ";
                }
                if (Datos.P_Observaciones != "" && Datos.P_Observaciones != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Observaciones + " = '" + Datos.P_Observaciones + "', ";
                }
                if (Datos.P_No_Descuento != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_No_Descuento + " = '" + Datos.P_No_Descuento + "', ";
                }
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Ordinarios + " = " + Datos.P_Descuento_Recargos_Ordinarios + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Moratorios + " = " + Datos.P_Descuento_Recargos_Moratorios + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Descuento_Multas + " = " + Datos.P_Descuento_Multas + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Adeudo_Corriente + " = " + Datos.P_Adeudo_Corriente + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Adeudo_Rezago + " = " + Datos.P_Adeudo_Rezago + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Predial + " = " + Datos.P_Total_Predial + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Recargos + " = " + Datos.P_Total_Recargos + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Honorarios + " = " + Datos.P_Total_Honorarios + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Adeudo + " = " + Datos.P_Total_Adeudo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Descuento + " = " + Datos.P_Total_Descuento + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Sub_Total + " = " + Datos.P_Sub_Total + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Porcentaje_Anticipo + " = " + Datos.P_Porcentaje_Anticipo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Anticipo + " = " + Datos.P_Total_Anticipo + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Convenio + " = " + Datos.P_Total_Convenio + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Parcialidades_Manual + " = '" + Datos.P_Parcialidades_Manual + "', ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario + "', ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL += "WHERE " + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = '" + Datos.P_No_Convenio + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //Elimina el desglose de los Detalles de Impuestos
                Mi_SQL = "DELETE FROM " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial;
                Mi_SQL += " WHERE " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_No_Convenio + " = '" + Datos.P_No_Convenio + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                //Elimina los Detalles de los Impuestos
                Mi_SQL = "DELETE FROM " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial;
                Mi_SQL += " WHERE " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " = '" + Datos.P_No_Convenio + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //Inserta los Detalles de las Parcialidades de los Convenios
                if (Datos.P_Dt_Parcialidades != null)
                {
                    foreach (DataRow Dr_Detalle in Datos.P_Dt_Parcialidades.Rows)
                    {
                        Mi_SQL = "INSERT INTO " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + " (";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Usuario_Creo + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Creo + ") ";
                        Mi_SQL += "VALUES (";
                        Mi_SQL += "'" + Datos.P_No_Convenio + "', ";
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (!String.IsNullOrEmpty(Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo].ToString()))
                        {
                            Mi_SQL += "'" + Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo].ToString() + "', ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento] != null)
                        {
                            Mi_SQL += "'" + Convert.ToDateTime(Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento]).ToString("d-M-yyyy") + "', ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus] != null)
                        {
                            Mi_SQL += "'" + Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus].ToString() + "', ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        Mi_SQL += "'" + Datos.P_Usuario + "', SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                }

                //Inserta el desglose de las parcialidades del convenio
                if (Datos.P_Dt_Desglose_Parcialidades != null)
                {
                    foreach (DataRow Dr_Desglose in Datos.P_Dt_Desglose_Parcialidades.Rows)
                    {
                        Mi_SQL = "INSERT INTO " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + " (";
                        Mi_SQL += Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_No_Convenio + ", ";
                        Mi_SQL += Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_No_Pago + ", ";
                        Mi_SQL += Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Anio + ", ";
                        Mi_SQL += Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_1 + ", ";
                        Mi_SQL += Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_2 + ", ";
                        Mi_SQL += Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_3 + ", ";
                        Mi_SQL += Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_4 + ", ";
                        Mi_SQL += Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_5 + ", ";
                        Mi_SQL += Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_6 + ", ";
                        Mi_SQL += Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Usuario_Creo + ", ";
                        Mi_SQL += Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Fecha_Creo + ") ";
                        Mi_SQL += "VALUES (";
                        Mi_SQL += "'" + Datos.P_No_Convenio + "', '";
                        Mi_SQL += Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_No_Pago].ToString() + "', '";
                        Mi_SQL += Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Anio].ToString() + "', ";
                        if (Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_1].ToString() != "")
                        {
                            Mi_SQL += Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_1].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "0, ";
                        }
                        if (Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_2].ToString() != "")
                        {
                            Mi_SQL += Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_2].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "0, ";
                        }
                        if (Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_3].ToString() != "")
                        {
                            Mi_SQL += Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_3].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "0, ";
                        }
                        if (Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_4].ToString() != "")
                        {
                            Mi_SQL += Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_4].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "0, ";
                        }
                        if (Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_5].ToString() != "")
                        {
                            Mi_SQL += Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_5].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "0, ";
                        }
                        if (Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_6].ToString() != "")
                        {
                            Mi_SQL += Dr_Desglose[Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_6].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "0, ";
                        }
                        Mi_SQL += "'" + Datos.P_Usuario + "', SYSDATE)";
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
                    Mensaje = "Error al intentar modificar un Registro de Impuesto por Convenio predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///PARAMETROS           : 1. Convenio_Traslado_Dominio.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio
        ///                                 con los datos de Convenio_Traslado_Dominio que van a ser Modificados.
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 20/Marzo/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Modificar_Estatus_Convenio_Reestructura(Cls_Ope_Pre_Convenios_Predial_Negocio Convenio_Predial)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Modificado = false;

            if (Convenio_Predial.P_Cmmd != null)
            {
                Cmmd = Convenio_Predial.P_Cmmd;
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
                String Mi_SQL = "UPDATE " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial;
                Mi_SQL += " SET ";
                if (Convenio_Predial.P_Campos_Dinamicos != null && Convenio_Predial.P_Campos_Dinamicos != "")
                {
                    Mi_SQL += Convenio_Predial.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Estatus + " = '" + Convenio_Predial.P_Estatus + "'";
                }
                Mi_SQL += " WHERE ";
                if (Convenio_Predial.P_Filtros_Dinamicos != null && Convenio_Predial.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL += Convenio_Predial.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = '" + Convenio_Predial.P_No_Convenio + "'";
                }
                Cmmd.CommandText = Mi_SQL;
                Cmmd.ExecuteNonQuery();

                if (Convenio_Predial.P_Cmmd == null)
                {
                    Trans.Commit();
                }
                Modificado = true;
            }
            catch (OracleException Ex)
            {
                if (Convenio_Predial.P_Cmmd == null)
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
                if (Convenio_Predial.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }
            return Modificado;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Actualizar_Ruta_Convenio_Escaneado
        ///DESCRIPCIÓN          : Asigna un valor dado al campo Ruta_Convenio_Escaneado
        ///PARAMETROS          : 1. Datos. Instancia de la Clase de Negocio de Cls_Ope_Pre_Convenio_Predial_Negocio
        ///                         con los datos del Convenio que va a ser Modificado.
        ///CREO                 : Roberto González Oseguera
        ///FECHA_CREO           : 14-nov-2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static int Actualizar_Ruta_Convenio_Escaneado(Cls_Ope_Pre_Convenios_Predial_Negocio Datos)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Int32 Filas_Actualizadas = 0;

            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try
            {
                String Mi_SQL = "UPDATE " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + " SET ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Ruta_Convenio_Escaneado + " = '" + Datos.P_Ruta_Convenio_Escaneado + "', ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario + "', ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL += "WHERE " + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = '" + Datos.P_No_Convenio + "'";

                Cmd.CommandText = Mi_SQL;
                Filas_Actualizadas = Cmd.ExecuteNonQuery();

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
                    Mensaje = "Error al intentar modificar un Registro de Impuesto por Convenio predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
            return Filas_Actualizadas;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Reestructura_Convenio_Predial
        ///DESCRIPCIÓN          : Actualiza en la Base de Datos un registro de Convenio_Predial
        ///PARAMETROS           : 1. Datos.   Instancia de la Clase de Negocio 
        ///                                 con los datos del convenio que va a ser actualizado.
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 10/Agosto/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Modificar_Reestructura_Convenio_Predial(Cls_Ope_Pre_Convenios_Predial_Negocio Datos)
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
                String Mi_SQL = "UPDATE " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + " SET ";
                //Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "', ";
                if (Datos.P_Propietario_ID != "" && Datos.P_Propietario_ID != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Contribuyente_Id + " = '" + Datos.P_Propietario_ID + "', ";
                }
                if (Datos.P_Realizo != "" && Datos.P_Realizo != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Realizo + " = '" + Datos.P_Realizo + "', ";
                }
                if (Datos.P_Estatus != "" && Datos.P_Estatus != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                }
                if (Datos.P_Solicitante != "" && Datos.P_Solicitante != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Solicitante + " = '" + Datos.P_Solicitante + "', ";
                }
                if (Datos.P_RFC != "" && Datos.P_RFC != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_RFC + " = '" + Datos.P_RFC + "', ";
                }
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Numero_Parcialidades + " = " + Datos.P_Numero_Parcialidades + ", ";
                if (Datos.P_Periodicidad_Pago != "" && Datos.P_Periodicidad_Pago != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Periodicidad_Pago + " = '" + Datos.P_Periodicidad_Pago + "', ";
                }
                if (Datos.P_Fecha.ToString() != "" && Datos.P_Fecha != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Fecha + " = '" + Datos.P_Fecha.ToString("d-M-yyyy") + "', ";
                }
                if (Datos.P_Fecha_Vencimiento.ToString() != "" && Datos.P_Fecha_Vencimiento != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento + " = '" + Datos.P_Fecha_Vencimiento.ToString("d-M-yyyy") + "', ";
                }
                if (Datos.P_Observaciones != "" && Datos.P_Observaciones != null)
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Observaciones + " = '" + Datos.P_Observaciones + "', ";
                }
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Ordinarios + " = " + Datos.P_Descuento_Recargos_Ordinarios + ", "
                    + Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Moratorios + " = " + Datos.P_Descuento_Recargos_Moratorios + ", "
                    + Ope_Pre_Convenios_Predial.Campo_Descuento_Multas + " = " + Datos.P_Descuento_Multas + ", "
                    + Ope_Pre_Convenios_Predial.Campo_Adeudo_Corriente + " = " + Datos.P_Adeudo_Corriente + ", "
                    + Ope_Pre_Convenios_Predial.Campo_Adeudo_Rezago + " = " + Datos.P_Adeudo_Rezago + ", "
                    + Ope_Pre_Convenios_Predial.Campo_Total_Adeudo + " = " + Datos.P_Total_Adeudo + ", "
                    + Ope_Pre_Convenios_Predial.Campo_Total_Moratorios + " = " + Datos.P_Total_Moratorios + ", "
                    + Ope_Pre_Convenios_Predial.Campo_Total_Honorarios + " = " + Datos.P_Total_Honorarios + ", "
                    + Ope_Pre_Convenios_Predial.Campo_Total_Descuento + " = " + Datos.P_Total_Descuento + ", "
                    + Ope_Pre_Convenios_Predial.Campo_Sub_Total + " = " + Datos.P_Sub_Total + ", "
                    + Ope_Pre_Convenios_Predial.Campo_Porcentaje_Anticipo + " = " + Datos.P_Porcentaje_Anticipo + ", "
                    + Ope_Pre_Convenios_Predial.Campo_Total_Anticipo + " = " + Datos.P_Total_Anticipo + ", "
                    + Ope_Pre_Convenios_Predial.Campo_Total_Convenio + " = " + Datos.P_Total_Convenio + ", "
                    + Ope_Pre_Convenios_Predial.Campo_Parcialidades_Manual + " = '" + Datos.P_Parcialidades_Manual + "', "
                    + Ope_Pre_Convenios_Predial.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario + "', "
                    + Ope_Pre_Convenios_Predial.Campo_Fecha_Modifico + " = SYSDATE "
                    + "WHERE " + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = '" + Datos.P_No_Convenio + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //Elimina los Detalles de los Impuestos
                Mi_SQL = "DELETE FROM " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial;
                Mi_SQL += " WHERE " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " = '" + Datos.P_No_Convenio + "'";
                Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + " = (SELECT " + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + " FROM " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + " WHERE " + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + "='" + Datos.P_No_Convenio + "')";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //Inserta los Detalles de las Parcialidades de los Convenios
                if (Datos.P_Dt_Parcialidades != null
                    && Datos.P_Dt_Parcialidades.Rows.Count > 0)
                {
                    foreach (DataRow Dr_Detalle in Datos.P_Dt_Parcialidades.Rows)
                    {
                        Mi_SQL = "INSERT INTO " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + " ("
                            + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + ", "
                            + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + ", "
                            + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ", "
                            + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios + ", "
                            + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios + ", "
                            + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios + ", "
                            + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto + ", "
                            + Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento + ", "
                            + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + ", "
                            + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ", "
                            + Ope_Pre_Detalles_Convenios_Predial.Campo_Usuario_Creo + ", "
                            + Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Creo + ") "
                            + "VALUES ("
                            + "'" + Datos.P_No_Convenio + "', ";
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento] != null)
                        {
                            Mi_SQL += "'" + Convert.ToDateTime(Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento]).ToString("d-M-yyyy") + "', ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus] != null)
                        {
                            Mi_SQL += "'" + Dr_Detalle[Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus].ToString() + "', ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        Mi_SQL += Datos.P_No_Reestructura + ",";
                        Mi_SQL += "'" + Datos.P_Usuario + "', SYSDATE)";
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Convenio de predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN : Consultar_Convenio_Predial
        ///DESCRIPCIÓN          : Obtiene todos las Convenis de predial dados de alta en la base de datos
        ///PARAMETROS           : 1. Datos.   Instancia de la Clase de Negocio con informacion para la consulta
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 22/Julio/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Convenio_Predial(Cls_Ope_Pre_Convenios_Predial_Negocio Datos)
        {
            DataTable Dt_Predial = new DataTable();
            String Mi_SQL = "";
            try
            {
                if (Datos.P_Mostrar_Ultimo_Convenio)
                {
                    Mi_SQL += "SELECT * FROM (";
                }
                Mi_SQL += "SELECT ";
                if (Datos.P_Campos_Foraneos)
                {
                    Mi_SQL += "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial +
                        " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE "
                        + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = "
                        + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "."
                        + Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id + ") AS Cuenta_Predial, ";
                    Mi_SQL += "(SELECT " + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || "
                        + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || "
                        + Cat_Empleados.Campo_Nombre + " FROM "
                        + Cat_Empleados.Tabla_Cat_Empleados + " WHERE "
                        + Cat_Empleados.Campo_Empleado_ID + " = "
                        + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial
                        + "." + Ope_Pre_Convenios_Predial.Campo_Realizo + ") AS Nombre_Realizo, ";
                    Mi_SQL += "(SELECT " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || " +
                        Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || " +
                        Cat_Pre_Contribuyentes.Campo_Nombre + " FROM " +
                        Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " WHERE " +
                        Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = " +
                        Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." +
                        Ope_Pre_Convenios_Predial.Campo_Contribuyente_Id + ") AS Nombre_Propietario, ";
                }
                if (Datos.P_Campos_Dinamicos != null && Datos.P_Campos_Dinamicos != "")
                {
                    Mi_SQL += Datos.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_No_Convenio + ", "
                        + Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id + ", "
                        + Ope_Pre_Convenios_Predial.Campo_Contribuyente_Id + ", "
                        + Ope_Pre_Convenios_Predial.Campo_Realizo + ", "
                        + Ope_Pre_Convenios_Predial.Campo_Hasta_Periodo + ", "
                        + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ", TRIM("
                        + Ope_Pre_Convenios_Predial.Campo_Estatus + ") " + Ope_Pre_Convenios_Predial.Campo_Estatus + ", "
                        + Ope_Pre_Convenios_Predial.Campo_Solicitante + ", "
                        + Ope_Pre_Convenios_Predial.Campo_RFC + ", "
                        + Ope_Pre_Convenios_Predial.Campo_Numero_Parcialidades + ", "
                        + Ope_Pre_Convenios_Predial.Campo_Periodicidad_Pago + ", "
                        + Ope_Pre_Convenios_Predial.Campo_Fecha + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Observaciones + ", NVL(";
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Parcialidades_Manual
                        + ",'NO') " + Ope_Pre_Convenios_Predial.Campo_Parcialidades_Manual + ", NVL(";
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Ordinarios
                        + ",0) " + Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Ordinarios + ", NVL(";
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Moratorios
                        + ",0) " + Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Moratorios + ", NVL(";
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Descuento_Multas
                        + ",0) " + Ope_Pre_Convenios_Predial.Campo_Descuento_Multas + ", ";
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_No_Descuento + ", NVL(";
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Adeudo_Corriente
                        + ",0) " + Ope_Pre_Convenios_Predial.Campo_Adeudo_Corriente + ", NVL(";
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Adeudo_Rezago
                        + ",0) " + Ope_Pre_Convenios_Predial.Campo_Adeudo_Rezago + ", NVL(";
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Predial
                        + ",0) " + Ope_Pre_Convenios_Predial.Campo_Total_Predial + ", NVL(";
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Recargos
                        + ",0) " + Ope_Pre_Convenios_Predial.Campo_Total_Recargos + ", NVL(";
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Moratorios
                        + ",0) " + Ope_Pre_Convenios_Predial.Campo_Total_Moratorios + ", NVL(";
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Honorarios
                        + ",0) " + Ope_Pre_Convenios_Predial.Campo_Total_Honorarios + ", NVL(";
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Adeudo
                        + ",0)" + Ope_Pre_Convenios_Predial.Campo_Total_Adeudo + ", NVL(";
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Descuento
                        + ",0) " + Ope_Pre_Convenios_Predial.Campo_Total_Descuento + ", NVL(";
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Sub_Total
                        + ",0)" + Ope_Pre_Convenios_Predial.Campo_Sub_Total + ", NVL(";
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Porcentaje_Anticipo
                        + ",0) " + Ope_Pre_Convenios_Predial.Campo_Porcentaje_Anticipo + ", NVL(";
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Anticipo
                        + ",0) " + Ope_Pre_Convenios_Predial.Campo_Total_Anticipo + ", NVL(";
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Anticipo
                        + ",0) " + Ope_Pre_Convenios_Predial.Campo_Anticipo + ", NVL(";
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Anticipo_Reestructura
                        + ",0) " + Ope_Pre_Convenios_Predial.Campo_Anticipo_Reestructura + ", NVL(";
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Total_Convenio
                        + ",0) " + Ope_Pre_Convenios_Predial.Campo_Total_Convenio;
                }
                Mi_SQL += " FROM " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial;
                if (Datos.P_Filtros_Dinamicos != null && Datos.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL += " WHERE " + Datos.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL += " WHERE ";
                    if (Datos.P_No_Convenio != "" && Datos.P_No_Convenio != null)
                    {
                        Mi_SQL += Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = '" + Datos.P_No_Convenio + "' AND ";
                    }
                    if (Datos.P_Cuenta_Predial_ID != "" && Datos.P_Cuenta_Predial_ID != null)
                    {
                        Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id + " = '" + Datos.P_Cuenta_Predial_ID + "' AND ";
                    }
                    if (Datos.P_Propietario_ID != "" && Datos.P_Propietario_ID != null)
                    {
                        Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Contribuyente_Id + " = '" + Datos.P_Propietario_ID + "' AND ";
                    }
                    if (Datos.P_Realizo != "" && Datos.P_Realizo != null)
                    {
                        Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Realizo + " = '" + Datos.P_Realizo + "' AND ";
                    }
                    if (Datos.P_No_Reestructura != null)
                    {
                        Mi_SQL += Ope_Pre_Convenios_Predial.Campo_No_Reestructura + " = " + Datos.P_No_Reestructura + " AND ";
                    }
                    if (Datos.P_Estatus != "" && Datos.P_Estatus != null)
                    {
                        Mi_SQL += "TRIM(" + Ope_Pre_Convenios_Predial.Campo_Estatus + ") " + Datos.P_Estatus + " AND ";
                    }
                    if (Datos.P_Solicitante != "" && Datos.P_Solicitante != null)
                    {
                        Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Solicitante + " LIKE '%" + Datos.P_Solicitante + "%' AND ";
                    }
                    if (Datos.P_RFC != "" && Datos.P_RFC != null)
                    {
                        Mi_SQL += Ope_Pre_Convenios_Predial.Campo_RFC + " = '" + Datos.P_RFC + "' AND ";
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
                if (Datos.P_Agrupar_Dinamico != null && Datos.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " GROUP BY " + Datos.P_Agrupar_Dinamico;
                }
                if (Datos.P_Ordenar_Dinamico != null && Datos.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Datos.P_Ordenar_Dinamico;
                }
                else
                {
                    Mi_SQL += " ORDER BY " + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " DESC";
                }
                if (Datos.P_Mostrar_Ultimo_Convenio)
                {
                    Mi_SQL += ") WHERE ROWNUM = 1";
                }

                DataSet Ds_Predial = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Predial != null)
                {
                    Dt_Predial = Ds_Predial.Tables[0];

                    if (Dt_Predial.Rows.Count > 0)
                    {
                        //Consulta las Parcialidades del Convenio
                        Mi_SQL = "SELECT ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto + ", ";
                        Mi_SQL += "(" + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios + " + " +
                            Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios + " + " +
                            Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios + " + " +
                            Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto + ") MONTO_IMPORTE, ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ", TRIM(";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + ") " + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus;
                        Mi_SQL += " FROM ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial;
                        Mi_SQL += " WHERE ";
                        if (Datos.P_No_Convenio != null && Datos.P_No_Convenio != "")
                        {
                            Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " = '" + Datos.P_No_Convenio + "' AND ";
                            if (Datos.P_Reestructura)
                            {
                                Mi_SQL += "" + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + " = (SELECT " + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + " FROM " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + " WHERE " + Ope_Pre_Convenios_Predial.Campo_No_Convenio + "='" + Datos.P_No_Convenio + "') AND ";
                            }
                            else
                            {
                                Mi_SQL += " NVL(" + Ope_Pre_Convenios_Predial.Campo_No_Reestructura + ", 0) = 0 AND ";
                            }
                        }
                        else
                        {
                            Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " = '" + Ds_Predial.Tables[0].Rows[0][Ope_Pre_Convenios_Predial.Campo_No_Convenio] + "' AND ";
                        }
                        if (Datos.P_Cuenta_Predial_ID != null && Datos.P_Cuenta_Predial_ID != "")
                        {
                            Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " IN (SELECT " + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " FROM " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + " WHERE " + Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id + " = '" + Datos.P_Cuenta_Predial_ID + "') AND ";
                        }
                        if (Mi_SQL.EndsWith(" AND "))
                        {
                            Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                        }
                        if (Mi_SQL.EndsWith(" WHERE "))
                        {
                            Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                        }
                        Mi_SQL += " ORDER BY " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago;
                        Datos.P_Dt_Parcialidades = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    }
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Impuestos Predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Predial;
        }




        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Ordenes_Variacion
        ///DESCRIPCIÓN          : Devuelve un DataTable con los registros de las Órdenes de Variación
        ///PARAMETROS           : Ordenes_Variacion, instancia de la capa de Negocios
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 20/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Ordenes_Variacion(Cls_Ope_Pre_Convenios_Predial_Negocio Ordenes_Variacion)
        {
            DataTable Dt_Ordenes_Variacion = new DataTable();
            String Mi_SQL = "";
            try
            {
                Mi_SQL += "SELECT ";
                if (Ordenes_Variacion.P_Incluir_Campos_Foraneos)
                {
                    Mi_SQL += "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial
                        + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas
                        + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = "
                        + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                        + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + ") AS Cuenta_Predial, ";
                    Mi_SQL += "(SELECT " + Cat_Pre_Movimientos.Campo_Identificador
                        + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos
                        + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = "
                        + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                        + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ") AS Identificador_Movimiento, ";
                    Mi_SQL += "(SELECT " + Cat_Pre_Movimientos.Campo_Descripcion
                        + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos
                        + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = "
                        + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                        + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ") AS Descripcion_Movimiento, ";
                }
                if (!string.IsNullOrEmpty(Ordenes_Variacion.P_Campos_Dinamicos))
                {
                    Mi_SQL += Ordenes_Variacion.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ", " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + ".";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ", " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + ".";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_No_Nota + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Fecha_Nota + ", ";
                    Mi_SQL += "(SELECT " + Cat_Pre_Movimientos.Campo_Identificador + " || ' - ' || "
                        + Cat_Pre_Movimientos.Campo_Descripcion
                        + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos
                        + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = "
                        + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                        + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ") AS Clave_Y_Movimiento, ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Grupo_Movimiento_ID + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Numero_Nota_Impreso + ", " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + ".";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Anio + ", " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + ".";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Observaciones + ", " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + ".";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Usuario_Creo + ", " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + ".";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo;
                }
                Mi_SQL += " FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion;

                if (Ordenes_Variacion.P_Join_Contrarecibo)
                {
                    Mi_SQL += " JOIN " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + " CR ON "
                        + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                                + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + " = CR."
                                + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " AND "
                                + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                                + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = CR."
                                + Ope_Pre_Contrarecibos.Campo_Anio;
                    if (!String.IsNullOrEmpty(Ordenes_Variacion.P_Contrarecibo_Estatus))
                    {
                        Mi_SQL += " AND CR." + Ope_Pre_Contrarecibos.Campo_Estatus + Ordenes_Variacion.P_Contrarecibo_Estatus;
                    }
                    else
                    {
                        Mi_SQL += " AND CR." + Ope_Pre_Contrarecibos.Campo_Estatus + " NOT IN ('PAGADO', 'PENDIENTE')";
                    }
                }

                if (!string.IsNullOrEmpty(Ordenes_Variacion.P_Filtros_Dinamicos))
                {
                    Mi_SQL += " WHERE " + Ordenes_Variacion.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL += " WHERE ";
                    if (!string.IsNullOrEmpty(Ordenes_Variacion.P_Orden_Variacion_ID))
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                            + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion
                            + Validar_Operador_Comparacion(Ordenes_Variacion.P_Orden_Variacion_ID) + " AND ";
                    }
                    if (!string.IsNullOrEmpty(Ordenes_Variacion.P_Cuenta_Predial_ID))
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                            + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " = '"
                            + Ordenes_Variacion.P_Cuenta_Predial_ID + "' AND ";
                    }
                    if (!string.IsNullOrEmpty(Ordenes_Variacion.P_Cuenta_Predial))
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                            + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " IN (SELECT "
                            + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " FROM "
                            + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE UPPER("
                            + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ")"
                            + Validar_Operador_Comparacion(Ordenes_Variacion.P_Cuenta_Predial) + ") AND ";
                    }
                    if (!string.IsNullOrEmpty(Ordenes_Variacion.P_Generar_Orden_Movimiento_ID))
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                            + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + " = '"
                            + Ordenes_Variacion.P_Generar_Orden_Movimiento_ID + "' AND ";
                    }
                    if (!string.IsNullOrEmpty(Ordenes_Variacion.P_Contrarecibo))
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                            + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo
                            + Validar_Operador_Comparacion(Ordenes_Variacion.P_Contrarecibo) + " AND ";
                    }
                    if (!string.IsNullOrEmpty(Ordenes_Variacion.P_Generar_Orden_Anio))
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                            + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = "
                            + Ordenes_Variacion.P_Generar_Orden_Anio + " AND ";
                    }
                    if (!string.IsNullOrEmpty(Ordenes_Variacion.P_Generar_Orden_Estatus))
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                            + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " = '"
                            + Ordenes_Variacion.P_Generar_Orden_Estatus + "' AND ";
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
                if (!string.IsNullOrEmpty(Ordenes_Variacion.P_Agrupar_Dinamico))
                {
                    Mi_SQL += " GROUP BY " + Ordenes_Variacion.P_Agrupar_Dinamico;
                }
                if (!string.IsNullOrEmpty(Ordenes_Variacion.P_Ordenar_Dinamico))
                {
                    Mi_SQL += " ORDER BY " + Ordenes_Variacion.P_Ordenar_Dinamico;
                }
                else
                {
                    Mi_SQL += " ORDER BY " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                            + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " DESC";
                }

                DataSet Ds_Ordenes_Variacion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                
                if (Ds_Ordenes_Variacion != null)
                {
                    Dt_Ordenes_Variacion = Ds_Ordenes_Variacion.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Ordenes_Variacion;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Propietarios_Variacion
        /// DESCRIPCIÓN: Se consultan con el ID de la cuenta los datos del Propietario dado de alta en al orden de varación
        /// PARÁMETROS:
        /// 		1. Cuenta_Predial_ID: Numero de cuenta predial a consultar
        /// 		2. No_Orden_Variacion: Numero de orden de variación con los datos a consultar
        /// 		3. Anio: Año de la orden de variación
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 05-nov-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************    
        internal static DataTable Consultar_Propietarios_Variacion(String Cuenta_Predial_ID, String No_Orden_Variacion, String Anio_Orden)
        {
            String Mi_SQL = "";
            DataSet Ds_Copropietarios_Variacion = new DataSet();
            try
            {
                Mi_SQL += "SELECT ";
                Mi_SQL += Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID + ", ";
                Mi_SQL += Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo;

                Mi_SQL += " FROM " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion;

                Mi_SQL += " WHERE ";
                Mi_SQL += Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_ID + "'";
                Mi_SQL += " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " IN ('PROPIETARIO', 'POSEEDOR')";
                Mi_SQL += " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + " = '" + No_Orden_Variacion + "'";
                Mi_SQL += " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus + " = 'ALTA'";
                Mi_SQL += " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio + " = " + Anio_Orden;

                Ds_Copropietarios_Variacion.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                Ds_Copropietarios_Variacion.Tables[0].TableName = "Dt_Copropietarios_Variacion";

                return Ds_Copropietarios_Variacion.Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Mi_SQL + Ex.Message);
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
        ///NOMBRE DE LA FUNCIÓN : Validar_Operador_Comparacion
        ///DESCRIPCIÓN          : Devuelve una cadena adecuada al operador indicado en la capa de Negocios
        ///PARAMETROS           : 
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 20/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static String Validar_Operador_Comparacion(String Filtro)
        {
            String Cadena_Validada;
            if (Filtro.Trim().StartsWith("<")
               || Filtro.Trim().StartsWith(">")
               || Filtro.Trim().StartsWith("<>")
               || Filtro.Trim().StartsWith("<=")
               || Filtro.Trim().StartsWith(">=")
               || Filtro.Trim().StartsWith("=")
               || Filtro.Trim().ToUpper().StartsWith("BETWEEN")
               || Filtro.Trim().ToUpper().StartsWith("LIKE")
               || Filtro.Trim().ToUpper().StartsWith("IN"))
            {
                Cadena_Validada = " " + Filtro + " ";
            }
            else
            {
                if (Filtro.Trim().ToUpper().StartsWith("NULL"))
                {
                    Cadena_Validada = " IS " + Filtro + " ";
                }
                else
                {
                    Cadena_Validada = " = '" + Filtro + "' ";
                }
            }
            return Cadena_Validada;
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
        ///NOMBRE DE LA FUNCIÓN : Consultar_Adeudos_Cuenta
        ///DESCRIPCIÓN          : Obtiene los adeudos de la cuenta predial 
        ///PARAMETROS           : 1. Datos.   Instancia de la Clase de Negocio con informacion para la consulta
        ///CREO                 : Miguel Angel Bedolla Moreno
        ///FECHA_CREO           : 02/Octubre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Adeudos_Cuenta(Cls_Ope_Pre_Convenios_Predial_Negocio Datos)
        {
            DataTable Dt_Adeudos = new DataTable();
            String Mi_SQL = "";
            try
            {
                Mi_SQL += "SELECT ";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Anio + " AS ANIO, SUM(NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ",0)) AS ADEUDO_BIMESTRE_1, SUM(NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ",0)) AS ADEUDO_BIMESTRE_2, SUM(NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ",0)) AS ADEUDO_BIMESTRE_3, SUM(NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ",0)) AS ADEUDO_BIMESTRE_4, SUM(NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ",0)) AS ADEUDO_BIMESTRE_5, SUM(NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ",0)) AS ADEUDO_BIMESTRE_6 ";
                Mi_SQL += "FROM " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + " WHERE ((NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ",0)) + (NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ",0)) + (NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ",0)) + (NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ",0)) + (NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ",0)) + (NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ",0))) !=0.00 AND ";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Anio + "<= " + Datos.P_Año + " AND ";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + "= '" + Datos.P_Cuenta_Predial_ID + "' ";
                if (Datos.P_No_Convenio != null && Datos.P_No_Convenio != "")
                {
                    Mi_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_No_Convenio + "= '" + Datos.P_No_Convenio + "'";
                }
                Mi_SQL += " GROUP BY " + Ope_Pre_Adeudos_Predial.Campo_Anio;
                Mi_SQL += " ORDER BY " + Ope_Pre_Adeudos_Predial.Campo_Anio;
                DataSet Ds_Adeudos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Adeudos != null)
                {
                    Dt_Adeudos = Ds_Adeudos.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los Adeudos de Predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Adeudos;
        }

        #endregion

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Adeudos_Convenio
        ///DESCRIPCIÓN          : Consulta el saldo del convenio
        ///PARAMETROS           : 1. Convenio_Traslado_Dominio.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio
        ///CREO                 : Miguel Angel Bedolla Moreno
        ///FECHA_CREO           : 09/Octubre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Adeudos_Convenio(Cls_Ope_Pre_Convenios_Predial_Negocio Convenio_Predial, Boolean Total_O_APagar)
        {
            DataTable Dt_Traslado_Dominio = new DataTable();
            try
            {
                String Mi_SQL = " SELECT SUM(";
                Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios + ") AS TOTAL_MULTAS, SUM(";
                Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto + ") AS TOTAL_IMPUESTO, SUM(";
                Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios + ") AS TOTAL_ORDINARIOS, SUM(";
                Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios + ") AS TOTAL_MORATORIOS ";
                Mi_SQL += "FROM ";
                Mi_SQL += Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + " WHERE ";
                Mi_SQL += Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio + "='" + Convenio_Predial.P_No_Convenio + "' AND ";
                if (Convenio_Predial.P_Reestructura)
                {
                    Mi_SQL += Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Reestructura + "=(SELECT " + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Reestructura + " FROM " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + " WHERE " + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio + "='" + Convenio_Predial.P_No_Convenio + "')";
                }
                else
                {
                    Mi_SQL += Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Reestructura + " IS NULL";
                }
                if (Total_O_APagar)
                {
                    Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Estatus + " = 'PENDIENTE'";
                }
                DataSet Ds_Traslado_Dominio = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Traslado_Dominio != null)
                {
                    Dt_Traslado_Dominio = Ds_Traslado_Dominio.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Impuestos por Derechos de Supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Traslado_Dominio;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Estatus_Archivo_Convenio
        ///DESCRIPCIÓN          : Consulta el estatus y el campo ruta a convenio escaneado
        ///PARAMETROS           : 1. Convenio_Traslado_Dominio.   Instancia de la Clase de Negocio con datos a consultar
        ///CREO                 : Roberto González Oseguera
        ///FECHA_CREO           : 12-nov-2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Estatus_Archivo_Convenio(Cls_Ope_Pre_Convenios_Predial_Negocio Convenio_Predial)
        {
            DataTable Dt_Convenio = new DataTable();
            try
            {
                String Mi_SQL = "SELECT ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Estatus + ", ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Ruta_Convenio_Escaneado;
                Mi_SQL += " FROM ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + " WHERE ";
                Mi_SQL += Ope_Pre_Convenios_Predial.Campo_No_Convenio + "='" + Convenio_Predial.P_No_Convenio + "'";

                DataSet Ds_Traslado_Dominio = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Traslado_Dominio != null)
                {
                    Dt_Convenio = Ds_Traslado_Dominio.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Convenios de predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Convenio;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Parcialidades_Ultimo_Convenio
        ///DESCRIPCIÓN          : Consulta las parcialidades de ultimo convenio para un numero de convenio dado
        ///                     (se filtra el numero de reestructura)
        ///PARAMETROS           : 1. Convenio_Traslado_Dominio.   Instancia de la Clase de Negocio con datos a consultar
        ///CREO                 : Roberto González Oseguera
        ///FECHA_CREO           : 23-nov-2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Parcialidades_Ultimo_Convenio(Cls_Ope_Pre_Convenios_Predial_Negocio Convenio_Predial)
        {
            DataTable Dt_Convenio = new DataTable();
            try
            {
                String Mi_SQL = "SELECT "
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + ", "
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + ", "
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento + ", TRIM("
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + ") " + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + ", "
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios + ", "
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios + ", "
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios + ", "
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto + ", "
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + ", "
                    + "(" + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios + " + "
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios + " + "
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios + " + "
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto + ") MONTO_IMPORTE, "
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura
                    + " FROM "
                    + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + " WHERE "
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + "='" + Convenio_Predial.P_No_Convenio + "'"
                    + " AND NVL("
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + " , 0) = "
                    + "(SELECT NVL(MAX(" + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + "),0) FROM "
                    + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + " WHERE "
                    + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " = '" + Convenio_Predial.P_No_Convenio + "')";

                // decrementar el número de reestructura sólo si se especifica en filtros dinamicos el decremento
                if (!string.IsNullOrEmpty(Convenio_Predial.P_Filtros_Dinamicos) && Convenio_Predial.P_Filtros_Dinamicos == "-1")
                {
                    Mi_SQL += Convenio_Predial.P_Filtros_Dinamicos;
                }

                // si se especifica orden, agregar ORDER BY
                if (!string.IsNullOrEmpty(Convenio_Predial.P_Ordenar_Dinamico))
                {
                    Mi_SQL +=  " ORDER BY " + Convenio_Predial.P_Ordenar_Dinamico;
                }

                DataSet Ds_Traslado_Dominio = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Traslado_Dominio != null)
                {
                    Dt_Convenio = Ds_Traslado_Dominio.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Convenios de predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Convenio;
        }

    }

}