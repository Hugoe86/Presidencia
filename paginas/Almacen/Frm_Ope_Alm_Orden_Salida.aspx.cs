using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.Collections.Generic;
using Presidencia.Orden_Salida.Negocio;
using Presidencia.Reportes;


public partial class paginas_Compras_Frm_Ope_Com_Ordenes_Compra : System.Web.UI.Page
{
    #region Variables
    
    Cls_Ope_Com_Alm_Orden_Salida_Negocio Orden_Salida = new Cls_Ope_Com_Alm_Orden_Salida_Negocio();

    #endregion

    #region Page_Load

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["Activa"] = true;
            Chk_Fecha_B.Enabled = true;
            Chk_Fecha_B.Checked = true;

            DateTime _DateTime = DateTime.Now;
            int dias = _DateTime.Day;
            dias = dias * -1;
            dias++;
            _DateTime = _DateTime.AddDays(dias);
            Txt_Fecha_Inicio.Text = _DateTime.ToString("dd/MMM/yyyy").ToUpper();
            Txt_Fecha_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
            Consultar_Requisiciones();
            Carga_Componentes_Busqueda();
            Llenar_Combos_Busqueda();

            if (Btn_Imprimir.Visible)
            {
                Configuracion_Acceso("Frm_Ope_Alm_Orden_Salida.aspx");
                Configurar_Boton_Imprimir();
            }
            Chk_Fecha_B.Enabled = true;
            Chk_Fecha_B.Checked = true;
            Img_Btn_Fecha_Inicio.Enabled = true;
            Img_Btn_Fecha_Fin.Enabled = true;
            Txt_Fecha_Inicio.Text = _DateTime.ToString("dd/MMM/yyyy").ToUpper();
            Txt_Fecha_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
       }
    }

    #endregion

    #region Eventos



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Click
    ///DESCRIPCIÓN: Maneja el evento click del boton "Btn_Imprimir"
    ///PROPIEDADES:     
    ///CREO: Salvador Hernándz Ramírez
    ///FECHA_CREO: 18/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        //String Formato = "PDF";
        //if (Session["ORDEN_SALIDA_OS"].ToString() == "true")
        //{
        String No_Orden_Salida = Session["No_Orden_Salida_OS"].ToString().Trim();
            Mostrar_Orden_Salida(No_Orden_Salida);
        //}
        //else
        //{
        //    Consultar_Ordenes_Salida(Formato);
        //}
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Click
    ///DESCRIPCIÓN: Maneja el evento click del boton "Btn_Imprimir"
    ///PROPIEDADES:     
    ///CREO: Salvador Hernándz Ramírez
    ///FECHA_CREO: 17/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Imprimir_Excel_Click(object sender, ImageClickEventArgs e)
    {
        String Formato = "Excel";

        //if (Session["ORDEN_SALIDA_OS"].ToString() == "true")
        //{
        String No_Orden_Salida = Session["No_Orden_Salida_OS"].ToString().Trim();
            Mostrar_Orden_Salida(No_Orden_Salida);
        //}
        //else
        //{
        //    Consultar_Ordenes_Salida(Formato);
        //}
    }




    

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Maneja el evento click del boton "Btn_Salir"
    ///PROPIEDADES:     
    ///CREO: Salvador Hernándz Ramírez
    ///FECHA_CREO: 18/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText == "Salir")
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Consultar_Requisiciones();
            Configuracion_Formulario(false);
            Btn_Imprimir.Visible = false;
            Btn_Imprimir_Excel.Visible = false;
            
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN: Maneja el evento click del boton "Btn_Buscar"
    ///PROPIEDADES:     
    ///CREO: Salvador Hernándz Ramírez
    ///FECHA_CREO: 18/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Consultar_Requisiciones();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Area_B_CheckedChanged
    ///DESCRIPCIÓN: Maneja el evento click del boton "Chk_Area_B"
    ///PROPIEDADES:     
    ///CREO: Salvador Hernándz Ramírez
    ///FECHA_CREO: 18/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Chk_Area_B_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Area_B.Checked)
            Cmb_Area.Enabled = true;
        else
            Cmb_Area.Enabled = false;

        if (Cmb_Area.Items.Count != 0)
            Cmb_Area.SelectedIndex = 0;
    }

   #region Eventos Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Requisiciones_PageIndexChanging
    ///DESCRIPCIÓN: Maneja el evento para llenar las siguientes páginas del grid con la informaciçon de la consulta
    ///PROPIEDADES:     
    ///CREO: Salvador Hernández Ramírez
    ///FECHA_CREO:  18/Febrero/2011  
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Requisiciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        Grid_Requisiciones.PageIndex = e.NewPageIndex;
        Grid_Requisiciones.DataSource = (DataTable)Session["Dt_Requisiciones_OS"];
        Grid_Requisiciones.DataBind();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Requisiciones_SelectedIndexChanged
    ///DESCRIPCIÓN: Maneja el evento click del boton "Grid_Requisiciones"
    ///PROPIEDADES:     
    ///CREO: Salvador Hernándz Ramírez
    ///FECHA_CREO: 18/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Requisiciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow SelectedRow = Grid_Requisiciones.Rows[Grid_Requisiciones.SelectedIndex];//GridViewRow representa una fila individual de un control gridview
        
        String No_Orden_Salida = Convert.ToString(SelectedRow.Cells[1].Text); // Se optiene el No. Orden Salida
        Session["No_Orden_Salida_OS"] = No_Orden_Salida;
        Orden_Salida.P_No_Orden_Salida = No_Orden_Salida;

        String No_Requisicion = Convert.ToString(SelectedRow.Cells[2].Text); // Se optiene el No. Requsicion
        Session["No_Requisicion_OS"] = No_Requisicion;
        Orden_Salida.P_No_Requisicion = No_Requisicion;

        DataTable Dt_Productos_Requisicion = new DataTable();
        Dt_Productos_Requisicion = Orden_Salida.Consulta_Productos_Orden_Salida(); // Se consultan los productos de la orden de salida
        if (Dt_Productos_Requisicion.Rows.Count > 0)
        {
            Grid_Productos.DataSource = Dt_Productos_Requisicion;
            Grid_Productos.DataBind();
            Session["Dt_Productos_Requisicion_OS"] = Dt_Productos_Requisicion; // Se guarda la tabla en una variable de session
            Mostrar_Datos_Orden_Salida(); // Se llenan los TextBox con la información de la orden de salida seleccionada por el usuario
            Div_Contenedor_Msj_Error.Visible = false;
            Div_Requisiciones.Visible = false;
            Div_Detalles_Requisicion.Visible = true;
            Div_Productos_Requisicion.Visible = true;
            Configuracion_Formulario(true);
            Btn_Imprimir.Visible = true;
            Btn_Imprimir_Excel.Visible = false;//@
            Btn_Print_PDF_Ordenes_Salida.Visible = false;

            Session["ORDEN_SALIDA_OS"] = "true";
            
        }
        else
        {
            Lbl_Informacion.Text = "La Requisición No Contiene Productos";
            Div_Contenedor_Msj_Error.Visible = true;
            Div_Detalles_Requisicion.Visible = false;
            Div_Productos_Requisicion.Visible = false;
            //Btn_Imprimir.Visible = false;
            //Btn_Imprimir_Excel.Visible = false;
            Session["ORDEN_SALIDA_OS"] = "false";
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Productos_PageIndexChanging
    ///DESCRIPCIÓN: Maneja el evento para llenar las siguientes páginas del grid con la informaciçon de la consulta
    ///PROPIEDADES:     
    ///CREO: Salvador Hernández Ramírez
    ///FECHA_CREO: 18/Febrero/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Productos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Productos.PageIndex = e.NewPageIndex;
        Grid_Productos.DataSource = (DataTable)Session["Dt_Productos_Requisicion_OS"];
        Grid_Productos.DataBind();
    }

   #endregion

    #endregion

    #region Métodos


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Requisiciones
    ///DESCRIPCIÓN: Metodo que valida que seleccione un estatus dentro del modalpopup
    ///PARAMETROS:   
    ///CREO: Salvador Hernández Ramírez
    ///FECHA_CREO: 18/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Consultar_Requisiciones(){

        DataTable Dt_Requisiciones = new DataTable();

        if (Txt_OrdenC_Buscar.Text.Trim() != "")
            Orden_Salida.P_No_Orden_Compra = Txt_OrdenC_Buscar.Text.Trim();
        else
            Orden_Salida.P_No_Orden_Compra = null;

        if (Txt_Req_Buscar.Text.Trim() != "")
            Orden_Salida.P_No_Requisicion = Txt_Req_Buscar.Text.Trim();
        else
            Orden_Salida.P_No_Requisicion = null;

        if (Txt_No_OrdenS_Buscar.Text.Trim() != "")
            Orden_Salida.P_No_Orden_Salida = Txt_No_OrdenS_Buscar.Text.Trim();
        else
            Orden_Salida.P_No_Orden_Salida = null;


           if (Chk_Dependencia_B.Checked) // Si esta activado el Check
            {
                if (Cmb_Dependencia.SelectedIndex != 0)
                        Orden_Salida.P_Dependencia = Cmb_Dependencia.SelectedValue.Trim();
                    else
                        Orden_Salida.P_Dependencia = "";
           }

           //if (Chk_Area_B.Checked) // Si esta activado el Check
           // {
           //     if (Cmb_Area.SelectedIndex != 0)
           //             Orden_Salida.P_Area= Cmb_Area.SelectedValue.Trim();
           //     else
           //             Orden_Salida.P_Area = "";
           //}

           if (Chk_Tipo_Salida_B.Checked) // Si esta activado el Check
           {
               if (Cmb_Tipo_Salida.SelectedIndex == 1)
                   Orden_Salida.P_Tipo_Salida = "STOCK";
               else if (Cmb_Tipo_Salida.SelectedIndex == 2)
                   Orden_Salida.P_Tipo_Salida = "TRANSITORIA";
                   else
                   Orden_Salida.P_Tipo_Salida = "";
           }

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
                            Orden_Salida.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim());
                            Orden_Salida.P_Fecha_Final = Formato_Fecha(Txt_Fecha_Fin.Text.Trim());
                            Div_Contenedor_Msj_Error.Visible = false;
                        }
                        else
                        {
                            String Fecha = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim()); //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                            Orden_Salida.P_Fecha_Inicial = Fecha;
                            Orden_Salida.P_Fecha_Final= Fecha;
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



        Dt_Requisiciones= Orden_Salida.Consulta_Requisiciones();

        if (Dt_Requisiciones.Rows.Count > 0)
        {
            Grid_Requisiciones.Columns[7].Visible = true;  //Se pone visible la columna.. Usuario entrego
            Grid_Requisiciones.DataSource = Dt_Requisiciones;
            Grid_Requisiciones.DataBind();
            Session["Dt_Requisiciones_OS"] = Dt_Requisiciones;
            Grid_Requisiciones.Columns[7].Visible = false;
            Div_Requisiciones.Visible=true;
            Div_Productos_Requisicion.Visible = false;
            Div_Detalles_Requisicion.Visible = false;
            Div_Contenedor_Msj_Error.Visible=false;
            Btn_Print_PDF_Ordenes_Salida.Visible = true;
        }
        else
        {
            Lbl_Informacion.Text = "No se encontraron requisiciones surtidas";
            Div_Contenedor_Msj_Error.Visible=true;
            Div_Requisiciones.Visible=false;
            Div_Productos_Requisicion.Visible = false;
            Div_Detalles_Requisicion.Visible = false;
            Btn_Print_PDF_Ordenes_Salida.Visible = false;
            //Btn_Imprimir.Visible = false;
            //Btn_Imprimir_Excel.Visible = false;

        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN: Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:  1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                     en caso de que cumpla la condicion del if
    ///CREO: Salvador Hernández Ramírez
    ///FECHA_CREO: 18/Febrero/2011 
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


    private void Configuracion_Formulario(Boolean Estatus)
    {
        if (Estatus)
        {
            Btn_Salir.AlternateText = "Atrás";
            Btn_Salir.ToolTip = "Atrás";

            Mostrar_Busqueda(false);
            //Btn_Imprimir.Visible = false;
            //Btn_Imprimir_Excel.Visible = false;
        }
        else
        {
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ToolTip = "Salir";

            Mostrar_Busqueda(true);
        }

        if (Btn_Imprimir.Visible)
        {
            Configuracion_Acceso("Frm_Ope_Alm_Orden_Salida.aspx");
            Configurar_Boton_Imprimir();
        }
    }


    public void Configurar_Boton_Imprimir()
    {
        //if (Btn_Imprimir.Visible)
        //    Btn_Imprimir_Excel.Visible = true;
        //else
        //    Btn_Imprimir_Excel.Visible = false;
        Btn_Imprimir_Excel.Visible = false;
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Busqueda
    ///DESCRIPCIÓN:          Método utilizado para mostrar y ocultar los controles
    ///                      utilizados para realizar la búsqueda simble y abanzada
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           12/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Mostrar_Busqueda(Boolean Estatus)
    {
        Div_Busqueda_Av.Visible = Estatus;
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Ordenes_Salida
    ///DESCRIPCIÓN: Método utilizado para consultar las ordenes de salida
    ///PROPIEDADES:     
    ///CREO: Salvador Hernándz Ramírez
    ///FECHA_CREO: 17/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Reporte_Ordenes_Salida(String Formato)
    {
        DataTable Dt_Salidas_Imprimir = new DataTable();
        Ds_Alm_Com_Rep_Ordenes_Salida Ds_Reporte_Ordenes_Salida = new Ds_Alm_Com_Rep_Ordenes_Salida();
        Dt_Salidas_Imprimir = (DataTable)Session["Dt_Requisiciones_OS"];
        Generar_Reporte(Dt_Salidas_Imprimir, Ds_Reporte_Ordenes_Salida, Formato);
    }



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
    private void Generar_Reporte(DataTable Dt_Requisiciones_Imprimir, DataSet Ds_Reporte_Ordenes_Salida, String Formato)
    {
        DataRow Renglon;
        ReportDocument Reporte = new ReportDocument();
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";

        try
        {
            // Se llena la cabecera del DataSet
            Renglon = Dt_Requisiciones_Imprimir.Rows[0];
            Ds_Reporte_Ordenes_Salida.Tables[1].ImportRow(Renglon);

            // Se llenan los detalles del DataSet
            for (int Cont_Elementos = 0; Cont_Elementos < Dt_Requisiciones_Imprimir.Rows.Count; Cont_Elementos++)
            {
                Renglon = Dt_Requisiciones_Imprimir.Rows[Cont_Elementos]; // Instanciar renglon e importarlo
                Ds_Reporte_Ordenes_Salida.Tables[0].ImportRow(Renglon);
            }

            // Ruta donde se encuentra el reporte Crystal
            Ruta_Reporte_Crystal = "../Rpt/Almacen/Rpt_Alm_Com_Rep_Ordenes_Salida.rpt";

            // Se crea el nombre del reporte
            String Nombre_Reporte = "Rpt_Ordenes_Salida_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

            // Se da el nombre del reporte que se va generar
            if (Formato == "PDF")
                Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
            else if (Formato == "Excel")
                Nombre_Reporte_Generar = Nombre_Reporte + ".xls";  // Es el nombre del repote en Excel que se va a generar



            Cls_Reportes Reportes = new Cls_Reportes();
            Reportes.Generar_Reporte(ref Ds_Reporte_Ordenes_Salida, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
            Mostrar_Reporte(Nombre_Reporte_Generar, Formato);
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
    /// FECHA MODIFICO:      17-Mayo-2011
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
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Datos_Requisicion
    ///DESCRIPCIÓN: 
    ///PARAMETROS:         
    ///CREO: Salvador Hernández Ramírez
    ///FECHA_CREO: 18/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Mostrar_Datos_Orden_Salida()
    {
        GridViewRow SelectedRow = Grid_Requisiciones.Rows[Grid_Requisiciones.SelectedIndex];//GridViewRow representa una fila individual de un control gridview

        Txt_Orden_Salida.Text = HttpUtility.HtmlDecode(Convert.ToString(SelectedRow.Cells[1].Text));
        Txt_Numero_Requisicion.Text = Convert.ToString(SelectedRow.Cells[2].Text);
        Txt_Dependencia.Text = HttpUtility.HtmlDecode(Convert.ToString(SelectedRow.Cells[4].Text.Trim()));
        Txt_Area.Text = HttpUtility.HtmlDecode(Convert.ToString(SelectedRow.Cells[5].Text.Trim()));
        Txt_Usuario_Surtio.Text = HttpUtility.HtmlDecode(Convert.ToString(SelectedRow.Cells[7].Text.Trim()));
        Txt_Fecha_Surtio.Text = Convert.ToString(SelectedRow.Cells[8].Text);

        String  Codigo_P = Convert.ToString(SelectedRow.Cells[6].Text.Trim());
        if ( Codigo_P.ToString().Trim() != "")
            Txt_Codigo_P.Text = Convert.ToString(SelectedRow.Cells[6].Text.Trim());
        else
            Txt_Codigo_P.Text = "";

        Txt_Subtotal.Text = Convert.ToString(SelectedRow.Cells[9].Text);
        Txt_Iva.Text = Convert.ToString(SelectedRow.Cells[10].Text);
        Txt_Total.Text = Convert.ToString(SelectedRow.Cells[11].Text);
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
    private void Mostrar_Orden_Salida(String No_Orden_Salida)
    {
        DataTable Dt_Cabecera = new DataTable(); // Contendrá los datos generales arrojados de la consulta
        DataTable Dt_Detalles = new DataTable(); // Contendrá los detalles arrojados de la consulta
        DataTable Dt_Cabecera_Completa = new DataTable();  // Contendra lso datos completos para la cabecera

        Double SubTotal_Req = 0;
        Double IVA_Req = 0;
        Double Total_Req = 0;

         try
             {
                Orden_Salida.P_No_Orden_Salida = No_Orden_Salida;
                Dt_Cabecera = Orden_Salida.Consultar_Informacion_General_OS();
                Dt_Detalles = Orden_Salida.Consultar_Detalles_Orden_Salida(); // Este método consultara al información de la tabla Salidas

                if (Dt_Detalles.Rows.Count > 0)
                {
                    for (int i = 0; i < Dt_Detalles.Rows.Count; i++)
                    {
                        if (Dt_Detalles.Rows[i]["SUBTOTAL"].ToString().Trim() != "")
                        SubTotal_Req = SubTotal_Req + (Convert.ToDouble(Dt_Detalles.Rows[i]["SUBTOTAL"].ToString()));

                        if (Dt_Detalles.Rows[i]["IVA"].ToString().Trim() != "")
                        IVA_Req = IVA_Req + (Convert.ToDouble(Dt_Detalles.Rows[i]["IVA"].ToString()));

                        if (Dt_Detalles.Rows[i]["TOTAL"].ToString().Trim() != "")
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

                Generar_Reporte2(Dt_Cabecera_Completa, Dt_Detalles, Ds_Orden_Salida, Nombre_Reporte_Crystal, Formato);
         }
         catch (Exception ex)
         {
             throw new Exception(ex.Message, ex);
         }
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
    private void Generar_Reporte2(DataTable Dt_Cabecera, DataTable Dt_Detalles, DataSet Ds_Orden_Salida, String Nombre_Reporte_Crystal, String Formato)
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


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Carga_Componentes_Busqueda
    ///DESCRIPCIÓN: Metodo que carga e inicializa los componentes del ModalPopUp
    ///PARAMETROS:   
    ///CREO: Salvador Hernández Ramírez
    ///FECHA_CREO: 20/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Carga_Componentes_Busqueda()
    {
        Chk_Dependencia_B.Checked = false;
        Chk_Fecha_B.Checked = false;
        Chk_Area_B.Checked = false;
        Chk_Tipo_Salida_B.Checked = false;

        Chk_Area_B.Enabled = false;

        if (Cmb_Tipo_Salida.Items.Count != 0)
            Cmb_Tipo_Salida.SelectedIndex = 0;

        if (Cmb_Dependencia.Items.Count != 0)
            Cmb_Dependencia.SelectedIndex = 0;

        if (Cmb_Area.Items.Count != 0)
            Cmb_Area.SelectedIndex = 0;

        Cmb_Dependencia.Enabled = false;
        Cmb_Tipo_Salida.Enabled = false;
        Cmb_Area.Enabled = false;

        Img_Btn_Fecha_Inicio.Enabled = false;
        Img_Btn_Fecha_Fin.Enabled = false;

        Txt_No_OrdenS_Buscar.Text = "";
        Txt_OrdenC_Buscar.Text = "";
        Txt_Req_Buscar.Text = "";
        Txt_Fecha_Inicio.Text = "";
        Txt_Fecha_Fin.Text = "";
    }


    //*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combos_Busqueda
    ///DESCRIPCIÓN: 
    ///PARAMETROS:         
    ///CREO: Salvador Hernández Ramírez
    ///FECHA_CREO: 18/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combos_Busqueda()
    {
        // Se llena el combo de dependencias
        Orden_Salida.P_Tipo_Data_Table = "DEPENDENCIAS";
        DataTable Dependencias = Orden_Salida.Consultar_DataTable();
        DataRow Fila_Dependencia = Dependencias.NewRow();
        Fila_Dependencia["DEPENDENCIA_ID"] = "SELECCIONE";
        Fila_Dependencia["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
        Dependencias.Rows.InsertAt(Fila_Dependencia, 0);
        Cmb_Dependencia.DataSource = Dependencias;
        Cmb_Dependencia.DataValueField = "DEPENDENCIA_ID";
        Cmb_Dependencia.DataTextField = "NOMBRE";
        Cmb_Dependencia.DataBind();


        if (Cmb_Tipo_Salida.Items.Count == 0)
        {
            Cmb_Tipo_Salida.Items.Add("<<SELECCIONAR>>");
            Cmb_Tipo_Salida.Items.Add("STOCK");
            Cmb_Tipo_Salida.Items.Add("TRANSITORIA");
            Cmb_Tipo_Salida.Items[0].Value = "0";
            Cmb_Tipo_Salida.Items[0].Selected = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Estatus_Busqueda
    ///DESCRIPCIÓN: Metodo que valida que seleccione un estatus dentro del modalpopup
    ///PARAMETROS:   
    ///CREO: Salvador Hernández Ramírez
    ///FECHA_CREO: 20/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Validar_Estatus_Busqueda()
    {
        if (Chk_Dependencia_B.Checked == true)
        {
            if (Cmb_Dependencia.SelectedIndex != 0)
            {
                Orden_Salida.P_Dependencia = Cmb_Dependencia.SelectedValue;
            }
            else
                Lbl_Informacion.Text += "+ Debe seleccionar una Dependencia <br />";
        }

        if (Chk_Area_B.Checked == true)
        {
            if (Cmb_Area.SelectedIndex != 0)
                Orden_Salida.P_Area = Cmb_Area.SelectedValue;
            else
                Lbl_Informacion.Text += "+ Debe seleccionar una Área <br />";
        }
    }



   
    #endregion

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
            Botones.Add(Btn_Imprimir);

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
                            Cls_Util.Configuracion_Acceso_Sistema_SIAS_AlternateText(Botones, Dr_Menus[0]); // Habilitamos la configuracón de los botones.
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
    protected void Configuracion_Acceso_LinkButton(String URL_Pagina)
    {
        List<LinkButton> Botones = new List<LinkButton>();//Variable que almacenara una lista de los botones de la página.
        DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

        try
        {
            //Agregamos los botones a la lista de botones de la página.
            //Botones.Add(Btn_Busqueda_Avanzada);

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
            throw new Exception("Error al validar si es un dato numerico. Error [" + Ex.Message + "]");
        }
        return Resultado;
    }
    #endregion
    protected void Chk_Fecha_B_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Fecha_B.Checked)
        {
            Img_Btn_Fecha_Inicio.Enabled = true;
            Img_Btn_Fecha_Fin.Enabled = true;
            Txt_Fecha_Inicio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        }
        else
        {
            Img_Btn_Fecha_Inicio.Enabled = false;
            Img_Btn_Fecha_Fin.Enabled = false;
            Txt_Fecha_Inicio.Text = "";
            Txt_Fecha_Fin.Text = "";
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Dependencia_B_CheckedChanged
    ///DESCRIPCIÓN: Maneja el evento click del boton "Chk_Dependencia_B"
    ///PROPIEDADES:     
    ///CREO: Salvador Hernándz Ramírez
    ///FECHA_CREO: 18/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Dependencia_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cmb_Dependencia.SelectedIndex != 0)
        {
            Chk_Area_B.Enabled = true;
            Llenar_Combo_Areas();
        }else
            Chk_Area_B.Enabled = false;
    }

    protected void Chk_Tipo_Salida_B_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Tipo_Salida_B.Checked)
            Cmb_Tipo_Salida.Enabled = true;
        else
            Cmb_Tipo_Salida.Enabled = false;

        if (Cmb_Tipo_Salida.Items.Count != 0)
            Cmb_Tipo_Salida.SelectedIndex = 0;
    }


    private void Llenar_Combo_Areas()
    {
        Orden_Salida.P_Dependencia = Cmb_Dependencia.SelectedValue;
        Orden_Salida.P_Tipo_Data_Table = "AREAS"; // Se llena el combo de areas
        DataTable Areas = Orden_Salida.Consultar_DataTable();
        DataRow Fila_Areas = Areas.NewRow();
        Fila_Areas["AREA_ID"] = "SELECCIONE";
        Fila_Areas["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
        Areas.Rows.InsertAt(Fila_Areas, 0);
        Cmb_Area.DataSource = Areas;
        Cmb_Area.DataValueField = "AREA_ID";
        Cmb_Area.DataTextField = "NOMBRE";
        Cmb_Area.DataBind();

        if (Cmb_Area.Items.Count != 0)
            Cmb_Area.SelectedIndex = 0;

        Chk_Area_B.Checked = false;
        Cmb_Area.Enabled = false;
    }

    protected void Chk_Dependencia_B_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Dependencia_B.Checked)
            Cmb_Dependencia.Enabled = true;
        else
            Cmb_Dependencia.Enabled = false;

        if (Cmb_Dependencia.Items.Count != 0)
            Cmb_Dependencia.SelectedIndex = 0;
    }

    protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
    {
        Carga_Componentes_Busqueda();
    }

    protected void Btn_Print_PDF_Ordenes_Salida_Click(object sender, ImageClickEventArgs e)
    {
        Reporte_Ordenes_Salida("PDF");
    }
}
