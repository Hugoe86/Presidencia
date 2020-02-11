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
using Presidencia.Catalogo_Rangos_De_Descuentos_Por_Rol.Negocio;


public partial class paginas_Predial_Frm_Cat_Pre_Rangos_De_Descuento_Por_Rol : System.Web.UI.Page
{
    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load.
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página.
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 11/Julio/2011 
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
                Configuracion_Acceso("Frm_Cat_Pre_Rangos_De_Descuento_Por_Rol.aspx");
                Configuracion_Formulario(true);
                Llenar_Rangos_De_Descuento_Por_Rol(0);
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
    ///FECHA_CREO: 11/Julio/2011 
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
            Cmb_Tipo.SelectedIndex = (0);
        }
        Btn_Nuevo.AlternateText = "Nuevo";
        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        Btn_Modificar.Visible = true;
        Btn_Modificar.AlternateText = "Modificar";
        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        Txt_Comentarios.Enabled = !estatus;
        Txt_Porcentaje_Maximo.Enabled = !estatus;
        Cmb_Estatus.Enabled = !estatus;
        Cmb_Tipo.Enabled = !estatus;
        Txt_Nombre_Empleado.Enabled = !estatus;
        Grid_Rango_De_Descuentos_Por_Rol.Enabled = estatus;
        Grid_Rango_De_Descuentos_Por_Rol.SelectedIndex = (-1);
        Btn_Busqueda.Enabled = estatus;
        Txt_Busqueda.Enabled = estatus;
        Btn_Mostrar_Popup_Busqueda.Enabled = !estatus;
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
        Txt_Porcentaje_Maximo.Text = "";
        Txt_id.Text = "";
        Txt_Busqueda.Text = "";
        Txt_Nombre_Empleado.Text = "";
        Txt_Id_Empleado.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
        Cmb_Tipo.SelectedIndex = 0;
    }

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Rangos_De_Descuento_Por_Rol
    ///DESCRIPCIÓN: Llena la tabla de Rangos de descuentos por rol
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 11/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Rangos_De_Descuento_Por_Rol(int Pagina)
    {
        try
        {
            Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio rango = new Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio();
            rango.P_Filtro = Txt_Busqueda.Text.Trim().ToUpper();
            Grid_Rango_De_Descuentos_Por_Rol.Columns[1].Visible = true;
            Grid_Rango_De_Descuentos_Por_Rol.DataSource = rango.Consultar_Rangos_De_Descuento_Por_Rol();
            Grid_Rango_De_Descuentos_Por_Rol.PageIndex = Pagina;
            Grid_Rango_De_Descuentos_Por_Rol.DataBind();
            Grid_Rango_De_Descuentos_Por_Rol.Columns[1].Visible = false;
        }
        catch (Exception Ex)
        {
            Div_Contenedor_error.Visible = true;
            Lbl_Error.Visible = true;
            Lbl_Ecabezado_Mensaje.Visible = true;
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
        }
    }
    
    private void Llenar_Empleados(int Pagina)
    {
        try
        {
            Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio rango = new Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio();
            if (Txt_Busqueda_No_Empleado.Text.Trim().Length != 0)
            {
                rango.P_Filtro_Dinamico = rango.P_Filtro_Dinamico + Cat_Empleados.Campo_No_Empleado + " LIKE '%" + Txt_Busqueda_No_Empleado.Text.ToUpper().Trim() + "%'";
                if (Txt_Busqueda_RFC.Text.Trim().Length != 0)
                {
                    rango.P_Filtro_Dinamico = rango.P_Filtro_Dinamico + " OR ";
                }
            }
            if (Txt_Busqueda_RFC.Text.Trim().Length != 0)
            {
                rango.P_Filtro_Dinamico = rango.P_Filtro_Dinamico + Cat_Empleados.Campo_RFC + " LIKE '%" + Txt_Busqueda_RFC.Text.ToUpper().Trim() + "%'";
                if (Txt_Busqueda_Nombre_Empleado.Text.Trim().Length != 0)
                {
                    rango.P_Filtro_Dinamico = rango.P_Filtro_Dinamico + " OR ";
                }
            }
            if (Txt_Busqueda_Nombre_Empleado.Text.Trim().Length != 0)
            {
                rango.P_Filtro_Dinamico = rango.P_Filtro_Dinamico + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" + Cat_Empleados.Campo_Apellido_Materno + " LIKE '%" + Txt_Busqueda_Nombre_Empleado.Text.ToUpper() + "%'";
            }
            Grid_Empleados.Columns[1].Visible = true;
            Grid_Empleados.DataSource = rango.Consultar_Empleados();
            Grid_Empleados.PageIndex = Pagina;
            Grid_Empleados.DataBind();
            Grid_Empleados.Columns[1].Visible = false;
            Cmb_Estatus.SelectedValue = "VIGENTE";
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
        if (Txt_Nombre_Empleado.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccione un Empleado.";
            Validacion = false;
        }
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
            Validacion = false;
        }
        if (Cmb_Tipo.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Tipos.";
            Validacion = false;
        }
        if (Txt_Porcentaje_Maximo.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el porcentaje de descuento.";
            Validacion = false;
        }
        try
        {
            int num_caja = Convert.ToInt32(Txt_Porcentaje_Maximo.Text);
        }
        catch (Exception ex)
        {
            Mensaje_Error = Mensaje_Error + "<br>";
            Mensaje_Error = Mensaje_Error + "+ Introducir un Porcentaje de descuento.";
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Rangos_De_Descuentos_Por_Rol_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de los Rangos de Descuentos por rol 
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 11/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Rangos_De_Descuentos_Por_Rol_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Rango_De_Descuentos_Por_Rol.SelectedIndex = (-1);
        Llenar_Rangos_De_Descuento_Por_Rol(e.NewPageIndex);
        Limpiar_Catalogo();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Rangos_De_Descuentos_Por_Rol_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de un Rango de Descuentos por rol Seleccionado para mostrarlo a detalle
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 11/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Rangos_De_Descuentos_Por_Rol_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Rango_De_Descuentos_Por_Rol.SelectedIndex > (-1))
            {
                Limpiar_Catalogo();
                String ID_Seleccionado = Grid_Rango_De_Descuentos_Por_Rol.SelectedRow.Cells[1].Text;
                Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio rango = new Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio();
                rango.P_Rangos_De_Descuento_Por_Rol_Id = ID_Seleccionado;
                rango = rango.Consultar_Datos_Rangos_De_Descuento_Por_Rol();
                Txt_id.Text = rango.P_Rangos_De_Descuento_Por_Rol_Id;
                Txt_Nombre_Empleado.Text = rango.P_Nombre_Empleado;
                Txt_Id_Empleado.Text = rango.P_Empleado_Id;
                Txt_Comentarios.Text = rango.P_Comentarios;
                Txt_Porcentaje_Maximo.Text = ""+rango.P_Porcentaje_Maximo;
                Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(rango.P_Estatus));
                Cmb_Tipo.SelectedIndex = Cmb_Tipo.Items.IndexOf(Cmb_Tipo.Items.FindByValue(rango.P_Tipo));
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Empleados_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de los Rangos de Descuentos por rol 
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 08/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Empleados_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Empleados.SelectedIndex = (-1);
        Llenar_Empleados(e.NewPageIndex);
        Limpiar_Catalogo();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Empleados_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de un Empleado seleccionado para mostrarlo a detalle
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 08/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Empleados_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Empleados.SelectedIndex > (-1))
            {
                Limpiar_Catalogo();
                String ID_Seleccionado = Grid_Empleados.SelectedRow.Cells[1].Text;
                Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio rango = new Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio();
                rango.P_Empleado_Id = ID_Seleccionado;
                rango = rango.Consultar_Datos_Empleados();
                Txt_Id_Empleado.Text = rango.P_Empleado_Id;
                Txt_Nombre_Empleado.Text = rango.P_Nombre_Empleado;
                Upd_Panel_Padron_Predios.Update();
                Cmb_Estatus.SelectedValue = "VIGENTE";
                Mpe_Busqueda_Empleados.Hide();
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

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Cerrar_Ventana_Click
    /// DESCRIPCION : Cierra la ventana de busqueda de empleados.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 13/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Cerrar_Ventana_Click(object sender, ImageClickEventArgs e)
    {
        Txt_Busqueda_No_Empleado.Text = "";
        Txt_Busqueda_Nombre_Empleado.Text = "";
        Txt_Busqueda_RFC.Text = "";
        Grid_Empleados.DataSource = null;
        Grid_Empleados.DataBind();
        Mpe_Busqueda_Empleados.Hide();
    }

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta un nuevo Rango de descuento por rol
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 12/Julio/2011 
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
                Cmb_Estatus.Enabled = false;
                Cmb_Estatus.SelectedValue = "VIGENTE";
                Txt_Nombre_Empleado.Enabled = false;
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio Rango = new Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio();
                    Rango.P_Comentarios = Txt_Comentarios.Text.Trim().ToUpper();
                    Rango.P_Estatus = Cmb_Estatus.SelectedItem.Value.ToUpper();
                    Rango.P_Tipo = Cmb_Tipo.SelectedItem.Value.ToUpper();
                    Rango.P_Porcentaje_Maximo = Convert.ToInt32(Txt_Porcentaje_Maximo.Text.ToUpper());
                    Rango.P_Empleado_Id = Txt_Id_Empleado.Text;
                    Rango.Alta_Rangos_De_Descuento_Por_Rol();
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Rangos_De_Descuento_Por_Rol(Grid_Rango_De_Descuentos_Por_Rol.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Rangos de descuentos por rol", "alert('Alta de descuento por Rol exitoso');", true);
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click1
    ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un rango de descuentos por rol
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 12/Julio/2011 
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
                if (Grid_Rango_De_Descuentos_Por_Rol.Rows.Count > 0 && Grid_Rango_De_Descuentos_Por_Rol.SelectedIndex > (-1))
                {
                    Btn_Modificar.AlternateText = "Actualizar";
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                    Txt_Nombre_Empleado.Enabled = false;
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
                    Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio Rango = new Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio();
                    Rango.P_Comentarios = Txt_Comentarios.Text.Trim().ToUpper();
                    Rango.P_Rangos_De_Descuento_Por_Rol_Id = Txt_id.Text.ToUpper();
                    Rango.P_Estatus = Cmb_Estatus.SelectedItem.Value.ToUpper();
                    Rango.P_Tipo = Cmb_Tipo.SelectedItem.Value.ToUpper();
                    Rango.P_Porcentaje_Maximo = Convert.ToInt32(Txt_Porcentaje_Maximo.Text.ToUpper());
                    Rango.P_Empleado_Id = Txt_Id_Empleado.Text;
                    Rango.Modificar_Rangos_De_Descuento_Por_Rol();
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Rangos_De_Descuento_Por_Rol(Grid_Rango_De_Descuentos_Por_Rol.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Rangos de descuentos por rol", "alert('Actualización de Descuento por Rol Exitoso');", true);
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Rango_De_Descuento_Por_Rol_Click
    ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 23/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Rango_De_Descuento_Click(object sender, ImageClickEventArgs e)
    {
        Grid_Rango_De_Descuentos_Por_Rol.SelectedIndex = (-1);
        Llenar_Rangos_De_Descuento_Por_Rol(0);
        Limpiar_Catalogo();
        if (Grid_Rango_De_Descuentos_Por_Rol.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
        {
            Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Identificador \"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
            Lbl_Error.Text = "(Se cargaron todos los Rangos de descuentos por rol almacenados)";
            Div_Contenedor_error.Visible = true;
            Txt_Busqueda.Text = "";
            Llenar_Rangos_De_Descuento_Por_Rol(0);
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Empleados_Click
    ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 23/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Busqueda_Empleados_Click(object sender, EventArgs e)
    {
        try
        {
            Llenar_Empleados(0);
            Mpe_Busqueda_Empleados.Show();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Visible = true;
            Img_Error.Visible = true;
            Lbl_Ecabezado_Mensaje.Text = Ex.Message.ToString();
        }
    }

}
