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
using Presidencia.ISR.Datos;

/// <summary>
/// Summary description for Cls_Tab_Nom_ISR_Negocio
/// </summary>
namespace Presidencia.ISR.Negocios
{
    public class Cls_Tab_Nom_ISR_Negocio
    {
        public Cls_Tab_Nom_ISR_Negocio()
        {
        }
        #region (Variables Internas)
            //Propiedades
            private String ISR_ID;
            private double Limite_Inferior;
            private double Couta_Fija;
            private double Porcentaje;
            private String Tipo_Nomina;
            private String Comentarios;
            private String Nombre_Usuario;
        #endregion
        #region (Variables Publicas)
            public String P_ISR_ID
            {
                get { return ISR_ID; }
                set { ISR_ID = value; }
            }

            public double P_Limite_Inferior
            {
                get { return Limite_Inferior; }
                set { Limite_Inferior = value; }
            }

            public double P_Couta_Fija
            {
                get { return Couta_Fija; }
                set { Couta_Fija = value; }
            }

            public double P_Porcentaje
            {
                get { return Porcentaje; }
                set { Porcentaje = value; }
            }

            public String P_Tipo_Nomina
            {
                get { return Tipo_Nomina; }
                set { Tipo_Nomina = value; }
            }

            public String P_Comentarios
            {
                get { return Comentarios; }
                set { Comentarios = value; }
            }

            public String P_Nombre_Usuario
            {
                get { return Nombre_Usuario; }
                set { Nombre_Usuario = value; }
            }
        #endregion
        #region (Metodos)
            public void Alta_ISR()
            {
                Cls_Tab_Nom_ISR_Datos.Alta_ISR(this);
            }
            public void Modificar_ISR()
            {
                Cls_Tab_Nom_ISR_Datos.Modificar_ISR(this);
            }
            public void Eliminar_ISR()
            {
                Cls_Tab_Nom_ISR_Datos.Eliminar_ISR(this);
            }
            public DataTable Consulta_Datos_ISR()
            {
                return Cls_Tab_Nom_ISR_Datos.Consulta_Datos_ISR(this);
            }
        #endregion
    }
}