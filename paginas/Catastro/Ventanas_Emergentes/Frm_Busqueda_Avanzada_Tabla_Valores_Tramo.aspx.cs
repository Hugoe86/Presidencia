using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Catalogo_Cat_Tabla_Valores_Tramos.Negocio;
using System.Data;
using Presidencia.Cat_Cat_Tramos_Calle.Negocio;

public partial class paginas_Catastro_Ventanas_Emergentes_Frm_Busqueda_Avanzada_Tabla_Valores_Tramo : System.Web.UI.Page
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

            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                //Configuracion_Formulario(true);
                //Llenar_Tabla_Calles(0);
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
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Calles
    ///DESCRIPCIÓN: Llena la tabla de Calles
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 20/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Calles(int Pagina)
    {
        try
        {
            DataTable Dt_Calles;
            Cls_Cat_Cat_Tramos_Calle_Negocio Calles = new Cls_Cat_Cat_Tramos_Calle_Negocio();
            if (Txt_Calle.Text.Trim() != "")
            {
                Calles.P_Calle_Busqueda = Txt_Calle.Text.ToUpper();
            }
            if (Txt_Colonia.Text.Trim() != "")
            {
                Calles.P_Colonia_Busqueda = Txt_Colonia.Text.ToUpper();
            }
            Grid_Calles.Columns[1].Visible = true;
            Dt_Calles = Calles.Consultar_Calles();
            Grid_Calles.DataSource = Dt_Calles;
            Grid_Calles.PageIndex = Pagina;
            Grid_Calles.DataBind();
            Grid_Calles.Columns[1].Visible = false;
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
            Session["VALOR_TRAMO_ID"] = null;
            Session["VALOR_M2"] = null;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", "window.close();", true);
        }
        else
        {
            Llenar_Tabla_Calles(Grid_Calles.PageIndex);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Div_Grid_Calles.Visible = true;
            Div_Grid_Tramos.Visible = false;
            Div_Grid_Valores_Tramos.Visible = false;
            Grid_Tramos.DataSource = null;
            Grid_Tramos.DataBind();
            Grid_Valores_Tramos.DataSource = null;
            Grid_Valores_Tramos.DataBind();
            Grid_Calles.SelectedIndex = -1;
            Grid_Tramos.SelectedIndex = -1;
            Grid_Valores_Tramos.SelectedIndex = -1;
            Btn_Limpiar_Click(null, null);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Click
    ///DESCRIPCIÓN: Evento del botón Limpiar
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
    {
        Txt_Calle.Text = "";
        Txt_Colonia.Text = "";
        Grid_Calles.DataSource = null;
        Grid_Calles.DataBind();
        Div_Grid_Calles.Visible = false;
        Grid_Tramos.DataSource = null;
        Grid_Tramos.DataBind();
        Div_Grid_Tramos.Visible = false;
        Grid_Valores_Tramos.DataSource = null;
        Grid_Valores_Tramos.DataBind();
        Div_Grid_Valores_Tramos.Visible = false;
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
        Llenar_Tabla_Calles(0);
        Div_Grid_Calles.Visible = true;
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
    protected void Grid_Calles_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Llenar_Tabla_Calles(e.NewPageIndex);
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
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
    protected void Grid_Calles_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Calles.SelectedIndex > -1)
        {
            Hdf_Calle_Id.Value = Grid_Calles.SelectedRow.Cells[1].Text;
            Cls_Cat_Cat_Tramos_Calle_Negocio Tramos = new Cls_Cat_Cat_Tramos_Calle_Negocio();
            Tramos.P_Calle_ID = Hdf_Calle_Id.Value;
            DataTable Dt_Tramos = Tramos.Consultar_Tramos();
            Grid_Tramos.Columns[1].Visible = true;
            Grid_Tramos.Columns[3].Visible = true;
            Grid_Tramos.DataSource = Dt_Tramos;
            Grid_Tramos.PageIndex = 0;
            Grid_Tramos.DataBind();
            Grid_Tramos.Columns[1].Visible = false;
            Grid_Tramos.Columns[3].Visible = false;
            Div_Grid_Calles.Visible = false;
            Div_Grid_Tramos.Visible = true;
            Btn_Salir.AlternateText = "Atras";
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
    protected void Grid_Tramo_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Cls_Cat_Cat_Tramos_Calle_Negocio Tramos = new Cls_Cat_Cat_Tramos_Calle_Negocio();
            Tramos.P_Calle_ID = Hdf_Calle_Id.Value;
            DataTable Dt_Tramos = Tramos.Consultar_Tramos();
            Grid_Tramos.Columns[1].Visible = true;
            Grid_Tramos.Columns[3].Visible = true;
            Grid_Tramos.DataSource = Dt_Tramos;
            Grid_Tramos.PageIndex = e.NewPageIndex;
            Grid_Tramos.DataBind();
            Grid_Tramos.Columns[1].Visible = false;
            Grid_Tramos.Columns[3].Visible = false;
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
    protected void Grid_Tramo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Tramos.SelectedIndex > -1)
            {
                Hdf_Tramo_Id.Value = Grid_Tramos.SelectedRow.Cells[1].Text;
                Div_Grid_Valores_Tramos.Visible = true;
                Div_Grid_Tramos.Visible = false;
                Cls_Cat_Cat_Tabla_Valores_Tramos_Negocio Tabla_Valores = new Cls_Cat_Cat_Tabla_Valores_Tramos_Negocio();
                Tabla_Valores.P_Tramo_Id = Hdf_Tramo_Id.Value;
                if (Session["ANIO"] != null)
                {
                    Tabla_Valores.P_Anio = Session["ANIO"].ToString();
                }
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
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione un Tramo.";
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
            DataTable Dt_Tabla_Valores = (DataTable)Session["Tabla_Valores"];
            Grid_Valores_Tramos.Columns[1].Visible = true;
            Grid_Valores_Tramos.Columns[4].Visible = true;
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
                Session["VALOR_TRAMO_ID"] = Grid_Valores_Tramos.SelectedRow.Cells[1].Text;
                Session["VALOR_M2"] = Convert.ToDouble(HttpUtility.HtmlDecode(Grid_Valores_Tramos.SelectedRow.Cells[3].Text.Replace("$", "")));
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", "window.close();", true);
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
}