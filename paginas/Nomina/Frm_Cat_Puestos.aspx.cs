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
using Presidencia.Puestos.Negocios;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;

public partial class paginas_Nomina_Frm_Cat_Puestos : System.Web.UI.Page
{
    #region (Page Load)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Perfil_Click
    ///DESCRIPCIÓN: Agrega un perfil nuevo, validando que no este ya agregado
    ///PARAMETROS:  
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 08/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************        
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones                    
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
        Lbl_Mensaje_Error.Text = "";
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
    }
    #endregion

    #region (Metodos)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 26-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            Cargar_Combo_Perfiles();        //Consulta todas los perfiles que fueron dadas de alta en la BD
            Limpia_Controles();             //Limpia los controles del forma
            Consulta_Puestos();             //Consulta todas los puestos que fueron dadas de alta en la BD
            Cargar_Combo_Plazas();          //Consulta todas las plazas que fueron dadas de alta en la BD
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
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 26-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {
            Txt_Puesto_ID.Text = "";
            Txt_Nombre_Puesto.Text = "";
            Txt_Comentarios_Puesto.Text = "";
            Txt_Busqueda_Puesto.Text = "";
            Cmb_Estatus_Puesto.SelectedIndex = 0;
            Cmb_Perfil_Puesto.SelectedIndex = 0;
            Session.Remove("Dt_Perfiles");
            Grid_Perfiles.DataSource = new DataTable();
            Grid_Perfiles.DataBind();
            Txt_Salario_Mensual.Text = "";
            Grid_Puesto.SelectedIndex = -1;
            Cmb_Plaza.SelectedIndex = 0;
            Cmb_Aplica_Fondo_Retiro.SelectedIndex = -1;
            Cmb_Aplica_PSM.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 26-Agosto-2010
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
                    Cmb_Estatus_Puesto.SelectedIndex = 0;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Cmb_Estatus_Puesto.Enabled = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                    Configuracion_Acceso("Frm_Cat_Puestos.aspx");
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Cmb_Estatus_Puesto.SelectedIndex = 0;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Cmb_Estatus_Puesto.Enabled = false;
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
                    Cmb_Estatus_Puesto.Enabled = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    break;
            }
            Txt_Nombre_Puesto.Enabled = Habilitado;
            Cmb_Plaza.Enabled = Habilitado;
            Txt_Comentarios_Puesto.Enabled = Habilitado;
            Txt_Busqueda_Puesto.Enabled = !Habilitado;
            Btn_Buscar_Puesto.Enabled = !Habilitado;
            Txt_Salario_Mensual.Enabled = Habilitado;
            Cmb_Perfil_Puesto.Enabled = Habilitado;
            Btn_Agregar_Perfil.Enabled = Habilitado;
            Grid_Puesto.Enabled = !Habilitado;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Grid_Perfiles.Enabled = Habilitado;
            Cmb_Aplica_Fondo_Retiro.Enabled = Habilitado;
            Cmb_Aplica_PSM.Enabled = Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Combo_Perfiles
    /// DESCRIPCION : Consulta los perfiles que estan dadas de alta en la base de datos
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 26-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Combo_Perfiles()
    {
        DataTable Dt_Perfiles; //Variable que obtendra los datos de la consulta
        Cls_Cat_Puestos_Negocio Rs_Consulta_Cat_Tra_Perfiles = new Cls_Cat_Puestos_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {
            Rs_Consulta_Cat_Tra_Perfiles.P_Tipo_DataTable = "PERFILES";
            Dt_Perfiles = Rs_Consulta_Cat_Tra_Perfiles.Consulta_DataTable(); //Consulta todos los perfiles que estan dadas de alta en la BD
            Cmb_Perfil_Puesto.DataSource = Dt_Perfiles;
            Cmb_Perfil_Puesto.DataValueField = "PERFIL_ID";
            Cmb_Perfil_Puesto.DataTextField = "NOMBRE";
            Cmb_Perfil_Puesto.DataBind();
            Cmb_Perfil_Puesto.Items.Insert(0, new ListItem("<-- SELECCIONE -->", "SELECCIONE"));
            Cmb_Perfil_Puesto.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Combo_Perfiles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Combo_Plazas
    /// DESCRIPCION : Consulta las plazas que estan dadas de alta en la base de datos
    /// PARAMETROS  : 
    /// CREO        : Francisco Antonio Gallardo Castañeda
    /// FECHA_CREO  : 04-Abril-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Combo_Plazas()
    {
        DataTable Dt_Plazas; //Variable que obtendra los datos de la consulta
        Cls_Cat_Puestos_Negocio Rs_Consulta_Cat_Tra_Plazas = new Cls_Cat_Puestos_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {
            Rs_Consulta_Cat_Tra_Plazas.P_Tipo_DataTable = "PLAZAS";
            Dt_Plazas = Rs_Consulta_Cat_Tra_Plazas.Consulta_DataTable(); //Consulta todos los perfiles que estan dadas de alta en la BD
            Cmb_Plaza.DataSource = Dt_Plazas;
            Cmb_Plaza.DataValueField = "PLAZA_ID";
            Cmb_Plaza.DataTextField = "NOMBRE";
            Cmb_Plaza.DataBind();
            Cmb_Plaza.Items.Insert(0, new ListItem("<-- SELECCIONE -->", "SELECCIONE"));
            Cmb_Plaza.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Combo_Plazas " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Puestos
    /// DESCRIPCION : Consulta los puestos que estan dadas de alta en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 26-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Puestos()
    {
        Cls_Cat_Puestos_Negocio Rs_Consulta_Cat_Puestos = new Cls_Cat_Puestos_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Puestos; //Variable que obtendra los datos de la consulta  
        try
        {
            if (Txt_Busqueda_Puesto.Text.Trim() != "")
            {
                Rs_Consulta_Cat_Puestos.P_Nombre = Txt_Busqueda_Puesto.Text.Trim();
            }
            Rs_Consulta_Cat_Puestos.P_Tipo_DataTable = "PUESTOS";
            Dt_Puestos = Rs_Consulta_Cat_Puestos.Consulta_DataTable(); //Consulta todos los puestos con sus datos generales
            Session["Dt_Puestos"] = Dt_Puestos;
            Llena_Grid_Puestos(0);
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Puestos " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llena_Grid_Puestos
    /// DESCRIPCION : Llena el grid con los puestos que se encuentran en la base de datos
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 26-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llena_Grid_Puestos(Int32 Pagina)
    {
        try
        {
            if (Session["Dt_Puestos"] != null)
            {
                Grid_Puesto.DataSource = (DataTable)Session["Dt_Puestos"];
                Grid_Puesto.PageIndex = Pagina;
            }
            else
            {
                Grid_Puesto.DataSource = new DataTable();
            }
            Grid_Puesto.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Puestos " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Puesto
    /// DESCRIPCION : Da de Alta el Puesto con los datos proporcionados por el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 26-Agosto-2010
    /// MODIFICO          : Francisco Antonio Gallardo Castañeda.
    /// FECHA_MODIFICO    : 08-Octubre-2010
    /// CAUSA_MODIFICACION: Se le agrego la parte de Perfiles y la parte de Salario Mensual
    ///*******************************************************************************
    private void Alta_Puesto()
    {
        Cls_Cat_Puestos_Negocio Rs_Alta_Cat_Puestos = new Cls_Cat_Puestos_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            Rs_Alta_Cat_Puestos.P_Nombre = Txt_Nombre_Puesto.Text.Trim();
            Rs_Alta_Cat_Puestos.P_Estatus = Cmb_Estatus_Puesto.SelectedValue;
            Rs_Alta_Cat_Puestos.P_Comentarios = Txt_Comentarios_Puesto.Text.Trim();
            Rs_Alta_Cat_Puestos.P_Plaza_ID = Cmb_Plaza.SelectedItem.Value;
            Rs_Alta_Cat_Puestos.P_Salario_Mensual = Convert.ToDouble(Txt_Salario_Mensual.Text);
            Rs_Alta_Cat_Puestos.P_Aplica_Fondo_Retiro = Cmb_Aplica_Fondo_Retiro.SelectedItem.Text.Trim();
            Rs_Alta_Cat_Puestos.P_Aplica_Psm = Cmb_Aplica_PSM.SelectedItem.Text.Trim();

            if (Session["Dt_Perfiles"] != null)
            {
                Rs_Alta_Cat_Puestos.P_Perfiles = (DataTable)Session["Dt_Perfiles"];
            }
            Rs_Alta_Cat_Puestos.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            Rs_Alta_Cat_Puestos.Alta_Puesto(); //Da de alta los datos del puesto proporcionados por el usuario en la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Puestos", "alert('El Alta del Puesto fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Puesto " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Puesto
    /// DESCRIPCION : Modifica los datos del puesto con los proporcionados por el usuario en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 26-Agosto-2010
    /// MODIFICO          : Francisco Antonio Gallardo Castañeda.
    /// FECHA_MODIFICO    : 08-Octubre-2010
    /// CAUSA_MODIFICACION: Se le agrego la parte de Perfiles y la parte de Salario Mensual
    ///*******************************************************************************
    private void Modificar_Puesto()
    {
        Cls_Cat_Puestos_Negocio Rs_Modificar_Cat_Puestos = new Cls_Cat_Puestos_Negocio(); //Variable de conexión hacia la capa de Negoccios para envio de datos a modificar
        try
        {
            Rs_Modificar_Cat_Puestos.P_Puesto_ID = Txt_Puesto_ID.Text;
            Rs_Modificar_Cat_Puestos.P_Nombre = Txt_Nombre_Puesto.Text;
            Rs_Modificar_Cat_Puestos.P_Estatus = Cmb_Estatus_Puesto.SelectedValue;
            Rs_Modificar_Cat_Puestos.P_Comentarios = Txt_Comentarios_Puesto.Text;
            Rs_Modificar_Cat_Puestos.P_Plaza_ID = Cmb_Plaza.SelectedItem.Value;
            Rs_Modificar_Cat_Puestos.P_Salario_Mensual = Convert.ToDouble(Txt_Salario_Mensual.Text);
            Rs_Modificar_Cat_Puestos.P_Aplica_Fondo_Retiro = Cmb_Aplica_Fondo_Retiro.SelectedItem.Text.Trim();
            Rs_Modificar_Cat_Puestos.P_Aplica_Psm = Cmb_Aplica_PSM.SelectedItem.Text.Trim();

            if (Session["Dt_Perfiles"] != null)
            {
                Rs_Modificar_Cat_Puestos.P_Perfiles = (DataTable)Session["Dt_Perfiles"];
            }
            Rs_Modificar_Cat_Puestos.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            Rs_Modificar_Cat_Puestos.Modificar_Puesto(); //Sustituye los datos que se encuentran en la BD por lo que introdujo el usuario
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Puestos", "alert('La Modificación del Puesto fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Puesto " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Puesto
    /// DESCRIPCION : Elimina los datos del Puesto que fue seleccionado por el Usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 26-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Puesto()
    {
        Cls_Cat_Puestos_Negocio Rs_Eliminar_Cat_Puestos = new Cls_Cat_Puestos_Negocio(); //Variable de conexión hacia la capa de Negocios para la eliminación de los datos
        try
        {
            Rs_Eliminar_Cat_Puestos.P_Puesto_ID = Txt_Puesto_ID.Text;
            Rs_Eliminar_Cat_Puestos.Eliminar_Puesto(); //Elimina el puesto que selecciono el usuario de la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Puestos", "alert('La Eliminación del Puesto fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Eliminar_Puesto " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Perfiles
    ///DESCRIPCIÓN: Llena el Grid de Pefiles
    ///PARAMETROS:  
    ///             1.  Pagina.     Pagina en que se mostrará el Grid.
    ///             2.  Tabla.      Datatable con que se llenará el Grid.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 08/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Perfiles(Int32 Pagina, DataTable Tabla)
    {
        Grid_Perfiles.SelectedIndex = (-1);
        Grid_Perfiles.DataSource = Tabla;
        Grid_Perfiles.PageIndex = Pagina;
        Grid_Perfiles.DataBind();
        Session["Dt_Perfiles"] = Tabla;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Buscar_Repetido_En_DataTable
    ///DESCRIPCIÓN: Busca un elemento en un DataTable
    ///PARAMETROS:  
    ///             1.  ID_Elemento.Identificador del elemento que se esta buscando.
    ///             2.  Tabla.      Datatable donde se va a buscar el Elemento.
    ///             3.  Columna.    Columna en la que se buscara el identificador del
    ///                             elemento que se desea agregar.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Buscar_Repetido_En_DataTable(Object ID_Elemento, DataTable Tabla, Int32 Columna)
    {
        Boolean Encontrada = false;
        if (Tabla != null && Tabla.Rows.Count > 0)
        {
            for (int cnt = 0; cnt < Tabla.Rows.Count; cnt++)
            {
                if (Tabla.Rows[cnt][Columna].ToString().Equals(ID_Elemento))
                {
                    Encontrada = true;
                    break;
                }
            }
        }
        return Encontrada;
    }
    #endregion

    #region (Grid)

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Puesto_SelectedIndexChanged
    /// DESCRIPCION : Consulta los datos del puesto que selecciono el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 26-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Puesto_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Puestos_Negocio Rs_Consulta_Cat_Puestos = new Cls_Cat_Puestos_Negocio(); //Variable de conexión a la capa de Negocios para la consulta de los datos del puesto
        Cls_Cat_Puestos_Negocio INF_PUESTO = null;

        try
        {
            INF_PUESTO = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Puestos(Grid_Puesto.SelectedRow.Cells[1].Text);

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Rs_Consulta_Cat_Puestos.P_Puesto_ID = Grid_Puesto.SelectedRow.Cells[1].Text;
            Rs_Consulta_Cat_Puestos = Rs_Consulta_Cat_Puestos.Consulta_Datos_Puestos(); //Consulta los datos del puesto que fue seleccionado por el usuario
            Txt_Puesto_ID.Text = Rs_Consulta_Cat_Puestos.P_Puesto_ID;
            Txt_Nombre_Puesto.Text = Rs_Consulta_Cat_Puestos.P_Nombre;
            Txt_Salario_Mensual.Text = Rs_Consulta_Cat_Puestos.P_Salario_Mensual.ToString("#,###,###.00");
            Txt_Comentarios_Puesto.Text = Rs_Consulta_Cat_Puestos.P_Comentarios;
            Cmb_Estatus_Puesto.SelectedIndex = Cmb_Estatus_Puesto.Items.IndexOf(Cmb_Estatus_Puesto.Items.FindByValue(Rs_Consulta_Cat_Puestos.P_Estatus));
            Cmb_Plaza.SelectedIndex = Cmb_Plaza.Items.IndexOf(Cmb_Plaza.Items.FindByValue(Rs_Consulta_Cat_Puestos.P_Plaza_ID));
            Llenar_Grid_Perfiles(0, Rs_Consulta_Cat_Puestos.P_Perfiles);

            Cmb_Aplica_Fondo_Retiro.SelectedIndex =
                Cmb_Aplica_Fondo_Retiro.Items.IndexOf(
                    Cmb_Aplica_Fondo_Retiro.Items.FindByText(INF_PUESTO.P_Aplica_Fondo_Retiro));

            Cmb_Aplica_PSM.SelectedIndex =
                Cmb_Aplica_PSM.Items.IndexOf(Cmb_Aplica_PSM.Items.FindByText(INF_PUESTO.P_Aplica_Psm));
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Puesto_PageIndexChanging
    ///DESCRIPCIÓN: Maneja el evento de la Paginacion en el Grid de Puestos.
    ///PARAMETROS:  
    ///CREO: Yazmin Delgado
    ///FECHA_CREO: 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************    
    protected void Grid_Puesto_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpia_Controles(); //Limpia todos los controles de la forma
            //Grid_Puesto.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            //Consulta_Puestos();//Carga los puestos que estan asignadas a la página seleccionada
            Llena_Grid_Puestos(e.NewPageIndex);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Perfiles_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del Grid de Perfiles.
    ///PARAMETROS:  
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 08/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Perfiles_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (Session["Dt_Perfiles"] != null)
        {
            Llenar_Grid_Perfiles(e.NewPageIndex, (DataTable)Session["Dt_Perfiles"]);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Perfil_RowDataBound
    ///DESCRIPCIÓN: Es el evento previo antes cargar el grid con informacion
    ///PARAMETROS:  
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 08/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Perfil_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                ((ImageButton)e.Row.Cells[0].FindControl("Btn_Eliminar_Perfil")).CommandArgument = e.Row.Cells[1].Text.Trim();
                ((ImageButton)e.Row.Cells[0].FindControl("Btn_Eliminar_Perfil")).ToolTip = "Quitar el Perfil " + e.Row.Cells[2].Text;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    #endregion

    #region (Eventos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Puesto_Click
    ///DESCRIPCIÓN: Maneja el evento del Boton de Buscar.
    ///PARAMETROS:  
    ///CREO: Yazmin Delgado
    ///FECHA_CREO: 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************        
    protected void Btn_Buscar_Puesto_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Consulta_Puestos(); //Consulta los puestos que coincidan con el nombre porporcionado por el usuario
            Limpia_Controles(); //Limpia los controles de la forma
            //Si no se encontraron puestos con un nombre similar al proporcionado por el usuario entonces manda un mensaje al usuario
            if (Grid_Puesto.Rows.Count <= 0)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encontraron Puestos con el nombre proporcionado <br>";
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Maneja el evento del Boton de Nuevo.
    ///PARAMETROS:  
    ///CREO: Yazmin Delgado
    ///FECHA_CREO: 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                Limpia_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
            }
            else
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces da de alta los mismo en la base de datos
                if (Txt_Nombre_Puesto.Text != "" && Txt_Salario_Mensual.Text != "" && Txt_Comentarios_Puesto.Text.Length <= 250 && Cmb_Plaza.SelectedIndex > 0)
                {
                    Alta_Puesto(); //Da de alta los datos proporcionados por el usuario
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                    if (Txt_Nombre_Puesto.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Nombre del Puesto <br>";
                    }
                    if (Cmb_Plaza.SelectedIndex == 0)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Debe seleccionar la Plaza a la que pertenece el puesto <br>";
                    }
                    if (Txt_Salario_Mensual.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Salario Mensual del Puesto <br>";
                    }
                    if (Txt_Comentarios_Puesto.Text.Length > 250)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Los comentarios proporcionados no deben ser mayor a 250 caracteres <br>";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Maneja el evento del Boton de Modificar.
    ///PARAMETROS:  
    ///CREO: Yazmin Delgado
    ///FECHA_CREO: 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                if (Txt_Puesto_ID.Text != "")
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el Puesto que desea modificar sus datos <br>";
                }
            }
            else
            {
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces modifica estos en la BD
                if (Txt_Nombre_Puesto.Text != "" && Txt_Salario_Mensual.Text != "" && Txt_Comentarios_Puesto.Text.Length <= 250)
                {
                    Modificar_Puesto(); //Modifica los datos del Puesto con los datos proporcionados por el usuario
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                    if (Txt_Nombre_Puesto.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Nombre del Puesto <br>";
                    }
                    if (Txt_Salario_Mensual.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Salario Mensual del Puesto <br>";
                    }
                    if (Txt_Comentarios_Puesto.Text.Length > 250)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Los comentarios proporcionados no deben ser mayor a 250 caracteres <br>";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Maneja el evento del Boton de Eliminar.
    ///PARAMETROS:  
    ///CREO: Yazmin Delgado
    ///FECHA_CREO: 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            //Si el usuario selecciono un Puesto entonces la elimina de la base de datos
            if (Txt_Puesto_ID.Text != "")
            {
                Eliminar_Puesto(); //Elimina el Puesto que fue seleccionada por el usuario
            }
            //Si el usuario no selecciono algun Puesto manda un mensaje indicando que es necesario que seleccione alguna para
            //poder eliminar
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione el Puesto que desea eliminar <br>";
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Maneja el evento del Boton de Salir y Cancelar.
    ///PARAMETROS:  
    ///CREO: Yazmin Delgado
    ///FECHA_CREO: 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Salir")
            {
                Session.Remove("Consulta_Puestos");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Inicializa_Controles();//Habilita los controles para la siguiente operación del usuario en el catálogo
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Perfil_Click
    ///DESCRIPCIÓN: Agrega un perfil nuevo, validando que no este ya agregado
    ///PARAMETROS:  
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 08/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Perfil_Click(object sender, EventArgs e)
    {
        if (Cmb_Perfil_Puesto.SelectedIndex > 0)
        {
            DataTable Tabla;
            if (Session["Dt_Perfiles"] == null)
            {
                Tabla = new DataTable("Perfiles");
                Tabla.Columns.Add("PERFIL_ID", Type.GetType("System.String"));
                Tabla.Columns.Add("NOMBRE", Type.GetType("System.String"));
            }
            else
            {
                Tabla = (DataTable)Session["Dt_Perfiles"];
            }
            if (!Buscar_Repetido_En_DataTable(Cmb_Perfil_Puesto.SelectedItem.Value, Tabla, 0))
            {
                DataRow Fila = Tabla.NewRow();
                Fila["PERFIL_ID"] = Cmb_Perfil_Puesto.SelectedItem.Value;
                Fila["NOMBRE"] = Cmb_Perfil_Puesto.SelectedItem.Text;
                Tabla.Rows.Add(Fila);
                Session["Dt_Perfiles"] = Tabla;
                Llenar_Grid_Perfiles(Grid_Perfiles.PageIndex, Tabla);
            }
            else
            {
                Lbl_Mensaje_Error.Text = "El Perfil ya se encuentra agregado";
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        else
        {
            Lbl_Mensaje_Error.Text = "Para Agregar un Perfil es necesario seleccionarlo del Combo";
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Perfil_Click
    ///DESCRIPCIÓN: Quita un perfil para este puesto
    ///PARAMETROS:  
    ///CREO: Juan Alberto Hernandez Negrete
    ///FECHA_CREO: 17/Noviembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Perfil_Click(object sender, EventArgs e)
    {
        DataRow[] Renglones;
        DataRow Renglon;
        ImageButton Btn_Eliminar_Perfiles = (ImageButton)sender;

        if (Session["Dt_Perfiles"] != null)
        {
            Renglones = ((DataTable)Session["Dt_Perfiles"]).Select("PERFIL_ID='" + Btn_Eliminar_Perfiles.CommandArgument + "'");

            if (Renglones.Length > 0)
            {
                Renglon = Renglones[0];
                DataTable Tabla = (DataTable)Session["Dt_Perfiles"];
                Tabla.Rows.Remove(Renglon);
                Session["Dt_Perfiles"] = Tabla;
                Grid_Perfiles.SelectedIndex = (-1);
                Llenar_Grid_Perfiles(Grid_Perfiles.PageIndex, Tabla);
            }
        }
        else
        {
            Lbl_Mensaje_Error.Text = "Se debe seleccionar de la tabla el Perfil a quitar";
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
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
            Botones.Add(Btn_Buscar_Puesto);

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
}
