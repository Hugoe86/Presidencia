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
using Presidencia.Grupos_Dependencias.Negocio;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Text;


public partial class paginas_Paginas_Generales_Frm_Cat_Grupos_Dependencias : System.Web.UI.Page
{
    ///*******************************************************************************
    /// REGION PAGE_LOAD
    ///*******************************************************************************
    #region Page_Load

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            Configurar_Formulario("Inicial");
            Habilitar_Cajas(false);
            Limpiar_Cajas();
            //llenamos el grid de Grupos_Dependencias
            Llenar_Grid_Grupo_Dependencia();
            Grid_Grupos_Dependencias.Enabled = true;
            ViewState["SortDirection"] = "ASC";
        }
    }
    #endregion

    ///*******************************************************************************
    /// REGION METODOS
    ///*******************************************************************************
    #region Metodos

    public void Configurar_Formulario(String Estatus)
    {
        switch (Estatus)
        {
            case "Inicial":
                Div_Contenido_Grupo_Dependencias.Visible = true;
                //Botones
                Btn_Eliminar.Visible = true;
                Btn_Eliminar.Enabled = true;
                //Boton Nuevo
                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Nuevo.Enabled = true;
                //Boton Modificar
                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Modificar.Enabled = false;
                //para eliminar
                Btn_Eliminar.Enabled = false;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Salir.Enabled = true;
                if (Grid_Grupos_Dependencias.Rows.Count != 0)
                    Div_Grupo_Dependencias.Visible = true;
                else
                    Div_Grupo_Dependencias.Visible = false;

                //manejo de accesos
                Configuracion_Acceso("Frm_Cat_Grupos_Dependencias.aspx");

                break;
            case "Nuevo":
                Div_Contenido_Grupo_Dependencias.Visible = true;
                //Botones
                Btn_Eliminar.Visible = false;
                Btn_Eliminar.Enabled = false;
                //Boton Nuevo
                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ToolTip = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Nuevo.Enabled = true;
                //Boton Modificar
                Btn_Modificar.Visible = false;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Modificar.Enabled = false;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Salir.Enabled = true;
                Grid_Grupos_Dependencias.Enabled = false;
                if (Grid_Grupos_Dependencias.Rows.Count != 0)
                    Div_Grupo_Dependencias.Visible = true;
                else
                    Div_Grupo_Dependencias.Visible = false;
                break;
            case "Modificar":
                Div_Contenido_Grupo_Dependencias.Visible = true;
                //Botones
                Btn_Eliminar.Visible = false;
                Btn_Eliminar.Enabled = false;
                //Boton Nuevo
                Btn_Nuevo.Visible = false;
                //Boton Modificar
                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Actualizar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                Btn_Modificar.Enabled = true;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Salir.Enabled = true;
                Grid_Grupos_Dependencias.Enabled = false;
                if (Grid_Grupos_Dependencias.Rows.Count != 0)
                    Div_Grupo_Dependencias.Visible = true;
                else
                    Div_Grupo_Dependencias.Visible = false;
                
                break;

        }
    }

    public void Llenar_Grid_Grupo_Dependencia()
    {
        Cls_Cat_Grupos_Dependencias_Negocio Grupo_Dependencia = new Cls_Cat_Grupos_Dependencias_Negocio();
        DataTable Dt_Grupo_Dependencia = Grupo_Dependencia.Consultar_Grupos_Dependencias();
        Grid_Grupos_Dependencias.DataSource = Dt_Grupo_Dependencia;
        Grid_Grupos_Dependencias.DataBind();

        if (Grid_Grupos_Dependencias.Rows.Count != 0)
            Div_Grupo_Dependencias.Visible = true;
        else
            Div_Grupo_Dependencias.Visible = false;

    }

    public void Limpiar_Cajas()
    {
        Txt_Busqueda.Text = "";
        Txt_Clave.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
        Txt_Nombre.Text = "";
        Txt_Comentarios.Text = "";
    }

    public void Habilitar_Cajas(bool Estatus)
    {
        Txt_Clave.Enabled = Estatus;
        Cmb_Estatus.Enabled = Estatus;
        Txt_Nombre.Enabled = Estatus;
        Txt_Comentarios.Enabled = Estatus;
    }

    public void Validar_Contenido_Cajas()
    {
        if (Txt_Clave.Text.Trim() == String.Empty)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "Es necesario ingresar la clave.</br>";
        }
        if (Txt_Nombre.Text.Trim() == String.Empty)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "Es necesario ingresar un nombre.</br>";
        }
        if (Txt_Comentarios.Text.Trim() == String.Empty)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "Es necesario ingresar un comentario.</br>";
        }
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "Es necesario ingresar el estatus.</br>";
        }
        
    }

    public Cls_Cat_Grupos_Dependencias_Negocio Cargar_Datos_Negocio(Cls_Cat_Grupos_Dependencias_Negocio Datos_Negocio)
    {
        if (Session["Grupo_Dependencia_ID"] != null)
        {
            Datos_Negocio.P_Grupo_Dependencia_ID = Session["Grupo_Dependencia_ID"].ToString();
        }
        else
        {
            Datos_Negocio.P_Grupo_Dependencia_ID = null;
        }
        Datos_Negocio.P_Clave = Txt_Clave.Text.Trim();
        Datos_Negocio.P_Nombre = Txt_Nombre.Text.Trim();
        Datos_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim();
        Datos_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
        
        return Datos_Negocio;
    }
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

    ///*******************************************************************************
    /// REGION GRID
    ///*******************************************************************************
    #region Grid 

    protected void Grid_Grupos_Dependencias_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Grupos_Dependencias_Negocio Datos_Negocio = new Cls_Cat_Grupos_Dependencias_Negocio();
        GridViewRow Row = Grid_Grupos_Dependencias.SelectedRow;
        Datos_Negocio.P_Grupo_Dependencia_ID = Grid_Grupos_Dependencias.SelectedDataKey["Grupo_Dependencia_ID"].ToString();
        Session["Grupo_Dependencia_ID"] = Grid_Grupos_Dependencias.SelectedDataKey["Grupo_Dependencia_ID"].ToString();
        //consultar los Grupos_Dependencia
        DataTable Dt_Grupo_Dependencia = Datos_Negocio.Consultar_Grupos_Dependencias();
        Txt_Clave.Text = Dt_Grupo_Dependencia.Rows[0]["CLAVE"].ToString().Trim();
        Txt_Nombre.Text = Dt_Grupo_Dependencia.Rows[0]["NOMBRE"].ToString().Trim();
        Txt_Comentarios.Text = Dt_Grupo_Dependencia.Rows[0]["COMENTARIOS"].ToString().Trim();
        Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Dt_Grupo_Dependencia.Rows[0]["ESTATUS"].ToString().Trim()));
        Habilitar_Cajas(false);
        Btn_Modificar.Enabled = true;
        Btn_Eliminar.Enabled = true;

    }
    protected void Grid_Grupos_Dependencias_Sorting(object sender, GridViewSortEventArgs e)
    {
        Llenar_Grid_Grupo_Dependencia();
        DataTable Dt_Grupo_Dependencia = (Grid_Grupos_Dependencias.DataSource as DataTable);

        if (Dt_Grupo_Dependencia != null)
        {
            DataView Dv_Grupo_Dependencia = new DataView(Dt_Grupo_Dependencia);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Grupo_Dependencia.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Grupo_Dependencia.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Grupos_Dependencias.DataSource = Dv_Grupo_Dependencia;
            Grid_Grupos_Dependencias.DataBind();
        }
    }
    #endregion

    ///*******************************************************************************
    /// REGION EVENTOS
    ///*******************************************************************************
    #region Eventos

    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        switch (Btn_Nuevo.ToolTip)
        {
            case "Nuevo":
                Configurar_Formulario("Nuevo");
                Limpiar_Cajas();
                Habilitar_Cajas(true);
                Session["Grupo_Dependencia_ID"] = null;
                break;
            case "Dar de Alta":

                Validar_Contenido_Cajas();
                if (Div_Contenedor_Msj_Error.Visible == false)
                {
                    Cls_Cat_Grupos_Dependencias_Negocio Grupos_Dependencias = new Cls_Cat_Grupos_Dependencias_Negocio();
                    Grupos_Dependencias = Cargar_Datos_Negocio(Grupos_Dependencias);
                    String Mensaje_Alta = Grupos_Dependencias.Alta_Grupo_Dependencia();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Grupo_Dependencia", "alert('" + Mensaje_Alta + "');", true);
                    Configurar_Formulario("Inicial");
                    Habilitar_Cajas(false);
                    Llenar_Grid_Grupo_Dependencia();
                    Grid_Grupos_Dependencias.Enabled = true;
                    Limpiar_Cajas();
                    Session["Grupo_Dependencia_ID"] = null;
                }
        break;
        }//fin switch
    }

    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        switch (Btn_Modificar.ToolTip)
        {
            case "Modificar":
                if (Txt_Clave.Text.Trim() == String.Empty)
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario Seleccionar un Grupo Dependencia";
                }
                else
                {
                    Configurar_Formulario("Modificar");
                    Habilitar_Cajas(true);
                }
                break;
            case "Actualizar":
                Validar_Contenido_Cajas();

                if (Div_Contenedor_Msj_Error.Visible == false)
                {
                    Cls_Cat_Grupos_Dependencias_Negocio Grupos_Dependencias = new Cls_Cat_Grupos_Dependencias_Negocio();
                    Grupos_Dependencias = Cargar_Datos_Negocio(Grupos_Dependencias);
                    String Mensaje_Modificar = Grupos_Dependencias.Modificar_Grupo_Dependencia();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Grupo_Dependencia", "alert('" + Mensaje_Modificar + "');", true);
                    Configurar_Formulario("Inicial");
                    Habilitar_Cajas(false);
                    Llenar_Grid_Grupo_Dependencia();
                    Grid_Grupos_Dependencias.Enabled = true;
                    Limpiar_Cajas();
                    Session["Grupo_Dependencia_ID"] = null;
                }
                break;
        }//Fin del switch
    }

    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        StringBuilder Mensaje_Modificado = new StringBuilder();
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        if (Txt_Clave.Text.Trim()!= String.Empty)
        {
            Cls_Cat_Grupos_Dependencias_Negocio Grupos_Dependencias = new Cls_Cat_Grupos_Dependencias_Negocio();
            Cmb_Estatus.SelectedIndex = 2;
            Grupos_Dependencias = Cargar_Datos_Negocio(Grupos_Dependencias);
            String Mensaje_Eliminar = Grupos_Dependencias.Modificar_Grupo_Dependencia();
            //para cambiar el mensaje de confirmacion de la acción de modificar
            Mensaje_Modificado.Append("" + Mensaje_Eliminar);
            Mensaje_Modificado.Remove(3,30);
            Mensaje_Modificado.Insert(3, "cambio Satisfactoriamente el estatus a INACTIVO al");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Grupo_Dependencia", "alert('" + Mensaje_Modificado + "');", true);
            Configurar_Formulario("Inicial");
            Habilitar_Cajas(false);
            Limpiar_Cajas();
            Session["Grupo_Dependencia_ID"] = null;
            Llenar_Grid_Grupo_Dependencia();
        }
        else
        {
            Lbl_Mensaje_Error.Text = "Seleccionar un Grupo Dependencia antes de eliminar";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        switch (Btn_Salir.ToolTip)
        { 
            case "Cancelar":
                Configurar_Formulario("Inicial");
                Limpiar_Cajas();
                Habilitar_Cajas(false);
                Session["Grupo_Dependencia_ID"] = null;
                Grid_Grupos_Dependencias.Enabled = true;
                break;
            case "Inicio":
                if (Div_Grupo_Dependencias.Visible == false)
                {
                    Configurar_Formulario("Inicial");
                    Habilitar_Cajas(false);
                    Limpiar_Cajas();
                    //llenamos el grid de Grupos_Dependencias
                    Llenar_Grid_Grupo_Dependencia();
                    Grid_Grupos_Dependencias.Enabled = true;
                }
                else
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    Limpiar_Cajas();
                    Habilitar_Cajas(false);
                    
                }
                break;

        }//fin Switch

    }

    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        if (Txt_Busqueda.Text.Trim() == String.Empty)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Es necesario Ingresar la Clave.</br>";
        }
        else
        {
            Cls_Cat_Grupos_Dependencias_Negocio Grupo_Dependencia = new Cls_Cat_Grupos_Dependencias_Negocio();
            Grupo_Dependencia.P_Clave = Txt_Busqueda.Text.Trim().ToUpper();
            DataTable Dt_Grupo_Dependencia = Grupo_Dependencia.Consultar_Grupos_Dependencias();
            Grid_Grupos_Dependencias.DataSource = Dt_Grupo_Dependencia;
            Grid_Grupos_Dependencias.DataBind();

            if (Grid_Grupos_Dependencias.Rows.Count != 0)
                Div_Grupo_Dependencias.Visible = true;
            else
            {
                Div_Grupo_Dependencias.Visible = false;
                Lbl_Mensaje_Error.Text = "No se encontraron datos.";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

    }

    #endregion

    
}
