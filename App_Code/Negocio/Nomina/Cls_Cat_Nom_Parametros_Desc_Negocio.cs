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
using Presidencia.Parametros_Descuentos.Datos;

namespace Presidencia.Parametros_Descuentos.Negocio
{
    public class Cls_Cat_Nom_Parametros_Desc_Negocio
    {
        #region (Variables Privadas)
        private String Parametro_ID;
        private String Desc_PMO_Mercados;
        private String Desc_PMO_Tesoreria;
        private String Desc_PMO_Corto_Plazo;
        private String Desc_PMO_Pago_Aval;
        private String Desc_PMO_IMUVI;
        private String Desc_Llamadas_Tel;
        private String Desc_Perdida_Equipo;
        private String Desc_Otros_Fijos;
        private String Desc_Otros_Variables;
        private String Desc_Agua;
        private String Desc_Pago_Predial;
        #endregion

        #region (Variables Públicas)
        public String P_Desc_PMO_Mercados {
            get { return Desc_PMO_Mercados; }
            set { Desc_PMO_Mercados = value; }
        }

        public String P_PMO_Tesoreria {
            get { return Desc_PMO_Tesoreria; }
            set { Desc_PMO_Tesoreria = value; }
        }

        public String P_PMO_Corto_Plazo {
            get { return Desc_PMO_Corto_Plazo; }
            set { Desc_PMO_Corto_Plazo = value; }
        }

        public String P_PMO_Pago_Aval {
            get { return Desc_PMO_Pago_Aval; }
            set { Desc_PMO_Pago_Aval = value; }
        }

        public String P_PMO_IMUVI {
            get { return Desc_PMO_IMUVI; }
            set { Desc_PMO_IMUVI = value; }
        }

        public String P_Desc_Llamadas_Tel {
            get { return Desc_Llamadas_Tel; }
            set { Desc_Llamadas_Tel = value; }
        }

        public String P_Desc_Perdida_Equipo {
            get { return Desc_Perdida_Equipo; }
            set { Desc_Perdida_Equipo = value; }
        }

        public String P_Desc_Otros_Fijos {
            get { return Desc_Otros_Fijos; }
            set { Desc_Otros_Fijos = value; }
        }

        public String P_Desc_Otros_Variables {
            get { return Desc_Otros_Variables; }
            set { Desc_Otros_Variables = value; }
        }

        public String P_Desc_Agua {
            get { return Desc_Agua; }
            set { Desc_Agua = value; }
        }

        public String P_Desc_Pago_Predial {
            get { return Desc_Pago_Predial; }
            set { Desc_Pago_Predial = value; }
        }

        public String P_Parametro_ID {
            get { return Parametro_ID; }
            set { Parametro_ID = value; }
        }
        #endregion

        #region (Métodos)
        public Boolean Alta() { return Cls_Cat_Nom_Parametros_Desc_Datos.Alta(this); }
        public Boolean Modificar() { return Cls_Cat_Nom_Parametros_Desc_Datos.Modificar(this); ; }
        public Boolean Eliminar() { return Cls_Cat_Nom_Parametros_Desc_Datos.Eliminar(this); ; }
        public DataTable Consultar() { return Cls_Cat_Nom_Parametros_Desc_Datos.Consultar_Parametro(); ; }
        #endregion
    }
}