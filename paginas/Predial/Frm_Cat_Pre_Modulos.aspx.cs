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
using Presidencia.Catalogo_Modulos.Negocio;

public partial class paginas_Predial_Frm_Cat_Pre_Modulos : System.Web.UI.Page
{
    public DataTable Dt_Datatable = new DataTable();

    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load.
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página.
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 28/Junio/2011 
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
                Configuracion_Acceso("Frm_Cat_Pre_Modulos.aspx");
                Configuracion_Formulario(true);
                Llenar_Modulos(0);
            }
        }
        catch (Exception ex)
        {
            Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
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
    ///FECHA_CREO: 28/Junio/2011 
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
            Cmd_Estatus.SelectedIndex = (0);
        }
        Btn_Nuevo.AlternateText = "Nuevo";
        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        Btn_Modificar.Visible = true;
        Btn_Modificar.AlternateText = "Modificar";
        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        Btn_Eliminar.Visible = estatus;
        Txt_Id.Enabled = !estatus;
        Txt_Clave.Enabled = !estatus;
        Txt_Descripcion.Enabled = !estatus;
        Txt_Ubicacion.Enabled = !estatus;
        Cmd_Estatus.Enabled = !estatus;
        Grid_Modulos.Enabled = estatus;
        Grid_Modulos.SelectedIndex = (-1);
        Btn_Busqueda.Enabled = estatus;
        Txt_Busqueda.Enabled = estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles del Formulario
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 28/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Txt_Id.Text = "";
        Txt_Clave.Text = "";
        Txt_Descripcion.Text = "";
        Txt_Ubicacion.Text = "";
        Txt_Busqueda.Text = "";
        Cmd_Estatus.SelectedIndex = 0;
    }

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Modulos
    ///DESCRIPCIÓN: Llena la tabla de Modulos
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 28/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Modulos(int Pagina)
    {
        try
        {
            Cls_Cat_Pre_Modulos_Negocio Modulos = new Cls_Cat_Pre_Modulos_Negocio();
            Modulos.P_Filtro = Txt_Busqueda.Text.Trim().ToUpper();
            Grid_Modulos.Columns[1].Visible = true;
            Grid_Modulos.DataSource = Modulos.Consultar_Modulo();
            Grid_Modulos.PageIndex = Pagina;
            Grid_Modulos.DataBind();
            Grid_Modulos.Columns[1].Visible = false;
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
    ///FECHA_CREO: 28/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Cmd_Estatus.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
            Validacion = false;
        }
        if (Txt_Clave.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir la Clave.";
            Validacion = false;
        }
        if (Txt_Descripcion.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir la Descripción.";
            Validacion = false;
        }
        if (Txt_Ubicacion.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir La ubicación del Módulo.";
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Modulos_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de los Modulos 
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 28/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Modulos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Modulos.SelectedIndex = (-1);
        Llenar_Modulos(e.NewPageIndex);
        Limpiar_Catalogo();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Modulos_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de un Modulo Seleccionado para mostrarlos a detalle
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 29/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Modulos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Modulos.SelectedIndex > (-1))
            {
                Limpiar_Catalogo();
                String ID_Seleccionado = Grid_Modulos.SelectedRow.Cells[1].Text;
                Cls_Cat_Pre_Modulos_Negocio Modulo = new Cls_Cat_Pre_Modulos_Negocio();
                Modulo.P_Id_Modulo = ID_Seleccionado;
                Modulo = Modulo.Consultar_Datos_Modulo();
                Txt_Id.Text = Modulo.P_Id_Modulo;
                Txt_Clave.Text = Modulo.P_Clave;
                Txt_Descripcion.Text = Modulo.P_Descripcion;
                Txt_Ubicacion.Text = Modulo.P_Ubicacion;
                Cmd_Estatus.SelectedIndex = Cmd_Estatus.Items.IndexOf(Cmd_Estatus.Items.FindByValue(Modulo.P_Estatus));
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click1
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta un nuevo Modulo
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 29/Junio/2011 
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
                    Cls_Cat_Pre_Modulos_Negocio Modulo = new Cls_Cat_Pre_Modulos_Negocio();
                    Modulo.P_Id_Modulo = Txt_Id.Text.Trim();
                    Modulo.P_Estatus = Cmd_Estatus.SelectedItem.Value.ToUpper();
                    Modulo.P_Clave = Txt_Clave.Text.ToUpper();
                    Modulo.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                    Modulo.P_Ubicacion = Txt_Ubicacion.Text.ToUpper();
                    Modulo.Alta_Modulo();
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Modulos(Grid_Modulos.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Módulos", "alert('Alta de Módulo Exitosa');", true);
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
    ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un Modulo
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 29/Junio/2011 
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
                if (Grid_Modulos.Rows.Count > 0 && Grid_Modulos.SelectedIndex > (-1))
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
                    Cls_Cat_Pre_Modulos_Negocio Modulo = new Cls_Cat_Pre_Modulos_Negocio();
                    Modulo.P_Id_Modulo = Txt_Id.Text;
                    Modulo.P_Clave = Txt_Clave.Text.ToUpper();
                    Modulo.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                    Modulo.P_Ubicacion = Txt_Ubicacion.Text.ToUpper();
                    Modulo.P_Estatus = Cmd_Estatus.SelectedItem.Value.ToUpper();
                    Modulo.Modificar_Modulo();
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Modulos(Grid_Modulos.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Módulos", "alert('Actualización de Módulo Exitosa');", true);
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Modulo_Click
    ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 29/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Modulo_Click(object sender, ImageClickEventArgs e)
    {
        Grid_Modulos.SelectedIndex = (-1);
        Llenar_Modulos(0);
        Limpiar_Catalogo();
        if (Grid_Modulos.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
        {
            Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Identificador \"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
            Lbl_Error.Text = "(Se cargaron todos los Modulos almacenados)";
            Div_Contenedor_error.Visible = true;
            Txt_Busqueda.Text = "";
            Llenar_Modulos(0);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click1
    ///DESCRIPCIÓN: Elimina un Modulo de la Base de Datos
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click1(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Modulos.Rows.Count > 0 && Grid_Modulos.SelectedIndex > (-1))
            {
                Cls_Cat_Pre_Modulos_Negocio Modulo = new Cls_Cat_Pre_Modulos_Negocio();
                Modulo.P_Id_Modulo = Grid_Modulos.SelectedRow.Cells[1].Text;
                Modulo.Eliminar_Modulo();
                Grid_Modulos.SelectedIndex = (-1);
                Llenar_Modulos(Grid_Modulos.PageIndex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Módulos", "alert('El Módulo fue eliminado exitosamente');", true);
                Limpiar_Catalogo();
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click1
    ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 29/Junio/2011 
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