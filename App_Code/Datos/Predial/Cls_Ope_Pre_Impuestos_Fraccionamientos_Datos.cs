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
using Presidencia.Operacion_Predial_Impuestos_Fraccionamientos.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Sessiones;

/// <summary>
/// Summary description for Cls_Ope_Pre_Impuestos_Fraccionamientos_Datos
/// </summary>

namespace Presidencia.Operacion_Predial_Impuestos_Fraccionamientos.Datos
{

    public class Cls_Ope_Pre_Impuestos_Fraccionamientos_Datos
    {
        #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Impuesto_Fraccionamiento
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo registro de Impuesto_Fraccionamiento
        ///PARAMETROS           : 1. Impuesto_Fraccionamiento.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Impuesto_Fraccionamientos_Negocio
        ///                                 con los datos de Impuesto_Fraccionamientos que va a ser dado de Alta.
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 22/Julio/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Alta_Impuesto_Fraccionamiento(Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio Impuesto_Fraccionamiento)
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
            String No_Impuesto_Fraccionamiento = Obtener_ID_Consecutivo(Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos, Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento, "", 10);
            try
            {
                String Mi_SQL = "INSERT INTO " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + " (";
                Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + ", ";
                Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Vencimiento + ", ";
                Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Oficio + ", ";
                Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + ", ";
                Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Observaciones + ", ";
                Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Usuario_Creo + ", ";
                Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Creo + ") ";
                Mi_SQL += "VALUES (";
                Mi_SQL += "'" + No_Impuesto_Fraccionamiento + "', ";
                if (Impuesto_Fraccionamiento.P_Cuenta_Predial_ID != "" && Impuesto_Fraccionamiento.P_Cuenta_Predial_ID != null)
                {
                    Mi_SQL += "'" + Impuesto_Fraccionamiento.P_Cuenta_Predial_ID + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Impuesto_Fraccionamiento.P_Fecha_Vencimiento.ToString() != "" && Impuesto_Fraccionamiento.P_Fecha_Vencimiento.ToString() != null)
                {
                    Mi_SQL += "'" + Impuesto_Fraccionamiento.P_Fecha_Vencimiento.ToString("d-M-yyyy") + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Impuesto_Fraccionamiento.P_Fecha_Oficio.ToString() != "" && Impuesto_Fraccionamiento.P_Fecha_Oficio.ToString() != null)
                {
                    Mi_SQL += "'" + Impuesto_Fraccionamiento.P_Fecha_Oficio.ToString("d-M-yyyy") + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                Mi_SQL += "'" + Impuesto_Fraccionamiento.P_Estatus + "', '";
                Mi_SQL += Impuesto_Fraccionamiento.P_Observaciones + "', ";
                Mi_SQL += "'" + Impuesto_Fraccionamiento.P_Usuario + "', SYSDATE)";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //Inserta los Detalles de los Impuestos
                if (Impuesto_Fraccionamiento.P_Dt_Detalles_Impuestos_Fraccionamiento != null
                    && Impuesto_Fraccionamiento.P_Dt_Detalles_Impuestos_Fraccionamiento.Rows.Count > 0)
                {
                    foreach (DataRow Dr_Detalle in Impuesto_Fraccionamiento.P_Dt_Detalles_Impuestos_Fraccionamiento.Rows)
                    {
                        Mi_SQL = "INSERT INTO " + Ope_Pre_Detalles_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Detalles_Impuestos_Fraccionamientos + " (";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Impuesto_Fraccionamiento_ID + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Superficie_Fraccionar + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Multas_Id + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Importe + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Recargos + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Total + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Usuario_Creo + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Fecha_Creo + ") ";
                        Mi_SQL += "VALUES (";
                        Mi_SQL += "'" + No_Impuesto_Fraccionamiento + "', ";
                        if (Dr_Detalle[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Impuesto_Fraccionamiento_ID] != null)
                        {
                            Mi_SQL += "'" + Dr_Detalle[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Impuesto_Fraccionamiento_ID].ToString() + "', ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Superficie_Fraccionar] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Superficie_Fraccionar].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Cat_Pre_Multas_Fraccionamientos_Detalles.Campo_Multa_Cuota_ID] != null && Dr_Detalle[Cat_Pre_Multas_Fraccionamientos_Detalles.Campo_Multa_Cuota_ID].ToString() != "")
                        {
                            Mi_SQL += "'"+Dr_Detalle[Cat_Pre_Multas_Fraccionamientos_Detalles.Campo_Multa_Cuota_ID].ToString() + "', ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Importe] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Importe].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Recargos] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Recargos].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Total] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Total].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        Mi_SQL += "'" + Impuesto_Fraccionamiento.P_Usuario + "', SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                }
                Impuesto_Fraccionamiento.P_No_Impuesto_Fraccionamiento = No_Impuesto_Fraccionamiento;
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Impuesto de Fraccionamiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN : Modificar_Impuesto_Fraccionamiento
        ///DESCRIPCIÓN          : Modifica en la Base de Datos el registro indicado del Impuesto_Fraccionamiento
        ///PARAMETROS          : 1. Impuesto_Fraccionamiento.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Impuesto_Fraccionamientos_Negocio
        ///                                 con los datos del Impuesto_Fraccionamientos que va a ser Modificado.
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 22/Julio/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Modificar_Impuesto_Fraccionamiento(Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio Impuesto_Fraccionamiento)
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
                String Mi_SQL = "UPDATE " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + " SET ";
                Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID + " = '" + Impuesto_Fraccionamiento.P_Cuenta_Predial_ID + "', ";
                //Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Vencimiento + " = '" + Impuesto_Fraccionamiento.P_Fecha_Vencimiento.ToShortDateString() + "', ";
                Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + " = '" + Impuesto_Fraccionamiento.P_Estatus + "', ";
                if (Impuesto_Fraccionamiento.P_Observaciones != "" && Impuesto_Fraccionamiento.P_Observaciones != null)
                {
                    Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Observaciones + " = '" + Impuesto_Fraccionamiento.P_Observaciones + "', ";
                }
                //Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Impuesto_Fraccionamiento.P_Usuario + "', ";
                Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Oficio + " = '" + Impuesto_Fraccionamiento.P_Fecha_Oficio.ToString("d-M-yyyy") + "', ";
                Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Vencimiento + " = '" + Impuesto_Fraccionamiento.P_Fecha_Vencimiento.ToString("d-M-yyyy") + "', ";
                Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Impuesto_Fraccionamiento.P_Usuario + "', ";
                Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL += "WHERE " + Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + " = '" + Impuesto_Fraccionamiento.P_No_Impuesto_Fraccionamiento + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //Elimina los Detalles de los Impuestos
                Mi_SQL = "DELETE FROM " + Ope_Pre_Detalles_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Detalles_Impuestos_Fraccionamientos;
                Mi_SQL += " WHERE " + Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + " = '" + Impuesto_Fraccionamiento.P_No_Impuesto_Fraccionamiento + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //Inserta los Detalles de los Impuestos
                if (Impuesto_Fraccionamiento.P_Dt_Detalles_Impuestos_Fraccionamiento != null
                    && Impuesto_Fraccionamiento.P_Dt_Detalles_Impuestos_Fraccionamiento.Rows.Count > 0)
                {
                    foreach (DataRow Dr_Detalle in Impuesto_Fraccionamiento.P_Dt_Detalles_Impuestos_Fraccionamiento.Rows)
                    {
                        Mi_SQL = "INSERT INTO " + Ope_Pre_Detalles_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Detalles_Impuestos_Fraccionamientos + " (";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Impuesto_Fraccionamiento_ID + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Superficie_Fraccionar + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Multas_Id + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Importe + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Recargos + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Total + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Usuario_Creo + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Fecha_Creo + ") ";
                        Mi_SQL += "VALUES (";
                        Mi_SQL += "'" + Impuesto_Fraccionamiento.P_No_Impuesto_Fraccionamiento + "', ";
                        if (Dr_Detalle[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Impuesto_Fraccionamiento_ID] != null)
                        {
                            Mi_SQL += "'" + Dr_Detalle[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Impuesto_Fraccionamiento_ID].ToString() + "', ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Superficie_Fraccionar] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Superficie_Fraccionar].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Cat_Pre_Multas_Fraccionamientos_Detalles.Campo_Multa_Cuota_ID] != null && Dr_Detalle[Cat_Pre_Multas_Fraccionamientos_Detalles.Campo_Multa_Cuota_ID].ToString() != "")
                        {
                            Mi_SQL += "'"+Dr_Detalle[Cat_Pre_Multas_Fraccionamientos_Detalles.Campo_Multa_Cuota_ID].ToString() + "', ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Importe] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Importe].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Recargos] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Recargos].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        if (Dr_Detalle[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Total] != null)
                        {
                            Mi_SQL += Dr_Detalle[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Total].ToString() + ", ";
                        }
                        else
                        {
                            Mi_SQL += "NULL, ";
                        }
                        Mi_SQL += "'" + Impuesto_Fraccionamiento.P_Usuario + "', SYSDATE)";
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
                    Mensaje = "Error al intentar modificar un Registro de Impuesto de Fraccionamiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN : Cancelar_Pasivo
        ///DESCRIPCIÓN          : Modifica en la Base de Datos el registro indicado del Impuesto_Fraccionamiento
        ///PARAMETROS          : 1. Referencia.   Referencia con la cual se modificara el registro a cancelado
        ///CREO                 : Miguel Angel Bedolla Moreno
        ///FECHA_CREO           : 26/Septiembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Cancelar_Pasivo(String Referencia, String Estatus, String Monto)
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
                Mi_SQL += Ope_Ing_Pasivo.Campo_Estatus + " = '" + Estatus + "', ";
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
        ///NOMBRE DE LA FUNCIÓN : Consultar_Impuesto_Fraccionamientos
        ///DESCRIPCIÓN          : Obtiene todos las Impuesto_Fraccionamiento que estan dadas de alta en la base de datos
        ///PARAMETROS           : 1. Impuesto_Fraccionamiento.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Impuesto_Fraccionamientos_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 22/Julio/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Impuestos_Fraccionamiento(Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio Impuesto_Fraccionamiento)
        {
            DataTable Dt_Impuesto_Fraccionamientos = new DataTable();
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT ";
                if (Impuesto_Fraccionamiento.P_Campos_Foraneos)
                {
                    Mi_SQL += "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID + ") AS Cuenta_Predial, ";
                }
                if (Impuesto_Fraccionamiento.P_Campos_Dinamicos != null && Impuesto_Fraccionamiento.P_Campos_Dinamicos != "")
                {
                    Mi_SQL += Impuesto_Fraccionamiento.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + ", ";
                    Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID + ", ";
                    Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Vencimiento + ", ";
                    Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + ", ";
                    Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Creo + ", ";
                    Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Oficio + ", ";
                    Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Usuario_Creo + ", ";
                    Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Observaciones;
                }
                Mi_SQL += " FROM " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos;
                if (Impuesto_Fraccionamiento.P_Filtros_Dinamicos != null && Impuesto_Fraccionamiento.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL += " WHERE " + Impuesto_Fraccionamiento.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL += " WHERE ";
                    if (Impuesto_Fraccionamiento.P_No_Impuesto_Fraccionamiento != "" && Impuesto_Fraccionamiento.P_No_Impuesto_Fraccionamiento != null)
                    {
                        Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + " = '" + Impuesto_Fraccionamiento.P_No_Impuesto_Fraccionamiento + "' AND ";
                    }
                    if (Impuesto_Fraccionamiento.P_Cuenta_Predial_ID != "" && Impuesto_Fraccionamiento.P_Cuenta_Predial_ID != null)
                    {
                        Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID + " = '" + Impuesto_Fraccionamiento.P_Cuenta_Predial_ID + "' AND ";
                    }
                    if (Impuesto_Fraccionamiento.P_Estatus != "" && Impuesto_Fraccionamiento.P_Estatus != null)
                    {
                        Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + " = '" + Impuesto_Fraccionamiento.P_Estatus + "' AND ";
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
                if (Impuesto_Fraccionamiento.P_Agrupar_Dinamico != null && Impuesto_Fraccionamiento.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " GROUP BY " + Impuesto_Fraccionamiento.P_Agrupar_Dinamico;
                }
                if (Impuesto_Fraccionamiento.P_Ordenar_Dinamico != null && Impuesto_Fraccionamiento.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Impuesto_Fraccionamiento.P_Ordenar_Dinamico;
                }
                else
                {
                    Mi_SQL += " ORDER BY " + Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID;
                }

                DataSet Ds_Impuesto_Fraccionamientos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Impuesto_Fraccionamientos != null)
                {
                    Dt_Impuesto_Fraccionamientos = Ds_Impuesto_Fraccionamientos.Tables[0];

                    //Consulta los Detalles de los Impuestos
                    if (Impuesto_Fraccionamiento.P_Campos_Sumados != null && Impuesto_Fraccionamiento.P_Campos_Sumados)
                    {
                        Mi_SQL = "SELECT ";
                        //Mi_SQL += Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Campo_No_Impuesto_Derecho_Supervision + ", ";
                        //Mi_SQL += Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Campo_Derecho_Supervision_Tasa_ID + ", ";
                        //Mi_SQL += "(SELECT " + Cat_Pre_Der_Super_Tasas.Campo_Tasa + "/100 FROM " + Cat_Pre_Der_Super_Tasas.Tabla_Cat_Pre_Der_Super_Tasas + " WHERE " + Cat_Pre_Der_Super_Tasas.Campo_Derecho_Supervision_Tasa_ID + " = " + Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Impuestos_Derechos_Supervision + "." + Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Campo_Derecho_Supervision_Tasa_ID + ") Tasa_Derecho_Supervision, ";
                        Mi_SQL += "NVL(SUM((SELECT " + Cat_Pre_Multas_Fraccionamientos_Detalles.Campo_Monto + " FROM " + Cat_Pre_Multas_Fraccionamientos_Detalles.Tabla_Cat_Pre_Multas_Fraccionamientos_Detalles + " WHERE " + Cat_Pre_Multas_Fraccionamientos_Detalles.Campo_Multa_Cuota_ID + " = " + Ope_Pre_Detalles_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Detalles_Impuestos_Fraccionamientos + "." + Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Multas_Id + ")), 0.00) AS " + Cat_Pre_Multas_Fraccionamientos_Detalles.Campo_Monto + ", ";
                        //Mi_SQL += "(SELECT (SELECT " + Cat_Pre_Derechos_Supervision.Campo_Descripcion + " FROM " + Cat_Pre_Derechos_Supervision.Tabla_Cat_Pre_Derechos_Supervision + " WHERE " + Cat_Pre_Derechos_Supervision.Tabla_Cat_Pre_Derechos_Supervision + "." + Cat_Pre_Derechos_Supervision.Campo_Derecho_Supervision_ID + "=" + Cat_Pre_Der_Super_Tasas.Tabla_Cat_Pre_Der_Super_Tasas + "." + Cat_Pre_Der_Super_Tasas.Campo_Derecho_Supervision_ID + ")";
                        //Mi_SQL += " FROM " + Cat_Pre_Der_Super_Tasas.Tabla_Cat_Pre_Der_Super_Tasas + " WHERE " + Cat_Pre_Der_Super_Tasas.Tabla_Cat_Pre_Der_Super_Tasas + "." + Cat_Pre_Der_Super_Tasas.Campo_Derecho_Supervision_Tasa_ID + "=" + Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Impuestos_Derechos_Supervision + "." + Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Campo_Derecho_Supervision_Tasa_ID + ") AS DESCRIPCION_TASA, ";
                        //Mi_SQL += Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Campo_Valor_Estimado_Obra + ", ";
                        Mi_SQL += "SUM(" + Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Importe + ") AS " + Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Importe + ", ";
                        //Mi_SQL += Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Campo_Multas_Id + " AS DER_MULTA_ID, ";
                        Mi_SQL += "0.00 HONORARIOS, ";
                        Mi_SQL += "SUM(" + Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Recargos + ") " + Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Recargos + ", ";
                        Mi_SQL += "SUM(" + Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Total + ") AS " + Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Total;
                        Mi_SQL += " FROM ";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Detalles_Impuestos_Fraccionamientos;
                    }
                    else
                    {
                        Mi_SQL = "SELECT ";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Impuesto_Fraccionamiento_ID + ", ";
                        Mi_SQL += "(SELECT " + Cat_Pre_Fracc_Impuestos.Campo_Monto + " FROM " + Cat_Pre_Fracc_Impuestos.Tabla_Cat_Pre_Fracc_Impuestos + " WHERE " + Cat_Pre_Fracc_Impuestos.Campo_Impuesto_Fraccionamiento_ID + " = " + Ope_Pre_Detalles_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Detalles_Impuestos_Fraccionamientos + "." + Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Impuesto_Fraccionamiento_ID + ") Monto_Fraccionamiento, ";
                        Mi_SQL += "(SELECT " + Cat_Pre_Multas_Fraccionamientos_Detalles.Campo_Monto + " FROM " + Cat_Pre_Multas_Fraccionamientos_Detalles.Tabla_Cat_Pre_Multas_Fraccionamientos_Detalles + " WHERE " + Cat_Pre_Multas_Fraccionamientos_Detalles.Campo_Multa_Cuota_ID + " = " + Ope_Pre_Detalles_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Detalles_Impuestos_Fraccionamientos + "." + Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Multas_Id + ") AS MULTAS, ";
                        Mi_SQL += "(SELECT (SELECT " + Cat_Pre_Fraccionamientos.Campo_Descripcion + " FROM " + Cat_Pre_Fraccionamientos.Tabla_Cat_Pre_Fraccionamientos + " WHERE " + Cat_Pre_Fraccionamientos.Tabla_Cat_Pre_Fraccionamientos + "." + Cat_Pre_Fraccionamientos.Campo_Fraccionamiento_ID + "=" + Cat_Pre_Fracc_Impuestos.Tabla_Cat_Pre_Fracc_Impuestos + "." + Cat_Pre_Fracc_Impuestos.Campo_Fraccionamiento_ID + ")";
                        Mi_SQL += " FROM " + Cat_Pre_Fracc_Impuestos.Tabla_Cat_Pre_Fracc_Impuestos + " WHERE " + Cat_Pre_Fracc_Impuestos.Tabla_Cat_Pre_Fracc_Impuestos + "." + Cat_Pre_Fracc_Impuestos.Campo_Impuesto_Fraccionamiento_ID + "=" + Ope_Pre_Detalles_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Detalles_Impuestos_Fraccionamientos + "." + Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Impuesto_Fraccionamiento_ID + ") AS DESCRIPCION_MONTO, ";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Superficie_Fraccionar + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Multas_Id + " AS FRA_MULTA_CUOTA_ID, ";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Importe + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Recargos + ", ";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Total;
                        Mi_SQL += " FROM ";
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Detalles_Impuestos_Fraccionamientos;
                    }
                    Mi_SQL += " WHERE ";
                    if (Impuesto_Fraccionamiento.P_No_Impuesto_Fraccionamiento != null && Impuesto_Fraccionamiento.P_No_Impuesto_Fraccionamiento != "")
                    {
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + " = '" + Impuesto_Fraccionamiento.P_No_Impuesto_Fraccionamiento + "' AND ";
                    }
                    if (Impuesto_Fraccionamiento.P_Cuenta_Predial_ID != null && Impuesto_Fraccionamiento.P_Cuenta_Predial_ID != "")
                    {
                        Mi_SQL += Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + " IN (SELECT " + Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + " FROM " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + " WHERE " + Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID + " = '" + Impuesto_Fraccionamiento.P_Cuenta_Predial_ID + "') AND ";
                    }
                    if (Mi_SQL.EndsWith(" AND "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                    }
                    if (Mi_SQL.EndsWith(" WHERE "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                    }
                    Impuesto_Fraccionamiento.P_Dt_Detalles_Impuestos_Fraccionamiento = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Impuestos por Fraccionamientos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Impuesto_Fraccionamientos;
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
        ///NOMBRE DE LA FUNCIÓN : Consultar_Cuenta_Pendiente
        ///DESCRIPCIÓN          : Obtiene si la cuenta predial esta pendiente
        ///PARAMETROS           : 1. Convenio_Tras.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio
        ///CREO                 : Miguel Angel Bedolla Moreno
        ///FECHA_CREO           : 02/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Propietario(String No_Orden_Var, String Anio)
        {
            DataTable Dt_Propietario = new DataTable();
            String Mi_SQL = "";
            try
            {
                //Consulta las Parcialidades del Convenio
                Mi_SQL = "SELECT PROP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID + " AS CONTRIBUYENTE_ID, CONT.";
                Mi_SQL += Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + "||' '||CONT." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + "||' '||CONT." + Cat_Pre_Contribuyentes.Campo_Nombre + " AS NOMBRE_PROPIETARIO FROM "+Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion +" PROP LEFT OUTER JOIN "+Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes +" CONT ON PROP."+Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID+"=CONT."+Cat_Pre_Contribuyentes.Campo_Contribuyente_ID ;
                Mi_SQL += " WHERE (PROP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + "='PROPIETARIO' OR PROP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + "='POSEEDOR') AND PROP."+Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus+"='ALTA' AND PROP."+Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion+"='"+No_Orden_Var+"' AND PROP."+Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio+"="+Anio+"";

                Dt_Propietario = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los detalles de convenios por derechos de supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Propietario;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Impuesto_Con_Convenio
        ///DESCRIPCIÓN: Obtiene el impuesto en un convenio
        ///PARAMENTROS:   
        ///             1.  Impuesto.           Instancia de la clase de negocios que contiene el impuesto a buscar en la tabla de convenios
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 18/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Impuesto_Con_Convenio(Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio Impuesto)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio;
                Mi_SQL += " FROM " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos;
                Mi_SQL += " WHERE " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + "='" + Impuesto.P_No_Impuesto_Fraccionamiento + "'";
                Mi_SQL += " AND " + Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus + " IN ('ACTIVO','PENDIENTE')";

                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Convenios de Derechos de Supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }

        #endregion

    }

    
}