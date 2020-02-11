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
using Presidencia.Administrar_Listado.Datos;


/// <summary>
/// Summary description for Cls_Ope_Alm_Com_Administrar_Listado_Negocio
/// </summary>
/// 
namespace Presidencia.Administrar_Listado.Negocio
{
    public class Cls_Ope_Alm_Com_Administrar_Listado_Negocio
    {

        #region Variables Internas 

        Cls_Ope_Alm_Com_Administrar_Listado_Datos Datos_Listado;
        private String Listado_ID;
        private String Folio;
        private String Usuario;
        private String Estatus;
        private String Comentario;
        private String Usuario_ID;
        private String Partida_ID;
        private String Estatus_Busqueda;
        //Variable que almacena la fecha inicial de la busqueda avanzada
        private String Fecha_Inicial;
        //Variable que almacena la fecha final de la busqueda avanzada
        private String Fecha_Final;
        //Variable que almacena el Id del giro seleccionado
        private String Giro_ID;
        //Variable que almacena el folio a buscar 
        private String Folio_Busqueda;
        //VAriable que almacena el total del listado 
        private String Total;
        private DataTable Dt_Productos;



        #endregion

        #region Variables Publicas

        public String P_Listado_ID
        {
            get { return Listado_ID; }
            set { Listado_ID = value; }
        }
        public String P_Folio
        {
            get { return Folio; }
            set { Folio = value; }
        }
        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Comentario
        {
            get { return Comentario; }
            set { Comentario = value; }
        }

        public String P_Usuario_ID
        {
            get { return Usuario_ID; }
            set { Usuario_ID = value; }
        }
        public String P_Partida_ID
        {
            get { return Partida_ID; }
            set { Partida_ID = value; }
        }
        public String P_Estatus_Busqueda
        {
            get { return Estatus_Busqueda; }
            set { Estatus_Busqueda = value; }
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
        public String P_Giro_ID
        {
            get { return Giro_ID; }
            set { Giro_ID = value; }
        }
        public String P_Folio_Busqueda
        {
            get { return Folio_Busqueda; }
            set { Folio_Busqueda = value; }
        }
        public String P_Total
        {
            get { return Total; }
            set { Total = value; }
        }
        public DataTable P_Dt_Productos
        {
            get { return Dt_Productos; }
            set { Dt_Productos = value; }
        } 
        #endregion

        #region Metodos
        
        public Cls_Ope_Alm_Com_Administrar_Listado_Negocio()
        {
            Datos_Listado = new Cls_Ope_Alm_Com_Administrar_Listado_Datos();
        }

        public DataTable Consulta_Listado_Almacen()
        {
            return Datos_Listado.Consulta_Listado_Almacen(this);
        }

        public DataTable Consulta_Listado_Detalle()
        {
            return Datos_Listado.Consulta_Listado_Detalle(this);
        }

        public DataTable Consultar_Observaciones_Listado()
        {
            return Datos_Listado.Consultar_Observaciones_Listado(this);
        }

        public String Modificar_Listado()
        {
            String Mensaje = Datos_Listado.Modificar_Listado(this);
            return Mensaje;
        }

        public void Liberar_Presupuesto_Cancelada()
        {
            Datos_Listado.Liberar_Presupuesto_Cancelada(this);
        }
        
        #endregion

    }//fin del Class
}//fin del Namespace