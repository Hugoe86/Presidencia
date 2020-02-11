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
using Presidencia.Presupuesto_Cierre_Presupuestal.Datos;

namespace Presidencia.Presupuesto_Cierre_Presupuestal.Negocio
{

    public class Cls_Ope_Psp_Cierre_Presupuestal_Negocio
    {
        
        #region(Variables Privadas)
            private String Anio;
            private String Enero;
            private String Febrero;
            private String Marzo;
            private String Abril;
            private String Mayo;
            private String Junio;
            private String Julio;
            private String Agosto;
            private String Septiembre;
            private String Octubre;
            private String Noviembre;
            private String Diciembre;
            private String Usuario_Creo;
        #endregion

        #region(Variables Publicas)
            public String P_Anio
            {
                get { return Anio; }
                set { Anio = value; }
            }
            public String P_Enero
            {
                get { return Enero; }
                set { Enero = value; }
            }
            public String P_Febrero
            {
                get { return Febrero; }
                set { Febrero = value; }
            }
            public String P_Marzo
            {
                get { return Marzo; }
                set { Marzo = value; }
            }
            public String P_Abril
            {
                get { return Abril; }
                set { Abril = value; }
            }
            public String P_Mayo
            {
                get { return Mayo; }
                set { Mayo = value; }
            }
            public String P_Junio
            {
                get { return Junio; }
                set { Junio = value; }
            }
            public String P_Julio
            {
                get { return Julio; }
                set { Julio = value; }
            }
            public String P_Agosto
            {
                get { return Agosto; }
                set { Agosto = value; }
            }
            public String P_Septiembre
            {
                get { return Septiembre; }
                set { Septiembre = value; }
            }
            public String P_Octubre
            {
                get { return Octubre; }
                set { Octubre = value; }
            }
            public String P_Noviembre
            {
                get { return Noviembre; }
                set { Noviembre = value; }
            }
            public String P_Diciembre
            {
                get { return Diciembre; }
                set { Diciembre = value; }
            }
            public String P_Usuario_Creo
            {
                get { return Usuario_Creo; }
                set { Usuario_Creo = value; }
            }
        #endregion

        #region(Metodos)
            public Boolean Modificar_Cierre_Presupuestal()
            {
                return Cls_Ope_Psp_Cierre_Presupuestal_Datos.Modificar_Cierre_Presupuestal(this);
            }
            public DataTable Consultar_Estatus()
            {
                return Cls_Ope_Psp_Cierre_Presupuestal_Datos.Consultar_Estatus(this);
            }
        #endregion

    }
}