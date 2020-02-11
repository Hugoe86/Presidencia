using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Cat_Peritos_Internos.Negocio;
using System.Data;
using Presidencia.Catalogo_Rangos_De_Descuentos_Por_Rol.Negocio;
using Presidencia.Constantes;

public partial class paginas_Catastro_Frm_Cat_Cat_Peritos_Internos : System.Web.UI.Page
{
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN:
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuracion_Formulario(true);
                Llenar_Tabla_Peritos_Internos(0);
            }
        }
        catch (Exception ex)
        {
            Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Img_Error.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
        Div_Contenedor_Msj_Error.Visible = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Establece la configuración del formulario
    ///PROPIEDADES:     Enabled: Especifica si estara habilitado o no el componente
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Enabled)
    {
        Btn_Busqueda_Empleados.Enabled = Enabled;       
        Cmb_Estatus.Enabled = !Enabled;
        Btn_Busqueda_Empleados.Enabled = !Enabled;
        Txt_Busqueda.Enabled = Enabled;
        Btn_Buscar.Enabled = Enabled;
        Grid_Peritos_Externos.Enabled = Enabled;
        Btn_Mostrar_Popup_Busqueda.Enabled = !Enabled;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Campos
    ///DESCRIPCIÓN: Limpia todos los campos del formulario
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Campos()
    {
        //Txt_Apellido_Materno.Text = "";
        //Txt_Apellido_Paterno.Text = "";
        Txt_Calle.Text = "";
        Txt_Celular.Text = "";
        Txt_Ciudad.Text = "";
        Txt_Colonia.Text = "";
        Txt_Estado.Text = "";
        Txt_Nombre.Text = "";
        Txt_Password.Text = "";
        Txt_Password.Attributes.Add("value", "");
        Txt_Password_Confirma.Text = "";
        Txt_Password_Confirma.Attributes.Add("value", "");
        Txt_Telefono.Text = "";
        Txt_Usuario.Text = "";
        Txt_Busqueda.Text = "";
        Hdf_Perito_Interno.Value = "";
        Hdf_Empleado_Id.Value = "";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Establece la configuración del formulario
    ///PROPIEDADES:     Enabled: Especifica si estara habilitado o no el componente
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Peritos_Internos(int Pagina)
    {
        try
        {
            Cls_Cat_Cat_Peritos_Internos_Negocio Peritos_Internos = new Cls_Cat_Cat_Peritos_Internos_Negocio();
            DataTable Dt_Peritos_Int;
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Peritos_Internos.P_Empleado_Nombre = Txt_Busqueda.Text.ToUpper();
            }
            Dt_Peritos_Int = Peritos_Internos.Consultar_Peritos_Internos();
            Grid_Peritos_Externos.Columns[1].Visible = true;
            Grid_Peritos_Externos.Columns[2].Visible = true;
            Grid_Peritos_Externos.DataSource = Dt_Peritos_Int;
            Grid_Peritos_Externos.PageIndex = Pagina;
            Grid_Peritos_Externos.DataBind();
            Grid_Peritos_Externos.Columns[1].Visible = false;
            Grid_Peritos_Externos.Columns[2].Visible = false;
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Img_Error.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Evento del botón nuevo
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
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
                Limpiar_Campos();
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.Visible = false;
                Div_Grid_Peritos.Visible = false;
                Div_Grid_Datos_Peritos.Visible = true;
                Cmb_Estatus.SelectedIndex = 1;
            }
            else if (Validar_Componentes())
            {
                Cls_Cat_Cat_Peritos_Internos_Negocio Perito = new Cls_Cat_Cat_Peritos_Internos_Negocio();
                Perito.P_Empleado_Id = Hdf_Empleado_Id.Value;
                Perito.P_Estatus = Cmb_Estatus.SelectedValue;
                if ((Perito.Alta_Perito_Interno()))
                {
                    Div_Grid_Peritos.Visible = true;
                    //Div_Grid_Datos_Peritos.Visible = false;
                    Configuracion_Formulario(true);
                    Llenar_Tabla_Peritos_Internos(Grid_Peritos_Externos.PageIndex);
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Peritos_Externos.SelectedIndex = -1;
                    Limpiar_Campos();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Peritos Internos", "alert('Alta de Perito Interno Exitosa');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Peritos Internos", "alert('Alta de Perito Interno Errónea');", true);
                }
            }
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Evento del botón modificar
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Peritos_Externos.SelectedIndex > -1)
            {
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
                {
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                    Div_Grid_Peritos.Visible = false;
                    Div_Grid_Datos_Peritos.Visible = true;
                    Btn_Mostrar_Popup_Busqueda.Enabled = false;
                }
                else
                {
                    if (Validar_Componentes())
                    {
                        Cls_Cat_Cat_Peritos_Internos_Negocio Perito = new Cls_Cat_Cat_Peritos_Internos_Negocio();
                        Perito.P_Estatus = Cmb_Estatus.SelectedValue;
                        Perito.P_Perito_Interno_Id = Hdf_Perito_Interno.Value;
                        if ((Perito.Modificar_Perito_Interno()))
                        {
                            Div_Grid_Peritos.Visible = true;
                            //Div_Grid_Datos_Peritos.Visible = false;
                            Configuracion_Formulario(true);
                            Llenar_Tabla_Peritos_Internos(Grid_Peritos_Externos.PageIndex);
                            Btn_Modificar.AlternateText = "Modificar";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            Btn_Nuevo.Visible = true;
                            Btn_Nuevo.AlternateText = "Nuevo";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            Btn_Salir.AlternateText = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Grid_Peritos_Externos.SelectedIndex = -1;
                            Limpiar_Campos();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Peritos Internos", "alert('Actualización de Perito Interno Exitosa.');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Peritos Internos", "alert('Error al Actualizar el Perito Interno.');", true);
                        }
                    }
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione el Perito Interno a Modificar.";
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
    ///DESCRIPCIÓN: Evento del botón salir
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
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
            Limpiar_Campos();
            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Configuracion_Formulario(true);
            Llenar_Tabla_Peritos_Internos(Grid_Peritos_Externos.PageIndex);
            Grid_Peritos_Externos.SelectedIndex = -1;
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Div_Grid_Peritos.Visible = true;
            //Div_Grid_Datos_Peritos.Visible = false;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN: Evento del botón buscar
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        if (Div_Grid_Peritos.Visible == true)
            Llenar_Tabla_Peritos_Internos(0);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Calles_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Peritos_Externos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Llenar_Tabla_Peritos_Internos(e.NewPageIndex);
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Calles_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona un registro del grid y toma los datos de los mismos campos del componente
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Peritos_Externos_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Peritos_Externos.SelectedIndex > -1)
        {
            Hdf_Perito_Interno.Value = Grid_Peritos_Externos.SelectedRow.Cells[1].Text;
            Hdf_Empleado_Id.Value = Grid_Peritos_Externos.SelectedRow.Cells[2].Text;
            Cls_Cat_Cat_Peritos_Internos_Negocio Perito = new Cls_Cat_Cat_Peritos_Internos_Negocio();
            DataTable Dt_Empleado;
            Perito.P_Perito_Interno_Id = Hdf_Perito_Interno.Value;
            Perito.P_Empleado_Id = Hdf_Empleado_Id.Value;
            Dt_Empleado = Perito.Consultar_Empleados();
            //Txt_Apellido_Materno.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString();
            //Txt_Apellido_Paterno.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString();
            Txt_Calle.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Calle].ToString();
            Txt_Celular.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Celular].ToString();
            Txt_Ciudad.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Ciudad].ToString();
            Txt_Colonia.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Colonia].ToString();
            Txt_Estado.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Estado].ToString();
            Txt_Nombre.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString() + " " + Dt_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString() + " " + Dt_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString();
            Txt_Password.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Password].ToString();
            Txt_Password.Attributes.Add("value", Txt_Password.Text);
            Txt_Password_Confirma.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Password].ToString();
            Txt_Password_Confirma.Attributes.Add("value", Txt_Password_Confirma.Text);
            Txt_Telefono.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Telefono_Casa].ToString();
            Txt_Usuario.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString();
            Cmb_Estatus.SelectedValue = Grid_Peritos_Externos.SelectedRow.Cells[5].Text;
            Btn_Salir.AlternateText = "Atras";
            Div_Grid_Peritos.Visible = false;
            Div_Grid_Datos_Peritos.Visible = true;
        }
    }

    private Boolean Validar_Componentes()
    {
        String Mensaje_Error = "Error: ";
        Boolean valido = true;

        if (Hdf_Empleado_Id.Value.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Seleccione un Empleado.";
            valido = false;
        }
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Seleccione un estatus.";
            valido = false;
        }

        if (!valido)
        {
            Lbl_Ecabezado_Mensaje.Text = Mensaje_Error;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return valido;
    }

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

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llenar_Empleados
    /// DESCRIPCION : carga todos los empleados, dependiendo de la búsqueda a realizar
    /// CREO        : Miguel Angel Bedolla Moreno
    /// FECHA_CREO  : 15/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
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
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Ecabezado_Mensaje.Visible = true;
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
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
        Mpe_Busqueda_Empleados.Show();
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
                String ID_Seleccionado = Grid_Empleados.SelectedRow.Cells[1].Text;

                Cls_Cat_Cat_Peritos_Internos_Negocio Perito = new Cls_Cat_Cat_Peritos_Internos_Negocio();
                Perito.P_Empleado_Id = ID_Seleccionado;
                Perito.P_Estatus = "='VIGENTE'";
                if (Perito.Consultar_Peritos_Internos().Rows.Count == 0)
                {
                    DataTable Dt_Empleado;
                    Dt_Empleado = Perito.Consultar_Empleados();
                    Hdf_Empleado_Id.Value = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString();
                    //Txt_Apellido_Materno.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString();
                    //Txt_Apellido_Paterno.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString();
                    Txt_Calle.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Calle].ToString();
                    Txt_Celular.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Celular].ToString();
                    Txt_Ciudad.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Ciudad].ToString();
                    Txt_Colonia.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Colonia].ToString();
                    Txt_Estado.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Estado].ToString();

                    Txt_Nombre.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString() + " " + Dt_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString() + " " + Dt_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString();
                   // Txt_Nombre.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString();
                    Txt_Password.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Password].ToString();
                    Txt_Password.Attributes.Add("value", Txt_Password.Text);
                    Txt_Password_Confirma.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Password].ToString();
                    Txt_Password_Confirma.Attributes.Add("value", Txt_Password_Confirma.Text);
                    Txt_Telefono.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Telefono_Casa].ToString();
                    Txt_Usuario.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Peritos Internos", "alert('El empleado ya existe en el sistema.');", true);
                }
                Txt_Busqueda_No_Empleado.Text = "";
                Txt_Busqueda_Nombre_Empleado.Text = "";
                Txt_Busqueda_RFC.Text = "";
                Grid_Empleados.DataSource = null;
                Grid_Empleados.DataBind();
                Mpe_Busqueda_Empleados.Hide();
            }
        }
        catch (Exception Ex)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Ecabezado_Mensaje.Visible = true;
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
        }
    }

}