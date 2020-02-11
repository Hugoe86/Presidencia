using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Dependencias.Negocios;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Sessiones;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Prestamos.Negocio;
using Presidencia.Reportes_Nomina_Control_Archivos_Dispersion.Negocio;
using CarlosAg.ExcelXmlWriter;

public partial class paginas_Nomina_Reporte_Frm_Rpt_Nom_Control_Archivos_Dispersion : System.Web.UI.Page
{
    #region (Load/Init)
    /// *************************************************************************************
    /// NOMBRE: Page_Load
    /// 
    /// DESCRIPCIÓN: Habilita la configuración inicial de la página.
    ///              
    /// PARÁMETROS: No Aplica
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 18:25 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Estado_Inicial();
            }

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    #endregion

    #region (Métodos)

    #region (Métodos Generales)
    /// *************************************************************************************
    /// NOMBRE: Estado_Inicial
    /// 
    /// DESCRIPCIÓN: Método que carga y habilita los controles a un estado inicial
    ///              para comenzar las operaciones.
    ///              
    /// PARÁMETROS: No Aplica
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 11:34 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Estado_Inicial()
    {
        try
        {
            Consultar_Calendarios_Nomina();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado al habilitar el estado inicial de los controles de la página. Error: [" + Ex.Message + "]");
        }
    }
    #endregion (Métodos Generales)

    #region (Consultar)

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Calendarios_Nomina
    /// DESCRIPCION : 
    /// 
    /// PARAMETROS:
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Calendarios_Nomina()
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nominales = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Calendarios_Nominales = null;//Variable que almacena los calendarios nominales que existén actualmente en el sistema.
        try
        {
            Dt_Calendarios_Nominales = Obj_Calendario_Nominales.Consultar_Calendario_Nominas();
            Dt_Calendarios_Nominales = Formato_Fecha_Calendario_Nomina(Dt_Calendarios_Nominales);

            if (Dt_Calendarios_Nominales is DataTable)
            {
                Cmb_Calendario_Nomina.DataSource = Dt_Calendarios_Nominales;
                Cmb_Calendario_Nomina.DataTextField = "Nomina";
                Cmb_Calendario_Nomina.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
                Cmb_Calendario_Nomina.DataBind();
                Cmb_Calendario_Nomina.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
                Cmb_Calendario_Nomina.SelectedIndex = -1;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los calendarios de nómina que existen actualmente registrados en el sistema. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Formato_Fecha_Calendario_Nomina
    /// DESCRIPCION : Crea el DataTable con la consulta de las nomina vigentes en el 
    /// sistema.
    /// PARAMETROS: Dt_Calendario_Nominas.- Lista de las nominas vigentes actualmente 
    ///             en el sistema.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Formato_Fecha_Calendario_Nomina(DataTable Dt_Calendario_Nominas)
    {
        DataTable Dt_Nominas = new DataTable();
        DataRow Renglon_Dt_Clon = null;
        Dt_Nominas.Columns.Add("Nomina", typeof(System.String));
        Dt_Nominas.Columns.Add(Cat_Nom_Calendario_Nominas.Campo_Nomina_ID, typeof(System.String));

        if (Dt_Calendario_Nominas is DataTable)
        {
            foreach (DataRow Renglon in Dt_Calendario_Nominas.Rows)
            {
                if (Renglon is DataRow)
                {
                    Renglon_Dt_Clon = Dt_Nominas.NewRow();
                    Renglon_Dt_Clon["Nomina"] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin].ToString().Split(new char[] { ' ' })[0].Split(new char[] { '/' })[2];
                    Renglon_Dt_Clon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID];
                    Dt_Nominas.Rows.Add(Renglon_Dt_Clon);
                }
            }
        }
        return Dt_Nominas;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Periodos_Catorcenales_Nomina
    ///DESCRIPCIÓN: Consulta los periodos catorcenales para el 
    ///calendario de nomina seleccionado.
    ///PARAMETROS: Nomina_ID.- Indica el calendario de nomina del cuál se desea consultar
    ///                        los periodos catorcenales.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Periodos_Catorcenales_Nomina(String Nomina_ID)
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nomina_Periodos = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Clase de conexion con la capa de negocios
        DataTable Dt_Periodos_Catorcenales = null;//Variable que almacenra unaa lista de los periodos catorcenales que le correspónden a la nomina seleccionada.

        try
        {
            Consulta_Calendario_Nomina_Periodos.P_Nomina_ID = Nomina_ID;
            Dt_Periodos_Catorcenales = Consulta_Calendario_Nomina_Periodos.Consulta_Detalles_Nomina();
            if (Dt_Periodos_Catorcenales != null)
            {
                if (Dt_Periodos_Catorcenales.Rows.Count > 0)
                {
                    Cmb_Periodos_Catorcenales_Nomina.DataSource = Dt_Periodos_Catorcenales;
                    Cmb_Periodos_Catorcenales_Nomina.DataTextField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodos_Catorcenales_Nomina.DataValueField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodos_Catorcenales_Nomina.DataBind();
                    Cmb_Periodos_Catorcenales_Nomina.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;

                    Validar_Periodos_Pago(Cmb_Periodos_Catorcenales_Nomina);
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron periodos catorcenales para la nomina seleccionada.";
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los periodos catorcenales del  calendario de nomina seleccionado. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Consultar_Depositos
    /// DESCRIPCIÓN: Llama al método en la clase de negocios que ejecuta la consulta con los montos para el reporte
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 11-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataTable Consultar_Depositos()
    {
        var Consulta_Depositos = new Cls_Rpt_Nom_Control_Archivos_Dispersion_Negocio();
        DataTable Dt_Depositos;

        Consulta_Depositos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue;
        Consulta_Depositos.P_Numero_Nomina = Cmb_Periodos_Catorcenales_Nomina.SelectedValue;
        Dt_Depositos = Consulta_Depositos.Consultar_Depositos_Tipo_Nomina();

        return Dt_Depositos;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Agregar_Todas_Nominas
    /// DESCRIPCIÓN: Recorre la tabla que se recibe como parámetro y agrega registros 
    /// 	            para que todos los bancos tengan todos los tipos de nómina
    /// PARÁMETROS:
    ///             1. Dt_Depositos: tabla que se va a validar que tenga todos los tipos de nomina
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 11-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataTable Agregar_Todas_Nominas(DataTable Dt_Depositos)
    {
        List<string> Lista_Tipos_Nomina = new List<string>() { "1", "2", "3", "4", "5", "9" };
        List<string> Lista_Bancos = new List<string>();
        string Nombre_Banco;
        DataRow Dr_Nuevo_Registro;
        Dictionary<string, string> Dic_Nomina_ID = new Dictionary<string, string>();
        DataTable Dt_Nuevos_Depositos;
        DataTable Dt_Depositos_Banco;
        bool Bnd_Registro_Agregado;

        // recorrer la tabla para obtener los tipo de nomina para almacenar en el diccionario (id-nombre) y obtener una lista de bancos en el archivo
        foreach (DataRow Deposito in Dt_Depositos.Rows)
        {
            // si el diccionario no tiene el tipo de nómina, agregarlo
            if (!Dic_Nomina_ID.ContainsKey(Deposito[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString()))
            {
                Dic_Nomina_ID.Add(Deposito[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString(), Deposito[Cat_Nom_Tipos_Nominas.Campo_Nomina].ToString());
            }
            Nombre_Banco = Deposito[Cat_Nom_Bancos.Campo_Nombre].ToString();
            // si el registro tiene nombre del banco y aún no está en la lista de bancos, agregarlo
            if (!string.IsNullOrEmpty(Nombre_Banco) && !Lista_Bancos.Contains(Nombre_Banco))
            {
                Lista_Bancos.Add(Nombre_Banco);
            }
        }

        // copiar la estructura de la tabla depositos en la nueva tabla
        Dt_Nuevos_Depositos = Dt_Depositos.Clone();

        // agregar cada banco en Lista_Bancos a la nueva tabla depositos
        foreach (string Banco in Lista_Bancos)
        {
            // obtener un datatable solo con los registros del Banco actual
            Dt_Depositos_Banco = (from Fila in Dt_Depositos.AsEnumerable()
                                  where Fila.Field<string>(Cat_Nom_Bancos.Campo_Nombre) == Banco
                                  select Fila).AsDataView().ToTable();

            // recorrer todos los tipos de nomina en Lista_Tipos_Nomina
            foreach (string Nomina_ID in Lista_Tipos_Nomina)
            {
                Bnd_Registro_Agregado = false;
                // buscar en la tabla Dt_Depositos_Banco el tipo de nomina
                for (int i = 0; i < Dt_Depositos_Banco.Rows.Count; i++)
                {
                    // si se encuentra el tipo de nomina actual (Nomina_ID) en la tabla, copiar el registro a la tabla Dt_Nuevos_Depositos y pasar al siguiente tipo de nomina
                    if (Dt_Depositos_Banco.Rows[i][Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString() == Nomina_ID)
                    {
                        Dt_Nuevos_Depositos.ImportRow(Dt_Depositos_Banco.Rows[i]);
                        Bnd_Registro_Agregado = true;
                        break;
                    }
                }

                // si no se encontró registro para el tipo de nomina en la tabla, agregar una fila a la tabla
                if (Bnd_Registro_Agregado == false)
                {
                    Dr_Nuevo_Registro = Dt_Nuevos_Depositos.NewRow();
                    Dr_Nuevo_Registro[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID] = Nomina_ID;
                    if (Dic_Nomina_ID.ContainsKey(Nomina_ID))
                    {
                        Dr_Nuevo_Registro[Cat_Nom_Tipos_Nominas.Campo_Nomina] = Dic_Nomina_ID[Nomina_ID];
                    }
                    Dr_Nuevo_Registro["CANTIDAD_DEPOSITOS"] = 0;
                    Dr_Nuevo_Registro[Ope_Nom_Recibos_Empleados.Campo_Total_Percepciones] = 0;
                    Dr_Nuevo_Registro[Cat_Nom_Bancos.Campo_Nombre] = Banco;
                    Dr_Nuevo_Registro["TIPO_PAGO"] = "DEPOSITO";
                    Dt_Nuevos_Depositos.Rows.Add(Dr_Nuevo_Registro);
                }
            }
        }

        // agregar registros para la forma de pago EFECTIVO
        // obtener un datatable solo con los registros de forma de pago EFECTIVO
        Dt_Depositos_Banco = (from Fila in Dt_Depositos.AsEnumerable()
                              where Fila.Field<string>("TIPO_PAGO") == "EFECTIVO"
                              select Fila).AsDataView().ToTable();

        // recorrer todos los tipos de nomina en Lista_Tipos_Nomina
        foreach (string Nomina_ID in Lista_Tipos_Nomina)
        {
            Bnd_Registro_Agregado = false;
            // buscar en la tabla Dt_Depositos_Banco el tipo de nomina
            for (int i = 0; i < Dt_Depositos_Banco.Rows.Count; i++)
            {
                // si se encuentra el tipo de nomina actual (Nomina_ID) en la tabla, copiar el registro a la tabla Dt_Nuevos_Depositos y pasar al siguiente tipo de nomina
                if (Dt_Depositos_Banco.Rows[i][Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString() == Nomina_ID)
                {
                    Dt_Nuevos_Depositos.ImportRow(Dt_Depositos_Banco.Rows[i]);
                    Bnd_Registro_Agregado = true;
                    break;
                }
            }

            // si no se encontró registro para el tipo de nomina en la tabla, agregar una fila a la tabla
            if (Bnd_Registro_Agregado == false)
            {
                Dr_Nuevo_Registro = Dt_Nuevos_Depositos.NewRow();
                Dr_Nuevo_Registro[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID] = Nomina_ID;
                if (Dic_Nomina_ID.ContainsKey(Nomina_ID))
                {
                    Dr_Nuevo_Registro[Cat_Nom_Tipos_Nominas.Campo_Nomina] = Dic_Nomina_ID[Nomina_ID];
                }
                Dr_Nuevo_Registro["CANTIDAD_DEPOSITOS"] = 0;
                Dr_Nuevo_Registro[Ope_Nom_Recibos_Empleados.Campo_Total_Percepciones] = 0;
                Dr_Nuevo_Registro["TIPO_PAGO"] = "EFECTIVO";
                Dt_Nuevos_Depositos.Rows.Add(Dr_Nuevo_Registro);
            }
        }

        return Dt_Nuevos_Depositos;
    }

    #endregion (Consultar)

    #region (Metodos Validacion)

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Periodos_Pago
    ///DESCRIPCIÓN: Valida que el empleado solo puedan comenzar a descontar la deduccion 
    ///a partir del periodo actual.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Validar_Periodos_Pago(DropDownList Combo)
    {
        Cls_Ope_Nom_Pestamos_Negocio Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
        DateTime Fecha_Actual = DateTime.Now;
        DateTime Fecha_Inicio = new DateTime();
        DateTime Fecha_Fin = new DateTime();

        Prestamos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();

        foreach (ListItem Elemento in Combo.Items)
        {
            if (IsNumeric(Elemento.Text.Trim()))
            {
                Prestamos.P_No_Nomina = Convert.ToInt32(Elemento.Text.Trim());
                Dt_Detalles_Nomina = Prestamos.Consultar_Fechas_Periodo();

                if (Dt_Detalles_Nomina != null)
                {
                    if (Dt_Detalles_Nomina.Rows.Count > 0)
                    {
                        Fecha_Inicio = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString());
                        Fecha_Fin = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString());
                    }
                }
            }
        }
    }

    /// *************************************************************************************
    /// NOMBRE: Validar_Datos_Formaulario
    /// 
    /// DESCRIPCIÓN: Valida que se hallan ingresado los datos requeridos para realizar la 
    ///              operación.
    ///              
    /// PARÁMETROS: No Aplicá
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 11:34 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected Boolean Validar_Datos_Formaulario()
    {
        Lbl_Mensaje_Error.Text = "Es necesario: <br />";
        Boolean Es_Valido = true;
        DateTime Fecha;
        DateTime.TryParse(Txt_Fecha.Text.Trim(), out Fecha);

        try
        {
            if (Cmb_Calendario_Nomina.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;+ Seleccionar el año de la nómina.<br />";
                Es_Valido = false;
            }

            if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;+ Seleccionar el Periodo. <br />";
                Es_Valido = false;
            }
            if (Fecha == DateTime.MinValue)
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;+ Especificar la Fecha para el reporte. <br />";
                Es_Valido = false;
            }

        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar la información del formulario. Error: [" + Ex.Message + "]");
        }
        return Es_Valido;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Mayo/2011 12:20p.m.
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean IsNumeric(String Cadena)
    {
        Boolean Resultado = true;
        Char[] Array = Cadena.ToCharArray();
        try
        {
            for (int index = 0; index < Array.Length; index++)
            {
                if (!Char.IsDigit(Array[index])) return false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Validar si es un dato numerico. Error [" + Ex.Message + "]");
        }
        return Resultado;
    }

    #endregion (Metodos Validacion)

    #region (Reportes)

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
        catch (System.Threading.ThreadAbortException)
        //// Response.End(); siempre genera una excepción (http://support.microsoft.com/kb/312629/EN-US/)
        { }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte en excel. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Generar_Reporte_Excel
    /// 	DESCRIPCIÓN: Regresa un libro de Excel con los datos en la tabla que recibe como parámetro
    /// 	PARÁMETROS:
    /// 		1. Dt_Depositos: tabla con los datos de los depositos que contendrá el archivo
    /// 		2. Fecha: fecha que va a aparecer en el archivo
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 11-abr-2012
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public static CarlosAg.ExcelXmlWriter.Workbook Generar_Reporte_Excel(DataTable Dt_Depositos, DateTime Fecha)
    {
        //Creamos el libro de Excel.
        CarlosAg.ExcelXmlWriter.Workbook Libro = new CarlosAg.ExcelXmlWriter.Workbook();
        string Nombre_Banco;
        string Nombre_Banco_Anterior = "";
        string Nombre_Nomina;
        string Tipo_Nomina_ID;
        string Forma_Pago;
        int Total_Depositos_Banco = 0;
        int Numero_Depositos;
        int Gran_Total_Depositos = 0;
        decimal Importe;
        decimal Total_Importe_Banco = 0;
        decimal Gran_Total_Importe = 0;
        Dictionary<string, decimal> Dic_Nomina_Total_Deposito = new Dictionary<string, decimal>();  // nombre nomia-total nomina
        Dictionary<string, string> Dic_Tipo_Nomina = new Dictionary<string, string>(); // tipo de nomina-nombre nomina

        try
        {
            Libro.Properties.Title = "Reporte de Archivos de dispersión";
            Libro.Properties.Created = DateTime.Now;
            Libro.Properties.Author = "Presidencia_RH";

            //Creamos una hoja que tendrá el libro.
            CarlosAg.ExcelXmlWriter.Worksheet Hoja = Libro.Worksheets.Add("Archivos_Dispersión");
            //Agregamos un renglón a la hoja de excel.
            CarlosAg.ExcelXmlWriter.WorksheetRow Renglon = Hoja.Table.Rows.Add();
            // estilos para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera = Libro.Styles.Add("HeaderStyle");
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Contenido = Libro.Styles.Add("BodyStyle");
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Subtitulo = Libro.Styles.Add("Subtitulo");
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Numeros = Libro.Styles.Add("Numeros");
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Totales = Libro.Styles.Add("Totales");

            // estilo centrado con negritas
            Estilo_Cabecera.Font.FontName = "Tahoma";
            Estilo_Cabecera.Font.Size = 8;
            Estilo_Cabecera.Font.Bold = true;
            Estilo_Cabecera.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Cabecera.Font.Color = "Black";
            Estilo_Cabecera.Interior.Color = "White";
            Estilo_Cabecera.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Cabecera.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "LightGray");
            Estilo_Cabecera.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "LightGray");
            Estilo_Cabecera.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "LightGray");
            Estilo_Cabecera.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "LightGray");

            // estilo alineado a la izquierda
            Estilo_Contenido.Font.FontName = "Tahoma";
            Estilo_Contenido.Font.Size = 8;
            Estilo_Contenido.Font.Bold = false;
            Estilo_Contenido.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            Estilo_Contenido.Font.Color = "#000000";
            Estilo_Contenido.Interior.Color = "White";
            Estilo_Contenido.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Contenido.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "LightGray");
            Estilo_Contenido.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "LightGray");
            Estilo_Contenido.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "LightGray");
            Estilo_Contenido.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "LightGray");

            // estilo alineado a la izquierda con negritas
            Estilo_Subtitulo.Font.FontName = "Tahoma";
            Estilo_Subtitulo.Font.Size = 8;
            Estilo_Subtitulo.Font.Bold = true;
            Estilo_Subtitulo.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            Estilo_Subtitulo.Font.Color = "Black";
            Estilo_Subtitulo.Interior.Color = "White";
            Estilo_Subtitulo.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Subtitulo.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "LightGray");
            Estilo_Subtitulo.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "LightGray");
            Estilo_Subtitulo.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "LightGray");
            Estilo_Subtitulo.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "LightGray");

            // estilo alineado a al derecha
            Estilo_Numeros.Font.FontName = "Tahoma";
            Estilo_Numeros.Font.Size = 8;
            Estilo_Numeros.Font.Bold = false;
            Estilo_Numeros.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            Estilo_Numeros.Font.Color = "#000000";
            Estilo_Numeros.Interior.Color = "White";
            Estilo_Numeros.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Numeros.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "LightGray");
            Estilo_Numeros.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "LightGray");
            Estilo_Numeros.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "LightGray");
            Estilo_Numeros.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "LightGray");

            // estilo alineado a la derecha con negritas
            Estilo_Totales.Font.FontName = "Tahoma";
            Estilo_Totales.Font.Size = 8;
            Estilo_Totales.Font.Bold = true;
            Estilo_Totales.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            Estilo_Totales.Font.Color = "Black";
            Estilo_Totales.Interior.Color = "White";
            Estilo_Totales.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Totales.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "LightGray");
            Estilo_Totales.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "LightGray");
            Estilo_Totales.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "LightGray");
            Estilo_Totales.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "LightGray");

            if (Dt_Depositos is System.Data.DataTable && Dt_Depositos != null)
            {
                if (Dt_Depositos.Rows.Count > 0)
                {
                    // agregar columnas
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(170));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Fecha.ToString("dd/MM/yyyy"), "Totales"));
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(65));
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(65));
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(15));
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));

                    // agregar filas del datatable al libro
                    foreach (System.Data.DataRow FILA in Dt_Depositos.Rows)
                    {
                        if (FILA is System.Data.DataRow)
                        {
                            Renglon = Hoja.Table.Rows.Add();

                            // leer datos
                            Nombre_Banco = FILA[Cat_Nom_Bancos.Campo_Nombre].ToString();
                            Forma_Pago = FILA["TIPO_PAGO"].ToString();
                            // si el banco no tiene nombre y la forma de pago es DEPOSITO, pasar al siguiente registro
                            if (Nombre_Banco == "" && Forma_Pago == "DEPOSITO")
                            {
                                continue;
                            }
                            Nombre_Nomina = FILA[Cat_Nom_Tipos_Nominas.Campo_Nomina].ToString();
                            Tipo_Nomina_ID = FILA[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString();
                            // cantidad de depositos del banco por tipo de nomina
                            int.TryParse(FILA["CANTIDAD_DEPOSITOS"].ToString(), out Numero_Depositos);
                            // importe 
                            decimal.TryParse(FILA[Ope_Nom_Recibos_Empleados.Campo_Total_Percepciones].ToString(), out Importe);
                            // cambiar nombre del tipo de nomina para que coincida con formato
                            Nombre_Nomina = Nombre_Nomina.Replace(" A SALARIOS", "").Replace("SUELDOS ", "").Replace("NOMINA ", "").Replace("ES Y JUBILACIONES", "ADO");


                            // reiniciar contadores por banco e insertar totales del banco anterior
                            if (Nombre_Banco_Anterior != Nombre_Banco)
                            {
                                if (Nombre_Banco_Anterior != "")
                                {
                                    Renglon = Hoja.Table.Rows.Add();
                                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Total_Depositos_Banco.ToString(), CarlosAg.ExcelXmlWriter.DataType.Number, "HeaderStyle"));
                                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("TOTAL ", "Subtitulo"));
                                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Total_Importe_Banco.ToString(), CarlosAg.ExcelXmlWriter.DataType.Number, "Totales"));
                                    // agregar espacios entre un banco y otro
                                    if (Nombre_Banco_Anterior != "")
                                    {
                                        Renglon = Hoja.Table.Rows.Add();
                                        Renglon = Hoja.Table.Rows.Add();
                                        Renglon = Hoja.Table.Rows.Add();
                                    }
                                }

                                Total_Depositos_Banco = 0;
                                Total_Importe_Banco = 0;
                                // encabezado nombre banco
                                Renglon = Hoja.Table.Rows.Add();
                                // insertar nombre del banco o si no tiene nombre, insertar el texto EFECTIVO
                                if (Nombre_Banco != "")
                                {
                                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("Depósitos Nómina " + Nombre_Banco + " " + Fecha.ToString("dd/MM/yyyy"), "Subtitulo"));
                                }
                                else
                                {
                                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("Depósitos Nómina EFECTIVO " + Fecha.ToString("dd/MM/yyyy"), "Subtitulo"));
                                }
                                Renglon = Hoja.Table.Rows.Add();

                                // rengln en blanco y nombres columnas
                                Renglon = Hoja.Table.Rows.Add();
                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("Regs.", "HeaderStyle"));
                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("Archivo", "HeaderStyle"));
                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("Importe", "HeaderStyle"));
                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(""));
                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("Verificación", "HeaderStyle"));
                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("Envió", "HeaderStyle"));
                                Renglon = Hoja.Table.Rows.Add();

                            }

                            // insertar registros de depósitos
                            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Numero_Depositos.ToString(), CarlosAg.ExcelXmlWriter.DataType.Number, "HeaderStyle"));

                            // dependiendo del nombre del banco insertar nombre de  archivo
                            switch (Nombre_Banco.Replace("CAJA ", ""))
                            {
                                case "BANCOMER":
                                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("BCMER00" + Tipo_Nomina_ID, "Subtitulo"));
                                    break;
                                case "BAJIO":
                                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("D02560" + Tipo_Nomina_ID + "0", "Subtitulo"));
                                    break;
                                case "BANORTE":
                                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("N030110" + Tipo_Nomina_ID, "Subtitulo"));
                                    break;
                                default:
                                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Nombre_Nomina, "Subtitulo"));
                                    break;
                            }
                            // importe
                            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Importe.ToString(), CarlosAg.ExcelXmlWriter.DataType.Number, "Numeros"));

                            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("- "));

                            // sumar contadores
                            Total_Importe_Banco += Importe;
                            Gran_Total_Importe += Importe;
                            Total_Depositos_Banco += Numero_Depositos;
                            Gran_Total_Depositos += Numero_Depositos;
                            Nombre_Banco_Anterior = Nombre_Banco;
                            // agregar al diccionario
                            if (!Dic_Nomina_Total_Deposito.ContainsKey(Nombre_Nomina))
                            {
                                Dic_Nomina_Total_Deposito.Add(Nombre_Nomina, Importe);
                                Dic_Tipo_Nomina.Add(Nombre_Nomina, Tipo_Nomina_ID);
                            }
                            else // simar al valor en el diccionario por nomina
                            {
                                Dic_Nomina_Total_Deposito[Nombre_Nomina] += Importe;
                            }
                        }
                    }

                    // agregar totales del ultimo banco
                    Renglon = Hoja.Table.Rows.Add();
                    Renglon = Hoja.Table.Rows.Add();
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Total_Depositos_Banco.ToString(), CarlosAg.ExcelXmlWriter.DataType.Number, "HeaderStyle"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("TOTAL ", "Subtitulo"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Total_Importe_Banco.ToString(), CarlosAg.ExcelXmlWriter.DataType.Number, "Totales"));

                    // insertar GRAN TOTAL
                    Renglon = Hoja.Table.Rows.Add();
                    Renglon = Hoja.Table.Rows.Add();
                    Renglon = Hoja.Table.Rows.Add();
                    Renglon = Hoja.Table.Rows.Add();
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Gran_Total_Depositos.ToString(), CarlosAg.ExcelXmlWriter.DataType.Number, "HeaderStyle"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("GRAN TOTAL ", "Subtitulo"));
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Gran_Total_Importe.ToString(), CarlosAg.ExcelXmlWriter.DataType.Number, "Totales"));

                    // agregar totales por tipo de nomina
                    Renglon = Hoja.Table.Rows.Add();
                    Renglon = Hoja.Table.Rows.Add();
                    foreach (KeyValuePair<string, decimal> Totales in Dic_Nomina_Total_Deposito)
                    {
                        Renglon = Hoja.Table.Rows.Add();
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Dic_Tipo_Nomina[Totales.Key], CarlosAg.ExcelXmlWriter.DataType.Number, "Numeros"));
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Totales.Key, "Subtitulo"));
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Totales.Value.ToString("#,#.##"), CarlosAg.ExcelXmlWriter.DataType.Number, "Numeros"));

                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte de Archivos de dispersión. Error: [" + Ex.Message + "]");
        }
        return Libro;
    }
    #endregion (Reportes)

    #endregion (Métodos)

    #region (Eventos)

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Calendario_Nomina_SelectedIndexChanged
    ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Calendario_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 index = Cmb_Calendario_Nomina.SelectedIndex;
        if (index > 0)
        {
            Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());
        }
        else
        {
            Cmb_Periodos_Catorcenales_Nomina.DataSource = new DataTable();
            Cmb_Periodos_Catorcenales_Nomina.DataBind();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Periodo_SelectedIndexChanged
    ///DESCRIPCIÓN: Carga la fecha de inicio y fin del periodo catorcenal seleccionado.
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Periodo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nomina = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Ope_Nom_Pestamos_Negocio Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
        DateTime Fecha_Inicio = new DateTime();//Fecha de inicio de la catorcena a generar la nómina.
        DateTime Fecha_Fin = new DateTime();//Fecha de fin de la catorcena a generar la nómina.

        try
        {
            if (Cmb_Calendario_Nomina.SelectedIndex > 0)
            {
                Prestamos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();

                Prestamos.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedItem.Text.Trim());
                Dt_Detalles_Nomina = Prestamos.Consultar_Fechas_Periodo();

                if (Dt_Detalles_Nomina != null)
                {
                    if (Dt_Detalles_Nomina.Rows.Count > 0)
                    {
                        Fecha_Inicio = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString());
                        Fecha_Fin = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString());

                        Txt_Inicia_Catorcena.Text = string.Format("{0:dd/MMM/yyyy}", Fecha_Inicio);
                        Txt_Fin_Catorcena.Text = string.Format("{0:dd/MMM/yyyy}", Fecha_Fin);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    #region (Botones)

    /// *************************************************************************************
    /// NOMBRE: Btn_Generar_Reporte_Excel_Click
    /// 
    /// DESCRIPCIÓN: manejo del evento click del botón generar reporte Excel que se 
    ///     encarga de llamar a los métodos que van a consultar los datos, cargarlos en 
    ///     un archivo Excel y ofrecerlos para descarga
    ///              
    /// PARÁMETROS: No Aplica
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 10/Diciembre/2011.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Btn_Generar_Reporte_Excel_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Depositos;
        DataTable Dt_Depositos_Tipos_Nomina;
        DateTime Fecha_Reporte;
        CarlosAg.ExcelXmlWriter.Workbook Libro;//Creamos la variable que almacenara el libro de excel.

        DateTime.TryParse(Txt_Fecha.Text, out Fecha_Reporte);

        Lbl_Mensaje_Error.Text = "";

        if (Validar_Datos_Formaulario())
        {
            try
            {
                Dt_Depositos = Consultar_Depositos();

                Dt_Depositos.TableName = "Dt_Depositos";

                // agregar todos los tipos de mómina a la tabla
                Dt_Depositos_Tipos_Nomina = Agregar_Todas_Nominas(Dt_Depositos);

                //Obtenemos el libro.
                Libro = Generar_Reporte_Excel(Dt_Depositos_Tipos_Nomina, Fecha_Reporte);
                //Mandamos a imprimir el reporte en excel.
                Mostrar_Excel(Libro, "Archivos_Dispersion.xls");

            }
            catch (Exception Ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Ex.Message.ToString();
            }
        }
        else
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }

    #endregion

    #endregion
}
