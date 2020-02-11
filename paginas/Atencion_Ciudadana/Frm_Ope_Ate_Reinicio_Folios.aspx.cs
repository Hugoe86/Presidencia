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
using AjaxControlToolkit;
using Presidencia.Constantes;
using Presidencia.Operacion_Atencion_Ciudadana_Reiniciar_Folios.Negocios;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Ventanilla_Usarios.Negocio;

public partial class paginas_Operacion_Atencion_Ciudadana_Frm_Ope_Ate_Reinicio_Folios : System.Web.UI.Page
{
    #region PageLoad/Init
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Habilitar_Botones("Inicial");
            Limpiar_Campos();
            LLenar_Combos();
        }
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
    ///FECHA_CREO: 24-jun-2012
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
    ///NOMBRE_FUNCIÓN: Validar_Campos_Obligatorios
    ///DESCRIPCIÓN: Valida el contenido de los campos obligatorios del formulario
    ///         regresa un mensaje con los errores encontrados o un texto vacío si no encuentra errores
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 24-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private string Validar_Campos_Obligatorios()
    {
        string Errores_Encontrados = "";

        // validar que haya un texto para prefijo
        if (Txt_Prefijo.Text.Trim().Length <= 0)
        {
            Errores_Encontrados += " + Ingresar un prefijo para anteponer a los folios que se van a cambiar.<br />";
        }
        // validar que se haya seleccionado un programa
        if (Cmb_Origen.SelectedIndex <= 0)
        {
            Errores_Encontrados += " + Seleccionar un programa (origen) para actualizar.<br />";
        }

        return Errores_Encontrados;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Limpiar_Campos
    ///DESCRIPCIÓN: Limpiar las cajas de texto y quitar la selección de los combos
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 24-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Limpiar_Campos()
    {
        //Se limpian los campos
        Txt_Prefijo.Text = "";
        Cmb_Origen.SelectedIndex = -1;
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
    ///FECHA_CREO: 24-jun-2012
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
    ///FECHA_CREO: 24-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void LLenar_Combos()
    {
        var Obj_Programas = new Cls_Cat_Ate_Programas_Negocio();

        try
        {
            // Combo de Programas
            Obj_Programas.P_Estatus = "ACTIVO";
            Llenar_Combo_Con_DataTable(Cmb_Origen, Obj_Programas.Consultar_Registros(), 0, 2);
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
    ///FECHA_CREO: 24-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Habilitar_Botones(String Estado)
    {
        bool Habilitar = false;

        switch (Estado)
        {
            //Estado Incicial de los controles
            case "Inicial":
                Habilitar = false;
                Btn_Reiniciar.Visible = true;
                Btn_Salir.Visible = true;
                Btn_Reiniciar.ToolTip = "Reiniciar folios";
                Btn_Salir.ToolTip = "Inicio";
                Btn_Reiniciar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                break;
            //Estado Nuevo reinicio de folios
            case "Nuevo":
                Habilitar = true;
                Btn_Reiniciar.ToolTip = "Dar de Alta";
                Btn_Reiniciar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                break;
        }

        Txt_Prefijo.Enabled = Habilitar;
        Cmb_Origen.Enabled = Habilitar;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Reiniciar_Folios
    ///DESCRIPCIÓN: llama a los métodos para actualizar una los datos de la peticiones con los datos ingresados
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 24-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Reiniciar_Folios()
    {
        Cls_Ope_Ate_Reiniciar_Folios_Negocio Obj_Peticiones = new Cls_Ope_Ate_Reiniciar_Folios_Negocio();
        int Registros_Afectados = -1;

        try
        {
            // asignar propiedades
            Obj_Peticiones.P_Programa_ID = Cmb_Origen.SelectedValue;
            Obj_Peticiones.P_Prefijo_Folio = Txt_Prefijo.Text.Trim();
            Obj_Peticiones.P_Usuario_Modifico = Cls_Sessiones.Nombre_Empleado;
            // llamar al método que ejecuta las consultas de actualización
            Registros_Afectados = Obj_Peticiones.Reiniciar_Folios();
            if (Registros_Afectados > 0)
            {
                Habilitar_Botones("Inicial");
                Limpiar_Campos();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('Registros actualizados correctamente.');", true);
            }
            else if (Registros_Afectados == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('No hay registros para afectar con el programa seleccionado.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('No fue posible actualizar los registros.');", true);
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al actualizar registros: " + Ex.Message, true);
        }
        finally
        {
            Obj_Peticiones = null;
        }
    }

    #endregion Métodos

    #region Eventos

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Reiniciar_Click
    ///DESCRIPCIÓN: Dependiendo del estado del botón (tooltipo: Reiniciar o Actualizar)
    ///         Configurar controles o actualiza una los registros llamando al método indicado
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 24-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Reiniciar_Click(object sender, ImageClickEventArgs e)
    {
        // limpiar mensajes de error
        Mostrar_Informacion("", false);

        try
        {
            if (Btn_Reiniciar.ToolTip == "Reiniciar folios")
            {
                Habilitar_Botones("Nuevo");
            }
            else
            {
                string Mensaje_Error = "";
                Mensaje_Error = Validar_Campos_Obligatorios();
                if (Mensaje_Error.Length <= 0)
                {
                    Reiniciar_Folios();
                }
                else
                {
                    Mostrar_Informacion("Es necesario:<br />" + Mensaje_Error, true);
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
    ///DESCRIPCIÓN: Manejo del evento clic en el botón de salir: dependiendo del tooltip del botón, regresa a 
    ///         la página principal o reinicia los controles de la página a su estado Inicial
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 24-jun-2012
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
            Habilitar_Botones("Inicial");
            Limpiar_Campos();
        }
    }

    #endregion Eventos

}
