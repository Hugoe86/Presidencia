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
using Presidencia.Operacion_Predial_Historial_Pagos.Negocio;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
/// <summary>
/// Summary description for Cls_Ope_Pre_Historial_Pagos_Datos
/// </summary>
/// 
namespace Presidencia.Operacion_Predial_Historial_Pagos.Datos
{
    public class Cls_Ope_Pre_Historial_Pagos_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Historial_Pagos
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene el tipo de Predio
        ///PARAMETROS:     
        ///             1.  Tipo Predio ID 
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 05/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Historial_Pagos(Cls_Ope_Pre_Historial_Pago_Negocio Datos)
        {
            DataTable Dt_Traslado_Dominio = new DataTable();
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Ope_Pre_Adeudos_Folio.Tabla_Ope_Pre_Adeudos_Folio + "." + Ope_Pre_Adeudos_Folio.Campo_Folio + " as Folio,";
                Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Cuenta_Predial_Id  + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Fecha + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_No_Recibo  + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_No_Operacion  + ",";
                Mi_SQL = Mi_SQL + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja  + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja  + " as Numero_Caja,";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Clave_Banco  + " ";
                //Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Monto  + "";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " inner join ";
                Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + " on ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "=";
                Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Cuenta_Predial_Id + " inner join ";    
                Mi_SQL =Mi_SQL + Ope_Pre_Adeudos_Folio.Tabla_Ope_Pre_Adeudos_Folio + " on ";
                Mi_SQL =Mi_SQL + Ope_Pre_Adeudos_Folio .Tabla_Ope_Pre_Adeudos_Folio + "." + Ope_Pre_Adeudos_Folio .Campo_No_Pago +"=";
                Mi_SQL =Mi_SQL + Ope_Pre_Pagos .Tabla_Ope_Pre_Pagos +"."+Ope_Pre_Pagos .Campo_No_Pago + " Inner Join ";
                Mi_SQL =Mi_SQL + Cat_Pre_Cajas .Tabla_Cat_Pre_Caja + " on ";
                Mi_SQL =Mi_SQL + Ope_Pre_Pagos .Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Caja_Id + "=";
                Mi_SQL =Mi_SQL + Cat_Pre_Cajas .Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id + " inner join ";
                Mi_SQL = Mi_SQL + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + " on ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Modulo_Id + "=";
                Mi_SQL =Mi_SQL + Cat_Pre_Modulos .Tabla_Cat_Pre_Modulo +"." + Cat_Pre_Modulos .Campo_Modulo_Id +" ";
                if ((Datos.P_Entre_Fecha != "" && Datos.P_Entre_Fecha != null) && (Datos.P_Y_Fecha != "" && Datos.P_Y_Fecha != null))
                {
                    if (Mi_SQL.Contains("Where"))
                    {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Fecha + " >= '" + Datos.P_Entre_Fecha + "' AND " + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Fecha + " <='" + Datos.P_Y_Fecha + "' ";
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + " Where " + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Fecha + " >= '" + Datos.P_Entre_Fecha + "' AND " + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Fecha + " <='" + Datos.P_Y_Fecha + "' ";
                    }
                }
                if (!string.IsNullOrEmpty(Datos.P_Recibo_Inicial) && string.IsNullOrEmpty(Datos.P_Recibo_Final))
                {
                    if (Mi_SQL.Contains("Where"))
                    {
                        Mi_SQL =Mi_SQL + " AND '" + Datos .P_Recibo_Inicial + "' <= " + Ope_Pre_Pagos .Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos .Campo_No_Recibo + " AND '" + Datos .P_Recibo_Final + "' >=" + Ope_Pre_Pagos .Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos .Campo_No_Recibo + " ";
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + " Where '" + Datos.P_Recibo_Inicial + "' <= " + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_No_Recibo + " AND '" + Datos.P_Recibo_Final + "' >=" + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_No_Recibo + " ";
                    }
                }
                if(!string .IsNullOrEmpty (Datos .P_Caja  ))
                {
                    if (Mi_SQL.Contains("Where"))
                    {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + "='" + Datos.P_Caja + "' ";
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + "='" + Datos.P_Caja + "' ";
                    }
                }
                if (Datos.P_Lugar_Pago!=null && Datos .P_Lugar_Pago !="SELECCIONE" )
                {
                    if (Mi_SQL.Contains("Where"))
                    {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Descripcion + "='" + Datos.P_Lugar_Pago + "' ";
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Descripcion + "='" + Datos.P_Lugar_Pago + "' ";
                    }
                }
                if (!string.IsNullOrEmpty(Datos.P_Cuenta_Predial_ID))
                {
                    if (Mi_SQL.Contains("Where"))
                    {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + "='" + Datos.P_Cuenta_Predial_ID + "'";
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + "='" + Datos.P_Cuenta_Predial_ID + "'";
                    }
                }
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Historial_Pagos_Detalles
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene el tipo de Predio
        ///PARAMETROS:     
        ///             1.  Tipo Predio ID 
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 05/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Historial_Pagos_Detalles(Cls_Ope_Pre_Historial_Pago_Negocio Datos)
        {
            DataTable Dt_Traslado_Dominio = new DataTable();
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT * ";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + " ";
               
                
                if (!string.IsNullOrEmpty(Datos.P_Cuenta_Predial_ID))
                {
                    if (Mi_SQL.Contains("Where"))
                    {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Cuenta_Predial_Id + "='" + Datos.P_Cuenta_Predial_ID + "'";
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + " Where " + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Cuenta_Predial_Id + "='" + Datos.P_Cuenta_Predial_ID + "'";
                    }
                    if (Mi_SQL.Contains("where"))
                    {
                        Mi_SQL = Mi_SQL + " AND  " + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Estatus + " <> 'CANCELADO' ";
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + " AND  " + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Estatus + " <> 'CANCELADO' ";
                    }
                }
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Historial_Pagos_Detalles
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene el tipo los folios de la cuenta predial
        ///PARAMETROS:     
        ///             1.  Tipo Predio ID 
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 13/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Detalles_Cuenta_Predial(Cls_Ope_Pre_Historial_Pago_Negocio Datos)
        {
            DataTable Dt_Traslado_Dominio = new DataTable();
            DataTable Tabla = new DataTable();
            try
            {
                for (int Cuenta_Folios = 0; Cuenta_Folios < Datos.P_Dt_Detalles_Cuenta.Rows.Count; Cuenta_Folios++)
                {
                    String Mi_SQL = "SELECT " + Ope_Pre_Adeudos_Folio.Tabla_Ope_Pre_Adeudos_Folio + "." + Ope_Pre_Adeudos_Folio.Campo_Folio + " as Folio,";
                    Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Cuenta_Predial_Id + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Documento + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Periodo_Corriente_Inicial + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Periodo_Corriente_Final + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Monto_Corriente + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Periodo_Rezago_Inicial + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Periodo_Rezago_Final + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Monto_Rezago + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Monto_Recargos_Ordinarios + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Monto_Recargos_Moratorios + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Honorarios + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Multas + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Gastos_Ejecucion + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Descuento_Recargos + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Descuento_Honorarios + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Descuento_Pronto_Pago + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Fecha + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_No_Recibo + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_No_Operacion + ",";
                    Mi_SQL = Mi_SQL + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " as Numero_Caja,";
                    Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Clave_Banco + " ";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " inner join ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + " on ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "=";
                    Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Cuenta_Predial_Id + " inner join ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Adeudos_Folio.Tabla_Ope_Pre_Adeudos_Folio + " on ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Adeudos_Folio.Tabla_Ope_Pre_Adeudos_Folio + "." + Ope_Pre_Adeudos_Folio.Campo_No_Pago + "=";
                    Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_No_Pago + " Inner Join ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " on ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Pagos.Tabla_Ope_Pre_Pagos + "." + Ope_Pre_Pagos.Campo_Caja_Id + "=";
                    Mi_SQL = Mi_SQL + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id + " inner join ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + " on ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Modulo_Id + "=";
                    Mi_SQL = Mi_SQL + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Modulo_Id + " ";
                    Mi_SQL = Mi_SQL + " Where " + Ope_Pre_Adeudos_Folio.Tabla_Ope_Pre_Adeudos_Folio + "." + Ope_Pre_Adeudos_Folio.Campo_Folio + "=" + Datos.P_Dt_Detalles_Cuenta.Rows[Cuenta_Folios]["Folio"];
                    DataTable  Dt_Folios_Guardados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    if (Dt_Folios_Guardados != null)
                    {
                        Tabla = Dt_Folios_Guardados.Clone();
                        DataRow No_Folio;
                        No_Folio = Tabla.NewRow();
                        No_Folio["FOLIO"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["FOLIO"].ToString();
                        No_Folio["CUENTA_PREDIAL_ID"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["CUENTA_PREDIAL_ID"].ToString();
                        No_Folio["DOCUMENTO"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["DOCUMENTO"].ToString();
                        No_Folio["PERIODO_CORRIENTE_INICIAL"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["PERIODO_CORRIENTE_INICIAL"].ToString();
                        No_Folio["PERIODO_CORRIENTE_FINAL"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["PERIODO_CORRIENTE_FINAL"].ToString();
                        No_Folio["MONTO_CORRIENTE"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["MONTO_CORRIENTE"].ToString();
                        No_Folio["PERIODO_REZAGO_INICIAL"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["PERIODO_REZAGO_INICIAL"].ToString();
                        No_Folio["PERIODO_REZAGO_FINAL"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["PERIODO_REZAGO_FINAL"].ToString();
                        No_Folio["MONTO_REZAGO"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["MONTO_REZAGO"].ToString();
                        No_Folio["MONTO_RECARGOS_ORDINARIOS"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["MONTO_RECARGOS_ORDINARIOS"].ToString();
                        No_Folio["MONTO_RECARGOS_MORATORIOS"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["MONTO_RECARGOS_MORATORIOS"].ToString();
                        No_Folio["HONORARIOS"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["HONORARIOS"].ToString();
                        No_Folio["MULTAS"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["MULTAS"].ToString();
                        No_Folio["GASTOS_EJECUCION"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["GASTOS_EJECUCION"].ToString();
                        if (Dt_Folios_Guardados.Rows[Cuenta_Folios]["DESCUENTO_RECARGOS"].ToString().Trim() != "")
                        {
                            No_Folio["DESCUENTO_RECARGOS"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["DESCUENTO_RECARGOS"].ToString();
                        }
                        if (Dt_Folios_Guardados.Rows[Cuenta_Folios]["DESCUENTO_HONORARIOS"].ToString().Trim() != "")
                        {
                            No_Folio["DESCUENTO_HONORARIOS"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["DESCUENTO_HONORARIOS"].ToString();
                        }
                        if (Dt_Folios_Guardados.Rows[Cuenta_Folios]["DESCUENTO_PRONTO_PAGO"].ToString().Trim() != "")
                        {
                            No_Folio["DESCUENTO_PRONTO_PAGO"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["DESCUENTO_PRONTO_PAGO"].ToString();
                        }
                            if (Dt_Folios_Guardados.Rows[Cuenta_Folios]["FECHA"].ToString().Trim() != "")
                        {
                            No_Folio["FECHA"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["FECHA"].ToString();
                        }
                        No_Folio["NO_RECIBO"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["NO_RECIBO"].ToString();
                        No_Folio["NO_OPERACION"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["NO_OPERACION"].ToString();
                        No_Folio["NUMERO_CAJA"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["NUMERO_CAJA"].ToString();
                        No_Folio["CUENTA_PREDIAL"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["CUENTA_PREDIAL"].ToString();
                        No_Folio["CLAVE_BANCO"] = Dt_Folios_Guardados.Rows[Cuenta_Folios]["CLAVE_BANCO"].ToString();
                        if (Tabla.Rows.Count == 0)
                        {
                            Tabla.Rows.InsertAt(No_Folio, 0);
                            
                        }
                        else
                        {
                            Tabla.Rows.InsertAt(No_Folio, Tabla.Rows.Count);
                        }
                    }
                    
                }
                
                
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
    }
}
