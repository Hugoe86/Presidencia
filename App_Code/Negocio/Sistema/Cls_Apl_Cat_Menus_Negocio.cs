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
using Presidencia.Menus.Datos;

/// <summary>
/// Summary description for Cls_Apl_Cat_Menus_Negocio
/// </summary>
namespace Presidencia.Menus.Negocios
{
    public class Cls_Apl_Cat_Menus_Negocio
    {
        public Cls_Apl_Cat_Menus_Negocio()
        {
        }
        #region (Variables Internas)
        //Propiedades
        private Int32 Menu_ID;
        private Int32 Parent_ID;
        private String Menu_Descripcion;
        private String Url_Link;
        private Int32 Orden;
        private String Nombre_Usuario;
        private String Clasificacion;
        private String Modulo_ID;
        #endregion
        #region (Variables Publicas)
        public String P_Clasificacion
        {
            get { return Clasificacion; }
            set { Clasificacion = value; }
        }

        public Int32 P_Menu_ID
        {
            get { return Menu_ID; }
            set { Menu_ID = value; }
        }
        public Int32 P_Parent_ID
        {
            get { return Parent_ID; }
            set { Parent_ID = value; }
        }
        public String P_Menu_Descripcion
        {
            get { return Menu_Descripcion; }
            set { Menu_Descripcion = value; }
        }
        public String P_Url_Link
        {
            get { return Url_Link; }
            set { Url_Link = value; }
        }
        public Int32 P_Orden
        {
            get { return Orden; }
            set { Orden = value; }
        }
        public String P_Nombre_Usuario
        {
            get { return Nombre_Usuario; }
            set { Nombre_Usuario = value; }
        }

        public String P_Modulo_ID {
            get { return Modulo_ID; }
            set { Modulo_ID = value; }
        }
        #endregion
        #region (Metodos)
        public void Alta_Menu()
        {
            Cls_Apl_Cat_Menus_Datos.Alta_Menu(this);
        }
        public void Modificar_Menu()
        {
            Cls_Apl_Cat_Menus_Datos.Modificar_Menu(this);
        }
        public void Eliminar_Menu()
        {
            Cls_Apl_Cat_Menus_Datos.Eliminar_Menu(this);
        }
        public DataTable Consulta_Menus()
        {
            return Cls_Apl_Cat_Menus_Datos.Consulta_Menus(this);
        }
        public DataTable Consulta_Solo_Menus()
        {
            return Cls_Apl_Cat_Menus_Datos.Consulta_Solo_Menus();
        }

        public DataTable Consulta_Menus_Submenus() {
            return Cls_Apl_Cat_Menus_Datos.Consulta_Menus_Submenus(this);
        }

        public Boolean Actualizar_Orden_Menus(DataTable Dt_Nuevo_Orden_Menus) {
            return Cls_Apl_Cat_Menus_Datos.Actualizar_Orden_Menus(Dt_Nuevo_Orden_Menus);
        }
        public DataTable Consulta_Menu_Parent_ID()
        {
            return Cls_Apl_Cat_Menus_Datos.Consulta_Menu_Parent_ID(this);
        }  
        #endregion
    }
}