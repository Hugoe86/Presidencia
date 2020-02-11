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
using Presidencia.Tipo_Trabajador.Negocios;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;

public partial class paginas_Nomina_Frm_Cat_Nom_Tipo_Trabajador : System.Web.UI.Page
{
    #region (Page Load)
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Estado_Inicial(); //coloca la pagina en un estatus inicial para la navegacion
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
    /// NOMBRE DE LA FUNCION:   Estado_Inicial 
    /// DESCRIPCION :           Colocar la pagina en el estatus inicial para su navegacion.
    /// PARAMETROS  : 
    /// CREO        :           Noe Mosqueda Valadez
    /// FECHA_CREO  :           18/Septiembre/2010 12:00
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Estado_Inicial()
    {
        Limpiar_Controles();
        Habilita_Controles("Inicial");
        Llena_Grid_Tipo_Trabajador();
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION:   Elimina_Sesiones 
    /// DESCRIPCION :           Eliminar variables de sesion utilizadas en esta pagina.
    /// PARAMETROS  : 
    /// CREO        :           Noe Mosqueda Valadez
    /// FECHA_CREO  :           18/Septiembre/2010 12:00
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Elimina_Sesiones()
    {
        Session.Remove("Dt_Tipos_Trabajadores_ID");
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION:   Limpiar_Controles 
    /// DESCRIPCION :           Limpiar los controles de la pagina
    /// PARAMETROS  : 
    /// CREO        :           Noe Mosqueda Valadez
    /// FECHA_CREO  :           03/Septiembre/2010 13:38
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Txt_Busqueda_Tipo_Trabajador.Text = "";
            Txt_Comentarios_Tipo_Trabajador.Text = "";
            Txt_Descripcion_Tipo_Trabajador.Text = "";
            Txt_Tipo_Trabajador_ID.Text = "";
            Cmb_Estatus_Tipo_Trabajador.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Limpiar_Controles " + ex.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION:   Habilita_Controles 
    /// DESCRIPCION :           Habilitar/Deshabilitar los controles de la pagina
    /// PARAMETROS  :           Modo: Cadena de texto que indica el modo de operacion a realizar
    /// CREO        :           Noe Mosqueda Valadez
    /// FECHA_CREO  :           04/Septiembre/2010 09:32
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilita_Controles(string Modo)
    {
        try
        {
            //variable que indica si los controles estan habilitados
            bool Habilitado = false;

            //seleccionar el modo de operacion
            switch (Modo)
            {
                case "Inicial":
                    Cmb_Estatus_Tipo_Trabajador.SelectedIndex = 0;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Buscar_Tipo_Trabajador.Enabled = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Nuevo.Visible = true;
                    Grid_Tipo_Trabajador.Enabled = true;
                    Cmb_Estatus_Tipo_Trabajador.Enabled = false;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                    Configuracion_Acceso("Frm_Cat_Nom_Tipo_Trabajador.aspx");
                    break;

                case "Nuevo":
                case "Modificar":
                    Habilitado = true;
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Buscar_Tipo_Trabajador.Enabled = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;

                    //verificar el modo
                    if (Modo == "Nuevo")
                    {
                        Cmb_Estatus_Tipo_Trabajador.SelectedIndex = 0;
                        Btn_Nuevo.ToolTip = "Dar de Alta";
                        Btn_Modificar.ToolTip = "Modificar";
                        Btn_Modificar.Visible = false;
                        Btn_Nuevo.Visible = true;
                        Cmb_Estatus_Tipo_Trabajador.Enabled = false;
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    }
                    else
                    {
                        Btn_Nuevo.ToolTip = "Nuevo";
                        Btn_Modificar.ToolTip = "Actualizar";
                        Cmb_Estatus_Tipo_Trabajador.Enabled = true;
                        Btn_Modificar.Visible = true;
                        Btn_Nuevo.Visible = false;
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    }
                    break;
            }

            Txt_Tipo_Trabajador_ID.Enabled = false;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Txt_Busqueda_Tipo_Trabajador.Enabled = !Habilitado;
            Btn_Buscar_Tipo_Trabajador.Enabled = !Habilitado;
            Txt_Comentarios_Tipo_Trabajador.Enabled = Habilitado;
            Txt_Descripcion_Tipo_Trabajador.Enabled = Habilitado;
            Grid_Tipo_Trabajador.Enabled = !Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilita_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION:   Alta_Tipo_Trabajador 
    /// DESCRIPCION :           Dar de alta un tipo de trabajador en la base de datos
    /// PARAMETROS  :           
    /// CREO        :           Noe Mosqueda Valadez
    /// FECHA_CREO  :           17/Septiembre/2010 13:22
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Tipo_Trabajador()
    {
        try
        {
            //Variable para la capa de negocios
            Cls_Cat_Nom_Tipo_Trabajador_Negocio Cat_Nom_Tipo_Trabajador = new Cls_Cat_Nom_Tipo_Trabajador_Negocio();

            //Asignar valores a las propiedades y darlo de alta
            Cat_Nom_Tipo_Trabajador.P_Descripcion = Txt_Descripcion_Tipo_Trabajador.Text.Trim();
            Cat_Nom_Tipo_Trabajador.P_Comentarios = Txt_Comentarios_Tipo_Trabajador.Text.Trim();
            Cat_Nom_Tipo_Trabajador.P_Estatus = Cmb_Estatus_Tipo_Trabajador.SelectedItem.Value;
            Cat_Nom_Tipo_Trabajador.P_Nombre_Usuario = (String)Session["Nombre_Usuario"];
            Cat_Nom_Tipo_Trabajador.Alta_Tipo_Trabajador();
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Tipo_Trabajador " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION:   Modifica_Tipo_Trabajador 
    /// DESCRIPCION :           Modificar un tipo de trabajador existente
    /// PARAMETROS  :           
    /// CREO        :           Noe Mosqueda Valadez
    /// FECHA_CREO  :           17/Septiembre/2010 13:29
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modifica_Tipo_Trabajador()
    {
        try
        {
            //Variable para la capa de negocios
            Cls_Cat_Nom_Tipo_Trabajador_Negocio Cat_Nom_Tipo_Trabajador = new Cls_Cat_Nom_Tipo_Trabajador_Negocio();

            //Asignar valores a las propiedades y modificar
            Cat_Nom_Tipo_Trabajador.P_Tipo_Trabajador_ID = Txt_Tipo_Trabajador_ID.Text.Trim();
            Cat_Nom_Tipo_Trabajador.P_Descripcion = Txt_Descripcion_Tipo_Trabajador.Text.Trim();
            Cat_Nom_Tipo_Trabajador.P_Comentarios = Txt_Comentarios_Tipo_Trabajador.Text.Trim();
            Cat_Nom_Tipo_Trabajador.P_Estatus = Cmb_Estatus_Tipo_Trabajador.SelectedItem.Value;
            Cat_Nom_Tipo_Trabajador.P_Nombre_Usuario = (String)Session["Nombre_Usuario"];
            Cat_Nom_Tipo_Trabajador.Modifica_Tipo_Trabajador();

        }
        catch (Exception ex)
        {
            throw new Exception("Modifica_Tipo_Trabajador " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION:   Elimina_Tipo_Trabajador 
    /// DESCRIPCION :           Eliminar un tipo de trabajador existente
    /// PARAMETROS  :           
    /// CREO        :           Noe Mosqueda Valadez
    /// FECHA_CREO  :           17/Septiembre/2010 13:46
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Elimina_Tipo_Trabajador()
    {
        try
        {
            //Variable para la capa de negocios
            Cls_Cat_Nom_Tipo_Trabajador_Negocio Cat_Nom_Tipo_Trabajador = new Cls_Cat_Nom_Tipo_Trabajador_Negocio();

            //Asignar valores a las propieades y eliminar
            Cat_Nom_Tipo_Trabajador.P_Tipo_Trabajador_ID = Txt_Tipo_Trabajador_ID.Text.Trim();
            Cat_Nom_Tipo_Trabajador.Elimina_Tipo_Trabajor();
        }
        catch (Exception ex)
        {
            throw new Exception("Elimina_Tipo_Trabajador " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION:   Llena_Grid_Tipo_Trabajador 
    /// DESCRIPCION :           LLenar el grid con los datos de los tipos de trabajadores.
    /// PARAMETROS  : 
    /// CREO        :           Noe Mosqueda Valadez
    /// FECHA_CREO  :           17/Septiembre/2010 10:22
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llena_Grid_Tipo_Trabajador()
    {
        try
        {
            //Variable para la capa de negocios
            Cls_Cat_Nom_Tipo_Trabajador_Negocio Cat_Nom_Tipo_Trabajador = new Cls_Cat_Nom_Tipo_Trabajador_Negocio();

            //Variable para la tabla
            DataTable Dt_Tipos_Trabajadores = new DataTable();

            //Verificar si hay algo que buscar
            if (Txt_Busqueda_Tipo_Trabajador.Text.Trim() != "")
                Cat_Nom_Tipo_Trabajador.P_Descripcion = Txt_Busqueda_Tipo_Trabajador.Text.Trim();

            //verificar si hay variable de sesion
            if (Session["Dt_Tipos_Trabajadores"] != null)
                Dt_Tipos_Trabajadores = (DataTable)Session["Dt_Tipos_Trabajadores"];
            else
                Dt_Tipos_Trabajadores = Cat_Nom_Tipo_Trabajador.Consulta_Tipo_Trabajador();

            //llenar el grid
            Grid_Tipo_Trabajador.DataBind();
            Grid_Tipo_Trabajador.DataSource = Dt_Tipos_Trabajadores;
            Grid_Tipo_Trabajador.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Tipo_Trabajador " + ex.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION:   Valida_Datos 
    /// DESCRIPCION :           Valida que esten los datos requeridos para dar de alta o modificar
    /// PARAMETROS  :           
    /// CREO        :           Noe Mosqueda Valadez
    /// FECHA_CREO  :           17/Septiembre/2010 18:11
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private string Valida_Datos()
    {
        //Variable para erl resultado
        String Resultado = "";

        //Variable para la cabecera del mensaje de Error
        String Cabecera = "Es necesario Introducir: <br>";
        try
        {
            //Verificar si se tienen llenos los datos
            if (Txt_Descripcion_Tipo_Trabajador.Text.Trim() == "")
                Resultado = Cabecera + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Nombre del Tipo de Trabajador <br>";

            if (Cmb_Estatus_Tipo_Trabajador.SelectedIndex < 0)
                Resultado = Cabecera + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Estatus del Tipo de Trabajador <br>";

            if (Txt_Comentarios_Tipo_Trabajador.Text.Trim().Length > 250)
                Resultado = Cabecera + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Los comentarios proporcionados no deben ser mayor a 250 caracteres <br>";

            //Entregar resultado
            return Resultado;
        }
        catch (Exception ex)
        {
            throw new Exception("Valida_Datos " + ex.ToString(), ex);
        }
    }
    #endregion

    #region (Grid)
    protected void Grid_Tipo_Trabajador_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            //Colocar valores en los controles
            Txt_Tipo_Trabajador_ID.Text = Grid_Tipo_Trabajador.SelectedRow.Cells[1].Text.Trim();
            Txt_Descripcion_Tipo_Trabajador.Text = HttpUtility.HtmlDecode(Grid_Tipo_Trabajador.SelectedRow.Cells[2].Text.Trim());

            //Verificar si los comentarios no son nulos o vacios
            if (Grid_Tipo_Trabajador.SelectedRow.Cells[3].Text != null)
            {
                if (Grid_Tipo_Trabajador.SelectedRow.Cells[3].Text.Trim() != "" && Grid_Tipo_Trabajador.SelectedRow.Cells[3].Text.Trim() != "&nbsp;")
                    Txt_Comentarios_Tipo_Trabajador.Text = HttpUtility.HtmlDecode(Grid_Tipo_Trabajador.SelectedRow.Cells[3].Text.Trim());
                else
                    Txt_Comentarios_Tipo_Trabajador.Text = "";
            }
            else
                Txt_Comentarios_Tipo_Trabajador.Text = "";

            //Verificar el estatus
            if (Grid_Tipo_Trabajador.SelectedRow.Cells[4].Text.Trim() == "ACTIVO")
                Cmb_Estatus_Tipo_Trabajador.SelectedIndex = 0;
            else
                Cmb_Estatus_Tipo_Trabajador.SelectedIndex = 1;
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Grid_Tipo_Trabajador_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpiar_Controles();

            //Asignar pagina  y llenar el grid
            Grid_Tipo_Trabajador.PageIndex = e.NewPageIndex;
            Llena_Grid_Tipo_Trabajador();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Grid_Tipo_Trabajador_Sorting
    /// 
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Febrero/2011 19:04 pm.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    protected void Grid_Tipo_Trabajador_Sorting(object sender, GridViewSortEventArgs e)
    {
        Llena_Grid_Tipo_Trabajador();
        DataTable Dt_Tipo_Trabajador = (Grid_Tipo_Trabajador.DataSource as DataTable);

        if (Dt_Tipo_Trabajador != null)
        {
            DataView Dv_Tipo_Trabajador = new DataView(Dt_Tipo_Trabajador);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Tipo_Trabajador.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Tipo_Trabajador.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Tipo_Trabajador.DataSource = Dv_Tipo_Trabajador;
            Grid_Tipo_Trabajador.DataBind();
        }
    }
    #endregion

    #region (Eventos)
    protected void Btn_Nuevo_Click(object sender, EventArgs e)
    {
        String Validacion = ""; //Variable para la validacion de los datos

        try
        {
            //Verificar el texto del boton
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Habilita_Controles("Nuevo");
                Limpiar_Controles();
            }
            else
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;

                //Validacion de los datos
                Validacion = Valida_Datos();

                //Verificar si faltan datos
                if (Validacion != "")
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = Validacion;
                }
                else
                {
                    Alta_Tipo_Trabajador();
                    Estado_Inicial();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tipo de Trabajador", "alert('El Alta del Tipo de Trabajador fue Exitosa');", true);
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
    protected void Btn_Buscar_Tipo_Trabajador_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Llena_Grid_Tipo_Trabajador();
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
        //Variable para la validacion
        String Validacion = "";

        try
        {
            //Verificar el texto del boton
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                //Verificar si se ha seleccionado un elemento
                if (Txt_Tipo_Trabajador_ID.Text.Trim() != "")
                    Habilita_Controles("Modificar");
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el Tipo de Trabajador que desea modificar sus datos <br>";
                }
            }
            else
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;

                //Validacion de los datos
                Validacion = Valida_Datos();

                //Verificar si faltan datos
                if (Validacion != "")
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = Validacion;
                }
                else
                {
                    Modifica_Tipo_Trabajador();
                    Estado_Inicial();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tipos de Trabajador", "alert('La Modificación del Tipo de Trabajador fue Exitosa');", true);
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
            //Verificar si ha sido seleccionado un elemento
            if (Txt_Tipo_Trabajador_ID.Text.Trim() != "")
            {
                Elimina_Tipo_Trabajador();
                Estado_Inicial();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Trabajador", "alert('La Eliminación del Tipo de Trabajador fue Exitosa');", true);
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione el Tipo de Trabajador que desea eliminar <br>";
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
            //Verificar el texto del boton
            if (Btn_Salir.ToolTip == "Salir")
            {
                Elimina_Sesiones();
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
                Estado_Inicial();
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
            Botones.Add(Btn_Buscar_Tipo_Trabajador);

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
