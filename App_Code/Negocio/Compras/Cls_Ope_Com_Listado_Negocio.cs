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
using Presidencia.Listado_Almacen.Datos;

/// <summary>
/// Summary description for Ope_Com_Listado_Negocio
/// </summary>

namespace Presidencia.Listado_Almacen.Negocio
{
    public class Cls_Ope_Com_Listado_Negocio
    {
    #region Variables Internas
        //Objeto de la clase de Datos
        Cls_Ope_Com_Listado_Datos Datos_Listado;
        //Variable que almacena el ID del listado
        private String Listado_ID;
        //Variable que almacena el ID del proyecto
        private String Proyecto_ID;
        //Variable que almacena el ID de la partida
        private String Partida_ID;
        //Variable que almacena el ID del Producto seleccionado en el grid de Busqueda_Productos
        private String Producto_ID;
        //Variable que almacena el Estatus del listado
        private String Estatus;
        //Variable que almacena el Tipo del listado Manual o automatico
        private String Tipo;
        //Variable que almacena el Folio del listado
        private String Folio;
        //Variable que almacena los comentarios del 
        private String Comentarios;
        //DataTable que trae el listado de los productos seleccionados
        private DataTable Productos_Seleccionados;
        //Variable que guarda el nombre del usuario logeado
        private String Usuario;
        //Variable que guarda el ID del usuario logeado
        private String Usuario_ID;
        //Variable que guarda el monto disponible de la partida correspondiente al listado
        private String Monto_Disponible;
        //Variable que guarda el monto comprometido de la partida correspondiente al listado
        private String Monto_Comprometido;
        //Variable que almacena el total de la 
        private String Total;
        //Variable que guarda el estas de la busqueda
        private String Estatus_Busqueda;
        //Variable que almacena la fecha inicial de la busqueda avanzada
        private String Fecha_Inicial;
         //Variable que almacena la fecha final de la busqueda avanzada
        private String Fecha_Final;
        //Variable que almacena el Id del giro seleccionado
        private String Giro_ID;
        //Variable que almacena el folio a buscar 
        private String Folio_Busqueda;
        //Impuesto aplicable al producto
        private String Impuesto_ID;
        //Variables para la busqueda de productos
        private String Familia_ID;
        private String Modelo_ID;
        private String Subfamilia_ID;
        private String Nombre_Producto;

    
    #endregion

    #region Variables Publicas
        public String P_Listado_ID
        {
            get { return Listado_ID; }
            set { Listado_ID = value; }
        }
        public String P_Proyecto_ID
        {
            get { return Proyecto_ID; }
            set { Proyecto_ID = value; }
        }
        public String P_Partida_ID
        {
            get { return Partida_ID; }
            set { Partida_ID = value; }
        }
        public String P_Producto_ID
        {
            get { return Producto_ID; }
            set { Producto_ID = value; }
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
        public String P_Folio
        {
            get { return Folio; }
            set { Folio = value; }
        }
        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }
        public DataTable P_Productos_Seleccionados
        {
            get { return Productos_Seleccionados; }
            set { Productos_Seleccionados = value; }
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
        public String P_Monto_Disponible
        {
            get { return Monto_Disponible; }
            set { Monto_Disponible = value; }
        }

        public String P_Monto_Comprometido
        {
            get { return Monto_Comprometido; }
            set { Monto_Comprometido = value; }
        }
        public String P_Total
        {
            get { return Total; }
            set { Total = value; }
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
        public String P_Impuesto_ID
        {
            get { return Impuesto_ID; }
            set { Impuesto_ID = value; }
        }
        public String P_Subfamilia_ID
        {
            get { return Subfamilia_ID; }
            set { Subfamilia_ID = value; }
        }

        public String P_Modelo_ID
        {
            get { return Modelo_ID; }
            set { Modelo_ID = value; }
        }

        public String P_Familia_ID
        {
            get { return Familia_ID; }
            set { Familia_ID = value; }
        }

        public String P_Nombre_Producto
        {
            get { return Nombre_Producto; }
            set { Nombre_Producto = value; }
        }

        
    #endregion


        public Cls_Ope_Com_Listado_Negocio()
        {
            Datos_Listado = new Cls_Ope_Com_Listado_Datos();
        }

    #region Metodos

        public DataSet Consulta_Listado()
        {
            return Datos_Listado.Consulta_Listado(this);
        }

        public DataTable Consulta_Programas()
        {
            return Datos_Listado.Consulta_Proyectos(this);
        }

        public DataTable Consulta_Partidas()
        {
            return Datos_Listado.Consulta_Partidas(this);
        }

        public DataTable Consulta_Giros()
        {
            return Datos_Listado.Consulta_Giro();
        }

        public String Alta_Listado()
        {
            return Datos_Listado.Alta_Listado(this);
        }

        public String Modificar_Listado()
        {
            return Datos_Listado.Modificar_Listado(this);
        }
        public bool Modificar_Presupuestos()
        {
            return Datos_Listado.Modificar_Presupuestos(this);
        }

        public DataTable Consultar_Productos()
        {
            return Datos_Listado.Consultar_Productos(this);
        }

        public DataTable Consultar_Productos_Reorden()
        {
            return Datos_Listado.Consultar_Productos_Reorden(this);
        }

        public DataTable Consultar_Presupuesto_Partidas()
        {
            return Datos_Listado.Consultar_Presupuesto_Partidas(this);
        }

        public void Alta_Observaciones_Listado()
        {
            Datos_Listado.Alta_Observaciones_Listado(this);
        }

        public DataTable Consultar_Observaciones_Listado()
        {
            return Datos_Listado.Consultar_Observaciones_Listado(this);
        }
      
        public DataTable Consultar_Modelos()
        {
            return Datos_Listado.Consultar_Modelos(this);
        }

        public bool Afectar_Presupuesto()
        {
            return Datos_Listado.Afectar_Presupuesto(this);
        }


    #endregion 
    }
}