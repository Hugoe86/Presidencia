using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Reporte_Cat_Catastrales.Negocio;
using System.Data;
using Presidencia.Sessiones;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class paginas_Catastro_Frm_Rpt_Cat_Reportes_Area_Privativa : System.Web.UI.Page
{
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN:
    ///PROPIEDADES:     
    ///            
    ///CREO: David Herrera Rincon
    ///FECHA_CREO: 22/Octubre/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuracion_Formulario(true);                
            }
        }
        catch (Exception ex)
        {
            Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
        Div_Contenedor_Msj_Error.Visible = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Establece la configuración del formulario
    ///PROPIEDADES:     Enabled: Especifica si estara habilitado o no el componente
    ///            
    ///CREO: David Herrera Rincon
    ///FECHA_CREO: 22/Octubre/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Enabled)
    {
        Txt_Fecha_Inicio.Enabled = false;
        Txt_Fecha_Fin.Enabled = false;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Predios_Estrategicos
    ///DESCRIPCIÓN          : Crea un DataSet para el reporte de predios estrategiscos
    ///PARAMETROS:          : DataTable que trae registros 
    ///CREO                 : David Herrera rincon
    ///FECHA_CREO           : 26/Octubre/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Area_Privativa(DataTable Dt_Temp)
    {

        Ds_Ope_Cat_Area_Privativa Ds_Area_Privativa = new Ds_Ope_Cat_Area_Privativa();

        foreach (DataRow Dr_Temp in Dt_Temp.Rows)
        {
            DataRow Dr_Fila = Ds_Area_Privativa.Tables["Dt_Area_Privativa"].NewRow();

            if (!String.IsNullOrEmpty(Dr_Temp["Fecha"].ToString()))
                Dr_Fila["Fecha"] = DateTime.Parse(Dr_Temp["Fecha"].ToString()).ToString("dd/MMM/yyyy");
            Dr_Fila["Tipo_Condominio"] = Dr_Temp["Tipo_Condominio"].ToString();
            Dr_Fila["Propietario"] = Dr_Temp["Propietario"].ToString();
            Dr_Fila["Denominacion"] = Dr_Temp["Denominacion"].ToString();
            Dr_Fila["Ubicacion"] = Dr_Temp["Ubicacion"].ToString();
            Dr_Fila["Areas_Privativas"] = Dr_Temp["Areas_Privativas"].ToString();
            Dr_Fila["No_Cuenta"] = Dr_Temp["cuenta_predial"].ToString();
            Dr_Fila["Cantidad"] = Dr_Temp["Cantidad"].ToString();

            Ds_Area_Privativa.Tables["Dt_Area_Privativa"].Rows.Add(Dr_Fila);
            Ds_Area_Privativa.Tables["Dt_Area_Privativa"].AcceptChanges();
        }

        return Ds_Area_Privativa;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Imprimir_Reporte(DataSet Ds_Convenios, String Nombre_Reporte, String Nombre_Archivo, String Formato, String Tipo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Catastro/" + Nombre_Reporte);
        try
        {
            Reporte.Load(File_Path);
            Reporte.SetDataSource(Ds_Convenios);
        }
        catch
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte para su impresión";
        }

        String Archivo_PDF = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar    
        try
        {
            ExportOptions Export_Options = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
            Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_PDF);
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Export_Options);
        }
        catch (Exception Ex)
        {

        }

        try
        {
            Mostrar_Reporte(Archivo_PDF, Tipo, Formato);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Mostrar_Reporte
    ///DESCRIPCIÓN          : Visualiza en pantalla el reporte indicado
    ///PARAMETROS           : Nombre_Reporte: cadena con el nombre del archivo.
    ///                     : Formato: Exensión del archivo a visualizar.
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Mostrar_Reporte(String Nombre_Reporte, String Formato, String Frm_Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(this, this.GetType(), Frm_Formato, "window.open('" + Pagina + "', '" + Formato + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Evento del botón salir
    ///PROPIEDADES:     
    ///            
    ///CREO: David Herrera Rincon
    ///FECHA_CREO: 26/Octubre/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Configuracion_Formulario(true);
            Btn_Exportar_Areas.Visible = true;
            Btn_Exportar_Areas.AlternateText = "Reporte Areas Privativas";
            Btn_Exportar_Areas.ImageUrl = "~/paginas/imagenes/gridview/pdf.png";
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";            
        }
    }    

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Exportar_Solicitudes_Click
    ///DESCRIPCIÓN: Evento del botón Exportar a pdf
    ///PROPIEDADES:     
    ///            
    ///CREO: David Herrera Rincon
    ///FECHA_CREO: 26/Oct/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Exportar_Areas_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Rpt_Cat_Catastrales_Negocio Negocio = new Cls_Rpt_Cat_Catastrales_Negocio();//Declaracion de la clase de negocios

        if (!String.IsNullOrEmpty(Txt_Fecha_Inicio.Text) && (!String.IsNullOrEmpty(Txt_Fecha_Fin.Text)))
        {
            Negocio.P_Fecha_Inicio = Txt_Fecha_Inicio.Text;
            Negocio.P_Fecha_Fin = Txt_Fecha_Fin.Text;

            DataTable Dt_Temp = Negocio.Consultar_Areas_Privativas();//Realizamos la consulta
            //Si tiene datos, mostramos el reporte
            if (Dt_Temp.Rows.Count > 0)
                Imprimir_Reporte(Crear_Ds_Area_Privativa(Dt_Temp), "Rpt_Ope_Cat_Areas_Privativa.rpt", "Reporte_Areas_Privativas", "Window_Frm", "Avaluo_Urbano");
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Reportes Areas Privativas", "alert('Debe seleccionar las fechas.');", true);
        }
    }
}