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
using Presidencia.Constantes;
using Presidencia.Catalogo_Cat_Tabla_Descrip_Rustico.Negocio;
using Presidencia.Catalogo_Cat_Descripcion_Construccion_Rustico.Negocio;

public partial class paginas_Catastro_Frm_Cat_Cat_Tabla_Descrip_Rustico : System.Web.UI.Page
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
                //Llenar_Tabla_Valores_Rustico(0);
                Llenar_Combo_Descripcion_Construccion();
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
        Txt_Valor_Indice.Enabled = !Enabled;
        Txt_Indicador_A.Enabled = !Enabled;
        //Txt_Indicador_B.Enabled = !Enabled;
        //Txt_Indicador_C.Enabled = !Enabled;
        //Txt_Indicador_D.Enabled = !Enabled;
        Btn_Actualizar_Valor.Enabled = !Enabled;
        Btn_Agregar_Valor.Enabled = !Enabled;
        Btn_Eliminar_Valor.Enabled = !Enabled;
        Cmb_Descripcion_Construccion.Enabled = Enabled;
        Txt_Anio.Style["text-align"] = "Right";
        Txt_Valor_Indice.Style["text-align"] = "Right";
        Txt_Indicador_A.Style["text-align"] = "Right";
        //Txt_Indicador_B.Style["text-align"] = "Right";
        //Txt_Indicador_C.Style["text-align"] = "Right";
        //Txt_Indicador_D.Style["text-align"] = "Right";
    
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Descrip_Rustico
    ///DESCRIPCIÓN: Llena el grid de Valores con los datos que sean introducidos
    ///PROPIEDADES:    
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Descrip_Rustico(int Pagina)
    {
        try
        {
            Cls_Cat_Cat_Tabla_Descrip_Rustico_Negocio Valores_Rustica = new Cls_Cat_Cat_Tabla_Descrip_Rustico_Negocio();
            DataTable Dt_Tabla_Valores_Rustica;

            Dt_Tabla_Valores_Rustica = Valores_Rustica.Consultar_Tabla_Valores_Rustico();
            Grid_Valores.Columns[1].Visible = true;
            Grid_Valores.Columns[7].Visible = true;
            Grid_Valores.DataSource = Dt_Tabla_Valores_Rustica;
            Grid_Valores.PageIndex = Pagina;
            Grid_Valores.DataBind();
            Grid_Valores.Columns[1].Visible = false;
            Grid_Valores.Columns[7].Visible = false;
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
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Descripcion_Construccion
    ///DESCRIPCIÓN: Lena el cmb de Descripciones 
    ///PROPIEDADES:     
    ///           
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Descripcion_Construccion()
    {
        try
        {
            Cls_Cat_Cat_Descripcion_Construccion_Rustico_Negocio Descripciones = new Cls_Cat_Cat_Descripcion_Construccion_Rustico_Negocio();
            DataTable tabla = Descripciones.Consultar_Descripcion_Construccion_Rustico();
            DataRow fila = tabla.NewRow();
            fila[Cat_Cat_Descrip_Const_Rustico.Campo_Desc_Constru_Rustico_Id] = "SELECCIONE";
            fila[Cat_Cat_Descrip_Const_Rustico.Campo_Identificador] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            tabla.Rows.InsertAt(fila, 0);
            Cmb_Descripcion_Construccion.DataSource = tabla;
            Cmb_Descripcion_Construccion.DataValueField = Cat_Cat_Descrip_Const_Rustico.Campo_Desc_Constru_Rustico_Id;
            Cmb_Descripcion_Construccion.DataTextField = Cat_Cat_Descrip_Const_Rustico.Campo_Identificador;
            Cmb_Descripcion_Construccion.DataBind();


        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Campos
    ///DESCRIPCIÓN: Limpia campos del formulario al hacer el posback
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Campos()
    {

        Txt_Anio.Text = "";
        Txt_Valor_Indice.Text = "";
        Txt_Indicador_A.Text = "";
        //Txt_Indicador_B.Text = "";
        //Txt_Indicador_C.Text = "";
        //Txt_Indicador_D.Text = "";

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
                        Limpiar_Campos();


                        Grid_Valores.SelectedIndex = -1;



                        Div_Grid_Tab_Val.Visible = true;
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
                    Cls_Cat_Cat_Tabla_Descrip_Rustico_Negocio Tabla_Val = new Cls_Cat_Cat_Tabla_Descrip_Rustico_Negocio();
                    Tabla_Val.P_Des_Constru_Rustico_Id = Cmb_Descripcion_Construccion.SelectedValue;
                    Tabla_Val.P_Dt_Tabla_Descrip_Rustico = (DataTable)Session["Tabla_Valores"];
                    if ((Tabla_Val.Alta_Valor_Rustico()))
                    {

                        Div_Grid_Tab_Val.Visible = false;
                        Configuracion_Formulario(true);
                        //Llenar_Tabla_Tipos_Construccion_Rustico(Grid_Tipos_Construccion.PageIndex);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Modificar.Visible = true;
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";

                        Grid_Valores.SelectedIndex = -1;
                        Cmb_Descripcion_Construccion.SelectedIndex = -1;

                        Grid_Valores.DataSource = null;
                        Grid_Valores.DataBind();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tabla de Descripción Rústica ", "alert('Alta Exitosa');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tabla de Descripción Rústica ", "alert('Alta Errónea');", true);
                    }
                }
            //}
            //else
            //{
            //    Lbl_Ecabezado_Mensaje.Text = "Selecciones un Tipo de Construccion.";
            //    Lbl_Mensaje_Error.Text = "";
            //    Div_Contenedor_Msj_Error.Visible = true;
            //}
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

                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;

                    Limpiar_Campos();
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
                    Cls_Cat_Cat_Tabla_Descrip_Rustico_Negocio Tabla_Val = new Cls_Cat_Cat_Tabla_Descrip_Rustico_Negocio();
                    Tabla_Val.P_Des_Constru_Rustico_Id = Cmb_Descripcion_Construccion.SelectedValue;
                    Tabla_Val.P_Dt_Tabla_Descrip_Rustico = (DataTable)Session["Tabla_Valores"];
                    if ((Tabla_Val.Modificar_Valor_Rustico()))
                    {

                        Div_Grid_Tab_Val.Visible = false;
                        Configuracion_Formulario(true);
                        //   Llenar_Tabla_Tipos_Construccion_Rustico(Grid_Tipos_Construccion.PageIndex);
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
                        Cmb_Descripcion_Construccion.SelectedIndex = -1;
                        Limpiar_Campos();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tabla de Descripción Rústica ", "alert('Actualización Exitosa.');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tabla de Descripción Rústica ", "alert('Error al intentar Actualizar.');", true);
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
            //Llenar_Tabla_Tipos_Construccion_Rustico(Grid_Tipos_Construccion.PageIndex);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            //Div_Grid_Tipos_Construccion.Visible = true;
            //Div_Datos_Construccion.Visible = false;
            Div_Grid_Tab_Val.Visible = false;
            Grid_Valores.DataSource = null;
            Grid_Valores.DataBind();
            //Grid_Tipos_Construccion.SelectedIndex = -1;
            Grid_Valores.SelectedIndex = -1;
            //Txt_Tipo_Construccion.Text = "";

            Limpiar_Campos();
            Cmb_Descripcion_Construccion.SelectedIndex = -1;
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
    //protected void Grid_Valores_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
    ///NOMBRE DE LA FUNCIÓN: Txt_Valor_Indice_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_Valor_Indice
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 09/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Valor_Indice_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_Valor_Indice.Text.Trim() == "")
            {
                Txt_Valor_Indice.Text = "0.00";
            }
            else
            {
                Txt_Valor_Indice.Text = Convert.ToDouble(Txt_Valor_Indice.Text.Trim()).ToString("##0.00");
            }
        }
        catch
        {
            Txt_Valor_Indice.Text = "0.00";
        }
    }

    private DataTable Crear_Dt_Tabla_Valores()
    {
        DataTable Dt_Tabla_Valores = new DataTable();
        Dt_Tabla_Valores.Columns.Add("DESCRIPCION_RUSTICO_ID", typeof(String));
        Dt_Tabla_Valores.Columns.Add("ANIO", typeof(Int16));
        Dt_Tabla_Valores.Columns.Add("VALOR_INDICE", typeof(Double));
        Dt_Tabla_Valores.Columns.Add("INDICADOR_A", typeof(String));
        Dt_Tabla_Valores.Columns.Add("INDICADOR_B", typeof(String));
        Dt_Tabla_Valores.Columns.Add("INDICADOR_C", typeof(String));
        Dt_Tabla_Valores.Columns.Add("INDICADOR_D", typeof(String));
        Dt_Tabla_Valores.Columns.Add("ACCION", typeof(String));
        return Dt_Tabla_Valores;
    }



   

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Valor_Construccion
    ///DESCRIPCIÓN: Agrega registro del grid de Valores
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
        if (Txt_Anio.Text.Trim() != "" && Txt_Valor_Indice.Text.Trim() != "")
        {
            DataTable Dt_Tabla_Val = (DataTable)Session["Tabla_Valores"];
            if (!Existe_Valor_Rustico(Dt_Tabla_Val, Txt_Anio.Text.Trim(), Convert.ToDouble(Txt_Valor_Indice.Text).ToString()))
            {
                DataRow Dr_Valor_Nuevo = Dt_Tabla_Val.NewRow();
                Dr_Valor_Nuevo["DESCRIPCION_RUSTICO_ID"] = " ";
                Dr_Valor_Nuevo["ANIO"] = Convert.ToInt16(Txt_Anio.Text.Trim());
                Dr_Valor_Nuevo["VALOR_INDICE"] = Convert.ToDouble(Txt_Valor_Indice.Text);
                Dr_Valor_Nuevo["INDICADOR_A"] = Txt_Indicador_A.Text.Trim().ToUpper();
                //Dr_Valor_Nuevo["INDICADOR_B"] = Txt_Indicador_B.Text;
                //Dr_Valor_Nuevo["INDICADOR_C"] = Txt_Indicador_C.Text;
                //Dr_Valor_Nuevo["INDICADOR_D"] = Txt_Indicador_D.Text;
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
                Limpiar_Campos();
                Hdf_Anio.Value = "";
                hdf_Valor_Indice.Value = "";
                Grid_Valores.SelectedIndex = -1;

            }
        }
        else
        {
            Lbl_Ecabezado_Mensaje.Text = "Introduzca los datos indicados con *.";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Actualizar_Tramo_Click
    ///DESCRIPCIÓN: Modifica un registro del grid de Valores
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
                if (Txt_Anio.Text.Trim() != "" && Txt_Valor_Indice.Text.Trim() != "")
                {
                    DataTable Dt_Tabla_Val = (DataTable)Session["Tabla_Valores"];
                    if (Txt_Anio.Text != Hdf_Anio.Value || Convert.ToDouble(Txt_Valor_Indice.Text) != Convert.ToDouble(hdf_Valor_Indice.Value) || Txt_Indicador_A.Text != Hdf_Indicador_A.Value )
                    {
                        if (Txt_Anio.Text == Hdf_Anio.Value || Convert.ToDouble(Txt_Valor_Indice.Text).ToString() != Convert.ToDouble(hdf_Valor_Indice.Value).ToString() || Txt_Indicador_A.Text != Hdf_Indicador_A.Value )
                        {
                            foreach (DataRow Dr_Renglon in Dt_Tabla_Val.Rows)
                            {
                                if (Dr_Renglon["ANIO"].ToString() == Hdf_Anio.Value && Dr_Renglon["ACCION"].ToString() != "BAJA")
                                {
                                    Dr_Renglon["ANIO"] = Convert.ToInt16(Txt_Anio.Text.Trim());
                                    Dr_Renglon["VALOR_INDICE"] = Convert.ToDouble(Txt_Valor_Indice.Text.Trim());
                                    Dr_Renglon["INDICADOR_A"] = Txt_Indicador_A.Text;
                                    //Dr_Renglon["INDICADOR_B"] = Txt_Indicador_B.Text;
                                    //Dr_Renglon["INDICADOR_C"] = Txt_Indicador_C.Text;
                                    //Dr_Renglon["INDICADOR_D"] = Txt_Indicador_D.Text;
                                    if (Dr_Renglon["DESCRIPCION_RUSTICO_ID"].ToString() != "&nbsp;" && Dr_Renglon["DESCRIPCION_RUSTICO_ID"].ToString().Trim() != "")
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
                        else if (!Existe_Valor_Rustico(Dt_Tabla_Val, Txt_Anio.Text.Trim(), Convert.ToDouble(Txt_Valor_Indice.Text).ToString()))
                        {
                            foreach (DataRow Dr_Renglon in Dt_Tabla_Val.Rows)
                            {
                                if (Dr_Renglon["ANIO"].ToString() == Hdf_Anio.Value && Dr_Renglon["VALOR_INDICE"].ToString() == Convert.ToDouble(hdf_Valor_Indice.Value).ToString() && Dr_Renglon["ACCION"].ToString() != "BAJA")
                                {
                                    Dr_Renglon["ANIO"] = Convert.ToInt16(Txt_Anio.Text.Trim());
                                    Dr_Renglon["VALOR_INDICE"] = Convert.ToDouble(Txt_Valor_Indice.Text.Trim());
                                    Dr_Renglon["INDICADOR_A"] = Txt_Indicador_A.Text;
                                    //Dr_Renglon["INDICADOR_B"] = Txt_Indicador_B.Text;
                                    //Dr_Renglon["INDICADOR_C"] = Txt_Indicador_C.Text;
                                    //Dr_Renglon["INDICADOR_D"] = Txt_Indicador_D.Text;
                                    if (Dr_Renglon["DESCRIPCION_RUSTICO_ID"].ToString() != "&nbsp;" && Dr_Renglon["DESCRIPCION_RUSTICO_ID"].ToString().Trim() != "")
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
                    Limpiar_Campos();
                    Hdf_Anio.Value = "";
                    hdf_Valor_Indice.Value = "";
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
                Lbl_Ecabezado_Mensaje.Text = "Seleccione el año  al que desea modificar su valores.";
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
    ///DESCRIPCIÓN: Da de baja un registro del grid de valores
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
                if (Dr_Renglon["ANIO"].ToString() == Hdf_Anio.Value && Convert.ToDouble(Dr_Renglon["VALOR_INDICE"].ToString()).ToString() == Convert.ToDouble(hdf_Valor_Indice.Value).ToString() && Dr_Renglon["ACCION"].ToString() != "BAJA")
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
            Limpiar_Campos();
            Hdf_Anio.Value = "";
            hdf_Valor_Indice.Value = "";
            Grid_Valores.SelectedIndex = -1;
        }
        else
        {
            Lbl_Ecabezado_Mensaje.Text = "Seleccione el año que desea eliminar.";
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
    private Boolean Existe_Valor_Rustico(DataTable Dt_Tabla_Val, String Anio, String Valor_Idice )
    {
        Boolean Existe = false;

        foreach (DataRow Dr_Renglon in Dt_Tabla_Val.Rows)
        {
            if (Dr_Renglon["ANIO"].ToString() == Anio && Dr_Renglon["INDICADOR_A"].ToString().ToUpper().Trim() == Txt_Indicador_A.Text.Trim().ToUpper() && Dr_Renglon["ACCION"].ToString() != "BAJA")
            {
                Existe = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tabla de Descripción Rústica ", "alert('Ya existe identificador para este año " + Anio + "');", true);
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
            Limpiar_Campos();
            Hdf_Indicador_A.Value = "";
            //Hdf_Indicador_B.Value = "";
            //Hdf_Indicador_C.Value = "";
            //Hdf_Indicador_D.Value = "";
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
    ///DESCRIPCIÓN: Selecciona un elemento del grid de Valores y toma sus valores correspondientes
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
                Txt_Valor_Indice.Text = Convert.ToDouble(Grid_Valores.SelectedRow.Cells[4].Text).ToString("##0.00");
                hdf_Valor_Indice.Value = HttpUtility.HtmlDecode(Grid_Valores.SelectedRow.Cells[4].Text);
                if (Grid_Valores.SelectedRow.Cells[3].Text != "&nbsp;")
                {
                    Hdf_Indicador_A.Value = Grid_Valores.SelectedRow.Cells[3].Text;
                    Txt_Indicador_A.Text = Grid_Valores.SelectedRow.Cells[3].Text;
                }
                else
                {
                    Hdf_Indicador_A.Value = "";
                    Txt_Indicador_A.Text = "";
                }
                //if (Grid_Valores.SelectedRow.Cells[5].Text != "&nbsp;")
                //{
                //    Hdf_Indicador_B.Value = Grid_Valores.SelectedRow.Cells[5].Text;
                //    Txt_Indicador_B.Text = Grid_Valores.SelectedRow.Cells[5].Text;
                //}
                //else
                //{
                //    Hdf_Indicador_B.Value = "";
                //    Txt_Indicador_B.Text = "";
                //}
                //if (Grid_Valores.SelectedRow.Cells[6].Text != "&nbsp;")
                //{
                //    Hdf_Indicador_C.Value = Grid_Valores.SelectedRow.Cells[6].Text;
                //    Txt_Indicador_C.Text = Grid_Valores.SelectedRow.Cells[6].Text;
                //}
                //else
                //{
                //    Hdf_Indicador_C.Value = "";
                //    Txt_Indicador_C.Text = "";
                //}
                //if (Grid_Valores.SelectedRow.Cells[7].Text != "&nbsp;")
                //{

                //    Hdf_Indicador_D.Value = Grid_Valores.SelectedRow.Cells[7].Text;
                //    Txt_Indicador_D.Text = Grid_Valores.SelectedRow.Cells[7].Text;
                //}
                //else
                //{
                //    Hdf_Indicador_D.Value = "";
                //    Txt_Indicador_D.Text = "";
                //}
                
                
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione un Valor de Construcción Rústico.";
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
        if (Cmb_Descripcion_Construccion.SelectedValue == "SELECCIONE")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Seleccione en estatus del tipo de construcción Rústico.";
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
        //if (Txt_Tipo_Construccion.Text.Trim() == "")
        //{
        //    Msj_Error += "<br/>";
        //    Msj_Error += "+ Ingrese el Tipo de Construcción.";
        //    Valido = false;
        //}
        if (Cmb_Descripcion_Construccion.SelectedValue == "SELECCIONE")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Seleccione una descripción.";
            Valido = false;
        }
        if (Grid_Valores.Rows.Count == 0)
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese Detalles de construccion.";
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
    ///NOMBRE DE LA FUNCIÓN: Cmb_Descripcion_Construccion_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona un elemento del cmb de Descripciones y toma sus valores correspondientes
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Descripcion_Construccion_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable Dt_Tabla_Valores;
        Cls_Cat_Cat_Tabla_Descrip_Rustico_Negocio Tab_Val = new Cls_Cat_Cat_Tabla_Descrip_Rustico_Negocio();
        Tab_Val.P_Des_Constru_Rustico_Id = Cmb_Descripcion_Construccion.SelectedValue;
        Dt_Tabla_Valores = Tab_Val.Consultar_Tabla_Valores_Rustico();
        Session["Tabla_Valores"] = Dt_Tabla_Valores.Copy();
        Grid_Valores.Columns[1].Visible = true;
        Grid_Valores.Columns[5].Visible = true;
        Dt_Tabla_Valores.DefaultView.Sort = "ANIO DESC";
        Grid_Valores.DataSource = Dt_Tabla_Valores;
        Grid_Valores.PageIndex = 0;
        Grid_Valores.DataBind();
        Grid_Valores.Columns[1].Visible = false;
        Grid_Valores.Columns[5].Visible = false;
        //Div_Grid_Tipos_Construccion.Visible = false;
        //Div_Datos_Construccion.Visible = true;
        Div_Grid_Tab_Val.Visible = true;
        Btn_Salir.AlternateText = "Atras";
    }
}