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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Programas_AC.Negocio;
using Presidencia.Catalogo_Atencion_Ciudadana_Parametros.Negocio;

public partial class paginas_Atencion_Ciudadana_Frm_Cat_Ate_Parametros : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Mostrar_Informacion("", false);

        if (!IsPostBack)
        {
            Habilitar_Controles("Inicial");
            LLenar_Combos();
            Consultar_Parametros();
        }
    }

    #region METODOS

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Habilitar_Botones
    ///DESCRIPCIÓN: Establece las propiedades de los botones modificar y salir y del combo programa
    ///         dependiendo del contenido del parámetro recibido.
    ///PARÁMETROS:
    /// 		1. Estado: indica la operación que se pretende realizar y para la que se van a preparar los controles
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 04-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Habilitar_Controles(String Estado)
    {
        switch (Estado)
        {
            //Estado Incicial de los controles
            case "Inicial":
                Btn_Modificar.Visible = true;
                Btn_Salir.Visible = true;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Salir.ToolTip = "Inicio";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Cmb_Programa_Atencion.Enabled = false;
                Cmb_Programa_Ventanilla_Web.Enabled = false;
                Cmb_Programa_Formato_Consecutivo.Enabled = false;
                break;
            //Estado de Modificar
            case "Modificar":
                Btn_Modificar.ToolTip = "Actualizar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Cmb_Programa_Atencion.Enabled = true;
                Cmb_Programa_Ventanilla_Web.Enabled = true;
                Cmb_Programa_Formato_Consecutivo.Enabled = true;
                break;
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: LLenar_Combos
    ///DESCRIPCIÓN: Consulta los programas en cat_ate_programas y carga los datos de la consulta en el combo
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 04-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void LLenar_Combos()
    {
        var Obj_Programas = new Cls_Cat_Ate_Programas_Negocio();
        DataTable Dt_Programas;
        try
        {
            // filtrar sólo programas con estatus ACTIVO
            Obj_Programas.P_Estatus = "ACTIVO";
            Dt_Programas = Obj_Programas.Consultar_Registros();
            // limpiar elementos del combo y agregar datos consultados
            Cmb_Programa_Atencion.Items.Clear();
            Cmb_Programa_Atencion.SelectedValue = null;
            Cmb_Programa_Atencion.DataSource = Dt_Programas;
            Cmb_Programa_Atencion.DataTextField = Cat_Ate_Programas.Campo_Nombre;
            Cmb_Programa_Atencion.DataValueField = Cat_Ate_Programas.Campo_Programa_ID;
            Cmb_Programa_Atencion.DataBind();
            Cmb_Programa_Atencion.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<SELECCIONAR>"), ""));
            Cmb_Programa_Atencion.SelectedIndex = 0;
            // limpiar elementos del combo y agregar datos consultados
            Cmb_Programa_Ventanilla_Web.Items.Clear();
            Cmb_Programa_Ventanilla_Web.SelectedValue = null;
            Cmb_Programa_Ventanilla_Web.DataSource = Dt_Programas;
            Cmb_Programa_Ventanilla_Web.DataTextField = Cat_Ate_Programas.Campo_Nombre;
            Cmb_Programa_Ventanilla_Web.DataValueField = Cat_Ate_Programas.Campo_Programa_ID;
            Cmb_Programa_Ventanilla_Web.DataBind();
            Cmb_Programa_Ventanilla_Web.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<SELECCIONAR>"), ""));
            Cmb_Programa_Ventanilla_Web.SelectedIndex = 0;
            // limpiar elementos del combo y agregar datos consultados
            Cmb_Programa_Formato_Consecutivo.Items.Clear();
            Cmb_Programa_Formato_Consecutivo.SelectedValue = null;
            Cmb_Programa_Formato_Consecutivo.DataSource = Dt_Programas;
            Cmb_Programa_Formato_Consecutivo.DataTextField = Cat_Ate_Programas.Campo_Nombre;
            Cmb_Programa_Formato_Consecutivo.DataValueField = Cat_Ate_Programas.Campo_Programa_ID;
            Cmb_Programa_Formato_Consecutivo.DataBind();
            Cmb_Programa_Formato_Consecutivo.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<SELECCIONAR>"), "0"));
            Cmb_Programa_Formato_Consecutivo.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al cargar programas: " + Ex, true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Consultar_Parametros
    ///DESCRIPCIÓN: Consulta los parámetros y si exiten en el combo correspondiente, lo selecciona
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 04-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Consultar_Parametros()
    {
        var Obj_Parametros = new Cls_Cat_Ate_Parametros_Negocio();
        DataTable Dt_Parametros;
        try
        {
            // obtener los parámetros
            Dt_Parametros = Obj_Parametros.Consultar_Parametros();

            // si se obtuvieron resultados, tratar de cargar en los combos
            if (Dt_Parametros != null && Dt_Parametros.Rows.Count > 0)
            {
                // si el programa id existe en el combo, seleccionarlo
                string Programa_Id = Dt_Parametros.Rows[0][Cat_Ven_Parametros.Campo_Programa_ID_Ventanilla].ToString();
                if (Cmb_Programa_Atencion.Items.FindByValue(Programa_Id) != null)
                {
                    Cmb_Programa_Atencion.SelectedValue = Programa_Id;
                }
                // si el programa id existe en el combo, seleccionarlo
                Programa_Id = Dt_Parametros.Rows[0][Cat_Ven_Parametros.Campo_Programa_ID_Web].ToString();
                if (Cmb_Programa_Ventanilla_Web.Items.FindByValue(Programa_Id) != null)
                {
                    Cmb_Programa_Ventanilla_Web.SelectedValue = Programa_Id;
                }
                // si el programa id existe en el combo, seleccionarlo
                Programa_Id = Dt_Parametros.Rows[0][Cat_Ven_Parametros.Campo_Programa_ID_Genera_Consecutivo].ToString();
                if (Cmb_Programa_Formato_Consecutivo.Items.FindByValue(Programa_Id) != null)
                {
                    Cmb_Programa_Formato_Consecutivo.SelectedValue = Programa_Id;
                }
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al cargar parámetros: " + Ex, true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Mostrar_Informacion
    ///DESCRIPCIÓN: Muestra en la página el mensaje recibido como parámetro y establece la visibilidad 
    ///             de los controles  para mostrar mensajes con el segundo parámetro
    ///PARÁMETROS:
    /// 		1. Mensaje: Texto a mostrar en la página
    /// 		2. Mostrar: establece la visibilidad de los controles en los que se muestran los mensajes de la página
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 04-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Mostrar_Informacion(String Mensaje, bool Mostrar)
    {
        Lbl_Informacion.Text = Mensaje;
        Lbl_Informacion.Visible = Mostrar;
        Img_Informacion.Visible = Mostrar;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Modificar_Parametros
    ///DESCRIPCIÓN: Consulta los parámetros y si exiten en el combo correspondiente, lo selecciona
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 04-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Modificar_Parametros()
    {
        var Obj_Parametros = new Cls_Cat_Ate_Parametros_Negocio();

        try
        {
            // agregar valores para actualizar
            Obj_Parametros.P_Programa_ID_Ventanilla = Cmb_Programa_Atencion.SelectedValue;
            Obj_Parametros.P_Programa_ID_Web = Cmb_Programa_Ventanilla_Web.SelectedValue;
            Obj_Parametros.P_Programa_ID_Genera_Consecutivo = Cmb_Programa_Formato_Consecutivo.SelectedValue;
            Obj_Parametros.P_Usuario = Cls_Sessiones.Nombre_Empleado;

            if (Obj_Parametros.Actualizar_Parametros() > 0)
            {
                Habilitar_Controles("Inicial");
                Consultar_Parametros();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('Actualización exitosa.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('No fue posible realizar la actualización.');", true);
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al actualizar parámetros: " + Ex, true);
        }
    }

    #endregion METODOS


    #region EVENTOS

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Dependiendo del estado del botón (tooltipo: Modificar o Actualizar)
    ///         Configurar controles o actualiza el parametro
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 04-jun-2012
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
            // validar estado del botón
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                Habilitar_Controles("Modificar");
            }
            else
            {
                // validar que haya un programa seleccionado
                if (Cmb_Programa_Ventanilla_Web.SelectedIndex > 0 && Cmb_Programa_Formato_Consecutivo.SelectedIndex > 0)
                {
                    Modificar_Parametros();
                }
                else
                {
                    Mostrar_Informacion("Es necesario seleccionar un programa.", true);
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
    ///DESCRIPCIÓN: Manejo del evento click en el botón de salir: dependiendo del tooltip del botón, regresa a 
    ///         la página principal o reinicia los controles de la página a su estado Inicial
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 04-jun-2012
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
            Habilitar_Controles("Inicial");
            Consultar_Parametros();
        }
    }

    #endregion EVENTOS

}
