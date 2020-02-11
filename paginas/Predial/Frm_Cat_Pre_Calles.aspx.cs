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
using Presidencia.Catalogo_Calles.Negocio;
using Presidencia.Catalogo_Tipos_Colonias.Negocio;

public partial class paginas_predial_Frm_Cat_Pre_Calles : System.Web.UI.Page
{

    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 20/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        Lbl_Ecabezado_Mensaje.Text = "";
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;

        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Configuracion_Acceso("Frm_Cat_Pre_Calles.aspx");
                Configuracion_Formulario(true);
                Llenar_Tabla_Calles(0);
                //Llenar_Combo_Colonias();
            }

            // Script para mostrar Ventana Modal de las Tasas de Traslado
            String Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Mostrar_Busqueda_Avanzada_Colonias.Attributes.Add("onclick", Ventana_Modal);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    #endregion

    #region Metodos

    ///****************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///             1. Estatus.    Estatus en el que se cargara la configuración de los controles.
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 20/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Estatus)
    {
        Btn_Nuevo.Visible = true;
        Btn_Nuevo.AlternateText = "Nuevo";
        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        Btn_Modificar.Visible = true;
        Btn_Modificar.AlternateText = "Modificar";
        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        Btn_Eliminar.Visible = Estatus;
        Txt_Clave.Enabled = !Estatus;
        Txt_Comentarios.Enabled = !Estatus;
        Txt_Nombre.Enabled = !Estatus;
        Cmb_Estatus.Enabled = !Estatus;
        Btn_Mostrar_Busqueda_Avanzada_Colonias.Enabled = !Estatus;
        Txt_Colonia.Enabled = false;
        Grid_Calles.Enabled = Estatus;
        Grid_Calles.SelectedIndex = (-1);
        Btn_Buscar_Calles.Enabled = Estatus;
        Txt_Busqueda_Calles.Enabled = Estatus;
        Cmb_Filtro_Busqueda.Enabled = Estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles del Formulario
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 20/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Txt_Calle_ID.Text = "";
        Txt_Clave.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
        Txt_Colonia.Text = "";
        Hdn_Colonia_ID.Value = "";
        Hdn_Colonia_ID_Anterior.Value = "";
        Txt_Comentarios.Text = "";
        Txt_Nombre.Text = "";
        Hf_Busqueda.Value = "";
        Hf_Filtro_Busqueda.Value = "";

    }

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación que se vea afectada en la basae de datos.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 20/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes_Generales()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Txt_Clave.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el la P_Clave de la Calle.";
            Validacion = false;
        }
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
            Validacion = false;
        }
        if (Hdn_Colonia_ID.Value == "")
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una Colonia.";
            Validacion = false;
        }
        if (Txt_Nombre.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre.";
            Validacion = false;
        }
        else if (Txt_Nombre.Text.Trim().Length > 0 && Hdn_Colonia_ID.Value != "")
        {
            // validar que sea una calle nueva o se cambie la colonia al modificar una calle
            if (Hdn_Colonia_ID_Anterior.Value == "" || (Hdn_Colonia_ID_Anterior.Value != "" && Hdn_Colonia_ID_Anterior.Value != Hdn_Colonia_ID.Value))
            {
                if (Validar_Calle_Existe())
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ La Calle ya existe en la colonia seleccionada.";
                    Validacion = false;
                }
            }
        }

        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = Mensaje_Error;
            Lbl_Mensaje_Error.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Comentarios
    ///DESCRIPCIÓN: Hace una validacion de que el campo de Comentarios no se repita en 
    ///             la base de datos
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 18/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Calle_Existe()
    {
        Cls_Cat_Pre_Calles_Negocio Calles = new Cls_Cat_Pre_Calles_Negocio();
        DataTable Dt_Calles;
        // consultar calles no el nombre especificado con el mismo numero de colonia
        Calles.P_Nombre_Calle = Txt_Nombre.Text.Trim().ToUpper();
        Calles.P_Colonia_ID = Hdn_Colonia_ID.Value;
        Dt_Calles = Calles.Consultar_Calles();

        // si se obtuvieron resultados de la consulta, la calle ya existe
        if (Dt_Calles != null && Dt_Calles.Rows.Count > 0)
        {
            return true;
        }
        return false;
    }

    #endregion

    #region Grid

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Calles
    ///DESCRIPCIÓN: Llena la tabla de Calles
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 20/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Calles(int Pagina)
    {
        try
        {
            Cls_Cat_Pre_Calles_Negocio Calles = new Cls_Cat_Pre_Calles_Negocio();
            Grid_Calles.DataSource = Calles.Consultar_Calles();
            Grid_Calles.PageIndex = Pagina;
            Grid_Calles.Columns[1].Visible = true;
            Grid_Calles.Columns[6].Visible = true;
            Grid_Calles.Columns[7].Visible = true;
            Grid_Calles.DataBind();
            Grid_Calles.Columns[1].Visible = false;
            Grid_Calles.Columns[6].Visible = false;
            Grid_Calles.Columns[7].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Calles_Busqueda
    ///DESCRIPCIÓN: Llena la tabla de Calles de acuerdo a la busqueda introducida.
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 20/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Calles_Busqueda(int Pagina, String Busqueda, String Filtro)
    {
        Lbl_Ecabezado_Mensaje.Text = "";
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;

        try
        {
            Cls_Cat_Pre_Calles_Negocio Calles = new Cls_Cat_Pre_Calles_Negocio();
            if (Filtro.Equals("CALLE"))
            {
                Calles.P_Nombre_Calle = Hf_Busqueda.Value;
            }
            else if (Filtro.Equals("COLONIA"))
            {
                Calles.P_Nombre_Colonia = Hf_Busqueda.Value;
            }
            else
            {
                Calles.P_Clave = Hf_Busqueda.Value;
            }
            Grid_Calles.DataSource = Calles.Consultar_Nombre();
            Grid_Calles.PageIndex = Pagina;
            Grid_Calles.Columns[6].Visible = true;
            Grid_Calles.Columns[1].Visible = true;
            Grid_Calles.Columns[7].Visible = true;
            Grid_Calles.DataBind();
            Grid_Calles.Columns[1].Visible = false;
            Grid_Calles.Columns[6].Visible = false;
            Grid_Calles.Columns[7].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Clave_Calle
    ///DESCRIPCIÓN: Consulta el consecutivo de calles para obtener el número de la siguiente calle a dar de alta
    ///PROPIEDADES:     
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-dic-2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Obtener_Clave_Calle()
    {
        Cls_Cat_Pre_Calles_Negocio Calles = new Cls_Cat_Pre_Calles_Negocio();
        Txt_Clave.Text = Calles.Ultima_Clave();
    }

    #endregion

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Calles_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView General de Calles
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 20/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Calles_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Lbl_Ecabezado_Mensaje.Text = "";
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;

        try
        {
            Grid_Calles.SelectedIndex = (-1);
            if (Hf_Busqueda.Value == "")
            {
                Llenar_Tabla_Calles(e.NewPageIndex);
            }
            else
            {
                Llenar_Tabla_Calles_Busqueda(e.NewPageIndex, Hf_Busqueda.Value, Hf_Filtro_Busqueda.Value);
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Calles_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de la Calle Seleccionada para mostrarlos a detalle
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 20/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Calles_SelectedIndexChanged(object sender, EventArgs e)
    {
        Lbl_Ecabezado_Mensaje.Text = "";
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;

        try
        {
            if (Grid_Calles.SelectedIndex > (-1))
            {
                Txt_Calle_ID.Text = Grid_Calles.SelectedRow.Cells[1].Text;
                Txt_Colonia.Text = HttpUtility.HtmlDecode(Grid_Calles.SelectedRow.Cells[4].Text);
                Hdn_Colonia_ID.Value = HttpUtility.HtmlDecode(Grid_Calles.SelectedRow.Cells[7].Text);
                Hdn_Colonia_ID_Anterior.Value = HttpUtility.HtmlDecode(Grid_Calles.SelectedRow.Cells[7].Text);
                Txt_Clave.Text = Grid_Calles.SelectedRow.Cells[2].Text;
                Txt_Nombre.Text = HttpUtility.HtmlDecode(Grid_Calles.SelectedRow.Cells[3].Text);
                Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByText(HttpUtility.HtmlDecode(Grid_Calles.SelectedRow.Cells[5].Text)));
                Txt_Comentarios.Text = HttpUtility.HtmlDecode(Grid_Calles.SelectedRow.Cells[6].Text);
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

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una nueva Calle
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 20/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, EventArgs e)
    {
        Lbl_Ecabezado_Mensaje.Text = "";
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;

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
                Btn_Modificar.Visible = false;
                Txt_Clave.Enabled = false;
                Grid_Calles.Visible = false;
                Obtener_Clave_Calle();
            }
            else
            {
                if (Validar_Componentes_Generales())
                {
                    Cls_Cat_Pre_Calles_Negocio Calles = new Cls_Cat_Pre_Calles_Negocio();
                    Calles.P_Calle_ID = Txt_Calle_ID.Text.Trim();
                    Calles.P_Colonia_ID = Hdn_Colonia_ID.Value;
                    Calles.P_Clave = Txt_Clave.Text.Trim().ToUpper();
                    Calles.P_Nombre = Txt_Nombre.Text.ToUpper().Trim();
                    Calles.P_Estatus = Cmb_Estatus.SelectedItem.Text;
                    Calles.P_Comentarios = Txt_Comentarios.Text.ToUpper();
                    Calles.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                    Calles.Alta_Calle();

                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Tabla_Calles(Grid_Calles.PageIndex);
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Calles.Enabled = true;
                    Grid_Calles.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Calles", "alert('Alta de Calle Exitosa');", true);
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
    ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de una Calle
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 20/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, EventArgs e)
    {
        Lbl_Ecabezado_Mensaje.Text = "";
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;

        try
        {
            if (Btn_Modificar.AlternateText.Equals("Modificar"))
            {
                if (Grid_Calles.Rows.Count > 0 && Grid_Calles.SelectedIndex > (-1))
                {
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                    Txt_Clave.Enabled = false;
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Modificar.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                if (Validar_Componentes_Generales())
                {
                    Cls_Cat_Pre_Calles_Negocio Calles = new Cls_Cat_Pre_Calles_Negocio();
                    Calles.P_Calle_ID = Txt_Calle_ID.Text.Trim();
                    Calles.P_Nombre = Txt_Nombre.Text.ToUpper().Trim();
                    Calles.P_Comentarios = Txt_Comentarios.Text.ToUpper();
                    Calles.P_Colonia_ID = Hdn_Colonia_ID.Value;
                    Calles.P_Estatus = Cmb_Estatus.SelectedItem.Text.ToUpper().Trim();
                    Calles.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                    Grid_Calles.Columns[1].Visible = true;
                    Grid_Calles.Columns[6].Visible = true;
                    Grid_Calles.Columns[7].Visible = true;
                    Calles.Modificar_Calle();
                    Grid_Calles.Columns[1].Visible = false;
                    Grid_Calles.Columns[6].Visible = false;
                    Grid_Calles.Columns[7].Visible = false;
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Tabla_Calles(Grid_Calles.PageIndex);
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Calles.Enabled = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Calles", "alert('Actualización de Calle Exitosa');", true);
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
    ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 20/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Ecabezado_Mensaje.Text = "";
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;

        try
        {
            Limpiar_Catalogo();
            Hf_Busqueda.Value = Txt_Busqueda_Calles.Text.ToUpper().Trim();
            Hf_Filtro_Busqueda.Value = Cmb_Filtro_Busqueda.SelectedValue;
            Txt_Busqueda_Calles.Text = "";

            Llenar_Tabla_Calles_Busqueda(0, Hf_Busqueda.Value, Hf_Filtro_Busqueda.Value);

            if (Grid_Calles.Rows.Count == 0 && Txt_Busqueda_Calles.Text.Trim().Length > 0)
            {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Concepto\"" + Txt_Busqueda_Calles.Text + "\" no se encotrarón coincidencias";
                Lbl_Mensaje_Error.Text = "(Se cargaron  todas las Calles almacenadas)";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Busqueda_Calles.Text = "";
                Llenar_Tabla_Calles_Busqueda(0, Hf_Busqueda.Value, Hf_Filtro_Busqueda.Value);
            }
            Cmb_Filtro_Busqueda.SelectedIndex = 0;
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
    ///DESCRIPCIÓN: Elimina una Calle de la Base de Datos
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 20/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, EventArgs e)
    {
        Lbl_Ecabezado_Mensaje.Text = "";
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;

        try
        {
            if (Grid_Calles.Rows.Count > 0 && Grid_Calles.SelectedIndex > (-1))
            {
                Cls_Cat_Pre_Calles_Negocio Calles = new Cls_Cat_Pre_Calles_Negocio();
                Calles.P_Colonia_ID = Grid_Calles.SelectedRow.Cells[1].Text;
                Calles.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                Grid_Calles.Columns[1].Visible = true;
                Grid_Calles.Columns[6].Visible = true;
                Grid_Calles.Columns[7].Visible = true;
                Calles.Eliminar_Calle();
                Grid_Calles.Columns[1].Visible = false;
                Grid_Calles.Columns[6].Visible = false;
                Grid_Calles.Columns[7].Visible = false;
                Grid_Calles.SelectedIndex = 0;
                Llenar_Tabla_Calles(Grid_Calles.PageIndex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Calles", "alert('La Calle fue eliminada exitosamente');", true);
                Limpiar_Catalogo();
                Llenar_Tabla_Calles(0);
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
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 20/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        Lbl_Ecabezado_Mensaje.Text = "";
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;

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
                Llenar_Tabla_Calles(0);
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Grid_Calles.Enabled = true;
                Grid_Calles.Visible = true;
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
    ///NOMBRE DE LA FUNCIÓN : Btn_Mostrar_Busqueda_Avanzada_Colonias
    ///DESCRIPCIÓN          : Muestra los datos de la consulta
    ///PARAMETROS:     
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 18-dic-2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Mostrar_Busqueda_Avanzada_Colonias_Click(object sender, ImageClickEventArgs e)
    {
        Boolean Busqueda_Colonias;

        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        Busqueda_Colonias = Convert.ToBoolean(Session["BUSQUEDA_COLONIAS"]);
        if (Busqueda_Colonias)
        {
            if (Session["COLONIA_ID"] != null && Session["NOMBRE_COLONIA"] != null)
            {
                Hdn_Colonia_ID.Value = Convert.ToString(Session["COLONIA_ID"]);
                Txt_Colonia.Text = Convert.ToString(Session["NOMBRE_COLONIA"]);
                Session.Remove("COLONIA_ID");
                Session.Remove("NOMBRE_COLONIA");
            }
        }
        // limpiar variables de sesion
        Session["BUSQUEDA_COLONIAS"] = null;
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
            Botones.Add(Btn_Buscar_Calles);

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