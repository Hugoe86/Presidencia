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
using Presidencia.Operacion_Predial_Constancias.Negocio;
using Presidencia.Catalogo_Predial_Tipos_Documento.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Contribuyentes.Negocio;
using Presidencia.Catalogo_Tipos_Constancias.Negocio;
using Presidencia.Colonias.Negocios;
using Presidencia.Catalogo_Calles.Negocio;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using Presidencia.Catalogo_Claves_Ingreso.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Empleados.Negocios;
using System.Text;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using System.IO;
using DocumentFormat.OpenXml.Packaging;

public partial class paginas_predial_Frm_Ope_Pre_Constancias_Reimpresion : System.Web.UI.Page
{
    #region Pago_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS:     
    ///CREO: 
    ///FECHA_CREO: 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************        
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Configuracion_Formulario(true);

            //Scrip para mostrar Ventana Modal de las Tasas de Traslado
            String Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Mostrar_Busqueda_Avanzada.Attributes.Add("onclick", Ventana_Modal);
        }
        Div_Contenedor_Msj_Error.Visible = false;
    }

    #endregion

    #region Generar Reporte
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: generar_reporte
    ///DESCRIPCIÓN: caraga el data set fisoco con el cual se genera el reporte especificado
    ///PARAMETROS:  1.- data_set.- contiene la informacion de la consulta a la base de datos
    ///             2.-ds_reporte, objeto que contiene la instancia del Data set fisico del reporte a generar
    ///             3.-nombre_reporte, contiene la ruta del reporte a mostrar en pantalla
    ///CREO: Jacqueline Ramirez
    ///FECHA_CREO: 5/Septiembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Generar_Reporte(DataSet data_set, DataSet ds_reporte, string nombre_reporte)
    {


        ReportDocument reporte = new ReportDocument();
        string filePath = Server.MapPath("../Rpt/Predial/" + nombre_reporte);

        reporte.Load(filePath);

        DataRow renglon;

        for (int i = 0; i < data_set.Tables["Dt_Folio"].Rows.Count; i++)
        {
            renglon = data_set.Tables["Dt_Folio"].Rows[i];
            ds_reporte.Tables["Dt_Folio"].ImportRow(renglon);
        }
        reporte.SetDataSource(ds_reporte);

        //1
        ExportOptions exportOptions = new ExportOptions();
        //2
        DiskFileDestinationOptions diskFileDestinationOptions = new DiskFileDestinationOptions();
        //3
        //4
        diskFileDestinationOptions.DiskFileName = Server.MapPath("../../Reporte/Rpt_Pre_Constancias_Folio.pdf");
        //5
        exportOptions.ExportDestinationOptions = diskFileDestinationOptions;
        //6
        exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //7
        exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //8
        reporte.Export(exportOptions);
        //9
        string ruta = "../../Reporte/Rpt_Pre_Constancias_Folio.pdf";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
    }
    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Configuracion_Formulario
    ///DESCRIPCIÓN          : Carga una configuracion de los controles del Formulario
    ///PARAMETROS           : 1. Estatus.    Estatus en el que se cargara la configuración de los controles.
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Estatus)
    {
        //Btn_Nuevo.Visible = true;
        //Btn_Nuevo.AlternateText = "Nuevo";
        //Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        //Btn_Modificar.Visible = true;
        //Btn_Modificar.AlternateText = "Modificar";
        //Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        Grid_Constancias.Enabled = Estatus;
        Grid_Constancias.SelectedIndex = (-1);
        Txt_Cuenta_Predial.Enabled = false;
        Txt_Observaciones.Enabled = false;
        Txt_Folio.Enabled = true;// !Estatus;
        Btn_Mostrar_Busqueda_Avanzada.Enabled = true;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Limpiar_Catálogo
    ///DESCRIPCIÓN          : Limpia los controles del Formulario
    ///PARAMETROS           :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Hdf_No_Constancia.Value = "";
        Hdf_Cuenta_Predial_ID.Value = "";
        Hdf_Propietario_ID.Value = "";
        Txt_Cuenta_Predial.Text = "";
        Txt_Observaciones.Text = "";
        Txt_Folio.Text = "";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Llenar_Tabla_Certificaciones
    ///DESCRIPCIÓN          : Llena la tabla de Certificaciones con una consulta que puede o no tener Filtros.
    ///PARAMETROS           : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 01/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Constancias(int Pagina)
    {
        try
        {
            if (Txt_Cuenta_Predial.Text.Trim() != ""
                || Txt_Folio.Text.Trim() != "")
            {
                Cls_Ope_Pre_Constancias_Negocio Constancias = new Cls_Ope_Pre_Constancias_Negocio();
                Constancias.P_Campos_Dinamicos = "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Cuenta_Predial_ID + ") AS Cuenta_Predial, ";
                Constancias.P_Campos_Dinamicos += "(SELECT " + Cat_Pre_Contribuyentes.Campo_Nombre + " ||' '|| " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " ||' '|| " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " IN(SELECT " + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " WHERE " + Cat_Pre_Propietarios.Campo_Propietario_ID + " = " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Propietario_ID + ")) AS Nombre_Propietario, ";
                Constancias.P_Campos_Dinamicos += "(SELECT " + Cat_Pre_Tipos_Documento.Campo_Nombre_Documento + " FROM " + Cat_Pre_Tipos_Documento.Tabla_Cat_Pre_Tipos_Documento + " WHERE " + Cat_Pre_Tipos_Documento.Campo_Documento_ID + " = " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Documento_ID + ") AS Nombre_Documento, ";
                Constancias.P_Campos_Dinamicos += "(SELECT " + Cat_Pre_Tipos_Constancias.Campo_Nombre + " FROM " + Cat_Pre_Tipos_Constancias.Tabla_Cat_Pre_Tipos_Constancias + " WHERE " + Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID + " = " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Tipo_Constancia_ID + ") AS TIPO_CONSTANCIA, ";
                Constancias.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Periodo_Bimestre + "||'/'||" + Ope_Pre_Constancias.Campo_Periodo_Año + " Periodo, ";
                Constancias.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Solicitante + ", ";
                Constancias.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Solicitante_RFC + " RFC, ";
                Constancias.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Tipo_Constancia_ID + ", ";
                Constancias.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Folio + ", ";
                Constancias.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Fecha + ", ";
                Constancias.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_No_Impresiones + ", ";
                Constancias.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Estatus;
                //Constancias.P_Filtros_Dinamicos = Ope_Pre_Constancias.Campo_Tipo_Constancia_ID + " = '00004' AND ";
                Constancias.P_Filtros_Dinamicos = "(" + Ope_Pre_Constancias.Campo_Estatus + " = 'PAGADA' OR " + Ope_Pre_Constancias.Campo_Estatus + "='IMPRESA') AND ";
                //if (Txt_Folio.Text.Trim() != "" && Hdf_Cuenta_Predial_ID.Value != "")
                //{
                //    Constancias.P_Filtros_Dinamicos += "(UPPER(" + Ope_Pre_Constancias.Campo_Folio + ") LIKE UPPER('%" + Txt_Folio.Text + "%') OR ";
                //    //Constancias.P_Filtros_Dinamicos += "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Cuenta_Predial_ID + ") LIKE '%" + Hdf_Cuenta_Predial_ID.Value + "%')";
                //    Constancias.P_Filtros_Dinamicos += Ope_Pre_Constancias.Campo_Cuenta_Predial_ID + " = '" + Hdf_Cuenta_Predial_ID.Value + "')";
                //}
                //else
                //{
                //    if (Txt_Folio.Text.Trim() != "")
                //    {
                //        Constancias.P_Filtros_Dinamicos += "UPPER(" + Ope_Pre_Constancias.Campo_Folio + ") LIKE UPPER('%" + Txt_Folio.Text + "%')";
                //    }
                //    else
                //    {
                //        if (Hdf_Cuenta_Predial_ID.Value != "")
                //        {
                //            Constancias.P_Filtros_Dinamicos += Ope_Pre_Constancias.Campo_Cuenta_Predial_ID + " = '" + Hdf_Cuenta_Predial_ID.Value + "'";
                //        }
                //    }
                //}
                if (Txt_Folio.Text.Trim() != "")
                {
                    Constancias.P_Filtros_Dinamicos += "UPPER(" + Ope_Pre_Constancias.Campo_Folio + ") LIKE UPPER('%" + Txt_Folio.Text + "%') AND ";
                }
                if (Hdf_Cuenta_Predial_ID.Value != "")
                {
                    Constancias.P_Filtros_Dinamicos += Ope_Pre_Constancias.Campo_Cuenta_Predial_ID + " = '" + Hdf_Cuenta_Predial_ID.Value + "' AND ";
                }
                if (Constancias.P_Filtros_Dinamicos.EndsWith(" AND "))
                {
                    Constancias.P_Filtros_Dinamicos = Constancias.P_Filtros_Dinamicos.Substring(0, Constancias.P_Filtros_Dinamicos.Length - 5);
                }
                Constancias.P_Ordenar_Dinamico = Ope_Pre_Constancias.Campo_Anio + " DESC, " + Ope_Pre_Constancias.Campo_No_Constancia + " DESC";
                DataTable Tabla = Constancias.Consultar_Constancias();
                Grid_Constancias.DataSource = Tabla;
                Grid_Constancias.PageIndex = Pagina;
                Grid_Constancias.DataBind();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }

    }

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Componentes
    ///DESCRIPCIÓN          : Hace una validacion de que haya datos en los componentes antes de hacer una operación.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 01/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Txt_Cuenta_Predial.Text.Trim().Equals(""))
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introduzca la Cuenta Predial.";
            Validacion = false;
        }
        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
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
    //private void Cargar_Combo_Colonias()
    //{
    //    try
    //    {
    //        Cls_Ate_Colonias_Negocio Colonias = new Cls_Ate_Colonias_Negocio();
    //        Colonias.P_Campos_Dinamicos = Cat_Ate_Colonias.Campo_Colonia_ID + " COLONIA_ID, ";
    //        Colonias.P_Campos_Dinamicos += Cat_Ate_Colonias.Campo_Nombre + " NOMBRE";
    //        DataTable Dt_Colonias = Colonias.Consultar_Colonias();
    //        Cmb_Busqueda_Colonia.DataSource = Dt_Colonias;

    //        DataRow Dr_Colonias;
    //        Dr_Colonias = Dt_Colonias.NewRow();
    //        Dr_Colonias["COLONIA_ID"] = HttpUtility.HtmlDecode("00000");
    //        Dr_Colonias["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
    //        Dt_Colonias.Rows.InsertAt(Dr_Colonias, 0);

    //        Cmb_Busqueda_Colonia.DataValueField = "COLONIA_ID";
    //        Cmb_Busqueda_Colonia.DataTextField = "NOMBRE";
    //        Cmb_Busqueda_Colonia.DataBind();
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
    //private void Cargar_Combo_Calles()
    //{
    //    try
    //    {
    //        Cls_Cat_Pre_Calles_Negocio Calles = new Cls_Cat_Pre_Calles_Negocio();
    //        Calles.P_Campos_Dinamicos = Cat_Pre_Calles.Campo_Calle_ID + " CALLE_ID, ";
    //        Calles.P_Campos_Dinamicos += Cat_Pre_Calles.Campo_Nombre + " NOMBRE";
    //        if (Cmb_Busqueda_Colonia.SelectedIndex > 0)
    //        {
    //            Calles.P_Filtros_Dinamicos = Cat_Pre_Calles.Campo_Calle_ID + " IN (SELECT " + Cat_Pre_Calles_Colonias.Campo_Calle_ID + " FROM " + Cat_Pre_Calles_Colonias.Tabla_Cat_Pre_Calles_Colonias + " WHERE  " + Cat_Pre_Calles_Colonias.Campo_Colonia_ID + " IN (SELECT " + Cat_Ate_Colonias.Campo_Colonia_ID + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID + " = " + Cmb_Busqueda_Colonia.SelectedValue + "))";
    //        }
    //        DataTable Dt_Calles = Calles.Consultar_Calles();
    //        Cmb_Busqueda_Calle.DataSource = Dt_Calles;

    //        DataRow Dr_Colonias;
    //        Dr_Colonias = Dt_Calles.NewRow();
    //        Dr_Colonias["CALLE_ID"] = HttpUtility.HtmlDecode("00000");
    //        Dr_Colonias["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
    //        Dt_Calles.Rows.InsertAt(Dr_Colonias, 0);

    //        Cmb_Busqueda_Calle.DataValueField = "CALLE_ID";
    //        Cmb_Busqueda_Calle.DataTextField = "NOMBRE";
    //        Cmb_Busqueda_Calle.DataBind();
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //        Lbl_Mensaje_Error.Text = "";
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }

    //}

    #endregion

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Certificaciones_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView de los Tipos_Constancias
    ///PARAMETROS:
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 01/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Constancias_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Constancias.SelectedIndex = (-1);
            Llenar_Tabla_Constancias(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Certificaciones_RowCommand
    ///DESCRIPCIÓN          : Obtiene los datos de un Constancias Seleccionado para mostrarlos a detalle
    ///PARAMETROS:     
    ///CREO                 : Antonio Benavides Guardado
    ///FECHA_CREO           : 01/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Constancias_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Print":
                if (Grid_Constancias.Rows[Convert.ToInt32(e.CommandArgument)].Cells[11].Text.Trim() == "PAGADA" || Grid_Constancias.Rows[Convert.ToInt32(e.CommandArgument)].Cells[11].Text.Trim() == "IMPRESA")
                {
                    Crear_Ds_Constancias(Convert.ToInt32(e.CommandArgument));
                    Cls_Ope_Pre_Constancias_Negocio Constancias = new Cls_Ope_Pre_Constancias_Negocio();
                    Constancias.P_Folio = Grid_Constancias.Rows[Convert.ToInt32(e.CommandArgument)].Cells[9].Text.Trim();
                    Constancias.Incrementar_No_Impresiones_Constancia();
                    if (Grid_Constancias.Rows[Convert.ToInt32(e.CommandArgument)].Cells[11].Text.Trim() == "PAGADA")
                    {
                        Constancias.Constancia_Impresa();
                    }
                    Llenar_Tabla_Constancias(0);
                }
                break;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Certificaciones_SelectedIndexChanged
    ///DESCRIPCIÓN          : Obtiene los datos de un Constancias Seleccionado para mostrarlos a detalle
    ///PARAMETROS:     
    ///CREO                 : Antonio Benavides Guardado
    ///FECHA_CREO           : 01/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Constancias_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Constancias.SelectedIndex > (-1))
            {
                Limpiar_Catalogo();
                Cls_Ope_Pre_Constancias_Negocio Constancias = new Cls_Ope_Pre_Constancias_Negocio();
                DataTable Dt_Certificacion;

                Constancias.P_Campos_Foraneos = true;
                Constancias.P_Campos_Dinamicos = Ope_Pre_Constancias.Campo_No_Constancia + ", ";
                Constancias.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Propietario_ID + ", ";
                Constancias.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Cuenta_Predial_ID + ", ";
                Constancias.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Estatus + ", ";
                Constancias.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Documento_ID + ", ";
                Constancias.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Leyenda_Certificacion + ", ";
                Constancias.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Observaciones + ", ";
                Constancias.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Confronto + ", ";
                Constancias.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Folio + ", ";
                Constancias.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Fecha + ", ";
                Constancias.P_Campos_Dinamicos += " (SELECT " + Cat_Empleados.Campo_Confronto + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_Empleado_ID + "=" + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Confronto + ") AS INICIALES,";
                Constancias.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_No_Recibo;
                Constancias.P_Filtros_Dinamicos = "FOLIO = '" + Grid_Constancias.SelectedRow.Cells[9].Text.Trim() + "'";
                Dt_Certificacion = Constancias.Consultar_Constancias();

                if (Dt_Certificacion != null)
                {
                    if (Dt_Certificacion.Rows.Count > 0)
                    {
                        Hdf_No_Constancia.Value = Dt_Certificacion.Rows[0]["NO_CONSTANCIA"].ToString();
                        Hdf_Propietario_ID.Value = Dt_Certificacion.Rows[0]["PROPIETARIO_ID"].ToString();
                        Hdf_Cuenta_Predial_ID.Value = Dt_Certificacion.Rows[0]["CUENTA_PREDIAL_ID"].ToString();
                        Txt_Cuenta_Predial.Text = Dt_Certificacion.Rows[0]["Cuenta_Predial"].ToString();
                        Txt_Observaciones.Text = Dt_Certificacion.Rows[0]["OBSERVACIONES"].ToString();
                        Txt_Folio.Text = Dt_Certificacion.Rows[0]["FOLIO"].ToString();
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }

    }

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Insertar_Pasivo
    ///DESCRIPCIÓN          : Consulta el Costo del Documento y lo Inserta en Pasivo
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 26/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Insertar_Pasivo(String Referencia)
    {
        try
        {
            Cls_Cat_Pre_Claves_Ingreso_Negocio Claves_Ingreso = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Calculo_Impuesto_Traslado = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
            Cls_Cat_Pre_Tipos_Constancias_Negocio Tipos_Constancias = new Cls_Cat_Pre_Tipos_Constancias_Negocio();
            DataTable Dt_Clave;
            DataTable Dt_Tipos_Constancias;

            Claves_Ingreso.P_Documento_ID = "00004";
            Dt_Clave = Claves_Ingreso.Consultar_Clave_Ingreso();
            if (Dt_Clave.Rows.Count > 0)
            {
                Tipos_Constancias.P_Tipo_Constancia_ID = Claves_Ingreso.P_Documento_ID;
                Tipos_Constancias.P_Año = DateTime.Now.Year;
                Dt_Tipos_Constancias = Tipos_Constancias.Consultar_Tipos_Constancias();
                if (Dt_Tipos_Constancias.Rows.Count > 0)
                {
                    if (Dt_Tipos_Constancias.Rows[0][Cat_Pre_Tipos_Constancias.Campo_Costo].ToString() != "")
                    {
                        if (Convert.ToDouble(Dt_Tipos_Constancias.Rows[0][Cat_Pre_Tipos_Constancias.Campo_Costo]) != 0)
                        {
                            Calculo_Impuesto_Traslado.P_Referencia = Referencia;
                            Calculo_Impuesto_Traslado.P_Estatus = "POR PAGAR";
                            Calculo_Impuesto_Traslado.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                            Calculo_Impuesto_Traslado.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                            Calculo_Impuesto_Traslado.P_Monto_Total_Pagar = Dt_Tipos_Constancias.Rows[0][Cat_Pre_Tipos_Constancias.Campo_Costo].ToString();
                            Calculo_Impuesto_Traslado.P_Fecha_Tramite = DateTime.Now.ToString("dd/MMM/yyyy");
                            Calculo_Impuesto_Traslado.P_Fecha_Vencimiento_Pasivo = DateTime.Now.ToString("dd/MMM/yyyy");
                            Calculo_Impuesto_Traslado.Alta_Pasivo();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "El Pasivo no pudo ser insertado: " + Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Certificacion_Click
    ///DESCRIPCIÓN          : Llena la Tabla con la opcion buscada
    ///PARAMETROS          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 01/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Limpiar_Catalogo();
            Grid_Constancias.SelectedIndex = (-1);
            Llenar_Tabla_Constancias(0);
            if (Grid_Constancias.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
            {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda de \"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
                Lbl_Mensaje_Error.Text = "(Se cargaron todos los Certificaciones encontradas)";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Busqueda.Text = "";
                Llenar_Tabla_Constancias(0);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }

    }

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN : Btn_Imprimir_Click
    /////DESCRIPCIÓN          : Manda Imprimir el reporte
    /////PARAMETROS          :     
    /////CREO                 : Antonio Salvador Benavides Guardado
    /////FECHA_CREO           : 30/Junio/2011
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    //{
    //    if (Hdf_No_Constancia.Value != "")
    //    {
    //        Imprimir_Reporte(Crear_Ds_Constancias(), "Rpt_Pre_Constancias.rpt", "Certificación");
    //    }
    //}

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN : Btn_Eliminar_Click
    /////DESCRIPCIÓN          : Elimina un Constancias de la Base de Datos
    /////PARAMETROS          :     
    /////CREO                 : Antonio Salvador Benavides Guardado
    /////FECHA_CREO           : 01/Julio/2011
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Btn_Eliminar_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (Grid_Certificaciones.Rows.Count > 0 && Grid_Certificaciones.SelectedIndex > (-1))
    //        {
    //            Cls_Ope_Pre_Constancias_Negocio Constancias = new Cls_Ope_Pre_Constancias_Negocio();
    //            Constancias.P_Folio = Grid_Certificaciones.SelectedRow.Cells[3].Text;
    //            if (Constancias.Eliminar_Certificacion())
    //            {
    //                Grid_Certificaciones.SelectedIndex = (-1);
    //                Llenar_Tabla_Certificaciones(Grid_Certificaciones.PageIndex);
    //                ScriptManager.RegisterStartupScript(this, this.GetType(), "Certificaciones", "alert('Certificación fue Eliminada Exitosamente');", true);
    //                Limpiar_Catalogo();
    //            }
    //            else
    //            {
    //                ScriptManager.RegisterStartupScript(this, this.GetType(), "Certificaciones", "alert('La Certificación No fue Eliminada');", true);
    //            }
    //        }
    //        else
    //        {
    //            Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Eliminar.";
    //            Lbl_Mensaje_Error.Text = "";
    //            Div_Contenedor_Msj_Error.Visible = true;
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //        Lbl_Mensaje_Error.Text = "";
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }

    //}

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
    ///DESCRIPCIÓN          : Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 01/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Configuracion_Formulario(true);
            Limpiar_Catalogo();
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Mostrar_Busqueda_Avanzada_Click
    ///DESCRIPCIÓN          : Muestra los datos de la consulta
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Mostrar_Busqueda_Avanzada_Click(object sender, ImageClickEventArgs e)
    {
        Boolean Busqueda_Ubicaciones;
        String Cuenta_Predial_ID;
        String Cuenta_Predial;

        Busqueda_Ubicaciones = Convert.ToBoolean(Session["BUSQUEDA_CUENTAS_PREDIAL"]);
        if (Busqueda_Ubicaciones)
        {
            if (Session["CUENTA_PREDIAL_ID"] != null)
            {
                Cuenta_Predial_ID = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
                Hdf_Cuenta_Predial_ID.Value = Cuenta_Predial_ID;
                Cuenta_Predial = Convert.ToString(Session["CUENTA_PREDIAL"]);
                Txt_Cuenta_Predial.Text = Cuenta_Predial;
                Txt_Folio.Text = "";
            }
        }
        Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
        Session.Remove("CUENTA_PREDIAL_ID");
        Session.Remove("CUENTA_PREDIAL");

        Consultar_Datos_Cuenta_Predial();
        Llenar_Tabla_Constancias(0);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Cuenta_Predial
    ///DESCRIPCIÓN          : Consulta del catálogo la Cuentas Predial
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Consultar_Datos_Cuenta_Predial()
    {
        DataTable Dt_Cuentas_Predial;
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        if (Txt_Cuenta_Predial.Text.Trim() != "")
        {
            //Consulta la Cuenta Predial
            Cuentas_Predial.P_Campos_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", ";
            Cuentas_Predial.P_Campos_Dinamicos += "NULL " + Cat_Pre_Cuentas_Predial.Campo_Propietario_ID + ", ";
            Cuentas_Predial.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial;
            Cuentas_Predial.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            Dt_Cuentas_Predial = Cuentas_Predial.Consultar_Cuenta();
            if (Dt_Cuentas_Predial.Rows.Count > 0)
            {
                Hdf_Cuenta_Predial_ID.Value = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                Hdf_Propietario_ID.Value = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Propietario_ID].ToString();

                DataTable Dt_Contribuyentes;
                Cls_Cat_Pre_Contribuyentes_Negocio Contribuyentes = new Cls_Cat_Pre_Contribuyentes_Negocio();
                //Consulta los datos del Contribuyente
                Contribuyentes.P_Campos_Dinamicos = "P." + Cat_Pre_Propietarios.Campo_Propietario_ID + ", ";
                Contribuyentes.P_Campos_Dinamicos += "(C." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + "|| ' ' ||C." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + "|| ', ' ||C." + Cat_Pre_Contribuyentes.Campo_Nombre + ") AS NOMBRE_COMPLETO, ";
                Contribuyentes.P_Campos_Dinamicos += "(CO." + Cat_Ate_Colonias.Campo_Nombre + "|| ', ' ||CA." + Cat_Pre_Calles.Campo_Nombre + "|| ', ' ||CU." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + "|| ', ' ||CU." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + ") AS DOMICILIO";
                Contribuyentes.P_Filtros_Dinamicos = "P." + Cat_Pre_Propietarios.Campo_Tipo + " = 'PROPIETARIO' AND ";
                Contribuyentes.P_Filtros_Dinamicos += "C." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " IN (SELECT " + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " WHERE " + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = '" + Hdf_Cuenta_Predial_ID.Value + "')";
                Contribuyentes.P_Ordenar_Dinamico = "P." + Cat_Pre_Propietarios.Campo_Propietario_ID;
                Contribuyentes.P_Usuario = Hdf_Cuenta_Predial_ID.Value;
                Dt_Contribuyentes = Contribuyentes.Consultar_Contribuyentes();
                if (Dt_Contribuyentes.Rows.Count > 0)
                {
                    Hdf_Propietario_ID.Value = Dt_Contribuyentes.Rows[0]["PROPIETARIO_ID"].ToString();
                }
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Folio_TextChanged
    ///DESCRIPCIÓN          : Evento que manda ejecutar la Búsqueda de Constancias
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 02/Septiembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Folio_TextChanged(object sender, EventArgs e)
    {
        Txt_Cuenta_Predial.Text = "";
        Hdf_Cuenta_Predial_ID.Value = "";
        Llenar_Tabla_Constancias(0);
    }

    #endregion

    #region Impresion Folios

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 19/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Boolean Imprimir_Reporte(DataSet Ds_Constancias, String Nombre_Reporte, String Nombre_Archivo, String Folio)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);
        Boolean Impresion_Correcta = false;
        try
        {
            Reporte.Load(File_Path);
            Reporte.Subreports["Rpt_Constancias_Propiedad"].SetDataSource(Ds_Constancias.Tables["Dt_Constancias_Propiedad"]);
            Reporte.Subreports["Rpt_Constancias_No_Propiedad"].SetDataSource(Ds_Constancias.Tables["Dt_Constancias_No_Propiedad"]);
            Reporte.Subreports["Rpt_Constancias_No_Adeudo"].SetDataSource(Ds_Constancias.Tables["Dt_Constancias_No_Adeudo"]);
            Reporte.Subreports["Rpt_Certificaciones"].SetDataSource(Ds_Constancias.Tables["Dt_Certificaciones"]);

            String Archivo_PDF = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar    
            try
            {
                ExportOptions Export_Options = new ExportOptions();
                DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();

                if (Folio.Contains("CNP"))
                {
                    try
                    {
                        StringBuilder Domicilio = new StringBuilder();
                        //******** PASAMOS EL DOMICILIO COMO PARÁMETRO AL REPORTE ********
                        Domicilio.Append(String.Empty);


                        ParameterFieldDefinitions Cr_Parametros;
                        ParameterFieldDefinition Cr_Parametro;
                        ParameterValues Cr_Valor_Parametro = new ParameterValues();
                        ParameterDiscreteValue Cr_Valor = new ParameterDiscreteValue();

                        Cr_Parametros = Reporte.DataDefinition.ParameterFields;

                        Cr_Parametro = Cr_Parametros["Domicilio"];
                        Cr_Valor_Parametro = Cr_Parametro.CurrentValues;
                        Cr_Valor_Parametro.Clear();

                        Cr_Valor.Value = String.Empty;
                        Cr_Valor_Parametro.Add(Cr_Valor);
                        Cr_Parametro.ApplyCurrentValues(Cr_Valor_Parametro);


                        Cr_Parametro = Cr_Parametros["Bimestres"];
                        Cr_Valor_Parametro = Cr_Parametro.CurrentValues;
                        Cr_Valor_Parametro.Clear();

                        Cr_Valor.Value = String.Empty;
                        Cr_Valor_Parametro.Add(Cr_Valor);
                        Cr_Parametro.ApplyCurrentValues(Cr_Valor_Parametro);
                        //****************************************************************
                    }
                    catch { }
                }
                else if (Folio.Contains("CP"))
                {
                    try
                    {

                        ParameterFieldDefinitions Cr_Parametros;
                        ParameterFieldDefinition Cr_Parametro;
                        ParameterValues Cr_Valor_Parametro = new ParameterValues();
                        ParameterDiscreteValue Cr_Valor = new ParameterDiscreteValue();

                        Cr_Parametros = Reporte.DataDefinition.ParameterFields;


                        Cr_Parametro = Cr_Parametros["Domicilio"];
                        Cr_Valor_Parametro = Cr_Parametro.CurrentValues;
                        Cr_Valor_Parametro.Clear();

                        Cr_Valor.Value = Hdf_Propietario_ID.Value.ToString().Trim();
                        Cr_Valor_Parametro.Add(Cr_Valor);
                        Cr_Parametro.ApplyCurrentValues(Cr_Valor_Parametro);
                        Hdf_Propietario_ID.Value = "";

                        Cr_Parametros = Reporte.DataDefinition.ParameterFields;

                        Cr_Parametro = Cr_Parametros["Bimestres"];
                        Cr_Valor_Parametro = Cr_Parametro.CurrentValues;
                        Cr_Valor_Parametro.Clear();

                        Cr_Valor.Value = String.Empty;
                        Cr_Valor_Parametro.Add(Cr_Valor);
                        Cr_Parametro.ApplyCurrentValues(Cr_Valor_Parametro);
                        //****************************************************************
                    }
                    catch { }
                }
                else if (Folio.Contains("CNA"))
                {
                    try
                    {
                        //******** PASAMOS EL DOMICILIO COMO PARÁMETRO AL REPORTE ********
                        ParameterFieldDefinitions Cr_Parametros;

                        ParameterFieldDefinition Cr_Parametro;
                        ParameterValues Cr_Valor_Parametro = new ParameterValues();
                        ParameterDiscreteValue Cr_Valor = new ParameterDiscreteValue();

                        Cr_Parametros = Reporte.DataDefinition.ParameterFields;

                        Cr_Parametro = Cr_Parametros["Domicilio"];
                        Cr_Valor_Parametro = Cr_Parametro.CurrentValues;
                        Cr_Valor_Parametro.Clear();

                        Cr_Valor.Value = String.Empty;
                        Cr_Valor_Parametro.Add(Cr_Valor);
                        Cr_Parametro.ApplyCurrentValues(Cr_Valor_Parametro);

                        Cr_Parametro = Cr_Parametros["Bimestres"];
                        Cr_Valor_Parametro = Cr_Parametro.CurrentValues;
                        Cr_Valor_Parametro.Clear();

                        Cr_Valor.Value = Hdf_Propietario_ID.Value.ToString().ToUpper();
                        Cr_Valor_Parametro.Add(Cr_Valor);
                        Cr_Parametro.ApplyCurrentValues(Cr_Valor_Parametro);


                        //****************************************************************
                    }
                    catch { }
                }
                else if (Folio.Contains("COD"))
                {
                    try
                    {
                        //******** PASAMOS EL DOMICILIO COMO PARÁMETRO AL REPORTE ********

                        ParameterFieldDefinitions Cr_Parametros;
                        ParameterFieldDefinition Cr_Parametro;
                        ParameterValues Cr_Valor_Parametro = new ParameterValues();
                        ParameterDiscreteValue Cr_Valor = new ParameterDiscreteValue();

                        Cr_Parametros = Reporte.DataDefinition.ParameterFields;

                        Cr_Parametro = Cr_Parametros["Domicilio"];
                        Cr_Valor_Parametro = Cr_Parametro.CurrentValues;
                        Cr_Valor_Parametro.Clear();

                        Cr_Valor.Value = String.Empty;
                        Cr_Valor_Parametro.Add(Cr_Valor);
                        Cr_Parametro.ApplyCurrentValues(Cr_Valor_Parametro);


                        Cr_Parametro = Cr_Parametros["Bimestres"];
                        Cr_Valor_Parametro = Cr_Parametro.CurrentValues;
                        Cr_Valor_Parametro.Clear();

                        Cr_Valor.Value = String.Empty;
                        Cr_Valor_Parametro.Add(Cr_Valor);
                        Cr_Parametro.ApplyCurrentValues(Cr_Valor_Parametro);
                        //****************************************************************
                    }
                    catch { }
                }
                Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_PDF);
                Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
                Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
                Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
                Reporte.Export(Export_Options);

                try
                {
                    //Reporte.PrintToPrinter(1, true, 0, 0);
                    if (Mostrar_Reporte(Archivo_PDF, "PDF"))
                    {
                        Impresion_Correcta = true;
                    }
                    else
                    {
                        Impresion_Correcta = false;
                    }
                }
                catch (Exception Ex)
                {
                    Impresion_Correcta = false;
                    Lbl_Mensaje_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = Ex.Message.ToString();
                }
            }
            catch
            {
                Impresion_Correcta = false;
                //Lbl_Mensaje_Error.Visible = true;
                //Lbl_Mensaje_Error.Text = "No se pudo exportar el reporte";
            }
        }
        catch
        {
            Impresion_Correcta = false;
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte para su impresión";
        }
        return Impresion_Correcta;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Mostrar_Reporte
    ///DESCRIPCIÓN          : Visualiza en pantalla el reporte indicado
    ///PARAMETROS           : Nombre_Reporte: cadena con el nombre del archivo.
    ///                     : Formato: Exensión del archivo a visualizar.
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Boolean Mostrar_Reporte(String Nombre_Reporte, String Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
        Boolean Impresion_Correcta = false;

        try
        {
            if (Formato == "PDF")
            {
                Pagina = Pagina + Nombre_Reporte;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt", "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
                Impresion_Correcta = true;
            }
            else if (Formato == "Excel")
            {
                String Ruta = "../../Reporte/" + Nombre_Reporte;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
                Impresion_Correcta = true;
            }
        }
        catch (Exception Ex)
        {
            Impresion_Correcta = false;
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
        return Impresion_Correcta;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Constancias
    ///DESCRIPCIÓN          : Crea un DataTable con las columnas y datos de la Constancia de Propiedad Seleccionada en el GridView
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 19/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Crear_Ds_Constancias(int Fila_Actual)
    {
        Cls_Ope_Pre_Constancias_Negocio Constancia = new Cls_Ope_Pre_Constancias_Negocio();
        Constancia.P_Folio = Grid_Constancias.Rows[Fila_Actual].Cells[9].Text;
        DataTable Dt_Constancia_Llenar_Reporte;
        string Domicilio = "";

        if (Grid_Constancias.Rows[Fila_Actual].Cells[9].Text.Contains("CP"))
        {
            Dt_Constancia_Llenar_Reporte = Consultar_Constancia_Propiedad(Constancia);
            Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            Cuenta.P_Cuenta_Predial_ID = Dt_Constancia_Llenar_Reporte.Rows[0]["CUENTA_PREDIAL_ID"].ToString();
            Cuenta = Cuenta.Consultar_Datos_Propietario();
            Domicilio = "";
            if (Cuenta.P_Nombre_Calle != null && Cuenta.P_Nombre_Calle != "")
            {
                Domicilio = "CALLE " + Cuenta.P_Nombre_Calle;
            }
            if (Cuenta.P_No_Exterior != null && Cuenta.P_No_Exterior != "")
            {
                if (Domicilio != "")
                {
                    Domicilio += ", ";
                }
                Domicilio += Cuenta.P_No_Exterior;
            }
            if (Cuenta.P_No_Interior != null && Cuenta.P_No_Interior != "")
            {
                if (Domicilio != "")
                {
                    Domicilio += ", ";
                }
                Domicilio += Cuenta.P_No_Interior;
            }
            if (Cuenta.P_Nombre_Colonia != null && Cuenta.P_Nombre_Colonia != "")
            {
                if (Domicilio != "")
                {
                    Domicilio += ", ";
                }
                Domicilio += "COLONIA " + Cuenta.P_Nombre_Colonia;
            }

            string Ruta_Plantilla = Server.MapPath("PlantillasWord/" + "Formato_Constancia_Propiedad.docx");
            string Documento_Salida = Server.MapPath("../../Reporte/" + "Constancia_Propiedad.docx");
            //Documento_Salida = Server.MapPath("~/Reporte/" + "Convenio_Der_Sup.docx");

            //create copy of template so that we don't overwrite it
            if (System.IO.File.Exists(Documento_Salida))
            {
                System.IO.File.Delete(Documento_Salida);
            }
            File.Copy(Ruta_Plantilla, Documento_Salida);

            ReportDocument Reporte = new ReportDocument();
            String Nombre_Archivo = "Formato_Constancia_Propiedad.docx";
            String PDF_Convenio = Nombre_Archivo + ".pdf";

            // si no existe el directorio, crearlo
            if (!System.IO.Directory.Exists(Server.MapPath("../../Reporte")))
                System.IO.Directory.CreateDirectory("../../Reporte");

            Lbl_Mensaje_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            String Confronto = Dt_Constancia_Llenar_Reporte.Rows[0]["INICIALES"].ToString();
            if (Confronto == "")
            {
                Confronto = " ";
            }
            try
            {
                using (WordprocessingDocument doc = WordprocessingDocument.Open(Documento_Salida, true))
                {
                    //create XML string matching custom XML part
                    string newXml = "<root>"
            + "<CUENTA_PREDIAL>" + Grid_Constancias.Rows[Fila_Actual].Cells[1].Text + "</CUENTA_PREDIAL>"
            + "<PROPIETARIO>" + Dt_Constancia_Llenar_Reporte.Rows[0]["Nombre_Propietario"].ToString() + "</PROPIETARIO>"
            + "<DOMICILIO>" + Domicilio + "</DOMICILIO>"
            + "<DIAS>" + DateTime.Now.ToString("dd") + "</DIAS>"
            + "<MES>" + DateTime.Now.ToString("MMMM").ToUpper() + "</MES>"
            + "<ANIO>" + DateTime.Now.ToString("yyyy") + "</ANIO>"
            + "<CONFRONTO>" + Confronto + "</CONFRONTO>"
            + "<FOLIO>" + Dt_Constancia_Llenar_Reporte.Rows[0]["FOLIO"].ToString() + "</FOLIO>"
            + "<PROTECCION_PAGO>" + Dt_Constancia_Llenar_Reporte.Rows[0]["PROTECCION_PAGO"].ToString() + "</PROTECCION_PAGO>"
            + "</root>";

                    MainDocumentPart main = doc.MainDocumentPart;
                    main.DeleteParts<CustomXmlPart>(main.CustomXmlParts);

                    //add and write new XML part
                    CustomXmlPart customXml = main.AddCustomXmlPart(CustomXmlPartType.CustomXml);

                    using (StreamWriter ts = new StreamWriter(customXml.GetStream()))
                    {
                        ts.Write(newXml);
                    }
                    // guardar los cambios en el documento
                    main.Document.Save();

                    //closing WordprocessingDocument automatically saves the document
                }

                //string Ruta = @HttpContext.Current.Server.MapPath("~/Reporte/" + Nombre_Archivo);
                //// ofrecer para descarga
                //Response.Clear();
                //Response.ClearContent();
                //Response.ClearHeaders();
                //Response.ContentType = "application/x-msword";
                //Response.AddHeader("Content-Disposition", "attachment; filename=" + Ruta);
                ////           'Visualiza el archivo
                //Response.WriteFile(Ruta);
                //Response.Flush();
                //Response.Close();

                String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
                Pagina = Pagina + "Constancia_Propiedad.docx";
                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "Formato_Convenio",
                    "window.open('" + Pagina +
                    "', '" + "msword" + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')",
                    true
                    );
            }
            catch (Exception Ex)
            {
                throw new Exception("Imprimir Constancia: " + Ex.Message);
            }
        }
        if (Grid_Constancias.Rows[Fila_Actual].Cells[9].Text.Contains("CNP"))
        {
            Dt_Constancia_Llenar_Reporte = Consultar_Constancia_No_Propiedad(Constancia);

            string Ruta_Plantilla = Server.MapPath("PlantillasWord/" + "Formato_Constancia_No_Propiedad.docx");
            string Documento_Salida = Server.MapPath("../../Reporte/" + "Constancia_No_Propiedad.docx");
            //Documento_Salida = Server.MapPath("~/Reporte/" + "Convenio_Der_Sup.docx");

            //create copy of template so that we don't overwrite it
            if (System.IO.File.Exists(Documento_Salida))
            {
                System.IO.File.Delete(Documento_Salida);
            }
            File.Copy(Ruta_Plantilla, Documento_Salida);

            ReportDocument Reporte = new ReportDocument();
            String Nombre_Archivo = "Formato_Constancia_No_Propiedad.docx";
            String PDF_Convenio = Nombre_Archivo + ".pdf";

            // si no existe el directorio, crearlo
            if (!System.IO.Directory.Exists(Server.MapPath("../../Reporte")))
                System.IO.Directory.CreateDirectory("../../Reporte");

            Lbl_Mensaje_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            DateTime Fecha = Convert.ToDateTime(Dt_Constancia_Llenar_Reporte.Rows[0]["FECHA"]);
            String Rfc = Dt_Constancia_Llenar_Reporte.Rows[0]["SOLICITANTE_RFC"].ToString();
            String Confronto = Dt_Constancia_Llenar_Reporte.Rows[0]["INICIALES"].ToString();
            if (Dt_Constancia_Llenar_Reporte.Rows[0]["SOLICITANTE_RFC"].ToString().Equals(""))
            {
                Rfc = " ";
            }
            if (Dt_Constancia_Llenar_Reporte.Rows[0]["INICIALES"].ToString() == "")
            {
                Confronto = " ";
            }

            try
            {
                using (WordprocessingDocument doc = WordprocessingDocument.Open(Documento_Salida, true))
                {
                    //create XML string matching custom XML part
                    string newXml = "<root>"
                        + "<FOLIO>" + Grid_Constancias.Rows[Fila_Actual].Cells[9].Text + "</FOLIO>"
                        + "<NOMBRE>" + Dt_Constancia_Llenar_Reporte.Rows[0]["SOLICITANTE"].ToString().ToUpper() + "</NOMBRE>"
                        + "<RFC>" + Rfc + "</RFC>"
                        + "<DOMICILIO>" + Dt_Constancia_Llenar_Reporte.Rows[0]["DOMICILIO"].ToString() + "</DOMICILIO>"
                        + "<DIAS>" + DateTime.Now.ToString("dd") + "</DIAS>"
                        + "<MES>" + DateTime.Now.ToString("MMMM").ToUpper() + "</MES>"
                        + "<ANIO>" + DateTime.Now.ToString("yyyy") + "</ANIO>"
                        + "<CONFRONTO>" + Confronto + "</CONFRONTO>"
                        + "<FECHA>" + Convert.ToDateTime(Dt_Constancia_Llenar_Reporte.Rows[0]["FECHA"].ToString()).ToString("dd/MMM/yyyy").ToUpper() + "</FECHA>"
                        + "<PROTECCION_PAGO>" + Dt_Constancia_Llenar_Reporte.Rows[0]["PROTECCION_PAGO"].ToString() + "</PROTECCION_PAGO>"
                        + "</root>";

                    MainDocumentPart main = doc.MainDocumentPart;
                    main.DeleteParts<CustomXmlPart>(main.CustomXmlParts);

                    //add and write new XML part
                    CustomXmlPart customXml = main.AddCustomXmlPart(CustomXmlPartType.CustomXml);

                    using (StreamWriter ts = new StreamWriter(customXml.GetStream()))
                    {
                        ts.Write(newXml);
                    }
                    // guardar los cambios en el documento
                    main.Document.Save();

                    //closing WordprocessingDocument automatically saves the document
                }

                //string Ruta = @HttpContext.Current.Server.MapPath("~/Reporte/" + Nombre_Archivo);
                //// ofrecer para descarga
                //Response.Clear();
                //Response.ClearContent();
                //Response.ClearHeaders();
                //Response.ContentType = "application/x-msword";
                //Response.AddHeader("Content-Disposition", "attachment; filename=" + Ruta);
                ////           'Visualiza el archivo
                //Response.WriteFile(Ruta);
                //Response.Flush();
                //Response.Close();

                String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
                Pagina = Pagina + "Constancia_No_Propiedad.docx";
                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "Formato_Convenio",
                    "window.open('" + Pagina +
                    "', '" + "msword" + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')",
                    true
                    );
            }
            catch (Exception Ex)
            {
                throw new Exception("Imprimir Constancia: " + Ex.Message);
            }
        }




        if (Grid_Constancias.Rows[Fila_Actual].Cells[9].Text.Contains("CNA"))
        {
            Dt_Constancia_Llenar_Reporte = Consultar_Constancia_No_Adeudo(Constancia);
            Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            Cuenta.P_Cuenta_Predial_ID = Dt_Constancia_Llenar_Reporte.Rows[0]["CUENTA_PREDIAL_ID"].ToString();
            Cuenta = Cuenta.Consultar_Datos_Propietario();
            //Se guardaran los anios y bimestres en la variables hdf_propietario_id
            Hdf_Propietario_ID.Value = Dt_Constancia_Llenar_Reporte.Rows[0]["PERIODO_BIMESTRE"].ToString() + " BIMESTRE DEL " + Dt_Constancia_Llenar_Reporte.Rows[0]["PERIODO_AÑO"].ToString() + " HASTA EL " + Dt_Constancia_Llenar_Reporte.Rows[0]["HASTA_BIMESTRE"].ToString() + " BIMESTRE DEL " + Dt_Constancia_Llenar_Reporte.Rows[0]["HASTA_ANIO"].ToString();


            string Ruta_Plantilla = Server.MapPath("PlantillasWord/" + "Formato_Constancia_No_Adeudo.docx");
            string Documento_Salida = Server.MapPath("../../Reporte/" + "Constancia_No_Adeudo.docx");
            //Documento_Salida = Server.MapPath("~/Reporte/" + "Convenio_Der_Sup.docx");

            //create copy of template so that we don't overwrite it
            if (System.IO.File.Exists(Documento_Salida))
            {
                System.IO.File.Delete(Documento_Salida);
            }
            File.Copy(Ruta_Plantilla, Documento_Salida);

            ReportDocument Reporte = new ReportDocument();
            String Nombre_Archivo = "Formato_Constancia_No_Adeudo.docx";
            String PDF_Convenio = Nombre_Archivo + ".pdf";

            // si no existe el directorio, crearlo
            if (!System.IO.Directory.Exists(Server.MapPath("../../Reporte")))
                System.IO.Directory.CreateDirectory("../../Reporte");

            Lbl_Mensaje_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            String Confronto = Dt_Constancia_Llenar_Reporte.Rows[0]["INICIALES"].ToString();
            if (Confronto == "")
            {
                Confronto = " ";
            }
            Domicilio = "";
            if (Cuenta.P_Nombre_Calle != null && Cuenta.P_Nombre_Calle != "")
            {
                Domicilio = "CALLE " + Cuenta.P_Nombre_Calle;
            }
            if (Cuenta.P_No_Exterior != null && Cuenta.P_No_Exterior != "")
            {
                if (Domicilio != "")
                {
                    Domicilio += ", ";
                }
                Domicilio += Cuenta.P_No_Exterior;
            }
            if (Cuenta.P_No_Interior != null && Cuenta.P_No_Interior != "")
            {
                if (Domicilio != "")
                {
                    Domicilio += ", ";
                }
                Domicilio += Cuenta.P_No_Interior;
            }
            if (Cuenta.P_Nombre_Colonia != null && Cuenta.P_Nombre_Colonia != "")
            {
                if (Domicilio != "")
                {
                    Domicilio += ", ";
                }
                Domicilio += "COLONIA " + Cuenta.P_Nombre_Colonia;
            }
            try
            {
                using (WordprocessingDocument doc = WordprocessingDocument.Open(Documento_Salida, true))
                {
                    //create XML string matching custom XML part
                    string newXml = "<root>"
                    + "<CUENTA_PREDIAL>" + Grid_Constancias.Rows[Fila_Actual].Cells[1].Text + "</CUENTA_PREDIAL>"
                    + "<PROPIETARIO>" + Dt_Constancia_Llenar_Reporte.Rows[0]["Nombre_Propietario"].ToString() + "</PROPIETARIO>"
                    + "<DOMICILIO>" + Domicilio + "</DOMICILIO>"
                    + "<BIMESTRES>" + Hdf_Propietario_ID.Value + "</BIMESTRES>"
                    + "<VALOR_FISCAL>" + "$ " + Convert.ToDouble(Obtener_Dato_Consulta(Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal, Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas, Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Grid_Constancias.Rows[Fila_Actual].Cells[1].Text + "'")).ToString("#,###,###,###,###,###,###,###,##0.00") + "</VALOR_FISCAL>"
                    + "<DIAS>" + DateTime.Now.ToString("dd") + "</DIAS>"
                    + "<MES>" + DateTime.Now.ToString("MMMM").ToUpper() + "</MES>"
                    + "<ANIO>" + DateTime.Now.ToString("yyyy") + "</ANIO>"
                    + "<CONFRONTO>" + Confronto + "</CONFRONTO>"
                    + "<FOLIO>" + Dt_Constancia_Llenar_Reporte.Rows[0]["FOLIO"].ToString() + "</FOLIO>"
                    + "<PROTECCION_PAGO>" + Dt_Constancia_Llenar_Reporte.Rows[0]["PROTECCION_PAGO"].ToString() + "</PROTECCION_PAGO>"
                    + "</root>";

                    MainDocumentPart main = doc.MainDocumentPart;
                    main.DeleteParts<CustomXmlPart>(main.CustomXmlParts);

                    //add and write new XML part
                    CustomXmlPart customXml = main.AddCustomXmlPart(CustomXmlPartType.CustomXml);

                    using (StreamWriter ts = new StreamWriter(customXml.GetStream()))
                    {
                        ts.Write(newXml);
                    }
                    // guardar los cambios en el documento
                    main.Document.Save();

                    //closing WordprocessingDocument automatically saves the document
                }

                //string Ruta = @HttpContext.Current.Server.MapPath("~/Reporte/" + Nombre_Archivo);
                //// ofrecer para descarga
                //Response.Clear();
                //Response.ClearContent();
                //Response.ClearHeaders();
                //Response.ContentType = "application/x-msword";
                //Response.AddHeader("Content-Disposition", "attachment; filename=" + Ruta);
                ////           'Visualiza el archivo
                //Response.WriteFile(Ruta);
                //Response.Flush();
                //Response.Close();

                String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
                Pagina = Pagina + "Constancia_No_Adeudo.docx";
                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "Formato_Convenio",
                    "window.open('" + Pagina +
                    "', '" + "msword" + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')",
                    true
                    );
            }
            catch (Exception Ex)
            {
                throw new Exception("Imprimir Constancia: " + Ex.Message);
            }
        }




        if (Grid_Constancias.Rows[Fila_Actual].Cells[9].Text.Contains("COD"))
        {
            Dt_Constancia_Llenar_Reporte = Consultar_Certificaciones(Constancia);
            string Ruta_Plantilla = Server.MapPath("PlantillasWord/" + "Formato_Certificaciones.docx");
            string Documento_Salida = Server.MapPath("../../Reporte/" + "Certificacion.docx");
            //Documento_Salida = Server.MapPath("~/Reporte/" + "Convenio_Der_Sup.docx");

            //create copy of template so that we don't overwrite it
            if (System.IO.File.Exists(Documento_Salida))
            {
                System.IO.File.Delete(Documento_Salida);
            }
            File.Copy(Ruta_Plantilla, Documento_Salida);

            ReportDocument Reporte = new ReportDocument();
            String Nombre_Archivo = "Formato_Certificaciones.docx";
            String PDF_Convenio = Nombre_Archivo + ".pdf";

            // si no existe el directorio, crearlo
            if (!System.IO.Directory.Exists(Server.MapPath("../../Reporte")))
                System.IO.Directory.CreateDirectory("../../Reporte");

            Lbl_Mensaje_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            String confronto = Dt_Constancia_Llenar_Reporte.Rows[0]["INICIALES"].ToString();
            if (Dt_Constancia_Llenar_Reporte.Rows[0]["INICIALES"].ToString() == "")
            {
                confronto = " ";
            }
            try
            {
                using (WordprocessingDocument doc = WordprocessingDocument.Open(Documento_Salida, true))
                {
                    //create XML string matching custom XML part
                    string newXml = "<root>"
                    + "<DIAS>" + DateTime.Now.ToString("dd") + "</DIAS>"
                    + "<MES>" + DateTime.Now.ToString("MMMM").ToUpper() + "</MES>"
                    + "<ANIO>" + DateTime.Now.ToString("yyyy") + "</ANIO>"
                    + "<CONFRONTO>" + confronto + "</CONFRONTO>"
                    + "<FOLIO>" + Grid_Constancias.Rows[Fila_Actual].Cells[9].Text + "</FOLIO>"
                    + "<PROTECCION_PAGO>" + Dt_Constancia_Llenar_Reporte.Rows[0]["PROTECCION_PAGO"].ToString() + "</PROTECCION_PAGO>"
                    + "</root>";

                    MainDocumentPart main = doc.MainDocumentPart;
                    main.DeleteParts<CustomXmlPart>(main.CustomXmlParts);

                    //add and write new XML part
                    CustomXmlPart customXml = main.AddCustomXmlPart(CustomXmlPartType.CustomXml);

                    using (StreamWriter ts = new StreamWriter(customXml.GetStream()))
                    {
                        ts.Write(newXml);
                    }
                    // guardar los cambios en el documento
                    main.Document.Save();

                    //closing WordprocessingDocument automatically saves the document
                }

                //string Ruta = @HttpContext.Current.Server.MapPath("~/Reporte/" + Nombre_Archivo);
                //// ofrecer para descarga
                //Response.Clear();
                //Response.ClearContent();
                //Response.ClearHeaders();
                //Response.ContentType = "application/x-msword";
                //Response.AddHeader("Content-Disposition", "attachment; filename=" + Ruta);
                ////           'Visualiza el archivo
                //Response.WriteFile(Ruta);
                //Response.Flush();
                //Response.Close();

                String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
                Pagina = Pagina + "Certificacion.docx";
                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "Formato_Convenio",
                    "window.open('" + Pagina +
                    "', '" + "msword" + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')",
                    true
                    );
            }
            catch (Exception Ex)
            {
                throw new Exception("Imprimir Constancia: " + Ex.Message);
            }
        }
    }

    #endregion
    protected void Grid_Constancias_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Grid_Constancias.DataKeys[e.Row.RowIndex].Values[0].ToString() == "00002")
            {
                e.Row.Cells[1].Text = "N/A";
                e.Row.Cells[2].Text = Grid_Constancias.DataKeys[e.Row.RowIndex].Values[1].ToString();
            }
        }
    }


    private DataTable Consultar_Constancia_Propiedad(Cls_Ope_Pre_Constancias_Negocio Constancia)
    {
        Constancia.P_Campos_Foraneos = true;
        Constancia.P_Campos_Dinamicos = Ope_Pre_Constancias.Campo_No_Constancia + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Estatus + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Cuenta_Predial_ID + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Folio + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Fecha + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Fecha_Vencimiento + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_No_Recibo + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Proteccion_Pago + ", ";
        Constancia.P_Campos_Dinamicos += "(SELECT " + Cat_Empleados.Campo_Confronto + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_Empleado_ID + "=" + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Confronto + ") AS INICIALES, ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Observaciones;
        Constancia.P_Filtros_Dinamicos = "FOLIO = '" + Constancia.P_Folio + "'";
        return Constancia.Consultar_Constancias();
    }

    private DataTable Consultar_Constancia_No_Propiedad(Cls_Ope_Pre_Constancias_Negocio Constancia)
    {
        Constancia.P_Campos_Foraneos = true;
        Constancia.P_Campos_Dinamicos = Ope_Pre_Constancias.Campo_No_Constancia + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Propietario_ID + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Cuenta_Predial_ID + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Estatus + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Solicitante + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Solicitante_RFC + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Confronto + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Folio + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Domicilio + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Fecha + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Fecha_Vencimiento + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Proteccion_Pago + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_No_Recibo + ", ";
        Constancia.P_Campos_Dinamicos += "(SELECT " + Cat_Empleados.Campo_Confronto + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_Empleado_ID + "=" + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Confronto + ") AS INICIALES, ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Observaciones;
        Constancia.P_Filtros_Dinamicos = "FOLIO = '" + Constancia.P_Folio + "'";
        return Constancia.Consultar_Constancias();
    }

    private DataTable Consultar_Constancia_No_Adeudo(Cls_Ope_Pre_Constancias_Negocio Constancia)
    {
        Constancia.P_Campos_Foraneos = true;
        Constancia.P_Campos_Dinamicos = Ope_Pre_Constancias.Campo_No_Constancia + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Cuenta_Predial_ID + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Propietario_ID + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Periodo_Año + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Periodo_Bimestre + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Periodo_Hasta_Anio + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Periodo_Hasta_Bimestre + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Estatus + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Confronto + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Folio + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Fecha + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Fecha_Vencimiento + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Proteccion_Pago + ", ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_No_Recibo + ", ";
        Constancia.P_Campos_Dinamicos += " (SELECT " + Cat_Empleados.Campo_Confronto + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_Empleado_ID + "=" + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Confronto + ") AS INICIALES, ";
        Constancia.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Observaciones;
        Constancia.P_Filtros_Dinamicos = "FOLIO = '" + Constancia.P_Folio + "'";
        return Constancia.Consultar_Constancias();
    }

    private DataTable Consultar_Certificaciones(Cls_Ope_Pre_Constancias_Negocio Certificacion)
    {
        Certificacion.P_Campos_Foraneos = true;
        Certificacion.P_Campos_Dinamicos = Ope_Pre_Constancias.Campo_No_Constancia + ", ";
        Certificacion.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Propietario_ID + ", ";
        Certificacion.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Cuenta_Predial_ID + ", ";
        Certificacion.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Estatus + ", ";
        Certificacion.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Documento_ID + ", ";
        Certificacion.P_Campos_Dinamicos += "(SELECT " + Cat_Pre_Tipos_Constancias.Tabla_Cat_Pre_Tipos_Constancias + "." + Cat_Pre_Tipos_Constancias.Campo_Nombre + " FROM " + Cat_Pre_Tipos_Constancias.Tabla_Cat_Pre_Tipos_Constancias + " WHERE " + Cat_Pre_Tipos_Constancias.Tabla_Cat_Pre_Tipos_Constancias + "." + Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID + "=" + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Documento_ID + ") AS DOCUMENTO, ";
        Certificacion.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Leyenda_Certificacion + ", ";
        Certificacion.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Observaciones + ", ";
        Certificacion.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Confronto + ", ";
        Certificacion.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Folio + ", ";
        Certificacion.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Fecha + ", ";
        Certificacion.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Proteccion_Pago + ", ";
        Certificacion.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Fecha_Vencimiento + ", ";
        Certificacion.P_Campos_Dinamicos += " (SELECT " + Cat_Empleados.Campo_Confronto + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_Empleado_ID + "=" + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Confronto + ") AS INICIALES,";
        Certificacion.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_No_Recibo;
        Certificacion.P_Filtros_Dinamicos = "FOLIO = '" + Certificacion.P_Folio + "'";
        return Certificacion.Consultar_Constancias();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Componentes
    ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 24/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private String Obtener_Dato_Consulta(String Campo, String Tabla, String Condiciones)
    {
        String Mi_SQL;
        String Dato_Consulta = "";

        try
        {
            Mi_SQL = "SELECT " + Campo;
            if (Tabla != "")
            {
                Mi_SQL += " FROM " + Tabla;
            }
            if (Condiciones != "")
            {
                Mi_SQL += " WHERE " + Condiciones;
            }

            OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Dr_Dato.Read())
            {
                if (Dr_Dato[0] != null)
                {
                    Dato_Consulta = Dr_Dato[0].ToString();
                }
                else
                {
                    Dato_Consulta = "";
                }
                Dr_Dato.Close();
            }
            else
            {
                Dato_Consulta = "";
            }
            Dr_Dato = null;
        }
        catch
        {
        }
        finally
        {
        }

        return Dato_Consulta;
    }
}
