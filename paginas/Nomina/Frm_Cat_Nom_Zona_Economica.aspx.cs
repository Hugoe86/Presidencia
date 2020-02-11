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
using Presidencia.Zona_Economica.Negocios;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;

public partial class paginas_Nomina_Frm_Cat_Nom_Zona_Economica : System.Web.UI.Page
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
    /// FECHA_CREO  : 27-Agosto-2010
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
            Consulta_Zonas_Economicas();    //Consulta todas las Zonas Económicas que fueron dadas de alta en la BD
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
    /// FECHA_CREO  : 27-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {
            Txt_Zona_Economica_ID.Text = "";
            Txt_Zona_Economica.Text = "";
            Txt_Salario_Diario_Zona_Economica.Text = "";
            Txt_Comentarios_Zona_Economica.Text = "";
            Txt_Busqueda_Zona_Economica.Text = "";
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
    /// FECHA_CREO  : 27-Agosto-2010
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
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                    Configuracion_Acceso("Frm_Cat_Nom_Zona_Economica.aspx");
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    break;

                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    break;
            }

            Txt_Zona_Economica_ID.Enabled = false;
            Txt_Comentarios_Zona_Economica.Enabled = Habilitado;
            Txt_Busqueda_Zona_Economica.Enabled = !Habilitado;
            Txt_Salario_Diario_Zona_Economica.Enabled = Habilitado;
            Btn_Buscar_Zona_Economica.Enabled = !Habilitado;
            Grid_Zona_Economica.Enabled = !Habilitado;

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Zonas_Economicas
    /// DESCRIPCION : Consulta las Zonas Económicas que estan dadas de alta en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 27-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Zonas_Economicas()
    {
        Cls_Cat_Nom_Zona_Economica_Negocio Rs_Consulta_Cat_Nom_Zona_Economica = new Cls_Cat_Nom_Zona_Economica_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Zona_Economica; //Variable que obtendra los datos de la consulta 

        try
        {
            if (Txt_Busqueda_Zona_Economica.Text != "")
            {
                Rs_Consulta_Cat_Nom_Zona_Economica.P_Zona_Economica = Txt_Busqueda_Zona_Economica.Text;
            }
            Dt_Zona_Economica = Rs_Consulta_Cat_Nom_Zona_Economica.Consulta_Datos_Zona_Economica(); //Consulta todos las Zonas Ecomónicas con sus datos generales
            Session["Consulta_Zonas_Economicas"] = Dt_Zona_Economica;
            Llena_Grid_Zonas_Economicas();
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Zonas_Economicas " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llena_Grid_Zonas_Economicas
    /// DESCRIPCION : Llena el grid con las Zonas Económicas que se encuentran en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 27-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llena_Grid_Zonas_Economicas()
    {
        DataTable Dt_Zona_Economica; //Variable que obtendra los datos de la consulta 
        try
        {
            Grid_Zona_Economica.Columns[4].Visible = true;
            Grid_Zona_Economica.DataBind();
            Dt_Zona_Economica = (DataTable)Session["Consulta_Zonas_Economicas"];
            Grid_Zona_Economica.DataSource = Dt_Zona_Economica;
            Grid_Zona_Economica.DataBind();
            Grid_Zona_Economica.Columns[4].Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Zonas_Economicas " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Zona_Economica
    /// DESCRIPCION : Da de Alta la Zona Económica con los datos proporcionados por el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 27-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Zona_Economica()
    {
        Cls_Cat_Nom_Zona_Economica_Negocio Rs_Alta_Cat_Nom_Zona_Economica = new Cls_Cat_Nom_Zona_Economica_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            Rs_Alta_Cat_Nom_Zona_Economica.P_Zona_Economica = Txt_Zona_Economica.Text;
            Rs_Alta_Cat_Nom_Zona_Economica.P_Comentarios = Txt_Comentarios_Zona_Economica.Text;
            Rs_Alta_Cat_Nom_Zona_Economica.P_Salario_Diario = Convert.ToDouble(Txt_Salario_Diario_Zona_Economica.Text.Trim());
            Rs_Alta_Cat_Nom_Zona_Economica.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

            Rs_Alta_Cat_Nom_Zona_Economica.Alta_Zona_Economica(); //Da de alta los datos de la Zona Económica proporcionados por el usuario en la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Zona Económica", "alert('El Alta de la Zona Económica fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Zona_Economica " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Zona_Economica
    /// DESCRIPCION : Modifica los datos de la Zona Económica con los proporcionados por el usuario en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 27-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Zona_Economica()
    {
        Cls_Cat_Nom_Zona_Economica_Negocio Rs_Modificar_Cat_Nom_Zona_Economica = new Cls_Cat_Nom_Zona_Economica_Negocio(); //Variable de conexión hacia la capa de Negocios para envio de datos a modificar
        try
        {
            Rs_Modificar_Cat_Nom_Zona_Economica.P_Zona_ID = Txt_Zona_Economica_ID.Text;
            Rs_Modificar_Cat_Nom_Zona_Economica.P_Zona_Economica = Txt_Zona_Economica.Text;
            Rs_Modificar_Cat_Nom_Zona_Economica.P_Salario_Diario = Convert.ToDouble(Txt_Salario_Diario_Zona_Economica.Text.Trim());
            Rs_Modificar_Cat_Nom_Zona_Economica.P_Comentarios = Txt_Comentarios_Zona_Economica.Text;
            Rs_Modificar_Cat_Nom_Zona_Economica.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

            Rs_Modificar_Cat_Nom_Zona_Economica.Modificar_Zona_Economica(); //Sustituye los datos que se encuentran en la BD por lo que introdujo el usuario
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Zona Económica", "alert('La Modificación de la Zona Económica fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Zona_Economica " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Zona_Economica
    /// DESCRIPCION : Elimina los datos de la Zona Económica que fue seleccionada por el Usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 27-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Zona_Economica()
    {
        Cls_Cat_Nom_Zona_Economica_Negocio Rs_Eliminar_Cat_Nom_Zona_Economica = new Cls_Cat_Nom_Zona_Economica_Negocio(); //Variable de conexión hacia la capa de Negocios para la eliminación de los datos
        try
        {
            Rs_Eliminar_Cat_Nom_Zona_Economica.P_Zona_ID = Txt_Zona_Economica_ID.Text;
            Rs_Eliminar_Cat_Nom_Zona_Economica.Eliminar_Zona_Economica(); //Elimina la Zona Económica que selecciono el usuario de la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Zona Económica", "alert('La Eliminación de la Zona Económica fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Eliminar_Zona_Economica " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region (Grid)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Zona_Economica_SelectedIndexChanged
    /// DESCRIPCION : Consulta los datos de la Zona Econímica que selecciono el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 27-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Zona_Economica_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Txt_Zona_Economica_ID.Text = Grid_Zona_Economica.SelectedRow.Cells[1].Text;
            Txt_Zona_Economica.Text = HttpUtility.HtmlDecode(Grid_Zona_Economica.SelectedRow.Cells[2].Text);
            Txt_Comentarios_Zona_Economica.Text = HttpUtility.HtmlDecode(Grid_Zona_Economica.SelectedRow.Cells[4].Text);
            Txt_Salario_Diario_Zona_Economica.Text = HttpUtility.HtmlDecode(Grid_Zona_Economica.SelectedRow.Cells[3].Text);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Grid_Zona_Economica_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpia_Controles();                             //Limpia todos los controles de la forma
            Grid_Zona_Economica.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Llena_Grid_Zonas_Economicas();                  //Carga las Zonas Económicas que estan asignadas a la página seleccionada
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Grid_Zona_Economica_Sorting
    /// 
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Febrero/2011 19:04 pm.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    protected void Grid_Zona_Economica_Sorting(object sender, GridViewSortEventArgs e)
    {
        Consulta_Zonas_Economicas();
        DataTable Dt_Zona_Economica = (Grid_Zona_Economica.DataSource as DataTable);

        if (Dt_Zona_Economica != null)
        {
            DataView Dv_Zona_Economica = new DataView(Dt_Zona_Economica);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Zona_Economica.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Zona_Economica.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Zona_Economica.DataSource = Dv_Zona_Economica;
            Grid_Zona_Economica.DataBind();
        }
    }
    #endregion

    #region (Eventos)
    protected void Btn_Buscar_Zona_Economica_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Consulta_Zonas_Economicas(); //Consulta las Zonas Económicas que coincidan con la zona porporcionada por el usuario
            Limpia_Controles();       //Limpia los controles de la forma
            //Si no se encontraron Zonas con una /// DESCRIPCION similar al proporcionado por el usuario entonces manda un mensaje al usuario
            if (Grid_Zona_Economica.Rows.Count <= 0)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encontraron Zonas Económicas con la /// DESCRIPCION proporcionada <br>";
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
                if (Txt_Zona_Economica.Text != "" & Txt_Comentarios_Zona_Economica.Text.Length <= 250)
                {
                    Alta_Zona_Economica(); //Da de alta los datos proporcionados por el usuario
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                    if (Txt_Zona_Economica.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Zona Económica <br>";
                    }
                    if (Txt_Comentarios_Zona_Economica.Text.Length > 250)
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
                if (Txt_Zona_Economica_ID.Text != "")
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione la Zona Económica que desea modificar sus datos <br>";
                }
            }
            else
            {
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces modifica estos en la BD
                if (Txt_Zona_Economica.Text != "" & Txt_Comentarios_Zona_Economica.Text.Length <= 250)
                {
                    Modificar_Zona_Economica(); //Modifica los datos de la Zona Económica con los datos proporcionados por el usuario
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                    if (Txt_Zona_Economica.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Zona Económica <br>";
                    }
                    if (Txt_Comentarios_Zona_Economica.Text.Length > 250)
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
            //Si el usuario selecciono una Zona Economica entonces la elimina de la base de datos
            if (Txt_Zona_Economica_ID.Text != "")
            {
                Eliminar_Zona_Economica(); //Elimina la Zona Económica que fue seleccionada por el usuario
            }
            //Si el usuario no selecciono alguna Zona Economica manda un mensaje indicando que es necesario que seleccione alguna para
            //poder eliminar
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione la Zona Económica que desea eliminar <br>";
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
                Session.Remove("Consulta_Zonas_Economicas");
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
            Botones.Add(Btn_Buscar_Zona_Economica);

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
