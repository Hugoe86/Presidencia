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
using Presidencia.Sessiones;
using System.Text;
using Presidencia.Operacion_Cancelacion_Pago.Negocio;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Caja_Pagos.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Cuotas_Minimas.Negocio;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Presidencia.Operaciones_Apertura_Turnos.Negocio;
using Presidencia.Predial_Pae_Honorarios.Negocio;

/// <summary>
/// Summary description for Cls_Ope_Pre_Cancelacion_Pago
/// </summary>

namespace Presidencia.Operacion_Cancelacion_Pago.Datos
{
    public class Cls_Ope_Pre_Cancelacion_Pago_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Cancelacion_Pago
        ///DESCRIPCIÓN: Actualiza en la Base de Datos una Cancelacion de Pago
        ///PARAMENTROS:     
        ///             1. Cancelacion.     Instancia de la Clase de Salarios Mínimos 
        ///                                 con los datos del Registro 
        ///                                 que va a ser Actualizado.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 20/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Cancelacion_Pago(Cls_Ope_Pre_Cancelacion_Pago_Negocio Cancelacion)
        {
            String Mensaje = "";
            StringBuilder Mi_SQL_Detalles = new StringBuilder();//Variable que almacenara la consulta.
            String Mi_SQL_PASIVO = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Boolean Bandero_Entro = false;
            Object No_Pasivo;
            Object No_Pago;
            Object No_Operacion;
            DataTable Dt_Pago = new DataTable();
            DataTable Dt_Consulta = new DataTable();

            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try
            {
                String Mi_SQL = "";
                //    = "UPDATE " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos ;
                //Mi_SQL = Mi_SQL + " SET " + Ope_Caj_Pagos.Campo_Comentarios + " = '" + Cancelacion.P_Comentarios + "'";
                //Mi_SQL = Mi_SQL + "," + Ope_Caj_Pagos.Campo_Motivo_Cancelacion_ID  + " = '" + Cancelacion.P_Motivo_Cancelacion_Id + "'";
                //Mi_SQL = Mi_SQL + "," + Ope_Caj_Pagos.Campo_Fecha_Cancelacion + " = sysdate";
                //Mi_SQL = Mi_SQL + "," + Ope_Caj_Pagos.Campo_Usuario_Modifico  + " = '" + Cancelacion.P_Usuario + "'";
                //Mi_SQL = Mi_SQL + "," + Ope_Caj_Pagos.Campo_Fecha_Modifico + " = sysdate";
                //Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Pagos.Campo_No_Pago + " = '" + Cancelacion.P_No_Pago + "'";
                //Cmd.CommandText = Mi_SQL;
                //Cmd.ExecuteNonQuery();

                //Consulta el último no de pago que fue registrado en la base de datos
                Mi_SQL = "SELECT MAX(" + Ope_Caj_Pagos.Campo_No_Pago + ")";
                Mi_SQL += " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                Cmd.CommandText = Mi_SQL.ToString();
                No_Pago = Cmd.ExecuteOracleScalar().ToString();
                if (Convert.IsDBNull(No_Pago))
                {
                    No_Pago = "0000000001";
                }
                else
                {
                    No_Pago = (Convert.ToInt64(No_Pago) + 1).ToString("0000000000");
                }

                //Consulta los pasivos para registrar el nuevo
                Mi_SQL = "SELECT * FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                Mi_SQL += " WHERE " + Ope_Caj_Pagos.Campo_No_Pago + " = '" + Cancelacion.P_No_Pago + "'";
                Cmd.CommandText = Mi_SQL.ToString();
                OracleDataAdapter Da_Pago = new OracleDataAdapter(Cmd);
                DataSet Ds_Pago = new DataSet();
                Da_Pago.Fill(Ds_Pago);
                Dt_Pago = Ds_Pago.Tables[0];

                //Consulta el último no de pago que fue registrado en la base de datos
                Mi_SQL = "SELECT MAX(" + Ope_Caj_Pagos.Campo_No_Operacion + ")";
                Mi_SQL += " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                Mi_SQL += " WHERE " + Ope_Caj_Pagos.Campo_Caja_ID + " = '" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Caja_ID].ToString() + "'";
                Mi_SQL += " AND " + Ope_Caj_Pagos.Campo_Fecha + " BETWEEN TO_DATE ('" + String.Format("{0:dd-MM-yyy}", Convert.ToDateTime(Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Fecha])) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')";
                Mi_SQL += " AND TO_DATE ('" + String.Format("{0:dd-MM-yyy}", Convert.ToDateTime(Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Fecha])) + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                Cmd.CommandText = Mi_SQL.ToString();
                No_Operacion = Cmd.ExecuteOracleScalar().ToString();
                if (Convert.IsDBNull(No_Operacion))
                {
                    No_Operacion = Convert.ToInt32("1");
                }
                else
                {
                    No_Operacion = Convert.ToInt32(No_Operacion) + 1;
                }

                Mi_SQL = " INSERT INTO " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " (";
                Mi_SQL += Ope_Caj_Pagos.Campo_No_Pago + ", " + Ope_Caj_Pagos.Campo_No_Recibo + ", " + Ope_Caj_Pagos.Campo_No_Operacion + ", " + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + ", " + Ope_Caj_Pagos.Campo_Caja_ID + ", " + Ope_Caj_Pagos.Campo_Fecha + ", " + Ope_Caj_Pagos.Campo_Clave_Banco + ", " + Ope_Caj_Pagos.Campo_Documento + ", " + Ope_Caj_Pagos.Campo_Usuario_Creo + ", " + Ope_Caj_Pagos.Campo_Fecha_Creo + ", " + Ope_Caj_Pagos.Campo_Usuario_Modifico + ", " + Ope_Caj_Pagos.Campo_Fecha_Modifico + ", " + Ope_Caj_Pagos.Campo_Motivo_Cancelacion_ID + ", " + Ope_Caj_Pagos.Campo_Estatus + ", " + Ope_Caj_Pagos.Campo_Comentarios + ", " + Ope_Caj_Pagos.Campo_Observaciones + ", " + Ope_Caj_Pagos.Campo_Monto_Corriente + ", " + Ope_Caj_Pagos.Campo_Monto_Rezago + ", " + Ope_Caj_Pagos.Campo_Monto_Recargos + ", " + Ope_Caj_Pagos.Campo_Monto_Recargos_Moratorios + ", " + Ope_Caj_Pagos.Campo_Honorarios + ", " + Ope_Caj_Pagos.Campo_Multas + ", " + Ope_Caj_Pagos.Campo_Gastos_Ejecucion + ", " + Ope_Caj_Pagos.Campo_Descuento_Recargos + ", " + Ope_Caj_Pagos.Campo_Descuento_Honorarios + ", " + Ope_Caj_Pagos.Campo_Descuento_Pronto_Pago + ", " + Ope_Caj_Pagos.Campo_No_Turno + ", " + Ope_Caj_Pagos.Campo_Folio + ", " + Ope_Caj_Pagos.Campo_Clave_Ingreso_ID + ", " + Ope_Caj_Pagos.Campo_Empleado_ID + ", " + Ope_Caj_Pagos.Campo_Ajuste_Tarifario + ", " + Ope_Caj_Pagos.Campo_Periodo_Corriente + ", " + Ope_Caj_Pagos.Campo_Periodo_Rezago + ", " + Ope_Caj_Pagos.Campo_Descuento_Multas + ", " + Ope_Caj_Pagos.Campo_Tipo_Pago + ", " + Ope_Caj_Pagos.Campo_Total + ", " + Ope_Caj_Pagos.Campo_Fecha_Cancelacion + ", " + Ope_Caj_Pagos.Campo_No_Convenio + ") VALUES(";

                Mi_SQL += "'" + No_Pago + "', ";
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_No_Recibo].ToString() != "")
                {
                    Mi_SQL += "" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_No_Recibo].ToString() + ", ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                Mi_SQL += No_Operacion + ", ";
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Cuenta_Predial_ID].ToString() != "")
                {
                    Mi_SQL += "'" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Cuenta_Predial_ID].ToString() + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Caja_ID].ToString() != "")
                {
                    Mi_SQL += "'" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Caja_ID].ToString() + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Fecha].ToString() != "")
                {
                    Mi_SQL += "'" + Convert.ToDateTime(Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Fecha].ToString()).ToString("dd-MM-yyyy") + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Clave_Banco].ToString() != "")
                {
                    Mi_SQL += "'" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Clave_Banco].ToString() + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Documento].ToString() != "")
                {
                    Mi_SQL += "'" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Documento].ToString() + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }

                Mi_SQL += "'" + Cls_Sessiones.Nombre_Empleado + "', ";

                Mi_SQL += "SYSDATE, ";

                Mi_SQL += "NULL, ";
                Mi_SQL += "NULL, ";
                Mi_SQL += "'" + Cancelacion.P_Motivo_Cancelacion_Id + "', ";
                Mi_SQL += "'CANCELADO', ";
                Mi_SQL += "'" + Cancelacion.P_Comentarios.ToUpper() + "', ";
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Observaciones].ToString() != "")
                {
                    Mi_SQL += "'" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Observaciones].ToString() + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Monto_Corriente].ToString() != "")
                {
                    Mi_SQL += "" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Monto_Corriente].ToString() + " *-1, ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Monto_Rezago].ToString() != "")
                {
                    Mi_SQL += "" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Monto_Rezago].ToString() + " *-1, ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Monto_Recargos].ToString() != "")
                {
                    Mi_SQL += "" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Monto_Recargos].ToString() + " *-1, ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Monto_Recargos_Moratorios].ToString() != "")
                {
                    Mi_SQL += "" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Monto_Recargos_Moratorios].ToString() + " *-1, ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Honorarios].ToString() != "")
                {
                    Mi_SQL += "" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Honorarios].ToString() + " *-1, ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Multas].ToString() != "")
                {
                    Mi_SQL += "" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Multas].ToString() + " *-1, ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Gastos_Ejecucion].ToString() != "")
                {
                    Mi_SQL += "" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Gastos_Ejecucion].ToString() + " *-1, ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Descuento_Recargos].ToString() != "")
                {
                    Mi_SQL += "" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Descuento_Recargos].ToString() + " *-1, ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Descuento_Honorarios].ToString() != "")
                {
                    Mi_SQL += "" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Descuento_Honorarios].ToString() + " *-1, ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Descuento_Pronto_Pago].ToString() != "")
                {
                    Mi_SQL += "" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Descuento_Pronto_Pago].ToString() + " *-1, ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_No_Turno].ToString() != "")
                {
                    Mi_SQL += "'" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_No_Turno].ToString() + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Folio].ToString() != "")
                {
                    Mi_SQL += "'" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Folio].ToString() + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Clave_Ingreso_ID].ToString() != "")
                {
                    Mi_SQL += "'" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Clave_Ingreso_ID].ToString() + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Empleado_ID].ToString() != "")
                {
                    Mi_SQL += "'" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Empleado_ID].ToString() + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Ajuste_Tarifario].ToString() != "")
                {
                    Mi_SQL += "" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Ajuste_Tarifario].ToString() + " *-1, ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Periodo_Corriente].ToString() != "")
                {
                    Mi_SQL += "'" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Periodo_Corriente].ToString() + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Periodo_Rezago].ToString() != "")
                {
                    Mi_SQL += "'" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Periodo_Rezago].ToString() + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Descuento_Multas].ToString() != "")
                {
                    Mi_SQL += "" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Descuento_Multas].ToString() + " *-1, ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Tipo_Pago].ToString() != "")
                {
                    Mi_SQL += "'" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Tipo_Pago].ToString() + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Total].ToString() != "")
                {
                    Mi_SQL += "" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_Total].ToString() + " *-1, ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                Mi_SQL += "SYSDATE, ";
                if (Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_No_Convenio].ToString() != "")
                {
                    Mi_SQL += "'" + Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_No_Convenio].ToString() + "' ";
                }
                else
                {
                    Mi_SQL += "NULL ";
                }
                Mi_SQL += ")";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //Cls_Ope_Pre_Apertura_Turno_Negocio Apertura_Turno = new Cls_Ope_Pre_Apertura_Turno_Negocio(); //Variable de conexion hacia la capa de negocio

                //Apertura_Turno.P_No_Turno = Dt_Pago.Rows[0][Ope_Caj_Pagos.Campo_No_Turno].ToString();
                //DataTable Dt_Formas_Pago_Turno = Apertura_Turno.Consultar_Formas_Pago_Turno(); //Consulta el monto total que fue pago en las diferentes formas de pago
                Object Consecutivo;

                //Consulta los pasivos para registrar el nuevo
                Mi_SQL = "SELECT " + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago;
                Mi_SQL += ", " + Ope_Caj_Pagos_Detalles.Campo_Monto;
                Mi_SQL += ", " + Ope_Caj_Pagos_Detalles.Campo_Banco_ID;
                Mi_SQL += ", " + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago;
                Mi_SQL += ", " + Ope_Caj_Pagos_Detalles.Campo_No_Pago;
                Mi_SQL += ", " + Ope_Caj_Pagos_Detalles.Campo_No_Cheque;
                Mi_SQL += ", " + Ope_Caj_Pagos_Detalles.Campo_No_Transaccion;
                Mi_SQL += ", " + Ope_Caj_Pagos_Detalles.Campo_No_Tarjeta_Bancaria;
                Mi_SQL += ", " + Ope_Caj_Pagos_Detalles.Campo_No_Autorizacion;
                Mi_SQL += ", " + Ope_Caj_Pagos_Detalles.Campo_Plan_Pago;
                Mi_SQL += ", " + Ope_Caj_Pagos_Detalles.Campo_Meses;
                Mi_SQL += ", " + Ope_Caj_Pagos_Detalles.Campo_Referencia_Transferencia;
                Mi_SQL += " FROM " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles;
                Mi_SQL += " WHERE " + Ope_Caj_Pagos_Detalles.Campo_No_Pago + " IN (SELECT " + Ope_Caj_Pagos.Campo_No_Pago + " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " WHERE " + Ope_Caj_Pagos.Campo_No_Recibo + " = '" + Cancelacion.P_No_Recibo + "')";
                Cmd.CommandText = Mi_SQL.ToString();
                OracleDataAdapter Da = new OracleDataAdapter(Cmd);
                DataSet Ds = new DataSet();
                Da.Fill(Ds);
                DataTable Dt_Formas_Pago_Turno = Ds.Tables[0];

                //Agrega los valores de los campos a los controles correspondientes de la forma
                foreach (DataRow Registro in Dt_Formas_Pago_Turno.Rows)
                {
                    Mi_SQL = "";
                    //Consulta el último no de pago que fue registrado en la base de datos
                    Mi_SQL += "SELECT NVL(MAX(" + Ope_Caj_Pagos_Detalles.Campo_Consecutivo + "),0)";
                    Mi_SQL += " FROM " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles;
                    Cmd.CommandText = Mi_SQL.ToString();
                    Consecutivo = Cmd.ExecuteOracleScalar().ToString();
                    //Consecutivo = OracleHelper.ExecuteScalar(Transaccion_SQL, CommandType.Text, Mi_SQL.ToString());

                    if (Convert.IsDBNull(Consecutivo))
                    {
                        Consecutivo = 1;
                    }
                    else
                    {
                        Consecutivo = Convert.ToInt64(Consecutivo) + 1;
                    }
                    Mi_SQL = "";
                    if (Registro["Forma_Pago"].ToString() == "EFECTIVO") //Forma de Pago en Efectivo
                    {
                        //Inserción de forma de pago en la base de datos
                        Mi_SQL += "INSERT INTO " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "(";
                        Mi_SQL += Ope_Caj_Pagos_Detalles.Campo_No_Pago + ", " + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + ", ";
                        Mi_SQL += Ope_Caj_Pagos_Detalles.Campo_Monto + ", " + Ope_Caj_Pagos_Detalles.Campo_Consecutivo + ")";
                        Mi_SQL += " VALUES ('" + No_Pago + "', '" + Registro["Forma_Pago"].ToString() + "', ";
                        Mi_SQL += Convert.ToDecimal(Registro["Monto"].ToString()) * -1 + ", " + Consecutivo + ")";
                    }
                    else //Forma de Pago en Banco
                    {
                        if (Registro["Forma_Pago"].ToString() == "BANCO")
                        {
                            //Inserción de forma de pago en la base de datos
                            Mi_SQL += "INSERT INTO " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "(";
                            Mi_SQL += Ope_Caj_Pagos_Detalles.Campo_No_Pago + ", " + Ope_Caj_Pagos_Detalles.Campo_Banco_ID + ", ";
                            Mi_SQL += Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + ", " + Ope_Caj_Pagos_Detalles.Campo_No_Transaccion + ", ";
                            Mi_SQL += Ope_Caj_Pagos_Detalles.Campo_No_Autorizacion + ", " + Ope_Caj_Pagos_Detalles.Campo_Plan_Pago + ", ";
                            Mi_SQL += Ope_Caj_Pagos_Detalles.Campo_Meses + ", " + Ope_Caj_Pagos_Detalles.Campo_Monto + ", ";
                            Mi_SQL += Ope_Caj_Pagos_Detalles.Campo_No_Tarjeta_Bancaria + ", " + Ope_Caj_Pagos_Detalles.Campo_Consecutivo + ")";
                            Mi_SQL += " VALUES ('" + No_Pago + "', '" + Registro["Banco_ID"].ToString() + "', '";
                            Mi_SQL += Registro["Forma_Pago"] + "', '" + Registro["No_Transaccion"].ToString() + "', '";
                            Mi_SQL += Registro["No_Autorizacion"] + "', '" + Registro["Plan_Pago"] + "', ";
                            Mi_SQL += Registro["Meses"].ToString() + ", " + Convert.ToDecimal(Registro["Monto"].ToString()) * -1 + ", '" + Registro["No_Tarjeta_Bancaria"] + "', " + Consecutivo + ")";
                        }
                        else //Forma de Pago en Cheque
                        {
                            if (Registro["Forma_Pago"].ToString() == "CHEQUE")
                            {
                                //Inserción de forma de pago en la base de datos
                                Mi_SQL += "INSERT INTO " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "(";
                                Mi_SQL += Ope_Caj_Pagos_Detalles.Campo_No_Pago + ", " + Ope_Caj_Pagos_Detalles.Campo_Banco_ID + ", ";
                                Mi_SQL += Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + ", " + Ope_Caj_Pagos_Detalles.Campo_No_Cheque + ", ";
                                Mi_SQL += Ope_Caj_Pagos_Detalles.Campo_Monto + ", " + Ope_Caj_Pagos_Detalles.Campo_Consecutivo + ")";
                                Mi_SQL += " VALUES ('" + No_Pago + "', '" + Registro["Banco_ID"].ToString() + "', '";
                                Mi_SQL += Registro["Forma_Pago"].ToString() + "', '" + Registro["No_Cheque"] + "', ";
                                Mi_SQL += Convert.ToDecimal(Registro["Monto"]) * -1 + ", " + Consecutivo + ")";
                            }
                            else //Forma de Pago de Transferencia
                            {
                                if (Registro["Forma_Pago"].ToString() == "TRANSFERENCIA")
                                {
                                    //Inserción de forma de pago en la base de datos
                                    Mi_SQL += "INSERT INTO " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "(";
                                    Mi_SQL += Ope_Caj_Pagos_Detalles.Campo_No_Pago + ", " + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + ", ";
                                    Mi_SQL += Ope_Caj_Pagos_Detalles.Campo_Monto + ", " + Ope_Caj_Pagos_Detalles.Campo_Referencia_Transferencia + ", ";
                                    Mi_SQL += Ope_Caj_Pagos_Detalles.Campo_Consecutivo + ")";
                                    Mi_SQL += " VALUES ('" + No_Pago + "', '" + Registro["Forma_Pago"].ToString() + "', ";
                                    Mi_SQL += Convert.ToDecimal(Registro["Monto"].ToString()) * -1 + ", '" + Registro["Referencia_Transferencia"].ToString() + "', ";
                                    Mi_SQL += Consecutivo + ")";
                                }
                                else //Ajuste tarifario
                                {
                                    if (Registro["Forma_Pago"].ToString() == "AJUSTE TARIFARIO") //Ajuste tarifario
                                    {
                                        //Inserción de forma de pago en la base de datos
                                        Mi_SQL += "INSERT INTO " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "(";
                                        Mi_SQL += Ope_Caj_Pagos_Detalles.Campo_No_Pago + ", " + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + ", ";
                                        Mi_SQL += Ope_Caj_Pagos_Detalles.Campo_Monto + ", " + Ope_Caj_Pagos_Detalles.Campo_Consecutivo + ")";
                                        Mi_SQL += " VALUES ('" + No_Pago + "', '" + Registro["Forma_Pago"].ToString() + "', ";
                                        Mi_SQL += Convert.ToDecimal(Registro["Monto"].ToString()) * -1 + ", " + Consecutivo + ")";
                                    }
                                    else //Cambio
                                    {
                                        //Inserción de forma de pago en la base de datos
                                        Mi_SQL += "INSERT INTO " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "(";
                                        Mi_SQL += Ope_Caj_Pagos_Detalles.Campo_No_Pago + ", " + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + ", ";
                                        Mi_SQL += Ope_Caj_Pagos_Detalles.Campo_Monto + ", " + Ope_Caj_Pagos_Detalles.Campo_Consecutivo + ")";
                                        Mi_SQL += " VALUES ('" + No_Pago + "', '" + Registro["Forma_Pago"].ToString() + "', ";
                                        Mi_SQL += Convert.ToDecimal(Registro["Monto"].ToString()) * -1 + ", " + Consecutivo + ")";
                                    }
                                }
                            }
                        }
                    }
                    Cmd.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Cmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                }

                //Consulta los pasivos para registrar el nuevo
                Mi_SQL = "SELECT " + Ope_Ing_Pasivo.Campo_No_Pasivo + ", " + Ope_Ing_Pasivo.Campo_Referencia;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID + ", " + Ope_Ing_Pasivo.Campo_Descripcion;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Fecha_Ingreso + ", " + Ope_Ing_Pasivo.Campo_Fecha_Vencimiento;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Monto + ", " + Ope_Ing_Pasivo.Campo_Recargos;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Estatus + ", " + Ope_Ing_Pasivo.Campo_No_Recibo + ", " + Ope_Ing_Pasivo.Campo_No_Pago;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Dependencia_ID + ", " + Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID;
                Mi_SQL += ", PERIODO, " + Ope_Ing_Pasivo.Campo_Cantidad;
                Mi_SQL += ", " + Ope_Ing_Pasivo.Campo_Contribuyente + ", " + Ope_Ing_Pasivo.Campo_Observaciones;
                Mi_SQL += " FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                Mi_SQL += " WHERE " + Ope_Ing_Pasivo.Campo_No_Pago + " = '" + Cancelacion.P_No_Pago + "'";
                Cmd.CommandText = Mi_SQL.ToString();
                Da = new OracleDataAdapter(Cmd);
                Ds = new DataSet();
                Da.Fill(Ds);
                Dt_Consulta = Ds.Tables[0];

                //Recorre los detalles de los montos Para insertar los Pasivos CANCELADOS
                foreach (DataRow Registro in Dt_Consulta.Rows)
                {
                    //Consulta el último no de pago que fue registrado en la base de datos
                    Mi_SQL = "SELECT MAX(" + Ope_Ing_Pasivo.Campo_No_Pasivo + ")";
                    Mi_SQL += " FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                    Cmd.CommandText = Mi_SQL.ToString();
                    No_Pasivo = Cmd.ExecuteOracleScalar().ToString();
                    if (Convert.IsDBNull(No_Pasivo))
                    {
                        No_Pasivo = Convert.ToInt32("1");
                    }
                    else
                    {
                        No_Pasivo = Convert.ToInt32(No_Pasivo) + 1;
                    }

                    Mi_SQL_PASIVO = "INSERT INTO " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " (" + Ope_Ing_Pasivo.Campo_No_Pasivo + ", " + Ope_Ing_Pasivo.Campo_Referencia;
                    Mi_SQL_PASIVO += ", " + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID + ", " + Ope_Ing_Pasivo.Campo_Descripcion;
                    Mi_SQL_PASIVO += ", " + Ope_Ing_Pasivo.Campo_Fecha_Ingreso + ", " + Ope_Ing_Pasivo.Campo_Fecha_Vencimiento;
                    Mi_SQL_PASIVO += ", " + Ope_Ing_Pasivo.Campo_Monto + ", " + Ope_Ing_Pasivo.Campo_Recargos;
                    Mi_SQL_PASIVO += ", " + Ope_Ing_Pasivo.Campo_Estatus + ", " + Ope_Ing_Pasivo.Campo_No_Recibo + ", " + Ope_Ing_Pasivo.Campo_No_Pago;
                    Mi_SQL_PASIVO += ", " + Ope_Ing_Pasivo.Campo_Dependencia_ID + ", " + Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID;
                    Mi_SQL_PASIVO += ", PERIODO, " + Ope_Ing_Pasivo.Campo_Cantidad;
                    Mi_SQL_PASIVO += ", " + Ope_Ing_Pasivo.Campo_Contribuyente + ", " + Ope_Ing_Pasivo.Campo_Observaciones;
                    Mi_SQL_PASIVO += ", " + Ope_Ing_Pasivo.Campo_Usuario_Creo + ", " + Ope_Ing_Pasivo.Campo_Fecha_Creo + ")";
                    Mi_SQL_PASIVO += " VALUES (" + No_Pasivo.ToString() + ", '" + Registro[Ope_Ing_Pasivo.Campo_Referencia].ToString() + "'";
                    Mi_SQL_PASIVO += ", '" + Registro[Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID].ToString() + "', '" + Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString() + "'";
                    Mi_SQL_PASIVO += ", '" + String.Format("{0:dd-MM-yyy}", Convert.ToDateTime(Registro[Ope_Ing_Pasivo.Campo_Fecha_Ingreso].ToString())) + "'";
                    Mi_SQL_PASIVO += ", '" + String.Format("{0:dd-MM-yyy}", Convert.ToDateTime(Registro[Ope_Ing_Pasivo.Campo_Fecha_Vencimiento].ToString())) + "'";
                    Mi_SQL_PASIVO += ", (" + Registro[Ope_Ing_Pasivo.Campo_Monto].ToString() + "*(-1)), (" + Registro[Ope_Ing_Pasivo.Campo_Recargos].ToString();
                    Mi_SQL_PASIVO += "*(-1)), 'CANCELADO'";
                    if (!String.IsNullOrEmpty(Registro[Ope_Ing_Pasivo.Campo_No_Recibo].ToString()))
                    {
                        Mi_SQL_PASIVO += ", '" + Registro[Ope_Ing_Pasivo.Campo_No_Recibo].ToString() + "'";
                    }
                    else
                    {
                        Mi_SQL_PASIVO += ", NULL";
                    }
                    Mi_SQL_PASIVO += ", '" + No_Pago + "'";
                    Mi_SQL_PASIVO += ", '" + Registro[Ope_Ing_Pasivo.Campo_Dependencia_ID].ToString() + "'";
                    if (!String.IsNullOrEmpty(Registro[Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID].ToString()))
                    {
                        Mi_SQL_PASIVO += ", '" + Registro[Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID].ToString() + "'";
                    }
                    else
                    {
                        Mi_SQL_PASIVO += ", NULL";
                    }
                    if (!String.IsNullOrEmpty(Registro["PERIODO"].ToString()))
                    {
                        Mi_SQL_PASIVO += ", '" + Registro["PERIODO"].ToString() + "'";
                    }
                    else
                    {
                        Mi_SQL_PASIVO += ", NULL";
                    }
                    if (!String.IsNullOrEmpty(Registro[Ope_Ing_Pasivo.Campo_Cantidad].ToString()))
                    {
                        Mi_SQL_PASIVO += ", " + Registro[Ope_Ing_Pasivo.Campo_Cantidad].ToString();
                    }
                    else
                    {
                        Mi_SQL_PASIVO += ", NULL";
                    }
                    if (!String.IsNullOrEmpty(Registro[Ope_Ing_Pasivo.Campo_Contribuyente].ToString()))
                    {
                        Mi_SQL_PASIVO += ", '" + Registro[Ope_Ing_Pasivo.Campo_Contribuyente].ToString() + "'";
                    }
                    else
                    {
                        Mi_SQL_PASIVO += ", NULL";
                    }
                    if (!String.IsNullOrEmpty(Registro[Ope_Ing_Pasivo.Campo_Observaciones].ToString()))
                    {
                        Mi_SQL_PASIVO += ", '" + Registro[Ope_Ing_Pasivo.Campo_Observaciones].ToString() + "'";
                    }
                    else
                    {
                        Mi_SQL_PASIVO += ", NULL";
                    }
                    Mi_SQL_PASIVO += ", '" + Cancelacion.P_Usuario + "', SYSDATE)";
                    Cmd.CommandText = Mi_SQL_PASIVO;
                    Cmd.ExecuteNonQuery();

                    No_Pasivo = null;
                }

                //Recorre los detalles de los montos Para insertar los Pasivos POR PAGAR
                foreach (DataRow Registro in Dt_Consulta.Rows)
                {
                    //Consulta el último no de pago que fue registrado en la base de datos
                    Mi_SQL = "SELECT MAX(" + Ope_Ing_Pasivo.Campo_No_Pasivo + ")";
                    Mi_SQL += " FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                    Cmd.CommandText = Mi_SQL.ToString();
                    No_Pasivo = Cmd.ExecuteOracleScalar().ToString();
                    if (Convert.IsDBNull(No_Pasivo))
                    {
                        No_Pasivo = Convert.ToInt32("1");
                    }
                    else
                    {
                        No_Pasivo = Convert.ToInt32(No_Pasivo) + 1;
                    }

                    Mi_SQL_PASIVO = "INSERT INTO " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " (" + Ope_Ing_Pasivo.Campo_No_Pasivo + ", " + Ope_Ing_Pasivo.Campo_Referencia;
                    Mi_SQL_PASIVO += ", " + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID + ", " + Ope_Ing_Pasivo.Campo_Descripcion;
                    Mi_SQL_PASIVO += ", " + Ope_Ing_Pasivo.Campo_Fecha_Ingreso + ", " + Ope_Ing_Pasivo.Campo_Fecha_Vencimiento;
                    Mi_SQL_PASIVO += ", " + Ope_Ing_Pasivo.Campo_Monto + ", " + Ope_Ing_Pasivo.Campo_Recargos;
                    Mi_SQL_PASIVO += ", " + Ope_Ing_Pasivo.Campo_Estatus;
                    Mi_SQL_PASIVO += ", " + Ope_Ing_Pasivo.Campo_Dependencia_ID + ", " + Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID;
                    Mi_SQL_PASIVO += ", " + Ope_Ing_Pasivo.Campo_Contribuyente + ", " + Ope_Ing_Pasivo.Campo_Observaciones;
                    Mi_SQL_PASIVO += ", " + Ope_Ing_Pasivo.Campo_Usuario_Creo + ", " + Ope_Ing_Pasivo.Campo_Fecha_Creo + ")";
                    Mi_SQL_PASIVO += " VALUES (" + No_Pasivo.ToString() + ", '" + Registro[Ope_Ing_Pasivo.Campo_Referencia].ToString() + "'";
                    Mi_SQL_PASIVO += ", '" + Registro[Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID].ToString() + "', '" + Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString() + "'";
                    Mi_SQL_PASIVO += ", '" + String.Format("{0:dd-MM-yyy}", Convert.ToDateTime(Registro[Ope_Ing_Pasivo.Campo_Fecha_Ingreso].ToString())) + "'";
                    Mi_SQL_PASIVO += ", '" + String.Format("{0:dd-MM-yyy}", Convert.ToDateTime(Registro[Ope_Ing_Pasivo.Campo_Fecha_Vencimiento].ToString())) + "'";
                    Mi_SQL_PASIVO += ", " + Registro[Ope_Ing_Pasivo.Campo_Monto].ToString() + ", " + Registro[Ope_Ing_Pasivo.Campo_Recargos].ToString();
                    Mi_SQL_PASIVO += ", 'POR PAGAR'";
                    Mi_SQL_PASIVO += ", '" + Registro[Ope_Ing_Pasivo.Campo_Dependencia_ID].ToString() + "'";
                    if (!String.IsNullOrEmpty(Registro[Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID].ToString()))
                    {
                        Mi_SQL_PASIVO += ", '" + Registro[Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID].ToString() + "'";
                    }
                    else
                    {
                        Mi_SQL_PASIVO += ", NULL";
                    }
                    if (!String.IsNullOrEmpty(Registro[Ope_Ing_Pasivo.Campo_Contribuyente].ToString()))
                    {
                        Mi_SQL_PASIVO += ", '" + Registro[Ope_Ing_Pasivo.Campo_Contribuyente].ToString() + "'";
                    }
                    else
                    {
                        Mi_SQL_PASIVO += ", NULL";
                    }
                    if (!String.IsNullOrEmpty(Registro[Ope_Ing_Pasivo.Campo_Observaciones].ToString()))
                    {
                        Mi_SQL_PASIVO += ", '" + Registro[Ope_Ing_Pasivo.Campo_Observaciones].ToString() + "'";
                    }
                    else
                    {
                        Mi_SQL_PASIVO += ", NULL";
                    }
                    Mi_SQL_PASIVO += ", '" + Cancelacion.P_Usuario + "', SYSDATE)";
                    Cmd.CommandText = Mi_SQL_PASIVO;
                    Cmd.ExecuteNonQuery();

                    No_Pasivo = null;
                }

                //foreach (DataRow Registro in Dt_Consulta.Rows)
                //{
                //    Mi_SQL_PASIVO = "UPDATE " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                //    Mi_SQL_PASIVO = Mi_SQL_PASIVO + " SET " + Ope_Ing_Pasivo.Campo_Estatus + " ='CANCELADO'";
                //    Mi_SQL_PASIVO = Mi_SQL_PASIVO + "," + Ope_Ing_Pasivo.Campo_Monto + " = " + Ope_Ing_Pasivo.Campo_Monto + " * (-1)";
                //    Mi_SQL_PASIVO = Mi_SQL_PASIVO + "," + Ope_Ing_Pasivo.Campo_Recargos + " = " + Ope_Ing_Pasivo.Campo_Recargos + " * (-1)";
                //    Mi_SQL_PASIVO = Mi_SQL_PASIVO + "," + Ope_Ing_Pasivo.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "'";
                //    Mi_SQL_PASIVO = Mi_SQL_PASIVO + "," + Ope_Ing_Pasivo.Campo_Fecha_Modifico + " = sysdate";
                //    Mi_SQL_PASIVO = Mi_SQL_PASIVO + " WHERE " + Ope_Ing_Pasivo.Campo_No_Pago + " ='" + Cancelacion.P_No_Pago + "' AND ";
                //    Mi_SQL_PASIVO = Mi_SQL_PASIVO + Ope_Ing_Pasivo.Campo_No_Pasivo + "='" + Registro[Ope_Ing_Pasivo.Campo_No_Pasivo].ToString() + "'";
                //    Cmd.CommandText = Mi_SQL_PASIVO;
                //    Cmd.ExecuteNonQuery();
                //}

                String Mi_SQL_PASIVO_DECS = "SELECT Distinct(REFERENCIA) FROM  " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                Mi_SQL_PASIVO_DECS = Mi_SQL_PASIVO_DECS + " WHERE  " + Ope_Ing_Pasivo.Campo_No_Pago + " ='" + Cancelacion.P_No_Pago + "'";
                Cmd.CommandText = Mi_SQL_PASIVO_DECS;
                String REFERENCIA = (String)Cmd.ExecuteScalar();

                Mi_SQL_PASIVO_DECS = "SELECT Distinct(CUENTA_PREDIAL_ID) FROM  " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                Mi_SQL_PASIVO_DECS = Mi_SQL_PASIVO_DECS + " WHERE  " + Ope_Ing_Pasivo.Campo_No_Pago + " ='" + Cancelacion.P_No_Pago + "'";
                Cmd.CommandText = Mi_SQL_PASIVO_DECS;
                String Cuenta_Predial_ID = Cmd.ExecuteScalar().ToString();

                Mi_SQL_Detalles.Length = 0;

                if (REFERENCIA.StartsWith("CDER"))
                {
                    Mi_SQL_Detalles.Length = 0;
                    //Actualiza el estatus del convenio de derechos de supervisión A POR PAGAR
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Convenios_Derechos_Supervision.Campo_Anticipo + " = 'POR PAGAR', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Convenios_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Convenios_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL_Detalles.Append(" WHERE " + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + REFERENCIA.Substring(4, 10) + "'");
                    Mi_SQL_Detalles.Append(" AND " + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Reestructura + " IS NULL");
                    Cmd.CommandText = Mi_SQL_Detalles.ToString(); //Asigna la inserción para ser ejecutada
                    Cmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Mi_SQL_Detalles.Length = 0;
                    //Actualiza el estatus del detalle del convenio  de derechos de supervisión A POR PAGAR
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Estatus + " = 'POR PAGAR', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL_Detalles.Append(" WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago + " = (SELECT " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago + " FROM " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + " WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + "='" + REFERENCIA.Substring(4, 10) + "' AND ROWNUM=1) AND ");
                    Mi_SQL_Detalles.Append(" " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + REFERENCIA.Substring(4, 10) + "'");
                    Cmd.CommandText = Mi_SQL_Detalles.ToString(); //Asigna la inserción para ser ejecutada
                    Cmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Bandero_Entro = true;
                }
                else if (REFERENCIA.StartsWith("CFRA"))
                {
                    Mi_SQL_Detalles.Length = 0;
                    //Actualiza el ANTICIPO del Convenio A POR PAGAR
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Convenios_Fraccionamientos.Campo_Anticipo + " = 'POR PAGAR', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Convenios_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL_Detalles.Append(" WHERE " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + REFERENCIA.Substring(4, 10) + "'");
                    Cmd.CommandText = Mi_SQL_Detalles.ToString(); //Asigna la inserción para ser ejecutada
                    Cmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Mi_SQL_Detalles.Length = 0;
                    //Actualiza el estatus del detalle del convenio A POR PAGAR
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + " = 'POR PAGAR', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL_Detalles.Append(" WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + " = (SELECT " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + " FROM " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + " WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + "='" + REFERENCIA.Substring(4, 10) + "' AND ROWNUM=1) AND ");
                    Mi_SQL_Detalles.Append(" " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + REFERENCIA.Substring(4, 10) + "'");
                    Mi_SQL_Detalles.Append(" AND " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Reestructura + " IS NULL");
                    Cmd.CommandText = Mi_SQL_Detalles.ToString(); //Asigna la inserción para ser ejecutada
                    Cmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Bandero_Entro = true;
                }
                else if (REFERENCIA.StartsWith("CPRE"))
                {
                    Mi_SQL_Detalles.Length = 0;
                    //Actualiza el estatus de la CONSTANCIA A PAGADA
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'POR PAGAR', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Detalles_Convenios_Predial.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL_Detalles.Append(" WHERE " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " || ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + " = ( "
                        + "SELECT * FROM ("
                        + "SELECT " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " || ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago
                        + " FROM " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial
                        + " WHERE TO_NUMBER(" + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + ") = ");
                    Mi_SQL_Detalles.Append(REFERENCIA.Substring(4, REFERENCIA.Length - 4) + " AND ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'POR PAGAR' ");
                    Mi_SQL_Detalles.Append(" ORDER BY " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago);
                    Mi_SQL_Detalles.Append(") WHERE ROWNUM = 1"
                        + ")");
                    Cmd.CommandText = Mi_SQL_Detalles.ToString(); //Asigna la inserción para ser ejecutada
                    Cmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Bandero_Entro = true;
                }
                else if (REFERENCIA.StartsWith("RTRA"))
                {
                    Mi_SQL_Detalles.Length = 0;
                    //Actualiza el ANTICIPO de la reestructura del Convenio de traslado de dominio A POR PAGAR
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Convenios_Traslados_Dominio.Campo_Anticipo_Reestructura + " = 'POR PAGAR', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Convenios_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Convenios_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL_Detalles.Append(" WHERE " + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + REFERENCIA.Substring(4, 10) + "'");
                    Cmd.CommandText = Mi_SQL_Detalles.ToString(); //Asigna la inserción para ser ejecutada
                    Cmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Mi_SQL_Detalles.Length = 0;
                    //Actualiza el estatus del detalle de la reestructura del convenio de traslado de dominio A POR PAGAR
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Estatus + " = 'POR PAGAR', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL_Detalles.Append(" WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago + " = (SELECT " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago + " FROM " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + " WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + "='" + REFERENCIA.Substring(4, 10) + "' AND ROWNUM=1 AND " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Reestructura + "=(SELECT MAX(" + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Reestructura + ") FROM " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + " WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + " ='" + REFERENCIA.Substring(4, 10) + "')");
                    Mi_SQL_Detalles.Append(") AND ");
                    Mi_SQL_Detalles.Append(" " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + REFERENCIA.Substring(4, 10) + "'");
                    Mi_SQL_Detalles.Append(" AND " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Reestructura + "=(SELECT MAX(" + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Reestructura + ") FROM " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + " WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + " ='" + REFERENCIA.Substring(4, 10) + "')");
                    Cmd.CommandText = Mi_SQL_Detalles.ToString(); //Asigna la inserción para ser ejecutada
                    Cmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Bandero_Entro = true;
                }
                else if (REFERENCIA.StartsWith("CTRA"))
                {
                    Mi_SQL_Detalles.Length = 0;
                    //Actualiza el ANTICIPO del Convenio de traslado de dominio A POR PAGAR
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Convenios_Fraccionamientos.Campo_Anticipo + " = 'POR PAGAR', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Convenios_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL_Detalles.Append(" WHERE " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + REFERENCIA.Substring(4, 10) + "'");
                    Cmd.CommandText = Mi_SQL_Detalles.ToString(); //Asigna la inserción para ser ejecutada
                    Cmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Mi_SQL_Detalles.Length = 0;
                    //Actualiza el estatus del detalle del convenio de traslado de dominio A POR PAGAR
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + " = 'POR PAGAR', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL_Detalles.Append(" WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + " = (SELECT " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + " FROM " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + " WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + "='" + REFERENCIA.Substring(4, 10) + "' AND ROWNUM=1) AND ");
                    Mi_SQL_Detalles.Append(" " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + REFERENCIA.Substring(4, 10) + "'");
                    Mi_SQL_Detalles.Append(" AND " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Reestructura + " IS NULL");
                    Cmd.CommandText = Mi_SQL_Detalles.ToString(); //Asigna la inserción para ser ejecutada
                    Cmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Bandero_Entro = true;
                }
                else if (REFERENCIA.StartsWith("RDER"))
                {
                    Mi_SQL_Detalles.Length = 0;
                    //Actualiza el ANTICIPO de la reestructura del Convenio de derechos de supervisión A POR PAGAR
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Convenios_Derechos_Supervision.Campo_Anticipo_Reestructura + " = 'POR PAGAR', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Convenios_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Convenios_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL_Detalles.Append(" WHERE " + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + REFERENCIA.Substring(4, 10) + "'");
                    Cmd.CommandText = Mi_SQL_Detalles.ToString(); //Asigna la inserción para ser ejecutada
                    Cmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Mi_SQL_Detalles.Length = 0;
                    //Actualiza el estatus del detalle de la reestructura del convenio de derechos de supervisión A POR PAGAR
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Estatus + " = 'POR PAGAR', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL_Detalles.Append(" WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago + " = (SELECT " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago + " FROM " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + " WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + "='" + REFERENCIA.Substring(4, 10) + "' AND ROWNUM=1 AND " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Reestructura + "=(SELECT MAX(" + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Reestructura + ") FROM " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + " WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + " ='" + REFERENCIA.Substring(4, 10) + "')");
                    Mi_SQL_Detalles.Append(") AND ");
                    Mi_SQL_Detalles.Append(" " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + REFERENCIA.Substring(4, 10) + "'");
                    Mi_SQL_Detalles.Append(" AND " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Reestructura + "=(SELECT MAX(" + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Reestructura + ") FROM " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + " WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + " ='" + REFERENCIA.Substring(4, 10) + "')");
                    Cmd.CommandText = Mi_SQL_Detalles.ToString(); //Asigna la inserción para ser ejecutada
                    Cmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Bandero_Entro = true;
                }
                else if (REFERENCIA.StartsWith("RFRA"))
                {
                    Mi_SQL_Detalles.Length = 0;
                    //Actualiza el ANTICIPO de la reestructura del Convenio de fraccionamiento A POR PAGAR
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Convenios_Fraccionamientos.Campo_Anticipo_Reestructura + " = 'POR PAGAR', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Convenios_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL_Detalles.Append(" WHERE " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + REFERENCIA.Substring(4, 10) + "'");
                    Cmd.CommandText = Mi_SQL_Detalles.ToString(); //Asigna la inserción para ser ejecutada
                    Cmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Mi_SQL_Detalles.Length = 0;
                    //Actualiza el estatus del detalle de la reestructura del convenio de fraccionamiento A POR PAGAR
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + " = 'POR PAGAR', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL_Detalles.Append(" WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + " = (SELECT " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + " FROM " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + " WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + "='" + REFERENCIA.Substring(4, 10) + "' AND ROWNUM=1 AND " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + "=(SELECT MAX(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + ") FROM " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + " WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " ='" + REFERENCIA.Substring(4, 10) + "')");
                    Mi_SQL_Detalles.Append(") AND ");
                    Mi_SQL_Detalles.Append(" " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + REFERENCIA.Substring(4, 10) + "'");
                    Mi_SQL_Detalles.Append(" AND " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + "=(SELECT MAX(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + ") FROM " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + " WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " ='" + REFERENCIA.Substring(4, 10) + "')");
                    Cmd.CommandText = Mi_SQL_Detalles.ToString(); //Asigna la inserción para ser ejecutada
                    Cmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Bandero_Entro = true;
                }
                else if (REFERENCIA.StartsWith("DER"))
                {
                    Revertir_Derechos_Supervision(ref Cmd, Cuenta_Predial_ID, REFERENCIA, Cancelacion.P_No_Pago, Cancelacion.P_Usuario);
                    Bandero_Entro = true;
                }
                else if (REFERENCIA.StartsWith("IMP"))
                {
                    Revertir_Impuesto_Fraccionamiento(ref Cmd, Cuenta_Predial_ID, REFERENCIA, Cancelacion.P_No_Pago, Cancelacion.P_Usuario);
                    Bandero_Entro = true;
                }
                else if (REFERENCIA.StartsWith("TD"))
                {
                    Revertir_Traslado_Dominio(ref Cmd, Cuenta_Predial_ID, REFERENCIA, Cancelacion.P_No_Pago, Cancelacion.P_Usuario);
                    Bandero_Entro = true;
                }
                else if (Char.IsLetter(REFERENCIA, 1) && !REFERENCIA.StartsWith("OTRPAG"))
                {
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Constancias.Campo_Estatus + " = 'POR PAGAR', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Constancias.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Constancias.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL_Detalles.Append(" WHERE " + Ope_Pre_Constancias.Campo_Folio + " = '" + REFERENCIA + "'");
                    Cmd.CommandText = Mi_SQL_Detalles.ToString(); //Asigna la inserción para ser ejecutada
                    Int32 filas_afectadas = Cmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                    if (filas_afectadas > 0)
                    {
                        Bandero_Entro = true;
                    }
                }
                else if (!Bandero_Entro && Cuenta_Predial_ID != "")
                {
                    Revertir_Adeudos_Predial(ref Cmd, Cuenta_Predial_ID, Cancelacion.P_No_Pago, Cancelacion.P_Usuario);
                    Bandero_Entro = true;
                }

                Cancelacion.P_No_Pago = No_Pago.ToString();
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
                    Mensaje = "Error al intentar modificar un Registro de Cancelación de Pagos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cancelaciones
        ///DESCRIPCIÓN: Obtiene todos las Cancelaciones que estan dadas de 
        ///             alta en la Base de Datos
        ///PARAMENTROS:
        ///                 Cancelacion     Contiene los campos necesarios para hacer un filtrado de 
        ///                                 información, si es que se
        ///                                 introdujeron datos de busqueda.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 27/Julio/2011 
        ///MODIFICO: SERGIO MANUEL GALLARDO ANDRADE
        ///FECHA_MODIFICO  08/OCTUBRE/2011
        ///CAUSA_MODIFICACIÓN  NO FUNCIONA LA CONSULTA YA QUE LOS CAMPOS QUE MENSIONO NO EXISTEN YA QUE HAY 2 CLASES EN LAS CONSTANTES 
        ///QUE HACEN REFERENCIA A LA MISMA TABLA 
        ///*******************************************************************************
        public static DataTable Consultar_Cancelaciones(Cls_Ope_Pre_Cancelacion_Pago_Negocio Cancelacion)
        {
            DataTable tabla = new DataTable();

            try
            {
                String Mi_SQL = "";
                Mi_SQL = Mi_SQL + "WITH PAGOS_CANCELADOS AS (";
                Mi_SQL = Mi_SQL + "SELECT PAGOS_1." + Ope_Caj_Pagos.Campo_No_Recibo;
                Mi_SQL = Mi_SQL + " ,PAGOS_1." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " PASIVOS";
                Mi_SQL = Mi_SQL + " ," + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " PAGOS_1";
                Mi_SQL = Mi_SQL + " ," + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " PAGOS_2";
                Mi_SQL = Mi_SQL + " ," + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " CAJA";
                Mi_SQL = Mi_SQL + " WHERE PAGOS_1." + Ope_Caj_Pagos.Campo_No_Pago + " = PASIVOS." + Ope_Ing_Pasivo.Campo_No_Pago;
                Mi_SQL = Mi_SQL + " AND PAGOS_1." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " = PAGOS_2." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID;
                Mi_SQL = Mi_SQL + " AND PAGOS_1." + Ope_Caj_Pagos.Campo_No_Recibo + " = PAGOS_2." + Ope_Caj_Pagos.Campo_No_Recibo;
                Mi_SQL = Mi_SQL + " AND (PAGOS_1." + Ope_Caj_Pagos.Campo_Caja_ID + " = CAJA." + Cat_Pre_Cajas.Campo_Caja_Id;
                Mi_SQL = Mi_SQL + "   OR PAGOS_2." + Ope_Caj_Pagos.Campo_Caja_ID + " = CAJA." + Cat_Pre_Cajas.Campo_Caja_Id + ")";
                Mi_SQL = Mi_SQL + " AND PAGOS_1." + Ope_Caj_Pagos.Campo_Estatus + " = 'PAGADO'";
                Mi_SQL = Mi_SQL + " AND PAGOS_2." + Ope_Caj_Pagos.Campo_Estatus + " = 'CANCELADO'";
                Mi_SQL = Mi_SQL + " AND PAGOS_1." + Ope_Caj_Pagos.Campo_No_Turno + " = '" + Cancelacion.P_No_Turno + "'";
                Mi_SQL = Mi_SQL + " AND PASIVOS." + Ope_Ing_Pasivo.Campo_Descripcion + " <> 'AJUSTE TARIFARIO'";
                Mi_SQL = Mi_SQL + "), ";

                Mi_SQL = Mi_SQL + "PAGOS_HECHOS AS (";
                Mi_SQL = Mi_SQL + "SELECT PAGOS_1." + Ope_Caj_Pagos.Campo_No_Recibo;
                Mi_SQL = Mi_SQL + " ,PAGOS_1." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID;
                Mi_SQL = Mi_SQL + " ,PAGOS_1." + Ope_Caj_Pagos.Campo_No_Pago;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " PASIVOS";
                Mi_SQL = Mi_SQL + " ," + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " PAGOS_1";
                Mi_SQL = Mi_SQL + " ," + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " CAJA";
                Mi_SQL = Mi_SQL + " WHERE PAGOS_1." + Ope_Caj_Pagos.Campo_No_Pago + " = PASIVOS." + Ope_Ing_Pasivo.Campo_No_Pago;
                Mi_SQL = Mi_SQL + " AND (PAGOS_1." + Ope_Caj_Pagos.Campo_Caja_ID + " = CAJA." + Cat_Pre_Cajas.Campo_Caja_Id + ")";
                Mi_SQL = Mi_SQL + " AND PAGOS_1." + Ope_Caj_Pagos.Campo_Estatus + " = 'CANCELADO'";
                Mi_SQL = Mi_SQL + " AND PAGOS_1." + Ope_Caj_Pagos.Campo_No_Turno + " = '" + Cancelacion.P_No_Turno + "'";
                Mi_SQL = Mi_SQL + " AND PASIVOS." + Ope_Ing_Pasivo.Campo_Descripcion + " <> 'AJUSTE TARIFARIO'";
                Mi_SQL = Mi_SQL + ") ";

                Mi_SQL = Mi_SQL + "SELECT DISTINCT P." + Ope_Caj_Pagos.Campo_No_Pago + ", P." + Ope_Caj_Pagos.Campo_No_Recibo + ", P." + Ope_Caj_Pagos.Campo_Fecha;
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS CAJA, P." + Ope_Caj_Pagos.Campo_Total + " AS MONTO";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_No_Operacion;
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_Documento;
                Mi_SQL = Mi_SQL + ", Pa." + Ope_Ing_Pasivo.Campo_Contribuyente;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " Pa, " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " P, " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " C  WHERE ";
                Mi_SQL = Mi_SQL + " P." + Ope_Caj_Pagos.Campo_Caja_ID + "= C." + Cat_Pre_Cajas.Campo_Caja_Id + "";
                Mi_SQL = Mi_SQL + " AND P." + Ope_Caj_Pagos.Campo_No_Pago + " = Pa." + Ope_Ing_Pasivo.Campo_No_Pago;
                if (Cancelacion.P_Filtro.Length != 0)
                {
                    Mi_SQL = Mi_SQL + " AND p." + Ope_Caj_Pagos.Campo_No_Recibo + " = '" + Cancelacion.P_Filtro + "'";
                }
                Mi_SQL = Mi_SQL + " AND P." + Ope_Caj_Pagos.Campo_No_Turno + " = '" + Cancelacion.P_No_Turno + "'";
                Mi_SQL = Mi_SQL + " AND Pa." + Ope_Ing_Pasivo.Campo_Descripcion + " <> 'AJUSTE TARIFARIO'";
                Mi_SQL = Mi_SQL + " AND (NOT P." + Ope_Caj_Pagos.Campo_No_Recibo + " || P." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " IN (SELECT  PAGOS_CANCELADOS." + Ope_Caj_Pagos.Campo_No_Recibo + " || PAGOS_CANCELADOS." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " FROM PAGOS_CANCELADOS) OR P." + Ope_Caj_Pagos.Campo_No_Recibo + " || P." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " || P." + Ope_Caj_Pagos.Campo_No_Pago + " IN (SELECT  PAGOS_HECHOS." + Ope_Caj_Pagos.Campo_No_Recibo + " || PAGOS_HECHOS." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " || PAGOS_HECHOS." + Ope_Caj_Pagos.Campo_No_Pago + " FROM PAGOS_HECHOS))";
                Mi_SQL = Mi_SQL + " ORDER BY P." + Ope_Caj_Pagos.Campo_No_Operacion + " DESC, " + Ope_Caj_Pagos.Campo_No_Recibo;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Cancelaciones de pagos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Cancelaciones
        ///DESCRIPCIÓN: Obtiene a detalle una CANCELACION.
        ///PARAMENTROS:   
        ///             1. P_Cancelacion.   Cancelacion que se va ver a Detalle.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 28/Julio/2011 
        ///MODIFICO: SERGIO MANUEL GALLARDO ANDRADE
        ///FECHA_MODIFICO 10/OCTUBRE/2011
        ///CAUSA_MODIFICACIÓN LA CONSULTA NO FUNCIONA Y NO TIENE BIEN SUS RELACIONES
        ///*******************************************************************************
        public static Cls_Ope_Pre_Cancelacion_Pago_Negocio Consultar_Datos_Cancelaciones(Cls_Ope_Pre_Cancelacion_Pago_Negocio P_Cancelacion)
        {
            Cls_Ope_Pre_Cancelacion_Pago_Negocio R_Cancelacion = new Cls_Ope_Pre_Cancelacion_Pago_Negocio();

            string Mi_SQL = "SELECT DISTINCT PAG." + Ope_Caj_Pagos.Campo_No_Pago + ", PAG." + Ope_Caj_Pagos.Campo_No_Recibo + ", PAG." + Ope_Caj_Pagos.Campo_Fecha + ", PAG." + Ope_Caj_Pagos.Campo_No_Operacion;
            Mi_SQL = Mi_SQL + ", MOD." + Cat_Pre_Modulos.Campo_Descripcion + " AS MODULO, PAG." + Ope_Caj_Pagos.Campo_Motivo_Cancelacion_ID + ", CAJ." + Cat_Pre_Cajas.Campo_Comentario + " AS CAJA_ID ";
            Mi_SQL = Mi_SQL + ", PAG." + Ope_Caj_Pagos.Campo_Usuario_Creo + " AS EMPLEADO_ID, PAG." + Ope_Caj_Pagos.Campo_Total + ", PAS." + Ope_Ing_Pasivo.Campo_Contribuyente + " FROM " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " CAJ, ";
            Mi_SQL = Mi_SQL + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " PAG, " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " PAS, " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + " MOD WHERE PAG." + Ope_Caj_Pagos.Campo_Caja_ID + "= CAJ." + Cat_Pre_Cajas.Campo_Caja_Id;
            Mi_SQL = Mi_SQL + " AND PAG." + Ope_Caj_Pagos.Campo_No_Pago + " = PAS." + Ope_Ing_Pasivo.Campo_No_Pago;
            Mi_SQL = Mi_SQL + " AND MOD." + Cat_Pre_Modulos.Campo_Modulo_Id + "= CAJ." + Cat_Pre_Cajas.Campo_Modulo_Id + " AND PAG." + Ope_Caj_Pagos.Campo_No_Pago + " = '" + P_Cancelacion.P_No_Pago + "'";
            OracleDataReader Data_Reader;
            try
            {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Cancelacion.P_No_Pago = P_Cancelacion.P_No_Pago;
                while (Data_Reader.Read())
                {
                    R_Cancelacion.P_No_Pago = Data_Reader[Ope_Caj_Pagos.Campo_No_Pago].ToString();
                    R_Cancelacion.P_No_Recibo = Convert.ToInt32(Data_Reader[Ope_Caj_Pagos.Campo_No_Recibo].ToString());
                    R_Cancelacion.P_No_Operacion = Convert.ToInt32(Data_Reader[Ope_Caj_Pagos.Campo_No_Operacion].ToString());
                    R_Cancelacion.P_Fecha = Data_Reader[Ope_Caj_Pagos.Campo_Fecha].ToString();
                    R_Cancelacion.P_Modulo = Data_Reader["MODULO"].ToString();
                    R_Cancelacion.P_Motivo_Cancelacion_Id = Data_Reader[Ope_Caj_Pagos.Campo_Motivo_Cancelacion_ID].ToString();
                    R_Cancelacion.P_Caja = Data_Reader["CAJA_ID"].ToString();
                    R_Cancelacion.P_Cajero = Data_Reader["EMPLEADO_ID"].ToString();
                    R_Cancelacion.P_Contribuyente = Data_Reader["CONTRIBUYENTE"].ToString();
                    R_Cancelacion.P_Monto = Data_Reader[Ope_Caj_Pagos.Campo_Total].ToString();
                }
                Data_Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el registro de Cancelación de Pagos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Cancelacion;
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

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Revertir_Adeudos_Predial
        ///DESCRIPCIÓN          : Realiza la reversion de los adeudos de predial
        ///PARAMETROS:          : Cmmd, pasa el command para ejecutar    
        ///CREO                 : Ismael Prieto Sánchez
        ///FECHA_CREO           : 25/Octubre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static void Revertir_Adeudos_Predial(ref OracleCommand Cmmd, string Cuenta_Predial_ID, string No_Pago, string Usuario)
        {
            String Mi_SQL; //Variable para ejecutar el query
            DataSet Ds_Adeudos = new DataSet(); //almacena la consulta de los adeudos del pago correspondiente
            String No_Convenio = ""; //Almacena el dato del convenio cuando asi sea el pago
            String No_Pago_Convenio = "";   //Almacena el dato del numero de pago del convenio
            DataTable Dt_Honorarios; //almacena la consulta de los honorarios cargados en la cuenta
            Double Monto_Honorarios_Temporal; //Almacena el monto de los honorarios para ir acumulando 

            try
            {
                //Realiza la consulta de los adeudos pagados
                Mi_SQL = "SELECT " + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Pago + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Adeudo + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_Anio;
                Mi_SQL += ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_Bimestre + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_Monto;
                Mi_SQL += ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Convenio + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Convenio_Pago;
                Mi_SQL += " FROM " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial;
                Mi_SQL += " WHERE " + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Pago + " = '" + No_Pago + "'";
                Mi_SQL += " ORDER BY " + Ope_Caj_Pagos_Adeudos_Predial.Campo_Anio + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_Bimestre;
                Mi_SQL += ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Convenio + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Convenio_Pago;
                Cmmd.CommandText = Mi_SQL;
                OracleDataAdapter Da_Adeudos = new OracleDataAdapter(Cmmd);
                Da_Adeudos.Fill(Ds_Adeudos);
                //Valida si trae adeudos
                if (Ds_Adeudos.Tables[0].Rows.Count > 0)
                {
                    //Asigna si es por convenio
                    if (!String.IsNullOrEmpty(Ds_Adeudos.Tables[0].Rows[0]["No_Convenio"].ToString()))
                    {
                        No_Convenio = Ds_Adeudos.Tables[0].Rows[0]["No_Convenio"].ToString();
                    }
                    if (!String.IsNullOrEmpty(Ds_Adeudos.Tables[0].Rows[0]["No_Convenio_Pago"].ToString()))
                    {
                        No_Pago_Convenio = Ds_Adeudos.Tables[0].Rows[0]["No_Convenio_Pago"].ToString();
                    }
                    //Recorre los adeudos de predial para revertirlos
                    foreach (DataRow Registro in Ds_Adeudos.Tables[0].Rows)
                    {
                        //Actualiza el adeudo con el monto del pago
                        Mi_SQL = "UPDATE " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + " SET";
                        if (Convert.ToInt32(Registro["Bimestre"].ToString()) == 1)
                        {
                            Mi_SQL += " " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + " = " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + " - " + Registro["Monto"].ToString();
                        }
                        else if (Convert.ToInt32(Registro["Bimestre"].ToString()) == 2)
                        {
                            Mi_SQL += " " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + " = " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + " - " + Registro["Monto"].ToString();
                        }
                        else if (Convert.ToInt32(Registro["Bimestre"].ToString()) == 3)
                        {
                            Mi_SQL += " " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + " = " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + " - " + Registro["Monto"].ToString();
                        }
                        else if (Convert.ToInt32(Registro["Bimestre"].ToString()) == 4)
                        {
                            Mi_SQL += " " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + " = " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + " - " + Registro["Monto"].ToString();
                        }
                        else if (Convert.ToInt32(Registro["Bimestre"].ToString()) == 5)
                        {
                            Mi_SQL += " " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + " = " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + " - " + Registro["Monto"].ToString();
                        }
                        else if (Convert.ToInt32(Registro["Bimestre"].ToString()) == 6)
                        {
                            Mi_SQL += " " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + " = " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + " - " + Registro["Monto"].ToString();
                        }
                        Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                        Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_SQL += " WHERE " + Ope_Pre_Adeudos_Predial.Campo_No_Adeudo + " = '" + Registro["NO_ADEUDO"] + "'";
                        Mi_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_Anio + " = " + Registro["ANIO"];
                        Mi_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_ID + "'";
                        Cmmd.CommandText = Mi_SQL;
                        Cmmd.ExecuteNonQuery();

                        //Actualiza el estatus del adeudo
                        Mi_SQL = "UPDATE " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + " SET " + Ope_Pre_Adeudos_Predial.Campo_Estatus + " = 'POR PAGAR', ";
                        Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                        Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_SQL += " WHERE ((" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + " - " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ")";
                        Mi_SQL += " + (" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + " - " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ")";
                        Mi_SQL += " + (" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + " - " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ")";
                        Mi_SQL += " + (" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + " - " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ")";
                        Mi_SQL += " + (" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + " - " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ")";
                        Mi_SQL += " + (" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + " - " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ")) <> 0";
                        Mi_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_No_Adeudo + " = '" + Registro["NO_ADEUDO"] + "'";
                        Mi_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_Anio + " = " + Registro["ANIO"];
                        Mi_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_ID + "'";
                        Cmmd.CommandText = Mi_SQL;
                        Cmmd.ExecuteNonQuery();

                        //Actualiza el numero de convenio y pago
                        if (No_Convenio != "" && No_Pago_Convenio != "")
                        {
                            if (No_Pago_Convenio != Registro["No_Convenio_Pago"].ToString())
                            {
                                //Actualiza el anticipo si es la primer parcialidad
                                if (Convert.ToInt32(No_Pago_Convenio) == 1)
                                {
                                    Mi_SQL = "UPDATE " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial;
                                    Mi_SQL += " SET " + Ope_Pre_Convenios_Predial.Campo_Anticipo + " = 'POR PAGAR', ";
                                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                                    Mi_SQL += " WHERE " + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = '" + No_Convenio + "'";
                                    Cmmd.CommandText = Mi_SQL;
                                    Cmmd.ExecuteNonQuery();
                                }

                                //Actualiza el registro para ponerlo por pagar
                                Mi_SQL = "UPDATE " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial;
                                Mi_SQL += " SET " + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'POR PAGAR'";
                                Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                                Mi_SQL += " WHERE " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " = '" + No_Convenio + "'";
                                Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + " = " + No_Pago_Convenio;
                                Cmmd.CommandText = Mi_SQL;
                                Cmmd.ExecuteNonQuery();
                                No_Pago_Convenio = Registro["No_Convenio_Pago"].ToString();
                            }
                        }
                    }
                }

                //Actualiza el numero de convenio y pago
                if (No_Convenio != "" && No_Pago_Convenio != "")
                {
                    //Actualiza el registro para ponerlo por pagar
                    Mi_SQL = "UPDATE " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial;
                    Mi_SQL += " SET " + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'POR PAGAR'";
                    Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                    Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                    Mi_SQL += " WHERE " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " = '" + No_Convenio + "'";
                    Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + " = " + No_Pago_Convenio;
                    Cmmd.CommandText = Mi_SQL;
                    Cmmd.ExecuteNonQuery();

                    //Actualiza el convenio a pendiente de pago
                    Mi_SQL = "UPDATE " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + " SET " + Ope_Pre_Convenios_Predial.Campo_Estatus + " = 'ACTIVO'";
                    if (Convert.ToInt32(No_Pago_Convenio) == 1)
                    {
                        Mi_SQL += ", " + Ope_Pre_Convenios_Predial.Campo_Anticipo + " = 'POR PAGAR'";
                    }
                    Mi_SQL += ", " + Ope_Pre_Convenios_Predial.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                    Mi_SQL += " WHERE " + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = '" + No_Convenio + "'";
                    Cmmd.CommandText = Mi_SQL;
                    Cmmd.ExecuteNonQuery();
                }

                //Regresa los honorarios correspondiente
                Mi_SQL = "SELECT NVL(SUM(" + Ope_Pre_Recargos.Campo_Monto + "),'0') FROM " + Ope_Pre_Recargos.Tabla_Ope_Pre_Recargos;
                Mi_SQL += " WHERE " + Ope_Pre_Recargos.Campo_No_Pago + " = '" + No_Pago + "'";
                Mi_SQL += " AND " + Ope_Pre_Recargos.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_ID + "'";
                Mi_SQL += " AND " + Ope_Pre_Recargos.Campo_Tipo + " = 'HONORARIOS'";
                Cmmd.CommandText = Mi_SQL;
                Monto_Honorarios_Temporal = Convert.ToDouble(Cmmd.ExecuteOracleScalar().ToString());
                if (Monto_Honorarios_Temporal > 0)
                {
                    //Recorre los honorarios actuales para regresarle el monto                    
                    Cls_Ope_Pre_Pae_Honorarios_Negocio Neg_Honorarios = new Cls_Ope_Pre_Pae_Honorarios_Negocio();
                    Neg_Honorarios.P_Campos_Dinamicos = "NVL(" + Ope_Pre_Pae_Honorarios.Campo_Monto_Pagado + ",0) AS " + Ope_Pre_Pae_Honorarios.Campo_Monto_Pagado + ", " + Ope_Pre_Pae_Honorarios.Campo_No_Honorario;
                    Neg_Honorarios.P_Filtro = Ope_Pre_Pae_Honorarios.Campo_Monto_Pagado + " > 0 AND (SELECT " + Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id + " FROM " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + " WHERE " + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + " = " + Ope_Pre_Pae_Honorarios.Tabla_Ope_Pre_Pae_Honorarios + "." + Ope_Pre_Pae_Honorarios.Campo_No_Detalle_Etapa + ")='" + Cuenta_Predial_ID + "'";
                    Neg_Honorarios.P_Agrupar_Dinamico = Ope_Pre_Pae_Honorarios.Campo_No_Honorario + " DESC";
                    Dt_Honorarios = Neg_Honorarios.Consultar_Honorario();
                    foreach (DataRow Dr_Renglon in Dt_Honorarios.Rows)
                    {
                        if (Monto_Honorarios_Temporal > 0)
                        {
                            //Actualiza el registro del honorario, restando el monto correspondiente
                            Mi_SQL = "UPDATE " + Ope_Pre_Pae_Honorarios.Tabla_Ope_Pre_Pae_Honorarios;
                            Mi_SQL += " SET " + Ope_Pre_Pae_Honorarios.Campo_Monto_Pagado + "=";
                            Mi_SQL += Ope_Pre_Pae_Honorarios.Campo_Monto_Pagado + " - " + Convert.ToDouble(Dr_Renglon[Ope_Pre_Pae_Honorarios.Campo_Monto_Pagado].ToString());
                            Mi_SQL += ", " + Ope_Pre_Pae_Honorarios.Campo_Usuario_Modifico + "='" + Usuario + "', ";
                            Mi_SQL += Ope_Pre_Pae_Honorarios.Campo_Fecha_Modifico + "= SYSDATE";
                            Mi_SQL += " WHERE " + Ope_Pre_Pae_Honorarios.Campo_No_Honorario + "='" + Dr_Renglon[Ope_Pre_Pae_Honorarios.Campo_No_Honorario].ToString() + "'";
                            Cmmd.CommandText = Mi_SQL;
                            Cmmd.ExecuteNonQuery();
                            //Actualiza la variable del acumulado, restando el monto correspondiente
                            Monto_Honorarios_Temporal = Monto_Honorarios_Temporal - Convert.ToDouble(Dr_Renglon[Ope_Pre_Pae_Honorarios.Campo_Monto_Pagado].ToString());
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                //Elimina los registros de los recargos y honorarios
                Mi_SQL = "DELETE FROM " + Ope_Pre_Recargos.Tabla_Ope_Pre_Recargos;
                Mi_SQL += " WHERE " + Ope_Pre_Recargos.Campo_No_Pago + " = '" + No_Pago + "'";
                Mi_SQL += " AND " + Ope_Pre_Recargos.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_ID + "'";
                Cmmd.CommandText = Mi_SQL;
                Cmmd.ExecuteNonQuery();

                //Actualiza el estatus de vigente
                Mi_SQL = "UPDATE " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial;
                Mi_SQL += " SET " + Ope_Pre_Descuentos_Predial.Campo_Estatus + " = 'VIGENTE', ";
                Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_No_Pago + " = NULL, ";
                Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL += " WHERE " + Ope_Pre_Descuentos_Predial.Campo_No_Pago + " = '" + No_Pago + "'";
                Mi_SQL += " AND " + Ope_Pre_Descuentos_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_ID + "'";
                Mi_SQL += " AND " + Ope_Pre_Descuentos_Predial.Campo_Estatus + " = 'APLICADO'";
                Cmmd.CommandText = Mi_SQL;
                Cmmd.ExecuteNonQuery();
            }
            catch (OracleException Ex)
            {
                //Indicamos el mensaje 
                throw new Exception(Ex.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Revertir_Traslado_Dominio
        ///DESCRIPCIÓN          : Realiza la reversion del traslado de dominio con los datos anteriores
        ///PARAMETROS:          : Cmmd, pasa el command para ejecutar    
        ///CREO                 : Ismael Prieto Sánchez
        ///FECHA_CREO           : 26/Octubre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static void Revertir_Traslado_Dominio(ref OracleCommand Cmmd, string Cuenta_Predial_ID, string Referencia, string No_Pago, string Usuario)
        {
            Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
            Cls_Ope_Caj_Pagos_Negocio Caja = new Cls_Ope_Caj_Pagos_Negocio();
            String No_Calculo = "";
            Int16 Año_Calculo = 0;
            String Mi_SQL;
            String No_Convenio = "";
            Int32 No_Pago_Convenio = 0;
            DataSet Ds_Convenios = new DataSet();

            try
            {
                //Consulta si el pago tiene convenio
                Mi_SQL = "SELECT " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio + ", ";
                Mi_SQL += Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago;
                Mi_SQL += " FROM " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                Mi_SQL += " WHERE " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago_Aplicado + " = '" + No_Pago + "'";
                Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio;
                Mi_SQL += " = " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Convenio;
                Mi_SQL += " ORDER BY " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago;
                Cmmd.CommandText = Mi_SQL;
                OracleDataAdapter Da_Convenios = new OracleDataAdapter(Cmmd);
                Da_Convenios.Fill(Ds_Convenios);
                //Valida si trae adeudos
                if (Ds_Convenios.Tables[0].Rows.Count > 0)
                {
                    //Asigna si es por convenio
                    if (!String.IsNullOrEmpty(Ds_Convenios.Tables[0].Rows[0][Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio].ToString()))
                    {
                        No_Convenio = Ds_Convenios.Tables[0].Rows[0][Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio].ToString();
                    }
                    if (!String.IsNullOrEmpty(Ds_Convenios.Tables[0].Rows[0][Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago].ToString()))
                    {
                        No_Pago_Convenio = Convert.ToInt32(Ds_Convenios.Tables[0].Rows[0][Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago].ToString());
                    }
                    //Recorre las parcialidades del convenio
                    foreach (DataRow Registro in Ds_Convenios.Tables[0].Rows)
                    {
                        //Pone como por pagar el pago de la parcialidad del convenio
                        Mi_SQL = "UPDATE " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio;
                        Mi_SQL += " SET " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Estatus + " = 'POR PAGAR'";
                        Mi_SQL += ", " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago_Aplicado + " = NULL";
                        Mi_SQL += ", " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Usuario_Modifico + " = '" + Usuario + "'";
                        Mi_SQL += ", " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_SQL += " WHERE " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio + " = '" + No_Convenio + "'";
                        Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago + " = " + Registro[Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago].ToString();
                        Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago_Aplicado + " = '" + No_Pago + "'";
                        Cmmd.CommandText = Mi_SQL;
                        Cmmd.ExecuteNonQuery();

                        //Actualiza el anticipo si es el primer pago
                        if (Convert.ToInt32(Registro[Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago].ToString()) == 1)
                        {
                            //Actualiza el anticipo del convenio
                            Mi_SQL = "UPDATE " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio;
                            Mi_SQL += " SET " + Ope_Pre_Convenios_Traslados_Dominio.Campo_Anticipo + " = 'POR PAGAR', ";
                            Mi_SQL += Ope_Pre_Convenios_Traslados_Dominio.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                            Mi_SQL += Ope_Pre_Convenios_Traslados_Dominio.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL += " WHERE " + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio + " = '" + No_Convenio + "'";
                            Cmmd.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                            Cmmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                        }
                    }
                    //Actualiza el convenio
                    Mi_SQL = "UPDATE " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + " SET " + Ope_Pre_Convenios_Traslados_Dominio.Campo_Estatus + " = 'ACTIVO'";
                    Mi_SQL += ", " + Ope_Pre_Convenios_Traslados_Dominio.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                    Mi_SQL += Ope_Pre_Convenios_Traslados_Dominio.Campo_Fecha_Modifico + " = SYSDATE";
                    Mi_SQL += " WHERE " + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio + " = '" + No_Convenio + "'";
                    Cmmd.CommandText = Mi_SQL;
                    Cmmd.ExecuteNonQuery();
                }

                //Actualiza el calculo
                Mi_SQL = "UPDATE " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado;
                if (No_Pago_Convenio <= 1)
                {
                    Mi_SQL += " SET " + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " = 'POR PAGAR'";
                }
                else
                {
                    Mi_SQL += " SET " + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " = 'PARCIAL'";
                }
                Mi_SQL += ", " + Ope_Pre_Calculo_Imp_Traslado.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL += " WHERE " + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + " = '" + No_Calculo + "'";
                Mi_SQL += " AND " + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + " = " + Año_Calculo;
                Cmmd.CommandText = Mi_SQL;
                Cmmd.ExecuteNonQuery();

                //Obtiene el año y numero de calculo
                if (No_Pago_Convenio <= 1)
                {
                    No_Calculo = Referencia.Substring(2);
                    Año_Calculo = 0;
                    if (No_Calculo.Length > 4)
                    {
                        Año_Calculo = Convert.ToInt16(No_Calculo.Substring(No_Calculo.Length - 4));
                        No_Calculo = No_Calculo.Substring(0, No_Calculo.Length - 4);
                    }

                    //Consulta el calculo
                    Caja.P_No_Calculo = Convert.ToInt64(No_Calculo).ToString("0000000000");
                    Caja.P_Año_Calculo = Año_Calculo;
                    DataTable Dt_Calculo = Caja.Consultar_Datos_Calculo();

                    if (Dt_Calculo.Rows.Count > 0)
                    {
                        //Consulta el Encabezado de la orden de variacion
                        Caja.P_Orden_Variacion_ID = Dt_Calculo.Rows[0]["No_Orden_Variacion"].ToString().Trim();
                        Caja.P_Anio = Dt_Calculo.Rows[0]["Anio"].ToString().Trim();
                        DataTable Dt_Consultar_Orden_Variacion = Caja.Consultar_Orden_Variacion();

                        //Consulta los detalles de la orden
                        //Orden_Variacion.P_Incluir_Campos_Foraneos = true;
                        //Orden_Variacion.P_Orden_Variacion_ID = Dt_Consultar_Orden_Variacion.Rows[0]["No_Orden_Variacion"].ToString().Trim();
                        //Orden_Variacion.P_Generar_Orden_Anio = Dt_Consultar_Orden_Variacion.Rows[0]["Anio"].ToString().Trim();
                        Orden_Variacion.P_Campos_Dinamicos = " * ";
                        Orden_Variacion.P_Filtros_Dinamicos = " " + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + "='ACEPTADA' AND " + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + "='" + Cuenta_Predial_ID + "' ";
                        Orden_Variacion.P_Ordenar_Dinamico = Ope_Pre_Ordenes_Variacion.Campo_Anio + " DESC, " + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " DESC ";
                        DataTable Dt_Detalle_Orden_Variacion = Orden_Variacion.Consultar_Ordenes_Variacion();
                        //Dt_Detalle_Orden_Variacion = Orden_Variacion.P_Generar_Orden_Dt_Detalles;
                        //Consulta los detalles de la orden
                        Orden_Variacion.P_Incluir_Campos_Foraneos = true;
                        Orden_Variacion.P_Orden_Variacion_ID = Dt_Consultar_Orden_Variacion.Rows[0]["No_Orden_Variacion"].ToString().Trim();
                        Orden_Variacion.P_Generar_Orden_Anio = Dt_Consultar_Orden_Variacion.Rows[0]["Anio"].ToString().Trim();
                        Orden_Variacion.P_Campos_Dinamicos = null;
                        Orden_Variacion.P_Filtros_Dinamicos = null;
                        Orden_Variacion.P_Ordenar_Dinamico = null;
                        String No_Orden_Anterior = "";
                        String Anio_Orden_Anterior = "";
                        if (Dt_Detalle_Orden_Variacion.Rows.Count > 0)
                        {
                            Int32 Indice_Orden = 0;
                            while (Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion].ToString().Trim() != Dt_Consultar_Orden_Variacion.Rows[0]["No_Orden_Variacion"].ToString().Trim())
                            {
                                Indice_Orden++;
                                if (Indice_Orden >= Dt_Detalle_Orden_Variacion.Rows.Count)
                                {
                                    break;
                                }
                            }
                            Indice_Orden++;
                            if (Indice_Orden < Dt_Detalle_Orden_Variacion.Rows.Count)
                            {
                                No_Orden_Anterior = Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion].ToString().Trim();
                                Anio_Orden_Anterior = Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Anio].ToString().Trim();
                                //Afecta la cuenta predial con los datos anteriores
                                Mi_SQL = "UPDATE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;
                                Mi_SQL += " SET ";
                                //foreach (DataRow Dr_Fila in Dt_Detalle_Orden_Variacion.Rows)
                                //{
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Calle_ID)
                                //{
                                if (Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Calle_ID].ToString() != "")
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_ID + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Calle_ID].ToString() + "', ";
                                }
                                else
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_ID + " = NULL, ";
                                }
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Colonia_ID)
                                //{
                                if (Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID].ToString() != "")
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID].ToString() + "', ";
                                }
                                else
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + " = NULL, ";
                                }
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID)
                                //{
                                if (Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Estado_Predio_ID].ToString() != "")
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Estado_Predio_ID].ToString() + "', ";
                                }
                                else
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID + " = NULL, ";
                                }
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID)
                                //{
                                if (Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID].ToString() != "")
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID].ToString() + "', ";
                                }
                                else
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + " = NULL, ";
                                }
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID)
                                //{
                                if (Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Uso_Suelo_ID].ToString() != "")
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Uso_Suelo_ID].ToString() + "', ";
                                }
                                else
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID + " = NULL, ";
                                }
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID)
                                //{
                                if (Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Cuota_Minima_ID].ToString() != "")
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Cuota_Minima_ID].ToString() + "', ";
                                }
                                else
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID + " = NULL, ";
                                }
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen)
                                //{
                                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Origen].ToString() + "', ";
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Estatus)
                                //{
                                if (Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Cuenta].ToString() != "")
                                {
                                    if (Dt_Detalle_Orden_Variacion.Rows.Count > 1)
                                    {
                                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estatus + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Cuenta].ToString() + "', ";
                                    }
                                    else
                                    {
                                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estatus + " = 'PENDIENTE', ";
                                    }
                                }
                                else
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estatus + " = 'PENDIENTE', ";
                                }
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_No_Exterior)
                                //{
                                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Exterior + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_No_Exterior].ToString() + "', ";
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_No_Interior)
                                //{
                                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Interior + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_No_Interior].ToString() + "', ";
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida)
                                //{
                                if (!String.IsNullOrEmpty(Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Superficie_Construida].ToString()))
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + " = " + Convert.ToDouble(Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Superficie_Construida].ToString()) + ", ";
                                }
                                else
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + " = NULL, ";
                                }
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Superficie_Total)
                                //{
                                if (!String.IsNullOrEmpty(Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Superficie_Total].ToString()))
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + " = " + Convert.ToDouble(Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Superficie_Total].ToString()) + ", ";
                                }
                                else
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + " = NULL, ";
                                }
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral)
                                //{
                                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Clave_Catastral].ToString() + "', ";
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal)
                                //{
                                if (!String.IsNullOrEmpty(Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Valor_Fiscal].ToString()))
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + " = " + Convert.ToDouble(Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Valor_Fiscal].ToString()) + ", ";
                                }
                                else
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + " = NULL, ";
                                }
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Efectos)
                                //{
                                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Efectos + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Efectos].ToString() + "', ";
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente)
                                //{
                                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Periodo_Corriente].ToString() + "', ";
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual)
                                //{
                                if (!String.IsNullOrEmpty(Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Cuota_Anual].ToString()))
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + " = " + Convert.ToDouble(Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Cuota_Anual].ToString()) + ", ";
                                }
                                else
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + " = NULL, ";
                                }
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija)
                                //{
                                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Cuota_Fija].ToString() + "', ";
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion)
                                //{
                                if (!String.IsNullOrEmpty(Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Diferencia_Construccion].ToString()))
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion + " = " + Convert.ToDouble(Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Diferencia_Construccion].ToString()) + ", ";
                                }
                                else
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion + " = NULL, ";
                                }
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion)
                                //{
                                if (!String.IsNullOrEmpty(Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Termino_Exencion].ToString()))
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + " = '" + Convert.ToDateTime(Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Termino_Exencion].ToString()).ToString("d-M-yyyy") + "', ";
                                }
                                else
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + " = NULL, ";
                                }
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo)
                                //{
                                if (!String.IsNullOrEmpty(Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Fecha_Avaluo].ToString()))
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo + " = '" + Convert.ToDateTime(Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Fecha_Avaluo].ToString()).ToString("d-M-yyyy") + "', ";
                                }
                                else
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo + " = NULL, ";
                                }
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Costo_m2)
                                //{
                                if (!String.IsNullOrEmpty(Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Costo_M2].ToString()))
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Costo_m2 + " = " + Convert.ToDouble(Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Costo_M2].ToString()) + ", ";
                                }
                                else
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Costo_m2 + " = NULL, ";
                                }
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion)
                                //{
                                if (!String.IsNullOrEmpty(Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Porcentaje_Exencion].ToString()))
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + " = " + Convert.ToDouble(Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Porcentaje_Exencion].ToString()) + ", ";
                                }
                                else
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + " = NULL, ";
                                }
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija)
                                //{
                                if (Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_No_Cuota_Fija].ToString() != "")
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_No_Cuota_Fija].ToString() + "', ";
                                }
                                else
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + " = NULL, ";
                                }
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Tasa_ID)
                                //{
                                if (Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Tasa_ID].ToString() != "")
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Tasa_ID + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Tasa_ID].ToString() + "', ";
                                }
                                else
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Tasa_ID + " = NULL, ";
                                }
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion)
                                //{
                                if (Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Calle_ID_Notificacion].ToString() != "")
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Calle_ID_Notificacion].ToString() + "', ";
                                }
                                else
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + " = NULL, ";
                                }
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion)
                                //{
                                if (Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID_Notificacion].ToString() != "")
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID_Notificacion].ToString() + "', ";
                                }
                                else
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + " = NULL, ";
                                }
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion)
                                //{
                                if (Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Estado_ID_Notificacion].ToString() != "")
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Estado_ID_Notificacion].ToString() + "', ";
                                }
                                else
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + " = NULL, ";
                                }
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion)
                                //{
                                if (Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Ciudad_ID_Notificacion].ToString() != "")
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Ciudad_ID_Notificacion].ToString() + "', ";
                                }
                                else
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + " = NULL, ";
                                }
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo)
                                //{
                                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Domicilio_Foraneo].ToString() + "', ";
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion)
                                //{
                                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Calle_Notificacion].ToString() + "', ";
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion)
                                //{
                                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_No_Exterior_Notificacion].ToString() + "', ";
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal)
                                //{
                                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Codigo_Postal].ToString() + "', ";
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion)
                                //{
                                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_No_Interior_Notificacion].ToString() + "', ";
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion)
                                //{
                                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Colonia_Notificacion].ToString() + "', ";
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_No_Diferencia)
                                //{
                                ////if (Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_No_Diferencia].ToString() != "")
                                ////    {
                                ////        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Diferencia + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_No_Diferencia].ToString() + "', ";
                                ////    }
                                ////    else
                                ////    {
                                ////        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Diferencia + " = NULL, ";
                                ////    }
                                //////}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID)
                                //{
                                if (Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Tasa_Predial_ID].ToString() != "")
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Tasa_Predial_ID].ToString() + "', ";
                                }
                                else
                                {
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID + " = NULL, ";
                                }
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion)
                                //{
                                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Estado_Notificacion].ToString() + "', ";
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion)
                                //{
                                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Ciudad_Notificacion].ToString() + "', ";
                                //}
                                //if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion)
                                //{
                                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion + " = '" + Dt_Detalle_Orden_Variacion.Rows[Indice_Orden][Ope_Pre_Ordenes_Variacion.Campo_Tipo_Suspencion].ToString() + "', ";
                                //}
                                //}
                                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                                Mi_SQL += " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_ID + "'";
                                Cmmd.CommandText = Mi_SQL;
                                Cmmd.ExecuteNonQuery();
                            }
                        }
                        //Regresa los adeudos
                        DataTable Dt_Diferencias = new DataTable();
                        DataRow Dr;

                        //Consulta los Datos de la Variación de las Diferencias
                        Dt_Diferencias.Columns.Add(new DataColumn("CUOTA_ANUAL", typeof(Decimal)));
                        Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_1", typeof(Decimal)));
                        Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_2", typeof(Decimal)));
                        Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_3", typeof(Decimal)));
                        Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_4", typeof(Decimal)));
                        Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_5", typeof(Decimal)));
                        Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_6", typeof(Decimal)));
                        Dt_Diferencias.Columns.Add(new DataColumn("AÑO", typeof(int)));
                        Dt_Diferencias.Columns.Add(new DataColumn("ALTA_BAJA", typeof(String)));

                        String Periodo = "";
                        Decimal Sum_Adeudos_Año = 0;
                        Decimal Sum_Adeudos_Periodo = 0;
                        int Cont_Cuotas_Minimas_Año = 0;
                        int Cont_Cuotas_Minimas_Periodo = 0;
                        int Cont_Adeudos_Año = 0;
                        int Cont_Adeudos_Periodo = 0;
                        int Desde_Bimestre = 0;
                        int Hasta_Bimestre = 0;
                        int Cont_Bimestres = 0;
                        int Año_Periodo = 0;
                        int Signo = 1;
                        Boolean Periodo_Corriente_Validado = false;
                        Boolean Periodo_Rezago_Validado = false;
                        Decimal Importe_Rezago = 0;
                        Decimal Cuota_Fija = 0;
                        Decimal Cuota_Minima_Año = 0;
                        Decimal Cuota_Anual = 0;
                        Boolean Nueva_Cuota_Fija = false;
                        String Cuota_Fija_Nueva = "";
                        String Cuota_Fija_Anterior = "";
                        Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuotas_Minimas = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
                        Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
                        DataTable Dt_Adeudos_Cuenta = new DataTable();

                        Orden_Variacion.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
                        Orden_Variacion.P_Generar_Orden_No_Orden = Dt_Consultar_Orden_Variacion.Rows[0]["No_Orden_Variacion"].ToString().Trim();
                        Orden_Variacion.P_Generar_Orden_Anio = Dt_Consultar_Orden_Variacion.Rows[0]["Anio"].ToString().Trim();
                        //Orden_Variacion.P_Generar_Orden_No_Orden = Orden_Variacion.P_Orden_Variacion_ID;
                        //Orden_Variacion.P_Generar_Orden_Anio = Orden_Variacion.P_Año.ToString();
                        DataTable Dt_Agregar_Diferencias = Orden_Variacion.Consulta_Diferencias();// Orden_Variacion.Consulta_Diferencias();

                        if (Cuota_Fija_Nueva != ""
                            && Cuota_Fija_Nueva != Cuota_Fija_Anterior)
                        {
                            Nueva_Cuota_Fija = true;
                        }

                        for (int x = 0; x < Dt_Agregar_Diferencias.Rows.Count; x++)
                        {
                            //if (Dt_Agregar_Diferencias.Rows[x]["TIPO"] != null)
                            //{
                            //    if (Dt_Agregar_Diferencias.Rows[x]["TIPO"].ToString().Trim() != "")
                            //    {
                            //        if (Dt_Agregar_Diferencias.Rows[x]["TIPO"].ToString().Trim() == "ALTA")
                            //        {
                            //            Signo = 1;
                            //        }
                            //        else
                            //        {
                            //            if (Dt_Agregar_Diferencias.Rows[x]["TIPO"].ToString().Trim() == "BAJA")
                            //            {
                            Signo = -1;
                            //            }
                            //        }
                            //    }
                            //}

                            Cuota_Anual = Convert.ToDecimal(Dt_Agregar_Diferencias.Rows[x]["Cuota_Bimestral"].ToString()) * 6;
                            Año_Periodo = Convert.ToInt32(Dt_Agregar_Diferencias.Rows[x]["Periodo"].ToString().Substring(Dt_Agregar_Diferencias.Rows[x]["Periodo"].ToString().Trim().Length - 4, 4));
                            Cuota_Minima_Año = Cuotas_Minimas.Consultar_Cuota_Minima_Anio(Año_Periodo.ToString());
                            Importe_Rezago = Convert.ToDecimal(Dt_Agregar_Diferencias.Rows[x]["Importe"].ToString().Replace("$", ""));
                            Periodo = Obtener_Periodos_Bimestre(Dt_Agregar_Diferencias.Rows[x]["Periodo"].ToString().Trim(), out Periodo_Corriente_Validado, out Periodo_Rezago_Validado);
                            if (Periodo.Trim() != "")
                            {
                                Desde_Bimestre = Convert.ToInt32(Periodo.Split('-').GetValue(0));
                                Hasta_Bimestre = Convert.ToInt32(Periodo.Split('-').GetValue(1));

                                //Cuotas_Minimas_Encontradas_Año = false;
                                Cont_Cuotas_Minimas_Año = 0;
                                Cont_Adeudos_Año = 0;
                                Sum_Adeudos_Año = 0;
                                //Cuotas_Minimas_Encontradas_Periodo = false;
                                Cont_Cuotas_Minimas_Periodo = 0;
                                Cont_Adeudos_Periodo = 0;
                                Sum_Adeudos_Periodo = 0;

                                Dt_Adeudos_Cuenta = Resumen_Predio.Consultar_Adeudos_Cuenta_Predial_Con_Totales(Orden_Variacion.P_Cuenta_Predial_ID, null, Año_Periodo, Año_Periodo);
                                if (Dt_Adeudos_Cuenta != null)
                                {
                                    if (Dt_Adeudos_Cuenta.Rows.Count > 0)
                                    {
                                        //Contador de los Adeudos/Cuotas del Año
                                        for (Cont_Bimestres = 1; Cont_Bimestres <= 6; Cont_Bimestres++)
                                        {
                                            if (Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres] != System.DBNull.Value)
                                            {
                                                if (Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]) == Cuota_Minima_Año)
                                                {
                                                    Cont_Cuotas_Minimas_Año += 1;
                                                }
                                                if (Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]) != 0)
                                                {
                                                    Cont_Adeudos_Año += 1;
                                                    Sum_Adeudos_Año += Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]);
                                                }
                                            }
                                        }
                                        //Contador de los Adeudos/Cuotas del Periodo indicado
                                        for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                        {
                                            if (Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres] != System.DBNull.Value)
                                            {
                                                if (Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]) == Cuota_Minima_Año)
                                                {
                                                    Cont_Cuotas_Minimas_Periodo += 1;
                                                }
                                                if (Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]) != 0)
                                                {
                                                    Cont_Adeudos_Periodo += 1;
                                                    Sum_Adeudos_Periodo += Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]);
                                                }
                                            }
                                        }
                                    }
                                }

                                Dr = Dt_Diferencias.NewRow();
                                Dr["CUOTA_ANUAL"] = Cuota_Anual;
                                Dr["AÑO"] = Año_Periodo;
                                Dr["ALTA_BAJA"] = "BAJA";
                                //VALIDACIONES PARA CASOS DE CUOTAS MÍNIMAS Y APLICACIÓN DE ADEUDOS
                                //if (Cont_Cuotas_Minimas_Periodo == 1 && Importe_Rezago != Cuota_Minima_Año && !Nueva_Cuota_Fija)
                                //{
                                //    Dr["ALTA_BAJA"] = "SOB";
                                //    //SUMA LA CUOTA MÍNIMA AL IMPORTE Y EL RESULTADO LO PRORRATEA EN EL PERIODO INDICADO
                                //    for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                //    {
                                //        Dr["BIMESTRE_" + Cont_Bimestres.ToString()] = Math.Round((Importe_Rezago + Cuota_Minima_Año) / (Hasta_Bimestre - Desde_Bimestre + 1) * Signo, 2);
                                //    }
                                //}
                                //else
                                {
                                    if (((Importe_Rezago == Cuota_Minima_Año)
                                            || (((Sum_Adeudos_Periodo - Importe_Rezago) == Cuota_Minima_Año && Signo < 0)))
                                        && !Nueva_Cuota_Fija
                                        && !(Importe_Rezago == Cuota_Minima_Año && (Hasta_Bimestre - Desde_Bimestre + 1) == 1))
                                    {
                                        //APLICA LA CUOTA MÍNIMA EN EL PRIMER BIMESTRE INDICADO, EL RESTO DE BIMESTRES LOS DEJA EN CEROS
                                        if (Importe_Rezago == Cuota_Minima_Año || Signo > 0)
                                        {
                                            Dr["ALTA_BAJA"] = "SOB1";
                                            if (Signo > 0)
                                            {
                                                Dr["ALTA_BAJA"] = "SOB2";
                                            }
                                        }
                                        else
                                        {
                                            Dr["ALTA_BAJA"] = "SOB";
                                        }
                                        for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                        {
                                            if (Cont_Bimestres > Desde_Bimestre && Cuota_Minima_Año != 0)
                                            {
                                                Cuota_Minima_Año = 0;
                                            }
                                            Dr["BIMESTRE_" + Cont_Bimestres.ToString()] = Math.Round(Cuota_Minima_Año * Signo, 2);
                                        }
                                    }
                                    else
                                    {
                                        if (Nueva_Cuota_Fija && Signo < 0)
                                        {
                                            //APLICA LA CUOTA FIJA EN EL PRIMER BIMESTRE INDICADO, EL RESTO DE BIMESTRES LOS DEJA EN CERO
                                            Dr["ALTA_BAJA"] = "SOB";
                                            if (Cuota_Fija_Nueva != "")
                                            {
                                                Cuota_Fija = Convert.ToDecimal(Obtener_Dato_Consulta(Ope_Pre_Cuotas_Fijas.Campo_Total_Cuota_Fija, Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas, Ope_Pre_Cuotas_Fijas.Campo_No_Cuota_Fija + " = '" + Cuota_Fija_Nueva + "'"));// Sum_Adeudos_Periodo - Importe_Rezago;
                                            }
                                            for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                            {
                                                if (Cont_Bimestres > Desde_Bimestre && Cuota_Minima_Año != 0)
                                                {
                                                    Cuota_Fija = 0;
                                                }
                                                Dr["BIMESTRE_" + Cont_Bimestres.ToString()] = Math.Round(Cuota_Fija, 2);
                                            }
                                        }
                                        else
                                        {
                                            //PRORRATEA EL IMPORTE EN EL PERIODO INDICADO
                                            for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                            {
                                                Dr["BIMESTRE_" + Cont_Bimestres.ToString()] = Math.Round(Importe_Rezago / (Hasta_Bimestre - Desde_Bimestre + 1) * Signo, 2);
                                            }
                                        }
                                    }
                                }
                                Dt_Diferencias.Rows.Add(Dr);
                            }
                        }
                        Orden_Variacion.P_Dt_Diferencias = Dt_Diferencias;
                        Orden_Variacion.P_Cmmd = Cmmd;
                        Orden_Variacion.Aplicar_Variacion_Diferencias();

                        //Actualiza los propietarios y copropietarios
                        DataSet Ds_Co_Pro_Cuentas = new DataSet();
                        //String No_Orden_Anterior;

                        //Elimina los propietarios y copropietarios de la cuenta
                        Mi_SQL = "DELETE FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios;
                        Mi_SQL += " WHERE " + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_ID + "'";
                        Cmmd.CommandText = Mi_SQL;
                        Cmmd.ExecuteNonQuery();

                        ////Obtiene la orden anterior aplicada
                        //Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Orden_Variacion.Campo_No_Orden_Variacion + "),'') FROM " + Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion;
                        //Mi_SQL += " WHERE " + Ope_Pre_Orden_Variacion.Campo_No_Orden_Variacion + " < '" + Dt_Consultar_Orden_Variacion.Rows[0]["No_Orden_Variacion"].ToString().Trim() + "'";
                        ////Mi_SQL += " AND " + Ope_Pre_Orden_Variacion.Campo_Anio + " = " + Dt_Consultar_Orden_Variacion.Rows[0]["Anio"].ToString().Trim();
                        //Mi_SQL += " AND " + Ope_Pre_Orden_Variacion.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_ID + "'";
                        //Mi_SQL += " AND " + Ope_Pre_Orden_Variacion.Campo_Estatus + " = 'ACEPTADA'";
                        Mi_SQL = "SELECT * FROM " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion;
                        Mi_SQL += " WHERE " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_ID + "'";
                        Mi_SQL += " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + " = " + Convert.ToDouble(Dt_Consultar_Orden_Variacion.Rows[0]["NO_ORDEN_VARIACION"]).ToString();
                        Mi_SQL += " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus + " = 'BAJA'";
                        Mi_SQL += " UNION";
                        Mi_SQL += " SELECT * FROM " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion;
                        Mi_SQL += " WHERE " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_ID + "'";
                        Mi_SQL += " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + " < " + Convert.ToDouble(Dt_Consultar_Orden_Variacion.Rows[0]["NO_ORDEN_VARIACION"]).ToString();
                        Mi_SQL += " ORDER BY 1 DESC, 2";
                        Cmmd.CommandText = Mi_SQL;
                        No_Orden_Anterior = Cmmd.ExecuteOracleScalar().ToString();
                        if (No_Orden_Anterior != "Null" && No_Orden_Anterior != "")
                        {
                            if (!String.IsNullOrEmpty(No_Orden_Anterior.ToString()))
                            {
                                //Consulta los propietarios y copropietarios de la orden anterior
                                Mi_SQL = "SELECT * FROM " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion;
                                Mi_SQL += " WHERE " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + " = '" + No_Orden_Anterior + "'";
                                Mi_SQL += " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio + " = " + Anio_Orden_Anterior;
                                Mi_SQL += " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_ID + "'";
                                Mi_SQL += " ORDER BY " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " DESC, " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID;
                                Cmmd.CommandText = Mi_SQL;
                                OracleDataAdapter Da_Co_Pro_Cuentas = new OracleDataAdapter(Cmmd);
                                Da_Co_Pro_Cuentas.Fill(Ds_Co_Pro_Cuentas);
                                //Recorre los propietarios y copropietarios de la orden anterior
                                foreach (DataRow Registro in Ds_Co_Pro_Cuentas.Tables[0].Rows)
                                {
                                    //Registra los cropietarios y propietarios de la orden anterior
                                    Int32 Propietario_ID;
                                    Mi_SQL = "SELECT NVL(MAX(" + Cat_Pre_Propietarios.Campo_Propietario_ID + "),'0') FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios;
                                    Cmmd.CommandText = Mi_SQL;
                                    Propietario_ID = Convert.ToInt32(Cmmd.ExecuteOracleScalar().ToString()) + 1;

                                    Mi_SQL = "INSERT INTO " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " (";
                                    Mi_SQL += Cat_Pre_Propietarios.Campo_Propietario_ID + ", ";
                                    Mi_SQL += Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + ", ";
                                    Mi_SQL += Cat_Pre_Propietarios.Campo_Contribuyente_ID + ", ";
                                    Mi_SQL += Cat_Pre_Propietarios.Campo_Tipo + ", ";
                                    Mi_SQL += Cat_Pre_Propietarios.Campo_Usuario_Creo + ", ";
                                    Mi_SQL += Cat_Pre_Propietarios.Campo_Fecha_Creo + ") ";
                                    Mi_SQL += "VALUES (";
                                    Mi_SQL += "'" + Propietario_ID.ToString("0000000000") + "', ";
                                    Mi_SQL += "'" + Cuenta_Predial_ID + "', ";
                                    Mi_SQL += "'" + Registro["Contribuyente_ID"].ToString() + "', ";
                                    Mi_SQL += "'" + Registro["Tipo"].ToString() + "', ";
                                    Mi_SQL += "'" + Usuario + "', SYSDATE)";
                                    Cmmd.CommandText = Mi_SQL;
                                    Cmmd.ExecuteNonQuery();
                                }
                            }
                        }
                        //Modificar el estatus
                        Orden_Variacion.P_Cmmd = Cmmd;
                        Orden_Variacion.P_Año = Año_Calculo;
                        Orden_Variacion.P_Contrarecibo_Anio = Año_Calculo;
                        Orden_Variacion.P_Orden_Variacion_ID = Dt_Consultar_Orden_Variacion.Rows[0]["NO_ORDEN_VARIACION"].ToString().Trim();
                        Orden_Variacion.P_Contrarecibo_No_Contrarecibo = Dt_Consultar_Orden_Variacion.Rows[0]["NO_CONTRARECIBO"].ToString().Trim();
                        Orden_Variacion.P_Contrarecibo_Estatus = "POR PAGAR";
                        Orden_Variacion.P_Contrarecibo_Usuario = Cls_Sessiones.Nombre_Empleado;
                        Orden_Variacion.Modificar_Contrarecibo();
                        Orden_Variacion.P_Contrarecibo_Estatus = "POR PAGAR";
                        Orden_Variacion.Modificar_Calculo_Traslado();
                    }
                }
            }
            catch (OracleException Ex)
            {
                //Indicamos el mensaje 
                throw new Exception(Ex.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Revertir_Impuesto_Fraccionamiento
        ///DESCRIPCIÓN          : Realiza la reversion del impuesto de fraccionamiento con los datos anteriores
        ///PARAMETROS:          : Cmmd, pasa el command para ejecutar    
        ///CREO                 : Ismael Prieto Sánchez
        ///FECHA_CREO           : 16/Noviembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static void Revertir_Impuesto_Fraccionamiento(ref OracleCommand Cmmd, string Cuenta_Predial_ID, string Referencia, string No_Pago, string Usuario)
        {
            String Mi_SQL;
            String No_Convenio = "";
            Int32 No_Pago_Convenio = 0;
            DataSet Ds_Convenios = new DataSet();

            try
            {
                //Consulta si el pago tiene convenio
                Mi_SQL = "SELECT " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + ", ";
                Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago;
                Mi_SQL += " FROM " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                Mi_SQL += " WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago_Aplicado + " = '" + No_Pago + "'";
                Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio;
                Mi_SQL += " = " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Convenio;
                Mi_SQL += " ORDER BY " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago;
                Cmmd.CommandText = Mi_SQL;
                OracleDataAdapter Da_Convenios = new OracleDataAdapter(Cmmd);
                Da_Convenios.Fill(Ds_Convenios);
                //Valida si trae adeudos
                if (Ds_Convenios.Tables[0].Rows.Count > 0)
                {
                    //Asigna si es por convenio
                    if (!String.IsNullOrEmpty(Ds_Convenios.Tables[0].Rows[0][Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio].ToString()))
                    {
                        No_Convenio = Ds_Convenios.Tables[0].Rows[0][Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio].ToString();
                    }
                    if (!String.IsNullOrEmpty(Ds_Convenios.Tables[0].Rows[0][Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago].ToString()))
                    {
                        No_Pago_Convenio = Convert.ToInt32(Ds_Convenios.Tables[0].Rows[0][Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago].ToString());
                    }
                    //Recorre las parcialidades del convenio
                    foreach (DataRow Registro in Ds_Convenios.Tables[0].Rows)
                    {
                        //Pone como por pagar el pago de la parcialidad del convenio
                        Mi_SQL = "UPDATE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos;
                        Mi_SQL += " SET " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + " = 'POR PAGAR'";
                        Mi_SQL += ", " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago_Aplicado + " = NULL";
                        Mi_SQL += ", " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Usuario + "'";
                        Mi_SQL += ", " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_SQL += " WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + No_Convenio + "'";
                        Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + " = " + Registro[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago].ToString();
                        Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago_Aplicado + " = '" + No_Pago + "'";
                        Cmmd.CommandText = Mi_SQL;
                        Cmmd.ExecuteNonQuery();

                        //Actualiza el anticipo si es el primer pago
                        if (Convert.ToInt32(Registro[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago].ToString()) == 1)
                        {
                            //Actualiza el anticipo del convenio
                            Mi_SQL = "UPDATE " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos;
                            Mi_SQL += " SET " + Ope_Pre_Convenios_Fraccionamientos.Campo_Anticipo + " = 'POR PAGAR', ";
                            Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                            Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL += " WHERE " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + No_Convenio + "'";
                            Cmmd.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                            Cmmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                        }
                    }
                    //Actualiza el convenio
                    Mi_SQL = "UPDATE " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + " SET " + Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus + " = 'ACTIVO'";
                    Mi_SQL += ", " + Ope_Pre_Convenios_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                    Mi_SQL += Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE";
                    Mi_SQL += " WHERE " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + No_Convenio + "'";
                    Cmmd.CommandText = Mi_SQL;
                    Cmmd.ExecuteNonQuery();
                }

                //Actualiza el impuesto de fraccionamiento
                Mi_SQL = "UPDATE " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos;
                if (No_Pago_Convenio <= 1)
                {
                    Mi_SQL += " SET " + Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + " = 'POR PAGAR', ";
                }
                else
                {
                    Mi_SQL += " SET " + Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + " = 'PARCIAL', ";
                }
                Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                Mi_SQL += Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL += " WHERE " + Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + " = '" + Referencia.Substring(5) + "'";
                Cmmd.CommandText = Mi_SQL;
                Cmmd.ExecuteNonQuery();
            }
            catch (OracleException Ex)
            {
                //Indicamos el mensaje 
                throw new Exception(Ex.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Revertir_Derechos_Supervision
        ///DESCRIPCIÓN          : Realiza la reversion de los derechos de supervision con los datos anteriores
        ///PARAMETROS:          : Cmmd, pasa el command para ejecutar    
        ///CREO                 : Ismael Prieto Sánchez
        ///FECHA_CREO           : 16/Noviembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static void Revertir_Derechos_Supervision(ref OracleCommand Cmmd, string Cuenta_Predial_ID, string Referencia, string No_Pago, string Usuario)
        {
            String Mi_SQL;
            String No_Convenio = "";
            Int32 No_Pago_Convenio = 0;
            DataSet Ds_Convenios = new DataSet();

            try
            {
                //Consulta si el pago tiene convenio
                Mi_SQL = "SELECT " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + ", ";
                Mi_SQL += Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago;
                Mi_SQL += " FROM " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                Mi_SQL += " WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago_Aplicado + " = '" + No_Pago + "'";
                Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio;
                Mi_SQL += " = " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Convenio;
                Mi_SQL += " ORDER BY " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago;
                Cmmd.CommandText = Mi_SQL;
                OracleDataAdapter Da_Convenios = new OracleDataAdapter(Cmmd);
                Da_Convenios.Fill(Ds_Convenios);
                //Valida si trae adeudos
                if (Ds_Convenios.Tables[0].Rows.Count > 0)
                {
                    //Asigna si es por convenio
                    if (!String.IsNullOrEmpty(Ds_Convenios.Tables[0].Rows[0][Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio].ToString()))
                    {
                        No_Convenio = Ds_Convenios.Tables[0].Rows[0][Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio].ToString();
                    }
                    if (!String.IsNullOrEmpty(Ds_Convenios.Tables[0].Rows[0][Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago].ToString()))
                    {
                        No_Pago_Convenio = Convert.ToInt32(Ds_Convenios.Tables[0].Rows[0][Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago].ToString());
                    }
                    //Recorre las parcialidades del convenio
                    foreach (DataRow Registro in Ds_Convenios.Tables[0].Rows)
                    {
                        //Pone como por pagar el pago de la parcialidad del convenio
                        Mi_SQL = "UPDATE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision;
                        Mi_SQL += " SET " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Estatus + " = 'POR PAGAR'";
                        Mi_SQL += ", " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago_Aplicado + " = NULL";
                        Mi_SQL += ", " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Usuario + "'";
                        Mi_SQL += ", " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_SQL += " WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + No_Convenio + "'";
                        Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago + " = " + Registro[Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago].ToString();
                        Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago_Aplicado + " = '" + No_Pago + "'";
                        Cmmd.CommandText = Mi_SQL;
                        Cmmd.ExecuteNonQuery();

                        //Actualiza el anticipo si es el primer pago
                        if (Convert.ToInt32(Registro[Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago].ToString()) == 1)
                        {
                            //Actualiza el anticipo del convenio
                            Mi_SQL = "UPDATE " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision;
                            Mi_SQL += " SET " + Ope_Pre_Convenios_Derechos_Supervision.Campo_Anticipo + " = 'POR PAGAR', ";
                            Mi_SQL += Ope_Pre_Convenios_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                            Mi_SQL += Ope_Pre_Convenios_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL += " WHERE " + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + No_Convenio + "'";
                            Cmmd.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                            Cmmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                        }
                    }
                    //Actualiza el convenio
                    Mi_SQL = "UPDATE " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + " SET " + Ope_Pre_Convenios_Derechos_Supervision.Campo_Estatus + " = 'ACTIVO'";
                    Mi_SQL += ", " + Ope_Pre_Convenios_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                    Mi_SQL += Ope_Pre_Convenios_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE";
                    Mi_SQL += " WHERE " + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + No_Convenio + "'";
                    Cmmd.CommandText = Mi_SQL;
                    Cmmd.ExecuteNonQuery();
                }

                //Actualiza los derechos en supervision
                Mi_SQL = "UPDATE " + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision;
                if (No_Pago_Convenio <= 1)
                {
                    Mi_SQL += " SET " + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus + " = 'POR PAGAR', ";
                }
                else
                {
                    Mi_SQL += " SET " + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus + " = 'PARCIAL', ";
                }
                Mi_SQL += Ope_Pre_Impuestos_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                Mi_SQL += Ope_Pre_Impuestos_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL += " WHERE " + Ope_Pre_Impuestos_Derechos_Supervision.Campo_No_Impuesto_Derecho_Supervision + " = '" + Referencia.Substring(5) + "'";
                Cmmd.CommandText = Mi_SQL;
                Cmmd.ExecuteNonQuery();
            }
            catch (OracleException Ex)
            {
                //Indicamos el mensaje 
                throw new Exception(Ex.ToString());
            }
        }

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Caja_Empleado
        /// DESCRIPCION : Consulta la caja que tiene abierta el empleado para poder realizar
        ///               la recolección de la misma
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 19-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Caja_Empleado(Cls_Ope_Pre_Cancelacion_Pago_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.

            try
            {
                //Consulta los datos generales de la caja que tiene abierta el empleado que requiere realizar la recolección del dinero
                Mi_SQL.Append("SELECT " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS Caja, ");
                Mi_SQL.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_Id + ", ");
                Mi_SQL.Append("(" + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Clave);
                Mi_SQL.Append("||' '||" + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Descripcion + ") AS Modulo, ");
                Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno);
                Mi_SQL.Append(" FROM " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + ", " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + ", " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo);
                Mi_SQL.Append(" WHERE " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id + " = " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_Id);
                Mi_SQL.Append(" AND " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Estatus + " = 'ABIERTO'");
                Mi_SQL.Append(" AND " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Modulo_Id + "=");
                Mi_SQL.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Modulo_Id);
                Mi_SQL.Append(" AND " + Ope_Caj_Turnos.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'");
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
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
        /// NOMBRE DE LA FUNCION: Consulta_Recibos_Cancelados_Acumulado
        /// DESCRIPCION : Consulta el numero acumulado de recibos cancelados por cajero por año
        /// PARAMETROS  : 
        /// CREO        : Ismael Prieto Sánchez
        /// FECHA_CREO  : 23-Abril-2012 3:40pm
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Recibos_Cancelados_Acumulado(Cls_Ope_Pre_Cancelacion_Pago_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.

            try
            {
                //Consulta los datos generales de la caja que tiene abierta el empleado que requiere realizar la recolección del dinero
                Mi_SQL.Append("SELECT NVL(COUNT(" + Ope_Caj_Pagos.Campo_No_Pago + "),'0') AS Acumulado");
                Mi_SQL.Append(" FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
                Mi_SQL.Append(" WHERE " + Ope_Caj_Pagos.Campo_Estatus + " = 'CANCELADO'");
                Mi_SQL.Append(" AND TO_CHAR(" + Ope_Caj_Pagos.Campo_Fecha + ",'YYYY') = " + DateTime.Now.Year);
                Mi_SQL.Append(" AND " + Ope_Caj_Pagos.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'");
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }

        private static String Obtener_Periodos_Bimestre(String Periodos, out Boolean Periodo_Corriente_Validado, out Boolean Periodo_Rezago_Validado)
        {
            String Periodo = "";
            int Indice = 0;
            Periodo_Corriente_Validado = false;
            Periodo_Rezago_Validado = false;

            if (Periodos.IndexOf("-") >= 0)
            {
                if (Periodos.Split('-').Length == 2)
                {
                    //Valida el segundo nodo del arreglo
                    if (Periodos.Split('-').GetValue(1).ToString().IndexOf("/") >= 0)
                    {
                        Periodo = Periodos.Split('-').GetValue(0).ToString().Trim().Substring(0, 1);
                        Periodo += "-";
                        Periodo += Periodos.Split('-').GetValue(1).ToString().Trim().Substring(0, 1);
                        Periodo_Rezago_Validado = true;
                    }
                    else
                    {
                        Periodo = Periodos.Split('-').GetValue(0).ToString().Replace("/", "-").Trim();
                        Periodo_Corriente_Validado = true;
                    }
                }
                else
                {
                    if (Periodos.Contains("/"))
                    {
                        Indice = Periodos.IndexOf("/");
                        Periodo = Periodos.Substring(Indice - 1, 1);
                        Periodo += "-";
                        Indice = Periodos.IndexOf("/", Indice + 1);
                        Periodo += Periodos.Substring(Indice - 1, 1);
                        Periodo_Rezago_Validado = true;
                    }
                    else
                    {
                        Periodo = Periodos.Substring(0, 3);
                        Periodo_Corriente_Validado = true;
                    }
                }
            }
            return Periodo;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta
        ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
        ///PARAMETROS:     
        ///CREO                 : Antonio Salvador Benvides Guardado
        ///FECHA_CREO           : 24/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static String Obtener_Dato_Consulta(String Campo, String Tabla, String Condiciones)
        {
            String Mi_SQL;
            String Dato_Consulta = "";

            try
            {
                Mi_SQL = "SELECT " + Campo;
                if (Tabla != "")
                {
                    Mi_SQL += " FROM " + Tabla;
                }
                if (Condiciones != "")
                {
                    Mi_SQL += " WHERE " + Condiciones;
                }

                OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Dr_Dato.Read())
                {
                    if (Dr_Dato[0] != null)
                    {
                        Dato_Consulta = Dr_Dato[0].ToString();
                    }
                    else
                    {
                        Dato_Consulta = "";
                    }
                    Dr_Dato.Close();
                }
                else
                {
                    Dato_Consulta = "";
                }
                if (Dr_Dato != null)
                {
                    Dr_Dato.Close();
                }
                Dr_Dato = null;
            }
            catch
            {
            }
            finally
            {
            }

            return Dato_Consulta;
        }
    }
}