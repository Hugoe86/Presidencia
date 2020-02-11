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
using Presidencia.ISR.Negocios;
using Presidencia.Sessiones;
using System.Collections.Generic;
using Presidencia.Constantes;

public partial class paginas_Nomina_Frm_Tab_Nom_ISR : System.Web.UI.Page
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
    /// FECHA_CREO  : 01-Septiembre-2010
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
            Consulta_ISR();                 //Consulta todas los ISR que fueron dadas de alta en la BD
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
    /// FECHA_CREO  : 01-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {
            Cmb_Tipo_Nomina_ISR.SelectedIndex = 0;
            Txt_ISR_ID.Text = "";
            Txt_Limite_Inferior_ISR.Text = "";
            Txt_Porcentaje_ISR.Text = "";
            Txt_Couta_Fija_ISR.Text = "";
            Txt_Comentarios_ISR.Text = "";
            Txt_Busqueda_ISR.Text = "";
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
    /// FECHA_CREO  : 01-Septiembre-2010
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

                    Configuracion_Acceso("Frm_Tab_Nom_ISR.aspx");
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
            Txt_Couta_Fija_ISR.Enabled = Habilitado;
            Txt_Limite_Inferior_ISR.Enabled = Habilitado;
            Txt_Porcentaje_ISR.Enabled = Habilitado;
            Txt_Comentarios_ISR.Enabled = Habilitado;
            Cmb_Tipo_Nomina_ISR.Enabled = Habilitado;
            Txt_Busqueda_ISR.Enabled = !Habilitado;
            Btn_Busqueda_ISR.Enabled = !Habilitado;
            Grid_ISR.Enabled = !Habilitado;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_ISR
    /// DESCRIPCION : Consulta los ISR que estan dados de alta en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 01-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_ISR()
    {
        Cls_Tab_Nom_ISR_Negocio Rs_Consulta_Tab_Nom_ISR = new Cls_Tab_Nom_ISR_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_ISR; //Variable que obtendra los datos de la consulta 

        try
        {
            if (Txt_Busqueda_ISR.Text != "")
            {
                Rs_Consulta_Tab_Nom_ISR.P_Tipo_Nomina = Convert.ToString(Txt_Busqueda_ISR.Text.ToUpper());
            }
            Dt_ISR = Rs_Consulta_Tab_Nom_ISR.Consulta_Datos_ISR(); //Consulta todos los ISR con sus datos generales
            Session["Consulta_ISR"] = Dt_ISR;
            Llena_Grid_ISR();
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_ISR " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llena_Grid_ISR
    /// DESCRIPCION : Llena el grid con los ISR que se encuentran en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 01-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llena_Grid_ISR()
    {
        DataTable Dt_ISR; //Variable que obtendra los datos de la consulta 
        try
        {
            Grid_ISR.Columns[6].Visible = true;
            Grid_ISR.DataBind();
            Dt_ISR = (DataTable)Session["Consulta_ISR"];
            Grid_ISR.DataSource = Dt_ISR;
            Grid_ISR.DataBind();
            Grid_ISR.Columns[6].Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_ISR " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_ISR
    /// DESCRIPCION : Da de Alta el ISR con los datos proporcionados por el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 01-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_ISR()
    {
        Cls_Tab_Nom_ISR_Negocio Rs_Alta_Tab_Nom_ISR = new Cls_Tab_Nom_ISR_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            Rs_Alta_Tab_Nom_ISR.P_Limite_Inferior = Convert.ToDouble(Txt_Limite_Inferior_ISR.Text);
            Rs_Alta_Tab_Nom_ISR.P_Couta_Fija = Convert.ToDouble(Txt_Couta_Fija_ISR.Text);
            Rs_Alta_Tab_Nom_ISR.P_Porcentaje = Convert.ToDouble(Txt_Porcentaje_ISR.Text);
            Rs_Alta_Tab_Nom_ISR.P_Tipo_Nomina = Convert.ToString(Cmb_Tipo_Nomina_ISR.SelectedValue);
            Rs_Alta_Tab_Nom_ISR.P_Comentarios = Convert.ToString(Txt_Comentarios_ISR.Text);
            Rs_Alta_Tab_Nom_ISR.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

            Rs_Alta_Tab_Nom_ISR.Alta_ISR(); //Da de alta los datos de el ISR proporcionados por el usuario en la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de ISR", "alert('El Alta del ISR fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_ISR " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_ISR
    /// DESCRIPCION : Modifica los datos del ISR con los proporcionados por el usuario en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 01-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_ISR()
    {
        Cls_Tab_Nom_ISR_Negocio Rs_Modificar_Tab_Nom_ISR = new Cls_Tab_Nom_ISR_Negocio(); //Variable de conexión hacia la capa de Negocios para envio de datos a modificar
        try
        {
            Rs_Modificar_Tab_Nom_ISR.P_ISR_ID = Convert.ToString(Txt_ISR_ID.Text);
            Rs_Modificar_Tab_Nom_ISR.P_Limite_Inferior = Convert.ToDouble(Txt_Limite_Inferior_ISR.Text);
            Rs_Modificar_Tab_Nom_ISR.P_Couta_Fija = Convert.ToDouble(Txt_Couta_Fija_ISR.Text);
            Rs_Modificar_Tab_Nom_ISR.P_Porcentaje = Convert.ToDouble(Txt_Porcentaje_ISR.Text);
            Rs_Modificar_Tab_Nom_ISR.P_Tipo_Nomina = Convert.ToString(Cmb_Tipo_Nomina_ISR.SelectedValue);
            Rs_Modificar_Tab_Nom_ISR.P_Comentarios = Convert.ToString(Txt_Comentarios_ISR.Text);
            Rs_Modificar_Tab_Nom_ISR.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

            Rs_Modificar_Tab_Nom_ISR.Modificar_ISR(); //Sustituye los datos que se encuentran en la BD por lo que introdujo el usuario
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de ISR", "alert('La Modificación del ISR fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_ISR " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_ISR
    /// DESCRIPCION : Elimina los datos del ISR que fue seleccionado por el Usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 01-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_ISR()
    {
        Cls_Tab_Nom_ISR_Negocio Rs_Eliminar_Tab_Nom_ISR = new Cls_Tab_Nom_ISR_Negocio(); //Variable de conexión hacia la capa de Negocios para la eliminación de los datos
        try
        {
            Rs_Eliminar_Tab_Nom_ISR.P_ISR_ID = Txt_ISR_ID.Text;
            Rs_Eliminar_Tab_Nom_ISR.Eliminar_ISR(); //Elimina el ISR que selecciono el usuario de la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de ISR", "alert('La Eliminación del ISR fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Eliminar_ISR " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region (Grid)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_ISR_SelectedIndexChanged
    /// DESCRIPCION : Consulta los datos del ISR que selecciono el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 01-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_ISR_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Txt_ISR_ID.Text = Grid_ISR.SelectedRow.Cells[1].Text;
            Cmb_Tipo_Nomina_ISR.SelectedValue = HttpUtility.HtmlDecode(Grid_ISR.SelectedRow.Cells[2].Text);
            Txt_Limite_Inferior_ISR.Text = Grid_ISR.SelectedRow.Cells[3].Text;
            Txt_Couta_Fija_ISR.Text = Grid_ISR.SelectedRow.Cells[4].Text;
            Txt_Porcentaje_ISR.Text = Grid_ISR.SelectedRow.Cells[5].Text;
            Txt_Comentarios_ISR.Text = HttpUtility.HtmlDecode(Grid_ISR.SelectedRow.Cells[6].Text);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Grid_ISR_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpia_Controles();                  //Limpia todos los controles de la forma
            Grid_ISR.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Llena_Grid_ISR();                    //Carga los ISR que estan asignadas a la página seleccionada
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
    protected void Btn_Busqueda_ISR_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Consulta_ISR();        //Consulta las Tipos de Nómina que coincidan con la porporcionado por el usuario
            Limpia_Controles();    //Limpia los controles de la forma
            //Si no se encontraron ISR con un nombre similar al proporcionado por el usuario entonces manda un mensaje al usuario
            if (Grid_ISR.Rows.Count <= 0)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encontraron ISR con el Tipo de Nómina proporcionada <br>";
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
                if (Txt_Couta_Fija_ISR.Text != "" & Txt_Limite_Inferior_ISR.Text != "" & Cmb_Tipo_Nomina_ISR.SelectedIndex > 0 & Txt_Comentarios_ISR.Text.Length <= 250 & Txt_Porcentaje_ISR.Text != "")
                {
                    Alta_ISR(); //Da de alta los datos proporcionados por el usuario
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                    if (Cmb_Tipo_Nomina_ISR.SelectedIndex == 0)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Tipo de Nómina <br>";
                    }
                    if (Txt_Limite_Inferior_ISR.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Límite Inferior <br>";
                    }
                    if (Txt_Couta_Fija_ISR.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Cuota Fija <br>";
                    }
                    if (Txt_Porcentaje_ISR.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Porcentaje <br>";
                    }
                    if (Txt_Comentarios_ISR.Text.Length > 250)
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
                if (Txt_ISR_ID.Text != "")
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el ISR que desea modificar sus datos <br>";
                }
            }
            else
            {
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces modifica estos en la BD
                if (Txt_Couta_Fija_ISR.Text != "" & Txt_Limite_Inferior_ISR.Text != "" & Cmb_Tipo_Nomina_ISR.SelectedIndex > 0 & Txt_Comentarios_ISR.Text.Length <= 250 & Txt_Porcentaje_ISR.Text != "")
                {
                    Modificar_ISR(); //Modifica los datos del ISR con los datos proporcionados por el usuario
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                    if (Cmb_Tipo_Nomina_ISR.SelectedIndex == 0)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Tipo de Nómina <br>";
                    }
                    if (Txt_Limite_Inferior_ISR.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Límite Inferior <br>";
                    }
                    if (Txt_Couta_Fija_ISR.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Couta Fija <br>";
                    }
                    if (Txt_Porcentaje_ISR.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Porcentaje <br>";
                    }
                    if (Txt_Comentarios_ISR.Text.Length > 250)
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
            //Si el usuario selecciono un ISR entonces lo elimina de la base de datos
            if (Txt_ISR_ID.Text != "")
            {
                Eliminar_ISR(); //Elimina el ISR que fue seleccionado por el usuario
            }
            //Si el usuario no selecciono algun ISR manda un mensaje indicando que es necesario que seleccione algun para
            //poder eliminar
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione el ISR que desea eliminar <br>";
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
                Session.Remove("Consulta_ISR");
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
            Botones.Add(Btn_Busqueda_ISR);

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
