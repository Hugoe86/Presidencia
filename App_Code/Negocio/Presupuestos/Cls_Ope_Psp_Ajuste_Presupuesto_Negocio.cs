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
using Presidencia.Ajuste_Presupuesto.Datos;

namespace Presidencia.Ajuste_Presupuesto.Negocio
{
    public class Cls_Ope_Psp_Ajuste_Presupuesto_Negocio
    {

        #region(Variables Privadas)
            private String Unidad_Responsable_Id;
            private String Fuente_Financiamiento_Id;
            private String Programa_Id;
            private String Partida_Id;
            private String Descripcion;
            private String Monto_Ampliacion;
            private String Monto_Reduccion;
            private String Monto_Incremento;
            private String Monto_Disponible;
            private String Usuario_Creo;
        #endregion

        #region(Variables Publicas)
            public String P_Unidad_Responsable_Id
            {
                get { return Unidad_Responsable_Id; }
                set { Unidad_Responsable_Id = value; }
            }
            public String P_Fuente_Financiamiento_Id
            {
                get { return Fuente_Financiamiento_Id; }
                set { Fuente_Financiamiento_Id = value; }
            }
            public String P_Programa_Id
            {
                get { return Programa_Id; }
                set { Programa_Id = value; }
            }
            public String P_Partida_Id
            {
                get { return Partida_Id; }
                set { Partida_Id = value; }
            }
            public String P_Descripcion
            {
                get { return Descripcion; }
                set { Descripcion = value; }
            }
            public String P_Monto_Ampliacion
            {
                get { return Monto_Ampliacion; }
                set { Monto_Ampliacion = value; }
            }
            public String P_Monto_Reduccion
            {
                get { return Monto_Reduccion; }
                set { Monto_Reduccion = value; }
            }
            public String P_Monto_Incremento
            {
                get { return Monto_Incremento; }
                set { Monto_Incremento = value; }
            }
            public String P_Monto_Disponible
            {
                get { return Monto_Disponible; }
                set { Monto_Disponible = value; }
            }
            public String P_Usuario_Creo
            {
                get { return Usuario_Creo; }
                set { Usuario_Creo = value; }
            }
        #endregion
       

        #region(Metodos)
            public Boolean Modificar_Presupuesto()
            {
                return Cls_Ope_Psp_Ajuste_Presupuesto_Datos.Modificar_Presupuesto(this);
            }
            public DataTable Consultar_Presupuesto()
            {
                return Cls_Ope_Psp_Ajuste_Presupuesto_Datos.Consultar_Presupuesto(this); 
            }
            
        #endregion

            
    }
}