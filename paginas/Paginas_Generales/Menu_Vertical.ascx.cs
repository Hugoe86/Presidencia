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

public partial class paginas_Paginas_Generales_Menu_Vertical : System.Web.UI.UserControl
{

    #region (Init/Load)
    /// ****************************************************************************************************************************
    /// NOMBRE: Page_Load
    /// 
    /// DESCRIPCIÓN: Ejecuta la carga inicial de la página.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: Enero/2012
    /// USUARIO MODIFICO: 
    /// FECHA MODIFICO: 
    /// CAUSA MODIFICACIÓN: 
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

    #region (Modulo)
    /// ****************************************************************************************************************************
    /// NOMBRE: Abrir_Elemento_Modulo
    /// 
    /// DESCRIPCIÓN: Abrimos el módulo para crear los menús que tendrá el mismo.
    /// 
    /// PARÁMETROS: Nombre_Modulo.- Nombre del módulo a generar sus menús.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: Enero/2012
    /// USUARIO MODIFICO: 
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************************************************
    public String Abrir_Elemento_Modulo(String Nombre_Modulo)
    {
        return "<li>&nbsp;&nbsp;&nbsp;<a href='#'>&nbsp;" + Nombre_Modulo + "</a>";
    }
    /// ****************************************************************************************************************************
    /// NOMBRE: Cerrar_Elemento_Modulo
    /// 
    /// DESCRIPCIÓN: Cerramos el módulo una vez creado todos sus menús. 
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: Enero/2012
    /// USUARIO MODIFICO: 
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN: 
    /// *****************************************************************************************************************************
    public String Cerrar_Elemento_Modulo()
    {
        return "</li>";
    }
    #endregion

    #region (Menú)
    /// ****************************************************************************************************************************
    /// NOMBRE: Abrir_Contenedor_Menu
    /// 
    /// DESCRIPCIÓN: Abre el contenedor de los menus.
    /// 
    /// PARÁMETROS: No Áplica.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: Enero/2012
    /// USUARIO MODIFICO: 
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN: 
    /// *****************************************************************************************************************************
    public String Abrir_Contenedor_Menu(String Nombre)
    {
        return "<ul id='" + Nombre + "'>";
    }
    /// ****************************************************************************************************************************
    /// NOMBRE: Abrir_Elemento_Menu
    /// 
    /// DESCRIPCIÓN: Crea el menú y lo nombra según el contenido de los submenus que le pertenceran.
    /// 
    /// PARÁMETROS: Nombre_Menu.- Es el nombre que se le ha asignadi al menú.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: Enero/2012
    /// USUARIO MODIFICO: 
    /// FECHA MODIFICO: 
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************************************************
    public String Abrir_Elemento_Menu(String Nombre_Menu)
    {
        return "<li>&nbsp;&nbsp;&nbsp;<a href='#'>&nbsp;" + Nombre_Menu + "</a>";
    }
    /// ****************************************************************************************************************************
    /// NOMBRE: Cerrar_Elemento_Menu
    /// 
    /// DESCRIPCIÓN: Cierre el elemento del menú.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: Enero/2012
    /// USUARIO MODIFICO: 
    /// FECHA MODIFICO: 
    /// CAUSA MODIFICACIÓN: 
    /// *****************************************************************************************************************************
    public String Cerrar_Elemento_Menu()
    {
        return "</li>";
    }
    /// ****************************************************************************************************************************
    /// NOMBRE: Cerrar_Contenedor_Menus
    /// 
    /// DESCRIPCIÓN: Cierra el contenedor de los submenus.
    /// 
    /// PARÁMETROS: No Áplica.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: Enero/2012
    /// USUARIO MODIFICO: 
    /// FECHA MODIFICO: 
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************************************************
    public String Cerrar_Contenedor_Menus()
    {
        return "</ul>";
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
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: Enero/2012
    /// USUARIO MODIFICO: 
    /// FECHA MODIFICO: 
    /// CAUSA MODIFICACIÓN: 
    /// *****************************************************************************************************************************
    public String Abrir_Contenedor_Submenus(String Nombre)
    {
        return "<ul id='" + Nombre + "'>";
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
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: Enero/2012
    /// USUARIO MODIFICO: 
    /// FECHA MODIFICO: 
    /// CAUSA MODIFICACIÓN: 
    /// *****************************************************************************************************************************
    public String Generar_Elemento_Submenu(String Nombre_Submenu, String URL_Menu, String Menu_ID)
    {
        return "<li style='width:217px;'><a href='" + URL_Menu + "?PAGINA=" + Menu_ID + "' id='" + Menu_ID + "'>" + Nombre_Submenu + "</a></li>";
    }
    /// ****************************************************************************************************************************
    /// NOMBRE: Cerrar_Contenedor_Submenus
    /// 
    /// DESCRIPCIÓN: Cierra el contenedor de los submenus.
    /// 
    /// PARÁMETROS: No Áplica.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: Enero/2012
    /// USUARIO MODIFICO: 
    /// FECHA MODIFICO: 
    /// CAUSA MODIFICACIÓN: 
    /// *****************************************************************************************************************************
    public String Cerrar_Contenedor_Submenus()
    {
        return "</ul>";
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
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: Enero/2012
    /// USUARIO MODIFICO: 
    /// FECHA MODIFICO: 
    /// CAUSA MODIFICACIÓN: 
    /// *****************************************************************************************************************************
    public void Crear_Menu_Sistema_SIAS()
    {
        Cls_Apl_Cat_Roles_Business Obj_Roles = new Cls_Apl_Cat_Roles_Business();//Variable de conexion a la capa de negocios.
        DataTable Dt_Menus_Submenus = null;//Variable que almacenara los menus del sistema-
        StringBuilder MENU_SISTEMA = new StringBuilder();//Variable que almacenara el menú completo del sistema.
        DataRow[] Submenus = null;//Variable que almacena los submenus de cada menú.
        DataTable Dt_Modulos = null;//Variable que almacena los modulos que hay en presidencia.
        String Modulo_ID = String.Empty;//Variable que almacena el identificador único del sistema.
        DataRow[] Elementos = null;//Variable que almacena los menús por módulo.

        try
        {
            //Consultamos los módulos que actualmente se encuentran desarrollados.
            Dt_Modulos = Obj_Roles.Consultar_Modulos_SIAG();

            //Identificamos el rol que actualmente se encuentra logueado al sistema.
            if (Cls_Sessiones.Rol_ID != null)
            {
                Obj_Roles.P_Rol_ID = Cls_Sessiones.Rol_ID;
                Dt_Menus_Submenus = Obj_Roles.Consulta_Menus_Rol();
                Cls_Sessiones.Menu_Control_Acceso = Dt_Menus_Submenus;
            }

            //Creamos la raíz de la estructura de nuestro menú.
            MENU_SISTEMA.Append("<ul id='menu'>");            

            if (Dt_Modulos is DataTable)
            {
                if (Dt_Modulos.Rows.Count > 0)
                {
                    foreach (DataRow MODULO in Dt_Modulos.Rows)
                    {
                        if (MODULO is DataRow)
                        {
                            if (!String.IsNullOrEmpty(MODULO[Apl_Cat_Modulos_Siag.Campo_Modulo_ID].ToString()))
                            {
                                //Obtenemos el módulo del cuál consultaremos sus menús. 
                                Modulo_ID = MODULO[Apl_Cat_Modulos_Siag.Campo_Modulo_ID].ToString();
                                //Consultamos los menús del módulo actual (Actual en el recorrido del ciclo).
                                Elementos = Dt_Menus_Submenus.Select(Apl_Cat_Menus.Campo_Modulo_ID + "='" + Modulo_ID + "'");

                                //Vallidamos que se hallan encontrado menús en el módulo actual (Actual en el recorrido del ciclo)
                                if (Elementos.Length > 0) {
                                    //Obtenemos el nombre del módulo [Nomina - Compras - Almacen - Ctrl Patrimonial].
                                    MENU_SISTEMA.Append(Abrir_Elemento_Modulo(MODULO[Apl_Cat_Modulos_Siag.Campo_Nombre].ToString()));

                                    //Abrimos el contenedor de los menus.
                                    MENU_SISTEMA.Append(Abrir_Contenedor_Menu(MODULO[Apl_Cat_Modulos_Siag.Campo_Nombre].ToString()));

                                    foreach (DataRow MENU in Elementos)
                                    { 
                                        if (MENU is DataRow)
                                        {
                                            if (!String.IsNullOrEmpty(MENU[Apl_Cat_Menus.Campo_Parent_ID].ToString()))
                                            {
                                                if (MENU[Apl_Cat_Menus.Campo_Parent_ID].ToString().Equals("0"))
                                                {
                                                    //Abrimos el menú [Catálogos Compras - Catálogo Nómina].
                                                    MENU_SISTEMA.Append(Abrir_Elemento_Menu(MENU[Apl_Cat_Menus.Campo_Menu_Descripcion].ToString()));

                                                    //Abrimos el contenedor de los submenus.
                                                    MENU_SISTEMA.Append(Abrir_Contenedor_Submenus(MENU[Apl_Cat_Menus.Campo_Menu_Descripcion].ToString()));

                                                    //Consultamos los submenus del menu.
                                                    Submenus = Dt_Menus_Submenus.Select(Apl_Cat_Menus.Campo_Parent_ID + "=" +
                                                        MENU[Apl_Cat_Menus.Campo_Menu_ID].ToString());

                                                    if (Submenus.Length > 0)
                                                    {
                                                        foreach (DataRow SUBMENUS in Submenus)
                                                        {
                                                            //Creamos los submenus.
                                                            MENU_SISTEMA.Append(Generar_Elemento_Submenu(
                                                                SUBMENUS[Apl_Cat_Menus.Campo_Menu_Descripcion].ToString(),
                                                                SUBMENUS[Apl_Cat_Menus.Campo_URL_Link].ToString(),
                                                                SUBMENUS[Apl_Cat_Menus.Campo_Menu_ID].ToString())
                                                                );
                                                        }
                                                    }
                                                    //Cerramos el contenedor de los sumenus.
                                                    MENU_SISTEMA.Append(Cerrar_Contenedor_Submenus());
                                                    //Cerramos el menú.
                                                    MENU_SISTEMA.Append(Cerrar_Elemento_Menu());
                                                }
                                            }
                                        }
                                    }
                                    //Cerremos el contenedor de los menus.
                                    MENU_SISTEMA.Append(Cerrar_Contenedor_Menus());
                                    //Cerramos el módulo.
                                    Cerrar_Elemento_Modulo();
                                }
                            }
                        }
                    }
                }
            }

            //Cerramos el menú raíz.         
            MENU_SISTEMA.Append("</ul>");

            //Ligamos el menú construido con el ctrl que lo mostrara en pantalla al usuario.
            Lbl_Menu.Text =MENU_SISTEMA.ToString();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al ejecutar la construcción del menú del sistema. Error: [" + Ex.Message + "]");
        }
    }
    #endregion
}
