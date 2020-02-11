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
using Presidencia.Subsidio.Negocios;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;


public partial class paginas_Nomina_Frm_Tab_Nom_Subsidio : System.Web.UI.Page
{
    #region (Page Load)
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
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
    /// FECHA_CREO  : 31-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            Limpia_Controles();             //Limpia los controles del forma
            Consulta_Subsidios();           //Consulta todas los Subsidios que fueron dadas de alta en la BD
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
    /// FECHA_CREO  : 31-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {
            Cmb_Tipo_Nomina_Subsidio.SelectedIndex = 0;
            Txt_Subsidio_ID.Text = "";
            Txt_Subsidio.Text = "";
            Txt_Limite_Inferior_Subsidio.Text = "";
            Txt_Comentarios_Subsidio.Text = "";
            Txt_Busqueda_Subsidio.Text = "";
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
    /// FECHA_CREO  : 31-Agosto-2010
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
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                    Configuracion_Acceso("Frm_Tab_Nom_Subsidio.aspx");
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
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    break;
            }
            Txt_Subsidio.Enabled = Habilitado;
            Txt_Limite_Inferior_Subsidio.Enabled = Habilitado;
            Txt_Comentarios_Subsidio.Enabled = Habilitado;
            Cmb_Tipo_Nomina_Subsidio.Enabled = Habilitado;
            Txt_Busqueda_Subsidio.Enabled = !Habilitado;
            Btn_Busqueda_Subsidio.Enabled = !Habilitado;
            Grid_Subsidio.Enabled = !Habilitado;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Subsidios
    /// DESCRIPCION : Consulta los Subsidio que estan dados de alta en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 31-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Subsidios()
    {
        Cls_Tab_Nom_Subsidio_Negocio Rs_Consulta_Tab_Nom_Subsidio = new Cls_Tab_Nom_Subsidio_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Subsidio; //Variable que obtendra los datos de la consulta 

        try
        {
            if (Txt_Busqueda_Subsidio.Text != "")
            {
                Rs_Consulta_Tab_Nom_Subsidio.P_Tipo_Nomina = Convert.ToString(Txt_Busqueda_Subsidio.Text.ToUpper());
            }
            Dt_Subsidio = Rs_Consulta_Tab_Nom_Subsidio.Consulta_Datos_Subsidio(); //Consulta todos los Subsidios con sus datos generales
            Session["Consulta_Subsidios"] = Dt_Subsidio;
            Llena_Grid_Subsidios();
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Subsidios " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llena_Grid_Subsidios
    /// DESCRIPCION : Llena el grid con los Subsidios que se encuentran en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 30-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llena_Grid_Subsidios()
    {
        DataTable Dt_Subsidio; //Variable que obtendra los datos de la consulta 
        try
        {
            Grid_Subsidio.Columns[5].Visible = true;
            Grid_Subsidio.DataBind();
            Dt_Subsidio = (DataTable)Session["Consulta_Subsidios"];
            Grid_Subsidio.DataSource = Dt_Subsidio;
            Grid_Subsidio.DataBind();
            Grid_Subsidio.Columns[5].Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Subsidios " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Subsidio
    /// DESCRIPCION : Da de Alta el Subsidio con los datos proporcionados por el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 31-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Subsidio()
    {
        Cls_Tab_Nom_Subsidio_Negocio Rs_Alta_Tab_Nom_Subsidios = new Cls_Tab_Nom_Subsidio_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            Rs_Alta_Tab_Nom_Subsidios.P_Limite_Inferior = Convert.ToDouble(Txt_Limite_Inferior_Subsidio.Text);
            Rs_Alta_Tab_Nom_Subsidios.P_Subsidio = Convert.ToDouble(Txt_Subsidio.Text);
            Rs_Alta_Tab_Nom_Subsidios.P_Tipo_Nomina = Convert.ToString(Cmb_Tipo_Nomina_Subsidio.SelectedValue);
            Rs_Alta_Tab_Nom_Subsidios.P_Comentarios = Convert.ToString(Txt_Comentarios_Subsidio.Text);
            Rs_Alta_Tab_Nom_Subsidios.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

            Rs_Alta_Tab_Nom_Subsidios.Alta_Subsidio(); //Da de alta los datos de el Subsidio proporcionados por el usuario en la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Subsidios", "alert('El Alta del Subsidio fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Subsidio " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Subsidio
    /// DESCRIPCION : Modifica los datos del Subsidio con los proporcionados por el usuario en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 31-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Subsidio()
    {
        Cls_Tab_Nom_Subsidio_Negocio Rs_Modificar_Tab_Nom_Subsidios = new Cls_Tab_Nom_Subsidio_Negocio(); //Variable de conexión hacia la capa de Negocios para envio de datos a modificar
        try
        {
            Rs_Modificar_Tab_Nom_Subsidios.P_Subsidio_ID = Convert.ToString(Txt_Subsidio_ID.Text);
            Rs_Modificar_Tab_Nom_Subsidios.P_Limite_Inferior = Convert.ToDouble(Txt_Limite_Inferior_Subsidio.Text);
            Rs_Modificar_Tab_Nom_Subsidios.P_Subsidio = Convert.ToDouble(Txt_Subsidio.Text);
            Rs_Modificar_Tab_Nom_Subsidios.P_Tipo_Nomina = Convert.ToString(Cmb_Tipo_Nomina_Subsidio.SelectedValue);
            Rs_Modificar_Tab_Nom_Subsidios.P_Comentarios = Convert.ToString(Txt_Comentarios_Subsidio.Text);
            Rs_Modificar_Tab_Nom_Subsidios.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

            Rs_Modificar_Tab_Nom_Subsidios.Modificar_Subsidio(); //Sustituye los datos que se encuentran en la BD por lo que introdujo el usuario
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Subsidio", "alert('La Modificación del Subsidio fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Subsidio " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Subsidio
    /// DESCRIPCION : Elimina los datos del Subsidio que fue seleccionado por el Usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 31-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Subsidio()
    {
        Cls_Tab_Nom_Subsidio_Negocio Rs_Eliminar_Tab_Nom_Subsidios = new Cls_Tab_Nom_Subsidio_Negocio(); //Variable de conexión hacia la capa de Negocios para la eliminación de los datos
        try
        {
            Rs_Eliminar_Tab_Nom_Subsidios.P_Subsidio_ID = Txt_Subsidio_ID.Text;
            Rs_Eliminar_Tab_Nom_Subsidios.Eliminar_Subsidio(); //Elimina el Subsidio que selecciono el usuario de la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Subsidios", "alert('La Eliminación del Subsidio fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Eliminar_Subsidio " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region (Grid)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Subsidio_SelectedIndexChanged
    /// DESCRIPCION : Consulta los datos del Subsidio que selecciono el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 31-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Subsidio_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Txt_Subsidio_ID.Text = Grid_Subsidio.SelectedRow.Cells[1].Text;
            Cmb_Tipo_Nomina_Subsidio.SelectedValue = HttpUtility.HtmlDecode(Grid_Subsidio.SelectedRow.Cells[2].Text);
            Txt_Limite_Inferior_Subsidio.Text = Grid_Subsidio.SelectedRow.Cells[3].Text;
            Txt_Subsidio.Text = Grid_Subsidio.SelectedRow.Cells[4].Text;
            Txt_Comentarios_Subsidio.Text = HttpUtility.HtmlDecode(Grid_Subsidio.SelectedRow.Cells[5].Text);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Grid_Subsidio_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpia_Controles();                       //Limpia todos los controles de la forma
            Grid_Subsidio.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Llena_Grid_Subsidios();                   //Carga los Subsidios que estan asignadas a la página seleccionada
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion

    #region (Eventos)
    protected void Btn_Busqueda_Subsidio_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Consulta_Subsidios();  //Consulta las Tipos de Nómina que coincidan con la porporcionado por el usuario
            Limpia_Controles();    //Limpia los controles de la forma
            //Si no se encontraron Subsidio con un nombre similar al proporcionado por el usuario entonces manda un mensaje al usuario
            if (Grid_Subsidio.Rows.Count <= 0)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encontraron Subsidio con el Tipo de Nómina proporcionada <br>";
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
                if (Txt_Subsidio.Text != "" & Txt_Limite_Inferior_Subsidio.Text != "" & Cmb_Tipo_Nomina_Subsidio.SelectedIndex > 0 & Txt_Comentarios_Subsidio.Text.Length <= 250)
                {
                    Alta_Subsidio(); //Da de alta los datos proporcionados por el usuario
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                    if (Cmb_Tipo_Nomina_Subsidio.SelectedIndex == 0)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Tipo de Nómina <br>";
                    }
                    if (Txt_Limite_Inferior_Subsidio.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Límite Inferior <br>";
                    }
                    if (Txt_Subsidio.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Subsidio <br>";
                    }
                    if (Txt_Comentarios_Subsidio.Text.Length > 250)
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
                if (Txt_Subsidio_ID.Text != "")
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el Subsidio que desea modificar sus datos <br>";
                }
            }
            else
            {
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces modifica estos en la BD
                if (Txt_Subsidio.Text != "" & Txt_Limite_Inferior_Subsidio.Text != "" & Cmb_Tipo_Nomina_Subsidio.SelectedIndex > 0 & Txt_Comentarios_Subsidio.Text.Length <= 250)
                {
                    Modificar_Subsidio(); //Modifica los datos del Subsidio con los datos proporcionados por el usuario
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                    if (Cmb_Tipo_Nomina_Subsidio.SelectedIndex == 0)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Tipo de Nómina <br>";
                    }
                    if (Txt_Limite_Inferior_Subsidio.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Límite Inferior <br>";
                    }
                    if (Txt_Subsidio.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Subsidio <br>";
                    }
                    if (Txt_Comentarios_Subsidio.Text.Length > 250)
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
            //Si el usuario selecciono un Subsidio entonces lo elimina de la base de datos
            if (Txt_Subsidio_ID.Text != "")
            {
                Eliminar_Subsidio(); //Elimina el Subsidio que fue seleccionado por el usuario
            }
            //Si el usuario no selecciono algun Subsidio manda un mensaje indicando que es necesario que seleccione algun para
            //poder eliminar
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione el Subsidio que desea eliminar <br>";
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
                Session.Remove("Consulta_Subsidios");
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
            Botones.Add(Btn_Busqueda_Subsidio);

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
