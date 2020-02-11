using System;
using System.Data;
using System.Data.OracleClient;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Caja_Pagos.Datos;

namespace Presidencia.Caja_Pagos.Negocio
{
    public class Cls_Ope_Caj_Pagos_Negocio
    {
        public Cls_Ope_Caj_Pagos_Negocio()
        {
        }
        #region(Variables_Internas)
        private String Orden_Variacion_ID;
        private String Cuenta_Predial;
        private String Cuenta_Predial_ID;
        private String Anio;
        private Int16 Año_Calculo;
        private String Caja_ID;
        private String No_Caja;
        private String Banco_ID;
        private String No_Pasivo;
        private String No_Pago;
        private String No_Recibo;
        private String Empleado_ID;
        private String No_Turno;
        private String Referencia;
        private String Origen;
        private Double Monto_Corriente;
        private Double Monto_Recargos;
        private DataTable Dt_Formas_Pago;
        private DataTable Dt_Adeudos_Predial_Cajas;
        private DataTable Dt_Adeudos_Predial_Cajas_Detalle;
        private String Nombre_Usuario;
        private String Estatus;
        private String No_Calculo;
        private DateTime Fecha_Pago;
        private Double Ajuste_Tarifario;
        private Double Total_Pagar;
        private DataTable Dt_Pasivos;
        private Double Banco_Total_Pagar;
        private String Banco_No_Tarjeta;
        private String Banco_Expira_Tarjeta;
        private String Banco_Codigo_Seguridad;
        private String Banco_Titular_Banco;
        private String Banco_Clave_Operacion;
        private OracleCommand Comando_Oracle;
        #endregion
        #region (Variables_Publicas)
        public String P_Cuenta_Predial
        {
            get { return Cuenta_Predial; }
            set { Cuenta_Predial = value; }
        }
        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }
        public String P_Orden_Variacion_ID
        {
            get { return Orden_Variacion_ID; }
            set { Orden_Variacion_ID = value; }
        }
        public String P_Cuenta_Predial_ID
        {
            get { return Cuenta_Predial_ID; }
            set { Cuenta_Predial_ID = value; }
        }
        public String P_No_Calculo
        {
            get { return No_Calculo; }
            set { No_Calculo = value; }
        }
        public Int16 P_Año_Calculo
        {
            get { return Año_Calculo; }
            set { Año_Calculo = value; }
        }
        public String P_Caja_ID
        {
            get { return Caja_ID; }
            set { Caja_ID = value; }
        }
        public String P_No_Caja
        {
            get { return No_Caja; }
            set { No_Caja = value; }
        }
        public String P_Banco_ID
        {
            get { return Banco_ID; }
            set { Banco_ID = value; }
        }
        public String P_No_Pasivo
        {
            get { return No_Pasivo; }
            set { No_Pasivo = value; }
        }
        public String P_No_Pago
        {
            get { return No_Pago; }
            set { No_Pago = value; }
        }
        public String P_No_Recibo
        {
            get { return No_Recibo; }
            set { No_Recibo = value; }
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
        public String P_Origen
        {
            get { return Origen; }
            set { Origen = value; }
        }
        public Double P_Monto_Corriente
        {
            get { return Monto_Corriente; }
            set { Monto_Corriente = value; }
        }
        public Double P_Monto_Recargos
        {
            get { return Monto_Recargos; }
            set { Monto_Recargos = value; }
        }
        public DataTable P_Dt_Formas_Pago
        {
            get { return Dt_Formas_Pago; }
            set { Dt_Formas_Pago = value; }
        }
        public DataTable P_Dt_Adeudos_Predial_Cajas
        {
            get { return Dt_Adeudos_Predial_Cajas; }
            set { Dt_Adeudos_Predial_Cajas = value; }
        }
        public DataTable P_Dt_Adeudos_Predial_Cajas_Detalle
        {
            get { return Dt_Adeudos_Predial_Cajas_Detalle; }
            set { Dt_Adeudos_Predial_Cajas_Detalle = value; }
        }
        public String P_No_Turno
        {
            get { return No_Turno; }
            set { No_Turno = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public DateTime P_Fecha_Pago
        {
            get { return Fecha_Pago; }
            set { Fecha_Pago = value; }
        }
        public Double P_Total_Pagar
        {
            get { return Total_Pagar; }
            set { Total_Pagar = value; }
        }
        public Double P_Ajuste_Tarifario
        {
            get { return Ajuste_Tarifario; }
            set { Ajuste_Tarifario = value; }
        }
        public String P_Nombre_Usuario
        {
            get { return Nombre_Usuario; }
            set { Nombre_Usuario = value; }
        }
        public DataTable P_Dt_Pasivos
        {
            get { return Dt_Pasivos; }
            set { Dt_Pasivos = value; }
        }
        public Double P_Banco_Total_Pagar
        {
            get { return Banco_Total_Pagar; }
            set { Banco_Total_Pagar = value; }
        }
        public String P_Banco_No_Tarjeta
        {
            get { return Banco_No_Tarjeta; }
            set { Banco_No_Tarjeta = value; }
        }
        public String P_Banco_Expira_Tarjeta
        {
            get { return Banco_Expira_Tarjeta; }
            set { Banco_Expira_Tarjeta = value; }
        }
        public String P_Banco_Codigo_Seguridad
        {
            get { return Banco_Codigo_Seguridad; }
            set { Banco_Codigo_Seguridad = value; }
        }
        public String P_Banco_Titular_Banco
        {
            get { return Banco_Titular_Banco; }
            set { Banco_Titular_Banco = value; }
        }
        public String P_Banco_Clave_Operacion
        {
            get { return Banco_Clave_Operacion; }
            set { Banco_Clave_Operacion = value; }
        }
        public OracleCommand P_Comando_Oracle
        {
            get { return Comando_Oracle; }
            set { Comando_Oracle = value; }
        }
        #endregion
        #region(Metodos)

        public DataTable Consultar_Orden()
        {
            return Cls_Ope_Caj_Pagos_Datos.Consultar_Orden(this);
        }

        public DataTable Consultar_Orden_Variacion()
        {
            return Cls_Ope_Caj_Pagos_Datos.Consultar_Orden_Variacion(this);
        }

        public DataTable Obtener_Cuenta_Predial()
        {
            return Cls_Ope_Caj_Pagos_Datos.Obtener_Cuenta_Predial(this);
        }

        public DataTable Consultar_Datos_Calculo()
        {
            return Cls_Ope_Caj_Pagos_Datos.Consultar_Datos_Calculo(this);
        }
        public String Consulta_Cajero_General()
        {
            return Cls_Ope_Caj_Pagos_Datos.Consulta_Cajero_General();
        }
        public DataTable Consulta_Caja_Empleado()
        {
            return Cls_Ope_Caj_Pagos_Datos.Consulta_Caja_Empleado(this);
        }
        public DataTable Consulta_Bancos_Ingresos()
        {
            return Cls_Ope_Caj_Pagos_Datos.Consulta_Bancos_Ingresos();
        }
        public DataTable Consulta_Plan_Pago_Banco()
        {
            return Cls_Ope_Caj_Pagos_Datos.Consulta_Plan_Pago_Banco(this);
        }
        public DataTable Consulta_Datos_Pasivo_Cuenta_Predial()
        {
            return Cls_Ope_Caj_Pagos_Datos.Consulta_Datos_Pasivo_Cuenta_Predial(this);
        }
        public DataTable Consulta_Datos_Pasivo()
        {
            return Cls_Ope_Caj_Pagos_Datos.Consulta_Datos_Pasivo(this);
        }
        public DataTable Consulta_Detalles_Formas_Pago()
        {
            return Cls_Ope_Caj_Pagos_Datos.Consulta_Detalles_Formas_Pago(this);
        }
        public String Consulta_Cuenta_Predial()
        {
            return Cls_Ope_Caj_Pagos_Datos.Consulta_Cuenta_Predial(this);
        }
        public DataTable Consulta_Datos_Turno()
        {
            return Cls_Ope_Caj_Pagos_Datos.Consulta_Datos_Turno(this);
        }
        public void Alta_Pago_Caja()
        {
            Cls_Ope_Caj_Pagos_Datos.Alta_Pago_Caja(this);
        }
        public void Alta_Pago_Internet()
        {
            Cls_Ope_Caj_Pagos_Datos.Alta_Pago_Internet(this);
        }
        public void Alta_Pago_Instituciones_Externas(List<string> Lista_Cuentas)
        {
            Cls_Ope_Caj_Pagos_Datos.Alta_Pago_Instituciones_Externas(this, Lista_Cuentas);
        }
        public void Alta_Pago_Pae()
        {
            Cls_Ope_Caj_Pagos_Datos.Alta_Pago_PAE(this);
        }
        #endregion

    }
}