using System;
using System.Data;
using System.Web.UI;
using Presidencia.Constantes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Registro_Peticion.Negocios;
using System.Web.UI.WebControls;

public partial class paginas_Ventanilla_Frm_Rpt_Ven_Consultar_Peticion : System.Web.UI.Page
{
    #region Page load
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    #endregion Page load

    #region METODOS

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Limpiar_Campos
    ///DESCRIPCIÓN: Limpia las cajas de texto y grids de la página
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 07-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Limpiar_Campos()
    {
        Txt_Solicitante.Text = "";
        Txt_Sexo.Text = "";
        Txt_Edad.Text = "";
        Txt_Domicilio.Text = "";
        Txt_Referencia.Text = "";
        Txt_Codigo_Postal.Text = "";
        Txt_Email.Text = "";
        Txt_Telefono.Text = "";
        Txt_Peticion.Text = "";
        Txt_Estatus.Text = "";
        Txt_Fecha_Peticion.Text = "";
        Txt_Fecha_Solucion.Text = "";
        Txt_Solucion.Text = "";

        // limpiar grids
        Grid_Observaciones.DataSource = null;
        Grid_Observaciones.DataBind();
        Grid_Seguimiento.DataSource = null;
        Grid_Seguimiento.DataBind();

        // ocultar campos descripción y fecha de solución
        Tr_Fecha_Solucion.Visible = false;
        Tr_Txt_Solucion.Visible = false;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Imprimir_Reporte
    /// DESCRIPCIÓN: Genera el reporte de Crystal con los datos proporcionados en el DataTable 
    /// PARÁMETROS:
    /// 		1. Ds_Convenio: Dataset con datos a imprimir
    /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
    /// 		3. Nombre_Archivo: Nombre del archivo a generar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Imprimir_Reporte(DataSet Ds_Convenio, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String Ruta = Server.MapPath("../Rpt/Ventanilla/" + Nombre_Reporte);

        Reporte.Load(Ruta);
        Reporte.SetDataSource(Ds_Convenio);

        String PDF_Convenio = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar

        ExportOptions Export_Options_Calculo = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options_Calculo = new DiskFileDestinationOptions();
        Disk_File_Destination_Options_Calculo.DiskFileName = Server.MapPath("../../Reporte/" + PDF_Convenio);
        Export_Options_Calculo.ExportDestinationOptions = Disk_File_Destination_Options_Calculo;
        Export_Options_Calculo.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options_Calculo.ExportFormatType = ExportFormatType.PortableDocFormat;
        Reporte.Export(Export_Options_Calculo);

        Mostrar_Reporte(PDF_Convenio, "Formato");
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Mostrar_Reporte
    /// DESCRIPCIÓN: Visualiza en pantalla el reporte indicado
    /// PARÁMETROS:
    /// 		1. Nombre_Reporte: Nombre del reporte a generar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Mostrar_Reporte(String Nombre_Reporte, String Tipo)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            Pagina = Pagina + Nombre_Reporte;
            AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "Formato",
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
    ///NOMBRE_FUNCIÓN: Crear_Ds_Imprimir_Reporte
    ///DESCRIPCIÓN: Crea un Dataset con los datos de la petición que se reciben como parámetro
    ///PARÁMETROS:
    /// 		1. Dt_Temporal: datatable con los datos a insertar en la tabla del dataset que se regresa
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 28-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataSet Crear_Ds_Imprimir_Reporte(DataTable Dt_Reporte)
    {
        var Ds_Reporte = new Ds_Ope_Consulta_Peticiones_Especifico();
        DateTime Fecha_Peticion;
        var Obj_Peticiones = new Cls_Cat_Ate_Peticiones_Negocio();
        DataTable Dt_Seguimiento;
        DataTable Dt_Observaciones;
        DataRow Dr_Fila_Reporte = Ds_Reporte.Tables["DataTable1"].NewRow();
        string Domicilio = "";

        // formar el domicilio
        if (Dt_Reporte.Rows[0]["CALLE"].ToString().Length > 0)
        {
            Domicilio += Dt_Reporte.Rows[0]["CALLE"].ToString() + " " 
                + Dt_Reporte.Rows[0][Ope_Ate_Peticiones.Campo_Numero_Exterior].ToString() + " " 
                + Dt_Reporte.Rows[0][Ope_Ate_Peticiones.Campo_Numero_Interior].ToString();
        }
        else
        {
            if (Dt_Reporte.Rows[0][Ope_Ate_Peticiones.Campo_Numero_Exterior].ToString().Trim().Length > 0)
            {
                Domicilio += "Numero exterior " + Dt_Reporte.Rows[0][Ope_Ate_Peticiones.Campo_Numero_Exterior].ToString();
            }
            if (Dt_Reporte.Rows[0][Ope_Ate_Peticiones.Campo_Numero_Interior].ToString().Trim().Length > 0)
            {
                Domicilio += " int. " + Dt_Reporte.Rows[0][Ope_Ate_Peticiones.Campo_Numero_Interior].ToString();
            }
        }
        // si hay colonia, agregar al domicilio
        if (Dt_Reporte.Rows[0]["COLONIA"].ToString().Length > 0)
        {
            if (Domicilio.Length > 0)
            {
                Domicilio += " " + Dt_Reporte.Rows[0]["COLONIA"].ToString();
            }
            else
            {
                Domicilio += "colonia " + Dt_Reporte.Rows[0]["COLONIA"].ToString();
            }
        }
        if (Domicilio.Length > 0 && Dt_Reporte.Rows[0][Ope_Ate_Peticiones.Campo_Referencia].ToString().Length > 0)
        {
            Domicilio += "\\r\\n" + Dt_Reporte.Rows[0][Ope_Ate_Peticiones.Campo_Referencia].ToString();
        }
        else if (Domicilio.Length <= 0 && Dt_Reporte.Rows[0][Ope_Ate_Peticiones.Campo_Referencia].ToString().Length > 0)
        {
            Domicilio = Dt_Reporte.Rows[0][Ope_Ate_Peticiones.Campo_Referencia].ToString();
        }

        // insertar datos en la fila instanciada directamente de los controles en pantalla
        Dr_Fila_Reporte["Folio"] = Dt_Reporte.Rows[0][Ope_Ate_Peticiones.Campo_Folio].ToString();
        DateTime.TryParse(Dt_Reporte.Rows[0][Ope_Ate_Peticiones.Campo_Fecha_Peticion].ToString(), out Fecha_Peticion);
        Dr_Fila_Reporte["Fecha_Peticion"] = Fecha_Peticion.ToString("dd/MMM/yyyy");
        if (DateTime.TryParse(Dt_Reporte.Rows[0][Ope_Ate_Peticiones.Campo_Fecha_Solucion_Real].ToString(), out Fecha_Peticion))
        {
            Dr_Fila_Reporte["FECHA_SOLUCION_REAL"] = Fecha_Peticion.ToString("dd/MMM/yyyy");
        }
        Dr_Fila_Reporte["Nombre_Solicitante"] = Dt_Reporte.Rows[0][Ope_Ate_Peticiones.Campo_Apellido_Paterno].ToString()
            + " " + Dt_Reporte.Rows[0][Ope_Ate_Peticiones.Campo_Apellido_Materno].ToString()
            + " " + Dt_Reporte.Rows[0][Ope_Ate_Peticiones.Campo_Nombre_Solicitante].ToString();
        Dr_Fila_Reporte["Direccion"] = Domicilio;
        Dr_Fila_Reporte["E_mail"] = Dt_Reporte.Rows[0][Ope_Ate_Peticiones.Campo_Email].ToString();
        Dr_Fila_Reporte["Peticion"] = Dt_Reporte.Rows[0][Ope_Ate_Peticiones.Campo_Descripcion_Peticion].ToString();
        Dr_Fila_Reporte["Telefono"] = Dt_Reporte.Rows[0][Ope_Ate_Peticiones.Campo_Telefono].ToString();
        Dr_Fila_Reporte["Asunto"] = Dt_Reporte.Rows[0]["ASUNTO"].ToString();
        Dr_Fila_Reporte["Estatus"] = Dt_Reporte.Rows[0][Ope_Ate_Peticiones.Campo_Estatus].ToString();
        Dr_Fila_Reporte["Respuesta"] = Dt_Reporte.Rows[0][Ope_Ate_Peticiones.Campo_Descripcion_Solucion].ToString();
        Dr_Fila_Reporte["Solucion"] = Dt_Reporte.Rows[0][Ope_Ate_Peticiones.Campo_Descripcion_Solucion].ToString();
        Dr_Fila_Reporte["Dependencia"] = Dt_Reporte.Rows[0]["DEPENDENCIA"].ToString();
        // agregar fila a la tabla
        Ds_Reporte.Tables["DataTable1"].Rows.Add(Dr_Fila_Reporte);

        // establecer parámetros para consulta
        Obj_Peticiones.P_No_Peticion = Dt_Reporte.Rows[0][Ope_Ate_Peticiones.Campo_No_Peticion].ToString();
        Obj_Peticiones.P_Anio_Peticion = Convert.ToInt32(Dt_Reporte.Rows[0][Ope_Ate_Peticiones.Campo_Anio_Peticion].ToString());
        Obj_Peticiones.P_Programa_ID = Dt_Reporte.Rows[0][Ope_Ate_Peticiones.Campo_Programa_ID].ToString();

        // ejecutar consulta de seguimiento y observaciones
        Dt_Seguimiento = Obj_Peticiones.Consulta_Peticion_Seguimiento();
        Dt_Observaciones = Obj_Peticiones.Consulta_Observaciones_Peticion();
        // quitar tablas observaciones y seguimiento del dataset
        Ds_Reporte.Tables.Remove("Dt_Seguimiento");
        Ds_Reporte.Tables.Remove("Dt_Observaciones");
        Dt_Seguimiento.TableName = "Dt_Seguimiento";
        Dt_Observaciones.TableName = "Dt_Observaciones";
        // copiar tablas observaciones y seguimiento al dataset
        Ds_Reporte.Tables.Add(Dt_Seguimiento.Copy());
        Ds_Reporte.Tables.Add(Dt_Observaciones.Copy());

        return Ds_Reporte;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cargar_Peticiones
    ///DESCRIPCIÓN: Muestra los datos de la petición que recibe como parámetro en los controles correspondientes
    ///PARÁMETROS:
    ///         1. Dt_Peticiones: tabla con datos de la petición a cargar
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 07-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Cargar_Peticiones(DataTable Dt_Peticiones)
    {
        DateTime Fecha;
        try
        {
            if (Dt_Peticiones != null && Dt_Peticiones.Rows.Count > 0)
            {
                Txt_Folio_Peticion.Text = Dt_Peticiones.Rows[0][Ope_Ate_Peticiones.Campo_Folio].ToString();
                Txt_Solicitante.Text = Dt_Peticiones.Rows[0]["NOMBRE_COMPLETO_SOLICITANTE"].ToString();
                Txt_Sexo.Text = Dt_Peticiones.Rows[0][Ope_Ate_Peticiones.Campo_Sexo].ToString();
                Txt_Edad.Text = Dt_Peticiones.Rows[0][Ope_Ate_Peticiones.Campo_Edad].ToString();
                Txt_Domicilio.Text = Dt_Peticiones.Rows[0]["CALLE"].ToString()
                    + " " + Dt_Peticiones.Rows[0][Ope_Ate_Peticiones.Campo_Numero_Exterior].ToString()
                    + " " + Dt_Peticiones.Rows[0][Ope_Ate_Peticiones.Campo_Numero_Interior].ToString()
                    + " " + Dt_Peticiones.Rows[0]["COLONIA"].ToString();
                Txt_Referencia.Text = Dt_Peticiones.Rows[0][Ope_Ate_Peticiones.Campo_Referencia].ToString();
                Txt_Codigo_Postal.Text = Dt_Peticiones.Rows[0][Ope_Ate_Peticiones.Campo_Codigo_Postal].ToString();
                Txt_Email.Text = Dt_Peticiones.Rows[0][Ope_Ate_Peticiones.Campo_Email].ToString();
                Txt_Telefono.Text = Dt_Peticiones.Rows[0][Ope_Ate_Peticiones.Campo_Telefono].ToString();
                Txt_Peticion.Text = Dt_Peticiones.Rows[0][Ope_Ate_Peticiones.Campo_Descripcion_Peticion].ToString();
                Txt_Estatus.Text = Dt_Peticiones.Rows[0][Ope_Ate_Peticiones.Campo_Estatus].ToString();
                DateTime.TryParse(Dt_Peticiones.Rows[0][Ope_Ate_Peticiones.Campo_Fecha_Peticion].ToString(), out Fecha);
                Txt_Fecha_Peticion.Text = Fecha.ToString("dd/MMM/yyyy");
                // si hay una solución, mostrar campos de solución
                if (Dt_Peticiones.Rows[0][Ope_Ate_Peticiones.Campo_Descripcion_Solucion].ToString().Length > 0)
                {
                    DateTime.TryParse(Dt_Peticiones.Rows[0][Ope_Ate_Peticiones.Campo_Fecha_Solucion_Real].ToString(), out Fecha);
                    Txt_Fecha_Solucion.Text = Fecha.ToString("dd/MMM/yyyy");
                    Txt_Solucion.Text = Dt_Peticiones.Rows[0][Ope_Ate_Peticiones.Campo_Descripcion_Solucion].ToString();
                    Tr_Fecha_Solucion.Visible = true;
                    Tr_Txt_Solucion.Visible = true;
                }

                int Anio_Peticion;
                int.TryParse(Dt_Peticiones.Rows[0][Ope_Ate_Peticiones.Campo_Anio_Peticion].ToString(), out Anio_Peticion);
                // cargar detalles de seguimiento
                Cargar_Detalles_Seguimiento(
                    Dt_Peticiones.Rows[0][Ope_Ate_Peticiones.Campo_No_Peticion].ToString(),
                    Anio_Peticion,
                    Dt_Peticiones.Rows[0][Ope_Ate_Peticiones.Campo_Programa_ID].ToString()
                    );
            }
        }
        catch
        {
        }
    }

    ///****************************************************************************************
    ///NOMBRE_FUNCIÓN:Cargar_Detalles_Seguimiento
    ///DESCRIPCIÓN : Consulta la tabla seguimiento y observaciones y muestra los resultados 
    ///             en los grids correspondientes
    ///PARAMETROS  : 
    /// 		1. No_Peticion: número de petición a consultar
    /// 		2. Anio_Peticion: año de la petición a consultar
    /// 		3. Programa_ID: id del programa de la petición a consultar
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 07-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Cargar_Detalles_Seguimiento(string No_Peticion, int Anio_Peticion, string Programa_ID)
    {
        var Obj_Peticiones = new Cls_Cat_Ate_Peticiones_Negocio();
        DataTable Dt_Seguimiento;
        DataTable Dt_Observaciones;

        // establecer parámetros para consulta
        Obj_Peticiones.P_No_Peticion = No_Peticion;
        Obj_Peticiones.P_Anio_Peticion = Anio_Peticion;
        Obj_Peticiones.P_Programa_ID = Programa_ID;

        // consultar, ordenar y mostrar en el grid
        Dt_Seguimiento = Obj_Peticiones.Consulta_Peticion_Seguimiento();
        Dt_Seguimiento.DefaultView.Sort = Ope_Ate_Seguimiento_Peticiones.Campo_Fecha_Asignacion + " DESC";
        Grid_Seguimiento.DataSource = Dt_Seguimiento;
        Grid_Seguimiento.DataBind();

        // consultar, ordenar y mostrar en el grid
        Dt_Observaciones = Obj_Peticiones.Consulta_Observaciones_Peticion();
        Dt_Observaciones.DefaultView.Sort = Ope_Ate_Observaciones_Peticiones.Campo_Fecha + " DESC";
        Grid_Observaciones.DataSource = Dt_Observaciones;
        Grid_Observaciones.DataBind();

        // si el grid observaciones no contiene filas, ocultar contenedor del grid
        if (Grid_Observaciones.Rows.Count <= 0)
        {
            Contenedor_Grid_Historial.Visible = false;
        }
        else
        {
            Contenedor_Grid_Historial.Visible = true;
        }
    }

    #endregion METODOS

    #region Eventos

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Redirecciona a la página de login de ventanilla
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 28-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("Frm_Apl_Login_Ventanilla.aspx");
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN: Busca peticiones con el folio ingresado y lo muestra, si no encuentra folio, muestra mensaje
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 28-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        var Obj_Peticiones = new Cls_Cat_Ate_Peticiones_Negocio();

        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            // limpiar campos
            Limpiar_Campos();

            // si se ingresó un folio buscar la petición
            if (Txt_Folio_Peticion.Text.Trim().Length > 0)
            {
                Obj_Peticiones.P_Filtros_Dinamicos = " UPPER(" + Ope_Ate_Peticiones.Campo_Folio + ") = UPPER('" + Txt_Folio_Peticion.Text.Trim() + "') AND " + Ope_Ate_Peticiones.Campo_Estatus + " != 'TERMINADA' ";
                DataTable Dt_Peticiones = Obj_Peticiones.Consulta_Peticion();
                // si se obtuvieron resultados, mostrar como pdf, si no, mostrar mensaje
                if (Dt_Peticiones != null && Dt_Peticiones.Rows.Count > 0)
                {
                    Cargar_Peticiones(Dt_Peticiones);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Consulta de peticiones", "alert('No se encontraron peticiones con el folio proporcionado.');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Consulta de peticiones", "alert('Debe proporcionar el folio de la petición.');", true);
            }
        }
        catch (Exception ex)
        {
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message;
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Imprimir_Click
    ///DESCRIPCIÓN: imprimir datos de la petición que se muestra en pantalla
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 28-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        var Obj_Peticiones = new Cls_Cat_Ate_Peticiones_Negocio();

        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            // si se ingresó un folio buscar la petición
            if (Txt_Folio_Peticion.Text.Trim().Length > 0)
            {
                Obj_Peticiones.P_Filtros_Dinamicos = " UPPER(" + Ope_Ate_Peticiones.Campo_Folio + ") = UPPER('" + Txt_Folio_Peticion.Text.Trim() + "') AND " + Ope_Ate_Peticiones.Campo_Estatus + " != 'TERMINADA' ";
                DataTable Dt_Peticiones = Obj_Peticiones.Consulta_Peticion();
                // si se obtuvieron resultados, mostrar como pdf, si no, mostrar mensaje
                if (Dt_Peticiones != null && Dt_Peticiones.Rows.Count > 0)
                {
                    Imprimir_Reporte(Crear_Ds_Imprimir_Reporte(Dt_Peticiones), "Rpt_Ope_Ven_Peticion_Ciudadana.rpt", "Folio_Reporte_Ciudadano");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Consulta de peticiones", "alert('No se encontraron peticiones con el folio proporcionado.');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Consulta de peticiones", "alert('Debe proporcionar el folio de la petición.');", true);
            }
        }
        catch (Exception ex)
        {
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message;
        }
    }

    #endregion Eventos

}
