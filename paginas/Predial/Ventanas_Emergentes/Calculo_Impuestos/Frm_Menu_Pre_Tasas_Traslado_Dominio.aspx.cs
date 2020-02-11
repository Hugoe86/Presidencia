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
using Presidencia.Catalogo_Impuestos_Traslado_Dominio.Negocio;

public partial class paginas_Predial_Ventanas_Emergentes_Calculo_Impuestos_Frm_Menu_Pre_Tasas_Traslado_Dominio : System.Web.UI.Page
{
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Page_Load
    ///DESCRIPCIÓN          : Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 01/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty))
            Response.Redirect("../../../Paginas_Generales/Frm_Apl_Login.aspx");
        if (!IsPostBack)
        {
            Cargar_Tasas_Traslado(0);
        }
        Frm_Menu_Pre_Tasas_Traslado_Dominio.Page.Title = "Tasas Traslado y Deducibles";
        Lbl_Title.Text = "Selecione una Tasa por favor";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Tasas_Traslado
    ///DESCRIPCIÓN          : Metodo que carga los datos de Tasas existentes en el catálogo de Cat_Pre_Impuestos_Traslado
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 01/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Cargar_Tasas_Traslado(int Page_Index)
    {
        try
        {
            Cls_Cat_Pre_Impuestos_Traslado_Dominio_Negocio Tasas = new Cls_Cat_Pre_Impuestos_Traslado_Dominio_Negocio();

            Grid_Tasas.Columns[0].Visible = true;
            Grid_Tasas.DataSource = Tasas.Consultar_Impuestos_Traslado_Dominio();
            Grid_Tasas.PageIndex = Page_Index;
            Grid_Tasas.DataBind();
            Grid_Tasas.Columns[0].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Tasas_SelectedIndexChanged
    ///DESCRIPCIÓN          : Evento SelectedIndexChanged del control DataGrid Grid_Tasas
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 01/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Grid_Tasas_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 Cont_Deducibles;
        Int32 Primer_Columa_Deducibles = 3;

        Session["CONCEPTO_PREDIAL_ID"] = Grid_Tasas.Rows[Grid_Tasas.SelectedIndex].Cells[0].Text;
        Session["IMPUESTO_ID_TRASLACION"] = Grid_Tasas.Rows[Grid_Tasas.SelectedIndex].Cells[2].Text;
        for (Cont_Deducibles = 0; Cont_Deducibles < 3; Cont_Deducibles++)
        {
            if (Grid_Tasas.Rows[Grid_Tasas.SelectedIndex].Cells[Primer_Columa_Deducibles + Cont_Deducibles].Controls.Count > 0)
            {
                if (Grid_Tasas.Rows[Grid_Tasas.SelectedIndex].Cells[Primer_Columa_Deducibles + Cont_Deducibles].Controls[3] is CheckBox)
                {
                    if (((CheckBox)Grid_Tasas.Rows[Grid_Tasas.SelectedIndex].Cells[Primer_Columa_Deducibles + Cont_Deducibles].Controls[3]).Checked)
                    {
                        Session["DEDUCIBLE"] = ((Label)Grid_Tasas.Rows[Grid_Tasas.SelectedIndex].Cells[Primer_Columa_Deducibles + Cont_Deducibles].Controls[1]).Text;
                    }
                }
            }
        }
        Session["TASA"] = Grid_Tasas.Rows[Grid_Tasas.SelectedIndex].Cells[6].Text;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Regresar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 01/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Regresar_Click(object sender, ImageClickEventArgs e)
    {
        Session["BUSQUEDA_TASA_TRASLADO_DOMINIO"] = false;
        Session.Remove("CONCEPTO_PREDIAL_ID");
        Session.Remove("IMPUESTO_ID_TRASLACION");
        Session.Remove("DEDUCIBLE");
        Session.Remove("TASA");
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        //Page.RegisterStartupScript("Cerrar_Script", Pagina);
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Aceptar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Aceptar
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 01/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Aceptar_Click(object sender, ImageClickEventArgs e)
    {
        Int32 Cont_Deducibles;
        Int32 Primer_Columa_Deducibles = 1;

        if (Grid_Tasas.SelectedIndex > -1)
        {
            Session["TASA_ID"] = Grid_Tasas.Rows[Grid_Tasas.SelectedIndex].Cells[0].Text;
            for (Cont_Deducibles = 0; Cont_Deducibles < 3; Cont_Deducibles++)
            {
                if (Grid_Tasas.Rows[Grid_Tasas.SelectedIndex].Cells[Primer_Columa_Deducibles + Cont_Deducibles].Controls.Count > 0)
                {
                    if (Grid_Tasas.Rows[Grid_Tasas.SelectedIndex].Cells[Primer_Columa_Deducibles + Cont_Deducibles].Controls[3] is CheckBox)
                    {
                        if (((CheckBox)Grid_Tasas.Rows[Grid_Tasas.SelectedIndex].Cells[Primer_Columa_Deducibles + Cont_Deducibles].Controls[3]).Checked)
                        {
                            Session["DEDUCIBLE"] = ((Label)Grid_Tasas.Rows[Grid_Tasas.SelectedIndex].Cells[Primer_Columa_Deducibles + Cont_Deducibles].Controls[1]).Text;
                            break;
                        }
                    }
                }
            }
            Session["TASA"] = Grid_Tasas.Rows[Grid_Tasas.SelectedIndex].Cells[6].Text;
        }

        Session["BUSQUEDA_TASA_TRASLADO_DOMINIO"] = true;
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        //Page.RegisterStartupScript("Cerrar_Script", Pagina);
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Mantener_Un_CheckBox_Seleccionado_Grid
    ///DESCRIPCIÓN          : Evento para deseleccionar los checks box previamente seleccionados al seleccionar uno el usaurio
    ///PARAMETROS           : Chk_Deducible, objeto de tipo CheckBox a comparar
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 11/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Mantener_Un_CheckBox_Seleccionado_Grid(CheckBox Chk_Deducible)
    {
        CheckBox Chk_Temp;
        Int32 Cont_Filas = 0;

        if (Chk_Deducible.Checked)
        {
            foreach (GridViewRow Fila in Grid_Tasas.Rows)
            {
                //Deducibles a 10 días
                if (Fila.Cells[1].Controls[3] != null && Fila.Cells[1].Controls[3] is CheckBox)
                {
                    Chk_Temp = (CheckBox)Fila.Cells[1].Controls[3];
                    if (!Chk_Deducible.Equals(Chk_Temp) && Chk_Temp.Checked)
                    {
                        Chk_Temp.Checked = false;
                    }
                    else
                    {
                        if (Chk_Deducible.Equals(Chk_Temp) && Grid_Tasas.SelectedIndex != Cont_Filas)
                        {
                            Grid_Tasas.SelectedIndex = Cont_Filas;
                        }
                    }
                }
                //Deducibles a 15 días
                if (Fila.Cells[2].Controls[3] != null && Fila.Cells[2].Controls[3] is CheckBox)
                {
                    Chk_Temp = (CheckBox)Fila.Cells[2].Controls[3];
                    if (!Chk_Deducible.Equals(Chk_Temp) && Chk_Temp.Checked)
                    {
                        Chk_Temp.Checked = false;
                    }
                    else
                    {
                        if (Chk_Deducible.Equals(Chk_Temp) && Grid_Tasas.SelectedIndex != Cont_Filas)
                        {
                            Grid_Tasas.SelectedIndex = Cont_Filas;
                        }
                    }
                }
                //Deducibles a 20 días
                if (Fila.Cells[3].Controls[3] != null && Fila.Cells[3].Controls[3] is CheckBox)
                {
                    Chk_Temp = (CheckBox)Fila.Cells[3].Controls[3];
                    if (!Chk_Deducible.Equals(Chk_Temp) && Chk_Temp.Checked)
                    {
                        Chk_Temp.Checked = false;
                    }
                    else
                    {
                        if (Chk_Deducible.Equals(Chk_Temp) && Grid_Tasas.SelectedIndex != Cont_Filas)
                        {
                            Grid_Tasas.SelectedIndex = Cont_Filas;
                        }
                    }
                }
                Cont_Filas++;
            }
        }
        else
        {
            Grid_Tasas.SelectedIndex = -1;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Chk_Deducible_10_CheckedChanged
    ///DESCRIPCIÓN          : Evento CheckedChanged del control Chk_Deducible_10
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 11/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Chk_Deducible_10_CheckedChanged(object sender, EventArgs e)
    {
        Mantener_Un_CheckBox_Seleccionado_Grid((CheckBox)sender);
        Btn_Aceptar_Click(sender, null);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Chk_Deducible_15_CheckedChanged
    ///DESCRIPCIÓN          : Evento CheckedChanged del control Chk_Deducible_15
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 11/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Chk_Deducible_15_CheckedChanged(object sender, EventArgs e)
    {
        Mantener_Un_CheckBox_Seleccionado_Grid((CheckBox)sender);
        Btn_Aceptar_Click(sender, null);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Chk_Deducible_20_CheckedChanged
    ///DESCRIPCIÓN          : Evento CheckedChanged del control Chk_Deducible_20
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 11/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Chk_Deducible_20_CheckedChanged(object sender, EventArgs e)
    {
        Mantener_Un_CheckBox_Seleccionado_Grid((CheckBox)sender);
        Btn_Aceptar_Click(sender, null);
    }
    protected void Grid_Tasas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Cargar_Tasas_Traslado(e.NewPageIndex);
        Grid_Tasas.SelectedIndex = (-1);
    }
}
