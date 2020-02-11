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
using Presidencia.Sessiones;
using Presidencia.Catalogo_Claves_Grupos_Movimiento.Negocio;
using Presidencia.Catalogo_Tipos_Predio.Negocio;
using Presidencia.Constantes;

public partial class paginas_predial_Frm_Cat_Pre_Claves_Grupos_Movimiento : System.Web.UI.Page
{
    DataTable P_Dt_Detalles_Folios
    {
        get
        {
            DataTable Dt_Detalles_Folios = null;
            if (Session["Dt_Detalles_Folios"] != null)
            {
                Dt_Detalles_Folios = (DataTable)Session["Dt_Detalles_Folios"];
            }
            return Dt_Detalles_Folios;
        }
        set
        {
            Session["Dt_Detalles_Folios"] = null;
            if (value != null)
            {
                Session["Dt_Detalles_Folios"] = value.Copy();
            }
        }
    }

    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 29/Octubre/2010 
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
                Configuracion_Acceso("Frm_Cat_Pre_Claves_Grupos_Movimiento.aspx");
                Configuracion_Formulario(true);
                P_Dt_Detalles_Folios = null;
                Llenar_Grid_Grupos_Movimiento(0);
                Llenar_Combo_Tipos_Predio();
                Div_Contenedor_Msj_Error.Visible = false;
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

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///             1. Estatus.    Estatus en el que se cargara la configuración de los
    ///                             controles.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 29/Octubre/2010 
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
        Grid_Grupos_Movimiento.Visible = Estatus;

        Txt_Busqueda_Clave_Grupo_Movimiento.Enabled = Estatus;
        Btn_Buscar_Clave_Grupo_Movimiento.Enabled = Estatus;
        Txt_Nombre.Enabled = !Estatus;
        Txt_Clave_Grupo_Movimiento.Enabled = !Estatus;
        Cmb_Estatus.Enabled = !Estatus;

        Txt_Folio_Inicial.Enabled = !Estatus;
        Txt_Año.Enabled = !Estatus;
        Cmb_Tipos_Predio.Enabled = !Estatus;
        Btn_Agregar_Detalle.Enabled = !Estatus;
        Btn_Actualizar_Detalle.Enabled = !Estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles del Formulario
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 29/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Txt_Clave_Grupo_Movimiento.Text = "";
        Txt_Nombre.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
        Hf_Clave_Grupo_Movimiento_ID.Value = "";
        Grid_Grupos_Movimiento.Enabled = true;

        Txt_Folio_Inicial.Text = "";
        Txt_Año.Text = "";
        Cmb_Tipos_Predio.SelectedIndex = 0;
        Grid_Grupos_Movimiento_Detalle_Folios.DataBind();

        P_Dt_Detalles_Folios = null;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Grupos_Movimiento
    ///DESCRIPCIÓN: Llena la tabla de Sectores con una consulta que puede o no
    ///             tener Filtros.
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 12/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Grupos_Movimiento(int Pagina)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            Grid_Grupos_Movimiento.Columns[1].Visible = true;
            //Grid_Grupos_Movimiento.Columns[4].Visible = true;
            Cls_Cat_Pre_Claves_Grupos_Movimiento_Negocio Sector = new Cls_Cat_Pre_Claves_Grupos_Movimiento_Negocio();
            Sector.P_Tipo_DataTable = "GRUPOS_MOVIMIENTO";
            Sector.P_Nombre = Txt_Busqueda_Clave_Grupo_Movimiento.Text.Trim().ToUpper();
            Grid_Grupos_Movimiento.DataSource = Sector.Consultar_DataTable();
            Grid_Grupos_Movimiento.PageIndex = Pagina;
            Grid_Grupos_Movimiento.DataBind();
            Grid_Grupos_Movimiento.Columns[1].Visible = false;
            //Grid_Grupos_Movimiento.Columns[4].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Llenar_Grid_Grupos_Movimiento_Detalles
    ///DESCRIPCIÓN          : Carga el Grid de detalles con los Folios Iniiales por Año
    ///PROPIEDADES          : Pagina.  Dato numérico para inicializar la página a mostrar del Grid de Detalles
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Grupos_Movimiento_Detalles(int Pagina)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            Cls_Cat_Pre_Claves_Grupos_Movimiento_Negocio Detalles_Folios = new Cls_Cat_Pre_Claves_Grupos_Movimiento_Negocio();
            DataTable Dt_Detalles_Folios;
            Detalles_Folios.P_Grupo_Movimiento_ID = Grid_Grupos_Movimiento.SelectedRow.Cells[1].Text;
            Dt_Detalles_Folios = Detalles_Folios.Consultar_Detalles_Folios();
            Dt_Detalles_Folios.DefaultView.Sort = Cat_Pre_Grupos_Movimiento_Detalles.Campo_Año + " DESC";
            Dt_Detalles_Folios = Dt_Detalles_Folios.DefaultView.ToTable();
            Grid_Grupos_Movimiento_Detalle_Folios.DataSource = Dt_Detalles_Folios;
            Grid_Grupos_Movimiento_Detalle_Folios.PageIndex = Pagina;
            Grid_Grupos_Movimiento_Detalle_Folios.DataBind();
            P_Dt_Detalles_Folios = Dt_Detalles_Folios;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 29/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes()
    {

        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Txt_Nombre.Text.Trim().Length == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre.";
            Validacion = false;
        }
        if (Txt_Clave_Grupo_Movimiento.Text.Trim().Length == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Introducir la Clave.";
            Validacion = false;
        }
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Seleccione una opción en el combo de Estatus.";
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Sectores_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de los Sectores
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 29/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Grupos_Movimiento_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            Grid_Grupos_Movimiento.SelectedIndex = (-1);
            Llenar_Grid_Grupos_Movimiento(e.NewPageIndex);
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Sectores_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de un Sector Seleccionado para mostrarlos a detalle
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 29/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Grupos_Movimiento_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            if (Grid_Grupos_Movimiento.SelectedIndex > (-1))
            {
                Limpiar_Catalogo();
                Hf_Clave_Grupo_Movimiento_ID.Value = HttpUtility.HtmlDecode(Grid_Grupos_Movimiento.SelectedRow.Cells[1].Text.Trim());
                Txt_Clave_Grupo_Movimiento.Text = HttpUtility.HtmlDecode(Grid_Grupos_Movimiento.SelectedRow.Cells[2].Text.Trim());
                Txt_Nombre.Text = HttpUtility.HtmlDecode(Grid_Grupos_Movimiento.SelectedRow.Cells[3].Text.Trim());
                Cmb_Estatus.SelectedValue = HttpUtility.HtmlDecode(Grid_Grupos_Movimiento.SelectedRow.Cells[4].Text.Trim());
                Llenar_Grid_Grupos_Movimiento_Detalles(0);
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
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una nuevo Sector.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 29/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, EventArgs e)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                Configuracion_Formulario(false);
                Limpiar_Catalogo();
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.Visible = false;
                Grid_Grupos_Movimiento.Visible = false;
                Btn_Actualizar_Detalle.Enabled = false;
            }
            else
            {
                if (Validar_Componentes() && Btn_Nuevo.AlternateText.Equals("Dar de Alta"))
                {
                    Cls_Cat_Pre_Claves_Grupos_Movimiento_Negocio Sector = new Cls_Cat_Pre_Claves_Grupos_Movimiento_Negocio();
                    Sector.P_Clave = Txt_Clave_Grupo_Movimiento.Text.Trim().ToUpper();
                    Sector.P_Nombre = Txt_Nombre.Text.Trim().ToUpper();
                    Sector.P_Estatus = Cmb_Estatus.SelectedValue;
                    Sector.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                    Sector.P_Dt_Detalles_Folios = P_Dt_Detalles_Folios;
                    Sector.Alta_Grupo_Movimiento();
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Grid_Grupos_Movimiento(Grid_Grupos_Movimiento.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Claves de Grupos de Movimiento", "alert('Alta de Clave de Grupo de Movimiento exitosa');", true);
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
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
    ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un Sector
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 29/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, EventArgs e)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            if (Btn_Modificar.AlternateText.Equals("Modificar"))
            {
                if (Grid_Grupos_Movimiento.Rows.Count > 0 && Grid_Grupos_Movimiento.SelectedIndex > (-1))
                {
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                    //Txt_Clave_Grupo_Movimiento.Enabled = false;
                    Grid_Grupos_Movimiento.Visible = true;
                    Grid_Grupos_Movimiento.Enabled = false;
                    Btn_Actualizar_Detalle.Enabled = false;

                    Txt_Folio_Inicial.Text = "";
                    Txt_Año.Text = "";
                    Cmb_Tipos_Predio.SelectedIndex = 0;
                    Grid_Grupos_Movimiento_Detalle_Folios.SelectedIndex = -1;
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

                if (Validar_Componentes())
                {
                    Cls_Cat_Pre_Claves_Grupos_Movimiento_Negocio Sector = new Cls_Cat_Pre_Claves_Grupos_Movimiento_Negocio();
                    Sector.P_Grupo_Movimiento_ID = Hf_Clave_Grupo_Movimiento_ID.Value;
                    Sector.P_Clave = Txt_Clave_Grupo_Movimiento.Text.ToUpper().Trim();
                    Sector.P_Nombre = Txt_Nombre.Text.Trim().ToUpper();
                    Sector.P_Estatus = Cmb_Estatus.SelectedValue;
                    Sector.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                    Sector.P_Dt_Detalles_Folios = P_Dt_Detalles_Folios;
                    Sector.Modificar_Grupo_Movimiento();
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Grid_Grupos_Movimiento(Grid_Grupos_Movimiento.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Claves de Grupos de Movimiento", "alert('Actualización de Clave de Grupo de Movimiento exitosa');", true);
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Sector_Click
    ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 29/Octubre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            Limpiar_Catalogo();
            Grid_Grupos_Movimiento.SelectedIndex = (-1);
            Llenar_Grid_Grupos_Movimiento(0);
            if (Grid_Grupos_Movimiento.Rows.Count == 0 && Txt_Busqueda_Clave_Grupo_Movimiento.Text.Trim().Length > 0)
            {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el nombre \"" + Txt_Busqueda_Clave_Grupo_Movimiento.Text + "\" no se encontraron coincidencias";
                Lbl_Mensaje_Error.Text = "(Se cargaron todas las Claves de Grupos de Movimiento almacenados)";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Busqueda_Clave_Grupo_Movimiento.Text = "";
                Llenar_Grid_Grupos_Movimiento(0);
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
    ///DESCRIPCIÓN: Elimina un Estado Predio de la Base de Datos
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 29/Octubre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, EventArgs e)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            if (Grid_Grupos_Movimiento.Rows.Count > 0 && Grid_Grupos_Movimiento.SelectedIndex > (-1))
            {
                Cls_Cat_Pre_Claves_Grupos_Movimiento_Negocio Sector = new Cls_Cat_Pre_Claves_Grupos_Movimiento_Negocio();
                Sector.P_Grupo_Movimiento_ID = Grid_Grupos_Movimiento.SelectedRow.Cells[1].Text;
                Sector.Eliminar_Grupo_Movimiento();
                Grid_Grupos_Movimiento.SelectedIndex = (-1);
                Llenar_Grid_Grupos_Movimiento(Grid_Grupos_Movimiento.PageIndex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Claves de Grupo de Movimiento", "alert('La Clave de Grupo de Movimiento fue eliminada exitosamente');", true);
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
    ///FECHA_CREO: 29/Octubre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Configuracion_Formulario(true);
            Limpiar_Catalogo();
            Grid_Grupos_Movimiento.SelectedIndex = -1;
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
        }
    }

    #endregion

    #region

    //protected void Tab_Colonias_Click()
    //{
    //    try
    //    {
    //        //Tab_Sectores.Enabled = false;
    //        Btn_Nuevo.AlternateText = "Colonias";
    //        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
    //        Btn_Modificar.Visible = false;
    //        Btn_Eliminar.Visible = false;
    //        Btn_Salir.AlternateText = "Cancelar_Colonia";
    //        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //        Lbl_Mensaje_Error.Text = "";
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }
    //}

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
            Botones.Add(Btn_Buscar_Clave_Grupo_Movimiento);

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


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Agregar_Detalle_Click
    ///DESCRIPCIÓN          : Evento de Botón para Agrega una nueva fila con los datos del Folio Inicial, Año y Tipo de Predio al Grid de detalles
    ///PROPIEDADES:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Detalle_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        if (Txt_Folio_Inicial.Text.Trim() != "")
        {
            if (Txt_Año.Text.Trim() != "")
            {
                if (Cmb_Tipos_Predio.SelectedIndex > 0)
                {
                    DataTable Dt_Detalles_Folios = new DataTable();
                    Dt_Detalles_Folios.Columns.Add(Cat_Pre_Grupos_Movimiento_Detalles.Campo_Grupo_Movimiento_ID, typeof(String));
                    Dt_Detalles_Folios.Columns.Add(Cat_Pre_Grupos_Movimiento_Detalles.Campo_Tipo_Predio_ID, typeof(String));
                    Dt_Detalles_Folios.Columns.Add(Cat_Pre_Grupos_Movimiento_Detalles.Campo_Folio_Inicial, typeof(Int16));
                    Dt_Detalles_Folios.Columns.Add(Cat_Pre_Grupos_Movimiento_Detalles.Campo_Año, typeof(Int16));
                    Dt_Detalles_Folios.Columns.Add("TIPO_PREDIO", typeof(String));

                    if (P_Dt_Detalles_Folios != null)
                    {
                        Dt_Detalles_Folios = P_Dt_Detalles_Folios;
                    }
                    if (Dt_Detalles_Folios.Select(Cat_Pre_Grupos_Movimiento_Detalles.Campo_Año + " = " + Txt_Año.Text.Trim() + " AND " + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Tipo_Predio_ID + " = " + Cmb_Tipos_Predio.SelectedItem.Value).Length == 0)
                    {
                        DataRow Dr_Detalles_Folios;
                        Dr_Detalles_Folios = Dt_Detalles_Folios.NewRow();
                        Dr_Detalles_Folios[Cat_Pre_Grupos_Movimiento_Detalles.Campo_Grupo_Movimiento_ID] = Hf_Clave_Grupo_Movimiento_ID.Value;
                        Dr_Detalles_Folios[Cat_Pre_Grupos_Movimiento_Detalles.Campo_Tipo_Predio_ID] = Cmb_Tipos_Predio.SelectedItem.Value;
                        Dr_Detalles_Folios[Cat_Pre_Grupos_Movimiento_Detalles.Campo_Folio_Inicial] = Convert.ToInt16(Txt_Folio_Inicial.Text);
                        Dr_Detalles_Folios[Cat_Pre_Grupos_Movimiento_Detalles.Campo_Año] = Convert.ToInt16(Txt_Año.Text);
                        Dr_Detalles_Folios["TIPO_PREDIO"] = Cmb_Tipos_Predio.SelectedItem.Text;
                        Dt_Detalles_Folios.Rows.Add(Dr_Detalles_Folios);

                        Dt_Detalles_Folios.DefaultView.Sort = Cat_Pre_Grupos_Movimiento_Detalles.Campo_Año + " DESC";
                        Grid_Grupos_Movimiento_Detalle_Folios.DataSource = Dt_Detalles_Folios;
                        Grid_Grupos_Movimiento_Detalle_Folios.DataBind();
                        P_Dt_Detalles_Folios = Dt_Detalles_Folios;

                        Txt_Folio_Inicial.Text = "";
                        Txt_Año.Text = "";
                        Cmb_Tipos_Predio.SelectedIndex = 0;
                        Btn_Actualizar_Detalle.Enabled = false;
                        Grid_Grupos_Movimiento_Detalle_Folios.SelectedIndex = -1;
                    }
                    else
                    {
                        Lbl_Ecabezado_Mensaje.Text = "El Detalle que intenta Agregar ya existe";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar un Tipo de Predio";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Debe introducir un Año";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        else
        {
            Lbl_Ecabezado_Mensaje.Text = "Debe introducir un Folio Inicial";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Actualizar_Detalle_Click
    ///DESCRIPCIÓN          : Evento de Botón para Actualizar una fila con los datos del Folio Inicial o Año en el Grid de detalles
    ///PROPIEDADES:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Actualizar_Detalle_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        if (Txt_Folio_Inicial.Text.Trim() != "")
        {
            if (Txt_Año.Text.Trim() != "")
            {
                if (Cmb_Tipos_Predio.SelectedIndex > 0)
                {
                    if (P_Dt_Detalles_Folios != null)
                    {
                        DataTable Dt_Detalles_Folios = P_Dt_Detalles_Folios;
                        int Page_Index = Grid_Grupos_Movimiento_Detalle_Folios.PageIndex;
                        int Page_Size = Grid_Grupos_Movimiento_Detalle_Folios.PageSize;
                        int Selected_Index = Grid_Grupos_Movimiento_Detalle_Folios.SelectedIndex;

                        if (Dt_Detalles_Folios != null)
                        {
                            if (Dt_Detalles_Folios.Rows.Count > 0)
                            {
                                if (Dt_Detalles_Folios.Select(Cat_Pre_Grupos_Movimiento_Detalles.Campo_Año + " = " + Txt_Año.Text.Trim() + " AND " + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Tipo_Predio_ID + " = " + Cmb_Tipos_Predio.SelectedItem.Value + " AND " + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Tipo_Predio_ID + " <> " + Dt_Detalles_Folios.Rows[Page_Index * Page_Size + Selected_Index][Cat_Pre_Grupos_Movimiento_Detalles.Campo_Tipo_Predio_ID].ToString()).Length == 0)
                                {
                                    Dt_Detalles_Folios.DefaultView.Sort = Cat_Pre_Grupos_Movimiento_Detalles.Campo_Año + " DESC";
                                    Dt_Detalles_Folios = Dt_Detalles_Folios.DefaultView.ToTable();
                                    Dt_Detalles_Folios.Rows[Page_Index * Page_Size + Selected_Index][Cat_Pre_Grupos_Movimiento_Detalles.Campo_Tipo_Predio_ID] = Cmb_Tipos_Predio.SelectedItem.Value;
                                    Dt_Detalles_Folios.Rows[Page_Index * Page_Size + Selected_Index][Cat_Pre_Grupos_Movimiento_Detalles.Campo_Año] = Convert.ToInt16(Txt_Año.Text);
                                    Dt_Detalles_Folios.Rows[Page_Index * Page_Size + Selected_Index][Cat_Pre_Grupos_Movimiento_Detalles.Campo_Folio_Inicial] = Convert.ToInt16(Txt_Folio_Inicial.Text);
                                    Dt_Detalles_Folios.Rows[Page_Index * Page_Size + Selected_Index]["TIPO_PREDIO"] = Cmb_Tipos_Predio.SelectedItem.Text;
                                    Grid_Grupos_Movimiento_Detalle_Folios.DataSource = Dt_Detalles_Folios;
                                    Grid_Grupos_Movimiento_Detalle_Folios.PageIndex = Page_Index;
                                    Grid_Grupos_Movimiento_Detalle_Folios.DataBind();
                                    Grid_Grupos_Movimiento_Detalle_Folios.SelectedIndex = Selected_Index;
                                    P_Dt_Detalles_Folios = Dt_Detalles_Folios;
                                }
                                else
                                {
                                    Lbl_Ecabezado_Mensaje.Text = "El Detalle que intenta Modificar ya existe";
                                    Lbl_Mensaje_Error.Text = "";
                                    Div_Contenedor_Msj_Error.Visible = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar un Tipo de Predio";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Debe introducir un Año";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        else
        {
            Lbl_Ecabezado_Mensaje.Text = "Debe introducir un Folio Inicial";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Grupos_Movimiento_Detalle_Folios_SelectedIndexChanged
    ///DESCRIPCIÓN          : Evento SelectedIndexChanged de Grid para Cargar los datos del Detalle y Activar botón Btn_Actualizar_Detalle
    ///PROPIEDADES:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Grupos_Movimiento_Detalle_Folios_SelectedIndexChanged(object sender, EventArgs e)
    {
        Txt_Folio_Inicial.Text = Grid_Grupos_Movimiento_Detalle_Folios.SelectedRow.Cells[3].Text;
        Txt_Año.Text = Grid_Grupos_Movimiento_Detalle_Folios.SelectedRow.Cells[4].Text;
        Cmb_Tipos_Predio.SelectedValue = Grid_Grupos_Movimiento_Detalle_Folios.SelectedDataKey["TIPO_PREDIO_ID"].ToString();
        if (Btn_Nuevo.AlternateText == "Dar de Alta"
            || Btn_Modificar.AlternateText == "Actualizar")
        {
            Btn_Actualizar_Detalle.Enabled = true;
        }
        else
        {
            Btn_Actualizar_Detalle.Enabled = false;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Grupos_Movimiento_Detalle_Folios_PageIndexChanging
    ///DESCRIPCIÓN          : Evento PageIndexChanging de Grid para actualizar el Grid y activar la página seleccionada
    ///PROPIEDADES:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Grupos_Movimiento_Detalle_Folios_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable Dt_Detalles_Folios = P_Dt_Detalles_Folios;
        Dt_Detalles_Folios.DefaultView.Sort = Cat_Pre_Grupos_Movimiento_Detalles.Campo_Año + " DESC";
        Grid_Grupos_Movimiento_Detalle_Folios.DataSource = Dt_Detalles_Folios;
        Grid_Grupos_Movimiento_Detalle_Folios.PageIndex = e.NewPageIndex;
        Grid_Grupos_Movimiento_Detalle_Folios.DataBind();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Grupos_Movimiento_Detalle_Folios_RowCommand
    ///DESCRIPCIÓN          : Evento RowCommand de Grid para quitar filas del Grid
    ///PROPIEDADES:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Grupos_Movimiento_Detalle_Folios_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Quitar_Detalle"
            && (Btn_Nuevo.AlternateText == "Dar de Alta"
                || Btn_Modificar.AlternateText == "Actualizar"))
        {
            DataTable Dt_Detalles_Folios = P_Dt_Detalles_Folios;
            int Page_Index = Grid_Grupos_Movimiento_Detalle_Folios.PageIndex;
            int Page_Size = Grid_Grupos_Movimiento_Detalle_Folios.PageSize;
            int Selected_Index = Convert.ToInt16(e.CommandArgument);

            if (Dt_Detalles_Folios != null)
            {
                if (Dt_Detalles_Folios.Rows.Count > 0)
                {
                    Dt_Detalles_Folios.DefaultView.Sort = Cat_Pre_Grupos_Movimiento_Detalles.Campo_Año + " DESC";
                    Dt_Detalles_Folios = Dt_Detalles_Folios.DefaultView.ToTable();
                    Dt_Detalles_Folios.Rows.RemoveAt(Page_Index * Page_Size + Selected_Index);
                    Grid_Grupos_Movimiento_Detalle_Folios.DataSource = Dt_Detalles_Folios;
                    Grid_Grupos_Movimiento_Detalle_Folios.PageIndex = Page_Index;
                    Grid_Grupos_Movimiento_Detalle_Folios.DataBind();
                    Grid_Grupos_Movimiento_Detalle_Folios.SelectedIndex = Selected_Index;
                    P_Dt_Detalles_Folios = Dt_Detalles_Folios;
                }
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN     : Llenar_Combo_Tipos_Predio
    ///DESCRIPCIÓN              : Consulta los Tipos de Predio en el Catálogo y los carga en el Combo.
    ///PROPIEDADES:     
    ///CREO                     : Antonio Salvador Benavides Guardado.
    ///FECHA_CREO               : 18/Abril/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Tipos_Predio()
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            Cls_Cat_Pre_Tipos_Predio_Negocio Tipos_Predio = new Cls_Cat_Pre_Tipos_Predio_Negocio();
            Tipos_Predio.P_Ordenar_Dinamico = Cat_Pre_Tipos_Predio.Campo_Descripcion;
            DataTable Dt_Tipos_Predios = Tipos_Predio.Consultar_Tipo_Predio();
            DataRow Dr_Fila_Cabecera = Dt_Tipos_Predios.NewRow();
            Dr_Fila_Cabecera[Cat_Pre_Tipos_Predio.Campo_Descripcion] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Dr_Fila_Cabecera[Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID] = "SELECCIONE";
            Dt_Tipos_Predios.Rows.InsertAt(Dr_Fila_Cabecera, 0);
            Cmb_Tipos_Predio.DataTextField = Cat_Pre_Tipos_Predio.Campo_Descripcion;
            Cmb_Tipos_Predio.DataValueField = Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID;
            Cmb_Tipos_Predio.DataSource = Dt_Tipos_Predios;
            Cmb_Tipos_Predio.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
}