using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using CarlosAg.ExcelXmlWriter;
using System.Text;
using Presidencia.Pago_Terceros.Negocio;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Tipos_Nominas.Negocios;

public partial class paginas_Nomina_Frm_Rpt_Nom_Pago_Terceros : System.Web.UI.Page
{
    #region PAGE LOAD

    ///*****************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Page_Load
    ///DESCRIPCIÓN          : Inicio de la pagina
    ///PARAMETROS           : 
    ///CREO                 : Leslie González Vázquez
    ///FECHA_CREO           : 17/Enero/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*****************************************************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                Reporte_Pago_Terceros_Inicio();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error en el inicio del reporte de pago a terceros. Error [" + Ex.Message + "]");
        }
    }
    #endregion

    #region METODOS

    ///*****************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Reporte_Pago_Terceros_Inicio
    ///DESCRIPCIÓN          : Metodo de inicio de la página
    ///PARAMETROS           : 
    ///CREO                 : Leslie González Vázquez
    ///FECHA_CREO           : 17/Enero/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*****************************************************************************************************************
    private void Reporte_Pago_Terceros_Inicio()
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            Limpiar_Controles("Todo");
            Llenar_Combo_Nomina();
            Cmb_Periodo.Enabled = false;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error en el inicio del reporte de pago a terceros. Error [" + Ex.Message + "]");
        }
    }

    ///*****************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Limpiar_Controles
    ///DESCRIPCIÓN          : Metodo para limpiar los controles del formulario
    ///PARAMETROS           1 Accion: para indicar que parte del codigo limpiara 
    ///CREO                 : Leslie González Vázquez
    ///FECHA_CREO           : 17/Enero/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*****************************************************************************************************************
    private void Limpiar_Controles(String Accion)
    {
        try
        {
            switch (Accion)
            {
                case "Todo":
                    Lbl_Ecabezado_Mensaje.Text = "";
                    Lbl_Mensaje_Error.Text = "";
                    Txt_No_Empleado.Text = "";
                    Txt_Nombre_Empleado.Text = "";
                    Cmb_Nomina.SelectedIndex = -1;
                    Cmb_Periodo.Items.Clear();
                    break;
                case "Error":
                    Lbl_Ecabezado_Mensaje.Text = "";
                    Lbl_Mensaje_Error.Text = "";
                    break;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al limpiar los controles Error [" + Ex.Message + "]");
        }
    }

    ///*****************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Nomina
    ///DESCRIPCIÓN          : Metodo para llenar el combo de nomina
    ///PARAMETROS           : 
    ///CREO                 : Leslie González Vázquez
    ///FECHA_CREO           : 16/Enero/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*****************************************************************************************************************
    private void Llenar_Combo_Nomina()
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nomina_Negocio = new Cls_Cat_Nom_Calendario_Nominas_Negocio();
        DataTable Dt_Nominas = new DataTable(); //Para almacenar los datos de los tipos de nominas

        try
        {
            Cmb_Nomina.Items.Clear();

            Dt_Nominas = Obj_Calendario_Nomina_Negocio.Consultar_Calendario_Nominas();

            Cmb_Nomina.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
            Cmb_Nomina.DataTextField = Cat_Nom_Calendario_Nominas.Campo_Anio;
            Cmb_Nomina.DataSource = Dt_Nominas;
            Cmb_Nomina.DataBind();

            Cmb_Nomina.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al llenar el combo de nomina. Error:[" + Ex.Message + "]");
        }
    }

    ///*****************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Periodo
    ///DESCRIPCIÓN          : Metodo para llenar el combo de periodo de nomina
    ///PARAMETROS           : 
    ///CREO                 : Leslie González Vázquez
    ///FECHA_CREO           : 16/Enero/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*****************************************************************************************************************
    private void Llenar_Combo_Periodo()
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nomina_Periodos = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Clase de conexion con la capa de negocios
        DataTable Dt_Periodos_Catorcenales = null;//Variable que almacenra unaa lista de los periodos catorcenales que le correspónden a la nomina seleccionada.

        try
        {
            Cmb_Periodo.Items.Clear();

            Consulta_Calendario_Nomina_Periodos.P_Nomina_ID = Cmb_Nomina.SelectedItem.Value.Trim();
            Dt_Periodos_Catorcenales = Consulta_Calendario_Nomina_Periodos.Consulta_Detalles_Nomina();

            Cmb_Periodo.DataValueField = Cat_Nom_Nominas_Detalles.Campo_Detalle_Nomina_ID;
            Cmb_Periodo.DataTextField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
            Cmb_Periodo.DataSource = Dt_Periodos_Catorcenales;
            Cmb_Periodo.DataBind();

            Cmb_Periodo.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al llenar el combo de periodo de nomina. Error:[" + Ex.Message + "]");
        }
    }


    ///*****************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Generar_Rpt_Pago_Terceros
    ///DESCRIPCIÓN          : Metodo generar el reporte de pago a terceros
    ///PARAMETROS           1 Dt_Datos: datos del reporte que se pasaran en excel 
    ///CREO                 : Leslie González Vázquez
    ///FECHA_CREO           : 17/Enero/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*****************************************************************************************************************
    public void Generar_Rpt_Pago_Terceros(DataTable Dt_Datos)
    {
        String Ruta_Archivo = "Reporte_Pago_Terceros.xls";
        try
        {
            CarlosAg.ExcelXmlWriter.Workbook Libro = new CarlosAg.ExcelXmlWriter.Workbook();
            Libro.Properties.Title = "Reporte de Pago a Terceros";
            Libro.Properties.Created = DateTime.Now;
            Libro.Properties.Author = "Presidencia";

            //Creamos una hoja que tendrá el libro.
            CarlosAg.ExcelXmlWriter.Worksheet Hoja = Libro.Worksheets.Add("Reporte de Pago a Terceros");
            //Agregamos un renglón a la hoja de excel.
            CarlosAg.ExcelXmlWriter.WorksheetRow Renglon = Hoja.Table.Rows.Add();
            //Creamos el estilo cabecera para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera = Libro.Styles.Add("HeaderStyle");
            //Creamos el estilo contenido para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Contenido = Libro.Styles.Add("BodyStyle");
            //Creamos el estilo contenido para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cantidad = Libro.Styles.Add("BodyStyleCant");


            Estilo_Cabecera.Font.FontName = "Tahoma";
            Estilo_Cabecera.Font.Size = 10;
            Estilo_Cabecera.Font.Bold = true;
            Estilo_Cabecera.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Cabecera.Font.Color = "#FFFFFF";
            Estilo_Cabecera.Interior.Color = "#193d61";
            Estilo_Cabecera.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Cabecera.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            Estilo_Contenido.Font.FontName = "Tahoma";
            Estilo_Contenido.Font.Size = 9;
            Estilo_Contenido.Font.Bold = true;
            Estilo_Contenido.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            Estilo_Contenido.Font.Color = "#000000";
            Estilo_Contenido.Interior.Color = "White";
            Estilo_Contenido.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Contenido.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            Estilo_Cantidad.Font.FontName = "Tahoma";
            Estilo_Cantidad.Font.Size = 9;
            Estilo_Cantidad.Font.Bold = true;
            Estilo_Cantidad.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            Estilo_Cantidad.Font.Color = "#000000";
            Estilo_Cantidad.Interior.Color = "White";
            Estilo_Cantidad.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Cantidad.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cantidad.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cantidad.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cantidad.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //Agregamos las columnas que tendrá la hoja de excel.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));//No empleado.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(240));//Nombre.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(300));//Unidad Responsable.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(200));//tipo nomina.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(50));//año.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));//periodo.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(200));//nombre terceros.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(120));//porcentaje.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(280));//Deduccion.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(120));//total.


            if (Dt_Datos is DataTable)
            {
                if (Dt_Datos.Rows.Count > 0)
                {
                    foreach (DataColumn Columna in Dt_Datos.Columns)
                    {
                        if (Columna is DataColumn)
                        {
                            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Columna.ColumnName.Replace("_", " "), "HeaderStyle"));
                        }
                    }
                    foreach (DataRow Dr in Dt_Datos.Rows)
                    {
                        Renglon = Hoja.Table.Rows.Add();

                        foreach (DataColumn Columna in Dt_Datos.Columns)
                        {
                            if (Columna is DataColumn)
                            {
                                if (Columna.ColumnName.Equals("TOTAL PAGO TERCEROS"))
                                {
                                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Dr[Columna.ColumnName].ToString(), DataType.String, "BodyStyleCant"));
                                }
                                else
                                {
                                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Dr[Columna.ColumnName].ToString(), DataType.String, "BodyStyle"));
                                }
                            }
                        }
                    }
                }
            }

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Ruta_Archivo);
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Libro.Save(Response.OutputStream);
            Response.End();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al generar el reporte en excel Error[" + ex.Message + "]");
        }
    }

    ///*****************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Campos
    ///DESCRIPCIÓN          : Metodo para validar los campos del formulario
    ///PARAMETROS           : 
    ///CREO                 : Leslie González Vázquez
    ///FECHA_CREO           : 16/Enero/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*****************************************************************************************************************
    private Boolean Validar_Campos()
    {
        Boolean Datos_Validos = true;
        Limpiar_Controles("Error");
        Lbl_Ecabezado_Mensaje.Text = "Favor de:";
        try
        {
            if (Cmb_Nomina.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccionar una nomina <br />";
                Datos_Validos = false;
            }

            return Datos_Validos;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al valodar los campos del formulario Error [" + Ex.Message + "]");
        }
    }
    #endregion

    #region EVENTOS

    ///*****************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
    ///DESCRIPCIÓN          : Ejecuta la operacion de click en el boton de salir
    ///PARAMETROS           : 
    ///CREO                 : Leslie González Vázquez
    ///FECHA_CREO           : 28/Diciembre/2011
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*****************************************************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.ToolTip.Trim().Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
    }

    ///*****************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Generar_Click
    ///DESCRIPCIÓN          : Ejecuta la operacion de click en el boton de generar
    ///PARAMETROS           : 
    ///CREO                 : Leslie González Vázquez
    ///FECHA_CREO           : 17/Enero/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*****************************************************************************************************************
    protected void Btn_Generar_Click(object sender, EventArgs e)
    {
        Cls_Rpt_Nom_Pago_Terceros_Negocio Pagos_Negocio = new Cls_Rpt_Nom_Pago_Terceros_Negocio(); //Conexion con la capa de negocios
        DataTable Dt_Datos = new DataTable(); //Para almacenar los datos de las ordenes de compras
        Div_Contenedor_Msj_Error.Visible = false;
        Limpiar_Controles("Error");
        String Complemento = String.Empty;

        try
        {
            if (Validar_Campos())
            {
                 if (Cmb_Nomina.SelectedIndex > 0)
                {
                    Pagos_Negocio.P_Nomina = Cmb_Nomina.SelectedItem.Value.Trim();
                }

                if (Cmb_Periodo.SelectedIndex > 0)
                {
                    Pagos_Negocio.P_Periodo = Cmb_Periodo.SelectedItem.Text.Trim();
                }

                if (!String.IsNullOrEmpty(Txt_No_Empleado.Text.Trim()))
                {
                    if (Txt_No_Empleado.Text.Trim().Length < 6)
                    {
                        for (Int32 i = 1; i <= 6 - Txt_No_Empleado.Text.Trim().Length; i++)
                        {
                            Complemento += "0";
                        }
                    }
                    Pagos_Negocio.P_No_Empleado = Complemento + Txt_No_Empleado.Text.Trim();
                }

                if (!String.IsNullOrEmpty(Txt_Nombre_Empleado.Text.Trim()))
                {
                    Pagos_Negocio.P_Nombre_Empleado = Txt_Nombre_Empleado.Text.Trim();
                }

                Dt_Datos = Pagos_Negocio.Consultar_Pago_Terceros();

                if (Dt_Datos != null)
                {
                    if (Dt_Datos.Rows.Count > 0)
                    {
                        Dt_Datos.Columns.Add("TOTAL PAGO TERCEROS");
                        foreach (DataRow Dr in Dt_Datos.Rows)
                        {
                            Dr["TOTAL PAGO TERCEROS"] = String.Format("{0:c}", Dr["MONTO"]);
                        }
                        Dt_Datos.Columns.Remove("MONTO");

                        Generar_Rpt_Pago_Terceros(Dt_Datos);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('No hay datos registrados.');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('No hay datos registrados.');", true);
                }
            }
            else
            {
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error en el evento del boton de generar el reporte. Error[" + ex.Message + "]");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cmb_Nomina_SelectedIndexChanged
    ///DESCRIPCIÓN          : Evento del combo de nomina
    ///PROPIEDADES          :
    ///CREO                 : Leslie González Vázquez
    ///FECHA_CREO           : 16/Enero/2012 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///******************************************************************************* 
    protected void Cmb_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Limpiar_Controles("Error");
        try
        {
            if (Cmb_Nomina.SelectedIndex > 0)
            {
                Llenar_Combo_Periodo();
                Cmb_Periodo.Enabled = true;
            }
            else
            {
                Cmb_Periodo.Items.Clear();
                Cmb_Periodo.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error en el evento del combo de capitulos Error[" + ex.Message + "]");
        }
    }

    #endregion
}
