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
using Presidencia.Sessiones;
using Presidencia.Bitacora_Eventos;
using Presidencia.Constantes;
using Presidencia.Capitulos.Negocio;
using Presidencia.Capitulos.Datos;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Collections.Generic;

public partial class paginas_Paginas_Generales_Frm_Cat_SAP_Capitulos : System.Web.UI.Page
{
    #region METODOS

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Inicializa_Controles
    /// 	DESCRIPCIÓN: Prepara los controles en la forma para que el usuario pueda realizar 
    /// 	            diferentes operaciones
    /// 	PARÁMETROS:
    /// 	CREO: Jacqueline Ramírez Sierra
    /// 	FECHA_CREO: 28-feb-2011
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
            Consulta_Capitulos(); //Consulta todos los capitulos en la BD
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
    /// 	CREO: Jacqueline Ramírez Sierra
    /// 	FECHA_CREO: 28-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado; ///Indica si el control de la forma va a ser habilitado para que los edite el usuario
        Boolean Habilitado2;

        try
        {
            Habilitado = false;
            Habilitado2 = true;
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

                    Configuracion_Acceso("Frm_Cat_SAP_Capitulos.aspx");
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
                    //Btn_Nuevo.CausesValidation = true;
                    //Btn_Modificar.CausesValidation = true;
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
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Txt_Clave.Enabled = false;
                    Txt_Descripcion.Enabled = false;
                    Txt_Clave.Enabled = Habilitado2;
                    Txt_Descripcion.Enabled = Habilitado2;
                    break;

                case "Eliminar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Txt_Clave.Enabled = false;
                    Txt_Descripcion.Enabled = false;
                    Txt_Clave.Enabled = Habilitado2;
                    Txt_Descripcion.Enabled = Habilitado2;
                    break;
            }

            ///Habilitar campos de texto para edición o inhabilitar si no se requieren de acuerdo con variable Habilitado
            Txt_Clave.Enabled = Habilitado;
            Txt_Descripcion.Enabled = Habilitado;
            Cmb_Estatus.Enabled = Habilitado;
            Grid_Capitulos.Enabled = !Habilitado;
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
    /// 	CREO: Jacqueline Ramírez Sierra
    /// 	FECHA_CREO: 28-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Txt_Capitulo_ID.Value = "";
            Txt_Busqueda.Text = "";
            Txt_Clave.Text = "";
            Cmb_Estatus.SelectedIndex = 0;
            Txt_Descripcion.Text = "";
        }
        catch (Exception ex)
        {
            throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Consulta_Capitulos
    /// 	DESCRIPCIÓN: Consulta los capitulos que estan dadas de alta en la BD
    /// 	PARÁMETROS:
    /// 	CREO: Jacqueline Ramírez Sierra
    /// 	FECHA_CREO: 28-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Consulta_Capitulos()
    {
        Cls_Cat_SAP_Capitulos_Negocio Capitulos = new Cls_Cat_SAP_Capitulos_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Capitulos; //Variable que obtendrá los datos de la consulta 

        try
        {
            if (Txt_Busqueda.Text != "")            //Si el campo búsqueda contiene texto
            {
                Capitulos.P_Clave = Txt_Busqueda.Text;
                Capitulos.P_Descripcion = Txt_Busqueda.Text;
            }
            Dt_Capitulos = Capitulos.Consulta_Datos_Capitulos(); //Consulta los capitulos con sus datos generales
            Session["Consulta_Capitulos"] = Dt_Capitulos;
            Llena_Grid_Capitulos();
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Capitulos " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Llena_Grid_Capitulols
    /// 	DESCRIPCIÓN: Llena el grid con los Capitulos de la base de datos
    /// 	PARÁMETROS:
    /// 	CREO: Jacqueline Ramírez Sierra
    /// 	FECHA_CREO: 28-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Llena_Grid_Capitulos()
    {
        DataTable Dt_Capitulos; //Variable que obtendrá los datos de la consulta 
        try
        {
            Grid_Capitulos.DataBind();
            Dt_Capitulos = (DataTable)Session["Consulta_Capitulos"];
            Grid_Capitulos.DataSource = Dt_Capitulos;
            Grid_Capitulos.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Capitulos " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Alta_Capitulos
    /// 	DESCRIPCIÓN: Dar de alta un capitulo con los datos que proporcionó el usuario
    /// 	PARÁMETROS:
    /// 	CREO: Jacqueline Ramírez Sierra
    /// 	FECHA_CREO: 28-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Alta_Capitulos()
    {
        Cls_Cat_SAP_Capitulos_Negocio Capitulos = new Cls_Cat_SAP_Capitulos_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            Capitulos.P_Clave = Txt_Clave.Text;
            Capitulos.P_Estatus = Cmb_Estatus.SelectedValue;
            Capitulos.P_Descripcion = Txt_Descripcion.Text;
            Capitulos.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

            Capitulos.Alta_Capitulos(); //Da de alta los datos del capitulo proporcionado por el usuario en la BD
            //Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo Capitulos ", "alert('El Alta del Capitulo fue Exitosa');", true);
            Inicializa_Controles();
        }
        catch (Exception Ex)
        {
            throw new Exception("Alta_Capitulos " + Ex.Message.ToString(), Ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Modificar_Capitulo
    /// 	DESCRIPCIÓN: Modifica los datos del capitulo con los datos que introdujo el usuario
    /// 	PARÁMETROS:
    /// 	CREO: Jacqueline Ramírez Sierra
    /// 	FECHA_CREO: 28-feb-2011
    /// 	MODIFICÓ: Jesus Toledo Rdz
    /// 	FECHA_MODIFICÓ: 27-Mayo-2011
    /// 	CAUSA_MODIFICACIÓN:
    ///*******************************************************************************************************
    private void Modificar_Capitulos()
    {
        Cls_Cat_SAP_Capitulos_Negocio Capitulos = new Cls_Cat_SAP_Capitulos_Negocio(); //Variable de conexión hacia la capa de Negocios para envio de datos a modificar
        try
        {
            Capitulos.P_Capitulo_ID = Txt_Capitulo_ID.Value;
            Capitulos.P_Clave = Txt_Clave.Text;
            Capitulos.P_Descripcion = Txt_Descripcion.Text;
            Capitulos.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            Capitulos.P_Estatus = Cmb_Estatus.SelectedValue;

            Capitulos.Modificar_Capitulos(); //Sustituye los datos que se encuentran en la BD por lo que introdujo el usuario
            Habilitar_Controles("Inicial");
            //Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo Capitulos", "alert('La Modificación del Capitulo fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Capitulos " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Validar_Campos
    /// 	DESCRIPCIÓN: Revisar que los campos obligatorios hayan sido llenados y si no, generar el mensaje 
    /// 	            correspondiente.
    /// 	PARÁMETROS:
    /// 	CREO: Jacqueline Ramírez Sierra
    /// 	FECHA_CREO: 28-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Validar_Campos()
    {
        //Si falta alguno de los campos mencionarlo en la etiqueta Lbl_Mensaje_Error para mostrarla 
        Lbl_Mensaje_Error.Text = "";
        if (Txt_Clave.Text == "")  //Validar campo CLAVE deL CAPITULo (no vacío)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir la Clave del Capitulo <br />";
        }
        else if (Txt_Clave.Text.Length != 4)  //Validar campo CLAVE del Capitulo (longitud diferente de 4)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + La clave debe ser de 4 caracteres<br />";
        }
        else    //Validar que lo que hay en el campo clave no esté ya en la base de datos
        {
            DataTable Dt_Capitulos; //Variable que obtendrá los datos de la consulta 
            Cls_Cat_SAP_Capitulos_Negocio Capitulos = new Cls_Cat_SAP_Capitulos_Negocio(); //Variable de conexión hacia la capa de Negocios
            try
            {
                if (Txt_Clave.Text != "" && Btn_Nuevo.ToolTip == "Dar de Alta")
                {
                    Capitulos.P_Clave = Txt_Clave.Text;
                    Dt_Capitulos = Capitulos.Consulta_Datos_Capitulos(); //Consulta los capitulos con sus datos generales
                    if (Dt_Capitulos.Rows.Count > 0)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + La Clave proporcionada ya se encuentra en la base de datos, la tiene el Capitulo: ";
                        Lbl_Mensaje_Error.Text += Dt_Capitulos.Rows[0][Cat_SAP_Capitulos.Campo_Descripcion].ToString() + "<br />";
                    }
                }

            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
       }
        if (Txt_Descripcion.Text == "")  //Validar campo DESCRIPCION (no vacío)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir la Descipción o nombre del Capitulo <br />";
        }
        else if (Txt_Descripcion.Text.Length > 255)  //Validar campo DESCRIPCION (longitud menor a 100)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Que el campo Descripión no contenga más de 255 caracteres <br />";
        }
    }

    #endregion METODOS

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
                Habilitar_Controles("Inicial");
                Limpiar_Controles();
                Llena_Grid_Capitulos();
                ViewState["SortDirection"] = "DESC";
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Nuevo_Click
    /// 	DESCRIPCIÓN: Habilita la forma para ingresar datos y permitir guardar un nuevo registro
    /// 	            en caso de guardar, verifica la validez de los datos ingresados y reporta cualquier
    /// 	            error
    /// 	PARÁMETROS:
    /// 	CREO: Jacqueline Ramírez Sierra
    /// 	FECHA_CREO: 28-feb-2011
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
                    Lbl_Mensaje_Error.Text = " <br />" + Lbl_Mensaje_Error.Text;
                }
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces da de alta los mismo en la base de datos
                else
                {
                    Alta_Capitulos(); //Da de alta los datos proporcionados por el usuario
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
    /// 	NOMBRE_FUNCIÓN: Btn_Modificar_Click
    /// 	DESCRIPCIÓN: Manejo del evento Click para el control Btn_Modificar. Validar los datos en los campos 
    /// 	        antes de enviar
    /// 	PARÁMETROS:
    /// 	CREO: Jacqueline Ramírez Sierra
    /// 	FECHA_CREO: 28-feb-2011
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
                if (Txt_Capitulo_ID.Value != "")
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                    Txt_Clave.Focus();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el capitulo a modificar<br />";
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
                    Lbl_Mensaje_Error.Text = " <br />" + Lbl_Mensaje_Error.Text;
                }
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces actualizar los mismo en la base de datos
                else
                {
                    Modificar_Capitulos(); //Actualizar los datos del capitulo
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
    /// 	CREO: Jacqueline Ramírez Sierra
    /// 	FECHA_CREO: 28-feb-2011
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
    /// 	NOMBRE_FUNCIÓN: Btn_Eliminar_Click
    /// 	DESCRIPCIÓN: Manejo del evento Click para el control Btn_Eliminar. 
    /// 	PARÁMETROS:
    /// 	CREO: Jacqueline Ramírez Sierra
    /// 	FECHA_CREO: 01-marzo-2011
    /// 	MODIFICÓ: Jesus Toledo  
    /// 	FECHA_MODIFICÓ: 27-Mayo-2011
    /// 	CAUSA_MODIFICACIÓN: se modifico los mensajes al usuario
    ///*******************************************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Cat_SAP_Capitulos_Negocio Capitulos = new Cls_Cat_SAP_Capitulos_Negocio();
        //Div_Contenedor_error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        if (Grid_Capitulos.SelectedIndex >= 0)
        {
            GridViewRow selectedRow = Grid_Capitulos.Rows[Grid_Capitulos.SelectedIndex];
            String clave = HttpUtility.HtmlDecode(selectedRow.Cells[1].Text).ToString().Trim();
            String estatus = Cmb_Estatus.SelectedValue.ToString();
            if (estatus == "ACTIVO")
            {
                Capitulos.P_Estatus = "INACTIVO";
                Capitulos.P_Clave = clave;
                Capitulos.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
                if (Capitulos.Eliminar_Capitulos())
                {
                    //Cargar_Datos_Negocio(true);
                    Habilitar_Controles("Inicial");
                    Llena_Grid_Capitulos();
                    //Inicializa_Controles();
                    //Habilitar_Forma(false);
                    //Limpiar_Controles();
                    //Limpiar_Formulario();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "INACTIVAR combo", "alert('El capítulo se desactivo exitosamente');", true);
                    Cmb_Estatus.SelectedValue = "INACTIVO";
                    Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Baja, "Frm_Cat_SAP_Capitulos.aspx", Capitulos.P_Capitulo_ID, "");
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se pudo eliminar el Capítulo seleccionado<br />";
                }
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "El capítulo ya tiene estatus de inactivo<br />";
            }
        }
        else
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Debe seleccionar un capítulo<br />";
        }
    }
    

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Buscar_Click
    /// 	DESCRIPCIÓN: Buscar capitulos en la base de datos por clave y descripcion
    /// 	PARÁMETROS:
    /// 	CREO: Jacqueline Ramírez Sierra
    /// 	FECHA_CREO: 28-feb-2011
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
            Consulta_Capitulos(); //Método que consulta los elemento en la base de datos
            Limpiar_Controles(); //Limpia los controles de la forma
            Btn_Salir.ToolTip = "Regresar";
            if (Grid_Capitulos.Rows.Count <= 0)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encuentra el Capitulo<br />";
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
    /// 	NOMBRE_FUNCIÓN: Grid_Capitulos_SelectedIndexChanged
    /// 	DESCRIPCIÓN: Consulta los datos del capitulo que seleccionó el usuario y los muestra en los campos
    /// 	PARÁMETROS:
    /// 	CREO: Jacqueline Ramírez Sierra
    /// 	FECHA_CREO: 28-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Capitulos_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_SAP_Capitulos_Negocio Capitulos = new Cls_Cat_SAP_Capitulos_Negocio(); //Variable de conexión a la capa de Negocios para la consulta de los datos del Producto
        DataTable Dt_Capitulos; //Variable que obtendrá los datos de la consulta

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Capitulos.P_Clave = Grid_Capitulos.SelectedRow.Cells[1].Text;
            Dt_Capitulos = Capitulos.Consulta_Datos_Capitulos(); //Consulta los datos del capitulo que fue seleccionado por el usuario
            if (Dt_Capitulos.Rows.Count > 0)
            {
                //Agrega los valores de los campos a los controles correspondientes de la forma
                foreach (DataRow Registro in Dt_Capitulos.Rows)
                {
                    Txt_Capitulo_ID.Value = Registro[Cat_SAP_Capitulos.Campo_Capitulo_ID].ToString();
                    Txt_Clave.Text = Registro[Cat_SAP_Capitulos.Campo_Clave].ToString();
                    Txt_Descripcion.Text = Registro[Cat_SAP_Capitulos.Campo_Descripcion].ToString();
                    Cmb_Estatus.SelectedValue = Registro[Cat_SAP_Capitulos.Campo_Estatus].ToString() == "ACTIVO" ? "ACTIVO" : "INACTIVO";
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
    /// 	NOMBRE_FUNCIÓN: Grid_Capitulos_PageIndexChanging
    /// 	DESCRIPCIÓN: Manejo del evento de paginación del grid (cargar los datos de la página seleccionada)
    /// 	PARÁMETROS:
    /// 	CREO: Jacqueline Ramírez Sierra
    /// 	FECHA_CREO: 28-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Capitulos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpiar_Controles(); //Limpia todos los controles de la forma
            Grid_Capitulos.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Llena_Grid_Capitulos(); //Carga los elementos que están asignados a la página seleccionada
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    #endregion EVENTOS

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
    #region ORDENAR GRIDS
    /// ******************************************************************************************
    /// NOMBRE: Grid_Requisiciones_Sorting
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// CREÓ: Gustavo Angeles Cruz
    /// FECHA CREÓ: 11/Junio/2011
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// ******************************************************************************************

    protected void Grid_Capitulos_Sorting(object sender, GridViewSortEventArgs e)
    {
        Grid_Sorting(Grid_Capitulos, ((DataTable)Session["Consulta_Capitulos"]), e);
    }
    /// *****************************************************************************************
    /// NOMBRE: Grid_Sorting
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// CREÓ: Gustavo Angeles Cruz
    /// FECHA CREÓ: 11/Junio/2011
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************
    private void Grid_Sorting(GridView Grid, DataTable Dt_Table, GridViewSortEventArgs e)
    {
        if (Dt_Table != null)
        {
            DataView Dv_Vista = new DataView(Dt_Table);
            String Orden = ViewState["SortDirection"].ToString();
            if (Orden.Equals("ASC"))
            {
                Dv_Vista.Sort = e.SortExpression + " DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Vista.Sort = e.SortExpression + " ASC";
                ViewState["SortDirection"] = "ASC";
            }
            Grid.DataSource = Dv_Vista;
            Grid.DataBind();
        }
    }
    #endregion
}
