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
using Presidencia.Catalogo_Colonias.Negocio;
using Presidencia.Catalogo_Tipos_Colonias.Negocio;

public partial class paginas_Predial_Frm_Cat_Pre_Colonias_Sector : System.Web.UI.Page
{

    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 16/Julio/2011 
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
                //Configuracion_Acceso("Frm_Cat_Pre_Colonias.aspx");
                Llenar_Combo_Sector("");
                Configuracion_Formulario(true);
                Llenar_Tabla_Colonias_Busqueda(0);
                Llenar_Combo_Tipos();                
            }
            Div_Contenedor_Msj_Error.Visible = false;
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
    ///FECHA_CREO: 18/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Estatus)
    {
        Btn_Modificar.Visible = true;
        Btn_Modificar.AlternateText = "Modificar";
        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        Cmb_Sector.Enabled = !Estatus;
        Grid_Colonias.Enabled = Estatus;
        Grid_Colonias.SelectedIndex = (-1);
        Btn_Buscar_Colonias.Enabled = Estatus;
        Txt_Busqueda_Colonias.Enabled = Estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Tipos
    ///DESCRIPCIÓN: Metodo que llena el Combo de Tipos con los tipos de colonias existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 18/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Tipos()
    {
        try
        {
            Cls_Cat_Pre_Colonias_Negocio Colonia = new Cls_Cat_Pre_Colonias_Negocio();
            DataTable Colonias = Colonia.Llenar_Combo_Tipos();
            DataRow fila = Colonias.NewRow();
            fila[Cat_Pre_Tipos_Colonias.Campo_Descripcion] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            fila[Cat_Pre_Tipos_Colonias.Campo_Tipo_Colonia_ID] = "SELECCIONE";
            Colonias.Rows.InsertAt(fila, 0);
            Cmb_Tipo.DataTextField = Cat_Pre_Tipos_Colonias.Campo_Descripcion;
            Cmb_Tipo.DataValueField = Cat_Pre_Tipos_Colonias.Campo_Tipo_Colonia_ID;
            Cmb_Tipo.DataSource = Colonias;
            Cmb_Tipo.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Sector
    ///DESCRIPCIÓN: Metodo que llena el Combo de Sectores
    ///PROPIEDADES:     
    ///CREO: Armando Zavala Moreno.
    ///FECHA_CREO: 25/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Sector(String Sector)
    {
        try
        {
            Cls_Cat_Pre_Colonias_Negocio Colonia = new Cls_Cat_Pre_Colonias_Negocio();
            if (Sector != "")
            {
                Colonia.P_Sector = Sector;
            }
            DataTable Colonias = Colonia.Consultar_Sectores();
            Cmb_Sector.DataTextField = Cat_Pre_Sectores.Campo_Nombre;
            Cmb_Sector.DataValueField = Cat_Pre_Sectores.Campo_Sector_ID;
            Cmb_Sector.DataSource = Colonias;
            Cmb_Sector.DataBind();
            Cmb_Sector.Items.Insert(0, new ListItem("<-- SELECCIONE -->", "0"));
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles del Formulario
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 18/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Txt_Colonia_ID.Text = "";
        Cmb_Sector.SelectedIndex = 0;
        Txt_Clave.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
        Cmb_Tipo.SelectedIndex = 0;
        Txt_Comentarios.Text = "";
        Txt_Nombre.Text = "";

    }

    #region Grid

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Colonias_Busqueda
    ///DESCRIPCIÓN: Llena la tabla de Colonias de auerdo a la busqueda introducida.
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 19/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Colonias_Busqueda(int Pagina)
    {
        try
        {
            Cls_Cat_Pre_Colonias_Negocio Colonias = new Cls_Cat_Pre_Colonias_Negocio();
            Colonias.P_Colonia_ID = Txt_Colonia_ID.Text.Trim();
            Colonias.P_Nombre = Txt_Busqueda_Colonias.Text.ToUpper().Trim();
            Grid_Colonias.DataSource = Colonias.Consultar_Nombre();
            Grid_Colonias.PageIndex = Pagina;
            Grid_Colonias.Columns[1].Visible = true;
            Grid_Colonias.Columns[5].Visible = true;
            Grid_Colonias.Columns[7].Visible = true;
            Grid_Colonias.Columns[8].Visible = true;
            Grid_Colonias.DataBind();
            Grid_Colonias.Columns[1].Visible = false;
            Grid_Colonias.Columns[5].Visible = false;
            Grid_Colonias.Columns[7].Visible = false;
            Grid_Colonias.Columns[8].Visible = false;
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Colonias_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView General de Colonias
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 19/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Colonias_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Colonias.SelectedIndex = (-1);
            Limpiar_Catalogo();
            Llenar_Tabla_Colonias_Busqueda(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Colonias_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de la Colonia Seleccionada para mostrarlos a detalle
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 19/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Colonias_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Colonias.SelectedIndex > (-1))
            {
                Txt_Colonia_ID.Text = Grid_Colonias.SelectedRow.Cells[1].Text;
                Txt_Clave.Text = Grid_Colonias.SelectedRow.Cells[2].Text;
                Txt_Nombre.Text = Grid_Colonias.SelectedRow.Cells[3].Text;
                Cmb_Tipo.SelectedIndex = Cmb_Tipo.Items.IndexOf(Cmb_Tipo.Items.FindByText(Grid_Colonias.SelectedRow.Cells[5].Text));
                Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Grid_Colonias.SelectedRow.Cells[6].Text));
                Txt_Comentarios.Text = Grid_Colonias.SelectedRow.Cells[7].Text;
                System.Threading.Thread.Sleep(1000);
                Cmb_Sector.SelectedItem.Text = Grid_Colonias.SelectedRow.Cells[4].Text;
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de una Colonia
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 19/Julio/2011 
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
                if (Grid_Colonias.Rows.Count > 0 && Grid_Colonias.SelectedIndex > (-1))
                {
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Cmb_Estatus.Enabled = false;
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
                Cls_Cat_Pre_Colonias_Negocio Colonia = new Cls_Cat_Pre_Colonias_Negocio();
                Colonia.P_Colonia_ID = Txt_Colonia_ID.Text.Trim();
                Colonia.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                Colonia.P_Sector = Cmb_Sector.SelectedValue;
                Grid_Colonias.Columns[1].Visible = true;
                Grid_Colonias.Columns[5].Visible = true;
                Grid_Colonias.Columns[7].Visible = true;
                Grid_Colonias.Columns[8].Visible = true;
                Colonia.Actualizar_Sector_Colonia();
                Grid_Colonias.Columns[1].Visible = false;
                Grid_Colonias.Columns[5].Visible = false;
                Grid_Colonias.Columns[7].Visible = false;
                Grid_Colonias.Columns[8].Visible = false;
                Configuracion_Formulario(true);
                Limpiar_Catalogo();
                Txt_Busqueda_Colonias.Text = "";
                Llenar_Tabla_Colonias_Busqueda(Grid_Colonias.PageIndex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Colonias", "alert('Actualización del Sector de la Colonia Exitosa');", true);
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Grid_Colonias.Enabled = true;
                Cmb_Sector.SelectedIndex = 0;
                Cmb_Sector.SelectedItem.Text = "<-- SELECCIONE -->";
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
    ///DESCRIPCIÓN: Llena la Tabla de Colonias con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 19/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Limpiar_Catalogo();
            Llenar_Tabla_Colonias_Busqueda(0);
            if (Grid_Colonias.Rows.Count == 0 && Txt_Busqueda_Colonias.Text.Trim().Length > 0)
            {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Concepto\"" + Txt_Busqueda_Colonias.Text + "\" no se encotrarón coincidencias";
                Lbl_Mensaje_Error.Text = "(Se cargaron  todas las Colonias almacenadas)";
                Div_Contenedor_Msj_Error.Visible = true;
                Llenar_Tabla_Colonias_Busqueda(0);
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
    ///FECHA_CREO: 19/Julio/2011 
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
                Txt_Busqueda_Colonias.Text = "";
                Llenar_Tabla_Colonias_Busqueda(0);
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Grid_Colonias.Enabled = true;
                Grid_Colonias.Visible = true;
                Cmb_Sector.SelectedIndex = 0;
                Cmb_Sector.SelectedItem.Text = "<-- SELECCIONE -->";
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
            Botones.Add(Btn_Modificar);
            Botones.Add(Btn_Buscar_Colonias);

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
