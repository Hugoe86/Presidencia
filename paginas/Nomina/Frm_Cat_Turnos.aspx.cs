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
using System.Globalization;
using Presidencia.Turnos.Negocios;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;

public partial class paginas_Nomina_Frm_Cat_Turnos : System.Web.UI.Page
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

            Lbl_Mensaje_Error.Text = String.Empty;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
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
            Limpia_Controles(); //Limpia los controles del forma
            Consulta_Turnos(); //Consulta todos los turnos que estan dados de alta en la base de datos
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
            Txt_Turno_ID.Text = "";
            Txt_Descripcion_Turno.Text = "";
            Txt_Hora_Entrada_Turno.Text = "";
            Txt_Hora_Salida_Turno.Text = "";
            Txt_Comentarios_Turno.Text = "";
            Txt_Busqueda_Turno.Text = "";
            Cmb_Estatus_Turno.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///                para a siguiente operación
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
                    Cmb_Estatus_Turno.SelectedIndex = 0;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Cmb_Estatus_Turno.Enabled = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                    Configuracion_Acceso("Frm_Cat_Turnos.aspx");

                    break;

                case "Nuevo":
                    Habilitado = true;
                    Cmb_Estatus_Turno.SelectedIndex = 0;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Cmb_Estatus_Turno.Enabled = false;
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
                    Cmb_Estatus_Turno.Enabled = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    break;
            }
            Txt_Descripcion_Turno.Enabled = Habilitado;
            Txt_Hora_Entrada_Turno.Enabled = Habilitado;
            Txt_Hora_Salida_Turno.Enabled = Habilitado;
            Txt_Comentarios_Turno.Enabled = Habilitado;
            Txt_Busqueda_Turno.Enabled = !Habilitado;
            Btn_Buscar_Turno.Enabled = !Habilitado;
            Grid_Turno.Enabled = !Habilitado;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Turnos
    /// DESCRIPCION : Consulta los turno que estan dados de alta en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Turnos()
    {
        Cls_Cat_Turnos_Negocio Rs_Consulta_Cat_Turnos = new Cls_Cat_Turnos_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Turnos; //Variable que obtendra los datos de la consulta 

        try
        {
            if (Txt_Busqueda_Turno.Text != "")
            {
                Rs_Consulta_Cat_Turnos.P_Descripcion = Txt_Busqueda_Turno.Text;
            }
            Dt_Turnos = Rs_Consulta_Cat_Turnos.Consulta_Datos_Turnos(); //Consulta todas los turnos con sus datos generales            
            Session["Consulta_Turnos"] = Dt_Turnos;
            Llena_Grid_Turnos();
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Turnos " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llena_Grid_Turnos
    /// DESCRIPCION : Llena el grid con los turnos que se encuentran en la base de datos
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llena_Grid_Turnos()
    {
        DataTable Dt_Turnos; //Variable que obtendra los datos de la consulta 
        Int32 Total_Registros = 1;

        try
        {
            Grid_Turno.DataBind();
            Dt_Turnos = (DataTable)Session["Consulta_Turnos"];
            Grid_Turno.DataSource = Dt_Turnos;
            Grid_Turno.DataBind();

           if (Dt_Turnos is DataTable)
           {
               Total_Registros = (Dt_Turnos.Rows.Count == 0) ? Total_Registros : Dt_Turnos.Rows.Count;
               custPager.TotalPages =
                   ((Total_Registros % Grid_Turno.PageSize) == 0) ?
                   (Total_Registros / Grid_Turno.PageSize) :
                   (Total_Registros / Grid_Turno.PageSize + 1);
           }
           else
           {
               custPager.TotalPages = 1;
           }
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Turnos" + ex.Message.ToString(), ex);
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Grid_Turno_Sorting
    /// 
    /// DESCRIPCIÓN:Valida la información ingresada en el catálogo de turnos.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 24/Junio/2011
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    private Boolean Validar_Datos()
    {
        Boolean Estatus = true;
        Lbl_Mensaje_Error.Text = "Es necesario ingresar:<br />";

        try
        {
            if (String.IsNullOrEmpty(Txt_Descripcion_Turno.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Descripción del turno es un dato requerido. <br />";
                Estatus = false;
            }

            if (String.IsNullOrEmpty(Txt_Hora_Entrada_Turno.Text.Trim().Replace("__:__:__", "")))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Hora de Entrada del Turno <br>";
                Estatus = false;
            }

            if (String.IsNullOrEmpty(Txt_Hora_Entrada_Turno.Text.Trim().Replace("__:__:__", "")))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Hora de Salida del Turno <br>";
                Estatus = false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar los datos. Error: [" + Ex.Message + "]");
        }
        return Estatus;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Turno
    /// DESCRIPCION : Da de Alta el Turno con los datos proporcionados por el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Turno()
    {
        Cls_Cat_Turnos_Negocio Rs_Alta_Cat_Turnos = new Cls_Cat_Turnos_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            Rs_Alta_Cat_Turnos.P_Descripcion = Txt_Descripcion_Turno.Text;
            Rs_Alta_Cat_Turnos.P_Hora_Entrada = Convert.ToDateTime(String.Format("{0:T}", Txt_Hora_Entrada_Turno.Text));
            Rs_Alta_Cat_Turnos.P_Hora_Salida = Convert.ToDateTime(String.Format("{0:T}", Txt_Hora_Salida_Turno.Text));
            Rs_Alta_Cat_Turnos.P_Estatus = Cmb_Estatus_Turno.SelectedValue;
            Rs_Alta_Cat_Turnos.P_Comentarios = Txt_Comentarios_Turno.Text;
            Rs_Alta_Cat_Turnos.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

            Rs_Alta_Cat_Turnos.Alta_Turnos(); //Da de alta todos los datos en la base de datos
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Turnos", "alert('El Alta del Turno fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Turno " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Turno
    /// DESCRIPCION : Modifica los datos del Turno con los proporcionados por el usuario
    ///               en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Turno()
    {
        Cls_Cat_Turnos_Negocio Rs_Modificar_Cat_Turnos = new Cls_Cat_Turnos_Negocio();  //Variable de conexión hacia la capa de Negoccios para envio de datos a modificar
        try
        {
            Rs_Modificar_Cat_Turnos.P_Turno_ID = Txt_Turno_ID.Text;
            Rs_Modificar_Cat_Turnos.P_Descripcion = Txt_Descripcion_Turno.Text;
            Rs_Modificar_Cat_Turnos.P_Hora_Entrada = Convert.ToDateTime(String.Format("{0:T}", Txt_Hora_Entrada_Turno.Text));
            Rs_Modificar_Cat_Turnos.P_Hora_Salida = Convert.ToDateTime(String.Format("{0:T}", Txt_Hora_Salida_Turno.Text));
            Rs_Modificar_Cat_Turnos.P_Estatus = Cmb_Estatus_Turno.SelectedValue;
            Rs_Modificar_Cat_Turnos.P_Comentarios = Txt_Comentarios_Turno.Text;
            Rs_Modificar_Cat_Turnos.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

            Rs_Modificar_Cat_Turnos.Modificar_Turno(); //Sustituye los datos que se encuentran en la BD por lo que introdujo el usuario
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Áreas", "alert('La Modificación del Turno fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Turno " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Turno
    /// DESCRIPCION : Elimina los datos del Turno que fue seleccionada por el Usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Turno()
    {
        Cls_Cat_Turnos_Negocio Rs_Eliminar_Cat_Turnos = new Cls_Cat_Turnos_Negocio(); //Variable de conexión hacia la capa de Negocios para la eliminación de los datos

        try
        {
            Rs_Eliminar_Cat_Turnos.P_Turno_ID = Txt_Turno_ID.Text;
            Rs_Eliminar_Cat_Turnos.Eliminar_Turno(); //Elimina el turno que selecciono el usuario de la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Turnos", "alert('La Eliminación del Turno fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Eliminar_Turno " + ex.Message.ToString(), ex);
        }
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
    protected void Grid_Turno_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Turnos_Negocio Rs_Consulta_Cat_Turnos = new Cls_Cat_Turnos_Negocio(); //Variable de conexión a la capa de Negocios para la consulta de los datos del área
        DataTable Dt_Turnos; //Variable que obtendra los datos de la consulta        
        DateTime Fecha_Hora; //Guarda la fecha con la hora que se tiene en la base de datos
        String Hora;         //Obtiene la hora del turno

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Rs_Consulta_Cat_Turnos.P_Turno_ID = Grid_Turno.SelectedRow.Cells[1].Text;
            Dt_Turnos = Rs_Consulta_Cat_Turnos.Consulta_Datos_Turnos(); //Consulta los datos del turno que fue seleccionada por el usuario

            //Agrega los valores de los campos a los controles correspondientes de la forma
            foreach (DataRow Registro in Dt_Turnos.Rows)
            {
                Txt_Turno_ID.Text = Registro[Cat_Turnos.Campo_Turno_ID].ToString();
                Cmb_Estatus_Turno.SelectedValue = Registro[Cat_Turnos.Campo_Estatus].ToString();
                Txt_Descripcion_Turno.Text = Registro[Cat_Turnos.Campo_Descripcion].ToString();
                Txt_Comentarios_Turno.Text = Registro[Cat_Turnos.Campo_Comentarios].ToString();

                Fecha_Hora = Convert.ToDateTime(Registro[Cat_Turnos.Campo_Hora_Entrada].ToString());
                Hora = String.Format("{0:HH:mm:ss tt}", Fecha_Hora);//, DateTimeFormatInfo.InvariantInfo);
                Txt_Hora_Entrada_Turno.Text = String.Empty;
                Txt_Hora_Entrada_Turno.Text = Hora;
                

                Fecha_Hora = Convert.ToDateTime(Registro[Cat_Turnos.Campo_Hora_Salida].ToString());
                Hora = Fecha_Hora.ToString("T", DateTimeFormatInfo.InvariantInfo);
                Txt_Hora_Salida_Turno.Text = Hora;
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    protected void Grid_Turno_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpia_Controles(); //Limpia todos los controles de la forma
            Grid_Turno.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Llena_Grid_Turnos(); //Carga los turnos que estan asignadas a la página seleccionada
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Grid_Turno_Sorting
    /// 
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Febrero/2011 19:04 pm.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    protected void Grid_Turno_Sorting(object sender, GridViewSortEventArgs e)
    {
        Consulta_Turnos();
        DataTable Dt_Turnos = (Grid_Turno.DataSource as DataTable);

        if (Dt_Turnos != null)
        {
            DataView Dv_Turnos = new DataView(Dt_Turnos);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Turnos.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Turnos.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Turno.DataSource = Dv_Turnos;
            Grid_Turno.DataBind();
        }
    }
    #endregion

    #region (Eventos)
    protected void Btn_Nuevo_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Nuevo.ToolTip.Trim().Equals("Nuevo"))
            {
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                Limpia_Controles(); //Limpia los controles de la forma para poder introducir nuevos datos
            }
            else
            {
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces da de alta los mismo en la base de datos
                if (Validar_Datos())
                {
                    Alta_Turno(); //Da de alta los datos proporcionados por el usuario
                }
                else {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
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
            if (Btn_Modificar.ToolTip.Trim().Equals("Modificar"))
            {
                if (!String.IsNullOrEmpty(Txt_Turno_ID.Text))
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el Turno que desea modificar sus datos <br>";
                }
            }
            else
            {
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces modifica estos valores en la base de datos
                if (Validar_Datos())
                {
                    Modificar_Turno(); //Modifica los datos del Turno con los datos proporcionados por el usuario
                }
                else {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
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
            //Si el usuario selecciono un Turno entonces la elimina de la base de datos
            if (!String.IsNullOrEmpty(Txt_Turno_ID.Text))
            {
                Eliminar_Turno(); //Elimina el Turno que fue seleccionada por el usuario
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione el Turno que desea eliminar <br>";
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Btn_Buscar_Turno_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Consulta_Turnos(); //Consulta los turnos que coincidan con la descipción porporcionada por el usuario
            Limpia_Controles(); //Limpia los controles de la forma
            //Si no se encontraron turnos con una /// DESCRIPCION similar proporcionada por el usuario entonces manda un mensaje al usuario
            if (Grid_Turno.Rows.Count <= 0)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encontraron Turnos con la /// DESCRIPCION proporcionada <br>";
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
                Session.Remove("Consulta_Turnos");
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
    protected void custPager_PageChanged(object sender, CustomPageChangeArgs e)
    {
        Grid_Turno.PageSize = e.CurrentPageSize;
        Grid_Turno.PageIndex = e.CurrentPageNumber;
        Consulta_Turnos();
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
            Botones.Add(Btn_Buscar_Turno);

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
