﻿using System;
using System.Collections;
using System.Collections.Generic;
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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Ramas.Negocio;

public partial class paginas_Predial_Frm_Cat_Pre_Ramas : System.Web.UI.Page
{
    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load.
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página.
    ///PROPIEDADES:
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 06/Julio/2011 
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
                Configuracion_Acceso("Frm_Cat_Pre_Ramas.aspx");
                Configuracion_Formulario(true);
                Llenar_Ramas(0);
            }
        }
        catch (Exception ex)
        {
            Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Lbl_Error.Visible = true;
            Img_Error.Visible = true;
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
        }
        Btn_Nuevo.AlternateText = "Nuevo";
        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        Btn_Modificar.Visible = true;
        Btn_Modificar.AlternateText = "Modificar";
        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        Txt_Clave.Enabled = !estatus;
        Txt_Descripcion.Enabled = !estatus;
        Txt_Nombre.Enabled = !estatus;
        Cmb_Estatus.Enabled = !estatus;
        Grid_Ramas.Enabled = estatus;
        Grid_Ramas.SelectedIndex = (-1);
        Btn_Busqueda.Enabled = estatus;
        Txt_Busqueda.Enabled = estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles del Formulario
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 06/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Txt_Clave.Text = "";
        Txt_Descripcion.Text = "";
        Txt_Nombre.Text = "";
        Txt_Busqueda.Text = "";
        Txt_id.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
    }

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Ramas
    ///DESCRIPCIÓN: Llena la tabla de Ramas
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 06/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Ramas(int Pagina)
    {
        try
        {
            Cls_Cat_Pre_Ramas_Negocio Ramas = new Cls_Cat_Pre_Ramas_Negocio();
            Ramas.P_Filtro = Txt_Busqueda.Text.Trim().ToUpper();
            Grid_Ramas.Columns[1].Visible = true;
            Grid_Ramas.DataSource = Ramas.Consultar_Rama();
            Grid_Ramas.PageIndex = Pagina;
            Grid_Ramas.DataBind();
            Grid_Ramas.Columns[1].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Error.Visible = true;
            Lbl_Ecabezado_Mensaje.Visible = true;
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Div_Contenedor_error.Visible = true;
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
    ///FECHA_CREO: 07/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
            Validacion = false;
        }
        if (Txt_Clave.Text.Trim().Length == 0 || Txt_Clave.Text.Trim().Length!=2)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir la Clave de dos dígitos.";
            Validacion = false;
        }
        if (Txt_Nombre.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre.";
            Validacion = false;
        }
        if (Txt_Descripcion.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; };
            Mensaje_Error = Mensaje_Error + "+ Introducir la descripci&oacute;n de la rama.";
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Ramas_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de las Ramas 
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 07/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Ramas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Ramas.SelectedIndex = (-1);
        Llenar_Ramas(e.NewPageIndex);
        Limpiar_Catalogo();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Ramas_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de una rama Seleccionado para mostrarlos a detalle
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 07/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Ramas_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Ramas.SelectedIndex > (-1))
            {
                Limpiar_Catalogo();
                String ID_Seleccionado = Grid_Ramas.SelectedRow.Cells[1].Text;
                Cls_Cat_Pre_Ramas_Negocio Rama = new Cls_Cat_Pre_Ramas_Negocio();
                Rama.P_Rama_ID = ID_Seleccionado;
                Rama = Rama.Consultar_Datos_Rama();
                Txt_id.Text = Rama.P_Rama_ID;
                Txt_Clave.Text = Rama.P_Clave;
                Txt_Descripcion.Text = Rama.P_Descripcion;
                Txt_Nombre.Text = Rama.P_Nombre;
                Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Rama.P_Estatus));
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
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una nueva Rama
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 07/Julio/2011 
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
                Btn_Eliminar.Visible = false;
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Cat_Pre_Ramas_Negocio Rama = new Cls_Cat_Pre_Ramas_Negocio();
                    Rama.P_Estatus = Cmb_Estatus.SelectedItem.Value.ToUpper();
                    Rama.P_Nombre = Txt_Nombre.Text.ToUpper();
                    Rama.P_Clave = Txt_Clave.Text.ToUpper();
                    Rama.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                    Rama.Alta_Rama();
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Ramas(Grid_Ramas.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Ramas", "alert('Alta de Rama Exitosa');", true);
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Eliminar.Visible = true;
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click1
    ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de una Caja
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/Junio/2011 
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
                if (Grid_Ramas.Rows.Count > 0 && Grid_Ramas.SelectedIndex > (-1))
                {
                    Btn_Modificar.AlternateText = "Actualizar";
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                    Btn_Eliminar.Visible = false;
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
                    Cls_Cat_Pre_Ramas_Negocio Rama = new Cls_Cat_Pre_Ramas_Negocio();
                    Rama.P_Rama_ID = Txt_id.Text;
                    Rama.P_Clave = Txt_Clave.Text.ToUpper();
                    Rama.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                    Rama.P_Estatus = Cmb_Estatus.SelectedItem.Value.ToUpper();
                    Rama.P_Nombre = Txt_Nombre.Text.ToUpper();
                    Rama.Modificar_Rama();
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Ramas(Grid_Ramas.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Ramas", "alert('Actualización de Rama Exitosa');", true);
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Eliminar.Visible = true;
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Rama_Click
    ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 07/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Rama_Click(object sender, ImageClickEventArgs e)
    {
        Grid_Ramas.SelectedIndex = (-1);
        Llenar_Ramas(0);
        Limpiar_Catalogo();
        if (Grid_Ramas.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
        {
            Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Identificador \"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
            Lbl_Error.Text = "(Se cargaron todos las Ramas almacenadas)";
            Div_Contenedor_error.Visible = true;
            Txt_Busqueda.Text = "";
            Llenar_Ramas(0);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Eliminar_Click
    ///DESCRIPCIÓN          : Elimina un Tipo_Constancia de la Base de Datos
    ///PARAMETROS          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Ramas.Rows.Count > 0 && Grid_Ramas.SelectedIndex > (-1))
            {
                Cls_Cat_Pre_Ramas_Negocio Rama = new Cls_Cat_Pre_Ramas_Negocio();
                Rama.P_Rama_ID = Grid_Ramas.SelectedRow.Cells[1].Text;
                Rama.Eliminar_Rama();
                    Grid_Ramas.SelectedIndex = (-1);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Ramas", "alert('La Rama fue Eliminado Exitosamente');", true);
                    Limpiar_Catalogo();
                    Llenar_Ramas(0);
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Eliminar.";
                Lbl_Error.Text = "";
                Div_Contenedor_error.Visible = true;
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
            Btn_Eliminar.Visible = true;
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
