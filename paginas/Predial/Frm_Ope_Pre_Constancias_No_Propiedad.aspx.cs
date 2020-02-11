using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Constancias.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio; 
using Presidencia.Catalogo_Contribuyentes.Negocio;
using Presidencia.Catalogo_Tipos_Constancias.Negocio;
using Presidencia.Colonias.Negocios;
using Presidencia.Catalogo_Calles.Negocio;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using Presidencia.Catalogo_Claves_Ingreso.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Empleados.Negocios;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using System.Text;
using System.IO;
using DocumentFormat.OpenXml.Packaging;

public partial class paginas_Predial_Frm_Ope_Pre_Constancias_No_Propiedad : System.Web.UI.Page
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
            Llenar_Tabla_Constancias_No_Propiedad(0);
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
    ///FECHA_CREO           : 30/Junio/2011
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
        Grid_Constancias_No_Propiedad.Enabled = Estatus;
        //Grid_Constancias_No_Propiedad.SelectedIndex = (-1);
        Txt_Nombre_Solicitante.Enabled = !Estatus;
        Txt_RFC.Enabled = !Estatus;
        Txt_Domicilio.Enabled = !Estatus;
        //Txt_Domicilio.Enabled = false;
        Txt_Observaciones.Enabled = !Estatus;
        Txt_Confronto.Enabled = false;// !Estatus;
        Txt_Fecha.Enabled = false;// !Estatus;
        Txt_Fecha_Vencimiento.Enabled = false;
        Txt_Folio.Enabled = false;// !Estatus;
        if (Btn_Modificar.AlternateText == "Actualizar")
        {
            Txt_No_Recibo_Pago.Enabled = true;// !Estatus;
            Cmb_Estatus.Enabled = true;
        }
        else
        {
            Txt_No_Recibo_Pago.Enabled = false;// !Estatus;
            Cmb_Estatus.Enabled = false;
        }
        Btn_Mostrar_Busqueda_Constancias_Avanzada.Enabled = !Estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Limpiar_Catálogo
    ///DESCRIPCIÓN          : Limpia los controles del Formulario
    ///PARAMETROS           :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 30/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Hdf_No_Constancia.Value = "";
        Cmb_Estatus.SelectedIndex = 0;
        Txt_Nombre_Solicitante.Text = "";
        Txt_RFC.Text = "";
        //Txt_Domicilio.Text = "";
        Txt_Observaciones.Text = "";
        Txt_Confronto.Text = "";
        Txt_Fecha.Text = "";
        Txt_Fecha_Vencimiento.Text = "";
        Txt_Folio.Text = "";
        Txt_No_Recibo_Pago.Text = "";
        Txt_Domicilio.Text = "";
        Hdf_Proteccion_Pago.Value = "";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Llenar_Tabla_Constancias_No_Propiedad
    ///DESCRIPCIÓN          : Llena la tabla de Constancias de No Propiedad con una consulta que puede o no tener Filtros.
    ///PARAMETROS           : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 30/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Constancias_No_Propiedad(int Pagina)
    {
        try
        {
            Cls_Ope_Pre_Constancias_Negocio Constancia_No_Propiedad = new Cls_Ope_Pre_Constancias_Negocio();
            //Constancia_No_Propiedad.P_Campos_Dinamicos = "(SELECT " + Cat_Pre_Contribuyentes.Campo_Nombre + " || " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " IN(SELECT " + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " WHERE " + Cat_Pre_Propietarios.Campo_Propietario_ID + " = " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Propietario_ID + ")) AS Nombre_Propietario, ";
            //Constancia_No_Propiedad.P_Campos_Dinamicos += "(SELECT " + Cat_Pre_Contribuyentes.Campo_RFC + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " IN(SELECT " + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " WHERE " + Cat_Pre_Propietarios.Campo_Propietario_ID + " = " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Propietario_ID + ")) AS RFC_Propietario, ";
            Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Solicitante + ", ";
            Constancia_No_Propiedad.P_Campos_Foraneos = true ;
            Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Solicitante_RFC + ", ";
            Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Folio + ", ";
            Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Domicilio + ", ";
            Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Fecha + ", ";
            Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Fecha_Vencimiento  + ", ";
            Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Estatus;
            Constancia_No_Propiedad.P_Tipo_Constancia_ID = "00002";
            Constancia_No_Propiedad.P_Filtros_Dinamicos = Ope_Pre_Constancias.Campo_Tipo_Constancia_ID + " = '00002' AND (";
            Constancia_No_Propiedad.P_Filtros_Dinamicos += Ope_Pre_Constancias.Campo_Folio + " LIKE '%" + Txt_Busqueda.Text.Trim().ToUpper() + "%' OR ";
            Constancia_No_Propiedad.P_Filtros_Dinamicos += Ope_Pre_Constancias.Campo_Solicitante  + " LIKE '%" + Txt_Busqueda.Text.Trim().ToUpper() + "%' OR ";
            Constancia_No_Propiedad.P_Filtros_Dinamicos += Ope_Pre_Constancias.Campo_Solicitante_RFC  + " LIKE '%" + Txt_Busqueda.Text.Trim().ToUpper() + "%')";
            Constancia_No_Propiedad.P_Ordenar_Dinamico = Ope_Pre_Constancias.Campo_Anio + " DESC, "+ Ope_Pre_Constancias.Campo_No_Constancia + " DESC";
            DataTable Tabla = Constancia_No_Propiedad.Consultar_Constancias();
            //DataView Vista = new DataView(Tabla);
            //String Expresion_De_Busqueda = string.Format("{0} '%{1}%'", Grid_Constancias_No_Propiedad.SortExpression, Txt_Busqueda.Text.Trim());
            //Vista.RowFilter = Ope_Pre_Constancias.Campo_Folio + " LIKE " + Expresion_De_Busqueda;
            Grid_Constancias_No_Propiedad.Columns[7].Visible = true;
            Grid_Constancias_No_Propiedad.DataSource = Tabla;
            Grid_Constancias_No_Propiedad.PageIndex = Pagina;
            Grid_Constancias_No_Propiedad.DataBind();
            Grid_Constancias_No_Propiedad.Columns[7].Visible = false;
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
    ///FECHA_CREO           : 30/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Txt_Nombre_Solicitante.Text.Trim().Equals(""))
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introduzca el Nombre del Solicitante.";
            Validacion = false;
        }
        //if (Txt_Domicilio.Text.Equals(""))
        //{
        //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //    Mensaje_Error = Mensaje_Error + "+ Introduzca el Domicilio del Solicitante.";
        //    Validacion = false;
        //}
        //if (Txt_RFC.Text.Equals(""))
        //{
        //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //    Mensaje_Error = Mensaje_Error + "+ Introduzca el RFC del Solicitante.";
        //    Validacion = false;
        //}

        if (Txt_Domicilio.Text.Trim().Equals(""))
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introduzca el Domicilio del Solicitante.";
            Validacion = false;
        }
        //if (Btn_Nuevo.AlternateText != "Dar de Alta" && Btn_Modificar.AlternateText == "Actualizar")
        //{
        //    if (Txt_Observaciones.Text.Equals(""))
        //    {
        //        if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //        Mensaje_Error = Mensaje_Error + "+ Introduzca las Observaciones.";
        //        Validacion = false;
        //    }
        //}
        //if (Cmb_Estatus.SelectedIndex <= 0)
        //{
        //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //    Mensaje_Error = Mensaje_Error + "+ Indique un Estatus.";
        //    Validacion = false;
        //}
        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }

    #endregion

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Constancias_No_Propiedad_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView de los Tipos_Constancias
    ///PARAMETROS:
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 30/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Constancias_No_Propiedad_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Constancias_No_Propiedad.SelectedIndex = (-1);
            Llenar_Tabla_Constancias_No_Propiedad(e.NewPageIndex);
            Limpiar_Catalogo();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Constancias_No_Propiedad_RowCommand
    ///DESCRIPCIÓN          : Evento RowCommand para procesas los diferentes botones de comando en el gridview
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Constancias_No_Propiedad_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Print":
                //Cls_Impresion Impresion = new Cls_Impresion();
                //Impresion.Crear_Pagina("HP LaserJet 9000 PCL 6", "Test");
                //Impresion.Printing("HP LaserJet 9000 PCL 6", "F:\\BackUp\\Desk\\reportes astaug abril.pdf");
                if (Grid_Constancias_No_Propiedad.Rows[Convert.ToInt32(e.CommandArgument)].Cells[6].Text.Trim() == "PAGADA")
                {
                    Limpiar_Catalogo();
                    Cls_Ope_Pre_Constancias_Negocio Constancia_No_Propiedad = new Cls_Ope_Pre_Constancias_Negocio();
                    DataTable Dt_Constancia_No_Propiedad;
                    String[] Proteccion_Pago;

                    Constancia_No_Propiedad.P_Campos_Foraneos = true;
                    Constancia_No_Propiedad.P_Campos_Dinamicos = Ope_Pre_Constancias.Campo_No_Constancia + ", ";
                    Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Propietario_ID + ", ";
                    Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Cuenta_Predial_ID + ", ";
                    Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Estatus + ", ";
                    Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Solicitante + ", ";
                    Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Solicitante_RFC + ", ";
                    Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Confronto + ", ";
                    Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Folio + ", ";
                    Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Domicilio + ", ";
                    Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Fecha + ", ";
                    Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Fecha_Vencimiento + ", ";
                    Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Proteccion_Pago + ", ";
                    Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_No_Recibo + ", ";
                    Constancia_No_Propiedad.P_Campos_Dinamicos += "(SELECT " + Cat_Empleados.Campo_Confronto + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_Empleado_ID + "=" + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Confronto + ") AS INICIALES, ";
                    Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Observaciones;
                    Constancia_No_Propiedad.P_Filtros_Dinamicos = "FOLIO = '" + Grid_Constancias_No_Propiedad.Rows[Convert.ToInt32(e.CommandArgument)].Cells[3].Text.Trim() + "'";
                    Dt_Constancia_No_Propiedad = Constancia_No_Propiedad.Consultar_Constancias();

                    if (Dt_Constancia_No_Propiedad != null)
                    {
                        Hdf_No_Constancia.Value = Dt_Constancia_No_Propiedad.Rows[0]["NO_CONSTANCIA"].ToString();
                        Hdf_Propietario_ID.Value = Dt_Constancia_No_Propiedad.Rows[0]["PROPIETARIO_ID"].ToString();
                        Hdf_Cuenta_Predial_ID.Value = Dt_Constancia_No_Propiedad.Rows[0]["CUENTA_PREDIAL_ID"].ToString();
                        Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Dt_Constancia_No_Propiedad.Rows[0]["ESTATUS"].ToString()));
                        Txt_Nombre_Solicitante.Text = Dt_Constancia_No_Propiedad.Rows[0]["SOLICITANTE"].ToString();
                        //Txt_Domicilio.Text = Dt_Constancia_No_Propiedad.Rows[0]["Domicilio_Propietario"].ToString();
                        Txt_RFC.Text = Dt_Constancia_No_Propiedad.Rows[0]["SOLICITANTE_RFC"].ToString();
                        Txt_Confronto.Text = Dt_Constancia_No_Propiedad.Rows[0]["INICIALES"].ToString();
                        Txt_Folio.Text = Dt_Constancia_No_Propiedad.Rows[0]["FOLIO"].ToString();
                        Txt_Fecha.Text = Dt_Constancia_No_Propiedad.Rows[0]["FECHA"].ToString().Substring(0, 10);
                        Txt_Fecha_Vencimiento.Text = Dt_Constancia_No_Propiedad.Rows[0]["FECHA_VENCIMIENTO"].ToString().Substring(0, 10);
                        Txt_No_Recibo_Pago.Text = Dt_Constancia_No_Propiedad.Rows[0]["NO_RECIBO"].ToString();
                        Txt_Observaciones.Text = Dt_Constancia_No_Propiedad.Rows[0]["OBSERVACIONES"].ToString();
                        Txt_Domicilio.Text = Dt_Constancia_No_Propiedad.Rows[0]["DOMICILIO"].ToString();
                        if (Dt_Constancia_No_Propiedad.Rows[0]["PROTECCION_PAGO"].ToString() != "")
                        {
                            Proteccion_Pago = Dt_Constancia_No_Propiedad.Rows[0]["PROTECCION_PAGO"].ToString().Split('/');
                            Txt_No_Recibo_Pago.Text = Proteccion_Pago[6];
                            Hdf_Proteccion_Pago.Value = Dt_Constancia_No_Propiedad.Rows[0]["PROTECCION_PAGO"].ToString();
                        }
                    }

                    Imprimir_Constancia();
                    Cls_Ope_Pre_Constancias_Negocio Constancias = new Cls_Ope_Pre_Constancias_Negocio();
                    Constancias.P_Folio = Grid_Constancias_No_Propiedad.Rows[Convert.ToInt32(e.CommandArgument)].Cells[3].Text.Trim();
                    Constancias.Incrementar_No_Impresiones_Constancia();
                    Constancias.P_Folio = Txt_Folio.Text;
                    Constancias.Constancia_Impresa();
                    Llenar_Tabla_Constancias_No_Propiedad(Grid_Constancias_No_Propiedad.PageIndex);
                    Limpiar_Catalogo();
                }
                break;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Constancias_No_Propiedad_SelectedIndexChanged
    ///DESCRIPCIÓN          : Obtiene los datos de un Constancia_No_Propiedad Seleccionado para mostrarlos a detalle
    ///PARAMETROS:     
    ///CREO                 : Antonio Benavides Guardado
    ///FECHA_CREO           : 30/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Constancias_No_Propiedad_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Constancias_No_Propiedad.SelectedIndex > (-1))
            {
                Limpiar_Catalogo();
                Cls_Ope_Pre_Constancias_Negocio Constancia_No_Propiedad = new Cls_Ope_Pre_Constancias_Negocio();
                DataTable Dt_Constancia_No_Propiedad;
                String[] Proteccion_Pago;

                Constancia_No_Propiedad.P_Campos_Foraneos = true;
                Constancia_No_Propiedad.P_Campos_Dinamicos = Ope_Pre_Constancias.Campo_No_Constancia + ", ";
                Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Propietario_ID + ", ";
                Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Cuenta_Predial_ID + ", ";
                Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Estatus + ", ";
                Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Solicitante + ", ";
                Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Solicitante_RFC + ", ";
                Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Confronto + ", ";
                Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Folio + ", ";
                Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Domicilio + ", ";
                Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Fecha + ", ";
                Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Fecha_Vencimiento + ", ";
                Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Proteccion_Pago + ", ";
                Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_No_Recibo + ", ";
                Constancia_No_Propiedad.P_Campos_Dinamicos += "(SELECT " + Cat_Empleados.Campo_Confronto + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_Empleado_ID + "=" + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Confronto + ") AS INICIALES, ";
                Constancia_No_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Observaciones;
                Constancia_No_Propiedad.P_Filtros_Dinamicos = "FOLIO = '" + Grid_Constancias_No_Propiedad.SelectedRow.Cells[3].Text.Trim() + "'";
                Dt_Constancia_No_Propiedad = Constancia_No_Propiedad.Consultar_Constancias();

                if (Dt_Constancia_No_Propiedad != null)
                {
                    Hdf_No_Constancia.Value = Dt_Constancia_No_Propiedad.Rows[0]["NO_CONSTANCIA"].ToString();
                    Hdf_Propietario_ID.Value = Dt_Constancia_No_Propiedad.Rows[0]["PROPIETARIO_ID"].ToString();
                    Hdf_Cuenta_Predial_ID.Value = Dt_Constancia_No_Propiedad.Rows[0]["CUENTA_PREDIAL_ID"].ToString();
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Dt_Constancia_No_Propiedad.Rows[0]["ESTATUS"].ToString()));
                    Txt_Nombre_Solicitante.Text = Dt_Constancia_No_Propiedad.Rows[0]["SOLICITANTE"].ToString();
                    //Txt_Domicilio.Text = Dt_Constancia_No_Propiedad.Rows[0]["Domicilio_Propietario"].ToString();
                    Txt_RFC.Text = Dt_Constancia_No_Propiedad.Rows[0]["SOLICITANTE_RFC"].ToString();
                    Txt_Confronto.Text = Dt_Constancia_No_Propiedad.Rows[0]["INICIALES"].ToString();
                    Txt_Folio.Text = Dt_Constancia_No_Propiedad.Rows[0]["FOLIO"].ToString();
                    Txt_Fecha.Text = Dt_Constancia_No_Propiedad.Rows[0]["FECHA"].ToString().Substring(0,10);
                    Txt_Fecha_Vencimiento .Text = Dt_Constancia_No_Propiedad.Rows[0]["FECHA_VENCIMIENTO"].ToString().Substring(0, 10);
                    Txt_No_Recibo_Pago.Text = Dt_Constancia_No_Propiedad.Rows[0]["NO_RECIBO"].ToString();
                    Txt_Observaciones.Text = Dt_Constancia_No_Propiedad.Rows[0]["OBSERVACIONES"].ToString();
                    Txt_Domicilio.Text = Dt_Constancia_No_Propiedad.Rows[0]["DOMICILIO"].ToString();
                    if (Dt_Constancia_No_Propiedad.Rows[0]["PROTECCION_PAGO"].ToString() != "")
                    {
                        Proteccion_Pago = Dt_Constancia_No_Propiedad.Rows[0]["PROTECCION_PAGO"].ToString().Split('/');
                        Txt_No_Recibo_Pago.Text = Proteccion_Pago[6];
                        Hdf_Proteccion_Pago.Value = Dt_Constancia_No_Propiedad.Rows[0]["PROTECCION_PAGO"].ToString();
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
    ///NOMBRE DE LA FUNCIÓN : Btn_Nuevo_Click
    ///DESCRIPCIÓN          : Deja los componentes listos para dar de Alta un nuevo Constancia_No_Propiedad
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 30/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Pre_Constancias_Negocio Constancias_Negocio = new Cls_Ope_Pre_Constancias_Negocio();
        Cls_Ope_Pre_Parametros_Negocio Dias_Habiles = new Cls_Ope_Pre_Parametros_Negocio();
        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                Configuracion_Formulario(false);
                Limpiar_Catalogo();
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.Visible = false;
                DataTable Dt_Ayudante;
                Cls_Cat_Empleados_Negocios Empleados = new Cls_Cat_Empleados_Negocios();
                Empleados.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
                Dt_Ayudante = Empleados.Consulta_Datos_Empleado();
                Txt_Confronto.Text = Dt_Ayudante.Rows[0][Cat_Empleados.Campo_Confronto].ToString();
                //Txt_Confronto.Text = Cls_Sessiones.Nombre_Empleado.ToUpper();
                Txt_Fecha.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);
                String Dias = Convert.ToString(Dias_Habiles.Consultar_Dias_Vencimiento());
                Txt_Fecha_Vencimiento.Text = String.Format("{0:dd/MMM/yyyy}", Constancias_Negocio.Calcular_Fecha(Txt_Fecha.Text.Trim(), Dias));
                //Cmb_Estatus.Enabled = false;
                Cmb_Estatus.SelectedIndex = 0;
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Ope_Pre_Constancias_Negocio Constancia_No_Propiedad = new Cls_Ope_Pre_Constancias_Negocio();
                    Constancia_No_Propiedad.P_Tipo_Constancia_ID = "00002";
                    //Constancia_No_Propiedad.P_Propietario_ID = Hdf_Propietario_ID.Value;
                    Constancia_No_Propiedad.P_Solicitante=Txt_Nombre_Solicitante.Text.ToUpper();
                    Constancia_No_Propiedad.P_Solicitante_RFC = Txt_RFC.Text.ToUpper();
                    Constancia_No_Propiedad.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                    Constancia_No_Propiedad.P_Confronto = Cls_Sessiones.Empleado_ID.ToUpper();
                    //Constancia_No_Propiedad.P_Folio = Txt_Folio.Text.Trim();
                    Constancia_No_Propiedad.P_Fecha = Convert.ToDateTime(Txt_Fecha.Text.Trim());
                    Constancia_No_Propiedad.P_Fecha_Vencimiento = Convert.ToDateTime(Txt_Fecha_Vencimiento  .Text.Trim());
                    Constancia_No_Propiedad.P_Periodo_Año = DateTime.Now.Year;
                    //Constancia_No_Propiedad.P_No_Recibo = Txt_No_Recibo_Pago.Text.Trim();
                    Constancia_No_Propiedad.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                    Constancia_No_Propiedad.P_Domicilio = Txt_Domicilio.Text.ToString().ToUpper();
                    Constancia_No_Propiedad.P_Observaciones = Txt_Observaciones.Text.Trim().ToUpper();
                    Constancia_No_Propiedad.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                    if (Constancia_No_Propiedad.Alta_Constancia())
                    {
                        Insertar_Pasivo(Constancia_No_Propiedad.P_Folio);
                        Txt_Folio.Text = Constancia_No_Propiedad.P_Folio;
                        Configuracion_Formulario(true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Constancias de No Propiedad", "alert('Alta de Constancia de No Propiedad Exitosa');", true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Modificar.Visible = true;
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        //Consultar el precio de la constancia de no propiedad...
                        Cls_Cat_Pre_Tipos_Constancias_Negocio Tipos_Constancias = new Cls_Cat_Pre_Tipos_Constancias_Negocio();
                        DataTable Dt_Tipos_Constancias;

                        Tipos_Constancias.P_Campos_Dinamicos = Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID + ", " + Cat_Pre_Tipos_Constancias.Campo_Costo;
                        Tipos_Constancias.P_Filtros_Dinamicos = Cat_Pre_Tipos_Constancias.Campo_Clave + "='CNP'";

                        Dt_Tipos_Constancias = Tipos_Constancias.Consultar_Tipos_Constancias();

                        Imprimir_Reporte_Folio(Crear_Ds_Constancias(Constancia_No_Propiedad.P_Folio, Dt_Tipos_Constancias.Rows[0][Cat_Pre_Tipos_Constancias.Campo_Costo].ToString()), "Rpt_Ope_Pre_Folio_Constancias.rpt", "Folio_Constancia_Propiedad");
                        Limpiar_Catalogo();
                        Llenar_Tabla_Constancias_No_Propiedad(Grid_Constancias_No_Propiedad.PageIndex);
                        Grid_Constancias_No_Propiedad.SelectedIndex = -1;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Constancias de No Propiedad", "alert('Alta de Constancia de No Propiedad No fue Exitosa');", true);
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

            Tipos_Constancias.P_Campos_Dinamicos = Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID+", "+Cat_Pre_Tipos_Constancias.Campo_Costo;
            Tipos_Constancias.P_Filtros_Dinamicos = Cat_Pre_Tipos_Constancias.Campo_Clave + "='CNP'";

            Calculo_Impuesto_Traslado.P_Referencia = Referencia;
            Calculo_Impuesto_Traslado.Eliminar_Referencias_Pasivo();
            
            Dt_Tipos_Constancias = Tipos_Constancias.Consultar_Tipos_Constancias();

            if (Dt_Tipos_Constancias.Rows.Count > 0 && Convert.ToDouble(Dt_Tipos_Constancias.Rows[0][Cat_Pre_Tipos_Constancias.Campo_Costo]) != 0)
            {
                Claves_Ingreso.P_Documento_ID = Dt_Tipos_Constancias.Rows[0][Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID].ToString();
                Dt_Clave = Claves_Ingreso.Consultar_Clave_Ingreso();
                if (Dt_Clave.Rows.Count > 0)
                {
                    if (Dt_Tipos_Constancias.Rows.Count > 0)
                    {
                        if (Dt_Tipos_Constancias.Rows[0][Cat_Pre_Tipos_Constancias.Campo_Costo].ToString() != "")
                        {
                            if (Convert.ToDouble(Dt_Tipos_Constancias.Rows[0][Cat_Pre_Tipos_Constancias.Campo_Costo]) != 0)
                            {
                                Calculo_Impuesto_Traslado.P_Referencia = Referencia;
                                Calculo_Impuesto_Traslado.P_Estatus = "POR PAGAR";
                                Calculo_Impuesto_Traslado.P_Descripcion = "CONSTANCIA DE NO PROPIEDAD";
                                Calculo_Impuesto_Traslado.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                                Calculo_Impuesto_Traslado.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                                Calculo_Impuesto_Traslado.P_Monto_Total_Pagar = Dt_Tipos_Constancias.Rows[0][Cat_Pre_Tipos_Constancias.Campo_Costo].ToString();
                                Calculo_Impuesto_Traslado.P_Fecha_Tramite = DateTime.Now.ToString("dd/MMM/yyyy");
                                Calculo_Impuesto_Traslado.P_Contribuyente = Txt_Nombre_Solicitante.Text.ToUpper();
                                Calculo_Impuesto_Traslado.P_Fecha_Vencimiento_Pasivo = DateTime.Now.ToString("dd/MMM/yyyy");
                                Cls_Ope_Pre_Constancias_Negocio Contribuyente = new Cls_Ope_Pre_Constancias_Negocio();
                                Contribuyente.P_Solicitante = Txt_Nombre_Solicitante.Text.ToUpper();
                                Contribuyente.Alta_Pasivo(Calculo_Impuesto_Traslado);
                            }
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
    ///NOMBRE DE LA FUNCIÓN : Btn_Modificar_Click
    ///DESCRIPCIÓN          : Deja los componentes listos para hacer la modificacion de un Constancia_No_Propiedad.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 30/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        if (Grid_Constancias_No_Propiedad.SelectedIndex > -1)
        {
            if (Grid_Constancias_No_Propiedad.SelectedRow.Cells[6].Text != "PAGADA")
            {
                try
                {
                    if (Btn_Modificar.AlternateText.Equals("Modificar"))
                    {
                        if (Grid_Constancias_No_Propiedad.Rows.Count > 0 && Grid_Constancias_No_Propiedad.SelectedIndex > (-1))
                        {
                            Btn_Modificar.AlternateText = "Actualizar";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                            Btn_Salir.AlternateText = "Cancelar";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                            Btn_Nuevo.Visible = false;
                            Configuracion_Formulario(false);
                            Txt_No_Recibo_Pago.Enabled = false;
                            //Img_Calendario.Enabled = false;
                            Btn_Mostrar_Busqueda_Constancias_Avanzada.Enabled = false;
                            Cmb_Estatus.Items.Remove("PAGADA");
                        }
                    }
                    else
                    {
                        if (Validar_Componentes())
                        {
                            Cls_Ope_Pre_Constancias_Negocio Constancia_No_Propiedad = new Cls_Ope_Pre_Constancias_Negocio();
                            Constancia_No_Propiedad.P_No_Constancia = Hdf_No_Constancia.Value;
                            Constancia_No_Propiedad.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                            Constancia_No_Propiedad.P_Propietario_ID = Hdf_Propietario_ID.Value;
                            Constancia_No_Propiedad.P_Solicitante = Txt_Nombre_Solicitante.Text.ToUpper();
                            Constancia_No_Propiedad.P_Solicitante_RFC = Txt_RFC.Text.ToUpper();
                            Constancia_No_Propiedad.P_Confronto = Cls_Sessiones.Empleado_ID.ToUpper();
                            Constancia_No_Propiedad.P_Folio = Txt_Folio.Text.ToUpper().Trim();
                            //Constancia_No_Propiedad.P_Fecha = Convert.ToDateTime(Txt_Fecha.Text.Trim());
                            Constancia_No_Propiedad.P_No_Recibo = Txt_No_Recibo_Pago.Text.Trim();
                            Constancia_No_Propiedad.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                            Constancia_No_Propiedad.P_Domicilio = Txt_Domicilio.Text.ToUpper();
                            Constancia_No_Propiedad.P_Observaciones = Txt_Observaciones.Text.Trim().ToUpper();
                            Constancia_No_Propiedad.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                            if (Constancia_No_Propiedad.Modificar_Constancia())
                            {
                                Insertar_Pasivo(Constancia_No_Propiedad.P_Folio);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Constancias de No Propiedad", "alert('Actualización de Constancia de No Propiedad Exitosa');", true);
                                Btn_Modificar.AlternateText = "Modificar";
                                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                                Btn_Modificar.Visible = true;
                                Btn_Nuevo.Visible = true;
                                Btn_Salir.AlternateText = "Salir";
                                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                                //Consultar el precio de la constancia de no propiedad...
                                Cls_Cat_Pre_Tipos_Constancias_Negocio Tipos_Constancias = new Cls_Cat_Pre_Tipos_Constancias_Negocio();
                                DataTable Dt_Tipos_Constancias;

                                Tipos_Constancias.P_Campos_Dinamicos = Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID + ", " + Cat_Pre_Tipos_Constancias.Campo_Costo;
                                Tipos_Constancias.P_Filtros_Dinamicos = Cat_Pre_Tipos_Constancias.Campo_Clave + "='CNP'";

                                Dt_Tipos_Constancias = Tipos_Constancias.Consultar_Tipos_Constancias();

                                Imprimir_Reporte_Folio(Crear_Ds_Constancias(Txt_Folio.Text.Trim(), Dt_Tipos_Constancias.Rows[0][Cat_Pre_Tipos_Constancias.Campo_Costo].ToString()), "Rpt_Ope_Pre_Folio_Constancias.rpt", "Folio_Constancia_Propiedad");
                                Limpiar_Catalogo();
                                Llenar_Tabla_Constancias_No_Propiedad(Grid_Constancias_No_Propiedad.PageIndex);
                                Configuracion_Formulario(true);
                                Cmb_Estatus.Items.Insert(1, new ListItem("PAGADA", "PAGADA"));
                                Grid_Constancias_No_Propiedad.SelectedIndex = -1;
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Constancias de No Propiedad", "alert('Actualización de Constancia de No Propiedad No fue Exitosa');", true);
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
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Constancias de No Propiedad", "alert('La constancia de No Propiedad se encuentra pagada.');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Constancias de No Propiedad", "alert('Selecciona el Registro que quieres Modificar.');", true);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Constancia_No_Propiedad_Click
    ///DESCRIPCIÓN          : Llena la Tabla con la opcion buscada
    ///PARAMETROS          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 30/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Limpiar_Catalogo();
            Grid_Constancias_No_Propiedad.SelectedIndex = (-1);
            Llenar_Tabla_Constancias_No_Propiedad(0);
            if (Grid_Constancias_No_Propiedad.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
            {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda de \"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
                Lbl_Mensaje_Error.Text = "(Se cargaron todos los Constancias de No Propiedad encontradas)";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Busqueda.Text = "";
                Llenar_Tabla_Constancias_No_Propiedad(0);
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
    //        Imprimir_Reporte(Crear_Ds_Constancias(Convert.ToInt32()), "Rpt_Pre_Constancias.rpt", "Constancia de No Propiedad");
    //    }
    //}

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN : Btn_Eliminar_Click
    /////DESCRIPCIÓN          : Elimina un Constancia_No_Propiedad de la Base de Datos
    /////PARAMETROS          :     
    /////CREO                 : Antonio Salvador Benavides Guardado
    /////FECHA_CREO           : 30/Junio/2011
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Btn_Eliminar_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (Grid_Constancias_No_Propiedad.Rows.Count > 0 && Grid_Constancias_No_Propiedad.SelectedIndex > (-1))
    //        {
    //            Cls_Ope_Pre_Constancias_Negocio Constancia_No_Propiedad = new Cls_Ope_Pre_Constancias_Negocio();
    //            Constancia_No_Propiedad.P_Folio = Grid_Constancias_No_Propiedad.SelectedRow.Cells[3].Text;
    //            if (Constancia_No_Propiedad.Eliminar_Constancia_No_Propiedad())
    //            {
    //                Grid_Constancias_No_Propiedad.SelectedIndex = (-1);
    //                Llenar_Tabla_Constancias_No_Propiedad(Grid_Constancias_No_Propiedad.PageIndex);
    //                ScriptManager.RegisterStartupScript(this, this.GetType(), "Constancias de No Propiedad", "alert('Constancia de No Propiedad fue Eliminada Exitosamente');", true);
    //                Limpiar_Catalogo();
    //            }
    //            else
    //            {
    //                ScriptManager.RegisterStartupScript(this, this.GetType(), "Constancias de No Propiedad", "alert('La Constancia de No Propiedad No fue Eliminada');", true);
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
    ///FECHA_CREO           : 30/Junio/2011
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
            if (Btn_Modificar.AlternateText == "Actualizar")
            {
                Cmb_Estatus.Items.Insert(1, new ListItem("PAGADA", "PAGADA"));
            }
            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Configuracion_Formulario(true);
            Limpiar_Catalogo();
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Grid_Constancias_No_Propiedad.SelectedIndex = -1;
        }
    }

    protected void Consultar_Datos_Contribuyente_Constancia()
    {
        Boolean Filtros = false;
        DataTable Dt_Contribuyentes;
        Cls_Cat_Pre_Contribuyentes_Negocio Contribuyentes = new Cls_Cat_Pre_Contribuyentes_Negocio();
        //Consulta los datos del Contribuyente
        Contribuyentes.P_Campos_Dinamicos = Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + ", ";
        Contribuyentes.P_Campos_Dinamicos += Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Nombre + " Nombre_Completo, ";
        Contribuyentes.P_Campos_Dinamicos += Cat_Pre_Contribuyentes.Campo_RFC;
        if (Txt_Nombre_Solicitante.Text.Trim() != "")
        {
            if (Contribuyentes.P_Filtros_Dinamicos != "" && Contribuyentes.P_Filtros_Dinamicos != null)
            {
                Contribuyentes.P_Filtros_Dinamicos += " AND ";
            }
            Contribuyentes.P_Filtros_Dinamicos += Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Nombre + " LIKE '%" + Txt_Nombre_Solicitante.Text.Trim().ToUpper() + "%'";
            Filtros = true;
        }
        if (Txt_RFC.Text.Trim() != "")
        {
            if (Contribuyentes.P_Filtros_Dinamicos != "" && Contribuyentes.P_Filtros_Dinamicos != null)
            {
                Contribuyentes.P_Filtros_Dinamicos += " AND ";
            }
            Contribuyentes.P_Filtros_Dinamicos += Cat_Pre_Contribuyentes.Campo_RFC + " LIKE '%" + Txt_RFC.Text.Trim().ToUpper() + "%'";
            Filtros = true;
        }
        //if (Txt_Domicilio.Text.Trim() != "")
        //{
        //    if (Contribuyentes.P_Filtros_Dinamicos != "" && Contribuyentes.P_Filtros_Dinamicos != null)
        //    {
        //        Contribuyentes.P_Filtros_Dinamicos += " AND ";
        //    }
        //    Contribuyentes.P_Filtros_Dinamicos += Cat_Pre_Contribuyentes.Campo_Domicilio + " LIKE '%" + Txt_Domicilio.Text.Trim().ToUpper() + "%'";
        //    Filtros = true;
        //}

        if (Filtros)
        {

            Dt_Contribuyentes = Contribuyentes.Consultar_Contribuyentes_Popup();
            if (Dt_Contribuyentes.Rows.Count > 0)
            {
                //DataTable Dt_Propietarios;
                //Cls_Cat_Pre_Propietarios_Negocio Propietarios = new Cls_Cat_Pre_Propietarios_Negocio();
                ////Consulta los Propietarios/Copropietarios de la Cuenta Predial
                //Propietarios.P_Campos_Dinamicos = Cat_Pre_Propietarios.Campo_Propietario_ID + ", " + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID;
                //Propietarios.P_Contribuyente_ID = Dt_Contribuyentes.Rows[0][Cat_Pre_Propietarios.Campo_Contribuyente_ID].ToString();
                //Dt_Propietarios = Propietarios.Consultar_Propietario();
                //if (Dt_Propietarios.Rows.Count > 0)
                //{
                //    Txt_Nombre_Solicitante.Text = Dt_Contribuyentes.Rows[0]["Nombre_Completo"].ToString();
                //    Txt_RFC.Text = Dt_Contribuyentes.Rows[0][Cat_Pre_Contribuyentes.Campo_RFC].ToString();
                //    Txt_Domicilio.Text = Dt_Contribuyentes.Rows[0][Cat_Pre_Contribuyentes.Campo_Domicilio].ToString();
                //    Hdf_Propietario_ID.Value = Dt_Propietarios.Rows[0][Cat_Pre_Propietarios.Campo_Propietario_ID].ToString();
                //    Hdf_Cuenta_Predial_ID.Value = Dt_Propietarios.Rows[0][Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID].ToString();
                //}
            }
        }
        else
        {
            Txt_Nombre_Solicitante.Text = "";
            Txt_RFC.Text = "";
            //Txt_Domicilio.Text = "";
            Hdf_Propietario_ID.Value = "";
        }

    }

    #endregion

    #region Modal PopUp's

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN      : Btn_Cerrar_Busqueda_Contribuyentes_Click
    /// 	DESCRIPCIÓN         : Ocultar el modal popup Busqueda de 
    /// 	PARÁMETROS:
    /// 	CREO                : Antonio Salvador Benavides Guardado
    /// 	FECHA_CREO          : 15/Julio/2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Cerrar_Busqueda_Contribuyentes_Click(object sender, ImageClickEventArgs e)
    {
        if (Txt_Busqueda_Propietatio.Text.Trim() != "")
        {
            Txt_Nombre_Solicitante.Text = Txt_Busqueda_Propietatio.Text.ToUpper();
        }
        if (Txt_Busqueda_RFC.Text.Trim() != "")
        {
            Txt_RFC.Text = Txt_Busqueda_RFC.Text.ToUpper();
        }
        if (Txt_Busqueda_Domicilio.Text.Trim() != "")
        {
            Txt_Domicilio.Text = Txt_Busqueda_Domicilio.Text.ToUpper();
        }
        Mpe_Busqueda_Contribuyentes.Hide();
        Grid_Contribuyentes.DataSource = null;
        Grid_Contribuyentes.DataBind();
        Txt_Busqueda_Contribuyente_ID.Text = "";
        Txt_Busqueda_Domicilio.Text = "";
        Txt_Busqueda_Propietatio.Text = "";
        Txt_Busqueda_RFC.Text = "";
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN      : Btn_Limpiar_Busqueda_Contribuyentes_Click
    /// 	DESCRIPCIÓN         : Limpia los controles de la búsqeuda avanzada
    /// 	PARÁMETROS:
    /// 	CREO                : Antonio Salvador Benavides Guardado
    /// 	FECHA_CREO          : 15/Julio/2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Limpiar_Busqueda_Contribuyentes_Click(object sender, ImageClickEventArgs e)
    {
        Txt_Busqueda_Contribuyente_ID.Text = "";
        //Cmb_Busqueda_Estatus.SelectedIndex = -1;
        Txt_Busqueda_Propietatio.Text = "";
        Txt_Busqueda_RFC.Text = "";
        Txt_Busqueda_Domicilio.Text = "";
        Grid_Contribuyentes.DataSource = null;
        Grid_Contribuyentes.DataBind();
        Mpe_Busqueda_Contribuyentes.Show();
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Btn_Busqueda_Contribuyentes_Click
    /// DESCRIPCION             : Ejecuta la búsqueda de mediante el modal popup 
    /// CREO                    : Antonio Salvador Benavides Guardado
    /// FECHA_CREO              : 15/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Busqueda_Contribuyentes_Click(object sender, EventArgs e)
    {
        Consultar_Contribuyentes(0);
    }

    private void Consultar_Contribuyentes(int Pagina)
    {
        try
        {
            DataTable Dt_Contribuyentes;
            Cls_Cat_Pre_Contribuyentes_Negocio Contribuyentes = new Cls_Cat_Pre_Contribuyentes_Negocio();
            if (Txt_Busqueda_Contribuyente_ID.Text.Trim() != ""
                //|| Cmb_Busqueda_Estatus.SelectedIndex > 0
            || Txt_Busqueda_Propietatio.Text.Trim() != ""
            || Txt_Busqueda_RFC.Text.Trim() != ""
            || Txt_Busqueda_Domicilio.Text.Trim() != "")
            {
                //Consulta la Cuenta Predial
                Contribuyentes.P_Campos_Dinamicos = Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + ", ";
                Contribuyentes.P_Campos_Dinamicos += "(SELECT " + Cat_Pre_Propietarios.Campo_Propietario_ID + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " WHERE " + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " = " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + ") AS " + Cat_Pre_Propietarios.Campo_Propietario_ID + ", ";
                Contribuyentes.P_Campos_Dinamicos += Cat_Pre_Contribuyentes.Campo_Nombre + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " Nombre, ";
                Contribuyentes.P_Campos_Dinamicos += Cat_Pre_Contribuyentes.Campo_Estatus + ", ";
                Contribuyentes.P_Campos_Dinamicos += Cat_Pre_Contribuyentes.Campo_RFC;
                Contribuyentes.P_Filtros_Dinamicos = "";
                if (Txt_Busqueda_Contribuyente_ID.Text.Trim() != "")
                {
                    if (Contribuyentes.P_Filtros_Dinamicos.Trim() != "")
                    {
                        Contribuyentes.P_Filtros_Dinamicos += " AND ";
                    }
                    Contribuyentes.P_Filtros_Dinamicos += "C." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " LIKE '%" + Txt_Busqueda_Contribuyente_ID.Text.Trim() + "%'";
                }
                //if (Cmb_Busqueda_Estatus.SelectedIndex > 0)
                //{
                //    if (Contribuyentes.P_Filtros_Dinamicos.Trim() != "")
                //    {
                //        Contribuyentes.P_Filtros_Dinamicos += " AND ";
                //    }
                //    Contribuyentes.P_Filtros_Dinamicos += Cat_Pre_Contribuyentes.Campo_Estatus + " = '" + Cmb_Busqueda_Estatus.SelectedValue + "'";
                //}
                if (Txt_Busqueda_Propietatio.Text.Trim() != "")
                {
                    if (Contribuyentes.P_Filtros_Dinamicos.Trim() != "")
                    {
                        Contribuyentes.P_Filtros_Dinamicos += " AND ";
                    }
                    Contribuyentes.P_Filtros_Dinamicos += "(C." + Cat_Pre_Contribuyentes.Campo_Nombre + " LIKE '%" + Txt_Busqueda_Propietatio.Text.Trim().ToUpper() + "%'";
                    Contribuyentes.P_Filtros_Dinamicos += " OR C." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " LIKE '%" + Txt_Busqueda_Propietatio.Text.Trim().ToUpper() + "%'";
                    Contribuyentes.P_Filtros_Dinamicos += " OR C." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " LIKE '%" + Txt_Busqueda_Propietatio.Text.Trim().ToUpper() + "%'";
                    Contribuyentes.P_Filtros_Dinamicos += " OR C." + Cat_Pre_Contribuyentes.Campo_Nombre + " || ' ' || C." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || C." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " LIKE '%" + Txt_Busqueda_Propietatio.Text.Trim().ToUpper() + "%'";
                    Contribuyentes.P_Filtros_Dinamicos += " OR C." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || C." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || C." + Cat_Pre_Contribuyentes.Campo_Nombre + " LIKE '%" + Txt_Busqueda_Propietatio.Text.Trim().ToUpper() + "%')";
                }
                if (Txt_Busqueda_RFC.Text.Trim() != "")
                {
                    if (Contribuyentes.P_Filtros_Dinamicos.Trim() != "")
                    {
                        Contribuyentes.P_Filtros_Dinamicos += " AND ";
                    }
                    Contribuyentes.P_Filtros_Dinamicos += Cat_Pre_Contribuyentes.Campo_RFC + " LIKE '%" + Txt_Busqueda_RFC.Text.Trim().ToUpper() + "%'";
                }
                if (Txt_Busqueda_Domicilio.Text.Trim() != "")
                {
                    if (Contribuyentes.P_Filtros_Dinamicos.Trim() != "")
                    {
                        Contribuyentes.P_Filtros_Dinamicos += " AND (";
                    }
                    else
                    {
                        Contribuyentes.P_Filtros_Dinamicos += "(";
                    }
                    Contribuyentes.P_Filtros_Dinamicos += "CA." + Cat_Pre_Calles.Campo_Nombre + " LIKE '%" + Txt_Busqueda_Domicilio.Text.Trim().ToUpper() + "%' OR ";
                    Contribuyentes.P_Filtros_Dinamicos += "CO." + Cat_Ate_Colonias.Campo_Nombre + " LIKE '%" + Txt_Busqueda_Domicilio.Text.Trim().ToUpper() + "%' OR ";
                    Contribuyentes.P_Filtros_Dinamicos += "CU." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + " LIKE '%" + Txt_Busqueda_Domicilio.Text.Trim().ToUpper() + "%' OR ";
                    Contribuyentes.P_Filtros_Dinamicos += "CU." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + " LIKE '%" + Txt_Busqueda_Domicilio.Text.Trim().ToUpper() + "%' ) ";
                }
                if (Contribuyentes.P_Filtros_Dinamicos.Trim() != "")
                {
                    Contribuyentes.P_Filtros_Dinamicos += " AND " + "C." + Cat_Pre_Contribuyentes.Campo_Estatus + "='VIGENTE' AND P."+Cat_Pre_Propietarios.Campo_Tipo+" IN ('PROPIETARIO','POSEEDOR','COPROPIETARIO') ";
                }
                else
                {
                    Contribuyentes.P_Filtros_Dinamicos += "C." + Cat_Pre_Contribuyentes.Campo_Estatus + "='VIGENTE' AND P." + Cat_Pre_Propietarios.Campo_Tipo + " IN ('PROPIETARIO','POSEEDOR','COPROPIETARIO') ";
                }
                Dt_Contribuyentes = Contribuyentes.Consultar_Contribuyentes_Popup();
                Grid_Contribuyentes.DataSource = Dt_Contribuyentes;
                Grid_Contribuyentes.PageIndex = Pagina;
                Grid_Contribuyentes.DataBind();
                if (Dt_Contribuyentes.Rows.Count < 1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Constancias de No Propiedad", "alert('No se encontró el contribuyente.');", true);
                }
            }
            else
            {
                Grid_Contribuyentes.DataSource = null;
                Grid_Contribuyentes.PageIndex = 0;
                Grid_Contribuyentes.DataBind();
            }
            Mpe_Busqueda_Contribuyentes.Show();
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN      : Grid_Contribuyentes_PageIndexChanging
    /// 	DESCRIPCIÓN         : Maneja el Evento de Cambio de Página del Grid de 
    /// 	PARÁMETROS          :
    /// 	CREO                : Antonio Salvador Benavides Guardado
    /// 	FECHA_CREO          : 15/Julio/2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Contribuyentes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Consultar_Contribuyentes(e.NewPageIndex);
            Mpe_Busqueda_Contribuyentes.Show();
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN      : Grid_Contribuyentes_SelectedIndexChanged
    /// 	DESCRIPCIÓN         : Maneja el Evento de Cambio de Selección del Grid 
    /// 	PARÁMETROS:
    /// 	CREO                : Antonio Salvador Benavides Guardado
    /// 	FECHA_CREO          : 15/Julio/2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Contribuyentes_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Txt_Busqueda_Contribuyente_ID.Text = Grid_Contribuyentes.SelectedRow.Cells[1].Text.Replace("&nbsp;", "");
            //if (Grid_Contribuyentes.SelectedRow.Cells[5].Text.Trim().Replace("&nbsp;", "") != "")
            //{
            //    Cmb_Busqueda_Estatus.SelectedValue = Grid_Contribuyentes.SelectedRow.Cells[5].Text;
            //}
            //else
            //{
            //    Cmb_Busqueda_Estatus.SelectedIndex = -1;
            //}
            //Txt_Busqueda_Propietatio.Text = Grid_Contribuyentes.SelectedRow.Cells[2].Text.Replace("&nbsp;", "");
            //Txt_Busqueda_RFC.Text = Grid_Contribuyentes.SelectedRow.Cells[3].Text.Replace("&nbsp;", "");
            ////Txt_Busqueda_Domicilio.Text = Grid_Contribuyentes.SelectedRow.Cells[4].Text.Replace("&nbsp;", "");

            Hdf_Propietario_ID.Value = Grid_Contribuyentes.SelectedRow.Cells[2].Text.Replace("&nbsp;", "");
            Txt_Nombre_Solicitante.Text = Grid_Contribuyentes.SelectedRow.Cells[3].Text.Replace("&nbsp;", "");
            Txt_RFC.Text = Grid_Contribuyentes.SelectedRow.Cells[4].Text.Replace("&nbsp;", "");
            //Txt_Domicilio.Text = Txt_Busqueda_Domicilio.Text.ToUpper();
            Consultar_Datos_Contribuyente_Constancia();
            Mpe_Busqueda_Contribuyentes.Hide();
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
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
    private Boolean Imprimir_Reporte(DataSet Ds_Constancias, String Nombre_Reporte, String Nombre_Archivo, int Fila)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);
        Boolean Impresion_Correcta = false;
        StringBuilder Domicilio = new StringBuilder();
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


                try
                {
                    //******** PASAMOS EL DOMICILIO COMO PARÁMETRO AL REPORTE ********
                    Domicilio.Append(Grid_Constancias_No_Propiedad.Rows[Convert.ToInt32(Fila)].Cells[7].Text);


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

                Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_PDF);
                Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
                Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
                Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
                Reporte.Export(Export_Options);

                try
                {
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
    ///NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Imprimir_Reporte_Folio(DataSet Ds_Convenios, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);
        try
        {
            Reporte.Load(File_Path);
            Reporte.SetDataSource(Ds_Convenios);
        }
        catch
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte para su impresión";
        }

        String Archivo_PDF = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar    
        try
        {
            ExportOptions Export_Options = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
            Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_PDF);
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Export_Options);
        }
        catch //(Exception Ex)
        {
            //Lbl_Mensaje_Error.Visible = true;
            //Lbl_Mensaje_Error.Text = "No se pudo exportar el reporte";
        }

        try
        {
            Mostrar_Reporte(Archivo_PDF, "PDF");
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
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
    private DataSet Crear_Ds_Constancias(int Indice_Fila)
    {
        Ds_Pre_Constancias Ds_Constancias = new Ds_Pre_Constancias();
        DataRow Dr_Constancias;

        foreach (DataTable Dt_Constancias in Ds_Constancias.Tables)
        {
            if (Dt_Constancias.TableName == "Dt_Constancias_No_Propiedad")
            {
                //Inserta los datos de la Constancia de No Propiedad en la Tabla
                Dr_Constancias = Dt_Constancias.NewRow();
                Dr_Constancias["Solicitante"] = Txt_Nombre_Solicitante.Text;
                Dr_Constancias["RFC"] = Txt_RFC.Text;
                Dr_Constancias["Folio"] = Txt_Folio.Text;
                Dr_Constancias["Fecha"] = Txt_Fecha.Text;
                Dr_Constancias["Domicilio"] = Txt_Domicilio.Text;
                Dr_Constancias["Iniciales"] = Txt_Confronto.Text;
                Dr_Constancias["Mes"] = DateTime.Now.ToString("MMMM").ToUpper();
                Dr_Constancias["Proteccion_Pago"] = Hdf_Proteccion_Pago.Value;
                Dt_Constancias.Rows.Add(Dr_Constancias);
            }
        }

        return Ds_Constancias;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Constancias_Folio
    ///DESCRIPCIÓN          : Crea un DataTable con las columnas y datos de la Constancia de Propiedad Seleccionada en el GridView
    ///PARAMETROS: 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 29/Noviembre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Constancias(String Folio, String Total_Pagar)
    {
        Ds_Ope_Pre_Constancias_Folio Ds_Constancias = new Ds_Ope_Pre_Constancias_Folio();
        DataRow Dr_Constancias;

        foreach (DataTable Dt_Constancias in Ds_Constancias.Tables)
        {
            if (Dt_Constancias.TableName == "Dt_Constancia")
            {
                //Inserta los datos de la Certificación en la Tabla
                Dr_Constancias = Dt_Constancias.NewRow();
                Dr_Constancias["Cuenta_predial"] = "";
                Dr_Constancias["Tipo"] = "Constancia de no propiedad";
                Dr_Constancias["Propietario"] = Txt_Nombre_Solicitante.Text.ToUpper();
                Dr_Constancias["Ubicacion"] = Txt_Domicilio.Text.ToUpper();
                Dr_Constancias["Folio"] = Folio;
                Dr_Constancias["Total_Pagar"] = "$" + Convert.ToDouble(Total_Pagar).ToString("#,###,###,##0.00");
                Dr_Constancias["Fecha"] = Convert.ToDateTime(Txt_Fecha.Text).ToString("dd/MMM/yyyy").ToUpper();
                Dt_Constancias.Rows.Add(Dr_Constancias);
            }
        }

        return Ds_Constancias;
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

    #endregion
    protected void Grid_Constancias_No_Propiedad_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[6].Text != "PAGADA")
            {
                if (e.Row.Cells[8].Controls.Count != 0)
                {
                    ((ImageButton)e.Row.Cells[8].Controls[0]).Enabled = false;
                }
            }
            else
            {
                if (e.Row.Cells[8].Controls.Count != 0)
                {
                    ((ImageButton)e.Row.Cells[8].Controls[0]).Enabled = true;
                }
            }
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Imprimir_Convenio
    /// DESCRIPCIÓN: Generar convenio (con OpenXML SDK a partir de documento con controles de contenido)
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Imprimir_Constancia()
    {
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

        DateTime Fecha = Convert.ToDateTime(Txt_Fecha.Text);
        if (Txt_Confronto.Text == "")
        {
            Txt_Confronto.Text = " ";
        }
        if (Txt_RFC.Text == "")
        {
            Txt_RFC.Text = " ";
        }
        
        try
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(Documento_Salida, true))
            {
                //create XML string matching custom XML part
                string newXml = "<root>"
                    + "<FOLIO>" + Txt_Folio.Text + "</FOLIO>"
                    + "<NOMBRE>" + Txt_Nombre_Solicitante.Text.ToUpper() + "</NOMBRE>"
                    + "<RFC>" + Txt_RFC.Text.ToUpper() + "</RFC>"
                    + "<DOMICILIO>" + Txt_Domicilio.Text.ToUpper() + "</DOMICILIO>"
                    + "<DIAS>" + DateTime.Now.ToString("dd") + "</DIAS>"
                    + "<MES>" + DateTime.Now.ToString("MMMM").ToUpper() + "</MES>"
                    + "<ANIO>" + DateTime.Now.ToString("yyyy") + "</ANIO>"
                    + "<CONFRONTO>" + Txt_Confronto.Text + "</CONFRONTO>"
                    + "<FECHA>" + Convert.ToDateTime(Txt_Fecha.Text).ToString("dd/MMM/yyyy").ToUpper() + "</FECHA>"
                    + "<PROTECCION_PAGO>" + Hdf_Proteccion_Pago.Value + "</PROTECCION_PAGO>"
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
}
