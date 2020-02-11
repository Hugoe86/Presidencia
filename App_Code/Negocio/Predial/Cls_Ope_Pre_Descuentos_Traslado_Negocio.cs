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
using System.Collections.Generic;
using Presidencia.Operacion_Descuentos_Traslado.Datos;
using System.Data.OracleClient;

namespace Presidencia.Operacion_Descuentos_Traslado.Negocio
{

    public class Cls_Ope_Pre_Descuentos_Traslado_Negocio
    {

        #region Variables Internas

        private String No_Descuento;
        private String No_Calculo;
        private String Anio_Calculo;
        private String No_Adeudo;
        private String Estatus;
        private DateTime Fecha;
        private String Desc_Multa;
        private String Desc_Recargo;
        private String Total_Por_Pagar;
        private String Realizo;
        private DateTime Fecha_Vencimiento;
        private String Observaciones;
        private String Fundamento_Legal;
        private String Usuario;
        private String Monto_Multa;
        private String Monto_Recargo;
        private String Desc_Monto_Recargos;
        private String Desc_Monto_Multas;
        private String Cuenta_Predial;
        private String Cuenta_Predial_ID;
        private String No_Contrarecibo;
        private String Monto_Traslado;
        private String Monto_Division;
        private String Costo_Constancia;
        private String Total_Impuesto;
        private String Referencia;
        private String Folio;
        private OracleCommand _Comando_Transaccion;

        #endregion

        #region Variables Publicas

        public String P_Cuenta_Predial_ID
        {
            get { return Cuenta_Predial_ID; }
            set { Cuenta_Predial_ID = value; }
        }

        public String P_Folio
        {
            get { return Folio; }
            set { Folio = value; }
        }
        public String P_Referencia
        {
            get { return Referencia; }
            set { Referencia = value; }
        }

        public String P_Total_Impuesto
        {
            get { return Total_Impuesto; }
            set { Total_Impuesto = value; }
        }

        public String P_Monto_Traslado
        {
            get { return Monto_Traslado; }
            set { Monto_Traslado = value; }
        }

        public String P_Monto_Division
        {
            get { return Monto_Division; }
            set { Monto_Division = value; }
        }

        public String P_Costo_Constancia
        {
            get { return Costo_Constancia; }
            set { Costo_Constancia = value; }
        }

        public String P_Cuenta_Predial
        {
            get { return Cuenta_Predial; }
            set { Cuenta_Predial = value; }
        }

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

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public DateTime P_Fecha
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

        public String P_Desc_Monto_Multas
        {
            get { return Desc_Monto_Multas; }
            set { Desc_Monto_Multas = value; }
        }

        public String P_Desc_Monto_Recargos
        {
            get { return Desc_Monto_Recargos; }
            set { Desc_Monto_Recargos = value; }
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

        public DateTime P_Fecha_Vencimiento
        {
            get { return Fecha_Vencimiento; }
            set { Fecha_Vencimiento = value; }
        }

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

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }
        
        public OracleCommand P_Comando_Transaccion
        {
            get { return _Comando_Transaccion; }
            set { _Comando_Transaccion = value; }
        }

        #endregion

        #region Metodos

        public void Alta_Descuentos_Traslado()
        {
            Cls_Ope_Pre_Descuentos_Traslado_Datos.Alta_Descuentos_Traslado(this);
        }

        public void Modificar_Descuentos_Traslado()
        {
            Cls_Ope_Pre_Descuentos_Traslado_Datos.Modificar_Descuentos_Traslado(this);
        }

        public String Traer_Descuento()
        {
            return Cls_Ope_Pre_Descuentos_Traslado_Datos.Consultar_Descuento(this);
        }

        public DataTable Consultar_Descuentos_Traslado()
        {
            return Cls_Ope_Pre_Descuentos_Traslado_Datos.Consultar_Descuentos_Traslado(this);
        }

        public DataTable Consultar_Descuentos_Traslado_Busqueda() //Busqueda
        {
            return Cls_Ope_Pre_Descuentos_Traslado_Datos.Consultar_Descuentos_Traslado_Busqueda(this);
        }

        public String Ultimo_Numero_Descuento()
        {
            return Cls_Ope_Pre_Descuentos_Traslado_Datos.Obtener_Clave_Maxima();
        }
        public DataTable Consultar_Convenios_Descuentos_Traslado()
        {
            return Cls_Ope_Pre_Descuentos_Traslado_Datos.Consultar_Convenios_Descuentos_Traslado(this);
        }

        #endregion

    }
}