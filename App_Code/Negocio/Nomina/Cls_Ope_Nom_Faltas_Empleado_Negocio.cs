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
using Presidencia.Faltas_Empleado.Datos;

namespace Presidencia.Faltas_Empleado.Negocio
{
    public class Cls_Ope_Nom_Faltas_Empleado_Negocio
    {
        #region (Variables Internas)
        private String No_Falta;
        private String Empleado_ID;
        private String Dependencia_ID;
        private String Fecha;
        private String Tipo_Falta;
        private String Retardo;
        private Double Cantidad;
        private String Comentarios;
        private String Usuario_Creo;
        private String Usuario_Modifico;
        private String Nombre_Empleado;
        private String Fecha_Inicio;
        private String Fecha_Fin;
        private String Nomina_ID;
        private Int32 No_Nomina;
        private String Estatus;
        #endregion

        #region (Variables Publicas)
        public String P_No_Falta
        {
            get { return No_Falta; }
            set { No_Falta = value; }
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
        public String P_Tipo_Falta
        {
            get { return Tipo_Falta; }
            set { Tipo_Falta = value; }
        }
        public String P_Retardo
        {
            get { return Retardo; }
            set { Retardo = value; }
        }
        public Double P_Cantidad
        {
            get { return Cantidad; }
            set { Cantidad = value; }
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
        public String P_Nombre_Empleado
        {
            get { return Nombre_Empleado; }
            set { Nombre_Empleado = value; }
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

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        #endregion

        #region (Metodos)
        public Boolean Alta_Falta_Empleado() {
            return Cls_Ope_Nom_Faltas_Empleado_Datos.Alta_Falta_Empleado(this);
        }
        public Boolean Modificar_Falta_Empleado()
        {
            return Cls_Ope_Nom_Faltas_Empleado_Datos.Modificar_Falta_Empleado(this);
        }
        public Boolean Eliminar_Falta_Empleado()
        {
            return Cls_Ope_Nom_Faltas_Empleado_Datos.Eliminar_Falta_Empleado(this);
        }
        public DataTable Consultar_Dependencia() {
            return Cls_Ope_Nom_Faltas_Empleado_Datos.Consultar_Dependencias();
        }
        public DataTable Consultar_Empleados() {
            return Cls_Ope_Nom_Faltas_Empleado_Datos.Consultar_Empleados(this);
        }
        public DataTable Consultar_Faltas_Empelado() {
            return Cls_Ope_Nom_Faltas_Empleado_Datos.Consultar_Faltas_Empleado(this);
        }
        public DataTable Consulta_Empleados_Avanzada() {
            return Cls_Ope_Nom_Faltas_Empleado_Datos.Consulta_Empleados_Avanzada(this);
        }
        public DataTable Consulta_Dependencia_Del_Empelado() {
            return Cls_Ope_Nom_Faltas_Empleado_Datos.Consulta_Dependencia_Del_Empelado(this);
        }
        public DataTable Consultar_Periodo_Por_Fecha() {
            return Cls_Ope_Nom_Faltas_Empleado_Datos.Consultar_Periodo_Por_Fecha(this);
        }
        public Boolean Cambiar_Estatus_Faltas_Empleados()
        {
            return Cls_Ope_Nom_Faltas_Empleado_Datos.Cambiar_Estatus_Faltas_Empleados(this);
        }
        public DataTable Consultar_Faltas_General() {
            return Cls_Ope_Nom_Faltas_Empleado_Datos.Consultar_Faltas_General(this);
        }
        #endregion
    }
}