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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Predial_Tipos_Documento.Negocio;

public partial class paginas_Predial_Frm_Cat_Pre_Tipos_Documento : System.Web.UI.Page
{

    ///********************************************************************************
    ///                                 METODOS
    ///********************************************************************************

    #region METODOS
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Roberto González Oseguera
    /// FECHA_CREO  : 23-mar-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            Limpiar_Controles(); //Limpia los controles del forma
            Consulta_Tipos_Documento(); //Consulta todos los Tipos de documento en la BD
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Habilitar_Controles
    /// DESCRIPCIÓN: Habilita o Deshabilita los controles de la forma para según se requiera 
    ///             para la siguiente operación
    /// PARÁMETROS:
    ///         1. Operacion: Indica la operación que se desea realizar por parte del usuario
    /// 	             (inicial, nuevo, modificar)
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 23-mar-2011 
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
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
                    Btn_Eliminar.ToolTip = "Eliminar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Eliminar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar.png";
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Cmb_Estatus.SelectedValue = "VIGENTE";
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
                    Btn_Eliminar.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    break;
            }

            ///Habilitar campos de texto para edición o inhabilitar si no se requieren de acuerdo con variable Habilitado
            Txt_Nombre.Enabled = Habilitado;
            Txt_Descripcion.Enabled = Habilitado;
            Cmb_Estatus.Enabled = Habilitado;

            Txt_Busqueda.Enabled = !Habilitado;
            Btn_Buscar.Enabled = !Habilitado;
            Grid_Tipos_Documento.Enabled = !Habilitado;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }


    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION: Limpia los controles que se encuentran en la forma
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 23-mar-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Txt_Busqueda.Text = "";

            Txt_Documento_ID.Text = "";
            Txt_Nombre.Text = "";
            Txt_Descripcion.Text = "";
        }
        catch (Exception ex)
        {
            throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Tipos_Documento
    /// DESCRIPCION: Consulta los Tipos de documento que estan dados de alta en la BD
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 23-mar-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Tipos_Documento()
    {
        Cls_Cat_Pre_Tipos_Documento_Negocio RS_Consulta_Cat_Pre_Tipos_Docum = new Cls_Cat_Pre_Tipos_Documento_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Tipos_Docum; //Variable que obtendrá los datos de la consulta 

        try
        {
            if (Txt_Busqueda.Text != "")        //si no está vacío, buscar por ID, Nombre y descripción
            {
                RS_Consulta_Cat_Pre_Tipos_Docum.P_Nombre_Documento = Txt_Busqueda.Text.ToUpper();
                RS_Consulta_Cat_Pre_Tipos_Docum.P_Descripcion = Txt_Busqueda.Text.ToUpper();
                RS_Consulta_Cat_Pre_Tipos_Docum.P_Documento_ID = Txt_Busqueda.Text;
            }

            Dt_Tipos_Docum = RS_Consulta_Cat_Pre_Tipos_Docum.Consulta_Datos_Tipos_Documento(); //Consulta los Tipos de documento con sus datos generales
            Session["Consulta_Tipos_Documento"] = Dt_Tipos_Docum;
            Llena_Grid_Tipos_Documento();
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Tipos_Documento " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llena_Grid_Tipos_Documento
    /// DESCRIPCION: Llena el grid con los Tipos dedocumento en la base de datos
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 23-mar-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llena_Grid_Tipos_Documento()
    {
        DataTable Dt_Tipos_Docum; //Variable que obtendrá los datos de la consulta 
        try
        {
            Grid_Tipos_Documento.DataBind();
            Dt_Tipos_Docum = (DataTable)Session["Consulta_Tipos_Documento"];
            Grid_Tipos_Documento.DataSource = Dt_Tipos_Docum;
            Grid_Tipos_Documento.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Tipos_Documento " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Alta_Tipo_Documento
    /// DESCRIPCIÓN: Da de alta un Tipo de documento en la base de datos a través de la capa de negocio
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 23-mar-2011 
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Alta_Tipo_Documento()
    {
        Cls_Cat_Pre_Tipos_Documento_Negocio Rs_Alta_Tipo_Docum = new Cls_Cat_Pre_Tipos_Documento_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            Rs_Alta_Tipo_Docum.P_Nombre_Documento = Txt_Nombre.Text.ToUpper();
            Rs_Alta_Tipo_Docum.P_Descripcion = Txt_Descripcion.Text.ToUpper();
            Rs_Alta_Tipo_Docum.P_Estatus = Cmb_Estatus.SelectedValue;
            Rs_Alta_Tipo_Docum.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();

            if (Rs_Alta_Tipo_Docum.Alta_Tipo_Documento() > 0) //Da de alta los datos del Tipo de documento proporcionados por el usuario en la BD
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo Tipos de documento ", "alert('El Alta del Tipo de documento fue Exitosa');", true);
                Inicializa_Controles();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo Tipos de documento ", "alert('Ocurrió un error y el Tipo de documento no se dio de alta');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Alta_Tipo_Documento " + Ex.Message.ToString(), Ex);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Modificar_Tipo_Documento
    /// DESCRIPCIÓN: Modifica los datos del Tipo de Documento con lo que introdujo el usuario
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 23-mar-2011 
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Modificar_Tipo_Documento()
    {
        Cls_Cat_Pre_Tipos_Documento_Negocio Rs_Modificar_Tipo_Docum = new Cls_Cat_Pre_Tipos_Documento_Negocio(); //Variable de conexión hacia la capa de Negoccios para envio de datos a modificar
        try
        {
            Rs_Modificar_Tipo_Docum.P_Documento_ID = Txt_Documento_ID.Text;
            Rs_Modificar_Tipo_Docum.P_Nombre_Documento = Txt_Nombre.Text.ToUpper();
            Rs_Modificar_Tipo_Docum.P_Descripcion = Txt_Descripcion.Text.ToUpper();
            Rs_Modificar_Tipo_Docum.P_Estatus = Cmb_Estatus.SelectedValue;
            Rs_Modificar_Tipo_Docum.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();

            if (Rs_Modificar_Tipo_Docum.Modificar_Tipo_Documento() > 0) //Sustituye los datos que se encuentran en la BD por lo que introdujo el usuario
            {
                Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo Tipos de documento ", "alert('La modificación del Tipo de documento fue Exitosa');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo Tipos de documento ", "alert('Ocurrió un error y el Tipo de documento no se modificó');", true);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Tipo_Documento " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Validar_Campos
    /// DESCRIPCIÓN: Revisar que los campos obligatorios hayan sido llenados y si no, generar el mensaje 
    ///             correspondiente.
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 23-mar-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Validar_Campos()
    {
        //Si falta alguno de los campos mencionarlo en la etiqueta Lbl_Mensaje_Error para mostrarla 
        Lbl_Mensaje_Error.Text = "";
        
        if (Txt_Nombre.Text == "")  //Validar campo NOMBRE (no vacío)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir el Nombre para el Tipo de documento <br />";
        }
        else if (Txt_Nombre.Text.Length > 100)  //Validar campo NOMBRE(longitud menor a 100)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Que el Nombre del tipo de documento no contenga más de 100 caracteres <br />";
        }
        if (Txt_Descripcion.Text.Length > 250)  //Validar campo DESCRIPCION (longitud menor a 250)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Que la Descripión no contenga más de 250 caracteres <br />";
        }
    }


#endregion METODOS

    ///********************************************************************************************
    ///                                 EVENTOS
    ///********************************************************************************************
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
    /// NOMBRE_FUNCIÓN: Btn_Nuevo_Click
    /// DESCRIPCIÓN: Habilita la forma para ingresar datos y permitir guardar un nuevo registro
    ///             en caso de guardar, verifica la validez de los datos ingresados y reporta cualquier
    ///             error
    /// PARAMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 23-mar-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                Limpiar_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
                Txt_Nombre.Focus();
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
                //Si todos los campos requeridos fueron proporcionados por el usuario, dar de alta los mismos en la base de datos
                else
                {
                    Alta_Tipo_Documento(); //Da de alta los datos proporcionados por el usuario
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
    /// NOMBRE_FUNCIÓN: Btn_Modificar_Click
    /// DESCRIPCIÓN: Manejo del evento Click para el control Btn_Modificar. Validar los datos en los campos 
    ///         antes de enviar
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 23-mar-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
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
                if (Txt_Documento_ID.Text != "")
                {
                    Habilitar_Controles("Modificar");   //Habilita los controles para la modificación de los datos
                    Cmb_Estatus.Focus();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el Tipo de documento cuyos datos desea modificar<br />";
                }
            }
            ///Si se da clic en el botón y el tooltip  es Actualizar, verificar la validez de los campos y enviar 
            ///los cambios o mostrar los mensajes de error correspondientes
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
                    Modificar_Tipo_Documento(); //Actualizar los datos de la partida
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
    /// NOMBRE_FUNCIÓN: Btn_Eliminar_Click
    /// DESCRIPCIÓN: Manejo del evento Click para el control Btn_Eliminar. Cambia el estatus del 
    ///             elemento seleccionado a inactivo
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 23-mar-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            //Revisar que haya un elemento seleccionado, si no, mostrar mensaje
            if (Txt_Documento_ID.Text == "")
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione el Tipo de documento que desea inactivar<br />";
            }
            //Si el combo estatus es diferente a INACTIVO seleccionar INACTIVO y enviar cambios a la BD
            if (Cmb_Estatus.SelectedValue != "BAJA")
            {
                Cmb_Estatus.SelectedValue = "BAJA"; //Seleccionar inactivo en el combo estatus para que se guarde con los otros datos
                Modificar_Tipo_Documento(); //Actualizar los datos de la Partida
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
    /// NOMBRE_FUNCIÓN: Btn_Salir_Click
    /// DESCRIPCIÓN: Manejo del evento Click para el control Btn_Salir. Ir a la página principal o 
    ///         inicializar controles 
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 23-mar-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Inicio")
            {
                Session.Remove("Consulta_Partidas");
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
    /// NOMBRE_FUNCIÓN: Btn_Buscar_Click
    /// DESCRIPCIÓN: Buscar Tipos de documento en la base de datos por nombre y descripcion
    ///             mediante la llamada al método Consulta_Tipos_Documento
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 23-mar-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Consulta_Tipos_Documento(); //Método que consulta los elementos en la base de datos
            Limpiar_Controles(); //Limpia los controles de la forma
            //Si no se encontraron Tipos de documento con un nombre similar al proporcionado por el usuario entonces manda un mensaje al usuario
            Btn_Salir.ToolTip = "Regresar";
            if (Grid_Tipos_Documento.Rows.Count <= 0)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encontraron Tipos de documento con el criterio proporcionado<br />";
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
    /// NOMBRE_FUNCIÓN: Grid_Tipos_Documento_SelectedIndexChanged
    /// DESCRIPCIÓN: Consulta los datos del elemento que seleccionó el usuario y los muestra 
    ///             en los campos correspondientes
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 23-mar-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Tipos_Documento_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Pre_Tipos_Documento_Negocio Rs_Consulta_Cat_Pre_Tipos_Docum = new Cls_Cat_Pre_Tipos_Documento_Negocio(); //Variable de conexión a la capa de Negocios para la consulta de los datos
        DataTable Dt_Tipos_Docum; // Datatable que obtendrá los datos de la consulta a la base de datos

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Rs_Consulta_Cat_Pre_Tipos_Docum.P_Documento_ID = Grid_Tipos_Documento.SelectedRow.Cells[1].Text;
            Dt_Tipos_Docum = Rs_Consulta_Cat_Pre_Tipos_Docum.Consulta_Datos_Tipos_Documento(); //Consulta los datos del Tipo de documento seleccionado por el usuario
            if (Dt_Tipos_Docum.Rows.Count > 0)
            {
                //Escribe los valores de los campos a los controles correspondientes de la forma
                foreach (DataRow Registro in Dt_Tipos_Docum.Rows)
                {
                    Txt_Documento_ID.Text = Registro[Cat_Pre_Tipos_Documento.Campo_Documento_ID].ToString();
                    Txt_Nombre.Text = Registro[Cat_Pre_Tipos_Documento.Campo_Nombre_Documento].ToString().ToUpper();
                    Txt_Descripcion.Text = Registro[Cat_Pre_Tipos_Documento.Campo_Descripcion].ToString().ToUpper();
                    Cmb_Estatus.SelectedValue = Registro[Cat_Pre_Tipos_Documento.Campo_Estatus].ToString().ToUpper() == "VIGENTE" ? "VIGENTE" : "BAJA";
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
    /// NOMBRE_FUNCIÓN: Grid_Tipos_Documento_PageIndexChanging
    /// DESCRIPCIÓN: Manejo del evento de paginación del grid (cargar los datos de la página seleccionada)
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 23-mar-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Tipos_Documento_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpiar_Controles(); //Limpia todos los controles de la forma
            Grid_Tipos_Documento.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Llena_Grid_Tipos_Documento(); //Carga los elementos que están asignados a la página seleccionada
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

#endregion EVENTOS

}
