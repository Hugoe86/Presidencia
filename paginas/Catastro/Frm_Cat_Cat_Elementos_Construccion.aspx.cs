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
using Presidencia.Catalogo_Cat_Elementos_Construccion.Negocio;
using Presidencia.Sessiones;

public partial class paginas_Catastro_Frm_Cat_Cat_Elementos_Construccion : System.Web.UI.Page
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
                Llenar_Tabla_Elementos_Construccion(0);                
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
        Txt_Elemento_Construccion.Enabled = !Habilitado;
        Cmb_Estatus.Enabled = !Habilitado;
        Grid_Elemento_Construccion.Enabled = Habilitado;
        Btn_Buscar.Enabled = Habilitado;
        Txt_Busqueda.Enabled = Habilitado;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Elementos_Construccion
    ///DESCRIPCIÓN: Llena la tabla de Motivos de Rechazo
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 07/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Elementos_Construccion(int Pagina)
    {
        try
        {
            Cls_Cat_Cat_Elementos_Construccion_Negocio Elemento_Const= new Cls_Cat_Cat_Elementos_Construccion_Negocio();
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Elemento_Const.P_Elemento_Construccion = Txt_Busqueda.Text.ToUpper();
            }
            Grid_Elemento_Construccion.Columns[1].Visible = true;
            Grid_Elemento_Construccion.DataSource = Elemento_Const.Consultar_Elemento_Construccion();
            Grid_Elemento_Construccion.PageIndex = Pagina;
            Grid_Elemento_Construccion.DataBind();
            Grid_Elemento_Construccion.Columns[1].Visible = false;
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
                Txt_Elemento_Construccion.Text = "";
                Cmb_Estatus.SelectedIndex = 1;
            }
            else if (Validar_Componentes())
            {
                Cls_Cat_Cat_Elementos_Construccion_Negocio Elemento_Const = new Cls_Cat_Cat_Elementos_Construccion_Negocio();
                Elemento_Const.P_Estatus = Cmb_Estatus.SelectedValue;
                Elemento_Const.P_Elemento_Construccion = Txt_Elemento_Construccion.Text.ToUpper();
                if ((Elemento_Const.Alta_Elemento_Construccion()))
                {
                    Txt_Busqueda.Text = "";
                    Llenar_Tabla_Elementos_Construccion(0);
                    Configuracion_Formulario(true);
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Elemento_Construccion.SelectedIndex = -1;
                    Cmb_Estatus.SelectedIndex = 0;
                    Txt_Elemento_Construccion.Text = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Elementos Construccion ", "alert('Alta de Elemento Exitosa');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Elementos Construccion", "alert('Alta de Construccion Errónea');", true);
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
            if (Grid_Elemento_Construccion.SelectedIndex > -1)
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
                        Cls_Cat_Cat_Elementos_Construccion_Negocio Elemento_Const = new Cls_Cat_Cat_Elementos_Construccion_Negocio();
                        Elemento_Const.P_Estatus = Cmb_Estatus.SelectedValue;
                        Elemento_Const.P_Elemento_Construccion = Txt_Elemento_Construccion.Text.ToUpper();
                        Elemento_Const.P_Elemento_Construccion_ID = Hdf_Elemento_Construccion_Id.Value;
                        if ((Elemento_Const.Modificar_Elemento_Construccion()))
                        {
                            Txt_Busqueda.Text = "";
                            Configuracion_Formulario(true);
                            Llenar_Tabla_Elementos_Construccion(Grid_Elemento_Construccion.PageIndex);
                            Btn_Modificar.AlternateText = "Modificar";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            Btn_Nuevo.Visible = true;
                            Btn_Nuevo.AlternateText = "Nuevo";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            Btn_Salir.AlternateText = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Grid_Elemento_Construccion.SelectedIndex = -1;
                            Cmb_Estatus.SelectedIndex = 0;
                            Txt_Elemento_Construccion.Text = "";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Elementos de Construccion", "alert('Actualización de Elemento de Construccion Exitosa.');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Elementos de Construccion", "alert('Error al Actualizar el Elemento de Construccion.');", true);
                        }
                    }
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione el Elemento de Construcción a modificar.";
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
            Cmb_Estatus.SelectedIndex = 0;
            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Configuracion_Formulario(true);
            Llenar_Tabla_Elementos_Construccion(Grid_Elemento_Construccion.PageIndex);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Grid_Elemento_Construccion.SelectedIndex = -1;
            Txt_Elemento_Construccion.Text = "";
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
        Llenar_Tabla_Elementos_Construccion(0);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Elemento_Construccion_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Elemento_Construccion_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Llenar_Tabla_Elementos_Construccion(e.NewPageIndex);
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
    protected void Grid_Elemento_Construccion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Elemento_Construccion.SelectedIndex > -1)
        {
            Hdf_Elemento_Construccion_Id.Value = Grid_Elemento_Construccion.SelectedRow.Cells[1].Text;
            Txt_Elemento_Construccion.Text = Grid_Elemento_Construccion.SelectedRow.Cells[2].Text;
            Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByText(HttpUtility.HtmlDecode(Grid_Elemento_Construccion.SelectedRow.Cells[3].Text.Trim())));
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
    ///DESCRIPCIÓN: valida que cada uno de los componentes contenga informacion
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
        String Mensaje_Error = "Error: ";
        Boolean valido = true;

        if (Txt_Elemento_Construccion.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese el Servicio.";
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
