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
using Presidencia.Catalogo_Cajas.Negocio;
using Presidencia.Catalogo_Modulos.Negocio;




public partial class paginas_Predial_Frm_Cat_Pre_Cajas : System.Web.UI.Page
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

        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Configuracion_Acceso("Frm_Cat_Pre_Cajas.aspx");
                Configuracion_Formulario(true);
                Llenar_Combo_Modulos();
                Llenar_Cajas(0);
            }
        }
        catch (Exception ex)
        {
            Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Lbl_Error.Visible = true;
            Img_Error.Visible = true;
        }
        Div_Contenedor_Error.Visible = false;
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
    ///FECHA_CREO: 22/Junio/2011 
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
            Cmb_Modulo.SelectedIndex = (0);
        }
        Btn_Nuevo.AlternateText = "Nuevo";
        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        Btn_Modificar.Visible = true;
        Btn_Modificar.AlternateText = "Modificar";
        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        Btn_Eliminar.Visible = estatus;
        Txt_Caja_Id.Enabled = !estatus;
        Txt_Clave.Enabled = !estatus;
        Txt_Comentarios.Enabled = !estatus;
        Chk_Foranea.Enabled = !estatus;
        Txt_Numero_De_Caja.Enabled = !estatus;
        Cmb_Estatus.Enabled = !estatus;
        Cmb_Modulo.Enabled = !estatus;
        Grid_Cajas.Enabled = estatus;
        Grid_Cajas.SelectedIndex = (-1);
        Btn_Busqueda.Enabled = estatus;
        Txt_Busqueda.Enabled = estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles del Formulario
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 22/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Txt_Caja_Id.Text = "";
        Txt_Clave.Text = "";
        Txt_Comentarios.Text = "";
        Txt_Numero_De_Caja.Text = "";
        Chk_Foranea.Checked = false;
        Txt_Busqueda.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
        Cmb_Modulo.SelectedIndex = 0;
    }

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Cajas
    ///DESCRIPCIÓN: Llena la tabla de Cajas
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 24/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Cajas(int Pagina)
    {
        try
        {
            Cls_Cat_Pre_Cajas_Negocio Cajas = new Cls_Cat_Pre_Cajas_Negocio();
            Cajas.P_Filtro = Txt_Busqueda.Text.Trim().ToUpper();
            Grid_Cajas.Columns[1].Visible = true;
            Grid_Cajas.DataSource = Cajas.Consultar_Caja();
            Grid_Cajas.PageIndex = Pagina;
            Grid_Cajas.DataBind();
            Grid_Cajas.Columns[1].Visible = false;
        }
        catch (Exception Ex)
        {
            Div_Contenedor_Error.Visible = true;
            Lbl_Error.Visible = true;
            Lbl_Ecabezado_Mensaje.Visible = true;
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Modulos
    ///DESCRIPCIÓN: Llena el combo de Módulos
    ///PROPIEDADES:         
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 24/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Modulos()
    {
        try
        {
            Cls_Cat_Pre_Modulos_Negocio Modulos = new Cls_Cat_Pre_Modulos_Negocio();
            DataTable tabla= Modulos.Consultar_Nombre_Modulos();
            DataRow fila= tabla.NewRow();
            fila[Cat_Pre_Modulos.Campo_Modulo_Id]="SELECCIONE";
            fila[Cat_Pre_Modulos.Campo_Descripcion]=HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            tabla.Rows.InsertAt(fila,0);
            Cmb_Modulo.DataSource = tabla;
            Cmb_Modulo.DataValueField = Cat_Pre_Modulos.Campo_Modulo_Id;
            Cmb_Modulo.DataTextField = Cat_Pre_Modulos.Campo_Descripcion;
            Cmb_Modulo.DataBind();
        }
        catch (Exception Ex)
        {
            Div_Contenedor_Error.Visible = true;
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
    ///FECHA_CREO: 23/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;        
        if (Cmb_Estatus.SelectedIndex == 0 || Cmb_Estatus.SelectedValue.Length>20)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
            Validacion = false;
        }
        if (Txt_Clave.Text.Trim().Length == 0 || Txt_Clave.Text.Trim().Length>20)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir la Clave.";
            Validacion = false;
        }
        //if (Txt_Comentarios.Text.Trim().Length == 0 || Txt_Comentarios.Text.Trim().Length>250)
        //{
        //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //    Mensaje_Error = Mensaje_Error + "+ Introducir los Comentarios.";
        //    Validacion = false;
        //}
        if (Txt_Numero_De_Caja.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el N&uacute;mero de Caja.";
            Validacion = false;
        }
        if (Cmb_Modulo.SelectedIndex ==0 || Cmb_Modulo.SelectedValue.Length>10)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el M&oacute;dulo.";
            Validacion = false;
        }
        try {
            int num_caja = Convert.ToInt32(Txt_Numero_De_Caja.Text);
        }catch(Exception ex){
            Mensaje_Error = Mensaje_Error + "<br>";
            Mensaje_Error = Mensaje_Error + "+ Introducir un N&uacute;mero de Caja.";
            Validacion = false;
        }
        if(Validacion==false){
            Lbl_Ecabezado_Mensaje.Visible = true;
            Lbl_Error.Text = Mensaje_Error;
            Lbl_Error.Visible = true;
            Div_Contenedor_Error.Visible = true;
        }
        return Validacion;
    }

    #endregion


    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Cajas_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de las Cajas 
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Cajas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Cajas.SelectedIndex = (-1);
        Llenar_Cajas(e.NewPageIndex);
        Limpiar_Catalogo();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Cajas_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de una Caja Seleccionado para mostrarlos a detalle
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Cajas_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Cajas.SelectedIndex > (-1))
            {
                Limpiar_Catalogo();
                String ID_Seleccionado = Grid_Cajas.SelectedRow.Cells[1].Text;
                Cls_Cat_Pre_Cajas_Negocio Caja = new Cls_Cat_Pre_Cajas_Negocio();
                Caja.P_Caja_ID = ID_Seleccionado;
                Caja = Caja.Consultar_Datos_Caja();
                Txt_Caja_Id.Text = Caja.P_Caja_ID;
                Txt_Clave.Text = Caja.P_Clave;
                Txt_Comentarios.Text = Caja.P_Comentarios;
                if (Caja.P_Foranea == "SI")
                {
                    Chk_Foranea.Checked = true;
                }
                else 
                {
                    Chk_Foranea.Checked = false;
                }
                Txt_Numero_De_Caja.Text = ""+Caja.P_Numero_De_Caja;
                Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Caja.P_Estatus));
                Cmb_Modulo.SelectedIndex = Cmb_Modulo.Items.IndexOf(Cmb_Modulo.Items.FindByValue(Caja.P_Modulo));
                System.Threading.Thread.Sleep(1000);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Error.Text = "";
            Div_Contenedor_Error.Visible = true;
        }
    }

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click1
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una nueva Caja
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click1(object sender, ImageClickEventArgs e)
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
            }
            else
            {                
                if (Validar_Componentes())
                {                    
                    Cls_Cat_Pre_Cajas_Negocio Caja = new Cls_Cat_Pre_Cajas_Negocio();
                    Caja.P_Caja_ID = Txt_Caja_Id.Text.Trim();
                    Caja.P_Estatus = Cmb_Estatus.SelectedItem.Value.ToUpper();
                    Caja.P_Modulo = Cmb_Modulo.SelectedItem.Value.ToUpper();
                    Caja.P_Clave = Txt_Clave.Text.ToUpper();
                    Caja.P_Comentarios = Txt_Comentarios.Text.ToUpper();
                    if (Chk_Foranea.Checked == true)
                    {
                        Caja.P_Foranea="SI";
                    }
                    else
                    {
                        Caja.P_Foranea = "NO";
                    }
                    Caja.P_Numero_De_Caja = Convert.ToInt32(Txt_Numero_De_Caja.Text);
                    Caja.Alta_Caja();
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Cajas(Grid_Cajas.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Cajas", "alert('Alta de Caja Exitosa');", true);
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
            Div_Contenedor_Error.Visible = true;            
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click1
    ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de una Caja
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click1(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Modificar.AlternateText.Equals("Modificar"))
            {
                if (Grid_Cajas.Rows.Count > 0 && Grid_Cajas.SelectedIndex > (-1))
                {
                    Btn_Modificar.AlternateText = "Actualizar";
                    Configuracion_Formulario(false);
                    Txt_Clave.Enabled = false;
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
                    Div_Contenedor_Error.Visible = true;
                }
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Cat_Pre_Cajas_Negocio Caja = new Cls_Cat_Pre_Cajas_Negocio();
                    Caja.P_Caja_ID = Txt_Caja_Id.Text;
                    Caja.P_Clave = Txt_Clave.Text.ToUpper();
                    Caja.P_Comentarios = Txt_Comentarios.Text.ToUpper();
                    Caja.P_Numero_De_Caja = Convert.ToInt32(Txt_Numero_De_Caja.Text);
                    Caja.P_Estatus = Cmb_Estatus.SelectedItem.Value.ToUpper();
                    Caja.P_Modulo = Cmb_Modulo.SelectedItem.Value.ToUpper();
                    if (Chk_Foranea.Checked == true)
                    {
                        Caja.P_Foranea = "SI";
                    }
                    else
                    {
                        Caja.P_Foranea = "NO";
                    }
                    Caja.Modificar_Caja();
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Cajas(Grid_Cajas.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Cajas", "alert('Actualización de Caja Exitosa');", true);
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
            Div_Contenedor_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Caja_Click
    ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 23/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Caja_Click(object sender, ImageClickEventArgs e)
    {
        Grid_Cajas.SelectedIndex = (-1);
        Llenar_Cajas(0);
        Limpiar_Catalogo();
        if (Grid_Cajas.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
        {
            Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Identificador \"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
            Lbl_Error.Text = "(Se cargaron todos las Cajas almacenadas)";
            Div_Contenedor_Error.Visible = true;
            Txt_Busqueda.Text = "";
            Llenar_Cajas(0);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click1
    ///DESCRIPCIÓN: Elimina una Caja de la Base de Datos
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 23/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click1(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Cajas.Rows.Count > 0 && Grid_Cajas.SelectedIndex > (-1))
            {
                Cls_Cat_Pre_Cajas_Negocio Caja = new Cls_Cat_Pre_Cajas_Negocio();
                Caja.P_Caja_ID = Grid_Cajas.SelectedRow.Cells[1].Text;
                Caja.Eliminar_Caja();
                Grid_Cajas.SelectedIndex = (-1);
                Llenar_Cajas(Grid_Cajas.PageIndex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Cajas", "alert('La Caja fue eliminada exitosamente');", true);
                Limpiar_Catalogo();
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Eliminar.";
                Lbl_Error.Text = "";
                Div_Contenedor_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Error.Text = "";
            Div_Contenedor_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click1
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
            Botones.Add(Btn_Eliminar);
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