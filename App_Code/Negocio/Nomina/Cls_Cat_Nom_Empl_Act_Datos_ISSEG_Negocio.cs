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
using Presidencia.Actualizar_Datos_ISSEG.Datos;

namespace Presidencia.Actualizar_Datos_ISSEG.Negocio
{
    public class Cls_Cat_Nom_Empl_Act_Datos_ISSEG_Negocio
    {
        #region (Variables Privadas)
        private String Empleado_ID;
        private String No_Empleado;
        private String Aplica_ISSEG;
        private String Fecha_Alta_ISSEG;
        #endregion

        #region (Variables Públicas)
        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }

        public String P_No_Empleado
        {
            get { return No_Empleado; }
            set { No_Empleado = value; }
        }


        public String P_Aplica_ISSEG
        {
            get { return Aplica_ISSEG; }
            set { Aplica_ISSEG = value; }
        }

        public String P_Fecha_Alta_ISSEG
        {
            get { return Fecha_Alta_ISSEG; }
            set { Fecha_Alta_ISSEG = value; }
        }
        #endregion

        #region (Métodos)
        public Boolean Actualizar_Datos_ISSEG() { return Cls_Cat_Nom_Empl_Act_Datos_ISSEG_Datos.Actualizar_Datos_ISSEG(this); }
        #endregion
    }
}
