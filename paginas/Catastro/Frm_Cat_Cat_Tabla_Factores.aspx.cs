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
using Presidencia.Catalogo_Cat_Tabla_Factores.Negocio;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Cat_Parametros.Negocio;
using Presidencia.Constantes;

public partial class paginas_Catastro_Frm_Cat_Cat_Tabla_Factores : System.Web.UI.Page
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
                Llenar_Tabla_Factores(0);
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
        Txt_Factor_Cobro.Enabled = !Enabled;
        Txt_Base_Factor_Cobro.Enabled = !Enabled;
        Txt_Base_Hasta_1_Ha.Enabled = !Enabled;
        Txt_Factor_Mayor_A_1_Ha.Enabled = !Enabled;
        Txt_Porcentaje_Perito_Externo.Enabled = !Enabled;


        Txt_Anio.Style["text-align"] = "Right";
        Txt_Factor_Cobro.Style["text-align"] = "Right";
        Txt_Base_Factor_Cobro.Style["text-align"] = "Right";
        Txt_Base_Hasta_1_Ha.Style["text-align"] = "Right";
        Txt_Factor_Mayor_A_1_Ha.Style["text-align"] = "Right";
        Txt_Porcentaje_Perito_Externo.Style["text-align"] = "Right";

        Btn_Actualizar_Valor.Enabled = !Enabled;
        Btn_Agregar_Valor.Enabled = !Enabled;
        Btn_Eliminar_Valor.Enabled = !Enabled;
        Btn_Actualizar_Valor.Enabled = !Enabled;
        Grid_Factores.Enabled = !Enabled;


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
    private void Llenar_Tabla_Factores(int Pagina)
    {
        try
        {
            DataTable Dt_Tabla_Factores;
            Cls_Cat_Cat_Tabla_Factores_Negocio Tabla_Fac = new Cls_Cat_Cat_Tabla_Factores_Negocio();
            Grid_Factores.Columns[2].Visible = true;
            Dt_Tabla_Factores = Tabla_Fac.Consultar_Tabla_Factores_Cobro_Avaluos();
            Dt_Tabla_Factores.DefaultView.Sort = "ANIO DESC";
            Grid_Factores.DataSource = Dt_Tabla_Factores;
            Grid_Factores.PageIndex = Pagina;
            Grid_Factores.DataBind();
            Grid_Factores.Columns[2].Visible = false;

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
                if (Grid_Factores.Rows.Count == 0)
                {
                    Configuracion_Formulario(false);
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.Visible = false;
                    Txt_Anio.Text = "";
                    Txt_Base_Factor_Cobro.Text = "";
                    Txt_Base_Hasta_1_Ha.Text = "";
                    Txt_Factor_Cobro.Text = "";
                    Txt_Factor_Mayor_A_1_Ha.Text = "";
                    Txt_Porcentaje_Perito_Externo.Text = "";

                    Grid_Factores.SelectedIndex = -1;
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
                Cls_Cat_Cat_Tabla_Factores_Negocio Tabla_Fac = new Cls_Cat_Cat_Tabla_Factores_Negocio();


                Tabla_Fac.P_Dt_Tabla_Factores = (DataTable)Session["Tabla_Factores"];
                if ((Tabla_Fac.Alta_Tabla_Factores_Cobro_Avaluos()))
                {
                    Div_Grid_Factores_Cobro_Avaluos.Visible = true;

                    Configuracion_Formulario(true);
                    Llenar_Tabla_Factores(Grid_Factores.PageIndex);
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Factores.SelectedIndex = -1;

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
                if (Grid_Factores.Rows.Count > 0)
                {

                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;

                    Txt_Anio.Text = "";
                    Txt_Base_Factor_Cobro.Text = "";
                    Txt_Factor_Mayor_A_1_Ha.Text = "";
                    Txt_Base_Hasta_1_Ha.Text = "";
                    Txt_Factor_Cobro.Text = "";
                    Txt_Porcentaje_Perito_Externo.Text = "";

                    Grid_Factores.Enabled = true;
                    Grid_Factores.SelectedIndex = -1;
                    DataTable Dt_Tabla_Factores;
                    Cls_Cat_Cat_Tabla_Factores_Negocio Tabla_Fac = new Cls_Cat_Cat_Tabla_Factores_Negocio();
                    Dt_Tabla_Factores = Tabla_Fac.Consultar_Tabla_Factores_Cobro_Avaluos();
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
                    Cls_Cat_Cat_Tabla_Factores_Negocio Tabla_Fac = new Cls_Cat_Cat_Tabla_Factores_Negocio();
                    Tabla_Fac.P_Dt_Tabla_Factores = (DataTable)Session["Tabla_Factores"];
                    if ((Tabla_Fac.Alta_Tabla_Factores_Cobro_Avaluos()))
                    {

                        Configuracion_Formulario(true);
                        Llenar_Tabla_Factores(Grid_Factores.PageIndex);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Nuevo.Visible = true;
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Factores.Enabled = true;
                        Grid_Factores.SelectedIndex = -1;


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
            Llenar_Tabla_Factores(Grid_Factores.PageIndex);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";


            Grid_Factores.SelectedIndex = -1;
            Txt_Anio.Text = "";
            Txt_Base_Factor_Cobro.Text = "";
            Txt_Base_Hasta_1_Ha.Text = "";
            Txt_Factor_Mayor_A_1_Ha.Text = "";
            Txt_Factor_Cobro.Text = "";
            Txt_Porcentaje_Perito_Externo.Text = "";
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
    protected void Grid_Factores_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Llenar_Tabla_Factores(e.NewPageIndex);
            Grid_Factores.SelectedIndex = -1;

            Txt_Anio.Text = "";
            Txt_Base_Factor_Cobro.Text = "";
            Txt_Base_Hasta_1_Ha.Text = "";
            Txt_Factor_Cobro.Text = "";
            Txt_Factor_Mayor_A_1_Ha.Text = "";
            Txt_Porcentaje_Perito_Externo.Text = "";

            Hdf_Anio.Value = "";
            Hdf_Base_Cobro.Value = "";
            Hdf_Factor_Cobro.Value = "";
            Hdf_Porcentaje_Perito_Externo.Value = "";
            Hdf_Factor_Mayor_1_Ha.Value = "";
            Hdf_Base_Hasta_1_Ha.Value = "";


            DataTable Dt_Tabla_Fac = (DataTable)Session["Tabla_Factores"];
            Grid_Factores.Columns[2].Visible = true;
            Dt_Tabla_Fac.DefaultView.RowFilter = "ACCION <> 'BAJA'";
            Dt_Tabla_Fac.DefaultView.Sort = "ANIO DESC";
            Grid_Factores.DataSource = Dt_Tabla_Fac;
            Grid_Factores.PageIndex = e.NewPageIndex;
            Grid_Factores.DataBind();
            Grid_Factores.Columns[2].Visible = false;
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
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
    protected void Txt_Factor_Cobro_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Cls_Cat_Cat_Parametros_Negocio Parametros = new Cls_Cat_Cat_Parametros_Negocio();
            DataTable Dt_Parametros = Parametros.Consultar_Parametros();
            Crear_Mascara(Convert.ToInt16(Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Decimales_Redondeo].ToString()));
            if (Txt_Factor_Cobro.Text.Trim() == "")
            {
                Txt_Factor_Cobro.Text = "0." + Mascara_Caracteres;
            }
            else
            {
                Txt_Factor_Cobro.Text = Convert.ToDouble(Txt_Factor_Cobro.Text.Trim()).ToString("##0." + Mascara_Caracteres);
            }
        }
        catch
        {
            Txt_Factor_Cobro.Text = "0." + Mascara_Caracteres;
        }
    }

    protected void Txt_Base_Hasta_1_Ha_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Cls_Cat_Cat_Parametros_Negocio Parametros = new Cls_Cat_Cat_Parametros_Negocio();
            DataTable Dt_Parametros = Parametros.Consultar_Parametros();
            Crear_Mascara(Convert.ToInt16(Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Decimales_Redondeo].ToString()));
            if (Txt_Base_Hasta_1_Ha.Text.Trim() == "")
            {
                Txt_Base_Hasta_1_Ha.Text = "0." + Mascara_Caracteres ;
            }
            else
            {
                Txt_Base_Hasta_1_Ha.Text = Convert.ToDouble(Txt_Base_Hasta_1_Ha.Text.Trim()).ToString("##0." + Mascara_Caracteres);
            }
        }
        catch
        {
            Txt_Base_Hasta_1_Ha.Text = "0.00";
        }
    }

    protected void Txt_Factor_Mayor_A_1_HaTextChanged(object sender, EventArgs e)
    {
        try
        {
            Cls_Cat_Cat_Parametros_Negocio Parametros = new Cls_Cat_Cat_Parametros_Negocio();
            DataTable Dt_Parametros = Parametros.Consultar_Parametros();
            Crear_Mascara(Convert.ToInt16(Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Decimales_Redondeo].ToString()));
            if (Txt_Factor_Mayor_A_1_Ha.Text.Trim() == "")
            {
                Txt_Factor_Mayor_A_1_Ha.Text = "0." + Mascara_Caracteres;
            }
            else
            {
                Txt_Factor_Mayor_A_1_Ha.Text = Convert.ToDouble(Txt_Factor_Mayor_A_1_Ha.Text.Trim()).ToString("##0." + Mascara_Caracteres);
            }
        }
        catch
        {
            Txt_Factor_Mayor_A_1_Ha.Text = "0.00";
        }
    }

    protected void Txt_Base_Factor_Cobro_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Cls_Cat_Cat_Parametros_Negocio Parametros = new Cls_Cat_Cat_Parametros_Negocio();
            DataTable Dt_Parametros = Parametros.Consultar_Parametros();
            Crear_Mascara(Convert.ToInt16(Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Decimales_Redondeo].ToString()));
            if (Txt_Base_Factor_Cobro.Text.Trim() == "")
            {
                Txt_Base_Factor_Cobro.Text = "0." + Mascara_Caracteres;
            }
            else
            {
                Txt_Base_Factor_Cobro.Text = Convert.ToDouble(Txt_Base_Factor_Cobro.Text.Trim()).ToString("##0." + Mascara_Caracteres);
            }
        }
        catch
        {
            Txt_Base_Factor_Cobro.Text = "0.00";
        }
    }
    
    protected void Txt_Porcentaje_Perito_Externo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Cls_Cat_Cat_Parametros_Negocio Parametros = new Cls_Cat_Cat_Parametros_Negocio();
            DataTable Dt_Parametros = Parametros.Consultar_Parametros();
            Crear_Mascara(Convert.ToInt16(Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Decimales_Redondeo].ToString()));
            if (Txt_Porcentaje_Perito_Externo.Text.Trim() == "")
            {
                Txt_Porcentaje_Perito_Externo.Text = "0." + Mascara_Caracteres;
            }
            else
            {
                Txt_Porcentaje_Perito_Externo.Text = Convert.ToDouble(Txt_Porcentaje_Perito_Externo.Text.Trim()).ToString("##0." + Mascara_Caracteres);
            }
        }
        catch
        {
            Txt_Porcentaje_Perito_Externo.Text = "0.00";
        }
    }


    private DataTable Crear_Dt_Tabla_Factores()
    {
        DataTable Dt_Tabla_Factores = new DataTable();
        Dt_Tabla_Factores.Columns.Add("FACTOR_COBRO_ID", typeof(String));
        Dt_Tabla_Factores.Columns.Add("ANIO", typeof(Int16));
        Dt_Tabla_Factores.Columns.Add("FACTOR_COBRO_2", typeof(Double));
        Dt_Tabla_Factores.Columns.Add("BASE_COBRO", typeof(Double));
        Dt_Tabla_Factores.Columns.Add("FACTOR_MENOR_1_HA", typeof(Double));
        Dt_Tabla_Factores.Columns.Add("FACTOR_MAYOR_1_HA", typeof(Double));
        Dt_Tabla_Factores.Columns.Add("PORCENTAJE_PE", typeof(Double));

        Dt_Tabla_Factores.Columns.Add("ACCION", typeof(String));
        return Dt_Tabla_Factores;
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
        if (Txt_Anio.Text.Trim() != ""
            && Txt_Base_Factor_Cobro.Text.Trim() != ""
            && Txt_Base_Hasta_1_Ha.Text.Trim() != ""
            && Txt_Factor_Cobro.Text.Trim() != ""
            && Txt_Factor_Mayor_A_1_Ha.Text.Trim() != ""
            && Txt_Porcentaje_Perito_Externo.Text.Trim() != "")
        {
            DataTable Dt_Tabla_Fac = (DataTable)Session["Tabla_Factores"];
            if (!Existe_Valor_Construccion(Dt_Tabla_Fac, Txt_Anio.Text.Trim()))
            {
                DataRow Dr_Valor_Nuevo = Dt_Tabla_Fac.NewRow();
                Dr_Valor_Nuevo["FACTOR_COBRO_ID"] = " ";
                Dr_Valor_Nuevo["ANIO"] = Convert.ToInt16(Txt_Anio.Text.Trim());
                Dr_Valor_Nuevo["FACTOR_COBRO_2"] = Convert.ToDouble(Txt_Factor_Cobro.Text);
                Dr_Valor_Nuevo["BASE_COBRO"] = Convert.ToDouble(Txt_Base_Factor_Cobro.Text);
                Dr_Valor_Nuevo["FACTOR_MENOR_1_HA"] = Convert.ToDouble(Txt_Base_Hasta_1_Ha.Text);
                Dr_Valor_Nuevo["FACTOR_MAYOR_1_HA"] = Convert.ToDouble(Txt_Factor_Mayor_A_1_Ha.Text);
                Dr_Valor_Nuevo["PORCENTAJE_PE"] = Convert.ToDouble(Txt_Porcentaje_Perito_Externo.Text);
                Dr_Valor_Nuevo["ACCION"] = "ALTA";
                Dt_Tabla_Fac.Rows.Add(Dr_Valor_Nuevo);
                Session["Tabla_Factores"] = Dt_Tabla_Fac.Copy();
                Dt_Tabla_Fac.DefaultView.RowFilter = "ACCION <> 'BAJA'";
                Dt_Tabla_Fac.DefaultView.Sort = "ANIO DESC";
                Grid_Factores.Columns[2].Visible = true;
                Grid_Factores.DataSource = Dt_Tabla_Fac;
                Grid_Factores.PageIndex = Convert.ToInt16(Dt_Tabla_Fac.Rows.Count / 10);
                Grid_Factores.DataBind();
                Grid_Factores.Columns[2].Visible = false;

                Txt_Anio.Text = "";
                Txt_Base_Factor_Cobro.Text = "";
                Txt_Base_Hasta_1_Ha.Text = "";
                Txt_Factor_Cobro.Text = "";
                Txt_Factor_Mayor_A_1_Ha.Text = "";
                Txt_Porcentaje_Perito_Externo.Text = "";

                Hdf_Anio.Value = "";
                Hdf_Base_Cobro.Value = "";
                Hdf_Factor_Cobro.Value = "";
                Hdf_Porcentaje_Perito_Externo.Value = "";
                Hdf_Factor_Mayor_1_Ha.Value = "";
                Hdf_Base_Hasta_1_Ha.Value = "";

                Grid_Factores.SelectedIndex = -1;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Factores de Cobro de Avaluos", "alert('Introduzca datos indicados con *.')", true);
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
            if (Grid_Factores.SelectedIndex > -1)
            {
                if (Txt_Anio.Text.Trim() != ""
                    && Txt_Base_Factor_Cobro.Text.Trim() != ""
                    && Txt_Base_Hasta_1_Ha.Text.Trim() != ""
                    && Txt_Factor_Cobro.Text.Trim() != ""
                    && Txt_Factor_Mayor_A_1_Ha.Text.Trim() != ""
                    && Txt_Porcentaje_Perito_Externo.Text.Trim() != "")
                {

                    DataTable Dt_Tabla_Fac = (DataTable)Session["Tabla_Factores"];
                    if (Txt_Anio.Text != Hdf_Anio.Value
                        || Convert.ToDouble(Txt_Factor_Cobro.Text) != Convert.ToDouble(Hdf_Factor_Cobro.Value)
                        || Convert.ToDouble(Txt_Base_Factor_Cobro.Text) != Convert.ToDouble(Hdf_Base_Cobro.Value)
                        || Convert.ToDouble(Txt_Base_Hasta_1_Ha.Text) != Convert.ToDouble(Hdf_Base_Hasta_1_Ha.Value)
                        || Convert.ToDouble(Txt_Factor_Mayor_A_1_Ha.Text) != Convert.ToDouble(Hdf_Factor_Mayor_1_Ha.Value)
                        || Convert.ToDouble(Txt_Porcentaje_Perito_Externo.Text) != Convert.ToDouble(Hdf_Porcentaje_Perito_Externo.Value))
                    {
                        if (Txt_Anio.Text == Hdf_Anio.Value && 
                            (Convert.ToDouble(Txt_Factor_Cobro.Text).ToString()!= Convert.ToDouble(Hdf_Factor_Cobro.Value).ToString() 
                                || Convert.ToDouble(Txt_Base_Factor_Cobro.Text).ToString()!= Convert.ToDouble(Hdf_Base_Cobro.Value).ToString())
                                || Convert.ToDouble(Txt_Base_Hasta_1_Ha.Text).ToString()!= Convert.ToDouble(Hdf_Base_Hasta_1_Ha.Value).ToString() 
                                || Convert.ToDouble(Txt_Factor_Mayor_A_1_Ha.Text).ToString()!= Convert.ToDouble(Hdf_Factor_Mayor_1_Ha.Value).ToString() 
                                || Convert.ToDouble(Txt_Porcentaje_Perito_Externo.Text).ToString() != Convert.ToDouble(Hdf_Porcentaje_Perito_Externo.Value).ToString())
                        {
                            foreach (DataRow Dr_Renglon in Dt_Tabla_Fac.Rows)
                            {
                                if (Dr_Renglon["ANIO"].ToString() == Hdf_Anio.Value && Dr_Renglon["ACCION"].ToString() != "BAJA")
                                {
                                    Dr_Renglon["ANIO"] = Convert.ToInt16(Txt_Anio.Text.Trim());
                                    Dr_Renglon["FACTOR_COBRO_2"] = Convert.ToDouble(Txt_Factor_Cobro.Text.Trim());
                                    Dr_Renglon["BASE_COBRO"] = Convert.ToDouble(Txt_Base_Factor_Cobro.Text.Trim());
                                    Dr_Renglon["FACTOR_MENOR_1_HA"] = Convert.ToDouble(Txt_Base_Hasta_1_Ha.Text.Trim());
                                    Dr_Renglon["FACTOR_MAYOR_1_HA"] = Convert.ToDouble(Txt_Factor_Mayor_A_1_Ha.Text.Trim());
                                    Dr_Renglon["PORCENTAJE_PE"] = Convert.ToDouble(Txt_Porcentaje_Perito_Externo.Text.Trim());
                                    if (Dr_Renglon["FACTOR_COBRO_ID"].ToString() != "&nbsp;" && Dr_Renglon["FACTOR_COBRO_ID"].ToString().Trim() != "")
                                    {
                                        Dr_Renglon["ACCION"] = "ACTUALIZAR";
                                    }
                                    else
                                    {
                                        Dr_Renglon["ACCION"] = "ALTA";
                                    }
                                    Grid_Factores.SelectedIndex = -1;
                                    break;
                                }
                            }
                        }
                        else if (!Existe_Valor_Construccion(Dt_Tabla_Fac, Txt_Anio.Text.Trim()))
                        {
                            foreach (DataRow Dr_Renglon in Dt_Tabla_Fac.Rows)
                            {
                                if (Dr_Renglon["ANIO"].ToString() == Hdf_Anio.Value
                                    && Convert.ToDouble(Dr_Renglon["FACTOR_COBRO_2"]).ToString() == Convert.ToDouble(Hdf_Factor_Cobro.Value).ToString()
                                    && Convert.ToDouble(Dr_Renglon["BASE_COBRO"]).ToString() == Convert.ToDouble(Hdf_Base_Cobro.Value).ToString()
                                    && Convert.ToDouble(Dr_Renglon["FACTOR_MENOR_1_HA"]).ToString() == Convert.ToDouble(Hdf_Base_Hasta_1_Ha.Value).ToString()
                                    && Convert.ToDouble(Dr_Renglon["FACTOR_MAYOR_1_HA"]).ToString() == Convert.ToDouble(Hdf_Factor_Mayor_1_Ha.Value).ToString()
                                    && Convert.ToDouble(Dr_Renglon["PORCENTAJE_PE"]).ToString() == Convert.ToDouble(Hdf_Porcentaje_Perito_Externo.Value).ToString()
                                    && Dr_Renglon["ACCION"].ToString() != "BAJA")
                                {
                                    Dr_Renglon["ANIO"] = Convert.ToInt16(Txt_Anio.Text.Trim());
                                    Dr_Renglon["FACTOR_MENOR_1_HA"] = Convert.ToDouble(Txt_Base_Hasta_1_Ha.Text.Trim());
                                    Dr_Renglon["FACTOR_COBRO_2"] = Convert.ToDouble(Txt_Factor_Cobro.Text.Trim());
                                    Dr_Renglon["BASE_COBRO"] = Convert.ToDouble(Txt_Base_Factor_Cobro.Text.Trim());
                                    Dr_Renglon["FACTOR_MENOR_1_HA"] = Convert.ToDouble(Txt_Base_Hasta_1_Ha.Text.Trim());
                                    Dr_Renglon["FACTOR_MAYOR_1_HA"] = Convert.ToDouble(Txt_Factor_Mayor_A_1_Ha.Text.Trim());
                                    Dr_Renglon["PORCENTAJE_PE"] = Convert.ToDouble(Txt_Porcentaje_Perito_Externo.Text.Trim());
                                    if (Dr_Renglon["FACTOR_COBRO_ID"].ToString() != "&nbsp;" && Dr_Renglon["FACTOR_COBRO_ID"].ToString().Trim() != "")
                                    {
                                        Dr_Renglon["ACCION"] = "ACTUALIZAR";
                                    }
                                    else
                                    {
                                        Dr_Renglon["ACCION"] = "ALTA";
                                    }
                                    Grid_Factores.SelectedIndex = -1;
                                    break;
                                }
                            }
                        }
                    }
                    Session["Tabla_Factores"] = Dt_Tabla_Fac.Copy();
                    Dt_Tabla_Fac.DefaultView.RowFilter = "ACCION <> 'BAJA'";
                    Dt_Tabla_Fac.DefaultView.Sort = "ANIO DESC";
                    Grid_Factores.Columns[2].Visible = true;
                    Grid_Factores.DataSource = Dt_Tabla_Fac;
                    Grid_Factores.PageIndex = Grid_Factores.PageIndex;
                    Grid_Factores.DataBind();
                    Grid_Factores.Columns[2].Visible = false;
                    Grid_Factores.SelectedIndex = -1;

                    Txt_Anio.Text = "";
                    Txt_Base_Factor_Cobro.Text = "";
                    Txt_Base_Hasta_1_Ha.Text = "";
                    Txt_Factor_Cobro.Text = "";
                    Txt_Factor_Mayor_A_1_Ha.Text = "";
                    Txt_Porcentaje_Perito_Externo.Text = "";

                    Hdf_Anio.Value = "";
                    Hdf_Base_Cobro.Value = "";
                    Hdf_Factor_Cobro.Value = "";
                    Hdf_Porcentaje_Perito_Externo.Value = "";
                    Hdf_Factor_Mayor_1_Ha.Value = "";
                    Hdf_Base_Hasta_1_Ha.Value = "";
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
        if (Grid_Factores.SelectedIndex > -1)
        {
            DataTable Dt_Tabla_Factores = (DataTable)Session["Tabla_Factores"];
            foreach (DataRow Dr_Renglon in Dt_Tabla_Factores.Rows)
            {
                if (Dr_Renglon["ANIO"].ToString() == Hdf_Anio.Value
                    && Convert.ToDouble(Dr_Renglon["FACTOR_COBRO_2"].ToString()).ToString() == Convert.ToDouble(Hdf_Factor_Cobro.Value).ToString()
                    && Convert.ToDouble(Dr_Renglon["BASE_COBRO"].ToString()).ToString() == Convert.ToDouble(Hdf_Base_Cobro.Value).ToString()
                    && Convert.ToDouble(Dr_Renglon["FACTOR_MENOR_1_HA"].ToString()).ToString() == Convert.ToDouble(Hdf_Base_Hasta_1_Ha.Value).ToString()
                    && Convert.ToDouble(Dr_Renglon["FACTOR_MAYOR_1_HA"].ToString()).ToString() == Convert.ToDouble(Hdf_Factor_Mayor_1_Ha.Value).ToString()
                    && Convert.ToDouble(Dr_Renglon["PORCENTAJE_PE"].ToString()).ToString() == Convert.ToDouble(Hdf_Porcentaje_Perito_Externo.Value).ToString()
                    && Dr_Renglon["ACCION"].ToString() != "BAJA")
                {
                    Dr_Renglon["ACCION"] = "BAJA";
                    Grid_Factores.SelectedIndex = -1;
                    break;
                }
            }
            Session["Tabla_Factores"] = Dt_Tabla_Factores.Copy();
            Dt_Tabla_Factores.DefaultView.RowFilter = "ACCION <> 'BAJA'";
            Dt_Tabla_Factores.DefaultView.Sort = "ANIO DESC";
            Grid_Factores.Columns[2].Visible = true;
            Grid_Factores.DataSource = Dt_Tabla_Factores;
            Grid_Factores.PageIndex = Convert.ToInt16(Dt_Tabla_Factores.Rows.Count / 10);
            Grid_Factores.DataBind();
            Grid_Factores.Columns[2].Visible = false;

            Txt_Anio.Text = "";
            Txt_Base_Factor_Cobro.Text = "";
            Txt_Base_Hasta_1_Ha.Text = "";
            Txt_Factor_Cobro.Text = "";
            Txt_Factor_Mayor_A_1_Ha.Text = "";
            Txt_Porcentaje_Perito_Externo.Text = "";

            Hdf_Anio.Value = "";
            Hdf_Base_Cobro.Value = "";
            Hdf_Factor_Cobro.Value = "";
            Hdf_Porcentaje_Perito_Externo.Value = "";
            Hdf_Factor_Mayor_1_Ha.Value = "";
            Hdf_Base_Hasta_1_Ha.Value = "";

            Grid_Factores.SelectedIndex = -1;
        }
        else
        {
            Lbl_Ecabezado_Mensaje.Text = "Seleccione el factor a eliminar.";
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
    protected void Grid_Factores_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Cls_Cat_Cat_Parametros_Negocio Parametros = new Cls_Cat_Cat_Parametros_Negocio();
            DataTable Dt_Parametros = Parametros.Consultar_Parametros();
            Crear_Mascara(Convert.ToInt16(Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Decimales_Redondeo].ToString()));
            if (Grid_Factores.SelectedIndex > -1)
            {
                Txt_Anio.Text = HttpUtility.HtmlDecode(Grid_Factores.SelectedRow.Cells[1].Text);
                Hdf_Anio.Value = Txt_Anio.Text;
                Txt_Factor_Cobro.Text = Convert.ToDouble(Grid_Factores.SelectedRow.Cells[3].Text.Replace("$", "")).ToString("###,###,###,##0." + Mascara_Caracteres);
                Hdf_Factor_Cobro.Value = HttpUtility.HtmlDecode(Grid_Factores.SelectedRow.Cells[3].Text.Replace("$", "").Replace(",", ""));
                Txt_Base_Factor_Cobro.Text = Convert.ToDouble(Grid_Factores.SelectedRow.Cells[4].Text.Replace("$", "")).ToString("###,###,###,##0." + Mascara_Caracteres);
                Hdf_Base_Cobro.Value = HttpUtility.HtmlDecode(Grid_Factores.SelectedRow.Cells[4].Text.Replace("$", "").Replace(",", ""));
                Txt_Base_Hasta_1_Ha.Text = Convert.ToDouble(Grid_Factores.SelectedRow.Cells[5].Text.Replace("$", "")).ToString("###,###,###,##0." + Mascara_Caracteres);
                Hdf_Base_Hasta_1_Ha.Value = HttpUtility.HtmlDecode(Grid_Factores.SelectedRow.Cells[5].Text.Replace("$", "").Replace(",", ""));
                Txt_Factor_Mayor_A_1_Ha.Text = Convert.ToDouble(Grid_Factores.SelectedRow.Cells[6].Text.Replace("$", "")).ToString("###,###,###,##0." + Mascara_Caracteres);
                Hdf_Factor_Mayor_1_Ha.Value = HttpUtility.HtmlDecode(Grid_Factores.SelectedRow.Cells[6].Text.Replace("$", "").Replace(",", ""));
                Txt_Porcentaje_Perito_Externo.Text = Convert.ToDouble(Grid_Factores.SelectedRow.Cells[7].Text.Replace("$", "")).ToString("###,###,###,##0." + Mascara_Caracteres);
                Hdf_Porcentaje_Perito_Externo.Value = HttpUtility.HtmlDecode(Grid_Factores.SelectedRow.Cells[7].Text.Replace("$", "").Replace(",", ""));

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


        if (Grid_Factores.Rows.Count == 0)
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