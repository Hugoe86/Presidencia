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
using Presidencia.Caja_Pagos.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Presidencia.Catalogo_Cajas.Negocio;
using System.Data.OracleClient;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Reportes;
using System.Net;
using System.Text;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Predial_Pae_Honorarios.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Pagos_Predial_Internet : System.Web.UI.Page
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
            if (!IsPostBack)
            {
                Configuracion_Formulario(false);
                Cargar_Combo_Anios();
            }
        }
        catch (Exception Ex)
        {
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
    private void Cargar_Combo_Anios()
    {
        DataTable Dt_Anios = new DataTable();
        DataRow Dr_Anio;
        Dt_Anios.Columns.Add(new DataColumn("Valor",typeof(String)));
        Dt_Anios.Columns.Add(new DataColumn("Texto", typeof(String)));
        Dr_Anio = Dt_Anios.NewRow();
        Dr_Anio["Valor"] = "SELECCIONE";
        Dr_Anio["Texto"] = "<SELECCIONE>";
        Dt_Anios.Rows.Add(Dr_Anio);
        Int32 Anio_Actual = DateTime.Now.Year;
        Int32 Anio_Final = Anio_Actual + 9;
        while (Anio_Actual <= Anio_Final)
        {
            Dr_Anio = Dt_Anios.NewRow();
            Dr_Anio["Valor"] = Anio_Actual.ToString();
            Dr_Anio["Texto"] = Anio_Actual.ToString();
            Dt_Anios.Rows.Add(Dr_Anio);
            Anio_Actual++;
        }
        Cmb_Valido_Anio.DataSource = Dt_Anios;
        Cmb_Valido_Anio.DataValueField = "Valor";
        Cmb_Valido_Anio.DataTextField = "Texto";
        Cmb_Valido_Anio.DataBind();
    }


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
        Hdf_Clave_Operacion.Value = "";
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
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Calculos
    ///DESCRIPCIÓN: Limpia la sección de cálculos
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno  
    ///FECHA_CREO: 18/Enero/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Limpiar_Calculos()
    {
        Txt_Periodo_Rezago.Text = "";
        Txt_Periodo_Actual.Text = "";
        Txt_Total_Recargos_Ordinarios.Text = "";
        Txt_Recargos_Moratorios.Text = "";
        Txt_Adeudo_Rezago.Text = "";
        Txt_Adeudo_Actual.Text = "";
        Txt_Honorarios.Text = "";
        Txt_Gastos_Ejecucion.Text = "";
        Txt_SubTotal.Text = "";
        Txt_Descuento_Corriente.Text = "";
        Txt_Total.Text = "";
        Txt_Ajuste_Tarifario.Text = "";
        Txt_Total_Pagar.Text = "";
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
                        Fila_Nueva["MONTO"] = (!String.IsNullOrEmpty(Dt_Tmp_Parcialidades.Rows[Cnt_Pacialidades]["MONTO"].ToString())) ? Convert.ToInt32(Dt_Tmp_Parcialidades.Rows[Cnt_Pacialidades]["MONTO"]) : 0.0;
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
        Fila_Recargos_Moratorios["CONCEPTO"] = "RECARGOS_MORATORIOS";
        Fila_Recargos_Moratorios["MONTO"] = Convert.ToDouble(Txt_Recargos_Moratorios.Text.Replace("$", "").Trim());
        Dt_Adeudos_Predial.Rows.Add(Fila_Recargos_Moratorios);

        //Agrega la Fila de Descuento Corriente
        DataRow Fila_Descuento_Corriente = Dt_Adeudos_Predial.NewRow();
        Fila_Descuento_Corriente["CONCEPTO"] = "DESCUENTOS_CORRIENTES";
        Fila_Descuento_Corriente["MONTO"] = Convert.ToDouble(Txt_Descuento_Corriente.Text.Replace("$", "").Trim());
        Dt_Adeudos_Predial.Rows.Add(Fila_Descuento_Corriente);

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
    ///NOMBRE DE LA FUNCIÓN: Visibilidad_Controles
    ///DESCRIPCIÓN: Carga la Visibilidad de los controles.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Boolean Validar_Campos_Tarjeta()
    {
        Boolean Aceptado = true;
        String Mensaje_Error= "";
        if (Hdf_Cuenta_Predial_ID.Value.Trim() == "")
        {
            Aceptado = false;
            Mensaje_Error += "Introduzca una Cuenta Predial válida. ";
        }
        if (Txt_Titular_Tarjeta.Text.Trim() == "")
        {
            Aceptado = false;
            Mensaje_Error += "Introduzca el nombre del titular de la tarjeta de crédito. ";
        }
        if (Txt_No_Tarjeta.Text.Trim() == "" || Txt_No_Tarjeta.Text.Trim().Length != 16)
        {
            Aceptado = false;
            Mensaje_Error += "Introduzca el número de tarjeta de crédito de 16 dígitos. ";
        }
        if (Txt_Codigo_Seguridad.Text.Trim() == "")
        {
            Aceptado = false;
            Mensaje_Error += "Introduzca el código de seguridad de 3 dígitos. ";
        }
        if (Cmb_Validez_Mes.SelectedValue == "SELECCIONE")
        {
            Aceptado = false;
            Mensaje_Error += "Seleccione un mes de la vigencia de la tarjeta de crédito. ";
        }
        if (Cmb_Valido_Anio.SelectedValue == "SELECCIONE")
        {
            Aceptado = false;
            Mensaje_Error += "Seleccione un año de la vigencia de la tarjeta de crédito.";
        }
        if (Txt_Total_Pagar.Text.Replace("$", "") == "" || Convert.ToDouble(Txt_Total_Pagar.Text.Replace("$", "")) == 0)
        {
            Aceptado = false;
            Mensaje_Error += "Es necesario presentar un adeudo.";
        }
        if (Mensaje_Error != "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pago de Predial por Internet", "alert('Error: Es necesario "+Mensaje_Error+"');", true);
        }
        return Aceptado;
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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Imprimir_Comprobante_Pago
    ///DESCRIPCIÓN: Realiza la consulta para crear el comprobante de pago en pdf.
    ///PARAMETROS: 
    ///CREO: Ismael Prieto Sánchez  
    ///FECHA_CREO: 26/Abril/2012 1:50pm
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Imprimir_Comprobante_Pago()
    {
        ReportDocument Reporte = new ReportDocument(); // Variable de tipo reporte.
        ExportOptions Opciones_Exportacion = new ExportOptions();
        DiskFileDestinationOptions Direccion_Guardar_Disco = new DiskFileDestinationOptions();
        PdfRtfWordFormatOptions Opciones_Formato_PDF = new PdfRtfWordFormatOptions();
        String Ruta = String.Empty;  // Variable que almacenará la ruta del archivo del crystal report. 
        DataTable Dt_Recibo = new DataTable();
        DataSet Ds_Recibo = new DataSet();
        DataRow Renglon;

        try
        {
            //Crea la estructura de la tabla
            Dt_Recibo.TableName = "Dt_Predial";
            Dt_Recibo.Columns.Add("Cuenta_Predial", typeof(String));
            Dt_Recibo.Columns.Add("Importe", typeof(Double));
            Dt_Recibo.Columns.Add("Fecha_Operacion", typeof(DateTime));
            Dt_Recibo.Columns.Add("Clave_Operacion", typeof(Int64));

            //Asigna el renglon a la tabla
            Renglon = Dt_Recibo.NewRow();
            Renglon["Cuenta_Predial"] = Txt_Cuenta_Predial.Text;
            Renglon["Importe"] = Convert.ToDouble(Txt_Total_Pagar.Text.Replace("$",""));
            Renglon["Fecha_Operacion"] = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            Renglon["Clave_Operacion"] = Convert.ToInt64(Hdf_Clave_Operacion.Value);
            Dt_Recibo.Rows.Add(Renglon);
            Ds_Recibo.Tables.Add(Dt_Recibo);

            // Se crea el nombre del reporte
            String Nombre_Reporte = "Recibo_Pago_" + Txt_Cuenta_Predial.Text.Trim() + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss")) + ".pdf";

            //Asigna la ruta del reporte
            Ruta = HttpContext.Current.Server.MapPath("../Rpt/Cajas/Rpt_Ope_Caj_Impresion_Recibo_Predial_Internet.rpt");
            Reporte.Load(Ruta);

            //Asigna lso datos al reporte
            Reporte.SetDataSource(Ds_Recibo);

            //Asigna la exportacion
            Direccion_Guardar_Disco.DiskFileName = HttpContext.Current.Server.MapPath("../../Reporte/" + Nombre_Reporte);
            Opciones_Exportacion.ExportDestinationOptions = Direccion_Guardar_Disco;
            Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
            Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Opciones_Exportacion);

            //Muestra el reporte en pdf
            Mostrar_Reporte(Nombre_Reporte, "PDF");
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta
    ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 24/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private static Int32 Obtener_Dato_Consulta(ref OracleCommand Cmmd, String Campo, String Tabla, String Condiciones)
    {
        String Mi_SQL;
        Int32 Dato_Consulta = 0;

        try
        {
            Mi_SQL = "SELECT " + Campo;
            if (Tabla != "")
            {
                Mi_SQL += " FROM " + Tabla;
            }
            if (Condiciones != "")
            {
                Mi_SQL += " WHERE " + Condiciones;
            }

            //OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            Cmmd.CommandText = Mi_SQL;
            Dato_Consulta = Convert.ToInt32(Cmmd.ExecuteOracleScalar().ToString());
            if (Convert.IsDBNull(Dato_Consulta))
            {
                Dato_Consulta = 1;
            }
            else
            {
                Dato_Consulta = Dato_Consulta + 1;
            }
        }
        catch (OracleException Ex)
        {
            //Indicamos el mensaje 
            throw new Exception(Ex.ToString());
        }
        return Dato_Consulta;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta
    ///DESCRIPCIÓN          : Consulta los montos de un convenio o reestructura según sea el caso
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 21/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private static DataTable Obtener_Dato_Consulta(String cuenta_predial, Int32 Anio)
    {
        String Mi_SQL;
        DataTable Dt_Montos = new DataTable();

        try
        {
            Mi_SQL = "SELECT ";
            Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ",0) as PAGO_1, ";
            Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ",0) as PAGO_2, ";
            Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ",0) as PAGO_3, ";
            Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ",0) as PAGO_4, ";
            Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ",0) as PAGO_5, ";
            Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ",0) as PAGO_6 ";
            Mi_SQL += "FROM " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + " WHERE " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + "='" + cuenta_predial + "' AND " + Ope_Pre_Adeudos_Predial.Campo_Anio + "=" + Anio;
            DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            if (dataset != null)
            {
                Dt_Montos = dataset.Tables[0];
            }
        }
        catch
        {
        }
        finally
        {
        }

        return Dt_Montos;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta
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
            Mi_SQL = "SELECT " + Cat_Pre_Cuotas_Minimas.Campo_Cuota + " FROM " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas + "  WHERE " + Cat_Pre_Cuotas_Minimas.Campo_Año + "=" + Anio;

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
    #endregion

    #region "Calculos para el Pago"

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Realizar_Calculos_Pago
    ///DESCRIPCIÓN: Realiza los Calculos del Pago.
    ///PARAMETROS: 
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
            Obtener_Recargos_Moratorios();
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
                    //Lbl_Ecabezado_Mensaje.Text = "Verificar el periodo del descuento aplicado.";
                    //Lbl_Mensaje_Error.Text = "No es posible pagar el adeudo ya que tiene un descuento pero no coincide el periodo del mismo con el del pago.";
                    //Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                Porcentaje_Descuento_Recargos_Ordinarios = 0;
                Porcentaje_Descuento_Recargos_Moratorios = 0;
            }

            //Se calcula el Valor de los honorarios
            if (Dt_Honorarios.Rows.Count > 0)
            {
                Honorarios = Convert.ToDouble(Dt_Honorarios.Rows[0][0].ToString());
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
        //Txt_Descuento_Recargos_Ordinarios.Text = "$ " + Descuento_Recargos_Ordinarios.ToString("#,####,###0.00");
        //Txt_Descuento_Recargos_Moratorios.Text = "$ " + Descuento_Recargos_Moratorios.ToString("#,####,###0.00");
        //Txt_Descuento_Honorarios.Text = "$ " + Descuento_Honorarios.ToString("#,####,###0.00");
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
                Txt_Periodo_Actual.Text = Bimestre_Corriente_Inicial + "/" + Anio_Corriente_Inicial + " - " + Bimestre_Corriente_Final + "/" + Anio_Corriente_Final;
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
            Hdf_Cuenta_Predial_ID.Value = "";
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
        Grid_Listado_Adeudos.Visible = true;
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
        Grid_Listado_Adeudos.Visible = false;
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
        Grid_Listado_Adeudos_Convenio.Visible = true;
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
        Grid_Listado_Adeudos_Convenio.Visible = false;
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
    private DataTable Registrar_Pasivos()
    {
        //Eliminar_Pasivos_No_Pagados();

        DataTable Dt_Pasivos = new DataTable();
        DataRow Dr_Renglon;
        Dt_Pasivos.Columns.Add(new DataColumn("Cuenta_Predial",typeof(String)));
        Dt_Pasivos.Columns.Add(new DataColumn("Cuenta_Predial_Id", typeof(String)));
        Dt_Pasivos.Columns.Add(new DataColumn("Clave_Ingreso", typeof(String)));
        Dt_Pasivos.Columns.Add(new DataColumn("Descripcion", typeof(String)));
        Dt_Pasivos.Columns.Add(new DataColumn("Fecha_Tramite", typeof(DateTime)));
        Dt_Pasivos.Columns.Add(new DataColumn("Fecha_Vencimiento", typeof(DateTime)));
        Dt_Pasivos.Columns.Add(new DataColumn("Monto", typeof(Double)));
        Dt_Pasivos.Columns.Add(new DataColumn("Estatus", typeof(String)));
        Dt_Pasivos.Columns.Add(new DataColumn("Dependencia", typeof(String)));
        Dt_Pasivos.Columns.Add(new DataColumn("Contribuyente", typeof(String)));
        Double Impuestos = Obtener_Impuesto_Corriente();
        if (Impuestos > 0)
        {
            Dr_Renglon = Dt_Pasivos.NewRow();
            Dr_Renglon["Cuenta_Predial_Id"] = Hdf_Cuenta_Predial_ID.Value;
            Dr_Renglon["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            if (Hfd_Tipo_Predio.Value == "00001")
            {
                Dr_Renglon["Clave_Ingreso"] = Obtener_Clave_Ingreso("IMPUESTO URBANO");
            }
            else
            {
                Dr_Renglon["Clave_Ingreso"] = Obtener_Clave_Ingreso("IMPUESTO RUSTICO");
            }
            Dr_Renglon["Descripcion"] = "IMPUESTO CORRIENTE [" + Txt_Periodo_Actual.Text.Trim() + "]";
            Dr_Renglon["Fecha_Tramite"] = DateTime.Today;
            Dr_Renglon["Fecha_Vencimiento"] = DateTime.Today;
            Dr_Renglon["Monto"] = Impuestos;
            Dr_Renglon["Estatus"] = "POR PAGAR";
            Dr_Renglon["Dependencia"] = Obtener_Dependencia_Clave_Ingreso(Dr_Renglon["Clave_Ingreso"].ToString());
            Dr_Renglon["Contribuyente"] = Txt_Propietario.Text;
            Dt_Pasivos.Rows.Add(Dr_Renglon);
        }
        Double Rezago = Obtener_Impuesto_Rezago();
        if (Rezago > 0)
        {
            Dr_Renglon = Dt_Pasivos.NewRow();
            Dr_Renglon["Cuenta_Predial_Id"] = Hdf_Cuenta_Predial_ID.Value;
            Dr_Renglon["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            if (Hfd_Tipo_Predio.Value == "00001")
            {
                Dr_Renglon["Clave_Ingreso"] = Obtener_Clave_Ingreso("REZAGO URBANO");
            }
            else
            {
                Dr_Renglon["Clave_Ingreso"] = Obtener_Clave_Ingreso("REZAGO RUSTICO");
            }
            Dr_Renglon["Descripcion"] = "IMPUESTO REZAGO [" + Txt_Periodo_Rezago.Text.Trim() + "]";
            Dr_Renglon["Fecha_Tramite"] = DateTime.Today;
            Dr_Renglon["Fecha_Vencimiento"] = DateTime.Today;
            Dr_Renglon["Monto"] = Rezago;
            Dr_Renglon["Estatus"] = "POR PAGAR";
            Dr_Renglon["Dependencia"] = Obtener_Dependencia_Clave_Ingreso(Dr_Renglon["Clave_Ingreso"].ToString());
            Dr_Renglon["Contribuyente"] = Txt_Propietario.Text;
            Dt_Pasivos.Rows.Add(Dr_Renglon);
        }
        Double Honorarios = Obtener_Honorarios();
        if (Honorarios > 0)
        {
            Dr_Renglon = Dt_Pasivos.NewRow();
            Dr_Renglon["Cuenta_Predial_Id"] = Hdf_Cuenta_Predial_ID.Value;
            Dr_Renglon["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            Dr_Renglon["Clave_Ingreso"] = Obtener_Clave_Ingreso("HONORARIOS");
            Dr_Renglon["Descripcion"] = "HONORARIOS";
            Dr_Renglon["Fecha_Tramite"] = DateTime.Today;
            Dr_Renglon["Fecha_Vencimiento"] = DateTime.Today;
            Dr_Renglon["Monto"] = Honorarios;
            Dr_Renglon["Estatus"] = "POR PAGAR";
            Dr_Renglon["Dependencia"] = Obtener_Dependencia_Clave_Ingreso(Dr_Renglon["Clave_Ingreso"].ToString());
            Dr_Renglon["Contribuyente"] = Txt_Propietario.Text;
            Dt_Pasivos.Rows.Add(Dr_Renglon);
        }
        Double Recargos_Ordinarios = Obtener_Recargos_Ordinarios();
        if (Recargos_Ordinarios > 0)
        {
            Dr_Renglon = Dt_Pasivos.NewRow();
            Dr_Renglon["Cuenta_Predial_Id"] = Hdf_Cuenta_Predial_ID.Value;
            Dr_Renglon["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            Dr_Renglon["Clave_Ingreso"] = Obtener_Clave_Ingreso("RECARGOS ORDINARIOS");
            Dr_Renglon["Descripcion"] = "RECARGOS ORDINARIOS";
            Dr_Renglon["Fecha_Tramite"] = DateTime.Today;
            Dr_Renglon["Fecha_Vencimiento"] = DateTime.Today;
            Dr_Renglon["Monto"] = Recargos_Ordinarios;
            Dr_Renglon["Estatus"] = "POR PAGAR";
            Dr_Renglon["Dependencia"] = Obtener_Dependencia_Clave_Ingreso(Dr_Renglon["Clave_Ingreso"].ToString());
            Dr_Renglon["Contribuyente"] = Txt_Propietario.Text;
            Dt_Pasivos.Rows.Add(Dr_Renglon);
        }
        Double Recargos_Moratorios = Obtener_Recargos_Moratorios();
        if (Recargos_Moratorios > 0)
        {
            Dr_Renglon = Dt_Pasivos.NewRow();
            Dr_Renglon["Cuenta_Predial_Id"] = Hdf_Cuenta_Predial_ID.Value;
            Dr_Renglon["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            Dr_Renglon["Clave_Ingreso"] = Obtener_Clave_Ingreso("RECARGOS MORATORIOS");
            Dr_Renglon["Descripcion"] = "RECARGOS MORATORIOS";
            Dr_Renglon["Fecha_Tramite"] = DateTime.Today;
            Dr_Renglon["Fecha_Vencimiento"] = DateTime.Today;
            Dr_Renglon["Monto"] = Recargos_Moratorios;
            Dr_Renglon["Estatus"] = "POR PAGAR";
            Dr_Renglon["Dependencia"] = Obtener_Dependencia_Clave_Ingreso(Dr_Renglon["Clave_Ingreso"].ToString());
            Dr_Renglon["Contribuyente"] = Txt_Propietario.Text;
            Dt_Pasivos.Rows.Add(Dr_Renglon);
        }
        return Dt_Pasivos;
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
        Recargos_Ordinarios = Recargos_Ordinarios_Neto - Recargos_Ordinarios_Descuento;
        return Recargos_Ordinarios;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Recargos_Moratorios
    ///DESCRIPCIÓN: Obtiene los Recargos Moratorios Totales.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Double Obtener_Recargos_Moratorios()
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
        for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos.Rows.Count; Contador++)
        {
            if (Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo") != null)
            {
                CheckBox Chk_Seleccion_Adeudo_Tmp = (CheckBox)Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo");
                if (Contador < No_Fila)
                {
                    Chk_Seleccion_Adeudo_Tmp.Checked = true;
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
        if (Validar_Campos_Tarjeta())
        {
            DataTable Dt_Formas_Pago = new DataTable(); //Obtiene las formas de pago con las cuales fue cubierto el importe del ingreso
            DataRow Renglon;
            Cls_Ope_Caj_Pagos_Negocio Rs_Alta_Ope_Caj_Pagos = new Cls_Ope_Caj_Pagos_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
            try
            {
                Dt_Formas_Pago.Columns.Add(new DataColumn("Forma_Pago", typeof(String)));
                Dt_Formas_Pago.Columns.Add(new DataColumn("Monto", typeof(Double)));


                //Agrega los datos de pago por internet
                Renglon = Dt_Formas_Pago.NewRow();
                Renglon["Forma_Pago"] = "INTERNET";
                Renglon["Monto"] = Convert.ToDouble(Txt_Total_Pagar.Text.Replace("$", ""));
                Dt_Formas_Pago.Rows.Add(Renglon);

                //Si tiene ajuste tarifario
                if (Convert.ToDouble(Txt_Ajuste_Tarifario.Text.Replace("$", "")) != 0) //Ajuste tarfario
                {
                    //Agrega los datos del ajuste tarifario
                    Renglon = Dt_Formas_Pago.NewRow();
                    Renglon["Forma_Pago"] = "AJUSTE TARIFARIO";
                    Renglon["Monto"] = Convert.ToDouble(Txt_Ajuste_Tarifario.Text.Replace("$", ""));
                    Dt_Formas_Pago.Rows.Add(Renglon);
                }

                Rs_Alta_Ope_Caj_Pagos.P_No_Recibo = "";
                Rs_Alta_Ope_Caj_Pagos.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                Rs_Alta_Ope_Caj_Pagos.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
                Rs_Alta_Ope_Caj_Pagos.P_Referencia = Txt_Cuenta_Predial.Text.Trim();

                //Consulta el id de la caja para pagos por internet.
                Cls_Ope_Pre_Parametros_Negocio Caja_Pago_Internet = new Cls_Ope_Pre_Parametros_Negocio();
                Cls_Cat_Pre_Cajas_Negocio Caja_Pago = new Cls_Cat_Pre_Cajas_Negocio();
                String Caja_Id;
                Caja_Id = Caja_Pago_Internet.Consultar_Parametro_Caja_Pagos_Internet().Rows[0][Ope_Pre_Parametros.Campo_Caja_Id_Pago_Internet].ToString();
                if (Caja_Id == "")
                {
                    throw new Exception("Error: No se puede realizar el pago, porque no esta establecida la caja de pago, favor de verificarlo.");
                }
                Caja_Pago.P_Caja_ID = Caja_Id;
                Caja_Pago = Caja_Pago.Consultar_Datos_Caja();
                Rs_Alta_Ope_Caj_Pagos.P_Caja_ID = Caja_Pago.P_Caja_ID;
                Rs_Alta_Ope_Caj_Pagos.P_No_Caja = Caja_Pago.P_Numero_De_Caja.ToString();
                Rs_Alta_Ope_Caj_Pagos.P_No_Turno = "";
                Rs_Alta_Ope_Caj_Pagos.P_Fecha_Pago = DateTime.Now;
                Rs_Alta_Ope_Caj_Pagos.P_Ajuste_Tarifario = Convert.ToDouble(Txt_Ajuste_Tarifario.Text.Replace("$",""));
                Rs_Alta_Ope_Caj_Pagos.P_Total_Pagar = Convert.ToDouble(Txt_Total_Pagar.Text.Replace("$", ""));
                Rs_Alta_Ope_Caj_Pagos.P_Empleado_ID = "";
                Rs_Alta_Ope_Caj_Pagos.P_Nombre_Usuario = "";
                Rs_Alta_Ope_Caj_Pagos.P_Monto_Corriente = Convert.ToDouble(Txt_Adeudo_Actual.Text.Replace("$", ""));
                Rs_Alta_Ope_Caj_Pagos.P_Dt_Formas_Pago = Dt_Formas_Pago;
                DataTable Dt_Adeudos = Crear_Tabla_Adeudos();
                DataTable Dt_Totales = Crear_Tabla_Totales();
                Rs_Alta_Ope_Caj_Pagos.P_Dt_Adeudos_Predial_Cajas = Crear_Tabla_Adeudos();
                Rs_Alta_Ope_Caj_Pagos.P_Dt_Adeudos_Predial_Cajas_Detalle = Crear_Tabla_Totales();
                Rs_Alta_Ope_Caj_Pagos.P_Dt_Pasivos = Registrar_Pasivos();
                //Llenado de datos para el pago de tarjeta...
                Rs_Alta_Ope_Caj_Pagos.P_Banco_Codigo_Seguridad = Txt_Codigo_Seguridad.Text;
                Rs_Alta_Ope_Caj_Pagos.P_Banco_Expira_Tarjeta = Cmb_Validez_Mes.SelectedValue + "/" + Cmb_Valido_Anio.SelectedValue;
                Rs_Alta_Ope_Caj_Pagos.P_Banco_No_Tarjeta = Txt_No_Tarjeta.Text;
                Rs_Alta_Ope_Caj_Pagos.P_Banco_Titular_Banco = Txt_Titular_Tarjeta.Text.ToUpper();
                Rs_Alta_Ope_Caj_Pagos.P_Banco_Total_Pagar = Convert.ToDouble(Txt_Total_Pagar.Text.Replace("$", ""));
                Rs_Alta_Ope_Caj_Pagos.Alta_Pago_Internet(); //Da de alta el pago del ingreso
                Hdf_Clave_Operacion.Value = Rs_Alta_Ope_Caj_Pagos.P_Banco_Clave_Operacion;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pago de Predial por Internet", "alert('Pago realizado exitosamente.');", true);
                Imprimir_Comprobante_Pago();
                Limpiar_Generales();
                Limpiar_Calculos();
                Limpiar_Calculos_Pago();
                Limpiar_Campos_Reporte();
                Configuracion_Formulario(false);
                Cmb_Bimestre_Final.Enabled = false;
                Cmb_Anio_Final.Enabled = false;
            }
            catch (Exception Exc)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pago de Predial por Internet", "alert('Error: [" + Exc.Message.Split(new String[]{" ."},StringSplitOptions.None)[0] + "].');", true);
            }
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Regresa a la Bandeja de Caja.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 24 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Cuenta_Click(object sender, ImageClickEventArgs e)
    {
        Cargar_Datos_Cuenta_Predial(Txt_Cuenta_Predial.Text);
        if (Txt_Total_Pagar.Text.Replace("$", "").Trim() != "" && Convert.ToDouble(Txt_Total_Pagar.Text.Replace("$","").Trim()) > 0)
        {
            Configuracion_Formulario(true);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cmb_Tipo_Pago_SelectedIndexChanged
    ///DESCRIPCIÓN: Regresa a la Bandeja de Caja.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 24 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Tipo_Pago_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cmb_Tipo_Pago.Text == "PAGO TOTAL")
        {
            Cmb_Bimestre_Final.SelectedIndex = (Cmb_Bimestre_Final.Items.Count - 1);
            Cmb_Anio_Final.SelectedIndex = (Cmb_Anio_Final.Items.Count - 1);
            Cmb_Bimestre_Final.Enabled = false;
            Cmb_Anio_Final.Enabled = false;
            if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0)
            {
                Grid_Listado_Adeudos.DataSource = new DataTable();
                Grid_Listado_Adeudos.DataBind();
                Llenar_Grid_Adeudos(Hdf_Cuenta_Predial_ID.Value, true);
            }
        }
        else
        {
            Cmb_Bimestre_Final.Enabled = true;
            Cmb_Anio_Final.Enabled = true;
            Cmb_Bimestre_Final.Focus();
        }
    }

    public void Configuracion_Formulario(Boolean Habilidato)
    {
        Txt_Cuenta_Predial.Enabled = true;
        Cmb_Tipo_Pago.SelectedIndex = 0;
        Cmb_Tipo_Pago.Enabled = Habilidato;
        Txt_Codigo_Seguridad.Enabled = Habilidato;
        Txt_Titular_Tarjeta.Enabled = Habilidato;
        Txt_No_Tarjeta.Enabled = Habilidato;
        Cmb_Validez_Mes.Enabled = Habilidato;
        Cmb_Valido_Anio.Enabled = Habilidato;
    }

    #endregion


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
                }

            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio;
    private void Busqueda_Cuentas()
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataSet Ds_Cuenta;
        try
        {
            Resumen_Predio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            //M_Orden_Negocio.P_Contrarecibo = Get_Contra_ID();
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
            }
        }
        catch (Exception Ex)
        {
            //Mensaje_Error(Ex.Message);
        }

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
            Session["Cuenta_Predial_ID"] = dataTable.Rows[0]["Cuenta_Predial_Id"].ToString().Trim();
            Txt_Cuenta_Predial.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString();
            Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial_ID = dataTable.Rows[0]["Cuenta_Predial_ID"].ToString().Trim();
            DataTable Dt_Ultimo_Movimiento = Rs_Consulta_Ope_Resumen_Predio.Consultar_Ultimo_Movimiento();
            if (Dt_Ultimo_Movimiento.Rows.Count > 0)
            {
                if (Dt_Ultimo_Movimiento.Rows[0]["Identificador"].ToString() != string.Empty)
                {
                     Hdf_Movimiento.Value = Dt_Ultimo_Movimiento.Rows[0]["Identificador"].ToString().Trim();
                }
            }

            M_Orden_Negocio.P_Cuenta_Origen = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen].ToString();
            if (dataTable.Rows[0]["Estado_ID_Notificacion"].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Estado_Predio = dataTable.Rows[0]["Estado_ID_Notificacion"].ToString();
                DataTable Dt_Estado_Predio = Rs_Consulta_Ope_Resumen_Predio.Consultar_Estado_Predio_Propietario();
                Hdf_Estado_Prop.Value = Dt_Estado_Predio.Rows[0]["Descripcion"].ToString();
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_ID].ToString()))
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Tasa_Predial_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_ID].ToString();
                DataTable Dt_Tasa = Rs_Consulta_Ope_Resumen_Predio.Consultar_Tasa();
                if (Dt_Tasa.Rows.Count > 0)
                {
                    Hdf_Tasa_Predio.Value = Dt_Tasa.Rows[0]["Descripcion"].ToString();
                }
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Calle_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
                DataTable Dt_Calles = Rs_Consulta_Ope_Resumen_Predio.Consultar_Calle_Generales();
                Hdf_Calle_Prop.Value = Dt_Calles.Rows[0]["Nombre"].ToString();
                Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = Dt_Calles.Rows[0]["Colonia_ID"].ToString();
                DataTable Dt_Colonia = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
                Hdf_Col_Predio.Value = Dt_Colonia.Rows[0]["Nombre"].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString() != string.Empty)
            {
                M_Orden_Negocio.P_Estatus_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString();
            }
            if (dataTable.Rows[0]["Nombre_Calle"].ToString() != "")
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString();
                DataTable Dt_Colonia = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
                Hdf_Calle_Predio.Value = dataTable.Rows[0]["Nombre_Calle"].ToString();
                M_Orden_Negocio.P_Ubicacion_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
            }
            if (dataTable.Rows[0]["Nombre_Colonia"].ToString() != "")
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString();
                DataTable Dt_Colonia = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
                Hdf_Col_Predio.Value = dataTable.Rows[0]["Nombre_Colonia"].ToString();
                M_Orden_Negocio.P_Ubicacion_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
            }
            Hdf_No_Ext_Predio.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
            M_Orden_Negocio.P_Exterior_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
            Hdf_No_Ext_Predio.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
            M_Orden_Negocio.P_Interior_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString() != string.Empty)
            {
                Hdf_efectos_Predio.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString();
            }
            Hdf_Valor_Fiscal_Predio.Value = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal]).ToString("$##,###,###,##0.00");
            M_Orden_Negocio.P_Valor_Fiscal = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal].ToString();
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()))
            {
                Hdf_Cuota_Bimestral.Value = "$ " + String.Format("{0:##,###,###,##0.00}", Convert.ToDecimal(Convert.ToDecimal(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()) / 6));
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual] != null)
            {
                M_Orden_Negocio.P_Cuota_Anual = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString());
                M_Orden_Negocio.P_Cuota_Bimestral = ((Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString())) / 6);
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion] != null)
            {
                M_Orden_Negocio.P_Exencion = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString());
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID] != null)
            {
                M_Orden_Negocio.P_Cuota_Minima = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString());
            }
            //Z1 HAY KU07A FIJ4!!! Seccion de carga de datos de la cuota fija
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija] != null)
            {

                Cargar_Datos_Cuota_Fija(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString());
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString()))
            {
                M_Orden_Negocio.P_Tasa = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString());
                Hdf_Tasa_Id.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString() != String.Empty)
            {
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0]["Estado_ID_Notificacion"].ToString()))
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Estado_Predio = (dataTable.Rows[0]["Estado_ID_Notificacion"].ToString());
                DataTable Dt_Estado_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Estado_Predio();
                //v
                if (Dt_Estado_Propietario.Rows.Count > 0)
                {
                    Hdf_Estado_Prop.Value = (Dt_Estado_Propietario.Rows[0]["Descripcion"].ToString());
                    M_Orden_Negocio.P_Estado_Propietario = (Dt_Estado_Propietario.Rows[0]["Descripcion"].ToString());
                }
            }
            else if (!String.IsNullOrEmpty(dataTable.Rows[0]["Estado_Notificacion"].ToString()))
            {
                Hdf_Estado_Prop.Value = dataTable.Rows[0]["Estado_Notificacion"].ToString();
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0]["Colonia_ID_Notificacion"].ToString()))
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = dataTable.Rows[0]["Colonia_ID_Notificacion"].ToString();
                DataTable DT_Colonia_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
                Hdf_Colonia_Prop.Value = DT_Colonia_Propietario.Rows[0]["Nombre"].ToString();
            }


            if (!String.IsNullOrEmpty(dataTable.Rows[0]["Ciudad_ID_Notificacion"].ToString()))
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Ciudad_ID = dataTable.Rows[0]["Ciudad_ID_Notificacion"].ToString();
                DataTable Dt_Ciudad_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Ciudad();
                Hdf_Ciudad_Prop.Value = Dt_Ciudad_Propietario.Rows[0]["Nombre"].ToString();
                M_Orden_Negocio.P_Ciudad_Propietario = dataTable.Rows[0]["Ciudad_ID_Notificacion"].ToString();
            }
            else if (!String.IsNullOrEmpty(dataTable.Rows[0]["Ciudad_Notificacion"].ToString()))
            {
                Hdf_Ciudad_Prop.Value = dataTable.Rows[0]["Ciudad_Notificacion"].ToString();
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0]["Calle_ID_Notificacion"].ToString()))
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Calle_ID = dataTable.Rows[0]["Calle_ID_Notificacion"].ToString();//*
                DataTable Dt_Calle_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Calle_Generales();
                Hdf_Calle_Prop.Value = Dt_Calle_Propietario.Rows[0]["Nombre"].ToString();
            }
            Hdf_No_Ext_Prop.Value = dataTable.Rows[0]["No_Exterior_Notificacion"].ToString();
            M_Orden_Negocio.P_Exterior_Propietario = dataTable.Rows[0]["No_Exterior_Notificacion"].ToString();
            Hdf_No_Int_Prop.Value = dataTable.Rows[0]["No_Interior_Notificacion"].ToString();
            M_Orden_Negocio.P_Interior_Propietario = dataTable.Rows[0]["No_Interior_Notificacion"].ToString();
            Hdf_Cod_Postal_Prop.Value = dataTable.Rows[0]["Codigo_Postal"].ToString();
            M_Orden_Negocio.P_CP_Propietario = dataTable.Rows[0]["Codigo_Postal"].ToString();
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
            Dt_Cuota_Detalles = M_Orden_Negocio.Consultar_Cuota_Fija_Detalles();
            if (Dt_Cuota_Detalles.Rows.Count - 1 > 0)
            {
                if (Dt_Cuota_Detalles.Rows[0]["Tasa_Valor"].ToString() != string.Empty)
                {
                    Hdf_Tasa_Predio.Value = Dt_Cuota_Detalles.Rows[0]["Tasa_Valor"].ToString();
                }
            }

        }
        catch (Exception Ex)
        {
        }
    }

    /// *************************************************************************************
    /// NOMBRE:              Img_Btn_Imprimir_Estado_Cuenta_Click
    /// DESCRIPCIÓN:         Genera el reporte de los datos del estado de cuenta
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          29/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    protected void Img_Btn_Imprimir_Estado_Cuenta_Click(object sender, ImageClickEventArgs e)
    {
        if (Hdf_Cuenta_Predial_ID.Value.Trim() != "")
        {
            Limpiar_Campos_Reporte();
            Cargar_Datos();
            String Nombre_Reporte = String.Empty;
            String Nombre_Repote_Crystal = String.Empty;
            String Formato = String.Empty;
            //instacia la clase de negocio
            Cls_Ope_Pre_Resumen_Predio_Negocio Imprimir_Resumen_Predio_Estado_Cuenta = new Cls_Ope_Pre_Resumen_Predio_Negocio();
            Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
            Int32 Anio_Corriente = 0;
            Cls_Ope_Pre_Parametros_Negocio Rs_Parametros = new Cls_Ope_Pre_Parametros_Negocio();

            Anio_Corriente = Rs_Parametros.Consultar_Anio_Corriente();

            DataTable Dt_Estado_Cuenta = Resumen_Predio.Consultar_Adeudos_Cuenta_Predial_Con_Totales(Txt_Cuenta_Predial.Text.Trim(), null, 0, Convert.ToInt16(Anio_Corriente));
            //Crea un nuevo data table
            DataTable Dt_Generales = new DataTable();
            DataTable Dt_Propietario = new DataTable();
            DataTable Dt_Impuestos = new DataTable();
            DataTable Dt_Grid_Estado_Cuenta = Dt_Estado_Cuenta;
            //LLenado de datos
            Dt_Estado_Cuenta = Asignar_Datos_Estado_Cuenta();
            //instancia el data set que contiene el data table 
            Ds_Pre_Resumen_Predio_Generales Resumen_Predio_Estado_Cuenta = new Ds_Pre_Resumen_Predio_Generales();
            //obtiene el numero de cuenta predial
            Imprimir_Resumen_Predio_Estado_Cuenta.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            //manda a llamar a la consulta para traer los datos
            Dt_Generales = Imprimir_Resumen_Predio_Estado_Cuenta.Consultar_Imprimir_Resumen_Generales();
            Dt_Impuestos = Asignar_Datos_Impuestos();
            Dt_Propietario = Asignar_Datos_Propietarios();
            Dt_Generales.TableName = "Dt_Generales";
            Dt_Estado_Cuenta.TableName = "Dt_Estado_Cuenta";
            Dt_Propietario.TableName = "Dt_Propietario";
            Dt_Impuestos.TableName = "Dt_Impuestos";
            Dt_Grid_Estado_Cuenta.TableName = "Dt_Grid_Estado_Cuenta";
            Resumen_Predio_Estado_Cuenta.Clear();
            Resumen_Predio_Estado_Cuenta.Tables.Clear();
            Resumen_Predio_Estado_Cuenta.Tables.Add(Dt_Generales.Copy());
            Resumen_Predio_Estado_Cuenta.Tables.Add(Dt_Estado_Cuenta.Copy());
            Resumen_Predio_Estado_Cuenta.Tables.Add(Dt_Propietario.Copy());
            Resumen_Predio_Estado_Cuenta.Tables.Add(Dt_Impuestos.Copy());
            Resumen_Predio_Estado_Cuenta.Tables.Add(Dt_Grid_Estado_Cuenta.Copy());
            Nombre_Reporte = "Reporte_Estado_Cuenta";
            Nombre_Repote_Crystal = "Rpt_Pre_Resumen_Predio_Estado_Cuenta.Rpt";
            Formato = "PDF";
            //llama el metodo con los parametros de la consulta y el data set
            Generar_Reportes(Resumen_Predio_Estado_Cuenta, Nombre_Repote_Crystal, Nombre_Reporte, Formato);
        }
    }

    /// *************************************************************************************
    /// NOMBRE:              Asignar_Datos_Estado_Cuenta
    /// DESCRIPCIÓN:         Metodo para asignar los datos de los propietarios a una datatable
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          26/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    Miguel Angel Bedolla Moreno
    /// FECHA MODIFICO:      24/Enero/2012
    /// CAUSA MODIFICACIÓN:  La fuente de información cambio.
    /// *************************************************************************************
    protected DataTable Asignar_Datos_Estado_Cuenta()
    {
        String Total_Construccion = String.Empty;
        DataTable Dt_Estado_Cuenta = new DataTable();
        try
        {

            DataRow Estado_Cuenta;
            Dt_Estado_Cuenta.Columns.Add("Periodo_Rezago");
            Dt_Estado_Cuenta.Columns.Add("Adeudo_Rezago", typeof(Double));
            Dt_Estado_Cuenta.Columns.Add("Periodo_Actual");
            Dt_Estado_Cuenta.Columns.Add("Adeudo_Actual", typeof(Double));
            Dt_Estado_Cuenta.Columns.Add("Total_Recargos_Ordinarios", typeof(Double));
            Dt_Estado_Cuenta.Columns.Add("Honorarios", typeof(Double));
            Dt_Estado_Cuenta.Columns.Add("Recargos_Moratorios", typeof(Double));
            Dt_Estado_Cuenta.Columns.Add("Gastos_Ejecucion", typeof(Double));
            Dt_Estado_Cuenta.Columns.Add("Subtotal", typeof(Double));
            Dt_Estado_Cuenta.Columns.Add("Descuentos_Pronto_Pago");
            Dt_Estado_Cuenta.Columns.Add("Descuento_Recargos_Ordinarios");
            Dt_Estado_Cuenta.Columns.Add("Descuento_Recargos_Moratorios");
            Dt_Estado_Cuenta.Columns.Add("Descuento_Honorarios");
            Dt_Estado_Cuenta.Columns.Add("Total", typeof(Double));

            Estado_Cuenta = Dt_Estado_Cuenta.NewRow();
            Estado_Cuenta["Periodo_Rezago"] = Txt_Periodo_Rezago.Text.Trim();
            Estado_Cuenta["Periodo_Actual"] = Txt_Periodo_Actual.Text.Trim();
            Estado_Cuenta["Total_Recargos_Ordinarios"] = Txt_Total_Recargos_Ordinarios.Text.Trim().Replace("$", "").Replace(",", "");
            Estado_Cuenta["Honorarios"] = Txt_Honorarios.Text.Trim().Replace("$", "").Replace(",", "");
            Estado_Cuenta["Recargos_Moratorios"] = Txt_Recargos_Moratorios.Text.Trim().Replace("$", "").Replace(",", "");
            Estado_Cuenta["Gastos_Ejecucion"] = Txt_Gastos_Ejecucion.Text.Trim().Replace("$", "").Replace(",", "");
            Estado_Cuenta["Subtotal"] = Txt_Total_Pagar.Text.Trim().Replace("$", "").Replace(",", "");
            Estado_Cuenta["Descuentos_Pronto_Pago"] = Txt_Descuento_Corriente.Text.Trim();
            //Detalles
            Estado_Cuenta["Adeudo_Rezago"] = Txt_Adeudo_Rezago.Text.Trim().Replace("$", "").Replace(",", "");
            Estado_Cuenta["Adeudo_Actual"] = Txt_Adeudo_Actual.Text.Trim().Replace("$", "").Replace(",", "");
            Estado_Cuenta["Descuento_Recargos_Ordinarios"] = "0";
            Estado_Cuenta["Descuento_Recargos_Moratorios"] = "0";
            Estado_Cuenta["Total"] = Txt_Total_Pagar.Text.Trim().Replace("$", "").Replace(",", "");

            if (Dt_Estado_Cuenta.Rows.Count == 0)
            {
                Dt_Estado_Cuenta.Rows.InsertAt(Estado_Cuenta, 0);

            }
        }
        catch (Exception EX)
        {
        }
        return Dt_Estado_Cuenta;

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
            Dt_Impuestos.Columns.Add("Valor_Fiscal", typeof(Double));
            Dt_Impuestos.Columns.Add("Tasa");
            Dt_Impuestos.Columns.Add("Periodo_Corriente");
            Dt_Impuestos.Columns.Add("Tipo_Predio");
            Dt_Impuestos.Columns.Add("Cuota_Bimestral", typeof(Double));


            Impuestos = Dt_Impuestos.NewRow();
            Impuestos["Valor_Fiscal"] = Hdf_Valor_Fiscal_Predio.Value.Trim().Replace("$", "").Replace(",", "");
            Impuestos["Tasa"] = Hdf_Tasa_Predio.Value.Trim();
            Impuestos["Tipo_Predio"] = "";
            Impuestos["Cuota_Bimestral"] = Hdf_Cuota_Bimestral.Value.Trim().Replace("$", "").Replace(",", "");
            if (Dt_Impuestos.Rows.Count == 0)
            {
                Dt_Impuestos.Rows.InsertAt(Impuestos, 0);

            }

        }
        catch (Exception EX)
        {
        }
        return Dt_Impuestos;

    }

    /// *************************************************************************************
    /// NOMBRE:              Asignar_Datos_Propietarios
    /// DESCRIPCIÓN:         Metodo para asignar los datos de los propietarios a una datatable
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          26/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    Miguel Angel Bedolla Moreno    
    /// FECHA MODIFICO:      24/Enero/2012
    /// CAUSA MODIFICACIÓN:  Se cambio la fuente de información
    /// *************************************************************************************
    protected DataTable Asignar_Datos_Propietarios()
    {
        DataTable Dt_Propietario = new DataTable();
        DataRow Propietarios;
        Dt_Propietario.Columns.Add("Nombre");

        Dt_Propietario.Columns.Add("Rfc");
        Dt_Propietario.Columns.Add("Colonia");
        Dt_Propietario.Columns.Add("Calle");
        Dt_Propietario.Columns.Add("Numero_Exterior");
        Dt_Propietario.Columns.Add("Numero_Interior");
        Dt_Propietario.Columns.Add("Estado");
        Dt_Propietario.Columns.Add("Ciudad");
        Dt_Propietario.Columns.Add("Cod_Pos");
        Propietarios = Dt_Propietario.NewRow();
        Propietarios["Nombre"] = Txt_Propietario.Text.Trim();

        Propietarios["Rfc"] = Hdf_RFC_Prop.Value.Trim();
        Propietarios["Colonia"] = Hdf_Colonia_Prop.Value.Trim();
        Propietarios["Calle"] = Hdf_Calle_Prop.Value.Trim();
        Propietarios["Numero_Exterior"] = Hdf_No_Ext_Prop.Value.Trim();
        Propietarios["Numero_Interior"] = Hdf_No_Int_Prop.Value.Trim();
        Propietarios["Estado"] = Hdf_Estado_Prop.Value.Trim();
        Propietarios["Ciudad"] = Hdf_Ciudad_Prop.Value.Trim();
        Propietarios["Cod_Pos"] = Hdf_Cod_Postal_Prop.Value.Trim();
        if (Dt_Propietario.Rows.Count == 0)
        {
            Dt_Propietario.Rows.InsertAt(Propietarios, 0);

        }
        return Dt_Propietario;

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

    protected void Limpiar_Campos_Reporte()
    {
        //Propietario
        Hdf_Calle_Prop.Value = "";
        Hdf_Colonia_Prop.Value = "";
        Hdf_Ciudad_Prop.Value = "";
        Hdf_Estado_Prop.Value = "";
        Hdf_No_Ext_Prop.Value = "";
        Hdf_No_Int_Prop.Value = "";
        Hdf_RFC_Prop.Value = "";
        Hdf_Cod_Postal_Prop.Value = "";
        //Predio
        Hdf_Calle_Predio.Value = "";
        Hdf_Col_Predio.Value = "";
        Hdf_No_Ext_Predio.Value = "";
        Hdf_No_Int_Predio.Value = "";
        Hdf_Valor_Fiscal_Predio.Value = "";
        Hdf_efectos_Predio.Value = "";
        Hdf_Movimiento.Value = "";
        Hdf_Tasa_Predio.Value = "";
        Hdf_Tasa_Id.Value = "";
        Hdf_Cuota_Bimestral.Value = "";
        //Pago
        Txt_Titular_Tarjeta.Text = "";
        Txt_No_Tarjeta.Text = "";
        Txt_Codigo_Seguridad.Text = "";
        Cmb_Validez_Mes.SelectedIndex = 0;
        Cmb_Valido_Anio.SelectedIndex = 0;
    }

    protected void Txt_Cuenta_Predial_TextChanged(object sender, EventArgs e)
    {
        Cargar_Datos_Cuenta_Predial(Txt_Cuenta_Predial.Text);
        if (Hdf_Cuenta_Predial_ID.Value != "")
        {
            if (Txt_Total_Pagar.Text.Replace("$", "").Trim() != "" && Convert.ToDouble(Txt_Total_Pagar.Text.Replace("$", "").Trim()) > 0)
            {
                Configuracion_Formulario(true);
                Cmb_Tipo_Pago.Focus();
            }
        }
        else
        {
            Configuracion_Formulario(false);
            Limpiar_Generales();
            Limpiar_Calculos();
        }
    }

    /// *************************************************************************************
    /// NOMBRE:              Btn_Imprimir_Estado_Cuenta_Click
    /// DESCRIPCIÓN:         Manda imprimir el estado de cuenta.
    /// PARÁMETROS:          
    /// USUARIO CREO:        Christian
    /// FECHA CREO:          
    /// USUARIO MODIFICO:    Miguel Angel Bedolla Moreno
    /// FECHA MODIFICO:      24-Enero-2012
    /// CAUSA MODIFICACIÓN:  Se cambio la fuente de la información.
    /// *************************************************************************************
    protected void Btn_Imprimir_Estado_Cuenta_Click(object sender, EventArgs e)
    {
        if (Hdf_Cuenta_Predial_ID.Value.Trim() != "")
        {
            Limpiar_Campos_Reporte();
            Cargar_Datos();
            String Nombre_Reporte = String.Empty;
            String Nombre_Repote_Crystal = String.Empty;
            String Formato = String.Empty;
            //instacia la clase de negocio
            Cls_Ope_Pre_Resumen_Predio_Negocio Imprimir_Resumen_Predio_Estado_Cuenta = new Cls_Ope_Pre_Resumen_Predio_Negocio();
            Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
            Int32 Anio_Corriente = 0;
            Cls_Ope_Pre_Parametros_Negocio Rs_Parametros = new Cls_Ope_Pre_Parametros_Negocio();

            Anio_Corriente = Rs_Parametros.Consultar_Anio_Corriente();

            DataTable Dt_Estado_Cuenta = Resumen_Predio.Consultar_Adeudos_Cuenta_Predial_Con_Totales(Txt_Cuenta_Predial.Text.Trim(), null, 0, Convert.ToInt16(Anio_Corriente));
            //Crea un nuevo data table
            DataTable Dt_Generales = new DataTable();
            DataTable Dt_Propietario = new DataTable();
            DataTable Dt_Impuestos = new DataTable();
            DataTable Dt_Grid_Estado_Cuenta = Dt_Estado_Cuenta;
            //LLenado de datos
            Dt_Estado_Cuenta = Asignar_Datos_Estado_Cuenta();
            //instancia el data set que contiene el data table 
            Ds_Pre_Resumen_Predio_Generales Resumen_Predio_Estado_Cuenta = new Ds_Pre_Resumen_Predio_Generales();
            //obtiene el numero de cuenta predial
            Imprimir_Resumen_Predio_Estado_Cuenta.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            //manda a llamar a la consulta para traer los datos
            Dt_Generales = Imprimir_Resumen_Predio_Estado_Cuenta.Consultar_Imprimir_Resumen_Generales();
            Dt_Impuestos = Asignar_Datos_Impuestos();
            Dt_Propietario = Asignar_Datos_Propietarios();
            Dt_Generales.TableName = "Dt_Generales";
            Dt_Estado_Cuenta.TableName = "Dt_Estado_Cuenta";
            Dt_Propietario.TableName = "Dt_Propietario";
            Dt_Impuestos.TableName = "Dt_Impuestos";
            Dt_Grid_Estado_Cuenta.TableName = "Dt_Grid_Estado_Cuenta";
            Resumen_Predio_Estado_Cuenta.Clear();
            Resumen_Predio_Estado_Cuenta.Tables.Clear();
            Resumen_Predio_Estado_Cuenta.Tables.Add(Dt_Generales.Copy());
            Resumen_Predio_Estado_Cuenta.Tables.Add(Dt_Estado_Cuenta.Copy());
            Resumen_Predio_Estado_Cuenta.Tables.Add(Dt_Propietario.Copy());
            Resumen_Predio_Estado_Cuenta.Tables.Add(Dt_Impuestos.Copy());
            Resumen_Predio_Estado_Cuenta.Tables.Add(Dt_Grid_Estado_Cuenta.Copy());
            Nombre_Reporte = "Reporte_Estado_Cuenta";
            Nombre_Repote_Crystal = "Rpt_Pre_Resumen_Predio_Estado_Cuenta.Rpt";
            Formato = "PDF";
            //llama el metodo con los parametros de la consulta y el data set
            Generar_Reportes(Resumen_Predio_Estado_Cuenta, Nombre_Repote_Crystal, Nombre_Reporte, Formato);
        }
    }
}