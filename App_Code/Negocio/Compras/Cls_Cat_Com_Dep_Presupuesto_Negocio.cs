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
using Presidencia.Catalogo_Compras_Presupuesto_Dependencias.Datos;

namespace Presidencia.Catalogo_Compras_Presupuesto_Dependencias.Negocio
{
    
    public class Cls_Cat_Com_Presupuesto_Dependencias_Negocio
    {

        #region Variables Locales
            private int Presupuesto_ID;        
            private String Dependencia_ID;
            private int Anio_Presupuesto;
            private Double Monto_Comprometido;
            private Double Monto_Presupuestal;
            private Double Monto_Disponible;
            private String Comentarios;
            private String Usuario_Creo;
        #endregion

        #region Variables Publicas
            public int P_Presupuesto_ID
            {
                get { return Presupuesto_ID; }
                set { Presupuesto_ID = value; }
            }

            public String P_Dependencia_ID
            {
                get { return Dependencia_ID; }
                set { Dependencia_ID = value; }
            }

            public int P_Anio_Presupuesto
            {
                get { return Anio_Presupuesto; }
                set { Anio_Presupuesto = value; }
            }

            public Double P_Monto_Comprometido
            {
                get { return Monto_Comprometido; }
                set { Monto_Comprometido = value; }
            }

            public Double P_Monto_Presupuestal
            {
                get { return Monto_Presupuestal; }
                set { Monto_Presupuestal = value; }
            }

            public Double P_Monto_Disponible
            {
                get { return Monto_Disponible; }
                set { Monto_Disponible = value; }
            }

            public String P_Comentarios
            {
                get { return Comentarios; }
                set { Comentarios = value; }
            }
            
            public String P_Usuario_Creo
            {
                get { return Usuario_Creo; }
                set { Usuario_Creo = value; }
            }
        #endregion

        #region Metodos
            
            public void Alta_Presupuesto()
            {                
                Cls_Cat_Com_Presupuesto_Dependencias_Datos.Alta_Presupuesto(this);
            }

           
            public void Baja_Presupuesto()
            {
                Cls_Cat_Com_Presupuesto_Dependencias_Datos.Baja_Presupuesto(this);
            }

            
            public void Cambio_Presupuesto()
            {
                Cls_Cat_Com_Presupuesto_Dependencias_Datos.Cambio_Presupuesto(this);
            }


            public DataTable Consulta_Presupuesto()
            {
                return Cls_Cat_Com_Presupuesto_Dependencias_Datos.Consulta_Presupuestos(this);
            }
        #endregion
    }
}