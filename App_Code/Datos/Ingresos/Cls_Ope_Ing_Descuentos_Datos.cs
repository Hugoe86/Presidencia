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
using Presidencia.Cls_Ope_Ing_Descuentos.Negocio;
using Presidencia.Catalogo_Claves_Ingreso.Negocio;
using Presidencia.Cls_Cat_Ing_Descuentos_Responsable.Negocio;


namespace Presidencia.Cls_Ope_Ing_Descuentos.Datos
{
    #region Metodos
    public class Cls_Ope_Ing_Descuentos_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Descuentos
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos un nuevo Descuentos de Traslado
        ///PARAMETROS           : Descuentos, instancia de Cls_Ope_Ing_Descuentos_Negocio
        ///CREO                 : Antonnio Salvador Benavides Guardado
        ///FECHA_CREO           : 08/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Alta_Descuentos(Cls_Ope_Ing_Descuentos_Negocio Descuentos)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Estatus_Alta = false;
            Boolean Varios_Conceptos = false;

            if (Descuentos.P_Cmmd != null)
            {
                Cmmd = Descuentos.P_Cmmd;
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
                if (Descuentos.P_Dt_Descuentos != null)
                {
                    if (Descuentos.P_Dt_Descuentos.Rows.Count > 0)
                    {
                        Varios_Conceptos = true;
                        foreach (DataRow Dr_Descuentos in Descuentos.P_Dt_Descuentos.Rows)
                        {
                            Descuentos.P_No_Descuento = Obtener_ID_Consecutivo(ref Cmmd, Ope_Ing_Descuentos.Campo_No_Descuento, 10);
                            String Mi_SQL = "INSERT INTO " + Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos;
                            Mi_SQL += " (" + Ope_Ing_Descuentos.Campo_No_Descuento;
                            Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Concepto_Orden_Pago_ID;
                            Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Referencia;
                            Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Unidades;
                            Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Monto_Honorarios;
                            Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Monto_Multas;
                            Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Monto_Moratorios;
                            Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Monto_Recargos;
                            Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Descuento_Honorarios;
                            Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Descuento_Multas;
                            Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Descuento_Moratorios;
                            Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Descuento_Recargos;
                            Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Total_Pagar;
                            Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Estatus;
                            Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Fecha_Descuento;
                            Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Fecha_Vencimiento;
                            Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Fundamento_Legal;
                            Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Observaciones;
                            Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Realizo;
                            Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Usuario_Creo;
                            Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Fecha_Creo + ")";
                            Mi_SQL += " VALUES ('" + Descuentos.P_No_Descuento + "'";
                            Mi_SQL += ", '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Concepto_Orden_Pago_ID].ToString() + "'";
                            Mi_SQL += ", '" + Descuentos.P_Referencia + "'";
                            Mi_SQL += ", '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Unidades].ToString() + "'";
                            Mi_SQL += ", '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Monto_Honorarios].ToString() + "'";
                            Mi_SQL += ", '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Monto_Multas].ToString() + "'";
                            Mi_SQL += ", '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Monto_Moratorios].ToString() + "'";
                            Mi_SQL += ", '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Monto_Recargos].ToString() + "'";
                            Mi_SQL += ", '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Descuento_Honorarios].ToString() + "'";
                            Mi_SQL += ", '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Descuento_Multas].ToString() + "'";
                            Mi_SQL += ", '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Descuento_Moratorios].ToString() + "'";
                            Mi_SQL += ", '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Descuento_Recargos].ToString() + "'";
                            Mi_SQL += ", '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Total_Pagar].ToString() + "'";
                            Mi_SQL += ", '" + Dr_Descuentos["ESTATUS_DESCUENTO"].ToString() + "'";
                            Mi_SQL += ", '" + Convert.ToDateTime(Dr_Descuentos[Ope_Ing_Descuentos.Campo_Fecha_Descuento]).ToString("dd-MM-yyyy") + "'";
                            Mi_SQL += ", '" + Convert.ToDateTime(Dr_Descuentos[Ope_Ing_Descuentos.Campo_Fecha_Vencimiento]).ToString("dd-MM-yyyy") + "'";
                            Mi_SQL += ", '" + Dr_Descuentos["FUNDAMENTO_LEGAL_DESCUENTO"].ToString() + "'";
                            Mi_SQL += ", '" + Dr_Descuentos["OBSERVACIONES_DESCUENTO"].ToString() + "'";
                            Mi_SQL += ", '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Realizo].ToString() + "'";
                            Mi_SQL += ", '" + Descuentos.P_Usuario + "'";
                            Mi_SQL += ", SYSDATE";
                            Mi_SQL += ")";
                            Cmmd.CommandText = Mi_SQL;
                            Cmmd.ExecuteNonQuery();
                        }
                    }
                }

                if (!Varios_Conceptos)
                {
                    Descuentos.P_No_Descuento = Obtener_ID_Consecutivo(ref Cmmd, Ope_Ing_Descuentos.Campo_No_Descuento, 10);
                    String Mi_SQL = "INSERT INTO " + Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos;
                    Mi_SQL += " (" + Ope_Ing_Descuentos.Campo_No_Descuento;
                    Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Concepto_Orden_Pago_ID;
                    Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Referencia;
                    Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Unidades;
                    Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Monto_Honorarios;
                    Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Monto_Multas;
                    Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Monto_Moratorios;
                    Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Monto_Recargos;
                    Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Descuento_Honorarios;
                    Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Descuento_Multas;
                    Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Descuento_Moratorios;
                    Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Descuento_Recargos;
                    Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Total_Pagar;
                    Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Estatus;
                    Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Fecha_Descuento;
                    Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Fecha_Vencimiento;
                    Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Fundamento_Legal;
                    Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Observaciones;
                    Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Realizo;
                    Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Usuario_Creo;
                    Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Fecha_Creo + ")";
                    Mi_SQL += " VALUES ('" + Descuentos.P_No_Descuento + "'";
                    Mi_SQL += ", '" + Descuentos.P_Concepto_Orden_Pago_ID + "'";
                    Mi_SQL += ", '" + Descuentos.P_Referencia + "'";
                    Mi_SQL += ", " + Descuentos.P_Unidades;
                    Mi_SQL += ", " + Descuentos.P_Monto_Honorarios;
                    Mi_SQL += ", " + Descuentos.P_Monto_Multas;
                    Mi_SQL += ", " + Descuentos.P_Monto_Moratorios;
                    Mi_SQL += ", " + Descuentos.P_Monto_Recargos;
                    Mi_SQL += ", " + Descuentos.P_Descuento_Honorarios;
                    Mi_SQL += ", " + Descuentos.P_Descuento_Multas;
                    Mi_SQL += ", " + Descuentos.P_Descuento_Moratorios;
                    Mi_SQL += ", " + Descuentos.P_Descuento_Recargos;
                    Mi_SQL += ", " + Descuentos.P_Total_Pagar;
                    Mi_SQL += ", '" + Descuentos.P_Estatus + "'";
                    Mi_SQL += ", '" + Descuentos.P_Fecha_Descuento.ToString("dd-MM-yyyy") + "'";
                    Mi_SQL += ", '" + Descuentos.P_Fecha_Vencimiento.ToString("dd-MM-yyyy") + "'";
                    Mi_SQL += ", '" + Descuentos.P_Fundamento_Legal + "'";
                    Mi_SQL += ", '" + Descuentos.P_Observaciones + "'";
                    Mi_SQL += ", '" + Descuentos.P_Realizo + "'";
                    Mi_SQL += ", '" + Descuentos.P_Usuario + "'";
                    Mi_SQL += ", SYSDATE";
                    Mi_SQL += ")";
                    Cmmd.CommandText = Mi_SQL;
                    Cmmd.ExecuteNonQuery();
                }

                if (Descuentos.P_Cmmd == null)
                {
                    Trans.Commit();
                }

                Estatus_Alta = true;
            }
            catch (OracleException Ex)
            {
                if (Descuentos.P_Cmmd == null)
                {
                    Trans.Rollback();
                }
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 2727)
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
                if (Descuentos.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }

            return Estatus_Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Descuentos
        ///DESCRIPCIÓN          : Modificar en la Base de Datos un Descuentos de Traslado
        ///PARAMETROS           : Descuentos, instancia de Cls_Ope_Ing_Descuentos_Negocio
        ///CREO                 : Antonnio Salvador Benavides Guardado
        ///FECHA_CREO           : 08/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Modificar_Descuentos(Cls_Ope_Ing_Descuentos_Negocio Descuentos)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Estatus_Modificar = false;
            Boolean Varios_Conceptos = false;

            if (Descuentos.P_Cmmd != null)
            {
                Cmmd = Descuentos.P_Cmmd;
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
                if (Descuentos.P_Dt_Descuentos != null)
                {
                    if (Descuentos.P_Dt_Descuentos.Rows.Count > 0)
                    {
                        Varios_Conceptos = true;
                        foreach (DataRow Dr_Descuentos in Descuentos.P_Dt_Descuentos.Rows)
                        {
                            Mi_SQL = "SELECT " + Ope_Ing_Descuentos.Campo_No_Descuento;
                            Mi_SQL += " FROM " + Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos;
                            Mi_SQL += " WHERE " + Ope_Ing_Descuentos.Campo_No_Descuento + " = '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_No_Descuento].ToString() + "'";
                            Cmmd.CommandText = Mi_SQL;
                            OracleDataReader DReader = Cmmd.ExecuteReader();

                            if (DReader.Read())
                            {
                                Mi_SQL = "UPDATE " + Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos;
                                Mi_SQL += " SET ";
                                Mi_SQL += Ope_Ing_Descuentos.Campo_Concepto_Orden_Pago_ID + " = '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Concepto_Orden_Pago_ID].ToString() + "', ";
                                Mi_SQL += Ope_Ing_Descuentos.Campo_Referencia + " = '" + Descuentos.P_Referencia + "', ";
                                Mi_SQL += Ope_Ing_Descuentos.Campo_Unidades + " = '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Unidades].ToString() + "', ";
                                Mi_SQL += Ope_Ing_Descuentos.Campo_Monto_Honorarios + " = '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Monto_Honorarios].ToString() + "', ";
                                Mi_SQL += Ope_Ing_Descuentos.Campo_Monto_Multas + " = '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Monto_Multas].ToString() + "', ";
                                Mi_SQL += Ope_Ing_Descuentos.Campo_Monto_Moratorios + " = '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Monto_Moratorios].ToString() + "', ";
                                Mi_SQL += Ope_Ing_Descuentos.Campo_Monto_Recargos + " = '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Monto_Recargos].ToString() + "', ";
                                Mi_SQL += Ope_Ing_Descuentos.Campo_Descuento_Honorarios + " = '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Descuento_Honorarios].ToString() + "', ";
                                Mi_SQL += Ope_Ing_Descuentos.Campo_Descuento_Multas + " = '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Descuento_Multas].ToString() + "', ";
                                Mi_SQL += Ope_Ing_Descuentos.Campo_Descuento_Moratorios + " = '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Descuento_Moratorios].ToString() + "', ";
                                Mi_SQL += Ope_Ing_Descuentos.Campo_Descuento_Recargos + " = '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Descuento_Recargos].ToString() + "', ";
                                Mi_SQL += Ope_Ing_Descuentos.Campo_Total_Pagar + " = '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Total_Pagar].ToString() + "', ";
                                Mi_SQL += Ope_Ing_Descuentos.Campo_Estatus + " = '" + Dr_Descuentos["ESTATUS_DESCUENTO"].ToString() + "', ";
                                Mi_SQL += Ope_Ing_Descuentos.Campo_Fecha_Descuento + " = '" + Convert.ToDateTime(Dr_Descuentos[Ope_Ing_Descuentos.Campo_Fecha_Descuento]).ToString("dd-MM-yyyy") + "', ";
                                Mi_SQL += Ope_Ing_Descuentos.Campo_Fecha_Vencimiento + " = '" + Convert.ToDateTime(Dr_Descuentos[Ope_Ing_Descuentos.Campo_Fecha_Vencimiento]).ToString("dd-MM-yyyy") + "', ";
                                Mi_SQL += Ope_Ing_Descuentos.Campo_Fundamento_Legal + " = '" + Dr_Descuentos["FUNDAMENTO_LEGAL_DESCUENTO"].ToString() + "', ";
                                Mi_SQL += Ope_Ing_Descuentos.Campo_Observaciones + " = '" + Dr_Descuentos["OBSERVACIONES_DESCUENTO"].ToString() + "', ";
                                Mi_SQL += Ope_Ing_Descuentos.Campo_Realizo + " = '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Realizo].ToString() + "', ";
                                Mi_SQL += Ope_Ing_Descuentos.Campo_Usuario_Modifico + " = '" + Descuentos.P_Usuario + "', ";
                                Mi_SQL += Ope_Ing_Descuentos.Campo_Fecha_Modifico + " = SYSDATE";
                                Mi_SQL += " WHERE " + Ope_Ing_Descuentos.Campo_No_Descuento + " = '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_No_Descuento].ToString() + "'";
                            }
                            else
                            {
                                Descuentos.P_No_Descuento = Obtener_ID_Consecutivo(ref Cmmd, Ope_Ing_Descuentos.Campo_No_Descuento, 10);
                                Mi_SQL = "INSERT INTO " + Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos;
                                Mi_SQL += " (" + Ope_Ing_Descuentos.Campo_No_Descuento;
                                Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Concepto_Orden_Pago_ID;
                                Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Referencia;
                                Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Unidades;
                                Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Monto_Honorarios;
                                Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Monto_Multas;
                                Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Monto_Moratorios;
                                Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Monto_Recargos;
                                Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Descuento_Honorarios;
                                Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Descuento_Multas;
                                Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Descuento_Moratorios;
                                Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Descuento_Recargos;
                                Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Total_Pagar;
                                Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Estatus;
                                Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Fecha_Descuento;
                                Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Fecha_Vencimiento;
                                Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Fundamento_Legal;
                                Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Observaciones;
                                Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Realizo;
                                Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Usuario_Creo;
                                Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Fecha_Creo + ")";
                                Mi_SQL += " VALUES ('" + Descuentos.P_No_Descuento + "'";
                                Mi_SQL += ", '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Concepto_Orden_Pago_ID].ToString() + "'";
                                Mi_SQL += ", '" + Descuentos.P_Referencia + "'";
                                Mi_SQL += ", '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Unidades].ToString() + "'";
                                Mi_SQL += ", '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Monto_Honorarios].ToString() + "'";
                                Mi_SQL += ", '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Monto_Multas].ToString() + "'";
                                Mi_SQL += ", '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Monto_Moratorios].ToString() + "'";
                                Mi_SQL += ", '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Monto_Recargos].ToString() + "'";
                                Mi_SQL += ", '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Descuento_Honorarios].ToString() + "'";
                                Mi_SQL += ", '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Descuento_Multas].ToString() + "'";
                                Mi_SQL += ", '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Descuento_Moratorios].ToString() + "'";
                                Mi_SQL += ", '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Descuento_Recargos].ToString() + "'";
                                Mi_SQL += ", '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Total_Pagar].ToString() + "'";
                                Mi_SQL += ", '" + Dr_Descuentos["ESTATUS_DESCUENTO"].ToString() + "'";
                                Mi_SQL += ", '" + Convert.ToDateTime(Dr_Descuentos[Ope_Ing_Descuentos.Campo_Fecha_Descuento]).ToString("dd-MM-yyyy") + "'";
                                Mi_SQL += ", '" + Convert.ToDateTime(Dr_Descuentos[Ope_Ing_Descuentos.Campo_Fecha_Vencimiento]).ToString("dd-MM-yyyy") + "'";
                                Mi_SQL += ", '" + Dr_Descuentos["FUNDAMENTO_LEGAL_DESCUENTO"].ToString() + "'";
                                Mi_SQL += ", '" + Dr_Descuentos["OBSERVACIONES_DESCUENTO"].ToString() + "'";
                                Mi_SQL += ", '" + Dr_Descuentos[Ope_Ing_Descuentos.Campo_Realizo].ToString() + "'";
                                Mi_SQL += ", '" + Descuentos.P_Usuario + "'";
                                Mi_SQL += ", SYSDATE";
                                Mi_SQL += ")";
                            }
                            Cmmd.CommandText = Mi_SQL;
                            Cmmd.ExecuteNonQuery();
                        }
                    }
                }

                if (!Varios_Conceptos)
                {
                    Mi_SQL = "SELECT " + Ope_Ing_Descuentos.Campo_No_Descuento;
                    Mi_SQL += " FROM " + Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos;
                    Mi_SQL += " WHERE " + Ope_Ing_Descuentos.Campo_No_Descuento + " = '" + Descuentos.P_No_Descuento + "'";
                    Cmmd.CommandText = Mi_SQL;
                    OracleDataReader DReader = Cmmd.ExecuteReader();

                    if (DReader.Read())
                    {
                        Mi_SQL = "UPDATE " + Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos;
                        Mi_SQL += " SET ";
                        Mi_SQL += Ope_Ing_Descuentos.Campo_Concepto_Orden_Pago_ID + " = '" + Descuentos.P_Concepto_Orden_Pago_ID + "', ";
                        Mi_SQL += Ope_Ing_Descuentos.Campo_Referencia + " = '" + Descuentos.P_Referencia + "', ";
                        Mi_SQL += Ope_Ing_Descuentos.Campo_Unidades + " = " + Descuentos.P_Unidades + ", ";
                        Mi_SQL += Ope_Ing_Descuentos.Campo_Monto_Honorarios + " = " + Descuentos.P_Monto_Honorarios + ", ";
                        Mi_SQL += Ope_Ing_Descuentos.Campo_Monto_Multas + " = " + Descuentos.P_Monto_Multas + ", ";
                        Mi_SQL += Ope_Ing_Descuentos.Campo_Monto_Moratorios + " = " + Descuentos.P_Monto_Moratorios + ", ";
                        Mi_SQL += Ope_Ing_Descuentos.Campo_Monto_Recargos + " = " + Descuentos.P_Monto_Recargos + ", ";
                        Mi_SQL += Ope_Ing_Descuentos.Campo_Descuento_Honorarios + " = " + Descuentos.P_Descuento_Honorarios + ", ";
                        Mi_SQL += Ope_Ing_Descuentos.Campo_Descuento_Multas + " = " + Descuentos.P_Descuento_Multas + ", ";
                        Mi_SQL += Ope_Ing_Descuentos.Campo_Descuento_Moratorios + " = " + Descuentos.P_Descuento_Moratorios + ", ";
                        Mi_SQL += Ope_Ing_Descuentos.Campo_Descuento_Recargos + " = " + Descuentos.P_Descuento_Recargos + ", ";
                        Mi_SQL += Ope_Ing_Descuentos.Campo_Total_Pagar + " = " + Descuentos.P_Total_Pagar + ", ";
                        Mi_SQL += Ope_Ing_Descuentos.Campo_Estatus + " = '" + Descuentos.P_Estatus + "', ";
                        Mi_SQL += Ope_Ing_Descuentos.Campo_Fecha_Descuento + " = '" + Descuentos.P_Fecha_Descuento.ToString("dd-MM-yyyy") + "', ";
                        Mi_SQL += Ope_Ing_Descuentos.Campo_Fecha_Vencimiento + " = '" + Descuentos.P_Fecha_Vencimiento.ToString("dd-MM-yyyy") + "', ";
                        Mi_SQL += Ope_Ing_Descuentos.Campo_Fundamento_Legal + " = '" + Descuentos.P_Fundamento_Legal + "', ";
                        Mi_SQL += Ope_Ing_Descuentos.Campo_Observaciones + " = '" + Descuentos.P_Observaciones + "', ";
                        Mi_SQL += Ope_Ing_Descuentos.Campo_Realizo + " = '" + Descuentos.P_Realizo + "', ";
                        Mi_SQL += Ope_Ing_Descuentos.Campo_Usuario_Modifico + " = '" + Descuentos.P_Usuario + "', ";
                        Mi_SQL += Ope_Ing_Descuentos.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_SQL += " WHERE " + Ope_Ing_Descuentos.Campo_No_Descuento + " = '" + Descuentos.P_No_Descuento + "'";
                    }
                    else
                    {
                        Descuentos.P_No_Descuento = Obtener_ID_Consecutivo(ref Cmmd, Ope_Ing_Descuentos.Campo_No_Descuento, 10);
                        Mi_SQL = "INSERT INTO " + Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos;
                        Mi_SQL += " (" + Ope_Ing_Descuentos.Campo_No_Descuento;
                        Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Concepto_Orden_Pago_ID;
                        Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Referencia;
                        Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Unidades;
                        Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Monto_Honorarios;
                        Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Monto_Multas;
                        Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Monto_Moratorios;
                        Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Monto_Recargos;
                        Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Descuento_Honorarios;
                        Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Descuento_Multas;
                        Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Descuento_Moratorios;
                        Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Descuento_Recargos;
                        Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Total_Pagar;
                        Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Estatus;
                        Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Fecha_Descuento;
                        Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Fecha_Vencimiento;
                        Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Fundamento_Legal;
                        Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Observaciones;
                        Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Realizo;
                        Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Usuario_Creo;
                        Mi_SQL += ", " + Ope_Ing_Descuentos.Campo_Fecha_Creo + ")";
                        Mi_SQL += " VALUES ('" + Descuentos.P_No_Descuento + "'";
                        Mi_SQL += ", '" + Descuentos.P_Concepto_Orden_Pago_ID + "'";
                        Mi_SQL += ", '" + Descuentos.P_Referencia + "'";
                        Mi_SQL += ", " + Descuentos.P_Unidades;
                        Mi_SQL += ", " + Descuentos.P_Monto_Honorarios;
                        Mi_SQL += ", " + Descuentos.P_Monto_Multas;
                        Mi_SQL += ", " + Descuentos.P_Monto_Moratorios;
                        Mi_SQL += ", " + Descuentos.P_Monto_Recargos;
                        Mi_SQL += ", " + Descuentos.P_Descuento_Honorarios;
                        Mi_SQL += ", " + Descuentos.P_Descuento_Multas;
                        Mi_SQL += ", " + Descuentos.P_Descuento_Moratorios;
                        Mi_SQL += ", " + Descuentos.P_Descuento_Recargos;
                        Mi_SQL += ", " + Descuentos.P_Total_Pagar;
                        Mi_SQL += ", '" + Descuentos.P_Estatus + "'";
                        Mi_SQL += ", '" + Descuentos.P_Fecha_Descuento.ToString("dd-MM-yyyy") + "'";
                        Mi_SQL += ", '" + Descuentos.P_Fecha_Vencimiento.ToString("dd-MM-yyyy") + "'";
                        Mi_SQL += ", '" + Descuentos.P_Fundamento_Legal + "'";
                        Mi_SQL += ", '" + Descuentos.P_Observaciones + "'";
                        Mi_SQL += ", '" + Descuentos.P_Realizo + "'";
                        Mi_SQL += ", '" + Descuentos.P_Usuario + "'";
                        Mi_SQL += ", SYSDATE";
                        Mi_SQL += ")";
                    }
                    Cmmd.CommandText = Mi_SQL;
                    Cmmd.ExecuteNonQuery();
                }

                if (Descuentos.P_Cmmd == null)
                {
                    Trans.Commit();
                }

                Estatus_Modificar = true;
            }
            catch (OracleException Ex)
            {
                if (Descuentos.P_Cmmd == null)
                {
                    Trans.Rollback();
                }
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 2727)
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
                if (Descuentos.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }

            return Estatus_Modificar;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Descuentos
        ///DESCRIPCIÓN          : Obtiene los Descuentos de la tabla.
        ///PARAMETROS           : Descuentos, instancia de Cls_Ope_Ing_Descuentos_Negocio
        ///CREO                 : Antonnio Salvador Benavides Guardado
        ///FECHA_CREO           : 08/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Descuentos(Cls_Ope_Ing_Descuentos_Negocio Descuentos)
        {
            DataTable Dt_Descuentos = new DataTable();
            String Mi_SQL;
            try
            {
                if (Descuentos.P_Campos_Dinamicos != null && Descuentos.P_Campos_Dinamicos != "")
                {
                    Mi_SQL = "SELECT " + Descuentos.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL = "SELECT ";
                    Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_No_Descuento + ", ";
                    Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Concepto_Orden_Pago_ID + ", ";
                    Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Referencia + ", ";
                    Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Unidades + ", ";
                    Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Monto_Honorarios + ", ";
                    Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Monto_Multas + ", ";
                    Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Monto_Moratorios + ", ";
                    Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Monto_Recargos + ", ";
                    Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Descuento_Honorarios + ", ";
                    Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Descuento_Multas + ", ";
                    Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Descuento_Moratorios + ", ";
                    Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Descuento_Recargos + ", ";
                    Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Total_Pagar + ", ";
                    Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Estatus + ", ";
                    Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Fecha_Descuento + ", ";
                    Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Fecha_Vencimiento + ", ";
                    Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Fundamento_Legal + ", ";
                    Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Observaciones + ", ";
                    Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Realizo + ", ";
                    Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Fecha_Creo + ", ";
                    Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Usuario_Creo + ", ";
                    Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Fecha_Modifico + ", ";
                    Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Usuario_Modifico + ", ";
                    if (Mi_SQL.EndsWith(", "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 2);
                    }
                }
                Mi_SQL += " FROM " + Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos;
                if (Descuentos.P_Unir_Tablas != null && Descuentos.P_Unir_Tablas != "")
                {
                    Mi_SQL += ", " + Descuentos.P_Unir_Tablas;
                }
                else
                {
                    if (Descuentos.P_Join != null && Descuentos.P_Join != "")
                    {
                        Mi_SQL += " " + Descuentos.P_Join;
                    }
                }
                if (Descuentos.P_Filtros_Dinamicos != null && Descuentos.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL += " WHERE " + Descuentos.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL += " WHERE ";
                    if (Descuentos.P_No_Descuento != null && Descuentos.P_No_Descuento != "")
                    {
                        Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_No_Descuento + " = '" + Descuentos.P_No_Descuento + "' AND ";
                    }
                    if (Descuentos.P_Concepto_Orden_Pago_ID != null && Descuentos.P_Concepto_Orden_Pago_ID != "")
                    {
                        Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Concepto_Orden_Pago_ID + " = '" + Descuentos.P_Concepto_Orden_Pago_ID + "' AND ";
                    }
                    if (Descuentos.P_Referencia != null && Descuentos.P_Referencia != "")
                    {
                        Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Referencia + " = '" + Descuentos.P_Referencia + "' AND ";
                    }
                    if (Descuentos.P_Estatus != null && Descuentos.P_Estatus != "")
                    {
                        Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Estatus + Validar_Operador_Comparacion(Descuentos.P_Estatus) + " AND ";
                    }
                    if (Descuentos.P_Fecha_Descuento > DateTime.MinValue)
                    {
                        Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Fecha_Descuento + " = '" + Descuentos.P_Fecha_Descuento + "' AND ";
                    }
                    if (Descuentos.P_Fecha_Vencimiento > DateTime.MinValue)
                    {
                        Mi_SQL += Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos + "." + Ope_Ing_Descuentos.Campo_Fecha_Vencimiento + " = '" + Descuentos.P_Fecha_Vencimiento + "' AND ";
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
                if (Descuentos.P_Agrupar_Dinamico != null && Descuentos.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " GROUP BY " + Descuentos.P_Agrupar_Dinamico;
                }
                if (Descuentos.P_Ordenar_Dinamico != null && Descuentos.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Descuentos.P_Ordenar_Dinamico;
                }

                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null)
                {
                    Dt_Descuentos = dataSet.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Descuentos de Trasladado. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Descuentos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_ID_Consecutivo
        ///DESCRIPCIÓN          : Método para generar un nuevo Id del Catálogo
        ///PARAMETROS           : Cmmd, Referencia del Comando de la transacción avierta previamente por el método que lo crea.
        ///                       Campos, Columnas de la tabla en la base de datos a ser conusultadas
        ///                       Longitud_ID, tamaño final con formato del Nuevo_Valor_ID a generar
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 27/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static String Obtener_ID_Consecutivo(ref OracleCommand Cmmd, String Campos, Int32 Longitud_ID)
        {
            String Nuevo_Valor_ID = Convertir_A_Formato_ID(1, Longitud_ID); ;
            try
            {
                String Mi_SQL = "SELECT MAX(" + Campos + ") FROM " + Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos;
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
        ///FECHA_CREO           : 27/Mayo/2012
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
               || Filtro.Trim().ToUpper().StartsWith("IN")
               || Filtro.Trim().ToUpper().StartsWith("NOT IN"))
            {
                Cadena_Validada = " " + Filtro + " ";
            }
            else
            {
                if (Filtro.Trim().ToUpper().StartsWith("NULL")
                    || Filtro.Trim().ToUpper().StartsWith("NOT NULL"))
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
    }
    #endregion
}