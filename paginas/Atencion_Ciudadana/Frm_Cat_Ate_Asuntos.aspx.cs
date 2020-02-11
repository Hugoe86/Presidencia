using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Presidencia.Asuntos_AC.Negocio;
using Presidencia.Constantes;
using Presidencia.Dependencias.Negocios;

public partial class paginas_Atencion_Ciudadana_Frm_Cat_Ate_Asuntos : System.Web.UI.Page
{
    //Modos de formulario
    private const String MODO_LISTADO = "listado";
    private const String MODO_INICIO = "inicio";
    private const String MODO_NUEVO = "nuevo";
    private const String MODO_MODIFICAR = "modificar";
    //Estatus
    private const String ESTATUS_ACTIVO = "ACTIVO";
    private const String ESTATUS_INACTIVO = "INACTIVO";
    //Tool Tips
    private const String TOOLTIP_NUEVO = "Nuevo";
    private const String TOOLTIP_GUARDAR = "Guardar";
    private const String TOOLTIP_ACTUALIZAR = "Actualizar";
    private const String TOOLTIP_MODIFICAR = "Modificar";
    private const String TOOLTIP_CANCELAR = "Cancelar";
    private const String TOOLTIP_INICIO = "Inicio";
    private const String TOOLTIP_SALIR = "Salir";
    private const String TOOLTIP_ELIMINAR = "Eliminar";
    //Sesiones
    private static String P_Dt_Datos = "Dt_Datos_Asuntos";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Cargar_Datos_Iniciales();
            Llenar_Informacion_Grid();
            Manejo_Comandos(MODO_INICIO);

            // registro de scripts del lado del servidor para mostrar ventanas emergentes para búsqueda avanzada
            string Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergente/Frm_Busqueda_Avanzada_Dependencias.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Dependencia.Attributes.Add("onclick", Ventana_Modal);
        }
        Informacion_Formulario("", false);
    }

    #region METODOS

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cargar_Datos_Iniciales
    ///DESCRIPCIÓN: Consulta las dependencias de catálogo para cargarlas en el combo unidad responsable 
    ///         y agrega elementos al combo estatus
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Cargar_Datos_Iniciales() 
    {
        //llenar estatus
        Cmb_Estatus.Items.Clear();
        Cmb_Estatus.Items.Add("SELECCIONAR");
        Cmb_Estatus.Items.Add("ACTIVO");
        Cmb_Estatus.Items.Add("INACTIVO");
        Cmb_Estatus.Items[0].Selected = true;
        //llenar UR
        Cls_Cat_Dependencias_Negocio Dependencia_Negocio = new Cls_Cat_Dependencias_Negocio();
        Dependencia_Negocio.P_Estatus = "ACTIVO";
        DataTable Dt_UR = Dependencia_Negocio.Consulta_Dependencias();
        Dt_UR.DefaultView.Sort = "CLAVE_NOMBRE ASC";
        Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Unidad_Responsable, Dt_UR, "CLAVE_NOMBRE", "DEPENDENCIA_ID");
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Limpiar_Formulario
    ///DESCRIPCIÓN: Limpia el contenido de las cajas de texto en el formulario y la selección de los combos
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Limpiar_Formulario() 
    {
        Txt_Busqueda.Text = "";
        Txt_Clave.Text = "";
        Txt_Nombre.Text = "";
        Txt_Descripcion.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
        Cmb_Unidad_Responsable.SelectedIndex = 0;   
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Manejo_Comandos
    ///DESCRIPCIÓN: Configura los controles de acuerdo con el parámetro que recibe
    ///PARÁMETROS:
    /// 		1. Modo: especifica la configuración que se dará a los controles mediante una 
    /// 		        constante con el tipo de operación a realizar
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Manejo_Comandos(String Modo)
    {
        switch(Modo)
        {
            case MODO_LISTADO:

                break;
            case MODO_INICIO:
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";

                Btn_Nuevo.Visible = true;
                Btn_Modificar.Visible = true;
                Btn_Eliminar.Visible = true;
                Btn_Salir.Visible = true;
                Btn_Buscar.Enabled = true;
                Txt_Busqueda.Enabled = true;

                Btn_Nuevo.ToolTip = TOOLTIP_NUEVO;
                Btn_Modificar.ToolTip = TOOLTIP_MODIFICAR;
                Btn_Eliminar.ToolTip = TOOLTIP_ELIMINAR;
                Btn_Salir.ToolTip = TOOLTIP_INICIO;

                Txt_Clave.Enabled = false;                
                Txt_Nombre.Enabled = false;
                Txt_Descripcion.Enabled = false;
                
                Cmb_Estatus.Enabled = false;
                Cmb_Unidad_Responsable.Enabled = false;
                Grid_Datos.Enabled = true;
                Btn_Buscar_Dependencia.Enabled = false;

                Llenar_Informacion_Grid();
                Limpiar_Formulario();
                HF_ID.Value = "";
                break;
            case MODO_NUEVO:
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";

                Btn_Nuevo.Visible = true;
                Btn_Modificar.Visible = false;
                Btn_Eliminar.Visible = false;
                Btn_Salir.Visible = true;
                Btn_Buscar.Enabled = false;
                Txt_Busqueda.Enabled = false;

                Btn_Nuevo.ToolTip = TOOLTIP_GUARDAR;                                
                Btn_Salir.ToolTip = TOOLTIP_CANCELAR;

                Limpiar_Formulario();
                Txt_Clave.Enabled = true;
                Txt_Nombre.Enabled = true;
                Txt_Descripcion.Enabled = true;
                Cmb_Estatus.Enabled = true;
                Cmb_Unidad_Responsable.Enabled = true;
                Btn_Buscar_Dependencia.Enabled = true;
                Grid_Datos.Enabled = false;
                Cmb_Estatus.SelectedValue = ESTATUS_ACTIVO;
                HF_ID.Value = "";
                break;
            case MODO_MODIFICAR:
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";

                Btn_Nuevo.Visible = false;
                Btn_Modificar.Visible = true;
                Btn_Eliminar.Visible = false;
                Btn_Salir.Visible = true;
                Btn_Buscar.Enabled = false;
                Txt_Busqueda.Enabled = false;

                Btn_Modificar.ToolTip = TOOLTIP_ACTUALIZAR;
                Btn_Salir.ToolTip = TOOLTIP_CANCELAR;

                Txt_Clave.Enabled = true;
                Txt_Nombre.Enabled = true;
                Txt_Descripcion.Enabled = true;
                Cmb_Estatus.Enabled = true;
                Cmb_Unidad_Responsable.Enabled = true;
                Btn_Buscar_Dependencia.Enabled = true;
                Grid_Datos.Enabled = false;
                break;
        }

    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cargar_Informacion_Seleccionada
    ///DESCRIPCIÓN: Obtiene los detalles del registro cuyo ID recibe como parámetro de la tabla en 
    ///             variable de sesión y los muestra en el control correspondiente del formulario
    ///PARÁMETROS:
    /// 		1. ID: id del registro a mostrar
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Cargar_Informacion_Seleccionada(String ID) 
    {
        DataTable Dt_Tabla = Session[P_Dt_Datos] as DataTable;
        DataRow[] Datos_Seleccionados = Dt_Tabla.Select(Cat_Ate_Asuntos.Campo_AsuntoID + " = '" + ID + "'");
        Limpiar_Formulario();
        if (Datos_Seleccionados != null && Datos_Seleccionados.Length > 0)
        {
            Txt_Clave.Text = Datos_Seleccionados[0][Cat_Ate_Asuntos.Campo_Clave].ToString().Trim();
            Txt_Nombre.Text = Datos_Seleccionados[0][Cat_Ate_Asuntos.Campo_Nombre].ToString().Trim();
            Txt_Descripcion.Text = Datos_Seleccionados[0][Cat_Ate_Asuntos.Campo_Descripcion].ToString().Trim();

            // seleccionar el valor en el combo estatus y unidad responsable (validando que exista el elemento)
            if (Cmb_Estatus.Items.FindByValue(Datos_Seleccionados[0][Cat_Ate_Asuntos.Campo_Estatus].ToString().Trim()) != null)
            {
                Cmb_Estatus.SelectedValue = Datos_Seleccionados[0][Cat_Ate_Asuntos.Campo_Estatus].ToString().Trim();
            }
            if (Cmb_Unidad_Responsable.Items.FindByValue(Datos_Seleccionados[0][Cat_Ate_Asuntos.Campo_DependenciaID].ToString().Trim()) != null)
            {
                Cmb_Unidad_Responsable.SelectedValue = Datos_Seleccionados[0][Cat_Ate_Asuntos.Campo_DependenciaID].ToString().Trim();
            }
            else // posiblemente la UR (o dependecia) tiene estatus INACTIVO
            {
                // consultar información de UR y cargar al combo para poder seleccionar
                DataTable Dt_Dependencias = null;
                var Dependencias_Negocio = new Cls_Cat_Dependencias_Negocio();
                Dependencias_Negocio.P_Dependencia_ID = Datos_Seleccionados[0][Cat_Ate_Asuntos.Campo_DependenciaID].ToString().Trim();
                Dt_Dependencias= Dependencias_Negocio.Consulta_Dependencias();
                if (Dt_Dependencias != null && Dt_Dependencias.Rows.Count > 0)
                {
                    // agregar al combo y seleccionar
                    Cmb_Unidad_Responsable.Items.Add(new ListItem("(INACTIVO) " + Dt_Dependencias.Rows[0]["CLAVE_NOMBRE"].ToString(), Dt_Dependencias.Rows[0][Cat_Dependencias.Campo_Dependencia_ID].ToString()));
                    Cmb_Unidad_Responsable.SelectedValue = Dt_Dependencias.Rows[0][Cat_Dependencias.Campo_Dependencia_ID].ToString();
                }
            }
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Llenar_Informacion_Grid
    ///DESCRIPCIÓN: llama al método que consulta asuntos del catálogo de asuntos y los guarda en variable 
    ///             sesión y los carga en el grid datos.
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Llenar_Informacion_Grid() 
    {
        DataTable Dt_Datos = Consultar_Registros();
        Session[P_Dt_Datos] = Dt_Datos;
        Grid_Datos.DataSource = Dt_Datos;
        Grid_Datos.DataBind();
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Validar_Campos
    ///DESCRIPCIÓN: valida que los campos obligatorios contengan datos o en el caso de los combos que 
    ///         haya un elemento seleccionado
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private bool Validar_Campos()
    {
        bool Valido = true;
        if (Txt_Nombre.Text.Trim().Length == 0)
        {
            Valido = false;
        }
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            Valido = false;
        }
        if (Cmb_Unidad_Responsable.SelectedIndex == 0)
        {
            Valido = false;
        }
        else if (Btn_Nuevo.ToolTip == TOOLTIP_GUARDAR) // validar que la UR esté activa si es alta de asunto
        {
            // validar que la unidad responsable tenga estatus ACTIVO
            DataTable Dt_Dependencias = null;
            var Dependencias_Negocio = new Cls_Cat_Dependencias_Negocio();
            Dependencias_Negocio.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue;
            Dependencias_Negocio.P_Estatus = "ACTIVO";
            Dt_Dependencias = Dependencias_Negocio.Consulta_Dependencias();
            // si la consulta no regresa resultados, asignar falso (la UR no está activa)
            if (Dt_Dependencias == null || Dt_Dependencias.Rows.Count <= 0)
            {
                Valido = false;
            }
        }
        return Valido;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Consultar_Registros
    ///DESCRIPCIÓN: Regresa un datatable con los resultados de la consulta al catálogo de asuntos, si la 
    ///         caja de texto Búsqueda contiene texto, se agrega como parámetro a la consulta para filtrar por clave
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataTable Consultar_Registros() 
    {
        DataTable Dt_Tabla_Consulta = null;
        //Cargar datos de negocio y consultar
        Cls_Cat_Ate_Asuntos_Negocio Negocio = new Cls_Cat_Ate_Asuntos_Negocio();
        Negocio.P_Descripcion = Txt_Busqueda.Text.Trim();
        Negocio.P_Nombre = Txt_Busqueda.Text.Trim();
        Negocio.P_Clave = Txt_Busqueda.Text.Trim();
        Dt_Tabla_Consulta = Negocio.Consultar_Registros();
        return Dt_Tabla_Consulta;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Actualizar_Registro
    ///DESCRIPCIÓN: Pasa los datos del formulario a la capa de negocio para actualizar el registro 
    ///         seleccionado en la base de datos
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private int Actualizar_Registro() 
    {
        int Registros_Actualizados = 0;
        //Cargar datos de negocio y actualizar
        Cls_Cat_Ate_Asuntos_Negocio Negocio = new Cls_Cat_Ate_Asuntos_Negocio();
        Negocio.P_ID = HF_ID.Value;
        Negocio.P_Clave = Txt_Clave.Text.Trim();
        Negocio.P_Nombre = Txt_Nombre.Text.Trim();
        Negocio.P_Descripcion = Txt_Descripcion.Text.Trim();
        Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
        Negocio.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue.Trim();
        Negocio.P_UR_ID = Cmb_Unidad_Responsable.SelectedValue.Trim();
        Registros_Actualizados = Negocio.Actualizar_Registro();
        return Registros_Actualizados;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Eliminar_Registro
    ///DESCRIPCIÓN: llama al método en la capa de negocio que elimina el registro seleccionado 
    ///             (se toma del control oculto HF_ID)
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private int Eliminar_Registro()
    {
        int Registros_Eliminados = 0;
        //Cargar datos de negocio y actualizar
        Cls_Cat_Ate_Asuntos_Negocio Negocio = new Cls_Cat_Ate_Asuntos_Negocio();
        Negocio.P_ID = HF_ID.Value;
        Registros_Eliminados = Negocio.Eliminar_Registro();
        return Registros_Eliminados;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Nuevo_Registro
    ///DESCRIPCIÓN: Pasa los datos del formulario a la capa de negocio para dar de alta el nuevo registro 
    ///          en la base de datos
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private int Nuevo_Registro() 
    {
        int Registros_Actualizados = 0;
        //Cargar datos de negocio y registrar
        Cls_Cat_Ate_Asuntos_Negocio Negocio = new Cls_Cat_Ate_Asuntos_Negocio();
        Negocio.P_Clave = Txt_Clave.Text.Trim();
        Negocio.P_Nombre = Txt_Nombre.Text.Trim();
        Negocio.P_Descripcion = Txt_Descripcion.Text.Trim();
        Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
        Negocio.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue.Trim();
        Negocio.P_UR_ID = Cmb_Unidad_Responsable.SelectedValue.Trim();
        Registros_Actualizados = Negocio.Guardar_Registro();
        return Registros_Actualizados;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Clave_Duplicada
    ///DESCRIPCIÓN: Regresa verdadero la clave ingresada ya se encuentra en el catálogo, de no ser así, regresa falso
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    private bool Clave_Duplicada()
    {
        bool Duplicada = false;
        Cls_Cat_Ate_Asuntos_Negocio Negocio = new Cls_Cat_Ate_Asuntos_Negocio();
        Negocio.P_Clave = Txt_Clave.Text.Trim();
        Negocio.P_ID = HF_ID.Value;
        Duplicada = Negocio.Clave_Duplicada();
        return Duplicada;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Mensaje_PopUp
    ///DESCRIPCIÓN: Muestra una alerta con el texto recibido como parámetro mediante el scriptmanager 
    ///PARÁMETROS:
    /// 		1. Texto: cadena de texto a mostrar como mensaje
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Mensaje_PopUp(String Texto)
    {
        ScriptManager.RegisterStartupScript(
            this, this.GetType(), "Catalogo", "alert('" + Texto + "');", true);
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Informacion_Formulario
    ///DESCRIPCIÓN: Oculta o muestra la etiqueta e imagen de información del formulario dependiendo del 
    ///         parámetro recibido
    ///PARÁMETROS:
    /// 		1. Texto: cadena de texto a mostrar como mensaje
    /// 		2. Visible: boleano que indica si se oculta o muestra el mensaje de error 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Informacion_Formulario(String Texto, bool Visible) 
    {
        Lbl_Informacion.Text = Texto;
        Lbl_Informacion.Visible = Visible;
        Img_Informacion.Visible = Visible;
    }

    #endregion METODOS

    #region EVENTOS

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN: Manejador del evento click en el botón buscar que llama al método que consulta y 
    ///             llena el grid con los resultados obtenidos
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Llenar_Informacion_Grid();
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Manejador del evento click en el botón nuevo, llama al método que activa los 
    ///     controles para ingresar un nuevo registro y si ya se ingresó, llama los métodos de validación y alta
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {

        if (Btn_Nuevo.ToolTip == TOOLTIP_NUEVO)
        {
            Manejo_Comandos(MODO_NUEVO);
        }
        else if (Btn_Nuevo.ToolTip == TOOLTIP_GUARDAR)
        {
            // proceso guardar aquí
            if (Validar_Campos())
            {
                if (!Clave_Duplicada())
                {

                    int Registros_Nuevos = Nuevo_Registro();
                    if (Registros_Nuevos > 0)
                    {
                        Mensaje_PopUp("Se guardó el registro correctamente");
                        Manejo_Comandos(MODO_INICIO);
                    }
                    else
                    {
                        Mensaje_PopUp("No se pudo guardar la información. Verifique datos");
                    }
                }
                else
                {
                    Mensaje_PopUp("La clave ingresada ya es usada por otro registro");
                }
            }
            else if (Cmb_Unidad_Responsable.SelectedItem.Text.Contains("(INACTIVO)"))
            {
                Informacion_Formulario("No puede asignar una Unidad Responsable con estatus INACTIVO", true);
            }
            else
            {
                Informacion_Formulario("Los campos marcados con * son obligatorios", true);
            }
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Manejador del evento click del botón Modificar, llama al método que activa los controles 
    ///         para modificar los datos del registro seleccionado o los de validación y actualización de registro
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Modificar.ToolTip == TOOLTIP_MODIFICAR)
        {
            if (HF_ID.Value.Trim().Length > 0)
            {
                Manejo_Comandos(MODO_MODIFICAR);
            }
            else
            {
                Mensaje_PopUp("Seleccione un registro para modificar");
            }
        }
        else if (Btn_Modificar.ToolTip == TOOLTIP_ACTUALIZAR)
        {
            if (Validar_Campos())
            {
                //cargar datos de negocio
                if (!Clave_Duplicada())
                {
                    int Registros_Actualizados = Actualizar_Registro();
                    if (Registros_Actualizados > 0)
                    {
                        Mensaje_PopUp("Se actualizó el registro correctamente");
                        Manejo_Comandos(MODO_INICIO);
                    }
                    else
                    {
                        Mensaje_PopUp("No se pudo actualizar la información. Verifique datos");
                    }
                }
                else
                {
                    Mensaje_PopUp("La clave ingresada ya es usada por otro registro");
                }
            }
            else
            {
                Informacion_Formulario("Los campos marcados con * son obligatorios", true);
            }
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: Manejador del evento click en el botón eliminar, llama al método eliminar registro y valida
    ///         si se eliminaron o no registros para mostrar un mensaje con el resultado de la operación
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        if (Txt_Clave.Text.Trim().Length > 0)
        {
            //cargar datos de negocio

            int Registros_Nuevos = Eliminar_Registro();
            if (Registros_Nuevos > 0)
            {
                Mensaje_PopUp("El registro fue eliminado");
                Limpiar_Formulario();
                Manejo_Comandos(MODO_INICIO);
            }
            else
            {
                Mensaje_PopUp("No se eliminaron los datos, es posible que sea usado en otros registros");
            }
            Manejo_Comandos(MODO_INICIO);
        }
        else
        {
            Mensaje_PopUp("Seleccione un registro para eliminar");
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
        Informacion_Formulario("", false);

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
                    if (Cmb_Unidad_Responsable.Items.FindByValue(Dependencia_ID) != null)
                    {
                        Cmb_Unidad_Responsable.SelectedValue = Dependencia_ID;
                    }
                    else // posiblemente la UR (o dependecia) tiene estatus INACTIVO
                    {
                        // consultar información de UR y cargar al combo para poder seleccionar
                        DataTable Dt_Dependencias = null;
                        var Dependencias_Negocio = new Cls_Cat_Dependencias_Negocio();
                        Dependencias_Negocio.P_Dependencia_ID = Dependencia_ID;
                        Dt_Dependencias = Dependencias_Negocio.Consulta_Dependencias();
                        if (Dt_Dependencias != null && Dt_Dependencias.Rows.Count > 0)
                        {
                            // agregar al combo y seleccionar
                            Cmb_Unidad_Responsable.Items.Add(new ListItem("(INACTIVO) " + Dt_Dependencias.Rows[0]["CLAVE_NOMBRE"].ToString(), Dt_Dependencias.Rows[0][Cat_Dependencias.Campo_Dependencia_ID].ToString()));
                            Cmb_Unidad_Responsable.SelectedValue = Dt_Dependencias.Rows[0][Cat_Dependencias.Campo_Dependencia_ID].ToString();
                        }
                    }
                }
                catch (Exception Ex)
                {
                    Informacion_Formulario(Ex.Message, true);
                }

                // limpiar variables de sesión
                Session.Remove("DEPENDENCIA_ID");
                Session.Remove("NOMBRE_DEPENDENCIA");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_DEPENDENCIAS");
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Manejador del evento click en el botón salir, redirecciona a la página principal o cancela 
    ///         la operación actual dependiendo del contenido de la propiedad tooltip del botón
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.ToolTip == TOOLTIP_INICIO)
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else if (Btn_Salir.ToolTip == TOOLTIP_CANCELAR)
        {
            //proceso cancelar aquí
            Limpiar_Formulario();
            Manejo_Comandos(MODO_INICIO);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Seleccionar_Click
    ///DESCRIPCIÓN: Manejador del evento click sobre uno de los elementos del grid, que llama al método 
    ///         que carga los detalles del registro seleccionado (mediante el commandArgument obtiene 
    ///         el ID del registro) en los controles del formulario
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Seleccionar_Click(object sender, ImageClickEventArgs e)
    {
        String ID = ((ImageButton)sender).CommandArgument;
        HF_ID.Value = ID;
        Cargar_Informacion_Seleccionada(ID);
    }

    #endregion EVENTOS

}
