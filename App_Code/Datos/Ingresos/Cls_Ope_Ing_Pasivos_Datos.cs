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
using Presidencia.Cls_Ope_Ing_Pasivos.Negocio;

namespace Presidencia.Cls_Ope_Ing_Pasivos.Datos
{

    public class Cls_Ope_Ing_Pasivos_Datos
    {
        #region Altas

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Pasivo
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos los datos de la Orden de Pago
        ///PARAMETROS           : Pasivos, instancia de Cls_Ope_Ing_Pasivos_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 22/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Alta_Pasivo(Cls_Ope_Ing_Pasivos_Negocio Pasivos)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Estatus_Alta = false;

            if (Pasivos.P_Cmmd != null)
            {
                Cmmd = Pasivos.P_Cmmd;
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
                Pasivos.P_No_Pasivo = Obtener_ID_Consecutivo(ref Cmmd, Ope_Ing_Pasivo.Campo_No_Pasivo, Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo, "", 10);
                String Mi_SQL = "INSERT INTO " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                Mi_SQL += " (" + Ope_Ing_Pasivo.Campo_No_Pasivo;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Concepto_Ing_ID;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_SubConcepto_Ing_ID;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Dependencia_ID;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_No_Recibo;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_No_Pago;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_No_Concepto;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Referencia;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Descripcion;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Origen;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Fecha_Ingreso;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Fecha_Vencimiento;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Fecha_Pago;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Monto;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Recargos;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Cantidad;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Estatus;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Contribuyente;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Observaciones;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Usuario_Creo;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Fecha_Creo + ")";
                Mi_SQL += " VALUES ('" + Pasivos.P_No_Pasivo + "'";
                Mi_SQL += ", '" + Pasivos.P_Claves_Ingreso_ID + "'";
                Mi_SQL += ", '" + Pasivos.P_Concepto_Ing_ID + "'";
                Mi_SQL += ", '" + Pasivos.P_SubConcepto_Ing_ID + "'";
                Mi_SQL += ", '" + Pasivos.P_Dependencia_ID + "'";
                Mi_SQL += ", '" + Pasivos.P_Cuenta_Predial_ID + "'";
                Mi_SQL += ", '" + Pasivos.P_No_Recibo + "'";
                Mi_SQL += ", '" + Pasivos.P_No_Pago + "'";
                Mi_SQL += ", " + Pasivos.P_No_Concepto;
                Mi_SQL += ", '" + Pasivos.P_Referencia + "'";
                Mi_SQL += ", '" + Pasivos.P_Descripcion + "'";
                Mi_SQL += ", '" + Pasivos.P_Origen + "'";
                Mi_SQL += ", '" + Pasivos.P_Fecha_Ingreso.ToString("dd-MM-yyyy") + "'";
                Mi_SQL += ", '" + Pasivos.P_Fecha_Vencimiento.ToString("dd-MM-yyyy") + "'";
                Mi_SQL += ", '" + Pasivos.P_Fecha_Pago.ToString("dd-MM-yyyy") + "'";
                Mi_SQL += ", " + Pasivos.P_Monto;
                Mi_SQL += ", " + Pasivos.P_Recargos;
                Mi_SQL += ", " + Pasivos.P_Cantidad;
                Mi_SQL += ", '" + Pasivos.P_Estatus + "'";
                Mi_SQL += ", '" + Pasivos.P_Contribuyente + "'";
                Mi_SQL += ", '" + Pasivos.P_Observaciones + "'";
                Mi_SQL += ", '" + Pasivos.P_Usuario + "'";
                Mi_SQL += ", SYSDATE";
                Mi_SQL += ")";
                Cmmd.CommandText = Mi_SQL;
                Cmmd.ExecuteNonQuery();

                if (Pasivos.P_Cmmd == null)
                {
                    Trans.Commit();
                }

                Estatus_Alta = true;
            }
            catch (OracleException Ex)
            {
                if (Pasivos.P_Cmmd == null)
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
                if (Pasivos.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }

            return Estatus_Alta;
        }
        #endregion

        #region Modificaciones
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Pasivo
        ///DESCRIPCIÓN          : Modifica en la Base de Datos los datos de la Orden de Pago
        ///PARAMETROS           : Pasivos, instancia de Cls_Ope_Ing_Pasivos_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 22/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Modificar_Pasivo(Cls_Ope_Ing_Pasivos_Negocio Pasivos)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Estatus_Modificar = false;

            if (Pasivos.P_Cmmd != null)
            {
                Cmmd = Pasivos.P_Cmmd;
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

                Mi_SQL = "SELECT " + Ope_Ing_Pasivo.Campo_No_Pasivo;
                Mi_SQL += " FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                Mi_SQL += " WHERE " + Ope_Ing_Pasivo.Campo_Referencia + " = '" + Pasivos.P_Referencia + "'";
                Mi_SQL += " AND " + Ope_Ing_Pasivo.Campo_SubConcepto_Ing_ID + " = '" + Pasivos.P_SubConcepto_Ing_ID + "'";
                Mi_SQL += " AND " + Ope_Ing_Pasivo.Campo_Descripcion + " = '" + Pasivos.P_Descripcion + "'";
                Cmmd.CommandText = Mi_SQL;
                OracleDataReader DReader = Cmmd.ExecuteReader();

                if (DReader.Read())
                {
                    Mi_SQL = "UPDATE " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                    Mi_SQL += " SET ";
                    if (Pasivos.P_Campos_Dinamicos != null && Pasivos.P_Campos_Dinamicos != "")
                    {
                        Mi_SQL += Pasivos.P_Campos_Dinamicos;
                    }
                    else
                    {
                        if (Pasivos.P_Claves_Ingreso_ID != null && Pasivos.P_Claves_Ingreso_ID != "")
                        {
                            Mi_SQL += Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID + " = '" + Pasivos.P_Claves_Ingreso_ID + "', ";
                        }
                        if (Pasivos.P_SubConcepto_Ing_ID != null && Pasivos.P_SubConcepto_Ing_ID != "")
                        {
                            Mi_SQL += Ope_Ing_Pasivo.Campo_SubConcepto_Ing_ID + " = '" + Pasivos.P_SubConcepto_Ing_ID + "', ";
                        }
                        if (Pasivos.P_Dependencia_ID != null && Pasivos.P_Dependencia_ID != "")
                        {
                            Mi_SQL += Ope_Ing_Pasivo.Campo_Dependencia_ID + " = '" + Pasivos.P_Dependencia_ID + "', ";
                        }
                        if (Pasivos.P_Cuenta_Predial_ID != null && Pasivos.P_Cuenta_Predial_ID != "")
                        {
                            Mi_SQL += Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID + " = '" + Pasivos.P_Cuenta_Predial_ID + "', ";
                        }
                        if (Pasivos.P_No_Recibo != null && Pasivos.P_No_Recibo != "")
                        {
                            Mi_SQL += Ope_Ing_Pasivo.Campo_No_Recibo + " = '" + Pasivos.P_No_Recibo + "', ";
                        }
                        if (Pasivos.P_No_Pago != null && Pasivos.P_No_Pago != "")
                        {
                            Mi_SQL += Ope_Ing_Pasivo.Campo_No_Pago + " = '" + Pasivos.P_No_Pago + "', ";
                        }
                        if (Pasivos.P_Referencia != null && Pasivos.P_Referencia != "")
                        {
                            Mi_SQL += Ope_Ing_Pasivo.Campo_Referencia + " = '" + Pasivos.P_Referencia + "', ";
                        }
                        if (Pasivos.P_Origen != null && Pasivos.P_Origen != "")
                        {
                            Mi_SQL += Ope_Ing_Pasivo.Campo_Origen + " = '" + Pasivos.P_Origen + "', ";
                        }
                        if (Pasivos.P_Descripcion != null && Pasivos.P_Descripcion != "")
                        {
                            Mi_SQL += Ope_Ing_Pasivo.Campo_Descripcion + " = '" + Pasivos.P_Descripcion + "', ";
                        }
                        if (Pasivos.P_Fecha_Ingreso > DateTime.MinValue)
                        {
                            Mi_SQL += Ope_Ing_Pasivo.Campo_Fecha_Ingreso + " = '" + Pasivos.P_Fecha_Ingreso.ToString("dd-MM-yyyy") + "', ";
                        }
                        if (Pasivos.P_Fecha_Vencimiento > DateTime.MinValue)
                        {
                            Mi_SQL += Ope_Ing_Pasivo.Campo_Fecha_Vencimiento + " = '" + Pasivos.P_Fecha_Vencimiento.ToString("dd-MM-yyyy") + "', ";
                        }
                        if (Pasivos.P_Fecha_Pago > DateTime.MinValue)
                        {
                            Mi_SQL += Ope_Ing_Pasivo.Campo_Fecha_Pago + " = '" + Pasivos.P_Fecha_Pago.ToString("dd-MM-yyyy") + "', ";
                        }
                        //if (Pasivos.P_Monto != 0)
                        {
                            Mi_SQL += Ope_Ing_Pasivo.Campo_Monto + " = " + Pasivos.P_Monto + ", ";
                        }
                        if (Pasivos.P_Recargos != 0)
                        {
                            Mi_SQL += Ope_Ing_Pasivo.Campo_Recargos + " = " + Pasivos.P_Recargos + ", ";
                        }
                        if (Pasivos.P_Cantidad != 0)
                        {
                            Mi_SQL += Ope_Ing_Pasivo.Campo_Cantidad + " = " + Pasivos.P_Cantidad + ", ";
                        }
                        if (Pasivos.P_Estatus != null && Pasivos.P_Estatus != "")
                        {
                            Mi_SQL += Ope_Ing_Pasivo.Campo_Estatus + " = '" + Pasivos.P_Estatus + "', ";
                        }
                        if (Pasivos.P_Contribuyente != null && Pasivos.P_Contribuyente != "")
                        {
                            Mi_SQL += Ope_Ing_Pasivo.Campo_Contribuyente + " = '" + Pasivos.P_Contribuyente + "', ";
                        }
                        if (Pasivos.P_Observaciones != null && Pasivos.P_Observaciones != "")
                        {
                            Mi_SQL += Ope_Ing_Pasivo.Campo_Observaciones + " = '" + Pasivos.P_Observaciones + "', ";
                        }
                    }
                    Mi_SQL += Ope_Ing_Pasivo.Campo_Usuario_Modifico + " = '" + Pasivos.P_Usuario + "', ";
                    Mi_SQL += Ope_Ing_Pasivo.Campo_Fecha_Modifico + " = SYSDATE";
                    if (Pasivos.P_Filtros_Dinamicos != null && Pasivos.P_Filtros_Dinamicos != "")
                    {
                        Mi_SQL += " WHERE " + Pasivos.P_Filtros_Dinamicos;
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                        if (Pasivos.P_No_Pasivo != null && Pasivos.P_No_Pasivo != "")
                        {
                            Mi_SQL += Ope_Ing_Pasivo.Campo_No_Pasivo + " = '" + Pasivos.P_No_Pasivo + "' AND ";
                        }
                        if (Pasivos.P_Referencia != null && Pasivos.P_Referencia != "")
                        {
                            Mi_SQL += Ope_Ing_Pasivo.Campo_Referencia + " = '" + Pasivos.P_Referencia + "' AND ";
                        }
                        if (Pasivos.P_Descripcion != null && Pasivos.P_Descripcion != "")
                        {
                            Mi_SQL += Ope_Ing_Pasivo.Campo_Descripcion + " = '" + Pasivos.P_Descripcion + "' AND ";
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
                }
                else
                {
                    Pasivos.P_No_Pasivo = Obtener_ID_Consecutivo(ref Cmmd, Ope_Ing_Pasivo.Campo_No_Pasivo, Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo, "", 10);
                    Mi_SQL = "INSERT INTO " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                    Mi_SQL += " (" + Ope_Ing_Pasivo.Campo_No_Pasivo;
                    Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID;
                    Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_SubConcepto_Ing_ID;
                    Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Dependencia_ID;
                    Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID;
                    Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_No_Recibo;
                    Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_No_Pago;
                    Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Referencia;
                    Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Descripcion;
                    Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Origen;
                    Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Fecha_Ingreso;
                    Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Fecha_Vencimiento;
                    Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Fecha_Pago;
                    Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Monto;
                    Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Recargos;
                    Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Cantidad;
                    Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Estatus;
                    Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Contribuyente;
                    Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Observaciones;
                    Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Usuario_Creo;
                    Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Fecha_Creo + ")";
                    Mi_SQL += " VALUES ('" + Pasivos.P_No_Pasivo + "'";
                    Mi_SQL += ", '" + Pasivos.P_Claves_Ingreso_ID + "'";
                    Mi_SQL += ", '" + Pasivos.P_SubConcepto_Ing_ID + "'";
                    Mi_SQL += ", '" + Pasivos.P_Dependencia_ID + "'";
                    Mi_SQL += ", '" + Pasivos.P_Cuenta_Predial_ID + "'";
                    Mi_SQL += ", '" + Pasivos.P_No_Recibo + "'";
                    Mi_SQL += ", '" + Pasivos.P_No_Pago + "'";
                    Mi_SQL += ", '" + Pasivos.P_Referencia + "'";
                    Mi_SQL += ", '" + Pasivos.P_Descripcion + "'";
                    Mi_SQL += ", '" + Pasivos.P_Origen + "'";
                    Mi_SQL += ", '" + Pasivos.P_Fecha_Ingreso.ToString("dd-MM-yyyy") + "'";
                    Mi_SQL += ", '" + Pasivos.P_Fecha_Vencimiento.ToString("dd-MM-yyyy") + "'";
                    Mi_SQL += ", '" + Pasivos.P_Fecha_Pago.ToString("dd-MM-yyyy") + "'";
                    Mi_SQL += ", " + Pasivos.P_Monto;
                    Mi_SQL += ", " + Pasivos.P_Recargos;
                    Mi_SQL += ", " + Pasivos.P_Cantidad;
                    Mi_SQL += ", '" + Pasivos.P_Estatus + "'";
                    Mi_SQL += ", '" + Pasivos.P_Contribuyente + "'";
                    Mi_SQL += ", '" + Pasivos.P_Observaciones + "'";
                    Mi_SQL += ", '" + Pasivos.P_Usuario + "'";
                    Mi_SQL += ", SYSDATE";
                    Mi_SQL += ")";
                }
                Cmmd.CommandText = Mi_SQL;
                Cmmd.ExecuteNonQuery();

                if (Pasivos.P_Cmmd == null)
                {
                    Trans.Commit();
                }

                Estatus_Modificar = true;
            }
            catch (OracleException Ex)
            {
                if (Pasivos.P_Cmmd == null)
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
                if (Pasivos.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }

            return Estatus_Modificar;
        }
        #endregion

        #region Eliminaciones

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Eliminar_Pasivo
        ///DESCRIPCIÓN          : Elimina de la Base de Datos los registros de Orden de Pago
        ///PARAMETROS           : Pasivos, instancia de Cls_Ope_Ing_Pasivos_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 22/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Eliminar_Pasivo(Cls_Ope_Ing_Pasivos_Negocio Pasivos)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Estatus_Eliminar = false;

            if (Pasivos.P_Cmmd != null)
            {
                Cmmd = Pasivos.P_Cmmd;
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
                    Mi_SQL = "DELETE FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                    Mi_SQL += " WHERE " + Ope_Ing_Pasivo.Campo_No_Pasivo + " = '" + Pasivos.P_No_Pasivo + "'";
                    Cmmd.CommandText = Mi_SQL;
                    Cmmd.ExecuteNonQuery();
                }
                catch (OracleException Ex)
                {
                    if (Ex.Code == 547 || Ex.Code == 2292)
                    {
                        Mi_SQL = "UPDATE " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                        Mi_SQL += " SET " + Ope_Ing_Pasivo.Campo_Estatus + " = 'CANCELADO'";
                        Mi_SQL += " WHERE " + Ope_Ing_Pasivo.Campo_No_Pasivo + " = '" + Pasivos.P_No_Pasivo + "'";
                        Cmmd.CommandText = Mi_SQL;
                        Cmmd.ExecuteNonQuery();
                    }
                    else
                    {
                        throw new Exception();
                    }
                }

                if (Pasivos.P_Cmmd == null)
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
                if (Pasivos.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }

            return Estatus_Eliminar;
        }
        #endregion

        #region Consultas

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Pasivos
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos los datos de la Orden de Pago
        ///PARAMETROS           : Pasivos, instancia de Cls_Ope_Ing_Pasivos_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 22/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Pasivos(Cls_Ope_Ing_Pasivos_Negocio Pasivos)
        {
            DataTable Dt_Tipos_Pagos = new DataTable();
            String Mi_SQL;
            String Mi_SQL_Campos_Foraneos = "";
            try
            {
                if (Pasivos.P_Incluir_Campos_Foraneos)
                {
                    Mi_SQL_Campos_Foraneos += "(SELECT SUBCONCEPTOS." + Cat_Psp_SubConcepto_Ing.Campo_Clave + " FROM " + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + " SUBCONCEPTOS WHERE SUBCONCEPTOS." + Cat_Psp_SubConcepto_Ing.Campo_SubConcepto_Ing_ID + " = " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_SubConcepto_Ing_ID + ") AS CLAVE_INGRESO, ";
                }
                if (Pasivos.P_Campos_Dinamicos != null && Pasivos.P_Campos_Dinamicos != "")
                {
                    Mi_SQL = "SELECT " + Mi_SQL_Campos_Foraneos + Pasivos.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL = "SELECT " + Mi_SQL_Campos_Foraneos;
                    Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Pasivo + ", ";
                    Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID + ", ";
                    Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_SubConcepto_Ing_ID + ", ";
                    Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Dependencia_ID + ", ";
                    Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID + ", ";
                    Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Recibo + ", ";
                    Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Pago + ", ";
                    Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Referencia + ", ";
                    Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Origen + ", ";
                    Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Descripcion + ", ";
                    Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Fecha_Ingreso + ", ";
                    Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Fecha_Vencimiento + ", ";
                    Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Fecha_Pago + ", ";
                    Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Monto + ", ";
                    Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Recargos + ", ";
                    Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Referencia + ", ";
                    Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Cantidad + ", ";
                    Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Estatus + ", ";
                    Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Contribuyente + ", ";
                    Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Observaciones + ", ";
                }
                if (Pasivos.P_Incluir_Campos_Vitacora)
                {
                    Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Usuario_Creo + ", ";
                    Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Fecha_Creo + ", ";
                    Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Usuario_Modifico + ", ";
                    Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Fecha_Modifico + ", ";
                }
                if (Mi_SQL.EndsWith(", "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 2);
                }
                Mi_SQL += " FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                if (Pasivos.P_Unir_Tablas != null && Pasivos.P_Unir_Tablas != "")
                {
                    Mi_SQL += ", " + Pasivos.P_Unir_Tablas;
                }
                else
                {
                    if (Pasivos.P_Join != null && Pasivos.P_Join != "")
                    {
                        Mi_SQL += " " + Pasivos.P_Join;
                    }
                }
                if (Pasivos.P_Filtros_Dinamicos != null && Pasivos.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL += " WHERE " + Pasivos.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL += " WHERE ";
                    if (Pasivos.P_No_Pasivo != null && Pasivos.P_No_Pasivo != "")
                    {
                        Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Pasivo + " = '" + Pasivos.P_No_Pasivo + "' AND ";
                    }
                    if (Pasivos.P_Claves_Ingreso_ID != null && Pasivos.P_Claves_Ingreso_ID != "")
                    {
                        Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID + " = '" + Pasivos.P_Claves_Ingreso_ID + "' AND ";
                    }
                    if (Pasivos.P_SubConcepto_Ing_ID != null && Pasivos.P_SubConcepto_Ing_ID != "")
                    {
                        Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_SubConcepto_Ing_ID + " = '" + Pasivos.P_SubConcepto_Ing_ID + "' AND ";
                    }
                    if (Pasivos.P_Dependencia_ID != null && Pasivos.P_Dependencia_ID != "")
                    {
                        Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Dependencia_ID + " = '" + Pasivos.P_Dependencia_ID + "' AND ";
                    }
                    if (Pasivos.P_Cuenta_Predial_ID != null && Pasivos.P_Cuenta_Predial_ID != "")
                    {
                        Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID + " = '" + Pasivos.P_Cuenta_Predial_ID + "' AND ";
                    }
                    if (Pasivos.P_No_Recibo != null && Pasivos.P_No_Recibo != "")
                    {
                        Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Recibo + Validar_Operador_Comparacion(Pasivos.P_No_Recibo) + " AND ";
                    }
                    if (Pasivos.P_No_Pago != null && Pasivos.P_No_Pago != "")
                    {
                        Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Pago + Validar_Operador_Comparacion(Pasivos.P_No_Pago) + " AND ";
                    }
                    if (Pasivos.P_Referencia != null && Pasivos.P_Referencia != "")
                    {
                        Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Referencia + Validar_Operador_Comparacion(Pasivos.P_Referencia) + " AND ";
                    }
                    if (Pasivos.P_Origen != null && Pasivos.P_Origen != "")
                    {
                        Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Origen + Validar_Operador_Comparacion(Pasivos.P_Origen) + " AND ";
                    }
                    if (Pasivos.P_Estatus != null && Pasivos.P_Estatus != "")
                    {
                        Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Estatus + Validar_Operador_Comparacion(Pasivos.P_Estatus) + " AND ";
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
                if (Pasivos.P_Agrupar_Dinamico != null && Pasivos.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " GROUP BY " + Pasivos.P_Agrupar_Dinamico;
                }
                if (Pasivos.P_Ordenar_Dinamico != null && Pasivos.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Pasivos.P_Ordenar_Dinamico;
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
        ///FECHA_CREO           : 22/Junio/2012
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
        ///FECHA_CREO           : 22/Junio/2012
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