using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Catalogo_Cat_Peritos_Internos.Negocio;
using System.Data;
using Presidencia.Constantes;
using Presidencia.Operacion_Cat_Avaluo_Urbano_Av.Negocio;
using Presidencia.Sessiones;

public partial class paginas_Catastro_Frm_Ope_Cat_Asignacion_Avaluos_Urbanos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuracion_Formulario(true);
                Llenar_Tabla_Avaluos_Urbanos_Asignados(0);
            }
        }
        catch (Exception ex)
        {
            Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
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
        Btn_Mostrar_Busqueda_Avanzada_Tasas.Enabled = !Enabled;
        Btn_Mostrar_Busqueda_Avanzada_Peritos_Internos.Enabled = !Enabled;
        Grid_Avaluos_Asignados.Enabled = Enabled;
        Txt_Busqueda.Enabled = Enabled;
        Btn_Buscar.Enabled = Enabled;
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
            Peritos_Internos.P_Empleado_Nombre = Txt_Busqueda_Nombre.Text.ToUpper();
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
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Calidad
    ///DESCRIPCIÓN: Llena la tabla de los datos de calidad
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 07/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Avaluos_Urbanos(int Pagina)
    {
        try
        {
            Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio Avaluo_Urb = new Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio();
            Avaluo_Urb.P_Folio = Txt_Busqueda_Avaluo.Text.Trim();
            Grid_Avaluos_Urbanos.Columns[1].Visible = true;
            Grid_Avaluos_Urbanos.Columns[2].Visible = true;
            Grid_Avaluos_Urbanos.DataSource = Avaluo_Urb.Consultar_Avaluo_Urbano_Av();
            Grid_Avaluos_Urbanos.PageIndex = Pagina;
            Grid_Avaluos_Urbanos.DataBind();
            Grid_Avaluos_Urbanos.Columns[1].Visible = false;
            Grid_Avaluos_Urbanos.Columns[2].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Avaluos_Urbanos_Asignados
    ///DESCRIPCIÓN: Llena la tabla de los Avalúos Urbanos asignados
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 07/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Avaluos_Urbanos_Asignados(int Pagina)
    {
        try
        {
            Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio Avaluo_Urb = new Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio();
            Avaluo_Urb.P_Folio = Txt_Busqueda_Avaluo.Text.Trim();
            Grid_Avaluos_Asignados.Columns[1].Visible = true;
            Grid_Avaluos_Asignados.Columns[2].Visible = true;
            Grid_Avaluos_Asignados.Columns[4].Visible = true;
            Grid_Avaluos_Asignados.DataSource = Avaluo_Urb.Consultar_Avaluo_Urbano_Asignados();
            Grid_Avaluos_Asignados.PageIndex = Pagina;
            Grid_Avaluos_Asignados.DataBind();
            Grid_Avaluos_Asignados.Columns[1].Visible = false;
            Grid_Avaluos_Asignados.Columns[2].Visible = false;
            Grid_Avaluos_Asignados.Columns[4].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Avaluos_Asignados_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Avaluos_Asignados_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Llenar_Tabla_Avaluos_Urbanos_Asignados(e.NewPageIndex);
    }

    protected void Grid_Avaluos_Asignados_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Avaluos_Asignados.SelectedIndex > -1)
        {
            Hdf_Perito_Interno_Id.Value = Grid_Avaluos_Asignados.SelectedRow.Cells[4].Text;
            Hdf_No_Avaluo.Value = Grid_Avaluos_Asignados.SelectedRow.Cells[1].Text;
            Hdf_Anio_Avaluo.Value = Grid_Avaluos_Asignados.SelectedRow.Cells[2].Text;
            Txt_Avaluo.Text = Grid_Avaluos_Asignados.SelectedRow.Cells[3].Text;
            Txt_Perito_Interno.Text = Grid_Avaluos_Asignados.SelectedRow.Cells[5].Text;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Avaluos_Urbanos_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Avaluos_Urbanos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Llenar_Tabla_Avaluos_Urbanos(e.NewPageIndex);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Avaluos_Urbanos_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona un registro del grid y toma los datos de los mismos campos del componente
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Avaluos_Urbanos_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Avaluos_Urbanos.SelectedIndex > -1)
        {
            DataTable Dt_Avaluo;
            Hdf_Anio_Avaluo.Value = Grid_Avaluos_Urbanos.SelectedRow.Cells[2].Text;
            Hdf_No_Avaluo.Value = Grid_Avaluos_Urbanos.SelectedRow.Cells[1].Text;
            Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio Aval_Urb = new Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio();
            Aval_Urb.P_No_Avaluo = Hdf_No_Avaluo.Value;
            Aval_Urb.P_Anio_Avaluo = Hdf_Anio_Avaluo.Value;
            Dt_Avaluo = Aval_Urb.Consultar_Avaluo_Urbano_Av();
            Cargar_Datos_Avaluo(Dt_Avaluo);
            Txt_Busqueda_Avaluo.Text = "";
            Grid_Avaluos_Urbanos.SelectedIndex = -1;
            Mpe_Avaluos.Hide();
        }
    }

    private void Cargar_Datos_Avaluo(DataTable Dt_Avaluo)
    {
        if (Dt_Avaluo.Rows.Count > 0)
        {
            Txt_Avaluo.Text = Dt_Avaluo.Rows[0]["AVALUO"].ToString();
            Hdf_No_Avaluo.Value = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_No_Avaluo].ToString();
            Hdf_Anio_Avaluo.Value = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Anio_Avaluo].ToString();
        }
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
            Hdf_Perito_Interno_Id.Value = Grid_Peritos_Externos.SelectedRow.Cells[1].Text;
            Hdf_Empleado_Id.Value = Grid_Peritos_Externos.SelectedRow.Cells[2].Text;
            Cls_Cat_Cat_Peritos_Internos_Negocio Perito = new Cls_Cat_Cat_Peritos_Internos_Negocio();
            DataTable Dt_Empleado;
            Perito.P_Perito_Interno_Id = Hdf_Perito_Interno_Id.Value;
            Perito.P_Empleado_Id = Hdf_Empleado_Id.Value;
            Dt_Empleado = Perito.Consultar_Empleados();
            //Txt_Apellido_Materno.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString();
            //Txt_Apellido_Paterno.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString();
            //Txt_Calle.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Calle].ToString();
            //Txt_Celular.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Celular].ToString();
            //Txt_Ciudad.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Ciudad].ToString();
            //Txt_Colonia.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Colonia].ToString();
            //Txt_Estado.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Estado].ToString();
            Txt_Perito_Interno.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString() + " " + Dt_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString() + " " + Dt_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString();
            //Txt_Password.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Password].ToString();
            //Txt_Password.Attributes.Add("value", Txt_Password.Text);
            //Txt_Password_Confirma.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Password].ToString();
            //Txt_Password_Confirma.Attributes.Add("value", Txt_Password_Confirma.Text);
            //Txt_Telefono.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Telefono_Casa].ToString();
            //Txt_Usuario.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString();
            //Cmb_Estatus.SelectedValue = Grid_Peritos_Externos.SelectedRow.Cells[5].Text;
            Txt_Busqueda_Nombre.Text = "";
            Grid_Peritos_Externos.SelectedIndex = -1;
            Mpe_Busqueda_Peritos_Internos.Hide();
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
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.Visible = false;
            }
            else if (Validar_Componentes())
            {
                Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio Perito = new Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio();
                Perito.P_Anio_Avaluo = Hdf_Anio_Avaluo.Value;
                Perito.P_No_Avaluo = Hdf_No_Avaluo.Value;
                Perito.P_Perito_Interno_Id = Hdf_Perito_Interno_Id.Value;
                if ((Perito.Asignar_Perito_Interno()))
                {
                    Configuracion_Formulario(true);
                    Llenar_Tabla_Avaluos_Urbanos_Asignados(Grid_Avaluos_Asignados.PageIndex);
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Nuevo.Visible = true;
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Peritos_Externos.SelectedIndex = -1;
                    Hdf_Anio_Avaluo.Value = "";
                    Hdf_Empleado_Id.Value = "";
                    Hdf_No_Avaluo.Value = "";
                    Hdf_Perito_Interno_Id.Value = "";
                    Txt_Avaluo.Text = "";
                    Txt_Perito_Interno.Text = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Asignación de Peritos Internos", "alert('El perito Interno ha sido asignado exitosamente.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Asignación de Peritos Internos", "alert('Error al intentar asignar el Perito Interno.');", true);
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
            if (Grid_Avaluos_Asignados.SelectedIndex > -1)
            {
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
                {
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                }
                else
                {
                    if (Validar_Componentes())
                    {
                        Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio Perito = new Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio();
                        Perito.P_Anio_Avaluo = Hdf_Anio_Avaluo.Value;
                        Perito.P_No_Avaluo = Hdf_No_Avaluo.Value;
                        Perito.P_Perito_Interno_Id = Hdf_Perito_Interno_Id.Value;
                        if ((Perito.Asignar_Perito_Interno()))
                        {
                            Configuracion_Formulario(true);
                            Llenar_Tabla_Avaluos_Urbanos_Asignados(Grid_Avaluos_Asignados.PageIndex);
                            Btn_Modificar.AlternateText = "Modificar";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            Btn_Nuevo.Visible = true;
                            Btn_Nuevo.AlternateText = "Nuevo";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            Btn_Salir.AlternateText = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Grid_Peritos_Externos.SelectedIndex = -1;
                            Txt_Avaluo.Text = "";
                            Txt_Perito_Interno.Text = "";
                            Hdf_Anio_Avaluo.Value = "";
                            Hdf_Empleado_Id.Value = "";
                            Hdf_No_Avaluo.Value = "";
                            Hdf_Perito_Interno_Id.Value = "";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Asignación de Peritos Internos", "alert('El perito Interno ha sido asignado exitosamente.');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Asignación de Peritos Internos", "alert('Error al intentar asignar el Perito Interno.');", true);
                        }
                    }
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione el Perito a re-asignar";
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
            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Configuracion_Formulario(true);
            Llenar_Tabla_Avaluos_Urbanos_Asignados(Grid_Avaluos_Asignados.PageIndex);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Grid_Avaluos_Asignados.SelectedIndex = -1;
            Grid_Avaluos_Urbanos.DataSource = null;
            Grid_Avaluos_Urbanos.DataBind();
            Grid_Peritos_Externos.DataSource = null;
            Grid_Peritos_Externos.DataBind();
            Txt_Avaluo.Text = "";
            Txt_Perito_Interno.Text = "";
            Hdf_Anio_Avaluo.Value = "";
            Hdf_Empleado_Id.Value = "";
            Hdf_No_Avaluo.Value = "";
            Hdf_Perito_Interno_Id.Value = "";
        }
    }
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Llenar_Tabla_Avaluos_Urbanos_Asignados(0);
    }

    protected void Btn_Cerrar_Ventana_Click(object sender, ImageClickEventArgs e)
    {
        Mpe_Busqueda_Peritos_Internos.Hide();
        Txt_Busqueda_Nombre.Text = "";
        Grid_Peritos_Externos.DataSource = null;
        Grid_Peritos_Externos.DataBind();
    }

    protected void Btn_Cerrar_Ventana_Avaluo_Click(object sender, ImageClickEventArgs e)
    {
        Mpe_Avaluos.Hide();
        Txt_Busqueda_Avaluo.Text = "";
        Grid_Avaluos_Urbanos.DataSource = null;
        Grid_Avaluos_Urbanos.DataBind();
    }

    protected void Btn_Busqueda_Empleados_Click(object sender, EventArgs e)
    {
        Llenar_Tabla_Peritos_Internos(0);
    }

    protected void Btn_Busqueda_Avaluos_Click(object sender, EventArgs e)
    {
        Llenar_Tabla_Avaluos_Urbanos(0);
    }

    private Boolean Validar_Componentes()
    {
        String Mensaje_Error = "Error: ";
        Boolean valido = true;
        if (Hdf_Anio_Avaluo.Value.Trim() == "" || Hdf_No_Avaluo.Value.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Seleccione un avalúo.";
            valido = false;
        }

        if (Hdf_Perito_Interno_Id.Value.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Seleccione el Perito interno a asignar.";
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

}