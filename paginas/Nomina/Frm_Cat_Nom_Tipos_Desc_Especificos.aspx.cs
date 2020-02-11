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
using Presidencia.Tipos_Descuentos_Especificos.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Text;

public partial class paginas_Nomina_Cls_Cat_Nom_Tipos_Desc_Especificos : System.Web.UI.Page
{
    #region (Load/Init)
    /// ************************************************************************************************************************************************************
    /// Nombre: Page_Load
    /// 
    /// Descripción: Metodo que carga y habilita la configuracion inicial d ela pagina.
    /// 
    /// Parámetros:
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Configuracion_Inicial();//Carga la configuración inicial de la página
            }

            Mostrar_Mensaje(false, String.Empty);
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(true, Ex.Message);
        }
    }
    #endregion

    #region (Métodos)

    #region (Generales)
    /// ************************************************************************************************************************************************************
    /// Nombre: Configuracion_Inicial
    /// 
    /// Descripción: Método que carga la configuración inicial de la página.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    private void Configuracion_Inicial()
    {
        try
        {
            Limpiar_Controles();//Limpia los controles de la pagina.
            Habilitar_Controles("Inicial");//Habilita la configuración inicial de la página.
            Cargar_Grid(Consultar_Claves());//Cargamos el grid de claves del tipo de desc esp.
            Consultar_Claves_Cargo_Abono();//Consultamos las claves de cargo/abono.
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(true, "Error al cargar la configuración inicial de la página. Error: [" + Ex.Message + "]");
        }
    }
    /// ************************************************************************************************************************************************************
    /// Nombre: Limpiar_Controles
    /// 
    /// Descripción: Método que limpia los controles de la página.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Txt_Clave_Tipos_Desc_Esp.Text = String.Empty;
            Txt_Descripcion.Text = String.Empty;
            HTxt_Tipo_Desc_Esp_ID.Value = String.Empty;
            Cmb_Clave_Cargo_Abono.SelectedIndex = (-1);
            Grid_Tipo_Desc_Esp.SelectedIndex = (-1);
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(true, "Error al limpiar los controles de la página. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// 
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    ///               
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    ///                          
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : Marzo/2012
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
                    Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                    //Configuracion_Acceso("Frm_Cat_Nom_Clave_Cargo_Abono.aspx");
                    break;
                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    break;
                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    break;
            }

            Txt_Clave_Tipos_Desc_Esp.Enabled = Habilitado;
            Txt_Descripcion.Enabled = Habilitado;
            Cmb_Clave_Cargo_Abono.Enabled = Habilitado;
            Grid_Tipo_Desc_Esp.Enabled = !Habilitado;
            Txt_Busqueda_Clave.Enabled = !Habilitado;
            Btn_Busqueda_Tipos_Desc_Esp.Enabled = !Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    /// ************************************************************************************************************************************************************
    /// Nombre: Mostrar_Mensaje
    /// 
    /// Descripción: Metodo que muestra el mensaje al usuario del sistema.
    /// 
    /// Parámetros: Estatus: Mostrar/Ocultar Mensaje.
    ///             Mensaje: Texto a mostrar al usuario.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    private void Mostrar_Mensaje(Boolean Estatus, String Mensaje)
    {
        try
        {
            Lbl_Mensaje_Error.Text = Mensaje;
            Lbl_Mensaje_Error.Visible = Estatus;
            Lbl_Mensaje_Error.Visible = Estatus;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generardo al mostrar el mensaje. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Consulta)
    /// ************************************************************************************************************************************************************
    /// Nombre: Consultar_Claves
    /// 
    /// Descripción: Metodo que consulta las claves de tipos de descuentos especificos.
    /// 
    /// Parámetros: No aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    private DataTable Consultar_Claves()
    {
        Cls_Cat_Nom_Tipos_Desc_Esp_Negocio Obj_Claves_Tipos_Desc_Esp = new Cls_Cat_Nom_Tipos_Desc_Esp_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Resultado = null;//Variable que almacenara un listado de claves decargo/abono.

        try
        {
            if (!String.IsNullOrEmpty(Txt_Busqueda_Clave.Text.Trim()))
                Obj_Claves_Tipos_Desc_Esp.P_Clave = Txt_Busqueda_Clave.Text.Trim();

            Dt_Resultado = Obj_Claves_Tipos_Desc_Esp.Consultar();
        }
        catch (Exception Ex)
        {

            Mostrar_Mensaje(true, "Error al consultar las claves de tipos de descuentos especificos. Error: [" + Ex.Message + "]");
        }
        return Dt_Resultado;
    }
    /// ************************************************************************************************************************************************************
    /// Nombre: Consultar_Claves_Cargo_Abono
    /// 
    /// Descripción: Metodo que consulta las claves de cargo/abono.
    /// 
    /// Parámetros: No aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    private void Consultar_Claves_Cargo_Abono()
    {
        Cls_Cat_Nom_Tipos_Desc_Esp_Negocio Obj_Tipos_Desc_Esp = new Cls_Cat_Nom_Tipos_Desc_Esp_Negocio();//Variable de conaxion con la capa de negocios.
        DataTable Dt_Resultado = null;

        try
        {
            Dt_Resultado = Obj_Tipos_Desc_Esp.Consultar_Clave_Cargo_Abono();
            Cmb_Clave_Cargo_Abono.DataSource = Dt_Resultado;
            Cmb_Clave_Cargo_Abono.DataTextField = "CLAVE_CARGO_ABONO";
            Cmb_Clave_Cargo_Abono.DataValueField = Cat_Nom_Claves_Cargo_Abono.Campo_Cargo_Abono_ID;
            Cmb_Clave_Cargo_Abono.DataBind();

            Cmb_Clave_Cargo_Abono.Items.Insert(0, new ListItem("<- Seleccione ->", String.Empty));
            Cmb_Clave_Cargo_Abono.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(true, "Error al consultar las claves de cargo/abono. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Operacion)
    /// ************************************************************************************************************************************************************
    /// Nombre: Alta
    /// 
    /// Descripción: Metodo que ejecuta el alta de una clave.
    /// 
    /// Parámetros:
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    private void Alta()
    {
        Cls_Cat_Nom_Tipos_Desc_Esp_Negocio Obj_Claves_Tipos_Desc_Esp = new Cls_Cat_Nom_Tipos_Desc_Esp_Negocio();//Variable de conexión con la capa de negocios.

        try
        {
            Obj_Claves_Tipos_Desc_Esp.P_Clave = Txt_Clave_Tipos_Desc_Esp.Text.Trim();
            Obj_Claves_Tipos_Desc_Esp.P_Descripcion = Txt_Descripcion.Text.Trim();
            Obj_Claves_Tipos_Desc_Esp.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            Obj_Claves_Tipos_Desc_Esp.P_Cargo_Abono_ID = Cmb_Clave_Cargo_Abono.SelectedValue.Trim();

            if (Obj_Claves_Tipos_Desc_Esp.Alta())
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Completa');", true);
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(true, "Error al ejecutar el alta de la clave. Error: [" + Ex.Message + "]");
        }
    }
    /// ************************************************************************************************************************************************************
    /// Nombre: Actualizar
    /// 
    /// Descripción: Metodo que ejecuta la actualización de una clave.
    /// 
    /// Parámetros:
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    private void Actualizar()
    {
        Cls_Cat_Nom_Tipos_Desc_Esp_Negocio Obj_Claves_Tipos_Desc_Esp = new Cls_Cat_Nom_Tipos_Desc_Esp_Negocio();//Variable de conexión con la capa de negocios.

        try
        {
            Obj_Claves_Tipos_Desc_Esp.P_Tipo_Desc_Esp_ID = HTxt_Tipo_Desc_Esp_ID.Value;
            Obj_Claves_Tipos_Desc_Esp.P_Clave = Txt_Clave_Tipos_Desc_Esp.Text.Trim();
            Obj_Claves_Tipos_Desc_Esp.P_Descripcion = Txt_Descripcion.Text.Trim();
            Obj_Claves_Tipos_Desc_Esp.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            Obj_Claves_Tipos_Desc_Esp.P_Cargo_Abono_ID = Cmb_Clave_Cargo_Abono.SelectedValue.Trim();

            if (Obj_Claves_Tipos_Desc_Esp.Actualizar())
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Completa');", true);
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(true, "Error al ejecutar el alta de la clave. Error: [" + Ex.Message + "]");
        }
    }
    /// ************************************************************************************************************************************************************
    /// Nombre: Delete
    /// 
    /// Descripción: Metodo que ejecuta la baja de una clave.
    /// 
    /// Parámetros:
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    private void Delete()
    {
        Cls_Cat_Nom_Tipos_Desc_Esp_Negocio Obj_Claves_Tipo_Desc_Esp = new Cls_Cat_Nom_Tipos_Desc_Esp_Negocio();

        try
        {
            Obj_Claves_Tipo_Desc_Esp.P_Tipo_Desc_Esp_ID = HTxt_Tipo_Desc_Esp_ID.Value;

            if (Obj_Claves_Tipo_Desc_Esp.Delete())
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Completa');", true);
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(true, "Error al eliminar una clave. Erro: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Validacion)
    /// ************************************************************************************************************************************************************
    /// Nombre: Validar
    /// 
    /// Descripción: Metodo que valida que se hallan ingresado los datos de forma correcta y completa.
    /// 
    /// Parámetros:
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    private Boolean Validar()
    {
        Boolean Estatus = true;
        StringBuilder Mensaje = new StringBuilder();

        try
        {
            Mensaje.Append("Es necesario ingresar: <br />");

            if (String.IsNullOrEmpty(Txt_Clave_Tipos_Desc_Esp.Text))
            {
                Mensaje.Append("&nbsp;&nbsp;&nbsp;&nbsp; + &nbsp;La clave es un dato obligatorio. <br />");
                Estatus = false;
            }

            if (String.IsNullOrEmpty(Txt_Descripcion.Text))
            {
                Mensaje.Append("&nbsp;&nbsp;&nbsp;&nbsp; + &nbsp;La descripción es un dato obligatorio. <br />");
                Estatus = false;
            }

            if (Cmb_Clave_Cargo_Abono.SelectedIndex <= 0) {
                Mensaje.Append("&nbsp;&nbsp;&nbsp;&nbsp; + &nbsp;La clave cargo/abono es un dato obligatorio. <br />");
                Estatus = false;
            }

            if (!Estatus)
                Mostrar_Mensaje(true, Mensaje.ToString());
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(true, "Error al validar los datos de la página. Error: [" + Ex.Message + "]");
        }
        return Estatus;
    }
    #endregion

    #endregion

    #region (Grids)
    /// ************************************************************************************************************************************************************
    /// Nombre: Cargar_Grid
    /// 
    /// Descripción: Método que carga el grid de claves.
    /// 
    /// Parámetros: No aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    private void Cargar_Grid(DataTable Dt_Resultado)
    {
        try
        {
            Grid_Tipo_Desc_Esp.Columns[1].Visible = true;
            Grid_Tipo_Desc_Esp.DataSource = Dt_Resultado;
            Grid_Tipo_Desc_Esp.DataBind();
            Grid_Tipo_Desc_Esp.SelectedIndex = -1;
            Grid_Tipo_Desc_Esp.Columns[1].Visible = false;
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(true, "Error al mostrar cargar el grid. Error: [" + Ex.Message + "]");
        }
    }
    /// ************************************************************************************************************************************************************
    /// Nombre: Grid_Tipo_Desc_Esp_PageIndexChanging
    /// 
    /// Descripción: evento que cambia la pagina de la tabla de claves.
    /// 
    /// Parámetros: No aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    protected void Grid_Tipo_Desc_Esp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Tipo_Desc_Esp.PageIndex = e.NewPageIndex;
            Configuracion_Inicial();
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(true, Ex.Message);
        }
    }
    /// ************************************************************************************************************************************************************
    /// Nombre: Grid_Tipo_Desc_Esp_SelectedIndexChanged
    /// 
    /// Descripción: Carga la información del elemento seleccionado.
    /// 
    /// Parámetros: No aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    protected void Grid_Tipo_Desc_Esp_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable Dt_Resultado = null;

        try
        {
            Dt_Resultado = Consultar_Claves();
            HTxt_Tipo_Desc_Esp_ID.Value = Grid_Tipo_Desc_Esp.SelectedRow.Cells[2].Text;

            var Clave_Tipo_Descuentos_Especificos = from clave in Dt_Resultado.AsEnumerable()
                                                    where clave.Field<String>(Cat_Nom_Tipos_Desc_Esp.Campo_Clave).ToString() == HTxt_Tipo_Desc_Esp_ID.Value
                                                    select new
                                                    {
                                                        ID = clave.Field<String>(Cat_Nom_Tipos_Desc_Esp.Campo_Tipo_Desc_Esp_ID),
                                                        Clave = clave.Field<String>(Cat_Nom_Tipos_Desc_Esp.Campo_Clave),
                                                        Descripcion = clave.Field<String>(Cat_Nom_Tipos_Desc_Esp.Campo_Descripcion), 
                                                        Clave_Cargo_Abono = clave.Field<String>("CLAVE_CARGO_ABONO")
                                                    };

            foreach (var objeto in Clave_Tipo_Descuentos_Especificos)
            {
                HTxt_Tipo_Desc_Esp_ID.Value = objeto.ID;
                Txt_Clave_Tipos_Desc_Esp.Text = objeto.Clave.ToString();
                Txt_Descripcion.Text = objeto.Descripcion;
                Cmb_Clave_Cargo_Abono.SelectedIndex = Cmb_Clave_Cargo_Abono.Items.IndexOf(
                    Cmb_Clave_Cargo_Abono.Items.FindByText(objeto.Clave_Cargo_Abono));
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(true, Ex.Message);
        }
    }
    #endregion

    #region (Eventos)
    /// ************************************************************************************************************************************************************
    /// Nombre: Btn_Nuevo_Click
    /// 
    /// Descripción: Alta de la clave
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.ToolTip.Equals("Nuevo"))
            {
                Limpiar_Controles();
                Habilitar_Controles("Nuevo");
            }
            else
            {

                if (Validar())
                {
                    //Invoke to method add
                    Alta();
                    Configuracion_Inicial();
                }
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(true, Ex.Message);
        }
    }
    /// ************************************************************************************************************************************************************
    /// Nombre: Btn_Modificar_Click
    /// 
    /// Descripción: Modificar la clave
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Modificar.ToolTip.Equals("Modificar"))
            {
                if (Grid_Tipo_Desc_Esp.SelectedIndex != (-1) && !String.IsNullOrEmpty(HTxt_Tipo_Desc_Esp_ID.Value))
                {
                    Habilitar_Controles("Modificar");
                }
                else
                {
                    Mostrar_Mensaje(true, "Seleccione el registro que desea modificar.");
                }
            }
            else
            {
                if (Validar())
                {
                    //Invoke to method modify
                    Actualizar();
                    Configuracion_Inicial();
                }
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(true, Ex.Message);
        }
    }
    /// ************************************************************************************************************************************************************
    /// Nombre: Btn_Eliminar_Click
    /// 
    /// Descripción: Eliminar la clave
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Eliminar.ToolTip.Equals("Eliminar"))
            {
                if ((Grid_Tipo_Desc_Esp.SelectedIndex != (-1)) &&
                    !String.IsNullOrEmpty(HTxt_Tipo_Desc_Esp_ID.Value))
                {
                    //Invoke to method delete
                    Delete();
                    Configuracion_Inicial();
                }
                else
                {
                    Mostrar_Mensaje(true, "Seleccione el registro que desea eliminar");
                }
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(true, Ex.Message);
        }
    }
    /// ************************************************************************************************************************************************************
    /// Nombre: Btn_Salir_Click
    /// 
    /// Descripción: Salir del sistema o cancelar la operacion.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
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
        catch (Exception Ex)
        {
            Mostrar_Mensaje(true, Ex.Message);
        }
    }
    /// ************************************************************************************************************************************************************
    /// Nombre: Btn_Busqueda_Tipos_Desc_Esp_Click
    /// 
    /// Descripción: Busca la clave en el sistema.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    protected void Btn_Busqueda_Tipos_Desc_Esp_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Cargar_Grid(Consultar_Claves());
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(true, Ex.Message);
        }
    }
    #endregion
}
