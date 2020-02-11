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
using Presidencia.Vacaciones_Empleado.Datos;

namespace Presidencia.Vacaciones_Empleado.Negocio
{
    public class Cls_Ope_Nom_Vacaciones_Empleado_Negocio
    {
        #region (Variables Internas)
        //Variables para la operacion de Ope_Nom_Vacaciones_Empleado_Negocio
        private String No_Vacacion;
        private String Empleado_ID;
        private String Dependencia_ID;
        private String Fecha_Inicio;
        private String Fecha_Termino;
        private Int32 Cantidad_Dias;
        private String Estatus;
        private String Comentarios;
        private String Comentarios_Estatus;
        private String Usuario_Creo;
        private String Usuario_Modifico;
        private DataTable Dt_Vacaciones;
        //Variable de la tabla de parametros de vacaciones
        private String Vacacion_ID;
        private Int32 Antiguedad;
        private Int32 Dias;
        //Variables Adicionales que se usaran en la operacion.
        private String No_Empleado;
        //Detalles del Prestamo
        private Int64 No_Dia_Vacacion;
        private String Fecha_Dia_Vacacion;
        private String Estatus_Dia_Vacacion;
        private String Estado_Dia_Vacacion;
        private DataTable Dt_Detalles_Vacaciones;
        private String Nomina_ID;
        private Int32 No_Nomina;

        //Detalle de las vacaciones detalles tomadas por el empleado.
        private String No_Vacacion_Detalle;
        private Int32 Anio;
        private Int32 Dias_Disponibles;
        private Int32 Dias_Tomados;
        private Int32 Periodo_Vacacional;
        private String Estatus_Detalle;
        //Campo solicitado por el usuario.
        private String Fecha_Regreso_Laboral;
        private String Estado;
        #endregion

        #region (Variables Publicas)
        public String P_No_Vacacion
        {
            get { return No_Vacacion; }
            set { No_Vacacion = value; }
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
        public String P_Fecha_Inicio
        {
            get { return Fecha_Inicio; }
            set { Fecha_Inicio = value; }
        }
        public String P_Fecha_Termino
        {
            get { return Fecha_Termino; }
            set { Fecha_Termino = value; }
        }
        public Int32 P_Cantidad_Dias
        {
            get { return Cantidad_Dias; }
            set { Cantidad_Dias = value; }
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
        public String P_Comentarios_Estatus
        {
            get { return Comentarios_Estatus; }
            set { Comentarios_Estatus = value; }
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
        public DataTable P_Dt_Vacaciones
        {
            get { return Dt_Vacaciones; }
            set { Dt_Vacaciones = value; }
        }
        /// *****************************
        public String P_Vacacion_ID
        {
            get { return Vacacion_ID; }
            set { Vacacion_ID = value; }
        }
        public Int32 P_Antiguedad
        {
            get { return Antiguedad; }
            set { Antiguedad = value; }
        }
        public Int32 P_Dias
        {
            get { return Dias; }
            set { Dias = value; }
        }
        ///**************************
        public String P_No_Empleado
        {
            get { return No_Empleado; }
            set { No_Empleado = value; }
        }
        public Int64 P_No_Dia_Vacacion
        {
            get { return No_Dia_Vacacion; }
            set { No_Dia_Vacacion = value; }
        }

        public String P_Fecha_Dia_Vacacion
        {
            get { return Fecha_Dia_Vacacion; }
            set { Fecha_Dia_Vacacion = value; }
        }

        public String P_Estatus_Dia_Vacacion
        {
            get { return Estatus_Dia_Vacacion; }
            set { Estatus_Dia_Vacacion = value; }
        }
        public String P_Estado_Dia_Vacacion
        {
            get { return Estado_Dia_Vacacion; }
            set { Estado_Dia_Vacacion = value; }
        }
        public DataTable P_Dt_Detalles_Vacaciones
        {
            get { return Dt_Detalles_Vacaciones; }
            set { Dt_Detalles_Vacaciones = value; }
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

        public String P_No_Vacacion_Detalle
        {
            get { return No_Vacacion_Detalle; }
            set { No_Vacacion_Detalle = value; }
        }

        public Int32 P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }
        public Int32 P_Dias_Disponibles
        {
            get { return Dias_Disponibles; }
            set { Dias_Disponibles = value; }
        }

        public Int32 P_Dias_Tomados
        {
            get { return Dias_Tomados; }
            set { Dias_Tomados = value; }
        }

        public Int32 P_Periodo_Vacacional
        {
            get { return Periodo_Vacacional; }
            set { Periodo_Vacacional = value; }
        }
        public String P_Estatus_Detalle
        {
            get { return Estatus_Detalle; }
            set { Estatus_Detalle = value; }
        }

        public String P_Fecha_Regreso_Laboral
        {
            get { return Fecha_Regreso_Laboral; }
            set { Fecha_Regreso_Laboral = value; }
        }

        public String P_Estado
        {
            get { return Estado; }
            set { Estado = value; }
        }
        #endregion

        #region (Metodos)
        public Int32 Consultar_Dias_Vacaciones_Empleado() {
            return Cls_Ope_Nom_Vacaciones_Empleado_Datos.Consultar_Dias_Vacaciones_Empleado(this);
        }

        public Cls_Ope_Nom_Vacaciones_Empleado_Negocio Consultar_Vacaciones() {
            return Cls_Ope_Nom_Vacaciones_Empleado_Datos.Consultar_Vacaciones(this);
        }

        public Boolean Alta_Vacaciones_Empleado() {
            return Cls_Ope_Nom_Vacaciones_Empleado_Datos.Alta_Vacaciones_Empleado(this);
        }

        public Boolean Modificar_Vacaciones_Empleado() {
            return Cls_Ope_Nom_Vacaciones_Empleado_Datos.Modificar_Vacaciones_Empleado(this);
        }

        public Boolean Eliminar_Vacaciones_Empleado() {
            return Cls_Ope_Nom_Vacaciones_Empleado_Datos.Eliminar_Vacacion_Empleado(this);
        }
        public Boolean Modifica_Estatus_Vacaciones() {
            return Cls_Ope_Nom_Vacaciones_Empleado_Datos.Modifica_Estatus_Vacaciones(this);
        }

        public DataTable Consulta_Detalles_Vacaciones() {
            return Cls_Ope_Nom_Vacaciones_Empleado_Datos.Consulta_Detalles_Vacaciones(this);
        }

        public Boolean Cambiar_Estatus_Vacaciones()
        {
            return Cls_Ope_Nom_Vacaciones_Empleado_Datos.Cambiar_Estatus_Vacaciones(this);
        }

        public DataTable Consultar_Cantidad_Dias_Disponiubles_Por_Periodo_Empleado() {
            return Cls_Ope_Nom_Vacaciones_Empleado_Datos.Consultar_Cantidad_Dias_Disponiubles_Por_Periodo_Empleado(this);
        }

        public DataTable Consultar_Vacaciones_Empl_Det() {
            return Cls_Ope_Nom_Vacaciones_Empleado_Datos.Consultar_Vacaciones_Empl_Det(this);
        }

        public Boolean Alta_Detalle_Vacaciones_Empleados() {
            return Cls_Ope_Nom_Vacaciones_Empleado_Datos.Alta_Detalle_Vacaciones_Empleados(this);
        }

        public Boolean Modificar_Detalle_Vacaciones_Empleados()
        {
            return Cls_Ope_Nom_Vacaciones_Empleado_Datos.Modificar_Detalle_Vacaciones_Empleados(this);
        }

        public DataTable Consultar_Vacaciones_Dia_Det_Pagados_Pendientes_Tomar() {
            return Cls_Ope_Nom_Vacaciones_Empleado_Datos.Consultar_Vacaciones_Dia_Det_Pagados_Pendientes_Tomar(this);
        }

        public DataTable Consultar_Vacaciones_Autorizadas_Con_Dias_Pendientes_Tomar() {
            return Cls_Ope_Nom_Vacaciones_Empleado_Datos.Consultar_Vacaciones_Autorizadas_Con_Dias_Pendientes_Tomar(this);
        }

        public Int32 Consultar_Dias_Comprometidos() {
            return Cls_Ope_Nom_Vacaciones_Empleado_Datos.Consultar_Dias_Comprometidos(this);
        }

        public DataTable Consultar_Reporte_Vacaciones()
        {
            return Cls_Ope_Nom_Vacaciones_Empleado_Datos.Consultar_Reporte_Vacaciones(this);
        }
        #endregion
    }
}