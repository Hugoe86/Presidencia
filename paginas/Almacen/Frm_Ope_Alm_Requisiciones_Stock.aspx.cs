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
using Presidencia.Requisiciones_Stock.Negocio;
using Presidencia.Sessiones;
using Presidencia.Empleados.Negocios;
using Presidencia.Reportes;
using Presidencia.Polizas_Stock.Negocio;

using Presidencia.Polizas.Negocios;
using Presidencia.Constantes;
using Presidencia.Stock;
public partial class paginas_Almacen_Frm_Ope_Alm_Requisiciones_Stock : System.Web.UI.Page
{
    #region  Variables
    Cls_Ope_Com_Alm_Requisiciones_Stock_Negocio Consulta_Requisiciones;
    #endregion

    #region LOAD
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            Session["Activa"] = true;
            Estatus_Inicial();
        }
    }
    #endregion

    #region Eventos

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Mostrar_Orden_Salida
    /// DESCRIPCION:            Evento utilizado para elaborar la orden de salida
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            24/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Btn_Orden_Salida_Click(object sender, ImageClickEventArgs e)
    {
        String Estatus = "";
        Estatus = Determinar_Estatus();  // Se valida y se asigna el estatus "PARCIAL O COMPLETA"
        String No_Requisicion = "";
        Int64 No_Orden_Salida = 0;
        Consulta_Requisiciones = new Cls_Ope_Com_Alm_Requisiciones_Stock_Negocio();
        DataTable Dt_Productos_OS = new DataTable();
        try
        {
            if (Cls_Sessiones.Nombre_Empleado != null && Cls_Sessiones.Nombre_Empleado != "")
            {
                if (Session["No_Requisicion_RQ"] != null)
                    No_Requisicion = Session["No_Requisicion_RQ"].ToString().Trim();
                Consulta_Requisiciones.P_No_Requisicion = No_Requisicion; // Se asigna el numero de requisición a la variable de la capa de negocios
                Consulta_Requisiciones.P_Empleado_Almacen_ID = Cls_Sessiones.Empleado_ID.Trim();
                Consulta_Requisiciones.P_Nombre_Empleado_Almacen = Cls_Sessiones.Nombre_Empleado.Trim();

                if (Validacion_Cantidad_Entregar())
                {
                    if (Cmb_Empleados_UR.SelectedIndex > 0)
                    {
                        Dt_Productos_OS = Calcular_Montos(); // Se calculan los montos y se guardan en la tabla los productos con sus cantidades

                        if (Dt_Productos_OS.Rows.Count > 0)
                        {
                            Consulta_Requisiciones.P_Empleado_Recibio_ID = Cmb_Empleados_UR.SelectedValue;
                            Consulta_Requisiciones.P_Estatus = Estatus;            // En esta parte se indica  si el estatus es PARCIAL o COMPLETA
                            Consulta_Requisiciones.P_Dt_Productos_Requisicion = Dt_Productos_OS;
                            Consulta_Requisiciones.P_Dependencia_ID = Txt_Dependencia_ID.Value.ToString().Trim();
                            Consulta_Requisiciones.P_Proyecto_Programa_ID = Txt_Proyecto_Programa_ID.Value.ToString().Trim();

                            No_Orden_Salida = Consulta_Requisiciones.Alta_Orden_Salida(); // Se instanciá el método donde se da de alta la orden de salida
                            if (No_Orden_Salida > 0)
                            {
                                Mostrar_Orden_Salida(No_Orden_Salida);
                                //ScriptManager.RegisterStartupScript(
                                //   this, this.GetType(),
                                //   "Requisiciones", "alert('Salida registrada con el No. " + No_Orden_Salida + "');", true);
                                Estatus_Inicial();  // Una vez que se genera el reporte se establece la configuración inicial de la página
                                Div_Contenedor_Msj_Error.Visible = false;

                                //GENERAR PÓLIZA
                                //Cls_Ope_Alm_Stock.Crear_Poliza_Compra_Stock(No_Requisicion);
                            }
                            else
                            {                                
                                ScriptManager.RegisterStartupScript(
                                   this, this.GetType(), 
                                   "Requisiciones",
                                   "alert('No se puede realizar la salida de la requisición No. " + Consulta_Requisiciones.P_No_Requisicion +
                                   ", " + Consulta_Requisiciones.P_Mensaje + "');", true);
                                Estatus_Inicial();
                            }                                                        
                        }
                        else
                        {
                            Lbl_Informacion.Text = "No se puede realizar la salida sin productos";
                            Div_Contenedor_Msj_Error.Visible = true;
                        }
                    }
                    else
                    {
                        Lbl_Informacion.Text = "Seleccionar el Empleado";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(
                    this, this.GetType(), "Requisiciones", "alert('Tiempo de espera agotado, vuelva a iniciar sesión!');", true);
                Cls_Sessiones.Mostrar_Menu = false;
                Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Mostrar_Orden_Salida
    /// DESCRIPCION:            Método utilizado consultar la información utilizada para mostrar la orden de salida
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            24/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    private void Mostrar_Orden_Salida(Int64 No_Orden_Salida)
    {
        //DataTable Dt_Cabecera = new DataTable(); // Contendrá los datos generales arrojados de la consulta
        //DataTable Dt_Detalles = new DataTable(); // Contendrá los detalles arrojados de la consulta
        //DataTable Dt_Cabecera_Completa = new DataTable();  // Contendra lso datos completos para la cabecera

        //Double SubTotal_Req = 0;
        //Double IVA_Req = 0;
        //Double Total_Req = 0;

        //Consulta_Requisiciones = new Cls_Ope_Com_Alm_Requisiciones_Transitorias_Negocio(); // Se crea el objeto de la clase de negocios
        //Consulta_Requisiciones.P_No_Orden_Salida = No_Orden_Salida;
        //Dt_Cabecera = Consulta_Requisiciones.Consultar_Informacion_General_OS();
        //Dt_Detalles = Consulta_Requisiciones.Consultar_Detalles_Orden_Salida();

        //if (Dt_Detalles.Rows.Count > 0)
        //{
        //    for (int i = 0; i < Dt_Detalles.Rows.Count; i++)
        //    {
        //        SubTotal_Req = SubTotal_Req + (Convert.ToDouble(Dt_Detalles.Rows[i]["SUBTOTAL"].ToString()));
        //        IVA_Req = IVA_Req + (Convert.ToDouble(Dt_Detalles.Rows[i]["IVA"].ToString()));
        //        Total_Req = Total_Req + (Convert.ToDouble(Dt_Detalles.Rows[i]["TOTAL"].ToString()));
        //    }
        //}

        //// Se  Crea una nueva tabla para agregar los detalles faltantes de la cabecera
        //Dt_Cabecera_Completa.Columns.Add("NO_ORDEN_SALIDA");
        //Dt_Cabecera_Completa.Columns.Add("UNIDAD_RESPONSABLE");
        //Dt_Cabecera_Completa.Columns.Add("F_FINANCIAMIENTO");
        //Dt_Cabecera_Completa.Columns.Add("PROGRAMA");
        //Dt_Cabecera_Completa.Columns.Add("FOLIO");
        //Dt_Cabecera_Completa.Columns.Add("FECHA_AUTORIZACION");
        //Dt_Cabecera_Completa.Columns.Add("SUBTOTAL");
        //Dt_Cabecera_Completa.Columns.Add("IVA");
        //Dt_Cabecera_Completa.Columns.Add("TOTAL");
        //Dt_Cabecera_Completa.Columns.Add("ENTREGO");
        //Dt_Cabecera_Completa.Columns.Add("RECIBIO");

        //DataRow Registro = Dt_Cabecera_Completa.NewRow(); // Se crea un nuevo registro

        //Registro["NO_ORDEN_SALIDA"] = Dt_Cabecera.Rows[0]["NO_ORDEN_SALIDA"].ToString().Trim();
        //Registro["UNIDAD_RESPONSABLE"] = HttpUtility.HtmlDecode(Dt_Cabecera.Rows[0]["UNIDAD_RESPONSABLE"].ToString().Trim());
        //Registro["F_FINANCIAMIENTO"] = HttpUtility.HtmlDecode(Dt_Cabecera.Rows[0]["F_FINANCIAMIENTO"].ToString().Trim());
        //Registro["PROGRAMA"] = HttpUtility.HtmlDecode(Dt_Cabecera.Rows[0]["PROGRAMA"].ToString().Trim());
        //Registro["FOLIO"] = HttpUtility.HtmlDecode(Dt_Cabecera.Rows[0]["FOLIO"].ToString().Trim());

        //if (Dt_Cabecera.Rows[0]["FECHA_AUTORIZACION"].ToString().Trim() != "")
        //    Registro["FECHA_AUTORIZACION"] = HttpUtility.HtmlDecode(Dt_Cabecera.Rows[0]["FECHA_AUTORIZACION"].ToString().Trim());
        //else
        //    Registro["FECHA_AUTORIZACION"] = "";

        //Registro["SUBTOTAL"] = Convert.ToString(SubTotal_Req);
        //Registro["IVA"] = Convert.ToString(IVA_Req);
        //Registro["TOTAL"] = Convert.ToString(Total_Req);

        //if (Dt_Cabecera.Rows[0]["ENTREGO"].ToString().Trim() != "")
        //    Registro["ENTREGO"] = HttpUtility.HtmlDecode(Dt_Cabecera.Rows[0]["ENTREGO"].ToString().Trim());
        //else
        //    Registro["ENTREGO"] = "";

        //if (Dt_Cabecera.Rows[0]["RECIBIO"].ToString().Trim() != "")
        //    Registro["RECIBIO"] = HttpUtility.HtmlDecode(Dt_Cabecera.Rows[0]["RECIBIO"].ToString().Trim());
        //else
        //    Registro["RECIBIO"] = "";

        //Dt_Cabecera_Completa.Rows.InsertAt(Registro, 0); // Se Inserta el Registro

        //String Formato = "PDF";
        //Ds_Alm_Com_Orden_Salida Ds_Orden_Salida = new Ds_Alm_Com_Orden_Salida();
        //String Nombre_Reporte_Crystal = "Rpt_Alm_Com_Orden_Salida.rpt";

        //Generar_Reporte(Dt_Cabecera_Completa, Dt_Detalles, Ds_Orden_Salida, Nombre_Reporte_Crystal, Formato);

        DataTable Dt_Cabecera = new DataTable(); // Contendrá los datos generales arrojados de la consulta
        DataTable Dt_Detalles = new DataTable(); // Contendrá los detalles arrojados de la consulta
        DataTable Dt_Cabecera_Completa = new DataTable();  // Contendra lso datos completos para la cabecera

        Double SubTotal_Req = 0;
        Double IVA_Req = 0;
        Double Total_Req = 0;

        Consulta_Requisiciones = new Cls_Ope_Com_Alm_Requisiciones_Stock_Negocio(); // Se crea el objeto de la clase de negocios
        Consulta_Requisiciones.P_No_Orden_Salida = No_Orden_Salida;
        Dt_Cabecera = Consulta_Requisiciones.Consultar_Informacion_General_OS();
        Dt_Detalles = Consulta_Requisiciones.Consultar_Detalles_Orden_Salida();

        if (Dt_Detalles.Rows.Count > 0)
        {

            for (int i = 0; i < Dt_Detalles.Rows.Count; i++)
            {
                SubTotal_Req = SubTotal_Req + (Convert.ToDouble(Dt_Detalles.Rows[i]["SUBTOTAL"].ToString()));
                IVA_Req = IVA_Req + (Convert.ToDouble(Dt_Detalles.Rows[i]["IVA"].ToString()));
                Total_Req = Total_Req + (Convert.ToDouble(Dt_Detalles.Rows[i]["TOTAL"].ToString()));
            }
        }

        // Se  Crea una nueva tabla para agregar los detalles faltantes de la cabecera
        Dt_Cabecera_Completa.Columns.Add("NO_ORDEN_SALIDA");
        Dt_Cabecera_Completa.Columns.Add("UNIDAD_RESPONSABLE");
        Dt_Cabecera_Completa.Columns.Add("F_FINANCIAMIENTO");
        Dt_Cabecera_Completa.Columns.Add("PROGRAMA");
        Dt_Cabecera_Completa.Columns.Add("FOLIO");
        Dt_Cabecera_Completa.Columns.Add("FECHA_AUTORIZACION");
        Dt_Cabecera_Completa.Columns.Add("SUBTOTAL");
        Dt_Cabecera_Completa.Columns.Add("IVA");
        Dt_Cabecera_Completa.Columns.Add("TOTAL");
        Dt_Cabecera_Completa.Columns.Add("ENTREGO");
        Dt_Cabecera_Completa.Columns.Add("RECIBIO");

        DataRow Registro = Dt_Cabecera_Completa.NewRow(); // Se crea un nuevo registro

        Registro["NO_ORDEN_SALIDA"] = Dt_Cabecera.Rows[0]["NO_ORDEN_SALIDA"].ToString().Trim();
        Registro["UNIDAD_RESPONSABLE"] = HttpUtility.HtmlDecode(Dt_Cabecera.Rows[0]["UNIDAD_RESPONSABLE"].ToString().Trim());
        Registro["F_FINANCIAMIENTO"] = HttpUtility.HtmlDecode(Dt_Cabecera.Rows[0]["F_FINANCIAMIENTO"].ToString().Trim());
        Registro["PROGRAMA"] = HttpUtility.HtmlDecode(Dt_Cabecera.Rows[0]["PROGRAMA"].ToString().Trim());
        Registro["FOLIO"] = HttpUtility.HtmlDecode(Dt_Cabecera.Rows[0]["FOLIO"].ToString().Trim());

        if (Dt_Cabecera.Rows[0]["FECHA_AUTORIZACION"].ToString().Trim() != "")
            Registro["FECHA_AUTORIZACION"] = HttpUtility.HtmlDecode(Dt_Cabecera.Rows[0]["FECHA_AUTORIZACION"].ToString().Trim());
        else
            Registro["FECHA_AUTORIZACION"] = "";

        Registro["SUBTOTAL"] = Convert.ToString(SubTotal_Req);
        Registro["IVA"] = Convert.ToString(IVA_Req);
        Registro["TOTAL"] = Convert.ToString(Total_Req);

        if (Dt_Cabecera.Rows[0]["ENTREGO"].ToString().Trim() != "")
            Registro["ENTREGO"] = HttpUtility.HtmlDecode(Dt_Cabecera.Rows[0]["ENTREGO"].ToString().Trim());
        else
            Registro["ENTREGO"] = "";

        if (Dt_Cabecera.Rows[0]["RECIBIO"].ToString().Trim() != "")
            Registro["RECIBIO"] = HttpUtility.HtmlDecode(Dt_Cabecera.Rows[0]["RECIBIO"].ToString().Trim());
        else
            Registro["RECIBIO"] = "";

        Dt_Cabecera_Completa.Rows.InsertAt(Registro, 0); // Se Inserta el Registro

        String Formato = "PDF";
        Ds_Alm_Com_Orden_Salida Ds_Orden_Salida = new Ds_Alm_Com_Orden_Salida();
        String Nombre_Reporte_Crystal = "Rpt_Alm_Com_Orden_Salida.rpt";

        Generar_Reporte(Dt_Cabecera_Completa, Dt_Detalles, Ds_Orden_Salida, Nombre_Reporte_Crystal, Formato);
    }


///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN:          Carga el data set físico con el cual se genera el Reporte especificado
    ///PARAMETROS:           1.- Dt_Cabecera.- Contiene la informacion de la consulta a la base de datos
    ///                      2.- Dt_Detalles.- Contiene los detalles de la consulta a la BD
    ///                      2.- Ds_Orden_Salida, Objeto que contiene la instancia del DataSet fisico del Reporte a generar
    ///                      3.- Nombre_Reporte_Crystal, contiene el nombre del Reporte  que se creó en Crystal Report
    ///                      4.- Formato, Es el formato con el que se va a generar el reporte, ya sea PDF o Excel
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           24/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataTable Dt_Cabecera, DataTable Dt_Detalles, DataSet Ds_Orden_Salida, String Nombre_Reporte_Crystal, String Formato)
    {
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";
        DataRow Renglon;

        try
        {
            // Se llena la tabla Cabecera del DataSet
            Renglon = Dt_Cabecera.Rows[0];
            Ds_Orden_Salida.Tables[0].ImportRow(Renglon);

            // Se llena la tabla Detalles del DataSet
            for (int Cont_Elementos = 0; Cont_Elementos < Dt_Detalles.Rows.Count; Cont_Elementos++)
            {
                Renglon = Dt_Detalles.Rows[Cont_Elementos]; //Instanciar renglon e importarlo
                Ds_Orden_Salida.Tables[1].ImportRow(Renglon);
            }

            // Ruta donde se encuentra el reporte Crystal
            Ruta_Reporte_Crystal = "../Rpt/Almacen/" + Nombre_Reporte_Crystal;

            // Se crea el nombre del reporte
            String Nombre_Reporte = "Rpt_Orden_Salida" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

            // Se da el nombre del reporte que se va generar
            if (Formato == "PDF")
                Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
            else if (Formato == "Excel")
                Nombre_Reporte_Generar = Nombre_Reporte + ".xls";  // Es el nombre del repote en Excel que se va a generar

            Cls_Reportes Reportes = new Cls_Reportes();
            Reportes.Generar_Reporte(ref Ds_Orden_Salida, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
            Mostrar_Reporte(Nombre_Reporte_Generar, Formato);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al llenar el DataSet. Error: [" + Ex.Message + "]");
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

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Btn_Salir_Click
    /// DESCRIPCION:            Evento utilizado para salir de la aplicación o configurar el estatus inicial de la misma
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            27/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText == "Salir")
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        else if (Btn_Salir.AlternateText == "Atras")
            Estatus_Inicial();
    }

   
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Btn_Salir_Click
    /// DESCRIPCION:            Evento utilizado realizar la busqueda de las requisiciones
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            27/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
            Llenar_Grid_Requisiciones();
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Btn_Seleccionar_Requisicion_Click
    /// DESCRIPCION:            Evento utilizado al seleccionar alguna requisición
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            27/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Btn_Seleccionar_Requisicion_Click(object sender, ImageClickEventArgs e)
    {
        // Declaración de Objetos y Variables
        ImageButton Btn_Selec_Requisicion = null;
        String No_Requisicion = String.Empty;
        DataTable Dt_Detalles_Requisicion = new DataTable();
        DataTable Dt_Productos_Requisicion = new DataTable();
        DataTable Dt_Programa_Financiamiento = new DataTable();

        try
        {
            Btn_Selec_Requisicion = (ImageButton)sender;
            No_Requisicion = Btn_Selec_Requisicion.CommandArgument;
            Session["No_Requisicion_RQ"] = No_Requisicion;

            Consulta_Requisiciones = new Cls_Ope_Com_Alm_Requisiciones_Stock_Negocio();
            Consulta_Requisiciones.P_No_Requisicion = No_Requisicion.Trim();
            Dt_Detalles_Requisicion = Consulta_Requisiciones.Consulta_Detalles_Requisicion(); // Se consultan los detalles de la requisición
            Dt_Productos_Requisicion = Consulta_Requisiciones.Consulta_Productos_Requisicion(); // Se consultan los productos de la requisición
            Dt_Programa_Financiamiento = Consulta_Requisiciones.Consulta_Pragrama_Financiamiento(); // Se consulta el programa financiamiento de la requisición

            if (Dt_Productos_Requisicion.Rows.Count > 0) // validación
            {
                Llenar_Detalles_Requisicion(Dt_Detalles_Requisicion, Dt_Productos_Requisicion, Dt_Programa_Financiamiento);
                Llenar_Combo_Empleados_UR();
                Div_Requisiciones_Stock.Visible = false;
                Div_Contenedor_Msj_Error.Visible = false;
                Btn_Orden_Salida.Visible = true;
                Btn_Salir.AlternateText = "Atras";
                Btn_Salir.ToolTip = "Atrás";
                Mostrar_Busqueda(false); // Se ocultan los controles para la búsqueda
            }
            else
            {
                Lbl_Informacion.Text = "La Requisición no contiene productos";
                Div_Contenedor_Msj_Error.Visible = true;
                Div_Requisiciones_Stock.Visible = true;
                Btn_Orden_Salida.Visible = false;
                Mostrar_Busqueda(true); // Se muestran los controles para la búsqueda
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Chk_Fecha_B_CheckedChanged
    /// DESCRIPCION:            Método utilizado para establecer la configuración inicial de los componentes de la fecha
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            27/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Chk_Fecha_B_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Fecha_B.Checked == true)
        {
            Txt_Fecha_Inicio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");

            Img_Btn_Fecha_Inicio.Enabled = true;
            Img_Btn_Fecha_Fin.Enabled = true;
        }
        else
        {
            Txt_Fecha_Inicio.Text = "";
            Txt_Fecha_Fin.Text = "";

            Img_Btn_Fecha_Inicio.Enabled = false;
            Img_Btn_Fecha_Fin.Enabled = false;
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Btn_Limpiar_Click
    /// DESCRIPCION:            Método utilizado para establecer la configuración inicial de los compoentes utilizados para la busqueda
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            27/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
    {
        Estatus_Incial_Busqueda();
    }

    #endregion



    #region Metodos

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Estatus_Inicial
    /// DESCRIPCION:            Método utilizado para establecer la configuración inicial de la página
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            22/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    private void Estatus_Inicial()
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Div_Detalles_Requisicion.Visible = false;
        Session["No_Requisicion_RQ"] = null;
        Session["Dt_Productos"] = null;
        Btn_Orden_Salida.Visible= false;
        Btn_Salir.AlternateText = "Salir";
        Btn_Salir.ToolTip = "Salir";
        Llenar_Grid_Requisiciones();
        Estatus_Incial_Busqueda();

            DataTable Dt_Empleados_UR = new DataTable();
            Cmb_Empleados_UR.DataSource = Dt_Empleados_UR; // Limpia el Combo
            Cmb_Empleados_UR.DataBind();
            Txt_Numero_Empleado.Text = "";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Estatus_Incial_Busqueda
    /// DESCRIPCION:            Método utilizado para establecer la configuración inicial de los componentes de busqueda
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            22/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    private void Estatus_Incial_Busqueda()
    {
        Chk_Fecha_B.Checked = false;
        Txt_Req_Buscar.Text = "";
        Txt_Fecha_Fin.Text = "";
        Txt_Fecha_Inicio.Text = "";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Llenar_Grid_Requisiciones
    /// DESCRIPCION:            Método utilizado llenar el grid con las requisiciones de stock
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            22/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    private void Llenar_Grid_Requisiciones()
    {
        Consulta_Requisiciones = new Cls_Ope_Com_Alm_Requisiciones_Stock_Negocio();
        DataTable Dt_Requisiciones = new DataTable();

      try
        {
            if (Txt_Req_Buscar.Text.Trim() != "")
                Consulta_Requisiciones.P_No_Requisicion = Txt_Req_Buscar.Text.Trim();
            else
                Consulta_Requisiciones.P_No_Requisicion = null;


            if (Chk_Fecha_B.Checked) // Si esta activado el Check
            {
                DateTime Date1 = new DateTime();  //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
                DateTime Date2 = new DateTime();

                if ((Txt_Fecha_Inicio.Text.Length != 0))
                {
                    if ((Txt_Fecha_Inicio.Text.Length == 11) && (Txt_Fecha_Fin.Text.Length == 11))
                    {
                        //Convertimos el Texto de los TextBox fecha a dateTime
                        Date1 = DateTime.Parse(Txt_Fecha_Inicio.Text);
                        Date2 = DateTime.Parse(Txt_Fecha_Fin.Text);

                        //Validamos que las fechas sean iguales o la final sea mayor que la inicial, de lo contrario se manda un mensaje de error 
                        if ((Date1 < Date2) | (Date1 == Date2))
                        {
                            if (Txt_Fecha_Fin.Text.Length != 0)
                            {
                                //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                                Consulta_Requisiciones.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim());
                                Consulta_Requisiciones.P_Fecha_Final = Formato_Fecha(Txt_Fecha_Fin.Text.Trim());
                                Div_Contenedor_Msj_Error.Visible = false;
                            }
                            else
                            {
                                String Fecha = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim()); //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                                Consulta_Requisiciones.P_Fecha_Inicial = Fecha;
                                Consulta_Requisiciones.P_Fecha_Final = Fecha;
                                Div_Contenedor_Msj_Error.Visible = false;
                            }
                        }
                        else
                        {
                            Lbl_Informacion.Text = " Fecha no valida ";
                            Div_Contenedor_Msj_Error.Visible = true;
                        }
                    }
                    else
                    {
                        Lbl_Informacion.Text = " Fecha no valida ";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
            }
            Dt_Requisiciones = Consulta_Requisiciones.Consulta_Requisiciones();

            if(Dt_Requisiciones.Rows.Count >0){
                Grid_Requisiciones_Stock.Columns[1].Visible = true; // Se pone visible la columna que contiene el numero de requisición
                Grid_Requisiciones_Stock.DataSource = Dt_Requisiciones;
                Grid_Requisiciones_Stock.DataBind();
                Grid_Requisiciones_Stock.Columns[1].Visible = false; // Se oculta la columna
                Div_Requisiciones_Stock.Visible = true;
                Div_Contenedor_Msj_Error.Visible=false;
                Mostrar_Busqueda(true); // Se muestra los controles para la búsqueda
            }else {
                Div_Requisiciones_Stock.Visible = false;
                Lbl_Informacion.Text = "No se encontraron requisiciones de stock";
                Div_Contenedor_Msj_Error.Visible=true;
            }

          }
          catch (Exception ex)
          {
              throw new Exception(ex.Message, ex);
          }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN:     Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:      1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                     en caso de que cumpla la condicion del if
    ///CREO:            Salvador Hernández Ramírez
    ///FECHA_CREO:      19/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Formato_Fecha(String Fecha)
    {
        String Fecha_Valida = Fecha;
        //Se le aplica un split a la fecha 
        String[] aux = Fecha.Split('/');
        //Se modifica a mayusculas para que oracle acepte el formato. 
        switch (aux[1])
        {
            case "dic":
                aux[1] = "DEC";
                break;
        }
        //Concatenamos la fecha, y se cambia el orden a DD-MMM-YYYY para que sea una fecha valida para oracle
        Fecha_Valida = aux[0] + "-" + aux[1] + "-" + aux[2];
        return Fecha_Valida;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Mostrar_Busqueda
    /// DESCRIPCION:            Método utilizado para mostrar u ocultar los controles para la búsqueda
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            28/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    private void Mostrar_Busqueda(Boolean Estatus)
    {
        Div_Busqueda_Av.Visible = Estatus;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Mostrar_Detalles_Requisicion
    /// DESCRIPCION:            Método utilizado llenar la tabla con los montos totales
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            22/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    private void Llenar_Detalles_Requisicion( DataTable Dt_Detalles_Requisicion, DataTable Dt_Productos_Requisicion, DataTable Dt_Programa_Financiamiento)
    {
        DataTable Dt_Productos = new DataTable();

        Dt_Productos.Columns.Add("PRODUCTO_ID"); // Se crean las columnas que tendra esta tablas
        Dt_Productos.Columns.Add("CLAVE");
        Dt_Productos.Columns.Add("PRODUCTO"); // Contendra el Nombre del producto
        Dt_Productos.Columns.Add("DESCRIPCION"); // Contendra la descripcion del producto
        Dt_Productos.Columns.Add("CANTIDAD_SOLICITADA");
        Dt_Productos.Columns.Add("CANTIDAD_ENTREGADA");
        Dt_Productos.Columns.Add("CANTIDAD_A_ENTREGAR");
        Dt_Productos.Columns.Add("UNIDAD");
        Dt_Productos.Columns.Add("PRECIO");
        Dt_Productos.Columns.Add("SUBTOTAL");
        Dt_Productos.Columns.Add("MONTO_IVA");
        Dt_Productos.Columns.Add("TOTAL");
        Dt_Productos.Columns.Add("PORCENTAJE_IVA");
        Dt_Productos.Columns.Add("PARTIDA_ID");

    try
        {
        for (int i = 0; i < Dt_Productos_Requisicion.Rows.Count; i++)
        {
            String Producto = HttpUtility.HtmlDecode(Dt_Productos_Requisicion.Rows[i]["NOMBRE_PRODUCTO"].ToString());
            String Descripcion = HttpUtility.HtmlDecode(Dt_Productos_Requisicion.Rows[i]["DESCRIPCION"].ToString()) ;
            DataRow Registro = Dt_Productos.NewRow();
            Int64 Diferencia = 0;
            Int64 Cantidad_Solicitada = 0;
            Int64 Cantidad_Entregada = 0;

            Registro["PRODUCTO_ID"] = Dt_Productos_Requisicion.Rows[i]["PRODUCTO_ID"].ToString(); // Se agregan valores a la tabla
            Registro["CLAVE"] = Dt_Productos_Requisicion.Rows[i]["CLAVE"].ToString();
            Registro["PRODUCTO"] = Producto.ToString().Trim();
            Registro["DESCRIPCION"] = Descripcion.ToString().Trim();

            Cantidad_Solicitada = Convert.ToInt64(HttpUtility.HtmlDecode(Dt_Productos_Requisicion.Rows[i]["CANTIDAD_SOLICITADA"].ToString().Trim()));

            Registro["CANTIDAD_SOLICITADA"] = Cantidad_Solicitada;
            
            if (Dt_Productos_Requisicion.Rows[i]["CANTIDAD_ENTREGADA"].ToString().Trim() != "")
                Cantidad_Entregada = Convert.ToInt64(HttpUtility.HtmlDecode(Dt_Productos_Requisicion.Rows[i]["CANTIDAD_ENTREGADA"].ToString()));

            Registro["CANTIDAD_ENTREGADA"] = Cantidad_Entregada;

            Diferencia = Cantidad_Solicitada - Cantidad_Entregada;

            Registro["CANTIDAD_A_ENTREGAR"] = Diferencia;

            Registro["UNIDAD"] = Dt_Productos_Requisicion.Rows[i]["UNIDAD"].ToString();
            Registro["PRECIO"] = Dt_Productos_Requisicion.Rows[i]["PRECIO"].ToString();
            Registro["SUBTOTAL"] = Dt_Productos_Requisicion.Rows[i]["SUBTOTAL"].ToString();
            Registro["MONTO_IVA"] = Dt_Productos_Requisicion.Rows[i]["MONTO_IVA"].ToString();
            Registro["TOTAL"] = Dt_Productos_Requisicion.Rows[i]["MONTO_TOTAL"].ToString();
            Registro["PORCENTAJE_IVA"] = Dt_Productos_Requisicion.Rows[i]["PORCENTAJE_IVA"].ToString();
            Registro["PARTIDA_ID"] = Dt_Productos_Requisicion.Rows[i]["PARTIDA_ID"].ToString();

            if (Diferencia != 0) // Si hay productos a entregar se inserta el registro
                Dt_Productos.Rows.InsertAt(Registro, i);
            else if (Diferencia == 0) // De todas formas se insertan, ya que  nada mas se pondra inabilitado el combo cuando ya se hayan entrgado
                Dt_Productos.Rows.InsertAt(Registro, i);
        }

        // Se agregan los valores al Grid
        Grid_Productos_Requisicion.Columns[0].Visible = true; // Se pone visible la columna que contine el Producto_ID
        Grid_Productos_Requisicion.Columns[10].Visible = true; // Se pone visible la columna que contine el IVA
        Grid_Productos_Requisicion.Columns[9].Visible = true; // Se pone visible la columna que contine el SubTotal
        Grid_Productos_Requisicion.Columns[12].Visible = true; // Se pone visible la columna que contine el Porcentaje IVA
        Grid_Productos_Requisicion.Columns[13].Visible = true; // Se pone visible la columna que contine la Partida_ID
        Grid_Productos_Requisicion.DataSource = Dt_Productos;
        Grid_Productos_Requisicion.DataBind();
        Grid_Productos_Requisicion.Columns[0]  .Visible = false; // Se  oculta la columna que contine el Producto_ID
        Grid_Productos_Requisicion.Columns[10].Visible = false; // Se pone visible la columna que contine el IVA
        Grid_Productos_Requisicion.Columns[9].Visible = false; // Se pone visible la columna que contine el SubTotal
        Grid_Productos_Requisicion.Columns[12].Visible = false; // Se  oculta la columna que contine el Porcentaje IVA
        Grid_Productos_Requisicion.Columns[13].Visible = false; // Se  oculta la columna que contine la Partida_ID

        Session["Dt_Detalles_Requisicion"] = Dt_Detalles_Requisicion; // Se crea la variable de session que guardara los detalles de la Requisición
        Session["Dt_Productos"] = Dt_Productos; // Se crea la variable de session que guardara los productos de la requisición
        // Se llena el TextBox Cantidad_Entregada
        for (int i = 0; i < Dt_Productos.Rows.Count; i++)
        {
            // Ya esta actualizada la columna grid
            String Cantidad_A_Entregar = Dt_Productos.Rows[i]["CANTIDAD_A_ENTREGAR"].ToString();
            TextBox Temporal = (TextBox)Grid_Productos_Requisicion.Rows[i].FindControl("Txt_Cantidad_A_Entregar");

            if (Temporal != null)
            {
                Temporal.Text = Cantidad_A_Entregar;

                if (Cantidad_A_Entregar == "0")
                    Temporal.Enabled = false;
            }
        }

        // Se agegan los valores a los TextBox y Label
        Txt_Folio.Text= HttpUtility.HtmlDecode(Dt_Detalles_Requisicion.Rows[0]["FOLIO"].ToString());

        String Fecha = Dt_Detalles_Requisicion.Rows[0]["FECHA_AUTORIZACION"].ToString(); // Se optiene y se convierte la fecha
        DateTime Fecha_Convertida = Convert.ToDateTime(Fecha);
        Txt_Fecha_Autorizacion.Text = String.Format("{0:dd/MMM/yyyy}", Fecha_Convertida);

        Txt_Unidad_Responsable.Text = HttpUtility.HtmlDecode(Dt_Detalles_Requisicion.Rows[0]["UNIDAD_RESPONSABLE"].ToString());
        Txt_Justificacion.Text = HttpUtility.HtmlDecode(Dt_Detalles_Requisicion.Rows[0]["COMENTARIOS"].ToString());

        if (Dt_Programa_Financiamiento.Rows.Count > 0) // Se agregá  la Fuente de financiamiento y el proyecto programa
        {
            Txt_Financiamiento.Text = HttpUtility.HtmlDecode(Dt_Programa_Financiamiento.Rows[0]["FINANCIAMIENTO"].ToString().Trim());
            Txt_Programa.Text = HttpUtility.HtmlDecode(Dt_Programa_Financiamiento.Rows[0]["PROYECTO_PROGRAMA"].ToString().Trim());
        }

        Lbl_SubTotal.Text = "$" + " " + HttpUtility.HtmlDecode(Dt_Detalles_Requisicion.Rows[0]["SUBTOTAL"].ToString());
        Lbl_IVA.Text = "$" + " " + HttpUtility.HtmlDecode(Dt_Detalles_Requisicion.Rows[0]["MONTO_IVA"].ToString());
        Lbl_Total.Text = "$" + " " + HttpUtility.HtmlDecode(Dt_Detalles_Requisicion.Rows[0]["MONTO_TOTAL"].ToString());

        Txt_Dependencia_ID.Value = HttpUtility.HtmlDecode(Dt_Detalles_Requisicion.Rows[0]["UNIDAD_RESPONSABLE_ID"].ToString()); // Se guarda el Id de la Dependencia en el campo oculto
        Txt_Proyecto_Programa_ID.Value = HttpUtility.HtmlDecode(Dt_Programa_Financiamiento.Rows[0]["PROYECTO_PROGRAMA_ID"].ToString().Trim()); // Se guarda el Id del Proyecto Programa ID
 
        Div_Detalles_Requisicion.Visible = true;

        //// Se revisa la información que tiene el Grid y de esta manera se hace mas grande el panel o mas pequeño
        //if (Dt_Productos.Rows.Count > 0)
        //{
        //    if (Dt_Productos.Rows.Count > 3)
        //        Pnl_Detalles_Requisicion.Height = System.Web.UI.WebControls.Unit.Pixel(250);
        //    else
        //        Pnl_Detalles_Requisicion.Height = System.Web.UI.WebControls.Unit.Pixel(115);
        //}

    }
    catch (Exception ex)
    {
        throw new Exception(ex.Message, ex);
    }
  }



    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Calcular_Montos
    /// DESCRIPCION:            Método utilizado para calcular los montos, cuando nos e van a entregar 
    ///                         los productos de la requisición completamente
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            23/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    private DataTable Calcular_Montos()
    {
        DataTable Dt_Productos = new DataTable();
      
        Dt_Productos.Columns.Add("PRODUCTO_ID"); // Se crean las columnas que tendra esta tablas
        Dt_Productos.Columns.Add("CANTIDAD_A_ENTREGAR");
        Dt_Productos.Columns.Add("CANTIDAD_ENTREGADA");
        Dt_Productos.Columns.Add("PRECIO");
        Dt_Productos.Columns.Add("SUBTOTAL");
        Dt_Productos.Columns.Add("MONTO_IVA");
        Dt_Productos.Columns.Add("TOTAL");
        Dt_Productos.Columns.Add("PARTIDA_ID");

          try
        {

            for (int i = 0; i < Grid_Productos_Requisicion.Rows.Count; i++) // guardar en una tabla los valores del grid y calcular lso valores
            {
                DataRow Registro = Dt_Productos.NewRow();
                Double Precio = 0;
                Double Porcentaje_IVA = 0;
                Int64 Cantidad_A_Entregar = 0;
                Double SubTotal = 0; // Variables para calcular los 
                Double IVA = 0;
                Double Total = 0;

                // Ya se actualizaron
                Registro["PRODUCTO_ID"] = HttpUtility.HtmlDecode(Grid_Productos_Requisicion.Rows[i].Cells[0].Text.Trim()); // Se consultan los valores del Grid
                Registro["PRECIO"] = HttpUtility.HtmlDecode(Grid_Productos_Requisicion.Rows[i].Cells[8].Text.Trim());
                    Precio = Convert.ToDouble(Grid_Productos_Requisicion.Rows[i].Cells[8].Text.Trim());
                    Porcentaje_IVA = Convert.ToDouble(Grid_Productos_Requisicion.Rows[i].Cells[12].Text.Trim());
                Registro["PARTIDA_ID"] = HttpUtility.HtmlDecode(Grid_Productos_Requisicion.Rows[i].Cells[13].Text.Trim()); // Se agrega la partida ID

                TextBox Txt_Cantidad_A_Entregar = (TextBox)Grid_Productos_Requisicion.Rows[i].Cells[6].FindControl("Txt_Cantidad_A_Entregar");

                if (Txt_Cantidad_A_Entregar.Text.Trim() != "")
                    Cantidad_A_Entregar = Convert.ToInt64(Txt_Cantidad_A_Entregar.Text);
                else
                    Cantidad_A_Entregar = 0;


                Registro["CANTIDAD_A_ENTREGAR"] = Convert.ToString(Cantidad_A_Entregar);

                if (HttpUtility.HtmlDecode(Grid_Productos_Requisicion.Rows[i].Cells[5].Text.Trim()) != "")
                    Registro["CANTIDAD_ENTREGADA"] = HttpUtility.HtmlDecode(Grid_Productos_Requisicion.Rows[i].Cells[5].Text.Trim());
                else
                    Registro["CANTIDAD_ENTREGADA"] = 0;

                if (Cantidad_A_Entregar != 0)
                {
                    SubTotal = Cantidad_A_Entregar * Precio;      // Se calcula el Subtotal
                    IVA = SubTotal * (Porcentaje_IVA / 100);   // Se calcula el IVA
                    Total = SubTotal + IVA;            // Se calcula el Total

                    Registro["SUBTOTAL"] = Convert.ToString(SubTotal.ToString()); // Se agregan los montos a la tabla.
                    Registro["MONTO_IVA"] = Convert.ToString(IVA.ToString());
                    Registro["TOTAL"] = Convert.ToString(Total.ToString());

                    if (Cantidad_A_Entregar != 0) // Si cantidad es Distinta de 0
                        Dt_Productos.Rows.InsertAt(Registro, i); // Se Inserta el Registro
                }
                else
                {
                    Registro["SUBTOTAL"] = "0.0"; // Se agregan los montos a la tabla.
                    Registro["MONTO_IVA"] = "0.0";
                    Registro["TOTAL"] = "0.0";
                }
            }
            return Dt_Productos;
         }
          catch (Exception ex)
          {
              throw new Exception(ex.Message, ex);
          }
        }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Validacion_Cantidad_Entregar
    /// DESCRIPCION:            Método utilizado para validad que los productos entregados
    ///                         sean menor a los solicitados
    ///                         
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            23/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    private Boolean Validacion_Cantidad_Entregar()
    {
        Boolean Validacion = true;

     try
        {
            for (int i = 0; i < Grid_Productos_Requisicion.Rows.Count; i++)
            {
                Double Cantidad_Solicitada = 0;
                Double Cantidad_Entregada = 0;
                Double Cantidad_A_Entregar = 0;
                Double Cantidad_Tatal = 0;

                Cantidad_Solicitada = Convert.ToInt64(Grid_Productos_Requisicion.Rows[i].Cells[4].Text.Trim());
                Cantidad_Entregada = Convert.ToInt64(Grid_Productos_Requisicion.Rows[i].Cells[5].Text.Trim());


                TextBox Txt_Cantidad_A_Entregar = (TextBox)Grid_Productos_Requisicion.Rows[i].Cells[6].FindControl("Txt_Cantidad_A_Entregar");

                if (Txt_Cantidad_A_Entregar.Text.Trim() != "")
                    Cantidad_A_Entregar = Convert.ToInt64(Txt_Cantidad_A_Entregar.Text.Trim());

                Cantidad_Tatal = Cantidad_A_Entregar + Cantidad_Entregada;


                if (Cantidad_Tatal > Cantidad_Solicitada)
                {
                    Validacion = false;
                    Lbl_Informacion.Text = "La Cantidad a Entregar no puede ser mayor a la Cantidad Solicitada";
                    Div_Contenedor_Msj_Error.Visible = true;
                    return Validacion;
                }
            }

            return Validacion;

        }
          catch (Exception ex)
          {
              throw new Exception(ex.Message, ex);
          }
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Llenar_Combo_Empleados_UR
    /// DESCRIPCION:            Método utilizado para llenar el combo con los empleados de la unidad responsable                  
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            23/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    private void Llenar_Combo_Empleados_UR()
    {
        Consulta_Requisiciones = new Cls_Ope_Com_Alm_Requisiciones_Stock_Negocio();
        DataTable Dt_Empleados_UR = new DataTable();

      try
        {
            String No_Requisicion = Session["No_Requisicion_RQ"].ToString();
            Consulta_Requisiciones.P_No_Requisicion = No_Requisicion.Trim();

            Consulta_Requisiciones.P_Tipo_Data_Table = "EMPLEADOS_UR";
            Dt_Empleados_UR = Consulta_Requisiciones.Consultar_DataTable(); // Se consultan los empleados de la unidad responzable

            if (Dt_Empleados_UR.Rows.Count > 0)
            {
                // Se crea una fila para agregar la palabra "SELECCIONE"
                DataRow Fila_Empleados= Dt_Empleados_UR.NewRow();
                Fila_Empleados["EMPLEADO_ID"] = "SELECCIONE";
                Fila_Empleados["EMPLEADO"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
                Dt_Empleados_UR.Rows.InsertAt(Fila_Empleados, 0);

                Cmb_Empleados_UR.DataSource = Dt_Empleados_UR;
                Cmb_Empleados_UR.DataValueField = "EMPLEADO_ID";
                Cmb_Empleados_UR.DataTextField = "EMPLEADO";
                Cmb_Empleados_UR.DataBind();

            // Se le agrega un ToolTip a cada elemento del combo, ya que los valores estan muy grandes.
            if (Cmb_Empleados_UR != null)
                foreach (ListItem li in Cmb_Empleados_UR.Items)
                    li.Attributes.Add("title", li.Text);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }



    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Llenar_Combo_Empleado
    /// DESCRIPCION:            Método utilizado para seleccionar del combo el numero de empleado a consultar                 
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            23/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    private void Llenar_Combo_Empleado( String No_Empleado)
    {
        Consulta_Requisiciones = new Cls_Ope_Com_Alm_Requisiciones_Stock_Negocio();
        DataTable Dt_Empleados_UR = new DataTable();

        try
        {
            Cmb_Empleados_UR.DataSource = Dt_Empleados_UR; // Limpia el Combo
            Cmb_Empleados_UR.DataBind();

            Consulta_Requisiciones.P_No_Empleado = No_Empleado.Trim();

            Consulta_Requisiciones.P_Tipo_Data_Table = "EMPLEADOS";
            Dt_Empleados_UR = Consulta_Requisiciones.Consultar_DataTable(); // Se consultan el empleado

            if (Dt_Empleados_UR.Rows.Count > 0)
            {
                // Se crea una fila para agregar la palabra "SELECCIONE"
                DataRow Fila_Empleados = Dt_Empleados_UR.NewRow();
                Fila_Empleados["EMPLEADO_ID"] = "SELECCIONE";
                Fila_Empleados["EMPLEADO"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
                Dt_Empleados_UR.Rows.InsertAt(Fila_Empleados, 0);
                Cmb_Empleados_UR.DataSource = Dt_Empleados_UR;
                Cmb_Empleados_UR.DataValueField = "EMPLEADO_ID";
                Cmb_Empleados_UR.DataTextField = "EMPLEADO";
                Cmb_Empleados_UR.DataBind();

                Cmb_Empleados_UR.SelectedIndex = 1;
                // Se le agrega un ToolTip a cada elemento del combo, ya que los valores estan muy grandes.
                if (Cmb_Empleados_UR != null)
                    foreach (ListItem li in Cmb_Empleados_UR.Items)
                        li.Attributes.Add("title", li.Text);

                Div_Contenedor_Msj_Error.Visible = false;
            }
            else
            {
                Lbl_Informacion.Text = "No se Encontró el Empleado con el Número Asignado";
                Div_Contenedor_Msj_Error.Visible = true;
                
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }



    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Determinar_Estatus
    /// DESCRIPCION:            Método utilizado para determinar si una requicisión se estrego completa o parcial                  
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            25/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    private String Determinar_Estatus()
    {
        Boolean Bandera = true;
        String Estatus = "";

      try
        {
            for (int i = 0; i < Grid_Productos_Requisicion.Rows.Count; i++)
            {
                Double Cantidad_Solicitada = 0;
                Double Cantidad_Entregada = 0;
                Double Cantidad_A_Entregar = 0;
                Double Cantidad_Tatal = 0;

                // Ya estan estos actualizados
                Cantidad_Solicitada = Convert.ToInt64(Grid_Productos_Requisicion.Rows[i].Cells[4].Text.Trim()); // Se optiene la Cantidad Solicitada
                Cantidad_Entregada = Convert.ToInt64(Grid_Productos_Requisicion.Rows[i].Cells[5].Text.Trim()); // Se optiene la Cantidad Solicitada

                TextBox Txt_Cantidad_A_Entregar = (TextBox)Grid_Productos_Requisicion.Rows[i].Cells[6].FindControl("Txt_Cantidad_A_Entregar");
                if (Txt_Cantidad_A_Entregar.Text.Trim() != "")
                    Cantidad_A_Entregar = Convert.ToInt64(Txt_Cantidad_A_Entregar.Text); // Se optiene la cantidad Entregada
                else
                    Cantidad_A_Entregar = 0;
                Cantidad_Tatal = Cantidad_A_Entregar + Cantidad_Entregada;
                if (Cantidad_Tatal < Cantidad_Solicitada)
                {
                    Bandera = false;
                    Estatus = "PARCIAL";
                    return Estatus;
                }
            }

            if (Bandera == true)
                Estatus = "CERRADA";
            else
                Estatus = "PARCIAL";

            return Estatus;
        }
         catch (Exception ex)
         {
             throw new Exception(ex.Message, ex);
         }
    }

   
    #endregion

    // Evento utilizado para realizar la busqueda del Empleado
    protected void Btn_Buscar_Empleado_Click(object sender, ImageClickEventArgs e)
    {
        if (Txt_Numero_Empleado.Text.Trim() != "")
        {
            Div_Contenedor_Msj_Error.Visible = false;
            Llenar_Combo_Empleado(String.Format("{0:000000}", Convert.ToInt32(Txt_Numero_Empleado.Text.Trim())));
        }
        else
        {
            Lbl_Informacion.Text = "Asignar el Número de Empleado a Consultar";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }    
}
