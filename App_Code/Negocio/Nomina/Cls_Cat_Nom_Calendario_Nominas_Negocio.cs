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
using Presidencia.Calendario_Nominas.Datos;

namespace Presidencia.Calendario_Nominas.Negocios
{
    public class Cls_Cat_Nom_Calendario_Nominas_Negocio
    {

        #region (Variables Internas)
        private String Nomina_ID;
        private String Fecha_Inicio;
        private String Fecha_Fin;
        private String Usuario_Creo;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
        private String Fecha_Creo;
        private String Usuario_Modico;
        private String Fecha_Modifico;
        private DataTable Dt_Periodos_Pago;
        private Int32 No_Nomina;
        private Int32 Anio;
        private String Tipo_Nomina_ID;
        private String Detalle_Nomina_ID;

        private String Fecha_Busqueda_Periodo;
        #endregion

        #region (Variables Publicas)
        public String P_Nomina_ID
        {
            get { return Nomina_ID; }
            set { Nomina_ID = value; }
        }
        public String P_Fecha_Inicio
        {
            get { return Fecha_Inicio; }
            set { Fecha_Inicio = value; }
        }
        public String P_Fecha_Fin
        {
            get { return Fecha_Fin; }
            set { Fecha_Fin = value; }
        }
        public String P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }
        public String P_Fecha_Creo
        {
            get { return Fecha_Creo; }
            set { Fecha_Creo = value; }
        }
        public String P_Usuario_Modico
        {
            get { return Usuario_Modico; }
            set { Usuario_Modico = value; }
        }
        public String P_Fecha_Modifico
        {
            get { return Fecha_Modifico; }
            set { Fecha_Modifico = value; }
        }
        public DataTable P_Dt_Periodos_Pago
        {
            get { return Dt_Periodos_Pago; }
            set { Dt_Periodos_Pago = value; }
        }

        public Int32 P_No_Nomina
        {
            get { return No_Nomina; }
            set { No_Nomina = value; }
        }

        public Int32 P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }

        public String P_Tipo_Nomina_ID {
            get { return Tipo_Nomina_ID; }
            set { Tipo_Nomina_ID = value; }
        }

        public String P_Detalle_Nomina_ID {
            get { return Detalle_Nomina_ID; }
            set { Detalle_Nomina_ID = value; }
        }

        public String P_Fecha_Busqueda_Periodo
        {
            get { return Fecha_Busqueda_Periodo; }
            set { Fecha_Busqueda_Periodo = value; }
        }
        #endregion

        #region (Metodos)
        public DataTable Consultar_Calendario_Nominas() {
            return Cls_Cat_Nom_Calendario_Nominas_Datos.Consultar_Calendario_Nominas();
        }
        public Boolean Alta_Calendarios_Nomina() {
            return Cls_Cat_Nom_Calendario_Nominas_Datos.Alta_Calendario_Nomina(this);
        }
        public Boolean Actualizar_Calendario_Nomina() {
            return Cls_Cat_Nom_Calendario_Nominas_Datos.Actualizar_Calendario_Nomina(this);
        }
        public Boolean Eliminar_Calendario_Nomina() {
            return Cls_Cat_Nom_Calendario_Nominas_Datos.Eliminar_Calendario_Nomina(this);
        }
        public DataTable Consulta_Detalles_Nomina() {
            return Cls_Cat_Nom_Calendario_Nominas_Datos.Consulta_Detalles_Nomina(this);
        }
        public DataTable Consulta_Periodos_Nomina() {
            return Cls_Cat_Nom_Calendario_Nominas_Datos.Consulta_Periodos_Nomina(this);
        }
        public DataTable Consultar_Calendario_Nomina_Fecha_Actual() {
            return Cls_Cat_Nom_Calendario_Nominas_Datos.Consultar_Calendario_Nomina_Fecha_Actual();
        }

        public Boolean Alta_Cierre_Periodo_Nomina() {
            return Cls_Cat_Nom_Calendario_Nominas_Datos.Alta_Cierre_Periodo_Nomina(this);
        }

        public DataTable Consulta_Periodo_Cerrado() {
            return Cls_Cat_Nom_Calendario_Nominas_Datos.Consulta_Periodo_Cerrado(this);
        }

        public DataTable Consultar_Periodos_Por_Nomina_Periodo() {
            return Cls_Cat_Nom_Calendario_Nominas_Datos.Consultar_Periodos_Por_Nomina_Periodo(this);
        }

        public DataTable Consulta_Detalle_Periodo_Actual() {
            return Cls_Cat_Nom_Calendario_Nominas_Datos.Consulta_Detalle_Periodo_Actual(this);
        }
        #endregion

    }
}