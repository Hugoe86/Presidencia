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
using Presidencia.Catalogo_Descuentos_Generales.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Descuentos_Generales_Negocio
/// </summary>

namespace Presidencia.Catalogo_Descuentos_Generales.Negocio
{
    public class Cls_Cat_Pre_Descuentos_Generales_Negocio
    {
        #region Variables Internas

        private String Descuentos_Generales_Id;
        private String Tipo_Impuesto;
        private String Estatus;
        private String Vigencia_Desde;
        private String Vigencia_Hasta;
        private Int32 Porcentaje_Descuento;
        private String Motivo;
        private String Comentarios;
        private String filtro;
        #endregion 

        #region Variables Publicas

        public String P_Descuentos_Generales_Id
        {
            get { return Descuentos_Generales_Id; }
            set { Descuentos_Generales_Id = value; }
        }

        public String P_Tipo_Impuesto
        {
            get { return Tipo_Impuesto; }
            set { Tipo_Impuesto = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Vigencia_Desde
        {
            get { return Vigencia_Desde; }
            set { Vigencia_Desde = value; }
        }

        public String P_Vigencia_Hasta
        {
            get { return Vigencia_Hasta; }
            set { Vigencia_Hasta = value; }
        }

        public Int32 P_Porcentaje_Descuento
        {
            get { return Porcentaje_Descuento; }
            set { Porcentaje_Descuento = value; }
        }


        public String P_Motivo
        {
            get { return Motivo; }
            set { Motivo = value; }
        }

        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }

        public String P_Filtro
        {
            get { return filtro; }
            set { filtro = value; }
        }

        #endregion

        #region Metodos

        public void Alta_Descuentos_Generales()
        {
            Cls_Cat_Pre_Descuentos_Generales_Datos.Alta_Descuentos_Generales(this);
        }

        public void Modificar_Descuentos_Generales()
        {
            Cls_Cat_Pre_Descuentos_Generales_Datos.Modificar_Descuentos_Generales(this);
        }

        public Cls_Cat_Pre_Descuentos_Generales_Negocio Consultar_Datos_Descuentos_Generales()
        {
            return Cls_Cat_Pre_Descuentos_Generales_Datos.Consultar_Datos_Descuentos_Generales(this);
        }

        public DataTable Consultar_descuentos_Generales()
        {
            return Cls_Cat_Pre_Descuentos_Generales_Datos.Consultar_Descuentos_Generales(this);
        }

        #endregion

    }
}