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
using Presidencia.Claves_Cargo_Abono.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Text;

public partial class paginas_Nomina_Frm_Cat_Nom_Clave_Cargo_Abono : System.Web.UI.Page
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
            Cargar_Grid(Consultar_Claves());//Cargamos el grid de claves cargo/abono.
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
            Txt_Clave_Cargo_Abono.Text = String.Empty;
            Txt_Descripcion.Text = String.Empty;
            HTxt_Clave_Cargo_Abono_ID.Value = String.Empty;
            Grid_Claves_Cargo_Abono.SelectedIndex = (-1);
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

            Txt_Clave_Cargo_Abono.Enabled = Habilitado;
            Txt_Descripcion.Enabled = Habilitado;
            Grid_Claves_Cargo_Abono.Enabled = !Habilitado;
            Txt_Busqueda_Clave.Enabled = !Habilitado;
            Btn_Busqueda_Claves_Cargo_Abono.Enabled = !Habilitado;
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
    /// Descripción: Metodo que consulta las claves de cargo/abono.
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
        Cls_Cat_Nom_Clave_Cargo_Abono_Negocio Obj_Claves_Cargo_Abono = new Cls_Cat_Nom_Clave_Cargo_Abono_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Resultado = null;//Variable que almacenara un listado de claves decargo/abono.

        try
        {
            if (!String.IsNullOrEmpty(Txt_Busqueda_Clave.Text.Trim()))
                Obj_Claves_Cargo_Abono.P_Clave = Txt_Busqueda_Clave.Text.Trim();

            Dt_Resultado = Obj_Claves_Cargo_Abono.Consultar_Clave_Cargo_Abono();
        }
        catch (Exception Ex)
        {

            Mostrar_Mensaje(true, "Error al consultar las claves de cargo/abono. Error: [" + Ex.Message + "]");
        }
        return Dt_Resultado;
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
        Cls_Cat_Nom_Clave_Cargo_Abono_Negocio Obj_Claves_Cargo_Abono = new Cls_Cat_Nom_Clave_Cargo_Abono_Negocio();//Variable de conexión con la capa de negocios.

        try
        {
            Obj_Claves_Cargo_Abono.P_Clave = Txt_Clave_Cargo_Abono.Text.Trim();
            Obj_Claves_Cargo_Abono.P_Descripcion = Txt_Descripcion.Text.Trim();
            Obj_Claves_Cargo_Abono.P_Usuario = Cls_Sessiones.No_Empleado;

            if (Obj_Claves_Cargo_Abono.Alta())
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
    private void Actualizar() {
        Cls_Cat_Nom_Clave_Cargo_Abono_Negocio Obj_Claves_Cargo_Abono = new Cls_Cat_Nom_Clave_Cargo_Abono_Negocio();//Variable de conexión con la capa de negocios.

        try
        {
            Obj_Claves_Cargo_Abono.P_Cargo_Abono_ID = HTxt_Clave_Cargo_Abono_ID.Value;
            Obj_Claves_Cargo_Abono.P_Clave = Txt_Clave_Cargo_Abono.Text.Trim();
            Obj_Claves_Cargo_Abono.P_Descripcion = Txt_Descripcion.Text.Trim();
            Obj_Claves_Cargo_Abono.P_Usuario = Cls_Sessiones.No_Empleado;

            if (Obj_Claves_Cargo_Abono.Actualizar())
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
        Cls_Cat_Nom_Clave_Cargo_Abono_Negocio Obj_Claves_Cargo_Abono = new Cls_Cat_Nom_Clave_Cargo_Abono_Negocio();

        try
        {
            Obj_Claves_Cargo_Abono.P_Cargo_Abono_ID = HTxt_Clave_Cargo_Abono_ID.Value;

            if (Obj_Claves_Cargo_Abono.Delete()) {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Completa');", true);
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(true, "Error al eliminar una clave de cargo/abono. Erro: [" + Ex.Message + "]");
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

            if (String.IsNullOrEmpty(Txt_Clave_Cargo_Abono.Text)) {
                Mensaje.Append("&nbsp;&nbsp;&nbsp;&nbsp; + &nbsp;La clave es un dato obligatorio. <br />");
                Estatus = false;
            }

            if (String.IsNullOrEmpty(Txt_Descripcion.Text))
            {
                Mensaje.Append("&nbsp;&nbsp;&nbsp;&nbsp; + &nbsp;La descripción es un dato obligatorio. <br />");
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
    /// Descripción: Método que carga el grid de claves de cargo/abono.
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
            Grid_Claves_Cargo_Abono.Columns[1].Visible = true;
            Grid_Claves_Cargo_Abono.DataSource = Dt_Resultado;
            Grid_Claves_Cargo_Abono.DataBind();
            Grid_Claves_Cargo_Abono.SelectedIndex = -1;
            Grid_Claves_Cargo_Abono.Columns[1].Visible = false;
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(true, "Error al mostrar cargar el grid. Error: [" + Ex.Message + "]");
        }
    }
    /// ************************************************************************************************************************************************************
    /// Nombre: Grid_Claves_Cargo_Abono_PageIndexChanging
    /// 
    /// Descripción: evento que cambia la pagina de la tabla de claves de cargo abono.
    /// 
    /// Parámetros: No aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    protected void Grid_Claves_Cargo_Abono_PageIndexChanging(object sender, GridViewPageEventArgs e) {
        try
        {            
            Grid_Claves_Cargo_Abono.PageIndex = e.NewPageIndex;
            Configuracion_Inicial();                 
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(true, Ex.Message);
        }
    }
    /// ************************************************************************************************************************************************************
    /// Nombre: Grid_Claves_Cargo_Abono_SelectedIndexChanged
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
    protected void Grid_Claves_Cargo_Abono_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable Dt_Resultado = null;

        try
        {
            Dt_Resultado = Consultar_Claves();
            HTxt_Clave_Cargo_Abono_ID.Value = Grid_Claves_Cargo_Abono.SelectedRow.Cells[2].Text;

            var Clave_Cargo_Abono = from clave in Dt_Resultado.AsEnumerable()
                                    where clave.Field<Decimal>(Cat_Nom_Claves_Cargo_Abono.Campo_Clave).ToString() == Grid_Claves_Cargo_Abono.SelectedRow.Cells[2].Text
                                    select new
                                    {
                                        ID = clave.Field<String>(Cat_Nom_Claves_Cargo_Abono.Campo_Cargo_Abono_ID),
                                        Clave = clave.Field<Decimal>(Cat_Nom_Claves_Cargo_Abono.Campo_Clave),
                                        Descripcion = clave.Field<String>(Cat_Nom_Claves_Cargo_Abono.Campo_Descripcion)
                                    };

            foreach (var objeto in Clave_Cargo_Abono)
            {
                HTxt_Clave_Cargo_Abono_ID.Value = objeto.ID;
                Txt_Clave_Cargo_Abono.Text = objeto.Clave.ToString();
                Txt_Descripcion.Text = objeto.Descripcion;
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
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e) {
        try
        {
            if (Btn_Nuevo.ToolTip.Equals("Nuevo"))
            {
                Limpiar_Controles();
                Habilitar_Controles("Nuevo");
            }
            else {

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
                if (Grid_Claves_Cargo_Abono.SelectedIndex != (-1) && !String.IsNullOrEmpty(HTxt_Clave_Cargo_Abono_ID.Value))
                {
                    Habilitar_Controles("Modificar");
                }
                else {
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
                if ((Grid_Claves_Cargo_Abono.SelectedIndex != (-1)) &&
                    !String.IsNullOrEmpty(HTxt_Clave_Cargo_Abono_ID.Value))
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
    /// Nombre: Btn_Busqueda_Claves_Cargo_Abono_Click
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
    protected void Btn_Busqueda_Claves_Cargo_Abono_Click(object sender, ImageClickEventArgs e) {
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
