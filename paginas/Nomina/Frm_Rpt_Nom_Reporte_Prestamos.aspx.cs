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
using System.Text.RegularExpressions;
using Presidencia.Prestamos.Negocio;
using Presidencia.Ayudante_Excel;
using CarlosAg.ExcelXmlWriter;
using Presidencia.Ayudante_CarlosAG;
using System.Text;
using System.Globalization;

public partial class paginas_Nomina_Frm_Rpt_Nom_Reporte_Prestamos : System.Web.UI.Page
{
    #region (Init/Load)
    /// ************************************************************************************************************************
    /// Nombre: Page_Load
    /// 
    /// Descripción: Inicializa la configuración inicial de la página.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Enero/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ************************************************************************************************************************ 
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Text = String.Empty;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }       
    }
    #endregion

    #region (Métodos)

    #region (Generales)
    /// *************************************************************************************
    /// NOMBRE: Mostrar_Excel
    /// 
    /// DESCRIPCIÓN: Muestra el reporte en excel.
    ///              
    /// PARÁMETROS: No Aplicá
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 10/Diciembre/2011.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    private void Mostrar_Excel(CarlosAg.ExcelXmlWriter.Workbook Libro, String Nombre_Archivo)
    {
        try
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Nombre_Archivo);
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Libro.Save(Response.OutputStream);
            Response.End();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte en excel. Error: [" + Ex.Message + "]");
        }
    }
    /// ***************************************************************************************************
    /// NOMBRE: Obtener_Fecha
    /// 
    /// DESCRIPCIÓN: Obtiene un objeto datetime a partir de un String.
    /// 
    /// PARÁMETROS: Fecha.- Objeto de tipo String que almacena la fecha a convertir a DateTime.
    /// 
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete. 
    /// FECHA CREÓ: 24/Mayo/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// ***************************************************************************************************
    public static DateTime Obtener_Fecha(String Fecha)
    {
        String[] Formatos = new String[]{
                    "dd/MM/yyyy"
        };

        try
        {
            return DateTime.ParseExact(Fecha, Formatos,
                CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces);
        }
        catch (FormatException Ex)
        {
            try
            {
                return DateTime.ParseExact(Fecha, Formatos,
                    new CultureInfo("es-Mx"), DateTimeStyles.AllowWhiteSpaces);
            }
            catch (FormatException Ex_Es_Mexico)
            {
                try
                {
                    return DateTime.ParseExact(Fecha, Formatos,
                        new CultureInfo("en-US"), DateTimeStyles.AllowWhiteSpaces);
                }
                catch (FormatException Ex_Ingles_EUA)
                {
                    try
                    {
                        return DateTime.ParseExact(Fecha, Formatos,
                            new CultureInfo("es-ES"), DateTimeStyles.AllowWhiteSpaces);
                    }
                    catch (FormatException Ex_Es_Espana)
                    {
                        try
                        {
                            return DateTime.ParseExact(Fecha, Formatos,
                                new CultureInfo("es"), DateTimeStyles.AllowWhiteSpaces);
                        }
                        catch (FormatException Ex_Es)
                        {
                            try
                            {
                                return DateTime.ParseExact(Fecha, Formatos,
                                    new CultureInfo("en"), DateTimeStyles.AllowWhiteSpaces);
                            }
                            catch (FormatException Ex_Ingles)
                            {
                                throw new Exception("Cultura de Fecha Incorrecto. Error: [" + Ex.Message + "]");
                            }
                        }
                    }
                }
            }
        }
    }
    #endregion

    #region (Validaciones)
    /// ************************************************************************************************************************
    /// Nombre: Validaciones
    /// 
    /// Descripción: Método que valida que los datos o formatos requeridos se hallan proporcinado de forma correcta.
    /// 
    /// Parámetros: No aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Enero/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ************************************************************************************************************************ 
    private Boolean Validaciones() {
        Boolean Datos_Validos = true;
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (string.IsNullOrEmpty(Txt_Fecha_Inicio.Text.Trim()) || Txt_Fecha_Inicio.Text.Equals("__/__/____"))
        {
            Txt_Fecha_Inicio.Text = String.Empty;
        }
        else if (!Validar_Formato_Fecha(Txt_Fecha_Inicio.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Formato de Fecha Inicial Incorrecto <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Fecha_Fin.Text.Trim()) || Txt_Fecha_Fin.Text.Equals("__/__/____"))
        {
            Txt_Fecha_Fin.Text = String.Empty;
        }
        else if (!Validar_Formato_Fecha(Txt_Fecha_Fin.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Formato de Fecha Final Incorrecto <br>";
            Datos_Validos = false;
        }

        return Datos_Validos;
    }
    /// ************************************************************************************************************************
    /// Nombre: Validar_Formato_Fecha
    /// 
    /// Descripción: Valida el formato de las fechas.
    /// 
    /// Parámetros: No aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Enero/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ************************************************************************************************************************ 
    private Boolean Validar_Formato_Fecha(String Fecha)
    {
        String Cadena_Fecha = @"^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$";
        if (Fecha != null) return Regex.IsMatch(Fecha, Cadena_Fecha);
        else return false;
    }
    #endregion

    #region (Consulta)
    /// ************************************************************************************************************************
    /// Nombre: Consultar_Prestamos
    /// 
    /// Descripción: Método que consulta los préstamos.
    /// 
    /// Parámetros: No aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Enero/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ************************************************************************************************************************ 
    private DataTable Consultar_Prestamos()
    {
        Cls_Ope_Nom_Pestamos_Negocio Obj_Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Prestamos = null;//Variable que lista los préstamos encontrados según los parámetros de consulta.

        try
        {
            if (!String.IsNullOrEmpty(Txt_No_Empleado.Text))
                Obj_Prestamos.P_Solicita_No_Empleado = Txt_No_Empleado.Text.Trim();
            
            if (!String.IsNullOrEmpty(Txt_Aval.Text))
                Obj_Prestamos.P_Aval_No_Empleado= Txt_Aval.Text.Trim();
            
            if (!String.IsNullOrEmpty(Txt_Cantidad_Prestamo.Text)) 
                Obj_Prestamos.P_Total_Prestamo = Convert.ToDouble(Txt_Cantidad_Prestamo.Text.Trim());

            if (!String.IsNullOrEmpty(Txt_Fecha_Inicio.Text)) 
                Obj_Prestamos.P_Fecha_Inicio_Pago = String.Format("{0:dd/MM/yyyy}", Obtener_Fecha(Txt_Fecha_Inicio.Text.Trim()));

            if (!String.IsNullOrEmpty(Txt_Fecha_Fin.Text)) 
                Obj_Prestamos.P_Fecha_Termino_Pago = String.Format("{0:dd/MM/yyyy}", Obtener_Fecha(Txt_Fecha_Fin.Text.Trim()));

            if (Cmb_Estatus_Prestamo.SelectedIndex > 0) 
                Obj_Prestamos.P_Estatus_Solicitud = Cmb_Estatus_Prestamo.SelectedItem.Text.Trim();

            if (Cmb_Estado_Prestamo.SelectedIndex > 0) 
                Obj_Prestamos.P_Estado_Prestamo = Cmb_Estado_Prestamo.SelectedItem.Text.Trim();

            Dt_Prestamos = Obj_Prestamos.Consulta_Reporte_Prestamos();

            if (Dt_Prestamos is DataTable)
            {
                if (Dt_Prestamos.Rows.Count == 0)
                {
                    Lbl_Mensaje_Error.Text = "No se encontraron registros de préstamos con los filtros proporcionados.";
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los prestamos. Error: [" + Ex.Message + "]");
        }
        return Dt_Prestamos;
    }
    #endregion

    #endregion

    #region (Eventos)
    /// ************************************************************************************************************************
    /// Nombre: Btn_Generar_Reporte_Excel_Click
    /// 
    /// Descripción: Evento que lanza el reporte de préstamos.
    /// 
    /// Parámetros: No aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Enero/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ************************************************************************************************************************ 
    protected void Btn_Generar_Reporte_Excel_Click(object sender, EventArgs e)
    {
        CarlosAg.ExcelXmlWriter.Workbook Libro = null;//Creamos la variable que almacenara el libro de excel.
        DataTable Dt_Prestamos = null;

        try
        {
            if (Validaciones())
            {
                Dt_Prestamos = Consultar_Prestamos();

                //Obtenemos el libro.
                Libro = Cls_Ayudante_Crear_Excel.Generar_Excel(Dt_Prestamos);
                //Mandamos a imprimir el reporte en excel.
                Mostrar_Excel(Libro, "Prestamos.xls");
            }
            else {
                Lbl_Mensaje_Error.Text = "No se encontraron registros de préstamos con los filtros proporcionados.";
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    /// ************************************************************************************************************************
    /// Nombre: Btn_Generar_Reporte_Excel_Click
    /// 
    /// Descripción: Evento que lanza el reporte de préstamos.
    /// 
    /// Parámetros: No aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Enero/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ************************************************************************************************************************ 
    protected void Btn_Generar_Reporte_PDF_Click(object sender, EventArgs e)
    {
        try
        {
            if (Validaciones())
            {

            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    #endregion
}
