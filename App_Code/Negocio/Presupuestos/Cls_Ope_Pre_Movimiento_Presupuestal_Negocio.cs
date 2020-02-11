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
using Presidencia.Movimiento_Presupuestal.Datos;

namespace Presidencia.Movimiento_Presupuestal.Negocio
{
    public class Cls_Ope_Pre_Movimiento_Presupuestal_Negocio
    {
        #region(Variables Privadas)
            private String No_Solicitud;
            private String Codigo_Programatico_De;//la palabra "de" sera la persona que envia la informacion
            private String Codigo_Programatico_Al;//la palabra "al"de sera la persona que recibe la informacion
            private String Fuente_Financiera_De;
            private String Fuente_Financiera_Al;
            private String Area_Funcional_De;
            private String Area_Funcional_Al;
            private String Programa_De;
            private String Programa_Al;
            private String Responsable_De;
            private String Responsable_Al;
            private String Partida_De;
            private String Partida_Al;
            private String Monto;
            private String Justificacion;
            private String Estatus;
            private String Usuario_Creo;
        #endregion

        #region(Variables Publicas)
            public String P_No_Solicitud
            {
                get { return No_Solicitud; }
                set { No_Solicitud = value; }
            }
            public String P_Codigo_Programatico_De
            {
                get { return Codigo_Programatico_De; }
                set { Codigo_Programatico_De = value; }
            }
            public String P_Codigo_Programatico_Al
            {
                get { return Codigo_Programatico_Al; }
                set { Codigo_Programatico_Al = value; }
            }
            public String P_Fuente_Financiera_De
            {
                get { return Fuente_Financiera_De; }
                set { Fuente_Financiera_De = value; }
            }
            public String P_Fuente_Financiera_AL
            {
                get { return Fuente_Financiera_Al; }
                set { Fuente_Financiera_Al = value; }
            }
            public String P_Area_Funcional_De
            {
                get { return Area_Funcional_De; }
                set { Area_Funcional_De = value; }
            }
            public String P_Area_Funcional_AL
            {
                get { return Area_Funcional_Al; }
                set { Area_Funcional_Al = value; }
            }
            public String P_Programa_De
            {
                get { return Programa_De; }
                set { Programa_De = value; }
            }
            public String P_Programa_Al
            {
                get { return Programa_Al; }
                set { Programa_Al = value; }
            }
            public String P_Responsable_De
            {
                get { return Responsable_De; }
                set { Responsable_De = value; }
            }
            public String P_Responsable_Al
            {
                get { return Responsable_Al; }
                set { Responsable_Al = value; }
            }
            public String P_Partida_De
            {
                get { return Partida_De; }
                set { Partida_De = value; }
            }
            public String P_Partida_Al
            {
                get { return Partida_Al; }
                set { Partida_Al = value; }
            }
            public String P_Monto
            {
                get { return Monto; }
                set { Monto = value; }
            }
            public String P_Justificacion
            {
                get { return Justificacion; }
                set { Justificacion = value; }
            }
            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }
            public String P_Usuario_Creo
            {
                get { return Usuario_Creo; }
                set { Usuario_Creo = value; }
            }
        #endregion

        #region(Metodos)
            public Boolean Alta_Movimiento()
            {
                return Cls_Ope_Pre_Movimiento_Presupuestal_Datos.Alta_Movimiento(this);
            }
            public Boolean Modificar_Movimiento()
            {
                return Cls_Ope_Pre_Movimiento_Presupuestal_Datos.Modificar_Movimiento(this);
            }
            public Boolean Eliminar_Movimiento()
            {
                return Cls_Ope_Pre_Movimiento_Presupuestal_Datos.Eliminar_Movimiento(this);
            }
            public DataTable Consulta_Movimiento()
            {
                return Cls_Ope_Pre_Movimiento_Presupuestal_Datos.Consultar_Movimiento(this);
            }
        #endregion


    }
}