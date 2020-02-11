using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Calles.Negocio;
using System.Data;
using Presidencia.Cat_Cat_Tramos_Calle.Negocio;

public partial class paginas_Catastro_Frm_Cat_Cat_Tramos_Calle : System.Web.UI.Page
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
                Llenar_Tabla_Calles(0);
                Txt_Tramo.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,250)");
                Txt_Tramo.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,250)");
                Txt_Tramo.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,250)");
                Txt_Tramo.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,250)");
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
        Txt_Tramo.Enabled = !Enabled;
        Btn_Actualizar_Tramo.Enabled = !Enabled;
        Btn_Agregar_Tramo.Enabled = !Enabled;
        Btn_Eliminar_Tramo.Enabled = !Enabled;
        Grid_Calles.Enabled = Enabled;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Calles
    ///DESCRIPCIÓN: Llena la tabla de Calles
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 20/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Calles(int Pagina)
    {
        try
        {
            DataTable Dt_Calles;
            Cls_Cat_Cat_Tramos_Calle_Negocio Calles = new Cls_Cat_Cat_Tramos_Calle_Negocio();
            if (Cmb_Filtro_Busqueda.SelectedValue == "CALLE" && Txt_Busqueda_Calles.Text.Trim() != "")
            {
                Calles.P_Calle_Busqueda = Txt_Busqueda_Calles.Text.ToUpper();
            }
            else if (Cmb_Filtro_Busqueda.SelectedValue == "COLONIA" && Txt_Busqueda_Calles.Text.Trim() != "")
            {
                Calles.P_Colonia_Busqueda = Txt_Busqueda_Calles.Text.ToUpper();
            }
            Grid_Calles.Columns[1].Visible = true;
            Grid_Calles.Columns[5].Visible = true;
            Dt_Calles = Calles.Consultar_Calles();
            Grid_Calles.DataSource = Dt_Calles;
            Grid_Calles.PageIndex = Pagina;
            Grid_Calles.DataBind();
            Grid_Calles.Columns[1].Visible = false;
            Grid_Calles.Columns[5].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
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
            if (Grid_Calles.SelectedIndex > -1)
            {
                if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
                {
                    if (Grid_Tramos.Rows.Count == 0)
                    {
                        Configuracion_Formulario(false);
                        Btn_Nuevo.AlternateText = "Dar de Alta";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                        Btn_Salir.AlternateText = "Cancelar";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Modificar.Visible = false;
                        Txt_Tramo.Text = "";
                        Grid_Tramos.SelectedIndex = -1;
                    }
                    else
                    {
                        Lbl_Ecabezado_Mensaje.Text = "Imposible dar de alta.";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
                else if (Validar_Componentes_Nuevo())
                {
                    Cls_Cat_Cat_Tramos_Calle_Negocio Tramos = new Cls_Cat_Cat_Tramos_Calle_Negocio();
                    Tramos.P_Calle_ID = Hdf_Calle_Id.Value;
                    Tramos.P_Dt_Tramos = (DataTable)Session["Tramos"];
                    if ((Tramos.Alta_Tramos()))
                    {
                        Configuracion_Formulario(true);
                        Llenar_Tabla_Calles(Grid_Calles.PageIndex);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Modificar.Visible = true;
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Calles.SelectedIndex = -1;
                        Txt_Nombre.Text = "";
                        Txt_Colonia.Text = "";
                        Txt_Clave.Text = "";
                        Txt_Tramo.Text = "";
                        Grid_Tramos.SelectedIndex = -1;
                        Grid_Tramos.DataSource = null;
                        Grid_Tramos.PageIndex = 0;
                        Grid_Tramos.DataBind();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tramos", "alert('Alta de Tramo(s) Exitosa');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tramos", "alert('Alta de Tramo(s) Errónea');", true);
                    }
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione la calle.";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
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
            if (Grid_Calles.SelectedIndex > -1)
            {
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
                {
                    if (Grid_Tramos.Rows.Count > 0)
                    {
                        Configuracion_Formulario(false);
                        Btn_Modificar.AlternateText = "Actualizar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                        Btn_Salir.AlternateText = "Cancelar";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Nuevo.Visible = false;
                        Txt_Tramo.Text = "";
                        Grid_Tramos.SelectedIndex = -1;
                    }
                    else
                    {
                        Lbl_Ecabezado_Mensaje.Text = "No puede modificar la calle ya que no contiene tramos";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
                else
                {
                    if (Validar_Componentes())
                    {
                        Cls_Cat_Cat_Tramos_Calle_Negocio Tramos = new Cls_Cat_Cat_Tramos_Calle_Negocio();
                        Tramos.P_Calle_ID = Hdf_Calle_Id.Value;
                        Tramos.P_Dt_Tramos = (DataTable)Session["Tramos"];
                        if ((Tramos.Modificar_Tramos()))
                        {
                            Configuracion_Formulario(true);
                            Llenar_Tabla_Calles(Grid_Calles.PageIndex);
                            Btn_Modificar.AlternateText = "Modificar";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            Btn_Nuevo.Visible = true;
                            Btn_Nuevo.AlternateText = "Nuevo";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            Btn_Salir.AlternateText = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Grid_Calles.Enabled = true;
                            Grid_Calles.SelectedIndex = -1;
                            Txt_Nombre.Text = "";
                            Txt_Colonia.Text = "";
                            Txt_Clave.Text = "";
                            Txt_Tramo.Text = "";
                            Grid_Tramos.SelectedIndex = -1;
                            Grid_Tramos.DataSource = null;
                            Grid_Tramos.PageIndex = 0;
                            Grid_Tramos.DataBind();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tramos", "alert('Actualización de Tramos Exitosa.');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tramos", "alert('Error al Actualizar los Tramos.');", true);
                        }
                    }
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione la calle a modificar.";
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
            Llenar_Tabla_Calles(Grid_Calles.PageIndex);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Grid_Calles.SelectedIndex = -1;
            Txt_Nombre.Text = "";
            Txt_Colonia.Text = "";
            Txt_Clave.Text = "";
            Txt_Tramo.Text = "";
            Grid_Tramos.SelectedIndex = -1;
            Grid_Tramos.DataSource = null;
            Grid_Tramos.PageIndex = 0;
            Grid_Tramos.DataBind();
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
        Llenar_Tabla_Calles(0);
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
    protected void Grid_Calles_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Llenar_Tabla_Calles(e.NewPageIndex);
            Grid_Calles.SelectedIndex = -1;
            Txt_Nombre.Text = "";
            Txt_Colonia.Text = "";
            Txt_Clave.Text = "";
            Txt_Tramo.Text = "";
            Grid_Tramos.SelectedIndex = -1;
            Grid_Tramos.DataSource = null;
            Grid_Tramos.PageIndex = 0;
            Grid_Tramos.DataBind();
        }
        catch(Exception E)
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
    protected void Grid_Calles_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Calles.SelectedIndex > -1)
        {
            Hdf_Calle_Id.Value = Grid_Calles.SelectedRow.Cells[1].Text;
            Txt_Clave.Text = HttpUtility.HtmlDecode(Grid_Calles.SelectedRow.Cells[2].Text);
            Txt_Colonia.Text = HttpUtility.HtmlDecode(Grid_Calles.SelectedRow.Cells[4].Text);
            Txt_Nombre.Text = HttpUtility.HtmlDecode(Grid_Calles.SelectedRow.Cells[3].Text);
            if (Grid_Calles.SelectedRow.Cells[5].Text != "&nbsp;")
            {
                Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Grid_Calles.SelectedRow.Cells[5].Text));
            }
            else
            {
                Cmb_Estatus.SelectedIndex = 3;
            }
            Cls_Cat_Cat_Tramos_Calle_Negocio Tramos = new Cls_Cat_Cat_Tramos_Calle_Negocio();
            Tramos.P_Calle_ID = Hdf_Calle_Id.Value;
            DataTable Dt_Tramos = Tramos.Consultar_Tramos();
            Session["Tramos"] = Dt_Tramos.Copy();
            Grid_Tramos.Columns[1].Visible = true;
            Grid_Tramos.Columns[3].Visible = true;
            Grid_Tramos.DataSource = Dt_Tramos;
            Grid_Tramos.PageIndex = 0;
            Grid_Tramos.DataBind();
            Grid_Tramos.Columns[1].Visible = false;
            Grid_Tramos.Columns[3].Visible = false;
            Btn_Salir.AlternateText = "Atras";
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Tramo_Click
    ///DESCRIPCIÓN: Da de baja un registro del grid de tramos
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Tramo_Click(object sender, ImageClickEventArgs e)
    {
        if (Grid_Tramos.SelectedIndex > -1)
        {
            DataTable Dt_Tramos = (DataTable)Session["Tramos"];
            foreach (DataRow Dr_Renglon in Dt_Tramos.Rows)
            {
                if (Dr_Renglon["TRAMO_DESCRIPCION"].ToString() == Hdf_Descripcion_Tramo.Value.ToUpper() && Dr_Renglon["ACCION"].ToString() != "BAJA")
                {
                    Dr_Renglon["ACCION"] = "BAJA";
                    Grid_Tramos.SelectedIndex = -1;
                    break;
                }
            }
            Session["Tramos"] = Dt_Tramos.Copy();
            Dt_Tramos.DefaultView.RowFilter = "ACCION <> 'BAJA'";
            Grid_Tramos.Columns[1].Visible = true;
            Grid_Tramos.Columns[3].Visible = true;
            Grid_Tramos.DataSource = Dt_Tramos;
            Grid_Tramos.PageIndex = Convert.ToInt16(Dt_Tramos.Rows.Count / 10);
            Grid_Tramos.DataBind();
            Grid_Tramos.Columns[1].Visible = false;
            Grid_Tramos.Columns[3].Visible = false;
            Txt_Tramo.Text = "";
            Hdf_Descripcion_Tramo.Value = "";
            Grid_Tramos.SelectedIndex = -1;
        }
        else
        {
            Lbl_Ecabezado_Mensaje.Text = "Seleccione el tramo a eliminar.";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Actualizar_Tramo_Click
    ///DESCRIPCIÓN: Modifica un registro del grid de tramos
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Actualizar_Tramo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Tramos.SelectedIndex > -1)
            {
                DataTable Dt_Tramos = (DataTable)Session["Tramos"];
                if (Txt_Tramo.Text.ToUpper() != Hdf_Descripcion_Tramo.Value)
                {
                    if (!Existe_Tramo_Calle(Dt_Tramos, Txt_Tramo.Text.ToUpper()))
                    {
                        foreach (DataRow Dr_Renglon in Dt_Tramos.Rows)
                        {
                            if (Dr_Renglon["TRAMO_DESCRIPCION"].ToString() == Hdf_Descripcion_Tramo.Value && Dr_Renglon["ACCION"].ToString() != "BAJA")
                            {
                                Dr_Renglon["TRAMO_DESCRIPCION"] = Txt_Tramo.Text.ToUpper();
                                if (Dr_Renglon["TRAMO_ID"].ToString() != "&nbsp;" && Dr_Renglon["TRAMO_ID"].ToString().Trim() != "")
                                {
                                    Dr_Renglon["ACCION"] = "ACTUALIZAR";
                                }
                                else
                                {
                                    Dr_Renglon["ACCION"] = "ALTA";
                                }
                                Grid_Tramos.SelectedIndex = -1;
                                break;
                            }
                        }
                    }
                }
                Session["Tramos"] = Dt_Tramos.Copy();
                Grid_Tramos.Columns[1].Visible = true;
                Dt_Tramos.DefaultView.RowFilter = "ACCION <> 'BAJA'";
                Grid_Tramos.Columns[1].Visible = true;
                Grid_Tramos.Columns[3].Visible = true;
                Grid_Tramos.DataSource = Dt_Tramos;
                Grid_Tramos.PageIndex = Grid_Tramos.PageIndex;
                Grid_Tramos.DataBind();
                Grid_Tramos.Columns[1].Visible = false;
                Grid_Tramos.Columns[3].Visible = false;
                Grid_Tramos.SelectedIndex = -1;
                Txt_Tramo.Text = "";
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione el tramo a modificar.";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Tramo_Click
    ///DESCRIPCIÓN: Agrega registro del grid de tramos
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Tramo_Click(object sender, ImageClickEventArgs e)
    {
        if (Txt_Tramo.Text.Trim() != "")
        {
            DataTable Dt_Tramos = (DataTable)Session["Tramos"];
            if (!Existe_Tramo_Calle(Dt_Tramos, Txt_Tramo.Text.ToUpper()))
            {
                DataRow Dr_Tramo_Nuevo = Dt_Tramos.NewRow();
                Dr_Tramo_Nuevo["TRAMO_ID"] = " ";
                Dr_Tramo_Nuevo["TRAMO_DESCRIPCION"] = Txt_Tramo.Text.ToUpper();
                Dr_Tramo_Nuevo["ACCION"] = "ALTA";
                Dt_Tramos.Rows.Add(Dr_Tramo_Nuevo);
                Session["Tramos"] = Dt_Tramos.Copy();
                Dt_Tramos.DefaultView.RowFilter = "ACCION <> 'BAJA'";
                Grid_Tramos.Columns[1].Visible = true;
                Grid_Tramos.Columns[3].Visible = true;
                Grid_Tramos.DataSource = Dt_Tramos;
                Grid_Tramos.PageIndex = Convert.ToInt16(Dt_Tramos.Rows.Count / 10);
                Grid_Tramos.DataBind();
                Grid_Tramos.Columns[1].Visible = false;
                Grid_Tramos.Columns[3].Visible = false;
                Txt_Tramo.Text = "";
                Grid_Tramos.SelectedIndex = -1;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tramos", "alert('Introduzca la descripción de una Tramo de calle')", true);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Tramo_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid de tramos
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Tramo_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Tramos.SelectedIndex = -1;
            Hdf_Descripcion_Tramo.Value = "";
            Txt_Tramo.Text = "";
            DataTable Dt_Tramos = (DataTable)Session["Tramos"];
            Grid_Tramos.Columns[1].Visible = true;
            Grid_Tramos.Columns[3].Visible = true;
            DataRow Dr_Tramo_Nuevo = Dt_Tramos.NewRow();
            Dt_Tramos.DefaultView.RowFilter = "ACCION <> 'BAJA'";
            Grid_Tramos.DataSource = Dt_Tramos;
            Grid_Tramos.PageIndex = e.NewPageIndex;
            Grid_Tramos.DataBind();
            Grid_Tramos.Columns[1].Visible = false;
            Grid_Tramos.Columns[3].Visible = false;
        }
        catch(Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Tramo_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona un elemento del grid de tramos y toma sus valores correspondientes
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Tramo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Tramos.SelectedIndex > -1)
            {
                Txt_Tramo.Text = HttpUtility.HtmlDecode(Grid_Tramos.SelectedRow.Cells[2].Text);
                Hdf_Descripcion_Tramo.Value = HttpUtility.HtmlDecode(Txt_Tramo.Text);
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione un Tramo.";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
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
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
    ///DESCRIPCIÓN: Valida los datos ingresados
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Componentes()
    {
        Boolean Valido = true;
        DataTable Dt_Tramos = (DataTable)Session["Tramos"];
        if (Dt_Tramos.Rows.Count == 0)
        {
            Lbl_Ecabezado_Mensaje.Text = "Necesita agregar tramos";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
            Valido = false;
        }
        return Valido;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Nuevo
    ///DESCRIPCIÓN: Valida los datos ingresados cuando se van a dar de alta los tramos por primera vez
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Componentes_Nuevo()
    {
        Boolean Valido = true;
        if (Grid_Tramos.Rows.Count == 0)
        {
            Lbl_Ecabezado_Mensaje.Text = "Necesita agregar tramos";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
            Valido = false;
        }
        return Valido;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Existe_Tramo_Calle
    ///DESCRIPCIÓN: Determina si los datos ingresado ya existen en el grid de tramos.
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Existe_Tramo_Calle(DataTable Dt_Tramo, String Descripcion_Tramo)
    {
        Boolean Existe = false;

        foreach (DataRow Dr_Renglon in Dt_Tramo.Rows)
        {
            if (Dr_Renglon["TRAMO_DESCRIPCION"].ToString() == Descripcion_Tramo && Dr_Renglon["ACCION"].ToString() != "BAJA")
            {
                Existe = true;
                break;
            }
        }
        return Existe;
    }

}