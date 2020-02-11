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
using Presidencia.Operacion_Predial_Convenios_Fraccionamientos.Datos;

/// <summary>
/// Summary description for Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio
/// </summary>

namespace Presidencia.Operacion_Predial_Convenios_Fraccionamientos.Negocio
{

    public class Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio
    {

        #region Varibles Internas

        private String No_Convenio;
        private String Cuenta_Predial_ID;
        private String Propietario_ID;
        private String Realizo;
        private String No_Reestructura;
        private String Estatus;
        private String Estatus_Cancelacion_Cuenta;
        private String Solicitante;
        private String RFC;
        private Int32 Numero_Parcialidades;
        private String Periodicidad_Pago;
        private DateTime Fecha;
        private DateTime Fecha_Vencimiento;
        private String Observaciones;
        private Double Descuento_Recargos_Ordinarios;
        private Double Descuento_Recargos_Moratorios;
        private Double Descuento_Multas;
        private Double Total_Adeudo;
        private Double Total_Descuento;
        private Double Sub_Total;
        private Double Porcentaje_Anticipo;
        private Double Total_Anticipo;
        private Double Total_Convenio;
        private String Usuario;
        private String Campos_Dinamicos;
        private String Filtros_Dinamicos;
        private String Agrupar_Dinamico;
        private String Ordenar_Dinamico;
        private Boolean Campos_Foraneos;
        private DataTable Dt_Parcialidades;
        private DataTable Dt_Parcialidades_Pagadas;
        private Boolean Mostrar_Ultimo_Convenio;
        private Boolean Mostrar_Detalles_Con_Reestructura;
        private Boolean Es_Reestructura;
        private String No_Impuesto_Fraccionamiento;
        private String No_Descuento;
        private String Anio;

        private OracleCommand Cmmd;

        #endregion

        #region Varibles Publicas

        public String P_No_Convenio
        {
            get { return No_Convenio; }
            set { No_Convenio = value; }
        }

        public String P_Cuenta_Predial_ID
        {
            get { return Cuenta_Predial_ID; }
            set { Cuenta_Predial_ID = value; }
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

        public String P_No_Reestructura
        {
            get { return No_Reestructura; }
            set { No_Reestructura = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Estatus_Cancelacion_Cuenta
        {
            get { return Estatus_Cancelacion_Cuenta; }
            set { Estatus_Cancelacion_Cuenta = value; }
        }

        public String P_Solicitante
        {
            get { return Solicitante; }
            set { Solicitante = value; }
        }

        public String P_RFC
        {
            get { return RFC; }
            set { RFC = value; }
        }

        public Int32 P_Numero_Parcialidades
        {
            get { return Numero_Parcialidades; }
            set { Numero_Parcialidades = value; }
        }

        public String P_Periodicidad_Pago
        {
            get { return Periodicidad_Pago; }
            set { Periodicidad_Pago = value; }
        }

        public DateTime P_Fecha_Vencimiento
        {
            get { return Fecha_Vencimiento; }
            set { Fecha_Vencimiento = value; }
        }

        public DateTime P_Fecha
        {
            get { return Fecha; }
            set { Fecha = value; }
        }

        public String P_Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
        }

        public Double P_Descuento_Recargos_Ordinarios
        {
            get { return Descuento_Recargos_Ordinarios; }
            set { Descuento_Recargos_Ordinarios = value; }
        }

        public Double P_Descuento_Recargos_Moratorios
        {
            get { return Descuento_Recargos_Moratorios; }
            set { Descuento_Recargos_Moratorios = value; }
        }

        public Double P_Descuento_Multas
        {
            get { return Descuento_Multas; }
            set { Descuento_Multas = value; }
        }

        public Double P_Total_Adeudo
        {
            get { return Total_Adeudo; }
            set { Total_Adeudo = value; }
        }

        public Double P_Total_Descuento
        {
            get { return Total_Descuento; }
            set { Total_Descuento = value; }
        }

        public Double P_Sub_Total
        {
            get { return Sub_Total; }
            set { Sub_Total = value; }
        }

        public Double P_Porcentaje_Anticipo
        {
            get { return Porcentaje_Anticipo; }
            set { Porcentaje_Anticipo = value; }
        }

        public Double P_Total_Anticipo
        {
            get { return Total_Anticipo; }
            set { Total_Anticipo = value; }
        }

        public Double P_Total_Convenio
        {
            get { return Total_Convenio; }
            set { Total_Convenio = value; }
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

        public Boolean P_Es_Reestructura
        {
            get { return Es_Reestructura; }
            set { Es_Reestructura = value; }
        }

        public DataTable P_Dt_Parcialidades
        {
            get { return Dt_Parcialidades; }
            set { Dt_Parcialidades = value; }
        }

        public DataTable P_Dt_Parcialidades_Pagadas
        {
            get { return Dt_Parcialidades_Pagadas; }
            set { Dt_Parcialidades_Pagadas = value; }
        }

        public Boolean P_Mostrar_Ultimo_Convenio
        {
            get { return Mostrar_Ultimo_Convenio; }
            set { Mostrar_Ultimo_Convenio = value; }
        }

        public Boolean P_Mostrar_Detalles_Con_Reestructura
        {
            get { return Mostrar_Detalles_Con_Reestructura; }
            set { Mostrar_Detalles_Con_Reestructura = value; }
        }

        public String P_No_Impuesto_Fraccionamiento
        {
            get { return No_Impuesto_Fraccionamiento; }
            set { No_Impuesto_Fraccionamiento = value; }
        }

        public String P_No_Descuento
        {
            get { return No_Descuento; }
            set { No_Descuento = value; }
        }

        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }

        public OracleCommand P_Cmmd
        {
            get { return Cmmd; }
            set { Cmmd = value; }
        }

        #endregion

        #region Metodos

            public Boolean Alta_Convenio_Fraccionamiento()
            {
                return Cls_Ope_Pre_Convenios_Fraccionamientos_Datos.Alta_Convenio_Fraccionamiento(this);
            }

            public Boolean Alta_Reestructura_Convenio_Fraccionamiento()
            {
                return Cls_Ope_Pre_Convenios_Fraccionamientos_Datos.Alta_Reestructura_Convenio_Fraccionamiento(this);
            }

            public Boolean Modificar_Convenio_Fraccionamiento()
            {
                return Cls_Ope_Pre_Convenios_Fraccionamientos_Datos.Modificar_Convenio_Fraccionamiento(this);
            }

            public Boolean Modificar_Reestructura_Convenio_Fraccionamiento()
            {
                return Cls_Ope_Pre_Convenios_Fraccionamientos_Datos.Modificar_Reestructura_Convenio_Fraccionamiento(this);
            }

            public Boolean Modificar_Estatus_Convenio_Reestructura()
            {
                return Cls_Ope_Pre_Convenios_Fraccionamientos_Datos.Modificar_Estatus_Convenio_Reestructura(this);
            }

            public Boolean Convenio_Incumplido()
            {
                return Cls_Ope_Pre_Convenios_Fraccionamientos_Datos.Convenio_Incumplido(this);
            }

            public DataTable Consultar_Convenio_Fraccionamiento()
            {
                return Cls_Ope_Pre_Convenios_Fraccionamientos_Datos.Consultar_Convenio_Fraccionamiento(this);
            }

            public DataTable Consultar_Adeudos_Derecho_Supervisions(Boolean Total_O_A_Pagar)
            {
                return Cls_Ope_Pre_Convenios_Fraccionamientos_Datos.Consultar_Adeudos_Fraccionamientos(this, Total_O_A_Pagar);
            }

            public Boolean Cancelar_Pasivo(String Referencia, String Monto)
            {
                return Cls_Ope_Pre_Convenios_Fraccionamientos_Datos.Cancelar_Pasivo(Referencia, Monto);
            }

            public DataTable Consultar_Descuentos()
            {
                return Cls_Ope_Pre_Convenios_Fraccionamientos_Datos.Consultar_Descuentos(this);
            }

            public bool Eliminar_Pasivo(String Referencia)
            {
                return Cls_Ope_Pre_Convenios_Fraccionamientos_Datos.Eliminar_Pasivo(Referencia);
            }

            public DataTable Consultar_Datos_A_Eliminar()
            {
                return Cls_Ope_Pre_Convenios_Fraccionamientos_Datos.Impuesto_Para_Eliminar_Pasivo(this);
            }
        
        #endregion

    }
}   