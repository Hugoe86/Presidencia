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
using System.Globalization;
using Presidencia.Catalogo_Descuentos_Generales.Negocio;

public partial class paginas_Predial_Frm_Cat_Pre_Descuentos_Generales : System.Web.UI.Page
{
    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load.
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página.
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 22/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************        
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //try
            //{
            //    Configuracion_Acceso("Frm_Cat_Pre_Descuentos_Generales.aspx");
            //}
            //catch (Exception ex)
            //{
            //}
            Configuracion_Formulario(true);
            Llenar_Descuentos_Generales(0);
        }
        Div_Contenedor_error.Visible = false;
    }

    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///             1. estatus.    Estatus en el que se cargara la configuración de los
    ///                            controles.
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 13/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean estatus)
    {
        Btn_Nuevo.Visible = true;
        if (Btn_Modificar.AlternateText.Equals("Actualizar"))
        {
        }
        else
        {
            Cmb_Estatus.SelectedIndex = (0);
            Cmb_Tipo_Descuento.SelectedIndex = (0);
        }
        Btn_Nuevo.AlternateText = "Nuevo";
        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        Btn_Modificar.Visible = true;
        Btn_Modificar.AlternateText = "Modificar";
        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        Txt_Comentarios.Enabled = !estatus;
        Txt_Motivo.Enabled = !estatus;
        Txt_Porcentaje_Descuento.Enabled = !estatus;
        Img_Calendario1.Enabled = !estatus;
        Img_Calendario2.Enabled = !estatus;
        Txt_Vigencia_Desde.Enabled = !estatus;
        Txt_Vigencia_hasta.Enabled = !estatus;
        Cmb_Estatus.Enabled = !estatus;
        Cmb_Tipo_Descuento.Enabled = !estatus;
        Grid_Descuentos_Generales.Enabled = estatus;
        Grid_Descuentos_Generales.SelectedIndex = (-1);
        Btn_Busqueda.Enabled = estatus;
        Txt_Busqueda.Enabled = estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles del Formulario
    ///PROPIEDADES:
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 13/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Txt_Comentarios.Text = "";
        Txt_id.Text = "";
        Txt_Motivo.Text = "";
        Txt_Porcentaje_Descuento.Text = "";
        Txt_Vigencia_Desde.Text = "";
        Txt_Vigencia_hasta.Text = "";
        Txt_Busqueda.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
        Cmb_Tipo_Descuento.SelectedIndex = 0;
    }

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Descuentos_Generales
    ///DESCRIPCIÓN: Llena la tabla de Descuentos Generales
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 13/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Descuentos_Generales(int Pagina)
    {
        try
        {
            Cls_Cat_Pre_Descuentos_Generales_Negocio Descuentos_Generales = new Cls_Cat_Pre_Descuentos_Generales_Negocio();
            Descuentos_Generales.P_Filtro = Txt_Busqueda.Text.Trim().ToUpper().ToUpper();
            Grid_Descuentos_Generales.Columns[1].Visible = true;
            Grid_Descuentos_Generales.Columns[7].Visible = true;
            Grid_Descuentos_Generales.DataSource = Descuentos_Generales.Consultar_descuentos_Generales();
            Grid_Descuentos_Generales.PageIndex = Pagina;
            Grid_Descuentos_Generales.DataBind();
            Grid_Descuentos_Generales.Columns[1].Visible = false;
            Grid_Descuentos_Generales.Columns[7].Visible = false;
        }
        catch (Exception Ex)
        {
            Div_Contenedor_error.Visible = true;
            Lbl_Error.Visible = true;
            Lbl_Ecabezado_Mensaje.Visible = true;
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
        }
    }

    #endregion

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación.
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 13/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Cmb_Estatus.SelectedIndex == 0 || Cmb_Estatus.SelectedValue.Length > 20)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
            Validacion = false;
        }
        if (Txt_Motivo.Text.Trim().Length == 0 || Txt_Motivo.Text.Trim().Length > 100)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Motivo.";
            Validacion = false;
        }
        if (Txt_Porcentaje_Descuento.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el porcentaje de descuento.";
            Validacion = false;
        }
        if (Txt_Vigencia_Desde.Text.Length == 0 || Txt_Vigencia_Desde.Text.Length!=10)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir una Fecha de inicio valida.";
            Validacion = false;
        }
        if (Txt_Vigencia_hasta.Text.Trim().Length == 0 || Txt_Vigencia_hasta.Text.Trim().Length != 10)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir una Fecha de fin valida.";
            Validacion = false;
        }
        if (Cmb_Tipo_Descuento.SelectedIndex == 0 || Cmb_Tipo_Descuento.SelectedValue.Length > 20)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el tipo de descuento.";
            Validacion = false;
        }

        try
        {
            DateTime t1 = Convert.ToDateTime(String.Format("{0:MM/dd/yyyy}", DateTime.ParseExact(Txt_Vigencia_Desde.Text.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)));
            t1.ToShortDateString();
            DateTime t2 = Convert.ToDateTime(String.Format("{0:MM/dd/yyyy}", DateTime.ParseExact(Txt_Vigencia_hasta.Text.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)));
            Int32 resultado = DateTime.Compare(t1, t2);
            if (resultado != (-1))
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ El campo 'vigencia hasta' debe de ser mayor que el campo 'vigencia desde'.";
                Validacion = false;
            }
        } catch(Exception e)
        {
            Mensaje_Error = Mensaje_Error + "<br>";
            Mensaje_Error = Mensaje_Error + "+ Introducir los campos de vigencia correctamente.";
            Validacion = false;
        }
        
        try
        {
            if (Convert.ToInt32(Txt_Porcentaje_Descuento.Text)>100)
            {
                Mensaje_Error = Mensaje_Error + "<br>";
                Mensaje_Error = Mensaje_Error + "+ Introducir un porcentaje de descuento menor o igual al 100.";
                Validacion = false;
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error = Mensaje_Error + "<br>";
            Mensaje_Error = Mensaje_Error + "+ Introducir un porcentaje de descuento en número entero.";
            Validacion = false;
        }
        if (Validacion == false)
        {
            Lbl_Ecabezado_Mensaje.Visible = true;
            Lbl_Error.Text = Mensaje_Error;
            Lbl_Error.Visible = true;
            Div_Contenedor_error.Visible = true;
        }
        return Validacion;
    }

    #endregion

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Descuentos_Generales_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de los Descuentos Generales 
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 13/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Descuentos_Generales_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Descuentos_Generales.SelectedIndex = (-1);
        Llenar_Descuentos_Generales(e.NewPageIndex);
        Limpiar_Catalogo();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Descuentos_Generales_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de una Caja Seleccionado para mostrarlos a detalle
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Descuentos_Generales_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Descuentos_Generales.SelectedIndex > (-1))
            {
                Limpiar_Catalogo();
                String ID_Seleccionado = Grid_Descuentos_Generales.SelectedRow.Cells[1].Text;
                Cls_Cat_Pre_Descuentos_Generales_Negocio Descuentos_Generales = new Cls_Cat_Pre_Descuentos_Generales_Negocio();
                Descuentos_Generales.P_Descuentos_Generales_Id = ID_Seleccionado;
                Descuentos_Generales = Descuentos_Generales.Consultar_Datos_Descuentos_Generales();
                Txt_id.Text = Descuentos_Generales.P_Descuentos_Generales_Id;
                Txt_Comentarios.Text = Descuentos_Generales.P_Comentarios;
                Txt_Motivo.Text = Descuentos_Generales.P_Motivo;
                Txt_Vigencia_Desde.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Descuentos_Generales.P_Vigencia_Desde.Substring(0, 10)));
                Txt_Vigencia_hasta.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Descuentos_Generales.P_Vigencia_Hasta.Substring(0, 10)));                
                //Txt_Vigencia_Desde.Text = String.Format("{0:dd/MM/yyyy}", DateTime.ParseExact(Descuentos_Generales.P_Vigencia_Desde.Substring(0, 10), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                //Txt_Vigencia_hasta.Text = String.Format("{0:dd/MM/yyyy}", DateTime.ParseExact(Descuentos_Generales.P_Vigencia_Hasta.Substring(0, 10), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                Txt_Porcentaje_Descuento.Text = "" + Descuentos_Generales.P_Porcentaje_Descuento;
                Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Descuentos_Generales.P_Estatus));
                Cmb_Tipo_Descuento.SelectedIndex = Cmb_Tipo_Descuento.Items.IndexOf(Cmb_Tipo_Descuento.Items.FindByValue(Descuentos_Generales.P_Tipo_Impuesto));
                System.Threading.Thread.Sleep(1000);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Error.Text = "";
            Div_Contenedor_error.Visible = true;
        }
    }

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta un nuevo Descuento General
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 13/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
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
                Btn_Modificar.Visible = false;
                Cmb_Estatus.SelectedValue = "VIGENTE";
                Cmb_Estatus.Enabled = false;
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Cat_Pre_Descuentos_Generales_Negocio Descuentos_Generales = new Cls_Cat_Pre_Descuentos_Generales_Negocio();
                    Descuentos_Generales.P_Comentarios = Txt_Comentarios.Text.ToUpper();
                    Descuentos_Generales.P_Motivo = Txt_Motivo.Text.ToUpper();
                    Descuentos_Generales.P_Porcentaje_Descuento = Convert.ToInt32(Txt_Porcentaje_Descuento.Text);
                    Descuentos_Generales.P_Vigencia_Desde = Txt_Vigencia_Desde.Text.Trim();
                    Descuentos_Generales.P_Vigencia_Hasta = Txt_Vigencia_hasta.Text.Trim();
                    Descuentos_Generales.P_Estatus = Cmb_Estatus.SelectedItem.Value.ToUpper();
                    Descuentos_Generales.P_Tipo_Impuesto = Cmb_Tipo_Descuento.SelectedItem.Value.ToUpper();
                    Descuentos_Generales.Alta_Descuentos_Generales();
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Descuentos_Generales(Grid_Descuentos_Generales.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Descuentos Generales", "alert('Alta de Descuento General Exitosa');", true);
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
            Lbl_Error.Text = "";
            Div_Contenedor_error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de una Caja
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/Junio/2011 
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
                if (Grid_Descuentos_Generales.Rows.Count > 0 && Grid_Descuentos_Generales.SelectedIndex > (-1))
                {
                    Btn_Modificar.AlternateText = "Actualizar";
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el registro que se quiere Modificar";
                    Lbl_Error.Text = "";
                    Div_Contenedor_error.Visible = true;
                }
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Cat_Pre_Descuentos_Generales_Negocio Descuentos_Generales = new Cls_Cat_Pre_Descuentos_Generales_Negocio();
                    Descuentos_Generales.P_Descuentos_Generales_Id = Txt_id.Text.Trim();
                    Descuentos_Generales.P_Comentarios = Txt_Comentarios.Text.ToUpper();
                    Descuentos_Generales.P_Motivo = Txt_Motivo.Text.ToUpper();
                    Descuentos_Generales.P_Porcentaje_Descuento = Convert.ToInt32(Txt_Porcentaje_Descuento.Text);
                    Descuentos_Generales.P_Vigencia_Desde = Txt_Vigencia_Desde.Text.Trim();
                    Descuentos_Generales.P_Vigencia_Hasta = Txt_Vigencia_hasta.Text.Trim();
                    Descuentos_Generales.P_Estatus = Cmb_Estatus.SelectedItem.Value.ToUpper();
                    Descuentos_Generales.P_Tipo_Impuesto = Cmb_Tipo_Descuento.SelectedItem.Value.ToUpper();
                    Descuentos_Generales.Modificar_Descuentos_Generales();
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Descuentos_Generales(Grid_Descuentos_Generales.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Descuentos Generales", "alert('Actualización de Descuento General Exitosa');", true);
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
            Lbl_Error.Text = "";
            Div_Contenedor_error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Descuentos_Generales_Click
    ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 23/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Descuentos_Generales_Click(object sender, ImageClickEventArgs e)
    {
        Grid_Descuentos_Generales.SelectedIndex = (-1);
        Llenar_Descuentos_Generales(0);
        Limpiar_Catalogo();
        if (Grid_Descuentos_Generales.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
        {
            Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Identificador \"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
            Lbl_Error.Text = "(Se cargaron todos los Descuentos Generales almacenados)";
            Div_Contenedor_error.Visible = true;
            Txt_Busqueda.Text = "";
            Llenar_Descuentos_Generales(0);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 02/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click1(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Configuracion_Formulario(true);
            Limpiar_Catalogo();
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
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
            Botones.Add(Btn_Busqueda);
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
