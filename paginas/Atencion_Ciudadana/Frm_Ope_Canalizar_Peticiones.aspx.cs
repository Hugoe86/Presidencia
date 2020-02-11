using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using Presidencia.Dependencias.Negocios;
using Presidencia.Colonias.Negocios;
using Presidencia.Acciones_AC.Negocio;
using Presidencia.Asuntos_AC.Negocio;
using Presidencia.Programas_AC.Negocio;
using Presidencia.Catalogo_Calles.Negocio;
using Presidencia.Registro_Peticion.Negocios;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class paginas_Atencion_Ciudadana_Frm_Ope_Canalizar_Peticiones : System.Web.UI.Page
{
    #region Variables Locales
    private int Contador_Columna;
    private String Informacion;
    #endregion
    #region PageLoad/Init
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Cls_Sessiones.Mostrar_Menu = true;
            Habilitar_Botones("Inicial");
            Limpiar_Campos();
            Habilitar_Campos(false);
            LLenar_Combos();
            Consultar_Peticiones();

            // registro de scripts del lado del servidor para mostrar ventanas emergentes para búsqueda avanzada
            string Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergente/Frm_Busqueda_Avanzada_Asuntos.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Asunto.Attributes.Add("onclick", Ventana_Modal);
            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergente/Frm_Busqueda_Avanzada_Acciones.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Accion.Attributes.Add("onclick", Ventana_Modal);
            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergente/Frm_Busqueda_Avanzada_Dependencias.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Dependencia.Attributes.Add("onclick", Ventana_Modal);
        }
        //Mostrar_Informacion("", false);
    }
    #endregion

    #region Métodos

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Mostrar_Informacion
    ///DESCRIPCIÓN: Muestra en la página el mensaje recibido como parámetro y establece la visibilidad 
    ///             de los controles  para mostrar mensajes con el segundo parámtro
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
        //Se seleccion el tipo de valor a validar
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

        //Se revisa la expresion
        Regex Exp_Regular = new Regex(Str_Expresion);
        //Regresa un valor true o false segun se cumplan las condiciones
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
        //Verifica que campos esten seleccionados o tengan valor
        if (Txt_Fecha_Solucion.Text.Trim() == String.Empty)
        {
            Informacion += "<td>+Fecha probable de solución.</td>";
            Bln_Bandera = false;
            Generar_Tabla_Informacion();
        }
        if (Cmb_Dependencia.SelectedValue == "0")
        {
            Informacion += "<td>+Campo Unidad responsable.</td>";
            Bln_Bandera = false;
            Generar_Tabla_Informacion();
        }
        if (Cmb_Asunto.SelectedValue == "0")
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
        Cmb_Origen.Enabled = false;
        Txt_Nombre.Enabled = false;
        Txt_Apellido_Paterno.Enabled = false;
        Txt_Apellido_Materno.Enabled = false;
        Txt_Edad.Enabled = false;
        Cmb_Sexo.Enabled = false;
        Cmb_Calle.Enabled = false;
        Txt_Numero_Exterior.Enabled = false;
        Txt_Numero_Interior.Enabled = false;
        Txt_Referencia.Enabled = false;
        Cmb_Colonia.Enabled = false;
        Txt_Codigo_Postal.Enabled = false;
        Txt_Telefono.Enabled = false;
        Txt_Email.Enabled = false;
        Txt_Fecha_Solucion.Enabled = Habilitar;
        Txt_Peticion.Enabled = false;
        Cmb_Asunto.Enabled = Habilitar;
        Cmb_Accion.Enabled = Habilitar;
        Cmb_Dependencia.Enabled = Habilitar;

        Btn_Buscar_Accion.Enabled = Habilitar;
        Btn_Buscar_Asunto.Enabled = Habilitar;
        Btn_Buscar_Dependencia.Enabled = Habilitar;
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
        Cmb_Origen.SelectedIndex = 0;
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
        Txt_Peticion.Text = String.Empty;
        Cmb_Dependencia.SelectedIndex = 0;
        Cmb_Asunto.SelectedIndex = 0;
        Cmb_Accion.SelectedIndex = 0;
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
        Obj_Combo.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<SELECCIONAR>"), "0"));
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
        Obj_Combo.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<SELECCIONAR>"), "0"));
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
        var Obj_Dependencias = new Cls_Cat_Dependencias_Negocio();
        var Obj_Colonias = new Cls_Cat_Ate_Colonias_Negocio();
        var Obj_Programas = new Cls_Cat_Ate_Programas_Negocio();
        var Obj_Asunto = new Cls_Cat_Ate_Asuntos_Negocio();
        var Obj_Acciones = new Cls_Cat_Ate_Acciones_Negocio();

        try
        {
            // Combo de Sexo
            Cmb_Sexo.Items.Add("<SELECCIONAR>");
            Cmb_Sexo.Items[0].Value = "0";
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
            Llenar_Combo_Con_DataTable(Cmb_Dependencia, Obj_Dependencias.Consulta_Dependencias());
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
                Btn_Nuevo.Visible = false;
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
                break;
            //Estado de Nuevo
            case "Nuevo":
                Txt_Nombre.Focus();
                Btn_Nuevo.ToolTip = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Modificar.Visible = false;
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Txt_Busqueda_Registro_Peticion.Text = String.Empty;
                Txt_Busqueda_Registro_Peticion.Enabled = false;
                Btn_Buscar_Registro_Peticion.Enabled = false;
                Grid_Peticiones.Enabled = false;
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
        DateTime Fecha_Solucion;

        Obj_Peticiones.P_Programa_ID = Cmb_Origen.SelectedValue;
        Obj_Peticiones.P_Origen = Cmb_Origen.SelectedItem.Text;
        Obj_Peticiones.P_Nombre = Txt_Nombre.Text;
        Obj_Peticiones.P_Apellido_Paterno = Txt_Apellido_Paterno.Text;
        Obj_Peticiones.P_Apellido_Materno = Txt_Apellido_Materno.Text;
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
        Obj_Peticiones.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
        Obj_Peticiones.P_Asunto_ID = Cmb_Asunto.SelectedValue;
        Obj_Peticiones.P_Accion_ID = Cmb_Accion.SelectedValue;
        Obj_Peticiones.P_Estatus = "EN PROCESO";
        Obj_Peticiones.P_Asignado = "S";
        if (Grid_Peticiones.SelectedIndex > -1)
        {
            int Anio_Peticion;
            int.TryParse(Grid_Peticiones.SelectedRow.Cells[2].Text, out Anio_Peticion);
            Obj_Peticiones.P_No_Peticion = Grid_Peticiones.SelectedRow.Cells[1].Text;
            Obj_Peticiones.P_Anio_Peticion = Anio_Peticion;
        }
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
        Cls_Cat_Ate_Peticiones_Negocio Obj_Peticiones;
        Obj_Peticiones = new Cls_Cat_Ate_Peticiones_Negocio();
        try
        {
            Registra_Datos_Negocio(Obj_Peticiones, "Nuevo");
            if (Obj_Peticiones.Alta_Peticion() > 0)
            {
                Txt_Folio.Text = Obj_Peticiones.P_Folio;
                Imprimir_Reporte(Crear_Ds_Imprimir_Reporte(), "Rpt_Ope_Ate_Folio_Peticion.rpt", "Folio_Reporte_Ciudadano");
                Habilitar_Botones("Inicial");
                Limpiar_Campos();
                Habilitar_Campos(false);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('La Petición fue registrada.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('La Petición no pudo ser registrada.');", true);
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("La petición no pudo ser registrada: " + Ex, true);
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
        Cls_Cat_Ate_Peticiones_Negocio Obj_Peticiones = new Cls_Cat_Ate_Peticiones_Negocio();

        try
        {
            Registra_Datos_Negocio(Obj_Peticiones, "Modificar");
            Obj_Peticiones.P_Folio = Txt_Folio.Text;
            if (Obj_Peticiones.Modificar_Peticion() > 0)
            {
                Habilitar_Botones("Inicial");
                Limpiar_Campos();
                Habilitar_Campos(false);
                Grid_Peticiones.SelectedIndex = -1;
                Consultar_Peticiones();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('La Petición fue actualizada.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('No fue posible actualizar la Petición.');", true);
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("La petición no pudo ser modificada: " + Ex, true);
        }
        finally
        {
            Obj_Peticiones = null;
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Consultar_Peticiones
    ///DESCRIPCIÓN: Consulta las peticiones de acuerdo con el criterior especificado en el campo de búsqueda
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
            // si se especifió un folio, agregar al la propiedad para la consulta
            if (Txt_Busqueda_Registro_Peticion.Text.Trim() != "")
            {
                Obj_Peticiones.P_Folio = Txt_Busqueda_Registro_Peticion.Text.Trim();
            }
            // agregar filtros dinamicos (filtrar por programa ID en parámetros y acción o asunto nulo)
            Obj_Peticiones.P_Filtros_Dinamicos = Ope_Ate_Peticiones.Campo_Programa_ID + " = (SELECT "
                + Cat_Ven_Parametros.Campo_Programa_ID_Web + " FROM "
                + Cat_Ven_Parametros.Tabla_Cat_Ven_Parametros + " WHERE ROWNUM =1) AND (" + Ope_Ate_Peticiones.Campo_Accion_ID + " IS NULL OR " + Ope_Ate_Peticiones.Campo_Asunto_ID + " IS NULL) ";

            Dt_Peticiones = Obj_Peticiones.Consulta_Peticion();

            Cargar_Datos_Grid(Grid_Peticiones, Dt_Peticiones, "FECHA_PETICION DESC");

            // almacenar datatable en variable de sesión
            Session["Dt_Peticiones"] = Dt_Peticiones; DataView Dv_Ordenar = new DataView(Dt_Peticiones);
            ViewState["SortDirection"] = "DESC";
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al consultar Peticiones: [" + Ex + "]", true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cargar_Datos_Peticion
    ///DESCRIPCIÓN: Consulta los datos de la petición expecificada en los constroles correspondientes
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
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al cargar Peticiones: [" + Ex + "]", true);
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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Imprimir_Reporte
    ///DESCRIPCIÓN          : Crea un Dataset con los datos de la petición (toma los datos de la página)
    ///PARAMETROS: 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 18-may-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Imprimir_Reporte()
    {
        var Ds_Reporte = new Ds_Ope_Consulta_Peticiones_Especifico();
        DataRow Dr_Fila_Reporte = Ds_Reporte.Tables[0].NewRow();
        string Domicilio = "";
        // formar domicilio primero con la referencia
        if (Txt_Referencia.Text.Trim().Length > 0)
        {
            Domicilio += Txt_Referencia.Text + " ";
        }
        // si hay una calle
        if (Cmb_Calle.SelectedIndex > 0)
        {
            Domicilio += Cmb_Calle.SelectedItem.Text + " ";
        }

        Domicilio += Txt_Numero_Exterior.Text + " " + Txt_Numero_Interior.Text + " " + Cmb_Colonia.SelectedItem.Text;

        // insertar datos en la fila instanciada directamente de los controles en pantalla
        Dr_Fila_Reporte["Folio"] = Txt_Folio.Text;
        Dr_Fila_Reporte["Fecha_Peticion"] = DateTime.Now.ToString("dd/MMM/yyyy");
        Dr_Fila_Reporte["Nombre_Solicitante"] = Txt_Apellido_Paterno.Text + " " + Txt_Apellido_Materno.Text + " " + Txt_Nombre.Text;
        Dr_Fila_Reporte["Direccion"] = Domicilio;
        Dr_Fila_Reporte["E_mail"] = Txt_Email.Text;
        Dr_Fila_Reporte["Peticion"] = Txt_Peticion.Text;
        Dr_Fila_Reporte["Asunto"] = Cmb_Asunto.SelectedItem.Text;
        Dr_Fila_Reporte["Respuesta"] = Cmb_Accion.SelectedItem.Text;
        Dr_Fila_Reporte["Telefono"] = Txt_Telefono.Text;
        Dr_Fila_Reporte["Dependencia"] = Cmb_Dependencia.SelectedItem.Text;
        // agregar fila a la tabla
        Ds_Reporte.Tables[0].Rows.Add(Dr_Fila_Reporte);

        return Ds_Reporte;
    }

    #endregion Métodos

    #region Eventos

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Dependiendo del estado del botón (tooltipo Nuevo o Dar de alta)
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
            Cargar_Datos_Grid(Grid_Peticiones, null, null);
            Habilitar_Campos(true);
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
                if (Txt_Folio.Text != String.Empty)
                {
                    Habilitar_Campos(true);
                    Habilitar_Botones("Modificar");
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
    ///NOMBRE_FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Dependiendo del estado del botón (tooltipo: Inicio o Cancelar)
    ///         Redireccionar a la página principal o limpiar controles y volver a consultar peticiones pendientes
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 18-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.ToolTip == "Inicio")
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Habilitar_Botones("Inicial");
            Limpiar_Campos();
            Habilitar_Campos(false);
            Grid_Peticiones.SelectedIndex = -1;
            Consultar_Peticiones();
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Buscar_Registro_Peticion_Click
    ///DESCRIPCIÓN: Manejo del evento click en el botón de búsqueda: llama al método que realiza la búsqueda de peticiones
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
            Mostrar_Informacion("No se pudo mostrar información: " + Ex, true);
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
        DataTable Dt_Asuntos;

        try
        {
            // si hay una dependencia seleccionada, agregar filtro al Obj_Asuntos
            if (Cmb_Dependencia.SelectedIndex > 0)
            {
                Obj_Asuntos.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
            }

            // cargar el combo asuntos con los resultados de la consulta
            Llenar_Combo_Con_DataTable(Cmb_Asunto, Obj_Asuntos.Consultar_Registros());
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("No se pudo mostrar información: " + Ex, true);
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
        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_ASUNTOS"] != null)
        {
            // si el valor de la sesión es igual a true
            if (Convert.ToBoolean(Session["BUSQUEDA_ASUNTOS"]) == true)
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
        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_ACCIONES"] != null)
        {
            // si el valor de la sesión es igual a true
            if (Convert.ToBoolean(Session["BUSQUEDA_ACCIONES"]) == true)
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
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion(Ex.Message, true);
        }
    }

    #endregion Eventos

}
