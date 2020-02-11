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
using Presidencia.Operacion_Predial_Recepcion_Pagos.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Descuentos_Predial.Datos;
using Presidencia.Descuentos_Predial.Negocio;

namespace Presidencia.Descuentos_Predial.Datos
{
    public class Cls_Ope_Pre_Descuentos_Predial_Datos
    {

        #region Metodos


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Descuentos_Predial
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo Descuento de Traslado
        ///PARAMETROS:    
        ///                            asi como de los datos necesarios a ingresar en la Tabla de Adeudos por Folio.
        ///CREO: Jacqueline Ramirez Sierra
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static bool Alta_Descuentos_Predial(Cls_Ope_Pre_Descuentos_Predial_Negocio Descuento)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            bool Alta = false;

            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;

            try
            {

                String No_Descuento_Predial = Obtener_ID_Consecutivo(Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial, Ope_Pre_Descuentos_Predial.Campo_No_Descuento_Predial, 10);
                String Mi_SQL;
                Mi_SQL = "INSERT INTO " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial
                    + " (" + Ope_Pre_Descuentos_Predial.Campo_No_Descuento_Predial
                    + ", " + Ope_Pre_Descuentos_Predial.Campo_Cuenta_Predial_ID
                    + ", " + Ope_Pre_Descuentos_Predial.Campo_Estatus
                    + ", " + Ope_Pre_Descuentos_Predial.Campo_Fecha
                    + ", " + Ope_Pre_Descuentos_Predial.Campo_Desc_Recargo
                    + ", " + Ope_Pre_Descuentos_Predial.Campo_Desc_Recargo_Moratorio
                    + ", " + Ope_Pre_Descuentos_Predial.Campo_Descuento_Pronto_Pago
                    + ", " + Ope_Pre_Descuentos_Predial.Campo_Total_a_Pagar
                    + ", " + Ope_Pre_Descuentos_Predial.Campo_Realizo
                    + ", " + Ope_Pre_Descuentos_Predial.Campo_Rezagos
                    + ", " + Ope_Pre_Descuentos_Predial.Campo_Corriente
                    + ", " + Ope_Pre_Descuentos_Predial.Campo_Recargos
                    + ", " + Ope_Pre_Descuentos_Predial.Campo_Recargos_Moratorios
                    + ", " + Ope_Pre_Descuentos_Predial.Campo_Honorarios
                    + ", " + Ope_Pre_Descuentos_Predial.Campo_Fecha_Vencimiento
                    + ", " + Ope_Pre_Descuentos_Predial.Campo_Observaciones
                    + ", " + Ope_Pre_Descuentos_Predial.Campo_Usuario_Creo
                    + ", " + Ope_Pre_Descuentos_Predial.Campo_Anio_Inicial
                    + ", " + Ope_Pre_Descuentos_Predial.Campo_Bimestre_Inicial
                    + ", " + Ope_Pre_Descuentos_Predial.Campo_Bimestre_Final
                    + ", " + Ope_Pre_Descuentos_Predial.Campo_Anio_Final
                    + ", " + Ope_Pre_Descuentos_Predial.Campo_Porcentaje_Recargo
                    + ", " + Ope_Pre_Descuentos_Predial.Campo_Porcentaje_Recargo_Moratorio
                    + ", " + Ope_Pre_Descuentos_Predial.Campo_Porcentaje_Pronto_Pago
                    + ", " + Ope_Pre_Descuentos_Predial.Campo_Constribuyente_ID
                    + ", " + Ope_Pre_Descuentos_Predial.Campo_Fecha_Creo + ")";
                Mi_SQL += " VALUES ('" + No_Descuento_Predial + "'"
                    + ",'" + Descuento.P_Cuenta_Predial_ID + "'"
                    + ",'" + Descuento.P_Estatus + "'"
                    + ", SYSDATE"
                    + ",'" + Descuento.P_Desc_Recargo.Replace(",", "") + "'"
                    + ",'" + Descuento.P_Desc_Moratorio.Replace(",", "") + "'"
                    + ",'" + Descuento.P_Descuento_Pronto_Pago.Replace(",", "") + "'"
                    + ",'" + Descuento.P_Total_Por_Pagar.Replace(",", "") + "'"
                    + ",'" + Descuento.P_Realizo.Replace(",", "") + "'"
                    + ",'" + Descuento.P_Rezagos.Replace(",", "") + "'"
                    + ",'" + Descuento.P_Corriente.Replace(",", "") + "'"
                    + ",'" + Descuento.P_Recargos.Replace(",", "") + "'"
                    + ",'" + Descuento.P_Recargos_Moratorios.Replace(",", "") + "'"
                    + ",'" + Descuento.P_Honorarios.Replace(",", "") + "'"
                    + ",'" + Descuento.P_Fecha_Vencimiento.ToString("d/M/yyyy") + "'"
                    + ",'" + Descuento.P_Observaciones + "'"
                    + ",'" + Descuento.P_Usuario + "'"
                    + ",'" + Descuento.P_Desde_Anio + "'"
                    + ",'" + Descuento.P_Desde_Bimestre + "'"
                    + ",'" + Descuento.P_Hasta_Bimestre + "'"
                    + ",'" + Descuento.P_Hasta_Anio + "'"
                    + ",'" + Descuento.P_Porcentaje_Recargo + "'"
                    + ",'" + Descuento.P_Porcentaje_Recargo_Moratorio + "'"
                    + ",'" + Descuento.P_Porcentaje_Pronto_Pago + "'"
                    + ",'" + Descuento.P_Contribuyente_ID + "'"
                    + ", SYSDATE)";


                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
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
                    Mensaje = "Error al intentar dar de Alta un Descuento de Traslado. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN : Modificar_Descuento_Predial
        ///DESCRIPCIÓN          : Modifica en la Base de Datos el registro indicado del Descuento predial
        ///PARAMETROS          : 1. Datos. Instancia de la Clase de Negocio de Cls_Ope_Pre_Descuentos_Predial_Negocio
        ///                         con los datos del Descuento que va a ser Modificado.
        ///CREO                 : Roberto González Oseguera
        ///FECHA_CREO           : 12-dic-2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Modificar_Descuento_Predial(Cls_Ope_Pre_Descuentos_Predial_Negocio Datos)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Boolean Actualizar = false;
            String Mi_SQL = "";

            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try
            {
                Mi_SQL = "UPDATE " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + " SET ";
                Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "', ";

                if (Datos.P_Realizo != null)
                {
                    Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Realizo + " = '" + Datos.P_Realizo + "', ";
                }
                if (Datos.P_Estatus != null)
                {
                    Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                }
                if (Datos.P_Fecha_Vencimiento != null)
                {
                    Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Fecha_Vencimiento + " = '" + Datos.P_Fecha_Vencimiento.ToString("d/MM/yyyy") + "', ";
                }
                if (Datos.P_Observaciones != null)
                {
                    Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Observaciones + " = '" + Datos.P_Observaciones + "', ";
                }
                if (Datos.P_Desc_Recargo != null)
                {
                    Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Desc_Recargo + " = '" + Datos.P_Desc_Recargo.Replace(",", "") + "', ";
                }
                if (Datos.P_Desc_Moratorio != null)
                {
                    Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Desc_Recargo_Moratorio + " = '" + Datos.P_Desc_Moratorio.Replace(",", "") + "', ";
                }
                if (Datos.P_Descuento_Pronto_Pago != null)
                {
                    Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Descuento_Pronto_Pago + " = '" + Datos.P_Descuento_Pronto_Pago.Replace(",", "") + "', ";
                }
                if (Datos.P_Total_Por_Pagar != null)
                {
                    Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Total_a_Pagar + " = '" + Datos.P_Total_Por_Pagar.Replace(",", "") + "', ";
                }
                if (Datos.P_Rezagos != null)
                {
                    Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Rezagos + " = '" + Datos.P_Rezagos.Replace(",", "") + "', ";
                }
                if (Datos.P_Corriente != null)
                {
                    Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Corriente + " = '" + Datos.P_Corriente.Replace(",", "") + "', ";
                }
                if (Datos.P_Recargos != null)
                {
                    Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Recargos + " = '" + Datos.P_Recargos.Replace(",", "") + "', ";
                }
                if (Datos.P_Recargos_Moratorios != null)
                {
                    Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Recargos_Moratorios + " = '" + Datos.P_Recargos_Moratorios.Replace(",", "") + "', ";
                }
                if (Datos.P_Honorarios != null)
                {
                    Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Honorarios + " = '" + Datos.P_Honorarios.Replace(",", "") + "', ";
                }
                if (Datos.P_Desde_Anio != null)
                {
                    Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Anio_Inicial + " = '" + Datos.P_Desde_Anio + "', ";
                }
                if (Datos.P_Desde_Bimestre != null)
                {
                    Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Bimestre_Inicial + " = '" + Datos.P_Desde_Bimestre + "', ";
                }
                if (Datos.P_Hasta_Anio != null)
                {
                    Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Anio_Final + " = '" + Datos.P_Hasta_Anio + "', ";
                }
                if (Datos.P_Hasta_Bimestre != null)
                {
                    Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Bimestre_Final + " = '" + Datos.P_Hasta_Bimestre + "', ";
                }
                if (Datos.P_Porcentaje_Recargo != null)
                {
                    Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Porcentaje_Recargo + " = '" + Datos.P_Porcentaje_Recargo + "', ";
                }
                if (Datos.P_Porcentaje_Recargo_Moratorio != null)
                {
                    Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Porcentaje_Recargo_Moratorio + " = '" + Datos.P_Porcentaje_Recargo_Moratorio + "', ";
                }
                if (Datos.P_Porcentaje_Pronto_Pago != null)
                {
                    Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Porcentaje_Pronto_Pago + " = '" + Datos.P_Porcentaje_Pronto_Pago + "', ";
                }
                Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Constribuyente_ID + " = '" + Datos.P_Contribuyente_ID + "', ";
                Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario + "', ";
                Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL += "WHERE " + Ope_Pre_Descuentos_Predial.Campo_No_Descuento_Predial + " = '" + Datos.P_No_Descuento + "'";
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Adeudos_Cuenta_Predial
        ///DESCRIPCIÓN:Consulta de Adeudos para la Cuenta Predial.
        ///PARAMETROS: Parametros. Trae los datos para realizar la Operación.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23 Agosto 2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************   
        public static DataTable Consultar_Adeudos_Cuenta_Predial(Cls_Ope_Pre_Descuentos_Predial_Negocio Parametros)
        {
            DataSet Ds_Datos = null;
            DataTable Dt_Datos = new DataTable();
            String Mi_SQL = null;
            try
            {
                Mi_SQL = "SELECT " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_No_Adeudo + " AS NO_ADEUDO";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Anio + " AS ANIO";
                Mi_SQL = Mi_SQL + ", (" + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + "";
                Mi_SQL = Mi_SQL + " - " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ") AS ADEUDO_BIMESTRE_1";
                Mi_SQL = Mi_SQL + ", (" + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + "";
                Mi_SQL = Mi_SQL + " - " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ") AS ADEUDO_BIMESTRE_2";
                Mi_SQL = Mi_SQL + ", (" + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + "";
                Mi_SQL = Mi_SQL + " - " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ") AS ADEUDO_BIMESTRE_3";
                Mi_SQL = Mi_SQL + ", (" + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + "";
                Mi_SQL = Mi_SQL + " - " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ") AS ADEUDO_BIMESTRE_4";
                Mi_SQL = Mi_SQL + ", (" + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + "";
                Mi_SQL = Mi_SQL + " - " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ") AS ADEUDO_BIMESTRE_5";
                Mi_SQL = Mi_SQL + ", (" + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + "";
                Mi_SQL = Mi_SQL + " - " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ") AS ADEUDO_BIMESTRE_6";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + " = '" + Parametros.P_Cuenta_Predial + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Adeudos_Predial.Campo_Estatus + " = 'POR PAGAR'";
                if (Parametros.P_Anio_Filtro > 0)
                {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Adeudos_Predial.Campo_Anio + " <= '" + Parametros.P_Anio_Filtro + "'";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pre_Adeudos_Predial.Campo_Anio + " ASC";
                Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if ((Ds_Datos != null) && (Ds_Datos.Tables.Count > 0))
                {
                    Dt_Datos = Ds_Datos.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Dt_Datos;
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Clave_Ingreso
        ///DESCRIPCIÓN:Consulta la clave de Ingreso.
        ///PARAMETROS: Parametros. Trae los datos para realizar la Operación.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25 Agosto 2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************   
        public static DataTable Consultar_Clave_Ingreso(Cls_Ope_Pre_Descuentos_Predial_Negocio Parametros)
        {
            DataSet Ds_Datos = null;
            DataTable Dt_Datos = new DataTable();
            String Mi_SQL = null;
            try
            {
                Mi_SQL = "SELECT " + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID + " AS CLAVE_INGRESO";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ingreso_Det.Campo_Tipo_Predial_Traslado + " = '" + Parametros.P_Clave_Ingreso + "'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Claves_Ingreso_Det.Campo_Tipo + " = 'PREDIAL'";
                Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if ((Ds_Datos != null) && (Ds_Datos.Tables.Count > 0))
                {
                    Dt_Datos = Ds_Datos.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Dt_Datos;
        }


        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Dependencia
        ///DESCRIPCIÓN:Consulta la dependencia de una clave de Ingreso.
        ///PARAMETROS: Parametros. Trae los datos para realizar la Operación.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25 Agosto 2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************   
        public static DataTable Consultar_Dependencia(Cls_Ope_Pre_Descuentos_Predial_Negocio Parametros)
        {
            DataSet Ds_Datos = null;
            DataTable Dt_Datos = new DataTable();
            String Mi_SQL = null;
            try
            {
                Mi_SQL = "SELECT " + Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID + " AS DEPENDENCIA";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + " = '" + Parametros.P_Clave_Ingreso + "'";
                Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if ((Ds_Datos != null) && (Ds_Datos.Tables.Count > 0))
                {
                    Dt_Datos = Ds_Datos.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Dt_Datos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Pasivo
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo registro Pasivo.
        ///PARAMETROS           : 
        ///                    1.  Parametros. Trae los datos para realizar la Operación.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 10/Diciembre/2010 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static void Alta_Pasivo(Cls_Ope_Pre_Descuentos_Predial_Negocio Parametros)
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
            String Raza_ID = Obtener_ID_Consecutivo(Cat_Pat_Razas.Tabla_Cat_Pat_Razas, Cat_Pat_Razas.Campo_Raza_ID, 5);
            try
            {
                String Mi_SQL = "INSERT INTO " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                Mi_SQL = Mi_SQL + "(" + Ope_Ing_Pasivo.Campo_No_Pasivo;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Contribuyente;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Referencia;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Descripcion;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Fecha_Ingreso;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Fecha_Vencimiento;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Monto;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Recargos;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_No_Recibo;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Fecha_Creo;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Usuario_Creo;
                Mi_SQL = Mi_SQL + ") VALUES(" + Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo, Ope_Ing_Pasivo.Campo_No_Pasivo, 10));
                Mi_SQL = Mi_SQL + ",'" + Parametros.P_Contribuyente + "'";
                Mi_SQL = Mi_SQL + ",'" + Parametros.P_Cuenta_Predial + "'";
                Mi_SQL = Mi_SQL + ",'" + Parametros.P_Clave_Ingreso + "'";
                Mi_SQL = Mi_SQL + ",'" + Parametros.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + ",'" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Tramite) + "'";
                Mi_SQL = Mi_SQL + ",'" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Vencimiento) + "'";
                Mi_SQL = Mi_SQL + ",'" + Parametros.P_Monto + "',0";
                Mi_SQL = Mi_SQL + ",'" + Parametros.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Parametros.P_No_Recibo + "'";
                Mi_SQL = Mi_SQL + ",'" + Parametros.P_Dependencia + "',SYSDATE,'" + Parametros.P_Usuario + "')";
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
                    Mensaje = "Error al intentar dar de Alta una Raza. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///PARAMETROS:     
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
        ///PARAMETROS:     
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Descuentos_Traslado
        ///DESCRIPCIÓN: Modificar en la Base de Datos un Descuento de Traslado
        ///PARAMETROS:     
        ///             1. Descuento.  Instancia de la Clase de Negocio de Descuentos de Traslado 
        ///                            con los datos del Descuento de Traslado que va a ser Modificado
        ///CREO: Jacqueline Ramirez Sierra
        ///FECHA_CREO: 15/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Descuentos_Predial(Cls_Ope_Pre_Descuentos_Predial_Negocio Descuento)
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
                String Mi_SQL = "UPDATE " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + " SET ";
                Mi_SQL = Mi_SQL + Ope_Pre_Descuentos_Predial.Campo_Estatus + " = '" + Descuento.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pre_Descuentos_Predial.Campo_Desc_Recargo + " = '" + Descuento.P_Desc_Recargo + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pre_Descuentos_Predial.Campo_Desc_Recargo_Moratorio + " = '" + Descuento.P_Desc_Moratorio + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pre_Descuentos_Predial.Campo_Realizo + " = '" + Descuento.P_Realizo + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pre_Descuentos_Predial.Campo_Fecha_Vencimiento + " = '" + Descuento.P_Fecha_Vencimiento + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pre_Descuentos_Predial.Campo_Observaciones + " = '" + Descuento.P_Observaciones + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pre_Descuentos_Predial.Campo_Usuario_Modifico + " = '" + Descuento.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pre_Descuentos_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Descuentos_Predial.Campo_No_Descuento_Predial + " = '" + Descuento.P_No_Descuento + "'";
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
                    Mensaje = "Error al intentar Modificar un Descuento de Traslado. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Descuentos_Predial
        ///DESCRIPCIÓN:Consulta los Datos de Descuento de la Cuenta Predial que se selecciona
        ///PARAMETROS: Cuenta Predial
        ///CREO:Jacqueline Ramírez Sierra
        ///FECHA_CREO: 23 Septiembre 2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************      
        public static DataTable Consultar_Descuentos_Predial(Cls_Ope_Pre_Descuentos_Predial_Negocio Parametros)
        {
            DataSet Ds_Datos = null;
            DataTable Dt_Datos = new DataTable();
            String Mi_SQL = null;
            try
            {
                Mi_SQL = "SELECT " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Cuenta_Predial_ID + " AS CUENTA_PREDIAL_ID"
                    + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_No_Descuento_Predial
                    + ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " AS CUENTA_PREDIAL"
                    + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Estatus + " AS ESTATUS"
                    + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Total_Impuesto + " AS TOTAL_IMPUESTO"
                    + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Desc_Recargo + " AS DESCUENTO_RECARGO"
                    + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Desc_Recargo_Moratorio + " AS DESCUENTO_RECARGO_MORATORIO"
                    + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Descuento_Pronto_Pago
                    + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Desc_Multa + " AS DESCUENTO_MULTA"
                    + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Total_a_Pagar + " AS TOTAL_PAGAR"
                    + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Anio_Inicial + " AS DESDE_PERIODO_ANIO"
                    + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Anio_Final + " AS HASTA_PERIODO_ANIO"
                    + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Fecha_Vencimiento
                    + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Observaciones
                    + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Corriente
                    + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Rezagos
                    + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Recargos
                    + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Recargos_Moratorios
                    + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Honorarios
                    + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Constribuyente_ID
                    + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Usuario_Creo
                    + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Bimestre_Inicial + " AS DESDE_PERIODO_BIMESTRE"
                    + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Bimestre_Final + " AS HASTA_PERIODO_BIMESTRE"
                    + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Porcentaje_Multa + " AS PORCENTAJE_MULTA"
                    + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Porcentaje_Recargo + " AS PORCENTAJE_RECARGO"
                    + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Porcentaje_Recargo_Moratorio + " AS PORCENTAJE_RECARGO_MORATORIO"
                    + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Porcentaje_Pronto_Pago
                    + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial
                    + " WHERE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ""
                    + " = " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Cuenta_Predial_ID + ""
                    + " AND " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Cuenta_Predial_ID + "='";
                Mi_SQL = Mi_SQL + Parametros.P_Cuenta_Predial + "'";
                if (Parametros.P_No_Descuento != null && Parametros.P_No_Descuento.Trim() != "")
                {
                    Mi_SQL += " AND " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_No_Descuento_Predial + " = '" + Parametros.P_No_Descuento.Trim() + "'";
                }
                else
                {
                    Mi_SQL += " ORDER BY " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_No_Descuento_Predial + " DESC";
                }
                Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if ((Ds_Datos != null) && (Ds_Datos.Tables.Count > 0))
                {
                    Dt_Datos = Ds_Datos.Tables[0];
                }

            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Dt_Datos;
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Descuentos_Predial
        ///DESCRIPCIÓN:Consulta los Datos de Descuento de la Cuenta Predial que se selecciona
        ///PARAMETROS: Cuenta Predial
        ///CREO:Jacqueline Ramírez Sierra
        ///FECHA_CREO: 23 Septiembre 2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************      
        public static DataTable Consultar_Descuentos_Prediales(Cls_Ope_Pre_Descuentos_Predial_Negocio Parametros)
        {
            DataSet Ds_Datos = null;
            DataTable Dt_Datos = new DataTable();
            String Mi_SQL = null;
            try
            {

                Mi_SQL = "SELECT " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " AS CUENTA_PREDIAL_ID";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " AS CUENTA_PREDIAL";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Propietario_ID + " AS PROPIETARIO_ID";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + "";
                Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + "";
                Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Nombre + " AS PROPIETARIO";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Nombre + "";
                Mi_SQL = Mi_SQL + " ||' '|| NVL(" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + ", 'S/N')";
                Mi_SQL = Mi_SQL + " ||' COL. '|| " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + "." + Cat_Ate_Colonias.Campo_Nombre + " AS UBICACION";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_No_Descuento_Predial + " AS NO_DESCUENTO_PREDIAL";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Estatus + " AS ESTATUS";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Total_Impuesto + " AS TOTAL_IMPUESTO";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Desc_Recargo + " AS DESCUENTO_RECARGO";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Desc_Recargo_Moratorio + " AS DESCUENTO_RECARGO_MORATORIO";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Descuento_Pronto_Pago;
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Desc_Multa + " AS DESCUENTO_MULTA";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Total_a_Pagar + " AS TOTAL_PAGAR";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Anio_Inicial + " AS DESDE_PERIODO_ANIO";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Anio_Final + " AS HASTA_PERIODO_ANIO";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Bimestre_Inicial + " AS DESDE_PERIODO_BIMESTRE";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Bimestre_Final + " AS HASTA_PERIODO_BIMESTRE";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Porcentaje_Multa + " AS PORCENTAJE_MULTA";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Porcentaje_Recargo + " AS PORCENTAJE_RECARGO";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Porcentaje_Recargo_Moratorio + " AS PORCENTAJE_RECARGO_MORATORIO";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Porcentaje_Pronto_Pago;
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Fecha_Vencimiento + " AS VENCIMIENTO";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Usuario_Creo + " AS REALIZO";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Observaciones + " AS OBSERVACIONES";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Rezagos + " AS REZAGOS";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Corriente + " AS CORRIENTE";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Recargos + " AS RECARGOS";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Recargos_Moratorios + " AS RECARGOS_MORATORIOS";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Honorarios;
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Multas + " AS MULTAS";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + ", " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + ", " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles;
                Mi_SQL = Mi_SQL + ", " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + ", " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "";
                Mi_SQL = Mi_SQL + " = " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + "";
                Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + "";
                Mi_SQL = Mi_SQL + " = " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + "";
                Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + " = 'PROPIETARIO'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Calle_ID + "";
                Mi_SQL = Mi_SQL + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + "";
                Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Colonia_ID + "";
                Mi_SQL = Mi_SQL + " = " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + "." + Cat_Ate_Colonias.Campo_Colonia_ID + "";
                //Mi_SQL = Mi_SQL + Parametros.P_Cuenta_Predial + "'";
                //Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " ='" + Parametros.P_Cuenta_Predial.Trim() + "'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "";
                Mi_SQL = Mi_SQL + " = " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Cuenta_Predial_ID + "";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + "." + Ope_Pre_Descuentos_Predial.Campo_Estatus + "= 'VIGENTE'";

                Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if ((Ds_Datos != null) && (Ds_Datos.Tables.Count > 0))
                {
                    Dt_Datos = Ds_Datos.Tables[0];
                }

            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Dt_Datos;
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Descuentos_Predial
        ///DESCRIPCIÓN:Consulta los Datos de Descuento de la Cuenta Predial que se selecciona
        ///PARAMETROS: Cuenta Predial
        ///CREO:Jacqueline Ramírez Sierra
        ///FECHA_CREO: 23 Septiembre 2011
        ///MODIFICO: Roberto González Oseguera
        ///FECHA_MODIFICO: 
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************      
        public static DataTable Consultar_Descuentos_Predial_Busqueda(Cls_Ope_Pre_Descuentos_Predial_Negocio Parametros)
        {
            DataSet Ds_Datos = null;
            DataTable Dt_Datos = new DataTable();
            String Mi_SQL = null;
            // subconsulta de cuenta predial
            Mi_SQL = "SELECT"
                + " CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial
                + ", DESCUENTO." + Ope_Pre_Descuentos_Predial.Campo_No_Descuento_Predial
                + ", DESCUENTO." + Ope_Pre_Descuentos_Predial.Campo_Cuenta_Predial_ID
                + ", DESCUENTO." + Ope_Pre_Descuentos_Predial.Campo_Estatus
                + ", DESCUENTO." + Ope_Pre_Descuentos_Predial.Campo_Fecha
                + ", DESCUENTO." + Ope_Pre_Descuentos_Predial.Campo_Fecha_Vencimiento
                + ", DESCUENTO." + Ope_Pre_Descuentos_Predial.Campo_Usuario_Creo
                + ", DESCUENTO." + Ope_Pre_Descuentos_Predial.Campo_Porcentaje_Recargo
                + ", DESCUENTO." + Ope_Pre_Descuentos_Predial.Campo_Porcentaje_Recargo_Moratorio
                + ", DESCUENTO." + Ope_Pre_Descuentos_Predial.Campo_Porcentaje_Pronto_Pago
                + ", DESCUENTO." + Ope_Pre_Descuentos_Predial.Campo_Desc_Recargo
                + ", DESCUENTO." + Ope_Pre_Descuentos_Predial.Campo_Desc_Recargo_Moratorio
                + ", DESCUENTO." + Ope_Pre_Descuentos_Predial.Campo_Descuento_Pronto_Pago
                + ", DESCUENTO." + Ope_Pre_Descuentos_Predial.Campo_Recargos
                + ", DESCUENTO." + Ope_Pre_Descuentos_Predial.Campo_Recargos_Moratorios
                + ", DESCUENTO." + Ope_Pre_Descuentos_Predial.Campo_Honorarios
                + ", DESCUENTO." + Ope_Pre_Descuentos_Predial.Campo_Total_Impuesto
                + ", DESCUENTO." + Ope_Pre_Descuentos_Predial.Campo_Total_a_Pagar;
            Mi_SQL += " FROM " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial + " DESCUENTO "
                + " JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUENTA "
                + " ON DESCUENTO." + Ope_Pre_Descuentos_Predial.Campo_Cuenta_Predial_ID
                + " = CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
            // si se proporcionan filtros para la consulta
            if (!String.IsNullOrEmpty(Parametros.P_Filtros_Dinamicos))
            {
                Mi_SQL += " WHERE " + Parametros.P_Filtros_Dinamicos;
            }
            else if (!String.IsNullOrEmpty(Parametros.P_Cuenta_Predial))
            {
                Mi_SQL += " WHERE CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial
                    + " = '" + Parametros.P_Cuenta_Predial + "'";
            }
            else if (!String.IsNullOrEmpty(Parametros.P_Estatus))
            {
                Mi_SQL += " WHERE DESCUENTO." + Ope_Pre_Descuentos_Predial.Campo_Estatus
                    + " = '" + Parametros.P_Estatus + "'";
            }

            try
            {
                Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if ((Ds_Datos != null) && (Ds_Datos.Tables.Count > 0))
                {
                    Dt_Datos = Ds_Datos.Tables[0];
                }

            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Dt_Datos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Descuento
        ///DESCRIPCIÓN: Obtiene el porcentaje del descuento que un Usuario puede aplicar.
        ///PARAMETROS:   
        ///             1. Descuento.   Usuario que realiza el Descuento.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 15/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static String Consultar_Descuento_Rango(Cls_Ope_Pre_Descuentos_Predial_Negocio Descuento)
        {
            Object Resultado;
            String Desc = "0";
            try
            {
                String Mi_SQL = "SELECT NVL(" + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Porcentaje + ", '0') AS PORCENTAJE"
                    + " FROM " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Tabla_Cat_Pre_Rangos_De_Descuento_Por_Rol
                    + " WHERE " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Estatus + " = 'VIGENTE' ";
                // si se especifico un tipo, agregar a la consulta
                if (Descuento.P_Tipo_Descuento != "")
                {
                    Mi_SQL += " AND " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Tipo + " = '" + Descuento.P_Tipo_Descuento + "'";
                }
                Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Empleado_Id + " = '" + Descuento.P_Usuario + "'";

                Resultado = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Resultado != null)
                {
                    Desc = Resultado.ToString();
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Descuentos de Trasladado. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Desc;
        }

        #endregion

    }

}


