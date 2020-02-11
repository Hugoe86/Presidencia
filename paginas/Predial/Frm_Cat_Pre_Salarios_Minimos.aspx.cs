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
using Presidencia.Catalogo_Salarios_Minimos.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;

public partial class paginas_Predial_Cat_Pre_Salarios_Minimos : System.Web.UI.Page
{

    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load.
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página.
    ///PROPIEDADES:
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 20/Julio/2011 
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
                //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                Configuracion_Acceso("Frm_Cat_Pre_Salarios_Minimos.aspx");
                Configuracion_Formulario(true);
                Llenar_Salarios(0);
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
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
    ///FECHA_CREO: 20/Julio/2011 
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
        Btn_Eliminar.Visible = estatus;
        Txt_Anio.Enabled = !estatus;
        Txt_Monto.Enabled = !estatus;
        Cmb_Estatus.Enabled = !estatus;
        Grid_Salarios.Enabled = estatus;
        Grid_Salarios.SelectedIndex = (-1);
        Btn_Buscar.Enabled = estatus;
        Txt_Busqueda.Enabled = estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles del Formulario
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 20/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Txt_Busqueda.Text = "";
        Txt_Id.Text = "";
        Txt_Anio.Text = "";
        Txt_Monto.Text = "";
        Cmb_Estatus.SelectedIndex = (0);
    }

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Salarios
    ///DESCRIPCIÓN: Llena la tabla de Salarios
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 20/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Salarios(int Pagina)
    {
        try
        {
            Cls_Cat_Pre_Salarios_Minimos_Negocio Salarios = new Cls_Cat_Pre_Salarios_Minimos_Negocio();
            Salarios.P_Filtro = Txt_Busqueda.Text.Trim().ToUpper();
            Grid_Salarios.Columns[1].Visible = true;
            Grid_Salarios.DataSource = Salarios.Consultar_Salarios();
            Grid_Salarios.PageIndex = Pagina;
            Grid_Salarios.DataBind();
            Grid_Salarios.Columns[1].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
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
    ///FECHA_CREO: 15/Julio/2011
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
        if (Txt_Anio.Text.Trim().Length == 0 || Txt_Anio.Text.Length != 4)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el año.";
            Validacion = false;
        }
        else
        {
            Cls_Cat_Pre_Salarios_Minimos_Negocio salario = new Cls_Cat_Pre_Salarios_Minimos_Negocio();
            salario.P_Anio = Txt_Anio.Text;
            try
            {
                if (!Btn_Modificar.AlternateText.Equals("Actualizar"))
                {
                    if (salario.Consultar_Anio())
                    {
                        if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                        Mensaje_Error = Mensaje_Error + "+ El año que introducio ya tiene un salario minimo.";
                        Validacion = false;
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
        if (Txt_Monto.Text.Trim().Length==0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir un Monto.";
            Validacion = false;
        }
        if(Txt_Monto.Text.Length>0){
            try
            {
                double c= Convert.ToDouble(Txt_Monto.Text);
                String Ayudante = Txt_Monto.Text.Substring((Txt_Monto.Text.Length-3),3);
                if (!Ayudante.StartsWith(".")) 
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir un Monto válido con DOS decimales.";
                    Validacion = false;
                }
            }
            catch (Exception e)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir un Monto válido.";
                Validacion = false;
            }
        }
        if (Validacion == false)
        {
            Lbl_Mensaje_Error.Text = Mensaje_Error;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
        return Validacion;
    }

    #endregion

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Salarios_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de los Salarios Minimos 
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 20/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Salarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Salarios.SelectedIndex = (-1);
        Llenar_Salarios(e.NewPageIndex);
        Limpiar_Catalogo();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Salarios_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de un Salario minimo seleccionado para mostrarlo a detalle
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 20/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Salarios_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Salarios.SelectedIndex > (-1))
            {
                Limpiar_Catalogo();
                String ID_Seleccionado = Grid_Salarios.SelectedRow.Cells[1].Text;
                Cls_Cat_Pre_Salarios_Minimos_Negocio Salario = new Cls_Cat_Pre_Salarios_Minimos_Negocio();
                Salario.P_Salario_ID = ID_Seleccionado;
                Salario = Salario.Consultar_Datos_Salario();
                Txt_Id.Text = Salario.P_Salario_ID;
                Txt_Anio.Text = Salario.P_Anio;
                Txt_Monto.Text = Salario.P_Monto;
                Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Salario.P_Estatus));
                System.Threading.Thread.Sleep(1000);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta un nuevo Salario Minimo
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 20/Julio/2011 
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
                Cmb_Estatus.SelectedIndex = 1;
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Cat_Pre_Salarios_Minimos_Negocio Salario = new Cls_Cat_Pre_Salarios_Minimos_Negocio();
                    Salario.P_Estatus = Cmb_Estatus.SelectedItem.Value.ToUpper();
                    Salario.P_Anio = Txt_Anio.Text.ToUpper();
                    Salario.P_Monto = Txt_Monto.Text.ToUpper();
                    Salario.Alta_Salario();
                    Salario.Cambiar_Estatus();
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Salarios(Grid_Salarios.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Salarios Minimos", "alert('Alta de Salario minimo Exitoso');", true);
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
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
    ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un Salario Minimo
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 20/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Modificar.AlternateText.Equals("Modificar"))
            {
                if (Grid_Salarios.Rows.Count > 0 && Grid_Salarios.SelectedIndex > (-1))
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
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Debe seleccionar el registro que quiere modificar.";
                }
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Cat_Pre_Salarios_Minimos_Negocio Salario = new Cls_Cat_Pre_Salarios_Minimos_Negocio();
                    Salario.P_Estatus = Cmb_Estatus.SelectedItem.Value.ToUpper();
                    Cls_Ope_Pre_Parametros_Negocio Parametros = new Cls_Ope_Pre_Parametros_Negocio();
                    //if (Convert.ToInt32(Txt_Anio.Text) > Parametros.Consultar_Anio_Corriente() && Cmb_Estatus.SelectedValue=="BAJA")
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Salarios Minimos", "alert('Actualización de Salario minimo Exitoso');", true);
                    //}
                    Salario.P_Anio = Txt_Anio.Text.ToUpper();
                    Salario.P_Monto = Txt_Monto.Text.ToUpper();
                    Salario.P_Salario_ID = Txt_Id.Text.ToUpper();
                    Salario.Modificar_Salario();
                    Salario.Cambiar_Estatus();
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Salarios(Grid_Salarios.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Salarios Minimos", "alert('Actualización de Salario minimo Exitoso');", true);
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: Elimina un Salario Minimo de la Base de Datos
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 20/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Salarios.Rows.Count > 0 && Grid_Salarios.SelectedIndex > (-1))
            {
                Cls_Ope_Pre_Parametros_Negocio Parametros = new Cls_Ope_Pre_Parametros_Negocio();
                if (Convert.ToInt32(Txt_Anio.Text) > Parametros.Consultar_Anio_Corriente())
                {
                    Cls_Cat_Pre_Salarios_Minimos_Negocio Salario = new Cls_Cat_Pre_Salarios_Minimos_Negocio();
                    Salario.P_Salario_ID = Grid_Salarios.SelectedRow.Cells[1].Text;
                    Salario.Eliminar_Salario();
                    Grid_Salarios.SelectedIndex = (-1);
                    Llenar_Salarios(Grid_Salarios.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Salarios Minimos", "alert('El Salario minimo fue eliminado exitosamente');", true);
                    Limpiar_Catalogo();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Salarios Minimos", "alert('El Salario minimo no puede eliminarse ya que se encuentra en uso.');", true);
                }
            }
            else
            {
                Lbl_Mensaje_Error.Text = "Debe seleccionar el Registro que se desea Eliminar.";
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Salarios_Click
    ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 15/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Salarios_Click(object sender, ImageClickEventArgs e)
    {
        Grid_Salarios.SelectedIndex = (-1);
        Llenar_Salarios(0);
        Limpiar_Catalogo();
        if (Grid_Salarios.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
        {
            Lbl_Mensaje_Error.Text = "Para la Busqueda con el Identificador \"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Txt_Busqueda.Text = "";
            Llenar_Salarios(0);
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
            Botones.Add(Btn_Eliminar);
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
