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
using Presidencia.Catalogo_Cat_Descripcion_Construccion_Rustico.Negocio;

public partial class paginas_Catastro_Frm_Cat_Cat_Descripcion_Construccion_Rustico : System.Web.UI.Page
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
                Llenar_Tabla_Tipos(0);
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
    ///DESCRIPCIÓN: habilita y deshabilita componentes del formulario
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 07/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Habilitado)
    {
        Txt_Identificador_Contruccion.Enabled = !Habilitado;
        Cmb_Estatus.Enabled = !Habilitado;
        Btn_Buscar.Enabled = Habilitado;
        Txt_Busqueda.Enabled = Habilitado;
        Grid_Descripcion_Construccion_Rustico.Enabled = Habilitado;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Tipos
    ///DESCRIPCIÓN: Llena la tabla de los datos de tipos
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 07/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    ///
    private void Llenar_Tabla_Tipos(int Pagina)
    {
        try
        {
            Cls_Cat_Cat_Descripcion_Construccion_Rustico_Negocio Tipo = new Cls_Cat_Cat_Descripcion_Construccion_Rustico_Negocio();
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Tipo.P_Identificador = Txt_Busqueda.Text.ToUpper();
            }
            Grid_Descripcion_Construccion_Rustico.Columns[1].Visible = true;
            Grid_Descripcion_Construccion_Rustico.DataSource = Tipo.Consultar_Descripcion_Construccion_Rustico();
            Grid_Descripcion_Construccion_Rustico.PageIndex = Pagina;
            Grid_Descripcion_Construccion_Rustico.DataBind();
            Grid_Descripcion_Construccion_Rustico.Columns[1].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    //*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Evento del botón nuevo
    ///PROPIEDADES:                 
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
                Txt_Identificador_Contruccion.Text = "";
                Cmb_Estatus.SelectedIndex = 1;
            }
            else if (Validar_Componentes())
            {
                Cls_Cat_Cat_Descripcion_Construccion_Rustico_Negocio Tipo = new Cls_Cat_Cat_Descripcion_Construccion_Rustico_Negocio();
                Tipo.P_Estatus = Cmb_Estatus.SelectedValue;
                Tipo.P_Identificador = Txt_Identificador_Contruccion.Text.ToUpper();
                if ((Tipo.Alta_Descripcion_Construccion_Rustico()))
                {
                    Txt_Busqueda.Text = "";
                    Llenar_Tabla_Tipos(0);
                    Configuracion_Formulario(true);
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Descripcion_Construccion_Rustico.SelectedIndex = -1;
                    Txt_Identificador_Contruccion.Text = "";
                    Cmb_Estatus.SelectedIndex = 0;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tipos de Construcción", "alert('Alta Exitosa');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tipos de Construcción", "alert('Alta Errónea');", true);
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
            if (Grid_Descripcion_Construccion_Rustico.SelectedIndex > -1)
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
                        Cls_Cat_Cat_Descripcion_Construccion_Rustico_Negocio Tipo = new Cls_Cat_Cat_Descripcion_Construccion_Rustico_Negocio();
                        Tipo.P_Estatus = Cmb_Estatus.SelectedValue;
                        Tipo.P_Identificador = Txt_Identificador_Contruccion.Text.ToUpper();
                        Tipo.P_Descripcion_Construccion_Rustico_ID = Hdf_Descript_Construccion_Id.Value;
                        if ((Tipo.Modificar_Descripcion_Construccion_Rustico()))
                        {
                            Txt_Busqueda.Text = "";
                            Configuracion_Formulario(true);
                            Llenar_Tabla_Tipos(Grid_Descripcion_Construccion_Rustico.PageIndex);
                            Btn_Modificar.AlternateText = "Modificar";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            Btn_Nuevo.Visible = true;
                            Btn_Nuevo.AlternateText = "Nuevo";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            Btn_Salir.AlternateText = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Grid_Descripcion_Construccion_Rustico.SelectedIndex = -1;
                            Txt_Identificador_Contruccion.Text = "";
                            Cmb_Estatus.SelectedIndex = 0;
                            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                "Catalogo de Descripcion de Construcción Rustico", "alert('Actualización Exitosa.');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                "Catalogo de Descripcion de Construcción Rustico", "alert('Error al intentar Actualizar.');", true);
                        }
                    }
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione Descripcion de Construcción Rustico.";
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
            Llenar_Tabla_Tipos(Grid_Descripcion_Construccion_Rustico.PageIndex);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Grid_Descripcion_Construccion_Rustico.SelectedIndex = -1;
            Txt_Identificador_Contruccion.Text = "";
            Cmb_Estatus.SelectedIndex = 0;
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
        Llenar_Tabla_Tipos(0);
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Llenar_Tabla_Tipos(e.NewPageIndex);
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona un registro del grid y toma los datos de los mismos campos del componente
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************    
    protected void Grid_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Descripcion_Construccion_Rustico.SelectedIndex > -1)
        {
            Hdf_Descript_Construccion_Id.Value = Grid_Descripcion_Construccion_Rustico.SelectedRow.Cells[1].Text;
            Txt_Identificador_Contruccion.Text = Grid_Descripcion_Construccion_Rustico.SelectedRow.Cells[2].Text;
            Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByText
                (HttpUtility.HtmlDecode(Grid_Descripcion_Construccion_Rustico.SelectedRow.Cells[3].Text.Trim())));
        }
    }
    //*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
    ///DESCRIPCIÓN: Valida que los componentes tengan los datos necesarios para ingresarlos a la BD.
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Componentes()
    {
        String Mensaje_Error = "Error: ";
        Boolean valido = true;
        if (Txt_Identificador_Contruccion.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese el Tipo de Construcción.";
            valido = false;
        }
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese el estatus.";
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
