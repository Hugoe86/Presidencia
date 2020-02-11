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
using Presidencia.Menus.Negocios;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.Collections.Generic;
using Presidencia.Roles.Negocio;  

public partial class paginas_Paginas_Generales_Frm_Apl_Cat_Menus : System.Web.UI.Page
{
    #region (Init/Load)
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Validan que exista una session activa al ingresar a la página de antiguedad sindicatos.
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                ViewState["SortDirection"] = "ASC";
            }
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion

    #region (Metodos)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 14-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            Cargar_Combo_Menus();           //Consulta los menus que estan dados de alta en la base de datos
            Limpia_Controles();             //Limpia los controles del forma
            Consulta_Menus();               //Consulta todos los menus y submenus que estan dados de alta
            Consultar_Modulos();            //Consulta los modulos del sistema SIAG. 
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los controles que se encuentran en la forma
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 14-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {
            Txt_Menu_ID.Text = "";
            Txt_Nombre_Menu.Text = "";
            Txt_Orden_Menu.Text = "";
            Txt_Url_Menu.Text = "";
            Cmb_Tipo_Menu.SelectedIndex = 0;
            Cmb_Clasificacion_Menu.SelectedIndex = 0;
            if (Cmb_Menus.Items.Count > 0) Cmb_Menus.SelectedIndex = 0;

            Cmb_Modulo.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 14-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado; ///Indica si el control de la forma va hacer habilitado para utilización del usuario

        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Cmb_Menus.Enabled = false;
                    Cmb_Clasificacion_Menu.Enabled = false;
                    Txt_Orden_Menu.Enabled = false;
                    Txt_Url_Menu.Enabled = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

             //       Configuracion_Acceso("Frm_Apl_Cat_Menus.aspx");
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Cmb_Menus.Enabled = true;
                    Cmb_Clasificacion_Menu.Enabled = true;
                    Txt_Orden_Menu.Enabled = true;
                    Txt_Url_Menu.Enabled = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    break;

                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    if (Cmb_Tipo_Menu.SelectedValue == "SUBMENU")
                    {
                        Cmb_Menus.Enabled = true;
                        Cmb_Clasificacion_Menu.Enabled = true;
                        Txt_Orden_Menu.Enabled = true;
                        Txt_Url_Menu.Enabled = true;
                    }
                    else
                    {
                        Cmb_Menus.Enabled = false;
                        Cmb_Clasificacion_Menu.Enabled = false;
                        Txt_Orden_Menu.Enabled = false;
                        Txt_Url_Menu.Enabled = false;
                    }
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    break;
            }
            Txt_Nombre_Menu.Enabled = Habilitado;
            Cmb_Tipo_Menu.Enabled = Habilitado;
            Txt_Busqueda_Menus.Enabled = !Habilitado;
            Btn_Buscar_Menus.Enabled = !Habilitado;
            Grid_Menus.Enabled = !Habilitado;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            Cmb_Modulo.Enabled = Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Combo_Menus
    /// DESCRIPCION : Consulta los Menus que estan dadas de alta en la DB
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Combo_Menus()
    {
        DataTable Dt_Menus; //Variable que obtendra los datos de la consulta de Menus
        Cls_Apl_Cat_Menus_Negocio Rs_Consulta_Apl_Cat_Menus = new Cls_Apl_Cat_Menus_Negocio(); //Variable de conexión hacia la capa de Negocios        

        try
        {
            Dt_Menus = Rs_Consulta_Apl_Cat_Menus.Consulta_Solo_Menus(); //Consulta todos los Menús que estan dadas de alta en la BD
            Cmb_Menus.DataSource = Dt_Menus;
            Cmb_Menus.DataValueField = "Menu_ID";
            Cmb_Menus.DataTextField = "Menu_Descripcion";
            Cmb_Menus.DataBind();
            Cmb_Menus.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Menus.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Combo_Menus " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Menus
    /// DESCRIPCION : Consulta los Menus y Submenus que estan dadas de alta en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 14-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Menus()
    {
        DataTable Dt_Menus;   //Variable que obtendra los datos de la consulta 
        Cls_Apl_Cat_Menus_Negocio Rs_Consulta_Apl_Cat_Menus = new Cls_Apl_Cat_Menus_Negocio(); //Variable de conexión hacia la capa de Negocios        
        Int32 totalPages = 0;
        try
        {
            if (Txt_Busqueda_Menus.Text != "")
            {
                Rs_Consulta_Apl_Cat_Menus.P_Menu_Descripcion = Txt_Busqueda_Menus.Text;
                Dt_Menus = Rs_Consulta_Apl_Cat_Menus.Consulta_Menus(); //Consulta todos los Menus que coindican con lo proporcionado por el usuario
                Grid_Menus.DataSource = Dt_Menus;
                Grid_Menus.DataBind();
            }
            else
            {
                Dt_Menus = Rs_Consulta_Apl_Cat_Menus.Consulta_Solo_Menus(); //Consulta todos los Menus que coindican con lo proporcionado por el usuario
                Grid_Menus.DataSource = Dt_Menus;
                Grid_Menus.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Menus " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Menu
    /// DESCRIPCION : Da de Alta el Menu con los datos proporcionados por el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 14-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Menu()
    {
        Cls_Apl_Cat_Menus_Negocio Rs_Apl_Cat_Menus = new Cls_Apl_Cat_Menus_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            if (Cmb_Tipo_Menu.SelectedValue == "MENU")
            {
                Rs_Apl_Cat_Menus.P_Parent_ID = 0;
            }
            else
            {
                Rs_Apl_Cat_Menus.P_Parent_ID = Convert.ToInt32(Cmb_Menus.SelectedValue);
                Rs_Apl_Cat_Menus.P_Clasificacion = Convert.ToString(Cmb_Clasificacion_Menu.SelectedValue);
            }
            Rs_Apl_Cat_Menus.P_Menu_Descripcion = Convert.ToString(Txt_Nombre_Menu.Text);
            Rs_Apl_Cat_Menus.P_Url_Link = Convert.ToString(Txt_Url_Menu.Text);
            Rs_Apl_Cat_Menus.P_Orden = Convert.ToInt32(Txt_Orden_Menu.Text);
            Rs_Apl_Cat_Menus.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            Rs_Apl_Cat_Menus.P_Modulo_ID = Cmb_Modulo.SelectedValue.Trim();
            Rs_Apl_Cat_Menus.Alta_Menu(); //Da de alta los datos del Menu proporcionados por el usuario en la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Menús", "alert('El Alta del Menu fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Menu " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Menu
    /// DESCRIPCION : Modifica los datos del Menu con los proporcionados por el usuario en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 14-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Menu()
    {
        Cls_Apl_Cat_Menus_Negocio Rs_Modificar_Apl_Cat_Menus = new Cls_Apl_Cat_Menus_Negocio(); //Variable de conexión hacia la capa de Negocios para envio de datos a modificar
        try
        {
            Rs_Modificar_Apl_Cat_Menus.P_Menu_ID = Convert.ToInt32(Txt_Menu_ID.Text);
            if (Cmb_Tipo_Menu.SelectedValue == "MENU")
            {
                Rs_Modificar_Apl_Cat_Menus.P_Parent_ID = 0;
            }
            else
            {
                Rs_Modificar_Apl_Cat_Menus.P_Parent_ID = Convert.ToInt32(Cmb_Menus.SelectedValue);
                Rs_Modificar_Apl_Cat_Menus.P_Clasificacion = Convert.ToString(Cmb_Clasificacion_Menu.SelectedValue);
            }
            Rs_Modificar_Apl_Cat_Menus.P_Menu_Descripcion = Convert.ToString(Txt_Nombre_Menu.Text);
            Rs_Modificar_Apl_Cat_Menus.P_Url_Link = Convert.ToString(Txt_Url_Menu.Text);
            Rs_Modificar_Apl_Cat_Menus.P_Orden = Convert.ToInt32(Txt_Orden_Menu.Text);
            Rs_Modificar_Apl_Cat_Menus.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            Rs_Modificar_Apl_Cat_Menus.P_Modulo_ID = Cmb_Modulo.SelectedValue.Trim();
            Rs_Modificar_Apl_Cat_Menus.Modificar_Menu(); //Sustituye los datos que se encuentran en la BD por lo que introdujo el usuario
            Consulta_Menus();
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Menu", "alert('La Modificación del Menú fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Menu " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Menu
    /// DESCRIPCION : Elimina los datos del Menu que fue seleccionado por el Usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 14-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Menu()
    {
        Cls_Apl_Cat_Menus_Negocio Rs_Eliminar_Apl_Cat_Menus = new Cls_Apl_Cat_Menus_Negocio(); //Variable de conexión hacia la capa de Negocios para la eliminación de los datos
        try
        {
            Rs_Eliminar_Apl_Cat_Menus.P_Menu_ID = Convert.ToInt32(Txt_Menu_ID.Text);
            Rs_Eliminar_Apl_Cat_Menus.Eliminar_Menu(); //Elimina el Menu que selecciono el usuario de la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Menú", "alert('La Eliminación del Menú fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Eliminar_Menu " + ex.Message.ToString(), ex);
        }
    }

    private void Consultar_Modulos()
    {
        Cls_Apl_Cat_Roles_Business Obj_Roles = new Cls_Apl_Cat_Roles_Business();
        DataTable Dt_Modulos = null; 

        try
        {
            Dt_Modulos = Obj_Roles.Consultar_Modulos_SIAG();

            Cmb_Modulo.DataSource = Dt_Modulos;
            Cmb_Modulo.DataTextField = Apl_Cat_Modulos_Siag.Campo_Nombre;
            Cmb_Modulo.DataValueField = Apl_Cat_Modulos_Siag.Campo_Modulo_ID;
            Cmb_Modulo.DataBind();

            Cmb_Modulo.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Modulo.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los modulos del sistema. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Grid)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Menus_SelectedIndexChanged
    /// DESCRIPCION : Consulta los datos del Menú que selecciono el usuario
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 27/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Menus_SelectedIndexChanged(object sender, EventArgs e)
    {
        String Menu_ID = String.Empty;
        Cls_Apl_Cat_Menus_Negocio Obj_Menus = new Cls_Apl_Cat_Menus_Negocio();
        DataTable Dt_Menus = null;

        try
        {
            Menu_ID = Grid_Menus.SelectedRow.Cells[2].Text;

            if (!String.IsNullOrEmpty(Menu_ID))
            {
                Obj_Menus.P_Menu_ID = Convert.ToInt32(Menu_ID);
                Dt_Menus = Obj_Menus.Consulta_Menus();

                if (Dt_Menus is DataTable)
                {
                    if (Dt_Menus.Rows.Count > 0)
                    {
                        foreach (DataRow _Menu in Dt_Menus.Rows)
                        {
                            if (_Menu is DataRow)
                            {
                                if (!String.IsNullOrEmpty(_Menu[Apl_Cat_Menus.Campo_Menu_ID].ToString()))
                                    Txt_Menu_ID.Text = _Menu[Apl_Cat_Menus.Campo_Menu_ID].ToString();
                                else Txt_Menu_ID.Text = String.Empty;

                                if (!String.IsNullOrEmpty(_Menu[Apl_Cat_Menus.Campo_Parent_ID].ToString()))
                                    Cmb_Menus.SelectedIndex = Cmb_Menus.Items.IndexOf(Cmb_Menus.Items.FindByValue(_Menu[Apl_Cat_Menus.Campo_Parent_ID].ToString()));
                                else Cmb_Menus.SelectedIndex = -1;

                                if (!String.IsNullOrEmpty(_Menu[Apl_Cat_Menus.Campo_Parent_ID].ToString()))
                                    Cmb_Tipo_Menu.SelectedIndex = (_Menu[Apl_Cat_Menus.Campo_Parent_ID].ToString().Equals("0")) ? 2 : 1;
                                else Cmb_Tipo_Menu.SelectedIndex = -1;

                                if (!String.IsNullOrEmpty(_Menu[Apl_Cat_Menus.Campo_Menu_Descripcion].ToString()))
                                    Txt_Nombre_Menu.Text = _Menu[Apl_Cat_Menus.Campo_Menu_Descripcion].ToString();
                                else Txt_Nombre_Menu.Text = String.Empty;

                                if (!String.IsNullOrEmpty(_Menu[Apl_Cat_Menus.Campo_URL_Link].ToString()))
                                    Txt_Url_Menu.Text = _Menu[Apl_Cat_Menus.Campo_URL_Link].ToString();
                                else Txt_Url_Menu.Text = String.Empty;

                                if (!String.IsNullOrEmpty(_Menu[Apl_Cat_Menus.Campo_Orden].ToString()))
                                    Txt_Orden_Menu.Text = _Menu[Apl_Cat_Menus.Campo_Orden].ToString();
                                else Txt_Orden_Menu.Text = String.Empty;

                                if (!String.IsNullOrEmpty(_Menu[Apl_Cat_Menus.Campo_Modulo_ID].ToString()))
                                    Cmb_Modulo.SelectedIndex = Cmb_Modulo.Items.IndexOf(Cmb_Modulo.Items.FindByValue(_Menu[Apl_Cat_Menus.Campo_Modulo_ID].ToString()));
                                else Cmb_Modulo.SelectedIndex = -1;

                                if (!String.IsNullOrEmpty(_Menu[Apl_Cat_Menus.Campo_Clasificacion].ToString()))
                                    Cmb_Clasificacion_Menu.SelectedIndex = Cmb_Clasificacion_Menu.Items.IndexOf(Cmb_Clasificacion_Menu.Items.FindByText(_Menu[Apl_Cat_Menus.Campo_Clasificacion].ToString()));
                                else Cmb_Clasificacion_Menu.SelectedIndex = -1;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Grid_Menus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpia_Controles();                    //Limpia todos los controles de la forma
            Grid_Menus.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Consulta_Menus();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Grid_Menus_Sorting
    /// 
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 27/Abril/2011
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    protected void Grid_Menus_Sorting(object sender, GridViewSortEventArgs e)
    {
        Consulta_Menus();
        DataTable Dt_Menus = (Grid_Menus.DataSource as DataTable);

        if (Dt_Menus != null)
        {
            DataView Dv_Menus = new DataView(Dt_Menus);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Menus.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Menus.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Menus.DataSource = Dv_Menus;
            Grid_Menus.DataBind();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Menus_RowDataBound
    ///DESCRIPCIÓN: 
    ///
    ///PARAMETROS:  
    ///CREO:Juan Alberto Hernández Negrete.
    ///FECHA_CREO: 27/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Menus_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Cls_Apl_Cat_Menus_Negocio Obj_Roles = new Cls_Apl_Cat_Menus_Negocio();//Variable de conexión a la capa de negocios.
        DataTable Dt_SubMenus = null;//Variable que almacena la lista de submenús que tiene el menú.
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                //Obtenemos el grid que almacenara el grid que contiene los submenús del menú.
                GridView Grid_SubMenus = (GridView)e.Row.Cells[4].FindControl("Grid_Submenus");

                Obj_Roles.P_Parent_ID = Convert.ToInt32(e.Row.Cells[2].Text.Trim());
                Dt_SubMenus = Obj_Roles.Consulta_Menus();      //Consultamos los submenús que tiene el parent_id del menú. 

                Grid_SubMenus.DataSource = Dt_SubMenus;
                Grid_SubMenus.DataBind();

            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Menus_SelectedIndexChanged
    /// DESCRIPCION : Consulta los datos del Menú que selecciono el usuario
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 27/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Submenus_SelectedIndexChanged(object sender, EventArgs e)
    {
        String Menu_ID = String.Empty;
        Cls_Apl_Cat_Menus_Negocio Obj_Menus = new Cls_Apl_Cat_Menus_Negocio();
        DataTable Dt_Menus = null;

        try
        {
            Menu_ID = ((GridView)sender).SelectedRow.Cells[1].Text;

            if (!String.IsNullOrEmpty(Menu_ID))
            {
                Obj_Menus.P_Menu_ID = Convert.ToInt32(Menu_ID);
                Dt_Menus = Obj_Menus.Consulta_Menus();

                if (Dt_Menus is DataTable)
                {
                    if (Dt_Menus.Rows.Count > 0)
                    {
                        foreach (DataRow _Menu in Dt_Menus.Rows)
                        {
                            if (_Menu is DataRow)
                            {
                                if (!String.IsNullOrEmpty(_Menu[Apl_Cat_Menus.Campo_Menu_ID].ToString()))
                                    Txt_Menu_ID.Text = _Menu[Apl_Cat_Menus.Campo_Menu_ID].ToString();
                                else Txt_Menu_ID.Text = String.Empty;

                                if (!String.IsNullOrEmpty(_Menu[Apl_Cat_Menus.Campo_Parent_ID].ToString()))
                                    Cmb_Menus.SelectedIndex = Cmb_Menus.Items.IndexOf(Cmb_Menus.Items.FindByValue(_Menu[Apl_Cat_Menus.Campo_Parent_ID].ToString()));
                                else Cmb_Menus.SelectedIndex = -1;

                                if (!String.IsNullOrEmpty(_Menu[Apl_Cat_Menus.Campo_Parent_ID].ToString()))
                                    Cmb_Tipo_Menu.SelectedIndex = (_Menu[Apl_Cat_Menus.Campo_Parent_ID].ToString().Equals("0")) ? 2 : 1;
                                else Cmb_Tipo_Menu.SelectedIndex = -1;

                                if (!String.IsNullOrEmpty(_Menu[Apl_Cat_Menus.Campo_Menu_Descripcion].ToString()))
                                    Txt_Nombre_Menu.Text = _Menu[Apl_Cat_Menus.Campo_Menu_Descripcion].ToString();
                                else Txt_Nombre_Menu.Text = String.Empty;

                                if (!String.IsNullOrEmpty(_Menu[Apl_Cat_Menus.Campo_URL_Link].ToString()))
                                    Txt_Url_Menu.Text = _Menu[Apl_Cat_Menus.Campo_URL_Link].ToString();
                                else Txt_Url_Menu.Text = String.Empty;

                                if (!String.IsNullOrEmpty(_Menu[Apl_Cat_Menus.Campo_Orden].ToString()))
                                    Txt_Orden_Menu.Text = _Menu[Apl_Cat_Menus.Campo_Orden].ToString();
                                else Txt_Orden_Menu.Text = String.Empty;

                                if (!String.IsNullOrEmpty(_Menu[Apl_Cat_Menus.Campo_Modulo_ID].ToString()))
                                    Cmb_Modulo.SelectedIndex = Cmb_Modulo.Items.IndexOf(Cmb_Modulo.Items.FindByValue(_Menu[Apl_Cat_Menus.Campo_Modulo_ID].ToString()));
                                else Cmb_Modulo.SelectedIndex = -1;

                                if (!String.IsNullOrEmpty(_Menu[Apl_Cat_Menus.Campo_Clasificacion].ToString()))
                                    Cmb_Clasificacion_Menu.SelectedIndex = Cmb_Clasificacion_Menu.Items.IndexOf(Cmb_Clasificacion_Menu.Items.FindByText(_Menu[Apl_Cat_Menus.Campo_Clasificacion].ToString()));
                                else Cmb_Clasificacion_Menu.SelectedIndex = -1;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Grid_Submenus_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow){
            e.Row.Attributes.Add("onmouseover","this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#507CD1';this.style.color='#FFFFFF'");
            e.Row.Attributes.Add("onmouseout","this.style.backgroundColor=this.originalstyle;this.style.color=this.originalstyle;");
        }
    }

    protected void Grid_Menus_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#507CD1';this.style.color='#FFFFFF'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;this.style.color=this.originalstyle;");
        }
    }



    #endregion

    #region (Eventos)
    protected void Btn_Buscar_Menus_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Consulta_Menus();    //Consulta los Menus que coincidan con la /// DESCRIPCION porporcionada por el usuario
            Limpia_Controles();  //Limpia los controles de la forma
            //Si no se encontraron Menus con una /// DESCRIPCION similar a la proporcionada por el usuario entonces manda un mensaje al usuario
            if (Grid_Menus.Rows.Count <= 0)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encontraron Menus con alguna /// DESCRIPCION proporcionada <br>";
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Btn_Nuevo_Click(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                Limpia_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
            }
            else
            {
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces da de alta los mismo en la base de datos
                if (Txt_Nombre_Menu.Text != "" & Cmb_Tipo_Menu.SelectedIndex > 0 & Txt_Orden_Menu.Text != "")
                {
                    if (Cmb_Tipo_Menu.SelectedValue == "SUBMENU")
                    {
                        if (Cmb_Menus.SelectedIndex <= 0)
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Menú al cual pertenece el Submenu";
                            return;
                        }
                        if (Txt_Url_Menu.Text == "")
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Url al cual pertenece el Submenu";
                            return;
                        }
                        if (Cmb_Clasificacion_Menu.SelectedIndex <= 0)
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La clasificación al cual pertenece el Submenu";
                            return;
                        }
                    }
                    Alta_Menu(); //Da de alta los datos proporcionados por el usuario
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                    if (Txt_Nombre_Menu.Text == "") Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Nombre del Menú <br>";
                    if (Cmb_Tipo_Menu.SelectedIndex <= 0) Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Tipo del Menu <br>";
                    if (Txt_Orden_Menu.Text == "") Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Orden del Menú <br>";
                }
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Btn_Modificar_Click(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                if (Txt_Menu_ID.Text != "")
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el Menú que desea modificar sus datos <br>";
                }
            }
            else
            {
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces modifica estos en la BD
                if (Txt_Nombre_Menu.Text != "" & Cmb_Tipo_Menu.SelectedIndex > 0 & Txt_Orden_Menu.Text != "")
                {
                    if (Cmb_Tipo_Menu.SelectedValue == "SUBMENU")
                    {
                        if (Cmb_Menus.SelectedIndex <= 0)
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Menú al cual pertenece el Submenu";
                            return;
                        }
                        if (Txt_Url_Menu.Text == "")
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Url al cual pertenece el Submenu";
                            return;
                        }
                        if (Cmb_Clasificacion_Menu.SelectedIndex <= 0)
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La clasificación al cual pertenece el Submenu";
                            return;
                        }
                    }
                    Modificar_Menu(); //Modifica los datos del Menu con los datos proporcionados por el usuario
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                    if (Txt_Nombre_Menu.Text == "") Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Nombre del Menú <br>";
                    if (Cmb_Tipo_Menu.SelectedIndex <= 0) Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Tipo del Menu <br>";
                    if (Txt_Orden_Menu.Text == "") Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Orden del Menú <br>";
                }
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Btn_Eliminar_Click(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            //Si el usuario selecciono un Menu entonces lo elimina de la base de datos
            if (Txt_Menu_ID.Text != "")
            {
                Eliminar_Menu(); //Elimina el Menu que fue seleccionado por el usuario
            }
            //Si el usuario no selecciono algún Menu manda un mensaje indicando que es necesario que seleccione algun para
            //poder eliminar
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione el Menú que desea eliminar <br>";
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Salir")
            {
                Session.Remove("Consulta_Menus");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Inicializa_Controles();//Habilita los controles para la siguiente operación del usuario en el catálogo
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Cmb_Tipo_Menu_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cmb_Menus.SelectedIndex = 0;
        Cmb_Clasificacion_Menu.SelectedIndex = 0;
        Txt_Orden_Menu.Text = "";
        Cmb_Menus.Enabled = true;
        Cmb_Clasificacion_Menu.Enabled = true;
        Txt_Orden_Menu.Enabled = true;
        Txt_Url_Menu.Enabled = true;
        if (Cmb_Tipo_Menu.SelectedValue == "MENU")
        {
            Txt_Orden_Menu.Text = "0";
            Txt_Url_Menu.Text = "";
            Txt_Orden_Menu.Enabled = false;
            Txt_Url_Menu.Enabled = false;
            Cmb_Menus.Enabled = false;
            Cmb_Clasificacion_Menu.Enabled = false;
        }
    }
    #endregion

    #region (Control Acceso Pagina)
    /// *****************************************************************************************************************************
    /// NOMBRE: Configuracion_Acceso
    /// 
    /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
    /// 
    /// PARÁMETROS: No Áplica.
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 23/Mayo/2011 10:43 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************************************************
    protected void Configuracion_Acceso(String URL_Pagina)
    {
        List<ImageButton> Botones = new List<ImageButton>();//Variable que almacenara una lista de los botones de la página.
        DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

        try
        {
            //Agregamos los botones a la lista de botones de la página.
            Botones.Add(Btn_Nuevo);
            Botones.Add(Btn_Modificar);
            Botones.Add(Btn_Eliminar);
            Botones.Add(Btn_Buscar_Menus);

            if (!String.IsNullOrEmpty(Request.QueryString["PAGINA"]))
            {
                if (Es_Numero(Request.QueryString["PAGINA"].Trim()))
                {
                    //Consultamos el menu de la página.
                    Dr_Menus = Cls_Sessiones.Menu_Control_Acceso.Select("MENU_ID=" + Request.QueryString["PAGINA"]);

                    if (Dr_Menus.Length > 0)
                    {
                        //Validamos que el menu consultado corresponda a la página a validar.
                        if (Dr_Menus[0][Apl_Cat_Menus.Campo_URL_Link].ToString().Contains(URL_Pagina))
                        {
                            Cls_Util.Configuracion_Acceso_Sistema_SIAS(Botones, Dr_Menus[0]);//Habilitamos la configuracón de los botones.
                        }
                        else
                        {
                            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    }
                }
                else
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
            }
            else
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al habilitar la configuración de accesos a la página. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Es_Numero(String Cadena)
    {
        Boolean Resultado = true;
        Char[] Array = Cadena.ToCharArray();
        try
        {
            for (int index = 0; index < Array.Length; index++)
            {
                if (!Char.IsDigit(Array[index])) return false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Validar si es un dato numerico. Error [" + Ex.Message + "]");
        }
        return Resultado;
    }
    #endregion
}
