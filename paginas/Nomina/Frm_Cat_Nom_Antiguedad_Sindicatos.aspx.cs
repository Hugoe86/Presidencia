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
using Presidencia.Antiguedad_Sindicato.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Collections.Generic;

public partial class paginas_Nomina_Frm_Cat_Nom_Antiguedad_Sindicatos : System.Web.UI.Page
{
    #region (Load)
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Configuracion_Inicial();
                ViewState["SortDirection"] = "ASC";
            }
            Lbl_Mensaje_Error.Text = "";
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    #endregion

    #region (Metodos)

    #region (Metodos Generales)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Inicial
    ///DESCRIPCIÓN: Configuracion Inicial del Catalogo de Antiguedad Sindicatos
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 27/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Inicial()
    {
        Habilitar_Controles("Inicial");
        Limpiar_Controles();
        Consulta_Antiguedad_Sindicatos();
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Ctlr
    /// DESCRIPCION : Limpia los Controles de la pagina.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 27/Diciembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        Txt_Clabe_Antiguedad.Text = "";
        Txt_Anyos_Antiguedad.Text = "";
        Txt_Comentarios_Antiguedad.Text = "";

        Grid_Antiguedad_Sindicatos.SelectedIndex = -1;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 27/Diciembre/2010 
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
                    //Campos de Busqueda
                    Txt_Busqueda_Antiguedad_Sindicatos.Enabled = true;
                    Btn_Busqueda_Antiguedad_Sindicatos.Enabled = true;
                    //Campo de Validacion
                    Lbl_Mensaje_Error.Text = "";
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;

                    Configuracion_Acceso("Frm_Cat_Nom_Antiguedad_Sindicatos.aspx");
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
                    //Campos de Busqueda
                    Txt_Busqueda_Antiguedad_Sindicatos.Enabled = true;
                    Btn_Busqueda_Antiguedad_Sindicatos.Enabled = true;
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
                    //Campos de Busqueda
                    Txt_Busqueda_Antiguedad_Sindicatos.Enabled = true;
                    Btn_Busqueda_Antiguedad_Sindicatos.Enabled = true;
                    break;
            }
            Txt_Clabe_Antiguedad.Enabled = false;
            Txt_Anyos_Antiguedad.Enabled = Habilitado;
            Txt_Comentarios_Antiguedad.Enabled = Habilitado;
            Grid_Antiguedad_Sindicatos.Enabled = !Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region (Metodos Validacion)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Antiguedad_Sindicatos
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 27/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Antiguedad_Sindicatos()
    {
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (string.IsNullOrEmpty(Txt_Anyos_Antiguedad.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Los Años de Antiguedad son Requeridos. <br>";
            Datos_Validos = false;
        }
        else {
            DataTable Dt_Antiguedad_Sindicatos = Consultar_Antiguedades();

            if (Dt_Antiguedad_Sindicatos is DataTable) {
                if (Dt_Antiguedad_Sindicatos.Rows.Count > 0) {
                    foreach (DataRow ANTIGUEDAD in Dt_Antiguedad_Sindicatos.Rows) {
                        if (ANTIGUEDAD is DataRow) {
                            if (!String.IsNullOrEmpty(ANTIGUEDAD[Cat_Nom_Antiguedad_Sindicato.Campo_Anios].ToString().Trim()))
                            {
                                if (Txt_Anyos_Antiguedad.Text.Trim().ToUpper().Equals(ANTIGUEDAD[Cat_Nom_Antiguedad_Sindicato.Campo_Anios].ToString().Trim().ToUpper()))
                                {
                                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Ya existe un registro de antiguedad con el mismo nuemo de años. . <br>";
                                    Datos_Validos = false;
                                }
                            }
                        }
                    }
                }
            }
        }

        if (string.IsNullOrEmpty(Txt_Comentarios_Antiguedad.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Los Comentarios son Requeridos <br>";
            Datos_Validos = false;
        }

        return Datos_Validos;
    }
    #endregion

    #region (Metodos Consulta)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Antiguedad_Sindicatos
    /// DESCRIPCION : Consulta las antiguedades de sindicato registradas en el sistema.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 27/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Antiguedad_Sindicatos() {
        Cls_Cat_Nom_Antiguedad_Sindicato_Negocio Cls_Antiguedad_Sindicatos = new Cls_Cat_Nom_Antiguedad_Sindicato_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Antiguedad_Sindicatos = null;//Variable que almacenara una lista de las antiguedades para los sindicatos registradas.

        try
        {
            if (!string.IsNullOrEmpty(Txt_Busqueda_Antiguedad_Sindicatos.Text))
            {
                Cls_Antiguedad_Sindicatos.P_Anios = Convert.ToInt32(Txt_Busqueda_Antiguedad_Sindicatos.Text);
                Cls_Antiguedad_Sindicatos.P_Antiguedad_Sindicato_ID = Txt_Busqueda_Antiguedad_Sindicatos.Text;
            }

            Dt_Antiguedad_Sindicatos = Cls_Antiguedad_Sindicatos.Consultar_Antiguedad_Sindicato();//Consulta los sindicatos que se encuentran registrados en el sistema
            Cargar_Grid_Antiguedad_Sindicatos(Dt_Antiguedad_Sindicatos);//cargamos el grid  de antiguedad sindicatos
            //Validar que la busqueda halla encontrdo resultados.
            if (Grid_Antiguedad_Sindicatos.Rows.Count == 0 && !string.IsNullOrEmpty(Txt_Busqueda_Antiguedad_Sindicatos.Text))
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encontraron registros de Antiguedad de sindicatos con los datos buscados";
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las antiguedades de sindicatos registradas en el sistema. Error: [" + Ex.Message + "]");
        }
    }

    protected DataTable Consultar_Antiguedades()
    {
        Cls_Cat_Nom_Antiguedad_Sindicato_Negocio Cls_Antiguedad_Sindicatos = new Cls_Cat_Nom_Antiguedad_Sindicato_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Antiguedad_Sindicatos = null;//Variable que almacenara una lista de las antiguedades para los sindicatos registradas.
        try
        {
            Dt_Antiguedad_Sindicatos = Cls_Antiguedad_Sindicatos.Consultar_Antiguedad_Sindicato();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las antiguedades registradas en sistema. Error: [" + Ex.Message + "]");
        }
        return Dt_Antiguedad_Sindicatos;
    }
    #endregion

    #region (Metodos Operacion)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Antiguedad_Sindicato
    /// DESCRIPCION : Ejecuta el alta antiguedad sindicato
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 27/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Antiguedad_Sindicato()
    {
        Cls_Cat_Nom_Antiguedad_Sindicato_Negocio Alta_Antiguedad_Sindicatos = new Cls_Cat_Nom_Antiguedad_Sindicato_Negocio();//Variable de conexion con la capa de negocios.
        try
        {
            Alta_Antiguedad_Sindicatos.P_Anios = Convert.ToInt32(Txt_Anyos_Antiguedad.Text);
            Alta_Antiguedad_Sindicatos.P_Comentarios = Txt_Comentarios_Antiguedad.Text;
            Alta_Antiguedad_Sindicatos.P_Usuario_Creo = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);

            if (Alta_Antiguedad_Sindicatos.Alta_Antiguedad_Sindicato())
            {
                Configuracion_Inicial();
                Limpiar_Controles();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al dar de Alta Antiguedad Sindicato. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Antiguedad_Sindicato
    /// DESCRIPCION : Ejecuta la Actualizacion Antiguedad Sindicato
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 27/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Antiguedad_Sindicato()
    {
        Cls_Cat_Nom_Antiguedad_Sindicato_Negocio Modificar_Antiguedad_Sindicato = new Cls_Cat_Nom_Antiguedad_Sindicato_Negocio();//Variable de conexion con la capa de negocios.
        try
        {
            Modificar_Antiguedad_Sindicato.P_Antiguedad_Sindicato_ID = Txt_Clabe_Antiguedad.Text;
            Modificar_Antiguedad_Sindicato.P_Anios = Convert.ToInt32(Txt_Anyos_Antiguedad.Text);
            Modificar_Antiguedad_Sindicato.P_Comentarios = Txt_Comentarios_Antiguedad.Text;
            Modificar_Antiguedad_Sindicato.P_Usuario_Creo = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);

            if (Modificar_Antiguedad_Sindicato.Modificar_Antiguedad_Sindicato())
            {
                Configuracion_Inicial();
                Limpiar_Controles();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al Actualizar Antiguedad Sindicato. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Antiguedad_Sindicato
    /// DESCRIPCION : Ejecuta la Baja Antiguedad Sindicato
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 27/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Antiguedad_Sindicato()
    {
        Cls_Cat_Nom_Antiguedad_Sindicato_Negocio Eliminar_Antiguedad_Sindicato = new Cls_Cat_Nom_Antiguedad_Sindicato_Negocio();//Variable de conexion con la capa de negocios.
        try
        {
            Eliminar_Antiguedad_Sindicato.P_Antiguedad_Sindicato_ID= Txt_Clabe_Antiguedad.Text;

            if (Eliminar_Antiguedad_Sindicato.Eliminar_Antiguedad_Sindicato())
            {
                Configuracion_Inicial();
                Limpiar_Controles();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al dar de Baja a un Proveedor. Error: [" + Ex.Message + "]");
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
            Botones.Add(Btn_Busqueda_Antiguedad_Sindicatos);

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

    #endregion

    #region (Grid)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Antiguedad_Sindicatos_PageIndexChanging
    ///DESCRIPCIÓN: Realiza el Cambio de la pagina de la tabla.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 27/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Antiguedad_Sindicatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Antiguedad_Sindicatos.PageIndex = e.NewPageIndex;
            Consulta_Antiguedad_Sindicatos();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error cambiar de un de la tabla. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Antiguedad_Sindicatos_SelectedIndexChanged
    ///DESCRIPCIÓN: Realiza la seleccion de un elemento de la tabla
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 27/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Antiguedad_Sindicatos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int Fila_Seleccionada = Grid_Antiguedad_Sindicatos.SelectedIndex;
            if (Fila_Seleccionada != -1)
            {
                Txt_Clabe_Antiguedad.Text = HttpUtility.HtmlDecode(Grid_Antiguedad_Sindicatos.Rows[Fila_Seleccionada].Cells[1].Text);
                Txt_Anyos_Antiguedad.Text = HttpUtility.HtmlDecode(Grid_Antiguedad_Sindicatos.Rows[Fila_Seleccionada].Cells[2].Text);
                Txt_Comentarios_Antiguedad.Text = HttpUtility.HtmlDecode(Grid_Antiguedad_Sindicatos.Rows[Fila_Seleccionada].Cells[3].Text);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error seleccionar un elemento de la tabla. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Grid_Antiguedad_Sindicatos
    /// DESCRIPCION : Carga las antiguedades de sindicato encontradas en el sistema. 
    ///               registradas en el sistema.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 27/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Grid_Antiguedad_Sindicatos(DataTable Dt_Antiguedad_Sindicatos)
    {
        try
        {
            Grid_Antiguedad_Sindicatos.DataSource = Dt_Antiguedad_Sindicatos;
            Grid_Antiguedad_Sindicatos.DataBind();
            Grid_Antiguedad_Sindicatos.SelectedIndex = -1;
            Limpiar_Controles();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar las antiguedades de sindicatos registradas en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Grid_Antiguedad_Sindicatos_Sorting
    /// 
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Febrero/2011 19:04 pm.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    protected void Grid_Antiguedad_Sindicatos_Sorting(object sender, GridViewSortEventArgs e)
    {
        Consulta_Antiguedad_Sindicatos();
        DataTable Dt_Calendario_Nominas = (Grid_Antiguedad_Sindicatos.DataSource as DataTable);

        if (Dt_Calendario_Nominas != null)
        {
            DataView Dv_Calendario_Nominas = new DataView(Dt_Calendario_Nominas);
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

            Grid_Antiguedad_Sindicatos.DataSource = Dv_Calendario_Nominas;
            Grid_Antiguedad_Sindicatos.DataBind();
        }
    }
    #endregion

    #region (Eventos)

    #region (Operacion Alta - Modificar - Eliminar - Consultar)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Alta Antiguedad Sindicato
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 27/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.ToolTip.Equals("Nuevo"))
            {
                Habilitar_Controles("Nuevo");
                Limpiar_Controles();
            }
            else
            {
                if (Validar_Datos_Antiguedad_Sindicatos())
                {
                    Alta_Antiguedad_Sindicato();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Modificar Antiguedad Sindicato
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 27/Diciembre/2010 
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

            if (Btn_Modificar.ToolTip.Equals("Modificar"))
            {
                if (Grid_Antiguedad_Sindicatos.SelectedIndex != -1 & !Txt_Clabe_Antiguedad.Text.Equals(""))
                {
                    Habilitar_Controles("Modificar");
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el registro que desea modificar sus datos <br>";
                }
            }
            else
            {
                if (Validar_Datos_Antiguedad_Sindicatos())
                {
                    Modificar_Antiguedad_Sindicato();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: Eliminar Antiguedad Sindicato
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 27/Diciembre/2010 
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

            if (Btn_Eliminar.ToolTip.Equals("Eliminar"))
            {
                if (Grid_Antiguedad_Sindicatos.SelectedIndex != -1 & !Txt_Clabe_Antiguedad.Text.Equals(""))
                {
                    Eliminar_Antiguedad_Sindicato();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el registro que desea eliminar <br>";
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Salir de la Operacion Actual
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 27/Diciembre/2010 
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Antiguedad_Sindicatos_Click
    ///DESCRIPCIÓN: Busqueda Antiguedad de Sindicato
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 27/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Busqueda_Antiguedad_Sindicatos_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Consulta_Antiguedad_Sindicatos();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error Ejecutar la Búsqueda Antiguedad Sindicatos. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion

}
