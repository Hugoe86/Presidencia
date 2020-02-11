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
using Presidencia.Consolidar_Requisicion.Datos;
namespace Presidencia.Consolidar_Requisicion.Negocio
{
    public class Cls_Ope_Com_Consolidar_Requisicion_Negocio
    {
        public Cls_Ope_Com_Consolidar_Requisicion_Negocio() { }
        #region VARIABLES INTERNAS
        private String Requisas_Seleccionadas;
        //estatus de las Requisiciones
        private String Estatus;
        private String Tipo;
        private String Tipo_Articulo;
        private String No_Consolidacion;
        private String Usuario;
        private double Monto;
        private DataTable Dt_Detalles;
        private String Fecha_Inicial;
        private String Fecha_Final;
        //Estatus de la Consolidacion
        private String Estatus_Consolidacion;
        private String Folio;

        #endregion

        #region VARIABLES PUBLICAS
        public String P_Folio
        {
            get { return Folio; }
            set { Folio = value; }
        }
        public String P_Estatus_Consolidacion
        {
            get { return Estatus_Consolidacion; }
            set { Estatus_Consolidacion = value; }
        }
        public String P_Fecha_Inicial
        {
            get { return Fecha_Inicial; }
            set { Fecha_Inicial = value; }
        }
        public String P_Fecha_Final
        {
            get { return Fecha_Final; }
            set { Fecha_Final = value; }
        }
        public DataTable P_Dt_Detalles_Consolidacion
        {
            get { return Dt_Detalles; }
            set { Dt_Detalles = value; }
        }
        public double P_Monto
        {
            get { return Monto; }
            set { Monto = value; }
        }
        public String P_Requisas_Seleccionadas
        {
            get { return Requisas_Seleccionadas; }
            set { Requisas_Seleccionadas = value; }
        }
        public String P_No_Consolidacion
        {
            get { return No_Consolidacion; }
            set { No_Consolidacion = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }
        public String P_Tipo_Articulo
        {
            get { return Tipo_Articulo; }
            set { Tipo_Articulo = value; }
        }
        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }
        #endregion

        #region MÉTODOS
        public DataTable Consultar_Requisicion()
        {
            return Cls_Ope_Com_Consolidar_Requisicion_Datos.Consultar_Requisicion(this);
        }
        public DataTable Consultar_Requisiciones_De_Consolidacion()
        {
            return Cls_Ope_Com_Consolidar_Requisicion_Datos.Consultar_Requisiciones_De_Consolidacion(this);
        }
        public DataTable Consolidar_Requisiciones_Productos()
        {
            return Cls_Ope_Com_Consolidar_Requisicion_Datos.Consolidar_Requisiciones_Productos(this);
        }
        public DataTable Consolidar_Requisiciones_Servicios()
        {
            return Cls_Ope_Com_Consolidar_Requisicion_Datos.Consolidar_Requisiciones_Servicios(this);
        }

        public DataTable Consultar_Productos_Requisas_Filtradas()
        {
            return Cls_Ope_Com_Consolidar_Requisicion_Datos.Consultar_Productos_Requisas_Filtradas(this);
        }

        public DataTable Obtener_Requisas_Con_Productos()
        {
            return Cls_Ope_Com_Consolidar_Requisicion_Datos.Obtener_Requisas_Con_Productos(this);
        }
        public DataTable Obtener_Requisas_Con_Servicios()
        {
            return Cls_Ope_Com_Consolidar_Requisicion_Datos.Obtener_Requisas_Con_Servicios(this);
        }
        public int Guardar_Consolidacion()
        {
            return Cls_Ope_Com_Consolidar_Requisicion_Datos.Guardar_Consolidacion(this);
        }
        public int Actualizar_Consolidacion()
        {
            return Cls_Ope_Com_Consolidar_Requisicion_Datos.Actualizar_Consolidacion(this);
        }
        public DataTable Consultar_Consolidaciones()
        {
            return Cls_Ope_Com_Consolidar_Requisicion_Datos.Consultar_Consolidaciones(this);
        }
        public DataTable Consultar_Requisiciones_Consolidacion()
        {
            return Cls_Ope_Com_Consolidar_Requisicion_Datos.Consultar_Requisiciones_Consolidacion(this);
        }
        //***
        public DataTable Consultar_Requisiciones_Por_Partida_Presupuestal()
        {
            return Cls_Ope_Com_Consolidar_Requisicion_Datos.Consultar_Requisiciones_Por_Partida_Presupuestal(this);
        }
        public DataTable Consultar_Partidas_De_Requisiciones_Posibles_De_Consolidar()
        {
            return Cls_Ope_Com_Consolidar_Requisicion_Datos.Consultar_Partidas_De_Requisiciones_Posibles_De_Consolidar(this);
        }        
        #endregion
    }
}