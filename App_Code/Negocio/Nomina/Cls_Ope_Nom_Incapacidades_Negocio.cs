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
using Presidencia.Incapacidades.Datos;

namespace Presidencia.Incapacidades.Negocio
{
    public class Cls_Ope_Nom_Incapacidades_Negocio
    {
        #region(Variables Privadas)
        private String No_Incapacidad;
        private String Empleado_ID;
        private String Nombre_Empleado;
        private String Dependencia_ID;
        private String Estatus;
        private String Tipo_Incapacidad;
        private String Aplica_Pago_Cuarto_Dia;
        private Double Porcentaje_Incapacidad;
        private String Extencion_Incapacidad;
        private String Fecha_Inicio_Inacapacidad;
        private String Fecha_Fin_Incapacidad;
        private String Comentarios;
        private String Nomina_ID;
        private Int32 No_Nomina;
        private String Usuario_Creo;
        private String Usuario_Modifico;
        private Int32 Dias_Incapacidad;
        private String No_Empleado;
        private String Tipo_Nomina_ID;
        #endregion

        #region(Variables Públicas)
        public String P_No_Incapacidad {
            get { return No_Incapacidad; }
            set { No_Incapacidad = value; }
        }

        public String P_Empleado_ID {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }


        public String P_Nombre_Empleado
        {
            get { return Nombre_Empleado; }
            set { Nombre_Empleado = value; }
        }

        public String P_Dependencia_ID {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }

        public String P_Estatus {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Tipo_Incapacidad {
            get { return Tipo_Incapacidad; }
            set { Tipo_Incapacidad = value; }
        }

        public String P_Aplica_Pago_Cuarto_Dia {
            get { return Aplica_Pago_Cuarto_Dia; }
            set { Aplica_Pago_Cuarto_Dia = value; }
        }

        public Double P_Porcentaje_Incapacidad {
            get { return Porcentaje_Incapacidad; }
            set { Porcentaje_Incapacidad = value; }
        }

        public String P_Extencion_Incapacidad {
            get { return Extencion_Incapacidad; }
            set { Extencion_Incapacidad = value; }
        }

        public String P_Fecha_Inicio_Incapacidad {
            get { return Fecha_Inicio_Inacapacidad; }
            set { Fecha_Inicio_Inacapacidad = value; }
        }

        public String P_Fecha_Fin_Incapacidad {
            get { return Fecha_Fin_Incapacidad; }
            set { Fecha_Fin_Incapacidad = value; }
        }

        public String P_Comentarios {
            get { return Comentarios; }
            set { Comentarios = value; }
        }

        public String P_Nomina_ID {
            get { return Nomina_ID; }
            set { Nomina_ID = value; }
        }

        public Int32 P_No_Nomina {
            get { return No_Nomina; }
            set { No_Nomina = value; }
        }

        public String P_Usuario_Creo {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }

        public String P_Usuario_Modifico {
            get { return Usuario_Modifico; }
            set { Usuario_Modifico = value; }
        }

        public Int32 P_Dias_Incapacidad {
            get { return Dias_Incapacidad; }
            set { Dias_Incapacidad = value; }
        }

        public String P_No_Empleado {
            get { return No_Empleado; }
            set { No_Empleado = value; }
        }

        public String P_Tipo_Nomina_ID {
            get { return Tipo_Nomina_ID; }
            set { Tipo_Nomina_ID = value; }
        }
        #endregion    

        #region(Métodos)
        public Boolean Alta_Icapacidad() { 
            return Cls_Ope_Nom_Incapacidades_Datos.Alta_Incapacidad(this);
        }
        public Boolean Modificar_Icapacidad()
        {
            return Cls_Ope_Nom_Incapacidades_Datos.Modificar_Incapacidad(this);
        }
        public Boolean Eliminar_Icapacidad()
        {
            return Cls_Ope_Nom_Incapacidades_Datos.Eliminar_Incapacidad(this);
        }
        public Boolean Cambiar_Estatus_Incapacidad() {
            return Cls_Ope_Nom_Incapacidades_Datos.Cambiar_Estatus_Incapacidad(this);
        }
        public DataTable Consultar_Incapacidades()
        {
            return Cls_Ope_Nom_Incapacidades_Datos.Consultar_Incapacidades(this);
        }

        public DataTable Consultar_Rpt_Incapacidades() {
            return Cls_Ope_Nom_Incapacidades_Datos.Consultar_Rpt_Incapacidades(this);
        }

        public DataTable Identificar_Periodo_Nomina() {
            return Cls_Ope_Nom_Incapacidades_Datos.Identificar_Periodo_Nomina(this);
        }

        public DataTable Identificar_UR()
        {
            return Cls_Ope_Nom_Incapacidades_Datos.Identificar_UR(this);
        }

        public DataTable Identificar_Periodo_Nomina_Reloj_Checador() { return Cls_Ope_Nom_Incapacidades_Datos.Identificar_Periodo_Nomina_Reloj_Checador(this); }
        #endregion
    }
}
