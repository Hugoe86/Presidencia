using System;
using System.Configuration;
using System.Data;
using System.Collections;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Catalogo_SAP_Fuente_Financiamiento.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Collections.Generic;

public partial class paginas_Paginas_Generales_Frm_Cat_SAP_Fuente_Financiamiento : System.Web.UI.Page
{


    

///**********************************************************************************************************************************
///                                                                METODOS
///**********************************************************************************************************************************

#region METODOS

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Inicializa_Controles
    /// 	DESCRIPCIÓN: Prepara los controles en la forma para que el usuario pueda realizar 
    /// 	            diferentes operaciones
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 26-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            Limpiar_Controles(); //Limpia los controles del forma
            Consulta_Fuentes_Financiamiento(); //Consulta todas las Fuente de financiamiento en la BD
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Habilitar_Controles
    /// 	DESCRIPCIÓN: Habilita o Deshabilita los controles de la forma para según se requiera para la siguiente operación
    /// 	PARÁMETROS:
    /// 	            1. Operacion: Indica si se preparan los controles para un alta, una modificación o
    /// 	                    se limpian como estado inicial
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 26-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado; ///Indica si el control de la forma va a ser habilitado para que los edite el usuario

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
                    Btn_Eliminar.Visible = true;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                    Configuracion_Acceso("Frm_Cat_SAP_Fuente_Financiamiento.aspx");
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    break;

                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    break;
            }

            ///Habilitar campos de texto para edición o inhabilitar si no se requieren de acuerdo con variable Habilitado
            Txt_Clave.Enabled = Habilitado;
            Txt_Descripcion.Enabled = Habilitado;
            Cmb_Estatus.Enabled = Habilitado;
            Cmb_Especiales_Ramo_33.Enabled = Habilitado;
            Txt_Anio.Enabled = Habilitado;
            Grid_Fuentes_Financiamiento.Enabled = !Habilitado;
            Txt_Busqueda.Enabled = !Habilitado;     //deshabilitar la búsqueda mientras se editan los datos
            Btn_Buscar.Enabled = !Habilitado;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Limpiar_Controles
    /// 	DESCRIPCIÓN: Limpia los controles que se encuentran en la forma
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 26-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Txt_Fuentes_Financiamiento_ID.Value = "";
            Txt_Busqueda.Text = "";
            Txt_Clave.Text = "";
            Cmb_Estatus.SelectedIndex = 0;
            Cmb_Especiales_Ramo_33.SelectedIndex = 0;
            Txt_Descripcion.Text = "";
            Txt_Anio.Text = "";
        }
        catch (Exception ex)
        {
            throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Consulta_Fuentes_Financiamiento
    /// 	DESCRIPCIÓN: Consulta las fuentes de financiamiento que estan dadas de alta en la BD
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 26-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Consulta_Fuentes_Financiamiento()
    {
        Cls_Cat_SAP_Fuente_Financiamiento_Negocio RS_Consulta_Cat_Fuentes_Financiamiento = new Cls_Cat_SAP_Fuente_Financiamiento_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Fuentes_Financiamiento; //Variable que obtendrá los datos de la consulta 

        try
        {
            if (Txt_Busqueda.Text != "")            //Si el campo búsqueda contiene texto
            {
                RS_Consulta_Cat_Fuentes_Financiamiento.P_Clave = Txt_Busqueda.Text;
                RS_Consulta_Cat_Fuentes_Financiamiento.P_Descripcion = Txt_Busqueda.Text;
            }
            Dt_Fuentes_Financiamiento = RS_Consulta_Cat_Fuentes_Financiamiento.Consulta_Datos_Fuente_Financiamiento(); //Consulta las fuentes de funanciamiento con sus datos generales
            Session["Consulta_Fuentes_Financiamiento"] = Dt_Fuentes_Financiamiento;
            Llena_Grid_Fuentes_Financiamiento();
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Fuentes_Financiamiento " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Llena_Grid_Fuentes_Financiamiento
    /// 	DESCRIPCIÓN: Llena el grid con las fuentes de financiamiento de la base de datos
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 26-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Llena_Grid_Fuentes_Financiamiento()
    {
        DataTable Dt_Fuentes_Financiamiento; //Variable que obtendrá los datos de la consulta 
        try
        {
            Grid_Fuentes_Financiamiento.DataBind();
            Dt_Fuentes_Financiamiento = (DataTable)Session["Consulta_Fuentes_Financiamiento"];
            Grid_Fuentes_Financiamiento.DataSource = Dt_Fuentes_Financiamiento;
            Grid_Fuentes_Financiamiento.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Fuentes_Financiamiento " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Alta_Fuente_Financiamiento
    /// 	DESCRIPCIÓN: Dar de alta una fuente de financiamiento con los datos que proporcionó el usuario
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 26-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Alta_Fuente_Financiamiento()
    {
        Cls_Cat_SAP_Fuente_Financiamiento_Negocio Rs_Alta_Fuente_Financiamiento = new Cls_Cat_SAP_Fuente_Financiamiento_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            Rs_Alta_Fuente_Financiamiento.P_Clave = Txt_Clave.Text;
            Rs_Alta_Fuente_Financiamiento.P_Estatus = Cmb_Estatus.SelectedValue;
            Rs_Alta_Fuente_Financiamiento.P_Descripcion = Txt_Descripcion.Text;
            Rs_Alta_Fuente_Financiamiento.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            Rs_Alta_Fuente_Financiamiento.P_Anio = Txt_Anio.Text;
            Rs_Alta_Fuente_Financiamiento.P_Especiales_Ramo_33 = Cmb_Especiales_Ramo_33.SelectedValue;

            Rs_Alta_Fuente_Financiamiento.Alta_Fuente_Financiamiento(); //Da de alta los datos dela fuente de financiamiento proporcionados por el usuario en la BD
            //Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo Fuentes de financiamiento ", "alert('El Alta de la Fuente de financiamiento fue Exitosa');", true);
            Inicializa_Controles();
        }
        catch (Exception Ex)
        {
            throw new Exception("Alta_Fuente_Financiamiento " + Ex.Message.ToString(), Ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Modificar_Fuente_Financiamiento
    /// 	DESCRIPCIÓN: Modifica los datos de la fuente de financiamiento con los datos que introdujo el usuario
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 26-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Modificar_Fuente_Financiamiento()
    {
        Cls_Cat_SAP_Fuente_Financiamiento_Negocio Rs_Modificar_Cat_Fuentes_Financiamiento = new Cls_Cat_SAP_Fuente_Financiamiento_Negocio(); //Variable de conexión hacia la capa de Negoccios para envio de datos a modificar
        try
        {
            Rs_Modificar_Cat_Fuentes_Financiamiento.P_Fuente_Financiamiento_ID = Txt_Fuentes_Financiamiento_ID.Value;
            Rs_Modificar_Cat_Fuentes_Financiamiento.P_Clave = Txt_Clave.Text;
            Rs_Modificar_Cat_Fuentes_Financiamiento.P_Descripcion = Txt_Descripcion.Text;
            Rs_Modificar_Cat_Fuentes_Financiamiento.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            Rs_Modificar_Cat_Fuentes_Financiamiento.P_Estatus = Cmb_Estatus.SelectedValue;
            Rs_Modificar_Cat_Fuentes_Financiamiento.P_Anio = Txt_Anio.Text;
            Rs_Modificar_Cat_Fuentes_Financiamiento.P_Especiales_Ramo_33 = Cmb_Especiales_Ramo_33.SelectedValue;

            Rs_Modificar_Cat_Fuentes_Financiamiento.Modificar_Fuente_Financiamiento(); //Sustituye los datos que se encuentran en la BD por lo que introdujo el usuario
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo Fuentes de financiamiento", "alert('La Modificación de la Fuente de financiamiento fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Fuente_Financiamiento " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Validar_Campos
    /// 	DESCRIPCIÓN: Revisar que los campos obligatorios hayan sido llenados y si no, generar el mensaje 
    /// 	            correspondiente.
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 26-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Validar_Campos()
    {
        //Si falta alguno de los campos mencionarlo en la etiqueta Lbl_Mensaje_Error para mostrarla 
        Lbl_Mensaje_Error.Text = "";
        if (Txt_Clave.Text == "")  //Validar campo CLAVE de la fuente de financiamiento (no vacío)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir la Clave de la Fuente de financiamiento <br />";
        }
        else if (Txt_Clave.Text.Length > 5 || Txt_Clave.Text.Length < 4)  //Validar campo CLAVE de la Fuente de financiamiento (longitud de 4 ó 5)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Que la Clave sea de 4 ó 5 caracteres<br />";
        }
        else    //Validar que lo que hay en el campo clave no esté ya en la base de datos
        {
            DataTable Dt_Fuentes_Financiamiento; //Variable que obtendrá los datos de la consulta 
            Cls_Cat_SAP_Fuente_Financiamiento_Negocio RS_Consulta_Cat_SAP_Fuentes_Financiamiento = new Cls_Cat_SAP_Fuente_Financiamiento_Negocio(); //Variable de conexión hacia la capa de Negocios
            try
            {
                if (Txt_Clave.Text != "")
                {
                    RS_Consulta_Cat_SAP_Fuentes_Financiamiento.P_Clave = Txt_Clave.Text;
                    Dt_Fuentes_Financiamiento = RS_Consulta_Cat_SAP_Fuentes_Financiamiento.Consulta_Fuente_Financiamiento(); //Consulta las Fuentes de financiamiento con sus datos generales
                    if (Dt_Fuentes_Financiamiento.Rows.Count > 0)
                    {             // Si se está actualizando y el ID que se recibió es igual al de la fuente de fin. seleccionada o es un registro nuevo, mostrar mensaje indicando que la clave ya existe
                        if ((Btn_Modificar.ToolTip == "Actualizar" && Txt_Fuentes_Financiamiento_ID.Value != Dt_Fuentes_Financiamiento.Rows[0][Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID].ToString()) || Btn_Nuevo.ToolTip == "Dar de Alta")
                        {
                            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + La Clave proporcionada ya se encuentra en la base de datos, la tiene la Fuente de financiamiento: ";
                            Lbl_Mensaje_Error.Text += Dt_Fuentes_Financiamiento.Rows[0][Cat_SAP_Fuente_Financiamiento.Campo_Descripcion].ToString() + "<br />";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Consulta_Fuentes_Financiamiento_Duplicadas " + ex.Message.ToString(), ex);
            }
        }
        if (Txt_Descripcion.Text == "")  //Validar campo DESCRIPCION (no vacío)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir la Descipción o nombre de la fuente de financiamiento <br />";
        }
        else if (Txt_Descripcion.Text.Length > 255)  //Validar campo DESCRIPCION (longitud menor a 100)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Que el campo Descripión no contenga más de 255 caracteres <br />";
        }
        //if (Cmb_Especiales_Ramo_33.SelectedIndex == 0)
        //{
        //    Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Selecciona si pertenece aun programa especial <br />";
        //}
    }

#endregion METODOS

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
            Botones.Add(Btn_Buscar);

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

///**********************************************************************************************************************************
///                                                                EVENTOS
///**********************************************************************************************************************************
#region EVENTOS

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) 
                Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                ViewState["SortDirection"] = "ASC";
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Nuevo_Click
    /// 	DESCRIPCIÓN: Habilita la forma para ingresar datos y permitir guardar un nuevo registro
    /// 	            en caso de guardar, verifica la validez de los datos ingresados y reporta cualquier
    /// 	            error
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 26-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                Limpiar_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
                Txt_Clave.Focus();
            }
            else
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Validar_Campos();

                //Si faltaron campos por capturar envía un mensaje al usuario indicando cuáles
                if (Lbl_Mensaje_Error.Text.Length > 0)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario: <br />" + Lbl_Mensaje_Error.Text;
                }
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces da de alta los mismo en la base de datos
                else
                {
                    Alta_Fuente_Financiamiento(); //Da de alta los datos proporcionados por el usuario
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
    
    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Eliminar_Click
    /// 	DESCRIPCIÓN: Manejo del evento Click para el control Btn_Eliminar. Cambia el estatus del 
    /// 	            elemento seleccionado a inactivo
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 26-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            //Revisar que haya un elemento seleccionado, si no, mostrar mensaje
            if (Txt_Fuentes_Financiamiento_ID.Value == "")
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione la Fuente de financiamiento que desea inactivar<br />";
            }
                        //Si el combo estatus es diferente a INACTIVO seleccionar INACTIVO y enviar cambios a la BD
            if (Cmb_Estatus.SelectedValue != "INACTIVO")
            {
                Cmb_Estatus.SelectedValue = "INACTIVO"; //Seleccionar inactivo en el combo estatus para que se guarde con los otros datos
                Modificar_Fuente_Financiamiento(); //Actualizar los datos de la fuente de financiamiento
            }
            
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Modificar_Click
    /// 	DESCRIPCIÓN: Manejo del evento Click para el control Btn_Modificar. Validar los datos en los campos 
    /// 	        antes de enviar
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 26-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            //si se dio clic en el botón Modificar, revisar que haya un elemento seleccionado, si no mostrar mensaje
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                if (Txt_Fuentes_Financiamiento_ID.Value != "")
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                    Txt_Clave.Focus();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione la Fuente de financiamiento cuyos datos desea modificar<br />";
                }
            }
            ///Si se da clic en el botón y el tooltip  es Actualizar, verificar la validez de los campos y enviar 
            ///los cambios o los mensajes de error correspondientes
            else
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Validar_Campos();

                //Si faltaron campos por capturar envia un mensaje al usuario indicando cuáles
                if (Lbl_Mensaje_Error.Text.Length > 0)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario: <br />" + Lbl_Mensaje_Error.Text;
                }
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces actualizar los mismo en la base de datos
                else
                {
                    Modificar_Fuente_Financiamiento(); //Actualizar los datos de la fuente de financiamiento
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

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Salir_Click
    /// 	DESCRIPCIÓN: Manejo del evento Click para el control Btn_Salir. Ir a la página principal o 
    /// 	        inicializar controles 
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 26-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Inicio")
            {
                Session.Remove("Consulta_Fuentes_Financiamiento");
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

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Buscar_Click
    /// 	DESCRIPCIÓN: Buscar fuentes de financiamiento en la base de datos por clave y descripcion
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 26-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Consulta_Fuentes_Financiamiento(); //Método que consulta los elemento en la base de datos
            Limpiar_Controles(); //Limpia los controles de la forma
            //Si no se encontraron Productos con un nombre similar al proporcionado por el usuario entonces manda un mensaje al usuario
            Btn_Salir.ToolTip = "Regresar";
            if (Grid_Fuentes_Financiamiento.Rows.Count <= 0)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encontraron Fuentes de financiamiento con el criterio proporcionado<br />";
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Grid_Fuentes_Financiamiento_SelectedIndexChanged
    /// 	DESCRIPCIÓN: Consulta los datos de la fuente de financiamient que seleccionó el usuario y los muestra en los campos
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 26-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Fuentes_Financiamiento_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_SAP_Fuente_Financiamiento_Negocio Rs_Consulta_Cat_SAP_Fuentes_Financiamiento = new Cls_Cat_SAP_Fuente_Financiamiento_Negocio(); //Variable de conexión a la capa de Negocios para la consulta de los datos del Producto
        DataTable Dt_Fuentes_Financiamiento; //Variable que obtendrá los datos de la consulta

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Rs_Consulta_Cat_SAP_Fuentes_Financiamiento.P_Clave = Grid_Fuentes_Financiamiento.SelectedRow.Cells[1].Text;
            Dt_Fuentes_Financiamiento = Rs_Consulta_Cat_SAP_Fuentes_Financiamiento.Consulta_Datos_Fuente_Financiamiento(); //Consulta los datos de la Fuentes de financiamiento que fue seleccionada por el usuario
            if (Dt_Fuentes_Financiamiento.Rows.Count > 0)
            {
                //Agrega los valores de los campos a los controles correspondientes de la forma
                foreach (DataRow Registro in Dt_Fuentes_Financiamiento.Rows)
                {
                    Txt_Fuentes_Financiamiento_ID.Value = Registro[Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID].ToString();
                    Txt_Clave.Text = Registro[Cat_SAP_Fuente_Financiamiento.Campo_Clave].ToString();
                    Txt_Descripcion.Text = Registro[Cat_SAP_Fuente_Financiamiento.Campo_Descripcion].ToString();
                    Txt_Anio.Text = Registro[Cat_SAP_Fuente_Financiamiento.Campo_Anio].ToString();
                    Cmb_Estatus.SelectedValue = Registro[Cat_SAP_Fuente_Financiamiento.Campo_Estatus].ToString() == "ACTIVO" ? "ACTIVO" : "INACTIVO";
                    Cmb_Especiales_Ramo_33.SelectedValue = Registro[Cat_SAP_Fuente_Financiamiento.Campo_Especiales_Ramo_33].ToString() == "NO" ? "NO" : "SI";
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

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Grid_Fuentes_Financiamiento_PageIndexChanging
    /// 	DESCRIPCIÓN: Manejo del evento de paginación del grid (cargar los datos de la página seleccionada)
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 26-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Fuentes_Financiamiento_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpiar_Controles(); //Limpia todos los controles de la forma
            Grid_Fuentes_Financiamiento.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Llena_Grid_Fuentes_Financiamiento(); //Carga los elementos que están asignados a la página seleccionada
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    /// ********************************************************************************
    /// NOMBRE: Grid_Fuentes_Financiamiento_Sorting
    /// 
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// 
    /// CREÓ:   Hugo Enrique Ramirez Aguilera
    /// FECHA CREÓ: 02-Noviembre-2011
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **********************************************************************************
    protected void Grid_Fuentes_Financiamiento_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            Consultar_Grid_Fuentes_Financiamiento();
            DataTable Dt_Fuentes_Financiamiento = (Grid_Fuentes_Financiamiento.DataSource as DataTable);

            if (Dt_Fuentes_Financiamiento != null)
            {
                DataView Dv_Fuentes_Financiamiento = new DataView(Dt_Fuentes_Financiamiento);
                String Orden = ViewState["SortDirection"].ToString();

                if (Orden.Equals("ASC"))
                {
                    Dv_Fuentes_Financiamiento.Sort = e.SortExpression + " " + "DESC";
                    ViewState["SortDirection"] = "DESC";
                }
                else
                {
                    Dv_Fuentes_Financiamiento.Sort = e.SortExpression + " " + "ASC";
                    ViewState["SortDirection"] = "ASC";
                }

                Grid_Fuentes_Financiamiento.DataSource = Dv_Fuentes_Financiamiento;
                Grid_Fuentes_Financiamiento.DataBind();
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
            /// NOMBRE DE LA FUNCION: Consultar_Grid_Movimientos
            /// DESCRIPCION : Realiza una consulta para obtener todos las registros de la tabla
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 19-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consultar_Grid_Fuentes_Financiamiento()
            {
                Cls_Cat_SAP_Fuente_Financiamiento_Negocio Cls_Fuente_Financiamiento = new Cls_Cat_SAP_Fuente_Financiamiento_Negocio(); //Variable de conexión hacia la capa de Negocios
                DataTable Dt_Fuentes_Financiamiento = null; 
                try
                {
                    Dt_Fuentes_Financiamiento = Cls_Fuente_Financiamiento.Consulta_Datos_Fuente_Financiamiento();

                    Session["Consulta_Fuentes_Financiamiento"] = Dt_Fuentes_Financiamiento;
                    Grid_Fuentes_Financiamiento.DataSource = (DataTable)Session["Consulta_Fuentes_Financiamiento"];
                    Grid_Fuentes_Financiamiento.DataBind();
                }
                catch (Exception ex)
                {
                    throw new Exception("Consultar Movimientos " + ex.Message.ToString(), ex);
                }
            }

#endregion EVENTOS
}


