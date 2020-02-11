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
using Presidencia.Operacion_Predial_Traslado.Negocio;
using Presidencia.Catalogo_Conceptos.Negocio;
using Presidencia.Sessiones;
using System.IO;

public partial class paginas_predial_Frm_Ope_Pre_Calculo_Impuestos : System.Web.UI.Page
{

    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Page_Load
    ///DESCRIPCIÓN          : Método que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string Ventana_Modal;

            Llenar_Combo_Notarios();
            Llenar_Grid_Contrarecibos(0);
            Limpiar_Campos();
            Habilitar_Campos(false);
            Tab_Contenedor_Pestañas.ActiveTabIndex = 0;
            Tab_Contenedor_Pestañas_Contrarecibos.ActiveTabIndex = 0;
            Tab_Contenedor_Pestañas.Tabs[1].Enabled = false;
            Btn_Calculo_Impuesto.Visible = false;

            //Scrip para mostrar Ventana Modal de las Tasas de Traslado
            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Calculo_Impuestos/Frm_Menu_Pre_Tasas_Traslado_Dominio.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Tasas_Traslado_Dominio.Attributes.Add("onclick", Ventana_Modal);

            //Scrip para mostrar Ventana Modal de las Multas
            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Calculo_Impuestos/Frm_Menu_Pre_Multas.aspx', 'center:yes;resizable:no;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Multas.Attributes.Add("onclick", Ventana_Modal);

            //Scrip para mostrar Ventana Modal de los Tipos de Divisiones y Lotificaciones
            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Calculo_Impuestos/Frm_Menu_Pre_Tipos_Divisiones_Lotificaciones.aspx', 'center:yes;resizable:no;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Tipo_Division_Lotificacion.Attributes.Add("onclick", Ventana_Modal);
        }
        Session["TIPO_CATALOGO"] = "TRASLADO";
        Div_Contenedor_Msj_Error.Visible = false;
    }

    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Notarios
    ///DESCRIPCIÓN: Llena el Combo de Notarios
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Notarios()
    {
        try
        {
            Cls_Ope_Pre_Traslado_Negocio Traslado_Dominio = new Cls_Ope_Pre_Traslado_Negocio();
            Traslado_Dominio.P_Tipo_DataTable = "LISTAR_NOTARIOS";
            DataTable Notarios = Traslado_Dominio.Consultar_DataTable();
            DataRow Fila_Notario = Notarios.NewRow();
            Fila_Notario["NOTARIO_ID"] = HttpUtility.HtmlDecode("00000");
            Fila_Notario["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Notarios.Rows.InsertAt(Fila_Notario, 0);
            Cmb_Notarios.DataSource = Notarios;
            Cmb_Notarios.DataValueField = "NOTARIO_ID";
            Cmb_Notarios.DataTextField = "NOMBRE";
            Cmb_Notarios.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Limpiar_Campos
    ///DESCRIPCIÓN          : Limpia los campo de la forma
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Campos()
    {
        Lbl_Calculo_Impuesto_Base_Gravable_T_Dominio.Text = "$0.00";
        Lbl_Calculo_Impuesto_Constancia_No_Adeudo.Text = "$0.00";
        Lbl_Calculo_Impuesto_Cuenta_Predial.Text = "";
        Lbl_Calculo_Impuesto_Imp_Div_Lot.Text = "$0.00";
        Lbl_Calculo_Impuesto_Impuesto_Traslado_Dominio.Text = "$0.00";
        Lbl_Calculo_Impuesto_Multa.Text = "$0.00";
        Lbl_Calculo_Impuesto_Tasa_Division_Lotificacion.Text = "0.00%";
        Lbl_Calculo_Impuesto_Tipo_Division_Lotificacion.Text = "Seleccione uno -->";
        Lbl_Calculo_Impuesto_Total.Text = "$0.00";

        Hidden_Tasa_Trasaldo_Dominio.Value = "0";

        Txt_Calculo_Impuesto_Base_Impuesto.Text = "$0.00";
        Txt_Calculo_Impuesto_Base_Impuesto_2.Text = "$0.00";
        Txt_Calculo_Impuesto_Minimo_Elevado_Año.Text = "$0.00";
        Txt_Calculo_Impuesto_Recargos.Text = "$0.00";
        Txt_Calculo_Impuesto_Tasa_Traslado_Dominio.Text = "0.00%";

        Chk_Predio_Colindante.Checked = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Habilitar_Campos
    ///DESCRIPCIÓN          : Habilita o dehabilita de acuerdo al valor del parámetro indicado
    ///PARAMETROS           : Habilita: True/False
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Habilitar_Campos(Boolean Habilita)
    {
        Txt_Calculo_Impuesto_Base_Impuesto.Enabled = Habilita;
        Txt_Calculo_Impuesto_Base_Impuesto_2.Enabled = Habilita;
        Txt_Calculo_Impuesto_Minimo_Elevado_Año.Enabled = Habilita;
        Txt_Calculo_Impuesto_Recargos.Enabled = Habilita;
        Txt_Calculo_Impuesto_Tasa_Traslado_Dominio.Enabled = Habilita;

        Btn_Calcular.Enabled = Habilita;
        Btn_Tasas_Traslado_Dominio.Enabled = Habilita;
        Btn_Multas.Enabled = Habilita;
        Btn_Tipo_Division_Lotificacion.Enabled = Habilita;

        Chk_Predio_Colindante.Enabled = Habilita;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Calcular_Impuesto_Traslado_Dominio
    ///DESCRIPCIÓN          : Calcula el Impuesto del Traslado de Dominio
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 30/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Calcular_Impuesto_Traslado_Dominio()
    {
        Double Base_Impuesto;
        Double Minimo_Elevado_Año;
        Double Tasa_Traslado_Dominio;
        Double Base_Gravable_Traslado_Dominio;
        Double Impuesto_Traslado_Dominio;
        Double Tasa_Division_Lotificacion;
        Double Impuesto_Division_Lotificacion;
        Double Constancia_No_Adeudos;
        Double Multas;
        Double Recargos;

        Base_Impuesto = Convert.ToDouble(Txt_Calculo_Impuesto_Base_Impuesto.Text.Replace("$", ""));
        Minimo_Elevado_Año = Convert.ToDouble(Txt_Calculo_Impuesto_Minimo_Elevado_Año.Text.Replace("$", ""));
        Tasa_Traslado_Dominio = Convert.ToDouble(Txt_Calculo_Impuesto_Tasa_Traslado_Dominio.Text.Replace("%", ""));

        Base_Gravable_Traslado_Dominio = Base_Impuesto - Minimo_Elevado_Año;

        Lbl_Calculo_Impuesto_Base_Gravable_T_Dominio.Text = Base_Gravable_Traslado_Dominio.ToString("###.00");
        Lbl_Calculo_Impuesto_Impuesto_Traslado_Dominio.Text = (Base_Gravable_Traslado_Dominio * Tasa_Traslado_Dominio).ToString("###.00");

        Impuesto_Traslado_Dominio = Convert.ToDouble(Lbl_Calculo_Impuesto_Impuesto_Traslado_Dominio.Text.Replace("$", ""));
        Base_Impuesto = Convert.ToDouble(Txt_Calculo_Impuesto_Base_Impuesto_2.Text.Replace("$", ""));
        Tasa_Division_Lotificacion = Convert.ToDouble(Lbl_Calculo_Impuesto_Tasa_Division_Lotificacion.Text.Replace("%", ""));
        Constancia_No_Adeudos = Convert.ToDouble(Lbl_Calculo_Impuesto_Constancia_No_Adeudo.Text.Replace("$", ""));
        Multas = Convert.ToDouble(Lbl_Calculo_Impuesto_Multa.Text.Replace("$", ""));
        Recargos = Convert.ToDouble(Txt_Calculo_Impuesto_Recargos.Text.Replace("$", ""));

        Impuesto_Division_Lotificacion = Base_Impuesto * Tasa_Division_Lotificacion;

        Lbl_Calculo_Impuesto_Imp_Div_Lot.Text = Impuesto_Division_Lotificacion.ToString("###.00");
        Lbl_Calculo_Impuesto_Total.Text = (Impuesto_Traslado_Dominio + Impuesto_Division_Lotificacion + Constancia_No_Adeudos + Multas + Recargos).ToString("###.00");
    }

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN : Calcular_Total_Impuestos
    /////DESCRIPCIÓN          : Calcula el Impuesto del Traslado de Dominio
    /////PARAMETROS           : Base_Impuesto: tipo double
    /////                       Minimo_Elevado_Año: tipo double
    /////                       Tasa_Traslado_Dominio: tipo double
    /////CREO                 : Antonio Salvador Benavides Guardado
    /////FECHA_CREO           : 30/Noviembre/2010 
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //private void Calcular_Total_Impuestos()
    //{
    //    Double Impuesto_Traslado_Dominio;
    //    Double Base_Impuesto;
    //    Double Tasa_Division_Lotificacion;
    //    Double Constancia_No_Adeudos;
    //    Double Multas;
    //    Double Recargos;
    //    Double Impuesto_Division_Lotificacion;

    //    Impuesto_Traslado_Dominio = Convert.ToDouble(Txt_Calculo_Impuesto_Tasa_Traslado_Dominio.Text.Replace("$", ""));

    //    Base_Impuesto = Convert.ToDouble(Txt_Calculo_Impuesto_Base_Impuesto_2.Text.Replace("$", ""));
    //    Tasa_Division_Lotificacion = Convert.ToDouble(Lbl_Calculo_Impuesto_Tasa_Division_Lotificacion.Text.Replace("%", ""));
    //    Constancia_No_Adeudos = Convert.ToDouble(Lbl_Calculo_Impuesto_Constancia_No_Adeudo.Text.Replace("$", ""));
    //    Multas = Convert.ToDouble(Lbl_Calculo_Impuesto_Multa.Text.Replace("$", ""));
    //    Recargos = Convert.ToDouble(Txt_Calculo_Impuesto_Recargos.Text.Replace("$", ""));

    //    Impuesto_Division_Lotificacion = Base_Impuesto * Tasa_Division_Lotificacion;

    //    Lbl_Calculo_Impuesto_Imp_Div_Lot.Text = Convert.ToString(Impuesto_Division_Lotificacion);
    //    Lbl_Calculo_Impuesto_Total.Text = Convert.ToString(Impuesto_Traslado_Dominio + Impuesto_Division_Lotificacion + Constancia_No_Adeudos + Multas + Recargos);
    //}

    #region Grid

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Contrarecibos
    ///DESCRIPCIÓN: Llena el Grid de Contrarecibos
    ///PARAMETROS:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_View
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Contrarecibos(Int32 Pagina)
    {
        try
        {
            Cls_Ope_Pre_Traslado_Negocio Traslado_Dominio = new Cls_Ope_Pre_Traslado_Negocio();
            Traslado_Dominio.P_Tipo_DataTable = "LISTAR_CONTRARECIBOS";
            if (Session["Tipo_Busqueda"] != null)
            {
                String Tipo_Busqueda = Session["Tipo_Busqueda"].ToString();
                if (Tipo_Busqueda.Trim().Equals("CONTRARECIBOS"))
                {
                    Traslado_Dominio.P_Cuenta_Predial_ID = Txt_Cuenta_Predial.Text.Trim();
                    Traslado_Dominio.P_No_Contrarecibo = Txt_No_Contrarecibo.Text.Trim();
                    if (Txt_Fecha_Escritura.Text.Trim().Length > 0)
                    {
                        Traslado_Dominio.P_Buscar_Fecha_Escritura = true;
                        Traslado_Dominio.P_Fecha_Escritura = Convert.ToDateTime(Txt_Fecha_Escritura.Text.Trim());
                    }
                    if (Txt_Fecha_Liberacion.Text.Trim().Length > 0)
                    {
                        Traslado_Dominio.P_Buscar_Fecha_Liberacion = true;
                        Traslado_Dominio.P_Fecha_Liberacion = Convert.ToDateTime(Txt_Fecha_Liberacion.Text.Trim());
                    }
                }
                else if (Tipo_Busqueda.Trim().Equals("LISTADOS"))
                {
                    Traslado_Dominio.P_Listado_ID = Txt_No_Listado.Text.Trim();
                    if (Txt_Fecha_Generacion.Text.Trim().Length > 0)
                    {
                        Traslado_Dominio.P_Buscar_Fecha_Generacion = true;
                        Traslado_Dominio.P_Fecha_Generacion = Convert.ToDateTime(Txt_Fecha_Generacion.Text.Trim());
                    }
                    if (Cmb_Notarios.SelectedIndex > 0)
                    {
                        Traslado_Dominio.P_Notario_ID = Cmb_Notarios.SelectedItem.Value;
                    }
                }
            }
            Traslado_Dominio.P_Con_Cuenta_Predial = true;
            Grid_Contrarecibos.DataSource = Traslado_Dominio.Consultar_DataTable();
            Grid_Contrarecibos.PageIndex = Pagina;
            Grid_Contrarecibos.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #endregion
    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Contrarecibos_RowDataBound
    ///DESCRIPCIÓN: Evento de RowDataBound del Grid de Contrarecibos
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Contrarecibos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[2].Text == null || e.Row.Cells[2].Text.Trim().Equals("") || e.Row.Cells[2].Text.Trim().Equals("SIN REGISTRO"))
                {
                    e.Row.Cells[0].Enabled = false;
                }
                else
                {
                    e.Row.Cells[0].Enabled = true;
                }
                if (e.Row.Cells[7].Text.Trim().Equals("TRASLADO"))
                {
                    e.Row.Cells[0].Enabled = false;
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

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Filtros_Buscar_Contrarecibos_Click
    ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Limpieza de los filtros
    ///             para la busqueda por parte de los Contrarecibos.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************    
    protected void Btn_Limpiar_Filtros_Buscar_Contrarecibos_Click(object sender, ImageClickEventArgs e)
    {
        Txt_Cuenta_Predial.Text = "";
        Txt_No_Contrarecibo.Text = "";
        Txt_Fecha_Escritura.Text = "";
        Txt_Fecha_Liberacion.Text = "";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Contrarecibos_Click
    ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Busqueda de los
    ///             Contrarecibos
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************    
    protected void Btn_Buscar_Contrarecibos_Click(object sender, ImageClickEventArgs e)
    {
        Session["Tipo_Busqueda"] = "CONTRARECIBOS";
        Llenar_Grid_Contrarecibos(0);
        if (Grid_Contrarecibos.Rows.Count == 0 && (Txt_Cuenta_Predial.Text.Trim().Length > 0 || Txt_No_Contrarecibo.Text.Trim().Length > 0 || Txt_Fecha_Escritura.Text.Trim().Length > 0 || Txt_Fecha_Liberacion.Text.Trim().Length > 0))
        {
            Lbl_Ecabezado_Mensaje.Text = "No se encontraron Datos para los filtros establecidos";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
            Txt_Cuenta_Predial.Text = "";
            Txt_No_Contrarecibo.Text = "";
            Txt_Fecha_Escritura.Text = "";
            Txt_Fecha_Liberacion.Text = "";
            Session.Remove("Tipo_Busqueda");
            Llenar_Grid_Contrarecibos(0);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Listado_Click
    ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Busqueda de los
    ///             Listados
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************    
    protected void Btn_Buscar_Listado_Click(object sender, ImageClickEventArgs e)
    {
        Session["Tipo_Busqueda"] = "LISTADOS";
        Llenar_Grid_Contrarecibos(0);
        if (Grid_Contrarecibos.Rows.Count == 0 && (Txt_No_Listado.Text.Trim().Length > 0 || Txt_Fecha_Generacion.Text.Trim().Length > 0 || Cmb_Notarios.SelectedIndex > 0))
        {
            Lbl_Ecabezado_Mensaje.Text = "No se encontraron Datos para los filtros establecidos";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
            Session.Remove("Tipo_Busqueda");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Filtros_Buscar_Listado_Click
    ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Limpieza de los filtros
    ///             para la busqueda por parte de los Listados.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 09/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Limpiar_Filtros_Buscar_Listado_Click(object sender, ImageClickEventArgs e)
    {
        Txt_No_Listado.Text = "";
        Txt_Fecha_Generacion.Text = "";
        Cmb_Notarios.SelectedIndex = 0;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Contrarecibos_SelectedIndexChanged
    ///DESCRIPCIÓN          : Evento SelectedIndexChange del Grid de Contrarecibos
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Noviembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Grid_Contrarecibos_SelectedIndexChanged(object sender, EventArgs e)
    {
        Limpiar_Campos();
        Habilitar_Campos(false);
        Lbl_Calculo_Impuesto_Cuenta_Predial.Text = (HttpUtility.HtmlDecode(Grid_Contrarecibos.SelectedRow.Cells[2].Text)).ToString();
        Tab_Contenedor_Pestañas.Tabs[1].Enabled = true;
        Tab_Contenedor_Pestañas.ActiveTabIndex = 1;
        Btn_Calculo_Impuesto.AlternateText = "Calcular";
        Btn_Calculo_Impuesto.Visible = true;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Calculo_Impuesto_Click
    ///DESCRIPCIÓN          : Evento Click del botón Calcular Impuesto
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Noviembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Calculo_Impuesto_Click(object sender, ImageClickEventArgs e)
    {
        Tab_Contenedor_Pestañas.ActiveTabIndex = 1;
        if (Btn_Calculo_Impuesto.AlternateText == "Calcular")
        {
            Btn_Calculo_Impuesto.AlternateText = "Aceptar";
            Btn_Calculo_Impuesto.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
            Btn_Calculo_Impuesto_Cancelar.AlternateText = "Cancelar";
            Btn_Calculo_Impuesto_Cancelar.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";

            Btn_Calculo_Impuesto.AlternateText = "Aceptar";
            Habilitar_Campos(true);
            Txt_Calculo_Impuesto_Base_Impuesto.Focus();
        }
        else
        {
            Habilitar_Campos(false);
            Btn_Calculo_Impuesto.AlternateText = "Calcular";
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Calculo_Impuest_Cancelar_Click
    ///DESCRIPCIÓN          : Evento Click del botón Cancelar
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Noviembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Calculo_Impuesto_Cancelar_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Calculo_Impuesto_Cancelar.AlternateText == "Salir" && Tab_Contenedor_Pestañas.ActiveTabIndex == 0)
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Limpiar_Campos();
            Habilitar_Campos(false);
            if (Btn_Calculo_Impuesto.AlternateText == "Calcular")
            {
                Tab_Contenedor_Pestañas.ActiveTabIndex = 0;
                Tab_Contenedor_Pestañas.Tabs[1].Enabled = false;
                Btn_Calculo_Impuesto.Visible = false;
            }
            else
            {
                if (Btn_Calculo_Impuesto.AlternateText == "Aceptar")
                {
                    Lbl_Calculo_Impuesto_Cuenta_Predial.Text = (HttpUtility.HtmlDecode(Grid_Contrarecibos.SelectedRow.Cells[2].Text)).ToString();
                    Tab_Contenedor_Pestañas.ActiveTabIndex = 1;
                }
            }
            Btn_Calculo_Impuesto.AlternateText = "Calcular";
            Btn_Calculo_Impuesto.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Btn_Calculo_Impuesto_Cancelar.AlternateText = "Salir";
            Btn_Calculo_Impuesto_Cancelar.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Calculo_Impuesto_Base_Impuesto_TextChanged
    ///DESCRIPCIÓN          : Evento TextChanged del TextBox Base del Impuesto
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 30/Noviembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Txt_Calculo_Impuesto_Base_Impuesto_TextChanged(object sender, EventArgs e)
    {
        Calcular_Impuesto_Traslado_Dominio();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Calculo_Impuesto_Minimo_Elevado_Año_TextChanged
    ///DESCRIPCIÓN          : Evento TextChanged del TextBox Mínimo Elevado al Año
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 30/Noviembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Txt_Calculo_Impuesto_Minimo_Elevado_Año_TextChanged(object sender, EventArgs e)
    {
        Calcular_Impuesto_Traslado_Dominio();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Calculo_Impuesto_Tasa_Traslado_Dominio_TextChanged
    ///DESCRIPCIÓN          : Evento TextChanged del TextBox Tasa de Traslado de Dominio
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 30/Noviembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Txt_Calculo_Impuesto_Tasa_Traslado_Dominio_TextChanged(object sender, EventArgs e)
    {
        Calcular_Impuesto_Traslado_Dominio();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Calculo_Impuesto_Base_Impuesto_2_TextChanged
    ///DESCRIPCIÓN          : Evento TextChanged del TextBox Base de Impuesto
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 30/Noviembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Txt_Calculo_Impuesto_Base_Impuesto_2_TextChanged(object sender, EventArgs e)
    {
        Calcular_Impuesto_Traslado_Dominio();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Tasas_Traslado_Dominio_Click
    ///DESCRIPCIÓN          : Evento Click del BotonImage Tasa Traslado de Dominio
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 30/Noviembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Tasas_Traslado_Dominio_Click(object sender, ImageClickEventArgs e)
    {
        Boolean Busqueda_Tasa_Traslado_Dominio;
        string Deducible_Normal;
        string Tasa;

        Busqueda_Tasa_Traslado_Dominio = Convert.ToBoolean(Session["BUSQUEDA_TASA_TRASLADO_DOMINIO"]);
        if (Busqueda_Tasa_Traslado_Dominio)
        {
            if (Session["DEDUCIBLE"] != null)
            {
                Deducible_Normal = Convert.ToString(Session["DEDUCIBLE"]);
                Txt_Calculo_Impuesto_Minimo_Elevado_Año.Text = Deducible_Normal;
            }
            if (Session["TASA"] != null)
            {
                Tasa = Convert.ToString(Session["TASA"]);
                Hidden_Tasa_Trasaldo_Dominio.Value = Tasa;
            }
        }
        Session.Remove("BUSQUEDA_TASA_TRASLADO_DOMINIO");
        Session.Remove("CONCEPTO_PREDIAL_ID");
        Session.Remove("IMPUESTO_ID_TRASLACION");
        Session.Remove("DEDUCIBLE");
        Session.Remove("TASA");

        if (!Chk_Predio_Colindante.Checked)
        {
            Txt_Calculo_Impuesto_Tasa_Traslado_Dominio.Text = Hidden_Tasa_Trasaldo_Dominio.Value.Replace("$", "");
        }

        Calcular_Impuesto_Traslado_Dominio();

        if (Chk_Predio_Colindante.Checked)
        {
            Txt_Calculo_Impuesto_Tasa_Traslado_Dominio.Text = Lbl_Calculo_Impuesto_Base_Gravable_T_Dominio.Text;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Multas_Click
    ///DESCRIPCIÓN          : Evento Click del BotonImage Multas
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 02/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Multas_Click(object sender, ImageClickEventArgs e)
    {
        Boolean Busqueda_Cuota_Multa;
        string Monto;

        Busqueda_Cuota_Multa = Convert.ToBoolean(Session["BUSQUEDA_CUOTA_MULTA"]);
        if (Busqueda_Cuota_Multa)
        {
            if (Session["MONTO"] != null)
            {
                Monto = Convert.ToString(Session["MONTO"]);
                Lbl_Calculo_Impuesto_Multa.Text = Monto;
            }
        }
        Session.Remove("BUSQUEDA_CUOTA_MULTA");
        Session.Remove("MULTA_ID");
        Session.Remove("MULTA_CUOTA_ID");
        Session.Remove("MONTO");

        Calcular_Impuesto_Traslado_Dominio();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Tipo_Division_Lotificacion_Click
    ///DESCRIPCIÓN          : Evento Click del BotonImage Tipo_Division_Lotificacion
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 10/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Tipo_Division_Lotificacion_Click(object sender, ImageClickEventArgs e)
    {
        Boolean Busqueda_Tipo_Division_Lotificacion;
        String Tasa;
        String Concepto;

        Busqueda_Tipo_Division_Lotificacion = Convert.ToBoolean(Session["BUSQUEDA_TASA_DIVISION_LOTIFICACION"]);
        if (Busqueda_Tipo_Division_Lotificacion)
        {
            if (Session["TASA"] != null)
            {
                Concepto = Convert.ToString(Session["CONCEPTO"]);
                Lbl_Calculo_Impuesto_Tipo_Division_Lotificacion.Text = Concepto;
                Tasa = Convert.ToString(Session["TASA"]);
                Lbl_Calculo_Impuesto_Tasa_Division_Lotificacion.Text = Tasa;
            }
        }
        Session.Remove("BUSQUEDA_TASA_DIVISION_LOTIFICACION");
        Session.Remove("DIVISION_ID");
        Session.Remove("CONCEPTO");
        Session.Remove("IMPUESTO_DIVISION_LOT_ID");
        Session.Remove("TASA");

        Calcular_Impuesto_Traslado_Dominio();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Chk_Predio_Colindante_CheckedChanged
    ///DESCRIPCIÓN          : Evento CheckedChanged del control CheckBox Chk_Predio_Colindante
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Chk_Predio_Colindante_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Predio_Colindante.Checked)
        {
            Txt_Calculo_Impuesto_Tasa_Traslado_Dominio.Text = Lbl_Calculo_Impuesto_Base_Gravable_T_Dominio.Text.Replace("$", "");
            Txt_Calculo_Impuesto_Tasa_Traslado_Dominio.BorderStyle = BorderStyle.None;
        }
        else
        {
            Txt_Calculo_Impuesto_Tasa_Traslado_Dominio.Text = Hidden_Tasa_Trasaldo_Dominio.Value;
            Txt_Calculo_Impuesto_Tasa_Traslado_Dominio.BorderStyle = BorderStyle.NotSet;
        }
        Calcular_Impuesto_Traslado_Dominio();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Calcular_Click
    ///DESCRIPCIÓN          : Evento Click del control ImageButton Btn_Calcular
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 23/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Calcular_Click(object sender, ImageClickEventArgs e)
    {
        Calcular_Impuesto_Traslado_Dominio();
    }
    #endregion
}