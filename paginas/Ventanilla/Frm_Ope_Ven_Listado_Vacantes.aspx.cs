using System;
using System.Data;
using System.Web.UI;
using Presidencia.Operacion_Atencion_Ciudadana_Vacantes.Negocio;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using Presidencia.Constantes;
using Presidencia.Sessiones;

public partial class paginas_Ventanilla_Frm_Ope_Ven_Listado_Vacantes : System.Web.UI.Page
{
    private const string Contacto_Municipal = "Para mayor información de este empleo, favor de acudir a\\r\\nLas ventanillas de Atención Ciudadana ubicadas en\\r\\nCalle Hidalgo #77 Colonia Centro, Irapuato, Gto.";

    #region Page load
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            Cls_Sessiones.Nombre_Empleado = "";
            Cls_Sessiones.Rol_ID = "";
            Cls_Sessiones.Empleado_ID = "";
            Cls_Sessiones.No_Empleado = "";
            Cls_Sessiones.Dependencia_ID_Empleado = "";
            Cls_Sessiones.Area_ID_Empleado = "";
            Cls_Sessiones.Mostrar_Menu = true;

            Grid_Vacantes.DataSource = Consultar_Vacantes();
            Grid_Vacantes.DataBind();
        }
    }
    #endregion Page load

    #region METODOS

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Consultar_Vacantes
    ///DESCRIPCIÓN: Ejecuta consulta de vacantes y regresa un datatable con los resultados de la consulta
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 01-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataTable Consultar_Vacantes()
    {
        var Obj_Vacantes = new Cls_Ope_Ate_Vacantes_Negocio();
        DataTable Dt_Vacantes;

        // si hay un filtro seleccionado, agrgar
        if (Cmb_Filtro.SelectedIndex > 0 && Txt_Busqueda.Text.Trim().Length > 0)
        {
            switch (Cmb_Filtro.SelectedValue)
            {
                case "VACANTE":
                    Obj_Vacantes.P_Nombre_Vacante = Txt_Busqueda.Text.Trim();
                    break;
                case "SEXO":
                    Obj_Vacantes.P_Sexo = Txt_Busqueda.Text.Trim();
                    break;
                case "ESCOLARIDAD":
                    Obj_Vacantes.P_Escolaridad = Txt_Busqueda.Text.Trim();
                    break;
                case "EXPERIENCIA":
                    Obj_Vacantes.P_Experiencia = Txt_Busqueda.Text.Trim();
                    break;
            }
        }

        Dt_Vacantes = Obj_Vacantes.Consultar_Vacantes();
        Dt_Vacantes.DefaultView.Sort = Ope_Ate_Vacantes.Campo_No_Vacante + " ASC";
        return Dt_Vacantes;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Exportar_Reporte
    ///DESCRIPCIÓN: Genera el reporte de Crystal con los datos proporcionados en el DataTable
    ///PARÁMETROS:
    /// 		1. Ds_Reporte: Dataset con datos a imprimir
    /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
    /// 		3. Nombre_Archivo: Nombre del archivo a generar
    /// 		4. Extension_Archivo: extensión del archivo a generar
    /// 		5. Formato: formato al que se va a exportar el reporte (excel o pdf)
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 01-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Exportar_Reporte(DataSet Ds_Reporte, String Nombre_Reporte, String Nombre_Archivo, String Extension_Archivo, ExportFormatType Formato)
    {
        ReportDocument Reporte = new ReportDocument();
        String Ruta = Server.MapPath("../Rpt/Atencion_Ciudadana/" + Nombre_Reporte);

        try
        {
            // si la tabla no trae datos, mostrar mensaje
            if (Ds_Reporte != null && Ds_Reporte.Tables.Count > 0 && Ds_Reporte.Tables[0].Rows.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('No se encontraron registros con el criterio seleccionado.');", true);
                return;
            }

            Reporte.Load(Ruta);
            Reporte.SetDataSource(Ds_Reporte);

            String Archivo_Convenio = Nombre_Archivo + "." + Extension_Archivo;  // formar el nombre del archivo a generar 

            ExportOptions Export_Options_Calculo = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options_Calculo = new DiskFileDestinationOptions();
            Disk_File_Destination_Options_Calculo.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_Convenio);
            Export_Options_Calculo.ExportDestinationOptions = Disk_File_Destination_Options_Calculo;
            Export_Options_Calculo.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options_Calculo.ExportFormatType = Formato;
            Reporte.Export(Export_Options_Calculo);

            Mostrar_Reporte(Archivo_Convenio, "Reporte");
        }
        catch
        {
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Mostrar_Reporte
    ///DESCRIPCIÓN: Visualiza en pantalla el reporte indicado
    ///PARÁMETROS:
    /// 		1. Nombre_Reporte: Nombre del reporte a generar
    /// 		2. Tipo: parámetro para ventana modal en la que se mostrará el archivo
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 01-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Mostrar_Reporte(String Nombre_Reporte, String Tipo)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "Formato",
                "window.open('" + Pagina +
                "', '" + Tipo + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')",
                true
                );
        }
        catch
        {
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Crear_Ds_Exportar_Reporte
    ///DESCRIPCIÓN: Crea un Dataset con los datos de la consulta de convenios
    ///PARÁMETROS:
    ///         1. No_Vacante: número de vacante a consultar, si no se especifica, se toma la tabla desde sesión
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 01-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataSet Crear_Ds_Exportar_Reporte(string No_Vacante)
    {
        var Ds_Vacantes = new Ds_Ope_Ate_Vacantes();
        DataTable Dt_Vacantes;

        var Obj_Vacantes = new Cls_Ope_Ate_Vacantes_Negocio();
        Obj_Vacantes.P_No_Vacante = No_Vacante;
        //// consultar vacantes
        Dt_Vacantes = Obj_Vacantes.Consultar_Vacantes();

        if (Dt_Vacantes != null)
        {
            // actualizar dato de contacto (mostrar mensaje único en lugar de datos de contacto)
            foreach (DataRow Dr_Vacante in Dt_Vacantes.Rows)
            {
                Dr_Vacante[Ope_Ate_Vacantes.Campo_Contacto] = "Para mayor información de este empleo, favor de acudir a\\r\\nLas ventanillas de Atención Ciudadana ubicadas en:\\r\\nCalle Hidalgo #77 Colonia Centro, Irapuato, Gto.";
            }
            // cargar al dataset para el reporte
            Dt_Vacantes.TableName = "Dt_Vacantes";
            Ds_Vacantes.Tables.Remove("Dt_Vacantes");
            Ds_Vacantes.Tables.Add(Dt_Vacantes.Copy());

            DataRow Dr_Fila_Datos_Contacto = Ds_Vacantes.Tables["Dt_Generales"].NewRow();
            Dr_Fila_Datos_Contacto[0] = Contacto_Municipal;
            Ds_Vacantes.Tables["Dt_Generales"].Rows.Add(Dr_Fila_Datos_Contacto);
        }

        return Ds_Vacantes;
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
        try
        {
            DataTable Dt_Vacantes = Consultar_Vacantes();
            // si se obtuvieron resultados, mostrar como pdf, si no, mostrar mensaje
            if (Dt_Vacantes != null && Dt_Vacantes.Rows.Count > 0)
            {
                Grid_Vacantes.DataSource = Dt_Vacantes;
                Grid_Vacantes.DataBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Consulta de peticiones", "alert('No se encontraron vacantes con el criterio de búsqueda proporcionado.');", true);
            }
        }
        catch
        {
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Grid_Contacto_Vacante_Click
    /// 	DESCRIPCIÓN: Manejo del evento Click en el boton contacto del grid, 
    /// 	            Mostrar detalles del contacto en un mensaje de alerta javascript
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 01-jun-2012
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Grid_Contacto_Vacante_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton Btn_Grid_Contacto_Vacante = (ImageButton)sender;

        try
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('Contacto: \\r\\n" + Contacto_Municipal + "');", true);
        }
        catch
        {
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Grid_Imprimir_Vacante_Click
    /// 	DESCRIPCIÓN: Manejo del evento Click en el boton imprimir vacante del grid, 
    /// 	            Mandar impresión de la vacante
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 01-jun-2012
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Grid_Imprimir_Vacante_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton Btn_Grid_Imprimir_Vacante = (ImageButton)sender;

        try
        {
            Exportar_Reporte(Crear_Ds_Exportar_Reporte(Btn_Grid_Imprimir_Vacante.CommandArgument), "Rpt_Ope_Ate_Detalles_Vacante.rpt", "Detalles_Vacante", "pdf", ExportFormatType.PortableDocFormat);
        }
        catch
        {
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Grid_Vacantes_RowDataBound
    /// 	DESCRIPCIÓN: Manejo del evento RowDataBound en el grid vacantes
    /// 	            al boton de impresión en cada fila agregar como argumento (propiedad CommandArgument del boton) 
    /// 	            el campo no_vacante para poder identificar la fila y al botón contacto el campo contacto
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 01-jun-2012
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Vacantes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var Btn_Grid_Imprimir_Vacante = (ImageButton)e.Row.FindControl("Btn_Grid_Imprimir_Vacante");

                DataRowView Dr_Fila_Vacante = (DataRowView)e.Row.DataItem;
                Btn_Grid_Imprimir_Vacante.CommandArgument = Dr_Fila_Vacante[Ope_Ate_Vacantes.Campo_No_Vacante].ToString();
            }
        }
        catch
        {
        }
    }

    #endregion Eventos

}
