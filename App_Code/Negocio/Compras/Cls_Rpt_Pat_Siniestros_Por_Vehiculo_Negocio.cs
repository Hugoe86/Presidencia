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
using Presidencia.Control_Patrimonial_Reporte_Siniestros.Datos;

/// <summary>
/// Summary description for Rpt_Pat_Siniestros_Por_Vehiculo
/// </summary>

namespace Presidencia.Control_Patrimonial_Reporte_Siniestros.Negocio {

    public class Cls_Rpt_Pat_Siniestros_Por_Vehiculo_Negocio{

        #region Variables Internas

            private String No_Inventario = null;
            private String Tipo_Vehiculo = null;
            private String RFC_Resguardante = null;
            private String Dependencia = null;
            private String Resguardante = null;
            private DateTime Fecha_Inicial;
            private Boolean Tomar_Fecha_Inicial = false;
            private DateTime Fecha_Final;
            private Boolean Tomar_Fecha_Final = false;
            private String Tipo_Siniestro = null;
            private String Reparacion = null;
            private String Estatus = null;
            private String Mpio_Responsable = null;
            private String Consignado = null;
            private String Pago_Sindicos = null;
            private String Aseguradora = null;

        #endregion

        #region Variables Publicas  

            public String P_No_Inventario
            {
              get { return No_Inventario; }
              set { No_Inventario = value; }
            }
            public String P_Tipo_Vehiculo
            {
              get { return Tipo_Vehiculo; }
              set { Tipo_Vehiculo = value; }
            }
            public String P_RFC_Resguardante
            {
              get { return RFC_Resguardante; }
              set { RFC_Resguardante = value; }
            }
            public String P_Dependencia
            {
              get { return Dependencia; }
              set { Dependencia = value; }
            }
            public String P_Resguardante {
              get { return Resguardante; }
              set { Resguardante = value; }
            }
            public DateTime P_Fecha_Inicial
            {
                get { return Fecha_Inicial; }
                set { Fecha_Inicial = value; }
            }
            public Boolean P_Tomar_Fecha_Inicial
            {
                get { return Tomar_Fecha_Inicial; }
                set { Tomar_Fecha_Inicial = value; }
            }
            public DateTime P_Fecha_Final
            {
                get { return Fecha_Final; }
                set { Fecha_Final = value; }
            }
            public Boolean P_Tomar_Fecha_Final
            {
                get { return Tomar_Fecha_Final; }
                set { Tomar_Fecha_Final = value; }
            }
            public String P_Tipo_Siniestro
            {
                get { return Tipo_Siniestro; }
                set { Tipo_Siniestro = value; }
            }
            public String P_Reparacion
            {
                get { return Reparacion; }
                set { Reparacion = value; }
            }
            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }
            public String P_Mpio_Responsable
            {
                get { return Mpio_Responsable; }
                set { Mpio_Responsable = value; }
            }
            public String P_Consignado
            {
                get { return Consignado; }
                set { Consignado = value; }
            }
            public String P_Pago_Sindicos
            {
                get { return Pago_Sindicos; }
                set { Pago_Sindicos = value; }
            }
            public String P_Aseguradora
            {
                get { return Aseguradora; }
                set { Aseguradora = value; }
            }
        #endregion

        #region Metodos

            public DataTable Obtener_Listado_Siniestros() {
                return Cls_Rpt_Pat_Siniestros_Por_Vehiculo_Datos.Obtener_Listado_Siniestros(this);
            }

            public DataTable Obtener_Listado_Siniestros_Observaciones() {
                return Cls_Rpt_Pat_Siniestros_Por_Vehiculo_Datos.Obtener_Listado_Siniestros_Observaciones(this);
            }

        #endregion

    }
}