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
using Presidencia.Catalogo_Colonias.Negocio;
using Presidencia.Catalogo_Calles.Negocio;
using Presidencia.Constantes;

public partial class paginas_Predial_Ventanas_Emergentes_Frm_Busqueda_Avanzada_Colonias_Calles : System.Web.UI.Page
{
    String Boton_Busqueda_Pulsado = "";
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Page_Load
    ///DESCRIPCIÓN          : Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        if (!IsPostBack)
        {
            Session["BUSQUEDA_COLONIAS_CALLES"] = false;
        }
        Frm_Busqueda_Avanzada_Cuentas_Predial.Page.Title = "Búsqueda Avanzada de Colonias y Calles";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "Window_Resize();", true);
        if (Session["Boton_Busqueda_Pulsado"] != null)
        {
            Boton_Busqueda_Pulsado = Session["Boton_Busqueda_Pulsado"].ToString();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Regresar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Regresar_Click(object sender, ImageClickEventArgs e)
    {
        Session["BUSQUEDA_COLONIAS_CALLES"] = false;
        Session.Remove("COLONIA_ID");
        Session.Remove("NOMBRE_COLONIA");
        Session.Remove("CLAVE_COLONIA");
        Session.Remove("CALLE_ID");
        Session.Remove("NOMBRE_CALLE");
        Session.Remove("CLAVE_CALLE");
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Aceptar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Aceptar
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Aceptar_Click(object sender, ImageClickEventArgs e)
    {
        Session["BUSQUEDA_COLONIAS_CALLES"] = true;
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Limpiar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Limpiar
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
    {
        Hdn_Colonia_ID.Value = "";
        Txt_Busqueda_Colonia.Text = "";
        Hdn_Calle_ID.Value = "";
        Txt_Busqueda_Calle.Text = "";
        Txt_Busqueda_Clave_Colonia.Text = "";
        Txt_Busqueda_Clave_Calle.Text = "";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Cargar_Colonias_Calles
    /// DESCRIPCION             : Carga en el grid la búsqueda de las Colonias y Calles
    /// CREO                    : Antonio Salvador Benavides Guardado
    /// FECHA_CREO              : 14/Septiembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Cargar_Colonias_Calles(Int32 No_Pagina, String Boton_Busqueda)
    {
        try
        {
            Cls_Cat_Pre_Calles_Negocio Calles = new Cls_Cat_Pre_Calles_Negocio();
            DataTable Dt_Colonias_Calles = null;

            if (Txt_Busqueda_Colonia.Text.Trim() != "")
            {
                Calles.P_Nombre_Colonia = "LIKE UPPER('%" + HttpUtility.HtmlDecode(Txt_Busqueda_Colonia.Text) + "%')";
            }
            else
            {
                if (Txt_Busqueda_Clave_Colonia.Text.Trim() != "")
                {
                    Calles.P_Clave_Colonia = HttpUtility.HtmlDecode(Txt_Busqueda_Clave_Colonia.Text).Trim();
                    //Calles.P_Clave_Colonia = "LIKE UPPER('%" + HttpUtility.HtmlDecode(Txt_Busqueda_Clave_Colonia.Text) + "%')";
                }
            }
            if (Txt_Busqueda_Calle.Text.Trim() != "")
            {
                Calles.P_Nombre_Calle = "LIKE UPPER('%" + HttpUtility.HtmlDecode(Txt_Busqueda_Calle.Text) + "%')";
            }
            else
            {
                if (Txt_Busqueda_Clave_Calle.Text.Trim() != "")
                {
                    Calles.P_Clave_Calle = HttpUtility.HtmlDecode(Txt_Busqueda_Clave_Calle.Text).Trim();
                    //Calles.P_Clave_Calle = "LIKE UPPER('%" + HttpUtility.HtmlDecode(Txt_Busqueda_Clave_Calle.Text) + "%')";
                }
            }
            Dt_Colonias_Calles = Calles.Consultar_Colonias_Calles();

            //if (Boton_Busqueda == Btn_Buscar_Colonias.ID
            //    || Boton_Busqueda == Btn_Buscar_Colonias_Clave.ID)
            //{
            //    if (Txt_Busqueda_Colonia.Text.Trim() != "")
            //    {
            //        Calles.P_Nombre_Colonia = "LIKE UPPER('%" + HttpUtility.HtmlDecode(Txt_Busqueda_Colonia.Text) + "%')";
            //    }
            //    else
            //    {
            //        if (Txt_Busqueda_Clave_Colonia.Text.Trim() != "")
            //        {
            //            Calles.P_Clave_Colonia = HttpUtility.HtmlDecode(Txt_Busqueda_Clave_Colonia.Text).Trim();
            //            //Calles.P_Clave_Colonia = "LIKE UPPER('%" + HttpUtility.HtmlDecode(Txt_Busqueda_Clave_Colonia.Text) + "%')";
            //        }
            //    }
            //    if (Txt_Busqueda_Colonia.Text.Trim() != ""
            //        || Txt_Busqueda_Clave_Colonia.Text.Trim() != "")
            //    {
            //        Dt_Colonias_Calles = Calles.Consultar_Colonias_Calles();
            //    }
            //}
            //else
            //{
            //    if (Boton_Busqueda == Btn_Buscar_Calles.ID
            //        || Boton_Busqueda == Btn_Buscar_Calles_Clave.ID)
            //    {
            //        if (Txt_Busqueda_Calle.Text.Trim() != "")
            //        {
            //            Calles.P_Nombre_Calle = "LIKE UPPER('%" + HttpUtility.HtmlDecode(Txt_Busqueda_Calle.Text) + "%')";
            //        }
            //        else
            //        {
            //            if (Txt_Busqueda_Clave_Calle.Text.Trim() != "")
            //            {
            //                Calles.P_Clave_Calle = HttpUtility.HtmlDecode(Txt_Busqueda_Clave_Calle.Text).Trim();
            //                //Calles.P_Clave_Calle = "LIKE UPPER('%" + HttpUtility.HtmlDecode(Txt_Busqueda_Clave_Calle.Text) + "%')";
            //            }
            //        }
            //        if (Txt_Busqueda_Calle.Text.Trim() != ""
            //            || Txt_Busqueda_Clave_Calle.Text.Trim() != "")
            //        {
            //            Dt_Colonias_Calles = Calles.Consultar_Colonias_Calles();
            //        }
            //    }
            //}
            Grid_Colonias_Calles.DataSource = Dt_Colonias_Calles;
            Grid_Colonias_Calles.PageIndex = No_Pagina;
            Grid_Colonias_Calles.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Búsqueda Avanzada Colonias y Calles", "Window_Resize();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN      : Grid_Colonias_Calles_PageIndexChanging
    /// 	DESCRIPCIÓN         : Maneja el Evento de Cambio de Página del Grid de 
    /// 	PARÁMETROS          :
    /// 	CREO                : Antonio Salvador Benavides Guardado
    /// 	FECHA_CREO          : 14/Septiembre/2010 
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Colonias_Calles_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Colonias_Calles.SelectedIndex = (-1);
            Cargar_Colonias_Calles(e.NewPageIndex, Boton_Busqueda_Pulsado);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN      : Grid_Colonias_Calles_SelectedIndexChanged
    /// 	DESCRIPCIÓN         : Maneja el Evento de Cambio de Selección del Grid 
    /// 	PARÁMETROS:
    /// 	CREO                : Antonio Salvador Benavides Guardado
    /// 	FECHA_CREO          : 14/Septiembre/2010 
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Colonias_Calles_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["COLONIA_ID"] = Grid_Colonias_Calles.DataKeys[Grid_Colonias_Calles.SelectedIndex].Values[0].ToString();
            Session["NOMBRE_COLONIA"] = HttpUtility.HtmlDecode(Grid_Colonias_Calles.Rows[Grid_Colonias_Calles.SelectedIndex].Cells[2].Text);
            Session["CLAVE_COLONIA"] = HttpUtility.HtmlDecode(Grid_Colonias_Calles.Rows[Grid_Colonias_Calles.SelectedIndex].Cells[3].Text);
            Session["CALLE_ID"] = Grid_Colonias_Calles.DataKeys[Grid_Colonias_Calles.SelectedIndex].Values[1].ToString();
            Session["NOMBRE_CALLE"] = HttpUtility.HtmlDecode(Grid_Colonias_Calles.Rows[Grid_Colonias_Calles.SelectedIndex].Cells[5].Text);
            Session["CLAVE_CALLE"] = HttpUtility.HtmlDecode(Grid_Colonias_Calles.Rows[Grid_Colonias_Calles.SelectedIndex].Cells[6].Text);

            Btn_Aceptar_Click(sender, null);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN      : Btn_Cerrar_Busqueda_Colonias_Click
    /// 	DESCRIPCIÓN         : Ocultar el modal popup Busqueda de 
    /// 	PARÁMETROS:
    /// 	CREO                : Antonio Salvador Benavides Guardado
    /// 	FECHA_CREO          : 14/Septiembre/2010 
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Cerrar_Busqueda_Colonias_Click(object sender, ImageClickEventArgs e)
    {
        Mpe_Busqueda_Colonias.Hide();
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN      : Btn_Limpiar_Busqueda_Colonias_Click
    /// 	DESCRIPCIÓN         : Limpia los controles de la búsqeuda avanzada
    /// 	PARÁMETROS:
    /// 	CREO                : Antonio Salvador Benavides Guardado
    /// 	FECHA_CREO          : 14/Septiembre/2010 
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Limpiar_Busqueda_Colonias_Click(object sender, ImageClickEventArgs e)
    {
        Txt_Busqueda_Avanzada_Colonia_ID.Text = "";
        Txt_Busqueda_Avanzada_Nombre_Colonia.Text = "";
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN      : Btn_Cerrar_Busqueda_Calles_Click
    /// 	DESCRIPCIÓN         : Ocultar el modal popup Busqueda de 
    /// 	PARÁMETROS:
    /// 	CREO                : Antonio Salvador Benavides Guardado
    /// 	FECHA_CREO          : 14/Septiembre/2010 
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Cerrar_Busqueda_Calles_Click(object sender, ImageClickEventArgs e)
    {
        Mpe_Busqueda_Calles.Hide();
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN      : Btn_Limpiar_Busqueda_Calles_Click
    /// 	DESCRIPCIÓN         : Limpia los controles de la búsqeuda avanzada
    /// 	PARÁMETROS:
    /// 	CREO                : Antonio Salvador Benavides Guardado
    /// 	FECHA_CREO          : 14/Septiembre/2010 
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Limpiar_Busqueda_Calles_Click(object sender, ImageClickEventArgs e)
    {
        Txt_Busqueda_Avanzada_Calle_ID.Text = "";
        Txt_Busqueda_Avanzada_Nombre_Calle.Text = "";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Btn_Busqueda_Colonias_Click
    /// DESCRIPCION             : Carga en el grid la búsqueda con las Colonias
    /// CREO                    : Antonio Salvador Benavides Guardado
    /// FECHA_CREO              : 14/Septiembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Busqueda_Colonias_Click(object sender, EventArgs e)
    {
        Buscar_Colonias(0);
    }

    private void Buscar_Colonias(int Indice_Pagina)
    {
        Cls_Ate_Colonias_Negocio Colonias = new Cls_Ate_Colonias_Negocio();
        DataTable Dt_Colonias;

        Colonias.P_Campos_Dinamicos = Cat_Ate_Colonias.Campo_Colonia_ID + ", " + Cat_Ate_Colonias.Campo_Nombre;
        Colonias.P_Filtros_Dinamicos = "";
        if (Txt_Busqueda_Avanzada_Colonia_ID.Text.Trim() != "")
        {
            Colonias.P_Filtros_Dinamicos += Cat_Ate_Colonias.Campo_Colonia_ID + " LIKE '%" + Txt_Busqueda_Avanzada_Colonia_ID.Text.Trim() + "%' AND ";
        }
        if (Txt_Busqueda_Avanzada_Nombre_Colonia.Text.ToUpper().Trim() != "")
        {
            Colonias.P_Filtros_Dinamicos += Cat_Ate_Colonias.Campo_Nombre + " LIKE '%" + Txt_Busqueda_Avanzada_Nombre_Colonia.Text.ToUpper().Trim() + "%' AND ";
        }
        if (Colonias.P_Filtros_Dinamicos.EndsWith(" AND"))
        {
            Colonias.P_Filtros_Dinamicos = Colonias.P_Filtros_Dinamicos.Substring(0, Colonias.P_Filtros_Dinamicos.Length - 4);
        }
        if (Colonias.P_Filtros_Dinamicos.EndsWith(" OR"))
        {
            Colonias.P_Filtros_Dinamicos = Colonias.P_Filtros_Dinamicos.Substring(0, Colonias.P_Filtros_Dinamicos.Length - 3);
        }
        if (Colonias.P_Filtros_Dinamicos.EndsWith(" WHERE"))
        {
            Colonias.P_Filtros_Dinamicos = Colonias.P_Filtros_Dinamicos.Substring(0, Colonias.P_Filtros_Dinamicos.Length - 6);
        }
        Dt_Colonias = Colonias.Consultar_Colonias();
        Grid_Colonias.DataSource = Dt_Colonias;
        Grid_Colonias.PageIndex = Indice_Pagina;
        Grid_Colonias.DataBind();
        Mpe_Busqueda_Colonias.Show();
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Btn_Busqueda_Calles_Click
    /// DESCRIPCION             : Carga en el grid la búsqueda con las Calles
    /// CREO                    : Antonio Salvador Benavides Guardado
    /// FECHA_CREO              : 14/Septiembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Busqueda_Calles_Click(object sender, EventArgs e)
    {
        Buscar_Calles(0);
    }

    private void Buscar_Calles(int Indice_Pagina)
    {
        Cls_Cat_Pre_Calles_Negocio Calles = new Cls_Cat_Pre_Calles_Negocio();
        DataTable Dt_Calles;

        if (Txt_Busqueda_Avanzada_Calle_ID.Text.Trim() != "")
        {
            Calles.P_Calle_ID = "LIKE '%" + Txt_Busqueda_Avanzada_Calle_ID.Text + "%'";
        }
        if (Txt_Busqueda_Avanzada_Nombre_Calle.Text.ToUpper().Trim() != "")
        {
            Calles.P_Nombre = "LIKE '%" + Txt_Busqueda_Avanzada_Nombre_Calle.Text.ToUpper().Trim() + "%'";
        }
        Dt_Calles = Calles.Consultar_Nombre_Id_Calles();
        Grid_Calles.DataSource = Dt_Calles;
        Grid_Calles.PageIndex = Indice_Pagina;
        Grid_Calles.DataBind();
        Mpe_Busqueda_Calles.Show();
    }

    protected void Grid_Colonias_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Buscar_Colonias(e.NewPageIndex);
    }
    
    protected void Grid_Colonias_SelectedIndexChanged(object sender, EventArgs e)
    {
        Hdn_Colonia_ID.Value = Grid_Colonias.SelectedRow.Cells[1].Text;
        Txt_Busqueda_Colonia.Text = Grid_Colonias.SelectedRow.Cells[2].Text;
        Mpe_Busqueda_Colonias.Hide();
    }
    
    protected void Grid_Calles_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Buscar_Calles(e.NewPageIndex);
    }
    
    protected void Grid_Calles_SelectedIndexChanged(object sender, EventArgs e)
    {
        Hdn_Calle_ID.Value = Grid_Calles.SelectedRow.Cells[1].Text;
        Txt_Busqueda_Calle.Text = Grid_Calles.SelectedRow.Cells[2].Text;
        Mpe_Busqueda_Calles.Hide();
    }

    protected void Txt_Busqueda_Avanzada_Colonia_ID_TextChanged(object sender, EventArgs e)
    {
        Buscar_Colonias(0);
    }

    protected void Txt_Busqueda_Avanzada_Nombre_Colonia_TextChanged(object sender, EventArgs e)
    {
        Buscar_Colonias(0);
    }

    protected void Txt_Busqueda_Avanzada_Calle_ID_TextChanged(object sender, EventArgs e)
    {
        Buscar_Calles(0);
    }

    protected void Txt_Busqueda_Avanzada_Nombre_Calle_TextChanged(object sender, EventArgs e)
    {
        Buscar_Calles(0);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Operador_Comparacion
    ///DESCRIPCIÓN          : Devuelve una cadena adecuada al operador indicado en la capa de Negocios
    ///PARAMETROS           : 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 20/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private static String Validar_Operador_Comparacion(String Filtro)
    {
        String Cadena_Validada;
        if (Filtro.Trim().StartsWith("<")
           || Filtro.Trim().StartsWith(">")
           || Filtro.Trim().StartsWith("<>")
           || Filtro.Trim().StartsWith("<=")
           || Filtro.Trim().StartsWith(">=")
           || Filtro.Trim().StartsWith("=")
           || Filtro.Trim().ToUpper().StartsWith("BETWEEN")
           || Filtro.Trim().ToUpper().StartsWith("LIKE")
           || Filtro.Trim().ToUpper().StartsWith("IN"))
        {
            Cadena_Validada = " " + Filtro + " ";
        }
        else
        {
            if (Filtro.Trim().ToUpper().StartsWith("NULL"))
            {
                Cadena_Validada = " IS " + Filtro + " ";
            }
            else
            {
                Cadena_Validada = " = '" + Filtro + "' ";
            }
        }
        return Cadena_Validada;
    }

    protected void Btn_Buscar_Colonias_Click(object sender, ImageClickEventArgs e)
    {
        Session["Boton_Busqueda_Pulsado"] = ((ImageButton)sender).ID;
        Cargar_Colonias_Calles(0, ((ImageButton)sender).ID);
    }

    protected void Btn_Buscar_Calles_Click(object sender, ImageClickEventArgs e)
    {
        Session["Boton_Busqueda_Pulsado"] = ((ImageButton)sender).ID;
        Cargar_Colonias_Calles(0, ((ImageButton)sender).ID);
    }

    protected void Txt_Busqueda_Colonia_TextChanged(object sender, EventArgs e)
    {
        Session["Boton_Busqueda_Pulsado"] = Btn_Buscar_Colonias.ID;
        Cargar_Colonias_Calles(0, Btn_Buscar_Colonias.ID);
    }
    
    protected void Txt_Busqueda_Calle_TextChanged(object sender, EventArgs e)
    {
        Session["Boton_Busqueda_Pulsado"] = Btn_Buscar_Calles.ID;
        Cargar_Colonias_Calles(0, Btn_Buscar_Calles.ID);
    }
}
