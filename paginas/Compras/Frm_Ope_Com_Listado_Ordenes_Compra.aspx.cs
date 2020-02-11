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
using Presidencia.Listado_Ordenes_Compra;
using Presidencia.Listado_Ordenes_Compra.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Collections.Generic;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Presidencia.Reportes;
using Presidencia.Orden_Compra.Negocio;

public partial class paginas_Compras_Frm_Ope_Com_Listado_Ordenes_Compra : System.Web.UI.Page
{

    # region Variables

    Cls_Ope_Com_Listado_Ordenes_Compra_Negocio Listado_Negocio = new Cls_Ope_Com_Listado_Ordenes_Compra_Negocio(); // Variable utilizada para el accesos a la clase de negocios

    # endregion

    # region  Load

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DateTime _DateTime = DateTime.Now;
            int dias = _DateTime.Day;
            dias = dias * -1;
            dias++;
            _DateTime = _DateTime.AddDays(dias);
            _DateTime = _DateTime.AddMonths(-1);
            Txt_Fecha_Inicial.Text = _DateTime.ToString("dd/MMM/yyyy").ToUpper();
            Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
            Llenar_Combo_Cotizadores();
            Estatus_Inicial();
            //Verificar si su rol es jefe de dependencia, admin de modulo o admin de sistema
            DataTable Dt_Grupo_Rol = Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Rol_ID.ToString());
            if (Dt_Grupo_Rol != null)
            {
                String Grupo_Rol = Dt_Grupo_Rol.Rows[0][Apl_Cat_Roles.Campo_Grupo_Roles_ID].ToString();
                if (Grupo_Rol == "00001" || Grupo_Rol == "00002")
                {
                    Cmb_Cotizadores.Enabled = true;
                }
                else
                {
                    Cmb_Cotizadores.Enabled = false;
                }
            }
        }        
    }

    #endregion

    # region Eventos

    # region Grid

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Ordenes_Compra_PageIndexChanging
    ///DESCRIPCIÓN:          Evento utilizado para manejar la páginación del grid
    ///PARAMETROS:          
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Ordenes_Compra_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Ordenes_Compra.PageIndex = e.NewPageIndex;
        Grid_Ordenes_Compra.DataSource = (DataTable)Session["Dt_Ordenes_Compra"];
        Grid_Ordenes_Compra.Columns[5].Visible = true;
        Grid_Ordenes_Compra.DataBind();
        Grid_Ordenes_Compra.Columns[5].Visible = false;
    }

    # endregion


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN:          Evento utilizado llenar el Grid con las ordenes de compra
    ///PARAMETROS:          
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        //Llenar_Grid_Ordenes_Compra();
        Llenar_Grid_Ordenes_Compra("", "SI",Txt_Busqueda.Text.Trim(),Txt_Fecha_Inicial.Text,Txt_Fecha_Final.Text);
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN:          Evento utilizado para salir de la página
    ///PARAMETROS:          
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Orden_Compra_Click
    ///DESCRIPCIÓN:          Evento utilizado para instanciar los métodos que consultan la 
    ///                      orden de compra seleccionada por el usuario y los productos de esta misma.
    ///PARAMETROS:          
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Imprimir_Orden_Compra_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton Boton = (ImageButton)sender;
        String No_Orden_Compra = "";
        No_Orden_Compra = Boton.CommandArgument;
        if (Boton.ToolTip == "Imprimir")
        {
            
            DataTable Dt_Cabecera_OC = new DataTable();
            DataTable Dt_Detalles_OC = new DataTable();
            ImageButton Btn_Imprimir_Orden_Compra = null;
            String Formato = "PDF";
            try
            {
                Btn_Imprimir_Orden_Compra = (ImageButton)sender;
                No_Orden_Compra = Btn_Imprimir_Orden_Compra.CommandArgument;
                Listado_Negocio.P_No_Orden_Compra = No_Orden_Compra.Trim();
                // Consultar Cabecera de la Orden de compra
                Dt_Cabecera_OC = Listado_Negocio.Consulta_Cabecera_Orden_Compra();
                // Consultar los detalles de la Orden de compra
                Dt_Detalles_OC = Listado_Negocio.Consulta_Detalles_Orden_Compra();
                // Instanciar el DataSet Fisico
                if (Convert.ToInt32(Dt_Cabecera_OC.Rows[0]["TOTAL"]) >= 50000)
                {
                Ds_Ope_Com_Orden_Compra Ds_Orden_Compra = new Ds_Ope_Com_Orden_Compra();
                // Instanciar al método que muestra el reporte
                Generar_Reporte(Dt_Cabecera_OC, Dt_Detalles_OC, Ds_Orden_Compra, Formato, true);
                }
                else
                {
                    Ds_Ope_Com_Orden_Compra Ds_Orden_Compra = new Ds_Ope_Com_Orden_Compra();
                    // Instanciar al método que muestra el reporte
                    Generar_Reporte(Dt_Cabecera_OC, Dt_Detalles_OC, Ds_Orden_Compra, Formato, false);
                }

                Cls_Ope_Com_Orden_Compra_Negocio Negocio_Cmp = new Cls_Ope_Com_Orden_Compra_Negocio();
                Negocio_Cmp.P_No_Orden_Compra = long.Parse(No_Orden_Compra);
                Negocio_Cmp.Actualizar_Impresion();
                Llenar_Grid_Ordenes_Compra("GENERADA','AUTORIZADA','RECHAZADA", "NO", "", "", "");     
            }
            catch (Exception ex)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Informacion.Text = (ex.Message);            
            }
        }
        else
        {
            DataTable Dt = ((DataTable)Session["Dt_Ordenes_Compra"]);
            if (Boton.ToolTip == "Rechazada")
            {                
                DataRow[] _DataRow = ((DataTable)Session["Dt_Ordenes_Compra"]).Select(Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = " + No_Orden_Compra );
                //Boton.ToolTip = _DataRow[0][Ope_Com_Ordenes_Compra.Campo_Comentarios].ToString();
            }
                ScriptManager.RegisterStartupScript(
                   this, this.GetType(), "Requisiciones", "alert('" + Boton.ToolTip + "');", true);            
        }     
    }


    protected void Btn_Info_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton Boton = (ImageButton)sender;

            ScriptManager.RegisterStartupScript(
               this, this.GetType(), "Requisiciones", "alert('" + Boton.ToolTip + "');", true);
        
    }

    # endregion

    # region Métodos
    ///*******************************************************************************
    //NOMBRE DE LA FUNCIÓN: Llenar_Combo_Cotizadores
    //DESCRIPCIÓN:Llenar_Combo_Cotizadores
    //CREO: Gustavo Angeles Cruz
    //FECHA_CREO: 14/Oct/2011 
    //MODIFICO:
    //FECHA_MODIFICO:
    //CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    private void Llenar_Combo_Cotizadores()
    {
        Cls_Ope_Com_Orden_Compra_Negocio Negocio_Compra = new Cls_Ope_Com_Orden_Compra_Negocio();
        DataTable Dt_Cotizadores = Negocio_Compra.Consultar_Cotizadores();
        if (Dt_Cotizadores != null && Dt_Cotizadores.Rows.Count > 0)
        {
            Cls_Util.Llenar_Combo_Con_DataTable_Generico
                (Cmb_Cotizadores, Dt_Cotizadores, Cat_Com_Cotizadores.Campo_Nombre_Completo, Cat_Com_Cotizadores.Campo_Empleado_ID);
            Cmb_Cotizadores.SelectedValue = Cls_Sessiones.Empleado_ID;
        }
        else
        {
            Cmb_Cotizadores.Items.Clear();
            Cmb_Cotizadores.Items.Add("COTIZADORES");
            Cmb_Cotizadores.SelectedIndex = 0;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estatus_Inicial
    ///DESCRIPCIÓN:          Metodo que se utiliza para llenar el combo con los proveedores
    ///                      y llenar el grid con las ordenes de compra
    ///PARAMETROS:          
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Estatus_Inicial()
    {
        //Llenar_Combo_Proveedores();
        Llenar_Grid_Ordenes_Compra("GENERADA','AUTORIZADA','RECHAZADA", "NO", null, null, null);
        Configuracion_Acceso("Frm_Ope_Com_Listado_Ordenes_Compra.aspx");
    }    

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Proveedores
    ///DESCRIPCIÓN:          Metodo en el que se consultan los proveedores
    ///                      y se llena el combo.
    ///PARAMETROS:          
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    //public void Llenar_Combo_Proveedores()
    //{
    //    try
    //    {
    //        Cmb_Proveedor.DataSource = Listado_Negocio.Consultar_Proveedores();
    //        Cmb_Proveedor.DataTextField = Cat_Com_Proveedores.Campo_Compañia;
    //        Cmb_Proveedor.DataValueField = Cat_Com_Proveedores.Campo_Proveedor_ID;
    //        Cmb_Proveedor.DataBind();
    //        Cmb_Proveedor.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
    //        Cmb_Proveedor.SelectedIndex = 0;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.Message, ex);
    //    }
    //}



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Ordenes_Compra
    ///DESCRIPCIÓN:          Evento utilizado para consultar las ordenes de compra
    ///                      y mostrarlas en el Grid
    ///PARAMETROS:          
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Abril/2011 
    ///MODIFICO:             Gustavo Angeles C.
    ///FECHA_MODIFICO:       14 OCT 2011
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Grid_Ordenes_Compra(String Estatus, String Impresa, String No_Orden_Compra, String Fecha_Inicial, String Fecha_Final)
    {
        DataTable Dt_Ordenes_Compra = new DataTable();
        try
        {
            Cls_Ope_Com_Listado_Ordenes_Compra_Negocio Listado_Negocio = new Cls_Ope_Com_Listado_Ordenes_Compra_Negocio();
            Listado_Negocio.P_Cotizador_ID = Cmb_Cotizadores.SelectedValue.Trim();
            Listado_Negocio.P_Estatus = Estatus;//"GENERADA','AUTORIZADA','RECHAZADA";
            Listado_Negocio.P_Impresa = Impresa;//"NO";
            Listado_Negocio.P_No_Orden_Compra = No_Orden_Compra;
            Listado_Negocio.P_Fecha_Inicial = Fecha_Inicial;
            Listado_Negocio.P_Fecha_Final = Fecha_Final;
            Dt_Ordenes_Compra = Listado_Negocio.Consulta_Ordenes_Compra();
            if (Dt_Ordenes_Compra.Rows.Count > 0)
            {
                Session["Dt_Ordenes_Compra"] = Dt_Ordenes_Compra;
                Grid_Ordenes_Compra.DataSource = Dt_Ordenes_Compra;
                Grid_Ordenes_Compra.DataBind();
            }
            else
            {
                Session["Dt_Ordenes_Compra"] = null;
                Grid_Ordenes_Compra.DataSource = null;
                Grid_Ordenes_Compra.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN:          Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:           1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                      en caso de que cumpla la condicion del if
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Abril/2011 
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
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN:          Carga el data set físico con el cual se genera el Reporte especificado
    ///PARAMETROS:           1.- Dt_Cabecera.- Contiene la informacion general de la orden de compra
    ///                      2.- Dt_Detalles.- Contiene los productos de la orden de compra
    ///                      3.- Ds_Recibo.- Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataTable Dt_Cabecera, DataTable Dt_Detalles, DataSet Ds_Reporte, String Formato, bool Tesorero)
    {

        DataRow Renglon;
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";

        DataTable Dt_Dir;
        Cls_Ope_Com_Listado_Ordenes_Compra_Negocio Listado = new Cls_Ope_Com_Listado_Ordenes_Compra_Negocio();
        Dt_Dir = Listado.Consulta_Directores();

        if (Dt_Dir.Rows.Count > 0)
        {
            Dt_Cabecera.Columns.Add("DIRECTOR_ADQUISICIONES", typeof(String));
            Dt_Cabecera.Columns.Add("OFICIALIA_MAYOR", typeof(String));
            Dt_Cabecera.Columns.Add("TESORERO", typeof(String));

            Dt_Cabecera.Rows[0]["DIRECTOR_ADQUISICIONES"] = Dt_Dir.Rows[0]["DIRECTOR_ADQUISICIONES"];
            Dt_Cabecera.Rows[0]["OFICIALIA_MAYOR"] = Dt_Dir.Rows[0]["OFICIALIA_MAYOR"];
            Dt_Cabecera.Rows[0]["TESORERO"] = Dt_Dir.Rows[0]["TESORERO"];

            Renglon = Dt_Cabecera.Rows[0];
            Ds_Reporte.Tables[0].ImportRow(Renglon);

            String Folio = Dt_Cabecera.Rows[0]["FOLIO"].ToString();

            for (int Cont_Elementos = 0; Cont_Elementos < Dt_Detalles.Rows.Count; Cont_Elementos++)
            {
                Renglon = Dt_Detalles.Rows[Cont_Elementos]; //Instanciar renglon e importarlo
                Ds_Reporte.Tables[1].ImportRow(Renglon);
                Ds_Reporte.Tables[1].Rows[Cont_Elementos].SetField("FOLIO", Folio);
            }

            // Ruta donde se encuentra el reporte Crystal
            if (Tesorero)
            {
                Ruta_Reporte_Crystal = "../Rpt/Compras/Rpt_Ope_Com_Orden_Compra_Tes.rpt";
            }
            else
            {
                Ruta_Reporte_Crystal = "../Rpt/Compras/Rpt_Ope_Com_Orden_Compra.rpt";
            }

            // Se crea el nombre del reporte
            String Nombre_Reporte = "Rpt_List_OrdenC_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

            // Se da el nombre del reporte que se va generar
            if (Formato == "PDF")
                Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
            else if (Formato == "Excel")
                Nombre_Reporte_Generar = Nombre_Reporte + ".xls";  // Es el nombre del repote en Excel que se va a generar

            Cls_Reportes Reportes = new Cls_Reportes();
            Reportes.Generar_Reporte(ref Ds_Reporte, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
            Mostrar_Reporte(Nombre_Reporte_Generar, Formato);
        }
        else
        {               
            throw new Exception("Error al Intentar consultar los Directores que autorizarán la Orden de compra ");
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
    protected void Cmb_Cotizadores_SelectedIndexChanged(object sender, EventArgs e)
    {
       //Llenar_Grid_Ordenes_Compra();
       Llenar_Grid_Ordenes_Compra("GENERADA','AUTORIZADA','RECHAZADA", "NO","","","");
    }


   
    protected void Grid_Ordenes_Compra_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Cls_Ope_Com_Orden_Compra_Negocio Negocio = new Cls_Ope_Com_Orden_Compra_Negocio();
            String Estatus = e.Row.Cells[8].Text.Trim();
            String Aux = "";
            //DataRow[] Renglon = ((DataTable)Session[P_Dt_Requisiciones]).Select("FOLIO = '" + Folio + "'");
            ImageButton Boton = (ImageButton)e.Row.FindControl("Btn_Imprimir_Orden_Compra");
            ImageButton Boton_Info = (ImageButton)e.Row.FindControl("Btn_Info");
            if (Estatus == "GENERADA")
            {
                Boton.ImageUrl = "../imagenes/gridview/circle_grey.png";
                Boton.ToolTip = "En espera de asignar reserva";
                //****
                Boton_Info.ImageUrl = "../imagenes/gridview/circle_grey.png";
              
                Boton_Info.ToolTip = "En espera de asignar reserva";
            }
            else if (Estatus == "RECHAZADA" || Estatus == "CANCELADA")
            {
                Aux = e.Row.Cells[2].Text.Replace("OC-","").Trim();
                Negocio.P_No_Orden_Compra = long.Parse(Aux);
                
                Boton.ImageUrl = "../imagenes/gridview/circle_red.png";
                //Boton.ToolTip = Negocio.Consultar_Comentarios_De_Orden_Compra();
                //***************

                Boton_Info.ImageUrl = "../imagenes/gridview/circle_red.png";
                Boton_Info.Visible = true;
                Boton.ImageUrl = "../imagenes/gridview/grid_print.png";
                Boton_Info.ToolTip = Negocio.Consultar_Comentarios_De_Orden_Compra();
                Boton.ToolTip = "Imprimir";

            }
            else 
            {

                Boton.ImageUrl = "../imagenes/gridview/grid_print.png";
                Boton.ToolTip = "Imprimir";
                //*******
                Boton_Info.ImageUrl = "../imagenes/gridview/circle_green.png";
                Boton_Info.Visible = true;
                Boton_Info.ToolTip = "Aceptada";

            }
        }
    }

    protected void Lnk_Reimprimir_Click(object sender, EventArgs e)
    {
        if(Lnk_Reimprimir.Text == "Reimprimir")
        {
            Lnk_Reimprimir.Text = "O.Compra Pendientes";
            Tr_Reimprimir.Visible = true;
            Llenar_Grid_Ordenes_Compra("", "SI",Txt_Busqueda.Text.Trim(),Txt_Fecha_Inicial.Text,Txt_Fecha_Final.Text);
        }
        else
        {
            Lnk_Reimprimir.Text = "Reimprimir";
            Tr_Reimprimir.Visible = false;
            Llenar_Grid_Ordenes_Compra("GENERADA','AUTORIZADA','RECHAZADA", "NO","","","");
        }       
    }
    # endregion

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
            //Botones.Add(Btn_Buscar);

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

}

    
