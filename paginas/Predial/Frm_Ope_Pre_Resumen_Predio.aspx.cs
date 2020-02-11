using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Catalogo_Cuotas_Minimas.Negocio;
using Presidencia.Reportes;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Operacion_Predial_Recepcion_Pagos.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Presidencia.Operacion_Predial_Convenios_Predial.Negocio;
using Presidencia.Predial_Pae_Honorarios.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Resumen_Predio : System.Web.UI.Page
{
    #region Variables
    private const int Const_Estado_Inicial = 0;
    private const int Const_Estado_Nuevo = 2;
    private const int Const_Estado_Modificar = 3;

    private static String M_Cuenta_ID;
    private static Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
    private static DataTable Dt_Agregar_Co_Propietarios = new DataTable();
    private static DataTable Dt_Agregar_Diferencias = new DataTable();
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Cls_Ope_Pre_Resumen_Predio_Negocio Cuentas = new Cls_Ope_Pre_Resumen_Predio_Negocio();
                Cuentas.Consultar_Estatus_Cuentas();
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Resumen de Predio", "alert('Cuenta No Generada')", true);

                Session["ESTATUS_CUENTAS"] = "IN ('BLOQUEADA','BAJA','VIGENTE','CANCELADA','SUSPENDIDA')";
                Session["TIPO_CONTRIBUYENTE"] = "  IN('PROPIETARIO','POSEEDOR')";

                Div_Detalles_Cuenta.Visible = false;
                string Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Mostrar_Busqueda_Avanzada.Attributes.Add("onclick", Ventana_Modal);
                Habilitar_Todo();
                String Ventana_Modal_Movimientos = "Abrir_Ventana_Modal('Ventanas_Emergentes/Resumen_Predial/Frm_Validacion_Orden_Variacion.aspx";

                String Propiedades = ", 'center:yes;resizable:yes;status:no;dialogWidth:680px;dialogHeight:800px;dialogHide:true;help:no;scroll:yes');";
                Grid_Historial_Movimientos.Attributes.Add("onclick", Ventana_Modal_Movimientos + Propiedades);

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
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Convenio_Detalles_Pre_Resumen_Predial
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente de los Detalles del Convenio con la ruta y parámtros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Otubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Emergente_Convenio_Detalles_Pre_Resumen_Predial()
    {
        string Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Resumen_Predial/Frm_Convenio_Detalles_Pre_Resumen_Predial.aspx?No_Convenio=" + Grid_Convenios_Cuenta.SelectedRow.Cells[1].Text.Trim() + "', 'center:yes;resizable:no;status:no ;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no ');";
        Grid_Convenios_Cuenta.Attributes.Add("onclick", Ventana_Modal);
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Estado_Cuenta
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente del Estado de Cuenta con la ruta y parámtros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Otubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Emergente_Estado_Cuenta()
    {
        if (Txt_Estatus_General.Text.Trim() != "CANCELADA")
        {
            string Ventana_Modal_Estado_Cuenta = "Abrir_Ventana_Estado_Cuenta('Ventanas_Emergentes/Resumen_Predial/Frm_Estado_Cuenta.aspx?Cuenta_Predial=" + Txt_Cuenta_Predial.Text.Trim() + "', 'height=600,width=800,scrollbars=1');";
            Btn_Estado_Cuenta.Attributes.Add("onclick", Ventana_Modal_Estado_Cuenta);
            Btn_Estado_Cuenta.Enabled = true;
        }
        else
        {
            Btn_Estado_Cuenta.Attributes.Remove("onclick");
            Btn_Estado_Cuenta.Enabled = false;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Validacion_Orden_Variacion
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente del Resumen de Predial con la ruta y parámtros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Otubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Emergente_Validacion_Orden_Variacion()
    {
        //String Ventana_Modal_Movimientos = "Abrir_Ventana_Modal('Ventanas_Emergentes/Resumen_Predial/Frm_Validacion_Orden_Variacion.aspx";
        String Parametros = "?Anio_Orden_Variacion=" + DateTime.ParseExact(Grid_Historial_Movimientos.SelectedRow.Cells[3].Text, "dd/MM/yy", null).Year;
        Parametros += "&Fecha_Orden_Variacion=" + DateTime.ParseExact(Grid_Historial_Movimientos.SelectedRow.Cells[3].Text, "dd/MM/yy", null).AddDays(1).ToString("dd-MM-yyyy");
        Parametros += "&No_Orden_Variacion=" + Convert.ToInt64(Grid_Historial_Movimientos.SelectedRow.Cells[4].Text).ToString("0000000000");
        //Parametros += "&No_Contrarecibo=" + Grid_Historial_Movimientos.SelectedRow.Cells[3].Text;
        Parametros += "&Cuenta_Predial=" + Txt_Cuenta_Predial.Text;
        Parametros += "&Cuenta_Predial_ID=" + Hdf_Cuenta_Predial_ID.Value;
        Parametros += "'";
        String Propiedades = ", 'height=600,width=800,scrollbars=1');";
        //Grid_Historial_Movimientos.Attributes.Add("onclick", Ventana_Modal_Movimientos + Parametros + Propiedades);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Validacion Orden Variacion", "window.open('Ventanas_Emergentes/Resumen_Predial/Frm_Validacion_Orden_Variacion.aspx" + Parametros + ", null " + Propiedades, true);
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Validacion_Orden_Variacion_Pendiente
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente de la Orden de Varición con la ruta y parámtros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 23/Marzo/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Emergente_Validacion_Orden_Variacion_Pendiente()
    {
        String Parametros = "";
        Parametros = "?Anio_Orden_Variacion=" + Grid_Movimientos_Pendientes.SelectedRow.Cells[2].Text.Substring(Grid_Movimientos_Pendientes.SelectedRow.Cells[2].Text.Length - 4);
        Parametros += "&Fecha_Orden_Variacion=" + DateTime.ParseExact(Grid_Movimientos_Pendientes.SelectedRow.Cells[2].Text, "dd/MMM/yyyy", null).AddDays(1).ToString("dd-MM-yyyy");
        Parametros += "&No_Orden_Variacion=" + Convert.ToInt64(Grid_Movimientos_Pendientes.SelectedRow.Cells[1].Text).ToString("0000000000");
        //Parametros += "&No_Contrarecibo=" + Grid_Historial_Movimientos.SelectedRow.Cells[3].Text;
        Parametros += "&Cuenta_Predial=" + Txt_Cuenta_Predial.Text;
        Parametros += "&Cuenta_Predial_ID=" + Hdf_Cuenta_Predial_ID.Value;
        Parametros += "'";
        String Propiedades = ", 'center:yes;resizable:yes;status:no;dialogWidth:680px;dialogHeight:800px;dialogHide:true;help:no;scroll:yes');";
        //Grid_Historial_Movimientos.Attributes.Add("onclick", Ventana_Modal_Movimientos + Parametros + Propiedades);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Validacion Orden Variacion", "window.showModalDialog('Ventanas_Emergentes/Resumen_Predial/Frm_Validacion_Orden_Variacion.aspx" + Parametros + ", null " + Propiedades, true);
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Validacion_Orden_Variacion
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente del Resumen de Predial con la ruta y parámtros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Otubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Detalles_Pago_Predial()
    {
        //String Ventana_Modal_Movimientos = "Abrir_Ventana_Modal('Ventanas_Emergentes/Resumen_Predial/Frm_Validacion_Orden_Variacion.aspx";
        //String Parametros = "?Anio_Orden_Variacion=" + Convert.ToDateTime(Grid_Historial_Movimientos.SelectedRow.Cells[3].Text).Year;
        //Parametros += "&No_Orden_Variacion=" + Convert.ToInt64(Grid_Historial_Movimientos.SelectedRow.Cells[4].Text).ToString("0000000000");
        ////Parametros += "&No_Contrarecibo=" + Grid_Historial_Movimientos.SelectedRow.Cells[3].Text;
        //Parametros += "&Cuenta_Predial=" + Txt_Cuenta_Predial.Text;
        //Parametros += "&Cuenta_Predial_ID=" + Hdf_Cuenta_Predial_ID.Value;
        String Parametros = "'";
        String Propiedades = ", 'center:yes;resizable:yes;status:no;dialogWidth:680px;dialogHeight:800px;dialogHide:true;help:no;scroll:yes');";
        //Grid_Historial_Movimientos.Attributes.Add("onclick", Ventana_Modal_Movimientos + Parametros + Propiedades);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Validacion Orden Variacion", "window.showModalDialog('Ventanas_Emergentes/Resumen_Predial/Frm_Historial_Pagos_Detalles.aspx" + Parametros + ", null " + Propiedades, true);
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Validacion_Orden_Variacion
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente del Resumen de Predial con la ruta y parámtros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Otubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Detalles_Pago_Traslado()
    {
        //String Ventana_Modal_Movimientos = "Abrir_Ventana_Modal('Ventanas_Emergentes/Resumen_Predial/Frm_Validacion_Orden_Variacion.aspx";
        String Parametros = "?Anio_Orden_Variacion=" + Convert.ToDateTime(Grid_Historial_Movimientos.SelectedRow.Cells[3].Text).Year;
        Parametros += "&No_Orden_Variacion=" + Convert.ToInt64(Grid_Historial_Movimientos.SelectedRow.Cells[4].Text).ToString("0000000000");
        //Parametros += "&No_Contrarecibo=" + Grid_Historial_Movimientos.SelectedRow.Cells[3].Text;
        Parametros += "&Cuenta_Predial=" + Txt_Cuenta_Predial.Text;
        Parametros += "&Cuenta_Predial_ID=" + Hdf_Cuenta_Predial_ID.Value;
        Parametros += "'";
        String Propiedades = ", 'center:yes;resizable:yes;status:no;dialogWidth:680px;dialogHeight:800px;dialogHide:true;help:no;scroll:yes');";
        //Grid_Historial_Movimientos.Attributes.Add("onclick", Ventana_Modal_Movimientos + Parametros + Propiedades);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Validacion Orden Variacion", "window.showModalDialog('Ventanas_Emergentes/Resumen_Predial/Frm_Historial_Pagos_Detalles.aspx" + Parametros + ", null " + Propiedades, true);
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Validacion_Orden_Variacion
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente del Resumen de Predial con la ruta y parámtros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Otubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Detalles_Pago_Fraccionamiento()
    {
        //String Ventana_Modal_Movimientos = "Abrir_Ventana_Modal('Ventanas_Emergentes/Resumen_Predial/Frm_Validacion_Orden_Variacion.aspx";
        String Parametros = "?Anio_Orden_Variacion=" + Convert.ToDateTime(Grid_Historial_Movimientos.SelectedRow.Cells[3].Text).Year;
        Parametros += "&No_Orden_Variacion=" + Convert.ToInt64(Grid_Historial_Movimientos.SelectedRow.Cells[4].Text).ToString("0000000000");
        //Parametros += "&No_Contrarecibo=" + Grid_Historial_Movimientos.SelectedRow.Cells[3].Text;
        Parametros += "&Cuenta_Predial=" + Txt_Cuenta_Predial.Text;
        Parametros += "&Cuenta_Predial_ID=" + Hdf_Cuenta_Predial_ID.Value;
        Parametros += "'";
        String Propiedades = ", 'center:yes;resizable:yes;status:no;dialogWidth:680px;dialogHeight:800px;dialogHide:true;help:no;scroll:yes');";
        //Grid_Historial_Movimientos.Attributes.Add("onclick", Ventana_Modal_Movimientos + Parametros + Propiedades);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Validacion Orden Variacion", "window.showModalDialog('Ventanas_Emergentes/Resumen_Predial/Frm_Historial_Pagos_Detalles.aspx" + Parametros + ", null " + Propiedades, true);
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Validacion_Orden_Variacion
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente del Resumen de Predial con la ruta y parámtros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Otubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Detalles_Pago_Supervision()
    {
        //String Ventana_Modal_Movimientos = "Abrir_Ventana_Modal('Ventanas_Emergentes/Resumen_Predial/Frm_Validacion_Orden_Variacion.aspx";
        String Parametros = "?Anio_Orden_Variacion=" + Convert.ToDateTime(Grid_Historial_Movimientos.SelectedRow.Cells[3].Text).Year;
        Parametros += "&No_Orden_Variacion=" + Convert.ToInt64(Grid_Historial_Movimientos.SelectedRow.Cells[4].Text).ToString("0000000000");
        //Parametros += "&No_Contrarecibo=" + Grid_Historial_Movimientos.SelectedRow.Cells[3].Text;
        Parametros += "&Cuenta_Predial=" + Txt_Cuenta_Predial.Text;
        Parametros += "&Cuenta_Predial_ID=" + Hdf_Cuenta_Predial_ID.Value;
        Parametros += "'";
        String Propiedades = ", 'center:yes;resizable:yes;status:no;dialogWidth:680px;dialogHeight:800px;dialogHide:true;help:no;scroll:yes');";
        //Grid_Historial_Movimientos.Attributes.Add("onclick", Ventana_Modal_Movimientos + Parametros + Propiedades);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Validacion Orden Variacion", "window.showModalDialog('Ventanas_Emergentes/Resumen_Predial/Frm_Historial_Pagos_Detalles.aspx" + Parametros + ", null " + Propiedades, true);
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Validacion_Orden_Variacion
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente del Resumen de Predial con la ruta y parámtros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Otubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Detalles_Pago_Constancias()
    {
        //String Ventana_Modal_Movimientos = "Abrir_Ventana_Modal('Ventanas_Emergentes/Resumen_Predial/Frm_Validacion_Orden_Variacion.aspx";
        String Parametros = "?Anio_Orden_Variacion=" + Convert.ToDateTime(Grid_Historial_Movimientos.SelectedRow.Cells[3].Text).Year;
        Parametros += "&No_Orden_Variacion=" + Convert.ToInt64(Grid_Historial_Movimientos.SelectedRow.Cells[4].Text).ToString("0000000000");
        //Parametros += "&No_Contrarecibo=" + Grid_Historial_Movimientos.SelectedRow.Cells[3].Text;
        Parametros += "&Cuenta_Predial=" + Txt_Cuenta_Predial.Text;
        Parametros += "&Cuenta_Predial_ID=" + Hdf_Cuenta_Predial_ID.Value;
        Parametros += "'";
        String Propiedades = ", 'center:yes;resizable:yes;status:no;dialogWidth:680px;dialogHeight:800px;dialogHide:true;help:no;scroll:yes');";
        //Grid_Historial_Movimientos.Attributes.Add("onclick", Ventana_Modal_Movimientos + Parametros + Propiedades);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Validacion Orden Variacion", "window.showModalDialog('Ventanas_Emergentes/Resumen_Predial/Frm_Historial_Pagos_Detalles.aspx" + Parametros + ", null " + Propiedades, true);
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
        Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
        Busqueda_Ubicaciones = Convert.ToBoolean(Session["BUSQUEDA_CUENTAS_PREDIAL"]);

        if (Busqueda_Ubicaciones)
        {
            if (Session["CUENTA_PREDIAL_ID"] != null)
            {

                Limpiar_Todo();
                Cuenta_Predial_ID = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
                Session["Cuenta_Predial_ID"] = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
                Hdf_Cuenta_Predial_ID.Value = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
                Session["Cuenta_Predial_ID_Convenios"] = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
                Cuenta_Predial = Convert.ToString(Session["CUENTA_PREDIAL"]);
                Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial_ID = Cuenta_Predial;
                Txt_Cuenta_Predial.Text = Cuenta_Predial;
                Habilitar_Controles();

            }
            Consultar_Datos_Cuenta_Constancia();
            Cargar_Ventana_Emergente_Estado_Cuenta();
        }
        Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
        Session.Remove("CUENTA_PREDIAL_ID");
        Session.Remove("CUENTA_PREDIAL");
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Adeudos
    ///DESCRIPCIÓN: Llena el grid de los Adeudos dependiendo de los seleccionados.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 23 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Llenar_Grid_Adeudos(String P_Cuenta_Predial, Boolean Tomar_Filtrado)
    {
        Int32 Anio_Corriente = 0;
        var Consulta_Parametros = new Cls_Ope_Pre_Parametros_Negocio();
        var Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        var RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
        var Dt_Estado_Cuenta = new DataTable();

        decimal Adeudo_Corriente = 0;
        decimal Adeudo_Rezago = 0;
        decimal Recargos_Ordinarios = 0;
        decimal Recargos_Moratorios = 0;
        decimal Honorarios = 0;
        String Periodo_Actual_Inicial = "";
        String Periodo_Actual_Final = "";
        String Periodo_Rezago_Inicial = "";
        String Periodo_Rezago_Final = "";
        Int16 Cont_Bimestres;

        Grid_Estado_Cuenta.Columns[0].Visible = true;

        // obtener año corriente
        Anio_Corriente = Consulta_Parametros.Consultar_Anio_Corriente();
        // verificar que se obtuvo valor mayor que cero, si no, tomar año actual
        if (Anio_Corriente <= 0)
        {
            Anio_Corriente = DateTime.Now.Year;
        }

        Txt_Adeudo_Rezago.Text = "";
        Txt_Adeudo_Actual.Text = "";
        Dt_Estado_Cuenta = Resumen_Predio.Consultar_Adeudos_Cuenta_Predial_Con_Totales(P_Cuenta_Predial, null, 0, 0);

        Txt_Descuento_Recargos_Ordinarios.Text = "$ 0.00";
        Txt_Descuento_Recargos_Moratorios.Text = "$ 0.00";

        for (int x = 0; x < Dt_Estado_Cuenta.Rows.Count; x++)
        {
            decimal Monto_Adeudo;
            int Anio_Adeudo;

            // recorrer los adeudos para extraer los periodos
            for (int Contador_Bimestre = 1; Contador_Bimestre <= 6; Contador_Bimestre++)
            {
                decimal.TryParse(Dt_Estado_Cuenta.Rows[x]["Adeudo_Bimestre_" + Contador_Bimestre].ToString().Trim(), out Monto_Adeudo);
                int.TryParse(Dt_Estado_Cuenta.Rows[x]["Anio"].ToString(), out Anio_Adeudo);
                // si hay adeudo
                if (Monto_Adeudo >= 0)
                {
                    // año rezago, tomar dato para periodo rezago y periodo inicial
                    if (Anio_Adeudo < Anio_Corriente)
                    {
                        if (Periodo_Rezago_Inicial.Length <= 0 && Monto_Adeudo > 0)
                        {
                            Periodo_Rezago_Inicial = Contador_Bimestre + "/" + Anio_Adeudo;
                        }
                        Periodo_Rezago_Final = Contador_Bimestre + "/" + Anio_Adeudo;
                    }
                    // año corriente, tomar dato para periodo corriente
                    if (Anio_Adeudo == Anio_Corriente)
                    {
                        if (Periodo_Actual_Inicial.Length <= 0 && Monto_Adeudo > 0)
                        {
                            Periodo_Actual_Inicial = Contador_Bimestre + "/" + Anio_Adeudo;
                        }
                        Periodo_Actual_Final = Contador_Bimestre + "/" + Anio_Adeudo;
                    }
                }
            }
        }

        // cargar periodo inicial del periodo rezago o periodo corriente
        if (Periodo_Rezago_Inicial.Length > 0)
        {
            Txt_Periodo_Inicial.Text = Periodo_Rezago_Inicial;
        }
        else if (Periodo_Actual_Inicial.Length > 0)
        {
            Txt_Periodo_Inicial.Text = Periodo_Actual_Inicial;
        }
        // cargar cajas de texto de periodos rezago y corriente
        if (Periodo_Rezago_Inicial.Length > 0)
        {
            Txt_Periodo_Rezago.Text = Periodo_Rezago_Inicial + " - " + Periodo_Rezago_Final;
        }
        if (Periodo_Actual_Inicial.Length > 0)
        {
            Txt_Periodo_Actual.Text = Periodo_Actual_Inicial + " - " + Periodo_Actual_Final;
        }
        // cargar periodo final del periodo corriente o periodo rezago
        if (Periodo_Actual_Final.Length > 0)
        {
            Txt_Periodo_Final.Text = Periodo_Actual_Final;
        }
        else if (Txt_Periodo_Final.Text.Length <= 0 && Periodo_Rezago_Final.Length > 0)
        {
            Txt_Periodo_Final.Text = Periodo_Rezago_Final;
        }

        decimal Descuento_Pronto_Pago = 0;
        decimal Porcentaje_Descuento;
        decimal Subtotal = 0;
        decimal Total_Estado_Cuenta = 0;
        decimal Descuento_Recargos_Ordinarios = 0;
        decimal Descuento_Recargos_Moratorios = 0;
        decimal Cuota_Minima = 0;
        var GAP_Negocio = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        var Cuotas_Minima = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
        var Consultar_Pagos = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        Cls_Ope_Pre_Pae_Honorarios_Negocio PAE_Honorarios = new Cls_Ope_Pre_Pae_Honorarios_Negocio();

        DataTable Dt_Cuotas_Minimas;
        DataTable Dt_Pagos;
        GAP_Negocio.Calcular_Recargos_Predial(P_Cuenta_Predial);

        // obtener cuota minima
        Cuotas_Minima.P_Anio = DateTime.Now.Year.ToString();
        Dt_Cuotas_Minimas = Cuotas_Minima.Consultar_Cuotas_Minimas();
        if (Dt_Cuotas_Minimas != null && Dt_Cuotas_Minimas.Rows.Count > 0)
        {
            Decimal.TryParse(Dt_Cuotas_Minimas.Rows[0]["CUOTA"].ToString(), out Cuota_Minima);
        }

        if (GAP_Negocio.p_Total_Corriente == 0 && GAP_Negocio.p_Total_Rezago == 0)
        {
            if (Grid_Estado_Cuenta.Rows.Count > 0)
            {
                foreach (GridViewRow Dr_Estado_Cuenta in Grid_Estado_Cuenta.Rows)
                {
                    if (Dr_Estado_Cuenta.Cells[0].Text == DateTime.Now.Year.ToString())
                    {
                        for (Cont_Bimestres = 1; Cont_Bimestres <= 6; Cont_Bimestres++)
                        {
                            if (Dr_Estado_Cuenta.Cells[Cont_Bimestres].Text.Trim() != "")
                            {
                                GAP_Negocio.p_Total_Corriente += Convert.ToDecimal(Dr_Estado_Cuenta.Cells[Cont_Bimestres].Text.Replace("$", ""));
                            }
                        }
                    }
                    else
                    {
                        for (Cont_Bimestres = 1; Cont_Bimestres <= 6; Cont_Bimestres++)
                        {
                            if (Dr_Estado_Cuenta.Cells[Cont_Bimestres].Text.Trim() != "")
                            {
                                GAP_Negocio.p_Total_Rezago += Convert.ToDecimal(Dr_Estado_Cuenta.Cells[Cont_Bimestres].Text.Replace("$", ""));
                            }
                        }
                    }
                }
            }
        }

        // tomar datos directamente de la consulta a calcular recargos en generar adeudos predial
        Adeudo_Corriente = GAP_Negocio.p_Total_Corriente;
        Adeudo_Rezago = GAP_Negocio.p_Total_Rezago;
        Recargos_Ordinarios = GAP_Negocio.p_Total_Recargos_Generados;
        Txt_Adeudo_Actual.Text = String.Format("{0:c}", Adeudo_Corriente);
        Txt_Adeudo_Rezago.Text = String.Format("{0:c}", Adeudo_Rezago);
        Txt_Total_Recargos_Ordinarios.Text = String.Format("{0:c}", Recargos_Ordinarios);

        DataTable Dt_PAE_Honorarios = null;
        PAE_Honorarios.P_Cuenta_Predial_ID = P_Cuenta_Predial;
        Dt_PAE_Honorarios = PAE_Honorarios.Consultar_Total_Honorarios();
        if (Dt_PAE_Honorarios != null)
        {
            if (Dt_PAE_Honorarios.Rows.Count > 0)
            {
                if (Dt_PAE_Honorarios.Rows[0]["TOTAL_HONORARIOS"] != null)
                {
                    if (Dt_PAE_Honorarios.Rows[0]["TOTAL_HONORARIOS"].ToString().Trim() != "")
                    {
                        Honorarios = Convert.ToDecimal(Dt_PAE_Honorarios.Rows[0]["TOTAL_HONORARIOS"]);
                    }
                }
            }
        }
        Txt_Honorarios.Text = "$ " + Honorarios.ToString("#,##0.00");
        Txt_Gastos_Ejecucion.Text = "$ 0.00";

        // consultar y mostrar recargos moratorios
        var Consulta_Moratorios = new Cls_Ope_Pre_Convenios_Predial_Negocio();
        Consulta_Moratorios.P_Cuenta_Predial_ID = P_Cuenta_Predial;
        Recargos_Moratorios = Consulta_Moratorios.Obtener_Recargos_Moratorios();
        Txt_Recargos_Moratorios.Text = Recargos_Moratorios.ToString("#,##0.00");

        // calcular subtotal
        Subtotal = Adeudo_Rezago + Adeudo_Corriente + Recargos_Ordinarios + Recargos_Moratorios + Honorarios;
        Txt_Subtotal.Text = "$ " + Subtotal.ToString("#,##0.00");

        // consultar pagos de la cuenta
        Consultar_Pagos.P_Cuenta_Predial_ID = P_Cuenta_Predial;
        Consultar_Pagos.p_Estatus = "PAGADO";
        Consultar_Pagos.p_Periodo_Corriente = " LIKE '%" + Anio_Corriente + "%'";
        Dt_Pagos = Consultar_Pagos.Consultar_Pagos_Predial_Por_Periodo();
        // solo si el adeudo consultado es hasta el sexto bimestre del año corriente, calcular descuento por pronto pago, tampoco debe tener beneficio
        if (Txt_Periodo_Actual.Text.Contains("6/" + Anio_Corriente) && (Dt_Pagos == null || Dt_Pagos.Rows.Count == 0) && Chk_Cuota_Fija.Checked == false)
        {
            DataTable Dt_Descuento_Pronto_Pago = Resumen_Predio.Consultar_Descuentos_Pronto_Pago();
            if (Dt_Descuento_Pronto_Pago != null && Dt_Descuento_Pronto_Pago.Rows.Count > 0)
            {
                decimal.TryParse(
                    Dt_Descuento_Pronto_Pago.Rows[0][String.Format("{0:MMMM}", DateTime.Now)].ToString().Trim(),
                    out Porcentaje_Descuento);
                Descuento_Pronto_Pago = Adeudo_Corriente * Porcentaje_Descuento / 100;
                // validar que el descuento restado al adeudo corriente no sea menor que la cuota minima
                if (Cuota_Minima > 0 && Adeudo_Corriente - Descuento_Pronto_Pago < Cuota_Minima)
                {
                    Descuento_Pronto_Pago = Adeudo_Corriente - Cuota_Minima;
                }
                // validar descuentos negativos
                if (Descuento_Pronto_Pago < 0)
                {
                    Descuento_Pronto_Pago = 0;
                }
                // mostrar resultado
                Txt_Descuento_Pronto_Pago.Text = "$ " + Descuento_Pronto_Pago.ToString("#,##0.00");
            }
        }
        else
        {
            Txt_Descuento_Pronto_Pago.Text = "$ 0.00";
        }

        // consultar descuento a recargos
        Resumen_Predio.P_Cuenta_Predial_ID = P_Cuenta_Predial;
        DataTable Dt_Descuentos_Recargos = Resumen_Predio.Consultar_Descuentos_Recargos();
        if (Dt_Descuentos_Recargos != null && Dt_Descuentos_Recargos.Rows.Count > 0)
        {
            decimal.TryParse(Dt_Descuentos_Recargos.Rows[0]["Desc_Recargo"].ToString().Trim(), out Descuento_Recargos_Ordinarios);
            decimal.TryParse(Dt_Descuentos_Recargos.Rows[0]["Desc_Recargo_Moratorio"].ToString(), out Descuento_Recargos_Moratorios);

        }
        Txt_Descuento_Recargos_Ordinarios.Text = Descuento_Recargos_Ordinarios.ToString("$ #,###,##0.00");
        Txt_Descuento_Recargos_Moratorios.Text = Descuento_Recargos_Moratorios.ToString("$ #,###,##0.00");

        // adeudo total (aplicar los descuentos solo a los recargos correspondientes)
        Total_Estado_Cuenta = Subtotal - Descuento_Pronto_Pago
            - (Recargos_Ordinarios - Descuento_Recargos_Ordinarios < 0 ? Recargos_Ordinarios : Descuento_Recargos_Ordinarios)
            - (Recargos_Moratorios - Descuento_Recargos_Moratorios < 0 ? Recargos_Moratorios : Descuento_Recargos_Moratorios);
        Txt_Total.Text = Convert.ToDecimal(Total_Estado_Cuenta.ToString()).ToString("$ #,###,###,##0.00");
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
    ///**************************************************************************** ***
    protected void Consultar_Datos_Cuenta_Constancia()
    {
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
        try
        {
            if (Txt_Cuenta_Predial.Text.Trim() != "")
            {
                Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
                //Consulta la Cuenta Predial
                Cuentas_Predial.P_Campos_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", ";
                Cuentas_Predial.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Campo_Propietario_ID;
                Cuentas_Predial.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
                M_Cuenta_ID = Txt_Cuenta_Predial.Text.Trim();
                Session.Remove("Ds_Cuenta_Datos");
                M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
                Limpiar_Todo();
                Cargar_Datos();

                String Cuenta_Predial_ID = (Session["Cuenta_Predial_ID"] != null) ? Session["Cuenta_Predial_ID"].ToString().Trim() : "";
                if (Cuenta_Predial_ID != "")
                {
                    Llenar_Grid_Convenios(0);
                    Llenar_Grid_Historial_Pagos(0);      //Manda a llamar el metodo para llenar el; grid de historial de pagos
                    Llenar_Grid_Historial_Movimientos(0);
                    RP_Negocio.P_Cuenta_Predial = Cuenta_Predial_ID;

                    Llenar_Grid_Adeudos(Cuenta_Predial_ID, false);
                    DataTable Dt_Adeudos = RP_Negocio.Consultar_Adeudos_Cuentas_Predial();
                    //Llenar_Combo_Anios(Dt_Adeudos); 
                    Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
                    Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial_ID = Txt_Cuenta_Predial.Text.Trim();
                    DataTable Dt_Contribuyentes = Rs_Consulta_Ope_Resumen_Predio.Consultar_Contribuyentes();
                    DataSet Ds_Impuestos = Rs_Consulta_Ope_Resumen_Predio.Consulta_Datos_Cuenta_Impuestos();
                    if (Ds_Impuestos.Tables[0].Rows.Count - 1 > 0)
                    {
                        if (!string.IsNullOrEmpty(Ds_Impuestos.Tables[0].Rows[0]["Cuenta_Origen"].ToString().Trim()))
                        {
                            Txt_Cuenta_Origen.Text = Ds_Impuestos.Tables[0].Rows[0]["Cuenta_Origen"].ToString().Trim();
                        }
                        if (!string.IsNullOrEmpty(Ds_Impuestos.Tables[0].Rows[0]["Valor_Fiscal"].ToString().Trim()))
                        {
                            Txt_Valor_Fiscal_Impuestos.Text = Convert.ToDecimal(Ds_Impuestos.Tables[0].Rows[0]["Valor_Fiscal"].ToString().Trim()).ToString("$ #,###,###,##0.00");
                        }
                        if (!string.IsNullOrEmpty(Ds_Impuestos.Tables[0].Rows[0]["Periodo_Corriente"].ToString().Trim()))
                        {
                            Txt_Periodo_Corriente_Impuestos.Text = Ds_Impuestos.Tables[0].Rows[0]["Periodo_Corriente"].ToString().Trim();
                        }
                        if (!string.IsNullOrEmpty(Ds_Impuestos.Tables[0].Rows[0]["Cuota_Anual"].ToString().Trim()))
                        {
                            Txt_Cuota_Anual_Impuestos.Text = Convert.ToDecimal(Ds_Impuestos.Tables[0].Rows[0]["Cuota_Anual"].ToString().Trim()).ToString("$ #,###,###,##0.00");
                        }
                        if (!string.IsNullOrEmpty(Ds_Impuestos.Tables[0].Rows[0]["Cuota_Bimestral"].ToString().Trim()))
                        {
                            Txt_Cuota_Bimestral_Impuestos.Text = Convert.ToDecimal(Ds_Impuestos.Tables[0].Rows[0]["Cuota_Bimestral"].ToString().Trim()).ToString("$ #,###,###,##0.00");
                        }
                        if (!string.IsNullOrEmpty(Ds_Impuestos.Tables[0].Rows[0]["Diferencia_Construccion"].ToString().Trim()))
                        {
                            Txt_Dif_Construccion_Impuestos.Text = Ds_Impuestos.Tables[0].Rows[0]["Diferencia_Construccion"].ToString().Trim();
                        }
                        if (!string.IsNullOrEmpty(Ds_Impuestos.Tables[0].Rows[0]["Porcentaje_Excencion"].ToString().Trim()))
                        {
                            Txt_Porciento_Exencion_Impuestos.Text = Ds_Impuestos.Tables[0].Rows[0]["Porcentaje_Excencion"].ToString().Trim();
                        }
                        if (!string.IsNullOrEmpty(Ds_Impuestos.Tables[0].Rows[0]["Termino_Excencion"].ToString().Trim()))
                        {
                            if (Ds_Impuestos.Tables[0].Rows[0]["Termino_Excencion"].ToString().Trim() == "01/01/0001 12:00:00 a.m.")
                            {
                                Txt_Fecha_Termino_Extencion.Text = "";
                            }
                            else
                            {
                                Txt_Fecha_Termino_Extencion.Text = String.Format("{0:dd/MMM/yyyy}", Ds_Impuestos.Tables[0].Rows[0]["Termino_Excencion"].ToString().Trim());
                            }
                        }
                        if (!string.IsNullOrEmpty(Ds_Impuestos.Tables[0].Rows[0]["Fecha_Avaluo"].ToString().Trim()))
                        {
                            DateTime Fecha_Avaluo;
                            DateTime.TryParse(Ds_Impuestos.Tables[0].Rows[0]["Fecha_Avaluo"].ToString(), out Fecha_Avaluo);
                            if (Fecha_Avaluo <= DateTime.MinValue)
                            {
                                Txt_Fecha_Avaluo_Impuestos.Text = "";
                            }
                            else
                            {
                                Txt_Fecha_Avaluo_Impuestos.Text = Fecha_Avaluo.ToString("dd-MMM-YYYY");
                            }
                        }
                        if (!string.IsNullOrEmpty(Ds_Impuestos.Tables[0].Rows[0]["Cuota_Fija"].ToString().Trim()))
                        {
                            Txt_Cuota_Fija_Impuestos.Text = Ds_Impuestos.Tables[0].Rows[0]["Cuota_Fija"].ToString().Trim();
                        }

                        Rs_Consulta_Ope_Resumen_Predio.P_No_Cuota_Fija = Ds_Impuestos.Tables[0].Rows[0]["Cuota_Fija_ID"].ToString().Trim();
                        DataTable Dt_Cuota_Detalles = Rs_Consulta_Ope_Resumen_Predio.Consultar_Cuota_Fija_Detalles();
                        //Validar que la tabla traigo un registro
                        if (Dt_Cuota_Detalles.Rows.Count > 0)
                        {

                            //if (!string.IsNullOrEmpty(Dt_Cuota_Detalles.Rows[0]["Excedente_Contruccion"].ToString().Trim()))
                            //{
                            //    Txt_Excedente_Construccion_Multiplicador.Text = Dt_Cuota_Detalles.Rows[0]["Excedente_Contruccion"].ToString().Trim();
                            //}
                            //if (!string.IsNullOrEmpty(Dt_Cuota_Detalles.Rows[0]["Excedente_Valor"].ToString().Trim()))
                            //{
                            //    Txt_Excedente_Valor_Multiplicador.Text = Dt_Cuota_Detalles.Rows[0]["Excedente_Valor"].ToString().Trim();
                            //}

                            //if (!string.IsNullOrEmpty(Dt_Cuota_Detalles.Rows[0]["Cuota_Minima"].ToString().Trim()))
                            //{
                            //    Txt_Cuota_Minima.Text = Dt_Cuota_Detalles.Rows[0]["Cuota_Minima"].ToString().Trim();
                            //}

                            //if (!string.IsNullOrEmpty(Dt_Cuota_Detalles.Rows[0]["Tasa_Valor"].ToString().Trim()))
                            //{
                            //    Txt_Excedente_Construccion_Multiplicando.Text = Dt_Cuota_Detalles.Rows[0]["Tasa_Valor"].ToString().Trim();
                            //}
                            //if (!string.IsNullOrEmpty(Dt_Cuota_Detalles.Rows[0]["Tasa_Valor"].ToString().Trim()))
                            //{
                            //    Txt_Excedente_Valor_Multiplicando.Text = Dt_Cuota_Detalles.Rows[0]["Tasa_Valor"].ToString().Trim();
                            //}
                            if (!string.IsNullOrEmpty(Dt_Cuota_Detalles.Rows[0]["Tipo"].ToString().Trim()))
                            {
                                Txt_Cuota_Fija.Text = Dt_Cuota_Detalles.Rows[0]["Tipo"].ToString().Trim();
                                //Asignar si es por financiamiento el total 
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {

        }
    }
    protected void Bloquear_Controles()
    {
        Img_Btn_Imprimir_Cuenta_Predial.Enabled = false;
        Img_Btn_Documento_Cuenta_Predial.Enabled = false;
        Img_Btn_Imprimir_Generales.Enabled = false;
        Img_Btn_Imprimir_Propietario.Enabled = false;
        Img_Btn_Imprimir_Impuestos.Enabled = false;
        Img_Btn_Imprimir_Histotial_Pagos.Enabled = false;
        Img_Btn_Imprimir_Historial_Movimientos.Enabled = false;
        Img_Btn_Imprimir_Historial_Convenios.Enabled = false;
        Btn_Estado_Cuenta.Enabled = false;
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
    private void Cargar_Generales_Cuenta(DataTable dataTable)
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
        try
        {
            Txt_Cuenta_Predial.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString();
            Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial_ID = dataTable.Rows[0]["Cuenta_Predial_ID"].ToString().Trim();
            DataTable Dt_Ultimo_Movimiento = Rs_Consulta_Ope_Resumen_Predio.Consultar_Ultimo_Movimiento();
            if (Dt_Ultimo_Movimiento.Rows.Count > 0)
            {
                if (Dt_Ultimo_Movimiento.Rows[0]["Identificador"].ToString() != string.Empty)
                {
                    if (Dt_Ultimo_Movimiento.Rows[0]["descripcion"].ToString() != "APERTURA")
                    {
                        Txt_Ultimo_Movimiento_General.Text = Dt_Ultimo_Movimiento.Rows[0]["Identificador"].ToString().Trim();
                    }
                }
            }
            DataTable Dt_Consultar_Beneficio = Rs_Consulta_Ope_Resumen_Predio.Consultar_Beneficio();
            if (Dt_Consultar_Beneficio.Rows.Count > 0)
            {
                if (Dt_Consultar_Beneficio.Rows[0].ToString() != String.Empty)
                {
                    if (Dt_Consultar_Beneficio.Rows[0].ToString() != "NO")
                    {
                        Lbl_Estatus.Text = " Beneficio Retirado por opción Global" + " " + Lbl_Estatus.Text;
                    }
                }
            }
            if (dataTable.Rows[0]["Estatus"].ToString().Trim() == "BLOQUEADA")
            {
                Lbl_Estatus.Text = " BLOQUEADA" + " " + Lbl_Estatus.Text;
            }
            if (dataTable.Rows[0]["Estatus"].ToString().Trim() == "BAJA")
            {
                Lbl_Estatus.Text = " BAJA" + " " + Lbl_Estatus.Text;
            }
            if (dataTable.Rows[0]["Estatus"].ToString().Trim() == "CANCELADA")
            {
                Lbl_Estatus.Text = " CANCELADA" + " " + Lbl_Estatus.Text;
            }
            if (dataTable.Rows[0]["Estatus"].ToString().Trim() == "SUSPENDIDA")
            {
                Lbl_Estatus.Text = "SUSPENDIDA" + " " + Lbl_Estatus.Text;
            }
            if (dataTable.Rows[0]["Estatus"].ToString().Trim() == "PENDIENTE")
            {
                Lbl_Estatus.Text = " Cuenta No Generada";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Resumen de Predio", "alert('Cuenta No Generada')", true);

                Bloquear_Controles();
            }
            Txt_Cuenta_Origen.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen].ToString();
            M_Orden_Negocio.P_Cuenta_Origen = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen].ToString();
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Tipo_Predio = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString();
                DataTable Dt_Tipo_Predio = Rs_Consulta_Ope_Resumen_Predio.Consultar_Tipo_Predio();
                Txt_Tipo_Periodo_Impuestos.Text = Dt_Tipo_Predio.Rows[0]["Descripcion"].ToString().Trim();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Uso_Suelo_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString();
                DataTable Dt_Uso_Suelo = Rs_Consulta_Ope_Resumen_Predio.Consultar_Uso_Predio();
                Txt_Uso_Predio_General.Text = Dt_Uso_Suelo.Rows[0]["Descripcion"].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Estado_Predio = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString();
                DataTable Dt_Estado_Predio = Rs_Consulta_Ope_Resumen_Predio.Consultar_Estado_Predio();
                Txt_Estado_Predio_General.Text = Dt_Estado_Predio.Rows[0]["Descripcion"].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Calle_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
                DataTable Dt_Calles = Rs_Consulta_Ope_Resumen_Predio.Consultar_Calle_Generales();
                Txt_Ubicacion_General.Text = Dt_Calles.Rows[0]["Nombre"].ToString();
                Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = Dt_Calles.Rows[0]["Colonia_ID"].ToString();
                DataTable Dt_Colonia = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
                Txt_Colonia_General.Text = Dt_Colonia.Rows[0]["Nombre"].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString() != string.Empty)
            {
                Txt_Estatus_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString();
                M_Orden_Negocio.P_Estatus_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString();
            }

            Txt_Supe_Construida_General.Text = dataTable.Rows[0]["Superficie_Construida"].ToString();
            M_Orden_Negocio.P_Superficie_Construida = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida].ToString();
            Txt_Super_Total_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total].ToString();
            M_Orden_Negocio.P_Superficie_Total = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total].ToString();
            if (dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString() != "")
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString();
                DataTable Dt_Colonia = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
                Txt_Calle_Propietario.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
                M_Orden_Negocio.P_Ubicacion_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
            }
            Txt_Numero_Exterior_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
            M_Orden_Negocio.P_Exterior_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
            Txt_Numero_Interior_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
            M_Orden_Negocio.P_Interior_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
            Txt_Clave_Catastral_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString();
            M_Orden_Negocio.P_Clave_Catastral = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString();
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString() != string.Empty)
            {
                Txt_Efectos_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString();
            }
            Txt_Valor_Fiscal_Impuestos.Text = Convert.ToDecimal(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal].ToString()).ToString("$ #,###,###,##0.00");
            M_Orden_Negocio.P_Valor_Fiscal = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal].ToString();
            Txt_Periodo_Corriente_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            M_Orden_Negocio.P_Periodo_Corriente_Inicial = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            Txt_Periodo_Corriente_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            M_Orden_Negocio.P_Periodo_Corriente_Inicial = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()))
            {
                Txt_Cuota_Anual_Impuestos.Text = Convert.ToDecimal(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()).ToString("$ #,###,###,##0.00");
                Decimal Cuota_Bimestral = Convert.ToDecimal(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()) / 6;
                Txt_Cuota_Bimestral_Impuestos.Text = Convert.ToDecimal(Cuota_Bimestral).ToString("$ #,###,###,##0.00");
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual] != null)
            {
                M_Orden_Negocio.P_Cuota_Anual = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString());
                M_Orden_Negocio.P_Cuota_Bimestral = ((Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString())) / 6);
            }
            Txt_Porciento_Exencion_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString();
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion] != null)
            {
                M_Orden_Negocio.P_Exencion = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString());
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo] != null)
            {
                DateTime Fecha_Avaluo;
                DateTime.TryParse(dataTable.Rows[0]["Fecha_Avaluo"].ToString(), out Fecha_Avaluo);
                if (Fecha_Avaluo <= DateTime.MinValue)
                {
                    Txt_Fecha_Avaluo_Impuestos.Text = "";
                    M_Orden_Negocio.P_Fecha_Avaluo = Fecha_Avaluo;
                }
                else
                {
                    Txt_Fecha_Avaluo_Impuestos.Text = Fecha_Avaluo.ToString("dd-MMM-yyyy");
                    M_Orden_Negocio.P_Fecha_Avaluo = Fecha_Avaluo;
                }
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion] != null)
            {
                if (dataTable.Rows[0]["Termino_Exencion"].ToString().Trim() == "01/01/0001 12:00:00 a.m.")
                {
                    Txt_Fecha_Termino_Extencion.Text = "";
                    M_Orden_Negocio.P_Fecha_Termina_Exencion = Convert.ToDateTime(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion].ToString());
                }
                else
                {
                    Txt_Fecha_Termino_Extencion.Text = String.Format("{0:dd/MMM/yyyy}", dataTable.Rows[0]["Termino_Exencion"].ToString().Trim());
                    M_Orden_Negocio.P_Fecha_Termina_Exencion = Convert.ToDateTime(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion].ToString());
                }
            }
            Txt_Dif_Construccion_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion].ToString();
            M_Orden_Negocio.P_Diferencia_Construccion = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion].ToString();

            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID] != null)
            {
                M_Orden_Negocio.P_Cuota_Minima = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString());
                //Cmb_Cuota_Minima.SelectedValue = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString();
            }
            //Z1 HAY KU07A FIJ4!!! Seccion de carga de datos de la cuota fija
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija] != null)
            {
                if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "NO" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "no" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "No")
                {
                    Chk_Cuota_Fija.Checked = false;
                    M_Orden_Negocio.P_Cuota_Fija = "NO";
                }
                if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "SI" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "si" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "Si")
                {
                    Chk_Cuota_Fija.Checked = true;
                    M_Orden_Negocio.P_Cuota_Fija = "SI";

                    //----K4RG4R D47OZ D3 14 CU0T4 F1J4!!!!!
                    if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString() != "")
                    {
                        M_Orden_Negocio.P_No_Cuota_Fija = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString().Trim().PadLeft(10, '0');
                        Cargar_Datos_Cuota_Fija(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString());
                    }
                }
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString()))
            {
                M_Orden_Negocio.P_Tasa = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString());
                Hdn_Tasa_ID.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString();
            }

            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_ID].ToString()))
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Tasa_Predial_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_ID].ToString();
                DataTable Dt_Tasa = Rs_Consulta_Ope_Resumen_Predio.Consultar_Tasa();
                if (Dt_Tasa.Rows.Count > 0)
                {
                    Txt_Tasa_Impuestos.Text = Dt_Tasa.Rows[0]["Descripcion"].ToString();
                }
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString() != String.Empty)
            {
                //Cmb_Domicilio_Foraneo.SelectedValue = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString();
                //M_Orden_Negocio.P_Domicilio_Foraneo = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString();
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0]["Estado_ID_Notificacion"].ToString()))
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Estado_Predio = (dataTable.Rows[0]["Estado_ID_Notificacion"].ToString());
                DataTable Dt_Estado_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Estado_Predio_Propietario();
                if (Dt_Estado_Propietario.Rows.Count > 0)
                {
                    Txt_Estado_Propietario.Text = (Dt_Estado_Propietario.Rows[0]["Descripcion"].ToString());
                    M_Orden_Negocio.P_Estado_Propietario = (Dt_Estado_Propietario.Rows[0]["Descripcion"].ToString());
                }
            }
            else if (!String.IsNullOrEmpty(dataTable.Rows[0]["Estado_Notificacion"].ToString()))
            {
                Txt_Estado_Propietario.Text = dataTable.Rows[0]["Estado_Notificacion"].ToString();
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0]["Ciudad_ID_Notificacion"].ToString()))
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Ciudad_ID = dataTable.Rows[0]["Ciudad_ID_Notificacion"].ToString();
                DataTable Dt_Ciudad_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Ciudad();
                Txt_Ciudad_Propietario.Text = Dt_Ciudad_Propietario.Rows[0]["Nombre"].ToString();
                M_Orden_Negocio.P_Ciudad_Propietario = dataTable.Rows[0]["Ciudad_ID_Notificacion"].ToString();
            }
            else if (!String.IsNullOrEmpty(dataTable.Rows[0]["Ciudad_Notificacion"].ToString()))
            {
                Txt_Ciudad_Propietario.Text = dataTable.Rows[0]["Ciudad_Notificacion"].ToString();
            }
            if (dataTable.Rows[0]["Domicilio_Foraneo"].ToString().Trim() == "SI")
            {
                if (!String.IsNullOrEmpty(dataTable.Rows[0]["Colonia_Notificacion"].ToString()))
                {
                    Txt_Colonia_Propietario.Text = dataTable.Rows[0]["Colonia_Notificacion"].ToString();
                }
                if (!String.IsNullOrEmpty(dataTable.Rows[0]["Calle_Notificacion"].ToString()))
                {
                    Txt_Calle_Propietario.Text = dataTable.Rows[0]["Calle_Notificacion"].ToString();
                }
            }
            else
            {

                if (!String.IsNullOrEmpty(dataTable.Rows[0]["Colonia_ID_Notificacion"].ToString()))
                {
                    Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = dataTable.Rows[0]["Colonia_ID_Notificacion"].ToString();
                    DataTable DT_Colonia_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
                    Txt_Colonia_Propietario.Text = DT_Colonia_Propietario.Rows[0]["Nombre"].ToString();
                }

                if (!String.IsNullOrEmpty(dataTable.Rows[0]["Calle_ID_Notificacion"].ToString()))
                {
                    Rs_Consulta_Ope_Resumen_Predio.P_Calle_ID = dataTable.Rows[0]["Calle_ID_Notificacion"].ToString();//*
                    DataTable Dt_Calle_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Calle_Generales();
                    Txt_Calle_Propietario.Text = Dt_Calle_Propietario.Rows[0]["Nombre"].ToString();
                }
            }

            Txt_Numero_Exterior_Propietario.Text = dataTable.Rows[0]["No_Exterior_Notificacion"].ToString();
            M_Orden_Negocio.P_Exterior_Propietario = dataTable.Rows[0]["No_Exterior_Notificacion"].ToString();
            Txt_Numero_Interior_Propietario.Text = dataTable.Rows[0]["No_Interior_Notificacion"].ToString();
            M_Orden_Negocio.P_Interior_Propietario = dataTable.Rows[0]["No_Interior_Notificacion"].ToString();
            Txt_Cod_Postal_Propietario.Text = dataTable.Rows[0]["Codigo_Postal"].ToString();
            M_Orden_Negocio.P_CP_Propietario = dataTable.Rows[0]["Codigo_Postal"].ToString();


            M_Orden_Negocio.P_Cuenta_Predial_ID = dataTable.Rows[0]["Cuenta_Predial_ID"].ToString().Trim();
            M_Orden_Negocio.P_Dt_Copropietarios = M_Orden_Negocio.Consulta_Co_Propietarios();
            Dt_Agregar_Co_Propietarios = M_Orden_Negocio.P_Dt_Copropietarios;
            if (Dt_Agregar_Co_Propietarios.Rows.Count - 1 >= 0)
            {
                for (int x = 0; x <= Dt_Agregar_Co_Propietarios.Rows.Count - 1; x++)
                {
                    if (Dt_Agregar_Co_Propietarios.Rows[x]["Tipo"].ToString().Trim() == "COPROPIETARIO")
                    {
                        Txt_Copropietarios_Propietario.Text += Dt_Agregar_Co_Propietarios.Rows[x]["Nombre_Contribuyente"].ToString().Trim() + " \t" + Dt_Agregar_Co_Propietarios.Rows[x]["Rfc"].ToString().Trim() + "\n";
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Cargar_Datos_Cuenta: " + Ex.Message);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Cuota_Fija
    ///DESCRIPCIÓN: asignar datos cuota fijao
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 05/Ago/2011 06:31:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Cargar_Datos_Cuota_Fija(String Cuota_Fija_ID)
    {
        DataTable Dt_Cuota_Detalles;
        try
        {
            M_Orden_Negocio.P_No_Cuota_Fija = Cuota_Fija_ID.Trim();
            Dt_Cuota_Detalles = M_Orden_Negocio.Consultar_Cuota_Fija_Detalles();
            if (Dt_Cuota_Detalles.Rows.Count - 1 >= 0)
            {
                Txt_Cuota_Fija.Text = Dt_Cuota_Detalles.Rows[0]["Descripcion"].ToString();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error("Cargar_Datos_Cuota_Fija: " + Ex.Message);
        }
    }

    private void Busqueda_Cuentas()
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataSet Ds_Cuenta;
        try
        {
            Resumen_Predio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            Ds_Cuenta = Resumen_Predio.Consulta_Datos_Cuenta_Generales();
            if (Ds_Cuenta.Tables[0].Rows.Count - 1 >= 0)
            {
                if (Ds_Cuenta.Tables[0].Rows[0]["Cuenta_Predial_Id"].ToString().Trim() != string.Empty)
                {
                    Session["Cuenta_Predial_ID"] = Ds_Cuenta.Tables[0].Rows[0]["Cuenta_Predial_Id"].ToString().Trim();
                }
            }
            if (Ds_Cuenta.Tables[0].Rows.Count > 0)
            {
                Session.Remove("Ds_Cuenta_Datos");
                M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
                Session["Ds_Cuenta_Datos"] = Ds_Cuenta;
            }
            else
            {
                Mensaje_Error("No se encontraron los datos necesarios para la consulta de la cuenta");
                Lbl_Mensaje_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos
    ///DESCRIPCIÓN: asignar datos de cuenta a los controles
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:31:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Cargar_Datos()
    {
        try
        {
            if (!String.IsNullOrEmpty(Txt_Cuenta_Predial.Text.Trim()))
            {
                //KONSULTA DATOS CUENTA HACER DS
                Busqueda_Cuentas();
                //LLENAR CAJAS
                if (Session["Ds_Cuenta_Datos"] != null)
                {
                    Cargar_Generales_Cuenta(((DataSet)Session["Ds_Cuenta_Datos"]).Tables["Dt_Generales"]);
                    Busqueda_Propietario();
                }

            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    private void Busqueda_Propietario()
    {
        DataSet Ds_Prop;
        String Cuenta_Predial_ID = Session["Cuenta_Predial_ID"].ToString().Trim();
        try
        {
            M_Orden_Negocio.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
            Ds_Prop = M_Orden_Negocio.Consulta_Datos_Propietario();
            if (Ds_Prop.Tables[0].Rows.Count > 0)
            {
                Session.Remove("Ds_Prop_Datos");
                Session["Ds_Prop_Datos"] = Ds_Prop;
                Cargar_Datos_Propietario(((DataSet)Session["Ds_Prop_Datos"]).Tables["Dt_Propietarios"]);
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Propietario
    ///DESCRIPCIÓN: asignar datos de propietario de la cuenta a los controles
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:44:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Datos_Propietario(DataTable dataTable)
    {
        try
        {
            if (dataTable.Rows.Count > 0 && dataTable != null)
            {
                Hdn_Propietario_ID.Value = dataTable.Rows[0]["PROPIETARIO"].ToString();
                M_Orden_Negocio.P_Propietario_ID = dataTable.Rows[0]["PROPIETARIO"].ToString(); ;

                Txt_Nombre_Propietario.Text = dataTable.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
                M_Orden_Negocio.P_Nombre_Propietario = dataTable.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
                if (dataTable.Rows[0]["TIPO_PROPIETARIO"].ToString() != "")
                {
                    Txt_Propietario_Poseedor_Propietario.Text = dataTable.Rows[0]["TIPO_PROPIETARIO"].ToString();
                    M_Orden_Negocio.P_Tipo_Propietario = dataTable.Rows[0]["TIPO_PROPIETARIO"].ToString();
                }

                Txt_Rfc_Propietario.Text = dataTable.Rows[0]["RFC"].ToString();
                M_Orden_Negocio.P_RFC_Propietario = dataTable.Rows[0]["RFC"].ToString();
            }

        }
        catch (Exception Ex)
        {
            Mensaje_Error("Cargar_Datos_Propietario: " + Ex.Message);
        }
    }
    private void Mensaje_Error()
    {
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Lbl_Encabezado_Error.Text = "";
    }
    private void Mensaje_Error(String P_Mensaje)
    {

        Img_Error.Visible = true;
        Lbl_Mensaje_Error.Text += P_Mensaje + "</br>";
    }
    protected void Habilitar_Controles()
    {
        Img_Btn_Imprimir_Cuenta_Predial.Enabled = true;
        Img_Btn_Documento_Cuenta_Predial.Enabled = true;
        Img_Btn_Imprimir_Generales.Enabled = true;
        Img_Btn_Imprimir_Propietario.Enabled = true;
        Img_Btn_Imprimir_Impuestos.Enabled = true;
        Img_Btn_Imprimir_Histotial_Pagos.Enabled = true;
        Img_Btn_Imprimir_Historial_Movimientos.Enabled = true;
        Img_Btn_Imprimir_Historial_Convenios.Enabled = true;
        Btn_Estado_Cuenta.Enabled = true;
    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Limpiar_Todo
    ///DESCRIPCION : Limpia los controles del formulario
    ///PARAMETROS  : 
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 05-Agsoto-2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Habilitar_Todo()
    {
        Txt_Cuenta_Predial.Enabled = false;
        Txt_Uso_Predio_General.Enabled = false;
        Txt_Estado_Predio_General.Enabled = false;
        Txt_Estatus_General.Enabled = false;
        Txt_Supe_Construida_General.Enabled = false;
        Txt_Super_Total_General.Enabled = false;
        Txt_Ubicacion_General.Enabled = false;
        Txt_Colonia_General.Enabled = false;
        Txt_Numero_Exterior_General.Enabled = false;
        Txt_Numero_Interior_General.Enabled = false;
        Txt_Clave_Catastral_General.Enabled = false;
        Txt_Efectos_General.Enabled = false;
        Txt_Ultimo_Movimiento_General.Enabled = false;


        Txt_Nombre_Propietario.Enabled = false;
        Txt_Propietario_Poseedor_Propietario.Enabled = false;
        Txt_Rfc_Propietario.Enabled = false;
        Txt_Calle_Propietario.Enabled = false;
        Txt_Colonia_Propietario.Enabled = false;
        Txt_Ciudad_Propietario.Enabled = false;
        Txt_Numero_Exterior_Propietario.Enabled = false;
        Txt_Numero_Interior_Propietario.Enabled = false;
        Txt_Estado_Propietario.Enabled = false;
        Txt_Ciudad_Propietario.Enabled = false;
        Txt_Cod_Postal_Propietario.Enabled = false;
        Txt_Copropietarios_Propietario.ReadOnly = true;

        Txt_Valor_Fiscal_Impuestos.Enabled = false;
        Txt_Tasa_Impuestos.Enabled = false;
        Txt_Periodo_Corriente_Impuestos.Enabled = false;
        Txt_Tipo_Periodo_Impuestos.Enabled = false;
        Txt_Cuota_Anual_Impuestos.Enabled = false;
        Txt_Cuota_Bimestral_Impuestos.Enabled = false;
        Txt_Dif_Construccion_Impuestos.Enabled = false;
        Txt_Porciento_Exencion_Impuestos.Enabled = false;
        Txt_Fecha_Termino_Extencion.Enabled = false;
        Txt_Fecha_Avaluo_Impuestos.Enabled = false;
        Txt_Cuota_Fija_Impuestos.Enabled = false;
        Chk_Cuota_Fija.Enabled = false;

        Txt_Cuota_Fija.Enabled = false;

        Txt_Periodo_Rezago.Enabled = false;
        Txt_Adeudo_Rezago.Enabled = false;
        Txt_Periodo_Actual.Enabled = false;
        Txt_Adeudo_Actual.Enabled = false;
        Txt_Total_Recargos_Ordinarios.Enabled = false;
        Txt_Honorarios.Enabled = false;
        Txt_Recargos_Moratorios.Enabled = false;
        Txt_Gastos_Ejecucion.Enabled = false;
        Txt_Subtotal.Enabled = false;
        Txt_Descuento_Pronto_Pago.Enabled = false;
        Txt_Descuento_Recargos_Ordinarios.Enabled = false;
        Txt_Descuento_Recargos_Moratorios.Enabled = false;
        Txt_Total.Enabled = false;

        Txt_Periodo_Inicial.Enabled = false;
        Txt_Periodo_Final.Enabled = false;
    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Limpiar_Todo
    ///DESCRIPCION : Limpia los controles del formulario
    ///PARAMETROS  : 
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 05-Agsoto-2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Limpiar_Todo()
    {
        Div_Detalles_Cuenta.Visible = false;
        Grid_Movimientos_Pendientes.DataBind();
        Cmb_Consultar_Tipo_Pago.SelectedIndex = -1;
        Txt_Cuenta_Origen.Text = "";
        Grid_Convenios_Cuenta.DataBind();
        Grid_Historial_Movimientos.DataBind();
        Grid_Traslado.DataBind();
        Grid_Impuesto_Fraccionamiento.DataBind();
        Grid_Historial_Pagos.DataBind();
        Grid_Derechos_Supervision.DataBind();
        Grid_Constancias.DataBind();
        Txt_Uso_Predio_General.Text = "";
        Txt_Estado_Predio_General.Text = "";
        Txt_Estatus_General.Text = "";
        Txt_Supe_Construida_General.Text = "";
        Txt_Super_Total_General.Text = "";
        Txt_Ubicacion_General.Text = "";
        Txt_Colonia_General.Text = "";
        Txt_Numero_Exterior_General.Text = "";
        Txt_Numero_Interior_General.Text = "";
        Txt_Clave_Catastral_General.Text = "";
        Txt_Efectos_General.Text = "";
        Txt_Ultimo_Movimiento_General.Text = "";
        Lbl_Estatus.Text = "";

        Txt_Nombre_Propietario.Text = "";
        Txt_Propietario_Poseedor_Propietario.Text = "";
        Txt_Rfc_Propietario.Text = "";
        Txt_Calle_Propietario.Text = "";
        Txt_Colonia_Propietario.Text = "";
        Txt_Ciudad_Propietario.Text = "";
        Txt_Numero_Exterior_Propietario.Text = "";
        Txt_Numero_Interior_Propietario.Text = "";
        Txt_Estado_Propietario.Text = "";
        Txt_Ciudad_Propietario.Text = "";
        Txt_Cod_Postal_Propietario.Text = "";
        Txt_Copropietarios_Propietario.Text = "";

        Txt_Valor_Fiscal_Impuestos.Text = "";
        Txt_Tasa_Impuestos.Text = "";
        Txt_Periodo_Corriente_Impuestos.Text = "";
        Txt_Tipo_Periodo_Impuestos.Text = "";
        Txt_Cuota_Anual_Impuestos.Text = "";
        Txt_Cuota_Bimestral_Impuestos.Text = "";
        Txt_Dif_Construccion_Impuestos.Text = "";
        Txt_Porciento_Exencion_Impuestos.Text = "";
        Txt_Fecha_Termino_Extencion.Text = "";
        Txt_Fecha_Avaluo_Impuestos.Text = "";
        Txt_Cuota_Fija_Impuestos.Text = "";
        Chk_Cuota_Fija.Checked = false;

        Txt_Cuota_Fija.Text = "";



        Txt_Periodo_Rezago.Text = "";
        Txt_Adeudo_Rezago.Text = "";
        Txt_Periodo_Actual.Text = "";
        Txt_Adeudo_Actual.Text = "";
        Txt_Total_Recargos_Ordinarios.Text = "";
        Txt_Honorarios.Text = "";
        Txt_Recargos_Moratorios.Text = "";
        Txt_Gastos_Ejecucion.Text = "";
        Txt_Subtotal.Text = "";
        Txt_Descuento_Pronto_Pago.Text = "";
        Txt_Descuento_Recargos_Ordinarios.Text = "";
        Txt_Descuento_Recargos_Moratorios.Text = "";
        Txt_Total.Text = "";

        Txt_Periodo_Inicial.Text = "";
        Txt_Periodo_Final.Text = "";
    }
    protected void Img_Btn_Documento_Cuenta_Predial_Click(object sender, ImageClickEventArgs e)
    {
        Cargar_Grid_Movimientos_Pendientes(0);
    }

    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION     : Cargar_Grid_Movimientos_Pendientes
    ///DESCRIPCION              : Carga el grid con los datos Consultados
    ///PARAMETROS: 
    ///CREO                     : Antonio Salvador Benavides Guardado
    ///FECHA_CREO               : 21/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Cargar_Grid_Movimientos_Pendientes(int Indice_Pagina)
    {
        Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        DataTable Dt_Historial_Movimientos;

        try
        {
            if (Hdf_Cuenta_Predial_ID.Value.Trim() != "")
            {
                Ordenes_Variacion.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                Ordenes_Variacion.P_Generar_Orden_Estatus = "ACEPTADA";
                Ordenes_Variacion.P_Contrarecibo_Estatus = "NOT IN ('PAGADO')";
                Dt_Historial_Movimientos = Ordenes_Variacion.Consultar_Historial_Estatus_Ordenes_Estatus_Contrarecibo();
                //Dt_Historial_Movimientos = Ordenes_Variacion.Consultar_Historial_Estatus_Ordenes_Variacion_Cuenta();
                if (Dt_Historial_Movimientos.Rows.Count > 0)
                {
                    Div_Detalles_Cuenta.Visible = true;
                    Grid_Movimientos_Pendientes.DataSource = Dt_Historial_Movimientos;
                    Grid_Movimientos_Pendientes.PageIndex = Indice_Pagina;
                    Grid_Movimientos_Pendientes.DataBind();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Resumen de Predio", "alert('No se encontraron Movimientos Pendientes de Aplicar')", true);
                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Convenios_Cuenta_SelectedIndexChanged
    ///DESCRIPCIÓN: Evento del grid de convenios que muestra los detalles de la fila en una ventana emergente
    ///PROPIEDADES:     
    ///CREO: Christian Perez Ibarra
    ///FECHA_CREO: 14/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Historial_Movimientos_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Historial_Movimientos.SelectedIndex > -1)
        {
            Cargar_Ventana_Emergente_Validacion_Orden_Variacion();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Historial_Pagos_SelectedIndexChanged
    ///DESCRIPCIÓN: Evento del grid de convenios que muestra los detalles de la fila en una ventana emergente
    ///PROPIEDADES:     
    ///CREO: Christian Perez Ibarra
    ///FECHA_CREO: 14/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Historial_Pagos_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Historial_Pagos.SelectedIndex > -1)
        {
            Cargar_Ventana_Detalles_Pago_Predial();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Convenios_Cuenta_SelectedIndexChanged
    ///DESCRIPCIÓN: Evento del grid de convenios que muestra los detalles de la fila en una ventana emergente
    ///PROPIEDADES:     
    ///CREO: Christian Perez Ibarra
    ///FECHA_CREO: 14/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Traslado_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Traslado.SelectedIndex > -1)
        {
            Cargar_Ventana_Detalles_Pago_Traslado();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Convenios_Cuenta_SelectedIndexChanged
    ///DESCRIPCIÓN: Evento del grid de convenios que muestra los detalles de la fila en una ventana emergente
    ///PROPIEDADES:     
    ///CREO: Christian Perez Ibarra
    ///FECHA_CREO: 14/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Impuesto_Fraccionamiento_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Impuesto_Fraccionamiento.SelectedIndex > -1)
        {
            Cargar_Ventana_Detalles_Pago_Fraccionamiento();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Convenios_Cuenta_SelectedIndexChanged
    ///DESCRIPCIÓN: Evento del grid de convenios que muestra los detalles de la fila en una ventana emergente
    ///PROPIEDADES:     
    ///CREO: Christian Perez Ibarra
    ///FECHA_CREO: 14/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Derechos_Supervision_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Derechos_Supervision.SelectedIndex > -1)
        {
            Cargar_Ventana_Detalles_Pago_Supervision();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Convenios_Cuenta_SelectedIndexChanged
    ///DESCRIPCIÓN: Evento del grid de convenios que muestra los detalles de la fila en una ventana emergente
    ///PROPIEDADES:     
    ///CREO: Christian Perez Ibarra
    ///FECHA_CREO: 14/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Constancias_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Constancias.SelectedIndex > -1)
        {
            Cargar_Ventana_Detalles_Pago_Constancias();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Convenios_Cuenta_SelectedIndexChanged
    ///DESCRIPCIÓN: Evento del grid de convenios que muestra los detalles de la fila en una ventana emergente
    ///PROPIEDADES:     
    ///CREO: Christian Perez Ibarra
    ///FECHA_CREO: 14/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Movimientos_Pendientes_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Movimientos_Pendientes.SelectedIndex > -1)
        {
            Cargar_Ventana_Emergente_Validacion_Orden_Variacion_Pendiente();
        }
    }

    protected void Grid_Movimientos_Pendientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Cargar_Grid_Movimientos_Pendientes(e.NewPageIndex);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN     : Grid_Movimientos_Pendientes_RowDataBound
    ///DESCRIPCIÓN              : Evento del grid para quitar los ceros del Número de Movimiento
    ///PROPIEDADES:     
    ///CREO                     : Antonio Salvador Benavides Guardado
    ///FECHA_CREO               : 22/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Movimientos_Pendientes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[1] != null)
            {
                if (e.Row.Cells[1].Text.Trim() != "")
                {
                    e.Row.Cells[1].Text = Convert.ToInt64(e.Row.Cells[1].Text).ToString();
                }
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Historial_Pagos
    ///DESCRIPCIÓN: Metodo que llena el grid de historial de pagos
    ///PROPIEDADES:     
    ///CREO: Christian Perez Ibarra
    ///FECHA_CREO: 14/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Llenar_Grid_Historial_Pagos(int Page_Index)
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
        Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial_ID = Txt_Cuenta_Predial.Text.Trim();
        DataTable Dt_Resumen_Predio_Pagos = Rs_Consulta_Ope_Resumen_Predio.Consultar_Historial_Pagos();
        Boolean Monto_Corriente = false;

        Grid_Historial_Pagos.DataSource = Dt_Resumen_Predio_Pagos;
        Grid_Historial_Pagos.PageIndex = Page_Index;
        Grid_Historial_Pagos.DataBind();
        for (int x = 0; x < Dt_Resumen_Predio_Pagos.Rows.Count; x++)
        {
            if (Dt_Resumen_Predio_Pagos.Rows[x]["Monto_Corriente"].ToString().Trim() != "0")
                Monto_Corriente = true;
        }
        if (Monto_Corriente != true)
            Grid_Historial_Pagos.Columns[4].Visible = false;
        Grid_Historial_Pagos.Columns[12].Visible = false;
        Grid_Historial_Pagos.Columns[15].Visible = false;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Convenios_Impuestos_Derechos_Supervision_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Historial_Pagos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Historial_Pagos.SelectedIndex = (-1);
            Llenar_Grid_Historial_Pagos(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Convenios_Impuestos_Derechos_Supervision_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Constancias_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Historial_Pagos.DataBind();
        Grid_Traslado.DataBind();
        Grid_Impuesto_Fraccionamiento.DataBind();
        Grid_Derechos_Supervision.DataBind();
        Cls_Ope_Pre_Resumen_Predio_Negocio Consultar_Pagos = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataTable Dt_Pagos = new DataTable();
        String P_Cuenta_Predial = Hdf_Cuenta_Predial_ID.Value;
        try
        {
            Consultar_Pagos.P_Cuenta_Predial_ID = P_Cuenta_Predial;
            Dt_Pagos = Consultar_Pagos.Consultar_Pagos_Constancias();
            Dt_Pagos.DefaultView.Sort = Ope_Caj_Pagos.Campo_Fecha + " DESC, " + Ope_Caj_Pagos.Campo_No_Recibo + " DESC";
            Grid_Constancias.DataSource = Dt_Pagos;
            Grid_Constancias.PageIndex = e.NewPageIndex;
            Grid_Constancias.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Convenios_Impuestos_Derechos_Supervision_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Derechos_Supervision_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Historial_Pagos.DataBind();
        Grid_Traslado.DataBind();
        Grid_Impuesto_Fraccionamiento.DataBind();
        Grid_Constancias.DataBind();
        Cls_Ope_Pre_Resumen_Predio_Negocio Consultar_Pagos = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataTable Dt_Pagos = new DataTable();
        String P_Cuenta_Predial = Hdf_Cuenta_Predial_ID.Value;
        try
        {
            Consultar_Pagos.P_Cuenta_Predial_ID = P_Cuenta_Predial;
            Dt_Pagos = Consultar_Pagos.Consultar_Pagos_Derechos_Supervision();
            Grid_Derechos_Supervision.DataSource = Dt_Pagos;
            Grid_Derechos_Supervision.PageIndex = e.NewPageIndex;
            Grid_Derechos_Supervision.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Convenios_Impuestos_Derechos_Supervision_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Impuesto_Fraccionamiento_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Historial_Pagos.DataBind();
        Grid_Traslado.DataBind();
        Grid_Derechos_Supervision.DataBind();
        Grid_Constancias.DataBind();
        Cls_Ope_Pre_Resumen_Predio_Negocio Consultar_Pagos = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataTable Dt_Pagos = new DataTable();
        String P_Cuenta_Predial = Hdf_Cuenta_Predial_ID.Value;
        try
        {
            Consultar_Pagos.P_Cuenta_Predial_ID = P_Cuenta_Predial;
            Dt_Pagos = Consultar_Pagos.Consultar_Pagos_Fraccionamientos();
            Grid_Impuesto_Fraccionamiento.DataSource = Dt_Pagos;
            Grid_Impuesto_Fraccionamiento.PageIndex = e.NewPageIndex;
            Grid_Impuesto_Fraccionamiento.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Convenios_Impuestos_Derechos_Supervision_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Traslado_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Historial_Pagos.DataBind();
        Grid_Impuesto_Fraccionamiento.DataBind();
        Grid_Derechos_Supervision.DataBind();
        Grid_Constancias.DataBind();
        Cls_Ope_Pre_Resumen_Predio_Negocio Consultar_Pagos = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataTable Dt_Pagos = new DataTable();
        String P_Cuenta_Predial = Hdf_Cuenta_Predial_ID.Value;
        try
        {
            Consultar_Pagos.P_Cuenta_Predial_ID = P_Cuenta_Predial;
            Dt_Pagos = Consultar_Pagos.Consultar_Pagos_Traslado();
            Grid_Traslado.DataSource = Dt_Pagos;
            Grid_Traslado.PageIndex = e.NewPageIndex;
            Grid_Traslado.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Historial_Movimientos
    ///DESCRIPCIÓN: Metodo que llena el grid de historial de movimientos
    ///PROPIEDADES:     
    ///CREO: Christian Perez Ibarra
    ///FECHA_CREO: 14/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Llenar_Grid_Historial_Movimientos(int Page_Index)
    {
        var Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
        string Comparar_Estatus_Vigente = "True";
        string Comparar_Estatus = "True";
        string rechazada = "false";
        string pendientes = "false";
        Resumen_Predio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
        Resumen_Predio.P_Validar_Contrarecibos_Pagados = true;
        DataTable Dt_Resumen_Predio_Movimientos = Resumen_Predio.Consultar_Historial_Movimientos();
        //Session["Dt_Historial"] = Dt_Historial_Pagos;

        foreach (DataRow Dr_Movs in Dt_Resumen_Predio_Movimientos.Rows)
        {
            //if ((Dr_Movs[Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden].ToString().Trim() == "ACEPTADA" || Dr_Movs[Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden].ToString().Trim() == "POR VALIDAR") || Dr_Movs["ESTATUS_CONTRARECIBO"].ToString().Trim() == "POR PAGAR" || Dr_Movs["ESTATUS_CONTRARECIBO"].ToString().Trim() != "DIRECTA" || Dr_Movs["ESTATUS_CONTRARECIBO"].ToString().Trim() != "VALIDADO")
            if (Dr_Movs["NO_CONTRARECIBO"].ToString().Trim() != "DIRECTA" && Dr_Movs["ESTATUS_CONTRARECIBO"].ToString().Trim() != "PAGADO")
            {
                Dt_Resumen_Predio_Movimientos.Rows.Remove(Dr_Movs);
                Dt_Resumen_Predio_Movimientos.AcceptChanges();
                //pendientes = "true";
                break;
            }
        }

        for (int x = 0; x < Dt_Resumen_Predio_Movimientos.Rows.Count; x++)
        {
            if (Dt_Resumen_Predio_Movimientos.Rows[x][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden].ToString().Trim() == "RECHAZADA" || Dt_Resumen_Predio_Movimientos.Rows[x][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden].ToString().Trim() == "POR VALIDAR" && Dt_Resumen_Predio_Movimientos.Rows[x]["ESTATUS_CONTRARECIBO"].ToString().Trim() == "DIRECTA")
                pendientes = "true";
            if (Dt_Resumen_Predio_Movimientos.Rows[x][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden].ToString().Trim() == "RECHAZADA" && rechazada == "false")
            {
                //Dt_Resumen_Predio_Movimientos.Rows[x][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden] = "BAJA TEMPORAL";
                //rechazada = "true";
                for (int y = 0; y < Dt_Resumen_Predio_Movimientos.Rows.Count; y++)
                {
                    if ((Dt_Resumen_Predio_Movimientos.Rows[y][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden].ToString().Trim() == "VIGENTE" && Comparar_Estatus == "True"))
                    {
                        Dt_Resumen_Predio_Movimientos.Rows[y][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden] = "BAJA";
                        Comparar_Estatus = "False";
                    }
                    //else
                    //{
                    //    Dt_Resumen_Predio_Movimientos.Rows[y][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden] = "BAJA TEMPORAL";
                    //}
                }
            }
        }

        for (int x = 0; x < Dt_Resumen_Predio_Movimientos.Rows.Count; x++)
        {
            if (Dt_Resumen_Predio_Movimientos.Rows[x][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden].ToString().Trim() == "ACEPTADA" && Comparar_Estatus_Vigente == "True")
            {
                if (pendientes == "false")
                    Dt_Resumen_Predio_Movimientos.Rows[x][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden] = "VIGENTE";
                else
                    Dt_Resumen_Predio_Movimientos.Rows[x][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden] = "BAJA TEMPORAL";
                Comparar_Estatus_Vigente = "False";

            } if (Dt_Resumen_Predio_Movimientos.Rows[x][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden].ToString().Trim() == "ACEPTADA" && Comparar_Estatus_Vigente == "False")
            {
                Dt_Resumen_Predio_Movimientos.Rows[x][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden] = "BAJA";
            }
        }


        Grid_Historial_Movimientos.PageIndex = Page_Index;
        Grid_Historial_Movimientos.DataSource = Dt_Resumen_Predio_Movimientos;
        Grid_Historial_Movimientos.DataBind();
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Convenios_Impuestos_Derechos_Supervision_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Historial_Movimientos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Historial_Movimientos.SelectedIndex = (-1);
            Llenar_Grid_Historial_Movimientos(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Convenios
    ///DESCRIPCIÓN: Metodo que llena el grid de historial de movimientos
    ///PROPIEDADES:     
    ///CREO: Christian Perez Ibarra
    ///FECHA_CREO: 14/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Llenar_Grid_Convenios(int Page_Index)
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
        Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
        Rs_Consulta_Ope_Resumen_Predio.P_Validar_Convenios_Cumplidos = true;
        DataTable Dt_Resumen_Predio_Convenios = Rs_Consulta_Ope_Resumen_Predio.Consultar_Convenios();
        Session["Dt_Resumen_Predio_Convenios"] = Dt_Resumen_Predio_Convenios;
        Hdn_Estatus_Convenio.Value = "";
        if (Dt_Resumen_Predio_Convenios.Rows.Count > 0)
        {
            if (Dt_Resumen_Predio_Convenios.Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Dt_Resumen_Predio_Convenios.Rows[0]["Estatus"].ToString()))
                {
                    Hdn_Estatus_Convenio.Value = Dt_Resumen_Predio_Convenios.Rows[0]["Estatus"].ToString();
                }
            }
            //for (int x = 0; x < Dt_Resumen_Predio_Convenios.Rows.Count; x++)
            //{
            //    if (Dt_Resumen_Predio_Convenios.Rows.Count < 2)
            //    {
            //        if (!String.IsNullOrEmpty(Dt_Resumen_Predio_Convenios.Rows[x]["Estatus"].ToString()))
            //        {
            //            Hdn_Estatus_Convenio.Value = Dt_Resumen_Predio_Convenios.Rows[x]["Estatus"].ToString();
            //        }
            //    }
            //}
            if (Hdn_Estatus_Convenio.Value == "ACTIVO")
            {
                foreach (DataRow Renglon in Dt_Resumen_Predio_Convenios.Rows)
                {
                    if (Renglon["No_Convenio"].ToString().StartsWith("CDER"))
                    {
                        if (!Lbl_Estatus.Text.Contains(" CONVENIDA DS"))
                        {
                            Lbl_Estatus.Text = " CONVENIDA DS" + " " + Lbl_Estatus.Text;
                        }
                        Hdn_Tipo_Convenio.Value = "CONVENIDA DS";
                    }
                    if (Renglon["No_Convenio"].ToString().StartsWith("CFRA"))
                    {
                        if (!Lbl_Estatus.Text.Contains(" CONVENIDA FRACCIONAMIENTOS"))
                        {
                            Lbl_Estatus.Text = " CONVENIDA FRACCIONAMIENTOS" + " " + Lbl_Estatus.Text;
                        }
                        Hdn_Tipo_Convenio.Value = "CONVENIDA FRACCIONAMIENTOS";
                    }
                    if (Renglon["No_Convenio"].ToString().StartsWith("CTRA"))
                    {
                        if (!Lbl_Estatus.Text.Contains(" CONVENIDA TD"))
                        {
                            Lbl_Estatus.Text = " CONVENIDA TD" + " " + Lbl_Estatus.Text;
                        }
                        Hdn_Tipo_Convenio.Value = "CONVENIDA TD";
                    }
                    if (Renglon["No_Convenio"].ToString().StartsWith("CPRE"))
                    {
                        if (!Lbl_Estatus.Text.Contains(" CONVENIDA PREDIAL"))
                        {
                            Lbl_Estatus.Text = " CONVENIDA PREDIAL" + " " + Lbl_Estatus.Text;
                        }
                        Hdn_Tipo_Convenio.Value = "CONVENIDA PREDIAL";
                    }
                }
            }
            else if (Hdn_Estatus_Convenio.Value == "INCUMPLIDO")
            {
                Lbl_Estatus.Text = " CONVENIO INCUMPLIDO " + Lbl_Estatus.Text;
            }
        }
        Grid_Convenios_Cuenta.PageIndex = Page_Index;
        Grid_Convenios_Cuenta.DataSource = Dt_Resumen_Predio_Convenios;
        Grid_Convenios_Cuenta.DataBind();
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Convenios_Impuestos_Derechos_Supervision_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Convenios_Cuenta_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Convenios_Cuenta.SelectedIndex = (-1);
            Llenar_Grid_Convenios(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Grid_Estado_Cuenta_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        String P_Cuenta_Predial = Session["Cuenta_Predial_ID"].ToString().Trim();
        try
        {
            Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
            RP_Negocio.P_Cuenta_Predial = P_Cuenta_Predial;
            Grid_Estado_Cuenta.Columns[0].Visible = true;
            Grid_Estado_Cuenta.SelectedIndex = (-1);
            DataTable Dt_Adeudos = RP_Negocio.Consultar_Adeudos_Cuentas_Predial();
            //Llenar_Combo_Anios(Dt_Adeudos);
            Grid_Estado_Cuenta.PageIndex = e.NewPageIndex;
            Grid_Estado_Cuenta.DataSource = Dt_Adeudos;
            Grid_Estado_Cuenta.DataBind();
            Grid_Estado_Cuenta.Columns[0].Visible = false;
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Convenios_Cuenta_SelectedIndexChanged
    ///DESCRIPCIÓN: Evento del grid de convenios que muestra los detalles de la fila en una ventana emergente
    ///PROPIEDADES:     
    ///CREO: Christian Perez Ibarra
    ///FECHA_CREO: 14/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Convenios_Cuenta_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Convenios_Cuenta.SelectedRow.Cells[1].Text.StartsWith("CDER"))
        {
            Cargar_Ventana_Emergente_Convenios_Derechos_Supervision();
        }
        if (Grid_Convenios_Cuenta.SelectedRow.Cells[1].Text.StartsWith("CFRA"))
        {
            Cargar_Ventana_Emergente_Convenios_Fraccionamientos();
        }
        if (Grid_Convenios_Cuenta.SelectedRow.Cells[1].Text.StartsWith("CTRA"))
        {
            Cargar_Ventana_Emergente_Convenios_Traslado();
        }
        if (Grid_Convenios_Cuenta.SelectedRow.Cells[1].Text.StartsWith("CPRE"))
        {
            Cargar_Ventana_Emergente_Convenios_Predial();
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Convenios_Traslado
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente del Conenio con la ruta y parámtros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Otubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Emergente_Convenios_Traslado()
    {
        //String Ventana_Modal = "Abrir_Resumen('Ventanas_Emergentes/Convenios/Frm_Convenios_Traslado.aspx";
        String Parametros = "?Cuenta_Predial_ID=" + Session["Cuenta_Predial_ID_Convenios"].ToString().Trim();
        Parametros += "&No_Convenio=" + Convert.ToInt64(Grid_Convenios_Cuenta.SelectedRow.Cells[1].Text.Substring(4)).ToString("0000000000");
        Parametros += "'";
        //String Popiedades = ", 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
        String Propiedades = ", 'height=600,width=800,scrollbars=1');";
        //Grid_Convenios_Cuenta.Attributes.Add("onclick", Ventana_Modal + Parametros + Propiedades);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios de Traslado", "window.open('Ventanas_Emergentes/Convenios/Frm_Convenios_Traslado.aspx" + Parametros + ", null " + Propiedades, true);
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Convenios_Predial
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente del Conenio con la ruta y parámtros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Otubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Emergente_Convenios_Predial()
    {
        //String Ventana_Modal = "Abrir_Resumen('Ventanas_Emergentes/Convenios/Frm_Convenios_Predial.aspx";
        String Parametros = "?Cuenta_Predial_ID=" + Session["Cuenta_Predial_ID_Convenios"].ToString().Trim();
        Parametros += "&No_Convenio=" + Convert.ToInt64(Grid_Convenios_Cuenta.SelectedRow.Cells[1].Text.Substring(4)).ToString("0000000000");
        Parametros += "'";
        //String Popiedades = ", 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
        String Propiedades = ", 'height=600,width=800,scrollbars=1');";
        //Grid_Convenios_Cuenta.Attributes.Add("onclick", Ventana_Modal + Parametros + Propiedades);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios de Traslado", "window.open('Ventanas_Emergentes/Convenios/Frm_Convenios_Predial.aspx" + Parametros + ", null " + Propiedades, true);
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Convenios_Fraccionamientos
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente del Conenio con la ruta y parámtros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Otubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Emergente_Convenios_Fraccionamientos()
    {
        //String Ventana_Modal = "Abrir_Resumen('Ventanas_Emergentes/Convenios/Frm_Convenios_Fraccionamiento.aspx";
        String Parametros = "?Cuenta_Predial_ID=" + Session["Cuenta_Predial_ID_Convenios"].ToString().Trim();
        Parametros += "&No_Convenio=" + Convert.ToInt64(Grid_Convenios_Cuenta.SelectedRow.Cells[1].Text.Substring(4)).ToString("0000000000");
        Parametros += "&No_Impuesto_Fraccionamiento=" + Convert.ToInt64(Grid_Convenios_Cuenta.SelectedRow.Cells[6].Text).ToString("0000000000");
        Parametros += "'";
        //String Popiedades = ", 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
        String Propiedades = ", 'height=600,width=800,scrollbars=1');";
        //Grid_Convenios_Cuenta.Attributes.Add("onclick", Ventana_Modal + Parametros + Propiedades);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios de Traslado", "window.open('Ventanas_Emergentes/Convenios/Frm_Convenios_Fraccionamiento.aspx" + Parametros + ", null " + Propiedades, true);
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Convenios_Derechos_Supervision
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente del Conenio con la ruta y parámtros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Otubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Emergente_Convenios_Derechos_Supervision()
    {
        //String Ventana_Modal = "Abrir_Resumen('Ventanas_Emergentes/Convenios/Frm_Convenios_Derechos_Supervision.aspx";
        String Parametros = "?Cuenta_Predial_ID=" + Session["Cuenta_Predial_ID_Convenios"].ToString().Trim();
        Parametros += "&No_Convenio=" + Convert.ToInt64(Grid_Convenios_Cuenta.SelectedRow.Cells[1].Text.Substring(4)).ToString("0000000000");
        Parametros += "&No_Impuesto_Derechos_Supervision=" + Convert.ToInt64(Grid_Convenios_Cuenta.SelectedRow.Cells[6].Text).ToString("0000000000");
        Parametros += "'";
        //String Popiedades = ", 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
        String Propiedades = ", 'height=600,width=800,scrollbars=1');";
        //Grid_Convenios_Cuenta.Attributes.Add("onclick", Ventana_Modal + Parametros + Propiedades);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios de Traslado", "window.open('Ventanas_Emergentes/Convenios/Frm_Convenios_Derechos_Supervision.aspx" + Parametros + ", null " + Propiedades, true);
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
            throw new Exception(ex.Message, ex);
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
    /// *************************************************************************************
    /// NOMBRE:              Asignar_Datos_Propietarios
    /// DESCRIPCIÓN:         Metodo para asignar los datos de los propietarios a una datatable
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          26/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    protected DataTable Asignar_Datos_Propietarios()
    {
        DataTable Dt_Propietario = new DataTable();
        DataRow Propietarios;
        Dt_Propietario.Columns.Add("Nombre");
        Dt_Propietario.Columns.Add("Propietario_Poseedor");
        Dt_Propietario.Columns.Add("Rfc");
        Dt_Propietario.Columns.Add("Colonia");
        Dt_Propietario.Columns.Add("Calle");
        Dt_Propietario.Columns.Add("Numero_Exterior");
        Dt_Propietario.Columns.Add("Numero_Interior");
        Dt_Propietario.Columns.Add("Estado");
        Dt_Propietario.Columns.Add("Ciudad");
        Dt_Propietario.Columns.Add("Cod_Pos");
        Dt_Propietario.Columns.Add("Copropietarios");
        Dt_Propietario.Columns.Add("Tipo_Convenio");
        Propietarios = Dt_Propietario.NewRow();
        Propietarios["Nombre"] = Txt_Nombre_Propietario.Text.Trim();
        Propietarios["Propietario_Poseedor"] = Txt_Propietario_Poseedor_Propietario.Text.Trim();
        Propietarios["Rfc"] = Txt_Rfc_Propietario.Text.Trim();
        Propietarios["Colonia"] = Txt_Colonia_Propietario.Text.Trim();
        Propietarios["Calle"] = Txt_Calle_Propietario.Text.Trim();
        Propietarios["Numero_Exterior"] = Txt_Numero_Exterior_Propietario.Text.Trim();
        Propietarios["Numero_Interior"] = Txt_Numero_Interior_Propietario.Text.Trim();
        Propietarios["Estado"] = Txt_Estado_Propietario.Text.Trim();
        Propietarios["Ciudad"] = Txt_Ciudad_Propietario.Text.Trim();
        Propietarios["Cod_Pos"] = Txt_Cod_Postal_Propietario.Text.Trim();
        Propietarios["Copropietarios"] = Txt_Copropietarios_Propietario.Text.Trim();
        Propietarios["Tipo_Convenio"] = Hdn_Tipo_Convenio.Value;
        if (Dt_Propietario.Rows.Count == 0)
        {
            Dt_Propietario.Rows.InsertAt(Propietarios, 0);

        }
        return Dt_Propietario;

    }

    /// *************************************************************************************
    /// NOMBRE:              Asignar_Datos_Impuestos
    /// DESCRIPCIÓN:         Metodo para asignar los datos de los propietarios a una datatable
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          26/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    protected DataTable Asignar_Datos_Impuestos()
    {
        String Total_Construccion = String.Empty;
        DataTable Dt_Impuestos = new DataTable();
        try
        {

            DataRow Impuestos;
            Dt_Impuestos.Columns.Add("Valor_Fiscal");
            Dt_Impuestos.Columns.Add("Tasa");
            Dt_Impuestos.Columns.Add("Periodo_Corriente");
            Dt_Impuestos.Columns.Add("Tipo_Predio");
            Dt_Impuestos.Columns.Add("Cuota_Anual");
            Dt_Impuestos.Columns.Add("Cuota_Bimestral");
            Dt_Impuestos.Columns.Add("Dif_Construccion");
            Dt_Impuestos.Columns.Add("Porciento_Exencion");
            Dt_Impuestos.Columns.Add("Termino_Exencion");
            Dt_Impuestos.Columns.Add("Fecha_Avaluo");
            Dt_Impuestos.Columns.Add("Cuota_Fija");
            Dt_Impuestos.Columns.Add("Beneficio");
            //Detalles
            Dt_Impuestos.Columns.Add("Cuota_Fija_Por");
            Dt_Impuestos.Columns.Add("Plazo_Financiamiento");
            Dt_Impuestos.Columns.Add("Cuota_Minima");
            Dt_Impuestos.Columns.Add("Excedente_Construccion_Multiplicador");
            Dt_Impuestos.Columns.Add("Excedente_Construccion_Multiplicando");
            Dt_Impuestos.Columns.Add("Excedente_Valor_Multiplicador");
            Dt_Impuestos.Columns.Add("Excedente_Valor_Multiplicando");
            Dt_Impuestos.Columns.Add("Total_Impuesto");
            Dt_Impuestos.Columns.Add("Fundamento_Legal");
            Dt_Impuestos.Columns.Add("Excedente_Construccion_Resultado");
            Dt_Impuestos.Columns.Add("Excedente_Valor_Resultado");

            Impuestos = Dt_Impuestos.NewRow();
            Impuestos["Valor_Fiscal"] = Txt_Valor_Fiscal_Impuestos.Text.Trim();
            Impuestos["Tasa"] = Txt_Tasa_Impuestos.Text.Trim();
            Impuestos["Periodo_Corriente"] = Txt_Periodo_Corriente_Impuestos.Text.Trim();
            Impuestos["Tipo_Predio"] = Txt_Tipo_Periodo_Impuestos.Text.Trim();
            Impuestos["Cuota_Anual"] = Txt_Cuota_Anual_Impuestos.Text.Trim();
            Impuestos["Cuota_Bimestral"] = Txt_Cuota_Bimestral_Impuestos.Text.Trim();
            Impuestos["Dif_Construccion"] = Txt_Dif_Construccion_Impuestos.Text.Trim();
            Impuestos["Porciento_Exencion"] = Txt_Porciento_Exencion_Impuestos.Text.Trim();
            Impuestos["Termino_Exencion"] = Txt_Fecha_Termino_Extencion.Text.Trim();
            Impuestos["Fecha_Avaluo"] = Txt_Fecha_Avaluo_Impuestos.Text.Trim();
            if (Chk_Cuota_Fija.Checked)
            {
                Impuestos["Cuota_Fija"] = "SI";// Txt_Cuota_Fija_Impuestos.Text.Trim();
            }
            else
            {
                Impuestos["Cuota_Fija"] = "NO";// Txt_Cuota_Fija_Impuestos.Text.Trim();
            }
            Impuestos["Beneficio"] = Txt_Cuota_Fija.Text.Trim();
            //Detalles
            Impuestos["Cuota_Fija_Por"] = Txt_Cuota_Fija.Text.Trim();

            Impuestos["Excedente_Construccion_Resultado"] = Total_Construccion;
            if (Dt_Impuestos.Rows.Count == 0)
            {
                Dt_Impuestos.Rows.InsertAt(Impuestos, 0);

            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Asignar Datos Impuestos" + Ex.Message);
        }
        return Dt_Impuestos;

    }
    /// *************************************************************************************
    /// NOMBRE:              Img_Btn_Imprimir_Propietario_Click
    /// DESCRIPCIÓN:         Genera el reporte de los datos generales del resumen de predio
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          26/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    protected void Img_Btn_Imprimir_Propietario_Click(object sender, ImageClickEventArgs e)
    {
        String Nombre_Reporte = String.Empty;
        String Nombre_Repote_Crystal = String.Empty;
        String Formato = String.Empty;
        //instacia la clase de negocio
        Cls_Ope_Pre_Resumen_Predio_Negocio Imprimir_Resumen_Predio_Propietario = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        //Crea un nuevo data table
        DataTable Dt_Generales = new DataTable();
        //LLenado de datos
        DataTable Dt_Propietarios = Asignar_Datos_Propietarios();
        //instancia el data set que contiene el data table 
        Ds_Pre_Resumen_Predio_Generales Resumen_Predio_Propietario = new Ds_Pre_Resumen_Predio_Generales();
        //obtiene el numero de cuenta predial
        Imprimir_Resumen_Predio_Propietario.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
        //manda a llamar a la consulta para traer los datos
        Dt_Generales = Imprimir_Resumen_Predio_Propietario.Consultar_Imprimir_Resumen_Generales();
        Dt_Generales.TableName = "Dt_Generales";
        Dt_Propietarios.TableName = "Dt_Propietario";
        Resumen_Predio_Propietario.Clear();
        Resumen_Predio_Propietario.Tables.Clear();
        Resumen_Predio_Propietario.Tables.Add(Dt_Generales.Copy());
        Resumen_Predio_Propietario.Tables.Add(Dt_Propietarios.Copy());
        Nombre_Reporte = "Reporte Propietario";
        Nombre_Repote_Crystal = "Rpt_Pre_Resumen_Predio_Propietario.rpt";
        Formato = "PDF";
        //llama el metodo con los parametros de la consulta y el data set
        Generar_Reportes(Resumen_Predio_Propietario, Nombre_Repote_Crystal, Nombre_Reporte, Formato);
    }
    /// *************************************************************************************
    /// NOMBRE:              Generar_Reportes
    /// DESCRIPCIÓN:         Metodo para generar el reporte
    /// PARÁMETROS:          Dataset a imprimir
    ///                      Nombre del reporte de Crystal
    ///                      Nombre como se llamara el reporte
    ///                      Formato del reporte
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          26/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    private void Generar_Reportes(DataSet Ds_Recibo, String Nombre_Reporte_Crystal, String Nombre_Reporte, String Formato)
    {

        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";


        // Ruta donde se encuentra el reporte Crystal
        Ruta_Reporte_Crystal = "../Rpt/Predial/Resumen_Predio/" + Nombre_Reporte_Crystal;

        // Se crea el nombre del reporte
        String Nombre_Report = Nombre_Reporte + "_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("HH'-'mm'-'ss"));

        // Se da el nombre del reporte que se va generar
        if (Formato == "PDF")
            Nombre_Reporte_Generar = Nombre_Report + ".pdf";  // Es el nombre del reporte PDF que se va a generar
        else if (Formato == "Excel")
            Nombre_Reporte_Generar = Nombre_Report + ".xls";  // Es el nombre del repote en Excel que se va a generar

        Cls_Reportes Reportes = new Cls_Reportes();
        Reportes.Generar_Reporte(ref Ds_Recibo, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
        Mostrar_Reporte(Nombre_Reporte_Generar, Formato);
    }

    /// *************************************************************************************
    /// NOMBRE:              Img_Btn_Imprimir_Generales_Click
    /// DESCRIPCIÓN:         Genera el reporte de los datos generales del resumen de predio
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          20/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    protected void Img_Btn_Imprimir_Generales_Click(object sender, ImageClickEventArgs e)
    {
        String Nombre_Reporte = String.Empty;
        String Nombre_Repote_Crystal = String.Empty;
        String Formato = String.Empty;
        //instancia el data set que contiene el data table 
        Ds_Pre_Resumen_Predio_Generales Resumen_Predio_Propietario = new Ds_Pre_Resumen_Predio_Generales();
        Cls_Ope_Pre_Resumen_Predio_Negocio Imprimir_Resumen_Predio_Generales = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataTable Dt_Imprimir_Resumen_General = new DataTable();
        Ds_Pre_Resumen_Predio_Generales Resumen_Predio_General = new Ds_Pre_Resumen_Predio_Generales();
        Imprimir_Resumen_Predio_Generales.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();

        Dt_Imprimir_Resumen_General = Imprimir_Resumen_Predio_Generales.Consultar_Imprimir_Resumen_Generales();
        Dt_Imprimir_Resumen_General.TableName = "Dt_Generales";
        Resumen_Predio_Propietario.Clear();
        Resumen_Predio_Propietario.Tables.Clear();
        Resumen_Predio_Propietario.Tables.Add(Dt_Imprimir_Resumen_General.Copy());
        Nombre_Reporte = "Reporte General";
        Nombre_Repote_Crystal = "Rpt_Pre_Resumen_Predio_Generales.rpt";
        Formato = "PDF";
        Generar_Reportes(Resumen_Predio_Propietario, Nombre_Repote_Crystal, Nombre_Reporte, Formato);
    }

    /// *************************************************************************************
    /// NOMBRE:              Img_Btn_Imprimir_Impuestos_Click
    /// DESCRIPCIÓN:         Genera el reporte de los datos generales del resumen de predio
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          29/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    protected void Img_Btn_Imprimir_Impuestos_Click(object sender, ImageClickEventArgs e)
    {
        String Nombre_Reporte = String.Empty;
        String Nombre_Repote_Crystal = String.Empty;
        String Formato = String.Empty;
        //instacia la clase de negocio
        Cls_Ope_Pre_Resumen_Predio_Negocio Imprimir_Resumen_Predio_Impuestos = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        //Crea un nuevo data table
        DataTable Dt_Generales = new DataTable();
        //LLenado de datos
        DataTable Dt_Impuestos = Asignar_Datos_Impuestos();
        //instancia el data set que contiene el data table 
        Ds_Pre_Resumen_Predio_Generales Resumen_Predio_Impuestos = new Ds_Pre_Resumen_Predio_Generales();
        //obtiene el numero de cuenta predial
        Imprimir_Resumen_Predio_Impuestos.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
        //manda a llamar a la consulta para traer los datos
        Dt_Generales = Imprimir_Resumen_Predio_Impuestos.Consultar_Imprimir_Resumen_Generales();
        Dt_Generales.TableName = "Dt_Generales";
        Dt_Impuestos.TableName = "Dt_Impuestos";
        Resumen_Predio_Impuestos.Clear();
        Resumen_Predio_Impuestos.Tables.Clear();
        Resumen_Predio_Impuestos.Tables.Add(Dt_Generales.Copy());
        Resumen_Predio_Impuestos.Tables.Add(Dt_Impuestos.Copy());
        Nombre_Reporte = "Reporte Impuetos";
        Nombre_Repote_Crystal = "Rpt_Pre_Resumen_Predio_Impuestos.rpt";
        Formato = "PDF";
        //llama el metodo con los parametros de la consulta y el data set
        Generar_Reportes(Resumen_Predio_Impuestos, Nombre_Repote_Crystal, Nombre_Reporte, Formato);
    }


    /// *************************************************************************************
    /// NOMBRE:              Asignar_Datos_Estado_Cuenta
    /// DESCRIPCIÓN:         Metodo para asignar los datos de los propietarios a una datatable
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          26/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    protected DataTable Asignar_Datos_Estado_Cuenta()
    {
        String Total_Construccion = String.Empty;
        DataTable Dt_Estado_Cuenta = new DataTable();
        try
        {

            DataRow Estado_Cuenta;
            Dt_Estado_Cuenta.Columns.Add("Periodo_Rezago");
            Dt_Estado_Cuenta.Columns.Add("Adeudo_Rezago");
            Dt_Estado_Cuenta.Columns.Add("Periodo_Actual");
            Dt_Estado_Cuenta.Columns.Add("Adeudo_Actual");
            Dt_Estado_Cuenta.Columns.Add("Total_Recargos_Ordinarios");
            Dt_Estado_Cuenta.Columns.Add("Honorarios");
            Dt_Estado_Cuenta.Columns.Add("Recargos_Moratorios");
            Dt_Estado_Cuenta.Columns.Add("Gastos_Ejecucion");
            Dt_Estado_Cuenta.Columns.Add("Subtotal");
            Dt_Estado_Cuenta.Columns.Add("Descuentos_Pronto_Pago");
            Dt_Estado_Cuenta.Columns.Add("Descuento_Recargos_Ordinarios");
            Dt_Estado_Cuenta.Columns.Add("Descuento_Recargos_Moratorios");
            Dt_Estado_Cuenta.Columns.Add("Descuento_Honorarios");
            Dt_Estado_Cuenta.Columns.Add("Total");

            Estado_Cuenta = Dt_Estado_Cuenta.NewRow();
            Estado_Cuenta["Periodo_Rezago"] = Txt_Periodo_Rezago.Text.Trim();
            Estado_Cuenta["Adeudo_Rezago"] = Txt_Adeudo_Rezago.Text.Trim();
            Estado_Cuenta["Periodo_Actual"] = Txt_Periodo_Actual.Text.Trim();
            Estado_Cuenta["Adeudo_Actual"] = Txt_Adeudo_Actual.Text.Trim();
            Estado_Cuenta["Total_Recargos_Ordinarios"] = Txt_Total_Recargos_Ordinarios.Text.Trim();
            Estado_Cuenta["Honorarios"] = Txt_Honorarios.Text.Trim();
            Estado_Cuenta["Recargos_Moratorios"] = Txt_Recargos_Moratorios.Text.Trim();
            Estado_Cuenta["Gastos_Ejecucion"] = Txt_Gastos_Ejecucion.Text.Trim();
            Estado_Cuenta["Subtotal"] = Txt_Subtotal.Text.Trim();
            Estado_Cuenta["Descuentos_Pronto_Pago"] = Txt_Descuento_Pronto_Pago.Text.Trim();
            //Detalles
            Estado_Cuenta["Descuento_Recargos_Ordinarios"] = Txt_Descuento_Recargos_Ordinarios.Text.Trim();
            Estado_Cuenta["Descuento_Recargos_Moratorios"] = Txt_Descuento_Recargos_Moratorios.Text.Trim();
            Estado_Cuenta["Total"] = Txt_Total.Text.Trim();

            if (Dt_Estado_Cuenta.Rows.Count == 0)
            {
                Dt_Estado_Cuenta.Rows.InsertAt(Estado_Cuenta, 0);

            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message, Ex);
        }
        return Dt_Estado_Cuenta;

    }
    protected void Img_Btn_Imprimir_Historial_Movimientos_Click(object sender, ImageClickEventArgs e)
    {
        String Nombre_Reporte = String.Empty;
        String Nombre_Repote_Crystal = String.Empty;
        String Formato = String.Empty;
        DataTable Dt_Generales = new DataTable();
        string Comparar_Estatus_Vigente = "True";
        string Comparar_Estatus = "True";
        string rechazada = "false";
        //instacia la clase de negocio
        Cls_Ope_Pre_Resumen_Predio_Negocio Imprimir_Resumen_Predio_Estado_Cuenta = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
        Ds_Pre_Resumen_Predio_Generales Resumen_Predio_Historial_Movimientos = new Ds_Pre_Resumen_Predio_Generales();
        try
        {
            Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            DataTable Dt_Resumen_Predio_Movimientos = Rs_Consulta_Ope_Resumen_Predio.Consultar_Historial_Movimientos();
            for (int x = 0; x < Dt_Resumen_Predio_Movimientos.Rows.Count; x++)
            {
                if (Dt_Resumen_Predio_Movimientos.Rows[x][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden].ToString().Trim() == "ACEPTADA" && Comparar_Estatus_Vigente == "True")
                {
                    Dt_Resumen_Predio_Movimientos.Rows[x][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden] = "VIGENTE";
                    Comparar_Estatus_Vigente = "False";
                } if (Dt_Resumen_Predio_Movimientos.Rows[x][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden].ToString().Trim() == "ACEPTADA" && Comparar_Estatus_Vigente == "False")
                {
                    Dt_Resumen_Predio_Movimientos.Rows[x][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden] = "BAJA";
                }
            }
            for (int x = 0; x < Dt_Resumen_Predio_Movimientos.Rows.Count; x++)
            {
                if (Dt_Resumen_Predio_Movimientos.Rows[x][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden].ToString().Trim() == "RECHAZADA" && rechazada == "false")
                {
                    Dt_Resumen_Predio_Movimientos.Rows[x][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden] = "BAJA TEMPORAL";
                    rechazada = "true";
                    for (int y = 0; y < Dt_Resumen_Predio_Movimientos.Rows.Count; y++)
                    {
                        if ((Dt_Resumen_Predio_Movimientos.Rows[y][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden].ToString().Trim() == "VIGENTE" && Comparar_Estatus == "True"))
                        {
                            Dt_Resumen_Predio_Movimientos.Rows[y][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden] = "BAJA";
                            Comparar_Estatus = "False";
                        }
                        //else
                        //{
                        //    Dt_Resumen_Predio_Movimientos.Rows[y][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden] = "BAJA TEMPORAL";
                        //}
                    }
                }
            }
            Dt_Generales = Rs_Consulta_Ope_Resumen_Predio.Consultar_Imprimir_Resumen_Generales();
            Dt_Generales.TableName = "Dt_Generales";
            Dt_Resumen_Predio_Movimientos.TableName = "Dt_Historial_Movimientos";
            Resumen_Predio_Historial_Movimientos.Clear();
            Resumen_Predio_Historial_Movimientos.Tables.Clear();
            Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Generales.Copy());
            Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Resumen_Predio_Movimientos.Copy());
            Nombre_Reporte = "Reporte Estado Cuenta";
            Nombre_Repote_Crystal = "Rpt_Pre_Resumen_Predio_Historial_Movimientos.rpt";
            Formato = "PDF";
            //llama el metodo con los parametros de la consulta y el data set
            Generar_Reportes_Historial_Movimientos(Resumen_Predio_Historial_Movimientos, Nombre_Repote_Crystal, Nombre_Reporte, Formato);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
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
    protected void Img_Btn_Imprimir_Histotial_Pagos_Click(object sender, ImageClickEventArgs e)
    {
        String Nombre_Reporte = String.Empty;
        String Nombre_Repote_Crystal = String.Empty;
        String Formato = String.Empty;
        DataTable Dt_Generales = new DataTable();
        //instacia la clase de negocio
        Cls_Ope_Pre_Resumen_Predio_Negocio Imprimir_Resumen_Predio_Estado_Cuenta = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
        Ds_Pre_Resumen_Predio_Generales Resumen_Predio_Historial_Movimientos = new Ds_Pre_Resumen_Predio_Generales();
        try
        {

            if (Cmb_Consultar_Tipo_Pago.SelectedValue == "IMPUESTOS FRACCIONAMIENTO")
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial_ID = Txt_Cuenta_Predial.Text.Trim();
                Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
                DataTable Dt_Resumen_Predio_Historial_Pagos = Rs_Consulta_Ope_Resumen_Predio.Consultar_Pagos_Fraccionamientos();
                DataTable Dt_Propietario = Asignar_Datos_Propietarios();
                Dt_Generales = Rs_Consulta_Ope_Resumen_Predio.Consultar_Imprimir_Resumen_Generales();
                Dt_Generales.TableName = "Dt_Generales";
                Dt_Propietario.TableName = "Dt_Propietario";
                Dt_Resumen_Predio_Historial_Pagos.TableName = "Dt_Historial_Pagos_Fraccionamiento";
                Resumen_Predio_Historial_Movimientos.Clear();
                Resumen_Predio_Historial_Movimientos.Tables.Clear();
                Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Generales.Copy());
                Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Propietario.Copy());
                Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Resumen_Predio_Historial_Pagos.Copy());
                Nombre_Reporte = "Reporte Historial Pagos";
                Nombre_Repote_Crystal = "Rpt_Pre_Historial_Pagos_Fraccionamiento.rpt";
                Formato = "PDF";
                //llama el metodo con los parametros de la consulta y el data set
                Generar_Reportes_Historial_Movimientos(Resumen_Predio_Historial_Movimientos, Nombre_Repote_Crystal, Nombre_Reporte, Formato);
            }
            else if (Cmb_Consultar_Tipo_Pago.SelectedValue == "DERECHOS SUPERVISION")
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial_ID = Txt_Cuenta_Predial.Text.Trim();
                Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
                DataTable Dt_Resumen_Predio_Historial_Pagos = Rs_Consulta_Ope_Resumen_Predio.Consultar_Pagos_Derechos_Supervision();
                DataTable Dt_Propietario = Asignar_Datos_Propietarios();
                Dt_Generales = Rs_Consulta_Ope_Resumen_Predio.Consultar_Imprimir_Resumen_Generales();
                Dt_Generales.TableName = "Dt_Generales";
                Dt_Propietario.TableName = "Dt_Propietario";
                Dt_Resumen_Predio_Historial_Pagos.TableName = "Dt_Historial_Pagos_derechos_Supervision";
                Resumen_Predio_Historial_Movimientos.Clear();
                Resumen_Predio_Historial_Movimientos.Tables.Clear();
                Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Generales.Copy());
                Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Propietario.Copy());
                Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Resumen_Predio_Historial_Pagos.Copy());
                Nombre_Reporte = "Reporte Historial Pagos";
                Nombre_Repote_Crystal = "Rpt_Pre_Historial_Pagos_Derechos_Supervision.rpt";
                Formato = "PDF";
                //llama el metodo con los parametros de la consulta y el data set
                Generar_Reportes_Historial_Movimientos(Resumen_Predio_Historial_Movimientos, Nombre_Repote_Crystal, Nombre_Reporte, Formato);
            }
            else if (Cmb_Consultar_Tipo_Pago.SelectedValue == "CONSTANCIAS")
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial_ID = Txt_Cuenta_Predial.Text.Trim();
                Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
                DataTable Dt_Resumen_Predio_Historial_Pagos = Rs_Consulta_Ope_Resumen_Predio.Consultar_Pagos_Constancias();
                DataTable Dt_Propietario = Asignar_Datos_Propietarios();
                Dt_Generales = Rs_Consulta_Ope_Resumen_Predio.Consultar_Imprimir_Resumen_Generales();
                Dt_Generales.TableName = "Dt_Generales";
                Dt_Propietario.TableName = "Dt_Propietario";
                Dt_Resumen_Predio_Historial_Pagos.TableName = "Dt_Historial_Pagos_Constancias";
                Resumen_Predio_Historial_Movimientos.Clear();
                Resumen_Predio_Historial_Movimientos.Tables.Clear();
                Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Generales.Copy());
                Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Propietario.Copy());
                Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Resumen_Predio_Historial_Pagos.Copy());
                Nombre_Reporte = "Reporte Historial Pagos";
                Nombre_Repote_Crystal = "Rpt_Pre_Historial_Pagos_Constancias.rpt";
                Formato = "PDF";
                //llama el metodo con los parametros de la consulta y el data set
                Generar_Reportes_Historial_Movimientos(Resumen_Predio_Historial_Movimientos, Nombre_Repote_Crystal, Nombre_Reporte, Formato);
            }
            else if (Cmb_Consultar_Tipo_Pago.SelectedValue == "TRASLADO")
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial_ID = Txt_Cuenta_Predial.Text.Trim();
                Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
                DataTable Dt_Resumen_Predio_Historial_Pagos = Rs_Consulta_Ope_Resumen_Predio.Consultar_Pagos_Traslado();
                DataTable Dt_Propietario = Asignar_Datos_Propietarios();
                Dt_Generales = Rs_Consulta_Ope_Resumen_Predio.Consultar_Imprimir_Resumen_Generales();
                Dt_Generales.TableName = "Dt_Generales";
                Dt_Propietario.TableName = "Dt_Propietario";
                Dt_Resumen_Predio_Historial_Pagos.TableName = "Dt_Historial_Pagos_Traslado";
                Resumen_Predio_Historial_Movimientos.Clear();
                Resumen_Predio_Historial_Movimientos.Tables.Clear();
                Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Generales.Copy());
                Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Propietario.Copy());
                Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Resumen_Predio_Historial_Pagos.Copy());
                Nombre_Reporte = "Reporte Historial Pagos";
                Nombre_Repote_Crystal = "Rpt_Pre_Historial_Pagos_Traslado.rpt";
                Formato = "PDF";
                //llama el metodo con los parametros de la consulta y el data set
                Generar_Reportes_Historial_Movimientos(Resumen_Predio_Historial_Movimientos, Nombre_Repote_Crystal, Nombre_Reporte, Formato);
            }
            else if (Cmb_Consultar_Tipo_Pago.SelectedValue == "PREDIAL")
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial_ID = Txt_Cuenta_Predial.Text.Trim();
                Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
                DataTable Dt_Resumen_Predio_Historial_Pagos = Rs_Consulta_Ope_Resumen_Predio.Consultar_Historial_Pagos();
                DataTable Dt_Propietario = Asignar_Datos_Propietarios();
                Dt_Generales = Rs_Consulta_Ope_Resumen_Predio.Consultar_Imprimir_Resumen_Generales();
                Dt_Generales.TableName = "Dt_Generales";
                Dt_Propietario.TableName = "Dt_Propietario";
                Dt_Resumen_Predio_Historial_Pagos.TableName = "Dt_Historial_Pagos";
                Resumen_Predio_Historial_Movimientos.Clear();
                Resumen_Predio_Historial_Movimientos.Tables.Clear();
                Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Generales.Copy());
                Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Propietario.Copy());
                Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Resumen_Predio_Historial_Pagos.Copy());
                Nombre_Reporte = "Reporte Historial Pagos";
                Nombre_Repote_Crystal = "Rpt_Pre_Resumen_Predio_Historial_Pagos.rpt";
                Formato = "PDF";
                //llama el metodo con los parametros de la consulta y el data set
                Generar_Reportes_Historial_Movimientos(Resumen_Predio_Historial_Movimientos, Nombre_Repote_Crystal, Nombre_Reporte, Formato);
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    /// *************************************************************************************
    /// NOMBRE:              Img_Btn_Imprimir_Cuenta_Predial_Click
    /// DESCRIPCIÓN:         Botron para mandar a imprimir el reporte en cristal
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          12/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    protected void Img_Btn_Imprimir_Cuenta_Predial_Click(object sender, ImageClickEventArgs e)
    {
        String Nombre_Reporte = String.Empty;
        String Nombre_Repote_Crystal = String.Empty;
        String Formato = String.Empty;
        DataTable Dt_Generales = new DataTable();
        int Propietarios = 0;
        int Impuestos = 0;
        //instacia la clase de negocio
        Cls_Ope_Pre_Resumen_Predio_Negocio Imprimir_Resumen_Predio_Estado_Cuenta = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
        Ds_Pre_Resumen_Predio_Generales Resumen_Predio_Historial_Movimientos = new Ds_Pre_Resumen_Predio_Generales();
        try
        {
            if (Txt_Cuenta_Predial.Text.Trim() != "")
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
                DataTable Dt_Propietarios = Asignar_Datos_Propietarios();
                DataTable Dt_Impuestos = Asignar_Datos_Impuestos();
                //DataTable Dt_Estado_Cuenta = Asignar_Datos_Estado_Cuenta();
                ////DataTable Dt_Resumen_Predio_Historial_Pagos = Rs_Consulta_Ope_Resumen_Predio.Consultar_Historial_Pagos();
                //DataTable Dt_Historial_Movimientos= Rs_Consulta_Ope_Resumen_Predio.Consultar_Historial_Movimientos();
                //DataTable Dt_Convenios= Rs_Consulta_Ope_Resumen_Predio.Consultar_Convenios();
                if (Dt_Propietarios.Rows.Count > 0)
                {
                    Propietarios = 1;
                }
                if (Dt_Impuestos.Rows.Count > 0)
                {
                    Impuestos = 1;
                }
                //if (Dt_Estado_Cuenta.Rows.Count  > 0)
                //{
                //    Estado_Cuenta = 1;
                //}
                //if (Dt_Resumen_Predio_Historial_Pagos.Rows.Count  > 0)
                //{
                //    Historial_Pagos = 1;
                //}
                //if (Dt_Historial_Movimientos.Rows.Count > 0)
                //{
                //    Historial_Movimientos = 1;
                //}
                //if (Dt_Convenios.Rows.Count > 0)
                //{
                //    Convenios = 1;
                //}
                DataTable Dt_Comparacion = new DataTable();
                DataRow Estado;
                Dt_Comparacion.Columns.Add("Propietarios");
                Dt_Comparacion.Columns.Add("Impuestos");
                //Dt_Comparacion.Columns.Add("Estado_Cuenta");
                //Dt_Comparacion.Columns.Add("Historial_Pagos");
                //Dt_Comparacion.Columns.Add("Historial_Movimientos");
                //Dt_Comparacion.Columns.Add("Convenios");

                Estado = Dt_Comparacion.NewRow();
                Estado["Propietarios"] = Propietarios;
                Estado["Impuestos"] = Impuestos;
                //Estado["Estado_Cuenta"] = Estado_Cuenta;
                //Estado["Historial_Pagos"] = Historial_Pagos;
                //Estado["Historial_Movimientos"] = Historial_Movimientos;
                //Estado["Convenios"] = Convenios;
                if (Dt_Comparacion.Rows.Count == 0)
                {
                    Dt_Comparacion.Rows.InsertAt(Estado, 0);

                }
                Dt_Comparacion.TableName = "Dt_Comparacion";
                Dt_Generales = Rs_Consulta_Ope_Resumen_Predio.Consultar_Imprimir_Resumen_Generales();
                Dt_Generales.Rows[0]["ESTATUS_RESUMEN"] = Lbl_Estatus.Text;
                Dt_Generales.TableName = "Dt_Generales";
                Dt_Propietarios.TableName = "Dt_Propietario";
                Dt_Impuestos.TableName = "Dt_Impuestos";
                //Dt_Estado_Cuenta.TableName = "Dt_Estado_Cuenta";
                //Dt_Historial_Movimientos.TableName = "Dt_Historial_Movimientos";
                //Dt_Convenios.TableName = "Dt_Convenios_Cuenta";
                //Dt_Resumen_Predio_Historial_Pagos.TableName = "Dt_Historial_Pagos";
                Resumen_Predio_Historial_Movimientos.Clear();
                Resumen_Predio_Historial_Movimientos.Tables.Clear();
                Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Generales.Copy());
                Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Propietarios.Copy());
                Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Impuestos.Copy());
                //Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Estado_Cuenta.Copy());
                //Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Historial_Movimientos.Copy());
                //Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Convenios.Copy());
                //Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Resumen_Predio_Historial_Pagos.Copy());
                Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Comparacion.Copy());
                Nombre_Reporte = "Reporte Resumen Predio";
                Nombre_Repote_Crystal = "Rpt_Pre_Resumen_Predio.rpt";
                Formato = "PDF";
                //llama el metodo con los parametros de la consulta y el data set
                Generar_Reportes_Historial_Movimientos(Resumen_Predio_Historial_Movimientos, Nombre_Repote_Crystal, Nombre_Reporte, Formato);
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    /// *************************************************************************************
    /// NOMBRE:              Img_Btn_Imprimir_Historial_Convenios_Click
    /// DESCRIPCIÓN:         Botron para mandar a imprimir el reporte en cristal
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          12/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    protected void Img_Btn_Imprimir_Historial_Convenios_Click(object sender, ImageClickEventArgs e)
    {
        String Nombre_Reporte = String.Empty;
        String Nombre_Repote_Crystal = String.Empty;
        String Formato = String.Empty;
        DataTable Dt_Generales = new DataTable();
        //instacia la clase de negocio
        Cls_Ope_Pre_Resumen_Predio_Negocio Imprimir_Resumen_Predio_Estado_Cuenta = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
        Ds_Pre_Resumen_Predio_Generales Resumen_Predio_Historial_Movimientos = new Ds_Pre_Resumen_Predio_Generales();
        try
        {
            Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            DataTable Dt_Resumen_Predio_Consultar_Convenios = Rs_Consulta_Ope_Resumen_Predio.Consultar_Convenios();
            Dt_Generales = Rs_Consulta_Ope_Resumen_Predio.Consultar_Imprimir_Resumen_Generales();
            Dt_Generales.TableName = "Dt_Generales";
            Dt_Resumen_Predio_Consultar_Convenios.TableName = "Dt_Convenios_Cuenta";
            Resumen_Predio_Historial_Movimientos.Clear();
            Resumen_Predio_Historial_Movimientos.Tables.Clear();
            Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Generales.Copy());
            Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Resumen_Predio_Consultar_Convenios.Copy());
            Nombre_Reporte = "Reporte Estado Cuenta";
            Nombre_Repote_Crystal = "Rpt_Pre_Resumen_Predio_Convenios_Cuenta.rpt";
            Formato = "PDF";
            //llama el metodo con los parametros de la consulta y el data set
            Generar_Reportes_Historial_Movimientos(Resumen_Predio_Historial_Movimientos, Nombre_Repote_Crystal, Nombre_Reporte, Formato);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    /// *************************************************************************************
    /// NOMBRE:              Generar_Reportes
    /// DESCRIPCIÓN:         Metodo para generar el reporte
    /// PARÁMETROS:          Dataset a imprimir
    ///                      Nombre del reporte de Crystal
    ///                      Nombre como se llamara el reporte
    ///                      Formato del reporte
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          26/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    private void Generar_Reportes_Historial_Movimientos(DataSet Ds_Recibo, String Nombre_Reporte_Crystal, String Nombre_Reporte, String Formato)
    {

        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";


        // Ruta donde se encuentra el reporte Crystal
        Ruta_Reporte_Crystal = "../Rpt/Predial/Resumen_Predio/" + Nombre_Reporte_Crystal;

        // Se crea el nombre del reporte
        String Nombre_Report = Nombre_Reporte + "_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("HH'-'mm'-'ss"));

        // Se da el nombre del reporte que se va generar
        if (Formato == "PDF")
            Nombre_Reporte_Generar = Nombre_Report + ".pdf";  // Es el nombre del reporte PDF que se va a generar
        else if (Formato == "Excel")
            Nombre_Reporte_Generar = Nombre_Report + ".xls";  // Es el nombre del repote en Excel que se va a generar

        Cls_Reportes Reportes = new Cls_Reportes();
        Reportes.Generar_Reporte(ref Ds_Recibo, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
        Mostrar_Reporte(Nombre_Reporte_Generar, Formato);
    }
    protected void Img_Btn_Quitar_Grid_Click(object sender, ImageClickEventArgs e)
    {
        Div_Detalles_Cuenta.Visible = false;
    }

    protected void Cmb_Consultar_Tipo_Pago_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Consultar_Pagos = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        String P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
        DataTable Dt_Pagos = new DataTable();
        Grid_Historial_Pagos.DataBind();
        Grid_Traslado.DataBind();
        Grid_Impuesto_Fraccionamiento.DataBind();
        Grid_Derechos_Supervision.DataBind();
        Grid_Constancias.DataBind();
        if (Cmb_Consultar_Tipo_Pago.SelectedValue == "IMPUESTOS FRACCIONAMIENTO")
        {
            Consultar_Pagos.P_Cuenta_Predial_ID = P_Cuenta_Predial;
            Dt_Pagos = Consultar_Pagos.Consultar_Pagos_Fraccionamientos();
            Grid_Impuesto_Fraccionamiento.DataSource = Dt_Pagos;
            Grid_Impuesto_Fraccionamiento.DataBind();
        }
        else if (Cmb_Consultar_Tipo_Pago.SelectedValue == "DERECHOS SUPERVISION")
        {
            Consultar_Pagos.P_Cuenta_Predial_ID = P_Cuenta_Predial;
            Dt_Pagos = Consultar_Pagos.Consultar_Pagos_Derechos_Supervision();
            Grid_Derechos_Supervision.DataSource = Dt_Pagos;
            Grid_Derechos_Supervision.DataBind();
        }
        else if (Cmb_Consultar_Tipo_Pago.SelectedValue == "CONSTANCIAS")
        {
            Consultar_Pagos.P_Cuenta_Predial_ID = P_Cuenta_Predial;
            Dt_Pagos = Consultar_Pagos.Consultar_Pagos_Constancias();
            Dt_Pagos.DefaultView.Sort = Ope_Caj_Pagos.Campo_Fecha + " DESC, " + Ope_Caj_Pagos.Campo_No_Recibo + " DESC";
            Grid_Constancias.DataSource = Dt_Pagos;
            Grid_Constancias.DataBind();
        }
        else if (Cmb_Consultar_Tipo_Pago.SelectedValue == "TRASLADO")
        {
            Consultar_Pagos.P_Cuenta_Predial_ID = P_Cuenta_Predial;
            Dt_Pagos = Consultar_Pagos.Consultar_Pagos_Traslado();
            Dt_Pagos.DefaultView.Sort = Ope_Caj_Pagos.Campo_No_Recibo + " DESC";
            Grid_Traslado.DataSource = Dt_Pagos;
            Grid_Traslado.DataBind();
        }
        else if (Cmb_Consultar_Tipo_Pago.SelectedValue == "PREDIAL")
        {
            Consultar_Pagos.P_Cuenta_Predial_ID = P_Cuenta_Predial;
            Dt_Pagos = Consultar_Pagos.Consultar_Historial_Pagos();
            Grid_Historial_Pagos.DataSource = Dt_Pagos;
            Grid_Historial_Pagos.DataBind();
        }
    }
}

