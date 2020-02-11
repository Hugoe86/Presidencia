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
using Presidencia.Contratos_Vencidos.Negocio;

public partial class paginas_Nomina_Frm_Rpt_Nom_Contratos_Vencidos : System.Web.UI.Page
{
    #region PAGE LOAD

    ///*****************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Page_Load
    ///DESCRIPCIÓN          : Inicio de la pagina
    ///PARAMETROS           : 
    ///CREO                 : Leslie González Vázquez
    ///FECHA_CREO           : 12/Enero/2012
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
                Reporte_Contratos_Vencidos_Inicio();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error en el inicio del reporte de contratos vencidos. Error [" + Ex.Message + "]");
        }
    }
    #endregion

    #region METODOS

    ///*****************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Reporte_Contratos_Vencidos_Inicio
    ///DESCRIPCIÓN          : Metodo de inicio de la página
    ///PARAMETROS           : 
    ///CREO                 : Leslie González Vázquez
    ///FECHA_CREO           : 12/Enero/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*****************************************************************************************************************
    private void Reporte_Contratos_Vencidos_Inicio()
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            Limpiar_Controles("Todo");
            Llenar_Combo_Tipos();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error en el inicio del reporte de contratos vencidos. Error [" + Ex.Message + "]");
        }
    }

    ///*****************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Limpiar_Controles
    ///DESCRIPCIÓN          : Metodo para limpiar los controles del formulario
    ///PARAMETROS           1 Accion: para indicar que parte del codigo limpiara 
    ///CREO                 : Leslie González Vázquez
    ///FECHA_CREO           : 28/Diciembre/2011
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
                    Txt_Fecha_Inicio.Text = "";
                    Txt_Fecha_Fin.Text = "";
                    Lbl_Ecabezado_Mensaje.Text = "";
                    Lbl_Mensaje_Error.Text = "";
                    Cmb_Tipo.SelectedIndex = -1;
                    Txt_No_Empleado.Text = "";
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
    ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Tipos
    ///DESCRIPCIÓN          : Metodo para llenar el combo del tipo de contrato
    ///PARAMETROS           : 
    ///CREO                 : Leslie González Vázquez
    ///FECHA_CREO           : 13/Enero/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*****************************************************************************************************************
    private void Llenar_Combo_Tipos()
    {
        //Cls_Rpt_Com_Mensual_Orden_Compra_Negocio Rpt_Ordenes_Negocio = new Cls_Rpt_Com_Mensual_Orden_Compra_Negocio(); //Conexion con la capa de negocios
        DataTable Dt_Estatus = new DataTable(); //Para almacenar los datos de los estatus de las ordenes de compras

        try
        {
            Cmb_Tipo.Items.Clear();
            Cmb_Tipo.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
            Cmb_Tipo.Items.Insert(1, new ListItem("Por Vencer", "POR VENCER"));
            Cmb_Tipo.Items.Insert(2, new ListItem("Vencidos", "VENCIDO"));
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al llenar el combo de tipo: Error[" + Ex.Message + "]");
        }
    }

    ///*****************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Generar_Rpt_Contratos_Vencidos
    ///DESCRIPCIÓN          : Metodo generar el reporte de contratos vencidos
    ///PARAMETROS           1 Dt_Contratos: contratos vencidos que se pasaran al reporte 
    ///CREO                 : Leslie González Vázquez
    ///FECHA_CREO           : 13/Enero/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*****************************************************************************************************************
    public void Generar_Rpt_Contratos_Vencidos(DataTable Dt_Contratos)
    {
        String Ruta_Archivo = "Reporte_Contratos_Vencidos_Y_Por_Vencer.xls";
        try
        {
            CarlosAg.ExcelXmlWriter.Workbook Libro = new CarlosAg.ExcelXmlWriter.Workbook();
            Libro.Properties.Title = "Reporte de Contratos Vencidos y Por Vencer";
            Libro.Properties.Created = DateTime.Now;
            Libro.Properties.Author = "Presidencia";

            //Creamos una hoja que tendrá el libro.
            CarlosAg.ExcelXmlWriter.Worksheet Hoja = Libro.Worksheets.Add("Reporte de Contratos Vencidos y Por Vencer");
            //Agregamos un renglón a la hoja de excel.
            CarlosAg.ExcelXmlWriter.WorksheetRow Renglon = Hoja.Table.Rows.Add();
            //Creamos el estilo cabecera para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera = Libro.Styles.Add("HeaderStyle");
            //Creamos el estilo contenido para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Contenido = Libro.Styles.Add("BodyStyle");
            //Creamos el estilo contenido para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Contratos_Vencidos = Libro.Styles.Add("BodyStyle_Cont_Vencidos");

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

            Estilo_Contratos_Vencidos.Font.FontName = "Tahoma";
            Estilo_Contratos_Vencidos.Font.Size = 9;
            Estilo_Contratos_Vencidos.Font.Bold = true;
            Estilo_Contratos_Vencidos.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            Estilo_Contratos_Vencidos.Font.Color = "#FF0000";
            Estilo_Contratos_Vencidos.Interior.Color = "White";
            Estilo_Contratos_Vencidos.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Contratos_Vencidos.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contratos_Vencidos.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contratos_Vencidos.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contratos_Vencidos.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //Agregamos las columnas que tendrá la hoja de excel.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));//No empleado.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(240));//Nombre.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(140));//Fecha.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(300));//Unidad Responsable.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(120));//Tipo_Nomina.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));//Tipo_Contrato.

            if (Dt_Contratos is DataTable)
            {
                if (Dt_Contratos.Rows.Count > 0)
                {
                    foreach (DataColumn Columna in Dt_Contratos.Columns)
                    {
                        if (Columna is DataColumn)
                        {
                            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Columna.ColumnName.Replace("_" ," "), "HeaderStyle"));
                        }
                    }
                    foreach (DataRow Dr in Dt_Contratos.Rows)
                    {
                        Renglon = Hoja.Table.Rows.Add();

                        if (Dr["TIPO CONTRATO"].ToString().Trim().Equals("VENCIDO"))
                        {
                            foreach (DataColumn Columna in Dt_Contratos.Columns)
                            {
                                if (Columna is DataColumn)
                                {
                                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Dr[Columna.ColumnName].ToString(), DataType.String, "BodyStyle_Cont_Vencidos"));
                                }
                            }
                        }
                        else 
                        {
                            foreach (DataColumn Columna in Dt_Contratos.Columns)
                            {
                                if (Columna is DataColumn)
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
    ///FECHA_CREO           : 13/Enero/2012
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
            if (Cmb_Tipo.SelectedIndex > 0) 
            {
                if (Cmb_Tipo.SelectedItem.Value.ToString().Trim().Equals("VENCIDO"))
                {
                    Txt_Fecha_Fin.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);
                }
                else 
                {
                    Txt_Fecha_Inicio.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);
                }
            }

            if (String.IsNullOrEmpty(Txt_Fecha_Inicio.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Introducir una fecha de inicio <br />";
                Datos_Validos = false;
            }
            
            if (String.IsNullOrEmpty(Txt_Fecha_Fin.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Introducir una fecha de termino <br />";
                Datos_Validos = false;
            }
            else
            {
                if (!String.IsNullOrEmpty(Txt_Fecha_Inicio.Text.Trim()))
                {
                    if (Convert.ToDateTime(Txt_Fecha_Inicio.Text.Trim()).CompareTo(Convert.ToDateTime(Txt_Fecha_Fin.Text.Trim())) > 0)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Fecha del no puede ser mayor a la Fecha termino  <br />";
                        Datos_Validos = false;
                    }
                }
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
    ///FECHA_CREO           : 28/Diciembre/2011
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*****************************************************************************************************************
    protected void Btn_Generar_Click(object sender, EventArgs e)
    {
        Cls_Rpt_Nom_Contratos_Vencidos_Negocio Contratos_Negocio = new Cls_Rpt_Nom_Contratos_Vencidos_Negocio(); //Conexion con la capa de negocios
        DataTable Dt_Contratos = new DataTable(); //Para almacenar los datos de las ordenes de compras
        Div_Contenedor_Msj_Error.Visible = false;
        Limpiar_Controles("Error");
        String Complemento = String.Empty;
        DateTime Fecha_Actual = DateTime.Now;
       
        try
        {
            if (Validar_Campos())
            {
                if (!String.IsNullOrEmpty(Txt_Fecha_Inicio.Text.Trim()))
                {
                    Contratos_Negocio.P_Fecha_Inicio = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Inicio.Text.Trim()));
                }

                if (!String.IsNullOrEmpty(Txt_Fecha_Fin.Text.Trim()))
                {
                    Contratos_Negocio.P_Fecha_Fin = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Fin.Text.Trim()));
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
                    Contratos_Negocio.P_No_Empleado = Complemento + Txt_No_Empleado.Text.Trim();
                }

                if (!String.IsNullOrEmpty(Txt_Nombre_Empleado.Text.Trim()))
                {
                    Contratos_Negocio.P_Nombre_Empleado = Txt_Nombre_Empleado.Text.Trim();
                }

                Dt_Contratos = Contratos_Negocio.Consultar_Contratos_Vencidos();

                if (Dt_Contratos != null)
                {
                    if (Dt_Contratos.Rows.Count > 0)
                    {
                        Dt_Contratos.Columns.Add("TIPO CONTRATO");

                        if (Cmb_Tipo.SelectedIndex > 0)
                        {
                            foreach (DataRow Dr in Dt_Contratos.Rows)
                            {
                                Dr["TIPO CONTRATO"] = Cmb_Tipo.SelectedItem.Value.ToString().Trim();
                                Dr["FECHA_TERMINO_CONTRATO"] = String.Format("{0:dd/MMM/yyyy}", Dr["FECHA"]);
                            }
                        }
                        else
                        {
                            foreach (DataRow Dr in Dt_Contratos.Rows)
                            {
                                if (Fecha_Actual.CompareTo(Dr["FECHA"]) > 0)
                                {
                                    Dr["TIPO CONTRATO"] = "VENCIDO";
                                }
                                else
                                {
                                    Dr["TIPO CONTRATO"] = "POR VENCER";
                                }
                                Dr["FECHA_TERMINO_CONTRATO"] = String.Format("{0:dd/MMM/yyyy}", Dr["FECHA"]);
                            }
                        }

                        Dt_Contratos.Columns.Remove("FECHA");
                        Generar_Rpt_Contratos_Vencidos(Dt_Contratos);
                        if (!Txt_Fecha_Fin.Enabled)
                            Txt_Fecha_Fin.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);
                        if (!Txt_Fecha_Inicio.Enabled)
                            Txt_Fecha_Inicio.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('No hay datos registrados con el rango de fechas.');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('No hay datos registrados con el rango de fechas.');", true);
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

    #endregion
}
