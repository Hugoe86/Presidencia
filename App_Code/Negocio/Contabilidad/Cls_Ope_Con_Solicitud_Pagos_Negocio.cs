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
using Presidencia.Solicitud_Pagos.Datos;

namespace Presidencia.Solicitud_Pagos.Negocio
{
    public class Cls_Ope_Con_Solicitud_Pagos_Negocio
    {
        #region(Variables Internas)
            private String No_Solicitud_Pago;
            private Double No_Reserva;
            private Double No_Reserva_Anterior;            
            private String No_Poliza;
            private String Tipo_Poliza_ID;
            private String Mes_Ano;
            private String Tipo_Solicitud_Pago_ID;
            private String Empleado_ID_Jefe_Area;
            private String Empleado_ID_Contabilidad;
            private String Proveedor_ID;            
            private String Dependencia_ID;
            private String Concepto;
            private String Fecha_Solicitud;
            private Double Monto;
            private Double Monto_Anterior;
            private String No_Factura;
            private String Fecha_Factura;
            private String Fecha_Inicial;            
            private String Fecha_Final;            
            private String Estatus;
            private String Beneficiario;
            private String Fecha_Autorizo_Rechazo_Jefe;
            private String Comentarios_Jefe_Area;
            private String Fecha_Autorizo_Rechazo_Contabi;
            private String Comentarios_Contabilidad;
            private String Nombre_Usuario;
            private DataTable Dt_Detalles_Poliza;
        #endregion
        #region(Variables Publicas)
            public String P_No_Solicitud_Pago
            {
                get { return No_Solicitud_Pago; }
                set { No_Solicitud_Pago = value; }
            }
            public Double P_No_Reserva
            {
                get { return No_Reserva; }
                set { No_Reserva = value; }
            }
            public String P_No_Poliza
            {
                get { return No_Poliza; }
                set { No_Poliza = value; }
            }
            public String P_Tipo_Poliza_ID
            {
                get { return Tipo_Poliza_ID; }
                set { Tipo_Poliza_ID = value; }
            }
            public String P_Mes_Ano
            {
                get { return Mes_Ano; }
                set { Mes_Ano = value; }
            }
            public String P_Tipo_Solicitud_Pago_ID
            {
                get { return Tipo_Solicitud_Pago_ID; }
                set { Tipo_Solicitud_Pago_ID = value; }
            }
            public String P_Empleado_ID_Jefe_Area
            {
                get { return Empleado_ID_Jefe_Area; }
                set { Empleado_ID_Jefe_Area = value; }
            }
            public String P_Empleado_ID_Contabilidad
            {
                get { return Empleado_ID_Contabilidad; }
                set { Empleado_ID_Contabilidad = value; }
            }
            public String P_Dependencia_ID
            {
                get { return Dependencia_ID; }
                set { Dependencia_ID = value; }
            }
            public String P_Proveedor_ID
            {
                get { return Proveedor_ID; }
                set { Proveedor_ID = value; }
            }
            public String P_Concepto
            {
                get { return Concepto; }
                set { Concepto = value; }
            }
            public String P_Fecha_Solicitud
            {
                get { return Fecha_Solicitud; }
                set { Fecha_Solicitud = value; }
            }
            public Double P_Monto
            {
                get { return Monto; }
                set { Monto = value; }
            }
            public String P_No_Factura
            {
                get { return No_Factura; }
                set { No_Factura = value; }
            }
            public String P_Fecha_Factura
            {
                get { return Fecha_Factura; }
                set { Fecha_Factura = value; }
            }
            public String P_Fecha_Inicial
            {
                get { return Fecha_Inicial; }
                set { Fecha_Inicial = value; }
            }
            public String P_Fecha_Final
            {
                get { return Fecha_Final; }
                set { Fecha_Final = value; }
            }
            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }
            public String P_Beneficiario
            {
                get { return Beneficiario; }
                set { Beneficiario = value; }
            }
            public String P_Fecha_Autorizo_Rechazo_Jefe
            {
                get { return Fecha_Autorizo_Rechazo_Jefe; }
                set { Fecha_Autorizo_Rechazo_Jefe = value; }
            }
            public String P_Comentarios_Jefe_Area
            {
                get { return Comentarios_Jefe_Area; }
                set { Comentarios_Jefe_Area = value; }
            }
            public String P_Fecha_Autorizo_Rechazo_Contabi
            {
                get { return Fecha_Autorizo_Rechazo_Contabi; }
                set { Fecha_Autorizo_Rechazo_Contabi = value; }
            }
            public String P_Comentarios_Contabilidad
            {
                get { return Comentarios_Contabilidad; }
                set { Comentarios_Contabilidad = value; }
            }
            public String P_Nombre_Usuario
            {
                get { return Nombre_Usuario; }
                set { Nombre_Usuario = value; }
            }
            public Double P_No_Reserva_Anterior
            {
                get { return No_Reserva_Anterior; }
                set { No_Reserva_Anterior = value; }
            }
            public Double P_Monto_Anterior
            {
                get { return Monto_Anterior; }
                set { Monto_Anterior = value; }
            }
            public DataTable P_Dt_Detalles_Poliza
            {
                get { return Dt_Detalles_Poliza; }
                set { Dt_Detalles_Poliza = value; }
            }
        #endregion
        #region(Metodos)
            public void Alta_Solicitud_Pago()
            {
                Cls_Ope_Con_Solicitud_Pagos_Datos.Alta_Solicitud_Pago(this);
            }
            public void Modificar_Solicitud_Pago()
            {
                Cls_Ope_Con_Solicitud_Pagos_Datos.Modificar_Solicitud_Pago(this);
            }
            public void Eliminar_Solicitud_Pago()
            {
                Cls_Ope_Con_Solicitud_Pagos_Datos.Eliminar_Solicitud_Pago(this);
            }
            public DataTable Consultar_Solicitud_Pago()
            {
                return Cls_Ope_Con_Solicitud_Pagos_Datos.Consultar_Solicitud_Pago(this);
            }
            public DataTable Consulta_Reservas()
            {
                return Cls_Ope_Con_Solicitud_Pagos_Datos.Consulta_Reservas(this);
            }
            public DataTable Consulta_Datos_Reserva()
            {
                return Cls_Ope_Con_Solicitud_Pagos_Datos.Consulta_Datos_Reserva(this);
            }
            public DataTable Consulta_Datos_Solicitud_Pago()
            {
                return Cls_Ope_Con_Solicitud_Pagos_Datos.Consulta_Datos_Solicitud_Pago(this);
            }
            public DataTable Consulta_Cuenta_Contable_Proveedor()
            {
                return Cls_Ope_Con_Solicitud_Pagos_Datos.Consulta_Cuenta_Contable_Proveedor(this);
            }
        #endregion
    }
}