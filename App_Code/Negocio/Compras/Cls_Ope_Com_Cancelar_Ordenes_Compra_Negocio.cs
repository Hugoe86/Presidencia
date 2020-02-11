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
using Presidencia.Cancelar_Ordenes.Datos;

/// <summary>
/// Summary description for Cls_Ope_Com_Cancelar_Ordenes_Compra_Negocio
/// </summary>
/// 
namespace Presidencia.Cancelar_Ordenes.Negocio
{
    public class Cls_Ope_Com_Cancelar_Ordenes_Compra_Negocio
    {
        ///*******************************************************************************
        /// VARIABLES INTERNAS
        ///*******************************************************************************
        #region Variables_Internas
        private String No_Orden_Compra;
        private String No_Requisicio;
        private String Estatus;
        private String Fecha_Inicio;
        private String Fecha_Fin;
        private String Folio_Busqueda;
        private String Monto_Total;
        private String Motivo_Cancelacion;
        private String Listado_Almacen;

      

        

        #endregion


        ///*******************************************************************************
        /// VARIABLES PUBLICAS
        ///*******************************************************************************
        #region Variables_Publicas
        public String P_No_Orden_Compra
        {
            get { return No_Orden_Compra; }
            set { No_Orden_Compra = value; }
        }

        public String P_No_Requisicio
        {
            get { return No_Requisicio; }
            set { No_Requisicio = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Fecha_Inicio
        {
            get { return Fecha_Inicio; }
            set { Fecha_Inicio = value; }
        }

        public String P_Fecha_Fin
        {
            get { return Fecha_Fin; }
            set { Fecha_Fin = value; }
        }

        public String P_Folio_Busqueda
        {
            get { return Folio_Busqueda; }
            set { Folio_Busqueda = value; }
        }

        public String P_Monto_Total
        {
            get { return Monto_Total; }
            set { Monto_Total = value; }
        }


        public String P_Motivo_Cancelacion
        {
            get { return Motivo_Cancelacion; }
            set { Motivo_Cancelacion = value; }
        }

        public String P_Listado_Almacen
        {
            get { return Listado_Almacen; }
            set { Listado_Almacen = value; }
        }
        #endregion

        ///*******************************************************************************
        /// METODOS
        ///*******************************************************************************
        #region Metodos

        public DataTable Consultar_Ordenes_Compra()
        {
            return Cls_Ope_Com_Cancelar_Ordenes_Compra_Datos.Consultar_Ordenes_Compra(this);
        }


        public DataTable Consultar_Productos_Servicios()
        {
            return Cls_Ope_Com_Cancelar_Ordenes_Compra_Datos.Consultar_Productos_Servicios(this);
        }

        public void Liberar_Presupuesto_Cancelacion_Total()
        {
            Cls_Ope_Com_Cancelar_Ordenes_Compra_Datos.Liberar_Presupuesto_Cancelacion_Total(this);
        }

        public String Modificar_Orden_Compra()
        {
            return Cls_Ope_Com_Cancelar_Ordenes_Compra_Datos.Modificar_Orden_Compra(this);
        }

        public String Liberar_Presupuesto_Cancelacion_Parcial()
        {
            return Cls_Ope_Com_Cancelar_Ordenes_Compra_Datos.Liberar_Presupuesto_Cancelacion_Parcial(this);
        }
        #endregion

    }
}//fin del Namespace