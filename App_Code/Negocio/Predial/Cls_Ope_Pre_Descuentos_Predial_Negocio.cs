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
using Presidencia.Operacion_Predial_Recepcion_Pagos.Datos;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Catalogo_Tabulador_Recargos.Negocio;
using System.Collections.Generic;
using Presidencia.Descuentos_Predial.Datos;
using Presidencia.Descuentos_Predial.Negocio;
using Presidencia.Operacion_Predial_Recepcion_Pagos.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;



namespace Presidencia.Descuentos_Predial.Negocio
{
    public class Cls_Ope_Pre_Descuentos_Predial_Negocio
    {

        #region "Variables Internas"

        private String Cuenta_Predial = null;
        private String Cuenta_Predial_ID = "";
        private String Clave_Ingreso = null;
        private String Descripcion = null;
        private DateTime Fecha_Tramite;
        private DateTime Fecha_Vencimiento;
        private Double Monto = 0.0;
        private String Estatus = null;
        private String No_Recibo = null;
        private String Dependencia = null;
        private String Usuario = null;
        private Int32 Anio_Filtro = 0;
        private Int32 Bimestre_Filtro = 0;
        private String Filtros_Dinamicos;
        private String _Tipo_Descuento = "";

        //para descuentos
        private String No_Descuento;
        private String No_Calculo;
        private String Anio_Calculo;
        private String No_Adeudo;
        //private String Estatus;
        private String Fecha;
        private String Desc_Multa;
        private String Desc_Recargo;
        private String Total_Por_Pagar;
        private String Realizo;
        //private String Fecha_Vencimiento;
        private String Observaciones;
        private String Fundamento_Legal;
        //private String Usuario;
        private String Monto_Multa;
        private String Monto_Recargo;
        //private String Cuenta_Predial;
        private String No_Contrarecibo;
        private String Hasta_Bimestre;
        private String Hasta_Anio;
        private String Desde_Bimestre;
        private String Desde_Anio;
        private String Anio_Inicial;
        private String Campos_Dinamicos;
        private String Bimestre_Inicial;
        private String Total_Impuesto;
        private String Desc_Moratorio;
        private String Fecha_Vencimiento1;
        private String Descuento_Pronto_Pago;
        private String Porcentaje_Pronto_Pago;
        private String Porcentaje_Multa;
        private String Porcentaje_Recargo;
        private String Porcentaje_Recargo_Moratorio;
        private String Recargos;
        private String Recargos_Moratorios;
        private String Honorarios;
        private String Rezagos;
        private String Corriente;
        private String Multas;
        private Boolean Campos_Foraneos;
        private String Contribuyente;
        private String Contribuyente_ID;
        #endregion

        #region "Variables Publicas"
        public String P_Contribuyente
        {
            get { return Contribuyente; }
            set { Contribuyente = value; }
        }
        public String P_Contribuyente_ID
        {
            get { return Contribuyente_ID; }
            set { Contribuyente_ID = value; }
        }
        public Boolean P_Campos_Foraneos
        {
            get { return Campos_Foraneos; }
            set { Campos_Foraneos = value; }
        }
        public String P_Multas
        {
            get { return Multas; }
            set { Multas = value; }
        }
        public String P_Corriente
        {
            get { return Corriente; }
            set { Corriente = value; }
        }
        public String P_Rezagos
        {
            get { return Rezagos; }
            set { Rezagos = value; }
        }
        public String P_Descuento_Pronto_Pago
        {
            get { return Descuento_Pronto_Pago; }
            set { Descuento_Pronto_Pago = value; }
        }
        public String P_Porcentaje_Pronto_Pago
        {
            get { return Porcentaje_Pronto_Pago; }
            set { Porcentaje_Pronto_Pago = value; }
        }
        public String P_Recargos_Moratorios
        {
            get { return Recargos_Moratorios; }
            set { Recargos_Moratorios = value; }
        }
        public String P_Honorarios
        {
            get { return Honorarios; }
            set { Honorarios = value; }
        }
        public String P_Recargos
        {
            get { return Recargos; }
            set { Recargos = value; }
        }
        public String P_Porcentaje_Recargo_Moratorio
        {
            get { return Porcentaje_Recargo_Moratorio; }
            set { Porcentaje_Recargo_Moratorio = value; }
        }
        public String P_Porcentaje_Recargo
        {
            get { return Porcentaje_Recargo; }
            set { Porcentaje_Recargo = value; }
        }
        public String P_Porcentaje_Multa
        {
            get { return Porcentaje_Multa; }
            set { Porcentaje_Multa = value; }
        }
        public String P_Campos_Dinamicos
        {
            get { return Campos_Dinamicos; }
            set { Campos_Dinamicos = value; }
        }
        public String P_Filtros_Dinamicos
        {
            get { return Filtros_Dinamicos; }
            set { Filtros_Dinamicos = value; }
        }
        public String P_Tipo_Descuento
        {
            get { return _Tipo_Descuento; }
            set { _Tipo_Descuento = value; }
        }

        public String P_Fecha_Vencimiento1
        {
            get { return Fecha_Vencimiento1; }
            set { Fecha_Vencimiento1 = value; }
        }
        public String P_Desc_Moratorio
        {
            get { return Desc_Moratorio; }
            set { Desc_Moratorio = value; }
        }
        public String P_Total_Impuesto
        {
            get { return Total_Impuesto; }
            set { Total_Impuesto = value; }
        }

        public String P_Desde_Anio
        {
            get { return Desde_Anio; }
            set { Desde_Anio = value; }
        }
        public String P_Desde_Bimestre
        {
            get { return Desde_Bimestre; }
            set { Desde_Bimestre = value; }
        }
        public String P_Hasta_Anio
        {
            get { return Hasta_Anio; }
            set { Hasta_Anio = value; }
        }
        public String P_Hasta_Bimestre
        {
            get { return Hasta_Bimestre; }
            set { Hasta_Bimestre = value; }
        }
        public String P_Cuenta_Predial
        {
            get { return Cuenta_Predial; }
            set { Cuenta_Predial = value; }
        }
        public String P_Cuenta_Predial_ID
        {
            get { return Cuenta_Predial_ID; }
            set { Cuenta_Predial_ID = value; }
        }


        public String P_Clave_Ingreso
        {
            get { return Clave_Ingreso; }
            set { Clave_Ingreso = value; }
        }

        public String P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }

        public DateTime P_Fecha_Tramite
        {
            get { return Fecha_Tramite; }
            set { Fecha_Tramite = value; }
        }
        public DateTime P_Fecha_Vencimiento
        {
            get { return Fecha_Vencimiento; }
            set { Fecha_Vencimiento = value; }
        }
        public Double P_Monto
        {
            get { return Monto; }
            set { Monto = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_No_Recibo
        {
            get { return No_Recibo; }
            set { No_Recibo = value; }
        }
        public String P_Dependencia
        {
            get { return Dependencia; }
            set { Dependencia = value; }
        }
        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }
        public Int32 P_Anio_Filtro
        {
            get { return Anio_Filtro; }
            set { Anio_Filtro = value; }
        }
        public Int32 P_Bimestre_Filtro
        {
            get { return Bimestre_Filtro; }
            set { Bimestre_Filtro = value; }
        }
        #endregion

        //para descuentos
        #region Variables Publicas
        public String P_Bimestre_Inicial
        {
            get { return Bimestre_Inicial; }
            set { Bimestre_Inicial = value; }
        }
        public String P_Anio_Inicial
        {
            get { return Anio_Inicial; }
            set { Anio_Inicial = value; }
        }
        //public String P_Cuenta_Predial
        //{
        //    get { return Cuenta_Predial; }
        //    set { Cuenta_Predial = value; }
        //}

        public String P_No_Contrarecibo
        {
            get { return No_Contrarecibo; }
            set { No_Contrarecibo = value; }
        }

        public String P_No_Descuento
        {
            get { return No_Descuento; }
            set { No_Descuento = value; }
        }

        public String P_No_Calculo
        {
            get { return No_Calculo; }
            set { No_Calculo = value; }
        }

        public String P_Anio_Calculo
        {
            get { return Anio_Calculo; }
            set { Anio_Calculo = value; }
        }

        public String P_No_Adeudo
        {
            get { return No_Adeudo; }
            set { No_Adeudo = value; }
        }

        //public String P_Estatus
        //{
        //    get { return Estatus; }
        //    set { Estatus = value; }
        //}

        public String P_Fecha
        {
            get { return Fecha; }
            set { Fecha = value; }
        }

        public String P_Desc_Multa
        {
            get { return Desc_Multa; }
            set { Desc_Multa = value; }
        }

        public String P_Desc_Recargo
        {
            get { return Desc_Recargo; }
            set { Desc_Recargo = value; }
        }

        public String P_Total_Por_Pagar
        {
            get { return Total_Por_Pagar; }
            set { Total_Por_Pagar = value; }
        }

        public String P_Realizo
        {
            get { return Realizo; }
            set { Realizo = value; }
        }

        //public String P_Fecha_Vencimiento
        //{
        //    get { return Fecha_Vencimiento; }
        //    set { Fecha_Vencimiento = value; }
        //}

        public String P_Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
        }

        public String P_Fundamento_Legal
        {
            get { return Fundamento_Legal; }
            set { Fundamento_Legal = value; }
        }

        public String P_Monto_Multa
        {
            get { return Monto_Multa; }
            set { Monto_Multa = value; }
        }

        public String P_Monto_Recargo
        {
            get { return Monto_Recargo; }
            set { Monto_Recargo = value; }
        }

        //public String P_Usuario
        //{
        //    get { return Usuario; }
        //    set { Usuario = value; }
        //}

        #endregion

        #region "Metodos"


        public DataTable Consultar_Clave_Ingreso()
        {
            return Cls_Ope_Pre_Descuentos_Predial_Datos.Consultar_Clave_Ingreso(this);
        }

        public DataTable Consultar_Dependencia()
        {
            return Cls_Ope_Pre_Descuentos_Predial_Datos.Consultar_Dependencia(this);
        }

        public void Alta_Pasivo()
        {
            Cls_Ope_Pre_Descuentos_Predial_Datos.Alta_Pasivo(this);
        }

        public bool Alta_Descuentos_Predial()
        {
            return Cls_Ope_Pre_Descuentos_Predial_Datos.Alta_Descuentos_Predial(this);
        }

        public bool Modificar_Descuento_Predial()
        {
            return Cls_Ope_Pre_Descuentos_Predial_Datos.Modificar_Descuento_Predial(this);
        }

        public DataTable Consultar_Descuentos_Predial()
        {
            return Cls_Ope_Pre_Descuentos_Predial_Datos.Consultar_Descuentos_Predial(this);
        }
        public DataTable Consultar_Descuentos_Prediales()
        {
            return Cls_Ope_Pre_Descuentos_Predial_Datos.Consultar_Descuentos_Prediales(this);
        }
        //public DataTable Consultar_Convenios_Descuentos()
        //{
        //    return Cls_Ope_Pre_Descuentos_Predial_Datos.Consultar_Convenios_Descuentos(this);
        //}
        public DataTable Consultar_Descuentos_Predial_Busqueda()
        {
            return Cls_Ope_Pre_Descuentos_Predial_Datos.Consultar_Descuentos_Predial_Busqueda(this);
        }
        public String Traer_Descuento()
        {
            return Cls_Ope_Pre_Descuentos_Predial_Datos.Consultar_Descuento_Rango(this);
        }
        #endregion

    }

}

