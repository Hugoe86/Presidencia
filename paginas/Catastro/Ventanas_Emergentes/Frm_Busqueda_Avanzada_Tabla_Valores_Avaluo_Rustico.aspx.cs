using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Presidencia.Catalogo_Cat_Tabla_Valores_Construccion_Rustico.Negocio;

public partial class paginas_Catastro_Ventanas_Emergentes_Frm_Busqueda_Avanzada_Tabla_Valores_Avaluo_Rustico : System.Web.UI.Page
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
    private void Llenar_Tabla_Tipos_Construccion_Rustico(int Pagina)
    {
        try
        {
            DataTable Dt_Tipos_Construccion;
            Cls_Cat_Cat_Tabla_Valores_Construccion_Rustico_Negocio Tabla_Val = new Cls_Cat_Cat_Tabla_Valores_Construccion_Rustico_Negocio();
            Tabla_Val.P_Estatus = "= 'VIGENTE' ";
            if (Txt_Tipo_Construccion.Text.Trim() != "")
            {
                Tabla_Val.P_Identificador = Txt_Tipo_Construccion.Text.ToUpper();
            }
            Grid_Tipos_Construccion.Columns[1].Visible = true;
            Dt_Tipos_Construccion = Tabla_Val.Consultar_Tipos_Construccion_Rustico();
            Grid_Tipos_Construccion.DataSource = Dt_Tipos_Construccion;
            Grid_Tipos_Construccion.PageIndex = Pagina;
            Grid_Tipos_Construccion.DataBind();
            Grid_Tipos_Construccion.Columns[1].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
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
    private void Llenar_Tabla_Valores_Construccion_Rustico(int Pagina)
    {
        try
        {
            DataTable Dt_Tabla_Valores_Construccion;
            Cls_Cat_Cat_Tabla_Valores_Construccion_Rustico_Negocio Tabla_Val = new Cls_Cat_Cat_Tabla_Valores_Construccion_Rustico_Negocio();
            Tabla_Val.P_Anio = Session["Anio"].ToString();
            Tabla_Val.P_Tipo_Constru_Rustico_Id = Hdf_Tipo_Construccion.Value;
            Grid_Valores.Columns[1].Visible = true;
            Dt_Tabla_Valores_Construccion = Tabla_Val.Consultar_Tabla_Valores_Construccion_Rustico();
            Dt_Tabla_Valores_Construccion.DefaultView.Sort = "ANIO DESC";
            Grid_Valores.DataSource = Dt_Tabla_Valores_Construccion;
            Grid_Valores.PageIndex = Pagina;
            Grid_Valores.DataBind();
            Grid_Valores.Columns[1].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
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
        Llenar_Tabla_Tipos_Construccion_Rustico(0);
        Div_Grid_Tipos_Construccion.Visible = true;
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
            Session["VALOR_CONSTRU_RUSTICO_ID"] = null;
            Session["VALOR_M2"] = null;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", "window.close();", true);
        }
        else
        {
            Llenar_Tabla_Tipos_Construccion_Rustico(Grid_Tipos_Construccion.PageIndex);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Div_Grid_Tipos_Construccion.Visible = true;
            Div_Grid_Tabla_Valores.Visible = false;
            Grid_Valores.DataSource = null;
            Grid_Valores.DataBind();
            Grid_Valores.SelectedIndex = -1;
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
        Txt_Tipo_Construccion.Text = "";
        Grid_Valores.DataSource = null;
        Grid_Valores.DataBind();
        Div_Grid_Tabla_Valores.Visible = false;
        Div_Grid_Tipos_Construccion.Visible = false;
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
    protected void Grid_Tipos_Construccion_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Llenar_Tabla_Tipos_Construccion_Rustico(e.NewPageIndex);
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
    protected void Grid_Tipos_Construccion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Tipos_Construccion.SelectedIndex > -1)
        {
            Hdf_Tipo_Construccion.Value = Grid_Tipos_Construccion.SelectedRow.Cells[1].Text;
            Txt_Tipo_Construccion.Text = Grid_Tipos_Construccion.SelectedRow.Cells[2].Text;
            Div_Grid_Tipos_Construccion.Visible = false;
            Div_Grid_Tabla_Valores.Visible = true;
            Btn_Salir.AlternateText = "Atras";
            Llenar_Tabla_Valores_Construccion_Rustico(0);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Valores_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid de Valores
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 11/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Valores_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Llenar_Tabla_Valores_Construccion_Rustico(e.NewPageIndex);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Valores_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona un elemento del grid de Valores y toma sus valores correspondientes
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 11/May_2012
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
                Session["VALOR_CONSTRU_RUSTICO_ID"] = Grid_Valores.SelectedRow.Cells[1].Text;
                Session["VALOR_M2"] = Convert.ToDouble(HttpUtility.HtmlDecode(Grid_Valores.SelectedRow.Cells[3].Text.Replace("$", "")));
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