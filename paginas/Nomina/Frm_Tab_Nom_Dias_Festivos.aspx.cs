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
using Presidencia.Dias_Festivos.Negocios;
using Presidencia.Sessiones;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Constantes;
using System.Collections.Generic;

public partial class paginas_Nomina_Frm_Tab_Nom_Dias_Festivos : System.Web.UI.Page
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
    /// FECHA_CREO  : 06-Septiembre-2010
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
            Consulta_Dias_Festivos();       //Consulta todas los datos de los Días Festivos que fueron dadas de alta en la BD
            Consultar_Calendarios_Nomina();
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
    /// FECHA_CREO  : 06-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {
            Txt_Dia_ID.Text = "";
            Txt_Fecha_Dia_Festivo.Text = "";
            Txt_Descripcion_Dia_Festivo.Text = "";
            Txt_Comentarios_Dia_Festivo.Text = "";
            Txt_Busqueda_Dias_Festivos.Text = "";
            Cmb_Calendario_Nomina.SelectedIndex = -1;
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
    /// FECHA_CREO  : 06-Septiembre-2010
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

                    Configuracion_Acceso("Frm_Tab_Nom_Dias_Festivos.aspx");
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
            Txt_Fecha_Dia_Festivo.Enabled = Habilitado;
            Txt_Descripcion_Dia_Festivo.Enabled = Habilitado;
            Txt_Comentarios_Dia_Festivo.Enabled = Habilitado;
            Txt_Busqueda_Dias_Festivos.Enabled = !Habilitado;
            Btn_Busqueda_Dias_Festivos.Enabled = !Habilitado;
            Grid_Dias_Festivos.Enabled = !Habilitado;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Cmb_Calendario_Nomina.Enabled = Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Dias_Festivos
    /// DESCRIPCION : Consulta los Días Festivos que estan dadas de alta en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 06-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Dias_Festivos()
    {
        Cls_Tab_Nom_Dias_Festivos_Negocios Rs_Consulta_Tab_Nom_Dias_Festivos = new Cls_Tab_Nom_Dias_Festivos_Negocios(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Dias_Festivos; //Variable que obtendra los datos de la consulta 

        try
        {
            if (Txt_Busqueda_Dias_Festivos.Text != "")
            {
                Rs_Consulta_Tab_Nom_Dias_Festivos.P_Dia_ID = Txt_Busqueda_Dias_Festivos.Text;
            }
            Dt_Dias_Festivos = Rs_Consulta_Tab_Nom_Dias_Festivos.Consulta_Datos_Dia_Festivo(); //Consulta todos los Días Festivos con sus datos generales
            Session["Consulta_Dias_Festivos"] = Dt_Dias_Festivos;
            Llena_Grid_Dias_Festivos();
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Dias_Festivos " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llena_Grid_Dias_Festivos
    /// DESCRIPCION : Llena el grid con los Días Festivos que se encuentran en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 06-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llena_Grid_Dias_Festivos()
    {
        DataTable Dt_Dias_Festivos; //Variable que obtendra los datos de la consulta 
        try
        {
            Grid_Dias_Festivos.Columns[4].Visible = true;
            Grid_Dias_Festivos.Columns[5].Visible = true;
            Grid_Dias_Festivos.DataBind();
            Dt_Dias_Festivos = (DataTable)Session["Consulta_Dias_Festivos"];
            Grid_Dias_Festivos.DataSource = Dt_Dias_Festivos;
            Grid_Dias_Festivos.DataBind();
            Grid_Dias_Festivos.Columns[4].Visible = false;
            Grid_Dias_Festivos.Columns[5].Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Dias_Festivos " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Dia_Festivo
    /// DESCRIPCION : Da de Alta el Dias Festivo con los datos proporcionados por el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 06-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Dia_Festivo()
    {
        Cls_Tab_Nom_Dias_Festivos_Negocios Rs_Alta_Tab_Nom_Dias_Festivos = new Cls_Tab_Nom_Dias_Festivos_Negocios(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            Rs_Alta_Tab_Nom_Dias_Festivos.P_Fecha = Convert.ToDateTime(Txt_Fecha_Dia_Festivo.Text);
            Rs_Alta_Tab_Nom_Dias_Festivos.P_Descripcion = Convert.ToString(Txt_Descripcion_Dia_Festivo.Text);
            Rs_Alta_Tab_Nom_Dias_Festivos.P_Comentarios = Convert.ToString(Txt_Comentarios_Dia_Festivo.Text);
            Rs_Alta_Tab_Nom_Dias_Festivos.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            Rs_Alta_Tab_Nom_Dias_Festivos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();

            Rs_Alta_Tab_Nom_Dias_Festivos.Alta_Dia_Festivo(); //Da de alta los datos del Día Festivo proporcionados por el usuario en la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Días Festivos", "alert('El Alta del Día Festivo fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Dia_Festivo " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Dia_Festivo
    /// DESCRIPCION : Modifica los datos del Día Festivo con los proporcionados por el usuario en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 06-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Dia_Festivo()
    {
        Cls_Tab_Nom_Dias_Festivos_Negocios Rs_Modificar_Tab_Nom_Dias_Festivos = new Cls_Tab_Nom_Dias_Festivos_Negocios(); //Variable de conexión hacia la capa de Negocios para envio de datos a modificar
        try
        {
            Rs_Modificar_Tab_Nom_Dias_Festivos.P_Dia_ID = Convert.ToString(Txt_Dia_ID.Text);
            Rs_Modificar_Tab_Nom_Dias_Festivos.P_Fecha = Convert.ToDateTime(Txt_Fecha_Dia_Festivo.Text);
            Rs_Modificar_Tab_Nom_Dias_Festivos.P_Descripcion = Convert.ToString(Txt_Descripcion_Dia_Festivo.Text);
            Rs_Modificar_Tab_Nom_Dias_Festivos.P_Comentarios = Convert.ToString(Txt_Comentarios_Dia_Festivo.Text);
            Rs_Modificar_Tab_Nom_Dias_Festivos.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            Rs_Modificar_Tab_Nom_Dias_Festivos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();

            Rs_Modificar_Tab_Nom_Dias_Festivos.Modificar_Dia_Festivo(); //Sustituye los datos que se encuentran en la BD por lo que introdujo el usuario
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Días Festivos", "alert('La Modificación del Día Festivo fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Dia_Festivo " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Dia_Festivo
    /// DESCRIPCION : Elimina los datos del Dia Festivo que fue seleccionada por el Usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 06-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Dia_Festivo()
    {
        Cls_Tab_Nom_Dias_Festivos_Negocios Rs_Eliminar_Tab_Nom_Dias_Festivos = new Cls_Tab_Nom_Dias_Festivos_Negocios(); //Variable de conexión hacia la capa de Negocios para la eliminación de los datos
        try
        {
            Rs_Eliminar_Tab_Nom_Dias_Festivos.P_Dia_ID = Txt_Dia_ID.Text;
            Rs_Eliminar_Tab_Nom_Dias_Festivos.Eliminar_Dia_Festivo(); //Elimina el Día Festivo que selecciono el usuario de la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Días Festivos", "alert('La Eliminación del Día Festivo fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Eliminar_Dia_Festivo " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region (Grid)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Dias_Festivos_SelectedIndexChanged
    /// DESCRIPCION : Consulta los datos del Día Festivo que selecciono el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 06-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Dias_Festivos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Txt_Dia_ID.Text = Grid_Dias_Festivos.SelectedRow.Cells[1].Text;
            Txt_Fecha_Dia_Festivo.Text = Grid_Dias_Festivos.SelectedRow.Cells[2].Text;
            Txt_Descripcion_Dia_Festivo.Text = HttpUtility.HtmlDecode(Grid_Dias_Festivos.SelectedRow.Cells[3].Text);
            Txt_Comentarios_Dia_Festivo.Text = HttpUtility.HtmlDecode(Grid_Dias_Festivos.SelectedRow.Cells[4].Text);
            Cmb_Calendario_Nomina.SelectedIndex = Cmb_Calendario_Nomina.Items.IndexOf(Cmb_Calendario_Nomina.Items.FindByValue(HttpUtility.HtmlDecode(Grid_Dias_Festivos.SelectedRow.Cells[5].Text)));
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Grid_Dias_Festivos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpia_Controles();                            //Limpia todos los controles de la forma
            Grid_Dias_Festivos.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Llena_Grid_Dias_Festivos();                       //Carga los Días Festivos que estan asignados a la página seleccionada
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
    protected void Btn_Busqueda_Dias_Festivos_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Consulta_Dias_Festivos();  //Consulta los Días Festivos que coincidan con el ID porporcionado por el usuario
            Limpia_Controles();        //Limpia los controles de la forma
            //Si no se encontraron Días Festivos con un ID similar al proporcionado por el usuario entonces manda un mensaje al usuario
            if (Grid_Dias_Festivos.Rows.Count <= 0)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encontraron Días Festivos con el ID proporcionado <br>";
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
                if (Txt_Fecha_Dia_Festivo.Text != "" & Txt_Descripcion_Dia_Festivo.Text != "" & Txt_Comentarios_Dia_Festivo.Text.Length <= 250)
                {
                    Alta_Dia_Festivo(); //Da de alta los datos proporcionados por el usuario
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                    if (Txt_Fecha_Dia_Festivo.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Fecha Del Día Festivo <br>";
                    }
                    if (Txt_Descripcion_Dia_Festivo.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Conmemoración del Día Festivo <br>";
                    }
                    if (Txt_Comentarios_Dia_Festivo.Text.Length > 250)
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
                if (Txt_Dia_ID.Text != "")
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el Día Festivo que desea modificar sus datos <br>";
                }
            }
            else
            {
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces modifica estos en la BD
                if (Txt_Fecha_Dia_Festivo.Text != "" & Txt_Descripcion_Dia_Festivo.Text != "" & Txt_Comentarios_Dia_Festivo.Text.Length <= 250)
                {
                    Modificar_Dia_Festivo(); //Modifica los datos del Día Festivo con los datos proporcionados por el usuario
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                    if (Txt_Fecha_Dia_Festivo.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Fecha Del Día Festivo <br>";
                    }
                    if (Txt_Descripcion_Dia_Festivo.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Conmemoración del Día Festivo <br>";
                    }
                    if (Txt_Comentarios_Dia_Festivo.Text.Length > 250)
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
            //Si el usuario selecciono un Día Festivo entonces lo elimina de la base de datos
            if (Txt_Dia_ID.Text != "")
            {
                Eliminar_Dia_Festivo(); //Elimina el Día Festivo que fue seleccionado por el usuario
            }
            //Si el usuario no selecciono algún Día Festivomanda un mensaje indicando que es necesario que seleccione alguna para
            //poder eliminar
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione el Día Festivo que desea eliminar <br>";
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
                Session.Remove("Consulta_Dias_Festivos");
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
            Botones.Add(Btn_Busqueda_Dias_Festivos);

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

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Calendarios_Nomina
    /// DESCRIPCION : 
    /// 
    /// PARAMETROS:
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Calendarios_Nomina()
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nominales = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Calendarios_Nominales = null;//Variable que almacena los calendarios nominales que existén actualmente en el sistema.
        try
        {
            Dt_Calendarios_Nominales = Obj_Calendario_Nominales.Consultar_Calendario_Nominas();
            Dt_Calendarios_Nominales = Formato_Fecha_Calendario_Nomina(Dt_Calendarios_Nominales);

            if (Dt_Calendarios_Nominales is DataTable)
            {
                Cmb_Calendario_Nomina.DataSource = Dt_Calendarios_Nominales;
                Cmb_Calendario_Nomina.DataTextField = "Nomina";
                Cmb_Calendario_Nomina.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
                Cmb_Calendario_Nomina.DataBind();
                Cmb_Calendario_Nomina.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
                Cmb_Calendario_Nomina.SelectedIndex = -1;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los calendarios de nómina que existen actualmente registrados en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Formato_Fecha_Calendario_Nomina
    /// DESCRIPCION : Crea el DataTable con la consulta de las nomina vigentes en el 
    /// sistema.
    /// PARAMETROS: Dt_Calendario_Nominas.- Lista de las nominas vigentes actualmente 
    ///             en el sistema.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Formato_Fecha_Calendario_Nomina(DataTable Dt_Calendario_Nominas)
    {
        DataTable Dt_Nominas = new DataTable();
        DataRow Renglon_Dt_Clon = null;
        Dt_Nominas.Columns.Add("Nomina", typeof(System.String));
        Dt_Nominas.Columns.Add(Cat_Nom_Calendario_Nominas.Campo_Nomina_ID, typeof(System.String));

        if (Dt_Calendario_Nominas is DataTable)
        {
            foreach (DataRow Renglon in Dt_Calendario_Nominas.Rows)
            {
                if (Renglon is DataRow)
                {
                    Renglon_Dt_Clon = Dt_Nominas.NewRow();
                    Renglon_Dt_Clon["Nomina"] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin].ToString().Split(new char[] { ' ' })[0].Split(new char[] { '/' })[2];
                    Renglon_Dt_Clon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID];
                    Dt_Nominas.Rows.Add(Renglon_Dt_Clon);
                }
            }
        }
        return Dt_Nominas;
    }
}
