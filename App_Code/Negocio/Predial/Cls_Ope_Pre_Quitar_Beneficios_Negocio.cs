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
using Presidencia.Operacion_Predial_Quitar_Beneficios.Datos;

namespace Presidencia.Operacion_Predial_Quitar_Beneficios.Negocio
{
    public class Cls_Ope_Pre_Quitar_Beneficios_Negocio
    {
        #region Varibles_Internas
        private String Beneficio;
        private String Caso_Especial;
        private String Usuario_Creo;
        private DataTable Dt_Detalles_Beneficios;
        private DataTable Dt_Quitar_Beneficio;
        private DateTime Fecha_Hora;               
        private String Cuenta_Predial;
        private String Campos_Dinamicos;
        private String Filtros_Dinamicos;
        private String Observaciones;
        //private String Agrupar_Dinamico;
        //private String Ordenar_Dinamico;
        private Boolean Campos_Foraneos;

        #endregion

        #region Variables Publicas
        public Boolean P_Campos_Foraneos
        {
            get { return Campos_Foraneos; }
            set { Campos_Foraneos = value; }
        }
        public String P_Filtros_Dinamicos
        {
            get { return Filtros_Dinamicos; }
            set { Filtros_Dinamicos = value; }
        }
        public String P_Campos_Dinamicos
        {
            get { return Campos_Dinamicos; }
            set { Campos_Dinamicos = value; }
        }
        public String P_Cuenta_Predial
        {
            get { return Cuenta_Predial; }
            set { Cuenta_Predial = value; }
        }
        public String P_Caso_Especial
        {
            get { return Caso_Especial; }
            set { Caso_Especial = value; }
        }
        public DataTable P_Dt_Quitar_Beneficio
        {
            get { return Dt_Quitar_Beneficio; }
            set { Dt_Quitar_Beneficio = value; }
        }
        public DataTable P_Dt_Detalles_Beneficios
        {
            get { return Dt_Detalles_Beneficios; }
            set { Dt_Detalles_Beneficios = value; }
        }
        public String P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }

        public String P_Beneficio
        {
            get { return Beneficio; }
            set { Beneficio = value; }
        }
        public DateTime P_Fecha_Hora
        {
            get { return Fecha_Hora; }
            set { Fecha_Hora = value; }
        }
        public String P_Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
        }
        #endregion

        #region Metodos
        public DataTable Consultar_Datos_Beneficios()
        {
            return Cls_Ope_Pre_Quitar_Beneficios_Datos.Consultar_Datos_Beneficios(this);
        }
        public void Modificar_Cuota_Fija()
        {
            Cls_Ope_Pre_Quitar_Beneficios_Datos.Modificar_Cuota_Fija(this);
        }
        public void Alta_Quitar_Beneficio()
        {
            Cls_Ope_Pre_Quitar_Beneficios_Datos.Alta_Quitar_Beneficio(this);
        }
        public DataSet Consulta_Reporte()
        {
            return Cls_Ope_Pre_Quitar_Beneficios_Datos.Consulta_Reporte(this);
        }
        
        #endregion

    }
}