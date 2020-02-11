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
using Presidencia;
using Presidencia.Ope_Pre_Impresion_Recibo_Datos;

namespace Presidencia.Ope_Pre_Impresion_Recibo_Negocio
{
    public class Cls_Ope_Pre_Impresion_Recibo_Negocio
    {
        #region Variables Internas
        private String No_Calculo;
        private String Anio_Calculo;
        private String Referencia;
        private String Tipo_Referencia;
        private String No_Pago;
        #endregion

        #region Variables Publicas
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
        public String P_Referencia
        {
            get { return Referencia; }
            set { Referencia = value; }
        }
        public String P_Tipo_Referencia
        {
            get { return Tipo_Referencia; }
            set { Tipo_Referencia = value; }
        }
        public String P_No_Pago
        {
            get { return No_Pago; }
            set { No_Pago = value; }
        }
        #endregion

        #region Metodos
        public DataTable Consultar_Datos_Recibos()
        {
            return Cls_Ope_Pre_Impresion_Recibo_Datos.Consultar_Datos_Recibos(this);
        }

        public DataTable Consultar_Datos_Cuenta_Predial()
        {
            return Cls_Ope_Pre_Impresion_Recibo_Datos.Consultar_Datos_Cuenta_Predial(this);
        }

        public DataTable Consultar_Datos_Constancias()
        {
            return Cls_Ope_Pre_Impresion_Recibo_Datos.Consultar_Datos_Recibos_Constancias(this);
        }

        public DataTable Consultar_Datos_Otros_Pagos()
        {
            return Cls_Ope_Pre_Impresion_Recibo_Datos.Consultar_Datos_Recibos_Otros_Pagos(this);
        }

        public DataTable Consultar_Datos_Recibos_Ingresos()
        {
            return Cls_Ope_Pre_Impresion_Recibo_Datos.Consultar_Datos_Recibos_Ingresos(this);
        }

        public DataTable Consultar_Datos_Recibos_Tramites()
        {
            return Cls_Ope_Pre_Impresion_Recibo_Datos.Consultar_Datos_Recibos_Tramites(this);
        }

        public DataTable Consultar_Datos_Cancelado()
        {
            return Cls_Ope_Pre_Impresion_Recibo_Datos.Consultar_Datos_Recibos_Cancelado(this);
        }

        public DataTable Consultar_Datos_Recibos_Impuestos()
        {
            return Cls_Ope_Pre_Impresion_Recibo_Datos.Consultar_Datos_Recibos_Impuestos(this);
        }
        #endregion
    }
}