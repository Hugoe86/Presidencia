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
using Presidencia.Empleados_No_Nominales.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Text.RegularExpressions;
using Presidencia.Roles.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Areas.Negocios;

public partial class paginas_Empleados_Frm_Cat_Nom_Empleados_No_Nominales : System.Web.UI.Page
{
    #region (Page Load)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Page_Load
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Inicializa_Controles();
            }

            Txt_Password_Empleado.Attributes.Add("value", Txt_Password_Empleado.Text);
            Txt_Confirma_Password_Empleado.Attributes.Add("value", Txt_Confirma_Password_Empleado.Text);

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion

    #region (Metodos)

    #region (Metodos Generales)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Limpia_Controles();
            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            Consultar_Roles();
            Consultar_SAP_Unidades_Responsables();
            Consulta_Empleados();

            Consultar_Dependencias_Busqueda();
            Consultar_Roles_Busqueda();
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
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {
            Txt_Empleado_Confronto.Text = string.Empty;
            Txt_Empleado_ID.Text = "";
            Txt_No_Empleado.Text = "";
            Cmb_Estatus_Empleado.SelectedIndex = 1;
            Txt_Nombre_Empleado.Text = "";
            Txt_Apellido_Paterno_Empleado.Text = "";
            Txt_Apellido_Materno_Empleado.Text = "";
            Txt_Password_Empleado.Text = "";
            Txt_Confirma_Password_Empleado.Text = "";
            Txt_Password_Empleado.Attributes.Add("value", "");
            Txt_Confirma_Password_Empleado.Attributes.Add("value", "");
            Txt_Comentarios_Empleado.Text = "";
            Txt_Fecha_Nacimiento_Empleado.Text = "";
            Cmb_Sexo_Empleado.SelectedIndex = 0;
            Txt_RFC_Empleado.Text = "";
            Txt_CURP_Empleado.Text = "";
            Txt_Domicilio_Empleado.Text = "";
            Txt_Colonia_Empleado.Text = "";
            Txt_Codigo_Postal_Empleado.Text = "";
            Txt_Ciudad_Empleado.Text = "";
            Txt_Estado_Empleado.Text = "";

            Cmb_Roles_Empleado.SelectedIndex = -1;
            Cmb_Sexo_Empleado.SelectedIndex = -1;
            Cmb_Areas_Empleado.SelectedIndex = -1;
            Cmb_SAP_Unidad_Responsable.SelectedIndex = -1;
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
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
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
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Salir";
                    Cmb_Estatus_Empleado.Enabled = Habilitado;
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
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
                    Cmb_Estatus_Empleado.Enabled = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    break;
                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Cmb_Estatus_Empleado.Enabled = true;
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    break;
            }

            Txt_No_Empleado.Enabled = Habilitado;
            Txt_Nombre_Empleado.Enabled = Habilitado;
            Txt_Apellido_Paterno_Empleado.Enabled = Habilitado;
            Txt_Apellido_Materno_Empleado.Enabled = Habilitado;
            Txt_Password_Empleado.Enabled = Habilitado;
            Txt_Confirma_Password_Empleado.Enabled = Habilitado;
            Txt_Comentarios_Empleado.Enabled = Habilitado;
            Txt_Fecha_Nacimiento_Empleado.Enabled = Habilitado;
            Cmb_Sexo_Empleado.Enabled = Habilitado;
            Txt_RFC_Empleado.Enabled = Habilitado;
            Txt_CURP_Empleado.Enabled = Habilitado;
            Txt_Domicilio_Empleado.Enabled = Habilitado;
            Txt_Colonia_Empleado.Enabled = Habilitado;
            Txt_Codigo_Postal_Empleado.Enabled = Habilitado;
            Txt_Ciudad_Empleado.Enabled = Habilitado;
            Txt_Estado_Empleado.Enabled = Habilitado;
            Cmb_Estatus_Empleado.Enabled = false;
            Grid_Empleados.Enabled = !Habilitado;

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            Cmb_Areas_Empleado.Enabled = false;
            Cmb_Roles_Empleado.Enabled = Habilitado;
            Cmb_SAP_Unidad_Responsable.Enabled = Habilitado;
            Btn_Fecha_Nacimiento.Enabled = Habilitado;
            Btn_Mostrar_Popup_Busqueda.Enabled = !Habilitado;
            Txt_Empleado_Confronto.Enabled = Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Agregar_Tooltip_Combos
    /// DESCRIPCION : Agregar tooltip al combo que es pasado como parametro.
    /// PARAMETROS  : _DropDownList es el combo al cual se le agregara el tooltip
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 26/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Agregar_Tooltip_Combos(DropDownList Cmb_Combo)
    {
        for (int i = 0; i <= Cmb_Combo.Items.Count - 1; i++)
        {
            Cmb_Combo.Items[i].Attributes.Add("Title", Cmb_Combo.Items[i].Text);
        }
    }
    /// ********************************************************************************
    /// Nombre: Crear_Tabla_Mostrar_Errores_Pagina
    /// Descripción: Crea la tabla que almacenara que datos son requeridos 
    /// por el sistema
    /// Creo: Juan Alberto Hernández Negrete 
    /// Fecha Creo: 20/Octubre/2010
    /// Modifico:
    /// Fecha Modifico:
    /// Causa Modifico:
    /// ********************************************************************************
    private String Crear_Tabla_Mostrar_Errores_Pagina(String Errores)
    {
        String Tabla_Inicio = "<table style='width:100%px;font-size:10px;color:red;text-align:left;'>";
        String Tabla_Cierra = "</table>";
        String Fila_Inicia = "<tr>";
        String Fila_Cierra = "</tr>";
        String Celda_Inicia = "<td style='width:25%;text-align:left;vertical-align:top;font-size:10px;' " +
                                "onmouseover=this.style.background='#DFE8F6';this.style.color='#000000'" +
                                " onmouseout=this.style.background='#ffffff';this.style.color='red'>";
        String Celda_Cierra = "</td>";
        char[] Separador = { '+' };
        String[] _Errores_Temp = Errores.Replace("<br>", "").Split(Separador);
        String[] _Errores = new String[(_Errores_Temp.Length - 1)];
        String Tabla;
        String Filas = "";
        String Celdas = "";
        int Contador_Celdas = 1;
        for (int i = 0; i < _Errores.Length; i++) _Errores[i] = _Errores_Temp[i + 1];

        Tabla = Tabla_Inicio;
        for (int i = 0; i < _Errores.Length; i++)
        {
            if (Contador_Celdas == 5)
            {
                Filas += Fila_Inicia;
                Filas += Celdas;
                Filas += Fila_Cierra;
                Celdas = "";
                Contador_Celdas = 0;
                i = i - 1;
            }
            else
            {
                Celdas += Celda_Inicia;
                Celdas += "<b style='font-size:12px;'>+</b>" + _Errores[i];
                Celdas += Celda_Cierra;
            }
            Contador_Celdas = Contador_Celdas + 1;
        }
        if (_Errores.Length < 5 || Contador_Celdas > 0)
        {
            Filas += Fila_Inicia;
            Filas += Celdas;
            Filas += Fila_Cierra;
        }
        Tabla += Filas;
        Tabla += Tabla_Cierra;
        return Tabla;
    }
    #endregion

    #region (Operacion [Alta - Modificar - Eliminar])
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Empleado
    /// DESCRIPCION : Da de Alta el Empleado con los datos proporcionados por el usuario
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :Juan Alberto Hernandez Negrete
    /// FECHA_MODIFICO    :3/Noviembre/2010
    /// CAUSA_MODIFICACION: Completar el Catalogo
    ///*******************************************************************************
    private void Alta_Empleado()
    {
        Cls_Cat_Nom_Empleados_No_Nominales_Negocio Rs_Alta_Cat_Empleados = new Cls_Cat_Nom_Empleados_No_Nominales_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta

        try
        {
            Rs_Alta_Cat_Empleados.P_No_Empleado = Convert.ToString(Txt_No_Empleado.Text);
            Rs_Alta_Cat_Empleados.P_Estatus = Cmb_Estatus_Empleado.SelectedValue;
            Rs_Alta_Cat_Empleados.P_Nombre = Convert.ToString(Txt_Nombre_Empleado.Text);
            Rs_Alta_Cat_Empleados.P_Apellido_Paterno = Convert.ToString(Txt_Apellido_Paterno_Empleado.Text);
            Rs_Alta_Cat_Empleados.P_Apelldo_Materno = Convert.ToString(Txt_Apellido_Materno_Empleado.Text);
            if (Cmb_Roles_Empleado.SelectedIndex > 0) Rs_Alta_Cat_Empleados.P_Rol_ID = Cmb_Roles_Empleado.SelectedValue.Trim();
            if (Txt_Password_Empleado.Text != "") Rs_Alta_Cat_Empleados.P_Password = Convert.ToString(Txt_Password_Empleado.Text);
            Rs_Alta_Cat_Empleados.P_Comentarios = Convert.ToString(Txt_Comentarios_Empleado.Text);
            Rs_Alta_Cat_Empleados.P_Fecha_Nacimiento = Convert.ToDateTime(Txt_Fecha_Nacimiento_Empleado.Text);
            Rs_Alta_Cat_Empleados.P_Sexo = Cmb_Sexo_Empleado.SelectedValue;
            if (Txt_RFC_Empleado.Text != "") Rs_Alta_Cat_Empleados.P_RFC = Convert.ToString(Txt_RFC_Empleado.Text);
            if (Txt_CURP_Empleado.Text != "") Rs_Alta_Cat_Empleados.P_CURP = Convert.ToString(Txt_CURP_Empleado.Text);
            Rs_Alta_Cat_Empleados.P_Calle = Convert.ToString(Txt_Domicilio_Empleado.Text);
            Rs_Alta_Cat_Empleados.P_Colonia = Convert.ToString(Txt_Colonia_Empleado.Text);
            if (Txt_Codigo_Postal_Empleado.Text != "") Rs_Alta_Cat_Empleados.P_Codigo_Postal = Convert.ToInt32(Txt_Codigo_Postal_Empleado.Text.ToString());
            Rs_Alta_Cat_Empleados.P_Ciudad = Convert.ToString(Txt_Ciudad_Empleado.Text);
            Rs_Alta_Cat_Empleados.P_Estado = Convert.ToString(Txt_Estado_Empleado.Text);
            Rs_Alta_Cat_Empleados.P_Area_ID = Cmb_Areas_Empleado.SelectedValue;
            Rs_Alta_Cat_Empleados.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            Rs_Alta_Cat_Empleados.P_Dependencia_ID = Cmb_SAP_Unidad_Responsable.SelectedValue.Trim();
            Rs_Alta_Cat_Empleados.Confronto = Txt_Empleado_Confronto.Text;

            Rs_Alta_Cat_Empleados.Alta_Empleado(); //Da de alta los datos del Empleado proporcionados por el usuario en la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ClientScript.RegisterStartupScript(this.GetType(), "Catalogo de Empleados", "alert('El Alta del Empleado fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Empleado " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Empleado
    /// DESCRIPCION : Modifica los datos del Empleado con los proporcionados por el usuario en la BD
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :Juan Alberto Hernandez Negrete
    /// FECHA_MODIFICO    :3/Noviembre/2010
    /// CAUSA_MODIFICACION: Completar el Catalogo
    ///*******************************************************************************
    private void Modificar_Empleado()
    {
        Cls_Cat_Nom_Empleados_No_Nominales_Negocio Rs_Modificar_Cat_Empleados = new Cls_Cat_Nom_Empleados_No_Nominales_Negocio();
        try
        {
            Rs_Modificar_Cat_Empleados.P_Empleado_ID = Txt_Empleado_ID.Text;
            Rs_Modificar_Cat_Empleados.P_No_Empleado = Convert.ToString(Txt_No_Empleado.Text);
            Rs_Modificar_Cat_Empleados.P_Estatus = Cmb_Estatus_Empleado.SelectedValue;
            Rs_Modificar_Cat_Empleados.P_Nombre = Convert.ToString(Txt_Nombre_Empleado.Text);
            Rs_Modificar_Cat_Empleados.P_Apellido_Paterno = Convert.ToString(Txt_Apellido_Paterno_Empleado.Text);
            Rs_Modificar_Cat_Empleados.P_Apelldo_Materno = Convert.ToString(Txt_Apellido_Materno_Empleado.Text);
            Rs_Modificar_Cat_Empleados.P_Password = Convert.ToString(Txt_Password_Empleado.Text);
            Rs_Modificar_Cat_Empleados.P_Rol_ID = Cmb_Roles_Empleado.SelectedValue.Trim();
            Rs_Modificar_Cat_Empleados.P_Comentarios = Convert.ToString(Txt_Comentarios_Empleado.Text);
            Rs_Modificar_Cat_Empleados.P_Fecha_Nacimiento = Convert.ToDateTime(Txt_Fecha_Nacimiento_Empleado.Text);
            Rs_Modificar_Cat_Empleados.P_Sexo = Cmb_Sexo_Empleado.SelectedValue;
            if (Txt_RFC_Empleado.Text != "") Rs_Modificar_Cat_Empleados.P_RFC = Convert.ToString(Txt_RFC_Empleado.Text);
            if (Txt_CURP_Empleado.Text != "") Rs_Modificar_Cat_Empleados.P_CURP = Convert.ToString(Txt_CURP_Empleado.Text);
            Rs_Modificar_Cat_Empleados.P_Calle = Convert.ToString(Txt_Domicilio_Empleado.Text);
            Rs_Modificar_Cat_Empleados.P_Colonia = Convert.ToString(Txt_Colonia_Empleado.Text);
            if (Txt_Codigo_Postal_Empleado.Text != "") Rs_Modificar_Cat_Empleados.P_Codigo_Postal = Convert.ToInt32(Txt_Codigo_Postal_Empleado.Text.ToString());
            Rs_Modificar_Cat_Empleados.P_Ciudad = Convert.ToString(Txt_Ciudad_Empleado.Text);
            Rs_Modificar_Cat_Empleados.P_Estado = Convert.ToString(Txt_Estado_Empleado.Text);
            Rs_Modificar_Cat_Empleados.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            Rs_Modificar_Cat_Empleados.P_Area_ID = Cmb_Areas_Empleado.SelectedValue;
            Rs_Modificar_Cat_Empleados.P_Dependencia_ID = Cmb_SAP_Unidad_Responsable.SelectedValue.Trim();
            Rs_Modificar_Cat_Empleados.Confronto = Txt_Empleado_Confronto.Text;

            Rs_Modificar_Cat_Empleados.Modificar_Empleado(); //Sustituye los datos que se encuentran en la BD por lo que introdujo el usuario
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ClientScript.RegisterStartupScript(this.GetType(), "Catalogo de Empleados", "alert('La Modificación del Empleado fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Empleado " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Empleado
    /// DESCRIPCION : Elimina los datos del Empleado que fue seleccionado por el Usuario
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 13-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Empleado()
    {
        Cls_Cat_Nom_Empleados_No_Nominales_Negocio Rs_Eliminar_Cat_Empleados = new Cls_Cat_Nom_Empleados_No_Nominales_Negocio(); //Variable de conexión hacia la capa de Negocios para la eliminación de los datos
        try
        {
            Rs_Eliminar_Cat_Empleados.P_Empleado_ID = Txt_Empleado_ID.Text;
            Rs_Eliminar_Cat_Empleados.Eliminar_Empleado(); //Elimina el Empleado que selecciono el usuario de la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
        }
        catch (Exception ex)
        {
            throw new Exception("Eliminar_Empleado " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region (Combos Pagina)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Roles
    /// DESCRIPCION : Consulta los Roles que estan dadas de alta en la DB
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Roles()
    {
        DataTable Dt_Roles;
        Cls_Apl_Cat_Roles_Business Rs_Consulta_Apl_Cat_Roles = new Cls_Apl_Cat_Roles_Business();

        try
        {
            Dt_Roles = Rs_Consulta_Apl_Cat_Roles.Llenar_Tbl_Roles();
            Cmb_Roles_Empleado.DataSource = Dt_Roles;
            Cmb_Roles_Empleado.DataValueField = "Rol_ID";
            Cmb_Roles_Empleado.DataTextField = "Nombre";
            Cmb_Roles_Empleado.DataBind();
            Cmb_Roles_Empleado.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Roles_Empleado.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Roles " + ex.Message.ToString(), ex);
        }
    }

    private void Consultar_SAP_Unidades_Responsables()
    {
        Cls_Cat_Dependencias_Negocio Obj_Unidades_Responsables = new Cls_Cat_Dependencias_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Unidades_Responsables = null;//Variable que lista las unidades responsables registrdas en sistema.

        try
        {
            Dt_Unidades_Responsables = Obj_Unidades_Responsables.Consulta_Dependencias();
            Cmb_SAP_Unidad_Responsable.DataSource = Dt_Unidades_Responsables;
            Cmb_SAP_Unidad_Responsable.DataTextField = Cat_Dependencias.Campo_Nombre;
            Cmb_SAP_Unidad_Responsable.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
            Cmb_SAP_Unidad_Responsable.DataBind();
            Cmb_SAP_Unidad_Responsable.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_SAP_Unidad_Responsable.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las unidades responsables registradas en sistema. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Areas_Dependencia
    /// DESCRIPCION : Consulta las áreas que tiene asignada la Dependencia
    /// PARAMETROS  : Dependencia_ID: Guarda el ID de la Dependencia a consultar sus áreas
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Areas_Dependencia(String Dependencia_ID)
    {
        DataTable Dt_Areas;
        Cls_Cat_Areas_Negocio Rs_Consulta_Cat_Areas = new Cls_Cat_Areas_Negocio();

        try
        {
            Rs_Consulta_Cat_Areas.P_Dependencia_ID = Dependencia_ID;
            Dt_Areas = Rs_Consulta_Cat_Areas.Consulta_Areas();
            Cmb_Areas_Empleado.DataSource = Dt_Areas;
            Cmb_Areas_Empleado.DataValueField = "Area_ID";
            Cmb_Areas_Empleado.DataTextField = "Nombre";
            Cmb_Areas_Empleado.DataBind();
            Cmb_Areas_Empleado.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Areas_Empleado.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Areas_Dependencia " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region (Combos Busqueda)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Dependencias_Busqueda
    /// DESCRIPCION : Consulta las Dependencias uy Roles que estan dadas de alta en la DB
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Dependencias_Busqueda()
    {
        DataTable Dt_Dependencias;
        Cls_Cat_Dependencias_Negocio Rs_Consulta_Cat_Dependencias = new Cls_Cat_Dependencias_Negocio();

        try
        {
            Dt_Dependencias = Rs_Consulta_Cat_Dependencias.Consulta_Dependencias();
            Cmb_Busqueda_Dependencia.DataSource = Dt_Dependencias;
            Cmb_Busqueda_Dependencia.DataValueField = "Dependencia_ID";
            Cmb_Busqueda_Dependencia.DataTextField = "Nombre";
            Cmb_Busqueda_Dependencia.DataBind();
            Cmb_Busqueda_Dependencia.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Dependencia.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Dependencias " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Roles
    /// DESCRIPCION : Consulta los Roles que estan dadas de alta en la DB
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Roles_Busqueda()
    {
        DataTable Dt_Roles;
        Cls_Apl_Cat_Roles_Business Rs_Consulta_Apl_Cat_Roles = new Cls_Apl_Cat_Roles_Business();

        try
        {
            Dt_Roles = Rs_Consulta_Apl_Cat_Roles.Llenar_Tbl_Roles();
            Cmb_Busqueda_Rol.DataSource = Dt_Roles;
            Cmb_Busqueda_Rol.DataValueField = "Rol_ID";
            Cmb_Busqueda_Rol.DataTextField = "Nombre";
            Cmb_Busqueda_Rol.DataBind();
            Cmb_Busqueda_Rol.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Rol.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Roles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Areas_Dependencia
    /// DESCRIPCION : Consulta las áreas que tiene asignada la Dependencia
    /// PARAMETROS  : Dependencia_ID: Guarda el ID de la Dependencia a consultar sus áreas
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Areas_Dependencia_Busqueda(String Dependencia_ID)
    {
        DataTable Dt_Areas;
        Cls_Cat_Areas_Negocio Rs_Consulta_Cat_Areas = new Cls_Cat_Areas_Negocio();

        try
        {
            Rs_Consulta_Cat_Areas.P_Dependencia_ID = Dependencia_ID;
            Dt_Areas = Rs_Consulta_Cat_Areas.Consulta_Areas();
            Cmb_Busqueda_Areas.DataSource = Dt_Areas;
            Cmb_Busqueda_Areas.DataValueField = "Area_ID";
            Cmb_Busqueda_Areas.DataTextField = "Nombre";
            Cmb_Busqueda_Areas.DataBind();
            Cmb_Busqueda_Areas.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Areas.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Areas_Dependencia " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region (Metodos Validacion)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_RFC
    /// DESCRIPCION : Valida el RFC Ingresado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public Boolean Validar_RFC()
    {
        string Patron_RFC = @"^[a-zA-Z]{3,4}(\d{6})((\D|\d){3})?$";

        if (Txt_RFC_Empleado.Text != null) return Regex.IsMatch(Txt_RFC_Empleado.Text, Patron_RFC);
        else return false;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Codigo_Postal
    /// DESCRIPCION : Valida el Codigo Postal Ingresado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public Boolean Validar_Codigo_Postal()
    {
        string Patron_CP = @"^([1-9]{2}|[0-9][1-9]|[1-9][0-9])[0-9]{3}$";

        if (Txt_Codigo_Postal_Empleado.Text != null) return Regex.IsMatch(Txt_Codigo_Postal_Empleado.Text, Patron_CP);
        else return false;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_CURP
    /// DESCRIPCION : Valida el Fax Ingresado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public Boolean Validar_CURP()
    {
        string Patron_Curp = @"^[a-zA-Z]{4}(\d{6})([a-zA-Z]{6})(\d{2})?$";

        if (Txt_CURP_Empleado.Text != null) return Regex.IsMatch(Txt_CURP_Empleado.Text, Patron_Curp);
        else return false;
    }
    /// ********************************************************************************
    /// Nombre: Validar_Datos
    /// Descripción: Validar Campos
    /// Creo: Juan Alberto Hernández Negrete 
    /// Fecha Creo: 20/Octubre/2010
    /// Modifico:
    /// Fecha Modifico:
    /// Causa Modifico:
    /// ********************************************************************************
    private Boolean Validar_Datos()
    {
        Boolean Datos_Validos = true;
        Lbl_Mensaje_Error.Text = "";

        ///-------------------------------  Datos Generales  --------------------------------------------
        if (Txt_No_Empleado.Text == "")
        {
            Lbl_Mensaje_Error.Text += "+ El No. de Empleado <br>";
            Datos_Validos = false;
        }

        if (Txt_Empleado_Confronto.Text == "")
        {
            Lbl_Mensaje_Error.Text += "+ Las iniciales del empleado es un dato requerido <br>";
            Datos_Validos = false;
        }

        if (Cmb_Estatus_Empleado.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "+ El Estatus del Empleado <br>";
            Datos_Validos = false;
        }
        if (Txt_Nombre_Empleado.Text == "")
        {
            Lbl_Mensaje_Error.Text += "+ El Nombre del Empleado <br>";
            Datos_Validos = false;
        }
        if (Cmb_Roles_Empleado.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "+ El Rol del Empleado <br>";
            Datos_Validos = false;
        }
        if (Txt_Apellido_Paterno_Empleado.Text == "")
        {
            Lbl_Mensaje_Error.Text += "+ El Apellido Paterno del Empleado <br>";
            Datos_Validos = false;
        }
        if (string.IsNullOrEmpty(Txt_Password_Empleado.Text))
        {
            Lbl_Mensaje_Error.Text += "+ Password del Empleado <br>";
            Datos_Validos = false;
        }
        if (string.IsNullOrEmpty(Txt_Confirma_Password_Empleado.Text))
        {
            Lbl_Mensaje_Error.Text += "+ Confirmacion del Password del Empleado <br>";
            Datos_Validos = false;
        }
        if (Txt_Comentarios_Empleado.Text.Length > 250)
        {
            Lbl_Mensaje_Error.Text += "+ Los comentarios proporcionados no deben ser mayor a 250 caracteres <br>";
            Datos_Validos = false;
        }
        ///---------------------------------------  Datos Personales  -----------------------------------------------
        if (string.IsNullOrEmpty(Txt_Fecha_Nacimiento_Empleado.Text) || (Txt_Fecha_Nacimiento_Empleado.Text.Trim().Equals("__/___/____")))
        {
            Lbl_Mensaje_Error.Text += "+ La Fecha de Nacimiento del Empleado <br>";
            Datos_Validos = false;
        }
        else if (!Validar_Formato_Fecha(Txt_Fecha_Nacimiento_Empleado.Text.Trim()))
        {
            Txt_Fecha_Nacimiento_Empleado.Text = "";
            Lbl_Mensaje_Error.Text += "+ Formato de Fecha de Nacimiento Incorrecto <br>";
            Datos_Validos = false;
        }

        if (Cmb_Sexo_Empleado.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += "+ El Sexo del Empleado <br>";
            Datos_Validos = false;
        }

        if (Txt_Domicilio_Empleado.Text == "")
        {
            Lbl_Mensaje_Error.Text += "+ El Domicilio del Empleado <br>";
            Datos_Validos = false;
        }
        if (Txt_Colonia_Empleado.Text == "")
        {
            Lbl_Mensaje_Error.Text += "+ La Colonia del Empleado <br>";
            Datos_Validos = false;
        }
        if (!string.IsNullOrEmpty(Txt_Codigo_Postal_Empleado.Text))
        {
            if (!Validar_Codigo_Postal())
            {
                Lbl_Mensaje_Error.Text += "+ Formato del Codigo Postal Incorrecto <br>";
                Datos_Validos = false;
            }
        }
        if (Txt_Ciudad_Empleado.Text == "")
        {
            Lbl_Mensaje_Error.Text += "+ La Ciudad del Empleado <br>";
            Datos_Validos = false;
        }
        if (Txt_Estado_Empleado.Text == "")
        {
            Lbl_Mensaje_Error.Text += "+ El Estado del Empleado <br>";
            Datos_Validos = false;
        }

        if (Cmb_SAP_Unidad_Responsable.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "+ La Unidad Responsable a la cual pertenece el Empleado <br>";
            Datos_Validos = false;
        }

        if (Cmb_Areas_Empleado.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "+ El Área a la cual pertenece el Empleado <br>";
            Datos_Validos = false;
        }

        Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("Es necesario Introducir: <br>" + Crear_Tabla_Mostrar_Errores_Pagina(Lbl_Mensaje_Error.Text));
        return Datos_Validos;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Formato_Fecha
    /// DESCRIPCION : Valida el formato de las fechas.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Formato_Fecha(String Fecha)
    {
        String Cadena_Fecha = @"^(([0-9])|([0-2][0-9])|([3][0-1]))\/(ene|feb|mar|abr|may|jun|jul|ago|sep|oct|nov|dic)\/\d{4}$";
        if (Fecha != null)
        {
            return Regex.IsMatch(Fecha, Cadena_Fecha);
        }
        else
        {
            return false;
        }
    }
    /// ********************************************************************************
    /// Nombre: Validar_Fechas
    /// Descripción: Valida que la Fecha Inicial no sea mayor que la Final
    /// Creo: Juan Alberto Hernández Negrete 
    /// Fecha Creo: 20/Octubre/2010
    /// Modifico:
    /// Fecha Modifico:
    /// Causa Modifico:
    /// ********************************************************************************
    private Boolean Validar_Fechas(String _Fecha_Inicio, String _Fecha_Fin)
    {
        DateTime Fecha_Inicio = Convert.ToDateTime(_Fecha_Inicio);
        DateTime Fecha_Fin = Convert.ToDateTime(_Fecha_Fin);
        Boolean Fecha_Valida = false;
        if (Fecha_Inicio < Fecha_Fin) Fecha_Valida = true;
        return Fecha_Valida;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 18/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean IsNumeric(String Cadena)
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

    #endregion

    #region (Grid)

    #region (Grid Empleados)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Empleados
    /// DESCRIPCION : Consulta los Empleados que estan dadas de alta en la BD
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Empleados()
    {
        Cls_Cat_Nom_Empleados_No_Nominales_Negocio Rs_Consulta_Ca_Empleados = new Cls_Cat_Nom_Empleados_No_Nominales_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Empleados; //Variable que obtendra los datos de la consulta 

        try
        {
            if (!string.IsNullOrEmpty(Txt_Empleado_ID.Text.Trim()))
            {
                Rs_Consulta_Ca_Empleados.P_Empleado_ID = Txt_Empleado_ID.Text.Trim();
            }
            Dt_Empleados = Rs_Consulta_Ca_Empleados.Consulta_Empleados_General().P_Dt_Empleados; //Consulta todos los Empleados que coindican con lo proporcionado por el usuario
            Llena_Grid_Empleados(Dt_Empleados);
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Empleados " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Empleados_SelectedIndexChanged
    /// DESCRIPCION : Consulta los datos del Empleado que selecciono el usuario
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 12-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Empleados_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Nom_Empleados_No_Nominales_Negocio Rs_Consulta_Cat_Empleados = new Cls_Cat_Nom_Empleados_No_Nominales_Negocio(); //Variable de conexión a la capa de Negocios para la consulta de los datos del empleado
        Cls_Cat_Nom_Empleados_No_Nominales_Negocio INF_EMPLEADO = null;

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            Rs_Consulta_Cat_Empleados.P_Empleado_ID = Grid_Empleados.SelectedRow.Cells[1].Text.Trim();
            INF_EMPLEADO = Rs_Consulta_Cat_Empleados.Consulta_Empleados_General(); //Consulta los datos del empleado que fue seleccionado por el usuario

            Txt_Empleado_Confronto.Text = INF_EMPLEADO.Confronto;
            Txt_Empleado_ID.Text = INF_EMPLEADO.P_Empleado_ID;
            Txt_No_Empleado.Text = INF_EMPLEADO.P_No_Empleado;
            Cmb_Estatus_Empleado.SelectedValue = INF_EMPLEADO.P_Estatus;
            Txt_Nombre_Empleado.Text = INF_EMPLEADO.P_Nombre;
            Txt_Apellido_Paterno_Empleado.Text = INF_EMPLEADO.P_Apellido_Paterno;
            Txt_Apellido_Materno_Empleado.Text = INF_EMPLEADO.P_Apelldo_Materno;
            Cmb_Roles_Empleado.SelectedIndex =
                Cmb_Roles_Empleado.Items.IndexOf(Cmb_Roles_Empleado.Items.FindByValue(INF_EMPLEADO.P_Rol_ID));
            Txt_Fecha_Nacimiento_Empleado.Text =
                String.Format("{0:dd/MMM/yyyy}", INF_EMPLEADO.P_Fecha_Nacimiento);
            Cmb_Sexo_Empleado.SelectedIndex =
                Cmb_Sexo_Empleado.Items.IndexOf(Cmb_Sexo_Empleado.Items.FindByText(INF_EMPLEADO.P_Sexo));

            Txt_RFC_Empleado.Text = INF_EMPLEADO.P_RFC;
            Txt_CURP_Empleado.Text = INF_EMPLEADO.P_CURP;
            Txt_Domicilio_Empleado.Text = INF_EMPLEADO.P_Calle;
            Txt_Colonia_Empleado.Text = INF_EMPLEADO.P_Colonia;
            Txt_Codigo_Postal_Empleado.Text = INF_EMPLEADO.P_Codigo_Postal.ToString();
            Txt_Ciudad_Empleado.Text = INF_EMPLEADO.P_Ciudad;
            Txt_Estado_Empleado.Text = INF_EMPLEADO.P_Estado;
            Txt_Comentarios_Empleado.Text = INF_EMPLEADO.P_Comentarios;


            if (!string.IsNullOrEmpty(INF_EMPLEADO.P_Password))
            {
                Txt_Password_Empleado.Text = INF_EMPLEADO.P_Password;
                Txt_Password_Empleado.Attributes.Add("value", Txt_Password_Empleado.Text);

                Txt_Confirma_Password_Empleado.Text = INF_EMPLEADO.P_Password;
                Txt_Confirma_Password_Empleado.Attributes.Add("value", Txt_Confirma_Password_Empleado.Text);
            }

            if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Dependencia_ID))
            {
                Cmb_SAP_Unidad_Responsable.SelectedIndex = Cmb_SAP_Unidad_Responsable.Items.IndexOf(
                    Cmb_SAP_Unidad_Responsable.Items.FindByValue(INF_EMPLEADO.P_Dependencia_ID));

                Consulta_Areas_Dependencia(INF_EMPLEADO.P_Dependencia_ID);
                if (INF_EMPLEADO.P_Area_ID != null)
                    Cmb_Areas_Empleado.SelectedIndex = Cmb_Areas_Empleado.Items.IndexOf(
                        Cmb_Areas_Empleado.Items.FindByValue(INF_EMPLEADO.P_Area_ID));
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
    /// NOMBRE DE LA FUNCION: Grid_Empleados_PageIndexChanging
    /// DESCRIPCION : Cambia la pagina de la tabla de empleados
    ///               
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Empleados_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpia_Controles();                        //Limpia todos los controles de la forma
            Grid_Empleados.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Consulta_Empleados();                    //Carga los Empleados que estan asignados a la página seleccionada
            Grid_Empleados.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llena_Grid_Empleados
    /// DESCRIPCION : Llena el grid con los Empleados que fueron obtenidos de la consulta
    ///               Consulta_Empleados
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llena_Grid_Empleados(DataTable Dt_Empleados)
    {
        try
        {
            Grid_Empleados.Columns[1].Visible = true;
            Grid_Empleados.Columns[3].Visible = true;
            Grid_Empleados.Columns[5].Visible = true;
            Grid_Empleados.DataSource = Dt_Empleados;
            Grid_Empleados.DataBind();
            Grid_Empleados.Columns[1].Visible = false;
            Grid_Empleados.Columns[3].Visible = false;
            Grid_Empleados.Columns[5].Visible = false;
            Grid_Empleados.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Empleados " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #endregion

    #region (Eventos)

    #region (Operacion [Alta - Modificar - Eliminar - Consultar])
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Nuevo_Click
    /// DESCRIPCION : Alta de Empleado
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                Limpia_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
            }
            else
            {
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces da de alta los mismo en la base de datos
                if (Validar_Datos())
                {
                    if (Txt_Confirma_Password_Empleado.Text != "" || Txt_Password_Empleado.Text != "")
                    {
                        if (Txt_Password_Empleado.Text == "")
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                            Lbl_Mensaje_Error.Text += "+ El password del Empleado";
                            return;
                        }
                        if (Txt_Confirma_Password_Empleado.Text == "")
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                            Lbl_Mensaje_Error.Text += "+ La confirmación del password del Empleado";
                            return;
                        }
                        if (Txt_Password_Empleado.Text != Txt_Confirma_Password_Empleado.Text)
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "+ El password y su confirmación deben ser iguales, favor de verificar";
                            return;
                        }
                    }
                    if (Validar_Datos())
                    {
                        Alta_Empleado(); //Da de alta los datos proporcionados por el usuario
                        Limpia_Controles();    //Limpia los controles de la forma
                        Inicializa_Controles();
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                    }
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
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
    /// NOMBRE DE LA FUNCION: Btn_Modificar_Click
    /// DESCRIPCION : Modificar al Empleado Seleccionado
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (Btn_Modificar.ToolTip == "Modificar")
            {
                if (Txt_Empleado_ID.Text != "")
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el Empleado que desea modificar sus datos <br>";
                }
            }
            else
            {
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces modifica estos en la BD
                if (Validar_Datos())
                {
                    if (Txt_Confirma_Password_Empleado.Text != "" || Txt_Password_Empleado.Text != "")
                    {
                        if (Txt_Password_Empleado.Text == "")
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                            Lbl_Mensaje_Error.Text += "+ El password del Empleado";
                            return;
                        }
                        if (Txt_Confirma_Password_Empleado.Text == "")
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                            Lbl_Mensaje_Error.Text += "+ La confirmación del password del Empleado";
                            return;
                        }
                        if (Txt_Password_Empleado.Text != Txt_Confirma_Password_Empleado.Text)
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "El password y su confirmación deben ser iguales, favor de verificar";
                            return;
                        }
                    }
                    if (Validar_Datos())
                    {
                        Modificar_Empleado(); //Modifica los datos del Empleado con los datos proporcionados por el usuario   
                        Inicializa_Controles();
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                    }
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
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
    /// NOMBRE DE LA FUNCION: Btn_Eliminar_Click
    /// DESCRIPCION : Eliminar al Empleado Seleccionado
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (!String.IsNullOrEmpty(Txt_Empleado_ID.Text))
            {
                Eliminar_Empleado();
                Inicializa_Controles();
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione el Empleado que desea eliminar <br>";
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
    /// NOMBRE DE LA FUNCION: Btn_Salir_Click
    /// DESCRIPCION : Salir o Cancelar la Operacion Actual
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Salir")
            {
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

    protected void Btn_Busqueda_Empleados_Click(object sender, EventArgs e)
    {
        Cls_Cat_Nom_Empleados_No_Nominales_Negocio INF_EMPLEADOS = new Cls_Cat_Nom_Empleados_No_Nominales_Negocio();
        DataTable Dt_Empleados = null;

        try
        {
            if (!String.IsNullOrEmpty(Txt_Busqueda_No_Empleado.Text.Trim()))
                INF_EMPLEADOS.P_No_Empleado = Txt_Busqueda_No_Empleado.Text.Trim();

            if (!String.IsNullOrEmpty(Txt_Busqueda_RFC.Text.Trim()))
                INF_EMPLEADOS.P_RFC = Txt_Busqueda_RFC.Text.Trim();

            if (Cmb_Busqueda_Estatus.SelectedIndex > 0)
                INF_EMPLEADOS.P_Estatus = Cmb_Busqueda_Estatus.SelectedItem.Text.Trim();

            if (!String.IsNullOrEmpty(Txt_Busqueda_Nombre_Empleado.Text.Trim()))
                INF_EMPLEADOS.P_Nombre = Txt_Busqueda_Nombre_Empleado.Text.Trim();

            if (Cmb_Busqueda_Rol.SelectedIndex > 0)
                INF_EMPLEADOS.P_Rol_ID = Cmb_Busqueda_Rol.SelectedValue.Trim();

            if (Cmb_Busqueda_Dependencia.SelectedIndex > 0)
                INF_EMPLEADOS.P_Dependencia_ID = Cmb_Busqueda_Dependencia.SelectedValue.Trim();

            if (Cmb_Busqueda_Areas.SelectedIndex > 0)
                INF_EMPLEADOS.P_Area_ID = Cmb_Busqueda_Areas.SelectedValue.Trim();

            Dt_Empleados = INF_EMPLEADOS.Consulta_Empleados_General().P_Dt_Empleados;
            Llena_Grid_Empleados(Dt_Empleados);

            Upnl_Empleado_No_Nominales_Sistema.Update();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al ejecutar la búsqueda de empleados. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Combos)
    protected void Cmb_SAP_Unidad_Responsable_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cmb_SAP_Unidad_Responsable.SelectedIndex > 0)
            {
                Consulta_Areas_Dependencia(Cmb_SAP_Unidad_Responsable.SelectedValue.Trim());
                Cmb_Areas_Empleado.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al seleccionar un elemento de la lista de Unidades Responsables. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cmb_Busqueda_Dependencia_SelectedIndexChanged
    /// DESCRIPCION : Cargar las area correspodientes a la dependencia seleccionada.
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Cmb_Busqueda_Dependencia_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Consulta_Areas_Dependencia_Busqueda(Cmb_Busqueda_Dependencia.SelectedValue.Trim());
            Cmb_Busqueda_Areas.Enabled = true;
            Mpe_Busqueda_Empleados.Show();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion

    #endregion
}
