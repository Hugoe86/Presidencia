using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Catalogo_Casos_Especiales.Negocio;
using Presidencia.Operacion_Predial_Quitar_Beneficios.Negocio;
using Presidencia.Operacion_Predial_Quitar_Beneficios.Datos;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Reportes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Quitar_Beneficios : System.Web.UI.Page
{
    private String Boton_Pulsado = "";
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (!IsPostBack)
            {
                Configuracion_Formulario(true);

            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion

    #region Orden de Variacion
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Crear la orden de Variaccion
    ///DESCRIPCIÓN          : Carga los Beneficios existentes en la base de datos
    ///PARAMETROS           : 
    ///CREO                 : Jesus Toledo 
    ///FECHA_CREO           : 7/marzo/2012
    ///MODIFICO
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Alta_Orden_Beneficios()
    {
        DataTable Tabla = (DataTable)Session["Dt_Quitar_Beneficio"];
        try
        {
            Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
            foreach (DataRow Dr_Cuenta in Tabla.Rows)
            {
                Cuenta.P_Cuenta_Predial_ID = Dr_Cuenta["CUENTA_PREDIAL_ID"].ToString().Trim();
                Cuenta.P_Cuota_Fija = "NO";
                Cuenta.P_No_Cuota_Fija = "NULL";
                Cuenta.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                Cuenta.Modifcar_Cuenta();
                Orden_Variacion.P_Cuenta_Predial_ID = Dr_Cuenta["CUENTA_PREDIAL_ID"].ToString().Trim();
                Orden_Variacion.P_Observaciones_Descripcion = Txt_Justificacion.Text.Trim();
                Orden_Variacion.P_Observaciones_Usuraio = Cls_Sessiones.Nombre_Empleado;
                Orden_Variacion.Quitar_Beneficio_Agregar_Observacion();
                //Orden_Variacion.P_Cuenta_Predial_ID = Dr_Cuenta["CUENTA_PREDIAL"].ToString().Trim();
                //Orden_Variacion.P_Cuenta_Predial = Dr_Cuenta["CUENTA_PREDIAL_ID"].ToString().Trim();
                //Orden_Variacion.P_Caso_Especial_ID = Dr_Cuenta["BENEFICIO"].ToString().Trim();
                //Orden_Variacion.P_Cuenta_Predial = Dr_Cuenta["PORCENTAJE"].ToString().Trim();
                //Orden_Variacion.P_No_Cuota_Fija = Dr_Cuenta["NO_CUOTA_FIJA"].ToString().Trim();
                //Orden_Variacion.P_Cuota_Fija = Dr_Cuenta["CUOTA_FIJA"].ToString().Trim();
                //Orden_Variacion.P_Cuenta_Predial = Dr_Cuenta["FECHA_TRAMITE"].ToString().Trim();
                //Orden_Variacion.P_Estatus_Cuenta = Dr_Cuenta["ESTATUS"].ToString().Trim();
                //Orden_Variacion.P_Generar_Orden_Anio = DateTime.Now.Year.ToString();
                //Orden_Variacion.P_Generar_Orden_Cuenta_ID = Dr_Cuenta["CUENTA_PREDIAL_ID"].ToString().Trim();
                //Orden_Variacion.P_Generar_Orden_Estatus = "REALIZADA";
                //Orden_Variacion.Consulta_Id_Movimiento("CANCEL");
                //Orden_Variacion.Agregar_Variacion(Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija, "NO");
                //Orden_Variacion.Modificar_Orden_Variacion();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Quitar Beneficio", "alert('Beneficio quitado Exitosamente');", true);                               
                //Orden_Variacion.Aplicar_Variacion_Orden();
            }


        }
        catch (Exception Ex)
        {
            //Lbl_Ecabezado_Mensaje.Text = "Alta Orden de Variacion Error";
            //Lbl_Mensaje_Error.Text = "Alta Bloqueo Error";
        }
    }
    #endregion

    #region Metodos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN:          Carga el data set físico con el cual se genera el Reporte especificado
    ///PARAMETROS:           Ds_Reporte Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///CREO:                 Jesus Toledo Rodriguez
    ///FECHA_CREO:           06/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataSet Ds_Reporte_Beneficios)
    {
        DataRow Renglon;
        ReportDocument Reporte = new ReportDocument();
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";
        DataSet Ds_Reporte = null;

        if (Ds_Reporte_Beneficios.Tables.Count >= 1)
        {
            Ds_Reporte = new DataSet();
            Ds_Reporte.Tables.Add(Ds_Reporte_Beneficios.Tables[0].Copy());
            Ds_Reporte.Tables[0].TableName = "Dt_Imprimir_Beneficios";
            // Ruta donde se encuentra el reporte Crystal
            Ruta_Reporte_Crystal = "../Rpt/Predial/Rpt_Pre_Quitar_Beneficio.rpt";
            // Se crea el nombre del reporte
            String Nombre_Reporte = "Quitar_Beneficio_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));
            Nombre_Reporte_Generar = Nombre_Reporte;  // Es el nombre del reporte PDF que se va a generar
            Cls_Reportes Reportes = new Cls_Reportes();
            Reportes.Generar_Reporte(ref Ds_Reporte, Ruta_Reporte_Crystal, Nombre_Reporte_Generar + ".pdf", "PDF");
            //Generar_Reporte(ref Ds_Reporte, Ruta_Reporte_Crystal, Nombre_Reporte_Generar + ".pdf", "PDF");
            Mostrar_Reporte(Nombre_Reporte_Generar + ".pdf", "PDF");
            if (Ds_Reporte_Beneficios.Tables.Count > 0)
            {
                Reportes.Generar_Reporte(ref Ds_Reporte, Ruta_Reporte_Crystal, Nombre_Reporte_Generar + ".xls", "Excel");
                Mostrar_Reporte(Nombre_Reporte_Generar + ".xls", "Excel");
            }
        }

    }
    /// *************************************************************************************
    /// NOMBRE:             Exportar_Reporte_PDF
    /// DESCRIPCIÓN:        Sobrecarga de Método que guarda el reporte generado en archivo XLS en la ruta
    ///                     especificada.
    /// PARÁMETROS:         Reporte.- Objeto de tipo documento que contiene el reporte a guardar.
    ///                     Nombre_Reporte.- Nombre que se le dio al reporte.
    /// USUARIO CREO:       Antonio Salvador Benavides Guardado
    /// FECHA CREO:         21/Noviembre/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    public void Exportar_Reporte_Excel(ReportDocument Reporte, String Ruta_Reporte_Generar, String Nombre_Reporte_Generar)
    {
        if (Reporte is ReportDocument)
        {
            ExportOptions CrExportOptions = new ExportOptions();

            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            CrDiskFileDestinationOptions.DiskFileName = HttpContext.Current.Server.MapPath("../../Reporte/" + Nombre_Reporte_Generar);
            CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;

            CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            CrExportOptions.ExportFormatType = ExportFormatType.Excel;
            Reporte.Export(CrExportOptions);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Beneficio
    ///DESCRIPCIÓN          : Carga los Beneficios existentes en la base de datos
    ///PARAMETROS           : 
    ///CREO                 : Jacqueline Ramírez Sierra
    ///FECHA_CREO           : 16/Agosto/2011
    ///MODIFICO
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Beneficio()
    {
        try
        {
            Cls_Cat_Pre_Casos_Especiales_Negocio Casos_Especiales = new Cls_Cat_Pre_Casos_Especiales_Negocio();
            DataTable Dt_Lugar_Pago = Casos_Especiales.Consultar_Nombre_Beneficios();
            DataRow fila = Dt_Lugar_Pago.NewRow();

            fila[Cat_Pre_Casos_Especiales.Campo_Descripcion] = HttpUtility.HtmlDecode("");
            fila[Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID] = "VACIO";
            Dt_Lugar_Pago.Rows.InsertAt(fila, 0);
            Cmb_Beneficio.DataTextField = Cat_Pre_Casos_Especiales.Campo_Descripcion;
            Cmb_Beneficio.DataValueField = Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID;

            fila = Dt_Lugar_Pago.NewRow();
            fila[Cat_Pre_Casos_Especiales.Campo_Descripcion] = HttpUtility.HtmlDecode("&lt;TODOS&gt;");
            fila[Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID] = "TODOS";
            Dt_Lugar_Pago.Rows.InsertAt(fila, 1);
            Cmb_Beneficio.DataTextField = Cat_Pre_Casos_Especiales.Campo_Descripcion;
            Cmb_Beneficio.DataValueField = Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID;
            Cmb_Beneficio.DataSource = Dt_Lugar_Pago;
            Cmb_Beneficio.DataBind();
        }
        catch (Exception Ex)
        {
            //Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            //Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    // llenar grid con columnas en blanco
    private void Ver_Columnas()
    {
        Grid_Quitar_Beneficios.Columns[1].Visible = true;
        Grid_Quitar_Beneficios.Columns[2].Visible = true;
        Grid_Quitar_Beneficios.Columns[3].Visible = true;
        Grid_Quitar_Beneficios.Columns[4].Visible = true;
        Grid_Quitar_Beneficios.Columns[5].Visible = true;
        Grid_Quitar_Beneficios.Columns[6].Visible = true;
        Grid_Quitar_Beneficios.Columns[7].Visible = true;
        Grid_Quitar_Beneficios.Columns[8].Visible = true;
        Grid_Quitar_Beneficios.Columns[9].Visible = true;
    }
    private void Llenar_Grid()
    {
        try
        {
            Cls_Ope_Pre_Quitar_Beneficios_Negocio Quitar_Beneficio = new Cls_Ope_Pre_Quitar_Beneficios_Negocio();
            DataTable Dt_Tabla = Quitar_Beneficio.Consultar_Datos_Beneficios();
            Grid_Quitar_Beneficios.DataSource = Dt_Tabla;
            Session["Dt_Tabla"] = Dt_Tabla;
            Grid_Quitar_Beneficios.Columns[1].Visible = true;
            Grid_Quitar_Beneficios.Columns[2].Visible = true;
            Grid_Quitar_Beneficios.Columns[3].Visible = true;
            Grid_Quitar_Beneficios.Columns[4].Visible = true;
            Grid_Quitar_Beneficios.Columns[5].Visible = true;
            Grid_Quitar_Beneficios.Columns[6].Visible = true;
            Grid_Quitar_Beneficios.Columns[7].Visible = true;
            Grid_Quitar_Beneficios.Columns[8].Visible = true;
            Grid_Quitar_Beneficios.Columns[9].Visible = true;
            Grid_Quitar_Beneficios.DataBind();
            Grid_Quitar_Beneficios.Columns[1].Visible = false;
            Grid_Quitar_Beneficios.Columns[2].Visible = false;

        }
        catch (Exception Ex)
        {
            //Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            //Div_Contenedor_Msj_Error.Visible = true;
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Configuracion_Formulario
    ///DESCRIPCIÓN          : Carga una configuracion de los controles del Formulario
    ///PARAMETROS           : 1. Estatus.    Estatus en el que se cargara la configuración de los controles.
    ///CREO                 : Jacqueline Ramírez Sierra
    ///FECHA_CREO           : 10/Agosto/2011
    ///MODIFICO
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Estatus)
    {
        Txt_Fecha.Enabled = false;
        Grid_Quitar_Beneficios.Enabled = Estatus;
        Grid_Quitar_Beneficios.SelectedIndex = (-1);
        //Btn_Buscar.Enabled = !Estatus;
        Cmb_Beneficio.Enabled = false;
        Llenar_Combo_Beneficio();
        Grid_Quitar_Beneficios.Enabled = false;
        Txt_Justificacion.Enabled = false;
        Txt_Justificacion.Text = null;
    }
    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Habilitar_Controles
    /// DESCRIPCIÓN: Habilita o Deshabilita los controles de la forma según se requiera 
    ///             para la siguiente operación
    /// PARÁMETROS:
    ///         1. Operacion: Indica la operación que se desea realizar por parte del usuario
    /// 	             (inicial)
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 23-jul-2011 
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        try
        {
            switch (Operacion)
            {
                case "Inicial":
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    break;

                case "Nuevo":
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    break;

                case "Modificar":
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    break;
            }

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }// termina metodo Habilitar_Controles
    ///******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Buscar_Roles
    /// 
    /// DESCRIPCIÓN: Ejecuta la Busqueda de roles por la cadena especificada.
    /// 
    /// CREO: Juan Alberto Hernandez Negrete.
    /// FECHA_CREO: 24/Agosto/2010
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///******************************************************************************
    public void Buscar_Beneficio(String Beneficio, GridView Tabla)
    {
        Cls_Ope_Pre_Quitar_Beneficios_Negocio Quitar_Beneficio = new Cls_Ope_Pre_Quitar_Beneficios_Negocio();
        DataTable Dt_Beneficio = null;//Variable que almacena los roles del sistema.
        DataView Dv_Beneficios = null;//Variable que almacena una vista que obtendra a partir de la búsqueda.
        String Expresion_Busqueda = String.Empty;//Variable que almacenara la expresion de búsqueda.

        try
        {
            if (Cmb_Beneficio.SelectedValue == "TODOS")
            {
                Quitar_Beneficio.P_Beneficio = null;
            }
            else
            {
                Quitar_Beneficio.P_Beneficio = Cmb_Beneficio.SelectedValue;
            }
            Dt_Beneficio = Quitar_Beneficio.Consultar_Datos_Beneficios();//Consultamos los roles registrados en sistema.

            Dv_Beneficios = new DataView(Dt_Beneficio);//Creamos el objeto que almacenara una vista de la tabla de roles.

            Expresion_Busqueda = String.Format("{0} '%{1}%'", Tabla.SortExpression, Beneficio);
            Tabla.DataBind();
            Tabla.DataSource = Dv_Beneficios;

            Tabla.DataBind();
            DataTable Dt_Dt = (DataTable)Dv_Beneficios.Table;
            Session["Dt_Quitar_Beneficio"] = Dt_Beneficio;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al ejecutar la busqueda de roles. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region Eventos
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");

        }
        else
        {
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Configuracion_Formulario(true);
            Grid_Quitar_Beneficios.SelectedIndex = (-1);
            Cmb_Beneficio.SelectedIndex = (-1);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Constancia_Propiedad_Click
    ///DESCRIPCIÓN          : Llena la Tabla con la opcion buscada
    ///PARAMETROS          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Beneficio_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Quitar_Beneficios_Negocio Combo_Beneficio = new Cls_Ope_Pre_Quitar_Beneficios_Negocio();
        String Beneficio;
        Validar_Beneficio();
        Beneficio = Cmb_Beneficio.SelectedItem.ToString().Trim();

        Buscar_Beneficio(Beneficio, Grid_Quitar_Beneficios);

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Constancia_Propiedad_Click
    ///DESCRIPCIÓN          : Llena la Tabla con la opcion buscada
    ///PARAMETROS          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Boton_Pulsado = ((ImageButton)sender).ID;
            Session["Boton_Pulsado"] = Boton_Pulsado;
            Grid_Quitar_Beneficios.SelectedIndex = (-1);
            Cargar_Grid_Beneficios(0);
        }
        catch (Exception Ex)
        {
            //Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            //Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Grid_Convenios_Impuestos_Fraccionamientos
    ///DESCRIPCIÓN          : Llena el grid de Convenios con los registros encontrados
    ///PARAMETROS           : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Grid_Beneficios(Int32 Pagina)
    {
        try
        {
            Cls_Ope_Pre_Quitar_Beneficios_Negocio Beneficios = new Cls_Ope_Pre_Quitar_Beneficios_Negocio();
            DataTable Dt_Beneficios;

            Beneficios.P_Campos_Foraneos = true;
            //if (Txt_Busqueda.Text.Trim() != "")
            //    Beneficios.P_Cuenta_Predial = Txt_Busqueda.Text.ToUpper();
            Dt_Beneficios = Beneficios.Consultar_Datos_Beneficios();

            if (Dt_Beneficios != null)
            {
                Grid_Quitar_Beneficios.Columns[1].Visible = true;
                Grid_Quitar_Beneficios.Columns[2].Visible = true;
                Grid_Quitar_Beneficios.Columns[3].Visible = true;
                Grid_Quitar_Beneficios.DataSource = Dt_Beneficios;
                Grid_Quitar_Beneficios.PageIndex = Pagina;
                Grid_Quitar_Beneficios.DataBind();
            }

        }
        catch (Exception Ex)
        {
            //Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            //Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Nuevo_Click
    ///DESCRIPCIÓN          : Deja los componentes listos para dar de Alta un nuevo Constancia_Propiedad
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Pre_Quitar_Beneficios_Negocio Quitar_Cuota = new Cls_Ope_Pre_Quitar_Beneficios_Negocio();
        DataTable Dt_Quitar_Beneficio = (DataTable)Session["Dt_Quitar_Beneficio"];
        DateTime Fecha_Hora;
        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                Configuracion_Formulario(false);
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Txt_Fecha.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Today);
                Txt_Usuario.Text = Cls_Sessiones.Nombre_Empleado;
                Cmb_Beneficio.Enabled = true;
                Txt_Justificacion.Enabled = true;
                Grid_Quitar_Beneficios.Enabled = true;
                Btn_Nuevo.AlternateText = "Guardar";
                Btn_Nuevo.Attributes.Clear();
            }
            else if (Btn_Eliminar.AlternateText.Equals("Eliminar"))
            {
                Fecha_Hora = DateTime.Now;
                Validar_Beneficio();
                Quitar_Cuota.P_Dt_Quitar_Beneficio = Dt_Quitar_Beneficio;
                Quitar_Cuota.P_Beneficio = Cmb_Beneficio.SelectedValue;
                Quitar_Cuota.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                Quitar_Cuota.P_Fecha_Hora = Fecha_Hora;
                Quitar_Cuota.P_Observaciones = Txt_Justificacion.Text.Trim();
                Alta_Orden_Beneficios();
                Quitar_Cuota.Alta_Quitar_Beneficio();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Quitar Beneficio", "alert('El Beneficio fue quitado Exitosamente');", true);
                Generar_Reporte(Quitar_Cuota.Consulta_Reporte());
                Grid_Quitar_Beneficios.DataSource = null;
                Grid_Quitar_Beneficios.DataBind();
                Configuracion_Formulario(true);
                //Btn_Busqueda_Cuota_Minima.Attributes.Add("onclick", Ventana_Cuotas);
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Nuevo.AlternateText = "Nuevo";
                Configuracion_Formulario(true);
                Btn_Nuevo.Attributes.Clear();
            }
        }

        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
        }
    }
    #endregion

    #region Reporte
    /// *************************************************************************************
    /// NOMBRE:              Btn_Imprimir_Click
    /// DESCRIPCIÓN:         Boton para mandar a imprimir el reporte en cristal Reports
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Jacqueline Ramírez Sierra
    /// FECHA CREO:          23/Agosto/2011
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    //protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    //{
    //    Cls_Ope_Pre_Quitar_Beneficios_Negocio Quitar_Beneficio = new Cls_Ope_Pre_Quitar_Beneficios_Negocio();
    //    DataTable Dt_Imprimir_Beneficios = new DataTable();
    //    Dt_Imprimir_Beneficios = (DataTable)Session["Dt_Beneficios"];
    //    Ds_Ope_Pre_Quitar_Beneficios Beneficios = new Ds_Ope_Pre_Quitar_Beneficios();
    //    Quitar_Beneficio.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado.ToUpper();
    //    Quitar_Beneficio.P_Dt_Detalles_Beneficios = Dt_Imprimir_Beneficios;
    //    Dt_Imprimir_Beneficios = Quitar_Beneficio.Consultar_Datos_Beneficios();
    //    Generar_Reporte(Dt_Imprimir_Beneficios, Beneficios);
    //}
    /// *************************************************************************************
    /// NOMBRE:             Generar_Reporte
    /// DESCRIPCIÓN:        Método que invoca la generación del reporte.
    ///              
    /// PARÁMETROS:         Ds_Reporte_Crystal.- Es el DataSet con el que se muestra el reporte en cristal 
    ///                     Ruta_Reporte_Crystal.-  Ruta y Nombre del archivo del Crystal Report.
    ///                     Nombre_Reporte_Generar.- Nombre que tendrá el reporte generado.
    ///                     Formato.- Es el tipo de reporte "PDF", "Excel"
    /// USUARIO CREO:       Juan Alberto Hernández Negrete.
    /// FECHA CREO:         3/Mayo/2011 18:15 p.m.
    /// USUARIO MODIFICO:   Salvador Henrnandez Ramirez
    /// FECHA MODIFICO:     16/Mayo/2011
    /// CAUSA MODIFICACIÓN: Se cambio Nombre_Plantilla_Reporte por Ruta_Reporte_Crystal, ya que este contendrá tambien la ruta
    ///                     y se asigno la opción para que se tenga acceso al método que muestra el reporte en Excel.
    /// *************************************************************************************
    public void Generar_Reporte(ref DataSet Ds_Reporte_Crystal, String Ruta_Reporte_Crystal, String Nombre_Reporte_Generar, String Formato)
    {
        ReportDocument Reporte = new ReportDocument(); // Variable de tipo reporte.
        String Ruta = String.Empty;  // Variable que almacenará la ruta del archivo del crystal report. 
        Cls_Reportes Reportes = new Cls_Reportes();
        try
        {
            Ruta = HttpContext.Current.Server.MapPath(Ruta_Reporte_Crystal);
            Reporte.Load(Ruta);

            if (Ds_Reporte_Crystal is DataSet)
            {
                if (Ds_Reporte_Crystal.Tables.Count > 0)
                {
                    Reporte.SetDataSource(Ds_Reporte_Crystal);
                    if (Ds_Reporte_Crystal.Tables["Dt_Copropietarios"] != null)
                        Reporte.Subreports["CO_PROPIETARIOS"].SetDataSource(Ds_Reporte_Crystal.Tables["Dt_Copropietarios"]);
                    if (Ds_Reporte_Crystal.Tables["Dt_Diferencias"] != null)
                        Reporte.Subreports["Diferencias"].SetDataSource(Ds_Reporte_Crystal.Tables["Dt_Diferencias"]);
                    //if (Formato == "PDF")
                    //{
                    Exportar_Reporte_PDF(Reporte, Nombre_Reporte_Generar);
                    //}
                    //else if (Formato == "Excel")
                    //{
                    Reportes.Generar_Reporte(ref Ds_Reporte_Crystal, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, "Excel");
                    Mostrar_Reporte(Nombre_Reporte_Generar, "Excel");
                    //}
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE:             Exportar_Reporte_PDF
    /// DESCRIPCIÓN:        Método que guarda el reporte generado en formato PDF en la ruta
    ///                     especificada.
    /// PARÁMETROS:         Reporte.- Objeto de tipo documento que contiene el reporte a guardar.
    ///                     Nombre_Reporte.- Nombre que se le dio al reporte.
    /// USUARIO CREO:       Salvador Hernandez Ramírez.
    /// FECHA CREO:         16/Mayo/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    public void Exportar_Reporte_Excel(ReportDocument Reporte, String Nombre_Reporte_Generar)
    {
        if (Reporte is ReportDocument)
        {
            ExportOptions CrExportOptions = new ExportOptions();

            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            CrDiskFileDestinationOptions.DiskFileName = HttpContext.Current.Server.MapPath("../../Reporte/" + Nombre_Reporte_Generar);
            CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;

            CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            CrExportOptions.ExportFormatType = ExportFormatType.Excel;
            Reporte.Export(CrExportOptions);
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
    protected void Grid_Quitar_Beneficios_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Cls_Ope_Pre_Quitar_Beneficios_Negocio Quitar_Beneficio = new Cls_Ope_Pre_Quitar_Beneficios_Negocio();
        DataTable Dt_Imprimir_Beneficios = new DataTable();
        try
        {
            Quitar_Beneficio.P_Beneficio = Cmb_Beneficio.SelectedValue.ToString();
            Grid_Quitar_Beneficios.SelectedIndex = (-1);
            Dt_Imprimir_Beneficios = Quitar_Beneficio.Consultar_Datos_Beneficios();
            Grid_Quitar_Beneficios.PageIndex = e.NewPageIndex;
            Grid_Quitar_Beneficios.DataSource = Dt_Imprimir_Beneficios;
            Grid_Quitar_Beneficios.DataBind();
            //Llenar_Grid(e.NewPageIndex);

        }

        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = "";
        }
    }


    /// *************************************************************************************
    /// NOMBRE:             Exportar_Reporte_PDF
    /// DESCRIPCIÓN:        Método que guarda el reporte generado en formato PDF en la ruta
    ///                     especificada.
    /// PARÁMETROS:         Reporte.- Objeto de tipo documento que contiene el reporte a guardar.
    ///                     Nombre_Reporte.- Nombre que se le dio al reporte.
    /// USUARIO CREO:       Juan Alberto Hernández Negrete.
    /// FECHA CREO:         3/Mayo/2011 18:19 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    public void Exportar_Reporte_PDF(ReportDocument Reporte, String Nombre_Reporte_Generar)
    {
        ExportOptions Opciones_Exportacion = new ExportOptions();
        DiskFileDestinationOptions Direccion_Guardar_Disco = new DiskFileDestinationOptions();
        PdfRtfWordFormatOptions Opciones_Formato_PDF = new PdfRtfWordFormatOptions();

        try
        {
            if (Reporte is ReportDocument)
            {
                Direccion_Guardar_Disco.DiskFileName = HttpContext.Current.Server.MapPath("../../Reporte/" + Nombre_Reporte_Generar);
                Opciones_Exportacion.ExportDestinationOptions = Direccion_Guardar_Disco;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                Reporte.Export(Opciones_Exportacion);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al exportar el reporte. Error: [" + Ex.Message + "]");
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

    #region Validacion

    private String Validar_Beneficio()
    {
        String Mensaje_Error = "";

        if (Cmb_Beneficio.SelectedIndex > 0)  //Validar que haya un notario seleccionado ()
        {
            Mensaje_Error += "&nbsp; &nbsp; &nbsp; &nbsp; + Favor de Seleccionar <br />";
        }
        //if (Txt_Numero_Escritura.Text == "")  //Validar que haya un numero de escritura
        //{
        //    Mensaje_Error += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir el número de escritura <br />";
        //}
        //else if (Txt_Numero_Escritura.Text.Length > 20)  //Validar campo numero escritura (longitud mayor a 20)
        //{
        //    Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + El número de escritura no puede contener más de 20 caracteres<br />";
        //}
        //if (!DateTime.TryParse(Txt_Fecha_Escritura.Text, out Fecha))  //Validar que haya una fecha de escritura
        //{
        //    Mensaje_Error += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir la fecha de la escritura <br />";
        //}
        else
        {
            try
            {
                //if (!(DateTime.Compare(DateTime.Now, Convert.ToDateTime(Txt_Fecha_Escritura.Text)) == 1))
                //{
                //    Mensaje_Error += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir una fecha de la escritura  menor o igual a la fecha actual <br />";
                //}
            }
            catch (Exception excep)
            {

            }
        }
        return Mensaje_Error;
    #endregion

    }

}


