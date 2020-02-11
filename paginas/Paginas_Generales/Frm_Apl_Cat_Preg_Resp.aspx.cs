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

using Presidencia.Preguntas_Respuestas.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;

public partial class paginas_Paginas_Generales_Frm_Apl_Cat_Preg_Resp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            Configuracion_Inicial();
        }
    }


    #region (Metodos Generales)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Inicial
    ///DESCRIPCIÓN: Configuracion Inicial del Catalogo de Bancos
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 17/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Inicial()
    {
        Habilitar_Controles("Inicial");
        Limpiar_Controles();
        Consultar_Preguntas();
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Ctlr
    /// DESCRIPCION : Limpia los Controles de la pagina.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 17/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        Txt_Pregunta_Respuesta_ID.Text = "";
        Txt_Pregunta.Text = "";
        Txt_Respuesta.Text = "";

        Grid_Preguntas_Respuesta.SelectedIndex = -1;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 17/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado;

        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    //Campos de Busqueda
                    Txt_Busqueda_Preguntas_Respuestas.Enabled = true;
                    Btn_Busqueda_Preguntas_Respuestas.Enabled = true;
                    //Campo de Validacion
                    Lbl_Mensaje_Error.Text = "";
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;

                    Txt_Pregunta.Enabled = false;
                    Txt_Respuesta.Enabled = false;
                    break;
                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    //Campos de Busqueda
                    Txt_Busqueda_Preguntas_Respuestas.Enabled = false;
                    Btn_Busqueda_Preguntas_Respuestas.Enabled = false;
                    Txt_Pregunta.Enabled = true;
                    break;
                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    //Campos de Busqueda
                    Txt_Busqueda_Preguntas_Respuestas.Enabled = false;
                    Btn_Busqueda_Preguntas_Respuestas.Enabled = false;
                    Txt_Respuesta.Enabled = true;
                    break;
            }
            Txt_Pregunta_Respuesta_ID.Enabled = false;
            Grid_Preguntas_Respuesta.Enabled = !Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    #endregion


    private void Alta_Pregunta() {
        Cls_Apl_Cat_Preg_Resp_Negocios Obj_Preg_Resp = new Cls_Apl_Cat_Preg_Resp_Negocios();

        Obj_Preg_Resp.P_Pregunta = Txt_Pregunta.Text.Trim();
        Obj_Preg_Resp.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;

        if (Obj_Preg_Resp.Alta_Preguntas()) {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operacion Completa');", true);
        }
    }

    private void Modificar_Pregunta()
    {
                Cls_Apl_Cat_Preg_Resp_Negocios Obj_Preg_Resp = new Cls_Apl_Cat_Preg_Resp_Negocios();

                Obj_Preg_Resp.P_Preg_Resp_ID = Txt_Pregunta_Respuesta_ID.Text.Trim();
                Obj_Preg_Resp.P_Respuesta = Txt_Respuesta.Text.Trim();
                Obj_Preg_Resp.P_Usuario_Modifico = Cls_Sessiones.Nombre_Empleado;

                if (Obj_Preg_Resp.Modificar_Preguntas())
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operacion Completa');", true);
                }
    }

    private void Consultar_Preguntas() {
        Cls_Apl_Cat_Preg_Resp_Negocios Obj_Preg_Resp = new Cls_Apl_Cat_Preg_Resp_Negocios();
        DataTable Dt_Listado_Preguntas = null;

        if (!String.IsNullOrEmpty(Txt_Busqueda_Preguntas_Respuestas.Text.Trim())) {
            Obj_Preg_Resp.P_Pregunta = Txt_Busqueda_Preguntas_Respuestas.Text;
        }

        Dt_Listado_Preguntas = Obj_Preg_Resp.Consultar_Preguntas();

        if (Dt_Listado_Preguntas is DataTable) {
            if (Dt_Listado_Preguntas.Rows.Count > 0) {
                Grid_Preguntas_Respuesta.Columns[1].Visible = true;
                Grid_Preguntas_Respuesta.DataSource = Dt_Listado_Preguntas;
                Grid_Preguntas_Respuesta.DataBind();
                Grid_Preguntas_Respuesta.SelectedIndex = -1;
                Grid_Preguntas_Respuesta.Columns[1].Visible = false;
            }
        }
    
    }

    protected void Btn_Nuevo_Click(object sender, EventArgs e) {
        try
        {
            if (Btn_Nuevo.ToolTip.Equals("Nuevo"))
            {
                Habilitar_Controles("Nuevo");
                Limpiar_Controles();
            }
            else
            {
                Alta_Pregunta();
                Configuracion_Inicial();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    protected void Btn_Modificar_Click(object sender, EventArgs e)
    {

        //Verificar si su rol es jefe de dependencia, admin de modulo o admin de sistema
        DataTable Dt_Grupo_Rol = Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Rol_ID.ToString());
        if (Dt_Grupo_Rol != null)
        {
            String Grupo_Rol = Dt_Grupo_Rol.Rows[0][Apl_Cat_Roles.Campo_Grupo_Roles_ID].ToString();
            if (Grupo_Rol == "00001" /*|| Grupo_Rol == "00002"*/ )
            {
                //$$
                try
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;

                    if (Btn_Modificar.ToolTip.Equals("Modificar"))
                    {
                        if (Grid_Preguntas_Respuesta.SelectedIndex != -1 & !Txt_Pregunta_Respuesta_ID.Text.Equals(""))
                        {
                            Habilitar_Controles("Modificar");
                        }
                        else
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "Seleccione el registro que desea modificar sus datos <br>";
                        }
                    }
                    else
                    {
                        Modificar_Pregunta();
                        Configuracion_Inicial();
                    }
                }
                catch (Exception Ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = Ex.Message.ToString();
                }

                //$$

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Solo puede hacer preguntas');", true);
            }
        }     

    }

    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Inicio")
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Configuracion_Inicial();//Habilita los controles para la siguiente operación del usuario en el catálogo
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    protected void Btn_Busqueda_Preguntas_Respuestas_Click(object sender, EventArgs e) {
        try
        {
            Consultar_Preguntas();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    protected void Grid_Preguntas_Respuesta_SelectedIndexChanged(object sender, EventArgs e)
    {
        Txt_Pregunta_Respuesta_ID.Text = HttpUtility.HtmlDecode(Grid_Preguntas_Respuesta.SelectedRow.Cells[1].Text.Trim());
        Txt_Pregunta.Text = HttpUtility.HtmlDecode(Grid_Preguntas_Respuesta.SelectedRow.Cells[3].Text.Trim());
        Txt_Respuesta.Text = HttpUtility.HtmlDecode(Grid_Preguntas_Respuesta.SelectedRow.Cells[4].Text.Trim());
    }

    protected void Grid_Preguntas_Respuesta_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Preguntas_Respuesta.PageIndex = e.NewPageIndex;
            Consultar_Preguntas();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error cambiar de un de la tabla. Error: [" + Ex.Message + "]");
        }
    }
}
