
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
using Presidencia.Operacion_Descuentos_Traslado.Negocio;
using Presidencia.Catalogo_Claves_Ingreso.Negocio;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;


namespace Presidencia.Operacion_Descuentos_Traslado.Datos
{
    #region Metodos
    public class Cls_Ope_Pre_Descuentos_Traslado_Datos
    {
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Convenios_Descuentos
        ///DESCRIPCIÓN:Consulta las cuentas con convenios y en estatus PENDIENTE o CANCELADO a las cuales se les podra realizar un descuento_predial
        ///PARAMETROS: Parametros. Trae los datos para realizar la Operación.
        ///CREO: Jacqueline Ramirez Sierra
        ///FECHA_CREO: 21 Octubre 2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************      
        public static DataTable Consultar_Convenios_Descuentos_Traslado(Cls_Ope_Pre_Descuentos_Traslado_Negocio Descuento)
        {
            DataSet Ds_Datos = null;
            DataTable Dt_Datos = new DataTable();
            String Mi_SQL = null;
            try
            {
                Mi_SQL = "SELECT "
                    + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + ".*"
                    + " FROM " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio 
                    + " inner join " + Ope_Pre_Descuento_Traslado.Tabla_Ope_Pre_Descuento_Traslado + " on "
                    + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Descuento + "="
                    + Ope_Pre_Descuento_Traslado.Tabla_Ope_Pre_Descuento_Traslado + "." + Ope_Pre_Descuento_Traslado.Campo_No_Descuento
                    + " WHERE ";

                // si se especifica numero de descuento, filtrar
                if (!string.IsNullOrEmpty(Descuento.P_No_Descuento))
                {
                    Mi_SQL += Ope_Pre_Descuento_Traslado.Tabla_Ope_Pre_Descuento_Traslado + "." + Ope_Pre_Descuento_Traslado.Campo_No_Descuento + " = '" + Descuento.P_No_Descuento + "' AND ";
                }
                // si se especifica estatus, filtrar por estatus de convenios
                if (!string.IsNullOrEmpty(Descuento.P_Estatus))
                {
                    Mi_SQL += Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." + Ope_Pre_Convenios_Traslados_Dominio.Campo_Estatus + " = '" + Descuento.P_Estatus + "' AND ";
                }

                // eliminar AND o WHERE al final de la consulta
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Descuentos_Traslado
        ///DESCRIPCIÓN: Obtiene todas los Descuentos de Traslado almacenados en la base de datos.
        ///PARAMETROS:   
        ///CREO: Jacqueline Ramirez Sierra
        ///FECHA_CREO: 15/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Descuentos_Traslado(Cls_Ope_Pre_Descuentos_Traslado_Negocio Descuento)
        {

            DataTable Tabla = new DataTable();
            String Mi_SQL;
            try
            {
                if (!string.IsNullOrEmpty(Descuento.P_No_Descuento))
                {

                    Mi_SQL = "SELECT ";
                    Mi_SQL += " CP." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + " AS NO_EXTERIOR";
                    Mi_SQL += ",CP." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + " AS NO_INTERIOR";
                    Mi_SQL += ",CA." + Cat_Pre_Calles.Campo_Nombre + " AS CALLE";
                    Mi_SQL += ",CO." + Cat_Ate_Colonias.Campo_Nombre + " AS COLONIA";
                    Mi_SQL += ",CON." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + "||' '|| CON." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + "||' '|| CON." + Cat_Pre_Contribuyentes.Campo_Nombre + " AS PROPIETARIO";
                    Mi_SQL += ", C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Costo_Constancia + " AS COSTO_CONSTANCIA";
                    Mi_SQL += ", C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Traslado + " AS MONTO_TRASLADO";
                    Mi_SQL += ", C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Division + " AS MONTO_DIVISION";
                    Mi_SQL += ", O." + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + " AS NO_CONTRARECIBO";
                }
                else
                {
                    Mi_SQL = "SELECT O." + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + " AS NO_CONTRARECIBO";
                }

                Mi_SQL += ", CP." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " AS CUENTA_PREDIAL"
                    + ", CP." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " AS CUENTA_PREDIAL_ID"
                    + ", NVL(D." + Ope_Pre_Descuento_Traslado.Campo_Fecha_Inicial + ",SYSDATE) AS FECHA_INICIAL"
                    + ", C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Recargos
                    + ", C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Costo_Constancia
                    + ", C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Traslado
                    + ", C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Division
                    + ", NVL(D." + Ope_Pre_Descuento_Traslado.Campo_Desc_Recargo + ",'0') AS DESC_RECARGO"
                    + ", C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Multa
                    + ", NVL(D." + Ope_Pre_Descuento_Traslado.Campo_Desc_Multa + ",'0') AS DESC_MULTA "
                    + ", D." + Ope_Pre_Descuento_Traslado.Campo_Estatus
                    + ", NVL(D." + Ope_Pre_Descuento_Traslado.Campo_No_Descuento + ",' ') AS NO_DESCUENTO"
                    + ", C." + Ope_Pre_Descuento_Traslado.Campo_No_Calculo
                    + ", C." + Ope_Pre_Descuento_Traslado.Campo_Anio_Calculo
                    + ",'TD'" + "||''|| TO_NUMBER (C." + Ope_Pre_Descuento_Traslado.Campo_No_Calculo + ")||''|| C." + Ope_Pre_Descuento_Traslado.Campo_Anio_Calculo + " AS FOLIO"
                    + ", C." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + " AS NO_ORDEN_VARIACION"
                    + ", C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + " AS " + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden
                    + ", C." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Adeudo
                    + ", NVL(D." + Ope_Pre_Descuento_Traslado.Campo_Realizo + ",' ') AS " + Ope_Pre_Descuento_Traslado.Campo_Realizo
                    + ", D." + Ope_Pre_Descuento_Traslado.Campo_Fecha_Vencimiento + " AS " + Ope_Pre_Descuento_Traslado.Campo_Fecha_Vencimiento
                    + ", NVL(D." + Ope_Pre_Descuento_Traslado.Campo_Observaciones + ",' ') AS " + Ope_Pre_Descuento_Traslado.Campo_Observaciones
                    + ", NVL(D." + Ope_Pre_Descuento_Traslado.Campo_Fundamento_Legal + ",' ') AS " + Ope_Pre_Descuento_Traslado.Campo_Fundamento_Legal
                    + ", NVL2(D." + Ope_Pre_Descuento_Traslado.Campo_Estatus
                    + ", " + Ope_Pre_Descuento_Traslado.Campo_Total_Por_Pagar
                    + " ,C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Total_Pagar + ") AS TOTAL";

                Mi_SQL += " FROM  " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " O LEFT JOIN " 
                    + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + " C ON " 
                    + "(C." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + " = O." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion
                    + " AND C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + " = O." + Ope_Pre_Ordenes_Variacion.Campo_Anio + ")"
                    + " LEFT JOIN " + Ope_Pre_Descuento_Traslado.Tabla_Ope_Pre_Descuento_Traslado + " D ON " 
                    + "(D." + Ope_Pre_Descuento_Traslado.Campo_No_Calculo + " = C." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo
                    + " AND D." + Ope_Pre_Descuento_Traslado.Campo_Anio_Calculo + " = C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + ")";
                if (Descuento.P_No_Descuento == null)
                {
                    Mi_SQL += " LEFT JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas
                        + " CP ON CP." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id;
                }
                else
                {
                    Mi_SQL += " LEFT JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CP ON C." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = CP." + Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id;
                    Mi_SQL += " LEFT JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " P ON P." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id;
                    Mi_SQL += " LEFT JOIN " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " CON ON CON." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = P." + Cat_Pre_Propietarios.Campo_Contribuyente_ID;
                    Mi_SQL += " LEFT JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CA ON CA." + Cat_Pre_Calles.Campo_Calle_ID + " = CP." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion;
                    Mi_SQL += " LEFT JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " CO ON CO." + Cat_Ate_Colonias.Campo_Colonia_ID + " = CP." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion;
                }

                if (!string.IsNullOrEmpty(Descuento.P_No_Descuento))
                {
                    Mi_SQL += " WHERE " + "D." + Ope_Pre_Descuento_Traslado.Campo_No_Descuento + " = '" + Descuento.P_No_Descuento + "' ";
                }
                else
                {
                    Mi_SQL += " WHERE " + "C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " = 'POR PAGAR' ";
                }

                Mi_SQL += " ORDER BY O." + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo;

                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Descuentos de Trasladado. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Descuentos_Traslado_Busqueda
        ///DESCRIPCIÓN: Obtiene todos los Descuentos de Traslado que coincidan con la Busqueda ingresada.
        ///PARAMETROS:   
        ///             1. Descuento.   Numero del Contraresivo o Cuenta Predial que se va ver a Detalle.
        ///CREO: Jacqueline Ramirez Sierra
        ///FECHA_CREO: 15/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Descuentos_Traslado_Busqueda(Cls_Ope_Pre_Descuentos_Traslado_Negocio Descuento)
        {

            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT O." + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + " AS NO_CONTRARECIBO";
                Mi_SQL = Mi_SQL + ", CP." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " AS CUENTA_PREDIAL";
                Mi_SQL = Mi_SQL + ", CP." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " AS CUENTA_PREDIAL_ID";
                Mi_SQL = Mi_SQL + ", NVL(D." + Ope_Pre_Descuento_Traslado.Campo_Fecha_Inicial + ", SYSDATE ) AS FECHA_INICIAL";
                Mi_SQL = Mi_SQL + ", C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Recargos + " AS MONTO_RECARGOS";
                Mi_SQL = Mi_SQL + ", NVL(D." + Ope_Pre_Descuento_Traslado.Campo_Desc_Recargo + ",'0') AS DESC_RECARGO";
                Mi_SQL = Mi_SQL + ", C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Multa + " AS MONTO_MULTA";
                Mi_SQL = Mi_SQL + ", NVL(D." + Ope_Pre_Descuento_Traslado.Campo_Desc_Multa + ",'0') AS DESC_MULTA ";
                Mi_SQL = Mi_SQL + ", NVL(D." + Ope_Pre_Descuento_Traslado.Campo_Total_Por_Pagar + ", '0') AS TOTAL_POR_PAGAR";
                Mi_SQL = Mi_SQL + ", D." + Ope_Pre_Descuento_Traslado.Campo_Estatus + " AS ESTATUS";
                Mi_SQL = Mi_SQL + ", NVL(D." + Ope_Pre_Descuento_Traslado.Campo_No_Descuento + ",' ') AS NO_DESCUENTO";
                Mi_SQL = Mi_SQL + ", C." + Ope_Pre_Descuento_Traslado.Campo_No_Calculo + " AS NO_CALCULO";
                Mi_SQL = Mi_SQL + ", C." + Ope_Pre_Descuento_Traslado.Campo_Anio_Calculo + " AS ANIO_CALCULO";
                Mi_SQL = Mi_SQL + ",'TD'" + "||''|| TO_NUMBER (C." + Ope_Pre_Descuento_Traslado.Campo_No_Calculo + ")||''|| C." + Ope_Pre_Descuento_Traslado.Campo_Anio_Calculo + " AS FOLIO";
                Mi_SQL = Mi_SQL + ", C." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + " AS NO_ORDEN_VARIACION";
                Mi_SQL = Mi_SQL + ", C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + " AS ANIO_ORDEN";
                Mi_SQL = Mi_SQL + ", C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Costo_Constancia + " AS COSTO_CONSTANCIA";
                Mi_SQL = Mi_SQL + ", C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Traslado + " AS MONTO_TRASLADO";
                Mi_SQL = Mi_SQL + ", C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Division + " AS MONTO_DIVISION";
                Mi_SQL = Mi_SQL + ", C." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Adeudo + " AS NO_ADEUDO";
                Mi_SQL = Mi_SQL + ", NVL(D." + Ope_Pre_Descuento_Traslado.Campo_Realizo + ",' ') AS REALIZO";
                Mi_SQL = Mi_SQL + ", NVL(D." + Ope_Pre_Descuento_Traslado.Campo_Fecha_Vencimiento + ", SYSDATE) AS FECHA_VENCIMIENTO";
                Mi_SQL = Mi_SQL + ", NVL(D." + Ope_Pre_Descuento_Traslado.Campo_Observaciones + ",' ') AS OBSERVACIONES";
                Mi_SQL = Mi_SQL + ", NVL(D." + Ope_Pre_Descuento_Traslado.Campo_Fundamento_Legal + ",' ') AS FUNDAMENTO_LEGAL";
                Mi_SQL = Mi_SQL + ", C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Total_Pagar + " AS TOTAL";
                Mi_SQL = Mi_SQL + ",NVL(D." + Ope_Pre_Descuento_Traslado.Campo_Monto_Recargos + ", '0') AS MONTO_RECARGO";
                Mi_SQL = Mi_SQL + ",NVL(D." + Ope_Pre_Descuento_Traslado.Campo_Monto_Multas + ", '0') AS MONTO_MULTAS";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion;
                Mi_SQL = Mi_SQL + " O LEFT JOIN " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado;
                Mi_SQL = Mi_SQL + " C ON " + "(C." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + " = O." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion;
                Mi_SQL = Mi_SQL + " AND C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + " = O." + Ope_Pre_Ordenes_Variacion.Campo_Anio + ")";
                Mi_SQL = Mi_SQL + " LEFT JOIN " + Ope_Pre_Descuento_Traslado.Tabla_Ope_Pre_Descuento_Traslado;
                Mi_SQL = Mi_SQL + " D ON " + "(D." + Ope_Pre_Descuento_Traslado.Campo_No_Calculo + " = C." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo;
                Mi_SQL = Mi_SQL + " AND D." + Ope_Pre_Descuento_Traslado.Campo_Anio_Calculo + " = C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + ")";
                Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;
                Mi_SQL = Mi_SQL + " CP ON CP." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id;

                if (Descuento.P_No_Contrarecibo != null && Descuento.P_No_Contrarecibo != "")
                {
                    Mi_SQL = Mi_SQL + " WHERE " + "O." + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + " = '" + Descuento.P_No_Contrarecibo + "'";
                }

                if (Descuento.P_Folio != null && Descuento.P_Folio != "")
                {
                    Mi_SQL = Mi_SQL + " WHERE " + " TO_NUMBER(C." + Ope_Pre_Descuento_Traslado.Campo_No_Calculo + ")||''|| TO_CHAR(C." + Ope_Pre_Descuento_Traslado.Campo_Anio_Calculo + ") = '" + Descuento.P_Folio + "'";
                }

                if (Descuento.P_Cuenta_Predial != null && Descuento.P_Cuenta_Predial != "")
                {
                    Mi_SQL = Mi_SQL + " WHERE " + "CP." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Descuento.P_Cuenta_Predial + "'";
                }

               // filtrar por estatus
                if (!string.IsNullOrEmpty(Descuento.P_Estatus))
                {
                    // agregar and o where
                    if (Mi_SQL.ToUpper().Contains("WHERE"))
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    // agregar filtro de estatus calculo
                    Mi_SQL += "C." + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + Descuento.P_Estatus;
                }

                Mi_SQL = Mi_SQL + " ORDER BY O." + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los descuentos de Traslado Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Descuentos_Traslado
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo Descuento de Traslado
        ///PARAMETROS:     
        ///             1. Descuento.  Instancia de la Clase de Negocio de Descuentos de Traslado con los datos 
        ///                            del Descuento de Traslado que va a ser dado de Alta
        ///                            asi como de los datos necesarios a ingresar en la Tabla de Adeudos por Folio.
        ///CREO: Jacqueline Ramirez Sierra
        ///FECHA_CREO: 15/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Descuentos_Traslado(Cls_Ope_Pre_Descuentos_Traslado_Negocio Descuento)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Rs_Modificar_Calculo = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio(); //Variable de conexión hacia la capa de Negocios para envio de datos a modificar
            DataTable Dt_Clave = new DataTable();

            // abrir conexion con la base de datos si no llego una conexion como parametro 
            if (Descuento.P_Comando_Transaccion != null)
            {
                Cmd = Descuento.P_Comando_Transaccion;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
            }

            try
            {
                String Mi_SQL = "UPDATE " + Ope_Pre_Adeudos_Folio.Tabla_Ope_Pre_Adeudos_Folio + " SET ";
                Mi_SQL = Mi_SQL + Ope_Pre_Adeudos_Folio.Campo_No_Descuento + " = '" + Descuento.P_No_Descuento + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pre_Adeudos_Folio.Campo_Desc_Multa + " = '" + Descuento.P_Monto_Multa + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pre_Adeudos_Folio.Campo_Desc_Recargo + " = '" + Descuento.P_Monto_Recargo + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pre_Adeudos_Folio.Campo_Usuario_Creo + " = '" + Descuento.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pre_Adeudos_Folio.Campo_Fecha_Creo + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Adeudos_Folio.Campo_No_Adeudo + " = '" + Descuento.P_No_Adeudo + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                Descuento.P_Referencia = "TD" + Convert.ToInt32(Descuento.P_No_Calculo) + Descuento.P_Anio_Calculo;
                String Descuento_ID = Obtener_ID_Consecutivo(Ope_Pre_Descuento_Traslado.Tabla_Ope_Pre_Descuento_Traslado, Ope_Pre_Descuento_Traslado.Campo_No_Descuento, 10);
                Mi_SQL = "INSERT INTO " + Ope_Pre_Descuento_Traslado.Tabla_Ope_Pre_Descuento_Traslado;
                Mi_SQL = Mi_SQL + " (" + Ope_Pre_Descuento_Traslado.Campo_No_Descuento + ", " + Ope_Pre_Descuento_Traslado.Campo_No_Calculo;
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuento_Traslado.Campo_Anio_Calculo + ", " + Ope_Pre_Descuento_Traslado.Campo_No_Adeudo;
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuento_Traslado.Campo_Estatus + ", " + Ope_Pre_Descuento_Traslado.Campo_Fecha_Inicial;
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuento_Traslado.Campo_Desc_Recargo + ", " + Ope_Pre_Descuento_Traslado.Campo_Desc_Multa;
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuento_Traslado.Campo_Total_Por_Pagar + ", " + Ope_Pre_Descuento_Traslado.Campo_Realizo;
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuento_Traslado.Campo_Fecha_Vencimiento + ", " + Ope_Pre_Descuento_Traslado.Campo_Observaciones;
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuento_Traslado.Campo_Fundamento_Legal + ", " + Ope_Pre_Descuento_Traslado.Campo_Monto_Recargos;
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuento_Traslado.Campo_Monto_Multas;
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuento_Traslado.Campo_Usuario_Creo + ", " + Ope_Pre_Descuento_Traslado.Campo_Fecha_Creo;
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Descuento_Traslado.Campo_Referencia;
                Mi_SQL = Mi_SQL + ") VALUES ('" + Descuento.P_No_Descuento + "', '" + Descuento.P_No_Calculo + "'";
                Mi_SQL = Mi_SQL + ",'" + Descuento.P_Anio_Calculo + "', '" + Descuento.P_No_Adeudo + "'";
                Mi_SQL = Mi_SQL + ",'" + Descuento.P_Estatus + "', '" + Descuento.P_Fecha.ToString("dd/MM/yyyy") + "'";
                Mi_SQL = Mi_SQL + ",'" + Descuento.P_Desc_Recargo + "', '" + Descuento.P_Desc_Multa + "'";
                Mi_SQL = Mi_SQL + ",'" + Descuento.P_Total_Por_Pagar + "', '" + Descuento.P_Realizo + "'";
                Mi_SQL = Mi_SQL + ",'" + Descuento.P_Fecha_Vencimiento.ToString("dd/MM/yyyy") + "', '" + Descuento.P_Observaciones + "'";
                Mi_SQL = Mi_SQL + ",'" + Descuento.P_Fundamento_Legal + "', '" + Descuento.P_Desc_Monto_Recargos + "'";
                Mi_SQL = Mi_SQL + ",'" + Descuento.P_Desc_Monto_Multas + "'";
                Mi_SQL = Mi_SQL + ",'" + Descuento.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", SYSDATE";
                Mi_SQL = Mi_SQL + ",'" + Descuento.P_Referencia + "'";
                Mi_SQL = Mi_SQL + ")";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                // aplicar cambios en base de datos
                if (Descuento.P_Comando_Transaccion == null && Trans != null)
                {
                    Trans.Commit();
                }
            }
            catch (OracleException Ex)
            {
                if (Descuento.P_Comando_Transaccion == null && Trans != null)
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
                    Mensaje = "Error al intentar dar de Alta un Descuento de Traslado. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Descuento.P_Comando_Transaccion == null && Trans != null)
                {
                    Cn.Close();
                }
            }
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
        public static void Modificar_Descuentos_Traslado(Cls_Ope_Pre_Descuentos_Traslado_Negocio Descuento)
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
                String Mi_SQL = "UPDATE " + Ope_Pre_Descuento_Traslado.Tabla_Ope_Pre_Descuento_Traslado + " SET ";
                Mi_SQL = Mi_SQL + Ope_Pre_Descuento_Traslado.Campo_Estatus + " = '" + Descuento.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pre_Descuento_Traslado.Campo_Desc_Multa + " = '" + Descuento.P_Desc_Multa + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pre_Descuento_Traslado.Campo_Desc_Recargo + " = '" + Descuento.P_Desc_Recargo + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pre_Descuento_Traslado.Campo_Total_Por_Pagar + " = '" + Descuento.P_Total_Por_Pagar + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pre_Descuento_Traslado.Campo_Realizo + " = '" + Descuento.P_Realizo + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pre_Descuento_Traslado.Campo_Fecha_Vencimiento + " = '" + Descuento.P_Fecha_Vencimiento.ToString("dd/MM/yyyy") + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pre_Descuento_Traslado.Campo_Observaciones + " = '" + Descuento.P_Observaciones + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pre_Descuento_Traslado.Campo_Fundamento_Legal + " = '" + Descuento.P_Fundamento_Legal + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pre_Descuento_Traslado.Campo_Monto_Recargos + " = '" + Descuento.P_Desc_Monto_Recargos + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pre_Descuento_Traslado.Campo_Monto_Multas + " = '" + Descuento.P_Desc_Monto_Multas + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pre_Descuento_Traslado.Campo_Usuario_Modifico + " = '" + Descuento.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pre_Descuento_Traslado.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Descuento_Traslado.Campo_No_Descuento + " = '" + Descuento.P_No_Descuento + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                Mi_SQL = "UPDATE " + Ope_Pre_Adeudos_Folio.Tabla_Ope_Pre_Adeudos_Folio + " SET ";
                Mi_SQL = Mi_SQL + Ope_Pre_Adeudos_Folio.Campo_Desc_Multa + " = '" + Descuento.P_Monto_Multa + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pre_Adeudos_Folio.Campo_Desc_Recargo + " = '" + Descuento.P_Monto_Recargo + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pre_Adeudos_Folio.Campo_Usuario_Modifico + " = '" + Descuento.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pre_Adeudos_Folio.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Adeudos_Folio.Campo_No_Adeudo + " = '" + Descuento.P_No_Adeudo + "'";

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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Descuento
        ///DESCRIPCIÓN: Obtiene el porcentaje del descuento que un Usuario puede aplicar.
        ///PARAMETROS:   
        ///             1. Descuento.   Usuario que realiza el Descuento.
        ///CREO: Jacqueline Ramirez Sierra
        ///FECHA_CREO: 15/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static String Consultar_Descuento(Cls_Ope_Pre_Descuentos_Traslado_Negocio Descuento)
        {
            DataSet dataset;
            String Desc = "0";
            try
            {
                String Mi_SQL = "SELECT NVL(" + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Porcentaje + ", '0') AS PORCENTAJE";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Tabla_Cat_Pre_Rangos_De_Descuento_Por_Rol;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Estatus + " = 'VIGENTE' ";
                Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Tipo + " = 'TRASLADO'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Empleado_Id + " = '" + Descuento.P_Usuario + "'";
                dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null && dataset.Tables[0].Rows.Count > 0)
                {
                    Desc = dataset.Tables[0].Rows[0]["PORCENTAJE"].ToString();
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Descuentos de Trasladado. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Desc;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Clave_Maxima
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo de una Clave disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS:   
        ///CREO: Jacqueline Ramirez Sierra
        ///FECHA_CREO: 15/Agosto/2011 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static String Obtener_Clave_Maxima()
        {
            return Obtener_ID_Consecutivo(Ope_Pre_Descuento_Traslado.Tabla_Ope_Pre_Descuento_Traslado,
            Ope_Pre_Descuento_Traslado.Campo_No_Descuento, 10);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS:     
        ///             1. Tabla: Tabla de referencia a la que se sacara el ultimo ID
        ///             2. Campo: Compo al que se le asignara la ultima ID
        ///             3. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: Jacqueline Ramirez Sierra
        ///FECHA_CREO: 15/Julio/2011 
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
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 15/Julio/2011 
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
    }
    #endregion
}