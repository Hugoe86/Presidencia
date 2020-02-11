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
using Presidencia.Tiempo_Extra.Datos;

namespace Presidencia.Tiempo_Extra.Negocio
{
    public class Cls_Ope_Nom_Tiempo_Extra_Negocio
    {
        #region (Variables Internas)
        private String No_Tiempo_Extra;
        private String Empleado_ID;
        private String Dependencia_ID;
        private String Fecha;
        private String Pago_Dia_Doble;
        private Double Horas;
        private String Estatus;
        private String Comentarios_Estatus;
        private String Comentarios;
        private String Usuario_Creo;
        private String Usuario_Modifico;
        private DataTable Dt_Empleados;
        private DataTable Dt_Horas_Extra;
        private String Nomina_ID;
        private Int32 No_Nomina;
        #endregion

        #region (Variables Publicas)
        public String P_No_Tiempo_Extra
        {
            get { return No_Tiempo_Extra; }
            set { No_Tiempo_Extra = value; }
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
        public String P_Fecha
        {
            get { return Fecha; }
            set { Fecha = value; }
        }
        public String P_Pago_Dia_Doble
        {
            get { return Pago_Dia_Doble; }
            set { Pago_Dia_Doble = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Comentarios_Estatus
        {
            get { return Comentarios_Estatus; }
            set { Comentarios_Estatus = value; }
        }
        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
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
        public Double P_Horas
        {
            get { return Horas; }
            set { Horas = value; }
        }
        public DataTable P_Dt_Empleados {
            get { return Dt_Empleados; }
            set {Dt_Empleados = value;}
        }
        public DataTable P_Dt_Horas_Extra
        {
            get { return Dt_Horas_Extra; }
            set { Dt_Horas_Extra = value; }
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
        #endregion

        #region (Metodos)
        public Boolean Alta_Tiempo_Extra() {
            return Cls_Ope_Nom_Tiempo_Extra_Datos.Alta_Tiempo_Extra(this);
        }
        public Boolean Modificar_Tiempo_Extra()
        {
            return Cls_Ope_Nom_Tiempo_Extra_Datos.Modificar_Tiempo_Extra(this);
        }
        public Boolean Eliminar_Tiempo_Extra()
        {
            return Cls_Ope_Nom_Tiempo_Extra_Datos.Eliminar_Tiempo_Extra(this);
        }
        public Cls_Ope_Nom_Tiempo_Extra_Negocio Consultar_Tiempo_Extra(String Fecha_Inicial, String Fecha_Final)
        {
            return Cls_Ope_Nom_Tiempo_Extra_Datos.Consultar_Tiempo_Extra(this, Fecha_Inicial, Fecha_Final);
        }
        public Boolean Cambiar_Estatus_Hora_Extra_Empleados() {
            return Cls_Ope_Nom_Tiempo_Extra_Datos.Cambiar_Estatus_Hora_Extra_Empleados(this);
        }
        public DataTable Consultar_Horas_Extra_Empleado(String Fecha_Inicial, String Fecha_Final)
        {
            return Cls_Ope_Nom_Tiempo_Extra_Datos.Consultar_Horas_Extra_Empleado(this, Fecha_Inicial, Fecha_Final);
        }

        public DataTable Consultar_Mini_Reporte_Tiempo_Extra()
        {
            return Cls_Ope_Nom_Tiempo_Extra_Datos.Consultar_Mini_Reporte_Tiempo_Extra(this);
        }
        #endregion

    }
}
