using System;
using System.Data;
using System.Data.OracleClient;
using Presidencia.Operacion_Atencion_Ciudadana_Pagos_Internet.Datos;

namespace Presidencia.Operacion_Atencion_Ciudadana_Pagos_Internet.Negocio
{
    public class Cls_Ope_Ate_Pagos_Internet_Negocio
    {
        public Cls_Ope_Ate_Pagos_Internet_Negocio()
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
        private String Origen_Pasivo;
        private String No_Solicitud_Tramite;
        private String No_Pago;
        private String No_Recibo;
        private String Empleado_ID;
        private String No_Turno;
        private String Referencia;
        private decimal Monto_Corriente;
        private decimal Monto_Recargos;
        private DataTable Dt_Formas_Pago;
        private DataTable Dt_Adeudos_Predial_Cajas;
        private DataTable Dt_Adeudos_Predial_Cajas_Detalle;
        private String Nombre_Usuario;
        private String Estatus;
        private String No_Calculo;
        private String Tipo_Pago;
        private DateTime Fecha_Pago;
        private decimal Ajuste_Tarifario;
        private decimal Total_Pagar;
        private DataTable Dt_Pasivos;
        private decimal Banco_Total_Pagar;
        private String Banco_No_Tarjeta;
        private String Banco_Expira_Tarjeta;
        private String Banco_Codigo_Seguridad;
        private String Banco_Titular_Banco;
        private String Banco_Clave_Operacion;
        private String Banco_3D_Tipo_Tarjeta;
        private String Banco_3D_XID;
        private String Banco_3D_CAVV;
        private String Banco_3D_ECI;
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
        public String P_Tipo_Pago
        {
            get { return Tipo_Pago; }
            set { Tipo_Pago = value; }
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
        public String P_Origen_Pasivo
        {
            get { return Origen_Pasivo; }
            set { Origen_Pasivo = value; }
        }
        public String P_No_Solicitud_Tramite
        {
            get { return No_Solicitud_Tramite; }
            set { No_Solicitud_Tramite = value; }
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
        public decimal P_Monto_Corriente
        {
            get { return Monto_Corriente; }
            set { Monto_Corriente = value; }
        }
        public decimal P_Monto_Recargos
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
        public decimal P_Total_Pagar
        {
            get { return Total_Pagar; }
            set { Total_Pagar = value; }
        }
        public decimal P_Ajuste_Tarifario
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
        public decimal P_Banco_Total_Pagar
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
        public String P_Banco_3D_Tipo_Tarjeta
        {
            get { return Banco_3D_Tipo_Tarjeta; }
            set { Banco_3D_Tipo_Tarjeta = value; }
        }
        public String P_Banco_3D_XID
        {
            get { return Banco_3D_XID; }
            set { Banco_3D_XID = value; }
        }
        public String P_Banco_3D_CAVV
        {
            get { return Banco_3D_CAVV; }
            set { Banco_3D_CAVV = value; }
        }
        public String P_Banco_3D_ECI
        {
            get { return Banco_3D_ECI; }
            set { Banco_3D_ECI = value; }
        }
        public OracleCommand P_Comando_Oracle
        {
            get { return Comando_Oracle; }
            set { Comando_Oracle = value; }
        }
        #endregion
        #region(Metodos)

        public int Alta_Pago_Internet()
        {
            return Cls_Ope_Ate_Pagos_Internet_Datos.Alta_Pago_Internet(this);
        }

        public DataTable Consulta_Datos_Pasivo()
        {
            return Cls_Ope_Ate_Pagos_Internet_Datos.Consulta_Datos_Pasivo(this);
        }

        public DataTable Consulta_Solicitud_Por_Pasivo()
        {
            return Cls_Ope_Ate_Pagos_Internet_Datos.Consulta_Solicitud_Por_Pasivo(this);
        }

        public DataTable Consulta_Total_Pasivo()
        {
            return Cls_Ope_Ate_Pagos_Internet_Datos.Consulta_Total_Pasivo(this);
        }
        #endregion

    }
}