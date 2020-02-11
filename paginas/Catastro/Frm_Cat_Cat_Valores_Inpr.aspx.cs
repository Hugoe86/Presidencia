using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Cat_Valores_Inpr.Negocio;

public partial class paginas_Catastro_Frm_Cat_Cat_Valores_Inpr : System.Web.UI.Page
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
                Llenar_Tabla_Valores_Inpr(0);
                Configuracion_Formulario(true);
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
        Txt_Anio.Enabled = !Enabled;
        Txt_Valor_Inpr.Enabled = !Enabled;
        Btn_Agregar_Inpr.Enabled = !Enabled;
        Btn_Actualizar_Inpr.Enabled = !Enabled;
        Btn_Eliminar_Inpr.Enabled = !Enabled;
        Txt_Busqueda.Enabled = Enabled;
        Btn_Buscar.Enabled = Enabled;
        Txt_Anio.Style["text-align"] = "Right";
        Txt_Valor_Inpr.Style["text-align"] = "Right";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Valores_Inpa
    ///DESCRIPCIÓN: Llena la tabla con los valores INPA
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 21/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Valores_Inpr(int Pagina)
    {
        try
        {
            DataTable Dt_Valores_Inpr;
            Cls_Cat_Cat_Valores_Inpr_Negocio Tabla_Val = new Cls_Cat_Cat_Valores_Inpr_Negocio();
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Tabla_Val.P_Anio = Txt_Busqueda.Text.ToUpper();
            }
            Grid_Inpr.Columns[1].Visible = true;
            Grid_Inpr.Columns[4].Visible = true;
            Dt_Valores_Inpr = Tabla_Val.Consultar_Valores_Inpr();
            Dt_Valores_Inpr.DefaultView.Sort = "ANIO DESC";
            Grid_Inpr.DataSource = Dt_Valores_Inpr;
            Grid_Inpr.PageIndex = Pagina;
            Grid_Inpr.DataBind();
            Grid_Inpr.Columns[1].Visible = false;
            Grid_Inpr.Columns[4].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                if (Grid_Inpr.Rows.Count == 0)
                {
                    Configuracion_Formulario(false);
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.Visible = false;
                    Txt_Anio.Text = "";
                    Txt_Valor_Inpr.Text = "";
                    Grid_Inpr.SelectedIndex = -1;
                    Cls_Cat_Cat_Valores_Inpr_Negocio Tabla_Val = new Cls_Cat_Cat_Valores_Inpr_Negocio();
                    Session["Dt_Valores_Inpr"] = Tabla_Val.Consultar_Valores_Inpr();
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
                Cls_Cat_Cat_Valores_Inpr_Negocio Tabla_Val = new Cls_Cat_Cat_Valores_Inpr_Negocio();
                Tabla_Val.P_Dt_Tabla_Valores_Inpr = (DataTable)Session["Dt_Valores_Inpr"];
                if ((Tabla_Val.Alta_Valor_Inpr()))
                {
                    Configuracion_Formulario(true);
                    Llenar_Tabla_Valores_Inpr(Grid_Inpr.PageIndex);
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Inpr.SelectedIndex = -1;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de I.N.P.R.", "alert('Alta Exitosa');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de I.N.P.R.", "alert('Alta Errónea');", true);
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
            if (Btn_Modificar.AlternateText.Equals("Modificar"))
            {
                if (Grid_Inpr.Rows.Count > 0)
                {
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                    Txt_Anio.Text = "";
                    Txt_Valor_Inpr.Text = "";
                    Cls_Cat_Cat_Valores_Inpr_Negocio Tabla_Val = new Cls_Cat_Cat_Valores_Inpr_Negocio();
                    Session["Dt_Valores_Inpr"] = Tabla_Val.Consultar_Valores_Inpr();
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "Imposible modificar.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Cat_Cat_Valores_Inpr_Negocio Tabla_Val = new Cls_Cat_Cat_Valores_Inpr_Negocio();
                    Tabla_Val.P_Dt_Tabla_Valores_Inpr = (DataTable)Session["Dt_Valores_Inpr"];
                    if ((Tabla_Val.Modificar_Valor_Inpr()))
                    {
                        Configuracion_Formulario(true);
                        Llenar_Tabla_Valores_Inpr(Grid_Inpr.PageIndex);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Nuevo.Visible = true;
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Inpr.Enabled = true;
                        Grid_Inpr.SelectedIndex = -1;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de I.N.P.R.", "alert('Actualización Exitosa.');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de I.N.P.R.", "alert('Error al intentar Actualizar.');", true);
                    }
                }
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
            Llenar_Tabla_Valores_Inpr(Grid_Inpr.PageIndex);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Grid_Inpr.SelectedIndex = -1;
            Txt_Anio.Text = "";
            Txt_Valor_Inpr.Text = "";
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
        Llenar_Tabla_Valores_Inpr(0);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Valor_Inpa_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_Valor_Inpa
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 09/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Valor_Inpr_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_Valor_Inpr.Text.Trim() == "")
            {
                Txt_Valor_Inpr.Text = "0.00";
            }
            else
            {
                Txt_Valor_Inpr.Text = Convert.ToDouble(Txt_Valor_Inpr.Text.Trim()).ToString("#,###,###,##0.00");
            }
        }
        catch
        {
            Txt_Valor_Inpr.Text = "0.00";
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
    protected void Btn_Agregar_Valor_Click(object sender, ImageClickEventArgs e)
    {
        if (Txt_Anio.Text.Trim() != "" && Txt_Valor_Inpr.Text.Trim() != "")
        {
            DataTable Dt_Tabla_Val = (DataTable)Session["Dt_Valores_Inpr"];
            if (!Existe_Valor_Inpr(Dt_Tabla_Val, Txt_Anio.Text.Trim(), Convert.ToDouble(Txt_Valor_Inpr.Text).ToString()))
            {
                DataRow Dr_Valor_Nuevo = Dt_Tabla_Val.NewRow();
                Dr_Valor_Nuevo["VALOR_INPR_ID"] = " ";
                Dr_Valor_Nuevo["ANIO"] = Convert.ToInt16(Txt_Anio.Text.Trim());
                Dr_Valor_Nuevo["VALOR_INPR"] = Convert.ToDouble(Txt_Valor_Inpr.Text);
                Dr_Valor_Nuevo["ACCION"] = "ALTA";
                Dt_Tabla_Val.Rows.Add(Dr_Valor_Nuevo);
                Session["Dt_Valores_Inpr"] = Dt_Tabla_Val.Copy();
                Dt_Tabla_Val.DefaultView.RowFilter = "ACCION <> 'BAJA'";
                Dt_Tabla_Val.DefaultView.Sort = "ANIO DESC";
                Grid_Inpr.Columns[1].Visible = true;
                Grid_Inpr.Columns[4].Visible = true;
                Grid_Inpr.DataSource = Dt_Tabla_Val;
                Grid_Inpr.PageIndex = Convert.ToInt16(Dt_Tabla_Val.Rows.Count / 10);
                Grid_Inpr.DataBind();
                Grid_Inpr.Columns[1].Visible = false;
                Grid_Inpr.Columns[4].Visible = false;
                Txt_Anio.Text = "";
                Txt_Valor_Inpr.Text = "";
                Grid_Inpr.SelectedIndex = -1;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Valores I.N.P.R.", "alert('Introduzca los valores indicados.')", true);
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
    protected void Btn_Actualizar_Valor_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Inpr.SelectedIndex > -1)
            {
                if (Txt_Anio.Text.Trim() != "" && Txt_Valor_Inpr.Text.Trim() != "")
                {
                    DataTable Dt_Tabla_Val = (DataTable)Session["Dt_Valores_Inpr"];
                    if (Txt_Anio.Text != Hdf_Anio.Value || Convert.ToDouble(Txt_Valor_Inpr.Text) != Convert.ToDouble(hdf_Valor_Inpr.Value))
                    {
                        if (Txt_Anio.Text == Hdf_Anio.Value && Convert.ToDouble(Txt_Valor_Inpr.Text).ToString() != Convert.ToDouble(hdf_Valor_Inpr.Value).ToString())
                        {
                            foreach (DataRow Dr_Renglon in Dt_Tabla_Val.Rows)
                            {
                                if (Dr_Renglon["ANIO"].ToString() == Hdf_Anio.Value && Dr_Renglon["ACCION"].ToString() != "BAJA")
                                {
                                    Dr_Renglon["ANIO"] = Convert.ToInt16(Txt_Anio.Text.Trim());
                                    Dr_Renglon["VALOR_INPR"] = Convert.ToDouble(Txt_Valor_Inpr.Text.Trim());
                                    if (Dr_Renglon["VALOR_INPR_ID"].ToString() != "&nbsp;" && Dr_Renglon["VALOR_INPR_ID"].ToString().Trim() != "")
                                    {
                                        Dr_Renglon["ACCION"] = "ACTUALIZAR";
                                    }
                                    else
                                    {
                                        Dr_Renglon["ACCION"] = "ALTA";
                                    }
                                    Grid_Inpr.SelectedIndex = -1;
                                    break;
                                }
                            }
                        }
                        else if (!Existe_Valor_Inpr(Dt_Tabla_Val, Txt_Anio.Text.Trim(), Convert.ToDouble(Txt_Valor_Inpr.Text).ToString()))
                        {
                            foreach (DataRow Dr_Renglon in Dt_Tabla_Val.Rows)
                            {
                                if (Dr_Renglon["ANIO"].ToString() == Hdf_Anio.Value && Dr_Renglon["VALOR_INPR"].ToString() == Convert.ToDouble(hdf_Valor_Inpr.Value).ToString() && Dr_Renglon["ACCION"].ToString() != "BAJA")
                                {
                                    Dr_Renglon["ANIO"] = Convert.ToInt16(Txt_Anio.Text.Trim());
                                    Dr_Renglon["VALOR_INPR"] = Convert.ToDouble(Txt_Valor_Inpr.Text.Trim());
                                    if (Dr_Renglon["VALOR_INPR_ID"].ToString() != "&nbsp;" && Dr_Renglon["VALOR_INPR_ID"].ToString().Trim() != "")
                                    {
                                        Dr_Renglon["ACCION"] = "ACTUALIZAR";
                                    }
                                    else
                                    {
                                        Dr_Renglon["ACCION"] = "ALTA";
                                    }
                                    Grid_Inpr.SelectedIndex = -1;
                                    break;
                                }
                            }
                        }
                    }
                    Session["Dt_Valores_Inpr"] = Dt_Tabla_Val.Copy();
                    Dt_Tabla_Val.DefaultView.RowFilter = "ACCION <> 'BAJA'";
                    Dt_Tabla_Val.DefaultView.Sort = "ANIO DESC";
                    Grid_Inpr.Columns[1].Visible = true;
                    Grid_Inpr.Columns[4].Visible = true;
                    Grid_Inpr.DataSource = Dt_Tabla_Val;
                    Grid_Inpr.PageIndex = Grid_Inpr.PageIndex;
                    Grid_Inpr.DataBind();
                    Grid_Inpr.Columns[1].Visible = false;
                    Grid_Inpr.Columns[4].Visible = false;
                    Grid_Inpr.SelectedIndex = -1;
                    Txt_Anio.Text = "";
                    Txt_Valor_Inpr.Text = "";
                    Hdf_Anio.Value = "";
                    hdf_Valor_Inpr.Value = "";
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "Introduzca el Anio y el I.N.P.R.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione el I.N.P.R. a modificar.";
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Valor_Click
    ///DESCRIPCIÓN: Da de baja un registro del grid de tramos
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Valor_Click(object sender, ImageClickEventArgs e)
    {
        if (Grid_Inpr.SelectedIndex > -1)
        {
            DataTable Dt_Tabla_Valores = (DataTable)Session["Dt_Valores_Inpr"];
            foreach (DataRow Dr_Renglon in Dt_Tabla_Valores.Rows)
            {
                if (Dr_Renglon["ANIO"].ToString() == Hdf_Anio.Value && Convert.ToDouble(Dr_Renglon["VALOR_INPR"].ToString()).ToString() == Convert.ToDouble(hdf_Valor_Inpr.Value).ToString() && Dr_Renglon["ACCION"].ToString() != "BAJA")
                {
                    Dr_Renglon["ACCION"] = "BAJA";
                    Grid_Inpr.SelectedIndex = -1;
                    break;
                }
            }
            Session["Dt_Valores_Inpr"] = Dt_Tabla_Valores.Copy();
            Dt_Tabla_Valores.DefaultView.RowFilter = "ACCION <> 'BAJA'";
            Dt_Tabla_Valores.DefaultView.Sort = "ANIO DESC";
            Grid_Inpr.Columns[1].Visible = true;
            Grid_Inpr.Columns[4].Visible = true;
            Grid_Inpr.DataSource = Dt_Tabla_Valores;
            Grid_Inpr.PageIndex = Convert.ToInt16(Dt_Tabla_Valores.Rows.Count / 10);
            Grid_Inpr.DataBind();
            Grid_Inpr.Columns[1].Visible = false;
            Grid_Inpr.Columns[4].Visible = false;
            Txt_Anio.Text = "";
            Hdf_Anio.Value = "";
            Txt_Valor_Inpr.Text = "";
            hdf_Valor_Inpr.Value = "";
            Grid_Inpr.SelectedIndex = -1;
        }
        else
        {
            Lbl_Ecabezado_Mensaje.Text = "Seleccione el I.N.P.R. a eliminar.";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Inpa_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Inpr_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Llenar_Tabla_Valores_Inpr(e.NewPageIndex);
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Tipos_Construccion_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona un registro del grid y toma los datos de los mismos campos del componente
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Inpr_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Inpr.SelectedIndex > -1)
        {
            Hdf_Inpr_Id.Value = Grid_Inpr.SelectedRow.Cells[1].Text;
            Txt_Anio.Text = Grid_Inpr.SelectedRow.Cells[2].Text;
            Txt_Valor_Inpr.Text = Convert.ToDouble(Grid_Inpr.SelectedRow.Cells[3].Text).ToString("###,###,###,##0.00");
            Hdf_Anio.Value = Grid_Inpr.SelectedRow.Cells[2].Text;
            hdf_Valor_Inpr.Value = Convert.ToDouble(Grid_Inpr.SelectedRow.Cells[3].Text).ToString();
            Btn_Salir.AlternateText = "Atras";
        }
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
    private Boolean Existe_Valor_Inpr(DataTable Dt_Tabla_Val, String Anio, String Valor_Inpr)
    {
        Boolean Existe = false;

        foreach (DataRow Dr_Renglon in Dt_Tabla_Val.Rows)
        {
            if (Dr_Renglon["ANIO"].ToString() == Anio && Dr_Renglon["ACCION"].ToString() != "BAJA")
            {
                Existe = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de I.N.P.R.", "alert('Ya existe un I.N.P.R. para el año " + Anio + "');", true);
                break;
            }
        }
        return Existe;
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
        String Msj_Error = "Error: ";
        if (Grid_Inpr.Rows.Count == 0)
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese Valores I.N.P.R.";
            Valido = false;
        }
        if (!Valido)
        {
            Lbl_Ecabezado_Mensaje.Text = Msj_Error;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Valido;
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
        String Msj_Error = "Error: ";
        DataTable Dt_Tabla_Valores = (DataTable)Session["Dt_Valores_Inpr"];
        if (Dt_Tabla_Valores.Rows.Count == 0)
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese Valores I.N.P.R.";
            Valido = false;
        }
        if (!Valido)
        {
            Lbl_Ecabezado_Mensaje.Text = Msj_Error;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Valido;
    }
}