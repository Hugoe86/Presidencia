using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;
using System.Text;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Colonias.Negocios;
using Presidencia.Acciones_AC.Negocio;
using Presidencia.Asuntos_AC.Negocio;
using Presidencia.Programas_AC.Negocio;
using Presidencia.Catalogo_Calles.Negocio;
using Presidencia.Registro_Peticion.Negocios;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;
using Presidencia.Ventanilla_Usarios.Negocio;
using Presidencia.Catalogo_Atencion_Ciudadana_Parametros.Negocio;
using Presidencia.Catalogo_Atencion_Ciudadana_Organigrama.Negocio;
using DocumentFormat.OpenXml.Packaging;
using openXML_Wp = DocumentFormat.OpenXml.Wordprocessing;
using Presidencia.Operacion_Atencion_Ciudadana_Registro_Correos_Enviados.Negocio;
using Presidencia.Catalogo_Atencion_Ciudadana_Parametros_Correo.Negocio;
using System.Net.Mail;
using System.Collections.Generic;

public partial class paginas_Atencion_Ciudadana_Frm_Ope_Registro_Peticion : System.Web.UI.Page
{
    #region Variables Locales
    private int Contador_Columna;
    private String Informacion;
    private const string Directorio_Imagenes_Correo = "~/Archivos/Atencion_Ciudadana/Imagenes/";
    #endregion
    #region PageLoad/Init
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(Btn_Formato_Consecutivo);

        Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 100000) + 5));
        if (string.IsNullOrEmpty(Cls_Sessiones.Empleado_ID))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
        }

        if (!Page.IsPostBack)
        {
            Cls_Sessiones.Mostrar_Menu = true;
            Habilitar_Botones("Inicial");
            Limpiar_Campos();
            Habilitar_Campos(false);
            LLenar_Combos();
            //Consultar_Peticiones();

            // revisar si hay una cookie con el número de peticiones a mostrar
            if (Request.Cookies["Numero_Peticiones_Recientes"] != null)
            {
                if (Cmb_Cantidad_Peticiones_Mostrar.Items.FindByValue(Request.Cookies["Numero_Peticiones_Recientes"].Value) != null)
                {
                    Cmb_Cantidad_Peticiones_Mostrar.SelectedValue = Request.Cookies["Numero_Peticiones_Recientes"].Value;
                }
            }

            // registro de scripts del lado del servidor para mostrar ventanas emergentes para búsqueda avanzada
            string Ventana_Modal = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias_Calles.aspx', 'center:yes;resizable:no;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Calle.Attributes.Add("onclick", Ventana_Modal);
            Ventana_Modal = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Colonia.Attributes.Add("onclick", Ventana_Modal);
            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergente/Frm_Busqueda_Avanzada_Asuntos.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Asunto.Attributes.Add("onclick", Ventana_Modal);
            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergente/Frm_Busqueda_Avanzada_Acciones.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Accion.Attributes.Add("onclick", Ventana_Modal);
            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergente/Frm_Busqueda_Avanzada_Dependencias.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Dependencia.Attributes.Add("onclick", Ventana_Modal);
            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergente/Frm_Ate_Busqueda_Avanzada_Ciudadano.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Ciudadano.Attributes.Add("onclick", Ventana_Modal);
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
        //Se selección el tipo de valor a validar
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
                Str_Expresion = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})+$";
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
        // Verifica que los campos estén seleccionados o tengan valor
        if (Cmb_Origen.SelectedIndex <= 0)
        {
            Informacion += "<td>+Campo Origen. </td>";
            Bln_Bandera = false;
            Generar_Tabla_Informacion();
        }
        if (Txt_Fecha_Solucion.Text.Trim() == String.Empty)
        {
            Informacion += "<td>+Fecha probable de solución.</td>";
            Bln_Bandera = false;
            Generar_Tabla_Informacion();
        }
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
        if (Cmb_Sexo.SelectedIndex <= 0)
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
            if (Cmb_Colonia.SelectedIndex <= 0)
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
            if (Validar_Longitud(Txt_Peticion, 4001))
            {
                Lbl_Mensaje.Text = String.Empty;
                Informacion += "<td>+Campo Petición excede la longitud permitida.</td>";
                Bln_Bandera = false;
                Generar_Tabla_Informacion();
            }
        }
        if (Cmb_Origen.SelectedIndex <= 0)
        {
            Informacion += "<td>+Campo Origen.</td>";
            Bln_Bandera = false;
            Generar_Tabla_Informacion();
        }
        if (Cmb_Dependencia.SelectedIndex <= 0)
        {
            Informacion += "<td>+Campo Dependencia.</td>";
            Bln_Bandera = false;
            Generar_Tabla_Informacion();
        }
        if (Cmb_Asunto.SelectedIndex <= 0)
        {
            Informacion += "<td>+Campo Tipo de Asunto.</td>";
            Bln_Bandera = false;
            Generar_Tabla_Informacion();
        }
        if (Cmb_Accion.SelectedIndex <= 0)
        {
            Informacion += "<td>+Campo Tipo de Acción.</td>";
            Bln_Bandera = false;
            Generar_Tabla_Informacion();
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
        // si el checkbox terminar salicitud está habilitado, validar estatus y descripción
        if (Chk_Terminar_Peticion.Checked == true)
        {
            if (Cmb_Tipo_Solucion.SelectedIndex <= 0)
            {
                Informacion += "<td>+Tipo de solución.</td>";
                Bln_Bandera = false;
                Generar_Tabla_Informacion();
            }
            if (Txt_Descripcion_Solucion.Text.Trim().Length <= 0)
            {
                Informacion += "<td>+Descripción de la solución.</td>";
                Bln_Bandera = false;
                Generar_Tabla_Informacion();
            }
        }
        // si está visible el combo seguimiento consecutivo, validar selección
        if (Tr_Contenedor_Combo_Consecutivo.Visible == true)
        {
            if (Cmb_Seguimiento_Consecutivo.SelectedIndex <= 0)
            {
                Informacion += "<td>+Campo Seguimiento consecutivo.</td>";
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
        Cmb_Origen.Enabled = Habilitar;
        Txt_Nombre.Enabled = Habilitar;
        Txt_Apellido_Paterno.Enabled = Habilitar;
        Txt_Apellido_Materno.Enabled = Habilitar;
        Txt_Edad.Enabled = Habilitar;
        Txt_Fecha_Nacimiento.Enabled = Habilitar;
        Btn_Cln_Txt_Fecha_Nacimiento.Enabled = Habilitar;
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
        Txt_Peticion.Enabled = Habilitar;
        Cmb_Asunto.Enabled = Habilitar;
        Cmb_Accion.Enabled = Habilitar;
        Cmb_Dependencia.Enabled = Habilitar;
        Chk_Terminar_Peticion.Enabled = Habilitar;
        Cmb_Seguimiento_Consecutivo.Enabled = Habilitar;

        Btn_Buscar_Accion.Enabled = Habilitar;
        Btn_Buscar_Asunto.Enabled = Habilitar;
        Btn_Buscar_Dependencia.Enabled = Habilitar;
        Btn_Buscar_Calle.Enabled = Habilitar;
        Btn_Buscar_Colonia.Enabled = Habilitar;
        Btn_Buscar_Ciudadano.Enabled = Habilitar;
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
        Cmb_Origen.SelectedIndex = 0;
        Txt_Nombre.Text = String.Empty;
        Txt_Apellido_Paterno.Text = String.Empty;
        Txt_Apellido_Materno.Text = String.Empty;
        Txt_Edad.Text = String.Empty;
        Txt_Fecha_Nacimiento.Text = String.Empty;
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
        Txt_Peticion.Text = String.Empty;
        Cmb_Dependencia.SelectedIndex = 0;
        Cmb_Asunto.SelectedIndex = 0;
        Cmb_Accion.SelectedIndex = 0;
        Chk_Terminar_Peticion.Checked = false;
        Cmb_Tipo_Solucion.SelectedIndex = 0;
        Cmb_Seguimiento_Consecutivo.SelectedIndex = -1;
        Txt_Descripcion_Solucion.Text = "";
        Cargar_Datos_Grid(Grid_Peticiones_Recientes, null, null);

        Hdn_Estatus.Value = "";
        Hdn_Ciudadano_Id.Value = "";
        Hdn_Atendio.Value = "";
        Hdn_Programa_Atendido_Direcciones.Value = "";
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Limpiar_Informacion_Personal
    ///DESCRIPCIÓN: Limpiar las cajas de texto y quitar la selección de los combos de la sección de información personal
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 13-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Limpiar_Informacion_Personal()
    {
        //Se limpian los campos
        Txt_Nombre.Text = String.Empty;
        Txt_Apellido_Paterno.Text = String.Empty;
        Txt_Apellido_Materno.Text = String.Empty;
        Txt_Edad.Text = String.Empty;
        Txt_Fecha_Nacimiento.Text = String.Empty;
        Cmb_Sexo.SelectedIndex = -1;
        Cmb_Calle.Items.Clear();
        Txt_Numero_Exterior.Text = String.Empty;
        Txt_Numero_Interior.Text = String.Empty;
        Txt_Referencia.Text = String.Empty;
        Cmb_Colonia.SelectedIndex = 0;
        Txt_Codigo_Postal.Text = String.Empty;
        Txt_Telefono.Text = String.Empty;
        Txt_Email.Text = String.Empty;

        Hdn_Estatus.Value = "";
        Hdn_Ciudadano_Id.Value = "";
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
        // ordenar elementos de la tabla
        Dt_Temporal.DefaultView.Sort = Dt_Temporal.Columns[1].ColumnName + " ASC";

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
        // ordenar elementos de la tabla
        Dt_Temporal.DefaultView.Sort = Dt_Temporal.Columns[Indice_Campo_Valor].ColumnName + " ASC";

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
        var Obj_Dependencias = new Cls_Cat_Ate_Peticiones_Negocio();
        var Obj_Colonias = new Cls_Cat_Ate_Colonias_Negocio();
        var Obj_Programas = new Cls_Cat_Ate_Programas_Negocio();
        var Obj_Asunto = new Cls_Cat_Ate_Asuntos_Negocio();
        var Obj_Acciones = new Cls_Cat_Ate_Acciones_Negocio();

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
            // Combo de Programas
            Obj_Programas.P_Estatus = "ACTIVO";
            Llenar_Combo_Con_DataTable(Cmb_Origen, Obj_Programas.Consultar_Registros(), 0, 2);
            // Combo de Colonia
            Obj_Colonias.P_Filtros_Dinamicos = Cat_Ate_Colonias.Campo_Estatus + " LIKE '%VIGENTE%'";
            Llenar_Combo_Con_DataTable(Cmb_Colonia, Obj_Colonias.Consulta_Datos().Tables[0]);
            // Combo de Dependencia
            Obj_Dependencias.P_Estatus = "ACTIVO";
            Llenar_Combo_Con_DataTable(Cmb_Dependencia, Obj_Dependencias.Consulta_Dependencias_Con_Asunto());
            // Combo de Asunto
            Obj_Asunto.P_Estatus = "ACTIVO";
            Llenar_Combo_Con_DataTable(Cmb_Asunto, Obj_Asunto.Consultar_Registros());
            // Combo de Acciones
            Obj_Acciones.P_Estatus = "ACTIVO";
            Llenar_Combo_Con_DataTable(Cmb_Accion, Obj_Acciones.Consultar_Registros(), 0, 2);
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("No se pudo mostrar información: " + Ex, true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Habilitar_Botones
    ///DESCRIPCIÓN: Establece las propiedades de los controles de búsqueda y botones nuevo, modificar y salir 
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
            //Estado Incicial de los controles
            case "Inicial":
                Btn_Nuevo.Visible = true;
                Btn_Modificar.Visible = true;
                Btn_Salir.Visible = true;
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Salir.ToolTip = "Inicio";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Txt_Busqueda_Registro_Peticion.Text = String.Empty;
                Txt_Busqueda_Registro_Peticion.Enabled = true;
                Btn_Buscar_Registro_Peticion.Enabled = true;
                Grid_Peticiones.Enabled = true;
                Div_Contenedor_Grid_Peticiones_Recientes.Style.Value = "clear: both; display:none;";
                break;
            //Estado de Nuevo
            case "Nuevo":
                Txt_Apellido_Paterno.Focus();
                Btn_Nuevo.ToolTip = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Modificar.Visible = false;
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Txt_Busqueda_Registro_Peticion.Text = String.Empty;
                Txt_Busqueda_Registro_Peticion.Enabled = false;
                Btn_Buscar_Registro_Peticion.Enabled = false;
                Grid_Peticiones.Enabled = false;
                Div_Contenedor_Grid_Peticiones_Recientes.Style.Value = "clear: both; display:block;";
                break;
            //Estado de Modificar
            case "Modificar":
                Btn_Nuevo.Visible = false;
                Btn_Modificar.ToolTip = "Actualizar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Txt_Busqueda_Registro_Peticion.Text = String.Empty;
                Txt_Busqueda_Registro_Peticion.Enabled = false;
                Btn_Buscar_Registro_Peticion.Enabled = false;
                Grid_Peticiones.Enabled = false;
                Div_Contenedor_Grid_Peticiones_Recientes.Style.Value = "clear: both; display:none;";
                break;
        }
        Txt_Descripcion_Solucion.Enabled = false;
        Cmb_Tipo_Solucion.Enabled = false;

        Btn_Imprimir.Visible = false;
        Btn_Imprimir.Enabled = false;
        Grid_Peticiones_Recientes.SelectedIndex = -1;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Registra_Datos_Negocio
    ///DESCRIPCIÓN: Asigna las propiedades del objeto recibido como parámetro con la selección en 
    ///             los controles del formulario en la página
    ///PARÁMETROS:
    /// 		1. Obj_Peticiones: instancia de la clase de negocio en la que se van a asignar los datos seleccionados en la página
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Registra_Datos_Negocio(Cls_Cat_Ate_Peticiones_Negocio Obj_Peticiones)
    {
        DateTime Fecha_Solucion;
        DateTime Fecha_Nacimiento;

        Obj_Peticiones.P_Programa_ID = Cmb_Origen.SelectedValue;
        Obj_Peticiones.P_Origen = Cmb_Origen.SelectedItem.Text;
        Obj_Peticiones.P_Nombre = Txt_Nombre.Text;
        Obj_Peticiones.P_Apellido_Paterno = Txt_Apellido_Paterno.Text;
        Obj_Peticiones.P_Apellido_Materno = Txt_Apellido_Materno.Text;
        // si hay texto en el campo edad convertir a número
        if (Txt_Edad.Text.Trim() != String.Empty)
        {
            Obj_Peticiones.P_Edad = int.Parse(Txt_Edad.Text);
        }
        // validar que haya una fecha de nacimiento
        if (DateTime.TryParse(Txt_Fecha_Nacimiento.Text.Trim(), out Fecha_Nacimiento))
        {
            Obj_Peticiones.P_Fecha_Nacimiento = Fecha_Nacimiento;
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
        Obj_Peticiones.P_Usuario_ID = Hdn_Ciudadano_Id.Value;
        Obj_Peticiones.P_Nombre_Atendio = Hdn_Atendio.Value;

        if (Txt_Peticion.Text.Length <= 4000)
        {
            Obj_Peticiones.P_Peticion = Txt_Peticion.Text;
        }
        else
        {
            Obj_Peticiones.P_Peticion = Txt_Peticion.Text.Substring(0, 4000);
        }
        // si el control para consecutivos está visible y hay un dato seleccionado, agregarlo
        if (Tr_Contenedor_Combo_Consecutivo.Visible == true && Cmb_Seguimiento_Consecutivo.SelectedIndex > 0)
        {
            Obj_Peticiones.P_Tipo_Consecutivo = Cmb_Seguimiento_Consecutivo.SelectedValue;
        }
        else
        {
            Obj_Peticiones.P_Tipo_Consecutivo = "";
        }

        Obj_Peticiones.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
        Obj_Peticiones.P_Asunto_ID = Cmb_Asunto.SelectedValue;
        Obj_Peticiones.P_Accion_ID = Cmb_Accion.SelectedValue;
        // si terminar petición no está seleccionado, asignar estatus EN PROCESO
        if (Chk_Terminar_Peticion.Checked == false)
        {
            Obj_Peticiones.P_Estatus = "EN PROCESO";
        }
        else
        {
            Obj_Peticiones.P_Estatus = "TERMINADA";
            Obj_Peticiones.P_Tipo_Solucion = Cmb_Tipo_Solucion.SelectedValue;
            Obj_Peticiones.P_Descripcion_Solucion = Txt_Descripcion_Solucion.Text.Trim();
        }
        Obj_Peticiones.P_Asignado = "S";
        Obj_Peticiones.P_Anio_Peticion = DateTime.Now.Year;
        Obj_Peticiones.P_Fecha_Peticion = DateTime.Now.ToString("dd/MM/yyyy");

        DateTime.TryParse(Txt_Fecha_Solucion.Text, out Fecha_Solucion);
        if (Fecha_Solucion != DateTime.MinValue)
        {
            Obj_Peticiones.P_Fecha_Solucion_Probable = Fecha_Solucion.ToString("dd/MM/yyyy");
        }
        Obj_Peticiones.P_Usuario_Creo_Modifico = Cls_Sessiones.Nombre_Empleado;

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
        Cls_Cat_Ate_Parametros_Correo_Negocio Neg_Consulta_Parametros = new Cls_Cat_Ate_Parametros_Correo_Negocio();
        string Domicilio;
        int Cantidad_Filas_Insertadas = 0;
        string Folio = "";

        // llamar al método que consulta los parámetros pasando como tipo de correo NOTIFICACION
        Neg_Consulta_Parametros.P_Tipo_Correo = "NOTIFICACION";
        Neg_Consulta_Parametros.Consultar_Parametros_Correo();

        try
        {
            // obtener el domicilio completo del solicitante para la impresión del formato (porque se limpian los campos con el domicilio antes de mandar la impresión)
            Domicilio = Obtener_Domicilio_Completo_Solicitante();
            string Nombre_Solicitante = Txt_Apellido_Paterno.Text + " " + Txt_Apellido_Materno.Text + " " + Txt_Nombre.Text;
            string Fecha = DateTime.Now.ToString("dd/MMM/yyyy");
            string Email = Txt_Email.Text;
            string Peticion = Txt_Peticion.Text;
            string Telefono = Txt_Telefono.Text;
            string Asunto = Cmb_Asunto.SelectedIndex > 0 ? Cmb_Asunto.SelectedItem.Text : "";
            string Accion = Cmb_Accion.SelectedIndex > 0 ? Cmb_Accion.SelectedItem.Text : "";
            string Dependencia = Cmb_Dependencia.SelectedIndex > 0 ? Cmb_Dependencia.SelectedItem.Text : "";
            string Descripcion_Solucion = Txt_Descripcion_Solucion.Text;
            string Ciudadano_Id = Hdn_Ciudadano_Id.Value;
            string Nombre_Atendio = Hdn_Atendio.Value;
            // se asigna la variable boleana bool_Enviar_Correo, debe ser una petición terminada con estatus positivo, debe haber un correo y parámetros para envío (servidor y usuario)
            bool bool_Enviar_Correo = Chk_Terminar_Peticion.Checked == true && Cmb_Tipo_Solucion.SelectedValue == "POSITIVA"
                && Txt_Email.Text.Length > 0 && Neg_Consulta_Parametros.P_Correo_Servidor.Length > 0 && Neg_Consulta_Parametros.P_Correo_Remitente.Length > 0;

            Registra_Datos_Negocio(Obj_Peticiones);
            Cantidad_Filas_Insertadas = Obj_Peticiones.Alta_Peticion();
            if (Cantidad_Filas_Insertadas > 0)
            {
                Folio = Obj_Peticiones.P_Folio;
                Txt_Folio.Text = Folio;
                // si el combo Seguimiento consecutivo no está visible, imprimir folio
                if (Tr_Contenedor_Combo_Consecutivo.Visible == false)
                {
                    Habilitar_Botones("Inicial");
                    Limpiar_Campos();
                    Habilitar_Campos(false);
                    // si la petición tiene estatus TERMINADO y tipo de solución positiva, enviar correo
                    if (bool_Enviar_Correo == true)
                    {
                        // llamar método de envío de correos
                        Enviar_Correo(Neg_Consulta_Parametros, Email, Nombre_Solicitante, Folio, Domicilio, Peticion, Descripcion_Solucion, Fecha, Ciudadano_Id);
                    }
                    Imprimir_Reporte(Crear_Ds_Imprimir_Reporte(Folio, Fecha, Nombre_Solicitante, Domicilio, Email, Peticion, Telefono, Asunto, Accion, Dependencia, Nombre_Atendio), "Rpt_Ope_Ate_Folio_Peticion.rpt", "Folio_Reporte_Ciudadano_" + Folio);
                }
                else
                {
                    Habilitar_Campos(false);
                    Btn_Formato_Consecutivo.Visible = true;
                    Btn_Nuevo.Visible = false;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('La Petición con folio: " + Folio + ", fue registrada.');", true);
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
    ///NOMBRE_FUNCIÓN: Modificar_Peticion
    ///DESCRIPCIÓN: llama a los métodos para actualizar una petición con los datos en la página 
    ///             y muestra un mensaje si la operación se realizó con éxito
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Modificar_Peticion()
    {
        Cls_Cat_Ate_Peticiones_Negocio Obj_Peticiones = Obj_Peticiones = new Cls_Cat_Ate_Peticiones_Negocio();
        Cls_Cat_Ate_Parametros_Correo_Negocio Neg_Consulta_Parametros = new Cls_Cat_Ate_Parametros_Correo_Negocio();

        // llamar al método que consulta los parámetros pasando como tipo de correo NOTIFICACION
        Neg_Consulta_Parametros.P_Tipo_Correo = "NOTIFICACION";
        Neg_Consulta_Parametros.Consultar_Parametros_Correo();

        try
        {
            Registra_Datos_Negocio(Obj_Peticiones);
            Obj_Peticiones.P_Folio = Txt_Folio.Text;
            if (Obj_Peticiones.Modificar_Peticion() > 0)
            {
                // llamar método para terminar peticion
                if (Chk_Terminar_Peticion.Checked == true)
                {
                    Obj_Peticiones.Modificar_Peticion_Solucion();
                }
                // obtener el domicilio completo del solicitante para la impresión del formato (porque se limpian los campos con el domicilio antes de mandar la impresión)
                string Domicilio = Obtener_Domicilio_Completo_Solicitante();
                string Nombre_Solicitante = Txt_Apellido_Paterno.Text + " " + Txt_Apellido_Materno.Text + " " + Txt_Nombre.Text;
                string Fecha = Txt_Fecha_Peticion.Text;
                string Email = Txt_Email.Text;
                string Peticion = Txt_Peticion.Text;
                string Telefono = Txt_Telefono.Text;
                string Asunto = Cmb_Asunto.SelectedIndex > 0 ? Cmb_Asunto.SelectedItem.Text : "";
                string Accion = Cmb_Accion.SelectedIndex > 0 ? Cmb_Accion.SelectedItem.Text : "";
                string Dependencia = Cmb_Dependencia.SelectedIndex > 0 ? Cmb_Dependencia.SelectedItem.Text : "";
                string Folio = Txt_Folio.Text;
                string Descripcion_Solucion = Txt_Descripcion_Solucion.Text;
                string Ciudadano_Id = Hdn_Ciudadano_Id.Value;
                string Nombre_Atendio = Hdn_Atendio.Value;
                // se asigna la variable boleana bool_Enviar_Correo, debe ser una petición terminada con estatus positivo, debe haber un correo y parámetros para envío (servidor y usuario)
                bool bool_Enviar_Correo = Chk_Terminar_Peticion.Checked == true && Cmb_Tipo_Solucion.SelectedValue == "POSITIVA"
                    && Txt_Email.Text.Length > 0 && Neg_Consulta_Parametros.P_Correo_Servidor.Length > 0 && Neg_Consulta_Parametros.P_Correo_Remitente.Length > 0;

                // si el combo Seguimiento consecutivo no está visible, imprimir folio
                if (Tr_Contenedor_Combo_Consecutivo.Visible == false)
                {
                    Habilitar_Botones("Inicial");
                    Limpiar_Campos();
                    Habilitar_Campos(false);
                    // si la petición tiene estatus TERMINADO, con tipo de solución positiva y un correo electrónico válido, enviar correo
                    if (bool_Enviar_Correo == true)
                    {
                        Enviar_Correo(Neg_Consulta_Parametros, Email, Nombre_Solicitante, Folio, Domicilio, Peticion, Descripcion_Solucion, Fecha, Ciudadano_Id);
                    }
                    Imprimir_Reporte(Crear_Ds_Imprimir_Reporte(Folio, Fecha, Nombre_Solicitante, Domicilio, Email, Peticion, Telefono, Asunto, Accion, Dependencia, Nombre_Atendio), "Rpt_Ope_Ate_Folio_Peticion.rpt", "Folio_Reporte_Ciudadano_" + Folio);
                }
                else
                {
                    Habilitar_Campos(false);
                    Btn_Formato_Consecutivo.Visible = true;
                    Btn_Modificar.Visible = false;
                }
                // mostrar mensaje
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('La Petición con folio: " + Folio + ", fue actualizada.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('No fue posible actualizar la Petición.');", true);
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("La petición no pudo ser modificada: " + Ex.Message, true);
        }
        finally
        {
            Obj_Peticiones = null;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Enviar_Correo
    ///DESCRIPCIÓN: Envia un correo al ciudadano con los parámetros recibidos
    ///PROPIEDADES: 
    ///             1. Neg_Parametros_Correo: parámetros para envío de correo
    ///             2. Email_Destinatario: direccción de correo electrónico a la que se envía la notificación
    /// 		    3. Nombre: nombre del ciudadano que solicita
    /// 		    4. Folio: folio asignado a la petición
    /// 		    5. Domicilio: domicilio dado para la petición
    /// 		    6. Peticion: descripción de la petición
    /// 		    7. Solucion: descripción de la solución dada a la petición
    /// 		    8. Fecha: fecha de la peticón para imprimir
    /// 		    9. Ciudadano_Id: id del ciudadano al que se envió el correo
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-oct-2012
    ///MODIFICO:
    ///FECHA_MODIFICO: 
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Enviar_Correo(Cls_Cat_Ate_Parametros_Correo_Negocio Neg_Parametros_Correo, string Email_Destinatario, string Nombre, string Folio, string Domicilio, string Peticion, string Solucion, string Fecha, string Ciudadano_Id)
    {
        Cls_Ope_Ate_Registro_Correo_Enviados_Negocio Neg_Alta_Registro_Correo = new Cls_Ope_Ate_Registro_Correo_Enviados_Negocio();
        MatchCollection Coincidencias_Encontradas;
        // expresión para encontrar el texto entre comillas precedido por cid:
        string Expresion = "\"cid:(.*)\"";
        List<string> Listado_Archivos;
        string Ruta_Directorio = Server.MapPath(Directorio_Imagenes_Correo);

        try
        {
            // validar que haya un Destinatario
            if (!string.IsNullOrEmpty(Email_Destinatario))
            {
                MailMessage Manejador_Correo = new MailMessage();
                string Cuerpo_Correo = Neg_Parametros_Correo.P_Correo_Cuerpo.Replace("[NOMBRE]", Nombre).Replace("[PETICION]", Peticion).Replace("[SOLUCION]", Solucion).Replace("[DOMICILIO]", Domicilio).Replace("[EMAIL]", Email_Destinatario).Replace("[FOLIO]", Folio).Replace("[FECHA_SOLUCION]", Fecha).Replace("[FECHA]", Fecha).Replace("[HOY]", DateTime.Today.ToString("d 'de' MMMM 'de' yyyy"));
                // agregar parámetros al correo
                Manejador_Correo.To.Add(Email_Destinatario);
                Manejador_Correo.From = new MailAddress(Neg_Parametros_Correo.P_Correo_Remitente, "Atención Ciudadana");
                Manejador_Correo.Subject = Neg_Parametros_Correo.P_Correo_Saludo;

                // crear vistas con el contenido del correo (una HTML y otra en texto plano para los clientes que visualizan sólo el texto)
                AlternateView Correo_HTML = AlternateView.CreateAlternateViewFromString(Cuerpo_Correo, null, "text/html");

                // obtener listado de archivos del directorio con imagenes para correo
                Listado_Archivos = Directory.GetFiles(Ruta_Directorio, "*.*", SearchOption.TopDirectoryOnly).ToList<string>();
                // obtener un listado de recursos dentro del correo
                Coincidencias_Encontradas = Regex.Matches(Cuerpo_Correo, Expresion);
                foreach (Match Coincidencia in Coincidencias_Encontradas)
                {
                    // validar el conteo del grupos en la coincidencia
                    if (Coincidencia.Groups.Count > 0)
                    {
                        // recorrer el listado de archivos en el directorio hasta encontrar el recurso
                        foreach (string Archivo in Listado_Archivos)
                        {
                            // si el nombre del archivo es igual al del recurso en el cuerpo del correo, agregar como LinkedResource
                            if (Coincidencia.Groups[1].Value == Path.GetFileNameWithoutExtension(Archivo))
                            {
                                // agregar LinkedResource para insertar imagenes en el correo
                                LinkedResource Recurso_Adjunto = new LinkedResource(Archivo);
                                Recurso_Adjunto.ContentId = Coincidencia.Groups[1].Value;
                                Correo_HTML.LinkedResources.Add(Recurso_Adjunto);
                                break;
                            }
                        }

                    }
                }

                // agregar vistas con el cuerpo de correo a la instancia de la clase 
                Manejador_Correo.AlternateViews.Add(Correo_HTML);
                // crear instancia de cliente SMTP
                SmtpClient sc = new SmtpClient(Neg_Parametros_Correo.P_Correo_Servidor);
                // usuario y contraseña para el envío de correo
                sc.Credentials = new System.Net.NetworkCredential(Neg_Parametros_Correo.P_Correo_Remitente, Neg_Parametros_Correo.P_Password_Usuario_Correo);
                // enviar el correo
                sc.Send(Manejador_Correo);

                Manejador_Correo.Dispose();

                // guardar registro de correo enviado
                Neg_Alta_Registro_Correo.P_Destinatario = Email_Destinatario;
                Neg_Alta_Registro_Correo.P_Nombre_Contribuyente = Nombre;
                Neg_Alta_Registro_Correo.P_Motivo = "NOTIFICACION";
                Neg_Alta_Registro_Correo.P_Contribuyente_ID = Ciudadano_Id;
                Neg_Alta_Registro_Correo.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                Neg_Alta_Registro_Correo.Alta_Registro_Correo_Enviado();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al enviar correo: " + Ex.Message);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Generar_Documento_Pdf_Respuesta_Peticion
    ///DESCRIPCIÓN: generar el documento en formato pdf con los datos de la petición
    ///PARÁMETROS:
    /// 		1. Nombre: Nombre de solicitante
    /// 		2. Folio: folio de petición
    /// 		3. Domicilio: domicilio de petición
    /// 		4. Descripcion_Peticion: texto de la descripción de la petición
    /// 		5. Descripcion_Solucion: texto de la descripción de la solución
    /// 		6. Fecha_Peticion: fecha de la petición
    /// 		7. Nombre_Firma: texto para la firma
    /// 		8. Dependencia: nombre de la dependencia para el reporte
    /// 		9. Asunto: descripción del asunto asignado a la petición
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-oct-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private String Generar_Documento_Pdf_Respuesta_Peticion(
        string Nombre,
        string Folio,
        string Domicilio,
        string Descripcion_Peticion,
        string Descripcion_Solucion,
        string Fecha_Peticion,
        string Nombre_Firma,
        string Dependencia,
        string Asunto)
    {
        Mostrar_Informacion("", false);
        try
        {
            ReportDocument Reporte = new ReportDocument();
            string filePath = Server.MapPath("../Rpt/Atencion_Ciudadana/Rpt_Correo_Solucion.rpt");
            Reporte.Load(filePath);

            // formar tabla para reporte
            DataRow Fila;
            DataTable Temporal_Correo = new DataTable("Correo");
            Temporal_Correo.Columns.Add("NOMBRE", typeof(string));
            Temporal_Correo.Columns.Add("FOLIO", typeof(string));
            Temporal_Correo.Columns.Add("CALLE_NO", typeof(string));
            Temporal_Correo.Columns.Add("PETICION", typeof(string));
            Temporal_Correo.Columns.Add("DESCRIPCION_SOLUCION", typeof(string));
            Temporal_Correo.Columns.Add("FECHA_PETICION", typeof(string));
            Temporal_Correo.Columns.Add("USUARIO_MODIFICO", typeof(string));
            Temporal_Correo.Columns.Add("DEPENDENCIA", typeof(string));
            Temporal_Correo.Columns.Add("ASUNTO", typeof(string));

            // agregar datos a la tabla
            Fila = Temporal_Correo.NewRow();
            Fila["NOMBRE"] = Nombre;
            Fila["FOLIO"] = Folio;
            Fila["CALLE_NO"] = Domicilio;
            Fila["PETICION"] = Descripcion_Peticion;
            Fila["DESCRIPCION_SOLUCION"] = Descripcion_Solucion;
            Fila["FECHA_PETICION"] = Fecha_Peticion;
            Fila["USUARIO_MODIFICO"] = Nombre_Firma;
            Fila["DEPENDENCIA"] = Dependencia;
            Fila["ASUNTO"] = Asunto;
            Temporal_Correo.Rows.Add(Fila);
            Temporal_Correo.Dispose();

            Reporte.SetDataSource(Temporal_Correo);
            Reporte.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Server.MapPath("~\\Reporte").ToString() + "\\solucion_peticion_" + Folio + ".pdf");
            Reporte.Dispose();
            Temporal_Correo.Dispose();
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion(Ex.Message, true);
        }
        return Server.MapPath("~").ToString() + "\\Reporte\\solucion_peticion_" + Folio + ".pdf";
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

        if (Txt_Referencia.Text.Trim().Length > 0)
        {
            Domicilio = Txt_Referencia.Text.Trim() + " ";
        }
        // formar el domicilio
        if (Cmb_Calle.SelectedIndex > 0)
        {
            Domicilio += Cmb_Calle.SelectedItem.Text + " " + Txt_Numero_Exterior.Text + " " + Txt_Numero_Interior.Text;
        }
        // si hay una colonia seleccionada, agregar al domicilio
        if (Cmb_Colonia.SelectedIndex > 0)
        {
            Domicilio = Domicilio.Trim() + ", " + Cmb_Colonia.SelectedItem.Text;
        }

        return Domicilio;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Consultar_Peticiones
    ///DESCRIPCIÓN: Consulta las peticiones de acuerdo con el criterio especificado en el campo de búsqueda
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
            // si se especificó un folio, agregar al la propiedad para la consulta
            if (Txt_Busqueda_Registro_Peticion.Text.Trim() != "")
            {
                Obj_Peticiones.P_Filtros_Dinamicos = " UPPER(" + Ope_Ate_Peticiones.Campo_Folio + ") = UPPER('" + Txt_Busqueda_Registro_Peticion.Text.Trim() + "')";
            }
            Dt_Peticiones = Obj_Peticiones.Consulta_Peticion();

            // si la consulta regresó resultados, cargar datos devueltos, si no, mostrar mensajeLimpiar_Campos();
            if (Dt_Peticiones != null && Dt_Peticiones.Rows.Count > 0)
            {
                int Anio_Peticion;
                string No_Peticion = Dt_Peticiones.Rows[0][Ope_Ate_Peticiones.Campo_No_Peticion].ToString();
                string Programa_Id = Dt_Peticiones.Rows[0][Ope_Ate_Peticiones.Campo_Programa_ID].ToString();
                int.TryParse(Dt_Peticiones.Rows[0][Ope_Ate_Peticiones.Campo_Anio_Peticion].ToString(), out Anio_Peticion);
                // limpiar campos y cargar datos de la petición
                Limpiar_Campos();
                Cargar_Datos_Peticion(No_Peticion, Anio_Peticion, Programa_Id);
                // mostrar y habilitar botón imprimir folio
                // si hay un tipo de consecutivo seleccionado, mostrar boton para generar plantilla
                if (Cmb_Seguimiento_Consecutivo.SelectedIndex > 0)
                {
                    Btn_Imprimir.Visible = false;
                    Btn_Imprimir.Enabled = false;
                    Btn_Formato_Consecutivo.Visible = true;
                    Btn_Formato_Consecutivo.Enabled = true;
                }
                else
                {
                    Btn_Imprimir.Visible = true;
                    Btn_Imprimir.Enabled = true;
                    Btn_Formato_Consecutivo.Visible = false;
                    Btn_Formato_Consecutivo.Enabled = false;
                }
            }
            else
            {
                Limpiar_Campos();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('No se encontraron peticiones con el folio proporcionado.');", true);
            }

            //Cargar_Datos_Grid(Grid_Peticiones, Dt_Peticiones, "FECHA_PETICION DESC");

            //// almacenar datatable en variable de sesión
            //Session["Dt_Peticiones"] = Dt_Peticiones;
            //ViewState["SortDirection"] = "DESC";
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al consultar Peticiones: [" + Ex.Message + "]", true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Consultar_Peticiones_Recientes
    ///DESCRIPCIÓN: Consulta las últimas peticiones dadas de alta y las regresa en un datatable
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 20-sep-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public DataTable Consultar_Peticiones_Recientes()
    {
        var Obj_Peticiones = new Cls_Cat_Ate_Peticiones_Negocio();
        DataTable Dt_Peticiones = null;
        int Cantidad_Peticiones;

        try
        {
            Obj_Peticiones.P_Anio_Peticion = DateTime.Now.Year;
            if (Cmb_Origen.SelectedIndex > 0)
            {
                Obj_Peticiones.P_Programa_ID = Cmb_Origen.SelectedValue;
            }

            // recuperar el número de peticiones del combo, si no se puede, asignar un valor por defecto
            if (!int.TryParse(Cmb_Cantidad_Peticiones_Mostrar.SelectedValue, out Cantidad_Peticiones) || Cantidad_Peticiones <= 0)
            {
                Cantidad_Peticiones = 10;
            }

            // asignar parámetros y ejecutar consulta
            Obj_Peticiones.P_Cantidad_Peticiones_Consultar = Cantidad_Peticiones;
            Obj_Peticiones.P_Orden_Dinamico = Ope_Ate_Peticiones.Campo_Fecha_Creo + " DESC";
            Dt_Peticiones = Obj_Peticiones.Consulta_Peticion();

        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al consultar Peticiones recientes: [" + Ex.Message + "]", true);
        }
        return Dt_Peticiones;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cargar_Grid_Peticiones_Recientes
    ///DESCRIPCIÓN: Carga el datatable que regresa la llamada al método Consultar_Peticiones_Recientes 
    ///             en el grid peticiones recientes
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 21-sep-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public DataTable Cargar_Grid_Peticiones_Recientes()
    {
        DataTable Dt_Peticiones = null;
        try
        {
            Dt_Peticiones = Consultar_Peticiones_Recientes();
            Grid_Peticiones_Recientes.Columns[1].Visible = true;
            Grid_Peticiones_Recientes.Columns[2].Visible = true;
            Grid_Peticiones_Recientes.Columns[3].Visible = true;
            Grid_Peticiones_Recientes.DataSource = Dt_Peticiones;
            Grid_Peticiones_Recientes.DataBind();
            Grid_Peticiones_Recientes.Columns[1].Visible = false;
            Grid_Peticiones_Recientes.Columns[2].Visible = false;
            Grid_Peticiones_Recientes.Columns[3].Visible = false;
            Grid_Peticiones_Recientes.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al consultar Peticiones recientes: [" + Ex.Message + "]", true);
        }
        return Dt_Peticiones;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cargar_Datos_Peticion
    ///DESCRIPCIÓN: Consulta los datos de la petición especificada y los muestra en los controles correspondientes
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
        try
        {
            var Obj_Peticiones = new Cls_Cat_Ate_Peticiones_Negocio();
            string Tipo_Solucion;
            int Edad_Ciudadano;
            DateTime Fecha_Nacimiento;

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
                // validar campo edad, si es un número mayor que 0 mostrar, si no, limpiar campo
                if (int.TryParse(Renglon[Ope_Ate_Peticiones.Campo_Edad].ToString(), out Edad_Ciudadano) && Edad_Ciudadano > 0)
                {
                    Txt_Edad.Text = Edad_Ciudadano.ToString();
                }
                else
                {
                    Txt_Edad.Text = "";
                }
                // validar campo fecha_nacimiento, si hay una fecha, cargar en el campo correspondiente
                if (DateTime.TryParse(Renglon[Ope_Ate_Peticiones.Campo_Fecha_Nacimiento].ToString(), out Fecha_Nacimiento) && Fecha_Nacimiento != DateTime.MinValue)
                {
                    Txt_Fecha_Nacimiento.Text = Fecha_Nacimiento.ToString("dd/MMM/yyyy");
                }
                else
                {
                    Txt_Fecha_Nacimiento.Text = "";
                }
                Txt_Numero_Exterior.Text = Renglon[Ope_Ate_Peticiones.Campo_Numero_Exterior].ToString();
                Txt_Numero_Interior.Text = Renglon[Ope_Ate_Peticiones.Campo_Numero_Interior].ToString();
                Txt_Referencia.Text = Renglon[Ope_Ate_Peticiones.Campo_Referencia].ToString();
                Txt_Codigo_Postal.Text = Renglon[Ope_Ate_Peticiones.Campo_Codigo_Postal].ToString();
                Txt_Telefono.Text = Renglon[Ope_Ate_Peticiones.Campo_Telefono].ToString();
                Txt_Email.Text = Renglon[Ope_Ate_Peticiones.Campo_Email].ToString();
                Txt_Peticion.Text = Renglon[Ope_Ate_Peticiones.Campo_Descripcion_Peticion].ToString();
                Hdn_Estatus.Value = Renglon[Ope_Ate_Peticiones.Campo_Estatus].ToString();
                Tipo_Solucion = Renglon[Ope_Ate_Peticiones.Campo_Tipo_Solucion].ToString();
                Txt_Descripcion_Solucion.Text = Renglon[Ope_Ate_Peticiones.Campo_Descripcion_Solucion].ToString();
                Hdn_Atendio.Value = Renglon[Ope_Ate_Peticiones.Campo_Nombre_Atendio].ToString();
                // si tipo de solución no es nulo, seleccionar en el combo (validar que existe en el combo)
                if (!string.IsNullOrEmpty(Tipo_Solucion) && Cmb_Tipo_Solucion.Items.FindByValue(Tipo_Solucion) != null)
                {
                    Cmb_Tipo_Solucion.SelectedValue = Tipo_Solucion;
                }
                // si el estatus es TERMINADA, marcar checkbox
                if (Hdn_Estatus.Value == "TERMINADA")
                {
                    Chk_Terminar_Peticion.Checked = true;
                }

                Hdn_Ciudadano_Id.Value = Renglon[Ope_Ate_Peticiones.Campo_Contribuyente_ID].ToString();
                DateTime Fecha_Solucion;
                DateTime.TryParse(Renglon[Ope_Ate_Peticiones.Campo_Fecha_Solucion_Probable].ToString(), out Fecha_Solucion);
                if (Fecha_Solucion != DateTime.MinValue)
                {
                    Txt_Fecha_Solucion.Text = Fecha_Solucion.ToString("dd/MMM/yyyy");
                }
                else
                {
                    Txt_Fecha_Solucion.Text = "";
                }
                DateTime.TryParse(Renglon[Ope_Ate_Peticiones.Campo_Fecha_Peticion].ToString(), out Fecha_Solucion);
                if (Fecha_Solucion != DateTime.MinValue)
                {
                    Txt_Fecha_Peticion.Text = Fecha_Solucion.ToString("dd/MMM/yyyy");
                }
                else
                {
                    Txt_Fecha_Peticion.Text = "";
                }

                // cargar los valores de los combos revisando primero que el valor a seleccionar exista entre los elementos del combo
                if (Cmb_Dependencia.Items.FindByValue(Renglon[Ope_Ate_Peticiones.Campo_Dependencia_ID].ToString()) != null)
                {
                    Cmb_Dependencia.SelectedValue = Renglon[Ope_Ate_Peticiones.Campo_Dependencia_ID].ToString();
                }
                if (Cmb_Asunto.Items.FindByValue(Renglon[Ope_Ate_Peticiones.Campo_Asunto_ID].ToString()) != null)
                {
                    Cmb_Asunto.SelectedValue = Renglon[Ope_Ate_Peticiones.Campo_Asunto_ID].ToString();
                }
                if (Cmb_Origen.Items.FindByValue(Renglon[Ope_Ate_Peticiones.Campo_Programa_ID].ToString()) != null)
                {
                    Cmb_Origen.SelectedValue = Renglon[Ope_Ate_Peticiones.Campo_Programa_ID].ToString();
                }
                if (Cmb_Accion.Items.FindByValue(Renglon[Ope_Ate_Peticiones.Campo_Accion_ID].ToString()) != null)
                {
                    Cmb_Accion.SelectedValue = Renglon[Ope_Ate_Peticiones.Campo_Accion_ID].ToString();
                }
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

                // llamar evento cambio de índice de combo origen para habilitar o deshabilitar impresión de consecutivos
                Cmb_Origen_SelectedIndexChanged(null, null);
                // deshabilitar combo seguimiento consecutivo
                Cmb_Seguimiento_Consecutivo.Enabled = false;

                // si hay un valor para el tipo de consecutivo, seleccionar
                if (!string.IsNullOrEmpty(Renglon[Ope_Ate_Peticiones.Campo_Tipo_Consecutivo].ToString()) && Cmb_Seguimiento_Consecutivo.Items.FindByValue(Renglon[Ope_Ate_Peticiones.Campo_Tipo_Consecutivo].ToString()) != null)
                {
                    Cmb_Seguimiento_Consecutivo.SelectedValue = Renglon[Ope_Ate_Peticiones.Campo_Tipo_Consecutivo].ToString();
                }
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al cargar Peticiones: [" + Ex.Message + "]", true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Validar_Atendio_Direccion
    ///DESCRIPCIÓN: Consulta los parámetros y si el programa_id seleccionado es igual al valor en 
    ///         Programa_ID_Atiende_Direccion, consulta el nombre del director de la unidad responsable seleccionada
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 07-nov-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Validar_Atendio_Direccion()
    {
        Cls_Cat_Ate_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ate_Parametros_Negocio();
        Cls_Cat_Ate_Organigrama_Negocios Neg_Consulta_Director = new Cls_Cat_Ate_Organigrama_Negocios();
        DataTable Dt_Parametros;
        DataTable Dt_Directores_UR;

        try
        {
            // sólo si se está modificando o ingresando una petición nueva, validar el contenido del campo atendió
            if (Btn_Modificar.ToolTip == "Actualizar" || Btn_Nuevo.ToolTip == "Dar de Alta")
            {
                // consultar parámetros de programas de atención ciudadana
                Dt_Parametros = Obj_Parametros.Consultar_Parametros();
                if (Dt_Parametros != null && Dt_Parametros.Rows.Count > 0)
                {
                    Hdn_Programa_Atendido_Direcciones.Value = Dt_Parametros.Rows[0][Cat_Ven_Parametros.Campo_Programa_ID_Atiende_Direccion].ToString();
                    // si el parámetro contiene un valor, comparar con programa origen
                    if (!string.IsNullOrEmpty(Hdn_Programa_Atendido_Direcciones.Value) && Hdn_Programa_Atendido_Direcciones.Value == Cmb_Origen.SelectedValue)
                    {
                        Hdn_Atendio.Value = "";
                        // consultar director de unidad responsable (si está seleccionado)
                        if (Cmb_Dependencia.SelectedIndex > 0)
                        {
                            // consulta el Director de U.R. seleccionada
                            Neg_Consulta_Director.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
                            Neg_Consulta_Director.P_Modulo = "ATENCION CIUDADANA";
                            Dt_Directores_UR = Neg_Consulta_Director.Consultar_Empleado_Unidad();
                            // validar que la tabla que regresó la consulta contenga datos
                            if (Dt_Directores_UR != null && Dt_Directores_UR.Rows.Count > 0)
                            {
                                Hdn_Atendio.Value = Dt_Directores_UR.Rows[0]["EMPLEADO"].ToString();
                            }
                        }
                    }
                    else
                    {
                        Hdn_Atendio.Value = Cls_Sessiones.Nombre_Empleado;
                    }
                }
                else
                {
                    Hdn_Atendio.Value = Cls_Sessiones.Nombre_Empleado;
                }
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error: [" + Ex.Message + "]", true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cargar_Datos_Ciudadano_Peticion
    ///DESCRIPCIÓN: Consulta los datos de la petición especificada y carga sólo los datos del solicitante
    ///         en los controles correspondientes
    ///PARÁMETROS:
    /// 		1. No_Peticion: número de petición a consultar
    /// 		2. Anio_Peticion: año de la petición a consultar
    /// 		3. Programa_ID: id del programa a consultar
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 28-sep-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Cargar_Datos_Ciudadano_Peticion(string No_Peticion, int Anio_Peticion, string Programa_ID)
    {
        try
        {
            var Obj_Peticiones = new Cls_Cat_Ate_Peticiones_Negocio();
            int Edad_Ciudadano;
            DateTime Fecha_Nacimiento;

            Obj_Peticiones.P_No_Peticion = No_Peticion;
            Obj_Peticiones.P_Anio_Peticion = Anio_Peticion;
            Obj_Peticiones.P_Programa_ID = Programa_ID;
            DataTable Dt_Temporal = Obj_Peticiones.Consulta_Peticion();

            // si la consulta arroja resultados, cargar datos
            if (Dt_Temporal != null && Dt_Temporal.Rows.Count > 0)
            {
                DataRow Renglon = Dt_Temporal.Rows[0];

                Txt_Nombre.Text = Renglon[Ope_Ate_Peticiones.Campo_Nombre_Solicitante].ToString();
                Txt_Apellido_Paterno.Text = Renglon[Ope_Ate_Peticiones.Campo_Apellido_Paterno].ToString();
                Txt_Apellido_Materno.Text = Renglon[Ope_Ate_Peticiones.Campo_Apellido_Materno].ToString();
                // validar campo edad, si es un número mayor que 0 mostrar, si no, limpiar campo
                if (int.TryParse(Renglon[Ope_Ate_Peticiones.Campo_Edad].ToString(), out Edad_Ciudadano) && Edad_Ciudadano > 0)
                {
                    Txt_Edad.Text = Edad_Ciudadano.ToString();
                }
                else
                {
                    Txt_Edad.Text = "";
                }
                // validar campo fecha_nacimiento, si hay una fecha, cargar en el campo correspondiente
                if (DateTime.TryParse(Renglon[Ope_Ate_Peticiones.Campo_Fecha_Nacimiento].ToString(), out Fecha_Nacimiento) && Fecha_Nacimiento != DateTime.MinValue)
                {
                    Txt_Fecha_Nacimiento.Text = Fecha_Nacimiento.ToString("dd/MMM/yyyy");
                }
                else
                {
                    Txt_Fecha_Nacimiento.Text = "";
                }
                Txt_Numero_Exterior.Text = Renglon[Ope_Ate_Peticiones.Campo_Numero_Exterior].ToString();
                Txt_Numero_Interior.Text = Renglon[Ope_Ate_Peticiones.Campo_Numero_Interior].ToString();
                Txt_Referencia.Text = Renglon[Ope_Ate_Peticiones.Campo_Referencia].ToString();
                Txt_Codigo_Postal.Text = Renglon[Ope_Ate_Peticiones.Campo_Codigo_Postal].ToString();
                Txt_Telefono.Text = Renglon[Ope_Ate_Peticiones.Campo_Telefono].ToString();
                Txt_Email.Text = Renglon[Ope_Ate_Peticiones.Campo_Email].ToString();

                Hdn_Ciudadano_Id.Value = Renglon[Ope_Ate_Peticiones.Campo_Contribuyente_ID].ToString();

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
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al cargar datos del ciudadano: [" + Ex.Message + "]", true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cargar_Informacion_Ciudadano_Peticion
    ///DESCRIPCIÓN: Consulta los datos de la petición especificada y carga los datos personales del 
    ///             solicitante en los controles correspondientes
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
    public void Cargar_Informacion_Ciudadano_Peticion(string No_Peticion, int Anio_Peticion, string Programa_ID)
    {
        try
        {
            var Obj_Peticiones = new Cls_Cat_Ate_Peticiones_Negocio();
            int Edad_Ciudadano;
            DateTime Fecha_Nacimiento;

            Obj_Peticiones.P_No_Peticion = No_Peticion;
            Obj_Peticiones.P_Anio_Peticion = Anio_Peticion;
            Obj_Peticiones.P_Programa_ID = Programa_ID;
            DataTable Dt_Temporal = Obj_Peticiones.Consulta_Peticion();

            // si la consulta arroja resultados, cargar datos
            if (Dt_Temporal != null && Dt_Temporal.Rows.Count > 0)
            {
                DataRow Renglon = Dt_Temporal.Rows[0];

                Txt_Nombre.Text = Renglon[Ope_Ate_Peticiones.Campo_Nombre_Solicitante].ToString();
                Txt_Apellido_Paterno.Text = Renglon[Ope_Ate_Peticiones.Campo_Apellido_Paterno].ToString();
                Txt_Apellido_Materno.Text = Renglon[Ope_Ate_Peticiones.Campo_Apellido_Materno].ToString();
                // validar campo edad, si es un número mayor que 0 mostrar, si no, limpiar campo
                if (int.TryParse(Renglon[Ope_Ate_Peticiones.Campo_Edad].ToString(), out Edad_Ciudadano) && Edad_Ciudadano > 0)
                {
                    Txt_Edad.Text = Edad_Ciudadano.ToString();
                }
                else
                {
                    Txt_Edad.Text = "";
                }
                // validar campo fecha_nacimiento, si hay una fecha, cargar en el campo correspondiente
                if (DateTime.TryParse(Renglon[Ope_Ate_Peticiones.Campo_Fecha_Nacimiento].ToString(), out Fecha_Nacimiento) && Fecha_Nacimiento != DateTime.MinValue)
                {
                    Txt_Fecha_Nacimiento.Text = Fecha_Nacimiento.ToString("dd/MMM/yyyy");
                }
                else
                {
                    Txt_Fecha_Nacimiento.Text = "";
                }
                Txt_Numero_Exterior.Text = Renglon[Ope_Ate_Peticiones.Campo_Numero_Exterior].ToString();
                Txt_Numero_Interior.Text = Renglon[Ope_Ate_Peticiones.Campo_Numero_Interior].ToString();
                Txt_Referencia.Text = Renglon[Ope_Ate_Peticiones.Campo_Referencia].ToString();
                Txt_Codigo_Postal.Text = Renglon[Ope_Ate_Peticiones.Campo_Codigo_Postal].ToString();
                Txt_Telefono.Text = Renglon[Ope_Ate_Peticiones.Campo_Telefono].ToString();
                Txt_Email.Text = Renglon[Ope_Ate_Peticiones.Campo_Email].ToString();

                // cargar los valores de los combos revisando primero que el valor a seleccionar exista entre los elementos del combo
                if (Cmb_Sexo.Items.FindByValue(Renglon[Ope_Ate_Peticiones.Campo_Sexo].ToString()) != null)
                {
                    Cmb_Sexo.SelectedValue = Renglon[Ope_Ate_Peticiones.Campo_Sexo].ToString();
                }
                if (Cmb_Colonia.Items.FindByValue(Renglon[Ope_Ate_Peticiones.Campo_Colonia_ID].ToString()) != null)
                {
                    Cmb_Colonia.SelectedValue = Renglon[Ope_Ate_Peticiones.Campo_Colonia_ID].ToString();
                    Cmb_Colonia_SelectedIndexChanged(null, null); // para que se carguen las calles de la colonia seleccionada
                }
                if (Cmb_Calle.Items.FindByValue(Renglon[Ope_Ate_Peticiones.Campo_Calle_ID].ToString()) != null)
                {
                    Cmb_Calle.SelectedValue = Renglon[Ope_Ate_Peticiones.Campo_Calle_ID].ToString();
                }
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al cargar Peticiones: [" + Ex.Message + "]", true);
        }
    }

    /// *****************************************************************************************
    /// NOMBRE: Cargar_Datos_Grid
    /// DESCRIPCIÓN: Carga los datos especificados en el grid que se recibe como parámetro
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 18-may-2012
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************
    private void Cargar_Datos_Grid(GridView Grid, DataTable Dt_Table, string Orden)
    {
        if (Dt_Table != null && !string.IsNullOrEmpty(Orden))
        {
            Dt_Table.DefaultView.Sort = Orden;
        }

        // mostrar resultado en grid peticiones
        Grid_Peticiones.Columns[1].Visible = true;
        Grid_Peticiones.Columns[2].Visible = true;
        Grid_Peticiones.Columns[3].Visible = true;
        Grid_Peticiones.Columns[4].Visible = true;
        Grid_Peticiones.DataSource = Dt_Table;
        Grid_Peticiones.DataBind();
        Grid_Peticiones.Columns[1].Visible = false;
        Grid_Peticiones.Columns[2].Visible = false;
        Grid_Peticiones.Columns[3].Visible = false;
        Grid_Peticiones.Columns[4].Visible = false;
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
        String Ruta = Server.MapPath("../Rpt/Atencion_Ciudadana/" + Nombre_Reporte);

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
    /// NOMBRE_FUNCIÓN: Generar_Formato
    /// DESCRIPCIÓN: Generar formato de Consecutivo (con OpenXML SDK a partir de documento con controles de contenido)
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 02-jul-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Generar_Formato()
    {
        var Neg_Consulta_Director = new Cls_Cat_Ate_Organigrama_Negocios();
        string Ruta_Plantilla = Server.MapPath("../../Archivos/Atencion_Ciudadana/PlantillasWord/" + "Formato_Consecutivos_AC.docx");
        string Documento_Salida = Server.MapPath("../../Reporte/" + "Formato_Consecutivos_AC.docx");
        string Nombre_Director_UR = "";
        DataTable Dt_Directores_UR;
        DateTime Fecha_Peticion;
        string Texto_Fecha_Peticion;

        // validar que existe el documento plantilla
        if (!File.Exists(Ruta_Plantilla))
        {
            throw new Exception("No se encontró el documento plantilla para generar documento.");
        }
        // copiar la plantilla para editarla en una nueva ubicación
        if (File.Exists(Documento_Salida))
        {
            File.Delete(Documento_Salida);
        }
        File.Copy(Ruta_Plantilla, Documento_Salida);

        ReportDocument Reporte = new ReportDocument();
        String Nombre_Archivo = "Formato_Consecutivos_AC.docx";

        // si no existe el directorio, crearlo
        if (!Directory.Exists(Server.MapPath("../../Reporte")))
            Directory.CreateDirectory("../../Reporte");

        // consulta el Director de U.R. seleccionada
        Neg_Consulta_Director.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
        Neg_Consulta_Director.P_Modulo = "ATENCION CIUDADANA";
        Dt_Directores_UR = Neg_Consulta_Director.Consultar_Empleado_Unidad();
        if (Dt_Directores_UR != null && Dt_Directores_UR.Rows.Count > 0)
        {
            Nombre_Director_UR = Dt_Directores_UR.Rows[0]["EMPLEADO"].ToString();
        }

        // formar domicilio
        string Domicilio = "";
        // obtener el domicilio
        string Calle = Cmb_Calle.SelectedIndex > 0 ? Cmb_Calle.SelectedItem.Text : "";
        string Colonia = Cmb_Colonia.SelectedIndex > 0 ? Cmb_Colonia.SelectedItem.Text : "";
        string Numero_Exterior = Txt_Numero_Exterior.Text;
        string Numero_Interior = Txt_Numero_Interior.Text;
        // si la calle contiene texto, agregar calle y números exterior e interior
        if (Calle.Length > 0)
        {
            Domicilio += Calle + " " + Numero_Exterior + " " + Numero_Interior;
        }
        else
        {
            if (Numero_Exterior.Trim().Length > 0)
            {
                Domicilio += "Número exterior " + Numero_Exterior;
            }
            if (Numero_Interior.Trim().Length > 0)
            {
                Domicilio += " int. " + Numero_Interior;
            }
        }
        // si hay una colonia, agregar al domicilio
        if (Colonia.Length > 0)
        {
            Domicilio += " col. " + Colonia;
        }

        try
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(Documento_Salida, true))
            {
                string Datos_Peticion = Txt_Nombre.Text + " " + Txt_Apellido_Paterno.Text + " " + Txt_Apellido_Materno.Text;

                // si el campo domicilio tiene texto, agregar a datos de petición
                if (Domicilio.Length > 0)
                {
                    Datos_Peticion += "\n" + Domicilio;
                }
                // si el campo referencia tiene texto, agregar a datos de petición
                if (Txt_Referencia.Text.Length > 0)
                {
                    Datos_Peticion += "\n" + Txt_Referencia.Text;
                }
                // si el campo oculto Txt_Email tiene texto, agregar a datos de petición
                if (Txt_Email.Text.Length > 0)
                {
                    Datos_Peticion += "\n" + Txt_Email.Text;
                }

                DateTime.TryParse(Txt_Fecha_Peticion.Text, out Fecha_Peticion);
                // si se logró convertir la fecha, asignar con formato, si no, tomar la fecha de caja de texto
                if (Fecha_Peticion != DateTime.MinValue)
                {
                    Texto_Fecha_Peticion = Fecha_Peticion.ToString("d 'de' MMMM 'de' yyyy");
                }
                else
                {
                    Texto_Fecha_Peticion = Txt_Fecha_Peticion.Text;
                }
                // crear un string XML para sustituir el customXMLpart en el documento
                string newXml = "<root>"
                    + "<FOLIO>" + Txt_Folio.Text.ToUpper() + "</FOLIO>"
                    + "<FECHA>" + Texto_Fecha_Peticion.ToUpper() + "</FECHA>"
                    + "<NOMBRE_DIRECTOR>" + Nombre_Director_UR.ToUpper() + "</NOMBRE_DIRECTOR>"
                    + "<UNIDAD_RESPONSABLE>" + Cmb_Dependencia.SelectedItem.Text.ToUpper() + "</UNIDAD_RESPONSABLE>"
                    + "<DATOS_PETICION>" + Datos_Peticion.ToUpper() + "</DATOS_PETICION>"
                    + "<ASUNTO_PETICION>" + Txt_Peticion.Text.Trim().ToUpper() + "</ASUNTO_PETICION>"
                    + "<OPCION_SEGUIMIENTO>" + Cmb_Seguimiento_Consecutivo.SelectedItem.Text.ToUpper() + "</OPCION_SEGUIMIENTO>"
                    + "</root>";

                MainDocumentPart main = doc.MainDocumentPart;
                main.DeleteParts<CustomXmlPart>(main.CustomXmlParts);

                // agregar y escribir la nueva parte en el documento
                CustomXmlPart customXml = main.AddCustomXmlPart(CustomXmlPartType.CustomXml);

                // escribir la nueva parte (customXMLpart) en el documento
                using (StreamWriter ts = new StreamWriter(customXml.GetStream()))
                {
                    ts.Write(newXml);
                }

                // si la opción en el combo es CONOCIMIENTO, eliminar texto en campo 
                if (Cmb_Seguimiento_Consecutivo.SelectedValue == "CONOCIMIENTO")
                {
                    openXML_Wp.SdtBlock ccWithTable = main.Document.Body.Descendants<openXML_Wp.SdtBlock>().Where(r => r.SdtProperties.GetFirstChild<openXML_Wp.Tag>().Val == "INSTRUCCIONES_SEGUIMIENTO").Single();
                    ccWithTable.Remove();
                }

                // guardar los cambios en el documento
                main.Document.Save();

                //closing WordprocessingDocument automatically saves the document
            }

            Abrir_Reporte(Server.MapPath("../../Reporte/" + Nombre_Archivo));
        }
        catch (Exception Ex)
        {
            throw new Exception("Imprimir formato: " + Ex.Message);
        }
    }

    #region (Mostrar Reporte)

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Abrir_Reporte
    ///DESCRIPCIÓN: Generar el encabezado para ofrecer para descarga el archivo recibido como parámetro
    ///PARÁMETROS:
    /// 		1. Ruta_Absoluta_Archivo: texto con la ruta absoluta del archivo a generar
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 02-jul-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Abrir_Reporte(String Ruta_Absoluta_Archivo)
    {
        String Nombre_Archivo = String.Empty;//Nombre del archivo a descargar.
        String Extensión_Archivo = String.Empty;//Extension del archivo
        String Tipo_Archivo = String.Empty;//MimeType. Forma de retorno de los datos del servidor al cliente.

        System.IO.FileInfo Archivo = new System.IO.FileInfo(Ruta_Absoluta_Archivo);//Crear instancia para leer la información del archivo.

        Nombre_Archivo = Path.GetFileName(Ruta_Absoluta_Archivo);//Obtenemos el nombre completo del archivo.
        Extensión_Archivo = Path.GetExtension(Ruta_Absoluta_Archivo);//Obtenemos la extensión del archivo.

        //Validamos la extensión del archivo.
        if (!String.IsNullOrEmpty(Extensión_Archivo))
            Extensión_Archivo = Extensión_Archivo.Trim().ToLower();

        //Obtenemos el estandar [MimeType] para retorno de datos al cliente.
        switch (Extensión_Archivo)
        {
            case ".doc":
                Tipo_Archivo = "Application/msword";
                break;
            case ".docm":
                Tipo_Archivo = "application/vnd.ms-word.document.macroEnabled.12";
                break;
            case ".dotx":
                Tipo_Archivo = "application/vnd.openxmlformats-officedocument.wordprocessingml.template";
                break;
            case ".dotm":
                Tipo_Archivo = "application/vnd.ms-word.template.macroEnabled.12";
                break;
            case ".docx":
                Tipo_Archivo = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                break;

            case ".xls":
                Tipo_Archivo = "application/vnd.ms-excel";
                break;
            case ".xlsx":
                Tipo_Archivo = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                break;
            case ".xlsm":
                Tipo_Archivo = "application/vnd.ms-excel.sheet.macroEnabled.12";
                break;
            case ".xltx":
                Tipo_Archivo = "application/vnd.openxmlformats-officedocument.spreadsheetml.template";
                break;

            case ".pdf":
                Tipo_Archivo = "Application/pdf";
                break;
            default:
                Tipo_Archivo = "text/plain";
                break;
        }

        //Borra toda la salida de contenido de la secuencia del búfer
        Response.Clear();
        //Obtiene o establece un valor que indica si la salida se va a almacenar en el búfer y se va a enviar después de que se haya terminado de procesar la respuesta completa
        Response.Buffer = true;
        //Obtiene o establece el tipo MIME HTTP de la secuencia de salida
        Response.ContentType = Tipo_Archivo;
        //Esta linea es opcional, en donde podemos cambiar el nombre del fichero a descargar (para que sea diferente al original)
        Response.AddHeader("Content-Disposition", "attachment;filename=" + (Archivo.Name).Replace(Session.SessionID, String.Empty));
        //Establecemos el juego de caracteres HTTP de la secuencia de salida
        Response.Charset = "UTF-8";
        //Establecemos el juego de caracteres HTTP de la secuencia de salida
        Response.ContentEncoding = Encoding.Default;
        //Escribimos el fichero a enviar
        Response.WriteFile(Archivo.FullName);
        //Envía al cliente toda la salida almacenada en el búfer
        Response.Flush();
        //Envía al cliente toda la salida del búfer actual, detiene la ejecución de la página y provoca el evento se detenga.
        Response.End();
    }

    #endregion (Mostrar Reporte)

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
    /// 		8. Asunto: asunto asignado a la petición a imprimir
    /// 		9. Accion: acción asignada a la petición a imprimir
    /// 		10. Dependencia: dependencia asignada para la petición
    /// 		11. Atendio: nombre de quien atiende la petición para mostrar en el folio impreso
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 18-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataSet Crear_Ds_Imprimir_Reporte(string Folio, string Fecha_Peticion, string Nombre_Solicitante, string Domicilio, string Email, string Peticion, string Telefono, string Asunto, string Accion, string Dependencia, string Atendio)
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
        Dr_Fila_Reporte["Asunto"] = Asunto;
        Dr_Fila_Reporte["Respuesta"] = Accion;
        Dr_Fila_Reporte["Dependencia"] = Dependencia;
        // si se especificó un valor en el parámetro Atendio, agregar al renglón del reporte
        if (!string.IsNullOrEmpty(Atendio))
        {
            Dr_Fila_Reporte["NOMBRE_ATENDIO"] = Atendio;
        }
        else
        {
            Dr_Fila_Reporte["NOMBRE_ATENDIO"] = "ATENDIÓ";
        }
        // agregar fila a la tabla
        Ds_Reporte.Tables[0].Rows.Add(Dr_Fila_Reporte);

        return Ds_Reporte;
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
        Cls_Ven_Usuarios_Negocio Obj_Parametros = new Cls_Ven_Usuarios_Negocio();
        DataTable Dt_Parametros;

        // limpiar mensajes de error
        Mostrar_Informacion("", false);

        try
        {
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Habilitar_Botones("Nuevo");
                Limpiar_Campos();
                //limpiar grid_peticiones
                Cargar_Datos_Grid(Grid_Peticiones, null, null);
                // consultar el parámetro programa_id_ventanilla
                Dt_Parametros = Obj_Parametros.Consultar_Parametros();
                if (Dt_Parametros != null && Dt_Parametros.Rows.Count > 0)
                {
                    string Programa_Id_Ventanilla = Dt_Parametros.Rows[0][Cat_Ven_Parametros.Campo_Programa_ID_Ventanilla].ToString();
                    // si el combo origen contiene el id, seleccionarlo
                    if (Cmb_Origen.Items.FindByValue(Programa_Id_Ventanilla) != null)
                    {
                        Cmb_Origen.SelectedValue = Programa_Id_Ventanilla;
                    }
                }
                Cmb_Origen_SelectedIndexChanged(null, null);
                Habilitar_Campos(true);
                Txt_Fecha_Peticion.Text = DateTime.Now.ToString("dd/MMM/yyyy");
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
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error alta registro: " + Ex.Message, true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Formato_Consecutivo_Click
    ///DESCRIPCIÓN: Dependiendo del estado del botón (tooltip Nuevo o Dar de alta)
    ///         Configurar controles o dar de alta una nueva petición
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 18-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Formato_Consecutivo_Click(object sender, ImageClickEventArgs e)
    {
        // limpiar mensajes de error
        Mostrar_Informacion("", false);
        try
        {
            Generar_Formato();
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al generar formato: " + Ex.Message, true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Dependiendo del estado del botón (tooltipo: Modificar o Actualizar)
    ///         Configurar controles o actualiza una petición
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 18-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        // limpiar mensajes de error
        Mostrar_Informacion("", false);


        try
        {
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                // validar que haya un folio
                if (Txt_Folio.Text != String.Empty)
                {
                    // validar el estatus de la petición seleccionada
                    if (Hdn_Estatus.Value == "EN PROCESO")
                    {
                        Habilitar_Campos(true);
                        Habilitar_Botones("Modificar");
                    }
                    else
                    {
                        Mostrar_Informacion("No es posible modificar peticiones con estatus: " + Hdn_Estatus.Value, true);
                    }
                }
                else
                {
                    Mostrar_Informacion("No existen datos para modificar", true);
                }
            }
            else
            {
                if (Validar_Campos_Obligatorios())
                {
                    Modificar_Peticion();
                }
                else
                {
                    Mostrar_Informacion(Informacion, true);
                }
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion(Ex.Message, true);
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

        try
        {
            if (Txt_Folio.Text.Length > 0)
            {
                string Domicilio = Obtener_Domicilio_Completo_Solicitante();
                string Nombre_Solicitante = Txt_Apellido_Paterno.Text + " " + Txt_Apellido_Materno.Text + " " + Txt_Nombre.Text;
                string Asunto = Cmb_Asunto.SelectedIndex > 0 ? Cmb_Asunto.SelectedItem.Text : "";
                string Accion = Cmb_Accion.SelectedIndex > 0 ? Cmb_Accion.SelectedItem.Text : "";
                string Dependencia = Cmb_Dependencia.SelectedIndex > 0 ? Cmb_Dependencia.SelectedItem.Text : "";
                Imprimir_Reporte(Crear_Ds_Imprimir_Reporte(Txt_Folio.Text, Txt_Fecha_Peticion.Text, Nombre_Solicitante, Domicilio, Txt_Email.Text, Txt_Peticion.Text, Txt_Telefono.Text, Asunto, Accion, Dependencia, Hdn_Atendio.Value), "Rpt_Ope_Ate_Folio_Peticion.rpt", "Folio_Reporte_Ciudadano_" + Txt_Folio.Text);
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion(Ex.Message, true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Manejo del evento clic en el botón de salir: dependiendo del tooltip del botón, regresa a 
    ///         la página principal o reinicia los controles de la página a su estado Inicial
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 18-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Mostrar_Informacion("", false);

        if (Btn_Salir.ToolTip == "Inicio")
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Tr_Contenedor_Combo_Consecutivo.Visible = false;
            Btn_Formato_Consecutivo.Visible = false;
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
    ///NOMBRE_FUNCIÓN: Cmb_Origen_SelectedIndexChanged
    ///DESCRIPCIÓN: validar si el origen seleccionado genera consecutivo, mostrar combo seguimiento consecutivo
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 02-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cmb_Origen_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Ate_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ate_Parametros_Negocio();
        Cls_Cat_Ate_Organigrama_Negocios Neg_Consulta_Director = new Cls_Cat_Ate_Organigrama_Negocios();
        DataTable Dt_Parametros;
        DataTable Dt_Directores_UR;
        string Parametro_Programa_Id = "";

        Mostrar_Informacion("", false);

        try
        {
            // consultar parámetros de programas de atención ciudadana
            Dt_Parametros = Obj_Parametros.Consultar_Parametros();
            if (Dt_Parametros != null && Dt_Parametros.Rows.Count > 0)
            {
                Parametro_Programa_Id = Dt_Parametros.Rows[0][Cat_Ven_Parametros.Campo_Programa_ID_Genera_Consecutivo].ToString();
                // si el origen seleccionado es igual al programa del parámetro, mostrar control combo seguimiento
                if (Parametro_Programa_Id == Cmb_Origen.SelectedValue)
                {
                    Tr_Contenedor_Combo_Consecutivo.Visible = true;
                    Cmb_Seguimiento_Consecutivo.Enabled = true;
                }
                else
                {
                    Tr_Contenedor_Combo_Consecutivo.Visible = false;
                    Cmb_Seguimiento_Consecutivo.Enabled = false;
                    Cmb_Seguimiento_Consecutivo.SelectedIndex = 0;
                }

                // sólo si se está modificando o ingresando una petición nueva, validar el contenido del campo atendió
                if (Btn_Modificar.ToolTip == "Actualizar" || Btn_Nuevo.ToolTip == "Dar de Alta")
                {
                    Hdn_Programa_Atendido_Direcciones.Value = Dt_Parametros.Rows[0][Cat_Ven_Parametros.Campo_Programa_ID_Atiende_Direccion].ToString();
                    // si el parámetro contiene un valor, comparar con programa origen
                    if (!string.IsNullOrEmpty(Hdn_Programa_Atendido_Direcciones.Value) && Hdn_Programa_Atendido_Direcciones.Value == Cmb_Origen.SelectedValue)
                    {
                        Hdn_Atendio.Value = "";
                        // consultar director de unidad responsable (si está seleccionado)
                        if (Cmb_Dependencia.SelectedIndex > 0)
                        {
                            // consulta el Director de U.R. seleccionada
                            Neg_Consulta_Director.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
                            Neg_Consulta_Director.P_Modulo = "ATENCION CIUDADANA";
                            Dt_Directores_UR = Neg_Consulta_Director.Consultar_Empleado_Unidad();
                            // validar que la tabla que regresó la consulta contenga datos
                            if (Dt_Directores_UR != null && Dt_Directores_UR.Rows.Count > 0)
                            {
                                Hdn_Atendio.Value = Dt_Directores_UR.Rows[0]["EMPLEADO"].ToString();
                            }
                        }
                    }
                    else
                    {
                        Hdn_Atendio.Value = Cls_Sessiones.Nombre_Empleado;
                    }
                }
            }
            Cargar_Grid_Peticiones_Recientes();
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al consultar parámetros: " + Ex, true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cmb_Cantidad_Peticiones_Mostrar_SelectedIndexChanged
    ///DESCRIPCIÓN: actualizar el número de peticiones en el grid peticiones recientes
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 12-oct-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cmb_Cantidad_Peticiones_Mostrar_SelectedIndexChanged(object sender, EventArgs e)
    {
        Mostrar_Informacion("", false);

        try
        {
            Cargar_Grid_Peticiones_Recientes();
            // si el número de peticiones a mostrar es diferente de 10, almacenar cookie, si no, marcar para eliminar
            if (Cmb_Cantidad_Peticiones_Mostrar.SelectedValue != "10")
            {
                HttpCookie cCantidad_Peticiones = new HttpCookie("Numero_Peticiones_Recientes");
                cCantidad_Peticiones.Value = Cmb_Cantidad_Peticiones_Mostrar.SelectedValue;
                // especificar un tiempo de vida de cinco días a la cookie
                cCantidad_Peticiones.Expires = DateTime.Now.Add(new TimeSpan(5, 0, 0, 0));
                // almacenar cookie
                Response.Cookies.Add(cCantidad_Peticiones);
            }
            else
            {
                // si la cookie existe, actualizar cookie con fecha de expiración actual menos un día para que se elimine
                if (Request.Cookies["Numero_Peticiones_Recientes"] != null)
                {
                    HttpCookie cCantidad_Peticiones = new HttpCookie("Numero_Peticiones_Recientes");
                    cCantidad_Peticiones.Expires = DateTime.Now.AddDays(-1d);
                    Response.Cookies.Add(cCantidad_Peticiones);
                }
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al consultar peticiones: " + Ex, true);
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

        Mostrar_Informacion("", false);

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
            Mostrar_Informacion("No se pudo mostrar información: " + Ex, true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cmb_Asunto_SelectedIndexChanged
    ///DESCRIPCIÓN: Si se selecciona un asunto se consulta la dependencia a la que pertenece y se selecciona la dependencia seleccionada
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cmb_Asunto_SelectedIndexChanged(object sender, EventArgs e)
    {
        var Obj_Asuntos = new Cls_Cat_Ate_Asuntos_Negocio();
        DataTable Dt_Asuntos;

        Mostrar_Informacion("", false);

        try
        {
            // si hay una asunto seleccionado, consultar la dependencia de ese Asunto
            if (Cmb_Asunto.SelectedIndex > 0)
            {
                Obj_Asuntos.P_ID = Cmb_Asunto.SelectedValue;
                Dt_Asuntos = Obj_Asuntos.Consultar_Registros();
                // validar que la tabla contiene registros
                if (Dt_Asuntos != null && Dt_Asuntos.Rows.Count > 0)
                {
                    string Dependencia_ID = Dt_Asuntos.Rows[0][Cat_Ate_Asuntos.Campo_DependenciaID].ToString();
                    // si el combo dependencias contiene un elemento con el id de la dependencia en la consulta, seleccionar el elemento
                    if (Cmb_Dependencia.Items.FindByValue(Dependencia_ID) != null)
                    {
                        Cmb_Dependencia.SelectedValue = Dependencia_ID;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("No se pudo mostrar información: " + Ex, true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cmb_Accion_SelectedIndexChanged
    ///DESCRIPCIÓN: Si se selecciona una acción, se consulta el número de días para calcular la fecha probable de solución
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cmb_Accion_SelectedIndexChanged(object sender, EventArgs e)
    {
        var Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
        var Obj_Acciones = new Cls_Cat_Ate_Acciones_Negocio();
        DateTime Fecha_Solucion;
        DataTable Dt_Acciones;

        Mostrar_Informacion("", false);

        try
        {
            Txt_Fecha_Solucion.Text = "";
            // si hay una asunto seleccionado, consultar la dependencia de ese Asunto
            if (Cmb_Accion.SelectedIndex > 0)
            {
                Obj_Acciones.P_ID = Cmb_Accion.SelectedValue;
                Dt_Acciones = Obj_Acciones.Consultar_Registros();
                // validar que la tabla contiene registros
                if (Dt_Acciones != null && Dt_Acciones.Rows.Count > 0)
                {
                    int Numero_Dias;
                    int.TryParse(Dt_Acciones.Rows[0][Cat_Ate_Acciones.Campo_Tiempo_Estimado_Solucion].ToString(), out Numero_Dias);
                    // calcular la fecha probable de solución sumando el número de días obtenidos a la fecha actual
                    if (Numero_Dias > 0)
                    {
                        Fecha_Solucion = Dias_Inhabilies.Calcular_Fecha(DateTime.Now.ToShortDateString(), Numero_Dias.ToString());
                        if (Fecha_Solucion != DateTime.MinValue)
                        {
                            Txt_Fecha_Solucion.Text = Fecha_Solucion.ToString("dd/MMM/yyyy");
                        }
                    }
                    else
                    {
                        Txt_Fecha_Solucion.Text = DateTime.Now.ToString("dd/MMM/yyyy");
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
    ///NOMBRE_FUNCIÓN: Cmb_Dependencia_SelectedIndexChanged
    ///DESCRIPCIÓN: Si se selecciona un dependencia, se consultan los asuntos de la dependencia seleccionada
    ///         y se cargan en el combo Asunto
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cmb_Dependencia_SelectedIndexChanged(object sender, EventArgs e)
    {
        var Obj_Asuntos = new Cls_Cat_Ate_Asuntos_Negocio();

        Mostrar_Informacion("", false);

        try
        {
            // si hay una dependencia seleccionada, agregar filtro al Obj_Asuntos
            if (Cmb_Dependencia.SelectedIndex > 0)
            {
                Obj_Asuntos.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
            }

            // cargar el combo asuntos con los resultados de la consulta
            Llenar_Combo_Con_DataTable(Cmb_Asunto, Obj_Asuntos.Consultar_Registros());

            // llamar método para consultar el director de la unidad seleccionada
            Validar_Atendio_Direccion();
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("No se pudo mostrar información: " + Ex, true);
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
        Mostrar_Informacion("", false);

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
        Mostrar_Informacion("", false);

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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Asunto_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID del asunto seleccionado en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Asunto_Click(object sender, ImageClickEventArgs e)
    {
        Mostrar_Informacion("", false);

        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_ASUNTOS"] != null)
        {
            // si el valor de la sesión es igual a true y la sesión asunto_id no es nulo ni vacío
            if (Convert.ToBoolean(Session["BUSQUEDA_ASUNTOS"]) == true && Session["ASUNTO_ID"] != null && Session["ASUNTO_ID"].ToString().Length > 0)
            {
                try
                {
                    // volver a cargar combo asuntos
                    var Obj_Asunto = new Cls_Cat_Ate_Asuntos_Negocio();
                    Obj_Asunto.P_Estatus = "ACTIVO";
                    Llenar_Combo_Con_DataTable(Cmb_Asunto, Obj_Asunto.Consultar_Registros());

                    string Asunto_ID = Session["ASUNTO_ID"].ToString().Replace("&nbsp;", "");
                    // si el combo Asuntos contiene el ID, seleccionar
                    if (Cmb_Asunto.Items.FindByValue(Asunto_ID) != null)
                    {
                        Cmb_Asunto.SelectedValue = Asunto_ID;
                        Cmb_Asunto_SelectedIndexChanged(null, null);
                    }
                    else if (Session["NOMBRE_ASUNTO"] != null && Session["NOMBRE_ASUNTO"].ToString().Length > 0)
                    {
                        Cmb_Asunto.Items.Add(new ListItem(HttpUtility.HtmlDecode(Session["NOMBRE_ASUNTO"].ToString()), Asunto_ID));
                        Cmb_Asunto.SelectedValue = Asunto_ID;
                        Cmb_Asunto_SelectedIndexChanged(null, null);
                    }
                }
                catch (Exception Ex)
                {
                    Mostrar_Informacion(Ex.Message, true);
                }

                // limpiar variables de sesión
                Session.Remove("ASUNTO_ID");
                Session.Remove("NOMBRE_ASUNTO");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_ASUNTOS");
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Dependencia_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la Dependencia seleccionada en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Dependencia_Click(object sender, ImageClickEventArgs e)
    {
        Mostrar_Informacion("", false);

        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_DEPENDENCIAS"] != null)
        {
            // si el valor de la sesión es igual a true
            if (Convert.ToBoolean(Session["BUSQUEDA_DEPENDENCIAS"]) == true)
            {
                try
                {
                    string Dependencia_ID = Session["DEPENDENCIA_ID"].ToString().Replace("&nbsp;", "");
                    // si el combo colonias contiene la colonia con el ID, seleccionar
                    if (Cmb_Dependencia.Items.FindByValue(Dependencia_ID) != null)
                    {
                        Cmb_Dependencia.SelectedValue = Dependencia_ID;
                        Cmb_Dependencia_SelectedIndexChanged(null, null);
                    }
                }
                catch (Exception Ex)
                {
                    Mostrar_Informacion(Ex.Message, true);
                }

                // limpiar variables de sesión
                Session.Remove("DEPENDENCIA_ID");
                Session.Remove("NOMBRE_DEPENDENCIA");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_DEPENDENCIAS");
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Accion_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la Acción seleccionada en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Accion_Click(object sender, ImageClickEventArgs e)
    {
        Mostrar_Informacion("", false);

        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_ACCIONES"] != null)
        {
            // si el valor de la sesión es igual a true y el valor de la sesiones accion_id no es nulo ni vacío
            if (Convert.ToBoolean(Session["BUSQUEDA_ACCIONES"]) == true && Session["ACCION_ID"] != null && Session["ACCION_ID"].ToString().Length > 0)
            {
                try
                {
                    string Accion_ID = Session["ACCION_ID"].ToString().Replace("&nbsp;", "");
                    // si el combo colonias contiene la colonia con el ID, seleccionar
                    if (Cmb_Accion.Items.FindByValue(Accion_ID) != null)
                    {
                        Cmb_Accion.SelectedValue = Accion_ID;
                        Cmb_Accion_SelectedIndexChanged(null, null);
                    }
                    else if (Session["NOMBRE_ACCION"] != null && Session["NOMBRE_ACCION"].ToString().Length > 0)
                    {
                        Cmb_Accion.Items.Add(new ListItem(HttpUtility.HtmlDecode(Session["NOMBRE_ACCION"].ToString()), Accion_ID));
                        Cmb_Accion.SelectedValue = Accion_ID;
                        Cmb_Accion_SelectedIndexChanged(null, null);
                    }
                }
                catch (Exception Ex)
                {
                    Mostrar_Informacion(Ex.Message, true);
                }

                // limpiar variables de sesión
                Session.Remove("ACCION_ID");
                Session.Remove("NOMBRE_ACCION");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_ACCIONES");
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Ciudadano_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID del Ciudadano seleccionado en la 
    ///             búsqueda avanzada y llama al método que consulta los datos y los carga en los 
    ///             controles correspondientes
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 13-jun-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Ciudadano_Click(object sender, ImageClickEventArgs e)
    {
        Mostrar_Informacion("", false);

        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_CIUDADANO"] != null)
        {
            // si el valor de la sesión es igual a true y el valor de la sesiones CIUDADANO_ID no es nulo ni vacío
            if (Convert.ToBoolean(Session["BUSQUEDA_CIUDADANO"]) == true)
            {
                try
                {
                    if (Session["NO_PETICION"] != null && Session["ANIO_PETICION"] != null && Session["PROGRAMA_ID"] != null)
                    {
                        int Anio_Peticion;
                        Limpiar_Informacion_Personal();
                        int.TryParse((string)Session["ANIO_PETICION"], out Anio_Peticion);
                        // llamar al método que carga los datos del ciudadano en los controles
                        Cargar_Informacion_Ciudadano_Peticion((string)Session["NO_PETICION"], Anio_Peticion, (string)Session["PROGRAMA_ID"]);
                    }
                    else
                    {
                        Limpiar_Informacion_Personal();
                        // llenar datos que llegaron de sesión
                        if (Session["NOMBRE"] != null)
                        {
                            Txt_Nombre.Text = (string)Session["NOMBRE"];
                        }
                        if (Session["APELLIDO_PATERNO"] != null)
                        {
                            Txt_Apellido_Paterno.Text = (string)Session["APELLIDO_PATERNO"];
                        }
                        if (Session["APELLIDO_MATERNO"] != null)
                        {
                            Txt_Apellido_Materno.Text = (string)Session["APELLIDO_MATERNO"];
                        }
                        if (Session["EMAIL"] != null)
                        {
                            Txt_Email.Text = (string)Session["EMAIL"];
                        }
                        if (Session["TELEFONO"] != null)
                        {
                            Txt_Telefono.Text = (string)Session["TELEFONO"];
                        }
                    }
                }
                catch (Exception Ex)
                {
                    Mostrar_Informacion(Ex.Message, true);
                }
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_CIUDADANO");
            Session.Remove("CIUDADANO_ID");
            Session.Remove("NO_PETICION");
            Session.Remove("ANIO_PETICION");
            Session.Remove("PROGRAMA_ID");
            Session.Remove("NOMBRE");
            Session.Remove("APELLIDO_PATERNO");
            Session.Remove("APELLIDO_MATERNO");
            Session.Remove("EMAIL");
            Session.Remove("TELEFONO");
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
                Cargar_Datos_Grid(Grid_Peticiones, Dt_Peticiones, e.SortExpression + " DESC");
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Cargar_Datos_Grid(Grid_Peticiones, Dt_Peticiones, e.SortExpression + " ASC");
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
                int.TryParse(Grid_Peticiones.SelectedRow.Cells[2].Text, out Anio_Peticion);
                Cargar_Datos_Peticion(Grid_Peticiones.SelectedRow.Cells[1].Text, Anio_Peticion, Grid_Peticiones.SelectedRow.Cells[3].Text);
                // mostrar y habilitar botón imprimir folio
                // si hay un tipo de consecutivo seleccionado, mostrar boton para generar plantilla
                if (Cmb_Seguimiento_Consecutivo.SelectedIndex > 0)
                {
                    Btn_Imprimir.Visible = false;
                    Btn_Imprimir.Enabled = false;
                    Btn_Formato_Consecutivo.Visible = true;
                    Btn_Formato_Consecutivo.Enabled = true;
                }
                else
                {
                    Btn_Imprimir.Visible = true;
                    Btn_Imprimir.Enabled = true;
                    Btn_Formato_Consecutivo.Visible = false;
                    Btn_Formato_Consecutivo.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion(Ex.Message, true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Grid_Peticiones_Recientes_SelectedIndexChanged
    ///DESCRIPCIÓN: Maneja el evento cambio índice del grid, llama al método que carga los detalles de una petición en la página
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 21-sep-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Peticiones_Recientes_SelectedIndexChanged(object sender, EventArgs e)
    {
        Mostrar_Informacion("", false);
        int Anio_Peticion;
        int Indice_Seleccionado_Cmb_Origen;
        string Texto_Campo_Fecha;

        try
        {
            //si hay una fila seleccionada, llamar al método que carga los datos en la página
            if (Grid_Peticiones_Recientes.SelectedIndex > -1)
            {
                // limpiar campos pero conservar el valor de cmb_origen y Txt_Fecha_Peticion
                Indice_Seleccionado_Cmb_Origen = Cmb_Origen.SelectedIndex;
                Texto_Campo_Fecha = Txt_Fecha_Peticion.Text;
                Limpiar_Campos();
                Cmb_Origen.SelectedIndex = Indice_Seleccionado_Cmb_Origen;
                Txt_Fecha_Peticion.Text = Texto_Campo_Fecha;

                int.TryParse(Grid_Peticiones_Recientes.SelectedRow.Cells[2].Text, out Anio_Peticion);
                Cargar_Datos_Ciudadano_Peticion(Grid_Peticiones_Recientes.SelectedRow.Cells[1].Text, Anio_Peticion, Grid_Peticiones_Recientes.SelectedRow.Cells[3].Text);
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion(Ex.Message, true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Chk_Terminar_Peticion_CheckedChanged
    ///DESCRIPCIÓN: deshabilita o habilita los controles para dar de alta peticiones con estatus TERMINADA
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 12-ago-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Chk_Terminar_Peticion_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Terminar_Peticion.Checked)
        {
            Cmb_Tipo_Solucion.Enabled = true;
            Txt_Descripcion_Solucion.Enabled = true;
        }
        else
        {
            Cmb_Tipo_Solucion.Enabled = false;
            Txt_Descripcion_Solucion.Enabled = false;
            Cmb_Tipo_Solucion.SelectedIndex = 0;
        }
    }

    #endregion Eventos

}
