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
using Presidencia.Operacion_Predial_Impuestos_Fraccionamientos.Datos;

/// <summary>
/// Summary description for Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio
/// </summary>

namespace Presidencia.Operacion_Predial_Impuestos_Fraccionamientos.Negocio
{

    public class Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio
    {

        #region Varibles Internas

        private String No_Impuesto_Fraccionamiento;
        private String Cuenta_Predial_ID;
        private DateTime Fecha_Vencimiento;
        private DateTime Fecha_Oficio;
        private String Estatus;
        private String Observaciones;
        private String Usuario;
        private String Campos_Dinamicos;
        private String Filtros_Dinamicos;
        private String Agrupar_Dinamico;
        private String Ordenar_Dinamico;
        private Boolean Campos_Foraneos;
        private Boolean Campos_Sumados;

        private DataTable Dt_Detalles_Impuestos_Fraccionamiento;

        #endregion

        #region Varibles Publicas

        public String P_No_Impuesto_Fraccionamiento
        {
            get { return No_Impuesto_Fraccionamiento; }
            set { No_Impuesto_Fraccionamiento = value; }
        }

        public String P_Cuenta_Predial_ID
        {
            get { return Cuenta_Predial_ID; }
            set { Cuenta_Predial_ID = value; }
        }

        public DateTime P_Fecha_Vencimiento
        {
            get { return Fecha_Vencimiento; }
            set { Fecha_Vencimiento = value; }
        }

        public DateTime P_Fecha_Oficio
        {
            get { return Fecha_Oficio; }
            set { Fecha_Oficio = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
        }

        public Boolean P_Campos_Sumados
        {
            get { return Campos_Sumados; }
            set { Campos_Sumados = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public String P_Campos_Dinamicos
        {
            get { return Campos_Dinamicos; }
            set { Campos_Dinamicos = value; }
        }

        public String P_Filtros_Dinamicos
        {
            get { return Filtros_Dinamicos; }
            set { Filtros_Dinamicos = value; }
        }

        public String P_Agrupar_Dinamico
        {
            get { return Agrupar_Dinamico; }
            set { Agrupar_Dinamico = value; }
        }

        public String P_Ordenar_Dinamico
        {
            get { return Ordenar_Dinamico; }
            set { Ordenar_Dinamico = value; }
        }

            public Boolean P_Campos_Foraneos
            {
                get { return Campos_Foraneos; }
                set { Campos_Foraneos = value; }
            }

            public DataTable P_Dt_Detalles_Impuestos_Fraccionamiento
            {
                get { return Dt_Detalles_Impuestos_Fraccionamiento; }
                set { Dt_Detalles_Impuestos_Fraccionamiento = value; }
            }

        #endregion

        #region Metodos

            public Boolean Alta_Impuesto_Fraccionamiento()
            {
                return Cls_Ope_Pre_Impuestos_Fraccionamientos_Datos.Alta_Impuesto_Fraccionamiento(this);
            }

            public Boolean Modificar_Impuesto_Fraccionamiento()
            {
                return Cls_Ope_Pre_Impuestos_Fraccionamientos_Datos.Modificar_Impuesto_Fraccionamiento(this);
            }

            public Boolean Cancelar_Pasivo(String Referencia, String Estatus, String Monto)
            {
                return Cls_Ope_Pre_Impuestos_Fraccionamientos_Datos.Cancelar_Pasivo(Referencia, Estatus, Monto);
            }

            public DataTable Consultar_Impuestos_Fraccionamiento()
            {
                return Cls_Ope_Pre_Impuestos_Fraccionamientos_Datos.Consultar_Impuestos_Fraccionamiento(this);
            }

            public DataTable Consultar_Impuestos_Con_Convenio()
            {
                return Cls_Ope_Pre_Impuestos_Fraccionamientos_Datos.Consultar_Impuesto_Con_Convenio(this);
            }

            public DataTable Consultar_Propietario(String No_Orden_Var, String Anio)
            {
                return Cls_Ope_Pre_Impuestos_Fraccionamientos_Datos.Consultar_Propietario(No_Orden_Var,Anio);
            }
        
        #endregion

    }
}   