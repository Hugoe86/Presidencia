using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Dependencias.Negocios;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Area_Funcional.Negocio;
using Presidencia.Catalogo_SAP_Fuente_Financiamiento.Negocio;
using Presidencia.Catalogo_Compras_Proyectos_Programas.Negocio;
using System.Collections.Generic;
using Presidencia.Puestos.Negocios;
using Presidencia.Grupos_Dependencias.Negocio;
using Presidencia.Informacion_Presupuestal;
using Presidencia.Parametros_Contables.Negocio;
using Presidencia.Ayudante_Informacion;
using Presidencia.Cat_Parametros_Nomina.Negocio;

public partial class paginas_Nomina_Frm_Cat_Dependencias : System.Web.UI.Page
{
    #region (Load/Init)
    ///************************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Page_Load
    /// 
    /// DESCRIPCION : Carga la configuración inicial de los controles de la página.
    /// 
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 08/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///************************************************************************************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                ViewState["SortDirection"] = "ASC";
            }

            Lbl_Mensaje_Error.Text = String.Empty;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///************************************************************************************
    /// Nombre Método: Page_LoadComplete
    /// 
    /// Descripción: Método que se ejecuta una vez cargada la página.
    /// 
    /// Parámetros: No Aplica.
    ///
    /// Usuario creó: Juan Alberto Hernández Negrete
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa modificación:
    ///************************************************************************************
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        try
        {
            //Validamos que exista seleccionada una unidad responsable.
            if (Grid_Dependencias.SelectedIndex != -1)
            {
                Validacion_Presupuestal(); //Mostramos la informacion de la validación presupuestal.
                Validacion_Presupuestal_PSM();
            }
            else
            {
                Ltr_Inf_Presupuestal_Sueldos.Text = String.Empty;
                    //Limpiamos la información cargada al realizar la validación presupuestal.
                Ltr_Inf_Presupuestal_PSM.Text = String.Empty;
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Text = ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }

    #endregion

    #region (Metodos)

    #region (Métodos Generales)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 8/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            Limpia_Controles();             //Limpia los controles del forma
            Consulta_Dependencias();        //Consulta todas las dependencias que fueron dadas de alta en la BD
            Consulta_Areas_Funcionales();   //Consulta las áreas funcionales registradas en el sistema.
            Consulta_Fte_Financiamiento();  //Consulta las fuentes de financiamiento registradas en el sistema.
            Consulta_Programas();           //Consulta los programas registrados en el sistema.
            Consultar_Puestos();
            Consulta_Grupos_Dependencia();  //consulta los grupos dependencia
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los controles que se encuentran en la forma
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 8/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {
            Txt_Dependencia_ID.Text = "";
            Txt_Clave_Dependecia.Text = "";
            Txt_Nombre_Dependencia.Text = "";
            Txt_Comentarios_Dependencia.Text = "";
            Txt_Busqueda_Dependencia.Text = "";
            Cmb_Estatus_Dependencia.SelectedIndex = -1;
            Cmb_Area_Funcional.SelectedIndex = -1;
            Cmb_Grupo_Dependencia.SelectedIndex = -1;

            Cmb_Fuente_Financiamiento.SelectedIndex = -1;
            Grid_Fuentes_Financiamiento.SelectedIndex = -1;
            Grid_Fuentes_Financiamiento.DataSource = new DataTable();
            Grid_Fuentes_Financiamiento.DataBind();

            Cmb_Programa.SelectedIndex = -1;
            Grid_Programas.SelectedIndex = -1;
            Grid_Programas.DataSource = new DataTable();
            Grid_Programas.DataBind();

            Cmb_Puestos.SelectedIndex = -1;
            Grid_Puestos.SelectedIndex = -1;
            Grid_Puestos.DataSource = new DataTable();
            Grid_Puestos.DataBind();

            Grid_Dependencias.SelectedIndex = -1;

            Cmb_Tipo_Plaza.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception("Limpia_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///                para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                           si es una alta, modificacion
    ///                           
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 8/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado; ///Indica si el control de la forma va hacer habilitado para utilización del usuario
        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Cmb_Estatus_Dependencia.SelectedIndex = 0;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Cmb_Estatus_Dependencia.Enabled = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                    Configuracion_Acceso("Frm_Cat_Dependencias.aspx");
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Cmb_Estatus_Dependencia.SelectedIndex = 0;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Cmb_Estatus_Dependencia.Enabled = false;
                    Cmb_Estatus_Dependencia.SelectedIndex = 1;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";

                    if (Session["Dt_Fte_Financiamiento"] != null) Session.Remove("Dt_Fte_Financiamiento");
                    if (Session["Dt_Programas"] != null) Session.Remove("Dt_Programas");
                    if (Session["PUESTOS_UNIDAD_RESPNSABLE"] != null) Session.Remove("PUESTOS_UNIDAD_RESPNSABLE");
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
                    Cmb_Estatus_Dependencia.Enabled = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    break;
            }
            Txt_Nombre_Dependencia.Enabled = Habilitado;
            Txt_Comentarios_Dependencia.Enabled = Habilitado;
            Cmb_Grupo_Dependencia.Enabled = Habilitado;
            Txt_Busqueda_Dependencia.Enabled = !Habilitado;
            Btn_Buscar_Dependencia.Enabled = !Habilitado;
            Grid_Dependencias.Enabled = !Habilitado;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            Cmb_Area_Funcional.Enabled = Habilitado;
            Txt_Clave_Dependecia.Enabled = Habilitado;

            Cmb_Fuente_Financiamiento.Enabled = Habilitado;
            Btn_Agregar_Fte_Financiamiento.Enabled = Habilitado;
            Grid_Fuentes_Financiamiento.Enabled = Habilitado;

            Cmb_Programa.Enabled = Habilitado;
            Btn_Agregar_Programa.Enabled = Habilitado;
            Grid_Programas.Enabled = Habilitado;

            Cmb_Puestos.Enabled = Habilitado;
            Btn_Agregar_Puestos.Enabled = Habilitado;
            Grid_Puestos.Enabled = Habilitado;

            Cmb_Tipo_Plaza.Enabled = Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    /// *******************************************************************************************************
    /// Nombre: Consultar_Consecutivo
    /// 
    /// Descripción: Metodo que devuelve la plaza consecutiva.
    /// 
    /// Parámetros: Dt_Puestos.- El listado de puestos del cual se obtendra su consecutivo.
    /// 
    /// Usuario Creó: Juan alberto Hernández Negrete. 
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *******************************************************************************************************
    protected String Consultar_Consecutivo(DataTable Dt_Puestos)
    {
        Int32 Consecutivo = 0;//Variable que almacena la clave consecutiva.
        String Clave = String.Empty;//Variable que almacena la clave consecutiva como un cadena.

        try
        {
            if (Dt_Puestos is DataTable)
            {
                if (Dt_Puestos.Rows.Count > 0)
                {
                    foreach (DataRow PUESTO in Dt_Puestos.Rows)
                    {
                        if (PUESTO is DataRow)
                        {
                            if (!String.IsNullOrEmpty(PUESTO[Cat_Nom_Dep_Puestos_Det.Campo_Clave].ToString().Trim()))
                            {
                                if (Consecutivo <= (Convert.ToInt32(PUESTO[Cat_Nom_Dep_Puestos_Det.Campo_Clave].ToString().Trim())))
                                {
                                    Consecutivo = (Convert.ToInt32(PUESTO[Cat_Nom_Dep_Puestos_Det.Campo_Clave].ToString().Trim()) + 1);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Consecutivo = 1;
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al obtener la clave consecutiva. Error: [" + Ex.Message + "]");
        }
        return Consecutivo.ToString().Trim();
    }
    #endregion

    #region (Métodos Operación)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Dependencia
    /// DESCRIPCION : Da de Alta la Dependencia con los datos proporcionados por el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 23-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Dependencia()
    {
        Cls_Cat_Dependencias_Negocio Obj_Cat_Dependencias = new Cls_Cat_Dependencias_Negocio();  //Variable de conexión hacia la capa de Negocios

        try
        {
            Obj_Cat_Dependencias.P_Nombre = Txt_Nombre_Dependencia.Text;
            Obj_Cat_Dependencias.P_Estatus = Cmb_Estatus_Dependencia.SelectedValue;
            Obj_Cat_Dependencias.P_Comentarios = Txt_Comentarios_Dependencia.Text;
            Obj_Cat_Dependencias.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            Obj_Cat_Dependencias.P_Area_Funcional_ID = Cmb_Area_Funcional.SelectedValue.Trim();
            Obj_Cat_Dependencias.P_Clave = Txt_Clave_Dependecia.Text.Trim();
            Obj_Cat_Dependencias.P_Grupo_Dependencia_ID = Cmb_Grupo_Dependencia.SelectedValue;

            if (Session["Dt_Fte_Financiamiento"] != null)
            {
                Obj_Cat_Dependencias.P_Dt_Fuentes_Financiamiento = (DataTable)Session["Dt_Fte_Financiamiento"];
            }
            if (Session["Dt_Fte_Financiamiento"] != null)
            {
                Session.Remove("Dt_Fte_Financiamiento");
            }

            if (Session["Dt_Programas"] != null)
            {
                Obj_Cat_Dependencias.P_Dt_Programas = (DataTable)Session["Dt_Programas"];
            }
            if (Session["Dt_Programas"] != null)
            {
                Session.Remove("Dt_Programas");
            }

            if (Session["PUESTOS_UNIDAD_RESPNSABLE"] != null)
            {
                Obj_Cat_Dependencias.P_Dt_Puestos = (DataTable)Session["PUESTOS_UNIDAD_RESPNSABLE"];
            }
            if (Session["PUESTOS_UNIDAD_RESPNSABLE"] != null)
            {
                Session.Remove("PUESTOS_UNIDAD_RESPNSABLE");
            }
           

            Obj_Cat_Dependencias.Alta_Dependencia(); //Da de alto los datos de la dependencia en la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            Habilitar_Controles("Inicial");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Dependencias", "alert('El Alta de Dependencia fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Dependencia " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Dependencia
    /// DESCRIPCION : Modifica los datos de la dependencia con los proporcionados por el usuario
    ///               en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 23-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Dependencia()
    {
        Cls_Cat_Dependencias_Negocio Obj_Cat_Dependecias = new Cls_Cat_Dependencias_Negocio();  //Variable de conexión hacia la capa de Negocios

        try
        {
            Obj_Cat_Dependecias.P_Dependencia_ID = Txt_Dependencia_ID.Text;
            Obj_Cat_Dependecias.P_Nombre = Txt_Nombre_Dependencia.Text;
            Obj_Cat_Dependecias.P_Estatus = Cmb_Estatus_Dependencia.SelectedValue;
            Obj_Cat_Dependecias.P_Comentarios = Txt_Comentarios_Dependencia.Text;
            Obj_Cat_Dependecias.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            Obj_Cat_Dependecias.P_Area_Funcional_ID = Cmb_Area_Funcional.SelectedValue.Trim();
            Obj_Cat_Dependecias.P_Clave = Txt_Clave_Dependecia.Text.Trim();
            Obj_Cat_Dependecias.P_Grupo_Dependencia_ID = Cmb_Grupo_Dependencia.SelectedValue;

            if (Session["Dt_Fte_Financiamiento"] != null)
            {
                Obj_Cat_Dependecias.P_Dt_Fuentes_Financiamiento = (DataTable)Session["Dt_Fte_Financiamiento"];
            }
            if (Session["Dt_Fte_Financiamiento"] != null)
            {
                Session.Remove("Dt_Fte_Financiamiento");
            }

            if (Session["Dt_Programas"] != null)
            {
                Obj_Cat_Dependecias.P_Dt_Programas = (DataTable)Session["Dt_Programas"];
            }
            if (Session["Dt_Programas"] != null)
            {
                Session.Remove("Dt_Programas");
            }

            if (Session["PUESTOS_UNIDAD_RESPNSABLE"] != null)
            {
                Obj_Cat_Dependecias.P_Dt_Puestos = (DataTable)Session["PUESTOS_UNIDAD_RESPNSABLE"];
            }
            if (Session["PUESTOS_UNIDAD_RESPNSABLE"] != null)
            {
                Session.Remove("PUESTOS_UNIDAD_RESPNSABLE");
            }

            Obj_Cat_Dependecias.Modificar_Dependencia(); //Sustituye los datos de la dependencia que se encuentran en la BD por los que fueron proporcionados por el usuario
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Dependencias", "alert('La Modificación de la Dependencia fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Dependencia " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Dependencia
    /// DESCRIPCION : Elimina los datos de la dependencia que fue seleccionada por el Usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 23-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Dependencia()
    {
        Cls_Cat_Dependencias_Negocio Rs_Eliminar_Cat_Dependencias = new Cls_Cat_Dependencias_Negocio();  //Variable de conexión hacia la capa de Negocios
        try
        {
            Rs_Eliminar_Cat_Dependencias.P_Dependencia_ID = Txt_Dependencia_ID.Text;
            Rs_Eliminar_Cat_Dependencias.Elimina_Dependencia(); //Elimina la dependencia seleccionada por el usuario de la BD

            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Dependencias", "alert('La Eliminación de la Dependencia fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Eliminar_Dependencia " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region (Metodos Validacion)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Dependencias
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 9/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Dependencia()
    {
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (string.IsNullOrEmpty(Txt_Nombre_Dependencia.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Nombre de la dependencia es un dato requerido por el sistema. <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Clave_Dependecia.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Clave es un dato requerido por el sistema. La clave deberá de ser de 5 carácteres alfanumericos. <br>";
            Datos_Validos = false;
        }

        if (Cmb_Estatus_Dependencia.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Estatus es un dato requerido por el sistema. <br>";
            Datos_Validos = false;
        }

        if (Cmb_Area_Funcional.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Área Funcional es un dato requerido por el sistema. <br>";
            Datos_Validos = false;
        }
        if (Cmb_Grupo_Dependencia.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La depenendica es un dato requerido por el sistema. <br>";
            Datos_Validos = false;
        }

        if (Grid_Fuentes_Financiamiento.Rows.Count <= 0) {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + No se ha agregado ninguna Fuente de Financiamiento a la Dependencia. <br>";
            Datos_Validos = false;
        }

        if (Grid_Programas.Rows.Count <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + No se ha agregado ningún Programa a la Dependencia. <br>";
            Datos_Validos = false;
        }
        return Datos_Validos;
    }
    #endregion

    #region (Métodos Consulta)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Dependencias
    /// DESCRIPCION : Consulta las dependencias que estan dadas de alta en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 24-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Dependencias()
    {
        Cls_Cat_Dependencias_Negocio Rs_Consulta_Cat_Dependencias = new Cls_Cat_Dependencias_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Dependencias; //Variable que obtendra los datos de la consulta 

        try
        {
            if (Txt_Busqueda_Dependencia.Text != "")
            {
                Rs_Consulta_Cat_Dependencias.P_Nombre = Txt_Busqueda_Dependencia.Text;
            }
            Dt_Dependencias = Rs_Consulta_Cat_Dependencias.Consulta_Dependencias(); //Consulta los datos generales de las dependencias dadas de alta en la BD
            Session["Consulta_Dependencias"] = Dt_Dependencias;
            Llena_Grid_Dependencias(); //Agrega las dependencias obtenidas de la consulta anterior
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Dependencias " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Areas_Funcionales
    /// DESCRIPCION : Consulta las áreas funcionales que estan dadas de alta en la BD
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 8/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Areas_Funcionales()
    {
        Cls_Cat_Dependencias_Negocio Obj_Dependencias = new Cls_Cat_Dependencias_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Areas_Funcionales = null;                                             //Variable que almacena un listado de areas funcionales registradas actualmente en el sistema.

        try
        {
            Dt_Areas_Funcionales = Obj_Dependencias.Consulta_Area_Funcional();
            Cmb_Area_Funcional.DataSource = Dt_Areas_Funcionales;
            Cmb_Area_Funcional.DataTextField = "CLAVE_NOMBRE";
            Cmb_Area_Funcional.DataValueField = Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID;
            Cmb_Area_Funcional.DataBind();

            Cmb_Area_Funcional.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
            Cmb_Area_Funcional.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las áreas funcionales registradas actualmente en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Grupos_Dependencia
    /// DESCRIPCION : Consulta las áreas funcionales que estan dadas de alta en la BD
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 01/Diciembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Grupos_Dependencia()
    {
        Cls_Cat_Grupos_Dependencias_Negocio Obj_Dependencias = new Cls_Cat_Grupos_Dependencias_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Grupos_Dependencia = null;                                             //Variable que almacena un listado de areas funcionales registradas actualmente en el sistema.

        try
        {
            Dt_Grupos_Dependencia = Obj_Dependencias.Consultar_Grupos_Dependencias();
            Cmb_Grupo_Dependencia.DataSource = Dt_Grupos_Dependencia;
            Cmb_Grupo_Dependencia.DataTextField = "Clave_Nombre";
            Cmb_Grupo_Dependencia.DataValueField = Cat_Grupos_Dependencias.Campo_Grupo_Dependencia_ID;
            Cmb_Grupo_Dependencia.DataBind();
            Cmb_Grupo_Dependencia.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
            Cmb_Grupo_Dependencia.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las áreas funcionales registradas actualmente en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Fte_Financiamiento
    /// DESCRIPCION : Consulta las fuentes de financiamiento registradas en el sistema.
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 8/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Fte_Financiamiento()
    {
        Cls_Cat_SAP_Fuente_Financiamiento_Negocio Obj_Fte_Financiamiento = new Cls_Cat_SAP_Fuente_Financiamiento_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Fte_Financiamiento = null;//Variable que almacenara los resultados de la busqueda realizada.

        try
        {
            Dt_Fte_Financiamiento = Obj_Fte_Financiamiento.Consulta_Datos_Fuente_Financiamiento();
            Cmb_Fuente_Financiamiento.DataSource = Dt_Fte_Financiamiento;
            Cmb_Fuente_Financiamiento.DataTextField = Cat_SAP_Fuente_Financiamiento.Campo_Descripcion;
            Cmb_Fuente_Financiamiento.DataValueField = Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID;
            Cmb_Fuente_Financiamiento.DataBind();

            Cmb_Fuente_Financiamiento.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
            Cmb_Fuente_Financiamiento.SelectedIndex = -1;

        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las fuentes de financiamento registradas actualmente en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Areas_Funcionales
    /// DESCRIPCION : Consulta los programas registrados en el sistema.
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 8/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Programas()
    {
        Cls_Cat_Com_Proyectos_Programas_Negocio Obj_Programas = new Cls_Cat_Com_Proyectos_Programas_Negocio();//Variable de conexion con la capa de negocios
        DataTable Dt_Programas = null;//Variable que alamacenara los resultados obtenidos de la busqueda realizada.

        try
        {
            Dt_Programas = Obj_Programas.Consulta_Programas_Proyectos();
            Cmb_Programa.DataSource = Dt_Programas;
            Cmb_Programa.DataTextField = Cat_Com_Proyectos_Programas.Campo_Nombre;
            Cmb_Programa.DataValueField = Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID;
            Cmb_Programa.DataBind();

            Cmb_Programa.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
            Cmb_Programa.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los programas registrados actualmente en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    ///*************************************************************************************
    /// NOMBRE DE LA FUNCION: Es_Clave_Repetida
    /// DESCRIPCION : Consulta las dependencias por clave y si encuentra alguna dependecia
    ///               con esta clave valida que no se trate de una modificacion si es asi
    ///               se permitira que la dependencia mantega la misma clave al modificarse.
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 8/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*************************************************************************************
    private Boolean Es_Clave_Repetida(String Clave_Ingresada)
    {
        Cls_Cat_Dependencias_Negocio Obj_Dependencias = new Cls_Cat_Dependencias_Negocio();//Variable de conexion a la capa de negocios.
        DataTable Dt_Dependencias = null;   //Variable que almacenara la lista de resultados obtenidos de la búsqueda realizada.
        String Dependecia_ID = "";          //Identificador unico de la dependencia para uso interno del sistema.
        Boolean Permitir = true;

        try
        {
            if (Clave_Ingresada.Trim().Length == 5)
            {
                Obj_Dependencias.P_Clave = Clave_Ingresada;
                Dt_Dependencias = Obj_Dependencias.Consulta_Dependencias();

                if (Dt_Dependencias is DataTable)
                {
                    if (Dt_Dependencias.Rows.Count == 1)
                    {
                        foreach (DataRow Dependencia in Dt_Dependencias.Rows)
                        {
                            if (Dependencia is DataRow)
                            {
                                if (!string.IsNullOrEmpty(Dependencia[Cat_Dependencias.Campo_Dependencia_ID].ToString()))
                                {
                                    Dependecia_ID = Dependencia[Cat_Dependencias.Campo_Dependencia_ID].ToString();

                                    if (Dependecia_ID.Trim().Equals(Txt_Dependencia_ID.Text.Trim()))
                                    {
                                        Permitir = true;
                                    }
                                    else
                                    {
                                        Permitir = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else {
                Permitir = false;
            }
        }   
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar si la nueva clave ingresada ya le pertence alguna dependencia actualmente. Error: [" + Ex.Message + "]");
        }
        return Permitir;
    }
    #endregion

    #region (Métodos Grids)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llena_Grid_Dependencias
    /// DESCRIPCION : Llena el grid con las dependencias que se encuentran en la base de datos
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 23-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llena_Grid_Dependencias()
    {
        DataTable Dt_Dependencias; //Variable que obtendra los datos de la consulta 
        try
        {
            Grid_Dependencias.DataBind();
            Dt_Dependencias = (DataTable)Session["Consulta_Dependencias"];
            Grid_Dependencias.DataSource = Dt_Dependencias;
            Grid_Dependencias.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Dependencias " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region (Control Acceso Pagina)
    /// *****************************************************************************************************************************
    /// NOMBRE: Configuracion_Acceso
    /// 
    /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
    /// 
    /// PARÁMETROS: No Áplica.
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 23/Mayo/2011 10:43 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************************************************
    protected void Configuracion_Acceso(String URL_Pagina)
    {
        List<ImageButton> Botones = new List<ImageButton>();//Variable que almacenara una lista de los botones de la página.
        DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

        try
        {
            //Agregamos los botones a la lista de botones de la página.
            Botones.Add(Btn_Nuevo);
            Botones.Add(Btn_Modificar);
            Botones.Add(Btn_Eliminar);
            Botones.Add(Btn_Buscar_Dependencia);

            if (!String.IsNullOrEmpty(Request.QueryString["PAGINA"]))
            {
                if (Es_Numero(Request.QueryString["PAGINA"].Trim()))
                {
                    //Consultamos el menu de la página.
                    Dr_Menus = Cls_Sessiones.Menu_Control_Acceso.Select("MENU_ID=" + Request.QueryString["PAGINA"]);

                    if (Dr_Menus.Length > 0)
                    {
                        //Validamos que el menu consultado corresponda a la página a validar.
                        if (Dr_Menus[0][Apl_Cat_Menus.Campo_URL_Link].ToString().Contains(URL_Pagina))
                        {
                            Cls_Util.Configuracion_Acceso_Sistema_SIAS(Botones, Dr_Menus[0]);//Habilitamos la configuracón de los botones.
                        }
                        else
                        {
                            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    }
                }
                else
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
            }
            else
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al habilitar la configuración de accesos a la página. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Es_Numero(String Cadena)
    {
        Boolean Resultado = true;
        Char[] Array = Cadena.ToCharArray();
        try
        {
            for (int index = 0; index < Array.Length; index++)
            {
                if (!Char.IsDigit(Array[index])) return false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Validar si es un dato numerico. Error [" + Ex.Message + "]");
        }
        return Resultado;
    }
    #endregion

    #region (Puestos)
    /// *******************************************************************************************************
    /// Nombre: Consultar_Puestos
    /// 
    /// Descripción: Consulta los puestos que actualmente se encuentran en el sistema.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan alberto Hernández Negrete. 
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *******************************************************************************************************
    protected void Consultar_Puestos() {
        Cls_Cat_Puestos_Negocio Obj_Puestos = new Cls_Cat_Puestos_Negocio();//Variable de conexion a al capa de negocios.
        DataTable Dt_Puestos = null;//Variable que almacenara una lista de puestos.

        try
        {
            Dt_Puestos = Obj_Puestos.Consultar_Puestos();
            Cmb_Puestos.DataSource = Dt_Puestos;
            Cmb_Puestos.DataTextField = Cat_Puestos.Campo_Nombre;
            Cmb_Puestos.DataValueField = Cat_Puestos.Campo_Puesto_ID;
            Cmb_Puestos.DataBind();
            Cmb_Puestos.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
            Cmb_Puestos.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los puestos registrados en sistema. Error: [" + Ex.Message + "]");
        }
    }
    /// *******************************************************************************************************
    /// Nombre: Consultar_Puestos_Unidad_Responsable
    /// 
    /// Descripción: Consultamos las plazas que actualmente tiene asignadas la unidad responsable.
    /// 
    /// Parámetros: Unidad_Responsable_ID.- Identificador de la unidad responsable a la cuál se le agregara 
    ///             ela plaza.
    /// 
    /// Usuario Creó: Juan alberto Hernández Negrete. 
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *******************************************************************************************************
    protected void Consultar_Puestos_Unidad_Responsable(String Unidad_Responsable_ID)
    {
        Cls_Cat_Puestos_Negocio Obj_Puestos = new Cls_Cat_Puestos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Puestos_Unidad_Rersponsable = null;//Variable que lista las plazas de la unidad responsable.

        try
        {
            Obj_Puestos.P_Dependencia_ID = Unidad_Responsable_ID;
            Dt_Puestos_Unidad_Rersponsable = Obj_Puestos.Consultar_Puestos_UR();
            Session["PUESTOS_UNIDAD_RESPNSABLE"] = Dt_Puestos_Unidad_Rersponsable;
            Grid_Puestos.DataSource = (DataTable)Session["PUESTOS_UNIDAD_RESPNSABLE"];
            Grid_Puestos.DataBind();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los puestos de la unidad responsable registrados en sistema. Error: [" + Ex.Message + "]");
        }
    }
    /// *******************************************************************************************************
    /// Nombre: Agregar_Puesto_Unidad_Responsable
    /// 
    /// Descripción: Método que dependiendo el caso crea la estructura del datatable que almacena las plazas
    ///              o bien utiliza la tabla ya en memoria para invocar el método que agregara la nueva plaza 
    ///              a la lista de plazas vacantes de la unidad responsable.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan alberto Hernández Negrete. 
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *******************************************************************************************************
    protected void Agregar_Puesto_Unidad_Responsable()
    {
        ListItem Puesto_Seleccionado = null;//Variable que almacena el identificador de la plaza agregar al listado de plazas de la unidad responsable.

        try
        {
            Puesto_Seleccionado = Cmb_Puestos.SelectedItem;

            if (Session["PUESTOS_UNIDAD_RESPNSABLE"] != null)
            {
                Agregar_Puesto((DataTable)Session["PUESTOS_UNIDAD_RESPNSABLE"], Puesto_Seleccionado);
            }
            else
            {
                DataTable Dt_Puestos = new DataTable();
                Dt_Puestos.Columns.Add(Cat_Puestos.Campo_Puesto_ID, typeof(String));
                Dt_Puestos.Columns.Add(Cat_Puestos.Campo_Nombre, typeof(String));
                Dt_Puestos.Columns.Add(Cat_Puestos.Campo_Salario_Mensual, typeof(String));
                Dt_Puestos.Columns.Add("ESTATUS_PUESTO", typeof(String));
                Dt_Puestos.Columns.Add(Cat_Nom_Dep_Puestos_Det.Campo_Clave, typeof(String));
                Dt_Puestos.Columns.Add(Cat_Nom_Dep_Puestos_Det.Campo_Tipo_Plaza, typeof(String));

                Agregar_Puesto(Dt_Puestos, Puesto_Seleccionado);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al agregar un puesto a la unidad responsable. Error: [" + Ex.Message + "]");
        }
    }
    /// *******************************************************************************************************
    /// Nombre: Agregar_Puesto
    /// 
    /// Descripción: Método que agrega la nueva plaza a la tabla de plazas de la unidad responsable a modificar.
    /// 
    /// Parámetros: Dt_Puestos.- Lista de plazas que actualmente tiene la unidad responsable asignadas.
    ///             Puesto_Agregar.- Identificador de la nueva plaza agregar a la unidad responsable.
    /// 
    /// Usuario Creó: Juan alberto Hernández Negrete. 
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *******************************************************************************************************
    protected void Agregar_Puesto(DataTable Dt_Puestos, ListItem Puesto_Agregar)
    {
        Cls_Cat_Puestos_Negocio Obj_Puestos = new Cls_Cat_Puestos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Puestos_Consultado = null;//Variable que almacena el resultado de la consulta.
        DataRow Renglon = null;//Variablñe que almacena un resgitro de la consulta.

        try
        {
            Obj_Puestos.P_Puesto_ID = Puesto_Agregar.Value;
            Dt_Puestos_Consultado = Obj_Puestos.Consultar_Puestos();

            Dt_Puestos_Consultado.Columns.Add(Cat_Nom_Dep_Puestos_Det.Campo_Tipo_Plaza, typeof(String));//se agrego para tener un identificador del puesto si es una plaza base, eventual, asimilable, subrogado, pensionado ó dieta.

            if (Dt_Puestos_Consultado is DataTable)
            {
                if (Dt_Puestos_Consultado.Rows.Count > 0)
                {
                    foreach (DataRow PUESTO in Dt_Puestos_Consultado.Rows)
                    {
                        if (PUESTO is DataRow)
                        {
                            Renglon = Dt_Puestos.NewRow();
                            Renglon[Cat_Puestos.Campo_Puesto_ID] = PUESTO[Cat_Puestos.Campo_Puesto_ID].ToString().Trim();
                            Renglon[Cat_Puestos.Campo_Nombre] = PUESTO[Cat_Puestos.Campo_Nombre].ToString().Trim();
                            Renglon[Cat_Puestos.Campo_Salario_Mensual] = PUESTO[Cat_Puestos.Campo_Salario_Mensual].ToString().Trim();
                            Renglon["ESTATUS_PUESTO"] = "DISPONIBLE";
                            Renglon[Cat_Nom_Dep_Puestos_Det.Campo_Clave] = Consultar_Consecutivo(Dt_Puestos);
                            Renglon[Cat_Nom_Dep_Puestos_Det.Campo_Tipo_Plaza] = Cmb_Tipo_Plaza.SelectedItem.Text.Trim().ToUpper();
                            Dt_Puestos.Rows.Add(Renglon);
                        }
                    }
                }
            }

            Session["PUESTOS_UNIDAD_RESPNSABLE"] = Dt_Puestos;
            Grid_Puestos.DataSource = (DataTable)Session["PUESTOS_UNIDAD_RESPNSABLE"];
            Grid_Puestos.DataBind();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al agregar un puesto a la unidad responsable. Error: [" + Ex.Message + "]");
        }
    }
    /// *******************************************************************************************************
    /// Nombre: Eliminar_Puesto
    /// 
    /// Descripción: Método que ejecuta quita una plaza de la lista de plaza de la unidad responsable.
    /// 
    /// Parámetros: Dt_Puestos.- Tabla que se almacena en memoria y que lista las plazas ue actualmente 
    ///             se tiene en la unidad responsable.
    ///             Puestos_ID.- Identificador de la plaza a quitar de la lista de plazas de la unidad responsable.
    /// 
    /// Usuario Creó: Juan alberto Hernández Negrete. 
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *******************************************************************************************************
    protected void Eliminar_Puesto(DataTable Dt_Puestos, String Puestos_ID)
    {
        try
        {
            String Clave = Puestos_ID.Split(new Char[] { '-' })[1];
            Puestos_ID = Puestos_ID.Split(new Char[] { '-' })[0];

            DataRow[] Dr_Puestos = Dt_Puestos.Select(Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID + "='" + Puestos_ID + "' AND " + Cat_Nom_Dep_Puestos_Det.Campo_Clave + "='" + Clave + "'");
            DataRow Dr_Puesto = null;

            if (Dr_Puestos.Length > 0)
            {
                Dr_Puesto = Dr_Puestos[0];
                Dt_Puestos.Rows.Remove(Dr_Puesto);
                Session["PUESTOS_UNIDAD_RESPNSABLE"] = Dt_Puestos;
                Grid_Puestos.DataSource = (DataTable)Session["PUESTOS_UNIDAD_RESPNSABLE"];
                Grid_Puestos.DataBind();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al agregar un puesto a la unidad responsable. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Validación Presupuestal)

    #region (Validación Presupuestal Sueldos)
    ///************************************************************************************
    /// Nombre Método: Total_Proyeccion_Resta_Anio
    /// 
    /// Descripción: Método que consulta y obtiene una proyección de lo que tiene
    ///              comprometido una unidad responble en la partida de SUELDOS.
    ///           
    /// Parámetros:
    ///
    /// Usuario creó: Juan Alberto Hernández Negrete
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa modificación:
    ///************************************************************************************
    private Double Total_Proyeccion_Resta_Anio(String Unidad_Responsable_ID, String Tipo_Plaza)
    {
        var TOTAL_PROYECCION = 0.0;//Variable almancena el total de presupuesto compremetido por el resto del año.
        Cls_Cat_Dependencias_Negocio Obj_Dependencias_Negocio = new Cls_Cat_Dependencias_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Dep_Puestos = null;//Variable que almacenara los puestos que tiene la unidad responsable.

        try
        {
            Obj_Dependencias_Negocio.P_Dependencia_ID = Unidad_Responsable_ID;
            Obj_Dependencias_Negocio.P_Tipo_Plaza = Tipo_Plaza;
            Dt_Dep_Puestos = Obj_Dependencias_Negocio.Consultar_Plazas_UR();

            if (Dt_Dep_Puestos is DataTable)
            {

                var PUESTOS = from objPuesto in Dt_Dep_Puestos.AsEnumerable()
                              where objPuesto.Field<String>(Cat_Nom_Dep_Puestos_Det.Campo_Estatus) == "OCUPADO"
                              select
                                  new
                                  {
                                      SD =
                              (Convert.ToDouble(objPuesto.Field<Decimal>(Cat_Puestos.Campo_Salario_Mensual)) /
                               Presidencia.Utilidades_Nomina.Cls_Utlidades_Nomina.Dias_Mes_Fijo)
                                  };

                TOTAL_PROYECCION =
                    PUESTOS.Sum(
                        puesto =>
                        puesto.SD *
                        ((new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59)).Subtract(
                            new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0)).Days + 1));
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el TOTAL ($) proyectado al número de días que le restan al año y a los puestos que existen en la unidad responsable. Error: [" + ex.Message + "]");
        }
        return TOTAL_PROYECCION;
    }
    ///************************************************************************************
    /// Nombre Método: Obtener_Proyeccion_Puesto_Agregar
    /// 
    /// Descripción: Método que realiza una proyeccion en base a su salario mensual de lo
    ///              deberá haber de presupuesto disponible para poder realizar el alta del
    ///              mismo en la unidad responsable.
    /// 
    /// Parámetros: Puesto_ID.- Identificador del puesto y que es de uso interno del sistema.
    ///
    /// Usuario creó: Juan Alberto Hernández Negrete
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa modificación:
    ///************************************************************************************
    private Double Obtener_Proyeccion_Puesto_Agregar(String Puesto_ID)
    {
        var TOTAL_PROYECCION = 0.0;//Variable que almacenara el monto total de la proyección realizada a puesto agregar.
        Cls_Cat_Puestos_Negocio Obj_Puestos = new Cls_Cat_Puestos_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Puestos = null;//Variable que almacenara el resultado de la consulta.

        try
        {
            Obj_Puestos.P_Puesto_ID = Puesto_ID;
            Dt_Puestos = Obj_Puestos.Consultar_Puestos();

            if (Dt_Puestos != null)
            {
                var puesto = from objPuesto in Dt_Puestos.AsEnumerable()
                             select
                                 new
                                 {
                                     SD =
                             (Convert.ToDouble(objPuesto.Field<Decimal>(Cat_Puestos.Campo_Salario_Mensual)) /
                              Presidencia.Utilidades_Nomina.Cls_Utlidades_Nomina.Dias_Mes_Fijo)
                                 };

                TOTAL_PROYECCION =
                    puesto.Sum(
                        auxPuesto =>
                        auxPuesto.SD *
                        ((new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59).Subtract(new DateTime(DateTime.Now.Year,
                                                                                                    DateTime.Now.Month,
                                                                                                    DateTime.Now.Day, 0,
                                                                                                    0, 0))).Days + 1));
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al realizar la proyección del puesto presupuestalmente. Error: [" + ex.Message + "]");
        }
        return TOTAL_PROYECCION;
    }
    ///************************************************************************************
    /// Nombre Método: Validacion_Presupuestal
    ///     
    /// Descripción: Método que hace la proyeccion del puesto agregar y lo valida contra 
    ///              el presupuesto disponible en la partida de sueldos de la UR.
    /// 
    /// Parámetros:
    ///
    /// Usuario creó: Juan Alberto Hernández Negrete
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa modificación:
    ///************************************************************************************
    private Boolean Validacion_Presupuestal()
    {
        Cls_Help_Nom_Validate_Presupuestal Obj_Presupuesto = new Cls_Help_Nom_Validate_Presupuestal();//Variable de conexión con la clase ayudante.
        Cls_Cat_Nom_Parametros_Contables_Negocio INF_PARAMETROS_CONTABLES = null;//Variable tipo object que almacena la informacion de los parametros contebles.
        Boolean Estatus = true;//Variable que almacena el estatus de la validación.
        var Mensaje = new StringBuilder();//Variable que almacena el mensaje a mostrar al usuario con información de la validacion.
        //::::::::::::::::::::::::::::::
        var Proyeccion_Puestos_Actuales_SUELDOS_BASE = 0.0;//Variable que almacena la proyección de lo que se tiene comprometido en sueldos base.
        var Proyeccion_Puestos_Actuales_EVENTUALES = 0.0;//Variable que almacena la proyección de lo que se tiene comprometido en sueldos eventuales.
        var Proyeccion_Puestos_Actuales_ASIMILADOS = 0.0;//Variable que almacena la proyección de lo que se tiene comprometido en sueldos asimilables.
        var Proyeccion_Puestos_Actuales_PENSIONADOS = 0.0;//Variable que almacena la proyección de lo que se tiene comprometido en sueldos pensionados.
        var Proyeccion_Puestos_Actuales_DIETAS = 0.0;//Variable que almacena la proyección de lo que se tiene comprometido en sueldos dietas.
        //::::::::::::::::::::::::::::::
        var Proyeccion_Puesto_Agregar_SUELDOS_BASE = 0.0;//Variable que almacenara el total de la proyección del costo de la plaza agregar de sueldos base.
        var Proyeccion_Puesto_Agregar_EVENTUALES = 0.0;//Variable que almacenara el total de la proyección del costo de la plaza agregar de sueldos eventuales.
        var Proyeccion_Puesto_Agregar_ASIMILADOS = 0.0;//Variable que almacenara el total de la proyección del costo de la plaza agregar de sueldos asimilables.
        var Proyeccion_Puesto_Agregar_PENSIONADOS = 0.0;//Variable que almacenara el total de la proyección del costo de la plaza agregar de sueldos base pensionados.
        var Proyeccion_Puesto_Agregar_DIETA = 0.0;//Variable que almacenara el total de la proyección del costo de la plaza agregar de sueldos base ditas.
        //::::::::::::::::::::::::::::::
        var Acumulado_Sueldos_Base = 0.0;//Variable que almacena el acumulado de las proyecciones de las plazas existentes y las nuevas agregar en sueldos base.
        var Acumulado_Eventuales = 0.0;//Variable que almacena el acumulado de las proyecciones de las plazas existentes y las nuevas agregar en sueldos eventuales.
        var Acumulado_Asimilables = 0.0;//Variable que almacena el acumulado de las proyecciones de las plazas existentes y las nuevas agregar en sueldos asimilables.
        var Acumulado_Pensionados = 0.0;//Variable que almacena el acumulado de las proyecciones de las plazas existentes y las nuevas agregar en sueldos pensionados.
        var Acumulado_Dieta = 0.0;//Variable que almacena el acumulado de las proyecciones de las plazas existentes y las nuevas agregar en sueldos dietas.
        //::::::::::::::::::::::::::::::
        var PRESUPUESTO_SUELDOS_BASE = 0.0;//Variable que almacena el presupuesto disponible en la UR para la partida de sueldos base.
        var PRESUPUESTO_EVENTUALES = 0.0;//Variable que almacena el presupuesto disponible en la UR para la partida de sueldos eventuales.
        var PRESUPUESTO_ASIMILADOS = 0.0;//Variable que almacena el presupuesto disponible en la UR para la partida de sueldos asimilables.
        var PRESUPUESTO_PENSIONADOS = 0.0;//Variable que almacena el presupuesto disponible en la UR para la partida de sueldos pensionados.
        var PRESUPUESTO_DIETAS = 0.0;//Variable que almacena el presupuesto disponible en la UR para la partida de sueldos dietas.

        try
        {
            INF_PARAMETROS_CONTABLES = Cls_Ayudante_Nom_Informacion._Informacion_Parametro_Contable();//Consultamos el parámetro contable.

            //Obtenemos los montos en sueldos comprometidos actualmente apartir del día de hoy hasta fin de año..
            Proyeccion_Puestos_Actuales_SUELDOS_BASE = Total_Proyeccion_Resta_Anio(Txt_Dependencia_ID.Text.Trim(), "BASE-SUBSEMUN-SUBROGADOS");
            Proyeccion_Puestos_Actuales_EVENTUALES = Total_Proyeccion_Resta_Anio(Txt_Dependencia_ID.Text.Trim(), "EVENTUAL");
            Proyeccion_Puestos_Actuales_ASIMILADOS = Total_Proyeccion_Resta_Anio(Txt_Dependencia_ID.Text.Trim(), "ASIMILABLE");
            Proyeccion_Puestos_Actuales_PENSIONADOS = Total_Proyeccion_Resta_Anio(Txt_Dependencia_ID.Text.Trim(), "PENSIONADO");
            Proyeccion_Puestos_Actuales_DIETAS = Total_Proyeccion_Resta_Anio(Txt_Dependencia_ID.Text.Trim(), "DIETA");


            if (Cmb_Tipo_Plaza.SelectedIndex > 0)
            {
                if (Cmb_Puestos.SelectedIndex > 0)
                {
                    switch (Cmb_Tipo_Plaza.SelectedItem.Text.Trim().ToUpper())
                    {
                        case "BASE-SUBSEMUN-SUBROGADOS":
                            Proyeccion_Puesto_Agregar_SUELDOS_BASE += Obtener_Proyeccion_Puesto_Agregar(Cmb_Puestos.SelectedValue.Trim());
                            break;
                        case "EVENTUAL":
                            Proyeccion_Puesto_Agregar_EVENTUALES += Obtener_Proyeccion_Puesto_Agregar(Cmb_Puestos.SelectedValue.Trim());
                            break;
                        case "ASIMILABLE":
                            Proyeccion_Puesto_Agregar_ASIMILADOS += Obtener_Proyeccion_Puesto_Agregar(Cmb_Puestos.SelectedValue.Trim());
                            break;
                        case "PENSIONADO":
                            Proyeccion_Puesto_Agregar_PENSIONADOS += Obtener_Proyeccion_Puesto_Agregar(Cmb_Puestos.SelectedValue.Trim());
                            break;
                        case "DIETA":
                            Proyeccion_Puesto_Agregar_DIETA += Obtener_Proyeccion_Puesto_Agregar(Cmb_Puestos.SelectedValue.Trim());
                            break;

                        default:
                            break;
                    }
                }
            }

            //Obtenemos la proyección presupuestal del puesto agregados a la tabla de puestos y con un estatus de disponible apartir del día de hoy hasta fin de año.
            foreach (GridViewRow puesto in Grid_Puestos.Rows)
            {
                if (puesto != null)
                {
                    if (!String.IsNullOrEmpty(puesto.Cells[3].Text))
                    {
                        if (puesto.Cells[3].Text.ToUpper().Equals("DISPONIBLE"))
                        {
                            if (!String.IsNullOrEmpty(puesto.Cells[0].Text))
                            {
                                switch (puesto.Cells[5].Text.Trim().ToUpper())
                                {
                                    case "BASE-SUBSEMUN-SUBROGADOS":
                                        Proyeccion_Puesto_Agregar_SUELDOS_BASE += Obtener_Proyeccion_Puesto_Agregar(puesto.Cells[0].Text.Trim());
                                        break;
                                    case "EVENTUAL":
                                        Proyeccion_Puesto_Agregar_EVENTUALES += Obtener_Proyeccion_Puesto_Agregar(puesto.Cells[0].Text.Trim());
                                        break;
                                    case "ASIMILABLE":
                                        Proyeccion_Puesto_Agregar_ASIMILADOS += Obtener_Proyeccion_Puesto_Agregar(puesto.Cells[0].Text.Trim());
                                        break;
                                    case "PENSIONADO":
                                        Proyeccion_Puesto_Agregar_PENSIONADOS += Obtener_Proyeccion_Puesto_Agregar(puesto.Cells[0].Text.Trim());
                                        break;
                                    case "DIETA":
                                        Proyeccion_Puesto_Agregar_DIETA += Obtener_Proyeccion_Puesto_Agregar(puesto.Cells[0].Text.Trim());
                                        break;

                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            //Obtenemos los montos acumulados de cada proyección realizada por tipo de sueldo [BASE, EVENTUAL, ASIMILABLE Y PENSIONADOS].
            Acumulado_Sueldos_Base = Proyeccion_Puesto_Agregar_SUELDOS_BASE + Proyeccion_Puestos_Actuales_SUELDOS_BASE;
            Acumulado_Eventuales = Proyeccion_Puesto_Agregar_EVENTUALES + Proyeccion_Puestos_Actuales_EVENTUALES;
            Acumulado_Asimilables = Proyeccion_Puesto_Agregar_ASIMILADOS + Proyeccion_Puestos_Actuales_ASIMILADOS;
            Acumulado_Pensionados = Proyeccion_Puesto_Agregar_PENSIONADOS + Proyeccion_Puestos_Actuales_PENSIONADOS;
            Acumulado_Dieta = Proyeccion_Puesto_Agregar_DIETA + Proyeccion_Puestos_Actuales_DIETAS;

            //CONSULTAMOS EL PRESUPUESTO DISPONIBLE EN LA UNIDAD RESPONSABLE PARA LA PARTIDA DE : [BASE, EVENTUAL, ASIMILABLE Y PENSIONADOS].
            PRESUPUESTO_SUELDOS_BASE = Obj_Presupuesto.Consultar_Comprometido_Sueldos(Txt_Dependencia_ID.Text.Trim(),
                                                                               INF_PARAMETROS_CONTABLES.P_Sueldos_Base);

            PRESUPUESTO_EVENTUALES = Obj_Presupuesto.Consultar_Comprometido_Sueldos(Txt_Dependencia_ID.Text.Trim(),
                                                                   INF_PARAMETROS_CONTABLES.P_Remuneraciones_Eventuales);

            PRESUPUESTO_ASIMILADOS = Obj_Presupuesto.Consultar_Comprometido_Sueldos(Txt_Dependencia_ID.Text.Trim(),
                                                       INF_PARAMETROS_CONTABLES.P_Honorarios_Asimilados);

            PRESUPUESTO_PENSIONADOS = Obj_Presupuesto.Consultar_Disponible(Txt_Dependencia_ID.Text.Trim(),
                                                       INF_PARAMETROS_CONTABLES.P_Pensiones);

            PRESUPUESTO_DIETAS = Obj_Presupuesto.Consultar_Comprometido_Sueldos(Txt_Dependencia_ID.Text.Trim(),
                                           INF_PARAMETROS_CONTABLES.P_Dietas);


            //REALIZAMOS LAS VALIDACIONES PARA VALIDAR EL PRESUPUESTO DISPONIBLE SEA SUSTENTABLE PARA LAS OPERACIONES REALIZADAS.
            if (Acumulado_Sueldos_Base > PRESUPUESTO_SUELDOS_BASE)
            {
                Estatus = false;
                Mensaje.Append(Crear_Mensaje(Estatus, Proyeccion_Puestos_Actuales_SUELDOS_BASE,
                                             Proyeccion_Puesto_Agregar_SUELDOS_BASE,
                                             Acumulado_Sueldos_Base, PRESUPUESTO_SUELDOS_BASE, "SUELDOS BASE"));
            }
            else
            {
                Mensaje.Append(Crear_Mensaje(Estatus, Proyeccion_Puestos_Actuales_SUELDOS_BASE,
                                             Proyeccion_Puesto_Agregar_SUELDOS_BASE,
                                             Acumulado_Sueldos_Base, PRESUPUESTO_SUELDOS_BASE, "SUELDOS BASE"));
            }

            //:::::::::::::::::::::
            if (Acumulado_Eventuales > PRESUPUESTO_EVENTUALES)
            {
                Estatus = false;

                Mensaje.Append(Crear_Mensaje(Estatus, Proyeccion_Puestos_Actuales_EVENTUALES,
                                             Proyeccion_Puesto_Agregar_EVENTUALES,
                                             Acumulado_Eventuales, PRESUPUESTO_EVENTUALES, "REMUNERACIONES EVENTUALES"));
            }
            else
            {
                Mensaje.Append(Crear_Mensaje(Estatus, Proyeccion_Puestos_Actuales_EVENTUALES,
                             Proyeccion_Puesto_Agregar_EVENTUALES,
                             Acumulado_Eventuales, PRESUPUESTO_EVENTUALES, "REMUNERACIONES EVENTUALES"));
            }


            //:::::::::::::::::::::
            if (Acumulado_Asimilables > PRESUPUESTO_ASIMILADOS)
            {
                Estatus = false;

                Mensaje.Append(Crear_Mensaje(Estatus, Proyeccion_Puestos_Actuales_ASIMILADOS,
                                             Proyeccion_Puesto_Agregar_ASIMILADOS,
                                             Acumulado_Asimilables, PRESUPUESTO_ASIMILADOS, "HONORARIOS ASIMILADOS"));
            }
            else
            {
                Mensaje.Append(Crear_Mensaje(Estatus, Proyeccion_Puestos_Actuales_ASIMILADOS,
                                             Proyeccion_Puesto_Agregar_ASIMILADOS,
                                             Acumulado_Asimilables, PRESUPUESTO_ASIMILADOS, "HONORARIOS ASIMILADOS"));
            }

            //:::::::::::::::::
            if (Acumulado_Pensionados > PRESUPUESTO_PENSIONADOS)
            {
                Estatus = false;

                Mensaje.Append(Crear_Mensaje(Estatus, Proyeccion_Puestos_Actuales_PENSIONADOS,
                                             Proyeccion_Puesto_Agregar_PENSIONADOS,
                                             Acumulado_Pensionados, PRESUPUESTO_PENSIONADOS, "PENSIONADOS"));
            }
            else
            {
                Mensaje.Append(Crear_Mensaje(Estatus, Proyeccion_Puestos_Actuales_PENSIONADOS,
                             Proyeccion_Puesto_Agregar_PENSIONADOS,
                             Acumulado_Pensionados, PRESUPUESTO_PENSIONADOS, "PENSIONADOS"));
            }

            //:::::::::::::::::
            if (Acumulado_Dieta > PRESUPUESTO_DIETAS)
            {
                Estatus = false;

                Mensaje.Append(Crear_Mensaje(Estatus, Proyeccion_Puestos_Actuales_DIETAS,
                                             Proyeccion_Puesto_Agregar_DIETA,
                                             Acumulado_Dieta, PRESUPUESTO_DIETAS, "DIETA"));
            }
            else
            {
                Mensaje.Append(Crear_Mensaje(Estatus, Proyeccion_Puestos_Actuales_DIETAS,
                                             Proyeccion_Puesto_Agregar_DIETA,
                                             Acumulado_Dieta, PRESUPUESTO_DIETAS, "DIETA"));
            }          

            Ltr_Inf_Presupuestal_Sueldos.Text = HttpUtility.HtmlDecode(Mensaje.ToString());
        }
        catch (Exception ex)
        {
            throw new Exception("Error . Error: [" + ex.Message + "]");
        }
        return Estatus;
    }
    ///************************************************************************************
    /// Nombre Método: Crear_Mensaje
    /// 
    /// Descripción: Método que mostrara la informacion al usuario.
    /// 
    /// Parámetros: Estatus.-
    ///             Proyeccion_Puestos_Actuales.-
    ///             Proyeccion_Puesto_Agregar.- 
    ///             Proyeccion_Total_Acumulado.-
    ///             TOTAL_PRESUPUESTO.-
    ///             TIPO_SUELDO.- 
    ///
    /// Usuario creó: Juan Alberto Hernández Negrete
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa modificación:
    ///************************************************************************************
    private String Crear_Mensaje(Boolean Estatus,
        Double Proyeccion_Puestos_Actuales,
        Double Proyeccion_Puesto_Agregar,
        Double Proyeccion_Total_Acumulado,
        Double TOTAL_PRESUPUESTO,
        String TIPO_SUELDO)
    {
        var Mensaje = new StringBuilder();

        try
        {
            Mensaje.Append(
                "<table width='97%' style='background-color: #2F4E7D;'");

            Mensaje.Append("<thead>");
            Mensaje.Append("<tr>");
            Mensaje.Append(
                "<th style='font-size:10px; background-color: #2F4E7D; color:White; width:70%;' align='left'>");
            Mensaje.Append("DESCRIPCIÓN");
            Mensaje.Append("</th>");
            Mensaje.Append(
                "<th style='font-size:10px; background-color: #2F4E7D; color:White; width:30%;' align='center'>");
            Mensaje.Append("($) TOTAL");
            Mensaje.Append("</th>");
            Mensaje.Append("</thead>");

            Mensaje.Append("<tbody>");

            Mensaje.Append("<tr>");
            Mensaje.Append(
                "<td style='font-size:10px; background-color: #cccccc; width:70%; color:#333333; font-weight: bold;' align='left'>");
            Mensaje.Append("Cantidad Comprometida en " + TIPO_SUELDO);
            Mensaje.Append("</td>");
            Mensaje.Append(
                "<td style='font-size:11px; font-family:Courier New; background-color: #cccccc; width:30%; color:#333333; font-weight: bold;' align='center'>");
            Mensaje.Append(String.Format("{0:c}", Proyeccion_Puestos_Actuales));
            Mensaje.Append("</td>");
            Mensaje.Append("</tr>");

            Mensaje.Append("<tr>");
            Mensaje.Append(
                "<td style='font-size:10px; background-color: #cccccc; width:70%; color:#333333; font-weight: bold;' align='left'>");
            Mensaje.Append("Total Proyección nuevos Puestos agregar");
            Mensaje.Append("</td>");
            Mensaje.Append(
                "<td style='font-size:11px; font-family:Courier New; background-color: #cccccc; width:30%; color:#333333; font-weight: bold;' align='center'>");
            Mensaje.Append(String.Format("{0:c}", Proyeccion_Puesto_Agregar));
            Mensaje.Append("</td>");
            Mensaje.Append("</tr>");

            if (Estatus)
            {
                Mensaje.Append("<tr>");
                Mensaje.Append(
                    "<td style='font-size:12px; background-color: #cccccc; width:70%; color:#333333; font-weight: bold;' align='right'>");
                Mensaje.Append("TOTAL =");
                Mensaje.Append("</td>");
                Mensaje.Append(
                    "<td style='font-size:11px; font-family:Courier New; background-color: #cccccc; width:20%; color:#333333; font-weight: bold;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Proyeccion_Total_Acumulado));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");
            }
            else
            {
                Mensaje.Append("<tr>");
                Mensaje.Append(
                    "<td style='font-size:12px; background-color: #cccccc; width:70%; color:#333333; font-weight: bold;' align='right'>");
                Mensaje.Append("TOTAL =");
                Mensaje.Append("</td>");
                Mensaje.Append(
                    "<td style='font-size:11px; font-family:Courier New; background-color: #cccccc; width:20%; color:Red; font-weight: bold;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Proyeccion_Total_Acumulado));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");
            }

            Mensaje.Append("<tr>");
            Mensaje.Append(
                "<td style='font-size:10px; background-color: #cccccc; width:70%; color:#333333; font-weight: bold;' align='left'>");
            Mensaje.Append("Presupuesto Partida de " + TIPO_SUELDO + " de la Unidad Responsable " +
                           Txt_Nombre_Dependencia.Text);
            Mensaje.Append("</td>");
            Mensaje.Append(
                "<td style='font-size:11px; font-family:Courier New; background-color: #cccccc; width:20%; color:#333333; font-weight: bold;' align='center'>");
            Mensaje.Append(String.Format("{0:c}", TOTAL_PRESUPUESTO));
            Mensaje.Append("</td>");
            Mensaje.Append("</tr>");

            Mensaje.Append("<tr>");
            Mensaje.Append("<td colspan='2' style='color:White; font-size:10px; width:100%;' align='center'>");
            Mensaje.Append("¡¡¡Validación de " + TIPO_SUELDO + "!!!");
            Mensaje.Append("</td<");
            Mensaje.Append("</tr>");
            Mensaje.Append("</table>");
            Mensaje.Append("<hr />");
        }
        catch (Exception ex)
        {
            throw new Exception("Error . Error: [" + ex.Message + "]");
        }
        return Mensaje.ToString();
    }
    #endregion

    #region (Validación Presupuestal PSM)
    ///************************************************************************************
    /// Nombre Método: Total_Proyeccion_Resta_Anio_PSM
    /// 
    /// Descripción: Método que consulta y obtiene una proyección de lo que tiene
    ///              comprometido una unidad responsable en la partida de PSM.
    ///           
    /// Parámetros:
    ///
    /// Usuario creó: Juan Alberto Hernández Negrete
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa modificación:
    ///************************************************************************************
    private Double Total_Proyeccion_Resta_Anio_PSM(String Unidad_Responsable_ID)
    {
        var TOTAL_PROYECCION = 0.0;//Variable almancena el total de presupuesto compremetido por el resto del año.
        Cls_Cat_Dependencias_Negocio Obj_Dependencias_Negocio = new Cls_Cat_Dependencias_Negocio();//Variable de conexión con la capa de negocios.
        Cls_Cat_Nom_Parametros_Negocio INF_PARAMETROS = null;
        DataTable Dt_Dep_Puestos = null;//Variable que almacenara los puestos que tiene la unidad responsable.

        try
        {
            INF_PARAMETROS = Cls_Ayudante_Nom_Informacion._Informacion_Parametros_Nomina();//Consultamos el parámetro de la nómina.

            Obj_Dependencias_Negocio.P_Dependencia_ID = Unidad_Responsable_ID;
            Dt_Dep_Puestos = Obj_Dependencias_Negocio.Consultar_Puestos_UR();

            if (Dt_Dep_Puestos is DataTable)
            {

                var PUESTOS = from objPuesto in Dt_Dep_Puestos.AsEnumerable()
                              where objPuesto.Field<String>(Cat_Nom_Dep_Puestos_Det.Campo_Estatus) == "OCUPADO" &&
                                    objPuesto.Field<String>(Cat_Puestos.Campo_Aplica_PSM) == "S"
                              select
                                  new
                                  {
                                      PSM =
                              ((Convert.ToDouble(objPuesto.Field<Decimal>(Cat_Puestos.Campo_Salario_Mensual)) /
                                Presidencia.Utilidades_Nomina.Cls_Utlidades_Nomina.Dias_Mes_Fijo) *
                               Convert.ToDouble(
                                   String.IsNullOrEmpty(INF_PARAMETROS.P_ISSEG_Porcentaje_Prevision_Social_Multiple)
                                       ? "0"
                                       : INF_PARAMETROS.P_ISSEG_Porcentaje_Prevision_Social_Multiple))
                                  };

                TOTAL_PROYECCION =
                    PUESTOS.Sum(
                        puesto =>
                        puesto.PSM *
                        ((new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59)).Subtract(
                            new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0)).Days + 1));
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el TOTAL ($) proyectado al número de días que le restan al año y a los puestos que existen en la unidad responsable. Error: [" + ex.Message + "]");
        }
        return TOTAL_PROYECCION;
    }
    ///************************************************************************************
    /// Nombre Método: Obtener_Proyeccion_Puesto_Agregar_PSM
    /// 
    /// Descripción: Método que realiza una proyección en base a la PSM
    ///              deberá haber de presupuesto disponible para poder realizar el alta del
    ///              mismo en la unidad responsable.
    /// 
    /// Parámetros: Puesto_ID.- Identificador del puesto y que es de uso interno del sistema.
    ///
    /// Usuario creó: Juan Alberto Hernández Negrete
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa modificación:
    ///************************************************************************************
    private Double Obtener_Proyeccion_Puesto_Agregar_PSM(String Puesto_ID)
    {
        var TOTAL_PROYECCION = 0.0;//Variable que almacenara el monto total de la proyección realizada a puesto agregar.
        Cls_Cat_Puestos_Negocio Obj_Puestos = new Cls_Cat_Puestos_Negocio();//Variable de conexión con la capa de negocios.
        Cls_Cat_Nom_Parametros_Negocio INF_PARAMETROS = null;
        DataTable Dt_Puestos = null;//Variable que almacenara el resultado de la consulta.

        try
        {
            INF_PARAMETROS = Cls_Ayudante_Nom_Informacion._Informacion_Parametros_Nomina();//Consultamos el parámetro de la nómina.

            Obj_Puestos.P_Puesto_ID = Puesto_ID;
            Dt_Puestos = Obj_Puestos.Consultar_Puestos();

            if (Dt_Puestos != null)
            {
                var puesto = from objPuesto in Dt_Puestos.AsEnumerable()
                             where objPuesto.Field<String>(Cat_Puestos.Campo_Aplica_PSM) == "S"
                             select
                                 new
                                 {
                                     PSM =
                             ((Convert.ToDouble(objPuesto.Field<Decimal>(Cat_Puestos.Campo_Salario_Mensual)) /
                               Presidencia.Utilidades_Nomina.Cls_Utlidades_Nomina.Dias_Mes_Fijo) *
                              Convert.ToDouble(
                                  String.IsNullOrEmpty(INF_PARAMETROS.P_ISSEG_Porcentaje_Prevision_Social_Multiple)
                                      ? "0"
                                      : INF_PARAMETROS.P_ISSEG_Porcentaje_Prevision_Social_Multiple))
                                 };

                TOTAL_PROYECCION =
                    puesto.Sum(
                        auxPuesto =>
                        auxPuesto.PSM *
                        ((new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59).Subtract(new DateTime(DateTime.Now.Year,
                                                                                                    DateTime.Now.Month,
                                                                                                    DateTime.Now.Day, 0,
                                                                                                    0, 0))).Days + 1));
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al realizar la proyección del puesto presupuestalmente. Error: [" + ex.Message + "]");
        }
        return TOTAL_PROYECCION;
    }
    ///************************************************************************************
    /// Nombre Método: Validacion_Presupuestal_PSM
    /// 
    /// Descripción: Método que hace la proyeccion de la PSM del puesto agregar y lo valida contra 
    ///              el presupuesto disponible en la partida de PSM de la UR.
    /// 
    /// Parámetros:
    ///
    /// Usuario creó: Juan Alberto Hernández Negrete
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa modificación:
    ///************************************************************************************
    private Boolean Validacion_Presupuestal_PSM()
    {
        Cls_Help_Nom_Validate_Presupuestal Obj_Presupuesto = new Cls_Help_Nom_Validate_Presupuestal();
        Cls_Cat_Nom_Parametros_Contables_Negocio INF_PARAMETROS_CONTABLES = null;
        Boolean Estatus = true;
        var Mensaje = new StringBuilder();
        var Proyeccion_Puestos_Actuales_PSM = 0.0;
        var Proyeccion_Puesto_Agregar_PSM = 0.0;
        var Acumulado = 0.0;
        var PRESUPUESTO = 0.0;

        try
        {
            INF_PARAMETROS_CONTABLES = Cls_Ayudante_Nom_Informacion._Informacion_Parametro_Contable();

            //Obtenemos los montos en PSM comprometidos actualmente apartir del día de hoy hasta fin de año..
            Proyeccion_Puestos_Actuales_PSM = Total_Proyeccion_Resta_Anio_PSM(Txt_Dependencia_ID.Text.Trim());

            //Obtenemos la proyección presupuestal del PSM agregar apartir del día de hoy hasta fin de año.
            if (Cmb_Puestos.SelectedIndex > 0)
                Proyeccion_Puesto_Agregar_PSM =
                    Obtener_Proyeccion_Puesto_Agregar_PSM(Cmb_Puestos.SelectedValue.Trim());


            //Obtenemos la proyección presupuestal de la PSM de los puestos agregados a la tabla de puestos y con un estatus de disponible apartir del día de hoy hasta fin de año.
            foreach (GridViewRow puesto in Grid_Puestos.Rows)
            {
                if (puesto != null)
                {
                    if (!String.IsNullOrEmpty(puesto.Cells[3].Text))
                    {
                        if (puesto.Cells[3].Text.ToUpper().Equals("DISPONIBLE"))
                        {
                            if (!String.IsNullOrEmpty(puesto.Cells[0].Text))
                                Proyeccion_Puesto_Agregar_PSM +=
                                    Obtener_Proyeccion_Puesto_Agregar_PSM(puesto.Cells[0].Text.Trim());
                        }
                    }
                }
            }

            //Obtenemos los montos acumulados de cada proyección de PSM.
            Acumulado = Proyeccion_Puestos_Actuales_PSM + Proyeccion_Puesto_Agregar_PSM;

            //CONSULTAMOS EL PRESUPUESTO DISPONIBLE EN LA UNIDAD RESPONSABLE PARA LA PARTIDA DE PSM.
            PRESUPUESTO = Obj_Presupuesto.Consultar_Comprometido_Sueldos(Txt_Dependencia_ID.Text.Trim(),
                                                                               INF_PARAMETROS_CONTABLES.P_Prevision_Social_Multiple);

            //REALIZAMOS LAS VALIDACIONES PARA VALIDAR EL PRESUPUESTO DISPONIBLE SEA SUSTENTABLE PARA LAS OPERACIONES REALIZADAS.
            if (Acumulado > PRESUPUESTO)
            {
                Estatus = false;
                Mensaje.Append(Crear_Mensaje_PSM(Estatus, Proyeccion_Puestos_Actuales_PSM,
                                                 Proyeccion_Puesto_Agregar_PSM,
                                                 Acumulado, PRESUPUESTO));
            }
            else
            {
                Mensaje.Append(Crear_Mensaje_PSM(Estatus, Proyeccion_Puestos_Actuales_PSM,
                                                 Proyeccion_Puesto_Agregar_PSM,
                                                 Acumulado, PRESUPUESTO));
            }

            Ltr_Inf_Presupuestal_PSM.Text = Mensaje.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al realizar la validación presupuestal de la PSM. Error: [" + ex.Message + "]");
        }
        return Estatus;
    }
    ///************************************************************************************
    /// Nombre Método: Crear_Mensaje
    /// 
    /// Descripción: Método que mostrara la informacion al usuario.
    /// 
    /// Parámetros: Estatus.-
    ///             Proyeccion_Puestos_Actuales.-
    ///             Proyeccion_Puesto_Agregar.- 
    ///             Proyeccion_Total_Acumulado.-
    ///             TOTAL_PRESUPUESTO.-
    ///             TIPO_SUELDO.- 
    ///
    /// Usuario creó: Juan Alberto Hernández Negrete
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa modificación:
    ///************************************************************************************
    private String Crear_Mensaje_PSM(Boolean Estatus,
        Double Proyeccion_Puestos_Actuales,
        Double Proyeccion_Puesto_Agregar,
        Double Proyeccion_Total_Acumulado,
        Double TOTAL_PRESUPUESTO)
    {
        var Mensaje = new StringBuilder();

        try
        {
            Mensaje.Append(
                "<table width='98%' style='border-style:solid; background-color: #2F4E7D; border-color:silver;'");

            Mensaje.Append("<thead>");
            Mensaje.Append("<tr>");
            Mensaje.Append(
                "<th style='font-size:10px; background-color: #2F4E7D; color:White; width:70%;' align='left'>");
            Mensaje.Append("DESCRIPCIÓN");
            Mensaje.Append("</th>");
            Mensaje.Append(
                "<th style='font-size:10px; background-color: #2F4E7D; color:White; width:30%;' align='center'>");
            Mensaje.Append("($) TOTAL");
            Mensaje.Append("</th>");
            Mensaje.Append("</thead>");

            Mensaje.Append("<tbody>");

            Mensaje.Append("<tr>");
            Mensaje.Append(
                "<td style='font-size:10px; background-color: #cccccc; width:70%; color:#333333; font-weight: bold;' align='left'>");
            Mensaje.Append("Cantidad Comprometida en PSM");
            Mensaje.Append("</td>");
            Mensaje.Append(
                "<td style='font-size:11px; font-family:Courier New; background-color: #cccccc; width:30%; color:#333333; font-weight: bold;' align='center'>");
            Mensaje.Append(String.Format("{0:c}", Proyeccion_Puestos_Actuales));
            Mensaje.Append("</td>");
            Mensaje.Append("</tr>");

            Mensaje.Append("<tr>");
            Mensaje.Append(
                "<td style='font-size:10px; background-color: #cccccc; width:70%; color:#333333; font-weight: bold;' align='left'>");
            Mensaje.Append("Total Proyección de PSM en nuevos Puestos agregar");
            Mensaje.Append("</td>");
            Mensaje.Append(
                "<td style='font-size:11px; font-family:Courier New; background-color: #cccccc; width:30%; color:#333333; font-weight: bold;' align='center'>");
            Mensaje.Append(String.Format("{0:c}", Proyeccion_Puesto_Agregar));
            Mensaje.Append("</td>");
            Mensaje.Append("</tr>");

            if (Estatus)
            {
                Mensaje.Append("<tr>");
                Mensaje.Append(
                    "<td style='font-size:12px; background-color: #cccccc; width:70%; color:#333333; font-weight: bold;' align='right'>");
                Mensaje.Append("TOTAL =");
                Mensaje.Append("</td>");
                Mensaje.Append(
                    "<td style='font-size:11px; font-family:Courier New; background-color: #cccccc; width:20%; color:#333333; font-weight: bold;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Proyeccion_Total_Acumulado));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");
            }
            else
            {
                Mensaje.Append("<tr>");
                Mensaje.Append(
                    "<td style='font-size:12px; background-color: #cccccc; width:70%; color:#333333; font-weight: bold;' align='right'>");
                Mensaje.Append("TOTAL =");
                Mensaje.Append("</td>");
                Mensaje.Append(
                    "<td style='font-size:11px; font-family:Courier New; background-color: #cccccc; width:20%; color:Red; font-weight: bold;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Proyeccion_Total_Acumulado));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");
            }

            Mensaje.Append("<tr>");
            Mensaje.Append(
                "<td style='font-size:10px; background-color: #cccccc; width:70%; color:#333333; font-weight: bold;' align='left'>");
            Mensaje.Append("Presupuesto Partida de PSM de la Unidad Responsable " +
                           Txt_Nombre_Dependencia.Text);
            Mensaje.Append("</td>");
            Mensaje.Append(
                "<td style='font-size:11px; font-family:Courier New; background-color: #cccccc; width:20%; color:#333333; font-weight: bold;' align='center'>");
            Mensaje.Append(String.Format("{0:c}", TOTAL_PRESUPUESTO));
            Mensaje.Append("</td>");
            Mensaje.Append("</tr>");

            Mensaje.Append("<tr>");
            Mensaje.Append("<td colspan='2' style='color:White; font-size:10px; width:100%;' align='center'>");
            Mensaje.Append("¡¡¡Validación de Previsión Social Múltiple!!!");
            Mensaje.Append("</td<");
            Mensaje.Append("</tr>");
            Mensaje.Append("</table>");
            Mensaje.Append("<hr />");
        }
        catch (Exception ex)
        {
            throw new Exception("Error . Error: [" + ex.Message + "]");
        }
        return Mensaje.ToString();
    }
    #endregion

    #endregion

    #endregion

    #region (Grid)

    #region (Grid Dependencias)
    ///************************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Dependencias_SelectedIndexChanged
    /// 
    /// DESCRIPCION : Consulta los datos de la dependencia que selecciono el usuario
    /// 
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 08/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///************************************************************************************************************************************************
    protected void Grid_Dependencias_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Dependencias_Negocio Rs_Consulta_Cat_Dependencias = new Cls_Cat_Dependencias_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Dependencias; //Variable que obtendra los datos de la consulta 

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Rs_Consulta_Cat_Dependencias.P_Dependencia_ID = Grid_Dependencias.SelectedRow.Cells[1].Text;
            Dt_Dependencias = Rs_Consulta_Cat_Dependencias.Consulta_Dependencias(); //Consulta todos los datos de la dependencia que fue seleccionada por el usuario
            if (Dt_Dependencias.Rows.Count > 0)
            {
                //Asigna los valores de los campos obtenidos de la consulta anterior a los controles de la forma
                foreach (DataRow Registro in Dt_Dependencias.Rows)
                {
                    if (!String.IsNullOrEmpty(Registro[Cat_Dependencias.Campo_Dependencia_ID].ToString()))
                    {
                        Txt_Dependencia_ID.Text = Registro[Cat_Dependencias.Campo_Dependencia_ID].ToString();

                        LLenar_Grid_Fte_Financiamiento(0, Rs_Consulta_Cat_Dependencias.Consultar_Sap_Det_Fte_Dependencia());
                        LLenar_Grid_Programas(0, Rs_Consulta_Cat_Dependencias.Consultar_Sap_Det_Prog_Dependencia());
                        Consultar_Puestos_Unidad_Responsable(Grid_Dependencias.SelectedRow.Cells[1].Text.Trim());
                    }

                    if (!String.IsNullOrEmpty(Registro[Cat_Dependencias.Campo_Nombre].ToString()))
                        Txt_Nombre_Dependencia.Text = Registro[Cat_Dependencias.Campo_Nombre].ToString();

                    if (!String.IsNullOrEmpty(Registro[Cat_Dependencias.Campo_Comentarios].ToString()))
                        Txt_Comentarios_Dependencia.Text = Registro[Cat_Dependencias.Campo_Comentarios].ToString();

                    if (!String.IsNullOrEmpty(Registro[Cat_Dependencias.Campo_Estatus].ToString()))
                        Cmb_Estatus_Dependencia.SelectedValue = Registro[Cat_Dependencias.Campo_Estatus].ToString();

                    if (!String.IsNullOrEmpty(Registro[Cat_Dependencias.Campo_Area_Funcional_ID].ToString()))
                        Cmb_Area_Funcional.SelectedIndex = Cmb_Area_Funcional.Items.IndexOf(Cmb_Area_Funcional.Items.FindByValue(Registro[Cat_Dependencias.Campo_Area_Funcional_ID].ToString()));

                    if (!String.IsNullOrEmpty(Registro[Cat_Dependencias.Campo_Clave].ToString()))
                        Txt_Clave_Dependecia.Text = Registro[Cat_Dependencias.Campo_Clave].ToString();

                    if (!String.IsNullOrEmpty(Registro[Cat_Dependencias.Campo_Grupo_Dependencia_ID].ToString()))
                        Cmb_Grupo_Dependencia.SelectedIndex = Cmb_Grupo_Dependencia.Items.IndexOf(Cmb_Grupo_Dependencia.Items.FindByValue(Registro[Cat_Dependencias.Campo_Grupo_Dependencia_ID].ToString()));

                }
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Cat_Dependencias();", true);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///************************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Dependencias_PageIndexChanging
    /// 
    /// DESCRIPCION : Consulta y cambia la página del Grid. 
    /// : 
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 08/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///************************************************************************************************************************************************
    protected void Grid_Dependencias_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpia_Controles(); //Limpia los controles de la forma
            Grid_Dependencias.PageIndex = e.NewPageIndex; //Asigna la nueva página que selecciono el usuario
            Llena_Grid_Dependencias(); //Muestra las dependencias que estan asignadas en la página seleccionada por el usuario
            Grid_Dependencias.SelectedIndex = -1;
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Cat_Dependencias();", true);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///************************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Dependencias_Sorting
    /// 
    /// DESCRIPCION : Reordena la columna del grid, ya sea de forma DESC o ASC
    /// : 
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 08/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///************************************************************************************************************************************************
    protected void Grid_Dependencias_Sorting(object sender, GridViewSortEventArgs e)
    {
        //Se consultan las dependencias que actualmente se encuentran registradas en el sistema.
        Consulta_Dependencias();

        DataTable Dt_Dependencias = (Grid_Dependencias.DataSource as DataTable);

        if (Dt_Dependencias != null)
        {
            DataView Dv_Dependencias = new DataView(Dt_Dependencias);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Dependencias.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Dependencias.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Dependencias.DataSource = Dv_Dependencias;
            Grid_Dependencias.DataBind();
        }
        ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Cat_Dependencias();", true);
    }
    #endregion

    #region (Grid Fuentes Financiamiento)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Agregar_Fuente_Financiamiento
    /// DESCRIPCION : Agrega una Fuente Financiamiento
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 8/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Agregar_Fuente_Financiamiento(DataTable _DataTable, GridView _GridView, DropDownList _DropDownList)
    {
        DataRow[] Filas;//Variable que almacenara un arreglo de DataRows
        DataTable Dt_Fuente_Financiamiento = (DataTable)Session["Dt_Fte_Financiamiento"];//Variable que almacenara una lista de empleados.
        Cls_Cat_SAP_Fuente_Financiamiento_Negocio Obj_Fuente_Financiamiento = new Cls_Cat_SAP_Fuente_Financiamiento_Negocio();//Variable de conexion con la capa de negocios

        try
        {
            int indice = _DropDownList.SelectedIndex;
            if (indice > 0)
            {
                Filas = _DataTable.Select(Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + "='" + _DropDownList.SelectedValue.Trim() + "'");
                if (Filas.Length > 0)
                {
                    //Si se encontro algun coincidencia entre el grupo a agregar con alguno agregado anteriormente, se avisa
                    //al usuario que elemento ha agregar ya existe en la tabla de grupos.
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                        "alert('No se puede agregar la fuente de financiamiento, ya que esta ya se ha agregada');", true);
                    _DropDownList.SelectedIndex = -1;
                }
                else
                {
                    Obj_Fuente_Financiamiento.P_Fuente_Financiamiento_ID = _DropDownList.SelectedValue.Trim();
                    DataTable Dt_Temporal = Obj_Fuente_Financiamiento.Consulta_Datos_Fuente_Financiamiento();
                    if (!(Dt_Temporal == null))
                    {
                        if (Dt_Temporal.Rows.Count > 0)
                        {
                            DataRow Renglon = Dt_Fuente_Financiamiento.NewRow();
                            Renglon[Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID] = Dt_Temporal.Rows[0][0].ToString();
                            Renglon[Cat_SAP_Fuente_Financiamiento.Campo_Clave] = Dt_Temporal.Rows[0][1].ToString();
                            Renglon[Cat_SAP_Fuente_Financiamiento.Campo_Descripcion] = Dt_Temporal.Rows[0][2].ToString();

                            Dt_Fuente_Financiamiento.Rows.Add(Renglon);
                            Dt_Fuente_Financiamiento.AcceptChanges();
                            Session["Dt_Fte_Financiamiento"] = Dt_Fuente_Financiamiento;
                            _GridView.Columns[0].Visible = true;
                            _GridView.DataSource = (DataTable)Session["Dt_Fte_Financiamiento"];
                            _GridView.DataBind();
                            _GridView.Columns[0].Visible = false;
                            _DropDownList.SelectedIndex = -1;
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                    "alert('No se a seleccionado ninguna fuente de financiamiento a agregar');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al agregar las fuentes de financiamiento. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Agregar_Fte_Financiamiento_Click
    /// 
    /// DESCRIPCION : Evento que genera la peticion para agregar una nueva fuente de financiamiento del
    ///               combo de fuentes de financiamiento a la tabla de fuentes de financiamiento.
    ///               
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 8/Marzo/201a
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Agregar_Fte_Financiamiento_Click(object sender, EventArgs e)
    {
        if (Cmb_Fuente_Financiamiento.SelectedIndex > 0)
        {
            if (Session["Dt_Fte_Financiamiento"] != null)
            {
                Agregar_Fuente_Financiamiento((DataTable)Session["Dt_Fte_Financiamiento"], Grid_Fuentes_Financiamiento, Cmb_Fuente_Financiamiento);
            }
            else
            {
                DataTable Dt_Fuentes_Financiamiento = new DataTable();//Variable que almacenara una lista de empleados
                //Definicion de sus columnas.
                Dt_Fuentes_Financiamiento.Columns.Add(Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID, typeof(System.String));
                Dt_Fuentes_Financiamiento.Columns.Add(Cat_SAP_Fuente_Financiamiento.Campo_Clave, typeof(System.String));
                Dt_Fuentes_Financiamiento.Columns.Add(Cat_SAP_Fuente_Financiamiento.Campo_Descripcion, typeof(System.String));

                Session["Dt_Fte_Financiamiento"] = Dt_Fuentes_Financiamiento;
                Grid_Fuentes_Financiamiento.DataSource = (DataTable)Session["Dt_Fte_Financiamiento"];
                Grid_Fuentes_Financiamiento.DataBind();

                Agregar_Fuente_Financiamiento(Dt_Fuentes_Financiamiento, Grid_Fuentes_Financiamiento, Cmb_Fuente_Financiamiento);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                "alert('No se a seleccionado ninguna percepcion a agregar');", true);
        }
        ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Cat_Dependencias();", true);
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Eliminar_Fte_Financiamiento_Click
    /// 
    /// DESCRIPCION : Evento que genera la peticion para Quitar el la fuente de financiamiento 
    ///               seleccionado de la tabla de fuentes de financiamiento.
    ///               
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 8/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Eliminar_Fte_Financiamiento_Click(object sender, EventArgs e)
    {
        DataRow[] Renglones;//Variable que almacena una lista de DataRows del  Grid_Empleados
        DataRow Renglon;//Variable que almacenara un Renglon del Grid_Empleados
        ImageButton Btn_Eliminar_Fte_Financiamiento = (ImageButton)sender;//Variable que almacenra el control Btn_Eliminar_Empleado

        if (Session["Dt_Fte_Financiamiento"] != null)
        {
            Renglones = ((DataTable)Session["Dt_Fte_Financiamiento"]).Select(Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + "='" + Btn_Eliminar_Fte_Financiamiento.CommandArgument + "'");

            if (Renglones.Length > 0)
            {
                Renglon = Renglones[0];
                DataTable Tabla = (DataTable)Session["Dt_Fte_Financiamiento"];
                Tabla.Rows.Remove(Renglon);
                Session["Dt_Fte_Financiamiento"] = Tabla;
                Grid_Fuentes_Financiamiento.SelectedIndex = (-1);
                LLenar_Grid_Fte_Financiamiento(Grid_Fuentes_Financiamiento.PageIndex, Tabla);
            }
        }
        else
        {
            Lbl_Mensaje_Error.Text = "Se debe seleccionar de la tabla el Empleados a quitar";
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
        ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Cat_Dependencias();", true);
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: LLenar_Grid_Fte_Financiamiento
    ///
    ///DESCRIPCIÓN: LLena el grid de fuentes de financiamiento
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 8/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void LLenar_Grid_Fte_Financiamiento(Int32 Pagina, DataTable Tabla)
    {
        Grid_Fuentes_Financiamiento.Columns[0].Visible = true;
        Grid_Fuentes_Financiamiento.SelectedIndex = (-1);
        Grid_Fuentes_Financiamiento.DataSource = Tabla;
        Grid_Fuentes_Financiamiento.PageIndex = Pagina;
        Grid_Fuentes_Financiamiento.DataBind();
        Grid_Fuentes_Financiamiento.Columns[0].Visible = false;
        Session["Dt_Fte_Financiamiento"] = Tabla;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Fuentes_Financiamiento_PageIndexChanging
    ///DESCRIPCIÓN: Realiza el Cambio de la pagina de la tabla.
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 8/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Fuentes_Financiamiento_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (Session["Dt_Fte_Financiamiento"] != null)
            {
                LLenar_Grid_Fte_Financiamiento(e.NewPageIndex, (DataTable)Session["Dt_Fte_Financiamiento"]);
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Cat_Dependencias();", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error cambiar de un de la tabla. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Grid Programas)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Agregar_Programas
    /// DESCRIPCION : Agrega un Programa
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 8/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Agregar_Programas(DataTable _DataTable, GridView _GridView, DropDownList _DropDownList)
    {
        DataRow[] Filas;//Variable que almacenara un arreglo de DataRows
        DataTable Dt_Programas = (DataTable)Session["Dt_Programas"];//Variable que almacenara una lista de empleados.
        Cls_Cat_Com_Proyectos_Programas_Negocio Obj_Programas = new Cls_Cat_Com_Proyectos_Programas_Negocio();//Variable de conexion con la capa de negocios

        try
        {
            int indice = _DropDownList.SelectedIndex;
            if (indice > 0)
            {
                Filas = _DataTable.Select(Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + "='" + _DropDownList.SelectedValue.Trim() + "'");
                if (Filas.Length > 0)
                {
                    //Si se encontro algun coincidencia entre el grupo a agregar con alguno agregado anteriormente, se avisa
                    //al usuario que elemento ha agregar ya existe en la tabla de grupos.
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                        "alert('No se puede agregar el Programa, ya que este ya se ha agregado');", true);
                    _DropDownList.SelectedIndex = -1;
                }
                else
                {
                    Obj_Programas.P_Proyecto_Programa_ID = _DropDownList.SelectedValue.Trim();
                    DataTable Dt_Temporal = Obj_Programas.Consulta_Programas_Proyectos();
                    if (!(Dt_Temporal == null))
                    {
                        if (Dt_Temporal.Rows.Count > 0)
                        {
                            DataRow Renglon = Dt_Programas.NewRow();
                            Renglon[Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID] = Dt_Temporal.Rows[0][0].ToString();
                            Renglon[Cat_SAP_Fuente_Financiamiento.Campo_Clave] = Dt_Temporal.Rows[0][5].ToString();
                            Renglon[Cat_SAP_Fuente_Financiamiento.Campo_Descripcion] = Dt_Temporal.Rows[0][3].ToString();

                            Dt_Programas.Rows.Add(Renglon);
                            Dt_Programas.AcceptChanges();
                            Session["Dt_Programas"] = Dt_Programas;
                            _GridView.Columns[0].Visible = true;
                            _GridView.DataSource = (DataTable)Session["Dt_Programas"];
                            _GridView.DataBind();
                            _GridView.Columns[0].Visible = false;
                            _DropDownList.SelectedIndex = -1;
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                    "alert('No se a seleccionado ningun programa a agregar');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al agregar las fuentes de financiamiento. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Agregar_Programa_Click
    /// 
    /// DESCRIPCION : Evento que genera la peticion para agregar un nuevo programa del
    ///               combo de programas a la tabla de programas.
    ///               
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 8/Marzo/201a
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Agregar_Programa_Click(object sender, EventArgs e)
    {
        if (Cmb_Programa.SelectedIndex > 0)
        {
            if (Session["Dt_Programas"] != null)
            {
                Agregar_Programas((DataTable)Session["Dt_Programas"], Grid_Programas, Cmb_Programa);
            }
            else
            {
                DataTable Dt_Programas = new DataTable();//Variable que almacenara una lista de empleados
                //Definicion de sus columnas.
                Dt_Programas.Columns.Add(Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID, typeof(System.String));
                Dt_Programas.Columns.Add(Cat_Com_Proyectos_Programas.Campo_Clave, typeof(System.String));
                Dt_Programas.Columns.Add(Cat_Com_Proyectos_Programas.Campo_Descripcion, typeof(System.String));

                Session["Dt_Programas"] = Dt_Programas;
                Grid_Programas.DataSource = (DataTable)Session["Dt_Programas"];
                Grid_Programas.DataBind();

                Agregar_Programas(Dt_Programas, Grid_Programas, Cmb_Programa);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                "alert('No se a seleccionado ningun programa a agregar');", true);
        }
        ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Cat_Dependencias();", true);
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Eliminar_Programa_Click
    /// 
    /// DESCRIPCION : Evento que genera la peticion para Quitar el programa
    ///               seleccionado de la tabla de programas.
    ///               
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 8/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Eliminar_Programa_Click(object sender, EventArgs e)
    {
        DataRow[] Renglones;//Variable que almacena una lista de DataRows del  Grid_Empleados
        DataRow Renglon;//Variable que almacenara un Renglon del Grid_Empleados
        ImageButton Btn_Eliminar_Programa = (ImageButton)sender;//Variable que almacenra el control Btn_Eliminar_Empleado

        if (Session["Dt_Programas"] != null)
        {
            Renglones = ((DataTable)Session["Dt_Programas"]).Select(Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + "='" + Btn_Eliminar_Programa.CommandArgument + "'");

            if (Renglones.Length > 0)
            {
                Renglon = Renglones[0];
                DataTable Tabla = (DataTable)Session["Dt_Programas"];
                Tabla.Rows.Remove(Renglon);
                Session["Dt_Programas"] = Tabla;
                Grid_Programas.SelectedIndex = (-1);
                LLenar_Grid_Programas(Grid_Fuentes_Financiamiento.PageIndex, Tabla);
            }
        }
        else
        {
            Lbl_Mensaje_Error.Text = "Se debe seleccionar de la tabla el Empleados a quitar";
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
        ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Cat_Dependencias();", true);
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: LLenar_Grid_Programas
    ///
    ///DESCRIPCIÓN: LLena el grid de programas
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 8/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void LLenar_Grid_Programas(Int32 Pagina, DataTable Tabla)
    {
        Grid_Programas.Columns[0].Visible = true;
        Grid_Programas.SelectedIndex = (-1);
        Grid_Programas.DataSource = Tabla;
        Grid_Programas.PageIndex = Pagina;
        Grid_Programas.DataBind();
        Grid_Programas.Columns[0].Visible = false;
        Session["Dt_Programas"] = Tabla;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Programas_PageIndexChanging
    ///DESCRIPCIÓN: Realiza el Cambio de la pagina de la tabla.
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 8/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Programas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (Session["Dt_Programas"] != null)
            {
                LLenar_Grid_Programas(e.NewPageIndex, (DataTable)Session["Dt_Programas"]);
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Cat_Dependencias();", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error cambiar de un de la tabla. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion

    #region (Eventos)

    #region (Eventos Operación)
    ///************************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Nuevo_Click
    /// 
    /// DESCRIPCION : Alta de un nuevo registro de dependencia.
    /// 
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 08/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///************************************************************************************************************************************************
    protected void Btn_Nuevo_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Limpia_Controles(); //Limpia los controles de la forma para poder introducir nuevos datos
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
            }
            else
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                //Valida si todos los campos requeridos estan llenos si es así da de alta los datos en la base de datos
                if (Validar_Datos_Dependencia())
                {
                    if (!Es_Clave_Repetida(Txt_Clave_Dependecia.Text.Trim()))
                    {
                        Txt_Clave_Dependecia.Text = "";

                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "La clave ya existe en el sistema.";
                    }
                    else
                    {
                        Alta_Dependencia(); //Da de alta la Dependencia con los datos que proporciono el usuario
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Cat_Dependencias();", true);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///************************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Modificar_Click
    /// 
    /// DESCRIPCION : Modifica el registro de dependencia seleccionado.
    /// 
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 08/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///************************************************************************************************************************************************
    protected void Btn_Modificar_Click(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                //Si el usuario selecciono una dependencia entonces habilita los controles para que pueda modificar la información
                //de la dependencia
                if (Txt_Dependencia_ID.Text != "")
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                }
                //Si el usuario no selecciono una dependencia le indica al usuario que la seleccione para poder modificar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione la Dependencia que desea modificar sus datos <br>";
                }
            }
            else
            {
                //Si el usuario proporciono todos los datos requeridos entonces modificar los datos de la dependencia en la BD
                if (Validar_Datos_Dependencia())
                {
                    //if (!Es_Clave_Repetida(Txt_Clave_Dependecia.Text.Trim()))
                    //{
                    //    Txt_Clave_Dependecia.Text = "";

                    //    Lbl_Mensaje_Error.Visible = true;
                    //    Img_Error.Visible = true;
                    //    Lbl_Mensaje_Error.Text = "La clave ya existe en el sistema.";
                    //}
                    //else
                    //{
                        Modificar_Dependencia(); //Modifica los datos de la Dependencia con los datos proporcionados por el usuario
                    //}
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Cat_Dependencias();", true);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///************************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Eliminar_Click
    /// 
    /// DESCRIPCION : Elimina el registro de dependencia seleccionado.
    /// 
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 08/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///************************************************************************************************************************************************
    protected void Btn_Eliminar_Click(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            //Si el usuario selecciono una dependencia entonces la elimina de la base de datos
            if (Txt_Dependencia_ID.Text != "")
            {
                Eliminar_Dependencia(); //Elimina la Dependencia que fue seleccionada por el usuario
            }
            //Si el usuario no selecciono alguna dependencia manda un mensaje indicando que es necesario que seleccione alguna para
            //poder eliminar
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione la Dependencia que desea eliminar <br>";
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Cat_Dependencias();", true);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///************************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Salir_Click
    /// 
    /// DESCRIPCION : Cancela y sale de la operación actual.
    /// 
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 08/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///************************************************************************************************************************************************
    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Salir")
            {
                Session.Remove("Consulta_Dependencias");
                Session.Remove("Dt_Fte_Financiamiento");
                Session.Remove("Dt_Programas");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Inicializa_Controles();//Habilita los controles para la siguiente operación del usuario en el catálogo
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Cat_Dependencias();", true);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///************************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Buscar_Dependencia_Click
    /// 
    /// DESCRIPCION : Ejecuta la búsqueda de la dependencia por la descripción ingresada.
    /// 
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 08/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///************************************************************************************************************************************************
    protected void Btn_Buscar_Dependencia_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Consulta_Dependencias(); //Consulta las dependencias que coincidan con el nombre porporcionado por el usuario
            //Limpia_Controles();
            //Si no se encontraron dependencias con un nombre similar al proporcionado por el usuario entonces manda un mensaje al usuario
            if (Grid_Dependencias.Rows.Count <= 0)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encontraron Dependencias con el nombre proporcionado <br>";
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Cat_Dependencias();", true);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion

    #region (Cajas de Texto)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Clave_Dependecia_TextChanged
    ///
    ///DESCRIPCIÓN: Valida si la clave que se ingreso no corresponde a una dependencia 
    ///             en el sistema.
    ///             
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 8/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Clave_Dependecia_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (!Es_Clave_Repetida(Txt_Clave_Dependecia.Text.Trim()))
            {
                Txt_Clave_Dependecia.Text = "";

                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "La clave ya existe en el sistema!";
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Cat_Dependencias();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    #endregion

    #region (Eventos (+/-) Puestos)
    /// *******************************************************************************************************
    /// Nombre: Btn_Agregar_Puesto_Click
    /// 
    /// Descripción: Ejecuta la operación de agregar un puesto a la unidad responsable.
    /// 
    /// Parámetros: No aplica.
    /// 
    /// Usuario Creó: Juan alberto Hernández Negrete. 
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *******************************************************************************************************
    protected void Btn_Agregar_Puesto_Click(object sender, EventArgs e)
    {
        try
        {
            if (Validacion_Presupuestal() && Validacion_Presupuestal_PSM())
            {
                if (Cmb_Puestos.SelectedIndex > 0)
                {
                    Agregar_Puesto_Unidad_Responsable();
                    Cmb_Puestos.SelectedIndex = -1;
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    /// *******************************************************************************************************
    /// Nombre: Btn_Eliminar_Puesto_Click
    /// 
    /// Descripción: Ejecuta la operación de eliminar un puesto a la unidad responsable.
    /// 
    /// Parámetros:
    /// 
    /// Usuario Creó: Juan alberto Hernández Negrete. 
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *******************************************************************************************************
    protected void Btn_Eliminar_Puesto_Click(object sender, EventArgs e)
    {
        Boolean Estatus_Plaza = false;//Variable que almacena el estatus de la operacion a realizar.

        try
        {
            //Validamos si el puesto a eliminar no se encuentra ocupado.
            Estatus_Plaza = !(((GridViewRow)((ImageButton)sender).Parent.Parent).Cells[3].Text.ToUpper().Equals("OCUPADO"));

            if (Estatus_Plaza)
                Eliminar_Puesto((DataTable)Session["PUESTOS_UNIDAD_RESPNSABLE"], ((ImageButton)sender).CommandArgument.Trim());
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Información", "alert('No es posible eliminar un puesto de la unidad responsable " +
                            "si el puesto actualmente se encuentra ocupado por algun empleado.');", true);


            Cmb_Puestos.SelectedIndex = -1;
            Validacion_Presupuestal();//Mostrara el cuadro conn la información presupuestal.
            Validacion_Presupuestal_PSM();
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    #endregion

    #endregion
}
