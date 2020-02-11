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
using Presidencia.Sessiones;
using Presidencia.Catalogo_Cat_Tabla_Valores_Catastrales.Negocio;
using Presidencia.Constantes;

public partial class paginas_Catastro_Frm_Cat_Cat_Tabla_Valores_Catastrales : System.Web.UI.Page
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
            Llenar_Tabla_Valores_Catastrales(0);
                //Llenar_Combo_Tipos();
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
        Txt_Cantidad_1.Enabled = !Enabled;
        Txt_Cantidad_2.Enabled = !Enabled;
        Btn_Actualizar_Valor.Enabled = !Enabled;
        Btn_Agregar_Valor.Enabled = !Enabled;
        Btn_Eliminar_Valor.Enabled = !Enabled;
        //Cmb_Tipos_Construccion.Enabled = Enabled;
        Txt_Anio.Style["text-align"] = "Right";
        Txt_Cantidad_1.Style["text-align"] = "Right";
        Txt_Cantidad_2.Style["text-align"] = "Right";
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Valores_Catastrales
    ///DESCRIPCIÓN: Llena la tabla de Tipos de Construccion Rústico
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Valores_Catastrales(int Pagina)
    {
        try{

            DataTable Dt_Tabla_Valores;
            Cls_Cat_Cat_Tabla_Valores_Catastrales_Negocio Tabla_Val = new Cls_Cat_Cat_Tabla_Valores_Catastrales_Negocio();

            Grid_Valores.Columns[1].Visible = true;
            Grid_Valores.Columns[5].Visible = true;
            Dt_Tabla_Valores = Tabla_Val.Consultar_Tabla_Valores_Catastrales();
            Dt_Tabla_Valores.DefaultView.Sort = "ANIO DESC";
            Grid_Valores.DataSource = Dt_Tabla_Valores;
            Grid_Valores.PageIndex = Pagina;
            Grid_Valores.DataBind();
            Grid_Valores.Columns[1].Visible = false;
            Grid_Valores.Columns[5].Visible = false;

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
            //if (Cmb_Tipos_Construccion.SelectedValue != "SELECCIONE")
            //{
                if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
                {
                    if (Grid_Valores.Rows.Count == 0)
                    {
                        Configuracion_Formulario(false);
                        Btn_Nuevo.AlternateText = "Dar de Alta";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                        Btn_Salir.AlternateText = "Cancelar";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Modificar.Visible = false;
                        Txt_Anio.Text = "";
                        Txt_Cantidad_1.Text = "";
                        Txt_Cantidad_2.Text = "";

                        Grid_Valores.SelectedIndex = -1;



                        //Div_Grid_Tab_Val.Visible = true;
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
                    Cls_Cat_Cat_Tabla_Valores_Catastrales_Negocio Tabla_Val = new Cls_Cat_Cat_Tabla_Valores_Catastrales_Negocio();
                    //Tabla_Val.P_Tipo_Constru_Rustico_Id = Cmb_Tipos_Construccion.SelectedValue;
                    Tabla_Val.P_Dt_Tabla_Valores_Catastrales = (DataTable)Session["Tabla_Valores"];
                    if ((Tabla_Val.Alta_Valor_Catastral()))
                    {

                        //Div_Grid_Tab_Val.Visible = false;
                        Configuracion_Formulario(true);
                        
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Modificar.Visible = true;
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";

                        Grid_Valores.SelectedIndex = -1;
                        //Cmb_Tipos_Construccion.SelectedIndex = -1;

                        Grid_Valores.DataSource = null;
                        Grid_Valores.DataBind();
                        Llenar_Tabla_Valores_Catastrales(Grid_Valores.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tabla de Valores Catastrales", "alert('Alta Exitosa');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tabla de Valores Catastrales", "alert('Alta Errónea');", true);
                    }
                }
            //}
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Selecciones un Valor Catastral.";
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

            if (Btn_Modificar.AlternateText.Equals("Modificar"))
            {

                if (Grid_Valores.Rows.Count > 0)
                {
                    Cls_Cat_Cat_Tabla_Valores_Catastrales_Negocio Tabla_Val = new Cls_Cat_Cat_Tabla_Valores_Catastrales_Negocio();
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                    Session["Tabla_Valores"] = Tabla_Val.Consultar_Tabla_Valores_Catastrales();
                    Txt_Anio.Text = "";
                    Txt_Cantidad_1.Text = "";
                    Txt_Cantidad_2.Text = "";
                    Grid_Valores.SelectedIndex = -1;

                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "No hay registros en la tabla de valores que modificar.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Cat_Cat_Tabla_Valores_Catastrales_Negocio Tabla_Val = new Cls_Cat_Cat_Tabla_Valores_Catastrales_Negocio();
                    Tabla_Val.P_Valor_Catastral_Id = Hdf_Valor_Contruccion_Id.Value;
                    Tabla_Val.P_Dt_Tabla_Valores_Catastrales = (DataTable)Session["Tabla_Valores"];
                    if ((Tabla_Val.Modificar_Valor_Catastral()))
                    {

                        //Div_Grid_Tab_Val.Visible = false;
                        Configuracion_Formulario(true);
                        
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Nuevo.Visible = true;
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        //Grid_Tipos_Construccion.Enabled = true;
                        //Grid_Tipos_Construccion.SelectedIndex = -1;
                        Grid_Valores.SelectedIndex = -1;

                        //Txt_Tipo_Construccion.Text = "";
                        //Cmb_Estatus.SelectedIndex = 0;
                        Grid_Valores.DataSource = null;
                        Grid_Valores.DataBind();
                        //Cmb_Tipos_Construccion.SelectedIndex = -1;
                        Txt_Anio.Text = "";
                        Txt_Cantidad_1.Text = "";
                        Txt_Cantidad_2.Text = "";
                        Llenar_Tabla_Valores_Catastrales(Grid_Valores.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tabla de Valores Catastrales", "alert('Actualización Exitosa.');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tabla de Valores Catastrales", "alert('Error al intentar Actualizar.');", true);
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
            Llenar_Tabla_Valores_Catastrales(Grid_Valores.PageIndex);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            //Div_Grid_Tipos_Construccion.Visible = true;
            //Div_Datos_Construccion.Visible = false;
            //Div_Grid_Tab_Val.Visible = false;
            Grid_Valores.DataSource = null;
            Grid_Valores.DataBind();
            //Grid_Tipos_Construccion.SelectedIndex = -1;
            Grid_Valores.SelectedIndex = -1;
            //Txt_Tipo_Construccion.Text = "";
            Txt_Cantidad_1.Text = "";
            Txt_Cantidad_2.Text = "";
            Txt_Anio.Text = "";
            //Cmb_Tipos_Construccion.SelectedIndex = -1;
            //Cmb_Estatus.SelectedIndex = 0;
        }
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
    //        // Llenar_Tabla_Tipos_Construccion_Rustico(e.NewPageIndex);
    //    }
    //    catch (Exception E)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = E.Message;
    //        Lbl_Mensaje_Error.Text = "";
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }
    //}


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Cantidad_1_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_Cantidad_1
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 09/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Cantidad_1_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_Cantidad_1.Text.Trim() == "")
            {
                Txt_Cantidad_1.Text = "0.00";
            }
            else
            {
                Txt_Cantidad_1.Text = Convert.ToDouble(Txt_Cantidad_1.Text.Trim()).ToString("#,###,###,##0.00");
            }
        }
        catch
        {
            Txt_Cantidad_1.Text = "0.00";
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Cantidad_2_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_Cantidad_2
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 09/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Cantidad_2_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_Cantidad_2.Text.Trim() == "")
            {
                Txt_Cantidad_2.Text = "0.00";
            }
            else
            {
                Txt_Cantidad_2.Text = Convert.ToDouble(Txt_Cantidad_2.Text.Trim()).ToString("#,###,###,##0.00");
            }
        }
        catch
        {
            Txt_Cantidad_2.Text = "0.00";
        }
    }

    private DataTable Crear_Dt_Tabla_Valores()
    {
        DataTable Dt_Tabla_Valores = new DataTable();
        Dt_Tabla_Valores.Columns.Add("VALOR_CATASTRAL_ID", typeof(String));
        Dt_Tabla_Valores.Columns.Add("ANIO", typeof(Int16));
        Dt_Tabla_Valores.Columns.Add("CANTIDAD_1", typeof(Double));
        Dt_Tabla_Valores.Columns.Add("CANTIDAD_2", typeof(Double));
        Dt_Tabla_Valores.Columns.Add("ACCION", typeof(String));
        return Dt_Tabla_Valores;
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
        if (Txt_Anio.Text.Trim() != "" && Txt_Cantidad_1.Text.Trim() != "" && Txt_Cantidad_2.Text.Trim() != "")
        {
            DataTable Dt_Tabla_Val = (DataTable)Session["Tabla_Valores"];
            if (!Existe_Valor_Construccion(Dt_Tabla_Val, Txt_Anio.Text.Trim()))
            {
                DataRow Dr_Valor_Nuevo = Dt_Tabla_Val.NewRow();
                Dr_Valor_Nuevo["VALOR_CATASTRAL_ID"] = " ";
                Dr_Valor_Nuevo["ANIO"] = Convert.ToInt16(Txt_Anio.Text.Trim());
                Dr_Valor_Nuevo["CANTIDAD_1"] = Convert.ToDouble(Txt_Cantidad_1.Text);
                Dr_Valor_Nuevo["CANTIDAD_2"] = Convert.ToDouble(Txt_Cantidad_2.Text);
                Dr_Valor_Nuevo["ACCION"] = "ALTA";
                Dt_Tabla_Val.Rows.Add(Dr_Valor_Nuevo);
                Session["Tabla_Valores"] = Dt_Tabla_Val.Copy();
                Dt_Tabla_Val.DefaultView.RowFilter = "ACCION <> 'BAJA'";
                Dt_Tabla_Val.DefaultView.Sort = "ANIO DESC";
                Grid_Valores.Columns[1].Visible = true;
                Grid_Valores.Columns[5].Visible = true;
                Grid_Valores.DataSource = Dt_Tabla_Val;
                Grid_Valores.PageIndex = Convert.ToInt16(Dt_Tabla_Val.Rows.Count / 10);
                Grid_Valores.DataBind();
                Grid_Valores.Columns[1].Visible = false;
                Grid_Valores.Columns[5].Visible = false;
                Txt_Anio.Text = "";
                Txt_Cantidad_1.Text = "";
                Txt_Cantidad_2.Text = "";
                Hdf_Anio.Value = "";
                hdf_Txt_Cantidad_1.Value = "";
                hdf_Txt_Cantidad_2.Value = "";
                Grid_Valores.SelectedIndex = -1;

            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tabla de Valores Catastrales", "alert('Introduzca datos indicados con *.')", true);
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
            if (Grid_Valores.SelectedIndex > -1)
            {
                if (Txt_Anio.Text.Trim() != "" && Txt_Cantidad_1.Text.Trim() != "" && Txt_Cantidad_2.Text.Trim() != "")
                {
                    DataTable Dt_Tabla_Val = (DataTable)Session["Tabla_Valores"];
                    if (Txt_Anio.Text != Hdf_Anio.Value || Convert.ToDouble(Txt_Cantidad_1.Text) != Convert.ToDouble(hdf_Txt_Cantidad_1.Value) || Convert.ToDouble(Txt_Cantidad_2.Text) != Convert.ToDouble(hdf_Txt_Cantidad_2.Value))
                    {
                        if (Txt_Anio.Text == Hdf_Anio.Value || Convert.ToDouble(Txt_Cantidad_1.Text).ToString() != Convert.ToDouble(hdf_Txt_Cantidad_1.Value).ToString() || Convert.ToDouble(Txt_Cantidad_2.Text).ToString() != Convert.ToDouble(hdf_Txt_Cantidad_2.Value).ToString())
                        {
                            foreach (DataRow Dr_Renglon in Dt_Tabla_Val.Rows)
                            {
                                if (Dr_Renglon["ANIO"].ToString() == Hdf_Anio.Value && Dr_Renglon["ACCION"].ToString() != "BAJA")
                                {
                                    Dr_Renglon["ANIO"] = Convert.ToInt16(Txt_Anio.Text.Trim());
                                    Dr_Renglon["CANTIDAD_1"] = Convert.ToDouble(Txt_Cantidad_1.Text.Trim());
                                    Dr_Renglon["CANTIDAD_2"] = Convert.ToDouble(Txt_Cantidad_2.Text.Trim());
                                    if (Dr_Renglon["VALOR_CATASTRAL_ID"].ToString() != "&nbsp;" && Dr_Renglon["VALOR_CATASTRAL_ID"].ToString().Trim() != "")
                                    {
                                        Dr_Renglon["ACCION"] = "ACTUALIZAR";
                                    }
                                    else
                                    {
                                        Dr_Renglon["ACCION"] = "ALTA";
                                    }
                                    Grid_Valores.SelectedIndex = -1;
                                    break;
                                }
                            }
                        }
                        else if (!Existe_Valor_Construccion(Dt_Tabla_Val, Txt_Anio.Text.Trim()))
                        {
                            foreach (DataRow Dr_Renglon in Dt_Tabla_Val.Rows)
                            {
                                if (Dr_Renglon["ANIO"].ToString() == Hdf_Anio.Value && Dr_Renglon["CANTIDAD_1"].ToString() == Convert.ToDouble(hdf_Txt_Cantidad_1.Value).ToString() && Dr_Renglon["CANTIDAD_2"].ToString() == Convert.ToDouble(hdf_Txt_Cantidad_2.Value).ToString() && Dr_Renglon["ACCION"].ToString() != "BAJA")
                                {
                                    Dr_Renglon["ANIO"] = Convert.ToInt16(Txt_Anio.Text.Trim());
                                    Dr_Renglon["CANTIDAD_1"] = Convert.ToDouble(Txt_Cantidad_1.Text.Trim());
                                    Dr_Renglon["CANTIDAD_2"] = Convert.ToDouble(Txt_Cantidad_2.Text.Trim());
                                    if (Dr_Renglon["VALOR_CATASTRAL_ID"].ToString() != "&nbsp;" && Dr_Renglon["VALOR_CATASTRAL_ID"].ToString().Trim() != "")
                                    {
                                        Dr_Renglon["ACCION"] = "ACTUALIZAR";
                                    }
                                    else
                                    {
                                        Dr_Renglon["ACCION"] = "ALTA";
                                    }
                                    Grid_Valores.SelectedIndex = -1;
                                    break;
                                }
                            }
                        }
                    }
                    Session["Tabla_Valores"] = Dt_Tabla_Val.Copy();
                    Dt_Tabla_Val.DefaultView.RowFilter = "ACCION <> 'BAJA'";
                    Dt_Tabla_Val.DefaultView.Sort = "ANIO DESC";
                    Grid_Valores.Columns[1].Visible = true;
                    Grid_Valores.Columns[5].Visible = true;
                    Grid_Valores.DataSource = Dt_Tabla_Val;
                    Grid_Valores.PageIndex = Grid_Valores.PageIndex;
                    Grid_Valores.DataBind();
                    Grid_Valores.Columns[1].Visible = false;
                    Grid_Valores.Columns[5].Visible = false;
                    Grid_Valores.SelectedIndex = -1;
                    Txt_Anio.Text = "";
                    Txt_Cantidad_1.Text = "";
                    Txt_Cantidad_2.Text = "";
                    Hdf_Anio.Value = "";
                    hdf_Txt_Cantidad_1.Value = "";
                    hdf_Txt_Cantidad_2.Value = "";
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
                Lbl_Ecabezado_Mensaje.Text = "Seleccione el año a modificar.";
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
        if (Grid_Valores.SelectedIndex > -1)
        {
            DataTable Dt_Tabla_Valores = (DataTable)Session["Tabla_Valores"];
            foreach (DataRow Dr_Renglon in Dt_Tabla_Valores.Rows)
            {
                if (Dr_Renglon["ANIO"].ToString() == Hdf_Anio.Value && Convert.ToDouble(Dr_Renglon["CANTIDAD_1"].ToString()).ToString() == Convert.ToDouble(hdf_Txt_Cantidad_1.Value).ToString() && Convert.ToDouble(Dr_Renglon["CANTIDAD_2"].ToString()).ToString() == Convert.ToDouble(hdf_Txt_Cantidad_2.Value).ToString() && Dr_Renglon["ACCION"].ToString() != "BAJA")
                {
                    Dr_Renglon["ACCION"] = "BAJA";
                    Grid_Valores.SelectedIndex = -1;
                    break;
                }
            }
            Session["Tabla_Valores"] = Dt_Tabla_Valores.Copy();
            Dt_Tabla_Valores.DefaultView.RowFilter = "ACCION <> 'BAJA'";
            Dt_Tabla_Valores.DefaultView.Sort = "ANIO DESC";
            Grid_Valores.Columns[1].Visible = true;
            Grid_Valores.Columns[5].Visible = true;
            Grid_Valores.DataSource = Dt_Tabla_Valores;
            Grid_Valores.PageIndex = Convert.ToInt16(Dt_Tabla_Valores.Rows.Count / 10);
            Grid_Valores.DataBind();
            Grid_Valores.Columns[1].Visible = false;
            Grid_Valores.Columns[5].Visible = false;
            Txt_Anio.Text = "";
            Txt_Cantidad_1.Text = "";
            Txt_Cantidad_2.Text = "";
            Hdf_Anio.Value = "";
            hdf_Txt_Cantidad_1.Value = "";
            hdf_Txt_Cantidad_2.Value = "";
            Grid_Valores.SelectedIndex = -1;
        }
        else
        {
            Lbl_Ecabezado_Mensaje.Text = "Seleccione el año a eliminar.";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Existe_Valor_Construccion
    ///DESCRIPCIÓN: Determina si los datos ingresado ya existen en el grid de Valores.
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Existe_Valor_Construccion(DataTable Dt_Tabla_Val, String Anio)
    {
        Boolean Existe = false;

        foreach (DataRow Dr_Renglon in Dt_Tabla_Val.Rows)
        {
            if (Dr_Renglon["ANIO"].ToString() == Anio && Dr_Renglon["ACCION"].ToString() != "BAJA")
            {
                Existe = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tabla de Valores Catastrales", "alert('Ya existen valores para el año " + Anio + "');", true);
                break;
            }
        }
        return Existe;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Valores_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid de Valores
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Valores_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Valores.SelectedIndex = -1;
            Txt_Anio.Text = "";
            Txt_Cantidad_1.Text = "";
            Txt_Cantidad_2.Text = "";
            Hdf_Anio.Value = "";
            hdf_Txt_Cantidad_1.Value = "";
            hdf_Txt_Cantidad_2.Value = "";
            DataTable Dt_Tabla_Valores = (DataTable)Session["Tabla_Valores"];
            Grid_Valores.Columns[1].Visible = true;
            Grid_Valores.Columns[5].Visible = true;
            Dt_Tabla_Valores.DefaultView.RowFilter = "ACCION <> 'BAJA'";
            Dt_Tabla_Valores.DefaultView.Sort = "ANIO DESC";
            Grid_Valores.DataSource = Dt_Tabla_Valores;
            Grid_Valores.PageIndex = e.NewPageIndex;
            Grid_Valores.DataBind();
            Grid_Valores.Columns[1].Visible = false;
            Grid_Valores.Columns[5].Visible = false;
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Valores_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona un elemento del grid de tramos y toma sus valores correspondientes
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Valores_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Valores.SelectedIndex > -1)
            {
                Txt_Anio.Text = HttpUtility.HtmlDecode(Grid_Valores.SelectedRow.Cells[2].Text);
                Hdf_Anio.Value = Txt_Anio.Text;
                Txt_Cantidad_1.Text = Convert.ToDouble(Grid_Valores.SelectedRow.Cells[3].Text.Replace("$", "")).ToString("###,###,###,##0.00");
                hdf_Txt_Cantidad_1.Value = HttpUtility.HtmlDecode(Grid_Valores.SelectedRow.Cells[3].Text.Replace("$", "").Replace(",", ""));
                Txt_Cantidad_2.Text = Convert.ToDouble(Grid_Valores.SelectedRow.Cells[4].Text.Replace("$", "")).ToString("###,###,###,##0.00");
                hdf_Txt_Cantidad_2.Value = HttpUtility.HtmlDecode(Grid_Valores.SelectedRow.Cells[4].Text.Replace("$", "").Replace(",", ""));
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione un Valor Catastral.";
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
        //if (Txt_Tipo_Construccion.Text.Trim() == "")
        //{
        //    Msj_Error += "<br/>";
        //    Msj_Error += "+ Ingrese el Tipo de Construcción.";
        //    Valido = false;
        //}
        //if (Cmb_Tipos_Construccion.SelectedValue == "SELECCIONE")
        //{
        //    Msj_Error += "<br/>";
        //    Msj_Error += "+ Seleccione en estatus del tipo de construcción Rústico.";
        //    Valido = false;
        //}
        if (Dt_Tabla_Valores.Rows.Count == 0)
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese Valores Catastrales.";
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
        //if (Txt_Tipo_Construccion.Text.Trim() == "")
        //{
        //    Msj_Error += "<br/>";
        //    Msj_Error += "+ Ingrese el Tipo de Construcción.";
        //    Valido = false;
        //}
        //if (Cmb_Tipos_Construccion.SelectedValue == "SELECCIONE")
        //{
        //    Msj_Error += "<br/>";
        //    Msj_Error += "+ Seleccione en estatus del tipo de construcción Rústico.";
        //    Valido = false;
        //}
        if (Grid_Valores.Rows.Count == 0)
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

    //protected void Cmb_Tipos_Construccion_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    DataTable Dt_Tabla_Valores;
    //    Cls_Cat_Cat_Tabla_Valores_Construccion_Rustico_Negocio Tab_Val = new Cls_Cat_Cat_Tabla_Valores_Construccion_Rustico_Negocio();
    //    Tab_Val.P_Tipo_Constru_Rustico_Id = Cmb_Tipos_Construccion.SelectedValue;
    //    Dt_Tabla_Valores = Tab_Val.Consultar_Tabla_Valores_Construccion_Rustico();
    //    Session["Tabla_Valores"] = Dt_Tabla_Valores.Copy();
    //    Grid_Valores.Columns[1].Visible = true;
    //    Grid_Valores.Columns[4].Visible = true;
    //    Dt_Tabla_Valores.DefaultView.Sort = "ANIO DESC";
    //    Grid_Valores.DataSource = Dt_Tabla_Valores;
    //    Grid_Valores.PageIndex = 0;
    //    Grid_Valores.DataBind();
    //    Grid_Valores.Columns[1].Visible = false;
    //    Grid_Valores.Columns[4].Visible = false;
    //    //Div_Grid_Tipos_Construccion.Visible = false;
    //    //Div_Datos_Construccion.Visible = true;
    //    Div_Grid_Tab_Val.Visible = true;
    //    Btn_Salir.AlternateText = "Atras";
    //}
}
