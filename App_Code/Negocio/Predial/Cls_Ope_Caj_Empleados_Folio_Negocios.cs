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
using Presidencia.Empleados_Folios.Datos;

namespace Presidencia.Empleados_Folios.Negocio
{
    public class Cls_Ope_Caj_Empleados_Folio_Negocios
    {
        public Cls_Ope_Caj_Empleados_Folio_Negocios()
        {
        }
        #region (Variables Internas)
            private String No_Folio;
            private String Empleado_ID;
            private String Folio_Inicial;
            private String Folio_Final;
            private String Ultimo_Folio_Utilizado;
            private String Usuario_Creo;
            private DateTime Fecha_Creo;
            private String Usuario_Modifico;
            private DateTime Fecha_Modifico;
            private String Nombre_Empleado;
        #endregion
        #region(Variables Publicas)
            public String P_No_Folio
            {
                get { return No_Folio; }
                set { No_Folio = value; }
            }
            public String P_Empleado_ID
            {
                get { return Empleado_ID; }
                set { Empleado_ID = value; }
            }
            public String P_Folio_Inicial
            {
                get { return Folio_Inicial; }
                set { Folio_Inicial = value; }
            }
            public String P_Folio_Final
            {
                get { return Folio_Final; }
                set { Folio_Final = value; }
            }
            public String P_Ultimo_Folio_Utilizado
            {
                get { return Ultimo_Folio_Utilizado; }
                set { Ultimo_Folio_Utilizado = value; }
            }
            public String P_Usuario_Creo
            {
                get { return Usuario_Creo; }
                set { Usuario_Creo = value; }
            }
            public DateTime P_Fecha_Creo
            {
                get { return Fecha_Creo; }
                set { Fecha_Creo = value; }
            }
            public String P_Usuario_Modifico
            {
                get { return Usuario_Modifico; }
                set { Usuario_Modifico = value; }
            }
            public DateTime P_Fecha_Modifico
            {
                get { return Fecha_Modifico; }
                set { Fecha_Modifico = value; }
            }
            public String P_Nombre_Empleado
            {
                get { return Nombre_Empleado; }
                set { Nombre_Empleado = value; }
            }
        #endregion
        #region (Metodos)
            public void Alta_Folio_Empleado()
            {
                Cls_Ope_Caj_Empleados_Folio_Datos.Alta_Folio_Empleado(this);
            }
            public void Modificar_Folio_Empleado()
            {
                Cls_Ope_Caj_Empleados_Folio_Datos.Modificar_Folio_Empleado(this);
            }
            public void Actualiza_Ultimo_Folio()
            {
                Cls_Ope_Caj_Empleados_Folio_Datos.Actualiza_Ultimo_Folio(this);
            }
            public void Eliminar_Folio_Empleado()
            {
                Cls_Ope_Caj_Empleados_Folio_Datos.Eliminar_Folio_Empleado(this);
            }
            public DataTable Consulta_Datos_Folios_Empleados()
            {
                return Cls_Ope_Caj_Empleados_Folio_Datos.Consulta_Datos_Folios_Empleados(this);
            }
            public DataTable Consulta_Rango_Folio_Empleado()
            {
                return Cls_Ope_Caj_Empleados_Folio_Datos.Consulta_Rango_Folio_Empleado(this);
            }
        #endregion
    }
}
