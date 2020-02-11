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
using Presidencia.Cls_Ope_Ing_Ordenes_Pago.Negocio;

namespace Presidencia.Cls_Ope_Ing_Ordenes_Pago.Datos
{

    public class Cls_Ope_Ing_Ordenes_Pago_Datos
    {

        #region Altas

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Orden_Pago
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos los datos de la Orden de Pago
        ///PARAMETROS           : Orden_Pago, instancia de Cls_Ope_Ing_Ordenes_Pago_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 07/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Alta_Orden_Pago(Cls_Ope_Ing_Ordenes_Pago_Negocio Orden_Pago)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Estatus_Alta = false;

            if (Orden_Pago.P_Cmmd != null)
            {
                Cmmd = Orden_Pago.P_Cmmd;
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
                Orden_Pago.P_No_Orden_Pago = Obtener_ID_Consecutivo(ref Cmmd, Ope_Ing_Ordenes_Pago.Campo_No_Orden_Pago, Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago, Ope_Ing_Ordenes_Pago.Campo_Año + " = " + DateTime.Now.ToString("yyyy") + " AND " + Ope_Ing_Ordenes_Pago.Campo_Folio + " LIKE '" + Orden_Pago.P_Folio + "%'", 10);
                Orden_Pago.P_Año = DateTime.Now.Year;
                Orden_Pago.P_Folio += /*"ING" +*/ Orden_Pago.P_Año.ToString() + Convert.ToInt64(Orden_Pago.P_No_Orden_Pago).ToString();
                String Mi_SQL = "INSERT INTO " + Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago;
                Mi_SQL += " (" + Ope_Ing_Ordenes_Pago.Campo_No_Orden_Pago;
                Mi_SQL += ", " + Ope_Ing_Ordenes_Pago.Campo_Año;
                Mi_SQL += ", " + Ope_Ing_Ordenes_Pago.Campo_Contribuyente_ID;
                Mi_SQL += ", " + Ope_Ing_Ordenes_Pago.Campo_Dependencia_ID;
                Mi_SQL += ", " + Ope_Ing_Ordenes_Pago.Campo_Estatus;
                Mi_SQL += ", " + Ope_Ing_Ordenes_Pago.Campo_Folio;
                Mi_SQL += ", " + Ope_Ing_Ordenes_Pago.Campo_Total;
                Mi_SQL += ", " + Ope_Ing_Ordenes_Pago.Campo_Usuario_Creo;
                Mi_SQL += ", " + Ope_Ing_Ordenes_Pago.Campo_Fecha_Creo + ")";
                Mi_SQL += " VALUES ('" + Orden_Pago.P_No_Orden_Pago + "'";
                Mi_SQL += ", '" + Orden_Pago.P_Año + "'";
                Mi_SQL += ", '" + Orden_Pago.P_Contribuyente_ID + "'";
                Mi_SQL += ", '" + Orden_Pago.P_Dependencia_ID + "'";
                Mi_SQL += ", '" + Orden_Pago.P_Estatus + "'";
                Mi_SQL += ", '" + Orden_Pago.P_Folio + "'";
                Mi_SQL += ", " + Orden_Pago.P_Total;
                Mi_SQL += ", '" + Orden_Pago.P_Usuario + "'";
                Mi_SQL += ", SYSDATE";
                Mi_SQL += ")";
                Cmmd.CommandText = Mi_SQL;
                Cmmd.ExecuteNonQuery();

                if (Orden_Pago.P_Dt_Conceptos_Ordenes_Pago != null)
                {
                    foreach (DataRow Dr_Conceptos_Orden_Pago in Orden_Pago.P_Dt_Conceptos_Ordenes_Pago.Rows)
                    {
                        Orden_Pago.P_Concepto_Orden_Pago_ID = Obtener_ID_Consecutivo(ref Cmmd, Ope_Ing_Conceptos_Ordenes_Pago.Campo_Concepto_Orden_Pago_ID, Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago, "", 10);
                        Mi_SQL = "INSERT INTO " + Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago;
                        Mi_SQL += " (" + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Concepto_Orden_Pago_ID;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_No_Orden_Pago;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Año;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Folio;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_SubConcepto_Ing_ID;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Tipo_Pago_ID;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Garantia_Proceso_ID;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Banco_ID;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fuente_Financiamiento_ID;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Numero_Garantia;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Clave_Padron;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Estatus;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fecha_Multa;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Unidades;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Importe;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Descuento_Importe;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Monto_Importe;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Honorarios;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Multas;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Moratorios;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Recargos;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Ajuste_Tarifario;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Total;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Referencia;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Observaciones;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Usuario_Creo;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fecha_Creo + ")";
                        Mi_SQL += " VALUES ('" + Orden_Pago.P_Concepto_Orden_Pago_ID + "'";
                        Mi_SQL += ", '" + Orden_Pago.P_No_Orden_Pago + "'";
                        Mi_SQL += ", '" + Orden_Pago.P_Año + "'";
                        Mi_SQL += ", '" + Orden_Pago.P_Folio + "'";
                        Mi_SQL += ", '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_SubConcepto_Ing_ID].ToString() + "'";
                        Mi_SQL += ", '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Tipo_Pago_ID].ToString() + "'";
                        Mi_SQL += ", '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Garantia_Proceso_ID].ToString() + "'";
                        Mi_SQL += ", '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Banco_ID].ToString() + "'";
                        Mi_SQL += ", '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fuente_Financiamiento_ID].ToString() + "'";
                        Mi_SQL += ", '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Numero_Garantia].ToString() + "'";
                        Mi_SQL += ", '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Clave_Padron].ToString() + "'";
                        Mi_SQL += ", '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Estatus].ToString() + "'";
                        Mi_SQL += ", '" + Convert.ToDateTime(Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fecha_Multa]).ToString("dd-MM-yyyy") + "'";
                        Mi_SQL += ", " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Unidades].ToString();
                        Mi_SQL += ", " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Importe].ToString();
                        Mi_SQL += ", " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Descuento_Importe].ToString();
                        Mi_SQL += ", " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Monto_Importe].ToString();
                        Mi_SQL += ", " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Honorarios].ToString();
                        Mi_SQL += ", " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Multas].ToString();
                        Mi_SQL += ", " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Moratorios].ToString();
                        Mi_SQL += ", " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Recargos].ToString();
                        Mi_SQL += ", " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Ajuste_Tarifario].ToString();
                        Mi_SQL += ", " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Total].ToString();
                        Mi_SQL += ", '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Referencia].ToString() + "'";
                        Mi_SQL += ", '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Observaciones].ToString() + "'";
                        Mi_SQL += ", '" + Orden_Pago.P_Usuario + "'";
                        Mi_SQL += ", SYSDATE";
                        Mi_SQL += ")";
                        Cmmd.CommandText = Mi_SQL;
                        Cmmd.ExecuteNonQuery();
                    }
                }

                if (Orden_Pago.P_Cmmd == null)
                {
                    Trans.Commit();
                }

                Estatus_Alta = true;
            }
            catch (OracleException Ex)
            {
                if (Orden_Pago.P_Cmmd == null)
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
                    Mensaje = "Error al intentar dar de Alta una P_Clave de Ingreso. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Orden_Pago.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }

            return Estatus_Alta;
        }
        #endregion

        #region Modificaciones
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Orden_Pago
        ///DESCRIPCIÓN          : Modifica en la Base de Datos los datos de la Orden de Pago
        ///PARAMETROS           : Orden_Pago, instancia de Cls_Ope_Ing_Ordenes_Pago_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 07/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Modificar_Orden_Pago(Cls_Ope_Ing_Ordenes_Pago_Negocio Orden_Pago)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Estatus_Modificar = false;
            Boolean Varios_Conceptos = false;

            if (Orden_Pago.P_Cmmd != null)
            {
                Cmmd = Orden_Pago.P_Cmmd;
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
                String Mi_SQL;
                Mi_SQL = "UPDATE " + Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago;
                Mi_SQL += " SET ";
                if (Orden_Pago.P_Contribuyente_ID != null && Orden_Pago.P_Contribuyente_ID != "")
                {
                    Mi_SQL += Ope_Ing_Ordenes_Pago.Campo_Contribuyente_ID + " = '" + Orden_Pago.P_Contribuyente_ID + "', ";
                }
                if (Orden_Pago.P_Dependencia_ID != null && Orden_Pago.P_Dependencia_ID != "")
                {
                    Mi_SQL += Ope_Ing_Ordenes_Pago.Campo_Dependencia_ID + " = '" + Orden_Pago.P_Dependencia_ID + "', ";
                }
                if (Orden_Pago.P_Estatus != null && Orden_Pago.P_Estatus != "")
                {
                    Mi_SQL += Ope_Ing_Ordenes_Pago.Campo_Estatus + " = '" + Orden_Pago.P_Estatus + "', ";
                }
                if (Orden_Pago.P_Folio != null && Orden_Pago.P_Folio != "")
                {
                    Mi_SQL += Ope_Ing_Ordenes_Pago.Campo_Folio + " = '" + Orden_Pago.P_Folio + "', ";
                }
                if (Orden_Pago.P_Proteccion != null && Orden_Pago.P_Proteccion != "")
                {
                    Mi_SQL += Ope_Ing_Ordenes_Pago.Campo_Proteccion + " = '" + Orden_Pago.P_Proteccion + "', ";
                }
                if (Orden_Pago.P_Total != 0)
                {
                    Mi_SQL += Ope_Ing_Ordenes_Pago.Campo_Total + " = " + Orden_Pago.P_Total + ", ";
                }
                Mi_SQL += Ope_Ing_Ordenes_Pago.Campo_Usuario_Modifico + " = '" + Orden_Pago.P_Usuario + "', ";
                Mi_SQL += Ope_Ing_Ordenes_Pago.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL += " WHERE " + Ope_Ing_Ordenes_Pago.Campo_No_Orden_Pago + " = '" + Orden_Pago.P_No_Orden_Pago + "'";
                Mi_SQL += " AND " + Ope_Ing_Ordenes_Pago.Campo_Año + " = '" + Orden_Pago.P_Año + "'";
                Mi_SQL += " AND " + Ope_Ing_Ordenes_Pago.Campo_Folio + " = '" + Orden_Pago.P_Folio + "'";
                Cmmd.CommandText = Mi_SQL;
                Cmmd.ExecuteNonQuery();

                if (Orden_Pago.P_Dt_Conceptos_Ordenes_Pago != null)
                {
                    if (Orden_Pago.P_Dt_Conceptos_Ordenes_Pago.Rows.Count > 0
                        && Orden_Pago.P_Actualiza_Conceptos)
                    {
                        Varios_Conceptos = true;
                        foreach (DataRow Dr_Conceptos_Orden_Pago in Orden_Pago.P_Dt_Conceptos_Ordenes_Pago.Rows)
                        {
                            Mi_SQL = "SELECT " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Concepto_Orden_Pago_ID;
                            Mi_SQL += " FROM " + Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago;
                            //Mi_SQL += " WHERE " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_No_Orden_Pago + " = '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_No_Orden_Pago].ToString() + "'";
                            //Mi_SQL += " AND " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Año + " = " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Año].ToString();
                            Mi_SQL += " WHERE " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Concepto_Orden_Pago_ID + " = '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Concepto_Orden_Pago_ID].ToString() + "'";
                            Cmmd.CommandText = Mi_SQL;
                            OracleDataReader DReader = Cmmd.ExecuteReader();

                            if (DReader.Read())
                            {
                                Mi_SQL = "UPDATE " + Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago;
                                Mi_SQL += " SET ";
                                Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_SubConcepto_Ing_ID + " = '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_SubConcepto_Ing_ID].ToString() + "', ";
                                Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Tipo_Pago_ID + " = '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Tipo_Pago_ID].ToString() + "', ";
                                Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Garantia_Proceso_ID + " = '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Garantia_Proceso_ID].ToString() + "', ";
                                Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Banco_ID + " = '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Banco_ID].ToString() + "', ";
                                Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fuente_Financiamiento_ID + " = '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fuente_Financiamiento_ID].ToString() + "', ";
                                Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Numero_Garantia + " = '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Numero_Garantia].ToString() + "', ";
                                Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Clave_Padron + " = '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Clave_Padron].ToString() + "', ";
                                Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fecha_Multa + " = '" + Convert.ToDateTime(Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fecha_Multa]).ToString("dd-MM-yyyy") + "', ";
                                Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Estatus + " = '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Estatus].ToString() + "', ";
                                Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Unidades + " = " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Unidades].ToString() + ", ";
                                Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Importe + " = " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Importe].ToString() + ", ";
                                Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Descuento_Importe + " = " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Descuento_Importe].ToString() + ", ";
                                Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Monto_Importe + " = " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Monto_Importe].ToString() + ", ";
                                Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Honorarios + " = " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Honorarios].ToString() + ", ";
                                Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Multas + " = " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Multas].ToString() + ", ";
                                Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Moratorios + " = " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Moratorios].ToString() + ", ";
                                Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Recargos + " = " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Recargos].ToString() + ", ";
                                Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Ajuste_Tarifario + " = " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Ajuste_Tarifario].ToString() + ", ";
                                Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Total + " = " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Total].ToString() + ", ";
                                Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Referencia + " = '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Referencia].ToString() + "', ";
                                Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Observaciones + " = '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Observaciones].ToString() + "', ";
                                Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Usuario_Modifico + " = '" + Orden_Pago.P_Usuario + "', ";
                                Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fecha_Modifico + " = SYSDATE";
                                Mi_SQL += " WHERE " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_No_Orden_Pago + " = '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_No_Orden_Pago].ToString() + "'";
                                Mi_SQL += " AND " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Año + " = " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Año].ToString();
                                Mi_SQL += " AND " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Concepto_Orden_Pago_ID + " = '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Concepto_Orden_Pago_ID].ToString() + "'";
                                Mi_SQL += " AND " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Folio + " = '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Folio].ToString() + "'";
                            }
                            else
                            {
                                Orden_Pago.P_Concepto_Orden_Pago_ID = Obtener_ID_Consecutivo(ref Cmmd, Ope_Ing_Conceptos_Ordenes_Pago.Campo_Concepto_Orden_Pago_ID, Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago, "", 10);
                                Mi_SQL = "INSERT INTO " + Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago;
                                Mi_SQL += " (" + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Concepto_Orden_Pago_ID;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_No_Orden_Pago;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Año;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Folio;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_SubConcepto_Ing_ID;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Tipo_Pago_ID;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Garantia_Proceso_ID;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Banco_ID;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fuente_Financiamiento_ID;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Numero_Garantia;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Clave_Padron;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Estatus;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fecha_Multa;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Unidades;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Importe;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Descuento_Importe;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Monto_Importe;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Honorarios;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Multas;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Moratorios;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Recargos;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Ajuste_Tarifario;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Total;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Referencia;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Observaciones;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Usuario_Creo;
                                Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fecha_Creo + ")";
                                Mi_SQL += " VALUES ('" + Orden_Pago.P_Concepto_Orden_Pago_ID + "'";
                                Mi_SQL += ", '" + Orden_Pago.P_No_Orden_Pago + "'";
                                Mi_SQL += ", '" + Orden_Pago.P_Año + "'";
                                Mi_SQL += ", '" + Orden_Pago.P_Folio + "'";
                                Mi_SQL += ", '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_SubConcepto_Ing_ID].ToString() + "'";
                                Mi_SQL += ", '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Tipo_Pago_ID].ToString() + "'";
                                Mi_SQL += ", '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Garantia_Proceso_ID].ToString() + "'";
                                Mi_SQL += ", '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Banco_ID].ToString() + "'";
                                Mi_SQL += ", '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fuente_Financiamiento_ID].ToString() + "'";
                                Mi_SQL += ", '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Numero_Garantia].ToString() + "'";
                                Mi_SQL += ", '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Clave_Padron].ToString() + "'";
                                Mi_SQL += ", '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Estatus].ToString() + "'";
                                Mi_SQL += ", '" + Convert.ToDateTime(Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fecha_Multa]).ToString("dd-MM-yyyy") + "'";
                                Mi_SQL += ", " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Unidades].ToString();
                                Mi_SQL += ", " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Importe].ToString();
                                Mi_SQL += ", " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Descuento_Importe].ToString();
                                Mi_SQL += ", " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Monto_Importe].ToString();
                                Mi_SQL += ", " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Honorarios].ToString();
                                Mi_SQL += ", " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Multas].ToString();
                                Mi_SQL += ", " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Moratorios].ToString();
                                Mi_SQL += ", " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Recargos].ToString();
                                Mi_SQL += ", " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Ajuste_Tarifario].ToString();
                                Mi_SQL += ", " + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Total].ToString();
                                Mi_SQL += ", '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Referencia].ToString() + "'";
                                Mi_SQL += ", '" + Dr_Conceptos_Orden_Pago[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Observaciones].ToString() + "'";
                                Mi_SQL += ", '" + Orden_Pago.P_Usuario + "'";
                                Mi_SQL += ", SYSDATE";
                                Mi_SQL += ")";
                            }
                            Cmmd.CommandText = Mi_SQL;
                            Cmmd.ExecuteNonQuery();
                        }
                    }
                }

                if (!Varios_Conceptos 
                    && Orden_Pago.P_Actualiza_Conceptos)
                {
                    Mi_SQL = "SELECT " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Concepto_Orden_Pago_ID;
                    Mi_SQL += " FROM " + Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago;
                    //Mi_SQL += " WHERE " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_No_Orden_Pago + " = '" + Orden_Pago.P_No_Orden_Pago + "'";
                    //Mi_SQL += " AND " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Año + " = " + Orden_Pago.P_Año;
                    Mi_SQL += " WHERE " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Concepto_Orden_Pago_ID + " = '" + Orden_Pago.P_Concepto_Orden_Pago_ID + "'";
                    Cmmd.CommandText = Mi_SQL;
                    OracleDataReader DReader = Cmmd.ExecuteReader();

                    if (DReader.Read())
                    {
                        Mi_SQL = "UPDATE " + Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago;
                        Mi_SQL += " SET ";
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_SubConcepto_Ing_ID + " = '" + Orden_Pago.P_SubConcepto_Ing_ID + "', ";
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Tipo_Pago_ID + " = '" + Orden_Pago.P_Tipo_Pago_ID + "', ";
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Garantia_Proceso_ID + " = '" + Orden_Pago.P_Garantia_Proceso_ID + "', ";
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Banco_ID + " = '" + Orden_Pago.P_Banco_ID + "', ";
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fuente_Financiamiento_ID + " = '" + Orden_Pago.P_Fuente_Financiamiento_ID + "', ";
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Numero_Garantia + " = '" + Orden_Pago.P_Numero_Garantia + "', ";
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Clave_Padron + " = '" + Orden_Pago.P_Clave_Padron + "', ";
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fecha_Multa + " = '" + Orden_Pago.P_Fecha_Multa.ToString("dd-MM-yyyy") + "', ";
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Estatus + " = '" + Orden_Pago.P_Estatus + "', ";
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Unidades + " = " + Orden_Pago.P_Unidades + ", ";
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Importe + " = " + Orden_Pago.P_Importe + ", ";
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Descuento_Importe + " = " + Orden_Pago.P_Descuento_Importe + ", ";
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Monto_Importe + " = " + Orden_Pago.P_Monto_Importe + ", ";
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Honorarios + " = " + Orden_Pago.P_Honorarios + ", ";
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Multas + " = " + Orden_Pago.P_Multas + ", ";
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Moratorios + " = " + Orden_Pago.P_Moratorios + ", ";
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Recargos + " = " + Orden_Pago.P_Recargos + ", ";
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Ajuste_Tarifario + " = " + Orden_Pago.P_Ajuste_Tarifario + ", ";
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Total + " = " + Orden_Pago.P_Total + ", ";
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Referencia + " = '" + Orden_Pago.P_Referencia + "', ";
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Observaciones + " = '" + Orden_Pago.P_Observaciones + "', ";
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Usuario_Modifico + " = '" + Orden_Pago.P_Usuario + "', ";
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_SQL += " WHERE " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_No_Orden_Pago + " = '" + Orden_Pago.P_No_Orden_Pago + "'";
                        Mi_SQL += " AND " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Año + " = " + Orden_Pago.P_Año;
                        Mi_SQL += " AND " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Concepto_Orden_Pago_ID + " = '" + Orden_Pago.P_Concepto_Orden_Pago_ID + "'";
                        Mi_SQL += " AND " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Folio + " = '" + Orden_Pago.P_Folio + "'";
                    }
                    else
                    {
                        Orden_Pago.P_Concepto_Orden_Pago_ID = Obtener_ID_Consecutivo(ref Cmmd, Ope_Ing_Conceptos_Ordenes_Pago.Campo_Concepto_Orden_Pago_ID, Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago, "", 10);
                        Mi_SQL = "INSERT INTO " + Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago;
                        Mi_SQL += " (" + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Concepto_Orden_Pago_ID;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_No_Orden_Pago;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Año;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Folio;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_SubConcepto_Ing_ID;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Tipo_Pago_ID;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Garantia_Proceso_ID;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Banco_ID;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fuente_Financiamiento_ID;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Numero_Garantia;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Clave_Padron;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Estatus;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fecha_Multa;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Unidades;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Importe;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Descuento_Importe;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Monto_Importe;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Honorarios;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Multas;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Moratorios;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Recargos;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Ajuste_Tarifario;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Total;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Referencia;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Observaciones;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Usuario_Creo;
                        Mi_SQL += ", " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fecha_Creo + ")";
                        Mi_SQL += " VALUES ('" + Orden_Pago.P_Concepto_Orden_Pago_ID + "'";
                        Mi_SQL += ", '" + Orden_Pago.P_No_Orden_Pago + "'";
                        Mi_SQL += ", '" + Orden_Pago.P_Año + "'";
                        Mi_SQL += ", '" + Orden_Pago.P_Folio + "'";
                        Mi_SQL += ", '" + Orden_Pago.P_SubConcepto_Ing_ID + "'";
                        Mi_SQL += ", '" + Orden_Pago.P_Tipo_Pago_ID + "'";
                        Mi_SQL += ", '" + Orden_Pago.P_Garantia_Proceso_ID + "'";
                        Mi_SQL += ", '" + Orden_Pago.P_Banco_ID + "'";
                        Mi_SQL += ", '" + Orden_Pago.P_Fuente_Financiamiento_ID + "'";
                        Mi_SQL += ", '" + Orden_Pago.P_Numero_Garantia + "'";
                        Mi_SQL += ", '" + Orden_Pago.P_Clave_Padron + "'";
                        Mi_SQL += ", '" + Orden_Pago.P_Estatus + "'";
                        Mi_SQL += ", '" + Orden_Pago.P_Fecha_Multa.ToString("dd-MM-yyyy") + "'";
                        Mi_SQL += ", " + Orden_Pago.P_Unidades;
                        Mi_SQL += ", " + Orden_Pago.P_Importe;
                        Mi_SQL += ", " + Orden_Pago.P_Descuento_Importe;
                        Mi_SQL += ", " + Orden_Pago.P_Monto_Importe;
                        Mi_SQL += ", " + Orden_Pago.P_Honorarios;
                        Mi_SQL += ", " + Orden_Pago.P_Multas;
                        Mi_SQL += ", " + Orden_Pago.P_Moratorios;
                        Mi_SQL += ", " + Orden_Pago.P_Recargos;
                        Mi_SQL += ", " + Orden_Pago.P_Ajuste_Tarifario;
                        Mi_SQL += ", " + Orden_Pago.P_Total;
                        Mi_SQL += ", '" + Orden_Pago.P_Referencia + "'";
                        Mi_SQL += ", '" + Orden_Pago.P_Observaciones + "'";
                        Mi_SQL += ", '" + Orden_Pago.P_Usuario + "'";
                        Mi_SQL += ", SYSDATE";
                        Mi_SQL += ")";
                    }
                    Cmmd.CommandText = Mi_SQL;
                    Cmmd.ExecuteNonQuery();
                }

                if (Orden_Pago.P_Cmmd == null)
                {
                    Trans.Commit();
                }

                Estatus_Modificar = true;
            }
            catch (OracleException Ex)
            {
                if (Orden_Pago.P_Cmmd == null)
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
                    Mensaje = "Error al intentar modificar un Registro de Colonias. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Orden_Pago.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }

            return Estatus_Modificar;
        }
        #endregion

        #region Eliminaciones

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Eliminar_Orden_Pago
        ///DESCRIPCIÓN          : Elimina de la Base de Datos los registros de Orden de Pago
        ///PARAMETROS           : Orden_Pago, instancia de Cls_Ope_Ing_Ordenes_Pago_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 07/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Eliminar_Orden_Pago(Cls_Ope_Ing_Ordenes_Pago_Negocio Orden_Pago)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Estatus_Eliminar = false;

            if (Orden_Pago.P_Cmmd != null)
            {
                Cmmd = Orden_Pago.P_Cmmd;
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
                String Mi_SQL = "";
                try
                {
                    Mi_SQL = "DELETE FROM " + Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago;
                    Mi_SQL += " WHERE " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_No_Orden_Pago + " = '" + Orden_Pago.P_No_Orden_Pago + "'";
                    Mi_SQL += " AND " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Año + " = '" + Orden_Pago.P_Año + "'";
                    Mi_SQL += " AND " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Folio + " = '" + Orden_Pago.P_Folio + "'";
                    Cmmd.CommandText = Mi_SQL;
                    Cmmd.ExecuteNonQuery();

                    Mi_SQL = "DELETE FROM " + Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago;
                    Mi_SQL += " WHERE " + Ope_Ing_Ordenes_Pago.Campo_No_Orden_Pago + " = '" + Orden_Pago.P_No_Orden_Pago + "'";
                    Mi_SQL += " AND " + Ope_Ing_Ordenes_Pago.Campo_Año + " = '" + Orden_Pago.P_Año + "'";
                    Mi_SQL += " AND " + Ope_Ing_Ordenes_Pago.Campo_Folio + " = '" + Orden_Pago.P_Folio + "'";
                    Cmmd.CommandText = Mi_SQL;
                    Cmmd.ExecuteNonQuery();
                }
                catch (OracleException Ex)
                {
                    if (Ex.Code == 547 || Ex.Code == 2292)
                    {
                        Mi_SQL = "UPDATE " + Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago;
                        Mi_SQL += " SET " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Estatus + " = 'CANCELADA'";
                        Mi_SQL += " WHERE " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_No_Orden_Pago + " = '" + Orden_Pago.P_No_Orden_Pago + "'";
                        Mi_SQL += " AND " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Año + " = '" + Orden_Pago.P_Año + "'";
                        Mi_SQL += " AND " + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Folio + " = '" + Orden_Pago.P_Folio + "'";
                        Cmmd.CommandText = Mi_SQL;
                        Cmmd.ExecuteNonQuery();

                        Mi_SQL = "UPDATE " + Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago;
                        Mi_SQL += " SET " + Ope_Ing_Ordenes_Pago.Campo_Estatus + " = 'CANCELADA'";
                        Mi_SQL += " WHERE " + Ope_Ing_Ordenes_Pago.Campo_No_Orden_Pago + " = '" + Orden_Pago.P_No_Orden_Pago + "'";
                        Mi_SQL += " AND " + Ope_Ing_Ordenes_Pago.Campo_Año + " = '" + Orden_Pago.P_Año + "'";
                        Mi_SQL += " AND " + Ope_Ing_Ordenes_Pago.Campo_Folio + " = '" + Orden_Pago.P_Folio + "'";
                        Cmmd.CommandText = Mi_SQL;
                        Cmmd.ExecuteNonQuery();
                    }
                    else
                    {
                        throw new Exception();
                    }
                }

                if (Orden_Pago.P_Cmmd == null)
                {
                    Trans.Commit();
                }

                Estatus_Eliminar = true;
            }
            catch (OracleException Ex)
            {
                if (Ex.Code == 547)
                {
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]";
                }
                else
                {
                    Mensaje = "Error al intentar eliminar el registro de Tipos de Pagos. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al intentar eliminar el registro de Tipos de Pagos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Orden_Pago.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }

            return Estatus_Eliminar;
        }
        #endregion

        #region Consultas

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Ordenes_Pago
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos los datos de la Orden de Pago
        ///PARAMETROS           : Orden_Pago, instancia de Cls_Ope_Ing_Ordenes_Pago_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 07/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Ordenes_Pago(Cls_Ope_Ing_Ordenes_Pago_Negocio Orden_Pago)
        {
            DataTable Dt_Tipos_Pagos = new DataTable();
            String Mi_SQL;
            String Mi_SQL_Campos_Foraneos = "";
            try
            {
                if (Orden_Pago.P_Incluir_Campos_Foraneos)
                {
                    Mi_SQL_Campos_Foraneos += "(SELECT CONTRIBUYENTES." + Cat_Ing_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || CONTRIBUYENTES." + Cat_Ing_Contribuyentes.Campo_Apellido_Materno + " || ' ' || CONTRIBUYENTES." + Cat_Ing_Contribuyentes.Campo_Nombre + " FROM " + Cat_Ing_Contribuyentes.Tabla_Cat_Ing_Contribuyentes + " CONTRIBUYENTES WHERE CONTRIBUYENTES." + Cat_Ing_Contribuyentes.Campo_Contribuyente_ID + " = " + Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago + "." + Ope_Ing_Ordenes_Pago.Campo_Contribuyente_ID + ") AS NOMBRE_CONTRIBUYENTE, ";
                    Mi_SQL_Campos_Foraneos += "(SELECT DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS WHERE DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID + " = " + Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago + "." + Ope_Ing_Ordenes_Pago.Campo_Dependencia_ID + ") AS NOMBRE_DEPENDENCIA, ";
                }
                if (Orden_Pago.P_Campos_Dinamicos != null && Orden_Pago.P_Campos_Dinamicos != "")
                {
                    Mi_SQL = "SELECT " + Mi_SQL_Campos_Foraneos + Orden_Pago.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL = "SELECT " + Mi_SQL_Campos_Foraneos;
                    Mi_SQL += Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago + "." + Ope_Ing_Ordenes_Pago.Campo_No_Orden_Pago + ", ";
                    Mi_SQL += Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago + "." + Ope_Ing_Ordenes_Pago.Campo_Año + ", ";
                    Mi_SQL += Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago + "." + Ope_Ing_Ordenes_Pago.Campo_Contribuyente_ID + ", ";
                    Mi_SQL += Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago + "." + Ope_Ing_Ordenes_Pago.Campo_Dependencia_ID + ", ";
                    Mi_SQL += Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago + "." + Ope_Ing_Ordenes_Pago.Campo_Proteccion + ", ";
                    Mi_SQL += Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago + "." + Ope_Ing_Ordenes_Pago.Campo_Estatus + ", ";
                    Mi_SQL += Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago + "." + Ope_Ing_Ordenes_Pago.Campo_Folio + ", ";
                    Mi_SQL += Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago + "." + Ope_Ing_Ordenes_Pago.Campo_Total + ", ";
                    if (Orden_Pago.P_Incluir_Campos_Dinamicos != null && Orden_Pago.P_Incluir_Campos_Dinamicos != "")
                    {
                        Mi_SQL += Orden_Pago.P_Incluir_Campos_Dinamicos;
                    }
                }
                if (Orden_Pago.P_Incluir_Campos_Vitacora)
                {
                    Mi_SQL += Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago + "." + Ope_Ing_Ordenes_Pago.Campo_Usuario_Creo + ", ";
                    Mi_SQL += Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago + "." + Ope_Ing_Ordenes_Pago.Campo_Fecha_Creo + ", ";
                    Mi_SQL += Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago + "." + Ope_Ing_Ordenes_Pago.Campo_Usuario_Modifico + ", ";
                    Mi_SQL += Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago + "." + Ope_Ing_Ordenes_Pago.Campo_Fecha_Modifico + ", ";
                }
                if (Mi_SQL.EndsWith(", "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 2);
                }
                Mi_SQL += " FROM " + Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago;
                if (Orden_Pago.P_Unir_Tablas != null && Orden_Pago.P_Unir_Tablas != "")
                {
                    Mi_SQL += ", " + Orden_Pago.P_Unir_Tablas;
                }
                else
                {
                    if (Orden_Pago.P_Join != null && Orden_Pago.P_Join != "")
                    {
                        Mi_SQL += " " + Orden_Pago.P_Join;
                    }
                }
                if (Orden_Pago.P_Filtros_Dinamicos != null && Orden_Pago.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL += " WHERE " + Orden_Pago.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL += " WHERE ";
                    if (Orden_Pago.P_No_Orden_Pago != null && Orden_Pago.P_No_Orden_Pago != "")
                    {
                        Mi_SQL += Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago + "." + Ope_Ing_Ordenes_Pago.Campo_No_Orden_Pago + " = '" + Orden_Pago.P_No_Orden_Pago + "' AND ";
                    }
                    if (Orden_Pago.P_Año > 0)
                    {
                        Mi_SQL += Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago + "." + Ope_Ing_Ordenes_Pago.Campo_Año + " = " + Orden_Pago.P_Año + " AND ";
                    }
                    if (Orden_Pago.P_Contribuyente_ID != null && Orden_Pago.P_Contribuyente_ID != "")
                    {
                        Mi_SQL += Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago + "." + Ope_Ing_Ordenes_Pago.Campo_Contribuyente_ID + " = '" + Orden_Pago.P_Contribuyente_ID + "' AND ";
                    }
                    if (Orden_Pago.P_Dependencia_ID != null && Orden_Pago.P_Dependencia_ID != "")
                    {
                        Mi_SQL += Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago + "." + Ope_Ing_Ordenes_Pago.Campo_Dependencia_ID + " = '" + Orden_Pago.P_Dependencia_ID + "' AND ";
                    }
                    if (Orden_Pago.P_Folio != null && Orden_Pago.P_Folio != "")
                    {
                        Mi_SQL += Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago + "." + Ope_Ing_Ordenes_Pago.Campo_Folio + Validar_Operador_Comparacion(Orden_Pago.P_Folio) + " AND ";
                    }
                    if (Orden_Pago.P_Estatus != null && Orden_Pago.P_Estatus != "")
                    {
                        Mi_SQL += Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago + "." + Ope_Ing_Ordenes_Pago.Campo_Estatus + Validar_Operador_Comparacion(Orden_Pago.P_Estatus) + " AND ";
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
                if (Orden_Pago.P_Agrupar_Dinamico != null && Orden_Pago.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " GROUP BY " + Orden_Pago.P_Agrupar_Dinamico;
                }
                if (Orden_Pago.P_Ordenar_Dinamico != null && Orden_Pago.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Orden_Pago.P_Ordenar_Dinamico;
                }
                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null)
                {
                    Dt_Tipos_Pagos = dataSet.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de la Cuentas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Tipos_Pagos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Conceptos_Ordenes_Pago
        ///DESCRIPCIÓN          : Consulta los Conceptos de las Órdenes de Pago
        ///PARAMETROS           : Orden_Pago, instancia de Cls_Ope_Ing_Ordenes_Pago_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 06/Agosto/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Conceptos_Ordenes_Pago(Cls_Ope_Ing_Ordenes_Pago_Negocio Orden_Pago)
        {
            DataTable Dt_Tipos_Pagos = new DataTable();
            String Mi_SQL;
            String Mi_SQL_Campos_Foraneos = "";
            try
            {
                if (Orden_Pago.P_Incluir_Campos_Foraneos)
                {
                    Mi_SQL_Campos_Foraneos += "(SELECT SUBCONCEPTOS." + Cat_Psp_SubConcepto_Ing.Campo_Clave + " FROM " + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + " SUBCONCEPTOS WHERE SUBCONCEPTOS." + Cat_Psp_SubConcepto_Ing.Campo_SubConcepto_Ing_ID + " = " + Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_SubConcepto_Ing_ID + ") AS CLAVE_INGRESO, ";
                    Mi_SQL_Campos_Foraneos += "(SELECT SUBCONCEPTOS." + Cat_Psp_SubConcepto_Ing.Campo_Descripcion + " FROM " + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + " SUBCONCEPTOS WHERE SUBCONCEPTOS." + Cat_Psp_SubConcepto_Ing.Campo_SubConcepto_Ing_ID + " = " + Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_SubConcepto_Ing_ID + ") AS DESCRIPCION_CLAVE_INGRESO, ";
                    Mi_SQL_Campos_Foraneos += "(SELECT SUBCONCEPTOS." + Cat_Psp_SubConcepto_Ing.Campo_Fundamento + " FROM " + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + " SUBCONCEPTOS WHERE SUBCONCEPTOS." + Cat_Psp_SubConcepto_Ing.Campo_SubConcepto_Ing_ID + " = " + Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_SubConcepto_Ing_ID + ") AS FUNDAMENTO, ";
                }
                if (Orden_Pago.P_Campos_Dinamicos != null && Orden_Pago.P_Campos_Dinamicos != "")
                {
                    Mi_SQL = "SELECT " + Mi_SQL_Campos_Foraneos + Orden_Pago.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL = "SELECT " + Mi_SQL_Campos_Foraneos;
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Concepto_Orden_Pago_ID + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_No_Orden_Pago + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Año + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Folio + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_SubConcepto_Ing_ID + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Tipo_Pago_ID + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Garantia_Proceso_ID + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Banco_ID + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fuente_Financiamiento_ID + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Numero_Garantia + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Clave_Padron + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Estatus + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fecha_Multa + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Unidades + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Importe + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Descuento_Importe + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Monto_Importe + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Honorarios + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Multas + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Moratorios + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Recargos + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Ajuste_Tarifario + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Total + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Referencia + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Observaciones + ", ";
                }
                if (Orden_Pago.P_Incluir_Campos_Vitacora)
                {
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Usuario_Creo + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fecha_Creo + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Usuario_Modifico + ", ";
                    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fecha_Modifico + ", ";
                }
                if (Mi_SQL.EndsWith(", "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 2);
                }
                Mi_SQL += " FROM " + Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago;
                if (Orden_Pago.P_Unir_Tablas != null && Orden_Pago.P_Unir_Tablas != "")
                {
                    Mi_SQL += ", " + Orden_Pago.P_Unir_Tablas;
                }
                else
                {
                    if (Orden_Pago.P_Join != null && Orden_Pago.P_Join != "")
                    {
                        Mi_SQL += " " + Orden_Pago.P_Join;
                    }
                }
                if (Orden_Pago.P_Filtros_Dinamicos != null && Orden_Pago.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL += " WHERE " + Orden_Pago.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL += " WHERE ";
                    if (Orden_Pago.P_Concepto_Orden_Pago_ID != null && Orden_Pago.P_Concepto_Orden_Pago_ID != "")
                    {
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Concepto_Orden_Pago_ID + " = '" + Orden_Pago.P_Concepto_Orden_Pago_ID + "' AND ";
                    }
                    if (Orden_Pago.P_No_Orden_Pago != null && Orden_Pago.P_No_Orden_Pago != "")
                    {
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_No_Orden_Pago + Validar_Operador_Comparacion(Orden_Pago.P_No_Orden_Pago) + " AND ";
                    }
                    if (Orden_Pago.P_Año > 0)
                    {
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Año + " = " + Orden_Pago.P_Año + " AND ";
                    }
                    if (Orden_Pago.P_Folio != null && Orden_Pago.P_Folio != "")
                    {
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Folio + Validar_Operador_Comparacion(Orden_Pago.P_Folio) + " AND ";
                    }
                    if (Orden_Pago.P_SubConcepto_Ing_ID != null && Orden_Pago.P_SubConcepto_Ing_ID != "")
                    {
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_SubConcepto_Ing_ID + " = '" + Orden_Pago.P_SubConcepto_Ing_ID + "' AND ";
                    }
                    if (Orden_Pago.P_Tipo_Pago_ID != null && Orden_Pago.P_Tipo_Pago_ID != "")
                    {
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Tipo_Pago_ID + " = '" + Orden_Pago.P_Tipo_Pago_ID + "' AND ";
                    }
                    if (Orden_Pago.P_Garantia_Proceso_ID != null && Orden_Pago.P_Garantia_Proceso_ID != "")
                    {
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Garantia_Proceso_ID + " = '" + Orden_Pago.P_Garantia_Proceso_ID + "' AND ";
                    }
                    if (Orden_Pago.P_Banco_ID != null && Orden_Pago.P_Banco_ID != "")
                    {
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Banco_ID + " = '" + Orden_Pago.P_Banco_ID + "' AND ";
                    }
                    if (Orden_Pago.P_Fuente_Financiamiento_ID != null && Orden_Pago.P_Fuente_Financiamiento_ID != "")
                    {
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fuente_Financiamiento_ID + " = '" + Orden_Pago.P_Fuente_Financiamiento_ID + "' AND ";
                    }
                    if (Orden_Pago.P_Numero_Garantia != null && Orden_Pago.P_Numero_Garantia != "")
                    {
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Numero_Garantia + Validar_Operador_Comparacion(Orden_Pago.P_Numero_Garantia) + " AND ";
                    }
                    if (Orden_Pago.P_Clave_Padron != null && Orden_Pago.P_Clave_Padron != "")
                    {
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Clave_Padron + Validar_Operador_Comparacion(Orden_Pago.P_Clave_Padron) + " AND ";
                    }
                    //if (Orden_Pago.P_Folio != null && Orden_Pago.P_Folio != "")
                    //{
                    //    String No_Orden_Pago = Convert.ToInt64(Orden_Pago.P_Folio.Trim().Substring(7)).ToString("0000000000");
                    //    String Año = Orden_Pago.P_Folio.Trim().Substring(3, 4);
                    //    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_No_Orden_Pago + " = '" + No_Orden_Pago + "' AND ";
                    //    Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Año + " = " + Año + " AND ";
                    //}
                    if (Orden_Pago.P_Fecha_Multa > DateTime.MinValue)
                    {
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fecha_Multa + " >= '" + Orden_Pago.P_Fecha_Multa.ToString("dd-MM-yyyy") + "' AND ";
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fecha_Multa + " < '" + Orden_Pago.P_Fecha_Multa.AddDays(1).ToString("dd-MM-yyyy") + "' AND ";
                    }
                    if (Orden_Pago.P_Estatus != null && Orden_Pago.P_Estatus != "")
                    {
                        Mi_SQL += Ope_Ing_Conceptos_Ordenes_Pago.Tabla_Ope_Ing_Conceptos_Ordenes_Pago + "." + Ope_Ing_Conceptos_Ordenes_Pago.Campo_Estatus + Validar_Operador_Comparacion(Orden_Pago.P_Estatus) + " AND ";
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
                if (Orden_Pago.P_Agrupar_Dinamico != null && Orden_Pago.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " GROUP BY " + Orden_Pago.P_Agrupar_Dinamico;
                }
                if (Orden_Pago.P_Ordenar_Dinamico != null && Orden_Pago.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Orden_Pago.P_Ordenar_Dinamico;
                }
                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null)
                {
                    Dt_Tipos_Pagos = dataSet.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de la Cuentas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Tipos_Pagos;
        }
        #endregion

        #region Consulta de ID Consecutivo

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_ID_Consecutivo
        ///DESCRIPCIÓN          : Método para generar un nuevo Id del Catálogo
        ///PARAMETROS           : Cmmd, Referencia del Comando de la transacción avierta previamente por el método que lo crea.
        ///                       Campos, Columnas de la tabla en la base de datos a ser conusultadas
        ///                       Longitud_ID, tamaño final con formato del Nuevo_Valor_ID a generar
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 07/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static String Obtener_ID_Consecutivo(ref OracleCommand Cmmd, String Campos, String Tabla, String Condiciones, Int32 Longitud_ID)
        {
            String Nuevo_Valor_ID = Convertir_A_Formato_ID(1, Longitud_ID); ;
            try
            {
                String Mi_SQL = "SELECT MAX(" + Campos + ") FROM " + Tabla;
                if (Condiciones != null && Condiciones != "")
                {
                    Mi_SQL += " WHERE " + Condiciones;
                }
                Cmmd.CommandText = Mi_SQL;
                Object Obj_Temp = Cmmd.ExecuteScalar();
                if (Obj_Temp != null)
                {
                    if (Obj_Temp.ToString() != "")
                    {
                        Nuevo_Valor_ID = Convertir_A_Formato_ID((Convert.ToInt32(Obj_Temp.ToString()) + 1), Longitud_ID);
                    }
                }
            }
            catch (OracleException Ex)
            {
                new Exception(Ex.Message);
            }
            return Nuevo_Valor_ID;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Convertir_A_Formato_ID
        ///DESCRIPCIÓN          : Método para dar el formato y tamaño a la cadena del nuevo Id del Catálogo
        ///PARAMETROS           : Dato_ID, valor que va a ser formateado
        ///                       Longitud_ID, valore que se usará para dar la longitud final del nuevo Id del Catálogo
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 07/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID)
        {
            String Cadena_Formateada = "";
            String Dato = "" + Dato_ID;
            for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++)
            {
                Cadena_Formateada = Cadena_Formateada + "0";
            }
            Cadena_Formateada = Cadena_Formateada + Dato;
            return Cadena_Formateada;
        }

        #endregion

        #region Validaciones
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
               || Filtro.Trim().ToUpper().StartsWith("BETWEEN ")
               || Filtro.Trim().ToUpper().StartsWith("LIKE ")
               || Filtro.Trim().ToUpper().StartsWith("IN ")
               || Filtro.Trim().ToUpper().StartsWith("NOT IN "))
            {
                Cadena_Validada = " " + Filtro + " ";
            }
            else
            {
                if (Filtro.Trim().ToUpper().StartsWith("NULL ")
                    || Filtro.Trim().ToUpper().StartsWith("NOT NULL "))
                {
                    Cadena_Validada = " IS " + Filtro + " ";
                }
                else
                {
                    if (Filtro.Trim().StartsWith("(") && Filtro.Trim().EndsWith(")"))
                    {
                        Cadena_Validada = " = " + Filtro;
                    }
                    else
                    {
                        Cadena_Validada = " = '" + Filtro + "' ";
                    }
                }
            }
            return Cadena_Validada;
        }
        #endregion
    }
}