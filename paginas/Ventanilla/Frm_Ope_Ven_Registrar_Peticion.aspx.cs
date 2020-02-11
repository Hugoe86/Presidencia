using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using Presidencia.Colonias.Negocios;
using Presidencia.Catalogo_Calles.Negocio;
using Presidencia.Registro_Peticion.Negocios;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Ventanilla_Usarios.Negocio;

public partial class paginas_Ventanilla_Frm_Ope_Ven_Registrar_Peticion : System.Web.UI.Page
{
    #region Variables Locales
    private int Contador_Columna;
    private String Informacion;
    #endregion
    #region PageLoad/Init
    protected void Page_Load(object sender, EventArgs e)
    {
        // si no hay un id de ciudadano (no ha iniciado sesión), mostrar mensaje indicando que primero se requiere iniciar sesión
        if (String.IsNullOrEmpty(Cls_Sessiones.Ciudadano_ID))
        {
            Habilitar_Botones("Inicial");
            Btn_Buscar_Registro_Peticion.Enabled = false;
            Div_Contenido.Controls.Clear();
            Contenedor_Datos_Peticion.Controls.Clear();
            // enlace a login
            HyperLink Hl_Enlace_Login = new HyperLink();
            StringBuilder Sb_Enlace = new StringBuilder();
            HtmlTextWriter Htw_Enlace = new HtmlTextWriter(new System.IO.StringWriter(Sb_Enlace, System.Globalization.CultureInfo.InvariantCulture));
            Hl_Enlace_Login.Text = "Regístrese por favor.";
            Htw_Enlace.AddAttribute(HtmlTextWriterAttribute.Href, "Frm_Apl_Login_Ventanilla.aspx");
            Htw_Enlace.AddAttribute(HtmlTextWriterAttribute.Class, "enlace_interno");
            Hl_Enlace_Login.RenderControl(Htw_Enlace);
            Mostrar_Informacion("Requiere tener cuenta para registrar peticiones, " + Sb_Enlace.ToString(), true);
            return;
            //Response.Redirect("Frm_Apl_Login_Ventanilla.aspx");
        }

        if (!Page.IsPostBack)
        {
            LLenar_Combos();
            Habilitar_Botones("Inicial");
            Limpiar_Campos();
            Habilitar_Campos(false);
            Consultar_Peticiones();

            // registro de scripts del lado del servidor para mostrar ventanas emergentes para búsqueda avanzada
            string Ventana_Modal = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias_Calles.aspx', 'center:yes;resizable:no;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Calle.Attributes.Add("onclick", Ventana_Modal);
            Ventana_Modal = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Colonia.Attributes.Add("onclick", Ventana_Modal);
            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergente/Frm_Busqueda_Avanzada_Asuntos.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
        }
        //Mostrar_Informacion("", false);
    }
    #endregion

    #region Métodos

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Mostrar_Informacion
    ///DESCRIPCIÓN: Muestra en la página el mensaje recibido como parámetro y establece la visibilidad 
    ///             de los controles  para mostrar mensajes con el segundo parámetro
    ///PARÁMETROS:
    /// 		1. Mensaje: Texto a mostrar en la página
    /// 		2. Mostrar: establece la visibilidad de los controles en los que se muestran los mensajes de la página
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Mostrar_Informacion(String Mensaje, bool Mostrar)
    {
        Lbl_Mensaje.Text = Mensaje;
        Lbl_Mensaje.Visible = Mostrar;
        Img_Informacion.Visible = Mostrar;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Validar_Expresion
    ///DESCRIPCIÓN: Valida mediante expresiones regulares tipo de dato y/o formato del texto recibido como parámetro
    ///PARÁMETROS:
    /// 		1. Str_Cadena: texto que se va a validar
    /// 		2. Str_Tipo_Validacion: tipo de validación a realizar (String, Integer, Email, etc.)
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public Boolean Validar_Expresion(String Str_Cadena, String Str_Tipo_Validacion)
    {
        String Str_Expresion;
        Str_Expresion = String.Empty;
        //Se selecciona el tipo de valor a validar
        switch (Str_Tipo_Validacion)
        {
            case "String":
                Str_Expresion = "[^a-zA-Z.ÑÁÉÍÓñáéíóúü\\s]";
                break;
            case "Integer":
                Str_Expresion = "[^0-9]";
                break;
            case "Varchar":
                Str_Expresion = "[^a-zA-ZÑÁÉÍÓñáéíóúü\\/\\*,-.()0-9\\s]";
                break;
            case "Date":
                Str_Expresion = "\\d{2}/\\d{2}/\\d{2}";
                break;
            case "Email":
                Str_Expresion = "^\\w+([\\.-]?\\w+)*@\\w+([\\.-]?\\w+)*(\\.\\w{2,3})+$";
                //@"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){2}(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5]))$";                        
                break;
            case "Password":
                Str_Expresion = "[^a-zA-ZÑÁÉÍÓñáéíóúü.0-9\\s]";
                break;
            case "CURP":
                Str_Expresion = "^[a-zA-Z]{4}(\\d{6})([a-zA-Z]{6})(\\d{2})?$";
                break;
        }

        //Se revisa la expresión
        Regex Exp_Regular = new Regex(Str_Expresion);
        //Regresa un valor true o false según se cumplan las condiciones
        if (Str_Tipo_Validacion == "Date" || Str_Tipo_Validacion == "Email" || Str_Tipo_Validacion == "CURP")
            return !(Exp_Regular.IsMatch(Str_Cadena));
        else
            return Exp_Regular.IsMatch(Str_Cadena);

    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Convertir_Fecha
    ///DESCRIPCIÓN: convierte la fecha que recibe como parámetro separando los componentes por diagonales 
    ///             y regresa una cadena de texto con el primer y segundo componente invertidos
    ///PARÁMETROS:
    /// 		1. Str_Fecha: fecha que se va a convertir
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public String Convertir_Fecha(String Str_Fecha)
    {
        String[] Str_Temporal = Str_Fecha.Split('/');
        return Str_Temporal[1] + "/" + Str_Temporal[0] + "/" + Str_Temporal[2];
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Validar_Longitud
    ///DESCRIPCIÓN: Regresa verdadero el texto en el control textbox recibido como parámetro 
    ///             tiene por lo menos la longitud indicada
    ///PARÁMETROS:
    /// 		1. Txt_Control: control de tipo textbox cuyo contenido se va a validar
    /// 		2. Int_Tamaño: longitud mínima que se espera del texto en el control recibido
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public Boolean Validar_Longitud(TextBox Txt_Control, int Int_Tamaño)
    {
        Boolean Bln_Bandera;
        Bln_Bandera = false;
        //Verifica el tamaño de el control
        if (Txt_Control.Text.Length >= Int_Tamaño)
            Bln_Bandera = true;
        return Bln_Bandera;
    }
    private void Generar_Tabla_Informacion()
    {
        Contador_Columna = Contador_Columna + 1;
        if (Contador_Columna > 2)
        {
            Contador_Columna = 0;
            Informacion += "</tr><tr>";
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Validar_Campos_Obligatorios
    ///DESCRIPCIÓN: Valida el contenido de los campos obligatorios del formulario
    ///         regresa verdadero si la validación es exitosa y falso si no, además genera un mensaje con 
    ///         los errores detectados
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Boolean Validar_Campos_Obligatorios()
    {
        Boolean Bln_Bandera;
        Contador_Columna = 0;
        Bln_Bandera = true;
        Informacion += "<table style='width: 100%;font-size:9px;' >" +
            "<tr colspan='3'>Es necesario introducir:</tr>" +
            "<tr>";
        // Verifica qué los campos seleccionados tengan un valor

        if (Txt_Nombre.Text.Trim() == String.Empty)
        {
            Informacion += "<td>+Campo Nombre.</td>";
            Bln_Bandera = false;
            Generar_Tabla_Informacion();
        }
        if (Txt_Apellido_Paterno.Text.Trim() == String.Empty)
        {
            Informacion += "<td>+Campo Apellido Paterno.</td>";
            Bln_Bandera = false;
            Generar_Tabla_Informacion();
        }
        if (Cmb_Sexo.SelectedValue == "0")
        {
            Informacion += "<td>+Campo Sexo.</td>";
            Bln_Bandera = false;
            Generar_Tabla_Informacion();
        }
        if (Txt_Referencia.Text.Trim().Length <= 0)
        {
            if (Cmb_Calle.SelectedIndex <= 0)
            {
                Informacion += "<td>+Campo Calle o referencia.</td>";
                Bln_Bandera = false;
                Generar_Tabla_Informacion();
            }
            if (Cmb_Colonia.SelectedValue == "0")
            {
                Informacion += "<td>+Campo Colonia o referencia.</td>";
                Bln_Bandera = false;
                Generar_Tabla_Informacion();
            }
        }
        if (Txt_Peticion.Text.Trim() == String.Empty)
        {
            Informacion += "<td>+Descripción de la Petición.</td>";
            Bln_Bandera = false;
            Generar_Tabla_Informacion();
        }
        else
        {
            if (Validar_Longitud(Txt_Peticion, 4000))
            {
                Lbl_Mensaje.Text = String.Empty;
                Informacion += "<td>+Campo Peticion excede la longitud permitida.</td>";
                Bln_Bandera = false;
                Generar_Tabla_Informacion();
            }
        }
        if (Txt_Email.Text.Length > 0)
        {
            if (Validar_Expresion(Txt_Email.Text, "Email"))
            {
                Informacion += "<td>+Formato de e-mail incorrecto.</td>";
                Bln_Bandera = false;
                Generar_Tabla_Informacion();
            }
        }


        //DateTime date = Convert.ToDateTime( "11/23/10");
        //System.Windows.Forms.MessageBox.Show(date.CompareTo(DateTime.Now) + "");
        Informacion += "</tr></table>";
        return Bln_Bandera;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Habilitar_Campos
    ///DESCRIPCIÓN: Habilita o deshabilita los controles en el formulario de acuerdo con el parámetro recibido
    ///PARÁMETROS:
    /// 		1. Habilitar: boleano que indica si se activan o desactivan los controles
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Habilitar_Campos(Boolean Habilitar)
    {
        Txt_Nombre.Enabled = Habilitar;
        Txt_Apellido_Paterno.Enabled = Habilitar;
        Txt_Apellido_Materno.Enabled = Habilitar;
        Txt_Edad.Enabled = Habilitar;
        Cmb_Sexo.Enabled = Habilitar;
        Cmb_Calle.Enabled = Habilitar;
        Txt_Numero_Exterior.Enabled = Habilitar;
        Txt_Numero_Interior.Enabled = Habilitar;
        Txt_Referencia.Enabled = Habilitar;
        Cmb_Colonia.Enabled = Habilitar;
        Txt_Codigo_Postal.Enabled = Habilitar;
        Txt_Telefono.Enabled = Habilitar;
        Txt_Email.Enabled = Habilitar;
        Txt_Fecha_Solucion.Enabled = Habilitar;
        Txt_Solucion.Enabled = false;
        Txt_Peticion.Enabled = Habilitar;

        Btn_Buscar_Calle.Enabled = Habilitar;
        Btn_Buscar_Colonia.Enabled = Habilitar;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Limpiar_Campos
    ///DESCRIPCIÓN: Limpiar las cajas de texto y quitar la selección de los combos
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Limpiar_Campos()
    {
        //Se limpian los campos
        Txt_Folio.Text = String.Empty;
        Txt_Fecha_Peticion.Text = String.Empty;
        Txt_Nombre.Text = String.Empty;
        Txt_Apellido_Paterno.Text = String.Empty;
        Txt_Apellido_Materno.Text = String.Empty;
        Txt_Edad.Text = String.Empty;
        Cmb_Sexo.SelectedIndex = -1;
        Cmb_Calle.Items.Clear();
        Txt_Numero_Exterior.Text = String.Empty;
        Txt_Numero_Interior.Text = String.Empty;
        Txt_Referencia.Text = String.Empty;
        Cmb_Colonia.SelectedIndex = 0;
        Txt_Codigo_Postal.Text = String.Empty;
        Txt_Telefono.Text = String.Empty;
        Txt_Email.Text = String.Empty;
        Txt_Fecha_Solucion.Text = String.Empty;
        Txt_Solucion.Text = String.Empty;
        Txt_Peticion.Text = String.Empty;

        Session.Remove("Dt_Seguimiento");
        Session.Remove("Dt_Observaciones");
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Llenar_Combo_Con_DataTable
    ///DESCRIPCIÓN: Asigna los valores de la tabla en el combo recibidos como parámetros
    ///PARÁMETROS:
    /// 		1. Obj_Combo: control al que se van a asignar los datos en la tabla
    /// 		2. Dt_Temporal: tabla con los datos a mostrar en el control Obj_Combo
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Llenar_Combo_Con_DataTable(DropDownList Obj_Combo, DataTable Dt_Temporal)
    {
        Obj_Combo.Items.Clear();
        Obj_Combo.SelectedValue = null;
        Obj_Combo.DataSource = Dt_Temporal;
        Obj_Combo.DataValueField = Dt_Temporal.Columns[0].ToString();
        Obj_Combo.DataTextField = Dt_Temporal.Columns[1].ToString();
        Obj_Combo.DataBind();
        Obj_Combo.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<SELECCIONAR>"), ""));
        Obj_Combo.SelectedIndex = 0;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Llenar_Combo_Con_DataTable
    ///DESCRIPCIÓN: Asigna los valores de la tabla en el combo recibidos como parámetros
    ///PARÁMETROS:
    /// 		1. Obj_Combo: control al que se van a asignar los datos en la tabla
    /// 		2. Dt_Temporal: tabla con los datos a mostrar en el control Obj_Combo
    /// 		3. Indice_Campo_Valor: entero con el número de columna de la tabla con el valor para el combo
    /// 		4. Indice_Campo_Texto: entero con el número de columna de la tabla con el texto para el combo
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Llenar_Combo_Con_DataTable(DropDownList Obj_Combo, DataTable Dt_Temporal, int Indice_Campo_Valor, int Indice_Campo_Texto)
    {
        Obj_Combo.Items.Clear();
        Obj_Combo.SelectedValue = null;
        Obj_Combo.DataSource = Dt_Temporal;
        Obj_Combo.DataTextField = Dt_Temporal.Columns[Indice_Campo_Texto].ToString();
        Obj_Combo.DataValueField = Dt_Temporal.Columns[Indice_Campo_Valor].ToString();
        Obj_Combo.DataBind();
        Obj_Combo.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<SELECCIONAR>"), ""));
        Obj_Combo.SelectedIndex = 0;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: LLenar_Combos
    ///DESCRIPCIÓN: Consulta los datos para los combos y llama al método que carga los datos de la consulta en
    ///             los combos
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void LLenar_Combos()
    {
        var Obj_Colonias = new Cls_Cat_Ate_Colonias_Negocio();

        try
        {
            // Combo de Sexo
            Cmb_Sexo.Items.Add("<SELECCIONAR>");
            Cmb_Sexo.Items[0].Value = "";
            Cmb_Sexo.Items.Add("MASCULINO");
            Cmb_Sexo.Items[1].Value = "MASCULINO";
            Cmb_Sexo.Items.Add("FEMENINO");
            Cmb_Sexo.Items[2].Value = "FEMENINO";
            Cmb_Sexo.Items[0].Selected = true;
            // Combo de Colonia
            Obj_Colonias.P_Filtros_Dinamicos = Cat_Ate_Colonias.Campo_Estatus + " LIKE '%VIGENTE%'";
            Llenar_Combo_Con_DataTable(Cmb_Colonia, Obj_Colonias.Consulta_Datos().Tables[0]);
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("No se pudo mostrar información: " + Ex.Message, true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cargar_Datos_Constribuyente
    ///DESCRIPCIÓN: Consulta los datos del usuario actual en la sesión para mostrar la información en el formulario
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 21-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Cargar_Datos_Constribuyente()
    {
        var Obj_Contribuyente = new Cls_Ven_Usuarios_Negocio();
        DataTable Dt_Contribuyente;

        try
        {
            // validar que la sesión no sea nulo
            if (!string.IsNullOrEmpty(Cls_Sessiones.Ciudadano_ID))
            {
                Obj_Contribuyente.P_Usuario_ID = Cls_Sessiones.Ciudadano_ID;
                Dt_Contribuyente = Obj_Contribuyente.Consultar_Contribuyente();
                // validar que la consulta arrojó resultados
                if (Dt_Contribuyente != null && Dt_Contribuyente.Rows.Count > 0)
                {
                    // cargar datos de la consulta en los controles
                    Txt_Apellido_Paterno.Text = Dt_Contribuyente.Rows[0][Cat_Ven_Usuarios.Campo_Apellido_Paterno].ToString();
                    Txt_Apellido_Materno.Text = Dt_Contribuyente.Rows[0][Cat_Ven_Usuarios.Campo_Apellido_Materno].ToString();
                    Txt_Nombre.Text = Dt_Contribuyente.Rows[0][Cat_Ven_Usuarios.Campo_Nombre].ToString();
                    Txt_Edad.Text = Dt_Contribuyente.Rows[0][Cat_Ven_Usuarios.Campo_Edad].ToString();
                    Txt_Email.Text = Dt_Contribuyente.Rows[0][Cat_Ven_Usuarios.Campo_Email].ToString();
                    Txt_Telefono.Text = Dt_Contribuyente.Rows[0][Cat_Ven_Usuarios.Campo_Telefono_Casa].ToString();
                    Txt_Codigo_Postal.Text = Dt_Contribuyente.Rows[0][Cat_Ven_Usuarios.Campo_Codigo_Postal].ToString();
                    // si la colonia existe en el combo, seleccionarla
                    if (Cmb_Colonia.Items.FindByValue(Dt_Contribuyente.Rows[0][Cat_Ven_Usuarios.Campo_Colonia_ID].ToString()) != null)
                    {
                        Cmb_Colonia.SelectedValue = Dt_Contribuyente.Rows[0][Cat_Ven_Usuarios.Campo_Colonia_ID].ToString();
                        Cmb_Colonia_SelectedIndexChanged(null, null);
                    }
                    // si la calle existe en el combo, seleccionarla
                    if (Cmb_Calle.Items.FindByValue(Dt_Contribuyente.Rows[0][Cat_Ven_Usuarios.Campo_Calle_ID].ToString()) != null)
                    {
                        Cmb_Calle.SelectedValue = Dt_Contribuyente.Rows[0][Cat_Ven_Usuarios.Campo_Calle_ID].ToString();
                    }
                    // seleccionar el sexo en el combo
                    if (Dt_Contribuyente.Rows[0][Cat_Ven_Usuarios.Campo_Sexo].ToString().ToUpper().Contains("HOMBRE") || Dt_Contribuyente.Rows[0][Cat_Ven_Usuarios.Campo_Sexo].ToString().ToUpper().Contains("MASCULINO"))
                    {
                        Cmb_Sexo.SelectedValue = "MASCULINO";
                    }
                    else if (Dt_Contribuyente.Rows[0][Cat_Ven_Usuarios.Campo_Sexo].ToString().ToUpper().Contains("MUJER") || Dt_Contribuyente.Rows[0][Cat_Ven_Usuarios.Campo_Sexo].ToString().ToUpper().Contains("FEMENINO"))
                    {
                        Cmb_Sexo.SelectedValue = "FEMENINO";
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("No se pudo mostrar información: " + Ex.Message, true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Habilitar_Botones
    ///DESCRIPCIÓN: Establece las propiedades de los controles de búsqueda y botones nuevo y salir 
    ///         dependiendo del contenido del parámetro recibido.
    ///PARÁMETROS:
    /// 		1. Estado: indica la operación que se pretende realizar y para la que se van a preparar los controles
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Habilitar_Botones(String Estado)
    {
        switch (Estado)
        {
            //Estado Inicial de los controles
            case "Inicial":
                Btn_Nuevo.Visible = false;
                Btn_Salir.Visible = true;
                Btn_Imprimir.Visible = false;
                Btn_Imprimir.Enabled = false;
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Txt_Busqueda_Registro_Peticion.Text = String.Empty;
                Txt_Busqueda_Registro_Peticion.Enabled = true;
                Btn_Buscar_Registro_Peticion.Enabled = true;
                Grid_Peticiones.Enabled = true;
                Contenedor_Datos_Peticion.Visible = false;
                Div_Contenido.Visible = true;
                Tr_Fecha_Solucion.Visible = false;
                Tr_Txt_Solucion.Visible = false;
                Grid_Observaciones.Visible = false;
                Lbl_Historial.Visible = false;
                Grid_Seguimiento.Visible = false;
                Lbl_Seguimiento.Visible = false;
                break;
            //Estado de Nuevo
            case "Nuevo":
                Btn_Nuevo.Visible = true;
                Btn_Imprimir.Visible = false;
                Btn_Imprimir.Enabled = false;
                Txt_Apellido_Paterno.Focus();
                Btn_Nuevo.ToolTip = "Enviar petición";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.ToolTip = "Regresar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Txt_Busqueda_Registro_Peticion.Text = String.Empty;
                Txt_Busqueda_Registro_Peticion.Enabled = false;
                Btn_Buscar_Registro_Peticion.Enabled = false;
                Grid_Peticiones.Enabled = true;
                Contenedor_Datos_Peticion.Visible = true;
                Div_Contenido.Visible = false;
                Tr_Fecha_Solucion.Visible = false;
                Tr_Txt_Solucion.Visible = false;
                Grid_Observaciones.Visible = false;
                Lbl_Historial.Visible = false;
                Grid_Seguimiento.Visible = false;
                Lbl_Seguimiento.Visible = false;
                break;
            case "Visualizar":
                Btn_Nuevo.Visible = false;
                Btn_Salir.Visible = true;
                Btn_Imprimir.Visible = true;
                Btn_Imprimir.Enabled = true;
                Btn_Salir.ToolTip = "Regresar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Txt_Busqueda_Registro_Peticion.Text = String.Empty;
                Txt_Busqueda_Registro_Peticion.Enabled = false;
                Btn_Buscar_Registro_Peticion.Enabled = false;
                Grid_Peticiones.Enabled = false;
                Contenedor_Datos_Peticion.Visible = true;
                Div_Contenido.Visible = false;
                Tr_Fecha_Solucion.Visible = true;
                Tr_Txt_Solucion.Visible = true;
                Grid_Observaciones.Visible = true;
                Lbl_Historial.Visible = true;
                Grid_Seguimiento.Visible = true;
                Lbl_Seguimiento.Visible = true;
                break;
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Registra_Datos_Negocio
    ///DESCRIPCIÓN: Asigna las propiedades del objeto recibido como parámetro con la selección en 
    ///             los controles del formulario en la página
    ///PARÁMETROS:
    /// 		1. Obj_Peticiones: instancia de la clase de negocio en la que se van a asignar los datos seleccionados en la página
    /// 		2. Modo: indica si se va a dar de alta un "Nuevo" registro o es una modificación
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Registra_Datos_Negocio(Cls_Cat_Ate_Peticiones_Negocio Obj_Peticiones, String Modo)
    {
        Obj_Peticiones.P_Nombre = Txt_Nombre.Text;
        Obj_Peticiones.P_Apellido_Paterno = Txt_Apellido_Paterno.Text;
        Obj_Peticiones.P_Apellido_Materno = Txt_Apellido_Materno.Text;
        Obj_Peticiones.P_Usuario_ID = Cls_Sessiones.Ciudadano_ID;
        if (Txt_Edad.Text.Trim() != String.Empty)
        {
            Obj_Peticiones.P_Edad = int.Parse(Txt_Edad.Text);
        }
        Obj_Peticiones.P_Sexo = Cmb_Sexo.SelectedValue;
        Obj_Peticiones.P_Colonia_ID = Cmb_Colonia.SelectedValue;
        Obj_Peticiones.P_Calle_ID = Cmb_Calle.SelectedValue;
        Obj_Peticiones.P_Numero_Exterior = Txt_Numero_Exterior.Text;
        Obj_Peticiones.P_Numero_Interior = Txt_Numero_Interior.Text;
        Obj_Peticiones.P_Referencia = Txt_Referencia.Text;
        Obj_Peticiones.P_Codigo_Postal = Txt_Codigo_Postal.Text;
        Obj_Peticiones.P_Telefono = Txt_Telefono.Text;
        Obj_Peticiones.P_Email = Txt_Email.Text;
        if (Txt_Peticion.Text.Length <= 4000)
        {
            Obj_Peticiones.P_Peticion = Txt_Peticion.Text;
        }
        else
        {
            Obj_Peticiones.P_Peticion = Txt_Peticion.Text.Substring(0, 4000);
        }
        //Obj_Peticiones.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
        //Obj_Peticiones.P_Asunto_ID = Cmb_Asunto.SelectedValue;
        //Obj_Peticiones.P_Accion_ID = Cmb_Accion.SelectedValue;
        Obj_Peticiones.P_Estatus = "GENERADA";
        Obj_Peticiones.P_Asignado = "N";
        Obj_Peticiones.P_Anio_Peticion = DateTime.Now.Year;
        Obj_Peticiones.P_Fecha_Peticion = DateTime.Now.ToString("dd/MM/yyyy");

        Obj_Peticiones.P_Usuario_Creo_Modifico = Cls_Sessiones.Nombre_Ciudadano;

    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Alta_Peticion
    ///DESCRIPCIÓN: llama a los métodos para dar de alta una petición con los datos en la página y 
    ///         muestra un mensaje si la operación se realizó con éxito
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Alta_Peticion()
    {
        Cls_Cat_Ate_Peticiones_Negocio Obj_Peticiones = new Cls_Cat_Ate_Peticiones_Negocio();
        Cls_Ven_Usuarios_Negocio Obj_Parametros = new Cls_Ven_Usuarios_Negocio();
        DataTable Dt_Parametros;
        int Cantidad_Filas_Insertadas = 0;
        string Folio = "";

        try
        {
            string Domicilio;
            // obtener el domicilio completo del solicitante para la impresión del formato (porque se limpian los campos con el domicilio antes de mandar la impresión)
            Domicilio = Obtener_Domicilio_Completo_Solicitante();
            string Nombre_Solicitante = Txt_Apellido_Paterno.Text + " " + Txt_Apellido_Materno.Text + " " + Txt_Nombre.Text;
            string Fecha = DateTime.Now.ToString("dd/MMM/yyyy");
            string Email = Txt_Email.Text;
            string Peticion = Txt_Peticion.Text;
            string Telefono = Txt_Telefono.Text;

            Dt_Parametros = Obj_Parametros.Consultar_Parametros();
            if (Dt_Parametros != null && Dt_Parametros.Rows.Count > 0)
            {
                Obj_Peticiones.P_Programa_ID = Dt_Parametros.Rows[0][Cat_Ven_Parametros.Campo_Programa_ID_Web].ToString();
                Obj_Peticiones.P_Origen = Dt_Parametros.Rows[0][Cat_Ate_Programas.Campo_Nombre].ToString();

                Registra_Datos_Negocio(Obj_Peticiones, "Nuevo");
                Cantidad_Filas_Insertadas = Obj_Peticiones.Alta_Peticion();
                if (Cantidad_Filas_Insertadas > 0)
                {
                    Folio = Obj_Peticiones.P_Folio;
                    Habilitar_Botones("Inicial");
                    Limpiar_Campos();
                    Habilitar_Campos(false);
                    Consultar_Peticiones();
                    Imprimir_Reporte(Crear_Ds_Imprimir_Reporte(Folio, Fecha, Nombre_Solicitante, Domicilio, Email, Peticion, Telefono), "Rpt_Ope_Ven_Folio_Peticion.rpt", "Folio_Reporte_Ciudadano");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('La Petición fue registrada.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('La Petición no pudo ser registrada.');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('La Petición no pudo ser registrada.');", true);
            }
        }
        catch (Exception Ex)
        {
            if (Cantidad_Filas_Insertadas > 0)
            {
                string Mensaje_Error;
                // si hay un folio, mostrar mensaje de error con folio
                if (!string.IsNullOrEmpty(Folio))
                {
                    Mostrar_Informacion("Alta exitosa de petición con folio: " + Folio + ". Ocurrió un error al imprimir el formato.", true);
                }
                else
                {
                    Mostrar_Informacion("Alta exitosa de petición, ocurrió un error al imprimir el formato.", true);
                }
            }
            else
            {
                Mostrar_Informacion("Alta petición: " + Ex.Message, true);
            }
        }
        finally
        {
            Obj_Peticiones = null;
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Obtener_Domicilio_Completo_Solicitante
    ///DESCRIPCIÓN: Regresa una cadena de caracteres con el domicilio completo del solicitante tomado de 
    ///             los valores en la página
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 19-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private string Obtener_Domicilio_Completo_Solicitante()
    {
        string Domicilio = "";

        // formar el domicilio
        if (Cmb_Calle.SelectedIndex > 0)
        {
            Domicilio += Cmb_Calle.SelectedItem.Text + Txt_Numero_Exterior.Text + " " + Txt_Numero_Interior.Text;
        }
        else
        {
            if (Txt_Numero_Exterior.Text.Trim().Length > 0)
            {
                Domicilio += "Número exterior " + Txt_Numero_Exterior.Text;
            }
            if (Txt_Numero_Interior.Text.Trim().Length > 0)
            {
                Domicilio += " int. " + Txt_Numero_Interior.Text;
            }
        }
        // si hay una colonia seleccionada, agregar al domicilio
        if (Cmb_Colonia.SelectedIndex > 0)
        {
            if (Domicilio.Length > 0)
            {
                Domicilio += " " + Cmb_Colonia.SelectedItem.Text;
            }
            else
            {
                Domicilio += "colonia " + Cmb_Colonia.SelectedItem.Text;
            }
        }
        if (Domicilio.Length > 0 && Txt_Referencia.Text.Trim().Length > 0)
        {
            Domicilio += "\\r\\n" + Txt_Referencia.Text;
        }
        else if (Domicilio.Length <= 0 && Txt_Referencia.Text.Trim().Length > 0)
        {
            Domicilio = Txt_Referencia.Text;
        }

        return Domicilio;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Consultar_Peticiones
    ///DESCRIPCIÓN: Consulta las peticiones de acuerdo con el criterio especificado en el campo de búsqueda
    ///         (sólo para el usuario actual)
    ///         y llama al método que muestra los resultados en el grid de peticiones
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 18-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Consultar_Peticiones()
    {
        var Obj_Peticiones = new Cls_Cat_Ate_Peticiones_Negocio();
        DataTable Dt_Peticiones = null;
        try
        {
            // agregar el id de usuario para sólo consultar las peticiones del usuario actual
            if (!string.IsNullOrEmpty(Cls_Sessiones.Ciudadano_ID))
            {
                Obj_Peticiones.P_Usuario_ID = Cls_Sessiones.Ciudadano_ID;
            }
            else
            {
                Obj_Peticiones.P_Usuario_ID = "";
            }
            // si se especificó un folio, agregar al la propiedad para la consulta
            if (Txt_Busqueda_Registro_Peticion.Text.Trim() != "")
            {
                Obj_Peticiones.P_Folio = Txt_Busqueda_Registro_Peticion.Text.Trim();
            }
            // omitir folio de peticiones con estatus TERMINADA
            Obj_Peticiones.P_Filtros_Dinamicos = Ope_Ate_Peticiones.Campo_Estatus + " != 'TERMINADA' ";

            Dt_Peticiones = Obj_Peticiones.Consulta_Peticion();

            Cargar_Datos_Grid_Peticiones(Dt_Peticiones, "FECHA_PETICION DESC");

            // almacenar datatable en variable de sesión
            Session["Dt_Peticiones"] = Dt_Peticiones;
            ViewState["SortDirection"] = "DESC";
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al consultar Peticiones: [" + Ex + "]", true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cargar_Datos_Peticion
    ///DESCRIPCIÓN: Consulta los datos de la petición especificada en los controles correspondientes
    ///PARÁMETROS:
    /// 		1. No_Peticion: número de petición a consultar
    /// 		2. Anio_Peticion: año de la petición a consultar
    /// 		3. Programa_ID: id del programa a consultar
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 18-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Cargar_Datos_Peticion(string No_Peticion, int Anio_Peticion, string Programa_ID)
    {
        DateTime Fecha;
        try
        {
            var Obj_Peticiones = new Cls_Cat_Ate_Peticiones_Negocio();

            Obj_Peticiones.P_No_Peticion = No_Peticion;
            Obj_Peticiones.P_Anio_Peticion = Anio_Peticion;
            Obj_Peticiones.P_Programa_ID = Programa_ID;
            DataTable Dt_Temporal = Obj_Peticiones.Consulta_Peticion();

            // si la consulta arroja resultados, cargar datos
            if (Dt_Temporal != null && Dt_Temporal.Rows.Count > 0)
            {
                DataRow Renglon = Dt_Temporal.Rows[0];

                Txt_Folio.Text = Renglon[Ope_Ate_Peticiones.Campo_Folio].ToString();
                Txt_Nombre.Text = Renglon[Ope_Ate_Peticiones.Campo_Nombre_Solicitante].ToString();
                Txt_Apellido_Paterno.Text = Renglon[Ope_Ate_Peticiones.Campo_Apellido_Paterno].ToString();
                Txt_Apellido_Materno.Text = Renglon[Ope_Ate_Peticiones.Campo_Apellido_Materno].ToString();
                Txt_Edad.Text = Renglon[Ope_Ate_Peticiones.Campo_Edad].ToString();
                Txt_Numero_Exterior.Text = Renglon[Ope_Ate_Peticiones.Campo_Numero_Exterior].ToString();
                Txt_Numero_Interior.Text = Renglon[Ope_Ate_Peticiones.Campo_Numero_Interior].ToString();
                Txt_Referencia.Text = Renglon[Ope_Ate_Peticiones.Campo_Referencia].ToString();
                Txt_Codigo_Postal.Text = Renglon[Ope_Ate_Peticiones.Campo_Codigo_Postal].ToString();
                Txt_Telefono.Text = Renglon[Ope_Ate_Peticiones.Campo_Telefono].ToString();
                Txt_Email.Text = Renglon[Ope_Ate_Peticiones.Campo_Email].ToString();
                Txt_Peticion.Text = Renglon[Ope_Ate_Peticiones.Campo_Descripcion_Peticion].ToString();
                Txt_Solucion.Text = Renglon[Ope_Ate_Peticiones.Campo_Descripcion_Solucion].ToString();
                // si hay una descripción de solución mostrar el control, si no, ocultarlo
                if (Txt_Solucion.Text.Length > 0)
                {
                    Tr_Txt_Solucion.Visible = true;
                }
                else
                {
                    Tr_Txt_Solucion.Visible = false;
                }

                DateTime.TryParse(Renglon[Ope_Ate_Peticiones.Campo_Fecha_Peticion].ToString(), out Fecha);
                Txt_Fecha_Peticion.Text = Fecha.ToString("dd/MMM/yyyy");
                DateTime.TryParse(Renglon[Ope_Ate_Peticiones.Campo_Fecha_Solucion_Real].ToString(), out Fecha);
                if (Fecha != DateTime.MinValue)
                {
                    Txt_Fecha_Solucion.Text = Fecha.ToString("dd/MMM/yyyy");
                    Tr_Fecha_Solucion.Visible = true;
                    Tr_Txt_Solucion.Visible = true;
                }
                else
                {
                    Txt_Fecha_Solucion.Text = "";
                    Tr_Fecha_Solucion.Visible = false;
                    Tr_Txt_Solucion.Visible = false;
                }

                // cargar los valores de los combos revisando primero que el valor a seleccionar exista entre los elementos del combo
                if (Cmb_Sexo.Items.FindByValue(Renglon[Ope_Ate_Peticiones.Campo_Sexo].ToString()) != null)
                {
                    Cmb_Sexo.SelectedValue = Renglon[Ope_Ate_Peticiones.Campo_Sexo].ToString();
                }
                if (Cmb_Colonia.Items.FindByValue(Renglon[Ope_Ate_Peticiones.Campo_Colonia_ID].ToString()) != null)
                {
                    Cmb_Colonia.SelectedValue = Renglon[Ope_Ate_Peticiones.Campo_Colonia_ID].ToString();
                    Cmb_Colonia_SelectedIndexChanged(null, null); // para que se carguen las calles de dicha colonia
                }
                if (Cmb_Calle.Items.FindByValue(Renglon[Ope_Ate_Peticiones.Campo_Calle_ID].ToString()) != null)
                {
                    Cmb_Calle.SelectedValue = Renglon[Ope_Ate_Peticiones.Campo_Calle_ID].ToString();
                }
            }

            Cargar_Detalles_Seguimiento(No_Peticion, Anio_Peticion, Programa_ID);
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al cargar Peticiones: [" + Ex + "]", true);
        }
    }

    /// *****************************************************************************************
    /// NOMBRE: Cargar_Datos_Grid_Peticiones
    /// DESCRIPCIÓN: Carga los datos especificados en el grid que se recibe como parámetro
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 18-may-2012
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************
    private void Cargar_Datos_Grid_Peticiones(DataTable Dt_Table, string Orden)
    {
        if (Dt_Table != null && !string.IsNullOrEmpty(Orden))
        {
            Dt_Table.DefaultView.Sort = Orden;
        }

        // mostrar resultado en grid peticiones
        Grid_Peticiones.Columns[1].Visible = true;
        Grid_Peticiones.Columns[2].Visible = true;
        Grid_Peticiones.Columns[3].Visible = true;
        Grid_Peticiones.DataSource = Dt_Table;
        Grid_Peticiones.DataBind();
        Grid_Peticiones.Columns[1].Visible = false;
        Grid_Peticiones.Columns[2].Visible = false;
        Grid_Peticiones.Columns[3].Visible = false;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Imprimir_Reporte
    /// DESCRIPCIÓN: Genera el reporte de Crystal con los datos proporcionados en el DataTable 
    /// PARÁMETROS:
    /// 		1. Ds_Convenio: Dataset con datos a imprimir
    /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
    /// 		3. Nombre_Archivo: Nombre del archivo a generar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Imprimir_Reporte(DataSet Ds_Convenio, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String Ruta = Server.MapPath("../Rpt/Ventanilla/" + Nombre_Reporte);

        Reporte.Load(Ruta);
        Reporte.SetDataSource(Ds_Convenio);

        String PDF_Convenio = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar

        ExportOptions Export_Options_Calculo = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options_Calculo = new DiskFileDestinationOptions();
        Disk_File_Destination_Options_Calculo.DiskFileName = Server.MapPath("../../Reporte/" + PDF_Convenio);
        Export_Options_Calculo.ExportDestinationOptions = Disk_File_Destination_Options_Calculo;
        Export_Options_Calculo.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options_Calculo.ExportFormatType = ExportFormatType.PortableDocFormat;
        Reporte.Export(Export_Options_Calculo);

        Mostrar_Reporte(PDF_Convenio, "Formato");
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Mostrar_Reporte
    /// DESCRIPCIÓN: Visualiza en pantalla el reporte indicado
    /// PARÁMETROS:
    /// 		1. Nombre_Reporte: Nombre del reporte a generar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Mostrar_Reporte(String Nombre_Reporte, String Tipo)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            Pagina = Pagina + Nombre_Reporte;
            AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "Formato",
                "window.open('" + Pagina +
                "', '" + Tipo + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')",
                true
                );
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Crear_Ds_Imprimir_Reporte
    ///DESCRIPCIÓN: Crea un Dataset con los datos de la petición recibidos como parámetro
    ///PARÁMETROS:
    /// 		1. Folio: folio de la petición a imprimir
    /// 		2. Fecha_Peticion: fecha de la petición a imprimir
    /// 		3. Nombre_Solicitante: nombre completo del solicitante
    /// 		4. Domicilio: variable con el domicilio completo del solicitante
    /// 		5. Email: dirección de correo electrónico del solicitante
    /// 		6. Peticion: descripción de la petición a imprimir
    /// 		7. Telefono: número telefónico del solicitante
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 18-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataSet Crear_Ds_Imprimir_Reporte(string Folio, string Fecha_Peticion, string Nombre_Solicitante, string Domicilio, string Email, string Peticion, string Telefono)
    {
        var Ds_Reporte = new Ds_Ope_Consulta_Peticiones_Especifico();
        DataRow Dr_Fila_Reporte = Ds_Reporte.Tables[0].NewRow();

        // insertar datos en la fila instanciada directamente de los controles en pantalla
        Dr_Fila_Reporte["Folio"] = Folio;
        Dr_Fila_Reporte["Fecha_Peticion"] = Fecha_Peticion;
        Dr_Fila_Reporte["Nombre_Solicitante"] = Nombre_Solicitante;
        Dr_Fila_Reporte["Direccion"] = Domicilio;
        Dr_Fila_Reporte["E_mail"] = Email;
        Dr_Fila_Reporte["Peticion"] = Peticion;
        Dr_Fila_Reporte["Telefono"] = Telefono;
        // agregar fila a la tabla
        Ds_Reporte.Tables[0].Rows.Add(Dr_Fila_Reporte);

        return Ds_Reporte;
    }

    ///****************************************************************************************
    ///NOMBRE_FUNCIÓN:Ver_Seguimiento
    ///DESCRIPCIÓN : Consulta la tabla seguimiento y observaciones, muestra los resultados 
    ///             en los grids correspondientes y guarda en variable de sesión el datatable 
    ///             obtenido de la consulta
    ///PARAMETROS  : 
    /// 		1. No_Peticion: número de petición a consultar
    /// 		2. Anio_Peticion: año de la petición a consultar
    /// 		3. Programa_ID: id del programa de la petición a consultar
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 23-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Cargar_Detalles_Seguimiento(string No_Peticion, int Anio_Peticion, string Programa_ID)
    {
        var Obj_Peticiones = new Cls_Cat_Ate_Peticiones_Negocio();
        DataTable Dt_Seguimiento;
        DataTable Dt_Observaciones;

        Obj_Peticiones.P_No_Peticion = No_Peticion;
        Obj_Peticiones.P_Anio_Peticion = Anio_Peticion;
        Obj_Peticiones.P_Programa_ID = Programa_ID;
        Dt_Seguimiento = Obj_Peticiones.Consulta_Peticion_Seguimiento();

        Grid_Seguimiento.DataSource = Dt_Seguimiento;
        Grid_Seguimiento.DataBind();

        Dt_Observaciones = Obj_Peticiones.Consulta_Observaciones_Peticion();
        Grid_Observaciones.DataSource = Dt_Observaciones;
        Grid_Observaciones.DataBind();

        // si el grid observaciones no contiene filas, ocultarlo
        if (Grid_Observaciones.Rows.Count <= 0)
        {
            Contenedor_Grid_Historial.Visible = false;
        }
        else
        {
            Contenedor_Grid_Historial.Visible = true;
        }

        Session["Dt_Seguimiento"] = Dt_Seguimiento;
        Session["Dt_Observaciones"] = Dt_Observaciones;
        ViewState["SortDirection"] = "DESC";
    }

    #endregion Métodos

    #region Eventos

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Dependiendo del estado del botón (tooltip Nuevo o Dar de alta)
    ///         Configurar controles o dar de alta una nueva petición
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 18-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        // limpiar mensajes de error
        Mostrar_Informacion("", false);

        if (Btn_Nuevo.ToolTip == "Nuevo")
        {
            Habilitar_Botones("Nuevo");
            Limpiar_Campos();
            //limpiar grid_peticiones
            Cargar_Datos_Grid_Peticiones(null, null);
            Habilitar_Campos(true);
            // fecha actual en campo fecha_peticion
            Txt_Fecha_Peticion.Text = DateTime.Now.ToString("dd/MMM/2012");
        }
        else
        {
            if (Validar_Campos_Obligatorios())
            {
                // llamar método para dar de alta
                Alta_Peticion();
            }
            else
            {
                Mostrar_Informacion(Informacion, true);
            }
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Imprimir_Click
    ///DESCRIPCIÓN: Reimpresión de folio
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 18-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        // limpiar mensajes de error
        Mostrar_Informacion("", false);
        string Domicilio = "";

        try
        {
            if (Txt_Folio.Text.Length > 0)
            {
                Domicilio = Obtener_Domicilio_Completo_Solicitante();
                string Nombre_Solicitante = Txt_Apellido_Paterno.Text + " " + Txt_Apellido_Materno.Text + " " + Txt_Nombre.Text;
                Imprimir_Reporte(Crear_Ds_Imprimir_Reporte(Txt_Folio.Text, Txt_Fecha_Peticion.Text, Nombre_Solicitante, Domicilio, Txt_Email.Text, Txt_Peticion.Text, Txt_Telefono.Text), "Rpt_Ope_Ven_Folio_Peticion.rpt", "Folio_Reporte_Ciudadano");
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion(Ex.Message, true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Lnk_Nueva_Peticion_Click
    ///DESCRIPCIÓN: Configurar controles o dar de alta una nueva petición
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 23-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Lnk_Nueva_Peticion_Click(object sender, EventArgs e)
    {
        // limpiar mensajes de error
        Mostrar_Informacion("", false);

        try
        {
            Habilitar_Botones("Nuevo");
            Limpiar_Campos();
            Habilitar_Campos(true);
            Cargar_Datos_Constribuyente();
            // fecha actual en campo fecha_peticion
            Txt_Fecha_Peticion.Text = DateTime.Now.ToString("dd/MMM/2012");
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion(Ex.Message, true);
        }
    }

    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.ToolTip == "Inicio")
        {
            Session.Remove("Dt_Seguimiento");
            Session.Remove("Dt_Observaciones");
            Response.Redirect("Frm_Apl_Ventanilla.aspx");
        }
        else
        {
            Mostrar_Informacion("", false);
            Habilitar_Botones("Inicial");
            Limpiar_Campos();
            Habilitar_Campos(false);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Buscar_Registro_Peticion_Click
    ///DESCRIPCIÓN: Manejo del evento clic en el botón de búsqueda: llama al método que realiza la búsqueda de peticiones
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 18-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Buscar_Registro_Peticion_Click(object sender, ImageClickEventArgs e)
    {
        Mostrar_Informacion("", false);

        try
        {
            //Verifica si existe un folio para buscar
            if (Txt_Busqueda_Registro_Peticion.Text.Trim() == "")
            {
                Mostrar_Informacion("Ingrese número de folio para hacer la búsqueda, ej. PT-0012431234", true);
            }
            else
            {
                Consultar_Peticiones();
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion(Ex.Message, true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cmb_Colonia_SelectedIndexChanged
    ///DESCRIPCIÓN: Si se selecciona una colonia se actualiza el combo de calles de la colonia seleccionada
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cmb_Colonia_SelectedIndexChanged(object sender, EventArgs e)
    {
        var Obj_Calles = new Cls_Cat_Pre_Calles_Negocio();

        try
        {
            Cmb_Calle.Items.Clear();
            // cargar combo calles si hay una colonia seleccionada
            if (Cmb_Colonia.SelectedIndex > 0)
            {
                Obj_Calles.P_Colonia_ID = Cmb_Colonia.SelectedValue;
                Llenar_Combo_Con_DataTable(Cmb_Calle, Obj_Calles.Consultar_Calles(), 0, 5);
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("No se pudo mostrar información: " + Ex.Message, true);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Calles_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la calle seleccionada en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Calles_Click(object sender, ImageClickEventArgs e)
    {
        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_COLONIAS_CALLES"] != null)
        {
            // si el valor de la sesión es igual a true
            if (Convert.ToBoolean(Session["BUSQUEDA_COLONIAS_CALLES"]) == true)
            {
                var Obj_Calles = new Cls_Cat_Pre_Calles_Negocio();

                try
                {
                    string Calle_ID = Session["CALLE_ID"].ToString().Replace("&nbsp;", "");
                    string Colonia_ID = Session["COLONIA_ID"].ToString().Replace("&nbsp;", "");
                    // consultar las calles de la colonia a la que pertenece la calle seleccionada
                    Cmb_Calle.Items.Clear();
                    Obj_Calles.P_Colonia_ID = Colonia_ID;
                    Llenar_Combo_Con_DataTable(Cmb_Calle, Obj_Calles.Consultar_Calles(), 0, 5);
                    // si el combo colonias contiene la colonia con el ID, seleccionar
                    if (Cmb_Colonia.Items.FindByValue(Colonia_ID) != null)
                    {
                        Cmb_Colonia.SelectedValue = Colonia_ID;
                    }
                    // si el combo calles contiene un elemento con el ID, seleccionar
                    if (Cmb_Calle.Items.FindByValue(Calle_ID) != null)
                    {
                        Cmb_Calle.SelectedValue = Calle_ID;
                    }
                }
                catch (Exception Ex)
                {
                    Mostrar_Informacion("No se pudo mostrar información: " + Ex.Message, true);
                }

                // limpiar variables de sesión
                Session.Remove("COLONIA_ID");
                Session.Remove("CLAVE_COLONIA");
                Session.Remove("CALLE_ID");
                Session.Remove("CLAVE_CALLE");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_COLONIAS_CALLES");
        }

    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Colonia_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la colonia seleccionada en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Colonia_Click(object sender, ImageClickEventArgs e)
    {
        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_COLONIAS"] != null)
        {
            // si el valor de la sesión es igual a true
            if (Convert.ToBoolean(Session["BUSQUEDA_COLONIAS"]) == true)
            {
                try
                {
                    string Colonia_ID = Session["COLONIA_ID"].ToString().Replace("&nbsp;", "");
                    // si el combo colonias contiene la colonia con el ID, seleccionar
                    if (Cmb_Colonia.Items.FindByValue(Colonia_ID) != null)
                    {
                        Cmb_Colonia.SelectedValue = Colonia_ID;
                        Cmb_Colonia_SelectedIndexChanged(null, null);
                    }
                }
                catch (Exception Ex)
                {
                    Mostrar_Informacion(Ex.Message, true);
                }

                // limpiar variables de sesión
                Session.Remove("COLONIA_ID");
                Session.Remove("NOMBRE_COLONIA");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_COLONIAS");
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Grid_Peticiones_Sorting
    ///DESCRIPCIÓN: Maneja el evento cambio del orden del grid, toma los datos de variables de sesión
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 18-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Peticiones_Sorting(object sender, GridViewSortEventArgs e)
    {
        Mostrar_Informacion("", false);

        if (Session["Dt_Peticiones"] != null)
        {

            DataTable Dt_Peticiones = (DataTable)Session["Dt_Peticiones"];
            String Orden = ViewState["SortDirection"].ToString();
            if (Orden.Equals("ASC"))
            {
                Cargar_Datos_Grid_Peticiones(Dt_Peticiones, e.SortExpression + " DESC");
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Cargar_Datos_Grid_Peticiones(Dt_Peticiones, e.SortExpression + " ASC");
                ViewState["SortDirection"] = "ASC";
            }

        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Grid_Peticiones_SelectedIndexChanged
    ///DESCRIPCIÓN: Maneja el evento cambio índice del grid, llama al método que carga los detalles de una petición en la página
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 18-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Peticiones_SelectedIndexChanged(object sender, EventArgs e)
    {
        Mostrar_Informacion("", false);
        int Anio_Peticion;

        try
        {
            //si hay una fila seleccionada, llamar al método que carga los datos en la página
            if (Grid_Peticiones.SelectedIndex > -1)
            {
                Limpiar_Campos();
                Habilitar_Botones("Visualizar");
                Habilitar_Campos(false);
                int.TryParse(Grid_Peticiones.SelectedRow.Cells[2].Text, out Anio_Peticion);
                Cargar_Datos_Peticion(Grid_Peticiones.SelectedRow.Cells[1].Text, Anio_Peticion, Grid_Peticiones.SelectedRow.Cells[3].Text);
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion(Ex.Message, true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Grid_Observaciones_Sorting
    ///DESCRIPCIÓN: Maneja el evento cambio del orden del grid, toma los datos de variables de sesión
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 23-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Observaciones_Sorting(object sender, GridViewSortEventArgs e)
    {
        Mostrar_Informacion("", false);

        try
        {
            if (Session["Dt_Observaciones"] != null)
            {
                DataTable Dt_Observaciones = (DataTable)Session["Dt_Observaciones"];
                String Orden = ViewState["SortDirection"].ToString();
                if (Orden.Equals("ASC"))
                {
                    Dt_Observaciones.DefaultView.Sort = e.SortExpression + " DESC";
                    Grid_Observaciones.DataSource = Dt_Observaciones;
                    Grid_Observaciones.DataBind();
                    ViewState["SortDirection"] = "DESC";
                }
                else
                {
                    Dt_Observaciones.DefaultView.Sort = e.SortExpression + " ASC";
                    Grid_Observaciones.DataSource = Dt_Observaciones;
                    Grid_Observaciones.DataBind();
                    ViewState["SortDirection"] = "ASC";
                }
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion(Ex.Message, true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Grid_Seguimiento_Sorting
    ///DESCRIPCIÓN: Maneja el evento cambio del orden del grid, toma los datos de variables de sesión
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 18-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Seguimiento_Sorting(object sender, GridViewSortEventArgs e)
    {
        Mostrar_Informacion("", false);

        try
        {
            if (Session["Dt_Seguimiento"] != null)
            {
                DataTable Dt_Seguimiento = (DataTable)Session["Dt_Seguimiento"];
                String Orden = ViewState["SortDirection"].ToString();
                if (Orden.Equals("ASC"))
                {
                    Dt_Seguimiento.DefaultView.Sort = e.SortExpression + " DESC";
                    Grid_Seguimiento.DataSource = Dt_Seguimiento;
                    Grid_Seguimiento.DataBind();
                    ViewState["SortDirection"] = "DESC";
                }
                else
                {
                    Dt_Seguimiento.DefaultView.Sort = e.SortExpression + " ASC";
                    Grid_Seguimiento.DataSource = Dt_Seguimiento;
                    Grid_Seguimiento.DataBind();
                    ViewState["SortDirection"] = "ASC";
                }
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion(Ex.Message, true);
        }
    }

    #endregion Eventos

}
