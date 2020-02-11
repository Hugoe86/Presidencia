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
using Presidencia.IMSS.Datos;

/// <summary>
/// Summary description for Cls_Tab_Nom_IMSS_Negocio
/// </summary>
namespace Presidencia.IMSS.Negocios
{
    public class Cls_Tab_Nom_IMSS_Negocio
    {
        public Cls_Tab_Nom_IMSS_Negocio()
        {
        }
        #region (Variables Internas)
        //Propiedades
        private String IMSS_ID;
        private Double Porcentaje_Enfermedad_Maternidad_Especie;
        private Double Porcentaje_Enfermedad_Maternidad_Pesos;
        private Double Porcentaje_Invalidez_Vida;
        private Double Porcentaje_Cesantia_Vejez;
        private Double Excendente_SMG_DF;
        private Double Prestaciones_Dinero;
        private Double Gastos_Medicos;
        private String Comentarios;
        private String Nombre_Usuario;
        #endregion
        #region (Variables Publicas)
        public String P_IMSS_ID
        {
            get { return IMSS_ID; }
            set { IMSS_ID = value; }
        }

        public Double P_Porcentaje_Enfermedad_Maternidad_Especie
        {
            get { return Porcentaje_Enfermedad_Maternidad_Especie; }
            set { Porcentaje_Enfermedad_Maternidad_Especie = value; }
        }

        public Double P_Porcentaje_Enfermedad_Maternidad_Pesos
        {
            get { return Porcentaje_Enfermedad_Maternidad_Pesos; }
            set { Porcentaje_Enfermedad_Maternidad_Pesos = value; }
        }

        public Double P_Porcentaje_Invalidez_Vida
        {
            get { return Porcentaje_Invalidez_Vida; }
            set { Porcentaje_Invalidez_Vida = value; }
        }

        public Double P_Porcentaje_Cesantia_Vejez
        {
            get { return Porcentaje_Cesantia_Vejez; }
            set { Porcentaje_Cesantia_Vejez = value; }
        }

        public Double P_Excendente_SMG_DF
        {
            get { return Excendente_SMG_DF; }
            set { Excendente_SMG_DF = value; }
        }

        public Double P_Prestaciones_Dinero
        {
            get { return Prestaciones_Dinero; }
            set { Prestaciones_Dinero = value; }
        }

        public Double P_Gastos_Medicos
        {
            get { return Gastos_Medicos; }
            set { Gastos_Medicos = value; }
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
        public void Alta_IMSS()
        {
            Cls_Tab_Nom_IMSS_Datos.Alta_IMSS(this);
        }
        public void Modificar_IMSS()
        {
            Cls_Tab_Nom_IMSS_Datos.Modificar_IMSS(this);
        }
        public void Eliminar_IMSS()
        {
            Cls_Tab_Nom_IMSS_Datos.Eliminar_IMSS(this);
        }
        public DataTable Consulta_Datos_IMSS()
        {
            return Cls_Tab_Nom_IMSS_Datos.Consulta_Datos_IMSS(this);
        }
        #endregion
    }
}