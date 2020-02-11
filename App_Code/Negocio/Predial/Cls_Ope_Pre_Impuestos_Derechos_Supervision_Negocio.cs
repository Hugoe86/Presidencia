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
using Presidencia.Operacion_Predial_Impuestos_Derechos_Supervision.Datos;

/// <summary>
/// Summary description for Cls_Ope_Pre_Impuestos_Derechos_Supervision_Negocio
/// </summary>

namespace Presidencia.Operacion_Predial_Impuestos_Derechos_Supervision.Negocio
{

    public class Cls_Ope_Pre_Impuestos_Derechos_Supervision_Negocio
    {

        #region Varibles Internas

        private String No_Impuesto_Derecho_Supervision;
        private String Cuenta_Predial_ID;
        private String Derecho_Supervision_Tasa_ID;
        private Double Valor_Estimado_Obra;
        private DateTime Fecha_Vencimiento;
        private DateTime Fecha_Oficio;
        private Double Recargos;
        private String Estatus;
        private String Observaciones;
        private String Usuario;
        private String Campos_Dinamicos;
        private String Filtros_Dinamicos;
        private String Agrupar_Dinamico;
        private String Ordenar_Dinamico;
        private Boolean Campos_Foraneos;
        private Boolean Campos_Sumados;

        private DataTable Dt_Detalles_Impuestos_Derechos_Supervision;

        #endregion

        #region Varibles Publicas

        public String P_No_Impuesto_Derecho_Supervision
        {
            get { return No_Impuesto_Derecho_Supervision; }
            set { No_Impuesto_Derecho_Supervision = value; }
        }

        public String P_Cuenta_Predial_ID
        {
            get { return Cuenta_Predial_ID; }
            set { Cuenta_Predial_ID = value; }
        }

        public String P_Derecho_Supervision_Tasa_ID
        {
            get { return Derecho_Supervision_Tasa_ID; }
            set { Derecho_Supervision_Tasa_ID = value; }
        }

        public Double P_Valor_Estimado_Obra
        {
            get { return Valor_Estimado_Obra; }
            set { Valor_Estimado_Obra = value; }
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

        public Double P_Recargos
        {
            get { return Recargos; }
            set { Recargos = value; }
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

        public Boolean P_Campos_Sumados
        {
            get { return Campos_Sumados; }
            set { Campos_Sumados = value; }
        }

            public Boolean P_Campos_Foraneos
            {
                get { return Campos_Foraneos; }
                set { Campos_Foraneos = value; }
            }

            public DataTable P_Dt_Detalles_Impuestos_Derechos_Supervision
            {
                get { return Dt_Detalles_Impuestos_Derechos_Supervision; }
                set { Dt_Detalles_Impuestos_Derechos_Supervision = value; }
            }

        #endregion

        #region Metodos

            public Boolean Alta_Impuestos_Derecho_Supervision()
            {
                return Cls_Ope_Pre_Impuestos_Derechos_Supervision_Datos.Alta_Impuestos_Derecho_Supervision(this);
            }

            public Boolean Modificar_Impuestos_Derecho_Supervision()
            {
                return Cls_Ope_Pre_Impuestos_Derechos_Supervision_Datos.Modificar_Impuestos_Derecho_Supervision(this);
            }

            public DataTable Consultar_Impuestos_Derecho_Supervisions()
            {
                return Cls_Ope_Pre_Impuestos_Derechos_Supervision_Datos.Consultar_Impuestos_Derecho_Supervisions(this);
            }

            public DataTable Consultar_Impuestos_Con_Convenio()
            {
                return Cls_Ope_Pre_Impuestos_Derechos_Supervision_Datos.Consultar_Impuesto_Con_Convenio(this);
            }

            public Boolean Cancelar_Pasivo(String Referencia, String Estatus, String Monto)
            {
                return Cls_Ope_Pre_Impuestos_Derechos_Supervision_Datos.Cancelar_Pasivo(Referencia, Estatus, Monto);
            }
        
        #endregion

    }
}   