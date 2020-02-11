using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Linq;
using Presidencia.Sessiones;

public partial class paginas_Atencion_Ciudadana_Frm_Cat_Ate_Plantillas_Documentos : System.Web.UI.Page
{
    // ruta del directorio con las plantillas (ruta relativa)
    private const string Ruta_Directorio_Plantillas = @"../../Archivos/Atencion_Ciudadana/PlantillasWord/";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Form.Enctype = "multipart/form-data";

        Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
        if (string.IsNullOrEmpty(Cls_Sessiones.Empleado_ID))
        {
            Response.Redirect("../Paginas_Generales/Frm_Login.aspx");
        }

        if (!Page.IsPostBack)
        {
            // llenar el grid con el listado de archivos en el directorio de plantillas
            Grid_Archivos.Columns[0].Visible = true;
            Grid_Archivos.DataSource = Consultar_Plantillas();
            Grid_Archivos.DataBind();
            Grid_Archivos.Columns[0].Visible = false;
        }
    }

    #region METODOS

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Mensaje_Error
    ///DESCRIPCIÓN: Muestra el mensaje recibido como parámetro o si es nulo o un texto vacío limpia el mensaje
    ///PARÁMETROS:
    /// 		1. Texto_Mensaje: texto a mostrar en el área de información
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 27-jul-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Mensaje_Error(String Texto_Mensaje)
    {
        // si no es nulo o texto vacío, mostrar mensaje
        if (!string.IsNullOrEmpty(Texto_Mensaje))
        {
            Img_Warning.Visible = true;
            Lbl_Warning.Visible = true;
            Lbl_Warning.Text = Texto_Mensaje;
        }
        else
        {
            Img_Warning.Visible = false;
            Lbl_Warning.Text = "";
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Crear_Tabla_Archivos
    ///DESCRIPCIÓN: Regresa un datatable con las columnas para contener datos de archivos
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 27-jul-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Llenar_Grid_Archivos()
    {
        Grid_Archivos.Columns[0].Visible = true;
        Grid_Archivos.DataSource = Consultar_Plantillas();
        Grid_Archivos.DataBind();
        Grid_Archivos.Columns[0].Visible = false;
        Txt_Busqueda.Focus();
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Crear_Tabla_Archivos
    ///DESCRIPCIÓN: Regresa un datatable con las columnas para contener datos de archivos
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 27-jul-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected DataTable Crear_Tabla_Archivos()
    {
        DataTable Dt_Archivos = new DataTable();
        Dt_Archivos.Columns.Add(new DataColumn("NOMBRE_ARCHIVO", typeof(String)));
        Dt_Archivos.Columns.Add(new DataColumn("RUTA_ARCHIVO", typeof(String)));
        return Dt_Archivos;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Consultar_Plantillas
    ///DESCRIPCIÓN: Obtiene el listado de archivos plantilla de word y lo regresa como un datatable
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 27-jul-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataTable Consultar_Plantillas()
    {
        DataTable Dt_Plantillas = null;
        List<string> Lista_Archivos;
        DataRow Dr_Archivo;
        string Nombre_Archivo;

        try
        {
            string Directorio_Plantillas = Server.MapPath(Ruta_Directorio_Plantillas);
            Dt_Plantillas = Crear_Tabla_Archivos();

            // si se especificó un texto en el campo de búsqueda, obtener el listado de archivos filtrado
            if (Txt_Busqueda.Text.Trim().Length > 0)
            {
                // obtener el listado de archivos en el directorio filtrado con contenido del campo de búsqueda
                Lista_Archivos = Directory.GetFiles(Directorio_Plantillas, "*" + Txt_Busqueda.Text.Trim() + "*", SearchOption.TopDirectoryOnly).ToList<string>();
            }
            else
            {
                // obtener el listado de archivos en el directorio sin filtrar
                Lista_Archivos = Directory.GetFiles(Directorio_Plantillas).ToList<string>();
            }

            // recorrer el listado de archivos y agregar como renglón a la tabla
            foreach (string Archivo in Lista_Archivos)
            {
                // validar que sea un archivo docx, y si es así, agregar nuevo renglón
                if (Archivo.EndsWith(".docx"))
                {
                    Nombre_Archivo = Path.GetFileName(Archivo);
                    Dr_Archivo = Dt_Plantillas.NewRow();
                    Dr_Archivo["NOMBRE_ARCHIVO"] = Nombre_Archivo;
                    Dr_Archivo["RUTA_ARCHIVO"] = Ruta_Directorio_Plantillas + Nombre_Archivo;
                    Dt_Plantillas.Rows.Add(Dr_Archivo);
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Consulta plantillas: " + Ex.Message);
        }
        return Dt_Plantillas;
    }

    #endregion METODOS

    #region EVENTOS

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN: Consultar archivos filtrando con el texto proporcionado
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 27-jul-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************   
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Mensaje_Error("");

        try
        {
            // llenar el grid con el listado de archivos en el directorio de plantillas
            Llenar_Grid_Archivos();
        }
        catch (Exception ex)
        {
            Mensaje_Error("Búsqueda: " + ex.Message);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: si el botón tiene el texto "Salir", se redirecciona a la página principal,
    ///             si no, se inicializan los controles
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 27-jul-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************   
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Actualizar_Archivo_Click
    ///DESCRIPCIÓN: Subir un archivo
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 27-jul-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************   
    protected void Btn_Actualizar_Archivo_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow Fila_Grid;
        ImageButton Btn_Actualizar = (ImageButton)sender;
        FileUpload Fup_Grid_Archivo_Subido;

        Mensaje_Error("");

        try
        {
            // recuperar la fila del grid con el control presionado
            Fila_Grid = (GridViewRow)FindControl(Btn_Actualizar.Parent.Parent.UniqueID);
            // comprobar que la fila contiene un valor diferente de nulo
            if (Fila_Grid != null)
            {
                // localizar el control fileupload en la fila del botón presionado
                Fup_Grid_Archivo_Subido = (FileUpload)Fila_Grid.Cells[2].FindControl("Fup_Archivo_Plantilla");
                // comprobar que se encontró el control
                if (Fup_Grid_Archivo_Subido != null)
                {
                    // comprobar que se recibe un archivo
                    if (Fup_Grid_Archivo_Subido.HasFile)
                    {
                        // comprobar que el archivo tiene el mismo nombre (comparar con la segunda celda de la fila), si no, mostrar mensaje
                        if (Fup_Grid_Archivo_Subido.FileName == Fila_Grid.Cells[1].Text)
                        {
                            string Ruta_Archivo = Server.MapPath(Fila_Grid.Cells[0].Text);
                            // borrar archivo antes de guardar nuevo
                            if (File.Exists(Ruta_Archivo))
                            {
                                File.Delete(Ruta_Archivo);
                            }
                            // guardar el archivo
                            Fup_Grid_Archivo_Subido.SaveAs(Ruta_Archivo);
                            // mostrar mensaje notificando que el archivo se guardó
                            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                "Plantillas Atencion_Ciudadana", "alert('El archivo ha sido actualizado con éxito.');", true);
                            Llenar_Grid_Archivos();
                        }
                        else
                        {
                            Llenar_Grid_Archivos();
                            Mensaje_Error("El nombre del archivo subido es diferente al del archivo en el servidor, verifique que sea el archivo correcto y vuelva a intentarlo.");
                        }
                    }
                    else
                    {
                        Llenar_Grid_Archivos();
                        Mensaje_Error("Se requiere especificar un archivo.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error("Actualizar archivo: " + ex.Message);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Grid_Archivos_OnRowDataBound
    /// DESCRIPCIÓN: Cambia el contenido de la columna archivo para que sólo contenga el nombre del archivo
    /// PARÁMETROS: NO APLICA
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 21-jun-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Archivos_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        Mensaje_Error("");

        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink Hl_Enlace_Plantilla = new HyperLink();
                Hl_Enlace_Plantilla.Text = e.Row.Cells[1].Text;
                Hl_Enlace_Plantilla.NavigateUrl = e.Row.Cells[0].Text;
                Hl_Enlace_Plantilla.CssClass = "Enlace_Archivo";
                // agregar enlace a la celda
                e.Row.Cells[1].Controls.Clear();
                e.Row.Cells[1].Controls.Add(Hl_Enlace_Plantilla);
                // agregar parámetro al botón
                ImageButton Btn_Grid_Actualizar_Archivo = (ImageButton)e.Row.FindControl("Btn_Actualizar_Archivo");
                // validar que se recuperó el control
                if (Btn_Grid_Actualizar_Archivo != null)
                {
                    ScriptManager1.RegisterPostBackControl(Btn_Grid_Actualizar_Archivo);
                    DataRowView Dr_Fila_Archivo = (DataRowView)e.Row.DataItem;
                }
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error("Grid_Archivos_OnRowDataBound: " + ex.Message);
        }
    }

    #endregion EVENTOS

}
