using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Recepcion_Pagos.Negocio;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Presidencia.Operacion_Predial_Convenios_Predial.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;
using Presidencia.Predial_Pae_Honorarios.Negocio;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Caja_Pagos.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Recepcion_Pagos_Predial : System.Web.UI.Page
{

    #region "Page Load"

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Evento al Cargar la Pagina.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 22 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            Lbl_Ecabezado_Mensaje.Text = "";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = false;
            Btn_Resumen_Predio.Visible = false;
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                if (Session["CUENTA_ADEUDO_PREDIAL"] != null)
                {
                    String Cuenta_Predial_Adeudo = Session["CUENTA_ADEUDO_PREDIAL"].ToString().Trim();
                    Session.Remove("CUENTA_ADEUDO_PREDIAL");
                    Cargar_Datos_Cuenta_Predial(Cuenta_Predial_Adeudo);
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

    #region "Metodos"

    #region "Generales"

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Generales
    ///DESCRIPCIÓN: Limpia los Generales de la Forma de Recepción de Pago.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 22 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Limpiar_Generales()
    {
        Hdf_Convenio.Value = "";
        Hdf_Cuenta_Predial_ID.Value = "";
        Txt_Cuenta_Predial.Text = "";
        Txt_Convenio.Text = "";
        Txt_Propietario.Text = "";
        Txt_Ubicacion.Text = "";
        Txt_Bimestre_Inicial.Text = "";
        Txt_Anio_Inicial.Text = "";
        Cmb_Bimestre_Final.SelectedIndex = 0;
        Cmb_Anio_Final.Items.Clear();
        Grid_Listado_Adeudos.DataSource = new DataTable();
        Grid_Listado_Adeudos.DataBind();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Calculos_Pago
    ///DESCRIPCIÓN: Limpia los caluclos Finales de la Forma de Recepción de Pago.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 22 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Limpiar_Calculos_Pago()
    {
        Hfd_No_Descuento_Recargos.Value = "";
        Txt_Periodo_Rezago.Text = "";
        Txt_Adeudo_Rezago.Text = "";
        Txt_Periodo_Actual.Text = "";
        Txt_Adeudo_Actual.Text = "";
        Txt_Total_Recargos_Ordinarios.Text = "";
        Txt_Honorarios.Text = "";
        Txt_Recargos_Moratorios.Text = "";
        Txt_Gastos_Ejecucion.Text = "";
        Txt_SubTotal.Text = "";
        Txt_Descuento_Corriente.Text = "";
        Txt_Descuento_Recargos_Ordinarios.Text = "";
        Txt_Descuento_Recargos_Moratorios.Text = "";
        Txt_Descuento_Honorarios.Text = "";
        Txt_Total.Text = "";
        Txt_Ajuste_Tarifario.Text = "";
        Txt_Total_Pagar.Text = "";
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Direccionar_Caja
    ///DESCRIPCIÓN: Direcciona a la caja para ejecutar el pago.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 11 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Direccionar_Caja()
    {
        try
        {
            DataTable Dt_Adeudos = Crear_Tabla_Adeudos();
            DataTable Dt_Totales = Crear_Tabla_Totales();
            Session["ADEUDO_PREDIAL_CAJA"] = Dt_Adeudos;
            Session["ADEUDO_PREDIAL_DETALLES"] = Dt_Totales;
            Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
            String Ruta_Completa = "../Predial/Frm_Ope_Caj_Pagos.aspx" + "?Referencia=" + Txt_Cuenta_Predial.Text.Trim();
            Response.Redirect(Ruta_Completa);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "A ocurrido una exceoción, favor de verificarlo";
            Lbl_Mensaje_Error.Text = "['" + Ex.Message + "']";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Crear_Tabla_Adeudos
    ///DESCRIPCIÓN: Crea la Tabla con el listado y los montos de los adeudos.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 13 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Crear_Tabla_Adeudos()
    {
        DataTable Dt_Adeudos_Predial = new DataTable();
        Dt_Adeudos_Predial.Columns.Add("NO_ADEUDO", Type.GetType("System.String"));
        Dt_Adeudos_Predial.Columns.Add("NO_CONVENIO", Type.GetType("System.String"));
        Dt_Adeudos_Predial.Columns.Add("NO_PAGO", Type.GetType("System.String"));
        Dt_Adeudos_Predial.Columns.Add("BIMESTRE", Type.GetType("System.Int32"));
        Dt_Adeudos_Predial.Columns.Add("ANIO", Type.GetType("System.Int32"));
        Dt_Adeudos_Predial.Columns.Add("MONTO", Type.GetType("System.Double"));
        if (Hdf_Convenio.Value.Trim().Length > 0)
        {
            String BS = Obtener_Pagos_Seleccionados();
            if (BS.Trim().Length > 0)
            {
                Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
                Negocio.P_No_Convenio = Hdf_Convenio.Value.Trim();
                Negocio.P_No_Pagos = BS;
                DataTable Dt_Tmp_Parcialidades = Negocio.Obtener_Biemestres_A_Pagar();
                if (Dt_Tmp_Parcialidades != null && Dt_Tmp_Parcialidades.Rows.Count > 0)
                {
                    for (Int32 Cnt_Pacialidades = 0; Cnt_Pacialidades < Dt_Tmp_Parcialidades.Rows.Count; Cnt_Pacialidades++)
                    {
                        DataRow Fila_Nueva = Dt_Adeudos_Predial.NewRow();
                        Fila_Nueva["NO_CONVENIO"] = Dt_Tmp_Parcialidades.Rows[Cnt_Pacialidades]["NO_CONVENIO"].ToString();
                        Fila_Nueva["NO_PAGO"] = Dt_Tmp_Parcialidades.Rows[Cnt_Pacialidades]["NO_PAGO"].ToString();
                        Fila_Nueva["BIMESTRE"] = (!String.IsNullOrEmpty(Dt_Tmp_Parcialidades.Rows[Cnt_Pacialidades]["BIMESTRE"].ToString())) ? Convert.ToInt32(Dt_Tmp_Parcialidades.Rows[Cnt_Pacialidades]["BIMESTRE"]) : 0;
                        Fila_Nueva["ANIO"] = (!String.IsNullOrEmpty(Dt_Tmp_Parcialidades.Rows[Cnt_Pacialidades]["ANIO"].ToString())) ? Convert.ToInt32(Dt_Tmp_Parcialidades.Rows[Cnt_Pacialidades]["ANIO"]) : 0;
                        Fila_Nueva["MONTO"] = (!String.IsNullOrEmpty(Dt_Tmp_Parcialidades.Rows[Cnt_Pacialidades]["MONTO"].ToString())) ? Convert.ToDouble(Dt_Tmp_Parcialidades.Rows[Cnt_Pacialidades]["MONTO"]) : 0.0;
                        Dt_Adeudos_Predial.Rows.Add(Fila_Nueva);
                    }
                }
                else
                {
                    for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos_Convenio.Rows.Count; Contador++)
                    {
                        if (Grid_Listado_Adeudos_Convenio.Rows[Contador].FindControl("Chk_Seleccion_Parcialidad") != null)
                        {
                            CheckBox Chk_Seleccion_Adeudo_Tmp = (CheckBox)Grid_Listado_Adeudos_Convenio.Rows[Contador].FindControl("Chk_Seleccion_Parcialidad");
                            if (Chk_Seleccion_Adeudo_Tmp.Checked)
                            {
                                DataRow Fila_Nueva = Dt_Adeudos_Predial.NewRow();
                                Fila_Nueva["NO_CONVENIO"] = Hdf_Convenio.Value.Trim();
                                Fila_Nueva["NO_PAGO"] = HttpUtility.HtmlDecode(Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[1].Text.Trim());
                                Dt_Adeudos_Predial.Rows.Add(Fila_Nueva);
                                //DataRow Fila_Tmp = Dt_Adeudos_Predial.NewRow();
                                //Fila_Tmp["NO_ADEUDO"] = HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[1].Text.Trim());
                                //Fila_Tmp["BIMESTRE"] = HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[4].Text.Trim());
                                //Fila_Tmp["ANIO"] = HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[3].Text.Trim());
                                //Fila_Tmp["MONTO"] = Convert.ToDouble(HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[7].Text.Trim().Replace("$", ""))) + Convert.ToDouble(HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[8].Text.Trim().Replace("$", "")));
                                //Dt_Adeudos_Predial.Rows.Add(Fila_Tmp);
                            }
                        }
                    }
                }
            }

        }
        else
        {
            for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos.Rows.Count; Contador++)
            {
                if (Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo") != null)
                {
                    CheckBox Chk_Seleccion_Adeudo_Tmp = (CheckBox)Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo");
                    if (Chk_Seleccion_Adeudo_Tmp.Checked)
                    {
                        DataRow Fila_Tmp = Dt_Adeudos_Predial.NewRow();
                        Fila_Tmp["NO_ADEUDO"] = HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[1].Text.Trim());
                        Fila_Tmp["BIMESTRE"] = HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[4].Text.Trim());
                        Fila_Tmp["ANIO"] = HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[3].Text.Trim());
                        Fila_Tmp["MONTO"] = Convert.ToDouble(HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[7].Text.Trim().Replace("$", ""))) + Convert.ToDouble(HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[8].Text.Trim().Replace("$", "")));
                        Dt_Adeudos_Predial.Rows.Add(Fila_Tmp);
                    }
                }
            }
        }

        return Dt_Adeudos_Predial;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Crear_Tabla_Totales
    ///DESCRIPCIÓN: Crea la Tabla con el listado y los montos de los totales.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 13 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Crear_Tabla_Totales()
    {
        DataTable Dt_Adeudos_Predial = new DataTable();
        Dt_Adeudos_Predial.Columns.Add("CONCEPTO", Type.GetType("System.String"));
        Dt_Adeudos_Predial.Columns.Add("MONTO", Type.GetType("System.Double"));
        Dt_Adeudos_Predial.Columns.Add("REFERENCIA", Type.GetType("System.String"));
        //Agrega la Fila de Importe Corriente
        DataRow Fila_Corriente = Dt_Adeudos_Predial.NewRow();
        Fila_Corriente["CONCEPTO"] = "CORRIENTE";
        Fila_Corriente["MONTO"] = Convert.ToDouble(Txt_Adeudo_Actual.Text.Replace("$", "").Trim());
        Dt_Adeudos_Predial.Rows.Add(Fila_Corriente);
        //Agrega la Fila de Importe REzago
        DataRow Fila_Rezago = Dt_Adeudos_Predial.NewRow();
        Fila_Rezago["CONCEPTO"] = "REZAGO";
        Fila_Rezago["MONTO"] = Convert.ToDouble(Txt_Adeudo_Rezago.Text.Replace("$", "").Trim());
        Dt_Adeudos_Predial.Rows.Add(Fila_Rezago);
        //Agrega la Fila de Importe Honorarios
        DataRow Fila_Honorarios = Dt_Adeudos_Predial.NewRow();
        Fila_Honorarios["CONCEPTO"] = "HONORARIOS";
        Fila_Honorarios["MONTO"] = Convert.ToDouble(Txt_Honorarios.Text.Replace("$", "").Trim());
        Dt_Adeudos_Predial.Rows.Add(Fila_Honorarios);

        //Agrega la Fila de Recargos
        DataRow Fila_Recargos = Dt_Adeudos_Predial.NewRow();
        Fila_Recargos["CONCEPTO"] = "RECARGOS";
        Fila_Recargos["MONTO"] = Convert.ToDouble(Txt_Total_Recargos_Ordinarios.Text.Replace("$", "").Trim());
        Dt_Adeudos_Predial.Rows.Add(Fila_Recargos);

        //Agrega la Fila de Recargos Moratorios
        DataRow Fila_Recargos_Moratorios = Dt_Adeudos_Predial.NewRow();
        Fila_Recargos_Moratorios["CONCEPTO"] = "MORATORIOS";
        Fila_Recargos_Moratorios["MONTO"] = Convert.ToDouble(Txt_Recargos_Moratorios.Text.Replace("$", "").Trim());
        Dt_Adeudos_Predial.Rows.Add(Fila_Recargos_Moratorios);

        //Agrega la Fila de Descuento Corriente
        DataRow Fila_Descuento_Corriente = Dt_Adeudos_Predial.NewRow();
        Fila_Descuento_Corriente["CONCEPTO"] = "DESCUENTOS_CORRIENTES";
        Fila_Descuento_Corriente["MONTO"] = Convert.ToDouble(Txt_Descuento_Corriente.Text.Replace("$", "").Trim());
        Dt_Adeudos_Predial.Rows.Add(Fila_Descuento_Corriente);

        //Agrega la Fila de Descuento Honorarios
        DataRow Fila_Descuento_Honorarios = Dt_Adeudos_Predial.NewRow();
        Fila_Descuento_Honorarios["CONCEPTO"] = "DESCUENTOS_HONORARIOS";
        Fila_Descuento_Honorarios["MONTO"] = Convert.ToDouble(Txt_Descuento_Honorarios.Text.Replace("$", "").Trim());
        Fila_Descuento_Honorarios["REFERENCIA"] = Hfd_No_Descuento_Recargos.Value;
        Dt_Adeudos_Predial.Rows.Add(Fila_Descuento_Honorarios);

        //Agrega la Fila de Descuento Recargos
        DataRow Fila_Descuento_Recargos = Dt_Adeudos_Predial.NewRow();
        Fila_Descuento_Recargos["CONCEPTO"] = "DESCUENTOS_RECARGOS";
        Fila_Descuento_Recargos["MONTO"] = Convert.ToDouble(Txt_Descuento_Recargos_Ordinarios.Text.Replace("$", "").Trim());
        Fila_Descuento_Recargos["REFERENCIA"] = Hfd_No_Descuento_Recargos.Value;
        Dt_Adeudos_Predial.Rows.Add(Fila_Descuento_Recargos);

        //Agrega la Fila de Descuento Moratorios
        DataRow Fila_Descuento_Recargos_Moratorios = Dt_Adeudos_Predial.NewRow();
        Fila_Descuento_Recargos_Moratorios["CONCEPTO"] = "DESCUENTOS_MORATORIOS";
        Fila_Descuento_Recargos_Moratorios["MONTO"] = Convert.ToDouble(Txt_Descuento_Recargos_Moratorios.Text.Replace("$", "").Trim());
        Fila_Descuento_Recargos_Moratorios["REFERENCIA"] = Hfd_No_Descuento_Recargos.Value;
        Dt_Adeudos_Predial.Rows.Add(Fila_Descuento_Recargos_Moratorios);
        return Dt_Adeudos_Predial;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Visibilidad_Controles
    ///DESCRIPCIÓN: Carga la Visibilidad de los controles.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Visibilidad_Controles(String Tipo_Visibilidad)
    {
        if (Tipo_Visibilidad.Trim().Equals("PAGO_NORMAL"))
        {
            Txt_Convenio.Text = "SIN CONVENIO";
            Div_Listado_Adeudos_Predial.Visible = true;
            Div_Listado_Adeudos_Convenio.Visible = false;
        }
        else if (Tipo_Visibilidad.Trim().Equals("PAGO_CONVENIO"))
        {
            Div_Listado_Adeudos_Predial.Visible = false;
            Div_Listado_Adeudos_Convenio.Visible = true;
            Txt_Bimestre_Inicial.Text = "-";
            Txt_Anio_Inicial.Text = "-";
            Cmb_Bimestre_Final.Items.Clear();
            Cmb_Bimestre_Final.Items.Insert(0, new ListItem("-", ""));
            Cmb_Bimestre_Final.Enabled = false;
            Cmb_Anio_Final.Items.Clear();
            Cmb_Anio_Final.Items.Insert(0, new ListItem("-", ""));
            Cmb_Anio_Final.Enabled = false;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Pagos_Seleccionados
    ///DESCRIPCIÓN: Obtiene los pagos seleccionados.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 29 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private String Obtener_Pagos_Seleccionados()
    {
        String No_Pagos = "";
        Boolean Poner_Coma = false;
        for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos_Convenio.Rows.Count; Contador++)
        {
            if (Grid_Listado_Adeudos_Convenio.Rows[Contador].FindControl("Chk_Seleccion_Parcialidad") != null)
            {
                CheckBox Chk_Seleccion_Adeudo_Tmp = (CheckBox)Grid_Listado_Adeudos_Convenio.Rows[Contador].FindControl("Chk_Seleccion_Parcialidad");
                if (Chk_Seleccion_Adeudo_Tmp.Checked)
                {
                    if (Poner_Coma)
                    {
                        No_Pagos = No_Pagos + ", ";
                    }
                    No_Pagos = No_Pagos + Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[1].Text.Trim();
                    Poner_Coma = true;
                }
            }
        }
        return No_Pagos;
    }

    #endregion

    #region "Calculos para el Pago"

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Realizar_Calculos_Pago
    ///DESCRIPCIÓN: Realiza los Calculos del Pago.
    ///PARAMETROS: Existe_Convenio, si la cuenta tiene un convenio
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Realizar_Calculos_Pago(Boolean Existe_Convenio)
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataTable Dt_Honorarios;
        //Declaración de las Variables para la realización de los Calculos.
        Double Corriente = 0;
        Double Rezagos = 0;
        Double Recargos_Ordinarios = 0;
        Double Recargos_Moratorios = 0;
        Double Honorarios = 0;
        Double Gastos_Ejecucion = 0;
        Double Subtotal = 0;
        Double Porcentaje_Descuento_Corriente = 0;
        Double Descuento_Corriente = 0;
        Double Porcentaje_Descuento_Recargos_Ordinarios = 0;
        Double Descuento_Recargos_Ordinarios = 0;
        Double Porcentaje_Descuento_Recargos_Moratorios = 0;
        Double Descuento_Recargos_Moratorios = 0;
        Double Porcentaje_Descuento_Honorarios = 0;
        Double Descuento_Honorarios = 0;
        Double Total = 0;
        Double Ajuste_Tarifario = 0;
        Double Total_Pagar = 0;
        Double Decimales = 0;
        Boolean Incumplido = false;

        Cls_Ope_Pre_Pae_Honorarios_Negocio Consulta_Honorarios = new Cls_Ope_Pre_Pae_Honorarios_Negocio();
        Consulta_Honorarios.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
        Dt_Honorarios = Consulta_Honorarios.Consultar_Total_Honorarios();
        Btn_Ejecutar_Pago.Enabled = true;
        //Se calcula el Adeudo y el Corriente en caso de no haber convenio
        if (Existe_Convenio)
        {
            for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos_Convenio.Rows.Count; Contador++)
            {

                if (Grid_Listado_Adeudos_Convenio.Rows[Contador].FindControl("Chk_Seleccion_Parcialidad") != null)
                {
                    CheckBox Chk_Seleccion_Adeudo = (CheckBox)Grid_Listado_Adeudos_Convenio.Rows[Contador].FindControl("Chk_Seleccion_Parcialidad");
                    if (Chk_Seleccion_Adeudo.Checked)
                    {
                        Corriente = Corriente + Convert.ToDouble(Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[7].Text.Trim().Replace("$", ""));
                        Rezagos = Rezagos + Convert.ToDouble(Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[8].Text.Trim().Replace("$", ""));
                        Recargos_Ordinarios = Recargos_Ordinarios + Convert.ToDouble(Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[9].Text.Trim().Replace("$", ""));
                        Recargos_Moratorios = Recargos_Moratorios + Convert.ToDouble(Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[10].Text.Trim().Replace("$", ""));
                        Honorarios = Honorarios + Convert.ToDouble(Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[11].Text.Trim().Replace("$", ""));
                    }
                }
            }
            DataTable Dt_Convenio = Buscar_Convenio_Cuenta_Predial();
            if (Dt_Convenio != null && Dt_Convenio.Rows.Count > 0)
            {
                Incumplido = Validar_Convenio_No_Imcumplido(Dt_Convenio);
            }
            Obtener_Recargos_Moratorios(Incumplido);
            Recargos_Moratorios += Convert.ToDouble(Txt_Recargos_Moratorios.Text);
        }
        else
        {
            for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos.Rows.Count; Contador++)
            {
                if (Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo") != null)
                {
                    CheckBox Chk_Seleccion_Adeudo = (CheckBox)Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo");
                    if (Chk_Seleccion_Adeudo.Checked)
                    {
                        Corriente = Corriente + Convert.ToDouble(Grid_Listado_Adeudos.Rows[Contador].Cells[7].Text.Trim().Replace("$", ""));
                        Rezagos = Rezagos + Convert.ToDouble(Grid_Listado_Adeudos.Rows[Contador].Cells[8].Text.Trim().Replace("$", ""));
                        Recargos_Ordinarios = Recargos_Ordinarios + Convert.ToDouble(Grid_Listado_Adeudos.Rows[Contador].Cells[9].Text.Trim().Replace("$", ""));
                        Recargos_Moratorios = Recargos_Moratorios + Convert.ToDouble(Grid_Listado_Adeudos.Rows[Contador].Cells[10].Text.Trim().Replace("$", ""));
                        Honorarios = Honorarios + Convert.ToDouble(Grid_Listado_Adeudos.Rows[Contador].Cells[11].Text.Trim().Replace("$", ""));
                    }
                }
            }

            DataTable Dt_Descuento_Pronto_Pago = Resumen_Predio.Consultar_Descuentos_Pronto_Pago();
            if (Dt_Descuento_Pronto_Pago.Rows.Count > 0)
            {
                if (Dt_Descuento_Pronto_Pago.Rows[0][String.Format("{0:MMMMMMMMMMMMMM}", DateTime.Now)].ToString().Trim() != "0")
                {
                    Porcentaje_Descuento_Corriente = Convert.ToDouble(Dt_Descuento_Pronto_Pago.Rows[0][String.Format("{0:MMMMMMMMMMMMMM}", DateTime.Now)].ToString().Trim()) / 100;
                }
                else
                {
                    Porcentaje_Descuento_Corriente = 0;
                }
            }
            else
            {
                Porcentaje_Descuento_Corriente = 0;
            }

            Resumen_Predio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            DataTable Dt_Descuentos_Recargos = Resumen_Predio.Consultar_Descuentos_Recargos();
            if (Dt_Descuentos_Recargos.Rows.Count > 0)
            {
                Hfd_No_Descuento_Recargos.Value = Dt_Descuentos_Recargos.Rows[0]["No_Descuento_Predial"].ToString();
                Double Periodo_Descuento = Convert.ToDouble(Dt_Descuentos_Recargos.Rows[0]["Hasta_Periodo_Anio"].ToString() + Convert.ToInt32(Dt_Descuentos_Recargos.Rows[0]["Hasta_Periodo_Bimestre"].ToString()).ToString());
                Double Periodo_Pago = 0;
                if (Cmb_Anio_Final.Items.Count > 0)
                {
                    Periodo_Pago = Convert.ToDouble(Cmb_Anio_Final.SelectedItem.Text + Convert.ToInt32(Cmb_Bimestre_Final.SelectedItem.Text).ToString());
                }
                else
                {
                    for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos.Rows.Count; Contador++)
                    {
                        if (Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo") != null)
                        {
                            CheckBox Chk_Seleccion_Adeudo = (CheckBox)Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo");
                            if (Chk_Seleccion_Adeudo.Checked)
                            {
                                Periodo_Pago = Convert.ToDouble(Grid_Listado_Adeudos.Rows[Contador].Cells[3].Text + Convert.ToInt32(Grid_Listado_Adeudos.Rows[Contador].Cells[4].Text));
                            }
                        }
                    }
                    if (Periodo_Pago == 0)
                    {
                        Periodo_Pago = Convert.ToDouble(DateTime.Now.Year + Convert.ToInt32("6").ToString());
                    }
                    else
                    {
                        if (Obtener_Dato_Consulta(Hdf_Cuenta_Predial_ID.Value) == "SI")
                        {
                            Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio Adeudos = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
                            DataTable Dt_Adeudos = Adeudos.Obtener_Dato_Consulta(Hdf_Cuenta_Predial_ID.Value, Convert.ToInt32(Periodo_Pago.ToString().Substring(0, 4)));
                            int Periodo = Convert.ToInt32(Periodo_Pago.ToString().Substring(4));
                            Periodo++;
                            for (int i = Periodo; i < 7; i++)
                            {
                                if (Convert.ToDouble(Dt_Adeudos.Rows[0]["PAGO_" + i].ToString()) > 0)
                                {
                                    Periodo = Convert.ToInt32(Periodo_Pago.ToString().Substring(4));
                                    break;
                                }
                                else
                                {
                                    Periodo = i;
                                }
                            }
                            if (Periodo >= 7)
                            {
                                Periodo = 6;
                            }
                            Periodo_Pago = Convert.ToDouble(Periodo_Pago.ToString().Substring(0, 4) + Periodo);
                        }
                        else
                        {
                            String Valores = "";
                            Valores = Obtener_Dato_Consulta_Valor(Hdf_Cuenta_Predial_ID.Value);
                            if (Valores != "")
                            {
                                String[] valores_cuota_minima = Valores.Split('-');
                                if (valores_cuota_minima[1].Trim() == "")
                                {
                                    valores_cuota_minima[1] = Obtener_Dato_Consulta_Cuota(Periodo_Pago.ToString().Substring(0, 4));
                                }
                                if (Convert.ToDouble(valores_cuota_minima[0]) < Convert.ToDouble(valores_cuota_minima[1]))
                                {
                                    Periodo_Pago = Convert.ToDouble(DateTime.Now.Year + Convert.ToInt32("6").ToString());
                                }
                                else
                                {
                                    Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio Adeudos = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
                                    DataTable Dt_Adeudos = Adeudos.Obtener_Dato_Consulta(Hdf_Cuenta_Predial_ID.Value, Convert.ToInt32(Periodo_Pago.ToString().Substring(0, 4)));
                                    int Periodo = Convert.ToInt32(Periodo_Pago.ToString().Substring(4));
                                    Periodo++;
                                    for (int i = Periodo; i < 7; i++)
                                    {
                                        if (Convert.ToDouble(Dt_Adeudos.Rows[0]["PAGO_" + i].ToString()) > 0)
                                        {
                                            Periodo = Convert.ToInt32(Periodo_Pago.ToString().Substring(4));
                                            break;
                                        }
                                        else
                                        {
                                            Periodo = i;
                                        }
                                    }
                                    if (Periodo >= 7)
                                    {
                                        Periodo = 6;
                                    }
                                    Periodo_Pago = Convert.ToDouble(Periodo_Pago.ToString().Substring(0, 4) + Periodo);
                                }
                            }
                        }
                    }
                }
                if (Periodo_Descuento == Periodo_Pago)
                {
                    if (!string.IsNullOrEmpty(Dt_Descuentos_Recargos.Rows[0]["Porcentaje_Recargo"].ToString().Trim()))
                    {
                        Porcentaje_Descuento_Recargos_Ordinarios = Convert.ToDouble(Dt_Descuentos_Recargos.Rows[0]["Porcentaje_Recargo"]) / 100;
                    }
                    else
                    {
                        Porcentaje_Descuento_Recargos_Ordinarios = 0;
                    }
                    if (!string.IsNullOrEmpty(Dt_Descuentos_Recargos.Rows[0]["Porcentaje_Recargo_Moratorio"].ToString().Trim()))
                    {
                        Porcentaje_Descuento_Recargos_Moratorios = Convert.ToDouble(Dt_Descuentos_Recargos.Rows[0]["Porcentaje_Recargo_Moratorio"]) / 100;
                    }
                    else
                    {
                        Porcentaje_Descuento_Recargos_Moratorios = 0;
                    }
                }
                else
                {
                    Porcentaje_Descuento_Recargos_Ordinarios = 0;
                    Porcentaje_Descuento_Recargos_Moratorios = 0;
                    Btn_Ejecutar_Pago.Enabled = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('EXISTE UN DESCUENTO QUE NO COINCIDE CON EL PERIODO A PAGAR, FAVOR DE VERIFICARLO');", true);
                    Lbl_Ecabezado_Mensaje.Text = "Verificar el periodo del descuento aplicado.";
                    Lbl_Mensaje_Error.Text = "No es posible pagar el adeudo ya que tiene un descuento pero no coincide el periodo del mismo con el del pago.";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                Porcentaje_Descuento_Recargos_Ordinarios = 0;
                Porcentaje_Descuento_Recargos_Moratorios = 0;
            }

            //Se calcula el Valor de los Descuentos
            if (Dt_Honorarios.Rows.Count > 0)
            {
                Honorarios = Convert.ToDouble(Dt_Honorarios.Rows[0]["TOTAL_HONORARIOS"].ToString());
            }
            Descuento_Corriente = (Corriente * Porcentaje_Descuento_Corriente);
            Descuento_Recargos_Ordinarios = (Recargos_Ordinarios * Porcentaje_Descuento_Recargos_Ordinarios);
            Descuento_Recargos_Moratorios = (Recargos_Moratorios * Porcentaje_Descuento_Recargos_Moratorios);
            Descuento_Honorarios = (Honorarios * Porcentaje_Descuento_Honorarios);
        }

        //Se calcula el Subtotal
        Subtotal = Rezagos + Corriente + Honorarios + Gastos_Ejecucion + Recargos_Ordinarios + Recargos_Moratorios;

        //Se calcula el Total Neto
        Total = Subtotal - Descuento_Corriente;
        Total = Total - Descuento_Recargos_Ordinarios;
        Total = Total - Descuento_Recargos_Moratorios;
        Total = Total - Descuento_Honorarios;

        Total = Convert.ToDouble(String.Format("{0:#0.00}", Total));
        //Se obtiene el Ajuste Tarifario y Total a Pagar
        Decimales = Convert.ToDouble(String.Format("{0:#0.00}", Total - Math.Truncate(Total)));
        if (Decimales <= 0.5)
        {
            Total_Pagar = Math.Floor(Total);
        }
        else
        {
            Total_Pagar = Math.Ceiling(Total);
        }
        Ajuste_Tarifario = Total - Total_Pagar;

        //Se muestran los resultados
        Txt_Adeudo_Rezago.Text = "$ " + Rezagos.ToString("#,####,###0.00");
        Txt_Adeudo_Actual.Text = "$ " + Corriente.ToString("#,####,###0.00");
        Txt_Total_Recargos_Ordinarios.Text = "$ " + Recargos_Ordinarios.ToString("#,####,###0.00");
        Txt_Recargos_Moratorios.Text = "$ " + Recargos_Moratorios.ToString("#,####,###0.00");
        Txt_Honorarios.Text = "$ " + Honorarios.ToString("#,####,###0.00");
        Txt_Gastos_Ejecucion.Text = "$ " + Gastos_Ejecucion.ToString("#,####,###0.00");
        Txt_SubTotal.Text = "$ " + Subtotal.ToString("#,####,###0.00");
        Txt_Descuento_Corriente.Text = "$ " + Descuento_Corriente.ToString("#,####,###0.00");
        Txt_Descuento_Recargos_Ordinarios.Text = "$ " + Descuento_Recargos_Ordinarios.ToString("#,####,###0.00");
        Txt_Descuento_Recargos_Moratorios.Text = "$ " + Descuento_Recargos_Moratorios.ToString("#,####,###0.00");
        Txt_Descuento_Honorarios.Text = "$ " + Descuento_Honorarios.ToString("#,####,###0.00");
        Txt_Total.Text = "$ " + Total.ToString("#,####,###0.00");
        Txt_Ajuste_Tarifario.Text = "$ " + (Ajuste_Tarifario * (-1)).ToString("#,####,###0.00");
        Txt_Total_Pagar.Text = "$ " + Total_Pagar.ToString("#,####,###0.00");
        if (Existe_Convenio)
        {
            Cargar_Periodos_Parcialidades_Pagar();
        }
        else
        {
            Cargar_Periodos_Pagar();
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Periodos_Pagar
    ///DESCRIPCIÓN: Obtiene los Periodos tanto de Rezsago como Actual y los muestra en
    ///             los Campos.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 16 Octubre 2011 [Domingo ¬¬]
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Periodos_Pagar()
    {
        Txt_Periodo_Actual.Text = "";
        Txt_Periodo_Rezago.Text = "";
        if (Grid_Listado_Adeudos.Rows.Count > 0)
        {
            String Bimestre_Rezago_Inicial = "";
            String Anio_Rezago_Inicial = "";
            String Bimestre_Rezago_Final = "";
            String Anio_Rezago_Final = "";
            Boolean Capturo_Inicial_Rezago = false;
            String Bimestre_Corriente_Inicial = "";
            String Anio_Corriente_Inicial = "";
            String Bimestre_Corriente_Final = "";
            String Anio_Corriente_Final = "";
            Boolean Capturo_Inicial_Corriente = false;
            for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos.Rows.Count; Contador++)
            {
                if (Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo") != null)
                {
                    CheckBox Chk_Seleccion_Adeudo = (CheckBox)(Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo"));
                    if (Chk_Seleccion_Adeudo.Checked)
                    {
                        if (Grid_Listado_Adeudos.Rows[Contador].Cells[2].Text.ToString().Trim().Equals("REZAGO"))
                        {
                            if (!Capturo_Inicial_Rezago)
                            {
                                Bimestre_Rezago_Inicial = Grid_Listado_Adeudos.Rows[Contador].Cells[4].Text.ToString().Trim();
                                Anio_Rezago_Inicial = Grid_Listado_Adeudos.Rows[Contador].Cells[3].Text.ToString().Trim();
                                Capturo_Inicial_Rezago = true;
                            }
                            Bimestre_Rezago_Final = Grid_Listado_Adeudos.Rows[Contador].Cells[4].Text.ToString().Trim();
                            Anio_Rezago_Final = Grid_Listado_Adeudos.Rows[Contador].Cells[3].Text.ToString().Trim();
                        }
                        else if (Grid_Listado_Adeudos.Rows[Contador].Cells[2].Text.ToString().Trim().Equals("CORRIENTE"))
                        {
                            if (!Capturo_Inicial_Corriente)
                            {
                                Bimestre_Corriente_Inicial = Grid_Listado_Adeudos.Rows[Contador].Cells[4].Text.ToString().Trim();
                                Anio_Corriente_Inicial = Grid_Listado_Adeudos.Rows[Contador].Cells[3].Text.ToString().Trim();
                                Capturo_Inicial_Corriente = true;
                            }
                            Bimestre_Corriente_Final = Grid_Listado_Adeudos.Rows[Contador].Cells[4].Text.ToString().Trim();
                            Anio_Corriente_Final = Grid_Listado_Adeudos.Rows[Contador].Cells[3].Text.ToString().Trim();
                        }
                    }
                }
            }
            if (Bimestre_Rezago_Inicial.Trim().Length > 0)
            {
                Txt_Periodo_Rezago.Text = Bimestre_Rezago_Inicial + "/" + Anio_Rezago_Inicial + " - " + Bimestre_Rezago_Final + "/" + Anio_Rezago_Final;
            }
            if (Bimestre_Corriente_Inicial.Trim().Length > 0)
            {
                if (Hfd_Cuota_Fija.Value == "")
                {
                    Txt_Periodo_Actual.Text = Bimestre_Corriente_Inicial + "/" + Anio_Corriente_Inicial + " - " + Bimestre_Corriente_Final + "/" + Anio_Corriente_Final;
                }
                else
                {
                    Txt_Periodo_Actual.Text = Bimestre_Corriente_Inicial + "/" + Anio_Corriente_Inicial + " - 6/" + Anio_Corriente_Final;
                }
            }
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Periodos_Parcialidades_Pagar
    ///DESCRIPCIÓN: Obtiene los Periodos tanto de Rezsago como Actual y los muestra en
    ///             los Campos.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 28 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Periodos_Parcialidades_Pagar()
    {
        Txt_Periodo_Actual.Text = "";
        Txt_Periodo_Rezago.Text = "";
        if (Grid_Listado_Adeudos_Convenio.Rows.Count > 0)
        {
            String Bimestre_Rezago_Inicial = "";
            String Anio_Rezago_Inicial = "";
            String Bimestre_Rezago_Final = "";
            String Anio_Rezago_Final = "";
            Boolean Capturo_Inicial_Rezago = false;
            String Bimestre_Corriente_Inicial = "";
            String Anio_Corriente_Inicial = "";
            String Bimestre_Corriente_Final = "";
            String Anio_Corriente_Final = "";
            Boolean Capturo_Inicial_Corriente = false;
            for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos_Convenio.Rows.Count; Contador++)
            {
                if (Grid_Listado_Adeudos_Convenio.Rows[Contador].FindControl("Chk_Seleccion_Parcialidad") != null)
                {
                    CheckBox Chk_Seleccion_Adeudo_Tmp = (CheckBox)Grid_Listado_Adeudos_Convenio.Rows[Contador].FindControl("Chk_Seleccion_Parcialidad");
                    if (Chk_Seleccion_Adeudo_Tmp.Checked)
                    {
                        if (Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[4].Text.Trim().Length > 0)
                        {
                            Int32 Anio_Registro = (Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[4].Text.Trim().Length > 0) ? Convert.ToInt32(Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[4].Text.Trim()) : 0;
                            if (Anio_Registro < DateTime.Today.Year)
                            {
                                if (!Capturo_Inicial_Rezago)
                                {
                                    Bimestre_Rezago_Inicial = HttpUtility.HtmlDecode(Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[5].Text.Trim().Substring(0, 1));
                                    Anio_Rezago_Inicial = Anio_Registro.ToString();
                                    Capturo_Inicial_Rezago = true;
                                }
                                Bimestre_Rezago_Final = HttpUtility.HtmlDecode(Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[5].Text.Trim().Substring((Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[5].Text.Trim().Length - 1), 1));
                                Anio_Rezago_Final = Anio_Registro.ToString();
                            }
                            else
                            {
                                if (!Capturo_Inicial_Corriente)
                                {
                                    Bimestre_Corriente_Inicial = HttpUtility.HtmlDecode(Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[5].Text.Trim().Substring(0, 1));
                                    Anio_Corriente_Inicial = Anio_Registro.ToString();
                                    Capturo_Inicial_Corriente = true;
                                }
                                Bimestre_Corriente_Final = HttpUtility.HtmlDecode(Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[5].Text.Trim().Substring((Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[5].Text.Trim().Length - 1), 1));
                                Anio_Corriente_Final = Anio_Registro.ToString();
                            }
                        }
                    }
                }
            }

            if (Bimestre_Rezago_Inicial.Trim().Length > 0)
            {
                Txt_Periodo_Rezago.Text = Bimestre_Rezago_Inicial + "/" + Anio_Rezago_Inicial + " - " + Bimestre_Rezago_Final + "/" + Anio_Rezago_Final;
            }
            if (Bimestre_Corriente_Inicial.Trim().Length > 0)
            {
                Txt_Periodo_Actual.Text = Bimestre_Corriente_Inicial + "/" + Anio_Corriente_Inicial + " - " + Bimestre_Corriente_Final + "/" + Anio_Corriente_Final;
            }
        }
    }

    #endregion

    #region "Interaccion con Clases de Negocio [Base de Datos]"

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Anios
    ///DESCRIPCIÓN: Llena el Combo de los Años en los que existe un Adeudo.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 23 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Llenar_Combo_Anios(DataTable Dt_Anios)
    {
        Dt_Anios = Dt_Anios.DefaultView.ToTable(true, "ANIO");
        Cmb_Anio_Final.DataSource = Dt_Anios;
        Cmb_Anio_Final.DataTextField = "ANIO";
        Cmb_Anio_Final.DataValueField = "ANIO";
        Cmb_Anio_Final.DataBind();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Cuenta_Predial
    ///DESCRIPCIÓN: Carga los Campos con los Datos de la Cuenta predial Seleccionada.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 22 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Datos_Cuenta_Predial(String P_Cuenta_Predial)
    {
        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
        RP_Negocio.P_Cuenta_Predial = P_Cuenta_Predial;
        DataTable Dt_Datos = RP_Negocio.Consultar_Cuentas_Predial();
        if (Dt_Datos.Rows.Count > 0)
        {
            Hdf_Cuenta_Predial_ID.Value = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim())) ? Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim() : "";
            Txt_Cuenta_Predial.Text = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["CUENTA_PREDIAL"].ToString().Trim())) ? Dt_Datos.Rows[0]["CUENTA_PREDIAL"].ToString().Trim() : "";
            Hfd_Tipo_Predio.Value = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["TIPO_PREDIO"].ToString().Trim())) ? Dt_Datos.Rows[0]["TIPO_PREDIO"].ToString().Trim() : "";
            Txt_Propietario.Text = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["PROPIETARIO"].ToString().Trim())) ? Dt_Datos.Rows[0]["PROPIETARIO"].ToString().Trim() : "---------------------------";
            String Ubicacion = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["UBICACION"].ToString().Trim())) ? Dt_Datos.Rows[0]["UBICACION"].ToString().Trim() : "";
            Txt_Ubicacion.Text = (!Ubicacion.Trim().Equals("S/N COL.")) ? Ubicacion : "---------------------------";
            Hfd_Cuota_Fija.Value = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["CUOTA_FIJA"].ToString().Trim())) ? Dt_Datos.Rows[0]["CUOTA_FIJA"].ToString().Trim() : "";
            //Se verifica si hay o no convenios
            DataTable Dt_Convenio = Buscar_Convenio_Cuenta_Predial();
            if (Dt_Convenio != null && Dt_Convenio.Rows.Count > 0)
            {
                Visibilidad_Controles("PAGO_CONVENIO");
                Txt_Convenio.Text = Dt_Convenio.Rows[0]["NO_CONVENIO"].ToString();
                Hdf_Convenio.Value = Dt_Convenio.Rows[0]["NO_CONVENIO"].ToString();
                Hfd_No_Descuento_Recargos.Value = Dt_Convenio.Rows[0]["NO_DESCUENTO"].ToString();
                Llenar_Grid_Convenio_Adeudos(Dt_Convenio);
            }
            else
            {
                Visibilidad_Controles("PAGO_NORMAL");
                if (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim()))
                {
                    Llenar_Grid_Adeudos(Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim(), false);
                }
                RP_Negocio.P_Cuenta_Predial = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim())) ? Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim() : "";
                DataTable Dt_Adeudos = RP_Negocio.Consultar_Adeudos_Cuentas_Predial();
                Llenar_Combo_Anios(Dt_Adeudos);
                Cmb_Bimestre_Final.SelectedIndex = (Cmb_Bimestre_Final.Items.Count - 1);
                Cmb_Anio_Final.SelectedIndex = (Cmb_Anio_Final.Items.Count - 1);
            }
        }
        else
        {
            Lbl_Ecabezado_Mensaje.Text = "No se han encontrado datos para la Cuenta Predial.";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
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
        Grid_Listado_Adeudos.Columns[1].Visible = true;
        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
        RP_Negocio.P_Cuenta_Predial = P_Cuenta_Predial;
        if (Tomar_Filtrado)
        {
            RP_Negocio.P_Anio_Filtro = Convert.ToInt32(Cmb_Anio_Final.SelectedItem.Text);
            RP_Negocio.P_Bimestre_Filtro = Convert.ToInt32(Cmb_Bimestre_Final.SelectedItem.Text);
        }
        DataTable Dt_Adeudos = RP_Negocio.Consultar_Adeudos_Cuentas_Predial();
        Grid_Listado_Adeudos.DataSource = Dt_Adeudos;
        Grid_Listado_Adeudos.DataBind();
        Grid_Listado_Adeudos.Columns[1].Visible = false;
        if (Dt_Adeudos != null && Dt_Adeudos.Rows.Count > 0)
        {
            Txt_Anio_Inicial.Text = Dt_Adeudos.Rows[0]["ANIO"].ToString();
            Txt_Bimestre_Inicial.Text = Dt_Adeudos.Rows[0]["BIMESTRE"].ToString();
        }
        else
        {
            Txt_Anio_Inicial.Text = "";
            Txt_Bimestre_Inicial.Text = "";
        }
        Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio GAP_Negocio = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        DataTable dt = GAP_Negocio.Calcular_Recargos_Predial(P_Cuenta_Predial);
        Txt_Periodo_Actual.Text = GAP_Negocio.p_Periodo_Corriente;
        Txt_Periodo_Rezago.Text = GAP_Negocio.p_Periodo_Rezago;
        Realizar_Calculos_Pago(false);
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Convenio_Adeudos
    ///DESCRIPCIÓN: Llena el grid de los Adeudos dependiendo de los seleccionados.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 28 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Llenar_Grid_Convenio_Adeudos(DataTable Dt_Datos)
    {
        //Se llena el Grid de convenios de adeudos
        Grid_Listado_Adeudos_Convenio.Columns[1].Visible = true;
        Grid_Listado_Adeudos_Convenio.Columns[2].Visible = true;
        Grid_Listado_Adeudos_Convenio.DataSource = Dt_Datos;
        Grid_Listado_Adeudos_Convenio.DataBind();
        Grid_Listado_Adeudos_Convenio.Columns[1].Visible = false;
        Grid_Listado_Adeudos_Convenio.Columns[2].Visible = false;

        //Se seleccionan solo los adeudos obligatorios
        for (Int32 Cnt = 0; Cnt < Grid_Listado_Adeudos_Convenio.Rows.Count; Cnt++)
        {
            if (Grid_Listado_Adeudos_Convenio.Rows[Cnt].FindControl("Chk_Seleccion_Parcialidad") != null)
            {
                CheckBox Chk_Seleccion_Parcialidad = (CheckBox)Grid_Listado_Adeudos_Convenio.Rows[Cnt].FindControl("Chk_Seleccion_Parcialidad");
                if (Grid_Listado_Adeudos_Convenio.Rows[Cnt].Cells[2].Text.Trim().Equals("OBLIGATORIO"))
                {
                    Chk_Seleccion_Parcialidad.Checked = true;
                }
                else if (Grid_Listado_Adeudos_Convenio.Rows[Cnt].Cells[2].Text.Trim().Equals("OPCIONAL"))
                {
                    Chk_Seleccion_Parcialidad.Checked = false;
                }
            }
        }

        //Se ejecuta el metodo para calcular pagos
        Realizar_Calculos_Pago(true);
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Clave_Ingreso
    ///DESCRIPCIÓN:Obtiene la Clave de Ingreso.
    ///PARAMETROS: Tipo. Tipo de la Clave que se buscara
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private String Obtener_Clave_Ingreso(String Tipo)
    {
        String Clave = null;
        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
        RP_Negocio.P_Clave_Ingreso = Tipo;
        DataTable Dt_Temporal = RP_Negocio.Consultar_Clave_Ingreso();
        if (Dt_Temporal.Rows.Count > 0)
        {
            Clave = Dt_Temporal.Rows[0]["CLAVE_INGRESO"].ToString();
        }
        return Clave;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Componentes
    ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 24/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private static String Obtener_Dato_Consulta(String cuenta_predial_id)
    {
        String Mi_SQL;
        String Dato_Consulta = "";

        try
        {
            Mi_SQL = "SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "='" + cuenta_predial_id + "'";

            OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Dr_Dato.Read())
            {
                if (Dr_Dato[0] != null)
                {
                    Dato_Consulta = Dr_Dato[0].ToString();
                }
                else
                {
                    Dato_Consulta = "";
                }
                Dr_Dato.Close();
            }
            else
            {
                Dato_Consulta = "";
            }
            Dr_Dato = null;
        }
        catch
        {
        }
        finally
        {
        }

        return Dato_Consulta;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta_Valor
    ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 24/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private static String Obtener_Dato_Consulta_Valor(String cuenta_predial_id)
    {
        String Mi_SQL;
        String Dato_Consulta = "";

        try
        {
            Cls_Ope_Pre_Parametros_Negocio Anio_Corriente = new Cls_Ope_Pre_Parametros_Negocio();
            //select cuenta.valor_fiscal*(select tasa.tasa_anual from cat_pre_tasas_predial_anual tasa where tasa.tasa_PREDIAL_id=cuenta.tasa_PREDIAL_id 
            //and tasa.ANIO=2012)/1000||' - '||(SELECT MINIMA.CUOTA FROM CAT_PRE_CUOTAS_MINIMAS MINIMA WHERE MINIMA.CUOTA_MINIMA_ID=CUENTA.CUOTA_MINIMA_ID) 
            //from cat_pre_cuentas_predial cuenta where cuenta.cuenta_predial_id='0000031126';
            Mi_SQL = "SELECT CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + " * (SELECT TASA." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual + " FROM " + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + " TASA WHERE  TASA." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID + "=CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Tasa_ID + ")/1000||' - '||(SELECT MINIMA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota + " FROM " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas + " MINIMA WHERE MINIMA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + "=CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID + ") FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUENTA WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "='" + cuenta_predial_id + "'";

            OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Dr_Dato.Read())
            {
                if (Dr_Dato[0] != null)
                {
                    Dato_Consulta = Dr_Dato[0].ToString();
                }
                else
                {
                    Dato_Consulta = "";
                }
                Dr_Dato.Close();
            }
            else
            {
                Dato_Consulta = "";
            }
            Dr_Dato = null;
        }
        catch
        {
        }
        finally
        {
        }

        return Dato_Consulta;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta_Valor
    ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 24/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private static String Obtener_Dato_Consulta_Cuota(String Anio)
    {
        String Mi_SQL;
        String Dato_Consulta = "";

        try
        {
            //select cuenta.valor_fiscal*(select tasa.tasa_anual from cat_pre_tasas_predial_anual tasa where tasa.tasa_PREDIAL_id=cuenta.tasa_PREDIAL_id 
            //and tasa.ANIO=2012)/1000||' - '||(SELECT MINIMA.CUOTA FROM CAT_PRE_CUOTAS_MINIMAS MINIMA WHERE MINIMA.CUOTA_MINIMA_ID=CUENTA.CUOTA_MINIMA_ID) 
            //from cat_pre_cuentas_predial cuenta where cuenta.cuenta_predial_id='0000031126';
            Mi_SQL = "SELECT "+Cat_Pre_Cuotas_Minimas.Campo_Cuota+" FROM "+Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas+"  WHERE "+Cat_Pre_Cuotas_Minimas.Campo_Año+"=" + Anio;

            OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Dr_Dato.Read())
            {
                if (Dr_Dato[0] != null)
                {
                    Dato_Consulta = Dr_Dato[0].ToString();
                }
                else
                {
                    Dato_Consulta = "";
                }
                Dr_Dato.Close();
            }
            else
            {
                Dato_Consulta = "";
            }
            Dr_Dato = null;
        }
        catch
        {
        }
        finally
        {
        }

        return Dato_Consulta;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Clave_Ingreso
    ///DESCRIPCIÓN:Obtiene la Dependencia de una Clave de Ingreso.
    ///PARAMETROS: Tipo. Tipo de la Clave que se buscara
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private String Obtener_Dependencia_Clave_Ingreso(String Clave_Ingreso)
    {
        String Dependencia = null;
        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
        RP_Negocio.P_Clave_Ingreso = Clave_Ingreso;
        DataTable Dt_Temporal = RP_Negocio.Consultar_Dependencia();
        if (Dt_Temporal.Rows.Count > 0)
        {
            Dependencia = Dt_Temporal.Rows[0]["DEPENDENCIA"].ToString();
        }
        return Dependencia;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Registrar_Pasivos
    ///DESCRIPCIÓN: Registra los pasivos que será cobrados en la Caja.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 23 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Registrar_Pasivos()
    {
        Eliminar_Pasivos_No_Pagados();
        Double Rezago = Obtener_Impuesto_Rezago();
        if (Rezago > 0)
        {
            Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
            RP_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            RP_Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            RP_Negocio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            if (Hfd_Tipo_Predio.Value == "00001")
            {
                RP_Negocio.P_Clave_Ingreso = Obtener_Clave_Ingreso("REZAGO URBANO");
            }
            else
            {
                RP_Negocio.P_Clave_Ingreso = Obtener_Clave_Ingreso("REZAGO RUSTICO");
            }
            RP_Negocio.P_Descripcion = "IMPUESTO REZAGO [" + Txt_Periodo_Rezago.Text.Trim() + "]";
            RP_Negocio.P_Fecha_Tramite = DateTime.Today;
            RP_Negocio.P_Fecha_Vencimiento = DateTime.Today;
            RP_Negocio.P_Monto = Rezago;
            RP_Negocio.P_Estatus = "POR PAGAR";
            RP_Negocio.P_Dependencia = Obtener_Dependencia_Clave_Ingreso(RP_Negocio.P_Clave_Ingreso);
            RP_Negocio.P_Contribuyente = Txt_Propietario.Text.Replace("'", "´");
            RP_Negocio.Alta_Pasivo();
        }
        Double Impuestos = Obtener_Impuesto_Corriente();
        if (Impuestos > 0)
        {
            Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
            RP_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            RP_Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            RP_Negocio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            if (Hfd_Tipo_Predio.Value == "00001")
            {
                RP_Negocio.P_Clave_Ingreso = Obtener_Clave_Ingreso("IMPUESTO URBANO");
            }
            else
            {
                RP_Negocio.P_Clave_Ingreso = Obtener_Clave_Ingreso("IMPUESTO RUSTICO");
            }
            RP_Negocio.P_Descripcion = "IMPUESTO CORRIENTE [" + Txt_Periodo_Actual.Text.Trim() + "]";
            RP_Negocio.P_Fecha_Tramite = DateTime.Today;
            RP_Negocio.P_Fecha_Vencimiento = DateTime.Today;
            RP_Negocio.P_Monto = Impuestos;
            RP_Negocio.P_Estatus = "POR PAGAR";
            RP_Negocio.P_Dependencia = Obtener_Dependencia_Clave_Ingreso(RP_Negocio.P_Clave_Ingreso);
            RP_Negocio.P_Contribuyente = Txt_Propietario.Text.Replace("'","´");
            RP_Negocio.Alta_Pasivo();
        }
        Double Honorarios = Obtener_Honorarios();
        if (Honorarios > 0)
        {
            Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
            RP_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            RP_Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            RP_Negocio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            RP_Negocio.P_Clave_Ingreso = Obtener_Clave_Ingreso("HONORARIOS");
            RP_Negocio.P_Descripcion = "HONORARIOS";
            RP_Negocio.P_Fecha_Tramite = DateTime.Today;
            RP_Negocio.P_Fecha_Vencimiento = DateTime.Today;
            RP_Negocio.P_Monto = Honorarios;
            RP_Negocio.P_Estatus = "POR PAGAR";
            RP_Negocio.P_Dependencia = Obtener_Dependencia_Clave_Ingreso(RP_Negocio.P_Clave_Ingreso);
            RP_Negocio.P_Contribuyente = Txt_Propietario.Text.Replace("'", "´");
            RP_Negocio.Alta_Pasivo();
        }
        Double Recargos_Ordinarios = Obtener_Recargos_Ordinarios();
        if (Recargos_Ordinarios > 0)
        {
            Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
            RP_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            RP_Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            RP_Negocio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            RP_Negocio.P_Clave_Ingreso = Obtener_Clave_Ingreso("RECARGOS ORDINARIOS");
            RP_Negocio.P_Descripcion = "RECARGOS ORDINARIOS";
            RP_Negocio.P_Fecha_Tramite = DateTime.Today;
            RP_Negocio.P_Fecha_Vencimiento = DateTime.Today;
            RP_Negocio.P_Monto = Recargos_Ordinarios;
            RP_Negocio.P_Estatus = "POR PAGAR";
            RP_Negocio.P_Dependencia = Obtener_Dependencia_Clave_Ingreso(RP_Negocio.P_Clave_Ingreso);
            RP_Negocio.P_Contribuyente = Txt_Propietario.Text.Replace("'", "´");
            RP_Negocio.Alta_Pasivo();
        }
        Double Recargos_Moratorios = Obtener_Recargos_Moratorios_Calculos();
        if (Recargos_Moratorios > 0)
        {
            Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
            RP_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            RP_Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            RP_Negocio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            RP_Negocio.P_Clave_Ingreso = Obtener_Clave_Ingreso("RECARGOS MORATORIOS");
            RP_Negocio.P_Descripcion = "RECARGOS MORATORIOS";
            RP_Negocio.P_Fecha_Tramite = DateTime.Today;
            RP_Negocio.P_Fecha_Vencimiento = DateTime.Today;
            RP_Negocio.P_Monto = Recargos_Moratorios;
            RP_Negocio.P_Estatus = "POR PAGAR";
            RP_Negocio.P_Dependencia = Obtener_Dependencia_Clave_Ingreso(RP_Negocio.P_Clave_Ingreso);
            RP_Negocio.P_Contribuyente = Txt_Propietario.Text.Replace("'", "´");
            RP_Negocio.Alta_Pasivo();
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Eliminar_Pasivos_No_Pagados
    ///DESCRIPCIÓN: Elimina los pasivos no pagados de la tabla de ingresos.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 16 Octubre 2011 [Domingo ¬¬]
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Eliminar_Pasivos_No_Pagados()
    {
        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
        Negocio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
        Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value.Trim();
        Negocio.P_Estatus = "POR PAGAR";
        Negocio.Eliminar_Pasivos_No_Pagados_Anteriormente();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Honorarios
    ///DESCRIPCIÓN: Obtiene los Honorarios a Cobrar.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Double Obtener_Honorarios()
    {
        Double Honorarios = 0;
        Double Honorarios_Neto = 0;
        Double Honorarios_Descuento = 0;
        if (Txt_Honorarios.Text.Trim().Length > 0)
        {
            if (Convert.ToDouble(Txt_Honorarios.Text.Trim().Replace("$", "")) > 0)
            {
                Honorarios_Neto = Convert.ToDouble(Txt_Honorarios.Text.Trim().Replace("$", ""));
            }
        }
        if (Txt_Descuento_Honorarios.Text.Trim().Length > 0)
        {
            if (Convert.ToDouble(Txt_Descuento_Honorarios.Text.Trim().Replace("$", "")) > 0)
            {
                Honorarios_Descuento = Convert.ToDouble(Txt_Descuento_Honorarios.Text.Trim().Replace("$", ""));
            }
        }
        Honorarios = Honorarios_Neto - Honorarios_Descuento;
        return Honorarios;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Recargos_Ordinarios
    ///DESCRIPCIÓN: Obtiene los Recargos Ordinarios Totales.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Double Obtener_Recargos_Ordinarios()
    {
        Double Recargos_Ordinarios = 0;
        Double Recargos_Ordinarios_Neto = 0;
        Double Recargos_Ordinarios_Descuento = 0;
        if (Txt_Total_Recargos_Ordinarios.Text.Trim().Length > 0)
        {
            if (Convert.ToDouble(Txt_Total_Recargos_Ordinarios.Text.Trim().Replace("$", "")) > 0)
            {
                Recargos_Ordinarios_Neto = Convert.ToDouble(Txt_Total_Recargos_Ordinarios.Text.Trim().Replace("$", ""));
            }
        }
        if (Txt_Descuento_Recargos_Ordinarios.Text.Trim().Length > 0)
        {
            if (Convert.ToDouble(Txt_Descuento_Recargos_Ordinarios.Text.Trim().Replace("$", "")) > 0)
            {
                Recargos_Ordinarios_Descuento = Convert.ToDouble(Txt_Descuento_Recargos_Ordinarios.Text.Trim().Replace("$", ""));
            }
        }
        Recargos_Ordinarios = Recargos_Ordinarios_Neto - Recargos_Ordinarios_Descuento;
        return Recargos_Ordinarios;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Recargos_Moratorios_Calsulos
    ///DESCRIPCIÓN: Obtiene los Recargos Moratorios Totales.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Double Obtener_Recargos_Moratorios_Calculos()
    {
        Double Recargos_Moratorios = 0;
        Double Recargos_Moratorios_Neto = 0;
        Double Recargos_Moratorios_Descuento = 0;
        if (Txt_Recargos_Moratorios.Text.Trim().Length > 0)
        {
            if (Convert.ToDouble(Txt_Recargos_Moratorios.Text.Trim().Replace("$", "")) > 0)
            {
                Recargos_Moratorios_Neto = Convert.ToDouble(Txt_Recargos_Moratorios.Text.Trim().Replace("$", ""));
            }
        }
        if (Txt_Descuento_Recargos_Moratorios.Text.Trim().Length > 0)
        {
            if (Convert.ToDouble(Txt_Descuento_Recargos_Moratorios.Text.Trim().Replace("$", "")) > 0)
            {
                Recargos_Moratorios_Descuento = Convert.ToDouble(Txt_Descuento_Recargos_Ordinarios.Text.Trim().Replace("$", ""));
            }
        }
        Recargos_Moratorios = Recargos_Moratorios_Neto - Recargos_Moratorios_Descuento;
        return Recargos_Moratorios;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Impuesto_Corriente
    ///DESCRIPCIÓN: Obtiene los Recargos Impuesto.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Double Obtener_Impuesto_Corriente()
    {
        Double Impuesto = 0;
        Double Corriente = 0;
        Double Descuento_Corriente = 0;

        if (Txt_Adeudo_Actual.Text.Trim().Length > 0)
        {
            if (Convert.ToDouble(Txt_Adeudo_Actual.Text.Trim().Replace("$", "")) > 0)
            {
                Corriente = Convert.ToDouble(Txt_Adeudo_Actual.Text.Trim().Replace("$", ""));
            }
        }

        if (Txt_Descuento_Corriente.Text.Trim().Length > 0)
        {
            if (Convert.ToDouble(Txt_Descuento_Corriente.Text.Trim().Replace("$", "")) > 0)
            {
                Descuento_Corriente = Convert.ToDouble(Txt_Descuento_Corriente.Text.Trim().Replace("$", ""));
            }
        }

        Impuesto = Corriente - Descuento_Corriente;

        return Impuesto;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Impuesto_Rezago
    ///DESCRIPCIÓN: Obtiene los Recargos Impuesto.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Double Obtener_Impuesto_Rezago()
    {
        Double Impuesto = 0;
        Double Rezago = 0;

        if (Txt_Adeudo_Rezago.Text.Trim().Length > 0)
        {
            if (Convert.ToDouble(Txt_Adeudo_Rezago.Text.Trim().Replace("$", "")) > 0)
            {
                Rezago = Convert.ToDouble(Txt_Adeudo_Rezago.Text.Trim().Replace("$", ""));
            }
        }


        Impuesto = Rezago;
        return Impuesto;
    }
    #endregion

    #region "Convenios"

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Buscar_Convenio_Cuenta_Predial
    ///DESCRIPCIÓN: Busca un convenio Activo para la cuenta predial.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 22 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Buscar_Convenio_Cuenta_Predial()
    {
        DataTable Dt_Convenio = new DataTable();
        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
        Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
        Dt_Convenio = Negocio.Consultar_Convenio_Cuenta_Predia();
        return Dt_Convenio;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: obtener_Recargos_Moratorios
    /// DESCRIPCIÓN: Leer las parcialidades del ultimo convenio o reestructura para obtener los adeudos
    ///            a tomar en cuenta para la reestructura
    /// PARÁMETROS:
    /// 		1. Incumplido: Valida el calculo cuando un convenio es incumplido
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 13-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Obtener_Recargos_Moratorios(Boolean Incumplido)
    {
        Cls_Ope_Caj_Pagos_Negocio Rs_Consulta_Datos_Turno = new Cls_Ope_Caj_Pagos_Negocio(); //Variable de conexión hacia la capa de Negocios
        Cls_Ope_Pre_Convenios_Predial_Negocio Consulta_Parcialidades = new Cls_Ope_Pre_Convenios_Predial_Negocio();
        Cls_Ope_Pre_Convenios_Predial_Negocio Consulta_Convenios = new Cls_Ope_Pre_Convenios_Predial_Negocio();
        DataTable Dt_Parcialidades;
        DataTable Dt_Convenios;
        DataTable Dt_Caja; //Variable que obtendra los datos de la consulta  de la caja del empleado
        DataTable Dt_Turno; //Variable que obtendra los datos de la consulta de la fecha de aplicacion
        Decimal Recargos_Moratorios = 0;
        Decimal Recargos_Ordinarios = 0;
        Decimal Honorarios = 0;
        Decimal Monto_Impuesto = 0;
        Decimal Monto_Base = 0;
        Decimal Adeudo_Honorarios = 0;
        Decimal Adeudo_Recargos = 0;
        Decimal Adeudo_Moratorios = 0;
        String No_Convenio = "";
        int Parcialidad = 0;
        DateTime Fecha_Vencimiento = DateTime.MinValue;
        int Meses_Transcurridos = 0;
        int Hasta_Anio = 0;
        int Hasta_Bimestre = 0;
        DateTime Fecha_Actual = DateTime.Today; //Almacena la fecha de aplicacion o actual
        String Caja_ID = "";    //Almacena el id de la caja del empleado

        if (Incumplido)
        {
            //Obtiene la consulta de la caja del empleado
            Rs_Consulta_Datos_Turno.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
            Dt_Caja = Rs_Consulta_Datos_Turno.Consulta_Caja_Empleado();
            if (Dt_Caja.Rows.Count > 0)
            {
                foreach (DataRow Registro in Dt_Caja.Rows)
                {
                    Caja_ID = Registro["CAJA_ID"].ToString();
                }
            }

            //Obtiene la fecha de aplicacion
            Rs_Consulta_Datos_Turno.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
            Rs_Consulta_Datos_Turno.P_Caja_ID = Caja_ID;
            Dt_Turno = Rs_Consulta_Datos_Turno.Consulta_Datos_Turno();
            if (Dt_Turno.Rows.Count > 0)
            {
                foreach (DataRow Registro in Dt_Turno.Rows)
                {
                    Fecha_Actual = Convert.ToDateTime(Registro["APLICACION_PAGO"].ToString());
                }
            }
            else
            {
                Fecha_Actual = DateTime.Today;
            }

            // consultar convenios de la cuenta
            Consulta_Convenios.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            Consulta_Convenios.P_Ordenar_Dinamico = Ope_Pre_Convenios_Predial.Campo_Fecha + " DESC,"
                + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " DESC";
            Dt_Convenios = Consulta_Convenios.Consultar_Convenio_Predial();
            // si la consulta arrojó resultado, utilizar el primer registro (convenio mas reciente)
            if (Dt_Convenios != null && Dt_Convenios.Rows.Count > 0)
            {
                No_Convenio = Dt_Convenios.Rows[0][Ope_Pre_Convenios_Predial.Campo_No_Convenio].ToString();
                // consultar las parcialidades del ultimo convenio guardado (convenio o ultima reestructura)
                Consulta_Parcialidades.P_No_Convenio = No_Convenio;
                Dt_Parcialidades = Consulta_Parcialidades.Consultar_Parcialidades_Ultimo_Convenio();

                // llamar metodo para determinar si el convenio esta vencido
                if (Convenio_Vencido(Dt_Parcialidades))
                {
                    Parcialidad = Dt_Parcialidades.Rows.Count - 1;
                    // recorrer la tabla de parcialidades hasta encontrar parcialidades con estatus PAGADO
                    while (Parcialidad >= 0)
                    {
                        // obtener el ultimo periodo incluido en el convenio
                        if (Hasta_Anio <= 0 || Hasta_Bimestre <= 0)
                        {
                            string Periodo_Parcialidad = Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo].ToString();
                            if (Periodo_Parcialidad.Trim().Length >= 13)
                            {
                                int.TryParse(Periodo_Parcialidad.Substring(Periodo_Parcialidad.Trim().Length - 4, 4), out Hasta_Anio);
                                int.TryParse(Periodo_Parcialidad.Substring(Periodo_Parcialidad.Trim().Length - 6, 1), out Hasta_Bimestre);
                            }
                        }

                        // si la parcialidad tiene estatus diferente de PAGADO, sumar adeudos
                        if (Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus].ToString() != "PAGADO")
                        {
                            Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios].ToString(), out Honorarios);
                            Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios].ToString(), out Recargos_Ordinarios);
                            Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios].ToString(), out Recargos_Moratorios);
                            Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto].ToString(), out Monto_Impuesto);
                            DateTime.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento].ToString(), out Fecha_Vencimiento);
                            Adeudo_Honorarios += Honorarios;
                            Adeudo_Recargos += Recargos_Ordinarios;
                            Adeudo_Moratorios += Recargos_Moratorios;
                            Monto_Base += Monto_Impuesto;
                        }
                        Parcialidad--;
                    }

                    //Obtiene el monto total de la cuenta
                    Llenar_Grid_Adeudos(Hdf_Cuenta_Predial_ID.Value, false);

                    // restar adeudos de bimestres que no han vencido (si el año es mayor al actual o es igual con el bimestre mayor al actual)
                    if (Hasta_Anio > Fecha_Actual.Year || (Hasta_Anio == Fecha_Actual.Year && Hasta_Bimestre >= Fecha_Actual.Month / 2))
                    {
                        Monto_Base -= Adeudos_Predial_Sin_Vencer(Hdf_Cuenta_Predial_ID.Value, Hasta_Anio, Hasta_Bimestre, Fecha_Actual);
                    }

                    // agregar adeudos vencidos despues de convenio
                    Monto_Base += Adeudos_Predial_Actuales_Despues_Convenio(Hdf_Cuenta_Predial_ID.Value, Hasta_Anio, Hasta_Bimestre, Fecha_Actual);

                    Meses_Transcurridos = Calcular_Meses_Entre_Fechas(Fecha_Vencimiento, Fecha_Actual);

                }
            }
        }
        else
        {
            Meses_Transcurridos = 0;
        }

        Recargos_Moratorios = Calcular_Recargos_Moratorios(Monto_Base, Meses_Transcurridos);
        Txt_Recargos_Moratorios.Text = Math.Round(Math.Round(Recargos_Moratorios + Adeudo_Moratorios, 3), 2).ToString("###,##0.00");
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Convenio_Vencido
    /// DESCRIPCIÓN: Revisar las parcialidades en busca de parcialidades vencidas 
    ///             parcialidades sin pagar con fecha de vencimiento de hace mas de 10 dias habiles
    ///             Regresa verdadero si el convenio esta vencido.
    /// PARÁMETROS:
    /// 		1. Dt_Parcialidades: datatable con parcialidades de un convenio
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 13-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private bool Convenio_Vencido(DataTable Dt_Parcialidades)
    {
        Cls_Ope_Caj_Pagos_Negocio Rs_Consulta_Datos_Turno = new Cls_Ope_Caj_Pagos_Negocio(); //Variable de conexión hacia la capa de Negocios
        Cls_Ope_Pre_Dias_Inhabiles_Negocio Calcular_Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();  //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Caja; //Variable que obtendra los datos de la consulta  de la caja del empleado
        DataTable Dt_Turno; //Variable que obtendra los datos de la consulta de la fecha de aplicacion
        DateTime Fecha_Periodo;
        DateTime Fecha_Vencimiento;
        DateTime Fecha_Actual = DateTime.Today; //Almacena la fecha de aplicacion o actual
        String Caja_ID = "";    //Almacena el id de la caja del empleado

        int Dias = 0;
        int Meses = 0;
        bool Convenio_Vencido = false;

        //Obtiene la consulta de la caja del empleado
        Rs_Consulta_Datos_Turno.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
        Dt_Caja = Rs_Consulta_Datos_Turno.Consulta_Caja_Empleado();
        if (Dt_Caja.Rows.Count > 0)
        {
            foreach (DataRow Registro in Dt_Caja.Rows)
            {
                Caja_ID = Registro["CAJA_ID"].ToString();
            }
        }

        //Obtiene la fecha de aplicacion
        Rs_Consulta_Datos_Turno.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
        Rs_Consulta_Datos_Turno.P_Caja_ID = Caja_ID;
        Dt_Turno = Rs_Consulta_Datos_Turno.Consulta_Datos_Turno();
        if (Dt_Turno.Rows.Count > 0)
        {
            foreach (DataRow Registro in Dt_Turno.Rows)
            {
                Fecha_Actual = Convert.ToDateTime(Registro["APLICACION_PAGO"].ToString());
            }
        }
        else
        {
            Fecha_Actual = DateTime.Today;
        }

        // recorrer las parcialidades del convenio
        for (int Pago = 0; Pago < Dt_Parcialidades.Rows.Count; Pago++)
        {
            // si el estatus de la parcialidad es INCUMPLIDO
            if (Dt_Parcialidades.Rows[Pago][Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus].ToString().Trim() == "INCUMPLIDO")
            {
                // obtener la fecha de vencimiento de la parcialidad
                DateTime.TryParse(Dt_Parcialidades.Rows[Pago][Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento].ToString(), out Fecha_Periodo);
                Fecha_Vencimiento = Calcular_Dias_Inhabilies.Calcular_Fecha(Fecha_Periodo.ToShortDateString(), "10");
                // obtener el tiempo transcurrido desde la fecha de vencimiento
                Calcular_Tiempo_Entre_Fechas(Fecha_Vencimiento, Fecha_Actual, out Dias, out Meses);
                // si el numero de dias transcurridos en mayor que cero, escribir fecha de vencimiento
                if (Dias > 0)
                {
                    Convenio_Vencido = true;
                }
                // abandonar el ciclo for
                break;
            }
        }
        if (!Convenio_Vencido)
        {
            for (Int32 Contador = 0; Contador < Dt_Parcialidades.Rows.Count; Contador++)
            {
                if (!String.IsNullOrEmpty(Dt_Parcialidades.Rows[Contador]["FECHA_VENCIMIENTO"].ToString()))
                {
                    Fecha_Vencimiento = Convert.ToDateTime(Dt_Parcialidades.Rows[Contador]["FECHA_VENCIMIENTO"].ToString());
                    Fecha_Vencimiento = Convert.ToDateTime(Calcular_Dias_Inhabilies.Calcular_Fecha(Fecha_Vencimiento.ToShortDateString(), "10"));
                    if (Fecha_Vencimiento < Fecha_Actual)
                    {
                        Convenio_Vencido = true;
                        break;
                    }
                }
            }
        }
        return Convenio_Vencido;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Meses_Entre_Fechas
    /// DESCRIPCIÓN: Regresa un enteron con el numero de meses vencidos entre dos fechas
    ///             (tomando el primer dia de cada mes)
    /// PARÁMETROS:
    /// 		1. Desde_Fecha: Fecha inicial a comparar
    /// 		2. Hasta_Fecha: Fecha final a comparar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 05-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Int32 Calcular_Meses_Entre_Fechas(DateTime Desde_Fecha, DateTime Hasta_Fecha)
    {
        DateTime Fecha_Inicial;
        DateTime Fecha_Final;
        int Meses = 0;

        // establecer fecha inicial como el primer día del mes en Desde_Fecha
        DateTime.TryParse(Desde_Fecha.ToString("01-MMM-yyyy"), out Fecha_Inicial);
        // tomar la fecha en Hasta_Fecha (ignorando la hora)
        DateTime.TryParse(Hasta_Fecha.ToString("dd-MMM-yyyy"), out Fecha_Final);

        // validar que se obtuvo una fecha inicial
        if (Fecha_Inicial != DateTime.MinValue)
        {
            // aumentar el numero de meses mientras la fecha inicial mas los meses no supere la fecha final
            while (Fecha_Final >= Fecha_Inicial.AddMonths(Meses))
            {
                Meses++;
            }
        }

        return Meses;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Tiempo_Entre_Fechas
    /// DESCRIPCIÓN: Calcular numero de dias y meses transcurridos entre una fecha y otra
    /// PARÁMETROS:
    /// 		1. Desde_Fecha: Fecha inferior a tomar
    /// 		2. Hasta_Fecha: Fecha final hasta la que se calcula
    /// 		3. Dias: Se almacenan los dias de diferencia entre las fechas
    /// 		4. Meses: Almacena los meses transcurridos entre una fecha y otra
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 12-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Calcular_Tiempo_Entre_Fechas(DateTime Desde_Fecha, DateTime Hasta_Fecha, out Int32 Dias, out Int32 Meses)
    {
        TimeSpan Transcurrido = Hasta_Fecha - Desde_Fecha;
        if (Transcurrido > TimeSpan.Parse("0"))
        {
            DateTime Tiempo = DateTime.MinValue + Transcurrido;
            Meses = ((Tiempo.Year - 1) * 12) + (Tiempo.Month - 1);

            long tickDiff = Hasta_Fecha.Ticks - Desde_Fecha.Ticks;
            tickDiff = tickDiff / 10000000; // segundos
            Dias = (int)(tickDiff / 86400);
        }
        else
        {
            Dias = 0;
            Meses = 0;
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Recargos_Moratorios
    /// DESCRIPCIÓN: Calcular los recargos moratorios para una cantidad a partir de una fecha dados
    ///             (el numero de meses por el porcentaje de recargos por el monto base)
    /// PARÁMETROS:
    /// 		1. Monto_Base: Cantidad a la que se van a calcular los recargos
    /// 		2. Meses: Numero de meses a considedar para el calculo de recargos 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 21-nov-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Decimal Calcular_Recargos_Moratorios(Decimal Monto_Base, Int32 Meses)
    {
        Cls_Ope_Pre_Parametros_Negocio Parametros = new Cls_Ope_Pre_Parametros_Negocio();
        DataTable Dt_Parametros;
        Decimal Recargos_Moratorios = 0;
        Decimal Porcentaje_Recargos = 0;

        // recuperar el porcentaje de recargos moratorios de la tabla de parametros
        Dt_Parametros = Parametros.Consultar_Parametros();
        if (Dt_Parametros != null)
        {
            if (Dt_Parametros.Rows.Count > 0)
            {
                Decimal.TryParse(Dt_Parametros.Rows[0][Ope_Pre_Parametros.Campo_Recargas_Traslado].ToString(), out Porcentaje_Recargos);
            }
        }

        // obtener el producto de los meses por el porcentaje de recargos
        Porcentaje_Recargos *= Meses;

        // calcular recargos
        Recargos_Moratorios = Monto_Base * Porcentaje_Recargos / 100;

        return Recargos_Moratorios;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Adeudos_Predial_Sin_Vencer
    /// DESCRIPCIÓN: Regresa la suma de los adeudos aún no vencidos incluidos en el convenio
    ///             Consulta adeudos predial y suma los adeudos a partir del bimestre vencido actual
    /// PARÁMETROS:
    /// 		1. Cuenta_Predial_ID: id de la cuenta predial para consultar adeudos
    /// 		2. Hasta_Anio: año del último bimestre incluido en el convenio
    /// 		3. Ultimo_Bimestre: bimestre del último periodo incluido en el convenio
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 01-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private decimal Adeudos_Predial_Sin_Vencer(string Cuenta_Predial_ID, int Hasta_Anio, int Ultimo_Bimestre, DateTime Fecha_Actual)
    {
        decimal Adeudos_Sin_Vencer = 0;
        var Consulta_Adeudos = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        DataTable Dt_Adeudos;
        int Anio_Actual = Fecha_Actual.Year;
        int Bimestre_Vencido;

        Bimestre_Vencido = Fecha_Actual.Month % 2 == 0 ? Fecha_Actual.Month / 2 : (Fecha_Actual.Month / 2) + 1;

        // consultar adeudos actuales de la cuenta
        Dt_Adeudos = Consulta_Adeudos.Consultar_Adeudos_Cuenta_Predial(Cuenta_Predial_ID, "POR PAGAR", 0, 0);
        // validar que se obtuvieron datos de la consulta de adeudos
        if (Dt_Adeudos != null && Dt_Adeudos.Rows.Count > 0)
        {
            // recorrer todas las filas de la tabla de adeudos
            for (int Contador_Filas = 0; Contador_Filas < Dt_Adeudos.Rows.Count; Contador_Filas++)
            {
                int Anio_Adeudo;
                int.TryParse(Dt_Adeudos.Rows[Contador_Filas][Ope_Pre_Adeudos_Predial.Campo_Anio].ToString(), out Anio_Adeudo);
                // si el año es menor que Hasta_Anio pasar al siguiente adeudo
                if (Anio_Adeudo >= Anio_Actual)
                {
                    int Desde_Bimestre = 1;
                    int Hasta_Bimestre = 6;
                    if (Hasta_Anio == Anio_Actual)
                    {
                        Hasta_Bimestre = Ultimo_Bimestre;
                        Desde_Bimestre = Bimestre_Vencido + 1;
                        // recorrer los bimestres para agregar adeudos
                        for (int Contador_Bimestres = Desde_Bimestre; Contador_Bimestres <= Hasta_Bimestre; Contador_Bimestres++)
                        {
                            decimal Adeudo_Bimestre;
                            decimal.TryParse(Dt_Adeudos.Rows[Contador_Filas]["ADEUDO_BIMESTRE_" + Contador_Bimestres].ToString(), out Adeudo_Bimestre);
                            Adeudos_Sin_Vencer += Adeudo_Bimestre;
                        }
                    }
                    else if (Hasta_Anio > Anio_Actual)
                    {
                        Hasta_Bimestre = Ultimo_Bimestre;
                        // recorrer los bimestres para agregar adeudos
                        for (int Contador_Bimestres = Desde_Bimestre; Contador_Bimestres <= Hasta_Bimestre; Contador_Bimestres++)
                        {
                            decimal Adeudo_Bimestre;
                            decimal.TryParse(Dt_Adeudos.Rows[Contador_Filas]["ADEUDO_BIMESTRE_" + Contador_Bimestres].ToString(), out Adeudo_Bimestre);
                            Adeudos_Sin_Vencer += Adeudo_Bimestre;
                        }
                    }
                }
            } // for
        }

        return Adeudos_Sin_Vencer;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Adeudos_Predial_Actuales_Despues_Convenio
    /// DESCRIPCIÓN: Regresa la suma de los adeudos vencidos despues del periodo indicado como parametro
    /// PARÁMETROS:
    /// 		1. Cuenta_Predial_ID: id de la cuenta predial para consultar adeudos
    /// 		2. Desde_Anio: Año del periodo inicial a tomar
    /// 		3. Desde_Bimestre: bimestre del periodo inicial a tomar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 18-feb-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private decimal Adeudos_Predial_Actuales_Despues_Convenio(string Cuenta_Predial_ID, int Desde_Anio, int Desde_Bimestre, DateTime Fecha_Actual)
    {
        decimal Adeudos_Despues_Convenio = 0;
        var Consulta_Adeudos = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        DataTable Dt_Adeudos;
        int Anio_Actual = Fecha_Actual.Year;
        int Bimestre_Vencido;

        Bimestre_Vencido = Fecha_Actual.Month % 2 == 0 ? Fecha_Actual.Month / 2 : (Fecha_Actual.Month / 2) + 1;

        // periodo a partir del cual se va a tomar en cuenta (desde_bimestre + 1)
        Desde_Bimestre++;
        if (Desde_Bimestre > 6)
        {
            Desde_Bimestre = 1;
            Desde_Anio++;
        }

        // consultar adeudos actuales de la cuenta
        Dt_Adeudos = Consulta_Adeudos.Consultar_Adeudos_Cuenta_Predial(Cuenta_Predial_ID, "POR PAGAR", 0, 0);
        // agregar adeudos vencidos a la fecha
        if (Dt_Adeudos != null && Dt_Adeudos.Rows.Count > 0)
        {
            // recorrer todas las filas de la tabla de adeudos
            for (int Contador_Filas = 0; Contador_Filas < Dt_Adeudos.Rows.Count; Contador_Filas++)
            {
                int Anio_Adeudo;
                int.TryParse(Dt_Adeudos.Rows[Contador_Filas][Ope_Pre_Adeudos_Predial.Campo_Anio].ToString(), out Anio_Adeudo);
                // si el año es menor que Desde_Anio pasar al siguiente adeudo
                if (Anio_Adeudo < Desde_Anio)
                {
                    continue;
                }
                // si el año del adeudo es igual al año desde el que se calculan los moratorios, agregar solo los adeudos despues del bimestre indicado
                else if (Anio_Adeudo == Desde_Anio)
                {
                    int Hasta_Bimestre = 6;
                    // si es el año actual, tomar hasta el bimestre vencido
                    if (Anio_Adeudo == Anio_Actual)
                        Hasta_Bimestre = Bimestre_Vencido;
                    // recorrer los bimestres para agregar adeudos
                    for (int Contador_Bimestres = Desde_Bimestre; Contador_Bimestres <= Hasta_Bimestre; Contador_Bimestres++)
                    {
                        decimal Adeudo_Bimestre;
                        decimal.TryParse(Dt_Adeudos.Rows[Contador_Filas]["ADEUDO_BIMESTRE_" + Contador_Bimestres].ToString(), out Adeudo_Bimestre);
                        Adeudos_Despues_Convenio += Adeudo_Bimestre;
                    }
                }
                // si el año es mayor que el año especificado, agregar adeudos al monto total
                else if (Anio_Adeudo > Desde_Anio)
                {
                    int Hasta_Bimestre = 6;
                    // si es el año actual, tomar hasta el bimestre vencido
                    if (Anio_Adeudo == Anio_Actual)
                        Hasta_Bimestre = Bimestre_Vencido;
                    // recorrer los bimestres para agregar al adeudo
                    for (int Contador_Bimestres = 1; Contador_Bimestres <= Hasta_Bimestre; Contador_Bimestres++)
                    {
                        decimal Adeudo_Bimestre;
                        decimal.TryParse(Dt_Adeudos.Rows[Contador_Filas]["ADEUDO_BIMESTRE_" + Contador_Bimestres].ToString(), out Adeudo_Bimestre);
                        Adeudos_Despues_Convenio += Adeudo_Bimestre;
                    }
                }

            } // for
        }

        return Adeudos_Despues_Convenio;
    }
    #endregion

    #endregion

    #region "Grids"

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Adeudos_RowDataBound
    ///DESCRIPCIÓN: Evento RowDataBound del Grid de Adeudos sin Convenio.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Listado_Adeudos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.FindControl("Chk_Seleccion_Adeudo") != null)
            {
                CheckBox Chk_Seleccion_Adeudo = (CheckBox)e.Row.FindControl("Chk_Seleccion_Adeudo");
                Chk_Seleccion_Adeudo.TabIndex = (short)e.Row.RowIndex;
            }
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Adeudos_Convenio_RowDataBound
    ///DESCRIPCIÓN: Evento RowDataBound del Grid de Adeudos del Convenio.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Listado_Adeudos_Convenio_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.FindControl("Chk_Seleccion_Parcialidad") != null)
            {
                CheckBox Chk_Seleccion_Parcialidad = (CheckBox)e.Row.FindControl("Chk_Seleccion_Parcialidad");
                Chk_Seleccion_Parcialidad.TabIndex = (short)e.Row.RowIndex;
                if (e.Row.Cells[2].Text.Trim().Equals("OBLIGATORIO"))
                {
                    Chk_Seleccion_Parcialidad.Enabled = false;
                }
                else if (e.Row.Cells[2].Text.Trim().Equals("OPCIONAL"))
                {
                    Chk_Seleccion_Parcialidad.Enabled = true;
                }
            }
            if (e.Row.Cells[4].Text.Trim().Length > 0 && e.Row.Cells[4].Text.Trim().Equals("0"))
            {
                e.Row.Cells[4].Text = "";
            }
        }
    }

    #endregion

    #region "Eventos"

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Actualizar_Listado_Adeudos_Click
    ///DESCRIPCIÓN: Actualiza el Listado de los Adeudos.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 22 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Actualizar_Listado_Adeudos_Click(object sender, EventArgs e)
    {
        if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0)
        {
            Grid_Listado_Adeudos.DataSource = new DataTable();
            Grid_Listado_Adeudos.DataBind();
            Llenar_Grid_Adeudos(Hdf_Cuenta_Predial_ID.Value, true);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cmb_Bimestre_Final_SelectedIndexChanged
    ///DESCRIPCIÓN: Actualiza el Listado de los Adeudos.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Bimestre_Final_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0)
        {
            Grid_Listado_Adeudos.DataSource = new DataTable();
            Grid_Listado_Adeudos.DataBind();
            Llenar_Grid_Adeudos(Hdf_Cuenta_Predial_ID.Value, true);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cmb_Anio_Final_SelectedIndexChanged
    ///DESCRIPCIÓN: Actualiza el Listado de los Adeudos.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Anio_Final_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0)
        {
            Grid_Listado_Adeudos.DataSource = new DataTable();
            Grid_Listado_Adeudos.DataBind();
            Llenar_Grid_Adeudos(Hdf_Cuenta_Predial_ID.Value, true);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Calcular_Pago_Click
    ///DESCRIPCIÓN: Recalcula el Pago.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 24 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Seleccion_Adeudo_CheckedChanged(object sender, EventArgs e)
    {
        Int32 No_Fila = Convert.ToInt32(((CheckBox)(sender)).TabIndex);
        Cls_Ope_Pre_Parametros_Negocio Par= new Cls_Ope_Pre_Parametros_Negocio();
        Int32 Anio_Corriente = Par.Consultar_Anio_Corriente();
        for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos.Rows.Count; Contador++)
        {
            if (Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo") != null)
            {
                CheckBox Chk_Seleccion_Adeudo_Tmp = (CheckBox)Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo");
                if (Contador < No_Fila)
                {
                    Chk_Seleccion_Adeudo_Tmp.Checked = true;
                    if (Convert.ToInt32(Grid_Listado_Adeudos.Rows[Contador].Cells[3].Text) > Anio_Corriente)
                    {
                        No_Fila = Contador;
                        Chk_Seleccion_Adeudo_Tmp.Checked = false;
                    }
                }
                else if (Contador > No_Fila)
                {
                    Chk_Seleccion_Adeudo_Tmp.Checked = false;
                }
            }
        }
        Realizar_Calculos_Pago(false);
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Calcular_Pago_Click
    ///DESCRIPCIÓN: Recalcula el Pago.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 24 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Ejecutar_Pago_Click(object sender, EventArgs e)
    {
        Boolean Ejecutar_Pago = true;
        if (Hdf_Cuenta_Predial_ID.Value.Trim().Length == 0) { Ejecutar_Pago = false; }
        if (Txt_Total_Pagar.Text.Trim().Length == 0)
        {
            Ejecutar_Pago = false;
        }
        else
        {
            if (Convert.ToDouble(Txt_Total_Pagar.Text.Trim().Replace("$", "")) == 0) { Ejecutar_Pago = false; }
        }
        if (Ejecutar_Pago)
        {
            Registrar_Pasivos();
            Direccionar_Caja();
        }
        else
        {
            Lbl_Ecabezado_Mensaje.Text = "Es necesario tener algo que cobrar";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Regresa a la Bandeja de Caja.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 24 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Predial/Frm_Ope_Listado_Adeudos_Predial.aspx");
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Chk_Seleccion_Parcialidad_CheckedChanged
    ///DESCRIPCIÓN: Recalcula el Pago.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 27 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Seleccion_Parcialidad_CheckedChanged(object sender, EventArgs e)
    {
        Int32 No_Fila = Convert.ToInt32(((CheckBox)(sender)).TabIndex);
        String No_Pago = Grid_Listado_Adeudos_Convenio.Rows[No_Fila].Cells[1].Text.Trim();
        for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos_Convenio.Rows.Count; Contador++)
        {
            if (Grid_Listado_Adeudos_Convenio.Rows[Contador].FindControl("Chk_Seleccion_Parcialidad") != null)
            {
                CheckBox Chk_Seleccion_Adeudo_Tmp = (CheckBox)Grid_Listado_Adeudos_Convenio.Rows[Contador].FindControl("Chk_Seleccion_Parcialidad");
                if (Contador < No_Fila)
                {
                    Chk_Seleccion_Adeudo_Tmp.Checked = true;
                }
                else if (Contador > No_Fila)
                {
                    Chk_Seleccion_Adeudo_Tmp.Checked = false;
                }
                if (Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[1].Text.Trim().Equals(No_Pago))
                {
                    Chk_Seleccion_Adeudo_Tmp.Checked = ((CheckBox)(sender)).Checked;
                }
            }
        }
        Realizar_Calculos_Pago(true);
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Validar_Convenio_No_Imcumplido
    ///DESCRIPCIÓN: Valdia el Incumplimiento de un Convenio de Predial.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 22 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Boolean Validar_Convenio_No_Imcumplido(DataTable Dt_Parcialidades)
    {
        Cls_Ope_Caj_Pagos_Negocio Rs_Consulta_Datos_Turno = new Cls_Ope_Caj_Pagos_Negocio(); //Variable de conexión hacia la capa de Negocios
        Cls_Ope_Pre_Dias_Inhabiles_Negocio Dias_Inhabiles = new Cls_Ope_Pre_Dias_Inhabiles_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Caja; //Variable que obtendra los datos de la consulta  de la caja del empleado
        DataTable Dt_Turno; //Variable que obtendra los datos de la consulta de la fecha de aplicacion
        Boolean Convenio_Incumplido = false; //Almacena para saber si se incumplio o no la parcialidad
        DateTime Fecha_Actual = DateTime.Today; //Almacena la fecha de aplicacion o actual
        String Caja_ID = "";    //Almacena el id de la caja del empleado

        //Obtiene la consulta de la caja del empleado
        Rs_Consulta_Datos_Turno.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
        Dt_Caja = Rs_Consulta_Datos_Turno.Consulta_Caja_Empleado();
        if (Dt_Caja.Rows.Count > 0)
        {
            foreach (DataRow Registro in Dt_Caja.Rows)
            {
                Caja_ID = Registro["CAJA_ID"].ToString();
            }
        }

        //Obtiene la fecha de aplicacion
        Rs_Consulta_Datos_Turno.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
        Rs_Consulta_Datos_Turno.P_Caja_ID = Caja_ID;
        Dt_Turno = Rs_Consulta_Datos_Turno.Consulta_Datos_Turno();
        if (Dt_Turno.Rows.Count > 0)
        {
            foreach (DataRow Registro in Dt_Turno.Rows)
            {
                Fecha_Actual = Convert.ToDateTime(Registro["APLICACION_PAGO"].ToString());
            }
        }
        else
        {
            Fecha_Actual = DateTime.Today;
        }

        //Valida que tenga parcialidades
        if (Dt_Parcialidades != null && Dt_Parcialidades.Rows.Count > 0)
        {
            for (Int32 Contador = 0; Contador < Dt_Parcialidades.Rows.Count; Contador++)
            {
                if (!String.IsNullOrEmpty(Dt_Parcialidades.Rows[Contador]["FECHA_VENCIMIENTO"].ToString()))
                {
                    DateTime Fecha_Vencimiento = Convert.ToDateTime(Dt_Parcialidades.Rows[Contador]["FECHA_VENCIMIENTO"].ToString());
                    Fecha_Vencimiento = Convert.ToDateTime(Dias_Inhabiles.Calcular_Fecha(Fecha_Vencimiento.ToShortDateString(), "10"));
                    if (Fecha_Vencimiento < Fecha_Actual)
                    {
                        Convenio_Incumplido = true;
                        break;
                    }
                }
            }
        }
        return Convenio_Incumplido;
    }
    #endregion

}
