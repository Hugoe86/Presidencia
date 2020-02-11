using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.IO;
using System.Data.OleDb;
using Microsoft.Office.Interop.Excel;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.SAP_Operacion_Departamento_Presupuesto.Negocio;
using Presidencia.Capitulos.Negocio;

public partial class paginas_Paginas_Generales_Frm_Ope_SAP_Dep_Presupuesto : System.Web.UI.Page
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
    /// 	FECHA_CREO: 03-mar-2011
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
            Cargar_Combo_Dependencias();
            
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
    /// 	FECHA_CREO: 03-mar-2011
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
                    Btn_Importar_Archivo_Excel.Visible = true;
                    Btn_Importar_Archivo_Excel.CausesValidation = false;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Cmb_Dependencia.Enabled = true;
                    Txt_Monto_Presupuestal.Enabled = false;
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
                    Btn_Importar_Archivo_Excel.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Cmb_Dependencia.Enabled = true;
                    Txt_Monto_Presupuestal.Enabled = true;
                    Txt_Anio.Text = DateTime.Now.Year.ToString();
                    Txt_Numero_Asignacion.Text = "1";
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
                    Btn_Importar_Archivo_Excel.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Cmb_Dependencia.Enabled = false;
                    Txt_Monto_Presupuestal.Enabled = true;
                    break;
            }

            ///Habilitar campos de texto para edición o inhabilitar si no se requieren de acuerdo con variable Habilitado

            //Txt_Anio.Enabled = false;
            Txt_Comentarios.Enabled = Habilitado;
            Grid_Presupuestos.Enabled = !Habilitado;
            Grid_Presupuestos.DataSource = null;    // Borrar datos del grid
            Grid_Presupuestos.DataBind();
            Txt_Busqueda.Enabled = !Habilitado;     //deshabilitar la búsqueda mientras se editan los datos
            //Combos
            Cmb_Fuente_Financiamiento.Enabled = false;
            Cmb_Programa.Enabled = false;
            Cmb_Partida.Enabled = false;

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
    /// 	FECHA_CREO: 04-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Txt_Presupuesto_ID.Value = "";
            Txt_Busqueda.Text = "";
            Txt_Anio.Text = "";
            Txt_Numero_Asignacion.Text = "";
            Txt_Monto_Presupuestal.Text = "";
            Txt_Ejercido.Text = "";
            Txt_Disponible.Text = "";
            Txt_Comprometido.Text = "";
            Txt_Comentarios.Text = "";
            Lbl_Presupuesto_Partida.Text = "";
            Cmb_Dependencia.SelectedIndex = 0;
            Cmb_Fuente_Financiamiento.SelectedIndex = -1;
            Cmb_Programa.SelectedIndex = -1;
            Cmb_Partida.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
        }
    }


    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Combo_Dependencias
    /// DESCRIPCION: Consulta las Dependencias dadas de alta en la base de datos (CAT_DEPENDENCIAS)
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-mar-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Combo_Dependencias()
    {
        System.Data.DataTable Dt_Dependencias; //Variable que obtendrá los datos de la consulta        
        Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Consulta_Cat_Dependencias = new Cls_Ope_SAP_Dep_Presupuesto_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {
            Dt_Dependencias = Rs_Consulta_Cat_Dependencias.Consulta_Dependencias(); //Consulta las Dependencias que estan dadas de alta en la BD
            Cmb_Dependencia.DataSource = Dt_Dependencias;
            Cmb_Dependencia.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
            Cmb_Dependencia.DataTextField = Cat_Dependencias.Campo_Nombre;
            Cmb_Dependencia.DataBind();
            Cmb_Dependencia.Items.Insert(0, "----- < SELECCIONE > -----");
            Cmb_Dependencia.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Combo_Dependencias " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Combo_Fuentes_Financiamiento
    /// DESCRIPCION: Consulta las Fuentes_Financiamiento dadas de alta en la base de 
    ///         datos (Cat_SAP_Fuente_Financiamiento) filtradas por Dependencia, 
    ///         mediante CAT_SAP_DET_FTE_DEPENDENCIA
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-mar-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Combo_Fuentes_Financiamiento()
    {
        System.Data.DataTable Dt_Fuentes_Financiamiento; //Variable que obtendrá los datos de la consulta        
        Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Consulta_Cat_SAP_Fuentes_Financiamiento = new Cls_Ope_SAP_Dep_Presupuesto_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {
            if (Cmb_Dependencia.SelectedIndex > 0)   //Si hay una dependencia seleccionada, asignar el ID de la dependencia a la clase de negocio
                Rs_Consulta_Cat_SAP_Fuentes_Financiamiento.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;

            //Consulta las fuentes de financiamiento que estan dadas de alta en la BD
            Dt_Fuentes_Financiamiento = Rs_Consulta_Cat_SAP_Fuentes_Financiamiento.Consulta_Fuente_Financiamiento();
            Cmb_Fuente_Financiamiento.DataSource = Dt_Fuentes_Financiamiento;
            Cmb_Fuente_Financiamiento.DataValueField = Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID;
            Cmb_Fuente_Financiamiento.DataTextField = "Clave_Y_Descripcion";
            Cmb_Fuente_Financiamiento.DataBind();
            Cmb_Fuente_Financiamiento.Items.Insert(0, "----- < SELECCIONE > -----");
            Cmb_Fuente_Financiamiento.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Combo_Fuentes_Financiamiento " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Combo_Programas
    /// DESCRIPCION: Consulta los Programas dados de alta en la base de datos 
    ///             (CAT_SAP_PROYECTOS_PROGRAMAS filtrados por Dependencia 
    ///             mediante CAT_SAP_DET_PROG_DEPENDENCIA)
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-mar-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Combo_Programas()
    {
        System.Data.DataTable Dt_Programas; //Variable que obtendrá los datos de la consulta        
        Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Consulta_Cat_SAP_Programas = new Cls_Ope_SAP_Dep_Presupuesto_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {
            if (Cmb_Dependencia.SelectedIndex > 0)   //Si hay una dependencia seleccionada, asignar el ID de la dependencia a la clase de negocio
                Rs_Consulta_Cat_SAP_Programas.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;

            //Consulta todos los Programas que estan dadas de alta en la BD
            Dt_Programas = Rs_Consulta_Cat_SAP_Programas.Consulta_Programas();
            Cmb_Programa.DataSource = Dt_Programas;
            Cmb_Programa.DataValueField = Cat_SAP_Det_Prog_Dependencia.Campo_Proyecto_Programa_ID;
            Cmb_Programa.DataTextField = "Clave_Y_Nombre";
            Cmb_Programa.DataBind();
            Cmb_Programa.Items.Insert(0, "----- < SELECCIONE > -----");
            Cmb_Programa.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Combo_Programas " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: 
    /// DESCRIPCION: Consulta los Programas dados de alta en la base de datos 
    ///             (CAT_SAP_PROYECTOS_PROGRAMAS filtrados por Dependencia 
    ///             mediante CAT_SAP_DET_PROG_DEPENDENCIA)
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-mar-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Combo_Partidas()
    {
        System.Data.DataTable Dt_Partidas; //Variable que obtendrá los datos de la consulta        
        Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Consulta_Cat_SAP_Partidas = new Cls_Ope_SAP_Dep_Presupuesto_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {
            if (Cmb_Programa.SelectedIndex > 0)   //Si hay un Programa seleccionado, asignar el ID del Programa a la clase de negocio
                Rs_Consulta_Cat_SAP_Partidas.P_Programa_ID = Cmb_Programa.SelectedValue;

            //Consulta las Partidas que estan dadas de alta en la BD
            Dt_Partidas = Rs_Consulta_Cat_SAP_Partidas.Consulta_Partidas();
            Cmb_Partida.DataSource = Dt_Partidas;
            Cmb_Partida.DataValueField = Cat_Sap_Det_Prog_Partidas.Campo_Det_Partida_ID;
            Cmb_Partida.DataTextField = "Clave_Y_Nombre";
            Cmb_Partida.DataBind();
            Cmb_Partida.Items.Insert(0, "----- < SELECCIONE > -----");
            Cmb_Partida.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Combo_Partidas " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Consulta_Presupuestos
    /// 	DESCRIPCIÓN: Consulta los Presupuestos que están dadas de alta en la BD
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 04-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Consulta_Presupuestos()
    {
        Cls_Ope_SAP_Dep_Presupuesto_Negocio RS_Consulta_Ope_Cat_Partidas = new Cls_Ope_SAP_Dep_Presupuesto_Negocio(); //Variable de conexión hacia la capa de Negocios
        System.Data.DataTable Dt_Partidas; //Variable que obtendrá los datos de la consulta 

        try
        {
            if (Cmb_Dependencia.SelectedIndex > 0) // Si hay una dependencia seleccionada, pasar el ID de la dependencia para filtrar
            {
                RS_Consulta_Ope_Cat_Partidas.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
            }
            if (Txt_Presupuesto_ID.Value != "" && Btn_Nuevo.ToolTip != "Nuevo" && Btn_Modificar.ToolTip != "Modificar")     //Si hay un registro seleccionado, pasar el ID para filtrar por ese ID
            {
                RS_Consulta_Ope_Cat_Partidas.P_Presupuesto_ID = Convert.ToInt32(Txt_Presupuesto_ID.Value);
                RS_Consulta_Ope_Cat_Partidas.P_Dependencia_ID = null; //si hay una Dependencia seleccionada, ignorarla
            }
            if (Txt_Busqueda.Text != "")
            {
                RS_Consulta_Ope_Cat_Partidas.P_Busqueda = Txt_Busqueda.Text;
            }
            Dt_Partidas = RS_Consulta_Ope_Cat_Partidas.Consulta_Datos_Presupuestos(); //Consulta las Partidas con sus datos generales
            Session["Consulta_Dep_Presupuestos"] = Dt_Partidas;
            Llena_Grid_Presupuestos();
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Presupuestos " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Llena_Grid_Presupuestos
    /// 	DESCRIPCIÓN: Llena el grid con los Presupuestos de la base de datos
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 04-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Llena_Grid_Presupuestos()
    {
        System.Data.DataTable Dt_Presupuestos; //Variable que obtendrá los datos de la consulta 
        try
        {
            Grid_Presupuestos.DataBind();
            Dt_Presupuestos = (System.Data.DataTable)Session["Consulta_Dep_Presupuestos"];
            Grid_Presupuestos.Columns[1].Visible = true;
            Grid_Presupuestos.DataSource = Dt_Presupuestos;
            Grid_Presupuestos.DataBind();
            Grid_Presupuestos.Columns[1].Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Presupuestos " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Alta_Presupuesto
    /// 	DESCRIPCIÓN: Dar de alta un Presupuesto con los datos que proporcionó el usuario
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 02-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Alta_Presupuesto()
    {
        Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Presupuesto = new Cls_Ope_SAP_Dep_Presupuesto_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            Rs_Presupuesto.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
            Rs_Presupuesto.P_Fuente_Financiamiento_ID = Cmb_Fuente_Financiamiento.SelectedValue;
            Rs_Presupuesto.P_Programa_ID = Cmb_Programa.SelectedValue;
            Rs_Presupuesto.P_Partida_ID = Cmb_Partida.SelectedValue;
            Rs_Presupuesto.P_Anio = Txt_Anio.Text;
            Rs_Presupuesto.P_Numero_Asignacion = Txt_Numero_Asignacion.Text;
            Rs_Presupuesto.P_Monto_Presupuestal = Txt_Monto_Presupuestal.Text;
            Rs_Presupuesto.P_Ejercido = Txt_Ejercido.Text;
            Rs_Presupuesto.P_Disponible = Txt_Disponible.Text;
            Rs_Presupuesto.P_Comprometido = Txt_Comprometido.Text;
            Rs_Presupuesto.P_Comentarios = Txt_Comentarios.Text;
            Rs_Presupuesto.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

            Rs_Presupuesto.Alta_Presupuestos(); //Da de alta los datos del Presupuesto proporcionados por el usuario en la BD
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Asignación de Presupuesto ", "alert('El Alta del Presupuesto fue Exitosa');", true);
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
        }
        catch (Exception Ex)
        {
            throw new Exception("Alta_Presupuesto " + Ex.Message.ToString(), Ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Validar_Campos
    /// 	DESCRIPCIÓN: Revisar que los campos obligatorios hayan sido llenados y si no, generar el mensaje 
    /// 	            correspondiente.
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 05-mar-2011
    /// 	MODIFICÓ: Roberto González Oseguera
    /// 	FECHA_MODIFICÓ: 28-abr-2011
    /// 	CAUSA_MODIFICACIÓN: Se comentaron las validaciones de anio y presupuesto
    ///*******************************************************************************************************
    private void Validar_Campos()
    {
        //Double Monto_Presupuestal;
        //Double Monto_Disponible;

        //Si falta alguno de los campos mencionarlo en la etiqueta Lbl_Mensaje_Error para mostrarla 
        Lbl_Mensaje_Error.Text = "";
        if (Cmb_Dependencia.SelectedIndex <= 0)  //Validar combo Unidad
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar la Unidad Responsable<br />";
        }
        else if (Cmb_Programa.SelectedIndex <= 0)  //Validar combo Programa
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar un Programa<br />";
        }
        else if (Cmb_Partida.SelectedIndex <= 0)  //Validar combo Partida
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar una Partida<br />";
        }
        if (Cmb_Fuente_Financiamiento.SelectedIndex <= 0)  //Validar combo Fuente_Financiamiento
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar la Fuente de Financiamiento<br />";
        }
        //if (Txt_Anio.Text.Length != 4)  //Validar campo AÑO de la Presupuesto (longitud diferente de 4)
        //{
        //    Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir un año (4 dígitos) para el Presupuesto<br />";
        //}
        if (Txt_Monto_Presupuestal.Text.Length <= 0 || Txt_Disponible.Text.Length <= 0)  //Validar campo Monto presupuestal de Presupuesto (longitud diferente de 4)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir el Monto Presupuestal a asignar<br />";
        }
        if (Txt_Comentarios.Text.Length > 250)  //Validar campo COMENTARIOS (longitud menor a 250)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Que los Comentarios no contengan más de 250 caracteres <br />";
        }
                    //verificar que la cantidad en Monto Presupuestal no sea mayor que el monto disponible de la partida
        //if (Txt_Anio.Text.Length == 4 && Txt_Monto_Presupuestal.Text.Length > 0)
        //{
        //    System.Data.DataTable Dt_Pres_Partida;
        //    Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Consulta_Pres_Partida = new Cls_Ope_SAP_Dep_Presupuesto_Negocio();

        //    try
        //    {
        //        if (Cmb_Partida.SelectedIndex > 0 && Cmb_Programa.SelectedIndex > 0)
        //        {
        //            Rs_Consulta_Pres_Partida.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
        //            Rs_Consulta_Pres_Partida.P_Programa_ID = Cmb_Programa.SelectedValue;
        //            Rs_Consulta_Pres_Partida.P_Partida_ID = Cmb_Partida.SelectedValue;
        //            Rs_Consulta_Pres_Partida.P_Anio = Txt_Anio.Text;
        //            Dt_Pres_Partida = Rs_Consulta_Pres_Partida.Consulta_Ope_Pres_Partidas();
        //            // verficar que se recibieron datos
        //            if (Dt_Pres_Partida.Rows.Count > 0)
        //            {
        //                Monto_Disponible = Convert.ToDouble(Dt_Pres_Partida.Rows[0][Ope_Com_Pres_Partida.Campo_Monto_Disponible].ToString());
        //                Monto_Presupuestal = Convert.ToDouble(Txt_Monto_Presupuestal.Text);


        //                if (Btn_Nuevo.ToolTip == "Dar de Alta")     // si es alta de registro, ver que el monto disponible no sea menor que el monto Presupuestal
        //                {
        //                    //Si el monto disponible de la partida es menor que el valor en Monto presupuestal, mostrar mensaje
        //                    if (Monto_Disponible < Monto_Presupuestal)
        //                    {
        //                        Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + El Monto Presupuestal excede el Monto disponible de la Partida seleccionada <br />";
        //                    }
        //                }
        //                else if (Btn_Modificar.ToolTip == "Actualizar")
        //                {       //si el monto disponible de la partida más el monto anterior del Presupuesto modificado es es menor que el nuevo valor en Monto Presupuestal, mostrar mensaje
        //                        //Esta validacion se comentó devido a los cambios solicitados por el usuario final
        //                    //if ((Monto_Disponible + Convert.ToDouble(Txt_Monto_Presupuestal_Anterior.Value)) < Monto_Presupuestal)
        //                    //{
        //                    //    Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + El Monto Presupuestal excede el Monto disponible de la Partida seleccionada <br />";
        //                    //}
        //                }
        //            }
        //            else        // Si no sobtuvieron valores de la consulta, no hay un presupuesto con los criterios seleccionados
        //            {
        //                Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + La Partida seleccionada no tiene presupuesto asignado<br />";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Validar Pres_Partida " + ex.Message.ToString(), ex);
        //    }
        //}
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Extraer_Numero
    /// 	DESCRIPCIÓN: Mediante una expresión regular encuentra números en el texto
    /// 	PARÁMETROS:
    /// 		1. Texto: Texto en el que se va a buscar un número
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 05-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private String Extraer_Numero(String Texto)
    {
        Regex Rge_Decimal = new Regex(@"(?<entero>[0-9]{1,12})(?:\.[0-9]{0,4})?");
        Match Numero_Encontrado = Rge_Decimal.Match(Texto);
        if (Numero_Encontrado.Value != "")
            return Numero_Encontrado.Value;
        else
            return "0";
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Consulta_Presupuesto_Partida
    /// 	DESCRIPCIÓN: Consultar Ope_SAP_Pres_Partidas, para mostrar el monto disponible de la partida 
    /// 	        seleccionada
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 05-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Consulta_Presupuesto_Partida()
    {
        System.Data.DataTable Dt_Pres_Partida;
        Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Consulta_Pres_Partida = new Cls_Ope_SAP_Dep_Presupuesto_Negocio();

        try
        {
            Lbl_Presupuesto_Partida.Text = "";
            // Consultar sólo si se han llenado todos los datos requeridos ()
            if (Cmb_Partida.SelectedIndex > 0 && Cmb_Programa.SelectedIndex > 0 && Txt_Anio.Text.Length == 4)
            {
                Rs_Consulta_Pres_Partida.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
                Rs_Consulta_Pres_Partida.P_Programa_ID = Cmb_Programa.SelectedValue;
                Rs_Consulta_Pres_Partida.P_Partida_ID = Cmb_Partida.SelectedValue;
                Rs_Consulta_Pres_Partida.P_Anio = Txt_Anio.Text;
                Dt_Pres_Partida = Rs_Consulta_Pres_Partida.Consulta_Ope_Pres_Partidas();
                // verficar que se recibieron datos
                if (Dt_Pres_Partida.Rows.Count > 0)
                {
                    //Si el monto disponible de la partida es mayor a cero, mostrar el monto y activar el campo de texto Monto Presupuesto
                    if (Convert.ToDouble(Dt_Pres_Partida.Rows[0][Ope_Com_Pres_Partida.Campo_Monto_Disponible].ToString()) > 0)
                    {
                        //Linea comentada devido a cambios solicitados por el cliente
                        //Lbl_Presupuesto_Partida.Text = string.Format("El monto disponible para la Partida seleccionada es: {0:C}", 
                        //Convert.ToDouble(Dt_Pres_Partida.Rows[0][Ope_Com_Pres_Partida.Campo_Monto_Disponible].ToString()));
                        Txt_Monto_Presupuestal.Enabled = true;
                    }
                    if ((Convert.ToDouble(Dt_Pres_Partida.Rows[0][Ope_Com_Pres_Partida.Campo_Monto_Disponible].ToString()) <= 0) || (Dt_Pres_Partida.Rows[0][Ope_Com_Pres_Partida.Campo_Monto_Disponible].ToString() == ""))
                    //else        //si no, mostrar un mensaje al usuario y limpiar desactivar el campo de texto Monto Presupuesto
                    {
                        Lbl_Presupuesto_Partida.Text = "La Partida seleccionada no tiene Presupuesto asignado para el año proporcionado.";
                        Lbl_Presupuesto_Partida.Visible = true;
                        if (Btn_Modificar.ToolTip == "Actualizar")      //Para una actualización, se permite editar monto Presupuestal
                        {
                            Txt_Monto_Presupuestal.Enabled = true;
                        }
                        else
                        {
                            Txt_Monto_Presupuestal.Text = "";
                            Txt_Monto_Presupuestal.Enabled = false;
                        }
                    }
                }
                else
                {
                    Txt_Monto_Presupuestal.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Consulta_Presupuesto_Partida: " + Ex.Message);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Modificar_Presupuesto_Asignado
    /// 	DESCRIPCIÓN: Modifica los datos del Presupesto con los datos que introdujo el usuario
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 07-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Modificar_Presupuesto_Asignado()
    {
        Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Modificar_Ope_SAP_Dep_Presupuesto = new Cls_Ope_SAP_Dep_Presupuesto_Negocio(); //Variable de conexión hacia la capa de Negoccios para envio de datos a modificar
        try
        {
            Rs_Modificar_Ope_SAP_Dep_Presupuesto.P_Presupuesto_ID = Convert.ToInt32(Txt_Presupuesto_ID.Value);
            Rs_Modificar_Ope_SAP_Dep_Presupuesto.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
            Rs_Modificar_Ope_SAP_Dep_Presupuesto.P_Fuente_Financiamiento_ID = Cmb_Fuente_Financiamiento.SelectedValue;
            Rs_Modificar_Ope_SAP_Dep_Presupuesto.P_Programa_ID = Cmb_Programa.SelectedValue;
            Rs_Modificar_Ope_SAP_Dep_Presupuesto.P_Partida_ID = Cmb_Partida.SelectedValue;
            Rs_Modificar_Ope_SAP_Dep_Presupuesto.P_Anio = Txt_Anio.Text;
            Rs_Modificar_Ope_SAP_Dep_Presupuesto.P_Numero_Asignacion = Txt_Numero_Asignacion.Text;
            Rs_Modificar_Ope_SAP_Dep_Presupuesto.P_Monto_Presupuestal = Txt_Monto_Presupuestal.Text;
            Rs_Modificar_Ope_SAP_Dep_Presupuesto.P_Ejercido = Txt_Ejercido.Text;
            Rs_Modificar_Ope_SAP_Dep_Presupuesto.P_Disponible = Txt_Disponible.Text;
            Rs_Modificar_Ope_SAP_Dep_Presupuesto.P_Comprometido = Txt_Comprometido.Text;
            Rs_Modificar_Ope_SAP_Dep_Presupuesto.P_Comentarios = Txt_Comentarios.Text;
            Rs_Modificar_Ope_SAP_Dep_Presupuesto.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

            Rs_Modificar_Ope_SAP_Dep_Presupuesto.Modificar_Dep_Presupuesto(); //Sustituye los datos que se encuentran en la BD por lo que introdujo el usuario
            
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Asignación de Presupuestos ", "alert('La Modificación del Presupuesto fue Exitosa');", true);
            
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Presupuesto_Asignado " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Sustituir_IDs_Presupuestos
    /// 	DESCRIPCIÓN: Cambiar el dato de la Clave por el ID correspondiente de Fuente de financiamiento, 
    /// 	            Programa, Partida, Unidad Responsable
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 11-abr-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    //private String Sustituir_IDs_Presupuestos()
    //{
    //    System.Data.DataTable Datos_Sincronizar = new System.Data.DataTable();
    //    Cls_Ope_SAP_Dep_Presupuesto_Negocio RS_Consulta_IDs = new Cls_Ope_SAP_Dep_Presupuesto_Negocio();
    //    Cls_Cat_SAP_Capitulos_Negocio Rs_Consulta_Capitulos = new Cls_Cat_SAP_Capitulos_Negocio();

    //    String Mensaje = "";

    //    Dictionary<String, String> Capitulos_Clave_ID_Diccio = new Dictionary<String, String>();

    //    Mpe_Cargar_Archivo.Show();

    //    Datos_Sincronizar = (System.Data.DataTable)Session["Tabla_Datos_Archivo"];  //Obtener tabla con datos de la variable de sesion

    //    //Llamar metodo que sustituye las claves de los campos Fuente de financiamiento, Programa, partida y área funcional
    //    Datos_Sincronizar = RS_Consulta_IDs.Consulta_IDs_De_Claves(Datos_Sincronizar, out Mensaje);

    //    //--------------<Sustituir capitulos
    //    foreach (DataRow Fila in Rs_Consulta_Capitulos.Consulta_Datos_Capitulos().Rows) //Para cada fila de los capitulos consultados
    //    {
    //        Capitulos_Clave_ID_Diccio.Add(                  //Agregar una entrada al diccionario de capitulos con la clave y el id del capitulo
    //            Fila[Cat_SAP_Capitulos.Campo_Clave].ToString().Trim(),
    //            Fila[Cat_SAP_Capitulos.Campo_Capitulo_ID].ToString().Trim());
    //    }
    //    foreach (DataRow Fila in Datos_Sincronizar.Rows)       // Sustituir valores en la tabla con el valor del diccionario
    //    {
    //        if (Capitulos_Clave_ID_Diccio.ContainsKey(Fila["UNIDAD_RESPONSABLE"].ToString()))
    //            Fila["UNIDAD_RESPONSABLE"] = Capitulos_Clave_ID_Diccio[Fila["UNIDAD_RESPONSABLE"].ToString()];
    //        else
    //            Mensaje += "La clave " + Fila["UNIDAD_RESPONSABLE"] + " (fila " + Fila[0] + ") no se encontró en el catálogo de Unidades responsables.<br />";
    //    }
    //    //--------------Sustituir capitulos>

    //    //Actualizar la variable de sesion con el Datatable que contiene los IDs
    //    Session["Tabla_Datos_Archivo"] = Datos_Sincronizar;

    //    return Mensaje;
    //}

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: buscar_IDs
    /// 	DESCRIPCIÓN: Busca el ID de cada Clave correspondiente de Fuente de financiamiento, 
    /// 	            Programa, Partida, Unidad Responsable
    /// 	            Regresa un mensaje en caso de no encontrar cualquiera de los IDs
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 30-abr-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private String buscar_IDs()
    {
        System.Data.DataTable Datos_Sincronizar = new System.Data.DataTable();
        Cls_Ope_SAP_Dep_Presupuesto_Negocio RS_Consulta_IDs = new Cls_Ope_SAP_Dep_Presupuesto_Negocio();
        Cls_Cat_SAP_Capitulos_Negocio Rs_Consulta_Capitulos = new Cls_Cat_SAP_Capitulos_Negocio();

        String Mensaje = "";

        Dictionary<String, String> Capitulos_Clave_ID_Diccio = new Dictionary<String, String>();

        Mpe_Cargar_Archivo.Show();

        Datos_Sincronizar = (System.Data.DataTable)Session["Tabla_Datos_Archivo"];  //Obtener tabla con datos de la variable de sesion

        //Llamar metodo que sustituye las claves de los campos Fuente de financiamiento, Programa, partida y área funcional
        Datos_Sincronizar = RS_Consulta_IDs.Consulta_IDs_De_Claves(Datos_Sincronizar, out Mensaje);

        //Actualizar la variable de sesion con el Datatable que contiene los IDs
        Session["Tabla_Datos_Archivo"] = Datos_Sincronizar;

        return Mensaje;
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Buscar_Filas_Duplicadas
    /// 	DESCRIPCIÓN: Recorre la tabla que se encuentra en la variable de sesion en busca de duplicados
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 12-abr-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private String Buscar_Filas_Duplicadas()
    {
        Dictionary<String, String> Lista_Datos = new Dictionary<string, String>();
        String Mensaje = "";
        System.Data.DataTable Datos = (System.Data.DataTable)Session["Tabla_Datos_Archivo"];

        try
        {
            foreach (System.Data.DataRow Fila in Datos.Rows)
            {
                String Datos_Concatenados = Fila[1] + "," + Fila[2] + "," + Fila[3] + "," + Fila[4] + "," + Fila[5] + "," + Fila[10];
                if (!Lista_Datos.ContainsKey(Datos_Concatenados))
                {
                    Lista_Datos.Add(Datos_Concatenados, Fila[0].ToString());
                }
                else
                {
                    Mensaje += "Las filas " + Fila[0] + " y " + Lista_Datos[Datos_Concatenados] + " son iguales.<br />";
                }
            }
            Lista_Datos.Clear();
            return Mensaje;
        }
        catch (Exception ex)
        {
            throw new Exception("Buscar_Filas_Duplicadas " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Alta_Presupuestos_Sincronizacion
    /// 	DESCRIPCIÓN: Da de alta los registros 
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 12-abr-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    //private void Alta_Presupuestos_Sincronizacion()
    //{
    //    String Mensaje;
    //    Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Alta_Sincronizacion = new Cls_Ope_SAP_Dep_Presupuesto_Negocio();
    //    System.Data.DataTable Datos_Sincronizar;
    //    int Numero_Inserciones;
    //    String Ruta_Archivo;

    //    Lbl_Mensaje_Error_Modal.Visible = false;
    //    Lbl_Mensaje_Error_Modal.Text = "";
    //    Img_Error_Modal.Visible = false;

    //        //Verificar que no haya filas duplicadas
    //    Mensaje = Buscar_Filas_Duplicadas();
    //    if (Mensaje.Length > 0) //Mostrar informacion de filas duplicadas
    //    {
    //        Lbl_Mensaje_Error_Modal.Visible = true;
    //        Lbl_Mensaje_Error_Modal.Text = Mensaje;
    //        Img_Error_Modal.Visible = true;
    //        Btn_Sincronizar_Presupuestos.Visible = false;
    //        Mpe_Cargar_Archivo.Show();
    //    }
    //    else        // Si no hay filas duplicadas, sustituir las Claves con los IDs
    //    {
    //        Mensaje = buscar_IDs();

    //        if (Mensaje.Length > 0)     //Si se recibio mensaje, no se encontro el ID de alguna(s) clave(s), se muestra el mensaje con los detalles
    //        {
    //            Lbl_Mensaje_Error_Modal.Visible = true;
    //            Lbl_Mensaje_Error_Modal.Text = Mensaje;
    //            Img_Error_Modal.Visible = true;
    //            Btn_Sincronizar_Presupuestos.Visible = false;
    //            Mpe_Cargar_Archivo.Show();
    //        }
    //        else        //Si no hay filas duplicadas, enviar datos para insercion en la base de datos
    //        {
    //            Datos_Sincronizar = (System.Data.DataTable)Session["Tabla_Datos_Archivo"];  //Obtener tabla con datos de la variable de sesion
    //            Ruta_Archivo = Hdn_Ruta_Archivo.Value; //Recuperar la ruta del archivo

    //            Rs_Alta_Sincronizacion.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
    //            if (Txt_Comentario_Modal.Text.Length > 250) //Si el comentario contiene mas de 250 caracteres, truncar a 250 caracteres
    //                Rs_Alta_Sincronizacion.P_Comentarios = Txt_Comentario_Modal.Text.Substring(0, 250);
    //            else
    //                Rs_Alta_Sincronizacion.P_Comentarios = Txt_Comentario_Modal.Text;
    //            Rs_Alta_Sincronizacion.P_Ruta_Archivo = Ruta_Archivo;
    //            Numero_Inserciones = Rs_Alta_Sincronizacion.Sincronizar_Datos(Datos_Sincronizar);
    //            if (Numero_Inserciones > 0) //Si hubo inserciones en la base de datos, mostrar el total
    //            {
    //                Grid_Datos_Archivo.DataSource = null;       //Vaciar datagrids
    //                Grid_Datos_Archivo.DataBind();
    //                Grid_Datos_Archivo_Modificar.DataSource = null;
    //                Grid_Datos_Archivo_Modificar.DataBind();
    //                Fila_Presupuestos_Actualizar.Style.Value = "display:none;";
    //                Fila_Presupuestos_Alta.Style.Value = "display:none;";
    //                Btn_Sincronizar_Presupuestos.Visible = false;
    //                Lbl_Comentario_Modal.Visible = false;
    //                Txt_Comentario_Modal.Text = "";
    //                Txt_Comentario_Modal.Visible = false;
    //                Mpe_Cargar_Archivo.Hide();
    //                Lbl_Resumen_Carga_Archivo.Text = "";
    //                Hdn_Ruta_Archivo.Value = "";
    //                ScriptManager.RegisterStartupScript(this, this.GetType(),
    //                    "Sincronización de Presupuestos", "alert('Se agregaron " + Numero_Inserciones +
    //                    " registros al catálogo Asignación de presupuestos.');", true);
    //            }
    //            else
    //            {
    //                Lbl_Mensaje_Error_Modal.Visible = true;
    //                Lbl_Mensaje_Error_Modal.Text = "Ocurrió un error al agregar los registros a la base de datos (0 registros guardados en la base de datos)";
    //                Img_Error_Modal.Visible = true;
    //                Btn_Sincronizar_Presupuestos.Visible = false;
    //            }
    //        }
    //    }
    //}
    
    
    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Sincronizacion_Datos
    /// 	DESCRIPCIÓN: Enviar informacion a la capa de datos para insercion y actualizacion
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 30-abr-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Sincronizacion_Datos()
    {
        Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Alta_Sincronizacion = new Cls_Ope_SAP_Dep_Presupuesto_Negocio();
        System.Data.DataTable Datos_Alta = (System.Data.DataTable)Session["Tabla_Dep_Pres_Altas"];
        System.Data.DataTable Datos_Actualizar = (System.Data.DataTable)Session["Tabla_Dep_Pres_Modificaciones"];
        int Total_Afectaciones;
        int[] Numero_Afectaciones;
        String Ruta_Archivo;

        Lbl_Mensaje_Error_Modal.Visible = false;
        Lbl_Mensaje_Error_Modal.Text = "";
        Img_Error_Modal.Visible = false;

        Ruta_Archivo = Hdn_Ruta_Archivo.Value; //Recuperar la ruta del archivo

        Rs_Alta_Sincronizacion.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
        if (Txt_Comentario_Modal.Text.Length > 250) //Si el comentario contiene mas de 250 caracteres, truncar a 250 caracteres
            Rs_Alta_Sincronizacion.P_Comentarios = Txt_Comentario_Modal.Text.Substring(0, 250);
        else
            Rs_Alta_Sincronizacion.P_Comentarios = Txt_Comentario_Modal.Text;
        Rs_Alta_Sincronizacion.P_Ruta_Archivo = Ruta_Archivo;
        Numero_Afectaciones = Rs_Alta_Sincronizacion.Sincronizar_Datos(Datos_Alta, Datos_Actualizar);
        Total_Afectaciones = Numero_Afectaciones[0] + Numero_Afectaciones[1];
        if (Total_Afectaciones > 0.0) //Si hubo inserciones en la base de datos, mostrar el total
        {
            Grid_Datos_Archivo.DataSource = null;       //Vaciar datagrids
            Grid_Datos_Archivo.DataBind();
            Grid_Datos_Archivo_Modificar.DataSource = null;
            Grid_Datos_Archivo_Modificar.DataBind();
            Fila_Presupuestos_Actualizar.Style.Value = "display:none;";
            Fila_Presupuestos_Alta.Style.Value = "display:none;";
            Btn_Sincronizar_Presupuestos.Visible = false;
            Lbl_Comentario_Modal.Visible = false;
            Txt_Comentario_Modal.Text = "";
            Txt_Comentario_Modal.Visible = false;
            Mpe_Cargar_Archivo.Hide();
            Lbl_Resumen_Carga_Archivo.Text = "";
            Hdn_Ruta_Archivo.Value = "";
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                "Sincronización de Presupuestos", "alert('Se sincronizaron " + Total_Afectaciones +
                " registros al catálogo Asignación de presupuestos. Nuevos Presupuestos: " + Numero_Afectaciones[0] +
                ". Presupuestos actualizados: " + Numero_Afectaciones[1] + "');", true);
        }
        else
        {
            Lbl_Mensaje_Error_Modal.Visible = true;
            Lbl_Mensaje_Error_Modal.Text = "Ocurrió un error al agregar los registros a la base de datos (0 registros guardados en la base de datos)";
            Img_Error_Modal.Visible = true;
            Btn_Sincronizar_Presupuestos.Visible = false;
        }
    }
    
    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Generar_Tabla_Datos
    /// 	DESCRIPCIÓN: Regresa un datatable con la estructura para contener los datos de 
    /// 	            presupuestos del archivo de sincronizacion
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 30-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected System.Data.DataTable Generar_Tabla_Datos()
    {
        DataColumn Columna0_Consecutivo;
        DataColumn Columna1_Fte_Fin;
        DataColumn Columna2_Area_Funcional;
        DataColumn Columna3_Programa;
        DataColumn Columna4_Unidad;
        DataColumn Columna5_Partida;
        DataColumn Columna6_Presupuesto;
        DataColumn Columna7_Disponible;
        DataColumn Columna8_Comprometido;
        DataColumn Columna9_Ejercido;
        DataColumn Columna10_Anio;
        DataColumn Columna11_Estatus;
        DataColumn Columna12_Fte_Fin_ID;
        DataColumn Columna13_Area_Funcional_ID;
        DataColumn Columna14_Programa_ID;
        DataColumn Columna15_Unidad_ID;
        DataColumn Columna16_Partida_ID;
        DataColumn Columna17_Dep_Presupuesto_ID;
        System.Data.DataTable Tabla_Datos = new System.Data.DataTable();

        // ---------- Inicializar columnas
        Columna0_Consecutivo = new DataColumn();
        Columna0_Consecutivo.DataType = System.Type.GetType("System.String");
        Columna0_Consecutivo.ColumnName = "CONSECUTIVO";
        Tabla_Datos.Columns.Add(Columna0_Consecutivo);
        Columna1_Fte_Fin = new DataColumn();
        Columna1_Fte_Fin.DataType = System.Type.GetType("System.String");
        Columna1_Fte_Fin.ColumnName = "FUENTE_FINACIAMIENTO";
        Tabla_Datos.Columns.Add(Columna1_Fte_Fin);
        Columna2_Area_Funcional = new DataColumn();
        Columna2_Area_Funcional.DataType = System.Type.GetType("System.String");
        Columna2_Area_Funcional.ColumnName = "AREA_FUNCIONAL";
        Tabla_Datos.Columns.Add(Columna2_Area_Funcional);
        Columna3_Programa = new DataColumn();
        Columna3_Programa.DataType = System.Type.GetType("System.String");
        Columna3_Programa.ColumnName = "PROGRAMA";
        Tabla_Datos.Columns.Add(Columna3_Programa);
        Columna4_Unidad = new DataColumn();
        Columna4_Unidad.DataType = System.Type.GetType("System.String");
        Columna4_Unidad.ColumnName = "UNIDAD_RESPONSABLE";
        Tabla_Datos.Columns.Add(Columna4_Unidad);
        Columna5_Partida = new DataColumn();
        Columna5_Partida.DataType = System.Type.GetType("System.String");
        Columna5_Partida.ColumnName = "PARTIDA";
        Tabla_Datos.Columns.Add(Columna5_Partida);
        Columna6_Presupuesto = new DataColumn();
        Columna6_Presupuesto.DataType = System.Type.GetType("System.String");
        Columna6_Presupuesto.ColumnName = "PRESUPUESTO";
        Tabla_Datos.Columns.Add(Columna6_Presupuesto);
        Columna7_Disponible = new DataColumn();
        Columna7_Disponible.DataType = System.Type.GetType("System.String");
        Columna7_Disponible.ColumnName = "DISPONIBLE";
        Tabla_Datos.Columns.Add(Columna7_Disponible);
        Columna8_Comprometido = new DataColumn();
        Columna8_Comprometido.DataType = System.Type.GetType("System.String");
        Columna8_Comprometido.ColumnName = "COMPROMETIDO";
        Tabla_Datos.Columns.Add(Columna8_Comprometido);
        Columna9_Ejercido = new DataColumn();
        Columna9_Ejercido.DataType = System.Type.GetType("System.String");
        Columna9_Ejercido.ColumnName = "EJERCIDO";
        Tabla_Datos.Columns.Add(Columna9_Ejercido);
        Columna10_Anio = new DataColumn();
        Columna10_Anio.DataType = System.Type.GetType("System.String");
        Columna10_Anio.ColumnName = "ANIO";
        Tabla_Datos.Columns.Add(Columna10_Anio);
        Columna11_Estatus = new DataColumn();
        Columna11_Estatus.DataType = System.Type.GetType("System.String");
        Columna11_Estatus.ColumnName = "ESTATUS";
        Tabla_Datos.Columns.Add(Columna11_Estatus);
        // ---------- Columnas para IDs
        Columna12_Fte_Fin_ID = new DataColumn();
        Columna12_Fte_Fin_ID.DataType = System.Type.GetType("System.String");
        Columna12_Fte_Fin_ID.ColumnName = "FUENTE_FINACIAMIENTO_ID";
        Tabla_Datos.Columns.Add(Columna12_Fte_Fin_ID);
        Columna13_Area_Funcional_ID = new DataColumn();
        Columna13_Area_Funcional_ID.DataType = System.Type.GetType("System.String");
        Columna13_Area_Funcional_ID.ColumnName = "AREA_FUNCIONAL_ID";
        Tabla_Datos.Columns.Add(Columna13_Area_Funcional_ID);
        Columna14_Programa_ID = new DataColumn();
        Columna14_Programa_ID.DataType = System.Type.GetType("System.String");
        Columna14_Programa_ID.ColumnName = "PROGRAMA_ID";
        Tabla_Datos.Columns.Add(Columna14_Programa_ID);
        Columna15_Unidad_ID = new DataColumn();
        Columna15_Unidad_ID.DataType = System.Type.GetType("System.String");
        Columna15_Unidad_ID.ColumnName = "UNIDAD_RESPONSABLE_ID";
        Tabla_Datos.Columns.Add(Columna15_Unidad_ID);
        Columna16_Partida_ID = new DataColumn();
        Columna16_Partida_ID.DataType = System.Type.GetType("System.String");
        Columna16_Partida_ID.ColumnName = "PARTIDA_ID";
        Tabla_Datos.Columns.Add(Columna16_Partida_ID);
        Columna17_Dep_Presupuesto_ID = new DataColumn();
        Columna17_Dep_Presupuesto_ID.DataType = System.Type.GetType("System.String");
        Columna17_Dep_Presupuesto_ID.ColumnName = "DEP_PRESUPUESTO_ID";
        Tabla_Datos.Columns.Add(Columna17_Dep_Presupuesto_ID);


        return Tabla_Datos;
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Validar_Existencia_Dep_Presupuesto
    /// 	DESCRIPCIÓN: Valida si existe un presupuesto para la dependencia, con la misma fuente de 
    /// 	            financiamiento, programa y partida, de ser asi, ofrece actualizar dicho presupuesto
    /// 	            Regresa verdadero si no encontro presupuestos duplicados y falso si encontro un 
    /// 	            presupuesto con los datos seleccionados
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 28-abr-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private bool Validar_Existencia_Dep_Presupuesto()
    {
        Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Consulta_Presupuestos = new Cls_Ope_SAP_Dep_Presupuesto_Negocio();
        System.Data.DataTable Dt_Partidas;

        // Verificar que todos los combos tienen un valor seleccionado
        if (Cmb_Dependencia.SelectedIndex > 0 && Cmb_Fuente_Financiamiento.SelectedIndex > 0 &&
            Cmb_Programa.SelectedIndex > 0 && Cmb_Partida.SelectedIndex > 0)
        {
            Rs_Consulta_Presupuestos.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
            Rs_Consulta_Presupuestos.P_Fuente_Financiamiento_ID = Cmb_Fuente_Financiamiento.SelectedValue;
            Rs_Consulta_Presupuestos.P_Programa_ID = Cmb_Programa.SelectedValue;
            Rs_Consulta_Presupuestos.P_Partida_ID = Cmb_Partida.SelectedValue;
            //Consultar presupuestos
            Dt_Partidas = Rs_Consulta_Presupuestos.Consulta_Datos_Presupuestos();
            
            //Si la consulta arrojo resultados, ya hay un presupuesto con estos datos, mostrar mensaje
            if (Dt_Partidas.Rows.Count > 0)
            {
                Lbl_Mensaje_Modal_Presupuesto_Existente.Text =
                    " &nbsp; &nbsp; &nbsp; + Unidad responsable: " + Cmb_Dependencia.SelectedItem.Text + "<br />" +
                    " &nbsp; &nbsp; &nbsp; + Fuente de financiamiento: " + Cmb_Fuente_Financiamiento.SelectedItem.Text + "<br />" +
                    " &nbsp; &nbsp; &nbsp; + Programa: " + Cmb_Programa.SelectedItem.Text + "<br />" +
                    " &nbsp; &nbsp; &nbsp; + Partida: " + Cmb_Partida.SelectedItem.Text + "<br />" +
                    String.Format(" &nbsp; &nbsp; &nbsp; + Monto presupuestal: {0:C}<br />", Dt_Partidas.Rows[0]["MONTO_PRESUPUESTAL"]) +
                    String.Format(" &nbsp; &nbsp; &nbsp; + Monto disponible: {0:C}<br />", Dt_Partidas.Rows[0]["MONTO_DISPONIBLE"]) +
                    String.Format(" &nbsp; &nbsp; &nbsp; + Monto comprometido: {0:C}<br />", Dt_Partidas.Rows[0]["MONTO_COMPROMETIDO"]) +
                    String.Format(" &nbsp; &nbsp; &nbsp; + Monto ejercido: {0:C}<br /><br />", Dt_Partidas.Rows[0]["MONTO_EJERCIDO"]) +
                    " ¿Desea actualizar el monto asignado a esta partida?";
                //Asignar valor a los campos ocultos de montos y comentario
                Session["Presupuesto_Existente"] = Dt_Partidas;

                Btn_Salir_Modal_Presupuesto.Visible = false;
                Btn_Si_Modificar.Visible = true;
                Btn_No_Modificar.Visible = true;
                Mpe_Pnl_Contenedor_Editar_Presupuesto.Show();
                return false;
            }
            else        //la consulta no encontro presupuestos con los datos seleccionados, regresar true
            {
                return true;
            }
        }
            return false;
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Validaciones_Presupuestos_A_Sinconizar
    /// 	DESCRIPCIÓN: Valida antes de ofrecer la sincronizacion:
    /// 	        Que no haya filas duplicadas,
    /// 	        Que todas las claves existan y las convierte a su respectivo ID
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 28-abr-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Validaciones_Presupuestos_A_Sinconizar()
    {
        String Mensaje;

        Lbl_Mensaje_Error_Modal.Visible = false;
        Lbl_Mensaje_Error_Modal.Text = "";
        Img_Error_Modal.Visible = false;

        //Verificar que no haya filas duplicadas
        Mensaje = Buscar_Filas_Duplicadas();
        if (Mensaje.Length > 0) //Mostrar informacion de filas duplicadas
        {
            Lbl_Mensaje_Error_Modal.Visible = true;
            Lbl_Mensaje_Error_Modal.Text = Mensaje;
            Img_Error_Modal.Visible = true;
            Btn_Sincronizar_Presupuestos.Visible = false;
            //Mpe_Cargar_Archivo.Show();
        }
        else        // Si no hay filas duplicadas, sustituir las Claves con los IDs
        {
            Mensaje = buscar_IDs();

            if (Mensaje.Length > 0)     //Si se recibio mensaje, no se encontro el ID de alguna(s) clave(s), se muestra el mensaje con los detalles
            {
                Lbl_Mensaje_Error_Modal.Visible = true;
                Lbl_Mensaje_Error_Modal.Text = Mensaje;
                Img_Error_Modal.Visible = true;
                Btn_Sincronizar_Presupuestos.Visible = false;
                //Mpe_Cargar_Archivo.Show();
            }
            else        //Separar en dos tablas los presupuestos que se van a actualizar y los que se daran de alta
            {
                Separar_Presupuestos_Sincronizar();
            }
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Separar_Presupuestos_Sincronizar
    /// 	DESCRIPCIÓN: Separar los presupuestos que se van a actualizar de los que se van a dar de alta
    /// 	            para informar al usuario mostrando el resultado en dos tablas
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 28-abr-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Separar_Presupuestos_Sincronizar()
    {
        System.Data.DataTable Dt_Presupuestos = (System.Data.DataTable)Session["Tabla_Datos_Archivo"];
        System.Data.DataTable Dt_Presupuestos_Alta = Dt_Presupuestos.Clone();      //Copiar estructura de la tabla para poder importar filas
        System.Data.DataTable Dt_Presupuestos_Modificar = Dt_Presupuestos.Clone();
        Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Consulta_Presupuestos = new Cls_Ope_SAP_Dep_Presupuesto_Negocio();
        System.Data.DataTable Dt_Consulta_Presupuesto;

        foreach (System.Data.DataRow Fila in Dt_Presupuestos.Rows)
        {
            Rs_Consulta_Presupuestos.P_Dependencia_ID = Fila["UNIDAD_RESPONSABLE_ID"].ToString();
            Rs_Consulta_Presupuestos.P_Fuente_Financiamiento_ID = Fila["FUENTE_FINACIAMIENTO_ID"].ToString();
            Rs_Consulta_Presupuestos.P_Programa_ID = Fila["PROGRAMA_ID"].ToString();
            Rs_Consulta_Presupuestos.P_Partida_ID = Fila["PARTIDA_ID"].ToString();

            Dt_Consulta_Presupuesto = Rs_Consulta_Presupuestos.Consulta_Datos_Presupuestos();
            //Si la consulta arrojo resultados, ya hay un presupuesto con estos datos, agregar a la tabla a modificar
            if (Dt_Consulta_Presupuesto.Rows.Count > 0)
            {
                Fila["DEP_PRESUPUESTO_ID"] = Dt_Consulta_Presupuesto.Rows[0][Cat_Com_Dep_Presupuesto.Campo_Presupuesto_ID].ToString();//recuperar campo Presupuesto_ID 
                Dt_Presupuestos_Modificar.ImportRow(Fila);
            }
            else // si no se encuentra el presupuesto en
            {
                Dt_Presupuestos_Alta.ImportRow(Fila);
            }
        }
        Session["Tabla_Dep_Pres_Altas"] = Dt_Presupuestos_Alta;
        Session["Tabla_Dep_Pres_Modificaciones"] = Dt_Presupuestos_Modificar;
    }

#endregion

    ///**********************************************************************************************************************************
    ///                                                                EVENTOS
    ///**********************************************************************************************************************************
    #region EVENTOS

    protected void Page_Load(object sender, EventArgs e)
    {
        //Esconder el panel modal para cargar archivo
        Mpe_Cargar_Archivo.Hide();
        Mpe_Pnl_Contenedor_Editar_Presupuesto.Hide();
        Fle_Cargar_Archivo.Style.Value = "width:665px;height:24px;";
        ScriptManager.GetCurrent(Page).RegisterPostBackControl(Btn_Enviar_Archivo);
        ScriptManager.GetCurrent(Page).RegisterPostBackControl(Btn_Sincronizar_Presupuestos);
        ScriptManager.GetCurrent(Page).RegisterPostBackControl(Btn_Cancelar_Sincronizacion);
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
    /// 	NOMBRE_FUNCIÓN: Btn_Salir_Click
    /// 	DESCRIPCIÓN: Manejo del evento Click para el control Btn_Salir. Ir a la página principal o 
    /// 	        inicializar controles 
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 04-mar-2011
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
                Session.Remove("Consulta_Dep_Presupuestos");
                Session.Remove("Tabla_Datos_Archivo");
                Session.Remove("Presupuesto_Existente");
                Session.Remove("Tabla_Dep_Pres_Modificaciones");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Inicializa_Controles();//Habilita los controles para la siguiente operación del usuario 
                Grid_Presupuestos.DataSource = null;
                Grid_Presupuestos.DataBind();
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
    /// 	FECHA_CREO: 04-mar-2011
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
                Limpiar_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                Txt_Ejercido.Text = "0.00";
                Txt_Comprometido.Text = "0.00";
                Cmb_Dependencia.Focus();
                Grid_Presupuestos.DataSource = null;
                Grid_Presupuestos.DataBind();
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
                    if (Validar_Existencia_Dep_Presupuesto())   // verificar que no haya un presupuesto con los mismos datos
                        Alta_Presupuesto(); //Da de alta los datos proporcionados por el usuario
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
    /// 	NOMBRE_FUNCIÓN: Cmb_Dependencia_SelectedIndexChanged
    /// 	DESCRIPCIÓN: Evento Cambio de índice en combo Dependencia, activar y cargar el combo Fuentes de 
    /// 	            financiamiento (si es un Alta de registro) y el grid con los Presupuestos de la 
    /// 	            dependencia seleccionada si no se está editando
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 04-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cmb_Dependencia_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Valor_Combo_Dependencia = "";
        // Verificar que hay una Dependencia seleccionada y es un nuevo registro, activación de combos 
        if (Cmb_Dependencia.SelectedIndex > 0 && Btn_Nuevo.ToolTip == "Dar de Alta")
        {
            Consulta_Presupuestos(); //Consulta todos los Presupuestos en la BD
            Cmb_Fuente_Financiamiento.Enabled = true;
            Cargar_Combo_Fuentes_Financiamiento();
            Cmb_Programa.Enabled = true;
            Cargar_Combo_Programas();
            Cmb_Fuente_Financiamiento.Focus();
            //Si el combo Parida está activado, llamar al manejador del evento cambio de índice del combo Partida
            if (Cmb_Partida.Enabled == true)
                Cmb_Programa_SelectedIndexChanged(this, EventArgs.Empty);
        }
        else if (Cmb_Dependencia.SelectedIndex > 0 && Btn_Nuevo.ToolTip == "Nuevo")
        {
            Consulta_Presupuestos(); //Consulta todos los Presupuestos en la BD
            Valor_Combo_Dependencia = Cmb_Dependencia.SelectedValue;    //almacenar el valor del combo para que no se pierda al limpiar controles
            Limpiar_Controles();
            Cmb_Dependencia.SelectedValue = Valor_Combo_Dependencia;    // restaurar el valor seleccionado en el combo
        }
        else
        {
            // Si no hay una Dependencia seleccionada, desactivar y limpiar el combo Fuente financiamiento
            Cmb_Fuente_Financiamiento.Enabled = false;
            Cmb_Fuente_Financiamiento.DataSource = null;
            Cmb_Fuente_Financiamiento.DataBind();
            Cmb_Fuente_Financiamiento.SelectedIndex = -1;
            Cmb_Programa.Enabled = false;
            Cmb_Programa.DataSource = null;
            Cmb_Programa.DataBind();
            Cmb_Programa.SelectedIndex = -1;
            Cmb_Programa_SelectedIndexChanged(this, EventArgs.Empty);
            //limpiar y desactivar Txt_Anio
            //Txt_Anio.Text = "";
            //Txt_Anio.Enabled = false;
            //Txt_Numero_Asignacion.Text = "";
            Lbl_Presupuesto_Partida.Text = "";
            //limpiar el grid Presupuestos
            Grid_Presupuestos.DataSource = null;
            Grid_Presupuestos.DataBind();
            Cmb_Dependencia.Focus();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Cmb_Programa_SelectedIndexChanged
    /// 	DESCRIPCIÓN: Evento Cambio de índice en combo Programa, activar y cargar el combo Partidas 
    /// 	        con las Partidas del programa seleccionado
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 04-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cmb_Programa_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Verificar que hay una Dependencia seleccionada
        if (Cmb_Programa.SelectedIndex > 0)
        {
            Cmb_Partida.Enabled = true;
            Cargar_Combo_Partidas();
            Cmb_Partida.Focus();
        }
        else
        {
            // Si no hay una Dependencia seleccionada, desactivar y limpiar el combo Partida
            Cmb_Partida.Enabled = false;
            Cmb_Partida.DataSource = null;
            Cmb_Partida.DataBind();
            Cmb_Partida.SelectedIndex = -1;
            Cmb_Programa.Focus();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Cmb_Partida_SelectedIndexChanged
    /// 	DESCRIPCIÓN: Evento Cambio de índice en combo Partida, mostrar al usuario el monto disponible 
    /// 	            de la partida seleccionada
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 04-mar-2011
    /// 	MODIFICÓ: Roberto González Oseguera
    /// 	FECHA_MODIFICÓ: 28-abr-2011
    /// 	CAUSA_MODIFICACIÓN: Se comento debido a que ya no se valida el anio en curso
    /// 	                    Se agrego la llamada al metodo Validar_Existencia cuando es una alta
    ///*******************************************************************************************************
    protected void Cmb_Partida_SelectedIndexChanged(object sender, EventArgs e)
    {
        Txt_Monto_Presupuestal.Focus();

        if (Btn_Nuevo.ToolTip == "Dar de Alta") //Si es un alta, verificar la existencias de presupuestos con los mismos datos
        {
            Validar_Existencia_Dep_Presupuesto();
        }
        // Verificar que hay una Partida seleccionada
        //if (Cmb_Partida.SelectedIndex > 0)
        //{
        //    // Si el campo de texto ya contiene un año, volver a consultar el presupuesto de la Partida
        //    if (Txt_Anio.Text.Length == 4)
        //    {
        //        Txt_Anio.Enabled = true;
        //        //Txt_Anio_TextChanged(this, EventArgs.Empty);
        //    }
        //    else
        //    {
        //        // activar Txt_Anio
        //        Txt_Anio.Enabled = true;
        //        if (Txt_Anio.Text == "")    //Si el campo año está vacío, asignar el año en curso
        //        {
        //            Txt_Anio.Text = DateTime.Now.Year.ToString();
        //            //Txt_Anio_TextChanged(this, EventArgs.Empty);
        //        }
        //        Txt_Anio.Focus();
        //    }
        //    Txt_Monto_Presupuestal.Enabled = true;

        //}
        //else
        //{
        //    // Si no hay una Partida seleccionada, desactivar Txt_Anio
        //    //Txt_Anio.Enabled = false;
        //    //Txt_Anio.Text = "";
        //    Txt_Monto_Presupuestal.Enabled = false;
        //    Txt_Monto_Presupuestal.Text = "";
        //    Txt_Disponible.Text = "";
        //    Txt_Numero_Asignacion.Text = "";
        //    Lbl_Presupuesto_Partida.Text = "";
        //    Txt_Monto_Presupuestal.Focus();
        //}
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Txt_Anio_TextChanged
    /// 	DESCRIPCIÓN: Evento Cambio de texto en campo de Texto Año, Asigna el valor del campo 
    /// 	            Asignación por año, Consecutivo de presupuestos asignados en el año a una 
    /// 	            determinada Dependencia por Partida y programa
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 04-mar-2011
    /// 	MODIFICÓ: Roberto González Oseguera
    /// 	FECHA_MODIFICÓ: 28-abr-2011
    /// 	CAUSA_MODIFICACIÓN: Se comento debido a que el anio se oculto en la pagina del catalogo 
    /// 	                    y se elimino el evento del control
    ///*******************************************************************************************************
    //protected void Txt_Anio_TextChanged(object sender, EventArgs e)
    //{
    //    Int32 Numero_Asignacion;    //variable para almacenar el número de asignación
    //    Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Consulta_Siguiente_Numero_Asignacion = new Cls_Ope_SAP_Dep_Presupuesto_Negocio();

    //    if (Btn_Nuevo.ToolTip == "Dar de Alta" && Txt_Anio.Text.Length == 4)
    //    {
    //        //verificar que todos los combos fueron seleccionados
    //        if (Cmb_Partida.SelectedIndex > 0 && Cmb_Programa.SelectedIndex > 0 && Cmb_Dependencia.SelectedIndex > 0)
    //        {
    //            try
    //            {
    //                Rs_Consulta_Siguiente_Numero_Asignacion.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
    //                Rs_Consulta_Siguiente_Numero_Asignacion.P_Programa_ID = Cmb_Programa.SelectedValue;
    //                Rs_Consulta_Siguiente_Numero_Asignacion.P_Partida_ID = Cmb_Partida.SelectedValue;
    //                Rs_Consulta_Siguiente_Numero_Asignacion.P_Anio = Txt_Anio.Text;
    //                Numero_Asignacion = Rs_Consulta_Siguiente_Numero_Asignacion.Consulta_Numero_Asignacion_Presupuesto_Anio();
    //                if (Numero_Asignacion > 0)
    //                    Txt_Numero_Asignacion.Text = Numero_Asignacion.ToString();
    //            }
    //            catch (Exception Ex)
    //            {
    //                throw new Exception("Numero Asginación por año: " + Ex.Message);
    //            }
    //            //Llamar método que consulta el monto disponible de la partida seleccionada en el año proporcionado
    //            //Consulta_Presupuesto_Partida();
    //        }
    //    }
    //    Txt_Monto_Presupuestal.Focus();
    //}

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Txt_Monto_Presupuestal_TextChanged
    /// 	DESCRIPCIÓN: Manejo del evento Cambio de texto en campo de Texto Monto presupuestal, si es un alta, 
    /// 	        asigna 0 a ejercido y a comprometido y al campo Disponible le asigna lo mismo que el usuario 
    /// 	        introdujo en Monto Presupuesto
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 04-mar-2011
    /// 	MODIFICÓ: Roberto González Oseguera
    /// 	FECHA_MODIFICÓ: 28-abr-2011
    /// 	CAUSA_MODIFICACIÓN: Validar que si se actualiza el monto presupuestal, no sea menor que el actual
    ///*******************************************************************************************************
    protected void Txt_Monto_Presupuestal_TextChanged(object sender, EventArgs e)
    {
        Double Monto_Presupuestal = 0.0;
        Double Monto_Presupuestal_Anterior = 0.0;

        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        if (Txt_Monto_Presupuestal.Text != "")
        {
            Monto_Presupuestal = Convert.ToDouble(Extraer_Numero(Txt_Monto_Presupuestal.Text));
            if (Monto_Presupuestal > 0)       //Si se encontró un número mayor que 0 en el 
            {       //Asignar valor al campo Disponible y Monto presupuestal
                if (Btn_Modificar.ToolTip == "Actualizar") //si es una actualizacion, verificar que el presupuesto no sea menor que el comprometido
                {
                    Monto_Presupuestal_Anterior = Convert.ToDouble(Extraer_Numero(Txt_Monto_Presupuestal_Anterior.Value));
                    if (Monto_Presupuestal < Monto_Presupuestal_Anterior) //Verificar que el monto introducido sea mayor que el monto anterior
                    {               //Mostrar mensaje si el monto disminuyo
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "&nbsp; &nbsp; &nbsp; &nbsp; El Monto Presupuestal no se puede disminuir<br />";
                        Txt_Monto_Presupuestal.Text = Monto_Presupuestal_Anterior.ToString();
                        Txt_Disponible.Text = Txt_Monto_Disponible_Anterior.Value;
                    }
                    else if (Monto_Presupuestal == Monto_Presupuestal_Anterior)
                    {       // si el monto presupuestal es igual
                        Txt_Monto_Presupuestal.Text = Monto_Presupuestal.ToString();
                        Txt_Disponible.Text = Txt_Monto_Disponible_Anterior.Value;
                    }
                    else
                    {       //Si el monto presupuestal aumento, sumar al monto disponible la diferencia
                        Txt_Monto_Presupuestal.Text = Monto_Presupuestal.ToString();
                        Txt_Disponible.Text = (Convert.ToDouble(Txt_Disponible.Text) + (Monto_Presupuestal - Monto_Presupuestal_Anterior)).ToString();
                    }
                }
                else
                {
                    Txt_Disponible.Text = Monto_Presupuestal.ToString();
                    Txt_Monto_Presupuestal.Text = Monto_Presupuestal.ToString();
                }
            }
        }
        Txt_Comentarios.Focus();
        if (Btn_Nuevo.ToolTip == "Dar de Alta") //Si es un alta, verificar la existencias de presupuestos con los mismos datos
        {
            Validar_Existencia_Dep_Presupuesto();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Grid_Presupuestos_SelectedIndexChanged
    /// 	DESCRIPCIÓN: Consulta los datos del elemento que seleccionó el usuario y los muestra 
    /// 	            en los campos correspondientes
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 05-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Presupuestos_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Consulta_Cat_Ope_Presupuesto = new Cls_Ope_SAP_Dep_Presupuesto_Negocio(); //Variable de conexión a la capa de Negocios para la consulta de los datos del Presupuesto
        System.Data.DataTable Dt_Prespuestos; //Variable que obtendrá los datos de la consulta
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Rs_Consulta_Cat_Ope_Presupuesto.P_Presupuesto_ID = Convert.ToInt32(Grid_Presupuestos.SelectedRow.Cells[1].Text);
            Dt_Prespuestos = Rs_Consulta_Cat_Ope_Presupuesto.Consulta_Datos_Presupuestos(); //Consulta los datos del Presupuesto que fue seleccionado por el usuario
            if (Dt_Prespuestos.Rows.Count > 0)
            {
                //Escribe los valores de los campos a los controles correspondientes de la forma
                foreach (DataRow Registro in Dt_Prespuestos.Rows)
                {
                    Txt_Presupuesto_ID.Value = Registro[Cat_Com_Dep_Presupuesto.Campo_Presupuesto_ID].ToString();
                    Txt_Anio.Text = Registro[Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto].ToString();
                    Txt_Numero_Asignacion.Text = Registro[Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio].ToString();
                    Txt_Monto_Presupuestal.Text = Registro[Cat_Com_Dep_Presupuesto.Campo_Monto_Presupuestal].ToString();
                    Txt_Monto_Presupuestal_Anterior.Value = Registro[Cat_Com_Dep_Presupuesto.Campo_Monto_Presupuestal].ToString(); //Guardar por si el usuario cambia el valor
                    Txt_Ejercido.Text = Registro[Cat_Com_Dep_Presupuesto.Campo_Monto_Ejercido].ToString();
                    Txt_Disponible.Text = Registro[Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible].ToString();
                    Txt_Monto_Disponible_Anterior.Value = Txt_Disponible.Text;
                    Txt_Comprometido.Text = Registro[Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido].ToString();
                    Txt_Comentarios.Text = Registro[Cat_Com_Dep_Presupuesto.Campo_Comentarios].ToString();
                    Cmb_Dependencia.SelectedValue = Registro[Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID].ToString();
                    Cargar_Combo_Programas();
                    Cmb_Programa.SelectedValue = Registro[Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID].ToString();
                    Cargar_Combo_Partidas();
                    Cmb_Partida.SelectedValue = Registro[Cat_Com_Dep_Presupuesto.Campo_Partida_ID].ToString();
                    Cargar_Combo_Fuentes_Financiamiento();
                    Cmb_Fuente_Financiamiento.SelectedValue = Registro[Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID].ToString();
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
    /// 	NOMBRE_FUNCIÓN: Grid_Presupuestos_PageIndexChanging
    /// 	DESCRIPCIÓN: Manejo del evento de paginación del grid (cargar los datos de la página seleccionada)
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 05-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Presupuestos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpiar_Controles(); //Limpia todos los controles de la forma
            Grid_Presupuestos.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Llena_Grid_Presupuestos(); //Carga los elementos que están asignados a la página seleccionada
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
    /// 	DESCRIPCIÓN: Manejo del evento Click para el control Btn_Modificar. Activar el campo 
    /// 	            Monto Presupuestal sólo si Comprometido y ejercido son 0
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 01-mar-2011
    /// 	MODIFICÓ: Roberto González Oseguera
    /// 	FECHA_MODIFICÓ: 28-abr-2011
    /// 	CAUSA_MODIFICACIÓN: Se comento la validacion que impedia modificar un presupuesto con monto ejercido o comprometido
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
                if (Txt_Presupuesto_ID.Value != "")
                {               //si monto ejercido o comprometido es mayor a 0, no permitir modificación
                    //if (Convert.ToDouble(Txt_Ejercido.Text) > 0 || Convert.ToDouble(Txt_Comprometido.Text) > 0)
                    //{
                    //    Lbl_Mensaje_Error.Visible = true;
                    //    Img_Error.Visible = true;
                    //    Lbl_Mensaje_Error.Text = "No está permitido modificar Presupuestos que ya tienen Monto Ejercido o comprometido.<br />";
                    //}
                    //else
                    //{

                        Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                        //Consulta_Presupuesto_Partida();
                        Txt_Monto_Presupuestal.Focus();
                    //}
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el Presupuesto cuyos datos desea modificar<br />";
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
                    Modificar_Presupuesto_Asignado(); //Actualizar los datos del Presupuesto
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
    /// 	NOMBRE_FUNCIÓN: Btn_Buscar_Click
    /// 	DESCRIPCIÓN: Buscar Pr en la base de datos por clave, nombre y descripcion
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 02-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        int Valor_Dependencia;      //Almacenar temporalmente el valor seleccionado del combo Dependencia
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            
            if (Cmb_Dependencia.SelectedIndex > 0)      // Solo buscar partidas si hay una dependencia seleccionada
            {
                Valor_Dependencia = Cmb_Dependencia.SelectedIndex;
                Consulta_Presupuestos(); //Método que consulta los elementos en la base de datos
                Limpiar_Controles(); //Limpia los controles de la forma
                Cmb_Dependencia.SelectedIndex = Valor_Dependencia;
                //Si no se encontraron Partidas con un nombre similar al proporcionado por el usuario entonces manda un mensaje al usuario
                if (Grid_Presupuestos.Rows.Count <= 0)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron Presupuestos con el criterio proporcionado<br />";
                }
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Debe seleccionar una Unidad responsable<br />";
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
    /// 	NOMBRE_FUNCIÓN: Btn_Enviar_Archivo_Click
    /// 	DESCRIPCIÓN: Manejar el evento UploadComplete del controle AsyncFileUpload en el panel 
    /// 	            Lee un archivo de excel con formato especifico y valida que contenga todos los campos
    /// 	            Si contiene todos los campos, muestra un boton para sincronizar los datos 
    /// 	            (enviarlos a la base de datos)
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 08-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Enviar_Archivo_Click(object sender, EventArgs e)
    {
        String Cadena_Conexion = "";
        String Ruta_Archivo;
        String Filas_Con_Errores = "";
        //String Mensaje_Buscar_Duplicador = "";
        Int16 Contador_Filas = 0;
        Int16 Contador_Errores = 0;
        DataRow Nueva_Fila;
        String Pestania_Actual_Archivo = "";

        OleDbDataAdapter myCommand;
        DataSet Datos_Archivo = new DataSet();

        System.Data.DataTable Tabla_Datos = Generar_Tabla_Datos();      //Obtener estructura de tabla

        Img_Error_Modal.Visible = false;            //Limpiar y ocultar mensaje de error modal
        Lbl_Mensaje_Error_Modal.Visible = false;
        Lbl_Mensaje_Error_Modal.Text = "";
        Lbl_Resumen_Carga_Archivo.Text = "";
        Txt_Comentario_Modal.Visible = false;
        Btn_Sincronizar_Presupuestos.Visible = false;   //Inicializar controles ocultos y grids del control modal
        Grid_Datos_Archivo.DataSource = null;
        Grid_Datos_Archivo.DataBind();
        Grid_Datos_Archivo_Modificar.DataSource = null;
        Grid_Datos_Archivo_Modificar.DataBind();
        Fila_Presupuestos_Alta.Style.Value = "display:none;";
        Fila_Presupuestos_Actualizar.Style.Value = "display:none;";


        Mpe_Cargar_Archivo.Show();
        Contenedor_Roller.Style.Value = "display:block;";

        if (Fle_Cargar_Archivo.HasFile)
        {
            if (!string.IsNullOrEmpty(Hdn_Ruta_Archivo.Value))  //Si el campo no esta vacio, significa que se cargo otro archivo, se debe eliminar el archivo anterior del servidor
            {
                File.Delete(Hdn_Ruta_Archivo.Value);    // Borrar archivo
                Hdn_Ruta_Archivo.Value = "";            //Limpiar contenido del campo
            }

            try
            {
                if (!Fle_Cargar_Archivo.FileName.Contains(".xls"))
                {
                    //Mostrar mensaje indicando que el archivo no es de Excel
                    Img_Error_Modal.Visible = true;
                    Lbl_Mensaje_Error_Modal.Visible = true;
                    Lbl_Mensaje_Error_Modal.Text = "&nbsp; &nbsp; &nbsp; &nbsp; + Debe proporcionar la ruta del archivo de Excel con los presupuestos.<br />";
                    Contenedor_Roller.Style.Value = "display:none;";
                    return;
                }
                    //Si el directorio no existe, crearlo
                if (!Directory.Exists(MapPath("Sincronizacion_Presupuestos")))
                    Directory.CreateDirectory(MapPath("Sincronizacion_Presupuestos"));
                // Guardar archivo en el servidor con un nombre especifico (incluyendo fecha)
                Ruta_Archivo = MapPath("Sincronizacion_Presupuestos/presupuestos_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + Path.GetExtension(Fle_Cargar_Archivo.FileName));
                Fle_Cargar_Archivo.SaveAs(Ruta_Archivo);
                Fle_Cargar_Archivo.FileContent.Close();
                Hdn_Ruta_Archivo.Value = Ruta_Archivo;

                if (Ruta_Archivo.Contains(".xlsx"))       // Formar la cadena de conexion si el archivo es Exceml xml
                {
                    Cadena_Conexion = @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                            "Data Source=" + Ruta_Archivo + ";" +
                            "Extended Properties=\"Excel 12.0 Xml;HDR=YES\"";
                }
                else if (Ruta_Archivo.Contains(".xls"))   // Formar la cadena de conexion si el archivo es Exceml binario
                {
                    Cadena_Conexion = @"Provider=Microsoft.Jet.OLEDB.4.0;" +
                            "Data Source=" + Ruta_Archivo + ";" +
                            "Extended Properties=Excel 8.0;";
                }

                OleDbConnection dbConnection = new OleDbConnection(Cadena_Conexion);
                dbConnection.Open();

                try
                {
                    System.Data.DataTable Dt_Esquema = dbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    foreach (DataRow Fila_Hoja_Excel in Dt_Esquema.Rows)
                    {
                        // Seleccionar las celdas desde nombres de campos (nombres de columnas ) con datos 
                        Pestania_Actual_Archivo = Fila_Hoja_Excel["TABLE_NAME"].ToString();
                        myCommand = new OleDbDataAdapter("SELECT * FROM [" + Pestania_Actual_Archivo + "A5:J100000]", Cadena_Conexion);
                        Datos_Archivo.Clear();
                        myCommand.Fill(Datos_Archivo, "ExcelInfo");


                        //Crear tabla con registros del archivo Excel si se encontraron celdas con datos
                        if (Datos_Archivo.Tables["ExcelInfo"].Rows.Count > 0)
                        {
                            // Agregar los registros encontrados en el archivo a filas en la tabla datos
                            foreach (DataRow Fila in Datos_Archivo.Tables["ExcelInfo"].Rows)        // recorrer cada fila
                            {
                                //verificar contenido de primeras 5 columnas, si cualquiera esta vacia, saltar columna
                                if (String.IsNullOrEmpty(Fila[0].ToString()) || String.IsNullOrEmpty(Fila[1].ToString()) ||
                                    String.IsNullOrEmpty(Fila[2].ToString()) || String.IsNullOrEmpty(Fila[3].ToString()) ||
                                    String.IsNullOrEmpty(Fila[4].ToString()))
                                    break;

                                Int16 Contador_Columnas = 1;
                                Nueva_Fila = Tabla_Datos.NewRow();

                                Nueva_Fila["CONSECUTIVO"] = (++Contador_Filas).ToString();
                                Nueva_Fila["ESTATUS"] = ResolveUrl("../imagenes/paginas/accept.png");

                                foreach (DataColumn celda in Datos_Archivo.Tables["ExcelInfo"].Columns) //recorrer cada columna en la fila
                                {
                                    if (Fila[celda] == null)//Si es nulo, indicar que no se subira el registro mediante el icono de la ultima celda
                                    {
                                        Nueva_Fila["ESTATUS"] = ResolveUrl("../imagenes/paginas/icono_cancelar.png");
                                        Contador_Errores++;
                                        if (Filas_Con_Errores.Length > 0)  //Guardar numero de fila con error
                                            Filas_Con_Errores = Filas_Con_Errores + ", " + Contador_Filas;
                                        else
                                            Filas_Con_Errores = "Revisar los registros: " + Contador_Filas;
                                    }
                                    if (Fila[celda].ToString().Length <= 0)
                                        Nueva_Fila[Contador_Columnas] = "0";
                                    else
                                        Nueva_Fila[Contador_Columnas] = Fila[celda].ToString().Trim();
                                    Contador_Columnas++;
                                }

                                Tabla_Datos.Rows.Add(Nueva_Fila); //Agregar la fila a la tabla de datos
                            }

                        }
                        Session["Tabla_Datos_Archivo"] = Tabla_Datos;
                    }
                    // llamar al metodo que valida duplicidad y separa altas de actualizaciones
                    Validaciones_Presupuestos_A_Sinconizar();

                    // Llenar el datagrid
                    //Grid_Datos_Archivo.DataSource = (System.Data.DataTable)Tabla_Datos;
                    //Grid_Datos_Archivo.DataBind();
                    //Tabla_Datos.Columns.Remove("ESTATUS");
                }
                catch
                {
                    //Mostrar mensaje indicando que el archivo no es de Excel
                    Img_Error_Modal.Visible = true;
                    Lbl_Mensaje_Error_Modal.Visible = true;
                    Lbl_Mensaje_Error_Modal.Text = "&nbsp; &nbsp; &nbsp; &nbsp; + El archivo de Excel proporcionado no tiene el formato requerido para la carga de Presupuestos.<br />";
                    if (Pestania_Actual_Archivo != "")
                    {
                        if (Pestania_Actual_Archivo.Contains(" ") || Pestania_Actual_Archivo.Contains("-"))
                            Lbl_Mensaje_Error_Modal.Text += "El error ocurrió durante la lectura de la hoja: " + Pestania_Actual_Archivo.Remove(Pestania_Actual_Archivo.Length - 2, 1) +
                                ". El nombre de la hoja de Excel no debe contener espacios ni guión medio.";
                        else
                            Lbl_Mensaje_Error_Modal.Text += "El error ocurrió durante la lectura de la hoja: " + Pestania_Actual_Archivo.Remove(Pestania_Actual_Archivo.Length - 1, 1);
                    }
                    Contenedor_Roller.Style.Value = "display:none;";
                    //Ocultar el campo comentario
                    Lbl_Comentario_Modal.Visible = false;
                    Txt_Comentario_Modal.Text = "";
                    Txt_Comentario_Modal.Visible = false;
                    //Eliminar archivo de excel si ocurrió una excepción
                    if (!string.IsNullOrEmpty(Hdn_Ruta_Archivo.Value))  //Si el campo no esta vacio, significa que se cargo otro archivo, se debe eliminar el archivo anterior del servidor
                    {
                        dbConnection.Close();
                        File.Delete(Hdn_Ruta_Archivo.Value);    // Borrar archivo
                        Hdn_Ruta_Archivo.Value = "";            //Limpiar contenido del campo
                    }
                    return;
                }
                finally
                {
                    dbConnection.Close();
                }
                //Mensaje_Buscar_Duplicador = Buscar_Filas_Duplicadas();

                Contenedor_Roller.Style.Value = "display:none;";
                Lbl_Resumen_Carga_Archivo.Text = "Total de registros: " + Contador_Filas + "<br />Registros completos: " + (Contador_Filas - Contador_Errores);
                if (Contador_Errores > 0)       //Si se encontraron errores, mostrar mensaje (total de errores y el numero de las filas en la que se encuentran)
                {
                    Lbl_Resumen_Carga_Archivo.Text += "<br />Registros incompletos: " + Contador_Errores + " ( " + Filas_Con_Errores + ")";
                    Fila_Presupuestos_Alta.Style.Value = "display:none;";       //Ocultar filas con titilo de grids
                    Fila_Presupuestos_Actualizar.Style.Value = "display:none;";
                    Grid_Datos_Archivo_Modificar.DataSource = null;             //borrar datos Grid modificaciones
                    Grid_Datos_Archivo_Modificar.DataBind();
                    Grid_Datos_Archivo.DataSource = (System.Data.DataTable)Session["Tabla_Datos_Archivo"];
                    Grid_Datos_Archivo.DataBind();
                }
                else if (Contador_Filas <= 0)   //Si no se encontraron registros al leer el archivo, mostrar mensaje 
                    Lbl_Resumen_Carga_Archivo.Text += "<br />No se encontraron registros de presupuestos.";
                else
                {
                    if (Lbl_Mensaje_Error_Modal.Text == "")     //Si en la etiqueta no hay un mensaje de error, mostrar tablas separadas (actualizaciones y altas)
                    {
                        System.Data.DataTable Dt_Presupuestos_Alta = (System.Data.DataTable)Session["Tabla_Dep_Pres_Altas"];
                        System.Data.DataTable Dt_Presupuestos_Modificar = (System.Data.DataTable)Session["Tabla_Dep_Pres_Modificaciones"];

                        if (Dt_Presupuestos_Modificar.Rows.Count + Dt_Presupuestos_Alta.Rows.Count > 0) //Si hay registros para mostrar
                        {
                            if (Dt_Presupuestos_Modificar.Rows.Count > 0) // Si la tabla de presupuestos para actualizar contiene registros, mostrarlos en el grid
                            {
                                Fila_Presupuestos_Actualizar.Style.Value = "display:table-row;";//Mostrar la fila con titulo de la tabla
                                Grid_Datos_Archivo_Modificar.DataSource = Dt_Presupuestos_Modificar;
                                Grid_Datos_Archivo_Modificar.DataBind();
                            }
                            if (Dt_Presupuestos_Alta.Rows.Count > 0) // Si la tabla de presupuestos para actualizar contiene registros, mostrarlos en el grid
                            {
                                Fila_Presupuestos_Alta.Style.Value = "display:table-row;";
                                Grid_Datos_Archivo.DataSource = Dt_Presupuestos_Alta;
                                Grid_Datos_Archivo.DataBind();
                            }
                            Btn_Sincronizar_Presupuestos.Visible = true;
                            Lbl_Comentario_Modal.Visible = true;
                            Txt_Comentario_Modal.Visible = true;
                        }
                    }
                    else            //mostrar una sola tabla
                    {
                        Fila_Presupuestos_Alta.Style.Value = "display:none;";       //Ocultar filas con titilo de grids
                        Fila_Presupuestos_Actualizar.Style.Value = "display:none;";
                        Grid_Datos_Archivo_Modificar.DataSource = null;             //borrar datos Grid modificaciones
                        Grid_Datos_Archivo_Modificar.DataBind();
                        Grid_Datos_Archivo.DataSource = (System.Data.DataTable)Session["Tabla_Datos_Archivo"];
                        Grid_Datos_Archivo.DataBind();
                    }
                }

            }
            catch (Exception Ex)
            {
                //Eliminar archivo de excel si ocurrió una excepción
                if (!string.IsNullOrEmpty(Hdn_Ruta_Archivo.Value))  //Si el campo no esta vacio, significa que se cargo otro archivo, se debe eliminar el archivo anterior del servidor
                {
                    File.Delete(Hdn_Ruta_Archivo.Value);    // Borrar archivo
                    Hdn_Ruta_Archivo.Value = "";            //Limpiar contenido del campo
                }
                throw new Exception("Error al leer archivo: " + Ex.Message);
            }
            finally
            {
                Contenedor_Roller.Style.Value = "display:none;";
            }
        }
        else
        {
            Img_Error_Modal.Visible = true;
            Lbl_Mensaje_Error_Modal.Visible = true;
            Lbl_Mensaje_Error_Modal.Text = "&nbsp; &nbsp; &nbsp; &nbsp; + Debe proporcionar la ruta del archivo de Excel con los presupuestos.<br />";
            Contenedor_Roller.Style.Value = "display:none;";
        }
    }
    
    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Cancelar_Sincronizacion_Click
    /// 	DESCRIPCIÓN: Manejar el evento clic del boton Cancelar, ocultar ventana modal, limpiar controles
    /// 	            y eliminar archivo de Excel guardado en el servidor
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 13-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Cancelar_Sincronizacion_Click(object sender, EventArgs e)
    {
        Mpe_Cargar_Archivo.Hide();
        if (!string.IsNullOrEmpty(Hdn_Ruta_Archivo.Value))
        {
            File.Delete(Hdn_Ruta_Archivo.Value);
            Hdn_Ruta_Archivo.Value = "";        //Limpiar contenido del campo
        }
        Btn_Sincronizar_Presupuestos.Visible = false;
        Img_Error_Modal.Visible = false;
        Lbl_Mensaje_Error_Modal.Visible = false;
        Lbl_Mensaje_Error_Modal.Text = "";
        Lbl_Resumen_Carga_Archivo.Text = "";
        Txt_Comentario_Modal.Visible = false;
        Lbl_Comentario_Modal.Visible = false;
        Grid_Datos_Archivo.DataSource = null;
        Grid_Datos_Archivo.DataBind();
        Grid_Datos_Archivo_Modificar.DataSource = null;
        Grid_Datos_Archivo_Modificar.DataBind();
        Fila_Presupuestos_Alta.Style.Value = "display:none;";
        Fila_Presupuestos_Actualizar.Style.Value = "display:none;";
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Sincronizar_Presupuestos_Click
    /// 	DESCRIPCIÓN: Manejar el evento Click en el boton Sincronizar archivo, cambia el dato de la 
    /// 	            Clave por el ID correspondiente de Fuente de financiamiento, Programa, Partida, Unidad Responsable
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 08-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Sincronizar_Presupuestos_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Txt_Comentario_Modal.Text))  //Verificar que se proporciono un comentario
        {
            Contenedor_Roller.Style.Value = "display:block;";
            Sincronizacion_Datos();
            Contenedor_Roller.Style.Value = "display:none;";
        }
        else
        {
            Lbl_Mensaje_Error_Modal.Visible = true;
            Lbl_Mensaje_Error_Modal.Text = "Debe proporcionar un comentario para los presupuestos.";
            Img_Error_Modal.Visible = true;
            Mpe_Cargar_Archivo.Show();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Si_Modificar_Click
    /// 	DESCRIPCIÓN: Manejar el evento Click en el boton Si modificar. Si se encontro que ya 
    /// 	            existe un presupuesto con los datos seleccionados, pasar a edicion de presupuesto
    /// 	            (Dependencia, fuente de financiamiento, programa y partida)
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 28-abr-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Si_Modificar_Click(object sender, EventArgs e)
    {
        System.Data.DataTable Dt_Presupuesto = new System.Data.DataTable();
        Dt_Presupuesto = (System.Data.DataTable)Session["Presupuesto_Existente"];

        Habilitar_Controles("Modificar");//Cambiar controles para modificar
        //Asignar valores de montos y comentario desde controles ocultos
        Txt_Monto_Presupuestal.Text = Dt_Presupuesto.Rows[0][Cat_Com_Dep_Presupuesto.Campo_Monto_Presupuestal].ToString();
        Txt_Monto_Presupuestal_Anterior.Value = Txt_Monto_Presupuestal.Text;
        Txt_Disponible.Text = Dt_Presupuesto.Rows[0][Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible].ToString();
        Txt_Monto_Disponible_Anterior.Value = Txt_Disponible.Text;
        Txt_Comprometido.Text = Dt_Presupuesto.Rows[0][Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido].ToString();
        Txt_Ejercido.Text = Dt_Presupuesto.Rows[0][Cat_Com_Dep_Presupuesto.Campo_Monto_Ejercido].ToString();
        Txt_Comentarios.Text = Dt_Presupuesto.Rows[0][Cat_Com_Dep_Presupuesto.Campo_Comentarios].ToString();
        Txt_Anio.Text = Dt_Presupuesto.Rows[0][Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto].ToString();
        Txt_Numero_Asignacion.Text = Dt_Presupuesto.Rows[0][Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio].ToString();
        Txt_Presupuesto_ID.Value = Dt_Presupuesto.Rows[0][Cat_Com_Dep_Presupuesto.Campo_Presupuesto_ID].ToString();

        Txt_Monto_Presupuestal.Focus();

        Mpe_Pnl_Contenedor_Editar_Presupuesto.Hide();
        Session.Remove("Presupuesto_Existente");
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_No_Modificar_Click
    /// 	DESCRIPCIÓN: Manejar el evento Click en el boton no modificar presupuesto. Ocultar el panel 
    /// 	            modal, ya que el usuario selecciono no pasar a actualizar el presupuesto encontrado 
    /// 	            con los mismos datos seleccionados (Dependencia, fuente de financiamiento, programa y partida)
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 28-abr-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_No_Modificar_Click(object sender, EventArgs e)
    {
        Mpe_Pnl_Contenedor_Editar_Presupuesto.Hide();
        Txt_Monto_Presupuestal.Focus();
        Session.Remove("Presupuesto_Existente");
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Salir_Modal_Presupuesto_Click
    /// 	DESCRIPCIÓN: Manejar el evento Click en el boton no salir. Ocultar ventana modal de presupuestos
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 28-abr-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Salir_Modal_Presupuesto_Click(object sender, EventArgs e)
    {
        Mpe_Pnl_Contenedor_Editar_Presupuesto.Hide();
    }

#endregion

}

