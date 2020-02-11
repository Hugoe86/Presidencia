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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Menus.Negocios;
using Presidencia.Ayudante_JQuery;

public partial class paginas_Paginas_Generales_Frm_Controlador_Orden_Menus : System.Web.UI.Page
{
    #region (Load/Init)
    /// **********************************************************************************************************
    /// NOMBRE: Page_Load
    /// 
    /// DESCRIPCIÓN: Carga la configuración inicial de la página.
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 20/Octubre/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// **********************************************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Ejecutar_Load_Pagina();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar la configuración inicial de la pagina. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Control Peticiones Pagina)
    /// **********************************************************************************************************
    /// NOMBRE: Ejecutar_Load_Pagina
    /// 
    /// DESCRIPCIÓN: Control de peticiones de la página.
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 20/Octubre/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// **********************************************************************************************************
    protected void Ejecutar_Load_Pagina()
    {
        String JSON = String.Empty;
        String Opcion = Request.QueryString["Opcion"];
        String Nombre_Tabla = Request.QueryString["Tabla"]; ;
        String Respuesta = String.Empty;

        try
        {
            switch (Opcion)
            {
                case "Consultar_Menus":
                    Respuesta = Ayudante_JQuery.Crear_Tabla_Formato_JSON(Consultar_Menus(Nombre_Tabla));
                    break;
                case "Consultar_Sub_Menus":
                    Respuesta = Ayudante_JQuery.Crear_Tabla_Formato_JSON(Consultar_Sub_Menus(Nombre_Tabla));
                    break;
                case "Actualizar_Orden_Menus":
                    Respuesta = Actualizar_Orden_Menus();
                    break;
                default:
                    break;
            }
            Response.Clear();
            Response.ContentType = "application/json";
            Response.Write(Respuesta);
            Response.Flush();
            Response.Close();
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    #endregion

    #region (Consultas)
    /// **********************************************************************************************************
    /// NOMBRE: Consultar_Menus
    /// 
    /// DESCRIPCIÓN: Consulta los menús que se encuentran registrados en el sistema.
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 20/Octubre/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// **********************************************************************************************************
    protected DataTable Consultar_Menus(String Nombre_Tabla)
    {
        Cls_Apl_Cat_Menus_Negocio Obj_Menus = new Cls_Apl_Cat_Menus_Negocio();
        DataTable Dt_Menus = null;

        try
        {
            Obj_Menus.P_Parent_ID = 0;
            Dt_Menus = Obj_Menus.Consulta_Menus_Submenus();
            Dt_Menus.TableName = Nombre_Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los menus. Error: [" + Ex.Message + "]");
        }
        return Dt_Menus;
    }
    /// **********************************************************************************************************
    /// NOMBRE: Consultar_Sub_Menus
    /// 
    /// DESCRIPCIÓN: Consulta los sub menús que se encuentran registrados en el sistema.
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 20/Octubre/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// **********************************************************************************************************
    protected DataTable Consultar_Sub_Menus(String Nombre_Tabla)
    {
        Cls_Apl_Cat_Menus_Negocio Obj_Menus = new Cls_Apl_Cat_Menus_Negocio();
        DataTable Dt_Menus = null;
        String Parent_ID = Request.QueryString["Parent_ID"];

        try
        {
            Obj_Menus.P_Parent_ID = Convert.ToInt32(Parent_ID);

            Dt_Menus = Obj_Menus.Consulta_Menus_Submenus();
            Dt_Menus.TableName = Nombre_Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los menus. Error: [" + Ex.Message + "]");
        }
        return Dt_Menus;
    }
    #endregion

    #region (Operación)
    /// **********************************************************************************************************
    /// NOMBRE: Actualizar_Orden_Menus
    /// 
    /// DESCRIPCIÓN: Ejecuta la actualización del orden de los menus.
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 20/Octubre/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// **********************************************************************************************************
    protected String Actualizar_Orden_Menus()
    {
        Cls_Apl_Cat_Menus_Negocio Obj_Menus = new Cls_Apl_Cat_Menus_Negocio();
        DataTable Dt_Menus_Ordenados = null;
        String Respuesta = "NO";
        try
        {
            Dt_Menus_Ordenados = Crear_Tabla_Menus_Nuevo_Orden();

            if (Dt_Menus_Ordenados is DataTable)
            {
                if (Dt_Menus_Ordenados.Rows.Count > 0)
                {
                    if (Obj_Menus.Actualizar_Orden_Menus(Dt_Menus_Ordenados))
                    {
                        Respuesta = "SI";
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al actualizar el orden de los menus. Error: [" + Ex.Message + "]");
        }
        return Respuesta;
    }
    /// **********************************************************************************************************
    /// NOMBRE: Consultar_Menus
    /// 
    /// DESCRIPCIÓN: Crea la tabla con el nuevo orden que tendran los menus.
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 20/Octubre/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// **********************************************************************************************************
    protected DataTable Crear_Tabla_Menus_Nuevo_Orden()
    {
        DataTable Dt_Nuevo_Orden_Menus = new DataTable();
        String Menus = Request.QueryString["Orden_Menus"];
        Int32 Contador = 1;

        try
        {
            Dt_Nuevo_Orden_Menus.Columns.Add(Apl_Cat_Menus.Campo_Menu_ID, typeof(String));
            Dt_Nuevo_Orden_Menus.Columns.Add(Apl_Cat_Menus.Campo_Orden, typeof(String));

            String[] Nuevo_Orden_Menus = Menus.Split(new Char[] { ',' });

            if (Nuevo_Orden_Menus.Length > 0)
            {
                foreach (String MENU in Nuevo_Orden_Menus)
                {
                    DataRow Dr_Menu = Dt_Nuevo_Orden_Menus.NewRow();
                    Dr_Menu[Apl_Cat_Menus.Campo_Menu_ID] = MENU;
                    Dr_Menu[Apl_Cat_Menus.Campo_Orden] = (Contador++).ToString();
                    Dt_Nuevo_Orden_Menus.Rows.Add(Dr_Menu);
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al crear la tabla con el nuevo orden de los menus. Error: [" + Ex.Message + "]");
        }
        return Dt_Nuevo_Orden_Menus;
    }
    #endregion
}
