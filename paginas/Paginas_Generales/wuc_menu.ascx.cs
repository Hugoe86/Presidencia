using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Data.OracleClient;
using System.Data.SqlClient;
using Presidencia.Roles.Negocio;
using System.Text;

public partial class menu_wuc_menu : System.Web.UI.UserControl
{
    #region (Init/Load)
    /// ****************************************************************************************************************************
    /// NOMBRE: Page_Load
    /// 
    /// DESCRIPCIÓN: Ejecuta la carga inicial de la página.
    /// 
    /// USUARIO CREO: Alberto Pantoja Hernandez
    /// FECHA CREÓ: 05/08/10 
    /// USUARIO MODIFICO: Juan Alberto Hernández Negrete.
    /// FECHA MODIFICO: 26/Mayo/2011 09:11 a.m.
    /// CAUSA MODIFICACIÓN: Cambiar de usar tags table para el menu a usar listas.
    /// *****************************************************************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Válida si el menú ya se encuentra generado. Si aun no se a generado lo genera.
            if (Session["Menu_"] == null)
                Crear_Menu_Sistema_SIAS();
            else
                Lbl_Menu.Text = Session["Menu_"].ToString();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar la configuración inicial de la página. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Menú)
    /// ****************************************************************************************************************************
    /// NOMBRE: Generar_Menu
    /// 
    /// DESCRIPCIÓN: Crea el menú y lo nombra según el contenido de los submenus que le pertenceran.
    /// 
    /// PARÁMETROS: Nombre_Menu.- Es el nombre que se le ha asignadi al menú.
    /// 
    /// USUARIO CREO: Alberto Pantoja Hernandez
    /// FECHA CREÓ: 05/08/10 
    /// USUARIO MODIFICO: Juan Alberto Hernández Negrete.
    /// FECHA MODIFICO: 26/Mayo/2011 09:11 a.m.
    /// CAUSA MODIFICACIÓN: Cambiar de usar tags table para el menu a usar listas.
    /// *****************************************************************************************************************************
    public String Generar_Menu(String Nombre_Menu)
    {
        return "<a class='menuitem submenuheader'>" + Nombre_Menu + "</a>";
    }
    #endregion

    #region (Submenus)
    /// ****************************************************************************************************************************
    /// NOMBRE: Abrir_Contenedor_Submenus
    /// 
    /// DESCRIPCIÓN: Abre el contenedor de los submenus.
    /// 
    /// PARÁMETROS: No Áplica.
    /// 
    /// USUARIO CREO: Alberto Pantoja Hernandez
    /// FECHA CREÓ: 05/08/10 
    /// USUARIO MODIFICO: Juan Alberto Hernández Negrete.
    /// FECHA MODIFICO: 26/Mayo/2011 09:11 a.m.
    /// CAUSA MODIFICACIÓN: Cambiar de usar tags table para el menu a usar listas.
    /// *****************************************************************************************************************************
    public String Abrir_Contenedor_Submenus()
    {
        return "<div class='submenu'><ul>";
    }
    /// ****************************************************************************************************************************
    /// NOMBRE: Generar_Submenu
    /// 
    /// DESCRIPCIÓN: Crea el submenu.
    /// 
    /// PARÁMETROS: Nombre_Submenu.- Es el nombre que se le ha asignado al submenú.
    ///             URL_Submenu.- Dirección de la página a la cual accederemos desde el submenu.
    ///             Menu_ID.- Identificador del submenu.
    /// 
    /// USUARIO CREO: Alberto Pantoja Hernandez
    /// FECHA CREÓ: 05/08/10 
    /// USUARIO MODIFICO: Juan Alberto Hernández Negrete.
    /// FECHA MODIFICO: 26/Mayo/2011 09:11 a.m.
    /// CAUSA MODIFICACIÓN: Cambiar de usar tags table para el menu a usar listas.
    /// *****************************************************************************************************************************
    public String Generar_Submenu(String Nombre_Submenu, String URL_Menu, String Menu_ID)
    {
        return "<li><a  href='" + URL_Menu + "?PAGINA=" + Menu_ID + "'>" + Nombre_Submenu + "</a></li>";
    }
    /// ****************************************************************************************************************************
    /// NOMBRE: Cerrar_Contenedor_Submenus
    /// 
    /// DESCRIPCIÓN: Cierra el contenedor de los submenus.
    /// 
    /// PARÁMETROS: No Áplica.
    /// 
    /// USUARIO CREO: Alberto Pantoja Hernandez
    /// FECHA CREÓ: 05/08/10 
    /// USUARIO MODIFICO: Juan Alberto Hernández Negrete.
    /// FECHA MODIFICO: 26/Mayo/2011 09:11 a.m.
    /// CAUSA MODIFICACIÓN: Cambiar de usar tags table para el menu a usar listas.
    /// *****************************************************************************************************************************
    public String Cerrar_Contenedor_Submenus()
    {
        return "</ul></div>";
    }
    #endregion

    #region (Métodos)
    /// ****************************************************************************************************************************
    /// NOMBRE: Crear_Menu_Sistema_SIAS
    /// 
    /// DESCRIPCIÓN: Ejecuta la construcción del menú del sistema SIAS.
    /// 
    /// PARÁMETROS: Nombre_Menu.- Es el nombre que se le ha asignadi al menú.
    /// 
    /// USUARIO CREO: Alberto Pantoja Hernandez
    /// FECHA CREÓ: 05/08/10 
    /// USUARIO MODIFICO: Juan Alberto Hernández Negrete.
    /// FECHA MODIFICO: 26/Mayo/2011 09:11 a.m.
    /// CAUSA MODIFICACIÓN: Cambiar de usar tags table para el menu a usar listas.
    /// *****************************************************************************************************************************
    public void Crear_Menu_Sistema_SIAS()
    {
        Cls_Apl_Cat_Roles_Business Obj_Roles = new Cls_Apl_Cat_Roles_Business();//Variable de conexion a la capa de negocios.
        DataTable Dt_Menus_Submenus = null;//Variable que almacenara los menus del sistema-
        StringBuilder MENU_SISTEMA = new StringBuilder();//Variable que almacenara el menú completo del sistema.
        DataRow[] Submenus = null;//Variable que almacena los submenus de cada menú.

        try
        {
            if (Cls_Sessiones.Rol_ID != null) {
                Obj_Roles.P_Rol_ID = Cls_Sessiones.Rol_ID;
                Dt_Menus_Submenus = Obj_Roles.Consulta_Menus_Rol();
                Cls_Sessiones.Menu_Control_Acceso = Dt_Menus_Submenus;
            }

            MENU_SISTEMA.Append("<div class='glossymenu'>");

            if(Dt_Menus_Submenus is DataTable){
                if(Dt_Menus_Submenus.Rows.Count > 0){
                    foreach(DataRow MENU in Dt_Menus_Submenus.Rows){
                        if(MENU is DataRow){
                            if (!String.IsNullOrEmpty(MENU[Apl_Cat_Menus.Campo_Parent_ID].ToString())){
                                if (MENU[Apl_Cat_Menus.Campo_Parent_ID].ToString().Equals("0")) {
                                    //Creamos el menú.
                                    MENU_SISTEMA.Append(Generar_Menu(MENU[Apl_Cat_Menus.Campo_Menu_Descripcion].ToString()));
                                    //Abrimos el contenedor de los submenus.
                                    MENU_SISTEMA.Append(Abrir_Contenedor_Submenus());
                                    //Consultamos los submenus del menu.
                                    Submenus = Dt_Menus_Submenus.Select(Apl_Cat_Menus.Campo_Parent_ID + "=" + 
                                        MENU[Apl_Cat_Menus.Campo_Menu_ID].ToString());

                                    if (Submenus.Length > 0) {
                                        foreach (DataRow SUBMENUS in Submenus) {
                                            //Creamos los submenus.
                                            MENU_SISTEMA.Append(Generar_Submenu(
                                                SUBMENUS[Apl_Cat_Menus.Campo_Menu_Descripcion].ToString(),
                                                SUBMENUS[Apl_Cat_Menus.Campo_URL_Link].ToString(),
                                                SUBMENUS[Apl_Cat_Menus.Campo_Menu_ID].ToString())
                                                );
                                        }
                                    }
                                    //Cerramos el contenedor de los sumenus.
                                    MENU_SISTEMA.Append(Cerrar_Contenedor_Submenus())   ;
                                }
                            }
                        }
                    }
                }
            }
            MENU_SISTEMA.Append("</div>");
            Lbl_Menu.Text = MENU_SISTEMA.ToString();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al ejecutar la construcción del menú del sistema. Error: [" + Ex.Message + "]");
        }
    }
    #endregion
}
