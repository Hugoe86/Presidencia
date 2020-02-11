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
using Presidencia.Programas.Negocios;
using Presidencia.Dependencias.Negocios;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;

public partial class paginas_Nomina_Frm_Cat_Nom_Programas : System.Web.UI.Page
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
    /// FECHA_CREO  : 26-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            Cargar_Combo_Dependencias();    //Consulta todas las dependencias que fueron dadas de alta en la BD
            Limpia_Controles();             //Limpia los controles del forma
            Consulta_Programas();           //Consulta todas los programas que fueron dadas de alta en la BD
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
    /// FECHA_CREO  : 26-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {
            Txt_Programa_ID.Text = "";
            Txt_Nombre_Programa.Text = "";
            Txt_Comentarios_Programa.Text = "";
            Txt_Busqueda_Programa.Text = "";
            Cmb_Estatus_Programa.SelectedIndex = 0;
            Cmb_Dependencia_Programa.SelectedIndex = 0;
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
    /// FECHA_CREO  : 26-Agosto-2010
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
                    Cmb_Estatus_Programa.SelectedIndex = 0;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Cmb_Estatus_Programa.Enabled = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                    Configuracion_Acceso("Frm_Cat_Nom_Programas.aspx");
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Cmb_Estatus_Programa.SelectedIndex = 0;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Cmb_Estatus_Programa.Enabled = false;
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
                    Cmb_Estatus_Programa.Enabled = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    break;
            }
            Txt_Nombre_Programa.Enabled = Habilitado;
            Txt_Comentarios_Programa.Enabled = Habilitado;
            Txt_Busqueda_Programa.Enabled = !Habilitado;
            Btn_Buscar_Programa.Enabled = !Habilitado;
            Cmb_Dependencia_Programa.Enabled = Habilitado;
            Grid_Programa.Enabled = !Habilitado;
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
            Cmb_Dependencia_Programa.DataSource = Dt_Dependencias;
            Cmb_Dependencia_Programa.DataValueField = "Dependencia_ID";
            Cmb_Dependencia_Programa.DataTextField = "Nombre";
            Cmb_Dependencia_Programa.DataBind();
            Cmb_Dependencia_Programa.Items.Insert(0, "---------------------------- < SELECCIONE >---------------------------- ");
            Cmb_Dependencia_Programa.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Combo_Dependencias " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Programas
    /// DESCRIPCION : Consulta los programas que estan dadas de alta en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 26-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Programas()
    {
        Cls_Cat_Nom_Programas_Negocio Rs_Consulta_Cat_Nom_Programas = new Cls_Cat_Nom_Programas_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Programas; //Variable que obtendra los datos de la consulta 

        try
        {
            if (Txt_Busqueda_Programa.Text != "")
            {
                Rs_Consulta_Cat_Nom_Programas.P_Nombre = Txt_Busqueda_Programa.Text;
            }
            Dt_Programas = Rs_Consulta_Cat_Nom_Programas.Consulta_Datos_Programas(); //Consulta todos los programas con sus datos generales            
            Session["Consulta_Programas"] = Dt_Programas;
            Llena_Grid_Programas();
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Programas " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llena_Grid_Programas
    /// DESCRIPCION : Llena el grid con los programas que se encuentran en la base de datos
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 26-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llena_Grid_Programas()
    {
        DataTable Dt_Programas; //Variable que obtendra los datos de la consulta 
        try
        {
            Grid_Programa.DataBind();
            Dt_Programas = (DataTable)Session["Consulta_Programas"];
            Grid_Programa.DataSource = Dt_Programas;
            Grid_Programa.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Programas " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Programa
    /// DESCRIPCION : Da de Alta el programa con los datos proporcionados por el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 26-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Programa()
    {
        Cls_Cat_Nom_Programas_Negocio Rs_Alta_Cat_Nom_Programas = new Cls_Cat_Nom_Programas_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            Rs_Alta_Cat_Nom_Programas.P_Dependencia_ID = Cmb_Dependencia_Programa.SelectedValue;
            Rs_Alta_Cat_Nom_Programas.P_Nombre = Txt_Nombre_Programa.Text;
            Rs_Alta_Cat_Nom_Programas.P_Estatus = Cmb_Estatus_Programa.SelectedValue;
            Rs_Alta_Cat_Nom_Programas.P_Comentarios = Txt_Comentarios_Programa.Text;
            Rs_Alta_Cat_Nom_Programas.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

            Rs_Alta_Cat_Nom_Programas.Alta_Programa(); //Da de alta los datos del programa proporcionados por el usuario en la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Programas", "alert('El Alta del Programa fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Programa " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Programa
    /// DESCRIPCION : Modifica los datos del programa con los proporcionados por el usuario en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 26-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Programa()
    {
        Cls_Cat_Nom_Programas_Negocio Rs_Modificar_Cat_Nom_Programas = new Cls_Cat_Nom_Programas_Negocio(); //Variable de conexión hacia la capa de Negoccios para envio de datos a modificar
        try
        {
            Rs_Modificar_Cat_Nom_Programas.P_Programa_ID = Txt_Programa_ID.Text;
            Rs_Modificar_Cat_Nom_Programas.P_Dependencia_ID = Cmb_Dependencia_Programa.SelectedValue;
            Rs_Modificar_Cat_Nom_Programas.P_Nombre = Txt_Nombre_Programa.Text;
            Rs_Modificar_Cat_Nom_Programas.P_Estatus = Cmb_Estatus_Programa.SelectedValue;
            Rs_Modificar_Cat_Nom_Programas.P_Comentarios = Txt_Comentarios_Programa.Text;
            Rs_Modificar_Cat_Nom_Programas.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

            Rs_Modificar_Cat_Nom_Programas.Modificar_Programa(); //Sustituye los datos que se encuentran en la BD por lo que introdujo el usuario
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Programas", "alert('La Modificación del Programa fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Programa " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Programa
    /// DESCRIPCION : Elimina los datos del Programa que fue seleccionado por el Usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 26-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Programa()
    {
        Cls_Cat_Nom_Programas_Negocio Rs_Eliminar_Cat_Nom_Programas = new Cls_Cat_Nom_Programas_Negocio(); //Variable de conexión hacia la capa de Negocios para la eliminación de los datos
        try
        {
            Rs_Eliminar_Cat_Nom_Programas.P_Programa_ID = Txt_Programa_ID.Text;
            Rs_Eliminar_Cat_Nom_Programas.Eliminar_Programa(); //Elimina el programa que selecciono el usuario de la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Programas", "alert('La Eliminación del Programa fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Eliminar_Programa " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region (Grid)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Programa_SelectedIndexChanged
    /// DESCRIPCION : Consulta los datos del Programa que selecciono el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 26-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Programa_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Nom_Programas_Negocio Rs_Consulta_Cat_Nom_Programas = new Cls_Cat_Nom_Programas_Negocio(); //Variable de conexión a la capa de Negocios para la consulta de los datos
        DataTable Dt_Programas; //Variable que obtendra los datos de la consulta

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Rs_Consulta_Cat_Nom_Programas.P_Programa_ID = Grid_Programa.SelectedRow.Cells[1].Text;
            Dt_Programas = Rs_Consulta_Cat_Nom_Programas.Consulta_Datos_Programas(); //Consulta los datos del programa que fue seleccionado por el usuario
            if (Dt_Programas.Rows.Count > 0)
            {
                //Agrega los valores de los campos a los controles correspondientes de la forma
                foreach (DataRow Registro in Dt_Programas.Rows)
                {
                    Txt_Programa_ID.Text = Registro[Cat_Nom_Programas.Campo_Programa_ID].ToString();
                    Txt_Nombre_Programa.Text = Registro[Cat_Nom_Programas.Campo_Nombre].ToString();
                    Txt_Comentarios_Programa.Text = Registro[Cat_Nom_Programas.Campo_Comentarios].ToString();
                    Cmb_Estatus_Programa.SelectedValue = Registro[Cat_Nom_Programas.Campo_Estatus].ToString();
                    Cmb_Dependencia_Programa.SelectedValue = Registro[Cat_Nom_Programas.Campo_Dependencia_ID].ToString();
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
    protected void Grid_Programa_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpia_Controles();                       //Limpia todos los controles de la forma
            Grid_Programa.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Llena_Grid_Programas();                   //Carga los programas que estan asignadas a la página seleccionada
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Grid_Programa_Sorting
    /// 
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Febrero/2011 19:04 pm.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    protected void Grid_Programa_Sorting(object sender, GridViewSortEventArgs e)
    {
        Consulta_Programas();
        DataTable Dt_Programas = (Grid_Programa.DataSource as DataTable);

        if (Dt_Programas != null)
        {
            DataView Dv_Programas = new DataView(Dt_Programas);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Programas.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Programas.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Programa.DataSource = Dv_Programas;
            Grid_Programa.DataBind();
        }
    }
    #endregion

    #region (Eventos)
    protected void Btn_Buscar_Programa_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Consulta_Programas(); //Consulta los Programas que coincidan con el nombre porporcionado por el usuario
            Limpia_Controles();   //Limpia los controles de la forma
            //Si no se encontraron Programas con un nombre similar al proporcionado por el usuario entonces manda un mensaje al usuario
            if (Grid_Programa.Rows.Count <= 0)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encontraron Programas con el nombre proporcionado <br>";
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
                if (Txt_Nombre_Programa.Text != "" & Cmb_Dependencia_Programa.SelectedIndex > 0 & Txt_Comentarios_Programa.Text.Length <= 250)
                {
                    Alta_Programa(); //Da de alta los datos proporcionados por el usuario
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                    if (Txt_Nombre_Programa.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Nombre del Programa <br>";
                    }
                    if (Cmb_Dependencia_Programa.SelectedIndex == 0)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccionar la Dependecia del Programa <br>";
                    }
                    if (Txt_Comentarios_Programa.Text.Length > 250)
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
                if (Txt_Programa_ID.Text != "")
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el Programa que desea modificar sus datos <br>";
                }
            }
            else
            {
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces modifica estos en la BD
                if (Txt_Nombre_Programa.Text != "" & Cmb_Dependencia_Programa.SelectedIndex > 0 & Txt_Comentarios_Programa.Text.Length <= 250)
                {
                    Modificar_Programa(); //Modifica los datos del Programa con los datos proporcionados por el usuario
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                    if (Txt_Nombre_Programa.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Nombre del Programa <br>";
                    }
                    if (Cmb_Dependencia_Programa.SelectedIndex == 0)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccionar la Dependecia del Programa <br>";
                    }
                    if (Txt_Comentarios_Programa.Text.Length > 250)
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
            //Si el usuario selecciono un Programa entonces la elimina de la base de datos
            if (Txt_Programa_ID.Text != "")
            {
                Eliminar_Programa(); //Elimina el Programa que fue seleccionada por el usuario
            }
            //Si el usuario no selecciono algun Programa manda un mensaje indicando que es necesario que seleccione alguna para
            //poder eliminar
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione el Programa que desea eliminar <br>";
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
                Session.Remove("Consulta_Programas");
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
            Botones.Add(Btn_Buscar_Programa);

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
