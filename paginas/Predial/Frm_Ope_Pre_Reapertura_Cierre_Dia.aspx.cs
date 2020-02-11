using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class paginas_Predial_Frm_Ope_Pre_Reapertura_Cierre_Dia : System.Web.UI.Page
{
    #region Pago_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS:     
    ///CREO: 
    ///FECHA_CREO: 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************        
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Configuracion_Formulario(true);
        }
        Div_Contenedor_Msj_Error.Visible = false;
    }

    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Configuracion_Formulario
    ///DESCRIPCIÓN          : Carga una configuracion de los controles del Formulario
    ///PARAMETROS           : 1. Estatus.    Estatus en el que se cargara la configuración de los controles.
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Estatus)
    {
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Limpiar_Campos
    ///DESCRIPCIÓN          : Limpia los controles del Formulario
    ///PARAMETROS           :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Campos()
    {
    }

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Componentes
    ///DESCRIPCIÓN          : Hace una validacion de que haya datos en los componentes antes de hacer una operación.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        //if (Txt_Cuenta_Predial.Text.Trim().Equals(""))
        //{
        //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //    Mensaje_Error = Mensaje_Error + "+ Indique la Cuenta Predial.";
        //    Validacion = false;
        //}
        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }
    #endregion

    #endregion

    #region Grids

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Nuevo_Click
    ///DESCRIPCIÓN          : Deja los componentes listos para dar de Alta un nuevo Impuestos_Fraccionamiento
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                Configuracion_Formulario(false);
                Limpiar_Campos();
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.Visible = false;
                Btn_Imprimir.Visible = false;
            }
            else
            {
                if (Validar_Componentes())
                {
                    Configuracion_Formulario(true);
                    Limpiar_Campos();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Impuestos de Fraccionamientos", "alert('Alta de Impuestos de Fraccionamiento Exitosa');", true);
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.Visible = true;
                    Btn_Imprimir.Visible = true;
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Modificar_Click
    ///DESCRIPCIÓN          : Deja los componentes listos para hacer la modificacion de un Impuestos_Fraccionamiento.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Modificar.AlternateText.Equals("Modificar"))
            {
                Btn_Modificar.AlternateText = "Actualizar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Nuevo.Visible = false;
                Btn_Imprimir.Visible = false;
                Configuracion_Formulario(false);
            }
            else
            {
                if (Validar_Componentes())
                {
                    Limpiar_Campos();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Impuestos de Fraccionamientos", "alert('Actualización de Impuestos de Fraccionamiento Exitosa');", true);
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Nuevo.Visible = true;
                    Btn_Imprimir.Visible = true;
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Configuracion_Formulario(true);
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Constancia_Propiedad_Click
    ///DESCRIPCIÓN          : Llena la Tabla con la opcion buscada
    ///PARAMETROS          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Limpiar_Campos();
            Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda de \"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
            Lbl_Mensaje_Error.Text = "";
            //Lbl_Mensaje_Error.Text = "(Se cargarón todos los Impuestos de Fraccionamientos encontrados)";
            Div_Contenedor_Msj_Error.Visible = true;
            Txt_Busqueda.Text = "";
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }

    }

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN : Btn_Eliminar_Click
    /////DESCRIPCIÓN          : Elimina un Impuestos_Fraccionamiento de la Base de Datos
    /////PARAMETROS          :     
    /////CREO                 : Antonio Salvador Benavides Guardado
    /////FECHA_CREO           : 22/Julio/2011
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Btn_Eliminar_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (Grid_Impuestos_Fraccionamientos.Rows.Count > 0 && Grid_Impuestos_Fraccionamientos.SelectedIndex > (-1))
    //        {
    //            Cls_Ope_Pre_Constancias_Negocio Impuestos_Fraccionamiento = new Cls_Ope_Pre_Constancias_Negocio();
    //            Impuestos_Fraccionamiento.P_Folio = Grid_Impuestos_Fraccionamientos.SelectedRow.Cells[3].Text;
    //            if (Impuestos_Fraccionamiento.Eliminar_Constancia_Propiedad())
    //            {
    //                Grid_Impuestos_Fraccionamientos.SelectedIndex = (-1);
    //                Llenar_Tabla_Constancias_Propiedad(Grid_Impuestos_Fraccionamientos.PageIndex);
    //                ScriptManager.RegisterStartupScript(this, this.GetType(), "Impuestos de Fraccionamientos", "alert('Impuestos de Fraccionamiento fue Eliminada Exitosamente');", true);
    //                Limpiar_Catalogo();
    //            }
    //            else
    //            {
    //                ScriptManager.RegisterStartupScript(this, this.GetType(), "Impuestos de Fraccionamientos", "alert('La Impuestos de Fraccionamiento No fue Eliminada');", true);
    //            }
    //        }
    //        else
    //        {
    //            Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Eliminar.";
    //            Lbl_Mensaje_Error.Text = "";
    //            Div_Contenedor_Msj_Error.Visible = true;
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //        Lbl_Mensaje_Error.Text = "";
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }

    //}

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
    ///DESCRIPCIÓN          : Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
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
        else if (Btn_Salir.AlternateText == "Atrás")
        {
            Btn_Salir.AlternateText = "Salir";
        }
        else
        {
            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Btn_Imprimir.Visible = true;
            Configuracion_Formulario(true);
            Limpiar_Campos();
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Btn_Salir.AlternateText = "Salir";
        }
    }

    #endregion

    #region Cálculos

    #endregion

    #region Impresion Folios

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Imprimir_Reporte(DataTable Dt_Impuestos_Fraccionamientos, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        Reporte.SetDataSource(Dt_Impuestos_Fraccionamientos);

        String Archivo_PDF = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar        
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_PDF);
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;

        Reporte.Export(Export_Options);
        Mostrar_Reporte(Archivo_PDF, "PDF");
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Imprimir_Reporte(DataSet Ds_Impuestos_Fraccionamientos, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);

        try
        {
            Reporte.Load(File_Path);
            Reporte.SetDataSource(Ds_Impuestos_Fraccionamientos);
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
        catch //(Exception Ex)
        {
            //Lbl_Mensaje_Error.Visible = true;
            //Lbl_Mensaje_Error.Text = "No se pudo exportar el reporte";
        }

        try
        {
            Mostrar_Reporte(Archivo_PDF, "PDF");
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
    private void Mostrar_Reporte(String Nombre_Reporte, String Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            if (Formato == "PDF")
            {
                Pagina = Pagina + Nombre_Reporte;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt", "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
            else if (Formato == "Excel")
            {
                String Ruta = "../../Reporte/" + Nombre_Reporte;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Dt_Constancias_Propiedad
    ///DESCRIPCIÓN          : Crea un DataTable con las columnas y datos de la Impuestos de Fraccionamiento Seleccionada en el GridView
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Crear_Dt_Constancias_Propiedad(int Indice_Fila)
    {
        Ds_Pre_Constancias Ds_Impuestos_Fraccionamientos = new Ds_Pre_Constancias();
        //DataRow Dr_Constancias_Propiedad;

        ////Inserta los datos de la Impuestos de Fraccionamiento en la Tabla
        //Dr_Constancias_Propiedad = Ds_Impuestos_Fraccionamientos.Tables["Dt_Constancias_Propiedad"].NewRow();
        //Dr_Constancias_Propiedad["Cuenta_Predial"] = Grid_Detalle_Impuesto_Fraccionamiento.Rows[Indice_Fila].Cells[1].Text;
        //Dr_Constancias_Propiedad["Propietario"] = Grid_Detalle_Impuesto_Fraccionamiento.Rows[Indice_Fila].Cells[2].Text;
        //Dr_Constancias_Propiedad["Folio"] = Grid_Detalle_Impuesto_Fraccionamiento.Rows[Indice_Fila].Cells[3].Text;
        //Dr_Constancias_Propiedad["Fecha"] = Grid_Detalle_Impuesto_Fraccionamiento.Rows[Indice_Fila].Cells[4].Text;

        //Ds_Impuestos_Fraccionamientos.Tables["Dt_Constancias_Propiedad"].Rows.Add(Dr_Constancias_Propiedad);

        return Ds_Impuestos_Fraccionamientos.Tables["Dt_Constancias_Propiedad"];
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Constancias_Propiedad
    ///DESCRIPCIÓN          : Crea un DataTable con las columnas y datos de la Impuestos de Fraccionamiento Seleccionada en el GridView
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Impuestos_Fraccionamientos()
    {
        Ds_Pre_Impuestos_Fraccionamientos Ds_Impuestos_Fraccionamientos = new Ds_Pre_Impuestos_Fraccionamientos();

        //Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio Impuestos_Fraccionamientos = new Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio();
        //Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        //Cls_Cat_Pre_Tipos_Predio_Negocio Tipos_Predio = new Cls_Cat_Pre_Tipos_Predio_Negocio();
        //Cls_Cat_Pre_Fraccionamientos_Negocio Fraccionamientos = new Cls_Cat_Pre_Fraccionamientos_Negocio();

        ////DataTable Dt_Impuestos_Fraccionamientos;
        //DataTable Dt_Cuenta_Predial = null;
        ////DataTable Dt_Tipo_Predio;
        //DataTable Dt_Temp;
        //DataTable Dt_Temp_Detalles = null;
        //DataRow Dr_Impuestos_Fraccionameinto;

        //String Impuestos_Fraccionamientos_ID = "";

        ////Cuentas_Predial.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
        ////Dt_Cuenta_Predial = Cuentas_Predial.Consultar_Cuenta();

        ////Tipos_Predio.P_Filtros_Dinamicos = Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " = '" + Dt_Cuenta_Predial.Rows[0][Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID] + "'";
        ////Dt_Tipo_Predio = Tipos_Predio.Consultar_Tipo_Predio();

        //foreach (DataTable Dt_Impuestos_Fraccionamientos in Ds_Impuestos_Fraccionamientos.Tables)
        //{
        //    if (Dt_Impuestos_Fraccionamientos.TableName == "Dt_Impuestos_Fraccionamientos")
        //    {
        //        Impuestos_Fraccionamientos.P_No_Impuesto_Fraccionamiento = Hdf_No_Impuesto_Fraccionamiento.Value;
        //        Dt_Temp = Impuestos_Fraccionamientos.Consultar_Impuestos_Fraccionamiento();
        //        Dt_Temp_Detalles = Impuestos_Fraccionamientos.P_Dt_Detalles_Impuestos_Fraccionamiento;

        //        foreach (DataRow Dr_Temp in Dt_Temp.Rows)
        //        {
        //            //Inserta los datos de los Impuestos de Fraccionamiento en la Tabla
        //            Dr_Impuestos_Fraccionameinto = Dt_Impuestos_Fraccionamientos.NewRow();
        //            Dr_Impuestos_Fraccionameinto["NO_IMPUESTO_FRACCIONAMIENTO"] = Dr_Temp[Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento];
        //            Dr_Impuestos_Fraccionameinto["CUENTA_PREDIAL_ID"] = Dr_Temp[Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID];
        //            Dr_Impuestos_Fraccionameinto["FECHA_VENCIMIENTO"] = Dr_Temp[Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Vencimiento].ToString();
        //            Dr_Impuestos_Fraccionameinto["ESTATUS"] = Dr_Temp[Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus];
        //            Dr_Impuestos_Fraccionameinto["OBSERVACIONES"] = Dr_Temp[Ope_Pre_Impuestos_Fraccionamientos.Campo_Observaciones];
        //            Dr_Impuestos_Fraccionameinto["FECHA_ELABORACION"] = Dr_Temp[Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Creo].ToString();
        //            Dr_Impuestos_Fraccionameinto["FECHA_OFICIO"] = Dr_Temp[Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Oficio].ToString().Substring(0, 10);
        //            Dr_Impuestos_Fraccionameinto["ELABORO"] = Dr_Temp[Ope_Pre_Impuestos_Fraccionamientos.Campo_Usuario_Creo];
        //            Dr_Impuestos_Fraccionameinto["UBICACION"] = Txt_Colonia.Text + ", " + Txt_Calle.Text + ", NO. EXT " + Txt_No_Exterior.Text + ", NO. INT " + Txt_No_Interior.Text;
        //            Dr_Impuestos_Fraccionameinto["PROPIETARIO"] = Txt_Propietario.Text;
        //            Dt_Impuestos_Fraccionamientos.Rows.Add(Dr_Impuestos_Fraccionameinto);
        //        }
        //    }
        //    if (Dt_Impuestos_Fraccionamientos.TableName == "Dt_Detalles_Impuestos_Fraccionamientos")
        //    {
        //        //Impuestos_Fraccionamientos.P_No_Impuesto_Fraccionamiento = Hdf_No_Impuesto_Fraccionamiento.Value;
        //        //Impuestos_Fraccionamientos.Consultar_Impuestos_Fraccionamiento();
        //        //Dt_Temp = Impuestos_Fraccionamientos.P_Dt_Detalles_Impuestos_Fraccionamiento;

        //        foreach (DataRow Dr_Temp in Dt_Temp_Detalles.Rows)
        //        {
        //            //Inserta los datos de los Impuestos de Fraccionamiento en la Tabla
        //            Dr_Impuestos_Fraccionameinto = Dt_Impuestos_Fraccionamientos.NewRow();
        //            Dr_Impuestos_Fraccionameinto["NO_IMPUESTO_FRACCIONAMIENTO"] = Dr_Temp[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento];
        //            Dr_Impuestos_Fraccionameinto["SUPERFICIE_FRACCIONAR"] = Dr_Temp[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Superficie_Fraccionar];
        //            Dr_Impuestos_Fraccionameinto["DESCRIPCION_MONTO"] = Dr_Temp["DESCRIPCION_MONTO"];
        //            Dr_Impuestos_Fraccionameinto["IMPUESTO_FRACCIONAMIENTO_ID"] = Dr_Temp[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Impuesto_Fraccionamiento_ID];
        //            Dr_Impuestos_Fraccionameinto["IMPORTE"] = Dr_Temp[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Importe];
        //            Dr_Impuestos_Fraccionameinto["RECARGOS"] = Dr_Temp[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Recargos];
        //            Dr_Impuestos_Fraccionameinto["TOTAL"] = Dr_Temp[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Total];
        //            Dt_Impuestos_Fraccionamientos.Rows.Add(Dr_Impuestos_Fraccionameinto);
        //            Impuestos_Fraccionamientos_ID += "'" + Dr_Temp[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Impuesto_Fraccionamiento_ID] + "', ";
        //        }
        //        if (Impuestos_Fraccionamientos_ID.EndsWith("', "))
        //        {
        //            Impuestos_Fraccionamientos_ID = Impuestos_Fraccionamientos_ID.Substring(0, Impuestos_Fraccionamientos_ID.Length - 2);
        //        }
        //    }
        //    if (Dt_Impuestos_Fraccionamientos.TableName == "Dt_Cuentas_Predial")
        //    {
        //        Cuentas_Predial.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
        //        Dt_Temp = Cuentas_Predial.Consultar_Cuenta();
        //        Dt_Cuenta_Predial = Dt_Temp;

        //        foreach (DataRow Dr_Temp in Dt_Temp.Rows)
        //        {
        //            //Inserta los datos de los Impuestos de Fraccionamiento en la Tabla
        //            Dr_Impuestos_Fraccionameinto = Dt_Impuestos_Fraccionamientos.NewRow();
        //            Dr_Impuestos_Fraccionameinto["CUENTA_PREDIAL_ID"] = Dr_Temp[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID];
        //            Dr_Impuestos_Fraccionameinto["CUENTA_PREDIAL"] = Dr_Temp[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial];
        //            Dr_Impuestos_Fraccionameinto["TIPO_PREDIO_ID"] = Dr_Temp[Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID];
        //            Dr_Impuestos_Fraccionameinto["SUPERFICIE_CONSTRUIDA"] = Dr_Temp[Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida];
        //            Dt_Impuestos_Fraccionamientos.Rows.Add(Dr_Impuestos_Fraccionameinto);
        //        }
        //    }
        //    if (Dt_Impuestos_Fraccionamientos.TableName == "Dt_Tipos_Predio")
        //    {
        //        Tipos_Predio.P_Filtros_Dinamicos = Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " = '" + Dt_Cuenta_Predial.Rows[0][Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID] + "'";
        //        Dt_Temp = Tipos_Predio.Consultar_Tipo_Predio();

        //        foreach (DataRow Dr_Temp in Dt_Temp.Rows)
        //        {
        //            //Inserta los datos de los Impuestos de Fraccionamiento en la Tabla
        //            Dr_Impuestos_Fraccionameinto = Dt_Impuestos_Fraccionamientos.NewRow();
        //            Dr_Impuestos_Fraccionameinto["TIPO_PREDIO_ID"] = Dr_Temp[Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID];
        //            Dr_Impuestos_Fraccionameinto["DESCRIPCION"] = Dr_Temp[Cat_Pre_Tipos_Predio.Campo_Descripcion];
        //            Dt_Impuestos_Fraccionamientos.Rows.Add(Dr_Impuestos_Fraccionameinto);
        //        }
        //    }
        //    if (Dt_Impuestos_Fraccionamientos.TableName == "Dt_Fraccionamientos")
        //    {
        //        Fraccionamientos.P_Fraccionamiento_ID = "IN (SELECT " + Cat_Pre_Fracc_Impuestos.Campo_Fraccionamiento_ID + " FROM " + Cat_Pre_Fracc_Impuestos.Tabla_Cat_Pre_Fracc_Impuestos + " WHERE " + Cat_Pre_Fracc_Impuestos.Campo_Impuesto_Fraccionamiento_ID + " IN (" + Impuestos_Fraccionamientos_ID + "))";
        //        Dt_Temp = Fraccionamientos.Consultar_Fraccionamientos();

        //        foreach (DataRow Dr_Temp in Dt_Temp.Rows)
        //        {
        //            //Inserta los datos de los Impuestos de Derechos de Supervision en la Tabla
        //            Dr_Impuestos_Fraccionameinto = Dt_Impuestos_Fraccionamientos.NewRow();
        //            Dr_Impuestos_Fraccionameinto["FRACCIONAMIENTO_ID"] = Dr_Temp[Cat_Pre_Fraccionamientos.Campo_Fraccionamiento_ID];
        //            Dr_Impuestos_Fraccionameinto["IDENTIFICADOR"] = Dr_Temp[Cat_Pre_Fraccionamientos.Campo_Identificador];
        //            Dr_Impuestos_Fraccionameinto["DESCRIPCION"] = Dr_Temp[Cat_Pre_Fraccionamientos.Campo_Descripcion];
        //            Dt_Impuestos_Fraccionamientos.Rows.Add(Dr_Impuestos_Fraccionameinto);
        //        }
        //    }
        //    if (Dt_Impuestos_Fraccionamientos.TableName == "Dt_Fraccionamientos_Impuestos")
        //    {
        //        Fraccionamientos.P_Fraccionamiento_Impuesto_ID = "IN (" + Impuestos_Fraccionamientos_ID + ")";
        //        Fraccionamientos = Fraccionamientos.Consultar_Datos_Fraccionamiento();
        //        Dt_Temp = Fraccionamientos.P_Fraccionamientos_Impuestos;

        //        foreach (DataRow Dr_Temp in Dt_Temp.Rows)
        //        {
        //            //Inserta los datos de los Impuestos de Derechos de Supervision en la Tabla
        //            Dr_Impuestos_Fraccionameinto = Dt_Impuestos_Fraccionamientos.NewRow();
        //            Dr_Impuestos_Fraccionameinto["IMPUESTO_FRACCIONAMIENTO_ID"] = Dr_Temp[Cat_Pre_Fracc_Impuestos.Campo_Impuesto_Fraccionamiento_ID];
        //            Dr_Impuestos_Fraccionameinto["FRACCIONAMIENTO_ID"] = Dr_Temp[Cat_Pre_Fracc_Impuestos.Campo_Fraccionamiento_ID];
        //            Dr_Impuestos_Fraccionameinto["ANIO"] = Dr_Temp[Cat_Pre_Fracc_Impuestos.Campo_Año];
        //            Dr_Impuestos_Fraccionameinto["MONTO"] = Dr_Temp[Cat_Pre_Fracc_Impuestos.Campo_Monto];
        //            Dt_Impuestos_Fraccionamientos.Rows.Add(Dr_Impuestos_Fraccionameinto);
        //        }
        //    }
        //}

        return Ds_Impuestos_Fraccionamientos;
    }

    #endregion
}
