using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using Presidencia.Reportes_Contrarecibos.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Reportes;
using System.Reflection;
using CarlosAg.ExcelXmlWriter;
using Presidencia.Catalogo_Compras_Partidas.Negocio;
using Presidencia.Compras_Reporte_Proveedores_Excel.Negocio;
using System.Text;

public partial class paginas_Compras_Frm_Rpt_Com_Proveedores_Excel : System.Web.UI.Page
{

#region Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Refresca la sesion del usuario logeado en el sistema
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            //Valida que existe un usuario logueado en el sistema
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Inicializa_Controles();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Inicializa_Controles " + Ex.Message.ToString());
        }
    }
#endregion

#region Metodos
    #region (Metodos Generales)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda
    ///               realizar diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 18/Enero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Limpiar_Controles(); //Limpia los controles del formulario
        }
        catch (Exception ex)
        {
            throw new Exception("Inicializa_Controles " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los controles que se encuentran en la forma
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 18/Enero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Chk_Partida.Checked = false;
            Chk_Compras_Otorgadas.Checked = false;
            Habilitar_Checked("Todo", false);
            Cargar_Combo();
        }
        catch (Exception ex)
        {
            throw new Exception("Limpia_Controles " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Checked
    /// DESCRIPCION : Habilitara las etiquetas y cajas de texto para que se muestren
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 18/Enero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Checked(String Operacion, Boolean Habilitado)
    {
        try
        {
            switch (Operacion)
            {
                case "Todo":
                    //  para compras otorgadas
                    Lbl_Fecha_Inicial.Visible = false;
                    Txt_Fecha_Inicial.Visible = false;
                    Btn_Fecha_Inicial.Visible = false;
                    Lbl_Fecha_Final.Visible = false;
                    Txt_Fecha_Final.Visible = false;
                    Btn_Fecha_Final.Visible = false;
                    //  para partida
                    Lbl_Partida.Visible = false;
                    Cmb_Partida.Visible = false;
                    // para fecha de registro 
                    Lbl_Fecha_Ini.Visible = false;
                    Lbl_Fecha_Fin.Visible = false;
                    Txt_Fecha_Ini.Visible = false;
                    Txt_Fecha_Fin.Visible = false;
                    Btn_Fecha_Ini.Visible = false;
                    Btn_Fecha_Fin.Visible = false;
                    Lbl_Fecha_Ini_Act.Visible = false;
                    Lbl_Fecha_Fin_Act.Visible = false;
                    Txt_Fecha_Ini_Act.Visible = false;
                    Txt_Fecha_Fin_Act.Visible = false;
                    Btn_Fecha_Ini_Act.Visible = false;
                    Btn_Fecha_Fin_Act.Visible = false;
                    break;

                case "Fecha":
                    //  para fechas
                    Lbl_Fecha_Inicial.Visible = Habilitado;
                    Txt_Fecha_Inicial.Visible = Habilitado;
                    Btn_Fecha_Inicial.Visible = Habilitado;
                    Lbl_Fecha_Final.Visible = Habilitado;
                    Txt_Fecha_Final.Visible = Habilitado;
                    Btn_Fecha_Final.Visible = Habilitado;

                    break;
                case "Partida":
                    //para el numero de contra recibo
                    Lbl_Partida.Visible = Habilitado;
                    Cmb_Partida.Visible = Habilitado;
                   
                    break;
                case "Registro":

                    Lbl_Fecha_Ini.Visible = Habilitado;
                    Txt_Fecha_Ini.Visible = Habilitado;
                    Btn_Fecha_Ini.Visible = Habilitado;
                    Lbl_Fecha_Fin.Visible = Habilitado;
                    Txt_Fecha_Fin.Visible = Habilitado;
                    Btn_Fecha_Fin.Visible = Habilitado;    
                  
                    
                    break;
                case "Actualizacion":
                    Lbl_Fecha_Ini_Act.Visible = Habilitado;
                    Lbl_Fecha_Fin_Act.Visible = Habilitado;
                    Txt_Fecha_Ini_Act.Visible = Habilitado;
                    Txt_Fecha_Fin_Act.Visible = Habilitado;
                    Btn_Fecha_Ini_Act.Visible = Habilitado;
                    Btn_Fecha_Fin_Act.Visible = Habilitado;
                 



                    break;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Limpia_Controles " + ex.Message.ToString());
        }
    }
    #endregion

    #region (Operaion)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN:          Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///                      en la busqueda del Modalpopup
    ///PARAMETROS:   
    ///CREO:                 Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:           18/Enero/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public bool Verificar_Fecha(TextBox Txt_Inicial, TextBox Txt_Final)
    {
        DateTime Date1 = new DateTime();  //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
        DateTime Date2 = new DateTime();
        Boolean Fecha_Valida = true;

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if ((Txt_Inicial.Text.Length != 0))
            {
                // Convertimos el Texto de los TextBox fecha a dateTime
                Date1 = DateTime.Parse(Txt_Inicial.Text);
                Date2 = DateTime.Parse(Txt_Final.Text);

                //Validamos que las fechas sean iguales o la final sea mayor que la inicial, de lo contrario se manda un mensaje de error 
                if ((Date1 < Date2) | (Date1 == Date2))
                {
                    Fecha_Valida = true;
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = " La fecha inicial no pude ser mayor que la fecha final <br />";
                    Fecha_Valida = false;
                }
            }

            return Fecha_Valida;
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message;
            throw new Exception(ex.Message, ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN:          Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:           1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                      en caso de que cumpla la condicion del if
    ///CREO:                 Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:           18/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Formato_Fecha(String Fecha)
    {
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;

        String Fecha_Valida = Fecha;
        //Se le aplica un split a la fecha 
        String[] aux = Fecha.Split('/');
        //Se modifica a mayusculas para que oracle acepte el formato. 
        switch (aux[1])
        {
            case "dic":
                aux[1] = "DEC";
                break;
        }
        //Concatenamos la fecha, y se cambia el orden a DD-MMM-YYYY para que sea una fecha valida para oracle
        Fecha_Valida = aux[0] + "-" + aux[1] + "-" + aux[2];
        return Fecha_Valida;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Aumentar_Fecha
    ///DESCRIPCIÓN:          Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:           1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                      en caso de que cumpla la condicion del if
    ///CREO:                 Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:           18/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Aumentar_Fecha(String Fecha)
    {
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Int32 Dia = 0;
        Int32 Anio = 0;
        String Fecha_Valida = Fecha;
        //Se le aplica un split a la fecha 
        String[] aux = Fecha.Split('/');
        //Se modifica a mayusculas para que oracle acepte el formato. 
        #region Mes
        switch (aux[1])
        {
            case "ene":
                Dia = Convert.ToInt32(aux[0]) + 1;
                if (Dia == 32)
                {
                    aux[0] = "1";
                    aux[1] = "feb";
                }
                else
                {
                    aux[0] = "" + Dia;
                }
                break;

            case "feb":
                Dia = Convert.ToInt32(aux[0]) + 1;
                Anio = Convert.ToInt32(aux[2]);
                Anio = Anio % 4;
                if ((Dia == 30) && (Anio == 0))
                {
                    aux[0] = "1";
                    aux[1] = "mar";
                }
                else if (Dia == 29)
                {
                    aux[0] = "1";
                    aux[1] = "mar";
                }
                else
                {
                    aux[0] = "" + Dia;
                }
                break;

            case "mar":
                Dia = Convert.ToInt32(aux[0]) + 1;
                if (Dia == 32)
                {
                    aux[0] = "1";
                    aux[1] = "abr";
                }
                else
                {
                    aux[0] = "" + Dia;
                }
                break;

            case "abr":
                Dia = Convert.ToInt32(aux[0]) + 1;
                if (Dia == 31)
                {
                    aux[0] = "1";
                    aux[1] = "may";
                }
                break;
            case "may":
                Dia = Convert.ToInt32(aux[0]) + 1;
                if (Dia == 32)
                {
                    aux[0] = "1";
                    aux[1] = "jun";
                }
                else
                {
                    aux[0] = "" + Dia;
                }
                break;
            case "jun":
                Dia = Convert.ToInt32(aux[0]) + 1;
                if (Dia == 31)
                {
                    aux[0] = "1";
                    aux[1] = "jul";
                }
                else
                {
                    aux[0] = "" + Dia;
                }
                break;
            case "jul":
                Dia = Convert.ToInt32(aux[0]) + 1;
                if (Dia == 32)
                {
                    aux[0] = "1";
                    aux[1] = "ago";
                }
                else
                {
                    aux[0] = "" + Dia;
                }
                break;
            case "ago":
                Dia = Convert.ToInt32(aux[0]) + 1;
                if (Dia == 32)
                {
                    aux[0] = "1";
                    aux[1] = "sep";
                }
                else
                {
                    aux[0] = "" + Dia;
                }
                break;
            case "sep":
                Dia = Convert.ToInt32(aux[0]) + 1;
                if (Dia == 31)
                {
                    aux[0] = "1";
                    aux[1] = "oct";
                }
                else
                {
                    aux[0] = "" + Dia;
                }
                break;
            case "oct":
                Dia = Convert.ToInt32(aux[0]) + 1;
                if (Dia == 32)
                {
                    aux[0] = "1";
                    aux[1] = "nov";
                }
                else
                {
                    aux[0] = "" + Dia;
                }
                break;
            case "nov":
                Dia = Convert.ToInt32(aux[0]) + 1;
                if (Dia == 31)
                {
                    aux[0] = "1";
                    aux[1] = "dic";
                }
                else
                {
                    aux[0] = "" + Dia;
                }
                break;
            case "dic":
                Dia = Convert.ToInt32(aux[0]) + 1;
                Anio = Convert.ToInt32(aux[2]) + 1;
                if (Dia == 32)
                {
                    aux[0] = "1";
                    aux[1] = "ene";
                    aux[2] = "" + Anio;
                }
                else
                {
                    aux[0] = "" + Dia;
                }
                break;
        }
        #endregion
        //Concatenamos la fecha, y se cambia el orden a DD-MMM-YYYY para que sea una fecha valida para oracle
        Fecha_Valida = aux[0] + "/" + aux[1] + "/" + aux[2];
        return Fecha_Valida;
    }
     ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Generar_Rpt_Proveedores
    /// DESCRIPCION :   Se encarga de generar el archivo de excel pasandole los paramentros
    ///                 al documento
    /// PARAMETROS  :   Dt_Reporte.- Es la consulta que se va a reportar
    /// CREO        :   Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  :   19/Enero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public void Generar_Rpt_Proveedores(DataTable Dt_Reporte)
    {
        Cls_Cat_Com_Partidas_Negocio Partida = new Cls_Cat_Com_Partidas_Negocio();
        WorksheetCell Celda = new WorksheetCell();
        DataTable Dt_Consulta = new DataTable();
        DataTable Dt_Documentos = new DataTable();
        String Nombre_Archivo = "";
        String Ruta_Archivo = "";
        String Tipo_Estilo = "";
        String Tipo_Estilo_Total = "";
        String Tipo_Ventas = "";
        Double Importe = 0.0;
        String Informacion_Registro = "";
        Int32 Contador_Estilo= 0;
        Int32 Operacion = 0;
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            Nombre_Archivo = "Rpt_Proveedores.xls";
            Ruta_Archivo = @Server.MapPath("../../Archivos/" + Nombre_Archivo);

            //  Creamos el libro de Excel.
            CarlosAg.ExcelXmlWriter.Workbook Libro = new CarlosAg.ExcelXmlWriter.Workbook();
            //  propiedades del libro
            Libro.Properties.Title = "Reporte_Proveedores";
            Libro.Properties.Created = DateTime.Now;
            Libro.Properties.Author = "Presidencia_";

            //  Creamos el estilo cabecera para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera = Libro.Styles.Add("HeaderStyle");
            //  Creamos el estilo cabecera 2 para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera2 = Libro.Styles.Add("HeaderStyle2");
            //  Creamos el estilo cabecera 3 para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Contenido2 = Libro.Styles.Add("BodyStyle2");
            //  Creamos el estilo contenido para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Contenido = Libro.Styles.Add("BodyStyle");
            //  Creamos el estilo contenido del presupuesto para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Totales = Libro.Styles.Add("Totales");
            //  Creamos el estilo contenido del concepto para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Totales2 = Libro.Styles.Add("Totales2");
            //  Creamos el estilo contenido del concepto para la hoja de excel.
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Presupuesto_Total = Libro.Styles.Add("Total");
            //Creamos el estilo contenido del concepto para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Concepto = Libro.Styles.Add("Concepto");
            //Creamos el estilo contenido del concepto para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Ventas = Libro.Styles.Add("Ventas");
            //Creamos el estilo contenido del concepto para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Ventas2 = Libro.Styles.Add("Ventas2");
            //  Creamos el estilo contenido del concepto para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Sin_Contenido = Libro.Styles.Add("Sin_Contenido");


            //***************************************inicio de los estilos***********************************************************
            //  estilo para la cabecera
            Estilo_Cabecera.Font.FontName = "Tahoma";
            Estilo_Cabecera.Font.Size = 10;
            Estilo_Cabecera.Font.Bold = true;
            Estilo_Cabecera.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Cabecera.Alignment.Vertical = StyleVerticalAlignment.Center;
            Estilo_Cabecera.Alignment.Rotate = 0;
            Estilo_Cabecera.Font.Color = "#FFFFFF";
            Estilo_Cabecera.Interior.Color = "Gray";
            Estilo_Cabecera.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Cabecera.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //  estilo para la cabecera2
            Estilo_Cabecera2.Font.FontName = "Tahoma";
            Estilo_Cabecera2.Font.Size = 10;
            Estilo_Cabecera2.Font.Bold = true;
            Estilo_Cabecera2.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Cabecera2.Alignment.Vertical = StyleVerticalAlignment.JustifyDistributed;
            Estilo_Cabecera2.Alignment.Rotate = 0;
            Estilo_Cabecera2.Font.Color = "#FFFFFF";
            Estilo_Cabecera2.Interior.Color = "DarkGray";
            Estilo_Cabecera2.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Cabecera2.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera2.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera2.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera2.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //  estilo para la BodyStyle2
            Estilo_Contenido2.Font.FontName = "Tahoma";
            Estilo_Contenido2.Font.Size = 9;
            Estilo_Contenido2.Font.Bold = false;
            Estilo_Contenido2.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Contenido2.Alignment.Vertical = StyleVerticalAlignment.JustifyDistributed;
            Estilo_Contenido2.Alignment.Rotate = 0;
            Estilo_Contenido2.Font.Color = "#000000";
            Estilo_Contenido2.Interior.Color = "LightGray";
            Estilo_Contenido2.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Contenido2.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido2.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido2.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido2.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //estilo para el BodyStyle
            Estilo_Contenido.Font.FontName = "Tahoma";
            Estilo_Contenido.Font.Size = 9;
            Estilo_Contenido.Font.Bold = false;
            Estilo_Contenido.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Contenido.Alignment.Vertical = StyleVerticalAlignment.JustifyDistributed;
            Estilo_Contenido.Alignment.Rotate = 0;
            Estilo_Contenido.Font.Color = "#000000";
            Estilo_Contenido.Interior.Color = "White";
            Estilo_Contenido.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Contenido.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");


            //  estilo para el presupuesto (importe)
            Estilo_Totales.Font.FontName = "Tahoma";
            Estilo_Totales.Font.Size = 9;
            Estilo_Totales.Font.Bold = false;
            Estilo_Totales.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            Estilo_Totales.Alignment.Vertical = StyleVerticalAlignment.Center;
            Estilo_Totales.Alignment.Rotate = 0;
            Estilo_Totales.Font.Color = "#000000";
            Estilo_Totales.Interior.Color = "White";
            Estilo_Totales.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Totales.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Totales.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Totales.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Totales.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //  estilo para el presupuesto (importe)
            Estilo_Totales2.Font.FontName = "Tahoma";
            Estilo_Totales2.Font.Size = 9;
            Estilo_Totales2.Font.Bold = false;
            Estilo_Totales2.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            Estilo_Totales2.Alignment.Vertical = StyleVerticalAlignment.Center;
            Estilo_Totales2.Alignment.Rotate = 0;
            Estilo_Totales2.Font.Color = "#000000";
            Estilo_Totales2.Interior.Color = "LightGray";
            Estilo_Totales2.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Totales2.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Totales2.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Totales2.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Totales2.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
            
            //estilo para el  (Total del importe)
            Estilo_Presupuesto_Total.Font.FontName = "Tahoma";
            Estilo_Presupuesto_Total.Font.Size = 9;
            Estilo_Presupuesto_Total.Font.Bold = true;
            Estilo_Presupuesto_Total.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            Estilo_Presupuesto_Total.Alignment.Vertical = StyleVerticalAlignment.Center;
            Estilo_Presupuesto_Total.Alignment.Rotate = 0;
            Estilo_Presupuesto_Total.Font.Color = "#000000";
            Estilo_Presupuesto_Total.Interior.Color = "Yellow";
            Estilo_Presupuesto_Total.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Presupuesto_Total.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Presupuesto_Total.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Presupuesto_Total.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Presupuesto_Total.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //estilo para el Concepto
            Estilo_Concepto.Font.FontName = "Tahoma";
            Estilo_Concepto.Font.Size = 9;
            Estilo_Concepto.Font.Bold = false;
            Estilo_Concepto.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Concepto.Alignment.Vertical = StyleVerticalAlignment.Center;
            Estilo_Concepto.Font.Color = "#000000";
            Estilo_Concepto.Interior.Color = "White";
            Estilo_Concepto.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Concepto.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Concepto.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Concepto.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Concepto.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //  para ventas
            Estilo_Ventas.Font.FontName = "Tahoma";
            Estilo_Ventas.Font.Size = 9;
            Estilo_Ventas.Font.Bold = false;
            Estilo_Ventas.Alignment.Horizontal = StyleHorizontalAlignment.JustifyDistributed;
            Estilo_Ventas.Alignment.Vertical = StyleVerticalAlignment.JustifyDistributed;
            Estilo_Ventas.Font.Color = "#000000";
            Estilo_Ventas.Interior.Color = "White";
            Estilo_Ventas.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Ventas.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Ventas.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Ventas.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Ventas.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //  para ventas2
            Estilo_Ventas2.Font.FontName = "Tahoma";
            Estilo_Ventas2.Font.Size = 9;
            Estilo_Ventas2.Font.Bold = false;
            Estilo_Ventas2.Alignment.Horizontal = StyleHorizontalAlignment.JustifyDistributed;
            Estilo_Ventas2.Alignment.Vertical = StyleVerticalAlignment.JustifyDistributed;
            Estilo_Ventas2.Font.Color = "#000000";
            Estilo_Ventas2.Interior.Color = "LightGray";
            Estilo_Ventas2.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Ventas2.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Ventas2.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Ventas2.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Ventas2.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //  estilo para los sin documentos
            Estilo_Sin_Contenido.Font.FontName = "Arial";
            Estilo_Sin_Contenido.Font.Size = 9;
            Estilo_Sin_Contenido.Font.Bold = true;
            Estilo_Sin_Contenido.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Sin_Contenido.Alignment.Vertical = StyleVerticalAlignment.Center;
            Estilo_Sin_Contenido.Alignment.Rotate = 0;
            Estilo_Sin_Contenido.Font.Color = "#000000";
            Estilo_Sin_Contenido.Interior.Color = "White";
            Estilo_Sin_Contenido.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Sin_Contenido.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Sin_Contenido.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Sin_Contenido.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Sin_Contenido.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
            //*************************************** fin de los estilos***********************************************************

            //***************************************Inicio del reporte Proveedores por partida Hoja 1***************************
            
            //  Creamos una hoja que tendrá el libro.
            CarlosAg.ExcelXmlWriter.Worksheet Hoja = Libro.Worksheets.Add("REPORTE DE PROVEEDORES");
            //  Agregamos un renglón a la hoja de excel.
            CarlosAg.ExcelXmlWriter.WorksheetRow Renglon = Hoja.Table.Rows.Add();

            if (Chk_Partida.Checked == true)
            {
                //  Agregamos las columnas que tendrá la hoja de excel.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(50));//  1 PAdron.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));//  1 RFC.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(125));//  2 nombre del proveedor.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(125));//  3 compañia.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(125));//  4 Representante legal.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(125));//  5 Contacto.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(50));//  6 Estatus.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));//  7 Fecha_Registro.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));//  8 Telefono 1.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));//  9 telefono 2.

                //  se llena el encabezado principal
                Renglon = Hoja.Table.Rows.Add();
                Celda = Renglon.Cells.Add("REPORTE DE PROVEEDORES");
                Celda.MergeAcross = 8; // Merge 6 cells together
                Celda.StyleID = "HeaderStyle";

                Renglon = Hoja.Table.Rows.Add();
                Celda = Renglon.Cells.Add("IRAPUATO, GUANAJUATO, " + DateTime.Now);
                Celda.MergeAcross = 8; // Merge 6 cells together
                Celda.StyleID = "HeaderStyle";

                if (Dt_Reporte.Rows.Count > 0)
                {
                    //  Se comienza a extraer la informaicon de la consulta para saber que partida se selecciono
                    foreach (DataRow Renglon_Reporte in Dt_Reporte.Rows)
                    {
                        Informacion_Registro = (Renglon_Reporte["CLAVE_PARTIDA"].ToString());
                        Informacion_Registro += " ";
                        Informacion_Registro += (Renglon_Reporte["NOMBRE_PARTIDA"].ToString());
                        Renglon = Hoja.Table.Rows.Add();
                        Celda = Renglon.Cells.Add("RELACION DE LA PARTIDA " + Informacion_Registro);
                        Celda.MergeAcross = 8; // Merge 6 cells together
                        Celda.StyleID = "HeaderStyle";
                        break;
                    }
                }
                else
                {
                    Partida.P_Partida_Generica_ID = Cmb_Partida.SelectedValue;
                    Dt_Consulta = Partida.Consulta_Partidas_Genericas();

                    foreach (DataRow Renglon_Reporte in Dt_Consulta.Rows)
                    {
                        Informacion_Registro=(Renglon_Reporte["Clave_Descripcion"].ToString());
                    }
                    Renglon = Hoja.Table.Rows.Add();
                    Celda = Renglon.Cells.Add("RELACION DE LA PARTIDA " + Informacion_Registro);
                    Celda.MergeAcross = 8; // Merge 6 cells together
                    Celda.StyleID = "HeaderStyle";
                }

                //  para los titulos de las columnas
                Renglon = Hoja.Table.Rows.Add();
                Renglon = Hoja.Table.Rows.Add();
                
                Celda = Renglon.Cells.Add("PADRON");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("RFC");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("NOMBRE DEL PROVEEDOR");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("COMPAÑIA");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("REPRESENTANTE LEGAL");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("CONTACTO");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("ESTATUS");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("FECHA REGISTRO");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("TELEFONO 1");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("TELEFONO 2");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Renglon = Hoja.Table.Rows.Add();

                //  para llenar el reporte
                if (Dt_Reporte.Rows.Count > 0)
                {
                    //  Se comienza a extraer la informaicon de la onsulta
                    foreach (DataRow Renglon_Reporte in Dt_Reporte.Rows)
                    {
                        Contador_Estilo++;
                        Operacion = Contador_Estilo % 2;
                        if (Operacion == 0)
                        {
                            //Estilo_Concepto.Interior.Color = "LightGray";
                            Tipo_Estilo = "BodyStyle2";
                        }
                        else
                        {
                            Tipo_Estilo = "BodyStyle";
                            //Estilo_Concepto.Interior.Color = "White";
                        }

                        Renglon = Hoja.Table.Rows.Add();
                        Informacion_Registro = (Convert.ToInt64( Renglon_Reporte["PROVEEDOR_ID"].ToString()).ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  1 para el rfc 
                        Informacion_Registro = (Renglon_Reporte["RFC"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  2 para el NOMBRE del PROVEEDOR 
                        Informacion_Registro = (Renglon_Reporte["NOMBRE_PROVEEDOR"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  3 para la compañia
                        Informacion_Registro = (Renglon_Reporte["Compañia"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  4 para el representante legal
                        Informacion_Registro = (Renglon_Reporte["Representante"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  5 para el contacto
                        Informacion_Registro = (Renglon_Reporte["CONTACTO"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  6 para el estatus
                        Informacion_Registro = (Renglon_Reporte["ESTATUS"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  7 para el Fecha registro
                        Informacion_Registro = (Renglon_Reporte["FECHA_REGISTRO"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  8 para el telefono 1
                        Informacion_Registro = (Renglon_Reporte["TELEFONO1"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  9 para el telefono 2
                        Informacion_Registro = (Renglon_Reporte["TELEFONO2"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                    }
                }
                else
                {
                    Renglon = Hoja.Table.Rows.Add();
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                }
            }
            //***************************************Fin del reporte Proveedores por partida Hoja 1***************************

            //***************************************Inicio del reporte Proveedores por fecha Hoja 1***************************

            if (Chk_Compras_Otorgadas.Checked == true)
            {
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80)); // Padron de Proveedor
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(250));//  1 NOMBRE_PROVEEDOR.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));//   2 ARTICULOS_VENDIDOS.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(250));//  3 TIPO_ARTICULO.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));//  4 SUB_TOTAL legal.
                //Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));//  5 TOTAL_IEPS.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));//  6 TOTAL_IVA.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));//  7 TOTAL.

                //  se llena el encabezado principal
                Renglon = Hoja.Table.Rows.Add();
                Celda = Renglon.Cells.Add("REPORTE DE PROVEEDORES");
                Celda.MergeAcross = 5; // Merge 6 cells together
                Celda.StyleID = "HeaderStyle";

                Renglon = Hoja.Table.Rows.Add();
                Celda = Renglon.Cells.Add("IRAPUATO, GUANAJUATO, " + DateTime.Now);
                Celda.MergeAcross = 5; // Merge 6 cells together
                Celda.StyleID = "HeaderStyle";

                Renglon = Hoja.Table.Rows.Add();
                Celda = Renglon.Cells.Add("VENTAS REALIZADAS Y ARTICULOS VENDIDOS EN EL PERIODO DEL  " + Txt_Fecha_Inicial.Text.ToUpper() +
                        "  AL  " + Txt_Fecha_Final.Text.ToUpper());
                Celda.MergeAcross = 5; // Merge 6 cells together
                Celda.StyleID = "HeaderStyle";

                //  se llenan los titulos de la tabla
                Renglon = Hoja.Table.Rows.Add();
                Renglon = Hoja.Table.Rows.Add();

                Celda = Renglon.Cells.Add("PADRON");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("NOMBRE DEL PROVEEDOR");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("VENTAS REALIZADAS");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("ARTICULOS VENDIDOS");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("SUB TOTAL");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                //Celda = Renglon.Cells.Add("TOTAL IEPS");
                //Celda.MergeDown = 1; // Merge two cells together
                //Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("IVA");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("TOTAL");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                //  para el contenido de la tabla
                Renglon = Hoja.Table.Rows.Add();
                //  para llenar el reporte
                if (Dt_Reporte.Rows.Count > 0)
                {
                    Contador_Estilo = 0;
                    //  Se comienza a extraer la informaicon de la onsulta
                    foreach (DataRow Renglon_Reporte in Dt_Reporte.Rows)
                    {
                        Contador_Estilo++;
                        Operacion = Contador_Estilo % 2;
                        if (Operacion == 0)
                        {
                            Tipo_Estilo = "BodyStyle2";
                            Tipo_Estilo_Total = "Totales2";
                            Tipo_Ventas = "Ventas2";
                        }
                        else
                        {
                            Tipo_Estilo = "BodyStyle";
                            Tipo_Estilo_Total = "Totales";
                            Tipo_Ventas = "Ventas";
                        }
                        Renglon = Hoja.Table.Rows.Add();
                        //  1 para el numero de Padron de Proveedor
                        Informacion_Registro = Convert.ToInt64(Renglon_Reporte["PROVEEDOR_ID"].ToString()).ToString();
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  1 para el Nombre del proveedor 
                        Informacion_Registro = (Renglon_Reporte["NOMBRE_PROVEEDOR"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  2 para los articulos vendidos 
                        Informacion_Registro = (Renglon_Reporte["ARTICULOS_VENDIDOS"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Ventas));
                        //  3 para el tipo de articulo vendido 
                        Informacion_Registro = (Renglon_Reporte["TIPO_ARTICULO"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  4 para el sub total
                        Informacion_Registro = (Renglon_Reporte["SUB_TOTAL"].ToString());
                        Importe = Convert.ToDouble(Informacion_Registro);
                        Informacion_Registro=String.Format("{0:n}", Importe);
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo_Total));
                        //  5 para el total ieps
                        //Informacion_Registro = (Renglon_Reporte["TOTAL_IEPS"].ToString());
                        //Importe = Convert.ToDouble(Informacion_Registro);
                        //Informacion_Registro = String.Format("{0:n}", Importe);
                        //Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo_Total));
                        //  6 para el total iva
                        Informacion_Registro = (Renglon_Reporte["TOTAL_IVA"].ToString());
                        Importe = Convert.ToDouble(Informacion_Registro);
                        Informacion_Registro = String.Format("{0:n}", Importe);
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo_Total));
                        //  7 para el total 
                        Informacion_Registro = (Renglon_Reporte["TOTAL"].ToString());
                        Importe = Convert.ToDouble(Informacion_Registro);
                        Informacion_Registro = String.Format("{0:n}", Importe);
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo_Total));
                    }
                }
                else
                {
                    Renglon = Hoja.Table.Rows.Add();
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    //Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                }

            }
            if (Chk_Fecha_Registro.Checked == true)
            {
                //  Agregamos las columnas que tendrá la hoja de excel.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(50));//  1 Padron.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));//  1 RFC.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(125));//  2 nombre del proveedor.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(125));//  3 compañia.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(125));//  4 Representante legal.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(125));//  5 Contacto.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(50));//  6 Estatus.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));//  7 Fecha_Registro.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));//  8 Telefono 1.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));//  9 telefono 2.

                //  se llena el encabezado principal
                Renglon = Hoja.Table.Rows.Add();
                Celda = Renglon.Cells.Add("REPORTE DE PROVEEDORES");
                Celda.MergeAcross = 8; // Merge 6 cells together
                Celda.StyleID = "HeaderStyle";

                Renglon = Hoja.Table.Rows.Add();
                Celda = Renglon.Cells.Add("IRAPUATO, GUANAJUATO, " + DateTime.Now);
                Celda.MergeAcross = 8; // Merge 6 cells together
                Celda.StyleID = "HeaderStyle";

                if (Dt_Reporte.Rows.Count > 0)
                {
                    //  Se comienza a extraer la informaicon de la consulta para saber que partida se selecciono
                    //foreach (DataRow Renglon_Reporte in Dt_Reporte.Rows)
                    //{
                    //    Informacion_Registro = (Renglon_Reporte["CLAVE_PARTIDA"].ToString());
                    //    Informacion_Registro += " ";
                    //    Informacion_Registro += (Renglon_Reporte["NOMBRE_PARTIDA"].ToString());
                    //    Renglon = Hoja.Table.Rows.Add();
                    //    Celda = Renglon.Cells.Add("RELACION DE LA PARTIDA " + Informacion_Registro);
                    //    Celda.MergeAcross = 8; // Merge 6 cells together
                    //    Celda.StyleID = "HeaderStyle";
                    //    break;
                    //}
                }
                else
                {
                    //Partida.P_Partida_Generica_ID = Cmb_Partida.SelectedValue;
                    //Dt_Consulta = Partida.Consulta_Partidas_Genericas();

                    //foreach (DataRow Renglon_Reporte in Dt_Consulta.Rows)
                    //{
                    //    Informacion_Registro = (Renglon_Reporte["Clave_Descripcion"].ToString());
                    //}
                    //Renglon = Hoja.Table.Rows.Add();
                    //Celda = Renglon.Cells.Add("RELACION DE LA PARTIDA " + Informacion_Registro);
                    //Celda.MergeAcross = 8; // Merge 6 cells together
                    //Celda.StyleID = "HeaderStyle";
                }

                //  para los titulos de las columnas
                Renglon = Hoja.Table.Rows.Add();
                Renglon = Hoja.Table.Rows.Add();

                Celda = Renglon.Cells.Add("PADRON");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("RFC");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("NOMBRE DEL PROVEEDOR");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("COMPAÑIA");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("REPRESENTANTE LEGAL");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("CONTACTO");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("ESTATUS");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("FECHA REGISTRO");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("TELEFONO 1");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("TELEFONO 2");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Renglon = Hoja.Table.Rows.Add();

                //  para llenar el reporte
                if (Dt_Reporte.Rows.Count > 0)
                {
                    //  Se comienza a extraer la informaicon de la onsulta
                    foreach (DataRow Renglon_Reporte in Dt_Reporte.Rows)
                    {
                        Contador_Estilo++;
                        Operacion = Contador_Estilo % 2;
                        if (Operacion == 0)
                        {
                            //Estilo_Concepto.Interior.Color = "LightGray";
                            Tipo_Estilo = "BodyStyle2";
                        }
                        else
                        {
                            Tipo_Estilo = "BodyStyle";
                            //Estilo_Concepto.Interior.Color = "White";
                        }

                        Renglon = Hoja.Table.Rows.Add();
                        //  1 para el Padron 
                        Informacion_Registro = Convert.ToInt64(Renglon_Reporte["PROVEEDOR_ID"].ToString()).ToString();
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  1 para el rfc 
                        Informacion_Registro = (Renglon_Reporte["RFC"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  2 para el NOMBRE del PROVEEDOR 
                        Informacion_Registro = (Renglon_Reporte["NOMBRE_PROVEEDOR"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  3 para la compañia
                        Informacion_Registro = (Renglon_Reporte["Compañia"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  4 para el representante legal
                        Informacion_Registro = (Renglon_Reporte["Representante"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  5 para el contacto
                        Informacion_Registro = (Renglon_Reporte["CONTACTO"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  6 para el estatus
                        Informacion_Registro = (Renglon_Reporte["ESTATUS"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  7 para el Fecha registro
                        Informacion_Registro = (Renglon_Reporte["FECHA_REGISTRO"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  8 para el telefono 1
                        Informacion_Registro = (Renglon_Reporte["TELEFONO1"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  9 para el telefono 2
                        Informacion_Registro = (Renglon_Reporte["TELEFONO2"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                    }
                }
                else
                {
                    Renglon = Hoja.Table.Rows.Add();
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                }
            }

            if (Chk_Fecha_Actualizacion.Checked == true)
            {
                //  Agregamos las columnas que tendrá la hoja de excel.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(50));//  1 Padron.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));//  1 RFC.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(125));//  2 nombre del proveedor.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(125));//  3 compañia.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(125));//  4 Representante legal.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(125));//  5 Contacto.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(50));//  6 Estatus.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));//  7 Fecha_Actualizacion.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));//  8 Telefono 1.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));//  9 telefono 2.

                //  se llena el encabezado principal
                Renglon = Hoja.Table.Rows.Add();
                Celda = Renglon.Cells.Add("REPORTE DE PROVEEDORES");
                Celda.MergeAcross = 8; // Merge 6 cells together
                Celda.StyleID = "HeaderStyle";

                Renglon = Hoja.Table.Rows.Add();
                Celda = Renglon.Cells.Add("IRAPUATO, GUANAJUATO, " + DateTime.Now);
                Celda.MergeAcross = 8; // Merge 6 cells together
                Celda.StyleID = "HeaderStyle";
                //  para los titulos de las columnas
                Renglon = Hoja.Table.Rows.Add();
                Renglon = Hoja.Table.Rows.Add();

                Celda = Renglon.Cells.Add("PADRON");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("RFC");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("NOMBRE DEL PROVEEDOR");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("COMPAÑIA");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("REPRESENTANTE LEGAL");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("CONTACTO");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("ESTATUS");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("FECHA ACTUALIZACION");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("TELEFONO 1");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Celda = Renglon.Cells.Add("TELEFONO 2");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                Renglon = Hoja.Table.Rows.Add();

                //  para llenar el reporte
                if (Dt_Reporte.Rows.Count > 0)
                {
                    //  Se comienza a extraer la informaicon de la onsulta
                    foreach (DataRow Renglon_Reporte in Dt_Reporte.Rows)
                    {
                        Contador_Estilo++;
                        Operacion = Contador_Estilo % 2;
                        if (Operacion == 0)
                        {
                            //Estilo_Concepto.Interior.Color = "LightGray";
                            Tipo_Estilo = "BodyStyle2";
                        }
                        else
                        {
                            Tipo_Estilo = "BodyStyle";
                            //Estilo_Concepto.Interior.Color = "White";
                        }

                        Renglon = Hoja.Table.Rows.Add();
                        //  1 para el PADRON 
                        Informacion_Registro = Convert.ToInt64(Renglon_Reporte["PROVEEDOR_ID"].ToString()).ToString();
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  1 para el rfc 
                        Informacion_Registro = (Renglon_Reporte["RFC"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  2 para el NOMBRE del PROVEEDOR 
                        Informacion_Registro = (Renglon_Reporte["NOMBRE_PROVEEDOR"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  3 para la compañia
                        Informacion_Registro = (Renglon_Reporte["Compañia"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  4 para el representante legal
                        Informacion_Registro = (Renglon_Reporte["Representante"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  5 para el contacto
                        Informacion_Registro = (Renglon_Reporte["CONTACTO"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  6 para el estatus
                        Informacion_Registro = (Renglon_Reporte["ESTATUS"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  7 para el Fecha registro
                        Informacion_Registro = (Renglon_Reporte["FECHA_ACTUALIZACION"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  8 para el telefono 1
                        Informacion_Registro = (Renglon_Reporte["TELEFONO1"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                        //  9 para el telefono 2
                        Informacion_Registro = (Renglon_Reporte["TELEFONO2"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "" + Tipo_Estilo));
                    }
                }
                else
                {
                    Renglon = Hoja.Table.Rows.Add();
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Contenido"));
                }
            }


            //***************************************Fin del reporte Proveedores por fecha Hoja 1***************************
            //  se guarda el documento
            Libro.Save(Ruta_Archivo);
            //  mostrar el archivo
            //Mostrar_Reporte(Nombre_Archivo);


            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Ruta_Archivo);
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Libro.Save(Response.OutputStream);
            Response.End();
        }// fin try

        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    /// *************************************************************************************
    /// NOMBRE:              Mostrar_Reporte
    /// DESCRIPCIÓN:         Muestra el reporte en pantalla.
    /// PARÁMETROS:          Nombre_Reporte_Generar.- Nombre que tiene el reporte que se mostrará en pantalla.
    /// USUARIO CREO:        Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO:          18/Enero/2012
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACIÓN:
    /// *************************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte_Generar)
    {
        String Ruta = "../../Archivos/" + Nombre_Reporte_Generar;
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=100,height=100')", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region(Validacion)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Reporte
    /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el reporte
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 18/Enero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Reporte()
    {
        String Espacios_Blanco;
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario Introducir: <br>";
        Lbl_Mensaje_Error.Visible = true;
        Img_Error.Visible = true;

        if (Chk_Compras_Otorgadas.Checked == true)
        {
            if (Txt_Fecha_Inicial.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Selecione la fecha inicial.<br>";
                Datos_Validos = false;
            }
            if (Txt_Fecha_Final.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Selecione la fecha final.<br>";
                Datos_Validos = false;
            }
        }
        //  para partida
        else if (Chk_Partida.Checked == true)
        {
            if (Cmb_Partida.SelectedIndex == -1)
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el numero de contra recibo.<br>";
                Datos_Validos = false;
            }
        }
        else if (Chk_Fecha_Registro.Checked == true)
        {
            if (Txt_Fecha_Ini.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Selecione la fecha inicial.<br>";
                Datos_Validos = false;
            }
            if (Txt_Fecha_Fin.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Selecione la fecha final.<br>";
                Datos_Validos = false;
            }
        }
        else if (Chk_Fecha_Actualizacion.Checked == true)
        {
            if (Txt_Fecha_Ini_Act.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Selecione la fecha inicial.<br>";
                Datos_Validos = false;
            }
            if (Txt_Fecha_Fin_Act.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Selecione la fecha final.<br>";
                Datos_Validos = false;
            }
        }
        else
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione el tipo de reporte.<br>";
            Datos_Validos = false;
        }
        return Datos_Validos;
    }
    #endregion

    #region (Consultas)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Realizar_Consulta
    ///DESCRIPCIÓN: Realizara la consulta de lo que s quiere imprimir
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  19/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public DataTable Realizar_Consulta()
    {
        Cls_Rpt_Com_Proveedores_Excel_Negocio Consulta_Proveedores = new Cls_Rpt_Com_Proveedores_Excel_Negocio();
        DataTable Dt_Consulta = new DataTable();
        bool Consulta = false;
        String Dia = "";
         try
        {
            if (Chk_Partida.Checked == true)
            {
                Consulta_Proveedores.P_Partida_Generica_ID = Cmb_Partida.SelectedValue;
                Dt_Consulta = Consulta_Proveedores.Consultar_Datos_Proveedor();
            }
            if (Chk_Compras_Otorgadas.Checked == true)
            {
                if (!Verificar_Fecha(Txt_Fecha_Inicial,Txt_Fecha_Final) )
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Consulta_Proveedores.P_Fecha_Inicial = null;
                    Consulta_Proveedores.P_Fecha_Final = null;
                    Consulta_Proveedores.P_Estatus = null;
                    Consulta = false;
                }
                else
                {
                    Consulta_Proveedores.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Inicial.Text.Trim());
                    Dia = Aumentar_Fecha(Txt_Fecha_Final.Text);
                    Consulta_Proveedores.P_Fecha_Final = Formato_Fecha(Dia);
                    Consulta_Proveedores.P_Estatus = "AUTORIZADA";
                    Consulta = true;
                }
                if (Consulta == true)
                {
                    Dt_Consulta = Consulta_Proveedores.Consultar_Ventas_Proveedor();
                }
            }
            if (Chk_Fecha_Registro.Checked == true)
            {
                if (!Verificar_Fecha(Txt_Fecha_Ini, Txt_Fecha_Fin))
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Consulta_Proveedores.P_Fecha_Inicial = null;
                    Consulta_Proveedores.P_Fecha_Final = null;
                    
                    Consulta = false;
                }
                else
                {
                    Consulta_Proveedores.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Ini.Text.Trim());
                    Dia = Aumentar_Fecha(Txt_Fecha_Fin.Text);
                    Consulta_Proveedores.P_Fecha_Final = Formato_Fecha(Dia);
                    Consulta = true;
                }
                if (Consulta == true)
                {
                    Dt_Consulta = Consulta_Proveedores.Consultar_Fecha_Registro();
                }
            }
            if (Chk_Fecha_Actualizacion.Checked == true)
            {
                if (!Verificar_Fecha(Txt_Fecha_Ini_Act, Txt_Fecha_Fin_Act))
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Consulta_Proveedores.P_Fecha_Inicial = null;
                    Consulta_Proveedores.P_Fecha_Final = null;

                    Consulta = false;
                }
                else
                {
                    Consulta_Proveedores.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Ini_Act.Text.Trim());
                    Dia = Aumentar_Fecha(Txt_Fecha_Fin_Act.Text);
                    Consulta_Proveedores.P_Fecha_Final = Formato_Fecha(Dia);
                    Consulta = true;
                }
                if (Consulta == true)
                {
                    Dt_Consulta = Consulta_Proveedores.Consultar_Fecha_Actualizacion();
                }
            }
            return Dt_Consulta;
        }
         catch (Exception ex)
         {
             Lbl_Mensaje_Error.Visible = true;
             Img_Error.Visible = true;
             Lbl_Mensaje_Error.Text = ex.Message;
             throw new Exception(ex.Message, ex);
         }

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Construir_Tabla
    ///DESCRIPCIÓN: Realizara la consulta de lo que se quiere imprimir
    ///PARAMETROS:  DataTable.- Contiene la consulta realizada por fecha o numero de contra recibo
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  18/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public DataTable Construir_Tabla (DataTable Dt_Consulta)
    {
        Cls_Rpt_Com_Proveedores_Excel_Negocio Consulta_Proveedores = new Cls_Rpt_Com_Proveedores_Excel_Negocio();
        DataTable Dt_Reporte_Proveedor = new DataTable();
        DataTable Dt_Auxiliar = new DataTable();
        Boolean Repetido = false;
        String Proveedor_ID = "";
        String Productos = "";
        String Dia = ""; 
        String Suma_Total="";
        Int32 Orden_Compra = 0;
        Int32 Contador_Articulos = 0;
        Int32 Articulos= 0;
        try
        {
            Dt_Reporte_Proveedor.Columns.Add("PROVEEDOR_ID");
            Dt_Reporte_Proveedor.Columns.Add("NOMBRE_PROVEEDOR");
            Dt_Reporte_Proveedor.Columns.Add("ARTICULOS_VENDIDOS");
            Dt_Reporte_Proveedor.Columns.Add("TIPO_ARTICULO");
            Dt_Reporte_Proveedor.Columns.Add("SUB_TOTAL");
            //Dt_Reporte_Proveedor.Columns.Add("TOTAL_IEPS");
            Dt_Reporte_Proveedor.Columns.Add("TOTAL_IVA");
            Dt_Reporte_Proveedor.Columns.Add("TOTAL");
            
            DataRow Dr_Registro;

            if (Dt_Consulta.Rows.Count > 0)
            {
                foreach (DataRow Registro in Dt_Consulta.Rows)
                {
                    if (Repetido)
                    {
                        Contador_Articulos--;
                        if (Contador_Articulos == 0)
                            Repetido = false;
                    }
                    else
                    {
                        Dr_Registro = Dt_Reporte_Proveedor.NewRow();
                        //  se toma el valor del proveedor id
                        Proveedor_ID = (Registro[Ope_Com_Ordenes_Compra.Campo_Proveedor_ID].ToString());
                        //  se consultas los articulos vendidos
                        Consulta_Proveedores.P_Proveedor_ID = Proveedor_ID;
                        Consulta_Proveedores.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Inicial.Text.Trim());
                        Dia = Aumentar_Fecha(Txt_Fecha_Final.Text);
                        Consulta_Proveedores.P_Fecha_Final = Formato_Fecha(Dia);
                        Consulta_Proveedores.P_Estatus = "AUTORIZADA";
                        Dt_Auxiliar = Consulta_Proveedores.Consultar_Ventas_Realizadas();
                        foreach (DataRow Articulos_Vendidos in Dt_Auxiliar.Rows)
                        {
                            Articulos=Convert.ToInt32((Articulos_Vendidos["ARTICULOS_VENDIDOS"].ToString()));
                        }
                        //  para asignar si se repite el registro
                        if (Articulos >= 2)
                        {
                            Repetido = true;
                            Contador_Articulos = Articulos - 1;
                        }
                        else
                            Articulos = 1;
                        // si existe un articulo solamente 
                        if (Articulos == 1)
                        {
                            Dr_Registro = Dt_Reporte_Proveedor.NewRow();
                            Dr_Registro["PROVEEDOR_ID"] = (Registro[Ope_Com_Ordenes_Compra.Campo_Proveedor_ID].ToString());
                            Dr_Registro["NOMBRE_PROVEEDOR"] = (Registro[Ope_Com_Ordenes_Compra.Campo_Nombre_Proveedor].ToString());
                            Dr_Registro["ARTICULOS_VENDIDOS"] = "1";
                            //Dr_Registro["TIPO_ARTICULO"] = (Registro[Ope_Com_Ordenes_Compra.Campo_Tipo_Articulo].ToString());
                            //  para el nombre de los articulos vendidos
                            Consulta_Proveedores.P_No_Requisicion = (Registro[Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones].ToString());
                            Dt_Auxiliar = Consulta_Proveedores.Consultar_Nombre_Articulos_Vendidos();
                            Contador_Articulos = 0;
                            Orden_Compra++;
                            Productos = "OREDEN DE COMPRA " + Orden_Compra + " --- ";
                            foreach (DataRow Tipo_Articulos in Dt_Auxiliar.Rows)
                            {
                                Contador_Articulos++;

                                if (Contador_Articulos == Dt_Auxiliar.Rows.Count)
                                    Productos += (Tipo_Articulos["TIPO_ARTICULO"].ToString());

                                else
                                    Productos += (Tipo_Articulos["TIPO_ARTICULO"].ToString()) + ", ";
                            }
                            Dr_Registro["TIPO_ARTICULO"] = Productos;
                            Dr_Registro["SUB_TOTAL"] = (Registro[Ope_Com_Ordenes_Compra.Campo_Subtotal].ToString());
                            //Dr_Registro["TOTAL_IEPS"] = (Registro[Ope_Com_Ordenes_Compra.Campo_Total_IEPS].ToString());
                            Dr_Registro["TOTAL_IVA"] = (Registro[Ope_Com_Ordenes_Compra.Campo_Total_IVA].ToString());
                            Dr_Registro["TOTAL"] = (Registro[Ope_Com_Ordenes_Compra.Campo_Total].ToString());
                            Dt_Reporte_Proveedor.Rows.Add(Dr_Registro);
                        }
                        else
                        {
                            Dr_Registro = Dt_Reporte_Proveedor.NewRow();
                            Dr_Registro["PROVEEDOR_ID"] = (Registro[Ope_Com_Ordenes_Compra.Campo_Proveedor_ID].ToString());
                            Dr_Registro["NOMBRE_PROVEEDOR"] = (Registro[Ope_Com_Ordenes_Compra.Campo_Nombre_Proveedor].ToString());
                            Dr_Registro["ARTICULOS_VENDIDOS"] = "" + Articulos;
                            
                            //  se consultan los tipos de articulos vendidos
                            Consulta_Proveedores.P_Proveedor_ID = Proveedor_ID;
                            Consulta_Proveedores.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Inicial.Text.Trim());
                            Dia = Aumentar_Fecha(Txt_Fecha_Final.Text);
                            Consulta_Proveedores.P_Fecha_Final = Formato_Fecha(Dia);
                            Consulta_Proveedores.P_Estatus = "AUTORIZADA";

                            //No_Requisicion = (Registro[Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones].ToString());
                            Dt_Auxiliar = Consulta_Proveedores.Consultar_Tipos_Articulos_Vendidos();
                            Contador_Articulos = 0;
                            DataTable Dt_Nombre_Productos = new DataTable();
                            Orden_Compra = 0;
                            foreach (DataRow Lista_Requisiciones in Dt_Auxiliar.Rows)
                            {
                                Orden_Compra++;
                                Consulta_Proveedores.P_No_Requisicion = (Lista_Requisiciones["TIPO_ARTICULO"].ToString());
                                Dt_Nombre_Productos = Consulta_Proveedores.Consultar_Nombre_Articulos_Vendidos();
                                Contador_Articulos = 0;
                                if(Orden_Compra==1)
                                    Productos = "OREDEN DE COMPRA " + Orden_Compra +" --- ";

                                else
                                    Productos += "--- OREDEN DE COMPRA " + Orden_Compra +" --- ";
                                foreach (DataRow Tipo_Articulos in Dt_Nombre_Productos.Rows)
                                {
                                    Contador_Articulos++;

                                    if (Contador_Articulos == Dt_Nombre_Productos.Rows.Count)
                                        Productos += (Tipo_Articulos["TIPO_ARTICULO"].ToString()) +" ";

                                    else
                                        Productos += (Tipo_Articulos["TIPO_ARTICULO"].ToString()) + ", ";
                                }
                            }
                            Dr_Registro["TIPO_ARTICULO"] = Productos;
                            
                            //para el total
                            Consulta_Proveedores.P_Proveedor_ID = Proveedor_ID;
                            Consulta_Proveedores.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Inicial.Text.Trim());
                            Dia = Aumentar_Fecha(Txt_Fecha_Final.Text);
                            Consulta_Proveedores.P_Fecha_Final = Formato_Fecha(Dia);
                            Consulta_Proveedores.P_Estatus = "AUTORIZADA";
                            Dt_Auxiliar = Consulta_Proveedores.Consultar_Suma_Ventas_Realizadas();
                            foreach (DataRow Tipo_Articulos in Dt_Auxiliar.Rows)
                            {
                                Suma_Total += (Tipo_Articulos["SUMA_TOTAL"].ToString());
                                Dr_Registro["SUB_TOTAL"] = (Tipo_Articulos["SUMA_SUBTOTAL"].ToString());
                                //Dr_Registro["TOTAL_IEPS"] = (Tipo_Articulos["SUMA_TOTAL_IEPS"].ToString());
                                Dr_Registro["TOTAL_IVA"] = (Tipo_Articulos["SUMA_TOTAL_IVA"].ToString());
                                Dr_Registro["TOTAL"] = (Tipo_Articulos["SUMA_TOTAL"].ToString());
                            }
                            Dt_Reporte_Proveedor.Rows.Add(Dr_Registro);
                        }
                        Suma_Total = "";
                        Productos = "";
                        Articulos = 0;
                        Orden_Compra = 0;
                    }
                }
            }
            return Dt_Reporte_Proveedor;
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message;
            throw new Exception(ex.Message, ex);
        }

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combo
    ///DESCRIPCIÓN: Realizara la consulta de lo que se quiere imprimir
    ///PARAMETROS:  DataTable.- Contiene la consulta realizada por fecha o numero de contra recibo
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  18/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Cargar_Combo()
    {
        Cls_Cat_Com_Partidas_Negocio Partida = new Cls_Cat_Com_Partidas_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Cmb_Partida.Items.Clear();
            Dt_Consulta= Partida.Consulta_Partidas_Genericas();
            Cmb_Partida.DataSource = Dt_Consulta;
            Cmb_Partida.DataValueField = Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID;
            Cmb_Partida.DataTextField = "Clave_Descripcion";
            Cmb_Partida.DataBind();
            Cmb_Partida.Items.Insert(0, "< SELECCIONE PARTIDA >");
            Cmb_Partida.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message;
            throw new Exception(ex.Message, ex);
        }

    }
    #endregion
#endregion

#region Eventos

    #region (Botones)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Excel_Click
    ///DESCRIPCIÓN: Realizara los metodos requeridos para el reporte
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  19/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Imprimir_Excel_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Consulta = new DataTable();
        try
        {
            if (Validar_Reporte())
            {
                if (Chk_Compras_Otorgadas.Checked == true )
                {
                    if (!Verificar_Fecha(Txt_Fecha_Inicial,Txt_Fecha_Final))
                    {

                    }
                    else
                    {
                        //  se realiza la consulta por fecha 
                        Dt_Consulta = Realizar_Consulta();
                        //  se construira la tabla
                        Dt_Consulta = Construir_Tabla(Dt_Consulta);
                        //  para generar el reporte de excel
                        Generar_Rpt_Proveedores(Dt_Consulta);
                    }
                }
                if (Chk_Partida.Checked == true)
                {
                    //  se realiza la consulta por fecha 
                    Dt_Consulta = Realizar_Consulta();
                    //  para generar el reporte de excel
                    Generar_Rpt_Proveedores(Dt_Consulta);
                }
                if (Chk_Fecha_Registro.Checked == true)
                {
                    if (!Verificar_Fecha(Txt_Fecha_Ini,Txt_Fecha_Fin))
                    {
                    }
                    else
                    {
                        //  se realiza la consulta por fecha 
                        Dt_Consulta = Realizar_Consulta();
                        
                        //  para generar el reporte de excel
                        Generar_Rpt_Proveedores(Dt_Consulta);
                    }
                }
                if (Chk_Fecha_Actualizacion.Checked == true)
                {
                    if (!Verificar_Fecha(Txt_Fecha_Ini_Act, Txt_Fecha_Fin_Act))
                    {
                    }
                    else
                    {
                        //  se realiza la consulta por fecha 
                        Dt_Consulta = Realizar_Consulta();
                        
                        //  para generar el reporte de excel
                        Generar_Rpt_Proveedores(Dt_Consulta);
                    }
                    
                }

            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
            throw new Exception(Ex.Message, Ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operacion actual que se este realizando
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  18/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
    }
    #endregion


    #region (CheckBox)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Partida_OnCheckedChanged
    ///DESCRIPCIÓN: muestra las cajas de texto de fecha y desabilita las de numero de contra recibo
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  18/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Partida_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //limpia los mensajes mostados
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            if (Chk_Partida.Checked == true)
            {
                //  para fecha
                Habilitar_Checked("Partida", true);
                //  para numero de contra recibo
                Chk_Compras_Otorgadas.Checked = false;
                Habilitar_Checked("Fecha", false);
                Txt_Fecha_Inicial.Text = "";
                Txt_Fecha_Final.Text = "";
                //para limpiar la opcion de por registro 
                Chk_Fecha_Registro.Checked = false;
                Habilitar_Checked("Registro", false);
                Habilitar_Checked("Actualizacion", false);
                Txt_Fecha_Ini.Text = "";
                Txt_Fecha_Fin.Text = "";
                Chk_Fecha_Actualizacion.Checked = false;
               

            }
            else
            {
                Habilitar_Checked("Partida", false);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
            throw new Exception("Error al generar el reporte de las Dependencias. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Partida_OnCheckedChanged
    ///DESCRIPCIÓN: muestra las cajas de texto de fecha y desabilita las de numero de contra recibo
    ///PARAMETROS: 
    ///CREO:        Susana Trigueros Armenta
    ///FECHA_CREO:  22/Ago/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Fecha_Registro_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //limpia los mensajes mostados
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            if (Chk_Fecha_Registro.Checked == true)
            {
                //  para fecha
                Habilitar_Checked("Registro", true);
                //  para limpiar por compras otorgadas
                Chk_Compras_Otorgadas.Checked = false;
                Habilitar_Checked("Fecha", false);
                Txt_Fecha_Inicial.Text = "";
                Txt_Fecha_Final.Text = "";
                //Para limpiar por Partida
                Chk_Partida.Checked = false;
                Habilitar_Checked("Partida", false);

                Habilitar_Checked("Actualizacion", false);
                
                Chk_Fecha_Actualizacion.Checked = false;
               
            }
            else
            {
                Habilitar_Checked("Registro", false);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
            throw new Exception("Error al generar el reporte de las Dependencias. Error: [" + Ex.Message + "]");
        }

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Compras_Otorgadas_OnCheckedChanged
    ///DESCRIPCIÓN: muestra las cajas de texto de fecha y desabilita las de numero de contra recibo
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  18/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Compras_Otorgadas_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //limpia los mensajes mostados
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            if (Chk_Compras_Otorgadas.Checked == true)
            {
                //  para fecha
                Habilitar_Checked("Fecha", true);
                //  para numero de contra recibo
                Chk_Partida.Checked = false;
                Chk_Fecha_Registro.Checked = false;
                Chk_Fecha_Actualizacion.Checked = false;
                Habilitar_Checked("Partida", false);
                Cmb_Partida.SelectedIndex = 0;
                Habilitar_Checked("Actualizacion", false);
                
                Habilitar_Checked("Registro", false);
            }
            else
            {
                Habilitar_Checked("Fecha", false);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
            throw new Exception("Error al generar el reporte de las Dependencias. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Fecha_Actualizacion_CheckedChanged
    ///DESCRIPCIÓN: muestra las cajas de texto de fecha de inicio y fin 
    ///PARAMETROS: 
    ///CREO:        Susana Trigueros Armenta 
    ///FECHA_CREO:  7/Nov/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Fecha_Actualizacion_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //limpia los mensajes mostados
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            if (Chk_Fecha_Actualizacion.Checked == true)
            {
               
                Chk_Partida.Checked = false;
                Habilitar_Checked("Actualizacion", true);
                Habilitar_Checked("Partida", false);
                Habilitar_Checked("Fecha", false);
                Habilitar_Checked("Registro", false);
                Cmb_Partida.SelectedIndex = 0;
                Chk_Compras_Otorgadas.Checked = false;
                Chk_Fecha_Registro.Checked = false;
                Chk_Partida.Checked = false;
            }
            else
            {
                Habilitar_Checked("Actualizacion", false);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
            throw new Exception("Error al generar el reporte de las Dependencias. Error: [" + Ex.Message + "]");
        }
    }
    #endregion
#endregion

}
