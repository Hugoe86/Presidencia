using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Historial_Pagos.Negocio;
using Presidencia.Catalogo_Modulos.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Contribuyentes.Negocio;
using Presidencia.Reportes;
public partial class paginas_Predial_Frm_Ope_Pre_Historial_Pagos : System.Web.UI.Page
{
    #region Pago_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS:     
    ///CREO: Christian Perez Ibarra
    ///FECHA_CREO: 10/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************        
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //string Ventana_Modal;

            //Configuracion_Formulario(true);
            //Llenar_Tabla_Impuestos_Derechos_Supervision(0);
            Llenar_Combo_Lugar_Pago();
            string Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no ');";
            Btn_Mostrar_Busqueda_Avanzada.Attributes.Add("onclick", Ventana_Modal);
            Habilitar_Botones();
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Resumen_Predio
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente del Resumen de Predial con la ruta y parámtros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Otubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Emergente_Resumen_Predio()
    {
        String Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Resumen_Predio.aspx";
        String Propiedades = ", 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHide:true;help:no;scroll:no');";
        Btn_Detalles_Cuenta_Predial.Attributes.Add("onclick", Ventana_Modal + "?Cuenta_Predial=" + Txt_Cuenta_Predial.Text.Trim() + "'" + Propiedades);
    }
    #endregion
    protected void Habilitar_Botones()
    {
        Txt_Cuenta_Predial.Enabled = false;
        Txt_Documento.Enabled = false;
        Txt_Periodo_Corriente.Enabled = false;
        Txt_Monto_Corriente.Enabled = false;
        Txt_Periodo_Rezago.Enabled = false;
        Txt_Monto_Rezago.Enabled = false;
        Txt_Recargos_Ordinarios.Enabled = false;
        Txt_Recargos_Moratorios.Enabled = false;
        Txt_Honorarios.Enabled = false;
        Txt_Multas.Enabled = false;
        Txt_Gastos_Ejecucion.Enabled = false;
        Txt_Descuento_Recargos.Enabled = false;
        Txt_Descuento_Honorarios.Enabled = false;
        Txt_Descuento_Pronto_Pago.Enabled = false;
    }
    #region Llenar_Combo
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Lugar_Pago
    ///DESCRIPCIÓN: Metodo que llena el Combo de Lugar de Pago del catalogo de Modulos.
    ///PROPIEDADES:     
    ///CREO: Christian Perez Ibarra
    ///FECHA_CREO: 10/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Lugar_Pago()
    {
        try
        {
            Cls_Cat_Pre_Modulos_Negocio Lugar_Pago = new Cls_Cat_Pre_Modulos_Negocio();
            DataTable Dt_Lugar_Pago = Lugar_Pago.Consultar_Nombre_Modulos();
            DataRow fila = Dt_Lugar_Pago.NewRow();
            fila[Cat_Pre_Modulos.Campo_Descripcion] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            fila[Cat_Pre_Modulos.Campo_Modulo_Id] = "SELECCIONE";
            Dt_Lugar_Pago.Rows.InsertAt(fila, 0);
            Cmb_Lugar_Pago.DataTextField = Cat_Pre_Modulos.Campo_Descripcion;
            Cmb_Lugar_Pago.DataValueField = Cat_Pre_Modulos.Campo_Modulo_Id;
            Cmb_Lugar_Pago.DataSource = Dt_Lugar_Pago;
            Cmb_Lugar_Pago.DataBind();
        }
        catch (Exception Ex)
        {
            //Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            //Div_Contenedor_Msj_Error.Visible = true;
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Mostrar_Busqueda_Avanzada_Click
    ///DESCRIPCIÓN          : Muestra los datos de la consulta
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Mostrar_Busqueda_Avanzada_Click(object sender, ImageClickEventArgs e)
    {
        Boolean Busqueda_Ubicaciones;
        String Cuenta_Predial_ID;
        String Cuenta_Predial;

        Busqueda_Ubicaciones = Convert.ToBoolean(Session["BUSQUEDA_CUENTAS_PREDIAL"]);
        if (Busqueda_Ubicaciones)
        {
            if (Session["CUENTA_PREDIAL_ID"] != null)
            {
                Cuenta_Predial_ID = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
                //Hdf_Cuenta_Predial_ID.Value = Cuenta_Predial_ID;
                Cuenta_Predial = Convert.ToString(Session["CUENTA_PREDIAL"]);
                Txt_Cuenta_Predial.Text = Cuenta_Predial;
            }
            Consultar_Datos_Cuenta_Constancia();
            Cargar_Ventana_Emergente_Resumen_Predio();
        }
        Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
        Session.Remove("CUENTA_PREDIAL_ID");
        Session.Remove("CUENTA_PREDIAL");
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Cuenta_Constanica
    ///DESCRIPCIÓN          : Realiza la búsqueda de los datos de la cuenta predial introducida
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Consultar_Datos_Cuenta_Constancia()
    {
        DataTable Dt_Cuentas_Predial;
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        if (Txt_Cuenta_Predial.Text.Trim() != "")
        {
            //Consulta la Cuenta Predial
            Cuentas_Predial.P_Campos_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", ";
            Cuentas_Predial.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Campo_Propietario_ID;
            Cuentas_Predial.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            Cls_Ope_Pre_Historial_Pago_Negocio Historial_Pagos = new Cls_Ope_Pre_Historial_Pago_Negocio();
            DataTable Dt_Historial_Pagos = new DataTable();
            Session["Dt_Historial"] = Dt_Historial_Pagos;

            Historial_Pagos.P_Cuenta_Predial_ID = Txt_Cuenta_Predial.Text.Trim();
            Historial_Pagos.P_Entre_Fecha = Txt_Fecha_Inicial.Text.Trim();
            Historial_Pagos.P_Y_Fecha = Txt_Fecha_Final.Text.Trim();
            Historial_Pagos.P_Recibo_Inicial = Txt_Recibo_Inicial.Text.Trim();
            Historial_Pagos.P_Recibo_Final = Txt_Recibo_Final.Text.Trim();
            Historial_Pagos.P_Lugar_Pago = Cmb_Lugar_Pago.SelectedValue.Trim();
            Historial_Pagos.P_Caja = Txt_Caja.Text.Trim();
            Dt_Historial_Pagos = Historial_Pagos.Consultar_Historial_Pagos();
            Session["Dt_Historial"] = Dt_Historial_Pagos;
            Grid_Pagos.DataSource = Dt_Historial_Pagos;
            Grid_Pagos.Columns[2].Visible = true;
            Grid_Pagos.DataBind();
            Grid_Pagos.Columns[1].Visible = false;
            Grid_Pagos.Columns[2].Visible = false;
            Grid_Pagos.Columns[3].Visible = false;
            Grid_Pagos.Columns[4].Visible = false;
            Grid_Pagos.Columns[5].Visible = false;
            Grid_Pagos.Columns[6].Visible = false;
            //Grid_Pagos.Columns[7].Visible = false;
        }
    }

    #endregion

    protected void Grid_Pagos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Cls_Ope_Pre_Historial_Pago_Negocio Historial_Pagos = new Cls_Ope_Pre_Historial_Pago_Negocio();
        DataTable Dt_Historial_Pagos = new DataTable();
        try
        {
            Grid_Pagos.SelectedIndex = (-1);
            Historial_Pagos.P_Cuenta_Predial_ID = Txt_Cuenta_Predial.Text.Trim();
            Historial_Pagos.P_Entre_Fecha = Txt_Fecha_Inicial.Text.Trim();
            Historial_Pagos.P_Y_Fecha = Txt_Fecha_Final.Text.Trim();
            Historial_Pagos.P_Recibo_Inicial = Txt_Recibo_Inicial.Text.Trim();
            Historial_Pagos.P_Recibo_Final = Txt_Recibo_Final.Text.Trim();
            Historial_Pagos.P_Lugar_Pago = Cmb_Lugar_Pago.SelectedValue.Trim();
            Historial_Pagos.P_Caja = Txt_Caja.Text.Trim();
            Dt_Historial_Pagos = Historial_Pagos.Consultar_Historial_Pagos();
            Session["Dt_Historial"] = Dt_Historial_Pagos;
            Grid_Pagos.PageIndex = e.NewPageIndex;
            Grid_Pagos.DataSource = Dt_Historial_Pagos;
            Grid_Pagos.Columns[2].Visible = true;
            Grid_Pagos.DataBind();
            Grid_Pagos.Columns[1].Visible = false;
            Grid_Pagos.Columns[2].Visible = false;
            Grid_Pagos.Columns[3].Visible = false;
            Grid_Pagos.Columns[4].Visible = false;
            Grid_Pagos.Columns[5].Visible = false;
            Grid_Pagos.Columns[6].Visible = false;
            //Grid_Pagos.Columns[7].Visible = false;


        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Grid: " + ex.Message.ToString(), ex);
        }
    }
    /// *************************************************************************************
    /// NOMBRE:              Btn_Detalles_Cuenta_Predial_Click
    /// DESCRIPCIÓN:         Evento del grid para seleccionar un folio y mostrar sus detalles
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          11/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    protected void Btn_Detalles_Cuenta_Predial_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Pre_Historial_Pago_Negocio Historial_Pagos = new Cls_Ope_Pre_Historial_Pago_Negocio();
        DataTable Dt_Historial_Pagos = new DataTable();
        Session["Dt_Historial"] = Dt_Historial_Pagos;
        try
        {
            Historial_Pagos.P_Cuenta_Predial_ID = Txt_Cuenta_Predial.Text.Trim();
            Historial_Pagos.P_Entre_Fecha = Txt_Fecha_Inicial.Text.Trim();
            Historial_Pagos.P_Y_Fecha = Txt_Fecha_Final.Text.Trim();
            Historial_Pagos.P_Recibo_Inicial = Txt_Recibo_Inicial.Text.Trim();
            Historial_Pagos.P_Recibo_Final = Txt_Recibo_Final.Text.Trim();
            Historial_Pagos.P_Lugar_Pago = Cmb_Lugar_Pago.SelectedValue.Trim();
            Historial_Pagos.P_Caja = Txt_Caja.Text.Trim();
            Dt_Historial_Pagos = Historial_Pagos.Consultar_Historial_Pagos();
            Session["Dt_Historial"] = Dt_Historial_Pagos;
            Grid_Pagos.DataSource = Dt_Historial_Pagos;
            Grid_Pagos.Columns[2].Visible = true;
            Grid_Pagos.DataBind();
            Grid_Pagos.Columns[1].Visible = false;
            Grid_Pagos.Columns[2].Visible = false;
            Grid_Pagos.Columns[3].Visible = false;
            Grid_Pagos.Columns[4].Visible = false;
            Grid_Pagos.Columns[5].Visible = false;
            Grid_Pagos.Columns[6].Visible = false;
            //Grid_Pagos.Columns[7].Visible = false;


        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Grid: " + ex.Message.ToString(), ex);
        }
    }
    /// *************************************************************************************
    /// NOMBRE:              Grid_Pagos_SelectedIndexChanged
    /// DESCRIPCIÓN:         Evento del grid para seleccionar un folio y mostrar sus detalles
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          11/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    protected void Grid_Pagos_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Historial_Pago_Negocio Historial_Pagos_Detalles = new Cls_Ope_Pre_Historial_Pago_Negocio();
        DataTable Dt_Historial_Pagos_Detalles = new DataTable();
        try
        {
            //Txt_No_Supervision.Text = HttpUtility.HtmlDecode(Grid_Antigrafiti_General.SelectedRow.Cells(1).Text.Trim)
            Historial_Pagos_Detalles.P_Cuenta_Predial_ID = Grid_Pagos.SelectedRow.Cells[2].Text;
            Txt_Documento.Text = Grid_Pagos.SelectedRow.Cells[2].ToString().Trim();
            Dt_Historial_Pagos_Detalles = Historial_Pagos_Detalles.Consultar_Historial_Pagos_Detalles();
            Txt_Documento.Text = Dt_Historial_Pagos_Detalles.Rows[0]["Documento"].ToString().Trim();
            Txt_Periodo_Corriente.Text = Dt_Historial_Pagos_Detalles.Rows[0]["Periodo_Corriente_Inicial"].ToString().Trim() + ", " + Dt_Historial_Pagos_Detalles.Rows[0]["Periodo_Corriente_Final"].ToString().Trim();
            Txt_Monto_Corriente.Text = "$ " + String.Format("{0:#,###,###.00}", Convert.ToDecimal(Dt_Historial_Pagos_Detalles.Rows[0]["Monto_Corriente"].ToString().Trim()));
            Txt_Periodo_Rezago.Text = Dt_Historial_Pagos_Detalles.Rows[0]["Periodo_Rezago_Inicial"].ToString().Trim() + ", " + Dt_Historial_Pagos_Detalles.Rows[0]["Periodo_Rezago_Final"].ToString().Trim();
            Txt_Monto_Rezago.Text = "$ " + String.Format("{0:#,###,###.00}", Convert.ToDecimal(Dt_Historial_Pagos_Detalles.Rows[0]["Monto_Rezago"].ToString().Trim()));
            Txt_Gastos_Ejecucion.Text = "$ " + String.Format("{0:#,###,###.00}", Convert.ToDecimal(Dt_Historial_Pagos_Detalles.Rows[0]["Gastos_Ejecucion"].ToString().Trim()));
            Txt_Recargos_Ordinarios.Text = "$ " + String.Format("{0:#,###,###.00}", Convert.ToDecimal(Dt_Historial_Pagos_Detalles.Rows[0]["Monto_Recargos_Ordinarios"].ToString().Trim()));
            Txt_Recargos_Moratorios.Text = "$ " + String.Format("{0:#,###,###.00}", Convert.ToDecimal(Dt_Historial_Pagos_Detalles.Rows[0]["Monto_Recargos_Moratorios"].ToString().Trim()));
            Txt_Honorarios.Text = "$ " + String.Format("{0:#,###,###.00}", Convert.ToDecimal(Dt_Historial_Pagos_Detalles.Rows[0]["Honorarios"].ToString().Trim()));
            Txt_Multas.Text = "$ " + String.Format("{0:#,###,###.00}", Convert.ToDecimal(Dt_Historial_Pagos_Detalles.Rows[0]["Multas"].ToString().Trim()));
            Txt_Descuento_Recargos.Text = "$ " + String.Format("{0:#,###,###.00}", Convert.ToDecimal(Dt_Historial_Pagos_Detalles.Rows[0]["Descuento_Recargos"].ToString().Trim()));
            Txt_Descuento_Honorarios.Text = "$ " + String.Format("{0:#,###,###.00}", Convert.ToDecimal(Dt_Historial_Pagos_Detalles.Rows[0]["Descuento_Honorarios"].ToString().Trim()));
            Txt_Descuento_Pronto_Pago.Text = "$ " + String.Format("{0:#,###,###.00}", Convert.ToDecimal(Dt_Historial_Pagos_Detalles.Rows[0]["Descuento_Pronto_Pago"].ToString().Trim()));
        }
        catch (Exception ex)
        {
        }

    }
    /// *************************************************************************************
    /// NOMBRE:              Btn_Imprimir_Click
    /// DESCRIPCIÓN:         Botron para mandar a imprimir el reporte en cristal
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          12/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Pre_Historial_Pago_Negocio Historial_Pagos_Detalles = new Cls_Ope_Pre_Historial_Pago_Negocio();
        DataTable Dt_Imprimir_Historial = new DataTable();
        Dt_Imprimir_Historial = (DataTable)Session["Dt_Historial"];
        Ds_Historial_Pagos Historial_Pagos = new Ds_Historial_Pagos();
        Historial_Pagos_Detalles.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado.ToUpper();
        Historial_Pagos_Detalles.P_Dt_Detalles_Cuenta = Dt_Imprimir_Historial;
        Dt_Imprimir_Historial = Historial_Pagos_Detalles.Consultar_Detalles_Cuenta_Predial();
        Generar_Reporte(Dt_Imprimir_Historial, Historial_Pagos);
    }
    /// *************************************************************************************
    /// NOMBRE:              Generar_Reporte
    /// DESCRIPCIÓN:         Genera el reporte.
    /// PARÁMETROS:          Dt_Imprimir_Historial.- Data table de los campos de la consulta.
    ///                      Historial_Pagos.- Data set para  mandar a la imporesion del reporte
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          12/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    protected void Generar_Reporte(DataTable Dt_Imprimir_Historial, DataSet Historial_Pagos)
    {
        DataRow Renglon;
        String Nombre_Reporte = "Rpt_Historial_Pagos" + Convert.ToString(DateTime.Now.ToString("HH'-'mm'-'ss"));
        Cls_Reportes Reportes = new Cls_Reportes();
        try
        {
            Renglon = Dt_Imprimir_Historial.Rows[0];
            Historial_Pagos.Tables[1].ImportRow(Renglon);
            for (int Detalles = 0; Detalles < Dt_Imprimir_Historial.Rows.Count; Detalles++)
            {
                Renglon = Dt_Imprimir_Historial.Rows[Detalles];
                Historial_Pagos.Tables[0].ImportRow(Renglon);

            }
            Reportes.Generar_Reporte(ref Historial_Pagos, "../Rpt/Predial/Rpt_Pre_Historial_Pagos.rpt", Nombre_Reporte, "PDF");
            Mostrar_Reporte(Nombre_Reporte, "PDF");

        }
        catch (Exception ex)
        {
        }

    }
    /// *************************************************************************************
    /// NOMBRE:              Mostrar_Reporte
    /// DESCRIPCIÓN:         Muestra el reporte en pantalla.
    /// PARÁMETROS:          Nombre_Reporte_Generar.- Nombre que tiene el reporte que se mostrará en pantalla.
    ///                      Formato.- Variable que contiene el formato en el que se va a generar el reporte "PDF" O "Excel"
    /// USUARIO CREO:        Juan Alberto Hernández Negrete.
    /// FECHA CREO:          3/Mayo/2011 18:20 p.m.
    /// USUARIO MODIFICO:    Salvador Hernández Ramírez
    /// FECHA MODIFICO:      16-Mayo-2011
    /// CAUSA MODIFICACIÓN:  Se asigno la opción para que en el mismo método se muestre el reporte en excel
    /// *************************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte_Generar, String Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            if (Formato == "PDF")
            {
                Pagina = Pagina + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
                "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
            else if (Formato == "Excel")
            {
                String Ruta = "../../Reporte/" + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    protected void Cmb_Lugar_Pago_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Historial_Pago_Negocio Historial_Pagos = new Cls_Ope_Pre_Historial_Pago_Negocio();
        DataTable Dt_Historial_Pagos = new DataTable();
        Session["Dt_Historial"] = Dt_Historial_Pagos;
        try
        {
            Historial_Pagos.P_Cuenta_Predial_ID = Txt_Cuenta_Predial.Text.Trim();
            Historial_Pagos.P_Entre_Fecha = Txt_Fecha_Inicial.Text.Trim();
            Historial_Pagos.P_Y_Fecha = Txt_Fecha_Final.Text.Trim();
            Historial_Pagos.P_Recibo_Inicial = Txt_Recibo_Inicial.Text.Trim();
            Historial_Pagos.P_Recibo_Final = Txt_Recibo_Final.Text.Trim();
            Historial_Pagos.P_Lugar_Pago = Cmb_Lugar_Pago.SelectedValue.Trim();
            Historial_Pagos.P_Caja = Txt_Caja.Text.Trim();
            Dt_Historial_Pagos = Historial_Pagos.Consultar_Historial_Pagos();
            Session["Dt_Historial"] = Dt_Historial_Pagos;
            Grid_Pagos.DataSource = Dt_Historial_Pagos;
            Grid_Pagos.Columns[2].Visible = true;
            Grid_Pagos.DataBind();
            Grid_Pagos.Columns[1].Visible = false;
            Grid_Pagos.Columns[2].Visible = false;
            Grid_Pagos.Columns[3].Visible = false;
            Grid_Pagos.Columns[4].Visible = false;
            Grid_Pagos.Columns[5].Visible = false;
            Grid_Pagos.Columns[6].Visible = false;
            //Grid_Pagos.Columns[7].Visible = false;


        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Grid: " + ex.Message.ToString(), ex);
        }
    }
    protected void Txt_Fecha_Inicial_TextChanged(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Historial_Pago_Negocio Historial_Pagos = new Cls_Ope_Pre_Historial_Pago_Negocio();
        DataTable Dt_Historial_Pagos = new DataTable();
        Session["Dt_Historial"] = Dt_Historial_Pagos;
        try
        {
            Historial_Pagos.P_Cuenta_Predial_ID = Txt_Cuenta_Predial.Text.Trim();
            Historial_Pagos.P_Entre_Fecha = Txt_Fecha_Inicial.Text.Trim();
            Historial_Pagos.P_Y_Fecha = Txt_Fecha_Final.Text.Trim();
            Historial_Pagos.P_Recibo_Inicial = Txt_Recibo_Inicial.Text.Trim();
            Historial_Pagos.P_Recibo_Final = Txt_Recibo_Final.Text.Trim();
            Historial_Pagos.P_Lugar_Pago = Cmb_Lugar_Pago.SelectedValue.Trim();
            Historial_Pagos.P_Caja = Txt_Caja.Text.Trim();
            if (Historial_Pagos.P_Entre_Fecha != "" && Historial_Pagos.P_Y_Fecha != "")
            {
                Dt_Historial_Pagos = Historial_Pagos.Consultar_Historial_Pagos();
                Session["Dt_Historial"] = Dt_Historial_Pagos;
                Grid_Pagos.DataSource = Dt_Historial_Pagos;
                Grid_Pagos.Columns[2].Visible = true;
                Grid_Pagos.DataBind();
                Grid_Pagos.Columns[1].Visible = false;
                Grid_Pagos.Columns[2].Visible = false;
                Grid_Pagos.Columns[3].Visible = false;
                Grid_Pagos.Columns[4].Visible = false;
                Grid_Pagos.Columns[5].Visible = false;
                Grid_Pagos.Columns[6].Visible = false;
                //Grid_Pagos.Columns[7].Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Grid: " + ex.Message.ToString(), ex);
        }
    }

    protected void Txt_Fecha_Final_TextChanged(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Historial_Pago_Negocio Historial_Pagos = new Cls_Ope_Pre_Historial_Pago_Negocio();
        DataTable Dt_Historial_Pagos = new DataTable();
        Session["Dt_Historial"] = Dt_Historial_Pagos;
        try
        {
            Historial_Pagos.P_Cuenta_Predial_ID = Txt_Cuenta_Predial.Text.Trim();
            Historial_Pagos.P_Entre_Fecha = Txt_Fecha_Inicial.Text.Trim();
            Historial_Pagos.P_Y_Fecha = Txt_Fecha_Final.Text.Trim();
            Historial_Pagos.P_Recibo_Inicial = Txt_Recibo_Inicial.Text.Trim();
            Historial_Pagos.P_Recibo_Final = Txt_Recibo_Final.Text.Trim();
            Historial_Pagos.P_Lugar_Pago = Cmb_Lugar_Pago.SelectedValue.Trim();
            Historial_Pagos.P_Caja = Txt_Caja.Text.Trim();
            if (Historial_Pagos.P_Entre_Fecha != "" && Historial_Pagos.P_Y_Fecha != "")
            {
                Dt_Historial_Pagos = Historial_Pagos.Consultar_Historial_Pagos();
                Session["Dt_Historial"] = Dt_Historial_Pagos;
                Grid_Pagos.DataSource = Dt_Historial_Pagos;
                Grid_Pagos.Columns[2].Visible = true;
                Grid_Pagos.DataBind();
                Grid_Pagos.Columns[1].Visible = false;
                Grid_Pagos.Columns[2].Visible = false;
                Grid_Pagos.Columns[3].Visible = false;
                Grid_Pagos.Columns[4].Visible = false;
                Grid_Pagos.Columns[5].Visible = false;
                Grid_Pagos.Columns[6].Visible = false;
                //Grid_Pagos.Columns[7].Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Grid: " + ex.Message.ToString(), ex);
        }
    }
    protected void Txt_Recibo_Inicial_TextChanged(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Historial_Pago_Negocio Historial_Pagos = new Cls_Ope_Pre_Historial_Pago_Negocio();
        DataTable Dt_Historial_Pagos = new DataTable();
        Session["Dt_Historial"] = Dt_Historial_Pagos;
        try
        {
            Historial_Pagos.P_Cuenta_Predial_ID = Txt_Cuenta_Predial.Text.Trim();
            Historial_Pagos.P_Entre_Fecha = Txt_Fecha_Inicial.Text.Trim();
            Historial_Pagos.P_Y_Fecha = Txt_Fecha_Final.Text.Trim();
            Historial_Pagos.P_Recibo_Inicial = Txt_Recibo_Inicial.Text.Trim();
            Historial_Pagos.P_Recibo_Final = Txt_Recibo_Final.Text.Trim();
            Historial_Pagos.P_Lugar_Pago = Cmb_Lugar_Pago.SelectedValue.Trim();
            Historial_Pagos.P_Caja = Txt_Caja.Text.Trim();
            Dt_Historial_Pagos = Historial_Pagos.Consultar_Historial_Pagos();
            Session["Dt_Historial"] = Dt_Historial_Pagos;
            Grid_Pagos.DataSource = Dt_Historial_Pagos;
            Grid_Pagos.Columns[2].Visible = true;
            Grid_Pagos.DataBind();
            Grid_Pagos.Columns[1].Visible = false;
            Grid_Pagos.Columns[2].Visible = false;
            Grid_Pagos.Columns[3].Visible = false;
            Grid_Pagos.Columns[4].Visible = false;
            Grid_Pagos.Columns[5].Visible = false;
            Grid_Pagos.Columns[6].Visible = false;
            //Grid_Pagos.Columns[7].Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Grid: " + ex.Message.ToString(), ex);
        }
    }
    protected void Txt_Recibo_Final_TextChanged(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Historial_Pago_Negocio Historial_Pagos = new Cls_Ope_Pre_Historial_Pago_Negocio();
        DataTable Dt_Historial_Pagos = new DataTable();
        Session["Dt_Historial"] = Dt_Historial_Pagos;
        try
        {
            Historial_Pagos.P_Cuenta_Predial_ID = Txt_Cuenta_Predial.Text.Trim();
            Historial_Pagos.P_Entre_Fecha = Txt_Fecha_Inicial.Text.Trim();
            Historial_Pagos.P_Y_Fecha = Txt_Fecha_Final.Text.Trim();
            Historial_Pagos.P_Recibo_Inicial = Txt_Recibo_Inicial.Text.Trim();
            Historial_Pagos.P_Recibo_Final = Txt_Recibo_Final.Text.Trim();
            Historial_Pagos.P_Lugar_Pago = Cmb_Lugar_Pago.SelectedValue.Trim();
            Historial_Pagos.P_Caja = Txt_Caja.Text.Trim();
            Dt_Historial_Pagos = Historial_Pagos.Consultar_Historial_Pagos();
            Session["Dt_Historial"] = Dt_Historial_Pagos;
            Grid_Pagos.DataSource = Dt_Historial_Pagos;
            Grid_Pagos.Columns[2].Visible = true;
            Grid_Pagos.DataBind();
            Grid_Pagos.Columns[1].Visible = false;
            Grid_Pagos.Columns[2].Visible = false;
            Grid_Pagos.Columns[3].Visible = false;
            Grid_Pagos.Columns[4].Visible = false;
            Grid_Pagos.Columns[5].Visible = false;
            Grid_Pagos.Columns[6].Visible = false;
            //Grid_Pagos.Columns[7].Visible = false;
        }

        catch (Exception ex)
        {
            throw new Exception("Llenar_Grid: " + ex.Message.ToString(), ex);
        }
    }
}
