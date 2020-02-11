using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Operacion_Predial_Orden_Variacion.Negocio;
using System.Text.RegularExpressions;
using System.Data.OracleClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Text;

public partial class paginas_Predial_Frm_Ope_Pre_Cierre_Anual_Ordenes_Variacion : System.Web.UI.Page
{

    ///********************************************************************************
    ///                                 METODOS
#region METODOS

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Roberto González Oseguera
    /// FECHA_CREO  : 23-jul-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Txt_Anio_Aplicar.Text = (DateTime.Now.Year + 1).ToString();
            Habilitar_Controles("Inicial"); // Habilita los controles de la forma
            //Limpiar_Controles(); // Limpia los controles del forma
            Obtener_Parametros_Proyeccion(); // Consulta los parametros de la BD
            Consultar_Ordenes_Pendientes();
            Consultar_Cuentas_Cuota_Anual_Menor_Minima();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION: Limpia los controles que se encuentran en la forma
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 25-jul-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            // ocultar y limpiar mensaje de error
            Lbl_Mensaje_Error.Text = "";
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            // limpiar parametros
            Txt_Cuota_Minima.Text = "";
            // llamar metodo para limpiar etiquetas con mensajes de error
            Limpiar_Mensajes_Parametros();

            // limpiar grid
            Grid_Errores.DataSource = null;
            Grid_Errores.DataBind();

            //Limpiar_Controles_Tasas();
        }
        catch (Exception ex)
        {
            throw new Exception("Limpiar_Controles: " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Habilitar_Controles
    /// DESCRIPCIÓN: Habilita o Deshabilita los controles de la forma según se requiera 
    ///             para la siguiente operación
    /// PARÁMETROS:
    ///         1. Operacion: Indica la operación que se desea realizar por parte del usuario
    /// 	             (inicial)
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 23-jul-2011 
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        try
        {
            switch (Operacion)
            {
                case "Inicial":
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    break;
            }
            Txt_Anio_Aplicar.Enabled = true;
            Txt_Cuota_Minima.Enabled = false;

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }// termina metodo Habilitar_Controles

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Obtener_Parametros_Proyeccion
    /// DESCRIPCION: Consulta los parametros (salario y cuota minima, tasas, etc) de la BD 
    ///             mediante la clase de negocio Generar adeudos predial
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 24-jul-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Obtener_Parametros_Proyeccion()
    {
        Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio RS_Consulta_Parametros = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        Int32 Anio = 0;     // anio siguiente
        DataTable Dt_Tasas = new DataTable();

        Lbl_Mensaje_Error.Text = "";
        Lbl_Mensaje_Error.Visible = false;  
        Img_Error.Visible = false;

        // obtener el año de la caja de texto
        Int32.TryParse(Txt_Anio_Aplicar.Text, out Anio);

        try
        {
            RS_Consulta_Parametros.p_Anio = Anio;
            RS_Consulta_Parametros.Obtener_Parametros(); //Consulta los parametros
            
            // mostrar Anio
            if (Anio > 0)
            {
                Txt_Anio_Aplicar.Text = Anio.ToString();
            }

            // obtener cuota minima
            if (RS_Consulta_Parametros.p_Cuota_Minima > 0)
            {
                Txt_Cuota_Minima.Text = RS_Consulta_Parametros.p_Cuota_Minima.ToString("#,##0.00");
            }
            else            // mostrar mensaje de error
            {
                Decimal Cuota_Minima_Anterior = RS_Consulta_Parametros.Obtener_Cuota_Minima(Anio - 1); // Consultar cuota minima
                if (Cuota_Minima_Anterior > 0)
                {
                    Txt_Cuota_Minima.Text = Cuota_Minima_Anterior.ToString();
                    Lbl_Msg_Cuota_Minima.Text = "No se encontró Cuota mínima para " + Anio + " se muestra la de " + (Anio - 1).ToString();
                    Lbl_Msg_Cuota_Minima.Style.Value += "color:red;";
                }
                else
                {
                    Lbl_Msg_Cuota_Minima.Text = "No se encontró Cuota mínima";
                    Lbl_Msg_Cuota_Minima.Style.Value += "color:red;";
                }
            }

        }
        catch (Exception ex)
        {
            throw new Exception("Obtener_Parametros: " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Ordenes_Pendientes
    /// DESCRIPCION: Metodo para consultar si existen ordenes de variacion pendientes 
    ///             (POR VALIDAR)
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 12-sep-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Ordenes_Pendientes()
    {
        Cls_Ope_Pre_Orden_Variacion_Negocio Rs_Ordenes = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        DataTable Dt_Ordenes;

        // consultar ordenes de variacion por validar
        Rs_Ordenes.P_Incluir_Campos_Foraneos = false;
        Rs_Ordenes.P_Generar_Orden_Estatus = "POR VALIDAR";
        Dt_Ordenes = Rs_Ordenes.Consultar_Ordenes_Variacion();

        if (Dt_Ordenes.Rows.Count > 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; Existen " + Dt_Ordenes.Rows.Count.ToString("#,##0") + " órdenes de variación POR VALIDAR.<br />";
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }

    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Cuentas_Cuota_Anual_Menor_Minima
    /// DESCRIPCION: Metodo para consultar cuantas cuentas tienen cuota anual menor a la minima
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 07-mar-2012
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Cuentas_Cuota_Anual_Menor_Minima()
    {
        var Consulta_Cuentas = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        DataTable Dt_Cuentas;
        int Anio;
        decimal Cuota_Minima;
        string Estatus_Filtro;

        // solo el valor en la caja de texto es igual al año actual o el siguiente
        if (int.TryParse(Txt_Anio_Aplicar.Text, out Anio) && (Anio == DateTime.Now.Year || Anio == DateTime.Now.AddYears(1).Year))
        {
            // consultar cuota mínima del año seleccionado
            Cuota_Minima = Consulta_Cuentas.Obtener_Cuota_Minima(Anio);
            Estatus_Filtro = " NOT IN ('CANCELADA','PENDIENTE','BAJA','TEMPORAL')";
            // consultar cuentas con cuota mínima
            Dt_Cuentas = Consulta_Cuentas.Consultar_Cuentas_Adeudo_Menor_Cuota_Minima(Cuota_Minima, Estatus_Filtro);

            if (Dt_Cuentas.Rows.Count > 0)
            {
                Lbl_Mensaje_Error.Text += "<br /> &nbsp; &nbsp; &nbsp; &nbsp; Se encontraron " + Dt_Cuentas.Rows.Count.ToString("#,##0") + " cuentas con cuota anual menor a la cuota mínima.<br />";
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Generar_Ordenes_Cierre_Anual
    /// DESCRIPCION: Metodo para llamar al metodo en la clase de negocio que generar las ordenes
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 14-ene-2012
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private String Generar_Ordenes_Cierre_Anual()
    {
        Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio RS_Genera_Adeudos = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        Cls_Ope_Pre_Parametros_Negocio Parametro = new Cls_Ope_Pre_Parametros_Negocio();
        Int32 Anio;

        DateTime Hora_Inicio = DateTime.Now;

        String Mensajes_Error = "";

        Int32.TryParse(Txt_Anio_Aplicar.Text, out Anio);
        // si el año en la caja de texto no es el siguiente o el actual, regresar mensaje
        if (Anio != DateTime.Now.Year && Anio != DateTime.Now.Year + 1)
        {
            return "Sólo se puede especificar el año siguiente o el actual.";
        }
        RS_Genera_Adeudos.p_Anio = Anio;

        // metodo que genera las ordenes de variacion
        Mensajes_Error = RS_Genera_Adeudos.Generar_Ordenes_Variacion_Cuota_Minima();

        // Mostrar resultados generacion
        Mostrar_Resultados_Generacion_Ordenes(RS_Genera_Adeudos);
        Lbl_Encabezado_Resultados.Text = "Ordenes generadas";
        Pnl_Resultados.Visible = true;

        // Mostrar errores ocurridos
        Cargar_Listado_Errores_Generacion(RS_Genera_Adeudos.p_Errores_Cuentas);


        if (!Directory.Exists(Server.MapPath("~/Reporte/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/Reporte/"));
        }
        using (StreamWriter w = File.AppendText(Server.MapPath("~/Reporte/generacion_adeudos.txt")))
        {

            Log("Generar_Adeudos_Cierre", "Cuentas generadas: " + RS_Genera_Adeudos.p_Total_Adeudos_Generados, Hora_Inicio, w);
            // Close the writer and underlying file.
            w.Close();
        }

        return Mensajes_Error;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Cargar_Listado_Errores_Generacion
    /// DESCRIPCIÓN: Muestra el listado de errores ocurridos durante la generacion de adeudos
    /// PARÁMETROS:
    /// 	1. Errores_Cuentas: Diccionario con cuenta predial y errores ocurridos
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 26-jul-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Cargar_Listado_Errores_Generacion(Dictionary<String, String> Errores_Cuentas)
    {
        // si contiene registros, mostrar panel y cargar en datagrid
        if (Errores_Cuentas != null)
        {
            if (Errores_Cuentas.Count > 0)
            {
                Pnl_Errores_Generacion.Visible = true;
                Grid_Errores.DataSource = Errores_Cuentas;
                Grid_Errores.DataBind();
            }
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Crear_Ds_Generacion_Adeudos
    /// DESCRIPCIÓN: Crea un dataset y lo guarda en una variable de sesion para el reporte 
    ///             con la misma informacion que se muestra en pantalla
    /// PARÁMETROS:
    /// 	1. Sumatoria_Adeudos: Sumatoria de cuota anual y bimestres 
    /// 	2. Totales: Instancia de la capa de negocio con totales de la generacion
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 19-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Crear_Ds_Generacion_Adeudos(Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Totales)
    {
        Ds_Ope_Pre_Cierre_Anual Ds_Cierre = new Ds_Ope_Pre_Cierre_Anual();

        if (Totales != null)
        {
            DataRow Dr_Datos_Generales = Ds_Cierre.Tables[0].NewRow();

            // Ordenes de variacion por cuota minima
            if (Totales.p_Total_Ordenes_Cuota_Minima > 0)
            {
                Dr_Datos_Generales["Ordenes_Generadas"] = Totales.p_Total_Ordenes_Cuota_Minima.ToString("#,##0") + " órdenes de variación por cuota mínima";
            }

            if (!(Totales.p_Errores_Cuentas == null))
            {
                Dr_Datos_Generales["Errores"] = "Errores: " + Totales.p_Errores_Cuentas.Count;
                foreach (KeyValuePair<string, string> Error in Totales.p_Errores_Cuentas)
                {
                    DataRow Dr_Error = Ds_Cierre.Tables["Dt_Errores"].NewRow();
                    Dr_Error["Cuenta_Predial"] = Error.Key;
                    Dr_Error["Error"] = Error.Value;
                    Ds_Cierre.Tables["Dt_Errores"].Rows.Add(Dr_Error);
                }
            }

            Ds_Cierre.Tables["Dt_Datos_Generales"].Rows.Add(Dr_Datos_Generales);
        }
        Session["Ds_Generacion_Adeudos"] = Ds_Cierre;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Mostrar_Resultados_Generacion_Ordenes
    /// DESCRIPCIÓN: Muestra el resultado de los adeudos aplicados
    /// PARÁMETROS:
    /// 	1. Sumatoria_Adeudos: Sumatoria de cuota anual y bimestres 
    /// 	2. Totales: Instancia de la capa de negocio con totales de la generacion
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 14-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Mostrar_Resultados_Generacion_Ordenes(Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Totales)
    {
        String Resultado = "";
        String Reporte_Errores = Lbl_Resultado_Generacion.Text;
        HyperLink Hl_Enlace_Urbanos = new HyperLink();
        String Propiedades_Ventana_Emergente;

        // quitar ultima linea del resultado anterior
        if (Resultado.IndexOf("Total de montos ") > 0)
        {
            Resultado = Resultado.Substring(0, Resultado.IndexOf("Total de montos "));
        }

        Propiedades_Ventana_Emergente = ", 'toolbar=0,location=0,status=0,menubar=0,scrollbars=1,resizable=0,width=550,height=450,left=300,top=200');";
        // si contiene registros, mostrar panel y cargar en datagrid
        if (Totales != null)
        {
            // Ordenes de variacion por cuota minima
            if (Totales.p_Total_Ordenes_Cuota_Minima > 0)
            {
                String Fecha_Generacion = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");
                StringBuilder Sb_Enlace_Ordenes = new StringBuilder();
                HtmlTextWriter Htw_Enlace_Ordenes = new HtmlTextWriter(new System.IO.StringWriter(Sb_Enlace_Ordenes, System.Globalization.CultureInfo.InvariantCulture));
                Hl_Enlace_Urbanos.Text = Totales.p_Total_Ordenes_Cuota_Minima + " órdenes de variación por cuota mínima";
                Htw_Enlace_Ordenes.AddAttribute(HtmlTextWriterAttribute.Onclick,
                    "window.open('Ventanas_Emergentes/Frm_Reporte_Ordenes_Cuota_Minima.aspx?Fecha=" + Fecha_Generacion
                    + "&Anio=" + DateTime.Now.Year
                    + "', 'Cuentas_Pendientes'" + Propiedades_Ventana_Emergente);
                Htw_Enlace_Ordenes.AddAttribute(HtmlTextWriterAttribute.Class, "Enlace_Archivo");
                Hl_Enlace_Urbanos.RenderControl(Htw_Enlace_Ordenes);
                Resultado += " Se generaron " + Sb_Enlace_Ordenes.ToString() + " <br /><br />";
            }

            // escribir resultados en etiqueta de generacion
            Lbl_Resultado_Generacion.Text = Resultado;
            Lbl_Errores_Generacion.Text = Reporte_Errores;
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Crear_Ds_Cierre_Anual
    /// DESCRIPCIÓN: Regresa un Dataset con los datos para imprimir el reporte
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 19-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataSet Crear_Ds_Cierre_Anual()
    {
        Ds_Ope_Pre_Cierre_Anual Ds_Cierre_Anual = new Ds_Ope_Pre_Cierre_Anual();
        
        // si se encuentra el dataset en variable de sesion, sustituir el que se creo y agregarle datos
        if (Session["Ds_Generacion_Adeudos"] != null)
        {
            Ds_Cierre_Anual = (Ds_Ope_Pre_Cierre_Anual)Session["Ds_Generacion_Adeudos"];
            // verificar que contenga tablas y que la primera tabla contenga renglones
            if (Ds_Cierre_Anual.Tables.Count > 0 && Ds_Cierre_Anual.Tables[0].Rows.Count > 0)
            {
                Ds_Cierre_Anual.Tables[0].Rows[0]["Titulo"] = "ORDENES DE CIERRE ANUAL " + Txt_Anio_Aplicar.Text;
                Ds_Cierre_Anual.Tables[0].Rows[0]["Subtitulo"] = Lbl_Encabezado_Resultados.Text;
                Ds_Cierre_Anual.Tables[0].Rows[0]["Fecha"] = DateTime.Now.ToString("dd/MMM/yyyy");
            }
        }
        return Ds_Cierre_Anual;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Imprimir_Reporte
    /// DESCRIPCIÓN: Genera el reporte de Crystal con los datos proporcionados en el DataTable
    /// PARÁMETROS:
    /// 		1. Ds_Convenio: Dataset con datos a imprimir
    /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
    /// 		3. Nombre_Archivo: Nombre del archivo a generar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 19-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Imprimir_Reporte(DataSet Ds_Convenio, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String Ruta = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);

        try
        {
            Reporte.Load(Ruta);
            Reporte.SetDataSource(Ds_Convenio);
        }
        catch
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte para su impresión";
        }

        String PDF_Convenio = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar 
        try
        {
            ExportOptions Export_Options_Calculo = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options_Calculo = new DiskFileDestinationOptions();
            Disk_File_Destination_Options_Calculo.DiskFileName = Server.MapPath("../../Reporte/" + PDF_Convenio);
            Export_Options_Calculo.ExportDestinationOptions = Disk_File_Destination_Options_Calculo;
            Export_Options_Calculo.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options_Calculo.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Export_Options_Calculo);

            Mostrar_Reporte(PDF_Convenio, "Cierre_Anual", "Window_Fmt");
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Mostrar_Reporte
    /// DESCRIPCIÓN: Visualiza en pantalla el reporte indicado
    /// PARÁMETROS:
    /// 		1. Nombre_Reporte: Nombre del reporte a generar
    /// 		2. Tipo: Parametro tipo de reporte
    /// 		3. Window_Fmt: Parametros para la ventana en la que se muestra el reporte
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 20-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Mostrar_Reporte(String Nombre_Reporte, String Tipo, String Window_Fmt)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                Window_Fmt,
                "window.open('" + Pagina +
                "', '" + Tipo + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')",
                true
                );
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Mostrar_Reporte
    /// DESCRIPCIÓN: Visualiza en pantalla el reporte indicado
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 20-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Limpiar_Mensajes_Parametros()
    {
        Lbl_Msg_Cuota_Minima.Text = "";
    }

#endregion METODOS


    ///********************************************************************************
    ///                                 EVENTOS
#region EVENTOS

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Page_Load
    /// DESCRIPCIÓN:  Manejador del evento Carga de pagina, 
    /// PARÁMETROS:
    /// 	1. sender: Objeto que  llama al evento
    /// 	2. e: Argumentos del evento
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 22-jul-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            //if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!Page.IsPostBack)
            {
                Session["Activa"] = true;
                //Recuperar_Sesion();       // recuperar el valor de cookies con el estado de los paneles
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                //Llenar_Grid();
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Imprimir_Click
    /// DESCRIPCIÓN: Manejo del evento clic en el boton imprimir (imprimir reporte de cierre)
    /// PARÁMETROS: sender, e
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 19-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        // verificar que hay texto para imprimir cuenta seleccionada
        if (Lbl_Resultado_Generacion.Text != "" || Lbl_Errores_Generacion.Text != "")
        {
            // llamar metodo impresion de reporte
            Imprimir_Reporte(Crear_Ds_Cierre_Anual(),
                "Rpt_Pre_Cierre_Anual.rpt",
                "Cierre_Anual");
        }
        else
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No hay resultados para generar reporte.<br />";
        }
    }
    
    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Salir_Click
    /// DESCRIPCIÓN: Manejo del evento Click para el control Btn_Salir. Ir a la página principal
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 23-jul-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Inicio")
            {
                Session.Remove("Consulta_Partidas");
                Session.Remove("Dicc_Tasas");
                Session.Remove("Dicc_Conceptos");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Inicializa_Controles();//Habilita los controles para la siguiente operación del usuario en el catálogo
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Generar_Ordenes_Click
    /// DESCRIPCIÓN: Llamar al método en la capa de negocio que genera órdenes de variación
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 14-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Generar_Ordenes_Click(object sender, EventArgs e)
    {
        String Mensaje = "";

        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            Mensaje = Generar_Ordenes_Cierre_Anual();
            if (Mensaje == "")
            {
                Btn_Generar_Ordenes.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType()
                    , "Ordenes de variación por cierre anual", "alert('Generación de órdenes Exitosa');", true);
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Mensaje;
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
        
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Txt_Anio_Aplicar_Changed
    /// DESCRIPCIÓN: Manejar el cambio de texto en la caja Anio_Aplicar
    ///             validar el año
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 13-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Txt_Anio_Aplicar_Changed(object sender, EventArgs e)
    {
        Int32 Anio;

        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        Limpiar_Mensajes_Parametros();
        Int32.TryParse(Txt_Anio_Aplicar.Text, out Anio);
        // validar que el año sea el actual o el siguiente
        if (Anio == DateTime.Now.Year || Anio == DateTime.Now.Year + 1)
        {
            Obtener_Parametros_Proyeccion();
            Consultar_Ordenes_Pendientes();
            Consultar_Cuentas_Cuota_Anual_Menor_Minima();
        }
        else
        {
            Limpiar_Controles();
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Sólo se puede hacer la generación para el año actual o el siguiente";
        }
    }

#endregion EVENTOS

    public static void Log(string Funcion, string Mensaje, DateTime Hora_Inicial, TextWriter w)
    {
        w.WriteLine("\r\nFuncion : " + Funcion);
        w.WriteLine("Iniciado   {0} {1}", Hora_Inicial.ToLongTimeString(), Hora_Inicial.ToLongDateString());
        w.WriteLine("Finalizado {0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
        w.WriteLine("  :");
        w.WriteLine("  :{0}", Mensaje);
        w.WriteLine("-------------------------------");
        // Update the underlying file.
        w.Flush();
    }

}
