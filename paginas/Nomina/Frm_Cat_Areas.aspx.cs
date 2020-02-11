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
using Presidencia.Areas.Negocios;
using Presidencia.Dependencias.Negocios;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Collections.Generic;

public partial class paginas_Nomina_Frm_Cat_Areas : System.Web.UI.Page
{
    #region (Page Load)
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                ViewState["SortDirection"] = "ASC";
            }
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
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            Cargar_Combo_Dependencias(); //Consulta todas las dependencias que fueron dadas de alta en la BD
            Limpia_Controles(); //Limpia los controles del forma
            Consulta_Areas(); //Consulta todas las áreas que fueron dadas de alta en la BD
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
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {
            Txt_Area_ID.Text = "";
            Txt_Nombre_Area.Text = "";
            Txt_Comentarios_Area.Text = "";
            Txt_Busqueda_Area.Text = "";
            Cmb_Estatus_Area.SelectedIndex = 0;
            Cmb_Dependencias_Area.SelectedIndex = 0;
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
    /// FECHA_CREO  : 25-Agosto-2010
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
                    Cmb_Estatus_Area.SelectedIndex = 0;
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Cmb_Estatus_Area.Enabled = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                    Configuracion_Acceso("Frm_Cat_Areas.aspx");
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Cmb_Estatus_Area.SelectedIndex = 0;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Cmb_Estatus_Area.Enabled = false;
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
                    Cmb_Estatus_Area.Enabled = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    break;
            }
            Txt_Nombre_Area.Enabled = Habilitado;
            Txt_Comentarios_Area.Enabled = Habilitado;
            Txt_Busqueda_Area.Enabled = !Habilitado;
            Btn_Buscar_Area.Enabled = !Habilitado;
            Cmb_Dependencias_Area.Enabled = Habilitado;
            Grid_Areas.Enabled = !Habilitado;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Combo_Dependencias
    /// DESCRIPCION : Consulta las Dependencias que estan dadas de alta en la base de datos
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Combo_Dependencias()
    {
        DataTable Dt_Dependencias; //Variable que obtendra los datos de la consulta        
        Cls_Cat_Dependencias_Negocio Rs_Consulta_Cat_Dependencias = new Cls_Cat_Dependencias_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {
            Dt_Dependencias = Rs_Consulta_Cat_Dependencias.Consulta_Dependencias(); //Consulta todas las dependencias que estan dadas de alta en la BD
            Cmb_Dependencias_Area.DataSource = Dt_Dependencias;
            Cmb_Dependencias_Area.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
            Cmb_Dependencias_Area.DataTextField = Cat_Dependencias.Campo_Nombre;
            Cmb_Dependencias_Area.DataBind();
            Cmb_Dependencias_Area.Items.Insert(0, "---------------------------------------------------------- < SELECCIONE > ----------------------------------------------------------");
            Cmb_Dependencias_Area.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Combo_Dependencias " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Areas
    /// DESCRIPCION : Consulta las áreas que estan dadas de alta en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Areas()
    {
        Cls_Cat_Areas_Negocio Rs_Consulta_Cat_Areas = new Cls_Cat_Areas_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Areas; //Variable que obtendra los datos de la consulta 

        try
        {
            if (Txt_Busqueda_Area.Text != "")
            {
                Rs_Consulta_Cat_Areas.P_Nombre = Txt_Busqueda_Area.Text;
            }
            Dt_Areas = Rs_Consulta_Cat_Areas.Consulta_Datos_Areas(); //Consulta todas las áreas con sus datos generales            
            Session["Consulta_Areas"] = Dt_Areas;
            Llena_Grid_Areas();
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Areas " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llena_Grid_Areas
    /// DESCRIPCION : Llena el grid con las áreas que se encuentran en la base de datos
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llena_Grid_Areas()
    {
        DataTable Dt_Areas; //Variable que obtendra los datos de la consulta 
        try
        {
            Grid_Areas.DataBind();
            Dt_Areas = (DataTable)Session["Consulta_Areas"];
            Grid_Areas.DataSource = Dt_Areas;
            Grid_Areas.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Areas " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Areas
    /// DESCRIPCION : Da de Alta el Área con los datos proporcionados por el usuario
    /// PARAMETROS  : 
    /// FECHA_CREO  : 25-Agosto-2010
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Areas()
    {
        Cls_Cat_Areas_Negocio Rs_Alta_Cat_Areas = new Cls_Cat_Areas_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            Rs_Alta_Cat_Areas.P_Dependencia_ID = Cmb_Dependencias_Area.SelectedValue;
            Rs_Alta_Cat_Areas.P_Nombre = Txt_Nombre_Area.Text;
            Rs_Alta_Cat_Areas.P_Estatus = Cmb_Estatus_Area.SelectedValue;
            Rs_Alta_Cat_Areas.P_Comentarios = Txt_Comentarios_Area.Text;
            Rs_Alta_Cat_Areas.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

            Rs_Alta_Cat_Areas.Alta_Area(); //Da de alta los datos del Área proporcionados por el usuario en la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Áreas", "alert('El Alta del Área fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Areas " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Area
    /// DESCRIPCION : Modifica los datos del Área con los proporcionados por el usuario en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Area()
    {
        Cls_Cat_Areas_Negocio Rs_Modificar_Cat_Areas = new Cls_Cat_Areas_Negocio(); //Variable de conexión hacia la capa de Negoccios para envio de datos a modificar
        try
        {
            Rs_Modificar_Cat_Areas.P_Area_ID = Txt_Area_ID.Text;
            Rs_Modificar_Cat_Areas.P_Dependencia_ID = Cmb_Dependencias_Area.SelectedValue;
            Rs_Modificar_Cat_Areas.P_Nombre = Txt_Nombre_Area.Text;
            Rs_Modificar_Cat_Areas.P_Estatus = Cmb_Estatus_Area.SelectedValue;
            Rs_Modificar_Cat_Areas.P_Comentarios = Txt_Comentarios_Area.Text;
            Rs_Modificar_Cat_Areas.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

            Rs_Modificar_Cat_Areas.Modificar_Area(); //Sustituye los datos que se encuentran en la BD por lo que introdujo el usuario
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Áreas", "alert('La Modificación del Área fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Areas " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Area
    /// DESCRIPCION : Elimina los datos del Área que fue seleccionada por el Usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Area()
    {
        Cls_Cat_Areas_Negocio Rs_Eliminar_Cat_Areas = new Cls_Cat_Areas_Negocio(); //Variable de conexión hacia la capa de Negocios para la eliminación de los datos
        try
        {
            Rs_Eliminar_Cat_Areas.P_Area_ID = Txt_Area_ID.Text;
            Rs_Eliminar_Cat_Areas.Elimina_Area(); //Elimina el Área que selecciono el usuario de la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Áreas", "alert('La Eliminación del Área fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Eliminar_Area " + ex.Message.ToString(), ex);
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
            Botones.Add(Btn_Buscar_Area);

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

    #region (Grid)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Areas_SelectedIndexChanged
    /// DESCRIPCION : Consulta los datos del área que selecciono el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Areas_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Areas_Negocio Rs_Consulta_Cat_Areas = new Cls_Cat_Areas_Negocio(); //Variable de conexión a la capa de Negocios para la consulta de los datos del área
        DataTable Dt_Areas; //Variable que obtendra los datos de la consulta

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Rs_Consulta_Cat_Areas.P_Area_ID = Grid_Areas.SelectedRow.Cells[1].Text;
            Dt_Areas = Rs_Consulta_Cat_Areas.Consulta_Datos_Areas(); //Consulta los datos del área que fue seleccionada por el usuario
            if (Dt_Areas.Rows.Count > 0)
            {
                //Agrega los valores de los campos a los controles correspondientes de la forma
                foreach (DataRow Registro in Dt_Areas.Rows)
                {
                    Txt_Area_ID.Text = Registro[Cat_Areas.Campo_Area_ID].ToString();
                    Txt_Nombre_Area.Text = Registro[Cat_Areas.Campo_Nombre].ToString();
                    Txt_Comentarios_Area.Text = Registro[Cat_Areas.Campo_Comentarios].ToString();
                    Cmb_Estatus_Area.SelectedValue = Registro[Cat_Areas.Campo_Estatus].ToString();
                    Cmb_Dependencias_Area.SelectedValue = Registro[Cat_Areas.Campo_Dependencia_ID].ToString();
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
    protected void Grid_Areas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpia_Controles(); //Limpia todos los controles de la forma
            Grid_Areas.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Llena_Grid_Areas(); //Carga las áreas que estan asignadas a la página seleccionada
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Grid_Areas_Sorting(object sender, GridViewSortEventArgs e)
    {
        Consulta_Areas();
        DataTable Dt_Areas = (Grid_Areas.DataSource as DataTable);

        if (Dt_Areas != null)
        {
            DataView Dv_Calendario_Nominas = new DataView(Dt_Areas);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Calendario_Nominas.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Calendario_Nominas.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Areas.DataSource = Dv_Calendario_Nominas;
            Grid_Areas.DataBind();
        }
    }
    #endregion

    #region (Eventos)
    protected void Btn_Nuevo_Click(object sender, EventArgs e)
    {
        DataTable Dt_Areas_Consulta;
        try
        {
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                Limpia_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
            }
            else
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces da de alta los mismo en la base de datos
                if (Cmb_Dependencias_Area.SelectedIndex > 0 & Txt_Nombre_Area.Text != "" & Txt_Comentarios_Area.Text.Length <= 250)
                {
                    Dt_Areas_Consulta = (DataTable)Session["Consulta_Areas"];
                    String filtro = "[NOMBRE]" + " = '" + Txt_Nombre_Area.Text.Trim() + "' AND [DEPENDENCIA] = '" + Cmb_Dependencias_Area.SelectedItem.Text + "'";
                    DataRow[] RenglonesEncontrados = Dt_Areas_Consulta.Select(filtro);
                    //RenglonesEncontrados = dt.Select (@"Nombre_de_columna = 'dato a buscar' );

                    if (RenglonesEncontrados.Length > 0)
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "Ya se encuentra un Área registrada con el nombre: '" + Txt_Nombre_Area.Text.Trim() + "' En la unidad responsable: '" + Cmb_Dependencias_Area.SelectedItem.Text.Trim()+"'";
                    }
                    else
                    {
                        Alta_Areas(); //Da de alta los datos proporcionados por el usuario
                    }
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                    if (Txt_Nombre_Area.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Nombre del Área <br>";
                    }
                    if (Cmb_Dependencias_Area.SelectedIndex <= 0)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccionar la Dependencia a la cual pertenece el Área <br>";
                    }
                    if (Txt_Comentarios_Area.Text.Length > 250)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Los comentarios proporcionados no deben ser mayor a 250 caracteres <br>";
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

    protected void Btn_Modificar_Click(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                if (Txt_Area_ID.Text != "")
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el Área que desea modificar sus datos <br>";
                }
            }
            else
            {
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces modifica estos valores en la base de datos
                if (Cmb_Dependencias_Area.SelectedIndex > 0 & Txt_Nombre_Area.Text != "" & Txt_Comentarios_Area.Text.Length <= 250)
                {
                    Modificar_Area(); //Modifica los datos del Área con los datos proporcionados por el usuario
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                    if (Txt_Nombre_Area.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Nombre del Área <br>";
                    }
                    if (Cmb_Dependencias_Area.SelectedIndex <= 0)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione la Dependencia a la cual pertenece el Área <br>";
                    }
                    if (Txt_Comentarios_Area.Text.Length > 250)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Los comentarios proporcionados no deben ser mayor a 250 caracteres <br>";
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

    protected void Btn_Eliminar_Click(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            //Si el usuario selecciono un Área entonces la elimina de la base de datos
            if (Txt_Area_ID.Text != "")
            {
                Eliminar_Area(); //Elimina el Área que fue seleccionada por el usuario
            }
            //Si el usuario no selecciono algun Área manda un mensaje indicando que es necesario que seleccione alguna para
            //poder eliminar
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione el Área que desea eliminar <br>";
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    protected void Btn_Buscar_Area_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Consulta_Areas(); //Consulta las areas que coincidan con el nombre porporcionado por el usuario
            Limpia_Controles(); //Limpia los controles de la forma
            //Si no se encontraron áreas con un nombre similar al proporcionado por el usuario entonces manda un mensaje al usuario
            if (Grid_Areas.Rows.Count <= 0)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encontraron Areas con el nombre proporcionado <br>";
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
                Session.Remove("Consulta_Areas");
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
    #endregion
}
