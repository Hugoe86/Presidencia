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
using Presidencia.Recibos_Empleados.Datos;

namespace Presidencia.Recibos_Empleados.Negocio
{
    public class Cls_Ope_Nom_Recibos_Empleados_Negocio
    {
        #region (Variables Internas)
        private String No_Recibo;
        private String Detalle_Nomina_ID;
        private Int32 No_Nomina;
        private String Nomina_ID;
        private String Tipo_Nomina_ID;
        private String Empleado_ID;
        private String Dependencia_ID;
        private String Puesto_ID;
        private Int32 Dias_Trabajados;
        private Double Total_Percepciones;
        private Double Total_Deducciones;
        private Double Total_Nomina;
        private Double Gravado;
        private Double Exento;
        private Double Salario_Diario;
        private Double Salario_Diario_Integrado;
        private String Usuario_Creo;
        private String Usuario_Modifico;
        DataTable Dt_Recibos_Empleados;
        DataTable Dt_Recibo_Empleado_Detalles;
        String Nomina_Generada;
        #endregion

        #region (Variables Publicas)
        public String P_No_Recibo
        {
            get { return No_Recibo; }
            set { No_Recibo = value; }
        }
        public String P_Detalle_Nomina_ID
        {
            get { return Detalle_Nomina_ID; }
            set { Detalle_Nomina_ID = value; }
        }
        public Int32 P_No_Nomina
        {
            get { return No_Nomina; }
            set { No_Nomina = value; }
        }
        public String P_Nomina_ID
        {
            get { return Nomina_ID; }
            set { Nomina_ID = value; }
        }
        public String P_Tipo_Nomina_ID
        {
            get { return Tipo_Nomina_ID; }
            set { Tipo_Nomina_ID = value; }
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
        public String P_Puesto_ID
        {
            get { return Puesto_ID; }
            set { Puesto_ID = value; }
        }
        public Int32 P_Dias_Trabajados
        {
            get { return Dias_Trabajados; }
            set { Dias_Trabajados = value; }
        }
        public Double P_Total_Percepciones
        {
            get { return Total_Percepciones; }
            set { Total_Percepciones = value; }
        }
        public Double P_Total_Deducciones
        {
            get { return Total_Deducciones; }
            set { Total_Deducciones = value; }
        }
        public Double P_Total_Nomina
        {
            get { return Total_Nomina; }
            set { Total_Nomina = value; }
        }
        public Double P_Gravado
        {
            get { return Gravado; }
            set { Gravado = value; }
        }
        public Double P_Exento
        {
            get { return Exento; }
            set { Exento = value; }
        }
        public Double P_Salario_Diario
        {
            get { return Salario_Diario; }
            set { Salario_Diario = value; }
        }
        public Double P_Salario_Diario_Integrado
        {
            get { return Salario_Diario_Integrado; }
            set { Salario_Diario_Integrado = value; }
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
        public DataTable P_Dt_Recibos_Empleados
        {
            get { return Dt_Recibos_Empleados; }
            set { Dt_Recibos_Empleados = value; }
        }
        public DataTable P_Dt_Recibo_Empleado_Detalles
        {
            get { return Dt_Recibo_Empleado_Detalles; }
            set { Dt_Recibo_Empleado_Detalles = value; }
        }

        public String P_Nomina_Generada {
            get { return Nomina_Generada; }
            set { Nomina_Generada = value; }
        }
        #endregion

        #region (Métodos)
        public void Alta_Recibo_Empleado()
        {
            Cls_Ope_Nom_Recibos_Empleados_Datos.Alta_Recibo_Empleado(this);
        }
        public void Baja_Recibo_Empleado()
        {

        }
        public DataTable Consultar_Recibos_Empleados()
        {
            return Cls_Ope_Nom_Recibos_Empleados_Datos.Consultar_Recibos_Empleados(this);
        }
        public DataTable Consultar_Recibo_Empleado_Detalles()
        {
            return null;
        }

        public DataTable Consultar_Recibos_Con_Nomina_Negativa() {
            return Cls_Ope_Nom_Recibos_Empleados_Datos.Consultar_Recibos_Con_Nomina_Negativa(this);
        }
        #endregion
    }
}