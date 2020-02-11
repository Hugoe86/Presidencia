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
using Presidencia.Proveedores.Negocios;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Text.RegularExpressions;
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;

public partial class paginas_Nomina_Frm_Cat_Nom_Proveedores : System.Web.UI.Page
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
        catch (Exception Ex) {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;            
        }
    }
    #endregion

    #region (Grid)

    #region(Grid Proveedores)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Proveedores
    ///DESCRIPCIÓN: Consulta los Proveedores que exiten en la Base de Datos
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Proveedores() {
        Cls_Cat_Nom_Proveedores_Negocio Proveedores = new Cls_Cat_Nom_Proveedores_Negocio();
        DataTable Dt_Proveedres = null;
        try
        {
            Dt_Proveedres = Proveedores.Consultar_Proveedores();
            Session["Proveedores"] = Dt_Proveedres;
            Grid_Proveedores.DataSource = (DataTable)Session["Proveedores"];
            Grid_Proveedores.DataBind();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar la tabla de proveeores. Error generado [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Proveedores_PageIndexChanging
    ///DESCRIPCIÓN: Realiza el Cambio de la pagina de la tabla.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Proveedores_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Proveedores.PageIndex = e.NewPageIndex;
            Consultar_Proveedores();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error cambiar de un de la tabla. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Proveedores_SelectedIndexChanged
    ///DESCRIPCIÓN: Realiza la seleccion de un elemento de la tabla
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Proveedores_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = ((Grid_Proveedores.PageIndex) * Grid_Proveedores.PageSize) + (Grid_Proveedores.SelectedIndex);
            Cargar_Datos_Proveedor(index);
            Consultar_Deducciones_Proveedores();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error seleccionar un elemento de la tabla. Error: [" + Ex.Message + "]");
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Grid_Proveedores_Sorting
    /// 
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Febrero/2011 19:04 pm.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    protected void Grid_Proveedores_Sorting(object sender, GridViewSortEventArgs e)
    {
        Consultar_Proveedores();
        DataTable Dt_Proveedores = (Grid_Proveedores.DataSource as DataTable);

        if (Dt_Proveedores != null)
        {
            DataView Dv_Proveedores = new DataView(Dt_Proveedores);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Proveedores.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Proveedores.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Proveedores.DataSource = Dv_Proveedores;
            Grid_Proveedores.DataBind();
        }
    }
    #endregion

    #region (Grid Deducciones)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Deducciones_RowDataBound
    /// DESCRIPCION : Agrega un identificador al boton de cancelar de la tabla
    /// para identicar la fila seleccionada de tabla.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 11/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Deducciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                ((ImageButton)e.Row.Cells[2].FindControl("Btn_Delete_Deduccion")).CommandArgument = e.Row.Cells[0].Text.Trim();
                ((ImageButton)e.Row.Cells[2].FindControl("Btn_Delete_Deduccion")).ToolTip = "Quitar la Deduccion " + e.Row.Cells[1].Text + " al Proveedor";
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    #endregion

    #endregion

    #region (Metodos)

    #region (Metodos Generales)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Inicial
    ///DESCRIPCIÓN: Configuracion Inicial del Catalogo de Proveedores
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Inicial()
    {
        Habilitar_Controles("Inicial");
        Limpiar_Controles();
        Consultar_Deducciones();
        Consultar_Proveedores();
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
    private void Limpiar_Controles()
    {
        //Datos del Proveedor
        Txt_Proveedor_ID.Text = "";
        Cmb_Aval.SelectedIndex = -1;
        Txt_Nombre_Proveedor.Text = "";
        Txt_RFC_Proveedor.Text = "";
        Cmb_Estatus_Proveedor.SelectedIndex = -1;
        Txt_Calle_Proveedor.Text = "";
        Txt_Numero_Proveedor.Text = "";
        Txt_Colonia_Proveedor.Text = "";
        Txt_Codigo_Postal_Proveedor.Text = "";
        Txt_Ciudad_Proveedor.Text = "";
        Txt_Estado_Proveedor.Text = "";
        Txt_Telefono_Proveedor.Text = "";
        Txt_Fax_Proveedor.Text = "";
        Txt_Email_Proveedor.Text = "";
        Txt_Contacto_Proveedor.Text = "";
        Txt_Comentarios_Proveedor.Text = "";

        if (Session["Dt_Deducciones_Grid"] != null)
        {
            Session.Remove("Dt_Deducciones_Grid");
        }

        Grid_Deducciones.DataSource = new DataTable();
        Grid_Deducciones.DataBind();
        Grid_Deducciones.SelectedIndex = -1;
        Grid_Proveedores.SelectedIndex = -1;
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

                    Txt_Busqueda_Proveedor.Enabled = true;
                    Btn_Buscar_Proveedor.Enabled = true;

                    Lbl_Mensaje_Error.Text = "";
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    TPnl_Contenedor.ActiveTabIndex = 0;
                    Cmb_Estatus_Proveedor.Enabled = Habilitado;
                    Configuracion_Acceso("Frm_Cat_Nom_Proveedores.aspx");
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Cmb_Estatus_Proveedor.SelectedIndex = 1;
                    Cmb_Estatus_Proveedor.Enabled = !Habilitado;
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

                    Txt_Busqueda_Proveedor.Enabled = false;
                    Btn_Buscar_Proveedor.Enabled = false;
                    TPnl_Contenedor.ActiveTabIndex = 1;
                    break;

                case "Modificar":
                    Habilitado = true;
                    Cmb_Estatus_Proveedor.Enabled = Habilitado;
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

                    Txt_Busqueda_Proveedor.Enabled = false;
                    Btn_Buscar_Proveedor.Enabled = false;
                    TPnl_Contenedor.ActiveTabIndex = 1;
                    break;
            }
            Txt_Proveedor_ID.Enabled = false;
            Cmb_Aval.Enabled = Habilitado;
            Txt_Nombre_Proveedor.Enabled = Habilitado;
            Txt_RFC_Proveedor.Enabled = Habilitado;
            Txt_Calle_Proveedor.Enabled = Habilitado;
            Txt_Numero_Proveedor.Enabled = Habilitado;
            Txt_Colonia_Proveedor.Enabled = Habilitado;
            Txt_Codigo_Postal_Proveedor.Enabled = Habilitado;
            Txt_Ciudad_Proveedor.Enabled = Habilitado;
            Txt_Estado_Proveedor.Enabled = Habilitado;
            Txt_Telefono_Proveedor.Enabled = Habilitado;
            Txt_Fax_Proveedor.Enabled = Habilitado;
            Txt_Email_Proveedor.Enabled = Habilitado;
            Txt_Contacto_Proveedor.Enabled = Habilitado;
            Txt_Comentarios_Proveedor.Enabled = Habilitado;
            Grid_Proveedores.Enabled = !Habilitado;

            Grid_Proveedores.Enabled = !Habilitado;
            Grid_Deducciones.Enabled = Habilitado;
            Cmb_Deducciones.Enabled = Habilitado;
            Btn_Agregar_Deducciones.Enabled = Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Proveedor
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Proveedor()
    {
        Boolean Datos_Validos = true;
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (Cmb_Aval.SelectedIndex <= 0) {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Es necesario indicar si el proveedor requiere aval. <br>";
            Datos_Validos = false;
        }

        if (Txt_Nombre_Proveedor.Text.Equals(""))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Nombre <br>";
            Datos_Validos = false;
        }
        if (Txt_RFC_Proveedor.Text.Equals(""))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + RFC <br>";
            Datos_Validos = false;
        }
        else if (!Validar_RFC())
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Formato de  RFC Incorrecto <br>";
            Datos_Validos = false;
        }

        if (Cmb_Estatus_Proveedor.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Estatus <br>";
            Datos_Validos = false;
        }
        if (Txt_Calle_Proveedor.Text.Equals(""))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Calle <br>";
            Datos_Validos = false;
        }
        if (Txt_Numero_Proveedor.Text.Equals(""))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Numero <br>";
            Datos_Validos = false;
        }
        if (Txt_Colonia_Proveedor.Text.Equals(""))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Colonia <br>";
            Datos_Validos = false;
        }
        if (Txt_Codigo_Postal_Proveedor.Text.Equals(""))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Codigo Postal <br>";
            Datos_Validos = false;
        }
        else if (!Validar_Codigo_Postal())
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Formato del Codigo Postal Incorrecto <br>";
            Datos_Validos = false;
        }

        if (Txt_Ciudad_Proveedor.Text.Equals(""))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Ciudad <br>";
            Datos_Validos = false;
        }
        if (Txt_Estado_Proveedor.Text.Equals(""))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Estado <br>";
            Datos_Validos = false;
        }
        if (!Txt_Telefono_Proveedor.Text.Equals(""))
        {
            //    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Telefono <br>";
            //    Datos_Validos = false;
            //}
            if (!Validar_Telefono())
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Formato del Telefono Incorrecto <br>";
                Datos_Validos = false;
            }
        }

        if (!Txt_Fax_Proveedor.Text.Equals(""))
        {
            if (!Validar_Fax())
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Formato del Fax Incorrecto <br>";
                Datos_Validos = false;
            }
        }
        if (!Txt_Email_Proveedor.Text.Equals(""))
        {
            //    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + E-mail <br>";
            //    Datos_Validos = false;
            //}
            if (!Validar_Email())
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Formato de  E-mail Incorrecto <br>";
                Datos_Validos = false;
            }
        }
        if (Txt_Contacto_Proveedor.Text.Equals(""))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Contacto <br>";
            Datos_Validos = false;
        }
        return Datos_Validos;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Email
    /// DESCRIPCION : Valida el E-mail Ingresado
    /// CREO        : Susana Trigueros Armenta
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public Boolean Validar_Email()
    {
        string MatchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@" +
                                   @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\." +
                                   @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|" +
                                   @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

        if (Txt_Email_Proveedor.Text != null) return Regex.IsMatch(Txt_Email_Proveedor.Text, MatchEmailPattern);
        else return false;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_RFC
    /// DESCRIPCION : Valida el RFC Ingresado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public Boolean Validar_RFC()
    {
        string MatchEmailPattern = @"^[a-zA-Z]{3,4}(\d{6})((\D|\d){3})?$";

        if (Txt_RFC_Proveedor.Text != null) return Regex.IsMatch(Txt_RFC_Proveedor.Text, MatchEmailPattern);
        else return false;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Codigo_Postal
    /// DESCRIPCION : Valida el Codigo Postal Ingresado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public Boolean Validar_Codigo_Postal()
    {
        string MatchEmailPattern = @"^([1-9]{2}|[0-9][1-9]|[1-9][0-9])[0-9]{3}$";

        if (Txt_Codigo_Postal_Proveedor.Text != null) return Regex.IsMatch(Txt_Codigo_Postal_Proveedor.Text, MatchEmailPattern);
        else return false;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Telefono
    /// DESCRIPCION : Valida el Telefono Ingresado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public Boolean Validar_Telefono()
    {
        string MatchEmailPattern = @"^[0-9]{2,3}-? ?[0-9]{7,10}$";

        if (Txt_Telefono_Proveedor.Text != null) return Regex.IsMatch(Txt_Telefono_Proveedor.Text, MatchEmailPattern);
        else return false;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Fax
    /// DESCRIPCION : Valida el Fax Ingresado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public Boolean Validar_Fax()
    {
        string MatchEmailPattern = @"^[0-9]{2,3}-? ?[0-9]{7,10}$";

        if (Txt_Fax_Proveedor.Text != null) return Regex.IsMatch(Txt_Fax_Proveedor.Text, MatchEmailPattern);
        else return false;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Juntar_Clave_Nombre
    /// DESCRIPCION : Junta el nombre del concepto con la clave.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected DataTable Juntar_Clave_Nombre(DataTable Dt_Conceptos)
    {
        try
        {
            if (Dt_Conceptos is DataTable)
            {
                if (Dt_Conceptos.Rows.Count > 0)
                {
                    foreach (DataRow CONCEPTO in Dt_Conceptos.Rows)
                    {
                        if (CONCEPTO is DataRow)
                        {
                            CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Nombre] =
                                "[" + CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Clave].ToString().Trim() + "] -- " +
                                    CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString().Trim();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al unir el nombre con la clave del concepto. Error: [" + Ex.Message + "]");
        }
        return Dt_Conceptos;
    }
    #endregion

    #region (Metodos Consulta)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Proveedor
    ///DESCRIPCIÓN: Consulta la lista de proveedores y establece los valores em sus 
    ///respectivos campos.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************    
    private void Cargar_Datos_Proveedor(int index)
    {
        DataRow[] Fila_Selelccionada = null;
        try
        {
            Fila_Selelccionada = ((DataTable)Session["Proveedores"]).Select(Cat_Nom_Proveedores.Campo_Proveedor_ID + " ='" + Grid_Proveedores.Rows[index].Cells[1].Text + "'");
            Txt_Proveedor_ID.Text = Fila_Selelccionada[0][Cat_Nom_Proveedores.Campo_Proveedor_ID].ToString();
            Txt_Nombre_Proveedor.Text = Fila_Selelccionada[0][Cat_Nom_Proveedores.Campo_Nombre].ToString();
            Txt_RFC_Proveedor.Text = Fila_Selelccionada[0][Cat_Nom_Proveedores.Campo_RFC].ToString();
            Cmb_Estatus_Proveedor.SelectedIndex = Cmb_Estatus_Proveedor.Items.IndexOf(Cmb_Estatus_Proveedor.Items.FindByText(Fila_Selelccionada[0][Cat_Nom_Proveedores.Campo_Estatus].ToString()));
            Txt_Calle_Proveedor.Text = Fila_Selelccionada[0][Cat_Nom_Proveedores.Campo_Calle].ToString();
            Txt_Numero_Proveedor.Text = Fila_Selelccionada[0][Cat_Nom_Proveedores.Campo_Numero].ToString();
            Txt_Colonia_Proveedor.Text = Fila_Selelccionada[0][Cat_Nom_Proveedores.Campo_Colonia].ToString();
            Txt_Codigo_Postal_Proveedor.Text = Fila_Selelccionada[0][Cat_Nom_Proveedores.Campo_Codigo_Postal].ToString();
            Txt_Ciudad_Proveedor.Text = Fila_Selelccionada[0][Cat_Nom_Proveedores.Campo_Ciudad].ToString();
            Txt_Estado_Proveedor.Text = Fila_Selelccionada[0][Cat_Nom_Proveedores.Campo_Estado].ToString();
            Txt_Telefono_Proveedor.Text = Fila_Selelccionada[0][Cat_Nom_Proveedores.Campo_Telefono].ToString();
            Txt_Fax_Proveedor.Text = Fila_Selelccionada[0][Cat_Nom_Proveedores.Campo_Fax].ToString();
            Txt_Email_Proveedor.Text = Fila_Selelccionada[0][Cat_Nom_Proveedores.Campo_Email].ToString();
            Txt_Contacto_Proveedor.Text = Fila_Selelccionada[0][Cat_Nom_Proveedores.Campo_Contacto].ToString();
            Txt_Comentarios_Proveedor.Text = Fila_Selelccionada[0][Cat_Nom_Proveedores.Campo_Comentarios].ToString();
            Cmb_Aval.SelectedIndex = Cmb_Aval.Items.IndexOf(Cmb_Aval.Items.FindByText(Fila_Selelccionada[0][Cat_Nom_Proveedores.Campo_Aval].ToString()));
        }
        catch (Exception Ex)
        {
            throw new Exception("Error seleccionar un elemento de la tabla. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consulta_Proveedores_Nombre
    ///DESCRIPCIÓN: Busca el proveedor con el nombre proporcionado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************   
    private void Consulta_Proveedores_Nombre(String Nombre, GridView _GridView)
    {
        DataTable Dt_Proveedores = (DataTable)Session["Proveedores"];
        DataView dv = new DataView(Dt_Proveedores);
        String Expresion_De_Busqueda = null;

        Expresion_De_Busqueda = string.Format("{0} '%{1}%'", _GridView.SortExpression, Nombre);

        dv.RowFilter = Cat_Nom_Proveedores.Campo_Nombre + " like " + Expresion_De_Busqueda;
        _GridView.DataSource = dv;
        _GridView.DataBind();
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Deducciones
    /// DESCRIPCION : Carga las Deducciones Variables que puede tener el proveedor
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Deducciones()
    {
        DataTable Dt_Deducciones = null; //Variable a obtener los datos de la consulta
        Cls_Cat_Nom_Proveedores_Negocio Rs_Consulta_Cat_Nom_Percepcion_Deduccion = new Cls_Cat_Nom_Proveedores_Negocio(); //Variable para la realización de la conexión hacia la capa de negocios
        try
        {
            Dt_Deducciones = Rs_Consulta_Cat_Nom_Percepcion_Deduccion.Consulta_Percepciones_Deducciones();
            Session["Dt_Deducciones_Combo"] = Dt_Deducciones;
            Cmb_Deducciones.DataSource = Dt_Deducciones;
            Cmb_Deducciones.DataTextField = "NOMBRE_CONCEPTO";
            Cmb_Deducciones.DataValueField = Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID;
            Cmb_Deducciones.DataBind();
            Cmb_Deducciones.Items.Insert(0, new ListItem("< Seleccione >", ""));
            Cmb_Deducciones.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado al consultar las Deducciones. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Deducciones_Proveedores
    /// DESCRIPCION : Consultar las Deducciones del Sindicato Seleccionado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 07/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Deducciones_Proveedores()
    {
        Cls_Cat_Nom_Proveedores_Negocio Rs_Consulta_Cat_Nom_Proveedores_Detalles = new Cls_Cat_Nom_Proveedores_Negocio();
        int index = Grid_Proveedores.SelectedIndex;
        DataTable Dt_Deducciones;

        if (index != -1)
        {
            Rs_Consulta_Cat_Nom_Proveedores_Detalles.P_Proveedor_ID = Grid_Proveedores.Rows[index].Cells[1].Text;
            Dt_Deducciones = Rs_Consulta_Cat_Nom_Proveedores_Detalles.Consultar_Deducciones_Proveedor();
            Dt_Deducciones = Juntar_Clave_Nombre(Dt_Deducciones);
            Session["Dt_Deducciones_Grid"] = Dt_Deducciones;

            Grid_Deducciones.Columns[0].Visible = true;
            Grid_Deducciones.DataSource = (DataTable)Session["Dt_Deducciones_Grid"];
            Grid_Deducciones.DataBind();
            Grid_Deducciones.Columns[0].Visible = false;
        }
    }
    #endregion

    #region (Metodos Operacion)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Proveedor
    /// DESCRIPCION : Ejecuta el alta de un Proveedor
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Proveedor() {
        Cls_Cat_Nom_Proveedores_Negocio _Cat_Proveedores = new Cls_Cat_Nom_Proveedores_Negocio();
        try
        {
            _Cat_Proveedores.P_Aval = Cmb_Aval.SelectedItem.Text.Trim();
            _Cat_Proveedores.P_Nombre = Txt_Nombre_Proveedor.Text;
            _Cat_Proveedores.P_RFC = Txt_RFC_Proveedor.Text;
            _Cat_Proveedores.P_Estatus = Cmb_Estatus_Proveedor.SelectedItem.Text;
            _Cat_Proveedores.P_Calle = Txt_Calle_Proveedor.Text;
            _Cat_Proveedores.P_Numero = (Txt_Numero_Proveedor.Text.Equals("") ? 0 : Convert.ToInt32(Txt_Numero_Proveedor.Text));
            _Cat_Proveedores.P_Colonia = Txt_Colonia_Proveedor.Text;
            _Cat_Proveedores.P_Codigo_Postal = (Txt_Codigo_Postal_Proveedor.Text.Equals("") ? 0 : Convert.ToInt32(Txt_Codigo_Postal_Proveedor.Text));
            _Cat_Proveedores.P_Ciudad = Txt_Ciudad_Proveedor.Text;
            _Cat_Proveedores.P_Estado = Txt_Estado_Proveedor.Text;
            _Cat_Proveedores.P_Telefono = Txt_Telefono_Proveedor.Text;
            _Cat_Proveedores.P_Fax = Txt_Fax_Proveedor.Text;
            _Cat_Proveedores.P_Email = Txt_Email_Proveedor.Text;
            _Cat_Proveedores.P_Contacto = Txt_Contacto_Proveedor.Text;
            _Cat_Proveedores.P_Comentarios = Txt_Comentarios_Proveedor.Text;
            _Cat_Proveedores.P_Usuario_Creo = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);

            if(Session["Dt_Deducciones_Grid"] != null){
                _Cat_Proveedores.P_Dt_Deducciones = (DataTable)Session["Dt_Deducciones_Grid"];
                Session.Remove("Dt_Deducciones_Grid");
            }             

            if (_Cat_Proveedores.Alta_Proveedores())
            {
                Configuracion_Inicial();
                Limpiar_Controles();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "","alert('Operación Exitosa [Alta Proveedor]');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al dar de Alta a un Proveedor. Error: [" + Ex.Message + "]");
        }    
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Proveedor
    /// DESCRIPCION : Ejecuta la Actualizacion de un Proveedor
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Proveedor()
    {
        Cls_Cat_Nom_Proveedores_Negocio _Cat_Proveedores = new Cls_Cat_Nom_Proveedores_Negocio();
        try{
            _Cat_Proveedores.P_Proveedor_ID = Txt_Proveedor_ID.Text;
            _Cat_Proveedores.P_Aval = Cmb_Aval.SelectedItem.Text.Trim();
            _Cat_Proveedores.P_Nombre = Txt_Nombre_Proveedor.Text;
            _Cat_Proveedores.P_RFC = Txt_RFC_Proveedor.Text;
            _Cat_Proveedores.P_Estatus = Cmb_Estatus_Proveedor.SelectedItem.Text;
            _Cat_Proveedores.P_Calle = Txt_Calle_Proveedor.Text;
            _Cat_Proveedores.P_Numero =(Txt_Numero_Proveedor.Text.Equals("")? 0:  Convert.ToInt32(Txt_Numero_Proveedor.Text));
            _Cat_Proveedores.P_Colonia = Txt_Colonia_Proveedor.Text;
            _Cat_Proveedores.P_Codigo_Postal = (Txt_Codigo_Postal_Proveedor.Text.Equals("") ? 0 : Convert.ToInt32(Txt_Codigo_Postal_Proveedor.Text));
            _Cat_Proveedores.P_Ciudad = Txt_Ciudad_Proveedor.Text;
            _Cat_Proveedores.P_Estado = Txt_Estado_Proveedor.Text;
            _Cat_Proveedores.P_Telefono = Txt_Telefono_Proveedor.Text;
            _Cat_Proveedores.P_Fax = Txt_Fax_Proveedor.Text;
            _Cat_Proveedores.P_Email = Txt_Email_Proveedor.Text;
            _Cat_Proveedores.P_Contacto = Txt_Contacto_Proveedor.Text;
            _Cat_Proveedores.P_Comentarios = Txt_Comentarios_Proveedor.Text;
            _Cat_Proveedores.P_Usuario_Modifico = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);

            if (Session["Dt_Deducciones_Grid"] != null)
            {
                _Cat_Proveedores.P_Dt_Deducciones = (DataTable)Session["Dt_Deducciones_Grid"];
                Session.Remove("Dt_Deducciones_Grid");
            }

            if (_Cat_Proveedores.Modificar_Proveedores())
            {
                Configuracion_Inicial();
                Limpiar_Controles();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa [Actualizar Proveedor]');", true);
            }            
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al Actualizar a un Proveedor. Error: [" + Ex.Message + "]");
        } 
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Proveedor
    /// DESCRIPCION : Ejecuta la Baja de un Proveedor
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Proveedor()
    {
        Cls_Cat_Nom_Proveedores_Negocio _Cat_Proveedores = new Cls_Cat_Nom_Proveedores_Negocio();
        try
        {
            _Cat_Proveedores.P_Proveedor_ID = Txt_Proveedor_ID.Text;

            if (_Cat_Proveedores.Eliminar_Proveedores())
            {
                Configuracion_Inicial();
                Limpiar_Controles();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa [Eliminación del Proveedor]');", true);
            }   
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al dar de Baja a un Proveedor. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Metodos Deducciones)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Agregar_Deduccion
    /// DESCRIPCION : Agrega una nueva deduccion a la tabla de deducciones.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 11/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Agregar_Deduccion(DataTable _DataTable, GridView _GridView, DropDownList _DropDownList)
    {
        DataRow[] Filas;
        DataTable Dt = (DataTable)Session["Dt_Deducciones_Grid"];
        Cls_Cat_Nom_Percepciones_Deducciones_Business Cat_Percepciones_Deducciones = new Cls_Cat_Nom_Percepciones_Deducciones_Business();

        try
        {
            int index = _DropDownList.SelectedIndex;
            if (index > 0)
            {
                Filas = _DataTable.Select(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + "='" + _DropDownList.SelectedValue.Trim() + "'");
                if (Filas.Length > 0)
                {
                    //Si se encontro algun coincidencia entre el grupo a agregar con alguno agregado anteriormente, se avisa
                    //al usuario que elemento ha agregar ya existe en la tabla de grupos.
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                        "alert('No se puede agregar la Deduccion, ya que esta ya se ha agregado');", true);
                    Cmb_Deducciones.SelectedIndex = 0;
                }
                else
                {
                    DataTable Dt_Temporal = Cat_Percepciones_Deducciones.Busqueda_Percepcion_Deduccion_Por_ID(_DropDownList.SelectedValue.Trim());
                    if (!(Dt_Temporal == null))
                    {
                        if (Dt_Temporal.Rows.Count > 0)
                        {
                            DataRow row = Dt.NewRow();
                            row[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID] = Dt_Temporal.Rows[0][0].ToString();
                            row[Cat_Nom_Percepcion_Deduccion.Campo_Nombre] = "[" + Dt_Temporal.Rows[0][15].ToString() + "] - " + Dt_Temporal.Rows[0][1].ToString();
                            Dt.Rows.Add(row);
                            Dt.AcceptChanges();
                            Session["Dt_Deducciones_Grid"] = Dt;
                            Grid_Deducciones.Columns[0].Visible = true;
                            _GridView.DataSource = (DataTable)Session["Dt_Deducciones_Grid"];
                            _GridView.DataBind();
                            Grid_Deducciones.Columns[0].Visible = false;
                            Cmb_Deducciones.SelectedIndex = 0;
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                    "alert('No se a seleccionado ninguna deduccion a agregar');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al agregar la deduccion al Grid de Deducciones" + Ex.Message);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Crear_DataTable_Percepciones_Deducciones
    /// DESCRIPCION : Crea un datatable con la informacion de del id de la percepcion.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 11/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Crear_DataTable_Percepciones_Deducciones(GridView _GridView, String TextBox_ID)
    {
        DataTable Dt_Percepciones_Deducciones = new DataTable();
        Dt_Percepciones_Deducciones.Columns.Add(Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID, typeof(System.String));
        DataRow Renglon;

        try
        {
            for (int index = 0; index < _GridView.Rows.Count; index++)
            {
                Renglon = Dt_Percepciones_Deducciones.NewRow();
                Renglon[Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID] = _GridView.Rows[index].Cells[0].Text;
                Dt_Percepciones_Deducciones.Rows.Add(Renglon);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return Dt_Percepciones_Deducciones;
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
            Botones.Add(Btn_Buscar_Proveedor);

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

    #region (Eventos)

    #region (Operacion Alta - Modificar - Eliminar - Consultar)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Alta de un Proveedor
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Octubre/2010 
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
                Limpiar_Controles();
                Habilitar_Controles("Nuevo");
            }
            else
            {
                if (Validar_Datos_Proveedor())
                {
                    Alta_Proveedor();
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
    ///DESCRIPCIÓN: Modificar un Proveedor
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Octubre/2010 
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
                if (Grid_Proveedores.SelectedIndex != -1 & !Txt_Proveedor_ID.Text.Equals(""))
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
                if (Validar_Datos_Proveedor())
                {
                    Modificar_Proveedor();
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
    ///DESCRIPCIÓN: Eliminar un Proveedor
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Octubre/2010 
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
                if (Grid_Proveedores.SelectedIndex != -1 & !Txt_Proveedor_ID.Text.Equals(""))
                {
                    Eliminar_Proveedor();

                    if (Session["Dt_Deducciones_Grid"] != null)
                    {
                        Session.Remove("Dt_Deducciones_Grid");
                    }
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
    ///FECHA_CREO: 23/Octubre/2010 
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
                
                if (Session["Dt_Deducciones_Grid"] != null)
                {
                    Session.Remove("Dt_Deducciones_Grid");
                }

                if (Session["Proveedores"] != null)
                {
                    Session.Remove("Proveedores");
                }

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
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Proveedor_Click
    ///DESCRIPCIÓN: Busqueda de Proveedores
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Proveedor_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Consulta_Proveedores_Nombre(Txt_Busqueda_Proveedor.Text, Grid_Proveedores);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error ejecutar la busqueda de un proveedor. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Eventos [Agregar - Eliminar Deducciones al Grid Deducciones])
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Agregar_Deduccion
    /// DESCRIPCION : Agrega una nueva deduccion a la tabla de deducciones.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Agregar_Deduccion(object sender, EventArgs e)
    {
        if (Cmb_Deducciones.SelectedIndex > 0)
        {
            if (Session["Dt_Deducciones_Grid"] != null)
            {
                Agregar_Deduccion((DataTable)Session["Dt_Deducciones_Grid"], Grid_Deducciones, Cmb_Deducciones);

            }
            else
            {
                DataTable Dt_Deducciones = new DataTable();
                Dt_Deducciones.Columns.Add(Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID, typeof(System.String));
                Dt_Deducciones.Columns.Add(Cat_Nom_Percepcion_Deduccion.Campo_Nombre, typeof(System.String));
                Dt_Deducciones.Columns.Add(Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Cantidad, typeof(System.String));

                Session["Dt_Deducciones_Grid"] = Dt_Deducciones;
                Grid_Deducciones.Columns[0].Visible = true;
                Grid_Deducciones.DataSource = (DataTable)Session["Dt_Deducciones_Grid"];
                Grid_Deducciones.DataBind();
                Grid_Deducciones.Columns[0].Visible = false;

                Agregar_Deduccion(Dt_Deducciones, Grid_Deducciones, Cmb_Deducciones);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                "alert('No se a seleccionado ninguna percepcion a agregar');", true);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Delete_Deduccion
    /// DESCRIPCION : Elimina la fila seleccionada del Grid de Deducciones.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 11/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Delete_Deduccion(object sender, EventArgs e)
    {
        ImageButton Btn_Eliminar_Deduccion = (ImageButton)sender;
        DataTable Dt_Deducciones = (DataTable)Session["Dt_Deducciones_Grid"];
        DataRow[] Filas = Dt_Deducciones.Select(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID +
                "='" + Btn_Eliminar_Deduccion.CommandArgument + "'");

        if (!(Filas == null))
        {
            if (Filas.Length > 0)
            {
                Dt_Deducciones.Rows.Remove(Filas[0]);
                Session["Dt_Deducciones_Grid"] = Dt_Deducciones;
                Grid_Deducciones.Columns[0].Visible = true;
                Grid_Deducciones.DataSource = (DataTable)Session["Dt_Deducciones_Grid"];
                Grid_Deducciones.DataBind();
                Grid_Deducciones.Columns[0].Visible = false;
                Cmb_Deducciones.SelectedIndex = 0;
            }
        }
    }
    #endregion

    #endregion
}
