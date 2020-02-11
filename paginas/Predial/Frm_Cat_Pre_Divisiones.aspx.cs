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
using Presidencia.Catalogo_Divisiones.Negocio;
using Presidencia.Sessiones;

public partial class paginas_predial_Frm_Cat_Pre_Divisiones : System.Web.UI.Page
{

    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 26/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Configuracion_Formulario(true);
            Llenar_Tabla_Divisiones(0);
        }
        Div_Contenedor_Msj_Error.Visible = false;
    }

    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///             1. estatus.    Estatus en el que se cargara la configuración de los
    ///                             controles.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 26/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean estatus)
    {
        Btn_Nuevo.Visible = true;
        Btn_Nuevo.AlternateText = "Nuevo";
        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        Btn_Modificar.Visible = true;
        Btn_Modificar.AlternateText = "Modificar";
        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        Btn_Eliminar.Visible = estatus;
        Txt_Identificador.Enabled = !estatus;
        Cmb_Estatus.Enabled = !estatus;
        Txt_Descripcion.Enabled = !estatus;
        Grid_Divisiones_Generales.Enabled = estatus;
        Grid_Divisiones_Generales.SelectedIndex = (-1);
        Txt_Anio.Enabled = !estatus;
        Txt_Tasa.Enabled = !estatus;
        Btn_Agregar_Impuesto.Visible = !estatus;
        Btn_Quitar_Impuesto.Visible = !estatus;
        Btn_Modificar_Impuesto.Visible = !estatus;
        Grid_Divisiones_Impuesto.SelectedIndex = (-1);
        Grid_Divisiones_Impuesto.Columns[1].Visible = false;
        Btn_Buscar_Division.Enabled = estatus;
        Txt_Busqueda_Division.Enabled = estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles del Formulario
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 26/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Txt_Identificador.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
        Txt_Descripcion.Text = "";
        Txt_Anio.Text = "";
        Txt_Tasa.Text = "";
        Txt_ID_Division.Text = "";
        Grid_Divisiones_Impuesto.DataSource = new DataTable();
        Grid_Divisiones_Impuesto.DataBind();
        Hdf_Division_ID.Value = "";
        Hdf_Division_Impuesto_ID.Value = "";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Divisiones
    ///DESCRIPCIÓN: Llena la tabla de Divisiones con una consulta que puede o no
    ///             tener Filtros.
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 26/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Divisiones(int Pagina)
    {
        try
        {
            Cls_Cat_Pre_Divisiones_Negocio divisiones = new Cls_Cat_Pre_Divisiones_Negocio();
            divisiones.P_Identificador = Txt_Busqueda_Division.Text.Trim().ToUpper();
            Grid_Divisiones_Generales.DataSource = divisiones.Consultar_Divisiones();
            Grid_Divisiones_Generales.PageIndex = Pagina;
            Grid_Divisiones_Generales.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Divisiones_Impuestos
    ///DESCRIPCIÓN: Llena la tabla de Divisiones Impuestos.
    ///PROPIEDADES:     
    ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
    ///             2.  tabla.  Tabla que se va a cargar en el Grid.    
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 26/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Divisiones_Impuestos(int Pagina, DataTable Tabla)
    {
        Grid_Divisiones_Impuesto.Columns[1].Visible = true;
        Tabla.DefaultView.Sort = "ANIO DESC, TASA DESC";
        Grid_Divisiones_Impuesto.DataSource = Tabla;
        Grid_Divisiones_Impuesto.PageIndex = Pagina;
        Grid_Divisiones_Impuesto.DataBind();
        Grid_Divisiones_Impuesto.Columns[1].Visible = false;
        Session["Dt_Divisiones_Impuestos"] = Tabla;
    }

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación en la pestaña de Divisiones.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 02/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes_Generales()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Txt_Identificador.Text.Trim().Length == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Introducir el Identificador (Pestaña 1 de 2).";
            Validacion = false;
        }
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus (Pestaña 1 de 2).";
            Validacion = false;
        }
        if (Txt_Descripcion.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir la Descripci&oacute;n (Pestaña 1 de 2).";
            Validacion = false;
        }
        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Impuestos
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación en la pestaña de Divisiones - Impuestos.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 02/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes_Impuestos()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Txt_Anio.Text.Trim().Length == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Introducir el Año (Pestaña 2 de 2).";
            Validacion = false;
        }
        if (Txt_Tasa.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir la Tasa (Pestaña 2 de 2).";
            Validacion = false;
        }
        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }

    #endregion

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Divisiones_Generales_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView General de las Divisiones 
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 26/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Divisiones_Generales_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Divisiones_Generales.SelectedIndex = (-1);
            Llenar_Tabla_Divisiones(e.NewPageIndex);
            Limpiar_Catalogo();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Divisiones_Generales_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de la Division Seleccionada para mostrarlos a detalle
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 26/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Divisiones_Generales_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Divisiones_Generales.SelectedIndex > (-1))
            {
                Limpiar_Catalogo();
                Session["Dt_Divisiones_Impuestos"] = null;
                String id_Seleccionado = Grid_Divisiones_Generales.SelectedRow.Cells[1].Text;
                Cls_Cat_Pre_Divisiones_Negocio division = new Cls_Cat_Pre_Divisiones_Negocio();
                division.P_Division_ID = id_Seleccionado;
                division = division.Consultar_Datos_Division();
                Hdf_Division_ID.Value = division.P_Division_ID;
                Txt_ID_Division.Text = division.P_Division_ID;
                Txt_Identificador.Text = division.P_Identificador;
                Txt_Descripcion.Text = division.P_Descripcion;
                Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(division.P_Estatus));
                Llenar_Tabla_Divisiones_Impuestos(0, division.P_Divisiones_Impuestos);
                System.Threading.Thread.Sleep(1000);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Divisiones_Impuesto_PageIndexChanging
    ///DESCRIPCIÓN: Maneja el Cambio de Pagina de la Tabla de Divisiones Impuesto
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 26/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Divisiones_Impuesto_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (Session["Dt_Divisiones_Impuestos"] != null)
        {
            DataTable tabla = (DataTable)Session["Dt_Divisiones_Impuestos"];
            Llenar_Tabla_Divisiones_Impuestos(e.NewPageIndex, tabla);
            Grid_Divisiones_Impuesto.SelectedIndex = (-1);
        }
    }

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una nueva Division
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 26/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                Configuracion_Formulario(false);
                Limpiar_Catalogo();
                Session["Dt_Divisiones_Impuestos"] = null;
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.Visible = false;
            }
            else
            {
                if (Validar_Componentes_Generales())
                {
                    Cls_Cat_Pre_Divisiones_Negocio division = new Cls_Cat_Pre_Divisiones_Negocio();
                    division.P_Identificador = Txt_Identificador.Text.ToUpper();
                    division.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                    division.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                    division.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                    division.P_Divisiones_Impuestos = (DataTable)Session["Dt_Divisiones_Impuestos"];
                    division.Alta_Division();
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Session["Dt_Divisiones_Impuestos"] = null;
                    Llenar_Tabla_Divisiones(Grid_Divisiones_Generales.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Divisiones", "alert('Alta de División Exitosa');", true);
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Divisiones_Impuesto.Enabled = true;
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de una Division
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 26/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Modificar.AlternateText.Equals("Modificar"))
            {
                if (Grid_Divisiones_Generales.Rows.Count > 0 && Grid_Divisiones_Generales.SelectedIndex > (-1))
                {
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "Selecciona el Registro que quieres Modificar.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                if (Validar_Componentes_Generales())
                {
                    Cls_Cat_Pre_Divisiones_Negocio division = new Cls_Cat_Pre_Divisiones_Negocio();
                    division.P_Division_ID = Hdf_Division_ID.Value;
                    division.P_Identificador = Txt_Identificador.Text.ToUpper();
                    division.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                    division.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                    division.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                    division.P_Divisiones_Impuestos = (DataTable)Session["Dt_Divisiones_Impuestos"];
                    division.Modificar_Division();
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Tabla_Divisiones(Grid_Divisiones_Generales.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Divisiones", "alert('Actualización de División Exitosa');", true);
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Divisiones_Impuesto.Enabled = true;
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Division_Click
    ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 26/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Division_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Limpiar_Catalogo();
            Session["Dt_Divisiones_Impuestos"] = null;
            Grid_Divisiones_Generales.SelectedIndex = (-1);
            Grid_Divisiones_Impuesto.SelectedIndex = (-1);
            Llenar_Tabla_Divisiones(0);
            if (Grid_Divisiones_Generales.Rows.Count == 0 && Txt_Busqueda_Division.Text.Trim().Length > 0)
            {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Identificador \"" + Txt_Busqueda_Division.Text + "\" no se encotrarón coincidencias";
                Lbl_Mensaje_Error.Text = "(Se cargaron todas las divisiones almacenadas)";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Busqueda_Division.Text = "";
                Llenar_Tabla_Divisiones(0);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: Elimina una división de la Base de Datos
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 26/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Divisiones_Generales.Rows.Count > 0 && Grid_Divisiones_Generales.SelectedIndex > (-1))
            {
                Cls_Cat_Pre_Divisiones_Negocio division = new Cls_Cat_Pre_Divisiones_Negocio();
                division.P_Division_ID = Grid_Divisiones_Generales.SelectedRow.Cells[1].Text;
                division.P_Estatus = "BAJA";
                division.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                division.Eliminar_Division();
                Grid_Divisiones_Generales.SelectedIndex = (-1);
                Llenar_Tabla_Divisiones(Grid_Divisiones_Generales.PageIndex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Divisiones", "alert('La División fue actualizada exitosamente');", true);
                Tab_Contenedor_Pestagnas.TabIndex = 0;
                Limpiar_Catalogo();
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Eliminar.";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 26/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Session["Dt_Divisiones_Impuestos"] = null;
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Configuracion_Formulario(true);
            Limpiar_Catalogo();
            Tab_Contenedor_Pestagnas.TabIndex = 0;
            Session["Dt_Divisiones_Impuestos"] = null;
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Grid_Divisiones_Impuesto.Enabled = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Impuesto_Click
    ///DESCRIPCIÓN: Agrega un nuevo impuesto a la tabla de Divisiones Impuestos(Solo en Interfaz no en la Base de Datos)
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 26/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Impuesto_Click(object sender, EventArgs e)
    {
        try
        {
            if (Validar_Componentes_Impuestos())
            {
                DataTable tabla = (DataTable)Grid_Divisiones_Impuesto.DataSource;
                if (tabla == null)
                {
                    if (Session["Dt_Divisiones_Impuestos"] == null)
                    {
                        tabla = new DataTable("div_imp");
                        tabla.Columns.Add("IMPUESTO_DIVISION_LOT_ID", Type.GetType("System.String"));
                        tabla.Columns.Add("ANIO", Type.GetType("System.String"));
                        tabla.Columns.Add("TASA", Type.GetType("System.String"));
                    }
                    else
                    {
                        tabla = (DataTable)Session["Dt_Divisiones_Impuestos"];
                    }
                }
                // validar que el año no esté ya en la tabla
                foreach (DataRow Dr_Registro in tabla.Rows)
                {
                    if (Dr_Registro["ANIO"].ToString() == Txt_Anio.Text.Trim() && Convert.ToDouble(Dr_Registro["TASA"].ToString()) == Convert.ToDouble(Txt_Tasa.Text.Trim()))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Divisiones", "alert('Año y tasa ya existentes. Registro no agregado.');", true);
                        return;
                    }
                    else if (Dr_Registro["ANIO"].ToString() == Txt_Anio.Text.Trim())
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Divisiones", "alert('Año ya existente. Registro no agregado.');", true);
                        return;
                    }
                }
                // agregar registro a la tabla
                DataRow fila = tabla.NewRow();
                fila["IMPUESTO_DIVISION_LOT_ID"] = HttpUtility.HtmlDecode("");
                fila["ANIO"] = HttpUtility.HtmlDecode(Txt_Anio.Text.Trim());
                fila["TASA"] = HttpUtility.HtmlDecode(Txt_Tasa.Text.Trim());
                tabla.Rows.Add(fila);
                // cargar en grid
                tabla.DefaultView.Sort = "ANIO DESC, TASA DESC";
                Grid_Divisiones_Impuesto.DataSource = tabla;
                Session["Dt_Divisiones_Impuestos"] = tabla;
                Grid_Divisiones_Impuesto.DataBind();
                Grid_Divisiones_Impuesto.SelectedIndex = (-1);
                Txt_Anio.Text = "";
                Txt_Tasa.Text = "";
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Impuesto_Click
    ///DESCRIPCIÓN: Quita un impuesto a la tabla de Divisiones Impuestos(Solo en Interfaz no en la Base de Datos)
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 26/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Quitar_Impuesto_Click(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Divisiones_Impuesto.Rows.Count > 0 && Grid_Divisiones_Impuesto.SelectedIndex > (-1))
            {
                int registro = ((Grid_Divisiones_Impuesto.PageIndex) * Grid_Divisiones_Impuesto.PageSize) + (Grid_Divisiones_Impuesto.SelectedIndex);
                if (Session["Dt_Divisiones_Impuestos"] != null)
                {
                    DataTable tabla = (DataTable)Session["Dt_Divisiones_Impuestos"];
                    tabla.Rows.RemoveAt(registro);
                    tabla.DefaultView.Sort = "ANIO DESC, TASA DESC";
                    Session["Dt_Divisiones_Impuestos"] = tabla;
                    Grid_Divisiones_Impuesto.SelectedIndex = (-1);
                    Llenar_Tabla_Divisiones_Impuestos(Grid_Divisiones_Impuesto.PageIndex, tabla);
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Quitar.";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Impuesto_Click
    ///DESCRIPCIÓN: Modifica un impuesto a la tabla de Divisiones Impuestos(Solo en Interfaz no en la Base de Datos)
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 26/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Impuesto_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Modificar_Impuesto.AlternateText.Equals("Modificar"))
            {
                if (Grid_Divisiones_Impuesto.Rows.Count > 0 && Grid_Divisiones_Impuesto.SelectedIndex > (-1))
                {
                    Hdf_Division_Impuesto_ID.Value = Grid_Divisiones_Impuesto.SelectedRow.Cells[1].Text.Trim();
                    Txt_Impuesto_ID.Text = Grid_Divisiones_Impuesto.SelectedRow.Cells[1].Text.Trim();
                    Txt_Anio.Text = Grid_Divisiones_Impuesto.SelectedRow.Cells[2].Text.Trim();
                    Txt_Tasa.Text = Convert.ToDouble(Grid_Divisiones_Impuesto.SelectedRow.Cells[3].Text.Trim()).ToString("#,###,###.00");
                    Btn_Modificar_Impuesto.AlternateText = "Actualizar";
                    Btn_Quitar_Impuesto.Visible = false;
                    Btn_Agregar_Impuesto.Visible = false;
                    Grid_Divisiones_Impuesto.Enabled = false;
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "Selecciona el Registro que quieres Modificar.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                if (Validar_Componentes_Impuestos())
                {
                    int registro = ((Grid_Divisiones_Impuesto.PageIndex) * Grid_Divisiones_Impuesto.PageSize) + (Grid_Divisiones_Impuesto.SelectedIndex);
                    if (Session["Dt_Divisiones_Impuestos"] != null)
                    {
                        DataTable tabla = (DataTable)Session["Dt_Divisiones_Impuestos"];
                        int indice = 0;
                        foreach (DataRow Dr_Registro in tabla.Rows)
                        {
                            if (Dr_Registro["ANIO"].ToString() == Txt_Anio.Text.Trim() && Convert.ToDouble(Dr_Registro["TASA"].ToString()) == Convert.ToDouble(Txt_Tasa.Text.Trim()) && indice != registro)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Divisiones", "alert('Año y tasa ya existentes. Registro no Modificado.');", true);
                                return;
                            }
                            indice++;
                        }
                        tabla.DefaultView.AllowEdit = true;
                        tabla.Rows[registro].BeginEdit();
                        tabla.Rows[registro][1] = Txt_Anio.Text.Trim();
                        tabla.Rows[registro][2] = Txt_Tasa.Text.Trim();
                        tabla.Rows[registro].EndEdit();
                        tabla.DefaultView.Sort = "ANIO DESC, TASA DESC";
                        Session["Dt_Divisiones_Impuestos"] = tabla;
                        Llenar_Tabla_Divisiones_Impuestos(Grid_Divisiones_Impuesto.PageIndex, tabla);
                        Grid_Divisiones_Impuesto.SelectedIndex = (-1);
                        Btn_Modificar_Impuesto.AlternateText = "Modificar";
                        Btn_Quitar_Impuesto.Visible = true;
                        Btn_Agregar_Impuesto.Visible = true;
                        Tab_Contenedor_Pestagnas.TabIndex = 0;
                        Grid_Divisiones_Impuesto.Enabled = true;
                        Hdf_Division_Impuesto_ID.Value = "";
                        Txt_Impuesto_ID.Text = "";
                        Txt_Anio.Text = "";
                        Txt_Tasa.Text = "";
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #endregion

}