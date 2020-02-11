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
using Presidencia.Licitaciones_Compras.Datos;

/// <summary>
/// Summary description for Cls_Ope_Com_Licitaciones_Negocio
/// </summary>

namespace Presidencia.Licitaciones_Compras.Negocio
{
  
    public class Cls_Ope_Com_Licitaciones_Negocio
    {

        #region Variables Internas

        private String No_Licitacion;
        private String Folio;
        private String Estatus;
        private String Fecha_Inicio;
        private String Fecha_Fin;
        private String Justificacion;
        private String Comentarios;
        private String Monto_Total;
        private String Lista_Requisiciones;
        private String Lista_Consolidaciones;
        private String Usuario;
        private String Usuario_ID;
        private DataTable Dt_Requisiciones;
        private Cls_Ope_Com_Licitaciones_Datos Licitacion_Datos;
        //variables Consulta de requisiciones
        private String Requisicion_ID;
        private String Area_ID;
        private String Dependencia_ID;
        private String No_Consolidacion;
        private String Folio_Busqueda;
        private String Tipo;
        private String Clasificacion;
        
        #endregion

        #region Variables Publicas

        public String P_No_Licitacion
        {
            get { return No_Licitacion; }
            set { No_Licitacion = value; }
        }
        public String P_Folio
        {
            get { return Folio; }
            set { Folio = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
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
        public String P_Justificacion
        {
            get { return Justificacion; }
            set { Justificacion = value; }
        }

        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }
        public String P_Monto_Total
        {
            get { return Monto_Total; }
            set { Monto_Total = value; }
        }

        public String P_Lista_Requisiciones
        {
            get { return Lista_Requisiciones; }
            set { Lista_Requisiciones = value; }
        }
        public String P_Usuario    
        {
            get { return Usuario; }
            set { Usuario = value; }
        }
        public String P_Usuario_ID
        {
            get { return Usuario_ID; }
            set { Usuario_ID = value; }
        }
        public DataTable P_Dt_Requisiciones
        {
            get { return Dt_Requisiciones; }
            set { Dt_Requisiciones = value; }
        }

        public String P_Requisicion_ID
        {
            get { return Requisicion_ID; }
            set { Requisicion_ID = value; }
        }

        public String P_Area_ID
        {
            get { return Area_ID; }
            set { Area_ID = value; }
        }

        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }
        public String P_No_Consolidacion
        {
            get { return No_Consolidacion; }
            set { No_Consolidacion = value; }
        }
        public bool P_Consolidadas
        {
            get { return Consolidadas; }
            set { Consolidadas = value; }
        }

        public String P_Folio_Busqueda
        {
            get { return Folio_Busqueda; }
            set { Folio_Busqueda = value; }
        }

        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }

        public String P_Lista_Consolidaciones
        {
            get { return Lista_Consolidaciones; }
            set { Lista_Consolidaciones = value; }
        }

        public String P_Clasificacion
        {
            get { return Clasificacion; }
            set { Clasificacion = value; }
        }
        private bool Consolidadas;

        #endregion

        #region Metodos
        public Cls_Ope_Com_Licitaciones_Negocio()
        {
            Licitacion_Datos = new Cls_Ope_Com_Licitaciones_Datos();
        }

        public void Alta_Licitacion()
        {
            Licitacion_Datos.Alta_Licitacion(this);
        }

        public DataTable Consultar_Licitaciones()
        {
            return Licitacion_Datos.Consultar_Licitaciones(this);
        }

        public DataTable Consultar_Requisiciones()
        {
            return Licitacion_Datos.Consultar_Requisiciones(this);
        }

        public DataTable Consultar_Licitacion_Detalle_Requisicion()
        {
            return Licitacion_Datos.Consultar_Licitacion_Detalle_Requisicion(this);
        }

        public DataTable Consultar_Licitacion_Detalle_Consolidacion()
        {
            return Licitacion_Datos.Consultar_Licitacion_Detalle_Consolidacion(this);
        }

        public void Modificar_Licitacion()
        {
            Licitacion_Datos.Modificar_Licitacion(this);
        }

        public DataTable Consulta_Consolidaciones()
        {
            return Licitacion_Datos.Consulta_Consolidaciones(this);
        }
  
        #endregion

    }//fin del class
}//fin del namespace