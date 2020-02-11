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
using Presidencia.Compra_Directa.Datos;

namespace Presidencia.Compra_Directa.Negocio
{
    public class Cls_Ope_Com_Compra_Directa_Negocio
    {
        public Cls_Ope_Com_Compra_Directa_Negocio()
        {
        }
        #region VARIABLES INTERNAS

        //estatus de las Requisiciones
        private String Estatus_Requisicion;
        private String Tipo;//tipo articulo
        private String No_Requisicion;
        private String Usuario;
        private double Monto;
        private DataTable Dt_Detalles;
        private String Fecha_Inicial;
        private String Fecha_Final;

        private double Subtotal_Cotizado;
        private double IEPS_Cotizado;
        private double IVA_Cotizado;
        private double Total_Cotizado;

        private String Tipo_Requisicion;
        private String Partida;

        //Estatus de la Orden Compra
        //private String Estatus_Orden_Compra;
        //private String No_Orden_Compra;
        //private String Folio;
        private String Giro_ID;


        #endregion

        #region VARIABLES PUBLICAS
        public double P_Subtotal_Cotizado
        {
            get { return Subtotal_Cotizado; }
            set { Subtotal_Cotizado = value; }
        }
        public double P_IEPS_Cotizado
        {
            get { return IEPS_Cotizado; }
            set { IEPS_Cotizado = value; }
        }
        public double P_IVA_Cotizado
        {
            get { return IVA_Cotizado; }
            set { IVA_Cotizado = value; }
        }
        public double P_Total_Cotizado
        {
            get { return Total_Cotizado; }
            set { Total_Cotizado = value; }
        }

        public String P_Lista_Partidas
        {
            get { return Partida; }
            set { Partida = value; }
        }
        public String P_Giro_ID
        {
            get { return Giro_ID; }
            set { Giro_ID = value; }
        }
        //public String P_Folio
        //{
        //    get { return Folio; }
        //    set { Folio = value; }
        //}
        //public String P_Estatus_Orden_Compra
        //{
        //    get { return Estatus_Orden_Compra; }
        //    set { Estatus_Orden_Compra = value; }
        //}
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
        public DataTable P_Dt_Detalles_Orden_Compra
        {
            get { return Dt_Detalles; }
            set { Dt_Detalles = value; }
        }
        //public double P_Monto_Orden_Compra
        //{
        //    get { return Monto; }
        //    set { Monto = value; }
        //}
        public String P_No_Requisicion
        {
            get { return No_Requisicion; }
            set { No_Requisicion = value; }
        }
        public String P_Tipo_Requisicion
        {
            get { return Tipo_Requisicion; }
            set { Tipo_Requisicion = value; }
        }
        public String P_Estatus_Requisicion
        {
            get { return Estatus_Requisicion; }
            set { Estatus_Requisicion = value; }
        }
        public String P_Tipo_Articulo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }
        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }
        #endregion

        #region MÉTODOS
        public DataTable Consultar_Requisiciones_Filtradas()
        {
            return Cls_Ope_Com_Compra_Directa_Datos.Consultar_Requisiciones_Filtradas(this);
        }
        public DataTable Consultar_Articulos_Requisiciones_Filtradas()
        {
            return Cls_Ope_Com_Compra_Directa_Datos.Consultar_Articulos_Requisiciones_Filtradas(this);
        }
        public DataTable Consultar_Proveedores_Por_Concepto()
        {
            return Cls_Ope_Com_Compra_Directa_Datos.Consultar_Proveedores_Por_Concepto(this);
        }
        public DataTable Consultar_Conceptos_De_Partidas()
        {
            return Cls_Ope_Com_Compra_Directa_Datos.Consultar_Conceptos_De_Partidas(this);
        }
        public int Guardar_Actualizacion_Precios_Proveedor() 
        {
            return Cls_Ope_Com_Compra_Directa_Datos.Guardar_Actualizacion_Precios_Proveedor(this);
        }
        #endregion
    }
}