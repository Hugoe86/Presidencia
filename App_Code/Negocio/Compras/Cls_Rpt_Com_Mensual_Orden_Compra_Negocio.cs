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
using Presidencia.Reporte_Mensual_Ordenes_Compra.Datos;

namespace Presidencia.Reporte_Mensual_Ordenes_Compra.Negocio
{
    public class Cls_Rpt_Com_Mensual_Orden_Compra_Negocio
    {
        #region VARIABLES LOCALES
            
            private String Fecha_Inicio;
            private String Fecha_Fin;
            private String Estatus;

        #endregion

        #region VARIABLES GLOBALES

            // set y get de P_Fecha_Inicio
            public String P_Fecha_Inicio
            {
                get { return Fecha_Inicio; }
                set { Fecha_Inicio = value; }
            }

            // set y get de P_Fecha_Fin
            public String P_Fecha_Fin
            {
                get { return Fecha_Fin; }
                set { Fecha_Fin = value; }
            }

            // set y get de P_Estatus
            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }

        #endregion

        #region METODOS

            ///*****************************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Ordenes_Compra
            ///DESCRIPCIÓN          : Metodo para obtener los datos de las ordenes de compras
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 28/Diciembre/2011
            ///*****************************************************************************************************************
            public DataTable Consultar_Ordenes_Compra()
            {
                return Cls_Rpt_Com_Mensual_Orden_Compra_Datos.Consultar_Ordenes_Compra(this);
            }

            ///*****************************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Estatus
            ///DESCRIPCIÓN          : Metodo para obtener los datos del estatus de las ordenes de compras
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 28/Diciembre/2011
            ///*****************************************************************************************************************
            public DataTable Consultar_Estatus()
            {
                return Cls_Rpt_Com_Mensual_Orden_Compra_Datos.Consultar_Estatus();
            }

        #endregion
    }
}