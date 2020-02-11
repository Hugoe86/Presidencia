using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.IO;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.text.html;
//using iTextSharp.text.html.simpleparser;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Recepcion_Pagos.Negocio;
using Presidencia.Operacion_Atencion_Ciudadana_Pagos_Internet.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Presidencia.Catalogo_Cajas.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Net;
using System.Text;
using Presidencia.Ope_Pre_Impresion_Recibo_Negocio;
using System.Collections.Specialized;
using Presidencia.Sessiones;

public partial class paginas_Atencion_Ciudadana_Frm_Ope_Ven_Pagos_Internet : System.Web.UI.Page
{
    #region PAGE LOAD

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
            ScriptManager.RegisterPostBackControl(Btn_Ejecutar_Pago);
            if (!IsPostBack)
            {
                Configuracion_Formulario(false);
                Cargar_Combo_Anios();
                Hdf_3D_Estatus.Value = "";
                //Valida la respuesta del 3D
                if (Request.Form["Status"] != null) //No sea nula
                {
                    if (Request.Form["Status"].ToString().Trim() != "") //No este vacia
                    {
                        //Inhabilita los botones
                        Btn_Ejecutar_Pago.Enabled = false;
                        Limpiar_Campos_3D();
                        //Estatus de la respuesa
                        if (Request.Form["Status"].ToString().Trim() != "")
                        {
                            Hdf_3D_Estatus.Value = Request.Form["Status"].ToString().Trim();
                        }
                        else
                        {
                            Hdf_3D_Estatus.Value = "0";
                        }
                        //Datos del folio
                        Txt_Folio_Pago.Text = Request.Form["Folio"].ToString().Trim();
                        if (Txt_Folio_Pago.Text != "")
                        {
                            Txt_Folio_Pago_TextChanged(sender, null);
                        }
                        //Datos del pago
                        Cmb_Tipo_Tarjeta.SelectedValue = Request.Form["TipoTarjeta"].ToString().Trim();
                        Txt_Titular_Tarjeta.Text = Request.Form["BillToFirstName"].ToString().Trim();
                        Txt_No_Tarjeta.Text = Request.Form["Number"].ToString().Trim();
                        Txt_Codigo_Seguridad.Text = Request.Form["Cvv2Val"].ToString().Trim();
                        Cmb_Validez_Mes.SelectedValue = Request.Form["Expires"].ToString().Trim().Substring(0, 2);
                        Cmb_Valido_Anio.SelectedValue = Request.Form["Expires"].ToString().Trim().Substring(3);
                        //Datos de la respuesta
                        if (Convert.ToInt64(Hdf_3D_Estatus.Value) != 200) //Si trae error
                        {
                            //habilita los botones
                            Btn_Ejecutar_Pago.Enabled = true;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pagos Externos", "alert('" + Mensaje_Error_3D(Convert.ToInt64(Hdf_3D_Estatus.Value)) + "');", true);
                        }
                        else //Si es exitosa
                        {
                            //Asigna los datos de la respuesta
                            if (Request.Form["ECI"] != null)
                            {
                                Hdf_3D_ECI.Value = Request.Form["ECI"].ToString().Trim();
                                //Valida los posibles valores
                                if (!String.IsNullOrEmpty(Hdf_3D_ECI.Value))
                                {
                                    if (Hdf_3D_ECI.Value != "05" && Hdf_3D_ECI.Value != "06" && Hdf_3D_ECI.Value != "07" && Hdf_3D_ECI.Value != "01" && Hdf_3D_ECI.Value != "02")
                                    {
                                        //habilita los botones
                                        Btn_Ejecutar_Pago.Enabled = true;
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pagos Externos", "alert('El código ECI no esta dentro de los valores permitidos, favor de intentar nuevamente.');", true);
                                        return;
                                    }
                                }
                            }
                            if (Request.Form["CardType"] != null)
                            {
                                Hdf_3D_CardType.Value = Request.Form["CardType"].ToString().Trim();
                            }
                            if (Request.Form["XID"] != null)
                            {
                                Hdf_3D_XID.Value = Request.Form["XID"].ToString().Trim();
                            }
                            if (Request.Form["CAVV"] != null)
                            {
                                Hdf_3D_CAVV.Value = Request.Form["CAVV"].ToString().Trim();
                            }
                            //Realiza la ejecucion del pago
                            Ejecutar_Pago();
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    #endregion PAGE LOAD

    #region METODOS

    #region "Generales"

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Anios
    ///DESCRIPCIÓN: Carga el combo años de vigencia de tarjeta hasta el año actual más nueve
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
        Dt_Anios.Columns.Add(new DataColumn("Valor", typeof(String)));
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
            Dr_Anio["Valor"] = Anio_Actual.ToString().Substring(2, 2);
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
        Hdf_Clave_Operacion.Value = "";
        Txt_Folio_Pago.Text = "";
        Txt_Solicitante.Text = "";
        Txt_Concepto.Text = "";
        Txt_Total_Pagar.Text = "";
        Txt_Importe.Text = "";
        Txt_Ajuste_Tarifario.Text = "";
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
        Dt_Adeudos_Predial.Columns.Add("MONTO", Type.GetType("System.Decimal"));
        Dt_Adeudos_Predial.Columns.Add("REFERENCIA", Type.GetType("System.String"));
        decimal Monto_Pagar;
        decimal.TryParse(Txt_Total_Pagar.Text.Replace("$", "").Trim(), out Monto_Pagar);
        //Agrega la Fila de Importe Corriente
        DataRow Fila_Corriente = Dt_Adeudos_Predial.NewRow();
        Fila_Corriente["CONCEPTO"] = "CORRIENTE";
        Fila_Corriente["MONTO"] = Monto_Pagar;
        Dt_Adeudos_Predial.Rows.Add(Fila_Corriente);

        return Dt_Adeudos_Predial;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Validar_Campos_Tarjeta
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
        String Mensaje_Error = "";

        if (Txt_Folio_Pago.Text == "")
        {
            Aceptado = false;
            Mensaje_Error += "Introduzca una folio válido. ";
        }
        if (Cmb_Tipo_Tarjeta.SelectedItem.Value == "")
        {
            Aceptado = false;
            Mensaje_Error += "Seleccione el tipo de tarjeta bancaria. ";
        }
        if (Txt_Titular_Tarjeta.Text.Trim() == "")
        {
            Aceptado = false;
            Mensaje_Error += "Introduzca el nombre del titular de la tarjeta bancaria. ";
        }
        if (Txt_No_Tarjeta.Text.Trim() == "" || Txt_No_Tarjeta.Text.Trim().Length != 16)
        {
            Aceptado = false;
            Mensaje_Error += "Introduzca el número de tarjeta bancaria de 16 dígitos. ";
        }
        if (Txt_Codigo_Seguridad.Text.Trim() == "")
        {
            Aceptado = false;
            Mensaje_Error += "Introduzca el código de seguridad de 3 dígitos. ";
        }
        if (Cmb_Validez_Mes.SelectedValue == "SELECCIONE")
        {
            Aceptado = false;
            Mensaje_Error += "Seleccione un mes de la vigencia de la tarjeta bancaria. ";
        }
        if (Cmb_Valido_Anio.SelectedValue == "SELECCIONE")
        {
            Aceptado = false;
            Mensaje_Error += "Seleccione un año de la vigencia de la tarjeta bancaria.";
        }
        decimal Total_Pagar;
        if (!decimal.TryParse(Txt_Total_Pagar.Text.Replace("$", "").Trim(), out Total_Pagar) || Total_Pagar <= 0)
        {
            Aceptado = false;
            Mensaje_Error += "Es necesario presentar un adeudo.";
        }
        if (Mensaje_Error != "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pago por Internet", "alert('Error: Es necesario " + Mensaje_Error + "');", true);
        }
        return Aceptado;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Crear_Dataset_Solicitud
    ///DESCRIPCIÓN: Genera un dataset con los datos en el formulario
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 29-jun-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Dataset_Solicitud()
    {
        var Ds_Pago_Solicitud = new Ds_Ope_Ven_Recibo_Pago_Internet();
        DataRow Dr_Dato_Solicitud = Ds_Pago_Solicitud.Tables["Dt_Recibo_Pago"].NewRow();
        decimal Monto_Importe;
        decimal Monto_Total;
        DateTime Fecha_Hoy = DateTime.Now;

        decimal.TryParse(Txt_Importe.Text.Replace("$", ""), out Monto_Importe);
        decimal.TryParse(Txt_Total_Pagar.Text.Replace("$", ""), out Monto_Total);

        Dr_Dato_Solicitud["Folio"] = Txt_Folio_Pago.Text;
        Dr_Dato_Solicitud["Solicitante"] = Txt_Solicitante.Text;
        Dr_Dato_Solicitud["Concepto"] = Txt_Concepto.Text;
        Dr_Dato_Solicitud["Importe"] = Monto_Importe;
        Dr_Dato_Solicitud["Total"] = Monto_Total;
        Dr_Dato_Solicitud["Fecha"] = Fecha_Hoy;
        // agregar fila a la tabla
        Ds_Pago_Solicitud.Tables["Dt_Recibo_Pago"].Rows.Add(Dr_Dato_Solicitud);

        return Ds_Pago_Solicitud;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Imprimir_Comprobante_Pago
    ///DESCRIPCIÓN: Genera un reporte de crystal como pdf con la información en el dataset que recibe como parámetro
    ///PARAMETROS: 
    ///     1. Ds_Recibo: dataset con información para el reporte
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 29-jun-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Imprimir_Comprobante_Pago(DataSet Ds_Recibo)
    {
        ReportDocument Reporte = new ReportDocument(); // Variable de tipo reporte.
        ExportOptions Opciones_Exportacion = new ExportOptions();
        DiskFileDestinationOptions Direccion_Guardar_Disco = new DiskFileDestinationOptions();
        PdfRtfWordFormatOptions Opciones_Formato_PDF = new PdfRtfWordFormatOptions();
        String Ruta = String.Empty;  // Variable que almacenará la ruta del archivo del crystal report. 

        try
        {
            // Se crea el nombre del reporte
            String Nombre_Reporte = "Recibo_Pago_" + Txt_Folio_Pago.Text.Trim() + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss")) + ".pdf";

            //Asigna la ruta del reporte
            Ruta = HttpContext.Current.Server.MapPath("../Rpt/Ventanilla/Rpt_Ope_Ven_Recibo_Pago_Internet.rpt");
            Reporte.Load(Ruta);

            //Asigna los datos al reporte
            Reporte.SetDataSource(Ds_Recibo);

            //Asigna la exportación
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
        String Pagina = "Frm_Ope_Ven_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            if (Formato == "PDF")
            {
                Pagina = Pagina + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
                "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600');", true);
            }
            else if (Formato == "Excel")
            {
                String Ruta = "../../Reporte/" + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600');", true);
            }
            //////String strFilename = Nombre_Reporte_Generar;
            //////String mstrExportType = "";
            //////String mstrExportFile = "";
            //////String strExportPath = "../../Reporte/";

            //////switch (Formato)
            //////{
            //////    case "RTF":
            //////        strFilename += ".rtf";
            //////        mstrExportType = "Application/rtf";
            //////        mstrExportFile = strExportPath + strFilename;
            //////        break;
            //////    case "PDF":
            //////        strFilename += ".pdf";
            //////        mstrExportType = "Application/pdf";
            //////        mstrExportFile = strExportPath + strFilename;
            //////        break;

            //////    case "WORD":
            //////        strFilename += ".doc";
            //////        mstrExportType = "Application/msword";
            //////        mstrExportFile = strExportPath + strFilename;
            //////        break;

            //////    case "EXCEL":
            //////        strFilename += ".xls";
            //////        mstrExportType = "Application/x-msexcel";
            //////        mstrExportFile = strExportPath + strFilename;
            //////        break;

            //////}

            //////Response.Clear();
            //////Response.ClearContent();
            //////Response.ClearHeaders();
            //////Response.ContentType = mstrExportType;
            //////Response.AddHeader("Content-Disposition", "inline; filename=" + strFilename);
            //////Response.TransmitFile(mstrExportFile);
            //////Response.End();
            //Response.Flush();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Se configuran los controles para ingreso de datos de pago
    ///PARÁMETROS:
    /// 		1. Habilidato: variable de tipo boleano que se utiliza para habilitar o deshabilitar los controles
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 28-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Configuracion_Formulario(bool Habilidato)
    {
        Txt_Folio_Pago.Enabled = true;
        Txt_Codigo_Seguridad.Enabled = Habilidato;
        Txt_Titular_Tarjeta.Enabled = Habilidato;
        Txt_No_Tarjeta.Enabled = Habilidato;
        Cmb_Tipo_Tarjeta.SelectedIndex = 0;
        Cmb_Tipo_Tarjeta.Enabled = Habilidato;
        Cmb_Validez_Mes.Enabled = Habilidato;
        Cmb_Valido_Anio.Enabled = Habilidato;
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
            if (!String.IsNullOrEmpty(Txt_Folio_Pago.Text.Trim()))
            {

            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Limpiar_Campos_Reporte
    ///DESCRIPCIÓN: Limpiar valores de los campos
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 28-nov-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
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

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Limpiar_Campos_3D
    ///DESCRIPCIÓN: se inicializan los valores de los campo ocultos de validación
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 28-nov-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Limpiar_Campos_3D()
    {
        Hdf_3D_CardType.Value = "";
        Hdf_3D_CAVV.Value = "";
        Hdf_3D_ECI.Value = "";
        Hdf_3D_XID.Value = "";
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Consulta_Datos_Pasivo
    ///DESCRIPCIÓN: Consulta los datos del pasivo y regresa el resultado en un datatable
    ///PARÁMETROS:
    /// 		1. Estatus: texto con el estatus del pasivo a consultar
    /// 		2. No_Folio: Folio a consultar
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 28-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataTable Consulta_Datos_Pasivo(string Estatus, string No_Folio)
    {
        DataTable Dt_Pasivo = null;                             // Variable que obtendrán los datos de la consulta
        var Consulta_Ope_Ing_Pasivo = new Cls_Ope_Ate_Pagos_Internet_Negocio(); // Variable de conexión hacia la capa de Negocios

        try
        {
            Consulta_Ope_Ing_Pasivo.P_Referencia = No_Folio;
            Consulta_Ope_Ing_Pasivo.P_Estatus = Estatus;
            Dt_Pasivo = Consulta_Ope_Ing_Pasivo.Consulta_Datos_Pasivo(); // Consulta los datos de la referencia que introdujo el empleado

            return Dt_Pasivo;
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Datos_Pasivo " + ex.Message);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Consulta_Datos_Solicitud
    ///DESCRIPCIÓN: llama al método de consulta de datos de solicitudes de trámite pasando como parámetro 
    ///         una referencia de pasivo (clave del trámite)
    ///PARÁMETROS:
    /// 		1. Referencia: pasivo a consultar
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 19-jul-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataTable Consulta_Datos_Solicitud(string Referencia)
    {                          // Variable que obtendrán los datos de la consulta
        var Consulta_Ope_Ing_Pasivo = new Cls_Ope_Ate_Pagos_Internet_Negocio(); // Variable de conexión hacia la capa de Negocios

        try
        {
            Consulta_Ope_Ing_Pasivo.P_Referencia = Referencia;

            return Consulta_Ope_Ing_Pasivo.Consulta_Solicitud_Por_Pasivo();
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Datos_Solicitud " + ex.Message);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cargar_Datos_Pasivo
    ///DESCRIPCIÓN: Carga los datos del pasivo en los controles correspondientes de la página
    ///         Regresa un string con cualquier mensaje de error que se encuentre
    ///PARÁMETROS:
    /// 		1. Dt_Pasivos: tablas con datos de pasivos a cargar en los controles 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 28-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private string Cargar_Datos_Pasivo(DataTable Dt_Pasivos)
    {
        DateTime Fecha_Vencimiento;
        DataTable Dt_Formas_Pago_Pasivo = new DataTable(); // Obtiene las formas de pago que tuvo el ingreso
        decimal Total_Importe = 0;
        var Consulta_Ope_Ing_Pasivo = new Cls_Ope_Ate_Pagos_Internet_Negocio();
        decimal Total_Ajuste_Tarifario = 0;
        decimal Total_Pagar = 0;
        decimal Decimales = 0;

        try
        {
            if (Dt_Pasivos != null && Dt_Pasivos.Rows.Count > 0)
            {
                // si el estatus es diferente a por pagar, regresar mensaje de error
                if (Dt_Pasivos.Rows[0][Ope_Ing_Pasivo.Campo_Estatus].ToString().Trim() != "POR PAGAR")
                {
                    return "No es posible pagar el folio proporcionado, verifique que esté escrito correctamente y vuelva a intentar.";
                }
                // validar fecha de vencimiento
                DateTime.TryParse(Dt_Pasivos.Rows[0][Ope_Ing_Pasivo.Campo_Fecha_Vencimiento].ToString(), out Fecha_Vencimiento);
                if (Fecha_Vencimiento.Date < DateTime.Today)
                {
                    return "No es posible pagar el folio proporcionado porque la fecha de pago ya pasó.";
                }

                Txt_Folio_Pago.Text = Dt_Pasivos.Rows[0][Ope_Ing_Pasivo.Campo_Referencia].ToString();
                Txt_Solicitante.Text = Dt_Pasivos.Rows[0][Ope_Ing_Pasivo.Campo_Contribuyente].ToString();
                Txt_Concepto.Text = Dt_Pasivos.Rows[0][Ope_Ing_Pasivo.Campo_Descripcion].ToString();

                Consulta_Ope_Ing_Pasivo.P_Referencia = Txt_Folio_Pago.Text.Trim().ToUpper();
                DataTable Dt_Costo = Consulta_Ope_Ing_Pasivo.Consulta_Total_Pasivo();
                decimal.TryParse(Dt_Costo.Rows[0][Ope_Ing_Pasivo.Campo_Monto].ToString(), out Total_Importe);

                Txt_Importe.Text = Total_Importe.ToString("#,##0.00");
                Txt_Total_Pagar.Text = Total_Importe.ToString("#,##0.00");

                // calcular el ajuste tarifario
                Decimales = Convert.ToDecimal(String.Format("{0:#0.00}", Total_Importe - Math.Truncate(Total_Importe)));
                if (Decimales <= 0.5M)
                {
                    Total_Pagar = Math.Floor(Total_Importe);
                }
                else
                {
                    Total_Pagar = Math.Ceiling(Total_Importe);
                }
                Total_Ajuste_Tarifario = Total_Pagar - Total_Importe;

                Txt_Ajuste_Tarifario.Text = String.Format("{0:#,##0.00}", Convert.ToDecimal(Total_Ajuste_Tarifario));
                Txt_Total_Pagar.Text = String.Format("{0:#,##0.00}", Convert.ToDecimal(Total_Pagar));
            }

            return "";
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Datos_Pasivo: " + ex.Message);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Mostrar_Mensaje_Alerta
    ///DESCRIPCIÓN: Muestra un mensaje como alerta llamando código del clinte
    ///PARÁMETROS:
    /// 		1. Mensaje: Texto a mostrar al usuario 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 28-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Mostrar_Mensaje_Alerta(string Mensaje)
    {

        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pago en Caja", "alert('" + Mensaje + "');", true); ;
    }

    #endregion

    #region "Interaccion con Clases de Negocio [Base de Datos]"

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
        Negocio.P_Cuenta_Predial = Txt_Folio_Pago.Text.Trim().ToUpper();
        Negocio.P_Estatus = "POR PAGAR";
        Negocio.Eliminar_Pasivos_No_Pagados_Anteriormente();
    }

    #endregion


    #region Seguridad3D
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Mensaje_Error_3D
    ///DESCRIPCIÓN: Obtiene el mensaje del error de acuerdo al código en estatus
    ///PARAMETROS: Estatus, recibo el numero de estatus
    ///CREO: Ismael Prieto Sánchez  
    ///FECHA_CREO: 19/Octubre/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected String Mensaje_Error_3D(Int64 Estatus)
    {
        String Mensaje; //Almacena el mensaje de acuerdo al numero de error

        //Se verifica el código de error.
        switch (Estatus)
        {
            case 0:
                Mensaje = "TRANSACCION RECHAZADA - Error desconocido. Por favor comuniquese con su banco.";
                break;
            case 201:
            case 401:
                Mensaje = "El servicio de autenticación de transacciones está fuera de linea. Por favor intentelo más tarde.";
                break;
            case 422:
            case 423:
            case 424:
            case 425:
                Mensaje = "La contraseña ingresada para la transacción no es válida.";
                break;
            case 430:
                Mensaje = "No se especificó un número de tarjeta. Por favor verifique.";
                break;
            case 431:
                Mensaje = "No se especificó una fecha de expiración de la tarjeta. Por favor verifique.";
                break;
            case 432:
                Mensaje = "No se especificó un monto a pagar. Por favor verifique.";
                break;
            case 433:
                Mensaje = "Falta indicar el ID del comercio. Por favor verifique.";
                break;
            case 434:
                Mensaje = "La liga de respuesta esta vacia. Por favor verifique.";
                break;
            case 435:
                Mensaje = "Falta indicar el nombre del comercio. Por favor verifique.";
                break;
            case 436:
                Mensaje = "El número de tarjeta debe de ser de 16 digitos. Por favor verifique.";
                break;
            case 437:
                Mensaje = "El formato de la fecha de expiración es incorrecto. Por favor verifique.";
                break;
            case 438:
                Mensaje = "La fecha de expiración de la tarjeta no es válida. Por favor verifique.";
                break;
            case 439:
                Mensaje = "El monto indicado para el pago no es correcto. Por favor verifique.";
                break;
            case 440:
                Mensaje = "El nombre del comercio no puede ser mayor a 25 caracteres.  Por favor verifique.";
                break;
            case 498:
                Mensaje = "El tiempo de espera para la transacción ha expirado. Por favor intente nuevamente.";
                break;
            case 499:
                Mensaje = "El tiempo de captura para la transacción ha expirado. Por favor intente nuevamente.";
                break;
            default:
                Mensaje = "TRANSACCION RECHAZADA - Error desconocido. Por favor comuniquese con su banco.";
                break;
        }
        //Regresa el error.
        return Mensaje;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Ejecutar_Pago
    ///DESCRIPCIÓN: Realiza la ejecucion del pago hacia la base de datos
    ///PARAMETROS: 
    ///CREO: Ismael Prieto Sánchez  
    ///FECHA_CREO: 19/Octubre/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Ejecutar_Pago()
    {
        int Filas_Afectadas = 0;
        DataTable Dt_Formas_Pago = new DataTable(); //Obtiene las formas de pago con las cuales fue cubierto el importe del ingreso
        DataRow Renglon; //Manejo del registro
        var Neg_Alta_Pago_Internet = new Cls_Ope_Ate_Pagos_Internet_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            //Agrega las columnas a la tabla
            Dt_Formas_Pago.Columns.Add(new DataColumn("Forma_Pago", typeof(string)));
            Dt_Formas_Pago.Columns.Add(new DataColumn("Monto", typeof(decimal)));

            //Agrega los datos de pago por internet
            Renglon = Dt_Formas_Pago.NewRow();
            decimal Monto_Pago;
            decimal.TryParse(Txt_Total_Pagar.Text.Replace("$", ""), out Monto_Pago);
            Renglon["Forma_Pago"] = "INTERNET";
            Renglon["Monto"] = Monto_Pago;
            Dt_Formas_Pago.Rows.Add(Renglon);

            //Si tiene ajuste tarifario
            decimal Ajuste_Tarifario;
            if (decimal.TryParse(Txt_Ajuste_Tarifario.Text.Replace("$", ""), out Ajuste_Tarifario) && Ajuste_Tarifario != 0) //Ajuste tarfario
            {
                //Agrega los datos del ajuste tarifario
                Renglon = Dt_Formas_Pago.NewRow();
                Renglon["Forma_Pago"] = "AJUSTE TARIFARIO";
                Renglon["Monto"] = Ajuste_Tarifario;
                Dt_Formas_Pago.Rows.Add(Renglon);
            }

            //Consulta el id de la caja para pagos por internet.
            Cls_Ope_Pre_Parametros_Negocio Caja_Pago_Internet = new Cls_Ope_Pre_Parametros_Negocio();
            Cls_Cat_Pre_Cajas_Negocio Caja_Pago = new Cls_Cat_Pre_Cajas_Negocio();
            var Recibo_Negocio = new Cls_Ope_Pre_Impresion_Recibo_Negocio();
            String Caja_Id;
            Caja_Id = Caja_Pago_Internet.Consultar_Parametro_Caja_Pagos_Internet().Rows[0][Ope_Pre_Parametros.Campo_Caja_Id_Pago_Internet].ToString();
            // validar que se obtuvo el id de la caja
            if (string.IsNullOrEmpty(Caja_Id))
            {
                // No se puede realizar el pago, porque no esta establecida la caja de pago
                Mostrar_Mensaje_Alerta("En estos momentos no es posible realizar el pago, por favor intente más tarde.");
                return;
            }
            // consultar número de caja
            Caja_Pago.P_Caja_ID = Caja_Id;
            Caja_Pago = Caja_Pago.Consultar_Datos_Caja();
            // validar que se obtuvo el número de caja
            if (Caja_Pago == null || Caja_Pago.P_Numero_De_Caja == 0)
            {
                // No se puede realizar el pago, porque no esta establecida la caja de pago
                Mostrar_Mensaje_Alerta("En estos momentos no es posible realizar el pago, por favor intente más tarde.");
                return;
            }

            //Asigna los datos generales
            Neg_Alta_Pago_Internet.P_No_Recibo = "";
            Neg_Alta_Pago_Internet.P_Referencia = Txt_Folio_Pago.Text.Trim();
            Neg_Alta_Pago_Internet.P_Caja_ID = Caja_Pago.P_Caja_ID;
            Neg_Alta_Pago_Internet.P_No_Caja = Caja_Pago.P_Numero_De_Caja.ToString();
            Neg_Alta_Pago_Internet.P_No_Turno = "";
            Neg_Alta_Pago_Internet.P_Fecha_Pago = DateTime.Now;
            Neg_Alta_Pago_Internet.P_Ajuste_Tarifario = Convert.ToDecimal(Txt_Ajuste_Tarifario.Text.Replace("$", ""));
            Neg_Alta_Pago_Internet.P_Total_Pagar = Monto_Pago;
            Neg_Alta_Pago_Internet.P_Empleado_ID = "";
            Neg_Alta_Pago_Internet.P_Nombre_Usuario = "";
            Neg_Alta_Pago_Internet.P_Dt_Formas_Pago = Dt_Formas_Pago;
            Neg_Alta_Pago_Internet.P_Dt_Adeudos_Predial_Cajas_Detalle = Crear_Tabla_Totales();
            Neg_Alta_Pago_Internet.P_Tipo_Pago = "OTROS PAGOS";
            Neg_Alta_Pago_Internet.P_Monto_Corriente = 0;
            Neg_Alta_Pago_Internet.P_Monto_Recargos = 0;
            Neg_Alta_Pago_Internet.P_Cuenta_Predial_ID = "";
            //Llenado de datos para el pago de tarjeta...
            Neg_Alta_Pago_Internet.P_Banco_Codigo_Seguridad = Txt_Codigo_Seguridad.Text;
            Neg_Alta_Pago_Internet.P_Banco_Expira_Tarjeta = Cmb_Validez_Mes.SelectedValue + "/" + Cmb_Valido_Anio.SelectedValue;
            Neg_Alta_Pago_Internet.P_Banco_No_Tarjeta = Txt_No_Tarjeta.Text;
            Neg_Alta_Pago_Internet.P_Banco_Titular_Banco = Txt_Titular_Tarjeta.Text.ToUpper();
            Neg_Alta_Pago_Internet.P_Banco_Total_Pagar = Monto_Pago;
            //Llenado de datos para seguridad 3D
            Neg_Alta_Pago_Internet.P_Banco_3D_CAVV = Hdf_3D_CAVV.Value;
            Neg_Alta_Pago_Internet.P_Banco_3D_ECI = Hdf_3D_ECI.Value;
            Neg_Alta_Pago_Internet.P_Banco_3D_Tipo_Tarjeta = Cmb_Tipo_Tarjeta.SelectedItem.Value;
            Neg_Alta_Pago_Internet.P_Banco_3D_XID = Hdf_3D_XID.Value;
            // consultar datos de la solicitud para la impresión
            DataSet Ds_Solicitud = Crear_Dataset_Solicitud();
            //Da de alta el pago del ingreso
            Filas_Afectadas = Neg_Alta_Pago_Internet.Alta_Pago_Internet();
            if (Filas_Afectadas > 0)
            {
                Hdf_Clave_Operacion.Value = Neg_Alta_Pago_Internet.P_Banco_Clave_Operacion;
                //Inicializa los controles
                Limpiar_Generales();
                Limpiar_Campos_Reporte();
                Limpiar_Campos_3D();
                Configuracion_Formulario(false);
                Hdf_3D_Estatus.Value = "";
                // obtener datos para impresión
                Recibo_Negocio.P_Referencia = Neg_Alta_Pago_Internet.P_Referencia;
                Recibo_Negocio.P_No_Pago = Neg_Alta_Pago_Internet.P_No_Pago;
                DataTable Dt_Datos_Recibo = Recibo_Negocio.Consultar_Datos_Recibos_Tramites();

                // validar que el dataset contiene datos y llamar al método de impresión
                if (Ds_Solicitud != null && Ds_Solicitud.Tables.Count > 0 && Ds_Solicitud.Tables[0].Rows.Count > 0)
                {
                    // agregar datos del recibo al dataset
                    //Ds_Solicitud.Tables.RemoveAt(1);
                    Dt_Datos_Recibo.TableName = "Dt_Detalles_Pago";
                    Ds_Solicitud.Tables.Remove("Dt_Detalles_Pago");
                    Ds_Solicitud.Tables.Add(Dt_Datos_Recibo);
                    // generar impresión
                    Imprimir_Comprobante_Pago(Ds_Solicitud);
                }
                //if (Cls_Sessiones.P_Requiere_Facturacion_Ingreso)
                //{
                //    Mostrar_Factura_Ingreso(Neg_Alta_Pago_Internet.P_Referencia, Neg_Alta_Pago_Internet.P_No_Pago);
                //    Cls_Sessiones.P_Requiere_Facturacion_Ingreso = false;
                //}
                //else
                {
                    // mostrar mensaje 
                    Mostrar_Mensaje_Alerta("Pago realizado exitosamente.");
                }
            }
            else
            {
                Mostrar_Mensaje_Alerta("Ocurrió un error y no fue posible realizar el pago, por favor intente más tarde.");
            }
        }
        catch (Exception Exc)
        {
            Hdf_3D_Estatus.Value = "";
            Limpiar_Campos_3D();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pago de Predial por Internet", "alert('Error: [" + Exc.Message.Split(new String[] { " ." }, StringSplitOptions.None)[0] + "].');", true);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Mostrar_Factura_Ingreso
    ///DESCRIPCIÓN          : Manda llamar la pantalla para la generación de la Factura Electrónica
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Mayo/2015
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Mostrar_Factura_Ingreso(String Referencia, String No_Pago)
    {
        //Envia a impresion la factura
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Factura_Pago_Caja", "window.open('../Predial/Frm_Ope_Pre_Facturacion_Folio.aspx?Referencia=" + Referencia + /*"&No_Pago=" + No_Pago + */"','Imprimir_Factura');", true);
    }
    #endregion Seguridad3D

    #endregion METODOS


    #region EVENTOS
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Regresar_Click
    ///DESCRIPCIÓN: Ejecuta el Pago.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 24 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Regresar_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Ventanilla/Frm_Apl_Login_ventanilla.aspx");
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Ejecutar_Pago_Click
    ///DESCRIPCIÓN: Ejecuta el Pago.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 24 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Ejecutar_Pago_Click(object sender, EventArgs e)
    {
        decimal Total_Pagar;
        decimal.TryParse(Txt_Total_Pagar.Text.Replace("$", ""), out Total_Pagar);
        if (Validar_Campos_Tarjeta())
        {
            //Cls_Sessiones.P_Requiere_Facturacion_Ingreso = Chk_Requiere_Factura.Checked;
            //Valida los campos de llenado del usuario
            NameValueCollection data = new NameValueCollection(); //Declara la coleccion de parametros
            //Asigna los parametros a enviar a la pagina 3D
            data.Add("Card", Txt_No_Tarjeta.Text.Trim());
            data.Add("Expires", Cmb_Validez_Mes.SelectedValue + "/" + Cmb_Valido_Anio.SelectedValue);
            data.Add("Total", Total_Pagar.ToString());
            data.Add("CardType", Cmb_Tipo_Tarjeta.SelectedItem.Value);
            data.Add("MerchantId", "461");
            data.Add("MerchantName", "predio");
            data.Add("MerchantCity", "Irapuato Guanajuato");
            data.Add("ForwardPath", HttpContext.Current.Request.Url.ToString());
            data.Add("Cert3D", "03");
            data.Add("TipoTarjeta", Cmb_Tipo_Tarjeta.SelectedItem.Value);
            data.Add("Number", Txt_No_Tarjeta.Text.Trim());
            data.Add("Cvv2Val", Txt_Codigo_Seguridad.Text);
            data.Add("BillToFirstName", Txt_Titular_Tarjeta.Text);
            data.Add("Folio", Txt_Folio_Pago.Text);
            HttpHelper.RedirectAndPOST(this.Page, "https://eps.banorte.com/secure3d/Solucion3DSecure.htm", data);
        }
    }

    //protected void Generar_Pdf_Desde_Texto(string Texto_Contenido, string Ruta_Pdf_Generar)
    //{
    //    //HttpContext context = HttpContext.Current;
    //    StringReader reader = new StringReader(Texto_Contenido);

    //    //Create PDF document
    //    Document document = new Document(PageSize.LETTER);
    //    HTMLWorker parser = new HTMLWorker(document);

    //    PdfWriter.GetInstance(document, new FileStream(Ruta_Pdf_Generar, FileMode.Create));
    //    document.Open();

    //    try
    //    {
    //        parser.Parse(reader);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    finally
    //    {
    //        document.Close();
    //    }
    //}

    /////*******************************************************************************************************
    /////NOMBRE_FUNCIÓN: ObtenerHTML_Recibo_Pago
    /////DESCRIPCIÓN: regresa el texto (HTML) de la página que se recibe como parámetro
    /////PARÁMETROS:
    ///// 		1. strURL: cadena de caracteres con la dirección de la página a consultar
    /////CREO: Roberto González Oseguera
    /////FECHA_CREO: 17-sep-2012
    /////MODIFICÓ: 
    /////FECHA_MODIFICÓ: 
    /////CAUSA_MODIFICACIÓN: 
    /////*******************************************************************************************************
    //static string ObtenerHTML_Recibo_Pago(string strURL)
    //{

    //    String strResultado;
    //    WebResponse Obj_Respuesta;
    //    WebRequest Obj_Peticion = HttpWebRequest.Create(strURL);
    //    Obj_Respuesta = Obj_Peticion.GetResponse();
    //    using (StreamReader sr = new StreamReader(Obj_Respuesta.GetResponseStream()))
    //    {
    //        strResultado = sr.ReadToEnd();
    //        sr.Close();
    //    }
    //    return strResultado;
    //}

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
        Response.Redirect("Frm_Apl_Login_Ventanilla.aspx");
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Txt_Folio_Pago_TextChanged
    ///DESCRIPCIÓN: Manejo del evento cambio de texto en Txt_folio, se busca el folio en la tabla de pasivos
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 28-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Txt_Folio_Pago_TextChanged(object sender, EventArgs e)
    {
        DataTable Dt_Pasivos;
        string Mensaje = "";

        Limpiar_Campos_3D();
        Txt_Titular_Tarjeta.Text = "";
        Txt_No_Tarjeta.Text = "";
        Txt_Codigo_Seguridad.Text = "";
        Cmb_Validez_Mes.SelectedIndex = 0;
        Cmb_Valido_Anio.SelectedIndex = 0;

        try
        {
            // validar que haya un folio para buscar
            if (Txt_Folio_Pago.Text.Trim().Length > 0)
            {
                // llamar al método que consulta los datos del pasivo
                Dt_Pasivos = Consulta_Datos_Pasivo("POR PAGAR", Txt_Folio_Pago.Text.ToUpper());
                // limpiar datos del formulario
                Limpiar_Generales();
                Configuracion_Formulario(false);

                // validar que la consulta arrojó datos
                if (Dt_Pasivos != null && Dt_Pasivos.Rows.Count > 0)
                {
                    // si el origen del pasivo es un TRÁMITE, validar que el trámite esté vigente
                    if (Dt_Pasivos.Rows[0][Ope_Ing_Pasivo.Campo_Origen].ToString() == "SOLICITUD TRAMITE")
                    {
                        // consultar los datos de la solicitud del pasivo encontrado
                        DataTable Dt_Solicitud = Consulta_Datos_Solicitud(Txt_Folio_Pago.Text.ToUpper());
                        if (Dt_Solicitud != null && Dt_Solicitud.Rows.Count > 0)
                        {
                            // si el trámite tiene estatus CANCELADO o DETENIDO, mostrar mensaje y abandonar el método
                            if (Dt_Solicitud.Rows[0][Ope_Tra_Solicitud.Campo_Estatus].ToString().Trim() == "CANCELADO" || Dt_Solicitud.Rows[0][Ope_Tra_Solicitud.Campo_Estatus].ToString().Trim() == "DETENIDO")
                            {
                                Mostrar_Mensaje_Alerta("No es posible realizar el pago porque el trámite está: " + Dt_Solicitud.Rows[0][Ope_Tra_Solicitud.Campo_Estatus].ToString());
                                return;
                            }
                        }
                    }

                    Mensaje = Cargar_Datos_Pasivo(Dt_Pasivos);
                    // si el método no regresó texto, configurar controles para  mostrar como mensaje de error
                    if (Mensaje.Length <= 0)
                    {
                        Configuracion_Formulario(true);
                    }
                    else
                    {
                        Mostrar_Mensaje_Alerta(Mensaje);
                    }
                }
                else
                {
                    Mostrar_Mensaje_Alerta("No se encontraron datos para el folio ingresado, verifique que esté escrito correctamente y vuelva a intentar.");
                }
            }
            else
            {
                Limpiar_Generales();
                Limpiar_Campos_Reporte();
                Limpiar_Campos_3D();
            }
        }
        catch
        {
        }
    }

    #endregion EVENTOS


}