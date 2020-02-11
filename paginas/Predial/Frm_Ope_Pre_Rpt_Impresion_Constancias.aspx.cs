using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Ope_Pre_Rpt_Impresion_Constancias.Negocio;
using Presidencia.Sessiones;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class paginas_Predial_Frm_Ope_Pre_Rpt_Impresion_Constancias : System.Web.UI.Page
{
    #region Page_Load

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Evento al Cargar la Pagina.
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 14 Enero 2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Txt_Fecha_Final.Text=DateTime.Now.ToString("dd/MMM/yyyy");
                Txt_Fecha_Inicial.Text=DateTime.Now.ToString("dd/MMM/yyyy");
                Rdb_Tipo_Reporte.SelectedIndex = 0;
            }
            //Limpiamos algún mensaje de error que se halla quedado en el log, que muestra los errores del sistema.
            Lbl_Mensaje_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    #endregion

    #region Eventos

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Realiza el evento de generar el reporte seleccionado.
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 14 Enero 2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        DataSet Ds_Reporte = new DataSet();
        Cls_Ope_Pre_Rpt_Impresion_Constancias_Negocio Cnp = new Cls_Ope_Pre_Rpt_Impresion_Constancias_Negocio();
        Cnp.P_Fecha_Inicial = Convert.ToDateTime(Txt_Fecha_Inicial.Text).ToString("dd/MM/yyyy");
        Cnp.P_Fecha_Final = Convert.ToDateTime(Txt_Fecha_Final.Text).ToString("dd/MM/yyyy");
        Cnp.P_Estatus = Cmb_Estatus.SelectedValue;
        Cnp.P_Tipo = Rdb_Tipo_Reporte.SelectedValue;
        DataTable Dt_Cnp;
        DataTable Dt_Totales;
        if (Cnp.P_Tipo == "CNP")
        {
            Dt_Cnp = Cnp.Consultar_Clave_Gasto_Ejecucion();
            Dt_Totales = Modificar_Costos_No_Propiedad(Dt_Cnp);
            Dt_Cnp.TableName = "Dt_Constancia_No_Propiedad";
            Dt_Totales.TableName = "Dt_Totales";
            Ds_Reporte.Tables.Add(Dt_Cnp.Copy());
            Ds_Reporte.Tables.Add(Dt_Totales.Copy());
            Generar_Reporte(ref Ds_Reporte, "Rpt_Ope_Pre_Impresion_Constancias_Concentrado.rpt", "Reporte_Constancia_No_Propiedad_" + Session.SessionID + ".pdf", ".pdf");
        }
        else
        {
            Dt_Cnp = Cnp.Consultar_Constancias_Y_Certificaciones();
            Dt_Totales = Modificar_Costos_Tabla(Dt_Cnp);
            Dt_Cnp.TableName = "Dt_Constancia_Certificacion";
            Dt_Totales.TableName = "Dt_Totales";
            Ds_Reporte.Tables.Add(Dt_Cnp.Copy());
            Ds_Reporte.Tables.Add(Dt_Totales.Copy());
            Generar_Reporte(ref Ds_Reporte, "Rpt_Ope_Pre_Impresion_Constancias_Certificaciones.rpt", "Reporte_Constancia_Certificacion_" + Session.SessionID + ".pdf", ".pdf");
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Regresa a la página principal.
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 14 Enero 2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
    }

    #endregion

    #region Metodos

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Modificar_Costos_Tabla
    ///DESCRIPCIÓN: Crea una tabla con los datos del pie del reporte.
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 14 Enero 2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Modificar_Costos_Tabla(DataTable Dt_Consulta)
    {
        DataTable Dt_Totales= new DataTable();
        Dt_Totales.Columns.Add("TOTAL_PAGADAS", typeof(Double));
        Dt_Totales.Columns.Add("TOTAL_CANCELADAS", typeof(Double));
        Dt_Totales.Columns.Add("TOTAL_POR_PAGAR", typeof(Double));
        Dt_Totales.Columns.Add("CANTIDAD_PAGADAS", typeof(Double));
        Dt_Totales.Columns.Add("CANTIDAD_CANCELADAS", typeof(Double));
        Dt_Totales.Columns.Add("CANTIDAD_POR_PAGAR", typeof(Double));
        Dt_Totales.Columns.Add("TOTAL_ACUMULADO", typeof(Double));
        Dt_Totales.Columns.Add("TIPO", typeof(String));
        DataRow Dr_Totales;
        Int32 Cantidad_Pagadas = 0;
        Int32 Cantidad_Canceladas = 0;
        Int32 Cantidad_Por_Pagar = 0;
        Double Total_Pagadas = 0;
        Double Total_Cancelada = 0;
        Double Total_Por_Pagar = 0;
        Double Total_Acumulado = 0;

        foreach (DataRow Dr_Renglon in Dt_Consulta.Rows)
        {
            if (Dr_Renglon["ESTATUS"].ToString() == "IMPRESA" || Dr_Renglon["ESTATUS"].ToString() == "PAGADA")
            {
                if (Dr_Renglon["PASIVO"].ToString() == "")
                {
                    Total_Pagadas += Convert.ToDouble(Dr_Renglon["COSTO"].ToString());
                }
                else
                {
                    Dr_Renglon["COSTO"]=Dr_Renglon["PASIVO"];
                    Total_Pagadas += Convert.ToDouble(Dr_Renglon["PASIVO"].ToString());
                }
                Cantidad_Pagadas++;
            }
            else if (Dr_Renglon["ESTATUS"].ToString() == "CANCELADA")
            {
                if (Dr_Renglon["PASIVO"].ToString() == "")
                {
                    Total_Pagadas += Convert.ToDouble(Dr_Renglon["COSTO"].ToString());
                }
                else
                {
                    Dr_Renglon["COSTO"] = Dr_Renglon["PASIVO"];
                    Total_Cancelada += Convert.ToDouble(Dr_Renglon["PASIVO"].ToString());
                }
                Cantidad_Canceladas++;
            }
            else
            {
                if (Dr_Renglon["PASIVO"].ToString() == "")
                {
                    Total_Por_Pagar += Convert.ToDouble(Dr_Renglon["COSTO"].ToString());
                }
                else
                {
                    Dr_Renglon["COSTO"] = Dr_Renglon["PASIVO"];
                    Total_Por_Pagar += Convert.ToDouble(Dr_Renglon["PASIVO"].ToString());
                }
                Cantidad_Por_Pagar++;
            }
            if (Dr_Renglon["NOMBRE_PROPIETARIO"].ToString() == "")
            {
                Dr_Renglon["NOMBRE_PROPIETARIO"] = Dr_Renglon["PROPIETARIO"].ToString();
            }
            Total_Acumulado += Convert.ToDouble(Dr_Renglon["COSTO"].ToString());
        }
        Dr_Totales = Dt_Totales.NewRow();
        Dr_Totales["TOTAL_PAGADAS"] = Total_Pagadas;
        Dr_Totales["TOTAL_CANCELADAS"] = Total_Cancelada;
        Dr_Totales["TOTAL_POR_PAGAR"] = Total_Por_Pagar;
        Dr_Totales["CANTIDAD_PAGADAS"] = Cantidad_Pagadas;
        Dr_Totales["CANTIDAD_CANCELADAS"] = Cantidad_Canceladas;
        Dr_Totales["CANTIDAD_POR_PAGAR"] = Cantidad_Por_Pagar;
        Dr_Totales["TOTAL_ACUMULADO"] = Total_Acumulado;
        Dr_Totales["TIPO"] = Rdb_Tipo_Reporte.SelectedItem.Text.ToUpper();
        Dt_Totales.Rows.Add(Dr_Totales);
        return Dt_Totales;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Modificar_Costos_No_Propiedad
    ///DESCRIPCIÓN: Crea una tabla con los datos del pie del reporte.
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 14 Enero 2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Modificar_Costos_No_Propiedad(DataTable Dt_Consulta)
    {
        DataTable Dt_Totales = new DataTable();
        Dt_Totales.Columns.Add("TOTAL_PAGADAS", typeof(Double));
        Dt_Totales.Columns.Add("TOTAL_CANCELADAS", typeof(Double));
        Dt_Totales.Columns.Add("TOTAL_POR_PAGAR", typeof(Double));
        Dt_Totales.Columns.Add("CANTIDAD_PAGADAS", typeof(Double));
        Dt_Totales.Columns.Add("CANTIDAD_CANCELADAS", typeof(Double));
        Dt_Totales.Columns.Add("CANTIDAD_POR_PAGAR", typeof(Double));
        Dt_Totales.Columns.Add("TOTAL_ACUMULADO", typeof(Double));
        Dt_Totales.Columns.Add("TIPO", typeof(String));
        DataRow Dr_Totales;
        Int32 Cantidad_Pagadas = 0;
        Int32 Cantidad_Canceladas = 0;
        Int32 Cantidad_Por_Pagar = 0;
        Double Total_Pagadas = 0;
        Double Total_Cancelada = 0;
        Double Total_Por_Pagar = 0;
        Double Total_Acumulado = 0;

        foreach (DataRow Dr_Renglon in Dt_Consulta.Rows)
        {
            if (Dr_Renglon["ESTATUS"].ToString() == "IMPRESA" || Dr_Renglon["ESTATUS"].ToString() == "PAGADA")
            {
                if (Dr_Renglon["PASIVO"].ToString() == "")
                {
                    Total_Pagadas += Convert.ToDouble(Dr_Renglon["COSTO"].ToString());
                }
                else
                {
                    Dr_Renglon["COSTO"] = Dr_Renglon["PASIVO"];
                    Total_Pagadas += Convert.ToDouble(Dr_Renglon["PASIVO"].ToString());
                }
                Cantidad_Pagadas++;
            }
            else if (Dr_Renglon["ESTATUS"].ToString() == "CANCELADA")
            {
                if (Dr_Renglon["PASIVO"].ToString() == "")
                {
                    Total_Pagadas += Convert.ToDouble(Dr_Renglon["COSTO"].ToString());
                }
                else
                {
                    Dr_Renglon["COSTO"] = Dr_Renglon["PASIVO"];
                    Total_Cancelada += Convert.ToDouble(Dr_Renglon["PASIVO"].ToString());
                }
                Cantidad_Canceladas++;
            }
            else
            {
                if (Dr_Renglon["PASIVO"].ToString() == "")
                {
                    Total_Por_Pagar += Convert.ToDouble(Dr_Renglon["COSTO"].ToString());
                }
                else
                {
                    Dr_Renglon["COSTO"] = Dr_Renglon["PASIVO"];
                    Total_Por_Pagar += Convert.ToDouble(Dr_Renglon["PASIVO"].ToString());
                }
                Cantidad_Por_Pagar++;
            }
            Total_Acumulado += Convert.ToDouble(Dr_Renglon["COSTO"].ToString());
        }
        Dr_Totales = Dt_Totales.NewRow();
        Dr_Totales["TOTAL_PAGADAS"] = Total_Pagadas;
        Dr_Totales["TOTAL_CANCELADAS"] = Total_Cancelada;
        Dr_Totales["TOTAL_POR_PAGAR"] = Total_Por_Pagar;
        Dr_Totales["CANTIDAD_PAGADAS"] = Cantidad_Pagadas;
        Dr_Totales["CANTIDAD_CANCELADAS"] = Cantidad_Canceladas;
        Dr_Totales["CANTIDAD_POR_PAGAR"] = Cantidad_Por_Pagar;
        Dr_Totales["TOTAL_ACUMULADO"] = Total_Acumulado;
        Dr_Totales["TIPO"] = Rdb_Tipo_Reporte.SelectedItem.Text.ToUpper();
        Dt_Totales.Rows.Add(Dr_Totales);
        return Dt_Totales;
    }

    #endregion

    #region Impresion reporte

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
        ParameterFieldDefinitions crParameterFieldDefinitions;
        ParameterValues crParameterValues;
        ParameterDiscreteValue crParameterDiscreteValue;
        String Rango = "DEL " + String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Txt_Fecha_Inicial.Text)) + " AL " + String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Txt_Fecha_Final.Text));

        try
        {
            Ruta = @Server.MapPath("../Rpt/Predial/" + Ruta_Reporte_Crystal);
            Reporte.Load(Ruta);

            if (Ds_Reporte_Crystal is DataSet)
            {
                if (Ds_Reporte_Crystal.Tables.Count > 0)
                {
                    Reporte.SetDataSource(Ds_Reporte_Crystal);
                    //Declara y asigna valores a los parametros
                    crParameterFieldDefinitions = Reporte.DataDefinition.ParameterFields;
                    foreach (ParameterFieldDefinition crParameterFieldDefinition in crParameterFieldDefinitions)
                    {
                        //Crea un nuevo valor de parametro
                        crParameterDiscreteValue = new ParameterDiscreteValue();
                        //Asigna el parametro
                        crParameterValues = crParameterFieldDefinition.CurrentValues;

                        //Aplica los valores cargados
                        switch (crParameterFieldDefinition.Name)
                        {
                            case "Rango_Fechas":
                                crParameterDiscreteValue.Value = Rango;
                                crParameterValues.Add(crParameterDiscreteValue);
                                break;
                        }
                        crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                    }
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt_Cajas_Generales",
                "window.open('" + Pagina + "', 'Cajas','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    #endregion
}
