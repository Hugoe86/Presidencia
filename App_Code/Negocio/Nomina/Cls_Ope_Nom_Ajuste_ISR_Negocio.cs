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
using Presidencia.Ajuste_ISR.Datos;

namespace Presidencia.Ajuste_ISR.Negocio
{
    public class Cls_Ope_Nom_Ajuste_ISR_Negocio
    {
        #region (Variables Internas)
        private String No_Ajuste_ISR;
        private String Empleado_ID;
        private String Dependencia_ID;
        private String Percepcion_Deduccion_ID;
        private String Nomina_ID;
        private Int32 No_Nomina;
        private String Estatus_Ajuste_ISR;
        private String Fecha_Inicio_Pago;
        private String Fecha_Termino_Pago;
        private Double Total_ISR_Ajustar;
        private Double Total_ISR_Ajustado;
        private Int32 No_Catorcenas;
        private Double Pago_Catorcenal_ISR;
        private Int32 No_Pago;
        private String Comentarios_Ajuste;
        private String Usuario_Creo;
        private String Fecha_Creo;
        private String Usuario_Modifico;
        private DataTable Dt_Ajustes_ISR;
        private String No_Empleado;
        private String RFC_Empleado;
        #endregion

        #region (Variables Publicas)
        public String P_No_Ajuste_ISR
        {
            get { return No_Ajuste_ISR; }
            set { No_Ajuste_ISR = value; }
        }
        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }
        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }
        public String P_Percepcion_Deduccion_ID
        {
            get { return Percepcion_Deduccion_ID; }
            set { Percepcion_Deduccion_ID = value; }
        }
        public String P_Nomina_ID
        {
            get { return Nomina_ID; }
            set { Nomina_ID = value; }
        }
        public Int32 P_No_Nomina
        {
            get { return No_Nomina; }
            set { No_Nomina = value; }
        }
        public String P_Estatus_Ajuste_ISR
        {
            get { return Estatus_Ajuste_ISR; }
            set { Estatus_Ajuste_ISR = value; }
        }
        public String P_Fecha_Inicio_Pago
        {
            get { return Fecha_Inicio_Pago; }
            set { Fecha_Inicio_Pago = value; }
        }
        public String P_Fecha_Termino_Pago
        {
            get { return Fecha_Termino_Pago; }
            set { Fecha_Termino_Pago = value; }
        }
        public Double P_Total_ISR_Ajustar
        {
            get { return Total_ISR_Ajustar; }
            set { Total_ISR_Ajustar = value; }
        }
        public Double P_Total_ISR_Ajustado
        {
            get { return Total_ISR_Ajustado; }
            set { Total_ISR_Ajustado = value; }
        }
        public Int32 P_No_Catorcenas
        {
            get { return No_Catorcenas; }
            set { No_Catorcenas = value; }
        }

        public Double P_Pago_Catorcenal_ISR
        {
            get { return Pago_Catorcenal_ISR; }
            set { Pago_Catorcenal_ISR = value; }
        }
        public Int32 P_No_Pago
        {
            get { return No_Pago; }
            set { No_Pago = value; }
        }
        public String P_Comentarios_Ajuste
        {
            get { return Comentarios_Ajuste; }
            set { Comentarios_Ajuste = value; }
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
        public DataTable P_Dt_Ajustes_ISR
        {
            get { return Dt_Ajustes_ISR; }
            set { Dt_Ajustes_ISR = value; }
        }
        public String P_No_Empleado
        {
            get { return No_Empleado; }
            set { No_Empleado = value; }
        }
        public String P_RFC_Empleado
        {
            get { return RFC_Empleado; }
            set { RFC_Empleado = value; }
        }
        public String P_Fecha_Creo
        {
            get { return Fecha_Creo; }
            set { Fecha_Creo = value; }
        }
        #endregion

        #region (Metodos)
        public Boolean Alta_Ajuste_ISR()
        {
            return Cls_Ope_Nom_Ajuste_ISR_Datos.Alta_Ajuste_ISR(this);
        }
        public Boolean Modificar_Ajuste_ISR()
        {
            return Cls_Ope_Nom_Ajuste_ISR_Datos.Modificar_Ajuste_ISR(this);
        }
        public Boolean Baja_Ajuste_ISR()
        {
            return Cls_Ope_Nom_Ajuste_ISR_Datos.Eliminar_Ajuste_ISR(this);
        }
        public Cls_Ope_Nom_Ajuste_ISR_Negocio Consulta_Ajuste_ISR()
        {
            return Cls_Ope_Nom_Ajuste_ISR_Datos.Consulta_Ajuste_ISR(this);
        }
        public DataTable Consultar_Fecha_Inicio_Periodo_Pago()
        {
            return Cls_Ope_Nom_Ajuste_ISR_Datos.Consultar_Fecha_Inicio_Periodo_Pago(this);
        }
        public DataTable Consultar_Fechas_Periodo()
        {
            return Cls_Ope_Nom_Ajuste_ISR_Datos.Consultar_Fechas_Periodo(this);
        }
        public DataTable Consultar_Percepcion_Ajuste_ISR() {
            return Cls_Ope_Nom_Ajuste_ISR_Datos.Consultar_Percepcion_Ajuste_ISR(this);
        }

        public void Finiquitar_Ajuste_ISR() {
            Cls_Ope_Nom_Ajuste_ISR_Datos.Finiquitar_Ajuste_ISR(this);
        }
        #endregion
    }
}
