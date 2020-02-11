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
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.Text;
using Presidencia.Indemnizacion.Negocio;
using System.Collections.Generic;

public partial class paginas_Nomina_Frm_Cat_Nom_Indemnizacion : System.Web.UI.Page
{
    #region (Load/Init)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Page_Load
    ///
    /// DESCRIPCIÓN: Carga la configuración inicial de la página.
    ///
    /// CREO: Juan alberto Hernández Negrete
    /// FECHA_CREO: 20/Julio/2011
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN
    ///*******************************************************************************
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

    #region (Métodos)

    #region (Generales)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Configuracion_Inicial
    ///
    /// DESCRIPCIÓN: Configuracion Inicial del Catalogo de Indemnizaciones
    ///
    /// CREO: Juan alberto Hernández Negrete
    /// FECHA_CREO: 20/Julio/2011
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Inicial()
    {
        Habilitar_Controles("Inicial");
        Limpiar_Controles();
        Consulta_Indemnizaciones();
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// 
    /// DESCRIPCION : Limpia los Controles de la pagina.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 20/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        Txt_Indemnizacion.Text = String.Empty;
        Txt_Nombre_Indemnizacion.Text = String.Empty;
        Txt_Dias_Indemnizacion.Text = String.Empty;
        Txt_Comentarios.Text = String.Empty;
        Grid_Indemnizaciones.SelectedIndex = -1;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// 
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    ///               
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    ///                          
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 20/Julio/2011
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
                    Txt_Busqueda_Indemnizacion.Enabled = true;
                    Btn_Busqueda_Indemnizacion.Enabled = true;
                    //Campo de Validacion
                    Lbl_Mensaje_Error.Text = "";
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;

                    Configuracion_Acceso("Frm_Cat_Nom_Indemnizacion.aspx");
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
                    Txt_Busqueda_Indemnizacion.Enabled = true;
                    Btn_Busqueda_Indemnizacion.Enabled = true;
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
                    Txt_Busqueda_Indemnizacion.Enabled = true;
                    Btn_Busqueda_Indemnizacion.Enabled = true;
                    break;
            }
            Txt_Indemnizacion.Enabled = false;
            Txt_Nombre_Indemnizacion.Enabled = Habilitado;
            Txt_Dias_Indemnizacion.Enabled = Habilitado;
            Txt_Comentarios.Enabled = Habilitado;

            Grid_Indemnizaciones.Enabled = !Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region (Consulta)
    /// *****************************************************************************************************
    /// Nombre: Consulta_Indemnizaciones
    /// 
    /// Descripción: Consulta los registros de indemnización en la base de datos.
    /// 
    /// Parámetros: No Áplica.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 20/Julio/2011
    /// Usuario modifico:
    /// Fecha Modifico:
    /// *****************************************************************************************************
    protected void Consulta_Indemnizaciones()
    {
        Cls_Cat_Nom_Indemnizacion_Negocio Obj_Indemnizacion = new Cls_Cat_Nom_Indemnizacion_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Indemnizaciones = null;//Variable que almacena un listado de indemnizaciones.

        try
        {
            if (!String.IsNullOrEmpty(Txt_Busqueda_Indemnizacion.Text.Trim()))
                Obj_Indemnizacion.P_Nombre = Txt_Busqueda_Indemnizacion.Text.Trim();

            Dt_Indemnizaciones = Obj_Indemnizacion.Consultar_Indemnizaciones();//Se ejecuta la consulta de indemnizaciones

            Llenar_Grid(Dt_Indemnizaciones, 0);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las indemnizaciones. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Validaciones)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Indemnizacion
    /// 
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 20/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Indemnizacion()
    {
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (string.IsNullOrEmpty(Txt_Nombre_Indemnizacion.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Nombre es un dato requerido por el sistema. <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Dias_Indemnizacion.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Número de Cuenta es un dato requerido por el sistema. <br>";
            Datos_Validos = false;
        }

        return Datos_Validos;
    }
    #endregion

    #region (Operación)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Indemnizacion
    /// 
    /// DESCRIPCION : Ejecuta el alta de un registro de indemnización.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 20/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Indemnizacion()
    {
        Cls_Cat_Nom_Indemnizacion_Negocio Alta_Indemnizacion = new Cls_Cat_Nom_Indemnizacion_Negocio();//Variable de conexion con la capa de negocios.
        try
        {
            Alta_Indemnizacion.P_Nombre = Txt_Nombre_Indemnizacion.Text.Trim();
            Alta_Indemnizacion.P_Dias = Txt_Dias_Indemnizacion.Text.Trim();
            Alta_Indemnizacion.P_Comentarios = Txt_Comentarios.Text.Trim();
            Alta_Indemnizacion.P_Usuario_Creo = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);

            if (Alta_Indemnizacion.Alta_Indemnizacion())
            {
                Configuracion_Inicial();
                Limpiar_Controles();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al dar de alta un registro de indemnización. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Indemnizacion
    /// 
    /// DESCRIPCION : Ejecuta la Actualizacion los datos del registro de indemnización.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 20/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Indemnizacion()
    {
        Cls_Cat_Nom_Indemnizacion_Negocio Modificar_indemnizacion = new Cls_Cat_Nom_Indemnizacion_Negocio();//Variable de conexion con la capa de negocios.
        try
        {
            Modificar_indemnizacion.P_Indemnizacion_ID = Txt_Indemnizacion.Text.Trim();
            Modificar_indemnizacion.P_Nombre = Txt_Nombre_Indemnizacion.Text.Trim();
            Modificar_indemnizacion.P_Dias = Txt_Dias_Indemnizacion.Text.Trim();
            Modificar_indemnizacion.P_Comentarios = Txt_Comentarios.Text.Trim();
            Modificar_indemnizacion.P_Usuario_Modifico = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);

            if (Modificar_indemnizacion.Actualizar_Indemnizacion())
            {
                Configuracion_Inicial();
                Limpiar_Controles();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al modificar un registro de indemnización. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Indemnizacion
    /// 
    /// DESCRIPCION : Ejecuta la Baja de un registro de indemnización.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 20/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Indemnizacion()
    {
        Cls_Cat_Nom_Indemnizacion_Negocio Eliminar_Indemnizacion = new Cls_Cat_Nom_Indemnizacion_Negocio();//Variable de conexion con la capa de negocios.
        try
        {
            Eliminar_Indemnizacion.P_Indemnizacion_ID = Txt_Indemnizacion.Text.Trim();

            if (Eliminar_Indemnizacion.Eliminar_Indemnizacion())
            {
                Configuracion_Inicial();
                Limpiar_Controles();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al dar de Baja a un Banco. Error: [" + Ex.Message + "]");
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
            Botones.Add(Btn_Busqueda_Indemnizacion);

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

    #region (Grids)

    #region (Generales)
    /// *****************************************************************************************************
    /// Nombre: Llenar_Grid
    /// 
    /// Descripción: Carga la informacion de los registros de indemnizacion consultados en el grid de 
    ///              indemnizaciones.
    /// 
    /// Parámetros: No Áplica.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 20/Julio/2011
    /// Usuario modifico:
    /// Fecha Modifico:
    /// *****************************************************************************************************
    protected void Llenar_Grid(DataTable Dt_Indemnizaciones, Int32 Pagina)
    {
        try
        {
            Grid_Indemnizaciones.Columns[1].Visible = true;
            Grid_Indemnizaciones.DataSource = Dt_Indemnizaciones;
            Grid_Indemnizaciones.DataBind();
            Grid_Indemnizaciones.SelectedIndex = -1;
            Grid_Indemnizaciones.Columns[1].Visible = false;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar el grid de indemnizaciones. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Eventos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Indemnizaciones_PageIndexChanging
    ///
    ///DESCRIPCIÓN: Realiza el Cambio de la pagina de la tabla.
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 20/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Indemnizaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Indemnizaciones.PageIndex = e.NewPageIndex;
            Consulta_Indemnizaciones();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error cambiar de un de la tabla. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Indemnizaciones_SelectedIndexChanged
    ///
    ///DESCRIPCIÓN: Realiza la seleccion de un elemento de la tabla
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 20/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Indemnizaciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Nom_Indemnizacion_Negocio Obj_Indemnizacion = new Cls_Cat_Nom_Indemnizacion_Negocio();//Variable de conexion con l capa de negocios.
        DataTable Dt_Indemnizaciones = null;//Variable que almacenara una lista de los bancos registrados en el sistema.
        String Indemnizacion_ID = "";//Variable que almacena el identificador único del banco.

        try
        {
            int Fila_Seleccionada = Grid_Indemnizaciones.SelectedIndex;

            if (Fila_Seleccionada != -1)
            {
                Indemnizacion_ID = HttpUtility.HtmlDecode(Grid_Indemnizaciones.Rows[Fila_Seleccionada].Cells[1].Text.Trim());

                Obj_Indemnizacion.P_Indemnizacion_ID = Indemnizacion_ID;
                Dt_Indemnizaciones = Obj_Indemnizacion.Consultar_Indemnizaciones();

                if (Dt_Indemnizaciones is DataTable)
                {
                    if (Dt_Indemnizaciones.Rows.Count > 0)
                    {
                        foreach (DataRow INDEMNIZACION in Dt_Indemnizaciones.Rows)
                        {
                            if (!string.IsNullOrEmpty(INDEMNIZACION[Cat_Nom_Indemnizacion.Campo_Indemnizacion_ID].ToString()))
                                Txt_Indemnizacion.Text = INDEMNIZACION[Cat_Nom_Indemnizacion.Campo_Indemnizacion_ID].ToString();

                            if (!string.IsNullOrEmpty(INDEMNIZACION[Cat_Nom_Indemnizacion.Campo_Nombre].ToString()))
                                Txt_Nombre_Indemnizacion.Text = INDEMNIZACION[Cat_Nom_Indemnizacion.Campo_Nombre].ToString();

                            if (!string.IsNullOrEmpty(INDEMNIZACION[Cat_Nom_Indemnizacion.Campo_Dias].ToString()))
                                Txt_Dias_Indemnizacion.Text = INDEMNIZACION[Cat_Nom_Indemnizacion.Campo_Dias].ToString();

                            if (!string.IsNullOrEmpty(INDEMNIZACION[Cat_Nom_Indemnizacion.Campo_Comentarios].ToString()))
                                Txt_Comentarios.Text = INDEMNIZACION[Cat_Nom_Indemnizacion.Campo_Comentarios].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error seleccionar un elemento de la tabla. Error: [" + Ex.Message + "]");
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Grid_Indemnizaciones_Sorting
    /// 
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 20/Julio/2011 19:04 pm.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    protected void Grid_Indemnizaciones_Sorting(object sender, GridViewSortEventArgs e)
    {
        Consulta_Indemnizaciones();
        DataTable Dt_Indemnizaciones = (Grid_Indemnizaciones.DataSource as DataTable);

        if (Dt_Indemnizaciones != null)
        {
            DataView Dv_Indemnizaciones = new DataView(Dt_Indemnizaciones);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Indemnizaciones.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Indemnizaciones.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Indemnizaciones.DataSource = Dv_Indemnizaciones;
            Grid_Indemnizaciones.DataBind();
        }
    }
    #endregion

    #endregion

    #region (Eventos)

    #region (Operacion Alta - Modificar - Eliminar - Consultar)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///
    ///DESCRIPCIÓN: Alta Indemnizacion
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 20/Julio/2011 
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
                if (Validar_Datos_Indemnizacion())
                {
                    Alta_Indemnizacion();
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
    ///
    ///DESCRIPCIÓN: Modificar Indemnización
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 20/Julio/2011
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
                if (Grid_Indemnizaciones.SelectedIndex != -1 & !Txt_Indemnizacion.Text.Equals(""))
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
                if (Validar_Datos_Indemnizacion())
                {
                    Modificar_Indemnizacion();
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
    ///
    ///DESCRIPCIÓN: Eliminar Indemnización
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 20/Julio/2011 
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
                if (Grid_Indemnizaciones.SelectedIndex != -1 & !Txt_Indemnizacion.Text.Equals(""))
                {
                    Eliminar_Indemnizacion();
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
    ///
    ///DESCRIPCIÓN: Salir de la Operacion Actual
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 20/Julio/2011 
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Indemnizacion_Click
    ///
    ///DESCRIPCIÓN: Busqueda Indemnizaciones en el sistema.
    ///
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 20/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Busqueda_Indemnizacion_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Consulta_Indemnizaciones();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error Ejecutar la Búsqueda Indemnizaciones en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (TextBox)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Txt_Dias_Indemnizacion_TextChanged
    /// 
    /// DESCRIPCION : Validar si los dias a regitrar de indemnización aún no se a
    ///               registrado.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 20/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Txt_Dias_Indemnizacion_TextChanged(object sender, EventArgs e) {
        Cls_Cat_Nom_Indemnizacion_Negocio Obj_Indemnizaciones = new Cls_Cat_Nom_Indemnizacion_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Indemnizaciones = null;//Variable que almacena los registros de indemnizaciones.

        try
        {
            Obj_Indemnizaciones.P_Dias = Txt_Dias_Indemnizacion.Text.Trim();
            Dt_Indemnizaciones = Obj_Indemnizaciones.Consultar_Indemnizaciones();

            if (Dt_Indemnizaciones is DataTable)
            {
                if (Dt_Indemnizaciones.Rows.Count > 0)
                {
                    Txt_Dias_Indemnizacion.Text = String.Empty;
                    Lbl_Mensaje_Error.Text = "La cantidad de dias para dar de alta el registro de indemnizacion ya existe.";
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    #endregion

    #endregion
}
