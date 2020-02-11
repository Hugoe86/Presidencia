using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Recepcion_Documentos.Negocio;
using Operacion_Predial_Validacion_Recepcion.Negocio;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;
using Presidencia.Reportes;
using System.IO;
using System.Security.Cryptography;
using System.Collections;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Colonias.Negocios;
using Presidencia.Catalogo_Calles.Negocio;
using Presidencia.Catalogo_Predial_Tipos_Documento.Negocio;
using AjaxControlToolkit;
using Presidencia.Catalogo_Notarios.Negocio;
using CrystalDecisions.Shared;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Sindicatos.Negocios;
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;
using Presidencia.Antiguedad_Sindicato.Negocio;


public partial class paginas_Predial_Frm_Ope_Pre_Validacion_Recep_Docs : System.Web.UI.Page
{

    #region Variables
    private const int Const_Estado_Inicial = 0;
    private const int Const_Estado_Modificar = 1;
    //private static String Estado_Combo_Busqueda = "0";
    //private static String Valor_Combo_Busqueda = "0";
    //private static String Estatus = "";
    #endregion

    #region Load/Init
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Session.Remove("Valor_Combo_Busqueda");
            Session.Remove("Estado_Combo_Busqueda");
            string Ventana_Modal;
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Estado_Botones(Const_Estado_Inicial);
                //if(Grid_Documentos.Rows.Count<=0)                
                if (Session["Tabla_Recepciones"] == null || Grid_Bandeja_Recepcion.Rows.Count <= 0)
                    Cargar_Bandeja(0, false);
                Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:yes;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:yes');";
                Btn_Mostrar_Busqueda_Cuentas.Attributes.Add("onclick", Ventana_Modal);
                Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Recepcion_Documentos/Frm_Busqueda_Notarios.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Mostrar_Busqueda_Notario.Attributes.Add("onclick", Ventana_Modal);
                Session["ESTATUS_CUENTAS"] = "IN ('BLOQUEADA','ACTIVA','VIGENTE','SUSPENDIDA')";
            }

            Session["Estatus"] = "='PENDIENTE'";

            Mensaje_Error();
            Llenar_Tabla_Anexos();
            Llenar_Tabla_Observaciones();
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message + "Empleado: " + Cls_Sessiones.Empleado_ID.ToString());
        }
    }
    #endregion

    #region Metodos Generales
    ///*******************************************************************************
    ///NOMBRE DE LA METODO: LLenar_Combo_Id
    ///        DESCRIPCIÓN: llena todos los combos
    ///         PARAMETROS: 1.- Obj_DropDownList: Combo a llenar
    ///                     2.- Dt_Temporal: DataTable genarada por una consulta a la base de datos
    ///                     3.- Texto: nombre de la columna del dataTable que mostrara el texto en el combo
    ///                     3.- Valor: nombre de la columna del dataTable que mostrara el valor en el combo
    ///                     3.- Seleccion: Id del combo el cual aparecera como seleccionado por default
    ///               CREO: Jesus S. Toledo Rdz.
    ///         FECHA_CREO: 06/9/2010
    ///           MODIFICO:
    ///     FECHA_MODIFICO:
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_ID(DropDownList Obj_DropDownList, DataTable Dt_Temporal, String _Texto, String _Valor, String Seleccion)
    {
        String Texto = "";
        try
        {
            Obj_DropDownList.Items.Clear();
            Obj_DropDownList.Items.Add(new ListItem("< SELECCIONAR >", "0"));
            foreach (DataRow row in Dt_Temporal.Rows)
            {
                if (_Texto.Contains("+"))
                {
                    String[] Array_Texto = _Texto.Split('+');

                    foreach (String Campo in Array_Texto)
                    {
                        Texto = Texto + row[Campo].ToString();
                        Texto = Texto + "  ";
                    }
                }
                else
                {
                    Texto = row[_Texto].ToString();
                }
                Obj_DropDownList.Items.Add(new ListItem(Texto, row[_Valor].ToString()));
                Texto = "";
            }
            Obj_DropDownList.SelectedValue = Seleccion;
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    public void Llenar_Combo_ID(DropDownList Obj_DropDownList)
    {
        try
        {
            Obj_DropDownList.Items.Clear();
            Obj_DropDownList.Items.Add(new ListItem("< SELECCIONAR >", "0"));
            Obj_DropDownList.SelectedValue = "0";
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mensaje_Error
    ///DESCRIPCION : Muestra el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 04-Septiembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mensaje_Error(String P_Mensaje)
    {
        Div_Contenedor_Msj_Error.Visible = true;
        Img_Error.Visible = true;
        Lbl_Mensaje_Error.Visible = true;

        Img_Error.Visible = true;
        Lbl_Mensaje_Error.Text += P_Mensaje + "</br>";
    }
    private void Mensaje_Error()
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Lbl_Mensaje_Error.Visible = false;
        Img_Error_Tamite.Visible = false;
        Lbl_Error_Tramite.Text = "";
        Lbl_Error_Tramite.Visible = false;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Estado_Botones
    ///DESCRIPCIÓN: Metodo para establecer el estado de los botones y componentes del formulario
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/02/2011 05:49:53 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Estado_Botones(int P_Estado)
    {
        switch (P_Estado)
        {
            case 0: //Estado inicial

                Datos_Movimiento.Style.Value = "display:none;";
                Div_Bandeja.Style.Value = "display:normal;";
                Div_Notarios.Style.Value = "display:none;";
                Pnl_Contenedor_Datos_Recepcion_Documentos.Style.Value = "display:none;";
                Limpiar_Controles();

                Txt_Comentarios_Area.Enabled = false;
                Cmb_Estatus.Enabled = false;

                Txt_Comentarios_Area.Enabled = false;
                Txt_Cuenta_Predial.Enabled = false;
                Txt_Fecha_Escritura.Enabled = false;
                Txt_Folio_Recepcion.Enabled = false;
                Txt_No_Escritura.Enabled = false;
                Cmb_Estatus.Enabled = false;
                Btn_Buscar_Recepciones_Notario.AlternateText = "Buscar";
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Salir.AlternateText = "Inicio";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Buscar_Recepciones_Notario.ToolTip = "Consultar";
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Salir.ToolTip = "Inicio";
                Btn_Nuevo.Visible = true;
                Btn_Modificar.Visible = false;
                Btn_Agregar_Recepcion.Visible = false;
                Btn_Buscar_Recepciones_Notario.ImageUrl = "~/paginas/imagenes/paginas/busqueda.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";


                break;

            case 1: //Modificar

                Txt_Comentarios_Area.Enabled = true;
                Cmb_Estatus.Enabled = true;

                Datos_Movimiento.Style.Value = "display:normal;";
                Btn_Buscar_Recepciones_Notario.AlternateText = "Buscar";
                Btn_Modificar.AlternateText = "Actualizar";
                Btn_Salir.AlternateText = "Cancelar";

                Btn_Buscar_Recepciones_Notario.ToolTip = "Consultar";
                Btn_Modificar.ToolTip = "Actualizar";
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Modificar.AlternateText = "Actualizar";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Nuevo.Visible = false;
                Btn_Modificar.Visible = true;
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Pnl_Contenedor_Datos_Recepcion_Documentos.Style.Value = "display:none;";
                Div_Notarios.Style.Value = "display:none;";
                break;

            case 2: //Estado Nuevo

                Datos_Movimiento.Style.Value = "display:none;";
                Div_Bandeja.Style.Value = "display:none;";
                Limpiar_Controles();

                Txt_Comentarios_Area.Enabled = false;
                Cmb_Estatus.Enabled = false;

                Txt_Comentarios_Area.Enabled = false;
                Txt_Cuenta_Predial.Enabled = false;
                Txt_Fecha_Escritura.Enabled = false;
                Txt_Folio_Recepcion.Enabled = false;
                Txt_No_Escritura.Enabled = false;
                Cmb_Estatus.Enabled = false;
                Btn_Buscar_Recepciones_Notario.AlternateText = "Buscar";
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Nuevo.Visible = true;
                Btn_Modificar.Visible = false;
                Btn_Buscar_Recepciones_Notario.ToolTip = "Consultar";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Salir.ToolTip = "Cancelar";

                Btn_Buscar_Recepciones_Notario.ImageUrl = "~/paginas/imagenes/paginas/busqueda.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                break;

        }
    }
    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Grid_Detalles_Recepcion_SelectedIndexChanged
    /// 	DESCRIPCIÓN: Manejo del evento SelectedIndexChanged en el grid Tramites
    /// 	            Consulta los detalles (documentos recibidos) del movimiento o tramite seleccionado
    /// 	            y muestra el resultado en el grid detalles recepcion
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 13-may-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Detalles_Recepcion_SelectedIndexChanged(object sender, EventArgs e)
    {
        //limpiar mensajes de error y campos de texto
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Ecabezado_Mensaje.Text = "";
        Lbl_Mensaje_Error.Text = "";

        try
        {
            Btn_Nuevo.Visible = false;
            Btn_Modificar.Visible = true;
            //Pnl_Contenedor_Datos_Cuenta_Predial.Visible = true;
            Pnl_Contenedor_Datos_Recepcion_Documentos.Style.Value = "display:inline;";
            Div_Notarios.Style.Value = "display:inline;";
            Cargar_Datos_Detalle_Tramite(Grid_Detalles_Recepcion.SelectedRow.Cells[1].Text);    // consultar datos del no. de movimiento seleccionado
            //Btn_Agregar_Recepcion.Visible = false;
        }
        catch (Exception ex)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Cargar_Datos_Detalle_Tramite
    /// 	DESCRIPCIÓN: Carga los datos de un tramite en los controles correspondientes
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 13-may-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Cargar_Datos_Detalle_Tramite(String No_Movimiento)
    {
        Cls_Ope_Pre_Recepcion_Documentos_Negocio Rs_Consulta_Tramite = new Cls_Ope_Pre_Recepcion_Documentos_Negocio();
        DataTable Dt_Movimiento;
        DataTable Dt_Anexos;
        DataTable Tabla_Documentos = Generar_Tabla_Documentos();
        Label Lbl_Nombre_Archivo;
        CheckBox Chk_Tipo_Documento;

        try
        {
            Rs_Consulta_Tramite.P_No_Movimiento = No_Movimiento;
            Dt_Movimiento = Rs_Consulta_Tramite.Consulta_Datos_Movimiento();
            Dt_Anexos = Rs_Consulta_Tramite.Consulta_Anexos_Movimiento();
            //Dt_Observaciones = Rs_Consulta_Tramite.Consulta_Observaciones_Movimiento();

            Lbl_Observaciones.Text = "";
            //foreach (DataRow Fila_Observacion in Dt_Observaciones.Rows)       // Cargar las observaciones 
            //{
            //    Lbl_Observaciones.Text += Fila_Observacion[Ope_Pre_Recep_Docs_Observ.Campo_Fecha] + " - ";
            //    Lbl_Observaciones.Text += Fila_Observacion[Ope_Pre_Recep_Docs_Observ.Campo_Usuario_Creo] + "<br/><span style='font-weight:bold;'>";
            //    Lbl_Observaciones.Text += Fila_Observacion[Ope_Pre_Recep_Docs_Observ.Campo_Observaciones] + "</span><br/><br/>";
            //}

            // si los datos recibidos contienen una cuenta predial, cargar dato
            if (!String.IsNullOrEmpty(Dt_Movimiento.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString()))
            {
                Txt_Cuenta_Predial.Text = Dt_Movimiento.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString();
                Txt_Cuenta_Predial_TextChanged(null, EventArgs.Empty);
            }
            Txt_Numero_Escritura.Text = Dt_Movimiento.Rows[0][Ope_Pre_Recep_Docs_Movs.Campo_Numero_Escritura].ToString();
            Txt_Fecha_Escritura.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Movimiento.Rows[0][Ope_Pre_Recep_Docs_Movs.Campo_Fecha_Escritura].ToString()));
            //Txt_Comentarios.Text = Dt_Movimiento.Rows[0][Ope_Pre_Recep_Docs_Movs.Campo_Observaciones].ToString();
            Hdn_No_Movimiento.Value = No_Movimiento;

            //recorrer el grid documentos y limpiar el checkbox y la etiqueta
            foreach (GridViewRow Fila_Grid in Grid_Documentos.Rows)
            {
                Lbl_Nombre_Archivo = (Label)Fila_Grid.FindControl("Lbl_Nombre_Archivo");
                Chk_Tipo_Documento = (CheckBox)Fila_Grid.FindControl("Chk_Documento_Recibido");
                if (Lbl_Nombre_Archivo != null)
                    Lbl_Nombre_Archivo.Text = "";
                if (Chk_Tipo_Documento != null)
                    Chk_Tipo_Documento.Checked = false;
            }

            foreach (DataRow Fila_Anexo in Dt_Anexos.Rows)       // Cargar los anexos encontrados en el grid documentos 
            {
                foreach (GridViewRow Fila_Grid in Grid_Documentos.Rows) //recorrer el grid documentos hasta encontrar el ID del tipo de documento
                {
                    //si el ID del documento en la tabla anexos del movimiento es igual al de la fila del grid, agregar a la tabla y al grid
                    if (Fila_Anexo[Ope_Pre_Recep_Docs_Anexos.Campo_Documento_ID].ToString() == Fila_Grid.Cells[1].Text)
                    {
                        DataRow Fila_Nuevo_Documento = Tabla_Documentos.NewRow();

                        Lbl_Nombre_Archivo = (Label)Fila_Grid.FindControl("Lbl_Nombre_Archivo");
                        Chk_Tipo_Documento = (CheckBox)Fila_Grid.FindControl("Chk_Documento_Recibido");
                        if (Chk_Tipo_Documento != null)
                            Chk_Tipo_Documento.Checked = true;
                        // si se obtuvo el objeto etiqueta y el anexo contiene una ruta al archivo, asignar valor a la etiqueta
                        if (Lbl_Nombre_Archivo != null && !String.IsNullOrEmpty(Fila_Anexo[Ope_Pre_Recep_Docs_Anexos.Campo_Ruta].ToString()))
                        {
                            Lbl_Nombre_Archivo.Text = HttpUtility.HtmlDecode(Path.GetFileName(Fila_Anexo[Ope_Pre_Recep_Docs_Anexos.Campo_Ruta].ToString()));
                            Fila_Nuevo_Documento["RUTA_ARCHIVO"] = Fila_Anexo[Ope_Pre_Recep_Docs_Anexos.Campo_Ruta].ToString();
                        }
                        Fila_Nuevo_Documento["CLAVE_DOCUMENTO"] = Fila_Grid.Cells[1].Text;
                        Fila_Nuevo_Documento["NOMBRE_DOCUMENTO"] = Fila_Grid.Cells[2].Text;
                        Fila_Nuevo_Documento["NO_MOVIMIENTO"] = Fila_Anexo[Ope_Pre_Recep_Docs_Anexos.Campo_No_Anexo].ToString();
                        Tabla_Documentos.Rows.Add(Fila_Nuevo_Documento);
                        break;  // no recorrer las fila restantes del grid en busca del tipo de documento
                    }
                }
            }
            Session["Tabla_Documentos"] = Tabla_Documentos;
            Session["Datos_Movimiento"] = Dt_Movimiento;
            Session["Datos_Anexos_Movimiento"] = Dt_Anexos;
        }
        catch (Exception Ex)
        {
            throw new Exception("Cargar_Datos_Detalle_Tramite: " + Ex.Message.ToString(), Ex);
        }
    }


    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Grid_Detalles_Recepcion_PageIndexChanging
    /// 	DESCRIPCIÓN: Maneja el Evento de Cambio de Página del Grid de Notarios (cargar los datos del 
    /// 	            Notario seleccionado)
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 31-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Detalles_Recepcion_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //limpiar mensajes de error y campos de texto
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Ecabezado_Mensaje.Text = "";
        Lbl_Mensaje_Error.Text = "";

        try
        {
            Grid_Detalles_Recepcion.PageIndex = e.NewPageIndex;
            Grid_Detalles_Recepcion.DataSource = (DataTable)Session["Tabla_Detalles_Recepcion_Docs"];    //recuperar fuente de datos de variable de sesion
            Grid_Detalles_Recepcion.DataBind();
        }
        catch (Exception Ex)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Ecabezado_Mensaje.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Borrar_Registro
    ///DESCRIPCIÓN: Permite borrar un registro del grid y elimina el registro en la base de datos
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 06/Septiembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Borrar_Registro(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName.Equals("Erase"))
        {
            if (Grid_Detalles_Recepcion.Rows.Count > 0)
            {
                Int32 Registro = ((Grid_Detalles_Recepcion.PageIndex) *
                Grid_Detalles_Recepcion.PageSize) + (Convert.ToInt32(e.CommandArgument));

                if (Session["Tabla_Detalles_Recepcion_Docs"] != null)
                {
                    DataTable Tabla = (DataTable)Session["Tabla_Detalles_Recepcion_Docs"];
                    Cls_Ope_Pre_Recepcion_Documentos_Negocio Eliminar = new Cls_Ope_Pre_Recepcion_Documentos_Negocio();
                    Eliminar.P_No_Movimiento = Grid_Detalles_Recepcion.Rows[Convert.ToInt32(e.CommandArgument)].Cells[1].Text;
                    Tabla.Rows.RemoveAt(Registro);
                    Eliminar.Eliminar_Movimiento();
                    Session["Asignacion_Turnos"] = Tabla;
                    Grid_Detalles_Recepcion.PageIndex = 0;
                    Grid_Detalles_Recepcion.DataSource = Tabla;
                    Grid_Detalles_Recepcion.DataBind();
                }

            }
        }

    }


    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Limpiar_Controles
    /// 	DESCRIPCIÓN: Limpia los controles para agregar nuevo trámite o nueva recepcion
    /// 	PARÁMETROS:
    /// 	        1. Operacion: Indica si se limpian los controles para nueva recepcion
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 28-abr-2011 
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Limpiar_Controles(String Operacion)
    {
        //Limpiar los controles de tramite
        Hdn_Cuenta_Predial_ID.Value = "";
        Txt_Cuenta_Predial_1.Text = "";
        Txt_Cuenta_Predial.Text = "";
        //Txt_Nombre_Propietario.Text = "";
        //Txt_Ubicacion_Cuenta.Text = "";
        //Llenar_Grid_Documentos();
        Txt_Numero_Escritura.Text = "";
        Txt_Fecha_Escritura_Docs.Text = "";
        Txt_Comentarios.Text = "";
        //Si se indica en la Operacion, borrar datos de recepcion de documentos
        if (Operacion == "Nueva_Recepcion")
        {
            Session.Remove("Tabla_Tramites");       // eliminar sesiones
            Session.Remove("Tabla_Documentos");
            Session.Remove("Diccionario_Archivos");
            Grid_Tramites.DataSource = null;        // limpiar grid tramites
            Grid_Tramites.DataBind();
            //Limpiar_Campos_Busqueda_Cuentas();
        }
        else if (Operacion == "Salir")
        {
            Session.Remove("Tabla_Tramites");       // eliminar sesiones
            Session.Remove("Tabla_Documentos");
            Session.Remove("Diccionario_Archivos");
            //Txt_Notario_ID.Text = "";               // borrar datos de notario
            //Txt_Nombre_Notario.Text = "";
            //Txt_RFC_Notario.Text = "";
            //Txt_Numero_Notaria.Text = "";
            //Limpiar_Campos_Busqueda_Notarios();
            //Limpiar_Campos_Busqueda_Cuentas();
        }
        //Limpiar mensajes de error
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Lbl_Mensaje_Error.Visible = false;
        Img_Error_Tamite.Visible = false;
        Lbl_Error_Tramite.Text = "";
        Lbl_Error_Tramite.Visible = false;
    }

    private void Limpiar_Controles()
    {
        Txt_Busqueda_Recep_Docs.Text = "";
        Txt_Comentarios_Area.Text = "";
        Txt_Cuenta_Predial.Text = "";
        Txt_Fecha_Escritura.Text = "";
        Txt_Folio_Recepcion.Text = "";
        Txt_No_Escritura.Text = "";
        Cmb_Busqueda_Por.SelectedIndex = 0;
        Cmb_Estatus.SelectedIndex = 0;
        Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
        Session.Remove("CUENTA_PREDIAL_ID");
        Session.Remove("CUENTA_PREDIAL");
        Hdn_Cuenta_Predial_ID.Value = "";
        Session.Remove("Notario_Datos");
        Txt_Numero_Notaria.Text = "";
        Txt_Nombre_Notario.Text = "";
        Txt_RFC_Notario.Text = "";
        Txt_Cuenta_Predial_1.Text = "";
        Txt_Nombre_Propietario.Text = "";
        Txt_Ubicacion_Cuenta.Text = "";
        Txt_Notario_ID.Text = "";
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Resumen_Predio
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente del Resumen de Predial con la ruta y parámtros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Otubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Emergente_Resumen_Predio()
    {
        if (Txt_Cuenta_Predial.Text.Replace(",", "") != "")
        {
            String Ventana_Modal = "Abrir_Resumen('Ventanas_Emergentes/Frm_Resumen_Predio.aspx";
            String Propiedades = ", 'resizable=no,status=no,width=750,scrollbars=yes');";
            //String Propiedades = ", 'center:yes;resizable:no;status:no;dialogWidth:800px;dialogHeight:600px;dialogHide:true;help:no;scroll:no');";
            Btn_Detalles_Cuenta_Predial.Enabled = true;
            Btn_Detalles_Cuenta_Predial.Attributes.Remove("onclick");
            Btn_Detalles_Cuenta_Predial.Attributes.Add("onclick", Ventana_Modal + "?Cuenta_Predial=" + Txt_Cuenta_Predial.Text.Trim().Replace(",", "") + "'" + Propiedades);
        }
        else
        {
            Btn_Detalles_Cuenta_Predial.Attributes.Remove("onclick");
            Btn_Detalles_Cuenta_Predial.Enabled = false;
        }
    }

    #endregion

    #region Metodos Operacion
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Realizar_Contrarecibo
    ///DESCRIPCIÓN: reporte de contrarecibo
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 1/Agosto/2011 02:18:14 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Realizar_Contrarecibo()
    {
        DataRow Dr_Renglon_Actual;
        String Recepcion;
        String Movimiento;
        DataSet Ds_Contrarecibo;
        Ds_Ope_Pre_Contrarecibo Ds_Obj_Cosulta_Reporte = new Ds_Ope_Pre_Contrarecibo();

        Cls_Ope_Pre_Validacion_Recepcion_Negocio Validacion = new Cls_Ope_Pre_Validacion_Recepcion_Negocio();

        Dr_Renglon_Actual = (DataRow)Session["Dr_Movimiento"];
        Recepcion = Dr_Renglon_Actual["NO_RECEPCION_DOCUMENTO"].ToString();
        Movimiento = Dr_Renglon_Actual["NO_MOVIMIENTO"].ToString();
        //Dt_Tabla_Recepciones = ((DataSet)Session["Tabla_Recepciones"]).Tables[0];

        //Dr_Renglones_Recepcion = Dt_Tabla_Recepciones.Select("NO_RECEPCION_DOCUMENTO = " + Recepcion + " AND ESTATUS = 'PENDIENTE'" ) ;

        //if (Dr_Renglones_Recepcion.Length == 0)
        //{
        Validacion.P_No_Movimiento = Movimiento;
        Validacion.P_No_Recepcion_Documento = Recepcion;
        Ds_Contrarecibo = Validacion.Generar_Reporte_Contra_Recibo();
        Generar_Reporte(Ds_Contrarecibo);

        //}
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Observaciones
    ///DESCRIPCIÓN: se llena la tabla con las observaciones
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/27/2011 12:45:28 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Llenar_Tabla_Observaciones()
    {
        Cls_Ope_Pre_Validacion_Recepcion_Negocio Validacion_Negocio = new Cls_Ope_Pre_Validacion_Recepcion_Negocio();
        DataTable Dt_Observaciones;
        try
        {
            Tbl_Comentarios.Rows.Clear();

            Validacion_Negocio.P_No_Movimiento = Hdn_Movimiento_ID.Value;
            Dt_Observaciones = Validacion_Negocio.Consultar_Observaciones();
            if (Dt_Observaciones.Rows.Count > 0)
            {
                foreach (DataRow Dr_Observaciones in Dt_Observaciones.Rows)
                {
                    if (Dr_Observaciones["FECHA_MODIFICO"].ToString() != "")
                    {
                        HtmlTableRow Encabezado = new HtmlTableRow();
                        HtmlTableCell Encabezado_Fecha = new HtmlTableCell();
                        Encabezado_Fecha.Width = "35%";
                        Encabezado_Fecha.Style.Add("background-color", "#ddddff");
                        Encabezado_Fecha.Style.Add("font-weight", "bold");
                        Encabezado_Fecha.InnerText = Dr_Observaciones["FECHA_MODIFICO"].ToString();


                        HtmlTableCell Encabezado_Reviso = new HtmlTableCell();
                        Encabezado_Reviso.Width = "65%";
                        Encabezado_Reviso.Style.Add("background-color", "#ddddff");
                        Encabezado_Reviso.Style.Add("font-weight", "bold");
                        Encabezado_Reviso.InnerText = Dr_Observaciones["USUARIO_MODIFICO"].ToString();

                        Encabezado.Cells.Add(Encabezado_Fecha);
                        Encabezado.Cells.Add(Encabezado_Reviso);

                        HtmlTableRow Observaciones = new HtmlTableRow();
                        HtmlTableCell Texto = new HtmlTableCell();
                        Texto.ColSpan = 2;
                        Texto.InnerText = Dr_Observaciones["OBSERVACIONES"].ToString();

                        Observaciones.Cells.Add(Texto);

                        Tbl_Comentarios.Rows.Add(Encabezado);
                        Tbl_Comentarios.Rows.Add(Observaciones);
                    }
                }
            }
            else
            {
                HtmlTableRow No_Observaciones = new HtmlTableRow();
                HtmlTableCell Celda_Vacia = new HtmlTableCell();
                Celda_Vacia.ColSpan = 2;
                Celda_Vacia.InnerText = "No hay observaciones";

                No_Observaciones.Cells.Add(Celda_Vacia);

                Tbl_Comentarios.Rows.Add(No_Observaciones);
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Anexos
    ///DESCRIPCIÓN: se llena la tabla con los documentos entregados por el notario para su revision
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/27/2011 12:45:28 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Llenar_Tabla_Anexos()
    {
        Cls_Ope_Pre_Validacion_Recepcion_Negocio Validacion_Negocio = new Cls_Ope_Pre_Validacion_Recepcion_Negocio();
        DataTable Dt_Documentos_Recibidos;
        try
        {
            Tbl_Documentos_Recibidos.Rows.Clear();

            HtmlTableRow Encabezado = new HtmlTableRow();
            HtmlTableCell Encabezado_Documento = new HtmlTableCell();
            Encabezado_Documento.Width = "35%";
            Encabezado_Documento.Style.Add("background-color", "#ddddff");
            Encabezado_Documento.Style.Add("font-weight", "bold");
            Encabezado_Documento.InnerText = "Documento";

            HtmlTableCell Encabezado_Adjunto = new HtmlTableCell();
            Encabezado_Adjunto.Width = "65%";
            Encabezado_Adjunto.Style.Add("background-color", "#ddddff");
            Encabezado_Adjunto.Style.Add("font-weight", "bold");
            Encabezado_Adjunto.InnerText = "Archivo adjunto";

            Encabezado.Cells.Add(Encabezado_Documento);
            Encabezado.Cells.Add(Encabezado_Adjunto);

            Tbl_Documentos_Recibidos.Rows.Add(Encabezado);

            if (Txt_Folio_Recepcion.Text.Trim() != "")
            {
                Validacion_Negocio.P_No_Movimiento = Hdn_Movimiento_ID.Value;
                Dt_Documentos_Recibidos = Validacion_Negocio.Consultar_Anexos_Recepcion();
                if (Dt_Documentos_Recibidos.Rows.Count > 0)
                {
                    Session["Dt_Documentos_Anexos"] = Dt_Documentos_Recibidos;

                    foreach (DataRow Dr_Documento in Dt_Documentos_Recibidos.Rows)
                    {
                        HtmlTableRow Row = new HtmlTableRow();
                        HtmlTableCell Documento = new HtmlTableCell();
                        Documento.InnerText = Dr_Documento["NOMBRE_DOCUMENTO"].ToString();
                        string Ruta = Dr_Documento["RUTA"].ToString().Replace("\\", "/");

                        HtmlTableCell Adjunto = new HtmlTableCell();
                        Adjunto.InnerHtml = "<a style='color:#25406D;text-decoration:underline;'" + "href='" + Ruta + "' target='_blank'>" + Dr_Documento["NOMBRE_DOCUMENTO"].ToString() + ".jpg" + "</a>";
                        Row.Cells.Add(Documento);

                        if (!String.IsNullOrEmpty(Dr_Documento["RUTA"].ToString()))
                            Row.Cells.Add(Adjunto);
                        else
                            Row.Cells.Add(new HtmlTableCell());
                        Tbl_Documentos_Recibidos.Rows.Add(Row);
                    }
                }
                else
                {
                    HtmlTableRow Row = new HtmlTableRow();
                    HtmlTableCell Documento = new HtmlTableCell();
                    Documento.InnerText = "No se encontraron Registros";

                    HtmlTableCell Adjunto = new HtmlTableCell();
                    Adjunto.InnerHtml = "&nbsp;";

                    Row.Cells.Add(Documento);
                    Row.Cells.Add(Adjunto);

                    Tbl_Documentos_Recibidos.Rows.Add(Row);

                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    #endregion

    #region Metodos Reportes

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN:          Carga el data set físico con el cual se genera el Reporte especificado
    ///PARAMETROS:           1.-Data_Set_Consulta_Inventario.- Contiene la informacion de la consulta a la base de datos
    ///                      2.-Ds_Reporte_Stock, Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///                      3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           17/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataSet Ds_Reporte_Ordenes_Salida)
    {
        ReportDocument Reporte = new ReportDocument();
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";

        try
        {
            // Ruta donde se encuentra el reporte Crystal
            Ruta_Reporte_Crystal = "../Rpt/Predial/Rpt_Pre_Contrarecibo.rpt";

            // Se crea el nombre del reporte
            String Nombre_Reporte = "Rpt_Contrarecibo_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

            Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
            Cls_Reportes Reportes = new Cls_Reportes();
            Reportes.Generar_Reporte(ref Ds_Reporte_Ordenes_Salida, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, "PDF");
            Mostrar_Reporte(Nombre_Reporte_Generar, "PDF");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    /// *************************************************************************************
    /// NOMBRE:              Mostrar_Reporte
    /// DESCRIPCIÓN:         Muestra el reporte en pantalla.
    /// PARÁMETROS:          Nombre_Reporte_Generar.- Nombre que tiene el reporte que se mostrará en pantalla.
    ///                      Formato.- Variable que contiene el formato en el que se va a generar el reporte "PDF" O "Excel"
    /// USUARIO CREO:        Juan Alberto Hernández Negrete.
    /// FECHA CREO:          3/Mayo/2011 18:20 p.m.
    /// USUARIO MODIFICO:    Salvador Hernández Ramírez
    /// FECHA MODIFICO:      16-Mayo-2011
    /// CAUSA MODIFICACIÓN:  Se asigno la opción para que en el mismo método se muestre el reporte en excel
    /// *************************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte_Generar, String Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            if (Formato == "PDF")
            {
                Pagina = Pagina + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
                "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
            else if (Formato == "Excel")
            {
                String Ruta = "../../Reporte/" + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    #endregion

    #region Metodos/ABC
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Modificar_Validacion
    ///DESCRIPCIÓN: guarda el estatus resultante de la revision
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/27/2011 03:23:05 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Modificar_Validacion()
    {
        Cls_Ope_Pre_Validacion_Recepcion_Negocio Validacion = new Cls_Ope_Pre_Validacion_Recepcion_Negocio();
        try
        {
            DataRow Dr_Movimiento;
            if (Validar_Datos())
            {
                Dr_Movimiento = (DataRow)Session["Dr_Movimiento"];

                Validacion.P_Notario_ID = Dr_Movimiento["NOTARIO_ID"].ToString();
                Validacion.P_Numero_Escritura = Dr_Movimiento["NUMERO_ESCRITURA"].ToString();
                Validacion.P_Cuenta_Predial_ID = Dr_Movimiento["CUENTA_PREDIAL_ID"].ToString();
                Validacion.P_Fecha_Escritura = Dr_Movimiento["FECHA_ESCRITURA"].ToString();

                Validacion.P_Estatus_Movimiento = Cmb_Estatus.SelectedValue.ToString();

                Validacion.P_Observaciones = Txt_Comentarios_Area.Text.Trim().ToUpper();
                if (Txt_Comentarios_Area.Text.Trim().Length > 249)
                    Validacion.P_Observaciones = Txt_Comentarios_Area.Text.Trim().Substring(0, 249).ToUpper();
                Validacion.P_Empleado_Session = Cls_Sessiones.Nombre_Empleado.ToString().ToUpper();
                Validacion.P_No_Movimiento = Hdn_Movimiento_ID.Value;
                Validacion.Modificar_Recepcion_Movimiento();
                Cargar_Bandeja(0, false);

                if (Validacion.P_Estatus_Movimiento == "ACEPTADO")
                    Realizar_Contrarecibo();

                Estado_Botones(Const_Estado_Inicial);
                Session["Estado_Combo_Busqueda"] = "";
                Session["Valor_Combo_Busqueda"] = "0";
                Cargar_Bandeja(0, false);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validación de Recepción de documentos", "alert('La validación se guardó correctamente');", true);


            }

        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    //******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Validar_Acceso
    ///DESCRIPCIÓN: se validan los datos necesarios en la generacion de orden de variacion
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 09/17/2011 02:43:06 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Validar_Acceso()
    {
        Txt_Numero_Escritura.Enabled = true;
        Txt_Fecha_Escritura_Docs.Enabled = true;
        Grid_Documentos.Enabled = true;
        Txt_Comentarios.Enabled = true;

        if (Session["Estatus_Cuenta"] != null && Session["Tipo_Suspencion"] != null)
        {
            if (Session["Estatus_Cuenta"].ToString() == "BLOQUEADA" || (Session["Estatus_Cuenta"].ToString() == "SUSPENDIDA" && Session["Tipo_Suspencion"].ToString() != "PREDIAL"))
            {
                Mensaje_Error("Esta Cuenta no tiene Autorizados movimientos para Traslado");
                Txt_Cuenta_Predial.Text = "";
                Txt_Nombre_Propietario.Text = "";
                Txt_Ubicacion_Cuenta.Text = "";

                Txt_Numero_Escritura.Enabled = false;
                Txt_Fecha_Escritura_Docs.Enabled = false;
                Grid_Documentos.Enabled = false;
                Txt_Comentarios.Enabled = false;
            }
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Validar_Datos
    ///DESCRIPCIÓN: valida que se ingresen los datos requeridos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/27/2011 04:01:29 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private bool Validar_Datos()
    {
        Boolean Resultado = true;

        if (Txt_Comentarios_Area.Text.Trim() == "" || Txt_Comentarios_Area.Text.Trim() == null)
        {
            if (Cmb_Estatus.SelectedValue.ToString() != "ACEPTADO" && Txt_Comentarios_Area.Text.ToString().Trim() == "")
            {
                Mensaje_Error("Las observaciones son Obligatorias");
                Resultado = false;
            }
        }
        if (Cmb_Estatus.SelectedIndex <= 0)
        {
            Mensaje_Error("Ingrese el estatus de la validación de Documentos");
            Resultado = false;
        }

        return Resultado;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos
    ///DESCRIPCIÓN: metodo que recibe el DataRow seleccionado de la grilla y carga los datos en los componetes de l formulario
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 02:07:54 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Datos(DataRow Dr_Tabla)
    {
        try
        {
            if (Dr_Tabla["ESTATUS"].ToString() == "PENDIENTE")
            {
                Btn_Modificar.Enabled = true;
            }
            else
            {
                Btn_Modificar.Enabled = false;
            }

            Session["Dr_Movimiento"] = Dr_Tabla;
            Txt_Cuenta_Predial.Text = Dr_Tabla["CUENTA_PREDIAL"].ToString().ToUpper();
            Txt_Fecha_Escritura.Text = Dr_Tabla["FECHA_ESCRITURA"].ToString().ToUpper().Split(' ')[0];
            Txt_Folio_Recepcion.Text = Dr_Tabla["NO_RECEPCION_DOCUMENTO"].ToString().ToUpper();
            Txt_No_Escritura.Text = Dr_Tabla["NUMERO_ESCRITURA"].ToString().ToUpper();
            Cmb_Estatus.SelectedValue = Dr_Tabla["ESTATUS"].ToString().ToUpper();
            Hdn_Movimiento_ID.Value = Dr_Tabla["NO_MOVIMIENTO"].ToString().ToUpper();
            Llenar_Tabla_Anexos();
            Llenar_Tabla_Observaciones();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }


    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Recepciones_Movimientos
    /// DESCRIPCION: Consulta los movimientos del notario seleccionado y los muestra 
    ///             en el grid Recepciones_Notario
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 12-may-2011
    /// MODIFICO: Jesus Toledo
    /// FECHA_MODIFICO: Se cambio la busqueda de recepciones
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Bandeja(int Page_Index, Boolean Grid_Sorting)
    {
        Cls_Ope_Pre_Validacion_Recepcion_Negocio Rs_Consulta_Recep_Docs = new Cls_Ope_Pre_Validacion_Recepcion_Negocio();
        DataSet Ds_Recep_Docs = null;

        try
        {
            if (!Grid_Sorting)
            {
                Session["Tabla_Recepciones_Vista"] = null;
                Rs_Consulta_Recep_Docs.P_No_Recepcion_Documento = null;
                Rs_Consulta_Recep_Docs.P_Notario_ID = null;
                Rs_Consulta_Recep_Docs.P_Numero_Notaria = null;
                Rs_Consulta_Recep_Docs.P_Cuenta_Predial = null;
                Rs_Consulta_Recep_Docs.P_Numero_Escritura = null;

                if (Session["Estado_Combo_Busqueda"] == null)
                    Session["Estado_Combo_Busqueda"] = "='PENDIENTE'";

                if (Session["Estatus"] == null)
                    Session["Estatus"] = "";

                switch (Session["Estado_Combo_Busqueda"].ToString())
                {
                    case "FOLIO":
                        Int32 Numero_Recepcion_Doc = 0;
                        if (Int32.TryParse(Session["Valor_Combo_Busqueda"].ToString(), out Numero_Recepcion_Doc))
                        {
                            Rs_Consulta_Recep_Docs.P_No_Recepcion_Documento = String.Format("{0:0000000000}", Numero_Recepcion_Doc);
                        }
                        else
                        {
                            Rs_Consulta_Recep_Docs.P_No_Recepcion_Documento = "0";
                        }
                        Session["Estatus"] = "";
                        break;
                    case "NOTARIO":
                        Rs_Consulta_Recep_Docs.P_Nombre_Notario = Session["Valor_Combo_Busqueda"].ToString();
                        Session["Estatus"] = "";
                        break;
                    case "NOTARIA":
                        Rs_Consulta_Recep_Docs.P_Notario_ID = Session["Valor_Combo_Busqueda"].ToString();
                        Session["Estatus"] = "";
                        break;
                    case "CUENTA":
                        Rs_Consulta_Recep_Docs.P_Cuenta_Predial = Session["Valor_Combo_Busqueda"].ToString();
                        Session["Estatus"] = "";
                        break;
                    case "ESCRITURA":
                        Rs_Consulta_Recep_Docs.P_Numero_Escritura = Session["Valor_Combo_Busqueda"].ToString();
                        Session["Estatus"] = "";
                        break;
                    default:
                        Session["Estatus"] = "='PENDIENTE'";
                        break;
                }

                if (!String.IsNullOrEmpty(Session["Estatus"].ToString()))
                {
                    Rs_Consulta_Recep_Docs.P_Estatus_Movimiento = Session["Estatus"].ToString();
                }
                Ds_Recep_Docs = Rs_Consulta_Recep_Docs.Busqueda_Recepciones_Movimientos();
                if (Ds_Recep_Docs.Tables[0].Rows.Count > 0)
                {
                    Grid_Bandeja_Recepcion.PageIndex = Page_Index;
                    Grid_Bandeja_Recepcion.DataSource = Ds_Recep_Docs.Tables[0];
                    Grid_Bandeja_Recepcion.DataBind();
                    Session["Tabla_Recepciones"] = Ds_Recep_Docs;
                }
                else
                {
                    Grid_Bandeja_Recepcion.PageIndex = 0;
                    Grid_Bandeja_Recepcion.DataSource = null;
                    Grid_Bandeja_Recepcion.DataBind();
                    Session["Tabla_Recepciones"] = null;
                    Mensaje_Error("No hay Pendientes asignados");
                }
                if (!String.IsNullOrEmpty(Session["Estatus"].ToString()))
                {
                    Session["Estado_Combo_Busqueda"] = "";
                    Session["Valor_Combo_Busqueda"] = "0";
                }
                Txt_Busqueda_Recep_Docs.Text = "";
                Cmb_Busqueda_Por.SelectedIndex = 0;
            }
            else
            {
                Grid_Bandeja_Recepcion.PageIndex = Page_Index;
                Grid_Bandeja_Recepcion.DataSource = (DataView)Session["Tabla_Recepciones_Vista"];
                Grid_Bandeja_Recepcion.DataBind();
                DataSet obNewDt = new DataSet();
                DataTable Dt_Nueva = ((DataView)Session["Tabla_Recepciones_Vista"]).Table.Copy();
                obNewDt.Tables.Add(Dt_Nueva);
                Session["Tabla_Recepciones"] = obNewDt;

            }

        }

        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString(), ex);
        }
    }


    #endregion

    #region Eventos Botones
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Recepciones_Notario_Click
    ///DESCRIPCIÓN: busqueda de recepciones de documentos
    ///PARAMETROS: object sender, ImageClickEventArgs e
    ///CREO: jtoledo
    ///FECHA_CREO: 07/25/2011 02:29:36 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Recepciones_Notario_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Session["Estado_Combo_Busqueda"] = Cmb_Busqueda_Por.SelectedValue.ToString(); ;
            Session["Valor_Combo_Busqueda"] = Txt_Busqueda_Recep_Docs.Text.Trim().ToUpper();
            Cargar_Bandeja(0, false);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    #endregion

    #region Eventos/Grid



    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Bandeja_Recepcion_Selectedindexchanged
    ///DESCRIPCIÓN: se obtienen los datos del movimiento
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/26/2011 06:22:19 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Grid_Bandeja_Recepcion_Sorting(object sender, GridViewSortEventArgs e)
    {
        //Consulta_Empleados_Avanzada();
        String Orden = "";
        Cargar_Bandeja(0, false);
        DataTable Dt_Recepcion = ((DataSet)Session["Tabla_Recepciones"]).Tables[0];

        if (Dt_Recepcion != null)
        {
            DataView Dv_Recepcion = new DataView(Dt_Recepcion);
            if (ViewState["SortDirection"] != null)
                Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Recepcion.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Recepcion.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }
            Session["Tabla_Recepciones_Vista"] = Dv_Recepcion;
            Grid_Bandeja_Recepcion.DataSource = Dv_Recepcion;
            Grid_Bandeja_Recepcion.DataBind();
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Bandeja_Recepcion_Selectedindexchanged
    ///DESCRIPCIÓN: se obtienen los datos del movimiento
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/26/2011 06:22:19 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Grid_Bandeja_Recepcion_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Bandeja_Recepcion.SelectedIndex = (-1);
            if (Session["Tabla_Recepciones_Vista"] != null)
                Cargar_Bandeja(e.NewPageIndex, true);
            else
                Cargar_Bandeja(e.NewPageIndex, false);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Bandeja_Recepcion_Selectedindexchanged
    ///DESCRIPCIÓN: se obtienen los datos del movimiento
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/26/2011 06:22:19 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Grid_Bandeja_Recepcion_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Bandeja_Recepcion.SelectedIndex > (-1))
            {
                Btn_Modificar.Visible = true;
                Btn_Nuevo.Visible = false;
                //if (Session["Tabla_Recepciones_Vista"] == null)
                //{
                DataRow[] Renglon;
                DataTable obNewDt = ((DataSet)Session["Tabla_Recepciones"]).Tables[0];
                Renglon = obNewDt.Select("NO_MOVIMIENTO = " + Grid_Bandeja_Recepcion.SelectedRow.Cells[2].Text);
                Cargar_Datos(obNewDt.Rows[obNewDt.Rows.IndexOf(Renglon[0])]);
                //Cargar_Datos(((DataSet)Session["Tabla_Recepciones"]).Tables[0].Rows[Grid_Bandeja_Recepcion.SelectedIndex +
                //(Grid_Bandeja_Recepcion.PageIndex * Grid_Bandeja_Recepcion.PageSize)]);
                //}
                //else
                //{
                //    DataRow[] Renglon;
                //    DataTable obNewDt = ((DataView)Session["Tabla_Recepciones_Vista"]).Table.Copy();
                //    Renglon = obNewDt.Select("NO_MOVIMIENTO = " + Grid_Bandeja_Recepcion.SelectedRow.Cells[2].Text);                    
                //    Cargar_Datos(obNewDt.Rows[obNewDt.Rows.IndexOf(Renglon[0])]);
                //}

                Datos_Movimiento.Style.Value = "display:normal;";
                Div_Bandeja.Style.Value = "display:none;";
                Btn_Salir.AlternateText = "Regresar";
                Btn_Salir.ToolTip = "Regresar";
                Session["Estado_Combo_Busqueda"] = "";
                Session["Valor_Combo_Busqueda"] = "0";
                Cargar_Ventana_Emergente_Resumen_Predio();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    #endregion

    #region Modal

    protected void Btn_Mostrar_Busqueda_Notario_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Cat_Pre_Notarios_Negocio Notarios_Negocio = new Cls_Cat_Pre_Notarios_Negocio();
        DataRow Dr_Notario;

        try
        {
            Dr_Notario = (DataRow)Session["Notario_Datos"];

            if (Dr_Notario != null && Dr_Notario[Cat_Pre_Notarios.Campo_Notario_ID].ToString() != "")
            {
                Txt_Notario_ID.Text = Dr_Notario[Cat_Pre_Notarios.Campo_Notario_ID].ToString();
                Txt_Nombre_Notario.Text = Dr_Notario["NOMBRE_COMPLETO"].ToString();
                Txt_RFC_Notario.Text = Dr_Notario[Cat_Pre_Notarios.Campo_RFC].ToString();
                Txt_Numero_Notaria.Text = Dr_Notario[Cat_Pre_Notarios.Campo_Numero_Notaria].ToString();
            }
        }
        catch (Exception ex)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Ecabezado_Mensaje.Text = "Error Mostrar_Busqueda_Notario: " + ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Notarios
    /// DESCRIPCION: Consulta los Notarios que estan dados de alta en la BD, 
    ///         los almacena en una variable de sesión para consultas futuras
    /// PARAMETROS: 
    ///         1. Tipo_Busqueda: Especifica si se va a buscar con los datos en los 
    ///                 controles del panel modal o con los campos en la pagina principal
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 28-mar-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Notarios(String Tipo_Busqueda)
    {
        Cls_Ope_Pre_Recepcion_Documentos_Negocio Rs_Consulta_Notarios = new Cls_Ope_Pre_Recepcion_Documentos_Negocio();
        DataTable Dt_Notatios;

        try
        {
            Int32 Numero_Notario = 0;
            Int32.TryParse(Txt_Notario_ID.Text, out Numero_Notario);
            Rs_Consulta_Notarios.P_Notario_ID = String.Format("{0:00000}", Numero_Notario);
            Dt_Notatios = Rs_Consulta_Notarios.Consulta_Notarios(); //Consulta los Notarios con sus datos generales
            Session["Consulta_Notarios"] = Dt_Notatios;
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Notarios " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Recepciones_Movimientos
    /// DESCRIPCION: Consulta los movimientos del notario seleccionado y los muestra 
    ///             en el grid Recepciones_Notario
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 12-may-2011
    /// MODIFICO: Jesus Toledo
    /// FECHA_MODIFICO: 23-jul-2011
    /// CAUSA_MODIFICACION: Se cambio la busqueda de recepciones
    ///*******************************************************************************
    //private void Consulta_Recepciones_Movimientos()
    //{
    //    Cls_Ope_Pre_Recepcion_Documentos_Negocio Rs_Consulta_Recep_Docs = new Cls_Ope_Pre_Recepcion_Documentos_Negocio();
    //    DataTable Dt_Recep_Docs = null;

    //    try
    //    {
    //        if (Txt_Busqueda_Recep_Docs.Text.Length > 0)    //buscar numero en la caja de texto
    //        {
    //            Rs_Consulta_Recep_Docs.P_No_Recepcion_Documento = null;
    //            Rs_Consulta_Recep_Docs.P_Notario_ID = null;
    //            Rs_Consulta_Recep_Docs.P_Numero_Notaria = null;
    //            Rs_Consulta_Recep_Docs.P_Cuenta_Predial = null;
    //            Rs_Consulta_Recep_Docs.P_Numero_Escritura = null;


    //            switch (Cmb_Busqueda_Por.SelectedValue.ToString())
    //            {
    //                case "FOLIO":
    //                    Int32 Numero_Recepcion_Doc = 0;
    //                    if (Int32.TryParse(Txt_Busqueda_Recep_Docs.Text, out Numero_Recepcion_Doc))
    //                    {
    //                        Rs_Consulta_Recep_Docs.P_No_Recepcion_Documento = String.Format("{0:0000000000}", Numero_Recepcion_Doc);
    //                    }
    //                    else
    //                    {
    //                        Rs_Consulta_Recep_Docs.P_No_Recepcion_Documento = "0";
    //                    }

    //                    break;
    //                case "NOTARIO":
    //                    //Int32 Numero_Notario = 0;
    //                    //if (Int32.TryParse(Txt_Busqueda_Recep_Docs.Text, out Numero_Notario))
    //                    Rs_Consulta_Recep_Docs.P_Notario_ID = Txt_Busqueda_Recep_Docs.Text;
    //                    break;
    //                case "NOTARIA":
    //                    Rs_Consulta_Recep_Docs.P_Numero_Notaria = Txt_Busqueda_Recep_Docs.Text.Trim();
    //                    break;
    //                case "CUENTA":
    //                    Rs_Consulta_Recep_Docs.P_Cuenta_Predial = Txt_Busqueda_Recep_Docs.Text.Trim();
    //                    break;
    //                case "ESCRITURA":
    //                    Rs_Consulta_Recep_Docs.P_Numero_Escritura = Txt_Busqueda_Recep_Docs.Text.Trim();
    //                    break;

    //            }
    //            if (Cmb_Busqueda_Por.SelectedIndex > 0)
    //            {
    //                Dt_Recep_Docs = Rs_Consulta_Recep_Docs.Busqueda_Recepciones_Movimientos();
    //                if (Dt_Recep_Docs.Rows.Count > 0)
    //                {
    //                    //Rs_Consulta_Recep_Docs.Consulta_Recepciones_Movimientos(); //Consulta los Notarios con sus datos generales
    //                    //cargar datos en grid, solo ocultando el nombre de notario
    //                    Grid_Recepciones_Notario.Columns[3].Visible = true;
    //                    Grid_Recepciones_Notario.DataSource = Dt_Recep_Docs;
    //                    Grid_Recepciones_Notario.DataBind();
    //                    Session["Tabla_Recepciones_Notario"] = Dt_Recep_Docs;
    //                    Grid_Recepciones_Notario.Columns[3].Visible = false;

    //                }
    //                else
    //                {
    //                    Grid_Recepciones_Notario.DataSource = null;
    //                    Grid_Recepciones_Notario.DataBind();
    //                    Grid_Tramites.DataSource = null;
    //                    Grid_Tramites.DataBind();
    //                    Grid_Detalles_Recepcion.DataSource = null;
    //                    Grid_Detalles_Recepcion.DataBind();
    //                    Div_Contenedor_Msj_Error.Visible = true;
    //                    Lbl_Ecabezado_Mensaje.Text = "No se encotraron datos";
    //                }
    //                Cmb_Busqueda_Por.SelectedIndex = 0;
    //                Txt_Busqueda_Recep_Docs.Text = "";
    //            }
    //        }
    //        else
    //        {
    //            if (Txt_Nombre_Notario.Text.Length > 0) // si se encontro o selecciono un notario, buscar 
    //                Rs_Consulta_Recep_Docs.P_Notario_ID = Txt_Nombre_Notario.Text.Trim();
    //            Dt_Recep_Docs = Rs_Consulta_Recep_Docs.Busqueda_Recepciones_Movimientos(); //Consulta los Notarios con sus datos generales
    //            if (Dt_Recep_Docs.Rows.Count > 0)
    //            {
    //                //cargar datos en grid, solo ocultando el nombre de notario
    //                Grid_Recepciones_Notario.Columns[3].Visible = false;
    //                Eliminar_Filas_Repetidas(Dt_Recep_Docs);
    //                Dt_Recep_Docs.DefaultView.Sort = "FECHA DESC";
    //                Grid_Recepciones_Notario.DataSource = Dt_Recep_Docs.DefaultView;
    //                Grid_Recepciones_Notario.DataBind();
    //                Session["Tabla_Recepciones_Notario"] = Dt_Recep_Docs;
    //            }
    //            else
    //            {
    //                Grid_Recepciones_Notario.DataSource = null;
    //                Grid_Recepciones_Notario.DataBind();
    //                Grid_Tramites.DataSource = null;
    //                Grid_Tramites.DataBind();
    //                Grid_Detalles_Recepcion.DataSource = null;
    //                Grid_Detalles_Recepcion.DataBind();
    //                Div_Contenedor_Msj_Error.Visible = true;
    //                Lbl_Ecabezado_Mensaje.Text = ("No se encotraron datos");

    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception("Consulta_Recepciones_Movimientos: " + ex.Message.ToString(), ex);
    //    }
    //}
    private DataTable Eliminar_Filas_Repetidas(DataTable Dt_A_Filtrar)
    {
        String No_recepcion_documento1 = "a";
        String No_recepcion_documento2 = "";
        foreach (DataRow Renglon_Actual in Dt_A_Filtrar.Rows)
        {
            if (No_recepcion_documento1 == Renglon_Actual["NO_RECEPCION_DOCUMENTO"].ToString())
            {
                No_recepcion_documento2 = No_recepcion_documento1;
                No_recepcion_documento1 = Renglon_Actual["NO_RECEPCION_DOCUMENTO"].ToString();
                Renglon_Actual.Delete();
            }
            else
            {
                No_recepcion_documento2 = No_recepcion_documento1;
                No_recepcion_documento1 = Renglon_Actual["NO_RECEPCION_DOCUMENTO"].ToString();
            }
        }
        return Dt_A_Filtrar;
    }
    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Grid_Recepciones_Notario_PageIndexChanging
    /// 	DESCRIPCIÓN: Maneja el Evento de Cambio de Página del Grid de Recepciones Notario 
    /// 	            (cargar los datos de la pagina correspondiente)
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 13-may-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    //protected void Grid_Recepciones_Notario_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    //limpiar mensajes de error y campos de texto
    //    Div_Contenedor_Msj_Error.Visible = false;
    //    Lbl_Ecabezado_Mensaje.Text = "";
    //    Lbl_Mensaje_Error.Text = "";

    //    try
    //    {
    //        Grid_Recepciones_Notario.PageIndex = e.NewPageIndex;
    //        Grid_Recepciones_Notario.DataSource = (DataTable)Session["Tabla_Recepciones_Notario"];
    //        Grid_Recepciones_Notario.DataBind();
    //    }
    //    catch (Exception Ex)
    //    {
    //        Div_Contenedor_Msj_Error.Visible = true;
    //        Lbl_Ecabezado_Mensaje.Text = Ex.Message.ToString();
    //    }
    //}
    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Grid_Recepciones_Notario_SelectedIndexChanged
    /// 	DESCRIPCIÓN: Manejo del evento SelectedIndexChanged en el grid Tramites
    /// 	            Consulta los detalles (documentos recibidos) del movimiento o tramite seleccionado
    /// 	            y muestra el resultado en el grid detalles recepcion
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 13-may-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    //protected void Grid_Recepciones_Notario_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Cls_Ope_Pre_Recepcion_Documentos_Negocio Rs_Consulta_Detalles_Recepcion = new Cls_Ope_Pre_Recepcion_Documentos_Negocio();
    //    DataTable Dt_Det_Recep_Docs;
    //    String Numero_Recepcion;
    //    //limpiar mensajes de error y campos de texto
    //    Div_Contenedor_Msj_Error.Visible = false;
    //    Lbl_Ecabezado_Mensaje.Text = "";
    //    Lbl_Mensaje_Error.Text = "";

    //    try
    //    {
    //        Numero_Recepcion = Grid_Recepciones_Notario.SelectedRow.Cells[1].Text;
    //        //Btn_Imprimir.Visible = true;
    //        Hdn_Recep_Docs.Value = Numero_Recepcion;

    //        Dt_Det_Recep_Docs = Rs_Consulta_Detalles_Recepcion.Consulta_Detalles_Movimientos_Recepcion(Numero_Recepcion); //Consulta los Notarios con sus datos generales
    //        //cargar datos en grid detalles recepcion
    //        Grid_Detalles_Recepcion.DataSource = Dt_Det_Recep_Docs;
    //        Grid_Detalles_Recepcion.DataBind();
    //        Limpiar_Controles("Nueva_Recepcion");   //limpiar controles y ocultar paneles 
    //        //Pnl_Contenedor_Datos_Cuenta_Predial.Visible = false;
    //        Pnl_Contenedor_Datos_Recepcion_Documentos.Style.Value = "display:none;";
    //        Div_Notarios.Style.Value = "display:none;";
    //        Session["Tabla_Detalles_Recepcion_Docs"] = Dt_Det_Recep_Docs; // guardar en variable de sesion
    //        Grid_Detalles_Recepcion.SelectedIndex = -1;

    //        //if (Txt_Nombre_Notario.Text == "")  //si no hay nombre de notario, escribir datos de notario
    //        //{
    //        DataTable Tabla_Recepciones_Notario = (DataTable)Session["Tabla_Recepciones_Notario"];
    //        if (Tabla_Recepciones_Notario != null)      // si se recupero la tabla de recepciones notarios de la variable de sesion
    //        {
    //            int index = (Grid_Recepciones_Notario.SelectedIndex + (Grid_Recepciones_Notario.PageIndex * Grid_Recepciones_Notario.PageSize));
    //            // agregar al nombre de notario y actualizar datos de notario
    //            Txt_Notario_ID.Text = Tabla_Recepciones_Notario.Rows[index][Ope_Pre_Recepcion_Documentos.Campo_Notario_ID].ToString();
    //            Txt_Nombre_Notario.Text = Tabla_Recepciones_Notario.Rows[index]["NOMBRE_NOTARIO"].ToString();
    //            Txt_Numero_Notaria.Text = Tabla_Recepciones_Notario.Rows[index][Cat_Pre_Notarios.Campo_Numero_Notaria].ToString();
    //            Txt_RFC_Notario.Text = Tabla_Recepciones_Notario.Rows[index][Cat_Pre_Notarios.Campo_RFC].ToString();
    //            //Txt_Notario_ID_TextChanged(null, EventArgs.Empty);
    //        }
    //        //}
    //        else
    //        {

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Div_Contenedor_Msj_Error.Visible = true;
    //        Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
    //    }
    //}

    #endregion

    #region Eventos Botones

    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //if (Txt_Nombre_Notario.Text.Length > 0)     // verificar que ya haya un notario seleccionado
            //{
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Div_Bandeja.Style.Value = "display:none;";
                Limpiar_Controles("Nueva_Recepcion");    //Limpia los controles de la forma para poder introducir nuevos datos
                Limpiar_Grid_Documentos();
                Llenar_Grid_Documentos();
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario                    
                Pnl_Contenedor_Datos_Recepcion_Documentos.Style.Value = "display:inline;";
                Div_Notarios.Style.Value = "display:inline;";
                Session.Remove("Tabla_Detalles_Recepcion_Docs");
                Session.Remove("Datos_Movimiento");
                Session.Remove("Datos_Anexos_Movimiento");
            }
            else
            {
                Validar_Tramites_Recepcion();

                //Si faltaron campos por capturar envía un mensaje al usuario indicando cuáles
                if (Lbl_Mensaje_Error.Text.Length > 0)
                {
                    //Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario: <br />" + Lbl_Mensaje_Error.Text;
                }
                else       //Si no hay mensaje de error, dar de alta la recepcion de documentos
                {
                    Alta_Recepcion_Documentos(); //Da de alta los datos proporcionados por el usuario
                }
            }
            //}
            //else
            //{
            //    Div_Contenedor_Msj_Error.Visible = true;
            //    Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar un notario";
            //}
        }
        catch (Exception ex)
        {
            //Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llenar_Grid_Documentos
    /// DESCRIPCION: Llena el grid Documentos con los tipos de documento en la 
    ///             base de datos, con un checkbox para marcar los documentos recibidos
    ///             y un control fileupload para enviar al servidor una copia del documento
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-may-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llenar_Grid_Documentos()
    {
        Cls_Cat_Pre_Tipos_Documento_Negocio Rs_Tipos_Documento = new Cls_Cat_Pre_Tipos_Documento_Negocio();
        DataTable Dt_Tipos_Documento;      //Variable que obtendrá los datos de la consulta

        DataRow Nueva_Fila;
        DataTable Tabla_Documentos = Generar_Tabla_Documentos();

        // limpiar el grid
        //Grid_Documentos.DataSource = null;
        //Grid_Documentos.DataBind();

        try
        {
            Dt_Tipos_Documento = (DataTable)Rs_Tipos_Documento.Consulta_Tipos_Documento();
            foreach (DataRow Fila in Dt_Tipos_Documento.Rows)   // Agregar una fila a la tabla de tipos de documentos en la pagina por cada fila recibida de la base de datos
            {
                Nueva_Fila = Tabla_Documentos.NewRow();     //nueva fila
                // agregar controles y asignar valores a la nueva fila
                Nueva_Fila["CLAVE_DOCUMENTO"] = Fila[0].ToString();
                Nueva_Fila["NOMBRE_DOCUMENTO"] = Fila[1].ToString();

                Tabla_Documentos.Rows.Add(Nueva_Fila);      // agregar la fila creada al datatable de documentos
            }

            // mostrar tabla en el grid Documentos
            Grid_Documentos.Columns[1].Visible = true;
            Grid_Documentos.DataSource = Tabla_Documentos;
            Grid_Documentos.DataBind();
            Grid_Documentos.Columns[1].Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Grid_Documentos " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Obtener_Diccionario_Archivos
    /// 	DESCRIPCIÓN: Regresa el diccionario checksum-archivo si se encuentra en variable de sesion y si no,
    /// 	            regresa un diccionario vacio
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 04-may-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Dictionary<String, Byte[]> Obtener_Diccionario_Archivos()
    {
        Dictionary<String, Byte[]> Diccionario_Archivos = new Dictionary<String, Byte[]>();

        // si existe el diccionario en variable de sesion
        if (Session["Diccionario_Archivos"] != null)
        {
            Diccionario_Archivos = (Dictionary<String, Byte[]>)Session["Diccionario_Archivos"];
        }

        return Diccionario_Archivos;
    }
    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Establecer_Ruta_Archivos
    /// 	DESCRIPCIÓN: Agrega el nombre de directorio relativo 
    /// 	        Recepcion_Documentos\CURP_Notario\Separador
    /// 	PARÁMETROS:
    /// 	            1. Archivo: Nombre de archivo al que se asignará ruta relativa
    /// 	            2. Subdirectorio: Nombre del directorio donde se almacenará 
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 10-may-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private String Establecer_Ruta_Archivos(String Archivo, String Subdirectorio)
    {
        try
        {
            return @"Recepcion_Documentos\" + "rcp" + @"\" + Subdirectorio + @"\" + Archivo;

        }
        catch (Exception Ex)
        {
            throw new Exception("Establecer_Ruta_Archivos: " + Ex.Message.ToString(), Ex);
        }
    }
    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Obtener_Lista_Documentos
    /// 	DESCRIPCIÓN: Lee la tabla documentos desde variable de sesión y regresa mediante las 
    /// 	            variables pasadas como parámetro cadenas de caracteres con los datos de 
    /// 	            los documentos seleccionados, tambien recorre el grid Documentos y si
    /// 	            encuentra documentos seleccionados que no se hayan agregado desde la
    /// 	            tabla, los incluye tambien
    /// 	PARÁMETROS:
    /// 		1. Claves_Docs: Escribir las claves de los tipos de documento separadas por coma
    /// 		2. Nombres_Docs: Escribir los nombre de los tipos de documento seleccionados, separados por coma
    /// 		3. Nombres_Archivos: Escribir en esta variable los nombres de los archivos recibidos
    /// 		4. Checksums: Escribir los checksums de los archivos recibidos de cada tipo de documento
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 09-may-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Obtener_Lista_Documentos(out String Claves_Docs, out String Nombres_Docs, out String Nombres_Archivos, out String Checksums)
    {
        DataTable Tabla_Documentos;
        Dictionary<String, Byte[]> Diccionario_Archivos = Obtener_Diccionario_Archivos();   //obtener diccionario checksum-archivo

        Claves_Docs = "";
        Nombres_Docs = "";
        Nombres_Archivos = "";
        Checksums = "";

        if (Session["Tabla_Documentos"] != null)    //si hay datos en la tabla de documentos
        {
            Tabla_Documentos = (DataTable)Session["Tabla_Documentos"];
            foreach (DataRow Fila in Tabla_Documentos.Rows)  //recorrer las filas del grid documentos
            {
                Nombres_Docs += HttpUtility.HtmlDecode(Fila["NOMBRE_DOCUMENTO"].ToString().Trim()) + ", ";
                Claves_Docs += Fila["CLAVE_DOCUMENTO"].ToString().Trim() + ",";
                // Si existe un nombre de archivo y un checksum agregar a la nueva fila
                if (Fila["CHECKSUM"].ToString() != ""
                    && Fila["NOMBRE_ARCHIVO"].ToString() != ""
                    && Diccionario_Archivos.ContainsKey(Fila["CHECKSUM"].ToString()))
                {
                    Nombres_Archivos += HttpUtility.HtmlDecode(Fila["NOMBRE_ARCHIVO"].ToString().Trim()) + ",";
                    Checksums += Fila["CHECKSUM"].ToString().Trim() + ",";
                }
                else        // Si no hay archivo, agregar coma para que al convertir a arreglo tengan el mismo indice
                {
                    Nombres_Archivos += ",";
                    Checksums += ",";
                }
            }
            foreach (GridViewRow Fila_Grid in Grid_Documentos.Rows)  //recorrer las filas del grid documentos en busca de un checkbox activado
            {
                CheckBox Chk_Orden = (CheckBox)Fila_Grid.FindControl("Chk_Documento_Recibido");
                Label Lbl_Nombre_Archivo = (Label)Fila_Grid.FindControl("Lbl_Nombre_Archivo");  // limpiar el texto de la etiqueta
                if (Lbl_Nombre_Archivo != null && Lbl_Nombre_Archivo.Text != "")
                    Lbl_Nombre_Archivo.Text = "";
                // agregar los documentos seleccionados (que no esten ya en la lista de documentos recibidos) a la tabla
                if (Chk_Orden != null && Chk_Orden.Checked && !Claves_Docs.Contains(Fila_Grid.Cells[1].Text))
                {
                    Nombres_Docs += HttpUtility.HtmlDecode(Fila_Grid.Cells[2].Text) + ", ";
                    Claves_Docs += Fila_Grid.Cells[1].Text + ",";
                    Nombres_Archivos += ",";
                    Checksums += ",";
                    Chk_Orden.Checked = false;
                }
                else if (Chk_Orden != null && Chk_Orden.Checked)    // cambiar estado de checkbox
                {
                    Chk_Orden.Checked = false;
                }
            }
        }
        else            // si no hay tabla, tomar datos de los checkbox activados
        {
            foreach (GridViewRow Fila_Grid in Grid_Documentos.Rows)  //recorrer las filas del grid documentos en busca de un checkbox activado
            {
                CheckBox Chk_Orden = (CheckBox)Fila_Grid.FindControl("Chk_Documento_Recibido");
                Label Lbl_Nombre_Archivo = (Label)Fila_Grid.FindControl("Lbl_Nombre_Archivo");  // limpiar el texto de la etiqueta
                if (Lbl_Nombre_Archivo != null && Lbl_Nombre_Archivo.Text != "")
                    Lbl_Nombre_Archivo.Text = "";
                // agregar los documentos seleccionados (que no esten ya en la lista de documentos recibidos) a la tabla
                if (Chk_Orden != null && Chk_Orden.Checked && !Claves_Docs.Contains(Fila_Grid.Cells[1].Text))
                {
                    Nombres_Docs += HttpUtility.HtmlDecode(Fila_Grid.Cells[2].Text) + ", ";
                    Claves_Docs += Fila_Grid.Cells[1].Text + ",";
                    Nombres_Archivos += ",";
                    Checksums += ",";
                    Chk_Orden.Checked = false;
                }
                else if (Chk_Orden != null && Chk_Orden.Checked)    // cambiar estado de checkbox
                {
                    Chk_Orden.Checked = false;
                }
            }
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Asignar_Ruta_Relativa_Archivos
    /// 	DESCRIPCIÓN: Lee los nombres de archivos contenidos en la tabla tramites y les 
    /// 	        agrega el nombre de directorio relativo 
    /// 	        Recepcion_Documentos\CURP_Notario\Cuenta_Predial y si no hay cuenta predial, Escritura_Numero
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 10-may-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Asignar_Ruta_Relativa_Archivos()
    {
        DataTable Tabla_Tramites = (DataTable)Session["Tabla_Tramites"];
        String Nombre_Directorio;
        Dictionary<String, Byte[]> Diccionario_Archivos = Obtener_Diccionario_Archivos();

        try
        {
            if (Tabla_Tramites != null)     //si la tabla tramites contiene datos
            {
                foreach (DataRow Fila_Tramite in Tabla_Tramites.Rows)   // recorrer la tabla
                {
                    if (!String.IsNullOrEmpty(Fila_Tramite["CUENTA_PREDIAL"].ToString()))     //si hay una cuenta predial, asignar como nombre de subdirectorio
                    {
                        Nombre_Directorio = @"NCP_" + Fila_Tramite["CUENTA_PREDIAL"].ToString();
                    }
                    else            // si no, asignar como nombre de subdirectorio el numero de escritura
                    {
                        Nombre_Directorio = @"Escritura_" + Fila_Tramite["NO_ESCRITURA"].ToString();
                    }
                    //separar los nombres de archivo en un arreglo
                    String[] Arr_Nombres_Archivos = Fila_Tramite["NOMBRES_ARCHIVO"].ToString().Split(',');
                    Fila_Tramite["NOMBRES_ARCHIVO"] = "";
                    for (int i = 0; i < Arr_Nombres_Archivos.Length - 1; i++)   //recorrer el arreglo de nombres de archivo
                    {
                        // Si contiene un nombre de archivo y no contiene diagonales, agregar ruta relativa
                        if (Arr_Nombres_Archivos[i] != "" && !Arr_Nombres_Archivos[i].Contains(@"\"))
                        {
                            // actualizar nombres de archivos (ruta completa)
                            Fila_Tramite["NOMBRES_ARCHIVO"] += Establecer_Ruta_Archivos(Arr_Nombres_Archivos[i], Nombre_Directorio) + ",";
                        }
                        else
                        {
                            // dejar valor original
                            Fila_Tramite["NOMBRES_ARCHIVO"] += Arr_Nombres_Archivos[i] + ",";
                        }
                    }
                }
                Session["Tabla_Tramites"] = Tabla_Tramites;                            // guardar cambios a la tabla tramites en variable de sesion
            }
            else            // si no hay tramites, buscar en los anexos nuevos o modificados (es una modificacin)
            {
                DataTable Tabla_Anexos_Alta = (DataTable)Session["Tabla_Anexos_Alta"];
                DataTable Tabla_Anexos_Actualizar = (DataTable)Session["Tabla_Anexos_Actualizar"];
                if (Tabla_Anexos_Alta != null)     //si la tabla anexos alta contiene datos
                {
                    foreach (DataRow Fila_Anexo in Tabla_Anexos_Alta.Rows)   // recorrer la tabla
                    {
                        // si contiene un checksum y ese checksum existe en el diccionario checksum-archivo
                        if (Fila_Anexo["NOMBRE_ARCHIVO"].ToString() != "" && Diccionario_Archivos.ContainsKey(Fila_Anexo["CHECKSUM"].ToString()))
                        {
                            if (!String.IsNullOrEmpty(Txt_Cuenta_Predial_1.Text))     //si hay una cuenta predial, asignar como nombre de subdirectorio
                            {
                                Nombre_Directorio = @"NCP_" + Txt_Cuenta_Predial_1.Text.Replace(",", "");
                            }
                            else            // si no, asignar como nombre de subdirectorio el numero de escritura
                            {
                                Nombre_Directorio = @"Escritura_" + Txt_Numero_Escritura.Text;
                            }
                            Fila_Anexo["NOMBRE_ARCHIVO"] = Establecer_Ruta_Archivos(Fila_Anexo["NOMBRE_ARCHIVO"].ToString(), Nombre_Directorio);
                        }
                    }
                    Session["Tabla_Anexos_Alta"] = Tabla_Anexos_Alta;
                }
                if (Tabla_Anexos_Actualizar != null)     //si la tabla Anexos_Actualizar contiene datos
                {
                    foreach (DataRow Fila_Anexo in Tabla_Anexos_Actualizar.Rows)   // recorrer la tabla
                    {
                        // si contiene un checksum y ese checksum existe en el diccionario checksum-archivo
                        if (Fila_Anexo["NOMBRE_ARCHIVO"].ToString() != "" && Diccionario_Archivos.ContainsKey(Fila_Anexo["CHECKSUM"].ToString()))
                        {
                            if (!String.IsNullOrEmpty(Txt_Cuenta_Predial_1.Text))     //si hay una cuenta predial, asignar como nombre de subdirectorio
                            {
                                Nombre_Directorio = @"NCP_" + Txt_Cuenta_Predial_1.Text.Replace(",", "");
                            }
                            else            // si no, asignar como nombre de subdirectorio el numero de escritura
                            {
                                Nombre_Directorio = @"Escritura_" + Txt_Numero_Escritura.Text;
                            }
                            Fila_Anexo["NOMBRE_ARCHIVO"] = Establecer_Ruta_Archivos(Fila_Anexo["NOMBRE_ARCHIVO"].ToString(), Nombre_Directorio);
                        }
                    }
                    Session["Tabla_Anexos_Actualizar"] = Tabla_Anexos_Actualizar;
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Asignar_Ruta_Relativa_Archivos: " + Ex.Message.ToString(), Ex);
        }
    }
    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Recargar_Nombres_Archivo_Grid_Documentos
    /// 	DESCRIPCIÓN: Lee la tabla de Documentos desde la variable de sesion y vuelve a 
    /// 	            cargar los nombres de archivo, se borran al hacer un postback
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 12-may-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Recargar_Nombres_Archivo_Grid_Documentos()
    {
        DataTable Tabla_Documentos = (DataTable)Session["Tabla_Documentos"];
        Label Lbl_Nombre_Archivo;

        if (Tabla_Documentos != null)
        {
            foreach (DataRow Fila_Tabla_Docs in Tabla_Documentos.Rows)
            {
                foreach (GridViewRow Fila_Grid in Grid_Documentos.Rows) //recorrer el grid documentos hasta encontrar el ID del tipo de documento
                {
                    if (Fila_Tabla_Docs["CLAVE_DOCUMENTO"].ToString() == Fila_Grid.Cells[1].Text) //si el ID del documento en el arreglo, es igual al de la fila del grid
                    {
                        Lbl_Nombre_Archivo = (Label)Fila_Grid.FindControl("Lbl_Nombre_Archivo");
                        // si hay un nombre de archivo, mostrar el nombre en la etiqueta
                        if (Lbl_Nombre_Archivo != null && !String.IsNullOrEmpty(Fila_Tabla_Docs["NOMBRE_ARCHIVO"].ToString()))
                            Lbl_Nombre_Archivo.Text = HttpUtility.HtmlDecode(Path.GetFileName(Fila_Tabla_Docs["NOMBRE_ARCHIVO"].ToString()));
                        break;  // no recorrer las fila restantes del grid en busca del tipo de documento
                    }
                }
            }
        }
    }
    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Guardar_Archivos
    /// 	DESCRIPCIÓN: Guardar en el servidor los archivos que se hayan recibido
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 10-may-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Guardar_Archivos()
    {
        DataTable Tabla_Tramites = (DataTable)Session["Tabla_Tramites"];
        Dictionary<String, Byte[]> Diccionario_Archivos = Obtener_Diccionario_Archivos();
        String Nombre_Directorio;
        String Ruta_Archivo;


        try
        {
            if (Tabla_Tramites != null)     //si la tabla tramites contiene datos
            {
                foreach (DataRow Fila_Tramite in Tabla_Tramites.Rows)   // recorrer la tabla
                {
                    // separar los nombres de archivo y los checksums en arreglos
                    String[] Arr_Nombres_Archivos = Fila_Tramite["NOMBRES_ARCHIVO"].ToString().Split(',');
                    String[] Arr_Checksum = Fila_Tramite["CHECKSUM"].ToString().Split(',');
                    for (int i = 0; i < Arr_Checksum.Length - 1; i++)   //recorrer el arreglo de checksums
                    {
                        // si contiene un checksum y ese checksum existe en el diccionario checksum-archivo
                        if (Arr_Checksum[i] != "" && Diccionario_Archivos.ContainsKey(Arr_Checksum[i]))
                        {
                            Nombre_Directorio = MapPath(Path.GetDirectoryName(Arr_Nombres_Archivos[i]));
                            Ruta_Archivo = MapPath(HttpUtility.HtmlDecode(Arr_Nombres_Archivos[i]));
                            if (!Directory.Exists(Nombre_Directorio))                       //si el directorio no existe, crearlo
                                Directory.CreateDirectory(Nombre_Directorio);
                            //crear filestream y binarywriter para guardar archivo
                            FileStream Escribir_Archivo = new FileStream(Ruta_Archivo, FileMode.Create, FileAccess.Write);
                            BinaryWriter Datos_Archivo = new BinaryWriter(Escribir_Archivo);

                            // Guardar archivo (escribir datos en el filestream)
                            Datos_Archivo.Write(Diccionario_Archivos[Arr_Checksum[i]]);
                        }
                    }
                }
            }
            else            // si no hay tramites, buscar en los anexos nuevos o modificados (es una modificacin)
            {
                DataTable Tabla_Anexos_Alta = (DataTable)Session["Tabla_Anexos_Alta"];
                DataTable Tabla_Anexos_Actualizar = (DataTable)Session["Tabla_Anexos_Actualizar"];
                if (Tabla_Anexos_Alta != null)     //si la tabla anexos alta contiene datos
                {
                    foreach (DataRow Fila_Anexo in Tabla_Anexos_Alta.Rows)   // recorrer la tabla
                    {
                        // si contiene un checksum y ese checksum existe en el diccionario checksum-archivo
                        if (Fila_Anexo["NOMBRE_ARCHIVO"].ToString() != "" && Diccionario_Archivos.ContainsKey(Fila_Anexo["CHECKSUM"].ToString()))
                        {
                            Nombre_Directorio = MapPath(Path.GetDirectoryName(Fila_Anexo["NOMBRE_ARCHIVO"].ToString()));
                            Ruta_Archivo = MapPath(HttpUtility.HtmlDecode(Fila_Anexo["NOMBRE_ARCHIVO"].ToString()));
                            if (!Directory.Exists(Nombre_Directorio))                       //si el directorio no existe, crearlo
                                Directory.CreateDirectory(Nombre_Directorio);
                            //crear filestream y binarywriter para guardar archivo
                            FileStream Escribir_Archivo = new FileStream(Ruta_Archivo, FileMode.Create, FileAccess.Write);
                            BinaryWriter Datos_Archivo = new BinaryWriter(Escribir_Archivo);

                            // Guardar archivo (escribir datos en el filestream)
                            Datos_Archivo.Write(Diccionario_Archivos[Fila_Anexo["CHECKSUM"].ToString()]);
                        }
                    }
                }
                if (Tabla_Anexos_Actualizar != null)     //si la tabla Anexos_Actualizar contiene datos
                {
                    foreach (DataRow Fila_Anexo in Tabla_Anexos_Actualizar.Rows)   // recorrer la tabla
                    {
                        // si contiene un checksum y ese checksum existe en el diccionario checksum-archivo
                        if (Fila_Anexo["NOMBRE_ARCHIVO"].ToString() != "" && Diccionario_Archivos.ContainsKey(Fila_Anexo["CHECKSUM"].ToString()))
                        {
                            Nombre_Directorio = MapPath(Path.GetDirectoryName(Fila_Anexo["NOMBRE_ARCHIVO"].ToString()));
                            Ruta_Archivo = MapPath(HttpUtility.HtmlDecode(Fila_Anexo["NOMBRE_ARCHIVO"].ToString()));
                            if (!Directory.Exists(Nombre_Directorio))                       //si el directorio no existe, crearlo
                                Directory.CreateDirectory(Nombre_Directorio);
                            //crear filestream y binarywriter para guardar archivo
                            FileStream Escribir_Archivo = new FileStream(Ruta_Archivo, FileMode.Create, FileAccess.Write);
                            BinaryWriter Datos_Archivo = new BinaryWriter(Escribir_Archivo);

                            // Guardar archivo (escribir datos en el filestream)
                            Datos_Archivo.Write(Diccionario_Archivos[Fila_Anexo["CHECKSUM"].ToString()]);
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Guardar_Archivos " + Ex.Message.ToString(), Ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Alta_Recepcion_Documentos
    /// 	DESCRIPCIÓN: Dar de alta la recepcion de documentos en la base de datos
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 09-may-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Alta_Recepcion_Documentos()
    {
        Presidencia.Operacion_Predial_Recepcion_Documentos.Negocio.Cls_Ope_Pre_Recepcion_Documentos_Negocio Rs_Recepcion = new Cls_Ope_Pre_Recepcion_Documentos_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        Cls_Ope_Pre_Validacion_Recepcion_Negocio Validacion = new Cls_Ope_Pre_Validacion_Recepcion_Negocio();
        DataTable Dt_Agregar = (DataTable)Session["Tabla_Tramites"];
        String ID_Recepcion = "0";
        int ID_Int_Recepcion = 0;
        DataSet Ds_Contrarecibo;
        try
        {
            Agregar_Recepcion();

            Asignar_Ruta_Relativa_Archivos();     // llamar al metodo que actualiza el nombre de archivo por la  ruta relativa
            if (Txt_Bandera_Modifica_tramite_o_Recepcion.Text == "")
            {
                Rs_Recepcion.P_Notario_ID = Txt_Notario_ID.Text;
                Rs_Recepcion.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
                Rs_Recepcion.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
                //Rs_Recepcion.P_Clave_Tramite = Txt_Numero_Escritura.Text;
                Rs_Recepcion.P_Dt_Observaciones = Dt_Agregar;
                ID_Int_Recepcion = Rs_Recepcion.Alta_Recepcion_Documentos((DataTable)Session["Tabla_Tramites"]); //Da de alta los datos de la recepcion de documentos en la BD
            }
            else
            {
                Rs_Recepcion.P_No_Recepcion_Documento = Hdn_Recep_Docs.Value;
                Rs_Recepcion.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
                ID_Int_Recepcion = Rs_Recepcion.Alta_Recepcion_Documento_Modifica((DataTable)Session["Tabla_Tramites"]);
            }
            ID_Recepcion = String.Format("{0:0000000000}", ID_Int_Recepcion + 1);
            Guardar_Archivos();     // llamar al metodo que guarda los archivos en el servidor            
            ///Aceptacion directa
            ///            
            Validacion.P_No_Recepcion_Documento = ID_Recepcion;
            Validacion.P_Estatus_Movimiento = "ACEPTADO";
            Validacion.P_Observaciones = "SE ACEPTA DIRECTAMENTE";
            Grid_Tramites.Rows[0].Cells[10].Visible = true;
            Validacion.P_Cuenta_Predial_ID = HttpUtility.HtmlDecode(Grid_Tramites.Rows[0].Cells[10].Text).Trim();
            Validacion.P_Numero_Escritura = Grid_Tramites.Rows[0].Cells[2].Text;
            Validacion.P_Fecha_Escritura = Grid_Tramites.Rows[0].Cells[3].Text;
            Grid_Tramites.Rows[0].Cells[10].Visible = false;
            Validacion.Modificar_Recepcion_Movimiento_Directa();
            Ds_Contrarecibo = Validacion.Generar_Reporte_Contra_Recibo();
            Generar_Reporte(Ds_Contrarecibo);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alta de recepción de documentos", "alert('El Alta de la recepción de documentos fue exitosa');", true);
            ///
            Limpiar_Controles();
            Limpiar_Controles("Salir"); //Limpialos controles de la pantalla
            Habilitar_Controles("Inicial");
            Estado_Botones(Const_Estado_Inicial);
            Cargar_Bandeja(0, false);
            //ocultar paneles cuenta y recepcion de documentos
            Pnl_Contenedor_Datos_Recepcion_Documentos.Style.Value = "display:none;";    // este control se oculta del lado del cliente, si se oculta con codigo de servidor no funciona el control AsyncFileUpload
            Div_Notarios.Style.Value = "display:none;";
        }
        catch (Exception Ex)
        {
            throw new Exception("Alta_Recepcion_Documentos: " + Ex.Message);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Validar_Tramites_Recepcion
    /// 	DESCRIPCIÓN: Revisar que hay recepcion de documentos (en la tabla tramites) para dar de alta
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 09-may-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Validar_Tramites_Recepcion()
    {
        DataTable Tabla_Tramites = (DataTable)Session["Tabla_Tramites"];

        //if (Tabla_Tramites == null || Tabla_Tramites.Rows.Count <= 0)   //si la tabla es nula o no contiene informacion, agregar mensaje de error
        //Lbl_Mensaje_Error.Text = "Agregar información de la recepción de documentos.";

    }
    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Habilitar_Controles
    /// 	DESCRIPCIÓN: Habilita o Deshabilita los controles de la forma según se requiera 
    /// 	            para la siguiente operación
    /// 	PARÁMETROS:
    /// 	        1. Operacion: Indica la operación a realizar por parte del usuario
    /// 		             (inicial, nuevo, modificar)
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 28-abr-2011 
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
                    //Btn_Imprimir.Visible = false;
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Salir.AlternateText = "Inicio";
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Lbl_Comentarios.Text = "Observaciones";
                    Lbl_Observaciones.Text = "";
                    Txt_Bandera_Modifica_tramite_o_Recepcion.Text = "";
                    //Cmb_Busqueda_Calle_Cuenta.Visible = false;
                    //Cmb_Busqueda_Colonia_Cuenta.Visible = false;
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ToolTip = "Cancelar";
                    //Btn_Salir.AlternateText = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Lbl_Comentarios.Text = "Observaciones";
                    Lbl_Observaciones.Text = "";
                    Txt_Bandera_Modifica_tramite_o_Recepcion.Text = "";
                    break;

                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Lbl_Comentarios.Text = "*Observaciones";
                    Txt_Bandera_Modifica_tramite_o_Recepcion.Text = "";
                    break;

                case "Modificar_Recepcion":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Lbl_Comentarios.Text = "*Observaciones";
                    Txt_Bandera_Modifica_tramite_o_Recepcion.Text = "RECEPCION";
                    break;
            }

            //Txt_Notario_ID.Enabled = !Habilitado;
            //Txt_RFC_Notario.Enabled = !Habilitado;
            //Btn_Mostrar_Busqueda_Notario.Enabled = !Habilitado;
            //Txt_Cuenta_Predial.Enabled = Habilitado;
            //Btn_Buscar_Recepciones_Notario.Enabled = !Habilitado;

            Txt_Numero_Escritura.Enabled = Habilitado;
            Txt_Fecha_Escritura_Docs.Enabled = Habilitado;
            Btn_Fecha_Escritura.Enabled = Habilitado;
            Btn_Agregar_Recepcion.Enabled = Habilitado;
            Txt_Comentarios.Enabled = Habilitado;
            Grid_Documentos.Enabled = Habilitado;

            //Btn_Mostrar_Busqueda_Cuentas.Enabled = Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Generar_Tabla_Tramites
    /// DESCRIPCION: Genera la tabla de tramites, el esquema para guardar los datos de cada tramite
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 27-abr-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Generar_Tabla_Tramites()
    {
        DataTable Tabla_Tramites = new DataTable();
        DataColumn Columna0_Cuenta_Predial;
        DataColumn Columna1_No_Escritura;
        DataColumn Columna2_Fecha_Escritura;
        DataColumn Columna3_Comentarios;
        DataColumn Columna4_Nombre_Documentos;
        DataColumn Columna5_Nombres_Archivo;
        DataColumn Columna6_Tipos_Documento;
        DataColumn Columna7_Checksum;
        DataColumn Columna8_No_Movimiento;
        DataColumn Columna9_Cuenta_Predial_ID;

        try
        {
            // ---------- Inicializar columnas
            Columna0_Cuenta_Predial = new DataColumn();
            Columna0_Cuenta_Predial.DataType = System.Type.GetType("System.String");
            Columna0_Cuenta_Predial.ColumnName = "CUENTA_PREDIAL";
            Tabla_Tramites.Columns.Add(Columna0_Cuenta_Predial);
            Columna1_No_Escritura = new DataColumn();
            Columna1_No_Escritura.DataType = System.Type.GetType("System.String");
            Columna1_No_Escritura.ColumnName = "NO_ESCRITURA";
            Tabla_Tramites.Columns.Add(Columna1_No_Escritura);
            Columna2_Fecha_Escritura = new DataColumn();
            Columna2_Fecha_Escritura.DataType = System.Type.GetType("System.String");
            Columna2_Fecha_Escritura.ColumnName = "FECHA_ESCRITURA";
            Tabla_Tramites.Columns.Add(Columna2_Fecha_Escritura);
            Columna3_Comentarios = new DataColumn();
            Columna3_Comentarios.DataType = System.Type.GetType("System.String");
            Columna3_Comentarios.ColumnName = "COMENTARIOS";
            Tabla_Tramites.Columns.Add(Columna3_Comentarios);
            Columna4_Nombre_Documentos = new DataColumn();
            Columna4_Nombre_Documentos.DataType = System.Type.GetType("System.String");
            Columna4_Nombre_Documentos.ColumnName = "NOMBRES_DOCUMENTOS";
            Tabla_Tramites.Columns.Add(Columna4_Nombre_Documentos);
            Columna5_Nombres_Archivo = new DataColumn();
            Columna5_Nombres_Archivo.DataType = System.Type.GetType("System.String");
            Columna5_Nombres_Archivo.ColumnName = "NOMBRES_ARCHIVO";
            Tabla_Tramites.Columns.Add(Columna5_Nombres_Archivo);
            Columna6_Tipos_Documento = new DataColumn();
            Columna6_Tipos_Documento.DataType = System.Type.GetType("System.String");
            Columna6_Tipos_Documento.ColumnName = "TIPOS_DOCUMENTO";
            Tabla_Tramites.Columns.Add(Columna6_Tipos_Documento);
            Columna7_Checksum = new DataColumn();
            Columna7_Checksum.DataType = System.Type.GetType("System.String");
            Columna7_Checksum.ColumnName = "CHECKSUM";
            Tabla_Tramites.Columns.Add(Columna7_Checksum);
            Columna8_No_Movimiento = new DataColumn();
            Columna8_No_Movimiento.DataType = System.Type.GetType("System.String");
            Columna8_No_Movimiento.ColumnName = "NO_MOVIMIENTO";
            Tabla_Tramites.Columns.Add(Columna8_No_Movimiento);
            Columna9_Cuenta_Predial_ID = new DataColumn();
            Columna9_Cuenta_Predial_ID.DataType = System.Type.GetType("System.String");
            Columna9_Cuenta_Predial_ID.ColumnName = "CUENTA_PREDIAL_ID";
            Tabla_Tramites.Columns.Add(Columna9_Cuenta_Predial_ID);

            return Tabla_Tramites;
        }
        catch (Exception ex)
        {
            throw new Exception("Generar_Tabla_Tramites " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Quitar_Tramite_Click
    /// 	DESCRIPCIÓN: Manejo del evento Click en el boton Quitar tramite, se elimina el tramite 
    /// 	            seleccionado (solo tramites locales temporales, los que se leen de la base de 
    /// 	            datos no se borran)
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 06-may-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Quitar_Tramite_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Tabla_Tramites;
        ImageButton Btn_Quitar_Tramite = (ImageButton)sender;
        if (Session["Tabla_Tramites"] != null)    //si hay datos en la tabla de documentos
        {
            Tabla_Tramites = (DataTable)Session["Tabla_Tramites"];

            foreach (DataRow Fila_Tramite in Tabla_Tramites.Rows)  //recorrer las filas de la tabla tramites
            {
                if (Fila_Tramite["NO_MOVIMIENTO"].ToString() == Btn_Quitar_Tramite.CommandArgument) //si el numero de movimiento es el argumentodel boton, eliminar la fila
                {
                    Tabla_Tramites.Rows.Remove(Fila_Tramite);
                    break;
                }
            }

            Grid_Tramites.Columns[6].Visible = true;
            Grid_Tramites.Columns[7].Visible = true;
            Grid_Tramites.Columns[8].Visible = true;
            Grid_Tramites.Columns[9].Visible = true;
            Grid_Tramites.DataSource = Tabla_Tramites;
            Grid_Tramites.DataBind();
            Grid_Tramites.Columns[6].Visible = false;
            Grid_Tramites.Columns[7].Visible = false;
            Grid_Tramites.Columns[8].Visible = false;
            Grid_Tramites.Columns[9].Visible = false;

            Session["Tabla_Tramites"] = Tabla_Tramites;      //Guardar cambios en variable de sesion

            Recargar_Nombres_Archivo_Grid_Documentos();         // metodo que vuelve a poner los nombres de archivo en las etiquetas
        }
    }
    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Grid_Tramites_RowDataBound
    /// 	DESCRIPCIÓN: Manejo del evento RowDataBound en el grid tramites
    /// 	            a cada fila agregar como argumento (propiedad CommandArgument del boton) 
    /// 	            el numero de movimiento
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 06-may-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Tramites_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton Btn_Quitar_Tramite = (ImageButton)e.Row.FindControl("Btn_Quitar_Tramite");
            Btn_Quitar_Tramite.CommandArgument = e.Row.Cells[9].Text;
        }
    }
    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Grid_Tramites_SelectedIndexChanged
    /// 	DESCRIPCIÓN: Manejo del evento SelectedIndexChanged en el grid Tramites
    /// 	            Cargar los datos de un tramite en los campos correspondientes para edicion
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 06-may-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Tramites_SelectedIndexChanged(object sender, EventArgs e)
    {
        String[] Arr_Nombres_Documentos = Grid_Tramites.SelectedRow.Cells[5].Text.Split(',');
        String[] Arr_Nombres_Archivos = Grid_Tramites.SelectedRow.Cells[6].Text.Split(',');
        String[] Arr_Tipos_Documentos = Grid_Tramites.SelectedRow.Cells[7].Text.Split(',');
        String[] Arr_Checksum = Grid_Tramites.SelectedRow.Cells[8].Text.Split(',');
        DataTable Tabla_Documentos;
        CheckBox Chk_CheckBox;
        Label Lbl_Nombre_Archivo;

        Limpiar_Controles(string.Empty);

        // cargar datos en campos correspondientes
        Txt_Cuenta_Predial_1.Text = HttpUtility.HtmlDecode(Grid_Tramites.SelectedRow.Cells[1].Text);
        Hdn_Cuenta_Predial_ID.Value = HttpUtility.HtmlDecode(Grid_Tramites.SelectedRow.Cells[10].Text);
        Txt_Numero_Escritura.Text = HttpUtility.HtmlDecode(Grid_Tramites.SelectedRow.Cells[2].Text);
        Txt_Fecha_Escritura_Docs.Text = HttpUtility.HtmlDecode(Grid_Tramites.SelectedRow.Cells[3].Text);
        Txt_Comentarios.Text = HttpUtility.HtmlDecode(Grid_Tramites.SelectedRow.Cells[4].Text);
        for (int i = 0; i < Arr_Nombres_Documentos.Length - 1; i++) //recorrer los arreglos para asginar los valores en cada campo
        {
            Tabla_Documentos = Generar_Tabla_Documentos();
            foreach (GridViewRow Fila_Grid in Grid_Documentos.Rows) //recorrer el grid documentos hasta encontrar el ID del tipo de documento
            {
                if (Arr_Tipos_Documentos[i] == Fila_Grid.Cells[1].Text) //si el ID del documento en el arreglo, es igual al de la fila del grid
                {
                    //activar el checkbox para el tipo de documento
                    Chk_CheckBox = (CheckBox)Fila_Grid.FindControl("Chk_Documento_Recibido");
                    Chk_CheckBox.Checked = true;
                    Lbl_Nombre_Archivo = (Label)Fila_Grid.FindControl("Lbl_Nombre_Archivo");
                    Lbl_Nombre_Archivo.Text = HttpUtility.HtmlDecode(Path.GetFileName(Arr_Nombres_Archivos[i]));
                    // agregar datos del arreglo a la Tabla documentos solo si contiene archivo y checksum
                    if (Arr_Checksum[i].Length > 0 && Arr_Nombres_Archivos[i].Length > 0)
                    {
                        DataRow Nueva_Fila_Documento;
                        Nueva_Fila_Documento = Tabla_Documentos.NewRow();
                        Nueva_Fila_Documento["CLAVE_DOCUMENTO"] = Arr_Tipos_Documentos[i];
                        Nueva_Fila_Documento["NOMBRE_DOCUMENTO"] = Arr_Nombres_Documentos[i];
                        Nueva_Fila_Documento["NOMBRE_ARCHIVO"] = Path.GetFileName(Arr_Nombres_Archivos[i]);
                        Nueva_Fila_Documento["CHECKSUM"] = Arr_Checksum[i];
                        Tabla_Documentos.Rows.Add(Nueva_Fila_Documento);
                    }
                    break;
                }
            }
            Session["Tabla_Documentos"] = Tabla_Documentos;
        }
        // cambiar tooltip al boton agregar recepcion (tramite) y guardar numero de movimiento para 
        //Btn_Actualizar_Recepcion.Visible = true;
        Btn_Actualizar_Recepcion.CommandArgument = Grid_Tramites.SelectedIndex.ToString();
        //Btn_Agregar_Recepcion.Visible = false;
        Grid_Tramites.Enabled = false;              // desactivar grid tramites mientras se actualiza el tramite seleccionado
        Txt_Bandera_Modifica_tramite_o_Recepcion.Text = "TRAMITE";
    }
    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Validar_Campos_Tramite
    /// 	DESCRIPCIÓN: Revisar que los campos obligatorios hayan sido llenados y si no, generar el mensaje 
    /// 	            correspondiente.
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 28-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private String Validar_Campos_Tramite()
    {
        String Mensaje_Error = "";
        DateTime Fecha;
        bool Bnd_Seleccionados = false;

        if (Txt_Numero_Escritura.Text == "")  //Validar que haya un numero de escritura
        {
            Mensaje_Error += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir el número de escritura <br />";
        }
        else if (Txt_Numero_Escritura.Text.Length > 20)  //Validar campo numero escritura (longitud mayor a 20)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + El número de escritura no puede contener más de 20 caracteres<br />";
        }
        if (!DateTime.TryParse(Txt_Fecha_Escritura_Docs.Text, out Fecha))  //Validar que haya una fecha de escritura
        {
            Mensaje_Error += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir la fecha de la escritura <br />";
        }
        else
        {
            try
            {
                if (!(DateTime.Compare(DateTime.Now, Convert.ToDateTime(Txt_Fecha_Escritura_Docs.Text)) == 1))
                {
                    Mensaje_Error += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir una fecha de la escritura  menor o igual a la fecha actual <br />";
                }
            }
            catch (Exception excep)
            {

            }
        }
        //************** validar que hay por lo menos un docuemnto recibido
        foreach (GridViewRow Fila in Grid_Documentos.Rows)  //recorrer las filas del grid documentos en busca de un checkbox activado
        {
            CheckBox Chk_Orden = (CheckBox)Fila.FindControl("Chk_Documento_Recibido");
            if (Chk_Orden != null && Chk_Orden.Checked) //Si hay por lo menos un checkbox seleccionado abandonar el foreach y cambiar bandera seleccionados
            {
                Bnd_Seleccionados = true;
                break;
            }
        }
        if (Bnd_Seleccionados == false)         // si ningun checkbox esta seleccionado, mostrar mensaje y salir
        {
            Mensaje_Error += "&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar por lo menos un documento recibido.<br />";
        }
        //************** validar que hay por lo menos un docuemnto recibido **************//
        return Mensaje_Error;
    }

    private void Agregar_Recepcion()
    {
        try
        {
            DataTable Tabla_Tramites;
            DataRow Nueva_Fila;
            String Resultado_Validacion = "";
            Dictionary<String, Byte[]> Diccionario_Archivos = Obtener_Diccionario_Archivos();   //obtener diccionario checksum-archivo

            String Claves_Docs = "";
            String Nombres_Docs = "";
            String Nombres_Archivos = "";
            String Checksums = "";

            //Borrar errores
            Lbl_Error_Tramite.Text = "";
            Lbl_Error_Tramite.Visible = false;
            Img_Error_Tamite.Visible = false;

            if (Grid_Tramites.Rows.Count < 1)
            {
                Resultado_Validacion = Validar_Campos_Tramite();        //Metodo para validar campos obligatorios para agregar tramite

                if (Resultado_Validacion == "")     //Si esta vacio, no hubo errores al validar, agregar tramite al grid
                {
                    if (Session["Tabla_Tramites"] != null) //Buscar tabla en sesion
                    {
                        Tabla_Tramites = (DataTable)Session["Tabla_Tramites"];
                    }
                    else            // Si no hay sesion, generar nueva tabla
                    {
                        Tabla_Tramites = Generar_Tabla_Tramites();
                    }

                    Nueva_Fila = Tabla_Tramites.NewRow();        //nueva fila con los datos ingresados

                    if (!String.IsNullOrEmpty(Hdn_Cuenta_Predial_ID.Value)) // solo agregar la cuenta predial si hay un ID de cuenta predial
                    {
                        Nueva_Fila["CUENTA_PREDIAL"] = Txt_Cuenta_Predial_1.Text;
                        Nueva_Fila["CUENTA_PREDIAL_ID"] = Hdn_Cuenta_Predial_ID.Value;
                    }
                    Nueva_Fila["NO_ESCRITURA"] = Txt_Numero_Escritura.Text;
                    Nueva_Fila["FECHA_ESCRITURA"] = Txt_Fecha_Escritura_Docs.Text;
                    Nueva_Fila["COMENTARIOS"] = Txt_Comentarios.Text.ToUpper();
                    Nueva_Fila["NO_MOVIMIENTO"] = Tabla_Tramites.Rows.Count + 1; //asignar consecutivo al tramite

                    Obtener_Lista_Documentos(out Claves_Docs, out Nombres_Docs, out Nombres_Archivos, out Checksums); // obtener lista de documentos
                    Nueva_Fila["NOMBRES_DOCUMENTOS"] = Nombres_Docs;
                    Nueva_Fila["TIPOS_DOCUMENTO"] = Claves_Docs;
                    Nueva_Fila["NOMBRES_ARCHIVO"] = Nombres_Archivos;
                    Nueva_Fila["CHECKSUM"] = Checksums;

                    Tabla_Tramites.Rows.Add(Nueva_Fila);

                    Grid_Tramites.Columns[6].Visible = true;
                    Grid_Tramites.Columns[7].Visible = true;
                    Grid_Tramites.Columns[8].Visible = true;
                    Grid_Tramites.Columns[9].Visible = true;
                    Grid_Tramites.Columns[10].Visible = true;
                    Grid_Tramites.DataSource = Tabla_Tramites;
                    Grid_Tramites.DataBind();
                    Grid_Tramites.Columns[6].Visible = false;
                    Grid_Tramites.Columns[7].Visible = false;
                    Grid_Tramites.Columns[8].Visible = false;
                    Grid_Tramites.Columns[9].Visible = false;
                    Grid_Tramites.Columns[10].Visible = false;

                    Session["Tabla_Tramites"] = Tabla_Tramites;      //Guardar datos temporales de tramite en variable de sesion
                    Limpiar_Controles(String.Empty);                //Limpiar controles para agregar nuevo tramite
                    Session.Remove("Tabla_Documentos");         //eliminar la sesion Tabla_Documentos

                }
                else            // Hubo errores al validar campos, mostrar error
                {
                    Lbl_Error_Tramite.Text = "Es necesario:<br />" + Resultado_Validacion;
                    Lbl_Error_Tramite.Visible = true;
                    Img_Error_Tamite.Visible = true;
                }
            }
        }
        catch (Exception Ex)
        {

        }

    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: `_Click
    /// 	DESCRIPCIÓN: Manejo del evento click en el boton Agregar recepcion, 
    /// 	        se agregan los datos del tramite al grid tramites, buscar en variable de 
    /// 	        sesion Tabla_Documentos, si no existe, buscar los checkbox y agregar los que 
    /// 	        esten marcados
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 27-abr-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Agregar_Recepcion_Click(object sender, ImageClickEventArgs e)
    {

    }

    //*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Actualizar_Recepcion_Click
    /// 	DESCRIPCIÓN: Manejo del evento click en el boton Actualizar recepcion, 
    /// 	        se modifican los datos del tramite en el grid tramites, mediante el numero de movimiento
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 27-abr-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Actualizar_Recepcion_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Tabla_Tramites;
        String Resultado_Validacion = "";
        Dictionary<String, Byte[]> Diccionario_Archivos = Obtener_Diccionario_Archivos();   //obtener diccionario checksum-archivo
        int Indice_Tramite;
        String Claves_Docs = "";
        String Nombres_Docs = "";
        String Nombres_Archivos = "";
        String Checksums = "";

        //Borrar errores
        Lbl_Error_Tramite.Text = "";
        Lbl_Error_Tramite.Visible = false;
        Img_Error_Tamite.Visible = false;

        //Resultado_Validacion = Validar_Campos_Tramite();        //Metodo para validar campos obligatorios para agregar tramite

        if (Resultado_Validacion == "")     //Si esta vacio, no hubo errores al validar, agregar tramite al grid
        {
            if (Session["Tabla_Tramites"] != null) //Buscar tabla en variable de sesion
            {
                Tabla_Tramites = (DataTable)Session["Tabla_Tramites"];

                //si el boton actualizar tiene un argumento (indice de fila), cambiar el valor de los datos en la tabla tramites con ese indice
                if (!String.IsNullOrEmpty(Btn_Actualizar_Recepcion.CommandArgument))
                {
                    Indice_Tramite = int.Parse(Btn_Actualizar_Recepcion.CommandArgument);

                    Tabla_Tramites.DefaultView.AllowEdit = true;
                    Tabla_Tramites.Rows[Indice_Tramite].BeginEdit();        //iniciar edicion de fila
                    if (!String.IsNullOrEmpty(Hdn_Cuenta_Predial_ID.Value)) // solo agregar la cuenta predial si hay un ID de cuenta predial
                    {
                        Tabla_Tramites.Rows[Indice_Tramite]["CUENTA_PREDIAL"] = Txt_Cuenta_Predial_1.Text;
                        Tabla_Tramites.Rows[Indice_Tramite]["CUENTA_PREDIAL_ID"] = Hdn_Cuenta_Predial_ID.Value;
                    }
                    else        // si no hay un ID de cuenta predial, asignar cadena vacia a cuenta predial
                    {
                        Tabla_Tramites.Rows[Indice_Tramite]["CUENTA_PREDIAL"] = "";
                        Tabla_Tramites.Rows[Indice_Tramite]["CUENTA_PREDIAL_ID"] = "";
                    }
                    Tabla_Tramites.Rows[Indice_Tramite]["NO_ESCRITURA"] = Txt_Numero_Escritura.Text;
                    Tabla_Tramites.Rows[Indice_Tramite]["FECHA_ESCRITURA"] = Txt_Fecha_Escritura_Docs.Text;
                    Tabla_Tramites.Rows[Indice_Tramite]["COMENTARIOS"] = Txt_Comentarios.Text;

                    Obtener_Lista_Documentos(out Claves_Docs, out Nombres_Docs, out Nombres_Archivos, out Checksums);

                    Tabla_Tramites.Rows[Indice_Tramite]["NOMBRES_DOCUMENTOS"] = Nombres_Docs;
                    Tabla_Tramites.Rows[Indice_Tramite]["TIPOS_DOCUMENTO"] = Claves_Docs;
                    Tabla_Tramites.Rows[Indice_Tramite]["NOMBRES_ARCHIVO"] = Nombres_Archivos;
                    Tabla_Tramites.Rows[Indice_Tramite]["CHECKSUM"] = Checksums;
                    Tabla_Tramites.Rows[Indice_Tramite].EndEdit();          // terminar edicion de fila

                    Grid_Tramites.Columns[6].Visible = true;        // actualizar contenido del grid Tramites
                    Grid_Tramites.Columns[7].Visible = true;
                    Grid_Tramites.Columns[8].Visible = true;
                    Grid_Tramites.Columns[9].Visible = true;
                    Grid_Tramites.Columns[10].Visible = true;
                    Grid_Tramites.DataSource = Tabla_Tramites;
                    Grid_Tramites.DataBind();
                    Grid_Tramites.Columns[6].Visible = false;
                    Grid_Tramites.Columns[7].Visible = false;
                    Grid_Tramites.Columns[8].Visible = false;
                    Grid_Tramites.Columns[9].Visible = false;
                    Grid_Tramites.Columns[10].Visible = false;

                    Session["Tabla_Tramites"] = Tabla_Tramites;      //Guardar datos temporales de tramite en variable de sesion
                    Limpiar_Controles(String.Empty);                //Limpiar controles para agregar nuevo tramite
                    Session.Remove("Tabla_Documentos");             //eliminar la sesion Tabla_Documentos
                    // ocultar boton Actualizar recepcion y limpiar contenido del parametro CommandArgument
                    Btn_Actualizar_Recepcion.Visible = false;
                    Btn_Actualizar_Recepcion.CommandArgument = "";
                    Btn_Agregar_Recepcion.Visible = false;
                    Grid_Tramites.Enabled = true;              // desactivar grid tramites mientras se actualiza el tramite seleccionado
                }
            }
        }
        else            // Hubo errores al validar campos, mostrar error
        {
            Lbl_Error_Tramite.Text = "Es necesario:<br />" + Resultado_Validacion;
            Lbl_Error_Tramite.Visible = true;
            Img_Error_Tamite.Visible = true;
        }
    }
    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Grid_Documentos_SelectedIndexChanged
    /// 	DESCRIPCIÓN: Manejo del evento SelectedIndexChanged en el grid documentos
    /// 	            Eliminar los datos del archivo recibido de la fila seleccionada
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 05-may-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Documentos_SelectedIndexChanged(object sender, EventArgs e)
    {
        CheckBox Mi_Checkbox;
        Label Lbl_Nombre_Archivo;
        DataTable Tabla_Documentos;
        Dictionary<String, Byte[]> Diccionario_Archivos = Obtener_Diccionario_Archivos();   //obtener diccionario checksum-archivo

        // Buscar y desactivar el checkbox del documento recibido.
        Mi_Checkbox = (CheckBox)Grid_Documentos.SelectedRow.FindControl("Chk_Documento_Recibido");
        if (Mi_Checkbox != null)
            Mi_Checkbox.Checked = false;
        Lbl_Nombre_Archivo = (Label)Grid_Documentos.SelectedRow.FindControl("Lbl_Nombre_Archivo");
        if (Lbl_Nombre_Archivo != null)
            Lbl_Nombre_Archivo.Text = "";
        if (Session["Tabla_Documentos"] != null)    //si hay datos en la tabla de documentos
        {
            Tabla_Documentos = (DataTable)Session["Tabla_Documentos"];
            foreach (DataRow Fila in Tabla_Documentos.Rows)
            {
                if (Grid_Documentos.SelectedRow.Cells[1].Text == Fila["CLAVE_DOCUMENTO"].ToString())  //buscar por clave
                {
                    Tabla_Documentos.Rows.Remove(Fila); // eliminar Fila de la tabla
                    break;
                }
            }
            Session["Tabla_Documentos"] = Tabla_Documentos;     // guardar tabla
        }
        Recargar_Nombres_Archivo_Grid_Documentos();         // metodo que vuelve a poner los nombres de archivo en las etiquetas

    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Generar_Tabla_Documentos
    /// DESCRIPCION: Genera la tabla de Documentos, el esquema para guardar los tipos de document a recibir
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-may-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Generar_Tabla_Documentos()
    {
        DataTable Tabla_Nueva = new DataTable();
        DataColumn Columna0_Clave_Documento;
        DataColumn Columna1_Nombre_Documento;
        DataColumn Columna2_Nombre_Archivo;
        DataColumn Columna3_Ruta_Archivo;
        DataColumn Columna4_Archivo;
        DataColumn Columna5_Checksum;
        DataColumn Columna6_No_Movimiento;

        try
        {
            // ---------- Inicializar columnas
            Columna0_Clave_Documento = new DataColumn();
            Columna0_Clave_Documento.DataType = System.Type.GetType("System.String");
            Columna0_Clave_Documento.ColumnName = "CLAVE_DOCUMENTO";
            Tabla_Nueva.Columns.Add(Columna0_Clave_Documento);
            Columna1_Nombre_Documento = new DataColumn();
            Columna1_Nombre_Documento.DataType = System.Type.GetType("System.String");
            Columna1_Nombre_Documento.ColumnName = "NOMBRE_DOCUMENTO";
            Tabla_Nueva.Columns.Add(Columna1_Nombre_Documento);
            Columna2_Nombre_Archivo = new DataColumn();
            Columna2_Nombre_Archivo.DataType = System.Type.GetType("System.String");
            Columna2_Nombre_Archivo.ColumnName = "NOMBRE_ARCHIVO";
            Tabla_Nueva.Columns.Add(Columna2_Nombre_Archivo);
            Columna3_Ruta_Archivo = new DataColumn();
            Columna3_Ruta_Archivo.DataType = System.Type.GetType("System.String");
            Columna3_Ruta_Archivo.ColumnName = "RUTA_ARCHIVO";
            Tabla_Nueva.Columns.Add(Columna3_Ruta_Archivo);
            Columna4_Archivo = new DataColumn();
            Columna4_Archivo.DataType = System.Type.GetType("System.String");
            Columna4_Archivo.ColumnName = "ARCHIVO";
            Tabla_Nueva.Columns.Add(Columna4_Archivo);
            Columna5_Checksum = new DataColumn();
            Columna5_Checksum.DataType = System.Type.GetType("System.String");
            Columna5_Checksum.ColumnName = "CHECKSUM";
            Tabla_Nueva.Columns.Add(Columna5_Checksum);
            Columna6_No_Movimiento = new DataColumn();
            Columna6_No_Movimiento.DataType = System.Type.GetType("System.String");
            Columna6_No_Movimiento.ColumnName = "NO_MOVIMIENTO";
            Tabla_Nueva.Columns.Add(Columna6_No_Movimiento);

            return Tabla_Nueva;
        }
        catch (Exception ex)
        {
            throw new Exception("Generar_Tabla_Documentos " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Archivo_UploadComplete
    /// 	DESCRIPCIÓN: Se completo el envio de archivo asincrono, guardar archivo en variable de sesion 
    /// 	            (diccionario <checksum-archivo>) y marcar el checkbox del tipo de documentos recibido
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 25-abr-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Archivo_UploadComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {
        System.Web.UI.WebControls.GridViewRow Fila_Grid;
        CheckBox Mi_Checkbox;
        System.Web.UI.WebControls.Label Lbl_Nombre_Archivo;
        AsyncFileUpload Fup_Archivo = (AsyncFileUpload)sender;
        HashAlgorithm sha = HashAlgorithm.Create("SHA1");
        String Checksum_Archivo = BitConverter.ToString(sha.ComputeHash(Fup_Archivo.FileBytes));       //obtener checksum del archivo
        Dictionary<String, Byte[]> Diccionario_Archivos = Obtener_Diccionario_Archivos();   //obtener diccionario checksum-archivo
        DataTable Tabla_Documentos;
        bool Bnd_Documento_Existente = false;
        String Extension_Archivo = Path.GetExtension(Fup_Archivo.FileName).ToLower();
        // arreglo con las extensiones de archivo permitidas
        String[] Extensiones_Permitidas = { ".jpg", ".jpeg", ".png", ".gif", ".doc", ".docx", ".ppt", ".pptx" };

        //limpiar mensajes de error y campos de texto
        //Div_Contenedor_Msj_Error.Visible = false;        
        Lbl_Mensaje_Error.Text = "";

        // si la extension del archivo recibido no es valida, regresar
        if (Array.IndexOf(Extensiones_Permitidas, Extension_Archivo) < 0)
        {
            //Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = " No se permite subir archivos con extensión: " + Extension_Archivo;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Error_Extension_Archivo", "top.$get(\"" +
            Fup_Archivo.ClientID + "\").style.background-color = 'red!important';", true);
            return;
        }

        if (Fup_Archivo.FileBytes.Length > 2048000) // si la longitud del archivo recibido es mayor que 2MB, mostrar mensaje
        {
            //Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = " No se permite subir archivos con extensión: " + Extension_Archivo;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Error_Tamanio_Archivo", "top.$get(\"" +
            Fup_Archivo.ClientID + "\").style.background-color = 'red!important';", true);
            return;
        }

        //fila del grid en la que se subio archivo
        Fila_Grid = (System.Web.UI.WebControls.GridViewRow)FindControl(Fup_Archivo.Parent.Parent.UniqueID);

        if (Session["Tabla_Documentos"] != null)
        {
            Tabla_Documentos = (DataTable)Session["Tabla_Documentos"];
        }
        else
        {
            Tabla_Documentos = Generar_Tabla_Documentos();
        }

        // Buscar y activar checkbox del documento recibido. Agregar el nombre del archivo al grid
        Mi_Checkbox = (CheckBox)Fila_Grid.FindControl("Chk_Documento_Recibido");

        Mi_Checkbox.Checked = true;

        Lbl_Nombre_Archivo = (Label)Fila_Grid.FindControl("Lbl_Nombre_Archivo");
        //asignar nuevo nombre al archivo (nombre documento con guion bajo en lugar de espacios)
        Lbl_Nombre_Archivo.Text = Fila_Grid.Cells[2].Text.Replace(' ', '_') + Extension_Archivo;

        // Scripts del lado del cliente para seleccionar el checkbox
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "checkbox", "top.$get(\"" +
            Mi_Checkbox.ClientID + "\").checked = 'checked';", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Escribir_Lbl_Nombre_Archivo", "top.$get(\"" +
            Lbl_Nombre_Archivo.ClientID + "\").innerHTML = '" + Lbl_Nombre_Archivo.Text + "';", true);

        if (!Diccionario_Archivos.ContainsKey(Checksum_Archivo)) //si el checksum no esta en el diccionario, agregarlo y guardar en variable de sesion
        {
            Diccionario_Archivos.Add(Checksum_Archivo, Fup_Archivo.FileBytes);
            Session["Diccionario_Archivos"] = Diccionario_Archivos;
        }

        // recorrer la tabla documentos
        foreach (DataRow Fila_Tabla_Documento in Tabla_Documentos.Rows)
        {
            //si ya existe un elemento con el tipo de documento, actualizarlo con el nuevo archivo
            if (Fila_Tabla_Documento["CLAVE_DOCUMENTO"].ToString() == Fila_Grid.Cells[1].Text)
            {
                Fila_Tabla_Documento["NOMBRE_DOCUMENTO"] = Fila_Grid.Cells[2].Text;
                Fila_Tabla_Documento["NOMBRE_ARCHIVO"] = Lbl_Nombre_Archivo.Text;
                Fila_Tabla_Documento["CHECKSUM"] = Checksum_Archivo;
                Bnd_Documento_Existente = true;     // bandera a verdadero para indicar que se actualizo un registro existente en la tabla
                break;
            }
        }
        // si la bandera es falso, no se encontro registro del tipo de documento, asi que se crea uno nuevo
        if (Bnd_Documento_Existente == false)
        {
            DataRow Nueva_Fila = Tabla_Documentos.NewRow();
            Nueva_Fila["CLAVE_DOCUMENTO"] = Fila_Grid.Cells[1].Text;
            Nueva_Fila["NOMBRE_DOCUMENTO"] = Fila_Grid.Cells[2].Text;
            Nueva_Fila["NOMBRE_ARCHIVO"] = Lbl_Nombre_Archivo.Text;
            Nueva_Fila["CHECKSUM"] = Checksum_Archivo;

            Tabla_Documentos.Rows.Add(Nueva_Fila);
        }

        Session["Tabla_Documentos"] = Tabla_Documentos;

    }
    protected void Btn_Fecha_Escritura_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (CE_Txt_Fecha_Escritura.SelectedDate > DateTime.Now)
            {
                //Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Error en la Fecha de Escritura: ";
                Txt_Fecha_Escritura_Docs.Text = "";
            }
        }
        catch (Exception Ex)
        {
            Txt_Fecha_Escritura_Docs.Text = "";
        }
    }
    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Limpiar_Grid_Documentos
    /// 	DESCRIPCIÓN: Limpiar los controles en el grid Documentos (checkbox y etiqueta)
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 28-abr-2011 
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Limpiar_Grid_Documentos()
    {
        foreach (GridViewRow Fila_Grid in Grid_Documentos.Rows)  //recorrer las filas del grid documentos en busca de un checkbox activado
        {
            CheckBox Chk_Orden = (CheckBox)Fila_Grid.FindControl("Chk_Documento_Recibido");
            Label Lbl_Nombre_Archivo = (Label)Fila_Grid.FindControl("Lbl_Nombre_Archivo");  // limpiar el texto de la etiqueta
            if (Lbl_Nombre_Archivo != null && Lbl_Nombre_Archivo.Text != "")
            {
                Lbl_Nombre_Archivo.Text = "";
            }
            if (Chk_Orden != null)
            {
                Chk_Orden.Checked = false;
            }
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: modificar registro
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/27/2011 04:05:51 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Bandeja_Recepcion.SelectedIndex > (-1))
            {
                if (Btn_Modificar.AlternateText == "Modificar")
                {
                    Estado_Botones(Const_Estado_Modificar);
                }
                else if (Btn_Modificar.AlternateText == "Actualizar")
                {
                    Modificar_Validacion();
                }
            }
            else
            {
                Mensaje_Error("Favor de seleccionar el registro a modificar");
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: salir de formulario
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/27/2011 04:05:34 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Limpiar_Controles();
        if (Btn_Salir.AlternateText == "Inicio")
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");

        }
        else
        {
            Limpiar_Grid_Documentos();
            Estado_Botones(Const_Estado_Inicial);
            Grid_Bandeja_Recepcion.SelectedIndex = (-1);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Mostrar_Busqueda_Cuentas_Click
    ///DESCRIPCIÓN: Obtener datos de busqueda avanzada de cuentas
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:29:06 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Btn_Mostrar_Busqueda_Cuentas_Click(object sender, ImageClickEventArgs e)
    {
        String Cuenta_Predial_ID;
        String Cuenta_Predial;
        try
        {

            if (Session["CUENTA_PREDIAL_ID"].ToString() != "" && Session["CUENTA_PREDIAL_ID"] != null)
            {
                Cuenta_Predial_ID = Session["CUENTA_PREDIAL_ID"].ToString();
                Cuenta_Predial = Session["CUENTA_PREDIAL"].ToString();
                Txt_Cuenta_Predial_1.Text = Cuenta_Predial;
                //M_Cuenta_ID = Cuenta_Predial_ID;
                //Txt_Cuenta_Predial_TextChanged(null, EventArgs.Empty);

                Hdn_Cuenta_Predial_ID.Value = Cuenta_Predial_ID;
                Txt_Cuenta_Predial_TextChanged(null, EventArgs.Empty);
                //Txt_Nombre_Propietario.Text = Grid_Cuentas_Predial.SelectedRow.Cells[3].Text.ToUpper();
                //Txt_Ubicacion_Cuenta.Text = Grid_Cuentas_Predial.SelectedRow.Cells[4].Text.ToUpper();
            }
            Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
            Session.Remove("CUENTA_PREDIAL_ID");
            Session.Remove("CUENTA_PREDIAL");
        }
        catch (Exception Ex)
        {
            if (Ex.Message != "Object reference not set to an instance of an object.")
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Ex.Message.ToString();
            }
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Cuentas_Predial
    /// DESCRIPCION: Consulta las Cuentas predial que estan dadas de alta en la BD, 
    ///         las almacena en una variable de sesión para consultas futuras o paginación
    /// PARAMETROS: 
    ///         1. Tipo_Busqueda: Especifica si se va a buscar con los datos en los 
    ///                 controles del panel modal o con los campos en la pagina principal
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 29-mar-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Cuentas_Predial(String Tipo_Busqueda)
    {
        Cls_Ope_Pre_Recepcion_Documentos_Negocio Rs_Consulta_Cuentas_Predial = new Cls_Ope_Pre_Recepcion_Documentos_Negocio();
        DataTable Dt_Cuentas_Predial;

        try
        {
            switch (Tipo_Busqueda)
            {
                case "Campos_Modal":
                    //if (Cmb_Busqueda_Estatus_Cuenta.SelectedIndex > 0)     // Si hay un estatus selecionado, filtrar por estatus, si no traer sólo las cuentas VALIDAS
                    //    Rs_Consulta_Cuentas_Predial.P_Estatus_Cuenta_Predial = Cmb_Busqueda_Estatus_Cuenta.SelectedValue;
                    //else
                    //    Rs_Consulta_Cuentas_Predial.P_Estatus_Cuenta_Predial = "ACTIVA";

                    //if (Txt_Busqueda_No_Cuenta.Text.Trim() != "")         // Filtrar por cuenta predial
                    //    Rs_Consulta_Cuentas_Predial.P_Cuenta_Predial = Txt_Busqueda_No_Cuenta.Text.Trim();
                    //if (Txt_Busqueda_Propietatio_Cuenta.Text != "")     // Filtrar por nombre de propietario
                    //    Rs_Consulta_Cuentas_Predial.P_Nombre_Propietario = Txt_Busqueda_Propietatio_Cuenta.Text;
                    //else     // Si no se especificó propietario sólo mostrar los de tipo PROPIETARIO (sin copropietarios)
                    //    Rs_Consulta_Cuentas_Predial.P_Tipo_Propietario = "PROPIETARIO";
                    //if (Cmb_Busqueda_Calle_Cuenta.SelectedIndex > 0)                // Filtrar por calle 
                    //    Rs_Consulta_Cuentas_Predial.P_Calle_ID = Cmb_Busqueda_Calle_Cuenta.SelectedValue;
                    //Dt_Cuentas_Predial = Rs_Consulta_Cuentas_Predial.Consulta_Cuentas(); //Consulta las Cuentas con sus datos generales
                    //Session["Consulta_Cuentas_Predial"] = Dt_Cuentas_Predial;
                    //Llenar_Grid_Cuentas();
                    break;
                case "Por_Cuenta_Predial":      //buscar por numero de cuenta predial en el campo de texto de la pagina 
                    Rs_Consulta_Cuentas_Predial.P_Cuenta_Predial = Txt_Cuenta_Predial_1.Text.Trim().Replace(",", "");
                    Dt_Cuentas_Predial = Rs_Consulta_Cuentas_Predial.Consulta_Cuentas(); //Consulta las Cuentas con sus datos generales
                    Session["Consulta_Cuentas_Predial"] = Dt_Cuentas_Predial;
                    break;
            }

        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Notarios " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Txt_Cuenta_Predial_TextChanged
    /// 	DESCRIPCIÓN: Buscar cuenta predial cuando cambie el texto en la caja de texto Txt_Cuenta_Predial
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 10-may-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Txt_Cuenta_Predial_TextChanged(object sender, EventArgs e)
    {
        DataTable Dt_Cuentas_Predial;
        //limpiar mensajes de error y campos de texto
        //Div_Contenedor_Msj_Error.Visible = false;        
        Lbl_Mensaje_Error.Text = "";

        Hdn_Cuenta_Predial_ID.Value = "";
        Txt_Nombre_Propietario.Text = "";
        Txt_Ubicacion_Cuenta.Text = "";
        if (Txt_Cuenta_Predial_1.Text.Length > 0) // solo si 
        {
            Consulta_Cuentas_Predial("Por_Cuenta_Predial");

            Dt_Cuentas_Predial = (DataTable)Session["Consulta_Cuentas_Predial"];    //recuperar los resultados de la consulta y cargar nuevos datos
            if (Dt_Cuentas_Predial.Rows.Count > 0)
            {
                Hdn_Cuenta_Predial_ID.Value = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                Txt_Cuenta_Predial_1.Text = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString();
                Txt_Nombre_Propietario.Text = Dt_Cuentas_Predial.Rows[0]["NOMBRE_PROPIETARIO"].ToString().ToUpper();
                Txt_Ubicacion_Cuenta.Text = Dt_Cuentas_Predial.Rows[0]["DOMICILIO"].ToString().ToUpper();
                Session["Estatus_Cuenta"] = Dt_Cuentas_Predial.Rows[0]["ESTATUS"].ToString();
                Session["Tipo_Suspencion"] = Dt_Cuentas_Predial.Rows[0]["TIPO_SUSPENCION"].ToString();
                Txt_Numero_Escritura.Focus();
            }
            else
            {
                Txt_Cuenta_Predial_1.Focus();
                //Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = " No se encontró el número de cuenta proporcionado.";
            }
            //Mpe_Busqueda_Cuentas.Hide();
            //Pnl_Contenedor_Busqueda_Cuentas.Style.Value = "display:none;";
            Recargar_Nombres_Archivo_Grid_Documentos();             // llamar al metodo para recuperar nombres de archivo si se perdieron
            Validar_Acceso();
        }
        Cargar_Ventana_Emergente_Resumen_Predio();
    }
    #endregion
}

