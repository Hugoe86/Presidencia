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
using Presidencia.Catalogo_Descuentos_Predial.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Descuentos_Predial_Negocio
/// </summary>

namespace Presidencia.Catalogo_Descuentos_Predial.Negocio
{
    public class Cls_Cat_Pre_Descuentos_Predial_Negocio
    {

        #region Variables Internas

        private String Descuento_ID;
        private Int32 Anio;
        private Double Enero;
        private Double Febrero;
        private Double Marzo;
        private Double Abril;
        private Double Mayo;
        private Double Junio;
        private Double Julio;
        private Double Agosto;
        private Double Septiembre;
        private Double Octubre;
        private Double Noviembre;
        private Double Diciembre;
        private String Usuario;
        private String Mes;

        #endregion

        #region Variables Publicas

        public String P_Descuento_ID
        {
            get { return Descuento_ID; }
            set { Descuento_ID = value; }
        }

        public Int32 P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }

        public Double P_Enero
        {
            get { return Enero; }
            set { Enero = value; }
        }

        public Double P_Febrero
        {
            get { return Febrero; }
            set { Febrero = value; }
        }

        public Double P_Marzo
        {
            get { return Marzo; }
            set { Marzo = value; }
        }

        public Double P_Abril
        {
            get { return Abril; }
            set { Abril = value; }
        }

        public Double P_Mayo
        {
            get { return Mayo; }
            set { Mayo = value; }
        }

        public Double P_Junio
        {
            get { return Junio; }
            set { Junio = value; }
        }

        public Double P_Julio
        {
            get { return Julio; }
            set { Julio = value; }
        }

        public Double P_Agosto
        {
            get { return Agosto; }
            set { Agosto = value; }
        }

        public Double P_Septiembre
        {
            get { return Septiembre; }
            set { Septiembre = value; }
        }

        public Double P_Octubre
        {
            get { return Octubre; }
            set { Octubre = value; }
        }

        public Double P_Noviembre
        {
            get { return Noviembre; }
            set { Noviembre = value; }
        }

        public Double P_Diciembre
        {
            get { return Diciembre; }
            set { Diciembre = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public String P_Mes
        {
            get { return Mes; }
            set { Mes = value; }
        }

        #endregion

        #region Metodos

        public void Alta_Descuento_Predial()
        {
            Cls_Cat_Pre_Descuentos_Predial_Datos.Alta_Descuento_Predial(this);
        }

        public void Modificar_Descuento_Predial()
        {
            Cls_Cat_Pre_Descuentos_Predial_Datos.Modificar_Descuento_Predial(this);
        }

        public void Eliminar_Descuento_Predial()
        {
            Cls_Cat_Pre_Descuentos_Predial_Datos.Eliminar_Descuento_Predial(this);
        }

        public Cls_Cat_Pre_Descuentos_Predial_Negocio Consultar_Datos_Descuento_Predial()
        {
            return Cls_Cat_Pre_Descuentos_Predial_Datos.Consultar_Datos_Descuento_Predial(this);
        }

        public DataTable Consultar_Descuentos_Predial()
        {
            return Cls_Cat_Pre_Descuentos_Predial_Datos.Consultar_Descuentos_Predial(this);
        }

        public Double Consultar_Descuento_Mes()
        {
            return Cls_Cat_Pre_Descuentos_Predial_Datos.Consultar_Descuento_Mes(this);
        }

        public Boolean Validar_Existe()
        {
            return Cls_Cat_Pre_Descuentos_Predial_Datos.Validar_Existe(this);
        }

        #endregion
    }
}