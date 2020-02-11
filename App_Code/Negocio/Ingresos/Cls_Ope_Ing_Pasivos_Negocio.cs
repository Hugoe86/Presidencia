using System;
using System.Data;
using System.Data.OracleClient;
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
using Presidencia.Cls_Ope_Ing_Pasivos.Datos;

namespace Presidencia.Cls_Ope_Ing_Pasivos.Negocio
{

    public class Cls_Ope_Ing_Pasivos_Negocio
    {
        #region Variables Internas

        private String No_Pasivo;
        private String Claves_Ingreso_ID;
        private String Concepto_Ing_ID;
        private String SubConcepto_Ing_ID;
        private String Dependencia_ID;
        private String Cuenta_Predial_ID;
        private String No_Recibo;
        private String No_Pago;
        private Int32 No_Concepto = 1;
        private String Referencia;
        private String Origen;
        private String Descripcion;
        private DateTime Fecha_Ingreso;
        private DateTime Fecha_Vencimiento;
        private DateTime Fecha_Pago;
        private Decimal Monto;
        private Decimal Recargos;
        private Int64 Cantidad;
        private String Estatus;
        private String Periodo;
        private String Contribuyente;
        private String Observaciones;
        private String Usuario;

        private Boolean Incluir_Campos_Vitacora;
        private Boolean Incluir_Campos_Foraneos;
        private String Campos_Dinamicos;
        private String Filtros_Dinamicos;
        private String Agrupar_Dinamico;
        private String Ordenar_Dinamico;
        private String Join;
        private String Unir_Tablas;

        private OracleCommand Cmmd;
        #endregion

        #region Variables Publicas
        public String P_No_Pasivo
        {
            get { return No_Pasivo; }
            set { No_Pasivo = value; }
        }

        public String P_Claves_Ingreso_ID
        {
            get { return Claves_Ingreso_ID; }
            set { Claves_Ingreso_ID = value; }
        }

        public String P_Concepto_Ing_ID
        {
            get { return Concepto_Ing_ID; }
            set { Concepto_Ing_ID = value; }
        }

        public String P_SubConcepto_Ing_ID
        {
            get { return SubConcepto_Ing_ID; }
            set { SubConcepto_Ing_ID = value; }
        }

        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }

        public String P_Cuenta_Predial_ID
        {
            get { return Cuenta_Predial_ID; }
            set { Cuenta_Predial_ID = value; }
        }

        public String P_No_Recibo
        {
            get { return No_Recibo; }
            set { No_Recibo = value; }
        }

        public String P_No_Pago
        {
            get { return No_Pago; }
            set { No_Pago = value; }
        }

        public Int32 P_No_Concepto
        {
            get { return No_Concepto; }
            set { No_Concepto = value; }
        }

        public String P_Referencia
        {
            get { return Referencia; }
            set { Referencia = value; }
        }

        public String P_Origen
        {
            get { return Origen; }
            set { Origen = value; }
        }

        public String P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }

        public DateTime P_Fecha_Ingreso
        {
            get { return Fecha_Ingreso; }
            set { Fecha_Ingreso = value; }
        }

        public DateTime P_Fecha_Vencimiento
        {
            get { return Fecha_Vencimiento; }
            set { Fecha_Vencimiento = value; }
        }

        public DateTime P_Fecha_Pago
        {
            get { return Fecha_Pago; }
            set { Fecha_Pago = value; }
        }

        public Decimal P_Monto
        {
            get { return Monto; }
            set { Monto = value; }
        }

        public Decimal P_Recargos
        {
            get { return Recargos; }
            set { Recargos = value; }
        }

        public Int64 P_Cantidad
        {
            get { return Cantidad; }
            set { Cantidad = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Periodo
        {
            get { return Periodo; }
            set { Periodo = value; }
        }

        public String P_Contribuyente
        {
            get { return Contribuyente; }
            set { Contribuyente = value; }
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



        public Boolean P_Incluir_Campos_Vitacora
        {
            get { return Incluir_Campos_Vitacora; }
            set { Incluir_Campos_Vitacora = value; }
        }

        public Boolean P_Incluir_Campos_Foraneos
        {
            get { return Incluir_Campos_Foraneos; }
            set { Incluir_Campos_Foraneos = value; }
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

        public String P_Join
        {
            get { return Join; }
            set { Join = value; }
        }

        public String P_Unir_Tablas
        {
            get { return Unir_Tablas; }
            set { Unir_Tablas = value; }
        }


        public OracleCommand P_Cmmd
        {
            get { return Cmmd; }
            set { Cmmd = value; }
        }
        #endregion

        #region Metodos
        public Boolean Alta_Pasivo()
        {
            return Cls_Ope_Ing_Pasivos_Datos.Alta_Pasivo(this);
        }

        public Boolean Modificar_Pasivo()
        {
            return Cls_Ope_Ing_Pasivos_Datos.Modificar_Pasivo(this);
        }

        public Boolean Eliminar_Pasivo()
        {
            return Cls_Ope_Ing_Pasivos_Datos.Eliminar_Pasivo(this);
        }

        public DataTable Consultar_Pasivos()
        {
            return Cls_Ope_Ing_Pasivos_Datos.Consultar_Pasivos(this);
        }
        #endregion

    }
}