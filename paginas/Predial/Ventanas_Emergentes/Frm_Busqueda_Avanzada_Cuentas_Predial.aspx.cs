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
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Colonias.Negocio;
using Presidencia.Catalogo_Calles.Negocio;
using Presidencia.Constantes;

public partial class paginas_Predial_Ventanas_Emergentes_Frm_Busqueda_Avanzada_Cuentas_Predial : System.Web.UI.Page
{
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Page_Load
    ///DESCRIPCIÓN          : Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 02/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        String Ventana_Modal = "";
        Div_Contenedor_Msj_Error.Visible = false;
        if (!IsPostBack)
        {
            Session["BUSQUEDA_CUENTAS_PREDIAL"] = false;
        }
        if (Session["ESTATUS_CUENTAS"] != null)
        {
            Hdn_Estatus_Cuentas.Value = Session["ESTATUS_CUENTAS"].ToString();
            //Session.Remove("ESTATUS_CUENTAS");
            //Session["ESTATUS_CUENTAS"] = null;
        }
        else
        {
            Hdn_Estatus_Cuentas.Value = "VIGENTE";
        }
        if (Session["TIPO_CONTRIBUYENTE"] != null)
        {
            Hdn_Tipo_Contribuyente.Value = Session["TIPO_CONTRIBUYENTE"].ToString();
        }
        Frm_Busqueda_Avanzada_Cuentas_Predial.Page.Title = "Búsqueda Avanzada de Cuentas Predial";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Validación de Órdenes de Variación", "Window_Resize();", true);

        Ventana_Modal = "Abrir_Ventana_Modal('Frm_Busqueda_Avanzada_Colonias_Calles.aspx', 'center:yes;resizable:yes;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
        Btn_Busqueda_Avanzada_Colonias.Attributes.Add("onclick", Ventana_Modal);

        Ventana_Modal = "Abrir_Ventana_Modal('Frm_Busqueda_Avanzada_Colonias_Calles.aspx', 'center:yes;resizable:yes;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
        Btn_Busqueda_Avanzada_Calles.Attributes.Add("onclick", Ventana_Modal);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Regresar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 02/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Regresar_Click(object sender, ImageClickEventArgs e)
    {
        Session["BUSQUEDA_CUENTAS_PREDIAL"] = false;
        Session.Remove("CUENTA_PREDIAL_ID");
        Session.Remove("CUENTA_PREDIAL");
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Aceptar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Aceptar
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 02/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Aceptar_Click(object sender, ImageClickEventArgs e)
    {
        Session["BUSQUEDA_CUENTAS_PREDIAL"] = true;
        //Cierra la ventana
        //string Pagina = "<script language='JavaScript'>";
        //Pagina += "window.close();";
        //Pagina += "</script>";
        //ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", "window.close();", true);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Limpiar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Limpiar
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
    {
        Txt_Busqueda_Cuenta_Predial.Text = "";
        Txt_Busqueda_Propietatio.Text = "";
        Txt_Busqueda_Colonia.Text = "";
        Txt_Busqueda_Calle.Text = "";
    }

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN : Cargar_Combo_Colonias
    /////DESCRIPCIÓN          : Carga el combo con los datos del catálogo
    /////PARAMETROS           : 
    /////CREO                 : Antonio Salvador Benavides Guardado
    /////FECHA_CREO           : 12/Julio/2011
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //private void Cargar_Combo_Colonias(String Filtro)
    //{
    //    try
    //    {
    //        String Calles_IDs;
    //        Cls_Cat_Pre_Colonias_Negocio Colonias = new Cls_Cat_Pre_Colonias_Negocio();
    //        if (Filtro == "Cmb_Busqueda_Calles" && Cmb_Busqueda_Calles.SelectedIndex > 0)
    //        {
    //            Colonias.P_Colonia_ID = "IN (SELECT " + Cat_Pre_Calles.Campo_Colonia_ID + " FROM " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " WHERE " + Cat_Pre_Calles.Campo_Calle_ID + " = '" + Cmb_Busqueda_Calles.SelectedValue + "')";
    //        }
    //        else
    //        {
    //            if (Filtro == "Txt_Busqueda_Colonia_Aproximacion")
    //            {
    //                if (Txt_Busqueda_Colonia_Aproximacion.Text.Trim() != "")
    //                {
    //                    Colonias.P_Colonia_ID = "LIKE '%" + Txt_Busqueda_Colonia_Aproximacion.Text.Trim() + "%'";
    //                    Colonias.P_Nombre = "LIKE UPPER('%" + Txt_Busqueda_Colonia_Aproximacion.Text + "%')";
    //                }
    //            }
    //            else
    //            {
    //                if (Filtro == "Txt_Busqueda_Calle_Aproximacion")
    //                {
    //                    Calles_IDs = Obtener_Combo_IDs(Cmb_Busqueda_Calles);
    //                    if (Calles_IDs.Trim() != "")
    //                    {
    //                        Colonias.P_Colonia_ID = "IN (SELECT " + Cat_Pre_Calles.Campo_Colonia_ID + " FROM " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " WHERE " + Cat_Pre_Calles.Campo_Calle_ID + " IN (" + Calles_IDs + "))";
    //                    }
    //                }
    //            }
    //        }
    //        DataTable Dt_Colonias = Colonias.Consultar_Nombre_Id_Colonias();
    //        Cmb_Busqueda_Colonias.DataSource = Dt_Colonias;

    //        DataRow Dr_Colonias;
    //        Dr_Colonias = Dt_Colonias.NewRow();
    //        Dr_Colonias["COLONIA_ID"] = HttpUtility.HtmlDecode("00000");
    //        Dr_Colonias["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
    //        Dt_Colonias.Rows.InsertAt(Dr_Colonias, 0);

    //        Cmb_Busqueda_Colonias.DataValueField = "COLONIA_ID";
    //        Cmb_Busqueda_Colonias.DataTextField = "NOMBRE";
    //        Cmb_Busqueda_Colonias.DataBind();
    //        if (Filtro == "Txt_Busqueda_Colonia_Aproximacion")
    //        {
    //            Cargar_Combo_Calles(Filtro);
    //        }
    //        if (Filtro == "Cmb_Busqueda_Calles" && Cmb_Busqueda_Calles.SelectedIndex > 0 && Cmb_Busqueda_Colonias.Items.Count > 1)
    //        {
    //            Cmb_Busqueda_Colonias.SelectedIndex = 1;
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //        Lbl_Mensaje_Error.Text = "";
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }
    //}

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN : Cargar_Combo_Calles
    /////DESCRIPCIÓN          : Carga el combo con los datos del catálogo
    /////PARAMETROS           : 
    /////CREO                 : Antonio Salvador Benavides Guardado
    /////FECHA_CREO           : 12/Julio/2011
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //private void Cargar_Combo_Calles(String Filtro)
    //{
    //    try
    //    {
    //        String Colonias_IDs;
    //        Cls_Cat_Pre_Calles_Negocio Calles = new Cls_Cat_Pre_Calles_Negocio();
    //        Calles.P_Mostrar_Nombre_Calle_Nombre_Colonia = true;
    //        if (Filtro == "Cmb_Busqueda_Colonias" && Cmb_Busqueda_Colonias.SelectedIndex > 0)
    //        {
    //            Calles.P_Colonia_ID = Cmb_Busqueda_Colonias.SelectedValue;
    //        }
    //        else
    //        {
    //            if (Filtro == "Txt_Busqueda_Calle_Aproximacion")
    //            {
    //                if (Txt_Busqueda_Calle_Aproximacion.Text.Trim() != "")
    //                {
    //                    Calles.P_Calle_ID = "LIKE '%" + Txt_Busqueda_Calle_Aproximacion.Text.Trim() + "%'";
    //                    Calles.P_Nombre = "LIKE UPPER('%" + Txt_Busqueda_Calle_Aproximacion.Text + "%')";
    //                }
    //            }
    //            else
    //            {
    //                if (Filtro == "Txt_Busqueda_Colonia_Aproximacion")
    //                {
    //                    Colonias_IDs = Obtener_Combo_IDs(Cmb_Busqueda_Colonias);
    //                    if (Colonias_IDs.Trim() != "")
    //                    {
    //                        Calles.P_Colonia_ID = "IN (" + Colonias_IDs + ")";
    //                    }
    //                }
    //            }
    //        }

    //        if (Cmb_Busqueda_Colonias.SelectedIndex > 0)
    //        {
    //        }
    //        DataTable Dt_Calles = Calles.Consultar_Nombre_Id_Calles();
    //        Cmb_Busqueda_Calles.DataSource = Dt_Calles;

    //        DataRow Dr_Colonias;
    //        Dr_Colonias = Dt_Calles.NewRow();
    //        Dr_Colonias["CALLE_ID"] = HttpUtility.HtmlDecode("00000");
    //        Dr_Colonias["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
    //        Dt_Calles.Rows.InsertAt(Dr_Colonias, 0);

    //        Cmb_Busqueda_Calles.DataValueField = "CALLE_ID";
    //        Cmb_Busqueda_Calles.DataTextField = "NOMBRE";
    //        Cmb_Busqueda_Calles.DataBind();
    //        if (Filtro == "Txt_Busqueda_Calle_Aproximacion")
    //        {
    //            Cargar_Combo_Colonias(Filtro);
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //        Lbl_Mensaje_Error.Text = "";
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }
    //}

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN      : Obtener_Combo_IDs
    /// 	DESCRIPCIÓN         : Obtiene una cadena con la lista separada por caraqcteres coma
    /// 	PARÁMETROS:
    /// 	CREO                : Antonio Salvador Benavides Guardado
    /// 	FECHA_CREO          : 11/Agosto/2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private String Obtener_Combo_IDs(DropDownList Combo)
    {
        String Combo_IDs = "";
        foreach (ListItem Item in Combo.Items)
        {
            Combo_IDs += "'" + Item.Value + "', ";
        }
        if (Combo_IDs.EndsWith("', "))
        {
            Combo_IDs = Combo_IDs.Substring(0, Combo_IDs.Length - 2);
        }
        return Combo_IDs;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Btn_Buscar_Click
    /// DESCRIPCION             : Ejecuta la búsqueda de las cuentas predial
    /// CREO                    : Antonio Salvador Benavides Guardado
    /// FECHA_CREO              : 11/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Cargar_Cuentas_Predial(0);
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Cargar_Cuentas_Predial
    /// DESCRIPCION             : Carga en el grid la búsqueda de las cuentas predial
    /// CREO                    : Antonio Salvador Benavides Guardado
    /// FECHA_CREO              : 11/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Cargar_Cuentas_Predial(Int32 No_Pagina)
    {
        try
        {
            if (Txt_Busqueda_Cuenta_Predial.Text.Trim() != ""
                || Txt_Busqueda_Propietatio.Text.Trim() != ""
                || Hdn_Colonia_ID.Value.Trim() != ""
                || Hdn_Calle_ID.Value.Trim() != "")
            {
                DataTable Dt_Cuentas_Predial;
                Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();

                //Prepara las validaciones de Tipo de Contribuyente para aplicar filtros en la capa de Datos
                if (Hdn_Tipo_Contribuyente.Value.Trim() != "")
                {
                    if (Hdn_Tipo_Contribuyente.Value.Trim() != "SIN TIPO")
                    {
                        String Filtro = Hdn_Tipo_Contribuyente.Value.Trim().ToUpper().Replace("IN", "").Replace("(", "").Replace(")", "").Replace("'", "").Trim();
                        String[] Tipos_Filtro = Filtro.Trim().Split((",").ToCharArray());
                        Cuentas_Predial.P_Incluir_Propietarios = false;
                        Cuentas_Predial.P_Incluir_Copropietarios = false;
                        foreach (String Tipo_Filtro in Tipos_Filtro)
                        {
                            if (Tipo_Filtro == "PROPIETARIO" || Tipo_Filtro.Trim().ToUpper() == "POSEEDOR"
                                && !Cuentas_Predial.P_Incluir_Propietarios)
                            {
                                Cuentas_Predial.P_Incluir_Propietarios = true;
                            }
                            if (Tipo_Filtro.Trim().ToUpper() == "COPROPIETARIO"
                                && !Cuentas_Predial.P_Incluir_Copropietarios)
                            {
                                Cuentas_Predial.P_Incluir_Copropietarios = true;
                            }
                        }
                    }
                }

                //Consulta la Cuenta Predial
                Cuentas_Predial.P_Incluir_Campos_Foraneos = true;
                Cuentas_Predial.P_Campos_Dinamicos = Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", ";
                Cuentas_Predial.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ";
                Cuentas_Predial.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estatus + ", ";
                Cuentas_Predial.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + ", ";
                Cuentas_Predial.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + ", ";
                Cuentas_Predial.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + ", ";
                Cuentas_Predial.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + ", ";
                Cuentas_Predial.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior;

                //Cuentas_Predial.P_Filtros_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Estatus + " = 'VIGENTE'";
                Cuentas_Predial.P_Filtros_Dinamicos = "";
                if (Txt_Busqueda_Cuenta_Predial.Text.Trim() != "")
                {
                    if (Cuentas_Predial.P_Filtros_Dinamicos.Trim() != "")
                    {
                        Cuentas_Predial.P_Filtros_Dinamicos += " AND ";
                    }
                    Cuentas_Predial.P_Filtros_Dinamicos += "UPPER(" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ") LIKE UPPER('%" + Txt_Busqueda_Cuenta_Predial.Text.Trim() + "%')";
                }
                if (Txt_Busqueda_Propietatio.Text.Trim() != "")
                {
                    if (Cuentas_Predial.P_Incluir_Propietarios || Cuentas_Predial.P_Incluir_Copropietarios)
                    {
                        if (Cuentas_Predial.P_Filtros_Dinamicos.Trim() != "")
                        {
                            Cuentas_Predial.P_Filtros_Dinamicos += " AND ";
                        }
                        Cuentas_Predial.P_Filtros_Dinamicos += "(";
                        if (Cuentas_Predial.P_Incluir_Propietarios)
                        {
                            Cuentas_Predial.P_Filtros_Dinamicos += "(PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Nombre + " LIKE '%" + Txt_Busqueda_Propietatio.Text.Trim().ToUpper() + "%' OR PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " LIKE '%" + Txt_Busqueda_Propietatio.Text.Trim().ToUpper() + "%' OR PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " LIKE '%" + Txt_Busqueda_Propietatio.Text.Trim().ToUpper() + "%' OR PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Nombre + " || ' ' || PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " LIKE '%" + Txt_Busqueda_Propietatio.Text.Trim().ToUpper() + "%' OR PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Nombre + " LIKE '%" + Txt_Busqueda_Propietatio.Text.Trim().ToUpper() + "%')";
                        }
                        if (Cuentas_Predial.P_Incluir_Copropietarios)
                        {
                            if (Cuentas_Predial.P_Incluir_Propietarios)
                            {
                                Cuentas_Predial.P_Filtros_Dinamicos += " OR ";
                            }
                            Cuentas_Predial.P_Filtros_Dinamicos += " (COPROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Nombre + " LIKE '%" + Txt_Busqueda_Propietatio.Text.Trim().ToUpper() + "%' OR COPROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " LIKE '%" + Txt_Busqueda_Propietatio.Text.Trim().ToUpper() + "%' OR COPROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " LIKE '%" + Txt_Busqueda_Propietatio.Text.Trim().ToUpper() + "%' OR COPROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Nombre + " || ' ' || COPROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || COPROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " LIKE '%" + Txt_Busqueda_Propietatio.Text.Trim().ToUpper() + "%' OR COPROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || COPROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || COPROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Nombre + " LIKE '%" + Txt_Busqueda_Propietatio.Text.Trim().ToUpper() + "%')";
                        }
                        Cuentas_Predial.P_Filtros_Dinamicos += ")";
                    }
                }
                if (Hdn_Colonia_ID.Value.Trim() != "" && Hdn_Calle_ID.Value.Trim() != "")
                {
                    if (Cuentas_Predial.P_Filtros_Dinamicos.Trim() != "")
                    {
                        Cuentas_Predial.P_Filtros_Dinamicos += " AND ";
                    }
                    //Cuentas_Predial.P_Filtros_Dinamicos += "(";
                    Cuentas_Predial.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + " = '" + Hdn_Colonia_ID.Value.Trim() + "'";
                    Cuentas_Predial.P_Filtros_Dinamicos += " AND ";
                    Cuentas_Predial.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + " = '" + Hdn_Calle_ID.Value.Trim() + "'";
                    //Cuentas_Predial.P_Filtros_Dinamicos += ")";
                }
                else
                {
                    if (Hdn_Colonia_ID.Value.Trim() != "")
                    {
                        if (Cuentas_Predial.P_Filtros_Dinamicos.Trim() != "")
                        {
                            Cuentas_Predial.P_Filtros_Dinamicos += " AND ";
                        }
                        Cuentas_Predial.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + " = '" + Hdn_Colonia_ID.Value.Trim() + "'";
                    }
                    if (Hdn_Calle_ID.Value.Trim() != "")
                    {
                        if (Cuentas_Predial.P_Filtros_Dinamicos.Trim() != "")
                        {
                            Cuentas_Predial.P_Filtros_Dinamicos += " AND ";
                        }
                        Cuentas_Predial.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + " = '" + Hdn_Calle_ID.Value.Trim() + "'";
                    }
                }
                if (Hdn_Estatus_Cuentas.Value != null && Hdn_Estatus_Cuentas.Value != "")
                {
                    if (Cuentas_Predial.P_Filtros_Dinamicos.Trim() != "")
                    {
                        Cuentas_Predial.P_Filtros_Dinamicos += " AND ";
                    }
                    Cuentas_Predial.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estatus + Validar_Operador_Comparacion(Hdn_Estatus_Cuentas.Value);
                }
                if (Hdn_Tipo_Contribuyente.Value.Trim() != "")
                {
                    if (Hdn_Tipo_Contribuyente.Value.Trim() != "SIN TIPO")
                    {
                        Cuentas_Predial.P_Tipo_Propietario = Hdn_Tipo_Contribuyente.Value.Trim();
                        //if (Cuentas_Predial.P_Filtros_Dinamicos.Trim() != "")
                        //{
                        //    Cuentas_Predial.P_Filtros_Dinamicos += " AND ";
                        //}
                        //Cuentas_Predial.P_Filtros_Dinamicos += Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + Validar_Operador_Comparacion(Hdn_Tipo_Contribuyente.Value.Trim());
                    }
                }
                else
                {
                    Cuentas_Predial.P_Tipo_Propietario = " IN ('PROPIETARIO', 'POSEEDOR')";
                    //Cuentas_Predial.P_Filtros_Dinamicos += Cat_Pre_Propietarios.Campo_Tipo + " IN ('PROPIETARIO', 'POSEEDOR')";
                }
                Cuentas_Predial.P_Ordenar_Dinamico = "TRIM(NOMBRE_PROPIETARIO), " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial;
                Dt_Cuentas_Predial = Cuentas_Predial.Consultar_Cuenta();
                Grid_Cuentas_Predial.DataSource = Dt_Cuentas_Predial;
                Grid_Cuentas_Predial.PageIndex = No_Pagina;
                Grid_Cuentas_Predial.DataBind();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validación de Órdenes de Variación", "Window_Resize();", true);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN      : Grid_Cuentas_Predial_PageIndexChanging
    /// 	DESCRIPCIÓN         : Maneja el Evento de Cambio de Página del Grid de 
    /// 	PARÁMETROS          :
    /// 	CREO                : Antonio Salvador Benavides Guardado
    /// 	FECHA_CREO          : 11/Julio/2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Cuentas_Predial_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Cuentas_Predial.SelectedIndex = (-1);
            Cargar_Cuentas_Predial(e.NewPageIndex);
            //Btn_Limpiar_Click(sender, null);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN      : Grid_Cuentas_Predial_SelectedIndexChanged
    /// 	DESCRIPCIÓN         : Maneja el Evento de Cambio de Selección del Grid 
    /// 	PARÁMETROS:
    /// 	CREO                : Antonio Salvador Benavides Guardado
    /// 	FECHA_CREO          : 11/Julio/2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Cuentas_Predial_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["CUENTA_PREDIAL_ID"] = Grid_Cuentas_Predial.DataKeys[Grid_Cuentas_Predial.SelectedIndex].Values[0].ToString();
            Session["CUENTA_PREDIAL"] = Grid_Cuentas_Predial.Rows[Grid_Cuentas_Predial.SelectedIndex].Cells[2].Text;

            Btn_Aceptar_Click(sender, null);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    /////*******************************************************************************************************
    ///// 	NOMBRE_FUNCIÓN      : Btn_Cerrar_Busqueda_Colonias_Click
    ///// 	DESCRIPCIÓN         : Ocultar el modal popup Busqueda de 
    ///// 	PARÁMETROS:
    ///// 	CREO                : Antonio Salvador Benavides Guardado
    ///// 	FECHA_CREO          : 15/Julio/2011
    ///// 	MODIFICÓ: 
    ///// 	FECHA_MODIFICÓ: 
    ///// 	CAUSA_MODIFICACIÓN: 
    /////*******************************************************************************************************
    //protected void Btn_Cerrar_Busqueda_Colonias_Click(object sender, ImageClickEventArgs e)
    //{
    //    Mpe_Busqueda_Colonias.Hide();
    //}

    /////*******************************************************************************************************
    ///// 	NOMBRE_FUNCIÓN      : Btn_Limpiar_Busqueda_Colonias_Click
    ///// 	DESCRIPCIÓN         : Limpia los controles de la búsqeuda avanzada
    ///// 	PARÁMETROS:
    ///// 	CREO                : Antonio Salvador Benavides Guardado
    ///// 	FECHA_CREO          : 15/Julio/2011
    ///// 	MODIFICÓ: 
    ///// 	FECHA_MODIFICÓ: 
    ///// 	CAUSA_MODIFICACIÓN: 
    /////*******************************************************************************************************
    //protected void Btn_Limpiar_Busqueda_Colonias_Click(object sender, ImageClickEventArgs e)
    //{
    //    Txt_Busqueda_Avanzada_Colonia_ID.Text = "";
    //    Txt_Busqueda_Avanzada_Nombre_Colonia.Text = "";
    //}

    /////*******************************************************************************************************
    ///// 	NOMBRE_FUNCIÓN      : Btn_Cerrar_Busqueda_Calles_Click
    ///// 	DESCRIPCIÓN         : Ocultar el modal popup Busqueda de 
    ///// 	PARÁMETROS:
    ///// 	CREO                : Antonio Salvador Benavides Guardado
    ///// 	FECHA_CREO          : 15/Julio/2011
    ///// 	MODIFICÓ: 
    ///// 	FECHA_MODIFICÓ: 
    ///// 	CAUSA_MODIFICACIÓN: 
    /////*******************************************************************************************************
    //protected void Btn_Cerrar_Busqueda_Calles_Click(object sender, ImageClickEventArgs e)
    //{
    //    Mpe_Busqueda_Calles.Hide();
    //}

    /////*******************************************************************************************************
    ///// 	NOMBRE_FUNCIÓN      : Btn_Limpiar_Busqueda_Calles_Click
    ///// 	DESCRIPCIÓN         : Limpia los controles de la búsqeuda avanzada
    ///// 	PARÁMETROS:
    ///// 	CREO                : Antonio Salvador Benavides Guardado
    ///// 	FECHA_CREO          : 15/Julio/2011
    ///// 	MODIFICÓ: 
    ///// 	FECHA_MODIFICÓ: 
    ///// 	CAUSA_MODIFICACIÓN: 
    /////*******************************************************************************************************
    //protected void Btn_Limpiar_Busqueda_Calles_Click(object sender, ImageClickEventArgs e)
    //{
    //    Txt_Busqueda_Avanzada_Calle_ID.Text = "";
    //    Txt_Busqueda_Avanzada_Nombre_Calle.Text = "";
    //}

    /////*******************************************************************************
    ///// NOMBRE DE LA FUNCION    : Btn_Busqueda_Colonias_Click
    ///// DESCRIPCION             : Carga en el grid la búsqueda con las Colonias
    ///// CREO                    : Antonio Salvador Benavides Guardado
    ///// FECHA_CREO              : 28/Agosto/2011
    ///// MODIFICO          :
    ///// FECHA_MODIFICO    :
    ///// CAUSA_MODIFICACION:
    /////*******************************************************************************
    //protected void Btn_Busqueda_Colonias_Click(object sender, EventArgs e)
    //{
    //    Buscar_Colonias(0);
    //}

    //private void Buscar_Colonias(int Indice_Pagina)
    //{
    //    Cls_Ate_Colonias_Negocio Colonias = new Cls_Ate_Colonias_Negocio();
    //    DataTable Dt_Colonias;

    //    Colonias.P_Campos_Dinamicos = Cat_Ate_Colonias.Campo_Colonia_ID + ", " + Cat_Ate_Colonias.Campo_Nombre;
    //    Colonias.P_Filtros_Dinamicos = "";
    //    if (Txt_Busqueda_Avanzada_Colonia_ID.Text.Trim() != "")
    //    {
    //        Colonias.P_Filtros_Dinamicos += Cat_Ate_Colonias.Campo_Colonia_ID + " LIKE '%" + Txt_Busqueda_Avanzada_Colonia_ID.Text.Trim() + "%' AND ";
    //    }
    //    if (Txt_Busqueda_Avanzada_Nombre_Colonia.Text.ToUpper().Trim() != "")
    //    {
    //        Colonias.P_Filtros_Dinamicos += Cat_Ate_Colonias.Campo_Nombre + " LIKE '%" + Txt_Busqueda_Avanzada_Nombre_Colonia.Text.ToUpper().Trim() + "%' AND ";
    //    }
    //    if (Colonias.P_Filtros_Dinamicos.EndsWith(" AND"))
    //    {
    //        Colonias.P_Filtros_Dinamicos = Colonias.P_Filtros_Dinamicos.Substring(0, Colonias.P_Filtros_Dinamicos.Length - 4);
    //    }
    //    if (Colonias.P_Filtros_Dinamicos.EndsWith(" OR"))
    //    {
    //        Colonias.P_Filtros_Dinamicos = Colonias.P_Filtros_Dinamicos.Substring(0, Colonias.P_Filtros_Dinamicos.Length - 3);
    //    }
    //    if (Colonias.P_Filtros_Dinamicos.EndsWith(" WHERE"))
    //    {
    //        Colonias.P_Filtros_Dinamicos = Colonias.P_Filtros_Dinamicos.Substring(0, Colonias.P_Filtros_Dinamicos.Length - 6);
    //    }
    //    Dt_Colonias = Colonias.Consultar_Colonias();
    //    Grid_Colonias.DataSource = Dt_Colonias;
    //    Grid_Colonias.PageIndex = Indice_Pagina;
    //    Grid_Colonias.DataBind();
    //    Mpe_Busqueda_Colonias.Show();
    //}

    /////*******************************************************************************
    ///// NOMBRE DE LA FUNCION    : Btn_Busqueda_Calles_Click
    ///// DESCRIPCION             : Carga en el grid la búsqueda con las Calles
    ///// CREO                    : Antonio Salvador Benavides Guardado
    ///// FECHA_CREO              : 28/Agosto/2011
    ///// MODIFICO          :
    ///// FECHA_MODIFICO    :
    ///// CAUSA_MODIFICACION:
    /////*******************************************************************************
    //protected void Btn_Busqueda_Calles_Click(object sender, EventArgs e)
    //{
    //    Buscar_Calles(0);
    //}

    //private void Buscar_Calles(int Indice_Pagina)
    //{
    //    Cls_Cat_Pre_Calles_Negocio Calles = new Cls_Cat_Pre_Calles_Negocio();
    //    DataTable Dt_Calles;

    //    if (Txt_Busqueda_Avanzada_Calle_ID.Text.Trim() != "")
    //    {
    //        Calles.P_Calle_ID = "LIKE '%" + Txt_Busqueda_Avanzada_Calle_ID.Text + "%'";
    //    }
    //    if (Txt_Busqueda_Avanzada_Nombre_Calle.Text.ToUpper().Trim() != "")
    //    {
    //        Calles.P_Nombre = "LIKE '%" + Txt_Busqueda_Avanzada_Nombre_Calle.Text.ToUpper().Trim() + "%'";
    //    }
    //    Dt_Calles = Calles.Consultar_Nombre_Id_Calles();
    //    Grid_Calles.DataSource = Dt_Calles;
    //    Grid_Calles.PageIndex = Indice_Pagina;
    //    Grid_Calles.DataBind();
    //    Mpe_Busqueda_Calles.Show();
    //}

    //protected void Grid_Colonias_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    Buscar_Colonias(e.NewPageIndex);
    //}

    //protected void Grid_Colonias_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Hdn_Colonia_ID.Value = Grid_Colonias.SelectedRow.Cells[1].Text;
    //    Txt_Busqueda_Colonia.Text = Grid_Colonias.SelectedRow.Cells[2].Text;
    //    Mpe_Busqueda_Colonias.Hide();
    //    //Upd_Parametros_Predial.Update();
    //}

    //protected void Grid_Calles_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    Buscar_Calles(e.NewPageIndex);
    //}

    //protected void Grid_Calles_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Hdn_Calle_ID.Value = Grid_Calles.SelectedRow.Cells[1].Text;
    //    Txt_Busqueda_Calle.Text = Grid_Calles.SelectedRow.Cells[2].Text;
    //    Mpe_Busqueda_Calles.Hide();
    //}

    //protected void Txt_Busqueda_Avanzada_Colonia_ID_TextChanged(object sender, EventArgs e)
    //{
    //    Buscar_Colonias(0);
    //}

    //protected void Txt_Busqueda_Avanzada_Nombre_Colonia_TextChanged(object sender, EventArgs e)
    //{
    //    Buscar_Colonias(0);
    //}

    //protected void Txt_Busqueda_Avanzada_Calle_ID_TextChanged(object sender, EventArgs e)
    //{
    //    Buscar_Calles(0);
    //}

    //protected void Txt_Busqueda_Avanzada_Nombre_Calle_TextChanged(object sender, EventArgs e)
    //{
    //    Buscar_Calles(0);
    //}

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Operador_Comparacion
    ///DESCRIPCIÓN          : Devuelve una cadena adecuada al operador indicado en la capa de Negocios
    ///PARAMETROS           : 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 20/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private static String Validar_Operador_Comparacion(String Filtro)
    {
        String Cadena_Validada;
        if (Filtro.Trim().StartsWith("<")
           || Filtro.Trim().StartsWith(">")
           || Filtro.Trim().StartsWith("<>")
           || Filtro.Trim().StartsWith("<=")
           || Filtro.Trim().StartsWith(">=")
           || Filtro.Trim().StartsWith("=")
           || Filtro.Trim().ToUpper().StartsWith("BETWEEN")
           || Filtro.Trim().ToUpper().StartsWith("LIKE")
           || Filtro.Trim().ToUpper().StartsWith("IN"))
        {
            Cadena_Validada = " " + Filtro + " ";
        }
        else
        {
            if (Filtro.Trim().ToUpper().StartsWith("NULL"))
            {
                Cadena_Validada = " IS " + Filtro + " ";
            }
            else
            {
                Cadena_Validada = " = '" + Filtro + "' ";
            }
        }
        return Cadena_Validada;
    }

    protected void Btn_Buscar_Colonias_Click(object sender, ImageClickEventArgs e)
    {
        if (Session["BUSQUEDA_COLONIAS_CALLES"] != null)
        {
            if (Convert.ToBoolean(Session["BUSQUEDA_COLONIAS_CALLES"]) == true)
            {
                Hdn_Colonia_ID.Value = Session["COLONIA_ID"].ToString().Replace("&nbsp;", "");
                Txt_Busqueda_Colonia.Text = Session["NOMBRE_COLONIA"].ToString().Replace("&nbsp;", "");
                Hdn_Calle_ID.Value = Session["CALLE_ID"].ToString().Replace("&nbsp;", "");
                Txt_Busqueda_Calle.Text = Session["NOMBRE_CALLE"].ToString().Replace("&nbsp;", "");
            }
        }
    }
}
