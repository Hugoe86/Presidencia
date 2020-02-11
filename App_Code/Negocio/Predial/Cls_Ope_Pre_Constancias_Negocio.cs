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
using Presidencia.Operacion_Predial_Constancias.Datos;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;

/// <summary>
/// Summary description for Cls_Ope_Pre_Constancias_Negocio
/// </summary>

namespace Presidencia.Operacion_Predial_Constancias.Negocio
{

    public class Cls_Ope_Pre_Constancias_Negocio {

        #region Varibles Internas

        private String No_Constancia;
        private String Tipo_Constancia_ID;
        private String Cuenta_Predial_ID;
        private String Cuenta_Predial;
        private String Propietario_ID;
        private String Realizo;
        private String Confronto;
        private String Documento_ID;
        private String Folio;
        private String No_Recibo;
        private Int32 No_Impresiones;
        private DateTime Fecha;
        private DateTime Fecha_Vencimiento;
        private Int32 Periodo_Año;
        private Int32 Periodo_Bimestre;
        private Int32 Periodo_Hasta_Anio;
        private Int32 Periodo_Hasta_Bimestre;
        private String Estatus;
        private String Observaciones;
        private String Leyenda_Certificacion;
        private String Usuario;
        private String Campos_Dinamicos;
        private String Filtros_Dinamicos;
        private String Agrupar_Dinamico;
        private String Ordenar_Dinamico;
        private Boolean Campos_Foraneos;
        private String Solicitante;
        private String Solicitante_RFC;
        private String Domicilio;

        #endregion

        #region Varibles Publicas

        public Int32 P_Periodo_Hasta_Anio
        {
            get { return Periodo_Hasta_Anio; }
            set { Periodo_Hasta_Anio = value; }
        }

        public Int32 P_Periodo_Hasta_Bimestre
        {
            get { return Periodo_Hasta_Bimestre; }
            set { Periodo_Hasta_Bimestre = value; }
        }

        public String P_No_Constancia
        {
            get { return No_Constancia; }
            set { No_Constancia = value; }
        }

        public String P_Tipo_Constancia_ID
        {
            get { return Tipo_Constancia_ID; }
            set { Tipo_Constancia_ID = value; }
        }

        public String P_Cuenta_Predial_ID
        {
            get { return Cuenta_Predial_ID; }
            set { Cuenta_Predial_ID = value; }
        }

        public String P_Domicilio
        {
            get { return Domicilio; }
            set { Domicilio = value; }
        }

        public String P_Cuenta_Predial
        {
            get { return Cuenta_Predial; }
            set { Cuenta_Predial = value; }
        }

        public String P_Propietario_ID
        {
            get { return Propietario_ID; }
            set { Propietario_ID = value; }
        }

        public String P_Realizo
        {
            get { return Realizo; }
            set { Realizo = value; }
        }

        public String P_Confronto
        {
            get { return Confronto; }
            set { Confronto = value; }
        }

        public String P_Documento_ID
        {
            get { return Documento_ID; }
            set { Documento_ID = value; }
        }

        public String P_Folio
        {
            get { return Folio; }
            set { Folio = value; }
        }

        public String P_No_Recibo
        {
            get { return No_Recibo; }
            set { No_Recibo = value; }
        }

        public Int32 P_No_Impresiones
        {
            get { return No_Impresiones; }
            set { No_Impresiones = value; }
        }

        public DateTime P_Fecha
        {
            get { return Fecha; }
            set { Fecha = value; }
        }

        public DateTime P_Fecha_Vencimiento
        {
            get { return Fecha_Vencimiento; }
            set { Fecha_Vencimiento = value; }
        }

        public Int32 P_Periodo_Año
        {
            get { return Periodo_Año; }
            set { Periodo_Año = value; }
        }

        public Int32 P_Periodo_Bimestre
        {
            get { return Periodo_Bimestre; }
            set { Periodo_Bimestre = value; }
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

        public String P_Leyenda_Certificacion
        {
            get { return Leyenda_Certificacion; }
            set { Leyenda_Certificacion = value; }
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

            public String P_Solicitante
            {
                get { return Solicitante; }
                set { Solicitante = value; }
            }

            public String P_Solicitante_RFC
            {
                get { return Solicitante_RFC; }
                set { Solicitante_RFC = value; }
            }

        #endregion

        #region Metodos

            public Boolean Alta_Constancia()
            {
                return Cls_Ope_Pre_Constancias_Datos.Alta_Constancia(this);
            }

            public Boolean Modificar_Constancia()
            {
                return Cls_Ope_Pre_Constancias_Datos.Modificar_Constancia(this);
            }

            public Boolean Constancia_Impresa()
            {
                return Cls_Ope_Pre_Constancias_Datos.Constancia_Impresa(this);
            }

            public Boolean Incrementar_No_Impresiones_Constancia()
            {
                return Cls_Ope_Pre_Constancias_Datos.Incrementar_No_Impresiones_Constancia(this);
            }

            public DataTable Consultar_Constancias()
            {
                return Cls_Ope_Pre_Constancias_Datos.Consultar_Constancias(this);
            }

            public DateTime Calcular_Fecha(String Fecha, String Dias)
            {
                return Cls_Ope_Pre_Constancias_Datos.Calcular_Fecha(Fecha, Dias);
            }

            public int Alta_Pasivo(Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Pasivo)
            {
                return Cls_Ope_Pre_Constancias_Datos.Alta_Pasivo(Pasivo, this);
            }
        #endregion

    }
}   