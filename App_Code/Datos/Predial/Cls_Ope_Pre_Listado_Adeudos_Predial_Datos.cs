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
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Predial_Listado_Adeudos_Predial.Negocio;

/// <summary>
/// Summary description for Cls_Ope_Pre_Listado_Adeudos_Predial_Datos
/// </summary>
namespace Presidencia.Predial_Listado_Adeudos_Predial.Datos {

    public class Cls_Ope_Pre_Listado_Adeudos_Predial_Datos {

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Cuentas_Predial
        ///DESCRIPCIÓN: Consulta los datos de la Cuenta Predial enviada como parametro.
        ///PARAMETROS: 1. Datos_Negocio. Datos para ejecutar la operacion.
        ///CREO: Francisco Antonio Gallardo Castañeda.  
        ///FECHA_CREO: 12 Octubre 2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Datos_Cuentas_Predial(Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Datos_Negocio) {
            String Mi_SQL = null;
            DataTable Dt_Datos = new DataTable();
            DataSet Ds_Datos = null;
            Boolean Entro_Where = false;
            try {
                Mi_SQL = "SELECT " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " AS CUENTA_PREDIAL_ID";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " AS CUENTA_PREDIAL";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Propietario_ID + " AS PROPIETARIO_ID";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + "";
                Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + "";
                Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Nombre + " AS PROPIETARIO";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Nombre + "";
                Mi_SQL = Mi_SQL + " ||' '|| NVL(" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + ", 'S/N')";
                Mi_SQL = Mi_SQL + " ||' COL. '|| " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + "." + Cat_Ate_Colonias.Campo_Nombre + " AS UBICACION";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios;
                Mi_SQL = Mi_SQL + " ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "";
                Mi_SQL = Mi_SQL + " = " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + "";
                Mi_SQL = Mi_SQL + " AND (" + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + " = 'PROPIETARIO'";
                Mi_SQL = Mi_SQL + " OR " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + " = 'POSEEDOR')";
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes;
                Mi_SQL = Mi_SQL + " ON " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + "";
                Mi_SQL = Mi_SQL + " = " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + "";
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles;
                Mi_SQL = Mi_SQL + " ON " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Calle_ID + "";
                Mi_SQL = Mi_SQL + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + "";
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
                Mi_SQL = Mi_SQL + " ON " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Colonia_ID + "";
                Mi_SQL = Mi_SQL + " = " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + "." + Cat_Ate_Colonias.Campo_Colonia_ID + "";
                if (Datos_Negocio.P_Cuenta_Predial != null) {
                    if (!Entro_Where) { 
                        Mi_SQL = Mi_SQL + " WHERE ";
                        Entro_Where = true;
                    } else {
                        Mi_SQL = Mi_SQL + " AND ";
                    }
                    Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Datos_Negocio.P_Cuenta_Predial + "'";
                    if (Datos_Negocio.P_Referencia.StartsWith("TD"))
                    {
                        Mi_SQL = Mi_SQL + "OR " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = '" + Datos_Negocio.P_Cuenta_Predial + "'";
                    }
                }
                if (Datos_Negocio.P_Cuenta_Predial_ID != null) {
                    if (!Entro_Where) {
                        Mi_SQL = Mi_SQL + " WHERE ";
                        Entro_Where = true;
                    } else {
                        Mi_SQL = Mi_SQL + " AND ";
                    }
                    Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = '" + Datos_Negocio.P_Cuenta_Predial_ID + "'";
                }
                Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Datos != null && Ds_Datos.Tables.Count > 0) {
                    Dt_Datos = Ds_Datos.Tables[0];
                }
            } catch (Exception Ex) {
                throw new Exception("Excepción al Cargar la Información de la Cuenta Predial: ['" + Ex.Message + "']");
            }
            return Dt_Datos;
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Cuentas_Predial
        ///DESCRIPCIÓN: Consulta los datos de la Cuenta Predial enviada como parametro.
        ///PARAMETROS: 1. Datos_Negocio. Datos para ejecutar la operacion.
        ///CREO: Francisco Antonio Gallardo Castañeda.  
        ///FECHA_CREO: 12 Octubre 2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Adeudos_Totales(Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Datos_Negocio)
        {
            String Mi_SQL = null;
            DataTable Dt_Datos = new DataTable();
            DataSet Ds_Datos = null;
            try
            {
                Mi_SQL = "SELECT " + Ope_Pre_Adeudos_Predial.Campo_No_Adeudo + " AS IDENTIFICADOR";
                Mi_SQL = Mi_SQL + ", 'ADEUDO_PREDIAL' AS TIPO_CONCEPTO";
                Mi_SQL = Mi_SQL + ", '" + Datos_Negocio.P_Cuenta_Predial + "' AS REFERENCIA";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Adeudos_Predial.Campo_Fecha + " AS FECHA";
                Mi_SQL = Mi_SQL + ", 'IMPUESTO DE PREDIAL' AS DESCRIPCION";
                Mi_SQL = Mi_SQL + ", ((NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ",0))";
                Mi_SQL = Mi_SQL + " + (NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ",0))";
                Mi_SQL = Mi_SQL + " + (NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ",0))";
                Mi_SQL = Mi_SQL + " + (NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ",0))";
                Mi_SQL = Mi_SQL + " + (NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ",0))";
                Mi_SQL = Mi_SQL + " + (NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ",0))) AS IMPORTE";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + " = '" + Datos_Negocio.P_Cuenta_Predial_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Adeudos_Predial.Campo_Estatus + " = 'POR PAGAR'";
                Mi_SQL = Mi_SQL + " AND ((NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ",0))";
                Mi_SQL = Mi_SQL + " + (NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ",0))";
                Mi_SQL = Mi_SQL + " + (NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ",0))";
                Mi_SQL = Mi_SQL + " + (NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ",0))";
                Mi_SQL = Mi_SQL + " + (NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ",0))";
                Mi_SQL = Mi_SQL + " + (NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ",0))) > 0";
                Mi_SQL = Mi_SQL + " UNION ";
                Mi_SQL = Mi_SQL + " SELECT " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_No_Constancia + " AS IDENTIFICADOR";
                Mi_SQL = Mi_SQL + ", 'CONSTANCIA' AS TIPO_CONCEPTO";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Folio + " AS REFERENCIA";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Fecha + " AS FECHA";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tipos_Constancias.Tabla_Cat_Pre_Tipos_Constancias + "." + Cat_Pre_Tipos_Constancias.Campo_Nombre + " AS DESCRIPCION";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tipos_Constancias.Tabla_Cat_Pre_Tipos_Constancias + "." + Cat_Pre_Tipos_Constancias.Campo_Costo + " AS IMPORTE";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias;
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Tipos_Constancias.Tabla_Cat_Pre_Tipos_Constancias;
                Mi_SQL = Mi_SQL + " ON " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Tipo_Constancia_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Pre_Tipos_Constancias.Tabla_Cat_Pre_Tipos_Constancias + "." + Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Cuenta_Predial_ID;
                Mi_SQL = Mi_SQL + " = '" + Datos_Negocio.P_Cuenta_Predial_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Estatus + " = 'POR PAGAR'";
                Mi_SQL = Mi_SQL + " UNION ";


                //Traslado
                Mi_SQL = Mi_SQL + " SELECT " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + " AS IDENTIFICADOR";
                Mi_SQL = Mi_SQL + ", 'TRASLADO_DOMINIO' AS TIPO_CONCEPTO";
                Mi_SQL = Mi_SQL + ", 'TD' || TO_CHAR(TO_NUMBER(" + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + ")) || TO_CHAR(" + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + ") AS REFERENCIA";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_Fecha_Creo + " AS FECHA";
                Mi_SQL = Mi_SQL + ", 'TRASLADO DE DOMINIO' AS DESCRIPCION";
                Mi_SQL = Mi_SQL + ", (" + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Total_Pagar + " - NVL(" + Ope_Pre_Descuento_Traslado.Tabla_Ope_Pre_Descuento_Traslado + "." + Ope_Pre_Descuento_Traslado.Campo_Monto_Multas + ",0) - NVL(" + Ope_Pre_Descuento_Traslado.Tabla_Ope_Pre_Descuento_Traslado + "." + Ope_Pre_Descuento_Traslado.Campo_Monto_Recargos + ",0)) AS IMPORTE";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado;
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pre_Descuento_Traslado.Tabla_Ope_Pre_Descuento_Traslado + " ON " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + "=" + Ope_Pre_Descuento_Traslado.Tabla_Ope_Pre_Descuento_Traslado + "." + Ope_Pre_Descuento_Traslado.Campo_Anio_Calculo + " AND " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Descuento_Traslado.Campo_No_Calculo + "=" + Ope_Pre_Descuento_Traslado.Tabla_Ope_Pre_Descuento_Traslado + "." + Ope_Pre_Descuento_Traslado.Campo_No_Calculo + " AND " + Ope_Pre_Descuento_Traslado.Tabla_Ope_Pre_Descuento_Traslado + "." + Ope_Pre_Descuento_Traslado.Campo_Estatus + " IN ('PARCIAL','POR PAGAR','VIGENTE') AND " + Ope_Pre_Descuento_Traslado.Tabla_Ope_Pre_Descuento_Traslado + "." + Ope_Pre_Descuento_Traslado.Campo_Fecha_Vencimiento + ">=TO_DATE(SYSDATE)";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id + " = '" + Datos_Negocio.P_Cuenta_Predial_ID + "'";
                Mi_SQL = Mi_SQL + " AND (" + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " = 'POR PAGAR'";
                Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " = 'PARCIAL')";

                Mi_SQL = Mi_SQL + " UNION ";
                //Impuesto Der
                Mi_SQL = Mi_SQL + " SELECT " + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + "." + Ope_Pre_Impuestos_Derechos_Supervision.Campo_No_Impuesto_Derecho_Supervision + " AS IDENTIFICADOR";
                Mi_SQL = Mi_SQL + ", 'DERECHO_SUPERVISION' AS TIPO_CONCEPTO";
                Mi_SQL = Mi_SQL + ", 'DER' || TO_CHAR(" + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + "." + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Fecha_Creo + ", 'YY')";
                Mi_SQL = Mi_SQL + " || TO_NUMBER(" + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + "." + Ope_Pre_Impuestos_Derechos_Supervision.Campo_No_Impuesto_Derecho_Supervision + ") AS REFERENCIA";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + "." + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Fecha_Creo + " AS FECHA";
                Mi_SQL = Mi_SQL + ", 'DERECHO DE SUPERVISION' AS DESCRIPCION";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Impuestos_Derechos_Supervision + "." + Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Campo_Total + " AS IMPORTE";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + ", " + Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Impuestos_Derechos_Supervision;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + "." + Ope_Pre_Impuestos_Derechos_Supervision.Campo_No_Impuesto_Derecho_Supervision + "";
                Mi_SQL = Mi_SQL + " = " + Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Impuestos_Derechos_Supervision + "." + Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Campo_No_Impuesto_Derecho_Supervision + "";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + "." + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Cuenta_Predial_ID + " = '" + Datos_Negocio.P_Cuenta_Predial_ID + "'";
                Mi_SQL = Mi_SQL + " AND (" + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + "." + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus + " = 'POR PAGAR'";
                Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + "." + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus + " = 'PARCIAL')";
                Mi_SQL = Mi_SQL + " UNION ";

                //Descuento Der
                Mi_SQL = Mi_SQL + " SELECT " + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + "." + Ope_Pre_Impuestos_Derechos_Supervision.Campo_No_Impuesto_Derecho_Supervision + " AS IDENTIFICADOR";
                Mi_SQL = Mi_SQL + ", 'DERECHO_SUPERVISION' AS TIPO_CONCEPTO";
                Mi_SQL = Mi_SQL + ", 'DER' || TO_CHAR(" + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + "." + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Fecha_Creo + ", 'YY')";
                Mi_SQL = Mi_SQL + " || TO_NUMBER(" + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + "." + Ope_Pre_Impuestos_Derechos_Supervision.Campo_No_Impuesto_Derecho_Supervision + ") AS REFERENCIA";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + "." + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Fecha_Creo + " AS FECHA";
                Mi_SQL = Mi_SQL + ", 'DERECHO DE SUPERVISIÓN' AS DESCRIPCION";
                Mi_SQL = Mi_SQL + ", (-NVL(" + Ope_Pre_Descuento_Der_Sup.Tabla_Ope_Pre_Descuento_Der_Sup + "." + Ope_Pre_Descuento_Der_Sup.Campo_Monto_Multas + ",0)-NVL(" + Ope_Pre_Descuento_Der_Sup.Tabla_Ope_Pre_Descuento_Der_Sup + "." + Ope_Pre_Descuento_Der_Sup.Campo_Monto_Recargos + ",0)) AS IMPORTE";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Descuento_Der_Sup.Tabla_Ope_Pre_Descuento_Der_Sup + " LEFT OUTER JOIN " + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + " ON " + Ope_Pre_Descuento_Der_Sup.Tabla_Ope_Pre_Descuento_Der_Sup + "." + Ope_Pre_Descuento_Der_Sup.Campo_No_Impuesto_Derecho_Supervision + "";
                Mi_SQL = Mi_SQL + " = " + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + "." + Ope_Pre_Impuestos_Derechos_Supervision.Campo_No_Impuesto_Derecho_Supervision + "";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + "." + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Cuenta_Predial_ID + " = '" + Datos_Negocio.P_Cuenta_Predial_ID + "'";
                Mi_SQL = Mi_SQL + " AND (" + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + "." + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus + " = 'POR PAGAR'";
                Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + "." + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus + " = 'PARCIAL')";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Descuento_Der_Sup.Tabla_Ope_Pre_Descuento_Der_Sup + "." + Ope_Pre_Descuento_Der_Sup.Campo_Estatus + " IN ('PARCIAL','POR PAGAR','VIGENTE') AND " + Ope_Pre_Descuento_Der_Sup.Tabla_Ope_Pre_Descuento_Der_Sup + "." + Ope_Pre_Descuento_Der_Sup.Campo_Fecha_Vencimiento + ">=TO_DATE(SYSDATE)";

                //Impuesto Fracc
                Mi_SQL = Mi_SQL + " UNION ";
                Mi_SQL = Mi_SQL + " SELECT " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + " AS IDENTIFICADOR";
                Mi_SQL = Mi_SQL + ", 'FRACCIONAMIENTO' AS TIPO_CONCEPTO";
                Mi_SQL = Mi_SQL + ", 'IMP' || TO_CHAR(" + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Creo + ", 'YY')";
                Mi_SQL = Mi_SQL + " || TO_NUMBER(" + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + ") AS REFERENCIA";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Creo + " AS FECHA";
                Mi_SQL = Mi_SQL + ", 'IMPUESTO DE FRACCIONAMIENTO' AS DESCRIPCION";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Detalles_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Detalles_Impuestos_Fraccionamientos + "." + Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Total + " AS IMPORTE";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + ", " + Ope_Pre_Detalles_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Detalles_Impuestos_Fraccionamientos;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + "";
                Mi_SQL = Mi_SQL + " = " + Ope_Pre_Detalles_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Detalles_Impuestos_Fraccionamientos + "." + Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + "";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID + " = '" + Datos_Negocio.P_Cuenta_Predial_ID + "'";
                Mi_SQL = Mi_SQL + " AND (" + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + " = 'POR PAGAR'";
                Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + " = 'PARCIAL')";

                Mi_SQL = Mi_SQL + " UNION ";
                //Descuento Fracc
                Mi_SQL = Mi_SQL + " SELECT " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + " AS IDENTIFICADOR";
                Mi_SQL = Mi_SQL + ", 'FRACCIONAMIENTO' AS TIPO_CONCEPTO";
                Mi_SQL = Mi_SQL + ", 'IMP' || TO_CHAR(" + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Creo + ", 'YY')";
                Mi_SQL = Mi_SQL + " || TO_NUMBER(" + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + ") AS REFERENCIA";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Creo + " AS FECHA";
                Mi_SQL = Mi_SQL + ", 'DERECHO DE SUPERVISIÓN' AS DESCRIPCION";
                Mi_SQL = Mi_SQL + ", (-NVL(" + Ope_Pre_Descuento_Fracc.Tabla_Ope_Pre_Descuento_Fracc + "." + Ope_Pre_Descuento_Fracc.Campo_Monto_Multas + ",0)-NVL(" + Ope_Pre_Descuento_Fracc.Tabla_Ope_Pre_Descuento_Fracc + "." + Ope_Pre_Descuento_Fracc.Campo_Monto_Recargos + ",0)) AS IMPORTE";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Descuento_Fracc.Tabla_Ope_Pre_Descuento_Fracc + " LEFT OUTER JOIN " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + " ON " + Ope_Pre_Descuento_Fracc.Tabla_Ope_Pre_Descuento_Fracc + "." + Ope_Pre_Descuento_Fracc.Campo_No_Impuesto_fraccionamiento + "";
                Mi_SQL = Mi_SQL + " = " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + "";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID + " = '" + Datos_Negocio.P_Cuenta_Predial_ID + "'";
                Mi_SQL = Mi_SQL + " AND (" + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + " = 'POR PAGAR'";
                Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + " = 'PARCIAL')";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Descuento_Fracc.Tabla_Ope_Pre_Descuento_Fracc + "." + Ope_Pre_Descuento_Fracc.Campo_Estatus + " IN ('PARCIAL','POR PAGAR','VIGENTE') AND " + Ope_Pre_Descuento_Fracc.Tabla_Ope_Pre_Descuento_Fracc + "." + Ope_Pre_Descuento_Fracc.Campo_Fecha_Vencimiento + ">=TO_DATE(SYSDATE)";
                //SE EJECUTA LA CONSULTA
                Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Datos != null && Ds_Datos.Tables.Count > 0)
                {
                    Dt_Datos = Ds_Datos.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Excepción al Cargar la Información de la Cuenta Predial: ['" + Ex.Message + "']");
            }
            return Dt_Datos;
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
        public static String Obtener_Menu_De_Ruta(String Ruta) {
            String Ruta_Completa = "";
            try {
                String Mi_SQL = "SELECT * FROM " + Apl_Cat_Menus.Tabla_Apl_Cat_Menus;
                Mi_SQL = Mi_SQL +" WHERE " + Apl_Cat_Menus.Campo_URL_Link + " = '" + Ruta + "'";
                DataSet Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Datos != null && Ds_Datos.Tables.Count > 0) {
                    DataTable Dt_Datos = Ds_Datos.Tables[0];
                    if (Dt_Datos != null && Dt_Datos.Rows.Count > 0) {
                        Ruta_Completa = Dt_Datos.Rows[0][Apl_Cat_Menus.Campo_URL_Link].ToString() + "?PAGINA=" + Dt_Datos.Rows[0][Apl_Cat_Menus.Campo_Menu_ID].ToString();
                    }
                }
            } catch (OracleException Ex) {
                new Exception(Ex.Message);
            }
            return Ruta_Completa;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Apertura_Existente
        ///DESCRIPCIÓN: Identifica si ya hay una apertura ese día.
        ///PARAMENTROS:   
        ///             1.  Negocio. Parametro de donde se sacará si existe la apertura de esa caja ese día.
        ///CREO: Francisco Antonio Gallardo Castañeda. 
        ///FECHA_CREO: 14/Octubre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Consultar_Apertura_Turno_Existente(Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio) {
            Boolean Fecha = false;
            try {
                String Mi_SQL = "SELECT * FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Turnos.Campo_Empleado_ID + " = '" + Negocio.P_Empleado_ID + "'"; 
                Mi_SQL= Mi_SQL + " AND " + Ope_Caj_Turnos.Campo_Estatus + " = 'ABIERTO'";
                DataSet Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Datos != null && Ds_Datos.Tables.Count > 0) {
                    if (Ds_Datos.Tables[0].Rows.Count > 0) {
                        Fecha = true;    
                    }
                }
            } catch (Exception Ex) {
                String Mensaje = "Error al intentar consultar los registros de Aperturas de Turno. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Fecha;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Adeudos_Referencia
        ///DESCRIPCIÓN: Consulta los adeudos en base a una referencia.
        ///PARAMENTROS:   
        ///             1.  Negocio. Contiene los Argumentos para realizar la Operación.
        ///CREO: Francisco Antonio Gallardo Castañeda. 
        ///FECHA_CREO: 14/Octubre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Adeudos_Referencia(Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio) {
            DataTable Dt_Datos = new DataTable();
            DataSet Ds_Datos = null;
            String Mi_SQL = null;
            try {
                Mi_SQL = "SELECT " + Ope_Ing_Pasivo.Campo_No_Pasivo + " AS IDENTIFICADOR";
                Mi_SQL = Mi_SQL + ", 'PASIVO' AS TIPO_CONCEPTO";
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Referencia + " AS REFERENCIA";
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Fecha_Ingreso + " AS FECHA";
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Descripcion + " AS DESCRIPCION";
                Mi_SQL = Mi_SQL + ", NVL(" + Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID + ",'') AS CUENTA_PREDIAL_ID";
                Mi_SQL = Mi_SQL + ", NVL(" + Ope_Ing_Pasivo.Campo_Contribuyente + ",'') AS CONTRIBUYENTE";
                Mi_SQL = Mi_SQL + ", (NVL(" + Ope_Ing_Pasivo.Campo_Monto + ",0) + NVL(" + Ope_Ing_Pasivo.Campo_Recargos + ",0)) AS IMPORTE";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Ing_Pasivo.Campo_Estatus + " = 'POR PAGAR'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Ing_Pasivo.Campo_Referencia + " = '" + Negocio.P_Referencia.Trim() + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Ing_Pasivo.Campo_Descripcion + " <> 'AJUSTE TARIFARIO'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Ing_Pasivo.Campo_No_Pasivo + " ASC";
                Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Datos != null && Ds_Datos.Tables.Count > 0) {
                    Dt_Datos = Ds_Datos.Tables[0];
                }
            } catch (Exception Ex) {
                throw new Exception("Consultar_Adeudos_Referencia: " + Ex.Message);
            }
            return Dt_Datos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Convenio_Cuenta_Predia
        ///DESCRIPCIÓN: Consulta los convenios en base a una cuenta predial.
        ///PARAMENTROS:   
        ///             1.  Negocio. Contiene los Argumentos para realizar la Operación.
        ///CREO: Francisco Antonio Gallardo Castañeda. 
        ///FECHA_CREO: 22/Octubre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Convenio_Cuenta_Predia(Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio) {
            DataTable Dt_Datos = new DataTable();
            DataSet Ds_Datos = null;
            String Mi_SQL = null;
            try {
                Mi_SQL = "SELECT " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " AS NO_CONVENIO";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento + " AS FECHA_VENCIMIENTO";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + " AS PARCIALIDAD";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Numero_Parcialidades;
                Mi_SQL = Mi_SQL + ", (NVL(" + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios + ", 0)";
                Mi_SQL = Mi_SQL + " + NVL(" + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios + ", 0)";
                Mi_SQL = Mi_SQL + " + NVL(" + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios + ", 0)";
                Mi_SQL = Mi_SQL + " + NVL(" + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto + ", 0)) AS IMPORTE";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial;
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial;
                Mi_SQL = Mi_SQL + " ON " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_No_Convenio;
                Mi_SQL = Mi_SQL + " = " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio;
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'POR PAGAR'";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Estatus + "= 'ACTIVO'";
                //Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Anticipo + "= 'PAGADO'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id + "= '" + Negocio.P_Cuenta_Predial_ID + "'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_No_Convenio;
                Mi_SQL = Mi_SQL + ", PARCIALIDAD";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento + "";
                Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Datos != null && Ds_Datos.Tables.Count > 0) {
                    Dt_Datos = Ds_Datos.Tables[0];
                }
            } catch (Exception Ex) {
                throw new Exception("Error: [Consultar_Convenio_Cuenta_Predia:'" +  Ex.Message + "']");
            }
            return Dt_Datos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Convenio_Traslado_Dominio
        ///DESCRIPCIÓN: Consulta los convenios en base a una cuenta predial.
        ///PARAMENTROS:   
        ///             1.  Negocio. Contiene los Argumentos para realizar la Operación.
        ///CREO: Ismael Prieto Sánchez 
        ///FECHA_CREO: 1/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Convenio_Traslado_Dominio(Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio)
        {
            DataTable Dt_Datos = new DataTable();
            DataSet Ds_Datos = null;
            String Mi_SQL = null;
            try
            {
                Mi_SQL = "SELECT " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio + " AS NO_CONVENIO";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Fecha_Vencimiento + " AS FECHA_VENCIMIENTO";
                Mi_SQL = Mi_SQL + ", (" + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago + "";
                Mi_SQL = Mi_SQL + " ||'/'|| " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." + Ope_Pre_Convenios_Traslados_Dominio.Campo_Numero_Parcialidades + ") AS PARCIALIDAD, ";
                //Mi_SQL = Mi_SQL + ", (NVL(" + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Monto_Honorarios + ", 0)";
                Mi_SQL = Mi_SQL + " (NVL(" + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Recargos_Ordinarios + ", 0)";
                Mi_SQL = Mi_SQL + " + NVL(" + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Recargos_Moratorios + ", 0)";
                Mi_SQL = Mi_SQL + " + NVL(" + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Monto + ", 0)";
                Mi_SQL = Mi_SQL + " + NVL(" + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Monto_Multas + ", 0)";
                Mi_SQL = Mi_SQL + " + NVL(" + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Monto_Impuesto + ", 0)) AS IMPORTE";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio;
                Mi_SQL = Mi_SQL + " INNER JOIN " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio;
                Mi_SQL = Mi_SQL + " ON " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio;
                Mi_SQL = Mi_SQL + " = " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio;
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Estatus + " = 'POR PAGAR'";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." + Ope_Pre_Convenios_Traslados_Dominio.Campo_Estatus + "= 'ACTIVO'";
                //Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." + Ope_Pre_Convenios_Traslados_Dominio.Campo_Anticipo + "= 'PAGADO'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." + Ope_Pre_Convenios_Traslados_Dominio.Campo_Cuenta_Predial_ID + "= '" + Negocio.P_Cuenta_Predial_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Calculo + " = '" + Negocio.P_No_Traslado + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Convenios_Traslados_Dominio.Campo_Anio + " = " + Negocio.P_Anio_Traslado;
                Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio;
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago + "";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Fecha_Vencimiento + "";
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Convenio_Traslado_Dominio_Parcialidades
        ///DESCRIPCIÓN: Consulta las parcialidades de los convenios en base a una cuenta predial.
        ///PARAMENTROS:   
        ///             1.  Negocio. Contiene los Argumentos para realizar la Operación.
        ///CREO: Ismael Prieto Sánchez 
        ///FECHA_CREO: 1/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Convenio_Traslado_Dominio_Parcialidades(Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio)
        {
            DataTable Dt_Datos = new DataTable();
            DataSet Ds_Datos = null;
            String Mi_SQL = null;
            try
            {
                Mi_SQL = "SELECT " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago;
                Mi_SQL += ", " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Monto_Impuesto;
                Mi_SQL += ", NVL(" + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Monto_Impuesto_Division + ",0) AS " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Monto_Impuesto_Division;
                Mi_SQL += ", NVL(" + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Monto_Constancia + ",0) AS " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Monto_Constancia;
                Mi_SQL += ", " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Monto;
                Mi_SQL += ", " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Monto_Multas;
                Mi_SQL += ", " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Recargos_Ordinarios;
                Mi_SQL += ", " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Recargos_Moratorios;
                Mi_SQL += ", NVL(" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + "," + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID + ") AS TIPO_PREDIO_ID";
                Mi_SQL += " FROM " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio;
                Mi_SQL += ", " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio; 
                Mi_SQL += ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion;
                Mi_SQL += ", " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado;
                Mi_SQL += " WHERE " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio + " = " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio;
                Mi_SQL += " AND " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." + Ope_Pre_Convenios_Traslados_Dominio.Campo_Cuenta_Predial_ID + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;

                Mi_SQL += " AND " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Calculo + " = " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo;
                Mi_SQL += " AND " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." + Ope_Pre_Convenios_Traslados_Dominio.Campo_Anio + " = " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo;

                Mi_SQL += " AND " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion;
                Mi_SQL += " AND " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden;

                Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio + " = '" + Negocio.P_No_Convenio + "'";
                if (Negocio.P_No_Convenio_Pago != 0)
                {
                    Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago + " = " + Negocio.P_No_Convenio_Pago;
                }
                Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Estatus + " = 'POR PAGAR'";
                Mi_SQL += " ORDER BY " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago;
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Convenio_Fraccionamiento
        ///DESCRIPCIÓN: Consulta los convenios en base a una cuenta predial.
        ///PARAMENTROS:   
        ///             1.  Negocio. Contiene los Argumentos para realizar la Operación.
        ///CREO: Ismael Prieto Sánchez 
        ///FECHA_CREO: 13/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Convenio_Fraccionamiento(Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio)
        {
            DataTable Dt_Datos = new DataTable();
            DataSet Ds_Datos = null;
            String Mi_SQL = null;
            try
            {
                Mi_SQL = "SELECT " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "." + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " AS NO_CONVENIO";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento + " AS FECHA_VENCIMIENTO";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + " AS PARCIALIDAD";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "." + Ope_Pre_Convenios_Fraccionamientos.Campo_Numero_Parcialidades + ", ";
                //Mi_SQL = Mi_SQL + ", (NVL(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Honorarios + ", 0)";
                Mi_SQL = Mi_SQL + " (NVL(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios + ", 0)";
                Mi_SQL = Mi_SQL + " + NVL(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios + ", 0)";
                Mi_SQL = Mi_SQL + " + NVL(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas + ", 0)";
                Mi_SQL = Mi_SQL + " + NVL(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto + ", 0)) AS IMPORTE";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos;
                Mi_SQL = Mi_SQL + " INNER JOIN " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos;
                Mi_SQL = Mi_SQL + " ON " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "." + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio;
                Mi_SQL = Mi_SQL + " = " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio;
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + " = 'POR PAGAR'";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "." + Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus + "= 'ACTIVO'";
                //Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "." + Ope_Pre_Convenios_Fraccionamientos.Campo_Anticipo + "= 'PAGADO'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "." + Ope_Pre_Convenios_Fraccionamientos.Campo_Cuenta_Predial_ID + "= '" + Negocio.P_Cuenta_Predial_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + " = '" + Negocio.P_No_Impuesto_Fraccionamiento + "'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "." + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio;
                Mi_SQL = Mi_SQL + ", PARCIALIDAD";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento + "";
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Convenio_Fraccionamiento_Parcialidades
        ///DESCRIPCIÓN: Consulta las parcialidades de los convenios en base a una cuenta predial.
        ///PARAMENTROS:   
        ///             1.  Negocio. Contiene los Argumentos para realizar la Operación.
        ///CREO: Ismael Prieto Sánchez 
        ///FECHA_CREO: 13/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Convenio_Fraccionamiento_Parcialidades(Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio)
        {
            DataTable Dt_Datos = new DataTable();
            DataSet Ds_Datos = null;
            String Mi_SQL = null;
            try
            {
                Mi_SQL = "SELECT " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago;
                Mi_SQL += ", " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto;
                Mi_SQL += ", '0' AS MONTO";
                Mi_SQL += ", " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas;
                Mi_SQL += ", " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios;
                Mi_SQL += ", " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios;
                Mi_SQL += " FROM " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos;
                Mi_SQL += " WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + Negocio.P_No_Convenio + "'";
                if (Negocio.P_No_Convenio_Pago != 0)
                {
                    Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + " = " + Negocio.P_No_Convenio_Pago;
                }
                Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + " = 'POR PAGAR'";
                Mi_SQL += " ORDER BY " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago;
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Convenio_Derechos_Supervision
        ///DESCRIPCIÓN: Consulta los convenios en base a una cuenta predial.
        ///PARAMENTROS:   
        ///             1.  Negocio. Contiene los Argumentos para realizar la Operación.
        ///CREO: Ismael Prieto Sánchez 
        ///FECHA_CREO: 13/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Convenio_Derechos_Supervision(Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio)
        {
            DataTable Dt_Datos = new DataTable();
            DataSet Ds_Datos = null;
            String Mi_SQL = null;
            try
            {
                Mi_SQL = "SELECT " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "." + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio + " AS NO_CONVENIO";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Fecha_Vencimiento + " AS FECHA_VENCIMIENTO";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago + " AS PARCIALIDAD";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "." + Ope_Pre_Convenios_Derechos_Supervision.Campo_Numero_Parcialidades + ", ";
                //Mi_SQL = Mi_SQL + ", (NVL(" + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Monto_Honorarios + ", 0)";
                Mi_SQL = Mi_SQL + " (NVL(" + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Recargos_Ordinarios + ", 0)";
                Mi_SQL = Mi_SQL + " + NVL(" + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Recargos_Moratorios + ", 0)";
                Mi_SQL = Mi_SQL + " + NVL(" + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Monto_Multas + ", 0)";
                Mi_SQL = Mi_SQL + " + NVL(" + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Monto_Impuesto + ", 0)) AS IMPORTE";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision;
                Mi_SQL = Mi_SQL + " INNER JOIN " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision;
                Mi_SQL = Mi_SQL + " ON " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "." + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio;
                Mi_SQL = Mi_SQL + " = " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio;
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Estatus + " = 'POR PAGAR'";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "." + Ope_Pre_Convenios_Derechos_Supervision.Campo_Estatus + "= 'ACTIVO'";
                //Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "." + Ope_Pre_Convenios_Derechos_Supervision.Campo_Anticipo + "= 'PAGADO'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "." + Ope_Pre_Convenios_Derechos_Supervision.Campo_Cuenta_Predial_ID + "= '" + Negocio.P_Cuenta_Predial_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Impuesto_Dereho_Supervisio + " = '" + Negocio.P_No_Impuesto_Derecho_Supervision + "'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "." + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio;
                Mi_SQL = Mi_SQL + ", PARCIALIDAD";
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Fecha_Vencimiento + "";
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Convenio_Derechos_Supervision_Parcialidades
        ///DESCRIPCIÓN: Consulta las parcialidades de los convenios en base a una cuenta predial.
        ///PARAMENTROS:   
        ///             1.  Negocio. Contiene los Argumentos para realizar la Operación.
        ///CREO: Ismael Prieto Sánchez 
        ///FECHA_CREO: 14/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Convenio_Derechos_Supervision_Parcialidades(Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio)
        {
            DataTable Dt_Datos = new DataTable();
            DataSet Ds_Datos = null;
            String Mi_SQL = null;
            try
            {
                Mi_SQL = "SELECT " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago;
                Mi_SQL += ", " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Monto_Impuesto;
                Mi_SQL += ", '0' AS MONTO";
                Mi_SQL += ", " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Monto_Multas;
                Mi_SQL += ", " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Recargos_Ordinarios;
                Mi_SQL += ", " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Recargos_Moratorios;
                Mi_SQL += " FROM " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision;
                Mi_SQL += " WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + Negocio.P_No_Convenio + "'";
                if (Negocio.P_No_Convenio_Pago != 0)
                {
                    Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago + " = " + Negocio.P_No_Convenio_Pago;
                }
                Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Estatus + " = 'POR PAGAR'";
                Mi_SQL += " ORDER BY " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago;
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Clave_Ingreso
        ///DESCRIPCIÓN: Consulta los Datos de una Clave de Ingreso.
        ///PARAMENTROS:   
        ///             1.  Negocio. Contiene los Argumentos para realizar la Operación.
        ///CREO: Francisco Antonio Gallardo Castañeda. 
        ///FECHA_CREO: 25/Octubre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Datos_Clave_Ingreso(Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio)
        {
            DataTable Dt_Datos = new DataTable();
            DataSet Ds_Datos = null;
            String Mi_SQL = null;
            try {
                Mi_SQL = "SELECT " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Descripcion + " AS DESCRIPCION";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Fundamento + " AS FUNDAMENTO";
                Mi_SQL = Mi_SQL + ", (" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + "";
                Mi_SQL = Mi_SQL + " ||' - '|| " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ") AS DEPENDENCIA";
                Mi_SQL = Mi_SQL + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " AS DEPENDENCIA_ID";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + " AS CLAVE_INGRESO_ID";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso;
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias;
                Mi_SQL = Mi_SQL + " ON " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";
                Mi_SQL = Mi_SQL + " WHERE (SELECT COUNT(*) FROM " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det + "." + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + ") = 0";
                Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Clave + " = '" + Negocio.P_Clave_Ingreso + "'";
                Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Datos != null && Ds_Datos.Tables.Count > 0) {
                    Dt_Datos = Ds_Datos.Tables[0];
                }
            } catch (Exception Ex) {
                throw new Exception("Error: [Consultar_Datos_Clave_Ingreso:'" + Ex.Message + "']");
            }
            return Dt_Datos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Pasivo
        ///DESCRIPCIÓN: Da de Alta el Pasivo a Pagar
        ///PARAMENTROS:   
        ///             1.  Parametros. Contiene los Argumentos para realizar la Operación.
        ///CREO: Francisco Antonio Gallardo Castañeda. 
        ///FECHA_CREO: 26/Octubre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Pasivo(Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Parametros) {
                String Mensaje = "";
                String Mi_SQL = "";
                OracleConnection Cn = new OracleConnection();
                OracleCommand Cmd = new OracleCommand();
                OracleTransaction Trans;
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                try {
                    //Elimina el pasivo si existe
                    Mi_SQL = "DELETE FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Ing_Pasivo.Campo_Usuario_Creo + " = '" + Parametros.P_Usuario + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Ing_Pasivo.Campo_Estatus + " = 'POR PAGAR'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();

                    //Registra el pasivo
                    Mi_SQL = "INSERT INTO " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                    Mi_SQL = Mi_SQL + "(" + Ope_Ing_Pasivo.Campo_No_Pasivo;
                    Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Referencia;
                    Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Descripcion;
                    Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Fecha_Ingreso;
                    Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Fecha_Vencimiento;
                    Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Monto;
                    Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Recargos;
                    Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Estatus;
                    Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Dependencia_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Fecha_Creo;
                    Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Usuario_Creo;
                    Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Cantidad;
                    Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Contribuyente;
                    Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Observaciones;
                    Mi_SQL = Mi_SQL + ") VALUES(" + Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo, Ope_Ing_Pasivo.Campo_No_Pasivo, 10));
                    Mi_SQL = Mi_SQL + ",'" + Parametros.P_Referencia + "'";
                    Mi_SQL = Mi_SQL + ",'" + Parametros.P_Clave_Ingreso + "'";
                    Mi_SQL = Mi_SQL + ",'" + Parametros.P_Descripcion + "'";
                    Mi_SQL = Mi_SQL + ", SYSDATE";
                    Mi_SQL = Mi_SQL + ", SYSDATE";
                    Mi_SQL = Mi_SQL + ",'" + Parametros.P_Monto + "',0";
                    Mi_SQL = Mi_SQL + ",'POR PAGAR'";
                    Mi_SQL = Mi_SQL + ",'" + Parametros.P_Dependencia_ID + "', SYSDATE,'" + Parametros.P_Usuario + "','" + Parametros.P_Cantidad.ToString() + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Contribuyente + "', '" + Parametros.P_Observaciones + "')";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Trans.Commit();
                } catch (OracleException Ex) {
                    Trans.Rollback();
                    //variable para el mensaje 
                    //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                    if (Ex.Code == 8152) {
                        Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    } else if (Ex.Code == 2627) {
                        if (Ex.Message.IndexOf("PRIMARY") != -1) {
                            Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                        } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                            Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                        } else {
                            Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
                        }
                    } else if (Ex.Code == 547) {
                        Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
                    } else if (Ex.Code == 515) {
                        Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    } else {
                        Mensaje = "Error al intentar dar de Alta una Raza. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    }
                    //Indicamos el mensaje 
                    throw new Exception(Mensaje);
                } finally {
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
        public static String Obtener_ID_Consecutivo(String Tabla, String Campo, Int32 Longitud_ID) {
            String Id = Convertir_A_Formato_ID(1, Longitud_ID); ;
            try {
                String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla;
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals("")) {
                    Id = Convertir_A_Formato_ID((Convert.ToInt32(Obj_Temp) + 1), Longitud_ID);
                }
            } catch (OracleException Ex) {
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
        private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID) {
            String Retornar = "";
            String Dato = "" + Dato_ID;
            for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++) {
                Retornar = Retornar + "0";
            }
            Retornar = Retornar + Dato;
            return Retornar;
        }      
    
    }
    
}
