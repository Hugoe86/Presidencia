using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Catalogo_Movimientos.Negocio;
using Presidencia.Operacion_Predial_Historial_Movimientos.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Historial_Movimientos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                //Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                //Llenar_Grid();
                //Llenar el combo movimiento
                Llenar_Combo_Movimiento();
                Txt_Cuenta_Predial.Enabled = false;
                string Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no ');";
                Btn_Mostrar_Busqueda_Avanzada.Attributes.Add("onclick", Ventana_Modal);
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
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

    protected void Grid_Historial_Movimientos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Cls_Ope_Pre_Historial_Movimientos_Negocio Historial_Movimiento = new Cls_Ope_Pre_Historial_Movimientos_Negocio();

        DataTable Dt_Historial_Movimientos = new DataTable();
        try
        {

            Grid_Historial_Movimientos.SelectedIndex = (-1);
            Historial_Movimiento.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            Historial_Movimiento.P_Entre_Fecha = Txt_Fecha_Inicial.Text.Trim();
            Historial_Movimiento.P_Y_Fecha = Txt_Fecha_Final.Text.Trim();
            Historial_Movimiento.P_Entre_Movimiento = Txt_Entre_Movimiento.Text.Trim();
            Historial_Movimiento.P_Y_Movimiento = Txt_Y_Movimiento.Text.Trim();
            Dt_Historial_Movimientos = Historial_Movimiento.Consultar_Historial_Movimientos();
            Grid_Historial_Movimientos.PageIndex = e.NewPageIndex;
            Grid_Historial_Movimientos.DataSource = Dt_Historial_Movimientos;
            Grid_Historial_Movimientos.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = "";
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
        DataTable Dt_Historial_Movimientos = new DataTable();
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        Cls_Ope_Pre_Historial_Movimientos_Negocio Historial_Movimiento = new Cls_Ope_Pre_Historial_Movimientos_Negocio();
        if (Txt_Cuenta_Predial.Text.Trim() != "")
        {
            //Consulta la Cuenta Predial
            Cuentas_Predial.P_Campos_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", ";
            Cuentas_Predial.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Campo_Propietario_ID;
            Cuentas_Predial.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            Historial_Movimiento.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            Dt_Historial_Movimientos = Historial_Movimiento.Consultar_Historial_Movimientos();
            Grid_Historial_Movimientos.DataSource = Dt_Historial_Movimientos;
            Grid_Historial_Movimientos.DataBind();

        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Mensaje_Error
    ///DESCRIPCIÓN: Metodo para Mostrar los errores
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:31:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Mensaje_Error()
    {
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Lbl_Encabezado_Error.Text = "";
    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mensaje_Error
    ///DESCRIPCION : Muestra el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 04-Septiembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mensaje_Error(String P_Mensaje)
    {

        Img_Error.Visible = true;
        Lbl_Mensaje_Error.Text += P_Mensaje + "</br>";
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Generales_Cuenta
    ///DESCRIPCIÓN: asignar datos generales de cuenta a los controles y objeto de negocio
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:31:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Llenar_Combo_Movimiento()
    {
        Cls_Cat_Pre_Movimientos_Negocio Movimiento = new Cls_Cat_Pre_Movimientos_Negocio();
        DataTable Dt_Movimientos = new DataTable();
        try
        {
            Dt_Movimientos = Movimiento.Consultar_Movimientos();
            DataRow fila = Dt_Movimientos.NewRow();
            fila[Cat_Pre_Movimientos.Campo_Descripcion] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            fila[Cat_Pre_Movimientos.Campo_Movimiento_ID] = "SELECCIONE";
            Dt_Movimientos.Rows.InsertAt(fila, 0);
            Cmb_Tipo_Movimiento.DataTextField = Cat_Pre_Movimientos.Campo_Descripcion;
            Cmb_Tipo_Movimiento.DataValueField = Cat_Pre_Movimientos.Campo_Movimiento_ID;
            Cmb_Tipo_Movimiento.DataSource = Dt_Movimientos;
            Cmb_Tipo_Movimiento.DataBind();
        }
        catch (Exception Ex)
        {
        }
    }

    protected void Btn_Detalles_Cuenta_Predial_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Pre_Historial_Movimientos_Negocio Historial_Movimiento = new Cls_Ope_Pre_Historial_Movimientos_Negocio();
        DataTable Dt_Historial_Movimientos = new DataTable();
        try
        {
            Historial_Movimiento.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            Historial_Movimiento.P_Entre_Fecha = Txt_Fecha_Inicial.Text.Trim();
            Historial_Movimiento.P_Y_Fecha = Txt_Fecha_Final.Text.Trim();
            Historial_Movimiento.P_Entre_Movimiento = Txt_Entre_Movimiento.Text.Trim();
            Historial_Movimiento.P_Y_Movimiento = Txt_Y_Movimiento.Text.Trim();
            Dt_Historial_Movimientos = Historial_Movimiento.Consultar_Historial_Movimientos();
            Grid_Historial_Movimientos.DataSource = Dt_Historial_Movimientos;
            Grid_Historial_Movimientos.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Txt_Fecha_Inicial_TextChanged(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Historial_Movimientos_Negocio Historial_Movimiento = new Cls_Ope_Pre_Historial_Movimientos_Negocio();
        DataTable Dt_Historial_Movimientos = new DataTable();
        try
        {
            Historial_Movimiento.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            Historial_Movimiento.P_Entre_Fecha = Txt_Fecha_Inicial.Text.Trim();
            Historial_Movimiento.P_Y_Fecha = Txt_Fecha_Final.Text.Trim();
            Historial_Movimiento.P_Entre_Movimiento = Txt_Entre_Movimiento.Text.Trim();
            Historial_Movimiento.P_Y_Movimiento = Txt_Y_Movimiento.Text.Trim();
            if (Historial_Movimiento.P_Entre_Fecha != "" && Historial_Movimiento.P_Y_Fecha != "")
            {
                Dt_Historial_Movimientos = Historial_Movimiento.Consultar_Historial_Movimientos();
                Grid_Historial_Movimientos.DataSource = Dt_Historial_Movimientos;
                Grid_Historial_Movimientos.DataBind();
            }

        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Txt_Fecha_Final_TextChanged(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Historial_Movimientos_Negocio Historial_Movimiento = new Cls_Ope_Pre_Historial_Movimientos_Negocio();
        DataTable Dt_Historial_Movimientos = new DataTable();
        try
        {
            Historial_Movimiento.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            Historial_Movimiento.P_Entre_Fecha = Txt_Fecha_Inicial.Text.Trim();
            Historial_Movimiento.P_Y_Fecha = Txt_Fecha_Final.Text.Trim();
            Historial_Movimiento.P_Entre_Movimiento = Txt_Entre_Movimiento.Text.Trim();
            Historial_Movimiento.P_Y_Movimiento = Txt_Y_Movimiento.Text.Trim();
            if (Historial_Movimiento.P_Entre_Fecha != "" && Historial_Movimiento.P_Y_Fecha != "")
            {
                Dt_Historial_Movimientos = Historial_Movimiento.Consultar_Historial_Movimientos();
                Grid_Historial_Movimientos.DataSource = Dt_Historial_Movimientos;
                Grid_Historial_Movimientos.DataBind();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Txt_Entre_Movimiento_TextChanged(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Historial_Movimientos_Negocio Historial_Movimiento = new Cls_Ope_Pre_Historial_Movimientos_Negocio();
        DataTable Dt_Historial_Movimientos = new DataTable();
        try
        {
            Historial_Movimiento.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            Historial_Movimiento.P_Entre_Fecha = Txt_Fecha_Inicial.Text.Trim();
            Historial_Movimiento.P_Y_Fecha = Txt_Fecha_Final.Text.Trim();
            Historial_Movimiento.P_Entre_Movimiento = Txt_Entre_Movimiento.Text.Trim();
            Historial_Movimiento.P_Y_Movimiento = Txt_Y_Movimiento.Text.Trim();
            if (Historial_Movimiento.P_Entre_Movimiento != "" && Historial_Movimiento.P_Y_Movimiento != "")
            {
                Dt_Historial_Movimientos = Historial_Movimiento.Consultar_Historial_Movimientos();
                Grid_Historial_Movimientos.DataSource = Dt_Historial_Movimientos;
                Grid_Historial_Movimientos.DataBind();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Txt_Y_Movimiento_TextChanged(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Historial_Movimientos_Negocio Historial_Movimiento = new Cls_Ope_Pre_Historial_Movimientos_Negocio();
        DataTable Dt_Historial_Movimientos = new DataTable();
        try
        {
            Historial_Movimiento.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            Historial_Movimiento.P_Entre_Fecha = Txt_Fecha_Inicial.Text.Trim();
            Historial_Movimiento.P_Y_Fecha = Txt_Fecha_Final.Text.Trim();
            Historial_Movimiento.P_Entre_Movimiento = Txt_Entre_Movimiento.Text.Trim();
            Historial_Movimiento.P_Y_Movimiento = Txt_Y_Movimiento.Text.Trim();
            if (Historial_Movimiento.P_Entre_Movimiento != "" && Historial_Movimiento.P_Y_Movimiento != "")
            {
                Dt_Historial_Movimientos = Historial_Movimiento.Consultar_Historial_Movimientos();
                Grid_Historial_Movimientos.DataSource = Dt_Historial_Movimientos;
                Grid_Historial_Movimientos.DataBind();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Cmb_Tipo_Movimiento_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Historial_Movimientos_Negocio Historial_Movimiento = new Cls_Ope_Pre_Historial_Movimientos_Negocio();
        DataTable Dt_Historial_Movimientos = new DataTable();
        try
        {
            Historial_Movimiento.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            Historial_Movimiento.P_Entre_Fecha = Txt_Fecha_Inicial.Text.Trim();
            Historial_Movimiento.P_Y_Fecha = Txt_Fecha_Final.Text.Trim();
            Historial_Movimiento.P_Entre_Movimiento = Txt_Entre_Movimiento.Text.Trim();
            Historial_Movimiento.P_Y_Movimiento = Txt_Y_Movimiento.Text.Trim();
            Historial_Movimiento.P_Tipo = Cmb_Tipo_Movimiento.SelectedValue;
            Dt_Historial_Movimientos = Historial_Movimiento.Consultar_Historial_Movimientos();
            Grid_Historial_Movimientos.DataSource = Dt_Historial_Movimientos;
            Grid_Historial_Movimientos.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
}
