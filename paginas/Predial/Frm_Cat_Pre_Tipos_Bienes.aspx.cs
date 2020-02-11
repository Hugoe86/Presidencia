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
using Presidencia.Catalogo_Tipos_Bienes.Negocio;

public partial class paginas_Predial_Frm_Cat_Pre_Tipos_Bienes : System.Web.UI.Page
{
    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load.
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página.
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 26/Julio/2011 
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
                Configuracion_Acceso("Frm_Cat_Pre_Tipos_Bienes.aspx");
                Configuracion_Formulario(true);
                Llenar_Bienes(0);
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
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
    ///FECHA_CREO: 26/Julio/2011 
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
        }
        Btn_Nuevo.AlternateText = "Nuevo";
        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        Btn_Modificar.Visible = true;
        Btn_Modificar.AlternateText = "Modificar";
        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        Txt_Nombre.Enabled = !estatus;
        Txt_Nombre.Enabled = !estatus;
        Txt_Comentarios.Enabled = !estatus;
        Cmb_Estatus.Enabled = !estatus;
        Grid_Bienes.Enabled = estatus;
        Grid_Bienes.SelectedIndex = (-1);
        Btn_Buscar.Enabled = estatus;
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
        Txt_Comentarios.Text = "";
        Txt_Nombre.Text = "";
        Txt_Busqueda.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
    }

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Bienes
    ///DESCRIPCIÓN: Llena la tabla de Bienes
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 26/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Bienes(int Pagina)
    {
        try
        {
            Cls_Cat_Pre_Tipos_Bienes_Negocio Bienes = new Cls_Cat_Pre_Tipos_Bienes_Negocio();
            Bienes.P_Filtro = Txt_Busqueda.Text.Trim().ToUpper();
            Grid_Bienes.Columns[1].Visible = true;
            Grid_Bienes.DataSource = Bienes.Consultar_Bien();
            Grid_Bienes.PageIndex = Pagina;
            Grid_Bienes.DataBind();
            Grid_Bienes.Columns[1].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
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
        Lbl_Mensaje_Error.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
            Validacion = false;
        }
        if (Txt_Nombre.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre del Bien.";
            Validacion = false;
        }
        if (Txt_Comentarios.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir los Comentarios.";
            Validacion = false;
        }
        if (Validacion == false)
        {
            Lbl_Mensaje_Error.Text = Mensaje_Error;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
        }
        return Validacion;
    }

    #endregion

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Bienes_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de los Bienes 
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 26/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Bienes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Bienes.SelectedIndex = (-1);
        Llenar_Bienes(e.NewPageIndex);
        Limpiar_Catalogo();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Bienes_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de un Bien Seleccionado para mostrarlos a detalle
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 26/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Bienes_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Bienes.SelectedIndex > (-1))
            {
                Limpiar_Catalogo();
                String ID_Seleccionado = Grid_Bienes.SelectedRow.Cells[1].Text;
                Cls_Cat_Pre_Tipos_Bienes_Negocio Bien = new Cls_Cat_Pre_Tipos_Bienes_Negocio();
                Bien.P_Tipo_Bien_ID = ID_Seleccionado;
                Bien = Bien.Consultar_Datos_Bien();
                Txt_ID.Text = ID_Seleccionado;
                Txt_Nombre.Text = Bien.P_Nombre;
                Txt_Comentarios.Text = Bien.P_Descripcion;
                Txt_ID.Text = Bien.P_Tipo_Bien_ID;
                Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Bien.P_Estatus));
                System.Threading.Thread.Sleep(1000);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
        }
    }

    #endregion


    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click1
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta un nuevo Bien
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 26/Julio/2011 
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
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Cat_Pre_Tipos_Bienes_Negocio Bien = new Cls_Cat_Pre_Tipos_Bienes_Negocio();
                    Bien.P_Estatus = Cmb_Estatus.SelectedItem.Value.ToUpper();
                    Bien.P_Descripcion = Txt_Comentarios.Text.ToUpper();
                    Bien.P_Nombre = Txt_Nombre.Text.ToUpper();
                    Bien.Alta_Bien();
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Bienes(Grid_Bienes.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tipos de Bienes", "alert('Alta de Bien Exitosa');", true);
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click1
    ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un Bien
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 26/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click1(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Modificar.AlternateText.Equals("Modificar"))
            {
                if (Grid_Bienes.Rows.Count > 0 && Grid_Bienes.SelectedIndex > (-1))
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
                    Lbl_Mensaje_Error.Text = "Debe seleccionar el registro que se quiere Modificar";
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Visible = true;
                }
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Cat_Pre_Tipos_Bienes_Negocio Bien = new Cls_Cat_Pre_Tipos_Bienes_Negocio();
                    Bien.P_Tipo_Bien_ID = Txt_ID.Text;
                    Bien.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                    Bien.P_Descripcion = Txt_Comentarios.Text.ToUpper();
                    Bien.P_Nombre = Txt_Nombre.Text.ToUpper();
                    Bien.Modificar_Bien();
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Bienes(Grid_Bienes.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tipos de Bienes", "alert('Actualización de Bien Exitosa');", true);
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Bien_Click
    ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 26/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Bien_Click(object sender, ImageClickEventArgs e)
    {
        Grid_Bienes.SelectedIndex = (-1);
        Llenar_Bienes(0);
        Limpiar_Catalogo();
        if (Grid_Bienes.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
        {
            Lbl_Mensaje_Error.Text = "Para la Busqueda con el Identificador \"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
            Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text+"<br>"+"(Se cargaron todos las Cajas almacenadas)";
            Img_Error.Visible = true;
            Txt_Busqueda.Text = "";
            Llenar_Bienes(0);
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
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
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
