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
using Presidencia.Operacion_Recolecciones.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Recolecciones : System.Web.UI.Page
{

    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Configuracion_Acceso("Frm_Ope_Pre_Recolecciones.aspx");
                Configuracion_Formulario(true);
                Llenar_Combo_Empleados();
                Llenar_Combo_Numeros_Caja();
                Llenar_Tabla_Recolecciones(0);
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
        Div_Contenedor_Msj_Error.Visible = false;
    }

    #endregion

    #region Metodos

    ///****************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///             1. Estatus.    Estatus en el que se cargara la configuración de los controles.
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Estatus)
    {
        Btn_Nuevo.Visible = true;
        Btn_Nuevo.AlternateText = "Nuevo";
        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        Cmb_Numero_Caja.Enabled = !Estatus;
        Txt_Modulo.Enabled = !Estatus;
        Cmb_Cajero.Enabled = !Estatus;
        Txt_Numero_Recoleccion.Enabled = !Estatus;
        Txt_Fecha.Enabled = !Estatus;
        Txt_Monto_Recolectado.Enabled = !Estatus;
        Grid_Recolecciones.SelectedIndex = (-1);
        Btn_Buscar.Enabled = Estatus;
        Txt_Busqueda.Enabled = Estatus;
        Btn_Txt_Fecha.Enabled = !Estatus;
    }

    #region Llenar_Combos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Numeros_Caja
    ///DESCRIPCIÓN: Metodo que llena el Combo de Numeros de Caja con las Cajas existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Numeros_Caja()
    {
        try
        {
            Cls_Ope_Pre_Recolecciones_Negocio Numero_Caja = new Cls_Ope_Pre_Recolecciones_Negocio();
            DataTable Numeros_Caja = Numero_Caja.Llenar_Combo_Numeros_Caja();
            DataRow fila = Numeros_Caja.NewRow();
            fila[Cat_Pre_Cajas.Campo_Numero_De_Caja] = HttpUtility.HtmlDecode("0");
            fila[Cat_Pre_Cajas.Campo_Caja_Id] = "SELECCIONE";
            Numeros_Caja.Rows.InsertAt(fila, 0);
            Cmb_Numero_Caja.DataTextField = Cat_Pre_Cajas.Campo_Numero_De_Caja;
            Cmb_Numero_Caja.DataValueField = Cat_Pre_Cajas.Campo_Caja_Id;
            Cmb_Numero_Caja.DataSource = Numeros_Caja;
            Cmb_Numero_Caja.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Caja_Modulo
    ///DESCRIPCIÓN: Metodo que llena el Combo de Modulos con los modulos existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Caja_Modulo()
    {
        try
        {
            Cls_Ope_Pre_Recolecciones_Negocio Modulo = new Cls_Ope_Pre_Recolecciones_Negocio();
            Modulo.P_Caja_ID = Cmb_Numero_Caja.SelectedItem.Value.Trim();
            DataSet Tabla = Modulo.Consultar_Modulos();
            Txt_Modulo.Text = Tabla.Tables[0].Rows[0]["CLAVE"].ToString();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Cajeros
    ///DESCRIPCIÓN: Metodo que llena el Combo de Cajeros con los cajeros existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Cajeros()
    {
        try
        {
            Cls_Ope_Pre_Recolecciones_Negocio Cajero = new Cls_Ope_Pre_Recolecciones_Negocio();
            Cajero.P_Caja_ID = Cmb_Numero_Caja.SelectedItem.Value.Trim();
            DataTable Cajeros = Cajero.Llenar_Combo_Cajeros();
            DataRow fila = Cajeros.NewRow();
            fila[Cat_Empleados.Campo_Nombre] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            fila[Cat_Empleados.Campo_Empleado_ID] = "SELECCIONE";
            Cajeros.Rows.InsertAt(fila, 0);
            Cmb_Cajero.DataTextField = Cat_Empleados.Campo_Nombre;
            Cmb_Cajero.DataValueField = Cat_Empleados.Campo_Empleado_ID;
            Cmb_Cajero.DataSource = Cajeros;
            Cmb_Cajero.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Empleados
    ///DESCRIPCIÓN: Metodo que llena el Combo de Empleados con los empleados existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Empleados()
    {
        try
        {
            Cls_Ope_Pre_Recolecciones_Negocio Cajero = new Cls_Ope_Pre_Recolecciones_Negocio();
            DataTable Cajeros = Cajero.LLenar_Combo_Empleados();
            DataRow fila = Cajeros.NewRow();
            fila[Cat_Empleados.Campo_Nombre] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            fila[Cat_Empleados.Campo_Empleado_ID] = "SELECCIONE";
            Cajeros.Rows.InsertAt(fila, 0);
            Cmb_Cajero.DataTextField = Cat_Empleados.Campo_Nombre;
            Cmb_Cajero.DataValueField = Cat_Empleados.Campo_Empleado_ID;
            Cmb_Cajero.DataSource = Cajeros;
            Cmb_Cajero.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    
    #endregion

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles del Formulario
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Txt_Modulo.Text = "";
        Txt_Numero_Recoleccion.Text = "";
        Txt_Fecha.Text = "";
        Txt_Monto_Recolectado.Text = "";
        Cmb_Cajero.SelectedIndex = 0;
        Cmb_Numero_Caja.SelectedIndex = 0;
        Grid_Recolecciones.DataSource = new DataTable();
        Grid_Recolecciones.DataBind();
    }

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación que se vea afectada en la basae de datos.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes_Generales()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Cmb_Numero_Caja.SelectedIndex == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Numero de Caja.";
            Validacion = false;
        }
        if (Cmb_Cajero.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Cajero.";
            Validacion = false;
        }
        if (Txt_Modulo.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Modulo.";
            Validacion = false;
        }
        if (Txt_Numero_Recoleccion.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Numero de Recoleccion.";
            Validacion = false;
        }
        if (Txt_Fecha.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir la Fecha.";
            Validacion = false;
        }
        if (Txt_Monto_Recolectado.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Monto Recolectado.";
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

    #region Grid

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Recolecciones
    ///DESCRIPCIÓN: Llena la tabla de Recolecciones
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_View
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Recolecciones(int Pagina)
    {
        try
        {
            Cls_Ope_Pre_Recolecciones_Negocio Recolecciones = new Cls_Ope_Pre_Recolecciones_Negocio();
            Grid_Recolecciones.DataSource = Recolecciones.Consultar_Recolecciones();
            Grid_Recolecciones.PageIndex = Pagina;
            Grid_Recolecciones.Columns[1].Visible = true;
            Grid_Recolecciones.Columns[4].Visible = true;
            Grid_Recolecciones.Columns[6].Visible = true;
            Grid_Recolecciones.DataBind();
            Grid_Recolecciones.Columns[1].Visible = false;
            Grid_Recolecciones.Columns[4].Visible = false;
            Grid_Recolecciones.Columns[6].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Recolecciones_Busqueda
    ///DESCRIPCIÓN: Llena la tabla de Recolecciones de auerdo a la busqueda introducida.
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Recolecciones_Busqueda(int Pagina)
    {
        try
        {
            Cls_Ope_Pre_Recolecciones_Negocio Recolecciones = new Cls_Ope_Pre_Recolecciones_Negocio();
            Recolecciones.P_Caja_ID  = Txt_Busqueda.Text.ToUpper().Trim();
            Grid_Recolecciones.DataSource = Recolecciones.Consultar_Recolecciones_Busqueda();
            Grid_Recolecciones.PageIndex = Pagina;
            Grid_Recolecciones.Columns[1].Visible = true;
            Grid_Recolecciones.Columns[4].Visible = true;
            Grid_Recolecciones.Columns[6].Visible = true;
            Grid_Recolecciones.DataBind();
            Grid_Recolecciones.Columns[1].Visible = false;
            Grid_Recolecciones.Columns[4].Visible = false;
            Grid_Recolecciones.Columns[6].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #endregion

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Recolecciones_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView General de Recolecciones
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Recolecciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpiar_Catalogo();
            Grid_Recolecciones.SelectedIndex = (-1);
            Llenar_Tabla_Recolecciones(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Recolecciones_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de la Recoleccion Seleccionada para mostrarlos a detalle
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Recolecciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Recolecciones.SelectedIndex > (-1))
            {
                Txt_Recoleccion_ID.Text = Grid_Recolecciones.SelectedRow.Cells[1].Text;
                Cmb_Numero_Caja.SelectedIndex = Cmb_Numero_Caja.Items.IndexOf(Cmb_Numero_Caja.Items.FindByValue(Grid_Recolecciones.SelectedRow.Cells[4].Text));
                Txt_Modulo.Text = Grid_Recolecciones.SelectedRow.Cells[3].Text;
                Cmb_Cajero.SelectedIndex = Cmb_Cajero.Items.IndexOf(Cmb_Cajero.Items.FindByValue(Grid_Recolecciones.SelectedRow.Cells[6].Text));
                Txt_Numero_Recoleccion.Text = Grid_Recolecciones.SelectedRow.Cells[2].Text;  
                Txt_Fecha.Text = Grid_Recolecciones.SelectedRow.Cells[9].Text;
                Txt_Monto_Recolectado.Text = Grid_Recolecciones.SelectedRow.Cells[8].Text;
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

    #endregion 

    #region Combos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Numero_Caja_SelectedIndexChanged
    ///DESCRIPCIÓN: Llena automaticamente el combo de Cajeros para identificar a la Caja que esta
    ///             asignado ese Cajero
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Numero_Caja_SelectedIndexChanged(object sender, EventArgs e)
    {
        Llenar_Caja_Modulo();
        Llenar_Combo_Cajeros();
    }

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una nueva Recoleccion
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011
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
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Imprimir.Visible = false;
                Txt_Numero_Recoleccion.Enabled = false;
                Txt_Modulo.Enabled = false;
                Cls_Ope_Pre_Recolecciones_Negocio Recolecciones = new Cls_Ope_Pre_Recolecciones_Negocio();
                Txt_Numero_Recoleccion.Text = Recolecciones.Obtener_Numero_Recoleccion();
                if (Txt_Numero_Recoleccion.Text == "") 
                {
                    Txt_Numero_Recoleccion.Text = "0000000001";
                }
            }
            else
            {
                if (Validar_Componentes_Generales())
                {

                    Cls_Ope_Pre_Recolecciones_Negocio Recolecciones = new Cls_Ope_Pre_Recolecciones_Negocio();
                    Recolecciones.P_Caja_ID = Cmb_Numero_Caja.SelectedItem.Value;
                    Recolecciones.P_Cajero_ID = Cmb_Cajero.SelectedItem.Value;
                    Recolecciones.P_Num_Recoleccion = Txt_Numero_Recoleccion.Text.Trim();
                    Recolecciones.P_Fecha = Txt_Fecha.Text.ToUpper().Trim();
                    Recolecciones.P_Mnt_Recoleccion = Txt_Monto_Recolectado.Text.ToUpper().Trim();
                    Recolecciones.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Limpiar_Catalogo();
                    Grid_Recolecciones.Columns[1].Visible = true;
                    Grid_Recolecciones.Columns[4].Visible = true;
                    Grid_Recolecciones.Columns[6].Visible = true;
                    Recolecciones.Alta_Recoleccion();
                    Grid_Recolecciones.Columns[1].Visible = false;
                    Grid_Recolecciones.Columns[4].Visible = false;
                    Grid_Recolecciones.Columns[6].Visible = false;
                    Configuracion_Formulario(true);
                    Llenar_Tabla_Recolecciones(Grid_Recolecciones.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Proceso de Recolecciones", "alert('Alta de Recoleccion Exitosa');", true);
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Recolecciones.Enabled = true;
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN: Llena la Tabla de Recolecciones con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Limpiar_Catalogo();
            Llenar_Tabla_Recolecciones_Busqueda(0);
            if (Grid_Recolecciones.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
            {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Concepto\"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
                Lbl_Mensaje_Error.Text = "(Se cargaron  todas las Recolecciones almacenadas)";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Busqueda.Text = "";
                Llenar_Tabla_Recolecciones(0);
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
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Salir.AlternateText.Equals("Salir"))
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Configuracion_Formulario(true);
                Limpiar_Catalogo();
                Llenar_Tabla_Recolecciones(0);
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Imprimir.Visible = true;
                Grid_Recolecciones.Enabled = true;
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
            Botones.Add(Btn_Imprimir);
            Botones.Add(Btn_Buscar);

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
