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
using Presidencia.Catalogo_Contribuyentes.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
public partial class paginas_Predial_Ventanas_Emergentes_Orden_Variacion_Frm_Menu_Pre_Propietarios : System.Web.UI.Page
{

    #region
    public static String M_Paterno;
    public static String M_Materno;
    public static String M_Nombre;
    public static DateTime M_Fecha;
    public static String M_RFC;
    #endregion
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Page_Load
    ///DESCRIPCIÓN          : Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio salvador Benavides Guardado
    ///FECHA_CREO           : 16/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        if (!IsPostBack)
        {
            Session["BUSQUEDA_CONTRIBUYENTE"] = false;
            Registro.Visible = false;
            Buscar.Visible = true;
            Txt_Fecha_Nacimiento.Text = "";
        }
        Frm_Menu_Pre_Propietarios.Page.Title = "Propietarios";
        Lbl_Title.Text = "Selecione un Propietario por favor";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Contribuyentes
    ///DESCRIPCIÓN          : Método que carga los datos de los Propietarios existentes en el catálogo de Cat_Pre_Contribuyentes
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 16/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private Boolean Cargar_Contribuyentes(int Page_Index)
    {
        Boolean Contribuyentes_Cargados = false;
        DataTable Dt_Contributentes;
        try
        {
            Cls_Cat_Pre_Contribuyentes_Negocio Contribuyentes = new Cls_Cat_Pre_Contribuyentes_Negocio();

            Contribuyentes.P_RFC = M_RFC;
            Contribuyentes.P_Nombre = M_Nombre;
            Dt_Contributentes = Contribuyentes.Consultar_Menu_Contribuyentes();
            if (Dt_Contributentes.Rows.Count > 0)
            {
                Grid_Contribuyentes.DataSource = Dt_Contributentes;
                Grid_Contribuyentes.PageIndex = Page_Index;
                Grid_Contribuyentes.DataBind();
            }
            else
            {
                Grid_Contribuyentes.DataSource = null;
                Grid_Contribuyentes.PageIndex = Page_Index;
                Grid_Contribuyentes.DataBind();
                Registro.Visible = true;
                Buscar.Visible = false;
                Btn_Regresar.AlternateText = "Cancelar";
                Btn_Guardar_Copropietario.Visible = true;
                Btn_Agregar_Co_Propietarios.Visible = false;
            }
            Contribuyentes_Cargados = true;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Contribuyentes_Cargados;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Regresar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 02/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Regresar_Click(object sender, ImageClickEventArgs e)
    {

        if (Btn_Regresar.AlternateText.Equals("Regresar"))
        {
            Session["BUSQUEDA_CONTRIBUYENTE"] = false;
            Session.Remove("CONTRIBUYENTE_ID");
            Session.Remove("CONTRIBUYENTE_NOMBRE");
            Session.Remove("RFC");
            //Session.Remove("DOMICILIO");
            //Session.Remove("INTERIOR");
            //Session.Remove("EXTERIOR");
            //Session.Remove("COLONIA");
            //Session.Remove("CIUDAD");
            //Session.Remove("CODIGO_POSTAL");
            //Session.Remove("ESTADO");
            //Cierra la ventana
            //Cierra la ventana
            string Pagina = "<script language='JavaScript'>";
            Pagina += "window.close();";
            Pagina += "</script>";
            //Page.RegisterStartupScript("Cerrar_Script", Pagina);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", "window.close();", true);
        }
        else
        {
            Registro.Visible = false;
            Buscar.Visible = true;
            Btn_Agregar_Co_Propietarios.Visible = true;
            Btn_Guardar_Copropietario.Visible = false;
            Cmb_Tipo_Persona.SelectedIndex = 0;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Busqueda_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 02/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Busqueda_Click(object sender, ImageClickEventArgs e)
    {
        String Validar_Campos = Txt_Rfc.Text.Trim() + Txt_Nombre.Text.Trim();


        if (!String.IsNullOrEmpty(Validar_Campos))
        {
            M_RFC = Txt_Rfc.Text.Trim();
            M_Nombre = Txt_Nombre.Text.Trim();

            if (!Cargar_Contribuyentes(0))
            {
                Grid_Contribuyentes.DataSource = null;
                Grid_Contribuyentes.DataBind();
                Lbl_Ecabezado_Mensaje.Text = "No hay datos qué mostrar";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;

            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Contribuyentes_PageIndexChanging
    ///DESCRIPCIÓN          : Evento PageIndexChanging del control Grid_Contribuyentes
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 16/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Grid_Contribuyentes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Cargar_Contribuyentes(e.NewPageIndex);
        Grid_Contribuyentes.SelectedIndex = (-1);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Contribuyentes_SelectedIndexChanged
    ///DESCRIPCIÓN          : Evento SelectedIndexChanged del control Grid_Contribuyentes
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 16/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Grid_Contribuyentes_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["CONTRIBUYENTE_ID"] = Grid_Contribuyentes.Rows[Grid_Contribuyentes.SelectedIndex].Cells[1].Text;
        Session["CONTRIBUYENTE_NOMBRE"] = Grid_Contribuyentes.Rows[Grid_Contribuyentes.SelectedIndex].Cells[2].Text;
        Session["RFC"] = Grid_Contribuyentes.Rows[Grid_Contribuyentes.SelectedIndex].Cells[3].Text;

        Session["BUSQUEDA_CONTRIBUYENTE"] = true;
        //Cierra la ventana
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", "window.close();", true);


    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Nombre_TextChanged
    ///DESCRIPCIÓN          : buscar datos
    ///PARAMETROS           : sender y e
    ///CREO                 : Toledo Rodriguez Jesus
    ///FECHA_CREO           : 18/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Txt_Nombre_TextChanged(object sender, EventArgs e)
    {
        ImageClickEventArgs e1 = null;
        Btn_Busqueda_Click(null, e1);
    }
    protected void Txt_Rfc_TextChanged(object sender, EventArgs e)
    {
        ImageClickEventArgs e1 = null;
        Btn_Busqueda_Click(null, e1);
    }
    protected void Btn_Agregar_Co_Propietarios_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Cat_Pre_Contribuyentes_Negocio Alta_Contribuyentes = new Cls_Cat_Pre_Contribuyentes_Negocio();
        try
        {
            if (Cmb_Tipo_Persona.SelectedValue == "SELECCIONE")
            {
                Registro.Visible = true;
                Buscar.Visible = false;
                Btn_Regresar.AlternateText = "Cancelar";
                Btn_Guardar_Copropietario.Visible = true;
                Btn_Agregar_Co_Propietarios.Visible = false;
            }
            else
            {
                Registro.Visible = false;
                Buscar.Visible = true;
                Btn_Regresar.AlternateText = "Cancelar";
                Btn_Guardar_Copropietario.Visible = true;
                Btn_Agregar_Co_Propietarios.Visible = false;
            }
            //else if (Cmb_Tipo_Persona.SelectedValue != "SELECCIONE")
            //{
            //    Alta_Contribuyentes.P_Apellido_Paterno = Txt_Apeido_Paterno.Text.Trim().ToUpper();
            //    Alta_Contribuyentes.P_Apellido_Materno = Txt_Apeido_Materno.Text.Trim().ToUpper();
            //    if (Txt_Nombre_Contribuyente.Text.Trim() != "")
            //    {
            //        Alta_Contribuyentes.P_Nombre = Txt_Nombre_Contribuyente.Text.Trim().ToUpper();
            //        if (Txt_Fecha_Nacimiento.Text != "")
            //        {
            //            Alta_Contribuyentes.P_Fecha_Nacimiento = DateTime.Parse(Txt_Fecha_Nacimiento.Text.Trim());
            //        }

            //    }
            //    if(Txt_Razon_Social .Text .Trim ()!=""){
            //        Alta_Contribuyentes.P_Nombre = Txt_Razon_Social.Text.Trim().ToUpper();
            //    }
            //    Alta_Contribuyentes.P_Tipo_Persona = Cmb_Tipo_Persona.SelectedValue.Trim();
            //    Alta_Contribuyentes.P_RFC = Txt_RFC_Contribuyente.Text.Trim();
            //    Alta_Contribuyentes.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
            //    if (Btn_Guardar_Copropietario.ToolTip == "Guardar")
            //    {
            //        Alta_Contribuyentes.Alta_Contribuyente_Orden_Variacion();
            //        Session["CONTRIBUYENTE_ID"] = Alta_Contribuyentes.P_Contribuyente_ID;
            //        if (Alta_Contribuyentes.P_Apellido_Paterno != null)
            //        {
            //            Session["CONTRIBUYENTE_NOMBRE"] = Alta_Contribuyentes.P_Nombre + " " + Alta_Contribuyentes.P_Apellido_Paterno + " " + Alta_Contribuyentes.P_Apellido_Materno;
            //        }
            //        else
            //        {
            //            Session["CONTRIBUYENTE_NOMBRE"] = Alta_Contribuyentes.P_Nombre;
            //        }
            //        Session["RFC"] = Txt_RFC_Contribuyente.Text.Trim();
            //        Btn_Guardar_Copropietario.Visible = false;
            //        Btn_Agregar_Co_Propietarios.Visible = true;
            //        Session["BUSQUEDA_CONTRIBUYENTE"] = true;
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Contribuyentes", "alert('Alta de Contribuyente Exitosa');", true);
            //        string Pagina = "<script language='JavaScript'>";
            //        Pagina += "window.close();";
            //        Pagina += "</script>";
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", "window.close();", true);
            //    }
            //    else
            //    {
            //    }
            //}

        }
        catch (Exception Ex)
        {
        }

    }
    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Validar_Campos
    /// DESCRIPCIÓN: Revisar que los campos obligatorios hayan sido llenados y si no, generar el mensaje 
    ///             correspondiente.
    /// PARÁMETROS:
    /// CREO: Christian Perez Ibarra
    /// FECHA_CREO: 31/Sept/2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Validar_Campos()
    {
        //Si falta alguno de los campos mencionarlo en la etiqueta Lbl_Mensaje_Error para mostrarla 
        Lbl_Mensaje_Error.Text = "";
        //if (Txt_Cuenta_Predial.Text == "")  //Validar campo cuenta predial (no vacío)
        //{
        //    Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir la cuenta predial <br />";
        //}
    }
    protected void Cmb_Tipo_Persona_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cmb_Tipo_Persona.SelectedValue == "FISICA")
        {
            Fisica.Visible = true;
            Moral.Visible = false;
        }
        else if (Cmb_Tipo_Persona.SelectedValue == "MORAL")
        {
            Fisica.Visible = false;
            Moral.Visible = true;
        }
    }
    protected void Btn_Guardar_Copropietario_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Cat_Pre_Contribuyentes_Negocio Alta_Contribuyentes = new Cls_Cat_Pre_Contribuyentes_Negocio();
        try
        {
            if (Cmb_Tipo_Persona.SelectedValue == "SELECCIONE")
            {
                Registro.Visible = true;
                Buscar.Visible = false;
                Btn_Regresar.AlternateText = "Cancelar";
                Btn_Guardar_Copropietario.Visible = true;
                Btn_Agregar_Co_Propietarios.Visible = false;
            }
            else if (Cmb_Tipo_Persona.SelectedValue != "SELECCIONE")
            {
                Lbl_Ecabezado_Mensaje.Text = "";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = false;
                if (Cmb_Tipo_Persona.SelectedItem.Text == "FISICA")
                {
                    if (Txt_Apeido_Paterno.Text.Trim() == "")
                    {
                        Lbl_Mensaje_Error.Text += "-Apellido Paterno<BR>";
                    }
                    //if (Txt_Apeido_Materno.Text.Trim() == "")
                    //{
                    //    Lbl_Mensaje_Error.Text += "-Apellido Materno<BR>";
                    //}
                    if (Txt_Nombre_Contribuyente.Text.Trim() == "")
                    {
                        Lbl_Mensaje_Error.Text += "-Nombre<BR>";
                    }
                    if (Lbl_Mensaje_Error.Text.Trim() != "")
                    {
                        Lbl_Ecabezado_Mensaje.Text = "Faltan los siguientes datos:";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
                else
                {
                    if (Cmb_Tipo_Persona.SelectedItem.Text == "MORAL")
                    {
                        if (Txt_Razon_Social.Text.Trim() == "")
                        {
                            Lbl_Mensaje_Error.Text += "-Razón Social<BR>";
                        }
                        if (Lbl_Mensaje_Error.Text.Trim() != "")
                        {
                            Lbl_Ecabezado_Mensaje.Text = "Faltan los siguientes datos:";
                            Div_Contenedor_Msj_Error.Visible = true;
                        }
                    }
                }
                if (!Div_Contenedor_Msj_Error.Visible)
                {
                    Alta_Contribuyentes.P_Apellido_Paterno = Txt_Apeido_Paterno.Text.Trim().ToUpper();
                    Alta_Contribuyentes.P_Apellido_Materno = Txt_Apeido_Materno.Text.Trim().ToUpper();
                    if (Txt_Nombre_Contribuyente.Text.Trim() != "")
                    {
                        Alta_Contribuyentes.P_Nombre = Txt_Nombre_Contribuyente.Text.Trim().ToUpper();
                        if (Txt_Fecha_Nacimiento.Text != "")
                        {
                            DateTime Fecha_Temp;
                            if (DateTime.TryParse(Txt_Fecha_Nacimiento.Text.Trim(), out Fecha_Temp))
                                Alta_Contribuyentes.P_Fecha_Nacimiento = Fecha_Temp;
                            else
                                Alta_Contribuyentes.P_Fecha_Nacimiento = DateTime.ParseExact(Txt_Fecha_Nacimiento.Text.Trim(), "dd/MM/yyyy", null);
                        }

                    }
                    if (Txt_Razon_Social.Text.Trim() != "")
                    {
                        Alta_Contribuyentes.P_Nombre = Txt_Razon_Social.Text.Trim().ToUpper();
                    }
                    Alta_Contribuyentes.P_Tipo_Persona = Cmb_Tipo_Persona.SelectedValue.Trim();
                    if (Txt_RFC_Contribuyente.Text.Trim() != "")
                    {
                        Alta_Contribuyentes.P_RFC = Txt_RFC_Contribuyente.Text.Trim().ToUpper();
                    }
                    //else
                    //{
                    //    Alta_Contribuyentes.P_RFC = "-";
                    //}
                    Alta_Contribuyentes.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                    if (Btn_Guardar_Copropietario.ToolTip == "Guardar")
                    {
                        bool Res = Alta_Contribuyentes.Alta_Contribuyente_Orden_Variacion();
                        Session["CONTRIBUYENTE_ID"] = Alta_Contribuyentes.P_Contribuyente_ID;
                        if (Alta_Contribuyentes.P_Apellido_Paterno != null)
                        {
                            Session["CONTRIBUYENTE_NOMBRE"] = Alta_Contribuyentes.P_Apellido_Paterno + " " + Alta_Contribuyentes.P_Apellido_Materno + " " + Alta_Contribuyentes.P_Nombre;
                        }
                        else
                        {
                            Session["CONTRIBUYENTE_NOMBRE"] = Alta_Contribuyentes.P_Nombre;
                        }
                        Session["RFC"] = Txt_RFC_Contribuyente.Text.Trim();
                        Btn_Guardar_Copropietario.Visible = false;
                        Btn_Agregar_Co_Propietarios.Visible = true;
                        Session["BUSQUEDA_CONTRIBUYENTE"] = true;
                        if(Res)
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Contribuyentes", "alert('Alta de Contribuyente Exitosa');", true);
                        else
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Contribuyentes", "alert('No se dio de alta al Contribuyente porque ya existe');", true);
                        string Pagina = "<script language='JavaScript'>";
                        Pagina += "window.close();";
                        Pagina += "</script>";
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", "window.close();", true);

                        Registro.Visible = false;
                        Buscar.Visible = true;
                        Btn_Regresar.AlternateText = "Regresar";
                        Btn_Guardar_Copropietario.Visible = false;
                        Btn_Agregar_Co_Propietarios.Visible = true;

                        Txt_Nombre.Text = HttpUtility.HtmlDecode(Session["CONTRIBUYENTE_NOMBRE"].ToString().Trim());
                        Txt_Rfc.Text = HttpUtility.HtmlDecode(Session["RFC"].ToString().Trim());
                        Btn_Busqueda_Click(sender, e);
                        if (Grid_Contribuyentes.Rows.Count == 1)
                        {
                            Grid_Contribuyentes.SelectedIndex = 0;
                            Grid_Contribuyentes_SelectedIndexChanged(sender, null);
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
        }
    }
}
