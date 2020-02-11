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
using Presidencia.Ope_Dias_Festivos.Datos;

namespace Presidencia.Ope_Dias_Festivos.Negocio
{
    public class Cls_Ope_Nom_Dias_Festivos_Negocio
    {
        #region (Variables Internas)
        private String No_Dia_Festivo;
        private String Dependencia_ID;
        private String Dia_ID;
        private String Estatus;
        private String Comentarios;
        private String Comentarios_Estatus;
        private String Usuario_Creo;
        private String Usuario_Modifico;
        private DataTable Dt_Empleados;
        private DataTable Dt_Dias_Festivos;
        private String Empleado_ID;
        private String Fecha;
        private String Fecha_Inicio;
        private String Fecha_Final;
        private String Nomina_ID;
        private Int32 No_Nomina;
        #endregion

        #region (Variables Publicas)
        public String P_No_Dia_Festivo
        {
            get { return No_Dia_Festivo; }
            set { No_Dia_Festivo = value; }
        }
        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }
        public String P_Dia_ID
        {
            get { return Dia_ID; }
            set { Dia_ID = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
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
        public DataTable P_Dt_Empleados
        {
            get { return Dt_Empleados; }
            set { Dt_Empleados = value; }
        }
        public DataTable P_Dt_Dias_Festivos
        {
            get { return Dt_Dias_Festivos; }
            set { Dt_Dias_Festivos = value; }
        }
        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }
        public String P_Comentarios_Estatus
        {
            get { return Comentarios_Estatus; }
            set { Comentarios_Estatus = value; }
        }
        public String P_Fecha
        {
            get { return Fecha; }
            set { Fecha = value; }
        }
        public String P_Fecha_Inicio
        {
            get { return Fecha_Inicio; }
            set { Fecha_Inicio = value; }
        }
        public String P_Fecha_Final
        {
            get { return Fecha_Final; }
            set { Fecha_Final = value; }
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
        public Boolean Alta_Dia_Festivo() {
            return Cls_Ope_Nom_Dias_Festivos_Datos.Alta_Dias_Festivos(this);
        }
        public Boolean Modificar_Dia_Festivo()
        {
            return Cls_Ope_Nom_Dias_Festivos_Datos.Modificar_Dia_Festivo(this);
        }
        public Boolean Eliminar_Dia_Festivo()
        {
            return Cls_Ope_Nom_Dias_Festivos_Datos.Eliminar_Dia_Festivo(this);
        }
        public Cls_Ope_Nom_Dias_Festivos_Negocio Consultar_Dias_Festivos() {
            return Cls_Ope_Nom_Dias_Festivos_Datos.Consultar_Dias_Festivos(this);
        }
        public Boolean Cambiar_Estatus_Dia_Festivo() {
            return Cls_Ope_Nom_Dias_Festivos_Datos.Cambiar_Estatus_Dia_Festivo(this);
        }
        public DataTable Consultar_Periodo_Por_Fecha() {
            return Cls_Ope_Nom_Dias_Festivos_Datos.Consultar_Periodo_Por_Fecha(this);
        }
        public DataTable Consultar_Dias_Festivos_Empleado() {
            return Cls_Ope_Nom_Dias_Festivos_Datos.Consultar_Dias_Festivos_Empleado(this);
        }
        public DataTable Consultar_Mini_Reporte_Dias_Festivos()
        {
            return Cls_Ope_Nom_Dias_Festivos_Datos.Consultar_Mini_Reporte_Dias_Festivos(this);
        }
        #endregion
    }
}