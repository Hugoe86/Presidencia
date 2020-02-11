using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Cat_Motivos_Avaluo.Negocio;

public partial class paginas_Catastro_Frm_Cat_Cat_Motivos_Avaluo : System.Web.UI.Page
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
                Llenar_Tabla_Motivos_Avaluo(0);
                Txt_Motivo_Avaluo_Descripcion.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,250)");
                Txt_Motivo_Avaluo_Descripcion.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,250)");
                Txt_Motivo_Avaluo_Descripcion.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,250)");
                Txt_Motivo_Avaluo_Descripcion.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,250)");
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
    private void Configuracion_Formulario(Boolean Habilitado)
    {
        Txt_Motivo_Avaluo_Descripcion.Enabled = !Habilitado;
        Cmb_Estatus.Enabled = !Habilitado;
        Grid_Motivos_Avaluo.Enabled = Habilitado;
        Btn_Buscar.Enabled = Habilitado;
        Txt_Busqueda.Enabled = Habilitado;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Motivos_Rechazo
    ///DESCRIPCIÓN: Llena la tabla de Motivos de Rechazo
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 07/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Motivos_Avaluo(int Pagina)
    {
        try
        {
            Cls_Cat_Cat_Motivos_Avaluo_Negocio Motivos = new Cls_Cat_Cat_Motivos_Avaluo_Negocio();
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Motivos.P_Motivo_Avaluo_Descripcion = Txt_Busqueda.Text.ToUpper();
            }
            Grid_Motivos_Avaluo.Columns[1].Visible = true;
            Grid_Motivos_Avaluo.DataSource = Motivos.Consultar_Motivos_Avaluo();
            Grid_Motivos_Avaluo.PageIndex = Pagina;
            Grid_Motivos_Avaluo.DataBind();
            Grid_Motivos_Avaluo.Columns[1].Visible = false;
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
                Configuracion_Formulario(false);
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.Visible = false;
                Txt_Motivo_Avaluo_Descripcion.Text = "";
                Cmb_Estatus.SelectedIndex = 1;
            }
            else if (Validar_Componentes())
            {
                Cls_Cat_Cat_Motivos_Avaluo_Negocio Motivo_Avaluo = new Cls_Cat_Cat_Motivos_Avaluo_Negocio();
                Motivo_Avaluo.P_Estatus = Cmb_Estatus.SelectedValue;
                Motivo_Avaluo.P_Motivo_Avaluo_Descripcion = Txt_Motivo_Avaluo_Descripcion.Text.ToUpper();
                if ((Motivo_Avaluo.Alta_Motivo_Avaluo()))
                {
                    Txt_Busqueda.Text = "";
                    Llenar_Tabla_Motivos_Avaluo(0);
                    Configuracion_Formulario(true);
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Motivos_Avaluo.SelectedIndex = -1;
                    Cmb_Estatus.SelectedIndex = 0;
                    Txt_Motivo_Avaluo_Descripcion.Text = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Motivos de Avalúo", "alert('Alta de Motivo de Avalúo Exitosa');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Avalúo", "alert('Alta de Motivo de Avalúo Errónea');", true);
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
            if (Grid_Motivos_Avaluo.SelectedIndex > -1)
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
                        Cls_Cat_Cat_Motivos_Avaluo_Negocio Motivo_Avaluo = new Cls_Cat_Cat_Motivos_Avaluo_Negocio();
                        Motivo_Avaluo.P_Estatus = Cmb_Estatus.SelectedValue;
                        Motivo_Avaluo.P_Motivo_Avaluo_Descripcion = Txt_Motivo_Avaluo_Descripcion.Text.ToUpper();
                        Motivo_Avaluo.P_Motivo_Avaluo_ID = Hdf_Motivo_Avaluo_Id.Value;
                        if ((Motivo_Avaluo.Modificar_Motivo_Avaluo()))
                        {
                            Txt_Busqueda.Text = "";
                            Configuracion_Formulario(true);
                            Llenar_Tabla_Motivos_Avaluo(Grid_Motivos_Avaluo.PageIndex);
                            Btn_Modificar.AlternateText = "Modificar";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            Btn_Nuevo.Visible = true;
                            Btn_Nuevo.AlternateText = "Nuevo";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            Btn_Salir.AlternateText = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Grid_Motivos_Avaluo.SelectedIndex = -1;
                            Cmb_Estatus.SelectedIndex = 0;
                            Txt_Motivo_Avaluo_Descripcion.Text = "";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Motivos de Avalúo", "alert('Actualización de Motivo de Avalúo Exitosa.');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Motivos de Avalúo", "alert('Error al Actualizar el Motivo de Avalúo.');", true);
                        }
                    }
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione el Motivo de Avalúo a modificar.";
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

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    /////DESCRIPCIÓN: Evento del botón salir
    /////PROPIEDADES:     
    /////            
    /////CREO: Miguel Angel Bedolla Moreno
    /////FECHA_CREO: 05/May_2012
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    //{

    //}

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
            Cmb_Estatus.SelectedIndex = 0;
            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Configuracion_Formulario(true);
            Llenar_Tabla_Motivos_Avaluo(Grid_Motivos_Avaluo.PageIndex);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Grid_Motivos_Avaluo.SelectedIndex = -1;
            Txt_Motivo_Avaluo_Descripcion.Text = "";
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
        Llenar_Tabla_Motivos_Avaluo(0);
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
    protected void Grid_Motivos_Avaluo_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Llenar_Tabla_Motivos_Avaluo(e.NewPageIndex);
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
    protected void Grid_Motivos_Avaluo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Motivos_Avaluo.SelectedIndex > -1)
        {
            Hdf_Motivo_Avaluo_Id.Value = Grid_Motivos_Avaluo.SelectedRow.Cells[1].Text;
            Txt_Motivo_Avaluo_Descripcion.Text = Grid_Motivos_Avaluo.SelectedRow.Cells[2].Text;
            Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByText(HttpUtility.HtmlDecode(Grid_Motivos_Avaluo.SelectedRow.Cells[3].Text.Trim())));
        }
    }

    private Boolean Validar_Componentes()
    {
        String Mensaje_Error = "Error: ";
        Boolean valido = true;

        if (Txt_Motivo_Avaluo_Descripcion.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese la descripción del motivo del avalúo.";
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
}