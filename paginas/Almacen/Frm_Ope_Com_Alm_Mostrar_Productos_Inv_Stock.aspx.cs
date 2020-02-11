using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Administrar_Stock.Negocios;
using Presidencia.Sessiones;
using Presidencia.Reportes;

public partial class paginas_Compras_Frm_Ope_Com_Alm_Mostrar_Productos_Inv_Stock : System.Web.UI.Page
{

    # region Variables
        DataSet Data_Set_Consulta_Inventarios = new DataSet(); // Objeto creado para guardar los datos provenientes de alguna consulta
        Cls_Ope_Com_Alm_Administrar_Stock_Negocios Stock_Negocios = new Cls_Ope_Com_Alm_Administrar_Stock_Negocios(); // Objeto de la capa de Negocios
    # endregion
   
    #region Page_Load
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            String No_Inventario = HttpUtility.HtmlDecode(Request.QueryString["No_Inventario"]).Trim();
            String Estatus = HttpUtility.HtmlDecode(Request.QueryString["Estatus"]).Trim();
            String Mostrar_Botones = HttpUtility.HtmlDecode(Request.QueryString["Mostrar_Botones"]).Trim();
            String Pagina_GI = HttpUtility.HtmlDecode(Request.QueryString["Pagina_GI"]).Trim();

            Session["Pagina_GI"] = Pagina_GI;
            Session["MOSTRAR_BOTONES"] = Mostrar_Botones;
            Session["ESTATUS"] = Estatus;

            if ((Estatus == "CANCELADO") | (Estatus == "APLICADO"))
            {
                Btn_Cancelar_Inventario.Visible = false;
            }
            else if (Estatus == "CAPTURADO")
            {
                Btn_Modificar_Captura.Visible = true;
            }
            Session["Estatus"] = Estatus;
            Session["No_Inventario"] = No_Inventario;
            Mostrar_Productos_Inventario();


            if (Mostrar_Botones == "false")
            {
                Estatus_Botones(false);
            }
        }
    }

    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Productos_Inventario
    ///DESCRIPCIÓN:          Carga el data set físico con el cual se genera el Reporte especificado
    ///PARAMETROS:           
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           24/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Mostrar_Productos_Inventario()
    {
        try
        { 
            Data_Set_Consulta_Inventarios = Llenar_DataSet_Inventarios_Stock();
            Session["Data_Set_Consulta_Inventarios"] = Data_Set_Consulta_Inventarios;

            if (Data_Set_Consulta_Inventarios.Tables[0].Rows.Count != 0)
            {
                DateTime Fecha_Convertida = new DateTime();
                String Fecha = Data_Set_Consulta_Inventarios.Tables[0].Rows[0]["FECHA"].ToString();
                Fecha_Convertida = Convert.ToDateTime(Fecha);
                Txt_Fecha.Text = String.Format("{0:dd/MMM/yyyy}", Fecha_Convertida);
                Txt_Observaciones.Text = HttpUtility.HtmlDecode(Data_Set_Consulta_Inventarios.Tables[0].Rows[0]["OBSERVACIONES"].ToString().Trim());
                Txt_Usuario_Creo.Text = HttpUtility.HtmlDecode(Data_Set_Consulta_Inventarios.Tables[0].Rows[0]["USUARIO_CREO"].ToString());
                Lbl_No_Inventario.Text = "No. Inventario "+ Session["No_Inventario"].ToString();

                if (Session["Estatus"] != null)
                {
                    Txt_Estatus.Text = Session["Estatus"].ToString();

                    if (Session["Estatus"].ToString() == "CAPTURADO")
                    {
                        Grid_Productos_Inventario.Columns[4].Visible = true; // Se pone visible la columna "CONTADOS_USUARIO" YA
                        Grid_Productos_Inventario.Columns[5].Visible = true; // Se pone visible la columna "DIFERENCIA"

                    }
                    else if (Session["Estatus"].ToString() == "APLICADO")
                    {
                        Grid_Productos_Inventario.Columns[4].Visible = true;// Se pone visible la columna "CONTADOS_USUARIO"
                        Grid_Productos_Inventario.Columns[5].Visible = true;// Se pone visible la columna "DIFERENCIA"
                    }
                    else if (Session["Estatus"].ToString() == "CANCELADO") // Esta pendiente que información se va a ver
                    {
                        Grid_Productos_Inventario.Columns[4].Visible = true; // Se pone visible la columna "CONTADOS_USUARIO"
                    }
                }
                Grid_Productos_Inventario.DataSource = Data_Set_Consulta_Inventarios;
                Grid_Productos_Inventario.DataBind();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar los productos del inventario. Error: [" + Ex.Message + "]");
        }
    }


    public void Estatus_Botones(Boolean Estatus){
        Btn_Cancelar_Inventario.Visible = Estatus;
        Btn_Modificar_Captura.Visible = Estatus;
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_DataSet_Inventarios_Stock
    ///DESCRIPCIÓN:          Función utilizada para consultar los inventarios
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernándz Ramírez
    ///FECHA_CREO:           27/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public DataSet Llenar_DataSet_Inventarios_Stock(){
        DataSet Data_Set_Consulta = new DataSet();

        try
        {
            if (Session["No_Inventario"] != null)
            Stock_Negocios.P_No_Inventario = Session["No_Inventario"].ToString();

            Data_Set_Consulta = Stock_Negocios.Consulta_Inventarios_General();
            return Data_Set_Consulta;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los inventarios. Error: [" + Ex.Message + "]");
        }
    }



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN:          Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///                      1.- Estatus: Estatus en el que se cargara la configuración de los 
    ///                      controles.
    ///CREO:                 Salvador Hernándz Ramírez
    ///FECHA_CREO:           27/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Estatus)
    {
        if (Estatus)
        {
            Btn_Imprimir.AlternateText = "Guardar Edición";
            Btn_Imprimir.ToolTip = "Guardar Edición";
            Btn_Imprimir.OnClientClick = "return confirm('¿Desea guardar el inventario?');";
            Btn_Imprimir.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
            Btn_Cancelar_Inventario.AlternateText = "Cancelar Edición";
            Btn_Cancelar_Inventario.ToolTip = "Cancelar Edición";
            Btn_Cancelar_Inventario.OnClientClick = "return confirm('¿Desea cancelar la operación?');";
            Btn_Cancelar_Inventario.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
            Btn_Modificar_Captura.Visible = false;
            Btn_Imprimir_Excel.Visible = false;
        }
        else
        {
            Btn_Imprimir.AlternateText = "Imprimir PDF";
            Btn_Imprimir.ToolTip = "Exportar PDF";
            Btn_Imprimir.OnClientClick = "return confirm('¿Desea Imprimir el inventario?');";
            Btn_Imprimir.ImageUrl = "~/paginas/imagenes/paginas/icono_rep_pdf.png";
            Btn_Imprimir.Visible = true;   
            Btn_Imprimir_Excel.Visible = true;
            Btn_Atras.Visible = true;
            Btn_Modificar_Captura.Visible = true;

            if (Session["MOSTRAR_BOTONES"].ToString().Trim()=="false")  // Si el usuario no tiene permitido que modifique ni actualice
            {
                Estatus_Botones(false);
            }
        }
    }
    #endregion  

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Click
    ///DESCRIPCIÓN:          Evento utilizado para llenar las tablas utilizadas para mostrar el reporte en PDF
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernándz Ramírez
    ///FECHA_CREO:           16/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
            if (Btn_Imprimir.AlternateText == "Imprimir PDF")
            {
                try
                {
                    String Formato = "PDF";
                    Determinar_Reporte(Formato);
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al determinar el reporte. Error: [" + Ex.Message + "]");
                }
            }
            else if (Btn_Imprimir.AlternateText == "Guardar Edición")
            {
                if (Validar_TextBox_Grid())
                {
                    DataTable Data_Table_Productos = new DataTable();
                    Data_Table_Productos.Columns.Add("PRODUCTO_ID");
                    Data_Table_Productos.Columns.Add("CONTADOS_USUARIO");
                    Data_Table_Productos.Columns.Add("DIFERENCIA");
                    Data_Table_Productos.Columns.Add("MARBETE");
                    String No_Inventario = "";

                    if (Session["No_Inventario"] != null)
                    No_Inventario = Session["No_Inventario"].ToString();

                    Lbl_No_Inventario.Text = No_Inventario;
                    Stock_Negocios.P_No_Inventario = No_Inventario.Trim();

                    DataTable Tabla_Productos_Capturados = new DataTable();
                    DataSet Data_Set_Captura = new DataSet();

                    if (Session["Data_Set_Consulta_Inventarios"] != null)
                    Data_Set_Captura = (DataSet)Session["Data_Set_Consulta_Inventarios"];

                    Tabla_Productos_Capturados = Data_Set_Captura.Tables[0];

                    for (int i = 0; i < Tabla_Productos_Capturados.Rows.Count; i++)  // En este for() Se llena la tabla "Data_Table_Productos"
                    {
                        TextBox Txt_Cantidad_src = (TextBox)Grid_Modificar_Captura.Rows[i].FindControl("Txt_Cantidad");
                        Double Cantidad = Convert.ToDouble(Txt_Cantidad_src.Text);
                        Double Existencia = Convert.ToDouble(Tabla_Productos_Capturados.Rows[i]["EXISTENCIA"].ToString().Trim());
                        String Marbete = Tabla_Productos_Capturados.Rows[i]["MARBETE"].ToString();
                        String Producto_Id = Tabla_Productos_Capturados.Rows[i]["PRODUCTO_ID"].ToString();

                        Double Diferencia = 0;
                        if (Cantidad >= Existencia)
                        {
                            Diferencia = (Cantidad - Existencia);
                        }
                        else
                        {
                            Diferencia = (Existencia - Cantidad);
                        }
                        DataRow Registro = Data_Table_Productos.NewRow();
                        Registro["PRODUCTO_ID"] = Producto_Id;
                        Registro["CONTADOS_USUARIO"] = Cantidad;
                        Registro["DIFERENCIA"] = Diferencia;
                        Registro["MARBETE"] = Marbete;
                        Data_Table_Productos.Rows.InsertAt(Registro, i);
                    }
                    Stock_Negocios.P_Datos_Productos = Data_Table_Productos;

                    if (Session["No_Inventario"] != null)
                    Stock_Negocios.P_No_Inventario = Session["No_Inventario"].ToString();

                    Stock_Negocios.P_Estatus = "CAPTURADO";
                    Stock_Negocios.P_Tipo_Ajuste = "MODIFICÓ CAPTURA";
                    Stock_Negocios.P_Usuario_Modifico = Cls_Sessiones.Nombre_Empleado;
                    Stock_Negocios.P_No_Empleado = Cls_Sessiones.No_Empleado;

                    if (Txt_Justificacion.Text.Length > 2000)
                    {
                        Stock_Negocios.P_Justificacion = Txt_Justificacion.Text.Substring(0, 2000);
                    }
                    else
                    {
                        Stock_Negocios.P_Justificacion = Txt_Justificacion.Text;
                    }
                    Stock_Negocios.Guardar_Inventarios_Capturado(); // Se llama al metodo utilizado para  guardar la captura

                    if (Session["Pagina_GI"].ToString() != null)
                    {
                        String ruta = "../Almacen/Frm_Ope_Com_Alm_Generar_Inventario_Stock.aspx?PAGINA=" + Session["Pagina_GI"].ToString().Trim();
                        Response.Redirect(ruta);
                    }
                }
            }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Excel_Click
    ///DESCRIPCIÓN:          Evento utilizado para instanciar el método que muestra el reporte en excel 
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernándz Ramírez
    ///FECHA_CREO:           16/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Imprimir_Excel_Click(object sender, ImageClickEventArgs e)
    {

        String Formato = "Excel";
        Determinar_Reporte(Formato);
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_TextBox_Grid
    ///DESCRIPCIÓN:          Valida que el usuario capture las cantidades de los productos en todos los TextBox
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernándz Ramírez
    ///FECHA_CREO:           27/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public bool Validar_TextBox_Grid()
    {
        Boolean Validacion = true;
        for (int i = 0; i < Grid_Modificar_Captura.Rows.Count; i++)
        {
            TextBox Txt_Cantidad_Capturada = (TextBox)Grid_Modificar_Captura.Rows[i].FindControl("Txt_Cantidad");
            if (Txt_Cantidad_Capturada.Text.Trim() == "")
            {
                Lbl_Mensaje_Error.Text = " Asignar los productos contados";
                Div_Contenedor_Msj_Error.Visible = true;
                Validacion = false;
                return Validacion;
            }
        }
            if (Txt_Justificacion.Text.Trim() == "")
            {
                Lbl_Mensaje_Error.Text = " Favor de escribir las observaciones";
                Div_Contenedor_Msj_Error.Visible = true;
                Validacion = false;
                return Validacion;
            }
        return Validacion;
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Determinar_Reporte
    ///DESCRIPCIÓN:          Metodo utilizado para determinar el tipo de reporte que se va a mostrar
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernándz Ramírez
    ///FECHA_CREO:           16/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Determinar_Reporte(String Formato)
    {
        String Nombre_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";
        Ds_Alm_Com_Inventario_Stock Ds_Reporte_Stock = new Ds_Alm_Com_Inventario_Stock();

        if (Session["Data_Set_Consulta_Inventarios"] != null)
            Data_Set_Consulta_Inventarios = (DataSet)Session["Data_Set_Consulta_Inventarios"];

        
        if (Session["Estatus"] != null)
        {
            if (Session["Estatus"].ToString() == "PENDIENTE")
            {
                Nombre_Reporte_Crystal = "Rpt_Alm_Com_Rep_Generacion_Stock.rpt";
                Nombre_Reporte_Generar = "Rpt_Inventario_Stock_Almacen";
            }
            else if (Session["Estatus"].ToString() == "CAPTURADO")
            {
                Nombre_Reporte_Crystal = "Rpt_Alm_Com_Rep_Captura_Stock.rpt";
                Nombre_Reporte_Generar = "Rpt_Captura_Stock_Almacen";
            }
            else if ((Session["Estatus"].ToString() == "APLICADO") | (Session["Estatus"].ToString() == "CANCELADO"))
            {
                Nombre_Reporte_Crystal = "Rpt_Alm_Com_Rep_Comparativo_Stock.rpt";
                Nombre_Reporte_Generar = "Rpt_Comparativo_Stock_Almacen";
            }
        }
        Generar_Reporte(Data_Set_Consulta_Inventarios, Ds_Reporte_Stock, Nombre_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN:          Carga el data set físico con el cual se genera el Reporte especificado
    ///PARAMETROS:           1.- Data_Set_Consulta_Inventario.- Contiene la informacion de la consulta a la base de datos
    ///                      2.- Ds_Reporte_Stock.- Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///                      3.- Nombre_Reporte_Crystal.- contiene el nombre del Reporte que se realizó en crystalReport
    ///                      4.- Nombre_Reporte_Generar.- Es el nombre del documento que se va a generar en PDF o en Excel
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           14/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Generar_Reporte(DataSet Data_Set_Consulta_Inventario, DataSet Ds_Reporte_Stock, String Nombre_Reporte_Crystal, String Nombre_Reporte_Generar, String Formato)
    {
        String Ruta_Reporte_Crystal = "";
        DataRow Renglon;

      try
        {
            // Se llena la tabla Cabecera del DataSet
            Renglon = Data_Set_Consulta_Inventario.Tables[0].Rows[0];
            Ds_Reporte_Stock.Tables[1].ImportRow(Renglon);

            // Se llena la tabla Detalles del DataSet
            for (int Cont_Elementos = 0; Cont_Elementos < Data_Set_Consulta_Inventario.Tables[0].Rows.Count; Cont_Elementos++)
            {
                Renglon = Data_Set_Consulta_Inventario.Tables[0].Rows[Cont_Elementos]; //Instanciar renglon e importarlo
                Ds_Reporte_Stock.Tables[0].ImportRow(Renglon);
            }

            // Ruta donde se encuentra el reporte Crystal
            Ruta_Reporte_Crystal = "../Rpt/Almacen/" + Nombre_Reporte_Crystal;

            // Se crea el nombre del reporte
            String Nombre_Reporte =  Nombre_Reporte_Generar + "_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

            // Se da el nombre del reporte que se va generar
            if (Formato == "PDF")
                Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
            else if (Formato == "Excel")
                Nombre_Reporte_Generar = Nombre_Reporte + ".xls";  // Es el nombre del repote en Excel que se va a generar

            Cls_Reportes Reportes = new Cls_Reportes();
            Reportes.Generar_Reporte(ref Ds_Reporte_Stock, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
            Mostrar_Reporte( Nombre_Reporte_Generar, Formato);
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Atras_Click 
    ///DESCRIPCIÓN:          Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///                     1.- Estatus. Estatus en el que se cargara la configuración de los 
    ///                     controles.
    ///CREO:                Salvador Hernándz Ramírez
    ///FECHA_CREO:          27/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Atras_Click(object sender, ImageClickEventArgs e)
    {
        if (Session["Pagina_GI"].ToString() != null)
        {
            String ruta = "../Almacen/Frm_Ope_Com_Alm_Generar_Inventario_Stock.aspx?PAGINA="+Session["Pagina_GI"].ToString().Trim();
            Response.Redirect(ruta);
        }
    }

    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cancelar_Inventario_Click
    ///DESCRIPCIÓN:          Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///                      1.- Estatus. Estatus en el que se cargara la configuración de los 
    ///                         controles.
    ///CREO:                 Salvador Hernándz Ramírez
    ///FECHA_CREO:           27/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Cancelar_Inventario_Click(object sender, ImageClickEventArgs e)
    {
            // Nota: No se agrego bloque TryCatch por el Response.Redirect
            if (Btn_Cancelar_Inventario.AlternateText == "Cancelar Inventario")
            {
                Btn_Atras.AlternateText = "Salir";
                Btn_Atras.ToolTip = "Salir";
                
                Txt_Justificacion.Text = "";

                if (Session["ESTATUS"].ToString() == "PENDIENTE")
                {
                    Btn_Modificar_Captura.Visible = true;
                }
                Grid_Productos_Inventario.Visible = false;
                Estado_Botones_Cancel_Inventario();
                Div_Justificación.Visible = true;
                Lbl_No_Inventario.Text = "Cancelando el Inventario  " + Session["No_Inventario"].ToString();
            }
            else if (Btn_Cancelar_Inventario.AlternateText == "Aceptar") // Entra a este apartado cuando se acepta cancela inventario
            {
                if(Txt_Justificacion.Text.Trim() != ""){

                    if (Session["No_Inventario"] != null)
                    Stock_Negocios.P_No_Inventario = Session["No_Inventario"].ToString();

                    Stock_Negocios.P_Estatus = "CANCELADO";
                    Stock_Negocios.P_Tipo_Ajuste = "CANCELÓ";
                    Stock_Negocios.P_Usuario_Modifico = Cls_Sessiones.Nombre_Empleado;
                    Stock_Negocios.P_No_Empleado = Cls_Sessiones.No_Empleado;
                    Stock_Negocios.P_Justificacion = Txt_Justificacion.Text;
                    Stock_Negocios.Cambiar_Estatus();

                    if (Session["Pagina_GI"].ToString() != null)
                    {
                        String ruta = "../Almacen/Frm_Ope_Com_Alm_Generar_Inventario_Stock.aspx?PAGINA=" + Session["Pagina_GI"].ToString().Trim();
                        Response.Redirect(ruta);
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Inventario Cancelado", "alert('Se Cancelo el Inventario  " + Session["No_Inventario"].ToString() + "');", true);
                }else{
                    Lbl_Mensaje_Error.Text = " Favor de escribir las observaciones";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else if (Btn_Cancelar_Inventario.AlternateText == "Cancelar Edición")
            {
                Div_Justificación.Visible = false;
                Grid_Modificar_Captura.Visible = false;
                Grid_Productos_Inventario.Visible = true;
                Configuracion_Formulario(false);
                Lbl_No_Inventario.Text = "Inventario " + Session["No_Inventario"].ToString();
                Btn_Cancelar_Inventario.AlternateText = "Cancelar Inventario";
                Btn_Cancelar_Inventario.ToolTip = "Cancelar Inventario";
                Btn_Cancelar_Inventario.OnClientClick = "return confirm('¿Está seguro de cancelar el inventario?');";
                Btn_Cancelar_Inventario.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar.png";
            }
        }
    


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estado_Botones_Cancel_Inventario
    ///DESCRIPCIÓN:          Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///                      1. Estatus. Estatus en el que se cargara la configuración de los 
    ///                         controles.
    ///CREO:                 Salvador Hernándz Ramírez
    ///FECHA_CREO:           27/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Estado_Botones_Cancel_Inventario(){ 
        Btn_Cancelar_Inventario.AlternateText = "Aceptar";
        Btn_Cancelar_Inventario.ToolTip = "Aceptar";
        Btn_Cancelar_Inventario.ImageUrl = "~/paginas/imagenes/gridview/grid_docto.png";
        Btn_Modificar_Captura.AlternateText = "Cancelar proceso";
        Btn_Modificar_Captura.ToolTip = "Cancelar proceso";
        Btn_Modificar_Captura.ImageUrl = "~/paginas/imagenes/paginas/delete.png";
        Btn_Modificar_Captura.OnClientClick = "return confirm('¿Está seguro de terminar el proceso?');";
        Btn_Imprimir.Visible = false;
        Btn_Imprimir_Excel.Visible = false;
    }



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:  Btn_Modificar_Captura_Click
    ///DESCRIPCIÓN:           Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///                       1.- Estatus. Estatus en el que se cargara la configuración de los 
    ///                       controles.
    ///CREO:                  Salvador Hernándz Ramírez
    ///FECHA_CREO:            27/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Captura_Click(object sender, ImageClickEventArgs e)
    {
        Txt_Justificacion.Text = "";
        Btn_Atras.AlternateText = "Salir";
        Btn_Atras.ToolTip = "Salir";

        try
        {
            if (Btn_Modificar_Captura.AlternateText == "Modificar Captura")
            {
                Div_Justificación.Visible = true;
                Grid_Modificar_Captura.Visible = true;
                Pnl_Modificar_Capturar.Visible = true;
                Configuracion_Formulario(true);
                Grid_Productos_Inventario.Visible = false;
                DataSet Data_Set_Captura = new DataSet();

                if (Session["Data_Set_Consulta_Inventarios"] != null)
                Data_Set_Captura = (DataSet)Session["Data_Set_Consulta_Inventarios"];

                Session["Data_Set_Captura"] = Data_Set_Captura;
                Lbl_No_Inventario.Text = "Modificando el Inventario  " + Session["No_Inventario"].ToString();


                // Se revisa la información que tiene el Grid y de esta manera se hace mas grande el panel o mas pequeño
                if (Data_Set_Captura.Tables[0].Rows.Count > 0)
                {
                    Grid_Modificar_Captura.DataSource = Data_Set_Captura;
                    Grid_Modificar_Captura.DataBind();

                    if (Data_Set_Captura.Tables[0].Rows.Count > 3)
                    {
                        Pnl_Modificar_Capturar.Height = System.Web.UI.WebControls.Unit.Pixel(250);
                    }
                    else
                    {
                        Pnl_Modificar_Capturar.Height = System.Web.UI.WebControls.Unit.Pixel(115);
                    }
                }

                for (int i = 0; i < Data_Set_Captura.Tables[0].Rows.Count; i++)
                {
                    String Contados_Usuario = Data_Set_Captura.Tables[0].Rows[i][8].ToString();
                    TextBox Temporal = (TextBox)Grid_Modificar_Captura.Rows[i].FindControl("Txt_Cantidad");

                    if (Temporal != null)
                    {
                        //int y;
                        //if (int.TryParse(Contados_Usuario, out y))
                        //{
                        //    String Valor = Contados_Usuario + ".00";
                        //    Temporal.Text = Valor;
                        //}
                        //else
                        //{
                        //    Temporal.Text = Contados_Usuario;
                        //}
                        Temporal.Text = Contados_Usuario;
                    }
                }
            }
            else if (Btn_Modificar_Captura.AlternateText == "Cancelar proceso")
            {
                Div_Justificación.Visible = false;
                Grid_Productos_Inventario.Visible = true;
                Lbl_No_Inventario.Visible = true;
                Div_Contenedor_Msj_Error.Visible = false;
                Configuracion_Formulario(false);

                Btn_Cancelar_Inventario.AlternateText = "Cancelar Inventario";
                Btn_Cancelar_Inventario.ToolTip = "Cancelar Inventario";
                Btn_Cancelar_Inventario.OnClientClick = "return confirm('¿Está seguro de cancelar el inventario?');";
                Btn_Cancelar_Inventario.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar.png";

                Btn_Modificar_Captura.AlternateText = "Modificar Captura";
                Btn_Modificar_Captura.ToolTip = "Modificar Captura";
                Btn_Modificar_Captura.OnClientClick = "return confirm('¿Está seguro de modificar la captura?');";
                Btn_Modificar_Captura.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Txt_Justificacion.Text = "";
                Lbl_No_Inventario.Text = "Inventario " + Session["No_Inventario"].ToString();
                
                if (Session["ESTATUS"].ToString() == "PENDIENTE")
                {
                    Btn_Modificar_Captura.Visible = false;
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al modificar la captura. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Aceptar_Aplicacion_Click
    ///DESCRIPCIÓN:          Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///                      1.- Estatus. Estatus en el que se cargara la configuración de los 
    ///                      controles.
    ///CREO:                 Salvador Hernándz Ramírez
    ///FECHA_CREO:           27/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Aceptar_Aplicacion_Click(object sender, EventArgs e)
    {
            if (Session["No_Inventario"] != null)
                Stock_Negocios.P_No_Inventario = Session["No_Inventario"].ToString();

            Stock_Negocios.Cambiar_Estatus();

            if (Session["Pagina_GI"].ToString() != null)
            {
                String ruta = "../Almacen/Frm_Ope_Com_Alm_Generar_Inventario_Stock.aspx?PAGINA=" + Session["Pagina_GI"].ToString().Trim();
                Response.Redirect(ruta);
            }
    }
    # region GRID


    //*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Productos_Inventario_PageIndexChanging
    ///DESCRIPCIÓN:          Maneja el evento para llenar las siguientes páginas del grid con la informaciçon de la consulta
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           21/Febrero/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Productos_Inventario_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        { 
            Grid_Productos_Inventario.PageIndex = e.NewPageIndex;
            if (Session["Data_Set_Consulta_Inventarios"] != null)
            Grid_Productos_Inventario.DataSource = (DataSet)Session["Data_Set_Consulta_Inventarios"];

            Grid_Productos_Inventario.DataBind();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error  en la paginación. Error: [" + Ex.Message + "]");
        }
    }

    # endregion
    #endregion  
}
