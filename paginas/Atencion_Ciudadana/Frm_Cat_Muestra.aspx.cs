using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class paginas_Atencion_Ciudadana_Frm_Cat_Ate_Acciones : System.Web.UI.Page
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Cargar_Datos_Iniciales(); 
            Manejo_Controles(MODO_INICIO);
        }
        
    }
    private void Cargar_Datos_Iniciales() 
    {
        Cmb_Estatus.Items.Clear();
        Cmb_Estatus.Items.Add("SELECCIONAR");
        Cmb_Estatus.Items.Add("ACTIVO");
        Cmb_Estatus.Items.Add("INACTIVO");
        Cmb_Estatus.Items[0].Selected = true;
    }

    private void Limpiar_Formulario() 
    {
        Txt_Busqueda.Text = "";
        Txt_Clave.Text = "";
        Txt_Nombre.Text = "";
        Txt_Descripcion.Text = "";        
    }

    private void Manejo_Controles(String Modo)
    {
        switch(Modo)
        {
            case MODO_LISTADO:

                break;
            case MODO_INICIO:
                Btn_Nuevo.Visible = true;
                Btn_Modificar.Visible = true;
                Btn_Eliminar.Visible = true;
                Btn_Salir.Visible = true;

                Btn_Nuevo.ToolTip = TOOLTIP_NUEVO;
                Btn_Modificar.ToolTip = TOOLTIP_MODIFICAR;
                Btn_Eliminar.ToolTip = TOOLTIP_ELIMINAR;
                Btn_Salir.ToolTip = TOOLTIP_INICIO;
                break;
            case MODO_NUEVO:
                Btn_Nuevo.Visible = true;
                Btn_Modificar.Visible = false;
                Btn_Eliminar.Visible = false;
                Btn_Salir.Visible = true;

                Btn_Nuevo.ToolTip = TOOLTIP_GUARDAR;                                
                Btn_Salir.ToolTip = TOOLTIP_CANCELAR;

                break;
            case MODO_MODIFICAR:
                Btn_Nuevo.Visible = false;
                Btn_Modificar.Visible = true;
                Btn_Eliminar.Visible = false;
                Btn_Salir.Visible = true;
                
                Btn_Modificar.ToolTip = TOOLTIP_ACTUALIZAR;
                Btn_Salir.ToolTip = TOOLTIP_CANCELAR;
                break;
        }

    }

    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        
    }

    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        Manejo_Controles(MODO_NUEVO);
    }

    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        Manejo_Controles(MODO_MODIFICAR);
    }

    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Manejo_Controles(MODO_INICIO);
    }
}
