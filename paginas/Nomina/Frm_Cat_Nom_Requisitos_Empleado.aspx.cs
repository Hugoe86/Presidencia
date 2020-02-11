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
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;

using Presidencia.Requisitos_Empleados.Negocios;

public partial class paginas_Nomina_Frm_Cat_Nom_Requisitos_Empleado : System.Web.UI.Page
{

    #region (Load)
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Configuracion_Inicial();
            ViewState["SortDirection"] = "ASC";
        }
    }
    #endregion

    #region (Eventos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Habilita la Configuracion para realizar el alta de 
    ///un Requisito del Empleado
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                Limpiar_Ctlrs();           //Limpia los controles de la forma para poder introducir nuevos datos
            }
            else
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces da de alta los mismo en la base de datos
                if (Txt_Nombre_Requisito.Text != ""  & Cmb_Estatus_Requisito.SelectedIndex > 0 &  Cmb_Aplica_Archivo.SelectedIndex > 0)
                {
                    Alta_Requisito_Empleado(); //Da de alta los datos proporcionados por el usuario
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                    if (Txt_Nombre_Requisito.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Nombre del Requisito <br>";
                    }
                    if (Cmb_Estatus_Requisito.SelectedIndex <= 0)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccionar el Estatus del Requisito <br>";
                    }
                    if (Cmb_Aplica_Archivo.SelectedIndex <= 0)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Selecciona si el Requisito sera un archivo <br>";
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
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Habilita la Configuracion para realizar la Actualizacion de 
    ///un Requisito del Empleado
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                if (Txt_Requisito_ID.Text != "")
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el Requisto que desea modificar sus datos <br>";
                }
            }
            else
            {
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces modifica estos valores en la base de datos
                if (Txt_Nombre_Requisito.Text != "" & Cmb_Estatus_Requisito.SelectedIndex > 0 & Cmb_Aplica_Archivo.SelectedIndex > 0)
                {
                    Actualizar_Requisito_Empleado(); 
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                    if (Txt_Nombre_Requisito.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Nombre del Requisito <br>";
                    }
                    if (Cmb_Estatus_Requisito.SelectedIndex <= 0)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccionar el Estatus del Requisito <br>";
                    }
                    if (Cmb_Aplica_Archivo.SelectedIndex <= 0)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Selecciona si el Requisito sera un archivo <br>";
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
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: Elimina el Registro Seleccionado
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            //Si el usuario selecciono un Área entonces la elimina de la base de datos
            if (Txt_Requisito_ID.Text != "")
            {
                Eliminar_Requisito_Empleado(); //Elimina el Área que fue seleccionada por el usuario
            }
            //Si el usuario no selecciono algun Área manda un mensaje indicando que es necesario que seleccione alguna para
            //poder eliminar
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione el requisito que desea eliminar <br>";
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la Operacion Actual.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Inicio")
            {
                Session.Remove("Requisitos_Empleado");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Configuracion_Inicial();//Habilita los controles para la siguiente operación del usuario en el catálogo
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Requisitos_Empleado_Click
    ///DESCRIPCIÓN: Busqueda de los requisitos de los empleados
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Requisitos_Empleado_Click(object sender, ImageClickEventArgs e)
    {
        Busqueda_Requisitos_Empleado(Txt_Busqueda_Requistos_Empleado.Text, Grid_Requistos);
    }
    #endregion

    #region (Grid)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consulta_Requisitos_Empleado
    ///DESCRIPCIÓN: Consulta los requisitos de los empleados
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consulta_Requisitos_Empleado() {
        Cls_Cat_Nom_Requisitos_Empleado_Negocio Cat_Requisitos_Empleados = new Cls_Cat_Nom_Requisitos_Empleado_Negocio();
        DataTable Dt_Requisitos_Empleado;
        try {
            Dt_Requisitos_Empleado = Cat_Requisitos_Empleados.Consultar_Requisitos_Empleados();
            Session["Requisitos_Empleado"] = Dt_Requisitos_Empleado;
            Grid_Requistos.DataSource = (DataTable)Session["Requisitos_Empleado"];
            Grid_Requistos.DataBind();
        }
        catch (Exception Ex) {
            throw new Exception("Cosultar Requisitos Empleados " + Ex.Message.ToString(), Ex);
        }        
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Requistos_PageIndexChanging
    ///DESCRIPCIÓN:Cambiar de Pagina de la tabla de requisitos de los empleados
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Requistos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Requistos.PageIndex = e.NewPageIndex;
            Consulta_Requisitos_Empleado();
            Grid_Requistos.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Al Cambiar de Pagina del Grid Requisitos Empleados " + Ex.Message.ToString(), Ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consulta_Requisitos_Empleado
    ///DESCRIPCIÓN: seleccionar algun elemento de la tabla de Requisitos de los empleados
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Requistos_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = Grid_Requistos.SelectedIndex;
        try
        {
            Txt_Requisito_ID.Text = HttpUtility.HtmlDecode(Grid_Requistos.Rows[index].Cells[1].Text);
            Txt_Nombre_Requisito.Text = HttpUtility.HtmlDecode(Grid_Requistos.Rows[index].Cells[2].Text);
            Cmb_Estatus_Requisito.SelectedIndex = Cmb_Estatus_Requisito.Items.IndexOf(Cmb_Estatus_Requisito.Items.FindByText(HttpUtility.HtmlDecode(Grid_Requistos.Rows[index].Cells[3].Text)));
            Cmb_Aplica_Archivo.SelectedIndex = Cmb_Aplica_Archivo.Items.IndexOf(Cmb_Aplica_Archivo.Items.FindByText(HttpUtility.HtmlDecode(Grid_Requistos.Rows[index].Cells[4].Text)));
        }
        catch (Exception Ex)
        {
            throw new Exception("Al Seleccionar Requisitos Empleados " + Ex.Message.ToString(), Ex);
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Grid_Requistos_Sorting
    /// 
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Febrero/2011 19:04 pm.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    protected void Grid_Requistos_Sorting(object sender, GridViewSortEventArgs e)
    {
        Consulta_Requisitos_Empleado();
        DataTable Dt_Requisitos = (Grid_Requistos.DataSource as DataTable);

        if (Dt_Requisitos != null)
        {
            DataView Dv_Requisitos = new DataView(Dt_Requisitos);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Requisitos.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Requisitos.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Requistos.DataSource = Dv_Requisitos;
            Grid_Requistos.DataBind();
        }
    }
    #endregion

    #region (Metodos)
    private void Configuracion_Inicial() {
        Consulta_Requisitos_Empleado();
        Limpiar_Ctlrs();
        Habilitar_Controles("Inicial");
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 19/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado; 

        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Cmb_Aplica_Archivo.SelectedIndex = -1;
                    Cmb_Estatus_Requisito.SelectedIndex = -1;
                    Txt_Busqueda_Requistos_Empleado.Enabled = true;
                    Btn_Buscar_Requisitos_Empleado.Enabled = true;

                    Configuracion_Acceso("Frm_Cat_Nom_Requisitos_Empleado.aspx");
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
                    Cmb_Aplica_Archivo.SelectedIndex = -1;
                    Cmb_Estatus_Requisito.SelectedIndex = -1;
                    Txt_Busqueda_Requistos_Empleado.Enabled = false;
                    Btn_Buscar_Requisitos_Empleado.Enabled = false;
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
                    Txt_Busqueda_Requistos_Empleado.Enabled = false;
                    Btn_Buscar_Requisitos_Empleado.Enabled = false;
                    break;
            }
            Txt_Requisito_ID.Enabled = false;
            Txt_Nombre_Requisito.Enabled = Habilitado;            
            Cmb_Estatus_Requisito.Enabled = Habilitado;
            Cmb_Aplica_Archivo.Enabled = Habilitado;            
            Grid_Requistos.Enabled = !Habilitado;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Ctlr
    /// DESCRIPCION : Limpia los Controles de la pagina.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 19/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Ctlrs() {
        Txt_Requisito_ID.Text = "";
        Txt_Nombre_Requisito.Text = "";
        Cmb_Aplica_Archivo.SelectedIndex = -1;
        Cmb_Aplica_Archivo.SelectedIndex = -1;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta Requisito del Empleado
    /// DESCRIPCION : Ejecuta el Alta del Empleado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 19/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************    
    private void Alta_Requisito_Empleado() {
        Cls_Cat_Nom_Requisitos_Empleado_Negocio Cat_Requisitos_Empleados = new Cls_Cat_Nom_Requisitos_Empleado_Negocio();
        try {
            Cat_Requisitos_Empleados.P_Nombre = HttpUtility.HtmlDecode( Txt_Nombre_Requisito.Text );
            Cat_Requisitos_Empleados.P_Estatus = HttpUtility.HtmlDecode(Cmb_Estatus_Requisito.SelectedItem.Text);
            Cat_Requisitos_Empleados.P_Archivo = HttpUtility.HtmlDecode(Cmb_Aplica_Archivo.SelectedItem.Text);
            Cat_Requisitos_Empleados.P_Usuario_Creo = HttpUtility.HtmlDecode( (String) Cls_Sessiones.Nombre_Empleado );
            if (Cat_Requisitos_Empleados.Alta_Requisito_Empleado()) {
                Configuracion_Inicial();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "","alert('Operación Exitosa [Alta Requisito del Empleado]');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Alta de Requisito del Empleado " + Ex.Message.ToString(), Ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Actualizar_Requisito_Empleado
    /// DESCRIPCION : Ejecuta la Actualizacion del Requisito del Empleado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 19/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************    
    private void Actualizar_Requisito_Empleado()
    {
        Cls_Cat_Nom_Requisitos_Empleado_Negocio Cat_Requisitos_Empleados = new Cls_Cat_Nom_Requisitos_Empleado_Negocio();
        try
        {
            Cat_Requisitos_Empleados.P_Requisito_ID = HttpUtility.HtmlDecode(Txt_Requisito_ID.Text);
            Cat_Requisitos_Empleados.P_Nombre = HttpUtility.HtmlDecode(Txt_Nombre_Requisito.Text);
            Cat_Requisitos_Empleados.P_Estatus = HttpUtility.HtmlDecode(Cmb_Estatus_Requisito.SelectedItem.Text);
            Cat_Requisitos_Empleados.P_Archivo = HttpUtility.HtmlDecode(Cmb_Aplica_Archivo.SelectedItem.Text);
            Cat_Requisitos_Empleados.P_Usuario_Modifico = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);
            if (Cat_Requisitos_Empleados.Actualizar_Requisito_Empleado())
            {
                Configuracion_Inicial();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa [Actualización Requisito del Empleado]');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Actualizacion de Requisito del Empleado " + Ex.Message.ToString(), Ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Actualizar_Requisito_Empleado
    /// DESCRIPCION : Ejecuta la Actualizacion del Requisito del Empleado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 19/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************    
    private void Eliminar_Requisito_Empleado()
    {
        Cls_Cat_Nom_Requisitos_Empleado_Negocio Cat_Requisitos_Empleados = new Cls_Cat_Nom_Requisitos_Empleado_Negocio();
        try
        {
            Cat_Requisitos_Empleados.P_Requisito_ID = HttpUtility.HtmlDecode(Txt_Requisito_ID.Text);

            if (Cat_Requisitos_Empleados.Eliminar_Requisito_Empleado())
            {
                Configuracion_Inicial();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa [Eliminar Requisito del Empleado]');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Actualizacion de Requisito del Empleado " + Ex.Message.ToString(), Ex);
        }
    }
    /// ********************************************************************************
    /// Nombre:Busqueda_Grupos_Percepcion_Deduccion_Por_Nombre
    /// Descripción: Ejecuta la busqueda de la percepcion deduccion por el nombre.
    /// Creo: Juan Alberto Hernández Negrete 
    /// Fecha Creo: 24 de Septiembre del 2010 1:35 p.m.
    /// Modifico:
    /// Fecha Modifico:
    /// Causa Modifico:
    /// ********************************************************************************
    private void Busqueda_Requisitos_Empleado(String Nombre, GridView _GridView)
    {
        DataTable Dt_Requisitos = (DataTable)Session["Requisitos_Empleado"];
        DataView dv = new DataView(Dt_Requisitos);
        String Expresion_De_Busqueda = null;

        Expresion_De_Busqueda = string.Format("{0} '%{1}%'", _GridView.SortExpression, Nombre);

        dv.RowFilter = Cat_Nom_Requisitos_Empleados.Campo_Nombre + " like " + Expresion_De_Busqueda;
        _GridView.DataSource = dv;
        _GridView.DataBind();
    }//End Function
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
            Botones.Add(Btn_Buscar_Requisitos_Empleado);

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

