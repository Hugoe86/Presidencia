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
using Presidencia.Predial_Listado_Adeudos_Predial.Datos;
using Presidencia.Operacion_Predial_Recepcion_Pagos.Negocio;

/// <summary>
/// Summary description for Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio
/// </summary>
namespace Presidencia.Predial_Listado_Adeudos_Predial.Negocio {
    
    public class Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio {

        #region "Variables Internas"

            private String Cuenta_Predial_ID = null;
            private String Cuenta_Predial = null;
            private String Empleado_ID = null;
            private String Referencia = null;
            private String Clave_Ingreso = null;
            private Int32 Cantidad = (-1);
            private Double Monto;
            private String Dependencia_ID = null;
            private String Descripcion = null;
            private String No_Convenio = null;
            private Int32 No_Convenio_Pago = 0;
            private String No_Traslado = null;
            private Int32 Anio_Traslado = 0;
            private String No_Impuesto_Fraccionamiento = null;
            private String No_Impuesto_Derecho_Supervision = null;
            private String Usuario = null;
            private String Contribuyente = null;
            private String Observaciones = null;

        #endregion

        #region "Variables Publicas"

            public String P_Cuenta_Predial_ID {
                get { return Cuenta_Predial_ID; }
                set { Cuenta_Predial_ID = value; }
            }
            public String P_Cuenta_Predial {
                get { return Cuenta_Predial; }
                set { Cuenta_Predial = value; }
            }
            public String P_Empleado_ID
            {
                get { return Empleado_ID; }
                set { Empleado_ID = value; }
            }
            public String P_Referencia
            {
                get { return Referencia; }
                set { Referencia = value; }
            }
            public String P_Clave_Ingreso
            {
                get { return Clave_Ingreso; }
                set { Clave_Ingreso = value; }
            }
            public Int32 P_Cantidad
            {
                get { return Cantidad; }
                set { Cantidad = value; }
            }
            public Double P_Monto
            {
                get { return Monto; }
                set { Monto = value; }
            }
            public String P_Dependencia_ID
            {
                get { return Dependencia_ID; }
                set { Dependencia_ID = value; }
            }
            public String P_Descripcion
            {
                get { return Descripcion; }
                set { Descripcion = value; }
            }
            public String P_No_Convenio
            {
                get { return No_Convenio; }
                set { No_Convenio = value; }
            }
            public Int32 P_No_Convenio_Pago
            {
                get { return No_Convenio_Pago; }
                set { No_Convenio_Pago = value; }
            }
            public String P_No_Traslado
            {
                get { return No_Traslado; }
                set { No_Traslado = value; }
            }
            public Int32 P_Anio_Traslado
            {
                get { return Anio_Traslado; }
                set { Anio_Traslado = value; }
            }
            public String P_No_Impuesto_Fraccionamiento
            {
                get { return No_Impuesto_Fraccionamiento; }
                set { No_Impuesto_Fraccionamiento = value; }
            }
            public String P_No_Impuesto_Derecho_Supervision
            {
                get { return No_Impuesto_Derecho_Supervision; }
                set { No_Impuesto_Derecho_Supervision = value; }
            }
            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value; }
            }
            public String P_Contribuyente
            {
                get { return Contribuyente; }
                set { Contribuyente = value; }
            }
            public String P_Observaciones
            {
                get { return Observaciones; }
                set { Observaciones = value; }
            }

        #endregion

        #region "Metodos"

            public DataTable Consultar_Datos_Cuentas_Predial() {
                return Cls_Ope_Pre_Listado_Adeudos_Predial_Datos.Consultar_Datos_Cuentas_Predial(this);
            }
            public DataTable Consultar_Adeudos_Totales() {
                return Cls_Ope_Pre_Listado_Adeudos_Predial_Datos.Consultar_Adeudos_Totales(this);
            }
            public String Obtener_Menu_De_Ruta(String Ruta) {
                return Cls_Ope_Pre_Listado_Adeudos_Predial_Datos.Obtener_Menu_De_Ruta(Ruta);
            }
            public Boolean Consultar_Apertura_Turno_Existente() {
                return Cls_Ope_Pre_Listado_Adeudos_Predial_Datos.Consultar_Apertura_Turno_Existente(this);
            }
            public DataTable Consultar_Adeudos_Referencia() {
                return Cls_Ope_Pre_Listado_Adeudos_Predial_Datos.Consultar_Adeudos_Referencia(this);
            }
            public Double Consultar_Adeudos_Predial_Cuenta() {
                Double Total_Debe = 0.0;
                Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio Recepcion_Pago_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
                Recepcion_Pago_Negocio.P_Cuenta_Predial = this.P_Cuenta_Predial_ID.Trim();
                DataTable Dt_Pagos =  Recepcion_Pago_Negocio.Consultar_Adeudos_Cuentas_Predial();
                if (Dt_Pagos != null && Dt_Pagos.Rows.Count > 0) {
                    for (Int32 Contador = 0; Contador < Dt_Pagos.Rows.Count; Contador++) {
                        Total_Debe = Total_Debe + Convert.ToDouble(Dt_Pagos.Rows[Contador]["MONTO"].ToString());
                    }
                }
                return Total_Debe;
            }
            public DataTable Consultar_Convenio_Cuenta_Predia() {
                return Cls_Ope_Pre_Listado_Adeudos_Predial_Datos.Consultar_Convenio_Cuenta_Predia(this);
            }
            public DataTable Consultar_Convenio_Traslado_Dominio()
            {
                return Cls_Ope_Pre_Listado_Adeudos_Predial_Datos.Consultar_Convenio_Traslado_Dominio(this);
            }
            public DataTable Consultar_Convenio_Traslado_Dominio_Parcialidades()
            {
                return Cls_Ope_Pre_Listado_Adeudos_Predial_Datos.Consultar_Convenio_Traslado_Dominio_Parcialidades(this);
            }
            public DataTable Consultar_Convenio_Fraccionamiento()
            {
                return Cls_Ope_Pre_Listado_Adeudos_Predial_Datos.Consultar_Convenio_Fraccionamiento(this);
            }
            public DataTable Consultar_Convenio_Fraccionamiento_Parcialidades()
            {
                return Cls_Ope_Pre_Listado_Adeudos_Predial_Datos.Consultar_Convenio_Fraccionamiento_Parcialidades(this);
            }
            public DataTable Consultar_Convenio_Derechos_Supervision()
            {
                return Cls_Ope_Pre_Listado_Adeudos_Predial_Datos.Consultar_Convenio_Derechos_Supervision(this);
            }
            public DataTable Consultar_Convenio_Derechos_Supervision_Parcialidades()
            {
                return Cls_Ope_Pre_Listado_Adeudos_Predial_Datos.Consultar_Convenio_Derechos_Supervision_Parcialidades(this);
            }
            public DataTable Consultar_Datos_Clave_Ingreso() {
                return Cls_Ope_Pre_Listado_Adeudos_Predial_Datos.Consultar_Datos_Clave_Ingreso(this);
            }
            public void Alta_Pasivo() {
                Cls_Ope_Pre_Listado_Adeudos_Predial_Datos.Alta_Pasivo(this);
            }

        #endregion

    }

}
