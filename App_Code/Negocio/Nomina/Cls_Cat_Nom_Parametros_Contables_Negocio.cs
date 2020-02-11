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
using Presidencia.Parametros_Contables.Datos;

namespace Presidencia.Parametros_Contables.Negocio
{
    public class Cls_Cat_Nom_Parametros_Contables_Negocio
    {
        #region (Variables Privadas)
        private String PrimaryKey_Parametro_ID;
        private String Dietas;
        private String Sueldos_Base;
        private String Honorarios_Asimilados;
        private String Remuneraciones_Eventuales;
        private String Prima_Vacacional;
        private String Gratificaciones_Fin_Anio;
        private String Prevision_Social_Multiple;
        private String Prima_Dominical;
        private String Horas_Extra;
        private String Participacipaciones_Vigilancia;
        private String Aportaciones_ISSEG;
        private String Aportaciones_IMSS;
        private String Cuotas_Fondo_Retiro;
        private String Prestaciones;
        private String Estimulos_Productividad_Eficiencia;
        private String Impuestos_Sobre_Nominas;
        private String Pensiones;
        private String Usuario_Creo;
        private String Usuario_Modifico;
        private String Prestaciones_Establecidas_Condiciones_Trabajo;
        private String Honorarios;
        private String Seguros;
        private String Liquidaciones_Indemnizacion;
        private String Prestaciones_Retiro;        
        #endregion

        #region (Variables Publicas)
        public String P_Dietas
        {
            get { return Dietas; }
            set { Dietas = value; }
        }

        public String P_Sueldos_Base
        {
            get { return Sueldos_Base; }
            set { Sueldos_Base = value; }
        }

        public String P_Honorarios_Asimilados
        {
            get { return Honorarios_Asimilados; }
            set { Honorarios_Asimilados = value; }
        }

        public String P_Remuneraciones_Eventuales
        {
            get { return Remuneraciones_Eventuales; }
            set { Remuneraciones_Eventuales = value; }
        }

        public String P_Prima_Vacacional
        {
            get { return Prima_Vacacional; }
            set { Prima_Vacacional = value; }
        }

        public String P_Gratificaciones_Fin_Anio
        {
            get { return Gratificaciones_Fin_Anio; }
            set { Gratificaciones_Fin_Anio = value; }
        }

        public String P_Prevision_Social_Multiple
        {
            get { return Prevision_Social_Multiple; }
            set { Prevision_Social_Multiple = value; }
        }

        public String P_Prima_Dominical
        {
            get { return Prima_Dominical; }
            set { Prima_Dominical = value; }
        }

        public String P_Horas_Extra
        {
            get { return Horas_Extra; }
            set { Horas_Extra = value; }
        }

        public String P_Participacipaciones_Vigilancia
        {
            get { return Participacipaciones_Vigilancia; }
            set { Participacipaciones_Vigilancia = value; }
        }

        public String P_Aportaciones_ISSEG
        {
            get { return Aportaciones_ISSEG; }
            set { Aportaciones_ISSEG = value; }
        }

        public String P_Aportaciones_IMSS
        {
            get { return Aportaciones_IMSS; }
            set { Aportaciones_IMSS = value; }
        }

        public String P_Cuotas_Fondo_Retiro
        {
            get { return Cuotas_Fondo_Retiro; }
            set { Cuotas_Fondo_Retiro = value; }
        }

        public String P_Prestaciones
        {
            get { return Prestaciones; }
            set { Prestaciones = value; }
        }

        public String P_Estimulos_Productividad_Eficiencia
        {
            get { return Estimulos_Productividad_Eficiencia; }
            set { Estimulos_Productividad_Eficiencia = value; }
        }

        public String P_Impuestos_Sobre_Nominas
        {
            get { return Impuestos_Sobre_Nominas; }
            set { Impuestos_Sobre_Nominas = value; }
        }

        public String P_Pensiones
        {
            get { return Pensiones; }
            set { Pensiones = value; }
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

        public String P_PrimaryKey_Parametro_ID {
            get { return PrimaryKey_Parametro_ID; }
            set { PrimaryKey_Parametro_ID = value; }
        }

        public string P_Prestaciones_Establecidas_Condiciones_Trabajo
        {
            get { return Prestaciones_Establecidas_Condiciones_Trabajo; }
            set { Prestaciones_Establecidas_Condiciones_Trabajo = value; }
        }

        public string P_Honorarios
        {
            get { return Honorarios; }
            set { Honorarios = value; }
        }

        public string P_Seguros
        {
            get { return Seguros; }
            set { Seguros = value; }
        }

        public string P_Liquidaciones_Indemnizacion
        {
            get { return Liquidaciones_Indemnizacion; }
            set { Liquidaciones_Indemnizacion = value; }
        }

        public string P_Prestaciones_Retiro
        {
            get { return Prestaciones_Retiro; }
            set { Prestaciones_Retiro = value; }
        }

        #endregion

        #region (Métodos)
        public Boolean Alta() { return Cls_Cat_Nom_Parametros_Contables_Datos.Alta(this); }
        public Boolean Actualizar() { return Cls_Cat_Nom_Parametros_Contables_Datos.Actualizar(this); }
        public Boolean Eliminar() { return Cls_Cat_Nom_Parametros_Contables_Datos.Eliminar(this); }
        public DataTable Consultar_Parametros_Contables() { return Cls_Cat_Nom_Parametros_Contables_Datos.Consultar_Parametros_Contables(); }
        #endregion
    }
}
