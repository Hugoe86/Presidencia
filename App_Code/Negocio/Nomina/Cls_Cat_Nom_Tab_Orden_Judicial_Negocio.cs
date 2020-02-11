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
using Presidencia.Orden_Judicial.Datos;

namespace Presidencia.Orden_Judicial.Negocio
{
    public class Cls_Cat_Nom_Tab_Orden_Judicial_Negocio
    {
        #region (Variables Privadas)
        private String Orden_Judicial_ID;
        private String Empleado_ID;
        private String Tipo_Descuento_Orden_Judicial_Sueldo;
        private Double Cantidad_Porcentaje_Sueldo;
        private String Bruto_Neto_Orden_Judicial_Sueldo;
        private String Tipo_Descuento_Orden_Judicial_Aguinaldo;
        private Double Cantidad_Porcentaje_Aguinaldo;
        private String Bruto_Neto_Orden_Aguinaldo;
        private String Tipo_Descuento_Orden_Judicial_Prima_Vacacional;
        private Double Cantidad_Porcentaje_Prima_Vacacional;
        private String Bruto_Neto_Orden_Prima_Vacacional;
        private String Tipo_Descuento_Orden_Judicial_Indemnizacion;
        private Double Cantidad_Porcentaje_Indemnizacion;
        private String Bruto_Neto_Orden_Indemnizacion;
        private String Beneficiario;
        private String Usuario_Creo;
        private String Usuario_Modifico;
        #endregion

        #region (Variables Públicas)
        public String P_Orden_Judicial_ID {
            get { return Orden_Judicial_ID; }
            set { Orden_Judicial_ID = value; }
        }
        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }
        public String P_Tipo_Descuento_Orden_Judicial_Sueldo
        {
            get { return Tipo_Descuento_Orden_Judicial_Sueldo; }
            set { Tipo_Descuento_Orden_Judicial_Sueldo = value; }
        }
        public Double P_Cantidad_Porcentaje_Sueldo
        {
            get { return Cantidad_Porcentaje_Sueldo; }
            set { Cantidad_Porcentaje_Sueldo = value; }
        }
        public String P_Bruto_Neto_Orden_Judicial_Sueldo
        {
            get { return Bruto_Neto_Orden_Judicial_Sueldo; }
            set { Bruto_Neto_Orden_Judicial_Sueldo = value; }
        }
        public String P_Tipo_Descuento_Orden_Judicial_Aguinaldo
        {
            get { return Tipo_Descuento_Orden_Judicial_Aguinaldo; }
            set { Tipo_Descuento_Orden_Judicial_Aguinaldo = value; }
        }
        public Double P_Cantidad_Porcentaje_Aguinaldo
        {
            get { return Cantidad_Porcentaje_Aguinaldo; }
            set { Cantidad_Porcentaje_Aguinaldo = value; }
        }
        public String P_Bruto_Neto_Orden_Aguinaldo
        {
            get { return Bruto_Neto_Orden_Aguinaldo; }
            set { Bruto_Neto_Orden_Aguinaldo = value; }
        }
        public String P_Tipo_Descuento_Orden_Judicial_Prima_Vacacional
        {
            get { return Tipo_Descuento_Orden_Judicial_Prima_Vacacional; }
            set { Tipo_Descuento_Orden_Judicial_Prima_Vacacional = value; }
        }
        public Double P_Cantidad_Porcentaje_Prima_Vacacional
        {
            get { return Cantidad_Porcentaje_Prima_Vacacional; }
            set { Cantidad_Porcentaje_Prima_Vacacional = value; }
        }
        public String P_Bruto_Neto_Orden_Prima_Vacacional
        {
            get { return Bruto_Neto_Orden_Prima_Vacacional; }
            set { Bruto_Neto_Orden_Prima_Vacacional = value; }
        }
        public String P_Tipo_Descuento_Orden_Judicial_Indemnizacion
        {
            get { return Tipo_Descuento_Orden_Judicial_Indemnizacion; }
            set { Tipo_Descuento_Orden_Judicial_Indemnizacion = value; }
        }
        public Double P_Cantidad_Porcentaje_Indemnizacion
        {
            get { return Cantidad_Porcentaje_Indemnizacion; }
            set { Cantidad_Porcentaje_Indemnizacion = value; }
        }
        public String P_Bruto_Neto_Orden_Indemnizacion
        {
            get { return Bruto_Neto_Orden_Indemnizacion; }
            set { Bruto_Neto_Orden_Indemnizacion = value; }
        }
        public String P_Beneficiario
        {
            get { return Beneficiario; }
            set { Beneficiario = value; }
        }
        public String P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }
        public String P_Usuario_Modifico
        {
            get { return Usuario_Modifico; }
            set { Usuario_Modifico = value; }
        }
        #endregion

        #region (Métodos)
        public Boolean Alta_Parametro_Orden_Judicial() {
            return Cls_Cat_Nom_Tab_Orden_Judicial_Datos.Alta_Parametro_Orden_Judicial(this);
        }

        public Boolean Modificar_Parametro_Orden_Judicial() {
            return Cls_Cat_Nom_Tab_Orden_Judicial_Datos.Modificar_Parametro_Orden_Judicial(this);
        }

        public Boolean Eliminar_Parametro_Orden_Judicial() {
            return Cls_Cat_Nom_Tab_Orden_Judicial_Datos.Eliminar_Parametro_Orden_Judicial(this);
        }

        public DataTable Consultar_Parametros_Orden_Judicial_Empleado() {
            return Cls_Cat_Nom_Tab_Orden_Judicial_Datos.Consultar_Parametros_Orden_Judicial_Empleado(this);
        }
        #endregion
    }
}
