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

/// <summary>
/// Summary description for Cls_Ope_Pre_Recepcion_Pagos_Predial_Datos
/// </summary>

namespace Presidencia.Operacion_Predial_Recepcion_Pagos.Datos
{

    public class Cls_Ope_Pre_Recepcion_Pagos_Predial_Datos
    {

        #region "Metodos"

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cuentas_Predial
        ///DESCRIPCIÓN:Consulta los Datos de la Cuenta Predial que se Consulta
        ///PARAMETROS: Parametros. Trae los datos para realizar la Operación.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 22 Agosto 2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************      
        public static DataTable Consultar_Cuentas_Predial(Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio Parametros)
        {
            DataSet Ds_Datos = null;
            DataTable Dt_Datos = new DataTable();
            String Mi_SQL = null;
            Boolean Entro_Where = false;
            try
            {
                Mi_SQL = "SELECT " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " AS CUENTA_PREDIAL_ID";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " AS CUENTA_PREDIAL";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + " AS TIPO_PREDIO";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + " AS CUOTA_FIJA";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Propietario_ID + " AS PROPIETARIO_ID";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + "";
                Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + "";
                Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Nombre + " AS PROPIETARIO";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Nombre + "";
                Mi_SQL = Mi_SQL + " ||' '|| NVL(" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + ", 'S/N')";
                Mi_SQL = Mi_SQL + " ||' COL. '|| " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + "." + Cat_Ate_Colonias.Campo_Nombre + " AS UBICACION";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios;
                Mi_SQL = Mi_SQL + " ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "";
                Mi_SQL = Mi_SQL + " = " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + "";
                Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + " IN ('PROPIETARIO', 'POSEEDOR')";
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes;
                Mi_SQL = Mi_SQL + " ON " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + "";
                Mi_SQL = Mi_SQL + " = " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + "";
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles;
                Mi_SQL = Mi_SQL + " ON " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Calle_ID + "";
                Mi_SQL = Mi_SQL + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + "";
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
                Mi_SQL = Mi_SQL + " ON " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Colonia_ID + "";
                Mi_SQL = Mi_SQL + " = " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + "." + Cat_Ate_Colonias.Campo_Colonia_ID + "";
                if (Parametros.P_Cuenta_Predial != null)
                {
                    if (!Entro_Where)
                    {
                        Mi_SQL = Mi_SQL + " WHERE ";
                        Entro_Where = true;
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + " AND ";
                    }
                    Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Parametros.P_Cuenta_Predial + "'";
                }
                if (Parametros.P_Cuenta_Predial_ID != null)
                {
                    if (!Entro_Where)
                    {
                        Mi_SQL = Mi_SQL + " WHERE ";
                        Entro_Where = true;
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + " AND ";
                    }
                    Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = '" + Parametros.P_Cuenta_Predial_ID + "'";
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Adeudos_Cuenta_Predial
        ///DESCRIPCIÓN:Consulta de Adeudos para la Cuenta Predial.
        ///PARAMETROS: Parametros. Trae los datos para realizar la Operación.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23 Agosto 2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************   
        public static DataTable Consultar_Adeudos_Cuenta_Predial(Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio Parametros)
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
        public static DataTable Consultar_Clave_Ingreso(Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio Parametros)
        {
            DataSet Ds_Datos = null;
            DataTable Dt_Datos = new DataTable();
            String Mi_SQL = null;
            try
            {
                Mi_SQL = "SELECT " + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID + " AS CLAVE_INGRESO";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ingreso_Det.Campo_Tipo_Predial_Traslado + " = '" + Parametros.P_Clave_Ingreso + "'";
                if (String.IsNullOrEmpty(Parametros.P_Tipo_Clave_Ingreso))
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Claves_Ingreso_Det.Campo_Tipo + " = 'PREDIAL'";
                }
                else
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Claves_Ingreso_Det.Campo_Tipo + " = '" + Parametros.P_Tipo_Clave_Ingreso + "'";
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Dependencia
        ///DESCRIPCIÓN:Consulta la dependencia de una clave de Ingreso.
        ///PARAMETROS: Parametros. Trae los datos para realizar la Operación.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25 Agosto 2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************   
        public static DataTable Consultar_Dependencia(Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio Parametros)
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
        ///FECHA_CREO           : 25 Agosto 2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static void Alta_Pasivo(Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio Parametros)
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
                String Mi_SQL = "INSERT INTO " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                Mi_SQL = Mi_SQL + "(" + Ope_Ing_Pasivo.Campo_No_Pasivo;
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
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Contribuyente;
                Mi_SQL = Mi_SQL + ") VALUES(" + Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo, Ope_Ing_Pasivo.Campo_No_Pasivo, 10));
                Mi_SQL = Mi_SQL + ",'" + Parametros.P_Cuenta_Predial + "'";
                Mi_SQL = Mi_SQL + ",'" + Parametros.P_Clave_Ingreso + "'";
                Mi_SQL = Mi_SQL + ",'" + Parametros.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + ",'" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Tramite) + "'";
                Mi_SQL = Mi_SQL + ",'" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Vencimiento) + "'";
                Mi_SQL = Mi_SQL + ",'" + Parametros.P_Monto + "',0";
                Mi_SQL = Mi_SQL + ",'" + Parametros.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Parametros.P_No_Recibo + "'";
                Mi_SQL = Mi_SQL + ",'" + Parametros.P_Dependencia + "',SYSDATE,'" + Parametros.P_Usuario + "','" + Parametros.P_Cuenta_Predial_ID + "','" + Parametros.P_Contribuyente + "')";
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
        ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta
        ///DESCRIPCIÓN          : Consulta los montos de un convenio o reestructura según sea el caso
        ///PARAMETROS:     
        ///CREO                 : Miguel Angel Bedolla Moreno
        ///FECHA_CREO           : 21/Diciembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Obtener_Dato_Consulta(String cuenta_predial, Int32 Anio)
        {
            String Mi_SQL;
            DataTable Dt_Montos = new DataTable();

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ",0) as PAGO_1, ";
                Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ",0) as PAGO_2, ";
                Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ",0) as PAGO_3, ";
                Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ",0) as PAGO_4, ";
                Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ",0) as PAGO_5, ";
                Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ",0) as PAGO_6 ";
                Mi_SQL += "FROM " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + " WHERE " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + "='" + cuenta_predial + "' AND " + Ope_Pre_Adeudos_Predial.Campo_Anio + "=" + Anio;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Dt_Montos = dataset.Tables[0];
                }
            }
            catch
            {
            }
            finally
            {
            }

            return Dt_Montos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Eliminar_Pasivos_No_Pagados_Anteriormente
        ///DESCRIPCIÓN          : Elimina los pasivos cargados anteriormente.
        ///PARAMETROS           :  1.  Parametros. Trae los datos para realizar la Operación.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 16/Octubre/2010 [Domingo ¬¬]
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static void Eliminar_Pasivos_No_Pagados_Anteriormente(Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio Parametros)
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
                String Mi_SQL = "DELETE FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID + " = '" + Parametros.P_Cuenta_Predial_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Ing_Pasivo.Campo_Referencia + " = '" + Parametros.P_Cuenta_Predial + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Ing_Pasivo.Campo_Estatus + " = '" + Parametros.P_Estatus + "'";
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
        ///MODIFICO             : l
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
        ///NOMBRE DE LA FUNCIÓN: Obtener_Menu_De_Ruta
        ///DESCRIPCIÓN: Obtiene la ruta completa del Menu
        ///PARAMETROS: 1. Ruta. Datos para ejecutar la operacion.
        ///CREO: Francisco Antonio Gallardo Castañeda.  
        ///FECHA_CREO: 13 Octubre 2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static String Obtener_Menu_De_Ruta(String Ruta)
        {
            String Ruta_Completa = "";
            try
            {
                String Mi_SQL = "SELECT * FROM " + Apl_Cat_Menus.Tabla_Apl_Cat_Menus;
                Mi_SQL = Mi_SQL + " WHERE " + Apl_Cat_Menus.Campo_URL_Link + " = '" + Ruta + "'";
                DataSet Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Datos != null && Ds_Datos.Tables.Count > 0)
                {
                    DataTable Dt_Datos = Ds_Datos.Tables[0];
                    if (Dt_Datos != null && Dt_Datos.Rows.Count > 0)
                    {
                        Ruta_Completa = Dt_Datos.Rows[0][Apl_Cat_Menus.Campo_URL_Link].ToString() + "?PAGINA=" + Dt_Datos.Rows[0][Apl_Cat_Menus.Campo_Menu_ID].ToString();
                    }
                }
            }
            catch (OracleException Ex)
            {
                new Exception(Ex.Message);
            }
            return Ruta_Completa;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Convenio_Cuenta_Predia
        ///DESCRIPCIÓN: Consulta los convenios en base a una cuenta predial.
        ///PARAMENTROS:   
        ///             1.  Negocio. Contiene los Argumentos para realizar la Operación.
        ///CREO: Francisco Antonio Gallardo Castañeda. 
        ///FECHA_CREO: 26/Octubre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Convenio_Cuenta_Predia(Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio Negocio)
        {
            DataTable Dt_Datos = new DataTable();
            DataSet Ds_Datos = null;
            String Mi_SQL = null;
            try
            {
                Mi_SQL = "SELECT " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " AS NO_CONVENIO";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento + " AS FECHA_VENCIMIENTO";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + " AS NO_PAGO";
                Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_No_Descuento + ",'') AS NO_DESCUENTO";
                Mi_SQL = Mi_SQL + ", (" + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + "";
                Mi_SQL = Mi_SQL + " ||'/'|| " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Numero_Parcialidades + ") AS PARCIALIDAD";
                Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios + ", 0) AS RECARGOS_ORDINARIOS";
                Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios + ", 0) AS RECARGOS_MORATORIOS";
                Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios + ", 0) AS HONORARIOS";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo + " AS PERIODO";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " AS ESTATUS";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial;
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial;
                Mi_SQL = Mi_SQL + " ON " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_No_Convenio;
                Mi_SQL = Mi_SQL + " = " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio;
                Mi_SQL = Mi_SQL + " AND (" + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'POR PAGAR'";
                Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'INCUMPLIDO')";
                Mi_SQL = Mi_SQL + " WHERE (" + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Estatus + " = 'ACTIVO'";
                Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Estatus + " = 'INCUMPLIDO')";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id + " = '" + Negocio.P_Cuenta_Predial_ID + "'";
                Mi_SQL = Mi_SQL + " AND NVL(" + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ", 0) = (SELECT NVL(MAX(OPE_DET_CONV_PRE." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + "), 0) FROM " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + " OPE_DET_CONV_PRE WHERE OPE_DET_CONV_PRE." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " = " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_No_Convenio + ")";
                Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_No_Convenio;
                Mi_SQL = Mi_SQL + ", PARCIALIDAD";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento + "";
                Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Datos != null && Ds_Datos.Tables.Count > 0)
                {
                    Dt_Datos = Ds_Datos.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: [Consultar_Convenio_Cuenta_Predia:'" + Ex.Message + "']");
            }
            return Dt_Datos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Detalle_Parcialidades_Convenio
        ///DESCRIPCIÓN: Consulta a Detalle las parcialidades del convenio.
        ///PARAMENTROS:   
        ///             1.  Negocio. Contiene los Argumentos para realizar la Operación.
        ///CREO: Francisco Antonio Gallardo Castañeda. 
        ///FECHA_CREO: 28/Octubre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Detalle_Parcialidades_Convenio(Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio Negocio)
        {
            DataTable Dt_Datos = new DataTable();
            DataSet Ds_Datos = null;
            String Mi_SQL = null;
            try
            {
                Mi_SQL = "SELECT " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " AS NO_CONVENIO";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + " AS NO_PAGO";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Anio + " AS ANIO";
                Mi_SQL = Mi_SQL + ", (NVL(" + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_1 + ", 0)";
                Mi_SQL = Mi_SQL + " + NVL(" + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_2 + ", 0)";
                Mi_SQL = Mi_SQL + " + NVL(" + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_3 + ", 0)";
                Mi_SQL = Mi_SQL + " + NVL(" + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_4 + ", 0)";
                Mi_SQL = Mi_SQL + " + NVL(" + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_5 + ", 0)";
                Mi_SQL = Mi_SQL + " + NVL(" + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_6 + ", 0)) AS MONTO_ANIO";
                Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_1 + ", 0) AS BIMESTRE_1";
                Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_2 + ", 0) AS BIMESTRE_2";
                Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_3 + ", 0) AS BIMESTRE_3";
                Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_4 + ", 0) AS BIMESTRE_4";
                Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_5 + ", 0) AS BIMESTRE_5";
                Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_6 + ", 0) AS BIMESTRE_6";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial;
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial;
                Mi_SQL = Mi_SQL + " ON " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio;
                Mi_SQL = Mi_SQL + " = " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_No_Convenio;
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago;
                Mi_SQL = Mi_SQL + " = " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_No_Pago;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " = '" + Negocio.P_No_Convenio + "'";
                Mi_SQL = Mi_SQL + " AND NVL(" + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + ", 0) = (SELECT NVL(MAX(OPE_DET_CONV_PRE." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Reestructura + "), 0) FROM " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + " OPE_DET_CONV_PRE WHERE OPE_DET_CONV_PRE." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " = " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + ")";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + " IN (" + Negocio.P_No_Pagos + ")";
                Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago;
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Anio;


                //Mi_SQL = "SELECT " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_No_Convenio + " AS NO_CONVENIO";
                //Mi_SQL = Mi_SQL + ", " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_No_Pago + " AS NO_PAGO";
                //Mi_SQL = Mi_SQL + ", " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Anio + " AS ANIO";
                //Mi_SQL = Mi_SQL + ", (NVL(" + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_1 + ", 0)";
                //Mi_SQL = Mi_SQL + " + NVL(" + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_2 + ", 0)";
                //Mi_SQL = Mi_SQL + " + NVL(" + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_3 + ", 0)";
                //Mi_SQL = Mi_SQL + " + NVL(" + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_4 + ", 0)";
                //Mi_SQL = Mi_SQL + " + NVL(" + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_5 + ", 0)";
                //Mi_SQL = Mi_SQL + " + NVL(" + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_6 + ", 0)) AS MONTO_ANIO";
                //Mi_SQL = Mi_SQL + ", " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_1 + " AS BIMESTRE_1";
                //Mi_SQL = Mi_SQL + ", " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_2 + " AS BIMESTRE_2";
                //Mi_SQL = Mi_SQL + ", " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_3 + " AS BIMESTRE_3";
                //Mi_SQL = Mi_SQL + ", " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_4 + " AS BIMESTRE_4";
                //Mi_SQL = Mi_SQL + ", " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_5 + " AS BIMESTRE_5";
                //Mi_SQL = Mi_SQL + ", " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Bimestre_6 + " AS BIMESTRE_6";
                //Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial;
                //Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_No_Convenio + " = '" + Negocio.P_No_Convenio + "'";
                //Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_No_Pago + " IN (" + Negocio.P_No_Pagos + ")";
                //Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_No_Pago;
                //Mi_SQL = Mi_SQL + ", " + Ope_Pre_Desglose_Detalle_Convenios_Predial.Tabla_Ope_Pre_Des_Det_Conv_Predial + "." + Ope_Pre_Desglose_Detalle_Convenios_Predial.Campo_Anio;
                Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Datos != null && Ds_Datos.Tables.Count > 0)
                {
                    Dt_Datos = Ds_Datos.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: [Consultar_Convenio_Cuenta_Predia:'" + Ex.Message + "']");
            }
            return Dt_Datos;
        }

        #endregion

    }

}
