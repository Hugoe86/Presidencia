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
using Presidencia.Almacen_Reporte_Contrarecibos.Negocio;
using System.Text;

public partial class paginas_Almacen_Frm_Ope_Com_Rpt_Contra_Recibos_Documentos : System.Web.UI.Page
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
    /// FECHA_CREO  : 13/Enero/2012
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
    /// FECHA_CREO  : 13/Enero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Chk_Fechas.Checked = false;
            Chk_Numero.Checked = false;
            Chk_Rango_Numeros.Checked = false;
            Habilitar_Checked("Todo", false);
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
    /// FECHA_CREO  : 16/Enero/2012
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
                    //  para fechas
                    Lbl_Fecha_Inicial.Visible = false;
                    Txt_Fecha_Inicial.Visible = false;
                    Btn_Fecha_Inicial.Visible = false;
                    Lbl_Fecha_Final.Visible = false;
                    Txt_Fecha_Final.Visible = false;
                    Btn_Fecha_Final.Visible = false;
                    //  para numero de contra recibo
                    Lbl_Numero_Contrarecibo.Visible = false;
                    Txt_Numero_Contra_Recibo.Visible = false;
                    //  para rango de numeros de contra recibo
                    Lbl_Numero_Inicial.Visible = false;
                    Txt_Numero_Inicial.Visible = false;
                    Lbl_Numero_Final.Visible = false;
                    Txt_Numero_Final.Visible = false;
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
                case "Numero":
                    //para el numero de contra recibo
                    Lbl_Numero_Contrarecibo.Visible = Habilitado;
                    Txt_Numero_Contra_Recibo.Visible = Habilitado;
                    break;
                case "Rango":
                    //  para rango de numeros de contra recibo
                    Lbl_Numero_Inicial.Visible = Habilitado;
                    Txt_Numero_Inicial.Visible = Habilitado;
                    Lbl_Numero_Final.Visible = Habilitado;
                    Txt_Numero_Final.Visible = Habilitado;
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
    ///FECHA_CREO:           13/Enero/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public bool Verificar_Fecha()
    {
        DateTime Date1 = new DateTime();  //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
        DateTime Date2 = new DateTime();
        Boolean Fecha_Valida = true;

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if ((Txt_Fecha_Inicial.Text.Length != 0))
            {
                // Convertimos el Texto de los TextBox fecha a dateTime
                Date1 = DateTime.Parse(Txt_Fecha_Inicial.Text);
                Date2 = DateTime.Parse(Txt_Fecha_Final.Text);

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
    ///NOMBRE DE LA FUNCIÓN: Verificar_Rango
    ///DESCRIPCIÓN:          Metodo que permite generar la cadena del rango valida 
    ///PARAMETROS:   
    ///CREO:                 Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:           17/Enero/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public bool Verificar_Rango()
    {
        Int32 Numero_Inicial = 0;
        Int32 Numero_Final = 0;
        Boolean Fecha_Valida = true;

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if ((Txt_Numero_Inicial.Text.Length != 0))
            {
                // Convertimos el Texto de los TextBox fecha a dateTime
                Numero_Inicial = Convert.ToInt32 (Txt_Numero_Inicial.Text);
                Numero_Final = Convert.ToInt32(Txt_Numero_Final.Text);

                //Validamos que las fechas sean iguales o la final sea mayor que la inicial, de lo contrario se manda un mensaje de error 
                if ((Numero_Inicial < Numero_Final) || (Numero_Inicial == Numero_Final))
                {
                    Fecha_Valida = true;
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text += " El numero de contra recibo inicial no pude ser mayor que El numero de contra recibo final <br />";
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
    ///NOMBRE DE LA FUNCIÓN: Eliminar_Columnas
    ///DESCRIPCIÓN:          Metodo que Eliminara las columnas que no contengan informacion
    ///                      para que se muestren en el reporte
    ///PARAMETROS:           DataTable Dt_Consulta.-Es la consulta a la que se eliminaran las columnas 
    ///CREO:                 Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:           18/Enero/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public DataTable Eliminar_Columnas(DataTable Dt_Consulta)
    {
        Cls_Ope_Alm_Rpt_Contrarecibos_Negocio Consulta_Contra_Recibos = new Cls_Ope_Alm_Rpt_Contrarecibos_Negocio();
        Boolean Contiene_Informacion=false;
        Int32 Contador = 0;
        Int32 Contador_Columnas = 0;
        Int32 Contador_For = 0;
        String Numero_Columna = "";   
        DataTable Dt_Auxiliar = new DataTable();
        String[] Documentos;
        String[] Columnas_Eliminar;
        String Nombre = "";
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            //Dt_Auxiliar = Dt_Consulta;

            //  para sacar los nombres de las columnas
            Dt_Auxiliar = Consulta_Contra_Recibos.Consultar_Documentos();
            if (Dt_Auxiliar.Rows.Count > 0)
            {
                foreach (DataRow Registro in Dt_Auxiliar.Rows)
                {
                    Contador_Columnas++;
                    Nombre += (Registro[Cat_Com_Documentos.Campo_Nombre].ToString()) + ",";
                }
            }
            Documentos = Nombre.Split(',');
            Contador_Columnas += 4;
            //  para buscar si contiene informacion la tabla y poder eliminar las columnas que no se usan
            if (Dt_Consulta.Rows.Count > 0)
            {
                Contador = -1;
                for (Contador_For = 5; Contador_For <= Contador_Columnas; Contador_For++)
                {
                    Contador++;
                    foreach (DataRow Documentos_Enviados in Dt_Consulta.Rows)
                    {
                        //  si se encuentra lleno se indica que no se elimina 
                        if (!String.IsNullOrEmpty(Documentos_Enviados["" + Documentos[Contador]].ToString()))
                        {
                            Contiene_Informacion = true;
                            break;
                        }
                        else
                        {
                            Contiene_Informacion = false;
                        }
                    }
                    if (Contiene_Informacion==false)
                        Numero_Columna += "" + Contador_For + ",";

                }
            }
            Columnas_Eliminar = Numero_Columna.Split(',');
            //  eliminar las columnas que no se utilizan
            for (Contador_For = Columnas_Eliminar.Length-1; Contador_For >= 0; Contador_For--)
            {
                if (Columnas_Eliminar[Contador_For] == "")
                {
                }
                else
                {
                    Contador = Convert.ToInt32(Columnas_Eliminar[Contador_For]);
                    Dt_Consulta.Columns.RemoveAt(Contador - 1);
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
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN:          Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:           1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                      en caso de que cumpla la condicion del if
    ///CREO:                 Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:           13/Enero/2012
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
    ///FECHA_CREO:           13/Enero/2012
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
                Anio=Convert.ToInt32(aux[2]) ;
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
        //Concatenamos la fecha, y se cambia el orden a DD-MMM-YYYY para que sea una fecha valida para oracle
        Fecha_Valida = aux[0] + "/" + aux[1] + "/" + aux[2];
        return Fecha_Valida;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Generar_Rpt_Contra_Recibo
    /// DESCRIPCION :   Se encarga de generar el archivo de excel pasandole los paramentros
    ///                 al documento
    /// PARAMETROS  :   Dt_Reporte.- Es la consulta que se va a reportar
    /// CREO        :   Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  :   14/Enero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public void Generar_Rpt_Contra_Recibo(DataTable Dt_Reporte)
    {
        Cls_Ope_Alm_Rpt_Contrarecibos_Negocio Consulta_Documentos = new Cls_Ope_Alm_Rpt_Contrarecibos_Negocio();
        WorksheetCell Celda = new WorksheetCell();
        DataTable Dt_Consulta = new DataTable();
        DataTable Dt_Documentos = new DataTable();
        String Nombre_Archivo = "";
        String Ruta_Archivo = "";
        String Tipos_Documentos = "";
        String Informacion_Registro = "";
        String[] Documentos;
        Int32 Contador_Generar_Reporte= 0;
        Int32 Contador_For = 0;
        Int32 Contador_Documentos = 0;
        Double Suma_Importe = 0.0;
        Double Importe = 0.0;
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            Nombre_Archivo = "Rpt_Contra_Recibo_Documentos.xls";
            Ruta_Archivo = @Server.MapPath("../../Archivos/" + Nombre_Archivo);

            //  Creamos el libro de Excel.
            CarlosAg.ExcelXmlWriter.Workbook Libro = new CarlosAg.ExcelXmlWriter.Workbook();
            //  propiedades del libro
            Libro.Properties.Title = "Reporte_Contra_Recibos_Documentos";
            Libro.Properties.Created = DateTime.Now;
            Libro.Properties.Author = "Presidencia_";

            //  Creamos el estilo cabecera para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera = Libro.Styles.Add("HeaderStyle");
            //  Creamos el estilo cabecera 2 para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera2 = Libro.Styles.Add("HeaderStyle2");
            //  Creamos el estilo cabecera 3 para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Total_Texto = Libro.Styles.Add("Total_Texto");
            //  Creamos el estilo contenido para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Contenido = Libro.Styles.Add("BodyStyle");
            //  Creamos el estilo contenido del presupuesto para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Presupuesto = Libro.Styles.Add("Presupuesto");
            //  Creamos el estilo contenido del concepto para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Orientacion_Documentos = Libro.Styles.Add("Orientacion_Documentos");
            //  Creamos el estilo contenido del concepto para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Documentos = Libro.Styles.Add("Documentos");
            //  Creamos el estilo contenido del concepto para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Sin_Documentos = Libro.Styles.Add("Sin_Documentos");
            //  Creamos el estilo contenido del concepto para la hoja de excel.
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Presupuesto_Total = Libro.Styles.Add("Total");
            //Creamos el estilo contenido del concepto para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Concepto = Libro.Styles.Add("Concepto");


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

            //  estilo para la total texto
            Estilo_Total_Texto.Font.FontName = "Tahoma";
            Estilo_Total_Texto.Font.Size = 9;
            Estilo_Total_Texto.Font.Bold = true;
            Estilo_Total_Texto.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Total_Texto.Alignment.Vertical = StyleVerticalAlignment.JustifyDistributed;
            Estilo_Total_Texto.Alignment.Rotate = 0;
            Estilo_Total_Texto.Font.Color = "#000000";
            Estilo_Total_Texto.Interior.Color = "Yellow";
            Estilo_Total_Texto.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Total_Texto.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Total_Texto.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Total_Texto.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Total_Texto.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //estilo para el contenido
            Estilo_Contenido.Font.FontName = "Tahoma";
            Estilo_Contenido.Font.Size = 9;
            Estilo_Contenido.Font.Bold = false;
            Estilo_Contenido.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Contenido.Alignment.Vertical = StyleVerticalAlignment.Center;
            Estilo_Contenido.Alignment.Rotate = 0;
            Estilo_Contenido.Font.Color = "#000000";
            Estilo_Contenido.Interior.Color = "White";
            Estilo_Contenido.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Contenido.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //  estilo para el presupuesto (importe)
            Estilo_Presupuesto.Font.FontName = "Tahoma";
            Estilo_Presupuesto.Font.Size = 9;
            Estilo_Presupuesto.Font.Bold = false;
            Estilo_Presupuesto.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            Estilo_Presupuesto.Alignment.Vertical = StyleVerticalAlignment.Center;
            Estilo_Presupuesto.Alignment.Rotate = 0;
            Estilo_Presupuesto.Font.Color = "#000000";
            Estilo_Presupuesto.Interior.Color = "White";
            Estilo_Presupuesto.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Presupuesto.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Presupuesto.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Presupuesto.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Presupuesto.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //  estilo para el encabeza de documentos
            Estilo_Orientacion_Documentos.Font.FontName = "Tahoma";
            Estilo_Orientacion_Documentos.Font.Size = 8;
            Estilo_Orientacion_Documentos.Font.Bold = false;
            Estilo_Orientacion_Documentos.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Orientacion_Documentos.Alignment.Vertical = StyleVerticalAlignment.JustifyDistributed;
            Estilo_Orientacion_Documentos.Alignment.Rotate = 90;
            Estilo_Orientacion_Documentos.Font.Color = "#FFFFFF";
            Estilo_Orientacion_Documentos.Interior.Color = "DarkGray";
            Estilo_Orientacion_Documentos.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Orientacion_Documentos.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Orientacion_Documentos.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Orientacion_Documentos.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Orientacion_Documentos.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //  estilo para los documentos
            Estilo_Documentos.Font.FontName = "Comic Sans MS";
            Estilo_Documentos.Font.Size = 9;
            Estilo_Documentos.Font.Bold = false;
            Estilo_Documentos.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Documentos.Alignment.Vertical = StyleVerticalAlignment.Center;
            Estilo_Documentos.Alignment.Rotate = 0;
            Estilo_Documentos.Font.Color = "#000000";
            Estilo_Documentos.Interior.Color = "LightBlue";
            Estilo_Documentos.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Documentos.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Documentos.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Documentos.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Documentos.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //  estilo para los sin documentos
            Estilo_Sin_Documentos.Font.FontName = "Arial";
            Estilo_Sin_Documentos.Font.Size = 9;
            Estilo_Sin_Documentos.Font.Bold = true;
            Estilo_Sin_Documentos.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Sin_Documentos.Alignment.Vertical = StyleVerticalAlignment.Center;
            Estilo_Sin_Documentos.Alignment.Rotate = 0;
            Estilo_Sin_Documentos.Font.Color = "#000000";
            Estilo_Sin_Documentos.Interior.Color = "White";
            Estilo_Sin_Documentos.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Sin_Documentos.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Sin_Documentos.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Sin_Documentos.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Sin_Documentos.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

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
            Estilo_Concepto.Alignment.Horizontal = StyleHorizontalAlignment.Justify;
            Estilo_Concepto.Alignment.Vertical = StyleVerticalAlignment.JustifyDistributed;
            Estilo_Concepto.Font.Color = "#000000";
            Estilo_Concepto.Interior.Color = "White";
            Estilo_Concepto.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Concepto.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Concepto.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Concepto.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Concepto.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
            //*************************************** fin de los estilos***********************************************************

            //***************************************Inicio del reporte Contra recibos documentos Hoja 1***************************
            //  Creamos una hoja que tendrá el libro.
            CarlosAg.ExcelXmlWriter.Worksheet Hoja = Libro.Worksheets.Add("ContraRecibos Doc Enviados");
            //  Agregamos un renglón a la hoja de excel.
            CarlosAg.ExcelXmlWriter.WorksheetRow Renglon = Hoja.Table.Rows.Add();
            //  Agregamos las columnas que tendrá la hoja de excel.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));//  1 No_Contra Recibo.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));//  2 Numero de Factura.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(200));// 3 Proveedor.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));// 4 Importe
            
            //  para los documentos
            for (Contador_For = 5; Contador_For <= Dt_Reporte.Columns.Count; Contador_For++)
            {
                Contador_Generar_Reporte++;
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(40));
            }
            
            //  se llena el encabezado principal
            Renglon = Hoja.Table.Rows.Add();
            Celda = Renglon.Cells.Add("ALMACEN GENERAL");
            Celda.MergeAcross = 3; // Merge 4 cells together
            Celda.StyleID = "HeaderStyle";
            
            Renglon = Hoja.Table.Rows.Add();
            Celda = Renglon.Cells.Add("RELACION DE DOCUMENTOS ENVIADOS A CONTABILIDAD");
            Celda.MergeAcross = 3; // Merge 4 cells together
            Celda.StyleID = "HeaderStyle";

            Renglon = Hoja.Table.Rows.Add();
            Celda = Renglon.Cells.Add("IRAPUATO, GUANAJUATO, " + DateTime.Now);
            Celda.MergeAcross = 3; // Merge 4 cells together
            Celda.StyleID = "HeaderStyle";

            if (Chk_Fechas.Checked == true)
            {
                Renglon = Hoja.Table.Rows.Add();
                Celda = Renglon.Cells.Add("PERIODO DEL  " + Txt_Fecha_Inicial.Text.ToUpper() + 
                        "  AL  " + Txt_Fecha_Final.Text.ToUpper());
                Celda.MergeAcross = 3; // Merge 4 cells together
                Celda.StyleID = "HeaderStyle";
                Renglon = Hoja.Table.Rows.Add();
            }
            else if (Chk_Rango_Numeros.Checked == true)
            {
                Renglon = Hoja.Table.Rows.Add();
                Celda = Renglon.Cells.Add("NUMERO DE CONTRA RECIBO DEL  " + Txt_Numero_Inicial.Text.ToUpper() +
                        "  AL  " + Txt_Numero_Final.Text.ToUpper());
                Celda.MergeAcross = 3; // Merge 4 cells together
                Celda.StyleID = "HeaderStyle";
                Renglon = Hoja.Table.Rows.Add();
            }
            else
            {
                Renglon = Hoja.Table.Rows.Add();
            }
            Renglon = Hoja.Table.Rows.Add();
            //para el numero de CONTRA RECIBO
            Celda = Renglon.Cells.Add("NUMERO DE CONTRA RECIBO");
            Celda.MergeDown = 2; // Merge two cells together
            Celda.StyleID = "HeaderStyle2";
            //para el numero de factura
            Celda = Renglon.Cells.Add("FACTURA");
            Celda.MergeDown = 2; // Merge two cells together
            Celda.StyleID = "HeaderStyle2";
            //para el PROVEEDOR
            Celda = Renglon.Cells.Add("PROVEEDOR");
            Celda.MergeDown = 2; // Merge two cells together
            Celda.StyleID = "HeaderStyle2";
            //para el IMPORTE
            Celda = Renglon.Cells.Add("IMPORTE");
            Celda.MergeDown = 2; // Merge two cells together
            Celda.StyleID = "HeaderStyle2";

            // para los encabezados de los documentos
            for (Contador_For = 4; Contador_For < Dt_Reporte.Columns.Count; Contador_For++)
            {
                Contador_Documentos++;
                Celda = Renglon.Cells.Add("" + Dt_Reporte.Columns[Contador_For].ColumnName);
                Celda.MergeDown = 2; // Merge two cells together
                Celda.StyleID = "Orientacion_Documentos";
                Tipos_Documentos += Dt_Reporte.Columns[Contador_For].ColumnName + ",";
            }

            Documentos=Tipos_Documentos.Split(',');
            Renglon = Hoja.Table.Rows.Add();
            Renglon = Hoja.Table.Rows.Add();
            //  para le llenado del reporte con la informacion de la tabla

            if (Dt_Reporte.Rows.Count > 0)
            { 
                //  Se comienza a extraer la informaicon de la onsulta
                foreach (DataRow Renglon_Reporte in Dt_Reporte.Rows)
                {
                    Renglon = Hoja.Table.Rows.Add();

                    //  para numero de contra recibo
                    Informacion_Registro = (Renglon_Reporte["No_Contra_Recibo"].ToString());
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "BodyStyle"));

                    //  para el numero de factura
                    Informacion_Registro = (Renglon_Reporte["Numero_Factura"].ToString());
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "BodyStyle"));

                    //  para el proveedor
                    Informacion_Registro = (Renglon_Reporte["Nombre_Proveedor"].ToString());
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "Concepto"));

                    //  para el importe
                    Informacion_Registro = (Renglon_Reporte["Importe"].ToString());
                    Importe = Convert.ToDouble(Informacion_Registro);
                    Suma_Importe += Importe;
                    Informacion_Registro = String.Format("{0:n}", Importe);
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "Presupuesto"));

                    //  para los documentos
                    for (Contador_For = 0; Contador_For < Documentos.Length - 1; Contador_For++)
                    {
                        Informacion_Registro = (Renglon_Reporte["" + Documentos[Contador_For]].ToString().ToUpper());

                        if (Informacion_Registro == "X")
                            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Informacion_Registro, "Documentos"));

                        else
                            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Documentos"));
                    }

                }// fin foreach
                //  para el encabezado de la suma de los importes
                Renglon = Hoja.Table.Rows.Add();
                Celda = Renglon.Cells.Add("Total del Importe");
                Celda.MergeAcross = 2; // Merge 3 cells together
                Celda.StyleID = "Total_Texto";
                //  para la suma
                Informacion_Registro = String.Format("{0:n}", Suma_Importe);
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Informacion_Registro, "Total"));
                //para el resto
                Celda = Renglon.Cells.Add("");
                if (Contador_Documentos == 0)
                {
                }
                else
                {
                    Celda.MergeAcross = Contador_Documentos-1; // Merge 3 cells together
                    Celda.StyleID = "Total";
                }
                
            }// fin if

            else
            {
                Renglon = Hoja.Table.Rows.Add();
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Documentos"));
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Documentos"));
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Documentos"));
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Documentos"));

                if (Contador_Generar_Reporte == 0)
                {
                }
                else
                {
                    for (Contador_For = 0; Contador_For < Contador_Generar_Reporte; Contador_For++)
                    {
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("---", "Sin_Documentos"));
                    }
                }
            }

            //***************************************Fin del reporte Contra recibos documentos Hoja 1***************************

            ////  se guarda el documento
            //Libro.Save(Ruta_Archivo);
            ////  mostrar el archivo
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
    /// FECHA_CREO:          14/Enero/2012
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=300,height=100')", true);
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
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 16/Enero/2012
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

        if (Chk_Fechas.Checked == true)
        {
            if (Txt_Fecha_Inicial.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Selecione la fecha inicial.<br>";
                Datos_Validos = false;
            }
            if (Txt_Fecha_Inicial.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Selecione la fecha fecha.<br>";
                Datos_Validos = false;
            }
        }
        else if (Chk_Numero.Checked == true)
        {
            if (Txt_Numero_Contra_Recibo.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el numero de contra recibo.<br>";
                Datos_Validos = false;
            }
        }
        else if (Chk_Rango_Numeros.Checked == true)
        {
            if (Txt_Numero_Inicial.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el numero de contra recibo inicial.<br>";
                Datos_Validos = false;
            }
            if (Txt_Numero_Final.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el numero de contra recibo final.<br>";
                Datos_Validos = false;
            }
            if (!Verificar_Rango())
            {
                Datos_Validos = false;
            }
            else
            {

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
    ///FECHA_CREO:  14/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public DataTable Realizar_Consulta()
    {
        Cls_Ope_Alm_Rpt_Contrarecibos_Negocio Consulta_Contrarecibos = new Cls_Ope_Alm_Rpt_Contrarecibos_Negocio();
        DataTable Dt_Consulta = new DataTable();
        Boolean consulta = true;
        String Dia = "";
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Chk_Fechas.Checked == true)
            {
                if (!Verificar_Fecha())
                { 
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Consulta_Contrarecibos.P_Fecha_Inicial = null;
                    Consulta_Contrarecibos.P_Fecha_Final = null;
                    consulta = false;
                }
                else
                {
                    Consulta_Contrarecibos.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Inicial.Text.Trim());
                    Dia = Aumentar_Fecha(Txt_Fecha_Final.Text);
                    Consulta_Contrarecibos.P_Fecha_Final = Formato_Fecha(Dia);
                }
                if (consulta == true)
                {
                    Dt_Consulta = Consulta_Contrarecibos.Consultar_Contra_Recibos();
                }
            }
            //  para el numero de contra recibo a buscar
            else if (Chk_Numero.Checked == true)
            {
                Consulta_Contrarecibos.P_No_Contra_Recibo = Txt_Numero_Contra_Recibo.Text;
                //  realizar consulta de numero de contra recibo
                Dt_Consulta = Consulta_Contrarecibos.Consultar_Numero_Contra_Recibos();
            }
            //  para el reango de numeros de contra recibo
            else if (Chk_Rango_Numeros.Checked == true)
            {
                if (!Verificar_Rango())
                {
                    Consulta_Contrarecibos.P_No_Contra_Recibo_Inicial = null;
                    Consulta_Contrarecibos.P_No_Contra_Recibo_Final = null;
                    consulta = false;
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Consulta_Contrarecibos.P_No_Contra_Recibo_Inicial = Txt_Numero_Inicial.Text;
                    Consulta_Contrarecibos.P_No_Contra_Recibo_Final = Txt_Numero_Final.Text;
                }
                if (consulta == true)
                {
                    Dt_Consulta = Consulta_Contrarecibos.Consultar_Numero_Contra_Recibos();
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
    ///NOMBRE DE LA FUNCIÓN: Realizar_Consulta_Soporte
    ///DESCRIPCIÓN: Realizara la consulta de lo que se quiere imprimir
    ///PARAMETROS:  DataTable.- Contiene la consulta realizada por fecha o numero de contra recibo
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  14/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public DataTable Realizar_Consulta_Soporte(DataTable Dt_Consulta)
    {
        Cls_Ope_Alm_Rpt_Contrarecibos_Negocio Consulta_Contra_Recibos = new Cls_Ope_Alm_Rpt_Contrarecibos_Negocio();
        DataTable Dt_Reporte_Contra_Recibos = new DataTable();
        DataTable Dt_Nombre = new DataTable();
        DataTable Dt_Documentos = new DataTable();
        Double Numerico = 0.0;
        Int32 Contador = 0;
        Int32 Contador_For = 0;
        String Tipos_Documentos = "";
        String Nombre = "";
        String[] Documentos;
        String[] Nombre_Documentos;
        String Numero_Contrarecibo_Doc = "";
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            Dt_Reporte_Contra_Recibos.Columns.Add("No_Contra_Recibo");
            Dt_Reporte_Contra_Recibos.Columns.Add("Numero_Factura");
            Dt_Reporte_Contra_Recibos.Columns.Add("Nombre_Proveedor");
            Dt_Reporte_Contra_Recibos.Columns.Add("Importe");
            DataRow Dr_Registro;

            Dt_Documentos = Consulta_Contra_Recibos.Consultar_Documentos();

            //  para los tipos de documentos y asignar las columnas a la tabla
            if (Dt_Documentos.Rows.Count > 0)
            {
                Contador = 0;
                foreach (DataRow Registro in Dt_Documentos.Rows)
                {
                    Contador++;
                    Dt_Reporte_Contra_Recibos.Columns.Add("" + (Registro[Cat_Com_Documentos.Campo_Nombre].ToString().ToUpper()));
                    Tipos_Documentos += (Registro[Cat_Com_Documentos.Campo_Documento_ID].ToString().ToUpper()) + ",";
                    Nombre += (Registro[Cat_Com_Documentos.Campo_Nombre].ToString().ToUpper()) + ",";
                }
            }
            Documentos = Tipos_Documentos.Split(',');
            Nombre_Documentos = Nombre.Split(',');

            if (Dt_Consulta.Rows.Count > 0)
            {
                foreach (DataRow Registro in Dt_Consulta.Rows)
                {
                    Dr_Registro = Dt_Reporte_Contra_Recibos.NewRow();
                    //  para el numero de contra recibo
                    if (!String.IsNullOrEmpty(Registro[Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno].ToString()))
                    {
                        Dr_Registro["No_Contra_Recibo"] = (Registro[Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno].ToString());
                        Tipos_Documentos=(Registro[Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno].ToString());
                        Numero_Contrarecibo_Doc = Tipos_Documentos; //  almaceno el numero de contra recibo para luego realizar consulta
                    }
                    //  para el numero de factura
                    if (!String.IsNullOrEmpty(Registro[Ope_Com_Facturas_Proveedores.Campo_No_Factura_Proveedor].ToString()))
                    {
                        Dr_Registro["Numero_Factura"] = (Registro[Ope_Com_Facturas_Proveedores.Campo_No_Factura_Proveedor].ToString());
                    }
                    //  pare el nombre del porveedor
                    if (!String.IsNullOrEmpty(Registro[Ope_Com_Facturas_Proveedores.Campo_Proveedor_ID].ToString()))
                    {
                        Consulta_Contra_Recibos.P_Fecha_Inicial = null;
                        Consulta_Contra_Recibos.P_Fecha_Final = null;
                        Consulta_Contra_Recibos.P_Proveedor_ID = (Registro[Ope_Com_Facturas_Proveedores.Campo_Proveedor_ID].ToString());
                        Dt_Nombre = Consulta_Contra_Recibos.Consultar_Nombre_Proveedor();
                        foreach (DataRow Registro_Proveedor in Dt_Nombre.Rows)
                        {
                            Dr_Registro["Nombre_Proveedor"] = (Registro_Proveedor[Cat_Com_Proveedores.Campo_Nombre].ToString());
                        }
                    }
                    //  para el importe
                    if (!String.IsNullOrEmpty(Registro[Ope_Com_Facturas_Proveedores.Campo_Total].ToString()))
                    {
                        String Importe = (Registro[Ope_Com_Facturas_Proveedores.Campo_Total].ToString());
                        Numerico = Convert.ToDouble(Importe);
                        Importe = String.Format("{0:n}", Numerico);
                        Dr_Registro["Importe"] = Importe;
                    }

                    Consulta_Contra_Recibos.P_Documento_ID = Numero_Contrarecibo_Doc;
                    Dt_Documentos = Consulta_Contra_Recibos.Consultar_Documentos_Soporte();

                    if (Dt_Documentos.Rows.Count > 0)
                    {
                        Contador = -1;
                        Contador_For = 0;
                        foreach (DataRow Documentos_Enviados in Dt_Documentos.Rows)
                        {
                            for (Contador_For = Contador; Contador_For < Documentos.Length-1; Contador_For++)
                            {
                                Contador++;
                                if (Documentos[Contador] == (Documentos_Enviados["DOCUMENTO_ID"].ToString()))
                                {
                                    Dr_Registro["" + Nombre_Documentos[Contador]] = "X";
                                    //Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("X", "Documentos"));
                                    break;
                                }
                                
                                
                            }
                        }
                        
                    }
                    Dt_Reporte_Contra_Recibos.Rows.Add(Dr_Registro);
                }
            }
            else 
            { 
                //se llena la tabla con espacios
            }
            return Dt_Reporte_Contra_Recibos;
            
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
    ///FECHA_CREO:  13/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Imprimir_Excel_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Consulta = new DataTable();
        DataTable Dt_Generar_Tabla = new DataTable();
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (Validar_Reporte())
            {
                if (Chk_Fechas.Checked == true)
                {
                    if (!Verificar_Fecha())
                    {
                    }
                    else
                    {
                        //  se realiza la consulta por fecha 
                        Dt_Consulta = Realizar_Consulta();
                        //  va el codigo para buscar los documentos
                        Dt_Generar_Tabla = Realizar_Consulta_Soporte(Dt_Consulta);
                        //  eliminar las columnas que no tengan informacion
                        Dt_Generar_Tabla = Eliminar_Columnas(Dt_Generar_Tabla);
                        //  para generar el reporte de excel
                        Generar_Rpt_Contra_Recibo(Dt_Generar_Tabla);
                    }
                }
                else if (Chk_Numero.Checked == true)
                {
                    //  se realiza la consulta por numero de contra recibo
                    Dt_Consulta = Realizar_Consulta();
                    //  va el codigo para buscar los documentos
                    Dt_Generar_Tabla = Realizar_Consulta_Soporte(Dt_Consulta);
                    //  eliminar las columnas que no tengan informacion
                    Dt_Generar_Tabla = Eliminar_Columnas(Dt_Generar_Tabla);
                    //  para generar el reporte de excel
                    Generar_Rpt_Contra_Recibo(Dt_Generar_Tabla);
                }
                else if (Chk_Rango_Numeros.Checked == true)
                {
                    //  se realiza la consulta por numero de contra recibo
                    Dt_Consulta = Realizar_Consulta();
                    //  va el codigo para buscar los documentos
                    Dt_Generar_Tabla = Realizar_Consulta_Soporte(Dt_Consulta);
                    //  eliminar las columnas que no tengan informacion
                    Dt_Generar_Tabla = Eliminar_Columnas(Dt_Generar_Tabla);
                    //  para generar el reporte de excel
                    Generar_Rpt_Contra_Recibo(Dt_Generar_Tabla);
                }
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }//else de validar_Datos
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
    ///FECHA_CREO:  13/Enero/2012
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
    ///NOMBRE DE LA FUNCIÓN: Chk_Fechas_OnCheckedChanged
    ///DESCRIPCIÓN: muestra las cajas de texto de fecha y desabilita las de numero de contra recibo
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  13/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Fechas_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //limpia los mensajes mostados
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            if (Chk_Fechas.Checked == true)
            {
                //  para fecha
                Habilitar_Checked("Fecha", true);
                //  para numero de contra recibo
                Chk_Numero.Checked = false;
                Habilitar_Checked("Numero", false);
                Txt_Numero_Contra_Recibo.Text = "";
                //  para el rango de numero de contra recibo
                Chk_Rango_Numeros.Checked = false;
                Habilitar_Checked("Rango", false);
                Txt_Numero_Inicial.Text = "";
                Txt_Numero_Final.Text = "";
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
    ///NOMBRE DE LA FUNCIÓN: Chk_Numero_OnCheckedChanged
    ///DESCRIPCIÓN:  muestra las cajas de texto de nuemro de contra recibo y desabilita las de fecha
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  13/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Numero_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //limpia los mensajes mostados
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            if (Chk_Numero.Checked == true)
            {
                //  para el numero de contra recibo
                Habilitar_Checked("Numero", true);
                //  para la fecha
                Habilitar_Checked("Fecha", false);
                Chk_Fechas.Checked = false;
                Txt_Fecha_Inicial.Text = "";
                Txt_Fecha_Final.Text = "";
                //  para el rango de numero de contra recibo
                Habilitar_Checked("Rango", false);
                Chk_Rango_Numeros.Checked = false;
                Txt_Numero_Inicial.Text = "";
                Txt_Numero_Final.Text = "";
            }
            else
            {
                Habilitar_Checked("Numero", false);
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
    ///NOMBRE DE LA FUNCIÓN: Chk_Rango_Numeros_OnCheckedChanged
    ///DESCRIPCIÓN:  muestra las cajas de texto de nuemro de contra recibo incial y final,
    ///              desabilita las de fecha y la de numero de contra recibo
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  17/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Rango_Numeros_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //limpia los mensajes mostados
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            if (Chk_Rango_Numeros.Checked == true)
            {
                //  para el rango de numero de contra recibo
                Habilitar_Checked("Rango", true);
                //  para la fecha
                Habilitar_Checked("Fecha", false);
                Chk_Fechas.Checked = false;
                Txt_Fecha_Inicial.Text = "";
                Txt_Fecha_Final.Text = "";
                //  para el numero de contra recibo
                Habilitar_Checked("Numero", false);
                Chk_Numero.Checked = false;
                Txt_Numero_Contra_Recibo.Text = "";
            }
            else
            {
                Habilitar_Checked("Rango", false);
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
