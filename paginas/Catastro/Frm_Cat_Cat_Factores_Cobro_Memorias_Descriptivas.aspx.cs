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
using Presidencia.Catalogo_Cat_Factores_Cobro_Memorias_Descriptivas.Negocio;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Cat_Parametros.Negocio;
using Presidencia.Constantes;

public partial class paginas_Catastro_Frm_Cat_Cat_Factores_Cobro_Memorias_Descriptivas : System.Web.UI.Page
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
                Llenar_Tabla_Valores(0);
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
    ///NOMBRE DE LA FUNCIÓN: Crear_Mascara
    ///DESCRIPCIÓN: realiza la mascara para los decimales
    ///PROPIEDADES:    
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Crear_Mascara(Int16 Cantidad_Decimales)
    {
        Mascara_Caracteres = "";
        if (Cantidad_Decimales > 0)
        {
            for (int i = 0; i < Cantidad_Decimales; i++)
            {
                Mascara_Caracteres += "0";
            }
        }
    }

    String Mascara_Caracteres;
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
        Txt_Cantidad_Cobro1.Enabled = !Enabled;
        Txt_Cantidad_Cobro2.Enabled = !Enabled;
        Txt_Anio.Style["text-align"] = "Right";
        Txt_Cantidad_Cobro1.Style["text-align"] = "Right";
        Txt_Cantidad_Cobro2.Style["text-align"] = "Right";
        Btn_Actualizar_Valor.Enabled = !Enabled;
        Btn_Agregar_Valor.Enabled = !Enabled;
        Btn_Eliminar_Valor.Enabled = !Enabled;
        Btn_Actualizar_Valor.Enabled = !Enabled;
        Grid_Valores.Enabled = !Enabled;


    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Tipos_Construccion_Rustico
    ///DESCRIPCIÓN: Llena la tabla de Tipos de Construccion Rústico
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Valores(int Pagina)
    {
        try
        {
            DataTable Dt_Tabla_Factores;
            Cls_Cat_Cat_Factores_Cobro_Memorias_Descriptivas_Negocio Tabla_Fac = new Cls_Cat_Cat_Factores_Cobro_Memorias_Descriptivas_Negocio();
            Grid_Valores.Columns[2].Visible = true;
            Dt_Tabla_Factores = Tabla_Fac.Consulta_Factores_Cobro_Memorias_Descriptivas();
            Dt_Tabla_Factores.DefaultView.Sort = "ANIO DESC";
            Grid_Valores.DataSource = Dt_Tabla_Factores;
            Grid_Valores.PageIndex = Pagina;
            Grid_Valores.DataBind();
            Grid_Valores.Columns[2].Visible = false;
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
                    Txt_Cantidad_Cobro1.Text = "";
                    Txt_Cantidad_Cobro2.Text = "";
                   

                    Grid_Valores.SelectedIndex = -1;


                    Session["Tabla_Factores"] = Crear_Dt_Tabla_Factores();
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
                Cls_Cat_Cat_Factores_Cobro_Memorias_Descriptivas_Negocio Tabla_Fac = new Cls_Cat_Cat_Factores_Cobro_Memorias_Descriptivas_Negocio();


                Tabla_Fac.P_Dt_Valores_Cobro = (DataTable)Session["Tabla_Factores"];
                if ((Tabla_Fac.Alta_Factores_Cobro_Memorias_Descriptivas()))
                {
                    Div_Grid_Valores_Cobro_Memorias_Descriptivas.Visible = true;

                    Configuracion_Formulario(true);
                    Llenar_Tabla_Valores(Grid_Valores.PageIndex);
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Valores.SelectedIndex = -1;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Factores de Cobro de Avaluos", "alert('Alta Exitosa');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Factores de Cobro de Avaluos", "alert('Alta Errónea');", true);
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
                if (Grid_Valores.Rows.Count > 0)
                {
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                    Txt_Anio.Text = "";
                    Txt_Cantidad_Cobro1.Text = "";
                    Txt_Cantidad_Cobro2.Text = "";
                    Grid_Valores.Enabled = true;
                    Grid_Valores.SelectedIndex = -1;
                    DataTable Dt_Tabla_Factores;
                    Cls_Cat_Cat_Factores_Cobro_Memorias_Descriptivas_Negocio Tabla_Fac = new Cls_Cat_Cat_Factores_Cobro_Memorias_Descriptivas_Negocio();
                    Dt_Tabla_Factores = Tabla_Fac.Consulta_Factores_Cobro_Memorias_Descriptivas();
                    Session["Tabla_Factores"] = Dt_Tabla_Factores.Copy();                    
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
                    Cls_Cat_Cat_Factores_Cobro_Memorias_Descriptivas_Negocio Tabla_Fac = new Cls_Cat_Cat_Factores_Cobro_Memorias_Descriptivas_Negocio();
                    Tabla_Fac.P_Dt_Valores_Cobro = (DataTable)Session["Tabla_Factores"];
                    if ((Tabla_Fac.Alta_Factores_Cobro_Memorias_Descriptivas()))
                    {
                        Configuracion_Formulario(true);
                        Llenar_Tabla_Valores(Grid_Valores.PageIndex);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Nuevo.Visible = true;
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Valores.Enabled = true;
                        Grid_Valores.SelectedIndex = -1;
                        Grid_Valores.Enabled = false;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Factores de Cobro de Avaluos", "alert('Actualización Exitosa.');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Factores de Cobro de Avaluos", "alert('Error al intentar Actualizar.');", true);
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
            Llenar_Tabla_Valores(Grid_Valores.PageIndex);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Grid_Valores.SelectedIndex = -1;
            Txt_Anio.Text = "";
            Txt_Cantidad_Cobro1.Text = "";
            Txt_Cantidad_Cobro2.Text = "";
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Tipos_Construccion_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid
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
            Llenar_Tabla_Valores(e.NewPageIndex);
            Grid_Valores.SelectedIndex = -1;
            Txt_Anio.Text = "";
            Txt_Cantidad_Cobro1.Text = "";
            Txt_Cantidad_Cobro2.Text = "";
            Hdf_Anio.Value = "";
            Hdf_Cantidad_Cobro1.Value = "";
            Hdf_Cantidad_Cobro2.Value = "";        
            DataTable Dt_Tabla_Fac = (DataTable)Session["Tabla_Factores"];
            Grid_Valores.Columns[2].Visible = true;
            Grid_Valores.Columns[5].Visible = true;
            Dt_Tabla_Fac.DefaultView.RowFilter = "ACCION <> 'BAJA'";
            Dt_Tabla_Fac.DefaultView.Sort = "ANIO DESC";
            Grid_Valores.DataSource = Dt_Tabla_Fac;
            Grid_Valores.PageIndex = e.NewPageIndex;
            Grid_Valores.DataBind();
            Grid_Valores.Columns[2].Visible = false;
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
    ///NOMBRE DE LA FUNCIÓN: Txt_Cantidad_Cobro1_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_Cantidad_Cobro1
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 09/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Cantidad_Cobro1_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Cls_Cat_Cat_Parametros_Negocio Parametros = new Cls_Cat_Cat_Parametros_Negocio();
            DataTable Dt_Parametros = Parametros.Consultar_Parametros();
            Crear_Mascara(Convert.ToInt16(Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Decimales_Redondeo].ToString()));
            if (Txt_Cantidad_Cobro1.Text.Trim() == "")
            {
                Txt_Cantidad_Cobro1.Text = "0." + Mascara_Caracteres;
            }
            else
            {
                Txt_Cantidad_Cobro1.Text = Convert.ToDouble(Txt_Cantidad_Cobro1.Text.Trim()).ToString("#,###,###,###,##0." + Mascara_Caracteres);
            }
        }
        catch
        {
            Txt_Cantidad_Cobro1.Text = "0.000000" + Mascara_Caracteres;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Cantidad_Cobro1_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_Cantidad_Cobro1
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 09/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Cantidad_Cobro2_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Cls_Cat_Cat_Parametros_Negocio Parametros = new Cls_Cat_Cat_Parametros_Negocio();
            DataTable Dt_Parametros = Parametros.Consultar_Parametros();
            Crear_Mascara(Convert.ToInt16(Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Decimales_Redondeo].ToString()));
            if (Txt_Cantidad_Cobro2.Text.Trim() == "")
            {
                Txt_Cantidad_Cobro2.Text = "0." + Mascara_Caracteres;
            }
            else
            {
                Txt_Cantidad_Cobro2.Text = Convert.ToDouble(Txt_Cantidad_Cobro2.Text.Trim()).ToString("#,###,###,###,##0." + Mascara_Caracteres);
            }
        }
        catch
        {
            Txt_Cantidad_Cobro2.Text = "0.000000";
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Crear_Dt_Tabla_Factores
    ///DESCRIPCIÓN: 
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 09/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private DataTable Crear_Dt_Tabla_Factores()
    {
        DataTable Dt_Tabla_Factores = new DataTable();
        Dt_Tabla_Factores.Columns.Add("FACTOR_COBRO_MEM_ID", typeof(String));
        Dt_Tabla_Factores.Columns.Add("ANIO", typeof(Int16));
        Dt_Tabla_Factores.Columns.Add("CANTIDAD_COBRO_1", typeof(Double));
        Dt_Tabla_Factores.Columns.Add("CANTIDAD_COBRO_2", typeof(Double));
        Dt_Tabla_Factores.Columns.Add("ACCION", typeof(String));
        return Dt_Tabla_Factores;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Valor_Click
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
        if (Txt_Anio.Text.Trim() != "" && Txt_Cantidad_Cobro1.Text.Trim() != "" && Txt_Cantidad_Cobro2.Text.Trim() != "")
        {
            DataTable Dt_Tabla_Fac = (DataTable)Session["Tabla_Factores"];
            if (!Existe_Valor_Construccion(Dt_Tabla_Fac, Txt_Anio.Text.Trim()))
            {
                DataRow Dr_Valor_Nuevo = Dt_Tabla_Fac.NewRow();
                Dr_Valor_Nuevo["FACTOR_COBRO_MEM_ID"] = " ";
                Dr_Valor_Nuevo["ANIO"] = Convert.ToInt16(Txt_Anio.Text.Trim());
                Dr_Valor_Nuevo["CANTIDAD_COBRO_1"] = Convert.ToDouble(Txt_Cantidad_Cobro1.Text);
                Dr_Valor_Nuevo["CANTIDAD_COBRO_2"] = Convert.ToDouble(Txt_Cantidad_Cobro2.Text);
                Dr_Valor_Nuevo["ACCION"] = "ALTA";
                Dt_Tabla_Fac.Rows.Add(Dr_Valor_Nuevo);
                Session["Tabla_Factores"] = Dt_Tabla_Fac.Copy();
                Dt_Tabla_Fac.DefaultView.RowFilter = "ACCION <> 'BAJA'";
                Dt_Tabla_Fac.DefaultView.Sort = "ANIO DESC";
                Grid_Valores.Columns[2].Visible = true;
                Grid_Valores.Columns[5].Visible = true;
                Grid_Valores.DataSource = Dt_Tabla_Fac;
                Grid_Valores.PageIndex = Convert.ToInt16(Dt_Tabla_Fac.Rows.Count / 10);
                Grid_Valores.DataBind();
                Grid_Valores.Columns[2].Visible = false;
                Grid_Valores.Columns[5].Visible = false;
                Txt_Anio.Text = "";
                Txt_Cantidad_Cobro1.Text = "";
                Txt_Cantidad_Cobro2.Text = "";
                Hdf_Anio.Value = "";
                Hdf_Cantidad_Cobro1.Value = "";
                Hdf_Cantidad_Cobro2.Value = "";
                Grid_Valores.SelectedIndex = -1;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Factores de Cobro de Avaluos", "alert('Introduzca datos indicados con *.')", true);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Actualizar_Valor_Click
    ///DESCRIPCIÓN: reacciona al evento del btn_Actualizar
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
                if (Txt_Anio.Text.Trim() != "" && Txt_Cantidad_Cobro1.Text.Trim() != "" && Txt_Cantidad_Cobro2.Text.Trim() != "")
                {

                    DataTable Dt_Tabla_Fac = (DataTable)Session["Tabla_Factores"];
                    if (Txt_Anio.Text != Hdf_Anio.Value || Convert.ToDouble(Txt_Cantidad_Cobro1.Text) != Convert.ToDouble(Hdf_Cantidad_Cobro1.Value) || Convert.ToDouble(Txt_Cantidad_Cobro2.Text) != Convert.ToDouble(Hdf_Cantidad_Cobro2.Value))
                    {
                        if (Txt_Anio.Text == Hdf_Anio.Value && (Convert.ToDouble(Txt_Cantidad_Cobro1.Text).ToString() != Convert.ToDouble(Hdf_Cantidad_Cobro1.Value).ToString() || Convert.ToDouble(Txt_Cantidad_Cobro2.Text).ToString() != Convert.ToDouble(Hdf_Cantidad_Cobro2.Value).ToString()))
                        {
                            foreach (DataRow Dr_Renglon in Dt_Tabla_Fac.Rows)
                            {
                                if (Dr_Renglon["ANIO"].ToString() == Hdf_Anio.Value && Dr_Renglon["ACCION"].ToString() != "BAJA")
                                {
                                    Dr_Renglon["ANIO"] = Convert.ToInt16(Txt_Anio.Text.Trim());
                                    Dr_Renglon["CANTIDAD_COBRO_1"] = Convert.ToDouble(Txt_Cantidad_Cobro1.Text.Trim());
                                    Dr_Renglon["CANTIDAD_COBRO_2"] = Convert.ToDouble(Txt_Cantidad_Cobro2.Text.Trim());
                                    if (Dr_Renglon["FACTOR_COBRO_MEM_ID"].ToString() != "&nbsp;" && Dr_Renglon["FACTOR_COBRO_MEM_ID"].ToString().Trim() != "")
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
                        else if (!Existe_Valor_Construccion(Dt_Tabla_Fac, Txt_Anio.Text.Trim()))
                        {
                            foreach (DataRow Dr_Renglon in Dt_Tabla_Fac.Rows)
                            {
                                if (Dr_Renglon["ANIO"].ToString() == Hdf_Anio.Value && Convert.ToDouble(Dr_Renglon["CANTIDAD_COBRO_1"]).ToString() == Convert.ToDouble(Hdf_Cantidad_Cobro1.Value).ToString() && Convert.ToDouble(Dr_Renglon["CANTIDAD_COBRO_2"]).ToString() == Convert.ToDouble(Hdf_Cantidad_Cobro2.Value).ToString()  && Dr_Renglon["ACCION"].ToString() != "BAJA")
                                {
                                    Dr_Renglon["ANIO"] = Convert.ToInt16(Txt_Anio.Text.Trim());
                                    Dr_Renglon["CANTIDAD_COBRO_1"] = Convert.ToDouble(Txt_Cantidad_Cobro1.Text.Trim());
                                    Dr_Renglon["CANTIDAD_COBRO_2"] = Convert.ToDouble(Txt_Cantidad_Cobro2.Text.Trim());
                                    if (Dr_Renglon["FACTOR_COBRO_MEM_ID"].ToString() != "&nbsp;" && Dr_Renglon["FACTOR_COBRO_MEM_ID"].ToString().Trim() != "")
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
                    Session["Tabla_Factores"] = Dt_Tabla_Fac.Copy();
                    Dt_Tabla_Fac.DefaultView.RowFilter = "ACCION <> 'BAJA'";
                    Dt_Tabla_Fac.DefaultView.Sort = "ANIO DESC";
                    Grid_Valores.Columns[2].Visible = true;
                    Grid_Valores.Columns[5].Visible = true;
                    Grid_Valores.DataSource = Dt_Tabla_Fac;
                    Grid_Valores.PageIndex = Grid_Valores.PageIndex;
                    Grid_Valores.DataBind();
                    Grid_Valores.Columns[2].Visible = false;
                    Grid_Valores.Columns[5].Visible = false;
                    Grid_Valores.SelectedIndex = -1;
                    Txt_Anio.Text = "";
                    Txt_Cantidad_Cobro1.Text = "";
                    Txt_Cantidad_Cobro2.Text = "";
                    Hdf_Anio.Value = "";
                    Hdf_Cantidad_Cobro1.Value = "";
                    Hdf_Cantidad_Cobro2.Value = "";

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
                Lbl_Ecabezado_Mensaje.Text = "Seleccione el factor a modificar.";
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
        if (Grid_Valores.SelectedIndex > -1)
        {
            DataTable Dt_Tabla_Factores = (DataTable)Session["Tabla_Factores"];
            foreach (DataRow Dr_Renglon in Dt_Tabla_Factores.Rows)
            {
                if (Dr_Renglon["ANIO"].ToString() == Hdf_Anio.Value && Convert.ToDouble(Dr_Renglon["CANTIDAD_COBRO_1"].ToString()).ToString() == Convert.ToDouble(Hdf_Cantidad_Cobro1.Value).ToString() && Convert.ToDouble(Dr_Renglon["CANTIDAD_COBRO_2"].ToString()).ToString() == Convert.ToDouble(Hdf_Cantidad_Cobro2.Value).ToString() && Dr_Renglon["ACCION"].ToString() != "BAJA")
                {
                    Dr_Renglon["ACCION"] = "BAJA";
                    Grid_Valores.SelectedIndex = -1;
                    break;
                }
            }
            Session["Tabla_Factores"] = Dt_Tabla_Factores.Copy();
            Dt_Tabla_Factores.DefaultView.RowFilter = "ACCION <> 'BAJA'";
            Dt_Tabla_Factores.DefaultView.Sort = "ANIO DESC";
            Grid_Valores.Columns[2].Visible = true;
            Grid_Valores.Columns[6].Visible = true;
            Grid_Valores.DataSource = Dt_Tabla_Factores;
            Grid_Valores.PageIndex = Convert.ToInt16(Dt_Tabla_Factores.Rows.Count / 10);
            Grid_Valores.DataBind();
            Grid_Valores.Columns[2].Visible = false;
            Grid_Valores.Columns[6].Visible = false;
            Txt_Anio.Text = "";
            Txt_Cantidad_Cobro1.Text = "";
            Txt_Cantidad_Cobro2.Text = "";
            Hdf_Anio.Value = "";
            Hdf_Cantidad_Cobro1.Value = "";
            Hdf_Cantidad_Cobro2.Value = "";
            Grid_Valores.SelectedIndex = -1;
        }
        else
        {
            Lbl_Ecabezado_Mensaje.Text = "Seleccione el factor a eliminar a eliminar.";
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
    private Boolean Existe_Valor_Construccion(DataTable Dt_Tabla_Fac, String Anio)
    {
        Boolean Existe = false;

        foreach (DataRow Dr_Renglon in Dt_Tabla_Fac.Rows)
        {
            if (Dr_Renglon["ANIO"].ToString() == Anio && Dr_Renglon["ACCION"].ToString() != "BAJA")
            {
                Existe = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Factores de Cobro de Avaluos", "alert('Ya existe un factor de cobro para el año " + Anio + "');", true);
                break;
            }
        }
        return Existe;
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
                Txt_Anio.Text = HttpUtility.HtmlDecode(Grid_Valores.SelectedRow.Cells[1].Text);
                Hdf_Anio.Value = Txt_Anio.Text;
                Txt_Cantidad_Cobro1.Text = Convert.ToDouble(Grid_Valores.SelectedRow.Cells[3].Text.Replace("$", "")).ToString("#,##0.000");
                Hdf_Cantidad_Cobro1.Value = HttpUtility.HtmlDecode(Grid_Valores.SelectedRow.Cells[3].Text.Replace("$", "").Replace(",", ""));
                Txt_Cantidad_Cobro2.Text = Convert.ToDouble(Grid_Valores.SelectedRow.Cells[4].Text.Replace("$", "")).ToString("#,##0.000");
                Hdf_Cantidad_Cobro2.Value = HttpUtility.HtmlDecode(Grid_Valores.SelectedRow.Cells[4].Text.Replace("$", "").Replace(",", ""));         
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione un Factor de Cobro de Avaluos.";
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
        DataTable Dt_Tabla_Factores = (DataTable)Session["Tabla_Factores"];
        if (Dt_Tabla_Factores.Rows.Count == 0)
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese Factores.";
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
        if (Grid_Valores.Rows.Count == 0)
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese Factores .";
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
