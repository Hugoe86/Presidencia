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
using Presidencia.Operacion_ReActivar_Pago.Negocio;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using Presidencia.Caja_Pagos.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Cuotas_Minimas.Negocio;
using Presidencia.Operacion_Resumen_Predio.Negocio;
/// <summary>
/// Summary description for Cls_Ope_Pre_Cancelacion_Pago
/// </summary>

namespace Presidencia.Operacion_ReActivar_Pago.Datos
{
    public class Cls_Ope_Pre_ReActivar_Pago_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Reactivar_Pago
        ///DESCRIPCIÓN: Actualiza en la Base de Datos una Cancelacion de Pago
        ///PARAMENTROS:     
        ///             1. Cancelacion.     Instancia de la Clase de Salarios Mínimos 
        ///                                 con los datos del Registro 
        ///                                 que va a ser Actualizado.
        ///CREO: Sergio Manuel Gallardo Andrade
        ///FECHA_CREO: 28/octubre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Reactivar_Pago(Cls_Ope_Pre_ReActivar_Pago_Negocio Cancelacion)
        {
            String Mensaje = "";
            StringBuilder Mi_SQL_Detalles = new StringBuilder();//Variable que almacenara la consulta.
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            String No_Calculo = "";
            DataSet Dt_Adeudos_Predial_Cajas = new DataSet();
            DataSet Ds_Datos = new DataSet();
            Int16 Año_Calculo = 0;
            string Proteccion_Pago = "";
            Boolean Bandero_Entro = false;
            Cls_Ope_Caj_Pagos_Negocio Caja = new Cls_Ope_Caj_Pagos_Negocio();
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();//Abre la conexión a la base de datos
            Trans = Cn.BeginTransaction();//Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Cmd.Connection = Cn; //Establece la conexión a la base de datos
            Cmd.Transaction = Trans;//Abre la transacción para la ejecución en la base de datos
            try
            {

                String Mi_SQL_PASIVO_DECS = "SELECT Distinct(REFERENCIA) FROM  " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                Mi_SQL_PASIVO_DECS = Mi_SQL_PASIVO_DECS + " WHERE  " + Ope_Ing_Pasivo.Campo_No_Pago + " ='" + Cancelacion.P_No_Pago + "'";
                Cmd.CommandText = Mi_SQL_PASIVO_DECS;
                String REFERENCIA = (String)Cmd.ExecuteScalar();

                Mi_SQL_PASIVO_DECS = "SELECT Distinct(CUENTA_PREDIAL_ID) FROM  " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                Mi_SQL_PASIVO_DECS = Mi_SQL_PASIVO_DECS + " WHERE  " + Ope_Ing_Pasivo.Campo_No_Pago + " ='" + Cancelacion.P_No_Pago + "'";
                Cmd.CommandText = Mi_SQL_PASIVO_DECS;
                String Cuenta_Predial_ID = Cmd.ExecuteScalar().ToString();

                //consulta la cuenta predial
                if (REFERENCIA.StartsWith("TD") && Cuenta_Predial_ID != "")
                {
                    No_Calculo = REFERENCIA.Substring(2);
                    Año_Calculo = 0;
                    if (No_Calculo.Length > 4)
                    {
                        Año_Calculo = Convert.ToInt16(No_Calculo.Substring(No_Calculo.Length - 4));
                        No_Calculo = No_Calculo.Substring(0, No_Calculo.Length - 4);
                    }
                }

                String Mi_SQL = "UPDATE " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                Mi_SQL = Mi_SQL + " SET " + Ope_Caj_Pagos.Campo_Comentarios + " = '" + Cancelacion.P_Comentarios + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Caj_Pagos.Campo_Estatus + " = 'PAGADO'";
                Mi_SQL = Mi_SQL + "," + Ope_Caj_Pagos.Campo_Motivo_Cancelacion_ID + " = '" + Cancelacion.P_Motivo_Cancelacion_Id + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Caj_Pagos.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Caj_Pagos.Campo_Fecha_Modifico + " = sysdate";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Pagos.Campo_No_Pago + " = '" + Cancelacion.P_No_Pago + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                String Mi_SQL_PASIVO = "UPDATE " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                Mi_SQL_PASIVO = Mi_SQL_PASIVO + " SET " + Ope_Ing_Pasivo.Campo_Estatus + " ='PAGADO', ";
                Mi_SQL_PASIVO = Mi_SQL_PASIVO + Ope_Ing_Pasivo.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "'";
                Mi_SQL_PASIVO = Mi_SQL_PASIVO + "," + Ope_Ing_Pasivo.Campo_Fecha_Modifico + " = sysdate";
                Mi_SQL_PASIVO = Mi_SQL_PASIVO + " WHERE " + Ope_Ing_Pasivo.Campo_No_Pago + " ='" + Cancelacion.P_No_Pago + "'";
                Cmd.CommandText = Mi_SQL_PASIVO;
                Cmd.ExecuteNonQuery();

                String Mi_SQL_DATOS = "SELECT " + Ope_Caj_Pagos.Campo_Caja_ID + ", " + Ope_Caj_Pagos.Campo_No_Operacion + ", " + Ope_Caj_Pagos.Campo_No_Recibo + ", ";
                Mi_SQL_DATOS = Mi_SQL_DATOS + Ope_Caj_Pagos.Campo_Fecha + ", " + Ope_Caj_Pagos.Campo_Total + " FROM  " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                Mi_SQL_DATOS = Mi_SQL_DATOS + " WHERE  " + Ope_Caj_Pagos.Campo_No_Pago + " ='" + Cancelacion.P_No_Pago + "'";
                Cmd.CommandText = Mi_SQL_DATOS;
                OracleDataAdapter Da_Datos = new OracleDataAdapter(Cmd);
                Da_Datos.Fill(Ds_Datos);

                if (Ds_Datos.Tables.Count > 0)
                {
                    Proteccion_Pago = "PAGADO/" + Convert.ToInt32(Ds_Datos.Tables[0].Rows[0]["Caja_ID"].ToString()) + "/" + Convert.ToInt32(Ds_Datos.Tables[0].Rows[0]["NO_Operacion"].ToString()) + "/" + String.Format("{0:yyyy.MM.dd}", Convert.ToDateTime(Ds_Datos.Tables[0].Rows[0]["Fecha"])) + "/" + String.Format("{0:HH:mm:ss}", Convert.ToDateTime(Ds_Datos.Tables[0].Rows[0]["Fecha"])) + "/" + String.Format("{0:###,##0.00}", Ds_Datos.Tables[0].Rows[0]["Total"]) + "/" + Ds_Datos.Tables[0].Rows[0]["No_Recibo"].ToString();
                    if (Cuenta_Predial_ID != "")
                    {
                        Proteccion_Pago += "/" + Cuenta_Predial_ID;
                    }
                }

                Mi_SQL_Detalles.Length = 0;

                if (REFERENCIA.StartsWith("CDER"))
                {
                    Mi_SQL_Detalles.Length = 0;
                    //Actualiza el estatus del convenio de derechos de supervisión A PAGADO
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Convenios_Derechos_Supervision.Campo_Anticipo + " = 'PAGADO', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Convenios_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Convenios_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL_Detalles.Append(" WHERE " + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + REFERENCIA.Substring(4, 10) + "'");
                    Mi_SQL_Detalles.Append(" AND " + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Reestructura + " IS NULL");
                    Cmd.CommandText = Mi_SQL_Detalles.ToString(); //Asigna la inserción para ser ejecutada
                    Cmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Mi_SQL_Detalles.Length = 0;
                    //Actualiza el estatus del detalle del convenio  de derechos de supervisión A PAGADO
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Estatus + " = 'PAGADO', ");
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
                    //Actualiza el ANTICIPO del Convenio A PAGADO
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Convenios_Fraccionamientos.Campo_Anticipo + " = 'PAGADO', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Convenios_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL_Detalles.Append(" WHERE " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + REFERENCIA.Substring(4, 10) + "'");
                    Cmd.CommandText = Mi_SQL_Detalles.ToString(); //Asigna la inserción para ser ejecutada
                    Cmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Mi_SQL_Detalles.Length = 0;
                    //Actualiza el estatus del detalle del convenio A PAGADO
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + " = 'PAGADO', ");
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
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'PAGADO', ");
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
                    Mi_SQL_Detalles.Append(Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'PAGADO' ");
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
                    //Actualiza el ANTICIPO de la reestructura del Convenio de traslado de dominio A PAGADO
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Convenios_Traslados_Dominio.Campo_Anticipo_Reestructura + " = 'PAGADO', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Convenios_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Convenios_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL_Detalles.Append(" WHERE " + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + REFERENCIA.Substring(4, 10) + "'");
                    Cmd.CommandText = Mi_SQL_Detalles.ToString(); //Asigna la inserción para ser ejecutada
                    Cmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Mi_SQL_Detalles.Length = 0;
                    //Actualiza el estatus del detalle de la reestructura del convenio de traslado de dominio A PAGADO
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Estatus + " = 'PAGADO', ");
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
                    //Actualiza el ANTICIPO del Convenio de traslado de dominio A PAGADO
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Convenios_Fraccionamientos.Campo_Anticipo + " = 'PAGADO', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Convenios_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL_Detalles.Append(" WHERE " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + REFERENCIA.Substring(4, 10) + "'");
                    Cmd.CommandText = Mi_SQL_Detalles.ToString(); //Asigna la inserción para ser ejecutada
                    Cmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Mi_SQL_Detalles.Length = 0;
                    //Actualiza el estatus del detalle del convenio de traslado de dominio A PAGADO
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + " = 'PAGADO', ");
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
                    //Actualiza el ANTICIPO de la reestructura del Convenio de derechos de supervisión A PAGADO
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Convenios_Derechos_Supervision.Campo_Anticipo_Reestructura + " = 'PAGADO', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Convenios_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Convenios_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL_Detalles.Append(" WHERE " + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + REFERENCIA.Substring(4, 10) + "'");
                    Cmd.CommandText = Mi_SQL_Detalles.ToString(); //Asigna la inserción para ser ejecutada
                    Cmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Mi_SQL_Detalles.Length = 0;
                    //Actualiza el estatus del detalle de la reestructura del convenio de derechos de supervisión A PAGADO
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Estatus + " = 'PAGADO', ");
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
                    //Actualiza el ANTICIPO de la reestructura del Convenio de fraccionamiento A PAGADO
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Convenios_Fraccionamientos.Campo_Anticipo_Reestructura + " = 'PAGADO', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Convenios_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL_Detalles.Append(" WHERE " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + REFERENCIA.Substring(4, 10) + "'");
                    Cmd.CommandText = Mi_SQL_Detalles.ToString(); //Asigna la inserción para ser ejecutada
                    Cmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Mi_SQL_Detalles.Length = 0;
                    //Actualiza el estatus del detalle de la reestructura del convenio de fraccionamiento A PAGADO
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + " = 'PAGADO', ");
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
                    Mi_SQL_Detalles.Length = 0;
                    //Actualiza el estatus del impuesto de derecho de supervisión a PAGADO
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus + " = 'PAGADO', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Impuestos_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Impuestos_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL_Detalles.Append(" WHERE " + Ope_Pre_Impuestos_Derechos_Supervision.Campo_No_Impuesto_Derecho_Supervision + " = '" + REFERENCIA.Substring(5, 10) + "'");
                    Cmd.CommandText = Mi_SQL_Detalles.ToString(); //Asigna la inserción para ser ejecutada
                    Cmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Bandero_Entro = true;
                }
                else if (REFERENCIA.StartsWith("IMP"))
                {
                    Mi_SQL_Detalles.Length = 0;
                    //Actualiza el estatus del impuesto de fraccionamiento a PAGADO
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + " = 'PAGADO', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Impuestos_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL_Detalles.Append(" WHERE " + Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + " = '" + REFERENCIA.Substring(5, 10) + "'");
                    Cmd.CommandText = Mi_SQL_Detalles.ToString(); //Asigna la inserción para ser ejecutada
                    Cmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Bandero_Entro = true;
                }
                else if (REFERENCIA.StartsWith("TD"))
                {
                    Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
                    Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Rs_Modificar_Calculo = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
                    //obterner el numero de orden de variacion
                    DataSet Ds_Cuenta;
                    No_Calculo = REFERENCIA.Substring(2);
                    Año_Calculo = 0;
                    if (No_Calculo.Length > 4)
                    {
                        Año_Calculo = Convert.ToInt16(No_Calculo.Substring(No_Calculo.Length - 4));
                        No_Calculo = No_Calculo.Substring(0, No_Calculo.Length - 4);
                    }

                    Caja.P_No_Calculo = Convert.ToInt64(No_Calculo).ToString("0000000000");
                    Caja.P_Año_Calculo = Año_Calculo;
                    DataTable Dt_Orden_Variacion = Caja.Consultar_Datos_Calculo();

                    if (Dt_Orden_Variacion.Rows.Count > 0)
                    {
                        Cuenta_Predial_ID = Dt_Orden_Variacion.Rows[0]["Cuenta_Predial_ID"].ToString().Trim();
                        M_Orden_Negocio.P_Orden_Variacion_ID = Dt_Orden_Variacion.Rows[0]["No_Orden_Variacion"].ToString().Trim();
                        Caja.P_Orden_Variacion_ID = Dt_Orden_Variacion.Rows[0]["No_Orden_Variacion"].ToString().Trim();
                        Caja.P_Anio = Dt_Orden_Variacion.Rows[0]["Anio"].ToString().Trim();
                        string Status = Dt_Orden_Variacion.Rows[0]["Estatus"].ToString().Trim();
                        DataTable Dt_Consultar_Orden_Variacion = Caja.Consultar_Orden_Variacion();


                        if (Status == "ACEPTADA")
                        {
                            M_Orden_Negocio.P_Cuenta_Predial = Dt_Orden_Variacion.Rows[0]["Cuenta_Predial"].ToString().Trim();
                            M_Orden_Negocio.P_Contrarecibo = null;
                            M_Orden_Negocio.P_Estatus_Cuenta = "VALIDADO";
                            M_Orden_Negocio.P_Agrupar_Dinamico = "True";
                            Ds_Cuenta = M_Orden_Negocio.Consulta_Datos_Cuenta();
                            Cargar_Datos(Cuenta_Predial_ID);
                            Cargar_Variacion(ref Cmd, Dt_Orden_Variacion, Ds_Cuenta, Dt_Consultar_Orden_Variacion.Rows[0]["NO_CONTRARECIBO"].ToString().Trim());
                        }
                    }
                    Bandero_Entro = true;
                }
                else if (Char.IsLetter(REFERENCIA, 1) && !REFERENCIA.StartsWith("OTRPAG"))
                {
                    Mi_SQL_Detalles.Append("UPDATE " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias);
                    Mi_SQL_Detalles.Append(" SET " + Ope_Pre_Constancias.Campo_Estatus + " = 'PAGADA', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Constancias.Campo_Proteccion_Pago + " = '" + Proteccion_Pago + "', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Constancias.Campo_Usuario_Modifico + " = '" + Cancelacion.P_Usuario + "', ");
                    Mi_SQL_Detalles.Append(Ope_Pre_Constancias.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL_Detalles.Append(" WHERE " + Ope_Pre_Constancias.Campo_Folio + " = '" + REFERENCIA + "'");
                    Cmd.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Int32 filas_afectadas = Cmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                    if (filas_afectadas > 0)
                    {
                        Bandero_Entro = true;
                    }
                }
                else if (Cuenta_Predial_ID != "" && !Bandero_Entro) //Predial
                {
                    //Seleccionar el adeudo pagado
                    Mi_SQL_DATOS = "Select * from " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial;
                    Mi_SQL_DATOS = Mi_SQL_DATOS + " WHERE " + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Pago + " ='" + Cancelacion.P_No_Pago + "'";
                    Cmd.CommandText = Mi_SQL_DATOS;
                    OracleDataAdapter Da_Adeudos_Predial_Cajas = new OracleDataAdapter(Cmd);
                    Da_Adeudos_Predial_Cajas.Fill(Dt_Adeudos_Predial_Cajas);
                    DataTable Dt_Adeudo = new DataTable();
                    //Dt_Adeudo = Dt_Adeudos_Predial_Cajas.Tables[0];
                    Aplicar_Adeudos_Predial(ref Cmd, Dt_Adeudos_Predial_Cajas.Tables[0], Cuenta_Predial_ID, Cancelacion.P_No_Pago, Cancelacion.P_Usuario);
                    Bandero_Entro = true;
                }
                if (!Bandero_Entro && !REFERENCIA.StartsWith("OTRPAG"))
                {
                    throw new Exception("Error: No es posible realizar el pago, debido a que no hay la sufiente información favor de verificarlo.");
                }
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
        ///NOMBRE DE LA FUNCIÓN : Aplicar_Adeudos_Predial
        ///DESCRIPCIÓN          : Realiza la aplicacion de los adeudos de predial
        ///PARAMETROS:          : Cmmd, pasa el command para ejecutar    
        ///CREO                 : Ismael Prieto Sánchez
        ///FECHA_CREO           : 13/Octubre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static void Aplicar_Adeudos_Predial(ref OracleCommand Cmmd, DataTable Dt_Adeudos, string Cuenta_Predial_ID, string No_Pago, string Usuario)
        {
            String Mi_SQL; //Variable para ejecutar el query
            ////String No_Recargo;
            //Double Monto_Corriente = 0;
            //Double Monto_Rezago = 0;
            //Double Monto_Honorarios = 0;
            //Double Monto_Recargos = 0;
            //Double Monto_Moratorios = 0;
            //Double Monto_Descuento_Recargos = 0;
            //Double Monto_Descuento_Moratorios = 0;
            //Double Monto_Descuento_Honorarios = 0;
            //Double Monto_Descuento_Pronto_Pago = 0;
            DataRow Registro_Perido;
            Int32 Bimestre_Inicial = 0;
            Int32 Bimestre_Final = 0;
            Int32 Anio_Inicial = 0;
            Int32 Anio_Final = 0;
            String Periodo_Corriente = "";
            String Periodo_Rezago = "";

            try
            {
                //Obtiene el periodo inicial y final
                if (Dt_Adeudos.Rows.Count > 0)
                {
                    //Selecciona el periodo inicial
                    Registro_Perido = Dt_Adeudos.Rows[0];
                    Bimestre_Inicial = Convert.ToInt32(Registro_Perido["BIMESTRE"].ToString());
                    Anio_Inicial = Convert.ToInt32(Registro_Perido["ANIO"].ToString());
                    //Selecciona el periodo final
                    Registro_Perido = Dt_Adeudos.Rows[Dt_Adeudos.Rows.Count - 1];
                    Bimestre_Final = Convert.ToInt32(Registro_Perido["BIMESTRE"].ToString());
                    Anio_Final = Convert.ToInt32(Registro_Perido["ANIO"].ToString());
                    //Asinga los periodos
                    if (Anio_Inicial == Anio_Final && Anio_Inicial < DateTime.Now.Year)
                    {
                        Periodo_Rezago = Bimestre_Inicial.ToString() + "/" + Anio_Inicial + "-" + Bimestre_Final + "/" + Anio_Final;
                    }
                    else
                    {
                        if (Anio_Inicial == Anio_Final && Anio_Inicial >= DateTime.Now.Year)
                        {
                            Periodo_Corriente = Bimestre_Inicial.ToString() + "/" + Anio_Inicial + "-" + Bimestre_Final + "/" + Anio_Final;
                        }
                        else
                        {
                            if (Anio_Inicial < Anio_Final && Anio_Inicial < DateTime.Now.Year)
                            {
                                Periodo_Rezago = Bimestre_Inicial + "/" + Anio_Inicial + "-6/" + Anio_Inicial;
                            }
                            if (Anio_Final >= DateTime.Now.Year)
                            {
                                Periodo_Corriente = "1/" + Anio_Final + "-" + Bimestre_Final + "/" + Anio_Final;
                            }
                        }
                    }
                }
                //Recorre los adeudos de predial para aplicarlos
                foreach (DataRow Registro in Dt_Adeudos.Rows)
                {
                    ////Inserta el adeudo pagado
                    //Mi_SQL = "INSERT INTO " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial + " (" + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Pago + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Adeudo + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_Anio + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_Bimestre;
                    //Mi_SQL += ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_Monto + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_Usuario_Creo + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_Fecha_Creo + ")";
                    //Mi_SQL += " VALUES ('" + No_Pago + "', '" + Registro["No_Adeudo"].ToString() + "', " + Registro["Anio"].ToString() + ", " + Registro["Bimestre"].ToString() + ", " + Registro["Monto"].ToString();
                    //Mi_SQL += ", '" + Usuario + "', SYSDATE)";
                    //Cmmd.CommandText = Mi_SQL;
                    //Cmmd.ExecuteNonQuery();

                    //Actualiza el adeudo con el monto del pago
                    Mi_SQL = "UPDATE " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + " SET";
                    if (Convert.ToInt32(Registro["Bimestre"].ToString()) == 1)
                    {
                        Mi_SQL += " " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + " = " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1;
                    }
                    else if (Convert.ToInt32(Registro["Bimestre"].ToString()) == 2)
                    {
                        Mi_SQL += " " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + " = " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2;
                    }
                    else if (Convert.ToInt32(Registro["Bimestre"].ToString()) == 3)
                    {
                        Mi_SQL += " " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + " = " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3;
                    }
                    else if (Convert.ToInt32(Registro["Bimestre"].ToString()) == 4)
                    {
                        Mi_SQL += " " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + " = " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4;
                    }
                    else if (Convert.ToInt32(Registro["Bimestre"].ToString()) == 5)
                    {
                        Mi_SQL += " " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + " = " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5;
                    }
                    else if (Convert.ToInt32(Registro["Bimestre"].ToString()) == 6)
                    {
                        Mi_SQL += " " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + " = " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6;
                    }
                    Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                    Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                    Mi_SQL += " WHERE " + Ope_Pre_Adeudos_Predial.Campo_No_Adeudo + " = '" + Registro["NO_ADEUDO"] + "'";
                    Mi_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_Anio + " = " + Registro["ANIO"];
                    Mi_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_ID + "'";
                    Cmmd.CommandText = Mi_SQL;
                    Cmmd.ExecuteNonQuery();

                    //Actualiza el estatus del adeudo
                    Mi_SQL = "UPDATE " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + " SET " + Ope_Pre_Adeudos_Predial.Campo_Estatus + " = 'PAGADO', ";
                    Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                    Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                    Mi_SQL += " WHERE ((" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + " - " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ")";
                    Mi_SQL += " + (" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + " - " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ")";
                    Mi_SQL += " + (" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + " - " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ")";
                    Mi_SQL += " + (" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + " - " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ")";
                    Mi_SQL += " + (" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + " - " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ")";
                    Mi_SQL += " + (" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + " - " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ")) = 0";
                    Mi_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_No_Adeudo + " = '" + Registro["NO_ADEUDO"] + "'";
                    Mi_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_Anio + " = " + Registro["ANIO"];
                    Mi_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_ID + "'";
                    Cmmd.CommandText = Mi_SQL;
                    Cmmd.ExecuteNonQuery();
                }
                ////Actualiza el pago con los montos
                //Mi_SQL = "UPDATE " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                //Mi_SQL += " SET " + Ope_Caj_Pagos.Campo_Monto_Corriente + " = " + Monto_Corriente;
                //Mi_SQL += ", " + Ope_Caj_Pagos.Campo_Monto_Rezago + " = " + Monto_Rezago;
                //Mi_SQL += ", " + Ope_Caj_Pagos.Campo_Honorarios + " = " + Monto_Honorarios;
                //Mi_SQL += ", " + Ope_Caj_Pagos.Campo_Monto_Recargos_Moratorios + " = " + Monto_Moratorios;
                //Mi_SQL += ", " + Ope_Caj_Pagos.Campo_Monto_Recargos + " = " + Monto_Recargos;
                //Mi_SQL += ", " + Ope_Caj_Pagos.Campo_Descuento_Honorarios + " = " + Monto_Descuento_Honorarios;
                //Mi_SQL += ", " + Ope_Caj_Pagos.Campo_Descuento_Recargos + " = " + Monto_Descuento_Recargos;
                //Mi_SQL += ", " + Ope_Caj_Pagos.Campo_Descuento_Moratorios + " = " + Monto_Descuento_Moratorios;
                //Mi_SQL += ", " + Ope_Caj_Pagos.Campo_Descuento_Pronto_Pago + " = " + Monto_Descuento_Pronto_Pago;
                //Mi_SQL += ", " + Ope_Caj_Pagos.Campo_Periodo_Corriente + " = '" + Periodo_Corriente + "'";
                //Mi_SQL += ", " + Ope_Caj_Pagos.Campo_Periodo_Rezago + " = '" + Periodo_Rezago + "'";
                //Mi_SQL += ", " + Ope_Caj_Pagos.Campo_Ajuste_Tarifario + " = " + Ajuste_Tarifario;
                //Mi_SQL += ", " + Ope_Caj_Pagos.Campo_Tipo_Pago + " = 'PREDIAL'";
                //Mi_SQL += " WHERE " + Ope_Caj_Pagos.Campo_No_Pago + " = '" + No_Pago + "'";
                //Cmmd.CommandText = Mi_SQL;
                //Cmmd.ExecuteNonQuery();
            }
            catch (OracleException Ex)
            {
                //Indicamos el mensaje 
                throw new Exception(Ex.ToString());
            }
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Cargar_Datos
        ///DESCRIPCIÓN: asignar datos de cuenta a los controles
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 03/Ago/2011 06:31:08 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************    
        private static Boolean Cargar_Datos(String Cuenta_Predial_ID)
        {
            Boolean Datos_Cargados = false;
            try
            {

                Busqueda_Propietarios(Cuenta_Predial_ID);
                Datos_Cargados = true;

            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Datos_Cargados;
        }
        private static void Busqueda_Propietarios(String Cuenta_Predial_ID)
        {
            DataSet Ds_Prop;
            Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
            try
            {
                M_Orden_Negocio.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
                Ds_Prop = M_Orden_Negocio.Consulta_Datos_Propietario();
                if (Ds_Prop.Tables[0].Rows.Count > 0)
                {
                }
            }
            catch (Exception Ex)
            {
            }

        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN : Cargar_Variacion
        ///DESCRIPCIÓN          : Consulta los datos de la Orden de Variacción
        ///PARAMETROS: 
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 21/Agosto/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************    
        private static Boolean Cargar_Variacion(ref OracleCommand Cmd, DataTable Dt_Caja_Variacion, DataSet Ds_Cuenta, string No_ContraRecibo)
        {
            Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
            DataTable Dt_Orden_Variacion;
            DataTable Dt_Detalle_Orden_Variacion;
            Boolean Variacion_Cargada = false;

            if (Dt_Caja_Variacion.Rows.Count > 0)
            {
                Orden_Variacion.P_Incluir_Campos_Foraneos = true;
                Orden_Variacion.P_Orden_Variacion_ID = Dt_Caja_Variacion.Rows[0]["No_Orden_Variacion"].ToString().Trim();
                Orden_Variacion.P_Generar_Orden_Anio = Dt_Caja_Variacion.Rows[0]["Anio"].ToString().Trim();

                Dt_Orden_Variacion = Orden_Variacion.Consultar_Ordenes_Variacion();
                Dt_Detalle_Orden_Variacion = Orden_Variacion.P_Generar_Orden_Dt_Detalles;

                if (Dt_Orden_Variacion.Rows.Count > 0)
                {
                    Orden_Variacion.P_Cmmd = Cmd;
                    String Tasa_ID = "";
                    try
                    {
                        Tasa_ID = Dt_Caja_Variacion.Rows[0]["Tasa_ID"].ToString().Trim();
                    }
                    catch
                    {
                    }
                    Aplicar_Variacion(ref Cmd, Dt_Orden_Variacion);
                    Orden_Variacion.P_Año = Convert.ToInt32(Dt_Orden_Variacion.Rows[0]["Anio"]);
                    Orden_Variacion.P_Contrarecibo_Anio = Convert.ToInt16(Dt_Orden_Variacion.Rows[0]["Anio"]);
                    Orden_Variacion.P_Contrarecibo_No_Contrarecibo = No_ContraRecibo;
                    Orden_Variacion.P_Contrarecibo_Estatus = "PAGADO";
                    Orden_Variacion.P_Contrarecibo_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Orden_Variacion.Modificar_Contrarecibo();
                    //poner debajo de MODIFICAR CONTRARECIBO EN CARGAR VARIACION
                    Orden_Variacion.Modificar_Calculo_Traslado();
                    Orden_Variacion.P_Cuenta_Predial_ID = Dt_Orden_Variacion.Rows[0]["Cuenta_Predial_ID"].ToString().Trim();
                    Orden_Variacion.P_Orden_Variacion_ID = Dt_Orden_Variacion.Rows[0]["No_Orden_Variacion"].ToString().Trim();
                    Orden_Variacion.P_Grupo_Movimiento_ID = Dt_Orden_Variacion.Rows[0]["Grupo_Movimiento_ID"].ToString();
                    Orden_Variacion.P_Tipo_Predio_ID = Dt_Orden_Variacion.Rows[0]["Tipo_Predio_ID"].ToString();
                    Int32 No_Nota_Consecutivo = Obtener_Dato_Consulta(ref Cmd, "NVL(MAX(" + Ope_Pre_Orden_Variacion.Campo_No_Nota + "), 0) + 1", Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion, Ope_Pre_Orden_Variacion.Campo_Anio + " = " + Orden_Variacion.P_Año + " AND " + Ope_Pre_Orden_Variacion.Campo_Grupo_Movimiento_ID + " = '" + Orden_Variacion.P_Grupo_Movimiento_ID + "' AND " + Ope_Pre_Orden_Variacion.Campo_Tipo_Predio_ID + " = '" + Orden_Variacion.P_Tipo_Predio_ID + "' ORDER BY " + Ope_Pre_Orden_Variacion.Campo_No_Nota, 1);
                    Int32 No_Nota_Inicila = Obtener_Dato_Consulta(ref Cmd, "NVL(" + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Folio_Inicial + ", 0)", Cat_Pre_Grupos_Movimiento_Detalles.Tabla_Cat_Pre_Grupos_Movimiento_Detalles, Cat_Pre_Grupos_Movimiento_Detalles.Campo_Año + " = " + Orden_Variacion.P_Año + " AND " + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Grupo_Movimiento_ID + " = '" + Orden_Variacion.P_Grupo_Movimiento_ID + "' AND " + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Tipo_Predio_ID + " = '" + Orden_Variacion.P_Tipo_Predio_ID + "'", 1);
                    Orden_Variacion.P_No_Nota = No_Nota_Consecutivo > No_Nota_Inicila ? No_Nota_Consecutivo : No_Nota_Inicila;
                    Orden_Variacion.P_Fecha_Nota = DateTime.Now;
                    Orden_Variacion.P_No_Nota_Impreso = "NO";
                    Orden_Variacion.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Orden_Variacion.Modificar_Orden_Variacion();
                }
            }

            return Variacion_Cargada;
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
        private static Int32 Obtener_Dato_Consulta(ref OracleCommand Cmmd, String Campo, String Tabla, String Condiciones, Int32 Dato_Default)
        {
            String Mi_SQL;
            Int32 Dato_Consulta = 0;

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

                //OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Cmmd.CommandText = Mi_SQL;
                object Dato = Cmmd.ExecuteOracleScalar();
                if (Dato != null)
                {
                    if (Dato.ToString() != "")
                    {
                        Dato_Consulta = Convert.ToInt32(Dato.ToString());
                    }
                }
                if (Convert.IsDBNull(Dato_Consulta))
                {
                    Dato_Consulta = 1;
                }
                else
                {
                    Dato_Consulta = Dato_Consulta + 1;
                }
            }
            catch (OracleException Ex)
            {
                //Indicamos el mensaje 
                throw new Exception(Ex.ToString());
            }

            if (Dato_Consulta == 0)
            {
                Dato_Consulta = Dato_Default;
            }

            return Dato_Consulta;
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN : Aplicar_Variacion
        ///DESCRIPCIÓN          : Guarda los datos de la Orden de Variación
        ///PARAMETROS: 
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 21/Agosto/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************    
        private static Boolean Aplicar_Variacion(ref OracleCommand Cmmd, DataTable Dt_Orden_Variacion)
        {
            Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
            Boolean Variacion_Aceptada = false;
            try
            {
                Ordenes_Variacion.P_Año = Convert.ToInt16(Dt_Orden_Variacion.Rows[0]["Anio"]);
                Ordenes_Variacion.P_Orden_Variacion_ID = Dt_Orden_Variacion.Rows[0]["No_Orden_Variacion"].ToString();
                Ordenes_Variacion.P_Cuenta_Predial_ID = Dt_Orden_Variacion.Rows[0]["CUENTA_PREDIAL_ID"].ToString();
                //Ordenes_Variacion.P_Contrarecibo_No_Contrarecibo = Grid_Ordenes_Variacion.DataKeys[Grid_Ordenes_Variacion.SelectedIndex].Values[0].ToString();
                Ordenes_Variacion.P_Cmmd = Cmmd;
                if (Ordenes_Variacion.Aplicar_Variacion_Orden())
                {
                    Variacion_Aceptada = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Aplicar Variacion: " + ex.ToString());
            }
            return Variacion_Aceptada;

            //Boolean Variacion_Aceptada = false;
            //Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            //String Cuenta_Predial_ID = "";

            //Cuenta_Predial_ID = Dt_Orden_Variacion.Rows[0]["CUENTA_PREDIAL_ID"].ToString();
            //Cuentas_Predial.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
            //if (Dt_Detalle_Orden_Variacion != null)
            //{
            //    foreach (DataRow Dr_Fila in Dt_Detalle_Orden_Variacion.Rows)
            //    {
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Calle_ID)
            //        {
            //            Cuentas_Predial.P_Calle_ID = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Colonia_ID)
            //        {
            //            Cuentas_Predial.P_Colonia_ID = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID)
            //        {
            //            Cuentas_Predial.P_Estado_Predio_ID = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID)
            //        {
            //            Cuentas_Predial.P_Tipo_Predio_ID = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID)
            //        {
            //            Cuentas_Predial.P_Uso_Suelo_ID = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID)
            //        {
            //            Cuentas_Predial.P_Cuota_Minima_ID = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen)
            //        {
            //            Cuentas_Predial.P_Cuenta_Origen = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        Cuentas_Predial.P_Estatus = "VIGENTE";
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_No_Exterior)
            //        {
            //            Cuentas_Predial.P_No_Exterior = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_No_Interior)
            //        {
            //            Cuentas_Predial.P_No_Interior = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida)
            //        {
            //            Cuentas_Predial.P_Superficie_Construida = Convert.ToDecimal(Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo]);
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Superficie_Total)
            //        {
            //            Cuentas_Predial.P_Superficie_Total = Convert.ToDecimal(Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo]);
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral)
            //        {
            //            Cuentas_Predial.P_Clave_Catastral = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal)
            //        {
            //            Cuentas_Predial.P_Valor_Fiscal = Convert.ToDecimal(Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo]);
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Efectos)
            //        {
            //            Cuentas_Predial.P_Efectos = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente)
            //        {
            //            Cuentas_Predial.P_Periodo_Corriente = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual)
            //        {
            //            Cuentas_Predial.P_Cuota_Anual = Convert.ToDecimal(Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo]);
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija)
            //        {
            //            Cuentas_Predial.P_Cuota_Fija = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion)
            //        {
            //            Cuentas_Predial.P_Diferencia_Construccion = Convert.ToDecimal(Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo]);
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion)
            //        {
            //            Cuentas_Predial.P_Termino_Exencion = Convert.ToDateTime(Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo]);
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo)
            //        {
            //            Cuentas_Predial.P_Fecha_Avaluo = Convert.ToDateTime(Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo]);
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Costo_m2)
            //        {
            //            Cuentas_Predial.P_Costo_M2 = Convert.ToDecimal(Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo]);
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion)
            //        {
            //            Cuentas_Predial.P_Porcentaje_Exencion = Convert.ToDecimal(Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo]);
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija)
            //        {
            //            Cuentas_Predial.P_No_Cuota_Fija = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Tasa_ID)
            //        {
            //            Cuentas_Predial.P_Tasa_ID = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion)
            //        {
            //            Cuentas_Predial.P_Calle_ID_Notificacion = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion)
            //        {
            //            Cuentas_Predial.P_Colonia_ID_Notificacion = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion)
            //        {
            //            Cuentas_Predial.P_Estado_ID_Notificacion = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion)
            //        {
            //            Cuentas_Predial.P_Ciudad_ID_Notificacion = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo)
            //        {
            //            Cuentas_Predial.P_Domicilio_Foraneo = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion)
            //        {
            //            Cuentas_Predial.P_Calle_Notificacion = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion)
            //        {
            //            Cuentas_Predial.P_No_Exterior_Notificacion = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal)
            //        {
            //            Cuentas_Predial.P_Codigo_Postal = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion)
            //        {
            //            Cuentas_Predial.P_No_Interior_Notificacion = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion)
            //        {
            //            Cuentas_Predial.P_Colonia_Notificacion = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_No_Diferencia)
            //        {
            //            Cuentas_Predial.P_No_Diferencia = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID)
            //        {
            //            Cuentas_Predial.P_Tasa_Predial_ID = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion)
            //        {
            //            Cuentas_Predial.P_Estado_Notificacion = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion)
            //        {
            //            Cuentas_Predial.P_Ciudad_Notificacion = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //        if (Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Campo].ToString() == Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion)
            //        {
            //            Cuentas_Predial.P_Tipo_Suspencion = Dr_Fila[Ope_Pre_Orden_Detalles.Campo_Dato_Nuevo].ToString();
            //        }
            //    }
            //}

            //Cuentas_Predial.P_Cmmd = Cmmd;
            //Cuentas_Predial.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            //if (Ds_Cuenta != null)
            //{
            //    if (Ds_Cuenta.Tables[0].Rows.Count > 0)
            //    {
            //        Variacion_Aceptada = Cuentas_Predial.Modifcar_Cuenta();
            //    }
            //    else
            //    {
            //        Variacion_Aceptada = Cuentas_Predial.Alta_Cuenta();
            //    }
            //}
            //else
            //{
            //    Variacion_Aceptada = Cuentas_Predial.Alta_Cuenta();
            //}
            //Cuentas_Predial = null;

            //if (Variacion_Aceptada)
            //{
            //    Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
            //    DataTable Dt_Copropietarios = new DataTable();
            //    DataTable Dt_Diferencias = new DataTable();
            //    String Orden_Variacion_ID = Dt_Orden_Variacion.Rows[0]["No_Orden_Variacion"].ToString().Trim();
            //    int Anio = Convert.ToInt32(Dt_Orden_Variacion.Rows[0]["Anio"]);
            //    Orden_Variacion.P_Copropietario_Cuenta_Predial_ID = Ds_Cuenta.Tables[0].Rows[0]["Cuenta_Predial_ID"].ToString().Trim();
            //    Orden_Variacion.P_Cuenta_Predial_ID = Ds_Cuenta.Tables[0].Rows[0]["Cuenta_Predial_ID"].ToString().Trim();
            //    Orden_Variacion.P_Copropietario_Tipo = "COPROPIETARIO";
            //    DataTable Dt_Copropietarios_Recorridos = Consultar_Variacion_Copropietarios(Cuenta_Predial_ID, Orden_Variacion_ID, Anio);// Orden_Variacion.Consulta_Co_Propietarios();
            //    Dt_Copropietarios.Columns.Add(new DataColumn("CONTRIBUYENTE_ID", typeof(String)));
            //    DataRow Dr;

            //    for (int x = 0; x < Dt_Copropietarios_Recorridos.Rows.Count; x++)
            //    {
            //        Dr = Dt_Copropietarios.NewRow();
            //        Dr["CONTRIBUYENTE_ID"] = Dt_Copropietarios_Recorridos.Rows[x]["CONTRIBUYENTE_ID"].ToString().Trim();
            //        Dt_Copropietarios.Rows.Add(Dr);
            //    }
            //    Orden_Variacion.P_Dt_Copropietarios = Dt_Copropietarios;

            //    Dt_Diferencias.Columns.Add(new DataColumn("CUOTA_ANUAL", typeof(Decimal)));
            //    Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_1", typeof(Decimal)));
            //    Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_2", typeof(Decimal)));
            //    Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_3", typeof(Decimal)));
            //    Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_4", typeof(Decimal)));
            //    Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_5", typeof(Decimal)));
            //    Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_6", typeof(Decimal)));
            //    Dt_Diferencias.Columns.Add(new DataColumn("AÑO", typeof(int)));

            //    String Periodo = "";
            //    int Desde_Bimestre = 0;
            //    int Hasta_Bimestre = 0;
            //    int Cont_Bimestres = 0;
            //    Boolean Periodo_Corriente_Validado = false;
            //    Boolean Periodo_Rezago_Validado = false;
            //    Decimal Importe_Rezago = 0;
            //    Decimal Cuota_Minima_Año = 0;
            //    Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuotas_Minimas = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
            //    Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
            //    DataTable Dt_Adeudos = new DataTable();

            //    Orden_Variacion.P_Cuenta_Predial_ID = Ds_Cuenta.Tables[0].Rows[0]["Cuenta_Predial_ID"].ToString().Trim();
            //    Orden_Variacion.P_Generar_Orden_No_Orden = Dt_Orden_Variacion.Rows[0]["No_Orden_Variacion"].ToString().Trim();
            //    Orden_Variacion.P_Generar_Orden_Anio = Dt_Orden_Variacion.Rows[0]["Anio"].ToString().Trim();
            //    DataTable Dt_Agregar_Diferencias = Orden_Variacion.Consulta_Diferencias();

            //    for (int x = 0; x < Dt_Agregar_Diferencias.Rows.Count; x++)
            //    {
            //        Periodo = Obtener_Periodos_Bimestre(Dt_Agregar_Diferencias.Rows[x]["Periodo"].ToString().Trim(), out Periodo_Corriente_Validado, out Periodo_Rezago_Validado);
            //        if (Periodo_Rezago_Validado)
            //        {
            //            if (Periodo.Trim() != "")
            //            {
            //                Desde_Bimestre = Convert.ToInt32(Periodo.Split('-').GetValue(0));
            //                Hasta_Bimestre = Convert.ToInt32(Periodo.Split('-').GetValue(1));
            //            }
            //        }
            //        if (Periodo_Corriente_Validado)
            //        {
            //            if (Periodo.Trim() != "")
            //            {
            //                Desde_Bimestre = Convert.ToInt32(Periodo.Split('-').GetValue(0));
            //                Hasta_Bimestre = Convert.ToInt32(Periodo.Split('-').GetValue(1));
            //            }
            //        }

            //        Dr = Dt_Diferencias.NewRow();
            //        Dr["CUOTA_ANUAL"] = Convert.ToDecimal(Dt_Agregar_Diferencias.Rows[x]["Cuota_Bimestral"].ToString()) * 6;
            //        Dr["AÑO"] = Convert.ToInt32(Dt_Agregar_Diferencias.Rows[x]["Periodo"].ToString().Substring(Dt_Agregar_Diferencias.Rows[x]["Periodo"].ToString().Trim().Length - 4, 4));
            //        Cuota_Minima_Año = Convert.ToDecimal(Cuotas_Minimas.Consultar_Cuota_Minima_Anio(Dr["AÑO"].ToString()));
            //        Importe_Rezago = Convert.ToDecimal(Dt_Agregar_Diferencias.Rows[x]["Importe"].ToString().Replace("$", ""));
            //        //Busca los Adeudos actuales de la Cuenta
            //        int Cont_Adeudos = 0;
            //        Boolean Cuotas_Minimas_Encontradas = false;
            //        Decimal Sum_Adeudos = 0;
            //        Dt_Adeudos = Resumen_Predio.Consultar_Adeudos_Cuenta_Predial_Con_Totales(Cuenta_Predial_ID, null, Convert.ToInt16(Dr["AÑO"]), Convert.ToInt16(Dr["AÑO"]));
            //        if (Dt_Adeudos != null)
            //        {
            //            if (Dt_Adeudos.Rows.Count > 0)
            //            {
            //                for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
            //                {
            //                    if (Dt_Adeudos.Rows[0][Cont_Bimestres] != System.DBNull.Value)
            //                    {
            //                        if (Convert.ToDecimal(Dt_Adeudos.Rows[0][Cont_Bimestres]) == Cuota_Minima_Año)
            //                        {
            //                            Cuotas_Minimas_Encontradas = true;
            //                            Cont_Adeudos += 1;
            //                            Sum_Adeudos = Convert.ToDecimal(Dt_Adeudos.Rows[0][Cont_Bimestres]);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        //4° CASO
            //        //Si la suma de los Adeudos menos el importe es igual a la Cuota Mínima
            //        if ((Sum_Adeudos - Importe_Rezago) == Cuota_Minima_Año)
            //        {
            //            for (Cont_Bimestres = 1; Cont_Bimestres <= 6; Cont_Bimestres++)
            //            {
            //                if (Cont_Bimestres == 1)
            //                {
            //                    Dr["BIMESTRE_" + Cont_Bimestres.ToString()] = Cuota_Minima_Año;
            //                }
            //                else
            //                {
            //                    Dr["BIMESTRE_" + Cont_Bimestres.ToString()] = 0;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            //Determina si el Importe en las Diferencias es igual a la Cuota Mínima
            //            if (Importe_Rezago == Cuota_Minima_Año)
            //            {
            //                //Determina si los periodos a afectar son de todo el Año
            //                if (Desde_Bimestre == 1 && Hasta_Bimestre == 6)
            //                {
            //                    //Afecta todos los adeudos dividiendo el Importe en el Año
            //                    if (Dt_Agregar_Diferencias.Rows[x]["TIPO"].ToString().Trim() == "ALTA")
            //                    {
            //                        for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
            //                        {
            //                            Dr["BIMESTRE_" + Cont_Bimestres.ToString()] = Importe_Rezago / (Hasta_Bimestre - Desde_Bimestre + 1);
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (Dt_Agregar_Diferencias.Rows[x]["TIPO"].ToString().Trim() == "BAJA")
            //                        {
            //                            for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
            //                            {
            //                                Dr["BIMESTRE_" + Cont_Bimestres.ToString()] = Importe_Rezago / (Hasta_Bimestre - Desde_Bimestre + 1) * (-1);
            //                            }
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    //Afecta el Adeudo del primer periodo indicado
            //                    if (Dt_Agregar_Diferencias.Rows[x]["TIPO"].ToString().Trim() == "ALTA")
            //                    {
            //                        Dr["BIMESTRE_" + Desde_Bimestre.ToString()] = Importe_Rezago;
            //                    }
            //                    else
            //                    {
            //                        if (Dt_Agregar_Diferencias.Rows[x]["TIPO"].ToString().Trim() == "BAJA")
            //                        {
            //                            Dr["BIMESTRE_" + Desde_Bimestre.ToString()] = Importe_Rezago * (-1);
            //                        }
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                //Afecta los Adeudos de los Bimestres indicados
            //                if (Dt_Agregar_Diferencias.Rows[x]["TIPO"].ToString().Trim() == "ALTA")
            //                {
            //                    for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
            //                    {
            //                        if (Cuotas_Minimas_Encontradas)
            //                        {
            //                            if (Cont_Adeudos == (Hasta_Bimestre - Desde_Bimestre) + 1)
            //                            {
            //                                //3° CASO
            //                                //Si se encontraron Cuotas Mínimas en todo el periodo divide el Importe sobre el total de periodos más la Cuota Mínima
            //                                Dr["BIMESTRE_" + Cont_Bimestres.ToString()] = Importe_Rezago / (Hasta_Bimestre - Desde_Bimestre + 1) + Cuota_Minima_Año;
            //                            }
            //                            else
            //                            {
            //                                if ((Hasta_Bimestre - Desde_Bimestre) + 1 == 6)
            //                                {
            //                                    //1° CASO
            //                                    //Suma el Importe del Bimestre más la Cuota Mínima y el resultado lo divide por el total de periodos.
            //                                    Dr["BIMESTRE_" + Cont_Bimestres.ToString()] = (Importe_Rezago + Cuota_Minima_Año) / (Hasta_Bimestre - Desde_Bimestre + 1);
            //                                }
            //                                else
            //                                {
            //                                    //2° CASO
            //                                    //Se divide el Importe sobre 6
            //                                    Dr["BIMESTRE_" + Cont_Bimestres.ToString()] = Importe_Rezago / 6;
            //                                }
            //                            }

            //                        }
            //                        else
            //                        {
            //                            //Reparte el Importe en los Bimestres indicadoss
            //                            Dr["BIMESTRE_" + Cont_Bimestres.ToString()] = Importe_Rezago / (Hasta_Bimestre - Desde_Bimestre + 1);
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    if (Dt_Agregar_Diferencias.Rows[x]["TIPO"].ToString().Trim() == "BAJA")
            //                    {
            //                        for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
            //                        {
            //                            if (Cuotas_Minimas_Encontradas)
            //                            {
            //                                if (Cont_Adeudos == (Hasta_Bimestre - Desde_Bimestre) + 1)
            //                                {
            //                                    //3° CASO
            //                                    //Si se encontraron Cuotas Mínimas en todo el periodo divide el Importe sobre el total de periodos más la Cuota Mínima
            //                                    Dr["BIMESTRE_" + Cont_Bimestres.ToString()] = (Importe_Rezago / (Hasta_Bimestre - Desde_Bimestre + 1) + Cuota_Minima_Año) * (-1);
            //                                }
            //                                else
            //                                {
            //                                    if ((Hasta_Bimestre - Desde_Bimestre) + 1 == 6)
            //                                    {
            //                                        //1° CASO
            //                                        //Suma el Importe del Bimestre más la Cuota Mínima y el resultado lo divide por el total de periodos.
            //                                        Dr["BIMESTRE_" + Cont_Bimestres.ToString()] = (Importe_Rezago + Cuota_Minima_Año) / (Hasta_Bimestre - Desde_Bimestre + 1) * (-1);
            //                                    }
            //                                    else
            //                                    {
            //                                        //2° CASO
            //                                        // Se divide el Importe sobre 6
            //                                        Dr["BIMESTRE_" + Cont_Bimestres.ToString()] = Importe_Rezago / 6 * (-1);
            //                                    }
            //                                }
            //                            }
            //                            else
            //                            {
            //                                //Reparte el Importe en los Bimestres indicadoss
            //                                Dr["BIMESTRE_" + Cont_Bimestres.ToString()] = Importe_Rezago / (Hasta_Bimestre - Desde_Bimestre + 1) * (-1);
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        Dt_Diferencias.Rows.Add(Dr);
            //    }
            //    Orden_Variacion.P_Dt_Diferencias = Dt_Diferencias;

            //    Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
            //    DataSet Ds_Prop;
            //    M_Orden_Negocio.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
            //    M_Orden_Negocio.P_Orden_Variacion_ID = Dt_Orden_Variacion.Rows[0]["No_Orden_Variacion"].ToString().Trim();
            //    M_Orden_Negocio.P_Año = Convert.ToInt32(Dt_Orden_Variacion.Rows[0]["Anio"]);
            //    Ds_Prop = M_Orden_Negocio.Consultar_Propietarios_Variacion();
            //    if (Ds_Prop.Tables.Count > 0)
            //    {
            //        if (Ds_Prop.Tables[0].Rows.Count > 0)
            //        {
            //            String Propietario_ID = Ds_Prop.Tables[0].Rows[0]["CONTRIBUYENTE_ID"].ToString().Trim();

            //            Orden_Variacion.P_Propietario_Cuenta_Predial_ID = Ds_Cuenta.Tables[0].Rows[0]["Cuenta_Predial_ID"].ToString().Trim();
            //            Orden_Variacion.P_Propietario_Propietario_ID = Propietario_ID;
            //            Orden_Variacion.P_Propietario_ID = Propietario_ID;
            //            Orden_Variacion.P_Propietario_Tipo = "PROPIETARIO";
            //        }
            //    }

            //    //    Orden_Variacion.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
            //    Orden_Variacion.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            //    Orden_Variacion.P_Cmmd = Cmmd;
            //    Orden_Variacion.Aplicar_Variacion_Propietarios();
            //    Orden_Variacion.Aplicar_Variacion_Copropietarios();
            //    Orden_Variacion.Aplicar_Variacion_Diferencias();
            //}

            //return Variacion_Aceptada;
        }

        private static DataTable Consultar_Variacion_Copropietarios(String Cuenta_Predial_ID, String Orden_Variacion_ID, Int32 Anio)
        {
            //DataSet Ds_Copropietarios_Cuenta;
            DataTable Dt_Copropietarios_Cuenta;
            DataSet Ds_Copropietarios_Variacion;
            DataTable Dt_Copropietarios_Variacion;
            DataTable Dt_Temp_Copropietarios = new DataTable();
            DataRow Dr_Temp_Copropietario;
            Boolean Copropietario_Nuevo;
            try
            {
                Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
                M_Orden_Negocio.P_Orden_Variacion_ID = Orden_Variacion_ID;
                M_Orden_Negocio.P_Año = Anio;
                M_Orden_Negocio.P_Cuenta_Predial_ID = Cuenta_Predial_ID;// Session["Cuenta_Predial_ID"].ToString().Trim();
                Dt_Copropietarios_Cuenta = M_Orden_Negocio.Consulta_Co_Propietarios();
                Ds_Copropietarios_Variacion = M_Orden_Negocio.Consultar_Copropietarios_Variacion();
                if (Ds_Copropietarios_Variacion.Tables.Count > 0)
                {
                    Dt_Copropietarios_Variacion = Ds_Copropietarios_Variacion.Tables[0];
                    //if (Ds_Copropietarios_Cuenta.Tables.Count > 0)
                    {
                        //Dt_Copropietarios_Cuenta = Ds_Copropietarios_Cuenta.Tables[0];
                        if (Dt_Copropietarios_Variacion.Rows.Count > 0)
                        {
                            Dt_Temp_Copropietarios.Columns.Add(new DataColumn("CONTRIBUYENTE_ID", typeof(String)));
                            Dt_Temp_Copropietarios.Columns.Add(new DataColumn("RFC", typeof(String)));
                            Dt_Temp_Copropietarios.Columns.Add(new DataColumn("NOMBRE_CONTRIBUYENTE", typeof(String)));
                            Dt_Temp_Copropietarios.Columns.Add(new DataColumn("ESTATUS_VARIACION", typeof(String)));
                            if (Dt_Copropietarios_Cuenta.Rows.Count > 0)
                            {
                                foreach (DataRow Copropietario_Cuenta in Dt_Copropietarios_Cuenta.Rows)
                                {
                                    Copropietario_Nuevo = false;
                                    foreach (DataRow Copropietario_Variacion in Dt_Copropietarios_Variacion.Rows)
                                    {
                                        if (Copropietario_Cuenta[Cat_Pre_Propietarios.Campo_Contribuyente_ID].ToString() == Copropietario_Variacion[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID].ToString())
                                        {
                                            Dr_Temp_Copropietario = Dt_Temp_Copropietarios.NewRow();
                                            Dr_Temp_Copropietario["CONTRIBUYENTE_ID"] = Copropietario_Variacion[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID].ToString();
                                            Dr_Temp_Copropietario["RFC"] = Copropietario_Variacion["RFC"].ToString();
                                            Dr_Temp_Copropietario["NOMBRE_CONTRIBUYENTE"] = Copropietario_Variacion["NOMBRE_CONTRIBUYENTE"].ToString();
                                            Dr_Temp_Copropietario["ESTATUS_VARIACION"] = "ACTUAL";
                                            Dt_Temp_Copropietarios.Rows.Add(Dr_Temp_Copropietario);
                                        }
                                    }
                                }
                                foreach (DataRow Copropietario_Variacion in Dt_Copropietarios_Variacion.Rows)
                                {
                                    Copropietario_Nuevo = true;
                                    foreach (DataRow Copropietario_Cuenta in Dt_Copropietarios_Cuenta.Rows)
                                    {
                                        if (Copropietario_Variacion[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID].ToString() == Copropietario_Cuenta[Cat_Pre_Propietarios.Campo_Contribuyente_ID].ToString())
                                        {
                                            Copropietario_Nuevo = false;
                                        }
                                    }
                                    if (Copropietario_Nuevo)
                                    {
                                        Dr_Temp_Copropietario = Dt_Temp_Copropietarios.NewRow();
                                        Dr_Temp_Copropietario["CONTRIBUYENTE_ID"] = Copropietario_Variacion[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID].ToString();
                                        Dr_Temp_Copropietario["RFC"] = Copropietario_Variacion["RFC"].ToString();
                                        Dr_Temp_Copropietario["NOMBRE_CONTRIBUYENTE"] = Copropietario_Variacion["NOMBRE_CONTRIBUYENTE"].ToString();
                                        Dr_Temp_Copropietario["ESTATUS_VARIACION"] = "NUEVO";
                                        Dt_Temp_Copropietarios.Rows.Add(Dr_Temp_Copropietario);
                                    }
                                }
                            }
                            else
                            {
                                foreach (DataRow Copropietario_Variacion in Dt_Copropietarios_Variacion.Rows)
                                {
                                    Dr_Temp_Copropietario = Dt_Temp_Copropietarios.NewRow();
                                    Dr_Temp_Copropietario["CONTRIBUYENTE_ID"] = Copropietario_Variacion[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID].ToString();
                                    Dr_Temp_Copropietario["RFC"] = Copropietario_Variacion["RFC"].ToString();
                                    Dr_Temp_Copropietario["NOMBRE_CONTRIBUYENTE"] = Copropietario_Variacion["NOMBRE_CONTRIBUYENTE"].ToString();
                                    Dr_Temp_Copropietario["ESTATUS_VARIACION"] = "NUEVO";
                                    Dt_Temp_Copropietarios.Rows.Add(Dr_Temp_Copropietario);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Temp_Copropietarios;
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cancelaciones
        ///DESCRIPCIÓN: Obtiene todos las Cancelaciones que estan dadas de 
        ///             alta en la Base de Datos
        ///PARAMENTROS:
        ///                 Cancelacion     Contiene los campos necesarios para hacer un filtrado de 
        ///                                 información, si es que se
        ///                                 introdujeron datos de busqueda.
        ///CREO: SERGIO MANUEL GALLARDO ANDRADE
        ///FECHA_CREO: 27/Octubre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO 
        ///*******************************************************************************
        public static DataTable Consultar_Cancelaciones(Cls_Ope_Pre_ReActivar_Pago_Negocio Cancelacion)
        {
            DataTable tabla = new DataTable();

            try
            {
                String Mi_SQL = "SELECT P." + Ope_Caj_Pagos.Campo_No_Pago + ", P." + Ope_Caj_Pagos.Campo_No_Recibo + ", P." + Ope_Caj_Pagos.Campo_Fecha;
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS CAJA, P." + Ope_Caj_Pagos.Campo_Monto_Corriente + " AS MONTO";
                Mi_SQL = Mi_SQL + ", P." + Ope_Caj_Pagos.Campo_No_Operacion + ",Ca." + Cat_Pre_Motivos.Campo_Nombre + " as MOTIVO, P." + Ope_Caj_Pagos.Campo_Comentarios;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " P, " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " C, " + Cat_Pre_Motivos.Tabla_Cat_Pre_Motivos + " Ca  WHERE ";
                Mi_SQL = Mi_SQL + " P." + Ope_Caj_Pagos.Campo_Motivo_Cancelacion_ID + " = Ca." + Cat_Pre_Motivos.Campo_Motivo_ID + " AND ";
                Mi_SQL = Mi_SQL + " P." + Ope_Caj_Pagos.Campo_Caja_ID + "= C." + Cat_Pre_Cajas.Campo_Caja_Id + " AND P." + Ope_Caj_Pagos.Campo_Estatus + " = 'CANCELADO'";
                if (Cancelacion.P_Filtro.Length != 0)
                {
                    Mi_SQL = Mi_SQL + " AND p." + Ope_Caj_Pagos.Campo_No_Recibo + " = '" + Cancelacion.P_Filtro + "'";
                }
                Mi_SQL = Mi_SQL + " AND P." + Ope_Caj_Pagos.Campo_No_Turno + " = '" + Cancelacion.P_No_Turno + "'";
                Mi_SQL = Mi_SQL + " ORDER BY p." + Ope_Caj_Pagos.Campo_No_Operacion + " DESC, " + Ope_Caj_Pagos.Campo_No_Recibo;
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
        public static Cls_Ope_Pre_ReActivar_Pago_Negocio Consultar_Datos_Cancelaciones(Cls_Ope_Pre_ReActivar_Pago_Negocio P_Cancelacion)
        {
            Cls_Ope_Pre_ReActivar_Pago_Negocio R_Cancelacion = new Cls_Ope_Pre_ReActivar_Pago_Negocio();

            string Mi_SQL = "SELECT PAG." + Ope_Caj_Pagos.Campo_No_Pago + ", PAG." + Ope_Caj_Pagos.Campo_No_Recibo + ", PAG." + Ope_Caj_Pagos.Campo_Fecha + ", PAG." + Ope_Caj_Pagos.Campo_No_Operacion;
            Mi_SQL = Mi_SQL + ", MOD." + Cat_Pre_Modulos.Campo_Descripcion + " AS MODULO, PAG." + Ope_Caj_Pagos.Campo_Motivo_Cancelacion_ID + ", CAJ." + Cat_Pre_Cajas.Campo_Comentario + " AS CAJA_ID ";
            Mi_SQL = Mi_SQL + ", PAG." + Ope_Caj_Pagos.Campo_Comentarios + ", CA." + Cat_Pre_Motivos.Campo_Nombre + " AS MOTIVO ";
            Mi_SQL = Mi_SQL + ", PAG." + Ope_Caj_Pagos.Campo_Usuario_Creo + " AS EMPLEADO_ID, PAG." + Ope_Caj_Pagos.Campo_Monto_Corriente + " FROM " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " CAJ, ";
            Mi_SQL = Mi_SQL + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " PAG, " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + " MOD, " + Cat_Pre_Motivos.Tabla_Cat_Pre_Motivos + " CA WHERE PAG." + Ope_Caj_Pagos.Campo_Caja_ID + "= CAJ." + Cat_Pre_Cajas.Campo_Caja_Id;
            Mi_SQL = Mi_SQL + " AND CA." + Cat_Pre_Motivos.Campo_Motivo_ID + "= PAG." + Ope_Caj_Pagos.Campo_Motivo_Cancelacion_ID;
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
                    R_Cancelacion.P_Motivo_Cancelacion_Id = Data_Reader["MOTIVO"].ToString();
                    R_Cancelacion.P_Caja = Data_Reader["CAJA_ID"].ToString();
                    R_Cancelacion.P_Comentarios = Data_Reader["COMENTARIOS"].ToString();
                    R_Cancelacion.P_Cajero = Data_Reader["EMPLEADO_ID"].ToString();
                    R_Cancelacion.P_Monto = Data_Reader[Ope_Caj_Pagos.Campo_Monto_Corriente].ToString();
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
            DataSet Ds_Adeudos = new DataSet();

            try
            {
                //Realiza la consulta de los adeudos pagados
                Mi_SQL = "SELECT " + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Pago + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Adeudo + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_Anio;
                Mi_SQL += ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_Bimestre + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_Monto;
                Mi_SQL += " FROM " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial;
                Mi_SQL += " WHERE " + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Pago + " = '" + No_Pago + "'";
                Mi_SQL += " ORDER BY " + Ope_Caj_Pagos_Adeudos_Predial.Campo_Anio + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_Bimestre;
                Cmmd.CommandText = Mi_SQL;
                OracleDataAdapter Da_Adeudos = new OracleDataAdapter(Cmmd);
                Da_Adeudos.Fill(Ds_Adeudos);
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
                }

                //Elimina los registros de los recargos
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
        public static DataTable Consulta_Caja_Empleado(Cls_Ope_Pre_ReActivar_Pago_Negocio Datos)
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

    }
}