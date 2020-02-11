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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Compras_Giros.Negocio;
using Presidencia.Catalogo_Compras_Partidas.Negocio;
using System.Collections.Generic;
using Presidencia.Capitulos.Negocio;
using Presidencia.Sap_Partida_Generica.Negocio;

public partial class paginas_Compras_Frm_Cat_Com_Partidas : System.Web.UI.Page
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
            Limpiar_Controles(); //Limpia los controles de la forma
            Cargar_Combo_Capitulos();
            //Cargar_Combo_Giros();
            Consulta_Partidas(); //Consulta todas las Partidas en la BD
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Habilitar_Controles
    /// 	DESCRIPCIÓN: Habilita o Deshabilita los controles de la forma para según se requiera para la 
    /// 	             siguiente operación
    /// 	PARÁMETROS:
    /// 	            1. Operacion: Indica si se preparan los controles para un alta, una modificación o
    /// 	                    se limpian como estado inicial
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 28-feb-2011
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
                    Cmb_Concepto.Enabled = false;
                    Cmb_Concepto.DataSource = null;
                    Cmb_Concepto.DataBind();
                    Cmb_Partida_Generica.Enabled = false;
                    Cmb_Partida_Generica.DataSource = null;
                    Cmb_Partida_Generica.DataBind();

                    Configuracion_Acceso("Frm_Cat_Com_Partidas.aspx");
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
                    Cmb_Concepto.Enabled = false;
                    Cmb_Partida_Generica.Enabled = false;
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
                    Btn_Modificar.CausesValidation = true;      //Sólo si hay un capítulo seleccionado, habilitar el combo Concepto
                    if (Cmb_Capitulo.SelectedIndex > 0)
                        Cmb_Concepto.Enabled = true;
                    else
                        Cmb_Concepto.Enabled = false;
                    if (Cmb_Concepto.SelectedIndex > 0)         //Sólo su hay un Concepto seleccionado, habilitar el combo Partda genérica
                        Cmb_Partida_Generica.Enabled = true;
                    else
                        Cmb_Partida_Generica.Enabled = false;
                    break;
            }

            ///Habilitar campos de texto para edición o inhabilitar si no se requieren de acuerdo con variable Habilitado
            Cmb_Capitulo.Enabled = Habilitado;
            Txt_Clave.Enabled = Habilitado;
            Txt_Nombre_Partida.Enabled = Habilitado;
            Txt_Descripcion.Enabled = Habilitado;
            Txt_Descripcion_Especifica.Enabled = Habilitado;
            Cmb_Estatus.Enabled = Habilitado;
            //Cmb_Giro.Enabled = Habilitado;
            Grid_Partidas.Enabled = !Habilitado;
            Txt_Busqueda.Enabled = !Habilitado;     //deshabilitar la búsqueda mientras se editan los datos
            Btn_Buscar.Enabled = !Habilitado;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            Cmb_Operacion.Enabled = Habilitado;
            Txt_Clave_SAP.Enabled = Habilitado;
            Txt_Cuenta_SAP.Enabled = Habilitado;
            Cmb_Centro_Aplicacion_SAP.Enabled = Habilitado;
            Cmb_Afecta_Area_Funcional.Enabled = Habilitado;
            Cmb_Afecta_Partida.Enabled = Habilitado;
            Cmb_Afecta_Elemento_PEP.Enabled = Habilitado;
            Cmb_Afecta_Fondo.Enabled = Habilitado;
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
    /// 	FECHA_CREO: 28-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Txt_Partida_ID.Value = "";
            Txt_Busqueda.Text = "";
            Txt_Clave.Text = "";
            Cmb_Estatus.SelectedIndex = -1;
            Txt_Descripcion.Text = "";
            Txt_Descripcion_Especifica.Text = "";
            Txt_Nombre_Partida.Text = "";
            //Cmb_Giro.SelectedIndex = -1;
            Cmb_Capitulo.SelectedIndex = -1;
            Cmb_Concepto.SelectedIndex = -1;
            Cmb_Partida_Generica.SelectedIndex = -1;
            Txt_Cuenta_SAP.Text = "";
            Cmb_Operacion.SelectedIndex = -1;
            Txt_Clave_SAP.Text = "";
            Cmb_Centro_Aplicacion_SAP.SelectedIndex = -1;
            Cmb_Afecta_Area_Funcional.SelectedIndex = -1;
            Cmb_Afecta_Partida.SelectedIndex = -1;
            Cmb_Afecta_Elemento_PEP.SelectedIndex = -1;
            Cmb_Afecta_Fondo.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
        }
    }


    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Consulta_Partidas
    /// 	DESCRIPCIÓN: Consulta las Partidas que están dadas de alta en la BD
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 28-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Consulta_Partidas()
    {
        Cls_Cat_Com_Partidas_Negocio RS_Consulta_Cat_Partidas = new Cls_Cat_Com_Partidas_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Partidas; //Variable que obtendrá los datos de la consulta 

        try
        {
            if (Txt_Busqueda.Text != "")            //Si el campo búsqueda contiene texto
            {
                RS_Consulta_Cat_Partidas.P_Clave = "%" + Txt_Busqueda.Text + "%";
                RS_Consulta_Cat_Partidas.P_Descripcion = Txt_Busqueda.Text;
                RS_Consulta_Cat_Partidas.P_Nombre_Partida = Txt_Busqueda.Text;
            }
            Dt_Partidas = RS_Consulta_Cat_Partidas.Consulta_Datos_Partidas(); //Consulta las Partidas con sus datos generales
            Session["Consulta_Partidas"] = Dt_Partidas;
            Llena_Grid_Partidas();
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Partidas " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Llena_Grid_Partidas
    /// 	DESCRIPCIÓN: Llena el grid con las partidas de la base de datos
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 28-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Llena_Grid_Partidas()
    {
        DataTable Dt_Partidas; //Variable que obtendrá los datos de la consulta 
        try
        {
            Grid_Partidas.DataBind();
            Dt_Partidas = (DataTable)Session["Consulta_Partidas"];
            Grid_Partidas.Columns[1].Visible = true; //Activar temporalmente la columna Partida_ID para asignarle valor
            Grid_Partidas.DataSource = Dt_Partidas;
            Grid_Partidas.DataBind();
            Grid_Partidas.Columns[1].Visible = false; //Ocultar la columna Partida_ID después de asignarle valor
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Partidas " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Alta_Partida
    /// 	DESCRIPCIÓN: Dar de alta una Partida con los datos que proporcionó el usuario
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 02-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Alta_Partida()
    {
        Cls_Cat_Com_Partidas_Negocio Rs_Partida = new Cls_Cat_Com_Partidas_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            Rs_Partida.P_Clave = Txt_Clave.Text;
            Rs_Partida.P_Nombre_Partida = Txt_Nombre_Partida.Text;
            //Rs_Partida.P_Giro_ID = Cmb_Giro.SelectedValue;
            Rs_Partida.P_Estatus = Cmb_Estatus.SelectedValue;
            Rs_Partida.P_Descripcion = Txt_Descripcion.Text;
            Rs_Partida.P_Partida_Generica_ID = Cmb_Partida_Generica.SelectedValue;
            Rs_Partida.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            //Rs_Partida.P_Operacion = Cmb_Operacion.SelectedValue;
            //Rs_Partida.P_Clave_SAP = Txt_Clave_SAP.Text;
            Rs_Partida.P_Cuenta_SAP = Txt_Cuenta_SAP.Text;
            //Rs_Partida.P_Centro_Aplicacion_SAP = Cmb_Centro_Aplicacion_SAP.SelectedValue;
            //Rs_Partida.P_Afecta_Area_Funcional = Cmb_Afecta_Area_Funcional.SelectedValue;
            //Rs_Partida.P_Afecta_Partida = Cmb_Afecta_Partida.SelectedValue;
            //Rs_Partida.P_Afecta_Elemento_PEP = Cmb_Afecta_Elemento_PEP.SelectedValue;
            //Rs_Partida.P_Afecta_Fondo = Cmb_Afecta_Fondo.SelectedValue;
            Rs_Partida.P_Descripcion_Especifica = Txt_Descripcion_Especifica.Text;


            Rs_Partida.Alta_Partida(); //Da de alta los datos de la partida proporcionados por el usuario en la BD
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo Partidas ", "alert('El Alta de la Partida fue Exitosa');", true);
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
        }
        catch (Exception Ex)
        {
            throw new Exception("Alta_Partida " + Ex.Message.ToString(), Ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Modificar_Partida
    /// 	DESCRIPCIÓN: Modifica los datos de la Partida con los datos que introdujo el usuario
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 28-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Modificar_Partida()
    {
        Cls_Cat_Com_Partidas_Negocio Rs_Modificar_Cat_Com_Partidas = new Cls_Cat_Com_Partidas_Negocio(); //Variable de conexión hacia la capa de Negoccios para envio de datos a modificar
        try
        {
            Rs_Modificar_Cat_Com_Partidas.P_Partida_ID = Txt_Partida_ID.Value;
            Rs_Modificar_Cat_Com_Partidas.P_Clave = Txt_Clave.Text;
            //Rs_Modificar_Cat_Com_Partidas.P_Giro_ID = Cmb_Giro.SelectedValue;
            Rs_Modificar_Cat_Com_Partidas.P_Descripcion = Txt_Descripcion.Text;
            Rs_Modificar_Cat_Com_Partidas.P_Nombre_Partida = Txt_Nombre_Partida.Text;
            Rs_Modificar_Cat_Com_Partidas.P_Partida_Generica_ID = Cmb_Partida_Generica.SelectedValue;
            Rs_Modificar_Cat_Com_Partidas.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            Rs_Modificar_Cat_Com_Partidas.P_Estatus = Cmb_Estatus.SelectedValue;
            //Rs_Modificar_Cat_Com_Partidas.P_Operacion = Cmb_Operacion.SelectedValue;
            //Rs_Modificar_Cat_Com_Partidas.P_Clave_SAP = Txt_Clave_SAP.Text;
            Rs_Modificar_Cat_Com_Partidas.P_Cuenta_SAP = Txt_Cuenta_SAP.Text;
            //Rs_Modificar_Cat_Com_Partidas.P_Centro_Aplicacion_SAP = Cmb_Centro_Aplicacion_SAP.SelectedValue;
            //Rs_Modificar_Cat_Com_Partidas.P_Afecta_Area_Funcional = Cmb_Afecta_Area_Funcional.SelectedValue;
            //Rs_Modificar_Cat_Com_Partidas.P_Afecta_Partida = Cmb_Afecta_Partida.SelectedValue;
            //Rs_Modificar_Cat_Com_Partidas.P_Afecta_Elemento_PEP = Cmb_Afecta_Elemento_PEP.SelectedValue;
            //Rs_Modificar_Cat_Com_Partidas.P_Afecta_Fondo = Cmb_Afecta_Fondo.SelectedValue;
            Rs_Modificar_Cat_Com_Partidas.P_Descripcion_Especifica = Txt_Descripcion_Especifica.Text;

            Rs_Modificar_Cat_Com_Partidas.Modificar_Partida(); //Sustituye los datos que se encuentran en la BD por lo que introdujo el usuario
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo Partidas", "alert('La Modificación de la Partida fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Partida " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Validar_Campos
    /// 	DESCRIPCIÓN: Revisar que los campos obligatorios hayan sido llenados y si no, generar el mensaje 
    /// 	            correspondiente.
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 28-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Validar_Campos()
    {
        //Si falta alguno de los campos mencionarlo en la etiqueta Lbl_Mensaje_Error para mostrarla 
        String[] Clave_Partida_Generica;
        Lbl_Mensaje_Error.Text = "";

        //Validar combos
        //if (Cmb_Giro.SelectedIndex < 1)
        //    Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar un Giro <br />";
        if (Cmb_Capitulo.SelectedIndex < 1)
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar un Capítulo <br />";
        if (Cmb_Concepto.SelectedIndex < 1)
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar un Concepto <br />";
        if (Cmb_Partida_Generica.SelectedIndex < 1)
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar una Partida genérica <br />";

        if (Txt_Clave.Text == "")  //Validar campo CLAVE de la Partida (no vacío)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir la Clave de la Partida <br />";
        }
        else if (Txt_Clave.Text.Length != 4)  //Validar campo CLAVE de la Partida (longitud diferente de 4)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Que la Clave sea de 4 caracteres<br />";
        }
        else if (Cmb_Partida_Generica.SelectedIndex > 0) //Validar campo CLAVE de la Partida mayor que clave de Partida genérica
        {
            Clave_Partida_Generica = Cmb_Partida_Generica.SelectedItem.Text.Split(' ');
            if (Convert.ToInt32(Txt_Clave.Text) <= Convert.ToInt32(Clave_Partida_Generica[0]))
                Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Clave sea mayor que la clave de la partida genérica<br />";
        }
        //Validar que lo que hay en el campo clave no esté ya en la base de datos
        DataTable Dt_Partidas; //Variable que obtendrá los datos de la consulta 
        Cls_Cat_Com_Partidas_Negocio RS_Consulta_Cat_Com_Partidas = new Cls_Cat_Com_Partidas_Negocio(); //Variable de conexión hacia la capa de Negocios
        try
        {
            if (Txt_Clave.Text != "")
            {
                RS_Consulta_Cat_Com_Partidas.P_Clave = Txt_Clave.Text;
                Dt_Partidas = RS_Consulta_Cat_Com_Partidas.Consulta_Partidas(); //Consulta las partidas con sus datos
                if (Dt_Partidas.Rows.Count > 0)
                {
                    if ((Btn_Modificar.ToolTip == "Actualizar" && Txt_Partida_ID.Value != Dt_Partidas.Rows[0][Cat_Com_Partidas.Campo_Partida_ID].ToString()) || Btn_Nuevo.ToolTip == "Dar de Alta")  // Verificar si el 
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Proporcione otra Clave, la Partida ";
                        Lbl_Mensaje_Error.Text += Dt_Partidas.Rows[0][Cat_Com_Partidas.Campo_Nombre].ToString() + " ya tiene asignada la clave: ";
                        Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + Txt_Clave.Text + "<br />";
                        Txt_Clave.Focus();
                    }
                }
            }

        }
        catch (Exception ex)
        {
            throw new Exception("Validar_Campos " + ex.Message.ToString(), ex);
        }

        if (Txt_Nombre_Partida.Text == "")  //Validar campo NOMBRE (no vacío)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir el Nombre de la Partida <br />";
        }
        else if (Txt_Nombre_Partida.Text.Length > 250)  //Validar campo NOMBRE (longitud menor a 250)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Campo Nombre no contenga más de 250 caracteres <br />";
        }

        //if (Cmb_Operacion.SelectedIndex < 1)
        //    Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar una Operación <br />";
        //if (Txt_Clave_SAP.Text.Length < 1)  //Validar campo CLAVE SAP (minimo 1 caracteres)
        //{
        //    Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir la clave SAP de la partida <br />";
        //}
        //else if (Txt_Clave_SAP.Text.Length > 2)  //Validar campo CLAVE SAP (longitud menor a 3)
        //{
        //    Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Clave SAP no contenga más de 2 caracteres <br />";
        //}
        //if (Txt_Cuenta_SAP.Text.Length < 4)  //Validar campo Cuenta SAP (mayor a 4)
        //{
        //    Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir la Cuenta SAP de la Partida (por lo menos 4 caracteres) <br />";
        //}
        //else if (Txt_Cuenta_SAP.Text.Length > 17)  //Validar campo Cuenta SAP (longitud menor a 18)
        //{
        //    Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Campo Cuenta SAP no contenga más de 17 caracteres <br />";
        //}
        //if (Cmb_Centro_Aplicacion_SAP.SelectedIndex < 1)
        //    Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar un Centro de aplicación <br />";
        //if (Cmb_Afecta_Area_Funcional.SelectedIndex < 1)
        //   Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar si Afecta área funcional o no<br />";
        //if (Cmb_Afecta_Partida.SelectedIndex < 1)
        //    Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar si Afecta Partida o no<br />";
        //if (Cmb_Afecta_Elemento_PEP.SelectedIndex < 1)
        //   Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar si Afecta Elemento o no<br />";
        //if (Cmb_Afecta_Fondo.SelectedIndex < 1)
        //   Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar si Afecta Fondo o no<br />";


        if (Txt_Descripcion.Text == "")  //Validar campo DESCRIPCION (no vacío)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir la Descripción de la Partida <br />";
        }
        else if (Txt_Descripcion.Text.Length > 255)  //Validar campo DESCRIPCION (longitud menor a 250)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Campo Descripión no contenga más de 250 caracteres <br />";
        }
    }

    /////*******************************************************************************
    ///// NOMBRE DE LA FUNCION: Cargar_Combo_Giros
    ///// DESCRIPCION: Consulta los Giros dados de alta en la base de datos (Cat_Com_Giros)
    ///// PARAMETROS: 
    ///// CREO: Roberto González Oseguera
    ///// FECHA_CREO: 28-Feb-2011
    ///// MODIFICO:
    ///// FECHA_MODIFICO:
    ///// CAUSA_MODIFICACION:
    /////*******************************************************************************
    //private void Cargar_Combo_Giros()
    //{
    //    DataTable Dt_Giros; //Variable que obtendrá los datos de la consulta        
    //    Cls_Cat_Com_Giros_Negocio Rs_Consulta_Cat_Giro = new Cls_Cat_Com_Giros_Negocio(); //Variable de conexión hacia la capa de Negocios

    //    try
    //    {
    //        Dt_Giros = Rs_Consulta_Cat_Giro.Consulta_Giros(); //Consulta todos los Giros que estan dadas de alta en la BD
    //        Cmb_Giro.DataSource = Dt_Giros;
    //        Cmb_Giro.DataValueField = Cat_Com_Giros.Campo_Giro_ID;
    //        Cmb_Giro.DataTextField = Cat_Com_Giros.Campo_Nombre;
    //        Cmb_Giro.DataBind();
    //        Cmb_Giro.Items.Insert(0, "----- < SELECCIONE > -----");
    //        Cmb_Giro.SelectedIndex = 0;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception("Cargar_Combo_Giros " + ex.Message.ToString(), ex);
    //    }
    //}

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Combo_Capitulos
    /// DESCRIPCION: Consulta los Capítulos dados de alta en la base de datos
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 28-Feb-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Combo_Capitulos()
    {
        DataTable Dt_Capitulos; //Variable que obtendrá los datos de la consulta        
        Cls_Cat_Com_Partidas_Negocio Rs_Consulta_Cat_SAP_Capitulos = new Cls_Cat_Com_Partidas_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {

            Dt_Capitulos = Rs_Consulta_Cat_SAP_Capitulos.Consulta_Capitulos(); //Consulta todos los capítulos que estan dadas de alta en la BD
            Cmb_Capitulo.DataSource = Dt_Capitulos;
            Cmb_Capitulo.DataValueField = Cat_SAP_Capitulos.Campo_Capitulo_ID;
            Cmb_Capitulo.DataTextField = Cat_SAP_Capitulos.Campo_Descripcion;
            Cmb_Capitulo.DataBind();
           // Cmb_Capitulo.Items.Clear();
            Cmb_Capitulo.Items.Insert(0, "----- < SELECCIONE > -----");

            //foreach (DataRow Registro in Dt_Capitulos.Rows) 
            //{
            //    Cmb_Capitulo.Items.Add(Registro[Cat_Com_Partidas.Campo_Descripcion].ToString());
            //}
            Cmb_Capitulo.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Combo_Capitulos " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Combo_Conceptos
    /// DESCRIPCION: Consulta los Conceptos dados de alta en la base de datos
    ///         (filtrados por capítulo)
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 28-Feb-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Combo_Conceptos()
    {
        DataTable Dt_Conceptos; //Variable que obtendrá los datos de la consulta        
        Cls_Cat_Com_Partidas_Negocio Rs_Consulta_Cat_SAP_Conceptos = new Cls_Cat_Com_Partidas_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {
            if (Cmb_Capitulo.SelectedIndex > 0) //Almacenar el valor seleccionado en el combo Capítulo si es mayor a 0
            {
                Rs_Consulta_Cat_SAP_Conceptos.P_Capitulo_ID = Cmb_Capitulo.SelectedValue;
                Dt_Conceptos = Rs_Consulta_Cat_SAP_Conceptos.Consulta_Conceptos(); //Consulta todos los Conceptos que estan dados de alta en la BD
                Cmb_Concepto.DataSource = Dt_Conceptos;
                Cmb_Concepto.DataValueField = Cat_Sap_Concepto.Campo_Concepto_ID;
                Cmb_Concepto.DataTextField = Cat_Sap_Concepto.Campo_Descripcion;
                Cmb_Concepto.ToolTip = "Descripcion";
                Cmb_Concepto.DataBind();
                Cmb_Concepto.Items.Insert(0, "----- < SELECCIONE > -----");
            }
            else
            {       // Si no hay un capítulo seleccionado, borrar los elementos del combo Conceptos
                if (Cmb_Concepto.SelectedIndex != -1)         //si hay un elemento seleccionado en el combo, seleccionar al elemento 0
                    Cmb_Concepto.SelectedIndex = 0;
                Cmb_Concepto.DataSource = null;
                Cmb_Concepto.DataBind();
                Cmb_Concepto.Enabled = false;
                Cargar_Combo_Partida_Generica();        // llamar el método para limpiar el combo
            }

        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Combo_Concepto " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Combo_Partida_Generica
    /// DESCRIPCION: Consulta las Partidas genéricas que están dadas de alta en la base de datos
    ///         (filtradas por Concepto)
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 03-Feb-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Combo_Partida_Generica()
    {
        DataTable Dt_Partidas_Genericas; //Variable que obtendrá los datos de la consulta        
        Cls_Cat_Com_Partidas_Negocio Rs_Consulta_Cat_SAP_Partidas_Genericas = new Cls_Cat_Com_Partidas_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {
            if (Cmb_Concepto.SelectedIndex > 0) //Almacenar el índice seleccionado en el combo Concepto si es mayor a 0
            {
                Rs_Consulta_Cat_SAP_Partidas_Genericas.P_Concepto_ID = Cmb_Concepto.SelectedValue;
                Dt_Partidas_Genericas = Rs_Consulta_Cat_SAP_Partidas_Genericas.Consulta_Partidas_Genericas(); //Consulta todas las Partidas genéricas que estan dadas de alta en la BD
                Cmb_Partida_Generica.DataSource = Dt_Partidas_Genericas;
                Cmb_Partida_Generica.DataValueField = Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID;
                Cmb_Partida_Generica.DataTextField = "Clave_Descripcion";
                Cmb_Partida_Generica.DataBind();
                Cmb_Partida_Generica.Items.Insert(0, "----- < SELECCIONE > -----");
            }
            else
            {           //Si no hay un Concepto seleccionado, borrar los elementos del combo Cmb_Partida_Generica
                if (Cmb_Partida_Generica.SelectedIndex != -1)         //si hay un elemento seleccionado en el combo, seleccionar al elemento 0
                    Cmb_Partida_Generica.SelectedIndex = 0;
                Cmb_Partida_Generica.DataSource = null;
                Cmb_Partida_Generica.DataBind();
                Cmb_Partida_Generica.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Combo_Partidas_Genericas " + ex.Message.ToString(), ex);
        }
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
                ViewState["SortDirection"] = "DESC";
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
                Cmb_Capitulo.Focus();
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
                    Alta_Partida(); //Da de alta los datos proporcionados por el usuario
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
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 01-mar-2011
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
                if (Txt_Partida_ID.Value != "")
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                    Txt_Clave.Focus();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione la Partia cuyos datos desea modificar<br />";
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
                    Modificar_Partida(); //Actualizar los datos de la partida
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
    /// 	FECHA_CREO: 02-mar-2011
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
            if (Txt_Partida_ID.Value == "")
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione la Partida que desea inactivar<br />";
            }
            //Si el combo estatus es diferente a INACTIVO seleccionar INACTIVO y enviar cambios a la BD
            if (Cmb_Estatus.SelectedValue != "INACTIVO")
            {
                Cmb_Estatus.SelectedValue = "INACTIVO"; //Seleccionar inactivo en el combo estatus para que se guarde con los otros datos
                Modificar_Partida(); //Actualizar los datos de la Partida
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
    /// 	NOMBRE_FUNCIÓN: Btn_Buscar_Click
    /// 	DESCRIPCIÓN: Buscar Partidas en la base de datos por clave, nombre y descripcion
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 02-mar-2011
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
            Consulta_Partidas(); //Método que consulta los elementos en la base de datos
            Limpiar_Controles(); //Limpia los controles de la forma
            //Si no se encontraron Partidas con un nombre similar al proporcionado por el usuario entonces manda un mensaje al usuario
            Btn_Salir.ToolTip = "Regresar";
            if (Grid_Partidas.Rows.Count <= 0)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encontraron Partidas con el criterio proporcionado<br />";
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
    /// 	NOMBRE_FUNCIÓN: Cmb_Capitulo_SelectedIndexChanged
    /// 	DESCRIPCIÓN: Evento Cambio de índice en combo Cmb_Capitulo, activar y cargar el Cmb_Concepto con 
    /// 	            los Conceptos del Capítulo seleccionado
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 28-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cmb_Capitulo_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Si se selecciona capítulo del combo, activar el combo concepto
        if (Cmb_Capitulo.SelectedIndex > 0)
        {
            // Activar el combo Concepto y volverlo a cargar
            Cmb_Concepto.Enabled = true;
            Cmb_Partida_Generica.Enabled = false;
            Cargar_Combo_Conceptos();
            Cargar_Combo_Partida_Generica();
            Cmb_Concepto.Focus();
        }
        else
        {
            // Desactivar y limpiar el combo Concepto
            Cmb_Concepto.Enabled = false;
            Cmb_Partida_Generica.Enabled = false;
            Cargar_Combo_Conceptos();
            Cargar_Combo_Partida_Generica();
            Cmb_Capitulo.Focus();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Cmb_Concepto_SelectedIndexChanged
    /// 	DESCRIPCIÓN: Evento Cambio de índice en combo Cmb_Concepto, activar y cargar el Cmb_Partida_Generica con 
    /// 	            las Partidas Genéricas del Concepto seleccionado
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 28-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cmb_Concepto_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Si se selecciona concepto del combo, activar el combo partidas genericas
        if (Cmb_Concepto.SelectedIndex > 0)
        {
            // Activar el combo partidas genericas y volverlo a cargar
            Cmb_Partida_Generica.Enabled = true;
            Cargar_Combo_Partida_Generica();
            Cmb_Partida_Generica.Focus();
        }
        else
        {
            // Desactivar y limpiar el combo partidas genericas
            Cargar_Combo_Partida_Generica();
            Cmb_Partida_Generica.Enabled = false;
            Cmb_Concepto.Focus();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Cmb_Partida_Generica_SelectedIndexChanged
    /// 	DESCRIPCIÓN: Evento Cambio de índice en combo Cmb_Partida_Generica para enviar el foco al campo Clave
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 03-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cmb_Partida_Generica_SelectedIndexChanged(object sender, EventArgs e)
    {
        Txt_Clave.Focus();
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Grid_Partidas_SelectedIndexChanged
    /// 	DESCRIPCIÓN: Consulta los datos del elemento que seleccionó el usuario y los muestra 
    /// 	            en los campos correspondientes
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 01-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Partidas_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Com_Partidas_Negocio Rs_Consulta_Cat_Com_Partidas = new Cls_Cat_Com_Partidas_Negocio(); //Variable de conexión a la capa de Negocios para la consulta de los datos del Producto
        Cls_Cat_SAP_Capitulos_Negocio Rs_Capitulos = new Cls_Cat_SAP_Capitulos_Negocio();
        Cls_Cat_Com_Partidas_Negocio Rs_Conceptos = new Cls_Cat_Com_Partidas_Negocio(); //Variable de conexión hacia la capa de Negocios
        Cls_Cat_Sap_Partida_Generica_Negocio Rs_Genericas = new Cls_Cat_Sap_Partida_Generica_Negocio();
        DataTable Dt_Partidas; //Variable que obtendrá los datos de la consulta
        DataTable Dt_IDs_Partida;
        DataTable Dt_Capitulo = new DataTable();
        String Partida_Generica_ID;
        String Clave;
        String Capitulo;
        String Concepto;
        String Genericas;
        
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;


            if (Grid_Partidas.SelectedIndex > (-1))
            {
               
                GridViewRow selectedRow = Grid_Partidas.Rows[Grid_Partidas.SelectedIndex];

                Partida_Generica_ID = HttpUtility.HtmlDecode(selectedRow.Cells[2].Text).ToString();

                //para capitulos
                Clave = Partida_Generica_ID.Substring(0, 1);
                Capitulo =  Clave +"000";

                Rs_Capitulos.P_Clave = Capitulo;
                Dt_Capitulo = Rs_Capitulos.Consulta_Capitulos();
                Cargar_Combo_Capitulos();
                
                foreach (DataRow Registro in Dt_Capitulo.Rows)
                {
                    Capitulo = Registro[Cat_SAP_Capitulos.Campo_Descripcion].ToString();
                    Cmb_Capitulo.SelectedIndex = Cmb_Capitulo.Items.IndexOf(Cmb_Capitulo.Items.FindByText(Capitulo));
                    //Cmb_Capitulo.Items.IndexOf(Cmb_Capitulo.Items.FindByValue(Capitulo));
                    
                    
                }
                Cargar_Combo_Conceptos();
                Cargar_Combo_Partida_Generica();
                //Clave = Cmb_Capitulo.SelectedValue;
                //Cmb_Concepto.SelectedIndex=2;
                Clave = Cmb_Concepto.SelectedValue;
                //para concepto
                Clave = Partida_Generica_ID.Substring(0, 2);
                Concepto = Clave + "00";
                Rs_Conceptos.P_Clave = Concepto;
               
                Dt_Partidas = Rs_Conceptos.Consulta_Conceptos();
                foreach (DataRow Registro in Dt_Partidas.Rows)
                {
                    Concepto = Registro[Cat_Sap_Concepto.Campo_Concepto_ID].ToString();
                    Cmb_Concepto.SelectedIndex = Cmb_Concepto.Items.IndexOf(Cmb_Concepto.Items.FindByValue(Concepto));

                }

                //para genericas
                //Cargar_Combo_Partida_Generica();
                Cargar_Combo_Partida_Generica();
                
                Clave = Partida_Generica_ID.Substring(0, 3);
                Genericas = Clave + "0";
                Rs_Genericas.P_Clave = Genericas;
                Dt_Partidas.Clear();
                Cargar_Combo_Partida_Generica();
                Dt_Partidas = Rs_Genericas.Consultar_Partidas_Genericas();
                foreach (DataRow Registro in Dt_Partidas.Rows)
                {
                    Genericas = Registro[Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID].ToString();
                    Cmb_Partida_Generica.SelectedIndex = Cmb_Partida_Generica.Items.IndexOf(Cmb_Partida_Generica.Items.FindByValue(Genericas));

                }
               

                Rs_Consulta_Cat_Com_Partidas.P_Clave = Partida_Generica_ID;
                Dt_Partidas = Rs_Consulta_Cat_Com_Partidas.Consulta_Datos_Partidas();

                foreach (DataRow Registro in Dt_Partidas.Rows)
                {
                    String Estatus = Registro[Cat_Com_Partidas.Campo_Estatus].ToString();
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Estatus));
                    Txt_Descripcion.Text = Registro[Cat_Com_Partidas.Campo_Descripcion].ToString();
                    Txt_Nombre_Partida.Text = Registro[Cat_Com_Partidas.Campo_Nombre].ToString();
                    Txt_Clave.Text = Registro[Cat_Com_Partidas.Campo_Clave].ToString();
                    Txt_Cuenta_SAP.Text = Registro[Cat_Com_Partidas.Campo_Cuenta_SAP].ToString();
                    Txt_Descripcion_Especifica.Text = Registro[Cat_Com_Partidas.Campo_Descripcion_Especifica].ToString();
                    Rs_Genericas.P_Partida_Generica_ID = Registro[Cat_Com_Partidas.Campo_Partida_Generica_ID].ToString();
                    Txt_Partida_ID.Value = Registro[Cat_Com_Partidas.Campo_Partida_ID].ToString();
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
    /// 	NOMBRE_FUNCIÓN: Grid_Partidas_PageIndexChanging
    /// 	DESCRIPCIÓN: Manejo del evento de paginación del grid (cargar los datos de la página seleccionada)
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 02-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Partidas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpiar_Controles(); //Limpia todos los controles de la forma
            Grid_Partidas.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Llena_Grid_Partidas(); //Carga los elementos que están asignados a la página seleccionada
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }






    /// ********************************************************************************
    /// NOMBRE: Grid_Partidas_Sorting
    /// 
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// 
    /// CREÓ:   Hugo Enrique Ramirez Aguilera
    /// FECHA CREÓ: 03-Noviembre-2011
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **********************************************************************************
    protected void Grid_Partidas_Sorting(object sender, GridViewSortEventArgs e)
    {
        //Se consultan los movimientos que actualmente se encuentran registradas en el sistema.
        Consultar_Grid_Partidas();
        DataTable Dt_Partidas = (Grid_Partidas.DataSource as DataTable);

        if (Dt_Partidas != null)
        {
            DataView Dv_Partidas = new DataView(Dt_Partidas);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Partidas.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Partidas.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Partidas.DataSource = Dv_Partidas;
            Grid_Partidas.DataBind();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Grid_Partidas
    /// DESCRIPCION : Llena el grid con las partidas especificas que se encuentran en la 
    ///               base de datos
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramirez Aguilera
    /// FECHA_CREO  : 03-Noviemre-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Grid_Partidas()
    {
        Cls_Cat_Com_Partidas_Negocio Rs_Consulta_Cat_Com_Partidas = new Cls_Cat_Com_Partidas_Negocio(); //Variable de conexión a la capa de Negocios para la consulta de los datos del Producto
        DataTable Dt_Partida = null; //Variable que obtendra los datos de la consulta 
        try
        {
            Dt_Partida = Rs_Consulta_Cat_Com_Partidas.Consulta_Datos_Partidas();
            Session["Consulta_Partida_Especifica"] = Dt_Partida;
            Grid_Partidas.DataSource = (DataTable)Session["Consulta_Partida_Especifica"];
            Grid_Partidas.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar Partida " + ex.Message.ToString(), ex);
        }
    }

#endregion EVENTOS
}
