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
using Presidencia.Reapertura_Turnos.Datos;

namespace Presidencia.Reapertura_Turnos.Negocio
{
    public class Cls_Ope_Pre_Reapertura_Turnos_Negocio
    {
        #region (Variables Privadas)
        private String No_Turno_Dia;
        private String Fecha_Turno;
        private String Hora_Apertura;
        private String Hora_Cierre;
        private String Estatus;
        private String Usario_Creo;
        private String Usuario_Modifico;
        private String Empleado_Reabrio;
        private String Fecha_Reapertura;
        private String Fecha_Reapertura_Cierre;
        private String Autorizo_Reapertura;
        private String Observaciones;
        private String Fecha_Inicio_Busqueda_Cierre_Dias;
        private String Fecha_Fin_Busqueda_Cierre_Dias;
        #endregion

        #region (Variables Públicas)
        public String P_No_Turno_Dia
        {
            get { return No_Turno_Dia; }
            set { No_Turno_Dia = value; }
        }

        public String P_Fecha_Turno
        {
            get { return Fecha_Turno; }
            set { Fecha_Turno = value; }
        }

        public String P_Hora_Apertura
        {
            get { return Hora_Apertura; }
            set { Hora_Apertura = value; }
        }

        public String P_Hora_Cierre
        {
            get { return Hora_Cierre; }
            set { Hora_Cierre = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Usario_Creo
        {
            get { return Usario_Creo; }
            set { Usario_Creo = value; }
        }

        public String P_Usuario_Modifico
        {
            get { return Usuario_Modifico; }
            set { Usuario_Modifico = value; }
        }

        public String P_Fecha_Inicio_Busqueda_Cierre_Dias
        {
            get { return Fecha_Inicio_Busqueda_Cierre_Dias; }
            set { Fecha_Inicio_Busqueda_Cierre_Dias = value; }
        }

        public String P_Fecha_Fin_Busqueda_Cierre_Dias
        {
            get { return Fecha_Fin_Busqueda_Cierre_Dias; }
            set { Fecha_Fin_Busqueda_Cierre_Dias = value; }
        }

        public String P_Empleado_Reabrio
        {
            get { return Empleado_Reabrio; }
            set { Empleado_Reabrio = value; }
        }

        public String P_Autorizo_Reapertura
        {
            get { return Autorizo_Reapertura; }
            set { Autorizo_Reapertura = value; }
        }

        public String P_Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
        }

        public String P_Fecha_Reapertura
        {
            get { return Fecha_Reapertura; }
            set { Fecha_Reapertura = value; }
        }

        public String P_Fecha_Reapertura_Cierre
        {
            get { return Fecha_Reapertura_Cierre; }
            set { Fecha_Reapertura_Cierre = value; }
        }

        #endregion

        #region (Métodos)
        public DataTable Consultar_Cierres_Dia() {
            return Cls_Ope_Pre_Reapertura_Turnos_Datos.Consultar_Cierres_Turno_Dia(this);
        }

        public DataTable Rpt_Reapertura_Cierre_Dia() {
            return Cls_Ope_Pre_Reapertura_Turnos_Datos.Rpt_Reapertura_Cierre_Turno_Dia(this);
        }

        public Boolean Actualiza_Estatus_Cierre_Dia() {
            return Cls_Ope_Pre_Reapertura_Turnos_Datos.Actualiza_Estatus_Cierre_Dia(this); 
        }
        #endregion
    }
}
