using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Catalogo_Cat_Tabla_Valores_Construccion.Negocio;
using Presidencia.Sessiones;
using System.Data;
using Presidencia.Catalogo_Cat_Tipos_Construccion.Negocio;
using Presidencia.Constantes;
using Presidencia.Catalogo_Cat_Calidad_Construccion.Negocio;

public partial class paginas_Catastro_Frm_Cat_Cat_Tabla_Valores_Construccion : System.Web.UI.Page
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
                Llenar_Combo_Tipos_Construccion();
                Llenar_Tabla_Calidad(0);
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
        Txt_Valor_M2.Enabled = !Enabled;
        Btn_Actualizar_Valor_Construccion.Enabled = !Enabled;
        Btn_Agregar_Valor_Construccion.Enabled = !Enabled;
        Btn_Eliminar_Valor_Construccion.Enabled = !Enabled;
        Txt_Clave_Valor.Enabled = !Enabled;
        Cmb_Estado.Enabled = !Enabled;
        Cmb_Tipos_Construccion.Enabled = false;
        Cmb_Calidad_Construccion.Enabled = false;
        Grid_Calidad.Enabled = Enabled;
        Txt_Busqueda.Enabled = Enabled;
        Btn_Buscar.Enabled = Enabled;
        Txt_Anio.Style["text-align"] = "Right";
        Txt_Clave_Valor.Style["text-align"] = "Right";
        Txt_Valor_M2.Style["text-align"] = "Right";
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
            if (Grid_Calidad.SelectedIndex > -1)
            {
                if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
                {
                    if (Grid_Valores_Construccion.Rows.Count == 0)
                    {
                        Configuracion_Formulario(false);
                        Btn_Nuevo.AlternateText = "Dar de Alta";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                        Btn_Salir.AlternateText = "Cancelar";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Modificar.Visible = false;
                        Txt_Anio.Text = "";
                        Txt_Valor_M2.Text = "";
                        Txt_Clave_Valor.Text = "";
                        Cmb_Estado.SelectedIndex = 0;
                        Grid_Valores_Construccion.SelectedIndex = -1;
                        Session["Tabla_Valores"] = Crear_Dt_Tabla_Valores();
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
                    Cls_Cat_Cat_Tabla_Valores_Construccion_Negocio Tabla_Val = new Cls_Cat_Cat_Tabla_Valores_Construccion_Negocio();
                    Tabla_Val.P_Calidad_Id = Cmb_Calidad_Construccion.SelectedValue;
                    Tabla_Val.P_Tipo_Construccion_Id = Cmb_Tipos_Construccion.SelectedValue;
                    Tabla_Val.P_Dt_Tabla_Valores_Construccion = (DataTable)Session["Tabla_Valores"];
                    if ((Tabla_Val.Alta_Valor_Construccion()))
                    {
                        Configuracion_Formulario(true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Modificar.Visible = true;
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Llenar_Tabla_Calidad(Grid_Calidad.PageIndex);
                        Grid_Valores_Construccion.DataSource = null;
                        Grid_Valores_Construccion.DataBind();
                        Grid_Valores_Construccion.SelectedIndex = -1;
                        Grid_Calidad.SelectedIndex = -1;
                        Txt_Valor_M2.Text = "";
                        Txt_Anio.Text = "";
                        Txt_Clave_Valor.Text = "";
                        Cmb_Tipos_Construccion.SelectedValue = "SELECCIONE";
                        Cmb_Tipos_Construccion_SelectedIndexChanged(null, null);
                        Cmb_Calidad_Construccion.SelectedIndex = -1;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tabla de Valores para Construcción", "alert('Alta Exitosa');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tabla de Valores para Construcción", "alert('Alta Errónea');", true);
                    }
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione un registro de la tabla de Calidad de la Construcción";
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
            if (Grid_Calidad.SelectedIndex > -1)
            {
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
                {
                    if (Grid_Valores_Construccion.Rows.Count > 0)
                    {
                        Configuracion_Formulario(false);
                        Btn_Modificar.AlternateText = "Actualizar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                        Btn_Salir.AlternateText = "Cancelar";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Nuevo.Visible = false;
                        Txt_Anio.Text = "";
                        Txt_Valor_M2.Text = "";
                        Grid_Valores_Construccion.SelectedIndex = -1;
                        Txt_Clave_Valor.Text = "";
                        Cmb_Estado.SelectedIndex = 0;
                    }
                    else
                    {
                        Lbl_Ecabezado_Mensaje.Text = "Imposible modificar";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
                else
                {
                    if (Validar_Componentes())
                    {
                        Cls_Cat_Cat_Tabla_Valores_Construccion_Negocio Tabla_Val = new Cls_Cat_Cat_Tabla_Valores_Construccion_Negocio();
                        Tabla_Val.P_Calidad_Id= Cmb_Calidad_Construccion.SelectedValue;
                        Tabla_Val.P_Tipo_Construccion_Id = Cmb_Tipos_Construccion.SelectedValue;
                        Tabla_Val.P_Dt_Tabla_Valores_Construccion = (DataTable)Session["Tabla_Valores"];
                        if ((Tabla_Val.Modificar_Valor_Construccion()))
                        {
                            Configuracion_Formulario(true);
                            Btn_Modificar.AlternateText = "Modificar";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            Btn_Nuevo.Visible = true;
                            Btn_Nuevo.AlternateText = "Nuevo";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            Btn_Salir.AlternateText = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Llenar_Tabla_Calidad(Grid_Calidad.PageIndex);
                            Grid_Valores_Construccion.DataSource = null;
                            Grid_Valores_Construccion.DataBind();
                            Grid_Valores_Construccion.SelectedIndex = -1;
                            Grid_Calidad.SelectedIndex = -1;
                            Txt_Valor_M2.Text = "";
                            Txt_Anio.Text = "";
                            Txt_Clave_Valor.Text = "";
                            Cmb_Tipos_Construccion.SelectedValue = "SELECCIONE";
                            Cmb_Tipos_Construccion_SelectedIndexChanged(null, null);
                            Cmb_Calidad_Construccion.SelectedIndex = -1;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tabla de Valores", "alert('Actualización Exitosa.');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tramos", "alert('Error al intentar Actualizar.');", true);
                        }
                    }
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione un registro de la tabla de Calidad de la Construcción";
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
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Grid_Valores_Construccion.DataSource = null;
            Grid_Valores_Construccion.DataBind();
            Grid_Valores_Construccion.SelectedIndex = -1;
            Grid_Calidad.SelectedIndex = -1;
            Txt_Valor_M2.Text = "";
            Txt_Anio.Text = "";
            Txt_Clave_Valor.Text = "";
            Cmb_Tipos_Construccion.SelectedValue = "SELECCIONE";
            Cmb_Tipos_Construccion_SelectedIndexChanged(null, null);
            Cmb_Calidad_Construccion.SelectedIndex = -1;
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
        Llenar_Tabla_Calidad(0);
    }

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN: Grid_Tipos_Construccion_PageIndexChanging
    /////DESCRIPCIÓN: Cambia la página del grid
    /////PROPIEDADES:     
    /////            
    /////CREO: Miguel Angel Bedolla Moreno
    /////FECHA_CREO: 05/May_2012
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Grid_Tipos_Construccion_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    try
    //    {
    //        Txt_Anio.Text = "";
    //        Txt_Clave_Valor.Text = "";
    //        Cmb_Estado.SelectedIndex = 0;
    //        Txt_Valor_M2.Text = "";
    //        Grid_Valores_Construccion.SelectedIndex = -1;
    //        Grid_Valores_Construccion.PageIndex = 0;
    //        Grid_Valores_Construccion.DataSource = null;
    //        Grid_Valores_Construccion.DataBind();
    //    }
    //    catch (Exception E)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = E.Message;
    //        Lbl_Mensaje_Error.Text = "";
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }
    //}

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN: Grid_Tipos_Construccion_SelectedIndexChanged
    /////DESCRIPCIÓN: Selecciona un registro del grid y toma los datos de los mismos campos del componente
    /////PROPIEDADES:     
    /////            
    /////CREO: Miguel Angel Bedolla Moreno
    /////FECHA_CREO: 05/May_2012
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Grid_Tipos_Construccion_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //if (Grid_Tipos_Construccion.SelectedIndex > -1)
    //    //{
    //        //hdf_Tipo_Construccion_Id.Value = Grid_Tipos_Construccion.SelectedRow.Cells[1].Text;
    //        //Txt_Tipo_Construccion.Text = Grid_Tipos_Construccion.SelectedRow.Cells[2].Text;
    //        //Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Grid_Tipos_Construccion.SelectedRow.Cells[3].Text.Trim()));
    //        //Llenar_Tabla_Calidad(0);
    //        Txt_Anio.Text = "";
    //        Txt_Clave_Valor.Text = "";
    //        Cmb_Estado.SelectedIndex = 0;
    //        Txt_Valor_M2.Text = "";
    //        //Txt_Calidad.Text = "";
    //        //Txt_Clave_Calidad.Text = "";
    //        //Grid_Calidad.SelectedIndex = -1;
    //        Grid_Valores_Construccion.SelectedIndex = -1;
    //        Grid_Valores_Construccion.PageIndex = 0;
    //        Grid_Valores_Construccion.DataSource = null;
    //        Grid_Valores_Construccion.DataBind();
    //        Btn_Salir.AlternateText = "Atras";
    //    //}
    //}

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
    protected void Btn_Eliminar_Valor_Click(object sender, ImageClickEventArgs e)
    {
        if (Grid_Valores_Construccion.SelectedIndex > -1)
        {
            DataTable Dt_Tabla_Valores = (DataTable)Session["Tabla_Valores"];
            foreach (DataRow Dr_Renglon in Dt_Tabla_Valores.Rows)
            {
                if (Dr_Renglon["ANIO"].ToString() == Hdf_Anio.Value && Convert.ToDouble(Dr_Renglon["VALOR_M2"].ToString()).ToString() == Convert.ToDouble(hdf_Valor_M2.Value).ToString() && Dr_Renglon["ESTADO_CONSERVACION"].ToString() == Hdf_Estado_Conservacion.Value && Dr_Renglon["ACCION"].ToString() != "BAJA")
                {
                    Dr_Renglon["ACCION"] = "BAJA";
                    Grid_Valores_Construccion.SelectedIndex = -1;
                    break;
                }
            }
            Session["Tabla_Valores"] = Dt_Tabla_Valores.Copy();
            Dt_Tabla_Valores.DefaultView.RowFilter = "ACCION <> 'BAJA'";
            Dt_Tabla_Valores.DefaultView.Sort = "ANIO DESC, CLAVE_VALOR ASC";
            Grid_Valores_Construccion.Columns[1].Visible = true;
            Grid_Valores_Construccion.Columns[6].Visible = true;
            Grid_Valores_Construccion.DataSource = Dt_Tabla_Valores;
            Grid_Valores_Construccion.PageIndex = Convert.ToInt16(Dt_Tabla_Valores.Rows.Count / 10);
            Grid_Valores_Construccion.DataBind();
            Grid_Valores_Construccion.Columns[1].Visible = false;
            Grid_Valores_Construccion.Columns[6].Visible = false;
            Txt_Anio.Text = "";
            Txt_Valor_M2.Text = "";
            Hdf_Anio.Value = "";
            hdf_Valor_M2.Value = "";
            Txt_Clave_Valor.Text = "";
            Hdf_Clave_Valor.Value = "";
            Cmb_Estado.SelectedIndex = 0;
            Hdf_Estado_Conservacion.Value = "";
            Grid_Valores_Construccion.SelectedIndex = -1;
        }
        else
        {
            Lbl_Ecabezado_Mensaje.Text = "Seleccione el valor de construcción a eliminar.";
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
    protected void Btn_Actualizar_Valor_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Valores_Construccion.SelectedIndex > -1)
            {
                if (Txt_Anio.Text.Trim() != "" && Txt_Valor_M2.Text.Trim() != "" && Cmb_Estado.SelectedIndex > 0 && Txt_Clave_Valor.Text.Trim() != "")
                {
                    DataTable Dt_Tabla_Val = (DataTable)Session["Tabla_Valores"];
                    if (Txt_Anio.Text != Hdf_Anio.Value || Convert.ToDouble(Txt_Valor_M2.Text) != Convert.ToDouble(hdf_Valor_M2.Value) || Txt_Clave_Valor.Text.Trim()!=Hdf_Clave_Valor.Value.Trim() || Cmb_Estado.SelectedValue!=Hdf_Estado_Conservacion.Value)
                    {
                        if (Txt_Anio.Text == Hdf_Anio.Value && Convert.ToDouble(Txt_Valor_M2.Text).ToString() != Convert.ToDouble(hdf_Valor_M2.Value).ToString())
                        {
                            foreach (DataRow Dr_Renglon in Dt_Tabla_Val.Rows)
                            {
                                if (Dr_Renglon["ANIO"].ToString() == Hdf_Anio.Value && Dr_Renglon["ACCION"].ToString() != "BAJA")
                                {
                                    Dr_Renglon["ANIO"] = Convert.ToInt16(Txt_Anio.Text.Trim());
                                    Dr_Renglon["VALOR_M2"] = Convert.ToDouble(Txt_Valor_M2.Text.Trim());
                                    Dr_Renglon["ESTADO_CONSERVACION"] = Cmb_Estado.SelectedValue;
                                    Dr_Renglon["CLAVE_VALOR"] = Convert.ToInt16(Txt_Clave_Valor.Text.Trim());
                                    if (Dr_Renglon["VALOR_CONSTRUCCION_ID"].ToString() != "&nbsp;" && Dr_Renglon["VALOR_CONSTRUCCION_ID"].ToString().Trim() != "")
                                    {
                                        Dr_Renglon["ACCION"] = "ACTUALIZAR";
                                    }
                                    else
                                    {
                                        Dr_Renglon["ACCION"] = "ALTA";
                                    }
                                    Grid_Valores_Construccion.SelectedIndex = -1;
                                    break;
                                }
                            }
                        }
                        else if (!Existe_Valor_Construccion(Dt_Tabla_Val, Txt_Anio.Text.Trim(), Convert.ToDouble(Txt_Valor_M2.Text).ToString(), Cmb_Estado.SelectedValue))
                        {
                            foreach (DataRow Dr_Renglon in Dt_Tabla_Val.Rows)
                            {
                                if (Dr_Renglon["ANIO"].ToString() == Hdf_Anio.Value && Dr_Renglon["VALOR_M2"].ToString() == Convert.ToDouble(hdf_Valor_M2.Value).ToString() && Dr_Renglon["ACCION"].ToString() != "BAJA")
                                {
                                    Dr_Renglon["ANIO"] = Convert.ToInt16(Txt_Anio.Text.Trim());
                                    Dr_Renglon["VALOR_M2"] = Convert.ToDouble(Txt_Valor_M2.Text.Trim());
                                    Dr_Renglon["ESTADO_CONSERVACION"] = Cmb_Estado.SelectedValue;
                                    Dr_Renglon["CLAVE_VALOR"] = Convert.ToInt16(Txt_Clave_Valor.Text.Trim());
                                    if (Dr_Renglon["VALOR_CONSTRUCCION_ID"].ToString() != "&nbsp;" && Dr_Renglon["VALOR_CONSTRUCCION_ID"].ToString().Trim() != "")
                                    {
                                        Dr_Renglon["ACCION"] = "ACTUALIZAR";
                                    }
                                    else
                                    {
                                        Dr_Renglon["ACCION"] = "ALTA";
                                    }
                                    Grid_Valores_Construccion.SelectedIndex = -1;
                                    break;
                                }
                            }
                        }
                    }
                    Session["Tabla_Valores"] = Dt_Tabla_Val.Copy();
                    Dt_Tabla_Val.DefaultView.RowFilter = "ACCION <> 'BAJA'";
                    Dt_Tabla_Val.DefaultView.Sort = "ANIO DESC, CLAVE_VALOR ASC";
                    Grid_Valores_Construccion.Columns[1].Visible = true;
                    Grid_Valores_Construccion.Columns[6].Visible = true;
                    Grid_Valores_Construccion.DataSource = Dt_Tabla_Val;
                    Grid_Valores_Construccion.PageIndex = Grid_Valores_Construccion.PageIndex;
                    Grid_Valores_Construccion.DataBind();
                    Grid_Valores_Construccion.Columns[1].Visible = false;
                    Grid_Valores_Construccion.Columns[6].Visible = false;
                    Grid_Valores_Construccion.SelectedIndex = -1;
                    Txt_Anio.Text = "";
                    Txt_Valor_M2.Text = "";
                    Hdf_Anio.Value = "";
                    hdf_Valor_M2.Value = "";
                    Txt_Clave_Valor.Text = "";
                    Hdf_Clave_Valor.Value = "";
                    Cmb_Estado.SelectedIndex = 0;
                    Hdf_Estado_Conservacion.Value = "";
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "Introduzca los datos indicados con *.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione la Calidad de Construcción a modificar.";
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Valor_Construccion
    ///DESCRIPCIÓN: Agrega registro del grid de tramos
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
        if (Txt_Anio.Text.Trim() != "" && Txt_Valor_M2.Text.Trim()!="" && Cmb_Estado.SelectedIndex>0 && Txt_Clave_Valor.Text.Trim()!="")
        {
            DataTable Dt_Tabla_Val = (DataTable)Session["Tabla_Valores"];
            if (!Existe_Valor_Construccion(Dt_Tabla_Val, Txt_Anio.Text.Trim(), Convert.ToDouble(Txt_Valor_M2.Text).ToString(), Cmb_Estado.SelectedValue))
            {
                DataRow Dr_Valor_Nuevo = Dt_Tabla_Val.NewRow();
                Dr_Valor_Nuevo["VALOR_CONSTRUCCION_ID"] = " ";
                Dr_Valor_Nuevo["ANIO"] = Convert.ToInt16(Txt_Anio.Text.Trim());
                Dr_Valor_Nuevo["VALOR_M2"] = Convert.ToDouble(Txt_Valor_M2.Text);
                Dr_Valor_Nuevo["ESTADO_CONSERVACION"] = Cmb_Estado.SelectedValue;
                Dr_Valor_Nuevo["CLAVE_VALOR"] = Convert.ToInt16(Txt_Clave_Valor.Text.Trim());
                Dr_Valor_Nuevo["ACCION"] = "ALTA";
                Dt_Tabla_Val.Rows.Add(Dr_Valor_Nuevo);
                Session["Tabla_Valores"] = Dt_Tabla_Val.Copy();
                Dt_Tabla_Val.DefaultView.RowFilter = "ACCION <> 'BAJA'";
                Dt_Tabla_Val.DefaultView.Sort = "ANIO DESC, CLAVE_VALOR ASC";
                Grid_Valores_Construccion.Columns[1].Visible = true;
                Grid_Valores_Construccion.Columns[6].Visible = true;
                Grid_Valores_Construccion.DataSource = Dt_Tabla_Val;
                Grid_Valores_Construccion.PageIndex = Convert.ToInt16(Dt_Tabla_Val.Rows.Count / 10);
                Grid_Valores_Construccion.DataBind();
                Grid_Valores_Construccion.Columns[1].Visible = false;
                Grid_Valores_Construccion.Columns[6].Visible = false;
                Txt_Anio.Text = "";
                Txt_Valor_M2.Text = "";
                Hdf_Anio.Value = "";
                hdf_Valor_M2.Value = "";
                Txt_Clave_Valor.Text = "";
                Hdf_Clave_Valor.Value = "";
                Cmb_Estado.SelectedIndex = 0;
                Hdf_Estado_Conservacion.Value = "";
                Grid_Valores_Construccion.SelectedIndex = -1;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tabla de Valores para Construcción", "alert('Introduzca datos indicados con *.')", true);
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
    protected void Grid_Valores_Construccion_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Valores_Construccion.SelectedIndex = -1;
            Txt_Anio.Text = "";
            Txt_Valor_M2.Text = "";
            Hdf_Anio.Value = "";
            hdf_Valor_M2.Value = "";
            Txt_Clave_Valor.Text = "";
            Hdf_Clave_Valor.Value = "";
            Cmb_Estado.SelectedIndex = 0;
            Hdf_Estado_Conservacion.Value = "";
            DataTable Dt_Tabla_Valores = (DataTable)Session["Tabla_Valores"];
            Grid_Valores_Construccion.Columns[1].Visible = true;
            Grid_Valores_Construccion.Columns[6].Visible = true;
            Dt_Tabla_Valores.DefaultView.RowFilter = "ACCION <> 'BAJA'";
            Dt_Tabla_Valores.DefaultView.Sort = "ANIO DESC, CLAVE_VALOR ASC";
            Grid_Valores_Construccion.DataSource = Dt_Tabla_Valores;
            Grid_Valores_Construccion.PageIndex = e.NewPageIndex;
            Grid_Valores_Construccion.DataBind();
            Grid_Valores_Construccion.Columns[1].Visible = false;
            Grid_Valores_Construccion.Columns[6].Visible = false;
        }
        catch (Exception E)
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
    protected void Grid_Valores_Construccion_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Valores_Construccion.SelectedIndex > -1)
            {
                Txt_Anio.Text = HttpUtility.HtmlDecode(Grid_Valores_Construccion.SelectedRow.Cells[2].Text);
                Hdf_Anio.Value = Txt_Anio.Text;
                Txt_Clave_Valor.Text = Convert.ToInt16(HttpUtility.HtmlDecode(Grid_Valores_Construccion.SelectedRow.Cells[3].Text.Trim())).ToString();
                Hdf_Clave_Valor.Value = Txt_Clave_Valor.Text;
                Txt_Valor_M2.Text = Convert.ToDouble(Grid_Valores_Construccion.SelectedRow.Cells[5].Text.Replace("$", "")).ToString("###,###,###,##0.00");
                hdf_Valor_M2.Value = HttpUtility.HtmlDecode(Grid_Valores_Construccion.SelectedRow.Cells[5].Text.Replace("$", "").Replace(",",""));
                Cmb_Estado.SelectedIndex = Cmb_Estado.Items.IndexOf(Cmb_Estado.Items.FindByValue(Grid_Valores_Construccion.SelectedRow.Cells[4].Text.Trim()));
                Hdf_Estado_Conservacion.Value = Cmb_Estado.SelectedValue;
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione un Valor de Construcción.";
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
        String Msj_Error = "Error: ";
        DataTable Dt_Tabla_Valores = (DataTable)Session["Tabla_Valores"];
        if (Grid_Calidad.SelectedIndex == -1)
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Seleccione un elemento de la tabla de Calidad de la construcción.";
            Valido = false;
        }
        if (Dt_Tabla_Valores.Rows.Count == 0)
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese Valores de Construcción.";
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
        if (Grid_Calidad.SelectedIndex == -1)
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Seleccione un elemento de la tabla de Calidad de la construcción.";
            Valido = false;
        }
        if (Grid_Valores_Construccion.Rows.Count == 0)
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese Valores de Construcción.";
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
    private Boolean Existe_Valor_Construccion(DataTable Dt_Tabla_Val, String Anio,String Valor_M2, String Estado_Conservacion)
    {
        Boolean Existe = false;

        foreach (DataRow Dr_Renglon in Dt_Tabla_Val.Rows)
        {
            if (Dr_Renglon["ANIO"].ToString() == Anio && Dr_Renglon["ESTADO_CONSERVACION"].ToString() == Estado_Conservacion && Dr_Renglon["ACCION"].ToString() != "BAJA")
            {
                Existe = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tabla de Valores para Construcción", "alert('Ya existe un valor de M2 para el año " + Anio + " con Estado de conservación: " + Dr_Renglon["ESTADO_CONSERVACION"].ToString() + "');", true);
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
            if (Dr_Renglon["ANIO"].ToString() == Anio && Dr_Renglon["VALOR_M2"].ToString() == Valor_M2 && Dr_Renglon["ACCION"].ToString() != "BAJA")
            {
                Existe = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tabla de Valores para Construcción", "alert('Ya existe un valor de M2 para el año " + Anio + "');", true);
                break;
            }
        }
        return Existe;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Anio_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_Anio
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 09/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Anio_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_Anio.Text.Trim() == "")
            {
                Txt_Anio.Text = "";
            }
            else
            {
                Txt_Anio.Text = Convert.ToInt16(Txt_Anio.Text.Trim()).ToString();
            }
        }
        catch
        {
            Txt_Anio.Text = "";
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

    private DataTable Crear_Dt_Tabla_Valores()
    {
        DataTable Dt_Tabla_Valores = new DataTable();
        Dt_Tabla_Valores.Columns.Add("VALOR_CONSTRUCCION_ID", typeof(String));
        Dt_Tabla_Valores.Columns.Add("ANIO", typeof(Int16));
        Dt_Tabla_Valores.Columns.Add("VALOR_M2", typeof(Double));
        Dt_Tabla_Valores.Columns.Add("CLAVE_VALOR", typeof(Int16));
        Dt_Tabla_Valores.Columns.Add("ESTADO_CONSERVACION", typeof(String));
        Dt_Tabla_Valores.Columns.Add("ACCION", typeof(String));
        return Dt_Tabla_Valores;
    }

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN: Grid_Calidad_PageIndexChanging
    /////DESCRIPCIÓN: Cambia la página del grid
    /////PROPIEDADES:     
    /////            
    /////CREO: Miguel Angel Bedolla Moreno
    /////FECHA_CREO: 05/May_2012
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Grid_Calidad_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    //Grid_Calidad.SelectedIndex = -1;
    //    //Llenar_Tabla_Calidad(e.NewPageIndex);
    //    //Txt_Calidad.Text = "";
    //    //Txt_Clave_Calidad.Text = "";
    //    Txt_Anio.Text = "";
    //    Txt_Clave_Valor.Text = "";
    //    Cmb_Estado.SelectedIndex = 0;
    //    Txt_Valor_M2.Text = "";
    //    Grid_Valores_Construccion.SelectedIndex = -1;
    //    Grid_Valores_Construccion.DataSource = null;
    //    Grid_Valores_Construccion.PageIndex = 0;
    //    Grid_Valores_Construccion.DataBind();
    //}

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN: Grid_Calidad_SelectedIndexChanged
    /////DESCRIPCIÓN: Selecciona un registro del grid y toma los datos de los mismos campos del componente
    /////PROPIEDADES:     
    /////            
    /////CREO: Miguel Angel Bedolla Moreno
    /////FECHA_CREO: 05/May_2012
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Grid_Calidad_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //if (Grid_Calidad.SelectedIndex > -1)
    //    //{
    //        Grid_Valores_Construccion.SelectedIndex = -1;
    //        //Hdf_Calidad_Id.Value = Grid_Calidad.SelectedRow.Cells[1].Text;
    //        //Txt_Calidad.Text = Grid_Calidad.SelectedRow.Cells[2].Text;
    //        //Txt_Clave_Calidad.Text = Grid_Calidad.SelectedRow.Cells[3].Text;
    //        Cls_Cat_Cat_Tabla_Valores_Construccion_Negocio Tabla_Val = new Cls_Cat_Cat_Tabla_Valores_Construccion_Negocio();
    //        //Tabla_Val.P_Calidad_Id = Hdf_Calidad_Id.Value;
    //        DataTable Dt_Tabla_Val = Tabla_Val.Consultar_Tabla_Valores_Construccion();
    //        Session["Tabla_Valores"] = Dt_Tabla_Val.Copy();
    //        Dt_Tabla_Val.DefaultView.Sort = "ANIO DESC, CLAVE_VALOR ASC";
    //        Grid_Valores_Construccion.Columns[1].Visible = true;
    //        Grid_Valores_Construccion.Columns[6].Visible = true;
    //        Grid_Valores_Construccion.DataSource = Dt_Tabla_Val;
    //        Grid_Valores_Construccion.PageIndex = 0;
    //        Grid_Valores_Construccion.DataBind();
    //        Grid_Valores_Construccion.Columns[1].Visible = false;
    //        Grid_Valores_Construccion.Columns[6].Visible = false;
    //        Txt_Anio.Text = "";
    //        Txt_Clave_Valor.Text = "";
    //        Cmb_Estado.SelectedIndex = 0;
    //        Txt_Valor_M2.Text = "";
    //    //}
    //}

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Calidad
    /////DESCRIPCIÓN: Llena la tabla de los datos de calidad
    /////PROPIEDADES:     
    /////             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    /////CREO: Miguel Angel Bedolla Moreno
    /////FECHA_CREO: 07/Mayo/2012 
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //private void Llenar_Tabla_Calidad(int Pagina)
    //{
    //    try
    //    {
    //        Cls_Cat_Cat_Tabla_Valores_Construccion_Negocio Calidad = new Cls_Cat_Cat_Tabla_Valores_Construccion_Negocio();
    //        Calidad.P_Tipo_Construccion_Id = hdf_Tipo_Construccion_Id.Value;
    //        Grid_Calidad.Columns[1].Visible = true;
    //        Grid_Calidad.DataSource = Calidad.Consultar_Calidad_Construccion();
    //        Grid_Calidad.PageIndex = Pagina;
    //        Grid_Calidad.DataBind();
    //        Grid_Calidad.Columns[1].Visible = false;
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //        Lbl_Mensaje_Error.Text = "";
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }
    //}

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Tipos_Construccion
    ///DESCRIPCIÓN: Llena el combo de Tipos de Construccion
    ///PROPIEDADES:         
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 24/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Tipos_Construccion()
    {
        try
        {
            Cls_Cat_Cat_Tipos_Construccion_Negocio Tipos_Construccion = new Cls_Cat_Cat_Tipos_Construccion_Negocio();
            Tipos_Construccion.P_Estatus = "='VIGENTE' ";
            DataTable tabla = Tipos_Construccion.Consultar_Tipos_Construccion();
            DataRow fila = tabla.NewRow();
            fila[Cat_Cat_Tipos_Construccion.Campo_Tipo_Construccion_Id] = "SELECCIONE";
            fila[Cat_Cat_Tipos_Construccion.Campo_Identificador] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            tabla.Rows.InsertAt(fila, 0);
            Cmb_Tipos_Construccion.DataSource = tabla;
            Cmb_Tipos_Construccion.DataValueField = Cat_Cat_Tipos_Construccion.Campo_Tipo_Construccion_Id;
            Cmb_Tipos_Construccion.DataTextField = Cat_Cat_Tipos_Construccion.Campo_Identificador;
            Cmb_Tipos_Construccion.DataBind();
        }
        catch (Exception Ex)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Ecabezado_Mensaje.Visible = true;
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Calidad_Construccion
    ///DESCRIPCIÓN: Llena el combo de Calidad de Construccion
    ///PROPIEDADES:         
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 24/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Calidad_Construccion(String Tipo_Construccion_Id)
    {
        try
        {
            Cls_Cat_Cat_Calidad_Construccion_Negocio Calidad_Construccion = new Cls_Cat_Cat_Calidad_Construccion_Negocio();
            Calidad_Construccion.P_Tipo_Construccion_Id = Tipo_Construccion_Id;
            DataTable tabla = Calidad_Construccion.Consultar_Calidad_Construccion();
            DataRow fila = tabla.NewRow();
            fila[Cat_Cat_Calidad_Construccion.Campo_Calidad_Id] = "SELECCIONE";
            fila[Cat_Cat_Calidad_Construccion.Campo_Calidad] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            tabla.Rows.InsertAt(fila, 0);
            Cmb_Calidad_Construccion.DataSource = tabla;
            Cmb_Calidad_Construccion.DataValueField = Cat_Cat_Calidad_Construccion.Campo_Calidad_Id;
            Cmb_Calidad_Construccion.DataTextField = Cat_Cat_Calidad_Construccion.Campo_Calidad;
            Cmb_Calidad_Construccion.DataBind();
        }
        catch (Exception Ex)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Ecabezado_Mensaje.Visible = true;
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
        }
    }
    protected void Cmb_Tipos_Construccion_SelectedIndexChanged(object sender, EventArgs e)
    {
        Llenar_Combo_Calidad_Construccion(Cmb_Tipos_Construccion.SelectedValue);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Calidad_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Calidad_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Llenar_Tabla_Calidad(e.NewPageIndex);
        Cmb_Tipos_Construccion.SelectedIndex = -1;
        Cmb_Tipos_Construccion_SelectedIndexChanged(null, null);
        Grid_Calidad.SelectedIndex = -1;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Calidad_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona un registro del grid y toma los datos de los mismos campos del componente
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Calidad_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Calidad.SelectedIndex > -1)
        {
            Cmb_Tipos_Construccion.SelectedValue = Grid_Calidad.SelectedRow.Cells[2].Text;
            Cmb_Tipos_Construccion_SelectedIndexChanged(null, null);
            Cmb_Calidad_Construccion.SelectedValue = Grid_Calidad.SelectedRow.Cells[1].Text;
            Grid_Valores_Construccion.SelectedIndex = -1;
            Cls_Cat_Cat_Tabla_Valores_Construccion_Negocio Tabla_Val = new Cls_Cat_Cat_Tabla_Valores_Construccion_Negocio();
            Tabla_Val.P_Calidad_Id = Cmb_Calidad_Construccion.SelectedValue;
            DataTable Dt_Tabla_Val = Tabla_Val.Consultar_Tabla_Valores_Construccion();
            Session["Tabla_Valores"] = Dt_Tabla_Val.Copy();
            Dt_Tabla_Val.DefaultView.Sort = "ANIO DESC, CLAVE_VALOR ASC";
            Grid_Valores_Construccion.Columns[1].Visible = true;
            Grid_Valores_Construccion.Columns[6].Visible = true;
            Grid_Valores_Construccion.DataSource = Dt_Tabla_Val;
            Grid_Valores_Construccion.PageIndex = 0;
            Grid_Valores_Construccion.DataBind();
            Grid_Valores_Construccion.Columns[1].Visible = false;
            Grid_Valores_Construccion.Columns[6].Visible = false;
            Txt_Anio.Text = "";
            Txt_Clave_Valor.Text = "";
            Cmb_Estado.SelectedIndex = 0;
            Txt_Valor_M2.Text = "";
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
    private void Llenar_Tabla_Calidad(int Pagina)
    {
        try
        {
            Cls_Cat_Cat_Calidad_Construccion_Negocio Motivos = new Cls_Cat_Cat_Calidad_Construccion_Negocio();
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Motivos.P_Calidad = Txt_Busqueda.Text.ToUpper();
            }
            Grid_Calidad.Columns[1].Visible = true;
            Grid_Calidad.Columns[2].Visible = true;
            Grid_Calidad.DataSource = Motivos.Consultar_Calidad_Construccion();
            Grid_Calidad.PageIndex = Pagina;
            Grid_Calidad.DataBind();
            Grid_Calidad.Columns[1].Visible = false;
            Grid_Calidad.Columns[2].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
}