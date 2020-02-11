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
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Prestamos.Datos;

namespace Presidencia.Prestamos.Negocio
{
    public class Cls_Ope_Nom_Pestamos_Negocio
    {
        #region(Variables Internas)
        private String No_Solicitud;
        private String Solicita_Empleado_ID;
        private String Aval_Empleado_ID;
        private String Proveedor_ID;
        private String Nomina_ID;
        private Int32 No_Nomina;
        private String Percepcion_Deduccion_ID;
        private String Estatus_Solicitud;
        private String Estatus_Pago;
        private String Fecha_Solicitud;
        private String Fecha_Autorizacion;
        private String Fecha_Inicio_Pago;
        private String Fecha_Termino_Pago;
        private String Motivo_Prestamo;
        private Int32 No_Pagos;
        private Double Importe_Prestamo;
        private Double Importe_Interes;
        private Double Total_Prestamo;
        private Double Monto_Abonado;
        private Double Saldo_Actual;
        private Double Abono;
        private Int32 No_Abono;
        private String Comentarios_Cancelacion;
        private String Comentarios_Rechazo;
        private String Referencia_Recibo_Pago;
        private String Usuario_Creo;
        private String Usuario_Modifico;
        private DataTable Dt_Solicitudes_Prestamos;
        private DataTable Dt_Prestamos;
        private String Solicita_No_Empleado;
        private String Aval_No_Empleado;
        private String RFC_Empleado_Solicita_Prestamo;
        private String RFC_Empleado_Aval_Prestamo;
        private String Fecha_Finiquito_Prestamo;
        private String Estado_Prestamo;
        private String No_Empleado;
        private String Aplica_Validaciones;
        #endregion

        #region(Variables Publicas)
        public String P_No_Solicitud
        {
            get { return No_Solicitud; }
            set { No_Solicitud = value; }
        }
        public String P_Solicita_Empleado_ID
        {
            get { return Solicita_Empleado_ID; }
            set { Solicita_Empleado_ID = value; }
        }
        public String P_Aval_Empleado_ID
        {
            get { return Aval_Empleado_ID; }
            set { Aval_Empleado_ID = value; }
        }
        public String P_Proveedor_ID
        {
            get { return Proveedor_ID; }
            set { Proveedor_ID = value; }
        }
        public String P_Nomina_ID
        {
            get { return Nomina_ID; }
            set { Nomina_ID = value; }
        }
        public Int32 P_No_Nomina
        {
            get { return No_Nomina; }
            set { No_Nomina = value; }
        }
        public String P_Percepcion_Deduccion_ID
        {
            get { return Percepcion_Deduccion_ID; }
            set { Percepcion_Deduccion_ID = value; }
        }
        public String P_Estatus_Solicitud
        {
            get { return Estatus_Solicitud; }
            set { Estatus_Solicitud = value; }
        }
        public String P_Estatus_Pago
        {
            get { return Estatus_Pago; }
            set { Estatus_Pago = value; }
        }
        public String P_Fecha_Solicitud
        {
            get { return Fecha_Solicitud; }
            set { Fecha_Solicitud = value; }
        }
        public String P_Fecha_Autorizacion
        {
            get { return Fecha_Autorizacion; }
            set { Fecha_Autorizacion = value; }
        }
        public String P_Fecha_Inicio_Pago
        {
            get { return Fecha_Inicio_Pago; }
            set { Fecha_Inicio_Pago = value; }
        }
        public String P_Fecha_Termino_Pago
        {
            get { return Fecha_Termino_Pago; }
            set { Fecha_Termino_Pago = value; }
        }
        public String P_Motivo_Prestamo
        {
            get { return Motivo_Prestamo; }
            set { Motivo_Prestamo = value; }
        }
        public Int32 P_No_Pagos
        {
            get { return No_Pagos; }
            set { No_Pagos = value; }
        }
        public Double P_Importe_Prestamo
        {
            get { return Importe_Prestamo; }
            set { Importe_Prestamo = value; }
        }
        public Double P_Importe_Interes
        {
            get { return Importe_Interes; }
            set { Importe_Interes = value; }
        }
        public Double P_Total_Prestamo
        {
            get { return Total_Prestamo; }
            set { Total_Prestamo = value; }
        }
        public Double P_Monto_Abonado
        {
            get { return Monto_Abonado; }
            set { Monto_Abonado = value; }
        }
        public Double P_Saldo_Actual
        {
            get { return Saldo_Actual; }
            set { Saldo_Actual = value; }
        }
        public Double P_Abono
        {
            get { return Abono; }
            set { Abono = value; }
        }
        public Int32 P_No_Abono
        {
            get { return No_Abono; }
            set { No_Abono = value; }
        }
        public String P_Comentarios_Cancelacion
        {
            get { return Comentarios_Cancelacion; }
            set { Comentarios_Cancelacion = value; }
        }
        public String P_Comentarios_Rechazo
        {
            get { return Comentarios_Rechazo; }
            set { Comentarios_Rechazo = value; }
        }
        public String P_Referencia_Recibo_Pago
        {
            get { return Referencia_Recibo_Pago; }
            set { Referencia_Recibo_Pago = value; }
        }
        public String P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }
        public String P_Usuario_Modifico
        {
            get { return Usuario_Modifico; }
            set { Usuario_Modifico = value; }
        }
        public DataTable P_Dt_Solicitudes_Prestamos
        {
            get { return Dt_Solicitudes_Prestamos; }
            set { Dt_Solicitudes_Prestamos = value; }
        }
        public DataTable P_Dt_Prestamos
        {
            get { return Dt_Prestamos; }
            set { Dt_Prestamos = value; }
        }
        public String P_Solicita_No_Empleado
        {
            get { return Solicita_No_Empleado; }
            set { Solicita_No_Empleado = value; }
        }
        public String P_Aval_No_Empleado
        {
            get { return Aval_No_Empleado; }
            set { Aval_No_Empleado = value; }
        }
        public String P_RFC_Empleado_Solicita_Prestamo
        {
            get { return RFC_Empleado_Solicita_Prestamo; }
            set { RFC_Empleado_Solicita_Prestamo = value; }
        }
        public String P_RFC_Empleado_Aval_Prestamo
        {
            get { return RFC_Empleado_Aval_Prestamo; }
            set { RFC_Empleado_Aval_Prestamo = value; }
        }
        public String P_Fecha_Finiquito_Prestamo
        {
            get { return Fecha_Finiquito_Prestamo; }
            set { Fecha_Finiquito_Prestamo = value; }
        }
        public String P_Estado_Prestamo
        {
            get { return Estado_Prestamo; }
            set { Estado_Prestamo = value; }
        }

        public String P_No_Empleado {
            get { return No_Empleado; }
            set { No_Empleado = value; }
        }

        public String P_Aplica_Validaciones
        {
            get { return Aplica_Validaciones; }
            set { Aplica_Validaciones = value; }
        }
        #endregion

        #region(Metodos)
        public Boolean Alta_Solicitud_Prestamo()
        {
            return Cls_Ope_Nom_Prestamos_Datos.Alta_Solicitud_Prestamo(this);
        }
        public Boolean Modificar_Solicitud_Prestamo()
        {
            return Cls_Ope_Nom_Prestamos_Datos.Modificar_Solicitud_Prestamo(this);
        }
        public Boolean Eliminar_Solicitud_Prestamo()
        {
            return Cls_Ope_Nom_Prestamos_Datos.Eliminar_Solicitud_Prestamo(this);
        }
        public Boolean Autorizacion_Solicitudes_Prestamos()
        {
            return Cls_Ope_Nom_Prestamos_Datos.Autorizacion_Solicitudes_Prestamos(this);
        }
        public Boolean Cancelacion_Por_Liquidacion_Prestamos()
        {
            return Cls_Ope_Nom_Prestamos_Datos.Cancelacion_Por_Liquidacion_Prestamos(this);
        }
        public Cls_Ope_Nom_Pestamos_Negocio Consulta_Solicitudes_Prestamos()
        {
            return Cls_Ope_Nom_Prestamos_Datos.Consulta_Solicitudes_Prestamos(this);
        }
        public Cls_Ope_Nom_Pestamos_Negocio Consulta_Prestamos()
        {
            return Cls_Ope_Nom_Prestamos_Datos.Consulta_Prestamos(this);
        }
        public DataTable Consultar_Fecha_Inicio_Periodo_Pago()
        {
            return Cls_Ope_Nom_Prestamos_Datos.Consultar_Fecha_Inicio_Periodo_Pago(this);
        }
        public DataTable Consultar_Fechas_Periodo()
        {
            return Cls_Ope_Nom_Prestamos_Datos.Consultar_Fechas_Periodo(this);
        }
        public String Consultar_Tipo_Nomina_Empleado_Solicitante() {
            return Cls_Ope_Nom_Prestamos_Datos.Consultar_Tipo_Nomina_Empleado_Solicitante(this);
        }

        public void Finiquitar_Prestamo() {
            Cls_Ope_Nom_Prestamos_Datos.Finiquitar_Prestamo(this);
        }

        public DataTable Consulta_Reporte_Prestamos() {
            return Cls_Ope_Nom_Prestamos_Datos.Consulta_Reporte_Prestamos(this);
        }
        #endregion
    }
}
