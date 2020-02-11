using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using System.Data;
using Presidencia.Cat_Cat_Tramos_Calle.Negocio;
using Presidencia.Catalogo_Cat_Tabla_Valores_Tramos.Negocio;

public partial class paginas_Catastro_Frm_Cat_Cat_Tabla_Valores_Tramos : System.Web.UI.Page
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
                Llenar_Tabla_Tramos_Calle(0);
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
        Btn_Actualizar.Enabled = !Enabled;
        Btn_Agregar.Enabled = !Enabled;
        Btn_Eliminar.Enabled = !Enabled;
        Txt_Anio.Enabled = !Enabled;
        Txt_Valor_M2.Enabled = !Enabled;
        Btn_Buscar_Calles.Enabled = Enabled;
        Txt_Busqueda_Calles.Enabled = Enabled;
        Grid_Tramos_Calle.Enabled = Enabled;
        Grid_Tramos_Calle.Enabled = Enabled;
        Txt_Anio.Style["text-align"] = "Right";
        Txt_Valor_M2.Style["text-align"] = "Right";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Tramos_Calle
    ///DESCRIPCIÓN: Llena el gridde Tramos Calle
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 14/Junio/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Tramos_Calle(int Pagina)
    {
        try
        {
            DataTable Dt_Calles;
            Cls_Cat_Cat_Tabla_Valores_Tramos_Negocio Calles = new Cls_Cat_Cat_Tabla_Valores_Tramos_Negocio();
            if (Txt_Busqueda_Calles.Text.Trim() != "")
            {
                if (Cmb_Filtro_Busqueda.SelectedValue == "TRAMO")
                {
                    Calles.P_Descripcion_Tramo = Txt_Busqueda_Calles.Text.ToUpper();
                }
                else if (Cmb_Filtro_Busqueda.SelectedValue == "CALLE")
                {
                    Calles.P_Calle_Busqueda = Txt_Busqueda_Calles.Text.ToUpper();
                }
            }
            Grid_Tramos_Calle.Columns[1].Visible = true;
            Dt_Calles = Calles.Consultar_Tramos_Tabla_Valores();
            Grid_Tramos_Calle.DataSource = Dt_Calles;
            Grid_Tramos_Calle.PageIndex = Pagina;
            Grid_Tramos_Calle.DataBind();
            Grid_Tramos_Calle.Columns[1].Visible = false;
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
            if (Grid_Tramos_Calle.SelectedIndex > -1)
            {
                if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
                {
                    if (Grid_Valores_Tramos.Rows.Count == 0)
                    {
                        Configuracion_Formulario(false);
                        Btn_Nuevo.AlternateText = "Dar de Alta";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                        Btn_Salir.AlternateText = "Cancelar";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Modificar.Visible = false;
                        Txt_Anio.Text = "";
                        Txt_Valor_M2.Text = "";
                        Grid_Valores_Tramos.SelectedIndex = -1;
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
                    Cls_Cat_Cat_Tabla_Valores_Tramos_Negocio Tabla_Val = new Cls_Cat_Cat_Tabla_Valores_Tramos_Negocio();
                    Tabla_Val.P_Tramo_Id = Hdf_Tramo_Id.Value;
                    Tabla_Val.P_Dt_Tabla_Valores_Tramos = (DataTable)Session["Tabla_Valores"];
                    if ((Tabla_Val.Alta_Tabla_Valor()))
                    {
                        Div_Grid_Calles.Visible = true;
                        Div_Grid_Tramos.Visible = false;
                        Div_Grid_Tabla_Valores.Visible = false;
                        Configuracion_Formulario(true);
                        Llenar_Tabla_Tramos_Calle(Grid_Tramos_Calle.PageIndex);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Modificar.Visible = true;
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Tramos_Calle.SelectedIndex = -1;
                        Grid_Valores_Tramos.SelectedIndex = -1;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo Tabla de valores por tramos", "alert('Alta de tabla de valores Exitosa');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo Tabla de valores por tramos", "alert('Alta tabla de valores Errónea');", true);
                    }
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione un tramo de calle.";
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
            if (Grid_Tramos_Calle.SelectedIndex > -1)
            {
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
                {
                    if (Grid_Valores_Tramos.Rows.Count > 0)
                    {
                        Configuracion_Formulario(false);
                        Btn_Modificar.AlternateText = "Actualizar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                        Btn_Salir.AlternateText = "Cancelar";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Nuevo.Visible = false;
                        Txt_Anio.Text = "";
                        Txt_Valor_M2.Text = "";
                        Grid_Valores_Tramos.SelectedIndex = -1;
                    }
                    else
                    {
                        Lbl_Ecabezado_Mensaje.Text = "No puede modificar la tabla de valores ya que no contiene datos.";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
                else
                {
                    if (Validar_Componentes())
                    {
                        Cls_Cat_Cat_Tabla_Valores_Tramos_Negocio Tabla_Val = new Cls_Cat_Cat_Tabla_Valores_Tramos_Negocio();
                        Tabla_Val.P_Tramo_Id = Hdf_Tramo_Id.Value;
                        Tabla_Val.P_Dt_Tabla_Valores_Tramos = (DataTable)Session["Tabla_Valores"];
                        if ((Tabla_Val.Modificar_Tabla_Valores()))
                        {
                            Div_Grid_Calles.Visible = true;
                            Div_Grid_Tramos.Visible = false;
                            Div_Grid_Tabla_Valores.Visible = false;
                            Configuracion_Formulario(true);
                            Llenar_Tabla_Tramos_Calle(Grid_Tramos_Calle.PageIndex);
                            Btn_Modificar.AlternateText = "Modificar";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            Btn_Nuevo.Visible = true;
                            Btn_Nuevo.AlternateText = "Nuevo";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            Btn_Salir.AlternateText = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            //Grid_Calles.Enabled = true;
                            Grid_Tramos_Calle.SelectedIndex = -1;
                            Grid_Valores_Tramos.SelectedIndex = -1;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo Tabla de valores por tramos", "alert('Actualización de tabla de valores Exitosa.');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo Tabla de valores por tramos", "alert('Error al Actualizar la tabla de valores.');", true);
                        }
                    }
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione un tramo de calle a modificar.";
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
            Llenar_Tabla_Tramos_Calle(Grid_Tramos_Calle.PageIndex);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Div_Grid_Calles.Visible = true;
            Div_Grid_Tramos.Visible = false;
            Div_Grid_Tabla_Valores.Visible = false;
            Grid_Valores_Tramos.DataSource = null;
            Grid_Valores_Tramos.DataBind();
            Grid_Tramos_Calle.SelectedIndex = -1;
            Grid_Valores_Tramos.SelectedIndex = -1;
            Txt_Tramo.Text = "";
            Txt_Anio.Text = "";
            Txt_Valor_M2.Text = "";
            Txt_Colonia.Text = "";
            Txt_Nombre.Text = "";
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
        if (Div_Grid_Calles.Visible == true)
        {
            Llenar_Tabla_Tramos_Calle(0);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Valor_M2_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_Valor_M2
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 09/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Valor_M2_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_Valor_M2.Text.Trim() == "")
            {
                Txt_Valor_M2.Text = "0.00";
            }
            else
            {
                Txt_Valor_M2.Text = Convert.ToDouble(Txt_Valor_M2.Text.Trim()).ToString("#,###,###,##0.00");
            }
        }
        catch
        {
            Txt_Valor_M2.Text = "0.00";
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Click
    ///DESCRIPCIÓN: Agrega registro del grid de Valores por tramo
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Click(object sender, ImageClickEventArgs e)
    {
        if (Txt_Anio.Text.Trim() != "" && Txt_Valor_M2.Text.Trim() != "")
        {
            DataTable Dt_Tabla_Val = (DataTable)Session["Tabla_Valores"];
            if (!Existe_Valor_Construccion(Dt_Tabla_Val, Txt_Anio.Text.Trim(), Convert.ToDouble(Txt_Valor_M2.Text).ToString()))
            {
                DataRow Dr_Valor_Nuevo = Dt_Tabla_Val.NewRow();
                Dr_Valor_Nuevo["VALOR_TRAMO_ID"] = " ";
                Dr_Valor_Nuevo["ANIO"] = Convert.ToInt16(Txt_Anio.Text.Trim());
                Dr_Valor_Nuevo["VALOR_TRAMO"] = Convert.ToDouble(Txt_Valor_M2.Text);
                Dr_Valor_Nuevo["ACCION"] = "ALTA";
                Dt_Tabla_Val.Rows.Add(Dr_Valor_Nuevo);
                Session["Tabla_Valores"] = Dt_Tabla_Val.Copy();
                Dt_Tabla_Val.DefaultView.RowFilter = "ACCION <> 'BAJA'";
                Dt_Tabla_Val.DefaultView.Sort = "ANIO DESC";
                Grid_Valores_Tramos.Columns[1].Visible = true;
                Grid_Valores_Tramos.Columns[4].Visible = true;
                Grid_Valores_Tramos.DataSource = Dt_Tabla_Val;
                Grid_Valores_Tramos.PageIndex = Convert.ToInt16(Dt_Tabla_Val.Rows.Count / 10);
                Grid_Valores_Tramos.DataBind();
                Grid_Valores_Tramos.Columns[1].Visible = false;
                Grid_Valores_Tramos.Columns[4].Visible = false;
                Txt_Anio.Text = "";
                Txt_Valor_M2.Text = "";
                Grid_Valores_Tramos.SelectedIndex = -1;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tabla de Valores para Construcción", "alert('Introduzca el Año y el valor del Metro Cuadrado.')", true);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Actualizar_Click
    ///DESCRIPCIÓN: Modifica un registro del grid de Valores por tramos
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Actualizar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Valores_Tramos.SelectedIndex > -1)
            {
                if (Txt_Anio.Text.Trim() != "" && Txt_Valor_M2.Text.Trim() != "")
                {
                    DataTable Dt_Tabla_Val = (DataTable)Session["Tabla_Valores"];
                    if (Txt_Anio.Text != Hdf_Anio.Value || Convert.ToDouble(Txt_Valor_M2.Text) != Convert.ToDouble(Hdf_Valor_M2.Value))
                    {
                        if (Txt_Anio.Text == Hdf_Anio.Value && Convert.ToDouble(Txt_Valor_M2.Text).ToString() != Convert.ToDouble(Hdf_Valor_M2.Value).ToString())
                        {
                            foreach (DataRow Dr_Renglon in Dt_Tabla_Val.Rows)
                            {
                                if (Dr_Renglon["ANIO"].ToString() == Hdf_Anio.Value && Dr_Renglon["ACCION"].ToString() != "BAJA")
                                {
                                    Dr_Renglon["ANIO"] = Convert.ToInt16(Txt_Anio.Text.Trim());
                                    Dr_Renglon["VALOR_TRAMO"] = Convert.ToDouble(Txt_Valor_M2.Text.Trim());
                                    if (Dr_Renglon["VALOR_TRAMO_ID"].ToString() != "&nbsp;" && Dr_Renglon["VALOR_TRAMO_ID"].ToString().Trim() != "")
                                    {
                                        Dr_Renglon["ACCION"] = "ACTUALIZAR";
                                    }
                                    else
                                    {
                                        Dr_Renglon["ACCION"] = "ALTA";
                                    }
                                    Grid_Valores_Tramos.SelectedIndex = -1;
                                    break;
                                }
                            }
                        }
                        else if (!Existe_Valor_Construccion(Dt_Tabla_Val, Txt_Anio.Text.Trim(), Convert.ToDouble(Txt_Valor_M2.Text).ToString()))
                        {
                            foreach (DataRow Dr_Renglon in Dt_Tabla_Val.Rows)
                            {
                                if (Dr_Renglon["ANIO"].ToString() == Hdf_Anio.Value && Dr_Renglon["VALOR_TRAMO"].ToString() == Convert.ToDouble(Hdf_Valor_M2.Value).ToString() && Dr_Renglon["ACCION"].ToString() != "BAJA")
                                {
                                    Dr_Renglon["ANIO"] = Convert.ToInt16(Txt_Anio.Text.Trim());
                                    Dr_Renglon["VALOR_TRAMO"] = Convert.ToDouble(Txt_Valor_M2.Text.Trim());
                                    if (Dr_Renglon["VALOR_TRAMO_ID"].ToString() != "&nbsp;" && Dr_Renglon["VALOR_TRAMO_ID"].ToString().Trim() != "")
                                    {
                                        Dr_Renglon["ACCION"] = "ACTUALIZAR";
                                    }
                                    else
                                    {
                                        Dr_Renglon["ACCION"] = "ALTA";
                                    }
                                    Grid_Valores_Tramos.SelectedIndex = -1;
                                    break;
                                }
                            }
                        }
                    }
                    Session["Tabla_Valores"] = Dt_Tabla_Val.Copy();
                    Dt_Tabla_Val.DefaultView.RowFilter = "ACCION <> 'BAJA'";
                    Dt_Tabla_Val.DefaultView.Sort = "ANIO DESC";
                    Grid_Valores_Tramos.Columns[1].Visible = true;
                    Grid_Valores_Tramos.Columns[4].Visible = true;
                    Grid_Valores_Tramos.DataSource = Dt_Tabla_Val;
                    Grid_Valores_Tramos.PageIndex = Grid_Valores_Tramos.PageIndex;
                    Grid_Valores_Tramos.DataBind();
                    Grid_Valores_Tramos.Columns[1].Visible = false;
                    Grid_Valores_Tramos.Columns[4].Visible = false;
                    Grid_Valores_Tramos.SelectedIndex = -1;
                    Txt_Anio.Text = "";
                    Txt_Valor_M2.Text = "";
                    Hdf_Anio.Value = "";
                    Hdf_Valor_M2.Value = "";
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "Introduzca el Anio y el Valor del Metro Cuadrado.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione el Valor a modificar.";
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: Da de baja un registro del grid de Valores por tramo
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        if (Grid_Valores_Tramos.SelectedIndex > -1)
        {
            DataTable Dt_Tabla_Valores = (DataTable)Session["Tabla_Valores"];
            foreach (DataRow Dr_Renglon in Dt_Tabla_Valores.Rows)
            {
                if (Dr_Renglon["ANIO"].ToString() == Hdf_Anio.Value && Convert.ToDouble(Dr_Renglon["VALOR_TRAMO"].ToString()).ToString() == Convert.ToDouble(Hdf_Valor_M2.Value).ToString() && Dr_Renglon["ACCION"].ToString() != "BAJA")
                {
                    Dr_Renglon["ACCION"] = "BAJA";
                    Grid_Valores_Tramos.SelectedIndex = -1;
                    break;
                }
            }
            Session["Tabla_Valores"] = Dt_Tabla_Valores.Copy();
            Dt_Tabla_Valores.DefaultView.RowFilter = "ACCION <> 'BAJA'";
            Dt_Tabla_Valores.DefaultView.Sort = "ANIO DESC";
            Grid_Valores_Tramos.Columns[1].Visible = true;
            Grid_Valores_Tramos.Columns[4].Visible = true;
            Grid_Valores_Tramos.DataSource = Dt_Tabla_Valores;
            Grid_Valores_Tramos.PageIndex = Convert.ToInt16(Dt_Tabla_Valores.Rows.Count / 10);
            Grid_Valores_Tramos.DataBind();
            Grid_Valores_Tramos.Columns[1].Visible = false;
            Grid_Valores_Tramos.Columns[4].Visible = false;
            Txt_Anio.Text = "";
            Hdf_Anio.Value = "";
            Txt_Valor_M2.Text = "";
            Hdf_Valor_M2.Value = "";
            Grid_Valores_Tramos.SelectedIndex = -1;
        }
        else
        {
            Lbl_Ecabezado_Mensaje.Text = "Seleccione el Valor a eliminar.";
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
        DataTable Dt_Tabla_Valores = (DataTable)Session["Tabla_Valores"];
        if (Dt_Tabla_Valores.Rows.Count == 0)
        {
            Lbl_Ecabezado_Mensaje.Text = "Necesita agregar Valores";
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
        if (Grid_Valores_Tramos.Rows.Count == 0)
        {
            Lbl_Ecabezado_Mensaje.Text = "Necesita agregar Valores";
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
    private Boolean Existe_Valor_Construccion(DataTable Dt_Tabla_Val, String Anio, String Valor_M2)
    {
        Boolean Existe = false;

        foreach (DataRow Dr_Renglon in Dt_Tabla_Val.Rows)
        {
            if (Dr_Renglon["ANIO"].ToString() == Anio && Dr_Renglon["ACCION"].ToString() != "BAJA")
            {
                Existe = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tabla de Valores para Construcción", "alert('Ya existe un valor de M2 para el año " + Anio + "');", true);
                break;
            }
        }
        return Existe;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Existe_Valor_Construccion_Actualizar
    ///DESCRIPCIÓN: Determina si los datos ingresado ya existen en el grid de tramos.
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Existe_Valor_Construccion_Actualizar(DataTable Dt_Tabla_Val, String Anio, String Valor_M2)
    {
        Boolean Existe = false;

        foreach (DataRow Dr_Renglon in Dt_Tabla_Val.Rows)
        {
            if (Dr_Renglon["ANIO"].ToString() == Anio && Dr_Renglon["VALOR_TRAMO"].ToString() == Valor_M2 && Dr_Renglon["ACCION"].ToString() != "BAJA")
            {
                Existe = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tabla de Valores para Construcción", "alert('Ya existe un valor de M2 para el año " + Anio + "');", true);
                break;
            }
        }
        return Existe;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Valores_Tramos_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid de Valores por tramo
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 11/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Valores_Tramos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Valores_Tramos.SelectedIndex = -1;
            Hdf_Anio.Value = "";
            Hdf_Valor_M2.Value = "";
            Txt_Anio.Text = "";
            Txt_Valor_M2.Text = "";
            DataTable Dt_Tabla_Valores = (DataTable)Session["Tabla_Valores"];
            Grid_Valores_Tramos.Columns[1].Visible = true;
            Grid_Valores_Tramos.Columns[4].Visible = true;
            Dt_Tabla_Valores.DefaultView.RowFilter = "ACCION <> 'BAJA'";
            Dt_Tabla_Valores.DefaultView.Sort = "ANIO DESC";
            Grid_Valores_Tramos.DataSource = Dt_Tabla_Valores;
            Grid_Valores_Tramos.PageIndex = e.NewPageIndex;
            Grid_Valores_Tramos.DataBind();
            Grid_Valores_Tramos.Columns[1].Visible = false;
            Grid_Valores_Tramos.Columns[4].Visible = false;
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Valores_Tramos_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona un elemento del grid de Valores por tramo y toma sus valores correspondientes
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 11/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Valores_Tramos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Valores_Tramos.SelectedIndex > -1)
            {
                Txt_Anio.Text = HttpUtility.HtmlDecode(Grid_Valores_Tramos.SelectedRow.Cells[2].Text);
                Hdf_Anio.Value = Txt_Anio.Text;
                Txt_Valor_M2.Text = Convert.ToDouble(HttpUtility.HtmlDecode(Grid_Valores_Tramos.SelectedRow.Cells[3].Text.Replace("$", ""))).ToString("#,###,###,##0.00");
                Hdf_Valor_M2.Value = HttpUtility.HtmlDecode(Grid_Valores_Tramos.SelectedRow.Cells[3].Text.Replace("$", "").Replace(",", ""));
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione un Valor.";
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Tramos_Calle_PageIndexChanging
    ///DESCRIPCIÓN: Cambia de página
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 11/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Tramos_Calle_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Txt_Nombre.Text = "";
        Txt_Colonia.Text = "";
        Txt_Tramo.Text = "";
        Hdf_Tramo_Id.Value = "";
        Grid_Valores_Tramos.SelectedIndex = -1;
        Llenar_Tabla_Tramos_Calle(e.NewPageIndex);
        Grid_Valores_Tramos.DataSource = null;
        Grid_Valores_Tramos.DataBind();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Tramos_Calle_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona el renglón y carga sus valores en los componentes
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 11/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Tramos_Calle_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Tramos_Calle.SelectedIndex > -1)
        {
            Hdf_Tramo_Id.Value = Grid_Tramos_Calle.SelectedRow.Cells[1].Text;
            Txt_Nombre.Text = HttpUtility.HtmlDecode(Grid_Tramos_Calle.SelectedRow.Cells[2].Text);
            Txt_Colonia.Text = HttpUtility.HtmlDecode(Grid_Tramos_Calle.SelectedRow.Cells[3].Text);
            Txt_Tramo.Text = HttpUtility.HtmlDecode(Grid_Tramos_Calle.SelectedRow.Cells[4].Text);
            Cls_Cat_Cat_Tabla_Valores_Tramos_Negocio Tabla_Valores = new Cls_Cat_Cat_Tabla_Valores_Tramos_Negocio();
            Tabla_Valores.P_Tramo_Id = Hdf_Tramo_Id.Value;
            DataTable Dt_Tabla_Valores = Tabla_Valores.Consultar_Tabla_Valores_Tramo();
            Session["Tabla_Valores"] = Dt_Tabla_Valores.Copy();
            Dt_Tabla_Valores.DefaultView.Sort = "ANIO DESC";
            Grid_Valores_Tramos.Columns[1].Visible = true;
            Grid_Valores_Tramos.Columns[4].Visible = true;
            Grid_Valores_Tramos.DataSource = Dt_Tabla_Valores;
            Grid_Valores_Tramos.PageIndex = 0;
            Grid_Valores_Tramos.DataBind();
            Grid_Valores_Tramos.Columns[1].Visible = false;
            Grid_Valores_Tramos.Columns[4].Visible = false;
            Grid_Valores_Tramos.SelectedIndex = -1;
            Txt_Anio.Text = "";
            Txt_Valor_M2.Text = "";
        }
    }
}