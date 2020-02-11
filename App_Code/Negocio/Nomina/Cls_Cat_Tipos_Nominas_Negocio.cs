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
using Presidencia.Tipos_Nominas.Datos;

namespace Presidencia.Tipos_Nominas.Negocios
{
    public class Cls_Cat_Tipos_Nominas_Negocio
    {
        #region (Variables Internas)
        private String Tipo_Nomina_ID;
        private String Nomina;
        private Double Dias_Prima_Vacacional_1;
        private Double Dias_Prima_Vacacional_2;
        private Double Dias_Aguinaldo;
        private Double Dias_Exenta_Prima_Vacacional;
        private Double Dias_Exenta_Aguinaldo;
        private Double Despensa;
        private String Tipo;
        private String Comentarios;
        private String Nombre_Usuario;
        private DataTable Percepciones_Nomina;
        private String Aplica_ISR;
        private String Aplica_Todos;
        private String Actualizar_Salario;
        private String Dias_Prima_Antiguedad;
        #endregion

        #region (Variables Publicas)
        public String P_Tipo_Nomina_ID
        {
            get { return Tipo_Nomina_ID; }
            set { Tipo_Nomina_ID = value; }
        }
        public String P_Nomina
        {
            get { return Nomina; }
            set { Nomina = value; }
        }
        public Double P_Dias_Prima_Vacacional_1
        {
            get { return Dias_Prima_Vacacional_1; }
            set { Dias_Prima_Vacacional_1 = value; }
        }
        public Double P_Dias_Prima_Vacacional_2
        {
            get { return Dias_Prima_Vacacional_2; }
            set { Dias_Prima_Vacacional_2 = value; }
        }
        public Double P_Dias_Aguinaldo
        {
            get { return Dias_Aguinaldo; }
            set { Dias_Aguinaldo = value; }
        }
        public Double P_Dias_Exenta_Prima_Vacacional
        {
            get { return Dias_Exenta_Prima_Vacacional; }
            set { Dias_Exenta_Prima_Vacacional = value; }
        }
        public Double P_Dias_Exenta_Aguinaldo
        {
            get { return Dias_Exenta_Aguinaldo; }
            set { Dias_Exenta_Aguinaldo = value; }
        }
        public Double P_Despensa
        {
            get { return Despensa; }
            set { Despensa = value; }
        }
        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
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
        public DataTable P_Percepciones_Nomina
        {
            get { return Percepciones_Nomina; }
            set { Percepciones_Nomina = value; }
        }
        private DataTable Deducciones_Nomina;

        public DataTable P_Deducciones_Nomina
        {
            get { return Deducciones_Nomina; }
            set { Deducciones_Nomina = value; }
        }
        public String P_Aplica_ISR
        {
            get { return Aplica_ISR; }
            set { Aplica_ISR = value; }
        }

        public String P_Aplica_Todos {
            get { return Aplica_Todos; }
            set { Aplica_Todos = value; }
        }

        public String P_Actualizar_Salario
        {
            get { return Actualizar_Salario; }
            set { Actualizar_Salario = value; }
        }

        public String P_Dias_Prima_Antiguedad {
            get { return Dias_Prima_Antiguedad; }
            set { Dias_Prima_Antiguedad = value; }
        }
        #endregion

        #region (Metodos)
        public void Alta_Tipo_Nomina()
        {
            Cls_Cat_Tipos_Nominas_Datos.Alta_Tipo_Nomina(this);
        }
        public void Modificar_Tipo_Nomina()
        {
            Cls_Cat_Tipos_Nominas_Datos.Modificar_Tipo_Nomina(this);
        }
        public void Eliminar_Tipo_Nomina()
        {
            Cls_Cat_Tipos_Nominas_Datos.Eliminar_Tipo_Nomina(this);
        }
        public DataTable Consulta_Datos_Tipo_Nomina()
        {
            return Cls_Cat_Tipos_Nominas_Datos.Consulta_Datos_Tipo_Nomina(this);
        }
        public DataTable Consulta_Tipos_Nominas()
        {
            return Cls_Cat_Tipos_Nominas_Datos.Consulta_Tipos_Nominas(this);
        }
        public DataTable Consulta_Percepciones_Deducciones_Nomina()
        {
            return Cls_Cat_Tipos_Nominas_Datos.Consulta_Percepciones_Deducciones_Nomina(this);
        }
        #endregion
    }
}
