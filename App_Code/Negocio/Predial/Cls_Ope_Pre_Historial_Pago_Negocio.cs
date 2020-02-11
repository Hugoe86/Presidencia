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
using Presidencia.Operacion_Predial_Historial_Pagos.Datos ;

namespace Presidencia.Operacion_Predial_Historial_Pagos.Negocio
{
    public class Cls_Ope_Pre_Historial_Pago_Negocio
    {
        public Cls_Ope_Pre_Historial_Pago_Negocio()
        {
        }

        #region Varibles Internas
        private String Cuenta_Predial_ID;
        private String Entre_Fecha;
        private String Y_Fecha;
        private String Recibo_Inicial;
        private String Recibo_Final;
        private String Lugar_Pago;
        private String Caja;
        private DataTable Dt_Detalles_Cuenta;
        private String Usuario_Creo;
        #endregion

        #region Varibles Publicas
        public String P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }
        public DataTable  P_Dt_Detalles_Cuenta
        {
            get { return Dt_Detalles_Cuenta; }
            set { Dt_Detalles_Cuenta = value; }
        }

        public String P_Cuenta_Predial_ID
        {
            get { return Cuenta_Predial_ID; }
            set { Cuenta_Predial_ID = value; }
        }
        public String P_Entre_Fecha
        {
            get { return Entre_Fecha; }
            set { Entre_Fecha = value; }
        }
        public String P_Y_Fecha
        {
            get { return Y_Fecha; }
            set { Y_Fecha = value; }
        }
        public String P_Recibo_Inicial
        {
            get { return Recibo_Inicial; }
            set { Recibo_Inicial = value; }
        }
        public String P_Recibo_Final
        {
            get { return Recibo_Final; }
            set { Recibo_Final = value; }
        }
        public String P_Lugar_Pago
        {
            get { return Lugar_Pago; }
            set { Lugar_Pago = value; }
        }
        public String P_Caja
        {
            get { return Caja; }
            set { Caja = value; }
        }
        #endregion

        #region Metodos

        //public DataTable Consultar_Cuota_Fija_Detalles()
        //{
        //    return Cls_Ope_Pre_Historial_Pagos_Datos.Consultar_Cuota_Fija_Detalles(this);
        //}
        public DataTable Consultar_Detalles_Cuenta_Predial()
        {
            return Cls_Ope_Pre_Historial_Pagos_Datos.Consultar_Detalles_Cuenta_Predial(this);
        }
        public DataTable Consultar_Historial_Pagos()
        {
            return Cls_Ope_Pre_Historial_Pagos_Datos.Consultar_Historial_Pagos(this);
        }
        public DataTable Consultar_Historial_Pagos_Detalles()
        {
            return Cls_Ope_Pre_Historial_Pagos_Datos.Consultar_Historial_Pagos_Detalles(this);
        }
        #endregion
    }
}
