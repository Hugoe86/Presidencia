using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Operacion_Caj_Cierre_Dia.Negocio;
using System.Text;
using System.Data;
using Presidencia.Constantes;
using Presidencia.Catalogo_Cajas.Negocio;
using Presidencia.Operacion_Pre_Caj_Detalles.Negocio;
using Presidencia.Sessiones;

public partial class paginas_Predial_Frm_Ope_Pre_Rpt_Cajas_Detalles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataSet Foraneas;
        if (!IsPostBack)
        {
            Cls_Ope_Pre_Caj_Detalles_Negocio Foranea = new Cls_Ope_Pre_Caj_Detalles_Negocio();
            Foranea.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
            Foraneas = Foranea.Consulta_Caja();
            if (Foraneas != null)
            {
                if (Foraneas.Tables.Count > 0)
                {
                    if (Foraneas.Tables[0].Rows.Count > 0)
                    {
                        if (Foraneas.Tables[0].Rows[0]["foranea"].ToString() == "SI")
                        {

                            Configuracion_Formulario(true);
                        }
                        else
                        {
                            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                        }
                    }
                }
            }
        }
    }
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
        DataSet CAJA;
        Txt_Caja.Enabled = !Estatus;
        Rdb_Tipo_Reporte.SelectedValue = "1";
        Cls_Ope_Pre_Caj_Detalles_Negocio Cajas = new Cls_Ope_Pre_Caj_Detalles_Negocio();
        Cajas.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
        CAJA = Cajas.Consulta_Caja();
        Txt_Caja.Text = CAJA.Tables[0].Rows[0]["NO_CAJA"].ToString();
        Txt_Caja_ID.Value = CAJA.Tables[0].Rows[0]["CAJA_ID"].ToString();
        Txt_Fecha_Inicio.Value = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(CAJA.Tables[0].Rows[0]["APLICACION_PAGO"].ToString()));
        Txt_Caja.ReadOnly = true;
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
        Rdb_Tipo_Reporte.SelectedValue = "1";
        String Mensaje_Error = "";
        Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
        Div_Contenedor_Msj_Error.Visible = false;
    }
    #endregion
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
        String fecha = DateTime.Now.ToString();
        if (Txt_Caja.Text.Trim() == "")
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ NO tiene caja asignada .";
            Validacion = false;
        }
        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }
    #endregion
    #region "Evento"
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta 
    ///PROPIEDADES:     
    ///CREO: Sergio Manuel Gallardo
    ///FECHA_CREO: 11/octubre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        DataSet Ds_Reporte = null;
        DataTable Dt_Pagos = null;
        DataView Dt_Filtro = null;
        int Total;
        Total = 0;
        try
        {
            if (Validar_Componentes())
            {
                if (Convert.ToInt32(Rdb_Tipo_Reporte.SelectedValue) == 2)
                {
                    Cls_Ope_Pre_Caj_Detalles_Negocio Caj_Detalles = new Cls_Ope_Pre_Caj_Detalles_Negocio();
                    Ds_Reporte = new DataSet();
                    Caj_Detalles.P_Caja_Id = Txt_Caja_ID.Value;
                    Caj_Detalles.P_Fecha = Txt_Fecha_Inicio.Value;
                    Dt_Pagos = Caj_Detalles.Consulta_Pagos();
                    if (Dt_Pagos.Rows.Count > 0)
                    {
                        Dt_Pagos.TableName = "Dt_CAJA_PAGOS";
                        Ds_Reporte.Tables.Add(Dt_Pagos.Copy());
                        //Se llama al método que ejecuta la operación de generar el reporte.
                        Generar_Reporte(ref Ds_Reporte, "Rpt_Ope_Caj_Pagos_Detalle.rpt", "Reporte_Caja_Detalle" + String.Format("{0:ddMMMyyyy}", Convert.ToDateTime(Txt_Fecha_Inicio.Value)), ".pdf");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Reportes", "alert('La caja No." + Txt_Caja.Text + " no tiene movimientos el dia " + Txt_Fecha_Inicio.Value + "');", true);
                    }
                }
                else
                {
                    Cls_Ope_Pre_Caj_Detalles_Negocio Caj_General = new Cls_Ope_Pre_Caj_Detalles_Negocio();
                    Ds_Reporte = new DataSet();
                    Caj_General.P_Caja_Id = Txt_Caja_ID.Value;
                    Caj_General.P_Fecha = String.Format("{0:MM/dd/yyyy}", Convert.ToString(Txt_Fecha_Inicio.Value));
                    Caj_General.P_Fecha_Final = String.Format("{0:MM/dd/yyyy}", Convert.ToString(Txt_Fecha_Inicio.Value));
                    Dt_Pagos = Caj_General.Consulta_Pagos_General();
                    if (Dt_Pagos.Rows.Count > 0)
                    {
                        Dt_Pagos.TableName = "DT_Datos_Generales";
                        // en esta parte agregamos un dataview para obtener el total de los recibos generados en el cajero
                        Dt_Filtro = Dt_Pagos.DefaultView;
                        Total = Dt_Filtro.ToTable(true, "No_recibo").Rows.Count;
                        // reasignamos los valores a las columnas del datatable dt_pagos para poner la iinformacion correcta en el rpt
                        foreach (DataRow DR in Dt_Pagos.Rows)
                        {
                            DR["TOTAL_RECIBOS"] = Total.ToString();
                            DR["FECHA"] = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Txt_Fecha_Inicio.Value));
                            DR["FECHA_FINAL"] = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Txt_Fecha_Inicio.Value));
                        }
                        Ds_Reporte.Tables.Add(Dt_Pagos.Copy());
                        //Se llama al método que ejecuta la operación de generar el reporte.
                        Generar_Reporte(ref Ds_Reporte, "Rpt_Ope_Caj_Pagos_General.rpt", "Reporte_Caja_General" + String.Format("{0:ddMMMyyyy}", Convert.ToDateTime(Txt_Fecha_Inicio.Value)), ".pdf");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Reportes", "alert('La caja No." + Txt_Caja.Text + " no tiene movimientos el los dia  " + Txt_Fecha_Inicio.Value + "al " + Txt_Fecha_Inicio.Value + "');", true);
                    }
                }
                Configuracion_Formulario(true);
                Limpiar_Campos();
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            }
        }
        //}
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
            Lbl_Mensaje_Error.Visible = true;
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 02/Septiembre/2010 
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
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        }
    }

    #endregion
    #region Metodos Reportes
    /// *************************************************************************************
    /// NOMBRE:             Generar_Reporte
    /// DESCRIPCIÓN:        Método que invoca la generación del reporte.
    ///              
    /// PARÁMETROS:         Ds_Reporte_Crystal.- Es el DataSet con el que se muestra el reporte en cristal 
    ///                     Ruta_Reporte_Crystal.-  Ruta y Nombre del archivo del Crystal Report.
    ///                     Nombre_Reporte_Generar.- Nombre que tendrá el reporte generado.
    ///                     Formato.- Es el tipo de reporte "PDF", "Excel"
    /// USUARIO CREO:       Juan Alberto Hernández Negrete.
    /// FECHA CREO:         3/Mayo/2011 18:15 p.m.
    /// USUARIO MODIFICO:   Salvador Henrnandez Ramirez
    /// FECHA MODIFICO:     16/Mayo/2011
    /// CAUSA MODIFICACIÓN: Se cambio Nombre_Plantilla_Reporte por Ruta_Reporte_Crystal, ya que este contendrá tambien la ruta
    ///                     y se asigno la opción para que se tenga acceso al método que muestra el reporte en Excel.
    /// *************************************************************************************
    public void Generar_Reporte(ref DataSet Ds_Reporte_Crystal, String Ruta_Reporte_Crystal, String Nombre_Reporte_Generar, String Formato)
    {
        ReportDocument Reporte = new ReportDocument(); // Variable de tipo reporte.
        String Ruta = String.Empty;  // Variable que almacenará la ruta del archivo del crystal report. 

        try
        {
            Ruta = @Server.MapPath("../Rpt/Cajas/" + Ruta_Reporte_Crystal);
            Reporte.Load(Ruta);

            if (Ds_Reporte_Crystal is DataSet)
            {
                if (Ds_Reporte_Crystal.Tables.Count > 0)
                {
                    Reporte.SetDataSource(Ds_Reporte_Crystal);
                    Exportar_Reporte_PDF(Reporte, Nombre_Reporte_Generar);
                    Mostrar_Reporte(Nombre_Reporte_Generar, ".pdf");
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE:             Exportar_Reporte_PDF
    /// DESCRIPCIÓN:        Método que guarda el reporte generado en formato PDF en la ruta
    ///                     especificada.
    /// PARÁMETROS:         Reporte.- Objeto de tipo documento que contiene el reporte a guardar.
    ///                     Nombre_Reporte.- Nombre que se le dio al reporte.
    /// USUARIO CREO:       Juan Alberto Hernández Negrete.
    /// FECHA CREO:         3/Mayo/2011 18:19 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    public void Exportar_Reporte_PDF(ReportDocument Reporte, String Nombre_Reporte_Generar)
    {
        ExportOptions Opciones_Exportacion = new ExportOptions();
        DiskFileDestinationOptions Direccion_Guardar_Disco = new DiskFileDestinationOptions();
        PdfRtfWordFormatOptions Opciones_Formato_PDF = new PdfRtfWordFormatOptions();

        try
        {
            if (Reporte is ReportDocument)
            {
                Direccion_Guardar_Disco.DiskFileName = @Server.MapPath("../../Reporte/" + Nombre_Reporte_Generar);
                Opciones_Exportacion.ExportDestinationOptions = Direccion_Guardar_Disco;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                Reporte.Export(Opciones_Exportacion);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al exportar el reporte. Error: [" + Ex.Message + "]");
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
            Pagina = Pagina + Nombre_Reporte_Generar;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt_Cajas_Detalles",
                "window.open('" + Pagina + "', 'Cajas','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    #endregion
}